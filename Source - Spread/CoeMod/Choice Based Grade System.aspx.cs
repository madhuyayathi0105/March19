using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Choice_Based_Grade_System : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    string collegecode = string.Empty;
    string usercode = string.Empty;
    string group_code = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    Hashtable hat = new Hashtable();

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
            group_code = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                year();
                month();
                //bindeducation();
                BindBatch();
                BindDegree();
                BindBranch();
                BindSubjectType();
                BindSubject();
                clear();
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string grouporusercode = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string strgeteleonly = d2.GetFunction("select value from Master_Settings where settings='Elective Subject only allot' and " + grouporusercode + " ");
                Session["electiveonly"] = string.Empty;
                if (strgeteleonly.Trim() == "1")
                {
                    Session["electiveonly"] = "1";
                }
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method_wo_parameter(Master, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }

                }
                ddlResultType.Items.Clear();
                ddlResultType.Items.Add("");
                ddlResultType.Items.Add("From <= Mark && To > Mark");
                ddlResultType.Items.Add("From < Mark && To >= Mark");


            }
        }
        catch
        {
        }
    }

    public void year()
    {
        try
        {
            ds = d2.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void month()
    {
        try
        {
            ddlMonth.Items.Clear();
            ds.Clear();
            string year1 = ddlYear.SelectedValue;
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year1 + "' order by Exam_month desc";
            ds = d2.select_method_wo_parameter(strsql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
                ddlMonth.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
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

    //public void bindeducation()
    //{
    //    try
    //    {
    //        ddledu.Items.Clear();
    //        string collegecode = Session["collegecode"].ToString();
    //        string usercode = Session["usercode"].ToString();
    //        string group_code = Session["group_code"].ToString();
    //        if (group_code.Contains(';'))
    //        {
    //            string[] group_semi = group_code.Split(';');
    //            group_code = group_semi[0].ToString();
    //        }
    //        string query =string.Empty;
    //        if ((group_code.ToString().Trim() != "") && (group_code.Trim() != "0") && (group_code.ToString().Trim() != "-1"))
    //        {
    //            query = "select distinct course.Edu_Level from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_code + "'";
    //        }
    //        else
    //        {
    //            query = "select distinct course.Edu_Level from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
    //        }
    //        ds = d2.select_method_wo_parameter(query, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddledu.DataSource = ds;
    //            ddledu.DataValueField = "Edu_Level";
    //            ddledu.DataTextField = "Edu_Level";
    //            ddledu.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerror.Visible = true;
    //        lblerror.Text = ex.ToString();
    //    }
    //}

    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void BindDegree()
    {
        try
        {
            ddldegree.Items.Clear();
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
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void BindBranch()
    {
        try
        {
            ddlbranch.Items.Clear();
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
            hat.Add("course_id", ddldegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void BindSubjectType()
    {
        try
        {
            ddlsubtype.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(ddlbatch.SelectedValue.ToString()) && !string.IsNullOrEmpty(ddlbranch.SelectedValue.ToString()))
            {
                string qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and  ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' and sy.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and d.degree_code='" + ddlbranch.SelectedValue.ToString() + "' AND ISNULL(M.Attempts,0) <= 1 order by ss.subject_type";
                if (ddltype.SelectedItem.ToString() == "Arrear")
                {
                    qeryss = "SELECT distinct ss.subject_type FROM Exam_Details ED,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and  ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' and sy.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and d.degree_code='" + ddlbranch.SelectedValue.ToString() + "' AND ISNULL(M.Attempts,0) > 1 order by ss.subject_type";
                }
                ds = d2.select_method_wo_parameter(qeryss, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubtype.DataSource = ds;
                    ddlsubtype.DataTextField = "subject_type";
                    ddlsubtype.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void BindSubject()
    {
        try
        {
            ddlsubject.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(ddlbatch.SelectedValue.ToString()) && !string.IsNullOrEmpty(ddlbranch.SelectedValue.ToString()) && !string.IsNullOrEmpty(ddlsubtype.Text.ToString()))
            {
                string qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_no FROM Exam_Details ED,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' and sy.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and d.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and ss.subject_type='" + ddlsubtype.Text.ToString() + "' AND ISNULL(M.Attempts,0) <= 1 order by s.subject_name";
                if (ddltype.SelectedItem.ToString() == "Arrear")
                {
                    qeryss = "SELECT distinct ss.subject_type,s.subject_name,s.subject_no FROM Exam_Details ED,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' and sy.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and d.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and ss.subject_type='" + ddlsubtype.Text.ToString() + "' AND ISNULL(M.Attempts,0)> 1 order by s.subject_name";
                }
                ds = d2.select_method_wo_parameter(qeryss, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubject.DataSource = ds;
                    ddlsubject.DataTextField = "subject_name";
                    ddlsubject.DataValueField = "subject_no";
                    ddlsubject.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void clear()
    {
        FpSpread1.Visible = false;
        lblerror.Visible = false;
        btngenerate.Visible = false;
        btncalculate.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = string.Empty;
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlResultType_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        month();
        BindSubjectType();
        BindSubject();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindSubjectType();
        BindSubject();
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddltype.SelectedItem.Text == "Arrear")
        {
            btncalculate.Visible = true;
        }
        else
        {
            btncalculate.Visible = true;
        }
        clear();
        BindDegree();
        BindBranch();
        BindSubjectType();
        BindSubject();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindDegree();
        BindBranch();
        BindSubjectType();
        BindSubject();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindBranch();
        BindSubjectType();
        BindSubject();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindSubjectType();
        BindSubject();
    }

    //protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    clear();
    //    BindDegree();
    //    BindBranch();
    //    BindSubjectType();
    //    BindSubject();
    //}

    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindSubject();
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void Btngo_Click(object sender, EventArgs e)
    {
        loadsubdetails();
    }

    public void loadsubdetails()
    {
        try
        {
            clear();
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].ColumnCount = 15;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[1].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
            FpSpread1.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[2].Width = 150;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Code";
            FpSpread1.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[3].Width = 150;
            FpSpread1.Sheets[0].Columns[3].Visible = false;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Appeared";
            FpSpread1.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[4].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Max Mark";
            FpSpread1.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[5].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Min Mark";
            FpSpread1.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[6].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Mean Value";
            FpSpread1.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[7].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Standard Division";
            FpSpread1.Sheets[0].Columns[8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[8].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "K Value";
            FpSpread1.Sheets[0].Columns[9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[9].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "O Grade";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = "O";
            FpSpread1.Sheets[0].Columns[10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[10].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "A+ Grade";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = "A+";
            FpSpread1.Sheets[0].Columns[11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[11].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "A Grade";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Tag = "A";
            FpSpread1.Sheets[0].Columns[12].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[12].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "B+ Grade";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Tag = "B+";
            FpSpread1.Sheets[0].Columns[13].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[13].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "B Grade";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Tag = "B";
            FpSpread1.Sheets[0].Columns[14].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[14].Width = 100;
            if (chkviewgrade.Checked == true)
            {
                FpSpread1.Sheets[0].Columns[10].Visible = true;
                FpSpread1.Sheets[0].Columns[11].Visible = true;
                FpSpread1.Sheets[0].Columns[12].Visible = true;
                FpSpread1.Sheets[0].Columns[13].Visible = true;
                FpSpread1.Sheets[0].Columns[14].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[14].Visible = false;
                FpSpread1.Sheets[0].Columns[10].Visible = false;
                FpSpread1.Sheets[0].Columns[11].Visible = false;
                FpSpread1.Sheets[0].Columns[12].Visible = false;
                FpSpread1.Sheets[0].Columns[13].Visible = false;
            }
            for (int c = 0; c < FpSpread1.Sheets[0].ColumnCount; c++)
            {
                if (c != 1)
                {
                    FpSpread1.Sheets[0].Columns[c].Locked = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[c].Locked = false;
                }
            }
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.Visible = true;
            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            int srno = 0;
            string strquery = "SELECT distinct ISNULL(ss.ElectivePap,'0') as ElectivePap,s.subject_name,ss.lab,s.subject_no,ISNULL(s.Elective,'0') as Elective  FROM Exam_Details ED,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and  ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' and sy.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and d.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and ss.subject_type='" + ddlsubtype.Text.ToString() + "' order by s.subject_name";
            strquery = strquery + " SELECT distinct ss.subject_type,s.subject_name,ss.lab,s.subject_no  FROM Exam_Details ED,mark_entry m,subject s,syllabus_master sy,sub_sem ss,Degree d,Course c where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and sy.syll_code=s.syll_code and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and d.Degree_Code=ed.degree_code and d.Course_Id=c.Course_Id and  ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' and sy.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and d.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and ss.subject_type='" + ddlsubtype.Text.ToString() + "' and isnull(m.grade,'')<>''";
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "text");
            string strinsquery = "select *,(ExamYear*12+ExamMonth) exmonval from SubWiseMeanValue where (ExamYear*12+ExamMonth)=('" + ddlYear.SelectedValue.ToString() + "'*12+'" + ddlMonth.SelectedValue.ToString() + "') order by exmonval desc";
            if (ddltype.SelectedItem.ToString() == "Arrear")
            {
                strinsquery = "select *,(ExamYear*12+ExamMonth) exmonval  from SubWiseMeanValue where (ExamYear*12+ExamMonth)<('" + ddlYear.SelectedValue.ToString() + "'*12+'" + ddlMonth.SelectedValue.ToString() + "') order by exmonval desc";
            }
            DataSet dsval = d2.select_method_wo_parameter(strinsquery, "text");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                srno++;
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.Black;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(ds.Tables[0].Rows[i]["Elective"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = ds.Tables[0].Rows[i]["subject_no"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = ds.Tables[0].Rows[i]["Lab"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(ds.Tables[0].Rows[i]["ElectivePap"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                string equalsub = string.Empty;
                string subno = ds.Tables[0].Rows[i]["subject_no"].ToString();
                string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + subno + "')";
                DataSet dsequlsub = d2.select_method_wo_parameter(strsuboquery, "text");
                for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                {
                    string getsubno = dsequlsub.Tables[0].Rows[es]["subject_no"].ToString();
                    if (equalsub.Trim() != "")
                    {
                        equalsub = equalsub + "," + getsubno;
                    }
                    else
                    {
                        equalsub = getsubno;
                    }
                }
                if (equalsub.Trim() == "")
                {
                    equalsub = subno;
                }
                string subname = ds.Tables[0].Rows[i]["subject_no"].ToString();
                dsval.Tables[0].DefaultView.RowFilter = "SubjectCode in(" + equalsub + ")";
                DataView dvsubgrademaster = dsval.Tables[0].DefaultView;
                if (ddltype.SelectedItem.ToString() == "Arrear")
                {
                    if (dvsubgrademaster.Count == 0)
                    {
                        dsval.Tables[0].DefaultView.RowFilter = "SubjectName='" + ds.Tables[0].Rows[i]["subject_name"].ToString() + "' and SubjectCode in(" + equalsub + ")";
                        dvsubgrademaster = dsval.Tables[0].DefaultView;
                    }
                }
                if (dvsubgrademaster.Count > 0)
                {
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.Blue;
                    string stucount = dvsubgrademaster[0]["TotAppear"].ToString();
                    string maxmark = dvsubgrademaster[0]["IndMaxMark"].ToString();
                    string minmark = dvsubgrademaster[0]["IndMinMark"].ToString();
                    string meanvalue = dvsubgrademaster[0]["MeanValue"].ToString();
                    string sdvalue = dvsubgrademaster[0]["SDValue"].ToString();
                    string kvalue = dvsubgrademaster[0]["KValue"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = stucount;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = maxmark;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = minmark;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = meanvalue;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = sdvalue;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = kvalue;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    ds.Tables[1].DefaultView.RowFilter = "subject_no='" + subname + "'";
                    DataView dvgen = ds.Tables[1].DefaultView;
                    if (dvgen.Count > 0)
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightPink;
                    }
                }
                if (chkviewgrade.Checked == true)
                {
                    strquery = "select * from SubWiseGrdeMaster where SubjectCode in(" + equalsub + ") and Exam_Year='" + ddlYear.SelectedValue.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and Subject_Type='" + ddlsubtype.SelectedItem.ToString() + "'  order by frange desc";//and IsTheory='" + ds.Tables[0].Rows[i]["Lab"].ToString() + "'
                    DataSet dsgradetais = d2.select_method_wo_parameter(strquery, "text");
                    if (ddltype.SelectedItem.ToString() == "Arrear")
                    {
                        if (dsgradetais.Tables[0].Rows.Count == 0)
                        {
                            strquery = "select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where  SubjectName='" + ds.Tables[0].Rows[i]["subject_name"].ToString() + "' and SubjectCode in(" + equalsub + ") and (Exam_Year*12+Exam_Month)<('" + ddlYear.SelectedValue.ToString() + "'*12+'" + ddlMonth.SelectedValue.ToString() + "') order by exmonval desc";
                            dsgradetais.Dispose();
                            dsgradetais.Reset();
                            dsgradetais = d2.select_method_wo_parameter(strquery, "text");

                        }
                    }
                    for (int c = 10; c < 15; c++)
                    {
                        string gradeset = FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Tag.ToString();
                        dsgradetais.Tables[0].DefaultView.RowFilter = "Grade='" + gradeset + "'";
                        DataView dvgradedetails = dsgradetais.Tables[0].DefaultView;
                        if (dvgradedetails.Count > 0)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = dvgradedetails[0]["trange"].ToString() + " - " + dvgradedetails[0]["frange"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                btngenerate.Visible = true;
                if (ddltype.SelectedItem.Text == "Arrear")
                    btncalculate.Visible = false;
                else
                    btncalculate.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btncalculate_Click(object sender, EventArgs e)
    {
        try
        {
            bool valfla = false;
            FpSpread1.SaveChanges();
            for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 1].Value);
                if (isval == 1)
                {
                    valfla = true;
                    string strsubname = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();
                    string subcode = strsubname;
                    string lab = FpSpread1.Sheets[0].Cells[r, 2].Note.ToString();
                    string subno = FpSpread1.Sheets[0].Cells[r, 2].Tag.ToString();
                    string ElectSub = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Note).Trim();
                    string Electpep = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 0].Note).Trim();

                    subcode = subno;
                    string equalsub = string.Empty;
                    string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + subno + "')";//tbl_equal_paper_Matching
                    DataSet dsequlsub = d2.select_method_wo_parameter(strsuboquery, "text");
                    for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                    {
                        string getsubno = dsequlsub.Tables[0].Rows[es]["subject_no"].ToString();
                        if (equalsub.Trim() != "")
                        {
                            equalsub = equalsub + ",'" + getsubno + "'";
                        }
                        else
                        {
                            equalsub = "'" + getsubno + "'";
                        }
                    }
                    if (equalsub.Trim() == "")
                    {
                        equalsub = "'" + subno + "'";
                    }
                    //round modify jairam
                    string strquery = "select count(m.roll_no) Studcount,sum(total) totalmarks,max(total) maxmark,min(total) minmark,(sum(total)/count(m.roll_no)) meanvalue from mark_entry m,Exam_Details ed,subject s,registration r where r.roll_no=m.roll_no and m.exam_code=ed.exam_code and m.subject_no=s.subject_no and m.internal_mark>=0 and m.external_mark>=0 and m.result in('Pass','Fail','RA','MC','SA') and ed.Exam_year='" + ddlYear.Text.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_no in(" + equalsub + ") and r.delflag<>1";

                    strquery = strquery + "  select m.total,m.roll_no,ss.lab,r.Reg_No from mark_entry m,Exam_Details ed,subject s,sub_sem ss,registration r where r.roll_no=m.roll_no and m.exam_code=ed.exam_code and m.subject_no=s.subject_no and m.internal_mark>=0 and m.external_mark>=0 and m.total>0 and m.result in('Pass','Fail','RA','MC','SA') and ss.subtype_no=s.subtype_no and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' and ed.Exam_year='" + ddlYear.Text.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_no in(" + equalsub + ") and r.delflag<>1  order by r.Reg_No";
                  //  strquery = strquery + "  select m.total,m.roll_no,ss.lab from mark_entry m,Exam_Details ed,subject s,sub_sem ss,registration r where r.roll_no=m.roll_no and m.exam_code=ed.exam_code and m.subject_no=s.subject_no and m.internal_mark>=0 and m.external_mark>=0 and m.total>0 and m.result in('Pass','Fail','RA','MC','SA') and ss.subtype_no=s.subtype_no and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' and ed.Exam_year='" + ddlYear.Text.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_no in(" + equalsub + ") and r.delflag<>1  order by m.roll_no";

                    //discontinue CASE================================
                    strquery = strquery + "  select sum(total) totalmarks from mark_entry m,Exam_Details ed,subject s where m.exam_code=ed.exam_code and m.subject_no=s.subject_no and m.internal_mark>=0 and m.external_mark>=0 and m.result in('Pass','Fail','RA','MC','SA') and ed.Exam_year='" + ddlYear.Text.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_no in(" + equalsub + ")";
                   //=====================================================

                    ds.Dispose();
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        string stucount = ds.Tables[0].Rows[0]["Studcount"].ToString();
                        string maxmark = ds.Tables[0].Rows[0]["maxmark"].ToString();
                        string minmark = ds.Tables[0].Rows[0]["minmark"].ToString();

                        //string totmark = ds.Tables[0].Rows[0]["totalmarks"].ToString();//without discontinue
                        string totmark = ds.Tables[2].Rows[0]["totalmarks"].ToString();
                        double totmarkNew = 0;
                        double totStu = 0;
                        double.TryParse(stucount, out totStu);
                        double.TryParse(totmark, out totmarkNew);

                        ///string meanvalue = ds.Tables[0].Rows[0]["meanvalue"].ToString();

                        string meanvalue = Convert.ToString(totmarkNew / totStu);

                        if (lab.Trim().ToLower() == "true" || lab.Trim() == "1")
                        {
                            lab = "1";
                        }
                        else
                        {
                            lab = "0";
                        }

                        if (!string.IsNullOrEmpty(ElectSub))
                        {
                            if (ElectSub == "1" || ElectSub.ToLower() == "true" || Electpep == "1" || Electpep.ToLower() == "true" && !string.IsNullOrEmpty(stucount))
                            {
                                if (Convert.ToInt32(stucount) < 30)
                                {
                                    lab = "1";
                                }
                            }
                        }
                        double meanvalues = 0; double kvalue = 0;
                        if (meanvalue.Trim() != "" && meanvalue.Trim() != "0")
                        {
                            double sdvalue = 0;
                            meanvalues = Convert.ToDouble(meanvalue);
                            meanvalues = Math.Round(meanvalues, 2, MidpointRounding.AwayFromZero);
                            double mean1 = meanvalues;
                            kvalue = (Convert.ToDouble(maxmark) - 50) / 5;
                            kvalue = Math.Round(kvalue, 2, MidpointRounding.AwayFromZero);
                            double getstumen = 0;
                            for (int s = 0; s < ds.Tables[1].Rows.Count; s++)
                            {
                                string studenmaek = ds.Tables[1].Rows[s]["total"].ToString();
                                double stumark = Convert.ToDouble(studenmaek);
                                getstumen =  Convert.ToDouble(stumark)-Convert.ToDouble(mean1) ;
                                getstumen = Math.Round(getstumen, 2, MidpointRounding.AwayFromZero);
                              
                               getstumen = getstumen * getstumen;
                               getstumen = Math.Round(getstumen, 2, MidpointRounding.AwayFromZero);
                              
                               sdvalue = sdvalue + getstumen;
                            }
                            sdvalue = sdvalue / Convert.ToDouble(stucount);
                            sdvalue = Math.Round(sdvalue, 2, MidpointRounding.AwayFromZero);
                            sdvalue = Math.Sqrt(sdvalue);
                            sdvalue = Math.Round(sdvalue, 2, MidpointRounding.AwayFromZero);
                            string strinsquery = "Delete from SubWiseMeanValue where SubjectCode in (" + equalsub + ") and ExamYear='" + ddlYear.SelectedValue.ToString() + "' and ExamMonth='" + ddlMonth.SelectedValue.ToString() + "'";
                            strinsquery = strinsquery + " Delete from SubWiseGrdeMaster where SubjectCode in (" + equalsub + ") and Exam_Year='" + ddlYear.SelectedValue.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and Subject_Type='" + ddlsubtype.SelectedItem.ToString() + "' and IsTheory='" + lab + "'";
                            int insva = d2.update_method_wo_parameter(strinsquery, "text");
                            strinsquery = " insert into SubWiseMeanValue(SubjectCode,TotAppear,IndMaxMark,IndMinMark,MeanValue,SDValue,KValue,ExamYear,ExamMonth,College_Code,SubjectName)";
                            strinsquery = strinsquery + " values('" + subcode + "','" + stucount + "','" + maxmark + "','" + minmark + "','" + meanvalues.ToString() + "','" + sdvalue + "','" + kvalue + "','" + ddlYear.SelectedValue.ToString() + "','" + ddlMonth.SelectedValue.ToString() + "','" + collegecode + "','" + strsubname + "')";
                            insva = d2.update_method_wo_parameter(strinsquery, "text");
                            double ogrdae = meanvalues + (1.65 * sdvalue);
                            double Xvalue = Convert.ToDouble(maxmark);

                            if (lab == "1")
                            {
                                ogrdae = Xvalue - kvalue;
                            }
                            ogrdae = Math.Round(ogrdae, 2, MidpointRounding.AwayFromZero);
                            strinsquery = "insert into SubWiseGrdeMaster(SubjectCode,Subject_Type,IsTheory,Grade,Frange,TRange,College_Code,Exam_Year,Exam_Month,SubjectName)";
                            if (ogrdae < 100)
                            {
                                strinsquery = strinsquery + " values('" + subcode + "','" + ddlsubtype.SelectedItem.ToString() + "','" + lab + "','O','" + ogrdae + "','100','" + collegecode + "','" + ddlYear.SelectedItem.ToString() + "','" + ddlMonth.SelectedValue.ToString() + "','" + strsubname + "')";
                            }
                            else
                            {
                                strinsquery = strinsquery + " values('" + subcode + "','" + ddlsubtype.SelectedItem.ToString() + "','" + lab + "','O','" + ogrdae + "','" + ogrdae + "','" + collegecode + "','" + ddlYear.SelectedItem.ToString() + "','" + ddlMonth.SelectedValue.ToString() + "','" + strsubname + "')";
                            }
                            insva = d2.update_method_wo_parameter(strinsquery, "text");
                            double Aplusgrdae = meanvalues + (0.85 * sdvalue);
                            if (lab == "1")
                            {
                                Aplusgrdae = Xvalue - (2 * kvalue);
                            }
                            Aplusgrdae = Math.Round(Aplusgrdae, 2, MidpointRounding.AwayFromZero);
                            strinsquery = "insert into SubWiseGrdeMaster(SubjectCode,Subject_Type,IsTheory,Grade,Frange,TRange,College_Code,Exam_Year,Exam_Month,SubjectName)";
                            strinsquery = strinsquery + " values('" + subcode + "','" + ddlsubtype.SelectedItem.ToString() + "','" + lab + "','A+','" + Aplusgrdae + "','" + ogrdae + "','" + collegecode + "','" + ddlYear.SelectedItem.ToString() + "','" + ddlMonth.SelectedValue.ToString() + "','" + strsubname + "')";
                            insva = d2.update_method_wo_parameter(strinsquery, "text");
                            double agrdae = meanvalues;
                            if (lab == "1")
                            {
                                agrdae = Xvalue - (3 * kvalue);
                            }
                            agrdae = Math.Round(agrdae, 2, MidpointRounding.AwayFromZero);
                            strinsquery = "insert into SubWiseGrdeMaster(SubjectCode,Subject_Type,IsTheory,Grade,Frange,TRange,College_Code,Exam_Year,Exam_Month,SubjectName)";
                            strinsquery = strinsquery + " values('" + subcode + "','" + ddlsubtype.SelectedItem.ToString() + "','" + lab + "','A','" + agrdae + "','" + Aplusgrdae + "','" + collegecode + "','" + ddlYear.SelectedItem.ToString() + "','" + ddlMonth.SelectedValue.ToString() + "','" + strsubname + "')";
                            insva = d2.update_method_wo_parameter(strinsquery, "text");
                            double bplusgrdae = meanvalues - (0.9 * sdvalue);
                            if (lab == "1")
                            {
                                bplusgrdae = Xvalue - (4 * kvalue);
                            }
                            bplusgrdae = Math.Round(bplusgrdae, 2, MidpointRounding.AwayFromZero);
                            strinsquery = "insert into SubWiseGrdeMaster(SubjectCode,Subject_Type,IsTheory,Grade,Frange,TRange,College_Code,Exam_Year,Exam_Month,SubjectName)";
                            strinsquery = strinsquery + " values('" + subcode + "','" + ddlsubtype.SelectedItem.ToString() + "','" + lab + "','B+','" + bplusgrdae + "','" + agrdae + "','" + collegecode + "','" + ddlYear.SelectedItem.ToString() + "','" + ddlMonth.SelectedValue.ToString() + "' ,'" + strsubname + "')";
                            insva = d2.update_method_wo_parameter(strinsquery, "text");
                            double bgrdae = meanvalues - (1.8 * sdvalue);
                            if (lab == "1")
                            {
                                bgrdae = Xvalue - (5 * kvalue);
                            }
                            bgrdae = Math.Round(bgrdae, 2, MidpointRounding.AwayFromZero);
                            strinsquery = "insert into SubWiseGrdeMaster(SubjectCode,Subject_Type,IsTheory,Grade,Frange,TRange,College_Code,Exam_Year,Exam_Month,SubjectName)";
                            strinsquery = strinsquery + " values('" + subcode + "','" + ddlsubtype.SelectedItem.ToString() + "','" + lab + "','B','" + bgrdae + "','" + bplusgrdae + "','" + collegecode + "','" + ddlYear.SelectedItem.ToString() + "','" + ddlMonth.SelectedValue.ToString() + "','" + strsubname + "')";
                            insva = d2.update_method_wo_parameter(strinsquery, "text");
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Please Check The Mark Details";
                            return;
                        }
                    }
                }
            }
            if (valfla == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Subject And Then Proceed";
            }
            else
            {
                loadsubdetails();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Calculated Successfully!')", true);
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btngenerate_Click(object sender, EventArgs e)
    {
        try
        {
            int insupdval = 0;
            string insupdatequery = string.Empty;
            string grade = string.Empty;
            string stumark = string.Empty;
            string rollno = string.Empty;
            string examcode = string.Empty;
            string actgrade = string.Empty;
            string subno = string.Empty;
            string strsubname = string.Empty;
            string subcode = string.Empty;
            string lab = string.Empty;
            string esemark = string.Empty;
            bool valfla = false;
            string Electsub = string.Empty;
            string Electsubsem = string.Empty;
            FpSpread1.SaveChanges();
            for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, 1].Value);
                if (isval == 1)
                {
                    valfla = true;
                    strsubname = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();
                    subcode = strsubname;
                    lab = FpSpread1.Sheets[0].Cells[r, 2].Note.ToString();
                    subno = FpSpread1.Sheets[0].Cells[r, 2].Tag.ToString();
                    Electsub = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Note).Trim();
                    Electsubsem = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 0].Note).Trim();
                    subcode = subno;
                    string equalsub = string.Empty;
                    string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + subno + "')";
                    DataSet dsequlsub = d2.select_method_wo_parameter(strsuboquery, "text");
                    for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                    {
                        string getsubno = dsequlsub.Tables[0].Rows[es]["subject_no"].ToString();
                        if (equalsub.Trim() != "")
                        {
                            equalsub = equalsub + ",'" + getsubno + "'";
                        }
                        else
                        {
                            equalsub = "'" + getsubno + "'";
                        }
                    }
                    if (equalsub.Trim() == "")
                    {
                        equalsub = "'" + subno + "'";
                    }
                    double minextmark = 0;
                    String strquery = " select isnull(m.total,'0') total,m.roll_no,ed.exam_code,m.subject_no,s.max_ext_marks,s.min_ext_marks,s.mintotal,s.maxtotal,m.grade,isnull(m.external_mark,'0') ese,m.result,m.attempts from mark_entry m,Exam_Details ed,subject s where m.exam_code=ed.exam_code and m.subject_no=s.subject_no and ed.Exam_year='" + ddlYear.Text.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_no in(" + equalsub + ")";
                    strquery = strquery + " select * from SubWiseGrdeMaster where SubjectCode in(" + equalsub + ")  and Exam_Year='" + ddlYear.SelectedValue.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' order by Frange desc";
                    strquery = strquery + " select * from SubWiseMeanValue where SubjectCode in(" + equalsub + ") and ExamYear='" + ddlYear.SelectedValue.ToString() + "' and ExamMonth='" + ddlMonth.SelectedValue.ToString() + "'";
                    ds.Dispose();
                    ds = d2.select_method_wo_parameter(strquery, "Text");

                    if (ddltype.SelectedItem.ToString().Trim().ToLower() == "arrear")
                    {
                        //if (ds.Tables[1].Rows.Count == 0)
                        //{
                            strquery = " select isnull(m.total,'0') total,m.roll_no,ed.exam_code,m.subject_no,s.max_ext_marks,s.min_ext_marks,s.mintotal,s.maxtotal,m.grade,isnull(m.external_mark,'0') ese,m.result,m.attempts from mark_entry m,Exam_Details ed,subject s where m.exam_code=ed.exam_code and m.subject_no=s.subject_no and ed.Exam_year='" + ddlYear.Text.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_no in(" + equalsub + ")";
                            strquery = strquery + " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + FpSpread1.Sheets[0].Cells[r, 2].Text.ToString() + "' and SubjectCode in(" + equalsub + ") and (Exam_Year*12+Exam_Month)<('" + ddlYear.SelectedValue.ToString() + "'*12+'" + ddlMonth.SelectedValue.ToString() + "') order by exmonval desc,Frange desc";
                            strquery = strquery + " select *,(ExamYear*12+ExamMonth) exmonval from SubWiseMeanValue where SubjectName='" + FpSpread1.Sheets[0].Cells[r, 2].Text.ToString() + "' and SubjectCode in(" + equalsub + ") and (ExamYear*12+ExamMonth)<('" + ddlYear.SelectedValue.ToString() + "'*12+'" + ddlMonth.SelectedValue.ToString() + "') order by exmonval desc";
                            ds.Dispose();
                            ds = d2.select_method_wo_parameter(strquery, "Text");
                       // }
                    }
                    //Rajkumar
                    double MaxExternalVal = 0;
                    double MaxPassMark = 0;
                    double NewMeanval = 0;
                    int totStudent = 0;
                    ds.Tables[1].DefaultView.RowFilter = "grade='O'";
                    DataView dvOgrade = ds.Tables[1].DefaultView;
                    Double ograde = 0;
                    if (dvOgrade.Count > 0)
                    {
                         ograde = Convert.ToDouble(dvOgrade[0]["Trange"].ToString());
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            string MaxNew = Convert.ToString(ds.Tables[2].Rows[0]["MeanValue"]);
                            totStudent = Convert.ToInt32(ds.Tables[2].Rows[0]["TotAppear"]);
                            if (!string.IsNullOrEmpty(MaxNew))
                            {
                                string MaxNewVal = Convert.ToString(ds.Tables[0].Rows[0]["max_ext_marks"]);
                                string maxIndMark = Convert.ToString(ds.Tables[2].Rows[0]["IndMaxMark"]);
                                double.TryParse(MaxNewVal, out MaxExternalVal);
                                double.TryParse(maxIndMark, out MaxPassMark);
                                double MeanVal = Convert.ToDouble(MaxNew);
                                double NewMeanAvgVal = Convert.ToDouble(MeanVal / 100);//MaxExternalVal
                                NewMeanval = NewMeanAvgVal * 60;
                            }
                        }
                    }
                    //Rajkumar 2/1/2018========
                    bool ELECTIVE = false;
                    if (Electsub == "1" || Electsub.ToLower() == "true" || Electsubsem == "1" || Electsubsem.ToLower() == "true" && totStudent > 0)
                    {
                        if (totStudent < 30)
                        {
                            lab = "1";
                            ELECTIVE = true;
                        }
                    }
                    //raj----------------

                    double mintotal = 0;
                    double MaxExternal = 0;
                    double MinExternal = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string MaxNew = Convert.ToString(ds.Tables[0].Rows[0]["max_ext_marks"]);
                        double.TryParse(MaxNew, out MaxExternal);
                        //rajkumar 4/1/2018 elective less then 30
                        string minexNew = Convert.ToString(ds.Tables[0].Rows[0]["min_ext_marks"]);
                        double.TryParse(minexNew, out MinExternal);
                        //
                        ds.Tables[1].DefaultView.RowFilter = "grade='B'";
                        DataView dvgrade = ds.Tables[1].DefaultView;
                        if (dvgrade.Count > 0)
                        {
                            if (ddltype.SelectedItem.ToString().Trim().ToLower() == "arrear")
                            {
                                minextmark = Convert.ToDouble(dvgrade[0]["Frange"].ToString());

                                if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                                {
                                    if (MaxExternal != 0) // Added by jairam 08-07-2017
                                    {
                                        minextmark = (minextmark * MaxExternal) / 100;
                                    }
                                    else
                                    {
                                        minextmark = (minextmark * 50) / 100;
                                    }
                                }
                                else
                                {
                                    minextmark = (minextmark * 60) / 100;
                                }
                                mintotal = Convert.ToDouble(dvgrade[0]["Frange"].ToString());

                            }
                            else
                            {
                                minextmark = Convert.ToDouble(dvgrade[0]["frange"].ToString());
                                if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                                {
                                    if (MaxExternal != 0) // Added by jairam 08-07-2017
                                    {
                                        minextmark = (minextmark * MaxExternal) / 100;
                                    }
                                    else
                                    {
                                        minextmark = (minextmark * 50) / 100;
                                    }
                                }
                                else
                                {
                                    minextmark = (minextmark * 60) / 100;
                                }
                                mintotal = Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            }
                        }
                        if (mintotal > 50)
                        {
                            mintotal = 50;
                        }
                        for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                        {
                            stumark = ds.Tables[0].Rows[s]["total"].ToString();
                            rollno = ds.Tables[0].Rows[s]["roll_no"].ToString();
                            examcode = ds.Tables[0].Rows[s]["exam_code"].ToString();//	150079  150043
                            subno = ds.Tables[0].Rows[s]["subject_no"].ToString();
                            actgrade = ds.Tables[0].Rows[s]["grade"].ToString();
                            esemark = ds.Tables[0].Rows[s]["ese"].ToString();
                            string Attempts = Convert.ToString(ds.Tables[0].Rows[s]["attempts"]);
                            string esemax = ds.Tables[0].Rows[s]["max_ext_marks"].ToString();
                            string maxtot = ds.Tables[0].Rows[s]["maxtotal"].ToString();
                            grade = "RA";
                            string presult = ds.Tables[0].Rows[s]["result"].ToString();
                            string result = "Fail";
                            int passorfail = 0;
                            double stumarkval = Convert.ToDouble(stumark) / Convert.ToDouble(maxtot) * 100;
                            bool failgrade = false;
                            double stuese = Convert.ToDouble(esemark) / Convert.ToDouble(esemax) * 100;
                            double minextMark1 = 0;
                            double maxextMark = 0;
                            double.TryParse(esemax.Trim(), out maxextMark);
                            //minextMark=minextmark
                            minextMark1 = (minextmark * maxextMark) / 100;
                            stuese = Math.Round(stuese, 2, MidpointRounding.AwayFromZero);
                            //rajkumar

                            if (ELECTIVE == false && lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                            {
                                if (MaxExternal != 0)
                                {
                                    if (stumarkval < MaxExternal || Convert.ToDouble(esemark) < minextmark)//lab/Doubt  MaxExternal
                                    {
                                        failgrade = true;
                                    }
                                }
                                else
                                {
                                    if (stumarkval < 50 || Convert.ToDouble(esemark) < minextmark)
                                    {
                                        failgrade = true;
                                    }
                                }
                            }
                            //rajkumar 2/1/2018 -------------
                            else if (ELECTIVE == true && lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                            {

                                if (mintotal != 0)
                                {
                                    if (stumarkval < mintotal || Convert.ToDouble(esemark) < minextmark)
                                    {
                                        failgrade = true;
                                    }
                                }
                                else
                                {
                                    if (stumarkval < 50 || Convert.ToDouble(esemark) < minextmark)//minextmark
                                    {
                                        failgrade = true;
                                    }
                                }
                            }
                            //-----------------
                            else
                            {

                                if (stumarkval < mintotal || Convert.ToDouble(esemark) < minextMark1)//theory--minextMark1
                                {
                                    failgrade = true;
                                }
                            }
                            
                           
                            if (ddlResultType.SelectedIndex == 0 || ddlResultType.SelectedIndex == 1)
                            {

                                if (failgrade == false)
                                {
                                    if (ddltype.SelectedItem.ToString().Trim().ToLower() == "arrear")
                                    {
                                        if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Frange<='" + stumark + "' and Trange >'" + stumark + "'";
                                        }
                                        else
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Frange<='" + stumark + "' and Trange >'" + stumark + "'";
                                        }
                                    }
                                    else
                                    {
                                        if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Frange<='" + stumark + "' and Trange >'" + stumark + "'";
                                        }
                                        else
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Frange<='" + stumark + "' and Trange >'" + stumark + "'";
                                        }
                                    }
                                }
                            }

                            else
                            {
                                if (failgrade == false)
                                {
                                    if (ddltype.SelectedItem.ToString().Trim().ToLower() == "arrear")
                                    {
                                        if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Frange<='" + stumark + "' and Trange >'" + stumark + "'";
                                        }
                                        else
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Frange<='" + stumark + "' and Trange >'" + stumark + "'";
                                        }
                                    }
                                    else
                                    {
                                        if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Frange<'" + stumark + "' and Trange >='" + stumark + "'";
                                        }
                                        else
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Frange<'" + stumark + "' and Trange >='" + stumark + "'";
                                        }
                                    }
                                }
                            }
                            dvgrade = ds.Tables[1].DefaultView;
                            dvgrade.Sort = "Exam_Year desc,Frange asc";
                            double omark = 0;
                            double.TryParse(stumark, out omark);
                            if (dvgrade.Count > 0)
                            {
                                grade = dvgrade[0]["Grade"].ToString();
                                result = "Pass";
                                passorfail = 1;
                            }
                            else if (ograde == omark)
                            {
                                grade = "O";
                                result = "Pass";
                                passorfail = 1;
                            }
                            else
                            {
                                grade = "B";
                                result = "Pass";
                                passorfail = 1;
                            }

                            //Rajkumar 31/12/2017
                            //if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                            //{
                            if (failgrade == true)
                            {
                                grade = "RA";
                                result = "Fail";
                            }
                            //}

                            if (Convert.ToInt16(Attempts) >= 3)//Attempts
                            {
                                if (failgrade)
                                {
                                    if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = "Frange<='" + stuese + "' and Trange >'" + stuese + "'";//Lab 50% ESE Pass
                                        dvgrade = ds.Tables[1].DefaultView;
                                        dvgrade.Sort = "Frange asc";
                                        if (dvgrade.Count > 0)
                                        {
                                            grade = "B";//dvgrade[0]["Grade"].ToString();
                                            result = "Pass";
                                            passorfail = 1;
                                        }
                                    }
                                }

                            }


                            if (lab != "1" && lab.ToLower().Trim() != "true")//&& ddltype.SelectedItem.ToString().Trim().ToLower() != "arrear" Rajkumar on 10-08-2018
                            {
                                if (Convert.ToInt16(Attempts) < 3)//Attempts
                                {
                                    if (stuese < NewMeanval)
                                    {
                                        grade = "RA";
                                        result = "Fail";
                                    }
                                }
                                else
                                {
                                    if (stuese < NewMeanval)
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = "Frange<='" + stuese + "' and Trange >'" + stuese + "'";
                                        dvgrade = ds.Tables[1].DefaultView;
                                        dvgrade.Sort = "Exam_Year desc,Frange asc";
                                        if (dvgrade.Count > 0)
                                        {
                                            grade = dvgrade[0]["Grade"].ToString();
                                            result = "Pass";
                                            passorfail = 1;
                                        }
                                        else
                                        {
                                            grade = "RA";
                                            result = "Fail";
                                        }
                                    }
                                    else //Mean value pass but min ESE fail CASE fro 3ed attempts
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = "Frange<='" + stuese + "' and Trange >'" + stuese + "'";
                                        dvgrade = ds.Tables[1].DefaultView;
                                        dvgrade.Sort = "Exam_Year desc,Frange asc";
                                        if (dvgrade.Count > 0)
                                        {
                                            grade = dvgrade[0]["Grade"].ToString();
                                            result = "Pass";
                                            passorfail = 1;
                                        }
                                        else
                                        {
                                            grade = "RA";
                                            result = "Fail";
                                        }
                                    }
                                }
                            }
                            //Rajkmar 

                            if (esemark.Trim() == "-1")
                            {
                                grade = "AAA";
                                result = "AAA";
                            }
                            else if (esemark.Trim() == "-4")
                            {
                                grade = "RA";
                                result = "WHD";
                            }
                            
                            if (string.Equals(presult.Trim().ToLower(), "sa"))
                            {
                                result = "-";
                                grade = "SA";
                            }
                            if (presult.Trim().ToLower().Contains("aa"))
                            {
                                grade = "AAA";
                                result = "AAA";
                            }
                            else if (presult.Trim().ToLower().Contains("w"))
                            {
                                grade = "RA";
                                result = "WHD";
                            }
                            else if (presult.Trim().ToLower().Contains("mc"))
                            {
                                grade = "RA";
                                result = "RA";
                            }
                           
                          


                            insupdatequery = "update mark_entry set grade='" + grade + "',Actual_Grade='" + actgrade + "',passorfail='" + passorfail + "',result='" + result + "' where roll_no='" + rollno + "' and exam_code='" + examcode + "' and subject_no='" + subno + "'";
                            insupdval = d2.update_method_wo_parameter(insupdatequery, "text");
                        }
                    }
                }
            }
            if (valfla == false)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The Subject And Then Proceed";
            }
            else
            {
                loadsubdetails();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Generated Successfully!')", true);
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnresult_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].ColumnCount = 9;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread1.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[1].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread1.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[2].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread1.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[3].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Internal Mark";
            FpSpread1.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[4].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "External Mark";
            FpSpread1.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[5].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total";
            FpSpread1.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[6].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Grade";
            FpSpread1.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[7].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Result";
            FpSpread1.Sheets[0].Columns[8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[8].Width = 50;
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;
            FpSpread1.Sheets[0].Columns[5].Locked = true;
            FpSpread1.Sheets[0].Columns[6].Locked = true;
            FpSpread1.Sheets[0].Columns[7].Locked = true;
            FpSpread1.Sheets[0].Columns[8].Locked = true;
            if (Session["Rollflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
            }
            if (Session["Regflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }
            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            int srno = 0;
            string strsubname = ddlsubject.SelectedItem.ToString();
            string subno = ddlsubject.SelectedValue.ToString();
            string equalsub = string.Empty;
            string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + subno + "')";
            DataSet dsequlsub = d2.select_method_wo_parameter(strsuboquery, "text");
            for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
            {
                string getsubno = dsequlsub.Tables[0].Rows[es]["subject_no"].ToString();
                if (equalsub.Trim() != "")
                {
                    equalsub = equalsub + "," + getsubno;
                }
                else
                {
                    equalsub = getsubno;
                }
            }
            if (equalsub.Trim() == "")
            {
                equalsub = subno;
            }
            //String strquery = "  select r.Roll_No,r.Reg_No,r.stud_name,m.total,m.roll_no,ed.exam_code,m.subject_no,m.grade,m.external_mark,m.result,m.internal_mark from mark_entry m,Exam_Details ed,subject s,Registration r  where m.exam_code=ed.exam_code and m.subject_no=s.subject_no and r.Roll_No=m.roll_no and ed.Exam_year='" + ddlYear.Text.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_name='" + strsubname + "' order by r.Reg_No";
            string strquery = "  select r.Roll_No,r.Reg_No,r.stud_name,m.total,m.roll_no,ed.exam_code,m.subject_no,m.grade,m.external_mark,m.result,m.internal_mark from mark_entry m,Exam_Details ed,subject s,Registration r  where m.exam_code=ed.exam_code and m.subject_no=s.subject_no and r.Roll_No=m.roll_no and ed.Exam_year='" + ddlYear.Text.ToString() + "' and ed.Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and s.subject_no in (" + equalsub + ") order by r.Reg_No";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
                FpSpread1.Visible = true;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    srno++;
                    FpSpread1.Sheets[0].RowCount++;
                    string ese = ds.Tables[0].Rows[i]["external_mark"].ToString().Trim();
                    if (ese == "-1")
                    {
                        ese = "AAA";
                    }
                    else if (ese == "-2")
                    {
                        ese = "NE";
                    }
                    else if (ese == "-3")
                    {
                        ese = "NR";
                    }
                    else if (ese == "-4")
                    {
                        ese = "WHD";
                    }
                    string intmaek = ds.Tables[0].Rows[i]["internal_mark"].ToString().Trim();
                    if (intmaek == "-1")
                    {
                        intmaek = "AAA";
                    }
                    else if (intmaek == "-2")
                    {
                        intmaek = "NE";
                    }
                    else if (intmaek == "-3")
                    {
                        intmaek = "NR";
                    }
                    string result = ds.Tables[0].Rows[i]["result"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["stud_name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = intmaek;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ese;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["total"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["grade"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[i]["result"].ToString();
                    if (ese.Trim().ToLower().Contains("aa") || ese.Trim().ToLower().Contains("nr") || ese.Trim().ToLower().Contains("ne") || ese.Trim().ToLower().Contains("whd") || ese.Trim().ToLower() == "" || intmaek.Trim().ToLower() == "" || result.Trim().ToLower().Contains("aa") || result.Trim().ToLower().Contains("w"))
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Red;
                    }
                    if (ds.Tables[0].Rows[i]["result"].ToString().Trim().ToLower() == "pass")
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                    }
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Records Found";
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Choice Based Grade System$" + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedValue.ToString() + "@Subject Code - Name : " + ddlsubject.SelectedValue.ToString() + " - " + ddlsubject.SelectedItem.ToString();
            string pagename = "Choice Based Grade System.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblerror.Visible = false;
            }
            else
            {
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public string sendErrorMail1(string test, string collcode, string pageName)
    {
        string sentMail = "Mail Not Sent";
        try
        {
            string userid = "palpaporange@gmail.com";
            string userpd = "palpap1234";
            string collegeName = d2.GetFunction("select collname from collinfo  where college_code='" + collcode + "'").Trim() + "-" + DateTime.Now;
            System.Net.Mail.SmtpClient Mail = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            System.Net.Mail.MailMessage mailmsg = new System.Net.Mail.MailMessage();
            System.Net.Mail.MailAddress mfrom = new System.Net.Mail.MailAddress(userid);
            mailmsg.From = mfrom;
            mailmsg.To.Add(userid);
            mailmsg.Subject = "Error from Try Catch";
            mailmsg.IsBodyHtml = true;
            mailmsg.Body = test + " " + "<br>" + collegeName + "<br>" + pageName;
            Mail.EnableSsl = true;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(userid, userpd);
            Mail.UseDefaultCredentials = false;
            Mail.Credentials = credentials;
            Mail.Send(mailmsg);
            sentMail = "Mail Sent";
        }
        catch { }
        return sentMail;
    }

}