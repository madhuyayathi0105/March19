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

public partial class Hoster_transport_report : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    bool Cellclick = false;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    Hashtable hat = new Hashtable();
    string q1 = "";
    int insert = 0;
    int i = 0;
    int k = 0;
    int sno = 0;
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
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master = "select * from Master_Settings where settings in('Roll No','Register No','Admission No') " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                if (ds.Tables.Count > 0)
                {
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
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Admissionflag"] = "1";
                        }
                    }
                }
                setLabelText();
                bind_batch();
                bindcollege();
                binddegree();
                bindbranch();
                LoadIncludeSetting();
                bindSec();
            }
        }
        catch (Exception ex) { }
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
        binddegree();
        bindbranch();
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


    protected void checkdicon_Changed(object sender, EventArgs e)
    {
        try
        {
            if (checkdicon.Checked == true)
            {
                txtinclude.Enabled = true;
                LoadIncludeSetting();
            }
            else
            {
                txtinclude.Enabled = false;
                cblinclude.Items.Clear();
                // LoadIncludeSetting();
            }
        }
        catch { }
    }

    private void LoadIncludeSetting()
    {
        try
        {
            cblinclude.Items.Clear();
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Course Completed", "1"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Debar", "2"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Discontinue", "3"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Cancel", "4"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Prolong Absent", "5"));

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

    protected void cbinclude_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Student", "--Select--");
    }
    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Student", "--Select--");

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
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname1 = txtexcelname1.Text;
            if (reportname1.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread2, reportname1);
                lblvalidation2.Visible = false;
            }
            else
            {
                lblvalidation2.Text = "Please Enter Your Report Name";
                lblvalidation2.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch
        {
        }
    }
    protected string getStudCategory()
    {
        string strInclude = string.Empty;
        try
        {
            #region includem
            string cc = "";
            string debar = "";
            string disc = "";
            string cancel = "";
            string pro = "";
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                            cc = " r.cc=1  ";//and  r.ProlongAbsent=0
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like '%debar'";
                        if (cblinclude.Items[i].Value == "3")
                            disc = "r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0 ";
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                        if (cblinclude.Items[i].Value == "5")
                            pro = " r.ProlongAbsent=1 and r.DelFlag=1";
                    }
                }
            }
            if (checkdicon.Checked)
            {
                if (cc != "")
                    strInclude = "(r.cc=1)";// and  r.ProlongAbsent=0
                if (debar != "")
                {
                    if (strInclude != "")
                    {
                        //strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        // strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                }
                if (disc != "")
                {
                    if (strInclude != "")
                    {
                        strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        strInclude += " (r.DelFlag=1 and isnull(r.ProlongAbsent,'0')=0)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += " r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0)";
                    }
                }
                if (cancel != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "  (r.DelFlag=2)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.DelFlag=2)";
                    }
                }
                if (pro != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += " (r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                }
                if (strInclude != "")

                    strInclude = "and (" + strInclude + ")";
            }

            else
            {

                strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0 and isnull(r.ProlongAbsent,'0')=0";


            }
            #endregion
        }
        catch { }
        return strInclude;
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Hoster and Transport Report";
            string pagename = "Hoster_transport_report.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Hoster and Transport Report";
            string pagename = "Hoster_transport_report.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
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
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            Hashtable totalvalue_dic = new Hashtable();
            if (txt_degree.Text != "--Select--" && txt_branch.Text != "--Select--")
            {
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].Columns.Count = 14;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Tag = 1;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[0].Width = 50;


                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = lbl_branch.Text;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Tag = 2;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Section";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Tag = 1;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[0].Width = 50;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Male";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Tag = 3;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[2].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Boys Hostel";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Tag = 4;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[3].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "College Bus";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Tag = 5;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[4].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Out Bus";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Tag = 6;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[5].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Female";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Tag = 7;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[6].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Girls Hostel";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Tag = 8;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[7].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "College Bus";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Tag = 9;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[8].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Out Bus";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = 10;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[9].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Total College Bus";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Tag = 11;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[10].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Total Out Bus";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].Tag = 12;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[11].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Total";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].Tag = 13;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[12].Width = 150;
                string strInclude = getStudCategory();

                int r = 1;
                q1 = " select count(r.App_No)count,r.Stud_Type,a.sex,r.degree_code,r.batch_year,r.sections from Registration r,applyn a where a.app_no=r.App_No " + strInclude + " group by r.Stud_Type,sex,r.degree_code,r.batch_year,r.sections";
                q1 += " select COUNT(r.app_no)count, a.sex,r.degree_code,r.batch_year, Bus_RouteID ,Boarding,r.Stud_Type,r.sections from Registration r,applyn a where a.app_no=r.App_No  and r.batch_year='" + Convert.ToString(ddl_batch.SelectedValue) + "'";
                q1 += "" + strInclude + "group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.sections";

                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                double CollegeBuscount = 0; double outbuscount = 0; double Malecount = 0; double Hosltercount = 0;
                double femaleCollegeBuscount = 0; double femaleoutbuscount = 0; double femalecount = 0; double FemaleHosltercount = 0;
                double totalclgbus = 0; double totaloutbus = 0; double overalltotal = 0; string value = ""; int total = 0;
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {


                        string query = "select distinct ISNULL(sections,'') as sections from registration where batch_year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "'";
                        ds1 = d2.select_method_wo_parameter(query, "text");
                        for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
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

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cbl_branch.Items[i].Text);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(cbl_branch.Items[i].Value);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //string sec = Convert.ToString(ds1.Tables[0].Rows[j]["sections"]);
                                //if (sec != "")
                                //{
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds1.Tables[0].Rows[j]["sections"]);
                                double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and sex='0' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "')  ")), out Malecount);
                                double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and sex='0' and Stud_Type='Hostler' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "')")), out Hosltercount);

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Malecount);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Hosltercount);

                                double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and sex='1' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "') ")), out femalecount);
                                double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and sex='1' and Stud_Type='Hostler' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "')")), out FemaleHosltercount);

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(femalecount);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(FemaleHosltercount);

                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and sex='0' and Stud_Type='Day Scholar' and Bus_RouteID <>'' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "') and Boarding<>''")), out CollegeBuscount);
                                    double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "') and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and sex='0' and Stud_Type='Day Scholar' and Bus_RouteID ='' and Boarding=''")), out outbuscount);

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(CollegeBuscount);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(outbuscount);

                                    double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and sex='1' and Stud_Type='Day Scholar' and Bus_RouteID <>'' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "') and Boarding<>''")), out femaleCollegeBuscount);
                                    double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and sex='1' and Stud_Type='Day Scholar' and Bus_RouteID ='' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "') and Boarding=''")), out femaleoutbuscount);

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(femaleCollegeBuscount);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(femaleoutbuscount);
                                    double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "') and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and Stud_Type='Day Scholar' and Bus_RouteID <>'' and Boarding<>''")), out totalclgbus);

                                    double.TryParse(Convert.ToString(ds.Tables[1].Compute("Sum(count)", "  Batch_Year='" + Convert.ToString(ddl_batch.SelectedValue) + "' and sections in('" + Convert.ToString(ds1.Tables[0].Rows[j]["sections"]) + "') and degree_code='" + Convert.ToString(cbl_branch.Items[i].Value) + "' and Stud_Type='Day Scholar' and Bus_RouteID ='' and Boarding=''")), out totaloutbus);

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(totalclgbus);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(totaloutbus);
                                    overalltotal = 0;
                                    overalltotal = Hosltercount + FemaleHosltercount + totalclgbus;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(overalltotal);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = "0";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = "0";

                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = "0";
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = "0";
                                }
                                #region Granttotal
                                if (totalvalue_dic.Contains("Male-1"))
                                {
                                    value = "";
                                    value = totalvalue_dic["Male-1"].ToString();
                                    totalvalue_dic.Remove("Male-1");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(Malecount);
                                    totalvalue_dic.Add("Male-1", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("Male-1", Convert.ToInt32(Malecount));
                                }
                                if (totalvalue_dic.Contains("Boys Hostel-2"))
                                {
                                    value = "";
                                    value = totalvalue_dic["Boys Hostel-2"].ToString();
                                    totalvalue_dic.Remove("Boys Hostel-2");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(Hosltercount);
                                    totalvalue_dic.Add("Boys Hostel-2", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("Boys Hostel-2", Convert.ToInt32(Hosltercount));
                                }
                                if (totalvalue_dic.Contains("College Bus-3"))
                                {
                                    value = "";
                                    value = totalvalue_dic["College Bus-3"].ToString();
                                    totalvalue_dic.Remove("College Bus-3");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(CollegeBuscount);
                                    totalvalue_dic.Add("College Bus-3", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("College Bus-3", Convert.ToInt32(CollegeBuscount));
                                }
                                if (totalvalue_dic.Contains("Out Bus-4"))
                                {
                                    value = "";
                                    value = totalvalue_dic["Out Bus-4"].ToString();
                                    totalvalue_dic.Remove("Out Bus-4");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(outbuscount);
                                    totalvalue_dic.Add("Out Bus-4", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("Out Bus-4", Convert.ToInt32(outbuscount));
                                }
                                if (totalvalue_dic.Contains("Female-5"))
                                {
                                    value = "";
                                    value = totalvalue_dic["Female-5"].ToString();
                                    totalvalue_dic.Remove("Female-5");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(femalecount);
                                    totalvalue_dic.Add("Female-5", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("Female-5", Convert.ToInt32(femalecount));
                                }
                                if (totalvalue_dic.Contains("Girls Hostel-6"))
                                {
                                    value = "";
                                    value = totalvalue_dic["Girls Hostel-6"].ToString();
                                    totalvalue_dic.Remove("Girls Hostel-6");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(FemaleHosltercount);
                                    totalvalue_dic.Add("Girls Hostel-6", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("Girls Hostel-6", Convert.ToInt32(FemaleHosltercount));
                                }
                                if (totalvalue_dic.Contains("College Bus-7"))
                                {
                                    value = "";
                                    value = totalvalue_dic["College Bus-7"].ToString();
                                    totalvalue_dic.Remove("College Bus-7");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(femaleCollegeBuscount);
                                    totalvalue_dic.Add("College Bus-7", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("College Bus-7", Convert.ToInt32(femaleCollegeBuscount));
                                }
                                if (totalvalue_dic.Contains("Out Bus-8"))
                                {
                                    value = "";
                                    value = totalvalue_dic["Out Bus-8"].ToString();
                                    totalvalue_dic.Remove("Out Bus-8");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(femaleoutbuscount);
                                    totalvalue_dic.Add("Out Bus-8", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("Out Bus-8", Convert.ToInt32(femaleoutbuscount));
                                }
                                if (totalvalue_dic.Contains("Total College Bus-9"))
                                {
                                    value = "";
                                    value = totalvalue_dic["Total College Bus-9"].ToString();
                                    totalvalue_dic.Remove("Total College Bus-9");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(totalclgbus);
                                    totalvalue_dic.Add("Total College Bus-9", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("Total College Bus-9", Convert.ToInt32(totalclgbus));
                                }
                                if (totalvalue_dic.Contains("Total Out Bus-10"))
                                {
                                    value = "";
                                    value = totalvalue_dic["Total Out Bus-10"].ToString();
                                    totalvalue_dic.Remove("Total Out Bus-10");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(totaloutbus);
                                    totalvalue_dic.Add("Total Out Bus-10", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("Total Out Bus-10", Convert.ToInt32(totaloutbus));
                                }
                                if (totalvalue_dic.Contains("Total-11"))
                                {
                                    value = "";
                                    value = totalvalue_dic["Total-11"].ToString();
                                    totalvalue_dic.Remove("Total-11");
                                    total = 0;
                                    total = Convert.ToInt32(value) + Convert.ToInt32(overalltotal);
                                    totalvalue_dic.Add("Total-11", total);
                                }
                                else
                                {
                                    totalvalue_dic.Add("Total-11", Convert.ToInt32(overalltotal));
                                }
                                #endregion
                                //}
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = "0";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = "0";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = "0";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = "0";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = "0";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = "0";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = "0";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = "0";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Text = "0";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].Text = "0";
                            }

                            #region alignment
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";

                            #endregion
                        }
                    }
                }
                #region Grant total Bind
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "Grant Total";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                for (int k = 3; k < Fpspread1.Sheets[0].ColumnHeader.Columns.Count; k++)
                {
                    string seattypevalue = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, k].Text);
                    if (totalvalue_dic.Count > 0)
                    {
                        value = "";
                        if (totalvalue_dic.Contains(seattypevalue + "-" + (k - 2).ToString()))
                        {
                            value = totalvalue_dic[seattypevalue + "-" + (k - 2).ToString()].ToString();
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
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].ForeColor = Color.IndianRed;
                #endregion
                Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Visible = true;
                rptprint.Visible = true;
                lbl_error.Visible = false;
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
            d2.sendErrorMail(ex, collegecode1, "Hoster_transport_report");
        }
    }
    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch
        {
        }
    }

    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            {

                fpspread1go1();
            }
        }
    }
    public void fpspread1go1()
    {
        int activerow = 0;
        int activecol = 0;
        string Branch_tagvalue = string.Empty;
        activerow = Convert.ToInt32(Fpspread1.ActiveSheetView.ActiveRow.ToString());
        activecol = Convert.ToInt32(Fpspread1.ActiveSheetView.ActiveColumn.ToString());
        Fpspread2.Visible = true;
        Fpspread2.CommandBar.Visible = false;
        Fpspread2.Sheets[0].AutoPostBack = true;
        Fpspread2.Sheets[0].RowHeader.Visible = false;

        Fpspread2.Sheets[0].RowCount = 0;
        Fpspread2.Sheets[0].RowCount = 0;
        Fpspread2.Sheets[0].ColumnCount = 0;
        Fpspread2.Sheets[0].ColumnCount = 7;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        Fpspread2.Columns[0].Width = 50;
        Branch_tagvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
        string query = string.Empty;
        string strInclude = getStudCategory();
        if (activecol == 3)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,r.Roll_No,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and r.degree_code='" + Branch_tagvalue + "'and sex='0' and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + " " + strInclude + "group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        if (activecol == 4)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,r.Roll_No,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and r.Stud_Type='Hostler'and r.degree_code='" + Branch_tagvalue + "'and sex='0' and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + " " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        else if (activecol == 5)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,r.Roll_No,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and Bus_RouteID <>'' and Boarding<>''and r.Stud_Type='Day Scholar'and r.degree_code='" + Branch_tagvalue + "'and sex='0' and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + " " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        else if (activecol == 6)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,r.Roll_No,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and Bus_RouteID ='' and Boarding=''and r.Stud_Type='Day Scholar'and r.degree_code='" + Branch_tagvalue + "'and sex='0' and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + "  " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        else if (activecol == 7)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,r.Roll_No,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and r.degree_code='" + Branch_tagvalue + "'and sex='1' and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + " " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        else if (activecol == 8)
        {

            query = " select  a.sex,r.degree_code,r.batch_year,r.Roll_No, Reg_No,C.course_Name+'-'+dt.Dept_name AS degree, r.Roll_Admit,Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and r.Stud_Type='Hostler'and r.degree_code='" + Branch_tagvalue + "'and sex='1' and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + " " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        else if (activecol == 9)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,r.Roll_No,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and Bus_RouteID <>'' and Boarding<>''and r.Stud_Type='Day Scholar'and r.degree_code='" + Branch_tagvalue + "'and sex='1' and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + "  " + strInclude + "group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        else if (activecol == 10)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,r.Roll_No,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and Bus_RouteID ='' and Boarding=''and r.Stud_Type='Day Scholar'and r.degree_code='" + Branch_tagvalue + "'and sex='1' and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + "  " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        else if (activecol == 11)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name,r.Roll_No from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and Bus_RouteID <>'' and Boarding<>''and r.Stud_Type='Day Scholar'and r.degree_code='" + Branch_tagvalue + "'  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + " " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        else if (activecol == 12)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,r.Roll_No,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and Bus_RouteID ='' and Boarding=''and r.Stud_Type='Day Scholar'and r.degree_code='" + Branch_tagvalue + "' and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + " " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";
        }
        else if (activecol == 13)
        {
            query = " select  a.sex,r.degree_code,r.batch_year,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name,r.Roll_No from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and Bus_RouteID <>'' and Boarding<>''and r.Stud_Type='Day Scholar'and r.degree_code='" + Branch_tagvalue + "'  and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + " " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";

            query += " select  a.sex,r.degree_code,r.batch_year,Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name AS degree, Bus_RouteID ,Boarding,r.Stud_Type,r.Stud_Name,r.Roll_No from degree d,Department dt,Course C ,Registration r,applyn a where a.app_no=r.App_No and r.Stud_Type='Hostler'  and r.degree_code='" + Branch_tagvalue + "'   and d.Degree_Code =a.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and r.batch_year=" + ddl_batch.SelectedValue + "  " + strInclude + " group by sex,r.degree_code,Bus_RouteID,Boarding,r.batch_year,r.Stud_Type,r.Stud_Name,r.Roll_No,r.Reg_No,r.Roll_Admit,C.course_Name+'-'+dt.Dept_name order by r.Roll_No";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "text");
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    Fpspread2.Sheets[0].RowCount++;
                    sno++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "RollNo";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Columns[0].Width = 50;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Admission No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Columns[0].Width = 50;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Register No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Columns[0].Width = 50;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Columns[0].Width = 50;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread2.Columns[0].Width = 50;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Degree";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FarPoint.Web.Spread.TextCellType tmxt = new FarPoint.Web.Spread.TextCellType();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = tmxt;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[1].Rows[i]["Roll_No"].ToString();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpspread2.Columns[0].Width = 50;
                    FarPoint.Web.Spread.TextCellType tmx = new FarPoint.Web.Spread.TextCellType();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = tmx;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[i]["Roll_Admit"].ToString();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspread2.Columns[0].Width = 50;
                    FarPoint.Web.Spread.TextCellType tmex = new FarPoint.Web.Spread.TextCellType();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].CellType = tmex;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[i]["Reg_No"].ToString();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpspread2.Columns[0].Width = 50;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[i]["Stud_Name"].ToString();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    Fpspread2.Columns[0].Width = 50;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[i]["Batch_Year"].ToString();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    Fpspread2.Columns[0].Width = 50;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = ds.Tables[1].Rows[i]["degree"].ToString();
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Width = 900;
                    Fpspread2.Height = 420;
                    Fpspread2.SaveChanges();

                }
            }

        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Fpspread2.Sheets[0].RowCount++;
                sno++;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "RollNo";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Admission No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Register No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Dergee";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                Fpspread2.Columns[0].Width = 50;
                FarPoint.Web.Spread.TextCellType tx = new FarPoint.Web.Spread.TextCellType();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Roll_Admit"].ToString();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                Fpspread2.Columns[0].Width = 50;
                FarPoint.Web.Spread.TextCellType tex = new FarPoint.Web.Spread.TextCellType();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].CellType = tex;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["degree"].ToString();
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                Fpspread2.Width = 900;
                Fpspread2.Height = 420;
                Fpspread2.SaveChanges();
                Fpspread2.Visible = true;
                rptprint1.Visible = true;
            }
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
    public void cb_sec_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_sec.Checked == true)
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Section(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
                txt_sec.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_sec.Text = "--Select--";
            cb_sec.Checked = false;
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_sec.Items.Count)
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
                cb_sec.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_sec.Text = "--Select--";
            }
            else
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    protected void bindSec()
    {
        string branch = returnwithsinglecodevalue(cbl_branch);
        string sqlquery = "select distinct sections from registration where batch_year in('" + ddl_batch.SelectedItem.Text + "') and degree_code in('" + branch + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
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
}