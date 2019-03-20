using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Configuration;

public partial class Revaluation_Request : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dv = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess strAcc = new InsproStoreAccess();
    
    Hashtable hat = new Hashtable();
    bool dateflag = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string search = string.Empty;
    string filterby = string.Empty;
    string batch_year = string.Empty;
    string degree_code = string.Empty;
    string exam_code = string.Empty;
    string roll_no = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.MaintainScrollPositionOnPostBack = true;
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
            //usercode = Session["usercode"].ToString();
            //collegecode1 = Session["collegecode"].ToString();
            //singleuser = Session["single_user"].ToString();
            //group_user = Session["group_code"].ToString();

            usercode = Convert.ToString(Session["usercode"]).Trim();
            collegecode1 = Convert.ToString(Session["collegecode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();

            if (!IsPostBack)
            {
                btn_save.Visible = false;
                Fpspread3.Visible = false;
                txt_searchbyreg.Visible = false;
                txt_searchbyroll.Visible = false;
                ddl_mm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddl_mm.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddl_mm.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddl_mm.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddl_mm.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddl_mm.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddl_mm.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddl_mm.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddl_mm.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddl_mm.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddl_mm.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddl_mm.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddl_mm.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                // RadioButton1.Checked = true;
                int year1 = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
                ddl_yy.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {
                    ddl_yy.Items.Add(Convert.ToString(year1 - l));
                }
                ddl_yy.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                if (ddl_searchby.Items.Count > 0)
                {
                    if (ddl_searchby.SelectedValue == "1")
                    {
                        txt_searchbyreg.Visible = true;
                        txt_searchbyroll.Visible = false;
                    }
                    else if (ddl_searchby.SelectedValue == "2")
                    {
                        txt_searchbyreg.Visible = false;
                        txt_searchbyroll.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRollNo(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Roll_No from Registration where DelFlag=0 and Exam_Flag <>'Debar' and Roll_No Like '" + prefixText + "%'  order by Roll_No";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRegNo(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Reg_No  from Registration where Reg_No like '" + prefixText + "%' and DelFlag=0 and Exam_Flag <>'Debar' order by Reg_No  ";
        name = ws.Getname(query);
        return name;
    }

    protected void ddl_searchby_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btn_save.Visible = false;
        Fpspread3.Visible = false;
        if (ddl_searchby.SelectedItem.Text == "Reg No")
        {
            txt_searchbyreg.Visible = true;
            txt_searchbyroll.Visible = false;
        }
        if (ddl_searchby.SelectedItem.Text == "Roll No")
        {
            txt_searchbyroll.Visible = true;
            txt_searchbyreg.Visible = false;
        }
    }

    protected void btn_go_OnClick(object sender, EventArgs e)
    {
        try
        {
            btn_save.Visible = false;
            Fpspread3.Visible = false;
            DataSet dsSubDetails = new DataSet();
            string month = Convert.ToString(ddl_mm.SelectedItem.Value).Trim();
            string year = Convert.ToString(ddl_yy.SelectedItem.Text).Trim();
            string yr_val = Convert.ToString(ddl_yy.SelectedItem.Value).Trim();

            lblErr.Visible = false;
            //if (txt_searchbyreg.Text == "" || txt_searchbyroll.Text == "")
            //{
            //    lblErr.Text = "Please Enter Roll No or Reg. No";
            //    lblErr.Visible = true;
            //    return;
            //}
            if (month.Trim() == "0" || yr_val.Trim() == "0")
            {
                lblErr.Text = "Please Select Month and Year";
                lblErr.Visible = true;
                return;
            }
            if (ddl_searchby.SelectedItem.Text.Trim() == "Reg No")
            {
                if (txt_searchbyreg.Text.Trim() == "")
                {
                    lblErr.Text = "Please Enter Reg. No";
                    lblErr.Visible = true;
                    return;
                }
                search = txt_searchbyreg.Text.Trim();
                filterby = "Reg_No='" + search + "'";
            }
            else if (ddl_searchby.SelectedItem.Text.Trim() == "Roll No")
            {
                if (txt_searchbyroll.Text.Trim() == "")
                {
                    lblErr.Text = "Please Enter Roll No";
                    lblErr.Visible = true;
                    return;
                }
                search = txt_searchbyroll.Text.Trim();
                filterby = "Roll_No='" + search + "'";
            }
            string query = string.Empty;
            if (!string.IsNullOrEmpty(filterby))
            {
                query = "select Stud_Name,Roll_No,Reg_No,Batch_Year,degree_code,Current_Semester from Registration where " + filterby;
                ds = d2.select_method_wo_parameter(query, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                batch_year = Convert.ToString(ds.Tables[0].Rows[0]["Batch_Year"]).Trim();
                degree_code = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]).Trim();
                roll_no = Convert.ToString(ds.Tables[0].Rows[0]["Roll_No"]).Trim();
                if (degree_code != "" && batch_year != "" && month != "0" && yr_val != "0")
                {
                    exam_code = d2.GetFunctionv("select exam_code from Exam_Details where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and Exam_year='" + year + "' and Exam_Month='" + month + "'");
                }
                if (rdb_tot.Checked == true)
                {
                    if (exam_code.Trim() != "" && exam_code.Trim() != "0" && roll_no.Trim() != "")
                    {

                        query = "select isnull(s.subjectpriority,'0') as subjectpriority,subject_code,subject_name,s.subject_no,me.exam_code,ed.Exam_year,ed.Exam_Month from mark_entry me,subject s,Exam_Details ed where ed.exam_code=me.exam_code and s.subject_no=me.subject_no and external_mark>='0' and me.exam_code='" + exam_code + "' and roll_no='" + roll_no + "' order by subjectpriority,s.subject_no";
                        dsSubDetails = d2.select_method_wo_parameter(query, "Text");
                    }
                }
                else if (rdb_take.Checked == true)
                {
                    if (!string.IsNullOrEmpty(roll_no.Trim()))
                    {
                        query = "select distinct isnull(s.subjectpriority,'0') as subjectpriority,subject_code,subject_name,s.subject_no,internal_mark,result,ed.Exam_year,ed.Exam_Month,me.exam_code from mark_entry me,subject s,Exam_Details ed where ed.exam_code=me.exam_code and s.subject_no=me.subject_no and isnull(internal_mark,'0')>='-1' and (isnull(internal_mark,'0')<isnull(s.min_int_marks,'0') or isnull(me.total,'0')<isnull(s.mintotal,'0')) and result<>'Pass' and roll_no='" + roll_no + "' and me.subject_no not in (select subject_no from mark_entry where roll_no='" + roll_no + "' and result ='Pass') order by subjectpriority,s.subject_no";
                        dsSubDetails = d2.select_method_wo_parameter(query, "Text");
                    }
                }
                if (dsSubDetails.Tables.Count > 0 && dsSubDetails.Tables[0].Rows.Count > 0)
                {
                    DataTable dtDistinctSubject = new DataTable();
                    dtDistinctSubject = dsSubDetails.Tables[0].DefaultView.ToTable(true, "subject_code", "subject_name", "subject_no");
                    Fpspread3.Visible = true;
                    Fpspread3.Sheets[0].AutoPostBack = false;
                    Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                    Fpspread3.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    Fpspread3.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    Fpspread3.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                    Fpspread3.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
                    Fpspread3.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].RowHeader.Visible = false;
                    Fpspread3.Sheets[0].ColumnHeader.RowCount = 0;
                    Fpspread3.Sheets[0].ColumnCount = 0;
                    Fpspread3.Sheets[0].RowCount = 0;

                    Fpspread3.Sheets[0].RowHeader.Visible = false;
                    //Fpspread1.Sheets[0].AutoPostBack = true;
                    Fpspread3.CommandBar.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.Black;
                    darkstyle.Font.Name = "Book Antiqua";
                    darkstyle.Font.Size = FontUnit.Medium;
                    darkstyle.Font.Bold = true;
                    darkstyle.Border.BorderSize = 0;
                    darkstyle.HorizontalAlign = HorizontalAlign.Center;
                    darkstyle.VerticalAlign = VerticalAlign.Middle;
                    darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                    Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FarPoint.Web.Spread.CheckBoxCellType chktypeall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chktypeall.AutoPostBack = true;
                    FarPoint.Web.Spread.CheckBoxCellType chktype = new FarPoint.Web.Spread.CheckBoxCellType();

                    Fpspread3.Sheets[0].RowCount = 0;
                    Fpspread3.Sheets[0].ColumnCount = 4;
                    Fpspread3.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread3.Sheets[0].ColumnHeader.Columns[0].Width = 50;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread3.Columns[0].Width = 75;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Code";
                    Fpspread3.Columns[1].Width = 200;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
                    Fpspread3.Columns[2].Width = 300;

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
                    Fpspread3.Columns[3].Width = 50;
                    btn_save.Visible = true;
                    Fpspread3.Sheets[0].RowCount = dtDistinctSubject.Rows.Count + 1;
                    Fpspread3.Sheets[0].Cells[0, 3].CellType = chktypeall;
                    Fpspread3.Sheets[0].Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
                    Fpspread3.SaveChanges();
                    for (int r = 0; r < dtDistinctSubject.Rows.Count; r++)
                    {
                        //subject_code,subject_name,s.subject_no
                        Fpspread3.Sheets[0].Cells[r + 1, 0].Text = Convert.ToString(r + 1);
                        Fpspread3.Sheets[0].Cells[r + 1, 0].Font.Bold = true;
                        Fpspread3.Sheets[0].Cells[r + 1, 0].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[r + 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[r + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[r + 1, 0].VerticalAlign = VerticalAlign.Middle;

                        string examCode = string.Empty;
                        DataView dvExamCode = new DataView();
                        dsSubDetails.Tables[0].DefaultView.RowFilter = "subject_no='" + Convert.ToString(dtDistinctSubject.Rows[r]["subject_no"]).Trim() + "'";
                        dvExamCode = dsSubDetails.Tables[0].DefaultView;
                        dvExamCode.Sort = "exam_year desc,exam_month desc";

                        if (dvExamCode.Count > 0)
                        {
                            examCode = Convert.ToString(dvExamCode[0]["exam_code"]).Trim();
                        }

                        Fpspread3.Sheets[0].Cells[r + 1, 1].Text = Convert.ToString(dtDistinctSubject.Rows[r]["subject_code"]).Trim();
                        Fpspread3.Sheets[0].Cells[r + 1, 1].Tag = Convert.ToString(dtDistinctSubject.Rows[r]["subject_no"]).Trim();
                        Fpspread3.Sheets[0].Cells[r + 1, 1].Font.Bold = true;
                        Fpspread3.Sheets[0].Cells[r + 1, 1].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[r + 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[r + 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[r + 1, 1].VerticalAlign = VerticalAlign.Middle;

                        Fpspread3.Sheets[0].Cells[r + 1, 2].Text = Convert.ToString(dtDistinctSubject.Rows[r]["subject_name"]).Trim();
                        Fpspread3.Sheets[0].Cells[r + 1, 2].Tag = examCode;
                        Fpspread3.Sheets[0].Cells[r + 1, 2].Font.Bold = true;
                        Fpspread3.Sheets[0].Cells[r + 1, 2].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[r + 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[r + 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[r + 1, 2].VerticalAlign = VerticalAlign.Middle;

                        Fpspread3.Sheets[0].Cells[r + 1, 3].CellType = chktype;
                        Fpspread3.Sheets[0].Cells[r + 1, 3].Font.Bold = true;
                        Fpspread3.Sheets[0].Cells[r + 1, 3].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].Cells[r + 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[r + 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[r + 1, 3].VerticalAlign = VerticalAlign.Middle;

                    }
                    Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                    Fpspread3.SaveChanges();
                }
                else
                {
                    Fpspread3.Visible = false;
                    btn_save.Visible = false;
                    lblErr.Text = "No Records Found";
                    lblErr.Visible = true;
                    return;
                }
            }
            else
            {
                Fpspread3.Visible = false;
                btn_save.Visible = false;
                lblErr.Text = "No Student Records Were Found";
                lblErr.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void Fpspread3_Command(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Fpspread3.Sheets[0].Cells[0, 3].Value) == 1)
        {
            for (int i = 0; i < Fpspread3.Sheets[0].RowCount; i++)
            {
                Fpspread3.Sheets[0].Cells[i, 3].Value = 1;
            }
        }
        else if (Convert.ToInt32(Fpspread3.Sheets[0].Cells[0, 3].Value) == 0)
        {
            for (int i = 0; i < Fpspread3.Sheets[0].RowCount; i++)
            {
                Fpspread3.Sheets[0].Cells[i, 3].Value = 0;
            }

        }

        Fpspread3.Visible = true;
    }

    protected void btn_save_OnClick(object sender, EventArgs e)
    {
        try
        {
            bool checkflage = false;
            string month = ddl_mm.SelectedItem.Value;
            string year = ddl_yy.SelectedItem.Text;
            string yr_val = ddl_yy.SelectedItem.Value;
            string exam_type = string.Empty;
            string appl_no = string.Empty;
            string today = DateTime.Now.ToString();
            Fpspread3.SaveChanges();
            bool isReTotal = false;
            bool isReVal = false;
            if (rdb_tot.Checked == true)
            {
                exam_type = "2";
                isReTotal = true;
            }
            else if (rdb_take.Checked == true)
            {
                exam_type = "6";
                isReVal = true;
            }
            lblErr.Visible = false;
            if (month.Trim() == "0" || yr_val.Trim() == "0")
            {
                lblErr.Text = "Please Select Month and Year";
                lblErr.Visible = true;
                return;
            }
            if (ddl_searchby.SelectedItem.Text.Trim() == "Reg No")
            {
                if (txt_searchbyreg.Text.Trim() == "")
                {
                    lblErr.Text = "Please Enter Reg. No";
                    lblErr.Visible = true;
                    return;
                }
                search = txt_searchbyreg.Text.Trim();
                filterby = " and Reg_No='" + search + "'";
            }
            else if (ddl_searchby.SelectedItem.Text == "Roll No")
            {
                if (txt_searchbyroll.Text.Trim() == "")
                {
                    lblErr.Text = "Please Enter Roll No";
                    lblErr.Visible = true;
                    return;
                }
                search = txt_searchbyroll.Text.Trim();
                filterby = " and Roll_No='" + search + "'";
            }
            bool isSelected = false;
            for (int r = 1; r < Fpspread3.Sheets[0].RowCount; r++)
            {
                int val = 0;
                int.TryParse(Convert.ToString(Fpspread3.Sheets[0].Cells[r, 3].Value).Trim(), out val);
                if (val == 1)
                {
                    isSelected = true;
                    break;
                }
            }
            if (!isSelected)
            {
                lblErr.Text = "Please Select Atleast One Subjects";
                lblErr.Visible = true;
                return;
            }
            string query = string.Empty;
            if (!string.IsNullOrEmpty(filterby))
            {
                query = "select Stud_Name,Roll_No,Reg_No,Batch_Year,degree_code,Current_Semester,college_code from Registration where exam_flag<>'debar' " + filterby;
                ds = d2.select_method_wo_parameter(query, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                batch_year = Convert.ToString(ds.Tables[0].Rows[0]["Batch_Year"]).Trim();
                degree_code = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]).Trim();
                string collegecode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]).Trim();
                string currentSem = Convert.ToString(ds.Tables[0].Rows[0]["Current_Semester"]).Trim();
                roll_no = Convert.ToString(ds.Tables[0].Rows[0]["Roll_No"]).Trim();

                if (degree_code.Trim() != "" && batch_year.Trim() != "" && month.Trim() != "0" && yr_val.Trim() != "0")
                {
                    exam_code = d2.GetFunctionv("select exam_code from Exam_Details where batch_year='" + batch_year + "' and degree_code='" + degree_code + "' and Exam_year='" + year + "' and Exam_Month='" + month + "'");
                }
                //if (string.IsNullOrEmpty(exam_code) || exam_code == "0")
                //{
                //    string qry = "if not exists(select * from exam_details where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + month + "' and exam_year='" + year + "') insert into exam_details (degree_code,exam_month,exam_year,batch_year,current_semester,coll_code,isSupplementaryExam) values ('" + degree_code + "','" + month + "','" + year + "','" + batch_year + "','" + currentSem + "','" + collegecode + "','0')else update exam_details set isSupplementaryExam='0'  where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + month + "' and exam_year='" + year + "'";
                //    int ins = dirAcc.updateData(qry);
                //    exam_code = dirAcc.selectScalarString("select exam_code from exam_details where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + month + "' and exam_year='" + year + "'");
                //}
                for (int r = 1; r < Fpspread3.Sheets[0].RowCount; r++)
                {
                    int val = 0;
                    int.TryParse(Convert.ToString(Fpspread3.Sheets[0].Cells[r, 3].Value).Trim(), out val);
                    string examCode = Convert.ToString(Fpspread3.Sheets[0].Cells[r, 2].Tag).Trim();
                    //if (string.IsNullOrEmpty(exam_code) || exam_code == "0")
                    //{
                    exam_code = examCode;
                    //}
                    if (!string.IsNullOrEmpty(exam_code) && exam_code != "0")
                    {
                        string q = "if exists (select * from exam_application where roll_no ='" + roll_no + "' and exam_code ='" + exam_code + "' and Exam_type ='" + exam_type + "')update exam_application set applied_date ='" + today + "' where roll_no ='" + roll_no + "' and exam_code ='" + exam_code + "' and Exam_type ='" + exam_type + "' else insert into exam_application (roll_no,Exam_type,applied_date,total_fee,exam_code,extra_fee,fine,cost_appl,cost_mark,LastDate) values ('" + roll_no + "','" + exam_type + "','" + today + "',0,'" + exam_code + "',0,0,0,0,'" + today + "')";
                        int i = dirAcc.updateData(q);
                        appl_no = dirAcc.selectScalarString("select appl_no from exam_application where roll_no ='" + roll_no + "' and Exam_type ='" + exam_type + "' and exam_code ='" + exam_code + "'");
                        if (val == 1)
                        {
                            string subno = Convert.ToString(Fpspread3.Sheets[0].Cells[r, 1].Tag).Trim();
                            if (!string.IsNullOrEmpty(subno))
                            {
                                if (!string.IsNullOrEmpty(appl_no) && appl_no.Trim() != "0")
                                {
                                    string revalcount = string.Empty;
                                   
                                    q = "if not  exists (select * from exam_appl_details where subject_no ='" + subno + "' and appl_no ='" + appl_no + "') insert into exam_appl_details (subject_no,attempts,appl_no,attend) values('" + subno + "',0,'" + appl_no + "',1)  else update exam_appl_details set subject_no ='" + subno + "' where appl_no ='" + appl_no + "' and subject_no ='" + subno + "'";
                                    i = dirAcc.updateData(q);
                                     //added by Mullai
                                    string revct =d2.GetFunctionv("select ISNULL(revaluation_count,'0') as revaluation_count from exam_appl_details where subject_no ='" + subno + "' and appl_no ='" + appl_no + "'");
                                  int revct1 = Convert.ToInt32(revct) + 1;
                                  if (revct1 > 3)
                                  {
                                      lbl_alert1.Text = "Only 3 Revaluation Can Be Applied";
                                      imgdiv2.Visible = true;
                                      return;
                                  }
                                  else
                                  {
                                      string revaluationct = "update exam_appl_details set revaluation_count='" + revct1 + "'  where appl_no ='" + appl_no + "' and subject_no ='" + subno + "'";

                                      i = dirAcc.updateData(revaluationct);
                                  }
                                    //**
                                    
                                }
                            }
                            if (i == 1)
                            {
                                checkflage = true;
                            }
                        }
                    }
                }
                if (checkflage == true)
                {
                    lbl_alert1.Text = "Saved Successfully";
                    imgdiv2.Visible = true;
                    return;
                }
                else
                {
                    lbl_alert1.Text = "Not Saved";
                    imgdiv2.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        lbl_alert1.Text = string.Empty;
        imgdiv2.Visible = false;
    }

    protected void rdb_tot_CheckedChanged(object sender, EventArgs e)
    {
        btn_save.Visible = false;
        Fpspread3.Visible = false;
    }

    protected void rdb_take_CheckedChanged(object sender, EventArgs e)
    {
        btn_save.Visible = false;
        Fpspread3.Visible = false;
    }

}