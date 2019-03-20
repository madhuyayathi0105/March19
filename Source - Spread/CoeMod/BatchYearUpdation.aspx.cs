using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class BatchYearUpdation : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();

    Hashtable htMandColumns = new Hashtable();

    string regNo = string.Empty;
    string batchYear = string.Empty;
    string status = string.Empty;
    string statusCode = string.Empty;

    public enum Status
    {
        NotImport = 0,
        Imported = 1,
        Available = 2,
        NotAvailable = 3
    };

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
            if (!IsPostBack)
            {
                divImport.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            divImport.Visible = false;
            htMandColumns.Clear();
            htMandColumns.Add("register no", "Reg_No");
            htMandColumns.Add("batch year", "Batch_Year");
            DataTable dtStudentBatchYear = new DataTable();
            dtStudentBatchYear.Clear();
            dtStudentBatchYear.Rows.Clear();
            dtStudentBatchYear.Columns.Clear();
            dtStudentBatchYear.Columns.Add("Reg_No");
            dtStudentBatchYear.Columns.Add("Batch_Year");
            dtStudentBatchYear.Columns.Add("StatusCode");
            dtStudentBatchYear.Columns.Add("Status");
            using (Stream Stream = this.fuImportExcel.FileContent as Stream)
            {
                if (fuImportExcel.HasFile)
                {
                    string extension = Path.GetFileName(fuImportExcel.PostedFile.FileName);
                    if (extension.Trim() != "")
                    {
                        if (System.IO.Path.GetExtension(fuImportExcel.FileName) == ".xls" || System.IO.Path.GetExtension(fuImportExcel.FileName) == ".xlsx")
                        {
                            OleDbDataAdapter adapter = new OleDbDataAdapter();
                            string path = Server.MapPath("~/Upload/StudentBatchYearUpdation" + System.IO.Path.GetExtension(fuImportExcel.FileName));
                            fuImportExcel.SaveAs(path);
                            ds.Clear();
                            ds = Excelconvertdataset(path);
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                int countMandColumns = 0;
                                bool isRegNoCol = false;
                                bool isBatchYearCol = false;
                                bool hasInvalidColumn = false;
                                string invalidColumn = string.Empty;
                                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                                {
                                    string columnName = Convert.ToString(ds.Tables[0].Columns[i].ColumnName).Trim().ToLower();
                                    if (columnName.Trim().ToLower() == "register no")
                                    {
                                        countMandColumns++;
                                        isRegNoCol = true;
                                    }
                                    else if (columnName.Trim().ToLower() == "batch year")
                                    {
                                        countMandColumns++;
                                        isBatchYearCol = true;
                                    }
                                    if (!htMandColumns.Contains(columnName))
                                    {
                                        hasInvalidColumn = true;
                                        if (invalidColumn == "")
                                        {
                                            invalidColumn = columnName;
                                        }
                                        else
                                        {
                                            invalidColumn += "," + columnName;
                                        }
                                    }
                                }
                                if (hasInvalidColumn)
                                {
                                    txtNotSave.Text = "Invalid Columns : " + invalidColumn;
                                    divNotSave.Visible = true;
                                    return;
                                }

                                if (countMandColumns != 2)
                                {
                                    string mand = ((!isRegNoCol) ? "Register No," : "") + ((!isBatchYearCol) ? "Batch Year" : "");
                                    mand = mand.Trim(',');
                                    txtNotSave.Text = "Madatory Columns " + mand.Trim(',') + (((mand.Split(',')).Length > 1) ? " Are " : " is ") + "missing";
                                    divNotSave.Visible = true;
                                    return;
                                }
                                dtStudentBatchYear.Rows.Clear();
                                DataRow drStudentBatchYear;
                                foreach (DataRow drStudent in ds.Tables[0].Rows)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(drStudent["register no"]).Trim()) && !string.IsNullOrEmpty(Convert.ToString(drStudent["batch year"]).Trim()))
                                    {
                                        drStudentBatchYear = dtStudentBatchYear.NewRow();
                                        drStudentBatchYear["Reg_No"] = Convert.ToString(drStudent["register no"]).Trim();
                                        drStudentBatchYear["Batch_Year"] = Convert.ToString(drStudent["batch year"]).Trim();
                                        drStudentBatchYear["StatusCode"] = Convert.ToString("0").Trim();
                                        drStudentBatchYear["Status"] = Convert.ToString("Not Import").Trim();
                                        dtStudentBatchYear.Rows.Add(drStudentBatchYear);
                                    }
                                }
                                if (dtStudentBatchYear.Rows.Count > 0)
                                {
                                    DataTable dtDistinctStudents = dtStudentBatchYear.DefaultView.ToTable(true);
                                    gvImport.DataSource = dtDistinctStudents;
                                    gvImport.DataBind();
                                    divImport.Visible = true;
                                }
                                else
                                {
                                    divImport.Visible = false;
                                    divPopAlert.Visible = true;
                                    lblAlertMsg.Text = "Excel does not having any data!";
                                    return;
                                }
                            }
                            else
                            {
                                divImport.Visible = false;
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = "Excel does not having any data!";
                                return;
                            }
                        }
                        else
                        {
                            divImport.Visible = false;
                            divPopAlert.Visible = true;
                            lblAlertMsg.Text = "Please Select .xls and .xlsx files only!!!";
                            return;
                        }
                    }
                    else
                    {
                        divImport.Visible = false;
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "Please Browse Upload File";
                        return;
                    }
                }
                else
                {
                    divImport.Visible = false;
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Browse Upload File";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            bool isUpdated = false;
            foreach (GridViewRow gvRows in gvImport.Rows)
            {
                Label lblRegNo = (Label)gvRows.FindControl("lblRegNo");
                Label lblBatchYear = (Label)gvRows.FindControl("lblBatchYear");
                Label lblStatus = (Label)gvRows.FindControl("lblStatus");
                Label lblStatusCode = (Label)gvRows.FindControl("lblStatusCode");
                if (!string.IsNullOrEmpty(lblRegNo.Text.Trim()) && !string.IsNullOrEmpty(lblBatchYear.Text.Trim()) && !string.IsNullOrEmpty(lblStatusCode.Text.Trim()) && lblStatusCode.Text.Trim() != "3")
                {
                    string OldExamCode = string.Empty;
                    string RollNo = string.Empty;
                    string batch_year = string.Empty;
                    string degreeCode = string.Empty;
                    string CurrentSemester = string.Empty;
                    DataSet Dnew = new DataSet();
                    string Selectquery = "select batch_year,degree_code,current_semester,roll_no from registration where reg_no='" + lblRegNo.Text.Trim() + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(Selectquery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        batch_year = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                        degreeCode = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]);
                        CurrentSemester = Convert.ToString(ds.Tables[0].Rows[0]["current_semester"]);
                        RollNo = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);
                        OldExamCode = d2.GetFunction("select exam_code from exam_details where exam_Month='11' and exam_year='2016' and batch_year='" + batch_year + "' and degree_code ='" + degreeCode + "'");
                    }

                    string qry = "if exists(select batch_year from registration where Reg_No='" + lblRegNo.Text.Trim() + "') update registration set batch_year='" + lblBatchYear.Text.Trim() + "' where Reg_No='" + lblRegNo.Text.Trim() + "'";
                    int result = d2.update_method_wo_parameter(qry, "text");

                    string NewExamCode = string.Empty;
                    string sqlquery = "if not exists (select exam_code from exam_details where exam_Month='11' and exam_year='2016' and batch_year='" + lblBatchYear.Text.Trim() + "' and degree_code ='" + degreeCode + "')  insert into exam_details (degree_code,Exam_Month,Exam_year,batch_year,current_semester,isSupplementaryExam) values ('" + degreeCode + "','11','2016','" + lblBatchYear.Text.Trim() + "','" + CurrentSemester + "','0')";

                    int Ins = d2.update_method_wo_parameter(sqlquery, "Text");

                    NewExamCode = d2.GetFunction("select exam_code from exam_details where exam_Month='11' and exam_year='2016' and batch_year='" + lblBatchYear.Text.Trim() + "' and degree_code ='" + degreeCode + "'");

                    if (OldExamCode.Trim() != "" && OldExamCode.Trim() != "0" && NewExamCode.Trim() != "0" && NewExamCode.Trim() != "")
                    {
                        string SQLUpdatequery = " update  exam_application set exam_code ='" + NewExamCode + "' where exam_code ='" + OldExamCode + "' and roll_no='" + RollNo + "'";
                        int Upd = d2.update_method_wo_parameter(SQLUpdatequery, "Text");

                        Selectquery = "select m.subject_no,s.subject_code from mark_entry M,subject s where s.subject_no=M.subject_no and exam_code ='" + OldExamCode + "' and roll_no='" + RollNo + "'";
                        Dnew = d2.select_method_wo_parameter(Selectquery, "Text");
                        if (Dnew.Tables.Count > 0 && Dnew.Tables[0].Rows.Count > 0)
                        {
                            for (int row = 0; row < Dnew.Tables[0].Rows.Count; row++)
                            {
                                string SubjectNo = d2.GetFunction("select subject_no from syllabus_Master sy,subject s  where s.syll_code =sy.syll_code and batch_year='" + lblBatchYear.Text.Trim() + "' and sy.degree_code ='" + degreeCode + "'  and s.subject_Code ='" + Convert.ToString(Dnew.Tables[0].Rows[row]["subject_code"]) + "'");

                                if (SubjectNo.Trim() != "" && SubjectNo.Trim() != "0")
                                {
                                    SQLUpdatequery = " update mark_entry set exam_code='" + NewExamCode + "' ,subject_no='" + SubjectNo + "'  where exam_code ='" + OldExamCode + "' and roll_no='" + RollNo + "' and subject_no ='" + Convert.ToString(Dnew.Tables[0].Rows[row]["subject_no"]) + "'";
                                    Upd = d2.update_method_wo_parameter(SQLUpdatequery, "Text");
                                }
                            }
                        }
                    }

                    if (result > 0)
                    {
                        isUpdated = true;
                        lblStatusCode.Text = "1";
                        lblStatus.Text = "Imported";
                        gvRows.Cells[3].ForeColor = Color.Green;
                    }
                    else
                    {
                        lblStatusCode.Text = "0";
                        lblStatus.Text = "Not Imported";
                        gvRows.Cells[3].ForeColor = Color.Red;
                    }
                }
            }
            if (isUpdated)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Imported Successfully";
                return;
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Not Imported";
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void imgbtnClose_Click(object sender, EventArgs e)
    {
        divNotSave.Visible = false;
        txtNotSave.Text = string.Empty;
        lblNotSave.Text = string.Empty;
    }

    public void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        divPopAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    public static DataSet Excelconvertdataset(string path)
    {
        DataSet ds3 = new DataSet();
        string StrSheetName = string.Empty;

        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';";
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
            }
        }
        catch
        {

        }
        finally
        {
            if (excelConnection.State != ConnectionState.Closed)
                excelConnection.Close();
        }
        return ds3;
    }

    protected void gvImport_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow row = e.Row;
                Label lblRegNo = (Label)row.FindControl("lblRegNo");
                Label lblBatchYear = (Label)row.FindControl("lblBatchYear");
                Label lblStatus = (Label)row.FindControl("lblStatus");
                Label lblStatusCode = (Label)row.FindControl("lblStatusCode");

                if (!string.IsNullOrEmpty(lblRegNo.Text.Trim()))
                {
                    string available = d2.GetFunctionv("select batch_year from registration where Reg_No='" + lblRegNo.Text.Trim() + "'");
                    if (!string.IsNullOrEmpty(available) && available != "0")
                    {
                        //if (!string.IsNullOrEmpty(lblBatchYear.Text.Trim()) && lblBatchYear.Text.Trim() == available.Trim())
                        //{
                        //    lblStatus.Text = "Imported";
                        //    lblStatusCode.Text = "1";
                        //    e.Row.Cells[3].ForeColor = Color.Green;
                        //}
                        //else
                        //{
                        lblStatus.Text = "Available";
                        lblStatusCode.Text = "2";
                        e.Row.Cells[3].ForeColor = Color.DarkGreen;
                        //}
                    }
                    else
                    {
                        lblStatus.Text = "Not Available";
                        lblStatusCode.Text = "3";
                        e.Row.Cells[3].ForeColor = Color.Red;
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

}