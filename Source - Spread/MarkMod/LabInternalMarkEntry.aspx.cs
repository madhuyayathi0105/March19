using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Collections;

public partial class MarkMod_LabInternalMarkEntry : System.Web.UI.Page
{
    #region variables Declaration

    SqlConnection funconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    static string usercode = string.Empty;
    static string collegeCode = string.Empty;
    bool isBasedOnBatchRights = false;
    static bool isSchool = false;
    bool Cellclick;
    string datelocksetting = string.Empty;
    public bool day_check;
    static string selectedSubTest = "";
    static string newMaxMinMark = "";
    Hashtable hat = new Hashtable();
    #endregion

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
            if (!IsPostBack)
            {
                collegeCode = Convert.ToString(Session["collegecode"]).Trim();
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
                    usercode = Session["group_code"].ToString();
                }
                else
                {
                    grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                    usercode = Convert.ToString(Session["usercode"]).Trim();
                }


                isBasedOnBatchRights = false;
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string batchYearSettings = da.GetFunction("select value from Master_Settings where settings='CAM Entry Based On Batch And Section Rights' and " + grouporusercode + "");
                    if (batchYearSettings.Trim() == "1")
                        isBasedOnBatchRights = true;
                }

                Session["StaffSelector"] = "0";
                string check_Stu_Staff_selector = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'");
                if (check_Stu_Staff_selector.Trim() == "1")
                {
                    Session["StaffSelector"] = "1";
                }

                if (Convert.ToString(Session["staff_code"]).Trim() != "")
                {
                    spreadSubDetails.Visible = true;
                }
                else
                {
                    spreadSubDetails.Visible = false;

                }

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Admisionflag"] = "0";
                Session["Appflag"] = "0";

                string masterQry = string.Empty;
                masterQry = "select * from Master_Settings where " + grouporusercode + "";
                DataSet dsMasterSetting = da.select_method_wo_parameter(masterQry, "Text");
                if (dsMasterSetting.Tables.Count > 0 && dsMasterSetting.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsMasterSetting.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Roll No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Register No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Student_Type" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Admission No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Admisionflag"] = "1";
                        }
                        if (Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["settings"]) == "Application No" && Convert.ToString(dsMasterSetting.Tables[0].Rows[i]["value"]) == "1")
                        {
                            Session["Appflag"] = "1";
                        }
                    }
                }

                loadSubjectDetails();
                testDetailsLblDiv.Visible = false;
                spreadTestDetails.Visible = false;
                Div1.Visible = false;
                divMarkEntry.Visible = false;
                btnok.Visible = false;
                btnSave.Visible = false;
                btnDelete.Visible = false;
                lblNote.Visible = false;

            }
        }
        catch { }
    }

    #region Subject Details
    private void loadSubjectDetails()
    {
        try
        {
            #region spreadSubDetails design

            spreadSubDetails.ActiveSheetView.AutoPostBack = true;
            spreadSubDetails.Sheets[0].ColumnCount = 7;
            spreadSubDetails.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Name = "Book Antiqua";
            style.HorizontalAlign = HorizontalAlign.Center;
            style.ForeColor = Color.Black;
            style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            spreadSubDetails.Sheets[0].ColumnHeader.DefaultStyle = style;
            spreadSubDetails.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            spreadSubDetails.Sheets[0].AllowTableCorner = true;
            spreadSubDetails.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";


            FarPoint.Web.Spread.TextCellType objlabel = new FarPoint.Web.Spread.TextCellType();

            spreadSubDetails.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            if (isSchool == true)
            {
                spreadSubDetails.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year";
            }
            spreadSubDetails.Sheets[0].Columns[1].CellType = objlabel;

            spreadSubDetails.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
            spreadSubDetails.Sheets[0].Columns[2].CellType = objlabel;

            spreadSubDetails.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Semester";
            if (isSchool == true)
            {
                spreadSubDetails.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Term";
            }
            spreadSubDetails.Sheets[0].Columns[3].CellType = objlabel;

            spreadSubDetails.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Section";
            spreadSubDetails.Sheets[0].Columns[4].CellType = objlabel;

            spreadSubDetails.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject";

            spreadSubDetails.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject Code";
            spreadSubDetails.Sheets[0].Columns[6].CellType = objlabel;

            spreadSubDetails.Sheets[0].ColumnHeader.Columns[0].Visible = false;
            //------------------------------------------------set width of each column in spreadSubDetails
            spreadSubDetails.Sheets[0].Columns[1].Width = 80;
            spreadSubDetails.Sheets[0].Columns[2].Width = 50;
            spreadSubDetails.Sheets[0].Columns[3].Width = 50;
            spreadSubDetails.Sheets[0].Columns[4].Width = 50;
            spreadSubDetails.Sheets[0].Columns[5].Width = 200;
            //----------------------------------------------- set style of each column in spreadSubDetails
            spreadSubDetails.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            spreadSubDetails.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            spreadSubDetails.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            spreadSubDetails.Sheets[0].SheetCorner.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            spreadSubDetails.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadSubDetails.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadSubDetails.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
            spreadSubDetails.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            spreadSubDetails.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            spreadSubDetails.Sheets[0].DefaultStyle.Font.Bold = false;
            spreadSubDetails.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            spreadSubDetails.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            spreadSubDetails.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            spreadSubDetails.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            spreadSubDetails.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            spreadSubDetails.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            spreadSubDetails.Sheets[0].Columns[0].Font.Underline = false;
            spreadSubDetails.Sheets[0].Columns[1].Font.Underline = false;
            spreadSubDetails.Sheets[0].Columns[2].Font.Underline = false;
            spreadSubDetails.Sheets[0].Columns[3].Font.Underline = false;
            spreadSubDetails.Sheets[0].Columns[5].Font.Underline = true;
            spreadSubDetails.Sheets[0].Columns[6].Font.Underline = false;
            spreadSubDetails.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
            spreadSubDetails.Sheets[0].Columns[0].ForeColor = Color.Black;
            spreadSubDetails.Sheets[0].Columns[1].ForeColor = Color.Black;
            spreadSubDetails.Sheets[0].Columns[2].ForeColor = Color.Black;
            spreadSubDetails.Sheets[0].Columns[3].ForeColor = Color.Black;
            spreadSubDetails.Sheets[0].Columns[4].ForeColor = Color.Black;
            spreadSubDetails.Sheets[0].Columns[5].ForeColor = Color.Blue;
            spreadSubDetails.Sheets[0].Columns[6].ForeColor = Color.Black;
            #endregion

            string staff_code = string.Empty;
            staff_code = Convert.ToString(Session["staff_code"]).Trim();


            if (staff_code != "")
            {
                string userId = grouporusercode;
                string subDetailsQry = string.Empty;

                string qryBatchBasedSetting = string.Empty;
                if (isBasedOnBatchRights)
                {
                    qryBatchBasedSetting = " and r.Batch_Year in(select Batch_Year from tbl_attendance_rights where user_id='" + Convert.ToString(userId).Trim() + "')";
                }

                subDetailsQry = "select distinct s.subject_no,s.subject_name,s.subject_code,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (LTRIM(RTRIM(ISNULL(st.sections,''))) =LTRIM(RTRIM(ISNULL(r.sections,''))) or LTRIM(RTRIM(ISNULL(st.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections, '')))) and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and st.staff_code='" + Session["staff_code"].ToString() + "' " + qryBatchBasedSetting + " and lab=1 order by st.batch_year,sy.degree_code,semester,st.sections ";

                if (Session["StaffSelector"].ToString() == "1")
                {
                    if (staff_code != null)
                    {
                        if (staff_code.ToString().Trim() != "" && staff_code.ToString().Trim() != "0")
                        {
                            subDetailsQry = "select distinct s.subject_no,s.subject_name,s.subject_code,s.syll_code,st.batch_year,sy.semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,sub_sem sb,subjectChooser sc where sb.subtype_no=s.subtype_no and sb.promote_count=1 and r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and (LTRIM(RTRIM(ISNULL(st.sections,''))) =LTRIM(RTRIM(ISNULL(r.sections,''))) or LTRIM(RTRIM(ISNULL(st.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections, '')))) and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar'  and st.staff_code = '" + Convert.ToString(staff_code) + "' and sc.staffcode like '%" + Convert.ToString(staff_code) + "%' and  sc.roll_no=sc.roll_no and sc.subject_no=st.subject_no and sy.semester=sc.semester and sb.subType_no=sc.subtype_no and s.subject_no=sc.subject_no and lab=1 order by st.batch_year,sy.degree_code,sy.semester,st.sections ";
                        }
                    }
                }

                DataSet dsSubDetails = da.select_method_wo_parameter(subDetailsQry, "Text");


                spreadSubDetails.Sheets[0].RowCount = 0;
                if (dsSubDetails.Tables.Count > 0 && dsSubDetails.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsSubDetails.Tables[0].Rows.Count; i++)
                    {
                        int rowCnt = 0;
                        string current_sem = string.Empty;
                        current_sem = GetFunction("select distinct current_semester from registration where degree_code='" + dsSubDetails.Tables[0].Rows[i]["degree_code"].ToString() + "' and batch_year='" + dsSubDetails.Tables[0].Rows[i]["batch_year"].ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar'");
                        if (Convert.ToString(current_sem) == Convert.ToString(dsSubDetails.Tables[0].Rows[i]["semester"]))
                        {
                            spreadSubDetails.Sheets[0].RowCount += 1;
                            if ((Convert.ToString(Session["collegecode"]) != "") && Convert.ToString(dsSubDetails.Tables[0].Rows[i]["degree_code"]) != "")
                            {
                                string sqlstr = string.Empty;
                                sqlstr = "select course_name + '-'+dept_acronym from degree d,course c,department dp where d.course_id=c.course_id and d.dept_code=dp.dept_code and degree_code= '" + Convert.ToString(dsSubDetails.Tables[0].Rows[i]["degree_code"]) + "'";
                                string degree = string.Empty;
                                degree = GetFunction(sqlstr.ToString());
                                rowCnt = Convert.ToInt32(spreadSubDetails.Sheets[0].RowCount) - 1;
                                spreadSubDetails.Sheets[0].Cells[rowCnt, 0].Text = rowCnt.ToString();
                                spreadSubDetails.Sheets[0].Cells[rowCnt, 1].Text = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["batch_year"]);
                                spreadSubDetails.Sheets[0].Cells[rowCnt, 2].Tag = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["degree_code"]);
                                spreadSubDetails.Sheets[0].Cells[rowCnt, 2].Text = degree.ToString();
                                if (Convert.ToString(dsSubDetails.Tables[0].Rows[i]["semester"]) == "-1")
                                {
                                    spreadSubDetails.Sheets[0].Cells[rowCnt, 3].Text = " ";
                                }
                                else
                                {
                                    spreadSubDetails.Sheets[0].Cells[rowCnt, 3].Text = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["semester"]);
                                }
                                if (Convert.ToString(dsSubDetails.Tables[0].Rows[i]["sections"]) == "-1")
                                {
                                    spreadSubDetails.Sheets[0].Cells[rowCnt, 4].Text = " ";
                                }
                                else
                                {
                                    spreadSubDetails.Sheets[0].Cells[rowCnt, 4].Text = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["sections"]);
                                }
                                if (spreadSubDetails.Sheets[0].Cells[rowCnt, 4].Text == "-1")
                                {
                                    spreadSubDetails.Sheets[0].Cells[rowCnt, 4].Text = " ";
                                }
                                spreadSubDetails.Sheets[0].Cells[rowCnt, 5].Tag = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["subject_no"]);
                                spreadSubDetails.Sheets[0].Cells[rowCnt, 5].Text = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["subject_name"]);
                                spreadSubDetails.Sheets[0].Cells[rowCnt, 6].Text = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["subject_code"]);

                            }

                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void spreadSubDetails_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Cellclick = true;
    }
    protected void spreadSubDetails_SelectedIndexChanged(Object sender, EventArgs e)
    {

        loadTestDetails();
    }
    #endregion

    #region Test Details

    public void loadTestDetails()
    {
        try
        {

            spreadTestDetails.SaveChanges();
            string staff_code = Convert.ToString(Session["staff_code"]).Trim();
            if (Cellclick == true)
            {
                string datelock = GetFunction("select value from master_settings where settings='Cam Date Lock' and " + grouporusercode + "");
                if (datelock.Trim() != "")
                {
                    datelocksetting = datelock;
                }
                else
                {
                    datelocksetting = "0";
                }

                string activerow = string.Empty;
                string activecol = string.Empty;
                activerow = spreadSubDetails.ActiveSheetView.ActiveRow.ToString();
                activecol = spreadSubDetails.ActiveSheetView.ActiveColumn.ToString();

                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());

                if (ar != -1)
                {
                    spreadTestDetails.Sheets[0].ColumnCount = 12;
                    int rowcnt = 0;
                    string sqlStr = string.Empty;
                    string batch = string.Empty;
                    string degreeCode = string.Empty;
                    string semester = string.Empty;
                    string section = string.Empty;
                    string subno = string.Empty;
                    string strsec = string.Empty;

                    string[] arrayMonth;
                    string[] arrayDate;
                    string[] arrayHour;
                    string[] arrayMin;

                    arrayMonth = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
                    arrayDate = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                    arrayHour = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };
                    arrayMin = new string[] { "00", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55", "60" };

                    string batchYearQry = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''order by batch_year";
                    DataSet dsBatchYear = da.select_method_wo_parameter(batchYearQry, "Text");

                    int batchYearsCount = 0;
                    int.TryParse(Convert.ToString(dsBatchYear.Tables[0].Rows.Count), out batchYearsCount);
                    string[] arrayBatchyear = new string[batchYearsCount + 1];

                    if (dsBatchYear.Tables.Count > 0 && dsBatchYear.Tables[0].Rows.Count > 0)
                    {
                        int r = 0;
                        for (r = 0; r < dsBatchYear.Tables[0].Rows.Count; r++)
                        {
                            arrayBatchyear[r] = Convert.ToString(dsBatchYear.Tables[0].Rows[r]["batch_year"]);
                        }
                        int curr_year = Convert.ToInt16(DateTime.Today.Year);

                        if (arrayBatchyear.Contains(Convert.ToString(curr_year)) != true)
                        {
                            arrayBatchyear[r] = Convert.ToString(curr_year);
                        }

                    }

                    #region  spreadTestDetails design
                    spreadTestDetails.Sheets[0].SheetCorner.RowCount = 2;
                    spreadTestDetails.Sheets[0].ColumnCount = 12;
                    spreadTestDetails.CommandBar.Visible = false;
                    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                    style.Font.Size = 10;
                    style.Font.Bold = true;
                    style.Font.Name = "Book Antiqua";
                    style.HorizontalAlign = HorizontalAlign.Center;
                    style.ForeColor = Color.Black;
                    style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    spreadTestDetails.Sheets[0].ColumnHeader.DefaultStyle = style;
                    spreadTestDetails.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    spreadTestDetails.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    spreadTestDetails.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                    spreadTestDetails.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);

                    spreadTestDetails.Columns[1].Locked = true;
                    spreadTestDetails.Sheets[0].Columns[10].Locked = true;
                    spreadTestDetails.Sheets[0].Columns[11].Locked = true;


                    spreadTestDetails.Sheets[0].Columns[0].Width = 60;
                    spreadTestDetails.Sheets[0].Columns[1].Width = 150;
                    spreadTestDetails.Sheets[0].Columns[2].Width = 50;
                    spreadTestDetails.Sheets[0].Columns[3].Width = 50;
                    spreadTestDetails.Sheets[0].Columns[4].Width = 50;
                    spreadTestDetails.Sheets[0].Columns[5].Width = 50;
                    spreadTestDetails.Sheets[0].Columns[6].Width = 50;
                    spreadTestDetails.Sheets[0].Columns[7].Width = 50;
                    spreadTestDetails.Sheets[0].Columns[8].Width = 50;
                    spreadTestDetails.Sheets[0].Columns[9].Width = 50;
                    spreadTestDetails.Sheets[0].Columns[10].Width = 80;
                    spreadTestDetails.Sheets[0].Columns[11].Width = 80;

                    spreadTestDetails.Sheets[0].Columns[0].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[1].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[2].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[3].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[4].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[5].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[6].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[7].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[8].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[9].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[10].ForeColor = Color.Black;
                    spreadTestDetails.Sheets[0].Columns[11].ForeColor = Color.Black;

                    FarPoint.Web.Spread.TextCellType lblcell = new FarPoint.Web.Spread.TextCellType();

                    spreadTestDetails.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                    spreadTestDetails.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
                    spreadTestDetails.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
                    spreadTestDetails.SheetCorner.Cells[0, 0].Font.Bold = true;
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Test";
                    spreadTestDetails.Sheets[0].Columns[1].CellType = lblcell;
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Exam Date";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Month";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Entry Date";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Date";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Month";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Duration";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Max Mark";
                    spreadTestDetails.Sheets[0].Columns[10].CellType = lblcell;
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Min Mark";
                    spreadTestDetails.Sheets[0].Columns[11].CellType = lblcell;

                    spreadTestDetails.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    spreadTestDetails.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    spreadTestDetails.Sheets[0].DefaultStyle.Font.Bold = false;

                    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                    spreadTestDetails.Sheets[0].Columns[0].CellType = chkcell;

                    FarPoint.Web.Spread.ComboBoxCellType cmbDate = new FarPoint.Web.Spread.ComboBoxCellType(arrayDate);
                    spreadTestDetails.Sheets[0].Columns[2].CellType = cmbDate;
                    FarPoint.Web.Spread.ComboBoxCellType cmbMonth = new FarPoint.Web.Spread.ComboBoxCellType(arrayMonth);
                    spreadTestDetails.Sheets[0].Columns[3].CellType = cmbMonth;
                    FarPoint.Web.Spread.ComboBoxCellType cmbYear = new FarPoint.Web.Spread.ComboBoxCellType(arrayBatchyear);
                    spreadTestDetails.Sheets[0].Columns[4].CellType = cmbYear;
                    FarPoint.Web.Spread.ComboBoxCellType cmbDate1 = new FarPoint.Web.Spread.ComboBoxCellType(arrayDate);
                    spreadTestDetails.Sheets[0].Columns[5].CellType = cmbDate1;
                    FarPoint.Web.Spread.ComboBoxCellType cmbMonth1 = new FarPoint.Web.Spread.ComboBoxCellType(arrayMonth);
                    spreadTestDetails.Sheets[0].Columns[6].CellType = cmbMonth1;
                    FarPoint.Web.Spread.ComboBoxCellType cmbYear1 = new FarPoint.Web.Spread.ComboBoxCellType(arrayBatchyear);
                    spreadTestDetails.Sheets[0].Columns[7].CellType = cmbYear1;
                    FarPoint.Web.Spread.ComboBoxCellType cmbHour = new FarPoint.Web.Spread.ComboBoxCellType(arrayHour);
                    spreadTestDetails.Sheets[0].Columns[8].CellType = cmbHour;
                    FarPoint.Web.Spread.ComboBoxCellType cmbMin = new FarPoint.Web.Spread.ComboBoxCellType(arrayMin);
                    spreadTestDetails.Sheets[0].Columns[9].CellType = cmbMin;

                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);
                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 3);
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Date";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Month";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Year";
                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 3);
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Date";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Month";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Year";
                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 2);
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Hrs";
                    spreadTestDetails.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Min";
                    spreadTestDetails.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 1, 2);

                    spreadTestDetails.Sheets[0].RowCount = 0;
                    #endregion

                    batch = spreadSubDetails.Sheets[0].Cells[ar, 1].Text.Trim();
                    degreeCode = spreadSubDetails.Sheets[0].Cells[ar, 2].Tag.ToString();
                    semester = spreadSubDetails.Sheets[0].Cells[ar, 3].Text.Trim();
                    section = spreadSubDetails.Sheets[0].Cells[ar, 4].Text.Trim();
                    subno = spreadSubDetails.Sheets[0].Cells[ar, 5].Tag.ToString();

                    lblTestName.InnerText = "Test Details - " + spreadSubDetails.Sheets[0].Cells[ar, 6].Text.ToString() + " - " + spreadSubDetails.Sheets[0].Cells[ar, 5].Text.ToString() + " ";
                    if (section.Trim().ToLower() == "all" || section.Trim() == "" || section == "-1" || section == null)
                    {
                        strsec = string.Empty;
                    }
                    else
                    {
                        strsec = " and sections='" + section + "'";
                    }

                    string bind = string.Empty;
                    bind = subno + "-" + batch + "-" + section + "-" + degreeCode + "-" + semester;

                    //------------------------------------------- Query for display the Testname,max,min marks,date and duration in the spread2-spreadTestDetails
                    if (staff_code != "")
                    {

                        sqlStr = "select criteria,criteria_no,max_mark,min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no=" + subno.ToString() + " " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code = (select top 1 staff_code  from staff_selector where subject_no =' " + subno.ToString() + " 'and batch_year = " + batch.ToString() + " and staff_code= ' " + Session["Staff_Code"].ToString() + "' " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";

                    }
                    else
                    {
                        sqlStr = "select CriteriaForInternal.criteria,CriteriaForInternal.criteria_no,CriteriaForInternal.max_mark,CriteriaForInternal.min_mark, isnull((select distinct '1' from exam_type where criteria_no=CriteriaForInternal.criteria_no and subject_no='" + subno.ToString() + "' " + strsec.ToString() + " and batch_year=" + batch.ToString() + " and staff_code in (select top 1 staff_code  from staff_selector where subject_no = '" + subno.ToString() + "' and batch_year = " + batch.ToString() + " " + strsec.ToString() + ")),'0') as returnVal  from CriteriaForInternal where syll_code=(select syll_code from subject where subject_no='" + subno.ToString() + "')  and (groupcode is null or groupcode='')";
                    }
                    DataSet dsExamDetails = da.select_method_wo_parameter(sqlStr, "text");

                    string examAllDetailsQry = "select * from exam_type where subject_no='" + subno.ToString() + "' " + strsec.ToString() + "";
                    DataSet dsExamAllDetails = da.select_method_wo_parameter(examAllDetailsQry, "text");
                    if (dsExamDetails.Tables.Count > 0 && dsExamDetails.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsExamDetails.Tables[0].Rows.Count; i++)
                        {
                            spreadTestDetails.Sheets[0].RowCount += 1;
                            rowcnt = Convert.ToInt32(spreadTestDetails.Sheets[0].RowCount) - 1;
                            string display = string.Empty;
                            string criteria_no = string.Empty;
                            string criteria = string.Empty;
                            float max_mark = 0;
                            float min_mark = 0;

                            spreadTestDetails.Sheets[0].Cells[rowcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 1].HorizontalAlign = HorizontalAlign.Left;
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 2].HorizontalAlign = HorizontalAlign.Center;
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 4].HorizontalAlign = HorizontalAlign.Center;
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 5].HorizontalAlign = HorizontalAlign.Center;
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 6].HorizontalAlign = HorizontalAlign.Center;

                            criteria = Convert.ToString(dsExamDetails.Tables[0].Rows[i]["criteria"]);
                            criteria_no = Convert.ToString(dsExamDetails.Tables[0].Rows[i]["criteria_no"]);

                            dsExamAllDetails.Tables[0].DefaultView.RowFilter = " criteria_no='" + criteria_no + "'";
                            DataView dvexm = dsExamAllDetails.Tables[0].DefaultView;

                            if (dvexm.Count > 0)
                            {
                                max_mark = Convert.ToSingle(dvexm[0]["max_mark"].ToString());
                                min_mark = Convert.ToSingle(dvexm[0]["min_mark"].ToString());
                            }
                            else
                            {
                                max_mark = Convert.ToSingle(dsExamDetails.Tables[0].Rows[i]["max_mark"]);
                                min_mark = Convert.ToSingle(dsExamDetails.Tables[0].Rows[i]["min_mark"]);
                            }

                            spreadTestDetails.Sheets[0].Cells[rowcnt, 1].Tag = criteria_no.ToString();
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 1].Note = bind.ToString();
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 1].Text = criteria.ToString();
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 10].Text = max_mark.ToString();
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 10].Note = max_mark.ToString();
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 11].Text = min_mark.ToString();
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 11].Note = min_mark.ToString();
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 0].Value = 0;
                            spreadTestDetails.Sheets[0].Cells[rowcnt, 0].Tag = "NE";


                            if (datelocksetting.Trim() == "1")
                            {
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 5].Locked = true;
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 6].Locked = true;
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 7].Locked = true;
                            }

                            int temp = Convert.ToInt32(criteria_no);
                            day_check = daycheck(temp);
                            if (Session["Staff_Code"].ToString().Trim() != "")
                            {
                                if (day_check == false)
                                {
                                    spreadTestDetails.Sheets[0].Rows[rowcnt].Locked = true;
                                }
                                else
                                {
                                    spreadTestDetails.Sheets[0].Rows[rowcnt].Locked = false;
                                }
                            }
                            spreadTestDetails.Sheets[0].Columns[10].Locked = true;
                            spreadTestDetails.Sheets[0].Columns[11].Locked = true;

                            if (dvexm.Count > 0)
                            {
                                string resExamDate = string.Empty;
                                string resEntryDate = string.Empty;
                                string resMaxMrk = string.Empty;
                                string resMinMrk = string.Empty;
                                string resDuration = string.Empty;
                                string resNewMaxmMrk = string.Empty;
                                string resNewMinMrk = string.Empty;

                                string bindnote = string.Empty;

                                string examDate = string.Empty;
                                string srtprd = string.Empty;
                                string endprd = string.Empty;
                                bind = string.Empty;
                                bind = subno + "-" + batch + "-" + section + "-" + degreeCode + "-" + semester;
                                examDate = dvexm[0]["exam_date"].ToString();
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 2].Note = examDate.ToString();
                                if (examDate != "")
                                {
                                    string[] examDateSplit = examDate.Split(new char[] { ' ' });
                                    string[] formatetime = examDateSplit[0].Split(new char[] { '/' });
                                    string examconcat = formatetime[1] + "/" + formatetime[0] + "/" + formatetime[2];
                                    if (formatetime[1].Length == 1)
                                    {
                                        formatetime[1] = "0" + formatetime[1];
                                    }
                                    if (formatetime[0].Length == 1)
                                    {
                                        formatetime[0] = "0" + formatetime[0];
                                    }
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 2].Text = formatetime[1].ToString().Trim().PadLeft(2, '0');
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 3].Text = formatetime[0].ToString().Trim().PadLeft(2, '0');
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 4].Text = formatetime[2].ToString();
                                }
                                else
                                {
                                    string examconcat = string.Empty;
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 2].Text = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 3].Text = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 4].Text = DateTime.Now.Year.ToString();
                                }
                                string entryDate = string.Empty;
                                entryDate = dvexm[0]["entry_date"].ToString();
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 5].Note = entryDate.ToString();
                                if (entryDate != "")
                                {
                                    string[] entryDateSplit = entryDate.Split(new char[] { ' ' });
                                    string[] formatentrytime = entryDateSplit[0].Split(new char[] { '/' });
                                    string entryconcat = formatentrytime[1] + "/" + formatentrytime[0] + "/" + formatentrytime[2];
                                    if (formatentrytime[1].Length == 1)
                                    {
                                        formatentrytime[1] = "0" + formatentrytime[1];
                                    }
                                    if (formatentrytime[0].Length == 1)
                                    {
                                        formatentrytime[0] = "0" + formatentrytime[0];
                                    }

                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 5].Text = formatentrytime[1].ToString().Trim().PadLeft(2, '0');
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 6].Text = formatentrytime[0].ToString().Trim().PadLeft(2, '0');
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 7].Text = formatentrytime[2].ToString();
                                    if (datelocksetting == "1")
                                    {
                                        spreadTestDetails.Sheets[0].Cells[rowcnt, 5].Locked = true;
                                        spreadTestDetails.Sheets[0].Cells[rowcnt, 6].Locked = true;
                                        spreadTestDetails.Sheets[0].Cells[rowcnt, 7].Locked = true;
                                    }
                                }
                                else
                                {
                                    string entryconcat = string.Empty;
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 5].Text = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 6].Text = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 7].Text = DateTime.Now.Year.ToString();
                                    if (datelocksetting == "1")
                                    {
                                        spreadTestDetails.Sheets[0].Cells[rowcnt, 5].Locked = true;
                                        spreadTestDetails.Sheets[0].Cells[rowcnt, 6].Locked = true;
                                        spreadTestDetails.Sheets[0].Cells[rowcnt, 7].Locked = true;
                                    }
                                }
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 10].Note = dvexm[0]["max_mark"].ToString();
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 10].Text = dvexm[0]["max_mark"].ToString();
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 11].Note = dvexm[0]["min_mark"].ToString();
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 11].Text = dvexm[0]["min_mark"].ToString();
                                //spreadTestDetails.Sheets[0].Cells[rowcnt, 12].Text = dvexm[0]["start_period"].ToString();
                                //spreadTestDetails.Sheets[0].Cells[rowcnt, 12].Note = dvexm[0]["start_period"].ToString();
                                //spreadTestDetails.Sheets[0].Cells[rowcnt, 13].Text = dvexm[0]["end_period"].ToString();
                                //spreadTestDetails.Sheets[0].Cells[rowcnt, 13].Note = dvexm[0]["end_period"].ToString();
                                subno = spreadSubDetails.Sheets[0].Cells[ar, 5].Tag.ToString();
                                string duration = string.Empty;
                                string examDurationNew = Convert.ToString(dvexm[0]["durationNew"]).Trim();
                                string examDuration = Convert.ToString(dvexm[0]["duration"]).Trim();
                                TimeSpan tsDuration = new TimeSpan(0, 0, 0);
                                duration = Convert.ToString(dvexm[0]["duration"]).Trim();
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 8].Note = duration.ToString();
                                if (duration.ToString().Trim() != "")
                                {
                                    string[] splitdur = duration.Split(new char[] { ':' });

                                    spreadTestDetails.Sheets[0].SetText(rowcnt, 8, splitdur[0].Trim().ToString());
                                    if (splitdur.GetUpperBound(0) == 1)
                                    {
                                        if (splitdur[1].ToString() != "")
                                        {

                                            spreadTestDetails.Sheets[0].SetText(rowcnt, 9, splitdur[1].Trim().ToString());
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
                                resExamDate = dvexm[0]["exam_date"].ToString();
                                resEntryDate = dvexm[0]["entry_date"].ToString();
                                resDuration = Convert.ToString(dvexm[0]["duration"]).Trim();
                                resMaxMrk = dvexm[0]["max_mark"].ToString();
                                resMinMrk = dvexm[0]["min_mark"].ToString();
                                resNewMaxmMrk = dvexm[0]["new_maxmark"].ToString();
                                resNewMinMrk = dvexm[0]["new_minmark"].ToString();

                                string newduartion = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
                                spreadTestDetails.Sheets[0].SetText(rowcnt, 8, hour.ToString().Trim().PadLeft(2, '0'));
                                spreadTestDetails.Sheets[0].SetText(rowcnt, 9, min.ToString().Trim().PadLeft(2, '0'));
                                string exam_code = string.Empty;
                                exam_code = dvexm[0]["exam_code"].ToString();
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 0].Tag = exam_code;

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
                                                    spreadTestDetails.Sheets[0].Rows[rowcnt].Locked = true;
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
                            }
                            else
                            {
                                string examconcat = string.Empty;
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 2].Text = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 3].Text = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 4].Text = DateTime.Now.Year.ToString();
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 5].Text = DateTime.Now.Day.ToString().Trim().PadLeft(2, '0');
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 6].Text = DateTime.Now.Month.ToString().Trim().PadLeft(2, '0');
                                spreadTestDetails.Sheets[0].Cells[rowcnt, 7].Text = DateTime.Now.Year.ToString();
                                if (datelocksetting == "1")
                                {
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 5].Locked = true;
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 6].Locked = true;
                                    spreadTestDetails.Sheets[0].Cells[rowcnt, 7].Locked = true;
                                }
                            }
                        }
                        spreadTestDetails.SaveChanges();
                    }
                    else
                    {
                        spreadTestDetails.Visible = false;
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "No Test Conducted For The Subject";
                    }
                }
                Cellclick = false;
                testDetailsLblDiv.Visible = true;
                spreadTestDetails.Visible = true;
                btnok.Visible = true;
            }
        }
        catch { }
    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        try
        {
            spreadTestDetails.SaveChanges();

            int val = 0;
            int check = 0;
            for (int row = 0; row < Convert.ToInt16(spreadTestDetails.Sheets[0].RowCount); row++)
            {
                val = Convert.ToInt32(spreadTestDetails.Sheets[0].GetValue(row, 0).ToString());
                if (val == 1)
                {
                    check++;
                    if (check == 1)
                    {
                        int ar;
                        ar = Convert.ToInt32(spreadSubDetails.ActiveSheetView.ActiveRow.ToString());
                        string testName = Convert.ToString(spreadTestDetails.Cells[row, 1].Text);
                        lblMarkEntry.InnerText = "Mark Entry - " + testName;
                        string testNo = Convert.ToString(spreadTestDetails.Cells[row, 1].Tag);
                        string subNo = Convert.ToString(spreadSubDetails.Cells[ar, 5].Tag);
                        selectedSubTest = subNo + "#" + testNo;

                        string date = string.Empty;
                        string month = string.Empty;
                        string year = string.Empty;
                        string examDate = "";
                        string date1 = string.Empty;
                        string month1 = string.Empty;
                        string year1 = string.Empty;
                        string entryDate = "";
                        string hours = string.Empty;
                        string minutes = string.Empty;
                        string duration = "";
                        float new_max_mark = 0;
                        float new_min_mark = 0;

                        date = spreadTestDetails.Sheets[0].Cells[row, 2].Text.Trim().PadLeft(2, '0');
                        month = spreadTestDetails.Sheets[0].Cells[row, 3].Text.Trim().PadLeft(2, '0');
                        year = spreadTestDetails.Sheets[0].Cells[row, 4].Text.Trim();
                        if ((date != "") && (month != "") && (year != "") && (date != null) && (month != null) && (year != null))
                        {
                            examDate = month + "/" + date + "/" + year;
                            spreadTestDetails.Sheets[0].Cells[row, 2].Note = examDate.ToString();
                        }
                        else
                        {
                            examDate = string.Empty;
                        }
                        date1 = spreadTestDetails.Sheets[0].Cells[row, 5].Text.Trim().PadLeft(2, '0');
                        month1 = spreadTestDetails.Sheets[0].Cells[row, 6].Text.Trim().PadLeft(2, '0');
                        year1 = spreadTestDetails.Sheets[0].Cells[row, 7].Text.Trim();
                        if ((date1 != "") && (month1 != "") && (year1 != "") && (date1 != null) && (month1 != null) && (year1 != null))
                        {
                            entryDate = month1 + "/" + date1 + "/" + year1;
                            spreadTestDetails.Sheets[0].Cells[row, 5].Note = entryDate.ToString();
                        }
                        else
                        {
                            entryDate = DateTime.Now.ToString("MM/dd/yyyy");
                        }

                        if ((hours == "") && (minutes == ""))
                        {
                            duration = "00:00:00";
                        }
                        hours = spreadTestDetails.Sheets[0].Cells[row, 8].Text.Trim().Trim().PadLeft(2, '0');
                        minutes = spreadTestDetails.Sheets[0].Cells[row, 9].Text.Trim().Trim().PadLeft(2, '0');
                        if ((minutes != null) && (hours != null) && (hours != "") && (minutes != ""))
                        {
                            duration = hours.Trim().PadLeft(2, '0') + ":" + minutes.Trim().PadLeft(2, '0') + ":00";
                            spreadTestDetails.Sheets[0].Cells[row, 8].Note = duration.ToString();
                        }
                        else
                        {
                            duration = "00:00:00";
                        }

                        new_max_mark = Convert.ToSingle(spreadTestDetails.Sheets[0].Cells[row, 10].Text.ToString());
                        new_min_mark = Convert.ToSingle(spreadTestDetails.Sheets[0].Cells[row, 11].Text.ToString());

                        newMaxMinMark = Convert.ToString(new_max_mark) + "," + Convert.ToString(new_min_mark);

                        loadMarkEntrySpread(row);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select single Test!')", true);
                        Div1.Visible = false;
                        divMarkEntry.Visible = false;
                        spreadMarkEntry.Visible = false;
                        btnSave.Visible = false;
                        lblNote.Visible = false;
                        break;
                    }
                }
            }
            if (check == 1)
            {
                Div1.Visible = true;
                divMarkEntry.Visible = true;
                spreadMarkEntry.Visible = true;
                lblNote.Visible = true;
                lblErrorMsg.Visible = false;
            }
            else if (check == 0)
            {
                Div1.Visible = false;
                divMarkEntry.Visible = false;
                btnSave.Visible = false;
                spreadMarkEntry.Visible = false;
                lblNote.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Select a Test!')", true);
            }
        }
        catch { }
    }
    #endregion

    public void loadMarkEntrySpread(int activeRow)
    {
        try
        {
            #region Mark Entry Spread design
            spreadMarkEntry.Sheets[0].ColumnHeader.RowCount = 2;
            spreadMarkEntry.Sheets[0].ColumnCount = 7;
            spreadMarkEntry.Sheets[0].RowCount = 0;
            spreadMarkEntry.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Name = "Book Antiqua";
            style.HorizontalAlign = HorizontalAlign.Center;
            style.ForeColor = Color.Black;
            style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            spreadMarkEntry.Sheets[0].ColumnHeader.DefaultStyle = style;
            spreadMarkEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            spreadMarkEntry.Sheets[0].AllowTableCorner = true;
            spreadMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            spreadMarkEntry.Sheets[0].SheetCornerSpanModel.Add(0, 0, 2, 1);
            spreadMarkEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Roll No.";
            spreadMarkEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            spreadMarkEntry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No.";
            spreadMarkEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            spreadMarkEntry.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
            spreadMarkEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            spreadMarkEntry.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Application No.";
            spreadMarkEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            spreadMarkEntry.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Day to Day Evaluation";
            spreadMarkEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 2);
            spreadMarkEntry.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Record Mark(10)";
            spreadMarkEntry.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Observation Mark(5)";
            spreadMarkEntry.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Internal Mark(10)";
            spreadMarkEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

            spreadMarkEntry.Sheets[0].Columns[0].Width = 120;
            spreadMarkEntry.Sheets[0].Columns[1].Width = 120;
            spreadMarkEntry.Sheets[0].Columns[2].Width = 200;
            spreadMarkEntry.Sheets[0].Columns[3].Width = 130;
            spreadMarkEntry.Sheets[0].Columns[4].Width = 120;
            spreadMarkEntry.Sheets[0].Columns[5].Width = 180;
            spreadMarkEntry.Sheets[0].Columns[6].Width = 130;

            spreadMarkEntry.Columns[0].Locked = true;
            spreadMarkEntry.Columns[1].Locked = true;
            spreadMarkEntry.Columns[2].Locked = true;
            spreadMarkEntry.Columns[3].Locked = true;


            #endregion

            FarPoint.Web.Spread.DoubleCellType intgrCellType = new FarPoint.Web.Spread.DoubleCellType();
            FarPoint.Web.Spread.DoubleCellType intgrCellType1 = new FarPoint.Web.Spread.DoubleCellType();
            FarPoint.Web.Spread.DoubleCellType intgrCellType2 = new FarPoint.Web.Spread.DoubleCellType();

            intgrCellType.MaximumValue = Convert.ToInt32("10");
            intgrCellType.MinimumValue = -1;
            intgrCellType.ErrorMessage = "Please enter the mark between -1 and 10 ";
            intgrCellType1.MaximumValue = Convert.ToInt32("5");
            intgrCellType1.MinimumValue = -1;
            intgrCellType1.ErrorMessage = "Please enter the mark between -1 and 5 ";
            intgrCellType2.MaximumValue = Convert.ToInt32("10");
            intgrCellType2.MinimumValue = -1;
            intgrCellType2.ErrorMessage = "Please enter the mark between -1 and 10 ";

            spreadMarkEntry.Sheets[0].Columns[4].CellType = intgrCellType;
            spreadMarkEntry.Sheets[0].Columns[5].CellType = intgrCellType1;
            spreadMarkEntry.Sheets[0].Columns[6].CellType = intgrCellType2;

            if (Session["Rollflag"].ToString() == "0" || Session["Rollflag"].ToString() == "1")
            {
                spreadMarkEntry.Sheets[0].ColumnHeader.Columns[0].Visible = true;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                spreadMarkEntry.Sheets[0].ColumnHeader.Columns[1].Visible = false;
            }
            if (Session["Studflag"].ToString() == "0")
            {
                spreadMarkEntry.Sheets[0].ColumnHeader.Columns[3].Visible = false;
            }
            if (Session["Admisionflag"].ToString() == "0")
            {
                spreadMarkEntry.Sheets[0].ColumnHeader.Columns[1].Visible = false;
            }
            if (Session["Appflag"].ToString() == "0")
            {
                spreadMarkEntry.Sheets[0].ColumnHeader.Columns[3].Visible = false;
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

            string activerow = string.Empty;
            string activecol = string.Empty;
            activerow = spreadSubDetails.ActiveSheetView.ActiveRow.ToString();
            activecol = spreadSubDetails.ActiveSheetView.ActiveColumn.ToString();

            int ar;
            int ac;
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());

            string batch = spreadSubDetails.Sheets[0].Cells[ar, 1].Text.Trim();
            string degreeCode = spreadSubDetails.Sheets[0].Cells[ar, 2].Tag.ToString();
            string semester = spreadSubDetails.Sheets[0].Cells[ar, 3].Text.Trim();
            string section = spreadSubDetails.Sheets[0].Cells[ar, 4].Text.Trim();
            string subno = spreadSubDetails.Sheets[0].Cells[ar, 5].Tag.ToString();
            string strsec = "";
            if (section.Trim().ToLower() == "all" || section.Trim() == "" || section == "-1" || section == null)
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + section + "'";
            }


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
                    if (Convert.ToInt32(batch) >= batchyearsetting)
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

            string sqlStr = "Select distinct len(registration.roll_no),registration.roll_no as RollNumber,registration.reg_no as RegistrationNumber,registration.app_no as app_no,registration.stud_name as Student_Name,registration.Stud_Type as StudentType,ap.app_formno as ApplicationNumber,registration.college_code from registration ,SubjectChooser,applyn ap where registration.App_No=ap.app_no and registration.roll_no = subjectchooser.roll_no and registration.Degree_Code ='" + Convert.ToString(degreeCode) + "' and Semester = '" + Convert.ToString(semester) + "' and registration.Batch_Year = '" + Convert.ToString(batch) + "' and Subject_No = '" + Convert.ToString(subno) + "' " + strsec + " and RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and Semester = '" + Convert.ToString(semester) + "'   " + strstaffselecotr + " " + strorderby + "";

            DataSet ds = da.select_method_wo_parameter(sqlStr, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                    {
                        spreadMarkEntry.Sheets[0].RowCount++;
                        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                        spreadMarkEntry.Sheets[0].Cells[irow, 1].CellType = tt;
                        spreadMarkEntry.Sheets[0].Cells[irow, 0].CellType = tt;
                        spreadMarkEntry.Sheets[0].Cells[irow, 0].Text = ds.Tables[0].Rows[irow]["RollNumber"].ToString();
                        spreadMarkEntry.Sheets[0].Cells[irow, 0].Note = ds.Tables[0].Rows[irow]["app_no"].ToString();
                        spreadMarkEntry.Sheets[0].Cells[irow, 0].HorizontalAlign = HorizontalAlign.Center;
                        spreadMarkEntry.Sheets[0].Cells[irow, 1].Text = ds.Tables[0].Rows[irow]["RegistrationNumber"].ToString();
                        spreadMarkEntry.Sheets[0].Cells[irow, 1].HorizontalAlign = HorizontalAlign.Center;
                        spreadMarkEntry.Sheets[0].Cells[irow, 2].Text = ds.Tables[0].Rows[irow]["Student_Name"].ToString();
                        spreadMarkEntry.Sheets[0].Cells[irow, 2].HorizontalAlign = HorizontalAlign.Left;
                        //spreadMarkEntry.Sheets[0].Cells[irow, 3].Text = ds.Tables[0].Rows[irow]["StudentType"].ToString();
                        spreadMarkEntry.Sheets[0].Cells[irow, 3].Text = ds.Tables[0].Rows[irow]["ApplicationNumber"].ToString();
                    }
                }
            }

            btnSave.Visible = true;
            btnSave.Text = "Save";
            btnSave.Width = 54;
            btnDelete.Visible = false;


            string examCode = Convert.ToString(spreadTestDetails.Sheets[0].Cells[activeRow, 0].Tag);

            for (int i = 0; i < spreadMarkEntry.Rows.Count; i++)
            {
                string rollNo = Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 0].Text);
                string appNo = Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 0].Note);
                string criteriaNo = Convert.ToString(spreadTestDetails.Sheets[0].Cells[activeRow, 1].Tag);
                string resultmark = "select marks_obtained from Result where roll_no='" + rollNo + "'and exam_code = '" + examCode + "'";
                string chkmark = da.GetFunctionv(resultmark);

                if (chkmark.Trim() != "0" && chkmark.Trim() != "" && chkmark.Trim() != null)
                {
                    string qry = "select * from InternalMarkEntry where subject_no='" + subno + "'  and criteria_no='" + criteriaNo + "' and app_no='" + appNo + "'";
                    DataSet dsMarks = da.select_method_wo_parameter(qry, "Text");

                    if (dsMarks.Tables.Count > 0 && dsMarks.Tables[0].Rows.Count > 0)
                    {
                        spreadMarkEntry.Sheets[0].Cells[i, 4].Text = Convert.ToString(dsMarks.Tables[0].Rows[0]["RecordMark"]);
                        spreadMarkEntry.Sheets[0].Cells[i, 5].Text = Convert.ToString(dsMarks.Tables[0].Rows[0]["ObservationMark"]);
                        spreadMarkEntry.Sheets[0].Cells[i, 6].Text = Convert.ToString(dsMarks.Tables[0].Rows[0]["LabInternal"]);

                        btnSave.Visible = true;
                        btnSave.Text = "Update";
                        btnSave.Width = 74;
                        btnDelete.Visible = true;
                    }
                }
            }


            spreadMarkEntry.Sheets[0].PageSize = spreadMarkEntry.Sheets[0].RowCount;
            spreadMarkEntry.SaveChanges();
        }
        catch { }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            spreadMarkEntry.SaveChanges();
            float recordMark = 0;
            float observationMark = 0;
            float internalMark = 0;

            string subNo = "";
            string criteriaNo = "";
            subjecttest();
            string[] arr = selectedSubTest.Split('#');
            subNo = Convert.ToString(arr[0]);
            criteriaNo = Convert.ToString(arr[1]);

            string staffCode = Convert.ToString(Session["staff_code"]).Trim();

            for (int i = 0; i < spreadMarkEntry.Rows.Count; i++)
            {
                float grandTotal = 0;
                string strRecordMark = Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 4].Text).Trim();
                string strObservationMark = Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 5].Text).Trim();
                string strInternalMark = Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 6].Text).Trim();

                if (!string.IsNullOrEmpty(strRecordMark) || !string.IsNullOrEmpty(strObservationMark) || !string.IsNullOrEmpty(strInternalMark))
                {
                    string rollNo = Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 0].Text).Trim();
                    string appNo = Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 0].Note);
                    float.TryParse(Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 4].Text).Trim(), out recordMark);
                    float.TryParse(Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 5].Text).Trim(), out observationMark);
                    float.TryParse(Convert.ToString(spreadMarkEntry.Sheets[0].Cells[i, 6].Text).Trim(), out internalMark);

                    float temp = 0;
                    string qry = "";

                    if (!string.IsNullOrEmpty(strRecordMark))
                    {
                        qry = "if exists(select * from InternalMarkEntry  where app_no='" + appNo + "' and subject_no='" + subNo + "' and Criteria_No='" + criteriaNo + "')update InternalMarkEntry set RecordMark='" + recordMark + "' where app_no='" + appNo + "' and subject_no='" + subNo + "' and Criteria_No='" + criteriaNo + "' else insert into InternalMarkEntry (app_no,subject_no,Criteria_No,RecordMark)values('" + appNo + "','" + subNo + "','" + criteriaNo + "','" + recordMark + "')";
                        int qryStatus = da.update_method_wo_parameter(qry, "Text");

                        if (recordMark != -1)
                            temp = recordMark;
                    }
                    if (!string.IsNullOrEmpty(strObservationMark))
                    {
                        qry = "if exists(select * from InternalMarkEntry  where app_no='" + appNo + "' and subject_no='" + subNo + "' and Criteria_No='" + criteriaNo + "')update InternalMarkEntry set ObservationMark='" + observationMark + "' where app_no='" + appNo + "' and subject_no='" + subNo + "' and Criteria_No='" + criteriaNo + "' else insert into InternalMarkEntry (app_no,subject_no,Criteria_No,ObservationMark)values('" + appNo + "','" + subNo + "','" + criteriaNo + "','" + observationMark + "')";
                        int qryStatus = da.update_method_wo_parameter(qry, "Text");

                        if (observationMark != -1)
                            temp = temp + observationMark;
                    }
                    if (!string.IsNullOrEmpty(strInternalMark))
                    {
                        qry = "if exists(select * from InternalMarkEntry  where app_no='" + appNo + "' and subject_no='" + subNo + "' and Criteria_No='" + criteriaNo + "')update InternalMarkEntry set LabInternal='" + internalMark + "' where app_no='" + appNo + "' and subject_no='" + subNo + "' and Criteria_No='" + criteriaNo + "' else insert into InternalMarkEntry (app_no,subject_no,Criteria_No,LabInternal)values('" + appNo + "','" + subNo + "','" + criteriaNo + "','" + internalMark + "')";
                        int qryStatus = da.update_method_wo_parameter(qry, "Text");

                        if (internalMark != -1)
                            temp = temp + internalMark;
                    }

                    if (recordMark == -1 && observationMark == -1 && internalMark == -1)
                        grandTotal = -1;
                    else
                        grandTotal = temp;
                    int ar;
                    ar = Convert.ToInt32(spreadTestDetails.ActiveSheetView.ActiveRow.ToString());
                    string[] arrBindNote = Convert.ToString(spreadTestDetails.Sheets[0].Cells[ar, 1].Note).Split('-');
                    string batchYear = arrBindNote[1];
                    string sec = arrBindNote[2];
                    string exam_date = Convert.ToString(spreadTestDetails.Sheets[0].Cells[ar, 2].Note);
                    string entry_date = Convert.ToString(spreadTestDetails.Sheets[0].Cells[ar, 5].Note);
                    string duration = Convert.ToString(spreadTestDetails.Sheets[0].Cells[ar, 8].Note);
                    string max_mark = Convert.ToString(spreadTestDetails.Sheets[0].Cells[ar, 10].Note);
                    string min_mark = Convert.ToString(spreadTestDetails.Sheets[0].Cells[ar, 11].Note);
                    string[] arrNewMark = newMaxMinMark.Split(',');
                    float newMaxMark = 0;
                    float newMinMark = 0;
                    float.TryParse(arrNewMark[0], out newMaxMark);
                    float.TryParse(arrNewMark[1], out newMinMark);
                    string startPeriod = "";
                    string endPeriod = "";

                    hat.Clear();
                    hat.Add("criteria_no", criteriaNo);
                    hat.Add("staff_code", staffCode);
                    hat.Add("subject_no", subNo);
                    hat.Add("duration", duration);
                    hat.Add("entry_date", entry_date);
                    hat.Add("exam_date", exam_date);
                    hat.Add("batch_year", batchYear);
                    hat.Add("max_mark", max_mark);
                    hat.Add("min_mark", min_mark);
                    hat.Add("sections", sec);
                    hat.Add("new_maxmark", newMaxMark);
                    hat.Add("new_minmark", newMinMark);
                    hat.Add("start_period", startPeriod);
                    hat.Add("end_period", endPeriod);
                    int insert = da.insert_method("sp_ins_upd_cam_exam_type_dead", hat, "sp");

                    if (insert == 1)
                    {
                        string exam_code = da.GetFunction("select exam_code from exam_type where subject_no='" + subNo + "' and sections='" + sec + "' and criteria_no='" + criteriaNo + "'");
                        hat.Clear();
                        hat.Add("roll_no", rollNo);
                        hat.Add("exam_code", exam_code);
                        hat.Add("marks_obtained", Math.Round(grandTotal));
                        insert = da.insert_method("sp_ins_upd_cam_mark_dead", hat, "sp");
                    }
                }
            }
            if (btnSave.Text == "Save")
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            else
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
            spreadMarkEntry.Visible = false;
            divMarkEntry.Visible = false;
            Div1.Visible = false;
            btnSave.Visible = false;
            btnDelete.Visible = false;
            lblNote.Visible = false;
            lblErrorMsg.Visible = false;
        }
        catch { }

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = string.Empty;
            string activecol = string.Empty;
            activerow = spreadTestDetails.ActiveSheetView.ActiveRow.ToString();
            activecol = spreadTestDetails.ActiveSheetView.ActiveColumn.ToString();

            int ar;
            int ac;
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());
            string examCode = Convert.ToString(spreadTestDetails.Sheets[0].Cells[ar, 0].Tag);
            string sqldel = "delete from result where exam_code=" + examCode + " ";
            int delStatus = da.update_method_wo_parameter(sqldel, "Text");
            subjecttest();
            string[] arr = selectedSubTest.Split('#');
            string subNo = Convert.ToString(arr[0]);
            string criteriaNo = Convert.ToString(arr[1]);

            sqldel = "delete from InternalMarkEntry  where subject_no='" + subNo + "'  and criteria_no='" + criteriaNo + "'";
            delStatus = da.update_method_wo_parameter(sqldel, "Text");

            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            spreadMarkEntry.Visible = false;
            divMarkEntry.Visible = false;
            Div1.Visible = false;
            btnSave.Visible = false;
            btnDelete.Visible = false;
            lblNote.Visible = false;
        }
        catch { }
    }

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

    public bool checkSchoolOrCollege()
    {
        try
        {
            DataSet schoolds = new DataSet();
            string schoolvalue = GetFunction("select value from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "");

            if (schoolvalue.Trim() == "0")
            {
                isSchool = true;
            }
            else
            {
                isSchool = false;
            }

            return isSchool;
        }
        catch
        {
            return false;
        }
    }

    public bool daycheck(int CriteriaNo)
    {
        bool daycheck = false;
        string curdate, Dateval;
        string[] ddate = new string[100];
        curdate = DateTime.Today.ToString();
        string qry = "select Clock,LastDate from CriteriaforInternal where Criteria_no='" + CriteriaNo + "' and Clock = '1' ";
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

    public void subjecttest()
    {
        try
        {
            spreadTestDetails.SaveChanges();
            int val = 0;
            int check = 0;
            for (int row = 0; row < Convert.ToInt16(spreadTestDetails.Sheets[0].RowCount); row++)
            {
                val = Convert.ToInt32(spreadTestDetails.Sheets[0].GetValue(row, 0).ToString());
                if (val == 1)
                {
                    check++;
                    if (check == 1)
                    {
                        int ar;
                        ar = Convert.ToInt32(spreadSubDetails.ActiveSheetView.ActiveRow.ToString());
                        string testName = Convert.ToString(spreadTestDetails.Cells[row, 1].Text);
                        lblMarkEntry.InnerText = "Mark Entry - " + testName;
                        string testNo = Convert.ToString(spreadTestDetails.Cells[row, 1].Tag);
                        string subNo = Convert.ToString(spreadSubDetails.Cells[ar, 5].Tag);
                        selectedSubTest = subNo + "#" + testNo;
                    }

                }
            }
        }
        catch
        {
        }
    }


}