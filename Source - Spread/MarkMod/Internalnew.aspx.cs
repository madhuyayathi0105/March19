using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using System.Collections.Generic;
using System.Configuration;

public partial class internalnew : System.Web.UI.Page
{
    static bool forschoolsetting = false;
    bool cellclick = false;

    double maximumsubjectmark = 0;

    DAccess2 dacc = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataTable scandacc = new DataTable();

    static ArrayList arr = new ArrayList();
    Hashtable hat = new Hashtable();

    bool isLockCIAMark = false;

    string grouporusercode = string.Empty;
    string fpbatch_year = string.Empty;
    string fpdegreecode = string.Empty;
    string fpbranch = string.Empty;
    string fpsem = string.Empty;
    string fpsec = string.Empty;
    string degreecode = string.Empty;
    string term = string.Empty;
    string grade_ids = string.Empty;
    string activity_ids = string.Empty;
    string batchyear = string.Empty;

    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolactivity = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocoldesc = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
    static Dictionary<int, string> dicmaxval = new Dictionary<int, string>();
    FpSpread fpspreadsample;

    double mxmark2 = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!Request.FilePath.Contains("CAMHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/MarkMod/CAMHome.aspx");
                return;
            }
        }
        isLockCIAMark = checkCIALock();
        if (!IsPostBack)
        {
            show1.Visible = false;
            show2.Visible = false;
            lblBatch.Text = "Batch";
            lblDegree.Text = "Degree";
            lblBranch.Text = "Branch";
            lblSemYr.Text = "Sem";
            lblErrorMsg.Text = string.Empty;
            lblErrorMsg.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
            divPopAlert.Visible = false;
            divConfirm.Visible = false;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = dacc.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]);
                if (schoolvalue.Trim() == "0")
                {
                    forschoolsetting = true;
                    lblBatch.Text = "Year";
                    lblDegree.Text = "School Type";
                    lblBranch.Text = "Standard";
                    lblSemYr.Text = "Term";
                }
            }
            BindBatch();
            BindDegree();
            if (ddlBatch.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " Year " : " Batch ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlDegree.Items.Count > 0)
            {
                bindbranch();
                if (isLockCIAMark)
                {
                    BindSemester();
                }
                else
                {
                    bindsem();
                }
                BindSectionDetail();
                lblErrorMsg.Text = string.Empty;
            }
            else
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " School Type " : " Degree ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlBranch.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " Standard " : " Department ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            fpspread.Sheets[0].RowHeader.Visible = false;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            //fpspread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.Black;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread.Sheets[0].AutoPostBack = false;
            fpspread.CommandBar.Visible = false;
            fpspread.Sheets[0].RowCount = 0;
            fpspread.Sheets[0].ColumnCount = 3;
            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
            //FarPoint.Web.Spread.StyleInfo darkstyle1 = new FarPoint.Web.Spread.StyleInfo();
            //darkstyle1.BackColor = ColorTranslator.FromHtml("#00aff0");
            ////darkstyle.ForeColor = System.Drawing.Color.Black;
            //darkstyle1.Font.Name = "Book Antiqua";
            //darkstyle1.Font.Size = FontUnit.Medium;
            //darkstyle1.Border.BorderSize = 0;
            //darkstyle1.Border.BorderColor = System.Drawing.Color.Transparent;
            //fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle1;
            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
            for (int i = 0; i < 3; i++)
            {
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].ForeColor = Color.White;
            }
            fpspread.Sheets[0].Columns[0].Locked = true;
            fpspread.Sheets[0].Columns[1].Locked = true;
            fpspread.Sheets[0].Columns[2].Locked = true;
            //fpspread.Height = 550;
            //fpspread.Width = 505;
            fpspread.Visible = false;
            //FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            //darkstyle.BackColor = ColorTranslator.FromHtml("#add8e6");
            ////darkstyle.ForeColor = System.Drawing.Color.White;
            //darkstyle.Font.Name = "Book Antiqua";
            //darkstyle.Font.Size = FontUnit.Medium;
            //darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
            //fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 90;
            fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 120;
            //fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 150;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;
            bindactivity();
            hideexportimport();
        }

        term = ddlSemYr.SelectedItem.Text.ToString().Trim();
    }

    public void BindSectionDetail()
    {
        string batchyear = string.Empty;
        string branch = string.Empty;
        string batch = string.Empty;
        DataSet ds = new DataSet();
        ddlSec.Items.Clear();
        txtSec.Enabled = false;
        if (ddlBatch.Items.Count > 0)
        {
            batch = Convert.ToString(ddlBatch.SelectedValue).Trim();
        }
        if (ddlBranch.Items.Count > 0)
        {
            branch = Convert.ToString(ddlBranch.SelectedValue).Trim();
        }
        string Master1 = string.Empty;
        if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            string group = Convert.ToString(Session["group_code"]).Trim();
            if (group.Contains(';'))
            {
                string[] group_semi = group.Split(';');
                Master1 = Convert.ToString(group_semi[0]);
            }
        }
        else
        {
            Master1 = Convert.ToString(Session["usercode"]).Trim();
        }
        string collegecode = Convert.ToString(Session["collegecode"]).Trim();
        string qrysections = string.Empty;
        if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(Master1))
        {
            qrysections = da.GetFunctionv("select distinct sections from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "' and batch_year='" + batch + "'").Trim();
        }
        if (!string.IsNullOrEmpty(qrysections.Trim()))
        {
            string[] sectionsAll = qrysections.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string sections = string.Empty;
            if (sectionsAll.Length > 0)
            {
                for (int sec = 0; sec < sectionsAll.Length; sec++)
                {
                    if (!string.IsNullOrEmpty(sectionsAll[sec].Trim()))
                    {
                        if (sections.Trim() == "")
                        {
                            sections = "'" + sectionsAll[sec] + "'";
                        }
                        else
                        {
                            sections += ",'" + sectionsAll[sec] + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(sections.Trim()))
            {
                string sqlnew = "select distinct sections from registration where batch_year=" + Convert.ToString(ddlBatch.SelectedValue).Trim() + " and degree_code=" + Convert.ToString(ddlBranch.SelectedValue).Trim() + " and sections<>'-1' and sections<>' ' and sections in(" + sections + ") and delflag=0 and exam_flag<>'Debar' order by sections";
                ds.Clear();
                ds = dacc.select_method_wo_parameter(sqlnew, "Text");
            }
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
            //ddlSec.Items.Insert(0, "All");
            ddlSec.Enabled = true;
            cblSec.DataSource = ds;
            cblSec.DataTextField = "sections";
            cblSec.DataValueField = "sections";
            cblSec.DataBind();
            for (int h = 0; h < cblSec.Items.Count; h++)
            {
                cblSec.Items[h].Selected = true;
            }
            txtSec.Text = "Section" + "(" + cblSec.Items.Count + ")";
            chkSec.Checked = true;
            txtSec.Enabled = true;
            //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        else
        {
            ddlSec.Enabled = false;
            txtSec.Enabled = false;
        }
    }

    public void BindBatch()
    {
        try
        {
            string Master1 = string.Empty;
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = Convert.ToString(group_semi[0]);
                }
            }
            else
            {
                Master1 = Convert.ToString(Session["usercode"]).Trim();
            }
            string collegecode = Convert.ToString(Session["collegecode"]).Trim();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "'";
            DataSet ds = dacc.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
            }
            //int year1;
            //year1 = Convert.ToInt16(DateTime.Today.Year);
            //ddlBatch.Items.Clear();
            //for (int l = 0; l <= 9; l++)
            //{
            //    ddlBatch.Items.Add(Convert.ToString(year1 - l));
            //}
            //ddlBatch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Clear();
            ddlBranch.Items.Clear();
            hat.Clear();
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            string singleuser = Session["single_user"].ToString();
            string group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string course_id = ddlDegree.SelectedValue.ToString();
            string query = string.Empty;
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "'";
            }
            else
            {
                query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "'";
            }
            ds = dacc.select_method_wo_parameter(query, "Text");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
        }
        catch
        {
        }
    }

    public void BindDegree()
    {
        string college_code = Session["collegecode"].ToString();
        string query = string.Empty;
        string usercode = Session["usercode"].ToString();
        string singleuser = Session["single_user"].ToString();
        string group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "'";
        }
        else
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "'  and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "'";
        }
        DataSet ds = new DataSet();
        ds.Clear();
        ds = dacc.select_method_wo_parameter(query, "Text");
        // DataSet ds = ClsAttendanceAccess.GetDegreeDetail(collegecode.ToString());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
            // ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
    }

    public void bindsem()
    {
        //--------------------semester load
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        degreecode = string.Empty;
        batchyear = string.Empty;
        string sqlnew = string.Empty;
        ds.Clear();
        ds.Reset();
        ds.Dispose();
        if (ddlBatch.Items.Count > 0)
        {
            batchyear = Convert.ToString(ddlBatch.SelectedItem.Text.Trim());
        }
        if (ddlBranch.Items.Count > 0)
        {
            degreecode = Convert.ToString(ddlBranch.SelectedItem.Value.Trim());
        }
        if (!string.IsNullOrEmpty(degreecode.Trim()) && !string.IsNullOrEmpty(batchyear.Trim()))
        {
            sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + degreecode.Trim() + "' and batch_year='" + batchyear.Trim() + "' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'";
            ds = dacc.select_method_wo_parameter(sqlnew, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
            int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i == 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(degreecode.Trim()))
            {
                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code='" + degreecode.Trim() + "' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'";
                ds.Clear();
                ds = dacc.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }
        }
        if (ddlSemYr.Items.Count > 0)
        {
            ddlSemYr.SelectedIndex = 0;
            BindSectionDetail();
        }
    }

    private void BindSemester()
    {
        try
        {
            ddlSemYr.Items.Clear();
            degreecode = string.Empty;
            batchyear = string.Empty;
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            if (ddlBatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlBatch.SelectedItem.Text.Trim());
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlBranch.SelectedItem.Value.Trim());
            }
            if (!string.IsNullOrEmpty(degreecode.Trim()) && !string.IsNullOrEmpty(batchyear.Trim()))
            {
                string qry = "select distinct r.Current_Semester as Semester from Registration r,Degree dg where r.degree_code=dg.Degree_Code and r.Current_Semester<=dg.Duration and CC='0' and DelFlag='0' and Exam_Flag<>'debar' and dg.Degree_Code='" + degreecode + "' and r.batch_year='" + batchyear + "' order by r.Current_Semester";
                ds = dacc.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSemYr.DataSource = ds;
                ddlSemYr.DataValueField = "Semester";
                ddlSemYr.DataTextField = "Semester";
                ddlSemYr.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindactivity()
    {
        //string Degree_Code =string.Empty;
        //string Batch_Year =string.Empty;
        //term = ddlSemYr.SelectedItem.Text.ToString().Trim();
        //Degree_Code = ddlBranch.SelectedItem.Value.ToString();
        //Batch_Year = ddlBatch.SelectedItem.Text.ToString();
        //string sqlselect = "select * from  activity_entry where  Degree_Code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' and term='" + term + "'";
        //DataSet dsselect = new DataSet();
        //dsselect.Clear();
        //dsselect = da.select_method_wo_parameter(sqlselect, "Text");
        //if (dsselect.Tables[0].Rows.Count > 0)
        //{
        //    string currentsem = ddlSemYr.SelectedItem.Text.ToString();
        //    string degreecode = ddlBranch.SelectedItem.Value.ToString();
        //    string batchyear = ddlBatch.SelectedItem.Text.ToString();
        //    string strtit_acitivity =string.Empty;
        //    for (int ij = 0; ij < dsselect.Tables[0].Rows.Count; ij++)
        //    {
        //        if (strtit_acitivity == "")
        //        {
        //            strtit_acitivity = dsselect.Tables[0].Rows[ij][1].ToString();
        //        }
        //        else
        //        {
        //            strtit_acitivity = strtit_acitivity + "','" + dsselect.Tables[0].Rows[ij][1].ToString();
        //        }
        //    }
        //    string queryactivity = " select * from textvaltable where TextCriteria='RActv' and college_code='" + Session["collegecode"].ToString() + "' and TextCode in ('" + strtit_acitivity + "') ";
        //    DataSet newact = new DataSet();
        //    newact.Clear();
        //    newact = da.select_method_wo_parameter(queryactivity, "Text");
        //    if (newact.Tables[0].Rows.Count > 0)
        //    {
        //        ddlactivity.DataSource = newact;
        //        ddlactivity.DataTextField = "TextVal";
        //        ddlactivity.DataValueField = "TextCode";
        //        ddlactivity.DataBind();
        //        ddlactivity.Visible = false;
        //    }
        //    else
        //    {
        //        //lblparterr.Visible = false;
        //    }
        //}
        //else
        //{
        //    ddlactivity.Visible = false;
        //}
    }

    public void hideexportimport()
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        fpmarkexcel.Visible = false;
        btn_import.Visible = false;
        //btn_import.Attributes.Add("style","margin-left:-1000px;");
        //fpmarkexcel.Attributes.Add("style", "margin-left:-1000px;");
    }

    public void showexportimport()
    {
        lblrptname.Visible = true;
        txtexcelname.Visible = true;
        btnExcel.Visible = true;
        fpmarkexcel.Visible = true;
        btn_import.Visible = true;
    }

    public string loadmarkat(string mr)
    {
        string strgetval = string.Empty;
        if (mr == "-1")
        {
            strgetval = "AAA";
        }
        else if (mr == "-2")
        {
            strgetval = "EL";
        }
        else if (mr == "-3")
        {
            strgetval = "EOD";
        }
        else if (mr == "-4")
        {
            strgetval = "ML";
        }
        else if (mr == "-5")
        {
            strgetval = "SOD";
        }
        else if (mr == "-6")
        {
            strgetval = "NSS";
        }
        else if (mr == "-7")
        {
            strgetval = "NJ";
        }
        else if (mr == "-8")
        {
            strgetval = "S";
        }
        else if (mr == "-9")
        {
            strgetval = "L";
        }
        else if (mr == "-10")
        {
            strgetval = "NCC";
        }
        else if (mr == "-11")
        {
            strgetval = "HS";
        }
        else if (mr == "-12")
        {
            strgetval = "PP";
        }
        else if (mr == "-13")
        {
            strgetval = "SYOD";
        }
        else if (mr == "-14")
        {
            strgetval = "COD";
        }
        else if (mr == "-15")
        {
            strgetval = "OOD";
        }
        else if (mr == "-16")
        {
            strgetval = "OD";
        }
        else if (mr == "-17")
        {
            strgetval = "LA";
        }
        else if (mr == "-18")
        {
            strgetval = "RAA";
        }
        return strgetval;
    }

    public void hidealls()
    {
        show1.Visible = false;
        show2.Visible = false;
        lblErrorMsg.Visible = false;
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            BindDegree();
            bindbranch();
            if (isLockCIAMark)
            {
                BindSemester();
            }
            else
            {
                bindsem();
            }
            lblErrorMsg.Visible = false;
            fpspread.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
            FpSpread1.Visible = false;
            btnok.Visible = false;
        }
        else
        {
            BindDegree();
            bindbranch();
            if (isLockCIAMark)
            {
                BindSemester();
            }
            else
            {
                bindsem();
            }
            lblErrorMsg.Visible = false;
            fpspread.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
        }
        hideexportimport();
        hidealls();
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        string course_id = ddlDegree.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["UserCode"].ToString();
        string sqlnew = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + course_id + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "";
        DataSet ds = new DataSet();
        ds.Clear();
        ds = dacc.select_method_wo_parameter(sqlnew, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
        if (isLockCIAMark)
        {
            BindSemester();
        }
        else
        {
            bindsem();
        }
        BindSectionDetail();
        lblErrorMsg.Visible = false;
        fpspread.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
        FpSpread1.Visible = false;
        btnok.Visible = false;
        hideexportimport();
        hidealls();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (isLockCIAMark)
        {
            BindSemester();
        }
        else
        {
            bindsem();
        }
        BindSectionDetail();
        lblErrorMsg.Visible = false;
        fpspread.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
        FpSpread1.Visible = false;
        btnok.Visible = false;
        bindactivity();
        hideexportimport();
        hidealls();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorMsg.Visible = false;
        fpspread.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
        FpSpread1.Visible = false;
        btnok.Visible = false;
        hideexportimport();
        hidealls();
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSectionDetail();
        lblErrorMsg.Visible = false;
        fpspread.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
        hidealls();
    }

    protected void chkSec_CheckedChanged(object sender, EventArgs e)
    {
        int count = 0;
        if (chkSec.Checked == true)
        {
            count++;
            for (int i = 0; i < cblSec.Items.Count; i++)
            {
                cblSec.Items[i].Selected = true;
            }
            txtSec.Text = "Section(" + (cblSec.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblSec.Items.Count; i++)
            {
                cblSec.Items[i].Selected = false;
            }
            txtSec.Text = "--Select--";
        }
    }

    protected void cblSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        chkSec.Checked = false;
        for (int i = 0; i < cblSec.Items.Count; i++)
        {
            if (cblSec.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblSec.Items.Count)
            {
                chkSec.Checked = true;
            }
            txtSec.Text = "Section(" + Convert.ToString(commcount) + ")";
        }
    }

    protected void ddlactivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorMsg.Visible = false;
        fpspread.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
    }

    protected void FpSpread1_OnButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value) == 1)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                    FpSpread1.Visible = true;
                }
            }
            else if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value) == 0)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                    FpSpread1.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnok_Click1(object sender, EventArgs e)
    {
        try
        {
            fpspread.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
            hideexportimport();
            show2.Visible = false;
            for (int Att_row = 0; Att_row < gvmarkentry.Columns.Count; Att_row++)
            {
                gvmarkentry.Columns[Att_row].Visible = true;
            }
            int selectsubcount = 0;
            for (int Att_row = 0; Att_row < gvatte.Rows.Count; Att_row++)
            {
                if ((gvatte.Rows[Att_row].Cells[1].FindControl("chksubject") as CheckBox).Checked == true)
                {
                    selectsubcount++;
                    if (selectsubcount > 7)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Only 7 Subjects')", true);
                        return;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[Att_row + 1, 1].Value = 1;
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Cells[Att_row + 1, 1].Value = 0;
                }
            }
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string groupCode = Convert.ToString(Session["group_code"]).Trim();
                string[] groupUser = Convert.ToString(Session["group_code"]).Trim().Split(';');
                if (groupUser.Length > 0)
                {
                    groupCode = groupUser[0].Trim();
                }
                grouporusercode = " and  group_code='" + Convert.ToString(groupCode).Trim() + "'";
                grouporusercode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            else
            {
                grouporusercode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string CIALock = da.GetFunction("select value from Master_Settings where settings='CAM CIA Mark Entry Lock' " + grouporusercode + "");
            string usercode = string.Empty;
            string userNAme = string.Empty;
            bool isLockMark = false;
            if (Session["usercode"] != null)
            {
                usercode = Convert.ToString(Session["usercode"]).Trim();
                userNAme = da.GetFunctionv(" select USER_ID from UserMaster where User_code='" + usercode + "'");
                //if (userNAme.Trim().ToLower() == "admin")
                //{
                //    isLockMark = false;
                //    lblLockErr.Text = string.Empty;
                //    lblLockErr.Visible = false;
                //}
                //else
                //{
                //    isLockMark = true;
                //    lblLockErr.Text = "If You Want To Change Mark.Please Contact COE Office!!! Because You Can Save Mark Only Once";
                //    lblLockErr.Visible = true;
                //}
            }
            if (string.IsNullOrEmpty(CIALock) || CIALock.Trim() == "0")
            {
                isLockMark = false;
                lblLockErr.Text = string.Empty;
                lblLockErr.Visible = false;
            }
            else if (!string.IsNullOrEmpty(CIALock) && CIALock.Trim() == "1")
            {
                isLockMark = true;
                lblLockErr.Text = "Note\t\t:\tFor Corrections/Modifications in the CIA mark Contact Office of The Controller of Examinations";
                lblLockErr.Visible = true;
            }
            else
            {
                isLockMark = true;
                //lblLockErr.Text = "If You Want To Change Mark.Please Contact COE Office!!! Because You Can Save Mark Only Once";
                lblLockErr.Text = "Note\t\t:\tFor Corrections/Modifications in the CIA mark Contact Office of The Controller of Examinations";
                lblLockErr.Visible = true;
            }
            FpSpread1.SaveChanges();
            //  gvmarkentry.Columns.Clear();
            scandacc.Columns.Add("Ac");
            scandacc.Columns.Add("code");
            scandacc.Rows.Clear();
            int cnt = 0;
            fpmarkexcel.Visible = true;
            btn_import.Visible = true;
            FpSpread1.SaveChanges();
            for (int Att_row = 0; Att_row < gvatte.Rows.Count; Att_row++)
            {
                if ((gvatte.Rows[Att_row].Cells[1].FindControl("chksubject") as CheckBox).Checked == true)
                {
                    string subject_accnmae = (gvatte.Rows[Att_row].Cells[3].FindControl("lblsubcode") as Label).Text;
                    string subjectnumbers = (gvatte.Rows[Att_row].Cells[4].FindControl("lblsubno") as Label).Text;
                    //arr.Add(subject_accnmae);
                    scandacc.Rows.Add(subject_accnmae, subjectnumbers);
                }
            }
            FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();
            fpspread.Sheets[0].RowCount = 0;
            fpspread.Sheets[0].ColumnCount = 3;
            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            fpspread.Sheets[0].RowHeader.Visible = false;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
            fpspread.CommandBar.Visible = false;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 50;
            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Height = 400;
            fpspread.Width = 800;
            //FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            //darkstyle.BackColor = ColorTranslator.FromHtml("#ADD8E6");
            ////darkstyle.ForeColor = System.Drawing.Color.Black;
            //darkstyle.Font.Name = "Book Antiqua";
            //darkstyle.Font.Size = FontUnit.Medium;
            //darkstyle.Border.BorderSize = 0;
            //darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
            //fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = Color.Black;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = Color.Black;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = Color.Black;
            DataSet dsmark = new DataSet();
            fpspread.Sheets[0].ColumnCount = 3;
            fpspread.SaveChanges();
            string subjectNo = string.Empty;
            dicmaxval.Clear();
            int ct3 = 0;
            for (int res = 1; res < Convert.ToInt32(FpSpread1.Sheets[0].RowCount); res++)
            {
                ct3++;
                int isval = 0;
                isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 1].Value);
                if (isval == 1)
                {
                    show2.Visible = true;
                    cnt++;
                    fpspread.Sheets[0].ColumnCount++;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 3].Tag).Trim();
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 4].Tag).Trim();
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 2].Tag).Trim();
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    // fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].ForeColor = Color.Black;
                    fpspread.Sheets[0].Columns[0].Locked = true;
                    fpspread.Sheets[0].Columns[1].Locked = true;
                    fpspread.Sheets[0].Columns[2].Locked = true;

                    intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
                    //if (FpSpread1.Sheets[0].RowCount > 1)
                    //{

                    double MaxMark = 0;
                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[res, 2].Tag).Trim(), out MaxMark);
                    intgrcel.MaximumValue = MaxMark;// Convert.ToDouble(FpSpread1.Sheets[0].Cells[res, 2].Tag);
                    maximumsubjectmark = MaxMark;
                    dicmaxval.Add(ct3, Convert.ToString(maximumsubjectmark));
                    //}
                    //else
                    //{
                    //    intgrcel.MaximumValue = 100;
                    //}
                    //intgrcel.MaximumValue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[2, 2].Tag);
                    intgrcel.MinimumValue = -1;
                    intgrcel.ErrorMessage = "Enter valid mark";

                    fpspread.Visible = false;
                    if (string.IsNullOrEmpty(subjectNo))
                    {
                        subjectNo = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 4].Tag).Trim();
                    }
                    else
                    {
                        subjectNo += "," + Convert.ToString(FpSpread1.Sheets[0].Cells[res, 4].Tag).Trim();
                    }
                }
            }
            string secsql = string.Empty;
            fpbatch_year = ddlBatch.SelectedItem.Text.ToString();
            fpdegreecode = ddlDegree.SelectedItem.Value.ToString();
            fpbranch = ddlBranch.SelectedItem.Value.ToString();
            fpsem = ddlSemYr.SelectedItem.Text.ToString();
            //if (ddlSec.Items.Count > 0)
            //{
            //    if (ddlSec.Enabled == true)
            //    {
            //        fpsec = Convert.ToString(ddlSec.SelectedItem.Text).ToLower().Trim();
            //        if (fpsec == "all" || fpsec == "")
            //        {
            //            // ------------- add start
            //            secsql =string.Empty;
            //        }
            //        else
            //        {
            //            secsql = "and Registration.Sections in ('" + fpsec + "')";
            //        }
            //    }
            //}
            //if (ddlSec.Enabled == true)
            //{
            //    fpsec = ddlSec.SelectedItem.Text.ToString();
            //    if (fpsec.Trim() != "")
            //    {
            //        secsql = "and Registration.Sections in ('" + fpsec + "')";
            //    }
            //    else
            //    {
            //        secsql =string.Empty;
            //    }
            //}
            secsql = string.Empty;
            fpsec = string.Empty;
            int count = 0;
            for (int i = 0; i < cblSec.Items.Count; i++)
            {
                if (cblSec.Items[i].Selected == true)
                {
                    count++;
                    if (fpsec == "")
                    {
                        fpsec = "'" + Convert.ToString(cblSec.Items[i].Value).Trim() + "'";
                    }
                    else
                    {
                        fpsec = fpsec + ",'" + Convert.ToString(cblSec.Items[i].Value).Trim() + "'";
                    }
                }
            }
            if (cblSec.Items.Count > 0)
            {
                if (count == 0)
                {
                    lblErrorMsg.Text = "Please Select Atleast One Section And Then Proceed";
                    lblErrorMsg.Visible = true;
                    return;
                }
            }
            if (!string.IsNullOrEmpty(fpsec.Trim()))
            {
                secsql = " and Registration.Sections in (" + fpsec + ")";
            }
            else
            {
                secsql = string.Empty;
            }
            //intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            //if (FpSpread1.Sheets[0].RowCount > 1)
            //{
            //    intgrcel.MaximumValue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[1, 2].Tag);
            //    maximumsubjectmark = Convert.ToDouble(FpSpread1.Sheets[0].Cells[1, 2].Tag);
            //}
            //else
            //{
            //    intgrcel.MaximumValue = 100;
            //}
            ////intgrcel.MaximumValue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[2, 2].Tag);
            //intgrcel.MinimumValue = -1;
            //intgrcel.ErrorMessage = "Enter valid mark";
            fpspread.SaveChanges();
            fpspread.Sheets[0].Columns[2].Width = 200;
            string strorderby = da.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = "ORDER BY registration.Roll_No";
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY registration.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY registration.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY Registration.Stud_Name";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = "ORDER BY registration.Roll_No,registration.Reg_No,Registration.Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = "ORDER BY registration.Roll_No,registration.Reg_No";
                }
                else if (strorderby == "1,2")
                {
                    strorderby = "ORDER BY registration.Reg_No,Registration.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY registration.Roll_No,Registration.Stud_Name";
                }
            }
            string sqlquery = "select Registration.roll_no,Registration.reg_no, Registration.stud_name,Registration.stud_type,registration.serialno,Registration.Adm_Date,Sections from registration, applyn a where a.app_no=registration.app_no and registration.degree_code='" + fpbranch + "'  and registration.batch_year='" + fpbatch_year + "' " + secsql + "   " + strorderby + " ;";
            DataSet studentdetails = new DataSet();
            studentdetails.Clear();
            studentdetails = dacc.select_method_wo_parameter(sqlquery, "Text");
            DataSet dsInternalMark = new DataSet();
            DataSet dssubjectchoo = new DataSet();
            DataView dvsub = new DataView();
            dssubjectchoo.Clear();
            DataSet dsCAMMarks = new DataSet();
            DataSet dsCOEMarks = new DataSet();
            DataView dvCAMMarks = new DataView();
            DataView dvCOEMarks = new DataView();
            if (!string.IsNullOrEmpty(subjectNo))
            {
                string qry = " select Registration.roll_no,Registration.reg_no, Registration.stud_name,Registration.stud_type,registration.serialno,Registration.Adm_Date,Registration.Sections from registration,applyn a,camarks cm where cm.roll_no=Registration.Roll_No and a.app_no=registration.app_no  and registration.degree_code='" + fpbranch + "' and registration.batch_year='" + fpbatch_year + "' and cm.subject_no in(" + subjectNo + ") and cm.UserId='" + usercode + "' " + secsql + "   " + strorderby + " ;";
                dsInternalMark = da.select_method_wo_parameter(qry, "text");
                dssubjectchoo.Clear();
                dssubjectchoo = da.select_method_wo_parameter("select * from subjectchooser where subject_no in(" + subjectNo + ")", "Text");
                qry = "select total,subject_no,Roll_No from camarks where subject_no in(" + subjectNo + ")";
                dsCAMMarks = da.select_method_wo_parameter(qry, "text");
                qry = "select internal_mark,subject_no,roll_no from mark_entry where subject_no in(" + subjectNo + ")";
                dsCOEMarks = da.select_method_wo_parameter(qry, "text");
            }
            if (isLockMark)
            {
                if (dsInternalMark.Tables.Count > 0 && dsInternalMark.Tables[0].Rows.Count > 0)
                {
                    btnfpspread1save.Enabled = false;
                    btnfpspread1delete.Enabled = false;
                    lblLockErr.Text = "Note\t\t:\tFor Corrections/Modifications in the CIA mark Contact Office of The Controller of Examinations";
                    lblLockErr.Visible = true;
                }
                else
                {
                    lblLockErr.Text = string.Empty;
                    lblLockErr.Visible = false;
                    btnfpspread1save.Enabled = true;
                }
            }
            else
            {
                lblLockErr.Text = string.Empty;
                lblLockErr.Visible = false;
                btnfpspread1save.Enabled = true;
                btnfpspread1delete.Enabled = true;
            }
            if (studentdetails.Tables.Count > 0 && studentdetails.Tables[0].Rows.Count > 0)
            {
                gvmarkentry.DataSource = studentdetails.Tables[0];
                gvmarkentry.DataBind();
                fpspread.Sheets[0].RowCount = studentdetails.Tables[0].Rows.Count;
                for (int i = 0; i < studentdetails.Tables[0].Rows.Count; i++)
                {
                    fpspread.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    fpspread.Sheets[0].Cells[i, 1].CellType = txtceltype;
                    fpspread.Sheets[0].Cells[i, 1].Text = Convert.ToString(studentdetails.Tables[0].Rows[i]["roll_no"]).Trim();
                    fpspread.Sheets[0].Cells[i, 2].Text = Convert.ToString(studentdetails.Tables[0].Rows[i]["stud_name"]).Trim();
                }
            }
            arr.Clear();
            int lastcol = 2;
            for (int Att_row = 0; Att_row < scandacc.Rows.Count; Att_row++)
            {
                lastcol++;
                gvmarkentry.Columns[Att_row + 3].HeaderText = Convert.ToString(scandacc.Rows[Att_row][0]).Trim();
            }
            lastcol++;
            for (int Att_row = lastcol; Att_row < gvmarkentry.Columns.Count; Att_row++)
            {
                gvmarkentry.Columns[Att_row].Visible = false;
            }
            // gvmarkentry.DataBind();
            fpspread.SaveChanges();
            for (int i = 3; i < fpspread.Sheets[0].ColumnCount; i++)
            {
                string maxInternal = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Note).Trim();
                double maxInt = 0;
                double minInt = -18;
                double.TryParse(maxInternal, out maxInt);
                intgrcel = new DoubleCellType();
                intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
                if (maxInt != 0)
                {
                    intgrcel.MaximumValue = maxInt;
                    maximumsubjectmark = maxInt;
                }
                //else
                //{
                //    intgrcel.MaximumValue = 100;
                //    maximumsubjectmark = 100;
                //}
                //intgrcel.MaximumValue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[2, 2].Tag);
                intgrcel.MinimumValue = -1;
                intgrcel.ErrorMessage = "Enter valid mark (Lesser Than or Equal To " + maxInt + ")";
                for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                {
                    fpspread.Sheets[0].Cells[j, i].CellType = intgrcel;
                    string rollNo = Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text).Trim();
                    string subjectNoNew = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag).Trim();
                    // string value = Convert.ToString(fpspread.Sheets[0].Cells[j, i].Tag);
                    //string marksql = "select total from camarks where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "' and subject_no ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag) + "'";
                    //marksql = marksql + "  select internal_mark from mark_entry where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "' and subject_no ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag) + "' ";
                    //dsmark.Clear();
                    //dsmark = da.select_method_wo_parameter(marksql, "Text");
                    //if (dsmark.Tables.Count > 0 && dsmark.Tables[0].Rows.Count > 0)
                    //{
                    //    btnfpspread1save.Text = "Update";
                    //    btnfpspread1save.Width = 81;
                    //    string loadedmark = dsmark.Tables[0].Rows[0][0].ToString();
                    //    int dummy;
                    //    if (Int32.TryParse(loadedmark, out dummy))
                    //    {
                    //        if (Convert.ToInt32(loadedmark) < 0)
                    //        {
                    //            loadedmark = loadmarkat(loadedmark);
                    //        }
                    //    }
                    //    fpspread.Sheets[0].Cells[j, i].Text = loadedmark;
                    //    fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                    //}
                    //else if (dsmark.Tables.Count > 1 && dsmark.Tables[1].Rows.Count > 0)
                    //{
                    //    btnfpspread1save.Text = "Update";
                    //    btnfpspread1save.Width = 81;
                    //    string loadedmark = dsmark.Tables[1].Rows[0][0].ToString();
                    //    int dummy;
                    //    if (Int32.TryParse(loadedmark, out dummy))
                    //    {
                    //        if (Convert.ToInt32(loadedmark) < 0)
                    //        {
                    //            loadedmark = loadmarkat(loadedmark);
                    //        }
                    //    }
                    //    fpspread.Sheets[0].Cells[j, i].Text = loadedmark;
                    //    fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                    //}
                    dvCAMMarks = new DataView();
                    dvCOEMarks = new DataView();
                    if (dsCAMMarks.Tables.Count > 0 && dsCAMMarks.Tables[0].Rows.Count > 0)
                    {
                        dsCAMMarks.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectNoNew + "' and Roll_No='" + rollNo + "'";
                        dvCAMMarks = dsCAMMarks.Tables[0].DefaultView;
                    }
                    if (dsCOEMarks.Tables.Count > 0 && dsCOEMarks.Tables[0].Rows.Count > 0)
                    {
                        dsCOEMarks.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectNoNew + "' and Roll_No='" + rollNo + "'";
                        dvCOEMarks = dsCOEMarks.Tables[0].DefaultView;
                    }
                    if (dvCAMMarks.Count > 0)
                    {
                        btnfpspread1save.Text = "Update";
                        btnfpspread1save.Width = 81;
                        string loadedmark = Convert.ToString(dvCAMMarks[0]["total"]).Trim();
                        double dummy = 0;
                        if (double.TryParse(loadedmark, out dummy))
                        {
                            if (dummy < 0)
                            {
                                loadedmark = loadmarkat(loadedmark);
                            }
                        }
                        fpspread.Sheets[0].Cells[j, i].Text = loadedmark;
                        fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else if (dvCOEMarks.Count > 0)
                    {
                        btnfpspread1save.Text = "Update";
                        btnfpspread1save.Width = 81;
                        string loadedmark = Convert.ToString(dvCOEMarks[0]["internal_mark"]).Trim();
                        double dummy;
                        if (double.TryParse(loadedmark, out dummy))
                        {
                            if (dummy < 0)
                            {
                                loadedmark = loadmarkat(loadedmark);
                            }
                        }
                        fpspread.Sheets[0].Cells[j, i].Text = loadedmark;
                        fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            int markcol = 1;
            for (int i = 3; i < fpspread.Sheets[0].ColumnCount; i++)
            {
                string maxInternal = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Note).Trim();
                double maxInt = 0;
                double minInt = -18;
                double.TryParse(maxInternal, out maxInt);
                if (maxInt != 0)
                {
                    intgrcel.MaximumValue = maxInt;
                    maximumsubjectmark = maxInt;
                }
                dssubjectchoo.Clear();
                dssubjectchoo = da.select_method_wo_parameter("select * from subjectchooser where subject_no ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag) + "'", "Text");
                for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                {
                    (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text = fpspread.Sheets[0].Cells[j, i].Text;
                    //(gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked 
                    if (dssubjectchoo.Tables.Count > 0 && dssubjectchoo.Tables[0].Rows.Count > 0)
                    {
                        dssubjectchoo.Tables[0].DefaultView.RowFilter = "roll_no='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text).Trim() + "' and subject_no='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag).Trim() + "'";
                        dvsub = dssubjectchoo.Tables[0].DefaultView;
                    }
                    if (dvsub.Count == 0)
                    {
                        (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Enabled = false;
                        //(gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).BackColor = Color.Red;
                        (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Attributes.Add("Style", "background-color: red;    border: 0 none;    font-size: medium;    font-weight: normal;    text-align: center;    width: 50px;");
                        gvmarkentry.Rows[j].Cells[i].BackColor = Color.Red;
                    }
                    else
                    {
                        (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Enabled = true;
                    }
                }
                markcol++;
            }
            if (fpspread.Sheets[0].ColumnCount > 4)
            {
                fpspread.Sheets[0].FrozenColumnCount = 3;
            }
            if (fpspread.Sheets[0].Rows.Count > 0)
            {
                showexportimport();
            }
            fpspread.SaveChanges();
            fpspread.Visible = true;
            lblErrorMsg.Visible = false;
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            btnfpspread1save.Visible = true;
            btnfpspread1delete.Visible = true;
            if (cnt == 0)
            {
                lblErrorMsg.Text = "Please Select Atleast One Subject";
                lblErrorMsg.Visible = true;
                fpspread.Visible = false;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                hideexportimport();
                show2.Visible = false;
            }
            else
            {
                btnok.Focus();
                show2.Visible = true;
                showexportimport();
            }
            fpspread.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            //FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            //darkstyle.BackColor = ColorTranslator.FromHtml("#ADD8E6");
            ////darkstyle.ForeColor = System.Drawing.Color.Black;
            //darkstyle.Font.Name = "Book Antiqua";
            //darkstyle.Font.Size = FontUnit.Medium;
            //darkstyle.Border.BorderSize = 0;
            //darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
            //FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            string Degree_Code = string.Empty;
            string Batch_Year = string.Empty;
            lblErrorMsg.Text = string.Empty;
            lblErrorMsg.Visible = false;
            // --------------- add start
            if (ddlBatch.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " Year " : " Batch ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpbatch_year = ddlBatch.SelectedItem.Text.ToString();
                Batch_Year = ddlBatch.SelectedItem.Text.ToString();
            }
            if (ddlDegree.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " School Type " : " Degree ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpdegreecode = ddlDegree.SelectedItem.Value.ToString();
            }
            if (ddlBranch.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " Standard " : " Department ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpbranch = ddlBranch.SelectedItem.Value.ToString();
                Degree_Code = ddlBranch.SelectedItem.Value.ToString();
            }
            if (ddlSemYr.Items.Count == 0)
            {
                lblErrorMsg.Text = "No " + ((forschoolsetting) ? " Term " : " Semester ") + "Were Found";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpsem = ddlSemYr.SelectedItem.Text.ToString();
            }
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            //FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            //style2.Font.Size = 13;
            //style2.Font.Name = "Book Antiqua";
            //style2.Font.Bold = true;
            //style2.HorizontalAlign = HorizontalAlign.Center;
            //style2.ForeColor = System.Drawing.Color.Black;
            //style2.BackColor = System.Drawing.Color.Teal;
            //FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = " ";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Code";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
            FpSpread1.Sheets[0].RowCount++;
            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkboxsel_all;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].Columns[1].Width = 50;
            FpSpread1.Sheets[0].Columns[2].Width = 130;
            FpSpread1.Sheets[0].Columns[3].Width = 370;
            FpSpread1.Sheets[0].Columns[4].Width = 170;
            // FpSpread1.Sheets[0].Columns[1].Locked = true;
            // --------------- add end
            string secsql = string.Empty;
            //if (ddlSec.Enabled == true)
            //{
            //    fpsec = ddlSec.SelectedItem.Text.ToString();
            //    if (fpsec.Trim() != "")
            //    {
            //        secsql = "and Registration.Sections in ('" + fpsec + "')";
            //    }
            //    else
            //    {
            //        secsql =string.Empty;
            //    }
            //}
            fpsec = string.Empty;
            int count = 0;
            for (int i = 0; i < cblSec.Items.Count; i++)
            {
                if (cblSec.Items[i].Selected == true)
                {
                    count++;
                    if (fpsec == "")
                    {
                        fpsec = "'" + Convert.ToString(cblSec.Items[i].Value).Trim() + "'";
                    }
                    else
                    {
                        fpsec = fpsec + ",'" + Convert.ToString(cblSec.Items[i].Value).Trim() + "'";
                    }
                }
            }
            if (cblSec.Items.Count > 0)
            {
                if (count == 0)
                {
                    lblErrorMsg.Text = "Please Select Atleast One Section And Then Proceed";
                    lblErrorMsg.Visible = true;
                    return;
                }
            }
            if (!string.IsNullOrEmpty(fpsec.Trim()))
            {
                secsql = "and Registration.Sections in (" + fpsec + ")";
            }
            else
            {
                secsql = string.Empty;
            }
            // string checksem = da.GetFunction("select top 1 Current_Semester from Registration where degree_code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' ");
            //if (checksem.Trim() != fpsem.Trim())
            //{
            //    lblErrorMsg.Text = "No Records Found";
            //    lblErrorMsg.Visible = true;
            //    show1.Visible = false;
            //    show2.Visible = false;
            //    return;
            //}
            string sqlselect = "select distinct subject_code,c.subject_no,subject_name,acronym,s.maxtotal,s.max_int_marks,Batch_Year,s.subjectpriority from syllabus_master y,subject s,subjectChooser c ,sub_sem ss  where ss.syll_code=y.syll_code and s.subType_no=ss.subType_no and ss.promote_count =1 and y.syll_code = s.syll_code and s.subject_no = c.subject_no and Batch_Year = '" + Batch_Year + "' and degree_code = '" + Degree_Code + "' and y.semester in ('" + Convert.ToString(ddlSemYr.SelectedItem.Value) + "' ) order by s.subjectpriority";
            DataSet dsselect = new DataSet();
            dsselect.Clear();
            dsselect = da.select_method_wo_parameter(sqlselect, "Text");
            if (dsselect.Tables.Count > 0 && dsselect.Tables[0].Rows.Count > 0)
            {
                show1.Visible = true;
                string currentsem = ddlSemYr.SelectedItem.Text.ToString();
                string degreecode = ddlBranch.SelectedItem.Value.ToString();
                string batchyear = ddlBatch.SelectedItem.Text.ToString();
                string strtit_acitivity = string.Empty;
                FpSpread1.Visible = true;
                btnok.Visible = true;
                fpspread.Visible = false;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                gvatte.DataSource = dsselect.Tables[0];
                gvatte.DataBind();
                for (int ij = 0; ij < dsselect.Tables[0].Rows.Count; ij++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ij + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = batchyear;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dsselect.Tables[0].Rows[ij]["maxtotal"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dsselect.Tables[0].Rows[ij]["max_int_marks"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsselect.Tables[0].Rows[ij]["subject_name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = dsselect.Tables[0].Rows[ij]["acronym"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dsselect.Tables[0].Rows[ij]["subject_code"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = dsselect.Tables[0].Rows[ij]["subject_no"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                }
                chkboxsel_all.AutoPostBack = true;
                show1.Visible = true;
                show2.Visible = false;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
            }
            else
            {
                lblErrorMsg.Text = "No Records Found";
                lblErrorMsg.Visible = true;
                FpSpread1.Visible = false;
                btnok.Visible = false;
                fpspread.Visible = false;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                show1.Visible = false;
                show2.Visible = false;
            }
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.SaveChanges();
            FpSpread1.Visible = false;
        }
        catch
        {
        }
    }

    protected void btnGofee_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    FpSpread1.Sheets[0].RowCount = 0;
        //    FpSpread1.Sheets[0].ColumnCount = 0;
        //    FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
        //    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
        //    FpSpread1.Sheets[0].ColumnCount = 5;
        //    FpSpread1.CommandBar.Visible = false;
        //    FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        //    FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        //    FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        //    FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
        //    //FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
        //    //style2.Font.Size = 13;
        //    //style2.Font.Name = "Book Antiqua";
        //    //style2.Font.Bold = true;
        //    //style2.HorizontalAlign = HorizontalAlign.Center;
        //    //style2.ForeColor = System.Drawing.Color.Black;
        //    //style2.BackColor = System.Drawing.Color.Teal;
        //    //FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = " ";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch Year";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Name";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Code";
        //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
        //    FpSpread1.Sheets[0].RowCount++;
        //    //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkboxsel_all;
        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].Columns[0].Width = 50;
        //    FpSpread1.Sheets[0].Columns[1].Width = 50;
        //    FpSpread1.Sheets[0].Columns[2].Width = 130;
        //    FpSpread1.Sheets[0].Columns[3].Width = 370;
        //    FpSpread1.Sheets[0].Columns[4].Width = 170;
        //    // FpSpread1.Sheets[0].Columns[1].Locked = true;
        //    // --------------- add end
        //    string secsql =string.Empty;
        //    fpbatch_year = ddlBatch.SelectedItem.Text.ToString();
        //    fpdegreecode = ddlDegree.SelectedItem.Value.ToString();
        //    fpbranch = ddlBranch.SelectedItem.Value.ToString();
        //    fpsem = ddlSemYr.SelectedItem.Text.ToString();
        //    if (ddlSec.Enabled == true)
        //    {
        //        fpsec = ddlSec.SelectedItem.Text.ToString();
        //        if (fpsec.Trim() != "")
        //        {
        //            secsql = "and Registration.Sections in ('" + fpsec + "')";
        //        }
        //        else
        //        {
        //            secsql =string.Empty;
        //        }
        //    }
        //    string Degree_Code =string.Empty;
        //    string Batch_Year =string.Empty;
        //    Degree_Code = ddlBranch.SelectedItem.Value.ToString();
        //    Batch_Year = ddlBatch.SelectedItem.Text.ToString();
        //    string checksem = da.GetFunction("select top 1 Current_Semester from Registration where degree_code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' ");
        //    if (checksem.Trim() != fpsem.Trim())
        //    {
        //        lblErrorMsg.Text = "No Records Found";
        //        lblErrorMsg.Visible = true;
        //        show1.Visible = false;
        //        show2.Visible = false;
        //        return;
        //    }
        //    string sqlselect = "select distinct subject_code,c.subject_no,subject_name,acronym,maxtotal,Batch_Year from syllabus_master y,subject s,subjectChooser c ,sub_sem ss  where ss.syll_code=y.syll_code and s.subType_no=ss.subType_no and ss.promote_count =1 and y.syll_code = s.syll_code and s.subject_no = c.subject_no and Batch_Year = '" + Batch_Year + "' and degree_code = '" + Degree_Code + "' and y.semester in (select top 1 Current_Semester from Registration where degree_code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' )";
        //    DataSet dsselect = new DataSet();
        //    dsselect.Clear();
        //    dsselect = da.select_method_wo_parameter(sqlselect, "Text");
        //    if (dsselect.Tables[0].Rows.Count > 0)
        //    {
        //        show1.Visible = true;
        //        string currentsem = ddlSemYr.SelectedItem.Text.ToString();
        //        string degreecode = ddlBranch.SelectedItem.Value.ToString();
        //        string batchyear = ddlBatch.SelectedItem.Text.ToString();
        //        string strtit_acitivity =string.Empty;
        //        FpSpread1.Visible = true;
        //        btnok.Visible = true;
        //        lblErrorMsg.Visible = false;
        //        fpspread.Visible = false;
        //        btnfpspread1save.Visible = false;
        //        btnfpspread1delete.Visible = false;
        //        gvatte.DataSource = dsselect.Tables[0];
        //        gvatte.DataBind();
        //        for (int ij = 0; ij < dsselect.Tables[0].Rows.Count; ij++)
        //        {
        //            FpSpread1.Sheets[0].RowCount++;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ij + 1);
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkcell;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
        //            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = batchyear;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dsselect.Tables[0].Rows[ij]["maxtotal"].ToString();
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dsselect.Tables[0].Rows[ij]["subject_name"].ToString();
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = dsselect.Tables[0].Rows[ij]["acronym"].ToString();
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dsselect.Tables[0].Rows[ij]["subject_code"].ToString();
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = dsselect.Tables[0].Rows[ij]["subject_no"].ToString();
        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
        //        }
        //        chkboxsel_all.AutoPostBack = true;
        //        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        //        FpSpread1.SaveChanges();
        //    }
        //    else
        //    {
        //        lblErrorMsg.Text = "No Records Found";
        //        lblErrorMsg.Visible = true;
        //        FpSpread1.Visible = false;
        //        btnok.Visible = false;
        //        fpspread.Visible = false;
        //        btnfpspread1save.Visible = false;
        //        btnfpspread1delete.Visible = false;
        //        show1.Visible = false;
        //        show2.Visible = false;
        //    }
        //    FpSpread1.Sheets[0].AutoPostBack = false;
        //    FpSpread1.SaveChanges();
        //    FpSpread1.Visible = false;
        //}
        //catch
        //{
        //}
    }

    protected void btnfpspread1save_Click1(object sender, EventArgs e)
    {
        try
        {
            lblSaveorDelete.Text = "1";
            lblConfirnMsg.Text = "Do You Want To Save Marks?";
            divConfirm.Visible = true;
            divConfirm.Focus();
            lblSaveorDelete.Focus();
            lblSaveorDelete.Page.MaintainScrollPositionOnPostBack = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnfpspread1delete_Click1(object sender, EventArgs e)
    {
        try
        {
            lblSaveorDelete.Text = "2";
            lblConfirnMsg.Text = "Do You Want To Delete Marks?";
            divConfirm.Visible = true;
            divConfirm.Focus();
            lblSaveorDelete.Focus();
            lblSaveorDelete.Page.MaintainScrollPositionOnPostBack = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_importex(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            Boolean rollflag = false;
            Boolean stro = false;
            string errorroll = string.Empty;
            int getstuco = 0;
            fpspread.SaveChanges();
            if (fpmarkexcel.FileName != "" && fpmarkexcel.FileName != null)
            {
                if (fpmarkexcel.FileName.EndsWith(".xls") || fpmarkexcel.FileName.EndsWith(".xlsx"))
                {
                    using (Stream stream = this.fpmarkexcel.FileContent as Stream)
                    {
                        stream.Position = 0;
                        this.fpmarkimport.OpenExcel(stream);
                        fpmarkimport.OpenExcel(stream);
                        fpmarkimport.SaveChanges();
                    }
                    for (int c = 1; c < fpmarkimport.Sheets[0].ColumnCount; c++)
                    {
                        string gettest = fpmarkimport.Sheets[0].Cells[0, c].Text.ToString().Trim().ToLower();
                        for (int g = 3; g < fpspread.Sheets[0].ColumnCount; g++)
                        {
                            string settest = fpspread.Sheets[0].ColumnHeader.Cells[0, g].Text.ToString().Trim().ToLower();
                            if (settest == gettest)
                            {
                                for (int i = 1; i < fpmarkimport.Sheets[0].RowCount; i++)
                                {
                                    string rollno = fpmarkimport.Sheets[0].Cells[i, 1].Text.ToString().Trim().ToLower();
                                    string markval = fpmarkimport.Sheets[0].Cells[i, c].Text.ToString().Trim().ToLower();
                                    rollflag = false;
                                    if (rollno.Trim() != "")
                                    {
                                        for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                                        {
                                            string getrollno = fpspread.Sheets[0].Cells[j, 1].Text.ToString().Trim().ToLower();
                                            if (getrollno == rollno)
                                            {
                                                rollflag = true;
                                                string setmark = markval;
                                                fpspread.Sheets[0].Cells[j, g].Text = setmark;
                                                j = fpspread.Sheets[0].RowCount;
                                            }
                                            else
                                            {
                                            }
                                        }
                                        if (stro == false)
                                        {
                                            if (rollflag == false)
                                            {
                                                if (errorroll == "")
                                                {
                                                    errorroll = rollno;
                                                }
                                                else
                                                {
                                                    errorroll = errorroll + " , " + rollno;
                                                }
                                            }
                                        }
                                    }
                                }
                                stro = true;
                            }
                        }
                    }
                    if (stro == true)
                    {
                        if (errorroll == "")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Imported Successfully')", true);
                        }
                        else
                        {
                            if (getstuco == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Imported Successfully But " + errorroll + " Regno Numbers (s) are  Not Found')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Imported Successfully But " + errorroll + " Roll Numbers (s) are  Not Found')", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Test Not Exists')", true);
                    }
                }
                else
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Please Select The File and Then Proceed";
                }
            }
            else
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Please Select The File and Then Proceed";
            }
            fpmarkimport.Visible = false;
            fpspread.SaveChanges();
            int markcol = 1;
            for (int i = 3; i < fpspread.Sheets[0].ColumnCount; i++)
            {
                for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                {
                    (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text = fpspread.Sheets[0].Cells[j, i].Text;
                    //(gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked 
                }
                markcol++;
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.ToString();
            lblErrorMsg.Visible = true;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            //for (int res = 0; res <= fpspread.Sheets[0].RowCount; res++)
            //{
            //    int colco = 0;
            //    colco = fpspread.Columns.Count;
            //    for (int col = 3; col < colco; col++)
            //    {
            //        string cpy = fpspread.Sheets[0].Cells[res, col].Text.ToString();
            //        fpspread.Sheets[0].Cells[res, colco].Text = cpy;
            //        FarPoint.Web.Spread.TextCellType intgrcel = new FarPoint.Web.Spread.TextCellType();
            //        fpspread.Sheets[0].Cells[res, colco].CellType = intgrcel;
            //        fpspread.SaveChanges();
            //    }
            //}
            //fpspread.SaveChanges();
            //Modified by Srinath 27/2/2013
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (fpspread.Rows.Count > 0)
                {
                    fpspread.Columns[0].Visible = true;
                    fpspread.Columns[0].Font.Size = FontUnit.Medium;
                    fpspread.Columns[0].Font.Name = "Book Antiqua";

                    fpspread.Columns[1].Visible = true;
                    fpspread.Columns[1].Font.Size = FontUnit.Medium;
                    fpspread.Columns[1].Font.Name = "Book Antiqua";

                    fpspread.Columns[2].Visible = true;
                    fpspread.Columns[2].Font.Size = FontUnit.Medium;
                    fpspread.Columns[2].Font.Name = "Book Antiqua";
                    int markcol = 1;
                    for (int i = 3; i < fpspread.Columns.Count; i++)
                    {
                        fpspread.Sheets[0].ColumnHeader.Cells[0, i].Text = gvmarkentry.HeaderRow.Cells[i].Text;
                        fpspread.Columns[i].Visible = true;
                        fpspread.Columns[i].Font.Name = "Book Antiqua";
                        fpspread.Columns[i].Font.Size = FontUnit.Medium;
                        for (int j = 0; j < fpspread.Rows.Count; j++)
                        {
                            string mark = (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text;
                            if (mark.Trim().ToLower().Contains("aaa") || mark.Trim().ToLower().Contains("-1"))
                            {
                                mark = "AAA";
                            }
                            fpspread.Sheets[0].Cells[j, i].Text = mark;//(gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text;
                            fpspread.Sheets[0].Cells[j, i].Font.Name = "Book Antiqua";
                            fpspread.Sheets[0].Cells[j, i].Font.Size = FontUnit.Medium;
                            fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[j, i].VerticalAlign = VerticalAlign.Middle;
                            //(gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked 
                        }
                        markcol++;
                    }
                }
                lblexcelerror.Text = string.Empty;
                lblexcelerror.Visible = false;
                da.printexcelreport(fpspread, reportname);
                txtexcelname.Text = string.Empty;
            }
            else
            {
                lblexcelerror.Text = "Please Enter Your Report Name";
                //lblnorec.Visible = true;
                lblexcelerror.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }

    #region Print PDF

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = "CIA Mark Report";
            string pagename = "Internalnew.aspx";
            int markcol = 1;
            if (fpspread.Rows.Count > 0)
            {
                fpspread.Columns[0].Visible = true;
                fpspread.Columns[0].Font.Size = FontUnit.Medium;
                fpspread.Columns[0].Font.Name = "Book Antiqua";

                fpspread.Columns[1].Visible = true;
                fpspread.Columns[1].Font.Size = FontUnit.Medium;
                fpspread.Columns[1].Font.Name = "Book Antiqua";

                fpspread.Columns[2].Visible = true;
                fpspread.Columns[2].Font.Size = FontUnit.Medium;
                fpspread.Columns[2].Font.Name = "Book Antiqua";

                for (int i = 3; i < fpspread.Columns.Count; i++)
                {
                    fpspread.Sheets[0].ColumnHeader.Cells[0, i].Text = gvmarkentry.HeaderRow.Cells[i].Text;
                    fpspread.Columns[i].Visible = true;
                    fpspread.Columns[i].Font.Size = FontUnit.Medium;
                    fpspread.Columns[i].Font.Name = "Book Antiqua";

                    for (int j = 0; j < fpspread.Rows.Count; j++)
                    {
                        string mark = (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text;
                        if (mark.Trim().ToLower().Contains("aaa") || mark.Trim().ToLower().Contains("-1"))
                        {
                            mark = "AAA";
                        }
                        fpspread.Sheets[0].Cells[j, i].Text = mark;//(gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text;
                        fpspread.Sheets[0].Cells[j, i].Font.Name = "Book Antiqua";
                        fpspread.Sheets[0].Cells[j, i].Font.Size = FontUnit.Medium;
                        fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[j, i].VerticalAlign = VerticalAlign.Middle;
                        //(gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked 
                    }
                    markcol++;
                }
                string Course_Name = Convert.ToString(ddlDegree.SelectedItem).Trim();
                rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlBatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlSemYr.SelectedItem).Trim();
                Printcontrol1.loadspreaddetails(fpspread, pagename, rptheadname);
                Printcontrol1.Visible = true;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

    //protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    e.Row.Cells[0].Width = 42;
    //    e.Row.Cells[1].Width = 42;
    //    e.Row.Cells[2].Width = 70;
    //    e.Row.Cells[3].Width = 250;
    //    e.Row.Cells[4].Width = 150;
    //    gvatte.Width = 800;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        for (int i = 4; i < e.Row.Cells.Count; i++)
    //        {
    //            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvatte, "Type-" + 0 + "$" + e.Row.RowIndex);
    //        }
    //    }
    //}
    //protected void gvatte_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (e.Row.RowIndex == 0)
    //            e.Row.Style.Add("height", "80px");
    //    }
    //}
    //protected void gvmarkentry_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (e.Row.RowIndex == 0)
    //            e.Row.Style.Add("height", "80px");
    //    }
    //}
    //protected void gvmarkentry_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.Cells.Count > 2)
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            //TextBox txt1 = (TextBox)e.Row.FindControl("txtm3");
    //            //Label lbl1batch = (Label)e.Row.FindControl("lblbatch_Year");
    //            //Label lbl1degree = (Label)e.Row.FindControl("lblCourse_id");
    //            //Label lbl1semester = (Label)e.Row.FindControl("lblcurrent_semester");
    //            //Label lbl1section = (Label)e.Row.FindControl("lblsections");
    //            ////CheckBox presentall = (CheckBox)e.Row.FindControl("presentall");
    //            //txt1.Attributes.Add("onkeyup", "javascript:get('" + txt1.ClientID + "','" + lbl1batch.Text + "','" + lbl1degree.Text + "','" + lbl1semester.Text + "','" + lbl1section.Text + "')");
    //            //txt1.Attributes.Add("onblur", "javascript:rollexits('" + txt1.ClientID + "','" + lbl1batch.Text + "','" + lbl1degree.Text + "','" + lbl1semester.Text + "','" + lbl1section.Text + "')");
    //        }
    //    }
    //}

    protected void gvmarkentry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        double mimisubjectmark = -1;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (e.Row.Cells.Count > 0)
            {
                if (scandacc.Rows.Count > 0)
                {
                    for (int i = 0; i < scandacc.Rows.Count; i++)
                    {
                        e.Row.Cells[i + 3].Text = scandacc.Rows[i][0].ToString();
                    }
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells.Count > 0)
            {
                //for (int i = 1; i < 8; i++)
               // {
                    foreach (KeyValuePair<int, string> dic in dicmaxval)
                    {
                        int txtno = dic.Key;
                        string maxmrk = dic.Value;
                         mxmark2 = Convert.ToDouble(maxmrk);
                        if (txtno == 1)
                        {
                            TextBox txt1 = (TextBox)e.Row.FindControl("txtm1");
                            txt1.Attributes.Add("onkeyup", "javascript:get('" + txt1.ClientID + "'," + mxmark2 + "," + mimisubjectmark + ")");
                        }
                        if (txtno == 2)
                        {
                            TextBox txt2 = (TextBox)e.Row.FindControl("txtm2");
                            txt2.Attributes.Add("onkeyup", "javascript:get('" + txt2.ClientID + "'," + mxmark2 + "," + mimisubjectmark + ")");
                        }
                        if (txtno == 3)
                        {
                            TextBox txt3 = (TextBox)e.Row.FindControl("txtm3");
                            txt3.Attributes.Add("onkeyup", "javascript:get('" + txt3.ClientID + "'," + mxmark2 + "," + mimisubjectmark + ")");
                        }
                        if (txtno == 4)
                        {
                            TextBox txt4 = (TextBox)e.Row.FindControl("txtm4");
                            txt4.Attributes.Add("onkeyup", "javascript:get('" + txt4.ClientID + "'," + mxmark2 + "," + mimisubjectmark + ")");
                        }
                        if (txtno == 5)
                        {
                            TextBox txt5 = (TextBox)e.Row.FindControl("txtm5");
                            txt5.Attributes.Add("onkeyup", "javascript:get('" + txt5.ClientID + "'," + mxmark2 + "," + mimisubjectmark + ")");
                        }
                        if (txtno == 6)
                        {
                            TextBox txt6 = (TextBox)e.Row.FindControl("txtm6");
                            txt6.Attributes.Add("onkeyup", "javascript:get('" + txt6.ClientID + "'," + mxmark2 + "," + mimisubjectmark + ")");
                        }
                        if (txtno == 7)
                        {
                            TextBox txt7 = (TextBox)e.Row.FindControl("txtm7");
                            txt7.Attributes.Add("onkeyup", "javascript:get('" + txt7.ClientID + "'," + mxmark2 + "," + mimisubjectmark + ")");
                        }
                    }
                    
               // }
            }
        }
    }

    //protected void chkallsubject_CheckedChanged(object sender,EventArgs e)
    //{
    //    if (chkallsubject)
    //    {
    //    for (int Att_row = 0; Att_row < gvatte.Rows.Count; Att_row++)
    //    {
    //        (gvatte.Rows[Att_row].Cells[1].FindControl("chksubject") as CheckBox).Checked = true;
    //    }
    //    }
    //}

    private bool checkCIALock()
    {
        try
        {
            bool isLockMark = false;
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string groupCode = Convert.ToString(Session["group_code"]).Trim();
                string[] groupUser = Convert.ToString(Session["group_code"]).Trim().Split(';');
                if (groupUser.Length > 0)
                {
                    groupCode = groupUser[0].Trim();
                }
                grouporusercode = " and  group_code='" + Convert.ToString(groupCode).Trim() + "'";
                grouporusercode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            else
            {
                grouporusercode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string CIALock = da.GetFunction("select value from Master_Settings where settings='CAM CIA Mark Entry Lock' " + grouporusercode + "");
            string usercode = string.Empty;
            string userNAme = string.Empty;
            isLockMark = false;
            if (Session["usercode"] != null)
            {
                usercode = Convert.ToString(Session["usercode"]).Trim();
                userNAme = da.GetFunctionv(" select USER_ID from UserMaster where User_code='" + usercode + "'");
                //if (userNAme.Trim().ToLower() == "admin")
                //{
                //    isLockMark = false;
                //    lblLockErr.Text = string.Empty;
                //    lblLockErr.Visible = false;
                //}
                //else
                //{
                //    isLockMark = true;
                //    lblLockErr.Text = "If You Want To Change Mark.Please Contact COE Office!!! Because You Can Save Mark Only Once";
                //    lblLockErr.Visible = true;
                //}
            }
            if (string.IsNullOrEmpty(CIALock) || CIALock.Trim() == "0")
            {
                isLockMark = false;
            }
            else if (!string.IsNullOrEmpty(CIALock) && CIALock.Trim() == "1")
            {
                isLockMark = true;
            }
            else
            {
                isLockMark = true;
            }
            return isLockMark;
        }
        catch
        {
            return false;
        }
    }

    #region Popup Confimation

    protected void btnConfirnYes_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlert.Visible = false;
            divConfirm.Visible = false;
            bool isSaveSucc = false;
            int save = 0;
            if (lblSaveorDelete.Text.Trim() == "1")
            {
                //for (int Att_row = 0; Att_row < gvmarkentry.Rows.Count; Att_row++)
                //{
                //    for (int Att_col = 3; Att_col < gvmarkentry.Columns.Count; Att_col++)
                //    {
                //        if ((gvatte.Rows[Att_row].Cells[1].FindControl("chksubject") as CheckBox).Checked == true)
                //        {
                //            FpSpread1.Sheets[0].Cells[Att_row + 1, 1].Value = 1;
                //        }
                //    }
                //}
                string userCode = string.Empty;
                if (Session["usercode"] != null)
                {
                    userCode = Convert.ToString(Session["usercode"]).Trim();
                }
                int markcol = 1;
                for (int i = 3; i < fpspread.Columns.Count; i++)
                {
                    for (int j = 0; j < fpspread.Rows.Count; j++)
                    {
                        fpspread.Sheets[0].Cells[j, i].Text = (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text;
                        //(gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked 
                    }
                    markcol++;
                }
                fpspread.SaveChanges();
                //return;

                Hashtable ht = new Hashtable();
                ht.Clear();
                string batch_year = ddlBatch.SelectedItem.Text.ToString();
                string degree_code = ddlBranch.SelectedItem.Value.ToString();
                if (fpspread.Sheets[0].RowCount > 0)
                {
                    if (fpspread.Sheets[0].ColumnCount > 0)
                    {
                        for (int im = 3; im < fpspread.Sheets[0].ColumnCount; im++)
                        {
                            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                string roll_no = fpspread.Sheets[0].Cells[i, 1].Text.ToString();
                                string acivityMark = fpspread.Sheets[0].Cells[i, im].Text.ToString();
                                // string acivityMark1 = fpspread.Sheets[0].Cells[i, fpspread.Sheets[0].ColumnCount - 1].Text.ToString();
                                string accodeval = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, im].Tag);
                                //string accodeval1 = fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note;
                                if (acivityMark.Trim() == "" || acivityMark.Trim() == null)
                                {
                                    acivityMark = "null";
                                }
                                if (acivityMark.Trim().ToLower().Contains("aaa") || acivityMark.Trim().ToLower().Contains("a") || acivityMark.Trim().ToLower().Contains("-1"))
                                {
                                    acivityMark = "-1";
                                }
                                //int dummy;
                                //  if (Int32.TryParse(acivityMark, out dummy))
                                // {
                                string strinsert = "if exists (select * from camarks where Roll_No='" + roll_no + "' and subject_no='" + accodeval + "' )  update camarks set total='" + acivityMark + "',UserId='" + userCode + "' where Roll_No='" + roll_no + "' and subject_no='" + accodeval + "'   else insert into camarks (roll_no,subject_no,total,UserId) values ('" + roll_no + "','" + accodeval + "','" + acivityMark + "','" + userCode + "')";
                                if (acivityMark.Trim().ToLower() == "null")
                                {
                                    strinsert = "if exists (select * from camarks where Roll_No='" + roll_no + "' and subject_no='" + accodeval + "' )  update camarks set total=" + acivityMark + ",UserId='" + userCode + "' where Roll_No='" + roll_no + "' and subject_no='" + accodeval + "'   else insert into camarks (roll_no,subject_no,total,UserId) values ('" + roll_no + "','" + accodeval + "'," + acivityMark + ",'" + userCode + "')";
                                }
                                save = da.insert_method(strinsert, ht, "Text");
                                if (save > 0)
                                {
                                    isSaveSucc = true;
                                }
                                //strinsert = "update mark_entry set internal_mark='" + acivityMark + "' where Roll_No='" + roll_no + "' and subject_no='" + accodeval + "'";
                                //if (acivityMark == "null")
                                //{
                                //    strinsert = " update mark_entry set internal_mark=" + acivityMark + " where Roll_No='" + roll_no + "' and subject_no='" + accodeval + "'";
                                //}
                                //da.insert_method(strinsert, ht, "Text");
                                //}
                            }
                        }
                    }
                    btnok_Click1(sender, e);
                    if (isSaveSucc)
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "Saved Successfully";
                        return;
                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "Not Saved";
                        return;
                    }
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                }
                lblexcelerror.Visible = false;
            }
            else if (lblSaveorDelete.Text.Trim() == "2")
            {
                fpspread.SaveChanges();
                Hashtable ht = new Hashtable();
                ht.Clear();
                string batch_year = ddlBatch.SelectedItem.Text.ToString();
                string degree_code = ddlBranch.SelectedItem.Value.ToString();
                //string accodeval1 = fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note;
                if (fpspread.Sheets[0].RowCount > 0)
                {
                    if (fpspread.Sheets[0].ColumnCount > 0)
                    {
                        for (int im = 3; im < fpspread.Sheets[0].ColumnCount; im++)
                        {
                            //if (im == 0)
                            //{ 
                            string accodeval = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, im].Tag);
                            string strinsert = "delete from camarks where subject_no ='" + accodeval + "'";
                            da.insert_method(strinsert, ht, "Text");
                            if (save > 0)
                            {
                                isSaveSucc = true;
                            }
                            //}
                            //else
                            //{
                            //    string strinsert = " delete from CoCurrActivitie_Det where Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and ActivityTextVal ='" + accodeval1 + "'";
                            //    da.insert_method(strinsert, ht, "Text");
                            //}
                        }
                    }
                }
                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                {
                    for (int j = 3; j < fpspread.Sheets[0].ColumnCount; j++)
                    {
                        fpspread.Sheets[0].Cells[i, j].Text = string.Empty;
                    }
                }
                fpspread.SaveChanges();
                btnok_Click1(sender, e);
                if (isSaveSucc)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Deleted Successfully";
                    return;
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Not Deleted";
                    return;
                }
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                btnfpspread1save.Text = "Save";
            }
        }
        catch
        {
        }
    }

    protected void btnConfirnNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirm.Visible = false;
        }
        catch
        {
        }
    }

    #endregion  Popup Confimation

    #region Popup Alert

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch
        {
        }
    }

    #endregion  Popup Alert

}