using System;
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

public partial class Enquiry_report : System.Web.UI.Page
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
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                CalendarExtender1.EndDate = DateTime.Now;
                CalendarExtender2.EndDate = DateTime.Now;
                bind_batch();
                bindcollege();
                bind_seattype();
                binddegree();
                bindbranch();
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
    protected void bind_seattype()
    {
        try
        {
            ds.Clear();
            if (ddl_college.Items.Count > 0)
            {
                q1 = " select textcode,textval from textvaltable where TextCriteria='seat' and college_code ='" + Convert.ToString(ddl_college.SelectedItem.Value) + "'";
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
    protected void binddegree()
    {
        try
        {
            ds.Clear();
            string query = "";
            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + "";
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
                query1 = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddl_college.SelectedValue + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "'";
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
        bind_seattype();
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        if (txt_fromdate.Text != "" && txt_todate.Text != "")
        {
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            TimeSpan ts = dt1 - dt;
            int days = ts.Days;
            if (dt > dt1)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Enter FromDate less than or equal to the ToDate";
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = false;
            }
            else
            {

            }
        }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        if (txt_todate.Text != "" && txt_fromdate.Text != "")
        {
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            TimeSpan ts = dt1 - dt;
            int days = ts.Days;

            if (dt > dt1)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Enter ToDate greater than or equal to the FromDate";
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = false;
            }
            else
            {

            }
        }
    }
    protected void cb_seat_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_seat, cbl_seat, txt_seat, "Seat Type", "--Select--");
    }
    protected void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_seat, cbl_seat, txt_seat, "Seat Type");
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
    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text, "--Select--");
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_branch, cbl_branch, txt_branch, lbl_branch.Text);
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
            string degreedetails = "Enquiry Report";
            string pagename = "Enquiry_report.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            Hashtable totalvalue_dic = new Hashtable();
            if (txt_seat.Text != "--Select--" && txt_degree.Text != "--Select--" && txt_branch.Text != "--Select--")
            {
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].Columns.Count = 2;

                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                string[] split1 = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[0].Width = 50;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = lbl_branch.Text;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 200;
                if (cbl_seat.Items.Count > 0)
                {
                    for (i = 0; i < cbl_seat.Items.Count; i++)
                    {
                        if (cbl_seat.Items[i].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnCount++;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_seat.Items[i].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_seat.Items[i].Value);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 160;
                        }
                    }
                }
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "Available";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 150;
                if (cbl_branch.Items.Count > 0)
                {
                    q1 = "";
                    q1 = " select count(seattype) countseat,seattype,degree_code from applyn where date_applied between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by seattype,degree_code";
                    q1 += "  select d.No_Of_seats,COUNT(r.degree_code)filled,(d.No_Of_seats-COUNT(r.degree_code)) available,r.degree_code from Registration r, Degree d,department dt,course c where c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and c.college_code=d.college_code and dt.college_code=d.college_code and r.degree_code=d.Degree_Code and r.college_code=d.college_code and r.Batch_Year='" + ddl_batch.SelectedItem.Value + "' and r.college_code='" + Convert.ToString(ddl_college.SelectedValue) + "' group by r.degree_code ,No_Of_seats";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    int r = 1; int availabletotal = 0;
                    for (i = 0; i < cbl_branch.Items.Count; i++)
                    {
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
                            for (int k = 2; k < Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1; k++)
                            {
                                string seattypevalue = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, k].Tag);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " seattype='" + seattypevalue + "'and degree_code=" + Convert.ToString(cbl_branch.Items[i].Value) + " ";
                                    DataView dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(dv[0]["countseat"]);
                                        if (totalvalue_dic.Contains(seattypevalue))
                                        {
                                            string value = totalvalue_dic[seattypevalue].ToString();
                                            totalvalue_dic.Remove(seattypevalue);
                                            int total = Convert.ToInt32(value) + Convert.ToInt32(dv[0]["countseat"]);
                                            totalvalue_dic.Add(seattypevalue, total);
                                        }
                                        else
                                        {
                                            totalvalue_dic.Add(seattypevalue, Convert.ToInt32(dv[0]["countseat"]));
                                        }
                                    }
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = "0";
                                    }
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = "0";
                                }
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Name = "Book Antiqua";
                            }
                            ds.Tables[1].DefaultView.RowFilter = " degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "'";
                            DataView dv1 = ds.Tables[1].DefaultView;
                            string avilable = "";
                            if (dv1.Count > 0)
                            {
                                avilable = dv1[0]["available"].ToString();
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = avilable;
                                availabletotal += Convert.ToInt32(avilable);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "0";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                        }
                    }
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "Grant Total";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    for (int k = 2; k < Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1; k++)
                    {
                        string seattypevalue = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, k].Tag);
                        if (totalvalue_dic.Count > 0)
                        {
                            string value = "";
                            if (totalvalue_dic.Contains(seattypevalue))
                            {
                                value = totalvalue_dic[seattypevalue].ToString();
                            }
                            else
                            {
                                value = "0";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Text = value;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, k].Font.Name = "Book Antiqua";
                        }
                    }
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = availabletotal.ToString();
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                    Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].ForeColor = Color.IndianRed;

                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Visible = true;
                    rptprint.Visible = true;
                    lbl_error.Visible = false;
                }
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "Please select All Fields";
                Fpspread1.Visible = false;
                rptprint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Enquiry_report");
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