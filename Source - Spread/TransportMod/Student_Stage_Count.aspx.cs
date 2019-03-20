using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using InsproDataAccess;
using System.Configuration;

public partial class Student_Stage_Count : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("TransportIndex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/TransportMod/TransportIndex.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            bindcollege();
            if (ddlcollege.Items.Count > 0)
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            bindStage();
        }
        lblMainErr.Visible = false;
        lblsmserror.Visible = false;
        lblsmserror1.Visible = false;
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        bindStage();
        Fpspread1.Visible = false;
        lblMainErr.Visible = false;
        rprint.Visible = false;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStageName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct Stage_Name,Stage_id from Stage_Master where Stage_Name like '" + prefixText + "%' order by Stage_Name";
        name = ws.Getname(query);
        return name;
    }

    protected void txt_AutoStage_Change(object sender, EventArgs e)
    {

    }

    protected void cbStage_Change(object sender, EventArgs e)
    {
        chkchange(cbStage, cblStage, txtStage, "Stage Name");
    }

    protected void cblStage_Change(object sender, EventArgs e)
    {
        chklstchange(cbStage, cblStage, txtStage, "Stage Name");
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string collCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string StageCode = string.Empty;
            StageCode = GetSelectedItemsValueAsString(cblStage);
            if (String.IsNullOrEmpty(StageCode))
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any Stage Name!";
                Fpspread1.Visible = false;
                rprint.Visible = false;
                rprint1.Visible = false;
                Fpspread2.Visible = false;
                return;
            }
            string SelQ = string.Empty;
            if (!String.IsNullOrEmpty(txt_AutoStage.Text.Trim()))
                SelQ = "select st.Stage_Name,st.Stage_id,COUNT(*) StudCount from Registration r,Stage_Master st where college_code='" + collCode + "' and Stage_Name ='" + Convert.ToString(txt_AutoStage.Text.Trim()) + "' and r.Boarding=CAST(Stage_id as Varchar) and CC='0' and DelFlag='0' and Exam_Flag<>'Debar' group by Stage_Name,Stage_id order by Stage_Name";
            else
                SelQ = "select st.Stage_Name,st.Stage_id,COUNT(*) StudCount from Registration r,Stage_Master st where college_code='" + collCode + "' and Boarding in('" + StageCode + "') and r.Boarding=CAST(Stage_id as Varchar) and CC='0' and DelFlag='0' and Exam_Flag<>'Debar' group by Stage_Name,Stage_id order by Stage_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                LoadHeader();

                #region added by prabhakaran on dec 27 2017  "Count appearing on button"

                string totaldayscholarcount = dirAcc.selectScalarString("select COUNT(App_No) as TotalDayScholar from Registration where Stud_Type='Day Scholar' and cc=0 and Delflag=0 and Exam_Flag<>'Debar'");
                if (!string.IsNullOrEmpty(totaldayscholarcount))
                {
                    btntotalDayScholarCount.Text = "DayScholars :" + totaldayscholarcount + "";
                    btntotalDayScholarCount.Visible = true;
                }
                else
                {
                    btntotalDayScholarCount.Text = "DayScholars : 0";
                    btntotalDayScholarCount.Visible = true;
                }
                string totalonlineregistered = dirAcc.selectScalarString("select COUNT(App_No) as Stageselected from Registration where  Stud_Type='DAY Scholar' and cc=0 and Delflag=0 and Exam_Flag<>'Debar'   and isnull(Boarding,'')<>'' and Online_StageChosser='1' ");  // and isnull(Bus_RouteID,'')<>'' or isnull(VehID,'')<>''
                if (!string.IsNullOrEmpty(totalonlineregistered))
                {
                    btnSelectedOnline.Text = "Selected Thorugh Online:" + totalonlineregistered + "";
                    btnSelectedOnline.Visible = true;
                }
                else
                {
                    btnSelectedOnline.Text = "Selected Thorugh Online: 0";
                    btnSelectedOnline.Visible = true;
                }
                string totalstageregisteredCount = dirAcc.selectScalarString("select COUNT(App_No) as Stageselected from Registration where  Stud_Type='DAY Scholar' and cc=0 and Delflag=0 and Exam_Flag<>'Debar' and isnull(Boarding,'')<>'' and Online_StageChosser='0' ");  // and Bus_RouteID<>'' or VehID<>''
                if (!string.IsNullOrEmpty(totaldayscholarcount))
                {
                    btnselectedgeneral.Text = "Selected Directly :" + totalstageregisteredCount + "";
                    btnselectedgeneral.Visible = true;
                }
                else
                {
                    btnselectedgeneral.Text = "Selected Directly : 0";
                    btnselectedgeneral.Visible = true;
                }

                string totalstageunselectedCount = dirAcc.selectScalarString("select COUNT(App_No) as Stageselected from Registration where ISNULL(Boarding,'')='' and Stud_Type='DAY Scholar' and cc=0 and Delflag=0 and Exam_Flag<>'Debar'  and ISNULL(Bus_RouteID,'')='' and ISNULL(VehID,'')=''");
                if (!string.IsNullOrEmpty(totaldayscholarcount))
                {
                    btnStageNotSelectedStudent.Text = "Stage Unselected Students :" + totalstageunselectedCount + "";
                    btnStageNotSelectedStudent.Visible = true;
                }
                else
                {
                    btnStageNotSelectedStudent.Text = "Stage Unselected Students : 0";
                    btnStageNotSelectedStudent.Visible = true;
                }


                #endregion

                FarPoint.Web.Spread.ButtonCellType btnCell = new FarPoint.Web.Spread.ButtonCellType();
                btnCell.Text = "View";
                btnCell.CssClass = "textbox1 textbox btn2";
                for (int my = 0; my < ds.Tables[0].Rows.Count; my++)
                {
                    Fpspread1.Sheets[0].RowCount++;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(my + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[my]["Stage_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[my]["Stage_id"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[my]["StudCount"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = btnCell;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                }
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.SaveChanges();
                Fpspread1.Visible = true;
                Fpspread2.Visible = false;
                rprint1.Visible = false;
                rprint.Visible = true;
                lblMainErr.Visible = false;
            }
            else
            {
                Fpspread1.Visible = false;
                Fpspread2.Visible = false;
                rprint1.Visible = false;
                rprint.Visible = false;
                lblMainErr.Visible = true;
                lblMainErr.Text = "No Record(s) Found!";
            }
        }
        catch { }
    }

    private void LoadHeader()
    {
        try
        {
            Fpspread1.Visible = false;
            Fpspread2.Visible = false;
            rprint1.Visible = false;
            rprint.Visible = false;
            lblMainErr.Visible = false;

            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnHeader.Columns.Count = 4;

            FarPoint.Web.Spread.StyleInfo darkStyle = new FarPoint.Web.Spread.StyleInfo();
            darkStyle.Font.Bold = true;
            darkStyle.Font.Name = "Book Antiqua";
            darkStyle.Font.Size = FontUnit.Medium;
            darkStyle.HorizontalAlign = HorizontalAlign.Center;
            darkStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkStyle.ForeColor = Color.Black;
            Fpspread1.Sheets[0].ColumnHeader.DefaultStyle = darkStyle;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread1.Columns[0].Width = 75;
            Fpspread1.Columns[0].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Stage Name";
            Fpspread1.Columns[1].Width = 375;
            Fpspread1.Columns[1].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "No.of Students";
            Fpspread1.Columns[2].Width = 100;
            Fpspread1.Columns[2].Locked = true;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "View Details";
            Fpspread1.Columns[3].Width = 100;

        }
        catch { }
    }

    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string CollCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string actRow = Convert.ToString(Fpspread1.ActiveSheetView.ActiveRow);
            string actCol = Convert.ToString(Fpspread1.ActiveSheetView.ActiveColumn);
            int Row = 0; int Col = 0;
            string SelQ = string.Empty;
            string StageID = string.Empty;
            string StageName = string.Empty;
            Int32.TryParse(actRow, out Row);
            Int32.TryParse(actCol, out Col);
            DataView dv = new DataView();

            if (Row >= 0 && Col == 3)
            {
                StageName = Convert.ToString(Fpspread1.Sheets[0].Cells[Row, 1].Text);
                StageID = Convert.ToString(Fpspread1.Sheets[0].Cells[Row, 1].Tag);
                SelQ = " select r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Batch_Year,r.degree_code,ISNULL(r.Sections,'') as Sections,Stage_Name from Registration r,Stage_Master st where r.Boarding=CAST(st.Stage_id as Varchar) and st.Stage_id='" + StageID + "' and r.CC='0' and r.DelFlag='0' and Exam_Flag<>'Debar' order by r.degree_code";
                SelQ = SelQ + " select c.Course_Name+' - '+d.Dept_Name as Dept,c.Course_Name+' - '+d.dept_acronym as DeptAcr,deg.Degree_Code from Course c,Degree deg,Department d where c.Course_Id=deg.Course_Id and deg.Dept_Code=d.Dept_Code and deg.college_code='" + CollCode + "' order by deg.Degree_Code";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    LoadNxtSpreadHeader();

                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[0, 0].Text = Convert.ToString(StageName);
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Rows[0].BackColor = Color.LightGreen;
                    Fpspread2.Sheets[0].SpanModel.Add(0, 0, 1, 7);
                    for (int spr = 0; spr < ds.Tables[0].Rows.Count; spr++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(spr + 1);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Reg_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_Admit"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Stud_Name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Batch_Year"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;

                        ds.Tables[1].DefaultView.RowFilter = " degree_Code='" + Convert.ToString(ds.Tables[0].Rows[spr]["degree_code"]) + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[spr]["Sections"])))
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[0]["Dept"]) + " - " + Convert.ToString(ds.Tables[0].Rows[spr]["Sections"]);
                            else
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[0]["Dept"]);
                        }
                        else
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = "";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    }
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Sheets[0].FrozenRowCount = 1;
                    Fpspread2.Visible = true;
                    rprint1.Visible = true;
                }
                else
                {
                    Fpspread2.Visible = false;
                    rprint1.Visible = false;
                }
            }
        }
        catch { }
    }

    private void LoadNxtSpreadHeader()
    {
        try
        {
            Fpspread2.Visible = false;
            rprint1.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].ColumnHeader.Columns.Count = 7;
            Fpspread2.Sheets[0].RowCount = 0;

            FarPoint.Web.Spread.StyleInfo myStyle = new FarPoint.Web.Spread.StyleInfo();
            myStyle.Font.Bold = true;
            myStyle.Font.Name = "Book Antiqua";
            myStyle.Font.Size = FontUnit.Medium;
            myStyle.HorizontalAlign = HorizontalAlign.Center;
            myStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            myStyle.ForeColor = Color.Black;
            Fpspread2.Sheets[0].ColumnHeader.DefaultStyle = myStyle;

            FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread2.Columns[0].Width = 75;
            Fpspread2.Columns[0].Locked = true;
            Fpspread2.Columns[0].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            Fpspread2.Columns[1].Width = 100;
            Fpspread2.Columns[1].Locked = true;
            Fpspread2.Columns[1].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
            Fpspread2.Columns[2].Width = 100;
            Fpspread2.Columns[2].Locked = true;
            Fpspread2.Columns[2].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            Fpspread2.Columns[3].Width = 100;
            Fpspread2.Columns[3].Locked = true;
            Fpspread2.Columns[3].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            Fpspread2.Columns[4].Width = 150;
            Fpspread2.Columns[4].Locked = true;
            Fpspread2.Columns[4].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year";
            Fpspread2.Columns[5].Width = 100;
            Fpspread2.Columns[5].Locked = true;
            Fpspread2.Columns[5].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department";
            Fpspread2.Columns[6].Width = 200;
            Fpspread2.Columns[6].Locked = true;
            Fpspread2.Columns[6].CellType = txttype;
        }
        catch { }
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                txtexcel.Text = "";
                d2.printexcelreport(Fpspread1, reportname);
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Text = "Please Enter Your Report Name";
                lblsmserror.Visible = true;
            }
            btnprintmaster.Focus();
        }
        catch { }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "StageWise Student Count Report";
            string pagename = "Student_Stage_Count.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }

    protected void btnexcel1_Click(object sender, EventArgs e)
    {
        try
        {
            Fpspread2.SaveChanges();
            string reportname = txtexcel1.Text;
            if (reportname.ToString().Trim() != "")
            {
                txtexcel1.Text = "";
                d2.printexcelreport(Fpspread2, reportname);
                lblsmserror1.Visible = false;
            }
            else
            {
                lblsmserror1.Text = "Please Enter Your Report Name";
                lblsmserror1.Visible = true;
            }
            btnprintmaster1.Focus();
        }
        catch { }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "StageWise Student Details Report";
            string pagename = "Student_Stage_Count.aspx";
            Printmaster1.loadspreaddetails(Fpspread2, pagename, degreedetails);
            Printmaster1.Visible = true;
            btnprintmaster1.Focus();
        }
        catch { }
    }

    private void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e) { }
    }

    private void bindStage()
    {
        try
        {
            cblStage.Items.Clear();
            txtStage.Text = "--Select--";
            cbStage.Checked = false;
            string SelQ = "select distinct Stage_id,Stage_Name from Stage_Master order by Stage_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblStage.DataSource = ds;
                cblStage.DataTextField = "Stage_Name";
                cblStage.DataValueField = "Stage_id";
                cblStage.DataBind();

                if (cblStage.Items.Count > 0)
                {
                    for (int ik = 0; ik < cblStage.Items.Count; ik++)
                    {
                        cblStage.Items[ik].Selected = true;
                    }
                    txtStage.Text = "Stage Name (" + Convert.ToString(cblStage.Items.Count) + ")";
                    cbStage.Checked = true;
                }
            }
        }
        catch { }
    }

    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + " (" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + " (" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }

    #region added by prabha on dec 27 2017


    protected void btntotalDayScholarCount_onclick(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string CollCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string SelQ = string.Empty;
            string StageID = string.Empty;
            string StageName = string.Empty;
            DataView dv = new DataView();
            string orderby = RollAndRegSettings();
            if (!string.IsNullOrEmpty(CollCode))
            {
                SelQ = " select r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Batch_Year,r.degree_code,ISNULL(r.Sections,'') as Sections,Stage_Name from Registration r,Stage_Master st where r.Boarding=CAST(st.Stage_id as Varchar)  and r.CC='0' and r.DelFlag='0' and r.Stud_Type='DAY Scholar' and Exam_Flag<>'Debar'" + orderby + "";
                SelQ = SelQ + " select c.Course_Name+' - '+d.Dept_Name as Dept,c.Course_Name+' - '+d.dept_acronym as DeptAcr,deg.Degree_Code from Course c,Degree deg,Department d where c.Course_Id=deg.Course_Id and deg.Dept_Code=d.Dept_Code and deg.college_code='" + CollCode + "' order by deg.Degree_Code";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Visible = false;
                    rprint.Visible = false;
                    //LoadNxtSpreadHeaderNEW(int sno, int rollno, int regno, int admissionno, int studname, int batchyr, int departmanent, bool stage)
                    LoadNxtSpreadHeaderNEW(50, 100, 100, 100, 200, 100, 150, false);

                    Fpspread2.Sheets[0].RowCount++;
                    //Fpspread2.Sheets[0].Cells[0, 0].Text = Convert.ToString(StageName);
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread2.Sheets[0].Rows[0].BackColor = Color.LightGreen;
                    Fpspread2.Sheets[0].SpanModel.Add(0, 0, 1, 7);
                    for (int spr = 0; spr < ds.Tables[0].Rows.Count; spr++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(spr + 1);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Reg_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_Admit"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Stud_Name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Batch_Year"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                        ds.Tables[1].DefaultView.RowFilter = " degree_Code='" + Convert.ToString(ds.Tables[0].Rows[spr]["degree_code"]) + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[spr]["Sections"])))
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[0]["DeptAcr"]) + " - " + Convert.ToString(ds.Tables[0].Rows[spr]["Sections"]);
                            else
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[0]["DeptAcr"]);
                        }
                        else
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = "";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    }
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Sheets[0].FrozenRowCount = 1;
                    Fpspread2.Visible = true;
                    rprint1.Visible = true;
                    Fpspread2.SaveChanges();
                }
                else
                {
                    Fpspread2.Visible = false;
                    rprint1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student_Stage_Count");

        }
    }

    protected void btnselectedgeneral_onclick(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string CollCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string SelQ = string.Empty;
            string StageID = string.Empty;
            string StageName = string.Empty;
            DataView dv = new DataView();
            string orderby = RollAndRegSettings();
            if (!string.IsNullOrEmpty(CollCode))
            {
                SelQ = " select r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Batch_Year,r.degree_code,ISNULL(r.Sections,'') as Sections,Stage_Name from Registration r,Stage_Master st where r.Boarding=CAST(st.Stage_id as Varchar)  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'Debar'  and r.Stud_Type='DAY Scholar'  and r.Boarding<>'' and Online_StageChosser='0' " + orderby + "  ";
                SelQ = SelQ + " select c.Course_Name+' - '+d.Dept_Name as Dept,c.Course_Name+' - '+d.dept_acronym as DeptAcr,deg.Degree_Code from Course c,Degree deg,Department d where c.Course_Id=deg.Course_Id and deg.Dept_Code=d.Dept_Code and deg.college_code='" + CollCode + "' order by deg.Degree_Code";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Visible = false;
                    rprint.Visible = false;
                    //LoadNxtSpreadHeaderNEW(int sno, int rollno, int regno, int admissionno, int studname, int batchyr, int departmanent, bool stage)
                    LoadNxtSpreadHeaderNEW(50, 100, 100, 100, 170, 90, 150, true);

                    Fpspread2.Sheets[0].RowCount++;
                    //Fpspread2.Sheets[0].Cells[0, 0].Text = Convert.ToString(StageName);
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread2.Sheets[0].Rows[0].BackColor = Color.LightGreen;
                    Fpspread2.Sheets[0].SpanModel.Add(0, 0, 1, 7);
                    for (int spr = 0; spr < ds.Tables[0].Rows.Count; spr++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(spr + 1);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Reg_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_Admit"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Stud_Name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Batch_Year"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Stage_Name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;

                        ds.Tables[1].DefaultView.RowFilter = " degree_Code='" + Convert.ToString(ds.Tables[0].Rows[spr]["degree_code"]) + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[spr]["Sections"])))
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv[0]["DeptAcr"]) + " - " + Convert.ToString(ds.Tables[0].Rows[spr]["Sections"]);
                            else
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv[0]["DeptAcr"]);
                        }
                        else
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = "-";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    }
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Sheets[0].FrozenRowCount = 1;
                    Fpspread2.Visible = true;
                    rprint1.Visible = true;
                    Fpspread2.SaveChanges();
                }
                else
                {
                    Fpspread2.Visible = false;
                    rprint1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student_Stage_Count");

        }
    }

    protected void btnStageNotSelectedStudent_onclick(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string CollCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string SelQ = string.Empty;
            string StageID = string.Empty;
            string StageName = string.Empty;
            DataView dv = new DataView();
            string orderby = RollAndRegSettings();
            if (!string.IsNullOrEmpty(CollCode))
            {
                SelQ = " select r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Batch_Year,r.degree_code,ISNULL(r.Sections,'') as Sections from Registration r where r.CC='0' and r.DelFlag='0' and Exam_Flag<>'Debar' and Stud_Type='DAY Scholar' and ISNULL(Bus_RouteID,'')='' and ISNULL(VehID,'')='' and ISNULL(Boarding,'')='' " + orderby + " ";
                SelQ = SelQ + " select c.Course_Name+' - '+d.Dept_Name as Dept,c.Course_Name+' - '+d.dept_acronym as DeptAcr,deg.Degree_Code from Course c,Degree deg,Department d where c.Course_Id=deg.Course_Id and deg.Dept_Code=d.Dept_Code and deg.college_code='" + CollCode + "' order by deg.Degree_Code";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Visible = false;
                    rprint.Visible = false;
                    //LoadNxtSpreadHeaderNEW(int sno, int rollno, int regno, int admissionno, int studname, int batchyr, int departmanent, bool stage)
                    LoadNxtSpreadHeaderNEW(50, 100, 100, 100, 200, 100, 150, false);

                    Fpspread2.Sheets[0].RowCount++;
                    //Fpspread2.Sheets[0].Cells[0, 0].Text = Convert.ToString(StageName);
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread2.Sheets[0].Rows[0].BackColor = Color.LightGreen;
                    Fpspread2.Sheets[0].SpanModel.Add(0, 0, 1, 7);
                    for (int spr = 0; spr < ds.Tables[0].Rows.Count; spr++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(spr + 1);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Reg_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_Admit"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Stud_Name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Batch_Year"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                        ds.Tables[1].DefaultView.RowFilter = " degree_Code='" + Convert.ToString(ds.Tables[0].Rows[spr]["degree_code"]) + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[spr]["Sections"])))
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[0]["DeptAcr"]) + " - " + Convert.ToString(ds.Tables[0].Rows[spr]["Sections"]);
                            else
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[0]["DeptAcr"]);
                        }
                        else
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = "";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    }
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Sheets[0].FrozenRowCount = 1;
                    Fpspread2.Visible = true;
                    rprint1.Visible = true;
                    Fpspread2.SaveChanges();
                }
                else
                {
                    Fpspread2.Visible = false;
                    rprint1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student_Stage_Count");

        }
    }

    protected void btnSelectedOnline_onclick(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string CollCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string SelQ = string.Empty;
            string StageID = string.Empty;
            string StageName = string.Empty;
            DataView dv = new DataView();
            string orderby = RollAndRegSettings();

            if (!string.IsNullOrEmpty(CollCode))
            {
                SelQ = " select r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Batch_Year,r.degree_code,ISNULL(r.Sections,'') as Sections,Stage_Name from Registration r,Stage_Master st where r.Boarding=CAST(st.Stage_id as Varchar)  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'Debar'  and r.Stud_Type='DAY Scholar'  and r.Boarding<>'' and Online_StageChosser='1' " + orderby + " ";    //and r.VehID<>'' and r.Bus_RouteID<>''
                SelQ = SelQ + " select c.Course_Name+' - '+d.Dept_Name as Dept,c.Course_Name+' - '+d.dept_acronym as DeptAcr,deg.Degree_Code from Course c,Degree deg,Department d where c.Course_Id=deg.Course_Id and deg.Dept_Code=d.Dept_Code and deg.college_code='" + CollCode + "' order by deg.Degree_Code";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Visible = false;
                    rprint.Visible = false;

                    //LoadNxtSpreadHeaderNEW(int sno, int rollno, int regno, int admissionno, int studname, int batchyr, int departmanent, bool stage)
                    LoadNxtSpreadHeaderNEW(50, 100, 100, 100, 170, 100, 150, true);

                    Fpspread2.Sheets[0].RowCount++;
                    //Fpspread2.Sheets[0].Cells[0, 0].Text = Convert.ToString(StageName);
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread2.Sheets[0].Rows[0].BackColor = Color.LightGreen;
                    Fpspread2.Sheets[0].SpanModel.Add(0, 0, 1, 7);
                    for (int spr = 0; spr < ds.Tables[0].Rows.Count; spr++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(spr + 1);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Reg_No"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Roll_Admit"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Stud_Name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Batch_Year"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[spr]["Stage_Name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;

                        ds.Tables[1].DefaultView.RowFilter = " degree_Code='" + Convert.ToString(ds.Tables[0].Rows[spr]["degree_code"]) + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[spr]["Sections"])))
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv[0]["DeptAcr"]) + " - " + Convert.ToString(ds.Tables[0].Rows[spr]["Sections"]);
                            else
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv[0]["DeptAcr"]);
                        }
                        else
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = "-";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    }
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Sheets[0].FrozenRowCount = 1;
                    Fpspread2.Visible = true;
                    rprint1.Visible = true;
                    Fpspread2.SaveChanges();
                }
                else
                {
                    Fpspread2.Visible = false;
                    rprint1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student_Stage_Count");

        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsgNEW.Text = string.Empty;
            divPopAlertNEW.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    private void LoadNxtSpreadHeaderNEW(int sno, int rollno, int regno, int admissionno, int studname, int batchyr, int departmanent, bool stage)
    {
        try
        {
            Fpspread2.Visible = false;
            rprint1.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            if (stage)
                Fpspread2.Sheets[0].ColumnHeader.Columns.Count = 8;
            else
                Fpspread2.Sheets[0].ColumnHeader.Columns.Count = 7;

            Fpspread2.Sheets[0].RowCount = 0;

            FarPoint.Web.Spread.StyleInfo myStyle = new FarPoint.Web.Spread.StyleInfo();
            myStyle.Font.Bold = true;
            myStyle.Font.Name = "Book Antiqua";
            myStyle.Font.Size = FontUnit.Medium;
            myStyle.HorizontalAlign = HorizontalAlign.Center;
            myStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            myStyle.ForeColor = Color.Black;
            Fpspread2.Sheets[0].ColumnHeader.DefaultStyle = myStyle;

            FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread2.Columns[0].Width = sno;
            Fpspread2.Columns[0].Locked = true;
            Fpspread2.Columns[0].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            Fpspread2.Columns[1].Width = rollno;
            Fpspread2.Columns[1].Locked = true;
            Fpspread2.Columns[1].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
            Fpspread2.Columns[2].Width = regno;
            Fpspread2.Columns[2].Locked = true;
            Fpspread2.Columns[2].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            Fpspread2.Columns[3].Width = admissionno;
            Fpspread2.Columns[3].Locked = true;
            Fpspread2.Columns[3].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            Fpspread2.Columns[4].Width = studname;
            Fpspread2.Columns[4].Locked = true;
            Fpspread2.Columns[4].CellType = txttype;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch Year";
            Fpspread2.Columns[5].Width = batchyr;
            Fpspread2.Columns[5].Locked = true;
            Fpspread2.Columns[5].CellType = txttype;
            if (stage)
            {
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Stage name";
                Fpspread2.Columns[6].Width = 100;
                Fpspread2.Columns[6].Locked = true;
                Fpspread2.Columns[6].CellType = txttype;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Department";
                Fpspread2.Columns[7].Width = departmanent;
                Fpspread2.Columns[7].Locked = true;
                Fpspread2.Columns[7].CellType = txttype;
            }
            else
            {
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department";
                Fpspread2.Columns[6].Width = departmanent;
                Fpspread2.Columns[6].Locked = true;
                Fpspread2.Columns[6].CellType = txttype;
            }
        }
        catch { }
    }

    private string RollAndRegSettings()
    {
        string orderby = string.Empty;
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        orderby = "order by Roll_No";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        orderby = "order by Reg_No";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        orderby = "order by Roll_Admit";
                    }
                }
            }

        }
        catch { }
        return orderby;
    }


    #endregion

}