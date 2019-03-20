using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Configuration;

public partial class TranserUpdate : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods reuse = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int isHeaderwise = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {

            }
        }
        catch (Exception ex) { }
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion

    #region transfer update
    protected void btnTrans_Click(object sender, EventArgs e)
    {
        try
        {
            updateTransfer();
           

        }
        catch { }
    }

    protected void updateTransfer()
    {
        try
        {
            bool boolSave = false;
            string selQ = "  select distinct r.app_no,r.stud_name,r.roll_no,r.reg_no,r.roll_admit,r.batch_year,st.FromDegree,st.Todegree,st.FromSection,st.ToSection,st.FromCollege,st.Tocollege,st.fromseattype,st.ToSeatType,convert(varchar(10),st.TransferDate,103)as TransferDate,convert(varchar(10),r.adm_date,103)as adm_date,Old_RollNo,Old_RegNo,Old_RollAdmit,Old_ReceiptNo,convert(varchar(10),Old_ReceiptDate,103)as Old_ReceiptDate,Old_Amt,New_ReceiptNo,convert(varchar(10),New_ReceiptDate,103)as New_ReceiptDate,New_Amt,New_ExcessAmt from registration r,st_student_transfer st,ST_Student_Transfer_Details std where r.app_no=st.appno and st.StudentTransferPK=std.StudentTransferfK ";
            DataSet dsTrans = d2.select_method_wo_parameter(selQ, "Text");
            if (dsTrans.Tables.Count > 0 && dsTrans.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsTrans.Tables[0].Rows.Count; row++)
                {
                    string appNo = Convert.ToString(dsTrans.Tables[0].Rows[row]["app_no"]);
                    string batch = Convert.ToString(dsTrans.Tables[0].Rows[row]["batch_year"]);
                    string toDegree = Convert.ToString(dsTrans.Tables[0].Rows[row]["Todegree"]);
                    string toSection = Convert.ToString(dsTrans.Tables[0].Rows[row]["ToSection"]);
                    string toCollege = Convert.ToString(dsTrans.Tables[0].Rows[row]["Tocollege"]);
                    string toSeatType = Convert.ToString(dsTrans.Tables[0].Rows[row]["ToSeatType"]);
                    string transDate = Convert.ToString(dsTrans.Tables[0].Rows[row]["TransferDate"]);
                    transDate = transDate.Split('/')[1] + "/" + transDate.Split('/')[0] + "/" + transDate.Split('/')[2];
                    if (!string.IsNullOrEmpty(appNo))
                    {
                        string updQ = " update ft_feeallot set istransfer='1' where app_no='" + appNo + "' and istransfer='0' and allotdate<'" + transDate + "'";
                        int updQw = d2.update_method_wo_parameter(updQ, "Text");
                        boolSave = true;
                        //    string selGen = "  select feeamount,totalamount,feecategory,headerfk,ledgerfk from ft_feeallotdegree where batchyear='" + batch + "' and degreecode='" + toDegree + "' and seattype='" + toSeatType + "' and isnull(ishostelfees,'0')='0'";
                        //    selGen += "  select * from ft_feeallot where app_no='" + appNo + "' and istransfer='0'";
                        //    DataSet dsGen = d2.select_method_wo_parameter(selGen, "Text");
                        //    if (dsGen.Tables.Count > 0 && dsGen.Tables[0].Rows.Count > 0)
                        //    {
                        //        for (int gen = 0; gen < dsGen.Tables[0].Rows.Count; gen++)
                        //        {
                        //            string hdFK = Convert.ToString(dsGen.Tables[0].Rows[gen]["headerfk"]);
                        //            string ldFK = Convert.ToString(dsGen.Tables[0].Rows[gen]["ledgerfk"]);
                        //            string feecaT = Convert.ToString(dsGen.Tables[0].Rows[gen]["feecategory"]);
                        //            double feeAmt = 0;
                        //            double totalAmt = 0;
                        //            double.TryParse(Convert.ToString(dsGen.Tables[0].Rows[gen]["feeamount"]), out feeAmt);
                        //            double.TryParse(Convert.ToString(dsGen.Tables[0].Rows[gen]["totalamount"]), out totalAmt);
                        //            string str = "headerfk='" + hdFK + "' and ledgerfk='" + ldFK + "' and feecategory='" + feecaT + "'";
                        //            // dsGen.Tables[1].DefaultView.RowFilter = 
                        //        }
                        //    }
                    }
                }
            }
            if (boolSave)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Not Updated')", true);
        }
        catch { }
    }
    #endregion





    protected string getAppNo(string applNo)
    {
        string appNo = string.Empty;
        try
        {
            appNo = d2.GetFunction("select app_no from applyn where app_formno='" + applNo + "'");
        }
        catch { appNo = "0"; }
        return appNo;

    }
    public static DataSet Excelconvertdataset(string path)
    {
        DataSet ds3 = new DataSet();
        string StrSheetName = "";

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
}