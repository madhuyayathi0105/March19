using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Configuration;

public partial class MonthlyFeesReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static bool usBasedRights = false;
    static ArrayList colord = new ArrayList();
    static byte roll = 0;
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
            Page.MaintainScrollPositionOnPostBack = false;
            if (!IsPostBack)
            {
                bindCollege();
                if (cblclg.Items.Count > 0)
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                // rblMemType_Selected(sender, e);
                bindheader();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                rdbCumulative.Checked = true;
                rdbduelist.Checked = false;
                rdbpaid.Checked = false;
                columnType();
                checkSchoolSetting();
                //loadpaid();
                //loadfinanceUser();
                ////  loadfinanceyear();
                //txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txt_fromdate.Attributes.Add("readonly", "readonly");
                //txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txt_todate.Attributes.Add("readonly", "readonly");
                //getPrintSettings();
                //columnType();
                //LoadIncludeSetting();
            }
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            // divcolorder.Attributes.Add("Style", "display:none;");
        }
        catch (Exception ex) { }
    }


    protected void bindCollege()
    {
        cblclg.Items.Clear();
        cbclg.Checked = false;
        txtclg.Text = "--Select--";
        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblclg.DataSource = ds;
            cblclg.DataTextField = "collname";
            cblclg.DataValueField = "college_code";
            cblclg.DataBind();
            if (cblclg.Items.Count > 0)
            {
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    cblclg.Items[row].Selected = true;
                }
                cbclg.Checked = true;
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
            }
        }
    }

    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        //bindheader();
        //loadpaid();
        //loadfinanceUser();
        //columnType();
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        // rblMemType_Selected(sender, e);
        //bindheader();
        //loadpaid();
        //loadfinanceUser();
        //columnType();
    }




    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        bindledger();
    }
    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        bindledger();
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studled, chkl_studled, txt_studled, lbl_ledger.Text, "--Select--");
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, lbl_ledger.Text, "--Select--");
    }
    public void rdbpaid_checkedChanged(object sender, EventArgs e)
    {
        if (rdbpaid.Checked == true)
        {
            rdbCumulative.Checked = false;
            rdbduelist.Checked = false;
            divcolorder.Visible = false;
        
        }
    }

    public void rdbduelist_checkedChanged(object sender, EventArgs e)
    {
        if (rdbduelist.Checked == true)
        {
            rdbCumulative.Checked = false;
            rdbpaid.Checked = false;
            divcolorder.Visible = false;
        }
    }

    public void rdbCumulative_checkedChanged(object sender, EventArgs e)
    {
        if (rdbCumulative.Checked == true)
        {
            rdbduelist.Checked = false;
            rdbpaid.Checked = false;
            divcolorder.Visible = false;
        }
    }
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

    public void bindheader()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            chkl_studhed.Items.Clear();
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            // string query = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode in('" + collegecode + "' ) ";
            //string query = " SELECT distinct HeaderName FROM FM_HeaderMaster where CollegeCode in('" + collegecode + "' ) ";
            string query = "SELECT distinct HeaderName,HeaderPK FROM FM_HeaderMaster h,FT_FeeAllot f where h.HeaderPK=f.HeaderFK and isnull(FeeAmountMonthly,'')<>'' and CollegeCode in('" + collegecode + "')";
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
                bindledger();
            }
        }
        catch
        {
        }
    }


    public void bindledger()
    {
        try
        {
            string headercode;

            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            headercode = Convert.ToString(getCblSelectedValue(chkl_studhed));
            chkl_studled.Items.Clear();
            txt_studled.Text = "--Select--";
            chk_studled.Checked = false;
            if (Convert.ToString(collegecode) != "" && Convert.ToString(headercode) != "")
            {
                //string query = " select distinct ledgername from FM_LedgerMaster l,FM_HeaderMaster h,FS_LedgerPrivilage P where l.HeaderFK =h.HeaderPK   and L.LedgerPK = P.LedgerFK and l.CollegeCode in('" + collegecode + "' ) and h.HeaderName in('" + headercode + "' )";

                string queryval = "select LinkValue from New_InsSettings where LinkName like '%-FineLedgerValue%' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
                DataSet linkds = new DataSet();
                linkds.Clear();
                linkds.Reset();
                string fineLedger = string.Empty;
                linkds = d2.select_method_wo_parameter(queryval, "text");
                if (linkds.Tables[0].Rows.Count > 0)
                {
                    string getval = Convert.ToString(linkds.Tables[0].Rows[0]["LinkValue"]);
                    if (getval.Contains('~'))
                    {
                        string[] splitval = getval.Split('~');
                        fineLedger = Convert.ToString(splitval[4]);
                    }
                }

                string query = " select distinct ledgername,LedgerPK from FM_LedgerMaster l,FM_HeaderMaster h,FS_LedgerPrivilage P,FT_FeeAllot f where f.LedgerFK=l.LedgerPK and isnull(FeeAmountMonthly,'')<>'' and l.HeaderFK =h.HeaderPK   and L.LedgerPK = P.LedgerFK and l.CollegeCode in('" + collegecode + "' ) and h.HeaderPK in('" + headercode + "' )";
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(query, "Text");
                string ledgerval = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ledgerval == "")
                        {
                            ledgerval = Convert.ToString(ds.Tables[0].Rows[i]["LedgerPK"]);

                        }
                        else
                        {
                            ledgerval = ledgerval +  "','" + Convert.ToString(ds.Tables[0].Rows[i]["LedgerPK"]);
                        
                        }
                        
                      
                    }
                
                }
                string allLedger = fineLedger + "','" + ledgerval;
                string legQuery = string.Empty;
                if (allLedger != "")
                {

                    legQuery = "select ledgername,LedgerPK from FM_LedgerMaster where LedgerPK in('" + allLedger + "')";
                
                }
               
                DataSet ldgrDs = new DataSet();
                ldgrDs.Clear();
                ldgrDs.Reset();
                ldgrDs = d2.select_method_wo_parameter(legQuery, "text");

                if (ldgrDs.Tables[0].Rows.Count > 0)
                {
                    chkl_studled.DataSource = ldgrDs;
                    chkl_studled.DataTextField = "ledgername";
                    chkl_studled.DataValueField = "LedgerPK";
                    chkl_studled.DataBind();
                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                    {
                        chkl_studled.Items[i].Selected = true;
                    }
                    txt_studled.Text = lbl_ledger.Text + "(" + chkl_studled.Items.Count + ")";
                    chk_studled.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    public void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            txtexcelname.Text = "";
            divcolorder.Visible = false;
            if (rdbCumulative.Checked == true)
            {
                bool boolCheck = false;
                string groupStr = string.Empty;
                string selColumn = getSelectedColumn(ref groupStr);

                string clgCode = Convert.ToString(getCblSelectedValue(cblclg));
                string fromdate = txt_fromdate.Text;
                string headerCode = Convert.ToString(getCblSelectedValue(chkl_studhed));


                string ledgerCode = Convert.ToString(getCblSelectedValue(chkl_studled));
                string todate = txt_todate.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

                string query = "select distinct r.App_No,r.Stud_Name from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and f.HeaderFK in('" + headerCode + "') and f.LedgerFK in('" + ledgerCode + "') and AllotMonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "' and AllotYear between '" + frdate[2].ToString() + "' and '" + tdate[2].ToString() + "'  and r.college_code in('" + clgCode + "') order by Stud_Name";

                query += " select r.Roll_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,fm.AllotMonth,r.App_No,fm.AllotYear,fm.AllotAmount,fm.PaidAmount,fm.BalAmount,fm.FeeAllotPK,f.ledgerfk,f.headerfk,f.feecategory from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and f.HeaderFK in('" + headerCode + "') and f.LedgerFK in('" + ledgerCode + "') and AllotMonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "' and AllotYear between '" + frdate[2].ToString() + "' and '" + tdate[2].ToString() + "'  and r.college_code in('" + clgCode + "')  order by Roll_No,AllotMonth";

                query += " select distinct fm.AllotMonth from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and f.HeaderFK in('" + headerCode + "') and f.LedgerFK in('" + ledgerCode + "') and AllotMonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "' and AllotYear between '" + frdate[2].ToString() + "' and '" + tdate[2].ToString() + "'  and r.college_code in('" + clgCode + "')  order by AllotMonth";

                //query += " select * from fm_finemaster where HeaderFK in('" + headerCode + "') and LedgerFK in('" + ledgerCode + "') and batchyear='" + tdate[2].ToString() + "' and finemonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "'";

                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(query, "text");


                if (ds.Tables[0].Rows.Count > 0)
                {
                    print.Visible=true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                    spreadDet.Visible = true;
                    imgdiv2.Visible = false;
                    lbl_alert.Text = " ";
                    spreadDet.Sheets[0].RowCount = 0;
                    spreadDet.Sheets[0].ColumnHeader.Rows.Count = 2;
                    spreadDet.Sheets[0].ColumnCount = 0;
                    spreadDet.CommandBar.Visible = false;
                    spreadDet.Sheets[0].AutoPostBack = true;
                    spreadDet.Sheets[0].RowHeader.Visible = false;
                    spreadDet.Sheets[0].ColumnCount = 1;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    string spHeadCol = getheadername();
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[0].Width = 50;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    int rollNo = 0;
                    int regNo = 0;
                    int admNo = 0;
                    bool boolroll = false;
                    string[] splVal = spHeadCol.Split(',');
                    for (int row = 0; row < splVal.Length; row++)
                    {
                        spreadDet.Sheets[0].ColumnCount++;

                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(splVal[row].Trim());
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
                        if (splVal[row].Trim() == "Name")
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 300;
                       
                        if (splVal[row].Trim() == "Admission No")
                        {
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                            admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                            boolroll = true;
                        }
                        if (splVal[row].Trim() == "Roll No")
                        {
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                            rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                            boolroll = true;
                        }
                        if (splVal[row].Trim() == "Reg No")
                        {
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                            regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                            boolroll = true;
                        }


                    }
                   

                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Month";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);


                    int ledgrCnt = 0;
                    
                    for (int ledger = 0; ledger < chkl_studled.Items.Count; ledger++)
                    {
                        ledgrCnt = spreadDet.Sheets[0].ColumnCount;
                        string ledgerValue = string.Empty;
                        if (chkl_studled.Items[ledger].Selected == true)
                        {
                            ledgerValue = Convert.ToString(chkl_studled.Items[ledger].Value);
                            ds.Tables[1].DefaultView.RowFilter = "LedgerFK='" + ledgerValue + "'";
                            DataTable dtLedgerBind = ds.Tables[0].DefaultView.ToTable();
                            if (dtLedgerBind.Rows.Count > 0)
                            {


                                string queryval = "select LinkValue from New_InsSettings where LinkName like '%FineLedgerValue%' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
                                DataSet linkds = new DataSet();
                                linkds.Clear();
                                linkds.Reset();
                                string fineLedger = string.Empty;
                                linkds = d2.select_method_wo_parameter(queryval, "text");
                                if (linkds.Tables[0].Rows.Count > 0)
                                {
                                    string getval = Convert.ToString(linkds.Tables[0].Rows[0]["LinkValue"]);
                                    if (getval.Contains('~'))
                                    {
                                        string[] splitval = getval.Split('~');
                                        fineLedger = Convert.ToString(splitval[4]);
                                    }
                                }

                              
                                    spreadDet.Sheets[0].ColumnCount++;

                                    // spreadDet.Sheets[0].ColumnCount = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount);
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Allot";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                                    spreadDet.Sheets[0].ColumnCount++;

                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Paid";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                                    spreadDet.Sheets[0].ColumnCount++;
                                    //  spreadDet.Sheets[0].ColumnCount = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount);
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Balance";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                              
                               
                            }
                            //spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].Text = chkl_studled.Items[ledger].Text.ToString();
                            //spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].Tag = ledgerValue;
                            //spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].HorizontalAlign = HorizontalAlign.Center;
                            //spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                            //spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, ledgrCnt, 1, 3);

                        }

                     

                    }

                    int sno = 0;

                    for (int dtTable = 0; dtTable < ds.Tables[0].Rows.Count; dtTable++)
                    {

                        string stuappNo = string.Empty;
                        string appNo = Convert.ToString(ds.Tables[0].Rows[dtTable]["app_no"]);
                        ds.Tables[1].DefaultView.RowFilter = "app_no='" + appNo + "'";
                        DataTable dtStuappfilter = ds.Tables[1].DefaultView.ToTable();


                        DateTime fromdateee = new DateTime();
                        fromdateee = TextToDate(txt_fromdate);
                        DateTime todateeee = new DateTime();
                        todateeee = TextToDate(txt_todate);
                        if (dtStuappfilter.Rows.Count > 0)
                        {

                            string stu_AppNo = Convert.ToString(dtStuappfilter.Rows[0]["app_no"]);

                            if (!stuappNo.Contains(stu_AppNo))
                            {
                                string studentName = Convert.ToString(dtStuappfilter.Rows[0]["stud_name"]);
                                string RollNo = Convert.ToString(dtStuappfilter.Rows[0]["Roll_No"]);
                                string regesterNo = Convert.ToString(dtStuappfilter.Rows[0]["Reg_No"]);
                                string AdmissionNo = Convert.ToString(dtStuappfilter.Rows[0]["Roll_Admit"]);
                                string batchyear = Convert.ToString(dtStuappfilter.Rows[0]["Batch_year"]);
                                string allotmonths = Convert.ToString(dtStuappfilter.Rows[0]["AllotMonth"]);
                                string degreecode=Convert.ToString(dtStuappfilter.Rows[0]["degree_code"]);

                               
                                sno++;
                                spreadDet.Sheets[0].RowCount++;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                               
                                int colcountVal = 0;
                               

                                for (int rows = 0; rows < splVal.Length; rows++)
                                {
                                    if (splVal[rows].Trim() == "Name")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows +1].Text =Convert.ToString(studentName);
                                    
                                    }

                                    if (splVal[rows].Trim() == "Admission No")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows +1].Text = Convert.ToString(AdmissionNo);
                                    }
                                    if (splVal[rows].Trim() == "Roll No")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows +1].Text = Convert.ToString(RollNo);
                                    }
                                    if (splVal[rows].Trim() == "Reg No")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows +1].Text = Convert.ToString(regesterNo);
                                    }
                                    colcountVal = rows;

                                }
                                colcountVal = colcountVal + 2;
                                
                                
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = studentName;
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = RollNo;
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                int rowcount = 0;
                                rowcount = spreadDet.Sheets[0].RowCount - 1;
                                for (int monthtab = 0; monthtab < ds.Tables[2].Rows.Count; monthtab++)
                                {
                                    int month_num = Convert.ToInt32(ds.Tables[2].Rows[monthtab]["AllotMonth"]);
                                    string strMonthName1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(month_num));
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcountVal].Text = Convert.ToString(strMonthName1);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcountVal].HorizontalAlign = HorizontalAlign.Left;
                                    
                                    int colcount = colcountVal;
                                    
                                    
                                    for (int ledger = 0; ledger < chkl_studled.Items.Count; ledger++)
                                    {
                                        if (chkl_studled.Items[ledger].Selected == true)
                                        {
                                            string ledgerValues = string.Empty;
                                            ledgerValues = Convert.ToString(chkl_studled.Items[ledger].Value);
                                            ds.Tables[1].DefaultView.RowFilter = "app_no='" + appNo + "' and AllotMonth='" + month_num + "' and  LedgerFK='" + ledgerValues + "'";
                                            DataTable dtStuappfilters = ds.Tables[1].DefaultView.ToTable();
                                            if (dtStuappfilters.Rows.Count > 0)
                                            {


                                                //string queryval = "select LinkValue from New_InsSettings where LinkName like '%FineLedgerValue%' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
                                                //DataSet linkds = new DataSet();
                                                //linkds.Clear();
                                                //linkds.Reset();
                                                //string fineLedger = string.Empty;
                                                //linkds = d2.select_method_wo_parameter(queryval, "text");
                                                //if (linkds.Tables[0].Rows.Count > 0)
                                                //{
                                                //    string getval = Convert.ToString(linkds.Tables[0].Rows[0]["LinkValue"]);
                                                //    if (getval.Contains('~'))
                                                //    {
                                                //        string[] splitval = getval.Split('~');
                                                //        fineLedger = Convert.ToString(splitval[4]);
                                                //    }
                                                //}

                                                //if (fineLedger.Trim().ToString() != ledgerValues.Trim().ToString())
                                                //{
                                                    string alloted = string.Empty;
                                                    string paidamt = string.Empty;
                                                    string blcamt = string.Empty;
                                                    string ledgerfk = Convert.ToString(dtStuappfilters.Rows[0]["ledgerfk"]);
                                                    string headerfk = Convert.ToString(dtStuappfilters.Rows[0]["headerfk"]);
                                                    string feecat = Convert.ToString(dtStuappfilters.Rows[0]["feecategory"]);
                                                    alloted = Convert.ToString(dtStuappfilters.Rows[0]["AllotAmount"]);
                                                    colcount++;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString
    (alloted);

                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                                    paidamt = Convert.ToString(dtStuappfilters.Rows[0]["PaidAmount"]);
                                                    colcount++;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(paidamt);

                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                                    blcamt = Convert.ToString(dtStuappfilters.Rows[0]["BalAmount"]);
                                                    colcount++;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString
    (blcamt);

                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                                    string s = d2.GetFunction("select PayAmount from OnlineFeeTransaction o,onlinefeetransactionmaster f where f.TransPk=o.TransFK and ledgerfk='" + ledgerfk + "' and feecat='" + feecat + "'  and fineflag=1 and appno='" + appNo + "'  and PaidStatus =1");//abarna and transdate between '" + fromdate + "' and '" + todate + "'
                                                    string transactiondate = d2.GetFunction("select transdate from OnlineFeeTransaction o,onlinefeetransactionmaster f where f.TransPk=o.TransFK and ledgerfk='" + ledgerfk + "' and feecat='" + feecat + "'  and fineflag=0 and appno='" + appNo + "'  and PaidStatus =1 and payamount='" + paidamt + "'");
                                                    string s1 = d2.GetFunction("select month from OnlineFeeTransaction o,onlinefeetransactionmaster f where f.TransPk=o.TransFK and ledgerfk='" + ledgerfk + "' and feecat='" + feecat + "' and fineflag=0 and appno='" + appNo + "'  and PaidStatus =1  and payamount='" + paidamt + "'");//and transdate between '" + fromdate + "' and '" + todate + "'
                                                    s = d2.GetFunction("select PayAmount from OnlineFeeTransaction o,onlinefeetransactionmaster f where f.TransPk=o.TransFK and ledgerfk='" + ledgerfk + "' and feecat='" + feecat + "'  and fineflag=1 and appno='" + appNo + "'  and PaidStatus =1 and transdate='" + transactiondate + "'");
                                                    if (s == "0" || s == "")
                                                    {
                                                    }
                                                    else
                                                    {
                                                        string month = string.Empty;
                                                        string amt = string.Empty;
                                                        if (s1 == "0" || s1 == "")
                                                        {
                                                             month = d2.GetFunction("select finemonth from FM_FineMaster where headerfk='" + headerfk + "' and LedgerFK ='" + ledgerfk + "' and FeeCatgory ='" + feecat + "' ");
                                                             amt = d2.GetFunction("select fineamount from FM_FineMaster where headerfk='" + headerfk + "' and LedgerFK ='" + ledgerfk + "' and FeeCatgory ='" + feecat + "' ");
                                                             amt = s;
                                                        }
                                                        else
                                                        {
                                                             month = d2.GetFunction("select finemonth from FM_FineMaster where headerfk='" + headerfk + "' and LedgerFK ='" + ledgerfk + "' and FeeCatgory ='" + feecat + "' and finemonth='" + s1 + "'");
                                                             amt = d2.GetFunction("select fineamount from FM_FineMaster where headerfk='" + headerfk + "' and LedgerFK ='" + ledgerfk + "' and FeeCatgory ='" + feecat + "' and finemonth='" + s1 + "'");
                                                             amt = s;
                                                        }
                                                        if (Convert.ToInt16(month) == month_num)
                                                        {
                                                            spreadDet.Sheets[0].RowCount++;
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount - 3].Text = "Fine";
                                                            if (amt == s)
                                                            {
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount - 1].Text = Convert.ToString(s);
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                            }
                                                            else
                                                            {
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount - 1].Text = Convert.ToString(s);
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                            }
                                                        }
                                                    }
                                               // }
                                               //// else if(fineLedger.Trim().ToString() == ledgerValues.Trim().ToString())
                                               // else
                                               // {
    //                                                //string alloted = string.Empty;
    //                                                //string paidamt = string.Empty;
    //                                                //string blcamt = string.Empty;
    //                                               // string value = d2.GetFunction("select distinct fineamount from fm_finemaster where HeaderFK in('" + headerCode + "') and LedgerFK in('" + fineLedger + "') and batchyear='" + batchyear + "' and finemonth ='" + allotmonths + "' and degreecode='" + degreecode + "'");
    //                                                //alloted = Convert.ToString(dtStuappfilters.Rows[0]["AllotAmount"]);
    //                                                if (value != "" || value !="0")
    //                                                {
    //                                                    alloted = "";

    //                                                }
    //                                                else
    //                                                {
    //                                                    alloted = value;
    //                                                }
    //                                                colcount++;
    //                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString
    //(alloted);

    //                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
    //                                                paidamt = Convert.ToString(dtStuappfilters.Rows[0]["PaidAmount"]);
    //                                                colcount++;
    //                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(paidamt);

    //                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
    //                                                colcount++;
    //                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString
    //(blcamt);

    //                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                                   
                                                //}

                                            }

                                        }
                                    }
                                    spreadDet.Sheets[0].RowCount++;
                                    
                                }
                                spreadDet.Sheets[0].RowCount--;
                                spreadDet.Sheets[0].SpanModel.Add(rowcount, 0, ds.Tables[2].Rows.Count, 1);//delsis
                                for (int rows = 0; rows < splVal.Length; rows++)
                                {
                                    spreadDet.Sheets[0].SpanModel.Add(rowcount, rows+1, ds.Tables[2].Rows.Count, 1);
                                }
                               
                            }
                        }

                    }
                    
                    spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                    spreadDet.SaveChanges();

                }
                else
                {
                    spreadDet.Visible = false;
                    print.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    btnprintmasterhed.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record Found";
                
                }


            }

            if (rdbpaid.Checked == true)//delsi2706
            {
                string clgCode = Convert.ToString(getCblSelectedValue(cblclg));
                string fromdate = txt_fromdate.Text;
                string headerCode = Convert.ToString(getCblSelectedValue(chkl_studhed));


                string ledgerCode = Convert.ToString(getCblSelectedValue(chkl_studled));
                string todate = txt_todate.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

                string query = "select distinct r.App_No,r.Stud_Name from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and f.HeaderFK in('" + headerCode + "') and f.LedgerFK in('" + ledgerCode + "') and AllotMonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "' and AllotYear between '" + frdate[2].ToString() + "' and '" + tdate[2].ToString() + "'  and r.college_code in('" + clgCode + "') and fm.BalAmount=0 and fm.AllotAmount!=0 order by Stud_Name";

                query += " select r.Roll_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.Batch_Year,fm.AllotMonth,r.App_No,fm.AllotYear,fm.FeeAllotPK,f.ledgerfk,fm.paidamount,f.headerfk,f.feecategory from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and f.HeaderFK in('" + headerCode + "') and f.LedgerFK in('" + ledgerCode + "') and AllotMonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "' and AllotYear between '" + frdate[2].ToString() + "' and '" + tdate[2].ToString() + "'  and r.college_code in('" + clgCode + "')  and fm.BalAmount=0 and fm.AllotAmount!=0  order by Roll_No,AllotMonth";
                //select r.Roll_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.Batch_Year,fm.AllotMonth,r.App_No,fm.AllotYear,ft.Debit,fm.FeeAllotPK,f.ledgerfk from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r,FT_FinDailyTransaction ft where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and ft.HeaderFK in('22') and ft.LedgerFK in('49') and AllotMonth between '05' and '07' and AllotYear between '2018' and '2018'  and r.college_code in('13') and fm.BalAmount=0 and fm.AllotAmount!=0  and f.app_no=ft.app_no and ft.transdate between '05/01/2018' and '06/30/2018' order by Roll_No,AllotMonth 


               

                query += " select distinct fm.AllotMonth from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and f.HeaderFK in('" + headerCode + "') and f.LedgerFK in('" + ledgerCode + "') and AllotMonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "' and AllotYear between '" + frdate[2].ToString() + "' and '" + tdate[2].ToString() + "'  and r.college_code in('" + clgCode + "') and fm.BalAmount=0  order by AllotMonth";//and fm.BalAmount=0 and fm.AllotAmount!=0 
                query += " select r.Roll_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.Batch_Year,r.App_No,ft.ledgerfk,ft.Debit as paidamount from Registration r,ft_findailytransaction ft where ft.App_No=r.App_No  and ft.HeaderFK in('" + headerCode + "') and ft.LedgerFK in('" + ledgerCode + "') and r.college_code in('" + clgCode + "')  and ft.transdate between '" + fromdate + "' and '" + todate + "'  order by Stud_Name";

                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(query, "text");


                if (ds.Tables[0].Rows.Count > 0)
                {
                    print.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                    spreadDet.Visible = true;
                    imgdiv2.Visible = false;
                    lbl_alert.Text = " ";
                    spreadDet.Sheets[0].RowCount = 0;
                    spreadDet.Sheets[0].ColumnHeader.Rows.Count = 2;
                    spreadDet.Sheets[0].ColumnCount = 0;
                    spreadDet.CommandBar.Visible = false;
                    spreadDet.Sheets[0].AutoPostBack = true;
                    spreadDet.Sheets[0].RowHeader.Visible = false;
                    spreadDet.Sheets[0].ColumnCount = 1;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    string spHeadCol = getheadername();
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[0].Width = 50;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                    string[] splVal = spHeadCol.Split(',');
                    int rollNo = 0;
                    int regNo = 0;
                    int admNo = 0;
                    bool boolroll = false;

                    for (int row = 0; row < splVal.Length; row++)
                    {
                        spreadDet.Sheets[0].ColumnCount++;

                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(splVal[row].Trim());
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
                        if (splVal[row].Trim() == "Name")
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 300;

                        if (splVal[row].Trim() == "Admission No")
                        {
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                            admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                            boolroll = true;
                        }
                        if (splVal[row].Trim() == "Roll No")
                        {
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                            rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                            boolroll = true;
                        }
                        if (splVal[row].Trim() == "Reg No")
                        {
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                            regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                            boolroll = true;
                        }


                    }

                  
                    spreadDet.Sheets[0].ColumnCount++;

                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Month";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);


                    int ledgrCnt = 0;
                   
                    for (int ledger = 0; ledger < chkl_studled.Items.Count; ledger++)
                    {
                        ledgrCnt = spreadDet.Sheets[0].ColumnCount;
                        string ledgerValue = string.Empty;
                        if (chkl_studled.Items[ledger].Selected == true)
                        {
                            ledgerValue = Convert.ToString(chkl_studled.Items[ledger].Value);
                            ds.Tables[1].DefaultView.RowFilter = "LedgerFK='" + ledgerValue + "'";
                            DataTable dtLedgerBind = ds.Tables[1].DefaultView.ToTable();
                            if (dtLedgerBind.Rows.Count > 0)
                            {
                               
                                spreadDet.Sheets[0].ColumnCount++;

                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Paid";
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                               
                            }
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].Text = chkl_studled.Items[ledger].Text.ToString();
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].Tag = ledgerValue;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, ledgrCnt, 1, 1);

                        }



                    }

                    int sno = 0;

                    for (int dtTable = 0; dtTable < ds.Tables[1].Rows.Count; dtTable++)
                    {

                        string stuappNo = string.Empty;
                        string appNo = Convert.ToString(ds.Tables[1].Rows[dtTable]["app_no"]);
                        ds.Tables[1].DefaultView.RowFilter = "app_no='" + appNo + "'";
                        DataTable dtStuappfilter = ds.Tables[1].DefaultView.ToTable();


                        DateTime fromdateee = new DateTime();
                        fromdateee = TextToDate(txt_fromdate);
                        DateTime todateeee = new DateTime();
                        todateeee = TextToDate(txt_todate);
                        if (dtStuappfilter.Rows.Count > 0)//delsiref
                        {

                            string stu_AppNo = Convert.ToString(dtStuappfilter.Rows[0]["app_no"]);

                            if (!stuappNo.Contains(stu_AppNo))
                            {
                                string studentName = Convert.ToString(dtStuappfilter.Rows[0]["stud_name"]);
                                string RollNo = Convert.ToString(dtStuappfilter.Rows[0]["Roll_No"]);
                                string regesterNo = Convert.ToString(dtStuappfilter.Rows[0]["Reg_No"]);
                                string AdmissionNo = Convert.ToString(dtStuappfilter.Rows[0]["Roll_Admit"]);

                                sno++;
                                spreadDet.Sheets[0].RowCount++;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);

                                int colcountVal = 0;

                                for (int rows = 0; rows < splVal.Length; rows++)
                                {
                                    if (splVal[rows].Trim() == "Name")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows + 1].Text = Convert.ToString(studentName);

                                    }

                                    if (splVal[rows].Trim() == "Admission No")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows + 1].Text = Convert.ToString(AdmissionNo);
                                    }
                                    if (splVal[rows].Trim() == "Roll No")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows + 1].Text = Convert.ToString(RollNo);
                                    }
                                    if (splVal[rows].Trim() == "Reg No")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows + 1].Text = Convert.ToString(regesterNo);
                                    }
                                    colcountVal = rows;

                                }
                                colcountVal = colcountVal + 2;
                                int rowcount = 0;
                                rowcount = spreadDet.Sheets[0].RowCount - 1;

                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = studentName;
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = RollNo;
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                for (int monthtab = 0; monthtab < ds.Tables[2].Rows.Count; monthtab++)
                                {
                                    int month_num = Convert.ToInt32(ds.Tables[2].Rows[monthtab]["AllotMonth"]);
                                    string strMonthName1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(month_num));
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcountVal].Text = Convert.ToString(strMonthName1);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcountVal].HorizontalAlign = HorizontalAlign.Left;

                                    int colcount = colcountVal;
                                    for (int ledger = 0; ledger < chkl_studled.Items.Count; ledger++)
                                    {
                                        if (chkl_studled.Items[ledger].Selected == true)
                                        {
                                            string ledgerValues = string.Empty;
                                            ledgerValues = Convert.ToString(chkl_studled.Items[ledger].Value);
                                            ds.Tables[1].DefaultView.RowFilter = "app_no='" + appNo + "'  and   AllotMonth='" + month_num + "' and LedgerFK='" + ledgerValues + "'";//and AllotMonth='" + month_num + "'
                                            DataTable dtStuappfilters = ds.Tables[1].DefaultView.ToTable();
                                            ////for (int k = 0; k < dtStuappfilters.Rows.Count; k++)
                                            ////{
                                            //    string feeallotpk = d2.GetFunction("select feeallotpk from ft_feeallot where app_no='" + appNo + "'  and  LedgerFK='" + ledgerValues + "'");
                                            //    string allotmonth = "select allotmonth,allotyear,paidamount,balamount from ft_feeallotmonthly where feeallotpk='" + feeallotpk + "'";
                                            //    DataSet month = d2.select_method_wo_parameter(allotmonth, "Text");
                                                if (dtStuappfilters.Rows.Count > 0)
                                                {

                                                    string paidamt = string.Empty;
                                                    string headerfk = Convert.ToString(dtStuappfilters.Rows[0]["headerfk"]);
                                                    string feecat = Convert.ToString(dtStuappfilters.Rows[0]["feecategory"]);
                                                    paidamt = Convert.ToString(dtStuappfilters.Rows[0]["PaidAmount"]);
                                                    string ledgerfk=Convert.ToString(dtStuappfilters.Rows[0]["ledgerfk"]);
                                                    colcount++;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(paidamt);

                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                                    string s = d2.GetFunction("select PayAmount from OnlineFeeTransaction o,onlinefeetransactionmaster f where f.TransPk=o.TransFK and ledgerfk='" + ledgerfk + "' and feecat='" + feecat + "'  and fineflag=1 and appno='" + appNo + "'  and PaidStatus =1");//abarna and transdate between '" + fromdate + "' and '" + todate + "'
                                                    string transactiondate = d2.GetFunction("select transdate from OnlineFeeTransaction o,onlinefeetransactionmaster f where f.TransPk=o.TransFK and ledgerfk='" + ledgerfk + "' and feecat='" + feecat + "'  and fineflag=0 and appno='" + appNo + "'  and PaidStatus =1 and payamount='" + paidamt + "'");
                                                    string s1 = d2.GetFunction("select month from OnlineFeeTransaction o,onlinefeetransactionmaster f where f.TransPk=o.TransFK and ledgerfk='" + ledgerfk + "' and feecat='" + feecat + "' and fineflag=0 and appno='" + appNo + "'  and PaidStatus =1  and payamount='" + paidamt + "'");//and transdate between '" + fromdate + "' and '" + todate + "'
                                                    s = d2.GetFunction("select PayAmount from OnlineFeeTransaction o,onlinefeetransactionmaster f where f.TransPk=o.TransFK and ledgerfk='" + ledgerfk + "' and feecat='" + feecat + "'  and fineflag=1 and appno='" + appNo + "'  and PaidStatus =1 and transdate='" + transactiondate + "'");
                                                    if (s == "0" || s == "")
                                                    {
                                                    }
                                                    else
                                                    {
                                                        string month = string.Empty;
                                                        string amt = string.Empty;
                                                        if (s1 == "0" || s1 == "")
                                                        {
                                                            month = d2.GetFunction("select finemonth from FM_FineMaster where headerfk='" + headerfk + "' and LedgerFK ='" + ledgerfk + "' and FeeCatgory ='" + feecat + "' ");
                                                            amt = d2.GetFunction("select fineamount from FM_FineMaster where headerfk='" + headerfk + "' and LedgerFK ='" + ledgerfk + "' and FeeCatgory ='" + feecat + "' ");
                                                            amt = s;
                                                        }
                                                        else
                                                        {
                                                            month = d2.GetFunction("select finemonth from FM_FineMaster where headerfk='" + headerfk + "' and LedgerFK ='" + ledgerfk + "' and FeeCatgory ='" + feecat + "' and finemonth='" + s1 + "'");
                                                            amt = d2.GetFunction("select fineamount from FM_FineMaster where headerfk='" + headerfk + "' and LedgerFK ='" + ledgerfk + "' and FeeCatgory ='" + feecat + "' and finemonth='" + s1 + "'");
                                                            amt = s;
                                                        }
                                                        if (Convert.ToInt16(month) == month_num)
                                                        {
                                                            spreadDet.Sheets[0].RowCount++;
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount - 1].Text = "Fine";
                                                            if (amt == s)
                                                            {
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(s);
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                                            }
                                                            else
                                                            {
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(s);
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;
                                                            }
                                                        }
                                                    }

                                                }
                                            //}

                                        }
                                    }
                                    spreadDet.Sheets[0].RowCount++;
                                }
                                spreadDet.Sheets[0].RowCount--;
                                spreadDet.Sheets[0].SpanModel.Add(rowcount, 0, ds.Tables[2].Rows.Count, 1);//delsis
                                for (int rows = 0; rows < splVal.Length; rows++)
                                {
                                    spreadDet.Sheets[0].SpanModel.Add(rowcount, rows + 1, ds.Tables[2].Rows.Count, 1);
                                }
                            }
                        }

                    }

                    spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                    spreadDet.SaveChanges();

                }
                else
                {
                    spreadDet.Visible = false;
                    print.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    btnprintmasterhed.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record Found";

                }
            }

            if (rdbduelist.Checked == true)
            {
                string clgCode = Convert.ToString(getCblSelectedValue(cblclg));
                string fromdate = txt_fromdate.Text;
                string headerCode = Convert.ToString(getCblSelectedValue(chkl_studhed));


                string ledgerCode = Convert.ToString(getCblSelectedValue(chkl_studled));
                string todate = txt_todate.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();


                string query = "select distinct r.App_No,r.Stud_Name from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and f.HeaderFK in('" + headerCode + "') and f.LedgerFK in('" + ledgerCode + "') and AllotMonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "' and AllotYear between '" + frdate[2].ToString() + "' and '" + tdate[2].ToString() + "'  and r.college_code in('" + clgCode + "') and fm.BalAmount!=0 and fm.AllotAmount!=0 order by Stud_Name";

                query += " select r.Roll_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.Batch_Year,fm.AllotMonth,r.App_No,fm.AllotYear,fm.BalAmount,fm.FeeAllotPK,f.ledgerfk from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and f.HeaderFK in('" + headerCode + "') and f.LedgerFK in('" + ledgerCode + "') and AllotMonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "' and AllotYear between '" + frdate[2].ToString() + "' and '" + tdate[2].ToString() + "'  and r.college_code in('" + clgCode + "') and fm.BalAmount!=0 and fm.AllotAmount!=0  order by Roll_No,AllotMonth";

                query += " select distinct fm.AllotMonth from FT_FeeallotMonthly fm,FT_FeeAllot f,Registration r where f.App_No=r.App_No and f.FeeAllotPK=fm.FeeAllotPK and f.HeaderFK in('" + headerCode + "') and f.LedgerFK in('" + ledgerCode + "') and AllotMonth between '" + frdate[1].ToString() + "' and '" + tdate[1].ToString() + "' and AllotYear between '" + frdate[2].ToString() + "' and '" + tdate[2].ToString() + "'  and r.college_code in('" + clgCode + "') and fm.BalAmount!=0 and fm.AllotAmount!=0  order by AllotMonth";

                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(query, "text");


                if (ds.Tables[0].Rows.Count > 0)
                {
                    print.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                    spreadDet.Visible = true;
                    imgdiv2.Visible = false;
                    lbl_alert.Text = " ";
                    spreadDet.Sheets[0].RowCount = 0;
                    spreadDet.Sheets[0].ColumnHeader.Rows.Count = 2;
                    spreadDet.Sheets[0].ColumnCount = 0;
                    spreadDet.CommandBar.Visible = false;
                    spreadDet.Sheets[0].AutoPostBack = true;
                    spreadDet.Sheets[0].RowHeader.Visible = false;
                    spreadDet.Sheets[0].ColumnCount = 1;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[0].Width = 50;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    string spHeadCol = getheadername();
                    string[] splVal = spHeadCol.Split(',');
                    int rollNo = 0;
                    int regNo = 0;
                    int admNo = 0;
                    bool boolroll = false;
                    for (int row = 0; row < splVal.Length; row++)
                    {
                        spreadDet.Sheets[0].ColumnCount++;

                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(splVal[row].Trim());
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
                        if (splVal[row].Trim() == "Name")
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 300;

                        if (splVal[row].Trim() == "Admission No")
                        {
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                            admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                            boolroll = true;
                        }
                        if (splVal[row].Trim() == "Roll No")
                        {
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                            rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                            boolroll = true;
                        }
                        if (splVal[row].Trim() == "Reg No")
                        {
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                            regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                            boolroll = true;
                        }

                    }


                    spreadDet.Sheets[0].ColumnCount++;


                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Month";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);


                    int ledgrCnt = 0;

                    for (int ledger = 0; ledger < chkl_studled.Items.Count; ledger++)
                    {
                        ledgrCnt = spreadDet.Sheets[0].ColumnCount;
                        string ledgerValue = string.Empty;
                        if (chkl_studled.Items[ledger].Selected == true)
                        {
                            ledgerValue = Convert.ToString(chkl_studled.Items[ledger].Value);
                            ds.Tables[1].DefaultView.RowFilter = "LedgerFK='" + ledgerValue + "'";
                            DataTable dtLedgerBind = ds.Tables[0].DefaultView.ToTable();
                            if (dtLedgerBind.Rows.Count > 0)
                            {

                                spreadDet.Sheets[0].ColumnCount++;

                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Balance";
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                            }
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].Text = chkl_studled.Items[ledger].Text.ToString();
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].Tag = ledgerValue;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, ledgrCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, ledgrCnt, 1, 1);

                        }



                    }

                    int sno = 0;

                    for (int dtTable = 0; dtTable < ds.Tables[0].Rows.Count; dtTable++)
                    {

                        string stuappNo = string.Empty;
                        string appNo = Convert.ToString(ds.Tables[0].Rows[dtTable]["app_no"]);
                        ds.Tables[1].DefaultView.RowFilter = "app_no='" + appNo + "'";
                        DataTable dtStuappfilter = ds.Tables[1].DefaultView.ToTable();


                        DateTime fromdateee = new DateTime();
                        fromdateee = TextToDate(txt_fromdate);
                        DateTime todateeee = new DateTime();
                        todateeee = TextToDate(txt_todate);
                        if (dtStuappfilter.Rows.Count > 0)
                        {

                            string stu_AppNo = Convert.ToString(dtStuappfilter.Rows[0]["app_no"]);

                            if (!stuappNo.Contains(stu_AppNo))
                            {
                                string studentName = Convert.ToString(dtStuappfilter.Rows[0]["stud_name"]);
                                string RollNo = Convert.ToString(dtStuappfilter.Rows[0]["Roll_No"]);
                                string regesterNo = Convert.ToString(dtStuappfilter.Rows[0]["Reg_No"]);
                                string AdmissionNo = Convert.ToString(dtStuappfilter.Rows[0]["Roll_Admit"]);

                                sno++;
                                spreadDet.Sheets[0].RowCount++;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);

                                int colcountVal = 0;

                                for (int rows = 0; rows < splVal.Length; rows++)
                                {
                                    if (splVal[rows].Trim() == "Name")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows + 1].Text = Convert.ToString(studentName);

                                    }

                                    if (splVal[rows].Trim() == "Admission No")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows + 1].Text = Convert.ToString(AdmissionNo);
                                    }
                                    if (splVal[rows].Trim() == "Roll No")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows + 1].Text = Convert.ToString(RollNo);
                                    }
                                    if (splVal[rows].Trim() == "Reg No")
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, rows + 1].Text = Convert.ToString(regesterNo);
                                    }
                                    colcountVal = rows;

                                }
                                colcountVal = colcountVal + 2;

                                int rowcount = 0;
                                rowcount = spreadDet.Sheets[0].RowCount - 1;
                               
                                for (int monthtab = 0; monthtab < ds.Tables[2].Rows.Count; monthtab++)
                                {
                                    int month_num = Convert.ToInt32(ds.Tables[2].Rows[monthtab]["AllotMonth"]);
                                    string strMonthName1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(month_num));
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcountVal].Text = Convert.ToString(strMonthName1);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcountVal].HorizontalAlign = HorizontalAlign.Left;

                                    int colcount = colcountVal;
                                    for (int ledger = 0; ledger < chkl_studled.Items.Count; ledger++)
                                    {
                                        if (chkl_studled.Items[ledger].Selected == true)
                                        {
                                            string ledgerValues = string.Empty;
                                            ledgerValues = Convert.ToString(chkl_studled.Items[ledger].Value);
                                            ds.Tables[1].DefaultView.RowFilter = "app_no='" + appNo + "' and AllotMonth='" + month_num + "' and  LedgerFK='" + ledgerValues + "'";
                                            DataTable dtStuappfilters = ds.Tables[1].DefaultView.ToTable();
                                            if (dtStuappfilters.Rows.Count > 0)
                                            {

                                                string paidamt = string.Empty;


                                                paidamt = Convert.ToString(dtStuappfilters.Rows[0]["BalAmount"]);
                                                colcount++;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcount].Text = Convert.ToString(paidamt);

                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcount].HorizontalAlign = HorizontalAlign.Right;

                                            }

                                        }
                                    }
                                    spreadDet.Sheets[0].RowCount++;
                                }
                                spreadDet.Sheets[0].RowCount--;

                                spreadDet.Sheets[0].SpanModel.Add(rowcount, 0, ds.Tables[2].Rows.Count, 1);//delsis
                                for (int rows = 0; rows < splVal.Length; rows++)
                                {
                                    spreadDet.Sheets[0].SpanModel.Add(rowcount, rows + 1, ds.Tables[2].Rows.Count, 1);
                                }
                            }
                        }

                    }

                    spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                    spreadDet.SaveChanges();

                }
                else
                {
                    spreadDet.Visible = false;
                    print.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    btnprintmasterhed.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record Found";

                }

            
            }
           
        }
        catch (Exception ex)
        {

        }
    }


    public DateTime TextToDate(TextBox txt)
    {
        DateTime dt = new DateTime();
        string firstdate = Convert.ToString(txt.Text);
        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        return dt;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }



    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            // lblvalidation1.Text = "";
            string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
         //   string counterName = getCounterName(Convert.ToString(getCblSelectedValue(cbluser)));

            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            degreedetails = "Individual Student Daybook Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
            //  degreedetails = "Individual Student Daybook Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "User/Counter : " + counterName;
            pagename = "FinanceBalDet.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails, 0, Convert.ToString(Session["usercode"]));
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    //protected void btn_print_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblvalidation1.Text = "";
    //        string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
    //      //  string counterName = getCounterName(Convert.ToString(getCblSelectedValue(cbluser)));

    //        txtexcelname.Text = "";
    //        string degreedetails;
    //        string pagename;
    //        degreedetails = "Monthly Fees Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
    //        pagename = "MonthlyFeesReport.aspx";
    //        Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails, 1, Convert.ToString(Session["usercode"]));
    //        Printcontrolhed.Visible = true;
    //    }
    //    catch { }
    //}

    protected string getclgAcr(string collegecode)
    {
        string strAcr = string.Empty;
        try
        {
            StringBuilder clgAcr = new StringBuilder();
            string selQ = " select collname,college_code,coll_acronymn as acr from collinfo where college_code in('" + collegecode + "')";
            DataSet dsclg = d2.select_method_wo_parameter(selQ, "Text");
            if (dsclg.Tables.Count > 0 && dsclg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsclg.Tables[0].Rows.Count; row++)
                {
                    clgAcr.Append(Convert.ToString(dsclg.Tables[0].Rows[row]["acr"]) + ",");
                }
                if (clgAcr.Length > 0)
                    clgAcr.Remove(clgAcr.Length - 1, 1);
                strAcr = Convert.ToString(clgAcr);
            }
        }
        catch { strAcr = string.Empty; }
        return strAcr;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void lnkcolorder_Click(object sender, EventArgs e)
    {
        divcolorder.Visible = true;
        txtcolorder.Text = string.Empty;
        loadcolumnorder();
        columnType();
        ddlreport_SelectedIndexChanged(sender, e);

        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        
    }


    public void loadcolumnorder()
    {
        cblcolumnorder.Items.Clear();

        {
            cblcolumnorder.Items.Add(new ListItem("Name", "1"));
            cblcolumnorder.Items.Add(new ListItem("Roll No", "2"));
            cblcolumnorder.Items.Add(new ListItem("Reg No", "3"));
            cblcolumnorder.Items.Add(new ListItem("Admission No", "4"));

            // cblcolumnorder.Items.Add(new ListItem("Dept Name", "3"));
          //  cblcolumnorder.Items.Add(new ListItem("Ledger", "5"));
           

        }
    }

    protected void btncolorderOK_Click(object sender, EventArgs e)
    {

        divcolorder.Visible = true;
        if (getsaveColumnOrder())
        {
            divcolorder.Attributes.Add("Style", "display:none;");
        }
    }

    protected bool getsaveColumnOrder()
    {
        bool boolSave = false;
        try
        {
            string strText = string.Empty;
            if (cblcolumnorder.Items.Count > 0)
                strText = Convert.ToString(getCblSelectedTextwithout(cblcolumnorder));
            if (!string.IsNullOrEmpty(strText))
                strText = Convert.ToString(txtcolorder.Text);
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0" && !string.IsNullOrEmpty(strText))
            {
                string SelQ = " if exists (select * from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "')update New_InsSettings set linkvalue='" + strText + "' where  LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "' else insert into New_InsSettings(LinkName,linkvalue,user_code,college_code) values('" + linkName + "','" + strText + "','" + usercode + "','" + Usercollegecode + "')";
                int insQ = d2.update_method_wo_parameter(SelQ, "Text");
                boolSave = true;
            }
            if (!boolSave)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please select corresponding values!')", true);
            }
        }
        catch { }
        return boolSave;
    }


    private string getCblSelectedTextwithout(CheckBoxList cblSelected)
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
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    protected void btnDel_OnClick(object sender, EventArgs e)
    {
        deleteReportType();
    }
    protected void deleteReportType()
    {
        int delMQ = 0;
        string Usercollegecode = string.Empty;
        if (Session["collegecode"] != null)
            Usercollegecode = Convert.ToString(Session["collegecode"]);
        string linkName = string.Empty;
        if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
            linkName = Convert.ToString(ddlreport.SelectedItem.Text);
        if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            string linkCriteria = string.Empty;
           
           
                linkCriteria = "MonthlyFeesReport";
            int delQ = 0;
            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "'", "Text")), out delQ);

            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete  from CO_MasterValues where MasterCriteria='" + linkCriteria + "' and mastervalue='" + linkName + "'  and collegecode='" + Usercollegecode + "'", "Text")), out delMQ);

        }
        if (delMQ > 0)
        {
            txtcolorder.Text = string.Empty;
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                cblcolumnorder.Items[i].Selected = false;
            }
            cb_column.Checked = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Failed')", true);
        columnType();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
    }


    public void columnType()
    {
        string Usercollegecode = string.Empty;
        if (Session["collegecode"] != null)
            Usercollegecode = Convert.ToString(Session["collegecode"]);
        ddlreport.Items.Clear();
        ddlMainreport.Items.Clear();
        if (!string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            string linkCriteria = string.Empty;
           
                linkCriteria = "MonthlyFeesReport";
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='" + linkCriteria + "' and CollegeCode='" + Usercollegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlreport.DataSource = ds;
                ddlreport.DataTextField = "MasterValue";
                ddlreport.DataValueField = "MasterCode";
                ddlreport.DataBind();
               
                ddlMainreport.DataSource = ds;
                ddlMainreport.DataTextField = "MasterValue";
                ddlMainreport.DataValueField = "MasterCode";
                ddlMainreport.DataBind();
                //ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlreport.Items.Insert(0, new ListItem("Select", "0"));
                ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }
    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        selectReportType();
    }

    protected void selectReportType()
    {
        try
        {
            bool boolClear = false;
            bool boolcheck = false;
            string getName = string.Empty;
            txtcolorder.Text = string.Empty;
            string strText = string.Empty;
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                getName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' ");
                if (!string.IsNullOrEmpty(getName) && getName != "0")
                {
                    string[] splName = getName.Split(',');
                    if (splName.Length > 0)
                    {
                        for (int sprow = 0; sprow < splName.Length; sprow++)
                        {
                            for (int flt = 0; flt < cblcolumnorder.Items.Count; flt++)
                            {
                                if (splName[sprow].Trim() == cblcolumnorder.Items[flt].Text.Trim())
                                {
                                    cblcolumnorder.Items[flt].Selected = true;
                                    boolcheck = true;
                                    // strText += cblcolumnorder.Items[flt].Text;
                                }
                            }
                        }
                    }
                }
                else
                    boolClear = true;
            }
            else
                boolClear = true;
            if (boolClear)
            {
                txtcolorder.Text = string.Empty;
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                }
                cb_column.Checked = false;
            }
            if (boolcheck)
            {
                txtcolorder.Text = getName;
            }
        }
        catch { }
    }

    protected void btnaddtype_Click(object sender, EventArgs e)
    {
        try
        {

            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string strDesc = Convert.ToString(txtdesc.Text);
            string linkCriteria = string.Empty;
           
                linkCriteria = "MonthlyFeesReport";
            if (!string.IsNullOrEmpty(strDesc) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkCriteria + "' and CollegeCode ='" + Usercollegecode + "') update CO_MasterValues set MasterValue ='" + strDesc + "' where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkCriteria + "' and CollegeCode ='" + Usercollegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + strDesc + "','" + linkCriteria + "','" + Usercollegecode + "')";
                int insert = d2.update_method_wo_parameter(sql, "Text");
                if (insert > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true); txtdesc.Text = string.Empty;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter report type')", true);
            }
            columnType();
            divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        }
        catch { }
    }



    protected string getSelectedColumn(ref string groupStr)
    {
        string val = string.Empty;
        try
        {
            StringBuilder strCol = new StringBuilder();
            StringBuilder grpstrCol = new StringBuilder();
            Hashtable htcolumn = htcolumnValue();
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splVal = selQ.Split(',');
                    if (splVal.Length > 0)
                    {
                        for (int row = 0; row < splVal.Length; row++)
                        {
                            string tempSel = Convert.ToString(htcolumn[splVal[row].Trim()]);
                            strCol.Append(tempSel + ",");
                            if (tempSel != "sum(debit) as debit" && tempSel != "sum(credit) as credit")
                            {
                                if (tempSel == "convert(varchar(10),transdate,103)as transdate")
                                    tempSel = "transdate";

                                grpstrCol.Append(tempSel + ",");
                            }
                        }
                    }
                }
                if (strCol.Length > 0 && grpstrCol.Length > 0)
                {
                    strCol.Remove(strCol.Length - 1, 1);
                    val = Convert.ToString(strCol);
                    grpstrCol.Remove(grpstrCol.Length - 1, 1);
                    groupStr = Convert.ToString(grpstrCol);
                }
            }
        }
        catch { }
        return val;
    }
    protected Hashtable htcolumnValue()
    {
        Hashtable htcol = new Hashtable();
        try
        {
            
                htcol.Add("Name", "stud_name");
                htcol.Add("Roll No", "roll_no");
                htcol.Add("Reg No", "reg_no");
                htcol.Add("Admission No", "roll_admit");
               // htcol.Add("Dept Name", "degree_code");
            
        }
        catch { }
        return htcol;
    }


    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }

    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }

     protected string getheadername()
    {
        string selQ = string.Empty;
        try
        {
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(Session["collegecode"]);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
            }
        }
        catch { }
        return selQ;
    }

     private double checkSchoolSetting()//delsi
     {
         double getVal = 0;
         double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
         return getVal;
     }
}