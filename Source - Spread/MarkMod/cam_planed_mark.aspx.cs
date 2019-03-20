using System;//modified on 31.07.12 fpreport minmark,maxmark column locked
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;

using System.Data.SqlClient;
using System.Drawing;


public partial class cam_planed_mark : System.Web.UI.Page
{
    public bool d_check;
    SqlCommand cmd;
    string srisql = "";
    DataSet srids = new DataSet();
    static ArrayList arrroll = new ArrayList();

    Boolean saveflag = false;
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection d_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection myconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection funconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection markcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection yrcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Attcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string Str_ExamType = "";
    string GetCellNote = "";
    //string strvar = "";
    Boolean Cellclick;
    Boolean Cellclick1;
    string strdayflag;
    string regularflag = "";
    string genderflag = "";
    SqlDataReader stfdr;
    SqlCommand stfcmd;
    string Att_mark;
    string Attvalue = "";
    Boolean serialflag;
    string rollno = "";
    DAccess2 da = new DAccess2();
    static string grouporusercode = "";
    string datelocksetting = "";
    Hashtable hat = new Hashtable();
    public DataSet Bind_Degree(string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public bool daycheck(int CriteriaNo)
    {
        bool daycheck = false;
        string curdate, Dateval;
        int total, k;
        string[] ddate = new string[100];
        curdate = DateTime.Today.ToString();
        d_con.Close();
        d_con.Open();
        SqlCommand cmd = new SqlCommand("select Clock,LastDate from CriteriaforInternal where Criteria_no=" + CriteriaNo + " and Clock = 1 ", d_con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i][0].ToString() != null && ds.Tables[0].Rows[i][1].ToString() != null)
                {
                    if (ds.Tables[0].Rows[i][0].ToString() == "True")
                    {
                        Dateval = ds.Tables[0].Rows[i][1].ToString();
                        string[] sel_date12 = Dateval.Split(new Char[] { ' ' });
                        //string[] sel_date1 = sel_date12[0].Split(new Char[] { '/' });
                        //Dateval = sel_date1[1] + "-" + sel_date1[0] + "-" + sel_date1[2];
                        string[] sel_date13 = curdate.Split(new Char[] { ' ' });
                        //string[] sel_date = sel_date13[0].Split(new Char[] { '/' });
                        //curdate = sel_date[1] + "-" + sel_date[0] + "-" + sel_date[2];
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
            lblnorec.Text = "";





            //  Save.Visible = true;

            // Delete.Visible = true;
            //  Buttontotal.Visible = true;
            // Exit1.Visible = true;

            FpSettings.CommandBar.Visible = false;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            if (!Page.IsPostBack)
            {



                Save1.Visible = false;
                printfp.Visible = false;
                FpReport.Visible = false;
                FpSettings.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                pHeaderEntry.Visible = false;
                pHeaderReport.Visible = false;

                chkmarkattendance.Visible = false;
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


                //FpSettings.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                //FpSettings.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                //FpSettings.Pager.Align = HorizontalAlign.Right;
                //FpSettings.Pager.Font.Bold = true;
                //FpSettings.Pager.Font.Name = "Arial";
                //FpSettings.Pager.ForeColor = Color.DarkGreen;
                //FpSettings.Pager.BackColor = Color.Beige;
                //FpSettings.Pager.BackColor = Color.AliceBlue;
                //FpSettings.Pager.PageCount = 5;

                FpEntry.ActiveSheetView.AutoPostBack = true;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Font.Name = "Book Antiqua";
                style.HorizontalAlign = HorizontalAlign.Center;
                style.ForeColor = Color.Black;
                style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpEntry.Sheets[0].ColumnHeader.DefaultStyle = style;
                FpSettings.Sheets[0].ColumnHeader.DefaultStyle = style;
                FpReport.Sheets[0].ColumnHeader.DefaultStyle = style;
                FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpEntry.Sheets[0].AllowTableCorner = true;
                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                FpEntry.Sheets[0].SheetCorner.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;

                Cellclick1 = false;

                //------------------------------------------------query for the grpvalue
                myconn.Open();
                string sqlgrop = "";
                if (Session["collegecode"] != "")
                {
                    sqlgrop = "select linkvalue from inssettings where college_code=" + Session["collegecode"] + " and linkname='CAM Group'";
                    SqlCommand cmdgrp = new SqlCommand(sqlgrop, myconn);
                    SqlDataReader cmdgrpdr;
                    cmdgrpdr = cmdgrp.ExecuteReader();
                    cmdgrpdr.Read();
                    if (cmdgrpdr.HasRows == true)
                    {
                        //string linkvalue = "";
                        //linkvalue = Convert.ToInt32(cmdgrpdr["linkvalue"]).ToString();
                        if (cmdgrpdr["linkvalue"].ToString() == "1")
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
                string staff_code = "";
                string sqlstf = "";
                string staffname = "";
                string sqldescode = "";
                string sqldesname = "";
                string stfdesname = "";
                string stfdescode = "";
                string sqldepname = "";
                string stfdepname = "";
                staff_code = (string)Session["staff_code"];
                //readcon.Close();
                readcon.Open();
                if (staff_code != "")
                {
                    FpEntry.Visible = true;

                }
                else
                {
                    FpEntry.Visible = false;
                    chkmarkattendance.Visible = false;
                }


                Cellclick = false;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";

                FpSettings.SaveChanges();
                //------------------------------------------------clmn cnt for fpentry and header name

                FarPoint.Web.Spread.TextCellType objlabel = new FarPoint.Web.Spread.TextCellType();
                FpEntry.Sheets[0].ColumnCount = 7; //clmn cnt changed from 6 to 7  on 28.02.12
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch year";
                FpEntry.Sheets[0].Columns[1].CellType = objlabel;
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                FpEntry.Sheets[0].Columns[2].CellType = objlabel;
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Semester";
                FpEntry.Sheets[0].Columns[3].CellType = objlabel;
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Section";
                FpEntry.Sheets[0].Columns[4].CellType = objlabel;
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject";
                FpEntry.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject Code";
                FpEntry.Sheets[0].Columns[6].CellType = objlabel;
                FpEntry.Sheets[0].RowHeader.Cells[0, 0].Text = "S.No";
                FpEntry.Sheets[0].Columns[0].CellType = objlabel;
                FpEntry.Sheets[0].ColumnHeader.Columns[0].Visible = false;
                //------------------------------------------------ to set width for partclr colmn in fpentry
                FpEntry.Sheets[0].Columns[1].Width = 50;
                FpEntry.Sheets[0].Columns[2].Width = 50;
                FpEntry.Sheets[0].Columns[3].Width = 50;
                FpEntry.Sheets[0].Columns[4].Width = 50;
                FpEntry.Sheets[0].Columns[5].Width = 200;

                //----------------------------------------------- to set the style property for the fpentry
                FpEntry.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpEntry.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpEntry.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;

                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;

                FpEntry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;

                FpEntry.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;

                FpEntry.Sheets[0].Columns[0].Font.Underline = false;
                FpEntry.Sheets[0].Columns[1].Font.Underline = false;
                FpEntry.Sheets[0].Columns[2].Font.Underline = false;
                FpEntry.Sheets[0].Columns[3].Font.Underline = false;
                FpEntry.Sheets[0].Columns[5].Font.Underline = true;
                FpEntry.Sheets[0].Columns[6].Font.Underline = false;
                FpEntry.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;

                FpEntry.Sheets[0].Columns[0].ForeColor = Color.Black;
                FpEntry.Sheets[0].Columns[1].ForeColor = Color.Black;
                FpEntry.Sheets[0].Columns[2].ForeColor = Color.Black;
                FpEntry.Sheets[0].Columns[3].ForeColor = Color.Black;
                FpEntry.Sheets[0].Columns[4].ForeColor = Color.Black;
                FpEntry.Sheets[0].Columns[5].ForeColor = Color.Blue;
                FpEntry.Sheets[0].Columns[6].ForeColor = Color.Black;
                //----------------------------------------------------clmn cnt & row cnt for fpreport
                FpReport.Sheets[0].ColumnCount = 12;
                FpReport.Sheets[0].SheetCorner.RowCount = 2;
                //---------------------------------------------------   -to lock the particular column
                FpReport.Columns[1].Locked = true;
                FpReport.Sheets[0].Columns[10].Locked = true;
                FpReport.Sheets[0].Columns[11].Locked = true;
                //--------------------------------------------------to set width for partclr colmn in fpreport

                FpReport.Sheets[0].Columns[0].Width = 60;
                FpReport.Sheets[0].Columns[1].Width = 150;
                FpReport.Sheets[0].Columns[2].Width = 50;
                FpReport.Sheets[0].Columns[3].Width = 50;
                FpReport.Sheets[0].Columns[4].Width = 50;
                FpReport.Sheets[0].Columns[5].Width = 50;
                FpReport.Sheets[0].Columns[6].Width = 50;
                FpReport.Sheets[0].Columns[7].Width = 50;
                FpReport.Sheets[0].Columns[8].Width = 50;
                FpReport.Sheets[0].Columns[9].Width = 50;
                FpReport.Sheets[0].Columns[10].Width = 80;
                FpReport.Sheets[0].Columns[11].Width = 80;
                //--------------------------------------------------------to set the header name for fpentry  

                FarPoint.Web.Spread.TextCellType lblcell = new FarPoint.Web.Spread.TextCellType();
                FpReport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                FpReport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Test";
                FpReport.Sheets[0].Columns[1].CellType = lblcell;
                FpReport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "ExamDate";
                FpReport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
                FpReport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Month";
                FpReport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "EntryDate";
                FpReport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Date";
                FpReport.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Month";
                FpReport.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Duration";
                // FpReport.Sheets[0].ColumnHeader.Cells[0, 9].Text = "MaxMark";
                FpReport.Sheets[0].ColumnHeader.Cells[0, 10].Text = "MaxMark";
                FpReport.Sheets[0].Columns[10].CellType = lblcell;
                FpReport.Sheets[0].ColumnHeader.Cells[0, 11].Text = "MinMark";
                FpReport.Sheets[0].Columns[11].CellType = lblcell;

                //---------------------------------------------------------------- to set the style property for the fpreport
                FpReport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpReport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpReport.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;


                FpReport.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                FpReport.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpReport.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
                FpReport.SheetCorner.Cells[0, 0].Font.Bold = true;

                FpReport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpReport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpReport.Sheets[0].DefaultStyle.Font.Bold = false;


                FarPoint.Web.Spread.TextCellType lblccell = new FarPoint.Web.Spread.TextCellType();



                if (staff_code == null || staff_code == "")
                {

                    RequiredFieldValidator1.Visible = true;
                    RequiredFieldValidator2.Visible = true;
                    RequiredFieldValidator3.Visible = true;
                    RequiredFieldValidator4.Visible = true;
                    RequiredFieldValidator5.Visible = true;
                    ddlBatch.Visible = true;
                    ddlDegree.Visible = true;
                    ddlBranch.Visible = true;
                    ddlSemYr.Visible = true;
                    ddlSec.Visible = true;

                    FpEntry.Sheets[0].Visible = true;
                    FpReport.Sheets[0].Visible = true;
                    FpSettings.Sheets[0].Visible = true;
                    ddlDegree.AutoPostBack = true;
                    ddlBranch.AutoPostBack = true;
                    ddlSemYr.AutoPostBack = true;
                    ddlSec.AutoPostBack = true;
                    FpEntry.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                    BindBatch(); // Binding Batch in drop down list
                    BindDegree();
                    if (ddlDegree.Items.Count > 0)
                    {
                        //'-------------branch loading
                        string course_id = ddlDegree.SelectedValue.ToString();
                        //string sem = ddlSem.SelectedValue.ToString();
                        string collegecode = Session["collegecode"].ToString();
                        string usercode = Session["UserCode"].ToString();//Session["UserCode"].ToString();
                        DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlBranch.DataSource = ds;
                            ddlBranch.DataTextField = "Dept_Name";
                            ddlBranch.DataValueField = "degree_code";
                            ddlBranch.DataBind();
                            //     ddlBranch.Items.Insert(0, new ListItem("- -Select- -", "-1"));

                        }

                        //'------------------------------semester loading
                        //Get_Semester();
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

                    //Table1.Visible = false;
                    btnGo.Visible = false;
                    FpEntry.Sheets[0].Visible = true;
                    FpReport.Sheets[0].Visible = true;
                    FpSettings.Sheets[0].Visible = true;
                    FpEntry.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                }

                myconn.Close();
                //********************************************* to display the subject details in the fpentry for the individual staff login
                myconn.Open();
                if (staff_code != "")
                {
                    string sqlstaff = "";
                    int rowcnt;
                    //aruna on 20/09/20111 sqlstaff = "select distinct s.subject_no,s.subject_name,s.syll_code,st.batch_year,semester,sections,degree_code from subject s,syllabus_master sy,staff_selector st where s.syll_code=sy.syll_code and st.subject_no=s.subject_no and st.batch_year=sy.batch_year  and staff_code='" + Session["staff_code"].ToString() + "' order by st.batch_year,degree_code,semester,sections ";
                    //sqlstaff = "select distinct s.subject_no,s.subject_name,s.subject_code,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (st.sections =isnull(r.sections,'-1')) and staff_code='" + Session["staff_code"].ToString() + "' order by st.batch_year,sy.degree_code,semester,st.sections ";
                    sqlstaff = "select distinct s.subject_no,s.subject_name,s.subject_code,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (st.sections =isnull(r.sections,'-1') or st.sections=ISNULL(NULLIF(r.sections, ''),'-1')) and staff_code='" + Session["staff_code"].ToString() + "' order by st.batch_year,sy.degree_code,semester,st.sections ";
                    SqlCommand staffcmd = new SqlCommand(sqlstaff, myconn);
                    SqlDataReader dr;
                    dr = staffcmd.ExecuteReader();
                    FpEntry.Sheets[0].RowCount = 0;
                    while (dr.Read())
                    {
                        string current_sem = "";
                        current_sem = GetFunction("select distinct current_semester from registration where degree_code='" + dr["degree_code"].ToString() + "' and batch_year='" + dr["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar'");

                        if (Convert.ToString(current_sem) == Convert.ToString(dr["semester"]))
                        {
                            FpEntry.Sheets[0].RowCount += 1;
                            if ((Session["collegecode"] != "") && dr["degree_code"].ToString() != "")
                            {
                                string sqlstr = "";
                                sqlstr = "select course_name + '-'+dept_acronym from degree d,course c,department dp where d.course_id=c.course_id and d.dept_code=dp.dept_code and degree_code= " + dr["degree_code"].ToString() + " and c.college_code='" + Session["collegecode"] + "'";

                                string degree = "";
                                degree = GetFunction(sqlstr.ToString());

                                rowcnt = Convert.ToInt32(FpEntry.Sheets[0].RowCount) - 1;
                                FpEntry.Sheets[0].Cells[rowcnt, 0].Text = rowcnt.ToString();
                                FpEntry.Sheets[0].Cells[rowcnt, 1].Text = dr["batch_year"].ToString();
                                FpEntry.Sheets[0].Cells[rowcnt, 2].Tag = dr["degree_code"].ToString();
                                FpEntry.Sheets[0].Cells[rowcnt, 2].Text = degree.ToString();

                                if (dr["semester"] == "-1")
                                {
                                    FpEntry.Sheets[0].Cells[rowcnt, 3].Text = " ";
                                }
                                else
                                {
                                    FpEntry.Sheets[0].Cells[rowcnt, 3].Text = dr["semester"].ToString();
                                }

                                if (dr["sections"] == "-1")
                                {
                                    FpEntry.Sheets[0].Cells[rowcnt, 4].Text = " ";
                                }
                                else
                                {
                                    FpEntry.Sheets[0].Cells[rowcnt, 4].Text = dr["sections"].ToString();
                                }

                                if (FpEntry.Sheets[0].Cells[rowcnt, 4].Text == "-1")
                                {
                                    FpEntry.Sheets[0].Cells[rowcnt, 4].Text = " ";
                                }

                                FpEntry.Sheets[0].Cells[rowcnt, 5].Tag = dr["subject_no"].ToString();
                                FpEntry.Sheets[0].Cells[rowcnt, 5].Text = dr["subject_name"].ToString();
                                FpEntry.Sheets[0].Cells[rowcnt, 6].Text = dr["subject_code"].ToString();
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
                FpEntry.Sheets[0].AutoPostBack = true;

                //---------------------------------------------------------------Query for the master settings
                if (Session["usercode"] != "")
                {
                    string Master = "";
                    Master = "select * from Master_Settings where " + grouporusercode + "";
                    readcon.Close();
                    readcon.Open();
                    SqlDataReader mtrdr;

                    SqlCommand mtcmd = new SqlCommand(Master, readcon);
                    mtrdr = mtcmd.ExecuteReader();
                    strdayflag = "";
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
                GetTest();
            }

            FpSettings.SaveChanges();
            if (FpEntry.Sheets[0].RowCount == 0)
            {
                FpEntry.Visible = false;
            }
            else
            {

                //  FpEntry.Visible = true;
            }
        }
        catch
        {

        }
    }

    public void GetTest()
    {
        try
        {
            ddlTest.Items.Clear();
            con.Close();
            con.Open();
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = "";


            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";


            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
            DataSet titles = new DataSet();
            con.Close();
            con.Open();
            sqlAdapter1.Fill(titles);
            if (titles.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = titles;
                ddlTest.DataValueField = "Criteria_No";
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataBind();
                // ddlTest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));

            }
        }
        catch
        {

        }

    }
    public void clear()
    {
        ddlSemYr.Items.Clear();
        // ddlSec.Items.Clear();
    }
    //---------------------------------------------- bind the batch in dropdown
    public void BindBatch()
    {

        string sqlstr = "";
        int max_bat = 0;
        //int typeval = 1;
        //string collegecode = Session["collegecode"].ToString();//Getting Collegecode from session
        DataSet ds = ClsAttendanceAccess.GetBatchDetail();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBatch.DataSource = ds;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
            sqlstr = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
            max_bat = Convert.ToInt32(GetFunction(sqlstr));
            ddlBatch.SelectedValue = max_bat.ToString();

            //ddlBatch.Items.Insert(0, new ListItem("- -Select- -", "-1"));

        }
    }
    //------------------------------------------------to bind the degree in dropdown
    public void BindDegree()
    {
        //int a = 2;


        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        //string degree = ddlDegree.SelectedValue.ToString();
        DataSet ds = Bind_Degree(collegecode.ToString(), usercode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
            //ddlDegree.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        }
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {

        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;

        FpEntry.Visible = false;
        FpReport.Visible = false;
        FpSettings.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;

        Save1.Visible = false;
        printfp.Visible = false;






        lblErrorMsg.Visible = false;

        ddlBranch.Items.Clear();
        //string  a = 13;
        string course_id = ddlDegree.SelectedValue.ToString();
        //string sem = ddlSem.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["UserCode"].ToString();//Session["UserCode"].ToString();
        DataSet ds = Bind_Dept(course_id, collegecode, usercode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
            // ddlBranch.Items.Insert(0, new ListItem("- -Select- -", "-1"));

        }
        //'----------- call the semester and section
        bindsem();
        BindSectionDetail();

    }
    public void bindsem()
    {

        //--------------------semester load
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
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
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlSemYr.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());

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

            dr1.Close();
        }
        if (ddlSemYr.Items.Count > 0)
        {
            ddlSemYr.SelectedIndex = 0;
            BindSectionDetail();
        }
        GetTest();
        //     ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
        con.Close();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;

        FpEntry.Visible = false;
        FpReport.Visible = false;
        FpSettings.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;

        Save1.Visible = false;
        printfp.Visible = false;




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
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }


    //---------------------------------------- to bind the section
    public void BindSectionDetail()
    {

        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlBatch.SelectedValue.ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
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

    //----------------------------------------to bind the semester
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        //int typeval = 4;

        string batch = ddlBatch.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        //Session["collegecode"].ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());

        if (ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

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
    //--------------------------------------------- defn for getfunction
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
    //--------------------------------------------------------- coding for ok btn

    protected void FpSettings_KeyPress(Object sender, EventArgs e)
    {


    }
    protected void Save1_Click(object sender, EventArgs e)
    {
        try
        {
            string rollxx = "";
            string plnedmark = "";
            int a = 0;
            string exmcode = FpSettings.Sheets[0].Cells[1, 0].Tag.ToString();
            int cc = FpSettings.Sheets[0].Rows.Count - 3;
            for (int n = 0; n <= cc; n++)
            {
                rollxx = FpSettings.Sheets[0].Cells[n, 1].Text.ToString();

                plnedmark = FpSettings.Sheets[0].Cells[n, 4].Text.ToString();

                if (plnedmark == "")
                {
                    plnedmark = "0";
                }
                srisql = "if exists (select * from Result where roll_no='" + rollxx + "'and exam_code = '" + exmcode + "') begin update Result set planedmark='" + plnedmark + "' where roll_no='" + rollxx + "'and exam_code = '" + exmcode + "' end ";
                hat.Clear();
                a = da.insert_method(srisql, hat, "Text");




            }

            if (a != 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

            }


        }
        catch
        {

        }

    }
    //---------------------------------------------------------------------coding for save btn

    protected void FpEntry_SelectedIndexChanged(Object sender, EventArgs e)
    {
        callentryselectnew();
        if (FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 4].Text == "")
        {
        }
        else
        {

            decimal plaper = Convert.ToDecimal(FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 4].Text);
            plaper = Math.Round(plaper, 2);

            FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(plaper);
        }

    }


    public void callentryselectnew()
    {
        if (Cellclick == true)
        {


            int minimark = 0;
            int maxmark = 0;
            string subno = "";
            string batch = "";
            string sections = "";
            int markcount = 0;
            decimal acualpert = 0;
            decimal acualtotmark = 0;
            int rowcount01 = 0;
            decimal plnedpert = 0;

            string semester = "";
            string degreecode = "";
            semester = FpEntry.Sheets[0].GetText(Convert.ToInt32(FpEntry.Sheets[0].ActiveRow), 3);
            degreecode = FpEntry.Sheets[0].Cells[Convert.ToInt32(FpEntry.Sheets[0].ActiveRow), 2].Tag.ToString();
            batch = FpEntry.Sheets[0].GetText(Convert.ToInt32(FpEntry.Sheets[0].ActiveRow), 1);
            subno = FpEntry.Sheets[0].Cells[Convert.ToInt32(FpEntry.Sheets[0].ActiveRow), 5].Tag.ToString();
            sections = FpEntry.Sheets[0].GetText(Convert.ToInt32(FpEntry.Sheets[0].ActiveRow), 4);
            string strsec = "";

            if (sections == "All" || sections == " " || sections == "-1" || sections == null)
            {
                strsec = "";
            }
            else
            {
                strsec = " and sections='" + sections + "'";
            }


            srisql = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + "' " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";
            srids.Clear();
            srids = da.select_method_wo_parameter(srisql, "Text");

            DataView testno = new DataView();
            srids.Tables[0].DefaultView.RowFilter = "criteria_no='" + ddlTest.SelectedItem.Value + "'";
            testno = srids.Tables[0].DefaultView;
            int count4 = 0;
            count4 = testno.Count;

            if (count4 > 0)
            {
                minimark = Convert.ToInt32(testno[0][3]);
                maxmark = Convert.ToInt32(testno[0][2]);
                Session["maximummark"] = Convert.ToString(maxmark);

            }
            string subxx = FpEntry.Sheets[0].Cells[Convert.ToInt32(FpEntry.Sheets[0].ActiveRow), 5].Tag.ToString();
            srisql = "select exam_code from Exam_type where  subject_no='" + subxx + "' and criteria_no='" + ddlTest.SelectedItem.Value + "'   " + strsec + " ";
            srids.Clear();
            srids = da.select_method_wo_parameter(srisql, "Text");

            string emcode = "";

            if (srids.Tables[0].Rows.Count == 0)
            {


                pHeaderEntry.Visible = true;
                pHeaderReport.Visible = false;
                //pHeaderReport.Visible = false;
                // pHeaderSettings.Visible = true;
                FpSettings.Visible = false;
                lblErrorMsg.Text = "Test Not Conducted";
                lblErrorMsg.Visible = true;

                Save1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                printfp.Visible = false;
                return;



            }
            else
            {
                emcode = srids.Tables[0].Rows[0][0].ToString();
            }

            string rn = "";
            string plnedmark = "";
            int cc = FpSettings.Sheets[0].Rows.Count - 3;
            for (int n = 0; n <= cc; n++)
            {
                rn = FpSettings.Sheets[0].Cells[n, 1].Text.ToString();
                srisql = "select * from Result where roll_no='" + rn + "'and exam_code = '" + emcode + "'";
                srids.Clear();
                srids = da.select_method_wo_parameter(srisql, "Text");

                if (srids.Tables[0].Rows.Count > 0)
                {

                    string chkmark = "";

                    plnedmark = srids.Tables[0].Rows[0][3].ToString();
                    chkmark = srids.Tables[0].Rows[0][0].ToString();
                    FpSettings.Sheets[0].Cells[n, 5].Text = chkmark;
                    if (Convert.ToInt32(chkmark) > 0)
                    {
                        acualtotmark = acualtotmark + Convert.ToInt32(chkmark);
                    }

                    if (Convert.ToString(chkmark) == "-1")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "AAA";
                    }
                    if (Convert.ToString(chkmark) == "-2")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "EL";
                    }
                    if (Convert.ToString(chkmark) == "-3")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "EOD";
                    }
                    if (Convert.ToString(chkmark) == "")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "0";
                    }
                    if (Convert.ToString(chkmark) == "-4")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "ML";
                    }
                    if (Convert.ToString(chkmark) == "-5")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "SOD";
                    }
                    if (Convert.ToString(chkmark) == "-6")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "NSS";
                    }
                    if (Convert.ToString(chkmark) == "-16")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "OD";
                    }
                    if (Convert.ToString(chkmark) == "-7")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "NJ";
                    }
                    if (Convert.ToString(chkmark) == "-8")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "S";
                    }
                    if (Convert.ToString(chkmark) == "-9")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "L";
                    }
                    if (Convert.ToString(chkmark) == "-10")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "NCC";
                    }
                    if (Convert.ToString(chkmark) == "-11")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "HS";
                    }
                    if (Convert.ToString(chkmark) == "-12")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "PP";
                    }
                    if (Convert.ToString(chkmark) == "-13")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "SYOD";
                    }
                    if (Convert.ToString(chkmark) == "-14")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "COD";
                    }
                    if (Convert.ToString(chkmark) == "-15")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "OOD";
                    }
                    if (Convert.ToString(chkmark) == "-17")
                    {
                        FpSettings.Sheets[0].Cells[n, 5].Text = "LA";
                    }





                    arrroll.Add(rn);


                }
                else
                {
                    FpSettings.Sheets[0].Cells[n, 5].Text = "0";

                }
                if (plnedmark != "")
                {
                    FpSettings.Sheets[0].Cells[n, 4].Text = plnedmark;
                    plnedpert = plnedpert + Convert.ToDecimal(plnedmark);
                }
                else
                {
                    FpSettings.Sheets[0].Cells[n, 4].Text = plnedmark;
                }

                markcount++;
                FpSettings.Sheets[0].Cells[n, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSettings.Sheets[0].Cells[n, 5].VerticalAlign = VerticalAlign.Middle;
                FpSettings.Sheets[0].Cells[n, 5].CellType = txt;

                FpSettings.Sheets[0].Cells[n, 4].HorizontalAlign = HorizontalAlign.Center;
                FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
                intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
                intgrcel.MaximumValue = Convert.ToInt32(maxmark.ToString());
                intgrcel.MinimumValue = 0;
                intgrcel.ErrorMessage = "Enter valid mark";
                FpSettings.Sheets[0].Cells[n, 4].CellType = intgrcel;

            }



            if (markcount > 0)
            {
                int rowco = FpSettings.Sheets[0].RowCount - 2;

                acualpert = (acualtotmark / (rowco * maxmark)) * 100;
                plnedpert = (plnedpert / (rowco * maxmark)) * 100;
                acualpert = Math.Round(acualpert, 2);
                FpSettings.Sheets[0].SpanModel.Add(FpSettings.Sheets[0].RowCount - 2, 0, 1, 4);
                FpSettings.Sheets[0].Cells[1, 0].Tag = emcode;
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 2, 0].Text = "Actual Percentage";
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 2, 0].ForeColor = Color.Blue;
                FpSettings.Sheets[0].Rows[FpSettings.Sheets[0].RowCount - 2].BackColor = Color.AliceBlue;
                FpSettings.Sheets[0].SpanModel.Add(FpSettings.Sheets[0].RowCount - 2, 4, 1, 2);
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 2, 4].Text = Convert.ToString(acualpert);


                FpSettings.Sheets[0].SpanModel.Add(FpSettings.Sheets[0].RowCount - 1, 0, 1, 4);

                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 0].Text = "Planed Percentage";
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;
                FpSettings.Sheets[0].Rows[FpSettings.Sheets[0].RowCount - 1].BackColor = Color.AliceBlue;
                FpSettings.Sheets[0].SpanModel.Add(FpSettings.Sheets[0].RowCount - 1, 4, 1, 2);
                FpSettings.Sheets[0].PageSize = FpSettings.Sheets[0].RowCount;

                FpSettings.Sheets[0].Rows[FpSettings.Sheets[0].RowCount - 2].Visible = true;
                FpSettings.Sheets[0].Rows[FpSettings.Sheets[0].RowCount - 1].Visible = true;
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(plnedpert);
                FpSettings.Height = 600;
                pHeaderEntry.Visible = true;
                pHeaderReport.Visible = true;
                // pHeaderSettings.Visible = true;
                lblErrorMsg.Visible = false;
                lblnorec.Text = "";
                FpSettings.Visible = true;
                Save1.Visible = true;
                printfp.Visible = true;
                lblrptname.Visible = true;
                btnExcel.Visible = true;
                txtexcelname.Visible = true;

            }



        }

    }
    //-------------------------------------------------------------function defn for Fpentry Selectedchanged

    //------------------------------------------------------- coding for go btn
    protected void btnGo_Click(object sender, EventArgs e)
    {
        string sections = "", strsec = "";
        if (ddlSec.Enabled == false)
        {
            sections = "";

        }
        if (ddlSec.Enabled == true)
        {
            sections = ddlSec.SelectedItem.Text.ToString();

        }

        if (sections.Trim() == "All" || sections.Trim() == "" || sections.Trim() == "-1" || sections.Trim() == null)
        {
            strsec = "";
        }
        else
        {
            strsec = " and Sections='" + sections + "'";
        }
        pHeaderReport.Visible = false;
        Save1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        printfp.Visible = false;


        srisql = "select roll_no as rollno, stud_name as studentname,reg_no, Convert(nvarchar(50),Batch_Year)+'-'+Convert(nvarchar(50),course.Course_Name)+'-'+ Convert(nvarchar(50),Degree.Acronym)+'-'+ Convert(nvarchar(50),current_semester)+'  Sem'  as details,registration.Roll_Admit  from registration,Degree,course where registration.degree_code in ('" + ddlBranch.SelectedItem.Value + "') and batch_year in ('" + ddlBatch.SelectedItem.Text + "')  " + strsec + " and Degree.Degree_Code=registration.Degree_Code and current_semester in ('" + ddlSemYr.SelectedItem.Text + "') and course.Course_Id=degree.Course_Id and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' ORDER BY  course.Course_Name,Registration.Roll_No,degree.Degree_Code";

        srids.Clear();
        srids = da.select_method_wo_parameter(srisql, "Text");
        if (srids.Tables[0].Rows.Count == 0)
        {
            lblErrorMsg.Text = "No Records Found";
            lblErrorMsg.Visible = true;
            return;
        }
        else
        {

            FpSettings.Sheets[0].RowCount = 0;
            FpSettings.Sheets[0].RowHeader.Visible = false;
            //  Fpspread.Sheets[0].AutoPostBack = false;
            FpSettings.CommandBar.Visible = false;
            FpSettings.Visible = false;


            FpSettings.Sheets[0].ColumnCount = 6;

            FpSettings.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

            FpSettings.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;

            FpSettings.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;

            FpSettings.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;

            FpSettings.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Planed Mark";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;

            FpSettings.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Actual Mark";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSettings.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;

            FpSettings.Sheets[0].Columns[0].Width = 70;
            FpSettings.Sheets[0].Columns[1].Width = 100;
            FpSettings.Sheets[0].Columns[2].Width = 140;
            FpSettings.Sheets[0].Columns[3].Width = 230;
            FpSettings.Sheets[0].Columns[4].Width = 80;
            FpSettings.Sheets[0].Columns[5].Width = 100;

            FpSettings.Width = 800;
            FpSettings.Sheets[0].GridLineColor = Color.Black;

            for (int k = 0; k < 4; k++)
            {
                FpSettings.Sheets[0].Columns[k].Locked = true;
            }

            FpSettings.Sheets[0].Columns[5].Locked = true;

            int sno1 = 0;
            for (int i = 0; i < srids.Tables[0].Rows.Count; i++)
            {
                sno1++;
                FpSettings.Sheets[0].RowCount++;



                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno1);
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 1].Text = srids.Tables[0].Rows[i]["rollno"].ToString();
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 1].CellType = txt;

                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 2].Text = srids.Tables[0].Rows[i]["reg_no"].ToString();
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 2].CellType = txt;
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 3].Text = srids.Tables[0].Rows[i]["studentname"].ToString();
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 3].Tag = srids.Tables[0].Rows[i]["rollno"].ToString();
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;



                //FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 4].Text = srids.Tables[0].Rows[i]["send_date"].ToString();
                //FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                //FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                //FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 4].CellType = txt;


                //FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 5].Text = srids.Tables[0].Rows[i]["details"].ToString();
                //FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                //FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                //FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 5].CellType = txt;





            }
            FpSettings.Sheets[0].RowCount++;
            FpSettings.Sheets[0].RowCount++;
            FpSettings.Sheets[0].Rows[FpSettings.Sheets[0].RowCount - 2].Visible = false;
            FpSettings.Sheets[0].Rows[FpSettings.Sheets[0].RowCount - 1].Visible = false;



            lblErrorMsg.Text = "There are no Records Found";
            lblErrorMsg.Visible = false;
        }
        myconn.Open();
        FpEntry.Sheets[0].RowCount = 0;

        FpEntry.SaveChanges();
        if (ddlSec.Enabled == false)
        {
            Load_gobtn();
        }
        if (ddlSec.Enabled == true)
        {
            Load_gobtn();
        }


    }
    public void Load_gobtn()
    {
        try
        {
            chkmarkattendance.Checked = false;
            string staff_code = "";
            staff_code = (string)Session["staff_code"];
            if (staff_code == null || staff_code == "")
            {
                // pHeaderEntry.Visible = false;
                FpEntry.Sheets[0].ColumnCount = 7;//change clmn count frm 6 to 7 on 28.02.12
                pnlEntry.Visible = true;
                FpEntry.Sheets[0].Visible = true;
                FpEntry.Width = 800;
                FpEntry.Height = 900;
                string strsec = "";
                myconn.Close();
                myconn.Open();
                string SyllabusYr;
                string SyllabusQry;

                //----------------------------------------------------Query for get the syllbs yr
                if (ddlBranch.SelectedValue.ToString() != "" && ddlSemYr.SelectedValue.ToString() != "" && ddlBatch.SelectedValue.ToString() != "")
                {
                    SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
                    SyllabusYr = GetFunction(SyllabusQry.ToString());
                    string Sqlstr;
                    Sqlstr = "";

                    if (ddlSec.Text.ToString() == "All" || ddlSec.Text.ToString() == "")
                    {
                        strsec = "";
                    }
                    else
                    {
                        strsec = " and sections='" + ddlSec.SelectedValue.ToString() + "'";
                    }
                    //------------------------------------------- Query for display the subject name inthe Spread1-Fpentry
                    if (SyllabusYr != "")
                    {
                        if (Session["Staff_Code"].ToString() == "")
                        {
                            Sqlstr = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlBranch.SelectedValue.ToString() + " and syllabus_master.semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_master.batch_year=" + ddlBatch.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlSemYr.SelectedValue.ToString() + " and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0 " + strsec.ToString() + " and exam_flag <> 'DEBAR'";
                        }
                        else if (Session["Staff_Code"].ToString() != "")
                        {
                            Sqlstr = "select distinct subject_name,subject.subject_no,subject_code,sections as sections from subject,sub_sem,staff_selector,usermaster,registration,subjectchooser,syllabus_master where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and  subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlBranch.SelectedValue.ToString() + " and  syllabus_master.semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_master.batch_year= " + ddlBatch.SelectedValue.ToString() + " and syllabus_master.syllabus_year= " + SyllabusYr.ToString() + " and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and  registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlSemYr.SelectedValue.ToString() + " and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and staff_selector.staff_code=usermaster.staff_code and subject.subject_no=staff_selector.subject_no and usermaster.staff_code='" + Session["Staff_Code"].ToString() + "'" + strsec.ToString() + "";
                        }
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
                                    FpEntry.Visible = true;
                                    pHeaderEntry.Visible = true;


                                    FpEntry.Sheets[0].RowCount = Convert.ToInt32(FpEntry.Sheets[0].RowCount) + 1;
                                    FpEntry.Sheets[0].Cells[rowcnt, 0].Text = rowcnt.ToString();
                                    FpEntry.Sheets[0].Cells[rowcnt, 1].Text = ddlBatch.Text.ToString();
                                    FpEntry.Sheets[0].Cells[rowcnt, 2].Tag = ddlBranch.Text.ToString();
                                    FpEntry.Sheets[0].Cells[rowcnt, 3].Text = ddlSemYr.Text.ToString();
                                    FpEntry.Sheets[0].Cells[rowcnt, 4].Text = ddlSec.Text.ToString();
                                    if (FpEntry.Sheets[0].Cells[rowcnt, 4].Text == "-1")
                                    {
                                        FpEntry.Sheets[0].Cells[rowcnt, 4].Text = "";
                                    }
                                    FpEntry.Sheets[0].Cells[rowcnt, 5].Tag = resreader["subject_no"].ToString();
                                    FpEntry.Sheets[0].Cells[rowcnt, 5].Text = resreader["subject_name"].ToString();
                                    FpEntry.Sheets[0].Cells[rowcnt, 6].Text = resreader["subject_code"].ToString();
                                    rowcnt = rowcnt + 1;
                                }
                            }
                            myconn.Close();
                        }
                    }
                    if (FpEntry.Sheets[0].RowCount == 0)
                    {
                        pHeaderEntry.Visible = false;
                        FpEntry.Visible = false;
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "No Record Found";
                        //lblnorec.Visible = true;
                        pHeaderReport.Visible = false;
                        FpReport.Visible = false;

                        FpSettings.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        chkmarkattendance.Visible = false;//added by srinath 15/5/2014

                    }
                    else
                    {
                        //lblnorec.Visible = false;
                        lblErrorMsg.Text = "";
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
    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlSubject.Items.Clear();
        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;

        FpEntry.Visible = false;
        FpReport.Visible = false;
        FpSettings.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        Save1.Visible = false;
        printfp.Visible = false;








        lblErrorMsg.Visible = false;

        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        BindSectionDetail();
        GetTest();
    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;

        FpEntry.Visible = false;
        FpReport.Visible = false;
        FpSettings.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;

        Save1.Visible = false;
        printfp.Visible = false;






        lblErrorMsg.Visible = false;
    }


    protected void FpReport_OnLoad(object sender, EventArgs e)
    {
        //      isValid(val);

    }
    //------------------------------------------------------------- coding for Delete 



    protected void btnpanelexit_Click(object sender, EventArgs e)
    {

    }
    //protected void Delete_Click(object sender, EventArgs e)
    //{
    //    try
    //    {


    //    }
    //    catch
    //    {
    //    }
    //}
    //---------------------------------------- Fpentry Cellclick
    protected void FpEntry_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

        Cellclick = true;
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


    }
    //-----------------------------------------function for group value
    protected void ddlGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            myconn.Close();
            myconn.Open();
            FpReport.Sheets[0].RowCount = 0;
            FpSettings.Sheets[0].ColumnCount = 5;
            string sqlStr = "";
            string semester = "";
            string degreecode = "";
            string batch = "";
            string subno = "";
            string sections = "";
            string activerow = "";
            string activecol = "";
            string strsec = "";
            int rerowcnt = 0;
            activerow = FpEntry.ActiveSheetView.ActiveRow.ToString();
            activecol = FpEntry.ActiveSheetView.ActiveColumn.ToString();
            int ar;
            int ac;
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());

            rerowcnt = ar;
            if (ar != -1)
            {
                semester = FpEntry.Sheets[0].GetText(rerowcnt, 3);
                degreecode = FpEntry.Sheets[0].Cells[rerowcnt, 2].Tag.ToString();
                batch = FpEntry.Sheets[0].GetText(rerowcnt, 1);
                subno = FpEntry.Sheets[0].Cells[rerowcnt, 5].Tag.ToString();

                if (FpEntry.Sheets[0].GetText(rerowcnt, 4) != null)
                {
                    sections = FpEntry.Sheets[0].GetText(rerowcnt, 4);
                }
                if (sections.ToString() == "All" || sections.ToString() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and sections='" + sections.ToString() + "'";
                }
                //--------------------------------------------based on the grpcode to display the test name
                if (Session["Staff_Code"].ToString() != "")
                {
                    if (chkGrp.Checked = true && ddlGrp.SelectedValue != "")
                    {
                        sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + " " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " and staff_code= ' " + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "') and groupcode='" + ddlGrp.Text.ToString() + "'";
                    }
                    else
                    {
                        sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + " " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no =' " + subno.ToString() + " 'and batch_year = " + batch.ToString() + " and staff_code= ' " + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";
                    }
                }
                else
                {
                    if (chkGrp.Checked = true && ddlGrp.SelectedValue != "")
                    {
                        sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + " '" + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and groupcode='" + ddlGrp.Text.ToString() + "'";
                    }
                    else
                    {
                        sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + "' " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";
                    }
                }

                readcon.Close();
                readcon.Open();
                string criteria = "";
                SqlCommand chkgrpcmd = new SqlCommand(sqlStr, readcon);

                SqlDataReader chkgrpdrr;
                chkgrpdrr = chkgrpcmd.ExecuteReader();
                FpSettings.SaveChanges();

                while (chkgrpdrr.Read())
                {
                    int rowcnt = 0;
                    FpReport.Sheets[0].RowCount += 1;
                    rowcnt = Convert.ToInt32(FpReport.Sheets[0].RowCount) - 1;
                    string display = "";
                    string criteria_no = "";

                    string max_mark = "";
                    string min_mark = "";
                    string bind = "";
                    bind = "";
                    bind = subno + "-" + batch + "-" + sections + "-" + degreecode + "-" + semester;
                    //criteria_no = FpReport.Sheets[0].Cells[rowcnt, 1].Tag.ToString();
                    criteria_no = chkgrpdrr[1].ToString();
                    criteria = chkgrpdrr[0].ToString();
                    max_mark = chkgrpdrr[2].ToString();
                    min_mark = chkgrpdrr[3].ToString();

                    FpReport.Sheets[0].Cells[rowcnt, 1].Tag = criteria_no.ToString();
                    FpReport.Sheets[0].Cells[rowcnt, 1].Note = bind.ToString();
                    FpReport.Sheets[0].Cells[rowcnt, 1].Text = criteria.ToString();
                    FpReport.Sheets[0].Cells[rowcnt, 5].Text = max_mark.ToString();
                    FpReport.Sheets[0].Cells[rowcnt, 5].Note = max_mark.ToString();
                    FpReport.Sheets[0].Cells[rowcnt, 6].Text = min_mark.ToString();
                    FpReport.Sheets[0].Cells[rowcnt, 6].Note = min_mark.ToString();
                    FpReport.Sheets[0].Cells[rowcnt, 0].Value = 0;
                    FpSettings.SaveChanges();
                    string[] splitvals = bind.Split(new char[] { '-' });
                    if ((splitvals[2].ToString() != " ") && (splitvals[2].ToString() != ""))
                    {
                        display = "select * from exam_type where  criteria_no=" + criteria_no + " and subject_no = " + splitvals[0] + " and sections='" + splitvals[2] + "' and batch_year=" + splitvals[1] + "";
                    }
                    else
                    {
                        display = "select * from exam_type where  criteria_no=" + criteria_no + " and subject_no = " + splitvals[0] + " and batch_year=" + splitvals[1] + "";
                    }
                    myconn.Close();
                    myconn.Open();
                    SqlCommand cmd1 = new SqlCommand(display, myconn);
                    SqlDataReader drr;
                    drr = cmd1.ExecuteReader();
                    FpSettings.SaveChanges();
                    while (drr.Read())
                    {
                        if (drr.HasRows == true)
                        {
                            string resexamdate = "";
                            string resentrydate = "";
                            string resmaxmrk = "";
                            string resminmrk = "";
                            string resduration = "";
                            string resnewmaxmrk = "";
                            string resnewminmrk = "";
                            string formatexam = "";
                            string bindnote = "";
                            string rollno = "";

                            //formatexam = drr["exam_date"].ToString();
                            //string[] formatexamsplit = formatexam.Split(new char[] { ' ' });
                            //string[] formatetime = formatexamsplit[0].Split(new char[] { '/' });
                            //string examconcat = formatetime[1] + "/" + formatetime[0] + "/" + formatetime[2];

                            //string formatentry = "";
                            //formatentry = drr["entry_date"].ToString();
                            //string[] formatentrysplit = formatentry.Split(new char[] { ' ' });
                            //string[] formatentrytime = formatentrysplit[0].Split(new char[] { '/' });
                            //string entryconcat = formatentrytime[1] + "/" + formatentrytime[0] + "/" + formatentrytime[2];

                            formatexam = drr["exam_date"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 2].Note = formatexam.ToString();
                            if (formatexam != "")
                            {
                                string[] formatexamsplit = formatexam.Split(new char[] { ' ' });
                                string[] formatetime = formatexamsplit[0].Split(new char[] { '/' });
                                string examconcat = formatetime[1] + "/" + formatetime[0] + "/" + formatetime[2];
                                FpReport.Sheets[0].Cells[rowcnt, 2].Text = Convert.ToInt32(formatetime[1]).ToString();
                                FpReport.Sheets[0].Cells[rowcnt, 3].Text = Convert.ToInt32(formatetime[0]).ToString();
                                FpReport.Sheets[0].Cells[rowcnt, 4].Text = formatetime[2].ToString();
                            }
                            else
                            {
                                string examconcat = "";

                                FpReport.Sheets[0].Cells[rowcnt, 2].Text = "";
                                FpReport.Sheets[0].Cells[rowcnt, 3].Text = "";
                                FpReport.Sheets[0].Cells[rowcnt, 4].Text = "";
                            }
                            string formatentry = "";
                            formatentry = drr["entry_date"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 5].Note = formatentry.ToString();
                            if (formatentry != "")
                            {
                                string[] formatentrysplit = formatentry.Split(new char[] { ' ' });
                                string[] formatentrytime = formatentrysplit[0].Split(new char[] { '/' });
                                string entryconcat = formatentrytime[1] + "/" + formatentrytime[0] + "/" + formatentrytime[2];
                                FpReport.Sheets[0].Cells[rowcnt, 5].Text = Convert.ToInt32(formatentrytime[1]).ToString();
                                FpReport.Sheets[0].Cells[rowcnt, 6].Text = Convert.ToInt32(formatentrytime[0]).ToString();
                                FpReport.Sheets[0].Cells[rowcnt, 7].Text = formatentrytime[2].ToString();
                            }
                            else
                            {
                                string entryconcat = "";
                                FpReport.Sheets[0].Cells[rowcnt, 5].Text = "";
                                FpReport.Sheets[0].Cells[rowcnt, 6].Text = "";
                                FpReport.Sheets[0].Cells[rowcnt, 7].Text = "";
                            }


                            //FpReport.Sheets[0].Cells[rowcnt, 5].Note = drr["max_mark"].ToString();
                            //FpReport.Sheets[0].Cells[rowcnt, 5].Text = drr["max_mark"].ToString();
                            //FpReport.Sheets[0].Cells[rowcnt, 6].Note = drr["min_mark"].ToString();
                            //FpReport.Sheets[0].Cells[rowcnt, 6].Text = drr["min_mark"].ToString();
                            //subno = FpEntry.Sheets[0].Cells[rowcnt, 5].Tag.ToString();
                            //FpReport.Sheets[0].Cells[rowcnt, 2].Text = formatexamsplit[0].ToString();
                            //FpReport.Sheets[0].Cells[rowcnt, 3].Text = formatentrysplit[0].ToString();
                            //FpReport.Sheets[0].Cells[rowcnt, 4].Text = drr["duration"].ToString();


                            FpReport.Sheets[0].Cells[rowcnt, 10].Note = drr["max_mark"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 10].Text = drr["max_mark"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 11].Note = drr["min_mark"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 11].Text = drr["min_mark"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 12].Text = drr["start_period"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 12].Note = drr["start_period"].ToString();

                            FpReport.Sheets[0].Cells[rowcnt, 13].Text = drr["end_period"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 13].Note = drr["end_period"].ToString();

                            subno = FpEntry.Sheets[0].Cells[rerowcnt, 5].Tag.ToString();

                            string duration = "";
                            duration = drr["duration"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 8].Note = duration.ToString();
                            if (duration.ToString().Trim() != "")
                            {
                                string[] splitdur = duration.Split(new char[] { ':' });
                                // FpReport.Sheets[0].Cells[rowcnt, 8].Value = splitdur[0].ToString();
                                FpReport.Sheets[0].SetText(rowcnt, 8, splitdur[0].Trim().ToString());
                                if (splitdur.GetUpperBound(0) == 1)
                                {
                                    if (splitdur[1].ToString() != "")
                                    {
                                        // FpReport.Sheets[0].Cells[rowcnt, 9].Value = splitdur[1].ToString();
                                        FpReport.Sheets[0].SetText(rowcnt, 9, splitdur[1].Trim().ToString());
                                    }
                                }
                            }



                            FpReport.Sheets[0].Cells[rowcnt, 0].Value = 1;
                            resexamdate = drr["exam_date"].ToString();
                            resentrydate = drr["entry_date"].ToString();
                            resmaxmrk = drr["max_mark"].ToString();
                            resminmrk = drr["min_mark"].ToString();
                            resduration = drr["duration"].ToString();
                            resnewmaxmrk = drr["new_maxmark"].ToString();
                            resnewminmrk = drr["new_minmark"].ToString();
                            string exam_code = "";
                            exam_code = drr["exam_code"].ToString();
                            FpReport.Sheets[0].Cells[rowcnt, 0].Tag = exam_code.ToString();

                            bindnote = bind + ";" + resexamdate + "-" + resentrydate + "-" + resduration + "-" + resnewmaxmrk + "-" + resmaxmrk + "-" + resnewminmrk + "-" + resminmrk;

                            FpSettings.Sheets[0].ColumnCount += 1;
                            FpSettings.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1].Tag = criteria_no.ToString();
                            FpSettings.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1].Note = bindnote.ToString();
                            FpSettings.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1].Text = criteria.ToString();
                            for (int res = 0; res <= Convert.ToInt16(FpSettings.Sheets[0].RowCount) - 1; res++)
                            {
                                int colco = 0;
                                colco = Convert.ToInt16(FpSettings.Sheets[0].ColumnCount) - 1;

                                for (int col = 5; col <= colco; col++)
                                {
                                    rollno = FpSettings.Sheets[0].Cells[res, 0].Text;
                                    string resultmark = "";
                                    resultmark = "select * from Result where roll_no='" + rollno + "'and exam_code = '" + exam_code + "'";
                                    markcon.Close();
                                    markcon.Open();
                                    SqlCommand command1 = new SqlCommand(resultmark, markcon);
                                    SqlDataReader resreader;
                                    resreader = command1.ExecuteReader();
                                    while (resreader.Read())
                                    {
                                        if (resreader.HasRows == true)
                                        {
                                            FpSettings.Sheets[0].Cells[res, colco].Text = resreader["marks_obtained"].ToString();
                                        }
                                    }
                                    string chkmark = "";
                                    chkmark = FpSettings.Sheets[0].Cells[res, colco].Text;
                                    if (Convert.ToString(chkmark) == "-1")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "AAA";
                                    }
                                    if (Convert.ToString(chkmark) == "-2")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "EL";
                                    }
                                    if (Convert.ToString(chkmark) == "-3")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "EOD";
                                    }
                                    if (Convert.ToString(chkmark) == "")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "0";
                                    }
                                    if (Convert.ToString(chkmark) == "-4")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "ML";
                                    }
                                    if (Convert.ToString(chkmark) == "-5")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "SOD";
                                    }
                                    if (Convert.ToString(chkmark) == "-6")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "NSS";
                                    }
                                    if (Convert.ToString(chkmark) == "-16")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "OD";
                                    }
                                    if (Convert.ToString(chkmark) == "-7")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "NJ";
                                    }
                                    if (Convert.ToString(chkmark) == "-8")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "S";
                                    }
                                    if (Convert.ToString(chkmark) == "-9")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "L";
                                    }
                                    if (Convert.ToString(chkmark) == "-10")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "NCC";
                                    }
                                    if (Convert.ToString(chkmark) == "-11")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "HS";
                                    }
                                    if (Convert.ToString(chkmark) == "-12")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "PP";
                                    }
                                    if (Convert.ToString(chkmark) == "-13")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "SYOD";
                                    }
                                    if (Convert.ToString(chkmark) == "-14")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "COD";
                                    }
                                    if (Convert.ToString(chkmark) == "-15")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "OOD";
                                    }
                                    if (Convert.ToString(chkmark) == "-17")
                                    {
                                        FpSettings.Sheets[0].Cells[res, colco].Text = "LA";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;


        }
        catch
        {
        }
    }

    protected void FpSpread3cmd(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {



        int a = (FpSettings.Sheets[0].RowCount) - 2;
        int tt = Convert.ToInt32(Session["maximummark"]);
        tt = tt * a;
        FpSettings.Sheets[0].Cells[FpSettings.Sheets[0].RowCount - 1, 4].Formula = "(SUM(E1:E" + a + ")/" + tt + ")*100 ";









    }
    protected void FpSettings_EditChanged(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    protected void FpSettings_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

        if (Cellclick == true)
        {

            pHeaderEntry.Visible = true;
            pHeaderReport.Visible = true;

            Exit1.Visible = false;



            Cellclick = false;
        }
    }








    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pHeaderEntry.Visible = false;
        pHeaderReport.Visible = false;

        FpEntry.Visible = false;
        FpReport.Visible = false;
        FpSettings.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;

        Save1.Visible = false;
        printfp.Visible = false;



        lblErrorMsg.Visible = false;
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            //ddlSemYr.Items.Clear();
            // Get_Semester();
            bindsem();
        }
        //ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        //ddlSec.SelectedIndex = -1;
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        // Button1.Visible = true;
    }
    //protected void FpSettings_EditChange(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    MessageBox.Show("e.View.Sheets(0).Cells(e.Row, e.Column).Value.ToString()");
    //}
    protected void Exit1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }
    protected void printfp_Click(object sender, EventArgs e)
    {

        string vehical_details = "";

        //string tt1 = ddlintimehh.SelectedValue.ToString() + ": " + ddlintimemm.SelectedValue.ToString() + ": " + ddlintimeses.SelectedValue.ToString();
        //string tt2 = ddlouttimehh.SelectedValue.ToString() + ": " + ddlouttimemm.SelectedValue.ToString() + ": " + ddlouttimeses.SelectedValue.ToString();

        //vehical_details = "Vehicle History Report" + '@' + "Time :" + tt1.ToString() + " To  " + tt2.ToString() + '@' + "Date :" + tbstart_date.Text.ToString() + " To " + tbend_date.Text.ToString();
        vehical_details = "Cam Planed Mark";

        //}
        //else
        //{
        //vehical_details = "Vehicle History Report";
        //}
        string pagename = "Cam_Planed_Mark.aspx";
        Printcontrol.loadspreaddetails(FpSettings, pagename, vehical_details);
        Printcontrol.Visible = true;

        //pHeaderEntry.Visible = true;
        //pHeaderReport.Visible = true;
        //// pHeaderSettings.Visible = true;
        //lblErrorMsg.Visible = false;
        //lblnorec.Text = "";
        //FpSettings.Visible = true;
        //Save1.Visible = true;
        //printfp.Visible = true;
        //lblrptname.Visible = true;
        //btnExcel.Visible = true;
        //txtexcelname.Visible = true;

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {

        try
        {
            //Modified by Srinath 27/2/2013
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                lblexcelerror.Text = "";
                lblexcelerror.Visible = false;

                da.printexcelreport(FpSettings, reportname);
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
        pHeaderEntry.Visible = true;
        pHeaderReport.Visible = true;


        //Buttontotal.Visible = true;


        Exit1.Visible = false;


    }
    protected void chkmarkattendance_CheckedChanged(object sender, EventArgs e)
    {
        FpReport.Visible = false;
        FpSettings.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;

        pHeaderReport.Visible = false;

        if (FpEntry.Visible == true)
        {
            pHeaderEntry.Visible = true;
        }
    }

}


