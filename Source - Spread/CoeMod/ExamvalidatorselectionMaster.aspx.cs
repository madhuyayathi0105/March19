using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class ExamvalidatorselectionMaster : System.Web.UI.Page
{
    string CollegeCode;
    Boolean Cellclick = false;
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsss = new DataSet();
    Hashtable hat = new Hashtable();
    Hashtable hatsubject = new Hashtable();
    Hashtable staffdetail = new Hashtable();
    Boolean flag_true = false;
    int temp = 0;
    static int perty = 0;
    Boolean sfadd = false;
    static Boolean sfadd1 = false;
    static int rowvl = 0;
    static int dsscnt1 = 0;
    static Boolean first1 = false;
    static Boolean first = false;
    static Boolean secnd = false;
    static Boolean secnd1 = false;
    int tolpaperperval = 0;
    int tolnooffval = 0;
    int tolnoofstudent = 0;
    int tolvall = 0;
    int temp1 = 0;
    int bintperpapar = 0;
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
            CollegeCode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                RadioButtonList3.SelectedValue = "3";
                txtDate.Attributes.Add("Readonly", "Readonly");
                txtDate.Text = DateTime.Today.ToString("d-MM-yyyy");
                fpspread.Visible = false;
                Evaluation();
                degree();
                year1();
                hassstati();
                txtnoofval.Text = "";
                txttolperval.Text = "";
                //subjectbind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void year1()
    {
        dsss.Clear();
        dsss = da.Examyear();
        if (dsss.Tables[0].Rows.Count > 0)
        {
            ddlYear.DataSource = dsss;
            ddlYear.DataTextField = "Exam_year";
            ddlYear.DataValueField = "Exam_year";
            ddlYear.DataBind();
        }
        ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));

    }
    public void degree()
    {
        ddldegree.Items.Clear();
        string usercode = Session["usercode"].ToString();
        string collegecode = Session["collegecode"].ToString();
        string singleuser = Session["single_user"].ToString();
        string group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = da.select_method("bind_degree", hat, "sp");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
        ddldegree.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }
    public void bindbranch()
    {
        ddlbranch.Items.Clear();
        hat.Clear();
        string usercode = Session["usercode"].ToString();
        string collegecode = Session["collegecode"].ToString();
        string singleuser = Session["single_user"].ToString();
        string group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddldegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds = da.select_method("bind_branch", hat, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }
        ddlbranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }
    public void bindsem()
    {
        ddlsem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        hat.Clear();
        string usercode = Session["usercode"].ToString();
        string collegecode = Session["collegecode"].ToString();
        string singleuser = Session["single_user"].ToString();
        string group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }

        ds = da.BindSem(ddlbranch.SelectedValue.ToString(), ddlYear.SelectedValue.ToString(), collegecode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
            duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlsem.Items.Add(i.ToString());

                }
                else if (first_year == true && i != 2)
                {
                    ddlsem.Items.Add(i.ToString());
                }
            }
        }
        ddlsem.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }
    protected void month()
    {
        try
        {
            dsss.Clear();
            string year1 = ddlYear.SelectedValue;
            dsss = da.Exammonth(year1);
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = dsss;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();

                // ddlMonth1.DataSource = dsss;
                // ddlMonth1.DataTextField = "monthName";
                // ddlMonth1.DataValueField = "Exam_month";
                // ddlMonth1.DataBind();

            }
            // ddlMonth1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
            ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch
        {
        }

    }
    protected void Evaluation()
    {
        try
        {
            drpevl.Items.Clear();
            string noofevl = "";
            noofevl = da.GetFunction("select distinct isnull(value,'') as value from COE_Master_Settings where settings='Evaluation'");
            if (Convert.ToString(noofevl) != "" && Convert.ToInt16(noofevl) >= 1)
            {
                for (int evl = 1; evl <= Convert.ToInt16(noofevl); evl++)
                {
                    drpevl.Items.Add(Convert.ToString("Evaluation-" + evl));
                }
            }
            else
            {
                drpevl.Items.Add("Evaluation-1");
            }
            drpevl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
        catch
        {
        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void ddlMonth1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnreet1.Visible = false;
            btnsavel1.Visible = false;
            txttolperval.Text = "";
            txtnoofval.Text = "";
            AttSpread.Visible = false;
            btnreet1.Visible = false;
            hassstati();
            tolnoremaion.Visible = false;
            btnsavel1.Visible = false;
            lblerr1.Visible = false;
            AttSpread.Visible = false;
            //subjectbind();
        }
        catch
        {
        }
    }
    protected void ddlYear1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnreet1.Visible = false;
            btnsavel1.Visible = false;
            txttolperval.Text = "";
            txtnoofval.Text = "";
            hassstati();
            AttSpread.Visible = false;
            btnreet1.Visible = false;
            tolnoremaion.Visible = false;
            btnsavel1.Visible = false;
            lblerr1.Visible = false;
            AttSpread.Visible = false;
        }
        catch
        {
        }
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hassstati();
            lblerr1.Visible = false;
            fpspread.Visible = false;
            AttSpread.Visible = false;
            btnreet1.Visible = false;
        }
        catch
        {
        }
    }
    protected void ddlsem1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hassstati();
            btnreet1.Visible = false;
            btnsavel1.Visible = false;
            AttSpread.Visible = false;
            txttplstudent.Text = "";
            txttolperval.Text = "";
            txtnoofval.Text = "";
            tolnoremaion.Visible = false;
            lbleerrr.Visible = false;
            fpspread.Visible = false;
            AttSpread.Visible = false;
            btnreet1.Visible = false;
        }
        catch
        {
        }
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hassstati();
            lbleerrr.Visible = false;
            fpspread.Visible = false;
            AttSpread.Visible = false;
            btnreet1.Visible = false;
        }
        catch
        {
        }
    }
    protected void drpevl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hassstati();
            AttSpread.Visible = false;
            btnreet1.Visible = false;
            tolnoremaion.Visible = false;
            btnsavel1.Visible = false;
            lblerr1.Visible = false;
            AttSpread.Visible = false;

        }
        catch
        {
        }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerr1.Visible = false;
            hassstati();
            AttSpread.Visible = false;
            btnreet1.Visible = false;
            fpspread.Visible = false;
            month();
        }
        catch
        {
        }

    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hassstati();
            lbleerrr.Visible = false;
            fpspread.Visible = false;
            bindbranch();
        }
        catch
        {
        }
    }
    protected void ddldegree1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hassstati();
            btnreet1.Visible = false;
            btnsavel1.Visible = false;
            txttplstudent.Text = "";
            txttolperval.Text = "";
            txtnoofval.Text = "";
            tolnoremaion.Visible = false;
            lbleerrr.Visible = false;
            fpspread.Visible = false;
            AttSpread.Visible = false;
        }
        catch
        {
        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            hassstati();
            fpspread.Visible = false;
            lbleerrr.Visible = false;
        }
        catch
        {
        }

    }
    protected void ddlbranch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hassstati();
            btnreet1.Visible = false;
            btnsavel1.Visible = false;
            txttplstudent.Text = "";
            txttolperval.Text = "";
            txtnoofval.Text = "";
            tolnoremaion.Visible = false;
            fpspread.Visible = false;
            lbleerrr.Visible = false;
            AttSpread.Visible = false;
        }
        catch
        {
        }

    }
    protected void bindloaddataa()
    {
        try
        {
            fpspread.Sheets[0].RowCount = 0;
            fpspread.Sheets[0].RowHeader.Visible = false;
            fpspread.CommandBar.Visible = false;
            fpspread.Sheets[0].AutoPostBack = true;
            fpspread.Sheets[0].ColumnCount = 7;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpspread.Sheets[0].Columns[0].Width = 60;
            fpspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            //fpspread.Sheets[0].Columns[0].Locked = true;

            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            //  fpspread.Sheets[0].Columns[1].Locked = true;
            fpspread.Sheets[0].Columns[1].Width = 60;
            fpspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            //fpspread.Sheets[0].Columns[2].Locked = true;
            fpspread.Sheets[0].Columns[2].Width = 80;
            fpspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            //  fpspread.Sheets[0].Columns[3].Locked = true;
            fpspread.Sheets[0].Columns[3].Width = 160;
            fpspread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            //  fpspread.Sheets[0].Columns[4].Locked = true;
            fpspread.Sheets[0].Columns[4].Width = 70;
            fpspread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code And Name";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            //  fpspread.Sheets[0].Columns[5].Locked = true;
            fpspread.Sheets[0].Columns[5].Width = 250;
            fpspread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;

            fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Student";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            fpspread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            //  fpspread.Sheets[0].Columns[6].Locked = true;
            fpspread.Sheets[0].Columns[6].Width = 80;
            fpspread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

            //fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
            //fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            //fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            //fpspread.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            //fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            //fpspread.Sheets[0].Columns[7].Width = 60;
            //fpspread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            //fpspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Visible = false;

            string exammonth1 = ddlMonth.SelectedItem.Value.ToString();
            string ExamYear = ddlYear.SelectedItem.Text.ToString();
            string degree = ddldegree.SelectedValue.ToString();
            string semmter = ddlsem.SelectedValue.ToString();
            string branch = ddlbranch.SelectedValue.ToString();

            string strsql = "select distinct ed.subject_no,s.subject_code,s.subject_name,CONVERT(varchar(50),et.exam_date,105) as exam_date,e.Exam_Month,e.Exam_year,e.batch_year,e.current_semester,et.exam_session,c.Course_Name,d.Degree_Code,d.Acronym,de.Dept_Name,COUNT(ea.roll_no) as stucount from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et,Degree d,Department de,course c where e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and e.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code   and d.Course_Id=c.Course_Id and ea.roll_no in(select r.roll_no from Registration r where ea.roll_no=r.Roll_No and r.DelFlag=0 and r.Exam_Flag<>'debar') and e.Exam_Month='" + exammonth1 + "' and e.Exam_year='" + ExamYear + "' and d.Degree_Code='" + branch + "' and e.current_semester='" + semmter + "' group by ed.subject_no,s.subject_code,s.subject_name,et.exam_date,d.Degree_Code,et.exam_session,c.Course_Name,d.Acronym,de.Dept_Name,e.Exam_Month,e.Exam_year,e.batch_year,e.current_semester order by et.exam_date,et.exam_session,stucount desc,ed.subject_no";
            int sno = 0;
            ds = da.select_method_wo_parameter(strsql, "Text");

            string strsql1 = "   select tvs.subject_no,tvs.subject_code,tvs.subject_name,tvs.total_atten_stud ,CONVERT(varchar(50),tvs.Exam_date,105) as Exam_date ,tvs.Session,tvs.Exam_month ,tvs.Exam_year from Tbl_validatorselection tvs ,Tbl_validatorcount tvc where tvs.subject_no=tvc.subject_no and  tvs.Exam_month=tvc.Exam_month and tvs.Exam_year=tvc.Exam_year and tvs.Exam_Month='" + exammonth1 + "' and tvs.Exam_year='" + ExamYear + "' ";
            DataSet ds1 = da.select_method_wo_parameter(strsql1, "Text");
            DataView dv1 = new DataView();
            FarPoint.Web.Spread.CheckBoxCellType cheall = new FarPoint.Web.Spread.CheckBoxCellType();
            if (RadioButtonList3.SelectedValue == "1")
            {
                if (ds1.Tables[0].Rows.Count <= 0)
                {
                    lbleerrr.Visible = true;
                    lbleerrr.Text = "No Records Found";
                    fpspread.Visible = false;
                    lblerr1.Visible = true;
                    return;
                }
            }
            FarPoint.Web.Spread.CheckBoxCellType cheselectall = new FarPoint.Web.Spread.CheckBoxCellType();
            cheselectall.AutoPostBack = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                //fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, fpspread.Sheets[0].ColumnCount - 1].CellType = cheselectall;
                //fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 9);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                    string subnko = Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]);
                    string degreedetails = Convert.ToString(ds.Tables[0].Rows[i]["Course_Name"]);
                    string fphallno = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                    string DegreeCode = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                    string fpsubject = ds.Tables[0].Rows[i]["subject_code"] + " - " + Convert.ToString(ds.Tables[0].Rows[i]["subject_name"]);
                    string fptot_stud = Convert.ToString(ds.Tables[0].Rows[i]["stucount"]);
                    string date = Convert.ToString(ds.Tables[0].Rows[i]["exam_date"]);
                    string sessnn = Convert.ToString(ds.Tables[0].Rows[i]["exam_session"]);
                    string tagsubname = Convert.ToString(ds.Tables[0].Rows[i]["subject_name"]);
                    string tagsubcodes = Convert.ToString(ds.Tables[0].Rows[i]["subject_code"]);
                    string batch = Convert.ToString(ds.Tables[0].Rows[i]["batch_year"]);
                    string semm = Convert.ToString(ds.Tables[0].Rows[i]["current_semester"]);
                    string emonths = Convert.ToString(ds.Tables[0].Rows[i]["Exam_Month"]);
                    string eyears = Convert.ToString(ds.Tables[0].Rows[i]["Exam_year"]);

                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = batch;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Note = emonths;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = eyears;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = degreedetails;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Note = date;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Tag = sessnn;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = fphallno;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Note = DegreeCode;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = semm;

                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = fpsubject;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Note = subnko;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Tag = tagsubname;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = fptot_stud;

                    if (RadioButtonList3.SelectedValue == "3")
                    {
                        sno++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds1.Tables[0].DefaultView.RowFilter = "subject_no='" + subnko + "' and subject_code='" + tagsubcodes + "' and Exam_date='" + date + "' and Session='" + sessnn + "'and Exam_month='" + emonths + "' and Exam_year='" + eyears + "'";
                            dv1 = ds1.Tables[0].DefaultView;
                            if (dv1.Count > 0)
                            {
                                //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = cheall;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Locked = true;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Value = 1;
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.LightBlue;
                                // fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Locked = true;
                            }
                            else
                            {
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = cheall;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Locked = false;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Value = 0;
                                //  fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Locked = false;
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.White;
                            }

                        }
                        else
                        {
                            // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = cheall;
                            // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Locked = true;
                            // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Value = 0;
                            //  fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Locked = false;
                            fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.White;
                        }
                    }
                    else if (RadioButtonList3.SelectedValue == "2")
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds1.Tables[0].DefaultView.RowFilter = "subject_no='" + subnko + "' and subject_code='" + tagsubcodes + "' and Exam_date='" + date + "' and Session='" + sessnn + "'and Exam_month='" + emonths + "' and Exam_year='" + eyears + "'";
                            dv1 = ds1.Tables[0].DefaultView;
                            if (dv1.Count > 0)
                            {
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Visible = false;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = cheall;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Locked = true;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Value = 1;
                                // fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Locked = true;
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.LightBlue;
                            }
                            else
                            {
                                sno++;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Visible = true;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = cheall;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Locked = false;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Value = 0;
                                //  fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Locked = false;
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.White;
                            }

                        }
                        else
                        {
                            fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Visible = false;
                            // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = cheall;
                            // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Locked = true;
                            //  fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Value = 0;
                            // fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Locked = false;
                            fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.White;
                        }
                    }
                    else if (RadioButtonList3.SelectedValue == "1")
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds1.Tables[0].DefaultView.RowFilter = "subject_no='" + subnko + "' and subject_code='" + tagsubcodes + "' and Exam_date='" + date + "' and Session='" + sessnn + "'and Exam_month='" + emonths + "' and Exam_year='" + eyears + "'";
                            dv1 = ds1.Tables[0].DefaultView;
                            if (dv1.Count > 0)
                            {
                                sno++;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Visible = true;
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.LightBlue;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = cheall;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Locked = true;
                                // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Value = 1;
                                //  fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Locked = true;
                            }
                            else
                            {
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Visible = false;
                                //fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Locked = false;
                                fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.White;
                            }

                        }
                        else
                        {
                            sno++;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                            fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Visible = true;
                            fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = Color.LightBlue;
                            // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = cheall;
                            // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Locked = true;
                            // fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Value = 0;
                            //  fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].Locked = false;
                        }
                    }
                }
                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                fpspread.Visible = true;
                lbleerrr.Visible = false;
            }
            else
            {
                lbleerrr.Visible = true;
                lbleerrr.Text = "No Records Found";
                fpspread.Visible = false;
                lblerr1.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }

    }
    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            fpspread.Sheets[0].AutoPostBack = true;
            Cellclick = true;
            Accordion1.SelectedIndex = 1;
        }
        catch (Exception ex)
        {

        }
    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (Cellclick == true)
        {
            try
            {
                AttSpread.Visible = false;
                tolnoremaion.Visible = false;
                btnsavel1.Visible = false;
                btnreet1.Visible = false;
                fpspread.Sheets[0].AutoPostBack = true;
                int activerow = 0;
                txtnoofval.Text = "";
                txttolperval.Text = "";
                Accordion1.SelectedIndex = 2;
                AddPageModify.Text = "Add";
                activerow = Convert.ToInt32(fpspread.ActiveSheetView.ActiveRow.ToString());
                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                {
                    if (i == Convert.ToInt32(activerow))
                    {

                        fpspread.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        fpspread.Sheets[0].SelectionBackColor = Color.IndianRed;
                        fpspread.Sheets[0].SelectionForeColor = Color.White;
                    }
                    else
                    {
                        fpspread.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                }
                string DegreeCode = fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Note.ToString();
                string dateofexam = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Note.ToString();
                string[] fromdatespit99 = dateofexam.ToString().Split('-');
                dateofexam = fromdatespit99[2] + '-' + fromdatespit99[1] + '-' + fromdatespit99[0];
                string emonths = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note.ToString();
                txtmonth.Text = emonths.ToString();
                string eyears = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
                txtyear.Text = eyears.ToString();
                string batch = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                string degreedetails = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                txtdegre.Text = degreedetails.ToString();
                string sessd = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                string fphallno = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text.ToString();
                txtbaranch.Text = fphallno.ToString();
                string semm = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text.ToString();
                txtsem.Text = semm.ToString();
                string fpsubject = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text.ToString();
                txtsubjects.Text = fpsubject.ToString();
                string subnko = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Note.ToString();
                lblsuuuuno.Text = subnko.ToString();
                string tagsubname = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag.ToString();
                string fptot_stud = fpspread.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text.ToString();
                txttplstudent.Text = fptot_stud.ToString();
                string srtr = "select distinct  subject_no,subject_code+'-'+subject_name as sst,total_atten_stud,No_of_Papers_Per_Person,no_of_validator  from Tbl_validatorselection where subject_no='" + subnko.ToString() + "'";
                DataSet dsdate1 = new DataSet();
                dsdate1 = da.select_method_wo_parameter(srtr, "txt");
                if (dsdate1.Tables[0].Rows.Count > 0)
                {
                    txttplstudent.Text = dsdate1.Tables[0].Rows[0]["total_atten_stud"].ToString();
                    if (dsdate1.Tables[0].Rows[0]["No_of_Papers_Per_Person"].ToString().Trim() != null && dsdate1.Tables[0].Rows[0]["No_of_Papers_Per_Person"].ToString().Trim() != "")
                    {
                        txttolperval.Text = dsdate1.Tables[0].Rows[0]["No_of_Papers_Per_Person"].ToString();
                    }
                    else
                    {
                        txttolperval.Text = "";
                    }
                    if (dsdate1.Tables[0].Rows[0]["no_of_validator"].ToString().Trim() != null && dsdate1.Tables[0].Rows[0]["no_of_validator"].ToString().Trim() != "")
                    {
                        txtnoofval.Text = dsdate1.Tables[0].Rows[0]["no_of_validator"].ToString();
                    }
                    else
                    {
                        txtnoofval.Text = "";
                    }
                }
                string[] subcode = fpsubject.Split('-');
                string subjectnam = subcode[1].ToString();
                string subjectncod = subcode[0].ToString();
                string sqlquery = " if exists( select * from Tbl_validatorselection where subject_no='" + subnko + "' and Exam_month='" + emonths + "' and Exam_year='" + eyears + "') update Tbl_validatorselection set total_atten_stud='" + fptot_stud + "' where subject_no='" + subnko + "' and Exam_month='" + emonths + "' and Exam_year='" + eyears + "' else insert into Tbl_validatorselection (subject_no,subject_code,subject_name,total_atten_stud,Exam_date,Session,Exam_month,Exam_year,degree_code,current_sem)values('" + subnko + "','" + subjectncod + "','" + subjectnam + "','" + fptot_stud + "','" + dateofexam + "','" + sessd + "','" + emonths + "','" + eyears + "','" + DegreeCode + "','" + semm + "')";
                int save = da.insert_method(sqlquery, hat, "Text");
                if (save == 1)
                {
                    if (fpspread.Sheets[0].RowCount > 0)
                    {
                        fpspread.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            bindloaddataa();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            hassstati();
            if (txttolperval.Text == "")
            {
                tolnoremaion.Visible = false;
                btnreet1.Visible = false;
                btnsavel1.Visible = false;
                lblerr1.Visible = true;
                AttSpread.Visible = false;
                lblerr1.Text = "Please Enter Total Paper Per Validator";
            }
            else if (txtnoofval.Text == "")
            {
                btnreet1.Visible = false;
                tolnoremaion.Visible = false;
                btnsavel1.Visible = false;
                lblerr1.Visible = true;
                AttSpread.Visible = false;
                lblerr1.Text = "Please Enter No Of Validator";
            }
            else
            {
                int m1 = Convert.ToUInt16(txttplstudent.Text);
                int m2 = Convert.ToUInt16(txttolperval.Text);
                if (m1 >= m2)
                {
                    getstaffdatd();
                }
                else
                {
                    btnreet1.Visible = false;
                    tolnoremaion.Visible = false;
                    btnsavel1.Visible = false;
                    lblerr1.Visible = true;
                    AttSpread.Visible = false;
                    lblerr1.Text = "Please Enter  No Of Validation Papers Less Than Total No Of Student";
                }

            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }
    protected void btnsavel1_click(object sender, EventArgs e)
    {
        try
        {
            string Subjectnon = lblsuuuuno.Text.ToString();
            string exmonths = txtmonth.Text.ToString();
            string exyears = txtyear.Text.ToString();
            string date = txtDate.Text;
            string[] dateq = date.Split('-');
            date = dateq[2].ToString() + "-" + dateq[1].ToString() + "-" + dateq[0].ToString();
            DateTime date1 = Convert.ToDateTime(date);
            for (int i = 0; i < AttSpread.Sheets[0].Rows.Count; i++)
            {
                int ckkks = Convert.ToInt16(AttSpread.Sheets[0].Cells[i, 3].Value);
                if (ckkks == 1)
                {
                    string staffcodes = AttSpread.Sheets[0].Cells[i, 2].Note.ToString();
                    string staffcodesname = AttSpread.Sheets[0].Cells[i, 2].Text.ToString();
                    string papercount = AttSpread.Sheets[0].Cells[i, 5].Text.ToString();
                    string peroty = AttSpread.Sheets[0].Cells[i, 4].Text.ToString();
                    string regfrom = AttSpread.Sheets[0].Cells[i, 6].Text.ToString();
                    string regto = AttSpread.Sheets[0].Cells[i, 7].Text.ToString();

                    if (staffcodesname != "" && staffcodes != "" && papercount != "")
                    {
                        string sqlquery = "insert into Tbl_validatorcount (staff_code,staff_name,subject_no,Priority,total_paper_count,Exam_month,Exam_year,Reg_no_from,Reg_no_to,Evaluation,val_date)values ('" + staffcodes + "','" + staffcodesname + "','" + Subjectnon + "','" + peroty + "','" + papercount + "','" + exmonths + "','" + exyears + "','" + regfrom + "','" + regto + "','" + drpevl.SelectedItem.Text + "','" + date1 + "' ) ; update Tbl_validatorselection set no_of_validator ='" + txtnoofval.Text + "',No_of_Papers_Per_Person='" + txttolperval.Text + "' where subject_no='" + Subjectnon + "' and Exam_month='" + exmonths + "' and Exam_year='" + exyears + "'";
                        int save = da.insert_method(sqlquery, hat, "Text");
                        if (save == 2)
                        {
                            if (AttSpread.Sheets[0].RowCount > 0)
                            {
                                AttSpread.Visible = true;
                            }
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                        }
                    }
                }
            }

            hassstati();
            lblerr1.Visible = false;
            getstaffdatd();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }
    protected void btnreet1_Click(object sender, EventArgs e)
    {
        try
        {
            string Subjectnon = lblsuuuuno.Text.ToString();
            string exmonths = txtmonth.Text.ToString();
            string exyears = txtyear.Text.ToString();

            string sqlquery = "  delete from Tbl_validatorcount where subject_no='" + Subjectnon + "' and Exam_month='" + exmonths + "' and Exam_year='" + exyears + "' and Evaluation='" + drpevl.SelectedItem.Text + "';update Tbl_validatorselection set  no_of_validator=null ,No_of_Papers_Per_Person=null  where subject_no='" + Subjectnon + "' and Exam_month='" + exmonths + "' and Exam_year='" + exyears + "'";
            int save = da.insert_method(sqlquery, hat, "Text");
            if (save == 2)
            {
                if (AttSpread.Sheets[0].RowCount > 0)
                {
                    AttSpread.Visible = true;
                }
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
            }
            hassstati();
            txttolperval.Text = "";
            txtnoofval.Text = "";
            getstaffdatd();
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }
    //public void recordsave1()
    //{
    //    try
    //    {
    //        string Subjectnon = lblsuuuuno.Text.ToString();
    //        string exmonths = txtmonth.Text.ToString();
    //        string exyears = txtyear.Text.ToString();

    //        for (int i = 0; i < AttSpread.Sheets[0].Rows.Count; i++)
    //        {
    //            int isval = 0;
    //            isval = Convert.ToInt32(AttSpread.Sheets[0].Cells[i, 3].Value);
    //            if (isval == 1)
    //            {
    //                string subjectnamecode1 = AttSpread.Sheets[0].Cells[i, 1].Text.ToString();
    //                string sessd = AttSpread.Sheets[0].Cells[i, 4].Text.ToString();
    //                string subjectnamecode = AttSpread.Sheets[0].Cells[i, 2].Text.ToString();

    //                string sqlquery = " insert into Tbl_validatorcount (staff_code,staff_name,subject_no,Priority,Exam_month,Exam_year,Evaluation)values ('" + subjectnamecode1 + "','" + subjectnamecode + "','" + Subjectnon + "','" + sessd + "','" + exmonths + "','" + exyears + "','" + drpevl.SelectedItem.Text + "')";
    //                int save = da.insert_method(sqlquery, hat, "Text");
    //                if (save == 1)
    //                {
    //                    if (AttSpread.Sheets[0].RowCount > 0)
    //                    {
    //                        AttSpread.Visible = true;
    //                    }
    //                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
    //                }
    //            }
    //        }
    //        hassstati();
    //        //loadhalldetails();
    //        getstaffdatd();
    //    }
    //    catch
    //    {
    //    }
    //}
    private void getstaffdatd()
    {
        try
        {
            int nopaper = 0;
            int tolstd = Convert.ToInt16(txttplstudent.Text);
            if (txttolperval.Text != "")
            {
                int novaltor = Convert.ToInt16(txtnoofval.Text);
                nopaper = Convert.ToInt16(txttolperval.Text);
                int tool = nopaper * novaltor;
                if (tolstd > tool)
                {

                    lblerr1.Text = "Please Enter Valid No Of Validator";
                    AttSpread.Visible = false;
                    btnreet1.Visible = false;
                    hassstati();
                    tolnoremaion.Visible = false;
                    btnsavel1.Visible = false;
                    AttSpread.Visible = false;
                    lblerr1.Visible = true;
                    return;
                }
            }
            else
            {
                nopaper = 0;
            }


            AttSpread.Visible = false;
            btnreet1.Visible = false;
            AttSpread.Sheets[0].RowCount = 0;
            AttSpread.Sheets[0].ColumnHeader.RowCount = 1;
            AttSpread.Sheets[0].ColumnCount = 8;
            AttSpread.Sheets[0].RowHeader.Visible = false;
            AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            AttSpread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].DefaultStyle.Font.Bold = false;

            AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Validator Code";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Validator Name";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total No Of Evaluation Paper";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Register No From";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Register No To";
            AttSpread.CommandBar.Visible = false;
            AttSpread.Sheets[0].AutoPostBack = false;
            AttSpread.Width = 840;
            //AttSpread.Sheets[0].Columns[0].Locked = true;
            //AttSpread.Sheets[0].Columns[1].Locked = true;
            //AttSpread.Sheets[0].Columns[2].Locked = true;
            //AttSpread.Sheets[0].Columns[4].Locked = true;

            AttSpread.Sheets[0].Columns[0].Width = 80;
            AttSpread.Sheets[0].Columns[1].Width = 80;
            //AttSpread.Sheets[0].Columns[1].Visible = false;
            AttSpread.Sheets[0].Columns[2].Width = 200;
            AttSpread.Sheets[0].Columns[3].Width = 80;
            AttSpread.Sheets[0].Columns[4].Width = 80;
            AttSpread.Sheets[0].Columns[5].Width = 100;
            AttSpread.Sheets[0].Columns[6].Width = 100;
            AttSpread.Sheets[0].Columns[7].Width = 100;


            AttSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            AttSpread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            AttSpread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

            AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

            AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;

            AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;

            AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;

            AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            AttSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;

            string subjecyno = lblsuuuuno.Text.ToString();
            string exmonths = txtmonth.Text.ToString();
            string exyears = txtyear.Text.ToString();

            //string subjecyno = ddlSubject.SelectedValue.ToString();

            string query = "select * from examstaffmaster where Valuation=1  and month='" + exmonths + "' and year='" + exyears + "'  ;select distinct staff_code,staff_name from staffmaster union select distinct CONVERT(nvarchar(50), staff_code,0) as staff_code,staff_name from external_staff order by staff_name ;select * from Tbl_validatorcount where  Exam_month='" + exmonths + "' and Exam_year='" + exyears + "' and subject_no='" + subjecyno + "' and Evaluation='" + drpevl.SelectedItem.Text + "'  order by priority ";
            DataSet subTypeRs1 = da.select_method(query, hat, "Text");
            int height1 = 150;
            if (subTypeRs1.Tables[1].Rows.Count > 0)
            {
                int i = 0;
                int sno = 1;
                FarPoint.Web.Spread.CheckBoxCellType cheall1 = new FarPoint.Web.Spread.CheckBoxCellType();
                cheall1.AutoPostBack = true;
                if (subTypeRs1.Tables[1].Rows.Count > 0)
                {
                    for (int dc = 0; dc < subTypeRs1.Tables[1].Rows.Count; dc++)
                    {

                        string staff_nos = subTypeRs1.Tables[1].Rows[dc]["staff_code"].ToString();
                        string staffsa = subTypeRs1.Tables[1].Rows[dc]["staff_name"].ToString();

                        if (!staffdetail.Contains(staff_nos))
                        {
                            staffdetail.Add(staff_nos, staffsa);
                        }
                    }
                }

                for (int j = 0; j < subTypeRs1.Tables[0].Rows.Count; j++)
                {
                    string stafcodee = subTypeRs1.Tables[0].Rows[j]["staff_code"].ToString();
                    string Valsubject_no = subTypeRs1.Tables[0].Rows[j]["Val_subject_no"].ToString();

                    string[] subno = Valsubject_no.Split(',');
                    for (int colon = 0; colon <= subno.GetUpperBound(0); colon++)
                    {
                        if (subno[colon].ToString() == subjecyno)
                        {
                            if (!hatsubject.ContainsKey(stafcodee))
                            {
                                hatsubject.Add(stafcodee, subno[colon].ToString());
                                string stafnmme = GetCorrespondingKey(stafcodee, staffdetail).ToString();

                                if (subTypeRs1.Tables[2].Rows.Count > i)
                                {
                                    AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                                    height1 = height1 + AttSpread.Sheets[0].Rows[i].Height;

                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = subTypeRs1.Tables[2].Rows[i]["staff_name"].ToString();
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = subTypeRs1.Tables[2].Rows[i]["staff_code"].ToString();
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].Text = subTypeRs1.Tables[2].Rows[i]["staff_code"].ToString();
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Note = subTypeRs1.Tables[2].Rows[i]["total_paper_count"].ToString();
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = subTypeRs1.Tables[2].Rows[i]["Priority"].ToString();
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].CellType = cheall1;
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Locked = true;
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Value = 0;
                                    tolvall = tolvall + Convert.ToUInt16(subTypeRs1.Tables[2].Rows[i]["total_paper_count"].ToString());
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = subTypeRs1.Tables[2].Rows[i]["total_paper_count"].ToString();
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = subTypeRs1.Tables[2].Rows[i]["Reg_no_from"].ToString();
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Text = subTypeRs1.Tables[2].Rows[i]["Reg_no_to"].ToString();
                                }
                                else
                                {
                                    AttSpread.Sheets[0].RowCount = AttSpread.Sheets[0].RowCount + 1;
                                    height1 = height1 + AttSpread.Sheets[0].Rows[i].Height;

                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 0].Text = sno + "";
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 1].Text = stafcodee;
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Text = stafnmme;
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 2].Note = stafcodee;
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].CellType = cheall1;
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Locked = false;
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 3].Value = 0;
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 4].Text = "";
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 5].Text = "";
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 6].Text = "";
                                    AttSpread.Sheets[0].Cells[AttSpread.Sheets[0].RowCount - 1, 7].Text = "";
                                }
                                sno++;
                                i++;
                            }
                        }
                    }
                }

                if (height1 > 600)
                {
                    AttSpread.Height = 400;
                }
                else if (height1 > 500)
                {
                    AttSpread.Height = height1 - 200;
                }
                else if (height1 > 400)
                {
                    AttSpread.Height = height1 - 100;
                }
                else
                {
                    AttSpread.Height = height1;
                }
                AttSpread.SaveChanges();
                AttSpread.Visible = true;
                lblerr1.Visible = false;
                btnreet1.Visible = true;
                btnsavel1.Visible = true;
                int tolnoofstudent1 = Convert.ToInt16(txttplstudent.Text);
                if (tolnoofstudent1 == tolvall)
                {
                    temp1 = 0;
                    tolnoremaion.Text = "  Total No Of Student Remaining = " + Convert.ToString(temp);
                    tolnoremaion.Visible = true;
                }
                else
                {
                    temp1 = tolnoofstudent1 - tolvall;
                    tolnoremaion.Text = "  Total No Of Student Remaining = " + Convert.ToString(temp1);
                    tolnoremaion.Visible = true;
                }
                if (hatsubject.Count == 0)
                {
                    tolnoremaion.Visible = false;
                    AttSpread.Visible = false;
                    btnsavel1.Visible = false;
                    btnreet1.Visible = false;
                    lblerr1.Visible = true;
                    lblerr1.Text = "Please Allot Exam Validator Staff";
                    return;
                }
            }
            else
            {
                tolnoremaion.Visible = false;
                AttSpread.Visible = false;
                btnsavel1.Visible = false;
                btnreet1.Visible = false;
                lblerr1.Visible = true;
                lblerr1.Text = "Please Allot Exam Validator Staff";
                return;
            }
        }

        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }

    protected void RadioButtonList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        tolnoremaion.Visible = false;
        fpspread.Visible = false;
        AttSpread.Visible = false;
        btnsavel1.Visible = false;
        btnreet1.Visible = false;
        lblerr1.Visible = false;
    }
    protected void AttSpread_OnUpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Boolean newckg = false;
            tolpaperperval = Convert.ToInt16(txttolperval.Text);
            int divval = tolpaperperval / 2;
            tolnooffval = Convert.ToInt16(txtnoofval.Text);
            string actrow = e.CommandArgument.ToString();
            string actcol = e.SheetView.ActiveColumn.ToString();
            string seltext = e.EditValues[3].ToString();
            lblerr1.Visible = false;
            if (txttolperval.Text == "")
            {
                tolnoremaion.Visible = false;
                btnreet1.Visible = false;
                btnsavel1.Visible = false;
                lblerr1.Visible = true;
                AttSpread.Visible = false;
                lblerr1.Text = "Please Enter Total Paper Per Validator";
                return;
            }
            else if (txtnoofval.Text == "")
            {
                btnreet1.Visible = false;
                tolnoremaion.Visible = false;
                btnsavel1.Visible = false;
                lblerr1.Visible = true;
                AttSpread.Visible = false;
                lblerr1.Text = "Please Enter No Of Validator";
                return;
            }
            else
            {
                if (seltext == "True" && sfadd == false)
                {
                    string subjecyno = lblsuuuuno.Text.ToString();
                    string exmonths = txtmonth.Text.ToString();
                    string exyears = txtyear.Text.ToString();

                    string query = "  select MAX(Priority) as Priority from  Tbl_validatorcount where subject_no='" + subjecyno + "' and Exam_month='" + exmonths + "' and Exam_year='" + exyears + "' and Evaluation='" + drpevl.SelectedItem.Text + "' ;select distinct re.Reg_No,Exam_Month,Exam_year,exam_session,re.Current_Semester,s.subject_code+'-'+subject_name as subjectname from Exam_Details e,exam_application ea,exam_appl_details ed,subject s,exmtt_det et,Degree d,Department de,course c,Registration re where re.Roll_No=ea.roll_no and e.exam_code=ea.exam_code and ea.appl_no=ed.appl_no and ed.subject_no=s.subject_no and s.subject_no=et.subject_no and e.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code   and d.Course_Id=c.Course_Id and ea.roll_no in(select r.roll_no from Registration r where ea.roll_no=r.Roll_No and  r.Exam_Flag<>'debar') and  e.Exam_month='" + exmonths + "' and e.Exam_year='" + exyears + "' and s.subject_no='" + subjecyno + "' order by  re.Reg_No asc";
                    DataSet subTypeRs1 = da.select_method(query, hat, "Text");
                    string vald = subTypeRs1.Tables[0].Rows[0]["Priority"].ToString();

                    if (sfadd1 == false)
                    {
                        if (vald.Trim().ToString() != "0" && vald.Trim().ToString() != null && vald.Trim().ToString() != "")
                        {
                            perty = Convert.ToInt16(subTypeRs1.Tables[0].Rows[0]["Priority"].ToString());
                            perty++;
                        }
                        else
                        {
                            perty++;
                        }
                        sfadd1 = true;
                    }

                    else
                    {
                        perty++;
                    }
                    if (tolnoremaion.Text == "")
                    {
                        tolnoofstudent = Convert.ToInt16(txttplstudent.Text);
                        temp = tolnoofstudent - tolpaperperval;
                    }
                    else
                    {
                        string datas = tolnoremaion.Text.ToString();
                        string[] splt = datas.Split('=');
                        int renain = Convert.ToInt16(splt[1].ToString());
                        if (renain > tolpaperperval)
                        {
                            temp = renain - tolpaperperval;

                        }
                        else
                        {
                            newckg = true;
                            bintperpapar = renain;
                            temp = 0;
                        }
                    }
                    if (temp < 0)
                    {

                        lblerr1.Visible = true;
                        lblerr1.Text = "All The Student Paper Alotted";
                        // AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol)+2].Value = 0;
                        AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 2].Text = "";
                        AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 3].Text = "";
                        AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 4].Text = "";
                        return;
                    }
                    else if (temp <= divval)
                    {
                        if (newckg == false)
                        {
                            bintperpapar = temp + tolpaperperval;
                        }
                    }
                    else
                    {
                        bintperpapar = tolpaperperval;
                    }
                    if (temp > divval)
                    {
                        tolnoremaion.Text = "  Total No Of Student Remaining = " + Convert.ToString(temp);
                        tolnoremaion.Visible = true;
                    }
                    else
                    {
                        temp = 0;
                        tolnoremaion.Text = "  Total No Of Student Remaining = " + Convert.ToString(temp);
                        tolnoremaion.Visible = true;
                    }
                    dsscnt1 = bintperpapar;
                    if (first == false)
                    {
                        if (first1 == false)
                        {
                            string regno1 = subTypeRs1.Tables[1].Rows[0]["Reg_No"].ToString();
                            AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 3].Text = regno1;
                            first1 = true;
                        }
                        else
                        {
                            rowvl = rowvl + 1;
                            string regno12 = subTypeRs1.Tables[1].Rows[rowvl]["Reg_No"].ToString();
                            AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 3].Text = regno12;
                            // first = true;
                        }
                    }
                    if (secnd == false)
                    {
                        if (secnd1 == false)
                        {
                            rowvl = dsscnt1 - 1;
                            string regno2 = subTypeRs1.Tables[1].Rows[rowvl]["Reg_No"].ToString();
                            AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 4].Text = regno2;
                            secnd1 = true;
                        }
                        else
                        {
                            dsscnt1 = dsscnt1 + rowvl;
                            rowvl = dsscnt1 - 1;
                            string regno22 = subTypeRs1.Tables[1].Rows[rowvl]["Reg_No"].ToString();
                            AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 4].Text = regno22;
                            // secnd = true;
                        }
                    }
                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 2].Text = bintperpapar.ToString();
                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 2].Note = bintperpapar.ToString();

                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 1].Text = perty.ToString();
                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 1].Note = perty.ToString();
                    sfadd = true;
                    AttSpread.SaveChanges();
                }
                else if (seltext == "False" && sfadd == false)
                {
                    string vallds1 = AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 1].Text.ToString();
                    int ddd = 0;
                    if (tolnoremaion.Text == "")
                    {
                        tolnoofstudent = Convert.ToInt16(txttplstudent.Text);
                        temp = tolnoofstudent + tolpaperperval;
                    }
                    else
                    {
                        string datas = tolnoremaion.Text.ToString();
                        string[] splt = datas.Split('=');

                        ddd = Convert.ToInt16(AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 2].Text.ToString());
                        temp = Convert.ToInt16(splt[1].ToString()) + ddd;
                    }
                    if (temp < 0)
                    {
                        lblerr1.Visible = true;
                        lblerr1.Text = "All The Student Paper Alotted";
                        // AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol)].Value = 0;
                        AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 2].Text = "";
                        AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 3].Text = "";
                        AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 4].Text = "";
                        return;
                    }
                    if (temp > divval)
                    {
                        tolnoremaion.Text = "  Total No Of Student Remaining = " + Convert.ToString(temp);
                        tolnoremaion.Visible = true;
                    }
                    else
                    {
                        temp = 0;
                        tolnoremaion.Text = "  Total No Of Student Remaining = " + Convert.ToString(temp);
                        tolnoremaion.Visible = true;
                    }
                    rowvl = rowvl + 1;
                    rowvl = rowvl - ddd;

                    if (rowvl == 0)
                    {
                        rowvl = 0;
                        first1 = false;
                        secnd1 = false;
                    }
                    else
                    {
                        rowvl = rowvl - 1;
                    }
                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 2].Text = "";
                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 2].Note = "";
                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 3].Text = "";
                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 4].Text = "";

                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 1].Text = "";
                    AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol) + 1].Note = "";
                    sfadd = true;
                    AttSpread.SaveChanges();
                    perty = perty - 1;

                    for (int i = 0; i < AttSpread.Sheets[0].RowCount; i++)
                    {
                        int ckkks = Convert.ToInt16(AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol)].Value);
                        if (ckkks == 1)
                        {
                            int vallds2 = Convert.ToInt16(AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 1].Text.ToString());

                            if (Convert.ToInt16(vallds1) <= vallds2)
                            {
                                ddd = 0;
                                if (tolnoremaion.Text == "")
                                {
                                    tolnoofstudent = Convert.ToInt16(txttplstudent.Text);
                                    temp = tolnoofstudent + tolpaperperval;
                                }
                                else
                                {
                                    string datas = tolnoremaion.Text.ToString();
                                    string[] splt = datas.Split('=');

                                    ddd = Convert.ToInt16(AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 2].Text.ToString());
                                    temp = Convert.ToInt16(splt[1].ToString()) + ddd;
                                }
                                if (temp <= 0)
                                {
                                    lblerr1.Visible = true;
                                    lblerr1.Text = "All The Student Paper Alotted";
                                    // AttSpread.Sheets[0].Cells[Convert.ToInt16(actrow), Convert.ToInt16(actcol)].Value = 0;
                                    AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol)].Value = 0;
                                    AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 2].Text = "";
                                    AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 3].Text = "";
                                    AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 4].Text = "";
                                    return;
                                }
                                if (temp > divval)
                                {
                                    tolnoremaion.Text = "  Total No Of Student Remaining = " + Convert.ToString(temp);
                                    tolnoremaion.Visible = true;
                                }
                                else
                                {
                                    temp = 0;
                                    tolnoremaion.Text = "  Total No Of Student Remaining = " + Convert.ToString(temp);
                                    tolnoremaion.Visible = true;
                                }
                                rowvl = rowvl + 1;
                                rowvl = rowvl - ddd;

                                if (rowvl == 0)
                                {
                                    rowvl = 0;
                                    first1 = false;
                                    secnd1 = false;
                                }
                                else
                                {
                                    rowvl = rowvl - 1;
                                }
                                AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 2].Text = "";
                                AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 2].Note = "";
                                AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 3].Text = "";
                                AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 4].Text = "";
                                AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol)].Value = 0;
                                AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 1].Text = "";
                                AttSpread.Sheets[0].Cells[i, Convert.ToInt16(actcol) + 1].Note = "";
                                sfadd = true;
                                AttSpread.SaveChanges();
                                perty = perty - 1;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerr1.Visible = true;
            lblerr1.Text = ex.ToString();
        }
    }
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }

        return null;
    }
    protected void txttolperval_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            AttSpread.Visible = false;
            btnreet1.Visible = false;
            hassstati();
            tolnoremaion.Visible = false;
            btnsavel1.Visible = false;
            lblerr1.Visible = false;
            AttSpread.Visible = false;
        }
        catch
        {
        }
    }
    protected void txtnoofval_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            AttSpread.Visible = false;
            btnreet1.Visible = false;
            hassstati();
            tolnoremaion.Visible = false;
            btnsavel1.Visible = false;
            AttSpread.Visible = false;

        }
        catch
        {
        }
    }
    protected void txtDate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            AttSpread.Visible = false;
            btnreet1.Visible = false;
            hassstati();
            tolnoremaion.Visible = false;
            btnsavel1.Visible = false;
            AttSpread.Visible = false;

        }
        catch
        {
        }
    }
    public void hassstati()
    {
        try
        {
            sfadd1 = false;
            perty = 0;
            rowvl = 0;
            dsscnt1 = 0;
            first1 = false;
            first = false;
            secnd = false;
            secnd1 = false;
        }
        catch
        {
        }
    }
}