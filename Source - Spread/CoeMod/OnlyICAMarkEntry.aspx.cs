using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using System.Configuration;

public partial class OnlyICAMarkEntry : System.Web.UI.Page
{
    double maximumsubjectmark = 0;

    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
       
    FpSpread fpspreadsample;

    static Boolean forschoolsetting = false;
    Boolean cellclick = false;

    static ArrayList arr = new ArrayList();

    DAccess2 dacc = new DAccess2();
    DAccess2 da = new DAccess2();

    DataSet ds = new DataSet();
    DataTable scandacc = new DataTable();

    Hashtable hat = new Hashtable();

    string degreecode = string.Empty;
    string batchyear = string.Empty;
    string term = string.Empty;
    string qrySubjectNo = string.Empty;
    string subjectNo = string.Empty;

    string grade_ids = string.Empty;
    string activity_ids = string.Empty;
    string grouporusercode = string.Empty;

    string fpbatch_year = string.Empty;
    string fpdegreecode = string.Empty;
    string fpbranch = string.Empty;
    string fpsem = string.Empty;
    string fpsec = string.Empty;

    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolactivity = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocoldesc = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();

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
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
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

                BindExamYear();
                BindExamMonth();
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
                    bindsem();
                    BindSectionDetail();
                    lblErrorMsg.Text = "";
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
                hideexportimport();
            }
            if (ddlSemYr.Items.Count > 0)
            {
                term = Convert.ToString(ddlSemYr.SelectedItem.Text).Trim();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }

    public void BindExamYear()
    {
        ds.Clear();
        string group_user = string.Empty;
        if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {

            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            grouporusercode = " and group_code='" + group_user + "'";
        }
        else
        {
            grouporusercode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
        }
        Boolean setflag = false;
        ddlExamYear.Items.Clear();
        string getexamvalue = da.GetFunction("select value from master_settings where settings='Exam year and month Valuation' " + grouporusercode + "");
        if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
        {
            string[] spe = getexamvalue.Split(',');
            if (spe.GetUpperBound(0) == 1)
            {
                if (spe[0].Trim() != "0")
                {
                    ddlExamYear.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(spe[0]), Convert.ToString(spe[0])));
                    setflag = true;
                }
            }
        }
        if (setflag == false)
        {
            ds = da.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlExamYear.DataSource = ds;
                ddlExamYear.DataTextField = "Exam_year";
                ddlExamYear.DataValueField = "Exam_year";
                ddlExamYear.DataBind();
            }
        }
        ddlExamYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }

    protected void BindExamMonth()
    {
        try
        {
            ddlExamMonth.Items.Clear();
            string group_user = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                grouporusercode = " and group_code='" + group_user + "'";
            }
            else
            {
                grouporusercode = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
            }
            Boolean setflag = false;
            string monthval = string.Empty;
            string getexamvalue = da.GetFunction("select value from master_settings where settings='Exam year and month Valuation' " + grouporusercode + "");
            if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
            {
                string[] spe = getexamvalue.Split(',');
                if (spe.GetUpperBound(0) == 1)
                {
                    if (spe[1].Trim() != "0")
                    {
                        string val = spe[1].ToString();
                        monthval = " and Exam_month='" + val + "'";
                    }
                }
            }
            ds.Clear();
            string year1 = Convert.ToString(ddlExamYear.SelectedValue).Trim();
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year1 + "'" + monthval + " order by Exam_month desc";
            ds = da.select_method_wo_parameter(strsql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlExamMonth.DataSource = ds;
                ddlExamMonth.DataTextField = "monthName";
                ddlExamMonth.DataValueField = "Exam_month";
                ddlExamMonth.DataBind();
                ddlExamMonth.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {

        }
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

        string Master1 = "";
        if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            string group = Convert.ToString(Session["group_code"]).Trim();
            if (group.Contains(';'))
            {
                string[] group_semi = group.Split(';');
                Master1 = Convert.ToString(group_semi[0]).Trim();
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
            string sections = string.Empty;// "''"; //string.Empty;
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
                string sqlnew = "select distinct isnull(ltrim(rtrim(sections)),'') sections from registration where batch_year=" + Convert.ToString(ddlBatch.SelectedValue).Trim() + " and degree_code=" + Convert.ToString(ddlBranch.SelectedValue).Trim() + " and isnull(ltrim(rtrim(sections)),'')<>'-1' and isnull(ltrim(rtrim(sections)),'')<>' ' and isnull(ltrim(rtrim(sections)),'') in(" + sections + ") and delflag=0 and exam_flag<>'Debar' order by sections";

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
            string Master1 = "";
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
            string usercode = Convert.ToString(Session["usercode"]).Trim();
            string collegecode = Convert.ToString(Session["collegecode"]).Trim();
            string singleuser =  Convert.ToString(Session["single_user"]).Trim();
            string group_user =  Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim(); ;
            }
            string course_id =  Convert.ToString(ddlDegree.SelectedValue).Trim();

            string query = "";
            if (( Convert.ToString(group_user).Trim() != "") && ( Convert.ToString(group_user).Trim() != "0") && ( Convert.ToString(group_user).Trim() != "-1"))
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
        string college_code = Convert.ToString(Session["collegecode"]).Trim();
        string query = "";

        string usercode = Convert.ToString(Session["usercode"]).Trim();

        string singleuser = Convert.ToString(Session["single_user"]).Trim();
        string group_user = Convert.ToString(Session["group_code"]).Trim();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = Convert.ToString(group_semi[0]).Trim();
        }


        if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
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
        if (ds.Tables[0].Rows.Count > 0)
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
            //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

            bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
            int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(Convert.ToString(i));
                    //ddlSemYr.Enabled = false;
                }
                else if (first_year == true && i == 2)
                {
                    ddlSemYr.Items.Add(Convert.ToString(i));
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
                //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(Convert.ToString(i));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(Convert.ToString(i));
                    }
                }
            }
        }
        //if (ddlSemYr.Items.Count > 0)
        //{
        //    ddlSemYr.SelectedIndex = 0;
        //    BindSectionDetail();
        //}
        BindSectionDetail();
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
        string strgetval = "";
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

    protected void ddlExamMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hideexportimport();
            hidealls();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindExamMonth();
            hideexportimport();
            hidealls();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            BindDegree();
            bindbranch();
            bindsem();
            bindsem();
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
            bindsem();
            bindsem();
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
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();


        }

        bindsem();
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
        bindsem();
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

            FpSpread1.SaveChanges();

            //  gvmarkentry.Columns.Clear();
            scandacc.Columns.Add("Ac");
            scandacc.Columns.Add("code");
            scandacc.Rows.Clear();
            int cnt = 0;
            fpmarkexcel.Visible = true;
            btn_import.Visible = true;
            FpSpread1.SaveChanges();

            qrySubjectNo = string.Empty;
            subjectNo = string.Empty;
            for (int Att_row = 0; Att_row < gvatte.Rows.Count; Att_row++)
            {
                if ((gvatte.Rows[Att_row].Cells[1].FindControl("chksubject") as CheckBox).Checked == true)
                {
                    string subjectCode = (gvatte.Rows[Att_row].Cells[3].FindControl("lblsubcode") as Label).Text;
                    string subjectNo1 = (gvatte.Rows[Att_row].Cells[4].FindControl("lblsubno") as Label).Text;
                    //arr.Add(subject_accnmae);
                    scandacc.Rows.Add(subjectCode, subjectNo1);
                    if (string.IsNullOrEmpty(subjectNo))
                    {
                        subjectNo = "'" + subjectNo1 + "'";
                    }
                    else
                    {
                        subjectNo += ",'" + subjectNo1 + "'";
                    }
                }
            }

            if(!string.IsNullOrEmpty(subjectNo))
            {
                qrySubjectNo=" and sc.subject_no in ("+subjectNo+")";
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
            for (int res = 1; res < Convert.ToInt32(FpSpread1.Sheets[0].RowCount); res++)
            {
                int isval = 0;

                isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 1].Value);

                if (isval == 1)
                {
                    show2.Visible = true;
                    cnt++;
                    fpspread.Sheets[0].ColumnCount++;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 3].Tag).Trim();
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 4].Tag).Trim();
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    // fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].ForeColor = Color.Black;

                    fpspread.Sheets[0].Columns[0].Locked = true;
                    fpspread.Sheets[0].Columns[1].Locked = true;
                    fpspread.Sheets[0].Columns[2].Locked = true;
                    fpspread.Visible = false;

                }
            }
            if (cnt == 0)
            {
                lblErrorMsg.Text = "Please Select Atleast One Subject";
                lblErrorMsg.Visible = true;
                fpspread.Visible = false;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                hideexportimport();
                show2.Visible = false;
                return;
            }
            string secsql = "";
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
            //            secsql = "";
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
            //        secsql = "";
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
                secsql = " and isnull(ltrim(rtrim(r.Sections)),'')  in (" + fpsec + ")";

            }
            else
            {
                secsql = "";
            }

            intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            if (FpSpread1.Sheets[0].RowCount > 1)
            {
                intgrcel.MaximumValue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[1, 2].Tag);
                maximumsubjectmark = Convert.ToDouble(FpSpread1.Sheets[0].Cells[1, 2].Tag);
            }
            else
            {
                intgrcel.MaximumValue = 100;
            }
            //intgrcel.MaximumValue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[2, 2].Tag);
            intgrcel.MinimumValue = -18;
            intgrcel.ErrorMessage = "Enter valid mark";

            fpspread.SaveChanges();
            fpspread.Sheets[0].Columns[2].Width = 200;
            string strorderby = da.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = "ORDER BY r.Roll_No";
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY r.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY r.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY r.Stud_Name";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No";
                }
                else if (strorderby == "1,2")
                {
                    strorderby = "ORDER BY r.Reg_No,r.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
                }
            }
            string sqlquery = "  select distinct r.roll_no,r.reg_no, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,isnull(ltrim(rtrim(r.Sections)),'') Sections from registration r, applyn a,subjectChooser sc where sc.roll_no=r.Roll_No and  a.app_no=r.app_no and r.degree_code='" + fpbranch + "'  and r.batch_year='" + fpbatch_year + "' " + secsql + qrySubjectNo + "   " + strorderby + " ;";
            DataSet studentdetails = new DataSet();
            studentdetails.Clear();
            studentdetails = dacc.select_method_wo_parameter(sqlquery, "Text");

            if (studentdetails.Tables[0].Rows.Count > 0)
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
                DataSet dsSubjectDetails = new DataSet();
                dsSubjectDetails = da.select_method_wo_parameter("select max_int_marks,min_int_marks,maxtotal,mintotal,credit_points from subject where subject_no ='" + Convert.ToString(scandacc.Rows[Att_row][1]).Trim() + "'", "Text");

                string maxInternal = "0";
                string minInternal = "0";
                string maxTotal = "0";
                string minTotal = "0";
                string creditPoint = "0";
                if (dsSubjectDetails.Tables.Count > 0 && dsSubjectDetails.Tables[0].Rows.Count > 0)
                {
                    maxInternal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["max_int_marks"]).Trim();
                    minInternal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["min_int_marks"]).Trim();
                    maxTotal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["maxtotal"]).Trim();
                    minTotal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["mintotal"]).Trim();
                    creditPoint = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["credit_points"]).Trim();
                }
                gvmarkentry.Columns[Att_row + 3].HeaderText = Convert.ToString(scandacc.Rows[Att_row][0]).Trim() + "[Max-" + maxInternal + "]";

            }
            lastcol++;
            for (int Att_row = lastcol; Att_row < gvmarkentry.Columns.Count; Att_row++)
            {
                gvmarkentry.Columns[Att_row].Visible = false;
            }
            // gvmarkentry.DataBind();
            fpspread.SaveChanges();
            DataSet dssubjectchoo = new DataSet();
            DataView dvsub = new DataView();
            for (int i = 3; i < fpspread.Sheets[0].ColumnCount; i++)
            {
                for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                {
                    fpspread.Sheets[0].Cells[j, i].CellType = intgrcel;
                    // string value = Convert.ToString(fpspread.Sheets[0].Cells[j, i].Tag);
                    string marksql = "select total from camarks where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "' and subject_no ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag) + "' ";
                    marksql = marksql + "  select max(internal_mark) as internal_mark from mark_entry where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "' and subject_no ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag) + "' ";
                    dsmark.Clear();
                    dsmark = da.select_method_wo_parameter(marksql, "Text");

                    string attempts = string.Empty;
                    attempts = da.GetFunctionv("select COUNT(internal_mark) as attempts from mark_entry where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "' and subject_no ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag) + "' ");

                    if (dsmark.Tables[0].Rows.Count > 0)
                    {
                        btnfpspread1save.Text = "Update";
                        btnfpspread1save.Width = 81;
                        string loadedmark = dsmark.Tables[0].Rows[0][0].ToString();
                        int dummy;
                        if (Int32.TryParse(loadedmark, out dummy))
                        {
                            if (Convert.ToInt32(loadedmark) < 0)
                            {
                                loadedmark = loadmarkat(loadedmark);
                            }
                        }
                        fpspread.Sheets[0].Cells[j, i].Text = loadedmark;
                        fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else if (dsmark.Tables[1].Rows.Count > 0)
                    {
                        btnfpspread1save.Text = "Update";
                        btnfpspread1save.Width = 81;
                        string loadedmark = dsmark.Tables[1].Rows[0][0].ToString();
                        int dummy;
                        if (Int32.TryParse(loadedmark, out dummy))
                        {
                            if (Convert.ToInt32(loadedmark) < 0)
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
                dssubjectchoo.Clear();
                dssubjectchoo = da.select_method_wo_parameter("select * from subjectchooser where subject_no ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag) + "'", "Text");
                DataSet dsSubjectDetails = new DataSet();
                dsSubjectDetails = da.select_method_wo_parameter("select max_int_marks,min_int_marks,maxtotal,mintotal,credit_points from subject where subject_no ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag) + "'", "Text");

                string maxInternal = "0";
                string minInternal = "0";
                string maxTotal = "0";
                string minTotal = "0";
                string creditPoint = "0";
                if (dsSubjectDetails.Tables.Count > 0 && dsSubjectDetails.Tables[0].Rows.Count > 0)
                {
                    maxInternal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["max_int_marks"]).Trim();
                    minInternal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["min_int_marks"]).Trim();
                    maxTotal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["maxtotal"]).Trim();
                    minTotal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["mintotal"]).Trim();
                    creditPoint = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["credit_points"]).Trim();
                }

                for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                {
                    (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text = fpspread.Sheets[0].Cells[j, i].Text;
                    //(gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked 
                    (gvmarkentry.Rows[j].Cells[i].FindControl("txtMaxMark" + markcol + "") as TextBox).Text = maxInternal;
                    (gvmarkentry.Rows[j].Cells[i].FindControl("txtMinMark" + markcol + "") as TextBox).Text = minInternal;

                    dssubjectchoo.Tables[0].DefaultView.RowFilter = "roll_no='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "'";
                    dvsub = dssubjectchoo.Tables[0].DefaultView;
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
            string examYear = string.Empty;
            string examMonth = string.Empty;
            lblErrorMsg.Text = string.Empty;
            lblErrorMsg.Visible = false;

            if (ddlExamYear.Items.Count == 0)
            {
                lblErrorMsg.Text = "No Exam Year Were Found!!! Give Exam Year rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                if (ddlExamYear.SelectedValue == "0")
                {
                    lblErrorMsg.Text = "Please Select Exam Year!!!";
                    lblErrorMsg.Visible = true;
                    return;
                }
                else
                {
                    examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
                }
            }

            if (ddlExamMonth.Items.Count == 0)
            {
                lblErrorMsg.Text = "No Exam Month Were Found!!! Give Exam Month rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                if (ddlExamMonth.SelectedValue == "0")
                {
                    lblErrorMsg.Text = "Please Select Exam Month!!!";
                    lblErrorMsg.Visible = true;
                    return;
                }
                else
                {
                    examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
                }
            }

            // --------------- add start

            if (ddlBatch.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " Year " : " Batch ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpbatch_year = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
                Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            }
            if (ddlDegree.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " School Type " : " Degree ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpdegreecode = Convert.ToString(ddlDegree.SelectedItem.Value).Trim();
            }
            if (ddlBranch.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " Standard " : " Department ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpbranch = Convert.ToString(ddlBranch.SelectedItem.Value).Trim();
                Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value).Trim();
            }
            if (ddlSemYr.Items.Count == 0)
            {
                lblErrorMsg.Text = "No " + ((forschoolsetting) ? " Term " : " Semester ") + "Were Found";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpsem = Convert.ToString(ddlSemYr.SelectedItem.Text).Trim();
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
            //        secsql = "";
            //    }
            //}
            fpsec = string.Empty;
            int count = 0;
            if (cblSec.Items.Count > 0)
            {
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

                if (count == 0)
                {
                    lblErrorMsg.Text = "Please Select Atleast One Section And Then Proceed";
                    lblErrorMsg.Visible = true;
                    return;
                }
            }
            if (!string.IsNullOrEmpty(fpsec.Trim()))
            {
                secsql = "and isnull(ltrim(rtrim(Registration.Sections)),'')  in (" + fpsec + ")";

            }
            else
            {
                secsql = "";
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

            string sqlselect = "select distinct subject_code,c.subject_no,subject_name,acronym,maxtotal,Batch_Year from syllabus_master y,subject s,subjectChooser c ,sub_sem ss  where ss.syll_code=y.syll_code and s.subType_no=ss.subType_no and ss.promote_count =1 and y.syll_code = s.syll_code and s.subject_no = c.subject_no and Batch_Year = '" + Batch_Year + "' and degree_code = '" + Degree_Code + "' and isnull(IsInternalOnly,'0')=1  and y.semester in ('" + Convert.ToString(ddlSemYr.SelectedItem.Value) + "' )";

            DataSet dsselect = new DataSet();
            dsselect.Clear();
            dsselect = da.select_method_wo_parameter(sqlselect, "Text");

            if (dsselect.Tables[0].Rows.Count > 0)
            {
                show1.Visible = true;
                string currentsem = Convert.ToString(ddlSemYr.SelectedItem.Text).Trim();
                string degreecode = Convert.ToString(ddlBranch.SelectedItem.Value).Trim();
                string batchyear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
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
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag =Convert.ToString( dsselect.Tables[0].Rows[ij]["maxtotal"]).Trim();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsselect.Tables[0].Rows[ij]["subject_name"]).Trim();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dsselect.Tables[0].Rows[ij]["acronym"]).Trim();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text =Convert.ToString( dsselect.Tables[0].Rows[ij]["subject_code"]).Trim();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(dsselect.Tables[0].Rows[ij]["subject_no"]).Trim();
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
        //    string secsql = "";
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
        //            secsql = "";
        //        }
        //    }

        //    string Degree_Code = "";
        //    string Batch_Year = "";

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
        //        string strtit_acitivity = "";
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
            string Degree_Code = string.Empty;
            string Batch_Year = string.Empty;
            string examYear = string.Empty;
            string examMonth = string.Empty;
            lblErrorMsg.Text = string.Empty;
            lblErrorMsg.Visible = false;

            if (ddlExamYear.Items.Count == 0)
            {
                lblErrorMsg.Text = "No Exam Year Were Found!!! Give Exam Year rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                if (ddlExamYear.SelectedValue == "0")
                {
                    lblErrorMsg.Text = "Please Select Exam Year!!!";
                    lblErrorMsg.Visible = true;
                    return;
                }
                else
                {
                    examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
                }
            }

            if (ddlExamMonth.Items.Count == 0)
            {
                lblErrorMsg.Text = "No Exam Month Were Found!!! Give Exam Month rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                if (ddlExamMonth.SelectedValue == "0")
                {
                    lblErrorMsg.Text = "Please Select Exam Month!!!";
                    lblErrorMsg.Visible = true;
                    return;
                }
                else
                {
                    examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
                }
            }

            // --------------- add start

            if (ddlBatch.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " Year " : " Batch ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpbatch_year = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
                Batch_Year = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            }
            if (ddlDegree.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " School Type " : " Degree ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpdegreecode = Convert.ToString(ddlDegree.SelectedItem.Value).Trim();
            }
            if (ddlBranch.Items.Count == 0)
            {
                lblErrorMsg.Text = "Give " + ((forschoolsetting) ? " Standard " : " Department ") + "rights to Staff";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpbranch = Convert.ToString(ddlBranch.SelectedItem.Value).Trim();
                Degree_Code = Convert.ToString(ddlBranch.SelectedItem.Value).Trim();
            }
            if (ddlSemYr.Items.Count == 0)
            {
                lblErrorMsg.Text = "No " + ((forschoolsetting) ? " Term " : " Semester ") + "Were Found";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                fpsem = Convert.ToString(ddlSemYr.SelectedItem.Text).Trim();
            }

            string examCode = string.Empty;
            examCode = da.GetFunctionv("select exam_code from Exam_Details where Exam_year='" + examYear + "' and Exam_Month='" + examMonth + "' and batch_year='" + Batch_Year + "' and degree_code='" + Degree_Code + "' and coll_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");

            if (string.IsNullOrEmpty(examCode) || examCode.Trim() == "0")
            {
                lblErrorMsg.Text = "Please Check The Exam Month And Year.";
                lblErrorMsg.Visible = true;
                return;
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

            int markround = 0;
            string getmarkround = da.GetFunctionv("select value from COE_Master_Settings where settings='Mark Round of'");
            if (getmarkround.Trim() != "" && getmarkround.Trim() != "0")
            {
                int num = 0;
                if (int.TryParse(getmarkround, out num))
                {
                    markround = Convert.ToInt32(getmarkround);
                }
            }
            int my = Convert.ToInt32(examMonth) + Convert.ToInt32(examYear) * 12;

            //return;
            Hashtable ht = new Hashtable();
            ht.Clear();
            string batch_year = ddlBatch.SelectedItem.Text.ToString();
            string degree_code = ddlBranch.SelectedItem.Value.ToString();

            bool isSuccess = false;
            if (fpspread.Sheets[0].RowCount > 0)
            {
                if (fpspread.Sheets[0].ColumnCount > 0)
                {
                    for (int im = 3; im < fpspread.Sheets[0].ColumnCount; im++)
                    {
                        for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                        {
                            string roll_no =Convert.ToString( fpspread.Sheets[0].Cells[i, 1].Text).Trim();
                            string acivityMark =Convert.ToString( fpspread.Sheets[0].Cells[i, im].Text).Trim();
                            // string acivityMark1 = fpspread.Sheets[0].Cells[i, fpspread.Sheets[0].ColumnCount - 1].Text.ToString();
                            string attempts = "1";
                            string subjectNo = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, im].Tag).Trim();
                            //string accodeval1 = fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note;
                            //string attempts = string.Empty;
                            attempts = da.GetFunctionv("select COUNT(internal_mark) as attempts from mark_entry where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[i, 1].Text) + "' and subject_no ='" + subjectNo + "' ");

                            if (string.IsNullOrEmpty(attempts.Trim()) || attempts.Trim() == "0")
                            {
                                attempts = "1";
                            }

                            DataSet dsSubjectDetails = new DataSet();
                            dsSubjectDetails = da.select_method_wo_parameter("select max_int_marks,min_int_marks,maxtotal,mintotal,credit_points from subject where subject_no ='" + subjectNo + "'", "Text");

                            double internalMarks = 0;
                            double externalMarks = 0;
                            double totalMarks = 0;
                            double minInternalMark = 0;
                            double minExternalMark = 0;
                            double maxInternalMark = 0;
                            double minTotallMark = 0;
                            double maxTotallMark = 0;
                            double creditPointNew = 0;                            
                            string result = string.Empty;
                            bool passOrFail = false;

                            string maxInternal = "0";
                            string minInternal = "0";
                            string maxTotal = "0";
                            string minTotal = "0";
                            string creditPoint = "0";
                            if (dsSubjectDetails.Tables.Count > 0 && dsSubjectDetails.Tables[0].Rows.Count > 0)
                            {
                                maxInternal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["max_int_marks"]).Trim();
                                minInternal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["min_int_marks"]).Trim();
                                maxTotal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["maxtotal"]).Trim();
                                minTotal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["mintotal"]).Trim();
                                creditPoint = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["credit_points"]).Trim();
                            }

                            double.TryParse(maxInternal, out maxInternalMark);
                            double.TryParse(minInternal, out minInternalMark);
                            double.TryParse(maxTotal, out maxTotallMark);
                            double.TryParse(minTotal, out minTotallMark);
                            double.TryParse(creditPoint, out creditPointNew);

                            double.TryParse(acivityMark, out internalMarks);

                            if (acivityMark.Trim() == "" || acivityMark.Trim() == null)
                            {
                                acivityMark = "null";
                            }
                            else if (acivityMark.Trim().ToLower().Contains('a') || acivityMark.Trim().ToLower().Contains("-1") || internalMarks < 0)
                            {
                                result = "AAA";
                                externalMarks = -1;
                                totalMarks = -1;
                                internalMarks = -1;
                                acivityMark = Convert.ToString(internalMarks).Trim();
                                passOrFail = false;
                            }
                            else
                            {
                                totalMarks = internalMarks;
                                acivityMark = Convert.ToString(internalMarks).Trim();
                                externalMarks = -1;
                                if (internalMarks >= minInternalMark)
                                {
                                    result = "Pass";
                                    passOrFail = true;
                                }
                                else
                                {
                                    result = "Fail";
                                    passOrFail = false;
                                }
                            }


                            string insupdquery = "if not exists(select * from mark_entry where exam_code='" + examCode + "' and roll_no='" + roll_no + "' and subject_no='" + subjectNo + "')";
                            insupdquery = insupdquery + " insert into mark_entry (roll_no,subject_no,exam_code,internal_mark,external_mark,total,result,passorfail,attempts,MYData,rej_stat,cp)";
                            insupdquery = insupdquery + " values('" + roll_no + "','" + subjectNo + "','" + examCode + "'," + acivityMark + "," + externalMarks + "," + totalMarks + ",'" + result + "','" + passOrFail + "','" + attempts + "','" + my + "','0','" + creditPointNew + "')";
                            insupdquery = insupdquery + " else";
                            insupdquery = insupdquery + " update mark_entry set internal_mark=" + acivityMark + ",external_mark=" + externalMarks + ",total=" + totalMarks + ",result='" + result + "',passorfail='" + passOrFail + "',attempts='" + attempts + "'";
                            insupdquery = insupdquery + " where exam_code='" + examCode + "' and roll_no='" + roll_no + "' and subject_no='" + subjectNo + "'";
                            int res = da.update_method_wo_parameter(insupdquery, "Text");

                            if (res > 0)
                            {
                                isSuccess = true;
                            }
                        }
                    }
                }
                if (isSuccess)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
                }
            }
            lblexcelerror.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnfpspread1delete_Click1(object sender, EventArgs e)
    {
        try
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
                    fpspread.Sheets[0].Cells[i, j].Text = "";
                }
            }
            fpspread.SaveChanges();
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            btnfpspread1save.Text = "Save";
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
            string errorroll = "";
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
                            string settest = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, g].Text).Trim().ToLower();
                            if (settest == gettest)
                            {
                                for (int i = 1; i < fpmarkimport.Sheets[0].RowCount; i++)
                                {
                                    string rollno = Convert.ToString(fpmarkimport.Sheets[0].Cells[i, 1].Text).Trim().ToLower();
                                    string markval = Convert.ToString(fpmarkimport.Sheets[0].Cells[i, c].Text).Trim().ToLower();
                                    rollflag = false;
                                    if (rollno.Trim() != "")
                                    {
                                        for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                                        {
                                            string getrollno = Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text).Trim().ToLower();
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
                lblexcelerror.Text = "";
                lblexcelerror.Visible = false;

                da.printexcelreport(fpspread, reportname);
                txtexcelname.Text = "";
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
    
    protected void gvmarkentry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        double mimisubjectmark = -18;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (e.Row.Cells.Count > 0)
            {
                if (scandacc.Rows.Count > 0)
                {
                    for (int i = 0; i < scandacc.Rows.Count; i++)
                    {

                        DataSet dsSubjectDetails = new DataSet();
                        dsSubjectDetails = da.select_method_wo_parameter("select max_int_marks,min_int_marks,maxtotal,mintotal,credit_points from subject where subject_no ='" + Convert.ToString(scandacc.Rows[i][1]).Trim() + "'", "Text");
                        string maxInternal = "0";
                        string minInternal = "0";
                        string maxTotal = "0";
                        string minTotal = "0";
                        string creditPoint = "0";
                        if (dsSubjectDetails.Tables.Count > 0 && dsSubjectDetails.Tables[0].Rows.Count > 0)
                        {
                            maxInternal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["max_int_marks"]).Trim();
                            minInternal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["min_int_marks"]).Trim();
                            maxTotal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["maxtotal"]).Trim();
                            minTotal = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["mintotal"]).Trim();
                            creditPoint = Convert.ToString(dsSubjectDetails.Tables[0].Rows[0]["credit_points"]).Trim();
                        }
                        e.Row.Cells[i + 3].Text = Convert.ToString(scandacc.Rows[i][0]).Trim() + "[Max-" + maxInternal + "]";
                        
                    }
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells.Count > 0)
            {
                for (int i = 1; i < 8; i++)
                {
                    TextBox txt1 = (TextBox)e.Row.FindControl("txtm" + i + "");
                    txt1.Attributes.Add("onkeyup", "javascript:get('" + txt1.ClientID + "'," + maximumsubjectmark + "," + mimisubjectmark + ")");
                }
            }
        }
    }

}