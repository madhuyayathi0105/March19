using System;
using System.Xml;
using System.Text;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;

//Code started by Mohamed Idhris Sheik Dawood -- 21-03-2016
public partial class ExportXMLForTally : System.Web.UI.Page
{
    int collegeCode = 0;
    int userCode = 0;
    DAccess2 DA = new DAccess2();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegeCode = Convert.ToInt32(Convert.ToString(Session["collegecode"]));
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
            if (!IsPostBack)
            {
                setLabelText();
                collegeCode = Convert.ToInt32(Convert.ToString(Session["collegecode"]));
                userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
                usercode = Convert.ToString(Convert.ToString(Session["usercode"]));
                checkSchoolSetting();
                bindCollege();
                updateClgCode();
                bindheader();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");

                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                trfnl.Visible = false;
                if (checkSchoolSetting() == 0)//for school only
                {
                    loadfinanceyear();
                    trfnl.Visible = true;
                }
            }
            updateClgCode();
        }
        catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); Response.Redirect("Default.aspx"); }
    }
    protected void lb_LogOut_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }
    protected void checkDate(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = Convert.ToDateTime(txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2]);
            DateTime todate = Convert.ToDateTime(txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2]);

            if (fromdate > todate)
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                imgAlert.Visible = true;
                lbl_alert.Text = "From Date Should Not Exceed To Date";
            }
        }
        catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); }
    }
    protected void ddlExpFormat_IndexChange(object sender, EventArgs e)
    {
        if (ddlExpFormat.SelectedIndex == 0 || ddlExpFormat.SelectedIndex == 2)
        {
            btnChlnExport.Enabled = true;
            btnChlnUndo.Enabled = true;
        }
        else
        {
            btnChlnExport.Enabled = false;
            btnChlnUndo.Enabled = false;
        }
    }
    protected void btnRcptExport_Click(object sender, EventArgs e)
    {
        if (ddlExpFormat.SelectedIndex == 0)
        {
            ExportRcptForMccNew();
            //ExportRcptForMcc(); old Format
        }
        else if (ddlExpFormat.SelectedIndex == 1)
        {
            ExportRcptForChristopher();
        }
        else if (ddlExpFormat.SelectedIndex == 2)
        {
            ExportRcptForMEC();
            //  ExportRcptForMccNew();
        }
    }
    //old
    private void ExportRcptForMcc()
    {
        imgAlert.Visible = true;
        contentDiv.InnerHtml = "";

        string selectedHdr = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Text : string.Empty;
        string selectHdrVal = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Value : "0";
        if (selectedHdr != string.Empty)
        {
            try
            {
                string fromDate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
                string toDate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

                string selectQ = "select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, r.Stud_Name, C.Course_Name+' '+dt.dept_acronym as DegreeName, R.Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch from FT_FinDailyTransaction f,FM_LedgerMaster L,Registration R,Degree d,Department dt,Course C where R.App_No =F.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =C.Course_Id and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and Memtype='1' and isnull(IsExported,0)<>1  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1' and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";
                DataSet dsExRecords = new DataSet();
                //dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                selectQ += " union select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, r.Stud_Name, C.Course_Name+' '+dt.dept_acronym as DegreeName, R.Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch from FT_FinDailyTransaction f,FM_LedgerMaster L,Applyn R,Degree d,Department dt,Course C where R.App_No =F.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =C.Course_Id and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and Memtype='1' and isnull(IsExported,0)<>1  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'  and r.app_no not in (select app_no from registration) ";

                selectQ += " union select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, s.staff_name  as Stud_Name, '' DegreeName, '' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch from FT_FinDailyTransaction f,FM_LedgerMaster L,staffmaster s,staff_appl_master a where s.appl_no =a.appl_no and a.appl_id =F.App_No and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and isnull(IsExported,0)<>1 and Memtype='2'  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";
                // dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                selectQ += " union  select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, VenContactName  as Stud_Name, '' DegreeName, '' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch from FT_FinDailyTransaction f,FM_LedgerMaster L, IM_VendorContactMaster c,CO_VendorMaster v where vendorcontactpk =F.App_No and  VendorFK =vendorpk and VendorType<>-5 and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1  and Memtype='3'  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";
                //dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                selectQ += " union select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, vendorName as Stud_Name, '' DegreeName,'' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch from FT_FinDailyTransaction f,FM_LedgerMaster L, co_vendormaster where vendorpk =F.App_No and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1 and VendorType=-5 and Memtype='4'  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'   and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";

                selectQ = selectQ + "  select b.bankName,f.bankfk,DailyTransID from ft_finbanktransaction f,Fm_finbankMaster b where b.bankpk=f.bankfk -- and Transdate between '" + fromDate + "' and '" + toDate + "' ";
                DataView dv = new DataView();
                dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");
                if (dsExRecords.Tables.Count > 0 && dsExRecords.Tables[0].Rows.Count > 0)
                {
                    string xmlHdr = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA>   <REQUESTDESC><REPORTNAME>Vouchers</REPORTNAME><STATICVARIABLES><SVCURRENTCOMPANY>" + selectedHdr + "</SVCURRENTCOMPANY></STATICVARIABLES></REQUESTDESC><REQUESTDATA><TALLYMESSAGE xmlns:UDF=\"TallyUDF\">";
                    string xmlFtr = "</TALLYMESSAGE></REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

                    contentDiv.InnerHtml = xmlHdr;
                    for (int xRec = 0; xRec < dsExRecords.Tables[0].Rows.Count; xRec++)
                    {
                        string curDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["TransDate"]);
                        string[] curDateAr = curDate.Split('/');

                        if (curDateAr.Length == 3)
                        {
                            string debit = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["debit"]);
                            string fnTransDate = curDateAr[2] + curDateAr[1] + curDateAr[0];
                            string studName = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["Stud_Name"]);
                            string studCourse = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DegreeName"]);
                            int memtype = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["Memtype"]);
                            string studyearCourse = string.Empty;
                            if (memtype == 1)
                            {
                                int batchYr = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["Batch_Year"]);
                                string studYr = feePaidYear(Convert.ToInt32(curDateAr[1]), Convert.ToInt32(curDateAr[2]), batchYr);
                                studyearCourse = "(" + studYr + " " + studCourse + ")";
                            }
                            string rcptNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["TransCode"]);
                            string ledgName = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["LedgerName"]);
                            string ledgID = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["LedgerFk"]);
                            int payMode = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["payMode"]);
                            string dispMode = "Cash";
                            string narration = studName + studyearCourse + " " + Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["narration"]);

                            if (payMode > 1)
                            {
                                string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDDate"]);
                                string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDNo"]);
                                string ddBranch = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDBankBranch"]);
                                dispMode = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["BankName"]);

                                narration += " Bank : " + dispMode + " Branch : " + ddBranch + " Date : " + ddDate + " No : " + ddNo + " ";

                                dsExRecords.Tables[1].DefaultView.RowFilter = "DailyTransID='" + rcptNo + "'";
                                dv = dsExRecords.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                {
                                    string BankName = Convert.ToString(dv[0]["bankName"]);

                                    if (BankName.Trim() == "INDIAN OVERSEAS BANK")
                                    {
                                        dispMode = "Bank I O B";
                                    }

                                }
                            }
                            else
                            {
                                narration += " Cash Rs." + debit + " received from " + studName + studyearCourse + " towards Reason (Rcpt.No." + rcptNo + ")";
                            }

                            StringBuilder sbLedeDet = new StringBuilder();
                            sbLedeDet.Append("<VOUCHER VCHTYPE=\"RECEIPT\" ACTION=\"CREATE\"><DATE>" + fnTransDate + "</DATE><NARRATION>" + narration + "</NARRATION><VOUCHERTYPENAME>RECEIPT</VOUCHERTYPENAME><VOUCHERNUMBER>" + rcptNo + "</VOUCHERNUMBER><PARTYLEDGERNAME>" + dispMode + "</PARTYLEDGERNAME><EFFECTIVEDATE>" + fnTransDate + "</EFFECTIVEDATE><HASCASHFLOW>Yes</HASCASHFLOW><ALLLEDGERENTRIES.LIST><LEDGERNAME>" + ledgName + "</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><AMOUNT>" + debit + "</AMOUNT></ALLLEDGERENTRIES.LIST><ALLLEDGERENTRIES.LIST>");

                            if (payMode > 1)
                            {
                                string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDDate"]);
                                string[] ddDateAr = ddDate.Split('/');
                                string fnDDDate = ddDateAr[2] + ddDateAr[1] + ddDateAr[0];
                                string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDNo"]);
                                string paymentFavor = string.Empty;
                                //sbLedeDet.Append(" <BANKALLOCATIONS.LIST><DATE>" + fnTransDate + "</DATE><INSTRUMENTDATE>" + fnDDDate + "</INSTRUMENTDATE><TRANSACTIONTYPE>Cheque/DD</TRANSACTIONTYPE><PAYMENTFAVOURING>" + paymentFavor + "</PAYMENTFAVOURING><STATUS>No</STATUS><INSTRUMENTNUMBER>" + ddNo + "</INSTRUMENTNUMBER><AMOUNT>-" + debit + "</AMOUNT></BANKALLOCATIONS.LIST>");
                                sbLedeDet.Append("<LEDGERNAME>Bank I O B</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>-" + debit + "</AMOUNT><BANKALLOCATIONS.LIST><DATE>" + fnTransDate + "</DATE><INSTRUMENTDATE>" + fnDDDate + "</INSTRUMENTDATE><INSTRUMENTNUMBER>" + ddNo + "</INSTRUMENTNUMBER><NAME>Name-" + rcptNo + "</NAME><TRANSACTIONTYPE>Cheque/DD</TRANSACTIONTYPE><PAYMENTFAVOURING>" + ledgName + "</PAYMENTFAVOURING><STATUS>No</STATUS><UNIQUEREFERENCENUMBER>UNIQ" + rcptNo + "</UNIQUEREFERENCENUMBER><PAYMENTMODE>Transacted</PAYMENTMODE><BANKPARTYNAME>Bank I O B</BANKPARTYNAME><ISCONNECTEDPAYMENT>No</ISCONNECTEDPAYMENT><ISSPLIT>No</ISSPLIT><ISCONTRACTUSED>No</ISCONTRACTUSED><CHEQUEPRINTED> 1</CHEQUEPRINTED><AMOUNT>-" + debit + "</AMOUNT></BANKALLOCATIONS.LIST>");
                            }
                            else
                            {
                                sbLedeDet.Append("<LEDGERNAME>" + dispMode + "</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>-" + debit + "</AMOUNT>");
                            }
                            sbLedeDet.Append("</ALLLEDGERENTRIES.LIST></VOUCHER>");

                            contentDiv.InnerHtml += sbLedeDet.ToString();

                            string newDt = (curDateAr[1] + "/" + curDateAr[0] + "/" + curDateAr[2]);
                            string upQ = "update FT_FinDailyTransaction set IsExported=1 where TransDate ='" + newDt + "' and HeaderFK =" + selectHdrVal + " and Ledgerfk=" + ledgID + " and Transcode='" + rcptNo + "' and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) ";
                            DA.update_method_wo_parameter(upQ, "Text");
                        }
                    }
                    contentDiv.InnerHtml += xmlFtr;

                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";

                        string szFile = "Tally" + DateTime.Now.ToString("ddMMyyyy") + "-" + DateTime.Now.ToString("HHMMss") + ".xml";
                        //szFile = "Tally.xml";
                        XmlDocument xdoc = new XmlDocument();
                        try
                        {
                            xdoc.LoadXml(contentDiv.InnerHtml.ToString());
                            xdoc.Save(szPath + szFile);
                        }
                        catch (Exception ex)
                        {
                            DA.sendErrorMail(ex, collegeCode.ToString(), contentDiv.InnerHtml.ToString()); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Exporting!";
                        }

                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/xml";
                        Response.WriteFile(szPath + szFile);
                        Response.Flush();
                        Response.End();
                        //HttpContext.Current.ApplicationInstance.CompleteRequest();

                    }
                    lbl_alert.Text = "Exported Successfully";
                    //imgAlert.Visible = false;
                    contentDiv.InnerHtml = "";

                }
                else
                {
                    lbl_alert.Text = "No Records Available From " + txt_fromdate.Text + " To " + txt_todate.Text + "";
                }
                contentDiv.InnerHtml = "";
            }
            catch (System.Threading.ThreadAbortException abrtEx) { }
            catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Exporting!"; }
        }
        else
        {
            lbl_alert.Text = "No Accounts Available";
        }
    }
    //new
    private void ExportRcptForMccNew()
    {
        imgAlert.Visible = true;
        contentDiv.InnerHtml = "";

        string selectedHdr = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Text : string.Empty;
        string selectHdrVal = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Value : "0";
        if (selectedHdr != string.Empty)
        {
            try
            {
                string strFnlYR = string.Empty;
                if (checkSchoolSetting() == 0)
                {
                    string finYr = string.Empty;
                    if (ddlfinyear.Items.Count > 0)
                    {
                        finYr = Convert.ToString(ddlfinyear.SelectedValue);
                        strFnlYR = " and f.actualfinyearfk in('" + finYr + "')";
                    }
                }
                string fromDate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
                string toDate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

                string selectQ = "select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, r.Stud_Name, C.Course_Name+' '+dt.dept_acronym as DegreeName, R.Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L,Registration R,Degree d,Department dt,Course C where R.App_No =F.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =C.Course_Id and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and Memtype='1' and isnull(IsExported,0)<>1  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1' and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "' " + strFnlYR + "";
                DataSet dsExRecords = new DataSet();
                //dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                selectQ += " union select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, r.Stud_Name, C.Course_Name+' '+dt.dept_acronym as DegreeName, R.Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L,Applyn R,Degree d,Department dt,Course C where R.App_No =F.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =C.Course_Id and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and Memtype='1' and isnull(IsExported,0)<>1  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'  and r.app_no not in (select app_no from registration) " + strFnlYR + "";

                selectQ += " union select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, s.staff_name  as Stud_Name, '' DegreeName, '' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L,staffmaster s,staff_appl_master a where s.appl_no =a.appl_no and a.appl_id =F.App_No and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and isnull(IsExported,0)<>1 and Memtype='2'  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";
                // dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                selectQ += " union  select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, VenContactName  as Stud_Name, '' DegreeName, '' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L, IM_VendorContactMaster c,CO_VendorMaster v where vendorcontactpk =F.App_No and  VendorFK =vendorpk and VendorType<>-5 and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1  and Memtype='3'  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";
                //dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                selectQ += " union select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, vendorName as Stud_Name, '' DegreeName,'' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L, co_vendormaster where vendorpk =F.App_No and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1 and VendorType=-5 and Memtype='4'  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'   and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";

                selectQ = selectQ + "  select b.bankName,f.bankfk,DailyTransID from ft_finbanktransaction f,Fm_finbankMaster b where b.bankpk=f.bankfk -- and Transdate between '" + fromDate + "' and '" + toDate + "' ";
                DataView dv = new DataView();
                dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");
                if (dsExRecords.Tables.Count > 0 && dsExRecords.Tables[0].Rows.Count > 0)
                {
                    string xmlHdr = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA>   <REQUESTDESC><REPORTNAME>Vouchers</REPORTNAME><STATICVARIABLES><SVCURRENTCOMPANY>" + selectedHdr + "</SVCURRENTCOMPANY></STATICVARIABLES></REQUESTDESC><REQUESTDATA><TALLYMESSAGE xmlns:UDF=\"TallyUDF\">";
                    string xmlFtr = "</TALLYMESSAGE></REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

                    contentDiv.InnerHtml = xmlHdr;

                    //DataTable uniqueReceiptNo = dsExRecords.Tables[0].DefaultView.ToTable(true, "TransCode");
                    Hashtable htReceiptCode = new Hashtable();
                    for (int xRec = 0; xRec < dsExRecords.Tables[0].Rows.Count; xRec++)
                    {
                        string rcptNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["TransCode"]).Trim();
                        if (htReceiptCode.Contains(rcptNo))
                        {
                            continue;
                        }
                        else
                        {
                            htReceiptCode.Add(rcptNo, rcptNo);
                        }

                        string curDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["TransDate"]);
                        string[] curDateAr = curDate.Split('/');

                        if (curDateAr.Length == 3)
                        {
                            string debit = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["debit"]);
                            string fnTransDate = curDateAr[2] + curDateAr[1] + curDateAr[0];
                            string studName = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["Stud_Name"]);
                            string studCourse = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DegreeName"]);
                            int memtype = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["Memtype"]);
                            string studyearCourse = string.Empty;
                            if (memtype == 1)
                            {
                                int batchYr = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["Batch_Year"]);
                                string studYr = feePaidYear(Convert.ToInt32(curDateAr[1]), Convert.ToInt32(curDateAr[2]), batchYr);
                                studyearCourse = "(" + studYr + " " + studCourse + ")";
                            }

                            string ledgName = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["LedgerName"]);
                            string ledgID = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["LedgerFk"]);
                            int payMode = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["payMode"]);
                            string dispMode = "Cash";
                            string narration = studName + studyearCourse + " " + Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["narration"]);


                            dsExRecords.Tables[0].DefaultView.RowFilter = " TransCode='" + rcptNo + "'";
                            DataView dvMultiLedgers = dsExRecords.Tables[0].DefaultView;

                            StringBuilder sbMultiLedeDet = new StringBuilder();
                            double debitAMT = 0;
                            for (int xxx = 0; xxx < dvMultiLedgers.Count; xxx++)
                            {
                                double debitLED = Convert.ToDouble(Convert.ToString(dvMultiLedgers[xxx]["debit"]));
                                string ledgNameLED = Convert.ToString(dvMultiLedgers[xxx]["LedgerName"]);
                                sbMultiLedeDet.Append("<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + ledgNameLED + "</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><AMOUNT>" + debitLED + "</AMOUNT></ALLLEDGERENTRIES.LIST>");

                                debitAMT += debitLED;
                            }
                            debit = debitAMT.ToString();

                            if (payMode > 1)
                            {
                                string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDDate"]);
                                string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDNo"]);
                                string ddBranch = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDBankBranch"]);
                                dispMode = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["BankName"]);

                                narration += " Bank : " + dispMode + " Branch : " + ddBranch + " Date : " + ddDate + " No : " + ddNo + " ";

                                dsExRecords.Tables[1].DefaultView.RowFilter = "DailyTransID='" + rcptNo + "'";
                                dv = dsExRecords.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                {
                                    string BankName = Convert.ToString(dv[0]["bankName"]);
                                    if (BankName.Trim() == "INDIAN OVERSEAS BANK")
                                    {
                                        dispMode = "Bank I O B";
                                    }
                                }
                            }
                            else
                            {
                                narration += " Cash Rs." + debit + " received from " + studName + studyearCourse + " towards Reason (Rcpt.No." + rcptNo + ")";
                            }

                            StringBuilder sbLedeDet = new StringBuilder();
                            sbLedeDet.Append("<VOUCHER VCHTYPE=\"RECEIPT\" ACTION=\"CREATE\"><DATE>" + fnTransDate + "</DATE><NARRATION>" + narration + "</NARRATION><VOUCHERTYPENAME>RECEIPT</VOUCHERTYPENAME><VOUCHERNUMBER>" + rcptNo + "</VOUCHERNUMBER><PARTYLEDGERNAME>" + dispMode + "</PARTYLEDGERNAME><EFFECTIVEDATE>" + fnTransDate + "</EFFECTIVEDATE><HASCASHFLOW>Yes</HASCASHFLOW>");


                            sbLedeDet.Append(sbMultiLedeDet.ToString() + "<ALLLEDGERENTRIES.LIST>");

                            if (payMode > 1)
                            {
                                string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDDate"]);
                                string[] ddDateAr = ddDate.Split('/');
                                string fnDDDate = ddDateAr[2] + ddDateAr[1] + ddDateAr[0];
                                string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDNo"]);
                                string paymentFavor = string.Empty;

                                //sbLedeDet.Append("<LEDGERNAME>Bank I O B</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>-" + debit + "</AMOUNT><BANKALLOCATIONS.LIST><DATE>" + fnTransDate + "</DATE><INSTRUMENTDATE>" + fnDDDate + "</INSTRUMENTDATE><INSTRUMENTNUMBER>" + ddNo + "</INSTRUMENTNUMBER><NAME>Name-" + rcptNo + "</NAME><TRANSACTIONTYPE>Cheque/DD</TRANSACTIONTYPE><PAYMENTFAVOURING>" + ledgName + "</PAYMENTFAVOURING><STATUS>No</STATUS><UNIQUEREFERENCENUMBER>UNIQ" + rcptNo + "</UNIQUEREFERENCENUMBER><PAYMENTMODE>Transacted</PAYMENTMODE><BANKPARTYNAME>Bank I O B</BANKPARTYNAME><ISCONNECTEDPAYMENT>No</ISCONNECTEDPAYMENT><ISSPLIT>No</ISSPLIT><ISCONTRACTUSED>No</ISCONTRACTUSED><CHEQUEPRINTED> 1</CHEQUEPRINTED><AMOUNT>-" + debit + "</AMOUNT></BANKALLOCATIONS.LIST>");
                                sbLedeDet.Append("<LEDGERNAME>Bank I O B</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>-" + debit + "</AMOUNT><BANKALLOCATIONS.LIST><DATE>" + fnTransDate + "</DATE><INSTRUMENTDATE>" + fnDDDate + "</INSTRUMENTDATE><INSTRUMENTNUMBER>" + ddNo + "</INSTRUMENTNUMBER><NAME>Name-" + rcptNo + "</NAME><TRANSACTIONTYPE>Cheque/DD</TRANSACTIONTYPE><PAYMENTFAVOURING>" + ledgName + "</PAYMENTFAVOURING><STATUS>No</STATUS><UNIQUEREFERENCENUMBER>UNIQ" + rcptNo + "</UNIQUEREFERENCENUMBER><PAYMENTMODE>Transacted</PAYMENTMODE><BANKPARTYNAME>" + ledgName + "</BANKPARTYNAME><ISCONNECTEDPAYMENT>No</ISCONNECTEDPAYMENT><ISSPLIT>No</ISSPLIT><ISCONTRACTUSED>No</ISCONTRACTUSED><CHEQUEPRINTED> 1</CHEQUEPRINTED><AMOUNT>-" + debit + "</AMOUNT></BANKALLOCATIONS.LIST>");
                            }
                            else
                            {
                                sbLedeDet.Append("<LEDGERNAME>" + dispMode + "</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>-" + debit + "</AMOUNT>");
                            }
                            sbLedeDet.Append("</ALLLEDGERENTRIES.LIST></VOUCHER>");

                            contentDiv.InnerHtml += sbLedeDet.ToString();

                            string newDt = (curDateAr[1] + "/" + curDateAr[0] + "/" + curDateAr[2]);
                            // string upQ = "update FT_FinDailyTransaction set IsExported=1 where TransDate ='" + newDt + "' and HeaderFK =" + selectHdrVal + " and Ledgerfk=" + ledgID + " and Transcode='" + rcptNo + "' and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) ";
                            string upQ = "update FT_FinDailyTransaction set IsExported=1 where TransDate ='" + newDt + "' and Transcode='" + rcptNo + "' and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) ";
                            DA.update_method_wo_parameter(upQ, "Text");
                        }
                    }
                    contentDiv.InnerHtml += xmlFtr;

                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";

                        string szFile = "Tally" + DateTime.Now.ToString("ddMMyyyy") + "-" + DateTime.Now.ToString("HHMMss") + ".xml";
                        //szFile = "Tally.xml";
                        XmlDocument xdoc = new XmlDocument();
                        try
                        {
                            xdoc.LoadXml(contentDiv.InnerHtml.ToString());
                            xdoc.Save(szPath + szFile);
                        }
                        catch (Exception ex)
                        {
                            DA.sendErrorMail(ex, collegeCode.ToString(), contentDiv.InnerHtml.ToString()); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Exporting!";
                        }

                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/xml";
                        Response.WriteFile(szPath + szFile);
                        Response.Flush();
                        Response.End();
                        //HttpContext.Current.ApplicationInstance.CompleteRequest();

                    }
                    lbl_alert.Text = "Exported Successfully";
                    //imgAlert.Visible = false;
                    contentDiv.InnerHtml = "";

                }
                else
                {
                    lbl_alert.Text = "No Records Available From " + txt_fromdate.Text + " To " + txt_todate.Text + "";
                }
                contentDiv.InnerHtml = "";
            }
            catch (System.Threading.ThreadAbortException abrtEx) { }
            catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Exporting!"; }
        }
        else
        {
            lbl_alert.Text = "No Accounts Available";
        }
    }
    private void ExportRcptForChristopher()
    {
        imgAlert.Visible = true;
        contentDiv.InnerHtml = "";

        string selectedHdr = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Text : string.Empty;
        string selectHdrVal = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Value : "0";
        if (selectedHdr != string.Empty)
        {
            try
            {
                string strFnlYR = string.Empty;
                if (checkSchoolSetting() == 0)
                {
                    string finYr = string.Empty;
                    if (ddlfinyear.Items.Count > 0)
                    {
                        finYr = Convert.ToString(ddlfinyear.SelectedValue);
                        strFnlYR = " and f.actualfinyearfk in('" + finYr + "')";
                    }
                }
                string fromDate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
                string toDate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

                string selectTotQ = "select f.app_no, sum(debit) as debit,CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype  from FT_FinDailyTransaction f,FM_LedgerMaster L where  f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1 and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "' group by TransDate,TransCode, Paymode,DDBankCode,DDNo,DDDate, Memtype , f.app_no ";

                string selectQ = "   select f.app_no,CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, isnull(Debit,0) as Debit, l.LedgerName, r.Stud_Name, C.Course_Name+' '+dt.dept_acronym as DegreeName, R.Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype  from FT_FinDailyTransaction f,FM_LedgerMaster L,Registration R,Degree d,Department dt,Course C where R.App_No =F.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =C.Course_Id and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and Memtype='1' and isnull(IsExported,0)<>1 and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "' " + strFnlYR + "";

                selectQ += " union  select f.app_no,CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, isnull(Debit,0) as Debit, l.LedgerName, r.Stud_Name, C.Course_Name+' '+dt.dept_acronym as DegreeName, R.Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype  from FT_FinDailyTransaction f,FM_LedgerMaster L,Applyn R,Degree d,Department dt,Course C where R.App_No =F.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =C.Course_Id and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and Memtype='1' and isnull(IsExported,0)<>1 and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'  and r.app_no not in (select app_no from registration) " + strFnlYR + " ";

                selectQ += " union  select f.app_no,CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, s.staff_name  as Stud_Name, '' DegreeName, '' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype from FT_FinDailyTransaction f,FM_LedgerMaster L,staffmaster s,staff_appl_master a where s.appl_no =a.appl_no and a.appl_id =F.App_No and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1 and Memtype='2' and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "' ";

                selectQ += " union  select f.app_no,CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, VenContactName  as Stud_Name, '' DegreeName, '' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype from FT_FinDailyTransaction f,FM_LedgerMaster L, IM_VendorContactMaster c,CO_VendorMaster v where vendorcontactpk =F.App_No and  VendorFK =vendorpk and VendorType<>-5 and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1  and Memtype='3' and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "' ";

                selectQ += " union  select f.app_no,CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, vendorName as Stud_Name, '' DegreeName,'' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype from FT_FinDailyTransaction f,FM_LedgerMaster L, co_vendormaster where vendorpk =F.App_No and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1 and VendorType=-5 and Memtype='4'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "' ";

                DataSet dsExRecords = new DataSet();
                dsExRecords = DA.select_method_wo_parameter((selectTotQ + selectQ), "Text");

                if (dsExRecords.Tables.Count > 1 && dsExRecords.Tables[0].Rows.Count > 0 && dsExRecords.Tables[1].Rows.Count > 0)
                {
                    string xmlHdr = "<ENVELOPE>";
                    string xmlFtr = "</ENVELOPE>";

                    contentDiv.InnerHtml = xmlHdr;
                    for (int xHrec = 0; xHrec < dsExRecords.Tables[0].Rows.Count; xHrec++)
                    {
                        string grossAmt = Convert.ToString(dsExRecords.Tables[0].Rows[xHrec]["Debit"]);
                        string transCode = Convert.ToString(dsExRecords.Tables[0].Rows[xHrec]["TransCode"]);
                        DataView dvRec = new DataView();
                        dsExRecords.Tables[1].DefaultView.RowFilter = "TransCode='" + transCode + "'";
                        dvRec = dsExRecords.Tables[1].DefaultView;
                        if (dvRec.Count > 0)
                        {
                            string curDate = Convert.ToString(dsExRecords.Tables[0].Rows[xHrec]["TransDate"]);
                            string[] curDateAr = curDate.Split('/');

                            if (curDateAr.Length == 3)
                            {
                                string debit = Convert.ToString(dsExRecords.Tables[0].Rows[xHrec]["debit"]);
                                string fnTransDate = curDateAr[2] + curDateAr[1] + curDateAr[0];
                                string studName = string.Empty;
                                string studCourse = string.Empty;

                                int memtype = Convert.ToInt32(dsExRecords.Tables[0].Rows[xHrec]["Memtype"]);
                                string studyearCourse = string.Empty;

                                string rcptNo = Convert.ToString(dsExRecords.Tables[0].Rows[xHrec]["TransCode"]);
                                //string ledgName = Convert.ToString(dsExRecords.Tables[0].Rows[xHrec]["LedgerName"]);

                                int payMode = Convert.ToInt32(dsExRecords.Tables[0].Rows[xHrec]["payMode"]);
                                string dispMode = "Cash";
                                if (payMode > 1)
                                {
                                    dispMode = Convert.ToString(dsExRecords.Tables[0].Rows[xHrec]["BankName"]);
                                }

                                //if (payMode > 1)
                                //{
                                //    string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xHrec]["DDDate"]);
                                //    string[] ddDateAr = ddDate.Split('/');
                                //    string fnDDDate = ddDateAr[2] + ddDateAr[1] + ddDateAr[0];
                                //    string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xHrec]["DDNo"]);
                                //    string paymentFavor = string.Empty;
                                //}
                                for (int xRec = 0; xRec < dvRec.Count; xRec++)
                                {
                                    if (xRec == 0)
                                    {
                                        studName = Convert.ToString(dvRec[xRec]["Stud_Name"]);
                                        studCourse = Convert.ToString(dvRec[xRec]["DegreeName"]);
                                        if (memtype == 1)
                                        {
                                            int batchYr = Convert.ToInt32(dvRec[xRec]["Batch_Year"]);
                                            string studYr = feePaidYear(Convert.ToInt32(curDateAr[1]), Convert.ToInt32(curDateAr[2]), batchYr);
                                            studyearCourse = "(" + studYr + " " + studCourse + ")";
                                        }
                                    }
                                    string newDt = (curDateAr[1] + "/" + curDateAr[0] + "/" + curDateAr[2]);
                                    string ledgID = Convert.ToString(dvRec[xRec]["LedgerFk"]);
                                    string upQ = "update FT_FinDailyTransaction set IsExported=1 where TransDate ='" + newDt + "' and HeaderFK =" + selectHdrVal + " and Ledgerfk=" + ledgID + " and Transcode='" + rcptNo + "' and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) ";
                                    DA.update_method_wo_parameter(upQ, "Text");
                                }
                                StringBuilder sbLedeDet = new StringBuilder();
                                sbLedeDet.Append("<DBCFIXED><DBCDATE>" + getExportDateFormatII(curDateAr) + "</DBCDATE><DBCPARTY>" + dispMode + "</DBCPARTY></DBCFIXED><DBCVCHTYPE>Receipt</DBCVCHTYPE><DBCVCHNO>" + transCode + "</DBCVCHNO><DBCVCHREF/><DBCNARR>" + studName + studyearCourse + "</DBCNARR><DBCGROSSAMT>" + grossAmt + "</DBCGROSSAMT><DBCLEDAMT>" + grossAmt + "</DBCLEDAMT><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/><DBCLEDAMT/>");
                                contentDiv.InnerHtml += sbLedeDet.ToString();
                            }
                        }
                    }
                    contentDiv.InnerHtml += xmlFtr;

                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";

                        string szFile = "Tally" + DateTime.Now.ToString("ddMMyyyy") + "-" + DateTime.Now.ToString("HHMMss") + ".xml";
                        //szFile = "Tally.xml";
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.LoadXml(contentDiv.InnerHtml.ToString());
                        xdoc.Save(szPath + szFile);

                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/xml";
                        Response.WriteFile(szPath + szFile);
                        Response.Flush();
                        Response.End();
                    }
                    lbl_alert.Text = "Exported Successfully";
                    //imgAlert.Visible = false;
                    contentDiv.InnerHtml = "";

                }
                else
                {
                    lbl_alert.Text = "No Records Available From " + txt_fromdate.Text + " To " + txt_todate.Text + "";
                }
                contentDiv.InnerHtml = "";
            }
            catch (System.Threading.ThreadAbortException st) { }
            catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Exporting!"; }
        }
        else
        {
            lbl_alert.Text = "No Accounts Available";
        }
    }
    private string getExportDateFormatII(string[] curDateAr)
    {
        string expDate;
        try
        {
            int monval = Convert.ToInt32(curDateAr[1]);
            string monText = string.Empty;
            switch (monval)
            {
                case 1:
                    monText = "Jan";
                    break;
                case 2:
                    monText = "Feb";
                    break;
                case 3:
                    monText = "Mar";
                    break;
                case 4:
                    monText = "Apr";
                    break;
                case 5:
                    monText = "May";
                    break;
                case 6:
                    monText = "Jun";
                    break;
                case 7:
                    monText = "Jul";
                    break;
                case 8:
                    monText = "Aug";
                    break;
                case 9:
                    monText = "Sep";
                    break;
                case 10:
                    monText = "Oct";
                    break;
                case 11:
                    monText = "Nov";
                    break;
                case 12:
                    monText = "Dec";
                    break;
            }
            expDate = Convert.ToInt32(curDateAr[0]) + "-" + monText + "-" + curDateAr[2];
        }
        catch { expDate = string.Empty; }
        return expDate;
    }
    protected void btnRcptUndo_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = true;
        contentDiv.InnerHtml = "";

        string selectedHdr = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Text : string.Empty;
        string selectHdrVal = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Value : "0";
        if (selectedHdr != string.Empty)
        {
            try
            {
                string strFnlYR = string.Empty;
                if (checkSchoolSetting() == 0)
                {
                    string finYr = string.Empty;
                    if (ddlfinyear.Items.Count > 0)
                    {
                        finYr = Convert.ToString(ddlfinyear.SelectedValue);
                        strFnlYR = " and actualfinyearfk in('" + finYr + "')";
                    }
                }
                string fromDate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
                string toDate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

                string upQ = "update FT_FinDailyTransaction set IsExported=0 where ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and TransDate between '" + fromDate + "' and '" + toDate + "' and HeaderFK ='" + selectHdrVal + "' " + strFnlYR + "";
                DA.update_method_wo_parameter(upQ, "Text");
                lbl_alert.Text = "Updated Successfully";
            }
            catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Updating!"; }
        }
        else
        {
            lbl_alert.Text = "No Accounts Available";
        }
    }
    protected void btnChlnExport_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = true;
        contentDiv.InnerHtml = "";

        string selectedHdr = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Text : string.Empty;
        string selectHdrVal = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Value : "0";
        if (selectedHdr != string.Empty)
        {
            try
            {
                string strFnlYR = string.Empty;
                if (checkSchoolSetting() == 0)
                {
                    string finYr = string.Empty;
                    if (ddlfinyear.Items.Count > 0)
                    {
                        finYr = Convert.ToString(ddlfinyear.SelectedValue);
                        strFnlYR = " and actualfinyearfk in('" + finYr + "')";
                    }
                }
                string fromDate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
                string toDate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

                string selectRcptQ = "select distinct CONVERT(varchar(10), TransDate,103) as TransDate,TransCode from FT_FinDailyTransaction where TransDate between '" + fromDate + "' and '" + toDate + "' and HeaderFK ='" + selectHdrVal + "' and PayMode in (4,5) and isnull(IsExported,0)<>1  " + strFnlYR + " order by Transdate asc ";
                DataSet dsRcptNo = new DataSet();
                dsRcptNo = DA.select_method_wo_parameter(selectRcptQ, "Text");
                if (dsRcptNo.Tables.Count > 0 && dsRcptNo.Tables[0].Rows.Count > 0)
                {
                    string xmlHdr = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA>   <REQUESTDESC><REPORTNAME>Vouchers</REPORTNAME><STATICVARIABLES><SVCURRENTCOMPANY>" + selectedHdr + "</SVCURRENTCOMPANY></STATICVARIABLES></REQUESTDESC><REQUESTDATA><TALLYMESSAGE xmlns:UDF=\"TallyUDF\">";
                    string xmlFtr = "</TALLYMESSAGE></REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

                    contentDiv.InnerHtml = xmlHdr;

                    for (int rcpIndx = 0; rcpIndx < dsRcptNo.Tables[0].Rows.Count; rcpIndx++)
                    {
                        string curRcptDate = Convert.ToString(dsRcptNo.Tables[0].Rows[rcpIndx]["TransDate"]);
                        string curRcptNo = Convert.ToString(dsRcptNo.Tables[0].Rows[rcpIndx]["TransCode"]);
                        string curRcptDtFormat = curRcptDate.Split('/')[1] + "/" + curRcptDate.Split('/')[0] + "/" + curRcptDate.Split('/')[2];

                        string selectQ = "select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, r.Stud_Name, C.Course_Name+' '+dt.dept_acronym as DegreeName, R.Batch_Year, Paymode, (SELECT BankName FROM FM_FinBankMaster  where CollegeCode=" + collegeCode + " and BankPK=DDBankCode) as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate,f.narration,f.DDBankBranch  from FT_FinDailyTransaction f,FM_LedgerMaster L,Registration R,Degree d,Department dt,Course C where R.App_No =F.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =C.Course_Id and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (4,5) and isnull(IsExported,0)<>1 and TransDate ='" + curRcptDtFormat + "' and f.HeaderFK ='" + selectHdrVal + "' and TransCode='" + curRcptNo + "' " + strFnlYR + "";
                        DataSet dsExRecords = new DataSet();
                        dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                        if (dsExRecords.Tables.Count > 0 && dsExRecords.Tables[0].Rows.Count > 0)
                        {
                            string[] curDateAr = curRcptDate.Split('/');
                            string fnTransDate = curDateAr[2] + curDateAr[1] + curDateAr[0];
                            if (curDateAr.Length == 3)
                            {
                                StringBuilder sbLedeDet = new StringBuilder();
                                decimal bankDebit = 0;//added on june 14,2016
                                for (int xRec = 0; xRec < dsExRecords.Tables[0].Rows.Count; xRec++)
                                {
                                    string debit = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["debit"]);

                                    string studName = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["Stud_Name"]);
                                    string studCourse = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DegreeName"]);
                                    int batchYr = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["Batch_Year"]);
                                    string studYr = feePaidYear(Convert.ToInt32(curDateAr[1]), Convert.ToInt32(curDateAr[2]), batchYr);
                                    string rcptNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["TransCode"]);
                                    string ledgName = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["LedgerName"]);
                                    string ledgID = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["LedgerFk"]);
                                    int payMode = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["payMode"]);
                                    string dispMode = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["BankName"]);
                                    string narration = "Amount Through Challan" + Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["narration"]);

                                    if (payMode > 1)
                                    {
                                        string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDDate"]);
                                        string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDNo"]);
                                        string ddBranch = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDBankBranch"]);

                                        narration += " Bank : " + dispMode + " Branch : " + ddBranch + " Date : " + ddDate + " No : " + ddNo + " ";
                                    }

                                    if (xRec == 0)
                                    {
                                        sbLedeDet.Append("<VOUCHER VCHTYPE=\"RECEIPT\" ACTION=\"CREATE\"><DATE>" + fnTransDate + "</DATE><NARRATION>" + narration + "</NARRATION><VOUCHERTYPENAME>RECEIPT</VOUCHERTYPENAME><VOUCHERNUMBER>" + curRcptNo + "</VOUCHERNUMBER><PARTYLEDGERNAME>" + dispMode + "</PARTYLEDGERNAME><EFFECTIVEDATE>" + fnTransDate + "</EFFECTIVEDATE><HASCASHFLOW>Yes</HASCASHFLOW>");
                                    }
                                    //Changed June 14, 2016
                                    sbLedeDet.Append("<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + ledgName + "</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><AMOUNT>" + debit + "</AMOUNT></ALLLEDGERENTRIES.LIST>");

                                    decimal debitval = 0;
                                    decimal.TryParse(debit, out debitval);
                                    bankDebit += debitval;
                                    if (xRec == dsExRecords.Tables[0].Rows.Count - 1)
                                    {
                                        string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDDate"]);
                                        string[] ddDateAr = ddDate.Split('/');
                                        string fnDDDate = ddDateAr[2] + ddDateAr[1] + ddDateAr[0];
                                        string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDNo"]);
                                        string paymentFavor = ledgName; //string.Empty;
                                        sbLedeDet.Append("<ALLLEDGERENTRIES.LIST><LEDGERNAME>Bank I O B</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>-" + bankDebit + "</AMOUNT>");
                                        sbLedeDet.Append("<BANKALLOCATIONS.LIST><DATE>" + fnTransDate + "</DATE><INSTRUMENTDATE>" + fnDDDate + "</INSTRUMENTDATE><INSTRUMENTNUMBER>" + ddNo + "</INSTRUMENTNUMBER><NAME>Name-" + curRcptNo + "</NAME><TRANSACTIONTYPE>Cheque/DD</TRANSACTIONTYPE><PAYMENTFAVOURING>" + paymentFavor + "</PAYMENTFAVOURING><STATUS>No</STATUS><UNIQUEREFERENCENUMBER>UNIQ" + curRcptNo + "</UNIQUEREFERENCENUMBER><PAYMENTMODE>Transacted</PAYMENTMODE><BANKPARTYNAME>" + ledgName + "</BANKPARTYNAME><ISCONNECTEDPAYMENT>No</ISCONNECTEDPAYMENT><ISSPLIT>No</ISSPLIT><ISCONTRACTUSED>No</ISCONTRACTUSED><CHEQUEPRINTED> 1</CHEQUEPRINTED><AMOUNT>-" + bankDebit + "</AMOUNT></BANKALLOCATIONS.LIST></ALLLEDGERENTRIES.LIST></VOUCHER>");
                                    }

                                    //if (xRec == dsExRecords.Tables[0].Rows.Count - 1)
                                    //{
                                    //    string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDDate"]);
                                    //    string[] ddDateAr = ddDate.Split('/');
                                    //    string fnDDDate = ddDateAr[2] + ddDateAr[1] + ddDateAr[0];
                                    //    string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDNo"]);
                                    //    string paymentFavor = string.Empty;
                                    //    sbLedeDet.Append("<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + ledgName + "</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>" + debit + "</AMOUNT><BANKALLOCATIONS.LIST><DATE>" + fnTransDate + "</DATE><INSTRUMENTDATE>" + fnDDDate + "</INSTRUMENTDATE><TRANSACTIONTYPE>Cheque/DD</TRANSACTIONTYPE><PAYMENTFAVOURING>" + paymentFavor + "</PAYMENTFAVOURING><STATUS>No</STATUS><INSTRUMENTNUMBER>" + ddNo + "</INSTRUMENTNUMBER><AMOUNT>-" + debit + "</AMOUNT></BANKALLOCATIONS.LIST></ALLLEDGERENTRIES.LIST></VOUCHER>");
                                    //}
                                    //else
                                    //{
                                    //    sbLedeDet.Append("<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + ledgName + "</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><AMOUNT>" + debit + "</AMOUNT></ALLLEDGERENTRIES.LIST>");
                                    //}

                                    //old code ended

                                    string newDt = (curDateAr[1] + "/" + curDateAr[0] + "/" + curDateAr[2]);
                                    string upQ = "update FT_FinDailyTransaction set IsExported=1 where TransDate ='" + newDt + "' and HeaderFK =" + selectHdrVal + " and Ledgerfk=" + ledgID + " and Transcode='" + rcptNo + "' and ISNULL(IsCanceled,0) =0 and PayMode in (4,5) ";
                                    DA.update_method_wo_parameter(upQ, "Text");
                                }
                                contentDiv.InnerHtml += sbLedeDet.ToString();
                            }
                        }
                    }

                    contentDiv.InnerHtml += xmlFtr;

                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";

                        string szFile = "Tally" + DateTime.Now.ToString("ddMMyyyy") + "-" + DateTime.Now.ToString("HHMMss") + ".xml";
                        //szFile = "Tally.xml";
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.LoadXml(contentDiv.InnerHtml.ToString());
                        xdoc.Save(szPath + szFile);

                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/xml";
                        Response.WriteFile(szPath + szFile);
                        Response.Flush();
                        Response.End();
                    }
                    lbl_alert.Text = "Exported Successfully";
                    //imgAlert.Visible = false;
                    contentDiv.InnerHtml = "";
                }
                else
                {
                    lbl_alert.Text = "No Records Available From " + txt_fromdate.Text + " To " + txt_todate.Text + "";
                    contentDiv.InnerHtml = "";
                }
            }
            catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Exporting!"; }
        }
        else
        {
            lbl_alert.Text = "No Accounts Available";
        }
    }
    protected void btnChlnUndo_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = true;
        contentDiv.InnerHtml = "";

        string selectedHdr = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Text : string.Empty;
        string selectHdrVal = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Value : "0";
        if (selectedHdr != string.Empty)
        {
            try
            {
                string strFnlYR = string.Empty;
                if (checkSchoolSetting() == 0)
                {
                    string finYr = string.Empty;
                    if (ddlfinyear.Items.Count > 0)
                    {
                        finYr = Convert.ToString(ddlfinyear.SelectedValue);
                        strFnlYR = " and actualfinyearfk in('" + finYr + "')";
                    }
                }
                string fromDate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
                string toDate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

                string upQ = "update FT_FinDailyTransaction set IsExported=0 where PayMode in (4,5)  and TransDate between '" + fromDate + "' and '" + toDate + "' and HeaderFK ='" + selectHdrVal + "' " + strFnlYR + "";
                DA.update_method_wo_parameter(upQ, "Text");
                lbl_alert.Text = "Updated Successfully";
            }
            catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Updating!"; }
        }
        else
        {
            lbl_alert.Text = "No Accounts Available";
        }
    }
    public void bindCollege()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Clear();
            ddl_college.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code";
            ds = DA.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); ddl_college.Items.Clear(); }
    }
    public void bindheader()
    {
        try
        {
            ddl_AccountDetail.Items.Clear();
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + userCode + " AND H.CollegeCode = " + collegeCode + "  ";

            DataSet dsHeader = DA.select_method_wo_parameter(query, "Text");
            if (dsHeader.Tables[0].Rows.Count > 0)
            {
                ddl_AccountDetail.DataSource = dsHeader;
                ddl_AccountDetail.DataTextField = "HeaderName";
                ddl_AccountDetail.DataValueField = "HeaderPK";
                ddl_AccountDetail.DataBind();
            }
        }
        catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); ddl_AccountDetail.Items.Clear(); }
    }
    private void xPortToXML()
    {
        StringBuilder sbXML = new StringBuilder();
        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(sbXML.ToString());
        xdoc.Save(Server.MapPath("~/Exports/Tally.xml"));
    }
    private string getLinkval()
    {
        string linkvalue = DA.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + userCode + "' and college_code ='" + collegeCode + "'");
        return linkvalue;
    }
    private string getFinYear()
    {
        string finYearid = string.Empty;
        try
        {
            finYearid = DA.getCurrentFinanceYear(userCode.ToString(), collegeCode.ToString());
        }
        catch { }
        return finYearid;
    }
    private void updateClgCode()
    {
        if (ddl_college.Items.Count > 0)
        {
            collegeCode = Convert.ToInt32(ddl_college.SelectedItem.Value);
        }
        else
        {
            collegeCode = 13;
        }
    }
    private string feePaidYear(int month, int year, int batchYr)
    {
        string retYear = string.Empty;
        int tmpYear = 1;
        if (batchYr == year)
        {
            tmpYear = (year - batchYr) + 1;
        }
        else if (year > batchYr)
        {
            if (month >= 6)
            {
                tmpYear = (year - batchYr) + 1;
            }
            else
            {
                tmpYear = (year - batchYr);
            }
        }

        switch (tmpYear)
        {
            case 1:
                retYear = "I";
                break;
            case 2:
                retYear = "II";
                break;
            case 3:
                retYear = "III";
                break;
            case 4:
                retYear = "IV";
                break;
            case 5:
                retYear = "V";
                break;
            default:
                retYear = "I";
                break;
        }
        return retYear;
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();

        lbl.Add(lblclg);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 05.07.2017 sudhagar
    #region financial year
    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + ddl_college.SelectedValue + "'  order by FinYearPK desc";

            ddlfinyear.Items.Clear();
            DataSet ds = DA.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    ddlfinyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    protected void ddl_college_Changed(object sender, EventArgs e)
    {
        trfnl.Visible = false;
        if (checkSchoolSetting() == 0)
        {
            loadfinanceyear();
            updateClgCode();
            trfnl.Visible = true;
        }
    }
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(DA.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }


    //Format 3 for Mahendra without bankname field
    private void ExportRcptForMEC()
    {
        imgAlert.Visible = true;
        contentDiv.InnerHtml = "";

        string selectedHdr = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Text : string.Empty;
        string selectHdrVal = ddl_AccountDetail.Items.Count > 0 ? ddl_AccountDetail.SelectedItem.Value : "0";
        if (selectedHdr != string.Empty)
        {
            try
            {
                string strFnlYR = string.Empty;
                if (checkSchoolSetting() == 0)
                {
                    string finYr = string.Empty;
                    if (ddlfinyear.Items.Count > 0)
                    {
                        finYr = Convert.ToString(ddlfinyear.SelectedValue);
                        strFnlYR = " and f.actualfinyearfk in('" + finYr + "')";
                    }
                }
                string fromDate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
                string toDate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

                string selectQ = "select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, r.Stud_Name, C.Course_Name+' '+dt.dept_acronym as DegreeName, R.Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L,Registration R,Degree d,Department dt,Course C where R.App_No =F.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =C.Course_Id and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and Memtype='1' and isnull(IsExported,0)<>1  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1' and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "' " + strFnlYR + "";
                DataSet dsExRecords = new DataSet();
                //dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                selectQ += " union select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, r.Stud_Name, C.Course_Name+' '+dt.dept_acronym as DegreeName, R.Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L,Applyn R,Degree d,Department dt,Course C where R.App_No =F.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =C.Course_Id and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and Memtype='1' and isnull(IsExported,0)<>1  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'  and r.app_no not in (select app_no from registration) " + strFnlYR + "";

                selectQ += " union select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, s.staff_name  as Stud_Name, '' DegreeName, '' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L,staffmaster s,staff_appl_master a where s.appl_no =a.appl_no and a.appl_id =F.App_No and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3)  and isnull(IsExported,0)<>1 and Memtype='2'  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";
                // dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                selectQ += " union  select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, VenContactName  as Stud_Name, '' DegreeName, '' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L, IM_VendorContactMaster c,CO_VendorMaster v where vendorcontactpk =F.App_No and  VendorFK =vendorpk and VendorType<>-5 and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1  and Memtype='3'  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'  and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";
                //dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");

                selectQ += " union select CONVERT(varchar(10), TransDate,103) as TransDate, TransCode, LedgerFK, Debit, l.LedgerName, vendorName as Stud_Name, '' DegreeName,'' Batch_Year, Paymode, (select TextVal from TextValTable where TextCriteria = 'BName' and TextCode=DDBankCode and college_code=" + collegeCode + ") as BankName,DDNo,CONVERT(varchar(10), DDDate,103)as DDDate, isnull(Memtype,'0') as Memtype,f.narration,f.DDBankBranch,feecategory from FT_FinDailyTransaction f,FM_LedgerMaster L, co_vendormaster where vendorpk =F.App_No and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK  and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) and isnull(IsExported,0)<>1 and VendorType=-5 and Memtype='4'  and isnull(f.IsDeposited,'0')='1' and isnull(f.IsCollected,'0')='1'   and TransDate between '" + fromDate + "' and '" + toDate + "' and f.HeaderFK ='" + selectHdrVal + "'";

                selectQ = selectQ + "  select b.bankName,f.bankfk,DailyTransID from ft_finbanktransaction f,Fm_finbankMaster b where b.bankpk=f.bankfk -- and Transdate between '" + fromDate + "' and '" + toDate + "' ";
                DataView dv = new DataView();
                dsExRecords = DA.select_method_wo_parameter(selectQ, "Text");
                if (dsExRecords.Tables.Count > 0 && dsExRecords.Tables[0].Rows.Count > 0)
                {
                    string xmlHdr = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA>   <REQUESTDESC><REPORTNAME>Vouchers</REPORTNAME><STATICVARIABLES><SVCURRENTCOMPANY>" + selectedHdr + "</SVCURRENTCOMPANY></STATICVARIABLES></REQUESTDESC><REQUESTDATA><TALLYMESSAGE xmlns:UDF=\"TallyUDF\">";
                    string xmlFtr = "</TALLYMESSAGE></REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

                    contentDiv.InnerHtml = xmlHdr;

                    //DataTable uniqueReceiptNo = dsExRecords.Tables[0].DefaultView.ToTable(true, "TransCode");
                    Hashtable htReceiptCode = new Hashtable();
                    for (int xRec = 0; xRec < dsExRecords.Tables[0].Rows.Count; xRec++)
                    {
                        string rcptNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["TransCode"]).Trim();
                        if (htReceiptCode.Contains(rcptNo))
                        {
                            continue;
                        }
                        else
                        {
                            htReceiptCode.Add(rcptNo, rcptNo);
                        }

                        string curDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["TransDate"]);
                        string[] curDateAr = curDate.Split('/');

                        if (curDateAr.Length == 3)
                        {
                            string debit = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["debit"]);
                            string fnTransDate = curDateAr[2] + curDateAr[1] + curDateAr[0];
                            string studName = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["Stud_Name"]);
                            string studCourse = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DegreeName"]);
                            int memtype = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["Memtype"]);
                            string studyearCourse = string.Empty;
                            if (memtype == 1)
                            {
                                int batchYr = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["Batch_Year"]);
                                string studYr = feePaidYear(Convert.ToInt32(curDateAr[1]), Convert.ToInt32(curDateAr[2]), batchYr);
                                studyearCourse = "(" + studYr + " " + studCourse + ")";
                            }

                            string ledgName = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["LedgerName"]);
                            string ledgID = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["LedgerFk"]);
                            int payMode = Convert.ToInt32(dsExRecords.Tables[0].Rows[xRec]["payMode"]);
                            string dispMode = "Cash";
                            string narration = studName + studyearCourse + " " + Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["narration"]);


                            dsExRecords.Tables[0].DefaultView.RowFilter = " TransCode='" + rcptNo + "'";
                            DataView dvMultiLedgers = dsExRecords.Tables[0].DefaultView;

                            StringBuilder sbMultiLedeDet = new StringBuilder();
                            double debitAMT = 0;
                            for (int xxx = 0; xxx < dvMultiLedgers.Count; xxx++)
                            {
                                double debitLED = Convert.ToDouble(Convert.ToString(dvMultiLedgers[xxx]["debit"]));
                                string ledgNameLED = Convert.ToString(dvMultiLedgers[xxx]["LedgerName"]);
                                sbMultiLedeDet.Append("<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + ledgNameLED + "</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><AMOUNT>" + debitLED + "</AMOUNT></ALLLEDGERENTRIES.LIST>");

                                debitAMT += debitLED;
                            }
                            debit = debitAMT.ToString();

                            if (payMode > 1)
                            {
                                string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDDate"]);
                                string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDNo"]);
                                string ddBranch = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDBankBranch"]);
                                dispMode = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["BankName"]);

                                narration += " Bank : " + dispMode + " Branch : " + ddBranch + " Date : " + ddDate + " No : " + ddNo + " ";

                                dsExRecords.Tables[1].DefaultView.RowFilter = "DailyTransID='" + rcptNo + "'";
                                dv = dsExRecords.Tables[1].DefaultView;
                                //if (dv.Count > 0)
                                //{
                                //    string BankName = Convert.ToString(dv[0]["bankName"]);
                                //    if (BankName.Trim() == "INDIAN OVERSEAS BANK")
                                //    {
                                //        dispMode = "Bank I O B";
                                //    }
                                //}
                            }
                            else
                            {
                                narration += " Cash Rs." + debit + " received from " + studName + studyearCourse + " towards Reason (Rcpt.No." + rcptNo + ")";
                            }

                            StringBuilder sbLedeDet = new StringBuilder();
                            sbLedeDet.Append("<VOUCHER VCHTYPE=\"RECEIPT\" ACTION=\"CREATE\"><DATE>" + fnTransDate + "</DATE><NARRATION>" + narration + "</NARRATION><VOUCHERTYPENAME>RECEIPT</VOUCHERTYPENAME><VOUCHERNUMBER>" + rcptNo + "</VOUCHERNUMBER><PARTYLEDGERNAME>" + dispMode + "</PARTYLEDGERNAME><EFFECTIVEDATE>" + fnTransDate + "</EFFECTIVEDATE><HASCASHFLOW>Yes</HASCASHFLOW>");


                            sbLedeDet.Append(sbMultiLedeDet.ToString() + "<ALLLEDGERENTRIES.LIST>");

                            if (payMode > 1)
                            {
                                string ddDate = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDDate"]);
                                string[] ddDateAr = ddDate.Split('/');
                                string fnDDDate = ddDateAr[2] + ddDateAr[1] + ddDateAr[0];
                                string ddNo = Convert.ToString(dsExRecords.Tables[0].Rows[xRec]["DDNo"]);
                                string paymentFavor = string.Empty;
                                string pay = string.Empty;
                                if (payMode == 2)
                                {
                                    pay = "Cheque";
                                }
                                if (payMode == 3)
                                {
                                    pay = "DD";
                                }
                                //sbLedeDet.Append("<LEDGERNAME>Bank I O B</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>-" + debit + "</AMOUNT><BANKALLOCATIONS.LIST><DATE>" + fnTransDate + "</DATE><INSTRUMENTDATE>" + fnDDDate + "</INSTRUMENTDATE><INSTRUMENTNUMBER>" + ddNo + "</INSTRUMENTNUMBER><NAME>Name-" + rcptNo + "</NAME><TRANSACTIONTYPE>Cheque/DD</TRANSACTIONTYPE><PAYMENTFAVOURING>" + ledgName + "</PAYMENTFAVOURING><STATUS>No</STATUS><UNIQUEREFERENCENUMBER>UNIQ" + rcptNo + "</UNIQUEREFERENCENUMBER><PAYMENTMODE>Transacted</PAYMENTMODE><BANKPARTYNAME>Bank I O B</BANKPARTYNAME><ISCONNECTEDPAYMENT>No</ISCONNECTEDPAYMENT><ISSPLIT>No</ISSPLIT><ISCONTRACTUSED>No</ISCONTRACTUSED><CHEQUEPRINTED> 1</CHEQUEPRINTED><AMOUNT>-" + debit + "</AMOUNT></BANKALLOCATIONS.LIST>");
                                sbLedeDet.Append("<LEDGERNAME>" + pay + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>-" + debit + "</AMOUNT><BANKALLOCATIONS.LIST><DATE>" + fnTransDate + "</DATE><INSTRUMENTDATE>" + fnDDDate + "</INSTRUMENTDATE><INSTRUMENTNUMBER>" + ddNo + "</INSTRUMENTNUMBER><NAME>Name-" + rcptNo + "</NAME><TRANSACTIONTYPE>Cheque/DD</TRANSACTIONTYPE><PAYMENTFAVOURING>" + ledgName + "</PAYMENTFAVOURING><STATUS>No</STATUS><UNIQUEREFERENCENUMBER>UNIQ" + rcptNo + "</UNIQUEREFERENCENUMBER><PAYMENTMODE>Transacted</PAYMENTMODE><BANKPARTYNAME>" + ledgName + "</BANKPARTYNAME><ISCONNECTEDPAYMENT>No</ISCONNECTEDPAYMENT><ISSPLIT>No</ISSPLIT><ISCONTRACTUSED>No</ISCONTRACTUSED><CHEQUEPRINTED> 1</CHEQUEPRINTED><AMOUNT>-" + debit + "</AMOUNT></BANKALLOCATIONS.LIST>");
                            }
                            else
                            {
                                sbLedeDet.Append("<LEDGERNAME>" + dispMode + "</LEDGERNAME><GSTCLASS /><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><AMOUNT>-" + debit + "</AMOUNT>");
                            }
                            sbLedeDet.Append("</ALLLEDGERENTRIES.LIST></VOUCHER>");

                            contentDiv.InnerHtml += sbLedeDet.ToString();

                            string newDt = (curDateAr[1] + "/" + curDateAr[0] + "/" + curDateAr[2]);
                            // string upQ = "update FT_FinDailyTransaction set IsExported=1 where TransDate ='" + newDt + "' and HeaderFK =" + selectHdrVal + " and Ledgerfk=" + ledgID + " and Transcode='" + rcptNo + "' and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) ";
                            string upQ = "update FT_FinDailyTransaction set IsExported=1 where TransDate ='" + newDt + "' and Transcode='" + rcptNo + "' and ISNULL(IsCanceled,0) =0 and PayMode in (1,2,3) ";
                            DA.update_method_wo_parameter(upQ, "Text");
                        }
                    }
                    contentDiv.InnerHtml += xmlFtr;

                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";

                        string szFile = "Tally" + DateTime.Now.ToString("ddMMyyyy") + "-" + DateTime.Now.ToString("HHMMss") + ".xml";
                        //szFile = "Tally.xml";
                        XmlDocument xdoc = new XmlDocument();
                        try
                        {
                            xdoc.LoadXml(contentDiv.InnerHtml.ToString());
                            xdoc.Save(szPath + szFile);
                        }
                        catch (Exception ex)
                        {
                            DA.sendErrorMail(ex, collegeCode.ToString(), contentDiv.InnerHtml.ToString()); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Exporting!";
                        }

                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/xml";
                        Response.WriteFile(szPath + szFile);
                        Response.Flush();
                        Response.End();
                        //HttpContext.Current.ApplicationInstance.CompleteRequest();

                    }
                    lbl_alert.Text = "Exported Successfully";
                    //imgAlert.Visible = false;
                    contentDiv.InnerHtml = "";

                }
                else
                {
                    lbl_alert.Text = "No Records Available From " + txt_fromdate.Text + " To " + txt_todate.Text + "";
                }
                contentDiv.InnerHtml = "";
            }
            catch (System.Threading.ThreadAbortException abrtEx) { }
            catch (Exception ex) { DA.sendErrorMail(ex, collegeCode.ToString(), "ExportXMLForTally"); contentDiv.InnerHtml = ""; lbl_alert.Text = "Error While Exporting!"; }
        }
        else
        {
            lbl_alert.Text = "No Accounts Available";
        }
    }

}