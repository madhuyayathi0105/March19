/*
  Code Started by Mohamed Idhris Sheik Dawood on 09-05-2017
*/
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;
using System.Web.UI;
using System.Text;
using InsproDataAccess;
using System.Configuration;

public partial class StudentMod_StudTcIssue : System.Web.UI.Page
{
    int collegeCode = 0;
    int userCode = 0;
    static int choosedmode = 0;
    static int collegecodestat = 13;
    DAccess2 DA = new DAccess2();
    InsproDirectAccess dirAccess = new InsproDirectAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //if (Session["collegecode"] == null)
                //{
                //    Response.Redirect("~/Default.aspx");
                //}
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

                collegeCode = Convert.ToInt32(Convert.ToString(Session["collegecode"]));
                userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
                setLabelText();
                bindCollege();
                updateClgCode();
                bindType();
                bindbatch();
                binddegree();
                bindbranch();

                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");

                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
            }
            lbl_validation.Visible = false;
            updateClgCode();
        }
        catch { Response.Redirect("~/Default.aspx"); }
    }
    //Base Screen 
    public void bindCollege()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Clear();
            ddl_college.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code";
            ds = DA.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex) { ddl_college.Items.Clear(); }
    }
    private void updateClgCode()
    {
        try
        {
            if (ddl_college.Items.Count > 0)
            {
                collegeCode = Convert.ToInt32(ddl_college.SelectedItem.Value);
            }
            else
            {
                collegeCode = 13;
            }
            userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
            if (ddl_college1.Items.Count > 0)
            {
                collegecodestat = Convert.ToInt32(ddl_college1.SelectedItem.Value);
            }
            else
            {
                collegecodestat = 13;
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
        List<byte> fields = new List<byte>();
        lbl.Add(lblCollege);
        fields.Add(0);

        lbl.Add(lbl_stream);
        fields.Add(1);

        lbl.Add(lbl_degree);
        fields.Add(2);

        lbl.Add(lbl_branch);
        fields.Add(3);


        lbl.Add(lblCollege1);
        fields.Add(0);

        lbl.Add(lbl_stream1);
        fields.Add(1);

        lbl.Add(lbl_degree1);
        fields.Add(2);

        lbl.Add(lbl_branch1);
        fields.Add(3);



        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    public void bindType()
    {
        try
        {
            lbl_stream.Text = useStreamShift();
            ddl_strm.Items.Clear();
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>''  and r.college_code='" + collegeCode + "'  order by type asc";

            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
                ddl_strm.Enabled = true;
            }
            else
            {
                ddl_strm.Enabled = false;
            }
        }
        catch (Exception ex) { }
    }
    public void bindbatch()
    {
        try
        {
            ddl_batch.Items.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            DataSet ds = DA.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    public void binddegree()
    {
        try
        {
            //ddl_degree.Items.Clear();
            cbl_degree.Items.Clear();
            txt_degree.Text = lbl_degree.Text;
            cb_degree.Checked = true;
            string stream = "";
            stream = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : "";

            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegeCode + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + userCode + " ";
            if (ddl_strm.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //ddl_degree.DataSource = ds;
                //ddl_degree.DataTextField = "course_name";
                //ddl_degree.DataValueField = "course_id";
                //ddl_degree.DataBind();

                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
            }
        }
        catch { }
    }
    public void bindbranch()
    {
        try
        {
            //ddl_branch.Items.Clear();
            cbl_branch.Items.Clear();
            txt_branch.Text = lbl_branch.Text;
            cb_branch.Checked = true;
            string degree = "";
            degree = GetSelectedItemsValueAsString(cbl_degree);//ddl_degree.Items.Count > 0 ? ddl_degree.SelectedValue : "";


            string commname = "";
            if (degree != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + degree + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }

            DataSet ds = DA.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddl_branch.DataSource = ds;
                //ddl_branch.DataTextField = "dept_name";
                //ddl_branch.DataValueField = "degree_code";
                //ddl_branch.DataBind();
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);
            }
        }
        catch { }
    }
    protected void ddl_college_OnIndexChange(object sender, EventArgs e)
    {
        bindType();
        bindbatch();
        binddegree();
        bindbranch();

        btn_go_Click(sender, e);
    }
    protected void ddl_batch_OnIndexChange(object sender, EventArgs e)
    {
        bindType();
        binddegree();
        bindbranch();

        btn_go_Click(sender, e);
    }
    protected void ddl_strm_OnIndexChange(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();

        btn_go_Click(sender, e);
    }
    protected void ddl_degree_OnIndexChange(object sender, EventArgs e)
    {
        bindbranch();

        btn_go_Click(sender, e);
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();

        btn_go_Click(sender, e);
    }
    protected void cb_degree_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, lbl_degree.Text);
        bindbranch();

        btn_go_Click(sender, e);
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);

        btn_go_Click(sender, e);
    }
    protected void cb_branch_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, lbl_branch.Text);

        btn_go_Click(sender, e);
    }
    protected void ddl_branch_OnIndexChange(object sender, EventArgs e)
    {

        btn_go_Click(sender, e);
    }
    private string useStreamShift()
    {
        string useStrShft = "Stream";
        string streamcode = DA.GetFunction("select value from Master_Settings where settings='Stream/Shift Rights' and usercode='" + userCode + "'").Trim();

        if (streamcode == "" || streamcode == "0")
        {
            useStrShft = "Stream";
        }
        if (streamcode.Trim() == "1")
        {
            useStrShft = "Shift";
        }
        if (streamcode.Trim() == "2")
        {
            useStrShft = "Stream";
        }
        return useStrShft;
    }
    protected void checkDate(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = Convert.ToDateTime(txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2]);
            DateTime todate = Convert.ToDateTime(txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2]);

            if (fromdate <= todate)
            {
            }
            else
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('From Date Should Not Exceed To Date')", true);
            }
        }
        catch { }
        btn_go_Click(sender, e);
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_errormsg.Visible = false;
            Printcontrol.Visible = false;
            rptprint.Visible = false;

            string selectquery;

            string collegeCode = ddl_college.Items.Count > 0 ? ddl_college.SelectedValue : string.Empty;

            string branch = GetSelectedItemsValueAsString(cbl_branch);//ddl_branch.Items.Count > 0 ? ddl_branch.SelectedValue : "";

            string degCode = GetSelectedItemsValueAsString(cbl_degree);//ddl_degree.Items.Count > 0 ? ddl_degree.SelectedValue : "";

            string stream = ddl_strm.Enabled ? ddl_strm.Items.Count > 0 ? ddl_strm.SelectedItem.Text.Trim() : "" : "";

            string batch_year = ddl_batch.Items.Count > 0 ? ddl_batch.SelectedItem.Text : "";

            string dateCheck = string.Empty;

            if (cbDateWise.Checked)
            {
                string[] fromDt = txt_fromdate.Text.Split('/');
                string[] toDt = txt_todate.Text.Split('/');
                dateCheck = " and r.tcdate >='" + (fromDt[1] + "/" + fromDt[0] + "/" + fromDt[2]) + "' and r.tcdate <='" + (toDt[1] + "/" + toDt[0] + "/" + toDt[2]) + "'  ";
            }

            DataSet ds = new DataSet();
            if (collegeCode != string.Empty && batch_year != string.Empty && degCode != string.Empty && branch != string.Empty)
            {
                if (stream != string.Empty)
                {
                    stream = " and c.type in ('" + stream + "')";
                }

                selectquery = "select r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.Reg_No,r.App_No,c.type,isnull(r.Sections,'') as Sections, case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transgender' else 'N/A' end as Gender,r.tcno,Convert(varchar(10),r.tcdate,103) as tcdate    from Registration r,applyn a,Degree d,Department dt,Course c where r.app_no=a.app_no and r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=1 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.Batch_Year =" + batch_year + " and r.degree_code in ('" + branch + "')  " + stream + " and Isnull(r.IsTcIssued,'0')='1'  and r.college_code='" + collegeCode + "'  " + dateCheck;

                selectquery += "  order by d.Degree_Code,isnull(r.Sections,''),ltrim (r.Roll_Admit) asc ";

                ds = DA.select_method_wo_parameter(selectquery, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                spreadStudList.Sheets[0].RowCount = 1;
                spreadStudList.Sheets[0].ColumnCount = 0;
                spreadStudList.Sheets[0].ColumnHeader.RowCount = 1;
                spreadStudList.CommandBar.Visible = false;
                spreadStudList.Sheets[0].ColumnCount = 10;

                spreadStudList.Sheets[0].RowHeader.Visible = false;
                spreadStudList.Sheets[0].AutoPostBack = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                spreadStudList.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[0].Locked = true;
                spreadStudList.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[1].Width = 60;
                spreadStudList.Sheets[0].Columns[1].Locked = false;
                spreadStudList.Sheets[0].Cells[0, 1].CellType = chkall;
                spreadStudList.Sheets[0].Columns[1].Visible = false;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Admission Number";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[2].Locked = true;
                spreadStudList.Columns[2].Width = 150;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[3].Locked = true;
                spreadStudList.Columns[3].Width = 100;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[4].Locked = true;
                spreadStudList.Columns[4].Width = 100;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Smartcard No";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[5].Locked = true;
                spreadStudList.Columns[5].Width = 100;
                spreadStudList.Sheets[0].Columns[5].Visible = false;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Name";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[6].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                spreadStudList.Sheets[0].Columns[6].Locked = true;
                spreadStudList.Columns[6].Width = 300;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].Text = lbl_degree.Text + "/" + lbl_branch.Text;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                spreadStudList.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                spreadStudList.Sheets[0].Columns[7].Locked = true;
                spreadStudList.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                spreadStudList.Columns[7].Width = 300;
                //spreadStudList.Sheets[0].Columns[7].Visible = false;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].Text = "TC No";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[8].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[8].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[8].Locked = true;
                spreadStudList.Sheets[0].Columns[8].Visible = false;
                spreadStudList.Columns[8].Width = 60;

                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].Text = "TC Date";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[9].Font.Name = "Book Antiqua";
                spreadStudList.Sheets[0].Columns[9].Font.Size = FontUnit.Medium;
                spreadStudList.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                spreadStudList.Sheets[0].Columns[9].Locked = true;
                spreadStudList.Columns[9].Width = 100;

                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtSmartno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtTcDateno = new FarPoint.Web.Spread.TextCellType();

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    spreadStudList.Sheets[0].RowCount++;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);

                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 1].CellType = check;

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 2].CellType = txtRollAd;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 3].CellType = txtRollno;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 4].CellType = txtRegno;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 5].CellType = txtSmartno;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["smart_serial_no"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["tcno"]);

                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 9].CellType = txtTcDateno;
                    spreadStudList.Sheets[0].Cells[spreadStudList.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[row]["tcdate"]);
                }
                spreadStudList.Visible = true;
                spreadStudList.Sheets[0].PageSize = spreadStudList.Sheets[0].RowCount;
                spreadStudList.Height = 320;
                spreadStudList.SaveChanges();
                rptprint.Visible = true;

            }
            else
            {
                spreadStudList.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_errormsg.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            spreadStudList.Visible = false;
            lbl_errormsg.Visible = true;
            lbl_errormsg.Text = "No Records Found";
        }
    }
    //Issue Screen
    protected void btn_Issue_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            popwindow.Visible = true;
            bindclg1();
            bindType1();
            bindbatch1();
            binddegree1();
            bindbranch1();

            spreadStudAdd.Visible = false;
            btn_go1_Click(sender, e);
        }
        catch { }
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    public void bindType1()
    {
        try
        {
            lbl_stream1.Text = useStreamShift();
            ddl_strm1.Items.Clear();
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>''  and r.college_code='" + Convert.ToString(ddl_college1.SelectedValue) + "'  order by type asc";

            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm1.DataSource = ds;
                ddl_strm1.DataTextField = "type";
                ddl_strm1.DataValueField = "type";
                ddl_strm1.DataBind();
                ddl_strm1.Enabled = true;
            }
            else
            {
                ddl_strm1.Enabled = false;
            }
        }
        catch (Exception ex) { }
    }
    public void bindbatch1()
    {
        try
        {
            ddl_batch1.Items.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            DataSet ds = DA.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch1.DataSource = ds;
                ddl_batch1.DataTextField = "batch_year";
                ddl_batch1.DataValueField = "batch_year";
                ddl_batch1.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    public void binddegree1()
    {
        try
        {
            ddl_degree1.Items.Clear();
            string stream = "";
            stream = ddl_strm1.Items.Count > 0 ? ddl_strm1.SelectedValue : "";

            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + Convert.ToString(ddl_college1.SelectedValue) + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + userCode + " ";
            if (ddl_strm1.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_degree1.DataSource = ds;
                ddl_degree1.DataTextField = "course_name";
                ddl_degree1.DataValueField = "course_id";
                ddl_degree1.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    public void bindbranch1()
    {
        try
        {
            ddl_branch1.Items.Clear();
            string degree = "";
            degree = ddl_degree1.Items.Count > 0 ? ddl_degree1.SelectedValue : "";


            string commname = "";
            if (degree != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + degree + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }

            DataSet ds = DA.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_branch1.DataSource = ds;
                ddl_branch1.DataTextField = "dept_name";
                ddl_branch1.DataValueField = "degree_code";
                ddl_branch1.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    public void bindclg1()
    {
        try
        {
            ddl_college1.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code";
            DataSet ds = DA.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_college1.DataSource = ds;
                ddl_college1.DataTextField = "collname";
                ddl_college1.DataValueField = "college_code";
                ddl_college1.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    protected void ddl_college1_OnIndexChange(object sender, EventArgs e)
    {
        bindType1();
        bindbatch1();
        binddegree1();
        bindbranch1();

        btn_go1_Click(sender, e);
    }
    protected void ddl_batch1_OnIndexChange(object sender, EventArgs e)
    {
        bindType1();
        binddegree1();
        bindbranch1();

        btn_go1_Click(sender, e);
    }
    protected void ddl_strm1_OnIndexChange(object sender, EventArgs e)
    {
        binddegree1();
        bindbranch1();

        btn_go1_Click(sender, e);
    }
    protected void ddl_degree1_OnIndexChange(object sender, EventArgs e)
    {
        bindbranch1();

        btn_go1_Click(sender, e);
    }
    protected void ddl_branch1_OnIndexChange(object sender, EventArgs e)
    {

        btn_go1_Click(sender, e);
    }
    protected void ddl_searchBy_OnIndexChange(object sender, EventArgs e)
    {
        txt_SearchBy.Text = string.Empty;
        if (ddl_searchBy.SelectedIndex == 0)
        {
            txt_SearchBy.Attributes.Add("placeholder", "Adm No");
            choosedmode = 0;
        }
        else if (ddl_searchBy.SelectedIndex == 1)
        {
            txt_SearchBy.Attributes.Add("placeholder", "Student Name");
            choosedmode = 1;
        }
        else if (ddl_searchBy.SelectedIndex == 2)
        {
            txt_SearchBy.Attributes.Add("placeholder", "Roll No");
            choosedmode = 2;
        }
        btn_go1_Click(sender, e);
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetSearch(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            if (choosedmode == 0)
            {
                query = "select top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat + " order by Roll_No asc";
            }
            else if (choosedmode == 1)
            {
                query = "select  top 100 stud_name from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and stud_name like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Reg_No asc";
            }
            else if (choosedmode == 2)
            {
                query = "select  top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Roll_admit asc";
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    protected void btn_go1_Click(object sender, EventArgs e)
    {
        retrieveSearch();
    }
    private void retrieveSearch()
    {
        try
        {
            btnIsssueTC.Visible = false;

            lbl_errormsg1.Visible = false;

            string selectquery;

            string collegeCode = ddl_college1.Items.Count > 0 ? ddl_college1.SelectedValue : "";

            string branch = ddl_branch1.Items.Count > 0 ? ddl_branch1.SelectedValue : "";

            string degCode = ddl_degree1.Items.Count > 0 ? ddl_degree1.SelectedValue : "";

            string stream = ddl_strm1.Enabled ? ddl_strm1.Items.Count > 0 ? ddl_strm1.SelectedItem.Text.Trim() : "" : "";

            string batch_year = ddl_batch1.Items.Count > 0 ? ddl_batch1.SelectedItem.Text : "";

            DataSet ds = new DataSet();
            string searchBytxt = txt_SearchBy.Text.Trim();
            if (searchBytxt != string.Empty)
            {
                if(rdb_general.Checked)
                {
                selectquery = "select r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.Reg_No,r.App_No,c.type,isnull(r.Sections,'') as Sections, case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transgender' else 'N/A' end as Gender,tcno,tcdate  from Registration r,applyn a,Degree d,Department dt,Course c where r.app_no=a.app_no and r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and isnull(r.IsTcIssued,'0')='0' and r.college_code='" + collegeCode + "'  ";

                if (ddl_searchBy.SelectedIndex == 0)
                {
                    selectquery += " and r.roll_admit='" + searchBytxt + "'";
                }
                else if (ddl_searchBy.SelectedIndex == 1)
                {
                    selectquery += " and r.stud_name='" + searchBytxt + "'";
                }
                else if (ddl_searchBy.SelectedIndex == 2)
                {
                    selectquery += " and r.roll_no='" + searchBytxt + "'";
                }
                ds = DA.select_method_wo_parameter(selectquery, "Text");
                }
                if (rdb_Request.Checked)
                {

                    selectquery = "select r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.Reg_No,r.App_No,c.type,isnull(r.Sections,'') as Sections, case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transgender' else 'N/A' end as Gender,tcno,tcdate  from Registration r,applyn a,Degree d,Department dt,Course c,RQ_Requisition rq where r.app_no=a.app_no and r.App_No=rq.ReqAppNo  and r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and isnull(r.IsTcIssued,'0')='0' and r.college_code='" + collegeCode + "' and RequestType='11' and ReqAppStatus=1 ";

                if (ddl_searchBy.SelectedIndex == 0)
                {
                    selectquery += " and r.roll_admit='" + searchBytxt + "'";
                }
                else if (ddl_searchBy.SelectedIndex == 1)
                {
                    selectquery += " and r.stud_name='" + searchBytxt + "'";
                }
                else if (ddl_searchBy.SelectedIndex == 2)
                {
                    selectquery += " and r.roll_no='" + searchBytxt + "'";
                }
                ds = DA.select_method_wo_parameter(selectquery, "Text");
                }
            }
            else
                if (batch_year != string.Empty && degCode != string.Empty && branch != string.Empty && collegeCode != string.Empty)
                {
                    if (rdb_general.Checked)
                    {
                        if (stream != string.Empty)
                        {
                            stream = " and c.type in ('" + stream + "')";
                        }

                        selectquery = "select r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.Reg_No,r.App_No,c.type,isnull(r.Sections,'') as Sections, case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transgender' else 'N/A' end as Gender,tcno,tcdate  from Registration r,applyn a,Degree d,Department dt,Course c where r.app_no=a.app_no and r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.Batch_Year =" + batch_year + " and r.degree_code in ('" + branch + "')   and isnull(r.IsTcIssued,'0')='0' " + stream + "  and r.college_code='" + collegeCode + "'  ";

                        selectquery += "  order by d.Degree_Code,isnull(r.Sections,''),ltrim (r.Roll_Admit) asc ";
                        ds = DA.select_method_wo_parameter(selectquery, "Text");
                    }
                    if (rdb_Request.Checked)
                    {

                        if (stream != string.Empty)
                        {
                            stream = " and c.type in ('" + stream + "')";
                        }

                        selectquery = "select r.Roll_No,r.Roll_Admit,r.smart_serial_no,r.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.Reg_No,r.App_No,c.type,isnull(r.Sections,'') as Sections, case when a.sex=0 then 'Male' when a.sex=1 then 'Female' when a.sex=2 then 'Transgender' else 'N/A' end as Gender,tcno,tcdate  from Registration r,applyn a,Degree d,Department dt,Course c ,RQ_Requisition rq where r.app_no=a.app_no and r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and r.App_No=rq.ReqAppNo and RequestType='11' and ReqAppStatus=1 and Exam_Flag <>'DEBAR' and r.Batch_Year =" + batch_year + " and r.degree_code in ('" + branch + "')   and isnull(r.IsTcIssued,'0')='0' " + stream + "  and r.college_code='" + collegeCode + "'  ";

                        selectquery += "  order by d.Degree_Code,isnull(r.Sections,''),ltrim (r.Roll_Admit) asc ";
                        ds = DA.select_method_wo_parameter(selectquery, "Text");
                    }


                }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                spreadStudAdd.Sheets[0].RowCount = 1;
                spreadStudAdd.Sheets[0].ColumnCount = 0;
                spreadStudAdd.Sheets[0].ColumnHeader.RowCount = 1;
                spreadStudAdd.CommandBar.Visible = false;
                spreadStudAdd.Sheets[0].ColumnCount = 8;

                spreadStudAdd.Sheets[0].RowHeader.Visible = false;
                spreadStudAdd.Sheets[0].AutoPostBack = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                spreadStudAdd.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[0].Locked = true;
                spreadStudAdd.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[1].Width = 60;
                spreadStudAdd.Sheets[0].Columns[1].Locked = false;
                spreadStudAdd.Sheets[0].Cells[0, 1].CellType = chkall;
                spreadStudAdd.Sheets[0].Columns[1].Visible = true;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Admission Number";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[2].Locked = true;
                spreadStudAdd.Columns[2].Width = 150;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[3].Locked = true;
                spreadStudAdd.Columns[3].Width = 100;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[4].Locked = true;
                spreadStudAdd.Columns[4].Width = 100;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Smartcard No";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[5].Locked = true;
                spreadStudAdd.Columns[5].Width = 100;
                spreadStudAdd.Sheets[0].Columns[5].Visible = false;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Name";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[6].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                spreadStudAdd.Sheets[0].Columns[6].Locked = true;
                spreadStudAdd.Columns[6].Width = 300;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].Text = lbl_degree1.Text + "/" + lbl_branch1.Text;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                spreadStudAdd.Sheets[0].Columns[7].Locked = true;
                spreadStudAdd.Sheets[0].Columns[7].Visible = true;

                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtSmartno = new FarPoint.Web.Spread.TextCellType();


                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    spreadStudAdd.Sheets[0].RowCount++;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);

                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 1].CellType = check;

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 2].CellType = txtRollAd;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 3].CellType = txtRollno;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 4].CellType = txtRegno;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 5].CellType = txtSmartno;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["smart_serial_no"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);

                }
                spreadStudAdd.Visible = true;
                spreadStudAdd.Sheets[0].PageSize = spreadStudAdd.Sheets[0].RowCount;

                spreadStudAdd.Height = 350;
                spreadStudAdd.SaveChanges();

                btnIsssueTC.Visible = true;
            }
            else
            {
                spreadStudAdd.Visible = false;
                lbl_errormsg1.Visible = true;
                lbl_errormsg1.Text = "No Records Found";
            }
        }
        catch
        {
            spreadStudAdd.Visible = false;
            lbl_errormsg1.Visible = true;
            lbl_errormsg1.Text = "No Records Found";
        }
    }
    //Issue TC
    protected void spreadStudAdd_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = spreadStudAdd.Sheets[0].ActiveRow.ToString();
            string actcol = spreadStudAdd.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (spreadStudAdd.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(spreadStudAdd.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < spreadStudAdd.Sheets[0].RowCount; i++)
                        {
                            spreadStudAdd.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < spreadStudAdd.Sheets[0].RowCount; i++)
                        {
                            spreadStudAdd.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void btnIsssueTC_Click(object sender, EventArgs e)
    {
        try
        {
            List<string> listHdrsLedgers = getTcHeaderLedgers();

            spreadStudAdd.SaveChanges();
            bool isSelected = false;
            int updateVal = 0;
            int feeNotClear = 0;
            for (int spreadCnt = 1; spreadCnt < spreadStudAdd.Sheets[0].RowCount; spreadCnt++)
            {
                int checkval = Convert.ToInt32(spreadStudAdd.Sheets[0].Cells[spreadCnt, 1].Value);
                if (checkval == 1)
                {
                    isSelected = true;
                    string appNo = Convert.ToString(spreadStudAdd.Sheets[0].Cells[spreadCnt, 0].Tag).Trim();
                    bool feesClear = false;

                    if (listHdrsLedgers.Count == 2)
                    {
                        feesClear = checkFeesClearance(appNo, listHdrsLedgers);
                    }
                    else
                    {
                        feesClear = true;
                    }

                    if (feesClear)
                    {
                        updateVal += dirAccess.updateData("update registration set IsTcIssued='1' , tcno ='' , tcdate = '" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appNo + "' ");
                    }
                    else
                    {
                        feeNotClear++;
                    }
                }
            }
            if (!isSelected)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Students')", true);
            }
            else if (updateVal > 0)
            {
                btn_go1_Click(sender, e);

                if (feeNotClear > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + updateVal + " TC Issued Successfully and " + feeNotClear + " TC Not Issued')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('TC Issued Successfully')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('TC Not Issued')", true);
            }
        }
        catch { ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('TC Not Issued')", true); }
    }
    private bool checkFeesClearance(string appNo, List<string> listHdrsLedgers)
    {
        bool IsCleared = false;
        try
        {
            int notPaidCnt = dirAccess.selectScalarInt("select COUNT(app_no) as notpaid from FT_FeeAllot where App_No = '" + appNo + "' and HeaderFK in(" + listHdrsLedgers[0] + ") and LedgerFK in (" + listHdrsLedgers[1] + ") and (ISNULL(totalamount,0)-ISNULL(paidamount,0))>0");
            if (notPaidCnt == 0)
            {
                IsCleared = true;
            }
        }
        catch { IsCleared = false; }
        return IsCleared;
    }
    private List<string> getTcHeaderLedgers()
    {
        List<string> listHeadersLedgers = new List<string>();
        try
        {
            string savedHdrLedgers = dirAccess.selectScalarString("select LinkValue from New_InsSettings where LinkName='TCLedgersForUser' and college_code ='" + ddl_college1.SelectedValue + "' and user_code='" + userCode + "'");
            if (!string.IsNullOrEmpty(savedHdrLedgers))
            {
                string[] hdrlgr = savedHdrLedgers.Split('#');
                foreach (string res in hdrlgr)
                {
                    listHeadersLedgers.Add(res);
                }
            }
        }
        catch { listHeadersLedgers = new List<string>(); }
        return listHeadersLedgers;
    }
    //Common Print
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Student TC Issue Report";
            string pagename = "StudTcIssue.aspx";
            Printcontrol.loadspreaddetails(spreadStudList, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                DA.printexcelreport(spreadStudList, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch (Exception ex) { }

    }
    //Common Functions
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch (Exception ex) { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
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
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private List<string> GetSelectedItemsValueList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Value);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }
    private List<string> GetSelectedItemsTextList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Text);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }
    private List<string> GetItemsValueList(CheckBoxList cblItems)
    {
        System.Collections.Generic.List<string> lsItems = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblItems.Items.Count; list++)
            {
                lsItems.Add(cblItems.Items[list].Value);
            }
        }
        catch { lsItems.Clear(); }
        return lsItems;
    }
    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }
}