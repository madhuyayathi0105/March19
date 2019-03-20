using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;

public partial class mark_import : System.Web.UI.Page
{
    public SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    string collegecode, usercode, singleuser, group_user;
    DAccess2 obi_access = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
    Hashtable hat = new Hashtable();
    static DataSet ds_grade = new DataSet();
    static DataTable dt_grade = new DataTable();
    bool flag_true = false;

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
            usercode = Session["UserCode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                Fp_Marks.Sheets[0].RowCount = 0;
                Fp_Marks.Visible = false;
                Fp_Marks.CommandBar.Visible = false;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                Fp_Marks.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                Fp_Marks.Sheets[0].AllowTableCorner = true;
                Fp_Marks.Sheets[0].RowHeader.Visible = false;
                Fp_Marks.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fp_Marks.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                Fp_Marks.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                Fp_Marks.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fp_Marks.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                Fp_Marks.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                Fp_Marks.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                Fp_Marks.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                Fp_Marks.Sheets[0].DefaultStyle.Font.Bold = false;
                Fp_Marks.SheetCorner.Cells[0, 0].Font.Bold = true;
                Fp_Marks.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.CommandBar.Visible = false;
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 10;
                style1.Font.Bold = true;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
                FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread2.Sheets[0].AutoPostBack = true;
                FpSpread2.CommandBar.Visible = false;
                style1.Font.Size = 10;
                style1.Font.Bold = true;
                FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread2.Sheets[0].AllowTableCorner = true;
                FpSpread2.Sheets[0].RowHeader.Visible = false;
                FpSpread2.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
                FpSpread2.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                Btn_save.Visible = false;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].RowCount = 0;

                bindbatch();
                BindDegree();
                bindbranch();
                Bind_Exam_MonthYear();
                ddl_operation_SelectedIndexChanged(sender, e);
                string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                DataSet ds = obi_access.select_method_wo_parameter(Master1, "text");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbatch()
    {
        ddl_batch.Items.Clear();
        //ds = obi_access.select_method_wo_parameter("bind_batch", "sp");
        ds.Clear();
        ds = obi_access.select_method_wo_parameter("select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar' order by batch_year desc ; select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar'", "text");
        int count = 0;// ds.Tables[0].Rows.Count;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddl_batch.DataSource = ds.Tables[0];
            ddl_batch.DataTextField = "batch_year";
            ddl_batch.DataValueField = "batch_year";
            ddl_batch.DataBind();
        }
        //int count1 = ds.Tables[1].Rows.Count;
        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            int max_bat = 0;
            int.TryParse(Convert.ToString(ds.Tables[1].Rows[0][0]).Trim(), out max_bat);
            ddl_batch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }

    public void BindDegree()
    {
        ddl_degree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
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
        ds = obi_access.select_method("bind_degree", hat, "sp");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddl_degree.DataSource = ds;
            ddl_degree.DataTextField = "course_name";
            ddl_degree.DataValueField = "course_id";
            ddl_degree.DataBind();
        }
    }

    public void bindbranch()
    {
        ddl_dept.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddl_degree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = obi_access.select_method("bind_branch", hat, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddl_dept.DataSource = ds;
            ddl_dept.DataTextField = "dept_name";
            ddl_dept.DataValueField = "degree_code";
            ddl_dept.DataBind();
        }
    }

    void Bind_Exam_MonthYear()
    {
        con.Close();
        con.Open();
        SqlCommand cmd_get_month = new SqlCommand("Select distinct Exam_Month from Exam_Details where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "'", con);
        SqlDataAdapter ad_get_month = new SqlDataAdapter(cmd_get_month);
        DataTable dt_get_month = new DataTable();
        ad_get_month.Fill(dt_get_month);
        SqlCommand cmd_get_year = new SqlCommand("Select distinct Exam_Year from Exam_Details where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "'", con);
        SqlDataAdapter ad_get_year = new SqlDataAdapter(cmd_get_year);
        DataTable dt_get_year = new DataTable();
        ad_get_year.Fill(dt_get_year);
        ddl_exmonth.Items.Clear();
        ddl_exyear.Items.Clear();
        if (dt_get_year.Rows.Count > 0)
        {
            ddl_exyear.DataSource = dt_get_year;
            ddl_exyear.DataTextField = "Exam_Year";
            ddl_exyear.DataValueField = "Exam_Year";
            ddl_exyear.DataBind();
        }
        if (dt_get_month.Rows.Count > 0)
        {
            DataTable dt = new DataTable();
            DataColumn dc;
            dc = new DataColumn();
            dc.ColumnName = "Month";
            dt.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "Month_Name";
            dt.Columns.Add(dc);
            DataRow dr;
            for (int i = 0; i < dt_get_month.Rows.Count; i++)
            {
                string month = dt_get_month.Rows[i]["Exam_Month"].ToString();
                string month_name = getmonth(month);
                dr = dt.NewRow();
                dr["Month"] = month;
                dr["Month_Name"] = month_name;
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                ddl_exmonth.DataSource = dt;
                ddl_exmonth.DataTextField = "Month_Name";
                ddl_exmonth.DataValueField = "Month";
                ddl_exmonth.DataBind();
            }
            load_subject();
        }
    }

    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind_Exam_MonthYear();
    }

    protected void ddl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        Bind_Exam_MonthYear();
    }

    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind_Exam_MonthYear();
    }

    protected void ddl_exmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_subject();
    }

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        Panel3.Visible = true;
    }

    private void load_subject()
    {
        string exam_code = "0";
        string cur_sem = "0";
        FarPoint.Web.Spread.LabelCellType chkcell0 = new FarPoint.Web.Spread.LabelCellType();
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].Columns[1].CellType = chkcell0;
        FpSpread1.Sheets[0].ColumnCount = 2;
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Headers";
        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].ColumnCount = 2;
        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Headers";
        FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        con.Close();
        con.Open();
        SqlCommand cmd_getsum = new SqlCommand("Select Exam_Code,current_semester from Exam_Details where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "' and Exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedValue.ToString() + "'", con);
        SqlDataAdapter ad_getsum = new SqlDataAdapter(cmd_getsum);
        DataTable dt_getsum = new DataTable();
        ad_getsum.Fill(dt_getsum);
        if (dt_getsum.Rows.Count > 0)
        {
            exam_code = dt_getsum.Rows[0]["Exam_Code"].ToString();
            cur_sem = dt_getsum.Rows[0]["current_semester"].ToString();
        }
        con.Close();
        con.Open();
        string qry = "select distinct ead.subject_no,s.subject_code,sm.semester,ISNULL(s.subjectpriority,'0') subjectpriority from exam_application ea,exam_appl_details ead,subject s,syllabus_master sm  where ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and sm.syll_code=s.syll_code and exam_code in(Select Exam_Code from Exam_Details where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "' and Exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedValue.ToString() + "') order by sm.semester desc,subjectpriority,s.subject_code";
        //select distinct exam_appl_details.subject_no,subject_code  from exam_application,exam_appl_details,subject where exam_application.appl_no=exam_appl_details.appl_no and exam_appl_details.subject_no=subject.subject_no and exam_code='" + exam_code + "'
        SqlCommand cmd_get_subject = new SqlCommand(qry, con);
        SqlDataAdapter ad_get_subject = new SqlDataAdapter(cmd_get_subject);
        DataTable dt_get_subject = new DataTable();
        ad_get_subject.Fill(dt_get_subject);
        if (dt_get_subject.Rows.Count > 0)
        {
            int sno = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            for (int i = 0; i < dt_get_subject.Rows.Count; i++)
            {
                sno++;
                FpSpread1.Sheets[0].RowCount = Convert.ToInt32(FpSpread1.Sheets[0].RowCount) + 1;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dt_get_subject.Rows[i]["subject_code"].ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dt_get_subject.Rows[i]["subject_no"].ToString();
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
    }

    protected void ddl_exyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_subject();
    }

    protected void ddl_operation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_operation.SelectedItem.ToString() == "Import")
        {
            lbl_choose.Visible = true;
            File_Upload.Visible = true;
            LinkButton3.Visible = true;
        }
        else
        {
            lbl_choose.Visible = false;
            File_Upload.Visible = false;
            LinkButton3.Visible = false;
        }
        Fp_Marks.Visible = false;
        Fp_Grade.Visible = false;
        Btn_save.Visible = false;
        Btn_Delete.Visible = false;
    }

    public string getmonth(string mname)
    {
        string month = string.Empty;
        if (Convert.ToInt32(mname) == 1)
        {
            month = "January";
        }
        else if (Convert.ToInt32(mname) == 2)
        {
            month = "February";
        }
        else if (Convert.ToInt32(mname) == 3)
        {
            month = "March";
        }
        else if (Convert.ToInt32(mname) == 4)
        {
            month = "April";
        }
        else if (Convert.ToInt32(mname) == 5)
        {
            month = "May";
        }
        else if (Convert.ToInt32(mname) == 6)
        {
            month = "June";
        }
        else if (Convert.ToInt32(mname) == 7)
        {
            month = "July";
        }
        else if (Convert.ToInt32(mname) == 8)
        {
            month = "August";
        }
        else if (Convert.ToInt32(mname) == 9)
        {
            month = "September";
        }
        else if (Convert.ToInt32(mname) == 10)
        {
            month = "October";
        }
        else if (Convert.ToInt32(mname) == 11)
        {
            month = "November";
        }
        else if (Convert.ToInt32(mname) == 12)
        {
            month = "December";
        }
        return month;
    }

    protected void Btn_go_Click(object sender, EventArgs e)
    {
        Fp_Marks.Visible = false;
        Fp_Grade.Visible = false;
        if (ddl_exmonth.Text.ToString().Trim() == "")
        {
            Btn_save.Visible = false;
            lbl_msg.Visible = true;
            lbl_msg.Text = "Please Select Exam Month and  then proceed.";
            return;
        }
        if (ddl_exyear.Text.ToString().Trim() == "")
        {
            Btn_save.Visible = false;
            lbl_msg.Visible = true;
            lbl_msg.Text = "Please Select Exam Year and  then proceed.";
            return;
        }
        if (ddl_operation.SelectedItem.ToString() == "Import")
        {
            if (File_Upload.FileName != "" && File_Upload.FileName != null)
            {
                lbl_msg.Visible = false;
                using (Stream stream = this.File_Upload.FileContent as Stream)
                {
                    stream.Position = 0;
                    this.Fp_Marks.OpenExcel(stream);
                    Fp_Marks.OpenExcel(stream);
                    Fp_Marks.SaveChanges();
                }
                Btn_save.Visible = true;
                Fp_Marks.ColumnHeader.Visible = false;
                Fp_Marks.Sheets[0].RowHeader.Visible = false;
                Fp_Marks.Sheets[0].PageSize = Fp_Marks.Sheets[0].RowCount;
                Fp_Marks.Visible = true;
                Fp_Marks.CommandBar.Visible = false;
                Fp_Marks.Sheets[0].AutoPostBack = true;
                Fp_Marks.SaveChanges();
            }
            else
            {
                Fp_Marks.Visible = false;
                Btn_save.Visible = false;
                lbl_msg.Visible = true;
                lbl_msg.Text = "Please Select any Excel file then proceed.";
                return;
            }
        }
        else if (ddl_operation.SelectedItem.ToString() == "Entry")
        {
            if (ddl_type.SelectedItem.Text == "Mark")
            {
                Goclick(Fp_Marks);
            }
            else if (ddl_type.SelectedItem.Text == "Grade")
            {
                Goclick(Fp_Grade);
            }
        }
    }

    private void Goclick(FpSpread objspread)
    {
        objspread.Sheets[0].RowCount = 0;
        objspread.Visible = false;
        objspread.CommandBar.Visible = false;
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        objspread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        objspread.Sheets[0].AllowTableCorner = true;
        objspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        objspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        objspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        objspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        objspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        objspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        objspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        objspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        objspread.Sheets[0].DefaultStyle.Font.Bold = false;
        objspread.SheetCorner.Cells[0, 0].Font.Bold = true;
        objspread.Sheets[0].RowHeader.Visible = false;
        objspread.ColumnHeader.Visible = true;
        objspread.Sheets[0].AutoPostBack = false;
        if (ddl_operation.SelectedItem.ToString() == "Entry")
        {
            string[] strcomb;
            FarPoint.Web.Spread.ComboBoxCellType objcombo = new FarPoint.Web.Spread.ComboBoxCellType();
            strcomb = new string[] { "Fail", "Pass" };
            objcombo = new FarPoint.Web.Spread.ComboBoxCellType(strcomb);
            //objcombo.ShowButton = true;
            objcombo.AutoPostBack = true;
            objcombo.UseValue = true;
            FarPoint.Web.Spread.TextCellType objtext = new FarPoint.Web.Spread.TextCellType();
            objspread.Sheets[0].RowCount = 0;
            objspread.Sheets[0].ColumnCount = 0;
            objspread.Sheets[0].ColumnHeader.RowCount = 2;
            objspread.Sheets[0].ColumnCount = 4;
            objspread.Sheets[0].ColumnHeaderSpanModel.Add((objspread.Sheets[0].ColumnHeader.RowCount - 2), 0, 2, 1);
            objspread.Sheets[0].ColumnHeader.Cells[(objspread.Sheets[0].ColumnHeader.RowCount - 2), 0].Text = "S.No";
            objspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
            objspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            objspread.Sheets[0].ColumnHeader.Columns[0].CellType = objtext;
            objspread.Sheets[0].ColumnHeaderSpanModel.Add((objspread.Sheets[0].ColumnHeader.RowCount - 2), 1, 2, 1);
            objspread.Sheets[0].ColumnHeader.Cells[(objspread.Sheets[0].ColumnHeader.RowCount - 2), 1].Text = "Roll No";
            objspread.Sheets[0].ColumnHeader.Columns[1].Width = 100;
            objspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            objspread.Sheets[0].ColumnHeader.Columns[1].CellType = objtext;
            objspread.Sheets[0].ColumnHeaderSpanModel.Add((objspread.Sheets[0].ColumnHeader.RowCount - 2), 2, 2, 1);
            objspread.Sheets[0].ColumnHeader.Cells[(objspread.Sheets[0].ColumnHeader.RowCount - 2), 2].Text = "Reg No";
            objspread.Sheets[0].ColumnHeader.Columns[2].Width = 100;
            objspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            objspread.Sheets[0].ColumnHeader.Columns[2].CellType = objtext;
            //if (Session["Rollflag"] == "1")
            //{
            //    objspread.Sheets[0].Columns[1].Visible = true;
            //}
            //else
            //{
            //    objspread.Sheets[0].Columns[1].Visible = false;
            //}
            //if (Session["Regflag"] == "1")
            //{
            //    objspread.Sheets[0].Columns[2].Visible = true;
            //}
            //else
            //{
            //    objspread.Sheets[0].Columns[2].Visible = false;
            //}
            objspread.Sheets[0].ColumnHeaderSpanModel.Add((objspread.Sheets[0].ColumnHeader.RowCount - 2), 3, 2, 1);
            objspread.Sheets[0].ColumnHeader.Cells[(objspread.Sheets[0].ColumnHeader.RowCount - 2), 3].Text = "Student Name";
            objspread.Sheets[0].ColumnHeader.Columns[3].Width = 200;
            objspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            objspread.Sheets[0].ColumnHeader.Columns[3].CellType = objtext;
            con.Close();
            con.Open();
            SqlCommand cmd_getsum = new SqlCommand("Select Exam_Code,current_semester from Exam_Details where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "' and Exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedValue.ToString() + "'", con);
            SqlDataAdapter ad_getsum = new SqlDataAdapter(cmd_getsum);
            DataTable dt_getsum = new DataTable();
            ad_getsum.Fill(dt_getsum);
            string exam_code = "", cur_sem = string.Empty;
            if (dt_getsum.Rows.Count > 0)
            {
                exam_code = dt_getsum.Rows[0]["Exam_Code"].ToString();
                cur_sem = dt_getsum.Rows[0]["current_semester"].ToString();
            }
            if (exam_code.Trim().ToString() != "")
            {
                con.Close();
                con.Open();
                SqlCommand cmd_get_subject = new SqlCommand("select distinct exam_appl_details.subject_no,subject_code,sy.semester,max_ext_marks,max_int_marks,maxtotal,min_ext_marks,min_int_marks,mintotal,credit_points  from exam_application,exam_appl_details,subject,syllabus_master sy where exam_application.appl_no=exam_appl_details.appl_no and exam_appl_details.subject_no=subject.subject_no and sy.syll_code=subject.syll_code and exam_code='" + exam_code + "'", con);
                SqlDataAdapter ad_get_subject = new SqlDataAdapter(cmd_get_subject);
                DataSet dt_get_subject = new DataSet();
                ad_get_subject.Fill(dt_get_subject);
                con.Close();
                con.Open();
                SqlCommand cmd_subj = new SqlCommand("select distinct ea.roll_no,subject_no,r.stud_name,r.reg_no,e.Attempts from exam_appl_details e,exam_application ea,registration r where e.appl_no=ea.appl_no and r.roll_no=ea.roll_no and ea.exam_code=" + exam_code + "", con);
                SqlDataAdapter ad_subj = new SqlDataAdapter(cmd_subj);
                DataTable dt_subj = new DataTable();
                ad_subj.Fill(dt_subj);
                con.Close();
                con.Open();
                SqlCommand cmd_mark = new SqlCommand("select * from mark_entry where exam_code=" + exam_code + "", con);
                SqlDataAdapter ad_mark = new SqlDataAdapter(cmd_mark);
                DataTable dt_mark = new DataTable();
                ad_mark.Fill(dt_mark);
                con.Close();
                con.Open();
                ds_grade.Clear();
                ds_grade.Dispose();
                ds_grade.Reset();
                dt_grade.Clear();
                dt_grade.Dispose();
                dt_grade.Reset();
                SqlCommand cmd_grade = new SqlCommand("select * from grade_master where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "' and college_code=" + Session["collegecode"].ToString() + " and semester=0 and ltrim(mark_grade)<>''", con);
                SqlDataAdapter ad_grade = new SqlDataAdapter(cmd_grade);
                ad_grade.Fill(ds_grade);
                ad_grade.Fill(dt_grade);
                con.Close();
                con.Open();
                SqlCommand cmd_exam_details = new SqlCommand("select distinct ea.roll_no,r.stud_name,r.reg_no from exam_appl_details e,exam_application ea,registration r where e.appl_no=ea.appl_no and r.roll_no=ea.roll_no and ea.exam_code=" + exam_code + "", con);
                SqlDataAdapter ad_exam_details = new SqlDataAdapter(cmd_exam_details);
                DataSet dt_exam_details = new DataSet();
                ad_exam_details.Fill(dt_exam_details);
                if (ddl_type.SelectedItem.Text == "Mark")
                {
                    if (dt_get_subject.Tables[0].Rows.Count > 0)
                    {
                        for (int sub = 0; sub < dt_get_subject.Tables[0].Rows.Count; sub++)//Load Header
                        {
                            int spcont = objspread.Sheets[0].ColumnCount;
                            objspread.Sheets[0].ColumnCount = objspread.Sheets[0].ColumnCount + 4;
                            objspread.Sheets[0].ColumnHeaderSpanModel.Add(0, spcont, 1, 4);
                            objspread.Sheets[0].ColumnHeader.Cells[0, spcont].Text = dt_get_subject.Tables[0].Rows[sub]["subject_code"].ToString() + "-Sem[" + dt_get_subject.Tables[0].Rows[sub]["semester"].ToString() + "]";
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 4].Text = "I";
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 4].Note = dt_get_subject.Tables[0].Rows[sub]["subject_no"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["credit_points"].ToString();
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 4].Tag = dt_get_subject.Tables[0].Rows[sub]["max_int_marks"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["min_int_marks"].ToString();
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 3].Text = "E";
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 3].Note = dt_get_subject.Tables[0].Rows[sub]["subject_no"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["credit_points"].ToString();
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 3].Tag = dt_get_subject.Tables[0].Rows[sub]["max_ext_marks"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["min_ext_marks"].ToString(); ;
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 2].Text = "T";
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 2].Note = dt_get_subject.Tables[0].Rows[sub]["subject_no"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["credit_points"].ToString();
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 2].Tag = dt_get_subject.Tables[0].Rows[sub]["maxtotal"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["mintotal"].ToString();
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 1].Text = "R";
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 1].Note = dt_get_subject.Tables[0].Rows[sub]["subject_no"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["credit_points"].ToString();
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 1].Tag = dt_get_subject.Tables[0].Rows[sub]["min_int_marks"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["min_ext_marks"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["mintotal"].ToString();
                            objspread.ActiveSheetView.Columns[objspread.Sheets[0].ColumnCount - 1].CellType = objcombo;
                        }
                        objspread.SaveChanges();
                        if (dt_exam_details.Tables[0].Rows.Count > 0)
                        {
                            for (int stud = 0; stud < dt_exam_details.Tables[0].Rows.Count; stud++)
                            {
                                objspread.Sheets[0].RowCount++;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 0].Text = objspread.Sheets[0].RowCount.ToString();
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 0].Locked = true;
                                objspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["roll_no"]);
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["roll_no"]);
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 1].CellType = objtext;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 1].Locked = true;
                                objspread.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["reg_no"]);
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 2].CellType = objtext;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 2].Locked = true;
                                objspread.Sheets[0].ColumnHeader.Columns[2].Width = 100;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["stud_name"]);
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 3].CellType = objtext;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 3].Locked = true;
                                objspread.Sheets[0].ColumnHeader.Columns[3].Width = 200;
                                for (int colcnt = 4; colcnt < objspread.Sheets[0].ColumnCount; colcnt++)
                                {
                                    string[] splitsub = Convert.ToString(objspread.Sheets[0].ColumnHeader.Cells[1, colcnt].Note).Split(new Char[] { '-' });
                                    string subject_no = Convert.ToString(splitsub[0]);
                                    string minimummarks = Convert.ToString(objspread.Sheets[0].ColumnHeader.Cells[1, colcnt].Tag);
                                    string[] maxmark = minimummarks.Split(new Char[] { '-' });
                                    string max_mark = maxmark[0].ToString();
                                    string attemps = string.Empty;
                                    DataView dv_subj = new DataView();
                                    dt_subj.DefaultView.RowFilter = "subject_no='" + subject_no + "' and roll_no='" + Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["roll_no"]) + "'";
                                    dv_subj = dt_subj.DefaultView;
                                    objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Center;
                                    FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
                                    intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
                                    intgrcel.MaximumValue = Convert.ToInt32(max_mark.ToString());
                                    intgrcel.MinimumValue = -1;
                                    intgrcel.ErrorMessage = "Masrk should be <=" + max_mark.ToString();
                                    string head = Convert.ToString(objspread.Sheets[0].ColumnHeader.Cells[1, colcnt].Text);
                                    if (dv_subj.Count == 0)
                                    {
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Text = "--";
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Locked = true;
                                    }
                                    else
                                    {
                                        DataView dv_mark = new DataView();
                                        dt_mark.DefaultView.RowFilter = "exam_code='" + exam_code + "' and subject_no='" + subject_no + "' and roll_no='" + Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["roll_no"]) + "'";
                                        dv_mark = dt_mark.DefaultView;
                                        if (dv_mark.Count > 0)
                                        {
                                            string intmark = Convert.ToString(dv_mark[0]["internal_mark"]);
                                            string extmark = Convert.ToString(dv_mark[0]["external_mark"]);
                                            string totmark = Convert.ToString(dv_mark[0]["total"]);
                                            string result = Convert.ToString(dv_mark[0]["result"]);
                                            attemps = Convert.ToString(dv_subj[0]["attempts"]);
                                            if (head.Trim().ToString() != "")
                                            {
                                                if (head.Trim().ToString() == "I")
                                                {
                                                    objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Text = Convert.ToString(intmark);
                                                }
                                                else if (head.Trim().ToString() == "E")
                                                {
                                                    objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Text = Convert.ToString(extmark);
                                                }
                                                else if (head.Trim().ToString() == "T")
                                                {
                                                    objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Text = Convert.ToString(totmark);
                                                }
                                                else if (head.Trim().ToString() == "R")
                                                {
                                                    objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Locked = false;
                                                    objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Text = Convert.ToString(result);
                                                    objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Locked = true;
                                                }
                                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Locked = false;
                                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].CellType = intgrcel;
                                            }
                                        }
                                        else
                                        {
                                            objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Text = "0";
                                            objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Locked = false;
                                            objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].CellType = intgrcel;
                                            attemps = Convert.ToString(dv_subj[0]["attempts"]);
                                        }
                                    }
                                    if (head.Trim().ToString() != "")
                                    {
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Note = head.Trim().ToString() + "-" + minimummarks.ToString() + "-" + Convert.ToString(attemps);//For Scripting
                                        if (head.Trim().ToString() == "R")
                                        {
                                            objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].CellType = objcombo;
                                            objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Locked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (ddl_type.SelectedItem.Text == "Grade")
                {
                    string[] cmb_grade = new string[100];
                    FarPoint.Web.Spread.ComboBoxCellType objcmbgrade = new FarPoint.Web.Spread.ComboBoxCellType();
                    if (dt_get_subject.Tables[0].Rows.Count > 0)
                    {
                        if (ds_grade.Tables[0].Rows.Count > 0)
                        {
                            cmb_grade = new string[ds_grade.Tables[0].Rows.Count + 1];
                            cmb_grade[0] = " ";
                            for (int gradeval = 1; gradeval <= ds_grade.Tables[0].Rows.Count; gradeval++)
                            {
                                cmb_grade[gradeval] = Convert.ToString(ds_grade.Tables[0].Rows[gradeval - 1]["mark_grade"]);
                            }
                            objcmbgrade = new FarPoint.Web.Spread.ComboBoxCellType(cmb_grade);
                            objcmbgrade.ShowButton = true;
                            objcmbgrade.UseValue = true;
                            objcmbgrade.AutoPostBack = true;
                        }
                        for (int sub = 0; sub < dt_get_subject.Tables[0].Rows.Count; sub++)//Load Header
                        {
                            int spcont = objspread.Sheets[0].ColumnCount;
                            objspread.Sheets[0].ColumnCount = objspread.Sheets[0].ColumnCount + 2;
                            objspread.Sheets[0].ColumnHeaderSpanModel.Add(0, spcont, 1, 2);
                            objspread.Sheets[0].ColumnHeader.Cells[0, spcont].Text = dt_get_subject.Tables[0].Rows[sub]["subject_code"].ToString() + "-Sem[" + dt_get_subject.Tables[0].Rows[sub]["semester"].ToString() + "]";
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 2].Text = "Grade";
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 2].Tag = dt_get_subject.Tables[0].Rows[sub]["subject_no"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["credit_points"].ToString();
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 2].Note = "G" + "-" + dt_get_subject.Tables[0].Rows[sub]["min_ext_marks"].ToString();
                            objspread.ActiveSheetView.Columns[objspread.Sheets[0].ColumnCount - 2].CellType = objcmbgrade;
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 1].Text = "Result";
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 1].Tag = dt_get_subject.Tables[0].Rows[sub]["subject_no"].ToString() + "-" + dt_get_subject.Tables[0].Rows[sub]["credit_points"].ToString();
                            objspread.Sheets[0].ColumnHeader.Cells[1, objspread.Sheets[0].ColumnCount - 1].Note = "R" + "-" + dt_get_subject.Tables[0].Rows[sub]["min_ext_marks"].ToString();
                            objspread.ActiveSheetView.Columns[objspread.Sheets[0].ColumnCount - 1].CellType = objcombo;
                        }
                        objspread.SaveChanges();
                        if (dt_exam_details.Tables[0].Rows.Count > 0)
                        {
                            for (int stud = 0; stud < dt_exam_details.Tables[0].Rows.Count; stud++)
                            {
                                objspread.Sheets[0].RowCount++;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 0].Text = objspread.Sheets[0].RowCount.ToString();
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 0].Locked = true;
                                objspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["roll_no"]);
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["roll_no"]);
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 1].CellType = objtext;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 1].Locked = true;
                                objspread.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["reg_no"]);
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 2].CellType = objtext;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 2].Locked = true;
                                objspread.Sheets[0].ColumnHeader.Columns[2].Width = 100;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["stud_name"]);
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 3].CellType = objtext;
                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, 3].Locked = true;
                                objspread.Sheets[0].ColumnHeader.Columns[3].Width = 200;
                                for (int colcnt = 4; colcnt < objspread.Sheets[0].ColumnCount; colcnt = colcnt + 2)
                                {
                                    string[] splitsub = Convert.ToString(objspread.Sheets[0].ColumnHeader.Cells[1, colcnt].Tag).Split(new Char[] { '-' });
                                    string subject_no = Convert.ToString(splitsub[0]);
                                    string attemps = string.Empty;
                                    DataView dv_subj = new DataView();
                                    dt_subj.DefaultView.RowFilter = "subject_no='" + subject_no + "' and roll_no='" + Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["roll_no"]) + "'";
                                    dv_subj = dt_subj.DefaultView;
                                    if (dv_subj.Count == 0)
                                    {
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Center;
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Text = "--";
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].CellType = objtext;
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Locked = true;
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt + 1].HorizontalAlign = HorizontalAlign.Center;
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt + 1].Text = "--";
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt + 1].CellType = objtext;
                                        objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt + 1].Locked = true;
                                    }
                                    else
                                    {
                                        DataView dv_mark = new DataView();
                                        dt_mark.DefaultView.RowFilter = "exam_code='" + exam_code + "' and subject_no='" + subject_no + "' and roll_no='" + Convert.ToString(dt_exam_details.Tables[0].Rows[stud]["roll_no"]) + "'";
                                        dv_mark = dt_mark.DefaultView;
                                        if (dv_mark.Count > 0)
                                        {
                                            string gradestr = Convert.ToString(dv_mark[0]["grade"]);
                                            string result = Convert.ToString(dv_mark[0]["result"]);
                                            attemps = Convert.ToString(dv_subj[0]["attempts"]);
                                            if (gradestr.Trim().ToString() != "")
                                            {
                                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Text = gradestr;
                                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Note = Convert.ToString(attemps);
                                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Locked = false;
                                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt + 1].Text = result;
                                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt + 1].Note = Convert.ToString(attemps);
                                                objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt + 1].Locked = true;
                                            }
                                        }
                                        else
                                        {
                                            attemps = Convert.ToString(dv_subj[0]["attempts"]);
                                            objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Note = Convert.ToString(attemps);
                                            objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt].Locked = false;
                                            objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt + 1].Note = Convert.ToString(attemps);
                                            objspread.Sheets[0].Cells[objspread.Sheets[0].RowCount - 1, colcnt + 1].Locked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                objspread.Height = objspread.Sheets[0].RowCount * 25;
                objspread.Width = objspread.Sheets[0].ColumnCount * 95;//aruna 6mar2013 75;
                objspread.Sheets[0].PageSize = objspread.Sheets[0].RowCount;
                objspread.Sheets[0].AutoPostBack = false;
                objspread.SaveChanges();
            }
            if (objspread.Sheets[0].RowCount > 0)
            {
                objspread.Visible = true;
                Btn_save.Visible = true;
                Btn_Delete.Visible = true;
            }
        }
    }

    protected void Btn_ok_Click(object sender, EventArgs e)
    {
        lbl_msg.Visible = false;
        if (FpSpread2.Sheets[0].Cells[0, 0].Text.ToString() != "")
        {
            Panel3.Visible = false;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select atleast one subject code then proceed.')", true);
        }
    }

    protected void Btn_cancel_Click(object sender, EventArgs e)
    {
        LinkButton3_Click(sender, e);
        Panel3.Visible = false;
    }

    protected void Btn_Move_Click(object sender, EventArgs e)
    {
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            if (Convert.ToInt32(activerow) >= 0)
            {
                string sub_code = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
                string sub_no = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
                ArrayList AL_subcode = new ArrayList();
                ArrayList AL_subno = new ArrayList();
                for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
                {
                    string subject_code = FpSpread2.Sheets[0].Cells[i, 1].Text.ToString();
                    string subject_no = string.Empty;
                    if (subject_code != "")
                    {
                        subject_no = FpSpread2.Sheets[0].Cells[i, 1].Tag.ToString();
                        if (subject_code != sub_code)
                        {
                            AL_subcode.Add(subject_code);
                            AL_subno.Add(subject_no);
                        }
                    }
                }
                AL_subcode.Add(sub_code);
                AL_subno.Add(sub_no);
                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount;
                int sno = 0;
                for (int j = 0; j < AL_subcode.Count; j++)
                {
                    sno++;
                    string subject_code = AL_subcode[j].ToString();
                    string subject_no = AL_subno[j].ToString();
                    FpSpread2.Sheets[0].Cells[j, 0].Text = sno.ToString();
                    FpSpread2.Sheets[0].Cells[j, 0].HorizontalAlign = HorizontalAlign.Center;
                    FarPoint.Web.Spread.LabelCellType chkcell0 = new FarPoint.Web.Spread.LabelCellType();
                    FpSpread2.Sheets[0].Columns[1].CellType = chkcell0;
                    FpSpread2.Sheets[0].Cells[j, 1].Text = subject_code;
                    FpSpread2.Sheets[0].Cells[j, 1].Tag = subject_no;
                }
            }
        }
        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
    }

    protected void Btn_Moveall_Click(object sender, EventArgs e)
    {
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount;
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread2.Sheets[0].Cells[i, 0].Text = FpSpread1.Sheets[0].Cells[i, 0].Text.ToString();
                FpSpread2.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[i, 1].Text = FpSpread1.Sheets[0].Cells[i, 1].Text.ToString();
                FpSpread2.Sheets[0].Cells[i, 1].Tag = FpSpread1.Sheets[0].Cells[i, 1].Tag.ToString();
            }
        }
        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
    }

    protected void Btn_Remove_Click(object sender, EventArgs e)
    {
        if (FpSpread2.Sheets[0].RowCount > 0)
        {
            string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
            if (Convert.ToInt32(activerow) >= 0)
            {
                string sub_code = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
                string sub_no = string.Empty;
                if (sub_code != "")
                {
                    sub_no = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
                    ArrayList AL_subcode = new ArrayList();
                    ArrayList AL_subno = new ArrayList();
                    for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
                    {
                        string subject_code = FpSpread2.Sheets[0].Cells[i, 1].Text.ToString();
                        string subject_no = string.Empty;
                        if (subject_code != "")
                        {
                            subject_no = FpSpread2.Sheets[0].Cells[i, 1].Tag.ToString();
                            if (subject_code != sub_code)
                            {
                                AL_subcode.Add(subject_code);
                                AL_subno.Add(subject_no);
                            }
                        }
                    }
                    FpSpread2.Sheets[0].RowCount = 0;
                    FpSpread2.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount;
                    int sno = 0;
                    for (int j = 0; j < AL_subcode.Count; j++)
                    {
                        sno++;
                        string subject_code = AL_subcode[j].ToString();
                        string subject_no = AL_subno[j].ToString();
                        FpSpread2.Sheets[0].Cells[j, 0].Text = sno.ToString();
                        FpSpread2.Sheets[0].Cells[j, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[j, 1].Text = subject_code;
                        FpSpread2.Sheets[0].Cells[j, 1].Tag = subject_no;
                    }
                }
            }
        }
        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
    }

    protected void Btn_Removeall_Click(object sender, EventArgs e)
    {
        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread2.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount;
        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
    }

    bool Cellclick1;
    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        Cellclick1 = true;
    }

    bool Cellclick2;
    protected void FpSpread2_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
        Cellclick2 = true;
    }

    protected void Btn_save_Click(object sender, EventArgs e)
    {
        bool saveflag = false;
        string exam_code = "0";
        string cur_sem = "0";
        bool intflag = false;
        bool extflag = false;
        ArrayList al_sub_code = new ArrayList();
        try
        {
            if (ddl_operation.SelectedItem.ToString() == "Import")
            {
                if (FpSpread2.Sheets[0].RowCount == 0)
                {
                    lbl_msg.Visible = true;
                    lbl_msg.Text = "Please Set Header Settings and then proceed.";
                    return;
                }
                for (int j = 1; j <= Fp_Marks.Sheets[0].ColumnCount - 1; j++)
                {
                    string int_subject_code = Fp_Marks.Sheets[0].Cells[0, j].Text.ToString();
                    string subject_code = string.Empty;
                    string[] spl_subject_code = int_subject_code.Split('-');
                    if (spl_subject_code.GetUpperBound(0) > 0)
                    {
                        subject_code = spl_subject_code[1].ToString().Trim();
                        if (!al_sub_code.Contains(subject_code.ToString()))
                        {
                            al_sub_code.Add(subject_code);
                        }
                    }
                }
                DataTable dt_subject = new DataTable();
                DataColumn dc_subject;
                dc_subject = new DataColumn();
                dc_subject.ColumnName = "Sub_Code";
                dt_subject.Columns.Add(dc_subject);
                dc_subject = new DataColumn();
                dc_subject.ColumnName = "Sub_No";
                dt_subject.Columns.Add(dc_subject);
                DataRow dr_subject;
                for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
                {
                    if (FpSpread2.Sheets[0].Cells[i, 0].Text.ToString() != "")
                    {
                        if (al_sub_code.Contains(FpSpread2.Sheets[0].Cells[i, 1].Text.ToString().Trim()) == true)
                        {
                            dr_subject = dt_subject.NewRow();
                            dr_subject["Sub_Code"] = FpSpread2.Sheets[0].Cells[i, 1].Text.ToString();
                            dr_subject["Sub_No"] = FpSpread2.Sheets[0].Cells[i, 1].Tag.ToString();
                            dt_subject.Rows.Add(dr_subject);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Selected Subject Code not available in the uploaded document.')", true);
                            return;
                        }
                    }
                }
                con.Close();
                con.Open();
                SqlCommand cmd_getsum = new SqlCommand("Select Exam_Code,current_semester from Exam_Details where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "' and Exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedValue.ToString() + "'", con);
                SqlDataAdapter ad_getsum = new SqlDataAdapter(cmd_getsum);
                DataTable dt_getsum = new DataTable();
                ad_getsum.Fill(dt_getsum);
                if (dt_getsum.Rows.Count > 0)
                {
                    exam_code = dt_getsum.Rows[0]["Exam_Code"].ToString();
                    cur_sem = dt_getsum.Rows[0]["current_semester"].ToString();
                }
                con.Close();
                con.Open();
                SqlCommand cmd_exam_details = new SqlCommand("select * from exam_appl_details e,exam_application ea where e.appl_no=ea.appl_no and ea.exam_code=" + exam_code + "", con);
                SqlDataAdapter ad_exam_details = new SqlDataAdapter(cmd_exam_details);
                DataTable dt_exam_details = new DataTable();
                ad_exam_details.Fill(dt_exam_details);
                con.Close();
                con.Open();
                // SqlCommand cmd_sub_details = new SqlCommand("select * from subject s,syllabus_master sy where s.syll_code=sy.syll_code and batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "' and semester=" + cur_sem + "", con);
                SqlCommand cmd_sub_details = new SqlCommand("select * from subject,exam_application,exam_appl_details where exam_application.appl_no=exam_appl_details.appl_no and exam_appl_details.subject_no=subject.subject_no and exam_code='" + exam_code + "'", con);
                SqlDataAdapter ad_sub_details = new SqlDataAdapter(cmd_sub_details);
                DataTable dt_sub_details = new DataTable();
                ad_sub_details.Fill(dt_sub_details);
                con.Close();
                con.Open();
                SqlCommand cmd_grade = new SqlCommand("select * from grade_master where degree_code = '" + ddl_dept.SelectedValue.ToString() + "' and batch_year = '" + ddl_batch.SelectedItem.ToString() + "'", con);
                SqlDataAdapter ad_grade = new SqlDataAdapter(cmd_grade);
                DataTable dt_grade = new DataTable();
                ad_grade.Fill(dt_grade);
                con.Close();
                con.Open();
                SqlCommand cmd_stu_details = new SqlCommand("select reg_no,exam_application.* from exam_application,registration Where exam_code ='" + exam_code + "' And exam_application.roll_no = registration.roll_no", con);
                SqlDataAdapter ad_get_stu_details = new SqlDataAdapter(cmd_stu_details);
                DataTable dt_get_stu_details = new DataTable();
                ad_get_stu_details.Fill(dt_get_stu_details);
                if (dt_get_stu_details.Rows.Count > 0)
                {
                    for (int i = 1; i < Fp_Marks.Sheets[0].RowCount; i++)
                    {
                        string roll_no = string.Empty;
                        // roll_no = Fp_Marks.Sheets[0].Cells[i, 0].Text.ToString();
                        string reg_no = Fp_Marks.Sheets[0].Cells[i, 0].Text.ToString();
                        double int_mark = 0;
                        double ex_mark = 0;
                        string grade = string.Empty;
                        double total_marks = 0;
                        DataView dv_stu_details = new DataView();
                        //dt_get_stu_details.DefaultView.RowFilter = "roll_no='" + roll_no + "'";
                        dt_get_stu_details.DefaultView.RowFilter = "reg_no='" + reg_no + "'";
                        dv_stu_details = dt_get_stu_details.DefaultView;
                        if (dv_stu_details.Count > 0)
                        {
                            roll_no = Convert.ToString(dv_stu_details[0]["roll_no"]);
                            string appl_no = string.Empty;
                            for (int j = 1; j <= Fp_Marks.Sheets[0].ColumnCount - 1; j++)
                            {
                                bool whdflag = false;
                                bool absflag = false;
                                bool SAflag = false;   //modified by prabha on feb 07 2018
                                bool RAflaf = false;
                                bool whflag = false;
                                bool mcflag = false;
                                intflag = false;
                                SAflag = false;
                                extflag = false;
                                string int_subject_code = Fp_Marks.Sheets[0].Cells[0, j].Text.ToString();
                                string subject_code = string.Empty;
                                string[] spl_subject_code = int_subject_code.Split('-');
                                if (spl_subject_code.GetUpperBound(0) > 0)//Added by srinath 14/10/2014
                                {
                                    subject_code = spl_subject_code[1].ToString();
                                    int_mark = 0;
                                    ex_mark = 0;
                                    grade = string.Empty;
                                    total_marks = 0;
                                    if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim() != "" && spl_subject_code[0].ToString().ToLower() == "int")//trim added by srinath 16/9/2014
                                    {
                                        if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "aaa")
                                        {
                                            int_mark = 0;
                                        }
                                        else
                                        {
                                            int_mark = Convert.ToDouble(Fp_Marks.Sheets[0].Cells[i, j].Text.ToString());
                                        }
                                        intflag = true;
                                    }
                                    if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim() != "" && spl_subject_code[0].ToString().ToLower() == "ext")//trim added by srinath 16/9/2014
                                    {
                                        if (ddl_type.SelectedItem.Text == "Mark")
                                        {
                                            if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "aaa")
                                            {
                                                ex_mark = 0;
                                                absflag = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "whd")
                                            {
                                                ex_mark = 0;
                                                whdflag = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "ra") //Added By Mullai
                                            {
                                                ex_mark = 0;
                                                RAflaf = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "sa") //Added By Mullai
                                            {
                                                ex_mark = 0;
                                                SAflag = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "wh") //Added By Mullai
                                            {
                                                ex_mark = 0;
                                                whflag = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "mc") //Added By Mullai
                                            {
                                                ex_mark = 0;
                                                mcflag = true;
                                            }
                                            else
                                            {
                                                ex_mark = Convert.ToDouble(Fp_Marks.Sheets[0].Cells[i, j].Text.ToString());
                                            }
                                        }
                                        else if (ddl_type.SelectedItem.Text == "Grade")
                                        {
                                            if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "whd")
                                            {
                                                grade = Convert.ToString(Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim());
                                                ex_mark = 0;
                                                whdflag = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "sa")
                                            {
                                                grade = Convert.ToString(Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim());
                                                ex_mark = 0;
                                                SAflag = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "aaa")
                                            {
                                                grade = Convert.ToString(Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim());
                                                ex_mark = 0;
                                                absflag = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "ra")  //Added By Mullai
                                            {
                                                grade = Convert.ToString(Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim());
                                                ex_mark = 0;
                                                RAflaf = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "wh")  //Added By Mullai
                                            {
                                                grade = Convert.ToString(Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim());
                                                ex_mark = 0;
                                                whflag = true;
                                            }
                                            else if (Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim().ToLower() == "mc") //Added By Mullai
                                            {
                                                grade = "RA";
                                                ex_mark = 0;
                                                mcflag = true;
                                            }
                                            else
                                            {
                                                grade = Convert.ToString(Fp_Marks.Sheets[0].Cells[i, j].Text.ToString().Trim());//Triming by srinath 30Jan2015
                                            }
                                        }
                                        extflag = true;
                                    }
                                    total_marks = int_mark + ex_mark;
                                    DataView dv_subject = new DataView();
                                    dt_subject.DefaultView.RowFilter = "Sub_Code='" + subject_code + "'";
                                    dv_subject = dt_subject.DefaultView;
                                    if (dv_subject.Count > 0)
                                    {
                                        string sub_no = dv_subject[0]["Sub_No"].ToString();
                                        DataView dv_exam_details = new DataView();
                                        dt_exam_details.DefaultView.RowFilter = "subject_no=" + sub_no + "";
                                        dv_exam_details = dt_exam_details.DefaultView;
                                        if (dv_exam_details.Count > 0)
                                        {
                                            string subject_no = dv_exam_details[0]["subject_no"].ToString();
                                            DataView dv_sub_details = new DataView();
                                            dt_sub_details.DefaultView.RowFilter = "subject_no='" + subject_no + "'";
                                            dv_sub_details = dt_sub_details.DefaultView;
                                            if (dv_sub_details.Count > 0)
                                            {
                                                string credit_points = string.Empty;//Modified by srinath
                                                credit_points = "0";
                                                if (dv_sub_details[0]["credit_points"].ToString() != "" && dv_sub_details[0]["credit_points"].ToString() != null)
                                                {
                                                    credit_points = dv_sub_details[0]["credit_points"].ToString();
                                                }
                                                int attempts = Convert.ToInt32(dv_exam_details[0]["attempts"].ToString()) + 1;
                                                int mydata = (Convert.ToInt32(ddl_exyear.SelectedItem.ToString()) * 12) + Convert.ToInt32(ddl_exmonth.SelectedValue.ToString());
                                                string type = string.Empty;
                                                if (Convert.ToInt32(dv_exam_details[0]["attempts"].ToString()) > 0)
                                                {
                                                    type = "*";
                                                }
                                                else
                                                {
                                                    type = string.Empty;
                                                }
                                                string result_text = string.Empty;
                                                string result_value = string.Empty;
                                                con.Close();
                                                con.Open();
                                                ds = obi_access.select_method("select * from mark_entry where roll_no='" + roll_no + "' and exam_code='" + exam_code + "' and subject_no='" + subject_no + "'", ht, "text");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                                                {
                                                    if (int_mark >= Convert.ToDouble(dv_sub_details[0]["min_int_marks"].ToString()) && ex_mark >= Convert.ToDouble(dv_sub_details[0]["min_ext_marks"].ToString()) && total_marks >= Convert.ToDouble(dv_sub_details[0]["mintotal"].ToString()))
                                                    {
                                                        result_text = "Pass";
                                                        result_value = "1";
                                                    }
                                                    else
                                                    {
                                                        result_text = "Fail";
                                                        result_value = "0";
                                                    }
                                                    if (whdflag == true)//Added By Srinath 26/6/2014
                                                    {
                                                        result_text = "WHD";
                                                        result_value = "0";
                                                    }
                                                    if (SAflag == true)  //added by prabha on feb 07 2018
                                                    {
                                                        result_text = "SA";
                                                        result_value = "0";
                                                    }
                                                    if (absflag == true)//Added By Srinath 26/6/2014
                                                    {
                                                        result_text = "AAA";
                                                        result_value = "0";
                                                    }
                                                    if (RAflaf == true)//Added By Mullai
                                                    {
                                                        result_text = "Fail";
                                                        result_value = "0";
                                                    }
                                                    if (whflag == true)//Added By Mullai
                                                    {
                                                        result_text = "Fail";
                                                        result_value = "0";
                                                    }
                                                    if (mcflag == true)
                                                    {
                                                        result_text = "MC";
                                                        result_value = "0";
                                                    }
                                                    if (intflag == true)
                                                    {
                                                        SqlCommand cmd_insert = new SqlCommand("insert into mark_entry (roll_no,subject_no,internal_mark,external_mark,total,result,passorfail,type,exam_code,attempts,mydata,cp,actual_internal_mark,actual_external_mark,actual_total) values('" + roll_no + "','" + subject_no + "','" + int_mark + "',0,'" + total_marks + "','" + result_text + "','" + result_value + "','" + type + "','" + exam_code + "','" + attempts + "','" + mydata + "'," + credit_points + ",'" + int_mark + "',0,'" + total_marks + "')", con);
                                                        cmd_insert.ExecuteNonQuery();
                                                        ds = obi_access.select_method("select * from camarks where roll_no='" + roll_no + "' and exam_code='" + exam_code + "' and subject_no='" + subject_no + "'", ht, "text");
                                                        if (ds.Tables[0].Rows.Count == 0)
                                                        {
                                                            SqlCommand cmd_insert1 = new SqlCommand("insert into camarks (subject_no,roll_no,total,exam_code,actual_total) values('" + subject_no + "','" + roll_no + "','" + int_mark + "','" + exam_code + "','" + int_mark + "')", con);
                                                            cmd_insert1.ExecuteNonQuery();
                                                            saveflag = true;
                                                        }
                                                        else
                                                        {
                                                            SqlCommand cmd_insert1 = new SqlCommand("update camarks set  total='" + int_mark + "',actual_total='" + int_mark + "' where subject_no='" + subject_no + "' and roll_no='" + roll_no + "' and exam_code='" + exam_code + "' ", con);
                                                            cmd_insert1.ExecuteNonQuery();
                                                            saveflag = true;
                                                        }
                                                    }
                                                    else if (extflag == true)
                                                    {
                                                        if (ddl_type.SelectedItem.Text == "Mark")
                                                        {
                                                            SqlCommand cmd_insert = new SqlCommand("insert into mark_entry (roll_no,subject_no,internal_mark,external_mark,total,result,passorfail,type,exam_code,attempts,mydata,cp,actual_internal_mark,actual_external_mark,actual_total) values('" + roll_no + "','" + subject_no + "',0,'" + ex_mark + "','" + total_marks + "','" + result_text + "','" + result_value + "','" + type + "','" + exam_code + "','" + attempts + "','" + mydata + "'," + credit_points + ",0,'" + ex_mark + "','" + total_marks + "')", con);
                                                            cmd_insert.ExecuteNonQuery();
                                                            saveflag = true;
                                                        }
                                                        else if (ddl_type.SelectedItem.Text == "Grade")
                                                        {
                                                            DataView dv_gradedet = new DataView();
                                                            dt_grade.DefaultView.RowFilter = "mark_grade='" + grade + "'";
                                                            dv_gradedet = dt_grade.DefaultView;
                                                            if (dv_gradedet.Count > 0 && !whflag && !RAflaf && !SAflag && !absflag && !whdflag)
                                                            {
                                                                int frange = Convert.ToInt16(dv_gradedet[0]["frange"]);
                                                                int trange = Convert.ToInt16(dv_gradedet[0]["trange"]);
                                                                if (frange >= Convert.ToDouble(dv_sub_details[0]["min_ext_marks"].ToString()))
                                                                {
                                                                    result_text = "Pass";
                                                                    result_value = "1";
                                                                }
                                                                else
                                                                {
                                                                    result_text = "Fail";
                                                                    result_value = "0";
                                                                }
                                                               
                                                            }
                                                            SqlCommand cmd_insert = new SqlCommand("insert into mark_entry (roll_no,subject_no,internal_mark,grade,total,result,passorfail,type,exam_code,attempts,mydata,cp,actual_internal_mark,actual_external_mark,actual_total) values('" + roll_no + "','" + subject_no + "',0,'" + grade + "','" + total_marks + "','" + result_text + "','" + result_value + "','" + type + "','" + exam_code + "','" + attempts + "','" + mydata + "'," + credit_points + ",0,'" + ex_mark + "','" + total_marks + "')", con);
                                                            cmd_insert.ExecuteNonQuery();
                                                            saveflag = true;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (intflag == true)
                                                    {
                                                        if (ds.Tables[0].Rows[0]["external_mark"].ToString() == "" || ds.Tables[0].Rows[0]["external_mark"].ToString() == null)
                                                        {
                                                            ex_mark = 0;
                                                        }
                                                        else
                                                        {
                                                            ex_mark = Convert.ToDouble(ds.Tables[0].Rows[0]["external_mark"].ToString());
                                                        }
                                                        total_marks = int_mark + ex_mark;
                                                        if (int_mark >= Convert.ToDouble(dv_sub_details[0]["min_int_marks"].ToString()) && ex_mark >= Convert.ToDouble(dv_sub_details[0]["min_ext_marks"].ToString()) && total_marks >= Convert.ToDouble(dv_sub_details[0]["mintotal"].ToString()))
                                                        {
                                                            result_text = "Pass";
                                                            result_value = "1";
                                                        }
                                                        else
                                                        {
                                                            result_text = "Fail";
                                                            result_value = "0";
                                                        }
                                                        SqlCommand cmd_insert = new SqlCommand("update mark_entry set internal_mark='" + int_mark + "', total='" + total_marks + "', result='" + result_text + "', passorfail='" + result_value + "', type='" + type + "', attempts='" + attempts + "', mydata='" + mydata + "', cp=" + credit_points + ", actual_internal_mark='" + int_mark + "', actual_total='" + total_marks + "' where roll_no='" + roll_no + "' and subject_no='" + subject_no + "' and exam_code='" + exam_code + "'", con);
                                                        cmd_insert.ExecuteNonQuery();
                                                        ds = obi_access.select_method("select * from camarks where roll_no='" + roll_no + "' and exam_code='" + exam_code + "' and subject_no='" + subject_no + "'", ht, "text");
                                                        if (ds.Tables[0].Rows.Count == 0)
                                                        {
                                                            SqlCommand cmd_insert1 = new SqlCommand("insert into camarks (subject_no,roll_no,total,exam_code,actual_total) values('" + subject_no + "','" + roll_no + "','" + int_mark + "','" + exam_code + "','" + int_mark + "')", con);
                                                            cmd_insert1.ExecuteNonQuery();
                                                            saveflag = true;
                                                        }
                                                        else
                                                        {
                                                            SqlCommand cmd_insert1 = new SqlCommand("update camarks set  total='" + int_mark + "',actual_total='" + int_mark + "' where subject_no='" + subject_no + "' and roll_no='" + roll_no + "' and exam_code='" + exam_code + "' ", con);
                                                            cmd_insert1.ExecuteNonQuery();
                                                            saveflag = true;
                                                        }
                                                    }
                                                    else if (extflag == true)
                                                    {
                                                        //added by gowtham
                                                        if (ds.Tables[0].Rows[0]["internal_mark"].ToString() == "" || ds.Tables[0].Rows[0]["internal_mark"].ToString() == null)
                                                        {
                                                            int_mark = 0;
                                                        }
                                                        else
                                                        {
                                                            int_mark = Convert.ToDouble(ds.Tables[0].Rows[0]["internal_mark"].ToString());
                                                        }
                                                        // end
                                                        total_marks = int_mark + ex_mark;
                                                        //if (int_mark >= Convert.ToInt32(dv_sub_details[0]["min_int_marks"].ToString()) && ex_mark >= Convert.ToInt32(dv_sub_details[0]["min_ext_marks"].ToString()) && total_marks >= Convert.ToInt32(dv_sub_details[0]["mintotal"].ToString())) // Hided by gowtham jan 22-2014
                                                        if (int_mark >= Convert.ToDouble(dv_sub_details[0]["min_int_marks"].ToString()) && ex_mark >= Convert.ToDouble(dv_sub_details[0]["min_ext_marks"].ToString()) && total_marks >= Convert.ToDouble(dv_sub_details[0]["mintotal"].ToString())) // modified by gowtham jan 22-2014
                                                        {
                                                            result_text = "Pass";
                                                            result_value = "1";
                                                        }
                                                        else
                                                        {
                                                            result_text = "Fail";
                                                            result_value = "0";
                                                        }
                                                        if (whdflag == true)//Added By Srinath 26/6/2014
                                                        {
                                                            result_text = "WHD";
                                                            result_value = "0";
                                                        }
                                                        if (absflag == true)//Added By Srinath 26/6/2014
                                                        {
                                                            result_text = "AAA";
                                                            result_value = "0";
                                                        }
                                                        if (SAflag == true)//Added By Srinath 26/6/2014
                                                        {
                                                            result_text = "SA";
                                                            result_value = "0";
                                                        }
                                                        if (RAflaf == true)  //Added By Mullai
                                                        {
                                                            result_text = "Fail";
                                                            result_value = "0";
                                                        }
                                                        if (whflag == true)  //Added By Mullai
                                                        {
                                                            result_text = "Fail";
                                                            result_value = "0";
                                                        }
                                                        if (mcflag == true)  //Added By Mullai
                                                        {
                                                            result_text = "MC";
                                                            result_value = "0";
                                                        }

                                                        if (ddl_type.SelectedItem.Text == "Mark")
                                                        {
                                                            SqlCommand cmd_insert = new SqlCommand("update mark_entry set external_mark='" + ex_mark + "', total='" + total_marks + "', result='" + result_text + "', passorfail='" + result_value + "', type='" + type + "', attempts='" + attempts + "', mydata='" + mydata + "', cp=" + credit_points + ", actual_external_mark='" + ex_mark + "', actual_total='" + total_marks + "' where roll_no='" + roll_no + "' and subject_no='" + subject_no + "' and exam_code='" + exam_code + "'", con);
                                                            cmd_insert.ExecuteNonQuery();
                                                            saveflag = true;
                                                        }
                                                        else if (ddl_type.SelectedItem.Text == "Grade")
                                                        {
                                                            DataView dv_gradedet = new DataView();
                                                            dt_grade.DefaultView.RowFilter = "mark_grade='" + grade.Trim() + "'";
                                                            dv_gradedet = dt_grade.DefaultView;


                                                            if (dv_gradedet.Count > 0 && !whflag && !RAflaf && !SAflag && !absflag && !whdflag)
                                                            {
                                                                int frange = Convert.ToInt16(dv_gradedet[0]["frange"]);
                                                                int trange = Convert.ToInt16(dv_gradedet[0]["trange"]);
                                                                if (frange >= Convert.ToDouble(dv_sub_details[0]["min_ext_marks"].ToString()))
                                                                {
                                                                    result_text = "Pass";
                                                                    result_value = "1";
                                                                }
                                                                else
                                                                {
                                                                    result_text = "Fail";
                                                                    result_value = "0";
                                                                }
                                                            }
                                                            
                                                            SqlCommand cmd_insert = new SqlCommand("update mark_entry set grade='" + grade + "', total='" + total_marks + "', result='" + result_text + "', passorfail='" + result_value + "', type='" + type + "', attempts='" + attempts + "', mydata='" + mydata + "', cp=" + credit_points + ", actual_external_mark='" + ex_mark + "', actual_total='" + total_marks + "' ,actual_grade='" + grade + "' where roll_no='" + roll_no + "' and subject_no='" + subject_no + "' and exam_code='" + exam_code + "'", con);//Modified by gowtham 10/3/2014
                                                            cmd_insert.ExecuteNonQuery();
                                                            saveflag = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }//Added by srinath 14/10/2014
                            }
                        }
                    }
                    if (saveflag == true)
                    {
                        SqlCommand grade_delete = new SqlCommand("delete from grademaster where degree_code = '" + ddl_dept.SelectedValue.ToString() + "' and batch_year = '" + ddl_batch.SelectedItem.ToString() + "' and exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedItem.Text.ToString() + "'", con);
                        grade_delete.ExecuteNonQuery();
                        if (ddl_type.SelectedItem.Text == "Mark")
                        {
                            SqlCommand grade_insert = new SqlCommand("insert into grademaster (batch_year,degree_code,exam_month,exam_year,grade_flag) values ('" + ddl_batch.SelectedItem.ToString() + "','" + ddl_dept.SelectedValue.ToString() + "','" + ddl_exmonth.SelectedValue.ToString() + "','" + ddl_exyear.SelectedItem.Text.ToString() + "',3)", con);
                            grade_insert.ExecuteNonQuery();
                        }
                        else if (ddl_type.SelectedItem.Text == "Grade")
                        {
                            SqlCommand grade_insert = new SqlCommand("insert into grademaster (batch_year,degree_code,exam_month,exam_year,grade_flag) values ('" + ddl_batch.SelectedItem.ToString() + "','" + ddl_dept.SelectedValue.ToString() + "','" + ddl_exmonth.SelectedValue.ToString() + "','" + ddl_exyear.SelectedItem.Text.ToString() + "',2)", con);
                            grade_insert.ExecuteNonQuery();
                        }
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Marks saved successfully.')", true);
                        Btn_save.Visible = false;
                        Fp_Marks.Visible = false;
                        Fp_Grade.Visible = false;
                    }
                }
            }
            else if (ddl_operation.SelectedItem.ToString() == "Entry")
            {
                if (ddl_type.SelectedItem.Text == "Mark")
                {
                    Fp_Marks.SaveChanges();
                    string intmar = "", extmark = "", total = "", result = "", passorfail = "", attempt = "", minint = "", minext = "", mintot = string.Empty;
                    con.Close();
                    con.Open();
                    SqlCommand cmd_getsum = new SqlCommand("Select Exam_Code,current_semester from Exam_Details where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "' and Exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedValue.ToString() + "'", con);
                    SqlDataAdapter ad_getsum = new SqlDataAdapter(cmd_getsum);
                    DataTable dt_getsum = new DataTable();
                    ad_getsum.Fill(dt_getsum);
                    string examcode = string.Empty;
                    if (dt_getsum.Rows.Count > 0)
                    {
                        examcode = dt_getsum.Rows[0]["Exam_Code"].ToString();
                    }
                    if (examcode.Trim().ToString() != "")
                    {
                        for (int rowcnt = 0; rowcnt < Fp_Marks.Sheets[0].RowCount; rowcnt++)
                        {
                            for (int colcnt = 4; colcnt < Fp_Marks.Sheets[0].ColumnCount; colcnt = colcnt + 4)
                            {
                                string roll_no = Convert.ToString(Fp_Marks.Sheets[0].Cells[rowcnt, 1].Note);
                                if (roll_no.Trim().ToString() != "")
                                {
                                    intmar = Convert.ToString(Fp_Marks.Sheets[0].Cells[rowcnt, colcnt].Text);
                                    string minint_tag = Convert.ToString(Fp_Marks.Sheets[0].ColumnHeader.Cells[1, colcnt].Tag);
                                    string[] minint_split = minint_tag.Split(new Char[] { '-' });
                                    if (minint_split.GetUpperBound(0) > 0)//added by srinath 14/10/2014
                                    {
                                        minint = Convert.ToString(minint_split[1]);
                                        if (intmar.ToString().Trim() == "--")
                                        {
                                            goto NextFor;
                                        }
                                        else
                                        {
                                            extmark = Convert.ToString(Fp_Marks.Sheets[0].Cells[rowcnt, colcnt + 1].Text);
                                            string minext_tag = Convert.ToString(Fp_Marks.Sheets[0].ColumnHeader.Cells[1, colcnt + 1].Tag);
                                            string[] minext_split = minext_tag.Split(new Char[] { '-' });
                                            if (minext_split.GetUpperBound(0) > 0)//added by srinath 14/10/2014
                                            {
                                                minext = Convert.ToString(minext_split[1]);
                                            }
                                            total = Convert.ToString(Fp_Marks.Sheets[0].Cells[rowcnt, colcnt + 2].Text);
                                            string mintot_tag = Convert.ToString(Fp_Marks.Sheets[0].ColumnHeader.Cells[1, colcnt + 2].Tag);
                                            string[] mintot_split = mintot_tag.Split(new Char[] { '-' });
                                            if (mintot_split.GetUpperBound(0) > 0)//added by srinath 14/10/2014
                                            {
                                                mintot = Convert.ToString(mintot_split[1]);
                                            }
                                            result = Convert.ToString(Fp_Marks.Sheets[0].Cells[rowcnt, colcnt + 3].Text);
                                        }
                                        string subnote = Convert.ToString(Fp_Marks.Sheets[0].ColumnHeader.Cells[1, colcnt].Note);
                                        string[] subno = subnote.Split(new Char[] { '-' });
                                        if (subno.GetUpperBound(0) > 0)//added by srinath 14/10/2014
                                        {
                                            string subject_no = Convert.ToString(subno[0]);
                                            string credit_points = Convert.ToString(subno[1]);
                                            if (credit_points == null && credit_points.Trim() == "")
                                            {
                                                credit_points = "0";//added by srinath 14/6/2014
                                            }
                                            string stunote = Convert.ToString(Fp_Marks.Sheets[0].Cells[rowcnt, colcnt].Note);
                                            string[] attm = stunote.Split(new Char[] { '-' });
                                            if (attm.GetUpperBound(0) > 0)//added by srinath 14/10/2014
                                            {
                                                string attempts = Convert.ToString(attm[3]);
                                                if (attempts == "" || attempts == null)
                                                {
                                                    attempts = "0";
                                                }
                                                ds = obi_access.select_method("select * from mark_entry where roll_no='" + roll_no + "' and exam_code='" + examcode + "' and subject_no='" + subject_no + "'", ht, "text");
                                                if (Convert.ToDouble(intmar) >= Convert.ToDouble(minint) && Convert.ToDouble(extmark) >= Convert.ToDouble(minext) && Convert.ToDouble(total) >= Convert.ToDouble(mintot))
                                                {
                                                    passorfail = "1";
                                                }
                                                else
                                                {
                                                    passorfail = "0";
                                                }
                                                string type = string.Empty;
                                                if (Convert.ToInt32(attempts) > 0)
                                                {
                                                    type = "*";
                                                }
                                                else
                                                {
                                                    type = string.Empty;
                                                }
                                                attempts = Convert.ToString(Convert.ToInt32(attempts) + 1);
                                                int mydata = (Convert.ToInt32(ddl_exyear.SelectedItem.ToString()) * 12) + Convert.ToInt32(ddl_exmonth.SelectedValue.ToString());
                                                if (ds.Tables[0].Rows.Count == 0)
                                                {
                                                    SqlCommand cmd_insert = new SqlCommand("insert into mark_entry (roll_no,subject_no,internal_mark,external_mark,total,result,passorfail,type,exam_code,attempts,mydata,cp,actual_internal_mark,actual_external_mark,actual_total) values('" + roll_no + "','" + subject_no + "','" + intmar + "','" + extmark + "','" + total + "','" + result + "','" + passorfail + "','" + type + "','" + examcode + "','" + attempts + "','" + mydata + "'," + credit_points + ",'" + intmar + "','" + extmark + "','" + total + "')", con);
                                                    cmd_insert.ExecuteNonQuery();
                                                    saveflag = true;
                                                }
                                                else
                                                {
                                                    SqlCommand cmd_insert = new SqlCommand("update mark_entry set internal_mark='" + intmar + "',external_mark='" + extmark + "',total='" + total + "',result='" + result + "',passorfail='" + passorfail + "',type='" + type + "',attempts='" + attempts + "',mydata='" + mydata + "',cp=" + credit_points + ",actual_internal_mark='" + intmar + "',actual_external_mark='" + extmark + "',actual_total='" + total + "' where roll_no='" + roll_no + "' and subject_no='" + subject_no + "' and exam_code='" + examcode + "'", con);
                                                    cmd_insert.ExecuteNonQuery();
                                                    saveflag = true;
                                                }
                                                con.Close();
                                                con.Open();
                                                ds = obi_access.select_method("select * from camarks where roll_no='" + roll_no + "' and exam_code='" + examcode + "' and subject_no='" + subject_no + "'", ht, "text");
                                                if (ds.Tables[0].Rows.Count == 0)
                                                {
                                                    SqlCommand cmd_insert1 = new SqlCommand("insert into camarks (subject_no,roll_no,total,exam_code,actual_total) values('" + subject_no + "','" + roll_no + "','" + intmar + "','" + examcode + "','" + intmar + "')", con);
                                                    cmd_insert1.ExecuteNonQuery();
                                                    saveflag = true;
                                                }
                                                else
                                                {
                                                    SqlCommand cmd_insert1 = new SqlCommand("update camarks set  total='" + intmar + "',actual_total='" + intmar + "' where subject_no='" + subject_no + "' and roll_no='" + roll_no + "' and exam_code='" + examcode + "' ", con);
                                                    cmd_insert1.ExecuteNonQuery();
                                                    saveflag = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            NextFor: int xx = 0;
                            }
                        }
                        if (saveflag == true)
                        {
                            SqlCommand grade_delete = new SqlCommand("delete from grademaster where degree_code = '" + ddl_dept.SelectedValue.ToString() + "' and batch_year = '" + ddl_batch.SelectedItem.ToString() + "' and exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedItem.Text.ToString() + "'", con);
                            grade_delete.ExecuteNonQuery();
                            if (ddl_type.SelectedItem.Text == "Mark")
                            {
                                SqlCommand grade_insert = new SqlCommand("insert into grademaster (batch_year,degree_code,exam_month,exam_year,grade_flag) values ('" + ddl_batch.SelectedItem.ToString() + "','" + ddl_dept.SelectedValue.ToString() + "','" + ddl_exmonth.SelectedValue.ToString() + "','" + ddl_exyear.SelectedItem.Text.ToString() + "',3)", con);
                                grade_insert.ExecuteNonQuery();
                            }
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Marks saved successfully.')", true);
                        }
                    }
                }
                else if (ddl_type.SelectedItem.Text == "Grade")
                {
                    Fp_Grade.SaveChanges();
                    string passorfail = string.Empty;
                    SqlCommand cmd_getsum = new SqlCommand("Select Exam_Code,current_semester from Exam_Details where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "' and Exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedValue.ToString() + "'", con);
                    SqlDataAdapter ad_getsum = new SqlDataAdapter(cmd_getsum);
                    DataTable dt_getsum = new DataTable();
                    ad_getsum.Fill(dt_getsum);
                    string examcode = string.Empty;
                    if (dt_getsum.Rows.Count > 0)
                    {
                        examcode = dt_getsum.Rows[0]["Exam_Code"].ToString();
                    }
                    if (examcode.Trim().ToString() != "")
                    {
                        for (int rowcnt = 0; rowcnt < Fp_Grade.Sheets[0].RowCount; rowcnt++)
                        {
                            for (int colcnt = 4; colcnt < Fp_Grade.Sheets[0].ColumnCount; colcnt = colcnt + 2)
                            {
                                string roll_no = Convert.ToString(Fp_Grade.Sheets[0].Cells[rowcnt, 1].Note);
                                if (roll_no.Trim().ToString() != "")
                                {
                                    String grade_var = Convert.ToString(Fp_Grade.Sheets[0].Cells[rowcnt, colcnt].Text);
                                    if (grade_var.ToString().Trim() == "--")
                                    {
                                        goto NextFor1;
                                    }
                                    else
                                    {
                                        string subnote = Convert.ToString(Fp_Grade.Sheets[0].ColumnHeader.Cells[1, colcnt].Tag);
                                        string[] subno = subnote.Split(new Char[] { '-' });
                                        if (subno.GetUpperBound(0) > 0)//added by srinath 14/10/2014
                                        {
                                            string subject_no = Convert.ToString(subno[0]);
                                            string credit_points = Convert.ToString(subno[1]);
                                            if (credit_points == null && credit_points.Trim() == "")
                                            {
                                                credit_points = "0";//added by srinath 14/6/2014
                                            }
                                            string attempts = Convert.ToString(Fp_Grade.Sheets[0].Cells[rowcnt, colcnt].Note);
                                            if (attempts == "" || attempts == null)
                                            {
                                                attempts = "0";
                                            }
                                            string grade_val = Convert.ToString(Fp_Grade.Sheets[0].Cells[rowcnt, colcnt].Text);
                                            string result = Convert.ToString(Fp_Grade.Sheets[0].Cells[rowcnt, colcnt + 1].Text);
                                            if (grade_val.Trim().ToString() != "")
                                            {
                                                if (result.Trim().ToString() == "Pass")
                                                {
                                                    passorfail = "1";
                                                }
                                                else
                                                {
                                                    passorfail = "0";
                                                }
                                                string type = string.Empty;
                                                if (Convert.ToInt32(attempts) > 0)
                                                {
                                                    type = "*";
                                                }
                                                else
                                                {
                                                    type = string.Empty;
                                                }
                                                attempts = Convert.ToString(Convert.ToInt32(attempts) + 1);
                                                int mydata = (Convert.ToInt32(ddl_exyear.SelectedItem.ToString()) * 12) + Convert.ToInt32(ddl_exmonth.SelectedValue.ToString());
                                                ds = obi_access.select_method("select * from mark_entry where roll_no='" + roll_no + "' and exam_code='" + examcode + "' and subject_no='" + subject_no + "'", ht, "text");
                                                con.Close();
                                                con.Open();
                                                if (ds.Tables[0].Rows.Count == 0)
                                                {
                                                    SqlCommand cmd_insert = new SqlCommand("insert into mark_entry (roll_no,subject_no,grade,result,passorfail,type,exam_code,attempts,mydata,cp,actual_grade) values('" + roll_no + "','" + subject_no + "','" + grade_val + "','" + result + "','" + passorfail + "','" + type + "','" + examcode + "','" + attempts + "','" + mydata + "'," + credit_points + ",'" + grade_val + "')", con);
                                                    cmd_insert.ExecuteNonQuery();
                                                    saveflag = true;
                                                }
                                                else
                                                {
                                                    SqlCommand cmd_insert = new SqlCommand("update mark_entry set grade='" + grade_val + "',result='" + result + "',passorfail='" + passorfail + "',type='" + type + "',attempts='" + attempts + "',mydata='" + mydata + "',cp=" + credit_points + ",actual_grade='" + grade_val + "' where roll_no='" + roll_no + "' and subject_no='" + subject_no + "' and exam_code='" + examcode + "'", con);
                                                    cmd_insert.ExecuteNonQuery();
                                                    saveflag = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            NextFor1: int yy = 0;
                            }
                        }
                        if (saveflag == true)
                        {
                            SqlCommand grade_delete = new SqlCommand("delete from grademaster where degree_code = '" + ddl_dept.SelectedValue.ToString() + "' and batch_year = '" + ddl_batch.SelectedItem.ToString() + "' and exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedItem.Text.ToString() + "'", con);
                            grade_delete.ExecuteNonQuery();
                            if (ddl_type.SelectedItem.Text == "Grade")
                            {
                                SqlCommand grade_insert = new SqlCommand("insert into grademaster (batch_year,degree_code,exam_month,exam_year,grade_flag) values ('" + ddl_batch.SelectedItem.ToString() + "','" + ddl_dept.SelectedValue.ToString() + "','" + ddl_exmonth.SelectedValue.ToString() + "','" + ddl_exyear.SelectedItem.Text.ToString() + "',2)", con);
                                grade_insert.ExecuteNonQuery();
                            }
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Grade saved successfully.')", true);
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        Control cntPageNextBtn = Fp_Marks.FindControl("Next");
        Control cntPagePreviousBtn = Fp_Marks.FindControl("Prev");
        if ((cntPageNextBtn != null))
        {
            TableCell tc = (TableCell)cntPageNextBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }

    protected void Btn_Delete_Click(object sender, EventArgs e)
    {
        try
        {
            bool delflag = false;
            con.Close();
            con.Open();
            SqlCommand cmd_getsum = new SqlCommand("Select Exam_Code,current_semester from Exam_Details where batch_year='" + ddl_batch.SelectedItem.ToString() + "' and degree_code='" + ddl_dept.SelectedValue.ToString() + "' and Exam_month='" + ddl_exmonth.SelectedValue.ToString() + "' and exam_year='" + ddl_exyear.SelectedValue.ToString() + "'", con);
            SqlDataAdapter ad_getsum = new SqlDataAdapter(cmd_getsum);
            DataTable dt_getsum = new DataTable();
            ad_getsum.Fill(dt_getsum);
            string examcode = string.Empty;
            if (dt_getsum.Rows.Count > 0)
            {
                examcode = dt_getsum.Rows[0]["Exam_Code"].ToString();
            }
            if (examcode.Trim().ToString() != "")
            {
                con.Close();
                con.Open();
                SqlCommand cmd_del1 = new SqlCommand("delete from  mark_entry where exam_code=" + examcode + "", con);
                cmd_del1.ExecuteNonQuery();
                con.Close();
                con.Open();
                delflag = true;
                try
                {
                    int delete = obi_access.update_method_wo_parameter("delete from  camarks where exam_code=" + examcode + "", "Text");
                    //SqlCommand cmd_del2 = new SqlCommand("delete from  camarks where exam_code=" + examcode + "", con);
                    // cmd_del2.ExecuteNonQuery();
                }
                catch
                {
                }
            }
            if (delflag == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Marks deleted successfully.')", true);
                Btn_go_Click(sender, e);
            }
            Btn_Delete.Enabled = false;
        }
        catch
        {
        }
    }

    protected void Fp_Grade_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    protected void Fp_Grade_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            if (flag_true == false)
            {
                flag_true = true;
                string actrow = e.SheetView.ActiveRow.ToString();
                string actcol = e.SheetView.ActiveColumn.ToString();
                string note = Convert.ToString(Fp_Grade.Sheets[0].ColumnHeader.Cells[1, Convert.ToInt16(actcol)].Note);
                int activerow = 0;
                int activecol = 0;
                double min_ext_marks = 0;
                string grade = string.Empty;
                DataView dv_gradedet = new DataView();
                activerow = Convert.ToInt16(actrow);
                activecol = Convert.ToInt16(actcol);
                if (note.Trim().ToString() != "")
                {
                    string[] noteheader = note.Split('-');
                    if (noteheader[0].ToString() == "G")
                    {
                        min_ext_marks = Convert.ToDouble(noteheader[1]);
                        grade = Convert.ToString(e.EditValues[Convert.ToInt16(activecol)]);
                        if (grade != null)
                        {
                            if (grade.Trim().ToString() != "")
                            {
                                dt_grade.DefaultView.RowFilter = "mark_grade='" + grade + "'";
                                dv_gradedet = dt_grade.DefaultView;
                                if (dv_gradedet.Count > 0)
                                {
                                    double frange = Convert.ToInt16(dv_gradedet[0]["frange"]);
                                    double trange = Convert.ToInt16(dv_gradedet[0]["trange"]);
                                    if (frange >= Convert.ToDouble(min_ext_marks))
                                    {
                                        Fp_Grade.Sheets[0].Cells[activerow, activecol + 1].Locked = false;
                                        Fp_Grade.Sheets[0].Cells[activerow, activecol + 1].Text = "Pass";
                                        Fp_Grade.Sheets[0].Cells[activerow, activecol + 1].Locked = true;
                                    }
                                    else
                                    {
                                        Fp_Grade.Sheets[0].Cells[activerow, activecol + 1].Locked = false;
                                        Fp_Grade.Sheets[0].Cells[activerow, activecol + 1].Text = "Fail";
                                        Fp_Grade.Sheets[0].Cells[activerow, activecol + 1].Locked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

}
