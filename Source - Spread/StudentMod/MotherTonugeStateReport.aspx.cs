using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using FarPoint.Web.Spread.Design;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class MotherTonugeStateReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    string user_code = "";
    string college_code = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            user_code = Session["usercode"].ToString();
            college_code = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                BindBatch();
                degree();
                bindbranch();
                bindsem();
                ddltype.Items.Add("Details");
                ddltype.Items.Add("Summary");
                fpspread.Rows.Count = 0;
                fpspread.Sheets[0].AutoPostBack = true;
                fpspread.Sheets[0].Columns.Count = 0;
                fpspread.Visible = false;
                rptprint.Visible = false;

                string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                ds = d2.select_method_wo_parameter(Master1, "text");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    public void bindcollege()
    {
        try
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
            }
        }
        catch
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void degree()
    {
        try
        {
            ddldegree.Items.Clear();
            ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where    course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + ddlcollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code", "Text");
            ddldegree.Items.Clear();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
                ddldegree.Items.Insert(0, "All"); // added by Deepali on 5.4.18
            }

        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            ddldepartment.Items.Clear();
            string commname = "";
            string branch = ddldegree.SelectedItem.Value;
            // added by Deepali on 5.4.18
            if (branch == "All")
            {
                branch = getDdlSelectedValue(ddldegree);
            }
            //----------
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code";
            }
            ds = d2.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldepartment.DataSource = ds;
                ddldepartment.DataTextField = "dept_name";
                ddldepartment.DataValueField = "degree_code";
                ddldepartment.DataBind();
                ddldepartment.Items.Insert(0, "All"); // added by Deepali on 5.4.18
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
            ds.Clear();
            ddlsem.Items.Clear();
            // added by Deepali on 5.4.18
            string depts = ddldepartment.SelectedItem.Value;
            if (depts == "All")
            {
                depts = getDdlSelectedValue(ddldepartment);
            }
            //---------------------------

            //ds = d2.BindSem(ddldepartment.SelectedItem.Value, ddlbatch.SelectedItem.Text, ddlcollege.SelectedItem.Value);
            ds = d2.BindSem(depts, ddlbatch.SelectedItem.Text, ddlcollege.SelectedItem.Value);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string duration = Convert.ToString(ds.Tables[0].Rows[0][0]);
                if (duration.Trim() != "")
                {
                    for (int i = 1; i <= Convert.ToInt32(duration); i++)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                }

            }
        }
        catch
        {

        }
    }

    protected void ddlbatch_Change(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            fpspread.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }
    }

    protected void ddldepartment_Change(object sender, EventArgs e)
    {
        bindsem();
        fpspread.Visible = false;
        rptprint.Visible = false;
    }

    protected void ddldegree_Change(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        fpspread.Visible = false;
        rptprint.Visible = false;
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            if (ddltype.SelectedItem.Text == "Details")
            {
                fpspread.Rows.Count = 0;
                fpspread.Width = 800;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#007ABC");
                darkstyle.ForeColor = System.Drawing.Color.Black;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fpspread.Sheets[0].AutoPostBack = true;
                fpspread.CommandBar.Visible = false;
                fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                fpspread.Sheets[0].RowHeader.Visible = false;
                fpspread.Sheets[0].Columns.Count = 8;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Application ID";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Sex";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Mother Tongue";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "State";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[0].Width = 50;


                string query = "select app_formno,r.roll_no,r.reg_no , a.Stud_Name,case when sex=0 then 'Male' when sex=1 then 'Female' else 'Transgender' end as sex ,(c.Course_Name +'-'+dt.Dept_Name) as Department , (select TEXTVAL from TextValTable where TextCode =mother_tongue) as mother_tongue , (select TEXTVAL from TextValTable where TextCode =parent_statep ) as parent_statep from applyn a, Registration r,Degree d,Department dt,Course c where a.app_no =r.App_No and d.Degree_Code =a.degree_code and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and admission_status ='1' and a.degree_code =" + ddldepartment.SelectedItem.Value + " and a.batch_year =" + ddlbatch.SelectedItem.Value + " and a.college_code=r.college_code and a.college_code ='" + ddlcollege.SelectedItem.Value + "' and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Current_Semester ='" + ddlsem.SelectedItem.Text + "'";
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    query = query + " order by mother_tongue,r.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    query = query + " order by mother_tongue ,r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    query = query + " order by mother_tongue ,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    query = query + " order by mother_tongue ,r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    query = query + " order by mother_tongue ,r.Roll_No,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    query = query + " order by mother_tongue ,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    query = query + " order by mother_tongue ,r.Roll_No,r.Stud_Name";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        fpspread.Sheets[0].RowCount++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k + 1);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Locked = true;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[k]["app_formno"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[k]["roll_no"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[k]["reg_no"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[k]["Stud_Name"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[k]["sex"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[k]["mother_tongue"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[k]["parent_statep"]);

                    }
                    if (Session["Rollflag"].ToString() == "1")
                    {
                        fpspread.Sheets[0].Columns[2].Visible = true;
                    }
                    else
                    {
                        fpspread.Sheets[0].Columns[2].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "1")
                    {
                        fpspread.Sheets[0].Columns[3].Visible = true;
                    }
                    else
                    {
                        fpspread.Sheets[0].Columns[3].Visible = false;
                    }
                    fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                    fpspread.Visible = true;
                    errorlable.Visible = false;
                    rptprint.Visible = true;
                }
                else
                {
                    errorlable.Visible = true;
                    errorlable.Text = "No Records Found";
                    fpspread.Visible = false;
                    rptprint.Visible = false;
                }
            }
            if (ddltype.SelectedItem.Text == "Summary")
            {
                fpspread.Rows.Count = 0;
                fpspread.Width = 500;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#007ABC");
                darkstyle.ForeColor = System.Drawing.Color.Black;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fpspread.Sheets[0].AutoPostBack = true;
                fpspread.CommandBar.Visible = false;
                fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                fpspread.Sheets[0].RowHeader.Visible = false;
                fpspread.Sheets[0].Columns.Count = 3;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Mother Tongue & State";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Count";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[0].Width = 50;
                fpspread.Sheets[0].Columns[1].Visible = true;
                fpspread.Sheets[0].Columns[2].Visible = true;
                string selectquery = " select count(mother_tongue )as Count,(select TEXTVAL from TextValTable where TextCode =mother_tongue) as mother_tongue  from applyn a, Registration r,Degree d,Department dt,Course c where a.app_no =r.App_No  and d.Degree_Code =a.degree_code and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code  and c.Course_Id =d.Course_Id  and admission_status ='1' and a.degree_code =" + ddldepartment.SelectedItem.Value + " and a.batch_year =" + ddlbatch.SelectedItem.Text + "  and a.college_code=r.college_code and a.college_code ='" + ddlcollege.SelectedItem.Value + "' and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Current_Semester ='" + ddlsem.SelectedItem.Text + "' group by mother_tongue order by mother_tongue ";
                selectquery = selectquery + " select count(parent_statep )as Count,(select TEXTVAL from TextValTable where TextCode =parent_statep ) as parent_statep  from applyn a, Registration r,Degree d,Department dt,Course c where a.app_no =r.App_No  and d.Degree_Code =a.degree_code and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code  and c.Course_Id =d.Course_Id  and admission_status ='1' and a.degree_code =" + ddldepartment.SelectedItem.Value + " and a.batch_year =" + ddlbatch.SelectedItem.Text + "  and a.college_code=r.college_code and a.college_code ='" + ddlcollege.SelectedItem.Value + "' and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Current_Semester ='" + ddlsem.SelectedItem.Text + "' group by parent_statep order by parent_statep ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                int sno = 0;
                int count = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 3);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Mother Tongue";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#10BADC");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        fpspread.Sheets[0].RowCount++;
                        sno++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["mother_tongue"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Count"]);
                        count = count + Convert.ToInt32(ds.Tables[0].Rows[i]["Count"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    }
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(count);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                }
                sno = 0;
                count = 0;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 3);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "State";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#10BADC");
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        fpspread.Sheets[0].RowCount++;
                        sno++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["parent_statep"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[1].Rows[i]["Count"]);
                        count = count + Convert.ToInt32(ds.Tables[1].Rows[i]["Count"]);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    }
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 2);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(count);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                }
                if (fpspread.Sheets[0].RowCount > 3)
                {
                    fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                    fpspread.Visible = true;
                    errorlable.Visible = false;
                    rptprint.Visible = true;
                }
                else
                {
                    errorlable.Visible = true;
                    errorlable.Text = "No Records Found";
                    fpspread.Visible = false;
                    rptprint.Visible = false;
                }
            }

        }
        catch
        {

        }
    }

    protected void ddltype_Change(object sender, EventArgs e)
    {
        try
        {
            fpspread.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    //FpEntry.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
                    //Aruna on 26feb2013============================
                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                    fpspread.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
                    //=============================================
                }
                else
                {
                    errorlable.Text = "Please enter your Report Name";
                    errorlable.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            errorlable.Text = ex.ToString();
        }

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        string degreedetails = string.Empty;
        degreedetails = "Mother Tongue and State Wise Report" + '@' + "BATCH:   " + ddlbatch.SelectedItem.Text + "" + '@' + "COURSE:   " + ddldegree.SelectedItem.Text + " - " + ddldepartment.SelectedItem.Text + "" + '@' + "SEMESTER:   " + ddlsem.SelectedItem.Text + "" + '@' + "DATE:   " + System.DateTime.Now.ToString("dd/MM/yyyy") + "";
        string pagename = "MotherTonugeStateReport.aspx";
        Printcontrol.loadspreaddetails(fpspread, pagename, degreedetails);
        Printcontrol.Visible = true;

    }


    //added by Deepali on 5.4.18

    private string getDdlSelectedValue(DropDownList ddlSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 1; sel < ddlSelected.Items.Count; sel++)
            {

                if (selectedvalue.Length == 0)
                {
                    selectedvalue.Append(Convert.ToString(ddlSelected.Items[sel].Value));
                }
                else
                {
                    selectedvalue.Append("','" + Convert.ToString(ddlSelected.Items[sel].Value));
                }

            }
        }
        catch { ddlSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    //---------------------
}