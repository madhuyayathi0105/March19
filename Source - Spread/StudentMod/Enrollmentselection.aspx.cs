using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using Gios.Pdf;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;
using System.Configuration;


public partial class Enrollmentselection : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataTable data = new DataTable();
    bool ledgercellclik = false;
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
            errorlable.Visible = false;
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                bindtype();
                lblbatch.Text = DateTime.Now.ToString("yyyy");
                bindedulevel();
                degree();
                bindbranch();
                fpspread.Visible = false;
                cbapply.Checked = true;
                errorlable.Visible = false;
                btn_go.Visible = true;
                txt_startdate.Attributes.Add("readonly", "readonly");
                txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_startdate.Attributes.Add("readonly", "readonly");
                txt_enddate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                txtfrconfrm.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttoconfrm.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtfrconfrm.Attributes.Add("readonly", "readonly");
                txttoconfrm.Attributes.Add("readonly", "readonly");
                txtconfirmdt.Attributes.Add("readonly", "readonly");
                txtconfirmdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                rights(sender, e);
                //TabContainer1_Changed(sender, e);
            }
            //if (cbdegreewise.Checked == true)
            //{
            //    txt_degree.Enabled = true;
            //    txt_department.Enabled = true;
            //}
            //else
            //{
            //    txt_degree.Enabled = false;
            //    txt_department.Enabled = false;
            //}
        }
        catch (Exception ex) { }
    }

    public void rights(object sender, EventArgs e)
    {
        try
        {
            Session["admissionrights"] = "";
            string rights = "select * from security_user_right where rights_desc='Enrollment Settings' and user_code='" + user_code + "' and college_code='" + college_code + "'";
            ds = d2.select_method_wo_parameter(rights, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int h = 0; h < ds.Tables[0].Rows.Count; h++)
                {

                    string value = ds.Tables[0].Rows[h]["rights_code"].ToString();
                    if (value == "0")
                    {
                        tabpnlsel.Visible = true;
                        divsel.Visible = true;
                    }
                    else if (value == "1")
                    {
                        tabpnlconfm.Visible = true;
                        divsel.Visible = true;
                        tblconfrm.Visible = true;
                    }
                    else if (value == "2")
                    {
                        tabpnlsett.Visible = true;
                        subdivbase.Visible = true;
                    }
                    tabpnl.Attributes.Add("Style", "background-color:default;");
                }
            }
            else
            {
                tabpnl.Attributes.Add("Style", "background-color:Gray;");
                errorspan.InnerHtml = "Please Set the Rights";
                poperrjs.Visible = true;
                return;
            }
            if (tabpnlsel.Visible == true)
            {
                TabContainer1.ActiveTabIndex = 0;
                TabContainer1_Changed(sender, e);
            }
            else if (tabpnlconfm.Visible == true)
            {
                TabContainer1.ActiveTabIndex = 1;
                TabContainer1_Changed(sender, e);
            }
            else if (tabpnlsett.Visible == true)
            {
                TabContainer1.ActiveTabIndex = 2;
                TabContainer1_Changed(sender, e);
            }


        }
        catch (Exception ex)
        {

        }
    }

    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string college_code = "";
            if (ddl_collegename.Items.Count > 0)
            {
                college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            bindtype();
            bindedulevel();
            degree();
            bindbranch();
        }
        catch
        {
        }
    }

    public void bindtype()
    {
        try
        {
            string college_code = "";
            if (ddl_collegename.Items.Count > 0)
            {
                college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string typequery = "select distinct type  from course where college_code =" + college_code + " and type<>''";
            ds = d2.select_method_wo_parameter(typequery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
            }
            else
            {
                ddltype.Enabled = false;
            }
            if (ddltype.Items.Count > 0)
            {
                ddltype.Items.Insert(ddltype.Items.Count, "All");
                ddltype.SelectedIndex = ddltype.Items.Count - 1;
            }

        }
        catch
        {

        }
    }

    public void bindedulevel()
    {
        string college_code = "";
        if (ddl_collegename.Items.Count > 0)
        {
            college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
        }
        string query = "";
        string type = "";
        if (ddltype.Items.Count > 0)
        {
            if (ddltype.SelectedItem.Text == "All")
            {
                query = "select distinct Edu_Level  from course where  college_code=" + college_code + " order by Edu_Level desc";
            }
            else
            {
                query = "select distinct Edu_Level  from course where type='" + ddltype.SelectedItem.Text + "' and college_code=" + college_code + " order by Edu_Level desc";
            }
        }
        else
        {
            query = "select distinct Edu_Level  from course where  college_code=" + college_code + " order by Edu_Level desc";
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddledulevel.DataSource = ds;
            ddledulevel.DataTextField = "Edu_Level";
            ddledulevel.DataBind();
            if (ddledulevel.Items.Count > 1)
            {
                ddledulevel.Items.Insert(0, "Both");
            }
        }
    }
    protected void type_Change(object sender, EventArgs e)
    {
        try
        {
            bindedulevel();
            degree();
            bindbranch();
        }
        catch
        {
        }
    }

    protected void edulevel_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            degree();
            bindbranch();
        }
        catch
        {
        }
    }

    public void degree()
    {
        try
        {
            string college_code = "";
            if (ddl_collegename.Items.Count > 0)
            {
                college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            cbldegree.Items.Clear();
            if (ddledulevel.SelectedItem.Text == "Both" && ddltype.SelectedItem.Text == "All")
            {
                ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where   course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code", "Text");
            }
            else if (ddledulevel.SelectedItem.Text == "Both")
            {
                ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where    course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and course.type='" + ddltype.SelectedItem.Text + "'", "Text");
            }
            else if (ddltype.SelectedItem.Text == "All")
            {
                ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where    course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code  and course.Edu_Level='" + ddledulevel.SelectedItem.Value + "'", "Text");
            }
            else
            {
                ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where    course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and course.type='" + ddltype.SelectedItem.Text + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Value + "'", "Text");
            }
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                cbldegree.DataSource = ds;
                cbldegree.DataTextField = "course_name";
                cbldegree.DataValueField = "course_id";
                cbldegree.DataBind();

            }
            if (cbldegree.Items.Count > 0)
            {
                int count11 = 0;
                cbdegree.Checked = true;
                for (int j = 0; j < cbldegree.Items.Count; j++)
                {
                    count11++;
                    cbldegree.Items[j].Selected = true;
                }
                txt_degree.Text = lblDeg.Text + "(" + count11 + ")";
            }
            else
            {
                cbldegree.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
        }
    }

    //public void bindbranch()
    //{
    //    try
    //    {
    //        string college_code = "";
    //        if (ddl_collegename.Items.Count > 0)
    //        {
    //            college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
    //        }
    //        string commname = "";
    //        string branch = "";
    //        if (branch != "")
    //        {
    //            commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
    //        }
    //        else
    //        {
    //            if (cbldegree.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbldegree.Items.Count; i++)
    //                {
    //                    if (cbldegree.Items[i].Selected == true)
    //                    {
    //                        if (branch == "")
    //                        {
    //                            branch = cbldegree.Items[i].Value;
    //                        }
    //                        else
    //                        {
    //                            branch = branch + "'" + "," + "'" + cbldegree.Items[i].Value;
    //                        }

    //                    }
    //                }
    //            }
    //            commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code";
    //        }
    //        {
    //            ds = d2.select_method_wo_parameter(commname, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbldepartment.DataSource = ds;
    //                cbldepartment.DataTextField = "dept_name";
    //                cbldepartment.DataValueField = "degree_code";
    //                cbldepartment.DataBind();
    //            }
    //            if (cbldepartment.Items.Count > 0)
    //            {
    //                int count11 = 0;
    //                cbdepartment1.Checked = true;
    //                for (int j = 0; j < cbldepartment.Items.Count; j++)
    //                {
    //                    count11++;
    //                    cbldepartment.Items[j].Selected = true;
    //                }
    //                txt_department.Text = "Department(" + count11 + ")";
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    public void bindbranch()
    {
        try
        {
            string college_code = "";
            if (ddl_collegename.Items.Count > 0)
            {
                college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            cbldepartment.Items.Clear();
            string deg = "";
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    if (deg == "")
                    {
                        deg = cbldegree.Items[i].Value;
                    }
                    else
                    {
                        deg = deg + "'" + "," + "'" + cbldegree.Items[i].Value;
                    }

                }
            }
            int count = 0;
            if (deg != "")
            {
                ds = d2.select_method_wo_parameter("select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + deg + "') and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + user_code + "'", "Text");

                count = ds.Tables[0].Rows.Count;
            }
            if (count > 0)
            {

                cbldepartment.DataSource = ds;
                cbldepartment.DataTextField = "dept_name";
                cbldepartment.DataValueField = "degree_code";
                cbldepartment.DataBind();

            }
            if (cbldepartment.Items.Count > 0)
            {
                int count11 = 0;
                cbdepartment1.Checked = true;
                for (int j = 0; j < cbldepartment.Items.Count; j++)
                {
                    count11++;
                    cbldepartment.Items[j].Selected = true;
                }
                txt_department.Text = lblBran.Text + "(" + count11 + ")";
            }
            else
            {
                // cbldepartment.Items.Insert(0, "--Select--");
                txt_department.Text = "--Select--";
                cbdepartment1.Checked = false;
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string resid = "";
            string degree = "";
            if (txt_department.Enabled == true)
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    if (cbldepartment.Items[i].Selected == true)
                    {
                        if (degree == "")
                        {
                            degree = cbldepartment.Items[i].Value;
                        }
                        else
                        {
                            degree = degree + "'" + "," + "'" + cbldepartment.Items[i].Value;
                        }
                    }
                }
            }
            string batchyear = Convert.ToString(lblbatch.Text);
            string type = ddltype.SelectedItem.Text;
            string eduleve = ddledulevel.SelectedItem.Text;
            string campus = "";

            if (cbapply.Checked == true)
            {
                campus = " and CampusReq='0'";
                resid = "0";
            }
            else if (cbnotapply.Checked == true)
            {
                campus = " and CampusReq='1'";
                resid = "1";
            }

            string query = "";

            #region Query
            if (ddledulevel.SelectedItem.Text == "Both" && ddltype.SelectedItem.Text == "All")
            {
                query = " select app_formno,'' Reg_No,a.stud_name,(c.Course_Name+' - '+dt.Dept_Name)  as deptname,Student_Mobile ,StuPer_Id,type,Edu_Level from applyn a,Degree d,Department dt,Course c where  a.admission_status ='1' and a.batch_year in ('" + batchyear + "') and a.degree_code =d.Degree_Code   and d.Dept_Code =dt.Dept_Code and c.Course_Id =D.Course_Id   and a.degree_code in ('" + degree + "')   " + campus + " and (isnull (enrollmentcard,'')='' or enrollmentcard='0')";

            }
            //in ('" + batchyear + "')
            else if (ddledulevel.SelectedItem.Text == "Both")
            {
                query = " select app_formno,'' Reg_No,a.stud_name,(c.Course_Name+' - '+dt.Dept_Name)  as deptname,Student_Mobile ,StuPer_Id,type,Edu_Level from applyn a,Degree d,Department dt,Course c where  a.admission_status ='1' and a.batch_year in ('" + batchyear + "') and a.degree_code =d.Degree_Code   and d.Dept_Code =dt.Dept_Code and c.Course_Id =D.Course_Id  and c.type='" + type + "'  and a.degree_code in ('" + degree + "')   " + campus + " and (isnull (enrollmentcard,'')='' or enrollmentcard='0')";
            }
            else if (ddltype.SelectedItem.Text == "All")
            {
                query = " select app_formno,'' Reg_No,a.stud_name,(c.Course_Name+' - '+dt.Dept_Name)  as deptname,Student_Mobile ,StuPer_Id,type,Edu_Level from applyn a,Degree d,Department dt,Course c where  a.admission_status ='1' and a.batch_year in ('" + batchyear + "') and a.degree_code =d.Degree_Code   and d.Dept_Code =dt.Dept_Code and c.Course_Id =D.Course_Id   and a.degree_code in ('" + degree + "') and c.Edu_Level ='" + eduleve + "'  " + campus + " and (isnull (enrollmentcard,'')='' or enrollmentcard='0')";
            }
            else
            {
                query = " select app_formno,'' Reg_No,a.stud_name,(c.Course_Name+' - '+dt.Dept_Name)  as deptname,Student_Mobile ,StuPer_Id,type,Edu_Level from applyn a,Degree d,Department dt,Course c where  a.admission_status ='1' and a.batch_year in ('" + batchyear + "') and a.degree_code =d.Degree_Code   and d.Dept_Code =dt.Dept_Code and c.Course_Id =D.Course_Id  and c.type='" + type + "' and a.degree_code in ('" + degree + "') and c.Edu_Level ='" + eduleve + "'  " + campus + " and (isnull (enrollmentcard,'')='' or enrollmentcard='0')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            #endregion

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region design

                fpspread.Sheets[0].RowCount = 0;
                fpspread.Sheets[0].ColumnCount = 0;
                fpspread.Sheets[0].ColumnCount = 7;
                fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;


                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg.No";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;

                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = lblDeg.Text + "/" + lblBran.Text;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Date";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;

                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Session";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;

                fpspread.Sheets[0].AutoPostBack = false;
                fpspread.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                fpspread.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                fpspread.Pager.Align = HorizontalAlign.Right;
                fpspread.Pager.Font.Bold = true;
                fpspread.Pager.Font.Name = "Arial Narrow";
                fpspread.Pager.ForeColor = Color.DarkGreen;
                fpspread.Pager.BackColor = Color.Beige;
                fpspread.Pager.BackColor = Color.AliceBlue;
                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                cb.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                cb1.AutoPostBack = false;
                fpspread.Sheets[0].RowHeader.Visible = false;
                fpspread.CommandBar.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fpspread.SaveChanges();

                fpspread.Sheets[0].Columns[0].Width = 44;
                fpspread.Sheets[0].Columns[1].Width = 85;
                fpspread.Sheets[0].Columns[2].Width = 121;
                fpspread.Sheets[0].Columns[3].Width = 230;
                fpspread.Sheets[0].Columns[4].Width = 180;
                fpspread.Sheets[0].Columns[5].Width = 109;
                fpspread.Sheets[0].Columns[6].Width = 147;
                #endregion

                #region fpspread value bind
                int sno_Value = 0;
                fpspread.Sheets[0].RowCount++;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = cb;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Value = 0;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    fpspread.Sheets[0].RowCount++;
                    sno_Value++;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno_Value);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["app_formno"]);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Locked = true;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = cb1;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Value = 0;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["app_formno"]);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Type"]);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Locked = true;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Locked = true;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["deptname"]);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Locked = true;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Locked = true;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Locked = true;
                }
                //  app_formno
                #endregion

                #region enrollment Session Date
                bool iscomp = false;
                DataSet dsdt = new DataSet();
                int rowcount = 0;
                string Selenroll = "  select CONVERT(varchar(10),Date,103)as Date,Iscomplete,totalcount,TotalNoPerDays,Noofsession,EnrollPK from Stud_Enrollment where isEntrolltype='" + resid + "' and ISNULL(Iscomplete,'0')<>'2' and Date >='" + DateTime.Now.ToString("MM/dd/yyyy") + "' order by Iscomplete desc,Date asc ";
                dsdt.Clear();
                dsdt = d2.select_method_wo_parameter(Selenroll, "Text");
                DataSet dsses = new DataSet();
                if (dsdt.Tables.Count > 0 && dsdt.Tables[0].Rows.Count > 0)
                {
                    for (int roll = 0; roll < dsdt.Tables[0].Rows.Count; roll++)
                    {
                        string EnrollFK = Convert.ToString(dsdt.Tables[0].Rows[roll]["EnrollPK"]);
                        string date = Convert.ToString(dsdt.Tables[0].Rows[roll]["Date"]);
                        string numofstud = Convert.ToString(dsdt.Tables[0].Rows[roll]["TotalNoPerDays"]);
                        string numofses = Convert.ToString(dsdt.Tables[0].Rows[roll]["Noofsession"]);
                        string complte = Convert.ToString(dsdt.Tables[0].Rows[roll]["Iscomplete"]);
                        if (complte == "" || complte == null)
                            complte = "0";
                        string totcount = Convert.ToString(dsdt.Tables[0].Rows[roll]["totalcount"]);
                        if (totcount == "" || totcount == null)
                            totcount = "0";
                        string SEsson = "select Start_session,Endsession from Enrollmentsession where EnrollFK='" + EnrollFK + "'";
                        dsses.Clear();
                        dsses = d2.select_method_wo_parameter(SEsson, "Text");
                        if (complte == "1")
                        {
                            int Sescount = Convert.ToInt32(numofstud) / Convert.ToInt32(numofses);
                            for (int k = 0; k < Convert.ToInt32(numofses); k++)
                            {
                                if (Convert.ToInt32(Sescount) <= Convert.ToInt32(totcount))
                                {
                                    Sescount += Sescount;
                                    continue;
                                }
                                else if (Sescount >= Convert.ToInt32(totcount))
                                {
                                    int Sglcount = Sescount - Convert.ToInt32(totcount);
                                    for (int fp = 0; fp <= Convert.ToInt32(Sglcount); fp++)
                                    {
                                        if (fp == 0)
                                            continue;
                                        for (int ses = k; ses <= k; ses++)
                                        {
                                            rowcount++;
                                            if (rowcount <= fpspread.Sheets[0].Rows.Count - 1)
                                            {
                                                fpspread.Sheets[0].Cells[rowcount, 5].Text = date;
                                                fpspread.Sheets[0].Cells[rowcount, 6].Text = Convert.ToString(dsses.Tables[0].Rows[ses]["Start_session"]) + "-" + Convert.ToString(dsses.Tables[0].Rows[ses]["Endsession"]);
                                                fpspread.Sheets[0].Cells[rowcount, 5].Font.Size = FontUnit.Medium;
                                                fpspread.Sheets[0].Cells[rowcount, 5].Font.Bold = true;
                                                fpspread.Sheets[0].Cells[rowcount, 5].Font.Name = "Book Antiqua";
                                                fpspread.Sheets[0].Cells[rowcount, 6].Font.Size = FontUnit.Medium;
                                                fpspread.Sheets[0].Cells[rowcount, 6].Font.Bold = true;
                                                fpspread.Sheets[0].Cells[rowcount, 6].Font.Name = "Book Antiqua";
                                                string upd = "update Stud_Enrollment set totalcount=isnull(totalcount,'0')+'1' where EnrollPK='" + EnrollFK + "'";
                                                d2.update_method_wo_parameter(upd, "Text");
                                                iscomp = true;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    totcount = "0";
                                }
                            }
                            if (iscomp == true)
                            {
                                string tcount = d2.GetFunction("select totalcount from Stud_Enrollment where EnrollPK='" + EnrollFK + "'");
                                string scount = d2.GetFunction("select TotalNoPerDays from Stud_Enrollment where EnrollPK='" + EnrollFK + "'");
                                if (tcount != "0" && tcount != "" && scount != "0" && scount != "")
                                {
                                    if (Convert.ToDouble(tcount) == Convert.ToDouble(scount))
                                    {
                                        string upd = "update Stud_Enrollment set Iscomplete='2',totalcount='" + numofstud + "' where EnrollPK='" + EnrollFK + "'";
                                        d2.update_method_wo_parameter(upd, "Text");
                                    }
                                }
                            }

                        }
                        else
                        {
                            int Sescount = Convert.ToInt32(numofstud) / Convert.ToInt32(numofses);
                            for (int k = 0; k < Convert.ToInt32(numofses); k++)
                            {
                                for (int fp = 0; fp <= Convert.ToInt32(Sescount); fp++)
                                {
                                    if (fp == 0)
                                        continue;
                                    for (int ses = k; ses <= k; ses++)
                                    {
                                        rowcount++;
                                        if (rowcount <= fpspread.Sheets[0].Rows.Count - 1)
                                        {
                                            fpspread.Sheets[0].Cells[rowcount, 5].Text = date;
                                            fpspread.Sheets[0].Cells[rowcount, 6].Text = Convert.ToString(dsses.Tables[0].Rows[ses]["Start_session"]) + "-" + Convert.ToString(dsses.Tables[0].Rows[ses]["Endsession"]);
                                            fpspread.Sheets[0].Cells[rowcount, 5].Font.Size = FontUnit.Medium;
                                            fpspread.Sheets[0].Cells[rowcount, 5].Font.Bold = true;
                                            fpspread.Sheets[0].Cells[rowcount, 5].Font.Name = "Book Antiqua";
                                            fpspread.Sheets[0].Cells[rowcount, 6].Font.Size = FontUnit.Medium;
                                            fpspread.Sheets[0].Cells[rowcount, 6].Font.Bold = true;
                                            fpspread.Sheets[0].Cells[rowcount, 6].Font.Name = "Book Antiqua";

                                            string upd = "update Stud_Enrollment set Iscomplete='1',totalcount=isnull(totalcount,'0')+'1' where EnrollPK='" + EnrollFK + "'";
                                            d2.update_method_wo_parameter(upd, "Text");
                                            iscomp = true;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }

                            }
                            if (iscomp == true)
                            {
                                string tcount = d2.GetFunction("select totalcount from Stud_Enrollment where EnrollPK='" + EnrollFK + "'");
                                string scount = d2.GetFunction("select TotalNoPerDays from Stud_Enrollment where EnrollPK='" + EnrollFK + "'");
                                if (tcount != "0" && tcount != "" && scount != "0" && scount != "")
                                {
                                    if (Convert.ToDouble(tcount) == Convert.ToDouble(scount))
                                    {
                                        string upd = "update Stud_Enrollment set Iscomplete='2',totalcount='" + numofstud + "' where EnrollPK='" + EnrollFK + "'";
                                        d2.update_method_wo_parameter(upd, "Text");
                                    }
                                }

                            }

                        }
                    }
                }
                else
                {
                    fpspread.Visible = false;
                    btnexcel.Visible = false;
                    btn_pdf.Visible = false;
                    btnenrolment.Visible = false;
                    //  btn_go.Visible = false;
                    errorspan.InnerHtml = "Please Set The Sessions";
                    poperrjs.Visible = true;
                    return;
                }
                #endregion

                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                fpspread.SaveChanges();
                fpspread.Height = 430;
                fpspread.Visible = true;
                btnexcel.Visible = true;
                btn_pdf.Visible = true;
                btnenrolment.Visible = true;
                btn_go.Visible = true;
                fpspread.ShowHeaderSelection = false;

            }
            else
            {
                fpspread.Visible = false;
                //Showgrid.Visible = false;
                btn_pdf.Visible = false;
                btnexcel.Visible = false;
                errorlable.Visible = true;
                btnenrolment.Visible = false;
                btnexcel.Visible = false;
                btn_pdf.Visible = false;
                //btn_go.Visible = false;
                errorspan.InnerHtml = "No Record Found";
                poperrjs.Visible = true;
            }


        }
        catch
        {

        }

    }

    protected void btnenrollconfrm_Click(object sender, EventArgs e)
    {
        try
        {
            LoadConfirmValues();
        }
        catch { }
    }

    protected void LoadConfirmValues()
    {
        try
        {
            #region Query

            string degree = "";
            if (txt_department.Enabled == true)
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    if (cbldepartment.Items[i].Selected == true)
                    {
                        if (degree == "")
                        {
                            degree = cbldepartment.Items[i].Value;
                        }
                        else
                        {
                            degree = degree + "'" + "," + "'" + cbldepartment.Items[i].Value;
                        }
                    }
                }
            }
            string batchyear = Convert.ToString(lblbatch.Text);
            string type = ddltype.SelectedItem.Text;
            string eduleve = ddledulevel.SelectedItem.Text;
            string campus = "";

            if (cbapply.Checked == true)
                campus = " and CampusReq='0'";
            else if (cbnotapply.Checked == true)
                campus = " and CampusReq='1'";



            string query = "";

            string fromdate = Convert.ToString(txtfrconfrm.Text);
            string todate = Convert.ToString(txttoconfrm.Text);
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            }
            string enrollment = "";
            string enrolltype = "";
            string enrolldate = "";
            string enrollorder = "";
            if (ddlenrollconfm.Items.Count > 0)
            {
                enrollment = Convert.ToString(ddlenrollconfm.SelectedItem.Text);
                if (enrollment.Trim() == "Enrolled")
                {
                    enrolltype = " and ISNULL(is_enroll,'0')<>'2' ";
                    //enrolldate = " and enrollment_card_date between '" + fromdate + "' and '" + todate + "'";
                    //enrollorder = " enrollment_card_date ";
                    enrolldate = " and admitcard_date between '" + fromdate + "' and '" + todate + "'";
                    enrollorder = " admitcard_date ";
                }
                else
                {
                    enrolltype = " and ISNULL(is_enroll,'0')='2' ";
                    enrolldate = " and enrollment_confirm_date between '" + fromdate + "' and '" + todate + "'";
                    enrollorder = " enrollment_confirm_date ";
                   
                }
            }//enrollment_confirm_date

            if (ddledulevel.SelectedItem.Text == "Both" && ddltype.SelectedItem.Text == "All")
            {
                query = "select app_formno,'' Reg_No,a.stud_name,(c.Course_Name+' - '+dt.Dept_Name)  as deptname,Student_Mobile ,StuPer_Id,type,Edu_Level,convert(varchar(10),a.enrollment_card_date,103) as enrollment_card_date,a.enrollmentcard,a.enrollment_session,a.is_enroll,a.batch_year,a.degree_code,convert(varchar(10),a.admitcard_date,103) as admitcard_date,convert(varchar(10),a.enrollment_confirm_date,103) as enrollment_confirm_date from applyn a,Degree d,Department dt,Course c where  a.admission_status ='1' and a.batch_year in ('" + batchyear + "') and a.degree_code =d.Degree_Code   and d.Dept_Code =dt.Dept_Code and c.Course_Id =D.Course_Id   and a.degree_code in ('" + degree + "')  " + campus + " and isnull(enrollmentcard,'0') ='1'  " + enrolltype + " " + enrolldate + " order by " + enrollorder + " asc";
            }
            //in ('" + batchyear + "')
            else if (ddledulevel.SelectedItem.Text == "Both")
            {
                query = "select app_formno,'' Reg_No,a.stud_name,(c.Course_Name+' - '+dt.Dept_Name)  as deptname,Student_Mobile ,StuPer_Id,type,Edu_Level,convert(varchar(10),a.enrollment_card_date,103) as enrollment_card_date,a.enrollmentcard,a.enrollment_session,a.is_enroll,a.batch_year,a.degree_code,convert(varchar(10),a.admitcard_date,103) as admitcard_date,convert(varchar(10),a.enrollment_confirm_date,103) as enrollment_confirm_date from applyn a,Degree d,Department dt,Course c where  a.admission_status ='1' and a.batch_year in ('" + batchyear + "') and a.degree_code =d.Degree_Code   and d.Dept_Code =dt.Dept_Code and c.Course_Id =D.Course_Id  and c.type='" + type + "' and a.degree_code in ('" + degree + "')  " + campus + " and isnull(enrollmentcard,'0') ='1'  " + enrolltype + " " + enrolldate + " order by " + enrollorder + " asc";
            }
            else if (ddltype.SelectedItem.Text == "All")
            {
                query = "select app_formno,'' Reg_No,a.stud_name,(c.Course_Name+' - '+dt.Dept_Name)  as deptname,Student_Mobile ,StuPer_Id,type,Edu_Level,convert(varchar(10),a.enrollment_card_date,103) as enrollment_card_date,a.enrollmentcard,a.enrollment_session,a.is_enroll,a.batch_year,a.degree_code,convert(varchar(10),a.admitcard_date,103) as admitcard_date,convert(varchar(10),a.enrollment_confirm_date,103) as enrollment_confirm_date from applyn a,Degree d,Department dt,Course c where  a.admission_status ='1' and a.batch_year in ('" + batchyear + "') and a.degree_code =d.Degree_Code   and d.Dept_Code =dt.Dept_Code and c.Course_Id =D.Course_Id   and a.degree_code in ('" + degree + "') and c.Edu_Level ='" + eduleve + "' " + campus + " and isnull(enrollmentcard,'0') ='1'  " + enrolltype + " " + enrolldate + " order by " + enrollorder + " asc";
            }
            else
            {
                query = "select app_formno,'' Reg_No,a.stud_name,(c.Course_Name+' - '+dt.Dept_Name)  as deptname,Student_Mobile ,StuPer_Id,type,Edu_Level,convert(varchar(10),a.enrollment_card_date,103) as enrollment_card_date,a.enrollmentcard,a.enrollment_session,a.is_enroll,a.batch_year,a.degree_code,convert(varchar(10),a.admitcard_date,103) as admitcard_date,convert(varchar(10),a.enrollment_confirm_date,103) as enrollment_confirm_date from applyn a,Degree d,Department dt,Course c where  a.admission_status ='1' and a.batch_year in ('" + batchyear + "') and a.degree_code =d.Degree_Code   and d.Dept_Code =dt.Dept_Code and c.Course_Id =D.Course_Id  and c.type='" + type + "' and a.degree_code in ('" + degree + "') and c.Edu_Level ='" + eduleve + "' " + campus + " and isnull(enrollmentcard,'0') ='1'  " + enrolltype + " " + enrolldate + " order by " + enrollorder + " asc";


            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");

            #endregion

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                #region design

                fpconfrm.Sheets[0].RowCount = 0;
                fpconfrm.Sheets[0].ColumnCount = 0;
                fpconfrm.CommandBar.Visible = false;
                fpconfrm.Sheets[0].AutoPostBack = false;
                fpconfrm.Sheets[0].ColumnHeader.RowCount = 1;
                fpconfrm.Sheets[0].RowHeader.Visible = false;
                fpconfrm.Sheets[0].ColumnCount = 7;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                fpconfrm.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpconfrm.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpconfrm.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;


                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpconfrm.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;


                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpconfrm.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;


                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 4].Text = lblDeg.Text + "/" + lblBran.Text;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpconfrm.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;


                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Date";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpconfrm.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;


                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Confirm Date";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                fpconfrm.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                fpconfrm.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;

                fpconfrm.Sheets[0].Columns[0].Width = 44;
                fpconfrm.Sheets[0].Columns[1].Width = 85;
                fpconfrm.Sheets[0].Columns[2].Width = 121;
                fpconfrm.Sheets[0].Columns[3].Width = 230;
                fpconfrm.Sheets[0].Columns[4].Width = 180;
                fpconfrm.Sheets[0].Columns[5].Width = 109;
                fpconfrm.Sheets[0].Columns[6].Width = 147;

                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                cb.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                cb1.AutoPostBack = false;
                #endregion

                #region values
                int height = 0;
                int sno_Value = 0;
                fpconfrm.Sheets[0].RowCount++;
                fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 1].CellType = cb;
                fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 1].Value = 0;
                fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    fpconfrm.Sheets[0].RowCount++;
                    height += 10;
                    sno_Value++;
                    string isenroll = Convert.ToString(ds.Tables[0].Rows[row]["is_enroll"]);
                    if (isenroll != "2")
                    {
                        if (isenroll == "1")
                            fpconfrm.Sheets[0].Rows[fpconfrm.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                        else
                            fpconfrm.Sheets[0].Rows[fpconfrm.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                    }
                    else
                    {
                        fpconfrm.Sheets[0].Rows[fpconfrm.Sheets[0].RowCount - 1].BackColor = Color.LightSalmon;
                    }
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno_Value);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["app_formno"]);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 0].Locked = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 1].CellType = cb1;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 1].Value = 0;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["app_formno"]);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Type"]);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 2].Locked = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["batch_year"]);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 3].Locked = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["deptname"]);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 4].Locked = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 5].Locked = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["admitcard_date"]);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 6].Locked = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["enrollment_confirm_date"]);
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                    fpconfrm.Sheets[0].Cells[fpconfrm.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                }

                #endregion

                fpconfrm.Sheets[0].PageSize = fpconfrm.Sheets[0].RowCount;
                fpconfrm.SaveChanges();
                if (height > 400)
                    fpconfrm.Height = height;
                else
                    fpconfrm.Height = 350;
                fpconfrm.Visible = true;
                printconffm.Visible = true;
                if (ddlenrollconfm.Items.Count > 0)
                {
                    enrollment = Convert.ToString(ddlenrollconfm.SelectedItem.Text);
                    if (enrollment.Trim() == "Enrolled")
                    {
                        btnConfrmsave.Visible = true;
                        txtconfirmdt.Visible = true;
                    }
                    else
                    {
                        btnConfrmsave.Visible = false;
                        txtconfirmdt.Visible = false;
                    }
                }

                fpconfrm.ShowHeaderSelection = false;
            }
            else
            {
                fpconfrm.Visible = false;
                printconffm.Visible = false;
                btnConfrmsave.Visible = false;
                txtconfirmdt.Visible = false;
                errorspan.InnerHtml = "No Record Found";
                poperrjs.Visible = true;
            }
        }
        catch { }
    }

    protected void btnConfrmsave_Click(object sender, EventArgs e)
    {
        try
        {
            // DateTime dt = new DateTime();
            bool save = false;
            string degreecode = "";
            string batchyr = "";
            fpconfrm.SaveChanges();
            string confmdt = Convert.ToString(txtconfirmdt.Text);
            string dt = "";
            string[] splitdt = confmdt.Split('/');
            if (splitdt.Length > 0)
            {
                dt = splitdt[1] + "/" + splitdt[0] + "/" + splitdt[2];
            }
            for (int i = 0; i < fpconfrm.Sheets[0].Rows.Count; i++)
            {
                if (i == 0)
                    continue;
                int isval = 0;
                isval = Convert.ToInt32(fpconfrm.Sheets[0].Cells[i, 1].Value);
                if (isval == 1)
                {
                    string rollno = Convert.ToString(fpconfrm.Sheets[0].Cells[i, 0].Tag);
                    if (rollno.Trim() != "")
                    {
                        string appno = d2.GetFunction("Select App_no from Applyn where app_formno='" + rollno + "'");
                        string critcode = d2.GetFunction("select criteria_Code  from selectcriteria where app_no='" + appno + "'");
                        string name = Convert.ToString(fpconfrm.Sheets[0].Cells[i, 3].Text);
                        batchyr = Convert.ToString(fpconfrm.Sheets[0].Cells[i, 3].Tag);
                        string deptname = Convert.ToString(fpconfrm.Sheets[0].Cells[i, 4].Text);
                        degreecode = Convert.ToString(fpconfrm.Sheets[0].Cells[i, 4].Tag);
                        string[] splitdptname = deptname.Split('-');
                        string deprt = Convert.ToString(splitdptname[0]);
                        string course = Convert.ToString(splitdptname[1]);
                        string date = Convert.ToString(fpconfrm.Sheets[0].Cells[i, 5].Text);
                        // string dt = "";
                        //string[] splitdt = date.Split('/');
                        //if (splitdt.Length > 0)
                        //{
                        //    dt = splitdt[1] + "/" + splitdt[0] + "/" + splitdt[2];
                        //}
                        string session = Convert.ToString(fpconfrm.Sheets[0].Cells[i, 6].Text);
                        string type = Convert.ToString(fpconfrm.Sheets[0].Cells[i, 2].Tag);
                        //applyn
                        if (appno != "")
                        {
                            string upadte = "update applyn set enrollment_confirm_date='" + dt + "' ,is_enroll='2' where app_no='" + appno + "'";
                            int a = d2.update_method_wo_parameter(upadte, "Text");
                            // Criteria code update
                            string CrUpd = "update selectcriteria set admit_confirm='1' where app_no='" + appno + "'";
                            int crup = d2.update_method_wo_parameter(CrUpd, "Text");
                            //admitcolumn update
                            string Adupd = "update admitcolumnset set allot_Confirm =allot_Confirm +1 where setcolumn ='" + degreecode + "' and column_name ='" + critcode + "'";
                            int admit = d2.update_method_wo_parameter(Adupd, "Text");

                            //registration
                            string regInsQ = "  if exists(select * from Registration where App_No='" + appno + "' and Adm_Date='" + dt + "' and Stud_Name='" + name + "' and Batch_Year='" + batchyr + "' and   degree_code='" + degreecode + "' and  college_code='" + college_code + "' )  delete from Registration where App_No='" + appno + "' and Adm_Date='" + dt + "' and Stud_Name='" + name + "' and Batch_Year='" + batchyr + "' and   degree_code='" + degreecode + "' and  college_code='" + college_code + "' insert into Registration    (App_No, Adm_Date, Roll_Admit, Roll_No, RollNo_Flag, Reg_No, Stud_Name, Batch_Year, degree_code, college_code, CC, DelFlag, Exam_Flag, Current_Semester,mode,Stud_Type) values ('" + appno + "','" + dt + "','" + rollno + "','" + rollno + "','1','" + rollno + "','" + name + "','" + batchyr + "','" + degreecode + "','" + college_code + "','0','0','OK','1',1,'Day Scholar')";
                            int ins = d2.update_method_wo_parameter(regInsQ, "Text");
                            save = true;
                        }
                    }
                }
            }
            if (save == true)
            {
                btnenrollconfrm_Click(sender, e);
                errorspan.InnerHtml = "Confirm Successfully";
                poperrjs.Visible = true;
            }
            else
            {
                errorspan.InnerHtml = "Please Select Any One Student";
                poperrjs.Visible = true;
            }
            // errorspan.InnerHtml = "No Record Found";
            // poperrjs.Visible = true;
        }
        catch { }
    }

    protected void fpconfrm_Command(object sender, EventArgs e)
    {
        try
        {
            // fpspread.SaveChanges();
            int value = 0;
            string activerow = fpconfrm.ActiveSheetView.ActiveRow.ToString();
            string activecol = fpconfrm.ActiveSheetView.ActiveColumn.ToString();
            if (activerow == "0" && activecol == "1")
            {
                value = Convert.ToInt32(fpconfrm.Sheets[0].Cells[0, 1].Value);
                for (int row = 0; row < fpconfrm.Rows.Count; row++)
                {
                    if (value == 1)
                    {
                        fpconfrm.Sheets[0].Cells[row, 1].Value = 1;
                    }
                    else
                    {
                        fpconfrm.Sheets[0].Cells[row, 1].Value = 0;
                    }
                }
            }

        }
        catch
        {

        }
    }

    protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // Showgrid.PageIndex = e.NewPageIndex;
        btn_go_Click(sender, e);
    }

    protected void cbdegree_Changed(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbdegree, cbldegree, txt_degree, lblDeg.Text, "--Select--");
            bindbranch();
            //if (cbdegree.Checked == true)
            //{
            //    for (int i = 0; i < cbldegree.Items.Count; i++)
            //    {

            //        cbldegree.Items[i].Selected = true;
            //        txt_degree.Text = "Degree(" + (cbldegree.Items.Count) + ")";
            //    }
            //    bindbranch();

            //}
            //else
            //{
            //    for (int i = 0; i < cbldegree.Items.Count; i++)
            //    {
            //        cbldegree.Items[i].Selected = false;
            //        txt_degree.Text = "--Select--";
            //    }
            //    cbldepartment.Items.Clear();
            //    txt_department.Text = "--Select--";
            //    cbdepartment1.Checked = false;
            //}
        }
        catch
        {

        }
    }

    protected void cbldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbdegree, cbldegree, txt_degree, lblDeg.Text, "--Select--");
            bindbranch();
            //int seatcount = 0;
            //cbdegree.Checked = false;
            //for (int i = 0; i < cbldegree.Items.Count; i++)
            //{
            //    if (cbldegree.Items[i].Selected == true)
            //    {
            //        seatcount = seatcount + 1;
            //    }
            //    bindbranch();
            //}
            //txt_degree.Text = "Degree(" + seatcount.ToString() + ")";

        }
        catch
        {

        }
    }

    protected void cbdepartment_Changed(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbdepartment1, cbldepartment, txt_department, lblBran.Text, "--Select--");
            //if (cbdepartment1.Checked == true)
            //{
            //    for (int i = 0; i < cbldepartment.Items.Count; i++)
            //    {

            //        cbldepartment.Items[i].Selected = true;
            //        txt_department.Text = "Department(" + (cbldepartment.Items.Count) + ")";
            //    }

            //}
            //else
            //{

            //    for (int i = 0; i < cbldepartment.Items.Count; i++)
            //    {
            //        cbldepartment.Items[i].Selected = false;
            //        txt_department.Text = "--Select--";
            //    }
            //}
        }
        catch
        {

        }
    }

    protected void cbldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //int seatcount = 0;
            //cbdepartment1.Checked = false;
            //for (int i = 0; i < cbldepartment.Items.Count; i++)
            //{
            //    if (cbldepartment.Items[i].Selected == true)
            //    {
            //        seatcount = seatcount + 1;
            //    }

            //}
            //txt_department.Text = "Department(" + seatcount.ToString() + ")";
            CallCheckboxListChange(cbdepartment1, cbldepartment, txt_department, lblBran.Text, "--Select--");

        }
        catch
        {

        }
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
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
            string name = "";
            cb.Checked = false;
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
            else
            {
                txt.Text = deft;
            }
        }
        catch { }
    }

    protected void lnk_logout(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {

        }
    }

    protected void pdf_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Enrollment Selection List";
            string pagename = "Enrollmentselection.aspx";
            Printcontrol.loadspreaddetails(fpspread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */

    }

    protected void btn_excelClcik(object sender, EventArgs e)
    {
        try
        {
            string reportname = "Enrollment list";

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(fpspread, reportname);

            }
        }
        catch
        {

        }
    }

    protected void txt_sessionchange(object sender, EventArgs e)
    {
        Bindsessiongrid("-1");
    }

    protected void Bindsessiongrid(string PK)
    {
        try
        {
            Sessiongrid.DataSource = null;
            Sessiongrid.Visible = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("Sno", typeof(string));
            dt.Columns.Add("statr", typeof(string));
            dt.Columns.Add("end", typeof(string));
            int sno_v = 0;
            Sessiongrid.Visible = true;

            if (PK == "-1")
            {
                string value = Convert.ToString(txt_noofsession.Text);
                if (value != "")
                {
                    for (int i = 0; i < Convert.ToInt32(value); i++)
                    {
                        sno_v++;
                        dt.Rows.Add(Convert.ToString(sno_v), "", "");
                    }

                }

            }
            else
            {
                string selectQ = "select Start_session,Endsession from Enrollmentsession where EnrollFK='" + PK + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQ, "Text");
                // int count = 0;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                    {
                        sno_v++;
                        dt.Rows.Add(Convert.ToString(sno_v), Convert.ToString(ds.Tables[0].Rows[sel]["Start_session"]), Convert.ToString(ds.Tables[0].Rows[sel]["Endsession"]));

                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                Sessiongrid.DataSource = dt;
                Sessiongrid.DataBind();
                Sessiongrid.Visible = true;
            }
        }
        catch
        {

        }
    }

    //added by sudhagar
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            bool check = false;
            string resid = "";
            if (rbresid.Checked == true)
                resid = "0";
            else
                resid = "1";

            string start_date = Convert.ToString(txt_startdate.Text);
            string end_date = Convert.ToString(txt_enddate.Text);

            string[] splitdaate = start_date.Split('/');
            DateTime s_date = Convert.ToDateTime(splitdaate[1] + "/" + splitdaate[0] + "/" + splitdaate[2]);

            string[] splitenddaate = end_date.Split('/');
            DateTime en_date = Convert.ToDateTime(splitenddaate[1] + "/" + splitenddaate[0] + "/" + splitenddaate[2]);

            string numofstud = Convert.ToString(txt_Noofseat.Text);

            string numofsess = Convert.ToString(txt_noofsession.Text);

            if (btnsave.Text == "Save")
            {
                if (Convert.ToString(s_date).Trim() != "" && Convert.ToString(en_date).Trim() != "" && numofstud.Trim() != "" && numofsess.Trim() != "")
                {
                    while (s_date <= en_date)
                    {
                        if (s_date.ToString("dddd") != "Sunday")
                        {
                            String InsertQ = " if exists (select * from Stud_Enrollment where isEntrolltype='" + resid + "' and Date='" + s_date + "' )update Stud_Enrollment set TotalNoPerDays='" + numofstud + "',Noofsession='" + numofsess + "' where isEntrolltype='" + resid + "' and Date='" + s_date + "' else insert into Stud_Enrollment (isEntrolltype,Date,TotalNoPerDays,Noofsession) values('" + resid + "','" + s_date + "','" + numofstud + "','" + numofsess + "')";
                            int instU = d2.update_method_wo_parameter(InsertQ, "Text");
                            check = true;
                            string EnrollPK = d2.GetFunction("select EnrollPK from Stud_Enrollment where isEntrolltype='" + resid + "' and Date='" + s_date + "'");
                            if (EnrollPK != "" && EnrollPK != "0")
                            {
                                if (Sessiongrid.Rows.Count > 0)
                                {
                                    for (int r = 0; r < Sessiongrid.Rows.Count; r++)
                                    {
                                        string sno = ((Sessiongrid.Rows[r].FindControl("snolbl") as Label).Text);
                                        string startsession = ((Sessiongrid.Rows[r].FindControl("txt_starttime") as TextBox).Text);
                                        string endsession = ((Sessiongrid.Rows[r].FindControl("txt_endtime") as TextBox).Text);
                                        if (sno.Trim() != "" && startsession.Trim() != "" && endsession.Trim() != "")
                                        {
                                            string insetquery = " insert into Enrollmentsession (EnrollFK,Start_session,Endsession) values('" + EnrollPK + "','" + startsession + "','" + endsession + "')";
                                            int inst = d2.update_method_wo_parameter(insetquery, "Text");
                                            check = true;
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            check = true;
                        }
                        s_date = s_date.AddDays(1);
                    }
                    if (check == true)
                    {
                        Sessiongrid.Visible = false;
                        txt_Noofseat.Text = "";
                        txt_noofsession.Text = "";
                        rbresid.Checked = true;
                        rbnotresid.Checked = false;
                        txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        subdiv.Visible = false;
                        btnEnrollset_Click(sender, e);
                        errorspan.InnerHtml = "Saved Successfully";
                        poperrjs.Visible = true;
                    }
                    else
                    {
                        errorspan.InnerHtml = "Please Avoid Sundays!";
                        poperrjs.Visible = true;
                    }
                }
                else
                {
                    errorspan.InnerHtml = "Please Fill the Values";
                    poperrjs.Visible = true;
                }
            }
            else
            {
                string enrollpk = "";
                if (Session["pk"] != null)
                {
                    enrollpk = Convert.ToString(Session["pk"]);
                    String InsertQ = " update Stud_Enrollment set TotalNoPerDays='" + numofstud + "',Noofsession='" + numofsess + "' where EnrollPK='" + enrollpk + "' ";
                    int instU = d2.update_method_wo_parameter(InsertQ, "Text");
                    if (Sessiongrid.Rows.Count > 0)
                    {
                        String DelSess = " delete from Enrollmentsession where EnrollFK='" + enrollpk + "'";
                        int Del = d2.update_method_wo_parameter(DelSess, "Text");
                        for (int r = 0; r < Sessiongrid.Rows.Count; r++)
                        {
                            string sno = ((Sessiongrid.Rows[r].FindControl("snolbl") as Label).Text);
                            string startsession = ((Sessiongrid.Rows[r].FindControl("txt_starttime") as TextBox).Text);
                            string endsession = ((Sessiongrid.Rows[r].FindControl("txt_endtime") as TextBox).Text);
                            if (sno.Trim() != "" && startsession.Trim() != "" && endsession.Trim() != "")
                            {
                                string insetquery = " insert into Enrollmentsession (EnrollFK,Start_session,Endsession) values('" + enrollpk + "','" + startsession + "','" + endsession + "')";
                                int inst = d2.update_method_wo_parameter(insetquery, "Text");
                                check = true;
                            }
                        }
                    }
                    else
                    {
                        errorspan.InnerHtml = "Please Fill The Session";
                        poperrjs.Visible = true;
                    }
                    if (check == true)
                    {
                        Sessiongrid.Visible = false;
                        txt_Noofseat.Text = "";
                        txt_noofsession.Text = "";
                        rbresid.Checked = true;
                        txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        subdiv.Visible = false;
                        btnEnrollset_Click(sender, e);
                        errorspan.InnerHtml = "Updated Successfully";
                        poperrjs.Visible = true;
                    }

                }
                else
                {
                    errorspan.InnerHtml = "Please Fill the Values";
                    poperrjs.Visible = true;
                }

            }
        }
        catch
        {

        }
    }

    protected void cbsetting_change(object sender, EventArgs e)
    {
        try
        {
            //if (cbsetting.Checked == true)
            //{
            if (cbapply.Checked == true)
            {
                subdiv.Visible = true;
                DataSet ds1 = new DataSet();
                string selectqury = "select session ,start_session ,end_session  from enrollmentsession ";
                selectqury = selectqury + " select CONVERT(varchar(20), value,103) as value from Master_Settings where settings ='Enrollmentstartdate' and usercode ='" + user_code + "'";
                selectqury = selectqury + " select * from Master_Settings where settings ='Enrollmentnoofseat' and usercode ='" + user_code + "'";
                selectqury = selectqury + " select * from Master_Settings where settings ='Enrollmentnoofsession' and usercode ='" + user_code + "'";
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(selectqury, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable d_date = new DataTable();
                    d_date.Columns.Add("S.No", typeof(string));
                    d_date.Columns.Add("Start Session", typeof(string));
                    d_date.Columns.Add("End Session", typeof(string));
                    for (int h = 0; h < ds1.Tables[0].Rows.Count; h++)
                    {
                        d_date.Rows.Add("", Convert.ToString(ds1.Tables[0].Rows[h]["start_session"]), Convert.ToString(ds1.Tables[0].Rows[h]["end_session"]));
                    }
                    if (d_date.Rows.Count > 0)
                    {
                        Sessiongrid.DataSource = d_date;
                        Sessiongrid.DataBind();
                        Sessiongrid.Visible = true;
                        for (int d = 0; d < d_date.Rows.Count; d++)
                        {
                            string startsession = Convert.ToString(d_date.Rows[d]["Start Session"]);
                            string endsession = Convert.ToString(d_date.Rows[d]["End Session"]);
                            ((Sessiongrid.Rows[d].FindControl("txt_starttime") as TextBox).Text) = Convert.ToString(startsession);
                            ((Sessiongrid.Rows[d].FindControl("txt_endtime") as TextBox).Text) = Convert.ToString(endsession);

                        }

                    }
                }
                if (ds1.Tables[1].Rows.Count > 0)
                {
                    string stardate = Convert.ToString(ds1.Tables[1].Rows[0]["value"]);
                    string[] splitdate = stardate.Split('/');
                    DateTime dh = Convert.ToDateTime(splitdate[0] + "/" + splitdate[1] + "/" + splitdate[2]);
                    txt_startdate.Text = Convert.ToString(dh.ToString("dd/MM/yyyy"));
                }
                else
                {
                    txt_startdate.Text = "";
                }
                if (ds1.Tables[2].Rows.Count > 0)
                {
                    txt_Noofseat.Text = Convert.ToString(ds1.Tables[2].Rows[0]["value"]);
                }
                else
                {
                    txt_Noofseat.Text = "";
                }
                if (ds1.Tables[3].Rows.Count > 0)
                {
                    txt_noofsession.Text = Convert.ToString(ds1.Tables[3].Rows[0]["value"]);
                }
                else
                {
                    txt_noofsession.Text = "";
                }
            }
            if (cbnotapply.Checked == true)
            {
                subdiv.Visible = true;
                DataSet ds1 = new DataSet();
                string selectqury = "select session ,start_session ,end_session  from enrollmentsession ";
                selectqury = selectqury + " select CONVERT(varchar(20), value,103) as value from Master_Settings where settings ='Enrollmentstartdatenon' and usercode ='" + user_code + "'";
                selectqury = selectqury + " select * from Master_Settings where settings ='Enrollmentnoofseatnon' and usercode ='" + user_code + "'";
                selectqury = selectqury + " select * from Master_Settings where settings ='Enrollmentnoofsessionnon' and usercode ='" + user_code + "'";
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(selectqury, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable d_date = new DataTable();
                    d_date.Columns.Add("S.No", typeof(string));
                    d_date.Columns.Add("Start Session", typeof(string));
                    d_date.Columns.Add("End Session", typeof(string));
                    for (int h = 0; h < ds1.Tables[0].Rows.Count; h++)
                    {
                        d_date.Rows.Add("", Convert.ToString(ds1.Tables[0].Rows[h]["start_session"]), Convert.ToString(ds1.Tables[0].Rows[h]["end_session"]));
                    }
                    if (d_date.Rows.Count > 0)
                    {
                        Sessiongrid.DataSource = d_date;
                        Sessiongrid.DataBind();
                        Sessiongrid.Visible = true;
                        for (int d = 0; d < d_date.Rows.Count; d++)
                        {
                            string startsession = Convert.ToString(d_date.Rows[d]["Start Session"]);
                            string endsession = Convert.ToString(d_date.Rows[d]["End Session"]);
                            ((Sessiongrid.Rows[d].FindControl("txt_starttime") as TextBox).Text) = Convert.ToString(startsession);
                            ((Sessiongrid.Rows[d].FindControl("txt_endtime") as TextBox).Text) = Convert.ToString(endsession);

                        }

                    }
                }
                if (ds1.Tables[1].Rows.Count > 0)
                {
                    string stardate = Convert.ToString(ds1.Tables[1].Rows[0]["value"]);
                    string[] splitdate = stardate.Split('/');
                    DateTime dh = Convert.ToDateTime(splitdate[0] + "/" + splitdate[1] + "/" + splitdate[2]);
                    txt_startdate.Text = Convert.ToString(dh.ToString("dd/MM/yyyy"));
                }
                else
                {
                    txt_startdate.Text = "";
                }
                if (ds1.Tables[2].Rows.Count > 0)
                {
                    txt_Noofseat.Text = Convert.ToString(ds1.Tables[2].Rows[0]["value"]);
                }
                else
                {
                    txt_Noofseat.Text = "";
                }
                if (ds1.Tables[3].Rows.Count > 0)
                {
                    txt_noofsession.Text = Convert.ToString(ds1.Tables[3].Rows[0]["value"]);
                }
                else
                {
                    txt_noofsession.Text = "";
                }
            }
            //}
            //if (cbsetting.Checked == false)
            //{
            //    subdiv.Visible = false;
            //}
        }
        catch
        {

        }
    }

    protected void fpspread_Command(object sender, EventArgs e)
    {
        try
        {
            // fpspread.SaveChanges();
            int value = 0;
            string activerow = fpspread.ActiveSheetView.ActiveRow.ToString();
            string activecol = fpspread.ActiveSheetView.ActiveColumn.ToString();
            if (activerow == "0" && activecol == "1")
            {
                value = Convert.ToInt32(fpspread.Sheets[0].Cells[0, 1].Value);
                for (int row = 0; row < fpspread.Rows.Count; row++)
                {
                    if (value == 1)
                    {
                        fpspread.Sheets[0].Cells[row, 1].Value = 1;
                    }
                    else
                    {
                        fpspread.Sheets[0].Cells[row, 1].Value = 0;
                    }
                }
            }

        }
        catch
        {

        }
    }

    protected void cbx_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbx.Checked == true)
            {
                if (fpspread.Sheets[0].RowCount > 0)
                {
                    for (int row = 0; row < fpspread.Rows.Count; row++)
                    {
                        fpspread.Sheets[0].Cells[row, 1].Value = 1;
                    }
                }
                fpspread.SaveChanges();
            }
            if (cbx.Checked == false)
            {
                if (fpspread.Sheets[0].RowCount > 0)
                {
                    for (int row = 0; row < fpspread.Rows.Count; row++)
                    {
                        fpspread.Sheets[0].Cells[row, 1].Value = 0;
                    }
                }
                fpspread.SaveChanges();
            }
        }
        catch
        {

        }
    }


    public void loadprint()
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc;
            Gios.Pdf.PdfDocument mydocnew = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Font Fontbold = new Font("Book Antiqua", 18, FontStyle.Regular);
            Font fbold = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 11, FontStyle.Regular);
            Font fontname = new Font("Book Antiqua", 11, FontStyle.Bold);
            Font fontmedium = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontmediumb = new Font("Book Antiqua", 13, FontStyle.Bold);
            Boolean saveflag = false;
            //string sign = "principal" + ddlcollege.SelectedValue.ToString() + "";
            DataSet d_value = new DataSet();
            string strquery = "select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string Collegename = "";
            string aff = "";
            string address = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                aff = "(Affiliated to the " + ds.Tables[0].Rows[0]["university"].ToString() + ")";
                address = ds.Tables[0].Rows[0]["address1"].ToString() + " , " + ds.Tables[0].Rows[0]["district"].ToString() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString();
            }

            fpspread.SaveChanges();
            for (int i = 0; i < fpspread.Sheets[0].Rows.Count; i++)
            {
                int isval = 0;
                isval = Convert.ToInt32(fpspread.Sheets[0].Cells[i, 1].Value);
                if (isval == 1)
                {

                    mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                    saveflag = true;
                    string rollno = Convert.ToString(fpspread.Sheets[0].Cells[i, 0].Tag);
                    if (rollno.Trim() != "")
                    {
                        string name = Convert.ToString(fpspread.Sheets[0].Cells[i, 3].Text);
                        string deptname = Convert.ToString(fpspread.Sheets[0].Cells[i, 4].Text);
                        string[] splitdptname = deptname.Split('-');
                        string deprt = Convert.ToString(splitdptname[0]);
                        string course = Convert.ToString(splitdptname[1]);
                        string date = Convert.ToString(fpspread.Sheets[0].Cells[i, 5].Text);
                        string session = Convert.ToString(fpspread.Sheets[0].Cells[i, 6].Text);
                        string type = Convert.ToString(fpspread.Sheets[0].Cells[i, 2].Tag);
                        if (type.Trim() == "DAY")
                        {
                            type = "Aided Stream (DAY)";
                        }
                        else
                        {
                            type = "SFS (Evening)";
                        }
                        if (session != "")
                        {
                            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                            Gios.Pdf.PdfPage mypdfpage1 = mydocnew.NewPage();
                            int ik = 1;
                            string[] splitdate = date.Split('/');
                            DateTime dt_date = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                            //string updatequery = "update applyn set admitcard_date ='" + dt_date.ToString("MM/dd/yyyy") + "' where app_formno ='" + rollno + "'";
                            //int d = d2.update_method_wo_parameter(updatequery, "Text");
                            //while (ik <= 3)
                            //{
                            //    dt_date = dt_date.AddDays(1);
                            //    if (dt_date.ToString("dddd") == "Sunday")
                            //    {
                            //        dt_date = dt_date.AddDays(1);
                            //    }
                            //    ik++;
                            //}

                            string sign = "principal" + Convert.ToString(Session["collegecode"]) + "";

                            string mail_id = "";
                            string stud_phoneno = "";
                            string mailidquery = "select StuPer_Id,Student_Mobile  from applyn where app_formno ='" + rollno + "'";
                            d_value.Clear();
                            d_value = d2.select_method_wo_parameter(mailidquery, "Text");
                            if (d_value.Tables[0].Rows.Count > 0)
                            {
                                mail_id = Convert.ToString(d_value.Tables[0].Rows[0]["StuPer_Id"]);
                                stud_phoneno = Convert.ToString(d_value.Tables[0].Rows[0]["Student_Mobile"]);
                            }
                            string upadte = "update applyn set enrollmentcard ='1', enrollment_card_date='" + dt_date.ToString("MM/dd/yyyy") + "',enrollment_session='" + session + "' where app_formno='" + rollno + "'";
                            int a = d2.update_method_wo_parameter(upadte, "Text");

                            int xvlaue = 40;

                            PdfArea tete = new PdfArea(mydoc, 10, 10, 570, 820);

                            PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                            mypdfpage.Add(pr1);
                            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 150, 20, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, Collegename + " (Autonomous)");
                            mypdfpage.Add(ptc);

                            PdfTextArea ptc01 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 190, 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, address);
                            mypdfpage.Add(ptc01);
                            PdfTextArea ptc02 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 180, 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, aff);
                            mypdfpage.Add(ptc02);

                            int year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));

                            PdfTextArea ptc0265 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 100, 120, 400, 30),
System.Drawing.ContentAlignment.MiddleCenter, "ENROLLMENT CARD " + year + " - " + Convert.ToInt32(year + 1) + "");
                            mypdfpage.Add(ptc0265);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 25, 25, 300);
                            }

                            PdfTextArea ptc07 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, xvlaue, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "MCC ID");
                            mypdfpage.Add(ptc07);

                            PdfTextArea ptc07ap = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 100, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + rollno.ToString() + "");
                            mypdfpage.Add(ptc07ap);

                            PdfTextArea ptc078 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                      new PdfArea(mydoc, 350, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Stream");
                            mypdfpage.Add(ptc078);

                            PdfTextArea ptc07ap9 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, 400, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + type.ToString() + "");
                            mypdfpage.Add(ptc07ap9);

                            //string[] spdeg = lbldegree.Text.ToString().Split('-');

                            PdfTextArea ptc071 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, xvlaue, 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class");
                            mypdfpage.Add(ptc071);

                            PdfTextArea ptc071a = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                              new PdfArea(mydoc, 100, 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + deprt.ToString() + "");
                            mypdfpage.Add(ptc071a);
                            PdfTextArea ptc08 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, xvlaue, 180, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name");
                            mypdfpage.Add(ptc08);
                            PdfTextArea ptc08na = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 100, 180, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + name.ToString() + "");
                            mypdfpage.Add(ptc08na);
                            PdfTextArea ptc081 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                              new PdfArea(mydoc, xvlaue, 220, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Group");
                            mypdfpage.Add(ptc081);
                            PdfTextArea ptc081a = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                              new PdfArea(mydoc, 100, 220, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":" + course.ToString() + "");
                            mypdfpage.Add(ptc081a);

                            PdfTextArea ptc093 = null;

                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, xvlaue, 240, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Welcome to the family of Madras Christian College.");
                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, xvlaue, 260, 450, 40), System.Drawing.ContentAlignment.MiddleLeft, "Fill the Enrollment form available in the link 'Enrollment' in the college website.");
                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue, 290, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date : " + dt_date.ToString("dd/MM/yyyy") + "                      Time : " + session.ToString() + ".");

                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue, 310, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "Venue: EXAM OFFICE GALLERY.");
                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);

                            //ptc093 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydoc, xvlaue + 20, 320, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "");
                            //mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue, 330, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Parent / local guardian must accompany the student at the time of enrollment.");

                            int collvalue = 330;
                            mypdfpage.Add(ptc093);
                            //  mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue, collvalue + 20, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "The following documents must be submitted at the enrollment desk:");
                            collvalue = collvalue + 20;
                            mypdfpage.Add(ptc093);
                            //  mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Admission card");
                            collvalue = collvalue + 20;
                            mypdfpage.Add(ptc093);
                            //  mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 450, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Original & 2 Photocopies of the fee receipt.");
                            collvalue = collvalue + 20;
                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Original & 2 Photocopies of +2 / U.G / P.G Mark sheet");
                            collvalue = collvalue + 20;
                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Original Transfer & Conduct certificate");
                            collvalue = collvalue + 20;
                            mypdfpage.Add(ptc093);
                            //  mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Original & 2 Photocopies of Community Certificate");
                            collvalue = collvalue + 20;
                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "* 1 stamp size & 2 passport size photographs");
                            collvalue = collvalue + 25;
                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 450, 40), System.Drawing.ContentAlignment.MiddleLeft, "Provisional eligibility cerificate obtained from the University of Madras along with 2 photocopies [for candidates from Boards other than Tamil Nadu Higher Secondary Board(Regular / Instant / Private) /CBSE /ISCE]");

                            collvalue = collvalue + 45;
                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 40), System.Drawing.ContentAlignment.MiddleLeft, "Provisional eligibility cerificate obtained from the University of Madras along with 2 photocopies (For PG candidates from universities other than University of Madras)");
                            collvalue = collvalue + 40;
                            mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);
                            //ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree cerificate / Provisional certificate / Course completion cerificate - for PG admission");
                            //collvalue = collvalue + 20;
                            //mypdfpage.Add(ptc093);
                            ////  mypdfpage1.Add(ptc093);
                            //if (cbnotapply.Checked == true)
                            //{
                            //    ptc093 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                            //                                                new PdfArea(mydoc, xvlaue, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Enrollment of Resident (Hostel) students");
                            //    collvalue = collvalue + 20;
                            //    mypdfpage.Add(ptc093);
                            //    //  mypdfpage1.Add(ptc093);
                            //    ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                                new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "After paying the College fee at the Bank, meet the Dean of student Affairs for Hall allotment");
                            //    collvalue = collvalue + 20;
                            //    mypdfpage.Add(ptc093);
                            //    //  mypdfpage1.Add(ptc093);
                            //    ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                                new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Pay the Hall fee at the office of the Hall allotted");
                            //    collvalue = collvalue + 20;
                            //    mypdfpage.Add(ptc093);
                            //    // mypdfpage1.Add(ptc093);
                            //}
                            //ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note: No change of data for enrollment will be granted");
                            //collvalue = collvalue + 30;
                            //mypdfpage.Add(ptc093);
                            //// mypdfpage1.Add(ptc093);
                            //ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydoc, xvlaue, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Wishing you an enjoyable academic life on MCC campus");
                            //collvalue = collvalue + 20;
                            //mypdfpage.Add(ptc093);
                            // mypdfpage1.Add(ptc093);
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                ds.Dispose();
                                ds.Reset();
                                ds = d2.select_method_wo_parameter("select principal_sign from collinfo where college_code='" + Session["collegecode"] + "' and principal_sign is not null", "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["principal_sign"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                mypdfpage.Add(LogoImage, 450, 740, 400);
                            }

                            PdfTextArea ptc82 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 400, 800, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "PRINCIPAL & SECRETARY");

                            mypdfpage.Add(ptc82);
                            // mypdfpage.Add(ptc7);

                            mypdfpage.SaveToDocument();


                            PdfArea tete1 = new PdfArea(mydocnew, 10, 10, 570, 820);

                            PdfRectangle pr11 = new PdfRectangle(mydocnew, tete, Color.Black);
                            mypdfpage1.Add(pr11);
                            PdfTextArea ptc11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocnew, 150, 20, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, Collegename + " (Autonomous)");
                            mypdfpage1.Add(ptc11);

                            PdfTextArea ptc011 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocnew, 190, 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, address);
                            mypdfpage1.Add(ptc011);
                            PdfTextArea ptc021 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocnew, 180, 50, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, aff);
                            mypdfpage1.Add(ptc021);
                            year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                            PdfTextArea ptc02651 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocnew, 100, 120, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "ENROLLMENT CARD " + year + " - " + Convert.ToInt32(year + 1) + "");
                            mypdfpage1.Add(ptc02651);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                            {
                                PdfImage LogoImage = mydocnew.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage1.Add(LogoImage, 25, 25, 300);
                            }

                            PdfTextArea ptc0718 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocnew, xvlaue, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "MCC ID");
                            mypdfpage1.Add(ptc0718);

                            PdfTextArea ptc07ap1 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocnew, 100, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + rollno.ToString() + "");
                            mypdfpage1.Add(ptc07ap1);

                            PdfTextArea ptc07845 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocnew, 350, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Stream");
                            mypdfpage1.Add(ptc07845);

                            PdfTextArea ptc07ap956 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocnew, 400, 160, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + type.ToString() + "");
                            mypdfpage1.Add(ptc07ap956);

                            //string[] spdeg = lbldegree.Text.ToString().Split('-');

                            PdfTextArea ptc0711 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocnew, xvlaue, 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Class");
                            mypdfpage1.Add(ptc071);

                            PdfTextArea ptc071a1 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocnew, 100, 200, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + deprt.ToString() + "");
                            mypdfpage1.Add(ptc071a1);
                            PdfTextArea ptc0811 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocnew, xvlaue, 180, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name");
                            mypdfpage1.Add(ptc0811);
                            PdfTextArea ptc08111 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocnew, 100, 180, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + name.ToString() + "");
                            mypdfpage1.Add(ptc08111);
                            PdfTextArea ptc08114 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocnew, xvlaue, 220, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Group");
                            mypdfpage1.Add(ptc08114);
                            PdfTextArea ptc081a5 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocnew, 100, 220, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ":" + course.ToString() + "");
                            mypdfpage1.Add(ptc081a5);

                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, xvlaue, 240, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Welcome to the family of Madras Christian College.");
                            // mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                               new PdfArea(mydoc, xvlaue, 260, 450, 40), System.Drawing.ContentAlignment.MiddleLeft, "Fill the Enrollment form available in the link 'Enrollment' in the college website.");
                            // mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue, 290, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date : " + dt_date.ToString("dd/MM/yyyy") + "                      Time : " + session.ToString() + ".");

                            //mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue, 310, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "Venue: EXAM OFFICE GALLERY.");
                            //mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);

                            //ptc093 = new PdfTextArea(fontname, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydoc, xvlaue + 20, 320, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "You have to give additional particulars at the college website before the date of Enrollment.");
                            //// mypdfpage.Add(ptc093);
                            //mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue, 330, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Parent / local guardian must accompany the student at the time of enrollment.");
                            collvalue = 0;
                            collvalue = 330;
                            //mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue, collvalue + 20, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "The following documents must be submitted at the enrollment desk:");
                            collvalue = collvalue + 20;
                            //mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Admission card");
                            collvalue = collvalue + 20;
                            // mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 450, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Original & 2 Photocopies of the fee receipt.");
                            collvalue = collvalue + 20;
                            //mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Original & 2 Photocopies of +2 / U.G / P.G Mark sheet");
                            collvalue = collvalue + 20;
                            // mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Original Transfer & Conduct certificate");
                            collvalue = collvalue + 20;
                            // mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "* Original & 2 Photocopies of Community Certificate");
                            collvalue = collvalue + 20;
                            // mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "* 1 stamp size & 2 passport size photographs");
                            collvalue = collvalue + 25;
                            // mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);
                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 450, 40), System.Drawing.ContentAlignment.MiddleLeft, "Provisional eligibility cerificate obtained from the University of Madras along with 2 photocopies [for candidates from Boards other than Tamil Nadu Higher Secondary Board(Regular / Instant / Private) /CBSE /ISCE]");

                            collvalue = collvalue + 45;
                            // mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);

                            ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 40), System.Drawing.ContentAlignment.MiddleLeft, "Provisional eligibility cerificate obtained from the University of Madras along with 2 photocopies (For PG candidates from universities other than University of Madras)");
                            collvalue = collvalue + 40;
                            //mypdfpage.Add(ptc093);
                            mypdfpage1.Add(ptc093);
                            //ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree cerificate / Provisional certificate / Course completion cerificate - for PG admission");
                            //collvalue = collvalue + 20;
                            //// mypdfpage.Add(ptc093);
                            //mypdfpage1.Add(ptc093);
                            //if (cbnotapply.Checked == true)
                            //{
                            //    ptc093 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                            //                                                new PdfArea(mydoc, xvlaue, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Enrollment of Resident (Hostel) students");
                            //    collvalue = collvalue + 20;
                            //    // mypdfpage.Add(ptc093);
                            //    mypdfpage1.Add(ptc093);
                            //    ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                                new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "After paying the College fee at the Bank, meet the Dean of student Affairs for Hall allotment");
                            //    collvalue = collvalue + 20;
                            //    // mypdfpage.Add(ptc093);
                            //    mypdfpage1.Add(ptc093);
                            //    ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                                new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Pay the Hall fee at the office of the Hall allotted");
                            //    collvalue = collvalue + 20;
                            //    //mypdfpage.Add(ptc093);
                            //    mypdfpage1.Add(ptc093);
                            //}
                            //ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydoc, xvlaue + 50, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note: No change of data for enrollment will be granted");
                            //collvalue = collvalue + 30;
                            //// mypdfpage.Add(ptc093);
                            //mypdfpage1.Add(ptc093);
                            //ptc093 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                            new PdfArea(mydoc, xvlaue, collvalue + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Wishing you an enjoyable academic life on MCC campus");
                            //collvalue = collvalue + 20;
                            ////mypdfpage.Add(ptc093);
                            //mypdfpage1.Add(ptc093);



                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                ds.Dispose();
                                ds.Reset();
                                ds = d2.select_method_wo_parameter("select principal_sign from collinfo where college_code='" + Session["collegecode"] + "' and principal_sign is not null", "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["principal_sign"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                            {
                                PdfImage LogoImage = mydocnew.NewImage(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg"));
                                mypdfpage1.Add(LogoImage, 450, 740, 400);
                            }

                            PdfTextArea ptc828 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocnew, 400, 800, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "PRINCIPAL & SECRETARY");

                            mypdfpage1.Add(ptc828);

                            // mypdfpage1.Add(ptc78);
                            mypdfpage1.SaveToDocument();
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                Response.Buffer = true;
                                Response.Clear();
                                string szPath = appPath + "/Report/";
                                string szFile = "" + rollno + ".pdf";
                                mydoc.SaveToFile(szPath + szFile);
                                //  mydocnew.SaveToFile(szPath + szFile);
                                //Response.ClearHeaders();
                                //Response.ClearHeaders();
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                //Response.ContentType = "application/pdf";
                                //Response.WriteFile(szPath + szFile);
                            }
                            string appPath1 = HttpContext.Current.Server.MapPath("~");
                            if (appPath1 != "")
                            {
                                Response.Buffer = true;
                                Response.Clear();
                                string szPath = appPath + "/Report/";
                                string szFile = "Report" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                                // mydoc.SaveToFile(szPath + szFile);
                                mydocnew.SaveToFile(szPath + szFile);
                                Response.ClearHeaders();
                                Response.ClearHeaders();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(szPath + szFile);
                            }
                            sendmail(mail_id, name, rollno);
                            //sendsms(stud_phoneno, rollno, date, session);
                            //12.07.16
                            string ssr = "select * from Track_Value where college_code='" + Convert.ToString(Session["collegecode"]) + "'";
                            ds.Clear();
                            string user_id = "";
                            ds = d2.select_method_wo_parameter(ssr, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                            }
                            string Msg = "You have to come for enrollment on " + date + " during " + session + " at the college. Please refer your Email-ID for Enrollment Details";
                            int sentsmscount = d2.send_sms(user_id, Convert.ToString(Session["collegecode"]), user_code, stud_phoneno, Msg, "0");
                        }
                    }

                }
            }
            // FpSpread4.SaveChanges();
            if (saveflag == true)
            {
                if (cbapply.Checked == true)
                {
                    string updateqeury = "update Master_Settings set value ='" + Convert.ToString(Session["Datevalue"]) + "' where settings ='Enrollmentstartdate' and usercode ='" + user_code + "'";
                    int upd = d2.update_method_wo_parameter(updateqeury, "Text");
                }
                if (cbnotapply.Checked == true)
                {
                    string updateqeury = "update Master_Settings set value ='" + Convert.ToString(Session["Datevalue"]) + "' where settings ='Enrollmentstartdatenon' and usercode ='" + user_code + "'";
                    int upd = d2.update_method_wo_parameter(updateqeury, "Text");
                }
                //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Admit Card Generate Generate Successfully\");", true);
                //errorspan.InnerHtml = "Admit Card Generate Generate Successfully";
                //poperrjs.Visible = true;
            }

        }
        catch (Exception ex)
        {
            errorlable.Visible = true;
            errorlable.Text = Convert.ToString(ex);
        }
    }

    public void sendmail(string mail, string name, string app)
    {
        try
        {
            string send_mail = "";
            string send_pw = "";
            string to_mail = Convert.ToString(mail);
            // string bodytext = "Hi Boy";
            string subtext = "MCC Enrollment-Regarding";
            string strstuname = Convert.ToString(name);

            string strquery = "select massemail,masspwd from collinfo where college_code = " + Session["collegecode"] + " ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                send_mail = Convert.ToString(ds.Tables[0].Rows[0]["massemail"]);
                send_pw = Convert.ToString(ds.Tables[0].Rows[0]["masspwd"]);
            }
            if (send_mail.Trim() != "" && send_pw.Trim() != "" && to_mail.Trim() != "")
            {
                SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mailmsg = new MailMessage();
                MailAddress mfrom = new MailAddress(send_mail);
                mailmsg.From = mfrom;
                mailmsg.To.Add(to_mail);
                mailmsg.Subject = subtext;
                mailmsg.IsBodyHtml = true;
                // mailmsg.Body = "Hi";
                mailmsg.Body = mailmsg.Body + strstuname;
                mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = app + ".pdf";
                    string attachementpath = szPath + szFile;
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/Report/" + szFile + "")))
                    {
                        Attachment data = new Attachment(attachementpath);
                        mailmsg.Attachments.Add(data);
                    }
                }
                Mail.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                Mail.UseDefaultCredentials = false;
                Mail.Credentials = credentials;
                Mail.Send(mailmsg);
            }
        }
        catch
        {

        }
    }

    //public void sendsms(string number, string app, string date, string session)
    //{
    //    try
    //    {
    //        int ik = 1;
    //        DateTime dt_date = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
    //        //while (ik <= 2)
    //        //{
    //        //    dt_date = dt_date.AddDays(1);
    //        //    if (dt_date.ToString("dddd") == "Sunday")
    //        //    {
    //        //        dt_date = dt_date.AddDays(1);
    //        //    }
    //        //    ik++;
    //        //}
    //        string Msg = "You have to come for enrollment on " + date + " during " + session + " at the college. Please refer your Email-ID for Enrollment Details";
    //        string Mobile_no = Convert.ToString(number);
    //        string user_id = "";
    //        string SenderID = "";
    //        string Password = "";
    //        string todaydate = System.DateTime.Now.ToString("dd/MM/yyyy");
    //        string[] splitdate = todaydate.Split('/');
    //        DateTime dt1 = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
    //        string ssr = "select * from Track_Value where college_code='" + Session["collegecode"] + "'";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(ssr, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
    //        }

    //        if (user_id.Trim() != "")
    //        {
    //            string getval = d2.GetUserapi(user_id);
    //            string[] spret = getval.Split('-');
    //            if (spret.GetUpperBound(0) == 1)
    //            {

    //                SenderID = spret[0].ToString();
    //                Password = spret[0].ToString();

    //            }
    //            string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + Mobile_no + "&text=" + Msg + "&priority=ndnd&stype=normal";
    //            string isst = "0";

    //            smsreport(strpath, isst, dt1, Mobile_no, Msg);
    //        }

    //    }
    //    catch
    //    {

    //    }
    //}

    //public void smsreport(string uril, string isstaff, DateTime dt1, string phone, string msg)
    //{
    //    try
    //    {
    //        string phoneno = phone;
    //        string message = msg;
    //        string date = dt1.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
    //        WebRequest request = WebRequest.Create(uril);
    //        WebResponse response = request.GetResponse();
    //        Stream data = response.GetResponseStream();
    //        StreamReader sr = new StreamReader(data);
    //        string strvel = sr.ReadToEnd();
    //        string groupmsgid = "";
    //        groupmsgid = strvel;
    //        int sms = 0;
    //        string smsreportinsert = "";

    //        smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date)values( '" + phoneno + "','" + groupmsgid + "','" + message + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "')";
    //        sms = d2.update_method_wo_parameter(smsreportinsert, "Text");

    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //}

    protected void btnenrolment_Click(object sender, EventArgs e)
    {
        try
        {
            loadprint();
        }
        catch
        {

        }

    }

    //protected void saveEnrollment()
    //{
    //    try
    //    {
    //        int value = 0;
    //        for (int sel = 0; sel < fpspread.Sheets[0].Rows.Count; sel++)
    //        {
    //            value = Convert.ToInt32(fpspread.Sheets[0].Cells[sel, 1].Value);
    //            if (value==1)
    //            {

    //            }
    //        }
    //    }
    //    catch { }
    //}

    //added by sudhagar
    protected void TabContainer1_Changed(object sender, EventArgs e)
    {
        try
        {
            if (TabContainer1.ActiveTabIndex == 0)
            {
                divsel.Visible = true;
                subdivbase.Visible = false;
                subdiv.Visible = false;
                fpspread.Visible = false;
                FpEnrollGo.Visible = false;
                btnexcel.Visible = false;
                btn_pdf.Visible = false;
                btnenrolment.Visible = false;
                print.Visible = false;
                tblconfrm.Visible = false;
                btn_go.Visible = true;
                printconffm.Visible = false;
                btnConfrmsave.Visible = false;
                txtconfirmdt.Visible = false;
                fpconfrm.Visible = false;
                cbapply.Checked = true;
                cbnotapply.Checked = false;

                lblalert.Text = "";
                txtreport.Text = "";
                lblvalidation1.Text = "";
                txtexcelname.Text = "";

                bindtype();
                bindedulevel();
                degree();
                bindbranch();

            }
            else if (TabContainer1.ActiveTabIndex == 1)
            {
                divsel.Visible = true;
                subdivbase.Visible = false;
                subdiv.Visible = false;
                fpspread.Visible = false;
                FpEnrollGo.Visible = false;
                btnexcel.Visible = false;
                btn_pdf.Visible = false;
                btnenrolment.Visible = false;
                print.Visible = false;
                tblconfrm.Visible = true;
                btn_go.Visible = false;
                printconffm.Visible = false;
                btnConfrmsave.Visible = false;
                txtconfirmdt.Visible = false;
                fpconfrm.Visible = false;
                cbapply.Checked = true;
                cbnotapply.Checked = false;
                bindtype();
                bindedulevel();
                degree();
                bindbranch();
                lblalert.Text = "";
                txtreport.Text = "";
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                ddlenrollconfm.SelectedIndex = 0;
            }
            else if (TabContainer1.ActiveTabIndex == 2)
            {
                divsel.Visible = false;
                subdiv.Visible = false;
                subdivbase.Visible = true;
                fpspread.Visible = false;
                FpEnrollGo.Visible = false;
                rbbaseresid.Checked = true;
                rbbasenotresid.Checked = false;
                btnexcel.Visible = false;
                btn_pdf.Visible = false;
                btnenrolment.Visible = false;
                print.Visible = false;
                tblconfrm.Visible = false;
                btn_go.Visible = false;
                printconffm.Visible = false;
                btnConfrmsave.Visible = false;
                txtconfirmdt.Visible = false;
                fpconfrm.Visible = false;
                lblalert.Text = "";
                txtreport.Text = "";
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
            }
        }
        catch { }
    }

    protected void btnEnrollset_Click(object sender, EventArgs e)
    {
        try
        {
            #region Query

            string enrollmenttype = "";
            if (rbbaseresid.Checked == true)
                enrollmenttype = "0";
            else
                enrollmenttype = "1";


            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            }

            if (fromdate != "" && todate != "")
            {
                String SelectQ = " select EnrollPK,isEntrolltype,CONVERT(varchar(10),Date,103) as Date,TotalNoPerDays,Noofsession from Stud_Enrollment where isEntrolltype='" + enrollmenttype + "' and Date between '" + fromdate + "' and '" + todate + "'";
                SelectQ = SelectQ + " select EnrollFK,Start_session,Endsession from Enrollmentsession";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelectQ, "Text");

            }
            #endregion

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region design

                FpEnrollGo.Sheets[0].RowCount = 0;
                FpEnrollGo.Sheets[0].ColumnCount = 0;
                FpEnrollGo.CommandBar.Visible = false;
                FpEnrollGo.Sheets[0].AutoPostBack = true;
                FpEnrollGo.Sheets[0].ColumnHeader.RowCount = 1;
                FpEnrollGo.Sheets[0].RowHeader.Visible = false;
                FpEnrollGo.Sheets[0].ColumnCount = 6;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpEnrollGo.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpEnrollGo.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpEnrollGo.Sheets[0].Columns[0].Locked = true;

                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Enrollment Type";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpEnrollGo.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                FpEnrollGo.Sheets[0].Columns[1].Locked = true;


                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Date";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpEnrollGo.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                FpEnrollGo.Sheets[0].Columns[2].Locked = true;


                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 3].Text = "TotalPer Day";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpEnrollGo.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;

                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Session Count";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpEnrollGo.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;

                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Session";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpEnrollGo.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpEnrollGo.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;

                FpEnrollGo.Sheets[0].Columns[0].Width = 80;
                FpEnrollGo.Sheets[0].Columns[1].Width = 166;
                FpEnrollGo.Sheets[0].Columns[2].Width = 100;
                FpEnrollGo.Sheets[0].Columns[3].Width = 100;
                FpEnrollGo.Sheets[0].Columns[4].Width = 122;
                FpEnrollGo.Sheets[0].Columns[5].Width = 312;

                #endregion

                #region value

                string session = "";
                string enrolltype = "";
                string sesscount = "";
                string enroll = "";
                for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                {
                    FpEnrollGo.Sheets[0].RowCount++;
                    FpEnrollGo.Sheets[0].Cells[FpEnrollGo.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                    enroll = Convert.ToString(ds.Tables[0].Rows[sel]["isEntrolltype"]);
                    if (Convert.ToBoolean(enroll) == false)
                        enrolltype = "Non Residancy";
                    else
                        enrolltype = "Residancy";
                    FpEnrollGo.Sheets[0].Cells[FpEnrollGo.Sheets[0].RowCount - 1, 1].Text = enrolltype;
                    FpEnrollGo.Sheets[0].Cells[FpEnrollGo.Sheets[0].RowCount - 1, 1].Tag = enroll;
                    FpEnrollGo.Sheets[0].Cells[FpEnrollGo.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[sel]["Date"]);
                    FpEnrollGo.Sheets[0].Cells[FpEnrollGo.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["TotalNoPerDays"]);
                    sesscount = Convert.ToString(ds.Tables[0].Rows[sel]["Noofsession"]);
                    FpEnrollGo.Sheets[0].Cells[FpEnrollGo.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["EnrollPK"]);
                    FpEnrollGo.Sheets[0].Cells[FpEnrollGo.Sheets[0].RowCount - 1, 4].Text = sesscount;
                    DataView dv = new DataView();
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ds.Tables[1].DefaultView.RowFilter = "EnrollFK='" + Convert.ToString(ds.Tables[0].Rows[sel]["EnrollPK"]) + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            for (int k = 0; k < dv.Count; k++)
                            {
                                if (session == "")
                                {
                                    session = Convert.ToString(dv[k]["Start_session"]) + "-" + Convert.ToString(dv[k]["Endsession"]);
                                }
                                else
                                {
                                    session = session + ";" + Convert.ToString(dv[k]["Start_session"]) + "-" + Convert.ToString(dv[k]["Endsession"]);
                                }
                            }
                            FpEnrollGo.Sheets[0].Cells[FpEnrollGo.Sheets[0].RowCount - 1, 5].Text = session;
                            session = "";
                        }
                    }



                }

                #endregion

                #region visible
                FpEnrollGo.Sheets[0].PageSize = FpEnrollGo.Sheets[0].RowCount++;
                FpEnrollGo.SaveChanges();
                FpEnrollGo.Visible = true;
                FpEnrollGo.Height = 380;
                print.Visible = true;
                FpEnrollGo.ShowHeaderSelection = false;

                #endregion
            }
            else
            {
                FpEnrollGo.Visible = false;
                print.Visible = false;
                errorspan.InnerHtml = "No Record Found";
                poperrjs.Visible = true;
            }
        }
        catch { }
    }
    protected void FpEnrollGo_OnCellClick(object sender, EventArgs e)
    {
        try
        { ledgercellclik = true; }
        catch
        { }
    }

    protected void FpEnrollGo_Selectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            if (ledgercellclik == true)
            {
                string actrow = FpEnrollGo.ActiveSheetView.ActiveRow.ToString();
                string actcol = FpEnrollGo.ActiveSheetView.ActiveColumn.ToString();
                if (actrow != "" && actcol != "")
                {
                    int actr = Convert.ToInt32(actrow);
                    int actc = Convert.ToInt32(actcol);
                    string enrollpk = Convert.ToString(FpEnrollGo.Sheets[0].Cells[actr, 0].Tag);
                    string enrolltype = Convert.ToString(FpEnrollGo.Sheets[0].Cells[actr, 1].Text);
                    string date = Convert.ToString(FpEnrollGo.Sheets[0].Cells[actr, 2].Text);
                    string numofstud = Convert.ToString(FpEnrollGo.Sheets[0].Cells[actr, 3].Text);
                    string numofsess = Convert.ToString(FpEnrollGo.Sheets[0].Cells[actr, 4].Text);
                    string session = Convert.ToString(FpEnrollGo.Sheets[0].Cells[actr, 5].Text);

                    if (enrollpk != "")
                    {
                        Session["pk"] = enrollpk;
                        txt_startdate.Text = date;
                        txt_enddate.Text = date;
                        txt_startdate.Enabled = false;
                        txt_enddate.Enabled = false;
                        txt_Noofseat.Text = numofstud;
                        txt_noofsession.Text = numofsess;
                        if (enrolltype == "Non Residancy")
                        {
                            rbresid.Checked = true;
                            rbnotresid.Checked = false;
                        }

                        else
                        {
                            rbnotresid.Checked = true;
                            rbresid.Checked = false;
                        }
                        Bindsessiongrid(enrollpk);
                        btnsave.Text = "Update";
                        subdiv.Visible = true;
                    }


                }
            }
        }
        catch { }

    }

    protected void btnEnrollsetadd_Click(object sender, EventArgs e)
    {
        subdiv.Visible = true;
        Sessiongrid.Visible = false;
        txt_Noofseat.Text = "";
        txt_noofsession.Text = "";
        rbresid.Checked = true;
        rbnotresid.Checked = false;
        txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        btnsave.Text = "Save";
        txt_startdate.Enabled = true;
        txt_enddate.Enabled = true;
    }
    protected void btnEnrollsetUp_Click(object sender, EventArgs e)
    {
        try
        {
            bool update = false;
            FpEnrollGo.SaveChanges();
            for (int sel = 0; sel < FpEnrollGo.Sheets[0].Rows.Count; sel++)
            {
                string enrollPK = Convert.ToString(FpEnrollGo.Sheets[0].Cells[sel, 0].Tag);
                string enrolltype = Convert.ToString(FpEnrollGo.Sheets[0].Cells[sel, 1].Tag);
                string enrolldate = Convert.ToString(FpEnrollGo.Sheets[0].Cells[sel, 2].Text);
                string numofstud = Convert.ToString(FpEnrollGo.Sheets[0].Cells[sel, 3].Value);
                string numofsess = Convert.ToString(FpEnrollGo.Sheets[0].Cells[sel, 4].Value);
                string sessioon = Convert.ToString(FpEnrollGo.Sheets[0].Cells[sel, 5].Value);

                string[] splitdaate = enrolldate.Split('/');
                DateTime s_date = Convert.ToDateTime(splitdaate[1] + "/" + splitdaate[0] + "/" + splitdaate[2]);

                if (enrollPK != "" && enrolltype != "")
                {
                    String DelSess = " delete from Enrollmentsession where EnrollFK='" + enrollPK + "'";
                    int Del = d2.update_method_wo_parameter(DelSess, "Text");
                    if (Del > 0)
                    {
                        String UpdatStud = "update Stud_Enrollment set TotalNoPerDays='" + numofstud + "',Noofsession='" + numofsess + "' where isEntrolltype='" + enrolltype + "' and Date='" + s_date + "' ";
                        int UPStud = d2.update_method_wo_parameter(UpdatStud, "Text");
                        update = true;
                        try
                        {
                            if (sessioon != "")
                            {
                                string[] splitsess = sessioon.Split(';');
                                if (splitsess.Length > 0)
                                {
                                    for (int i = 0; i < splitsess.Length; i++)
                                    {
                                        string[] splitsing = splitsess[i].Split('-');
                                        if (splitsing[0] != "" && splitsing[1] != "")
                                        {
                                            string insetquery = " insert into Enrollmentsession (EnrollFK,Start_session,Endsession) values('" + enrollPK + "','" + splitsing[0] + "','" + splitsing[1] + "')";
                                            int UPSess = d2.update_method_wo_parameter(insetquery, "Text");
                                            update = true;
                                        }
                                    }
                                }
                            }

                        }
                        catch
                        {
                            errorspan.InnerHtml = "Please Enter the Session Correnct Format";
                            poperrjs.Visible = true;
                        }
                    }
                    if (update == true)
                    {
                        btnEnrollset_Click(sender, e);
                        errorspan.InnerHtml = "Updated Successfully";
                        poperrjs.Visible = true;
                    }
                }
                else
                {
                    errorspan.InnerHtml = "Please Enter the Session Correnct Format";
                    poperrjs.Visible = true;
                }
            }
        }
        catch
        { }
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        subdiv.Visible = false;
    }
    protected void rbresid_Changed(object sender, EventArgs e)
    {
        txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_Noofseat.Text = "";
        txt_noofsession.Text = "";
        Sessiongrid.Visible = false;
    }
    protected void rbnotresid_Changed(object sender, EventArgs e)
    {
        txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_Noofseat.Text = "";
        txt_noofsession.Text = "";
        Sessiongrid.Visible = false;
    }
    protected void rbbaseresid_Changed(object sender, EventArgs e)
    {
        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
    protected void rbbasenotresid_Changed(object sender, EventArgs e)
    {
        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
    protected void btnpopup_clcik(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        lblvalidation1.Visible = false;
        string degreedetails = "Enrollment Setting Report";
        string pagename = "Enrollmentselection.aspx";
        Printcontrolhed.loadspreaddetails(FpEnrollGo, pagename, degreedetails);
        Printcontrolhed.Visible = true;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpEnrollGo, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Enrollment Setting Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch { }
    }

    public void btnprintconfrm_Click(object sender, EventArgs e)
    {
        txtreport.Text = "";
        lblalert.Visible = false;
        string degreedetails = "Enrollment Confirm Report";
        string pagename = "Enrollmentselection.aspx";
        printconfm.loadspreaddetails(fpconfrm, pagename, degreedetails);
        printconfm.Visible = true;
    }
    protected void btnexlconfrm_Click(object sender, EventArgs e)
    {
        try
        {
            lblalert.Text = "";
            string reportname = txtreport.Text;
            if (reportname.ToString().Trim() != "")
            {
                txtreport.Text = "";
                d2.printexcelreport(fpconfrm, reportname);
                lblalert.Visible = false;
            }
            else
            {
                lblalert.Text = "Please Enter Your Enrollment Confirm Report Name";
                lblalert.Visible = true;
                txtreport.Focus();
            }
        }
        catch { }
    }

    protected void ddlenrollconfm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        fpconfrm.Visible = false;
        txtconfirmdt.Visible = false;
        btnConfrmsave.Visible = false;
        printconffm.Visible = false;

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string university = "";
            string collname = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string pincode = "";
            string affliated = "";
            string fromdate = Convert.ToString(txt_fromdate.Text);
            string todate = Convert.ToString(txt_todate.Text);
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            }

            string SelQ = "select app_formno ,enrollment_card_date from applyn where enrollmentcard ='1' and enrollment_card_date between '" + fromdate + "' and '" + todate + "' order by enrollment_card_date ";
            DataSet dsdt = new DataSet();
            dsdt.Clear();
            dsdt = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsdt.Tables.Count > 0 && dsdt.Tables[0].Rows.Count > 0)
            {
                string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    collname = ds.Tables[0].Rows[0]["collname"].ToString();
                    address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                    address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                    address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                    pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                    affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                }
                string Timing = "";
                string photo = "";
                // string txtmsg = "[Please read the prospectus carfully filling up the application form. Use CAPITAL LETTERS only]";
                string ApplicationNo = "";
                int incr = 6;
                StringBuilder studdt = new StringBuilder();
                for (int sel = 0; sel < dsdt.Tables[0].Rows.Count; sel++)
                {
                    if (ApplicationNo == "")
                        ApplicationNo = Convert.ToString(dsdt.Tables[0].Rows[sel]["app_formno"]);
                    else
                        ApplicationNo = ApplicationNo + "," + Convert.ToString(dsdt.Tables[0].Rows[sel]["app_formno"]);
                    if (sel == incr)
                    {
                        ApplicationNo = ApplicationNo + "<br>";
                        incr += 8;
                    }
                }
                if (ApplicationNo != "")
                {
                    studdt.Append("<div style='height:auto; width:650px;'><table cellspacing='0' cellpadding='0' style='height:auto; width:650px;'><tr><td colspan='10'><table cellspacing='0' cellpadding='0' style='width:650px;'><tr><td style='align:left;  border:1px solid black;'><img src='" + "college/Left_Logo.jpeg" + "' style='height:80px; width:70px;'/></td><td colspan='6' style='font-size:14px;font-family:Times New Roman;font-weight:bold; border:1px solid black;text-align:center;'><span style='font-size:17;font-weight:bold; font-family:Times New Roman;'>" + collname + "&nbsp;&nbsp;&nbsp;(Autonomous)<br>" + address1 + " , " + address3 + " - " + pincode + "</span><br></td></tr></table><br>");
                    studdt.Append("<table cellspacing='0' cellpadding='0' style='width:650px;'><tr><td style='text-align:left; font-family:Times New Roman; font-weight:bold; font:size:12px;'>" + ApplicationNo + "</td></tr></table></td></tr></table></div>");

                    contentDiv.InnerHtml = studdt.ToString();
                    contentDiv.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "btnExport", "PrintDiv();", true);
                }
            }
            else
            {
            }

        }
        catch
        { }

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
        lbl.Add(lbl_collegename);
        fields.Add(0);

        lbl.Add(lblStr);
        fields.Add(1);

        lbl.Add(lblDeg);
        fields.Add(2);

        lbl.Add(lblBran);
        fields.Add(3);

        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
}