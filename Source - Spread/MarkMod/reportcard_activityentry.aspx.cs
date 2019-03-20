using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;
using FarPoint.Web.Spread;
using System.IO;
using System.Configuration;


public partial class reportcard_activityentry : System.Web.UI.Page
{
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
    string term = string.Empty;
    string grade_ids = string.Empty;
    string activity_ids = string.Empty;
    FpSpread fpspreadsample;
    DataSet ds = new DataSet();
    static Boolean forschoolsetting = false;
    DAccess2 dacc = new DAccess2();
    Hashtable hat = new Hashtable();
    Boolean cellclick = false;
    static ArrayList arr = new ArrayList();
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
    FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
    DAccess2 da = new DAccess2();

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
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                lblErrorMsg.Visible = false;
                lblErrorMsg.Text = string.Empty;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                txtexcelname.Text = string.Empty;



                //DataSet schoolds = new DataSet();
                //  string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                //  schoolds.Clear();
                //  schoolds.Dispose();
                //  schoolds = dacc.select_method_wo_parameter(sqlschool, "Text");
                //  if (schoolds.Tables[0].Rows.Count > 0)
                //  {
                //      string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                //      if (schoolvalue.Trim() == "0")
                //      {
                forschoolsetting = true;
                lblBatch.Text = "Year";
                lblDegree.Text = "School Type";
                lblBranch.Text = "Standard";
                lblSemYr.Text = "Term";
                //    }
                //}
                BindBatch();
                BindDegree();
                if (ddlDegree.Items.Count > 0)
                {
                    bindbranch();
                    bindsem();

                    BindSectionDetail();
                    lblErrorMsg.Text = string.Empty;
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

                FarPoint.Web.Spread.StyleInfo darkstyle1 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle1.BackColor = ColorTranslator.FromHtml("#00aff0");
                //darkstyle.ForeColor = System.Drawing.Color.Black;
                darkstyle1.Font.Name = "Book Antiqua";
                darkstyle1.Font.Size = FontUnit.Medium;
                darkstyle1.Border.BorderSize = 0;
                darkstyle1.Border.BorderColor = System.Drawing.Color.Transparent;
                fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle1;

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
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#add8e6");
                //darkstyle.ForeColor = System.Drawing.Color.White;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 90;
                fpspread.Sheets[0].ColumnHeader.Columns[2].Width = 120;
                //fpspread.Sheets[0].ColumnHeader.Columns[3].Width = 150;
                fpspread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.White;
                bindactivity();
                hideexportimport();
            }
            if (ddlSemYr.Items.Count > 0)
            {
                term = ddlSemYr.SelectedItem.Text.ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;

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
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
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

            FpSpread1.Visible = false;
            btnok.Visible = false;
            hideexportimport();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
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
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            fpspread.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;

            FpSpread1.Visible = false;
            btnok.Visible = false;
            hideexportimport();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }

    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            BindSectionDetail();
            lblErrorMsg.Visible = false;
            fpspread.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;

            lblErrorMsg.Visible = false;
            fpspread.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;

            FpSpread1.Visible = false;
            btnok.Visible = false;
            hideexportimport();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void FpSpread1_OnButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 0].Value) == 1)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    FpSpread1.Sheets[0].Cells[i, 0].Value = 1;
                    FpSpread1.Visible = true;
                }
            }
            else if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 0].Value) == 0)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    FpSpread1.Sheets[0].Cells[i, 0].Value = 0;
                    FpSpread1.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void btnok_Click1(object sender, EventArgs e)
    {
        try
        {

            int cnt = 0;
            fpmarkexcel.Visible = true;
            btn_import.Visible = true;
            FpSpread1.SaveChanges();
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
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
            fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Height = 400;
            fpspread.Width = 800;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Border.BorderSize = 0;
            darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
            fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            DataSet dsmark = new DataSet();
            DataSet min_max = new DataSet();

            for (int res = 1; res < Convert.ToInt32(FpSpread1.Sheets[0].RowCount); res++)
            {
                int isval = 0;

                isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 0].Value);

                if (isval == 1)
                {
                    cnt++;
                    fpspread.Sheets[0].ColumnCount++;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = FpSpread1.Sheets[0].Cells[res, 1].Text;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note = FpSpread1.Sheets[0].Cells[res, 1].Note;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                    fpspread.Sheets[0].Columns[0].Locked = true;
                    fpspread.Sheets[0].Columns[1].Locked = true;
                    fpspread.Sheets[0].Columns[2].Locked = true;
                    fpspread.Visible = false;

                }
            }
            string secsql = string.Empty;
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
                    secsql = string.Empty;
                }
                else
                {
                    secsql = "and r.Sections in ('" + fpsec + "')";

                }
            }
            if (chk_att.Checked == true)
            {
                intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
                intgrcel.MaximumValue = 365;
                intgrcel.MinimumValue = 0;
                intgrcel.ErrorMessage = "Enter valid Attendance Days";
            }
            else
            {
                intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            }
            fpspread.SaveChanges();
            string sql;
            Boolean serialflag;
            string strorderby = da.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");

            if (strorderby == "1")
            {
                serialflag = true;
            }
            else
            {
                serialflag = false;
            }
            strorderby = da.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = string.Empty;
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
            string sqlquery = string.Empty;

            if (serialflag == false)
            {

                // sql = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,serialno FROM Registration R,Applyn A WHERE R.App_No = A.App_No     " + sqlcondition + " and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + strorderby + "";
                sqlquery = "select r.roll_no,r.reg_no, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,Sections from registration r, applyn a where a.app_no=r.app_no and r.degree_code='" + fpbranch + "'  and r.batch_year='" + fpbatch_year + "' " + secsql + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strorderby + " ";
            }
            else
            {
                //sql = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,serialno FROM Registration R,Applyn A WHERE R.App_No = A.App_No     " + sqlcondition + " and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' ORDER BY serialno";
                sqlquery = "select r.roll_no,r.reg_no, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,Sections from registration r, applyn a where a.app_no=r.app_no and r.degree_code='" + fpbranch + "'  and r.batch_year='" + fpbatch_year + "' " + secsql + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0   ORDER BY r.serialno ";
            }

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
            string Degree_Code = string.Empty;
            string Batch_Year = string.Empty;

            Degree_Code = ddlBranch.SelectedItem.Value.ToString();
            Batch_Year = ddlBatch.SelectedItem.Text.ToString();
            fpspread.SaveChanges();
            for (int i = 3; i < fpspread.Sheets[0].ColumnCount; i++)
            {
                for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                {
                    if (fpspread.Sheets[0].ColumnHeader.Cells[0, i].Note.ToString() == "Att")
                    {
                        fpspread.Sheets[0].Cells[j, i].CellType = intgrcel;
                        string value = Convert.ToString(fpspread.Sheets[0].Cells[j, i].Note);
                        string marksql = "select mark from CoCurrActivitie_Det where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "' and istype ='Att' and term='" + term + "' and Batch_Year='" + Batch_Year + "' and Degree_Code='" + Degree_Code + "'";
                        dsmark.Clear();
                        dsmark = da.select_method_wo_parameter(marksql, "Text");
                        if (dsmark.Tables[0].Rows.Count > 0)
                        {
                            fpspread.Sheets[0].Cells[j, i].Text = dsmark.Tables[0].Rows[0][0].ToString();
                            fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else if (fpspread.Sheets[0].ColumnHeader.Cells[0, i].Note.ToString() == "totatt")
                    {
                        fpspread.Sheets[0].Cells[j, i].CellType = intgrcel;
                        string value = Convert.ToString(fpspread.Sheets[0].Cells[j, i].Note);
                        string marksql = "select totatt_remarks from CoCurrActivitie_Det where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "' and istype ='Att' and term='" + term + "'  and Batch_Year='" + Batch_Year + "' and Degree_Code='" + Degree_Code + "'";
                        dsmark.Clear();
                        dsmark = da.select_method_wo_parameter(marksql, "Text");

                        if (dsmark.Tables[0].Rows.Count > 0)
                        {
                            fpspread.Sheets[0].Cells[j, i].Text = dsmark.Tables[0].Rows[0][0].ToString();
                            fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else if (fpspread.Sheets[0].ColumnHeader.Cells[0, i].Note.ToString() == "Remks")
                    {
                        fpspread.Sheets[0].Columns[i].Width = 250;
                        fpspread.Sheets[0].Cells[j, i].CellType = txtcell;
                        string value = Convert.ToString(fpspread.Sheets[0].Cells[j, i].Note);
                        string marksql = "select totatt_remarks from CoCurrActivitie_Det where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "' and istype ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Note) + "' and term='" + term + "'  and Batch_Year='" + Batch_Year + "' and Degree_Code='" + Degree_Code + "'";
                        dsmark.Clear();
                        dsmark = da.select_method_wo_parameter(marksql, "Text");

                        if (dsmark.Tables[0].Rows.Count > 0)
                        {
                            fpspread.Sheets[0].Cells[j, i].Text = dsmark.Tables[0].Rows[0][0].ToString();
                            fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else
                    {
                        min_max = da.select_method_wo_parameter("select min(frompoint) from activity_gd where Degree_Code='" + Degree_Code + "' and term='" + term + "' and Batch_Year='" + Batch_Year + "' and ActivityTextVal in ('" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Note) + "'); select MAX(topoint) from activity_gd where Degree_Code='" + Degree_Code + "' and term='" + term + "' and Batch_Year='" + Batch_Year + "' and ActivityTextVal in ('" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Note) + "')", "Text");
                        if (min_max.Tables[0].Rows.Count > 0)
                        {
                            if (min_max.Tables[0].Rows[0][0].ToString() != "" && min_max.Tables[0].Rows[0][0].ToString() != null)
                                intgrcel.MinimumValue = Convert.ToDouble(min_max.Tables[0].Rows[0][0].ToString());
                        }
                        else
                        {
                            intgrcel.MinimumValue = 0;
                        }
                        if (min_max.Tables[1].Rows.Count > 0)
                        {
                            if (min_max.Tables[1].Rows[0][0].ToString() != "" && min_max.Tables[1].Rows[0][0].ToString() != null)
                                intgrcel.MaximumValue = Convert.ToDouble(min_max.Tables[1].Rows[0][0].ToString());
                        }
                        else
                        {
                            intgrcel.MaximumValue = 100;
                        }
                        intgrcel.ErrorMessage = "Enter valid mark between " + intgrcel.MinimumValue + " And " + intgrcel.MaximumValue;
                        fpspread.Sheets[0].Cells[j, i].CellType = intgrcel;
                        string marksql = "select mark from CoCurrActivitie_Det where Roll_No='" + Convert.ToString(fpspread.Sheets[0].Cells[j, 1].Text) + "' and ActivityTextVal ='" + Convert.ToString(fpspread.Sheets[0].ColumnHeader.Cells[0, i].Note) + "' and term='" + term + "'  and Batch_Year='" + Batch_Year + "' and Degree_Code='" + Degree_Code + "'";
                        dsmark.Clear();
                        dsmark = da.select_method_wo_parameter(marksql, "Text");
                        if (dsmark.Tables[0].Rows.Count > 0)
                        {
                            fpspread.Sheets[0].Cells[j, i].Text = dsmark.Tables[0].Rows[0][0].ToString();
                            fpspread.Sheets[0].Cells[j, i].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
            if (fpspread.Sheets[0].ColumnCount > 4)
            {
                fpspread.Sheets[0].FrozenColumnCount = 3;
            }

            fpspread.SaveChanges();
            fpspread.Visible = true;
            lblErrorMsg.Visible = false;
            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            btnfpspread1save.Visible = true;
            btnfpspread1delete.Visible = true;
            if (fpspread.Sheets[0].Rows.Count > 0)
            {
                showexportimport();
            }
            else
            {
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                hideexportimport();
            }

            if (cnt == 0)
            {
                lblErrorMsg.Text = "Please Select Atleast One Activity";
                lblErrorMsg.Visible = true;
                fpspread.Visible = false;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                hideexportimport();
            }
            else
            {
                btnfpspread1save.Visible = true;
                btnfpspread1delete.Visible = true;
                showexportimport();
            }
        }
        catch (Exception ex)
        {
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
            hideexportimport();
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            // --------------- add start
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;

            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            fpmarkexcel.Visible = false;
            btn_import.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].ColumnCount = 2;
            FpSpread1.CommandBar.Visible = false;

            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            style2.BackColor = System.Drawing.Color.White;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Activity Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;

            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = chkboxsel_all;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].Columns[1].Width = 320;

            FpSpread1.Sheets[0].Columns[1].Locked = true;
            // --------------- add end


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

            string secsql = string.Empty;
            fpbatch_year = ddlBatch.SelectedItem.Text.ToString();
            fpdegreecode = ddlDegree.SelectedItem.Value.ToString();
            fpbranch = ddlBranch.SelectedItem.Value.ToString();
            fpsem = ddlSemYr.SelectedItem.Text.ToString();

            if (ddlSec.Enabled == true)
            {
                fpsec = ddlSec.SelectedItem.Text.ToString();

                if (fpsec.Trim() != "" || fpsec == "All")
                {
                    secsql = "and r.Sections in ('" + fpsec + "')";

                }
                else
                {
                    secsql = string.Empty;
                }
            }

            Degree_Code = string.Empty;
            Batch_Year = string.Empty;

            Degree_Code = ddlBranch.SelectedItem.Value.ToString();
            Batch_Year = ddlBatch.SelectedItem.Text.ToString();
            term = ddlSemYr.SelectedItem.Text.ToString().Trim();

            if (chk_att.Checked == true)
            {
                chkboxsel_all.AutoPostBack = true;
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Attendance Days";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "Att";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Total Attendance Days";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "totatt";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Remarks";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = "Remks";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Visible = true;
                btnok.Visible = true;
                lblErrorMsg.Visible = false;
                fpspread.Visible = false;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
                return;
            }
            string sqlselect = "select * from  activity_entry where  Degree_Code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' and term='" + term + "'";
            DataSet dsselect = new DataSet();
            dsselect.Clear();
            dsselect = da.select_method_wo_parameter(sqlselect, "Text");

            if (dsselect.Tables[0].Rows.Count > 0)
            {
                string currentsem = ddlSemYr.SelectedItem.Text.ToString();
                string degreecode = ddlBranch.SelectedItem.Value.ToString();
                string batchyear = ddlBatch.SelectedItem.Text.ToString();
                string strtit_acitivity = string.Empty;

                for (int ij = 0; ij < dsselect.Tables[0].Rows.Count; ij++)
                {
                    if (strtit_acitivity == "")
                    {
                        strtit_acitivity = dsselect.Tables[0].Rows[ij][1].ToString();
                    }
                    else
                    {
                        strtit_acitivity = strtit_acitivity + "','" + dsselect.Tables[0].Rows[ij][1].ToString();
                    }
                }

                string queryactivity = " select * from textvaltable where TextCriteria='RActv' and college_code='" + Session["collegecode"].ToString() + "' and TextCode in ('" + strtit_acitivity + "') ";
                DataSet newact = new DataSet();
                newact.Clear();
                newact = da.select_method_wo_parameter(queryactivity, "Text");
                chkboxsel_all.AutoPostBack = true;

                if (newact.Tables[0].Rows.Count > 0)
                {
                    for (int dsi = 0; dsi < newact.Tables[0].Rows.Count; dsi++)
                    {
                        string acname = newact.Tables[0].Rows[dsi]["TextVal"].ToString();
                        string accode = newact.Tables[0].Rows[dsi]["TextCode"].ToString();
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = acname;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = accode;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Visible = true;
                        btnok.Visible = true;
                        lblErrorMsg.Visible = false;
                        fpspread.Visible = false;
                        btnfpspread1save.Visible = false;
                        btnfpspread1delete.Visible = false;
                    }

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
                }

                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
            }
            else
            {
                ddlactivity.Visible = false;

                lblErrorMsg.Text = "No Activity Yet Created";
                lblErrorMsg.Visible = true;
                FpSpread1.Visible = false;
                btnok.Visible = false;

                fpspread.Visible = false;
                btnfpspread1save.Visible = false;
                btnfpspread1delete.Visible = false;
            }


            //intgrcel.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            //intgrcel.MaximumValue = 100;
            //intgrcel.MinimumValue = 0;
            //intgrcel.ErrorMessage = "Enter valid mark";

            //if (ddlactivity.Visible == true)
            //{
            //    string sqlquery = "select Registration.roll_no,Registration.reg_no, Registration.stud_name,Registration.stud_type,registration.serialno,Registration.Adm_Date,Sections from registration, applyn a where a.app_no=registration.app_no and registration.degree_code='" + fpbranch + "'  and registration.batch_year='" + fpbatch_year + "' " + secsql + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0   ORDER BY Registration.Roll_No  ";
            //    DataSet studentdetails = new DataSet();
            //    studentdetails.Clear();
            //    studentdetails = dacc.select_method_wo_parameter(sqlquery, "Text");

            //    if (studentdetails.Tables[0].Rows.Count > 0)
            //    {
            //        fpspread.Sheets[0].RowCount = studentdetails.Tables[0].Rows.Count;

            //        for (int i = 0; i < studentdetails.Tables[0].Rows.Count; i++)
            //        {
            //            fpspread.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
            //            fpspread.Sheets[0].Cells[i, 1].Text = studentdetails.Tables[0].Rows[i]["roll_no"].ToString();
            //            fpspread.Sheets[0].Cells[i, 2].Text = studentdetails.Tables[0].Rows[i]["stud_name"].ToString();
            //            fpspread.Sheets[0].Cells[i, 3].CellType = intgrcel;
            //            string marksql = "select mark from CoCurrActivitie_Det where Roll_No='" + studentdetails.Tables[0].Rows[i]["roll_no"].ToString() + "' and ActivityTextVal ='" + ddlactivity.SelectedItem.Value.ToString() + "'";
            //            dsmark.Clear();
            //            dsmark = da.select_method_wo_parameter(marksql, "Text");

            //            if (dsmark.Tables[0].Rows.Count > 0)
            //            {
            //                fpspread.Sheets[0].Cells[i, 3].Text = dsmark.Tables[0].Rows[0][0].ToString();
            //                fpspread.Sheets[0].Cells[i, 3].HorizontalAlign = HorizontalAlign.Center;
            //            }
            //        }

            //        fpspread.SaveChanges();
            //        fpspread.Visible = false;
            //        lblErrorMsg.Visible = false;
            //        fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
            //        btnfpspread1save.Visible = false;
            //        btnfpspread1delete.Visible = false;
            //    }
            //    else
            //    {
            //        lblErrorMsg.Text = "No Records Founds";
            //        lblErrorMsg.Visible = true;
            //        fpspread.Visible = false;
            //        btnfpspread1save.Visible = false;
            //        btnfpspread1delete.Visible = false;
            //    }
            //}
            //else
            //{
            //    lblErrorMsg.Text = "No Activity Yet Created";
            //    lblErrorMsg.Visible = true;
            //    fpspread.Visible = false;
            //    btnfpspread1save.Visible = false;
            //    btnfpspread1delete.Visible = false;
            //}
            //fpspread.Sheets[0].Columns[3].Locked = false;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            string branch = ddlBranch.SelectedValue.ToString();
            string batch = ddlBatch.SelectedValue.ToString();
            ddlSec.Items.Clear();
            DataSet ds = new DataSet();
            if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0)
            {
                string sqlnew = "select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
                ds.Clear();
                ds = dacc.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Items.Insert(0, "All");
                ddlSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }

    }

    public void BindBatch()
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            string Master1 = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                Master1 = Convert.ToString(Session["group_code"]).Trim().Split(';')[0];
                //string group = Session["group_code"].ToString();
                //if (group.Contains(';'))
                //{
                //    string[] group_semi = group.Split(';');
                //    Master1 = group_semi[0].ToString();
                //}
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "' ";

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
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    public void bindbranch()
    {
        try
        {
            DataSet ds = new DataSet();
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;

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

            string query = string.Empty;
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
                    bindsem();
                    BindSectionDetail();
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    public void BindDegree()
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            string college_code = Session["collegecode"].ToString();
            string query = string.Empty;

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
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataValueField = "Course_Id";
                ddlDegree.DataTextField = "Course_Name";
                ddlDegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }

    }

    public void bindactivity()
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;

            string Degree_Code = string.Empty;
            string Batch_Year = string.Empty;

            Degree_Code = ddlBranch.SelectedItem.Value.ToString();
            Batch_Year = ddlBatch.SelectedItem.Text.ToString();
            term = ddlSemYr.SelectedItem.Text.ToString().Trim();
            string sqlselect = "select * from  activity_entry where  Degree_Code='" + Degree_Code + "' and Batch_Year='" + Batch_Year + "' and term='" + term + "'";
            DataSet dsselect = new DataSet();
            dsselect.Clear();
            dsselect = da.select_method_wo_parameter(sqlselect, "Text");

            if (dsselect.Tables.Count > 0 && dsselect.Tables[0].Rows.Count > 0)
            {
                string currentsem = ddlSemYr.SelectedItem.Text.ToString();
                string degreecode = ddlBranch.SelectedItem.Value.ToString();
                string batchyear = ddlBatch.SelectedItem.Text.ToString();
                string strtit_acitivity = string.Empty;

                for (int ij = 0; ij < dsselect.Tables[0].Rows.Count; ij++)
                {
                    if (strtit_acitivity == "")
                    {
                        strtit_acitivity = dsselect.Tables[0].Rows[ij][1].ToString();
                    }
                    else
                    {
                        strtit_acitivity = strtit_acitivity + "','" + dsselect.Tables[0].Rows[ij][1].ToString();
                    }
                }

                string queryactivity = " select * from textvaltable where TextCriteria='RActv' and college_code='" + Session["collegecode"].ToString() + "' and TextCode in ('" + strtit_acitivity + "') ";

                DataSet newact = new DataSet();
                newact.Clear();
                newact = da.select_method_wo_parameter(queryactivity, "Text");

                if (newact.Tables[0].Rows.Count > 0)
                {
                    ddlactivity.DataSource = newact;
                    ddlactivity.DataTextField = "TextVal";
                    ddlactivity.DataValueField = "TextCode";
                    ddlactivity.DataBind();
                    ddlactivity.Visible = false;
                }
                else
                {
                    //lblparterr.Visible = false;
                }
            }
            else
            {
                ddlactivity.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;

            ddlSemYr.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            DataSet ds = new DataSet();
            string sqlnew = string.Empty;

            if (ddlBranch.Items.Count > 0 && ddlBatch.Items.Count > 0)
            {
                sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.Text.ToString() + "' and college_code='" + Session["collegecode"] + "'";
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
                    sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and college_code='" + Session["collegecode"] + "'";
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
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void ddlactivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;

            fpspread.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void btnfpspread1save_Click1(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            bool issuc = false;
            fpspread.SaveChanges();
            Hashtable ht = new Hashtable();
            ht.Clear();
            string batch_year = ddlBatch.SelectedItem.Text.ToString();
            string degree_code = ddlBranch.SelectedItem.Value.ToString();

            if (fpspread.Sheets[0].RowCount > 0)
            {
                if (fpspread.Sheets[0].ColumnCount > 0)
                {
                    if (chk_att.Checked == true)
                    {
                        issuc = false;
                        for (int im = 3; im < fpspread.Sheets[0].ColumnCount; im++)
                        {
                            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                string roll_no = fpspread.Sheets[0].Cells[i, 1].Text.ToString();
                                string acivityMark = fpspread.Sheets[0].Cells[i, im].Text.ToString();
                                // string acivityMark1 = fpspread.Sheets[0].Cells[i, fpspread.Sheets[0].ColumnCount - 1].Text.ToString();
                                string accodeval = fpspread.Sheets[0].ColumnHeader.Cells[0, im].Note;
                                //string accodeval1 = fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note;
                                if (accodeval == "Att")
                                {
                                    if (acivityMark.Trim() == "" || acivityMark.Trim() == null)
                                    {
                                        acivityMark = "0";
                                    }
                                    string strinsert = "if exists (select * from CoCurrActivitie_Det where Roll_No='" + roll_no + "' and istype='" + accodeval + "' and Degree_Code ='" + degree_code + "' and Batch_Year='" + batch_year + "' and term='" + term + "')  update CoCurrActivitie_Det set Mark='" + acivityMark + "' where Roll_No='" + roll_no + "' and istype='" + accodeval + "' and Degree_Code ='" + degree_code + "' and Batch_Year='" + batch_year + "' and term='" + term + "' else insert into CoCurrActivitie_Det (Roll_No,istype,Mark,Degree_Code,Batch_Year,term) values ('" + roll_no + "','" + accodeval + "','" + acivityMark + "','" + degree_code + "','" + batch_year + "','" + term + "') ";
                                    int re = da.insert_method(strinsert, ht, "Text");
                                    if (re > 0)
                                    {
                                        issuc = true;
                                    }
                                }
                                else if (accodeval == "totatt")
                                {
                                    accodeval = "Att";
                                    if (acivityMark.Trim() == "" || acivityMark.Trim() == null)
                                    {
                                        acivityMark = "0";
                                    }
                                    string strinsert = "if exists (select * from CoCurrActivitie_Det where Roll_No='" + roll_no + "' and istype='" + accodeval + "' and Degree_Code ='" + degree_code + "' and Batch_Year='" + batch_year + "' and term='" + term + "')  update CoCurrActivitie_Det set totatt_remarks='" + acivityMark + "' where Roll_No='" + roll_no + "' and istype='" + accodeval + "' and Degree_Code ='" + degree_code + "' and Batch_Year='" + batch_year + "' and term='" + term + "' else insert into CoCurrActivitie_Det (Roll_No,istype,totatt_remarks,Degree_Code,Batch_Year,term) values ('" + roll_no + "','" + accodeval + "','" + acivityMark + "','" + degree_code + "','" + batch_year + "','" + term + "') ";
                                    int re = da.insert_method(strinsert, ht, "Text");

                                    if (re > 0)
                                    {
                                        issuc = true;
                                    }
                                }
                                else if (accodeval == "Remks")
                                {
                                    if (acivityMark.Trim() == "" || acivityMark.Trim() == null)
                                    {
                                        acivityMark = "-";
                                    }
                                    string strinsert = "if exists (select * from CoCurrActivitie_Det where Roll_No='" + roll_no + "' and istype='" + accodeval + "' and Degree_Code ='" + degree_code + "' and Batch_Year='" + batch_year + "' and term='" + term + "')  update CoCurrActivitie_Det set totatt_remarks='" + acivityMark + "' where Roll_No='" + roll_no + "' and istype='" + accodeval + "' and Degree_Code ='" + degree_code + "' and Batch_Year='" + batch_year + "' and term='" + term + "' else insert into CoCurrActivitie_Det (Roll_No,istype,totatt_remarks,Degree_Code,Batch_Year,term) values ('" + roll_no + "','" + accodeval + "','" + acivityMark + "','" + degree_code + "','" + batch_year + "','" + term + "') ";
                                    int re = da.insert_method(strinsert, ht, "Text");
                                    if (re > 0)
                                    {
                                        issuc = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        issuc = false;
                        for (int im = 3; im < fpspread.Sheets[0].ColumnCount; im++)
                        {
                            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                string roll_no = fpspread.Sheets[0].Cells[i, 1].Text.ToString();
                                string acivityMark = fpspread.Sheets[0].Cells[i, im].Text.ToString();
                                // string acivityMark1 = fpspread.Sheets[0].Cells[i, fpspread.Sheets[0].ColumnCount - 1].Text.ToString();
                                string accodeval = fpspread.Sheets[0].ColumnHeader.Cells[0, im].Note;
                                //string accodeval1 = fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Note;
                                if (acivityMark.Trim() == "" || acivityMark.Trim() == null)
                                {
                                    acivityMark = "0";
                                }
                                string strinsert = "if exists (select * from CoCurrActivitie_Det where Roll_No='" + roll_no + "' and ActivityTextVal='" + accodeval + "' and Degree_Code ='" + degree_code + "' and Batch_Year='" + batch_year + "' and term='" + term + "')  update CoCurrActivitie_Det set Mark='" + acivityMark + "' where Roll_No='" + roll_no + "' and ActivityTextVal='" + accodeval + "' and Degree_Code ='" + degree_code + "' and Batch_Year='" + batch_year + "' and term='" + term + "' else insert into CoCurrActivitie_Det (Roll_No,ActivityTextVal,Mark,Degree_Code,Batch_Year,term) values ('" + roll_no + "','" + accodeval + "','" + acivityMark + "','" + degree_code + "','" + batch_year + "','" + term + "') ";
                                int re = da.insert_method(strinsert, ht, "Text");
                                if (re > 0)
                                {
                                    issuc = true;
                                }
                            }
                        }
                    }
                }
                if (issuc)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void btnfpspread1delete_Click1(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;

            fpspread.SaveChanges();
            Hashtable ht = new Hashtable();
            ht.Clear();
            bool issuc = false;
            string batch_year = ddlBatch.SelectedItem.Text.ToString();
            string degree_code = ddlBranch.SelectedItem.Value.ToString();
            if (chk_att.Checked == true)
            {
                if (fpspread.Sheets[0].RowCount > 0)
                {
                    if (fpspread.Sheets[0].ColumnCount > 0)
                    {
                        for (int im = 3; im < fpspread.Sheets[0].ColumnCount; im++)
                        {
                            if (fpspread.Sheets[0].ColumnHeader.Cells[0, im].Note.ToString() == "Att")
                            {
                                string accodeval = fpspread.Sheets[0].ColumnHeader.Cells[0, im].Note;
                                string strinsert = " delete from CoCurrActivitie_Det where Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and istype='Att' and term='" + term + "' and roll_no in ( select roll_no from Registration where  Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and Sections='" + ddlSec.SelectedItem.Text.ToString() + "' )";
                                int re = da.insert_method(strinsert, ht, "Text");
                                if (re > 0)
                                {
                                    issuc = true;
                                }
                            }
                            else if (fpspread.Sheets[0].ColumnHeader.Cells[0, im].Note.ToString() == "totatt")
                            {
                                string accodeval = fpspread.Sheets[0].ColumnHeader.Cells[0, im].Note;
                                string strinsert = " delete from CoCurrActivitie_Det where Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and istype='Att' and term='" + term + "' and roll_no in ( select roll_no from Registration where  Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and Sections='" + ddlSec.SelectedItem.Text.ToString() + "' )";
                                int re = da.insert_method(strinsert, ht, "Text");
                                if (re > 0)
                                {
                                    issuc = true;
                                }
                            }
                            else if (fpspread.Sheets[0].ColumnHeader.Cells[0, im].Note.ToString() == "Remks")
                            {
                                string accodeval = fpspread.Sheets[0].ColumnHeader.Cells[0, im].Note;
                                string strinsert = " delete from CoCurrActivitie_Det where Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and istype='Remks' and term='" + term + "' and roll_no in ( select roll_no from Registration where  Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and Sections='" + ddlSec.SelectedItem.Text.ToString() + "' )";
                                int re = da.insert_method(strinsert, ht, "Text");
                                if (re > 0)
                                {
                                    issuc = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (fpspread.Sheets[0].RowCount > 0)
                {
                    if (fpspread.Sheets[0].ColumnCount > 0)
                    {
                        for (int im = 3; im < fpspread.Sheets[0].ColumnCount; im++)
                        {

                            //if (im == 0)
                            //{ 
                            string accodeval = fpspread.Sheets[0].ColumnHeader.Cells[0, im].Note;
                            string strinsert = " delete from CoCurrActivitie_Det where Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and ActivityTextVal ='" + accodeval + "' and term='" + term + "' and roll_no in ( select roll_no from Registration where  Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and Sections='" + ddlSec.SelectedItem.Text.ToString() + "' )";
                            int re = da.insert_method(strinsert, ht, "Text");
                            if (re > 0)
                            {
                                issuc = true;
                            }
                            //}
                            //else
                            //{
                            //    string strinsert = " delete from CoCurrActivitie_Det where Degree_Code='" + degree_code + "' and Batch_Year='" + batch_year + "' and ActivityTextVal ='" + accodeval1 + "'";
                            //    da.insert_method(strinsert, ht, "Text");
                            //}
                        }
                    }
                }
            }
            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
            {
                for (int j = 3; j < fpspread.Sheets[0].ColumnCount; j++)
                {
                    fpspread.Sheets[0].Cells[i, j].Text = string.Empty;
                }
            }
            fpspread.SaveChanges();
            if (issuc)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Can't Be Not Deleted')", true);
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void chk_att_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            fpspread.Visible = false;
            btnfpspread1save.Visible = false;
            btnfpspread1delete.Visible = false;

            FpSpread1.Visible = false;
            btnok.Visible = false;
            hideexportimport();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void btn_importex(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;

            Boolean rollflag = false;
            Boolean stro = false;
            string errorroll = string.Empty;
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
                    return;
                }
            }
            else
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Please Select The File and Then Proceed";
                return;
            }
            fpmarkimport.Visible = false;
            fpspread.SaveChanges();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;

            string reportname = txtexcelname.Text;
            txtexcelname.Text = string.Empty;
            if (reportname.ToString().Trim() != "")
            {
                lblexcelerror.Text = string.Empty;
                lblexcelerror.Visible = false;

                da.printexcelreport(fpspread, reportname);
                txtexcelname.Text = string.Empty;
            }
            else
            {
                lblexcelerror.Text = "Please Enter Your Report Name";
                lblexcelerror.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    public void hideexportimport()
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            fpmarkexcel.Visible = false;
            btn_import.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

    public void showexportimport()
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = string.Empty;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
            fpmarkexcel.Visible = true;
            btn_import.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = Convert.ToString(ex);
            lblErrorMsg.Visible = true;
        }
    }

}