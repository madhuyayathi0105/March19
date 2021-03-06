﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Drawing;

public partial class medium_of_study_report : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    string q1 = "";
    int insert = 0;
    int i = 0;
    int k = 0;
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
            lblvalidation1.Visible = false;
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            lblvalidation1.Text = "";
            if (!IsPostBack)
            {
                setLabelText();
                bind_batch();
                bindcollege();
                bind_seattype();
                binddegree();
                bindbranch();
                bind_medium();
            }
        }
        catch (Exception ex) { }
    }

    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            q1 = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"].ToString() + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch { }
    }
    protected void binddegree()
    {
        try
        {
            ds.Clear();
            string query = "";
            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + " order by course.course_name";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + " order by course.course_name";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            cbl_degree.Items.Clear();
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
                    txt_degree.Text = lbl_degree.Text + "(" + cbl_degree.Items.Count + ")";
                }
            }
            else
            {
                txt_degree.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void bindbranch()
    {
        try
        {
            string query1 = "";
            string buildvalue1 = "";
            if (cbl_degree.Items.Count > 0)
            {
                buildvalue1 = returnwithsinglecodevalue(cbl_degree);
                query1 = "select distinct degree.degree_code,(course_name+'-'+department.dept_name) as dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddl_college.SelectedValue + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' order by dept_name";
                ds = d2.select_method_wo_parameter(query1, "Text");
                cbl_branch.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txt_branch.Text = lbl_branch.Text + "(" + cbl_branch.Items.Count + ")";
                    }
                }
                else
                {
                    txt_branch.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void cb_degree_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text, "--Select--");
        bindbranch();
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbl_degree.Text);
        bindbranch();
    }
    protected void cbl_comm_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_comm, cbl_comm, txt_comm, "Medium");
    }
    protected void cb_comm_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_comm, cbl_comm, txt_comm, "Medium", "--Select--");
    }
    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, "--Select--");
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text);
    }
    protected void bind_medium()
    {
        try
        {
            q1 = "";
            q1 = " select distinct s.medium,t.textval from Stud_prev_details s,textvaltable t,Registration r where r.App_No=s.app_no and  s.medium=CONVERT(varchar(100),t.TextCode) and t.TextCriteria='PLang' and r.college_code=t.college_code order by t.textval";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "text");
            cbl_comm.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_comm.DataSource = ds;
                cbl_comm.DataTextField = "textval";
                cbl_comm.DataValueField = "medium";
                cbl_comm.DataBind();
                if (cbl_comm.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_comm.Items.Count; i++)
                    {
                        cbl_comm.Items[i].Selected = true;
                    }
                    txt_comm.Text = "Medium (" + cbl_comm.Items.Count + ")";
                }
            }
            else
            {
                txt_comm.Text = "--Select--";
            }

        }
        catch
        { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
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
                txt.Text = "--Select--";
            }
        }
        catch { }
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Medium Of Study Report";
            string pagename = "medium_of_study_report.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    protected void cb_seat_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_seat, cbl_seat, txt_seat, "Seat Type", "--Select--");
    }
    protected void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_seat, cbl_seat, txt_seat, "Seat Type");
    }
    protected void bind_seattype()
    {
        try
        {
            ds.Clear();
            if (ddl_college.Items.Count > 0)
            {
                q1 = " select textcode,textval from textvaltable where TextCriteria='seat' and college_code ='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' order by TextVal";
                ds = d2.select_method_wo_parameter(q1, "text");
                cbl_seat.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_seat.DataSource = ds;
                    cbl_seat.DataTextField = "textval";
                    cbl_seat.DataValueField = "textcode";
                    cbl_seat.DataBind();
                    if (cbl_seat.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_seat.Items.Count; i++)
                        {
                            cbl_seat.Items[i].Selected = true;
                        }
                        txt_seat.Text = "Seat Type(" + cbl_seat.Items.Count + ")";
                    }
                }
                else
                {
                    txt_seat.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            int medCount = 0;
            ArrayList arrq = new ArrayList();
            arrq.Clear();
            arrq.Add("Government");
            arrq.Add("Private");
            Printcontrol.Visible = false;
            Hashtable totalvalue_dic = new Hashtable();
            if (txt_seat.Text != "--Select--" && txt_degree.Text != "--Select--" && txt_branch.Text != "--Select--" && txt_comm.Text != "--Select--")
            {
                if (cbl_comm.Items.Count > 0)
                {
                    for (int k = 0; k < cbl_comm.Items.Count; k++)
                    {
                        if (cbl_comm.Items[k].Selected == true)
                        {
                            medCount++;
                        }
                    }
                }
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 3;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].Columns.Count = 2;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[0].Width = 50;
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = lbl_branch.Text;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 200;
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                int availabletotal = 0; double granttotal = 0; double subtotal = 0; double myTotal = 0;
                if (cbl_seat.Items.Count > 0)
                {
                    #region Header binding

                    for (i = 0; i < cbl_seat.Items.Count; i++)
                    {
                        if (cbl_seat.Items[i].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnCount++;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_seat.Items[i].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                            int medium = 0;
                            for (int q = 0; q < arrq.Count; q++)
                            {
                                if (cbl_comm.Items.Count > 0)
                                {
                                    for (int k = 0; k < cbl_comm.Items.Count; k++)
                                    {
                                        if (cbl_comm.Items[k].Selected == true)
                                        {
                                            if (medium != 0)
                                            {
                                                Fpspread1.Sheets[0].ColumnCount++;
                                            }
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(arrq[q]);
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_comm.Items[k].Text);
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_comm.Items[k].Value);
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(cbl_seat.Items[i].Value);
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            medium++;
                                        }
                                    }
                                    Fpspread1.Sheets[0].ColumnCount++;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Total";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].ColumnHeader.Cells[2, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpspread1.Sheets[0].ColumnCount - medCount - 1, 1, medCount + 1);
                                }
                            }
                            Fpspread1.Sheets[0].ColumnCount++;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Grand Total";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, Fpspread1.Sheets[0].ColumnCount - 1, 3, 1);
                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - medium - 3, 1, ((2 * (medCount + 1)) + 1));
                            Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 100;
                        }
                    }
                    Fpspread1.Sheets[0].ColumnCount++;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Total";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, Fpspread1.Sheets[0].ColumnCount - 1, 3, 1);
                    Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 150;

                    #endregion

                    int r = 1; double depttotal = 0;
                    string degreecode = returnwithsinglecodevalue(cbl_branch);
                    q1 = ""; q1 = "  select count(s.medium)count,s.medium,s.Sch_Clg_Type,t.textval ,r.degree_code,a.seattype,r.Batch_Year from Stud_prev_details s,textvaltable t,Registration r,applyn a where a.app_no=r.App_No and a.app_no=s.app_no and r.App_No=s.app_no and  s.medium=CONVERT(varchar(100),t.TextCode) and t.TextCriteria='PLang' and r.college_code=t.college_code and r.degree_code in('" + degreecode + "') and r.college_code in('" + Convert.ToString(ddl_college.SelectedValue) + "') and r.Batch_Year ='" + Convert.ToString(ddl_batch.SelectedValue) + "' group by s.medium,r.degree_code,a.seattype,t.TextVal ,r.Batch_Year,s.Sch_Clg_Type  ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    for (i = 0; i < cbl_branch.Items.Count; i++)
                    {
                        depttotal = 0;
                        if (cbl_branch.Items[i].Selected == true)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(r++);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cbl_branch.Items[i].Text);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(cbl_branch.Items[i].Value);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            #region spread bind
                            subtotal = 0;
                            myTotal = 0;
                            for (int k = 2; k < Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1; k++)
                            {
                                string seattypevalue = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Note);
                                string headertype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                                string schooltype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Text);
                                bool boolSchlType = false;
                                if (schooltype.Trim().ToUpper() == "GOVERNMENT")
                                    boolSchlType = true;
                                else if (schooltype.Trim().ToUpper() == "PRIVATE")
                                    boolSchlType = false;

                                if (headertype.Trim() != "")
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "seattype in('" + seattypevalue + "') and degree_code in('" + Convert.ToString(cbl_branch.Items[i].Value) + "') and batch_year='" + Convert.ToString(ddl_batch.SelectedItem.Text) + "' and medium='" + headertype + "' and Sch_Clg_Type='" + boolSchlType + "'";
                                        DataView dv = ds.Tables[0].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(dv[0]["count"]);
                                            double spval = 0;
                                            double.TryParse(Convert.ToString(dv[0]["count"]), out spval);
                                            subtotal += spval;
                                            if (totalvalue_dic.Contains(seattypevalue + "-" + headertype + "-" + schooltype))
                                            {
                                                string value = totalvalue_dic[seattypevalue + "-" + headertype + "-" + schooltype].ToString();
                                                totalvalue_dic.Remove(seattypevalue + "-" + headertype + "-" + schooltype);
                                                int total = Convert.ToInt32(value) + Convert.ToInt32(dv[0]["count"]);
                                                totalvalue_dic.Add(seattypevalue + "-" + headertype + "-" + schooltype, total);
                                            }
                                            else
                                            {
                                                totalvalue_dic.Add(seattypevalue + "-" + headertype + "-" + schooltype, Convert.ToInt32(dv[0]["count"]));
                                            }
                                        }
                                        else
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = "0";
                                        }
                                        depttotal += Convert.ToDouble(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text);
                                    }
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = "0";
                                    }
                                }
                                else
                                {
                                    myTotal = myTotal + subtotal;
                                    if (Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Text == "Total")
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(subtotal);
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(myTotal);
                                        myTotal = 0;
                                    }
                                    subtotal = 0;
                                }
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Name = "Book Antiqua";
                            }
                            #endregion

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = Convert.ToString(depttotal);
                            availabletotal += Convert.ToInt32(depttotal);

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                        }
                    }
                }
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "Grant Total";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "Grant Percentage";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = Convert.ToString(availabletotal);
                double grandpercent = 0;
                if (availabletotal != 0)
                {
                    grandpercent = availabletotal / availabletotal * 100;
                }
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = Convert.ToString(grandpercent);

                #region  grand total
                subtotal = 0;
                for (int k = 2; k < Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1; k++)
                {
                    string seattypevalue = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Note);
                    string headertype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Tag);
                    string schooltype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, k].Text);
                    if (headertype.Trim() != "")
                    {
                        if (totalvalue_dic.Count > 0)
                        {
                            string value = "";
                            if (totalvalue_dic.Contains(seattypevalue + "-" + headertype + "-" + schooltype))
                            {
                                value = totalvalue_dic[seattypevalue + "-" + headertype + "-" + schooltype].ToString();
                            }
                            else
                            {
                                value = "0";
                            }
                            double spval = 0;
                            double.TryParse(value, out spval);
                            subtotal += spval;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].Text = value;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].Font.Name = "Book Antiqua";
                            double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].Text), out granttotal);
                            grandpercent = 0;
                            grandpercent = granttotal / availabletotal * 100;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(Math.Round(grandpercent, 2));
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else
                    {
                        myTotal = myTotal + subtotal;
                        if (Fpspread1.Sheets[0].ColumnHeader.Cells[2, k].Text == "Total")
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].Text = Convert.ToString(subtotal);
                        else
                        {
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].Text = Convert.ToString(myTotal);
                            myTotal = 0;
                        }
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].Font.Name = "Book Antiqua";
                        subtotal = 0;
                        double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, k].Text), out granttotal);
                        grandpercent = 0;
                        grandpercent = granttotal / availabletotal * 100;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(Math.Round(grandpercent, 2));
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                #endregion

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 2].BackColor = Color.Bisque;
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 2].ForeColor = Color.IndianRed;

                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].ForeColor = Color.Blue;

                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Visible = true;
                rptprint.Visible = true;
                lbl_error.Visible = false;
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Fields";
                Fpspread1.Visible = false;
                rptprint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
            //d2.sendErrorMail(ex, collegecode1, "Medium Of Study Report");
        }
    }
    protected string returnwithsinglecodevalue(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                {
                    empty = Convert.ToString(cb.Items[i].Value);
                }
                else
                {
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
                }
            }
        }
        return empty;
    }
    protected string returnwithsinglecodetext(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                {
                    empty = Convert.ToString(cb.Items[i].Value);
                }
                else
                {
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
                }
            }
        }
        return empty;
    }

    public void bind_batch()
    {
        try
        {
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct batch_year from tbl_attendance_rights order by batch_year desc", "text");
            ddl_batch.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
        }
        catch
        {
        }
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
        lbl.Add(lbl_degree);
        lbl.Add(lbl_branch);
        //lbl.Add(lbl_sem);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        //fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
}