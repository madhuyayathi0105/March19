using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using InsproDataAccess;
using System.Data.OleDb;

public partial class Internal : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection d_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection myconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection funconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection markcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection yrcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Attcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    InsproDirectAccess dir = new InsproDirectAccess();
    SqlDataReader stfdr;
    SqlCommand stfcmd;
    SqlCommand cmd;

    DAccess2 da = new DAccess2();
    DataSet dsHeader = new DataSet();
    DataSet stafsub = new DataSet();

    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();

    DataTable dtsub = new DataTable();
    DataRow drow;
    DataRow drtest;
    DataTable dttst = new DataTable();
    static DataTable dtmrk = new DataTable();
    DataRow drmrk;

    static string maximum_mark = string.Empty;
    static string minimum_mark = string.Empty;

    Institution SchoolCollege;
    static Dictionary<string, string> dicstd = new Dictionary<string, string>();
    static Dictionary<int, string> diccellnote = new Dictionary<int, string>();
    static Dictionary<string, string> dicsub = new Dictionary<string, string>();
    static Dictionary<int, string> dicmaxmrk = new Dictionary<int, string>();
    static Dictionary<int, string> dicschsub = new Dictionary<int, string>();
    static Dictionary<string, string> dicschsub1 = new Dictionary<string, string>();
    static Dictionary<string, string> dicschsub2 = new Dictionary<string, string>();
    bool Cellclick;
    bool Cellclick1;
    public bool d_check;
    bool serialflag;
    bool saveflag = false;
    static bool forschoolsetting = false;
    int dicsubval = 0;
    static int dtmrkcount = 0;
    static int dtmrkcoutcol = 0;

    string strdayflag;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string Att_mark;
    string Attvalue = string.Empty;
    string rollno = string.Empty;
    string qry = string.Empty;
    static string datelocksetting = string.Empty;
    string Str_ExamType = string.Empty;
    string GetCellNote = string.Empty;
    static string grouporusercode = string.Empty;
    bool isBasedOnBatchRights = false;
    byte schoolOrCollege = 0;
    string Btach_Year_Val = string.Empty;
    string Semester_Val = string.Empty;
    string Degree_Code_Val = string.Empty;

    DataTable dtexport = new DataTable();
    DataRow drexport;

    //TextBox txtName = new TextBox();
    static string testname = string.Empty;
    bool lockdatechk = false;
    static int datechk = 0;
    static int datch1 = 0;
    static int datelckval = 0;

    static Dictionary<int, int> datchk = new Dictionary<int, int>();


    public DataSet Bind_Degree(string college_code, string user_code)
    {
        qry = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + user_code + "'";
        DataSet ds = new DataSet();
        ds = da.select_method_wo_parameter(qry, "Text");
        return ds;
    }

    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        qry = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + user_code + "'";
        DataSet ds = new DataSet();
        ds = da.select_method_wo_parameter(qry, "Text");
        return ds;
    }

    public bool daycheck(int CriteriaNo)
    {
        bool daycheck = false;
        string curdate, Dateval;
        int total, k;
        string[] ddate = new string[100];
        curdate = DateTime.Today.ToString();
        qry = "select Clock,LastDate from CriteriaforInternal where Criteria_no='" + CriteriaNo + "' and Clock = '1' ";
        DataSet ds = new DataSet();
        ds = da.select_method_wo_parameter(qry, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i][0].ToString() != null && ds.Tables[0].Rows[i][1].ToString() != null)
                {
                    if (ds.Tables[0].Rows[i][0].ToString().Trim().ToLower() == "true")
                    {
                        Dateval = ds.Tables[0].Rows[i][1].ToString();
                        string[] sel_date12 = Dateval.Split(new Char[] { ' ' });
                        string[] sel_date13 = curdate.Split(new Char[] { ' ' });
                        TimeSpan t = Convert.ToDateTime(sel_date13[0]).Subtract(Convert.ToDateTime(sel_date12[0]));
                        long days = t.Days;
                        if (days >= 0)
                        {
                            daycheck = false;
                        }
                        else
                        {
                            daycheck = true;
                        }
                    }
                    else
                    {
                        daycheck = true;
                    }
                }
                else
                {
                    daycheck = true;
                }
            }
        }
        else
        {
            daycheck = true;
        }
        return daycheck;
    }

    //protected override void Render(System.Web.UI.HtmlTextWriter writer)
    //{
    //    Control cntUpdateBtn = FpSettings.FindControl("Update");
    //    Control cntCancelBtn = FpSettings.FindControl("Cancel");
    //    Control cntCopyBtn = FpSettings.FindControl("Copy");
    //    Control cntCutBtn = FpSettings.FindControl("Clear");
    //    Control cntPasteBtn = FpSettings.FindControl("Paste");
    //    //Control cntPageNextBtn = FpSpread1.FindControl("Next");
    //    //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
    //    Control cntPagePrintBtn = FpSettings.FindControl("Print");
    //    if ((cntUpdateBtn != null))
    //    {
    //        TableCell tc = (TableCell)cntUpdateBtn.Parent;
    //        TableRow tr = (TableRow)tc.Parent;
    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntCancelBtn.Parent;
    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntCopyBtn.Parent;
    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntCutBtn.Parent;
    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntPasteBtn.Parent;
    //        tr.Cells.Remove(tc);
    //     //   tc = (TableCell)cntPagePrintBtn.Parent;
    //     //   tr.Cells.Remove(tc);
    //    }
    //    Response.BufferOutput = true;
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {

        try
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
            lblexcelerror.Visible = false;
            lblnorec.Visible = false;
            lblnorec.Text = string.Empty;
            Label2.Visible = false;
            lblnote2.Visible = false;
            lblNote3.Visible = false;
            lbltab.Visible = false;
            Save.Visible = false;
            pHeaderEntry.Visible = false;
            pHeaderReport.Visible = false;
            pHeaderSettings.Visible = false;
            Save.Enabled = false;
            Delete.Enabled = false;
            // Exit1.Visible = false;
            // Buttonexit.Visible = false;
            //FpSettings.CommandBar.Visible = false;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
            }
            else
            {
                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {
                isBasedOnBatchRights = false;
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string batchYearSettings = da.GetFunction("select value from Master_Settings where settings='CAM Entry Based On Batch And Section Rights' and " + grouporusercode + "");
                    if (batchYearSettings.Trim() == "1")
                        isBasedOnBatchRights = true;
                }
                txt_RetestMin.Text = string.Empty;
                chkretest.Checked = false;
                lblRetestMin.Visible = false;
                txt_RetestMin.Visible = false;
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = da.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables.Count > 0 && schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]).Trim();
                    if (schoolvalue.Trim() == "0")
                    {
                        forschoolsetting = true;
                        lblBatch.Text = "Year";
                        lblDegree.Text = "School Type";
                        lblBranch.Text = "Standard";
                        lblSemYr.Text = "Term";
                        lblStripHead.InnerHtml = "Test Mark Entry";
                        lblStripHead.Attributes.Add("style", "color:green;");
                        lblDegree.Attributes.Add("style", "width: 100px;");
                        lblBranch.Attributes.Add("style", "width: 52px;");
                        //ddlBranch.Attributes.Add("style", "height: 21px;");
                        lblSemYr.Attributes.Add("style", "width: 33px;");
                        ddlSemYr.Attributes.Add("style", "width: 44px; ");
                        lblSec.Attributes.Add("style", "width: 30px;");
                        ddlSec.Attributes.Add("style", "width: 47px;");
                        btnGo.Attributes.Add("style", "width: 40px");
                    }
                    else
                    {
                        forschoolsetting = false;
                    }
                }
                Delete.Visible = false;
                //FpReport.Visible = false;
                //FpSettings.Visible = false;
                GridView3.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                // Buttonexit.Visible = false;
                btnok.Visible = false;
                chkmarkattendance.Visible = false;
                chkretest.Visible = false;
                lblselectstaff.Visible = false;
                ddlstaffname.Visible = false;
                //btnExcel.Visible = false;   
                //ddlBatch.Items.Insert(0, new ListItem("- -Select- -", "-1"));
                //ddlDegree.Items.Insert(0, new ListItem("- -Select- -", "-1"));
                //ddlBranch.Items.Insert(0, new ListItem("- -Select- -", "-1"));
                //ddlSec.Items.Insert(0, new ListItem("- -Select- -", "-1"));
                //ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
                // FpSettings.Sheets[0].PageSize = 10;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                // FpSettings.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                // FpSettings.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                // FpSettings.Pager.Align = HorizontalAlign.Right;
                //FpSettings.Pager.Font.Bold = true;
                // FpSettings.Pager.Font.Name = "Arial";
                // FpSettings.Pager.ForeColor = Color.DarkGreen;
                // FpSettings.Pager.BackColor = Color.Beige;
                // FpSettings.Pager.BackColor = Color.AliceBlue;
                // FpSettings.Pager.PageCount = 5;
                GridView3.ForeColor = Color.Black;
                GridView3.BackColor = Color.Beige;
                GridView3.BackColor = Color.AliceBlue;
                GridView3.Font.Name = "Arial";
                GridView3.Font.Bold = true;
                //  GridView3.HeaderRow.ForeColor = Color.BlueViolet;


                // FpEntry.ActiveSheetView.AutoPostBack = true;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Font.Name = "Book Antiqua";
                style.HorizontalAlign = HorizontalAlign.Center;
                style.ForeColor = Color.Black;
                style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //FpEntry.Sheets[0].ColumnHeader.DefaultStyle = style;
                //FpReport.Sheets[0].ColumnHeader.DefaultStyle = style;
                // FpSettings.Sheets[0].ColumnHeader.DefaultStyle = style;
                //fpmarkimport.Sheets[0].ColumnHeader.DefaultStyle = style;
                // FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                // FpReport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                // FpSettings.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                // fpmarkimport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                // FpEntry.Sheets[0].AllowTableCorner = true;
                // FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                // FpEntry.Sheets[0].SheetCorner.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                Cellclick1 = false;
                //------------------------------------------------query for the grpvalue
                myconn.Open();
                string sqlgrop = string.Empty;
                if (Session["collegecode"] != null && Convert.ToString(Session["collegecode"]).Trim() != "")
                {
                    sqlgrop = "select linkvalue from inssettings where college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and linkname='CAM Group'";
                    //SqlCommand cmdgrp = new SqlCommand(sqlgrop, myconn);
                    //SqlDataReader cmdgrpdr;
                    //cmdgrpdr = cmdgrp.ExecuteReader();
                    //cmdgrpdr.Read();
                    DataSet dsCAMGrp = new DataSet();
                    dsCAMGrp = da.select_method_wo_parameter(sqlgrop, "text");
                    if (dsCAMGrp.Tables.Count > 0 && dsCAMGrp.Tables[0].Rows.Count > 0)
                    {
                        //string linkvalue =string.Empty;
                        //linkvalue = Convert.ToInt32(cmdgrpdr["linkvalue"]).ToString();
                        if (dsCAMGrp.Tables[0].Rows[0]["linkvalue"].ToString() == "1")
                        {
                            chkGrp.Visible = true;
                            chkGrp.Enabled = true;
                            chkGrp.Checked = false;
                            ddlGrp.Visible = true;
                        }
                        else
                        {
                            chkGrp.Visible = false;
                            ddlGrp.Visible = false;
                        }
                    }
                    else
                    {
                        chkGrp.Visible = false;
                        ddlGrp.Visible = false;
                    }
                }
                string staff_code = string.Empty;
                string sqlstf = string.Empty;
                string staffname = string.Empty;
                string sqldescode = string.Empty;
                string sqldesname = string.Empty;
                string stfdesname = string.Empty;
                string stfdescode = string.Empty;
                string sqldepname = string.Empty;
                string stfdepname = string.Empty;
                staff_code = Convert.ToString(Session["staff_code"]).Trim();
                //readcon.Close();
                readcon.Open();
                if (staff_code != "")
                {
                    // FpEntry.Visible = true;
                    GridView1.Visible = true;
                }
                else
                {
                    //FpEntry.Visible = false;
                    GridView1.Visible = false;
                    chkmarkattendance.Visible = false;
                }
                Cellclick = false;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                // FpSettings.SaveChanges();


                //----------------------------------------------------clmn cnt & row cnt for fpreport
                //FpReport.Sheets[0].ColumnCount = 12;
                //FpReport.Sheets[0].SheetCorner.RowCount = 2;
                //---------------------------------------------------   -to lock the particular column
                //FpReport.Columns[1].Locked = true;
                //FpReport.Sheets[0].Columns[10].Locked = true;
                //FpReport.Sheets[0].Columns[11].Locked = true;
                //--------------------------------------------------to set width for partclr colmn in fpreport
                //FpReport.Sheets[0].Columns[0].Width = 60;
                //FpReport.Sheets[0].Columns[1].Width = 150;
                //FpReport.Sheets[0].Columns[2].Width = 50;
                //FpReport.Sheets[0].Columns[3].Width = 50;
                //FpReport.Sheets[0].Columns[4].Width = 50;
                //FpReport.Sheets[0].Columns[5].Width = 50;
                //FpReport.Sheets[0].Columns[6].Width = 50;
                //FpReport.Sheets[0].Columns[7].Width = 50;
                //FpReport.Sheets[0].Columns[8].Width = 50;
                //FpReport.Sheets[0].Columns[9].Width = 50;
                //FpReport.Sheets[0].Columns[10].Width = 80;
                //FpReport.Sheets[0].Columns[11].Width = 80;
                //--------------------------------------------------------to set the header name for fpentry  
                //FarPoint.Web.Spread.TextCellType lblcell = new FarPoint.Web.Spread.TextCellType();
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Test";
                //FpReport.Sheets[0].Columns[1].CellType = lblcell;
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "ExamDate";
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Month";
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "EntryDate";
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Date";
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Month";
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Duration";
                //// FpReport.Sheets[0].ColumnHeader.Cells[0, 9].Text = "MaxMark";
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 10].Text = "MaxMark";
                //FpReport.Sheets[0].Columns[10].CellType = lblcell;
                //FpReport.Sheets[0].ColumnHeader.Cells[0, 11].Text = "MinMark";
                //FpReport.Sheets[0].Columns[11].CellType = lblcell;
                //---------------------------------------------------------------- to set the style property for the fpreport
                //FpReport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                //FpReport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                //FpReport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                //FpReport.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //FpReport.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
                //FpReport.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
                //FpReport.SheetCorner.Cells[0, 0].Font.Bold = true;
                //FpReport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                //FpReport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                //FpReport.Sheets[0].DefaultStyle.Font.Bold = false;
                //------------------------------------------------- to set the clmn cnt for the fpsettings
                // FpSettings.Sheets[0].ColumnCount = 5;
                //   //--------------------------------------------------------to set the header name for fpsettings  
                //dtmrk.Columns.Add("RollNo");
                //dtmrk.Columns.Add("RegNo");
                //dtmrk.Columns.Add("Student Type");
                //dtmrk.Columns.Add("AppNo");

                // GridView3.Columns[4].Visible = false;

                //FarPoint.Web.Spread.TextCellType lblccell = new FarPoint.Web.Spread.TextCellType();
                //FpSettings.Sheets[0].ColumnHeader.Cells[0, 0].Text = "RollNo";
                //FpSettings.Sheets[0].Columns[0].CellType = lblccell;
                //FpSettings.Sheets[0].ColumnHeader.Cells[0, 1].Text = "RegNo";
                //FpSettings.Sheets[0].Columns[1].CellType = lblccell;
                //FpSettings.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                //FpSettings.Sheets[0].Columns[2].CellType = lblccell;
                //FpSettings.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Type";
                //FpSettings.Sheets[0].Columns[3].CellType = lblccell;
                //FpSettings.Sheets[0].ColumnHeader.Cells[0, 4].Text = "AppNo";
                //FpSettings.Sheets[0].Columns[4].CellType = lblccell;
                //FpSettings.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                //------------------------------------------------ to set width for partclr colmn in fpsettings
                //FpSettings.Sheets[0].Columns[0].Width = 120;
                //FpSettings.Sheets[0].Columns[1].Width = 120;
                //FpSettings.Sheets[0].Columns[2].Width = 200;
                //FpSettings.Sheets[0].Columns[3].Width = 130;


                //------------------------------------------------- to set the style property for fpsettings


                //FpSettings.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                // FpSettings.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                //FpSettings.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                //FpSettings.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                //FpSettings.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
                //FpSettings.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
                //FpSettings.SheetCorner.Cells[0, 0].Font.Bold = true;
                //FpSettings.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                //FpSettings.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                //FpSettings.Sheets[0].DefaultStyle.Font.Bold = false;
                //FpSettings.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                //FpSettings.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                //FpSettings.Columns[0].Locked = true;
                //FpSettings.Columns[1].Locked = true;
                //FpSettings.Columns[2].Locked = true;
                //FpSettings.Columns[3].Locked = true;
                //FpSettings.Columns[4].Locked = true;


                //FpSettings.ActiveSheetView.Columns[5].Font.Name = "Arial";
                //ddlGrp.Enabled = false;
                Session["StaffSelector"] = "0";
                string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
                if (minimumabsentsms.Trim() == "1")
                {
                    Session["StaffSelector"] = "1";
                }
                if (staff_code == null || staff_code == "")
                {
                    RequiredFieldValidator1.Visible = true;
                    RequiredFieldValidator2.Visible = true;
                    RequiredFieldValidator3.Visible = true;
                    RequiredFieldValidator4.Visible = true;
                    RequiredFieldValidator5.Visible = true;
                    //Panel1.Visible = true;
                    pnlHeadingCAM.Visible = true;
                    ddlBatch.Visible = true;
                    ddlDegree.Visible = true;
                    ddlBranch.Visible = true;
                    ddlSemYr.Visible = true;
                    ddlSec.Visible = true;
                    // pnlEntry.Visible = true;
                    //  pnlReport.Visible = true;
                    //  pnlSettings.Visible = true;
                    // FpEntry.Sheets[0].Visible = true;
                    GridView1.Visible = true;
                    // FpReport.Sheets[0].Visible = true;
                    // FpSettings.Sheets[0].Visible = true;
                    GridView3.Visible = true;
                    ddlDegree.AutoPostBack = true;
                    ddlBranch.AutoPostBack = true;
                    ddlSemYr.AutoPostBack = true;
                    ddlSec.AutoPostBack = true;
                    //FpEntry.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                    BindBatch(); // Binding Batch in drop down list
                    BindDegree();
                    if (ddlDegree.Items.Count > 0)
                    {
                        ////'-------------branch loading
                        //string course_id = ddlDegree.SelectedValue.ToString();
                        ////string sem = ddlSem.SelectedValue.ToString();
                        //string collegecode = Session["collegecode"].ToString();
                        //string usercode = Session["UserCode"].ToString();//Session["UserCode"].ToString();
                        //DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
                        // if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        //{
                        //    ddlBranch.DataSource = ds;
                        //    ddlBranch.DataTextField = "Dept_Name";
                        //    ddlBranch.DataValueField = "degree_code";
                        //    ddlBranch.DataBind();
                        //    //     ddlBranch.Items.Insert(0, new ListItem("- -Select- -", "-1"));
                        //}
                        ////'------------------------------semester loading
                        ////Get_Semester();
                        bindbranch();// added by sridhar 06 sep 2014
                        bindsem();
                        //'---------------------------- section loading
                        BindSectionDetail();
                    }
                    else
                    {
                        lblErrorMsg.Text = "Give degree rights to staff";
                        lblErrorMsg.Visible = true;
                    }
                }
                else
                {
                    pHeaderEntry.Visible = true;
                    //Panel1.Visible = false;
                    pnlHeadingCAM.Visible = false;
                    ddlBatch.Visible = false;
                    ddlDegree.Visible = false;
                    ddlBranch.Visible = false;
                    ddlSemYr.Visible = false;
                    ddlSec.Visible = false;
                    lblBatch.Visible = false;
                    lblDegree.Visible = false;
                    lblBranch.Visible = false;
                    lblSemYr.Visible = false;
                    lblSec.Visible = false;
                    pnlEntry.Visible = true;
                    pnlReport.Visible = true;
                    pnlSettings.Visible = true;
                    //Table1.Visible = false;
                    btnGo.Visible = false;
                    // FpEntry.Sheets[0].Visible = true;
                    GridView1.Visible = true;
                    // FpReport.Sheets[0].Visible = true;
                    // FpSettings.Sheets[0].Visible = true;
                    GridView3.Visible = true;
                    // FpEntry.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                }
                myconn.Close();
                //********************************************* to display the subject details in the fpentry for the individual staff login
                string userId = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    userId = Convert.ToString(Session["group_code"]).Trim().Split(';')[0];
                }
                else
                {
                    userId = Convert.ToString(Session["usercode"]).Trim();
                }
                myconn.Open();
                if (staff_code != "")
                {
                    string sqlstaff = string.Empty;
                    int rowcnt;
                    //aruna on 20/09/20111 sqlstaff = "select distinct s.subject_no,s.subject_name,s.syll_code,st.batch_year,semester,sections,degree_code from subject s,syllabus_master sy,staff_selector st where s.syll_code=sy.syll_code and st.subject_no=s.subject_no and st.batch_year=sy.batch_year  and staff_code='" + Session["staff_code"].ToString() + "' order by st.batch_year,degree_code,semester,sections ";
                    //sqlstaff = "select distinct s.subject_no,s.subject_name,s.subject_code,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (st.sections =isnull(r.sections,'-1')) and staff_code='" + Session["staff_code"].ToString() + "' order by st.batch_year,sy.degree_code,semester,st.sections ";

                    string qryBatchBasedSetting = string.Empty;
                    if (isBasedOnBatchRights)
                    {
                        qryBatchBasedSetting = " and r.Batch_Year in(select Batch_Year from tbl_attendance_rights where user_id='" + Convert.ToString(userId).Trim() + "')";
                    }

                    sqlstaff = "select distinct s.subject_no as subject_no,s.subject_name as Subject,s.subject_code  as Subject_Code,s.syll_code,st.batch_year,semester,st.sections as Section,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (LTRIM(RTRIM(ISNULL(st.sections,''))) =LTRIM(RTRIM(ISNULL(r.sections,''))) or LTRIM(RTRIM(ISNULL(st.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections, '')))) and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and st.staff_code='" + Session["staff_code"].ToString() + "' " + qryBatchBasedSetting + " order by st.batch_year,sy.degree_code,semester,st.sections ";
                    //********************Added By Srinath For Student Staff Seletor************************************
                    if (Session["StaffSelector"].ToString() == "1")
                    {
                        if (Session["Staff_Code"] != null)
                        {
                            if (Session["Staff_Code"].ToString().Trim() != "" && Session["Staff_Code"].ToString().Trim() != "0")
                            {
                                //  sqlstaff = "select distinct s.subject_no,s.subject_name,s.subject_code,s.syll_code,st.batch_year,sy.semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb,subjectChooser sc where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (st.sections =isnull(r.sections,'-1') or st.sections=ISNULL(NULLIF(r.sections, ''),'-1')) and st.staff_code='" + Session["staff_code"].ToString() + "' and sc.staffcode=st.staff_code and sc.roll_no=sc.roll_no and sc.subject_no=st.subject_no and sy.semester=sc.semester and sb.subType_no=sc.subtype_no and s.subject_no=sc.subject_no order by st.batch_year,sy.degree_code,sy.semester,st.sections ";
                                sqlstaff = "select distinct s.subject_no as subject_no,s.subject_name as Subject,s.subject_code as Subject_Code,s.syll_code,st.batch_year,sy.semester,st.sections as Section,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb,subjectChooser sc where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (LTRIM(RTRIM(ISNULL(st.sections,''))) =LTRIM(RTRIM(ISNULL(r.sections,''))) or LTRIM(RTRIM(ISNULL(st.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections, '')))) and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'  and st.staff_code = '" + Session["staff_code"].ToString() + "' and sc.staffcode like '%" + Session["staff_code"].ToString() + "%' and  sc.roll_no=sc.roll_no and sc.subject_no=st.subject_no and sy.semester=sc.semester and sb.subType_no=sc.subtype_no and s.subject_no=sc.subject_no order by st.batch_year,sy.degree_code,sy.semester,st.sections "; // Added by jairam 07-03-2015
                            }
                        }
                    }
                    //******************************************End*************************************************************
                    //**************************************ATTENDANCE BASED*****************************************************
                    string staffsubattendancesettings = da.GetFunction("select value from Master_Settings where settings='Cam Entry Staff'");
                    string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
                    //******************************************End*************************************************************
                    stafsub = da.select_method_wo_parameter(sqlstaff, "text");
                    SqlCommand staffcmd = new SqlCommand(sqlstaff, myconn);
                    SqlDataReader dr;
                    dr = staffcmd.ExecuteReader();
                    dtsub.Columns.Add("Batch_Year");
                    dtsub.Columns.Add("degree");
                    dtsub.Columns.Add("degree_code");
                    dtsub.Columns.Add("Semester");
                    dtsub.Columns.Add("Section");
                    dtsub.Columns.Add("Subject");
                    dtsub.Columns.Add("Subject_Code");
                    dtsub.Columns.Add("Subject_no");

                    while (dr.Read())
                    {
                        drow = dtsub.NewRow();
                        string current_sem = string.Empty;
                        current_sem = GetFunction("select distinct current_semester from registration where degree_code='" + dr["degree_code"].ToString() + "' and batch_year='" + dr["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar'");
                        if (Convert.ToString(current_sem) == Convert.ToString(dr["semester"]))
                        {
                            Boolean staffflag = false;
                            if (staffsubattendancesettings.Trim().ToString() == "1")
                            {
                                string strsecst = dr["sections"].ToString();
                                if (strsecst.Trim() != "" && strsecst.Trim() != "-1")
                                {
                                    strsecst = " and Sections='" + dr["sections"].ToString() + "'  ";
                                }
                                string getsub = dr["subject_no"].ToString().Trim();
                                string strsecval = "select top 1 FromDate,* from Semester_Schedule where degree_code='" + dr["degree_code"].ToString() + "' and batch_year='" + dr["batch_year"].ToString() + "' and semester='" + current_sem + "' " + strsecst + " and FromDate<='" + DateTime.Now.ToString("MM/dd/yyyy") + "' order by Semester_Schedule.FromDate desc";
                                DataSet dsattndance = da.select_method_wo_parameter(strsecval, "text");
                                if (dsattndance.Tables[0].Rows.Count > 0)
                                {
                                    for (int day_lp = 0; day_lp < 7; day_lp++)
                                    {
                                        string strday = Days[day_lp].ToString();
                                        for (int h = 1; h < 10; h++)
                                        {
                                            string gethour = dsattndance.Tables[0].Rows[0][strday + h].ToString();
                                            string[] spsub = gethour.Split(';');
                                            for (int sa = 0; sa <= spsub.GetUpperBound(0); sa++)
                                            {
                                                string[] substaf = spsub[sa].Split('-');
                                                for (int stf = 1; stf < substaf.GetUpperBound(0); stf++)
                                                {
                                                    if (substaf[0].ToString().Trim() == getsub && substaf[stf].ToString().Trim().ToLower() == Session["staff_code"].ToString().Trim().ToLower())
                                                    {
                                                        staffflag = true;
                                                        stf = substaf.GetUpperBound(0) + 1;
                                                        sa = spsub.GetUpperBound(0) + 1;
                                                        h = 11;
                                                        day_lp = 10;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                staffflag = true;
                            }
                            if (staffflag == true)
                            {
                                // FpEntry.Sheets[0].RowCount += 1;
                                if ((Session["collegecode"] != "") && dr["degree_code"].ToString() != "")
                                {
                                    string sqlstr = string.Empty;
                                    sqlstr = "select course_name + '-'+dept_acronym from degree d,course c,department dp where d.course_id=c.course_id and d.dept_code=dp.dept_code and degree_code= '" + dr["degree_code"].ToString() + "'";
                                    string degree = string.Empty;
                                    degree = GetFunction(sqlstr.ToString());
                                    drow["Batch_Year"] = dr["batch_year"].ToString();
                                    drow["degree"] = degree.ToString();
                                    drow["degree_code"] = dr["degree_code"].ToString();
                                    if (dr["semester"] == "-1")
                                    {
                                        drow["Semester"] = "";
                                    }
                                    else
                                    {
                                        drow["Semester"] = dr["semester"].ToString();
                                    }
                                    if (dr["section"] == "-1")
                                    {
                                        drow["Section"] = " ";
                                    }
                                    else
                                    {
                                        drow["Section"] = dr["section"].ToString();
                                    }
                                    drow["Subject"] = dr["subject"].ToString();
                                    drow["Subject_Code"] = dr["subject_code"].ToString();
                                    drow["Subject_no"] = dr["subject_no"].ToString();
                                    dtsub.Rows.Add(drow);
                                    GridView1.DataSource = dtsub;
                                    GridView1.DataBind();

                                    //added by srinath 15/5/2014
                                    string markattendance = da.GetFunction("select value from Master_Settings where settings='cam mark attendance'");
                                    if (markattendance.Trim() != "0" && markattendance.Trim() != "" && markattendance != null)
                                    {
                                        chkmarkattendance.Visible = true;
                                    }
                                    else
                                    {
                                        chkmarkattendance.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                }
                // FpEntry.Sheets[0].AutoPostBack = true;
                //---------------------------------------------------------------Query for the master settings
                if (Session["usercode"] != "")
                {
                    string Master = string.Empty;
                    Master = "select * from Master_Settings where " + grouporusercode + "";
                    readcon.Close();
                    readcon.Open();
                    SqlDataReader mtrdr;
                    SqlCommand mtcmd = new SqlCommand(Master, readcon);
                    mtrdr = mtcmd.ExecuteReader();
                    strdayflag = string.Empty;
                    while (mtrdr.Read())
                    {
                        if (mtrdr.HasRows == true)
                        {
                            if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Rollflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Regflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Studflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                            {
                                if (strdayflag != "" && strdayflag != "\0")
                                {
                                    strdayflag = strdayflag + " or registration.Stud_Type='Day Scholar'";
                                }
                                else
                                {
                                    strdayflag = " and (registration.Stud_Type='Day Scholar'";
                                }
                            }
                            if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                            {
                                if (strdayflag != "" && strdayflag != "\0")
                                {
                                    strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                                }
                                else
                                {
                                    strdayflag = " and (registration.Stud_Type='Hostler'";
                                }
                            }
                            if (mtrdr["settings"].ToString() == "Regular")
                            {
                                regularflag = "and ((registration.mode=1)";
                                // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                            }
                            if (mtrdr["settings"].ToString() == "Lateral")
                            {
                                if (regularflag != "")
                                {
                                    regularflag = regularflag + " or (registration.mode=3)";
                                }
                                else
                                {
                                    regularflag = regularflag + " and ((registration.mode=3)";
                                }
                                //Session["strvar"] = Session["strvar"] + " and (mode=3)";
                            }
                            if (mtrdr["settings"].ToString() == "Transfer")
                            {
                                if (regularflag != "")
                                {
                                    regularflag = regularflag + " or (registration.mode=2)";
                                }
                                else
                                {
                                    regularflag = regularflag + " and ((registration.mode=2)";
                                }
                            }
                            if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                            {
                                genderflag = " and (sex='0'";
                            }
                            if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                            {
                                if (genderflag != "" && genderflag != "\0")
                                {
                                    genderflag = genderflag + " or sex='1'";
                                }
                                else
                                {
                                    genderflag = " and (sex='1'";
                                }
                            }
                        }
                    }
                    if (strdayflag != "")
                    {
                        strdayflag = strdayflag + ")";
                    }
                    Session["strvar"] = strdayflag;
                    if (regularflag != "")
                    {
                        regularflag = regularflag + ")";
                    }
                    Session["strvar"] = Session["strvar"] + regularflag;
                    if (genderflag != "")
                    {
                        genderflag = genderflag + ")";
                    }
                    Session["strvar"] = Session["strvar"] + regularflag + genderflag;
                }
            }
            //FpSettings.SaveChanges();
            // if (FpEntry.Sheets[0].RowCount == 0)
            if (stafsub.Tables.Count < 0)
            {
                //FpEntry.Visible = false;
                GridView1.Visible = false;
            }
            else
            {
                //  FpEntry.Visible = true;
                GridView1.Visible = true;
            }
        }
        catch
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlBatch.Items.Clear();
            string Master1 = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                Master1 = group.Split(';')[0];
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
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(Master1.Trim()) && !string.IsNullOrEmpty(collegecode))
            {
                string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "'";
                ds = da.select_method_wo_parameter(strbinddegree, "Text");
            }
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

    public void BindDegree()
    {
        string college_code = Convert.ToString(Session["collegecode"]).Trim();
        string query = string.Empty;
        ddlDegree.Items.Clear();
        string usercode = Convert.ToString(Session["usercode"]).Trim();
        string singleuser = Convert.ToString(Session["single_user"]).Trim();
        string group_user = Convert.ToString(Session["group_code"]).Trim();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' ";
        }
        else
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
        }
        DataSet ds = new DataSet();
        ds.Clear();
        ds = da.select_method_wo_parameter(query, "Text");
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
            string singleuser = Convert.ToString(Session["single_user"]).Trim();
            string group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            string course_id = string.Empty;// ddlDegree.SelectedValue.ToString();
            if (ddlDegree.Items.Count > 0)
            {
                course_id = Convert.ToString(ddlDegree.SelectedValue).Trim();
                string query = string.Empty;
                if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "'";
                }
                else
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
                }
                ds = da.select_method_wo_parameter(query, "Text");
            }
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

    public void bindsem()
    {
        //--------------------semester load
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        string query = string.Empty;
        DataSet ds = new DataSet();
        if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
        {
            query = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlBatch.SelectedItem.Text).Trim() + "' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
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
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            if (ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
            {
                query = "select distinct duration,first_year_nonsemester  from degree where degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'";
                ddlSemYr.Items.Clear();
                ds = new DataSet();
                ds.Clear();
                ds = da.select_method_wo_parameter(query, "Text");
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
        //     ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
    }

    public void BindSectionDetail()
    {
        DataSet ds = new DataSet();
        ddlSec.Items.Clear();
        if (ddlBranch.Items.Count > 0 && ddlBatch.Items.Count > 0)
        {
            string branch = Convert.ToString(ddlBranch.SelectedValue).Trim();
            string batch = Convert.ToString(ddlBatch.SelectedValue).Trim();
            string query = "select distinct sections from registration where batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() == "")
            {
                ddlSec.Enabled = false;
                RequiredFieldValidator5.Visible = false;
            }
            else
            {
                ddlSec.Enabled = true;
                //RequiredFieldValidator5.Visible = true;
            }
        }
        else
        {
            ddlSec.Enabled = false;
            RequiredFieldValidator5.Visible = false;
        }
    }

    public void Get_Semester()
    {
        bool first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        ddlSemYr.Items.Clear();
        //int typeval = 4;
        if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
        {
            string batch = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            string collegecode = Convert.ToString(Session["collegecode"]).Trim();
            string degree = Convert.ToString(ddlBranch.SelectedValue).Trim();
            batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
            //Session["collegecode"].ToString();
            DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (int i = 1; i <= duration; i++)
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
                //ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
            }
        }
    }

    public void clear()
    {
        ddlSemYr.Items.Clear();
        // ddlSec.Items.Clear();
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblRetestMin.Visible = false;
        txt_RetestMin.Visible = false;
        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;
        pHeaderSettings.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        // Buttonexit.Visible = false;
        btn_import.Visible = false;
        Delete.Visible = false;
        Delete.Enabled = false;
        Save.Visible = false;
        Save.Enabled = false;
        Label2.Visible = false;
        lblnote2.Visible = false;
        lblNote3.Visible = false;
        btnok.Visible = false;
        //Exit1.Visible = false;
        chkretest.Visible = false;
        lblselectstaff.Visible = false;
        ddlstaffname.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        lblPageSearch.Visible = false;
        lblErrorMsg.Visible = false;
        //BindBatch(); // Binding Batch in drop down list
        BindDegree();
        if (ddlDegree.Items.Count > 0)
        {
            bindbranch();
            bindsem();
            BindSectionDetail();
        }
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            bindsem();
        }
        //ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        //ddlSec.SelectedIndex = -1;
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblRetestMin.Visible = false;
        txt_RetestMin.Visible = false;
        // fpmarkimport.Visible = false;
        btn_import.Visible = false;
        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;
        pHeaderSettings.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        //  Buttonexit.Visible = false;
        fpmarkexcel.Visible = false;
        Delete.Visible = false;
        Delete.Enabled = false;
        Save.Visible = false;
        Save.Enabled = false;
        Label2.Visible = false;
        lblnote2.Visible = false;
        lblNote3.Visible = false;
        btnok.Visible = false;
        //  Exit1.Visible = false;
        chkretest.Visible = false;
        lblselectstaff.Visible = false;
        ddlstaffname.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        lblPageSearch.Visible = false;
        lblErrorMsg.Visible = false;
        ddlBranch.Items.Clear();
        //string  a = 13;
        string course_id = Convert.ToString(ddlDegree.SelectedValue).Trim();
        //string sem = ddlSem.SelectedValue.ToString();
        string collegecode = Convert.ToString(Session["collegecode"]).Trim();
        string usercode = Convert.ToString(Session["UserCode"]).Trim();//Session["UserCode"].ToString();
        DataSet ds = Bind_Dept(course_id, collegecode, usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
            // ddlBranch.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        }
        //'----------- call the semester and section
        bindbranch();
        bindsem();
        BindSectionDetail();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblRetestMin.Visible = false;
        txt_RetestMin.Visible = false;
        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;
        pHeaderSettings.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        // Buttonexit.Visible = false;
        btn_import.Visible = false;
        Delete.Visible = false;
        Delete.Enabled = false;
        Save.Visible = false;
        Save.Enabled = false;
        Label2.Visible = false;
        lblnote2.Visible = false;
        lblNote3.Visible = false;
        btnok.Visible = false;
        // Exit1.Visible = false;
        chkretest.Visible = false;
        lblselectstaff.Visible = false;
        ddlstaffname.Visible = false;
        fpmarkexcel.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        lblPageSearch.Visible = false;
        lblErrorMsg.Visible = false;
        clear();
        // ddlSubject.Items.Clear();
        if (!Page.IsPostBack == false)
        {
            ddlSemYr.Items.Clear();
        }
        try
        {
            if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            {
                // Get_Semester();
                bindsem();
            }
            if (ddlBranch.SelectedIndex == 0)
            {
                bindsem();
            }
            bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlSubject.Items.Clear();
        lblRetestMin.Visible = false;
        txt_RetestMin.Visible = false;
        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;
        pHeaderSettings.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        //Buttonexit.Visible = false;
        btn_import.Visible = false;
        // Exit1.Visible = false;
        Delete.Visible = false;
        Delete.Enabled = false;
        Save.Visible = false;
        Save.Enabled = false;
        Label2.Visible = false;
        lblnote2.Visible = false;
        lblNote3.Visible = false;
        btnok.Visible = false;
        // Exit1.Visible = false;
        chkretest.Visible = false;
        lblselectstaff.Visible = false;
        ddlstaffname.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        lblPageSearch.Visible = false;
        lblErrorMsg.Visible = false;
        //if (!Page.IsPostBack == false)
        //{
        //    ddlSec.Items.Clear();
        //}
        BindSectionDetail();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblRetestMin.Visible = false;
        txt_RetestMin.Visible = false;
        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;
        pHeaderSettings.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        // Buttonexit.Visible = false;
        btn_import.Visible = false;
        Delete.Visible = false;
        Delete.Enabled = false;
        Save.Visible = false;
        Save.Enabled = false;
        Label2.Visible = false;
        lblnote2.Visible = false;
        lblNote3.Visible = false;
        lbltab.Visible = false;
        btnok.Visible = false;
        //  Exit1.Visible = false;
        chkretest.Visible = false;
        lblselectstaff.Visible = false;
        ddlstaffname.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        lblPageSearch.Visible = false;
        lblErrorMsg.Visible = false;
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        Save.Visible = true;
        Save.Enabled = true;
        Delete.Visible = true;
        Delete.Enabled = true;
        // Exit1.Visible = true;
        Label2.Visible = true;
        lblnote2.Visible = true;
        lblNote3.Visible = true;
        pHeaderEntry.Visible = true;
        pHeaderReport.Visible = true;
        pHeaderSettings.Visible = true;
        TextBoxother.Text = string.Empty;
        lblErrorMsg.Visible = false;
        lblPageSearch.Visible = false;
        if (DropDownListpage.Text == "Others")
        {
            //  TextBoxother.Visible = true;
            // TextBoxother.Focus();
        }
        else
        {
            TextBoxother.Visible = false;
            TextBoxother.Text = string.Empty;
            int pageSize = 0;
            int.TryParse(Convert.ToString(DropDownListpage.Text).Trim(), out pageSize);
            //FpSettings.Sheets[0].PageSize = pageSize;// Convert.ToInt16(DropDownListpage.Text.ToString());
            // CalculateTotalPages();
        }
    }

    //-----------------------------------------function for group value
    protected void ddlGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //fpmarkexcel.Visible = false;
            //myconn.Close();
            //myconn.Open();
            //FpReport.Sheets[0].RowCount = 0;
            //FpSettings.Sheets[0].ColumnCount = 5;
            //string sqlStr = string.Empty;
            //string semester = string.Empty;
            //string degreecode = string.Empty;
            //string batch = string.Empty;
            //string subno = string.Empty;
            //string sections = string.Empty;
            //string activerow = string.Empty;
            //string activecol = string.Empty;
            //string strsec = string.Empty;
            //int rerowcnt = 0;
            //activerow = FpEntry.ActiveSheetView.ActiveRow.ToString();
            //activecol = FpEntry.ActiveSheetView.ActiveColumn.ToString();
            //int ar;
            //int ac;
            //ar = Convert.ToInt32(activerow.ToString());
            //ac = Convert.ToInt32(activecol.ToString());
            //rerowcnt = ar;
            //if (ar != -1)
            //{
            //    //semester = FpEntry.Sheets[0].Cells[rerowcnt, 3].Text.Trim();
            //   // degreecode = FpEntry.Sheets[0].Cells[rerowcnt, 2].Tag.ToString();
            //   // batch = FpEntry.Sheets[0].Cells[rerowcnt, 1].Text.Trim();
            //   // subno = FpEntry.Sheets[0].Cells[rerowcnt, 5].Tag.ToString();
            //    //if (FpEntry.Sheets[0].Cells[rerowcnt, 4].Text != null && FpEntry.Sheets[0].Cells[rerowcnt, 4].Text.Trim() != "")
            //    {
            //        sections = FpEntry.Sheets[0].Cells[rerowcnt, 4].Text.Trim();
            //    }
            //    if (Convert.ToString(sections).Trim().ToLower() == "all" || Convert.ToString(sections).Trim() == "" || Convert.ToString(sections).Trim() == "-1")
            //    {
            //        strsec = string.Empty;
            //    }
            //    else
            //    {
            //        strsec = " and sections='" + Convert.ToString(sections).Trim() + "'";
            //    }
            //    //--------------------------------------------based on the grpcode to display the test name
            //    if (Session["Staff_Code"] != null && Convert.ToString(Session["Staff_Code"]).Trim() != "")
            //    {
            //        if (chkGrp.Checked = true && ddlGrp.SelectedValue != "")
            //        {
            //            sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + " " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " and staff_code= ' " + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "') and groupcode='" + ddlGrp.Text.ToString() + "'";
            //        }
            //        else
            //        {
            //            sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + "' " + strsec.ToString() + " and batch_year='" + batch.ToString() + "' and staff_code = (select top 1 staff_code  from staff_selector where subject_no ='" + subno.ToString() + "' and batch_year = '" + batch.ToString() + "' and staff_code= '" + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";
            //        }
            //    }
            //    else
            //    {
            //        if (chkGrp.Checked = true && ddlGrp.SelectedValue != "")
            //        {
            //            sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + " '" + strsec.ToString() + " and batch_year='" + batch.ToString() + "' and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and groupcode='" + ddlGrp.Text.ToString() + "'";
            //        }
            //        else
            //        {
            //            sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + "' " + strsec.ToString() + " and batch_year='" + batch.ToString() + "' and staff_code in (select top 1 staff_code  from staff_selector where subject_no ='" + subno.ToString() + "' and batch_year ='" + batch.ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";
            //        }
            //    }
            //    readcon.Close();
            //    readcon.Open();
            //    string criteria = string.Empty;
            //    SqlCommand chkgrpcmd = new SqlCommand(sqlStr, readcon);
            //    SqlDataReader chkgrpdrr;
            //    chkgrpdrr = chkgrpcmd.ExecuteReader();
            //    FpSettings.SaveChanges();
            //    while (chkgrpdrr.Read())
            //    {
            //        int rowcnt = 0;
            //        FpReport.Sheets[0].RowCount += 1;
            //        rowcnt = Convert.ToInt32(FpReport.Sheets[0].RowCount) - 1;
            //        string display = string.Empty;
            //        string criteria_no = string.Empty;
            //        string max_mark = string.Empty;
            //        string min_mark = string.Empty;
            //        string bind = string.Empty;
            //        bind = string.Empty;
            //        bind = subno + "-" + batch + "-" + sections + "-" + degreecode + "-" + semester;
            //        //criteria_no = FpReport.Sheets[0].Cells[rowcnt, 1].Tag.ToString();
            //        criteria_no = chkgrpdrr[1].ToString();
            //        criteria = chkgrpdrr[0].ToString();
            //        max_mark = chkgrpdrr[2].ToString();
            //        min_mark = chkgrpdrr[3].ToString();
            //        FpReport.Sheets[0].Cells[rowcnt, 1].Tag = criteria_no.ToString();
            //        FpReport.Sheets[0].Cells[rowcnt, 1].Note = bind.ToString();
            //        FpReport.Sheets[0].Cells[rowcnt, 1].Text = criteria.ToString();
            //        FpReport.Sheets[0].Cells[rowcnt, 5].Text = max_mark.ToString();
            //        FpReport.Sheets[0].Cells[rowcnt, 5].Note = max_mark.ToString();
            //        FpReport.Sheets[0].Cells[rowcnt, 6].Text = min_mark.ToString();
            //        FpReport.Sheets[0].Cells[rowcnt, 6].Note = min_mark.ToString();
            //        FpReport.Sheets[0].Cells[rowcnt, 0].Value = 0;
            //        FpSettings.SaveChanges();
            //        string[] splitvals = bind.Split(new char[] { '-' });
            //        if ((splitvals[2].ToString() != " ") && (splitvals[2].ToString() != ""))
            //        {
            //            display = "select * from exam_type where  criteria_no='" + criteria_no + "' and subject_no = '" + splitvals[0] + "' and sections='" + splitvals[2] + "' and batch_year='" + splitvals[1] + "'";
            //        }
            //        else
            //        {
            //            display = "select * from exam_type where  criteria_no='" + criteria_no + "' and subject_no = '" + splitvals[0] + "' and batch_year='" + splitvals[1] + "";
            //        }
            //        myconn.Close();
            //        myconn.Open();
            //        SqlCommand cmd1 = new SqlCommand(display, myconn);
            //        SqlDataReader drr;
            //        drr = cmd1.ExecuteReader();
            //        FpSettings.SaveChanges();
            //        while (drr.Read())
            //        {
            //            if (drr.HasRows == true)
            //            {
            //                string resexamdate = string.Empty;
            //                string resentrydate = string.Empty;
            //                string resmaxmrk = string.Empty;
            //                string resminmrk = string.Empty;
            //                string resduration = string.Empty;
            //                string resnewmaxmrk = string.Empty;
            //                string resnewminmrk = string.Empty;
            //                string formatexam = string.Empty;
            //                string bindnote = string.Empty;
            //                string rollno = string.Empty;
            //                //formatexam = drr["exam_date"].ToString();
            //                //string[] formatexamsplit = formatexam.Split(new char[] { ' ' });
            //                //string[] formatetime = formatexamsplit[0].Split(new char[] { '/' });
            //                //string examconcat = formatetime[1] + "/" + formatetime[0] + "/" + formatetime[2];
            //                //string formatentry =string.Empty;
            //                //formatentry = drr["entry_date"].ToString();
            //                //string[] formatentrysplit = formatentry.Split(new char[] { ' ' });
            //                //string[] formatentrytime = formatentrysplit[0].Split(new char[] { '/' });
            //                //string entryconcat = formatentrytime[1] + "/" + formatentrytime[0] + "/" + formatentrytime[2];
            //                formatexam = drr["exam_date"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 2].Note = formatexam.ToString();
            //                if (formatexam != "")
            //                {
            //                    string[] formatexamsplit = formatexam.Split(new char[] { ' ' });
            //                    string[] formatetime = formatexamsplit[0].Split(new char[] { '/' });
            //                    string examconcat = formatetime[1] + "/" + formatetime[0] + "/" + formatetime[2];
            //                    FpReport.Sheets[0].Cells[rowcnt, 2].Text = Convert.ToInt32(formatetime[1]).ToString();
            //                    FpReport.Sheets[0].Cells[rowcnt, 3].Text = Convert.ToInt32(formatetime[0]).ToString();
            //                    FpReport.Sheets[0].Cells[rowcnt, 4].Text = formatetime[2].ToString();
            //                }
            //                else
            //                {
            //                    string examconcat = string.Empty;
            //                    FpReport.Sheets[0].Cells[rowcnt, 2].Text = string.Empty;
            //                    FpReport.Sheets[0].Cells[rowcnt, 3].Text = string.Empty;
            //                    FpReport.Sheets[0].Cells[rowcnt, 4].Text = string.Empty;
            //                }
            //                string formatentry = string.Empty;
            //                formatentry = drr["entry_date"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 5].Note = formatentry.ToString();
            //                if (formatentry != "")
            //                {
            //                    string[] formatentrysplit = formatentry.Split(new char[] { ' ' });
            //                    string[] formatentrytime = formatentrysplit[0].Split(new char[] { '/' });
            //                    string entryconcat = formatentrytime[1] + "/" + formatentrytime[0] + "/" + formatentrytime[2];
            //                    FpReport.Sheets[0].Cells[rowcnt, 5].Text = Convert.ToInt32(formatentrytime[1]).ToString();
            //                    FpReport.Sheets[0].Cells[rowcnt, 6].Text = Convert.ToInt32(formatentrytime[0]).ToString();
            //                    FpReport.Sheets[0].Cells[rowcnt, 7].Text = formatentrytime[2].ToString();
            //                }
            //                else
            //                {
            //                    string entryconcat = string.Empty;
            //                    FpReport.Sheets[0].Cells[rowcnt, 5].Text = string.Empty;
            //                    FpReport.Sheets[0].Cells[rowcnt, 6].Text = string.Empty;
            //                    FpReport.Sheets[0].Cells[rowcnt, 7].Text = string.Empty;
            //                }
            //                //FpReport.Sheets[0].Cells[rowcnt, 5].Note = drr["max_mark"].ToString();
            //                //FpReport.Sheets[0].Cells[rowcnt, 5].Text = drr["max_mark"].ToString();
            //                //FpReport.Sheets[0].Cells[rowcnt, 6].Note = drr["min_mark"].ToString();
            //                //FpReport.Sheets[0].Cells[rowcnt, 6].Text = drr["min_mark"].ToString();
            //                //subno = FpEntry.Sheets[0].Cells[rowcnt, 5].Tag.ToString();
            //                //FpReport.Sheets[0].Cells[rowcnt, 2].Text = formatexamsplit[0].ToString();
            //                //FpReport.Sheets[0].Cells[rowcnt, 3].Text = formatentrysplit[0].ToString();
            //                //FpReport.Sheets[0].Cells[rowcnt, 4].Text = drr["duration"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 10].Note = drr["max_mark"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 10].Text = drr["max_mark"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 11].Note = drr["min_mark"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 11].Text = drr["min_mark"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 12].Text = drr["start_period"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 12].Note = drr["start_period"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 13].Text = drr["end_period"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 13].Note = drr["end_period"].ToString();
            //                subno = FpEntry.Sheets[0].Cells[rerowcnt, 5].Tag.ToString();
            //                string duration = string.Empty;
            //                duration = drr["duration"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 8].Note = duration.ToString();
            //                if (duration.ToString().Trim() != "")
            //                {
            //                    string[] splitdur = duration.Split(new char[] { ':' });
            //                    // FpReport.Sheets[0].Cells[rowcnt, 8].Value = splitdur[0].ToString();
            //                    FpReport.Sheets[0].SetText(rowcnt, 8, splitdur[0].Trim().ToString());
            //                    if (splitdur.GetUpperBound(0) == 1)
            //                    {
            //                        if (splitdur[1].ToString() != "")
            //                        {
            //                            // FpReport.Sheets[0].Cells[rowcnt, 9].Value = splitdur[1].ToString();
            //                            FpReport.Sheets[0].SetText(rowcnt, 9, splitdur[1].Trim().ToString());
            //                        }
            //                    }
            //                }
            //                FpReport.Sheets[0].Cells[rowcnt, 0].Value = 1;
            //                resexamdate = drr["exam_date"].ToString();
            //                resentrydate = drr["entry_date"].ToString();
            //                resmaxmrk = drr["max_mark"].ToString();
            //                resminmrk = drr["min_mark"].ToString();
            //                resduration = drr["duration"].ToString();
            //                resnewmaxmrk = drr["new_maxmark"].ToString();
            //                resnewminmrk = drr["new_minmark"].ToString();
            //                string exam_code = string.Empty;
            //                exam_code = drr["exam_code"].ToString();
            //                FpReport.Sheets[0].Cells[rowcnt, 0].Tag = exam_code.ToString();
            //                bindnote = bind + ";" + resexamdate + "-" + resentrydate + "-" + resduration + "-" + resnewmaxmrk + "-" + resmaxmrk + "-" + resnewminmrk + "-" + resminmrk;
            //                FpSettings.Sheets[0].ColumnCount += 1;
            //                FpSettings.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1].Tag = criteria_no.ToString();
            //                FpSettings.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1].Note = bindnote.ToString();
            //                FpSettings.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1].Text = criteria.ToString();
            //                for (int res = 0; res <= Convert.ToInt16(FpSettings.Sheets[0].RowCount) - 1; res++)
            //                {
            //                    int colco = 0;
            //                    colco = Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1;
            //                    for (int col = 5; col <= colco; col++)
            //                    {
            //                        rollno = FpSettings.Sheets[0].Cells[res, 0].Text;
            //                        string resultmark = string.Empty;
            //                        resultmark = "select * from Result where roll_no='" + rollno + "'and exam_code = '" + exam_code + "'";
            //                        markcon.Close();
            //                        markcon.Open();
            //                        SqlCommand command1 = new SqlCommand(resultmark, markcon);
            //                        SqlDataReader resreader;
            //                        resreader = command1.ExecuteReader();
            //                        while (resreader.Read())
            //                        {
            //                            if (resreader.HasRows == true)
            //                            {
            //                                FpSettings.Sheets[0].Cells[res, colco].Text = resreader["marks_obtained"].ToString();
            //                            }
            //                        }
            //                        string chkmark = string.Empty;
            //                        chkmark = FpSettings.Sheets[0].Cells[res, colco].Text;
            //                        if (Convert.ToString(chkmark) == "-1")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "AAA";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-2")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "EL";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-3")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "EOD";
            //                        }
            //                        if (Convert.ToString(chkmark) == "")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "0";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-4")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "ML";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-5")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "SOD";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-6")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "NSS";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-16")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "OD";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-7")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "NJ";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-8")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "S";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-9")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "L";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-10")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "NCC";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-11")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "HS";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-12")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "PP";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-13")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "SYOD";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-14")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "COD";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-15")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "OOD";
            //                        }
            //                        if (Convert.ToString(chkmark) == "-17")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "LA";
            //                        }
            //                        //****Modified By Subburaj 20.08.2014******//
            //                        if (Convert.ToString(chkmark) == "-18")
            //                        {
            //                            FpSettings.Sheets[0].Cells[res, colco].Text = "RAA";
            //                        }
            //                        //*******************End*****************//
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //pHeaderEntry.Visible = true;
            //pHeaderReport.Visible = true;
            //pHeaderSettings.Visible = true;
            //Label2.Visible = true;
            //lblnote2.Visible = true;
            //lblNote3.Visible = true;
        }
        catch
        {
        }
    }

    protected void chkmarkattendance_CheckedChanged(object sender, EventArgs e)
    {
        //FpReport.Visible = false;
        //FpSettings.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        // Buttonexit.Visible = false;
        btnok.Visible = false;
        // Exit1.Visible = false;
        chkretest.Visible = false;
        lblselectstaff.Visible = false;
        ddlstaffname.Visible = false;
        pHeaderReport.Visible = false;
        pHeaderSettings.Visible = false;
        //if (FpEntry.Visible == true)
        //{
        pHeaderEntry.Visible = true;
        //}
    }

    protected void chkretest_CheckedChanged(object sender, EventArgs e)
    {
        pHeaderEntry.Visible = true;
        pHeaderReport.Visible = true;
        pHeaderSettings.Visible = true;
        lblRetestMin.Visible = false;
        txt_RetestMin.Visible = false;
        //pHeaderSettings.Attributes.Add("Style", "margin-top:10px;");
        pHeaderSettings.Visible = true;
        pHeaderReport.Visible = true;
        bool cbDaywisePeriodAttSchedule = false;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " and group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0].Trim() + "'";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
        }
        DataSet dsSettings = new DataSet();
        dsSettings = da.select_method_wo_parameter("select * from Master_Settings where settings='RetestMarkEntryBasedOnOptionalMinMarks' " + grouporusercode + "", "Text");
        if (dsSettings.Tables[0].Rows.Count > 0 && chkretest.Checked == true)
        {
            if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]).Trim() == "0")
            {
                cbDaywisePeriodAttSchedule = false;
            }
            else if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]).Trim() == "1")
            {
                cbDaywisePeriodAttSchedule = true;
            }
        }
        lblRetestMin.Visible = cbDaywisePeriodAttSchedule;
        txt_RetestMin.Visible = cbDaywisePeriodAttSchedule;
        txt_RetestMin.Text = string.Empty;
        if (!cbDaywisePeriodAttSchedule)
        {
            lblRetestMin.Attributes.Add("styles", "display:none");
            txt_RetestMin.Attributes.Add("styles", "display:none");
        }
        //btnok_Click(sender, e);
    }

    protected void chkGrp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkGrp.Checked == true)
        {
            ddlGrp.Enabled = true;
            ddlGrp.Visible = true;
        }
        else
        {
            ddlGrp.Enabled = false;
        }
        pHeaderEntry.Visible = true;
        pHeaderReport.Visible = true;
        pHeaderSettings.Visible = true;
        Label2.Visible = true;
        lblnote2.Visible = true;
        lblNote3.Visible = true;
    }

    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Save.Visible = true;
            Save.Enabled = true;
            Delete.Visible = true;
            // Exit1.Visible = true;
            Delete.Enabled = true;
            Label2.Visible = true;
            lblnote2.Visible = true;
            lblNote3.Visible = true;
            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
            if (TextBoxpage.Text.Trim() != "")
            {
                if (Session["totalPages"] != null && Convert.ToInt16(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                {
                    lblPageSearch.Visible = true;
                    lblPageSearch.Text = "Exceed The Page Limit";
                    //FpSettings.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    // Buttonexit.Visible = true;
                    TextBoxpage.Text = string.Empty;
                }
                else if (Convert.ToInt32(TextBoxpage.Text) == 0)
                {
                    lblPageSearch.Visible = true;
                    lblPageSearch.Text = "Search should be greater than zero";
                    TextBoxpage.Text = string.Empty;
                }
                else
                {
                    lblPageSearch.Visible = false;
                    //FpSettings.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    // FpSettings.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    // Buttonexit.Visible = true;
                    TextBoxpage.Text = string.Empty;
                }
            }
        }
        catch
        {
            Save.Visible = true;
            Save.Enabled = true;
            Delete.Visible = true;
            Delete.Enabled = true;
            //  Exit1.Visible = true;
            Label2.Visible = true;
            lblnote2.Visible = true;
            lblNote3.Visible = true;
            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
            lblPageSearch.Visible = true;
            TextBoxpage.Text = string.Empty;
            lblPageSearch.Text = "Please Give the Valid PageSearch";
        }
    }

    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxother.Text.Trim() != "")
            {
                //long pagesize=0;
                //pagesize=Convert.ToInt64(FpSettings.Sheets[0].PageSize);
                //pagesize = Convert.ToInt64(TextBoxother.Text.ToString());
                //FpSettings.Sheets[0].PageSize = Convert.ToInt32(TextBoxother.Text.ToString());
                pHeaderEntry.Visible = true;
                pHeaderReport.Visible = true;
                pHeaderSettings.Visible = true;
                lblPageSearch.Visible = false;
                Label2.Visible = true;
                lblnote2.Visible = true;
                lblNote3.Visible = true;
                Save.Visible = true;
                Delete.Visible = true;
                Save.Enabled = true;
                Delete.Enabled = true;
                // Exit1.Visible = true;
                //  CalculateTotalPages();
            }
        }
        catch
        {
            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
            TextBoxother.Text = string.Empty;
            lblPageSearch.Visible = true;
            Label2.Visible = true;
            lblnote2.Visible = true;
            lblNote3.Visible = true;
            Save.Visible = true;
            Delete.Visible = true;
            Save.Enabled = true;
            Delete.Enabled = true;
            // Exit1.Visible = true;
            TextBoxother.Text = string.Empty;
            lblPageSearch.Text = "Please Give the Valid RecordCount";
        }
        //   FpSettings.CurrentPage = 0;
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        // Button1.Visible = true;
    }


    //protected void FpSettings_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    if (Cellclick == true)
    //    {

    //        Visible = true;
    //        pHeaderReport.Visible = true;
    //        pHeaderSettings.Visible = true;
    //        Exit1.Visible = false;
    //        Save.Visible = true;
    //        Save.Enabled = true;
    //        Delete.Visible = true;
    //        Delete.Enabled = true;
    //        Exit1.Visible = true;
    //        Cellclick = false;
    //    }
    //}


    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        funconn.Close();
        funconn.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, funconn);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = funconn;
        drnew = funcmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }

    private string getMarkText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "-1":
                    mark = "AAA";
                    break;
                case "-2":
                    mark = "EL";
                    break;
                case "-3":
                    mark = "EOD";
                    break;
                case "-4":
                    mark = "ML";
                    break;
                case "-5":
                    mark = "SOD";
                    break;
                case "-6":
                    mark = "NSS";
                    break;
                case "-7":
                    mark = "NJ";
                    break;
                case "-8":
                    mark = "S";
                    break;
                case "-9":
                    mark = "L";
                    break;
                case "-10":
                    mark = "NCC";
                    break;
                case "-11":
                    mark = "HS";
                    break;
                case "-12":
                    mark = "PP";
                    break;
                case "-13":
                    mark = "SYOD";
                    break;
                case "-14":
                    mark = "COD";
                    break;
                case "-15":
                    mark = "OOD";
                    break;
                case "-16":
                    mark = "OD";
                    break;
                case "-17":
                    mark = "LA";
                    break;
                case "-18":
                    mark = "RAA";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }

    public void newsave()//Raj modeified
    {
        int savestdct = 0;
        bool emptymrkflag = false;
        string getmark = string.Empty;
        string exam_code = string.Empty;
        bool checkstaff = false; // added by jairam 2015-07-10
        string staff_code = string.Empty;
        staff_code = Convert.ToString(Session["staff_code"]).Trim();
        bool isSaveSuccess = true;
        string batchYear1 = string.Empty;
        string degreeCode1 = string.Empty;
        string sem = string.Empty;
        string sec = string.Empty;
        if (staff_code == null || staff_code.Trim() == "")
        {
            if (ddlstaffname.Items.Count > 0)
            {
                if (Convert.ToString(ddlstaffname.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlstaffname.SelectedItem.Text).Trim().ToLower() != "select")
                {
                    checkstaff = true;
                }
                else
                {
                    checkstaff = false;
                }
            }
            else
            {
                checkstaff = false;
            }

        }
        else
        {
            checkstaff = true;
        }
        if (checkstaff == true)
        {
            bool istrue = false;
            int colcount = 0;
            colcount = dtmrkcoutcol;
            FarPoint.Web.Spread.DoubleCellType intcell = new FarPoint.Web.Spread.DoubleCellType();
            foreach (GridViewRow i in GridView3.Rows)
            {
                emptymrkflag = true;
                rollno = string.Empty;
                Label rolno = (Label)i.FindControl("lblrollno");
                rollno = rolno.Text;
                Label apno = (Label)i.FindControl("lblappno");
                string appNo = apno.Text;
                double totalMark = 0;
                DataTable deginfo = dir.selectDataTable("select Batch_Year,degree_code,Current_Semester,Sections from Registration where App_No='" + appNo + "'");
                if (deginfo.Rows.Count > 0 && istrue == false)
                {
                    foreach (DataRow dtdeg in deginfo.Rows)
                    {
                        istrue = true;
                        batchYear1 = Convert.ToString(dtdeg["Batch_Year"]);
                        degreeCode1 = Convert.ToString(dtdeg["degree_code"]);
                        sem = Convert.ToString(dtdeg["Current_Semester"]);
                        sec = Convert.ToString(dtdeg["Sections"]);
                    }
                }
                Dictionary<string, double> dicExamWiseTotalMark = new Dictionary<string, double>();
                string txttest = "txttest";
                int tstct = 0;
                string ct3 = lblsubcout.Text;
                int tblct = Convert.ToInt32(ct3);
                int sct = 0;
                tblct = tblct + 4;
                for (int j1 = 5; j1 <= tblct; j1++)
                {
                    foreach (GridViewRow gr in GridView2.Rows)
                    {
                        System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr.FindControl("cbcell_1");
                        if (chk.Checked == true)
                        {
                            Label exmcod = (Label)gr.FindControl("lblexamcode");
                            exam_code = exmcod.Text;
                        }
                    }

                    getmark = string.Empty;
                    string reTestMark = string.Empty;

                    TextBox mrk = (TextBox)i.FindControl(txttest);
                    getmark = mrk.Text;
                    string[] gt1 = getmark.Split('.');
                    if (gt1.Length > 1)
                    {
                        if (!Convert.ToString(gt1[0]).All(char.IsLetterOrDigit) || Convert.ToString(gt1[1]).All(char.IsWhiteSpace))
                        {
                            divPopAlert.Visible = true;
                            divPopAlertContent.Visible = true;
                            lblAlertMsg.Text = "Invalid Characters!";
                            // Buttonexit.Visible = true;
                            Delete.Visible = true;
                            Save.Visible = true;
                            return;
                        }
                    }
                    TextBox retst = (TextBox)i.FindControl("txtretest");
                    reTestMark = retst.Text;
                    string[] gt2 = getmark.Split('.');
                    if (gt2.Length > 1)
                    {
                        if (!Convert.ToString(gt2[0]).All(char.IsLetterOrDigit) || Convert.ToString(gt2[1]).All(char.IsWhiteSpace))
                        {
                            divPopAlert.Visible = true;
                            divPopAlertContent.Visible = true;
                            lblAlertMsg.Text = "Invalid Characters!";
                            // Buttonexit.Visible = true;
                            Delete.Visible = true;
                            Save.Visible = true;
                            return;
                        }
                    }
                    //Label sbid = (Label)i.FindControl("lblsubid");
                    Label sbid = GridView3.Rows[0].FindControl("lblsubid") as Label;
                    string subid = sbid.Text;
                    if (subid.Length > 0)
                    {
                        string[] sid = subid.Split('-');
                        subid = sid[sct].ToString();
                    }
                    getmark = getMarkValue(getmark);
                    double testMark = 0;
                    double.TryParse(getmark, out testMark);
                    if (!string.IsNullOrEmpty(getmark))
                    {
                        emptymrkflag = false;
                    }
                    if (testMark >= 0 && !string.IsNullOrEmpty(getmark))
                    {
                        if (!dicExamWiseTotalMark.ContainsKey(exam_code.Trim()))
                            dicExamWiseTotalMark.Add(exam_code.Trim(), testMark);
                        else if (dicExamWiseTotalMark[exam_code.Trim()] >= 0 && testMark >= 0)
                            dicExamWiseTotalMark[exam_code.Trim()] += testMark;
                        else
                        {
                            if (dicExamWiseTotalMark[exam_code.Trim()] <= testMark)
                                dicExamWiseTotalMark[exam_code.Trim()] = testMark;
                        }
                    }
                    else
                    {
                        if (!dicExamWiseTotalMark.ContainsKey(exam_code.Trim()))
                            dicExamWiseTotalMark.Add(exam_code.Trim(), testMark);
                        else if (dicExamWiseTotalMark[exam_code.Trim()] >= 0 && testMark >= 0)
                            dicExamWiseTotalMark[exam_code.Trim()] += testMark;
                        else
                        {
                            if (dicExamWiseTotalMark[exam_code.Trim()] <= testMark)
                                dicExamWiseTotalMark[exam_code.Trim()] = testMark;
                        }
                    }
                    if (subid != "")
                    {
                        if (!string.IsNullOrEmpty(getmark.Trim()))
                        {
                            string qry = "if exists (select subjectId from subSubjectWiseMarkEntry where subjectId='" + subid + "' and appNo ='" + appNo + "') update subSubjectWiseMarkEntry set testMark='" + getmark + "',ReTestMark='" + reTestMark + "' where subjectId='" + subid + "' and appNo ='" + appNo + "' else insert into subSubjectWiseMarkEntry (appNo,subjectId,testMark,ReTestMark) values('" + appNo + "','" + subid + "','" + getmark + "','" + reTestMark + "')";
                            int resultNew = da.update_method_wo_parameter(qry, "text");
                            if (resultNew > 0)
                            {
                                // savestdct++;
                            }
                            else
                                isSaveSuccess = false;
                        }
                    }
                    sct++;
                    tstct++;
                    txttest = "txttest" + tstct + "";
                }
                if (dicExamWiseTotalMark.Count > 0)
                {
                    foreach (KeyValuePair<string, double> dicItem in dicExamWiseTotalMark)
                    {
                        string examCode = dicItem.Key;
                        double totalMarks = dicItem.Value;
                        if (examCode != "0")
                        {
                            if (!string.IsNullOrEmpty(examCode) && !string.IsNullOrEmpty(totalMarks.ToString()) && emptymrkflag == false)
                            {
                                string qry = "if exists (select marks_obtained from Result where exam_code='" + examCode + "' and roll_no ='" + rollno + "') update Result set marks_obtained='" + totalMarks + "' where exam_code='" + examCode + "' and roll_no ='" + rollno + "' else insert into Result (marks_obtained,exam_code,roll_no) values('" + totalMarks + "','" + examCode + "','" + rollno + "')";
                                int resultNew = da.update_method_wo_parameter(qry, "text");
                                if (resultNew > 0)
                                    savestdct++;
                                else
                                    isSaveSuccess = false;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            isSaveSuccess = false;
            lblexcelerror.Text = "Please Select Staff Name";
            lblexcelerror.Visible = true;
        }
        string saveOrUpdate = (Save.Text.Trim().ToLower() == "save") ? "Saved" : "Updated";
        string entrycode = Session["Entry_Code"].ToString();
        string PageName = "Student Mark Entry";
        string section = ddlSec.SelectedValue.ToString();
        string batchYear = ddlBatch.SelectedValue.ToString();
        string Semester = ddlSemYr.SelectedValue.ToString();
        string degreeCode = ddlBranch.SelectedValue.ToString();

        string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
        string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");

        if (!string.IsNullOrEmpty(Convert.ToString(Session["Btach_Year"])))
            Btach_Year_Val = Convert.ToString(Session["Btach_Year"]);
        else if (!string.IsNullOrEmpty(batchYear1))
            Btach_Year_Val = batchYear1;
        if (!string.IsNullOrEmpty(Convert.ToString(Session["Semester"])))
            Semester_Val = Convert.ToString(Session["Semester"]);
        else if (!string.IsNullOrEmpty(sem))
            Semester_Val = sem;
        if (!string.IsNullOrEmpty(Convert.ToString(Session["Degree_Code"])))
            Degree_Code_Val = Convert.ToString(Session["Degree_Code"]);
        else if (!string.IsNullOrEmpty(degreeCode1))
            Degree_Code_Val = degreeCode1;

        pHeaderEntry.Visible = true;
        pHeaderReport.Visible = true;
        pHeaderSettings.Visible = true;
        Delete.Visible = true;
        Delete.Enabled = true;
        //  Exit1.Visible = true;
        Label2.Visible = true;
        lblnote2.Visible = true;
        lblNote3.Visible = true;
        lbltab.Visible = true;

        if (isSaveSuccess)
        {
            string ctsname = "Save the Student Mark Information";
            if (string.IsNullOrEmpty(Btach_Year_Val) || string.IsNullOrEmpty(Semester_Val) || string.IsNullOrEmpty(Degree_Code_Val))
                da.insertUserActionLog(entrycode, batchYear, degreeCode, Semester, section, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 1);
            else
                da.insertUserActionLog(entrycode, Btach_Year_Val, Degree_Code_Val, Semester_Val, section, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 1);
            divPopAlert.Visible = true;
            divPopAlertContent.Visible = true;
            lblAlertMsg.Text = "" + saveOrUpdate + " Successfully For " + savestdct + " Students ";
            Delete.Visible = true;
            Save.Visible = true;
        }
        else
        {
            string ctsname = "Update the Student Mark Information";
            if (string.IsNullOrEmpty(Btach_Year_Val) || string.IsNullOrEmpty(Semester_Val) || string.IsNullOrEmpty(Degree_Code_Val))
                da.insertUserActionLog(entrycode, batchYear, degreeCode, Semester, section, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 2);
            else
                da.insertUserActionLog(entrycode, Btach_Year_Val, Degree_Code_Val, Semester_Val, section, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 2);
            divPopAlert.Visible = true;
            divPopAlertContent.Visible = true;
            lblAlertMsg.Text = "Not" + saveOrUpdate;
            Delete.Visible = true;
            Save.Visible = true;
        }
    }

    //---------------------------------------------------------------------coding for save btn
    protected void Save_Click(object sender, EventArgs e)
    {
        try
        {
            int savedstudct = 0;
            int notsavedstudct = 0;
            float[] testmax = new float[1];
            int isval = 0;
            int seltest = 0;
            bool isNew = false;
            bool isflag = false;

            string criteria_no = string.Empty;
            string criteria = string.Empty;
            string rollno = string.Empty;
            string appNo = string.Empty;
            int colcount = 0;
            double mark = 0;
            string Staff_Code = string.Empty;
            string examdate = string.Empty;
            string entrydate = string.Empty;
            string duration = string.Empty;
            float new_maxmark = 0;
            float new_minmark = 0;
            float maxmark = 0;
            float minmark = 0;
            string updateconcat = string.Empty;
            string criteria_noup = string.Empty;
            string exam_code = string.Empty;
            string date = string.Empty;
            string month = string.Empty;
            string year = string.Empty;
            string date1 = string.Empty;
            string month1 = string.Empty;
            string year1 = string.Empty;
            string hours = string.Empty;
            string minutes = string.Empty;
            string durval = string.Empty;
            string stprd = string.Empty;
            string endprd = string.Empty;


            foreach (GridViewRow flagrow in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox isvald = (System.Web.UI.WebControls.CheckBox)flagrow.FindControl("cbcell_1");
                if (isvald.Checked == true)
                {
                    seltest++;
                    Array.Resize(ref testmax, seltest);
                    Label critno = flagrow.FindControl("lblcriteriano") as Label;
                    criteria_noup = critno.Text;
                    criteria_no = criteria_noup;
                    DropDownList lblexdate = (flagrow.FindControl("ddlexamdate") as DropDownList);
                    date = lblexdate.Text;
                    DropDownList lblexmmonth = (flagrow.FindControl("ddlexammonth") as DropDownList);
                    month = lblexmmonth.Text;
                    DropDownList lblexmyear = (flagrow.FindControl("ddlexamyear") as DropDownList);
                    year = lblexmyear.Text;
                    examdate = date + "/" + month + "/" + year;
                    DropDownList date12 = flagrow.FindControl("ddlentrydate") as DropDownList;
                    date1 = date12.Text;
                    DropDownList month12 = flagrow.FindControl("ddlentrymonth") as DropDownList;
                    month1 = month12.Text;
                    DropDownList year11 = flagrow.FindControl("ddlentryyear") as DropDownList;
                    year1 = year11.Text;
                    entrydate = month1 + "/" + date1 + "/" + year1;
                    DropDownList hours11 = flagrow.FindControl("ddlhrs") as DropDownList;
                    hours = hours11.Text.Trim().PadLeft(2, '0');
                    DropDownList minutes11 = flagrow.FindControl("ddlmins") as DropDownList;
                    minutes = minutes11.Text.Trim().PadLeft(2, '0');
                    durval = hours.Trim().PadLeft(2, '0') + ":" + minutes.Trim().PadLeft(2, '0') + ":00";
                    Label new_maxmark11 = flagrow.FindControl("lblmaxmarks") as Label;
                    string mxmrk = new_maxmark11.Text;
                    new_maxmark = Convert.ToInt32(mxmrk);
                    maxmark = Convert.ToInt32(mxmrk);
                    Label new_minmark11 = flagrow.FindControl("lblminmarks") as Label;
                    string mimrk = new_minmark11.Text;
                    new_minmark = Convert.ToInt32(mimrk);
                    minmark = Convert.ToInt32(mimrk);
                    Label tst = flagrow.FindControl("lbltest") as Label;
                    string tst1 = tst.Text;

                    if (GridView2.Columns[14].Visible == true)
                    {
                        Label stprd11 = flagrow.FindControl("lblstartperiod") as Label;
                        stprd = stprd11.Text;
                        Label endprd11 = flagrow.FindControl("lblendperiod") as Label;
                        endprd = endprd11.Text;

                    }
                    else
                    {
                        stprd = null;
                        endprd = null;
                    }

                    Label exm_code = GridView3.Rows[0].FindControl("lblexmcd") as Label;
                    exam_code = exm_code.Text;

                    if (!string.IsNullOrEmpty(exam_code) && exam_code != "0" && exam_code.Trim().ToUpper() != "NE")
                    {
                        DataSet dsNew = da.select_method_wo_parameter("select s.subjectId,s.subSubjectName from subsubjectTestDetails s where s.examCode='" + exam_code + "'", "text");// and s.subjectNo='" + lblSubNo.Text + "'
                        if (dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                        {
                            isNew = true;
                        }
                    }
                    Label max_mark = flagrow.FindControl("lblmaxmarks") as Label;
                    string testmaxmrk = max_mark.Text;
                    float.TryParse(testmaxmrk.Trim(), out testmax[seltest - 1]);
                    isflag = true;
                    goto lbl1;
                }
            }
        lbl1:
            if (isNew)
            {
                newsave();
                Cellclick = true;
            }
            else
            {
                Boolean showsave = true;
                saveflag = true;
                if (saveflag == true)
                {
                    bool checkstaff = false; // added by jairam 2015-07-10
                    string staff_code = string.Empty;
                    staff_code = Convert.ToString(Session["staff_code"]).Trim();
                    if (staff_code == null || staff_code.Trim() == "")
                    {
                        if (ddlstaffname.Items.Count > 0)
                        {
                            if (Convert.ToString(ddlstaffname.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlstaffname.SelectedItem.Text).Trim().ToLower() != "select")
                            {
                                checkstaff = true;
                            }
                            else
                            {
                                checkstaff = false;
                            }
                        }
                        else
                        {
                            checkstaff = false;
                        }
                    }
                    else
                    {
                        checkstaff = true;
                    }
                    if (checkstaff == true)
                    {
                        updateconcat = examdate + "-" + entrydate + "-" + durval + "-" + new_maxmark + "-" + maxmark + "-" + new_minmark + "-" + minmark + "-" + stprd + "-" + endprd;

                        string celnot = string.Empty;
                        if (diccellnote.Count > 0)
                        {
                            foreach (KeyValuePair<int, string> drval in diccellnote)
                            {
                                celnot = drval.Value;
                                string[] splittag = celnot.Split(';');
                                celnot = Convert.ToString(splittag[0]);
                            }
                        }
                        GetCellNote = celnot + ";" + updateconcat;
                        int colcoupdate = 0;
                        colcount = Convert.ToInt32(dtmrk.Columns.Count);
                        //  for (int j1 = 5; j1 <= colcount; j1 = j1 + 2)
                        //foreach (GridViewRow j1 in GridView3.Rows)
                        //{
                        if (isflag == true)
                        {
                            string[] splitexam = GetCellNote.Split(new char[] { ';' });
                            string[] splitexamtype = splitexam[0].Split(new char[] { ',' });
                            string[] splitmarks = splitexam[1].Split(new char[] { '-' });
                            string[] spl_dur = splitmarks[2].Split(new char[] { ':' });
                            string concatdur = spl_dur[0].Trim().PadLeft(2, '0') + ":" + spl_dur[1].Trim().PadLeft(2, '0') + ":00";
                            string[] splitexamdate = splitmarks[0].Split(new char[] { '/' });
                            string[] splitentrydate = splitmarks[1].Split(new char[] { '/' });
                            string examconcat = splitexamdate[1] + "/" + splitexamdate[0] + "/" + splitexamdate[2];
                            string entryconcat = splitentrydate[1] + "/" + splitentrydate[0] + "/" + splitentrydate[2];
                            string[] formatetime = examconcat.Split(new char[] { ' ' });
                            string[] splitfinalexam = formatetime[0].Split(new char[] { '/' });
                            string examdatesplit = splitfinalexam[0] + "/" + splitfinalexam[1] + "/" + splitfinalexam[2];
                            string[] formatentrytime = entryconcat.Split(new char[] { ' ' });
                            string[] splitfinalentry = formatentrytime[0].Split(new char[] { '/' });
                            string entrydatesplit = splitfinalentry[1] + "/" + splitfinalentry[0] + "/" + splitfinalentry[2];
                            Label sec = GridView3.Rows[0].FindControl("lblsections1") as Label;
                            string sections = sec.Text;
                            Label bat = GridView3.Rows[0].FindControl("lblbatch2") as Label;
                            string batchyr2 = bat.Text;
                            Label sems = GridView3.Rows[0].FindControl("lblsems2") as Label;
                            string semester2 = sems.Text;
                            Label subsnos = GridView3.Rows[0].FindControl("lblsubno2") as Label;
                            string subjectnum = subsnos.Text;
                            string strsec = string.Empty;
                            string secval = string.Empty;
                            if (string.IsNullOrEmpty(sections) || sections.Trim().ToLower() == "all" || sections == " " || sections == "-1" || sections == null || sections == "")
                            {
                                strsec = string.Empty;
                            }
                            else
                            {
                                strsec = " and sections='" + sections.Trim() + "'";
                                secval = sections;
                            }
                            if (staff_code != null && staff_code.Trim() != "") // added by jairam 2015-07-10
                            {
                                Staff_Code = Convert.ToString(staff_code);
                            }
                            else
                            {
                                Staff_Code = ddlstaffname.SelectedItem.Value;
                            }

                            hat.Clear();
                            hat.Add("criteria_no", criteria_no);
                            hat.Add("staff_code", Staff_Code);
                            hat.Add("subject_no", subjectnum);
                            hat.Add("duration", concatdur);
                            hat.Add("entry_date", entrydatesplit);
                            hat.Add("exam_date", examdatesplit);
                            hat.Add("batch_year", batchyr2);
                            hat.Add("max_mark", splitmarks[4]);
                            hat.Add("min_mark", splitmarks[6]);
                            hat.Add("sections", secval);
                            hat.Add("new_maxmark", splitmarks[3]);
                            hat.Add("new_minmark", splitmarks[5]);
                            hat.Add("start_period", splitmarks[7]);
                            hat.Add("end_period", splitmarks[8]);
                            int insert = da.insert_method("sp_ins_upd_cam_exam_type_dead", hat, "sp");
                            Save.Visible = true;
                            Save.Enabled = true;
                            if (insert > 0)
                            {
                                // showsave = true;
                            }
                            else
                                showsave = false;
                            foreach (GridViewRow i in GridView3.Rows)
                            {
                                myconn.Close();
                                myconn.Open();
                                string sqlexam = string.Empty;
                                string resultupdate = string.Empty;
                                string resultinsert = string.Empty;
                                string getmark = string.Empty;
                                string AttnDay = string.Empty;
                                string temp_leave = string.Empty;
                                string update_value = string.Empty;
                                rollno = string.Empty;
                                mark = 0;

                                TextBox txt = (TextBox)i.FindControl("txttest");
                                getmark = txt.Text;
                                string[] gt = getmark.Split('.');
                                if (gt.Length > 1)
                                {
                                    if (!Convert.ToString(gt[0]).All(char.IsLetterOrDigit) || Convert.ToString(gt[1]).All(char.IsWhiteSpace))
                                    {

                                        divPopAlert.Visible = true;
                                        divPopAlertContent.Visible = true;
                                        lblAlertMsg.Text = "Invalid Characters!";
                                        Delete.Visible = true;
                                        Save.Visible = true;
                                        return;
                                    }
                                }
                                Label rolnum = (Label)i.FindControl("lblrollno");
                                rollno = rolnum.Text;
                                Label apno = (Label)i.FindControl("lblappno");
                                appNo = apno.Text;
                                Label colcod = (Label)i.FindControl("lblcollcode");
                                string collegeCodeNew = colcod.Text;
                                switch (getmark)
                                {
                                    case "AAA":
                                        getmark = "-1";
                                        break;
                                    case "EOD":
                                        getmark = "-3";
                                        break;
                                    case "OOD":
                                        getmark = "-15";
                                        break;
                                    case "EL":
                                        getmark = "-2";
                                        break;
                                    case "COD":
                                        getmark = "-14";
                                        break;
                                    case "ML":
                                        getmark = "-4";
                                        break;
                                    case "SOD":
                                        getmark = "-5";
                                        break;
                                    case "NSS":
                                        getmark = "-6";
                                        break;
                                    //case "H":
                                    //          getmark ="-7";
                                    //           break;
                                    case "NJ":
                                        getmark = "-7";
                                        break;
                                    case "S":
                                        getmark = "-8";
                                        break;
                                    case "L":
                                        getmark = "-9";
                                        break;
                                    case "NCC":
                                        getmark = "-10";
                                        break;
                                    case "HS":
                                        getmark = "-11";
                                        break;
                                    case "PP":
                                        getmark = "-12";
                                        break;
                                    case "SYOD":
                                        getmark = "-13";
                                        break;
                                    case "OD":
                                        getmark = "-16";
                                        break;
                                    case "LA":
                                        getmark = "-17";
                                        break;
                                    //****Modified By Subburaj 20.08.2014******//
                                    case "RAA":
                                        getmark = "-18";
                                        break;
                                    //****************End*****************//
                                }

                                //Modified by srinath 30/4/2014
                                if (GridView2.Columns[14].Visible == true && GridView2.Columns[15].Visible == true)
                                {
                                    if ((getmark.ToString() != "AAA") && (getmark.ToString() != "EOD") && (getmark.ToString() != "ML") && (getmark.ToString() != "NSS") && (getmark.ToString() != "SOD") && (getmark.ToString() != "H") && (getmark.ToString() != "S") && (getmark.ToString() != "NJ") && (getmark.ToString() != "L") && (getmark.ToString() != "PP") && (getmark.ToString() != "SYOD") && (getmark.ToString() != "COD") && (getmark.ToString() != "PP") && (getmark.ToString() != "OOD") && (getmark.ToString() != "HS") && (getmark.ToString() != "NCC") && (getmark.ToString() != "P") && (getmark.ToString() != "EL") && (getmark.ToString() != "OD") && (getmark.ToString() != "LA") && (getmark.ToString() != "RAA"))
                                    {
                                        Attmark(getmark);
                                        //'------------------------for attendance---------------------------------------
                                        if ((splitmarks[7].ToString() != "") && (splitmarks[8].ToString() != ""))
                                        {
                                            for (int attval = Convert.ToInt32(splitmarks[7].ToString()); attval <= Convert.ToInt32(splitmarks[8].ToString()); attval++)
                                            {
                                                DateTime dtent = Convert.ToDateTime(splitfinalexam[1].ToString() + '/' + splitfinalexam[0].ToString() + '/' + splitfinalexam[2].ToString());
                                                splitfinalexam[0] = dtent.Day.ToString();
                                                if (AttnDay == "")
                                                {
                                                    AttnDay = "d" + splitfinalexam[0].ToString() + "d" + attval;
                                                    Attvalues(Att_mark);
                                                    if (Attvalue != "")
                                                    {
                                                        temp_leave = Attvalue;
                                                        update_value = AttnDay + "=" + temp_leave;//'---for update
                                                    }
                                                }
                                                else
                                                {
                                                    AttnDay = AttnDay + " " + "," + " " + "d" + splitfinalexam[0].ToString() + "d" + attval;
                                                    Attvalues(Att_mark);
                                                    if (Attvalue != "")
                                                    {
                                                        temp_leave = temp_leave + "," + Attvalue;//'--------
                                                        update_value = update_value + "," + "d" + splitfinalexam[0].ToString() + "d" + attval + "=" + Attvalue; ;//'----------for update
                                                    }
                                                }
                                            }
                                        }
                                        //Modified by srinath 30/4/2014
                                        int mnthyr = 0;
                                        mnthyr = (Convert.ToInt32(splitfinalexam[1].ToString()) + (Convert.ToInt32(splitfinalexam[2].ToString()) * Convert.ToInt32(12)));
                                        hat.Clear();
                                        hat.Add("Att_App_no", appNo);
                                        hat.Add("Att_CollegeCode", collegeCodeNew);
                                        hat.Add("rollno", rollno);
                                        hat.Add("monthyear", mnthyr);
                                        hat.Add("columnname", AttnDay);
                                        hat.Add("colvalues", temp_leave);
                                        hat.Add("coulmnvalue", update_value);
                                        insert = da.insert_method("sp_ins_upd_student_attendance_Dead", hat, "sp");

                                    }
                                }
                                //'-----------------------------------end attendance-------------------------------------
                                if ((getmark.ToString() != "") && (getmark.ToString() != "AAA") && (getmark.ToString() != "EOD") && (getmark.ToString() != "ML") && (getmark.ToString() != "NSS") && (getmark.ToString() != "SOD") && (getmark.ToString() != "H") && (getmark.ToString() != "S") && (getmark.ToString() != "NJ") && (getmark.ToString() != "L") && (getmark.ToString() != "PP") && (getmark.ToString() != "SYOD") && (getmark.ToString() != "COD") && (getmark.ToString() != "PP") && (getmark.ToString() != "OOD") && (getmark.ToString() != "HS") && (getmark.ToString() != "NCC") && (getmark.ToString() != "EL") && (getmark.ToString() != "OD") && (getmark.ToString() != "LA") && (getmark.ToString() != "RAA"))
                                {
                                    mark = Convert.ToDouble(getmark.ToString());
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(getmark.Trim()))
                                    {
                                        getmark = getmark.Trim();
                                    }
                                    else
                                    {
                                        mark = 0;
                                    }
                                }

                                //Modified by srinath 30/4/2014 ========Start=============
                                string secvalue = string.Empty;
                                //Aruna 18sep2014 if (secvalue.Trim() != "" && secvalue != null && secvalue.Trim() != "-1")
                                if (sections != null && sections.Trim().ToLower() != "all" && sections.Trim() != "" && sections.Trim() != "-1")
                                {
                                    secvalue = " and sections='" + sections + "'";
                                }
                                if (string.IsNullOrEmpty(exam_code) || exam_code == "0")
                                {
                                    exam_code = da.GetFunction("select exam_code from exam_type where  criteria_no=" + criteria_no + " and subject_no = " + subjectnum + " " + secvalue + " and batch_year=" + batchyr2 + "");
                                    if (string.IsNullOrEmpty(exam_code) || exam_code.Trim() == "0")
                                    {
                                        exam_code = string.Empty;
                                    }
                                }
                                if (!string.IsNullOrEmpty(exam_code) || exam_code.Trim() != "0" || exam_code.ToLower() != "ne")
                                {

                                    int delval = 0;
                                    if (chkretest.Checked == true)
                                    {
                                        //=================Modified By Srinath 6/11/2015
                                        // delval = Convert.ToInt32(da.update_method_wo_parameter("delete from tbl_result_retest where Exam_Code='" + exam_code + "' and Roll_No='" + rollno + "'", "Text"));
                                        delval = Convert.ToInt32(da.update_method_wo_parameter("Update result set Retest_Marks_obtained=null where Exam_Code='" + exam_code + "' and Roll_No='" + rollno + "'", "Text"));
                                    }
                                    if (chkretest.Checked == true)
                                    {

                                        TextBox retst = (TextBox)i.FindControl("txtretest");
                                        string getma = retst.Text;
                                        string[] gt1 = getma.Split('.');
                                        if (gt1.Length > 1)
                                        {
                                            if (!Convert.ToString(gt1[0]).All(char.IsLetterOrDigit) || Convert.ToString(gt1[1]).All(char.IsWhiteSpace))
                                            {
                                                divPopAlert.Visible = true;
                                                divPopAlertContent.Visible = true;
                                                lblAlertMsg.Text = "Invalid Characters!";
                                                Delete.Visible = true;
                                                Save.Visible = true;
                                                return;
                                            }
                                        }
                                        if (getma.Trim() == "")
                                        {
                                            getma = string.Empty;
                                            getmark = string.Empty;
                                            mark = 0;
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(getmark.Trim()))
                                            {
                                                getma = mark.ToString();
                                            }
                                            else
                                            {
                                                getma = string.Empty;
                                            }
                                            TextBox retst1 = (TextBox)i.FindControl("txtretest");
                                            string mk = retst1.Text;
                                            if (!Convert.ToString(mk).All(char.IsLetterOrDigit))
                                            {
                                                divPopAlert.Visible = true;
                                                divPopAlertContent.Visible = true;
                                                lblAlertMsg.Text = "Invalid Characters!";
                                                Delete.Visible = true;
                                                Save.Visible = true;
                                                return;
                                            }
                                            mark = Convert.ToInt32(mk);
                                            getmark = Convert.ToString(mark);
                                        }
                                        if (getma.Trim() != "")
                                        {
                                            delval = Convert.ToInt32(da.update_method_wo_parameter("Update result set Retest_Marks_obtained='" + getma + "' where Exam_Code='" + exam_code + "' and Roll_No='" + rollno + "'", "Text"));
                                        }
                                    }
                                   // insert = 0;
                                    if (!string.IsNullOrEmpty(getmark.Trim()))
                                    {
                                        hat.Clear();
                                        hat.Add("roll_no", rollno);
                                        hat.Add("exam_code", exam_code);
                                        hat.Add("marks_obtained", mark);
                                        insert = da.insert_method("sp_ins_upd_cam_mark_dead", hat, "sp");
                                        if (insert > 0)
                                        {
                                            savedstudct++;
                                        }
                                        else
                                        {
                                            notsavedstudct++;
                                            showsave = false;
                                        }
                                    }
                                    Save.Visible = true;
                                    Save.Enabled = true;
                                   
                                }
                                else
                                {
                                    pHeaderEntry.Visible = true;
                                    pHeaderReport.Visible = true;
                                    pHeaderSettings.Visible = true;
                                    Delete.Visible = true;
                                    Delete.Enabled = true;
                                    Label2.Visible = true;
                                    lblnote2.Visible = true;
                                    lblNote3.Visible = true;
                                    lbltab.Visible = true;
                                    divPopAlert.Visible = true;
                                    divPopAlertContent.Visible = true;
                                    lblAlertMsg.Text = "Error Occured!";
                                    Delete.Visible = true;
                                    Save.Visible = true;
                                    return;
                                }
                            }
                        }
                        else
                            showsave = false;

                        pHeaderEntry.Visible = true;
                        pHeaderReport.Visible = true;
                        pHeaderSettings.Visible = true;
                        Delete.Visible = true;
                        Delete.Enabled = true;
                        Label2.Visible = true;
                        lblnote2.Visible = true;
                        lblNote3.Visible = true;
                        lbltab.Visible = true;
                        string entrycode = Session["Entry_Code"].ToString();
                        string PageName = "Student Mark Entry";
                        string section = ddlSec.SelectedValue.ToString();
                        string batchYear = ddlBatch.SelectedValue.ToString();
                        string Semester = ddlSemYr.SelectedValue.ToString();
                        string degreeCode = ddlBranch.SelectedValue.ToString();
                        string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                        string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["Btach_Year"])))
                            Btach_Year_Val = Convert.ToString(Session["Btach_Year"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["Semester"])))
                            Semester_Val = Convert.ToString(Session["Semester"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["Degree_Code"])))
                            Degree_Code_Val = Convert.ToString(Session["Degree_Code"]);
                        if (showsave == true)
                        {
                            if (Save.Text == "Save")
                            {

                                string ctsname = "Save the Student Mark Information";

                                if (string.IsNullOrEmpty(Btach_Year_Val) || string.IsNullOrEmpty(Semester_Val) || string.IsNullOrEmpty(Degree_Code_Val))
                                    da.insertUserActionLog(entrycode, batchYear, degreeCode, Semester, section, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 1);
                                else
                                    da.insertUserActionLog(entrycode, Btach_Year_Val, Degree_Code_Val, Semester_Val, section, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 1);
                                string std=string.Empty;
                                if(savedstudct==1)
                                    std=" Student";
                                else
                                    std = " Students";

                                divPopAlert.Visible = true;
                                divPopAlertContent.Visible = true;
                                lblAlertMsg.Text = "Saved Successfully For " + savedstudct + std;
                                Delete.Visible = true;
                                Save.Visible = true;

                                Save.Text = "Update";
                                return;
                            }
                            else
                            {
                                string ctsname = "Updated the Student Mark Information";

                                if (string.IsNullOrEmpty(Btach_Year_Val) || string.IsNullOrEmpty(Semester_Val) || string.IsNullOrEmpty(Degree_Code_Val))
                                    da.insertUserActionLog(entrycode, batchYear, degreeCode, Semester, section, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 2);
                                else
                                    da.insertUserActionLog(entrycode, Btach_Year_Val, Degree_Code_Val, Semester_Val, section, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 2);
                                string std = string.Empty;
                                if (savedstudct == 1)
                                    std = " Student";
                                else
                                    std = " Students";
                                divPopAlert.Visible = true;
                                divPopAlertContent.Visible = true;
                                lblAlertMsg.Text = "Updated successfully For " + savedstudct + std;
                                Delete.Visible = true;
                                Save.Visible = true;
                                return;
                            }//End saranya16.11.2017
                            txt_RetestMin.Text = string.Empty;
                            chkretest.Checked = false;
                            lblRetestMin.Visible = false;
                            txt_RetestMin.Visible = false;
                        }
                        else
                        {
                            divPopAlert.Visible = true;
                            divPopAlertContent.Visible = true;
                            lblAlertMsg.Text = "Not Saved";
                            Delete.Visible = true;
                            Save.Visible = true;
                        }
                    }
                    else
                    {
                        lblexcelerror.Text = "Please Select Staff Name";
                        lblexcelerror.Visible = true;
                    }
                }
                Cellclick = true;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "CAM Entry");
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            Label2.Visible = true;
            lblnote2.Visible = true;
            lblNote3.Visible = true;
            lbltab.Visible = true;
            Save.Visible = true;
            Save.Enabled = true;
           // Save.Text = "Update";
            Delete.Enabled = true;
            Delete.Visible = true;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }

        catch (Exception ex)
        {

        }
    }


    //'-----------------------------------func for attendance----------------------------
    public string Attmark(string Attstr_mark)
    {
        Att_mark = string.Empty;
        //if (Attstr_mark == "")
        //{
        //    Attstr_mark = "0";
        //}
        if (Attstr_mark == "-1")
        {
            Att_mark = "AAA";
        }
        else if (Attstr_mark == "-2")
        {
            Att_mark = "EL";
        }
        else if (Attstr_mark == "-3")
        {
            Att_mark = "EOD";
        }
        if (Attstr_mark == "-4")
        {
            Att_mark = "ML";
        }
        else if (Attstr_mark == "-5")
        {
            Att_mark = "SOD";
        }
        else if (Attstr_mark == "-6")
        {
            Att_mark = "NSS";
        }
        //else if (Attstr_mark == "-7")
        //{
        //    Att_mark = "H";
        //}
        if (Attstr_mark == "-7")
        {
            Att_mark = "NJ";
        }
        else if (Attstr_mark == "-8")
        {
            Att_mark = "S";
        }
        else if (Attstr_mark == "-9")
        {
            Att_mark = "L";
        }
        else if (Attstr_mark == "-10")
        {
            Att_mark = "NCC";
        }
        else if (Attstr_mark == "-11")
        {
            Att_mark = "HS";
        }
        else if (Attstr_mark == "-12")
        {
            Att_mark = "PP";
        }
        else if (Attstr_mark == "-13")
        {
            Att_mark = "SYOD";
        }
        else if (Attstr_mark == "-14")
        {
            Att_mark = "COD";
        }
        else if (Attstr_mark == "-15")
        {
            Att_mark = "OOD";
        }
        else if (Attstr_mark == "-16")
        {
            Att_mark = "OD";
        }
        else if (Attstr_mark == "-17")
        {
            Att_mark = "LA";
        }
        //****Modified By Subburaj 20.08.2014******//
        else if (Attstr_mark == "-18")
        {
            Att_mark = "RAA";
        }
        //**************End******************//
        else if ((Convert.ToDouble(Attstr_mark) >= Convert.ToDouble(0)) && (Attstr_mark != ""))
        {
            Att_mark = "P";
        }
        return Att_mark;
    }

    //-------------------------------func for storing the attendance values============
    public string Attvalues(string Att_str1)
    {
        //  "P", "A", "OD", "SOD", "ML", "NSS", "L", "NJ", "S", "NCC", "HS", "PP", "SYOD", "COD", "OOD"
        Attvalue = string.Empty;
        if (Att_str1 == "P")
        {
            Attvalue = "1";
        }
        else if ((Att_str1 == "A") || (Att_str1 == "AAA"))
        {
            Attvalue = "2";
        }
        else if (Att_str1 == "OD")
        {
            Attvalue = "3";
        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = "5";
        }
        else if (Att_str1 == "ML")
        {
            Attvalue = "4";
        }
        else if (Att_str1 == "NSS")
        {
            Attvalue = "6";
        }
        else if (Att_str1 == "L")
        {
            Attvalue = "10";
        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = "11";
        }
        else if (Att_str1 == "HS")
        {
            Attvalue = "12";
        }
        else if (Att_str1 == "PP")
        {
            Attvalue = "13";
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = "14";
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = "15";
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = "16";
        }
        else if (Att_str1 == "LA")
        {
            Attvalue = "17";
        }
        //****Modified By Subburaj 20.08.2014******//
        else if (Att_str1 == "RAA")
        {
            Attvalue = "18";
        }
        //**************End*******//
        //else if(Att_str1=="H")
        //{
        //    Attvalue = "7";
        //}
        else if (Att_str1 == "NJ")
        {
            Attvalue = "8";
        }
        else if (Att_str1 == "S")
        {
            Attvalue = "9";
        }
        //else if (Att_str1 == "EOD")
        //{
        //    Attvalue = "17";
        //}
        //else if (Att_str1 == "EL")
        //{
        //    Attvalue = "18";
        //}

        return Attvalue;
    }

    //-------------------------------------------------------------function defn for Fpentry Selectedchanged

    //public void callentryselect()
    //{
    //    //try
    //    //{
    //    FpReport.SaveChanges();
    //    // saveflag = true;
    //    int testmrk = 0;
    //    if (Cellclick == true)
    //    {
    //        string datelock = GetFunction("select value from master_settings where settings='Cam Date Lock' and " + grouporusercode + "");
    //        if (datelock.Trim() != "")
    //        {
    //            datelocksetting = datelock;
    //        }
    //        else
    //        {
    //            datelocksetting = "0";
    //        }
    //        FpSettings.SaveChanges();
    //        FpSettings.Sheets[0].FrozenColumnCount = 5;
    //        FpSettings.Sheets[0].RowCount = 0;




    //        Exit1.Visible = false;
    //        pHeaderEntry.Visible = true;
    //        pHeaderReport.Visible = true;
    //        pHeaderSettings.Visible = true;
    //        lblErrorMsg.Visible = false;
    //        lblnorec.Text = string.Empty;
    //        txt_RetestMin.Text = string.Empty;
    //        chkretest.Checked = false;
    //        // lblnorec.Visible = false;
    //        // lblrecord.Visible = true;
    //        //lblpage.Visible = true;
    //        lblPageSearch.Visible = true;
    //        // Buttontotal.Visible = true;
    //        //TextBoxother.Visible = true;
    //        // TextBoxpage.Visible = true;
    //        Label2.Visible = true;
    //        lblnote2.Visible = true;
    //        lblNote3.Visible = true;
    //        // Button1.Visible = true;
    //        // DropDownListpage.Visible = true;
    //        //Panel1.Visible = true;
    //        FpReport.Visible = true;
    //        FpSettings.Visible = true;
    //        lblrptname.Visible = true;
    //        txtexcelname.Visible = true;
    //        btnExcel.Visible = true;
    //        Buttonexit.Visible = true;
    //        btnok.Visible = true;
    //        Exit1.Visible = true;
    //        chkretest.Visible = true;
    //        lblselectstaff.Visible = true;
    //        ddlstaffname.Visible = true;
    //        chkGrp.Checked = false;
    //        string staff_code = string.Empty;
    //        staff_code = (string)Session["staff_code"].ToString();
    //        if (staff_code != "")
    //        {
    //            lblselectstaff.Visible = false;
    //            ddlstaffname.Visible = false;
    //            Save.Visible = false;
    //            Delete.Visible = false;
    //            RequiredFieldValidator1.Visible = false;
    //            RequiredFieldValidator2.Visible = false;
    //            RequiredFieldValidator3.Visible = false;
    //            RequiredFieldValidator4.Visible = false;
    //            RequiredFieldValidator5.Visible = false;
    //            // Exit.Visible = true;
    //            if ((FpSettings.Sheets[0].RowCount == 0) || (FpReport.Sheets[0].RowCount == 0))
    //            {
    //                //  Exit1.Visible = true;
    //                // FpEntry.Visible = false;
    //                FpReport.Visible = false;
    //                FpSettings.Visible = false;
    //                lblrptname.Visible = false;
    //                txtexcelname.Visible = false;
    //                btnExcel.Visible = false;
    //                Buttonexit.Visible = false;
    //                pHeaderReport.Visible = false;
    //                pHeaderSettings.Visible = false;
    //                lblselectstaff.Visible = false;
    //                ddlstaffname.Visible = false;
    //                //  pHeaderEntry.Visible = false;
    //            }
    //            else
    //            {
    //                // Exit.Visible = true;
    //                // FpEntry.Visible = true;
    //                Exit1.Visible = false;
    //                FpReport.Visible = true;
    //                FpSettings.Visible = true;
    //                lblrptname.Visible = true;
    //                txtexcelname.Visible = true;
    //                btnExcel.Visible = true;
    //                Buttonexit.Visible = true;
    //                pHeaderReport.Visible = true;
    //                pHeaderSettings.Visible = true;
    //                lblselectstaff.Visible = false;
    //                ddlstaffname.Visible = false;
    //                // pHeaderEntry.Visible = true;
    //            }
    //        }
    //        else
    //        {
    //            RequiredFieldValidator1.Visible = true;
    //            RequiredFieldValidator2.Visible = true;
    //            RequiredFieldValidator3.Visible = true;
    //            RequiredFieldValidator4.Visible = true;
    //            RequiredFieldValidator5.Visible = true;
    //            FpReport.Visible = true;
    //            lblselectstaff.Visible = true;
    //            ddlstaffname.Visible = true;
    //            //Exit.Visible = true;
    //        }
    //        //---------------------------------------------------- active row for the fpentry
    //        string activerow = string.Empty;
    //        string activecol = string.Empty;
    //        activerow = FpEntry.ActiveSheetView.ActiveRow.ToString();
    //        activecol = FpEntry.ActiveSheetView.ActiveColumn.ToString();



    //        int ar;
    //        int ac;
    //        ar = Convert.ToInt32(activerow.ToString());
    //        ac = Convert.ToInt32(activecol.ToString());

    //        //----------------RAJWork

    //        lblSubNo.Text = Convert.ToString(FpEntry.Sheets[0].Cells[ar, 5].Tag).Trim();


    //        Btach_Year_Val = string.Empty;
    //        Semester_Val = string.Empty;
    //        Degree_Code_Val = string.Empty;
    //        if (staff_code != "")
    //        {
    //            Btach_Year_Val = FpEntry.Sheets[0].Cells[ar, 1].Text.Trim();
    //            Semester_Val = FpEntry.Sheets[0].Cells[ar, 3].Text.Trim();
    //            Degree_Code_Val = Convert.ToString(FpEntry.Sheets[0].Cells[ar, 2].Tag).Trim();

    //            Session["Btach_Year"] = Btach_Year_Val;
    //            Session["Semester"] = Semester_Val;
    //            Session["Degree_Code"] = Degree_Code_Val;
    //        }

    //        //-------------EndRaj

    //        if (ar != -1)
    //        {
    //            FpReport.Sheets[0].ColumnCount = 14;
    //            myconn.Close();
    //            myconn.Open();
    //            int rowcnt = 0;
    //            string sqlStr = string.Empty;
    //            string subno = string.Empty;
    //            string batch = string.Empty;
    //            string sections = string.Empty;
    //            string strsec = string.Empty;
    //            int rerowcnt = 0;
    //            string semester = string.Empty;
    //            string degreecode = string.Empty;
    //            //string rollno =string.Empty;//26.03.12
    //            //------------------------------------------------- ---chkbox celltype for the select clmn
    //            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
    //            FpReport.Sheets[0].Columns[0].CellType = chkcell;
    //            //----------------------------------------------------To display the dropdown values in the cell(spread2)
    //            string[] cbstrmonth;
    //            cbstrmonth = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
    //            string[] cbstr1date;
    //            cbstr1date = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
    //            //Get Server Date===========================================
    //            //SELECT distinct cast(datepart(m,getdate()) as nvarchar) + '/' + cast(datepart(d,getdate()) as nvarchar) + '/' + cast(datepart(yyyy,getdate()) as nvarchar) ,cast(datepart(hh,getdate()) as nvarchar) + ':' + cast(datepart(n,getdate()) as nvarchar) + ':' + cast(datepart(s,getdate()) as nvarchar)
    //            string current_date = GetFunction("SELECT distinct cast(datepart(m,getdate()) as nvarchar) + '/' + cast(datepart(d,getdate()) as nvarchar) + '/' + cast(datepart(yyyy,getdate()) as nvarchar)");
    //            string date1 = string.Empty;
    //            string month1 = string.Empty;
    //            string year1 = string.Empty;
    //            if (current_date.Trim().ToString() != "")
    //            {
    //                string[] splitdate = current_date.Split(new char[] { '/' });
    //                date1 = splitdate[1].ToString();
    //                month1 = splitdate[0].ToString();
    //                year1 = splitdate[2].ToString();
    //                if (date1.Length == 1)
    //                {
    //                    date1 = "0" + date1;
    //                }
    //                if (month1.Length == 1)
    //                {
    //                    month1 = "0" + month1;
    //                }
    //            }
    //            //==========================================================
    //            FarPoint.Web.Spread.ComboBoxCellType cmbcel1 = new FarPoint.Web.Spread.ComboBoxCellType(cbstr1date);
    //            FpReport.Sheets[0].Columns[2].CellType = cmbcel1;
    //            FarPoint.Web.Spread.ComboBoxCellType cmbcel2 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrmonth);
    //            FpReport.Sheets[0].Columns[3].CellType = cmbcel2;
    //            string stryr = string.Empty;
    //            stryr = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''order by batch_year";
    //            SqlCommand cmdyr = new SqlCommand(stryr, yrcon);
    //            yrcon.Close();
    //            yrcon.Open();
    //            SqlDataAdapter dayr = new SqlDataAdapter(cmdyr);
    //            DataSet dsyr = new DataSet();
    //            dayr.Fill(dsyr);
    //            string sqlstr = "select distinct max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
    //            int max_bat = Convert.ToInt32(GetFunction(sqlstr)) + 1;
    //            ArrayList year = new ArrayList();
    //            SqlDataReader dr = cmdyr.ExecuteReader();
    //            if (dr.HasRows)
    //            {
    //                while (dr.Read())
    //                {
    //                    year.Add(dr.GetValue(0).ToString());
    //                }
    //            }
    //            string[] alyear = new string[year.Count + 1];
    //            int year3 = Convert.ToInt16(DateTime.Today.Year);
    //            //added by gowtham
    //            if (year.Contains(year3.ToString()) != true)
    //            {
    //                year.Add(year3.ToString());
    //            }
    //            int ks = 0;
    //            for (int r = 0; r < year.Count; r++)
    //            {
    //                alyear[r] = year[r].ToString();
    //                ks = r;
    //            }
    //            dr.Close();
    //            FarPoint.Web.Spread.ComboBoxCellType cmbyr = new FarPoint.Web.Spread.ComboBoxCellType(alyear);
    //            FpReport.Sheets[0].Columns[4].CellType = cmbyr;
    //            FarPoint.Web.Spread.ComboBoxCellType cmbyr2 = new FarPoint.Web.Spread.ComboBoxCellType(alyear);
    //            //cmbyr2.DataSource = dsyr;
    //            //cmbyr2.DataTextField = "batch_year";
    //            //cmbyr2.DataValueField = "batch_year";
    //            FpReport.Sheets[0].Columns[7].CellType = cmbyr2;
    //            yrcon.Close();
    //            FarPoint.Web.Spread.ComboBoxCellType cmbcel4 = new FarPoint.Web.Spread.ComboBoxCellType(cbstr1date);
    //            FpReport.Sheets[0].Columns[5].CellType = cmbcel4;
    //            FarPoint.Web.Spread.ComboBoxCellType cmbcel5 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrmonth);
    //            FpReport.Sheets[0].Columns[6].CellType = cmbcel5;
    //            string[] cbstrhour;
    //            cbstrhour = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };
    //            FarPoint.Web.Spread.ComboBoxCellType cmbcel7 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrhour);
    //            FpReport.Sheets[0].Columns[8].CellType = cmbcel7;
    //            string[] cbstrmin;
    //            cbstrmin = new string[] { "00", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55", "60" };
    //            FarPoint.Web.Spread.ComboBoxCellType cmbcel8 = new FarPoint.Web.Spread.ComboBoxCellType(cbstrmin);
    //            FpReport.Sheets[0].Columns[9].CellType = cmbcel8;
    //            //'-------------------------------------------------------
    //            string strperiod = "select max(No_of_hrs_per_day) from PeriodAttndSchedule";
    //            int noofhours = Convert.ToInt32(GetFunction(strperiod));
    //            string temp1 = string.Empty;
    //            for (int item = 1; item <= noofhours; item++)
    //            {
    //                if (temp1 == "")
    //                {
    //                    temp1 = item.ToString();
    //                }
    //                else
    //                {
    //                    temp1 = temp1 + "," + item.ToString();
    //                }
    //            }
    //            string[] split_temp = temp1.Split(new char[] { ',' });
    //            FarPoint.Web.Spread.ComboBoxCellType cmbcel9 = new FarPoint.Web.Spread.ComboBoxCellType(split_temp);
    //            FpReport.Sheets[0].Columns[12].CellType = cmbcel9;
    //            FpReport.Sheets[0].Columns[13].CellType = cmbcel9;
    //            //----------------------------------------------------- spaning the columns in the fpreport
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 3);
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Date";
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Month";
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Year";
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 3);
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Date";
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Month";
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Year";
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 2);
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Hrs";
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Min";
    //            FpReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 1, 2);
    //            FpReport.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Period";
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 12].Text = "Start";
    //            FpReport.Sheets[0].ColumnHeader.Cells[1, 13].Text = "End";
    //            FpReport.Sheets[0].Columns[12].Width = 40;
    //            FpReport.Sheets[0].Columns[13].Width = 40;
    //            string markattendance = da.GetFunction("select value from Master_Settings where settings='cam mark attendance'");
    //            //if (markattendance.Trim() != "0" && markattendance.Trim() != "" && markattendance != null)
    //            if (chkmarkattendance.Checked == true && chkmarkattendance.Visible == true)
    //            {
    //                FpReport.Sheets[0].Columns[12].Visible = true;
    //                FpReport.Sheets[0].Columns[13].Visible = true;
    //            }
    //            else
    //            {
    //                FpReport.Sheets[0].Columns[12].Visible = false;
    //                FpReport.Sheets[0].Columns[13].Visible = false;
    //            }
    //            FpEntry.SaveChanges();
    //            rerowcnt = ar;
    //            FpReport.Sheets[0].RowCount = 0;
    //            semester = FpEntry.Sheets[0].Cells[rerowcnt, 3].Text.Trim(); // jairam
    //            degreecode = FpEntry.Sheets[0].Cells[rerowcnt, 2].Tag.ToString();
    //            batch = FpEntry.Sheets[0].Cells[rerowcnt, 1].Text.Trim();
    //            subno = FpEntry.Sheets[0].Cells[rerowcnt, 5].Tag.ToString();
    //            sections = FpEntry.Sheets[0].Cells[rerowcnt, 4].Text.Trim();
    //            lblSub.Text = "Test Details - " + FpEntry.Sheets[0].Cells[rerowcnt, 6].Text.ToString() + " - " + FpEntry.Sheets[0].Cells[rerowcnt, 5].Text.ToString() + " ";
    //            if (staff_code.Trim() == "")
    //            {
    //                string sec_value = string.Empty;
    //                if (ddlSec.Enabled == true)
    //                {
    //                    if (sections.Trim().ToLower() != "all" && sections.Trim() != "" || sections.Trim() != "-1" || sections == null)
    //                    {
    //                        sec_value = " and Sections='" + sections + "'";
    //                    }
    //                }
    //                string selectquery = "select ss.staff_code,staff_name from staff_selector ss,staffmaster sm where ss.staff_code =sm.staff_code and batch_year =" + batch + " and subject_no ='" + subno + "' " + sec_value + " ";
    //                DataSet dnew = new DataSet();
    //                dnew.Clear();
    //                dnew = da.select_method_wo_parameter(selectquery, "Text"); // added by jairam 2015-07-10
    //                if (dnew.Tables[0].Rows.Count > 0)
    //                {
    //                    ddlstaffname.DataSource = dnew;
    //                    ddlstaffname.DataTextField = "staff_name";
    //                    ddlstaffname.DataValueField = "staff_code";
    //                    ddlstaffname.DataBind();
    //                    ddlstaffname.Items.Insert(0, "Select");
    //                }
    //                else
    //                {
    //                    ddlstaffname.Items.Clear();
    //                }
    //            }
    //            //---------------------------------------------------query for select the group code
    //            string sqlcheck = string.Empty;
    //            sqlcheck = "select distinct groupcode from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + degreecode.ToString() + " and semester=" + semester.ToString() + "  and batch_year=" + batch.ToString() + " and groupcode<>'' ";
    //            //sqlcheck = "select distinct groupcode from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + bind[3].ToString() + " and semester=" + bind[4].ToString() + "  and batch_year=" + bind[1].ToString() + " and groupcode<>'' ";
    //            SqlCommand cmdchk = new SqlCommand(sqlcheck, myconn);
    //            SqlDataReader grpreader;
    //            grpreader = cmdchk.ExecuteReader();
    //            if (grpreader.HasRows == true)
    //            {
    //                ddlGrp.Enabled = true;
    //                ddlGrp.DataValueField = "groupcode";
    //                ddlGrp.DataSource = grpreader;
    //                ddlGrp.DataBind();
    //                ddlGrp.Items.Add("");
    //                //ddlGrp.Enabled = false;
    //            }
    //            if (sections.Trim().ToLower() == "all" || sections.Trim() == "" || sections == "-1" || sections == null)
    //            {
    //                strsec = string.Empty;
    //            }
    //            else
    //            {
    //                strsec = " and sections='" + sections + "'";
    //            }
    //            string bind = string.Empty;
    //            bind = subno + "-" + batch + "-" + sections + "-" + degreecode + "-" + semester;
    //            //------------------------------------------- Query for display the Testname,max,min marks,date and duration in the spread2-Fpreport
    //            if (staff_code != "")
    //            {
    //                if (chkGrp.Checked = true && ddlGrp.SelectedValue != "")
    //                {
    //                    sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + " " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " and staff_code= ' " + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "') and groupcode='" + ddlGrp.Text.ToString() + "'";
    //                }
    //                else
    //                {
    //                    sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + " " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no =' " + subno.ToString() + " 'and batch_year = " + batch.ToString() + " and staff_code= ' " + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";
    //                }
    //            }
    //            else
    //            {
    //                if (chkGrp.Checked = true && ddlGrp.SelectedValue != "")
    //                {
    //                    sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + " '" + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and groupcode='" + ddlGrp.Text.ToString() + "'";
    //                }
    //                else
    //                {
    //                    sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + "' " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";
    //                }
    //            }
    //            string strgetlestval = "select * from exam_type where subject_no='" + subno.ToString() + "' " + strsec.ToString() + "";
    //            DataSet dsexmtype = da.select_method_wo_parameter(strgetlestval, "text");
    //            readcon.Close();
    //            readcon.Open();
    //            SqlCommand cmd_read = new SqlCommand(sqlStr, readcon);
    //            SqlDataReader reader;
    //            reader = cmd_read.ExecuteReader();
    //            rowcnt = 0;
    //            if (reader.HasRows == true)
    //            {
    //                FpSettings.Sheets[0].ColumnCount = 5;
    //                pHeaderReport.Visible = true;
    //                pHeaderSettings.Visible = true;
    //                FpReport.Visible = true;
    //                if (Session["Rollflag"].ToString() == "0")
    //                {
    //                    FpSettings.Sheets[0].ColumnHeader.Columns[0].Visible = false;
    //                }
    //                if (Session["Regflag"].ToString() == "0")
    //                {
    //                    FpSettings.Sheets[0].ColumnHeader.Columns[1].Visible = false;
    //                }
    //                if (Session["Studflag"].ToString() == "0")
    //                {
    //                    FpSettings.Sheets[0].ColumnHeader.Columns[3].Visible = false;
    //                }
    //                SqlDataReader serial_dr;
    //                con.Close();
    //                con.Open();
    //                SqlCommand cmd = new SqlCommand("select LinkValue from inssettings where college_code='" + Session["collegecode"].ToString() + "' and linkname='Student Attendance'", con);
    //                serial_dr = cmd.ExecuteReader();
    //                while (serial_dr.Read())
    //                {
    //                    if (serial_dr["LinkValue"].ToString() == "1")
    //                    {
    //                        serialflag = true;
    //                    }
    //                    else
    //                    {
    //                        serialflag = false;
    //                    }
    //                }
    //                //Added By Srinath 7/2/2013 =====Start
    //                string strorderby = GetFunction("select value from Master_Settings where settings='order_by'");
    //                if (strorderby == "")
    //                {
    //                    strorderby = string.Empty;
    //                }
    //                else
    //                {
    //                    if (strorderby == "0")
    //                    {
    //                        strorderby = "ORDER BY registration.Roll_No";
    //                    }
    //                    else if (strorderby == "1")
    //                    {
    //                        strorderby = "ORDER BY registration.Reg_No";
    //                    }
    //                    else if (strorderby == "2")
    //                    {
    //                        strorderby = "ORDER BY Registration.Stud_Name";
    //                    }
    //                    else if (strorderby == "0,1,2")
    //                    {
    //                        strorderby = "ORDER BY registration.Roll_No,registration.Reg_No,Registration.Stud_Name";
    //                    }
    //                    else if (strorderby == "0,1")
    //                    {
    //                        strorderby = "ORDER BY registration.Roll_No,registration.Reg_No";
    //                    }
    //                    else if (strorderby == "1,2")
    //                    {
    //                        strorderby = "ORDER BY registration.Reg_No,Registration.Stud_Name";
    //                    }
    //                    else if (strorderby == "0,2")
    //                    {
    //                        strorderby = "ORDER BY registration.Roll_No,Registration.Stud_Name";
    //                    }
    //                }
    //                //---------------------------------Query for display the Student details in the Spread3 FpSettings-- and Settings value to be passed in this query-
    //                //******************************************End*************************************************************
    //                string[] splitvals = bind.Split(new char[] { '-' });
    //                string strstaffselecotr = string.Empty;
    //                Session["StaffSelector"] = "0";
    //                strstaffselecotr = string.Empty;
    //                string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
    //                string[] splitminimumabsentsms = staffbatchyear.Split('-');
    //                if (splitminimumabsentsms.Length == 2)
    //                {
    //                    int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
    //                    if (splitminimumabsentsms[0].ToString() == "1")
    //                    {
    //                        if (Convert.ToInt32(splitvals[1].ToString()) >= batchyearsetting)
    //                        {
    //                            Session["StaffSelector"] = "1";
    //                        }
    //                    }
    //                }
    //                if (Session["StaffSelector"].ToString() == "1")
    //                {
    //                    if (Session["Staff_Code"] != null)
    //                    {
    //                        if (Session["Staff_Code"].ToString().Trim() != "" && Session["Staff_Code"].ToString().Trim() != "0")
    //                        {
    //                            strstaffselecotr = " and SubjectChooser.staffcode like '%" + Session["Staff_Code"].ToString() + "%' ";
    //                        }
    //                    }
    //                }
    //                if (serialflag == false)
    //                {
    //                    //sqlStr = "Select distinct len(registration.roll_no),registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,registration.App_No as ApplicationNumber from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code =" + splitvals[3].ToString() + " and Semester = " + splitvals[4].ToString() + " and registration.Batch_Year = " + splitvals[1].ToString() + " and Subject_No = " + splitvals[0].ToString() + " " + strsec + " and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and Semester = " + splitvals[4].ToString() + "  " + Session["strvar"] + " order by len(registration.roll_no),registration.roll_no";
    //                    //sqlStr = "Select distinct len(registration.roll_no),registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,registration.App_No as ApplicationNumber from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code =" + splitvals[3].ToString() + " and Semester = " + splitvals[4].ToString() + " and registration.Batch_Year = " + splitvals[1].ToString() + " and Subject_No = " + splitvals[0].ToString() + " " + strsec + " and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and Semester = " + splitvals[4].ToString() + "  " + Session["strvar"] + " " + strstaffselecotr + " " + strorderby + "";//Modified By Srinath 13/3/20123
    //                    sqlStr = "Select distinct len(registration.roll_no),registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,registration.App_No as ApplicationNumber,registration.college_code from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + splitvals[3].ToString() + "' and Semester = '" + splitvals[4].ToString() + "' and registration.Batch_Year = '" + splitvals[1].ToString() + "' and Subject_No = '" + splitvals[0].ToString() + "' " + strsec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + splitvals[4].ToString() + "'   " + strstaffselecotr + " " + strorderby + "";//Modified By Srinath 10/4/2014//" + Session["strvar"] + "
    //                }
    //                else
    //                {
    //                    //sqlStr = "Select  registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,registration.App_No as ApplicationNumber   from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code =" + splitvals[3].ToString() + " and Semester = " + splitvals[4].ToString() + " and registration.Batch_Year = " + splitvals[1].ToString() + " and Subject_No = " + splitvals[0].ToString() + " " + strsec + " and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and Semester = " + splitvals[4].ToString() + "  " + Session["strvar"] + " order by registration.serialno";
    //                    sqlStr = "Select  serialno,registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,registration.App_No as ApplicationNumber,registration.college_code from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + splitvals[3].ToString() + "' and Semester = '" + splitvals[4].ToString() + "' and registration.Batch_Year = '" + splitvals[1].ToString() + "' and Subject_No = '" + splitvals[0].ToString() + "' " + strsec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + splitvals[4].ToString() + "'  " + strstaffselecotr + "  ORDER BY serialno";//Modified By Srinath 13/3/20123
    //                    //==========End
    //                }
    //                //Modified by srinath 30/4/2014
    //                //myconn.Close();
    //                //myconn.Open();
    //                //SqlCommand command = new SqlCommand(sqlStr, myconn);
    //                //SqlDataAdapter da = new SqlDataAdapter(command);
    //                //DataSet ds = new DataSet();
    //                //da.Fill(ds);
    //                DataSet ds = da.select_method_wo_parameter(sqlStr, "Text");
    //                //FpSettings.DataSource = ds;
    //                //FpSettings.DataBind();
    //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
    //                {
    //                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
    //                        {
    //                            //serialno++;
    //                            FpSettings.Sheets[0].RowCount++;
    //                            FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
    //                            FpSettings.Sheets[0].Cells[irow, 1].CellType = tt;
    //                            FpSettings.Sheets[0].Cells[irow, 0].CellType = tt;
    //                            FpSettings.Sheets[0].Cells[irow, 0].Text = ds.Tables[0].Rows[irow]["RollNumber"].ToString();
    //                            FpSettings.Sheets[0].Cells[irow, 0].Note = ds.Tables[0].Rows[irow]["app_no"].ToString();
    //                            FpSettings.Sheets[0].Cells[irow, 0].HorizontalAlign = HorizontalAlign.Center;
    //                            FpSettings.Sheets[0].Cells[irow, 1].Text = ds.Tables[0].Rows[irow]["RegistrationNumber"].ToString();
    //                            FpSettings.Sheets[0].Cells[irow, 1].Note = Convert.ToString(ds.Tables[0].Rows[irow]["college_code"]).Trim();
    //                            FpSettings.Sheets[0].Cells[irow, 1].HorizontalAlign = HorizontalAlign.Center;
    //                            FpSettings.Sheets[0].Cells[irow, 2].Text = ds.Tables[0].Rows[irow]["Student_Name"].ToString();
    //                            FpSettings.Sheets[0].Cells[irow, 3].Text = ds.Tables[0].Rows[irow]["StudentType"].ToString();
    //                            FpSettings.Sheets[0].Cells[irow, 4].Text = ds.Tables[0].Rows[irow]["ApplicationNumber"].ToString();
    //                        }
    //                    }
    //                }
    //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //                {
    //                    FpSettings.Visible = true;
    //                    lblrptname.Visible = true;
    //                    txtexcelname.Visible = true;
    //                    btnExcel.Visible = true;
    //                    Buttonexit.Visible = true;
    //                }
    //                else
    //                {
    //                    FpSettings.Visible = false;
    //                    lblrptname.Visible = false;
    //                    txtexcelname.Visible = false;
    //                    btnExcel.Visible = false;
    //                    Buttonexit.Visible = false;
    //                }
    //                while (reader.Read())
    //                {
    //                    FpReport.Sheets[0].RowCount += 1;
    //                    rowcnt = Convert.ToInt32(FpReport.Sheets[0].RowCount) - 1;
    //                    string display = string.Empty;
    //                    string criteria_no = string.Empty;
    //                    string criteria = string.Empty;
    //                    float max_mark = 0;
    //                    float min_mark = 0;
    //                    //string stprd =string.Empty;
    //                    //string endprd =string.Empty;
    //                    FpReport.Sheets[0].Cells[rowcnt, 0].HorizontalAlign = HorizontalAlign.Center;
    //                    FpReport.Sheets[0].Cells[rowcnt, 1].HorizontalAlign = HorizontalAlign.Left;
    //                    FpReport.Sheets[0].Cells[rowcnt, 2].HorizontalAlign = HorizontalAlign.Center;
    //                    FpReport.Sheets[0].Cells[rowcnt, 3].HorizontalAlign = HorizontalAlign.Center;
    //                    FpReport.Sheets[0].Cells[rowcnt, 4].HorizontalAlign = HorizontalAlign.Center;
    //                    FpReport.Sheets[0].Cells[rowcnt, 5].HorizontalAlign = HorizontalAlign.Center;
    //                    FpReport.Sheets[0].Cells[rowcnt, 6].HorizontalAlign = HorizontalAlign.Center;
    //                    FpReport.Sheets[0].Cells[rowcnt, 12].HorizontalAlign = HorizontalAlign.Center;
    //                    FpReport.Sheets[0].Cells[rowcnt, 13].HorizontalAlign = HorizontalAlign.Center;
    //                    //criteria_no = FpReport.Sheets[0].Cells[rowcnt, 1].Tag.ToString();
    //                    criteria_no = reader[1].ToString();
    //                    criteria = reader[0].ToString();
    //                    dsexmtype.Tables[0].DefaultView.RowFilter = " criteria_no='" + criteria_no + "'";
    //                    DataView dvexm = dsexmtype.Tables[0].DefaultView;
    //                    if (dvexm.Count > 0)
    //                    {
    //                        max_mark = Convert.ToSingle(dvexm[0]["max_mark"].ToString());
    //                        min_mark = Convert.ToSingle(dvexm[0]["min_mark"].ToString());
    //                    }
    //                    else
    //                    {
    //                        max_mark = Convert.ToSingle(reader[2].ToString());
    //                        min_mark = Convert.ToSingle(reader[3].ToString());
    //                    }
    //                    FpReport.Sheets[0].Cells[rowcnt, 1].Tag = criteria_no.ToString();
    //                    FpReport.Sheets[0].Cells[rowcnt, 1].Note = bind.ToString();
    //                    FpReport.Sheets[0].Cells[rowcnt, 1].Text = criteria.ToString();
    //                    FpReport.Sheets[0].Cells[rowcnt, 10].Text = max_mark.ToString();
    //                    FpReport.Sheets[0].Cells[rowcnt, 10].Note = max_mark.ToString();
    //                    FpReport.Sheets[0].Cells[rowcnt, 11].Text = min_mark.ToString();
    //                    FpReport.Sheets[0].Cells[rowcnt, 11].Note = min_mark.ToString();
    //                    FpReport.Sheets[0].Cells[rowcnt, 0].Value = 0;
    //                    FpReport.Sheets[0].Cells[rowcnt, 0].Tag = "NE";
    //                    //Server Date===================================================
    //                    //FpReport.Sheets[0].Cells[rowcnt, 5].Text = date1;
    //                    //FpReport.Sheets[0].Cells[rowcnt, 6].Text = month1;
    //                    //FpReport.Sheets[0].Cells[rowcnt, 7].Text = year1;
    //                    if (datelocksetting.Trim() == "1")
    //                    {
    //                        FpReport.Sheets[0].Cells[rowcnt, 5].Locked = true;
    //                        FpReport.Sheets[0].Cells[rowcnt, 6].Locked = true;
    //                        FpReport.Sheets[0].Cells[rowcnt, 7].Locked = true;
    //                    }
    //                    //==============================================================
    //                    //--------------------------------------------------display the testname and retrieve the data from the fpreport
    //                    int temp = Convert.ToInt32(criteria_no);
    //                    d_check = daycheck(temp);
    //                    if (Session["Staff_Code"].ToString().Trim() != "")
    //                    {
    //                        if (d_check == false)
    //                        {
    //                            FpReport.Sheets[0].Rows[rowcnt].Locked = true;
    //                        }
    //                        else
    //                        {
    //                            FpReport.Sheets[0].Rows[rowcnt].Locked = false;
    //                        }
    //                    }
    //                    FpReport.Sheets[0].Columns[10].Locked = true;//added on 31.07.12
    //                    FpReport.Sheets[0].Columns[11].Locked = true;
    //                    //if ((splitvals[2].ToString() != " ") && (splitvals[2].ToString() != "")) //new added 01.02.12
    //                    //{
    //                    //    display = "select * from exam_type where  criteria_no=" + criteria_no + " and subject_no = " + splitvals[0] + " and sections='" + splitvals[2].ToString() + "' and batch_year=" + splitvals[1] + "";
    //                    //}
    //                    //else
    //                    //{
    //                    //    display = "select * from exam_type where  criteria_no=" + criteria_no + " and subject_no = " + splitvals[0] + " and batch_year=" + splitvals[1] + "";
    //                    //}
    //                    //myconn.Close();
    //                    //myconn.Open();
    //                    //SqlCommand cmd1 = new SqlCommand(display, myconn);
    //                    //SqlDataReader drr;
    //                    //drr = cmd1.ExecuteReader();
    //                    //FpSettings.SaveChanges();
    //                    //while (drr.Read())
    //                    //{
    //                    // if (drr.HasRows == true)
    //                    if (dvexm.Count > 0)
    //                    {
    //                        string resexamdate = string.Empty;
    //                        string resentrydate = string.Empty;
    //                        string resmaxmrk = string.Empty;
    //                        string resminmrk = string.Empty;
    //                        string resduration = string.Empty;
    //                        string resnewmaxmrk = string.Empty;
    //                        string resnewminmrk = string.Empty;
    //                        string formatexam = string.Empty;
    //                        string bindnote = string.Empty;
    //                        string srtprd = string.Empty;
    //                        string endprd = string.Empty;
    //                        bind = string.Empty;
    //                        bind = subno + "-" + batch + "-" + sections + "-" + degreecode + "-" + semester;
    //                        formatexam = dvexm[0]["exam_date"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 2].Note = formatexam.ToString();
    //                        if (formatexam != "")
    //                        {
    //                            string[] formatexamsplit = formatexam.Split(new char[] { ' ' });
    //                            string[] formatetime = formatexamsplit[0].Split(new char[] { '/' });
    //                            string examconcat = formatetime[1] + "/" + formatetime[0] + "/" + formatetime[2];
    //                            if (formatetime[1].Length == 1)
    //                            {
    //                                formatetime[1] = "0" + formatetime[1];
    //                            }
    //                            if (formatetime[0].Length == 1)
    //                            {
    //                                formatetime[0] = "0" + formatetime[0];
    //                            }
    //                            FpReport.Sheets[0].Cells[rowcnt, 2].Text = formatetime[1].ToString().Trim().PadLeft(2, '0');
    //                            FpReport.Sheets[0].Cells[rowcnt, 3].Text = formatetime[0].ToString().Trim().PadLeft(2, '0');
    //                            FpReport.Sheets[0].Cells[rowcnt, 4].Text = formatetime[2].ToString();
    //                        }
    //                        else
    //                        {
    //                            string examconcat = string.Empty;
    //                            FpReport.Sheets[0].Cells[rowcnt, 2].Text = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
    //                            FpReport.Sheets[0].Cells[rowcnt, 3].Text = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
    //                            FpReport.Sheets[0].Cells[rowcnt, 4].Text = DateTime.Now.Year.ToString();
    //                        }
    //                        string formatentry = string.Empty;
    //                        formatentry = dvexm[0]["entry_date"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 5].Note = formatentry.ToString();
    //                        if (formatentry != "")
    //                        {
    //                            string[] formatentrysplit = formatentry.Split(new char[] { ' ' });
    //                            string[] formatentrytime = formatentrysplit[0].Split(new char[] { '/' });
    //                            string entryconcat = formatentrytime[1] + "/" + formatentrytime[0] + "/" + formatentrytime[2];
    //                            if (formatentrytime[1].Length == 1)
    //                            {
    //                                formatentrytime[1] = "0" + formatentrytime[1];
    //                            }
    //                            if (formatentrytime[0].Length == 1)
    //                            {
    //                                formatentrytime[0] = "0" + formatentrytime[0];
    //                            }
    //                            //FpReport.Sheets[0].Cells[rowcnt, 5].Text =Convert.ToInt32(formatentrytime[1]).ToString();
    //                            //FpReport.Sheets[0].Cells[rowcnt, 6].Text =Convert.ToInt32(formatentrytime[0]).ToString();
    //                            //FpReport.Sheets[0].Cells[rowcnt, 7].Text = formatentrytime[2].ToString();
    //                            FpReport.Sheets[0].Cells[rowcnt, 5].Text = formatentrytime[1].ToString().Trim().PadLeft(2, '0');
    //                            FpReport.Sheets[0].Cells[rowcnt, 6].Text = formatentrytime[0].ToString().Trim().PadLeft(2, '0');
    //                            FpReport.Sheets[0].Cells[rowcnt, 7].Text = formatentrytime[2].ToString();
    //                            if (datelocksetting == "1")
    //                            {
    //                                FpReport.Sheets[0].Cells[rowcnt, 5].Locked = true;
    //                                FpReport.Sheets[0].Cells[rowcnt, 6].Locked = true;
    //                                FpReport.Sheets[0].Cells[rowcnt, 7].Locked = true;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            string entryconcat = string.Empty;
    //                            FpReport.Sheets[0].Cells[rowcnt, 5].Text = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
    //                            FpReport.Sheets[0].Cells[rowcnt, 6].Text = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
    //                            FpReport.Sheets[0].Cells[rowcnt, 7].Text = DateTime.Now.Year.ToString();
    //                            if (datelocksetting == "1")
    //                            {
    //                                FpReport.Sheets[0].Cells[rowcnt, 5].Locked = true;
    //                                FpReport.Sheets[0].Cells[rowcnt, 6].Locked = true;
    //                                FpReport.Sheets[0].Cells[rowcnt, 7].Locked = true;
    //                            }
    //                        }
    //                        FpReport.Sheets[0].Cells[rowcnt, 10].Note = dvexm[0]["max_mark"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 10].Text = dvexm[0]["max_mark"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 11].Note = dvexm[0]["min_mark"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 11].Text = dvexm[0]["min_mark"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 12].Text = dvexm[0]["start_period"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 12].Note = dvexm[0]["start_period"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 13].Text = dvexm[0]["end_period"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 13].Note = dvexm[0]["end_period"].ToString();
    //                        subno = FpEntry.Sheets[0].Cells[rerowcnt, 5].Tag.ToString();
    //                        string duration = string.Empty;
    //                        string examDurationNew = Convert.ToString(dvexm[0]["durationNew"]).Trim();
    //                        string examDuration = Convert.ToString(dvexm[0]["duration"]).Trim();
    //                        //examDurationNew = Convert.ToString(dvexm[0]["durationNew"]).Trim();
    //                        //examDuration = Convert.ToString(dvexm[0]["duration"]).Trim();
    //                        TimeSpan tsDuration = new TimeSpan(0, 0, 0);
    //                        duration = Convert.ToString(dvexm[0]["duration"]).Trim();
    //                        FpReport.Sheets[0].Cells[rowcnt, 8].Note = duration.ToString();
    //                        if (duration.ToString().Trim() != "")
    //                        {
    //                            string[] splitdur = duration.Split(new char[] { ':' });
    //                            // FpReport.Sheets[0].Cells[rowcnt, 8].Value = splitdur[0].ToString();
    //                            FpReport.Sheets[0].SetText(rowcnt, 8, splitdur[0].Trim().ToString());
    //                            if (splitdur.GetUpperBound(0) == 1)
    //                            {
    //                                if (splitdur[1].ToString() != "")
    //                                {
    //                                    // FpReport.Sheets[0].Cells[rowcnt, 9].Value = splitdur[1].ToString();
    //                                    FpReport.Sheets[0].SetText(rowcnt, 9, splitdur[1].Trim().ToString());
    //                                }
    //                            }
    //                        }
    //                        int hour = 0;
    //                        int min = 0;
    //                        int seconds = 0;
    //                        string[] durationSplit = examDurationNew.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
    //                        if (durationSplit.Length > 0)
    //                        {
    //                            if (durationSplit.Length >= 3)
    //                            {
    //                                int.TryParse(durationSplit[0].Trim(), out hour);
    //                                int.TryParse(durationSplit[1].Trim(), out min);
    //                                int.TryParse(durationSplit[2].Trim(), out seconds);
    //                            }
    //                            else if (durationSplit.Length == 2)
    //                            {
    //                                int tempnew1 = 0;
    //                                int tempnew2 = 0;
    //                                int.TryParse(durationSplit[0].Trim(), out tempnew1);
    //                                int.TryParse(durationSplit[1].Trim(), out tempnew2);
    //                                //int.TryParse(durationSplit[2].Trim(), out seconds);
    //                                if (tempnew1 <= 12 || tempnew1 <= 23)
    //                                {
    //                                    hour = tempnew1;
    //                                }
    //                                else if (tempnew1 < 60)
    //                                {
    //                                    min = tempnew1;
    //                                }
    //                                if (tempnew2 <= 59)
    //                                {
    //                                    min = tempnew2;
    //                                }
    //                            }
    //                            else if (durationSplit.Length == 1)
    //                            {
    //                                int tempnew1 = 0;
    //                                int.TryParse(durationSplit[0].Trim(), out tempnew1);
    //                                //int.TryParse(durationSplit[1].Trim(), out temp2);
    //                                //int.TryParse(durationSplit[2].Trim(), out seconds);
    //                                if (tempnew1 <= 12 || tempnew1 <= 23)
    //                                {
    //                                    hour = tempnew1;
    //                                }
    //                                else if (tempnew1 < 60)
    //                                {
    //                                    min = tempnew1;
    //                                }
    //                            }
    //                        }
    //                        if (hour == 0 && min == 0 && seconds == 0)
    //                        {
    //                            durationSplit = examDuration.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
    //                            if (durationSplit.Length > 0)
    //                            {
    //                                if (durationSplit.Length >= 3)
    //                                {
    //                                    int.TryParse(durationSplit[0].Trim(), out hour);
    //                                    int.TryParse(durationSplit[1].Trim(), out min);
    //                                    int.TryParse(durationSplit[2].Trim(), out seconds);
    //                                }
    //                                else if (durationSplit.Length == 2)
    //                                {
    //                                    int tempnew1 = 0;
    //                                    int tempnew2 = 0;
    //                                    int.TryParse(durationSplit[0].Trim(), out tempnew1);
    //                                    int.TryParse(durationSplit[1].Trim(), out tempnew2);
    //                                    //int.TryParse(durationSplit[2].Trim(), out seconds);
    //                                    if (tempnew1 <= 12 || tempnew1 <= 23)
    //                                    {
    //                                        hour = tempnew1;
    //                                    }
    //                                    else if (tempnew1 < 60)
    //                                    {
    //                                        min = tempnew1;
    //                                    }
    //                                    if (tempnew2 <= 59)
    //                                    {
    //                                        min = tempnew2;
    //                                    }
    //                                }
    //                                else if (durationSplit.Length == 1)
    //                                {
    //                                    int tempnew1 = 0;
    //                                    int.TryParse(durationSplit[0].Trim(), out tempnew1);
    //                                    if (tempnew1 <= 12 || tempnew1 <= 23)
    //                                    {
    //                                        hour = tempnew1;
    //                                    }
    //                                    else if (tempnew1 < 60)
    //                                    {
    //                                        min = tempnew1;
    //                                    }
    //                                }
    //                            }
    //                        }
    //                        tsDuration = new TimeSpan(hour, min, seconds);
    //                        string newduartion = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
    //                        FpReport.Sheets[0].SetText(rowcnt, 8, hour.ToString().Trim().PadLeft(2, '0'));
    //                        FpReport.Sheets[0].SetText(rowcnt, 9, min.ToString().Trim().PadLeft(2, '0'));
    //                        //  FpReport.Sheets[0].Cells[rowcnt, 0].Value = 1; //26.03.12
    //                        resexamdate = dvexm[0]["exam_date"].ToString();
    //                        resentrydate = dvexm[0]["entry_date"].ToString();
    //                        resmaxmrk = dvexm[0]["max_mark"].ToString();
    //                        resminmrk = dvexm[0]["min_mark"].ToString();
    //                        resduration = Convert.ToString(dvexm[0]["duration"]).Trim();
    //                        resnewmaxmrk = dvexm[0]["new_maxmark"].ToString();
    //                        resnewminmrk = dvexm[0]["new_minmark"].ToString();
    //                        string exam_code = string.Empty;
    //                        exam_code = dvexm[0]["exam_code"].ToString();
    //                        srtprd = dvexm[0]["start_period"].ToString();
    //                        endprd = dvexm[0]["end_period"].ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 0].Tag = exam_code.ToString();
    //                        resduration = newduartion;
    //                        bindnote = bind + ";" + resexamdate + "-" + resentrydate + "-" + resduration + "-" + resnewmaxmrk + "-" + resmaxmrk + "-" + resnewminmrk + "-" + resminmrk + "-" + srtprd + "-" + endprd;
    //                        //----------------- set the crtriano and examcode as tag and note... and increment the clmnin fpsettings
    //                        //   FpSettings.Sheets[0].ColumnCount += 1; //26.03.12
    //                        FpSettings.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1].Tag = criteria_no.ToString() + "/" + exam_code;
    //                        FpSettings.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1].Note = bindnote.ToString();
    //                        //  FpSettings.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1].Text = criteria.ToString(); //26.03.12
    //                        if (d_check == false)
    //                        {
    //                            FpSettings.Sheets[0].Columns[FpSettings.Sheets[0].ColumnCount - 1].Locked = true;
    //                        }
    //                        else
    //                        {
    //                            FpSettings.Sheets[0].Columns[FpSettings.Sheets[0].ColumnCount - 1].Locked = false;
    //                        }
    //                        try
    //                        {
    //                            if (Session["Staff_Code"].ToString().Trim() != "")
    //                            {
    //                                string examlock = dvexm[0]["islock"].ToString();
    //                                if (examlock.Trim().ToLower() == "true" || examlock.Trim() == "1")
    //                                {
    //                                    string elockdate = dvexm[0]["elockdate"].ToString();
    //                                    if (elockdate.Trim() != "")
    //                                    {
    //                                        DateTime dte = Convert.ToDateTime(elockdate);
    //                                        DateTime dtnow = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
    //                                        if (dte < dtnow)
    //                                        {
    //                                            FpReport.Sheets[0].Rows[rowcnt].Locked = true;
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                            else
    //                            {
    //                                //string examlock = dvexm[0]["islock"].ToString();
    //                                //if (examlock.Trim().ToLower() == "true" || examlock.Trim() == "1")
    //                                //{
    //                                //    string elockdate = dvexm[0]["elockdate"].ToString();
    //                                //    if (elockdate.Trim() != "")
    //                                //    {
    //                                //        DateTime dte = Convert.ToDateTime(elockdate);
    //                                //        DateTime dtnow = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
    //                                //        if (dte < dtnow)
    //                                //        {
    //                                //            FpReport.Sheets[0].Rows[rowcnt].Locked = true;
    //                                //        }
    //                                //    }
    //                                //}
    //                            }
    //                        }
    //                        catch
    //                        {
    //                        }
    //                        // Save.Text = "Update";
    //                        Save.Visible = true;
    //                        Save.Enabled = true;
    //                        Delete.Visible = true;
    //                        Delete.Enabled = true;
    //                         Exit1.Visible = true;
    //                        //FpSettings.SaveChanges();
    //                        if (FpReport.Sheets[0].Cells[rowcnt, 0].Value == "1")
    //                        {
    //                            //------------------------------------------------ loop for display the details from the result table
    //                            for (int res = 0; res <= Convert.ToInt16(FpSettings.Sheets[0].RowCount) - 1; res++)
    //                            {
    //                                int colco = 0;
    //                                colco = Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1;
    //                                for (int col = 5; col <= colco; col++)
    //                                {
    //                                    FpSettings.Sheets[0].Cells[res, colco].HorizontalAlign = HorizontalAlign.Center;
    //                                    FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
    //                                    intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
    //                                    intgrcel.MaximumValue = Convert.ToInt32(max_mark.ToString());
    //                                    intgrcel.MinimumValue = -16;
    //                                    intgrcel.ErrorMessage = "Enter valid mark";
    //                                    FpSettings.Sheets[0].Cells[res, colco].CellType = intgrcel;
    //                                    rollno = FpSettings.Sheets[0].Cells[res, 0].Text;
    //                                    string resultmark = string.Empty;
    //                                    resultmark = "select * from Result where roll_no='" + rollno + "'and exam_code = '" + exam_code + "'";
    //                                    markcon.Close();
    //                                    markcon.Open();
    //                                    SqlCommand command1 = new SqlCommand(resultmark, markcon);
    //                                    SqlDataReader resreader;
    //                                    resreader = command1.ExecuteReader();
    //                                    while (resreader.Read())
    //                                    {
    //                                        if (resreader.HasRows == true)
    //                                        {
    //                                            FpSettings.Sheets[0].Cells[res, colco].Text = resreader["marks_obtained"].ToString();
    //                                        }
    //                                    }
    //                                    string chkmark = string.Empty;
    //                                    chkmark = FpSettings.Sheets[0].Cells[res, colco].Text;
    //                                    if (Convert.ToString(chkmark) == "-1")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "AAA";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-2")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "EL";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-3")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "EOD";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "0";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-4")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "ML";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-5")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "SOD";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-6")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "NSS";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-16")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "OD";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-7")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "NJ";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-8")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "S";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-9")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "L";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-10")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "NCC";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-11")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "HS";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-12")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "PP";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-13")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "SYOD";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-14")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "COD";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-15")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "OOD";
    //                                    }
    //                                    if (Convert.ToString(chkmark) == "-17")
    //                                    {
    //                                        FpSettings.Sheets[0].Cells[res, colco].Text = "LA";
    //                                    }
    //                                }
    //                            }//for loop end
    //                        }//end condn for value 1
    //                    }
    //                    else
    //                    {
    //                        string examconcat = string.Empty;
    //                        FpReport.Sheets[0].Cells[rowcnt, 2].Text = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
    //                        FpReport.Sheets[0].Cells[rowcnt, 3].Text = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
    //                        FpReport.Sheets[0].Cells[rowcnt, 4].Text = DateTime.Now.Year.ToString();
    //                        FpReport.Sheets[0].Cells[rowcnt, 5].Text = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
    //                        FpReport.Sheets[0].Cells[rowcnt, 6].Text = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
    //                        FpReport.Sheets[0].Cells[rowcnt, 7].Text = DateTime.Now.Year.ToString();
    //                        if (datelocksetting == "1")
    //                        {
    //                            FpReport.Sheets[0].Cells[rowcnt, 5].Locked = true;
    //                            FpReport.Sheets[0].Cells[rowcnt, 6].Locked = true;
    //                            FpReport.Sheets[0].Cells[rowcnt, 7].Locked = true;
    //                        }
    //                    }
    //                    //  }
    //                }
    //                FpReport.SaveChanges();
    //                if (Convert.ToInt32(FpReport.Sheets[0].RowCount) != 0)
    //                {
    //                    Double totalRows1 = 0;
    //                    totalRows1 = Convert.ToInt32(FpReport.Sheets[0].RowCount);
    //                    FpReport.Sheets[0].PageSize = Convert.ToInt32(totalRows1);
    //                    //   FpReport.Height = 50 * Convert.ToInt32(totalRows1);
    //                }
    //                else
    //                {
    //                    lblErrorMsg.Visible = true;
    //                    lblErrorMsg.Text = "No Test Conducted For The Subject ";
    //                    //lblnorec.Visible = true;
    //                    Buttontotal.Visible = false;
    //                    lblrecord.Visible = false;
    //                    DropDownListpage.Visible = false;
    //                    TextBoxother.Visible = false;
    //                    lblpage.Visible = false;
    //                    TextBoxpage.Visible = false;
    //                    pHeaderSettings.Visible = false;
    //                    FpReport.Visible = false;
    //                    pHeaderReport.Visible = false;
    //                    Label2.Visible = false;
    //                    lblnote2.Visible = false;
    //                    lblNote3.Visible = false;
    //                    btnok.Visible = false;
    //                    Exit1.Visible = false;
    //                    chkretest.Visible = false;
    //                    lblselectstaff.Visible = false;
    //                    ddlstaffname.Visible = false;
    //                }
    //                if (Convert.ToInt32(FpSettings.Sheets[0].RowCount) == 0)
    //                {
    //                    lblErrorMsg.Visible = true;
    //                    lblErrorMsg.Text = "No More Students In The Section ";
    //                    //lblnorec.Visible = true;
    //                    Buttontotal.Visible = false;
    //                    lblrecord.Visible = false;
    //                    DropDownListpage.Visible = false;
    //                    TextBoxother.Visible = false;
    //                    lblpage.Visible = false;
    //                    TextBoxpage.Visible = false;
    //                    pHeaderSettings.Visible = false;
    //                    FpReport.Visible = false;
    //                    pHeaderReport.Visible = false;
    //                    Label2.Visible = false;
    //                    lblnote2.Visible = false;
    //                    lblNote3.Visible = false;
    //                    btnok.Visible = false;
    //                    Exit1.Visible = false;
    //                    chkretest.Visible = false;
    //                    lblselectstaff.Visible = false;
    //                    ddlstaffname.Visible = false;
    //                }
    //                else
    //                {
    //                    lblErrorMsg.Visible = false;
    //                    lblErrorMsg.Text = string.Empty;
    //                    //Buttontotal.Visible = true;
    //                    //lblrecord.Visible = true;
    //                    //DropDownListpage.Visible = true;
    //                    //TextBoxother.Visible = false;
    //                    //lblpage.Visible = true;
    //                    //TextBoxpage.Visible = true;
    //                    pHeaderReport.Visible = true;
    //                    FpReport.Visible = true;
    //                    pHeaderSettings.Visible = true;
    //                    FpSettings.Visible = true;
    //                    lblrptname.Visible = true;
    //                    txtexcelname.Visible = true;
    //                    btnExcel.Visible = true;
    //                    Buttonexit.Visible = true;
    //                    Label2.Visible = true;
    //                    lblnote2.Visible = true;
    //                    lblNote3.Visible = true;
    //                    btnok.Visible = true;
    //                    Exit1.Visible = true;
    //                    chkretest.Visible = true;
    //                    //---------------------------------------------- coding for calculate the total pages
    //                    Double totalRows = 0;
    //                    totalRows = Convert.ToInt32(FpSettings.Sheets[0].RowCount);
    //                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSettings.Sheets[0].PageSize);
    //                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //                    DropDownListpage.Items.Clear();
    //                    if (totalRows >= 10)
    //                    {
    //                        FpSettings.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //                        {
    //                            DropDownListpage.Items.Add((k + 10).ToString());
    //                        }
    //                        DropDownListpage.Items.Add("Others");
    //                        DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;
    //                        FpSettings.Height = (25 * Convert.ToInt32(totalRows) + 150);
    //                    }
    //                    else if (totalRows == 0)
    //                    {
    //                        DropDownListpage.Items.Add("0");
    //                        FpSettings.Height = (25 * Convert.ToInt32(totalRows) + 150);
    //                    }
    //                    else
    //                    {
    //                        FpSettings.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //                        DropDownListpage.Items.Add(FpSettings.Sheets[0].PageSize.ToString());
    //                        FpSettings.Height = (25 * Convert.ToInt32(totalRows) + 150);
    //                    }
    //                    FpSettings.Sheets[0].PageSize = Convert.ToInt32(totalRows) + 150;
    //                }
    //            }
    //            else
    //            {
    //                FpReport.Visible = false;
    //                FpSettings.Visible = false;
    //                lblrptname.Visible = false;
    //                txtexcelname.Visible = false;
    //                btnExcel.Visible = false;
    //                Buttonexit.Visible = false;
    //                btnok.Visible = false;
    //                Exit1.Visible = false;
    //                chkretest.Visible = false;
    //                lblselectstaff.Visible = false;
    //                ddlstaffname.Visible = false;
    //                Buttontotal.Visible = false;
    //                lblrecord.Visible = false;
    //                DropDownListpage.Visible = false;
    //                TextBoxother.Visible = false;
    //                lblpage.Visible = false;
    //                TextBoxpage.Visible = false;
    //                lblPageSearch.Visible = false;
    //                lblErrorMsg.Visible = true;
    //                lblErrorMsg.Text = "No Test Conducted For The Subject";
    //                // lblnorec.Visible = true;
    //                // lblnorec.Text = "No Records Found";
    //                pHeaderReport.Visible = false;
    //                pHeaderSettings.Visible = false;
    //                Label2.Visible = false;
    //                lblnote2.Visible = false;
    //                lblNote3.Visible = false;
    //            }
    //        }
    //        Cellclick = false;
    //        Session["Columncount"] = FpSettings.Sheets[0].ColumnCount;
    //        if (FpSettings.Visible == true)
    //        {
    //            int colcount = Convert.ToInt32(FpSettings.Sheets[0].ColumnCount) - 1;
    //            if (colcount > 4)
    //            {
    //                Save.Visible = true;
    //                Delete.Visible = true;
    //                Exit1.Visible = true;
    //            }
    //            else
    //            {
    //                Save.Visible = false;
    //                Delete.Visible = false;
    //                Exit1.Visible = false;
    //            }
    //        }
    //        else
    //        {
    //            Save.Visible = false;
    //            Delete.Visible = false;
    //            Exit1.Visible = false;
    //        }
    //    }
    //    if (Cellclick = false)
    //    {
    //        FpSettings.Visible = false;
    //        lblrptname.Visible = false;
    //        txtexcelname.Visible = false;
    //        btnExcel.Visible = false;
    //        Buttonexit.Visible = false;
    //    }
    //    //}
    //    //catch
    //    //{
    //    //}
    //}

    //------------------------------------------------------- coding for go btn
    protected void btnGo_Click(object sender, EventArgs e)
    {
        //-------------------Sridhar 06 sep 2014          @ Start @ -----------------------------------------//
        // Buttonexit.Visible = false;
        pHeaderEntry.Visible = false;
        lblerror.Visible = false;
        lblRetestMin.Visible = false;
        txt_RetestMin.Visible = false;
        GridView2.Visible = false;
        btn_import.Visible = false;
        // Exit1.Visible = false;
        fpmarkexcel.Visible = false;
        pHeaderReport.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        lblPageSearch.Visible = false;
        btnok.Visible = false;
        // Exit1.Visible = false;
        chkretest.Visible = false;
        lblselectstaff.Visible = false;
        ddlstaffname.Visible = false;
        GridView3.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        //  Buttonexit.Visible = false;
        chkmarkattendance.Visible = false;
        Delete.Visible = false;
        //  Exit1.Visible = false;
        string batchsetting = "", secsetting = string.Empty;
        DataSet dssetting = new DataSet();
        string is_staff_check = string.Empty;
        string Master1 = string.Empty;
        batchsetting = ddlBatch.SelectedItem.Text.ToString();
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            string group = Convert.ToString(Session["group_code"]).Trim();
            Master1 = Convert.ToString(group.Split(';')[0]).Trim();
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
        dssetting.Clear();
        string srisql = string.Empty;
        if (!string.IsNullOrEmpty(Master1.Trim()))
        {
            srisql = "select User_code,is_staff from usermaster where USER_code='" + Master1 + "'";
            dssetting.Clear();
            dssetting = da.select_method_wo_parameter(srisql, "Text");
        }
        if (dssetting.Tables.Count > 0 && dssetting.Tables[0].Rows.Count > 0)
        {
            is_staff_check = Convert.ToString(dssetting.Tables[0].Rows[0][1]).Trim();
        }
        string collegecode = Convert.ToString(Session["collegecode"]).Trim();
        string strbinddegree = "select batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "' and batch_year='" + batchsetting + "' ";
        if (is_staff_check.Trim().ToLower() == "false" || is_staff_check.Trim().ToLower() == "0")
        {
            dssetting.Clear();
            dssetting = da.select_method_wo_parameter(strbinddegree, "Text");
            if (dssetting.Tables.Count > 0 && dssetting.Tables[0].Rows.Count > 0)
            {
                int admincount = 0;
                if (batchsetting != "" || batchsetting != null || batchsetting != "-1" || batchsetting.ToLower().Trim() != "all")
                {
                    for (int n = 0; n < dssetting.Tables[0].Rows.Count; n++)
                    {
                        if (batchsetting == dssetting.Tables[0].Rows[0][n].ToString())
                        {
                            admincount++;
                        }
                    }
                }
                if (admincount <= 0)
                {
                    GridView1.Visible = false;
                    GridView2.Visible = false;
                    GridView3.Visible = false;
                    lblerror.Text = "Please Update The Admin Rights";
                    lblerror.Visible = true;
                    return;
                }
                else
                {
                    string secstring = string.Empty;
                    if (ddlSec.Enabled == true)
                    {
                        secstring = ddlSec.SelectedItem.Text.ToString();
                    }
                    srisql = "select sections from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "' and batch_year='" + batchsetting + "'";
                    dssetting.Clear();
                    dssetting = da.select_method_wo_parameter(srisql, "Text");
                    if (dssetting.Tables.Count > 0 && dssetting.Tables[0].Rows.Count > 0)
                    {
                        int adminseccount = 0;
                        string secstr = dssetting.Tables[0].Rows[0][0].ToString();
                        string[] split1 = secstr.Split(',');
                        for (int n = 0; n <= split1.GetUpperBound(0); n++)
                        {
                            if (secstring == split1[n].ToString())
                            {
                                adminseccount++;
                            }
                        }
                        if (adminseccount == 0)
                        {
                            GridView1.Visible = false;
                            GridView2.Visible = false;
                            GridView3.Visible = false;
                            lblerror.Text = "Please Update The Admin Rights";
                            lblerror.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        GridView1.Visible = false;
                        GridView2.Visible = false;
                        GridView3.Visible = false;
                        lblerror.Text = "Please Update The Admin Rights";
                        lblerror.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                GridView1.Visible = false;
                GridView2.Visible = false;
                GridView3.Visible = false;
                lblerror.Text = "Please Update The Admin Rights";
                lblerror.Visible = true;
                return;
            }
        }
        //-------------------Sridhar 06 sep 2014          @ End @ -----------------------------------------//    
        myconn.Open();

        Load_gobtn();

    }

    public void Load_gobtn()
    {
        try
        {

            chkmarkattendance.Checked = false;
            string staff_code = string.Empty;
            staff_code = Convert.ToString(Session["staff_code"]).Trim();
            if (staff_code == null || staff_code.Trim() == "")
            {

                pnlEntry.Visible = true;
                string strsec = string.Empty;
                myconn.Close();
                myconn.Open();
                string SyllabusYr;
                string SyllabusQry;
                //----------------------------------------------------Query for get the syllbs yr
                if (ddlBranch.SelectedValue.ToString() != "" && ddlSemYr.SelectedValue.ToString() != "" && ddlBatch.SelectedValue.ToString() != "")
                {
                    SyllabusQry = "select syllabus_year from syllabus_master where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester ='" + ddlSemYr.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "'";
                    SyllabusYr = GetFunction(SyllabusQry.ToString());
                    string Sqlstr;
                    Sqlstr = string.Empty;
                    if (ddlSec.Text.ToString().Trim().ToLower() == "all" || ddlSec.Text.ToString().Trim() == "" || ddlSec.Text.ToString().Trim() == "-1")
                    {
                        strsec = string.Empty;
                    }
                    else
                    {
                        strsec = " and sections='" + Convert.ToString(ddlSec.SelectedValue).Trim() + "'";
                    }
                    DataSet dss = new DataSet();
                    //------------------------------------------- Query for display the subject name inthe Spread1-Fpentry
                    if (SyllabusYr != "")
                    {
                        if (Session["Staff_Code"] == null || Convert.ToString(Session["Staff_Code"]).Trim() == "")
                        {
                            Sqlstr = "select distinct subject_name as Subject,subject.subject_no,subject_code,registration.batch_year,registration.current_semester as Semester,isnull(Registration.Sections,'') as section,syllabus_master.degree_code,registration.degree_code as degree from subject,sub_sem,syllabus_master,subjectchooser,registration where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and syllabus_master.semester='" + Convert.ToString(ddlSemYr.SelectedValue).Trim() + "' and syllabus_master.batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and syllabus_year='" + Convert.ToString(SyllabusYr).Trim() + "' and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and registration.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and registration.current_semester='" + Convert.ToString(ddlSemYr.SelectedValue).Trim() + "' and registration.batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and RollNo_Flag<>'0' and cc='0' " + Convert.ToString(strsec) + " and exam_flag <> 'DEBAR'";
                        }
                        else if (Session["Staff_Code"] != null && Convert.ToString(Session["Staff_Code"]).Trim() != "")
                        {
                            Sqlstr = "select distinct subject_name as Subject,subject.subject_no,subject_code,registration.sections as sections,syllabus_master.degree_code,registration.degree_code as degree from subject,sub_sem,staff_selector,usermaster,registration,subjectchooser,syllabus_master where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count ='1' and  subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and  syllabus_master.semester='" + Convert.ToString(ddlSemYr.SelectedValue).Trim() + "' and syllabus_master.batch_year= '" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and syllabus_master.syllabus_year='" + Convert.ToString(SyllabusYr).Trim() + "' and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and  registration.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and registration.current_semester='" + Convert.ToString(ddlSemYr.SelectedValue).Trim() + "' and registration.batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and RollNo_Flag<>'0' and cc='0' and exam_flag <> 'DEBAR' and staff_selector.staff_code=usermaster.staff_code and subject.subject_no=staff_selector.subject_no and usermaster.staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "'" + Convert.ToString(strsec) + "";
                        }
                        dss = da.select_method_wo_parameter(Sqlstr, "text");

                        if (dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
                        {
                            dtsub = dss.Tables[0];
                            if (Sqlstr != "")
                            {
                                SqlCommand command1 = new SqlCommand(Sqlstr, myconn);
                                SqlDataReader resreader;
                                resreader = command1.ExecuteReader();

                                int rowcnt = 0;
                                while (resreader.Read())
                                {
                                    if (resreader.HasRows == true)
                                    {

                                        pHeaderEntry.Visible = true;
                                        rowcnt = rowcnt + 1;
                                        GridView1.DataSource = dtsub;

                                        GridView1.DataBind();
                                    }
                                }
                                myconn.Close();
                            }
                            lblErrorMsg.Text = string.Empty;
                            lblErrorMsg.Visible = false;
                            //added by srinath 15/5/2014
                            string markattendance = da.GetFunction("select value from Master_Settings where settings='cam mark attendance'");
                            if (markattendance.Trim() != "0" && markattendance.Trim() != "" && markattendance != null)
                            {
                                chkmarkattendance.Visible = true;
                            }
                            else
                            {
                                chkmarkattendance.Visible = false;
                            }
                        }
                        else
                        {
                            pHeaderEntry.Visible = false;
                            GridView1.Visible = false;
                            lblErrorMsg.Visible = true;
                            lblErrorMsg.Text = "No Records Found";
                            //lblnorec.Visible = true;
                            pHeaderReport.Visible = false;
                            GridView2.Visible = false;
                            Buttontotal.Visible = false;
                            lblrecord.Visible = false;
                            DropDownListpage.Visible = false;
                            TextBoxother.Visible = false;
                            lblpage.Visible = false;
                            TextBoxpage.Visible = false;
                            lblPageSearch.Visible = false;
                            btnok.Visible = false;
                            // Exit1.Visible = false;
                            chkretest.Visible = false;
                            lblselectstaff.Visible = false;
                            ddlstaffname.Visible = false;
                            //FpSettings.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnExcel.Visible = false;
                            // Buttonexit.Visible = false;
                            chkmarkattendance.Visible = false;//added by srinath 15/5/2014
                        }


                    }
                    else
                    {
                        pHeaderEntry.Visible = false;
                        GridView1.Visible = false;
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "No Records Found";
                        //lblnorec.Visible = true;
                        pHeaderReport.Visible = false;
                        GridView2.Visible = false;
                        Buttontotal.Visible = false;
                        lblrecord.Visible = false;
                        DropDownListpage.Visible = false;
                        TextBoxother.Visible = false;
                        lblpage.Visible = false;
                        TextBoxpage.Visible = false;
                        lblPageSearch.Visible = false;
                        btnok.Visible = false;
                        // Exit1.Visible = false;
                        chkretest.Visible = false;
                        lblselectstaff.Visible = false;
                        ddlstaffname.Visible = false;
                        //FpSettings.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        //  Buttonexit.Visible = false;
                        chkmarkattendance.Visible = false;
                    }

                }
            }
        }
        catch
        {
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
    }

    //------------------------------------------------------------- coding for Delete 
    protected void btnpaneldelete_Click(object sender, EventArgs e)
    {
        try
        {
            DropDownListpage.Focus();
            //FpReport.SaveChanges();
            Boolean isflag = false;
            //  Exit1.Visible = false;
            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
            string exam_code = string.Empty;
            string Criteria_No = string.Empty;
            string sqldel = string.Empty;
            int isval;
            //if (System.Windows.Forms.MessageBox.Show("Do You Want To Delete The Record?", "Confirm delete", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            ////  if(MessageBox.Show ("Do You Want To Save The Receipt? ", YesNo, Information) = Insyes)
            //{
            float[] testmax = new float[1];
            isval = 0;
            int seltest = 0;
            bool isNew = false;
            isflag = false;
            foreach (GridViewRow flagrow in GridView2.Rows)
            // for (int flagrow = 0; flagrow <= Convert.ToInt16(FpReport.Sheets[0].RowCount) - 1; flagrow++)
            {
                System.Web.UI.WebControls.CheckBox ischeck = (System.Web.UI.WebControls.CheckBox)flagrow.FindControl("cbcell_1");
                if (ischeck.Checked == true)
                {
                    seltest++;
                    Array.Resize(ref testmax, seltest);
                    //testmax[seltest-1]
                    Label exmcod = (Label)flagrow.FindControl("lblexamcode");
                    string examCode = exmcod.Text;
                    if (!string.IsNullOrEmpty(examCode) && examCode != "0" && examCode.Trim().ToUpper() != "NE")
                    {
                        DataSet dsNew = da.select_method_wo_parameter("select s.subjectId,s.subSubjectName from subsubjectTestDetails s where s.examCode='" + examCode + "'", "text");// and s.subjectNo='" + lblSubNo.Text + "'
                        if (dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                        {
                            isNew = true;
                        }
                    }
                    string txt = string.Empty;
                    float.TryParse(Convert.ToString(txt), out testmax[seltest - 1]);
                    isflag = true;
                }
            }
            foreach (GridViewRow flagrow in GridView2.Rows)
            {
                //isval = 0;
                System.Web.UI.WebControls.CheckBox ischeck = (System.Web.UI.WebControls.CheckBox)flagrow.FindControl("cbcell_1");
                if (ischeck.Checked == false)
                {
                    isflag = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = ("Please select the test");
                }
                else
                {
                    lblnorec.Visible = false;
                    lblnorec.Text = string.Empty;
                }
            }
            isflag = true;
            if (isflag == true)
            {
                foreach (GridViewRow delrow in GridView2.Rows)
                // for (int delrow = 0; delrow <= Convert.ToInt16(FpReport.Sheets[0].RowCount) - 1; delrow++)
                {
                    // isval = 0;
                    //isval = Convert.ToInt32(FpReport.Sheets[0].GetValue(delrow, 0).ToString());
                    System.Web.UI.WebControls.CheckBox ischeck = (System.Web.UI.WebControls.CheckBox)delrow.FindControl("cbcell_1");
                    if (ischeck.Checked == true)
                    {
                        //if (FpSettings.Sheets[0].Cells[1, 5].Text != "")
                        //{
                        myconn.Close();
                        myconn.Open();
                        Label critno = (Label)delrow.FindControl("lblcriteriano");
                        Criteria_No = critno.Text;
                        Label exmcode = (Label)delrow.FindControl("lblexamcode");
                        exam_code = exmcode.Text;
                        if ((exam_code != "") && (exam_code != "NE"))
                        {
                            sqldel = "delete from result where exam_code=" + exam_code + " ";
                            SqlCommand cmd = new SqlCommand(sqldel, myconn);
                            cmd.ExecuteNonQuery();
                            //sqldel = "delete  from exam_type where exam_code=" + exam_code + " ";
                            //SqlCommand cmd1 = new SqlCommand(sqldel, myconn);
                            //cmd1.ExecuteNonQuery();
                            if (isNew)
                            {
                                qry = "delete swm from subSubjectWiseMarkEntry swm,subsubjectTestDetails std where std.subjectId=swm.subjectId and std.examCode='" + exam_code + "'";
                                da.update_method_wo_parameter(qry, "text");
                            }
                        }
                        //}else
                        if (GridView3.Rows[1].Cells[5].Text == "")
                        {
                            Save.Visible = true;
                            Save.Enabled = true;
                            Delete.Visible = true;
                            Delete.Enabled = true;
                            // Exit1.Visible = true;
                        }
                        pHeaderEntry.Visible = true;
                        pHeaderReport.Visible = true;
                        pHeaderSettings.Visible = true;
                        //Save.Visible = true;
                        //Save.Text = "Save";
                        //Save.Enabled = true;
                        // Delete.Visible = true;
                        // Delete.Enabled = false;
                        //Exit.Visible = true;
                        // Exit.Enabled = true;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
                        btnok_Click(sender, e);
                        // Cellclick = true; 
                    }
                    else if (isval == 0)
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select the Test and Delete";
                        //return;
                        Delete.Visible = true;
                        Delete.Enabled = true;
                        Save.Enabled = true;
                        Save.Visible = true;
                        Label2.Visible = true;
                        lblnote2.Visible = true;
                        lblNote3.Visible = true;
                        lbltab.Visible = true;
                        // Exit1.Visible = true;
                    }
                }
            }
            Cellclick = true;
            //  callentryselect();
            pnldeleterecord.Visible = false;
            //}
            //else
            //{
            //    Delete.Visible = true;
            //    Delete.Enabled = true;
            //    Save.Visible = true;
            //    Save.Enabled = true;
            //    Label2.Visible = true;
            //    lblnote2.Visible = true;
            //    lblNote3.Visible = true;
            //}
        }
        catch
        {
        }
    }

    protected void btnpanelexit_Click(object sender, EventArgs e)
    {
        pnldeleterecord.Visible = false;
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        try
        {
            pnldeleterecord.Visible = true;
            DropDownListpage.Focus();
            //FpReport.SaveChanges();
            Boolean isflag = false;
            //  Exit1.Visible = false;
            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
            string exam_code = string.Empty;
            string Criteria_No = string.Empty;
            string sqldel = string.Empty;
            int isval;
            float[] testmax = new float[1];
            isval = 0;
            int seltest = 0;
            bool isNew = false;
            isflag = false;
            foreach (GridViewRow flagrow in GridView2.Rows)
            // for (int flagrow = 0; flagrow <= Convert.ToInt16(FpReport.Sheets[0].RowCount) - 1; flagrow++)
            {
                System.Web.UI.WebControls.CheckBox ischeck = (System.Web.UI.WebControls.CheckBox)flagrow.FindControl("cbcell_1");
                //isval = Convert.ToInt32(FpReport.Sheets[0].GetValue(flagrow, 0).ToString());
                if (ischeck.Checked == true)
                {
                    seltest++;
                    Array.Resize(ref testmax, seltest);
                    //testmax[seltest-1]
                    Label exmcod = (Label)flagrow.FindControl("lblexamcode");
                    string examCode = exmcod.Text;
                    if (!string.IsNullOrEmpty(examCode) && examCode != "0" && examCode.Trim().ToUpper() != "NE")
                    {
                        DataSet dsNew = da.select_method_wo_parameter("select s.subjectId,s.subSubjectName from subsubjectTestDetails s where s.examCode='" + examCode + "'", "text");// and s.subjectNo='" + lblSubNo.Text + "'
                        if (dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                        {
                            isNew = true;
                        }
                    }
                    string txt = string.Empty;
                    float.TryParse(Convert.ToString(txt), out testmax[seltest - 1]);
                    isflag = true;
                }
            }
            if (System.Windows.Forms.MessageBox.Show("Do You Want To Delete The Record?", "Confirm delete", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //  if(MessageBox.Show ("Do You Want To Save The Receipt? ", YesNo, Information) = Insyes)
            {
                foreach (GridViewRow flagrow in GridView2.Rows)
                {
                    //isval = 0;
                    System.Web.UI.WebControls.CheckBox ischeck = (System.Web.UI.WebControls.CheckBox)flagrow.FindControl("cbcell_1");
                    if (ischeck.Checked == false)
                    {
                        isflag = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = ("Please select the test");
                    }
                    else
                    {
                        lblnorec.Visible = false;
                        lblnorec.Text = string.Empty;
                    }
                }
                isflag = true;
                if (isflag == true)
                {
                    foreach (GridViewRow delrow in GridView2.Rows)
                    // for (int delrow = 0; delrow <= Convert.ToInt16(FpReport.Sheets[0].RowCount) - 1; delrow++)
                    {
                        // isval = 0;
                        //isval = Convert.ToInt32(FpReport.Sheets[0].GetValue(delrow, 0).ToString());
                        System.Web.UI.WebControls.CheckBox ischeck = (System.Web.UI.WebControls.CheckBox)delrow.FindControl("cbcell_1");
                        if (ischeck.Checked == true)
                        {
                            //if (FpSettings.Sheets[0].Cells[1, 5].Text != "")
                            //{
                            myconn.Close();
                            myconn.Open();
                            Label critno = (Label)delrow.FindControl("lblcriteriano");
                            Criteria_No = critno.Text;
                            Label exmcode = (Label)delrow.FindControl("lblexamcode");
                            exam_code = exmcode.Text;
                            if ((exam_code != "") && (exam_code != "NE"))
                            {
                                sqldel = "delete from result where exam_code=" + exam_code + " ";
                                SqlCommand cmd = new SqlCommand(sqldel, myconn);
                                cmd.ExecuteNonQuery();
                                //sqldel = "delete  from exam_type where exam_code=" + exam_code + " ";
                                //SqlCommand cmd1 = new SqlCommand(sqldel, myconn);
                                //cmd1.ExecuteNonQuery();
                                if (isNew)
                                {
                                    qry = "delete swm from subSubjectWiseMarkEntry swm,subsubjectTestDetails std where std.subjectId=swm.subjectId and std.examCode='" + exam_code + "'";
                                    da.update_method_wo_parameter(qry, "text");
                                }
                            }
                            //}else
                            // if (FpSettings.Sheets[0].Cells[1, 5].Text == "")
                            if (GridView3.Rows[1].Cells[5].Text == "")
                            {
                                Save.Visible = true;
                                Save.Enabled = true;
                                Delete.Visible = true;
                                Delete.Enabled = true;
                            }
                            pHeaderEntry.Visible = true;
                            pHeaderReport.Visible = true;
                            pHeaderSettings.Visible = true;
                            //Save.Visible = true;
                            //Save.Text = "Save";
                            //Save.Enabled = true;
                            // Delete.Visible = true;
                            // Delete.Enabled = false;
                            //Exit.Visible = true;
                            // Exit.Enabled = true;


                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
                            btnok_Click(sender, e);
                            // Cellclick = true; 
                        }
                        else if (isval == 0)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Please Select the Test and Delete";
                            //return;
                            Delete.Visible = true;
                            Delete.Enabled = true;
                            Save.Enabled = true;
                            // Exit1.Visible = true;
                            Save.Visible = true;
                            Label2.Visible = true;
                            lblnote2.Visible = true;
                            lblNote3.Visible = true;
                            lbltab.Visible = true;
                        }
                    }
                }
                Cellclick = true;
                //callentryselect();
            }
            else
            {
                Delete.Visible = true;
                Delete.Enabled = true;
                // Exit1.Visible = true;
                Save.Visible = true;
                Save.Enabled = true;
                Label2.Visible = true;
                lblnote2.Visible = true;
                lblNote3.Visible = true;
                lbltab.Visible = true;
            }
        }
        catch
        {
        }
    }

    public int ValidateInt(object _Data, int _DefaultVal, int _MinVal, int _MaxVal)
    {
        int _val = _DefaultVal;
        try
        {
            if (_Data != null)
            {
                _val = int.Parse(_Data.ToString());
                if (_val < _MinVal)
                    _val = _MinVal;
                else if (_val > _MaxVal)
                    _val = _MaxVal;
            }
        }
        catch (Exception _Exception)
        {
            // Error occured while trying to validate
            // set default value if we ran into a error
            _val = _DefaultVal;
            // You can debug for the error here
            Console.WriteLine("Error : " + _Exception.Message);
        }
        return _val;
    }

    protected void Exit1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    public static DataSet Excelconvertdataset(string path)
    {
        DataSet ds3 = new DataSet();
        string StrSheetName = string.Empty;

        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;';";

        //  @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;Persist Security Info=False";
        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        try
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();

            if (excelConnection.State == ConnectionState.Closed)
                excelConnection.Open();

            DataTable dtSheets = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dtSheets != null && dtSheets.Rows.Count > 0)
            {
                StrSheetName = dtSheets.Rows[0].ItemArray[2].ToString();

            }
            if (!string.IsNullOrEmpty(StrSheetName))
            {
                OleDbCommand cmd = new OleDbCommand("Select * from [" + StrSheetName + "]", excelConnection);
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(ds3, "excelData");
                adapter.Dispose();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (excelConnection.State != ConnectionState.Closed)
                excelConnection.Close();
        }
        return ds3;
    }


    protected void btn_importex(object sender, EventArgs e)
    {
        try
        {
            using (Stream stream = this.fpmarkexcel.FileContent as Stream)
            {
                if (fpmarkexcel.HasFile == true)
                {
                    string extension = Path.GetFileName(fpmarkexcel.PostedFile.FileName);
                    if (extension.Trim() != "")
                    {
                        // string moduletype = Convert.ToString(ViewState["moduletype"]);
                        string filetype = string.Empty;
                        if (System.IO.Path.GetExtension(fpmarkexcel.FileName) == ".xls" || System.IO.Path.GetExtension(fpmarkexcel.FileName) == ".xlsx")
                        {
                            import();
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }

    public void import()
    {
        try
        {
            lblRetestMin.Visible = false;
            txt_RetestMin.Visible = false;
            lblErrorMsg.Visible = false;
            Delete.Enabled = false;
            // Exit1.Visible = true;
            Save.Visible = true;
            Save.Enabled = true;
            Label2.Visible = true;
            lblnote2.Visible = true;
            lblNote3.Visible = true;
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
            //fpmarkimport.Visible = false;
            Save.Enabled = true;
            Boolean rollflag = false;
            Boolean stro = false;
            string errorroll = string.Empty;
            DataSet dsimport = new DataSet();
            int getstuco = 0;
            double maxMarks = 0;
            foreach (GridViewRow gr in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr.FindControl("cbcell_1");
                if (chk.Checked == true)
                {
                    Label mxmrk = (Label)gr.FindControl("lblmaxmarks");
                    string maxmrk = mxmrk.Text;
                    maxMarks = Convert.ToDouble(maxmrk);
                }
            }
            if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "1")
            {
                getstuco = 1;
            }
            if (fpmarkexcel.FileName != "" && fpmarkexcel.FileName != null)
            {
                if (fpmarkexcel.FileName.EndsWith(".xls") || fpmarkexcel.FileName.EndsWith(".xlsx"))
                {
                    using (Stream stream = this.fpmarkexcel.FileContent as Stream)
                    {
                        string extension = Path.GetFileName(fpmarkexcel.PostedFile.FileName);
                        string filname = System.IO.Path.GetExtension(fpmarkexcel.FileName);
                        // string path = Server.MapPath("~/Import/abc" + System.IO.Path.GetExtension(fpmarkexcel.FileName));
                        OleDbDataAdapter adapter = new OleDbDataAdapter();
                        string path = Server.MapPath("~/Importfiles/" + extension);
                        string appPath = path.Replace("\\", "/");
                        fpmarkexcel.SaveAs(appPath);

                        //string extension = Path.GetFileName(fpmarkexcel.PostedFile.FileName);
                        dsimport.Clear();
                        dsimport = Excelconvertdataset(path);

                        stream.Position = 0;

                    }
                    if (dsimport.Tables.Count > 0 && dsimport.Tables[0].Rows.Count > 0)
                    {

                        bool entry = false;
                        int sttst = 0;
                        string settest = string.Empty;
                        string tstnam = string.Empty;

                        string sb = lbltxtbxnam.Text;


                        if (sb.Length > 0)
                        {
                            string[] spplt = sb.Split(';');
                            for (int f = 0; f < spplt.Length; f++)
                            {
                                string nm = Convert.ToString(spplt[f]);
                                string[] spt = nm.Split('-');
                                settest = Convert.ToString(spt[1]);
                                tstnam = Convert.ToString(spt[0]);

                                for (int c = 1; c < dsimport.Tables[0].Columns.Count; c++)
                                {
                                    string gettest = Convert.ToString(dsimport.Tables[0].Columns[c].ColumnName.Trim().ToUpper());
                                    string getAllDetails = string.Empty;

                                    if (settest.ToLower() == gettest.ToLower())
                                    {

                                        for (int i = 0; i < dsimport.Tables[0].Rows.Count; i++)
                                        {
                                            entry = false;
                                            string rollno = Convert.ToString(dsimport.Tables[0].Rows[i]["RollNo"]).Trim();
                                            string studname = Convert.ToString(dsimport.Tables[0].Rows[i]["StudentName"]).Trim();
                                            string markval = Convert.ToString(dsimport.Tables[0].Rows[i][gettest]).Trim();
                                            rollflag = false;
                                            if (rollno.Trim() != "")
                                            {

                                                for (int j = 0; j < dtmrk.Rows.Count; j++)
                                                {

                                                    string getrollno = Convert.ToString(dtmrk.Rows[j]["rollno"]).Trim();
                                                    if (getrollno == rollno)
                                                    {
                                                        entry = true;
                                                        rollflag = true;
                                                        string setmark = markval;
                                                        double studMark = 0;
                                                        double.TryParse(markval.Trim(), out studMark);
                                                        if (studMark > maxMarks)
                                                        {
                                                            setmark = markval = "";
                                                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Mark Should Be Less Than Or Equal To Maximum Mark')", true);
                                                            return;

                                                        }
                                                        if (markval == "-1")
                                                        {
                                                            setmark = "AAA";
                                                        }
                                                        if (markval == "-2")
                                                        {
                                                            setmark = "EL";
                                                        }
                                                        if (markval == "-3")
                                                        {
                                                            setmark = "EOD";
                                                        }
                                                        if (markval == "-4")
                                                        {
                                                            setmark = "ML";
                                                        }
                                                        if (markval == "-5")
                                                        {
                                                            setmark = "SOD";
                                                        }
                                                        if (markval == "-6")
                                                        {
                                                            setmark = "NSS";
                                                        }
                                                        if (markval == "-7")
                                                        {
                                                            setmark = "NJ";
                                                        }
                                                        if (markval == "-8")
                                                        {
                                                            setmark = "S";
                                                        }
                                                        if (markval == "-9")
                                                        {
                                                            setmark = "L";
                                                        }
                                                        if (markval == "-10")
                                                        {
                                                            setmark = "NCC";
                                                        }
                                                        if (markval == "-11")
                                                        {
                                                            setmark = "HS";
                                                        }
                                                        if (markval == "-12")
                                                        {
                                                            setmark = "PP";
                                                        }
                                                        if (markval == "-13")
                                                        {
                                                            setmark = "SYOD";
                                                        }
                                                        if (markval == "-14")
                                                        {
                                                            setmark = "COD";
                                                        }
                                                        if (markval == "-15")
                                                        {
                                                            setmark = "OOD";
                                                        }
                                                        if (markval == "-16")
                                                        {
                                                            setmark = "OD";
                                                        }
                                                        if (markval == "-17")
                                                        {
                                                            setmark = "LA";
                                                        }
                                                        dtmrk.Rows[j][tstnam] = setmark;
                                                        //FpSettings.Sheets[0].Cells[j, g].Text = setmark;
                                                        //j = FpSettings.Sheets[0].RowCount;
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

                                        GridView3.DataSource = dtmrk;
                                        GridView3.DataBind();
                                        stro = true;
                                        //  Buttonexit.Visible = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (GridViewRow grrow1 in GridView2.Rows)
                            {
                                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)grrow1.FindControl("cbcell_1");
                                if (chk.Checked == true)
                                {
                                    Label get_tst = grrow1.FindControl("lbltest") as Label;
                                    lbltsthead1.Text = get_tst.Text;
                                    goto lbl2;
                                }
                            }
                        lbl2:
                            for (int c = 1; c < dsimport.Tables[0].Columns.Count; c++)
                            {
                                string gettest = Convert.ToString(dsimport.Tables[0].Columns[c].ColumnName.Trim().ToUpper());
                                string colnam = Convert.ToString(dsimport.Tables[0].Columns[0].ColumnName.Trim().ToUpper());
                                //    settest = testname.ToUpper();
                                settest = lbltsthead1.Text.ToUpper();
                                string getAllDetails = string.Empty;

                                if (settest == gettest)
                                {

                                    for (int i = 0; i < dsimport.Tables[0].Rows.Count; i++)
                                    {
                                        entry = false;
                                        if (colnam == "ROLLNO")
                                        {
                                            colnam = "rollno";
                                        }
                                        else if (colnam == "REGNO")
                                        {
                                            colnam = "regno";
                                        }
                                        string rollno = Convert.ToString(dsimport.Tables[0].Rows[i][colnam]).Trim();
                                        string studname = Convert.ToString(dsimport.Tables[0].Rows[i]["StudentName"]).Trim();
                                        string markval = Convert.ToString(dsimport.Tables[0].Rows[i][gettest]).Trim();
                                        rollflag = false;
                                        if (rollno.Trim() != "")
                                        {
                                            //if(dtmrk.Rows.Count>0)
                                            for (int j = 0; j < dtmrk.Rows.Count; j++)
                                            {
                                                //dtmrk.DefaultView.RowFilter = " and rollno='" + rollno + "'";
                                                string getrollno = Convert.ToString(dtmrk.Rows[j][colnam]).Trim();


                                                if (getrollno == rollno)
                                                {
                                                    entry = true;
                                                    rollflag = true;
                                                    string setmark = markval;
                                                    double studMark = 0;
                                                    double.TryParse(markval.Trim(), out studMark);
                                                    if (studMark > maxMarks)
                                                    {
                                                        setmark = markval = "";
                                                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Mark Should Be Less Than Or Equal To Maximum Mark')", true);
                                                        return;

                                                    }
                                                    if (markval == "-1")
                                                    {
                                                        setmark = "AAA";
                                                    }
                                                    if (markval == "-2")
                                                    {
                                                        setmark = "EL";
                                                    }
                                                    if (markval == "-3")
                                                    {
                                                        setmark = "EOD";
                                                    }
                                                    if (markval == "-4")
                                                    {
                                                        setmark = "ML";
                                                    }
                                                    if (markval == "-5")
                                                    {
                                                        setmark = "SOD";
                                                    }
                                                    if (markval == "-6")
                                                    {
                                                        setmark = "NSS";
                                                    }
                                                    if (markval == "-7")
                                                    {
                                                        setmark = "NJ";
                                                    }
                                                    if (markval == "-8")
                                                    {
                                                        setmark = "S";
                                                    }
                                                    if (markval == "-9")
                                                    {
                                                        setmark = "L";
                                                    }
                                                    if (markval == "-10")
                                                    {
                                                        setmark = "NCC";
                                                    }
                                                    if (markval == "-11")
                                                    {
                                                        setmark = "HS";
                                                    }
                                                    if (markval == "-12")
                                                    {
                                                        setmark = "PP";
                                                    }
                                                    if (markval == "-13")
                                                    {
                                                        setmark = "SYOD";
                                                    }
                                                    if (markval == "-14")
                                                    {
                                                        setmark = "COD";
                                                    }
                                                    if (markval == "-15")
                                                    {
                                                        setmark = "OOD";
                                                    }
                                                    if (markval == "-16")
                                                    {
                                                        setmark = "OD";
                                                    }
                                                    if (markval == "-17")
                                                    {
                                                        setmark = "LA";
                                                    }
                                                    dtmrk.Rows[j]["test"] = setmark;
                                                    //FpSettings.Sheets[0].Cells[j, g].Text = setmark;
                                                    //j = FpSettings.Sheets[0].RowCount;
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

                                    GridView3.DataSource = dtmrk;
                                    GridView3.DataBind();
                                    stro = true;
                                    //Buttonexit.Visible = true;
                                }
                                //}
                            }
                        }
                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        divPopAlertContent.Visible = true;
                        lblAlertMsg.Text = "Error In Converting Excel";
                        return;
                    }


                    if (stro == true)
                    {
                        if (errorroll == "")
                        {
                            divPopAlert.Visible = true;
                            divPopAlertContent.Visible = true;
                            lblAlertMsg.Text = "Imported Successfully";
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Imported Successfully')", true);
                        }
                        else
                        {
                            if (getstuco == 1)
                            {
                                divPopAlert.Visible = true;
                                divPopAlertContent.Visible = true;
                                lblAlertMsg.Text = "Imported Successfully But " + errorroll + " Regno Numbers (s) are  Not Found";
                                // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Imported Successfully But " + errorroll + " Regno Numbers (s) are  Not Found')", true);
                            }
                            else
                            {
                                divPopAlert.Visible = true;
                                divPopAlertContent.Visible = true;
                                lblAlertMsg.Text = "Imported Successfully But " + errorroll + " Roll Numbers (s) are  Not Found";
                                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Imported Successfully But " + errorroll + " Roll Numbers (s) are  Not Found')", true);
                            }
                        }
                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        divPopAlertContent.Visible = true;
                        lblAlertMsg.Text = "Test Not Exists";
                        // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Test Not Exists')", true);
                    }
                }
                else
                {
                    divPopAlert.Visible = true;
                    divPopAlertContent.Visible = true;
                    lblAlertMsg.Text = "Please Import Only xls Format";
                    return;
                }
            }
            else
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Please Select The File and Then Proceed";
            }
            // fpmarkimport.Visible = false;
            Save.Visible = true;
            //FpReport.Height = 400;
        }
        catch (Exception ex)
        {
            //lblErrorMsg.Text = ex.ToString();
            //lblErrorMsg.Visible = true;
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "CAM Entry");
        }
    }

    //private void setLabelText()
    //{
    //    string grouporusercode = string.Empty;
    //    if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
    //    {
    //        grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
    //    }
    //    else if (Session["usercode"] != null)
    //    {
    //        grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
    //    }
    //    List<Label> lbl = new List<Label>();
    //    List<byte> fields = new List<byte>();
    //    lbl.Add(lblcollege);
    //    lbl.Add(lbldeg);
    //    lbl.Add(lblbranch);
    //    //lbl.Add(lblSem1);
    //    //lbl.Add(lblSem);
    //    fields.Add(0);
    //    fields.Add(2);
    //    fields.Add(3);
    //    //fields.Add(4);
    //    //fields.Add(4);
    //    new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    //    SchoolCollege = new Institution(grouporusercode);
    //    schoolOrCollege = SchoolCollege.TypeInstitute;
    //}

    private string getMarkValue(string mark)
    {
        mark = mark.ToUpper();
        string getmark = mark;
        try
        {
            switch (mark)
            {
                case "AAA":
                    getmark = "-1";
                    break;
                case "EOD":
                    getmark = "-3";
                    break;
                case "OOD":
                    getmark = "-15";
                    break;
                case "EL":
                    getmark = "-2";
                    break;
                case "COD":
                    getmark = "-14";
                    break;
                case "ML":
                    getmark = "-4";
                    break;
                case "SOD":
                    getmark = "-5";
                    break;
                case "NSS":
                    getmark = "-6";
                    break;
                //case "H":
                //          getmark ="-7";
                //           break;
                case "NJ":
                    getmark = "-7";
                    break;
                case "S":
                    getmark = "-8";
                    break;
                case "L":
                    getmark = "-9";
                    break;
                case "NCC":
                    getmark = "-10";
                    break;
                case "HS":
                    getmark = "-11";
                    break;
                case "PP":
                    getmark = "-12";
                    break;
                case "SYOD":
                    getmark = "-13";
                    break;
                case "OD":
                    getmark = "-16";
                    break;
                case "LA":
                    getmark = "-17";
                    break;
                //****Modified By Subburaj 20.08.2014******//
                case "RAA":
                    getmark = "-18";
                    break;
                //****************End*****************//
            }
        }
        catch
        {
        }
        return getmark;
    }

    //protected void btnclosespread_OnClick(object sender, EventArgs e)
    //{
    //    divPopSpread.Visible = false;
    //    pHeaderEntry.Visible = true;
    //    pHeaderReport.Visible = true;
    //    pHeaderSettings.Visible = true;

    //}
    protected void Buttonexit_Click(object sender, EventArgs e)
    {
        ////added by Mullai
        //DAccess2 da2 = new DAccess2();
        //string intime = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
        //int a = da2.update_method_wo_parameter("update UserEELog  set Out_Time='" + intime + "',LogOff='1' where entry_code='" + Session["Entry_Code"] + "'", "Text");
        //Session.Abandon();
        //Session.Clear();
        //Session.RemoveAll();
        //System.Web.Security.FormsAuthentication.SignOut();
        //Response.Redirect("~/Default.aspx", false);
    }

    protected void SelectedIndexChanged(object sender, EventArgs e)
    {

        datechk = 0;
        datchk.Clear();
        string exmon = string.Empty;
        string exdat = string.Empty;
        string exyr = string.Empty;
        string entrydat = string.Empty;
        string entrymon = string.Empty;
        string entryyr = string.Empty;
        string testcr = string.Empty;
        string exhrs = string.Empty;
        string exmin = string.Empty;
        string startper = string.Empty;
        string endper = string.Empty;
        float mxmark = 0;
        float mimark = 0;
        string testcrno = string.Empty;
        string subdetails = string.Empty;
        // Exit1.Visible = false;
        GridView2.Visible = true;
        GridView1.Visible = true;
        int datj = 0;


        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndx = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

        string activerow1 = rowIndx.ToString();
        //string activecol1 = colIndx.ToString();

        if (activerow1 != "-1")
        {
            Label lblsubname = (GridView1.Rows[rowIndx].FindControl("lblsubject") as Label);
            string subname = lblsubname.Text;

            Label lblsubno = (GridView1.Rows[rowIndx].FindControl("lblsubjectno") as Label);
            string sub_no = lblsubno.Text;

            string datelock = GetFunction("select value from master_settings where settings='Cam Date Lock' and " + grouporusercode + "");
            if (datelock.Trim() != "")
            {
                datelocksetting = datelock;
            }
            else
            {
                datelocksetting = "0";
            }


            //  Exit1.Visible = false;
            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
            lblErrorMsg.Visible = false;
            lblnorec.Text = string.Empty;
            txt_RetestMin.Text = string.Empty;
            chkretest.Checked = false;
            lblPageSearch.Visible = true;
            Label2.Visible = true;
            lblnote2.Visible = true;
            lblNote3.Visible = true;
            lbltab.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
            //  Buttonexit.Visible = true;
            btnok.Visible = true;
            //  Exit1.Visible = true;
            chkretest.Visible = true;
            lblselectstaff.Visible = true;
            ddlstaffname.Visible = true;
            chkGrp.Checked = false;
            string staff_code = string.Empty;
            staff_code = (string)Session["staff_code"].ToString();
            if (staff_code != "")
            {
                lblselectstaff.Visible = false;
                ddlstaffname.Visible = false;
                Save.Visible = false;
                Delete.Visible = false;
                RequiredFieldValidator1.Visible = false;
                RequiredFieldValidator2.Visible = false;
                RequiredFieldValidator3.Visible = false;
                RequiredFieldValidator4.Visible = false;
                RequiredFieldValidator5.Visible = false;


            }
            else
            {
                RequiredFieldValidator1.Visible = true;
                RequiredFieldValidator2.Visible = true;
                RequiredFieldValidator3.Visible = true;
                RequiredFieldValidator4.Visible = true;
                RequiredFieldValidator5.Visible = true;
                // FpReport.Visible = true;
                GridView2.Visible = true;
                lblselectstaff.Visible = true;
                ddlstaffname.Visible = true;
            }


            Btach_Year_Val = string.Empty;
            Semester_Val = string.Empty;
            Degree_Code_Val = string.Empty;
            if (staff_code != "")
            {
                Label batchyr = (GridView1.Rows[rowIndx].FindControl("lblbatchyear") as Label);
                Btach_Year_Val = batchyr.Text;
                Label sems = (GridView1.Rows[rowIndx].FindControl("lblsem") as Label);
                Semester_Val = sems.Text;
                Label degree_code = (GridView1.Rows[rowIndx].FindControl("lbldegree") as Label);
                Degree_Code_Val = degree_code.Text;

                Session["Btach_Year"] = Btach_Year_Val;
                Session["Semester"] = Semester_Val;
                Session["Degree_Code"] = Degree_Code_Val;
            }

            myconn.Close();
            myconn.Open();
            int rowcnt = 0;
            string sqlStr = string.Empty;
            string subno = string.Empty;
            string batch = string.Empty;
            string sections = string.Empty;
            string strsec = string.Empty;
            int rerowcnt = 0;
            string semester = string.Empty;
            string degreecode = string.Empty;

            Label semest = GridView1.Rows[rowIndx].FindControl("lblsem") as Label;
            semester = semest.Text;
            Label degcod = GridView1.Rows[rowIndx].FindControl("lbldegreecode") as Label;
            degreecode = degcod.Text;
            Label batchyear = GridView1.Rows[rowIndx].FindControl("lblbatchyear") as Label;
            batch = batchyear.Text;
            Label subnum = GridView1.Rows[rowIndx].FindControl("lblsubjectno") as Label;
            subno = subnum.Text;
            Label sec = GridView1.Rows[rowIndx].FindControl("lblsection") as Label;
            sections = sec.Text;
            Label subcode = GridView1.Rows[rowIndx].FindControl("lblsubcode") as Label;
            string sub_code = subcode.Text;
            lblSub.Text = "Test Details - " + sub_code + " - " + subname + " ";
            string valkey = degreecode + "," + semester + "," + batch + "," + subno + "," + sections + "," + sub_code + "," + subname;
            subdetails = valkey;
            //dicsubval++;
            //dicsub.Clear();
            //dicsub.Add("0", valkey);
            // dicsub.Add(degreecode, semester);

            if (staff_code.Trim() == "")
            {
                string sec_value = string.Empty;
                if (ddlSec.Enabled == true)
                {
                    if (sections.Trim().ToLower() != "all" && sections.Trim() != "" || sections.Trim() != "-1" || sections == null)
                    {
                        sec_value = " and Sections='" + sections + "'";
                    }
                }
                string selectquery = "select ss.staff_code,staff_name from staff_selector ss,staffmaster sm where ss.staff_code =sm.staff_code and batch_year =" + batch + " and subject_no ='" + subno + "' " + sec_value + " ";
                DataSet dnew = new DataSet();
                dnew.Clear();
                dnew = da.select_method_wo_parameter(selectquery, "Text"); // added by jairam 2015-07-10
                if (dnew.Tables[0].Rows.Count > 0)
                {
                    ddlstaffname.DataSource = dnew;
                    ddlstaffname.DataTextField = "staff_name";
                    ddlstaffname.DataValueField = "staff_code";
                    ddlstaffname.DataBind();
                    ddlstaffname.Items.Insert(0, "Select");
                }
                else
                {
                    ddlstaffname.Items.Clear();
                }
            }
            //---------------------------------------------------query for select the group code
            string sqlcheck = string.Empty;
            sqlcheck = "select distinct groupcode from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + degreecode.ToString() + " and semester=" + semester.ToString() + "  and batch_year=" + batch.ToString() + " and groupcode<>'' ";

            SqlCommand cmdchk = new SqlCommand(sqlcheck, myconn);
            SqlDataReader grpreader;
            grpreader = cmdchk.ExecuteReader();
            if (grpreader.HasRows == true)
            {
                ddlGrp.Enabled = true;
                ddlGrp.DataValueField = "groupcode";
                ddlGrp.DataSource = grpreader;
                ddlGrp.DataBind();
                ddlGrp.Items.Add("");
                //ddlGrp.Enabled = false;
            }
            if (sections.Trim().ToLower() == "all" || sections.Trim() == "" || sections == "-1" || sections == null)
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + sections + "'";
            }
            string bind = string.Empty;
            bind = subno + "," + batch + "," + sections + "," + degreecode + "," + semester;
            //------------------------------------------- Query for display the Testname,max,min marks,date and duration in the spread2-Fpreport
            if (staff_code != "")
            {
                if (chkGrp.Checked = true && ddlGrp.SelectedValue != "")
                {
                    //sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + " " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " and staff_code= ' " + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "') and groupcode='" + ddlGrp.Text.ToString() + "'";
                    sqlStr = "select distinct criteria,exam_type.criteria_no,exam_type.max_mark,exam_type.min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno + "   " + strsec + " and batch_year=" + batch + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no =' " + subno + " ' and batch_year = " + batch + " and staff_code= '" + Session["Staff_Code"].ToString() + "' " + strsec + ")),'0') as returnVal  from CriteriaForInternal,exam_type  where exam_type.criteria_no=CriteriaForInternal.criteria_no and exam_type.subject_no='" + subno + "' and syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and groupcode='" + ddlGrp.Text.ToString() + "' " + strsec + " order by criteria";

                }
                else
                {
                    //sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + " " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no =' " + subno.ToString() + " 'and batch_year = " + batch.ToString() + " and staff_code= ' " + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";
                    sqlStr = "select distinct criteria,exam_type.criteria_no,exam_type.max_mark,exam_type.min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno + "   " + strsec + " and batch_year=" + batch + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no =' " + subno + " ' and batch_year = " + batch + " and staff_code= '" + Session["Staff_Code"].ToString() + "' " + strsec + ")),'0') as returnVal  from CriteriaForInternal,exam_type  where exam_type.criteria_no=CriteriaForInternal.criteria_no and exam_type.subject_no='" + subno + "' and syll_code=(select syll_code from subject where subject_no='" + subno + "')  and (groupcode is null or groupcode='') " + strsec + " order by criteria";
                }
            }
            else
            {
                if (chkGrp.Checked = true && ddlGrp.SelectedValue != "")
                {
                    sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + " '" + strsec + " and batch_year=" + batch.ToString() + " and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and groupcode='" + ddlGrp.Text.ToString() + "' order by CriteriaForInternal.criteria";
                }
                else
                {
                    sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + "' " + strsec + " and batch_year=" + batch.ToString() + " and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='') order by CriteriaForInternal.criteria";
                }
            }
            string strgetlestval = "select * from exam_type where subject_no='" + subno.ToString() + "' " + strsec.ToString() + "";
            DataSet dsexmtype = da.select_method_wo_parameter(strgetlestval, "text");
            DataSet dsexmty = da.select_method_wo_parameter(sqlStr, "text");
            readcon.Close();
            readcon.Open();
            SqlCommand cmd_read = new SqlCommand(sqlStr, readcon);
            SqlDataReader reader;
            reader = cmd_read.ExecuteReader();
            rowcnt = 0;
            //if (reader.HasRows == true)
            if (dsexmty.Tables.Count > 0 && dsexmty.Tables[0].Rows.Count > 0)
            {
                pHeaderReport.Visible = true;
                pHeaderSettings.Visible = true;

                SqlDataReader serial_dr;
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand("select LinkValue from inssettings where college_code='" + Session["collegecode"].ToString() + "' and linkname='Student Attendance'", con);
                serial_dr = cmd.ExecuteReader();
                while (serial_dr.Read())
                {
                    if (serial_dr["LinkValue"].ToString() == "1")
                    {
                        serialflag = true;
                    }
                    else
                    {
                        serialflag = false;
                    }
                }
                //Added By Srinath 7/2/2013 =====Start
                string strorderby = GetFunction("select value from Master_Settings where settings='order_by'");
                if (strorderby == "")
                {
                    strorderby = string.Empty;
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
                //---------------------------------Query for display the Student details in the Spread3 FpSettings-- and Settings value to be passed in this query-
                //******************************************End*************************************************************
                string[] splitvals = bind.Split(new char[] { ',' });
                string strstaffselecotr = string.Empty;
                Session["StaffSelector"] = "0";
                strstaffselecotr = string.Empty;
                string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
                string[] splitminimumabsentsms = staffbatchyear.Split('-');
                if (splitminimumabsentsms.Length == 2)
                {
                    int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                    if (splitminimumabsentsms[0].ToString() == "1")
                    {
                        if (Convert.ToInt32(splitvals[1].ToString()) >= batchyearsetting)
                        {
                            Session["StaffSelector"] = "1";
                        }
                    }
                }
                if (Session["StaffSelector"].ToString() == "1")
                {
                    if (Session["Staff_Code"] != null)
                    {
                        if (Session["Staff_Code"].ToString().Trim() != "" && Session["Staff_Code"].ToString().Trim() != "0")
                        {
                            strstaffselecotr = " and SubjectChooser.staffcode like '%" + Session["Staff_Code"].ToString() + "%' ";
                        }
                    }
                }
                if (serialflag == false)
                {

                    sqlStr = "Select distinct len(registration.roll_no),registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,registration.App_No as ApplicationNumber,registration.college_code from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + splitvals[3].ToString() + "' and Semester = '" + splitvals[4].ToString() + "' and registration.Batch_Year = '" + splitvals[1].ToString() + "' and Subject_No = '" + splitvals[0].ToString() + "' " + strsec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + splitvals[4].ToString() + "'   " + strstaffselecotr + " " + strorderby + "";//Modified By Srinath 10/4/2014//" + Session["strvar"] + "
                }
                else
                {

                    sqlStr = "Select  serialno,registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,registration.App_No as ApplicationNumber,registration.college_code from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + splitvals[3].ToString() + "' and Semester = '" + splitvals[4].ToString() + "' and registration.Batch_Year = '" + splitvals[1].ToString() + "' and Subject_No = '" + splitvals[0].ToString() + "' " + strsec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + splitvals[4].ToString() + "'  " + strstaffselecotr + "  ORDER BY serialno";//Modified By Srinath 13/3/20123
                    //==========End
                }

                DataSet ds = da.select_method_wo_parameter(sqlStr, "Text");

                int datelockch = 0;
                string exam_code = string.Empty;
                dttst.Columns.Add("test");
                dttst.Columns.Add("subdetails");
                dttst.Columns.Add("examcode");
                dttst.Columns.Add("examdate");
                dttst.Columns.Add("exammonth");
                dttst.Columns.Add("examyear");
                dttst.Columns.Add("entrydate");
                dttst.Columns.Add("entrymonth");
                dttst.Columns.Add("entryyear");
                dttst.Columns.Add("durationhrs");
                dttst.Columns.Add("durationmins");
                dttst.Columns.Add("max_mark");
                dttst.Columns.Add("min_mark");
                dttst.Columns.Add("start_period");
                dttst.Columns.Add("end_period");
                dttst.Columns.Add("criteria_no");
                while (reader.Read())
                {
                    drtest = dttst.NewRow();
                    string display = string.Empty;
                    string criteria_no = string.Empty;
                    string criteria = string.Empty;
                    float max_mark = 0;
                    float min_mark = 0;
                    criteria_no = reader[1].ToString();
                    testcrno = criteria_no;
                    criteria = reader[0].ToString();
                    testcr = criteria;
                    dsexmtype.Tables[0].DefaultView.RowFilter = " criteria_no='" + criteria_no + "'";
                    DataView dvexm = dsexmtype.Tables[0].DefaultView;
                    if (dvexm.Count > 0)
                    {
                        max_mark = Convert.ToSingle(dvexm[0]["max_mark"].ToString());
                        min_mark = Convert.ToSingle(dvexm[0]["min_mark"].ToString());

                    }
                    else
                    {
                        max_mark = Convert.ToSingle(reader[2].ToString());
                        min_mark = Convert.ToSingle(reader[3].ToString());

                    }
                    mxmark = max_mark;
                    mimark = min_mark;
                    exam_code = "NE";

                    //==============================================================
                    //--------------------------------------------------display the testname and retrieve the data from the fpreport
                    int temp = Convert.ToInt32(criteria_no);
                    d_check = daycheck(temp);
                    datelockch++;
                    if (datelocksetting == "1")
                    {
                        datelckval = datelockch;
                    }
                    if (dvexm.Count > 0)
                    {
                        string resexamdate = string.Empty;
                        string resentrydate = string.Empty;
                        string resmaxmrk = string.Empty;
                        string resminmrk = string.Empty;
                        string resduration = string.Empty;
                        string resnewmaxmrk = string.Empty;
                        string resnewminmrk = string.Empty;
                        string formatexam = string.Empty;
                        string bindnote = string.Empty;
                        string srtprd = string.Empty;
                        string endprd = string.Empty;
                        bind = string.Empty;
                        bind = subno + "-" + batch + "-" + sections + "-" + degreecode + "-" + semester;
                        formatexam = dvexm[0]["exam_date"].ToString();
                        if (formatexam != "")
                        {
                            string[] formatexamsplit = formatexam.Split(new char[] { ' ' });
                            string[] formatetime = formatexamsplit[0].Split(new char[] { '/' });
                            string examconcat = formatetime[1] + "/" + formatetime[0] + "/" + formatetime[2];
                            if (formatetime[1].Length == 1)
                            {
                                formatetime[1] = "0" + formatetime[1];
                            }
                            if (formatetime[0].Length == 1)
                            {
                                formatetime[0] = "0" + formatetime[0];
                            }
                            exdat = formatetime[1].ToString().Trim().PadLeft(2, '0');
                            exmon = formatetime[0].ToString().Trim().PadLeft(2, '0');
                            exyr = formatetime[2].ToString();

                        }
                        else
                        {
                            string examconcat = string.Empty;
                            exdat = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                            exmon = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                            exyr = DateTime.Now.Year.ToString();
                        }
                        string formatentry = string.Empty;
                        formatentry = dvexm[0]["entry_date"].ToString();
                        if (formatentry != "")
                        {
                            string[] formatentrysplit = formatentry.Split(new char[] { ' ' });
                            string[] formatentrytime = formatentrysplit[0].Split(new char[] { '/' });
                            string entryconcat = formatentrytime[1] + "/" + formatentrytime[0] + "/" + formatentrytime[2];
                            if (formatentrytime[1].Length == 1)
                            {
                                formatentrytime[1] = "0" + formatentrytime[1];
                            }
                            if (formatentrytime[0].Length == 1)
                            {
                                formatentrytime[0] = "0" + formatentrytime[0];
                            }
                            entrydat = formatentrytime[1].ToString().Trim().PadLeft(2, '0');
                            entrymon = formatentrytime[0].ToString().Trim().PadLeft(2, '0');
                            entryyr = formatentrytime[2].ToString();

                        }
                        else
                        {
                            string entryconcat = string.Empty;
                            entrydat = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                            entrymon = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                            entryyr = DateTime.Now.Year.ToString();

                        }
                        mxmark = Convert.ToSingle(dvexm[0]["max_mark"].ToString());
                        mimark = Convert.ToSingle(dvexm[0]["min_mark"].ToString());
                        startper = dvexm[0]["start_period"].ToString();
                        endper = dvexm[0]["end_period"].ToString();
                        string duration = string.Empty;
                        string examDurationNew = Convert.ToString(dvexm[0]["durationNew"]).Trim();
                        string examDuration = Convert.ToString(dvexm[0]["duration"]).Trim();
                        TimeSpan tsDuration = new TimeSpan(0, 0, 0);
                        duration = Convert.ToString(dvexm[0]["duration"]).Trim();
                        if (duration.ToString().Trim() != "")
                        {
                            string[] splitdur = duration.Split(new char[] { ':' });
                            exhrs = splitdur[0].Trim().ToString();
                            if (splitdur.GetUpperBound(0) == 1)
                            {
                                if (splitdur[1].ToString() != "")
                                {
                                    exmin = splitdur[1].Trim().ToString();
                                }
                            }
                        }
                        int hour = 0;
                        int min = 0;
                        int seconds = 0;
                        string[] durationSplit = examDurationNew.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (durationSplit.Length > 0)
                        {
                            if (durationSplit.Length >= 3)
                            {
                                int.TryParse(durationSplit[0].Trim(), out hour);
                                int.TryParse(durationSplit[1].Trim(), out min);
                                int.TryParse(durationSplit[2].Trim(), out seconds);
                            }
                            else if (durationSplit.Length == 2)
                            {
                                int tempnew1 = 0;
                                int tempnew2 = 0;
                                int.TryParse(durationSplit[0].Trim(), out tempnew1);
                                int.TryParse(durationSplit[1].Trim(), out tempnew2);
                                if (tempnew1 <= 12 || tempnew1 <= 23)
                                {
                                    hour = tempnew1;
                                }
                                else if (tempnew1 < 60)
                                {
                                    min = tempnew1;
                                }
                                if (tempnew2 <= 59)
                                {
                                    min = tempnew2;
                                }
                            }
                            else if (durationSplit.Length == 1)
                            {
                                int tempnew1 = 0;
                                int.TryParse(durationSplit[0].Trim(), out tempnew1);
                                if (tempnew1 <= 12 || tempnew1 <= 23)
                                {
                                    hour = tempnew1;
                                }
                                else if (tempnew1 < 60)
                                {
                                    min = tempnew1;
                                }
                            }
                        }
                        if (hour == 0 && min == 0 && seconds == 0)
                        {
                            durationSplit = examDuration.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (durationSplit.Length > 0)
                            {
                                if (durationSplit.Length >= 3)
                                {
                                    int.TryParse(durationSplit[0].Trim(), out hour);
                                    int.TryParse(durationSplit[1].Trim(), out min);
                                    int.TryParse(durationSplit[2].Trim(), out seconds);
                                }
                                else if (durationSplit.Length == 2)
                                {
                                    int tempnew1 = 0;
                                    int tempnew2 = 0;
                                    int.TryParse(durationSplit[0].Trim(), out tempnew1);
                                    int.TryParse(durationSplit[1].Trim(), out tempnew2);
                                    if (tempnew1 <= 12 || tempnew1 <= 23)
                                    {
                                        hour = tempnew1;
                                    }
                                    else if (tempnew1 < 60)
                                    {
                                        min = tempnew1;
                                    }
                                    if (tempnew2 <= 59)
                                    {
                                        min = tempnew2;
                                    }
                                }
                                else if (durationSplit.Length == 1)
                                {
                                    int tempnew1 = 0;
                                    int.TryParse(durationSplit[0].Trim(), out tempnew1);
                                    if (tempnew1 <= 12 || tempnew1 <= 23)
                                    {
                                        hour = tempnew1;
                                    }
                                    else if (tempnew1 < 60)
                                    {
                                        min = tempnew1;
                                    }
                                }
                            }
                        }
                        tsDuration = new TimeSpan(hour, min, seconds);
                        string newduartion = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
                        exhrs = hour.ToString().Trim().PadLeft(2, '0');
                        exmin = min.ToString().Trim().PadLeft(2, '0');
                        resexamdate = dvexm[0]["exam_date"].ToString();
                        resentrydate = dvexm[0]["entry_date"].ToString();
                        resmaxmrk = dvexm[0]["max_mark"].ToString();
                        resminmrk = dvexm[0]["min_mark"].ToString();
                        resduration = Convert.ToString(dvexm[0]["duration"]).Trim();
                        resnewmaxmrk = dvexm[0]["new_maxmark"].ToString();
                        resnewminmrk = dvexm[0]["new_minmark"].ToString();

                        exam_code = dvexm[0]["exam_code"].ToString();

                        srtprd = dvexm[0]["start_period"].ToString();
                        endprd = dvexm[0]["end_period"].ToString();
                        resduration = newduartion;
                        bindnote = bind + ";" + resexamdate + "-" + resentrydate + "-" + resduration + "-" + resnewmaxmrk + "-" + resmaxmrk + "-" + resnewminmrk + "-" + resminmrk + "-" + srtprd + "-" + endprd;

                        try
                        {
                            if (Session["Staff_Code"].ToString().Trim() != "")
                            {
                                string examlock = dvexm[0]["islock"].ToString();
                                if (examlock.Trim().ToLower() == "true" || examlock.Trim() == "1")
                                {
                                    string elockdate = dvexm[0]["elockdate"].ToString();
                                    if (elockdate.Trim() != "")
                                    {
                                        DateTime dte = Convert.ToDateTime(elockdate);
                                        DateTime dtnow = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                                        if (dte < dtnow)
                                        {
                                            datj++;
                                            lockdatechk = true;
                                            // datechk = datelockch;
                                            datchk.Add(datj, datelockch);
                                        }
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                        catch
                        {
                        }
                        // Save.Text = "Update";
                        Save.Visible = true;
                        Save.Enabled = true;
                        Delete.Visible = true;
                        Delete.Enabled = true;
                        //  Exit1.Visible = true;
                    }
                    else
                    {
                        string examconcat = string.Empty;
                        exdat = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                        exmon = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                        exyr = DateTime.Now.Year.ToString();
                        entrydat = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                        entrymon = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                        entryyr = DateTime.Now.Year.ToString();

                    }

                    // drtest["ischecked"] = Convert.ToBoolean((GridView2.FindControl("chkRow") as CheckBox).Checked) ? 1 : 0;
                    drtest["test"] = testcr;
                    drtest["subdetails"] = subdetails;
                    drtest["examcode"] = exam_code;
                    drtest["examdate"] = exdat;
                    drtest["exammonth"] = exmon;
                    drtest["examyear"] = exyr;
                    drtest["entrydate"] = entrydat;
                    drtest["entrymonth"] = entrymon;
                    drtest["entryyear"] = entryyr;
                    drtest["durationhrs"] = exhrs;
                    drtest["durationmins"] = exmin;
                    drtest["max_mark"] = mxmark.ToString();
                    drtest["min_mark"] = mimark.ToString();
                    drtest["start_period"] = startper;
                    drtest["end_period"] = endper;
                    drtest["criteria_no"] = criteria_no;

                    testcr = "";
                    exdat = "";
                    exmon = "";
                    exyr = "";
                    entrydat = "";
                    entrymon = "";
                    entryyr = "";
                    exhrs = "";
                    exmin = "";
                    mxmark = 0;
                    mimark = 0;
                    startper = "";
                    endper = "";



                    dttst.Rows.Add(drtest);


                }
                GridView2.DataSource = dttst;
                GridView2.DataBind();
                // Exit1.Visible = false;
                // FpReport.SaveChanges();
                if (GridView2.Rows.Count == 0)
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "No Test Conducted For The Subject ";
                    Buttontotal.Visible = false;
                    lblrecord.Visible = false;
                    DropDownListpage.Visible = false;
                    TextBoxother.Visible = false;
                    lblpage.Visible = false;
                    TextBoxpage.Visible = false;
                    pHeaderSettings.Visible = false;
                    GridView2.Visible = false;
                    pHeaderReport.Visible = false;
                    Label2.Visible = false;
                    lblnote2.Visible = false;
                    lblNote3.Visible = false;
                    lbltab.Visible = false;
                    btnok.Visible = false;
                    // Exit1.Visible = false;
                    chkretest.Visible = false;
                    lblselectstaff.Visible = false;
                    ddlstaffname.Visible = false;
                }
                if (ds.Tables.Count < 0)
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "No More Students In The Section ";
                    Buttontotal.Visible = false;
                    lblrecord.Visible = false;
                    DropDownListpage.Visible = false;
                    TextBoxother.Visible = false;
                    lblpage.Visible = false;
                    TextBoxpage.Visible = false;
                    pHeaderSettings.Visible = false;
                    //FpReport.Visible = false;
                    pHeaderReport.Visible = false;
                    Label2.Visible = false;
                    lblnote2.Visible = false;
                    lblNote3.Visible = false;
                    lbltab.Visible = false;
                    btnok.Visible = false;
                    //  Exit1.Visible = false;
                    chkretest.Visible = false;
                    lblselectstaff.Visible = false;
                    ddlstaffname.Visible = false;
                }

            }
            else
            {
                GridView2.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                // Buttonexit.Visible = false;
                btnok.Visible = false;
                //  Exit1.Visible = false;
                chkretest.Visible = false;
                lblselectstaff.Visible = false;
                ddlstaffname.Visible = false;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                lblPageSearch.Visible = false;
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "No Test Conducted For The Subject";
                pHeaderReport.Visible = false;
                pHeaderSettings.Visible = false;
                Label2.Visible = false;
                lblnote2.Visible = false;
                lblNote3.Visible = false;
                lbltab.Visible = false;
            }





        }
    }

    public void studedetails()
    {
        try
        {
            SqlDataReader serial_dr;
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("select LinkValue from inssettings where college_code='" + Session["collegecode"].ToString() + "' and linkname='Student Attendance'", con);
            serial_dr = cmd.ExecuteReader();
            while (serial_dr.Read())
            {
                if (serial_dr["LinkValue"].ToString() == "1")
                {
                    serialflag = true;
                }
                else
                {
                    serialflag = false;
                }
            }
            string strorderby = GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = string.Empty;
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = " ORDER BY registration.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = " ORDER BY registration.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = " ORDER BY Registration.Stud_Name";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = " ORDER BY registration.Roll_No,registration.Reg_No,Registration.Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = " ORDER BY registration.Roll_No,registration.Reg_No";
                }
                else if (strorderby == "1,2")
                {
                    strorderby = " ORDER BY registration.Reg_No,Registration.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = " ORDER BY registration.Roll_No,Registration.Stud_Name";
                }
            }
            string batch1 = string.Empty;
            string semester1 = string.Empty;
            string degreecode1 = string.Empty;
            string subno1 = string.Empty;
            string sections1 = string.Empty;
            string sqlStr = string.Empty;
            foreach (GridViewRow gr in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr.FindControl("cbcell_1");
                if (chk.Checked == true)
                {
                    Label subd = (Label)gr.FindControl("lblsubdet");
                    string dicvals = subd.Text;
                    string[] dicval = dicvals.Split(',');
                    semester1 = Convert.ToString(dicval[1]);
                    degreecode1 = Convert.ToString(dicval[0]);
                    batch1 = Convert.ToString(dicval[2]);
                    subno1 = Convert.ToString(dicval[3]);
                    sections1 = Convert.ToString(dicval[4]);
                    goto label;
                }
            }

        label:

            string subtyp = da.GetFunction("select lab from sub_sem where subType_no in (select subType_no from subject where subject_no='" + subno1 + "')");
            string strsec = string.Empty;
            if (string.IsNullOrEmpty(sections1))
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and registration.sections='" + sections1 + "'";
            }
            //string[] splitvals = bind.Split(new char[] { '-' });
            string strstaffselecotr = string.Empty;
            string strbtatchstaff = string.Empty;
            string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
            string[] splitminimumabsentsms = staffbatchyear.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batch1) >= batchyearsetting)
                    {
                        Session["StaffSelector"] = "1";
                    }
                }
            }

            if (Session["StaffSelector"].ToString() == "1")
            {
                if (Session["Staff_Code"] != null)
                {
                    if (Session["Staff_Code"].ToString().Trim() != "" && Session["Staff_Code"].ToString().Trim() != "0")
                    {
                        strstaffselecotr = " and SubjectChooser.staffcode like '%" + Session["Staff_Code"].ToString() + "%' ";

                        if (subtyp.Trim() == "0" || subtyp.Trim().ToLower() == "false")
                            strbtatchstaff = " and SubjectChooser.staffcode=s.staff_code";
                    }
                }
            }
            hat.Clear();
            string strbatchallocation = string.Empty;
            string batchstaffcd = string.Empty;
            string sfcd = Convert.ToString(Session["Staff_Code"]);
            if (string.IsNullOrEmpty(sfcd))
            {
                sfcd = ddlstaffname.SelectedItem.Value;
            }
            if (subtyp.Trim() == "0" || subtyp.Trim().ToLower() == "false")
            {
                strbatchallocation = " and  (LTRIM(rtrim(ISNULL(s.Stud_batch,''))))=(LTRIM(rtrim(isnull(subjectChooser.Batch,''))))";
                batchstaffcd = " and  s.staff_code='" + sfcd + "'";
            }
            // }
            if (serialflag == false)
            {
                hat.Add("Degree_Code", degreecode1);
                hat.Add("Semester", semester1);
                hat.Add("batch", batch1);
                hat.Add("subject_no", subno1);
                hat.Add("strsec", strsec);
                hat.Add("strstaffselecotr", strstaffselecotr);
                hat.Add("strbatchstaff", strbtatchstaff);
                hat.Add("strbatchallocation", strbatchallocation);
                hat.Add("strstaffcode", batchstaffcd);
                hat.Add("strorderby", strorderby);

                //sqlStr = "Select distinct len(registration.roll_no),registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,registration.App_No as ApplicationNumber,registration.college_code from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + degreecode1.ToString() + "' and Semester = '" + semester1.ToString() + "' and registration.Batch_Year = '" + batch1.ToString() + "' and Subject_No = '" + subno1.ToString() + "' " + strsec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + semester1.ToString() + "'   " + strstaffselecotr + " " + strorderby + "";//Modified By Srinath 10/4/2014//" + Session["strvar"] + "
            }
            else
            {

                strorderby = " order by serialno ";
                hat.Add("Degree_Code", degreecode1);
                hat.Add("Semester", semester1);
                hat.Add("batch", batch1);
                hat.Add("subject_no", subno1);
                hat.Add("strsec", strsec);
                hat.Add("strstaffselecotr", strstaffselecotr);
                hat.Add("strbatchstaff", strbtatchstaff);
                hat.Add("strbatchallocation", strbatchallocation);
                hat.Add("strstaffcode", batchstaffcd);
                hat.Add("strorderby", strorderby);

                //sqlStr = "Select  serialno,registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,registration.App_No as ApplicationNumber,registration.college_code from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + degreecode1.ToString() + "' and Semester = '" + semester1.ToString() + "' and registration.Batch_Year = '" + batch1.ToString() + "' and Subject_No = '" + subno1.ToString() + "'  and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + semester1.ToString() + "'  " + strstaffselecotr + " " + strsec + " ";//Modified By Srinath 13/3/20123
                //==========End
            }
            DataSet ds = da.select_method("student_details", hat, "sp");
            string rollno = string.Empty;
            string regno = string.Empty;
            string appno = string.Empty;
            string studname = string.Empty;
            string collcode = string.Empty;
            dicstd.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                    {
                        drmrk = dtmrk.NewRow();
                        rollno = ds.Tables[0].Rows[irow]["RollNumber"].ToString();
                        regno = ds.Tables[0].Rows[irow]["RegistrationNumber"].ToString();
                        appno = ds.Tables[0].Rows[irow]["app_no"].ToString();
                        studname = ds.Tables[0].Rows[irow]["Student_Name"].ToString();
                        collcode = Convert.ToString(ds.Tables[0].Rows[irow]["college_code"]).Trim();
                        string studdetails = rollno + "," + regno + "," + appno + "," + studname + "," + collcode;
                        dicstd.Add(appno, studdetails);

                        drmrk["rollno"] = rollno;
                        drmrk["regno"] = regno;
                        drmrk["appno"] = appno;
                        drmrk["studname"] = studname;
                        drmrk["collcode"] = collcode;

                        dtmrk.Rows.Add(drmrk);

                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }
    protected void gridview2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            datch1 = 0;
            if (chkmarkattendance.Checked == true && chkmarkattendance.Visible == true)
            {
                GridView2.Columns[14].Visible = true;
                e.Row.Cells[14].Visible = true;
                GridView2.Columns[15].Visible = true;
                e.Row.Cells[15].Visible = true;
            }
            else
            {
                GridView2.Columns[14].Visible = false;
                e.Row.Cells[14].Visible = false;
                GridView2.Columns[15].Visible = false;
                e.Row.Cells[15].Visible = false;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            datch1++;
            DropDownList ddlexmdt = (e.Row.FindControl("ddlexamdate") as DropDownList);
            DropDownList ddlexmmth = (e.Row.FindControl("ddlexammonth") as DropDownList);
            DropDownList ddlexmyr = (e.Row.FindControl("ddlexamyear") as DropDownList);
            DropDownList ddletrydt = (e.Row.FindControl("ddlentrydate") as DropDownList);
            DropDownList ddletrymth = (e.Row.FindControl("ddlentrymonth") as DropDownList);
            DropDownList ddletryyr = (e.Row.FindControl("ddlentryyear") as DropDownList);
            DropDownList ddlhr = (e.Row.FindControl("ddlhrs") as DropDownList);
            DropDownList ddlmin = (e.Row.FindControl("ddlmins") as DropDownList);
            ddlexmdt.Items.Insert(0, " ");
            ddlexmmth.Items.Insert(0, " ");
            ddletrydt.Items.Insert(0, " ");
            ddletrymth.Items.Insert(0, " ");
            ddlhr.Items.Insert(0, " ");
            ddlmin.Items.Insert(0, " ");

            for (int i = 1; i < 13; i++)
            {
                string item = i.ToString();
                if (item.Length < 2)
                {
                    item = "0" + item;
                }
                ddlexmmth.Items.Add(item);
                ddletrymth.Items.Add(item);
            }
            for (int i1 = 0; i1 < 25; i1++)
            {
                string item = i1.ToString();
                if (item.Length < 2)
                {
                    item = "0" + item;
                }
                ddlhr.Items.Add(item);
            }
            for (int i2 = 0; i2 <= 12; i2++)
            {
                int cal = i2 * 5;
                string item = cal.ToString();
                if (item.Length < 2)
                {
                    item = "0" + item;
                }
                ddlmin.Items.Add(item);
            }
            for (int i3 = 0; i3 <= 31; i3++)
            {
                string item = i3.ToString();
                if (item.Length < 2)
                {
                    item = "0" + item;
                }
                ddlexmdt.Items.Add(item);
                ddletrydt.Items.Add(item);
            }
            string batch = "select distinct batch_year as batch_year  from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year";
            DataSet dbat = da.select_method_wo_parameter(batch, "text");
            string dty = DateTime.Now.ToString("yyyy");
            bool dtflag = true;
            for (int o = 0; o < dbat.Tables[0].Rows.Count; o++)
            {
                string yar = Convert.ToString(dbat.Tables[0].Rows[o]["batch_year"]);
                if (yar.Contains(dty))
                    dtflag = false;
                //else
                //    dtflag = true;


            }

            if (dbat.Tables.Count > 0 && dbat.Tables[0].Rows.Count > 0)
            {
                ddlexmyr.DataSource = dbat;
                ddlexmyr.DataValueField = "batch_year";
                ddlexmyr.DataTextField = "batch_year";
                ddlexmyr.DataBind();
                ddlexmyr.Items.Insert(0, " ");
                ddlexmyr.Items.Insert(1, "1900");



                ddletryyr.DataSource = dbat;
                ddletryyr.DataValueField = "batch_year";
                ddletryyr.DataTextField = "batch_year";
                ddletryyr.DataBind();
                ddletryyr.Items.Insert(0, " ");
                ddletryyr.Items.Insert(1, "1900");

                if (dtflag == true)
                {
                    ddletryyr.Items.Insert(2, dty);
                    ddlexmyr.Items.Insert(2, dty);
                }
            }

            Label exdt = e.Row.FindControl("lblexamdate") as Label;
            string exdtt = exdt.Text;
            if (string.IsNullOrEmpty(exdtt))
                ddlexmdt.Items[0].Selected = true;
            else
                ddlexmdt.Items.FindByText(exdtt).Selected = true;

            Label exmon = e.Row.FindControl("lblexammonth") as Label;
            string exmonn = exmon.Text;
            if (string.IsNullOrEmpty(exmonn))
                ddlexmmth.Items[0].Selected = true;
            else
                ddlexmmth.Items.FindByText(exmonn).Selected = true;

            Label exyr = e.Row.FindControl("lblexamyear") as Label;
            string exyrr = exyr.Text;
            if (string.IsNullOrEmpty(exyrr))
                ddlexmyr.Items[0].Selected = true;
            else
                ddlexmyr.Items.FindByText(exyrr).Selected = true;

            Label etdt = e.Row.FindControl("lblentrydate") as Label;
            string etdtt = etdt.Text;
            if (string.IsNullOrEmpty(etdtt))
                ddletrydt.Items[0].Selected = true;
            else
                ddletrydt.Items.FindByText(etdtt).Selected = true;

            Label etmth = e.Row.FindControl("lblentrymonth") as Label;
            string etmthh = etmth.Text;
            if (string.IsNullOrEmpty(etmthh))
                ddletrymth.Items[0].Selected = true;
            else
                ddletrymth.Items.FindByText(etmthh).Selected = true;

            Label etyr = e.Row.FindControl("lblentryyear") as Label;
            string etyrr = etyr.Text;
            if (string.IsNullOrEmpty(etyrr))
                ddletryyr.Items[0].Selected = true;
            else
                ddletryyr.Items.FindByText(etyrr).Selected = true;

            Label duhr = e.Row.FindControl("lblhrs") as Label;
            string duhrs = duhr.Text;
            if (string.IsNullOrEmpty(duhrs))
                ddlhr.Items[0].Selected = true;
            else
                ddlhr.Items.FindByText(duhrs).Selected = true;


            Label dumin = e.Row.FindControl("lblmins") as Label;
            string dumins = dumin.Text;
            if (string.IsNullOrEmpty(dumins))
                ddlmin.Items[0].Selected = true;
            else
                ddlmin.Items.FindByText(dumins).Selected = true;
            if (datch1 == datelckval)
            {
                if (datelocksetting == "1")
                {
                    e.Row.Cells[7].Enabled = false;
                    e.Row.Cells[8].Enabled = false;
                    e.Row.Cells[9].Enabled = false;
                }
            }

            // int rw = Convert.ToInt32(e.Row);
            foreach (KeyValuePair<int, int> dc in datchk)
            {
                datechk = dc.Value;
                if (datch1 == datechk)
                {
                    if (lockdatechk == true)
                    {
                        e.Row.Enabled = false;
                        break;
                        //GridView2.Rows[datechk-1].Enabled = false;
                        //goto stag;
                    }
                }
            }


        }
        //stag:
    }
    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (forschoolsetting == true)
            {
                e.Row.Cells[1].Text = "Year";
                e.Row.Cells[3].Text = "Term";

            }
            else
            {
                e.Row.Cells[1].Text = "Batch Year";
                e.Row.Cells[3].Text = "Semester";

            }
            string staff_code = Convert.ToString(Session["staff_code"]).Trim();
            if (staff_code == "")
            {
                e.Row.Cells[2].Visible = false;
                GridView1.Columns[2].Visible = false;
            }
            else
            {
                e.Row.Cells[2].Visible = true;
                GridView1.Columns[2].Visible = true;
            }
        }
    }

    protected void ddlexamdate_OnSelectedIndexedChanged(object sender, EventArgs e)
    {




    }
    protected void ddlexammonth_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlexamyear_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlentrydate_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlentrymonth_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlentryyear_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlhrs_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }
    protected void ddlmins_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        try
        {
            lbltxtbxnam.Text = string.Empty;
            lblsubcout.Text = string.Empty;
            lblsubexl.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            dicschsub.Clear();
            dicschsub1.Clear();
            dicschsub2.Clear();
            string date = string.Empty;
            string month = string.Empty;
            string year = string.Empty;
            string date1 = string.Empty;
            string month1 = string.Empty;
            string year1 = string.Empty;
            string exam_code = string.Empty;
            string examdate = string.Empty;
            string entrydate = string.Empty;
            string hours = string.Empty;
            string minutes = string.Empty;
            string startperiod = string.Empty;
            string endperiod = string.Empty;
            string gettest = string.Empty;
            string gttst2 = string.Empty;
            string semester1 = string.Empty;
            string degreecode1 = string.Empty;
            string batch1 = string.Empty;
            string sucd = string.Empty;
            string subname = string.Empty;
            string subno1 = string.Empty;
            string sections1 = string.Empty;
            string testnote = string.Empty;
            string mimrk1 = string.Empty;
            string mxmrk1 = string.Empty;
            // Buttonexit.Visible = true;
            GridView2.Visible = true;
            lblnorec.Visible = false;
            string exmcd1 = string.Empty;
            fpmarkexcel.Visible = true;
            btn_import.Visible = true;
            // Buttonexit.Visible = true;
            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
            Label2.Visible = true;
            lblnote2.Visible = true;
            lblNote3.Visible = true;
            lbltab.Visible = true;
            Boolean isflag = false;
            Save.Visible = true;
            Save.Text = "Save";
            Save.Enabled = true;
            Delete.Visible = true;
            Delete.Enabled = false;
            // Exit1.Visible = true;
            lblErrorMsg.Text = string.Empty;
            // Exit1.Visible = false;
            bool cbDaywisePeriodAttSchedule = false;
            string exmon = string.Empty;


            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
            }
            else
            {
                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            DataSet dsSettings = new DataSet();
            dsSettings = da.select_method_wo_parameter("select * from Master_Settings where settings='RetestMarkEntryBasedOnOptionalMinMarks' and " + grouporusercode + "", "Text");
            if (dsSettings.Tables[0].Rows.Count > 0 && chkretest.Checked == true)
            {
                if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]).Trim() == "0")
                {
                    cbDaywisePeriodAttSchedule = false;
                }
                else if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]).Trim() == "1")
                {
                    cbDaywisePeriodAttSchedule = true;
                }
            }
            if (cbDaywisePeriodAttSchedule == true)
            {
                if (txt_RetestMin.Text.Trim() == "" || txt_RetestMin.Text == null)
                {
                    lblnorec.Text = "Please Enter the Retest Minimum Marks";
                    lblnorec.Visible = true;
                    return;
                }
            }

            bool checkstaff = false; // added by jairam 2015-07-10
            string staff_code = string.Empty;
            staff_code = Convert.ToString(Session["staff_code"]).Trim();
            if (staff_code == null || staff_code.Trim() == "")
            {
                if (ddlstaffname.Items.Count > 0)
                {
                    if (Convert.ToString(ddlstaffname.SelectedItem.Text).Trim().ToLower() != "select" && Convert.ToString(ddlstaffname.SelectedItem.Text).Trim().ToLower() != "")
                    {
                        checkstaff = true;
                    }
                    else
                    {
                        checkstaff = false;
                    }
                }
            }
            else
            {
                checkstaff = true;
            }
            if (checkstaff == true)
            {
                string duration = string.Empty;
                float new_maxmark = 0;
                float new_minmark = 0;
                float maxmark = 0;
                float minmark = 0;
                float[] testmax = new float[1];
                int isval = 0;
                int seltest = 0;
                bool isNew = false;
                int check = 0;
                foreach (GridViewRow grdrow in GridView2.Rows)
                {
                    bool enter = false;
                    int row2 = Convert.ToInt32(grdrow.RowIndex);
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)grdrow.FindControl("cbcell_1");
                    if (chk.Checked == true)
                    {
                        check++;
                        if (check == 1)
                        {
                            seltest++;
                            Array.Resize(ref testmax, seltest);
                            Label critno = grdrow.FindControl("lblcriteriano") as Label;
                            string critno1 = critno.Text;
                            // criteria_no = criteria_noup;
                            Label exm_code = GridView2.Rows[row2].FindControl("lblexamcode") as Label;
                            exam_code = exm_code.Text;

                            if (!string.IsNullOrEmpty(exam_code) && exam_code != "0" && exam_code.Trim().ToUpper() != "NE")
                            {
                                DataSet dsNew = da.select_method_wo_parameter("select s.subjectId,s.subSubjectName from subsubjectTestDetails s where s.examCode='" + exam_code + "'", "text");// and s.subjectNo='" + lblSubNo.Text + "'
                                if (dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                                {
                                    isNew = true;
                                }
                            }
                            Label max_mark = GridView2.Rows[row2].FindControl("lblmaxmarks") as Label;
                            string testmaxmrk = max_mark.Text;
                            float.TryParse(Convert.ToString(testmaxmrk), out testmax[seltest - 1]);
                            isflag = true;
                            Label lblexdate = (grdrow.FindControl("lblexamdate") as Label);
                            date = lblexdate.Text;
                            Label lblexmmonth = (grdrow.FindControl("lblexammonth") as Label);
                            month = lblexmmonth.Text;
                            Label lblexmyear = (grdrow.FindControl("lblexamyear") as Label);
                            year = lblexmyear.Text;
                            string ccdate = month + "/" + date + "/" + year;
                            int period1 = 0;
                            int period2 = 0;

                            Label lblstartper = (grdrow.FindControl("lblstartperiod") as Label);
                            startperiod = lblstartper.Text;
                            Label lblendper = (grdrow.FindControl("lblendperiod") as Label);
                            endperiod = lblendper.Text;
                            int.TryParse(startperiod.Trim(), out period1);
                            int.TryParse(endperiod.Trim(), out period2);

                            Label dat1 = grdrow.FindControl("lblentrydate") as Label;
                            date1 = dat1.Text.Trim().PadLeft(2, '0');
                            Label mont1 = grdrow.FindControl("lblentrymonth") as Label;
                            month1 = mont1.Text.Trim().PadLeft(2, '0');
                            Label yr1 = grdrow.FindControl("lblentryyear") as Label;
                            year1 = yr1.Text;
                            entrydate = month1 + "/" + date1 + "/" + year1;
                            Label hrs1 = grdrow.FindControl("lblhrs") as Label;
                            hours = hrs1.Text.Trim().PadLeft(2, '0');
                            Label mins1 = grdrow.FindControl("lblmins") as Label;
                            minutes = mins1.Text.Trim().PadLeft(2, '0');
                            Label mx_mrk = grdrow.FindControl("lblmaxmarks") as Label;
                            mxmrk1 = mx_mrk.Text;
                            new_maxmark = Convert.ToInt32(mxmrk1);
                            Label mi_mrk = grdrow.FindControl("lblminmarks") as Label;
                            mimrk1 = mi_mrk.Text;
                            //  lblmamrk1.Text = mxmrk1;
                            txttestbox.Text = mxmrk1;
                            lblmin1.Text = mimrk1;
                            textboxmin.Text = mimrk1;
                            Label get_tst = grdrow.FindControl("lbltest") as Label;
                            gettest = get_tst.Text;
                            gttst2 = gettest;

                            Label subd = (Label)grdrow.FindControl("lblsubdet");
                            string dicvals = subd.Text;
                            string[] dicval = dicvals.Split(',');
                            semester1 = Convert.ToString(dicval[1]);
                            degreecode1 = Convert.ToString(dicval[0]);
                            batch1 = Convert.ToString(dicval[2]);
                            subno1 = Convert.ToString(dicval[3]);
                            sections1 = Convert.ToString(dicval[4]);
                            sucd = Convert.ToString(dicval[5]);
                            subname = Convert.ToString(dicval[6]);

                            lblsubtstdet.Text = "TEST DETAILS - " + sucd + " - " + subname.ToUpper();

                            string strsec2 = string.Empty;
                            if (!string.IsNullOrEmpty(sections1))
                            {
                                strsec2 = " and sections='" + sections1 + "'";
                            }

                            exmcd1 = exam_code;
                            if (exam_code == "NE")
                            {
                                string exmcd = da.GetFunction("select exam_code from exam_type where  criteria_no='" + critno1 + "' and subject_no = '" + subno1 + "'  " + strsec2 + " and batch_year='" + batch1 + "'");
                                exmcd1 = exmcd;
                                if (!string.IsNullOrEmpty(exmcd) && exmcd != "0")
                                {
                                    exam_code = exmcd;
                                }

                            }

                            if (ccdate != "//")
                            {
                                if (period1 <= period2)
                                {
                                    if (GridView2.Columns[14].Visible == true && GridView2.Columns[15].Visible == true)
                                    {
                                        lblnorec.Visible = true;
                                        lblnorec.Text = "Kindly Select Different Period";
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Select Single Test!";
                            divPopSpread.Visible = false;
                            GridView1.Visible = true;
                            GridView2.Visible = true;
                            GridView3.Visible = false;
                            pHeaderEntry.Visible = true;
                            pHeaderReport.Visible = true;
                            pHeaderSettings.Visible = true;
                            return;
                        }
                    }
                }

                dtmrk.Clear();
                dtmrk.Dispose();
                dtmrk.Columns.Clear();
                dtmrk.Columns.Add("rollno");
                dtmrk.Columns.Add("regno");
                dtmrk.Columns.Add("appno");
                dtmrk.Columns.Add("maxmrk");
                dtmrk.Columns.Add("minmrk");
                dtmrk.Columns.Add("subId");
                dtmrk.Columns.Add("studname");
                dtmrk.Columns.Add("collcode");
                dtmrk.Columns.Add("section");
                dtmrk.Columns.Add("degree");
                dtmrk.Columns.Add("semester");
                dtmrk.Columns.Add("batch");
                dtmrk.Columns.Add("subno");
                dtmrk.Columns.Add("examcode");
                dtmrk.Columns.Add("test");
                dtmrk.Columns.Add("retest");
                dtmrk.Columns.Add("test1");
                dtmrk.Columns.Add("test2");
                dtmrk.Columns.Add("test3");
                dtmrk.Columns.Add("test4");
                dtmrk.Columns.Add("test5");
                dtmrk.Columns.Add("test6");
                dtmrk.Columns.Add("test7");
                dtmrk.Columns.Add("test8");
                dtmrk.Columns.Add("test9");
                studedetails();

                if (dtmrk.Rows.Count == 0)
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "No More Students In The Section";
                    GridView3.Visible = false;
                    return;
                }
                else
                {
                    dtmrk.Rows[0]["section"] = sections1;
                    dtmrk.Rows[0]["degree"] = degreecode1;
                    dtmrk.Rows[0]["semester"] = semester1;
                    dtmrk.Rows[0]["batch"] = batch1;
                    dtmrk.Rows[0]["subno"] = subno1;
                    dtmrk.Rows[0]["examcode"] = exmcd1;
                }

                if (!isNew)
                {
                    if (cbDaywisePeriodAttSchedule == true)
                    {
                        bool checkretest = true;
                        bool chk = true;
                        if (txt_RetestMin.Text != "" && txt_RetestMin.Text != null)
                        {
                            float retest = 0;
                            float.TryParse(Convert.ToString(txt_RetestMin.Text), out retest);
                            for (int test = 0; test < seltest; test++)
                            {
                                if (testmax[test] >= retest)
                                {

                                }
                                else
                                {
                                    checkretest = false;
                                }
                                if (testmax[test] < retest)
                                {

                                    chk = false;
                                }
                            }
                        }
                        if (checkretest == false)
                        {
                            //the retest option only applicable when all selected test have same max mark
                            float retest = 0;
                            float.TryParse(Convert.ToString(txt_RetestMin.Text), out retest);
                            lblnorec.Text = "Re-Test Option Only Applicable When All Selected Test Have Same Maximum Mark!!!";
                            if (chk == true)
                            {
                                lblnorec.Text = "Re-Test Option Only Applicable When All Selected Test Have Same Maximum Mark!!!";
                            }
                            else
                            {
                                lblnorec.Text = "Please Check Retest Minimum Mark!!!";
                            }
                            lblnorec.Visible = true;
                            return;
                        }
                    }

                    string GetCellTag = string.Empty;
                    string excat = string.Empty;
                    string encat = string.Empty;
                    string selecttest = string.Empty;
                    string settingtest = string.Empty;

                    string dt = DateTime.Today.ToShortDateString();
                    string[] dsplit = dt.Split(new Char[] { '/' });
                    string dcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                    int splityr = 0;
                    splityr = Convert.ToInt32(dsplit[2].ToString());



                    ArrayList check_sameperiod = new ArrayList();
                    //---------------------------------- chk the condition for the flag value-chkbox

                    if (isflag == true)
                    {
                        //---------------------------------loop for get the values from fpreport

                        lblnorec.Visible = false;
                        lblnorec.Text = string.Empty;
                        dicmaxmrk.Clear();
                        int tstct = 1;


                        lblnorec.Text = string.Empty;

                        GetCellNote = string.Empty;
                        GetCellTag = string.Empty;

                        //------------------------------------------------------to increment the clmn in fpsettings

                        if ((date != "") && (month != "") && (year != "") && (date != null) && (month != null) && (year != null))
                        {
                            examdate = month + "/" + date + "/" + year;
                        }
                        else
                        {
                            examdate = string.Empty;// DateTime.Now.ToString("MM/dd/yyyy");
                        }

                        if ((date1 != "") && (month1 != "") && (year1 != "") && (date1 != null) && (month1 != null) && (year1 != null))
                        {
                            entrydate = month1 + "/" + date1 + "/" + year1;
                        }
                        else
                        {
                            entrydate = DateTime.Now.ToString("MM/dd/yyyy");
                        }
                        if ((hours == "") && (minutes == ""))
                        {
                            duration = "00:00:00";
                        }

                        if ((minutes != null) && (hours != null) && (hours != "") && (minutes != ""))
                        {
                            duration = hours.Trim().PadLeft(2, '0') + ":" + minutes.Trim().PadLeft(2, '0') + ":00";
                        }
                        else
                        {
                            duration = "00:00:00";
                        }


                        new_minmark = Convert.ToInt32(mimrk1);

                        Str_ExamType = examdate + "-" + entrydate + "-" + duration + "-" + new_maxmark + "-" + maxmark + "-" + new_minmark + "-" + minmark + "-" + startperiod + "-" + endperiod;
                        string check_date_and_period = examdate + "-" + startperiod + "-" + endperiod;
                        dicmaxmrk.Add(tstct, Str_ExamType);
                        check_sameperiod.Add(check_date_and_period);
                        //---------------------------------------to validate the mark

                        testnote = subno1 + "," + batch1 + "," + sections1 + "," + degreecode1 + "," + semester1;


                        GetCellTag = gettest;
                        GetCellNote = testnote + ";" + Str_ExamType;
                        diccellnote.Clear();
                        diccellnote.Add(1, GetCellNote);
                        string retestheader = gettest.ToString() + "/Re-Test";
                        string mrkatnd = string.Empty;
                        string retstmrk1 = string.Empty;

                        if (exam_code == "NE")// Add by jairam *************** 17-10-2014*******************
                        {
                            if ((startperiod != "") && (startperiod != null) && (endperiod != "") && (endperiod != null))
                            {
                                if ((examdate != "") && (examdate != null))
                                {
                                    DataSet ds11 = new DataSet();
                                    ArrayList arradd_check = new ArrayList();
                                    Hashtable hatattnd = new Hashtable();
                                    string strperiod = "select LeaveCode ,DispText  from AttMasterSetting where CollegeCode='" + Convert.ToString(Session["collegecode"]).Trim() + "' and DispText in('A','OD')";
                                    ds11.Clear();
                                    ds11 = da.select_method_wo_parameter(strperiod, "Text");
                                    if (ds11.Tables.Count > 0 && ds11.Tables[0].Rows.Count > 0)
                                    {
                                        for (int leave = 0; leave < ds11.Tables[0].Rows.Count; leave++)
                                        {
                                            hatattnd.Add(Convert.ToString(ds11.Tables[0].Rows[leave]["LeaveCode"]).Trim(), Convert.ToString(ds11.Tables[0].Rows[leave]["DispText"]).Trim());
                                        }
                                    }

                                    string[] split_examdate = examdate.Split('/');
                                    string date_check = split_examdate[1].ToString();
                                    date_check = date_check.TrimStart('0');
                                    string month_check = split_examdate[0].ToString();
                                    string year_Check = split_examdate[2].ToString();
                                    int startsem_date = (Convert.ToInt32(month_check) + Convert.ToInt32(year_Check) * 12);
                                    int check_date_count = Convert.ToInt32(endperiod) - Convert.ToInt32(startperiod);
                                    if (GridView3.Rows.Count > 0)
                                    {
                                        for (int fp = 0; fp < GridView3.Rows.Count; fp++)
                                        {
                                            string roll_no_check = GridView3.Rows[fp].Cells[1].Text.ToString();
                                            arradd_check.Clear();
                                            string count_date_hour = string.Empty;
                                            for (int h = Convert.ToInt32(startperiod); h <= Convert.ToInt32(endperiod); h++)
                                            {
                                                if (count_date_hour == "")
                                                {
                                                    count_date_hour = "[d" + date_check + "d" + h + "]";
                                                }
                                                else
                                                {
                                                    count_date_hour = count_date_hour + "," + "[d" + date_check + "d" + h + "]";
                                                }
                                            }
                                            string check_query = string.Empty;
                                            check_query = "select " + count_date_hour + " from Attendance where roll_no ='" + roll_no_check + "' and month_year='" + startsem_date + "'";
                                            ds11.Clear();
                                            ds11 = da.select_method_wo_parameter(check_query, "Text");
                                            if (ds11.Tables.Count > 0 && ds11.Tables[0].Rows.Count > 0)
                                            {
                                                for (int col = 0; col < ds11.Tables[0].Columns.Count; col++)
                                                {
                                                    string attnd_value = ds11.Tables[0].Rows[0][col].ToString();
                                                    if (attnd_value.ToString().Trim() != "")
                                                    {
                                                        if (arradd_check.Count == 0)
                                                        {
                                                            arradd_check.Add(attnd_value.ToString());
                                                        }
                                                        else if (arradd_check.Contains(attnd_value) == true)
                                                        {
                                                            arradd_check.Add(attnd_value.ToString());
                                                        }
                                                    }
                                                }
                                                if (arradd_check.Count == ds11.Tables[0].Columns.Count)
                                                {
                                                    if (hatattnd.Count > 0)
                                                    {
                                                        string value = Convert.ToString(hatattnd[arradd_check[0].ToString()]);
                                                        if (value != "")
                                                        {
                                                            if (value == "A")
                                                            {

                                                                mrkatnd = "AAA";
                                                            }
                                                            else if (value == "OD")
                                                            {

                                                                mrkatnd = "OD";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }// *************** 17-10-2014******************* End**********************


                        string Newtemp = "New";
                        if (Newtemp != null)
                        {

                        }

                        if (exam_code != "NE")
                        {
                            string mrks = "select marks_obtained,Retest_Marks_obtained,roll_no from Result  where  exam_code = '" + exam_code + "'";
                            DataSet dsmrk = da.select_method_wo_parameter(mrks, "text");
                            //------------------------------------------------ loop for display the details from the result table
                            for (int res = 0; res < dtmrk.Rows.Count; res++)
                            {
                                int colco = 0;
                                colco = dtmrk.Columns.Count - 10;
                                for (int col = 9; col <= colco; col++)
                                {
                                    DataTable dtroll = new DataTable();
                                    DataTable dtmrks = new DataTable();
                                    rollno = dtmrk.Rows[res]["RollNo"].ToString();
                                    // string resultmark = "select marks_obtained from Result where roll_no='" + rollno + "'and exam_code = '" + exam_code + "'";
                                    dsmrk.Tables[0].DefaultView.RowFilter = " roll_no='" + rollno + "'";
                                    DataView dvmrk = dsmrk.Tables[0].DefaultView;
                                    string chkmark = string.Empty;
                                    string getma = string.Empty;
                                    if (dvmrk.Count > 0)
                                    {
                                        chkmark = Convert.ToString(dvmrk[0]["marks_obtained"]);

                                        getma = Convert.ToString(dvmrk[0]["Retest_Marks_obtained"]);
                                    }
                                    string remark = getma;
                                    if (chkmark.Trim() != "0" && chkmark.Trim() != "" && chkmark.Trim() != null)
                                    {
                                        Delete.Enabled = true;
                                        Save.Text = "Update";
                                    }
                                    if (getma.Trim() != "")
                                    {
                                        getma = chkmark;
                                        chkmark = remark;
                                    }
                                    mrkatnd = chkmark;
                                    retstmrk1 = getma;

                                    if (chkretest.Checked == true && cbDaywisePeriodAttSchedule == true)
                                    {
                                        float.TryParse(Convert.ToString(txt_RetestMin.Text), out new_minmark);
                                    }
                                    if (chkmark.Trim() != "")
                                    {
                                        Double pama = Convert.ToDouble(chkmark);
                                        if (pama <= new_minmark)//modified By Srinath 26 Sep 2016
                                        {
                                            retstmrk1 = getma;
                                        }
                                    }
                                    else
                                    {
                                        double obtmrk = 0;
                                        double.TryParse(chkmark.Trim(), out obtmrk);
                                        if (obtmrk <= new_minmark)//modified By Srinath 26 Sep 2016
                                        {

                                            retstmrk1 = getma;
                                        }
                                    }
                                    chkmark = mrkatnd;

                                    if (Convert.ToString(chkmark) == "-1")
                                    {
                                        mrkatnd = "AAA";
                                    }
                                    if (Convert.ToString(chkmark) == "-2")
                                    {
                                        mrkatnd = "EL";
                                    }
                                    if (Convert.ToString(chkmark) == "-3")
                                    {
                                        mrkatnd = "EOD";
                                    }
                                    if (Convert.ToString(chkmark) == "-4")
                                    {
                                        mrkatnd = "ML";
                                    }
                                    if (Convert.ToString(chkmark) == "-5")
                                    {
                                        mrkatnd = "SOD";
                                    }
                                    if (Convert.ToString(chkmark) == "-6")
                                    {
                                        mrkatnd = "NSS";
                                    }
                                    if (Convert.ToString(chkmark) == "-16")
                                    {
                                        mrkatnd = "OD";
                                    }
                                    if (Convert.ToString(chkmark) == "-7")
                                    {
                                        mrkatnd = "NJ";
                                    }
                                    if (Convert.ToString(chkmark) == "-8")
                                    {
                                        mrkatnd = "S";
                                    }
                                    if (Convert.ToString(chkmark) == "-9")
                                    {
                                        mrkatnd = "L";
                                    }
                                    if (Convert.ToString(chkmark) == "-10")
                                    {
                                        mrkatnd = "NCC";
                                    }
                                    if (Convert.ToString(chkmark) == "-11")
                                    {
                                        mrkatnd = "HS";
                                    }
                                    if (Convert.ToString(chkmark) == "-12")
                                    {
                                        mrkatnd = "PP";
                                    }
                                    if (Convert.ToString(chkmark) == "-13")
                                    {
                                        mrkatnd = "SYOD";
                                    }
                                    if (Convert.ToString(chkmark) == "-14")
                                    {
                                        mrkatnd = "COD";
                                    }
                                    if (Convert.ToString(chkmark) == "-15")
                                    {
                                        mrkatnd = "OOD";
                                    }
                                    if (Convert.ToString(chkmark) == "-17")
                                    {
                                        mrkatnd = "LA";
                                    }
                                    //****Modified By Subburaj 20.08.2014******//
                                    if (Convert.ToString(chkmark) == "-18")
                                    {
                                        mrkatnd = "RAA";
                                    }

                                    //*******************End********************//
                                }

                                dtmrk.Rows[res]["test"] = mrkatnd;
                                dtmrk.Rows[res]["retest"] = retstmrk1;
                                dtmrk.Rows[res]["maxmrk"] = mxmrk1;
                                dtmrk.Rows[res]["minmrk"] = mimrk1;
                                //dtmrk.Rows[res]["section"] = sections1;
                                //dtmrk.Rows[res]["degree"] = degreecode1;
                                //dtmrk.Rows[res]["semester"] = semester1;
                                //dtmrk.Rows[res]["batch"] = batch1;
                                //dtmrk.Rows[res]["subno"] = subno1;

                            }
                            //for loop end
                        }
                        if ((isval == 1) && (date == "" || month == "" || year == ""))
                        {

                            lblnorec.Visible = true;
                            lblnorec.Text = "Please Enter The ExamDate";
                        }
                        if ((isval == 1) && (date1 == "" || month1 == "" || year1 == ""))
                        {

                            lblnorec.Visible = true;
                            lblnorec.Text = "Please Enter The EntryDate";
                        }
                        if ((isval == 1) && (hours == "" || minutes == ""))
                        {

                            lblnorec.Visible = true;
                            lblnorec.Text = "Please Enter the Duration";
                        }
                        //Added by srinath 30/4/2014
                        if (GridView2.Columns[14].Visible == true)
                        {
                            if ((isval == 1) && (startperiod == ""))
                            {

                                lblnorec.Visible = true;
                                lblnorec.Text = "Please Enter The Start Period";
                            }
                        }
                        //Added by srinath 30/4/2014
                        if (GridView2.Columns[15].Visible == true)
                        {
                            if ((isval == 1) && (endperiod == ""))
                            {

                                lblnorec.Visible = true;
                                lblnorec.Text = "Please Enter The End Period";
                            }
                        }
                        if ((startperiod != "") && (endperiod != ""))
                        {
                            if (Convert.ToInt32(startperiod) > Convert.ToInt32(endperiod))
                            {
                                lblnorec.Visible = true;
                                lblnorec.Text = "Start Period Should Be Less Than End Period";

                            }
                        }
                        if ((examdate != "") && (entrydate != ""))
                        {
                            string semstartdate = GetFunction("select start_date from seminfo where degree_code='" + degreecode1.ToString() + "' and semester='" + semester1.ToString() + "' and batch_year='" + batch1.ToString() + "'");
                            DateTime dt1 = new DateTime();
                            if (!string.IsNullOrEmpty(semstartdate))  //modified by prabha on jan 26 2018
                                dt1 = Convert.ToDateTime(semstartdate.ToString());
                            DateTime dt2 = Convert.ToDateTime(examdate.ToString());
                            if (dt1 >= dt2)
                            {
                                lblnorec.Visible = true;
                                lblnorec.Text = "ExamDate Should be Greater than Semester start Date ";
                            }
                        }

                        if ((examdate != "") && (entrydate != ""))
                        {
                            DateTime dt1 = Convert.ToDateTime(examdate.ToString());
                            DateTime dt2 = Convert.ToDateTime(entrydate.ToString());
                            TimeSpan t = dt2.Subtract(dt1);
                            long days = t.Days;
                            if (days < 0)
                            {
                                lblnorec.Visible = true;
                                lblnorec.Text = "EntryDate Should be Greater than ExamDate ";
                            }
                        }
                        else
                        {
                            lblnorec.Text = "Enter the Date";
                        }

                    }
                    else
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = ("Please Select The Test");
                        return;
                    }
                }
                else
                {
                    lblnorec.Visible = false;
                    lblnorec.Text = string.Empty;
                    string GetCellTag = string.Empty;
                    string excat = string.Empty;
                    string encat = string.Empty;
                    string selecttest = string.Empty;
                    string settingtest = string.Empty;
                    string dt = DateTime.Today.ToShortDateString();
                    string[] dsplit = dt.Split(new Char[] { '/' });
                    string dcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                    int splityr = 0;
                    splityr = Convert.ToInt32(dsplit[2].ToString());
                    ArrayList check_sameperiod = new ArrayList();
                    //---------------------------------- chk the condition for the flag value-chkbox

                    if (isflag == true)
                    {
                        //---------------------------------loop for get the values from fpreport
                        lblnorec.Text = string.Empty;
                        gettest = string.Empty;
                        GetCellNote = string.Empty;
                        GetCellTag = string.Empty;

                        //------------------------------------------------------to increment the clmn in fpsettings

                        if ((date != "") && (month != "") && (year != "") && (date != null) && (month != null) && (year != null))
                        {
                            examdate = month + "/" + date + "/" + year;
                        }
                        else
                        {
                            examdate = string.Empty;// DateTime.Now.ToString("MM/dd/yyyy");
                        }

                        if ((date1 != "") && (month1 != "") && (year1 != "") && (date1 != null) && (month1 != null) && (year1 != null))
                        {
                            entrydate = month1 + "/" + date1 + "/" + year1;
                        }
                        else
                        {
                            entrydate = DateTime.Now.ToString("MM/dd/yyyy");
                        }
                        if ((hours == "") && (minutes == ""))
                        {
                            duration = "00:00:00";
                        }

                        if ((minutes != null) && (hours != null) && (hours != "") && (minutes != ""))
                        {
                            duration = hours.Trim().PadLeft(2, '0') + ":" + minutes.Trim().PadLeft(2, '0') + ":00";
                        }
                        else
                        {
                            duration = "00:00:00";
                        }

                        new_minmark = Convert.ToInt32(mimrk1);

                        string examCode = exam_code;//26.03.12s
                        DataSet dsNew = da.select_method_wo_parameter("select s.subjectId,s.subSubjectName,s.minMark,s.maxMark,s.examCode from subsubjectTestDetails s where s.examCode='" + examCode + "'", "text");
                        DataSet dsStudentMark = da.select_method_wo_parameter("select s.subjectId,s.subSubjectName,s.textCode,s.examCode,s.minMark,s.maxMark,sm.appNo,sm.testMark,sm.ReTestMark,sm.remarks from subsubjectTestDetails s,subSubjectWiseMarkEntry sm  where sm.subjectId=s.subjectId and s.examCode='" + examCode + "'", "text");
                        int colscount = GridView3.Columns.Count;
                        string tst2 = string.Empty;
                        string tst3 = string.Empty;
                        string tst4 = string.Empty;
                        string retst2 = string.Empty;
                        tst2 = "test";
                        tst3 = "txttest";
                        tst4 = "test";
                        retst2 = "retest";
                        int couttst = 0;
                        int couttst1 = 0;
                        string subid2 = string.Empty;
                        foreach (DataRow drNew in dsNew.Tables[0].Rows)
                        {
                            if (string.IsNullOrEmpty(subid2))
                                subid2 = Convert.ToString(drNew["subjectId"]).Trim();
                            else
                                subid2 = subid2 + "-" + Convert.ToString(drNew["subjectId"]).Trim();
                        }
                        dtmrk.Rows[0]["subId"] = subid2;
                        int subcot = 0;
                        dicschsub.Clear();
                        string testbxnam = string.Empty;

                        if (dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                        {
                            string subnm1 = string.Empty;
                            lblsubtstdet.Text = "TEST DETAILS - " + sucd + " - " + subname.ToUpper() + "-" + gttst2.ToUpper();
                            foreach (DataRow drNew in dsNew.Tables[0].Rows)
                            {

                                colscount += 2;
                                couttst++;
                                string subSubjectName = Convert.ToString(drNew["subSubjectName"]).Trim();
                                string GetCellTag1 = Convert.ToString(drNew["subjectId"]).Trim();
                                dicschsub.Add(Convert.ToInt32(GetCellTag1), subSubjectName);
                                dicschsub1.Add(tst2, subSubjectName);
                                if (string.IsNullOrEmpty(subnm1))
                                    subnm1 = subSubjectName;
                                else
                                    subnm1 = subnm1 + ";" + subSubjectName;

                                string maxmarkcell = Convert.ToString(drNew["maxMark"]).Trim();
                                string minmarkcell = Convert.ToString(drNew["minMark"]).Trim();
                                dicschsub2.Add(tst3, minmarkcell);
                                if (couttst == 1)
                                {
                                    textboxmin.Text = minmarkcell;
                                    txttestbox.Text = maxmarkcell;
                                }
                                else if (couttst == 2)
                                {
                                    textboxmin1.Text = minmarkcell;
                                    txttestbox1.Text = maxmarkcell;
                                }
                                else if (couttst == 3)
                                {
                                    textboxmin2.Text = minmarkcell;
                                    txttestbox2.Text = maxmarkcell;
                                }

                                string subnm = subSubjectName + "Retest";

                                Str_ExamType = examdate + "-" + entrydate + "-" + duration + "-" + new_maxmark + "-" + maxmark + "-" + new_minmark + "-" + minmark + "-" + startperiod + "-" + endperiod;

                                for (int row = 0; row < dtmrk.Rows.Count; row++)
                                {
                                    string appNo = Convert.ToString(dtmrk.Rows[row]["appno"]);
                                    DataTable dtMarks = new DataTable();
                                    if (dsStudentMark.Tables.Count > 0)
                                    {
                                        dsStudentMark.Tables[0].DefaultView.RowFilter = "subjectId='" + GetCellTag1 + "' and appNo='" + appNo + "'";
                                        dtMarks = dsStudentMark.Tables[0].DefaultView.ToTable();
                                    }
                                    if (dtMarks.Rows.Count > 0)
                                    {
                                        string testMark = Convert.ToString(dtMarks.Rows[0]["testMark"]);
                                        string reTestMark = Convert.ToString(dtMarks.Rows[0]["ReTestMark"]);
                                        double testM = 0;
                                        double reTestM = 0;
                                        if (testMark.Trim() != "0" && testMark.Trim() != "" && testMark.Trim() != null)
                                        {
                                            Delete.Enabled = true;
                                            Save.Text = "Update";
                                        }
                                        string displayTestM = testMark;
                                        string displayReTestM = reTestMark;
                                        double.TryParse(testMark, out testM);
                                        double.TryParse(reTestMark, out reTestM);
                                        if (testM < 0)
                                            displayTestM = getMarkText(testMark);
                                        if (reTestM < 0)
                                            displayReTestM = getMarkText(reTestMark);
                                        dtmrk.Rows[row][tst2] = displayTestM;
                                        dtmrk.Rows[row][retst2] = displayReTestM;

                                        dtmrk.Rows[row]["maxmrk"] = maxmarkcell;
                                        dtmrk.Rows[row]["minmrk"] = minmarkcell;
                                        // subcot++;
                                        // subid2 = "subId" + subcot + "";
                                    }


                                }
                                tst2 = "test" + couttst + "";
                                tst3 = "txttest" + couttst + "";
                                tst4 = "test" + couttst1 + "-" + subSubjectName;
                                if (string.IsNullOrEmpty(testbxnam))
                                    testbxnam = "test-" + subSubjectName;
                                else
                                    testbxnam = testbxnam + ";" + tst4;
                                couttst1++;

                            }
                            int ct3 = dicschsub.Count;
                            lblsubexl.Text = subnm1;
                            lblsubcout.Text = Convert.ToString(ct3);
                            lbltxtbxnam.Text = Convert.ToString(testbxnam);
                            //  dtmrk.Rows[0]["subnm1"] = subnm1;

                        }
                    }
                    else
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = ("Please Select The Test");
                        return;
                    }
                }
                divPopSpread.Visible = true;
                GridView3.DataSource = dtmrk;
                GridView3.DataBind();
                dtmrkcount = Convert.ToInt32(dtmrk.Rows.Count);
                dtmrkcoutcol = Convert.ToInt32(dtmrk.Columns.Count);

                GridView3.Visible = true;
                dicstd.Clear();
                int columns = GridView3.HeaderRow.Cells.Count;
                pHeaderEntry.Visible = true;
                pHeaderReport.Visible = true;
                pHeaderSettings.Visible = true;
                //  Exit1.Visible = true;

            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Staff Name";
                return;
            }
        }
        catch
        {
        }

    }

    protected void gridview3_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        string minmrk = string.Empty;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView3.Columns[1].Visible = true;
            e.Row.Cells[1].Visible = true;
            e.Row.Cells[3].Visible = false;
            GridView3.Columns[3].Visible = false;

            string gettest = string.Empty;
            foreach (GridViewRow grrow1 in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)grrow1.FindControl("cbcell_1");
                if (chk.Checked == true)
                {
                    Label get_tst = grrow1.FindControl("lbltest") as Label;
                    gettest = get_tst.Text;
                    testname = get_tst.Text;
                    tstnameget.Text = testname;
                    lbltsthead1.Text = gettest;
                    lbltsthead2.Visible = false;
                    lbltsthead3.Visible = false;
                    lbltsthead4.Visible = false;
                    lbltsthead5.Visible = false;
                    txttestbox1.Visible = false;
                    txttestbox2.Visible = false;
                    txttestbox3.Visible = false;
                    txttestbox4.Visible = false;
                    textboxmin1.Visible = false;
                    textboxmin2.Visible = false;
                    textboxmin3.Visible = false;
                    textboxmin4.Visible = false;
                    lblmi1.Visible = false;
                    lblmi2.Visible = false;
                    lblmi3.Visible = false;
                    lblmi4.Visible = false;
                    lblmk1.Visible = false;
                    lblmk2.Visible = false;
                    lblmk3.Visible = false;
                    lblmk4.Visible = false;
                }
            }
            e.Row.Cells[5].Text = gettest;
            if (chkretest.Checked == true)
            {
                e.Row.Cells[15].Visible = true;
                GridView3.Columns[15].Visible = true;
                e.Row.Cells[15].Text = gettest + "/Re-Test";

            }
            else
            {
                e.Row.Cells[15].Visible = false;
                GridView3.Columns[15].Visible = false;
            }
            if (Session["Rollflag"].ToString() == "0")
            {
                GridView3.Columns[1].Visible = false;
                e.Row.Cells[1].Visible = false;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                GridView3.Columns[2].Visible = false;
                e.Row.Cells[2].Visible = false;
            }

            foreach (GridViewRow gr in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr.FindControl("cbcell_1");
                if (chk.Checked == true)
                {
                    Label mrk = (Label)gr.FindControl("lblmaxmarks");
                    maximum_mark = mrk.Text;
                    Label mik = (Label)gr.FindControl("lblminmarks");
                    minimum_mark = mik.Text;
                }

            }
            int ct = 4;
            int ct1 = 0;
            // string lblhead = "lbltsthead";
            if (dicschsub.Count > 0)
            {
                foreach (KeyValuePair<int, string> dic in dicschsub)
                {
                    string sub = dic.Value;
                    ct++;
                    ct1++;
                    e.Row.Cells[ct].Text = sub;
                    e.Row.Cells[ct].Visible = true;
                    GridView3.Columns[ct].Visible = true;
                    if (ct1 == 1)
                    {
                        lbltsthead1.Text = sub;
                        lbltsthead2.Visible = false;
                        lbltsthead3.Visible = false;
                        lbltsthead4.Visible = false;
                        lbltsthead5.Visible = false;
                        txttestbox1.Visible = false;
                        txttestbox2.Visible = false;
                        txttestbox3.Visible = false;
                        txttestbox4.Visible = false;
                        textboxmin1.Visible = false;
                        textboxmin2.Visible = false;
                        textboxmin3.Visible = false;
                        textboxmin4.Visible = false;
                        lblmi1.Visible = false;
                        lblmi2.Visible = false;
                        lblmi3.Visible = false;
                        lblmi4.Visible = false;
                        lblmk1.Visible = false;
                        lblmk2.Visible = false;
                        lblmk3.Visible = false;
                        lblmk4.Visible = false;
                    }
                    else if (ct1 == 2)
                    {
                        lbltsthead2.Text = sub;
                        lbltsthead2.Visible = true;
                        lbltsthead3.Visible = false;
                        lbltsthead4.Visible = false;
                        lbltsthead5.Visible = false;
                        txttestbox1.Visible = true;
                        txttestbox2.Visible = false;
                        txttestbox3.Visible = false;
                        txttestbox4.Visible = false;
                        textboxmin1.Visible = true;
                        textboxmin2.Visible = false;
                        textboxmin3.Visible = false;
                        textboxmin4.Visible = false;
                        lblmi1.Visible = true;
                        lblmi2.Visible = false;
                        lblmi3.Visible = false;
                        lblmi4.Visible = false;
                        lblmk1.Visible = true;
                        lblmk2.Visible = false;
                        lblmk3.Visible = false;
                        lblmk4.Visible = false;
                    }
                    else if (ct1 == 3)
                    {
                        lbltsthead3.Text = sub;
                        lbltsthead4.Visible = false;
                        lbltsthead5.Visible = false;
                        lbltsthead3.Visible = true;
                        txttestbox3.Visible = false;
                        txttestbox4.Visible = false;
                        txttestbox2.Visible = true;
                        textboxmin2.Visible = true;
                        textboxmin3.Visible = false;
                        textboxmin4.Visible = false;
                        lblmi2.Visible = true;
                        lblmi3.Visible = false;
                        lblmi4.Visible = false;
                        lblmk2.Visible = true;
                        lblmk3.Visible = false;
                        lblmk4.Visible = false;
                    }
                    else if (ct1 == 4)
                    {
                        lbltsthead4.Text = sub;
                        lbltsthead4.Visible = true;
                        lbltsthead5.Visible = false;
                        txttestbox3.Visible = true;
                        txttestbox4.Visible = false;
                        textboxmin3.Visible = true;
                        textboxmin4.Visible = false;
                        lblmi3.Visible = true;
                        lblmi4.Visible = false;
                        lblmk3.Visible = true;
                        lblmk4.Visible = false;

                    }
                    else if (ct1 == 5)
                    {
                        lbltsthead5.Text = sub;

                        lbltsthead5.Visible = true;
                        txttestbox4.Visible = true;
                        textboxmin4.Visible = true;
                        lblmi4.Visible = true;
                        lblmk4.Visible = true;
                    }

                }
                int cout = dicschsub.Count;
                cout = 10 - cout;
                cout = cout + 4;
                for (int j2 = ct + 1; j2 < 15; j2++)
                {
                    e.Row.Cells[j2].Visible = false;
                    GridView3.Columns[j2].Visible = false;

                }
            }
            else
            {
                e.Row.Cells[5].Visible = true;
                GridView3.Columns[5].Visible = true;
                e.Row.Cells[6].Visible = false;
                GridView3.Columns[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                GridView3.Columns[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                GridView3.Columns[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                GridView3.Columns[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                GridView3.Columns[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                GridView3.Columns[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                GridView3.Columns[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                GridView3.Columns[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                GridView3.Columns[14].Visible = false;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Font.Bold = false;
            e.Row.Cells[1].Font.Bold = false;
            e.Row.Cells[2].Font.Bold = false;
            e.Row.Cells[3].Font.Bold = false;
            if (dicschsub2.Count > 0)
            {
                foreach (KeyValuePair<string, string> dc in dicschsub2)
                {
                    string tstnm = dc.Key;
                    minimum_mark = dc.Value;
                    TextBox mk = e.Row.FindControl(tstnm) as TextBox;
                    string mark = Convert.ToString(mk.Text);
                    if (!string.IsNullOrEmpty(mark))
                    {
                        TextBox txt = e.Row.FindControl(tstnm) as TextBox;
                        //txt.Text = mark;
                        if (mark != "AAA" && mark != "EL" && mark != "EOD" && mark != "ML" && mark != "SOD" && mark != "NSS" && mark != "OD" && mark != "NJ" && mark != "S" && mark != "L" && mark != "NCC" && mark != "HS" && mark != "PP" && mark != "SYOD" && mark != "COD" && mark != "OOD" && mark != "LA" && mark != "RAA")
                            if (Convert.ToDouble(mark) < Convert.ToDouble(minimum_mark))
                            {

                                txt.ForeColor = Color.Red;
                                txt.Font.Underline = true;

                            }
                    }
                }
            }
            else
            {
                TextBox mk = e.Row.FindControl("txttest") as TextBox;
                string mark = Convert.ToString(mk.Text);
                if (!string.IsNullOrEmpty(mark))
                {
                    TextBox txt = e.Row.FindControl("txttest") as TextBox;
                    //txt.Text = mark;
                    if (mark != "AAA" && mark != "EL" && mark != "EOD" && mark != "ML" && mark != "SOD" && mark != "NSS" && mark != "OD" && mark != "NJ" && mark != "S" && mark != "L" && mark != "NCC" && mark != "HS" && mark != "PP" && mark != "SYOD" && mark != "COD" && mark != "OOD" && mark != "LA" && mark != "RAA")
                        if (Convert.ToDouble(mark) < Convert.ToDouble(minimum_mark))
                        {

                            txt.ForeColor = Color.Red;
                            txt.Font.Underline = true;

                        }
                }
            }
            if (chkretest.Checked == true)
            {
                TextBox remk = e.Row.FindControl("txtretest") as TextBox;
                string remark = Convert.ToString(remk.Text);
                if (!string.IsNullOrEmpty(remark) && remark != "AAA" && remark != "EL" && remark != "EOD" && remark != "ML" && remark != "SOD" && remark != "NSS" && remark != "OD" && remark != "NJ" && remark != "S" && remark != "L" && remark != "NCC" && remark != "HS" && remark != "PP" && remark != "SYOD" && remark != "COD" && remark != "OOD" && remark != "LA" && remark != "RAA")
                {
                    if (Convert.ToDouble(remark) < Convert.ToDouble(minimum_mark))
                    {
                        TextBox txt1 = e.Row.FindControl("txtretest") as TextBox;
                        txt1.ForeColor = Color.Red;
                        txt1.Font.Underline = true;
                    }
                }
            }

        }


    }

    protected void gridview3_databound(object sender, EventArgs e)
    {

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {

        string reportname = txtexcelname.Text;

        if (reportname.ToString().Trim() != "")
        {

            string testname = string.Empty;
            foreach (GridViewRow gr in GridView2.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr.FindControl("cbcell_1");
                if (chk.Checked == true)
                {
                    Label tst = (Label)gr.FindControl("lbltest");
                    testname = tst.Text;
                }
            }

            dtexport.Columns.Add("RollNo");

            if (Convert.ToString(Session["Regflag"]) == "1")
            {
                dtexport.Columns.Add("regno");
            }



            dtexport.Columns.Add("StudentName");
            string sb = lblsubexl.Text;
            if (sb.Length > 0)
            {
                string[] spplt = sb.Split(';');
                for (int f = 0; f < spplt.Length; f++)
                {
                    string sub3 = Convert.ToString(spplt[f]);

                    dtexport.Columns.Add(Convert.ToString(sub3));

                }
            }
            else
            {
                dtexport.Columns.Add(testname);
            }

            DataSet dsstud = new DataSet();
            foreach (GridViewRow gr in GridView3.Rows)
            {
                int tstct = 0;
                drexport = dtexport.NewRow();

                Label rolno = (Label)gr.FindControl("lblrollno");
                string rol_no = rolno.Text;
                drexport["RollNo"] = rol_no;

                if (Convert.ToString(Session["Regflag"]) == "1")
                {
                    Label regno = (Label)gr.FindControl("lblregno");
                    string reg_num = regno.Text;
                    drexport["regno"] = reg_num;
                }
                Label studnam = (Label)gr.FindControl("lblstudname");
                string studname = studnam.Text;
                drexport["StudentName"] = studname;
                string tstnam = "txttest";
                if (sb.Length > 0)
                // if (dicschsub.Count > 0)
                {
                    int i1 = 0;
                    string[] spplt = sb.Split(';');
                    for (int f = 0; f < spplt.Length; f++)
                    //foreach (KeyValuePair<int, string> dic in dicschsub)
                    {
                        string sub3 = Convert.ToString(spplt[f]);

                        for (int i = i1; i <= tstct; i++)
                        {

                            TextBox txtmrk = (TextBox)gr.FindControl(tstnam);
                            string mrk = txtmrk.Text;
                            tstct++;
                            tstnam = "txttest" + tstct + "";
                            drexport[Convert.ToString(sub3)] = mrk;
                            i1++;
                            break;
                            // goto lbl;
                        }
                        //lbl:

                    }
                }
                else
                {
                    TextBox txtmrk = (TextBox)gr.FindControl("txttest");
                    string mrk = txtmrk.Text;
                    drexport[testname] = mrk;
                }


                dtexport.Rows.Add(drexport);
            }
            if (dtexport.Columns.Count > 0)
            {
                ExportTable(dtexport, "CamEntry");
            }


        }
        else
        {
            lblerror.Text = "Please Enter Your Report Name";
            lblerror.Visible = true;
            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;
            pHeaderSettings.Visible = true;
        }


    }

    private void ExportTable(DataTable dtt, string filename)
    {
        Response.ClearContent();
        Response.Buffer = true;
        string headername = Convert.ToString(filename + DateTime.Now.ToString("dd/MM/yyyy-hh:ss") + ".xls");
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", headername));
        Response.ContentType = "application/ms-excel";
        DataTable dt = dtt;
        string str = string.Empty;
        foreach (DataColumn dtcol in dt.Columns)
        {
            Response.Write(str + dtcol.ColumnName);
            str = "\t";
        }
        str = string.Empty;
        Response.Write("\n");
        foreach (DataRow dr in dt.Rows)
        {
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string value = Convert.ToString(dr[j]);
                if (value.EndsWith("\r\n"))
                {
                    string[] values = value.Split('\r');
                    string val = values[0];
                    Response.Write(str + val);
                    str = "\t";
                }
                else
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
            }
            str = "\r\n";
        }
        System.Web.HttpContext.Current.Response.Flush();
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnclosepopup_Click(object sender, EventArgs e)
    {
        divPopSpread.Visible = false;
        pHeaderEntry.Visible = true;
        pHeaderReport.Visible = true;
        pHeaderSettings.Visible = true;
    }



}