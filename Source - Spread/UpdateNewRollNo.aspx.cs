using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.IO;

public partial class UpdateNewRollNo : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_update_click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds1 = new DataSet();
            using (Stream stream = this.FileUpload1.FileContent as Stream)
            {
                string extension = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (extension.Trim() != "")
                {
                    if (System.IO.Path.GetExtension(FileUpload1.FileName) == ".xls" || System.IO.Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                    {
                        
                        string path = Server.MapPath("~/Importfiles/" + System.IO.Path.GetFileName(FileUpload1.FileName));
                        //string path = System.IO.Path.GetFileName(FileUpload1.FileName);
                        FileUpload1.SaveAs(path);
                        ds1.Clear();
                        ds1 = Excelconvertdataset(path);
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            int count = 0;
                            bool isRegNocol = false;
                            bool issubcodecol = false;
                            for (int colu = 0; colu < ds1.Tables[0].Columns.Count; colu++)
                            {
                                string newcolname = ds1.Tables[0].Columns[colu].ColumnName.Trim().ToLower();
                                if (newcolname == "old_roll_no")
                                {
                                    count++;
                                    isRegNocol = true;
                                }
                                else if (newcolname == "new_roll_no")
                                {
                                    count++;
                                    issubcodecol = true;
                                }
                            }
                            if (count == 2 && isRegNocol & issubcodecol)
                            {
                                foreach (DataRow dt in ds1.Tables[0].Rows)
                                {
                                    string oldRollNo = Convert.ToString(dt["old_roll_no"]);
                                    string NewRollNo = Convert.ToString(dt["New_Roll_no"]);
                                    Hashtable hat = new Hashtable();
                                    hat.Add("@OldRollNo", oldRollNo);
                                    hat.Add("@NewRollNo", NewRollNo);
                                    d2.update_method_with_parameter("StudentRollNoUpdate", hat, "sp");
                                }
                            }
                            else
                            {
                                Label1.Visible = true;
                                Label1.Text = "Invaild Column Name";
                            }
                        }
                    }
                    else
                    {
                        Label1.Visible = true;
                        Label1.Text = "Browse XLS files only";
                    }
                }
                    Label1.Visible = true;
                    Label1.Text = "Saved Successfully..!";
               
            }
        }
        catch
        {

        }

       
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
                adapter.Dispose();
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
}