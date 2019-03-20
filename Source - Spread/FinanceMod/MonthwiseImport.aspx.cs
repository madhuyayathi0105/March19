using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Data.OleDb;
using System.Text;
using System.Configuration;

public partial class MonthwiseImport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string groupcode = string.Empty;
    Hashtable hsgetmon = new Hashtable();

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
            collegecode = Convert.ToString(Session["collegecode"]);
            usercode = Session["usercode"].ToString();
            groupcode = Convert.ToString(Session["group_code"]);

            if (!IsPostBack)
            {
                bindhdr();
                headerbind();
                ledgerbind();
            }
        }
        catch (Exception ex) { }
    }

    protected void lb3_Click(object sender, EventArgs e)
    {

    }

    protected bool chkHeader()
    {
        if (lb_hdr.Items.Count > 0)
        {
            alertpopwindow.Visible = false;
            return true;
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Please Select Header";
            return false;
        }
    }

    private string getLinkval()
    {
        string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
        return linkvalue;
    }
    private string getFinYear()
    {
        string finYearid = string.Empty;
        try
        {
            finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
        }
        catch { }
        return finYearid;
    }

    private bool checkLedgerIndx(out int feeCat, out int rollIndx, out int regIdx, out int Yearidx)
    {
        feeCat = -1;
        rollIndx = -1;
        regIdx = -1;
        Yearidx = -1;
        for (int i = 0; i < lb_hdr.Items.Count; i++)
        {
            if (lb_hdr.Items[i].Text.ToUpper() == "FEE CATEGORY")
            {
                feeCat = i;
            }
            else if (lb_hdr.Items[i].Text.ToUpper() == "ROLL NO")
            {
                rollIndx = i;
            }
            else if (lb_hdr.Items[i].Text.ToUpper() == "REGISTRATION NO")
            {
                regIdx = i;
            }
            else if (lb_hdr.Items[i].Text.ToUpper() == "YEAR")
            {
                Yearidx = i;
            }
        }
        if (feeCat == -1 || (rollIndx == -1 && regIdx == -1) || Yearidx == -1)
        {
            return false;
        }
        return true;
    }

    protected void btnimport_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkHeader())
            {
                checkalldet();
            }
        }
        catch
        {

        }
    }

    private static DataTable ConvertExcelToDataTable(string FileName)
    {
        DataTable dtResult = null;
        int totalSheet = 0;
        using (OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';"))
        {
            objConn.Open();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string sheetName = string.Empty;
            if (dt != null)
            {
                var tempDataTable = (from dataRow in dt.AsEnumerable()
                                     where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                     select dataRow).CopyToDataTable();
                dt = tempDataTable;
                totalSheet = dt.Rows.Count;
                sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
            }
            cmd.Connection = objConn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
            oleda = new OleDbDataAdapter(cmd);
            oleda.Fill(ds, "excelData");
            dtResult = ds.Tables["excelData"];
            objConn.Close();
            return dtResult;
        }
    }

    public bool chkhdr()
    {
        bindmonhdr();
        bool chk = false;
        try
        {
            if (lb_hdr.Items.Count > 0)
            {
                for (int k = 0; k < lb_hdr.Items.Count; k++)
                {
                    if (hsgetmon.ContainsValue(lb_hdr.Items[k].Value))
                    {
                        chk = true;
                    }
                }
            }
        }
        catch
        {
        }
        return chk;
    }

    public void checkalldet()
    {
        try
        {
            if (chkhdr() == true)
            {
                int feecatIndx = -1;
                int RollIndx = -1;
                int RegIdx = -1;
                int nameidx = -1;
                int yearidx = -1;
                string linkvalue = getLinkval();

                if (checkLedgerIndx(out feecatIndx, out RollIndx, out RegIdx, out yearidx))
                {
                    if (FileUpload1.HasFile)
                    {
                        if (System.IO.Path.GetExtension(FileUpload1.FileName) == ".xlsx" || System.IO.Path.GetExtension(FileUpload1.FileName) == ".xls")
                        {
                            string fname = Server.MapPath("~/BankStatement/ImpExcel" + System.IO.Path.GetExtension(FileUpload1.FileName));
                            FileUpload1.SaveAs(fname);
                            DataTable importedtbl = ConvertExcelToDataTable(fname);
                            if (importedtbl.Columns.Count >= lb_hdr.Items.Count)
                            {
                                if (importedtbl.Rows.Count > 0)
                                {
                                    string feecat = string.Empty;
                                    string rollno = string.Empty;
                                    string regno = string.Empty;
                                    string name = string.Empty;
                                    string year = string.Empty;
                                    string semYear = string.Empty;
                                    string roll_no = string.Empty;
                                    string appNo = string.Empty;
                                    string reg_no = string.Empty;
                                    string stud_name = string.Empty;
                                    string stryear = string.Empty;

                                    for (int ro = 0; ro < importedtbl.Rows.Count; ro++)
                                    {
                                        for (int i = 0; i < lb_hdr.Items.Count; i++)
                                        {
                                            if (feecatIndx == i)
                                            {
                                                feecat = Convert.ToString(importedtbl.Rows[ro][i]);
                                                if (feecat.Trim() != "" || feecat.Trim() != "0")
                                                {
                                                    //if (linkvalue == "1")
                                                    //{
                                                    if (feecat.ToUpper().Contains("SEMESTER") || feecat.ToUpper().Contains("YEAR"))
                                                    {
                                                        semYear = d2.GetFunction("Select Textcode from textvaltable where TextCriteria ='FEECA' and textval = '" + feecat + "' and college_code=" + collegecode + " ");
                                                        Session["feecat"] = semYear;
                                                    }
                                                    //else
                                                    //{
                                                    //    semYear = feecat + " Semester";
                                                    //    semYear = d2.GetFunction("Select Textcode from textvaltable where TextCriteria ='FEECA' and textval = '" + semYear + "' and college_code=" + collegecode + " ");
                                                    //}
                                                    //}
                                                    //else
                                                    //{
                                                    //    if (semYear.Contains("Year"))
                                                    //    {
                                                    //        semYear = d2.GetFunction("Select Textcode from textvaltable where TextCriteria ='FEECA' and textval = '" + feecat + "' and college_code=" + collegecode + " ");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        semYear = d2.GetFunction("Select Textcode from textvaltable where TextCriteria ='FEECA' and textval = '" + returnYearforSem(semYear) + "' and college_code=" + collegecode + " ");
                                                    //    }
                                                    //}
                                                    if (semYear.Trim() == "0" || semYear == "")
                                                    {
                                                        alertpopwindow.Visible = true;
                                                        lblalerterr.Text = "FeeCategory '" + feecat + "' is Invalid in Excel!";
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Text = "FeeCategory does not Exist in Excel!";
                                                    return;
                                                }
                                            }
                                            else if (RollIndx == i)
                                            {
                                                rollno = Convert.ToString(importedtbl.Rows[ro][i]).Trim();
                                                roll_no = rollno;
                                                if (rollno.Trim() == "" || rollno.Trim() == "0")
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Text = "Roll No does not Exist in Excel!";
                                                    return;
                                                }

                                                appNo = d2.GetFunction("select app_no from Registration where Roll_no='" + rollno + "'");
                                                Session["appno"] = appNo;
                                                if (appNo.Trim() == "" || appNo.Trim() == "0")
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Text = "Roll Number '" + rollno + "' is Invalid in Excel!";
                                                    return;
                                                }
                                            }
                                            else if (RegIdx == i)
                                            {
                                                regno = Convert.ToString(importedtbl.Rows[ro][i]).Trim();
                                                reg_no = regno;
                                                if (regno.Trim() == "" || regno.Trim() == "0")
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Text = "Reg No does not Exist in Excel!";
                                                    return;
                                                }

                                                appNo = d2.GetFunction("select app_no from Registration where Reg_No='" + regno + "'");
                                                Session["appno"] = appNo;
                                                if (appNo.Trim() == "" || appNo.Trim() == "0")
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Text = "Reg Number '" + regno + "' is Invalid in Excel!";
                                                    return;
                                                }
                                            }
                                            else if (nameidx == i)
                                            {
                                                string stname = Convert.ToString(importedtbl.Rows[ro][i]).Trim();
                                                stud_name = stname;
                                                if (stname.Trim() == "" || stname.Trim() == "0")
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Text = "Student Name does not Exist in Excel!";
                                                    return;
                                                }
                                            }
                                            else if (yearidx == i)
                                            {
                                                string yearval = Convert.ToString(importedtbl.Rows[ro][i]).Trim();
                                                stryear = yearval;
                                                if (yearval.Trim() == "" || yearval.Trim() == "0")
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Text = "Year does not Exist in Excel!";
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                string value = Convert.ToString(importedtbl.Rows[ro][i]).Trim();
                                                if (value == "" && value == "0")
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Text = "Amount does not given in Excel!";
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    SaveImportedData(importedtbl, feecatIndx, RollIndx, RegIdx, yearidx);
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Text = "No Rows Found in Excel!";
                                    return;
                                }
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Headers Count does not Match!";
                                return;
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Invalid File!";
                            return;
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Please Select A File!";
                        return;
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Roll No/Reg No,FeeCategory,Year Should Be Selected!";
                    return;
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please select all the month Given in Excel!";
                return;
            }
        }
        catch
        {

        }
    }

    protected void SaveImportedData(DataTable dtData, int feecatIndx, int rollNoIndx, int RegIdx, int YearIdx)
    {
        try
        {
            string linkvalue = getLinkval();
            double impCnt = 0;
            double faiCnt = 0;
            double rollnotcnt = 0;
            string finYearId = getFinYear();
            string headerid = Convert.ToString(ddlHeader.SelectedItem.Value);
            string Ledgerid = Convert.ToString(ddlLedger.SelectedItem.Value);
            string skipval = "";
            string alertstr = " No value Imported for ";
            if (finYearId != string.Empty && finYearId != "0")
            {
                for (int k = 0; k < dtData.Rows.Count; k++)
                {
                    StringBuilder queryCashTrans = new StringBuilder();
                    string appNo = string.Empty;
                    string feecat = "0";
                    string roll_no = string.Empty;
                    string reg_no = string.Empty;
                    string year = string.Empty;
                    string strval = string.Empty;
                    string strnewval = string.Empty;
                    string strvalamnt = string.Empty;
                    string feeamntyear = string.Empty;
                    string feeamntmon = string.Empty;
                    string feeamntval = string.Empty;
                    string feeamountval = "";
                    string[] splitamnt = new string[15];
                    string[] splnewamnt = new string[3];
                    string[] splval = new string[15];
                    string[] splamnt = new string[5];
                    double totamnt = 0;
                    double totval = 0;
                    double addedamnt = 0;
                    Double dstotval = 0;
                    DataSet dschk = new DataSet();
                    Dictionary<string, string> dictadd = new Dictionary<string, string>();
                    Dictionary<string, string> dicttot = new Dictionary<string, string>();
                    bool entryflag = false;

                    for (int j = 0; j < lb_hdr.Items.Count; j++)
                    {
                        int validx = 0;
                        string valtext = "";
                        if (lb_hdr.Items[j].Text != "S.No")
                        {
                            ListItem ls = new ListItem(lb_hdr.Items[j].Text, lb_hdr.Items[j].Value);
                            int field = lb_selecthdr.Items.IndexOf(ls);
                            string dtype = lb_hdrDataType.Items[field].Value.ToString();
                            if (rollNoIndx == j)
                            {
                                string rollno = Convert.ToString(dtData.Rows[k][j]).Trim();
                                roll_no = rollno;
                                appNo = d2.GetFunction("select app_no from Registration where Roll_no='" + rollno + "'");
                            }
                            else if (RegIdx == j)
                            {
                                string regno = Convert.ToString(dtData.Rows[k][j]).Trim();
                                reg_no = regno;
                                appNo = d2.GetFunction("select app_no from Registration where Reg_No='" + regno + "'");
                            }
                            else if (YearIdx == j)
                            {
                                string YearVal = Convert.ToString(dtData.Rows[k][j]).Trim();
                                year = YearVal;
                            }
                            else if (feecatIndx == j)
                            {
                                string semYear = Convert.ToString(dtData.Rows[k][j]).Trim();
                                //if (linkvalue == "1")
                                //{
                                if (semYear.ToUpper().Contains("SEMESTER") || semYear.ToUpper().Contains("YEAR"))
                                {
                                    semYear = d2.GetFunction("Select Textcode from textvaltable where TextCriteria ='FEECA' and textval = '" + semYear + "' and college_code=" + collegecode + " ");
                                }
                                //else
                                //{
                                //    semYear += " Semester";
                                //    semYear = d2.GetFunction("Select Textcode from textvaltable where TextCriteria ='FEECA' and textval = '" + semYear + "' and college_code=" + collegecode + " ");
                                //}
                                //}
                                //else
                                //{
                                //    if (semYear.Contains("Year"))
                                //    {
                                //        semYear = d2.GetFunction("Select Textcode from textvaltable where TextCriteria ='FEECA' and textval = '" + semYear + "' and college_code=" + collegecode + " ");
                                //    }
                                //    else
                                //    {
                                //        semYear = d2.GetFunction("Select Textcode from textvaltable where TextCriteria ='FEECA' and textval = '" + returnYearforSem(semYear) + "' and college_code=" + collegecode + " ");
                                //    }
                                //}
                                feecat = semYear;
                            }
                            else
                            {
                                string value = Convert.ToString(dtData.Rows[k][j]).Trim().ToUpper();
                                validx = Convert.ToInt32(lb_hdr.Items[j].Value);
                                valtext = Convert.ToString(lb_hdr.Items[j].Text);

                                string selq = d2.GetFunction("select FeeAmountMonthly from FT_FeeAllot where FeeCategory='" + feecat + "' and App_No='" + appNo + "' and HeaderFK='" + headerid + "' and LedgerFK='" + Ledgerid + "' and PayMode='2'");

                                if (dtype == "numeric" || dtype == "float" || dtype == "real" || dtype == "bigint" || dtype == "bit" || dtype == "smallint" || dtype == "decimal" || dtype == "smallmoney" || dtype == "int" || dtype == "tinyint" || dtype == "money")
                                {
                                    if (value == "" || value == "0")
                                    {
                                        value = "0";
                                    }
                                    else
                                    {
                                        try
                                        {
                                            Convert.ToDouble(value);
                                            double.TryParse(value, out totval);

                                            if (selq.Trim() != "" && selq.Trim() != "0")
                                            {
                                                if (entryflag == false)
                                                {
                                                    splitamnt = selq.Split(',');
                                                    if (splitamnt.Length > 0)
                                                    {
                                                        for (int ro = 0; ro < splitamnt.Length; ro++)
                                                        {
                                                            if (splitamnt[ro].Contains(":"))
                                                            {
                                                                splnewamnt = splitamnt[ro].Split(':');
                                                                if (splnewamnt.Length >= 3)
                                                                {
                                                                    feeamntmon = Convert.ToString(splnewamnt[0]);
                                                                    feeamntyear = Convert.ToString(splnewamnt[1]);
                                                                    feeamountval = Convert.ToString(splnewamnt[2]);
                                                                    Double.TryParse(feeamountval, out dstotval);

                                                                    if (!dictadd.ContainsKey(feeamntmon))
                                                                    {
                                                                        dictadd.Add(feeamntmon, (Convert.ToString(feeamntmon) + ":" + Convert.ToString(feeamntyear) + ":" + Convert.ToString(feeamountval)));
                                                                        if (strnewval.Trim() == "")
                                                                        {
                                                                            strnewval = "" + Convert.ToString(feeamntmon) + ":" + Convert.ToString(feeamntyear) + ":" + Convert.ToString(feeamountval) + "";
                                                                            totamnt = totamnt + dstotval;
                                                                        }
                                                                        else
                                                                        {
                                                                            strnewval = strnewval + "," + Convert.ToString(feeamntmon) + ":" + Convert.ToString(feeamntyear) + ":" + Convert.ToString(feeamountval);
                                                                            totamnt = totamnt + dstotval;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    entryflag = true;
                                                }
                                                if (!dictadd.ContainsKey(Convert.ToString(validx)))
                                                {
                                                    if (!dicttot.ContainsKey(Convert.ToString(validx)))
                                                    {
                                                        dicttot.Add(Convert.ToString(validx), (Convert.ToString(validx) + ":" + Convert.ToString(year) + ":" + Convert.ToString(value)));
                                                        if (strval.Trim() == "")
                                                        {
                                                            strval = "" + Convert.ToString(validx) + ":" + Convert.ToString(year) + ":" + Convert.ToString(value) + "";
                                                            totamnt = totamnt + totval;
                                                        }
                                                        else
                                                        {
                                                            strval = strval + "," + Convert.ToString(validx) + ":" + Convert.ToString(year) + ":" + Convert.ToString(value);
                                                            totamnt = totamnt + totval;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (strval.Trim() == "")
                                                {
                                                    strval = "" + Convert.ToString(validx) + ":" + Convert.ToString(year) + ":" + Convert.ToString(value) + "";
                                                    totamnt = totamnt + totval;
                                                }
                                                else
                                                {
                                                    strval = strval + "," + Convert.ToString(validx) + ":" + Convert.ToString(year) + ":" + Convert.ToString(value);
                                                    totamnt = totamnt + totval;
                                                }
                                            }
                                        }
                                        catch { value = "0"; }
                                    }
                                }
                                else
                                {
                                    if (dtype == "datetime")
                                    {
                                        try
                                        {
                                            Convert.ToDateTime(value);
                                        }
                                        catch { value = System.DateTime.Now.Date.ToString(); }
                                    }
                                    if (value == "")
                                    {
                                        value = "''";
                                    }
                                    else
                                    {
                                        value = "'" + value + "'";
                                    }
                                }
                            }
                        }
                    }
                    if (entryflag == true)
                    {
                        if (strval.Trim() == "")
                        {
                            strval = strnewval;
                        }
                        else
                        {
                            strval = strval + "," + strnewval;
                        }
                    }

                    if (strval.Trim() == "")
                    {
                        if (skipval.Trim() == "")
                        {
                            if (roll_no.Trim() != "")
                            {
                                skipval = Convert.ToString(roll_no);
                            }
                            if (reg_no.Trim() != "")
                            {
                                skipval = Convert.ToString(reg_no);
                            }
                        }
                        else
                        {
                            if (roll_no.Trim() != "")
                            {
                                skipval = skipval + "," + Convert.ToString(roll_no);
                            }
                            if (reg_no.Trim() != "")
                            {
                                skipval = skipval + "," + Convert.ToString(reg_no);
                            }
                        }
                    }

                    try
                    {
                        if (appNo.Trim() != "0")
                        {
                            string updq = "";
                            string insq = "";
                            int upcount = 0;
                            int incount = 0;

                            string selq = d2.GetFunction("select FeeAmountMonthly from FT_FeeAllot where FeeCategory='" + feecat + "' and App_No='" + appNo + "' and HeaderFK='" + headerid + "' and LedgerFK='" + Ledgerid + "' and PayMode='2' and FinYearFK='" + finYearId + "'");

                            if (selq.Trim() != "" && selq.Trim() != "0")
                            {
                                #region  for unwanted
                                //splitamnt = selq.Split(',');
                                //if (splitamnt.Length > 0)
                                //{
                                //    for (int ro = 0; ro < splitamnt.Length; ro++)
                                //    {
                                //        splnewamnt = splitamnt[ro].Split(':');
                                //        if (splnewamnt.Length >= 3)
                                //        {
                                //            feeamntmon = Convert.ToString(splnewamnt[0]);
                                //            feeamntyear = Convert.ToString(splnewamnt[1]);
                                //            feeamountval = Convert.ToString(splnewamnt[2]);
                                //            if (hsgetval.ContainsKey(feeamntmon))
                                //            {
                                //                strvalamnt = Convert.ToString(hsgetval[Convert.ToString(feeamntmon)]);
                                //                if (strvalamnt.Trim() != "")
                                //                {
                                //                    if (strnewval.Trim() == "")
                                //                    {
                                //                        strnewval = "" + strvalamnt + "";
                                //                    }
                                //                    else
                                //                    {
                                //                        strnewval = strnewval + "," + strvalamnt;
                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    goto label;
                                //                }
                                //            }
                                //            else
                                //            {
                                //                if (strnewval.Trim() == "")
                                //                {
                                //                    strnewval = "" + Convert.ToString(feeamntmon) + ":" + Convert.ToString(feeamntyear) + ":" + Convert.ToString(feeamountval) + "";
                                //                }
                                //                else
                                //                {
                                //                    strnewval = strnewval + "," + Convert.ToString(feeamntmon) + ":" + Convert.ToString(feeamntyear) + ":" + Convert.ToString(feeamountval);
                                //                }
                                //                goto label1;
                                //            }
                                //        label:
                                //            if (!hsgetval.ContainsKey(feeamntmon))
                                //            {
                                //                foreach (DictionaryEntry dr in hsgetval)
                                //                {
                                //                    string key = Convert.ToString(dr.Key);
                                //                    string value = Convert.ToString(dr.Value);
                                //                    if (hsgetval.ContainsKey(key))
                                //                    {
                                //                        if (strnewval.Trim() == "")
                                //                        {
                                //                            strnewval = "" + Convert.ToString(value) + "";
                                //                        }
                                //                        else
                                //                        {
                                //                            strnewval = strnewval + "," + Convert.ToString(value);
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //            continue;
                                //        label1:
                                //            continue;
                                //        }
                                //    }
                                //}
                                #endregion
                                updq = "update FT_FeeAllot set FeeAmountMonthly='" + strval + "',FeeAmount='" + totamnt + "',TotalAmount='" + totamnt + "',BalAmount='" + totamnt + "',PayMode='2' where FeeCategory='" + feecat + "' and App_No='" + appNo + "' and HeaderFK='" + headerid + "' and LedgerFK='" + Ledgerid + "' and FinYearFK='" + finYearId + "'";
                                upcount = d2.update_method_wo_parameter(updq, "Text");
                            }
                            else
                            {
                                insq = "INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "','1','" + appNo + "','" + Ledgerid + "','" + headerid + "','" + totamnt + "','0','0','0','" + totamnt + "','0','1','" + strval + "','2','" + feecat + "','','0','','0','0','" + totamnt + "','" + finYearId + "')";
                                incount = d2.update_method_wo_parameter(insq, "Text");
                            }


                            //Fee Allot Monthly
                            string getvalues = "";
                            string feeallotpk = "";
                            string feeamnt = "";
                            if (upcount > 0)
                            {
                                getvalues = "select FeeAllotPK,FeeAmountMonthly,FeeAmount from FT_FeeAllot where FeeCategory='" + feecat + "' and App_No='" + appNo + "' and HeaderFK='" + headerid + "' and LedgerFK='" + Ledgerid + "'";
                            }
                            else if (incount > 0)
                            {
                                getvalues = "select top 1 FeeAllotPK,FeeAmountMonthly,FeeAmount from FT_FeeAllot where FeeCategory='" + feecat + "' and App_No='" + appNo + "' and HeaderFK='" + headerid + "' and LedgerFK='" + Ledgerid + "' order by FeeAllotPK desc";
                            }

                            ds.Clear();
                            ds = d2.select_method_wo_parameter(getvalues, "Text");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    feeallotpk = Convert.ToString(ds.Tables[0].Rows[0]["FeeAllotPK"]);
                                    feeamnt = Convert.ToString(ds.Tables[0].Rows[0]["FeeAmount"]);
                                    if (Convert.ToString(ds.Tables[0].Rows[0]["FeeAmountMonthly"]).Trim() != "")
                                    {
                                        splval = Convert.ToString(ds.Tables[0].Rows[0]["FeeAmountMonthly"]).Split(',');
                                    }
                                }
                            }

                            if (splval.Length > 0)
                            {
                                for (int iv = 0; iv < splval.Length; iv++)
                                {
                                    splamnt = splval[iv].Split(':');
                                    if (splamnt.Length > 0)
                                    {
                                        queryCashTrans.Append(" if exists (select * from FT_FeeallotMonthly where AllotMonth='" + Convert.ToString(splamnt[0]) + "' and AllotYear='" + Convert.ToString(splamnt[1]) + "' and FeeAllotPK='" + feeallotpk + "') update FT_FeeallotMonthly set AllotAmount='" + Convert.ToString(splamnt[2]) + "',BalAmount='" + Convert.ToString(splamnt[2]) + "' where AllotMonth='" + Convert.ToString(splamnt[0]) + "' and AllotYear='" + Convert.ToString(splamnt[1]) + "'  and FeeAllotPK='" + feeallotpk + "' and FinYearFK='" + finYearId + "' else insert into FT_FeeallotMonthly (FeeAllotPK,AllotMonth,AllotYear,AllotAmount,BalAmount,FinYearFK) values ('" + feeallotpk + "','" + Convert.ToString(splamnt[0]) + "','" + Convert.ToString(splamnt[1]) + "','" + Convert.ToString(splamnt[2]) + "','" + Convert.ToString(splamnt[2]) + "','" + finYearId + "')");
                                        d2.update_method_wo_parameter(queryCashTrans.ToString(), "Text");
                                    }
                                }
                            }

                            if (upcount > 0 || incount > 0)
                            {
                                impCnt++;
                                strval = "";
                                strnewval = "";
                            }
                            else
                            {
                                faiCnt++;
                            }
                            dictadd.Clear();
                            dicttot.Clear();
                        }
                        else
                        {
                            roll_no.Trim();
                            rollnotcnt++;
                        }
                    }
                    catch { faiCnt++; }
                }
                if (skipval.Trim() == "")
                {
                    lblalerterr.Text = "Imported :" + impCnt + "; Failed:" + faiCnt + "; Invalid Number:" + rollnotcnt;
                }
                else
                {
                    lblalerterr.Text = "Imported :" + impCnt + "; Failed:" + faiCnt + "; Invalid Number:" + rollnotcnt + ";" + alertstr + skipval;
                }
                alertpopwindow.Visible = true;
            }
            else
            {
                lblalerterr.Text = "Please Select Finance Year";
                alertpopwindow.Visible = true;
            }
        }
        catch { }
    }

    protected void bindmonhdr()
    {
        hsgetmon.Clear();
        hsgetmon.Add("Jan", "1");
        hsgetmon.Add("Feb", "2");
        hsgetmon.Add("Mar", "3");
        hsgetmon.Add("Apr", "4");
        hsgetmon.Add("May", "5");
        hsgetmon.Add("Jun", "6");
        hsgetmon.Add("July", "7");
        hsgetmon.Add("Aug", "8");
        hsgetmon.Add("Sep", "9");
        hsgetmon.Add("Oct", "10");
        hsgetmon.Add("Nov", "11");
        hsgetmon.Add("Dec", "12");
    }

    protected void bindhdr()
    {
        alertpopwindow.Visible = false;
        lb_selecthdr.Items.Clear();
        lb_hdrDataType.Items.Clear();
        lb_selecthdr.Items.Add(new ListItem("Roll No", "Roll_No"));
        lb_hdrDataType.Items.Add("nvarchar");
        lb_selecthdr.Items.Add(new ListItem("Registration No", "Reg_No"));
        lb_hdrDataType.Items.Add("nvarchar");
        lb_selecthdr.Items.Add(new ListItem("Student Name", "Stud_Name"));
        lb_hdrDataType.Items.Add("nvarchar");
        lb_selecthdr.Items.Add(new ListItem("Fee Category", "FeeCategory"));
        lb_hdrDataType.Items.Add("int");
        lb_selecthdr.Items.Add(new ListItem("Year", "Year"));
        lb_hdrDataType.Items.Add("int");
        lb_selecthdr.Items.Add(new ListItem("Jan", "1"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("Feb", "2"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("Mar", "3"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("Apr", "4"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("May", "5"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("Jun", "6"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("July", "7"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("Aug", "8"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("Sep", "9"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("Oct", "10"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("Nov", "11"));
        lb_hdrDataType.Items.Add("decimal");
        lb_selecthdr.Items.Add(new ListItem("Dec", "12"));
        lb_hdrDataType.Items.Add("decimal");

    }

    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1 Year";
                break;
            case "3":
            case "4":
                cursem = "2 Year";
                break;
            case "5":
            case "6":
                cursem = "3 Year";
                break;
            case "7":
            case "8":
                cursem = "4 Year";
                break;
            case "9":
            case "10":
                cursem = "5 Year";
                break;
        }
        return cursem;
    }

    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lb_selecthdr.Items.Count > 0 && lb_selecthdr.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_hdr.Items.Count; j++)
                {
                    if (lb_hdr.Items[j].Value == lb_selecthdr.SelectedItem.Value)
                    {
                        ok = false;
                    }

                }
                if (ok)
                {
                    ListItem lst = new ListItem(lb_selecthdr.SelectedItem.Text, lb_selecthdr.SelectedItem.Value);
                    lb_hdr.Items.Add(lst);
                }
            }
        }
        catch { }
    }

    protected void btnMvTwoRt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_hdr.Items.Clear();
            if (lb_selecthdr.Items.Count > 0)
            {
                for (int j = 0; j < lb_selecthdr.Items.Count; j++)
                {
                    lb_hdr.Items.Add(new ListItem(lb_selecthdr.Items[j].Text.ToString(), lb_selecthdr.Items[j].Value.ToString()));
                }
            }
        }
        catch { }
    }

    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        try
        {
            if (lb_hdr.Items.Count > 0 && lb_hdr.SelectedItem.Value != "")
            {
                lb_hdr.Items.RemoveAt(lb_hdr.SelectedIndex);
            }
        }
        catch { }
    }

    protected void btnMvTwoLt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_hdr.Items.Clear();
        }
        catch { }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void ddlHeader_Change(object sender, EventArgs e)
    {
        ledgerbind();
    }

    public void headerbind()
    {
        try
        {
            ddlHeader.Items.Clear();
            ds.Clear();
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P,FT_FeeAllot f WHERE H.HeaderPK=f.HeaderFK and H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND f.PayMode='2' and P. UserCode = '" + usercode + "' AND H.CollegeCode = '" + collegecode + "' order by H.HeaderPK,H.HeaderName";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlHeader.DataSource = ds;
                ddlHeader.DataTextField = "HeaderName";
                ddlHeader.DataValueField = "HeaderPK";
                ddlHeader.DataBind();
            }
            else
            {
                ddlHeader.Items.Add(new ListItem("Select", "0"));
            }
            ledgerbind();
        }
        catch { }
    }

    public void ledgerbind()
    {
        try
        {
            ddlLedger.Items.Clear();
            ds.Clear();

            string headerid = Convert.ToString(ddlHeader.SelectedItem.Value);
            string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P,FT_FeeAllot f WHERE f.LedgerFK=L.LedgerPK and PayMode='2' and L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and l.LedgerMode=0 and L.HeaderFK='" + headerid + "' AND P. UserCode = '" + usercode + "' AND L.CollegeCode = '" + collegecode + "' ";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlLedger.DataSource = ds;
                ddlLedger.DataTextField = "LedgerName";
                ddlLedger.DataValueField = "LedgerPK";
                ddlLedger.DataBind();
            }
            else
            {
                ddlLedger.Items.Add(new ListItem("Select", "0"));
            }
        }
        catch
        {
        }
    }
}