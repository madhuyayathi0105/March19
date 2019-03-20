using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using FarPoint.Web.Spread;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;


public partial class remarksentry : System.Web.UI.Page
{
    DataTable scandacc = new DataTable();
    double maximumsubjectmark = 0;
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
    string term =string.Empty;
    string grade_ids =string.Empty;
    string activity_ids =string.Empty;
    FpSpread fpspreadsample;
    DataSet ds = new DataSet();
    static Boolean forschoolsetting = false;
    DAccess2 dacc = new DAccess2();
    Hashtable hat = new Hashtable();
    Boolean cellclick = false;
    static ArrayList arr = new ArrayList();
    string grouporusercode =string.Empty;
    string fpbatch_year =string.Empty;
    string fpdegreecode =string.Empty;
    string fpbranch =string.Empty;
    string fpsem =string.Empty;
    string fpsec =string.Empty;

    FarPoint.Web.Spread.ComboBoxCellType combocol = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolactivity = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocoldesc = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
    DAccess2 da = new DAccess2();

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
        if (!IsPostBack)
        {
            lblTest.Visible = false;
            ddlTest.Visible = false;
            show1.Visible = false;
            show2.Visible = false;
            btnPrintrmk.Visible = false;
            lblBatch.Text = "Batch";
            lblDegree.Text = "Degree";
            lblBranch.Text = "Branch";
            lblSemYr.Text = "Sem";
            FpSpread1.Sheets[0].AutoPostBack = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = dacc.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
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
            if (ddlDegree.Items.Count > 0)
            {
                bindbranch();
                bindsem();
                BindSectionDetail();
                lblErrorMsg.Text =string.Empty;
            }
            else
            {
                lblErrorMsg.Text = "Give degree rights to staff";
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
                fpspread.Sheets[0].ColumnHeader.Cells[0, i].ForeColor = System.Drawing.Color.White;
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
            fpspread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.White;
            bindactivity();
            hideexportimport();
        }
        term = ddlSemYr.SelectedItem.Text.ToString().Trim();
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();

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
            BindSectionDetail();
            lblErrorMsg.Visible = false;
            fpspread.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
        }
        GetTest();
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
        bindsem();
        BindSectionDetail();
        lblErrorMsg.Visible = false;
        fpspread.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
        GetTest();
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
        bindactivity();
        hideexportimport();
        hidealls();
        GetTest();
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
            GetTest();
            DataSet dsremark = new DataSet();
            int selectsubcount = 0;
            if (ddlTest.Items.Count != 0)
            {
                for (int Att_row = 0; Att_row < gvatte.Rows.Count; Att_row++)
                {
                    if ((gvatte.Rows[Att_row].Cells[1].FindControl("chksubject") as CheckBox).Checked == true)
                    {
                        selectsubcount++;
                        FpSpread1.Sheets[0].Cells[Att_row + 1, 1].Value = 1;
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
                FpSpread1.SaveChanges();

                for (int Att_row = 0; Att_row < gvatte.Rows.Count; Att_row++)
                {
                    if ((gvatte.Rows[Att_row].Cells[1].FindControl("chksubject") as CheckBox).Checked == true)
                    {
                        string subject_accnmae = (gvatte.Rows[Att_row].Cells[3].FindControl("lblsub_ac") as Label).Text;
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

                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = System.Drawing.Color.Black;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = System.Drawing.Color.Black;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = System.Drawing.Color.Black;
                DataSet dsmark = new DataSet();

                for (int res = 1; res < Convert.ToInt32(FpSpread1.Sheets[0].RowCount); res++)
                {
                    int isval = 0;
                    isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 1].Value);
                    if (isval == 1)
                    {
                        show2.Visible = true;
                        cnt++;
                        fpspread.Sheets[0].ColumnCount++;
                        fpspread.Sheets[0].ColumnHeader.Columns[fpspread.Sheets[0].ColumnCount - 1].Width = 150;
                        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 3].Tag);
                        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Tag = FpSpread1.Sheets[0].Cells[res, 4].Tag;
                        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        // fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.Black;
                        fpspread.Sheets[0].Columns[0].Locked = true;
                        fpspread.Sheets[0].Columns[1].Locked = true;
                        fpspread.Sheets[0].Columns[2].Locked = true;
                        fpspread.Visible = false;
                    }
                    //if ((gvatte.Rows[res].Cells[1].FindControl("chksubject") as CheckBox).Checked == true)
                    //{
                    //    show2.Visible = true;
                    //    cnt++;
                    //    fpspread.Sheets[0].ColumnCount++;
                    //    fpspread.Sheets[0].ColumnHeader.Columns[fpspread.Sheets[0].ColumnCount - 1].Width = 150;
                    //    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 3].Tag);
                    //    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Tag = FpSpread1.Sheets[0].Cells[res, 4].Tag;
                    //    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //    // fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].ForeColor = Color.Black;
                    //    fpspread.Sheets[0].Columns[0].Locked = true;
                    //    fpspread.Sheets[0].Columns[1].Locked = true;
                    //    fpspread.Sheets[0].Columns[2].Locked = true;
                    //    fpspread.Visible = false;
                    //}
                }
                string secsql =string.Empty;
                fpbatch_year = ddlBatch.SelectedItem.Text.ToString();
                fpdegreecode = ddlDegree.SelectedItem.Value.ToString();
                fpbranch = ddlBranch.SelectedItem.Value.ToString();
                fpsem = ddlSemYr.SelectedItem.Text.ToString();

                if (ddlSec.Enabled == true)
                {
                    fpsec = ddlSec.SelectedItem.Text.ToString();
                    if (fpsec == "All")
                    {
                        // ------------- add start
                        secsql =string.Empty;
                    }
                    else
                    {
                        secsql = "and Registration.Sections in ('" + fpsec + "')";

                    }
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
                    strorderby =string.Empty;
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
                string sqlquery = "select Registration.roll_no,Registration.reg_no, Registration.stud_name,Registration.stud_type,registration.serialno,Registration.Adm_Date,Sections from registration, applyn a where a.app_no=registration.app_no and registration.degree_code='" + fpbranch + "'  and registration.batch_year='" + fpbatch_year + "' " + secsql + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strorderby + "  ";
                DataSet studentdetails = new DataSet();
                studentdetails.Clear();
                studentdetails = dacc.select_method_wo_parameter(sqlquery, "Text");

                if (studentdetails.Tables[0].Rows.Count > 0)
                {
                    fpspread.Sheets[0].RowCount = studentdetails.Tables[0].Rows.Count;
                    for (int i = 0; i < studentdetails.Tables[0].Rows.Count; i++)
                    {
                        fpspread.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        fpspread.Sheets[0].Cells[i, 1].CellType = txtceltype;
                        fpspread.Sheets[0].Cells[i, 1].Text = studentdetails.Tables[0].Rows[i]["roll_no"].ToString();
                        fpspread.Sheets[0].Cells[i, 2].Text = studentdetails.Tables[0].Rows[i]["stud_name"].ToString();
                    }
                }
                arr.Clear();

                int lastcol = 2;
                for (int Att_row = 0; Att_row < scandacc.Rows.Count; Att_row++)
                {
                    lastcol++;

                }
                lastcol++;

                fpspread.SaveChanges();
                string query =string.Empty;

                fpspread.SaveChanges();
                for (int i = 3; i < fpspread.Sheets[0].ColumnCount; i++)
                {
                    for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                    {
                        string examcode =string.Empty;
                        fpspread.Sheets[0].Cells[j, i].CellType = combocol;
                        string roll_no = fpspread.Sheets[0].Cells[j, 1].Text.ToString();
                        string acivityRemark =string.Empty;
                        string subcodeval = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                        string sec = da.GetFunctionv("select Sections from Registration where Roll_No='" + roll_no + "'");
                        if (sec != "" && sec != null)
                        {
                            examcode = da.GetFunctionv("select exam_code from Exam_type where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and subject_no='" + subcodeval + "' and criteria_no='" + ddlTest.SelectedValue.ToString() + "' and sections='" + sec + "'");
                        }
                        else
                        {
                            examcode = da.GetFunctionv("select exam_code from Exam_type where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and subject_no='" + subcodeval + "' and criteria_no='" + ddlTest.SelectedValue.ToString() + "'");
                        }
                        // string acivityMark1 = fpspread.Sheets[0].Cells[i, fpspread.Sheets[0].ColumnCount - 1].Text.ToString();

                        //string accodeval1 = fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note;
                        if (examcode != "" && examcode != null)
                        {
                            string qryrmrk = "select * from result where  exam_code='" + examcode + "'  and roll_no='" + roll_no + "'";
                            dsremark = da.select_method_wo_parameter(qryrmrk, "Text");
                            if (dsremark.Tables[0].Rows.Count > 0)
                            {
                                acivityRemark = dsremark.Tables[0].Rows[0]["remarks"].ToString();
                                if (acivityRemark != "")
                                {
                                    query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'Rmrk' and college_code = '" + Session["collegecode"].ToString() + "' and TextCode='" + acivityRemark + "'";
                                    dsmark.Clear();
                                    dsmark = da.select_method_wo_parameter(query, "Text");
                                    if (dsmark.Tables[0].Rows.Count > 0)
                                    {
                                        fpspread.Sheets[0].Cells[j, i].Text = dsmark.Tables[0].Rows[0]["TextVal"].ToString();
                                        fpspread.Sheets[0].Cells[j, i].Value = Convert.ToInt16(dsmark.Tables[0].Rows[0]["TextCode"].ToString());
                                    }
                                    fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    fpspread.Sheets[0].Cells[j, i].Text =string.Empty;
                                }

                            }
                        }
                    }
                }
                query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'Rmrk' and college_code = '" + Session["collegecode"].ToString() + "'";
                dsmark.Clear();
                dsmark = da.select_method_wo_parameter(query, "Text");
                if (dsmark.Tables[0].Rows.Count > 0)
                {
                    btnfpspread1save.Text = "Update";
                    btnfpspread1save.Width = 81;
                    combocol.DataSource = dsmark;
                    combocol.DataTextField = "TextVal";
                    combocol.DataValueField = "TextCode";
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
                lblErrorMsg.Visible = false;
                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                btnfpspread1save.Visible = true;
                btnfpspread1delete.Visible = true;
                if (selectsubcount == 0)
                {
                    lblErrorMsg.Text = "Please Select Atleast One Subject";
                    lblErrorMsg.Visible = true;
                    lblTest.Visible = false;
                    ddlTest.Visible = false;
                    fpspread.Visible = false;
                    btnfpspread1save.Visible = false;
                    btnfpspread1delete.Visible = false;
                    btnPrintrmk.Visible = false;
                    hideexportimport();
                    show2.Visible = false;
                }
                else
                {
                    btnok.Focus();
                    GetTest();
                    fpspread.Visible = true;
                    //lblTest.Visible = true;
                    //ddlTest.Visible = true;
                    show2.Visible = true;
                    showexportimport();
                }
            }
            else
            {
                lblErrorMsg.Text = "Please Create a Test First.";
                lblErrorMsg.Visible = true;
                lblTest.Visible = false;
                ddlTest.Visible = false;
                fpspread.Visible = false;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                hideexportimport();
                btnPrintrmk.Visible = false;
                show2.Visible = false;
            }
            fpmarkexcel.Visible = false;
            btn_import.Visible = false;
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


            // --------------- add start
            string Batch_Year = string.Empty;
            string Degree_Code = string.Empty;
            string semNew = string.Empty;
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
                semNew = ddlSemYr.SelectedItem.Text.ToString();
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
            string secsql =string.Empty;
            fpbatch_year = ddlBatch.SelectedItem.Text.ToString();
            fpdegreecode = ddlDegree.SelectedItem.Value.ToString();
            fpbranch = ddlBranch.SelectedItem.Value.ToString();
            fpsem = ddlSemYr.SelectedItem.Text.ToString();

            if (ddlSec.Enabled == true)
            {
                fpsec = ddlSec.SelectedItem.Text.ToString();

                if (fpsec.Trim() != "")
                {
                    secsql = "and Registration.Sections in ('" + fpsec + "')";

                }
                else
                {
                    secsql =string.Empty;
                }
            }

            Degree_Code =string.Empty;
            Batch_Year =string.Empty;

            Degree_Code = ddlBranch.SelectedItem.Value.ToString();
            Batch_Year = ddlBatch.SelectedItem.Text.ToString();

            //string checksem = da.GetFunction("select top 1 Current_Semester from Registration where degree_code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' ");

            //if (checksem.Trim() != fpsem.Trim())
            //{
            //    lblErrorMsg.Text = "No Records Found";
            //    lblErrorMsg.Visible = true;
            //    show1.Visible = false;
            //    show2.Visible = false;
            //    return;

            //}

            //string sqlselect = "select distinct subject_code,c.subject_no,subject_name,acronym,maxtotal,Batch_Year from syllabus_master y,subject s,subjectChooser c ,sub_sem ss  where ss.syll_code=y.syll_code and s.subType_no=ss.subType_no and ss.promote_count =1 and y.syll_code = s.syll_code and s.subject_no = c.subject_no and Batch_Year = '" + Batch_Year + "' and degree_code = '" + Degree_Code + "' and y.semester in (select top 1 Current_Semester from Registration where degree_code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' )";

            string sqlselect = "select distinct subject_code,c.subject_no,subject_name,acronym,maxtotal,Batch_Year from syllabus_master y,subject s,subjectChooser c ,sub_sem ss  where ss.syll_code=y.syll_code and s.subType_no=ss.subType_no and ss.promote_count =1 and y.syll_code = s.syll_code and s.subject_no = c.subject_no and Batch_Year = '" + Batch_Year + "' and degree_code = '" + Degree_Code + "' and y.semester in ('" + fpsem.ToString() + "')";

            DataSet dsselect = new DataSet();
            dsselect.Clear();
            dsselect = da.select_method_wo_parameter(sqlselect, "Text");

            if (dsselect.Tables[0].Rows.Count > 0)
            {
                show1.Visible = true;

                string currentsem = ddlSemYr.SelectedItem.Text.ToString();
                string degreecode = ddlBranch.SelectedItem.Value.ToString();
                string batchyear = ddlBatch.SelectedItem.Text.ToString();
                string strtit_acitivity =string.Empty;
                FpSpread1.Visible = true;
                btnok.Visible = true;
                lblErrorMsg.Visible = false;
                fpspread.Visible = false;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                btnPrintrmk.Visible = false;
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
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dsselect.Tables[0].Rows[ij]["maxtotal"].ToString();
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
                btnPrintrmk.Visible = false;
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

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorMsg.Visible = false;
        fpspread.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
        GetTest();
        FpSpread1.Visible = false;
        btnok.Visible = false;
        hideexportimport();
        hidealls();
    }

    public void bindsem()
    {
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        DataSet ds = new DataSet();
        string sqlnew = string.Empty;
        if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0)
        {
            sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "";
            ds.Clear();
            ds = dacc.select_method_wo_parameter(sqlnew, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());
                    //ddlSemYr.Enabled = false;
                }
                else if (first_year == true && i == 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }

            }
        }
        else
        {
            if (ddlBranch.Items.Count > 0)
            {
                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "";
                ds.Clear();
                ds = dacc.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

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

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSectionDetail();
        lblErrorMsg.Visible = false;
        fpspread.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
        GetTest();
        hidealls();
    }

    public void BindSectionDetail()
    {
        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlBatch.SelectedValue.ToString();

        ddlSec.Items.Clear();
        DataSet ds = new DataSet();
        if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0)
        {
            string sqlnew = "select distinct sections from registration where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            ds.Clear();
            ds = dacc.select_method_wo_parameter(sqlnew, "Text");

            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
        }
        //ddlSec.Items.Insert(0, "All");
        //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.Enabled = true;
        }
        else
        {
            ddlSec.Enabled = false;
        }

    }

    public void BindBatch()
    {
        try
        {
            string Master1 =string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
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

            string query =string.Empty;
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + course_id + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + "";
            }
            else
            {
                query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + course_id + " and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "";
            }
            ds = dacc.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                }
            }
        }
        catch
        {
        }
    }

    public void BindDegree()
    {
        string college_code = Session["collegecode"].ToString();
        string query =string.Empty;

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
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + "";
        }
        else
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "";
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

    protected void ddlactivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorMsg.Visible = false;
        fpspread.Visible = false;
        btnfpspread1save.Visible = false;
        btnfpspread1delete.Visible = false;
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
            //int markcol = 1;
            //for (int i = 3; i < fpspread.Columns.Count; i++)
            //{
            //    for (int j = 0; j < fpspread.Rows.Count; j++)
            //    {

            //         fpspread.Sheets[0].Cells[j, i].Text = (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text;
            //        //(gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked 


            //    }
            //    markcol++;
            //}
            fpspread.SaveChanges();
            //return;
            Hashtable ht = new Hashtable();
            ht.Clear();
            string batch_year = ddlBatch.SelectedItem.Text.ToString();
            string degree_code = ddlBranch.SelectedItem.Value.ToString();
            DataSet dsremark = new DataSet();
            StringBuilder sbSuccessFail = new StringBuilder();
            if (fpspread.Sheets[0].RowCount > 0)
            {
                int re = 0;
                if (fpspread.Sheets[0].ColumnCount > 0)
                {
                    for (int im = 3; im < fpspread.Sheets[0].ColumnCount; im++)
                    {
                        for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                        {
                            string examcode =string.Empty;
                            string roll_no = fpspread.Sheets[0].Cells[i, 1].Text.ToString();
                            string acivityRemark =string.Empty;
                            if (fpspread.Sheets[0].Cells[i, im].Text.ToString() != "" && fpspread.Sheets[0].Cells[i, im].Text.ToString() != null)
                                acivityRemark = fpspread.Sheets[0].Cells[i, im].Value.ToString();
                            string subcodeval = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, im].Tag);
                            string sec = da.GetFunctionv("select Sections from Registration where Roll_No='" + roll_no + "'");
                            if (sec != "" && sec != null)
                                examcode = da.GetFunctionv("select exam_code from Exam_type where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and subject_no='" + subcodeval + "' and criteria_no='" + ddlTest.SelectedValue.ToString() + "' and sections='" + sec + "'");
                            else
                                examcode = da.GetFunctionv("select exam_code from Exam_type where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and subject_no='" + subcodeval + "' and criteria_no='" + ddlTest.SelectedValue.ToString() + "'");
                            // string acivityMark1 = fpspread.Sheets[0].Cells[i, fpspread.Sheets[0].ColumnCount - 1].Text.ToString();

                            //string accodeval1 = fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note;

                            if (acivityRemark.Trim() == "" || acivityRemark.Trim() == null)
                            {
                                acivityRemark =string.Empty;
                            }
                            if (examcode != "" && examcode != null)
                            {
                                string qryrmrk = "select * from result where  exam_code='" + examcode + "'  and roll_no='" + roll_no + "'";
                                dsremark = da.select_method_wo_parameter(qryrmrk, "Text");
                                if (dsremark.Tables[0].Rows.Count > 0)
                                {
                                    string strinsert = "update result set remarks='" + acivityRemark + "' where exam_code='" + examcode + "'  and roll_no='" + roll_no + "'";
                                    re = da.insert_method(strinsert, ht, "Text");
                                }
                                else
                                {
                                    lblErrorMsg.Text = "Please Enter The Marks in Mark Entry!!!";
                                    lblErrorMsg.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                lblErrorMsg.Text = "Please Create the Exam For Section - " + sec + " !!!";
                                lblErrorMsg.Visible = true;
                                return;
                            }
                        }

                    }
                }
                if (re == 1)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Create the Exam or Enter the Mark')", true);
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
        surediv.Visible = true;

    }

    protected void btn_importex(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;

            Boolean rollflag = false;
            Boolean stro = false;
            string errorroll =string.Empty;
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
            //for (int i = 3; i < fpspread.Sheets[0].ColumnCount; i++)
            //{
            //    for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
            //    {
            //        (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text = fpspread.Sheets[0].Cells[j, i].Text;
            //        //(gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked 


            //    }
            //    markcol++;
            //}
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
                //int markcol = 1;
                //for (int i = 3; i < fpspread.Columns.Count; i++)
                //{
                //    for (int j = 0; j < fpspread.Rows.Count; j++)
                //    {

                //        fpspread.Sheets[0].Cells[j, i].Text = (gvmarkentry.Rows[j].Cells[i].FindControl("txtm" + markcol + "") as TextBox).Text;
                //        //(gvatte.Rows[gvatte.Rows.Count - 1].Cells[gvcol].FindControl(timageids) as CheckBox).Checked 


                //    }
                //    markcol++;
                //}
                lblexcelerror.Text =string.Empty;
                lblexcelerror.Visible = false;

                da.printexcelreport(fpspread, reportname);
                txtexcelname.Text =string.Empty;
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

    public void hideexportimport()
    {
        btnPrintrmk.Visible = false;
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
        btnPrintrmk.Visible = true;

    }

    public string loadmarkat(string mr)
    {
        string strgetval =string.Empty;
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
        double mimisubjectmark = -18;
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
                for (int i = 1; i < 8; i++)
                {
                    TextBox txt1 = (TextBox)e.Row.FindControl("txtm" + i + "");
                    txt1.Attributes.Add("onkeyup", "javascript:get('" + txt1.ClientID + "'," + maximumsubjectmark + "," + mimisubjectmark + ")");

                }
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

    protected void btnPrintrmk_Click(object sender, EventArgs e)
    {

        try
        {

            //SpreadBind();
            btnGo_Click(sender, e);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Passpercentageanalysis.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Label lb = new Label();

            string collegename =string.Empty;

            StringWriter sw1 = new StringWriter();
            HtmlTextWriter hw1 = new HtmlTextWriter(sw1);
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();
            DataRow dr;
            PdfPTable pdftbl;
            PdfPCell cell;
            float[] width;
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 5f, 0f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            for (int c = 0; c < fpspread.Sheets[0].ColumnCount; c++)
            {
                dc = new DataColumn();
                dc.ColumnName = fpspread.Sheets[0].ColumnHeader.Cells[0, c].Text.ToString();
                dt.Columns.Add(dc);
            }
            for (int r = 0; r < fpspread.Sheets[0].RowCount; r++)
            {
                dr = dt.NewRow();
                for (int c = 0; c < fpspread.Sheets[0].ColumnCount; c++)
                {
                    dr[c] = fpspread.Sheets[0].Cells[r, c].Text.ToString();
                }
                dt.Rows.Add(dr);
            }


            if (dt.Columns.Count > 0 && dt.Rows.Count > 0)
            {
                pdftbl = new PdfPTable(dt.Columns.Count);

                cell = new PdfPCell();
                width = new float[dt.Columns.Count];
                pdftbl.TotalWidth = 580f;
                //pdftbl.HorizontalAlignment = 0;

                //gv.DataSource = dt;
                //gv.DataBind();
                //if (gv.Rows.Count > 0)
                //{
                //    gv.AllowPaging = false;
                //    gv.HeaderRow.Style.Add("width", "15%");
                //    gv.HeaderRow.Style.Add("font-size", "8px");
                //    gv.HeaderRow.Style.Add("text-align", "center");
                //    gv.Style.Add("font-family", "Book Antiqua;");
                //    gv.Style.Add("font-size", "6px");
                //    gv.RenderControl(hw);
                //    gv.DataBind();
                //}

                //StringReader sr = new StringReader(sw.ToString());
                //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //htmlparser.Parse(sr);
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    cell = new PdfPCell(new Phrase(dt.Columns[c].ToString()));
                    cell.HorizontalAlignment = 1;
                    cell.VerticalAlignment = 1;
                    width[c] = 200f;
                    pdftbl.AddCell(cell);
                }
                pdftbl.SetWidths(width);
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        cell = new PdfPCell(new Phrase(dt.Rows[r][c].ToString()));
                        cell.HorizontalAlignment = 1;
                        cell.VerticalAlignment = 1;
                        pdftbl.AddCell(cell);
                    }
                }
                pdfDoc.Add(pdftbl);
            }
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();

            //if (fpspread.Sheets[0].RowCount > 0)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-disposition", "attachment;filename=Passpercentageanalysis.pdf");
            //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    StringWriter sw = new StringWriter();
            //    HtmlTextWriter hw = new HtmlTextWriter(sw);
            //    btnok_Click1(sender, e);                
            //    //GridView gv = new GridView();
            //    DataTable dt = new DataTable();
            //    DataColumn dc = new DataColumn();
            //    DataRow dr;
            //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 5f, 0f);
            //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //    pdfDoc.Open();
            //    for (int c = 0; c < fpspread.Sheets[0].ColumnCount; c++)
            //    {
            //        dc = new DataColumn();
            //        dc.ColumnName = fpspread.Sheets[0].ColumnHeader.Cells[0, c].Text.ToString();
            //        dt.Columns.Add(dc);
            //    }
            //    for (int r = 0; r < fpspread.Sheets[0].RowCount; r++)
            //    {
            //        dr = dt.NewRow();
            //        for (int c = 0; c < fpspread.Sheets[0].ColumnCount; c++)
            //        {
            //            dr[c] = fpspread.Sheets[0].Cells[r, c].Text.ToString();
            //        }
            //        dt.Rows.Add(dr);
            //    }

            //    if (dt.Rows.Count > 0)
            //    {
            //        PdfPTable pdftbl = new PdfPTable(dt.Columns.Count);
            //        PdfPCell cell = new PdfPCell();
            //        //gv.DataSource = dt;
            //        //float[] width = new float[dt.Columns.Count];
            //        //gvatte.AllowPaging = false;
            //        //gvatte.HeaderRow.Style.Add("width", "15%");
            //        //gvatte.HeaderRow.Style.Add("font-size", "8px");
            //        //gvatte.HeaderRow.Style.Add("text-align", "center");
            //        //gvatte.Style.Add("font-family", "Book Antiqua;");
            //        //gvatte.Style.Add("font-size", "6px");
            //        ////gvatte.DataBind();
            //        //gvatte.Enabled = true;

            //        //gvatte.RenderControl(hw);
            //        //gvatte.DataBind();

            //        for (int r = 0; r < dt.Rows.Count; r++)
            //        {
            //            for (int c = 0; c < dt.Columns.Count; c++)
            //            {
            //                cell = new PdfPCell(new Phrase(dt.Rows[r][c].ToString()));
            //                //if (c != 0)
            //                //{
            //                //    cell.HorizontalAlignment = 1;
            //                //    cell.VerticalAlignment = 1;
            //                //}
            //                //else
            //                //{
            //                //    cell.HorizontalAlignment = 0;
            //                //    cell.VerticalAlignment = 1;
            //                //}
            //                pdftbl.AddCell(cell);
            //            }
            //        }
            //        pdfDoc.Add(pdftbl);
            //    }
            //    StringReader sr = new StringReader(sw.ToString());
            //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //    htmlparser.Parse(sr);
            //    Response.Write(pdfDoc);
            //    Response.End();
            //    Response.Clear();
            //}
        }
        catch
        {
        }


    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    public void GetTest()
    {
        try
        {
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = dacc.GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr =string.Empty;
            if (SyllabusYr == "0")
                SyllabusYr = "null";
            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";

            DataSet titles = new DataSet();
            titles = dacc.select_method_wo_parameter(Sqlstr, "text");
            if (titles.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = titles;
                ddlTest.DataValueField = "Criteria_No";
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataBind();
                ddlTest.SelectedIndex = 0;
            }
        }
        catch
        {

        }
    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
            fpspread.SaveChanges();
            Hashtable ht = new Hashtable();
            ht.Clear();
            string batch_year = ddlBatch.SelectedItem.Text.ToString();
            string degree_code = ddlBranch.SelectedItem.Value.ToString();
            DataSet dsremark = new DataSet();
            int re = 0;
            //string accodeval1 = fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note;
            if (fpspread.Sheets[0].RowCount > 0)
            {
                if (fpspread.Sheets[0].ColumnCount > 0)
                {
                    for (int im = 3; im < fpspread.Sheets[0].ColumnCount; im++)
                    {
                        for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                        {
                            string examcode =string.Empty;
                            string acivityRemark =string.Empty;
                            string roll_no = fpspread.Sheets[0].Cells[i, 1].Text.ToString();
                            if (fpspread.Sheets[0].Cells[i, im].Text != "" && fpspread.Sheets[0].Cells[i, im].Text != null)
                                acivityRemark = fpspread.Sheets[0].Cells[i, im].Value.ToString();
                            string subcodeval = Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, im].Tag);
                            string sec = da.GetFunctionv("select Sections from Registration where Roll_No='" + roll_no + "'");
                            examcode = da.GetFunctionv("select exam_code from Exam_type where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and subject_no='" + subcodeval + "' and criteria_no='" + ddlTest.SelectedValue.ToString() + "' and sections='" + sec + "'");
                            // string acivityMark1 = fpspread.Sheets[0].Cells[i, fpspread.Sheets[0].ColumnCount - 1].Text.ToString();
                            //string accodeval1 = fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note;
                            if (acivityRemark.Trim() == "" || acivityRemark.Trim() == null)
                            {
                                acivityRemark =string.Empty;
                            }
                            if (examcode != "" && examcode != null)
                            {
                                string qryrmrk = "select * from result where  exam_code='" + examcode + "'  and roll_no='" + roll_no + "'";
                                dsremark = da.select_method_wo_parameter(qryrmrk, "Text");
                                if (dsremark.Tables[0].Rows.Count > 0)
                                {
                                    string strinsert = "update result set remarks='' where exam_code='" + examcode + "'  and roll_no='" + roll_no + "'";
                                    re = da.insert_method(strinsert, ht, "Text");
                                }
                                else
                                {
                                    lblErrorMsg.Text = "Please Enter The Marks in Mark Entry!!!";
                                    lblErrorMsg.Visible = true;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
            {
                for (int j = 3; j < fpspread.Sheets[0].ColumnCount; j++)
                {
                    fpspread.Sheets[0].Cells[i, j].Text =string.Empty;
                }
            }
            fpspread.SaveChanges();
            if (re == 1)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                btnfpspread1save.Text = "Save";
            }
        }
        catch (Exception ex)
        {
        }
        surediv.Visible = false;
    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {

        surediv.Visible = false;

    }

}