using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Configuration;

public partial class EqualSubjectChoiceBasedGradeSystem : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
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
            errmsg.Visible = false;
            if (!IsPostBack)
            {
                string group_code = Session["group_code"].ToString();
                string columnfield = "";
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
                ds = d2.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.Enabled = true;
                    ddlcollege.DataSource = ds;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    loadedu();
                    loadbatch();
                    loadsubtype();
                    year();
                    month();
                }
                clear();
            }
        }
        catch (Exception ex) 
        { }
    }

    public void loadedu()
    {
        try
        {
            ddledu.Items.Clear();
            ds = d2.select_method_wo_parameter("select distinct Edu_Level from Course where college_code='" + ddlcollege.SelectedValue.ToString() + "'", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddledu.Enabled = true;
                ddledu.DataSource = ds;
                ddledu.DataTextField = "Edu_Level";
                ddledu.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void loadbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.Enabled = true;
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void loadsubtype()
    {
        try
        {
            ddlsubtype.Items.Clear();
            string strquery = "select distinct ss.subject_type from syllabus_master sy,Degree d,Course c,subject s,sub_sem ss,Department de where sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code";
            strquery = strquery + " and c.edu_level='" + ddledu.SelectedItem.ToString() + "' and d.college_code='" + ddlcollege.SelectedValue.ToString() + "' and sy.batch_year>='" + ddlbatch.SelectedItem.ToString() + "'";
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubtype.Enabled = true;
                ddlsubtype.DataSource = ds;
                ddlsubtype.DataTextField = "subject_type";
                ddlsubtype.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void year()
    {
        try
        {
            ds = d2.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlexamyear.DataSource = ds;
                ddlexamyear.DataTextField = "Exam_year";
                ddlexamyear.DataValueField = "Exam_year";
                ddlexamyear.DataBind();
            }
            ddlexamyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void month()
    {
        try
        {
            ddlmonth.Items.Clear();
            ds.Clear();
            string year1 = ddlexamyear.SelectedValue;
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year1 + "' order by Exam_month desc";
            ds = d2.select_method_wo_parameter(strsql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlmonth.DataSource = ds;
                ddlmonth.DataTextField = "monthName";
                ddlmonth.DataValueField = "Exam_month";
                ddlmonth.DataBind();
                ddlmonth.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void clear()
    {
        try
        {
            FpSpread1.Visible = false;
            FpSpread2.Visible = false;
            btnsave.Visible = false;
            btndelete.Visible = false;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
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
    protected void ddlcollge_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            loadedu();
            loadbatch();
            loadsubtype();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddledu_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            loadbatch();
            loadsubtype();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            loadsubtype();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddlexamyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            month();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void Buttongo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            FpSpread1.Sheets[0].ColumnCount = 8;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Sem";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Equal Subject";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Common Subject";

            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].Columns[1].Width = 100;
            FpSpread1.Sheets[0].Columns[2].Width = 180;
            FpSpread1.Sheets[0].Columns[3].Width = 50;
            FpSpread1.Sheets[0].Columns[4].Width = 300;
            FpSpread1.Sheets[0].Columns[5].Width = 80;
            FpSpread1.Sheets[0].Columns[6].Width = 80;
            FpSpread1.Sheets[0].Columns[7].Width = 80;

            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

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
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Large;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.Sheets[0].AutoPostBack = false;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

            string strquery = "select distinct sy.Batch_Year,c.Course_Name,de.Dept_Name,sy.semester,ss.subject_type,s.subject_name,s.subject_code,d.Degree_Code,s.subject_no from syllabus_master sy,Degree d,Course c,subject s,sub_sem ss,Department de,mark_entry m,Exam_Details ed where sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and m.subject_no=s.subject_no and ed.exam_code=m.exam_code";
            strquery = strquery + " and ed.Exam_year='" + ddlexamyear.SelectedValue.ToString() + "' and ed.Exam_Month='" + ddlmonth.SelectedValue.ToString() + "' and c.edu_level='" + ddledu.SelectedItem.ToString() + "' and d.college_code='" + ddlcollege.SelectedValue.ToString() + "' and sy.batch_year>='" + ddlbatch.SelectedItem.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' order by ss.subject_type,s.subject_name,s.subject_code desc,d.degree_code,sy.Batch_Year desc";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int srno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    srno++;
                    string batch = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                    string course = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                    string dept = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                    string sem = ds.Tables[0].Rows[i]["semester"].ToString();
                    string subtype = ds.Tables[0].Rows[i]["subject_type"].ToString();
                    string subjectname = ds.Tables[0].Rows[i]["subject_name"].ToString();
                    string subcode = ds.Tables[0].Rows[i]["subject_code"].ToString();
                    string degreecode = ds.Tables[0].Rows[i]["Degree_Code"].ToString();
                    string subno = ds.Tables[0].Rows[i]["subject_no"].ToString();
                    FpSpread1.Sheets[0].RowCount++;
                    if ((FpSpread1.Sheets[0].RowCount % 2) == 0)
                    {
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightGray;
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batch.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = course + " - " + dept;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = sem.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = subjectname.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = subcode.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = chk;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = chk;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = subno;
                }
                FpSpread1.Visible = true;
                btnsave.Visible = true;
                LoadDetails();
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            FpSpread1.Width = 940;
            FpSpread1.Height = 500;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();

            string commonsubject = "";
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 7].Value);
                if (isval > 0)
                {
                    commonsubject = FpSpread1.Sheets[0].Cells[i, 7].Tag.ToString();
                }
            }
            if (commonsubject.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Common Subject And Than Proceed";
                return;
            }
            Boolean saveflag = false;
            string strquery = "";
            int savval = 0;
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 6].Value);
                if (isval > 0)
                {
                    saveflag = true;
                    string subno = FpSpread1.Sheets[0].Cells[i, 7].Tag.ToString();
                    strquery = "if exists(select * from tbl_equal_subject_Grade_System where exam_year='" + ddlexamyear.SelectedValue.ToString() + "' and exam_month='" + ddlmonth.SelectedValue.ToString() + "' and Subject_no='" + subno + "')";
                    strquery = strquery + " update tbl_equal_subject_Grade_System set Common_Subject_no='" + commonsubject + "' where exam_year='" + ddlexamyear.SelectedValue.ToString() + "' and exam_month='" + ddlmonth.SelectedValue.ToString() + "' and Subject_no='" + subno + "'";
                    strquery = strquery + " else insert into tbl_equal_subject_Grade_System(exam_year,exam_month,Subject_no,Common_Subject_no) values('" + ddlexamyear.SelectedValue.ToString() + "','" + ddlmonth.SelectedValue.ToString() + "','" + subno + "','" + commonsubject + "')";
                    savval = d2.update_method_wo_parameter(strquery, "Text");
                }
            }
            if (saveflag == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved Successfully!')", true);
                LoadDetails();
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Equal Subject And Than Proceed";
                return;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void LoadDetails()
    {
        try
        {
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;

            FpSpread2.Sheets[0].ColumnCount = 7;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Sem";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Name - Code";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Common Subject Name - Code";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";

            FpSpread2.Sheets[0].Columns[0].Width = 50;
            FpSpread2.Sheets[0].Columns[1].Width = 80;
            FpSpread2.Sheets[0].Columns[2].Width = 200;
            FpSpread2.Sheets[0].Columns[3].Width = 50;
            FpSpread2.Sheets[0].Columns[4].Width = 220;
            FpSpread2.Sheets[0].Columns[5].Width = 220;
            FpSpread2.Sheets[0].Columns[6].Width = 50;

            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpSpread2.Sheets[0].SheetName = " ";
            FpSpread2.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread2.Sheets[0].AutoPostBack = false;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

            string strquery = "select sy.Batch_Year,c.Course_Name,de.Dept_Name,sy.semester,ss.subject_type,s.subject_name,s.subject_code,d.Degree_Code,s.subject_no,(Select s1.subject_name+' - '+s1.subject_code from subject s1 where s1.subject_no=t.Common_Subject_no) com_subname,";
            strquery = strquery + " t.Common_Subject_no from syllabus_master sy,Degree d,Course c,subject s,sub_sem ss,Department de,tbl_equal_subject_Grade_System t where sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and s.subject_no=t.Subject_no";
            strquery = strquery + " and c.edu_level='" + ddledu.SelectedItem.ToString() + "' and d.college_code='" + ddlcollege.SelectedValue.ToString() + "' and sy.batch_year>='" + ddlbatch.SelectedItem.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' order by ss.subject_type,s.subject_name,s.subject_code desc,d.degree_code,sy.Batch_Year desc";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int srno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    srno++;
                    string batch = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                    string course = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                    string dept = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                    string sem = ds.Tables[0].Rows[i]["semester"].ToString();
                    string subtype = ds.Tables[0].Rows[i]["subject_type"].ToString();
                    string subjectname = ds.Tables[0].Rows[i]["subject_name"].ToString();
                    string subcode = ds.Tables[0].Rows[i]["subject_code"].ToString();
                    string degreecode = ds.Tables[0].Rows[i]["Degree_Code"].ToString();
                    string subno = ds.Tables[0].Rows[i]["subject_no"].ToString();
                    string comsubno = ds.Tables[0].Rows[i]["Common_Subject_no"].ToString();
                    string comsubjectname = ds.Tables[0].Rows[i]["com_subname"].ToString();
                    FpSpread2.Sheets[0].RowCount++;

                    if ((FpSpread2.Sheets[0].RowCount % 2) == 0)
                    {
                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightGray;
                    }

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = batch.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = course + " - " + dept;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = sem.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = subjectname.ToString() + " - " + subcode.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = subno;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = comsubjectname.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = comsubno;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = chk;
                }
                FpSpread2.Visible = true;
                btndelete.Visible = true;
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            FpSpread2.Width = 940;
            FpSpread2.Height = 500;
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.SaveChanges();
            Boolean saveflag = false;
            string strquery = "";
            int savval = 0;
            for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
            {
                int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, 6].Value);
                if (isval > 0)
                {
                    saveflag = true;
                    string subno = FpSpread2.Sheets[0].Cells[i, 5].Tag.ToString();
                    strquery = "delete from tbl_equal_subject_Grade_System where exam_year='" + ddlexamyear.SelectedValue.ToString() + "' and exam_month='" + ddlmonth.SelectedValue.ToString() + "' and Common_Subject_no='" + subno + "'";
                    savval = d2.update_method_wo_parameter(strquery, "Text");
                }
            }
            if (saveflag == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Deleted Successfully!')", true);
                LoadDetails();
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Equal Subject And Than Proceed";
                return;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
}