using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class CoeMod_CourseWiseMaxandMinCriditPointEntry : System.Web.UI.Page
{
    Hashtable hat = new Hashtable();
    string usercode = "", collegecode = "", singleuser = "", group_user = "", grouporusercode = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
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
            if (!IsPostBack)
            {
                BindCollege();
                bindBatch();
                bindDegree();
                bindBranch();
            }
        }
        catch(Exception ex)
        {
        }
    }
    void BindCollege()
    {
        try
        {
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            ddlCollege.DataSource = ds;
            ddlCollege.DataTextField = "collname";
            ddlCollege.DataValueField = "college_code";
            ddlCollege.DataBind();
        }
        catch
        {
        }
    }
    public void bindBatch()
    {
        try
        {
            ds.Clear();

            ddlBatch.Items.Clear();
            ds = d2.BindBatch();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindDegree()
    {
        try
        {
            ddlDegree.Items.Clear();
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(ddlCollege.SelectedItem.Value);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
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
            ds.Clear();
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
                ddlDegree.Items.Add("All");
            }
        }
        catch
        {

        }
    }

    public void bindBranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            string commname = "";
            string branch = string.Empty;
            if (ddlDegree.SelectedItem.Text != "All")
            {
                branch = ddlDegree.SelectedValue;
            }
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlCollege.SelectedItem.Value + "' " + rights + " ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlCollege.SelectedItem.Value + "' " + rights + "";
            }
            ds = d2.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
                ddlBranch.SelectedIndex = 0;
                ddlBranch.Items.Add("All");
            }
        }
        catch
        {

        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindDegree();
            bindBranch();
            FpStudent.Visible = false;
            btnSave.Visible = false;
        }
        catch
        {

        }
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBranch();
            FpStudent.Visible = false;
            btnSave.Visible = false;
        }
        catch
        {

        }
    }
    protected void cbBatchWise_Change(object sender, EventArgs e)
    {
        if (cbBatchWise.Checked == true)
        {
            ddlBatch.Enabled = true;
        }
        else
        {
            ddlBatch.Enabled = false;
        }
        FpStudent.Visible = false;
        btnSave.Visible = false;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            GridHeader();
            FpStudent.SaveChanges();
            string degree = string.Empty;
            string branch = string.Empty;
            DataView dv = new DataView();
            if (ddlDegree.SelectedItem.Text != "All")
            {
                degree = ddlDegree.SelectedValue;
            }
            if (ddlBranch.SelectedItem.Text != "All")
            {
                branch = ddlBranch.SelectedValue;
            }
            FarPoint.Web.Spread.IntegerCellType intgrcel = new FarPoint.Web.Spread.IntegerCellType();
            string SqlQry = "select Course_Name,dt.dept_Name,d.degree_code from Degree d,Department dt,Course C where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.college_code ='" + ddlCollege.SelectedValue + "'";
            if (degree.Trim() != "")
            {
                SqlQry += "and c.course_id ='" + degree + "'";
            }
            if (branch.Trim() != "")
            {
                SqlQry += "and d.degree_code ='" + branch + "'";
            }
            SqlQry += " order by d.degree_code";
            SqlQry += "  select degree_code,totalcredits,minimcredits,IsCommon,batchYear from coe_ovrl_credits_Dts where";
            if (cbBatchWise.Checked == true)
            {
                SqlQry += " IsCommon='1' and batchYear='" + ddlBatch.SelectedItem.Text + "'";
            }
            else
            {
                SqlQry += " IsCommon='0'";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(SqlQry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                FpStudent.Sheets[0].RowCount = 0;
                for (int stu = 0; stu < ds.Tables[0].Rows.Count; stu++)
                {
                    FpStudent.Sheets[0].RowCount++;
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(stu + 1);

                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[stu]["Course_Name"]);
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[stu]["degree_code"]);


                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[stu]["dept_Name"]);

                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        if (cbBatchWise.Checked == false)
                        {
                            ds.Tables[1].DefaultView.RowFilter = " degree_code ='" + Convert.ToString(ds.Tables[0].Rows[stu]["degree_code"]) + "'";
                            dv = ds.Tables[1].DefaultView;
                        }
                        else
                        {
                            ds.Tables[1].DefaultView.RowFilter = " degree_code ='" + Convert.ToString(ds.Tables[0].Rows[stu]["degree_code"]) + "' and batchYear='" + ddlBatch.SelectedItem.Text + "'";
                            dv = ds.Tables[1].DefaultView;
                        }

                        if (dv.Count > 0)
                        {
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].CellType = intgrcel;
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[0]["totalcredits"]);
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].CellType = intgrcel;
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[0]["minimcredits"]);
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }

                }
                FpStudent.Visible = true;
                btnSave.Visible = true;
                FpStudent.Sheets[0].PageSize = FpStudent.Sheets[0].RowCount;
                FpStudent.Height = (FpStudent.Sheets[0].RowCount * 23) + 50;
                FpStudent.SaveChanges();
            }
            else
            {
                FpStudent.Visible = false;
                btnSave.Visible = false;
                lbl_popuperr.Text = "No Records Found ";
                errdiv.Visible = true;
                return;
            }

        }
        catch
        {

        }
    }
    public void GridHeader()
    {
        FpStudent.Sheets[0].AutoPostBack = false;
        FpStudent.CommandBar.Visible = false;
        FpStudent.Sheets[0].SheetCorner.ColumnCount = 0;
        FpStudent.Sheets[0].ColumnCount = 0;
        FpStudent.Sheets[0].RowCount = 0;
        FpStudent.Sheets[0].ColumnCount = 5;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Branch";
        FpStudent.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 3].Text = "MaxCredit";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Pass Credits For Consolidated Mark Sheet";


        FpStudent.Sheets[0].Columns[0].Width = 37;
        FpStudent.Sheets[0].Columns[1].Width = 100;
        FpStudent.Sheets[0].Columns[2].Width = 350;
        FpStudent.Sheets[0].Columns[3].Width = 100;
        FpStudent.Sheets[0].Columns[4].Width = 100;


        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
        style2.Font.Size = 13;
        style2.Font.Name = "Book Antiqua";
        style2.Font.Bold = true;
        style2.HorizontalAlign = HorizontalAlign.Center;
        style2.ForeColor = System.Drawing.Color.Black;
        // style2.BackColor = System.Drawing.Color.Teal;
        style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");

        FpStudent.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
        FpStudent.Sheets[0].SheetName = "Settings";
        FpStudent.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
        FpStudent.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
        FpStudent.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
        FpStudent.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpStudent.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpStudent.Sheets[0].DefaultStyle.Font.Bold = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FpStudent.SaveChanges();
            if (FpStudent.Sheets[0].RowCount > 0)
            {
                string degreeCode = string.Empty;
                string Minvalue = string.Empty;
                string Maxvalue = string.Empty;
                string batch = ddlBatch.SelectedItem.Text;
                double Min = 0;
                double Max = 0;
                bool result = false;
                for (int stu = 0; stu < FpStudent.Sheets[0].RowCount; stu++)
                {
                    degreeCode = Convert.ToString(FpStudent.Sheets[0].Cells[stu, 0].Tag);
                    Minvalue = Convert.ToString(FpStudent.Sheets[0].Cells[stu, 4].Text);
                    Maxvalue = Convert.ToString(FpStudent.Sheets[0].Cells[stu, 3].Text);
                    double.TryParse(Maxvalue, out Max);
                    double.TryParse(Minvalue, out Min);
                    string InstQuery = string.Empty;
                    if (Max >= Min)
                    {
                        if (cbBatchWise.Checked == false)
                        {
                            InstQuery = "if exists (select degree_Code from coe_ovrl_credits_Dts where degree_code ='" + degreeCode + "' and isCommon='0') update coe_ovrl_credits_Dts set minimcredits='" + Min + "',totalcredits='" + Max + "' where degree_code ='" + degreeCode + "' and isCommon='0' else insert into coe_ovrl_credits_Dts (degree_code,isCommon,minimcredits,totalcredits) values ('" + degreeCode + "','0','" + Min + "','" + Max + "')";
                        }
                        else if (cbBatchWise.Checked == true)
                        {
                            InstQuery = "if exists (select degree_Code from coe_ovrl_credits_Dts where degree_code ='" + degreeCode + "' and isCommon='1' and batchYear ='" + batch + "') update coe_ovrl_credits_Dts set minimcredits='" + Min + "',totalcredits='" + Max + "' where degree_code ='" + degreeCode + "' and isCommon='1' and batchYear ='" + batch + "' else insert into coe_ovrl_credits_Dts (degree_code,isCommon,minimcredits,totalcredits,batchYear) values ('" + degreeCode + "','1','" + Min + "','" + Max + "','" + batch + "')";
                        }
                        int intst = d2.update_method_wo_parameter(InstQuery, "Text");
                        if (intst == 1)
                        {
                            result = true;
                        }
                    }
                }
                if (result == true)
                {
                    lbl_popuperr.Text = "Saved Successfully ";
                    errdiv.Visible = true;
                    return;
                }
                else
                {
                    lbl_popuperr.Text = "Not Saved Successfully ";
                    errdiv.Visible = true;
                    return;
                }
            }
        }
        catch
        {

        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            errdiv.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }
}