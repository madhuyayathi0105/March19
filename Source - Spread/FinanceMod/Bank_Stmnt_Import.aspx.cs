using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class Bank_Stmnt_Import : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ArrayList ItemEmpty = new ArrayList();
    Hashtable htable = new Hashtable();
    static int isHeaderwise = 0;
    static string lstrcptNo = string.Empty;
    bool check = false;
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
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                //bindbank();
                loadBank();
                generateReceiptNo();
                rb_bank.Checked = true;
                rptprint.Visible = false;
                lb_hdrset.Visible = false;
                headerbind();
                ledgerbind();
            }

            if (ddl_bankname.Items.Count > 0 && ddl_bankname.SelectedItem.Text.Trim().ToUpper() == "OTHERS")
            {
                txt_other.Attributes.Add("style", "display:block;float:left;");
            }
        }
        catch (Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {

    }
    protected void btnimport_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlRcptType.SelectedIndex == 1)
            {
                checkHeaderWiseREceipt();
            }
            Hashtable newhashvalue = new Hashtable();
            using (Stream stream = this.FileUpload1.FileContent as Stream)
            {
                if (FileUpload1.FileName != "" && FileUpload1.FileName != null)
                {
                    poppernew.Visible = false;
                    lb_hdrset.Visible = true;
                    txtfilename.Text = FileUpload1.FileName;
                    stream.Position = 0;
                    this.Fpspread2.OpenExcel(stream);
                    Fpspread2.OpenExcel(stream);
                    Fpspread2.SaveChanges();
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.Sheets[0].Rows.Count = 0;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FarPoint.Web.Spread.TextCellType txtacc = new FarPoint.Web.Spread.TextCellType();

                    if (Fpspread2.Sheets[0].Columns.Count > 0)
                    {
                        Fpspread1.Sheets[0].Columns.Count++;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].Columns.Count - 1].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        for (int row = 0; row < Fpspread2.Sheets[0].ColumnCount; row++)
                        {
                            Fpspread1.Sheets[0].Columns.Count++;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].Columns.Count - 1].Text = Convert.ToString(Fpspread2.Sheets[0].Cells[0, row].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        for (int ro1 = 1; ro1 < Fpspread2.Sheets[0].RowCount; ro1++)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ro1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            for (int col = 0; col < Fpspread2.Sheets[0].ColumnCount; col++)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col + 1].CellType = txtacc;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col + 1].Text = Convert.ToString(Fpspread2.Sheets[0].Cells[ro1, col].Text);
                            }
                        }

                        lbl_error.Visible = false;
                        tblhdr.Visible = true;
                        Fpspread1.Visible = true;
                        div2.Visible = true;
                        rptprint.Visible = true;
                        Fpspread1.Sheets[0].AutoPostBack = true;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread2.Visible = false;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    }
                }
                else
                {
                    poppernew.Visible = false;
                    lb_hdrset.Visible = false;
                    tblhdr.Visible = true;
                    Fpspread1.Visible = false;
                    div2.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select any Excel file then proceed.";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void Cell_Click(object sender, EventArgs e)
    {

    }
    protected void Fpspread1_render(object sender, EventArgs e)
    {

    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        printId.Visible = false;
        DataTable dtReject = new DataTable();
        dtReject.Columns.Add("Reg No");
        bool recptNoOk = false;
        #region Check for Common or Headerwise Receipt Number
        string hdrSetPK = string.Empty;
        if (isHeaderwise == 1 || isHeaderwise == 3)
        {
            if (isHeaderReceipNoOk(out hdrSetPK))
            {
                if (hdrSetPK != string.Empty)
                {
                    recptNoOk = true;
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Select Particualar Header";//"Receipt No Not Assigned For Selected Headers";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Particualar Header"; //"Receipt No Not Assigned For Selected Headers";
            }
        }
        else
        {
            recptNoOk = true;
        }
        #endregion
        if (!recptNoOk)
        {
            return;
        }
        if (ddlRcptType.SelectedIndex == 1)
        {
            checkHeaderWiseREceipt();
        }
        string bankCode = string.Empty;
        if (ddl_bankname.Items.Count > 0 && ddl_bankname.SelectedIndex != 0)
        {
            if (ddl_bankname.SelectedItem.Text.ToUpper() == "OTHERS")
            {
                if (txt_other.Text.Trim() != string.Empty)
                    bankCode = subjectcode("BName", txt_other.Text.Trim());
            }
            else
            {
                bankCode = ddl_bankname.SelectedValue;
            }
        }

        if (bankCode != string.Empty)
        {
            try
            {

                string getrollidx = "";
                string getregidx = "";
                string getadmidx = "";
                string getappnoidx = "";
                string getsemidx = "";
                string gettransdt = "";
                string getrefnoidx = "";
                string gettotamnt = "";
                string getstatus = "";
                string getstuname = "";
                string getcourse = "";
                string getbranch = "";
                string getbatch = "";
                string getaccno = "";
                string getpaymode = "";
                string getcategory = "";

                string rollno = "";
                string regno = "";
                string admno = "";
                string appno = "";
                string semno = "";
                string transdt = "";
                string refno = "";
                string totamnt = "";
                string status = "";
                string stuname = "";
                string course = "";
                string branch = "";
                string batch = "";
                string accno = "";
                string paymode = "";
                string category = "";

                string insquery = "";
                int inscount = 0;
                int insert = 0;
                string updquery = "";
                int upscount = 0;
                int update = 0;
                bool succeed = false;

                Hashtable htab = new Hashtable();
                for (int i = 0; i < lb_hdr.Items.Count; i++)
                {
                    htab.Add(Convert.ToString(lb_hdr.Items[i].Text), i);
                }
                ListBox lb_str = new ListBox();
                ds.Clear();
                string selquery = "select distinct ChlGroupHeader from FS_ChlGroupHeaderSettings";
                ds = d2.select_method_wo_parameter(selquery, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            lb_str.Items.Add(Convert.ToString(ds.Tables[0].Rows[i]["ChlGroupHeader"]));
                        }
                    }
                }
                Hashtable checkhash = new Hashtable();
                string rollcode = "";
                //for (int atrow = 1; atrow < Fpspread1.Sheets[0].RowCount; atrow++)
                //{
                //    //Fpspread1.Sheets[0].Columns.Count++;
                //    //Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(lb_hdr.Items[atrow].Text);
                //    //Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                if (htab.ContainsKey("Roll No."))
                {
                    getrollidx = Convert.ToString(htab[Convert.ToString("Roll No.")]);
                }
                if (htab.ContainsKey("Registration No."))
                {
                    getregidx = Convert.ToString(htab[Convert.ToString("Registration No.")]);
                }
                if (htab.ContainsKey("Admission No."))
                {
                    getadmidx = Convert.ToString(htab[Convert.ToString("Admission No.")]);
                }
                if (htab.ContainsKey("Application No."))
                {
                    getappnoidx = Convert.ToString(htab[Convert.ToString("Application No.")]);
                }
                for (int ro = 1; ro < Fpspread2.Sheets[0].RowCount; ro++)
                {

                    if (getrollidx.Trim() != "")
                    {
                        rollno = Convert.ToString(Fpspread2.Sheets[0].Cells[ro, Convert.ToInt32(getrollidx)].Text);
                    }
                    if (getregidx.Trim() != "")
                    {
                        regno = Convert.ToString(Fpspread2.Sheets[0].Cells[ro, Convert.ToInt32(getregidx)].Text);
                    }
                    if (getadmidx.Trim() != "")
                    {
                        admno = Convert.ToString(Fpspread2.Sheets[0].Cells[ro, Convert.ToInt32(getadmidx)].Text);
                    }
                    if (getappnoidx.Trim() != "")
                    {
                        appno = Convert.ToString(Fpspread2.Sheets[0].Cells[ro, Convert.ToInt32(getappnoidx)].Text);
                    }
                    if (rollno.Trim() != "")
                    {
                        string selroll = "select App_No from Registration where Roll_No='" + rollno + "' and college_code='" + collegecode1 + "'";
                        rollcode = d2.GetFunction(selroll);
                    }
                    else
                        if (regno.Trim() != "")
                        {
                            string selreg = "select App_No from Registration where Reg_No='" + regno + "' and college_code='" + collegecode1 + "'";
                            rollcode = d2.GetFunction(selreg);
                        }
                        else
                            if (admno.Trim() != "")
                            {
                                string seladm = "select App_No from Registration where Roll_Admit='" + admno + "' and college_code='" + collegecode1 + "'";
                                rollcode = d2.GetFunction(seladm);
                            }
                            else
                                if (appno.Trim() != "")
                                {
                                    string selapp = "select App_No from Applyn where app_formno='" + appno + "' and college_code='" + collegecode1 + "'";
                                    rollcode = d2.GetFunction(selapp);
                                }
                                else
                                    if (rollcode.Trim() == "" || rollcode.Trim() == "0")
                                    {
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Please import the valid Roll Number!";
                                        return;
                                    }
                }
                //}


                if (lb_hdr.Items.Count > 0)
                {
                    //Fpspread1.Sheets[0].Columns.Count++;
                    //Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.No";
                    //Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                    for (int atrow = 0; atrow < Fpspread1.Sheets[0].RowCount; atrow++)
                    {
                        string StudNo = string.Empty;
                        bool boolStud = false;
                        //Fpspread1.Sheets[0].Columns.Count++;
                        //Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(lb_hdr.Items[atrow].Text);
                        //Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                        if (htab.ContainsKey("Roll No."))
                        {
                            getrollidx = Convert.ToString(htab[Convert.ToString("Roll No.")]);
                        }
                        if (htab.ContainsKey("Registration No."))
                        {
                            getregidx = Convert.ToString(htab[Convert.ToString("Registration No.")]);
                        }
                        if (htab.ContainsKey("Admission No."))
                        {
                            getadmidx = Convert.ToString(htab[Convert.ToString("Admission No.")]);
                        }
                        if (htab.ContainsKey("Application No."))
                        {
                            getappnoidx = Convert.ToString(htab[Convert.ToString("Application No.")]);
                        }
                        if (htab.ContainsKey("Semester"))
                        {
                            getsemidx = Convert.ToString(htab[Convert.ToString("Semester")]);
                        }
                        if (htab.ContainsKey("Transaction Date"))
                        {
                            gettransdt = Convert.ToString(htab[Convert.ToString("Transaction Date")]);
                        }
                        if (htab.ContainsKey("Bank Reference No."))
                        {
                            getrefnoidx = Convert.ToString(htab[Convert.ToString("Bank Reference No.")]);
                        }
                        if (htab.ContainsKey("Total Amount"))
                        {
                            gettotamnt = Convert.ToString(htab[Convert.ToString("Total Amount")]);
                        }
                        if (htab.ContainsKey("Status"))
                        {
                            getstatus = Convert.ToString(htab[Convert.ToString("Status")]);
                        }
                        if (htab.ContainsKey("Student Name"))
                        {
                            getstuname = Convert.ToString(htab[Convert.ToString("Student Name")]);
                        }
                        if (htab.ContainsKey("Course"))
                        {
                            getcourse = Convert.ToString(htab[Convert.ToString("Course")]);
                        }
                        if (htab.ContainsKey("Branch"))
                        {
                            getbranch = Convert.ToString(htab[Convert.ToString("Branch")]);
                        }
                        if (htab.ContainsKey("Batch"))
                        {
                            getbatch = Convert.ToString(htab[Convert.ToString("Batch")]);
                        }
                        if (htab.ContainsKey("Bank Account No."))
                        {
                            getaccno = Convert.ToString(htab[Convert.ToString("Bank Account No.")]);
                        }
                        if (htab.ContainsKey("Payment Mode"))
                        {
                            getpaymode = Convert.ToString(htab[Convert.ToString("Payment Mode")]);
                        }
                        if (htab.ContainsKey("Category Name"))
                        {
                            getcategory = Convert.ToString(htab[Convert.ToString("Category Name")]);
                        }
                        string groupheaderindex = "";
                        if (lb_str.Items.Count > 0)
                        {
                            for (int ik = 0; ik < lb_str.Items.Count; ik++)
                            {
                                if (htab.ContainsKey(Convert.ToString(lb_str.Items[ik].Text).Trim()))
                                {
                                    groupheaderindex = Convert.ToString(htab[Convert.ToString(lb_str.Items[ik].Text).Trim()]);

                                    if (!checkhash.ContainsKey(Convert.ToString(lb_str.Items[ik].Text).Trim()))
                                    {
                                        checkhash.Add(Convert.ToString(lb_str.Items[ik].Text).Trim(), groupheaderindex);
                                    }
                                }
                            }
                        }
                        if (getrollidx.Trim() != "" && getrollidx.Trim() != null)
                        {
                            rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getrollidx) + 1].Text);
                        }
                        if (getregidx.Trim() != "" && getregidx.Trim() != null)
                        {
                            regno = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getregidx) + 1].Text);
                        }
                        if (getadmidx.Trim() != "" && getadmidx.Trim() != null)
                        {
                            admno = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getadmidx) + 1].Text);
                        }
                        if (getappnoidx.Trim() != "" && getappnoidx.Trim() != null)
                        {
                            appno = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getappnoidx) + 1].Text);
                        }
                        if (getsemidx.Trim() != "" && getsemidx.Trim() != null)
                        {
                            semno = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getsemidx) + 1].Text);
                        }
                        if (gettransdt.Trim() != "" && gettransdt.Trim() != null)
                        {
                            transdt = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(gettransdt) + 1].Text);
                            if (transdt.Trim() != "")
                            {
                                string[] split = transdt.Split('/');
                                transdt = Convert.ToString(split[1] + '/' + split[0] + '/' + split[2]);
                            }
                        }
                        if (getrefnoidx.Trim() != "" && getrefnoidx.Trim() != null)
                        {
                            refno = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getrefnoidx) + 1].Text);
                        }
                        if (gettotamnt.Trim() != "" && gettotamnt.Trim() != null)
                        {
                            totamnt = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(gettotamnt) + 1].Text);
                        }
                        if (getstatus.Trim() != "" && getstatus.Trim() != null)
                        {
                            status = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getstatus) + 1].Text);
                        }
                        if (getstuname.Trim() != "" && getstuname.Trim() != null)
                        {
                            stuname = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getstuname) + 1].Text);
                        }
                        if (getcourse.Trim() != "" && getcourse.Trim() != null)
                        {
                            course = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getcourse) + 1].Text);
                        }
                        if (getbranch.Trim() != "" && getbranch.Trim() != null)
                        {
                            branch = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getbranch) + 1].Text);
                        }
                        if (getbatch.Trim() != "" && getbatch.Trim() != null)
                        {
                            batch = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getbatch) + 1].Text);
                        }
                        if (getaccno.Trim() != "" && getaccno.Trim() != null)
                        {
                            accno = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getaccno) + 1].Text);
                        }
                        if (getpaymode.Trim() != "" && getpaymode.Trim() != null)
                        {
                            paymode = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getpaymode) + 1].Text);
                        }
                        if (getcategory.Trim() != "" && getcategory.Trim() != null)
                        {
                            category = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(getcategory) + 1].Text);
                        }

                        #region App_no

                        if (rollno.Trim() != "")
                        {
                            string selroll = "select App_No from Registration where Roll_No='" + rollno + "' and college_code='" + collegecode1 + "'";
                            rollcode = d2.GetFunction(selroll);
                            StudNo = rollno;
                        }
                        else
                            if (regno.Trim() != "")
                            {
                                string selreg = "select App_No from Registration where Reg_No='" + regno + "' and college_code='" + collegecode1 + "'";
                                rollcode = d2.GetFunction(selreg);
                                StudNo = regno;
                            }
                            else
                                if (admno.Trim() != "")
                                {
                                    string seladm = "select App_No from Registration where Roll_Admit='" + admno + "' and college_code='" + collegecode1 + "'";
                                    rollcode = d2.GetFunction(seladm);
                                    StudNo = admno;
                                }
                                else
                                    if (appno.Trim() != "")
                                    {
                                        string selapp = "select App_No from Applyn where app_formno='" + appno + "' and college_code='" + collegecode1 + "'";
                                        rollcode = d2.GetFunction(selapp);
                                        StudNo = appno;
                                    }

                        #endregion

                        string semcode = "";
                        //string rollcode = "";
                        string finid = d2.getCurrentFinanceYear(usercode, collegecode1);
                        string recptno = generateReceiptNo();
                        if (semno.Trim() != "")
                        {
                            #region Semester Value

                            string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(settingquery, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                                if (linkvalue == "0")
                                {
                                    string semesterquery = "select distinct * from textvaltable where TextCriteria = 'FEECA'and textval='" + semno + " Semester' and textval not like '-1%' and college_code=" + collegecode1 + " order by textval asc";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(semesterquery, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        semcode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                    }
                                }
                                else
                                {
                                    string semesterquery = "select distinct * from textvaltable where TextCriteria = 'FEECA'and textval='" + semno + " Year' and textval not like '-1%' and college_code=" + collegecode1 + " order by textval asc";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(semesterquery, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        semcode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                    }
                                }
                            }

                            #endregion
                        }

                        string filterQ = string.Empty;
                        if (ddlHdrLedger.SelectedIndex == 0)
                        {
                            filterQ = " and A.HeaderFK in (" + GetSelectedItemsValue(cbl_HeaderPop) + ") ";
                        }
                        else
                        {
                            filterQ = " and A.LedgerFK in (" + GetSelectedItemsValue(cbl_ledgerpop) + ") ";
                        }

                        //excess Settings
                        string hedfk = "";
                        string ledfk = "";
                        if (cbexset.Checked == true)
                        {
                            if (ddlexcess.Items.Count > 0)
                            {
                                ledfk = Convert.ToString(ddlexcess.SelectedItem.Value);
                                hedfk = d2.GetFunction("select HeaderPK from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK=h.HeaderPK and l.LedgerPK='" + ledfk + "'");
                            }
                        }
                        bool rcptupdate = false;
                        if (cb_totfee.Checked == false)
                        {
                            #region
                            if (checkhash.Count > 0)
                            {
                                foreach (DictionaryEntry dr in checkhash)
                                {
                                    string key = Convert.ToString(dr.Key);
                                    string value = Convert.ToString(dr.Value);

                                    string spreadval = Convert.ToString(Fpspread1.Sheets[0].Cells[atrow, Convert.ToInt32(value) + 1].Text);
                                    if (spreadval.Trim() != "")
                                    {
                                        double totalpaidamount = Convert.ToDouble(spreadval);
                                        string catquery = "SELECT  distinct A.HeaderFK,HeaderName,A.LedgerFK,LedgerName,FeeAmount,DeductAmout as DeductAmount,TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,PaidAmount,BalAmount,TextVal,TextCode,ChlGroupHeader FROM FT_FeeAllot A,FM_HeaderMaster H,FS_ChlGroupHeaderSettings S, FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK and a.headerfk = s.headerfk and l.headerfk = s.headerfk  AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and h.headerpk = s.headerfk  and l.LedgerMode=0   and ChlGroupHeader in('" + key + "') and FeeCategory='" + semcode + "' and App_No='" + rollcode + "'";
                                        //catquery += filterQ;
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(catquery, "Text");
                                        if (ds.Tables.Count > 0)
                                        {
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                {
                                                    string total = Convert.ToString(ds.Tables[0].Rows[i]["BalAmount"]);
                                                    string hdrfk = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]);
                                                    string ledgefk = Convert.ToString(ds.Tables[0].Rows[i]["LedgerFK"]);
                                                    if (totalpaidamount != 0.00 && total.Trim() != "0.00")
                                                    {
                                                        if (total.Trim() != "")
                                                        {
                                                            double totalpaid = Convert.ToDouble(total);
                                                            if (totalpaidamount >= totalpaid)
                                                            {
                                                                totalpaidamount = totalpaidamount - totalpaid;
                                                                insquery = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected) VALUES('" + Convert.ToDateTime(transdt) + "','" + DateTime.Now.ToLongTimeString() + "','" + recptno + "', 1, '" + rollcode + "', '" + ledgefk + "', '" + hdrfk + "', '" + semcode + "', 0, '" + totalpaid + "', 5, '" + refno + "', '', '" + bankCode + "','', 1, '0', 0, '', '0', '0', '0', 0, '" + usercode + "', '" + finid + "','" + (ddlRcptType.SelectedIndex + 2) + "','1')";
                                                                inscount = d2.update_method_wo_parameter(insquery, "Text");

                                                                if (inscount > 0)
                                                                {
                                                                    insert++;
                                                                    rcptupdate = true;
                                                                }
                                                                updquery = "update FT_FeeAllot set PaidAmount=isnull(PaidAmount,0) +'" + totalpaid + "' ,BalAmount =0,ChlTaken=0  where App_No ='" + rollcode + "' and feecategory ='" + semcode + "' and ledgerfk ='" + ledgefk + "'";
                                                                upscount = d2.update_method_wo_parameter(updquery, "Text");
                                                                if (upscount > 0)
                                                                {
                                                                    update++;
                                                                }
                                                                boolStud = true;
                                                            }
                                                            else
                                                            {
                                                                insquery = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected) VALUES('" + Convert.ToDateTime(transdt) + "','" + DateTime.Now.ToLongTimeString() + "','" + recptno + "', 1, '" + rollcode + "', '" + ledgefk + "', '" + hdrfk + "', '" + semcode + "', 0, '" + totalpaidamount + "', 5, '" + refno + "', '', '" + bankCode + "','', 1, '0', 0, '', '0', '0', '0', 0, '" + usercode + "', '" + finid + "','" + (ddlRcptType.SelectedIndex + 2) + "','1')";
                                                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                                                                if (inscount > 0)
                                                                {
                                                                    insert++;
                                                                    rcptupdate = true;
                                                                }
                                                                updquery = "update FT_FeeAllot set PaidAmount=isnull(PaidAmount,0) +'" + totalpaidamount + "' ,BalAmount =BalAmount-" + totalpaidamount + ", ChlTaken=ChlTaken-" + totalpaidamount + "  where App_No ='" + rollcode + "' and feecategory ='" + semcode + "' and ledgerfk ='" + ledgefk + "'";
                                                                upscount = d2.update_method_wo_parameter(updquery, "Text");


                                                                if (upscount > 0)
                                                                {
                                                                    update++;
                                                                }
                                                                totalpaidamount = 0;
                                                            }
                                                            string UpdateQ = "Update FT_ChallanDet set IsConfirmed ='2' ,RcptTransCode='" + recptno + "',RcptTransDate='" + Convert.ToDateTime(transdt) + "'  where App_No ='" + rollcode + "' and FeeCategory ='" + semcode + "' and HeaderFK ='" + hdrfk + "' and LedgerFK ='" + ledgefk + "'";
                                                            int Updat = d2.update_method_wo_parameter(UpdateQ, "Text");
                                                            if (Updat > 0)
                                                            {
                                                                update++;
                                                            }

                                                            boolStud = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (i == 0 && totalpaidamount == 0.00 && total.Trim() == "0.00")
                                                        {
                                                            insquery = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected) VALUES('" + Convert.ToDateTime(transdt) + "','" + DateTime.Now.ToLongTimeString() + "','" + recptno + "', 1, '" + rollcode + "', '" + ledgefk + "', '" + hdrfk + "', '" + semcode + "', 0, '" + totalpaidamount + "', 5, '" + refno + "', '', '" + bankCode + "','', 1, '0', 0, '', '0', '0', '0', 0, '" + usercode + "', '" + finid + "','" + (ddlRcptType.SelectedIndex + 2) + "','1')";
                                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                                            if (inscount > 0)
                                                            {
                                                                insert++;
                                                                rcptupdate = true;
                                                            }
                                                            updquery = "update FT_FeeAllot set PaidAmount=isnull(PaidAmount,0) +'" + totalpaidamount + "' ,BalAmount =BalAmount-" + totalpaidamount + ", ChlTaken=ChlTaken-" + totalpaidamount + " where App_No ='" + rollcode + "' and feecategory ='" + semcode + "' and ledgerfk ='" + ledgefk + "'";
                                                            upscount = d2.update_method_wo_parameter(updquery, "Text");


                                                            if (upscount > 0)
                                                            {
                                                                update++;
                                                            }
                                                            totalpaidamount = 0;

                                                            string UpdateQ = "Update FT_ChallanDet set IsConfirmed ='2' ,RcptTransCode='" + recptno + "',RcptTransDate='" + Convert.ToDateTime(transdt) + "'  where App_No ='" + rollcode + "' and FeeCategory ='" + semcode + "' and HeaderFK ='" + hdrfk + "' and LedgerFK ='" + ledgefk + "'";
                                                            int Updat = d2.update_method_wo_parameter(UpdateQ, "Text");
                                                            boolStud = true;
                                                        }

                                                    }
                                                }
                                                //excess amount
                                                if (cbexset.Checked == true)
                                                {
                                                    if (totalpaidamount != 0)
                                                    {
                                                        string INsQExcess = "if exists (select * from FT_ExcessDet where App_No='" + rollcode + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "' and ExcessType='1')update FT_ExcessDet set ExcessAmt=ISNULL(ExcessAmt,'0')+'" + totalpaidamount + "',BalanceAmt =ISNULL(BalanceAmt,'0')+'" + totalpaidamount + "' where App_No='" + rollcode + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "' and ExcessType='1' else insert into FT_ExcessDet (ExcessTransDate,TransTime,App_No,MemType,ExcessType,ExcessAmt,BalanceAmt,FeeCategory,FinYearFK)values('" + Convert.ToDateTime(transdt) + "','" + DateTime.Now.ToLongTimeString() + "','" + rollcode + "','1','1','" + totalpaidamount + "','" + totalpaidamount + "','" + semcode + "','" + finid + "')";
                                                        int insUP = d2.update_method_wo_parameter(INsQExcess, "Text");
                                                        string ExdetPK = d2.GetFunction("Select ExcessDetPK from FT_ExcessDet where App_No='" + rollcode + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "' and ExcessType='1' ");
                                                        if (ExdetPK != "0" && ExdetPK != "")
                                                        {
                                                            string INSQExLEdg = " if exists (select * from FT_ExcessLedgerDet where HeaderFK='" + hedfk + "' and LedgerFK='" + ledfk + "' and ExcessDetFK='" + ExdetPK + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "')update FT_ExcessLedgerDet set ExcessAmt=ISNULL(ExcessAmt,'0')+'" + totalpaidamount + "',BalanceAmt =ISNULL(BalanceAmt,'0')+'" + totalpaidamount + "' where HeaderFK='" + hedfk + "' and LedgerFK='" + ledfk + "' and ExcessDetFK='" + ExdetPK + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "' else insert into FT_ExcessLedgerDet(HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FeeCategory,FinYearFK) values('" + hedfk + "','" + ledfk + "','" + totalpaidamount + "','" + totalpaidamount + "','" + ExdetPK + "','" + semcode + "','" + finid + "')";
                                                            int insUPL = d2.update_method_wo_parameter(INSQExLEdg, "Text");

                                                            if (insUP > 0)
                                                                insert++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region

                            if (totamnt.Trim() != "" && semcode.Trim() != "" && rollcode.Trim() != "" && transdt.Trim() != "" && recptno.Trim() != "")
                            {
                                bool save = false;
                                DataSet dsex = new DataSet();
                                double BalAmt = 0;
                                double totalpaidamnt = Convert.ToDouble(totamnt);
                                //total balance amount
                                string balance = d2.GetFunction("SELECT SUM(BalAmount) FROM FT_FeeAllot A,FM_HeaderMaster H, FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode   and l.LedgerMode=0  and FeeCategory='" + semcode + "' and App_No='" + rollcode + "'");
                                double.TryParse(balance, out BalAmt);
                                if (totalpaidamnt > BalAmt)
                                {
                                    if (hedfk != "0" && hedfk != "" && ledfk != "")
                                        save = true;
                                    else
                                        save = false;
                                }
                                else
                                    save = true;

                                if (save == true)
                                {
                                    string selectquery = "SELECT distinct A.HeaderFK,HeaderName,A.LedgerFK,LedgerName,FeeAmount,DeductAmout as DeductAmount,TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,PaidAmount,BalAmount,TextVal,TextCode FROM FT_FeeAllot A,FM_HeaderMaster H, FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode   and l.LedgerMode=0  and FeeCategory='" + semcode + "' and App_No='" + rollcode + "'";
                                    selectquery += filterQ;
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                            {
                                                string total = Convert.ToString(ds.Tables[0].Rows[i]["BalAmount"]);
                                                string hdrfk = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]);
                                                string ledgefk = Convert.ToString(ds.Tables[0].Rows[i]["LedgerFK"]);
                                                if (totalpaidamnt != 0.00 && total.Trim() != "0.00")
                                                {
                                                    if (total.Trim() != "")
                                                    {
                                                        double totalpaid = Convert.ToDouble(total);
                                                        if (totalpaidamnt >= totalpaid)
                                                        {
                                                            totalpaidamnt = totalpaidamnt - totalpaid;
                                                            insquery = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected) VALUES('" + Convert.ToDateTime(transdt) + "','" + DateTime.Now.ToLongTimeString() + "','" + recptno + "', 1, '" + rollcode + "', '" + ledgefk + "', '" + hdrfk + "', '" + semcode + "', 0, '" + totalpaid + "', 5, '" + refno + "', '', '" + bankCode + "','', 1, '0', 0, '', '0', '0', '0', 0, '" + usercode + "', '" + finid + "','" + (ddlRcptType.SelectedIndex + 2) + "','1')";
                                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                                            if (inscount > 0)
                                                            {
                                                                insert++;
                                                                rcptupdate = true;
                                                                //succeed = true;
                                                            }
                                                            updquery = "update FT_FeeAllot set PaidAmount=isnull(PaidAmount,0) +'" + totalpaid + "' ,BalAmount =0  where App_No ='" + rollcode + "' and feecategory ='" + semcode + "' and ledgerfk ='" + ledgefk + "'";
                                                            upscount = d2.update_method_wo_parameter(updquery, "Text");
                                                            if (upscount > 0)
                                                            {
                                                                update++;
                                                                //succeed = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            insquery = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected) VALUES('" + Convert.ToDateTime(transdt) + "','" + DateTime.Now.ToLongTimeString() + "','" + recptno + "', 1, '" + rollcode + "', '" + ledgefk + "', '" + hdrfk + "', '" + semcode + "', 0, '" + totalpaidamnt + "', 5, '" + refno + "', '', '" + bankCode + "','', 1, '0', 0, '', '0', '0', '0', 0, '" + usercode + "', '" + finid + "','" + (ddlRcptType.SelectedIndex + 2) + "','1')";
                                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                                            if (inscount > 0)
                                                            {
                                                                insert++;
                                                                rcptupdate = true;
                                                                //succeed = true;
                                                            }
                                                            updquery = "update FT_FeeAllot set PaidAmount=isnull(PaidAmount,0) +'" + totalpaidamnt + "' ,BalAmount =BalAmount-" + totalpaidamnt + "  where App_No ='" + rollcode + "' and feecategory ='" + semcode + "' and ledgerfk ='" + ledgefk + "'";
                                                            upscount = d2.update_method_wo_parameter(updquery, "Text");
                                                            if (upscount > 0)
                                                            {
                                                                update++;
                                                                //succeed = true;
                                                            }
                                                            totalpaidamnt = 0;
                                                        }
                                                    }
                                                }
                                            }
                                            //excess
                                            if (cbexset.Checked == true)
                                            {
                                                if (totalpaidamnt != 0)
                                                {
                                                    string INsQExcess = "if exists (select * from FT_ExcessDet where App_No='" + rollcode + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "' and ExcessType='1')update FT_ExcessDet set ExcessAmt=ISNULL(ExcessAmt,'0')+'" + totalpaidamnt + "',BalanceAmt =ISNULL(BalanceAmt,'0')+'" + totalpaidamnt + "' where App_No='" + rollcode + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "' and ExcessType='1' else insert into FT_ExcessDet (ExcessTransDate,TransTime,App_No,MemType,ExcessType,ExcessAmt,BalanceAmt,FeeCategory,FinYearFK)values('" + Convert.ToDateTime(transdt) + "','" + DateTime.Now.ToLongTimeString() + "','" + rollcode + "','1','1','" + totalpaidamnt + "','" + totalpaidamnt + "','" + semcode + "','" + finid + "')";
                                                    int insUP = d2.update_method_wo_parameter(INsQExcess, "Text");
                                                    string ExdetPK = d2.GetFunction("Select ExcessDetPK from FT_ExcessDet where App_No='" + rollcode + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "' and ExcessType='1' ");
                                                    if (ExdetPK != "0" && ExdetPK != "")
                                                    {
                                                        string INSQExLEdg = " if exists (select * from FT_ExcessLedgerDet where HeaderFK='" + hedfk + "' and LedgerFK='" + ledfk + "' and ExcessDetFK='" + ExdetPK + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "')update FT_ExcessLedgerDet set ExcessAmt=ISNULL(ExcessAmt,'0')+'" + totalpaidamnt + "',BalanceAmt =ISNULL(BalanceAmt,'0')+'" + totalpaidamnt + "' where HeaderFK='" + hedfk + "' and LedgerFK='" + ledfk + "' and ExcessDetFK='" + ExdetPK + "' and FeeCategory='" + semcode + "' and FinYearFK='" + finid + "' else insert into FT_ExcessLedgerDet(HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FeeCategory,FinYearFK) values('" + hedfk + "','" + ledfk + "','" + totalpaidamnt + "','" + totalpaidamnt + "','" + ExdetPK + "','" + semcode + "','" + finid + "')";
                                                        int insUPL = d2.update_method_wo_parameter(INSQExLEdg, "Text");

                                                        if (insUP > 0)
                                                            insert++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Visible = true;
                                    lblalerterr.Text = "You Have Excess Amount Please Provide Setting";
                                    return;
                                }
                            }
                            #endregion
                        }
                        //string lastRecptNo = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where FinYearFK=" + finid + ")");
                        //lastRecptNo = recptno.Replace(lastRecptNo, "");
                        //string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";

                        if (rcptupdate)
                        {
                            string lastRecptNo = lstrcptNo;
                            string updateRecpt = string.Empty;
                            if (isHeaderwise == 0 || isHeaderwise == 2)
                            {
                                updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                            }
                            else
                            {
                                updateRecpt = "update FM_HeaderFinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where HeaderSettingPK=" + hdrSetPK + " and FinyearFK=" + finid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1'";
                            }
                            d2.update_method_wo_parameter(updateRecpt, "Text");
                        }
                        // d2.update_method_wo_parameter(updateRecpt, "Text");
                        if (!boolStud)
                        {
                            DataRow dr = dtReject.NewRow();
                            dr["Reg No"] = StudNo;
                            dtReject.Rows.Add(dr);
                        }
                    }
                    if (insert > 0 || update > 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Saved Successfully";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "already import these records";
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Please select Fields in Header Settings!";
                }
            }
            catch
            {

            }
            loadBank();
            if (dtReject.Rows.Count > 0)
            {
                Session["dsDt"] = dtReject;
                printId.Visible = true;
            }
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Text = "Please provide bank name!";
        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        Fpspread1.Visible = false;
        txtfilename.Text = "";
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void rb_bank_OnCheckedChanged(object sender, EventArgs e)
    {
        lb_hdrset.Visible = false;
        Fpspread1.Visible = false;
        txtfilename.Text = "";
    }
    protected void rb_atm_OnCheckedChanged(object sender, EventArgs e)
    {
        lb_hdrset.Visible = false;
        Fpspread1.Visible = false;
        txtfilename.Text = "";
    }
    protected void lb_hdr_click(object sender, EventArgs e)
    {
        if (Fpspread1.Visible == false)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Text = "Please Import the Excel!";
        }
        else
        {
            alertpopwindow.Visible = false;
            lblalerterr.Visible = false;
            poppernew.Visible = true;
            tblhdr.Visible = false;
            div2.Visible = false;
            rptprint.Visible = false;
            lb_selecthdr.Items.Clear();
            lb_selecthdr.Items.Add("Bank Reference No.");
            lb_selecthdr.Items.Add("Transaction Date");
            lb_selecthdr.Items.Add("Total Amount");
            lb_selecthdr.Items.Add("Status");
            lb_selecthdr.Items.Add("Application No.");
            lb_selecthdr.Items.Add("Roll No.");
            lb_selecthdr.Items.Add("Admission No.");
            lb_selecthdr.Items.Add("Registration No.");
            lb_selecthdr.Items.Add("Student Name");
            lb_selecthdr.Items.Add("Course");
            lb_selecthdr.Items.Add("Branch");
            lb_selecthdr.Items.Add("Batch");
            lb_selecthdr.Items.Add("Semester");
            lb_selecthdr.Items.Add("Bank Account No.");
            //lb_selecthdr.Items.Add("Challan for hostel master");
            //lb_selecthdr.Items.Add("Challan for my account");
            //lb_selecthdr.Items.Add("Challan1");
            //lb_selecthdr.Items.Add("Change Challan");
            //lb_selecthdr.Items.Add("my header name");
            lb_selecthdr.Items.Add("Payment Mode");
            lb_selecthdr.Items.Add("Category Name");


            ds.Clear();
            string selquery = "select distinct ChlGroupHeader from FS_ChlGroupHeaderSettings";
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        lb_selecthdr.Items.Add(Convert.ToString(ds.Tables[0].Rows[i]["ChlGroupHeader"]));
                    }
                }
            }
        }
        //for (int i = 0; i < lb_selecthdr.Items.Count; i++)
        //{
        //    htab.Add(lb_selecthdr.Items[i].Text, i);
        //}
    }
    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            tblhdr.Visible = false;
            div2.Visible = false;
            rptprint.Visible = false;
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
            tblhdr.Visible = false;
            div2.Visible = false;
            rptprint.Visible = false;
            if (lb_selecthdr.Items.Count > 0)
            {
                for (int j = 0; j < lb_selecthdr.Items.Count; j++)
                {
                    lb_hdr.Items.Add(lb_selecthdr.Items[j].Text.ToString());
                }
            }
        }
        catch { }
    }
    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        try
        {
            tblhdr.Visible = false;
            div2.Visible = false;
            rptprint.Visible = false;
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
            tblhdr.Visible = false;
            div2.Visible = false;
            rptprint.Visible = false;
            lb_hdr.Items.Clear();
        }
        catch { }
    }
    protected void btnok_click(object sender, EventArgs e)
    {
        if (lb_hdr.Items.Count > 0)
        {
            poppernew.Visible = false;
            tblhdr.Visible = true;
            div2.Visible = true;
            rptprint.Visible = true;
            alertpopwindow.Visible = false;
            lblalerterr.Visible = false;
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Text = "Please select atleast one header then proceed!";
        }
    }
    protected void btnclose_click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
        tblhdr.Visible = true;
        div2.Visible = true;
        rptprint.Visible = true;
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
        tblhdr.Visible = true;
        div2.Visible = true;
    }
    public void bindbank()
    {
        try
        {
            ds.Clear();
            string selbank = "select BankPK,(BankName+' - '+AccNo) as BankName from FM_FinBankMaster where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(selbank, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_bankname.Items.Clear();
                    ddl_bankname.DataSource = ds;
                    ddl_bankname.DataTextField = "BankName";
                    ddl_bankname.DataValueField = "BankPK";
                    ddl_bankname.DataBind();
                }
            }
        }
        catch
        {

        }
    }
    public string generateReceiptNoOld()
    {
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where FinYearFK=" + finYearid + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;


                int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
            }
            return recno;
        }
        catch { return recno; }
    }
    //Code Added by Idhris - 19-03-2016
    protected void ddlHdrLedger_Indexchanged(object sender, EventArgs e)
    {
        try
        {
            upheader.Visible = false;
            upledger.Visible = false;
            if (ddlHdrLedger.SelectedIndex == 0)
            {
                upheader.Visible = true;
                for (int i = 0; i < cbl_HeaderPop.Items.Count; i++)
                {
                    cbl_HeaderPop.Items[i].Selected = true;
                }
                txt_HeaderPop.Text = "Header(" + cbl_HeaderPop.Items.Count + ")";
                cb_HeaderPop.Checked = true;
            }
            else
            {
                upledger.Visible = true;
                for (int i = 0; i < cbl_ledgerpop.Items.Count; i++)
                {
                    cbl_ledgerpop.Items[i].Selected = true;
                }
                txt_Ledgerpop.Text = "Ledger(" + cbl_ledgerpop.Items.Count + ")";
                cb_ledgerpop.Checked = true;
            }
        }
        catch { }
    }
    protected void cb_HeaderPop_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_HeaderPop, cb_HeaderPop, txt_HeaderPop, "Header");
        ledgerbind();
    }
    protected void cbl_HeaderPop_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_HeaderPop, cb_HeaderPop, txt_HeaderPop, "Header");
        ledgerbind();
    }
    public void headerbind()
    {
        try
        {
            txt_HeaderPop.Text = "Header";
            cb_HeaderPop.Checked = false;
            cbl_HeaderPop.Items.Clear();

            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "   ";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_HeaderPop.DataSource = ds;
                cbl_HeaderPop.DataTextField = "HeaderName";
                cbl_HeaderPop.DataValueField = "HeaderPK";
                cbl_HeaderPop.DataBind();
                for (int i = 0; i < cbl_HeaderPop.Items.Count; i++)
                {
                    cbl_HeaderPop.Items[i].Selected = true;
                }
                txt_HeaderPop.Text = "Header(" + cbl_HeaderPop.Items.Count + ")";
                cb_HeaderPop.Checked = true;
            }

        }
        catch { }
    }
    protected void cb_ledgerpop_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_ledgerpop, cb_ledgerpop, txt_Ledgerpop, "Ledger");
    }
    protected void cbl_ledgerpop_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_ledgerpop, cb_ledgerpop, txt_Ledgerpop, "Ledger");
    }
    public void ledgerbind()
    {
        try
        {
            txt_Ledgerpop.Text = "Ledger";
            cb_ledgerpop.Checked = false;

            cbl_ledgerpop.Items.Clear();
            string query = "SELECT  LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and l.LedgerMode=0   AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " order by len(isnull(l.priority,1000)) , l.priority asc";

            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ledgerpop.DataSource = ds;
                cbl_ledgerpop.DataTextField = "LedgerName";
                cbl_ledgerpop.DataValueField = "LedgerPK";
                cbl_ledgerpop.DataBind();
                for (int i = 0; i < cbl_ledgerpop.Items.Count; i++)
                {
                    cbl_ledgerpop.Items[i].Selected = true;
                }
                txt_Ledgerpop.Text = "Ledger(" + cbl_ledgerpop.Items.Count + ")";
                cb_ledgerpop.Checked = true;
            }
        }
        catch
        {
        }
    }
    #region excess setting

    protected void cbexcess_ChekedChanged(object sender, EventArgs e)
    {
        // CallCheckBoxChangedEvent(cblexcess, cbexcess, txtexcess, "Ledger");
    }
    protected void cblexcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        // CallCheckBoxListChangedEvent(cblexcess, cbexcess, txtexcess, "Ledger");
    }
    public void exledgerbind()
    {
        try
        {
            //  txtexcess.Text = "Ledger";
            //  cb_ledgerpop.Checked = false;

            ddlexcess.Items.Clear();
            string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and l.LedgerMode=0   AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " ";

            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlexcess.DataSource = ds;
                ddlexcess.DataTextField = "LedgerName";
                ddlexcess.DataValueField = "LedgerPK";
                ddlexcess.DataBind();
                //for (int i = 0; i < ddlexcess.Items.Count; i++)
                //{
                //    ddlexcess.Items[i].Selected = true;
                //}
                //  txtexcess.Text = "Ledger(" + ddlexcess.Items.Count + ")";
                // cbexcess.Checked = true;
            }
        }
        catch
        {
        }
    }

    protected void cb_totfee_Changed(object sender, EventArgs e)
    {
        if (cb_totfee.Checked == true)
        {
            cbexset.Enabled = true;
            cbexset.Checked = false;
            cbexset_Changed(sender, e);
        }
        else
        {
            cbexset.Enabled = false;
            cbexset.Checked = false;
            cbexset_Changed(sender, e);
        }
    }

    protected void cbexset_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbexset.Checked == true)
            {
                ddlexcess.Enabled = true;
                exledgerbind();
            }
            else
            {
                ddlexcess.Enabled = false;
                ddlexcess.Items.Clear();
            }
        }
        catch { }
    }
    #endregion
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }
    public void loadBank()
    {
        try
        {
            txt_other.Text = string.Empty;
            ddl_bankname.Items.Clear();
            string queru = "select TextCode,TextVal  from textvaltable where TextCriteria = 'BName'";
            DataSet dsBank = d2.select_method_wo_parameter(queru, "Text");

            if (dsBank.Tables[0].Rows.Count > 0)
            {
                ddl_bankname.DataSource = dsBank;
                ddl_bankname.DataTextField = "TextVal";
                ddl_bankname.DataValueField = "TextCode";
                ddl_bankname.DataBind();
            }
            ddl_bankname.Items.Insert(0, "Select");
            ddl_bankname.Items.Insert(ddl_bankname.Items.Count, "Others");
        }
        catch (Exception ex) { }
    }
    //public void loadBank()
    //{
    //    try
    //    {
    //        txt_other.Text = string.Empty;
    //        ddl_bankname.Items.Clear();
    //        string query = "select distinct BankPK,BankName from FM_FinBankMaster where CollegeCode=" + collegecode1  + "";
    //        DataSet dsBank = d2.select_method_wo_parameter(query, "Text");

    //        if (dsBank.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_bankname.DataSource = dsBank;
    //            ddl_bankname.DataTextField = "BankName";
    //            ddl_bankname.DataValueField = "BankPK";
    //            ddl_bankname.DataBind();
    //        }
    //        ddl_bankname.Items.Insert(0, "Select");
    //        ddl_bankname.Items.Insert(ddl_bankname.Items.Count, "Others");
    //    }
    //    catch (Exception ex) { }
    //}
    public string subjectcode(string textcri, string subjename)
    {
        //for new bank
        string subjec_no = "";
        try
        {
            string select_subno = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode1 + " and TextVal='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
            }
            else
            {
                string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode1 + " and TextVal='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                    }
                }
            }
        }
        catch (Exception ex) { }
        return subjec_no;
    }
    //Added by Idhris For Header wise receiptno generation 01-11-2016
    private void checkHeaderWiseREceipt()
    {
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
        }
        catch { isHeaderwise = 0; }

        if (isHeaderwise > 0)
        {
            try
            {
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                DataSet dsHdrsChk = new DataSet();

                dsHdrsChk = d2.select_method_wo_parameter("select HeaderFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1' and FinyearFK=" + finYearid + "  select Headerpk from FM_HeaderMaster H,FS_HeaderPrivilage P  where H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " and h.CollegeCode=" + collegecode1 + " ", "");

                if (dsHdrsChk.Tables.Count > 1)
                {
                    string uHdr = string.Empty;

                    for (int uhdr = 0; uhdr < dsHdrsChk.Tables[1].Rows.Count; uhdr++)
                    {
                        if (uHdr == string.Empty)
                        {
                            uHdr = Convert.ToString(dsHdrsChk.Tables[1].Rows[uhdr][0]);
                        }
                        else
                        {
                            uHdr += "," + Convert.ToString(dsHdrsChk.Tables[1].Rows[uhdr][0]);
                        }
                    }
                    dsHdrsChk.Tables[0].DefaultView.RowFilter = " headerfk in (" + uHdr + ")";
                    DataView dv = dsHdrsChk.Tables[0].DefaultView;
                    if (dv.Count != dsHdrsChk.Tables[1].Rows.Count)
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Receipt No Not Set For All Headers";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Receipt No Not Set For All Headers";
                }
            }
            catch (Exception ex) { }
        }

        //Last modified by Idhris -- 03-08-2016
    }
    public string generateReceiptNo()
    {
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
        }
        catch { isHeaderwise = 0; }
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 5)
                return string.Empty;
        }
        catch { return string.Empty; }
        if (isHeaderwise == 0 || isHeaderwise == 2)
        {
            return getCommonReceiptNo();
        }
        else
        {
            return getHeaderwiseReceiptNo();
        }
    }
    private string getCommonReceiptNo()
    {
        string recno = string.Empty;
        lstrcptNo = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;


                int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
                lstrcptNo = Convert.ToString(receno);
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    private string getHeaderwiseReceiptNo()
    {
        string recno = string.Empty;
        lstrcptNo = string.Empty;

        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            string isheaderFk = GetSelectedItemsValue(cbl_HeaderPop);

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

            DataSet dsFinHedDet = d2.select_method_wo_parameter("select distinct HeaderSettingFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode1 + " and FinyearFK=" + finYearid + " and isnull(onlineRec,'')<>'1'", "Text");

            if (dsFinHedDet.Tables.Count > 0 && dsFinHedDet.Tables[0].Rows.Count == 1 && ddlRcptType.SelectedIndex == 1)
            {
                string secondreciptqurey = "select * from FM_HeaderFinCodeSettings where HeaderSettingPK =" + Convert.ToString(dsFinHedDet.Tables[0].Rows[0][0]) + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1'";
                DataSet dsrecYr = new DataSet();
                dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
                if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
                {
                    recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptStNo"]);
                    if (recnoprev != "")
                    {
                        int recno_cur = Convert.ToInt32(recnoprev);
                        receno = recno_cur;
                    }
                    recacr = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptAcr"]);

                    int size = Convert.ToInt32(dsrecYr.Tables[0].Rows[0]["Rcptsize"]);

                    string recenoString = receno.ToString();

                    if (size != recenoString.Length && size > recenoString.Length)
                    {
                        while (size != recenoString.Length)
                        {
                            recenoString = "0" + recenoString;
                        }
                    }
                    recno = recacr + recenoString;
                    lstrcptNo = Convert.ToString(receno);
                }
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    private bool isHeaderReceipNoOk(out string hspk)
    {
        bool headerOK = false;
        hspk = string.Empty;
        try
        {
            string isheaderFk = GetSelectedItemsValue(cbl_HeaderPop);

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

            DataSet dsFinHedDet = d2.select_method_wo_parameter("select distinct HeaderSettingFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode1 + " and FinyearFK=" + finYearid + " and isnull(onlineRec,'')<>'1'", "Text");

            if (dsFinHedDet.Tables.Count > 0 && dsFinHedDet.Tables[0].Rows.Count == 1 && ddlRcptType.SelectedIndex == 1)
            {
                hspk = Convert.ToString(dsFinHedDet.Tables[0].Rows[0][0]).Trim();
                headerOK = true;
            }
        }
        catch (Exception ex) { headerOK = false; }
        return headerOK;
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
        if (lbl_alert.Text == "Receipt No Not Set For All Headers")
        {
            Response.Redirect("~/Finance.aspx");
        }
    }

    //last modified sudhagar 22.08.2017
    void ExportToExcel(DataTable dt, string FileName)
    {
        if (dt.Rows.Count > 0)
        {
            string filename = FileName + ".xls";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            dgGrid.DataSource = dt;
            dgGrid.DataBind();

            //Get the HTML for the control.
            dgGrid.RenderControl(hw);
            //Write the HTML back to the browser.
            //Response.ContentType = application/vnd.ms-excel;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition",
                                  "attachment; filename=" + filename + "");
            this.EnableViewState = false;
            Response.Write(tw.ToString());
            Response.End();
        }
    }

    protected void btndownload_Click(object sender, EventArgs e)
    {
        // get();
        if (Session["dsDt"] != null)
        {
            DataTable dt = (DataTable)Session["dsDt"];
            ExportToExcel(dt, "UploadFile");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Session Expired')", true);
        }
    }
    protected void get()
    {
        DataTable dtReject = new DataTable();
        dtReject.Columns.Add("reg No");
        DataRow dr = dtReject.NewRow();
        dr["Reg No"] = "1111";
        dtReject.Rows.Add(dr);
        Session["dsDt"] = dtReject;
    }
}