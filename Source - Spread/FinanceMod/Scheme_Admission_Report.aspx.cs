using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class Scheme_Admission_Report : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    InsproDirectAccess DirAccess = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static byte roll = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//

            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                BindCollege();
                if (ddlcollege.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollege.SelectedValue);
                bindbatch();
                degree();
                BindSectionDetail();
                bindScheme();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                loadStudenttype();
            }
            lblMainErr.Visible = false;
            lbl_validation1.Visible = false;
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedValue);
        }
        catch (Exception ex) { }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        degree();
        bindbatch();
        BindSectionDetail();
        bindScheme();
    }

    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        degree();
        BindSectionDetail();
    }

    protected void ddl_degree_Selectedindexchange(object sender, EventArgs e)
    {
        // bindbranch(Convert.ToString(ddl_degree.SelectedItem.Value));
        // BindSectionDetail();
    }

    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text);
        BindSectionDetail();
    }

    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text);
        BindSectionDetail();
    }

    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_branch, cbl_branch, txt_branch, "Branch");
        BindSectionDetail();
    }

    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_branch, cbl_branch, txt_branch, "Branch");
        BindSectionDetail();
    }

    protected void cb_sec_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_sec, cbl_sec, txt_sec, "Section");
    }

    protected void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_sec, cbl_sec, txt_sec, "Section");
    }

    protected void cb_Scheme_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_Scheme, cbl_Scheme, txtScheme, "Scheme Type");
    }

    protected void cbl_Scheme_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_Scheme, cbl_Scheme, txtScheme, "Scheme Type");
    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        try
        {
            lblMainErr.Visible = false;
            string MyDegree = "";
            string MyScheme = "";
            string MySection = "";
            string MyStudMode = "";
            string Batch_Year = Convert.ToString(ddl_batch.SelectedItem.Text);
            // string CourseID = Convert.ToString(ddl_degree.SelectedItem.Value);
            string CourseID = Convert.ToString(cbl_degree.SelectedItem.Value);
            string DegreeCode = GetSelectedItemsValueAsString(cbl_branch);
            string Section = GetSelectedItemsText(cbl_sec);
            string Scheme = GetSelectedItemsValueAsString(cbl_Scheme);
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

            string studMode = Convert.ToString(getCblSelectedValue(cbl_type));
            if (String.IsNullOrEmpty(DegreeCode))
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Branch!";
                return;
            }
            if (String.IsNullOrEmpty(Scheme))
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Scheme!";
                return;
            }
            if (String.IsNullOrEmpty(studMode))
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Student Type!";
                return;
            }
            MyDegree = "'" + DegreeCode + "'";
            MySection = "'" + Section + "'";
            MyScheme = "'" + Scheme + "'";
            MyStudMode = "" + studMode + "";
            LoadHeader(Batch_Year, CourseID, MyDegree, MySection, MyScheme, fromdate, todate, MyStudMode);
        }
        catch { }
    }

    private void LoadHeader(string BatchYear, string CourseID, string DegreeCode, string Section, string Scheme, string fromdate, string todate, string MyStudMode)
    {
        try
        {
            RollAndRegSettings();
            lblMainErr.Visible = false;
            lbl_validation1.Visible = false;
            Fpspread1.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.RowHeader.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].RowCount = 0;

            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].ColumnCount = 10;

            FarPoint.Web.Spread.StyleInfo DarkStyle = new FarPoint.Web.Spread.StyleInfo();
            DarkStyle.Font.Bold = true;
            DarkStyle.Font.Size = FontUnit.Medium;
            DarkStyle.Font.Name = "Book Antiqua";
            DarkStyle.HorizontalAlign = HorizontalAlign.Center;
            DarkStyle.ForeColor = Color.Black;
            DarkStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpspread1.Sheets[0].ColumnHeader.DefaultStyle = DarkStyle;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            Fpspread1.Columns[0].Width = 75;
            Fpspread1.Columns[0].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            Fpspread1.Columns[1].Width = 100;
            Fpspread1.Columns[1].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
            Fpspread1.Columns[2].Width = 125;
            Fpspread1.Columns[2].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            Fpspread1.Columns[3].Width = 100;
            Fpspread1.Columns[3].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            Fpspread1.Columns[4].Width = 200;
            Fpspread1.Columns[4].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year";
            Fpspread1.Columns[5].Width = 75;
            Fpspread1.Columns[5].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = Convert.ToString(lbl_branch.Text);
            Fpspread1.Columns[6].Width = 200;
            Fpspread1.Columns[6].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Scheme Type";
            Fpspread1.Columns[7].Width = 150;
            Fpspread1.Columns[7].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Scheme Amount";
            Fpspread1.Columns[8].Width = 100;
            Fpspread1.Columns[8].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Date";
            Fpspread1.Columns[9].Width = 100;
            Fpspread1.Columns[9].Locked = true;
            spreadColumnVisible();

            string SelQ = "select roll_no,Roll_Admit,Reg_No,(R.Stud_Name+'-'+(case when mode='1' then '(O)' when mode='2' then '(T)' when mode='3' then '(N)' end ))as stud_name,R.Batch_Year,(c.Course_Name+ '-' + dt.Dept_Name + case when isnull(Sections,'')='' then '' else '-'+ isnull(Sections,'') end) as Dept_Name,IsSchemeCode,IsSchemeAmount,convert(varchar(10),SchemeDate,103) as SchemeDate from Registration r,Department dt,Course c,Degree d where c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and dt.Dept_Code=d.Dept_Code and CC=0 and DelFlag=0 and r.Exam_Flag<>'Debar' and IsSchemeAdmission='1' and r.Batch_Year='" + BatchYear + "'  and d.Degree_Code in(" + DegreeCode + ") and isnull(Sections,'') in(" + Section + ",'') and IsSchemeCode in(" + Scheme + ") and d.college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and r.mode in('" + MyStudMode + "')   ";
            if (cbdate.Checked)
                SelQ += " and SchemeDate between '" + fromdate + "' and '" + todate + "'";
            SelQ += orderBy();
            SelQ = SelQ + " select TextCode,TextVal from TextValTable where TextCriteria ='Schm' and college_code ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds.Clear();
            ds = DirAccess.selectDataSet(SelQ);
            DataView dvnew = new DataView();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int ro = 0; ro < ds.Tables[0].Rows.Count; ro++)
                {
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ro + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[ro]["roll_no"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[ro]["Reg_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[ro]["Roll_Admit"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[ro]["Stud_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[ro]["Batch_Year"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[ro]["Dept_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    ds.Tables[1].DefaultView.RowFilter = " TextCode='" + Convert.ToString(ds.Tables[0].Rows[ro]["IsSchemeCode"]) + "'";
                    dvnew = ds.Tables[1].DefaultView;
                    if (dvnew.Count > 0)
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dvnew[0]["TextVal"]);
                    else
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = "";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[ro]["IsSchemeAmount"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[ro]["SchemeDate"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                }
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Sheets[0].SetColumnMerge(9, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Visible = true;
                Fpspread1.Width = 900;
                Fpspread1.Height = 400;
                div_report.Visible = true;
            }
            else
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "No Record(s) Found!";
                Fpspread1.Visible = false;
                div_report.Visible = false;
            }
        }
        catch { }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, report);
                lbl_validation1.Visible = false;
            }
            else
            {
                lbl_validation1.Text = "Please Enter Your Report Name";
                lbl_validation1.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch (Exception ex) { }
    }

    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Scheme Admission Report";
            string pagename = "Scheme_Admission_Report.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, attendance);
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { }
    }

    private void BindCollege()
    {
        try
        {
            ds.Clear();
            ddlcollege.Items.Clear();
            ds = d2.BindCollegebaseonrights(Session["usercode"].ToString());
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
        catch { }
    }

    public void bindbatch()
    {
        try
        {
            ds.Clear();
            ddl_batch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
                degree();
            }
        }
        catch { }
    }

    public void degree()
    {
        try
        {
            string query = "";
            string rights = "";
            txt_degree.Text = "--Select--";
            cb_degree.Checked = false;
            cbl_degree.Items.Clear();
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
            query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                //ddl_degree.DataSource = ds;
                //ddl_degree.DataTextField = "course_name";
                //ddl_degree.DataValueField = "course_id";
                //ddl_degree.DataBind();
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = lbl_degree.Text + "(" + cbl_degree.Items.Count + ")";
                cb_degree.Checked = true;
                string branch = string.Empty;
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected)
                    {
                        if (branch == string.Empty)
                            branch = Convert.ToString(cbl_degree.Items[i].Value);
                        else
                            branch += "," + Convert.ToString(cbl_degree.Items[i].Value);
                        // branch += "'" + "," + "'" + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

                bindbranch(branch);
            }
            else
            {
                // ddl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
        }
        catch (Exception ex) { }
    }

    public void bindbranch(string branch)
    {
        try
        {
            //branch = Convert.ToString(ddl_degree.SelectedItem.Value);
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
            cb_branch.Checked = false;
            string commname = "";
            //if (branch != "")
            //{
            //    commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + " ";
            //}
            //else
            //{
            //    commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            //}
            ds.Clear();
            cbl_branch.Items.Clear();
            // ds = d2.select_method(commname, hat, "Text");
            ds.Clear();
            ds = d2.BindBranchMultiple(singleuser, group_user, branch, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = lbl_branch.Text + "(" + cbl_branch.Items.Count + ")";
                cb_branch.Checked = true;

            }
        }
        catch (Exception ex) { }
    }

    public void BindSectionDetail()
    {
        try
        {
            cbl_sec.Items.Clear();
            string batch = "";
            string branch = "";
            int i = 0;
            if (cbl_branch.Items.Count > 0)
            {
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {

                    if (cbl_branch.Items[i].Selected == true)
                    {
                        string build = cbl_branch.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build;
                        }
                        else
                        {
                            branch = branch + "','" + build;

                        }
                    }
                }
            }
            if (ddl_batch.Items.Count > 0)
            {
                batch = ddl_batch.SelectedItem.Value;
            }

            string sqlquery = "select distinct sections from registration where batch_year in('" + batch + "') and degree_code in('" + branch + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";

            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(sqlquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sec.DataSource = ds;
                cbl_sec.DataTextField = "sections";
                cbl_sec.DataValueField = "sections";
                cbl_sec.DataBind();
                if (cbl_sec.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sec.Items.Count; row++)
                    {
                        cbl_sec.Items[row].Selected = true;
                        cb_sec.Checked = true;
                    }
                    txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                }
                else
                {
                    txt_sec.Text = "--Select--";
                }

            }
            else
            {
                txt_sec.Text = "--Select--";
            }
        }
        catch { }
    }

    private void bindScheme()
    {
        try
        {
            string SelQ = "select TextCode,TextVal from TextValTable where TextCriteria ='Schm' and college_code ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds.Clear();
            cbl_Scheme.Items.Clear();
            cb_Scheme.Checked = false;
            txtScheme.Text = "--Select--";
            ds = DirAccess.selectDataSet(SelQ);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_Scheme.DataSource = ds;
                cbl_Scheme.DataTextField = "TextVal";
                cbl_Scheme.DataValueField = "TextCode";
                cbl_Scheme.DataBind();
                if (cbl_Scheme.Items.Count > 0)
                {
                    for (int ik = 0; ik < cbl_Scheme.Items.Count; ik++)
                    {
                        cbl_Scheme.Items[ik].Selected = true;
                    }
                    cb_Scheme.Checked = true;
                    txtScheme.Text = "Scheme Type(" + Convert.ToString(cbl_Scheme.Items.Count) + ")";
                }
            }
        }
        catch { }
    }

    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Text));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsTextnew(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    else
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[j].Text));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                if (chklstchange.Items.Count == 0)
                    txtchange.Text = "--Select--";
                else
                    txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
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
        Label lbl_Stream = new Label();
        Label lbl_org_sem = new Label();
        List<byte> fields = new List<byte>();
        lbl.Add(lbl_clgname);
        fields.Add(0);

        lbl.Add(lbl_Stream);
        fields.Add(1);

        lbl.Add(lbl_degree);
        fields.Add(2);

        lbl.Add(lbl_branch);
        fields.Add(3);

        lbl.Add(lbl_org_sem);
        fields.Add(4);

        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    #region roll,reg,admission setting
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }

    protected void spreadColumnVisible()
    {
        try
        {
            #region student wise
            if (roll == 0)
            {
                Fpspread1.Columns[1].Visible = true;
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = true;
            }
            else if (roll == 1)
            {
                Fpspread1.Columns[1].Visible = true;
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = true;
            }
            else if (roll == 2)
            {
                Fpspread1.Columns[1].Visible = true;
                Fpspread1.Columns[2].Visible = false;
                Fpspread1.Columns[3].Visible = false;

            }
            else if (roll == 3)
            {
                Fpspread1.Columns[1].Visible = false;
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = false;
            }
            else if (roll == 4)
            {
                Fpspread1.Columns[1].Visible = false;
                Fpspread1.Columns[2].Visible = false;
                Fpspread1.Columns[3].Visible = true;
            }
            else if (roll == 5)
            {
                Fpspread1.Columns[1].Visible = true;
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = false;
            }
            else if (roll == 6)
            {
                Fpspread1.Columns[1].Visible = false;
                Fpspread1.Columns[2].Visible = true;
                Fpspread1.Columns[3].Visible = true;
            }
            else if (roll == 7)
            {
                Fpspread1.Columns[1].Visible = true;
                Fpspread1.Columns[2].Visible = false;
                Fpspread1.Columns[3].Visible = true;
            }
            #endregion

        }
        catch { }
    }

    protected string orderBy()
    {
        string strOrderBy = string.Empty;
        try
        {
            #region student wise
            if (roll == 0)
                strOrderBy = "order by roll_no,Reg_No,Roll_Admit asc";
            else if (roll == 1)
                strOrderBy = "order by roll_no,Reg_No,Roll_Admit asc";
            else if (roll == 2)
                strOrderBy = "order by roll_no asc";
            else if (roll == 3)
                strOrderBy = "order by Reg_No asc";
            else if (roll == 4)
                strOrderBy = "order by Roll_Admit asc";
            else if (roll == 5)
                strOrderBy = "order by roll_no,Reg_No asc";
            else if (roll == 6)
                strOrderBy = "order by Reg_No,Roll_Admit asc";
            else if (roll == 7)
                strOrderBy = "order by roll_no,Roll_Admit asc";
            #endregion

        }
        catch { }
        return strOrderBy;
    }

    #endregion

    //added by sudhagar  06.06.2017
    #region student type
    protected void cb_type_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_type, cbl_type, txt_type, "Type", "--Select--");
    }
    protected void cbl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_type, cbl_type, txt_type, "Type", "--Select--");
    }
    public void loadStudenttypes()
    {

        try
        {

            cbl_type.Items.Clear();

            string type = "";
            string deptquery = "select distinct case when mode =1 then 'Regular' when mode =3 then 'Lateral'  when mode =2 then 'Transfer' end as Modename,mode from Registration r,Degree g where r.degree_code = g.Degree_Code and g.college_code in('" + collegecode + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_type.DataSource = ds;
                cbl_type.DataTextField = "Modename";
                cbl_type.DataValueField = "mode";
                cbl_type.DataBind();

                if (cbl_type.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_type.Items.Count; i++)
                    {
                        cbl_type.Items[i].Selected = true;
                        type = Convert.ToString(cbl_type.Items[i].Text);
                    }
                    if (cbl_type.Items.Count == 1)
                    {
                        txt_type.Text = "Type(" + type + ")";
                    }
                    else
                    {
                        txt_type.Text = "Type(" + cbl_type.Items.Count + ")";
                    }
                    cb_type.Checked = true;
                }
            }
            else
            {
                txt_type.Text = "--Select--";

            }
        }
        catch
        {
        }

    }

    protected void loadStudenttype()
    {
        try
        {
            cbl_type.Items.Clear();
            if (checkSchoolSetting() == 0)
            {
                cbl_type.Items.Add(new ListItem("Old Studnent", "1"));
                cbl_type.Items.Add(new ListItem("New    ", "3"));
                // cbl_type.Items.Add(new ListItem("Transfer", "2"));
            }
            else
            {
                cbl_type.Items.Add(new ListItem("Regular", "1"));
                cbl_type.Items.Add(new ListItem("Lateral", "3"));
                cbl_type.Items.Add(new ListItem("Transfer", "2"));
                cbl_type.Items.Add(new ListItem("IrRegular", "4"));
            }
            if (cbl_type.Items.Count > 0)
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = true;
                }
                cb_type.Checked = true;
                txt_type.Text = "Type(" + cbl_type.Items.Count + ")";
            }
        }
        catch { }
    }

    protected Dictionary<string, string> getstudMode()
    {
        Dictionary<string, string> studMode = new Dictionary<string, string>();
        for (int i = 0; i < cbl_type.Items.Count; i++)
        {
            studMode.Add(cbl_type.Items[i].Text, cbl_type.Items[i].Value);
        }
        return studMode;
    }

    private double checkSchoolSetting()
    {
        double getVal = 0;
        try
        {
            double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);

        }
        catch { }
        return getVal;
    }
    #endregion

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
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
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
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
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
        }
        catch { }
    }

    #endregion
}