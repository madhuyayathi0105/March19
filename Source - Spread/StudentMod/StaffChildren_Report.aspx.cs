using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class StaffChildren_Report : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    FarPoint.Web.Spread.StyleInfo darkStyle = new FarPoint.Web.Spread.StyleInfo();

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
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddlcollegename.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                bindsec();
            }
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            lblMainErr.Visible = false;
            lblsmserror.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void ddlcollegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsec();
            Fpspread1.Visible = false;
            rprint.Visible = false;
            lblMainErr.Visible = false;
        }
        catch { }
    }

    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        binddeg();
        binddept();
    }

    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        binddeg();
        binddept();
    }

    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();
    }

    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();
    }

    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
        bindsec();
        bindsem();
    }

    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
        bindsec();
        bindsem();
    }

    protected void cb_sect_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sect, cbl_sect, txt_sect, "Section", "--Select--");
    }

    protected void cbl_sect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sect, cbl_sect, txt_sect, "Section", "--Select--");
    }

    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");
        bindsec();
    }

    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");
        bindsec();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string SelQ = string.Empty;
            lblMainErr.Visible = false;
            Fpspread1.Visible = false;
            rprint.Visible = false;
            string ClgCode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            string BatchYear = getCblSelectedText(cbl_batch);
            string DegreeCode = getCblSelectedValue(cbl_dept);
            string BatchCode = getCblSelectedValue(cbl_degree);
            string SemCode = getCblSelectedText(cbl_sem);
            string Section = getCblSelectedText(cbl_sect);
            DataView dvnew = new DataView();
            DataView myDv1 = new DataView();
            DataView myDv2 = new DataView();

            if (String.IsNullOrEmpty(BatchYear) || BatchYear == "0")
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any Batch Year!";
                return;
            }

            if (String.IsNullOrEmpty(DegreeCode) || DegreeCode == "0")
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any " + lbldept.Text + "!";
                return;
            }

            if (String.IsNullOrEmpty(BatchCode) || BatchCode == "0")
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any " + lbldeg.Text + "!";
                return;
            }

            if (String.IsNullOrEmpty(SemCode) || SemCode == "0")
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any " + lblsem.Text + "!";
                return;
            }
            if (rdbCount.Checked == true)
            {
                SelQ = "select COUNT(r.app_no)Total,r.degree_code,r.Current_Semester,r.Batch_Year,r.Sections from Registration r,Degree d,course c,Department dt where d.Degree_Code=r.degree_code and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and Is_Stud_Staff='1' and staff_appl_id is not null and CAST(staff_appl_id as varchar)<>'' and r.degree_code in('" + DegreeCode + "') and r.batch_year in('" + BatchYear + "') and r.Current_Semester in('" + SemCode + "') and ISNULL(r.Sections,'') in ('','" + Section + "') and r.college_code='" + ClgCode + "' group by r.degree_code,r.Current_Semester,r.Batch_Year,r.Sections";
                SelQ = SelQ + " select (Course_Name+' - '+Dept_Name) as Dept,deg.Degree_Code,deg.college_code from Degree deg,Department d,Course c where deg.Dept_Code=d.Dept_Code and deg.Course_Id=c.Course_Id and deg.college_code='" + ClgCode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    LoadCountHeader();
                    for (int co = 0; co < ds.Tables[0].Rows.Count; co++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(co + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[co]["Batch_Year"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        ds.Tables[1].DefaultView.RowFilter = " Degree_Code='" + Convert.ToString(ds.Tables[0].Rows[co]["degree_code"]) + "'";
                        dvnew = ds.Tables[1].DefaultView;
                        if (dvnew.Count > 0)
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvnew[0]["Dept"]);
                        else
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = "";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[co]["Current_Semester"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[co]["Sections"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[co]["Total"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    }
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Visible = true;
                    Fpspread1.Width = 665;
                    rprint.Visible = true;
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "No Record(s) Found!";
                }
            }
            else if (rdbDetail.Checked == true)
            {
                SelQ = "select r.Roll_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.degree_code,r.Current_Semester,r.Batch_Year,r.Sections,r.staff_appl_id from Registration r,Degree d,course c,Department dt where d.Degree_Code=r.degree_code and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and Is_Stud_Staff='1' and staff_appl_id is not null and CAST(staff_appl_id as varchar)<>'' and r.degree_code in('" + DegreeCode + "') and r.batch_year in('" + BatchYear + "') and r.Current_Semester in('" + SemCode + "') and ISNULL(r.Sections,'') in ('','" + Section + "') and r.college_code='" + ClgCode + "'";
                SelQ = SelQ + " select (Course_Name+' - '+Dept_Name) as Dept,deg.Degree_Code,deg.college_code from Degree deg,Department d,Course c where deg.Dept_Code=d.Dept_Code and deg.Course_Id=c.Course_Id and deg.college_code='" + ClgCode + "'";
                SelQ = SelQ + " select Staff_Name,sa.appl_id from staff_appl_master sa,staffmaster sm,stafftrans st where sa.appl_no=sm.appl_no and sm.staff_code=st.staff_code and st.latestrec='1' and sm.settled='0' and sm.resign='0' and ISNULL(Discontinue,'0')='0'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    LoadDetailHeader();
                    for (int de = 0; de < ds.Tables[0].Rows.Count; de++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(de + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[de]["Roll_No"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[de]["Reg_No"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[de]["Roll_Admit"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[de]["Stud_Name"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[de]["Batch_Year"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                        ds.Tables[1].DefaultView.RowFilter = " Degree_Code='" + Convert.ToString(ds.Tables[0].Rows[de]["degree_code"]) + "'";
                        myDv1 = ds.Tables[1].DefaultView;
                        if (myDv1.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[de]["Current_Semester"])) && !String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[de]["Sections"])))
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(myDv1[0]["Dept"]) + " - " + Convert.ToString(ds.Tables[0].Rows[de]["Current_Semester"]) + " - " + Convert.ToString(ds.Tables[0].Rows[de]["Sections"]);
                            else if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[de]["Current_Semester"])) && String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[de]["Sections"])))
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(myDv1[0]["Dept"]) + " - " + Convert.ToString(ds.Tables[0].Rows[de]["Current_Semester"]);
                            else
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(myDv1[0]["Dept"]);
                        }
                        else
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = "";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;

                        ds.Tables[2].DefaultView.RowFilter = " appl_id='" + Convert.ToString(ds.Tables[0].Rows[de]["staff_appl_id"]) + "'";
                        myDv2 = ds.Tables[2].DefaultView;
                        if (myDv2.Count > 0)
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(myDv2[0]["Staff_Name"]);
                        else
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = "";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    }
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Visible = true;
                    Fpspread1.Width = 900;
                    rprint.Visible = true;
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "No Record(s) Found!";
                }
            }
        }
        catch { }
    }

    private void LoadCountHeader()
    {
        try
        {
            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnHeader.Columns.Count = 6;

            darkStyle.Font.Bold = true;
            darkStyle.Font.Name = "Book Antiqua";
            darkStyle.Font.Size = FontUnit.Medium;
            darkStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkStyle.ForeColor = Color.Black;
            darkStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.DefaultStyle = darkStyle;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            Fpspread1.Columns[0].Width = 50;
            Fpspread1.Columns[0].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            Fpspread1.Columns[1].Width = 100;
            Fpspread1.Columns[1].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
            Fpspread1.Columns[2].Width = 200;
            Fpspread1.Columns[2].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = lblsem.Text;
            Fpspread1.Columns[3].Width = 100;
            Fpspread1.Columns[3].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Section";
            Fpspread1.Columns[4].Width = 100;
            Fpspread1.Columns[4].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "No.of Students";
            Fpspread1.Columns[5].Width = 100;
            Fpspread1.Columns[5].Locked = true;
        }
        catch { }
    }

    private void LoadDetailHeader()
    {
        try
        {
            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnHeader.Columns.Count = 8;

            darkStyle.Font.Bold = true;
            darkStyle.Font.Name = "Book Antiqua";
            darkStyle.Font.Size = FontUnit.Medium;
            darkStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkStyle.ForeColor = Color.Black;
            darkStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.DefaultStyle = darkStyle;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            Fpspread1.Columns[0].Width = 50;
            Fpspread1.Columns[0].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            Fpspread1.Columns[1].Width = 100;
            Fpspread1.Columns[1].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
            Fpspread1.Columns[2].Width = 100;
            Fpspread1.Columns[2].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            Fpspread1.Columns[3].Width = 100;
            Fpspread1.Columns[3].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            Fpspread1.Columns[4].Width = 150;
            Fpspread1.Columns[4].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year";
            Fpspread1.Columns[5].Width = 100;
            Fpspread1.Columns[5].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department";
            Fpspread1.Columns[6].Width = 250;
            Fpspread1.Columns[6].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Staff Name";
            Fpspread1.Columns[7].Width = 150;
            Fpspread1.Columns[7].Locked = true;
        }
        catch { }
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Text = "Please Enter Your  Report Name";
                lblsmserror.Visible = true;
                txtexcel.Focus();
            }
        }
        catch { }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblsmserror.Text = "";
            txtexcel.Text = "";
            string degreedetails = "";
            string pagename;
            if (rdbCount.Checked == true)
                degreedetails = "Staff Children Count Report";
            else if (rdbDetail.Checked == true)
                degreedetails = "Staff Children Detail Report";
            pagename = "StaffChildren_Report.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
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
        List<byte> fields = new List<byte>();

        lbl.Add(lblclg);
        // lbl.Add(lbl_str1);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    public void loadcollege()
    {
        ddlcollegename.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollegename);
    }

    public void bindBtch()
    {
        try
        {
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = lblbatch.Text + "(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }

    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            cbl_degree.Items.Clear();
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }
        }
        catch { }
    }

    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string batch = "";
            batch = getCblSelectedText(cbl_batch);
            string degree = "";
            degree = "'" + getCblSelectedValue(cbl_degree) + "'";
            string collegecode = ddlcollegename.SelectedItem.Value.ToString();
            if (batch != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }
        }
        catch { }
    }

    protected void bindsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            decimal SemCount = 0;
            string cbltext = string.Empty;
            ds = d2.BindmultSem(Convert.ToString(ddlcollegename.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<decimal> list = ds.Tables[0].AsEnumerable()
                                                       .Select(r => r.Field<decimal>(0))
                                                       .ToList();
                SemCount = list.ToArray().Max();
                if (SemCount > 0)
                {
                    for (int se = 0; se < SemCount; se++)
                    {
                        cbl_sem.Items.Insert(se, new ListItem(Convert.ToString(se + 1), Convert.ToString(se + 1)));
                    }
                }
                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        cbltext = Convert.ToString(cbl_sem.Items[i].Text);
                    }
                    if (cbl_sem.Items.Count == 1)
                        txt_sem.Text = "" + lblsem.Text + "(" + cbltext + ")";
                    else
                        txt_sem.Text = "" + lblsem.Text + "(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }

    public void bindsec()
    {
        try
        {
            cbl_sect.Items.Clear();
            txt_sect.Text = "---Select---";
            cb_sect.Checked = false;
            string build = "";
            build = getCblSelectedValue(cbl_sem);
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            if (build != "")
            {
                ds = d2.BindSectionDetailmult(clgvalue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sect.DataSource = ds;
                    cbl_sect.DataTextField = "sections";
                    cbl_sect.DataValueField = "sections";
                    cbl_sect.DataBind();
                    if (cbl_sect.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sect.Items.Count; row++)
                        {
                            cbl_sect.Items[row].Selected = true;
                        }
                        txt_sect.Text = "Section(" + cbl_sect.Items.Count + ")";
                        cb_sect.Checked = true;
                    }
                }
            }
            else
            {
                cb_sect.Checked = false;
                txt_sect.Text = "--Select--";
            }
        }
        catch (Exception ex) { }
    }

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
}