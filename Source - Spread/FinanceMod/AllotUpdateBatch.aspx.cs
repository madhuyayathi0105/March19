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

public partial class AllotUpdateBatch : System.Web.UI.Page
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
                loadcollege();
                if (ddlcollegename.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
                bindBtch();
                loadheaderandledger();
                ledgerload();
            }
        }
        catch (Exception ex) { }
    }
    public void loadcollege()
    {
        ddlcollegename.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollegename);
    }
    public void bindBtch()
    {
        try
        {
            ddlyear.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlyear.DataSource = ds;
                ddlyear.DataTextField = "batch_year";
                ddlyear.DataValueField = "batch_year";
                ddlyear.DataBind();
            }
        }
        catch { }
    }
    #region headerandledger
    public void loadheaderandledger()
    {
        try
        {
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            chkl_studhed.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  ";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderPK";
                chkl_studhed.DataBind();
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;
                }
                txt_studhed.Text = lblheader.Text + "(" + chkl_studhed.Items.Count + ")";
                chk_studhed.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void ledgerload()
    {
        try
        {
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            chkl_studled.Items.Clear();
            string hed = "";
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    if (hed == "")
                    {
                        hed = chkl_studhed.Items[i].Value.ToString();
                    }
                    else
                    {
                        hed = hed + "','" + "" + chkl_studhed.Items[i].Value.ToString() + "";
                    }
                }
            }


            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studled.DataSource = ds;
                chkl_studled.DataTextField = "LedgerName";
                chkl_studled.DataValueField = "LedgerPK";
                chkl_studled.DataBind();
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                }
                txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
                chk_studled.Checked = true; ;

            }
            else
            {
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = false;
                }
                txt_studled.Text = "--Select--";
                chk_studled.Checked = false; ;
            }

        }
        catch
        {
        }
    }
    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        ledgerload();
    }

    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        ledgerload();
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
    }
    #endregion
    protected void ddlcollegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollegename.Items.Count > 0)
            collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
        bindBtch();
        loadheaderandledger();
        ledgerload();
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        try
        {
            System.Text.StringBuilder SBroll = new System.Text.StringBuilder();
            string batch = Convert.ToString(ddlyear.SelectedItem.Value);
            string collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            string hdFK = Convert.ToString(getCblSelectedValue(chkl_studhed));
            string ldFK = Convert.ToString(getCblSelectedValue(chkl_studled));
            bool boolroll = false;
            string roll = Convert.ToString(txtroll.Text);
            if (!string.IsNullOrEmpty(roll) && roll != "0")
            {
                string[] splroll = roll.Split(',');
                if (splroll.Length > 0)
                {
                    for (int i = 0; i < splroll.Length; i++)
                    {
                        SBroll.Append(splroll[i] + ",");
                    }
                }
                if (SBroll.Length > 0)
                {
                    SBroll.Remove(SBroll.Length - 1, 1);
                    boolroll = true;
                }
            }

            if (!string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdFK) && !string.IsNullOrEmpty(ldFK))
            {
                string selQ = "     select sum(totalamount) as tot,sum(paidamount) as paid,sum(balamount) as bal,feecategory,ledgerfk,f.app_no,degree_code,batch_year from ft_feeallot f ,registration r where r.app_no=f.app_no  and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.batch_year in('" + batch + "')  and r.college_code='" + collegecode + "' and f.headerfk in ('" + hdFK + "') and f.ledgerFK in('" + ldFK + "') ";
                if (boolroll)
                    selQ += " and f.app_no in ('" + SBroll.ToString() + "')";
                selQ += " group by feecategory,ledgerfk,f.app_no,degree_code,batch_year having sum(isnull(totalamount,'0')) =sum(isnull(paidamount,'0')) and sum(isnull(totalamount,'0')) =sum(isnull(balamount,'0')) and sum(isnull(totalamount,'0'))<>'0'";//and f.app_no='13875'
                selQ += "   select sum(debit) as tot,feecategory,ledgerfk,f.app_no,degree_code,batch_year from ft_findailytransaction f,registration r where r.app_no=f.app_no  and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.batch_year in('" + batch + "')  and r.college_code='" + collegecode + "' and f.headerfk in ('" + hdFK + "') and f.ledgerFK in('" + ldFK + "') and isnull(transcode,'')<>'' and isnull(iscanceled,'0')='0' and memtype='1' ";
                if (boolroll)
                    selQ += " and f.app_no in ('" + SBroll.ToString() + "')";
                selQ += " group by feecategory,ledgerfk,f.app_no,degree_code,batch_year having sum(isnull(debit,'0'))<>'0' ";//and f.app_no='13875'
                ds.Clear();
                ds = d2.select_method_wo_parameter(selQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    bool check = false;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "app_no='" + ds.Tables[0].Rows[row]["app_no"] + "' and feecategory='" + ds.Tables[0].Rows[row]["feecategory"] + "' and ledgerfk='" + ds.Tables[0].Rows[row]["ledgerfk"] + "' and degree_code='" + ds.Tables[0].Rows[row]["degree_code"] + "' and batch_year='" + ds.Tables[0].Rows[row]["batch_year"] + "'";
                            DataView dv = ds.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                string updQ = " update ft_feeallot set paidamount=totalamount, balamount='0' where app_no='" + ds.Tables[0].Rows[row]["app_no"] + "' and feecategory='" + ds.Tables[0].Rows[row]["feecategory"] + "' and ledgerfk='" + ds.Tables[0].Rows[row]["ledgerfk"] + "'";
                                int upd = d2.update_method_wo_parameter(updQ, "Text");
                                check = true;
                            }
                        }
                    }
                    if (check)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('updated Successfully')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not updated')", true);
                    }
                }
            }
        }
        catch { }
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

    #region mcc update
    protected void btnmccUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolSave = false;
            Dictionary<string, string> dtUpdate = new Dictionary<string, string>();
            //  dtUpdate.Add("2015", "3547");//5 th semester
            dtUpdate.Add("2016", "3545");//3rd semester
            foreach (KeyValuePair<string, string> UpdateValue in dtUpdate)
            {
                string selQ = " select distinct sum(totalamount) as tot,sum(paidamount) as paid,sum(balamount) as bal,sum(debit) as totpaid,fa.feecategory,fa.app_no,ft.ledgerfk from registration r,ft_feeallot fa,ft_findailytransaction ft  where fa.app_no=r.app_no and ft.app_no=fa.app_no and ft.app_no=r.app_no and ft.feecategory=fa.feecategory and ft.headerfk=fa.headerfk and ft.ledgerfk=fa.ledgerfk  and batch_year ='" + UpdateValue.Key + "' and fa.feecategory in('" + UpdateValue.Value + "') and cc=0 and delflag=0 and exam_flag<>'Debar' and college_code='13' and ft.paymode in(4,5)  group by fa.feecategory,fa.app_no,ft.ledgerfk having sum(paidamount)<>sum(debit)";//and r.app_no in('57049','57267')
                DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                    {

                        string selD = "       select App_No,LedgerFK,HeaderFK,FeeCategory,TransCode,debit,COUNT(transcode) from FT_FinDailyTransaction where app_no='" + Convert.ToString(dsVal.Tables[0].Rows[row]["app_no"]) + "' and ledgerfk='" + Convert.ToString(dsVal.Tables[0].Rows[row]["ledgerfk"]) + "' and feecategory='" + Convert.ToString(dsVal.Tables[0].Rows[row]["feecategory"]) + "' group by App_No,LedgerFK,HeaderFK,FeeCategory,TransCode,debit having COUNT(transcode)>1";
                        DataSet dsDel = d2.select_method_wo_parameter(selD, "Text");
                        if (dsDel.Tables.Count > 0 && dsDel.Tables[0].Rows.Count > 0)
                        {
                            string transPK = d2.GetFunction("select top 1 Dailytranspk from FT_FinDailyTransaction where app_no='" + Convert.ToString(dsDel.Tables[0].Rows[0]["app_no"]) + "' and feecategory='" + Convert.ToString(dsDel.Tables[0].Rows[0]["feecategory"]) + "' and ledgerfk='" + Convert.ToString(dsDel.Tables[0].Rows[0]["ledgerfk"]) + "' and TransCode ='" + Convert.ToString(dsDel.Tables[0].Rows[0]["TransCode"]) + "' and debit='" + Convert.ToString(dsDel.Tables[0].Rows[0]["debit"]) + "'");
                            string DelQ = " delete from FT_FinDailyTransaction where app_no='" + Convert.ToString(dsDel.Tables[0].Rows[0]["app_no"]) + "' and feecategory='" + Convert.ToString(dsDel.Tables[0].Rows[0]["feecategory"]) + "' and ledgerfk='" + Convert.ToString(dsDel.Tables[0].Rows[0]["ledgerfk"]) + "' and TransCode ='" + Convert.ToString(dsDel.Tables[0].Rows[0]["TransCode"]) + "' and debit='" + Convert.ToString(dsDel.Tables[0].Rows[0]["debit"]) + "' and  Dailytranspk='" + transPK + "'";
                            d2.update_method_wo_parameter(DelQ, "Text");
                        }
                        string totamt = d2.GetFunction("select sum(debit)  from FT_FinDailyTransaction where app_no='" + Convert.ToString(dsVal.Tables[0].Rows[row]["app_no"]) + "' and feecategory='" + Convert.ToString(dsVal.Tables[0].Rows[row]["feecategory"]) + "' and ledgerfk='" + Convert.ToString(dsVal.Tables[0].Rows[row]["ledgerfk"]) + "' ");
                        string updQ = "  update ft_feeallot set paidamount='" + totamt + "', balamount=isnull(totalamount,'0')-'" + totamt + "' where app_no='" + Convert.ToString(dsVal.Tables[0].Rows[row]["app_no"]) + "' and feecategory='" + Convert.ToString(dsVal.Tables[0].Rows[row]["feecategory"]) + "' and ledgerfk='" + Convert.ToString(dsVal.Tables[0].Rows[row]["ledgerfk"]) + "'";
                        int updVal = d2.update_method_wo_parameter(updQ, "Text");
                        if (updVal > 0)
                            boolSave = true;
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

    #region 2017 paid update
    protected void btnupload_Click(object sender, EventArgs e)
    {
        importDetails();
    }
    protected void importDetails()
    {
        try
        {
            DataSet dsl = new DataSet();
            using (Stream stream = this.FileUpload1.FileContent as Stream)
            {
                string extension = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (extension.Trim() != "")
                {
                    if (FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".xlsx"))
                    {
                        string path = Server.MapPath("~/UploadFiles/" + System.IO.Path.GetFileName(FileUpload1.FileName));
                        FileUpload1.SaveAs(path);
                        DataSet dsVal = Excelconvertdataset(path);
                        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                        {
                            collegecode = "13";
                            updateStudent(dsVal, collegecode);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Excel Should Be Correct Format')", true);
                        }
                    }
                }
            }
        }
        catch { }
    }

    protected void updateStudent(DataSet dsVal, string collegecode)
    {
        try
        {
            bool boolSave = false;
            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
            {
                bool receipt = false;
                string lastRecptNo = string.Empty;
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
                string appNo = getAppNo(Convert.ToString(dsVal.Tables[0].Rows[row][0]));
                if (appNo != "0" && finYearid != "0" && !string.IsNullOrEmpty(finYearid))
                {
                    string selQ = "if exists(select app_no from ft_feeallot where app_no='" + appNo + "') update ft_feeallot set paidamount='0',balamount=totalamount where app_no='" + appNo + "' and headerfk in(9,11,12,13,14)";
                    selQ += " delete from ft_findailytransaction where app_no='" + appNo + "' and headerfk in(9,11,12,13,14)";
                    int upd = d2.update_method_wo_parameter(selQ, "Text");
                    string transcode = generateReceiptNo(finYearid, collegecode);//generate receipt no
                    // upd = 1;
                    if (upd > 0 && !string.IsNullOrEmpty(transcode) && transcode != "0")
                    {
                        lastRecptNo = lbllastrcpt.Text;
                        string transdate = Convert.ToString(dsVal.Tables[0].Rows[row][2]).Split(' ')[0];
                        // transdate = transdate.Split('/')[1] + "/" + transdate.Split('/')[0] + "/" + transdate.Split('/')[2];
                        //Convert.ToString(dsVal.Tables[0].Rows[row][1]);
                        string memtype = "1";
                        string PayMode = "5";
                        string checkDDno = Convert.ToString(dsVal.Tables[0].Rows[row][3]);
                        //  string dtchkdd = Convert.ToString(dsVal.Tables[0].Rows[row][4]).Split(' ')[0];
                        string rcptType = "1";
                        string iscollected = "1";
                        string selAllot = " select totalamount,paidamount,balamount,headerfk,ledgerfk,feecategory,FinYearFK from ft_feeallot where app_no='" + appNo + "' and headerfk in(9,11,12,13,14)";
                        DataSet dsAlt = d2.select_method_wo_parameter(selAllot, "Text");
                        if (dsAlt.Tables.Count > 0 && dsAlt.Tables[0].Rows.Count > 0)
                        {
                            for (int alt = 0; alt < dsAlt.Tables[0].Rows.Count; alt++)
                            {
                                string feecate = Convert.ToString(dsAlt.Tables[0].Rows[alt]["FeeCategory"]);
                                string hdFK = Convert.ToString(dsAlt.Tables[0].Rows[alt]["HeaderFK"]);
                                string ldFK = Convert.ToString(dsAlt.Tables[0].Rows[alt]["LedgerFK"]);
                                double paidAmt = 0;
                                double.TryParse(Convert.ToString(dsAlt.Tables[0].Rows[alt]["totalamount"]), out paidAmt);
                                string finYear = Convert.ToString(dsAlt.Tables[0].Rows[alt]["FinYearFK"]);
                                string insertDebit = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,TransType,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected,CollectedDate,IsDeposited,DepositedDate) VALUES('" + transdate + "','" + DateTime.Now.ToLongTimeString() + "','" + transcode + "', " + memtype + ", " + appNo + ", " + ldFK + ", " + hdFK + ", " + feecate + ", 0, " + paidAmt + ", " + PayMode + ", '" + checkDDno + "', '" + transdate + "', 1, '0', 0, '0', '0', '0', 0, " + usercode + ", " + finYear + ",'" + rcptType + "','" + iscollected + "','" + transdate + "','" + iscollected + "','" + transdate + "')";
                                int updAlt = d2.update_method_wo_parameter(insertDebit, "Text");
                                //updAlt = 1;
                                if (updAlt > 0)
                                {
                                    string updQ = "  update ft_feeallot set paidamount='" + paidAmt + "', balamount=isnull(totalamount,'0')-'" + paidAmt + "' where app_no='" + appNo + "' and feecategory='" + feecate + "' and ledgerfk='" + ldFK + "'";
                                    int updVal = d2.update_method_wo_parameter(updQ, "Text");
                                    boolSave = true;
                                }
                                receipt = true;
                            }
                        }
                    }
                    if (receipt)//receipt no update
                    {
                        #region receipt no update
                        if (!string.IsNullOrEmpty(transcode))
                        {
                            string updateRecpt = string.Empty;
                            if (isHeaderwise == 0 || isHeaderwise == 2)
                            {
                                updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where collegecode =" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                                lbllastrcpt.Text = string.Empty;
                            }
                            else
                            {
                                string hdrSetPK = string.Empty;
                                updateRecpt = "update FM_HeaderFinCodeSettings set RcptStNo=" + lastRecptNo + "+1 where HeaderSettingPK=" + hdrSetPK + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1'";
                            }
                            d2.update_method_wo_parameter(updateRecpt, "Text");
                        }
                        #endregion
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Financial Year Not Generated')", true);
                }
            }
            if (boolSave)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Not Updated')", true);
        }
        catch { }
    }
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


    #endregion
    public string generateReceiptNo(string finYearid, string collegecode)
    {
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
        }
        catch { isHeaderwise = 0; }
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 5)
                return string.Empty;
        }
        catch { return string.Empty; }
        if (isHeaderwise == 0 || isHeaderwise == 2)
        {
            return getCommonReceiptNo(finYearid, collegecode);
        }
        else
        {
            return getHeaderwiseReceiptNo(finYearid, collegecode);
        }
    }
    private string getCommonReceiptNo(string finYearid, string collegecode)
    {
        string recno = string.Empty;
        // lblaccid.Text = "";
        // lstrcpt.Text = "";
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            //  string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            // lblaccid.Text = accountid;
            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
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

                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;


                int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
                lbllastrcpt.Text = Convert.ToString(receno);
            }
            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    private string getHeaderwiseReceiptNo(string finYearid, string collegecode)
    {
        string recno = string.Empty;
        // lblaccid.Text = "";
        //  lstrcpt.Text = "";
        string accountid = "";
        // lblaccid.Text = accountid;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            //  string isheaderFk = GetSelectedItemsValue(cbl_grpheader);
            string isheaderFk = string.Empty;
            //   string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);

            DataSet dsFinHedDet = d2.select_method_wo_parameter("select distinct HeaderSettingFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode + " and FinyearFK=" + finYearid + " and isnull(onlineRec,'')<>'1'", "Text");

            if (dsFinHedDet.Tables.Count > 0 && dsFinHedDet.Tables[0].Rows.Count == 1)//&& rbl_headerselect.SelectedIndex == 1
            {
                string secondreciptqurey = "select * from FM_HeaderFinCodeSettings where HeaderSettingPK =" + Convert.ToString(dsFinHedDet.Tables[0].Rows[0][0]) + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1'";
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

                    //lstrcpt.Text = Convert.ToString(receno);
                }
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
}