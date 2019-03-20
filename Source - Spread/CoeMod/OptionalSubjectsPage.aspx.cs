using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Configuration;


public partial class OptionalSubjectsPage : System.Web.UI.Page
{

    #region Variable And Object Declaration

    Hashtable hat = new Hashtable();
    string usercode = "", collegecode = "", singleuser = "", group_user = "", grouporusercode = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string qry = "";
    string sptype = "Text";
    string batchyear = "";
    string degreecode = "";
    string semester = "";
    string sec = "";
    string sub_no = "";

    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

    #endregion Variable And Object Declaration

    #region Page Load

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
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }

            if (!IsPostBack)
            {
                FpStudent.Visible = false;
                btnSave.Visible = false;
                bindBatch();
                bindDegree();
                bindBranch();
                bindSemester();
                bindSection();
                bindsubjects();
                FpStudent.Visible = false;
                btnSave.Visible = false;
                lblerrmsg.Visible = false;

                GridHeader();
            }
        }
        catch (Exception ex)
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
        FpStudent.Sheets[0].ColumnCount = 11;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 0].CellType = txt;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 1].CellType = txt;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 2].CellType = txt;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
        FpStudent.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Name";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Good";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Excellent";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Outstanding";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Average";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Completed";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Not Completed";
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 5].Tag = 1;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 6].Tag = 2;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 7].Tag = 3;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 8].Tag = 4;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 9].Tag = 5;
        FpStudent.Sheets[0].ColumnHeader.Cells[0, 10].Tag = 6;
        FpStudent.Sheets[0].Columns[0].Width = 37;
        FpStudent.Sheets[0].Columns[1].Width = 100;
        FpStudent.Sheets[0].Columns[2].Width = 100;
        FpStudent.Sheets[0].Columns[3].Width = 310;
        FpStudent.Sheets[0].Columns[4].Width = 200;
        FpStudent.Sheets[0].Columns[5].Width = 60;
        FpStudent.Sheets[0].Columns[6].Width = 100;
        FpStudent.Sheets[0].Columns[7].Width = 150;
        FpStudent.Sheets[0].Columns[8].Width = 100;
        FpStudent.Sheets[0].Columns[9].Width = 100;
        FpStudent.Sheets[0].Columns[10].Width = 100;

        FpStudent.Sheets[0].Columns[0].Locked = true;
        FpStudent.Sheets[0].Columns[1].Locked = true;
        FpStudent.Sheets[0].Columns[2].Locked = true;
        FpStudent.Sheets[0].Columns[3].Locked = true;

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
    #endregion Page Load

    #region Logout

    protected void logout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    #endregion Logout

    #region DropDown Events

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpStudent.Visible = false;
        btnSave.Visible = false;
        bindDegree();
        bindBranch();
        bindSemester();
        bindSection();
        bindsubjects();
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpStudent.Visible = false;
        btnSave.Visible = false;
        bindBranch();
        bindSemester();
        bindSection();
        bindsubjects();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpStudent.Visible = false;
        btnSave.Visible = false;
        bindSemester();
        bindSection();
        bindsubjects();
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpStudent.Visible = false;
        btnSave.Visible = false;
        bindSection();
        bindsubjects();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpStudent.Visible = false;
        btnSave.Visible = false;
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpStudent.Visible = false;
        btnSave.Visible = false;
    }

    #endregion DropDown Events

    #region Bind Header

    public void bindBatch()
    {
        try
        {
            ds.Clear();
            //ddlBatch.Items.Clear();
            //ds = d2.select_method_wo_parameter("bind_batch", "sp");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlBatch.DataSource = ds;
            //    ddlBatch.DataTextField = "batch_year";
            //    ddlBatch.DataValueField = "batch_year";
            //    ddlBatch.DataBind();
            //    ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
            //}
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
            collegecode = Convert.ToString(Session["collegecode"]);
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
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Convert.ToString(Session["collegecode"]); ;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddlDegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
                ddlBranch.SelectedIndex = 0;
            }
        }
        catch
        {

        }
    }

    public void bindSemester()
    {
        try
        {
            ddlSem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Convert.ToString(Session["collegecode"]) + "";
            //DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(i.ToString());
                        //ddlSemYr.Enabled = false;
                    }
                    else if (first_year == true && i == 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }

                }
            }
            else
            {
                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and college_code=" + Convert.ToString(Session["collegecode"]) + "";

                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(i.ToString());
                        }
                    }
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                ddlSem.SelectedIndex = 0;
            }
        }
        catch
        {

        }
    }

    public void bindSection()
    {
        try
        {
            ddlSec.Enabled = false;
            ddlSec.Items.Clear();
            hat.Clear();
            ds.Clear();
            ds = d2.BindSectionDetail(ddlBatch.SelectedValue, ddlBranch.SelectedValue);
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
                ddlSec.Items.Insert(0, "All");
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch
        {

        }
    }

    public void bindsubjects()
    {
        try
        {

            ds.Clear();
            ddlSubject.Items.Clear();
            ds = d2.select_method_wo_parameter("select distinct subject_no,Convert(Varchar(max),subject_code+' - '+subject_name) as Subject_Name from Subject where Part_Type='5'", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSubject.DataSource = ds;
                ddlSubject.DataTextField = "Subject_Name";
                ddlSubject.DataValueField = "subject_no";
                ddlSubject.DataBind();
                ddlSubject.SelectedIndex = ddlSubject.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
        }

    }

    #endregion Bind Header

    #region Button Event

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            int max_sem1 = 0;
            string max_sem = "";
            int cc = 0;
            GridHeader();
            FpStudent.Visible = false;
            btnSave.Visible = false;
            FpStudent.SaveChanges();
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string semlico = d2.GetFunction("select value from Master_Settings where settings='previous sem subject allotment' " + grouporusercode + "");


            string strorder = "ORDER BY Roll_No";
            string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY batch_year,degree_code,serialno";
            }
            else
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Stud_Name";
                }
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlBranch.SelectedValue);
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedItem);
            }
            if (ddlSec.Enabled)
            {
                if (ddlSec.Items.Count > 0)
                {
                    sec = Convert.ToString(ddlSec.SelectedItem.Text);
                    if (sec.ToLower() == "all")
                    {
                        sec = "";
                    }
                }
            }
            else
            {
                sec = "";
            }
            //int stusemester = Convert.ToInt32(d2.GetFunction("select distinct isnull(Current_Semester,'0') sem from Registration where Batch_Year='" + batchyear + "' and degree_code in('" + degreecode + "')  order by sem"));//and cc=0 and DelFlag=0 and Exam_Flag<>'debar'
            //semester = Convert.ToString(stusemester);
            if (ddlSubject.Items.Count == 0)
            {
                FpStudent.Visible = false;
                btnSave.Visible = false;
                lbl_popuperr.Text = "No Recors Found ";
                errdiv.Visible = true;
                return;
            }
            else
            {
                sub_no = Convert.ToString(ddlSubject.SelectedValue);
            }



            if (batchyear != "" && batchyear != null && degreecode != "" && degreecode != null && semester != "" && semester != null)
            {

                max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchyear + "'  and Degree_code='" + degreecode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                if (max_sem == "" || max_sem == null)
                {
                    max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreecode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                }
                int.TryParse(max_sem, out max_sem1);
                if (cbpassedout.Checked)
                {
                    semester = Convert.ToString((max_sem1 + 1));
                    cc = 1;
                }

                ds.Clear();
                if (sec != null && sec != "")
                {

                    qry = "select serialno,App_No,Roll_No,Reg_No,degree_code,batch_year,Stud_Name from Registration where degree_code='" + degreecode + "' and Batch_Year='" + batchyear + "'  and Sections='" + sec + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'  and Exam_Flag<>'debar' and DelFlag=0 " + strorder + " ; select * from SpecialCourseSubject where Subject_No='" + sub_no + "' and CurrentSem='" + semester + "' ";
                    ds = d2.select_method_wo_parameter(qry, sptype);
                    //and Current_Semester='" + semester + "' and CC='" + cc + "'
                }
                else
                {
                    qry = "select serialno,App_No,Roll_No,Reg_No,degree_code,batch_year,Stud_Name from Registration where degree_code='" + degreecode + "' and Batch_Year='" + batchyear + "'  and college_code='" + Convert.ToString(Session["collegecode"]) + "'  and Exam_Flag<>'debar' and DelFlag=0 " + strorder + " ; select * from SpecialCourseSubject where Subject_No='" + sub_no + "' and CurrentSem='" + semester + "' ";
                    ds = d2.select_method_wo_parameter(qry, sptype);
                    //and Current_Semester='" + semester + "' and CC='" + cc + "'
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType chkeach = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.ComboBoxCellType comboeach = new FarPoint.Web.Spread.ComboBoxCellType();
                    string qrysub = "select distinct subject_no,Convert(Varchar(max),subject_code+' - '+subject_name) as Subject_Name from Subject where Part_Type='5'";
                    DataSet dssub = new DataSet();
                    dssub = d2.select_method_wo_parameter(qrysub, "Text");
                    if (dssub.Tables[0].Rows.Count > 0)
                    {
                        comboeach.DataSource = dssub;
                        comboeach.DataTextField = "Subject_Name";
                        comboeach.DataValueField = "subject_no";
                    }
                    else
                    {
                        FpStudent.Visible = false;
                        btnSave.Visible = false;
                        lbl_popuperr.Text = "There is No Part-V Subjects were Found.Please Allocate Subject To Part-V ";
                        errdiv.Visible = true;
                        return;
                    }
                    //comboeach.DataB
                    chkall.AutoPostBack = true;
                    chkeach.AutoPostBack = true;
                    FpStudent.Sheets[0].RowCount = 0;
                    FpStudent.Sheets[0].RowCount++;

                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].CellType = chkall;
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].CellType = chkall;
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 7].CellType = chkall;
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 8].CellType = chkall;
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 9].CellType = chkall;
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 10].CellType = chkall;
                    
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Value = 1;
                    for (int stu = 0; stu < ds.Tables[0].Rows.Count; stu++)
                    {
                        string app_no = Convert.ToString(ds.Tables[0].Rows[stu]["App_No"]);
                        DataView dv = new DataView();
                        FpStudent.Sheets[0].RowCount++;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].CellType = txt;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].CellType = txt;
                     
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(stu + 1);

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[stu]["Reg_No"]);
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[stu]["App_No"]);


                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[stu]["Roll_No"]);

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[stu]["Stud_Name"]);

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].CellType = comboeach;

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].CellType = chkeach;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].CellType = chkeach;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 7].CellType = chkeach;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 8].CellType = chkeach;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 9].CellType = chkeach;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 10].CellType = chkeach;
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "App_no='" + app_no + "'";
                            dv = ds.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                if (Convert.ToString(dv[0]["MarkType"]) == "1")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Value = 1;
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "2")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].Value = 1;
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "3")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 7].Value = 1;
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "4")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 8].Value = 1;
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "5")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 9].Value = 1;
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "6")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 10].Value = 1;
                                }
                                string sub_no1 = Convert.ToString(dv[0]["Subject_No"]);
                                string sub_name = d2.GetFunctionv("select subject_no,Convert(Varchar(max),subject_code+' - '+subject_name) as Subject_Name from subject where subject_no='" + sub_no1 + "' and Part_Type='5'");
                                if (sub_name != "" && sub_no1 != "" && sub_name != null && sub_no1 != null)
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = sub_name;
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Value = sub_no1;
                                }
                                else
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = "";
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Value = "";
                                }
                            }
                            else
                            {
                                FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Value = 1;
                            }
                        }
                        else
                        {
                            FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Value = 1;
                        }
                    }
                    FpStudent.Visible = true;
                    btnSave.Visible = true;
                    FpStudent.Sheets[0].PageSize = FpStudent.Sheets[0].RowCount;
                    FpStudent.Height = (FpStudent.Sheets[0].RowCount * 23) + 24;
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
            else
            {

            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int max_sem1 = 0;
            string max_sem = "";
            int cc = 0;
            FpStudent.SaveChanges();
            string subject_no = Convert.ToString(ddlSubject.SelectedValue);
            string findfinalSem = Convert.ToString(ddlSem.Items.Count);
            string curr_sem = Convert.ToString(ddlSem.SelectedItem);
            bool isfinal = false;
            string app_no = "";
            string mark_type = "";
            bool result = false;
            if (ddlBatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlBranch.SelectedValue);
            }
            max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchyear + "'  and Degree_code='" + degreecode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
            if (max_sem == "" || max_sem == null)
            {
                max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreecode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
            }
            max_sem = ddlSem.SelectedItem.Text;
            int.TryParse(max_sem, out max_sem1);
            if (cbpassedout.Checked)
            {
                semester = Convert.ToString((max_sem1 + 1));
                cc = 1;
                isfinal = true;
                curr_sem = semester;
            }
            else
            {
                isfinal = false;
            }
            //if (cbpassedout.Checked)
            //{
            //    curr_sem = Convert.ToString(max_sem1);
            //    isfinal = true;
            //    cc = 1;
            //}
            qry = "";
            if (FpStudent.Sheets[0].RowCount > 0)
            {
                for (int row = 1; row < FpStudent.Sheets[0].RowCount; row++)
                {
                    app_no = Convert.ToString(FpStudent.Sheets[0].Cells[row, 1].Tag);
                    for (int col = 5; col < FpStudent.Sheets[0].ColumnCount; col++)
                    {
                        int val = Convert.ToInt32(FpStudent.Sheets[0].Cells[row, col].Value);
                        if (val == 1)
                        {
                            mark_type = Convert.ToString(FpStudent.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                            //break;
                        }
                    }
                    subject_no = Convert.ToString(FpStudent.Sheets[0].Cells[row, 4].Value);
                    if (app_no != "" && app_no != null & subject_no != "0" && subject_no != "" && subject_no != null)
                    {
                        //Subject_No='" + subject_no + "' and Subject_No='" + subject_no + "' and
                        qry = "if exists (select * from SpecialCourseSubject where  App_no='" + app_no + "' and CurrentSem='" + curr_sem + "')update SpecialCourseSubject set MarkType='" + mark_type + "',subject_no='" + subject_no + "' where  App_no='" + app_no + "' and CurrentSem='" + curr_sem + "'  else  insert into SpecialCourseSubject (Subject_No,App_no,MarkType,IsFinalsem,CurrentSem) values ('" + subject_no + "','" + app_no + "','" + mark_type + "','" + isfinal + "','" + curr_sem + "')";
                        int res = d2.update_method_wo_parameter(qry, "Text");
                        if (res == 1)
                        {
                            result = true;
                        }
                    }
                }
            }
            else
            {
                FpStudent.Visible = false;
                btnSave.Visible = false;
                lbl_popuperr.Text = "No Records Found ";
                errdiv.Visible = true;
                return;
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
        catch (Exception ex)
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

    protected void FpStudent_ButtonCommand(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(FpStudent.Sheets[0].Cells[0, 6].Value) == 1)
            {
                for (int i = 0; i < FpStudent.Sheets[0].RowCount; i++)
                {
                    FpStudent.Sheets[0].Cells[i, 6].Value = 1;
                }

            }
            else if (Convert.ToInt32(FpStudent.Sheets[0].Cells[0, 6].Value) == 0)
            {
                for (int i = 0; i < FpStudent.Sheets[0].RowCount; i++)
                {
                    FpStudent.Sheets[0].Cells[i, 6].Value = 0;
                }

            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void FpStudent_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string r = FpStudent.Sheets[0].ActiveRow.ToString();
            string j = FpStudent.Sheets[0].ActiveColumn.ToString();
            int k = Convert.ToInt32(j);

            int a = Convert.ToInt32(r);
            int b = Convert.ToInt32(j);

            if (r.Trim() != "")
            {
                if (Convert.ToInt32(r) == 0)
                {
                    if (r.Trim() != "" && j.Trim() != "")
                    {
                        if (FpStudent.Sheets[0].RowCount > 0)
                        {
                            int checkval = Convert.ToInt32(FpStudent.Sheets[0].Cells[a, b].Value);
                            if (checkval == 0)
                            {
                                string headervalue = Convert.ToString(FpStudent.Sheets[0].ColumnHeader.Cells[0, b].Tag);
                                for (int i = 1; i < FpStudent.Sheets[0].RowCount; i++)
                                {
                                    int checkvalue = Convert.ToInt32(FpStudent.Sheets[0].Cells[i, b].Value);
                                    int checkvalue1 = Convert.ToInt32(FpStudent.Sheets[0].Cells[i, b].Value);
                                    if (headervalue.Trim() == "1")
                                    {
                                        FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        FpStudent.Sheets[0].Cells[i, b + 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 4].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 5].Value = 0;

                                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 4].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 5].Value = 0;
                                    }
                                    if (headervalue.Trim() == "2")
                                    {
                                        FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        FpStudent.Sheets[0].Cells[i, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 4].Value = 0;

                                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 4].Value = 0;
                                    }
                                    if (headervalue.Trim() == "3")
                                    {
                                        FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        FpStudent.Sheets[0].Cells[i, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 2].Value = 0;

                                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 3].Value = 0;
                                    }
                                    if (headervalue.Trim() == "4")
                                    {
                                        FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        FpStudent.Sheets[0].Cells[i, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 2].Value = 0;

                                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                                    }

                                    if (headervalue.Trim() == "5")
                                    {
                                        FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        FpStudent.Sheets[0].Cells[i, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 4].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 5].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b + 1].Value = 0;

                                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 4].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 5].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                                    }
                                    if (headervalue.Trim() == "6")
                                    {
                                        FpStudent.Sheets[0].Cells[i, b].Value = 1;
                                        FpStudent.Sheets[0].Cells[i, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 4].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 5].Value = 0;
                                        FpStudent.Sheets[0].Cells[i, b - 6].Value = 0;

                                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 3].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 4].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 5].Value = 0;
                                        FpStudent.Sheets[0].Cells[a, b - 6].Value = 0;
                                    }

                                }
                            }
                            if (checkval == 1)
                            {
                                for (int i = 1; i < FpStudent.Sheets[0].RowCount; i++)
                                {
                                    FpStudent.Sheets[0].Cells[i, b].Value = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    string headervalue = Convert.ToString(FpStudent.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(j)].Tag);

                    if (headervalue.Trim() == "1")
                    {
                        //FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        //FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                        //FpStudent.Sheets[0].Cells[a, b + 3].Value = 0;

                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 3].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 4].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 5].Value = 0;
                    }
                    if (headervalue.Trim() == "2")
                    {
                        //FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        //FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        //FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 3].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 4].Value = 0;
                    }
                    if (headervalue.Trim() == "3")
                    {
                        //FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        //FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                        //FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 3].Value = 0;
                    }
                    if (headervalue.Trim() == "4")
                    {
                        //FpStudent.Sheets[0].Cells[a, b - 3].Value = 0;
                        //FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                        //FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 3].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 2].Value = 0;
                    }
                    if (headervalue.Trim() == "5")
                    {
                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 3].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 4].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 5].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b + 1].Value = 0;
                    }
                    if (headervalue.Trim() == "6")
                    {
                        FpStudent.Sheets[0].Cells[a, b - 1].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 2].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 3].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 4].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 5].Value = 0;
                        FpStudent.Sheets[0].Cells[a, b - 6].Value = 0;
                    }

                }
            }
        }
        catch
        {

        }
    }

    protected void btnview_Click(object sender, EventArgs e)
    {
        try
        {
            int max_sem1 = 0;
            string max_sem = "";
            int cc = 0;
            FpStudent.Visible = false;
            btnSave.Visible = false;
            btnDelete.Visible = false;
            FpStudent.SaveChanges();
            columnBind();
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkeach = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;
            chkeach.AutoPostBack = false;
            string strorder = "ORDER BY Roll_No";
            string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY batch_year,degree_code,serialno";
            }
            else
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY batch_year,degree_code,sections,Roll_No,Stud_Name";
                }
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlBranch.SelectedValue);
            }
            if (ddlSem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlSem.SelectedItem);
            }
            if (ddlSec.Enabled)
            {
                if (ddlSec.Items.Count > 0)
                {
                    sec = Convert.ToString(ddlSec.SelectedItem.Text);
                    if (sec.ToLower() == "all")
                    {
                        sec = "";
                    }
                }
            }
            else
            {
                sec = "";
            }



            if (batchyear != "" && batchyear != null && degreecode != "" && degreecode != null && semester != "" && semester != null)
            {

                max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchyear + "'  and Degree_code='" + degreecode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                if (max_sem == "" || max_sem == null)
                {
                    max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreecode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
                }
                int.TryParse(max_sem, out max_sem1);
                if (cbpassedout.Checked)
                {
                    semester = Convert.ToString((max_sem1 + 1));
                    cc = 1;
                }

                ds.Clear();
                if (sec != null && sec != "")
                {
                    qry = "select serialno,App_No,Roll_No,Reg_No,degree_code,batch_year,Stud_Name from Registration where degree_code='" + degreecode + "' and Batch_Year='" + batchyear + "'  and Sections='" + sec + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and Exam_Flag<>'debar' and DelFlag=0 " + strorder + " ; select * from SpecialCourseSubject where  CurrentSem='" + semester + "' ";
                    ds = d2.select_method_wo_parameter(qry, sptype);
                    //Subject_No='" + sub_no + "' and and Current_Semester='" + semester + "' and CC='" + cc + "'
                }
                else
                {
                    qry = "select serialno,App_No,Roll_No,Reg_No,degree_code,batch_year,Stud_Name from Registration where degree_code='" + degreecode + "' and Batch_Year='" + batchyear + "'  and college_code='" + Convert.ToString(Session["collegecode"]) + "'  and Exam_Flag<>'debar' and DelFlag=0 " + strorder + " ; select * from SpecialCourseSubject where  CurrentSem='" + semester + "' ";
                    //Subject_No='" + sub_no + "' and and Current_Semester='" + semester + "' and CC='" + cc + "'
                    ds = d2.select_method_wo_parameter(qry, sptype);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {

                    FpStudent.Sheets[0].RowCount = 0;
                    FpStudent.Sheets[0].RowCount++;
                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].CellType = chkall;
                    for (int stu = 0; stu < ds.Tables[0].Rows.Count; stu++)
                    {
                        string app_no = Convert.ToString(ds.Tables[0].Rows[stu]["App_No"]);
                        DataView dv = new DataView();
                        FpStudent.Sheets[0].RowCount++;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(stu + 1);

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[stu]["Reg_No"]); 
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[stu]["App_No"]);


                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[stu]["Roll_No"]);

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[stu]["Stud_Name"]);

                        FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 6].CellType = chkeach;
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "App_no='" + app_no + "'";
                            dv = ds.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                if (Convert.ToString(dv[0]["MarkType"]) == "1")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Good";
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "2")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Excellent";
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "3")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Outstanding";
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "4")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Average";
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "5")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Completed";
                                }
                                else if (Convert.ToString(dv[0]["MarkType"]) == "6")
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 5].Text = "Not Completed";
                                }
                                string sub_no1 = Convert.ToString(dv[0]["Subject_No"]);
                                string sub_name = d2.GetFunctionv("select Convert(Varchar(max),subject_code+' - '+subject_name) as Subject_Name from subject where subject_no='" + sub_no1 + "' and Part_Type='5'");
                                if (sub_name != "" && sub_no1 != "" && sub_name != null && sub_no1 != null)
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = sub_name;
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Tag = sub_no1;
                                }
                                else
                                {
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Text = "";
                                    FpStudent.Sheets[0].Cells[FpStudent.Sheets[0].RowCount - 1, 4].Value = "";
                                }
                            }
                        }

                    }
                    FpStudent.Visible = true;
                    // btnSave.Visible = true;
                    btnDelete.Visible = true;
                    FpStudent.Sheets[0].PageSize = FpStudent.Sheets[0].RowCount;
                    FpStudent.Height = (FpStudent.Sheets[0].RowCount * 23) + 24;
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
            else
            {

            }

        }
        catch (Exception ex)
        {

        }
    }
    public void columnBind()
    {
        try
        {
            FpStudent.Sheets[0].AutoPostBack = false;
            FpStudent.CommandBar.Visible = false;
            FpStudent.Sheets[0].SheetCorner.ColumnCount = 0;
            FpStudent.Sheets[0].ColumnCount = 0;
            FpStudent.Sheets[0].RowCount = 0;
            FpStudent.Sheets[0].ColumnCount = 7;
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            FpStudent.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Name";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Mark Type";
            FpStudent.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
            FpStudent.Sheets[0].Columns[0].Width = 37;
            FpStudent.Sheets[0].Columns[1].Width = 100;
            FpStudent.Sheets[0].Columns[2].Width = 100;
            FpStudent.Sheets[0].Columns[3].Width = 310;
            FpStudent.Sheets[0].Columns[4].Width = 200;

        }
        catch
        {

        }

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int max_sem1 = 0;
            string max_sem = "";
            string app_no = "";
            string mark_type = "";
            bool result = false;
            bool isfinal = false;
            string subject_no = string.Empty;
            qry = "";
            int cc = 0;
            if (ddlBatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlBranch.SelectedValue);
            }
            string curr_sem = Convert.ToString(ddlSem.SelectedItem);
            max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchyear + "'  and Degree_code='" + degreecode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
            if (max_sem == "" || max_sem == null)
            {
                max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreecode + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'");
            }
            int.TryParse(max_sem, out max_sem1);
            if (cbpassedout.Checked)
            {
                semester = Convert.ToString((max_sem1 + 1));
                cc = 1;
                isfinal = true;
                curr_sem = semester;
            }
            FpStudent.SaveChanges();
            if (FpStudent.Sheets[0].RowCount > 0)
            {
                for (int row = 1; row < FpStudent.Sheets[0].RowCount; row++)
                {
                    int val = Convert.ToInt32(FpStudent.Sheets[0].Cells[row, 6].Value);
                    if (val == 1)
                    {
                        app_no = Convert.ToString(FpStudent.Sheets[0].Cells[row, 1].Tag);
                        subject_no = Convert.ToString(FpStudent.Sheets[0].Cells[row, 4].Tag);

                        if (app_no != "" && app_no != null & subject_no != "0" && subject_no != "" && subject_no != null)
                        {
                            //Subject_No='" + subject_no + "' and Subject_No='" + subject_no + "' and
                            qry = "if exists (select * from SpecialCourseSubject where  App_no='" + app_no + "' and CurrentSem='" + curr_sem + "' and subject_no='" + subject_no + "') delete SpecialCourseSubject where subject_no='" + subject_no + "' and App_no='" + app_no + "' and CurrentSem='" + curr_sem + "'";
                            int res = d2.update_method_wo_parameter(qry, "Text");
                            if (res == 1)
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            else
            {
                FpStudent.Visible = false;
                btnSave.Visible = false;
                lbl_popuperr.Text = "No Records Found ";
                errdiv.Visible = true;
                return;
            }
            if (result == true)
            {
                btnview_Click(sender, e);
                lbl_popuperr.Text = "Deleted Successfully ";
                errdiv.Visible = true;
                return;
            }
            else
            {
                lbl_popuperr.Text = "Not Deleted";
                errdiv.Visible = true;
                return;
            }
        }
        catch
        {

        }
    }

    #endregion

}