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
using System.Configuration;

public partial class MulInstHdCollection : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static bool usBasedRights = false;
    Dictionary<int, string> dicHeaderWise = new Dictionary<int, string>();
    static Dictionary<int, string> dicColumnVisible = new Dictionary<int, string>();
    static Dictionary<int, string> dicColumnAlignment = new Dictionary<int, string>();
    int grdRow = 0;
    ArrayList arrColHdrNames = new ArrayList();

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
                setLabelText();
                bindCollege();
                if (cblclg.Items.Count > 0)
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                rblMemType_Selected(sender, e);
            }
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
        }
        catch (Exception ex) { }
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
        // fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    #region college

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
        rblMemType_Selected(sender, e);
    }

    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        rblMemType_Selected(sender, e);
    }

    #endregion

    #region header

    public void bindheader()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            chkl_studhed.Items.Clear();
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            // string query = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode in('" + collegecode + "' ) ";
            string query = " SELECT distinct HeaderName FROM FM_HeaderMaster where CollegeCode in('" + collegecode + "' ) ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderName";
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

    #endregion

    #region Ledger

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
                string query = " select distinct ledgername from FM_LedgerMaster l,FM_HeaderMaster h,FS_LedgerPrivilage P where l.HeaderFK =h.HeaderPK and L.LedgerPK = P.LedgerFK and   l.CollegeCode in('" + collegecode + "' ) and h.HeaderName in('" + headercode + "' )";
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chkl_studled.DataSource = ds;
                    chkl_studled.DataTextField = "ledgername";
                    chkl_studled.DataValueField = "ledgername";
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

    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studled, chkl_studled, txt_studled, lbl_ledger.Text, "--Select--");
    }

    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, lbl_ledger.Text, "--Select--");
    }

    # endregion

    #region paymentmode

    public void loadpaid()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            chkl_paid.Items.Clear();
            txt_paid.Text = "--Select--";
            chk_paid.Checked = false;
            d2.BindPaymodeToCheckboxList(chkl_paid, usercode, collegecode);
            if (chkl_paid.Items.Count > 0)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    chkl_paid.Items[i].Selected = true;
                }
                txt_paid.Text = "Paid(" + chkl_paid.Items.Count + ")";
                chk_paid.Checked = true;
            }
        }
        catch
        {

        }

    }

    public void chk_paid_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }

    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }

    #endregion

    #region bank

    public void bindBank()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            cblbank.Items.Clear();
            txtbank.Text = "--Select--";
            cbbank.Checked = false;
            if (Convert.ToString(collegecode) != "")
            {
                string query = "  select distinct (coll_acronymn+'-'+BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster bk,collinfo c where bk.collegecode=c.college_code and bk.CollegeCode in('" + collegecode + "' )";
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblbank.DataSource = ds;
                    cblbank.DataTextField = "BankName";
                    cblbank.DataValueField = "BankPK";
                    cblbank.DataBind();
                    for (int i = 0; i < cblbank.Items.Count; i++)
                    {
                        cblbank.Items[i].Selected = true;
                    }
                    txtbank.Text = lblbank.Text + "(" + cblbank.Items.Count + ")";
                    cbbank.Checked = true;
                    bindOtherBank();
                }
            }
        }
        catch
        {
        }
    }

    public void cbbank_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbbank, cblbank, txtbank, lblbank.Text, "--Select--");
        bindOtherBank();
    }

    public void cblbank_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbbank, cblbank, txtbank, lblbank.Text, "--Select--");
        bindOtherBank();
    }

    #endregion

    #region bank

    public void bindOtherBank()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string otherBankPK = Convert.ToString(getCblSelectedValue(cblbank));
            cblobank.Items.Clear();
            txtobank.Text = "--Select--";
            cbobank.Checked = false;
            if (Convert.ToString(otherBankPK) != "")
            {
                string query = "  select distinct (coll_acronymn+'-'+BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster bk,collinfo c where bk.collegecode=c.college_code  and BankPK not in('" + otherBankPK + "')";//and bk.CollegeCode in('" + collegecode + "' )
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblobank.DataSource = ds;
                    cblobank.DataTextField = "BankName";
                    cblobank.DataValueField = "BankPK";
                    cblobank.DataBind();
                    for (int i = 0; i < cblobank.Items.Count; i++)
                    {
                        cblobank.Items[i].Selected = true;
                    }
                    txtobank.Text = lblbank.Text + "(" + cblobank.Items.Count + ")";
                    cbobank.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    public void cbobank_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbobank, cblobank, txtobank, Label1.Text, "--Select--");
    }

    public void cblobank_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbobank, cblobank, txtobank, Label1.Text, "--Select--");
    }

    #endregion

    #region financial year

    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            //string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate from FM_FinYearMaster where CollegeCode in('" + collegecode + "')";
            ds.Dispose();
            ds.Reset();
            chkfyear.Checked = false;
            chklsfyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    // string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));
                }
                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
                }
                if (chklsfyear.Items.Count == 1)
                    txtfyear.Text = "" + fnalyr + "";
                else
                    txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
                // txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
                tdlblfnl.Visible = true;
                tdfnl.Visible = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chklsfyear_selected(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");

    }

    protected void chkfyear_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
    }

    #endregion

    protected string getBankFk()
    {
        string bankFK = string.Empty;
        try
        {
            StringBuilder sbStr = new StringBuilder();
            if (cblbank.Items.Count > 0)
            {
                for (int bk = 0; bk < cblbank.Items.Count; bk++)
                {
                    if (!cblbank.Items[bk].Selected)
                        continue;
                    sbStr.Append(Convert.ToString(cblbank.Items[bk].Value) + "','");
                }
            }
            if (cblobank.Items.Count > 0)
            {
                for (int bk = 0; bk < cblobank.Items.Count; bk++)
                {
                    if (!cblobank.Items[bk].Selected)
                        continue;
                    sbStr.Append(Convert.ToString(cblobank.Items[bk].Value) + "','");
                }
            }
            if (sbStr.Length > 0)
            {
                sbStr.Remove(sbStr.Length - 3, 3);
                bankFK = Convert.ToString(sbStr);
            }
        }
        catch { }
        return bankFK;
    }

    protected DataSet loadDetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            UserbasedRights();
            string hdText = string.Empty;
            string payMode = string.Empty;
            string ldText = string.Empty;
            string bankFk = string.Empty;
            string otherBankFk = string.Empty;
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            hdText = Convert.ToString(getCblSelectedText(chkl_studhed));
            ldText = Convert.ToString(getCblSelectedText(chkl_studled));
            payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            // bankFk = Convert.ToString(getCblSelectedValue(cblbank));
            bankFk = getBankFk();
            //otherBankFk = Convert.ToString(getCblSelectedValue(cblobank));
            //otherBankFk = otherBankFk != "" ? otherBankFk = " and f.Deposite_BankFK in('" + otherBankFk + "')" : otherBankFk = "";

            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

            string strHdName = string.Empty;
            string strhdNameGroup = string.Empty;
            string strOrderBy = string.Empty;

            string sndstrHdName = string.Empty;
            string sndstrhdNameGroup = string.Empty;
            string sndstrOrderBy = string.Empty;
            string strInclude = string.Empty;
            string hdFK = getHeaderFK(hdText, collegecode);
            string ldFK = getLedgerFK(ldText, collegecode);

            if (rblmode.SelectedIndex == 0)
            {
                strHdName = " headerName as headerName";
                strhdNameGroup = " headerName,transdate";//Transdate added by saranya on 29/3/2018
                strOrderBy = " order by headerName";

                sndstrHdName = " headerName+'-'+'(excess)' as headerName";
                sndstrhdNameGroup = " headerName,excesstransdate";//excesstransdate added by saranya on 29/3/2018
                sndstrOrderBy = " order by headerName";
                if (cbDate.Checked)
                {
                    strHdName = " convert(varchar(10),transdate,103) as headerName";
                    strhdNameGroup = " transdate";
                    strOrderBy = " order by transdate";

                    sndstrHdName = " convert(varchar(10),excesstransdate,103)+'-'+'(excess)' as headerName";
                    sndstrhdNameGroup = " excesstransdate";
                    sndstrOrderBy = " order by excesstransdate";
                }

            }
            else
            {
                strHdName = " headerName,ledgerName";
                strhdNameGroup = " headerName,ledgerName,transdate";//Transdate added by saranya on 29/3/2018
                strOrderBy = " order by headerName";

                sndstrHdName = " headerName,ledgerName+'-'+'(excess)' as ledgerName";
                sndstrhdNameGroup = " headerName,ledgerName,excesstransdate";//excesstransdate added by saranya on 29/3/2018
                sndstrOrderBy = " order by headerName";
                if (cbDate.Checked)
                {
                    //strHdName = " headerName,DATEPART(month,TransDate) ,DATEPART(year,TransDate)";
                    strHdName = " headerName,convert(varchar(10),transdate,103) as ledgerName";
                    strhdNameGroup = " headerName,transdate";
                    strOrderBy = " order by headerName,transdate";

                    sndstrHdName = " headerName,convert(varchar(10),excesstransdate,103)+'-'+'(excess)' as ledgerName";
                    sndstrhdNameGroup = " headerName,excesstransdate";
                    sndstrOrderBy = " order by headerName,excesstransdate";
                }

            }

            strInclude = getStudCategory();

            //string strInclude = getStudCategory();
            #endregion

            if (!cbMonth.Checked)
            {
                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode))
                {
                    #region Query
                    string finlYrStr = string.Empty;
                    string selFinYr = string.Empty;
                    string selFinYrGrpBy = string.Empty;
                    string SelQ = string.Empty;
                    if (checkSchoolSetting() == 0)//school
                    {
                        #region
                        selFinYr = " ,(select convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)) from fm_finyearmaster where finyearpk=isnull(actualfinyearfk,'0'))as actualfinyearfk";
                        selFinYrGrpBy = " ,actualfinyearfk";
                        StringBuilder sbFinlYr = new StringBuilder();
                        Dictionary<string, string> htFinlYR = getFinancialYear();
                        if (chklsfyear.Items.Count > 0)
                        {
                            for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
                            {
                                if (!chklsfyear.Items[fnl].Selected)
                                    continue;
                                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                                {
                                    if (!cblclg.Items[clg].Selected)
                                        continue;
                                    string KeyVal = htFinlYR.Keys.FirstOrDefault(x => htFinlYR[x] == chklsfyear.Items[fnl].Text + "-" + cblclg.Items[clg].Value);//to pass value get key from dictionary 
                                    sbFinlYr.Append(KeyVal + "','");
                                }

                            }
                            if (sbFinlYr.Length > 0)
                                sbFinlYr.Remove(sbFinlYr.Length - 3, 3);
                        }
                        finlYrStr = " and f.finyearfk in('" + Convert.ToString(sbFinlYr) + "')";
                        // selCol = "f.paymode," + selectCol + ",f.app_no,isnull(f.transtype,'0') as transtype" + selFinYr + "";
                        //  GrpselCol = "f.paymode," + groupStr + ",f.app_no,f.transtype,actualfinyearfk";
                        #endregion
                    }
                    string incJournal = string.Empty;
                    if (cbJournal.Checked)
                        incJournal = " and isnull(f.transtype,'0')='3'";

                    if (rblMemType.SelectedIndex == 0)
                    {
                        if (!cbAcdYear.Checked)
                        {
                            #region student

                            SelQ = " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,isnull(paymode,'0') as paymode,isnull(f.transtype,'0') as transtype" + selFinYr + " ,convert(date,transdate,103) as transdate from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,registration r where f.app_no=r.app_no and h.headerpk=f.headerfk   and h.collegecode in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=l.collegecode    " + strInclude + " " + incJournal + " " + finlYrStr + " group by " + strhdNameGroup + ",paymode,f.transtype" + selFinYrGrpBy + " ";//and isnull(f.debit,'0')>0 and r.college_code=h.collegecode and r.college_code=l.collegecode
                            if (!cbJournal.Checked)
                            {
                                string selFinYrS = string.Empty;
                                string selFinYrGrpBys = string.Empty;
                                if (checkSchoolSetting() == 0)//school
                                {
                                    selFinYrS = ",convert(varchar(10),isnull(exl.finyearfk,'0')) as actualfinyearfk";
                                    selFinYrGrpBys = " ,exl.finyearfk";
                                }
                                SelQ += " union all select " + sndstrHdName + ",sum(isnull(exl.excessamt,'0')) as debit,'0' as credit,isnull(ex_paymode,'0') as paymode,'1'transtype" + selFinYrS + ",convert(date,excesstransdate,103)as transdate  from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1' and r.college_code in('" + collegecode + "') and exl.headerfk in('" + hdFK + "') and exl.ledgerfk in('" + ldFK + "') and excesstransdate between '" + fromdate + "' and '" + todate + "' group by ex_paymode," + sndstrhdNameGroup + " " + selFinYrGrpBys + " ";
                            }
                            SelQ += " order by transdate";

                            //only dd,check
                            SelQ += " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,isnull(paymode,'0') as paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype" + selFinYr + ",convert(date,transdate,103) as transdate from ft_findailytransaction f,fm_headermaster h ,FM_LedgerMaster l ,registration r where f.app_no=r.app_no  and h.headerpk=f.headerfk  and h.collegecode in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=l.collegecode and r.college_code=h.collegecode and r.college_code=l.collegecode  " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and isnull(deposite_bankfk,'0')<>'0' " + incJournal + "  " + finlYrStr + " group by " + strhdNameGroup + ",paymode,Deposite_BankFK,f.transtype" + selFinYrGrpBy + " ";//and isnull(f.debit,'0')>0//" + strOrderBy + "
                            if (!cbJournal.Checked)
                            {
                                string selFinYrS = string.Empty;
                                string selFinYrGrpBys = string.Empty;
                                if (checkSchoolSetting() == 0)//school
                                {
                                    selFinYrS = ",convert(varchar(10),isnull(exl.finyearfk,'0')) as actualfinyearfk";
                                    selFinYrGrpBys = " ,exl.finyearfk";
                                }
                                SelQ += " union all select " + sndstrHdName + ",sum(isnull(exl.excessamt,'0')) as debit,'0' as credit,isnull(ex_paymode,'0') as paymode,Ex_Deposite_BankFk ,'1'transtype" + selFinYrS + ",convert(date,excesstransdate,103)as transdate  from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1' and r.college_code in('" + collegecode + "')and exl.headerfk in('" + hdFK + "') and exl.ledgerfk in('" + ldFK + "') and excesstransdate between '" + fromdate + "' and '" + todate + "'  group by ex_paymode,Ex_Deposite_BankFk," + sndstrhdNameGroup + " " + selFinYrGrpBys + "  ";//and Ex_Deposite_BankFk in(2,3)
                            }
                            SelQ += " order by transdate";

                            //distinct bank name                     
                            SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,registration r,FM_FinBankMaster bk where f.app_no=r.app_no and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk   and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and  isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK and h.collegecode=l.collegecode and r.college_code=h.collegecode and r.college_code=l.collegecode  " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and isnull(deposite_bankfk,'0')<>'0' " + finlYrStr + " " + incJournal + " ";//f.transdate between '" + fromdate + "' and '" + todate + "' and
                            #endregion
                        }
                        else
                        {
                            #region student

                            SelQ = " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,isnull(paymode,'0') as paymode,isnull(f.transtype,'0') as transtype" + selFinYr + ",r.batch_year,f.feecategory,r.college_code,convert(date,transdate,103) as transdate from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,registration r where f.app_no=r.app_no and h.headerpk=f.headerfk   and h.collegecode in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=l.collegecode and r.college_code=h.collegecode and r.college_code=l.collegecode  " + strInclude + " " + incJournal + " " + finlYrStr + " group by r.batch_year,f.feecategory,r.college_code," + strhdNameGroup + ",paymode,f.transtype" + selFinYrGrpBy + " ";//and isnull(f.debit,'0')>0
                            if (!cbJournal.Checked)
                            {
                                string selFinYrS = string.Empty;
                                string selFinYrGrpBys = string.Empty;
                                if (checkSchoolSetting() == 0)//school
                                {
                                    selFinYrS = ",convert(varchar(10),isnull(exl.finyearfk,'0')) as actualfinyearfk";
                                    selFinYrGrpBys = " ,exl.finyearfk";
                                }
                                SelQ += " union all select " + sndstrHdName + ",sum(isnull(exl.excessamt,'0')) as debit,'0' as credit,isnull(ex_paymode,'0') as paymode,'1'transtype" + selFinYrS + ",r.batch_year,ex.feecategory,r.college_code,convert(date,excesstransdate,103)as transdate  from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1' and r.college_code in('" + collegecode + "') and exl.headerfk in('" + hdFK + "') and exl.ledgerfk in('" + ldFK + "') and excesstransdate between '" + fromdate + "' and '" + todate + "' group by ex_paymode,r.batch_year,ex.feecategory,r.college_code," + sndstrhdNameGroup + " " + selFinYrGrpBys + " ";
                            }
                            SelQ += " order by transdate";
                            //  SelQ += " " + strOrderBy + "";

                            //only dd,check
                            SelQ += " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,isnull(paymode,'0') as paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype" + selFinYr + ",r.batch_year,f.feecategory,r.college_code,convert(date,transdate,103) as transdate  from ft_findailytransaction f,fm_headermaster h ,FM_LedgerMaster l ,registration r where f.app_no=r.app_no  and h.headerpk=f.headerfk  and h.collegecode in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=l.collegecode and r.college_code=h.collegecode and r.college_code=l.collegecode  " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and isnull(deposite_bankfk,'0')<>'0' " + incJournal + "  " + finlYrStr + " group by r.batch_year,f.feecategory,r.college_code, " + strhdNameGroup + ",paymode,Deposite_BankFK,f.transtype" + selFinYrGrpBy + " ";//and isnull(f.debit,'0')>0//" + strOrderBy + "
                            if (!cbJournal.Checked)
                            {
                                string selFinYrS = string.Empty;
                                string selFinYrGrpBys = string.Empty;
                                if (checkSchoolSetting() == 0)//school
                                {
                                    selFinYrS = ",convert(varchar(10),isnull(exl.finyearfk,'0')) as actualfinyearfk";
                                    selFinYrGrpBys = " ,exl.finyearfk";
                                }
                                SelQ += " union all select " + sndstrHdName + ",sum(isnull(exl.excessamt,'0')) as debit,'0' as credit,isnull(ex_paymode,'0') as paymode,Ex_Deposite_BankFk,'1'transtype" + selFinYrS + ",r.batch_year,ex.feecategory,r.college_code,convert(date,excesstransdate,103)as transdate  from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1' and r.college_code in('" + collegecode + "') and exl.headerfk in('" + hdFK + "') and exl.ledgerfk in('" + ldFK + "') and excesstransdate between '" + fromdate + "' and '" + todate + "' group by ex_paymode,r.batch_year,ex.feecategory,r.college_code,Ex_Deposite_BankFk," + sndstrhdNameGroup + " " + selFinYrGrpBys + "";
                            }
                            SelQ += " order by transdate";

                            //distinct bank name                     
                            SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,registration r,FM_FinBankMaster bk where f.app_no=r.app_no and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk   and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK and h.collegecode=l.collegecode and r.college_code=h.collegecode and r.college_code=l.collegecode  " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and isnull(deposite_bankfk,'0')<>'0' " + finlYrStr + " " + incJournal + " ";
                            #endregion
                        }
                        if (cbIncOthers.Checked)
                        {
                            if (checkSchoolSetting() == 0)//school
                            {
                                selFinYr = " ,(select convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)) from fm_finyearmaster where finyearpk=isnull(actualfinyearfk,'0'))as actualfinyearfk";
                                selFinYrGrpBy = " ,actualfinyearfk";
                            }
                            #region staff,vendor,other
                            //if ((totSelcount != 1 && selectedName.Contains("Staff")) || (totSelcount == 1 && memName == "Staff"))
                            //  {
                            #region staff
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,isnull(f.transtype,'0') as transtype" + selFinYr + ",convert(date,transdate,103) as transdate from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK  and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2'  ";
                            // if (string.IsNullOrEmpty(strMemtypeValue)) //and sm.college_code in('" + collegecode + "')
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,f.transtype" + selFinYrGrpBy + " " + strOrderBy + " ";
                            // SelQ += "  order by Transcode";
                            //only dd,cheque
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype" + selFinYr + ",convert(date,transdate,103) as transdate from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK  and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2'  ";//and sm.college_code in('" + collegecode + "')
                            //  if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,Deposite_BankFK,f.transtype" + selFinYrGrpBy + "" + strOrderBy + "";

                            //distinct bank name                     
                            SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,staffmaster sm,staff_appl_master sa,stafftrans st,FM_FinBankMaster bk where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and sm.college_code in('" + collegecode + "') and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk   and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK   and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and isnull(deposite_bankfk,'0')<>'0' " + finlYrStr + " " + incJournal + " ";//" + strInclude + "

                            #endregion
                            //}
                            //if ((totSelcount != 1 && selectedName.Contains("Vendor")) || (totSelcount == 1 && memName == "Vendor"))
                            //{
                            #region Vendor
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,isnull(f.transtype,'0') as transtype" + selFinYr + ",convert(date,transdate,103) as transdate from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f ,fm_headermaster h,FM_LedgerMaster l where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3'  ";
                            //if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,f.transtype" + selFinYrGrpBy + " " + strOrderBy + "";
                            // SelQ += "  order by Transcode";

                            //only dd,cheque 
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype" + selFinYr + ",convert(date,transdate,103) as transdate from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f ,fm_headermaster h,FM_LedgerMaster l where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3' ";
                            // if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,Deposite_BankFK,f.transtype" + selFinYrGrpBy + "" + strOrderBy + "";

                            //distinct bank name                     
                            SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,CO_VendorMaster vm,IM_VendorContactMaster vc,FM_FinBankMaster bk where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk   and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK   and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and isnull(deposite_bankfk,'0')<>'0' " + finlYrStr + " ";//" + strInclude + "
                            #endregion
                            //}
                            //if ((totSelcount != 1 && selectedName.Contains("Others")) || (totSelcount == 1 && memName == "Others"))
                            //{
                            #region other
                            //other details
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,isnull(f.transtype,'0') as transtype" + selFinYr + ",convert(date,transdate,103) as transdate from CO_VendorMaster vm,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4'  ";
                            //  if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,f.transtype" + selFinYrGrpBy + " " + strOrderBy + "";
                            // SelQ += "  order by Transcode";
                            //only dd,cheque
                            SelQ += " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype" + selFinYr + ",convert(date,transdate,103) as transdate from CO_VendorMaster vm,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4'  ";
                            // if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,Deposite_BankFK,f.transtype" + selFinYrGrpBy + "" + strOrderBy + "";
                            //distinct bank name                     
                            SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,CO_VendorMaster vm,FM_FinBankMaster bk where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk   and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK   and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and memtype='4' and isnull(deposite_bankfk,'0')<>'0' " + finlYrStr + "";//" + strInclude + "
                            #endregion
                            //}
                            #endregion
                        }
                    }
                    else
                    {
                        #region others

                        string strMemtypeValue = string.Empty;
                        string selectedName = getSelMemName(cblmem);
                        int totSelcount = 0;
                        string memName = getSelectedMemName(cblmem, ref   totSelcount);
                        if (lblval.Text.Trim() != "")// if lookup selected any staff,vendor and other
                        {

                            if (totSelcount == 1 && memName == "Staff")
                                strMemtypeValue = " and appl_id in('" + lblval.Text.Trim() + "')";
                            else if (totSelcount == 1 && memName == "Vendor")
                                strMemtypeValue = " and VendorContactPK in('" + lblval.Text.Trim() + "')";
                            else if (totSelcount == 1 && memName == "Others")
                                strMemtypeValue = " and vendorpk in('" + lblval.Text.Trim() + "')";

                        }
                        if ((totSelcount != 1 && selectedName.Contains("Staff")) || (totSelcount == 1 && memName == "Staff"))
                        {
                            #region staff
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,isnull(f.transtype,'0') as transtype,convert(date,transdate,103) as transdate from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and sm.college_code in('" + collegecode + "') and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2' " + strMemtypeValue + " ";
                            if (string.IsNullOrEmpty(strMemtypeValue))
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,f.transtype " + strOrderBy + "";
                            // SelQ += "  order by Transcode";
                            //only dd,cheque
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype,convert(date,transdate,103) as transdate from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and sm.college_code in('" + collegecode + "') and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2' " + strMemtypeValue + " ";
                            if (string.IsNullOrEmpty(strMemtypeValue))
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,Deposite_BankFK,f.transtype" + strOrderBy + "";

                            //distinct bank name                     
                            SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,staffmaster sm,staff_appl_master sa,stafftrans st,FM_FinBankMaster bk where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and sm.college_code in('" + collegecode + "') and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk   and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK and  l.ledgername in('" + ldText + "') " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and isnull(deposite_bankfk,'0')<>'0' " + finlYrStr + " " + incJournal + " ";

                            #endregion
                        }
                        if ((totSelcount != 1 && selectedName.Contains("Vendor")) || (totSelcount == 1 && memName == "Vendor"))
                        {
                            #region Vendor
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,isnull(f.transtype,'0') as transtype,convert(date,transdate,103) as transdate from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f ,fm_headermaster h,FM_LedgerMaster l where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3' " + strMemtypeValue + " ";
                            if (string.IsNullOrEmpty(strMemtypeValue))
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,f.transtype " + strOrderBy + "";
                            // SelQ += "  order by Transcode";

                            //only dd,cheque 
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype,convert(date,transdate,103) as transdate from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f ,fm_headermaster h,FM_LedgerMaster l where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3' " + strMemtypeValue + " ";
                            if (string.IsNullOrEmpty(strMemtypeValue))
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,Deposite_BankFK,f.transtype" + strOrderBy + "";

                            //distinct bank name                     
                            SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,CO_VendorMaster vm,IM_VendorContactMaster vc,FM_FinBankMaster bk where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk   and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK and  l.ledgername in('" + ldText + "') " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and isnull(deposite_bankfk,'0')<>'0' " + finlYrStr + " ";
                            #endregion
                        }
                        if ((totSelcount != 1 && selectedName.Contains("Others")) || (totSelcount == 1 && memName == "Others"))
                        {
                            #region other
                            //other details
                            SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,isnull(f.transtype,'0') as transtype,convert(date,transdate,103) as transdate from CO_VendorMaster vm,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4' " + strMemtypeValue + " ";
                            if (string.IsNullOrEmpty(strMemtypeValue))
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,f.transtype " + strOrderBy + "";
                            // SelQ += "  order by Transcode";
                            //only dd,cheque
                            SelQ += " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype,convert(date,transdate,103) as transdate from CO_VendorMaster vm,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4' " + strMemtypeValue + " ";
                            if (string.IsNullOrEmpty(strMemtypeValue))
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                            SelQ += " group by " + strhdNameGroup + ",f.paymode,Deposite_BankFK,f.transtype" + strOrderBy + "";
                            //distinct bank name                     
                            SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,CO_VendorMaster vm,FM_FinBankMaster bk where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk   and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK and  l.ledgername in('" + ldText + "') " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') " + otherBankFk + " and memtype='4' and isnull(deposite_bankfk,'0')<>'0' " + finlYrStr + "";
                            #endregion
                        }
                        #endregion
                    }
                    dsload.Clear();

                    dsload = d2.select_method_wo_parameter(SelQ, "Text");
                    #endregion
                }
            }

            # region Added by saranya on 13/3/2018 for monthwise abstract report
            if (cbMonth.Checked)
            {
                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode))
                {
                    #region Query
                    string finlYrStr = string.Empty;
                    string selFinYr = string.Empty;
                    string selFinYrGrpBy = string.Empty;
                    string SelQ = string.Empty;
                    if (checkSchoolSetting() == 0)//school
                    {
                        #region
                        selFinYr = " ,(select convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)) from fm_finyearmaster where finyearpk=isnull(actualfinyearfk,'0'))as actualfinyearfk";
                        selFinYrGrpBy = " ,actualfinyearfk";
                        StringBuilder sbFinlYr = new StringBuilder();
                        Dictionary<string, string> htFinlYR = getFinancialYear();
                        if (chklsfyear.Items.Count > 0)
                        {
                            for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
                            {
                                if (!chklsfyear.Items[fnl].Selected)
                                    continue;
                                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                                {
                                    if (!cblclg.Items[clg].Selected)
                                        continue;
                                    string KeyVal = htFinlYR.Keys.FirstOrDefault(x => htFinlYR[x] == chklsfyear.Items[fnl].Text + "-" + cblclg.Items[clg].Value);//to pass value get key from dictionary 
                                    sbFinlYr.Append(KeyVal + "','");
                                }

                            }
                            if (sbFinlYr.Length > 0)
                                sbFinlYr.Remove(sbFinlYr.Length - 3, 3);
                        }
                        finlYrStr = " and f.finyearfk in('" + Convert.ToString(sbFinlYr) + "')";
                        // selCol = "f.paymode," + selectCol + ",f.app_no,isnull(f.transtype,'0') as transtype" + selFinYr + "";
                        //  GrpselCol = "f.paymode," + groupStr + ",f.app_no,f.transtype,actualfinyearfk";
                        #endregion
                    }
                    string incJournal = string.Empty;
                    if (cbJournal.Checked)
                        incJournal = " and isnull(f.transtype,'0')='3'";
                    if (!cbJournal.Checked)
                        incJournal = " and f.transtype<>3";

                    if (rblMemType.SelectedIndex == 0)
                    {
                        if (!cbAcdYear.Checked)
                        {
                            #region student
                            if (rblmode.SelectedIndex == 0)
                            {
                                SelQ = " select  DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,headerName from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,Registration r where  h.headerpk=f.headerfk and r.App_No=f.app_no and r.college_code=h.collegecode and r.college_code=l.collegecode  and h.collegecode in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=l.collegecode  " + strInclude + " " + incJournal + " " + finlYrStr + " group by  DATEPART(month,TransDate),DATEPART(year,TransDate),headerName  order by DATEPART(year,TransDate), DATEPART(month,TransDate)";//and isnull(f.debit,'0')>0

                                if (!cbJournal.Checked)
                                {
                                    string selFinYrS = string.Empty;
                                    string selFinYrGrpBys = string.Empty;
                                    if (checkSchoolSetting() == 0)//school
                                    {
                                        selFinYrS = ",convert(varchar(10),isnull(exl.finyearfk,'0')) as actualfinyearfk";
                                        selFinYrGrpBys = " ,exl.finyearfk";
                                    }
                                    SelQ += " select DATEPART(month,excesstransdate) as month ,DATEPART(year,excesstransdate) as year,sum(isnull(exl.excessamt,'0')) as debit,'0' as credit from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1' and r.college_code in('" + collegecode + "') and exl.headerfk in('" + hdFK + "') and exl.BalanceAmt > 0 and exl.ledgerfk in('" + ldFK + "') and excesstransdate between '" + fromdate + "' and '" + todate + "' group by DATEPART(month,excesstransdate),DATEPART(year,excesstransdate)  order by DATEPART(year,excesstransdate),DATEPART(month,excesstransdate) ";
                                }
                            }
                            if (rblmode.SelectedIndex == 1)
                            {
                                SelQ = " select  DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,ledgerName from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,Registration r where  h.headerpk=f.headerfk and r.App_No=f.app_no  and h.collegecode in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and r.college_code=h.collegecode and r.college_code=l.collegecode and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=l.collegecode  " + strInclude + " " + incJournal + " " + finlYrStr + " group by  DATEPART(month,TransDate),DATEPART(year,TransDate),ledgerName order by DATEPART(year,TransDate), DATEPART(month,TransDate)";//and isnull(f.debit,'0')>0
                                if (!cbJournal.Checked)
                                {
                                    string selFinYrS = string.Empty;
                                    string selFinYrGrpBys = string.Empty;
                                    if (checkSchoolSetting() == 0)//school
                                    {
                                        selFinYrS = ",convert(varchar(10),isnull(exl.finyearfk,'0')) as actualfinyearfk";
                                        selFinYrGrpBys = " ,exl.finyearfk";
                                    }
                                    SelQ += " select DATEPART(month,excesstransdate) as month ,DATEPART(year,excesstransdate) as year,sum(isnull(exl.excessamt,'0')) as debit,'0' as credit from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1' and r.college_code in('" + collegecode + "') and exl.headerfk in('" + hdFK + "') and exl.BalanceAmt > 0 and exl.ledgerfk in('" + ldFK + "') and excesstransdate between '" + fromdate + "' and '" + todate + "' group by DATEPART(month,excesstransdate),DATEPART(year,excesstransdate)  order by DATEPART(year,excesstransdate),DATEPART(month,excesstransdate) ";
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region student

                            if (rblmode.SelectedIndex == 0)
                            {
                                SelQ = " select DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,r.batch_year,f.feecategory,r.college_code,headerName from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,Registration r where h.headerpk=f.headerfk   and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and f.app_no=r.app_no and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=l.collegecode  " + strInclude + " " + incJournal + " " + finlYrStr + " group by r.batch_year,f.feecategory,r.college_code,DATEPART(month,TransDate),DATEPART(year,TransDate),headerName order by DATEPART(year,TransDate), DATEPART(month,TransDate)";//and isnull(f.debit,'0')>0
                                if (!cbJournal.Checked)
                                {
                                    string selFinYrS = string.Empty;
                                    string selFinYrGrpBys = string.Empty;
                                    if (checkSchoolSetting() == 0)//school
                                    {
                                        selFinYrS = ",convert(varchar(10),isnull(exl.finyearfk,'0')) as actualfinyearfk";
                                        selFinYrGrpBys = " ,exl.finyearfk";
                                    }
                                    SelQ += " select DATEPART(month,excesstransdate) as month ,DATEPART(year,excesstransdate) as year,sum(isnull(exl.excessamt,'0')) as debit,'0' as credit from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1' and r.college_code in('" + collegecode + "') and exl.headerfk in('" + hdFK + "') and exl.BalanceAmt > 0 and exl.ledgerfk in('" + ldFK + "') and excesstransdate between '" + fromdate + "' and '" + todate + "' group by DATEPART(month,excesstransdate),DATEPART(year,excesstransdate) order by DATEPART(year,excesstransdate),DATEPART(month,excesstransdate) ";
                                }
                            }
                            #endregion

                            if (rblmode.SelectedIndex == 1)
                            {
                                SelQ = " select DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit ,r.batch_year,f.feecategory,r.college_code,ledgerName from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,Registration r where h.headerpk=f.headerfk   and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and f.app_no=r.app_no and r.college_code=h.collegecode and r.college_code=l.collegecode and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=l.collegecode  " + strInclude + " " + incJournal + " " + finlYrStr + " group by r.batch_year,f.feecategory,r.college_code,DATEPART(month,TransDate),DATEPART(year,TransDate),ledgerName order by  DATEPART(year,TransDate), DATEPART(month,TransDate)";//and isnull(f.debit,'0')>0


                                if (!cbJournal.Checked)
                                {
                                    string selFinYrS = string.Empty;
                                    string selFinYrGrpBys = string.Empty;
                                    if (checkSchoolSetting() == 0)//school
                                    {
                                        selFinYrS = ",convert(varchar(10),isnull(exl.finyearfk,'0')) as actualfinyearfk";
                                        selFinYrGrpBys = " ,exl.finyearfk";
                                    }
                                    SelQ += " select DATEPART(month,excesstransdate) as month ,DATEPART(year,excesstransdate) as year,sum(isnull(exl.excessamt,'0')) as debit,'0' as credit from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1' and r.college_code in('" + collegecode + "') and exl.headerfk in('" + hdFK + "') and exl.BalanceAmt > 0 and exl.ledgerfk in('" + ldFK + "') and excesstransdate between '" + fromdate + "' and '" + todate + "' group by DATEPART(month,excesstransdate),DATEPART(year,excesstransdate) order by DATEPART(year,excesstransdate),DATEPART(month,excesstransdate) ";
                                }
                            }
                        }
                        SelQ = SelQ + " select distinct DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where  h.headerpk=f.headerfk   and h.collegecode in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=l.collegecode  " + strInclude + " " + incJournal + " " + finlYrStr + " group by  DATEPART(month,TransDate),DATEPART(year,TransDate) order by DATEPART(year,TransDate), DATEPART(month,TransDate)";
                        if (cbIncOthers.Checked)
                        {
                            if (checkSchoolSetting() == 0)//school
                            {
                                selFinYr = " ,(select convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)) from fm_finyearmaster where finyearpk=isnull(actualfinyearfk,'0'))as actualfinyearfk";
                                selFinYrGrpBy = " ,actualfinyearfk";
                            }

                            #region staff
                            if (rblmode.SelectedIndex == 0)
                            {
                                SelQ += " select DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,headerName  from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and  sm.college_code in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2'  ";
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                                SelQ += " group by DATEPART(month,TransDate),DATEPART(year,TransDate),headerName order by DATEPART(year,TransDate), DATEPART(month,TransDate)";
                            }
                            if (rblmode.SelectedIndex == 1)
                            {
                                SelQ += " select DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,ledgerName from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and  sm.college_code in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2'  ";
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                                SelQ += " group by DATEPART(month,TransDate),DATEPART(year,TransDate),ledgerName order by DATEPART(year,TransDate), DATEPART(month,TransDate)";
                            }
                            #endregion

                            #region Vendor
                            if (rblmode.SelectedIndex == 0)
                            {
                                SelQ += " select  DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,headerName from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f ,fm_headermaster h,FM_LedgerMaster l where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk and h.collegecode in('" + collegecode + "')  and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3'  ";
                                //if (string.IsNullOrEmpty(strMemtypeValue))
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                                SelQ += " group by DATEPART(month,TransDate),DATEPART(year,TransDate),headerName order by DATEPART(year,TransDate), DATEPART(month,TransDate)";
                            }
                            if (rblmode.SelectedIndex == 1)
                            {
                                SelQ += " select  DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,ledgerName from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f ,fm_headermaster h,FM_LedgerMaster l where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and h.collegecode in('" + collegecode + "')  and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3'  ";
                                //if (string.IsNullOrEmpty(strMemtypeValue))
                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                                SelQ += " group by DATEPART(month,TransDate),DATEPART(year,TransDate),ledgerName order by DATEPART(year,TransDate), DATEPART(month,TransDate)";
                            }
                            #endregion

                            #region other

                            if (rblmode.SelectedIndex == 0)
                            {
                                SelQ += " select  DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,headerName from CO_VendorMaster vm,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4'  ";

                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                                SelQ += " group by DATEPART(month,TransDate),DATEPART(year,TransDate),headerName order by DATEPART(year,TransDate), DATEPART(month,TransDate)";
                            }
                            if (rblmode.SelectedIndex == 1)
                            {
                                SelQ += " select  DATEPART(month,TransDate) as month ,DATEPART(year,TransDate) as year,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,ledgerName from CO_VendorMaster vm,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4'  ";

                                SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                                SelQ += " group by DATEPART(month,TransDate),DATEPART(year,TransDate),ledgerName order by DATEPART(year,TransDate), DATEPART(month,TransDate)";
                            }
                            #endregion
                        }


                    }

                    dsload.Clear();

                    dsload = d2.select_method_wo_parameter(SelQ, "Text");

                    #endregion
                }
            }
            #endregion
        }
        catch { }
        return dsload;
    }

    protected Hashtable getHeaderFK(ref Hashtable hdName)
    {
        Hashtable hthdName = new Hashtable();
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string selQFK = string.Empty;
            if (rblmode.SelectedIndex == 0)
                selQFK = "  select distinct headerpk as pk,headername as name from fm_headermaster where collegecode in('" + collegecode + "') ";
            else
                selQFK = "  select distinct ledgerpk as pk,ledgername as name from fm_ledgermaster where collegecode in('" + collegecode + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
            if (rblmode.SelectedIndex == 1)
            {
                selQFK = "  select distinct headerpk as pk,headername as name from fm_headermaster where collegecode in('" + collegecode + "') ";
                DataSet dsHd = d2.select_method_wo_parameter(selQFK, "Text");
                if (dsHd.Tables.Count > 0 && dsHd.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsHd.Tables[0].Rows.Count; row++)
                    {
                        if (!hdName.ContainsKey(Convert.ToString(dsHd.Tables[0].Rows[row]["pk"])))
                            hdName.Add(Convert.ToString(dsHd.Tables[0].Rows[row]["pk"]), Convert.ToString(dsHd.Tables[0].Rows[row]["name"]));
                    }
                }
            }
        }
        catch { hthdName.Clear(); }
        return hthdName;
    }

    //college spread load 

    protected DataTable loadPaidDetailsLedger(DataSet dspaid, ref Hashtable htpayMode)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            ArrayList arTranstype = new ArrayList();
            arTranstype.Add("1");
            arTranstype.Add("2");
            arTranstype.Add("3");
            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("Header_Name");
            dtpaid.Columns.Add("Credit");
            dtpaid.Columns.Add("Debit");
            ArrayList arMemType = getMemType();
            int tblCount = 0;
            int tblFirst = 0;
            int tblSecond = 0;
            int tblThird = 0;
            if (dtpaid.Columns.Count > 0)
            {
                DataRow drpaid;
                int rowCnt = 0;
                tblFirst = 0;
                tblSecond = 1;
                tblThird = 2;
                foreach (string memType in arMemType)
                {
                    Hashtable htSubTot = new Hashtable();
                    Hashtable htpaymode = new Hashtable();
                    string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                    for (int ar = 0; ar < arTranstype.Count; ar++)
                    {
                        #region
                        int TransTypeVal = 0;
                        int.TryParse(Convert.ToString(arTranstype[ar]), out TransTypeVal);
                        for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                        {
                            double tempCredit = 0;
                            double tempDebit = 0;
                            if (chkl_paid.Items[mode].Selected)
                            {
                                string payMode = Convert.ToString(chkl_paid.Items[mode].Value);
                                string payModeText = Convert.ToString(chkl_paid.Items[mode].Text);
                                if (!htpayMode.ContainsKey(chkl_paid.Items[mode].Value))
                                    htpayMode.Add(chkl_paid.Items[mode].Value, chkl_paid.Items[mode].Text);
                                if (payMode.Trim() != "2" && payMode.Trim() != "3")
                                {
                                    #region cash,online,card
                                    dspaid.Tables[tblFirst].DefaultView.RowFilter = "paymode='" + payMode + "' and Transtype='" + arTranstype[ar] + "'";
                                    DataView dvpaid = dspaid.Tables[tblFirst].DefaultView;
                                    dvpaid.Sort = "transdate";//Added by saranya on 28/03/2018
                                    if (dvpaid.Count > 0)
                                    {
                                        string dispText = string.Empty;
                                        string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                        if (transText != "")
                                            dispText = payModeText + "-(" + transText + ")";
                                        else
                                            dispText = payModeText;
                                        drpaid = dtpaid.NewRow();
                                        drpaid["Sno"] = strMemType + "~" + dispText + "#" + "Mode";
                                        dtpaid.Rows.Add(drpaid);
                                        for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                        {
                                            double hdCredit = 0;
                                            double hdDebit = 0;
                                            if (chkl_studhed.Items[hd].Selected)
                                            {
                                                string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                                DataTable dtbal = dvpaid.ToTable();
                                                dtbal.DefaultView.RowFilter = "headerName='" + hdName + "'";
                                                DataView dvpaids = dtbal.DefaultView;
                                                if (dvpaids.Count > 0)
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    drpaid["Sno"] = hdName + "!" + "Header";
                                                    dtpaid.Rows.Add(drpaid);
                                                    #region
                                                    for (int row = 0; row < dvpaids.Count; row++)
                                                    {
                                                        drpaid = dtpaid.NewRow();
                                                        double credit = 0;
                                                        double debit = 0;
                                                        drpaid["Sno"] = Convert.ToString(rowCnt++);
                                                        drpaid["Header_Name"] = Convert.ToString(dvpaids[row]["Ledgername"]);
                                                        double.TryParse(Convert.ToString(dvpaids[row]["debit"]), out credit);
                                                        double.TryParse(Convert.ToString(dvpaids[row]["credit"]), out debit);
                                                        drpaid["Credit"] = Convert.ToString(credit);
                                                        drpaid["Debit"] = Convert.ToString(debit);
                                                        tempCredit += credit;
                                                        tempDebit += debit;
                                                        hdCredit += credit;
                                                        hdDebit += debit;

                                                        if (TransTypeVal != 3)
                                                        {
                                                            if (!htSubTot.ContainsKey("Credit"))
                                                                htSubTot.Add("Credit", credit);
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                                amount += credit;
                                                                htSubTot.Remove("Credit");
                                                                htSubTot.Add("Credit", Convert.ToString(amount));
                                                            }

                                                            if (!htSubTot.ContainsKey("Debit"))
                                                                htSubTot.Add("Debit", debit);
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                                amount += debit;
                                                                htSubTot.Remove("Debit");
                                                                htSubTot.Add("Debit", Convert.ToString(amount));
                                                            }
                                                            //paymode
                                                            if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                                htpaymode.Add(payModeText + "-" + "CR", credit);
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                                amount += credit;
                                                                htpaymode.Remove(payModeText + "-" + "CR");
                                                                htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                            }
                                                            if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                                htpaymode.Add(payModeText + "-" + "DR", debit);
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                                amount += debit;
                                                                htpaymode.Remove(payModeText + "-" + "DR");
                                                                htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                            }
                                                        }

                                                        dtpaid.Rows.Add(drpaid);
                                                    }
                                                    #endregion
                                                }
                                            }
                                            if (hdCredit != 0 || hdDebit != 0)//every header total
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = "Header Total" + "-" + "*";
                                                drpaid["Credit"] = Convert.ToString(hdCredit);
                                                drpaid["Debit"] = Convert.ToString(hdDebit);
                                                dtpaid.Rows.Add(drpaid);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region dd,cheque
                                    bool boolMode = false;
                                    for (int bkrow = 0; bkrow < dspaid.Tables[tblThird].Rows.Count; bkrow++)
                                    {
                                        bool boolBank = false;
                                        dspaid.Tables[tblSecond].DefaultView.RowFilter = "paymode='" + payMode + "' and Deposite_BankFK='" + dspaid.Tables[tblThird].Rows[bkrow]["textcode"] + "' and Transtype='" + arTranstype[ar] + "'";
                                        DataView dvpaid = dspaid.Tables[tblSecond].DefaultView;
                                        dvpaid.Sort = "transdate";//Added by saranya on 28/03/2018
                                        if (dvpaid.Count > 0)
                                        {
                                            if (!boolMode)
                                            {
                                                string dispText = string.Empty;
                                                string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                                if (transText != "")
                                                    dispText = payModeText + "-(" + transText + ")";
                                                else
                                                    dispText = payModeText;
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = strMemType + "~" + dispText + "#" + "Mode";
                                                dtpaid.Rows.Add(drpaid);
                                                boolMode = true;
                                            }
                                            if (!boolBank)
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = Convert.ToString(dspaid.Tables[2].Rows[bkrow]["textval"]) + "$" + "BankName";
                                                dtpaid.Rows.Add(drpaid);
                                                boolBank = true;
                                            }
                                            double indivBankCredit = 0;
                                            double indivBankDebit = 0;
                                            for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                            {
                                                double hdCredit = 0;
                                                double hdDebit = 0;
                                                if (chkl_studhed.Items[hd].Selected)
                                                {
                                                    string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                                    DataTable dtbal = dvpaid.ToTable();
                                                    dtbal.DefaultView.RowFilter = "headerName='" + hdName + "'";
                                                    DataView dvpaids = dtbal.DefaultView;
                                                    if (dvpaids.Count > 0)
                                                    {
                                                        drpaid = dtpaid.NewRow();
                                                        drpaid["Sno"] = hdName + "!" + "Header";
                                                        dtpaid.Rows.Add(drpaid);
                                                        #region
                                                        for (int row = 0; row < dvpaids.Count; row++)
                                                        {
                                                            drpaid = dtpaid.NewRow();
                                                            double credit = 0;
                                                            double debit = 0;
                                                            drpaid["Sno"] = Convert.ToString(rowCnt++);
                                                            drpaid["Header_Name"] = Convert.ToString(dvpaids[row]["Ledgername"]);
                                                            double.TryParse(Convert.ToString(dvpaids[row]["debit"]), out credit);
                                                            double.TryParse(Convert.ToString(dvpaids[row]["credit"]), out debit);
                                                            drpaid["Credit"] = Convert.ToString(credit);
                                                            drpaid["Debit"] = Convert.ToString(debit);
                                                            tempCredit += credit;
                                                            tempDebit += debit;
                                                            hdCredit += credit;
                                                            hdDebit += debit;
                                                            indivBankCredit += credit;
                                                            indivBankDebit += debit;
                                                            if (TransTypeVal != 3)
                                                            {
                                                                if (!htSubTot.ContainsKey("Credit"))
                                                                    htSubTot.Add("Credit", credit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                                    amount += credit;
                                                                    htSubTot.Remove("Credit");
                                                                    htSubTot.Add("Credit", Convert.ToString(amount));
                                                                }

                                                                if (!htSubTot.ContainsKey("Debit"))
                                                                    htSubTot.Add("Debit", debit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                                    amount += debit;
                                                                    htSubTot.Remove("Debit");
                                                                    htSubTot.Add("Debit", Convert.ToString(amount));
                                                                }
                                                                //paymode
                                                                if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                                    htpaymode.Add(payModeText + "-" + "CR", credit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                                    amount += credit;
                                                                    htpaymode.Remove(payModeText + "-" + "CR");
                                                                    htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                                }
                                                                if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                                    htpaymode.Add(payModeText + "-" + "DR", debit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                                    amount += debit;
                                                                    htpaymode.Remove(payModeText + "-" + "DR");
                                                                    htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                                }
                                                            }

                                                            dtpaid.Rows.Add(drpaid);
                                                        }
                                                        #endregion
                                                    }
                                                }
                                                if (hdCredit != 0 || hdDebit != 0)//every header total
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    drpaid["Sno"] = "Header Total" + "-" + "*";
                                                    drpaid["Credit"] = Convert.ToString(hdCredit);
                                                    drpaid["Debit"] = Convert.ToString(hdDebit);
                                                    dtpaid.Rows.Add(drpaid);
                                                }
                                            }
                                            if (indivBankCredit != 0 || indivBankDebit != 0)//total
                                            {
                                                drpaid = dtpaid.NewRow();
                                                if (TransTypeVal != 3)
                                                    drpaid["Sno"] = "Total" + "-" + "*";
                                                else
                                                    drpaid["Sno"] = "Journal Total" + "-" + "*";
                                                drpaid["Credit"] = Convert.ToString(indivBankCredit);
                                                drpaid["Debit"] = Convert.ToString(indivBankDebit);
                                                dtpaid.Rows.Add(drpaid);
                                            }

                                        }
                                    }
                                    #endregion
                                }
                            }
                            if (tempCredit != 0 || tempDebit != 0)//total
                            {
                                drpaid = dtpaid.NewRow();
                                if (TransTypeVal != 3)
                                    drpaid["Sno"] = "Total" + "-" + "*";
                                else
                                    drpaid["Sno"] = "Journal Total" + "-" + "*";
                                drpaid["Credit"] = Convert.ToString(tempCredit);
                                drpaid["Debit"] = Convert.ToString(tempDebit);
                                dtpaid.Rows.Add(drpaid);
                            }
                        }
                        #endregion
                    }
                    if (htpaymode.Count > 0)
                    {
                        #region
                        double fnlmodecredit = 0;
                        double fnlmodedebit = 0;
                        for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                        {
                            if (chkl_paid.Items[mode].Selected)
                            {
                                double modecredit = 0;
                                double modedebit = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = chkl_paid.Items[mode].Text + "-" + "*";
                                double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "CR"]), out modecredit);
                                double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "DR"]), out modedebit);
                                if (modecredit != 0 || modedebit != 0)
                                {
                                    drpaid["Credit"] = Convert.ToString(modecredit);
                                    drpaid["Debit"] = Convert.ToString(modedebit);
                                    dtpaid.Rows.Add(drpaid);
                                    fnlmodecredit += modecredit;
                                    fnlmodedebit += modedebit;
                                }
                            }
                        }
                        if (fnlmodecredit != 0 || fnlmodedebit != 0)
                        {
                            drpaid = dtpaid.NewRow();
                            drpaid["Sno"] = "Paymode Total" + "-" + "*";
                            drpaid["Credit"] = Convert.ToString(fnlmodecredit);
                            drpaid["Debit"] = Convert.ToString(fnlmodedebit);
                            dtpaid.Rows.Add(drpaid);
                        }
                        #endregion
                    }
                    if (htSubTot.Count > 0)
                    {
                        #region
                        //final receipt and payment amount
                        double rcptAmt = 0;
                        double payAmt = 0;
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Receipt" + "-" + "*";
                        double.TryParse(Convert.ToString(htSubTot["Credit"]), out rcptAmt);
                        drpaid["Credit"] = Convert.ToString(rcptAmt);
                        dtpaid.Rows.Add(drpaid);

                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Payment" + "-" + "*";
                        double.TryParse(Convert.ToString(htSubTot["Debit"]), out payAmt);
                        drpaid["Debit"] = Convert.ToString(payAmt);
                        dtpaid.Rows.Add(drpaid);
                        //balance 
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Balance" + "-" + "*";
                        drpaid["Credit"] = Convert.ToString(rcptAmt - payAmt);
                        dtpaid.Rows.Add(drpaid);
                        #endregion
                    }
                    tblFirst += 3;
                    tblSecond += 3;
                    tblThird += 3;
                }
            }
        }
        catch { dtpaid.Clear(); }
        return dtpaid;
    }

    protected DataTable loadPaidDetails(DataSet dspaid, ref Hashtable htpayMode)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            ArrayList arTranstype = new ArrayList();
            arTranstype.Add("1");
            arTranstype.Add("2");
            arTranstype.Add("3");
            ArrayList arMemType = getMemType();

            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("Header_Name");
            dtpaid.Columns.Add("Credit");
            dtpaid.Columns.Add("Debit");
            int tblCount = 0;
            int tblFirst = 0;
            int tblSecond = 0;
            int tblThird = 0;
            if (dtpaid.Columns.Count > 0)
            {
                DataRow drpaid;
                int rowCnt = 0;
                tblFirst = 0;
                tblSecond = 1;
                tblThird = 2;
                Hashtable htgrandtotalcr = new Hashtable();
                Hashtable htgrandtotaldr = new Hashtable();
                foreach (string memType in arMemType)
                {
                    Hashtable htSubTot = new Hashtable();
                    Hashtable htpaymode = new Hashtable();
                    bool boolMemtype = false;
                    tblCount++;
                    string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                    for (int ar = 0; ar < arTranstype.Count; ar++)
                    {
                        #region
                        int TransTypeVal = 0;
                        int.TryParse(Convert.ToString(arTranstype[ar]), out TransTypeVal);
                        for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                        {
                            double tempCredit = 0;
                            double tempDebit = 0;
                            if (chkl_paid.Items[mode].Selected)
                            {
                                string payMode = Convert.ToString(chkl_paid.Items[mode].Value);
                                string payModeText = Convert.ToString(chkl_paid.Items[mode].Text);
                                if (!htpayMode.ContainsKey(chkl_paid.Items[mode].Value))
                                    htpayMode.Add(chkl_paid.Items[mode].Value, chkl_paid.Items[mode].Text);
                                if (payMode.Trim() != "2" && payMode.Trim() != "3")
                                {
                                    #region cash,online,card

                                    dspaid.Tables[tblFirst].DefaultView.RowFilter = "paymode='" + payMode + "' and Transtype='" + TransTypeVal + "'";
                                    DataView dvpaid = dspaid.Tables[tblFirst].DefaultView;
                                    dvpaid.Sort = "transdate";//Added by saranya on 28/03/2018
                                    if (dvpaid.Count > 0)
                                    {
                                        string dispText = string.Empty;
                                        string transText = TransTypeVal == 3 ? "Journal Entry" : "";
                                        if (transText != "")
                                            dispText = payModeText + "-(" + transText + ")";
                                        else
                                            dispText = payModeText;
                                        drpaid = dtpaid.NewRow();
                                        drpaid["Sno"] = strMemType + "~" + dispText + "#" + "Mode";
                                        dtpaid.Rows.Add(drpaid);
                                        for (int row = 0; row < dvpaid.Count; row++)
                                        {
                                            drpaid = dtpaid.NewRow();
                                            double credit = 0;
                                            double debit = 0;
                                            drpaid["Sno"] = Convert.ToString(rowCnt++);
                                            drpaid["Header_Name"] = Convert.ToString(dvpaid[row]["headerName"]);
                                            double.TryParse(Convert.ToString(dvpaid[row]["debit"]), out credit);
                                            double.TryParse(Convert.ToString(dvpaid[row]["credit"]), out debit);
                                            drpaid["Credit"] = Convert.ToString(credit);
                                            drpaid["Debit"] = Convert.ToString(debit);
                                            tempCredit += credit;
                                            tempDebit += debit;

                                            if (TransTypeVal != 3)
                                            {
                                                if (!htSubTot.ContainsKey("Credit"))
                                                    htSubTot.Add("Credit", credit);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                    amount += credit;
                                                    htSubTot.Remove("Credit");
                                                    htSubTot.Add("Credit", Convert.ToString(amount));
                                                }

                                                if (!htSubTot.ContainsKey("Debit"))
                                                    htSubTot.Add("Debit", debit);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                    amount += debit;
                                                    htSubTot.Remove("Debit");
                                                    htSubTot.Add("Debit", Convert.ToString(amount));
                                                }
                                                //paymode
                                                if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                    htpaymode.Add(payModeText + "-" + "CR", credit);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                    amount += credit;
                                                    htpaymode.Remove(payModeText + "-" + "CR");
                                                    htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                }
                                                if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                    htpaymode.Add(payModeText + "-" + "DR", debit);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                    amount += debit;
                                                    htpaymode.Remove(payModeText + "-" + "DR");
                                                    htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                }
                                            }

                                            dtpaid.Rows.Add(drpaid);
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region dd,cheque
                                    bool boolMode = false;
                                    for (int bkrow = 0; bkrow < dspaid.Tables[tblThird].Rows.Count; bkrow++)
                                    {
                                        bool boolBank = false;
                                        dspaid.Tables[tblSecond].DefaultView.RowFilter = "paymode='" + payMode + "' and Deposite_BankFK='" + dspaid.Tables[tblThird].Rows[bkrow]["textcode"] + "' and Transtype='" + arTranstype[ar] + "'";
                                        DataView dvpaid = dspaid.Tables[tblSecond].DefaultView;
                                        dvpaid.Sort = "transdate";//Added by saranya on 28/03/2018
                                        if (dvpaid.Count > 0)
                                        {
                                            if (!boolMode)
                                            {
                                                string dispText = string.Empty;
                                                string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                                if (transText != "")
                                                    dispText = payModeText + "-(" + transText + ")";
                                                else
                                                    dispText = payModeText;
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = strMemType + "~" + dispText + "#" + "Mode";
                                                dtpaid.Rows.Add(drpaid);
                                                boolMode = true;
                                            }
                                            if (!boolBank)
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = Convert.ToString(dspaid.Tables[2].Rows[bkrow]["textval"]) + "$" + "BankName";
                                                dtpaid.Rows.Add(drpaid);
                                                boolBank = true;
                                            }
                                            #region
                                            double indivBankCredit = 0;
                                            double indivBankDebit = 0;
                                            for (int row = 0; row < dvpaid.Count; row++)
                                            {
                                                drpaid = dtpaid.NewRow();
                                                double credit = 0;
                                                double debit = 0;
                                                drpaid["Sno"] = Convert.ToString(rowCnt++);
                                                drpaid["Header_Name"] = Convert.ToString(dvpaid[row]["headerName"]);
                                                double.TryParse(Convert.ToString(dvpaid[row]["debit"]), out credit);
                                                double.TryParse(Convert.ToString(dvpaid[row]["credit"]), out debit);
                                                drpaid["Credit"] = Convert.ToString(credit);
                                                drpaid["Debit"] = Convert.ToString(debit);
                                                tempCredit += credit;
                                                tempDebit += debit;
                                                indivBankCredit += credit;
                                                indivBankDebit += debit;

                                                if (TransTypeVal != 3)
                                                {
                                                    if (!htSubTot.ContainsKey("Credit"))
                                                        htSubTot.Add("Credit", credit);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                        amount += credit;
                                                        htSubTot.Remove("Credit");
                                                        htSubTot.Add("Credit", Convert.ToString(amount));
                                                    }

                                                    if (!htSubTot.ContainsKey("Debit"))
                                                        htSubTot.Add("Debit", debit);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                        amount += debit;
                                                        htSubTot.Remove("Debit");
                                                        htSubTot.Add("Debit", Convert.ToString(amount));
                                                    }
                                                    //paymode
                                                    if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                        htpaymode.Add(payModeText + "-" + "CR", credit);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                        amount += credit;
                                                        htpaymode.Remove(payModeText + "-" + "CR");
                                                        htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                    }
                                                    if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                        htpaymode.Add(payModeText + "-" + "DR", debit);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                        amount += debit;
                                                        htpaymode.Remove(payModeText + "-" + "DR");
                                                        htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                    }
                                                }

                                                dtpaid.Rows.Add(drpaid);
                                            }
                                            if (indivBankCredit != 0 || indivBankDebit != 0)//individual bankwise total
                                            {
                                                drpaid = dtpaid.NewRow();
                                                if (TransTypeVal != 3)
                                                    drpaid["Sno"] = "Total" + "-" + "*";
                                                else
                                                    drpaid["Sno"] = "Journal Total" + "-" + "*";
                                                drpaid["Credit"] = Convert.ToString(indivBankCredit);
                                                drpaid["Debit"] = Convert.ToString(indivBankDebit);
                                                dtpaid.Rows.Add(drpaid);
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                            }
                            if (tempCredit != 0 || tempDebit != 0)//total
                            {
                                drpaid = dtpaid.NewRow();
                                if (TransTypeVal != 3)
                                    drpaid["Sno"] = "Total" + "-" + "*";
                                else
                                    drpaid["Sno"] = "Journal Total" + "-" + "*";
                                drpaid["Credit"] = Convert.ToString(tempCredit);
                                drpaid["Debit"] = Convert.ToString(tempDebit);
                                dtpaid.Rows.Add(drpaid);
                            }
                        }
                        #endregion
                    }
                    if (htpaymode.Count > 0)
                    {
                        #region
                        double fnlmodecredit = 0;
                        double fnlmodedebit = 0;
                        for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                        {
                            if (chkl_paid.Items[mode].Selected)
                            {
                                double modecredit = 0;
                                double modedebit = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = chkl_paid.Items[mode].Text + "-" + "*";
                                double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "CR"]), out modecredit);
                                double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "DR"]), out modedebit);
                                if (modecredit != 0 || modedebit != 0)
                                {
                                    drpaid["Credit"] = Convert.ToString(modecredit);
                                    drpaid["Debit"] = Convert.ToString(modedebit);
                                    dtpaid.Rows.Add(drpaid);
                                    fnlmodecredit += modecredit;
                                    fnlmodedebit += modedebit;
                                    //=========================added by abarna 6.4.2018==========================
                                    if (!htgrandtotalcr.ContainsKey(chkl_paid.Items[mode].Text))
                                        htgrandtotalcr.Add(chkl_paid.Items[mode].Text, Convert.ToString(modecredit));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htgrandtotalcr[chkl_paid.Items[mode].Text]), out amount);
                                        amount += modecredit;
                                        htgrandtotalcr.Remove(chkl_paid.Items[mode].Text);
                                        htgrandtotalcr.Add(chkl_paid.Items[mode].Text, Convert.ToString(amount));
                                    }

                                    if (!htgrandtotaldr.ContainsKey(chkl_paid.Items[mode].Text))
                                        htgrandtotaldr.Add(chkl_paid.Items[mode].Text, Convert.ToString(modedebit));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htgrandtotaldr[chkl_paid.Items[mode].Text]), out amount);
                                        amount += modedebit;
                                        htgrandtotaldr.Remove(chkl_paid.Items[mode].Text);
                                        htgrandtotaldr.Add(chkl_paid.Items[mode].Text, Convert.ToString(amount));
                                    }
                                    //=================================================================================
                                }
                            }
                        }
                        if (fnlmodecredit != 0 || fnlmodedebit != 0)
                        {
                            drpaid = dtpaid.NewRow();
                            drpaid["Sno"] = "Paymode Total" + "-" + "*";
                            drpaid["Credit"] = Convert.ToString(fnlmodecredit);
                            drpaid["Debit"] = Convert.ToString(fnlmodedebit);
                            dtpaid.Rows.Add(drpaid);
                        }
                        #endregion
                    }
                    if (htSubTot.Count > 0)
                    {
                        #region
                        //final receipt and payment amount
                        double rcptAmt = 0;
                        double payAmt = 0;
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Receipt" + "-" + "*";
                        double.TryParse(Convert.ToString(htSubTot["Credit"]), out rcptAmt);
                        drpaid["Credit"] = Convert.ToString(rcptAmt);
                        dtpaid.Rows.Add(drpaid);

                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Payment" + "-" + "*";
                        double.TryParse(Convert.ToString(htSubTot["Debit"]), out payAmt);
                        drpaid["Debit"] = Convert.ToString(payAmt);
                        dtpaid.Rows.Add(drpaid);
                        //balance 
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Balance" + "-" + "*";
                        drpaid["Credit"] = Convert.ToString(rcptAmt - payAmt);
                        dtpaid.Rows.Add(drpaid);
                        #endregion
                    }
                    tblFirst += 3;
                    tblSecond += 3;
                    tblThird += 3;
                }
                //added by abarna-----------------------
                drpaid = dtpaid.NewRow();
                drpaid["Sno"] = "GrandPaymodeWise Total" + "-" + "*";
                dtpaid.Rows.Add(drpaid);
                double fnlmodecredittot = 0;
                double fnlmodedebittot = 0;
                for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                {

                    if (chkl_paid.Items[mode].Selected)
                    {
                        double modecredittot = 0;
                        double modedebittot = 0;

                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = chkl_paid.Items[mode].Text + "-" + "*";
                        double.TryParse(Convert.ToString(htgrandtotalcr[chkl_paid.Items[mode].Text]), out modecredittot);
                        double.TryParse(Convert.ToString(htgrandtotaldr[chkl_paid.Items[mode].Text]), out modedebittot);
                        if (modecredittot != 0 || modedebittot != 0)
                        {
                            drpaid["Credit"] = Convert.ToString(modecredittot);
                            drpaid["Debit"] = Convert.ToString(modedebittot);
                            fnlmodecredittot += modecredittot;
                            fnlmodedebittot += modedebittot;
                            dtpaid.Rows.Add(drpaid);
                        }
                    }
                }
                if (fnlmodecredittot != 0 || fnlmodedebittot != 0)
                {
                    drpaid = dtpaid.NewRow();
                    drpaid["Sno"] = "GrandPaymode Total" + "-" + "*";
                    drpaid["Credit"] = Convert.ToString(fnlmodecredittot);
                    drpaid["Debit"] = Convert.ToString(fnlmodedebittot);
                    dtpaid.Rows.Add(drpaid);
                }
                //-------------------------------------
            }
        }
        catch { dtpaid.Clear(); }
        return dtpaid;
    }

    protected void loadSpreadDetails(DataTable dtpaid, ref Hashtable htpayMode)
    {
        try
        {
            #region design

            DataTable dtHeaderWiseReport = new DataTable();
            DataRow drowInst;
            ArrayList arrColHdrNames = new ArrayList();
            arrColHdrNames.Add("S.No");
            dtHeaderWiseReport.Columns.Add("col0");
            if (!cbDate.Checked)
            {
                arrColHdrNames.Add(rblmode.SelectedItem.Text);
                dtHeaderWiseReport.Columns.Add("col1");
            }
            else
            {
                arrColHdrNames.Add("Date");
                dtHeaderWiseReport.Columns.Add("col1");
            }
            arrColHdrNames.Add("Credit");
            dtHeaderWiseReport.Columns.Add("col2");
            arrColHdrNames.Add("Debit");
            dtHeaderWiseReport.Columns.Add("col3");
            DataRow drHdr1 = dtHeaderWiseReport.NewRow();
            for (int grCol = 0; grCol < dtHeaderWiseReport.Columns.Count; grCol++)
            {
                drHdr1["col" + grCol] = arrColHdrNames[grCol];
            }
            dtHeaderWiseReport.Rows.Add(drHdr1);

            #endregion

            #region value
            string payType = string.Empty;
            int rowCnt = 0;
            int height = 0;
            for (int row = 0; row < dtpaid.Rows.Count; row++)
            {
                height += 10;
                string payModeText = Convert.ToString(dtpaid.Rows[row]["Sno"]);
                if (!payModeText.Trim().Contains("*"))
                {
                    bool boolcheck = false;
                    if (payModeText.Trim().Contains("#"))
                    {
                        payType = payModeText.Split('#')[0];
                        drowInst = dtHeaderWiseReport.NewRow();
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        drowInst[0] = payType;
                        dicHeaderWise.Add(grdRow, payType.Split('~')[1]);

                        boolcheck = true;
                        if (payType.Contains("-(Journal Entry)"))
                            payType = payType.Split('-')[0];
                        if (payType.Contains("~"))
                            payType = payType.Split('~')[1];
                        //rowColor(payType, 0, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                        dtHeaderWiseReport.Rows.Add(drowInst);
                    }
                    if (payModeText.Trim().Contains("$"))
                    {
                        payType = payModeText.Split('$')[0];
                        drowInst = dtHeaderWiseReport.NewRow();
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        drowInst[0] = payType;
                        dicHeaderWise.Add(grdRow, payModeText.Split('$')[1]);
                        boolcheck = true;
                        dtHeaderWiseReport.Rows.Add(drowInst);
                    }
                    if (!boolcheck)
                    {
                        drowInst = dtHeaderWiseReport.NewRow();
                        drowInst[0] = ++rowCnt;
                        drowInst[1] = Convert.ToString(dtpaid.Rows[row]["Header_Name"]);
                        drowInst[2] = Convert.ToString(dtpaid.Rows[row]["Credit"]);
                        drowInst[3] = Convert.ToString(dtpaid.Rows[row]["Debit"]);
                        dtHeaderWiseReport.Rows.Add(drowInst);
                    }
                }
                else
                {
                    drowInst = dtHeaderWiseReport.NewRow();
                    drowInst[0] = payModeText.Split('*')[0].TrimEnd('-');
                    if (payModeText.Split('*')[0].TrimEnd('-').Trim() == "Total" || payModeText.Split('*')[0].TrimEnd('-').Trim() == "Paymode Total" || payModeText.Split('*')[0].TrimEnd('-').Trim() == "Journal Total" || payModeText.Split('*')[0].TrimEnd('-').Trim() == "GrandPaymodeWise Total" || payModeText.Split('*')[0].TrimEnd('-').Trim() == "GrandPaymode Total")//change by abarna 6.4.2018
                    {
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        dicHeaderWise.Add(grdRow, payModeText.Split('*')[0].TrimEnd('-'));
                    }
                    else
                    {
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        dicHeaderWise.Add(grdRow, payModeText.Split('*')[0]);
                    }
                    drowInst[1] = Convert.ToString(dtpaid.Rows[row]["Header_Name"]);
                    drowInst[2] = Convert.ToString(dtpaid.Rows[row]["Credit"]);
                    drowInst[3] = Convert.ToString(dtpaid.Rows[row]["Debit"]);
                    dtHeaderWiseReport.Rows.Add(drowInst);
                }
            }
            grdHeaderWiseCollection.DataSource = dtHeaderWiseReport;
            grdHeaderWiseCollection.DataBind();
            grdHeaderWiseCollection.Visible = true;

            #region Grid ColSpan and Color

            foreach (KeyValuePair<int, string> dr in dicHeaderWise)
            {
                int rowcnt = dr.Key;
                int d = Convert.ToInt32(dtHeaderWiseReport.Columns.Count);
                string payModeVal = dr.Value.ToString();
                if (payModeVal.Trim().Contains("-"))
                {
                    int PayTot = Convert.ToInt32(dtHeaderWiseReport.Columns.Count);
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = PayTot - 2;
                    for (int Coltot = 1; Coltot < PayTot - 2; Coltot++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[2].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[3].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Right;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[Coltot].Visible = false;
                    }
                }
                else
                {
                    if (payModeVal != "Total" && payModeVal != "Paymode Total" && payModeVal != "Journal Total" && payModeVal != "GrandPaymodeWise Total" && payModeVal != "GrandPaymode Total")
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Size = 13;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = d;
                        if (payModeVal == "Cash")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#F08080");
                        else if (payModeVal == "Cheque")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                        else if (payModeVal == "DD")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FFA500");
                        else if (payModeVal == "Online")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#90EE90");
                        else if (payModeVal == "Card")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                        for (int a = 1; a < d; a++)
                        {
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[a].Visible = false;
                        }
                    }
                }
                if (payModeVal == "Total" || payModeVal == "Paymode Total" || payModeVal == "Journal Total" || payModeVal == "GrandPaymodeWise Total" || payModeVal == "GrandPaymode Total")
                {
                    for (int gridCol = 0; gridCol < 2; gridCol++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].BackColor = Color.Green;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].HorizontalAlign = HorizontalAlign.Right;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].Font.Size = 13;
                    }
                    for (int gridCol = 0; gridCol < dtHeaderWiseReport.Columns.Count; gridCol++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].HorizontalAlign = HorizontalAlign.Right;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                    }
                    int PayTot = Convert.ToInt32(dtHeaderWiseReport.Columns.Count);
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = PayTot - 2;
                    for (int Coltot = 1; Coltot < PayTot - 2; Coltot++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[Coltot].Visible = false;
                    }
                }
                if (payModeVal == "BankName")
                {
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Bold = true;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Size = 13;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = d;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = Color.YellowGreen;
                }
            }
            #endregion

            // lblvalidation1.Text = "";
            txtexcelname.Text = "";
            GrdMonthwise.Visible = false;
            print.Visible = true;
            payModeLabels(htpayMode);
            #endregion
        }
        catch { }
    }

    protected void loadSpreadDetailsLedger(DataTable dtpaid, ref Hashtable htpayMode)
    {
        try
        {
            #region design
            DataTable dtHeaderWiseReport = new DataTable();
            DataRow drowInst;
            ArrayList arrColHdrNames = new ArrayList();
            arrColHdrNames.Add("S.No");
            dtHeaderWiseReport.Columns.Add("col0");
            if (!cbDate.Checked)
            {
                arrColHdrNames.Add(rblmode.SelectedItem.Text);
                dtHeaderWiseReport.Columns.Add("col1");
            }
            else
            {
                arrColHdrNames.Add("Date");
                dtHeaderWiseReport.Columns.Add("col1");
            }
            arrColHdrNames.Add("Credit");
            dtHeaderWiseReport.Columns.Add("col2");
            arrColHdrNames.Add("Debit");
            dtHeaderWiseReport.Columns.Add("col3");
            DataRow drHdr1 = dtHeaderWiseReport.NewRow();
            for (int grCol = 0; grCol < dtHeaderWiseReport.Columns.Count; grCol++)
            {
                drHdr1["col" + grCol] = arrColHdrNames[grCol];
            }
            dtHeaderWiseReport.Rows.Add(drHdr1);

            #endregion

            #region value
            string payType = string.Empty;
            int rowCnt = 0;
            int height = 0;

            for (int row = 0; row < dtpaid.Rows.Count; row++)
            {
                height += 10;
                string payModeText = Convert.ToString(dtpaid.Rows[row]["Sno"]);
                if (!payModeText.Trim().Contains("*"))
                {
                    bool boolcheck = false;
                    if (payModeText.Trim().Contains("#"))
                    {
                        payType = payModeText.Split('#')[0];
                        drowInst = dtHeaderWiseReport.NewRow();
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        drowInst[0] = payType;
                        dicHeaderWise.Add(grdRow, payType.Split('~')[1]);
                        boolcheck = true;
                        if (payType.Contains("-(Journal Entry)"))
                            payType = payType.Split('-')[0];
                        if (payType.Contains("~"))
                            payType = payType.Split('~')[1];
                        //rowColor(payType, 0, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                        dtHeaderWiseReport.Rows.Add(drowInst);
                    }
                    if (payModeText.Trim().Contains("$"))
                    {
                        payType = payModeText.Split('$')[0];
                        drowInst = dtHeaderWiseReport.NewRow();
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        drowInst[0] = payType;
                        dicHeaderWise.Add(grdRow, payModeText.Split('$')[1]);
                        boolcheck = true;
                        dtHeaderWiseReport.Rows.Add(drowInst);
                    }
                    if (payModeText.Trim().Contains("!"))
                    {
                        payType = payModeText.Split('!')[0];
                        drowInst = dtHeaderWiseReport.NewRow();
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        drowInst[0] = payType;
                        dicHeaderWise.Add(grdRow, payModeText.Split('!')[1]);
                        boolcheck = true;
                        dtHeaderWiseReport.Rows.Add(drowInst);
                        boolcheck = true;
                    }
                    if (!boolcheck)
                    {
                        drowInst = dtHeaderWiseReport.NewRow();
                        drowInst[0] = ++rowCnt;
                        drowInst[1] = Convert.ToString(dtpaid.Rows[row]["Header_Name"]);
                        drowInst[2] = Convert.ToString(dtpaid.Rows[row]["Credit"]);
                        drowInst[3] = Convert.ToString(dtpaid.Rows[row]["Debit"]);
                        dtHeaderWiseReport.Rows.Add(drowInst);
                    }
                }
                else
                {
                    drowInst = dtHeaderWiseReport.NewRow();
                    drowInst[0] = payModeText.Split('*')[0].TrimEnd('-');
                    if (payModeText.Split('*')[0].TrimEnd('-').Trim() == "Header Total")
                    {
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        dicHeaderWise.Add(grdRow, "Header Total");
                    }
                    if (payModeText.Split('*')[0].TrimEnd('-').Trim() != "Header Total")
                    {
                        if (payModeText.Split('*')[0].TrimEnd('-').Trim() == "Total" || payModeText.Split('*')[0].TrimEnd('-').Trim() == "Paymode Total" || payModeText.Split('*')[0].TrimEnd('-').Trim() == "Journal Total")
                        {
                            grdRow = dtHeaderWiseReport.Rows.Count;
                            dicHeaderWise.Add(grdRow, payModeText.Split('*')[0].TrimEnd('-'));
                        }
                        else
                        {
                            grdRow = dtHeaderWiseReport.Rows.Count;
                            dicHeaderWise.Add(grdRow, payModeText.Split('*')[0]);
                        }
                    }
                    drowInst[2] = Convert.ToString(dtpaid.Rows[row]["Credit"]);
                    drowInst[3] = Convert.ToString(dtpaid.Rows[row]["Debit"]);
                    dtHeaderWiseReport.Rows.Add(drowInst);
                }
            }
            grdHeaderWiseCollection.DataSource = dtHeaderWiseReport;
            grdHeaderWiseCollection.DataBind();
            grdHeaderWiseCollection.Visible = true;

            #region Grid ColSpan and Color

            foreach (KeyValuePair<int, string> dr in dicHeaderWise)
            {
                int rowcnt = dr.Key;
                int d = Convert.ToInt32(dtHeaderWiseReport.Columns.Count);
                string payModeVal = dr.Value.ToString();
                if (payModeVal.Trim().Contains("-"))
                {
                    int PayTot = Convert.ToInt32(dtHeaderWiseReport.Columns.Count);
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = PayTot - 2;
                    for (int Coltot = 1; Coltot < PayTot - 2; Coltot++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[2].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[3].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Right;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[Coltot].Visible = false;
                    }
                }
                else
                {
                    if (payModeVal != "Total" && payModeVal != "Paymode Total" && payModeVal != "Journal Total" && payModeVal != "GrandPaymodeWise Total" && payModeVal != "GrandPaymode Total" && payModeVal != "Header" && payModeVal != "Header Total")
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Size = 13;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = d;
                        if (payModeVal == "Cash")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#F08080");
                        else if (payModeVal == "Cheque")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                        else if (payModeVal == "DD")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FFA500");
                        else if (payModeVal == "Online")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#90EE90");
                        else if (payModeVal == "Card")
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                        for (int a = 1; a < d; a++)
                        {
                            grdHeaderWiseCollection.Rows[rowcnt].Cells[a].Visible = false;
                        }
                    }
                }
                if (payModeVal == "Total" || payModeVal == "Paymode Total" || payModeVal == "Journal Total" || payModeVal == "GrandPaymodeWise Total" || payModeVal == "GrandPaymode Total")
                {
                    for (int gridCol = 0; gridCol < 2; gridCol++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].BackColor = Color.Green;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].HorizontalAlign = HorizontalAlign.Right;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].Font.Size = 13;
                    }
                    for (int gridCol = 0; gridCol < dtHeaderWiseReport.Columns.Count; gridCol++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].HorizontalAlign = HorizontalAlign.Right;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                    }
                    int PayTot = Convert.ToInt32(dtHeaderWiseReport.Columns.Count);
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = PayTot - 2;
                    for (int Coltot = 1; Coltot < PayTot - 2; Coltot++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[Coltot].Visible = false;
                    }
                }
                if (payModeVal == "BankName")
                {
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Bold = true;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Size = 13;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = d;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].BackColor = Color.YellowGreen;
                }
                if (payModeVal == "Header")
                {
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Bold = true;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].Font.Size = 13;
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = d;
                    for (int a = 1; a < d; a++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[a].Visible = false;
                    }
                }
                if (payModeVal == "Header Total")
                {
                    for (int gridCol = 0; gridCol < 2; gridCol++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].BackColor = ColorTranslator.FromHtml("#1A80D8");
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].HorizontalAlign = HorizontalAlign.Right;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].Font.Size = 13;
                    }
                    for (int gridCol = 0; gridCol < dtHeaderWiseReport.Columns.Count; gridCol++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].HorizontalAlign = HorizontalAlign.Right;
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                    }
                    int PayTot = Convert.ToInt32(dtHeaderWiseReport.Columns.Count);
                    grdHeaderWiseCollection.Rows[rowcnt].Cells[0].ColumnSpan = PayTot - 2;
                    for (int Coltot = 1; Coltot < PayTot - 2; Coltot++)
                    {
                        grdHeaderWiseCollection.Rows[rowcnt].Cells[Coltot].Visible = false;
                    }
                }
            }
            #endregion

            // lblvalidation1.Text = "";
            txtexcelname.Text = "";
            GrdMonthwise.Visible = false;
            print.Visible = true;
            payModeLabels(htpayMode);
            #endregion
        }
        catch { }
    }

    //school spread load
    protected DataTable loadPaidDetailsLedgerSchool(DataSet dspaid, ref Hashtable htpayMode)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            ArrayList arTranstype = new ArrayList();
            ArrayList arFnlYear = getSelFinlDate();
            arTranstype.Add("1");
            arTranstype.Add("2");
            arTranstype.Add("3");
            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("Header_Name");
            dtpaid.Columns.Add("Credit");
            dtpaid.Columns.Add("Debit");
            DataRow drpaid;
            int rowCnt = 0;
            ArrayList arMemType = getMemType();
            int tblCount = 0;
            int tblFirst = 0;
            int tblSecond = 0;
            int tblThird = 0;
            Hashtable htSubTot = new Hashtable();
            Hashtable htpaymode = new Hashtable();
            if (dtpaid.Columns.Count > 0)
            {
                tblFirst = 0;
                tblSecond = 1;
                tblThird = 2;
                foreach (string memType in arMemType)
                {
                    string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                    foreach (string fnlYear in arFnlYear)
                    {
                        #region

                        for (int ar = 0; ar < arTranstype.Count; ar++)
                        {
                            int TransTypeVal = 0;
                            int.TryParse(Convert.ToString(arTranstype[ar]), out TransTypeVal);
                            for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                            {
                                double tempCredit = 0;
                                double tempDebit = 0;
                                if (!chkl_paid.Items[mode].Selected)
                                    continue;
                                string payMode = Convert.ToString(chkl_paid.Items[mode].Value);
                                string payModeText = Convert.ToString(chkl_paid.Items[mode].Text);
                                if (!htpayMode.ContainsKey(chkl_paid.Items[mode].Value))
                                    htpayMode.Add(chkl_paid.Items[mode].Value, chkl_paid.Items[mode].Text);
                                if (payMode.Trim() != "2" && payMode.Trim() != "3")
                                {
                                    #region cash,online,card
                                    dspaid.Tables[tblFirst].DefaultView.RowFilter = "paymode='" + payMode + "' and Transtype='" + arTranstype[ar] + "' and actualfinyearfk='" + fnlYear + "'";
                                    DataView dvpaid = dspaid.Tables[tblFirst].DefaultView;
                                    dvpaid.Sort = "transdate";//Added by saranya on 28/03/2018
                                    if (dvpaid.Count > 0)
                                    {
                                        string dispText = string.Empty;
                                        string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                        if (transText != "")
                                            dispText = payModeText + "-(" + transText + ")";
                                        else
                                            dispText = payModeText;
                                        drpaid = dtpaid.NewRow();
                                        drpaid["Sno"] = strMemType + "~" + fnlYear + "~" + dispText + "#" + "Mode";
                                        dtpaid.Rows.Add(drpaid);
                                        for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                        {
                                            double hdCredit = 0;
                                            double hdDebit = 0;
                                            if (chkl_studhed.Items[hd].Selected)
                                            {
                                                string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                                DataTable dtbal = dvpaid.ToTable();
                                                dtbal.DefaultView.RowFilter = "headerName='" + hdName + "'";
                                                DataTable dvpaids = dtbal.DefaultView.ToTable();
                                                dvpaids.DefaultView.Sort = "transdate";//Added by saranya on 28/03/2018
                                                if (dvpaids.Rows.Count > 0)
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    drpaid["Sno"] = hdName + "!" + "Header";
                                                    dtpaid.Rows.Add(drpaid);
                                                    #region
                                                    ArrayList arChecking = new ArrayList();
                                                    for (int row = 0; row < dvpaids.Rows.Count; row++)
                                                    {
                                                        string ldName = Convert.ToString(dvpaids.Rows[row]["Ledgername"]);
                                                        if (!arChecking.Contains(payMode + "-" + TransTypeVal + "-" + fnlYear + "-" + ldName))
                                                        {
                                                            drpaid = dtpaid.NewRow();
                                                            double credit = 0;
                                                            double debit = 0;
                                                            drpaid["Sno"] = Convert.ToString(rowCnt++);
                                                            drpaid["Header_Name"] = Convert.ToString(dvpaids.Rows[row]["Ledgername"]);
                                                            double.TryParse(Convert.ToString(dvpaids.Compute("sum(debit)", "Ledgername='" + ldName + "' and paymode='" + payMode + "' and transtype='" + TransTypeVal + "' and actualfinyearfk='" + fnlYear + "'")), out credit);
                                                            double.TryParse(Convert.ToString(dvpaids.Compute("sum(credit)", "Ledgername='" + ldName + "' and paymode='" + payMode + "' and transtype='" + TransTypeVal + "' and actualfinyearfk='" + fnlYear + "'")), out debit);
                                                            //double.TryParse(Convert.ToString(dvpaids[row]["debit"]), out credit);
                                                            //double.TryParse(Convert.ToString(dvpaids[row]["credit"]), out debit);
                                                            drpaid["Credit"] = Convert.ToString(credit);
                                                            drpaid["Debit"] = Convert.ToString(debit);
                                                            tempCredit += credit;
                                                            tempDebit += debit;
                                                            hdCredit += credit;
                                                            hdDebit += debit;

                                                            if (TransTypeVal != 3)
                                                            {
                                                                if (!htSubTot.ContainsKey("Credit"))
                                                                    htSubTot.Add("Credit", credit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                                    amount += credit;
                                                                    htSubTot.Remove("Credit");
                                                                    htSubTot.Add("Credit", Convert.ToString(amount));
                                                                }

                                                                if (!htSubTot.ContainsKey("Debit"))
                                                                    htSubTot.Add("Debit", debit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                                    amount += debit;
                                                                    htSubTot.Remove("Debit");
                                                                    htSubTot.Add("Debit", Convert.ToString(amount));
                                                                }
                                                                //paymode
                                                                if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                                    htpaymode.Add(payModeText + "-" + "CR", credit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                                    amount += credit;
                                                                    htpaymode.Remove(payModeText + "-" + "CR");
                                                                    htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                                }
                                                                if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                                    htpaymode.Add(payModeText + "-" + "DR", debit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                                    amount += debit;
                                                                    htpaymode.Remove(payModeText + "-" + "DR");
                                                                    htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                                }
                                                            }

                                                            dtpaid.Rows.Add(drpaid);
                                                            arChecking.Add(payMode + "-" + TransTypeVal + "-" + fnlYear + "-" + ldName);
                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            if (hdCredit != 0 || hdDebit != 0)//every header total
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = "Header Total" + "-" + "*";
                                                drpaid["Credit"] = Convert.ToString(hdCredit);
                                                drpaid["Debit"] = Convert.ToString(hdDebit);
                                                dtpaid.Rows.Add(drpaid);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region dd,cheque
                                    bool boolMode = false;
                                    for (int bkrow = 0; bkrow < dspaid.Tables[tblThird].Rows.Count; bkrow++)
                                    {
                                        bool boolBank = false;
                                        dspaid.Tables[tblSecond].DefaultView.RowFilter = "paymode='" + payMode + "' and Deposite_BankFK='" + dspaid.Tables[tblThird].Rows[bkrow]["textcode"] + "' and Transtype='" + arTranstype[ar] + "' and actualfinyearfk='" + fnlYear + "'";
                                        DataView dvpaid = dspaid.Tables[tblSecond].DefaultView;
                                        dvpaid.Sort = "transdate";//Added by saranya on 28/03/2018
                                        if (dvpaid.Count > 0)
                                        {
                                            if (!boolMode)
                                            {
                                                string dispText = string.Empty;
                                                string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                                if (transText != "")
                                                    dispText = payModeText + "-(" + transText + ")";
                                                else
                                                    dispText = payModeText;
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = strMemType + "~" + fnlYear + "~" + dispText + "#" + "Mode";
                                                dtpaid.Rows.Add(drpaid);
                                                boolMode = true;
                                            }
                                            if (!boolBank)
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = Convert.ToString(dspaid.Tables[2].Rows[bkrow]["textval"]) + "$" + "BankName";
                                                dtpaid.Rows.Add(drpaid);
                                                boolBank = true;
                                            }
                                            double indivBankCredit = 0;
                                            double indivBankDebit = 0;
                                            for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                            {
                                                double hdCredit = 0;
                                                double hdDebit = 0;
                                                if (chkl_studhed.Items[hd].Selected)
                                                {
                                                    string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                                    DataTable dtbal = dvpaid.ToTable();
                                                    dtbal.DefaultView.RowFilter = "headerName='" + hdName + "'";
                                                    DataTable dvpaids = dtbal.DefaultView.ToTable();
                                                    dvpaids.DefaultView.Sort = "transdate";//Added by saranya on 28/03/2018
                                                    if (dvpaids.Rows.Count > 0)
                                                    {
                                                        drpaid = dtpaid.NewRow();
                                                        drpaid["Sno"] = hdName + "!" + "Header";
                                                        dtpaid.Rows.Add(drpaid);
                                                        #region
                                                        ArrayList arChecking = new ArrayList();
                                                        for (int row = 0; row < dvpaids.Rows.Count; row++)
                                                        {
                                                            string ldName = Convert.ToString(dvpaids.Rows[row]["Ledgername"]);
                                                            if (!arChecking.Contains(payMode + "-" + TransTypeVal + "-" + fnlYear + "-" + ldName))
                                                            {
                                                                drpaid = dtpaid.NewRow();
                                                                double credit = 0;
                                                                double debit = 0;
                                                                drpaid["Sno"] = Convert.ToString(rowCnt++);
                                                                drpaid["Header_Name"] = Convert.ToString(dvpaids.Rows[row]["Ledgername"]);
                                                                //double.TryParse(Convert.ToString(dvpaids[row]["debit"]), out credit);
                                                                //double.TryParse(Convert.ToString(dvpaids[row]["credit"]), out debit);
                                                                double.TryParse(Convert.ToString(dvpaids.Compute("sum(debit)", "Ledgername='" + ldName + "' and paymode='" + payMode + "' and transtype='" + TransTypeVal + "' and actualfinyearfk='" + fnlYear + "'")), out credit);
                                                                double.TryParse(Convert.ToString(dvpaids.Compute("sum(credit)", "Ledgername='" + ldName + "' and paymode='" + payMode + "' and transtype='" + TransTypeVal + "' and actualfinyearfk='" + fnlYear + "'")), out debit);
                                                                drpaid["Credit"] = Convert.ToString(credit);
                                                                drpaid["Debit"] = Convert.ToString(debit);
                                                                tempCredit += credit;
                                                                tempDebit += debit;
                                                                hdCredit += credit;
                                                                hdDebit += debit;
                                                                indivBankCredit += credit;
                                                                indivBankDebit += debit;
                                                                if (TransTypeVal != 3)
                                                                {
                                                                    if (!htSubTot.ContainsKey("Credit"))
                                                                        htSubTot.Add("Credit", credit);
                                                                    else
                                                                    {
                                                                        double amount = 0;
                                                                        double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                                        amount += credit;
                                                                        htSubTot.Remove("Credit");
                                                                        htSubTot.Add("Credit", Convert.ToString(amount));
                                                                    }

                                                                    if (!htSubTot.ContainsKey("Debit"))
                                                                        htSubTot.Add("Debit", debit);
                                                                    else
                                                                    {
                                                                        double amount = 0;
                                                                        double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                                        amount += debit;
                                                                        htSubTot.Remove("Debit");
                                                                        htSubTot.Add("Debit", Convert.ToString(amount));
                                                                    }
                                                                    //paymode
                                                                    if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                                        htpaymode.Add(payModeText + "-" + "CR", credit);
                                                                    else
                                                                    {
                                                                        double amount = 0;
                                                                        double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                                        amount += credit;
                                                                        htpaymode.Remove(payModeText + "-" + "CR");
                                                                        htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                                    }
                                                                    if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                                        htpaymode.Add(payModeText + "-" + "DR", debit);
                                                                    else
                                                                    {
                                                                        double amount = 0;
                                                                        double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                                        amount += debit;
                                                                        htpaymode.Remove(payModeText + "-" + "DR");
                                                                        htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                                    }
                                                                }

                                                                dtpaid.Rows.Add(drpaid);
                                                                arChecking.Add(payMode + "-" + TransTypeVal + "-" + fnlYear + "-" + ldName);
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }
                                                if (hdCredit != 0 || hdDebit != 0)//every header total
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    drpaid["Sno"] = "Header Total" + "-" + "*";
                                                    drpaid["Credit"] = Convert.ToString(hdCredit);
                                                    drpaid["Debit"] = Convert.ToString(hdDebit);
                                                    dtpaid.Rows.Add(drpaid);
                                                }
                                            }
                                            if (indivBankCredit != 0 || indivBankDebit != 0)//total
                                            {
                                                drpaid = dtpaid.NewRow();
                                                if (TransTypeVal != 3)
                                                    drpaid["Sno"] = "Total" + "-" + "*";
                                                else
                                                    drpaid["Sno"] = "Journal Total" + "-" + "*";
                                                drpaid["Credit"] = Convert.ToString(indivBankCredit);
                                                drpaid["Debit"] = Convert.ToString(indivBankDebit);
                                                dtpaid.Rows.Add(drpaid);
                                            }

                                        }
                                    }
                                    #endregion
                                }
                                if (tempCredit != 0 || tempDebit != 0)//total
                                {
                                    drpaid = dtpaid.NewRow();
                                    if (TransTypeVal != 3)
                                        drpaid["Sno"] = "Total" + "-" + "*";
                                    else
                                        drpaid["Sno"] = "Journal Total" + "-" + "*";
                                    drpaid["Credit"] = Convert.ToString(tempCredit);
                                    drpaid["Debit"] = Convert.ToString(tempDebit);
                                    dtpaid.Rows.Add(drpaid);
                                }
                            }
                        }

                        #endregion
                    }
                    tblFirst += 3;
                    tblSecond += 3;
                    tblThird += 3;
                }
                if (htpaymode.Count > 0)
                {
                    #region
                    double fnlmodecredit = 0;
                    double fnlmodedebit = 0;
                    for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                    {
                        if (chkl_paid.Items[mode].Selected)
                        {
                            double modecredit = 0;
                            double modedebit = 0;
                            drpaid = dtpaid.NewRow();
                            drpaid["Sno"] = chkl_paid.Items[mode].Text + "-" + "*";
                            double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "CR"]), out modecredit);
                            double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "DR"]), out modedebit);
                            if (modecredit != 0 || modedebit != 0)
                            {
                                drpaid["Credit"] = Convert.ToString(modecredit);
                                drpaid["Debit"] = Convert.ToString(modedebit);
                                dtpaid.Rows.Add(drpaid);
                                fnlmodecredit += modecredit;
                                fnlmodedebit += modedebit;
                            }
                        }
                    }
                    if (fnlmodecredit != 0 || fnlmodedebit != 0)
                    {
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Paymode Total" + "-" + "*";
                        drpaid["Credit"] = Convert.ToString(fnlmodecredit);
                        drpaid["Debit"] = Convert.ToString(fnlmodedebit);
                        dtpaid.Rows.Add(drpaid);
                    }
                    #endregion
                }
                if (htSubTot.Count > 0)
                {
                    #region
                    //final receipt and payment amount
                    double rcptAmt = 0;
                    double payAmt = 0;
                    drpaid = dtpaid.NewRow();
                    drpaid["Sno"] = "Receipt" + "-" + "*";
                    double.TryParse(Convert.ToString(htSubTot["Credit"]), out rcptAmt);
                    drpaid["Credit"] = Convert.ToString(rcptAmt);
                    dtpaid.Rows.Add(drpaid);

                    drpaid = dtpaid.NewRow();
                    drpaid["Sno"] = "Payment" + "-" + "*";
                    double.TryParse(Convert.ToString(htSubTot["Debit"]), out payAmt);
                    drpaid["Debit"] = Convert.ToString(payAmt);
                    dtpaid.Rows.Add(drpaid);
                    //balance 
                    drpaid = dtpaid.NewRow();
                    drpaid["Sno"] = "Balance" + "-" + "*";
                    drpaid["Credit"] = Convert.ToString(rcptAmt - payAmt);
                    dtpaid.Rows.Add(drpaid);
                    #endregion
                }
            }
        }
        catch { dtpaid.Clear(); }
        return dtpaid;
    }

    protected DataTable loadPaidDetailsSchool(DataSet dspaid, ref Hashtable htpayMode)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            ArrayList arTranstype = new ArrayList();
            ArrayList arFnlYear = getSelFinlDate();
            arTranstype.Add("1");
            arTranstype.Add("2");
            arTranstype.Add("3");
            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("Header_Name");
            dtpaid.Columns.Add("Credit");
            dtpaid.Columns.Add("Debit");
            DataRow drpaid;
            int rowCnt = 0;
            ArrayList arMemType = getMemType();

            int tblCount = 0;
            int tblFirst = 0;
            int tblSecond = 0;

            int tblThird = 0;
            Hashtable htSubTot = new Hashtable();
            Hashtable htpaymode = new Hashtable();
            if (dtpaid.Columns.Count > 0)
            {
                tblFirst = 0;
                tblSecond = 1;
                tblThird = 2;
                foreach (string memType in arMemType)
                {
                    string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                    foreach (string fnlYear in arFnlYear)
                    {
                        #region

                        for (int ar = 0; ar < arTranstype.Count; ar++)
                        {
                            int TransTypeVal = 0;
                            int.TryParse(Convert.ToString(arTranstype[ar]), out TransTypeVal);
                            for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                            {
                                double tempCredit = 0;
                                double tempDebit = 0;
                                if (!chkl_paid.Items[mode].Selected)
                                    continue;
                                string payMode = Convert.ToString(chkl_paid.Items[mode].Value);
                                string payModeText = Convert.ToString(chkl_paid.Items[mode].Text);
                                if (!htpayMode.ContainsKey(chkl_paid.Items[mode].Value))
                                    htpayMode.Add(chkl_paid.Items[mode].Value, chkl_paid.Items[mode].Text);
                                if (payMode.Trim() != "2" && payMode.Trim() != "3")
                                {
                                    #region cash,online,card

                                    dspaid.Tables[tblFirst].DefaultView.RowFilter = "paymode='" + payMode + "' and Transtype='" + TransTypeVal + "' and actualfinyearfk='" + fnlYear + "'";
                                    //===============Added by saranya on 28/03/2018===========//
                                    DataView dvdtPaid = dspaid.Tables[tblFirst].DefaultView;
                                    dvdtPaid.Sort = "transdate";
                                    DataTable dvpaid = dvdtPaid.ToTable();
                                    //DataTable dvpaid = dspaid.Tables[tblFirst].DefaultView.ToTable();
                                    //=================================================================//
                                    if (dvpaid.Rows.Count > 0)
                                    {
                                        string dispText = string.Empty;
                                        string transText = TransTypeVal == 3 ? "Journal Entry" : "";
                                        if (transText != "")
                                            dispText = payModeText + "-(" + transText + ")";
                                        else
                                            dispText = payModeText;
                                        drpaid = dtpaid.NewRow();
                                        drpaid["Sno"] = strMemType + "~" + fnlYear + "~" + dispText + "#" + "Mode";
                                        dtpaid.Rows.Add(drpaid);
                                        ArrayList arChecking = new ArrayList();
                                        for (int row = 0; row < dvpaid.Rows.Count; row++)
                                        {
                                            string hdName = Convert.ToString(dvpaid.Rows[row]["headerName"]);
                                            if (!arChecking.Contains(payMode + "-" + TransTypeVal + "-" + fnlYear + "-" + hdName))
                                            {
                                                drpaid = dtpaid.NewRow();
                                                double credit = 0;
                                                double debit = 0;
                                                drpaid["Sno"] = Convert.ToString(rowCnt++);
                                                drpaid["Header_Name"] = Convert.ToString(dvpaid.Rows[row]["headerName"]);

                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(debit)", "headerName='" + hdName + "' and paymode='" + payMode + "' and transtype='" + TransTypeVal + "' and actualfinyearfk='" + fnlYear + "'")), out credit);
                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(credit)", "headerName='" + hdName + "' and paymode='" + payMode + "' and transtype='" + TransTypeVal + "' and actualfinyearfk='" + fnlYear + "'")), out debit);
                                                // double.TryParse(Convert.ToString(dvpaid.Rows[row]["debit"]), out credit);
                                                //  double.TryParse(Convert.ToString(dvpaid.Rows[row]["credit"]), out debit);
                                                drpaid["Credit"] = Convert.ToString(credit);
                                                drpaid["Debit"] = Convert.ToString(debit);
                                                tempCredit += credit;
                                                tempDebit += debit;

                                                if (TransTypeVal != 3)
                                                {
                                                    if (!htSubTot.ContainsKey("Credit"))
                                                        htSubTot.Add("Credit", credit);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                        amount += credit;
                                                        htSubTot.Remove("Credit");
                                                        htSubTot.Add("Credit", Convert.ToString(amount));
                                                    }

                                                    if (!htSubTot.ContainsKey("Debit"))
                                                        htSubTot.Add("Debit", debit);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                        amount += debit;
                                                        htSubTot.Remove("Debit");
                                                        htSubTot.Add("Debit", Convert.ToString(amount));
                                                    }
                                                    //paymode
                                                    if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                        htpaymode.Add(payModeText + "-" + "CR", credit);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                        amount += credit;
                                                        htpaymode.Remove(payModeText + "-" + "CR");
                                                        htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                    }
                                                    if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                        htpaymode.Add(payModeText + "-" + "DR", debit);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                        amount += debit;
                                                        htpaymode.Remove(payModeText + "-" + "DR");
                                                        htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                    }
                                                }

                                                dtpaid.Rows.Add(drpaid);
                                                arChecking.Add(payMode + "-" + TransTypeVal + "-" + fnlYear + "-" + hdName);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region dd,cheque
                                    bool boolMode = false;
                                    for (int bkrow = 0; bkrow < dspaid.Tables[tblThird].Rows.Count; bkrow++)
                                    {
                                        bool boolBank = false;
                                        dspaid.Tables[tblSecond].DefaultView.RowFilter = "paymode='" + payMode + "' and Deposite_BankFK='" + dspaid.Tables[tblThird].Rows[bkrow]["textcode"] + "' and Transtype='" + arTranstype[ar] + "' and actualfinyearfk='" + fnlYear + "'";

                                        //================Added by saranya on 28/03/2018===============//
                                        //DataTable dvpaid = dspaid.Tables[tblSecond].DefaultView.ToTable();
                                        DataView dvdtPaid = dspaid.Tables[tblSecond].DefaultView;
                                        dvdtPaid.Sort = "transdate";
                                        DataTable dvpaid = dvdtPaid.ToTable();
                                        //=============================================================//
                                        if (dvpaid.Rows.Count > 0)
                                        {
                                            if (!boolMode)
                                            {
                                                string dispText = string.Empty;
                                                string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                                if (transText != "")
                                                    dispText = payModeText + "-(" + transText + ")";
                                                else
                                                    dispText = payModeText;
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = strMemType + "~" + fnlYear + "~" + dispText + "#" + "Mode";
                                                dtpaid.Rows.Add(drpaid);
                                                boolMode = true;
                                            }
                                            if (!boolBank)
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = Convert.ToString(dspaid.Tables[2].Rows[bkrow]["textval"]) + "$" + "BankName";
                                                dtpaid.Rows.Add(drpaid);
                                                boolBank = true;
                                            }
                                            #region
                                            double indivBankCredit = 0;
                                            double indivBankDebit = 0;
                                            ArrayList arChecking = new ArrayList();
                                            for (int row = 0; row < dvpaid.Rows.Count; row++)
                                            {
                                                string hdName = Convert.ToString(dvpaid.Rows[row]["headerName"]);
                                                if (!arChecking.Contains(payMode + "-" + TransTypeVal + "-" + fnlYear + "-" + hdName))
                                                {

                                                    drpaid = dtpaid.NewRow();
                                                    double credit = 0;
                                                    double debit = 0;
                                                    drpaid["Sno"] = Convert.ToString(rowCnt++);
                                                    drpaid["Header_Name"] = Convert.ToString(dvpaid.Rows[row]["headerName"]);
                                                    double.TryParse(Convert.ToString(dvpaid.Compute("sum(debit)", "headerName='" + hdName + "' and paymode='" + payMode + "' and transtype='" + TransTypeVal + "' and actualfinyearfk='" + fnlYear + "'")), out credit);
                                                    double.TryParse(Convert.ToString(dvpaid.Compute("sum(credit)", "headerName='" + hdName + "' and paymode='" + payMode + "' and transtype='" + TransTypeVal + "' and actualfinyearfk='" + fnlYear + "'")), out debit);
                                                    //double.TryParse(Convert.ToString(dvpaid.Rows[row]["debit"]), out credit);
                                                    //double.TryParse(Convert.ToString(dvpaid.Rows[row]["credit"]), out debit);
                                                    drpaid["Credit"] = Convert.ToString(credit);
                                                    drpaid["Debit"] = Convert.ToString(debit);
                                                    tempCredit += credit;
                                                    tempDebit += debit;
                                                    indivBankCredit += credit;
                                                    indivBankDebit += debit;

                                                    if (TransTypeVal != 3)
                                                    {
                                                        if (!htSubTot.ContainsKey("Credit"))
                                                            htSubTot.Add("Credit", credit);
                                                        else
                                                        {
                                                            double amount = 0;
                                                            double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                            amount += credit;
                                                            htSubTot.Remove("Credit");
                                                            htSubTot.Add("Credit", Convert.ToString(amount));
                                                        }

                                                        if (!htSubTot.ContainsKey("Debit"))
                                                            htSubTot.Add("Debit", debit);
                                                        else
                                                        {
                                                            double amount = 0;
                                                            double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                            amount += debit;
                                                            htSubTot.Remove("Debit");
                                                            htSubTot.Add("Debit", Convert.ToString(amount));
                                                        }
                                                        //paymode
                                                        if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                            htpaymode.Add(payModeText + "-" + "CR", credit);
                                                        else
                                                        {
                                                            double amount = 0;
                                                            double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                            amount += credit;
                                                            htpaymode.Remove(payModeText + "-" + "CR");
                                                            htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                        }
                                                        if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                            htpaymode.Add(payModeText + "-" + "DR", debit);
                                                        else
                                                        {
                                                            double amount = 0;
                                                            double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                            amount += debit;
                                                            htpaymode.Remove(payModeText + "-" + "DR");
                                                            htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                        }
                                                    }

                                                    dtpaid.Rows.Add(drpaid);
                                                    arChecking.Add(payMode + "-" + TransTypeVal + "-" + fnlYear + "-" + hdName);
                                                }
                                            }
                                            if (indivBankCredit != 0 || indivBankDebit != 0)//individual bankwise total
                                            {
                                                drpaid = dtpaid.NewRow();
                                                if (TransTypeVal != 3)
                                                    drpaid["Sno"] = "Total" + "-" + "*";
                                                else
                                                    drpaid["Sno"] = "Journal Total" + "-" + "*";
                                                drpaid["Credit"] = Convert.ToString(indivBankCredit);
                                                drpaid["Debit"] = Convert.ToString(indivBankDebit);
                                                dtpaid.Rows.Add(drpaid);
                                            }
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                                if (tempCredit != 0 || tempDebit != 0)//total
                                {
                                    drpaid = dtpaid.NewRow();
                                    if (TransTypeVal != 3)
                                        drpaid["Sno"] = "Total" + "-" + "*";
                                    else
                                        drpaid["Sno"] = "Journal Total" + "-" + "*";
                                    drpaid["Credit"] = Convert.ToString(tempCredit);
                                    drpaid["Debit"] = Convert.ToString(tempDebit);
                                    dtpaid.Rows.Add(drpaid);
                                }
                            }
                        }

                        #endregion
                    }
                    tblFirst += 3;
                    tblSecond += 3;
                    tblThird += 3;
                }
                if (htpaymode.Count > 0)
                {
                    #region
                    double fnlmodecredit = 0;
                    double fnlmodedebit = 0;
                    for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                    {
                        if (chkl_paid.Items[mode].Selected)
                        {
                            double modecredit = 0;
                            double modedebit = 0;
                            drpaid = dtpaid.NewRow();
                            drpaid["Sno"] = chkl_paid.Items[mode].Text + "-" + "*";
                            double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "CR"]), out modecredit);
                            double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "DR"]), out modedebit);
                            if (modecredit != 0 || modedebit != 0)
                            {
                                drpaid["Credit"] = Convert.ToString(modecredit);
                                drpaid["Debit"] = Convert.ToString(modedebit);
                                dtpaid.Rows.Add(drpaid);
                                fnlmodecredit += modecredit;
                                fnlmodedebit += modedebit;
                            }
                        }
                    }
                    if (fnlmodecredit != 0 || fnlmodedebit != 0)
                    {
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Paymode Total" + "-" + "*";
                        drpaid["Credit"] = Convert.ToString(fnlmodecredit);
                        drpaid["Debit"] = Convert.ToString(fnlmodedebit);
                        dtpaid.Rows.Add(drpaid);
                    }
                    #endregion
                }
                if (htSubTot.Count > 0)
                {
                    #region
                    //final receipt and payment amount
                    double rcptAmt = 0;
                    double payAmt = 0;
                    drpaid = dtpaid.NewRow();
                    drpaid["Sno"] = "Receipt" + "-" + "*";
                    double.TryParse(Convert.ToString(htSubTot["Credit"]), out rcptAmt);
                    drpaid["Credit"] = Convert.ToString(rcptAmt);
                    dtpaid.Rows.Add(drpaid);

                    drpaid = dtpaid.NewRow();
                    drpaid["Sno"] = "Payment" + "-" + "*";
                    double.TryParse(Convert.ToString(htSubTot["Debit"]), out payAmt);
                    drpaid["Debit"] = Convert.ToString(payAmt);
                    dtpaid.Rows.Add(drpaid);
                    //balance 
                    drpaid = dtpaid.NewRow();
                    drpaid["Sno"] = "Balance" + "-" + "*";
                    drpaid["Credit"] = Convert.ToString(rcptAmt - payAmt);
                    dtpaid.Rows.Add(drpaid);
                    #endregion
                }
            }
        }
        catch { dtpaid.Clear(); }
        return dtpaid;
    }

    protected ArrayList getMemType()
    {
        ArrayList arMemType = new ArrayList();
        try
        {
            arMemType.Add("1");
            if (cbIncOthers.Checked)
            {
                arMemType.Add("2");
                arMemType.Add("3");
                arMemType.Add("4");
            }
        }
        catch { }
        return arMemType;
    }

    protected void rowColor(string payModeVal, int curColCnt, FarPoint.Web.Spread.FpSpread spreadDet, int rowcnt)
    {
        if (payModeVal == "Cash")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
        else if (payModeVal == "Cheque")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
        else if (payModeVal == "DD")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
        else if (payModeVal == "Online")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
        else if (payModeVal == "Card")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
    }

    protected void payModeLabels(Hashtable htpay)
    {
        lblcash.Visible = false;
        lblchq.Visible = false;
        lbldd.Visible = false;
        lblchal.Visible = false;
        lblonline.Visible = false;
        lblcard.Visible = false;
        lblNeft.Visible = false;//Added by saranya 0n 13/02/2018
        foreach (DictionaryEntry row in htpay)
        {
            if (row.Key.ToString() == "1")
                lblcash.Visible = true;
            if (row.Key.ToString() == "2")
                lblchq.Visible = true;
            if (row.Key.ToString() == "3")
                lbldd.Visible = true;
            if (row.Key.ToString() == "4")
                lblchal.Visible = true;
            if (row.Key.ToString() == "5")
                lblonline.Visible = true;
            if (row.Key.ToString() == "6")
                lblcard.Visible = true;
            //Added by saranya 0n 13/02/2018
            if (row.Key.ToString() == "7")
                lblNeft.Visible = true;
        }
        divlabl.Visible = true;
    }

    protected bool getValidate()
    {
        bool boolCheck = false;
        try
        {
            if (ds.Tables.Count > 0)
            {
                if ((ds.Tables[0].Rows.Count > 0 || ds.Tables[3].Rows.Count > 0 || ds.Tables[5].Rows.Count > 0 || ds.Tables[7].Rows.Count > 0))
                {

                }
            }
        }
        catch { }
        return boolCheck;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        bool boolCheck = false;
        ds.Clear();
        ds = loadDetails();
        try
        {
            if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[3].Rows.Count > 0 || ds.Tables[5].Rows.Count > 0 || ds.Tables[7].Rows.Count > 0))
            {

            }
        }
        catch { }

        if (!cbMonth.Checked)
        {
            if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[3].Rows.Count > 0 || ds.Tables[5].Rows.Count > 0 || ds.Tables[7].Rows.Count > 0))
            {
                Hashtable htpayMode = new Hashtable();
                DataTable dtpaid = new DataTable();
                double schollSet = checkSchoolSetting();
                if (rblMemType.SelectedIndex == 0)
                {
                    if (cbAcdYear.Checked)
                    {
                        Dictionary<string, string> getAcdYear = new Dictionary<string, string>();

                        #region Academic Year
                        DataSet dsNornaml = ds.Copy();
                        try
                        {
                            string clgCode = Convert.ToString(getCblSelectedValue(cblclg));
                            string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                            getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);
                            DataSet dsFinal = new DataSet();
                            if (getAcdYear.Count > 0)
                            {
                                bool boolDs = false;
                                // DataTable dtFirst = ds.Tables[0].DefaultView.ToTable();
                                foreach (KeyValuePair<string, string> getVal in getAcdYear)
                                {
                                    string feeCate = getVal.Value.Replace(",", "','");
                                    if (checkSchoolSetting() != 0)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";////abarna 8.03
                                    }
                                    else
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "'  and feecategory in('" + feeCate + "')";//and batch_year='" + getVal.Key.Split('$')[1] + "'//abarna 8.03
                                    }
                                    DataTable dtFirst = ds.Tables[0].DefaultView.ToTable();
                                    if (checkSchoolSetting() != 0)
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and feecategory in('" + feeCate + "') and batch_year='" + getVal.Key.Split('$')[1] + "'";
                                    }
                                    else
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and feecategory in('" + feeCate + "') ";
                                    }
                                    DataTable dtSecond = ds.Tables[1].DefaultView.ToTable();


                                    //ds.Tables[2].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                                    //DataTable dtThird = ds.Tables[2].DefaultView.ToTable();

                                    if (!boolDs)
                                    {
                                        dsFinal.Reset();
                                        dsFinal.Tables.Add(dtFirst);
                                        dsFinal.Tables.Add(dtSecond);
                                        // dsFinal.Tables.Add(dtThird);
                                        boolDs = true;
                                    }
                                    else
                                    {
                                        dsFinal.Merge(dtFirst);
                                        dsFinal.Merge(dtSecond);
                                        // dsFinal.Merge(dtThird);
                                    }
                                }
                            }
                            ds.Reset();
                            if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                            {
                                string flTName = string.Empty;
                                string flThName = string.Empty;
                                if (rblmode.SelectedIndex == 0)
                                {
                                    flTName = "headerName";
                                }
                                else
                                {
                                    flTName = "headerName";
                                    flThName = "ledgername";
                                }
                                DataTable dtPertbl = new DataTable();
                                if (rblmode.SelectedIndex == 0)
                                {
                                    DataTable dtColumns = new DataTable();
                                    DataTable tempTbl = new DataTable();
                                    if (checkSchoolSetting() != 0)
                                    {
                                        dtColumns = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, "paymode", "transtype", "transdate");//transdate Added by saranya on 28.03.2018
                                    }
                                    else
                                    {
                                        dtColumns = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, "paymode", "transtype", "actualfinyearfk", "transdate");//change by abarna 22.1.2018//transdate Added by saranya on 28.03.2018
                                    }
                                    if (checkSchoolSetting() != 0)
                                    {
                                        tempTbl = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, "debit", "credit", "paymode", "transtype", "transdate", "feecategory");//transdate Added by saranya on 28.03.2018
                                    }
                                    else
                                    {
                                        tempTbl = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, "debit", "credit", "paymode", "transtype", "actualfinyearfk", "transdate");//change by abarna 22.1.2018//transdate Added by saranya on 28.03.2018
                                    }

                                    dtPertbl = tempTbl.DefaultView.ToTable();
                                    dtPertbl.Rows.Clear();
                                    foreach (DataRow drRow in dtColumns.Rows)
                                    {
                                        if (checkSchoolSetting() != 0)
                                        {
                                            tempTbl.DefaultView.RowFilter = "headername='" + drRow[flTName] + "' and paymode='" + drRow["paymode"] + "' and transtype='" + drRow["transtype"] + "'  and transdate='" + drRow["transdate"] + "'";//transdate Added by saranya on 28.03.2018
                                        }
                                        else
                                        {
                                            tempTbl.DefaultView.RowFilter = "headername='" + drRow[flTName] + "' and paymode='" + drRow["paymode"] + "' and transtype='" + drRow["transtype"] + "' and actualfinyearfk='" + drRow["actualfinyearfk"] + "' and transdate='" + drRow["transdate"] + "'";//change by abarna 22.1.2018//transdate Added by saranya on 28.03.2018
                                        }

                                        DataRow drPer = dtPertbl.NewRow();
                                        drPer[flTName] = drRow[flTName];
                                        drPer["debit"] = tempTbl.DefaultView.ToTable().Compute("SUM(debit)", "");
                                        drPer["credit"] = tempTbl.DefaultView.ToTable().Compute("SUM(credit)", "");
                                        drPer["paymode"] = drRow["paymode"];
                                        drPer["transtype"] = drRow["transtype"];
                                        drPer["transdate"] = drRow["transdate"];

                                        if (checkSchoolSetting() == 0)
                                        {
                                            drPer["actualfinyearfk"] = drRow["actualfinyearfk"];//change by abarna 22.1.2018
                                        }
                                        dtPertbl.Rows.Add(drPer);
                                    }
                                }
                                else
                                {
                                    DataTable dtColumns = new DataTable();
                                    DataTable tempTbl = new DataTable();
                                    if (checkSchoolSetting() != 0)
                                    {
                                        dtColumns = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, flThName, "paymode", "transtype", "transdate");//transdate Added by saranya on 28.03.2018
                                    }
                                    else
                                    {
                                        dtColumns = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, flThName, "paymode", "transtype", "actualfinyearfk", "transdate");//change by abarna 22.1.2018//transdate Added by saranya on 28.03.2018
                                    }
                                    if (checkSchoolSetting() != 0)
                                    {
                                        tempTbl = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, flThName, "debit", "credit", "paymode", "transtype", "transdate");//transdate Added by saranya on 28.03.2018
                                    }
                                    else
                                    {

                                        tempTbl = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, flThName, "debit", "credit", "paymode", "transtype", "actualfinyearfk", "transdate");//change by abarna 22.1.2018//transdate Added by saranya on 28.03.2018
                                    }
                                    dtPertbl = tempTbl.DefaultView.ToTable();
                                    dtPertbl.Rows.Clear();
                                    foreach (DataRow drRow in dtColumns.Rows)
                                    {
                                        if (checkSchoolSetting() != 0)
                                        {
                                            tempTbl.DefaultView.RowFilter = "headername='" + drRow[flTName] + "' and ledgername='" + drRow[flThName] + "' and paymode='" + drRow["paymode"] + "' and transtype='" + drRow["transtype"] + "' and transdate='" + drRow["transdate"] + "'";//transdate Added by saranya on 28.03.2018
                                        }
                                        else
                                        {
                                            tempTbl.DefaultView.RowFilter = "headername='" + drRow[flTName] + "' and ledgername='" + drRow[flThName] + "' and paymode='" + drRow["paymode"] + "' and transtype='" + drRow["transtype"] + "'and actualfinyearfk='" + drRow["actualfinyearfk"] + "' and transdate='" + drRow["transdate"] + "'";//change by abarna 22.1.2018//transdate Added by saranya on 28.03.2018
                                        }
                                        DataRow drPer = dtPertbl.NewRow();
                                        drPer[flTName] = drRow[flTName];
                                        drPer[flThName] = drRow[flThName];
                                        drPer["debit"] = tempTbl.DefaultView.ToTable().Compute("SUM(debit)", "");
                                        drPer["credit"] = tempTbl.DefaultView.ToTable().Compute("SUM(credit)", "");
                                        drPer["paymode"] = drRow["paymode"];
                                        drPer["transtype"] = drRow["transtype"];
                                        drPer["transdate"] = drRow["transdate"];
                                        if (checkSchoolSetting() == 0)
                                        {
                                            drPer["actualfinyearfk"] = drRow["actualfinyearfk"];//change by abarna 22.1.2018
                                        }
                                        dtPertbl.Rows.Add(drPer);
                                    }
                                }

                                //  var varGroup = from row in tempTbl.AsEnumerable() group row by row.Field<string>("headerName") into grp select new { headerName = grp.Key, debit = grp.Sum(r => r.Field<int>("debit")) };
                                DataTable tempTblOne = dsFinal.Tables[1].DefaultView.ToTable();
                                DataTable dtThird = dsNornaml.Tables[2].DefaultView.ToTable();
                                //tempTbl.Columns.Remove("college_code");
                                //tempTbl.Columns.Remove("batch_year");
                                //tempTbl.Columns.Remove("feecategory");

                                //tempTblOne.Columns.Remove("college_code");
                                //tempTblOne.Columns.Remove("batch_year");
                                //tempTblOne.Columns.Remove("feecategory");
                                //headerName,ledgerName

                                ds.Reset();
                                //ds.Tables.Add(tempTbl);
                                ds.Tables.Add(dtPertbl);
                                ds.Tables.Add(tempTblOne);
                                ds.Tables.Add(dtThird);
                                if (cbIncOthers.Checked)
                                {
                                    ds.Tables.Add(dsNornaml.Tables[3].DefaultView.ToTable());
                                    ds.Tables.Add(dsNornaml.Tables[4].DefaultView.ToTable());
                                    ds.Tables.Add(dsNornaml.Tables[5].DefaultView.ToTable());

                                    ds.Tables.Add(dsNornaml.Tables[6].DefaultView.ToTable());
                                    ds.Tables.Add(dsNornaml.Tables[7].DefaultView.ToTable());
                                    ds.Tables.Add(dsNornaml.Tables[8].DefaultView.ToTable());

                                    ds.Tables.Add(dsNornaml.Tables[9].DefaultView.ToTable());
                                    ds.Tables.Add(dsNornaml.Tables[10].DefaultView.ToTable());
                                    ds.Tables.Add(dsNornaml.Tables[11].DefaultView.ToTable());
                                }

                            }

                        }
                        catch
                        {
                            ds.Reset();
                            ds = dsNornaml.Copy();
                        }
                        #endregion
                    }

                    #region

                    if (rblmode.SelectedIndex == 0)
                    {
                        if (schollSet == 0)//school
                            dtpaid = loadPaidDetailsSchool(ds, ref htpayMode);
                        else//college
                            dtpaid = loadPaidDetails(ds, ref htpayMode);
                    }
                    else
                    {
                        if (schollSet == 0)//school
                            dtpaid = loadPaidDetailsLedgerSchool(ds, ref htpayMode);
                        else//college
                            dtpaid = loadPaidDetailsLedger(ds, ref htpayMode);
                    }
                    if (dtpaid.Rows.Count > 0)
                    {
                        if (rblmode.SelectedIndex == 0)
                            loadSpreadDetails(dtpaid, ref htpayMode);
                        else
                            loadSpreadDetailsLedger(dtpaid, ref htpayMode);
                    }
                    else
                        boolCheck = true;
                    #endregion
                }
                else//Others
                {
                    #region
                    if (rblmode.SelectedIndex == 0)
                        dtpaid = loadPaidDetailsOthers(ds, ref htpayMode);
                    else
                        dtpaid = loadPaidDetailsLedgerOthers(ds, ref htpayMode);
                    if (dtpaid.Rows.Count > 0)
                    {
                        if (rblmode.SelectedIndex == 0)
                            loadSpreadDetails(dtpaid, ref htpayMode);
                        else
                            loadSpreadDetailsLedger(dtpaid, ref htpayMode);
                    }
                    else
                        boolCheck = true;
                    #endregion
                }
                if (boolCheck)
                {
                    txtexcelname.Text = string.Empty;
                    grdHeaderWiseCollection.Visible = false;
                    print.Visible = false;
                    divlabl.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                }
            }
            else
            {
                //lblvalidation1.Text = string.Empty;
                txtexcelname.Text = string.Empty;
                grdHeaderWiseCollection.Visible = false;
                print.Visible = false;
                divlabl.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                //lbl_alert.Text = "No Record Found";
                //imgdiv2.Visible = true;
            }
        }
        if (cbMonth.Checked)
        {
            if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0))
            {
                loadSpreadMonthwise();
            }
            else
            {
                txtexcelname.Text = string.Empty;
                grdHeaderWiseCollection.Visible = false;
                GrdMonthwise.Visible = false;
                print.Visible = false;
                divlabl.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
        }
    }

    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;

    }

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (cbMonth.Checked == false)
            {
                string reportname = txtexcelname.Text;
                if (reportname.ToString().Trim() != "")
                {
                    d2.printexcelreportgrid(grdHeaderWiseCollection, reportname);
                    // d2.printexcelreport(spreadDet, reportname);
                    lblvalidation1.Visible = false;
                }
                else
                {
                    lblvalidation1.Text = "Please Enter Your  Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
            }
            if (cbMonth.Checked == true)
            {
                string reportname = txtexcelname.Text;
                if (reportname.ToString().Trim() != "")
                {
                    d2.printexcelreportgrid(GrdMonthwise, reportname);
                    //d2.printexcelreport(spreadMonthWise, reportname);
                    lblvalidation1.Visible = false;
                }
                else
                {
                    lblvalidation1.Text = "Please Enter Your  Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
            }
        }
        catch
        { }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = Session["usercode"].ToString();
            if (cbMonth.Checked == false)
            {
                // lblvalidation1.Text = "";
                string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));

                txtexcelname.Text = "";
                string degreedetails;
                string pagename;
                // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
                degreedetails = "Daybook Abstract Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + " " + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");

                //    degreedetails = "Daybook Abstract Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
                pagename = "MulInstHdCollection.aspx";

                Printcontrolhed.loadspreaddetails(grdHeaderWiseCollection, pagename, degreedetails, 0, ss);
                Printcontrolhed.Visible = true;
            }
            if (cbMonth.Checked == true)
            {
                // lblvalidation1.Text = "";
                string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));

                txtexcelname.Text = "";
                string degreedetails;
                string pagename;
                // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
                degreedetails = "Monthwise Abstract Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + " " + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");

                //    degreedetails = "Daybook Abstract Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
                pagename = "MulInstHdCollection.aspx";

                Printcontrolhed.loadspreaddetails(GrdMonthwise, pagename, degreedetails, 0, ss);
                Printcontrolhed.Visible = true;
            }
        }
        catch { }
    }

    protected void btn_print_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = Session["usercode"].ToString();
            // lblvalidation1.Text = "";
            if (cbMonth.Checked == false)
            {
                string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
                txtexcelname.Text = "";
                string degreedetails;
                string pagename;
                degreedetails = "Daybook Abstract Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + " " + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
                pagename = "MulInstHdCollection.aspx";
                Printcontrolhed.loadspreaddetails(grdHeaderWiseCollection, pagename, degreedetails, 0, ss);
                Printcontrolhed.Visible = true;
            }
            if (cbMonth.Checked == true)
            {
                string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
                txtexcelname.Text = "";
                string degreedetails;
                string pagename;
                degreedetails = "MonthWise Abstract Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + " " + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
                pagename = "MulInstHdCollection.aspx";
                Printcontrolhed.loadspreaddetails(GrdMonthwise, pagename, degreedetails, 0, ss);
                // Printcontrolhed.loadspreaddetails(spreadMonthWise, pagename, degreedetails, 1, Convert.ToString(Session["usercode"]));
                Printcontrolhed.Visible = true;
            }
        }
        catch { }
    }

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

    #endregion

    #region print settings

    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
            #region Excel print settings
            string usertype = "";
            if (usercode.Trim() != "")
                usertype = " and usercode='" + usercode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch { }
    }

    #endregion

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

    # endregion

    //added by sudhagar 18.05.2017
    protected void rblmode_Selected(object sender, EventArgs e)
    {
        txtexcelname.Text = string.Empty;
        grdHeaderWiseCollection.Visible = false;
        print.Visible = false;
        divlabl.Visible = false;
        checkdicon.Checked = false;
    }

    #region Include setting

    protected void checkdicon_Changed(object sender, EventArgs e)
    {
        try
        {
            if (checkdicon.Checked == true)
            {
                txtinclude.Enabled = true;
                LoadIncludeSetting();
            }
            else
            {
                txtinclude.Enabled = false;
                cblinclude.Items.Clear();
                // LoadIncludeSetting();
            }
        }
        catch { }
    }

    private void LoadIncludeSetting()
    {
        try
        {
            cblinclude.Items.Clear();
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Course Completed", "1"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Debar", "2"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Discontinue", "3"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Cancel", "4"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Prolong Absent", "5"));
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    cblinclude.Items[i].Selected = false;
                }
                cbinclude.Checked = false;
                txtinclude.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void cbinclude_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Setting", "--Select--");
    }

    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Setting", "--Select--");

    }

    #endregion

    //discontinue,delflag
    //protected string getStudCategory()
    //{
    //    string strInclude = string.Empty;
    //    try
    //    {
    //        #region includem

    //        string cc = "";
    //        string debar = "";
    //        string disc = "";
    //        string cancel = "";
    //        if (cblinclude.Items.Count > 0)
    //        {
    //            for (int i = 0; i < cblinclude.Items.Count; i++)
    //            {
    //                if (cblinclude.Items[i].Selected == true)
    //                {
    //                    if (cblinclude.Items[i].Value == "1")
    //                        cc = " r.cc=1";
    //                    if (cblinclude.Items[i].Value == "2")
    //                        debar = " r.Exam_Flag like '%debar'";
    //                    if (cblinclude.Items[i].Value == "3")
    //                        disc = "  r.DelFlag=1";
    //                    if (cblinclude.Items[i].Value == "4")
    //                        cancel = "  r.DelFlag=2";
    //                }
    //            }
    //        }
    //        if (!checkdicon.Checked)
    //        {
    //            if (cc != "" && debar == "" && disc == "" && cancel == "")
    //                strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
    //            if (cc == "" && debar != "" && disc == "" && cancel == "")
    //                strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
    //            if (cc == "" && debar == "" && disc != "" && cancel == "")
    //                strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
    //            if (cc == "" && debar == "" && disc == "" && cancel != "")
    //                strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
    //            //2
    //            if (cc != "" && debar != "" && disc == "" && cancel == "")
    //                strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
    //            if (cc != "" && debar == "" && disc != "" && cancel == "")
    //                strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
    //            if (cc != "" && debar == "" && disc == "" && cancel != "")
    //                strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
    //            //
    //            if (cc == "" && debar != "" && disc != "" && cancel == "")
    //                strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
    //            if (cc == "" && debar != "" && disc == "" && cancel != "")
    //                strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
    //            //
    //            if (cc == "" && debar == "" && disc != "" && cancel != "")
    //                strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
    //            //3
    //            if (cc != "" && debar != "" && disc != "" && cancel == "")
    //                strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
    //            if (cc != "" && debar == "" && disc != "" && cancel != "")
    //                strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
    //            if (cc != "" && debar != "" && disc == "" && cancel != "")
    //                strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
    //            if (cc == "" && debar != "" && disc != "" && cancel != "")
    //                strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
    //            if (cc == "" && debar == "" && disc == "" && cancel == "")
    //                strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
    //            if (cc != "" && debar != "" && disc != "" && cancel != "")
    //                strInclude = "";
    //        }
    //        else
    //        {
    //            if (cc != "" && debar == "" && disc == "" && cancel == "")
    //                strInclude = " and " + cc + "";
    //            if (cc == "" && debar != "" && disc == "" && cancel == "")
    //                strInclude = " and " + debar + "";
    //            if (cc == "" && debar == "" && disc != "" && cancel == "")
    //                strInclude = " and " + disc + "";
    //            if (cc == "" && debar == "" && disc == "" && cancel != "")
    //                strInclude = " and " + cancel + "";
    //            //2
    //            if (cc != "" && debar != "" && disc == "" && cancel == "")
    //                strInclude = " and( " + cc + " or " + debar + ")";
    //            if (cc != "" && debar == "" && disc != "" && cancel == "")
    //                strInclude = " and (" + cc + " or " + disc + ")";
    //            if (cc != "" && debar == "" && disc == "" && cancel != "")
    //                strInclude = " and (" + cc + " or " + cancel + ")";
    //            //
    //            if (cc == "" && debar != "" && disc != "" && cancel == "")
    //                strInclude = " and (" + debar + " or " + disc + ")";
    //            if (cc == "" && debar != "" && disc == "" && cancel != "")
    //                strInclude = " and (" + debar + " or " + cancel + ")";
    //            //
    //            if (cc == "" && debar == "" && disc != "" && cancel != "")
    //                strInclude = " and (" + disc + " or " + cancel + ")";
    //            //3
    //            if (cc != "" && debar != "" && disc != "" && cancel == "")
    //                strInclude = " and (" + cc + " or " + debar + " or " + disc + ")";
    //            if (cc != "" && debar == "" && disc != "" && cancel != "")
    //                strInclude = " and (" + cc + " or " + disc + " or " + cancel + ")";
    //            if (cc != "" && debar != "" && disc == "" && cancel != "")
    //                strInclude = " and (" + cc + " or " + debar + " or " + cancel + ")";
    //            if (cc == "" && debar != "" && disc != "" && cancel != "")
    //                strInclude = " and (" + debar + " or " + disc + " or " + cancel + ")";
    //            if (cc == "" && debar == "" && disc == "" && cancel == "")
    //                strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
    //            if (cc != "" && debar != "" && disc != "" && cancel != "")
    //                strInclude = " and (" + cc + " or " + debar + " or " + disc + " or " + cancel + ")";
    //        }

    //        #endregion
    //    }
    //    catch { }
    //    return strInclude;
    //}
    //modified by abarna 4.12.2017

    protected string getStudCategory()
    {
        string strInclude = string.Empty;
        try
        {
            #region includem
            string cc = "";
            string debar = "";
            string disc = "";
            string cancel = "";
            string pro = "";
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                            cc = " r.cc=1  ";//and  r.ProlongAbsent=0
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like '%debar'";
                        if (cblinclude.Items[i].Value == "3")
                            disc = "r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0 ";
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                        if (cblinclude.Items[i].Value == "5")
                            pro = " r.ProlongAbsent=1 and r.DelFlag=1";
                    }
                }
            }
            if (checkdicon.Checked)
            {
                if (cc != "")
                    strInclude = "(r.cc=1)";// and  r.ProlongAbsent=0
                if (debar != "")
                {
                    if (strInclude != "")
                    {
                        //strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        // strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                }
                if (disc != "")
                {
                    if (strInclude != "")
                    {
                        strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        strInclude += " (r.DelFlag=1 and isnull(r.ProlongAbsent,'0')=0)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += " r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0)";
                    }
                }
                if (cancel != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "  (r.DelFlag=2)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.DelFlag=2)";
                    }
                }
                if (pro != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += " (r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                }
                if (strInclude != "")

                    strInclude = "and (" + strInclude + ")";
            }
            //if (!checkdicon.Checked)
            //{
            //    if (cc != "" && debar == "" && disc == "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            //    if (cc == "" && debar != "" && disc == "" && cancel == "")
            //        strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
            //    if (cc == "" && debar == "" && disc != "" && cancel == "")
            //        strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
            //    if (cc == "" && debar == "" && disc == "" && cancel != "")
            //        strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
            //    //2
            //    if (cc != "" && debar != "" && disc == "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
            //    if (cc != "" && debar == "" && disc != "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
            //    if (cc != "" && debar == "" && disc == "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
            //    //
            //    if (cc == "" && debar != "" && disc != "" && cancel == "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
            //    if (cc == "" && debar != "" && disc == "" && cancel != "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
            //    //
            //    if (cc == "" && debar == "" && disc != "" && cancel != "")
            //        strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    //3
            //    if (cc != "" && debar != "" && disc != "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
            //    if (cc != "" && debar == "" && disc != "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    if (cc != "" && debar != "" && disc == "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
            //    if (cc == "" && debar != "" && disc != "" && cancel != "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    if (cc == "" && debar == "" && disc == "" && cancel == "")
            //        strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
            //    if (cc != "" && debar != "" && disc != "" && cancel != "")
            //        strInclude = "";
            //}
            else
            {
                // strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0 and isnull(r.ProlongAbsent,'0')=0";
                if (cc == "" && debar == "" && disc == "" && cancel == "" && pro == "")
                {
                    strInclude = "";
                }
                //if (cc != "" && debar == "" && disc == "" && cancel == "")
                //    strInclude = " and " + cc + "";
                //if (cc == "" && debar != "" && disc == "" && cancel == "")
                //    strInclude = " and " + debar + "";
                //if (cc == "" && debar == "" && disc != "" && cancel == "")
                //    strInclude = " and " + disc + "";
                //if (cc == "" && debar == "" && disc == "" && cancel != "")
                //    strInclude = " and " + cancel + "";
                ////2
                //if (cc != "" && debar != "" && disc == "" && cancel == "")
                //    strInclude = " and( " + cc + " or " + debar + ")";
                //if (cc != "" && debar == "" && disc != "" && cancel == "")
                //    strInclude = " and (" + cc + " or " + disc + ")";
                //if (cc != "" && debar == "" && disc == "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + cancel + ")";
                ////
                //if (cc == "" && debar != "" && disc != "" && cancel == "")
                //    strInclude = " and (" + debar + " or " + disc + ")";
                //if (cc == "" && debar != "" && disc == "" && cancel != "")
                //    strInclude = " and (" + debar + " or " + cancel + ")";
                ////
                //if (cc == "" && debar == "" && disc != "" && cancel != "")
                //    strInclude = " and (" + disc + " or " + cancel + ")";
                ////3
                //if (cc != "" && debar != "" && disc != "" && cancel == "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + disc + ")";
                //if (cc != "" && debar == "" && disc != "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + disc + " or " + cancel + ")";
                //if (cc != "" && debar != "" && disc == "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + cancel + ")";
                //if (cc == "" && debar != "" && disc != "" && cancel != "")
                //    strInclude = " and (" + debar + " or " + disc + " or " + cancel + ")";
                //if (cc == "" && debar == "" && disc == "" && cancel == "")
                //    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                //if (cc != "" && debar != "" && disc != "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + disc + " or " + cancel + ")";
            }
            #endregion
        }
        catch { }
        return strInclude;
    }

    /// <summary>
    /// school option included here 04.08.2017 by sudhagar
    /// </summary>
    /// <returns></returns>

    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }

    protected Dictionary<string, string> getFinancialYear()
    {
        Dictionary<string, string> htfinlYR = new Dictionary<string, string>();
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string selQFK = string.Empty;
            selQFK = "  select distinct (convert(nvarchar(15),FinYearStart,103)+'-'+convert(nvarchar(15),FinYearEnd,103)+'-'+convert(varchar(10),collegecode)) as finyear,finyearpk as pk from FM_FinYearMaster where CollegeCode in('" + collegecode + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!htfinlYR.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        htfinlYR.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["finyear"]));
                }
            }
        }
        catch { htfinlYR.Clear(); }
        return htfinlYR;
    }

    protected ArrayList getSelFinlDate()
    {
        ArrayList arDate = new ArrayList();
        try
        {
            for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
            {
                if (!chklsfyear.Items[fnl].Selected)
                    continue;
                string date = Convert.ToString(chklsfyear.Items[fnl].Text);
                string fnlYr = date.Split('-')[0].Split('/')[2] + "-" + date.Split('-')[1].Split('/')[2];
                if (!arDate.Contains(fnlYr))
                    arDate.Add(fnlYr);
            }
        }
        catch { }
        return arDate;
    }

    //08.08.2017
    /// <summary>
    /// others option included here like staff,vendor,others 02.08.2017
    /// </summary>
    /// 

    protected void rblMemType_Selected(object sender, EventArgs e)
    {
        bindheader();
        bindledger();
        loadpaid();
        LoadIncludeSetting();
        bindBank();
        //  loadfinanceyear();
        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fromdate.Attributes.Add("readonly", "readonly");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Attributes.Add("readonly", "readonly");
        getPrintSettings();
        chklsfyear.Items.Clear();
        tdlblfnl.Visible = false;
        tdfnl.Visible = false;
        txtexcelname.Text = string.Empty;
        grdHeaderWiseCollection.Visible = false;
        GrdMonthwise.Visible = false;
        print.Visible = false;
        divlabl.Visible = false;
        if (checkSchoolSetting() == 0)
        {
            loadfinanceyear();
        }
        tdmemtype.Visible = false;
        tdlblStudCat.Visible = false;
        tdvalStudCat.Visible = false;
        tdMemPopup.Visible = false;
        tdJournal.Visible = false;
        lbldisp.Text = string.Empty;
        lbldisp.Visible = false;
        lblval.Text = string.Empty;
        tdOthers.Visible = false;
        if (rblMemType.SelectedIndex == 0)//for others option
        {
            tdlblStudCat.Visible = true;
            tdvalStudCat.Visible = true;
            tdJournal.Visible = true;
            tdOthers.Visible = true;
        }
        else
        {
            tdmemtype.Visible = true;
            tdMemPopup.Visible = true;
            memtype();
        }
        getAcademicYear();
    }

    #region memtype

    private void memtype()
    {
        try
        {
            cblmem.Items.Clear();
            //cblmem.Items.Add(new ListItem("Student", "1"));
            cblmem.Items.Add(new ListItem("Staff", "2"));
            cblmem.Items.Add(new ListItem("Vendor", "3"));
            cblmem.Items.Add(new ListItem("Others", "4"));
            if (cblmem.Items.Count > 0)
            {
                for (int i = 0; i < cblmem.Items.Count; i++)
                {
                    cblmem.Items[i].Selected = true;
                }
                cbmem.Checked = true;
                txtmem.Text = "MemType(" + cblmem.Items.Count + ")";
            }
            tdmemtype.Visible = true;
        }
        catch { }
    }

    protected void cbmem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbmem, cblmem, txtmem, "MemType", "--Select--");
        lbldisp.Text = string.Empty;
        lbldisp.Visible = false;
        lblval.Text = string.Empty;
    }

    protected void cblmem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbmem, cblmem, txtmem, "MemType", "--Select--");
        lbldisp.Text = string.Empty;
        lbldisp.Visible = false;
        lblval.Text = string.Empty;
    }

    #endregion

    #region Others Popup

    protected void btnMemPopup_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = true;
        ddlsearch1_OnSelectedIndexChanged(sender, e);
        btn_staffOK.Visible = false;
        btn_exitstaff.Visible = false;
        GrdStaff.Visible = false;
        lbl_errormsgstaff.Visible = false;
        lbldisp.Text = string.Empty;
        lbldisp.Visible = false;
        lblval.Text = string.Empty;
    }

    protected void btn_staffOK_Click(object sender, EventArgs e)
    {
        try
        {
            lbldisp.Text = string.Empty;
            lblval.Text = string.Empty;
            divTreeView.Visible = false;
            StringBuilder sbStaff = new StringBuilder();
            int rowCnt = 0;
            string staffCode = string.Empty;
            foreach (GridViewRow gvrow in GrdStaff.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    rowCnt++;
                    string stfCode = Convert.ToString(GrdStaff.Rows[RowCnt].Cells[2].Text);//staff appl id
                    sbStaff.Append(stfCode + "','");
                }
            }
            if (sbStaff.Length > 0)
            {
                string selName = string.Empty;
                for (int mem = 0; mem < cblmem.Items.Count; mem++)
                {
                    if (!cblmem.Items[mem].Selected)
                        continue;
                    selName = Convert.ToString(cblmem.Items[mem].Text);
                }
                sbStaff.Remove(sbStaff.Length - 3, 3);
                staffCode = Convert.ToString(sbStaff);
                lbldisp.Text = "You have selected " + rowCnt + " " + selName + "";
                lbldisp.Visible = true;
                lblval.Text = staffCode;
            }
            //Fpspread2.SaveChanges();
            //if (Fpspread2.Sheets[0].RowCount > 0)
            //{
            //    string staffCode = string.Empty;
            //    int rowCnt = 0;
            //    StringBuilder sbStaff = new StringBuilder();
            //    for (int row = 1; row < Fpspread2.Sheets[0].RowCount; row++)
            //    {
            //        int value = 0;
            //        int.TryParse(Convert.ToString(Fpspread2.Sheets[0].Cells[row, 1].Value), out value);
            //        if (value == 1)
            //        {
            //            rowCnt++;
            //            string stfCode = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 1].Tag);//staff appl id
            //            sbStaff.Append(stfCode + "','");
            //        }
            //    }
            //    if (sbStaff.Length > 0)
            //    {
            //        string selName = string.Empty;
            //        for (int mem = 0; mem < cblmem.Items.Count; mem++)
            //        {
            //            if (!cblmem.Items[mem].Selected)
            //                continue;
            //            selName = Convert.ToString(cblmem.Items[mem].Text);
            //        }
            //        sbStaff.Remove(sbStaff.Length - 3, 3);
            //        staffCode = Convert.ToString(sbStaff);
            //        lbldisp.Text = "You have selected " + rowCnt + " " + selName + "";
            //        lbldisp.Visible = true;
            //        lblval.Text = staffCode;
            //    }
            //}
            div_staffLook.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void btn_exitstaff_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = false;
    }

    protected void ddlsearch1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtsearch1.Text = "";
        txtsearch1c.Text = "";
        txtsearch1c.Visible = false;
        txtsearch1.Visible = false;
        if (ddlsearch1.SelectedIndex == 0)
        {
            txtsearch1.Visible = true;
        }
        else
        {
            txtsearch1c.Visible = true;
        }
    }

    protected void btn_go2Staff_Click(object sender, EventArgs e)
    {
        try
        {
            int totSelcount = 0;
            string memName = getSelectedMemName(cblmem, ref   totSelcount);
            if (totSelcount == 1 && memName == "Staff")
            {
                spnHdName.InnerText = "Select The Staff";
                getStaffDetails();
            }
            else if (totSelcount == 1 && memName == "Vendor")
            {
                spnHdName.InnerText = "Select The Vendor";
                getVendorDetails(memName);
            }
            else if (totSelcount == 1 && memName == "Others")
            {
                spnHdName.InnerText = "Select The Others";
                getVendorDetails(memName);
            }
        }
        catch { }

    }

    //staff go method
    protected void getStaffDetails()
    {
        try
        {
            bool boolCheck = false;
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            div_staffLook.Visible = true;
            if (collegecode != null)
            {
                string selq = "";
                if (txtsearch1.Text.Trim() != "")
                {
                    string sname = string.Empty;
                    try
                    {
                        sname = txtsearch1.Text.Trim().Split('-')[0];
                    }
                    catch { sname = txtsearch1.Text.Trim(); }
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + collegecode + "') and staff_name like '" + Convert.ToString(sname) + "%'";
                }
                else if (txtsearch1c.Text.Trim() != "")
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + collegecode + "') and staff_code='" + Convert.ToString(txtsearch1c.Text) + "'";
                }
                else
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + collegecode + "') order by PrintPriority";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtStaffReport = new DataTable();
                    DataRow drowInst;
                    ArrayList arrColHdrNames = new ArrayList();
                    arrColHdrNames.Add("S.No");
                    arrColHdrNames.Add("Staff Code");
                    arrColHdrNames.Add("ApplId");
                    arrColHdrNames.Add("Staff Name");
                    dtStaffReport.Columns.Add("Sno");
                    dtStaffReport.Columns.Add("Staff Code");
                    dtStaffReport.Columns.Add("ApplId");
                    dtStaffReport.Columns.Add("Staff Name");
                    DataRow drHdr1 = dtStaffReport.NewRow();
                    for (int grCol = 0; grCol < dtStaffReport.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    dtStaffReport.Rows.Add(drHdr1);

                    bool boolFirst = false;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        drowInst = dtStaffReport.NewRow();
                        drowInst["Sno"] = Convert.ToString(row + 1);
                        drowInst["Staff Code"] = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]);
                        drowInst["ApplId"] = Convert.ToString(ds.Tables[0].Rows[row]["appl_id"]);
                        drowInst["Staff Name"] = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]);
                        dtStaffReport.Rows.Add(drowInst);
                    }
                    if (dtStaffReport.Columns.Count > 0 && dtStaffReport.Rows.Count > 0)
                    {
                        chkGridSelectAll.Visible = true;
                        divTreeView.Visible = true;
                        GrdStaff.DataSource = dtStaffReport;
                        GrdStaff.DataBind();
                        GrdStaff.Visible = true;
                        lbl_errormsgstaff.Visible = false;

                        GrdStaff.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        GrdStaff.Rows[0].Font.Bold = true;
                        GrdStaff.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                    // div2.Visible = true;
                    lbl_errormsgstaff.Visible = false;

                    if (GrdStaff.Rows.Count > 0)
                    {
                        btn_staffOK.Visible = true;
                        btn_exitstaff.Visible = true;
                    }
                    else
                    {
                        btn_staffOK.Visible = false;
                        btn_exitstaff.Visible = false;
                    }
                    boolCheck = true;
                }
            }
            if (!boolCheck)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
        }
        catch (Exception ex) { }
    }

    //vendor go method
    protected void getVendorDetails(string memName)
    {
        try
        {
            bool boolCheck = false;
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            div_staffLook.Visible = true;
            if (collegecode != null)
            {
                string selq = "";
                if (memName == "Vendor")
                {
                    if (txtsearch1.Text.Trim() != "")
                    {
                        string sname = string.Empty;
                        try
                        {
                            sname = txtsearch1.Text.Trim().Split('-')[0];
                        }
                        catch { sname = txtsearch1.Text.Trim(); }
                        selq = "select VendorCompName,VendorCode ,VendorContactPK as VendorPK  from CO_VendorMaster vm,IM_VendorContactMaster vc where vm.vendorpk=vc.vendorfk and VendorType =1 and VendorCompName like '" + Convert.ToString(sname) + "%'";
                    }
                    else if (txtsearch1c.Text.Trim() != "")
                        selq = "select VendorCompName,VendorCode , VendorContactPK as VendorPK  from CO_VendorMastervm,IM_VendorContactMaster vc where vm.vendorpk=vc.vendorfk and VendorType =1 and Vendorcode= '" + Convert.ToString(txtsearch1c.Text) + "'";
                    else
                        selq = "select VendorCompName,VendorCode ,VendorContactPK as VendorPK  from CO_VendorMaster vm,IM_VendorContactMaster vc where vm.vendorpk=vc.vendorfk and VendorType =1 ";
                }
                else
                {
                    if (txtsearch1.Text.Trim() != "")
                    {
                        string sname = string.Empty;
                        try
                        {
                            sname = txtsearch1.Text.Trim().Split('-')[0];
                        }
                        catch { sname = txtsearch1.Text.Trim(); }
                        selq = "select VendorCompName,VendorCode ,VendorPK  from CO_VendorMaster where VendorType ='-5' and VendorCompName like '" + Convert.ToString(sname) + "%'";
                    }
                    else if (txtsearch1c.Text.Trim() != "")
                        selq = "select VendorCompName,VendorCode ,VendorPK  from CO_VendorMaster where VendorType ='-5' and Vendorcode= '" + Convert.ToString(txtsearch1c.Text) + "'";
                    else
                        selq = "select VendorCompName,VendorCode ,VendorPK  from CO_VendorMaster where VendorType ='-5' ";
                }

                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtVendorReport = new DataTable();
                        DataRow drowInst;
                        ArrayList arrColHdrNames = new ArrayList();
                        arrColHdrNames.Add("S.No");
                        arrColHdrNames.Add("Vendor Code");
                        arrColHdrNames.Add("VendorPK");
                        arrColHdrNames.Add("Vendor Name");
                        dtVendorReport.Columns.Add("Sno");
                        dtVendorReport.Columns.Add("Vendor Code");
                        dtVendorReport.Columns.Add("VendorPK");
                        dtVendorReport.Columns.Add("Vendor Name");
                        DataRow drHdr1 = dtVendorReport.NewRow();
                        for (int grCol = 0; grCol < dtVendorReport.Columns.Count; grCol++)
                            drHdr1[grCol] = arrColHdrNames[grCol];
                        dtVendorReport.Rows.Add(drHdr1);

                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            drowInst = dtVendorReport.NewRow();
                            drowInst["Sno"] = Convert.ToString(row + 1);
                            drowInst["Vendor Code"] = Convert.ToString(ds.Tables[0].Rows[row]["Vendorcode"]);
                            drowInst["VendorPK"] = Convert.ToString(ds.Tables[0].Rows[row]["VendorPK"]);
                            drowInst["Vendor Name"] = Convert.ToString(ds.Tables[0].Rows[row]["VendorCompName"]);
                            dtVendorReport.Rows.Add(drowInst);
                        }
                        if (dtVendorReport.Columns.Count > 0 && dtVendorReport.Rows.Count > 0)
                        {
                            chkGridSelectAll.Visible = true;
                            divTreeView.Visible = true;
                            GrdStaff.DataSource = dtVendorReport;
                            GrdStaff.DataBind();
                            GrdStaff.Visible = true;
                            lbl_errormsgstaff.Visible = false;

                            GrdStaff.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            GrdStaff.Rows[0].Font.Bold = true;
                            GrdStaff.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (GrdStaff.Rows.Count > 0)
                        {
                            btn_staffOK.Visible = true;
                            btn_exitstaff.Visible = true;
                        }
                        boolCheck = true;
                    }
                }
            }
            if (!boolCheck)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);

        }
        catch (Exception ex) { }
    }

    protected string getSelectedMemName(CheckBoxList cbl, ref  int totSelcount)
    {
        string memName = string.Empty;
        for (int memRow = 0; memRow < cbl.Items.Count; memRow++)
        {
            if (cbl.Items[memRow].Selected)
            {
                memName = Convert.ToString(cbl.Items[memRow].Text);
                totSelcount++;
            }
        }
        return memName;
    }

    protected string getSelMemName(CheckBoxList cbl)
    {
        string memName = string.Empty;
        StringBuilder sbName = new StringBuilder();
        for (int memRow = 0; memRow < cbl.Items.Count; memRow++)
        {
            if (!cbl.Items[memRow].Selected)
                continue;
            sbName.Append(Convert.ToString(cbl.Items[memRow].Text) + ",");

        }
        if (sbName.Length > 0)
        {
            sbName.Remove(sbName.Length - 1, 1);
            memName = Convert.ToString(sbName);
        }
        return memName;
    }

    protected void GrdStaff_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[4].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[4].Visible = false;
            if (e.Row.RowIndex == 0)
            {
                e.Row.Cells[1].Text = "Select";
            }
        }
    }

    #endregion

    protected string getHeaderFK(string hdName, string collegecode)
    {
        string hdFK = string.Empty;
        try
        {
            string[] headerFK = new string[0];
            string selQFK = "  select distinct headerpk from fm_headermaster where collegecode in('" + collegecode + "') and headername in('" + hdName + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref headerFK, headerFK.Length + 1);
                    headerFK[headerFK.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["headerpk"]);
                }
                hdFK = string.Join("','", headerFK);
            }
        }
        catch { hdFK = string.Empty; }
        return hdFK;
    }

    protected string getLedgerFK(string hdName, string collegecode)
    {
        string hdFK = string.Empty;
        try
        {
            string[] headerFK = new string[0];
            string selQFK = "  select distinct ledgerpk from fm_ledgermaster where collegecode in('" + collegecode + "') and ledgername in('" + hdName + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref headerFK, headerFK.Length + 1);
                    headerFK[headerFK.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["ledgerpk"]);
                }
                hdFK = string.Join("','", headerFK);
            }
        }
        catch { hdFK = string.Empty; }
        return hdFK;
    }

    //college spread load 
    protected DataTable loadPaidDetailsLedgerOthers(DataSet dspaid, ref Hashtable htpayMode)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            ArrayList arTranstype = new ArrayList();
            arTranstype.Add("1");
            arTranstype.Add("2");
            arTranstype.Add("3");
            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("Header_Name");
            dtpaid.Columns.Add("Credit");
            dtpaid.Columns.Add("Debit");
            if (dtpaid.Columns.Count > 0)
            {
                DataRow drpaid;
                int rowCnt = 0;
                int firstDs = 0;
                int SeondDs = 1;
                int thirdDs = 2;
                for (int mem = 0; mem < cblmem.Items.Count; mem++)
                {
                    if (!cblmem.Items[mem].Selected)
                        continue;
                    string memText = Convert.ToString(cblmem.Items[mem].Text);
                    Hashtable htSubTot = new Hashtable();
                    Hashtable htpaymode = new Hashtable();
                    for (int ar = 0; ar < arTranstype.Count; ar++)
                    {
                        int TransTypeVal = 0;
                        int.TryParse(Convert.ToString(arTranstype[ar]), out TransTypeVal);
                        for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                        {
                            double tempCredit = 0;
                            double tempDebit = 0;
                            if (chkl_paid.Items[mode].Selected)
                            {
                                string payMode = Convert.ToString(chkl_paid.Items[mode].Value);
                                string payModeText = Convert.ToString(chkl_paid.Items[mode].Text);
                                if (!htpayMode.ContainsKey(chkl_paid.Items[mode].Value))
                                    htpayMode.Add(chkl_paid.Items[mode].Value, chkl_paid.Items[mode].Text);
                                if (payMode.Trim() != "2" && payMode.Trim() != "3")
                                {
                                    #region cash,online,card
                                    dspaid.Tables[firstDs].DefaultView.RowFilter = "paymode='" + payMode + "' and Transtype='" + arTranstype[ar] + "'";
                                    DataView dvpaid = dspaid.Tables[firstDs].DefaultView;
                                    if (dvpaid.Count > 0)
                                    {
                                        string dispText = string.Empty;
                                        string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                        if (transText != "")
                                            dispText = payModeText + "-(" + transText + ")";
                                        else
                                            dispText = payModeText;
                                        drpaid = dtpaid.NewRow();
                                        drpaid["Sno"] = memText + "~" + dispText + "#" + "Mode";
                                        dtpaid.Rows.Add(drpaid);
                                        for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                        {
                                            double hdCredit = 0;
                                            double hdDebit = 0;
                                            if (chkl_studhed.Items[hd].Selected)
                                            {
                                                string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                                DataTable dtbal = dvpaid.ToTable();
                                                dtbal.DefaultView.RowFilter = "headerName='" + hdName + "'";
                                                DataView dvpaids = dtbal.DefaultView;
                                                if (dvpaids.Count > 0)
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    drpaid["Sno"] = hdName + "!" + "Header";
                                                    dtpaid.Rows.Add(drpaid);
                                                    #region
                                                    for (int row = 0; row < dvpaids.Count; row++)
                                                    {
                                                        drpaid = dtpaid.NewRow();
                                                        double credit = 0;
                                                        double debit = 0;
                                                        drpaid["Sno"] = Convert.ToString(rowCnt++);
                                                        drpaid["Header_Name"] = Convert.ToString(dvpaids[row]["Ledgername"]);
                                                        double.TryParse(Convert.ToString(dvpaids[row]["debit"]), out credit);
                                                        double.TryParse(Convert.ToString(dvpaids[row]["credit"]), out debit);
                                                        drpaid["Credit"] = Convert.ToString(credit);
                                                        drpaid["Debit"] = Convert.ToString(debit);
                                                        tempCredit += credit;
                                                        tempDebit += debit;
                                                        hdCredit += credit;
                                                        hdDebit += debit;

                                                        if (TransTypeVal != 3)
                                                        {
                                                            if (!htSubTot.ContainsKey("Credit"))
                                                                htSubTot.Add("Credit", credit);
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                                amount += credit;
                                                                htSubTot.Remove("Credit");
                                                                htSubTot.Add("Credit", Convert.ToString(amount));
                                                            }

                                                            if (!htSubTot.ContainsKey("Debit"))
                                                                htSubTot.Add("Debit", debit);
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                                amount += debit;
                                                                htSubTot.Remove("Debit");
                                                                htSubTot.Add("Debit", Convert.ToString(amount));
                                                            }
                                                            //paymode
                                                            if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                                htpaymode.Add(payModeText + "-" + "CR", credit);
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                                amount += credit;
                                                                htpaymode.Remove(payModeText + "-" + "CR");
                                                                htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                            }
                                                            if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                                htpaymode.Add(payModeText + "-" + "DR", debit);
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                                amount += debit;
                                                                htpaymode.Remove(payModeText + "-" + "DR");
                                                                htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                            }
                                                        }

                                                        dtpaid.Rows.Add(drpaid);
                                                    }
                                                    #endregion
                                                }
                                            }
                                            if (hdCredit != 0 || hdDebit != 0)//every header total
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = "Header Total" + "-" + "*";
                                                drpaid["Credit"] = Convert.ToString(hdCredit);
                                                drpaid["Debit"] = Convert.ToString(hdDebit);
                                                dtpaid.Rows.Add(drpaid);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region dd,cheque
                                    bool boolMode = false;
                                    for (int bkrow = 0; bkrow < dspaid.Tables[thirdDs].Rows.Count; bkrow++)
                                    {
                                        bool boolBank = false;
                                        dspaid.Tables[SeondDs].DefaultView.RowFilter = "paymode='" + payMode + "' and Deposite_BankFK='" + dspaid.Tables[thirdDs].Rows[bkrow]["textcode"] + "' and Transtype='" + arTranstype[ar] + "'";
                                        DataView dvpaid = dspaid.Tables[SeondDs].DefaultView;
                                        if (dvpaid.Count > 0)
                                        {
                                            if (!boolMode)
                                            {
                                                string dispText = string.Empty;
                                                string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                                if (transText != "")
                                                    dispText = payModeText + "-(" + transText + ")";
                                                else
                                                    dispText = payModeText;
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = memText + "~" + dispText + "#" + "Mode";
                                                dtpaid.Rows.Add(drpaid);
                                                boolMode = true;
                                            }
                                            if (!boolBank)
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = Convert.ToString(dspaid.Tables[2].Rows[bkrow]["textval"]) + "$" + "BankName";
                                                dtpaid.Rows.Add(drpaid);
                                                boolBank = true;
                                            }
                                            double indivBankCredit = 0;
                                            double indivBankDebit = 0;
                                            for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                            {
                                                double hdCredit = 0;
                                                double hdDebit = 0;
                                                if (chkl_studhed.Items[hd].Selected)
                                                {
                                                    string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                                    DataTable dtbal = dvpaid.ToTable();
                                                    dtbal.DefaultView.RowFilter = "headerName='" + hdName + "'";
                                                    DataView dvpaids = dtbal.DefaultView;
                                                    if (dvpaids.Count > 0)
                                                    {
                                                        drpaid = dtpaid.NewRow();
                                                        drpaid["Sno"] = hdName + "!" + "Header";
                                                        dtpaid.Rows.Add(drpaid);
                                                        #region
                                                        for (int row = 0; row < dvpaids.Count; row++)
                                                        {
                                                            drpaid = dtpaid.NewRow();
                                                            double credit = 0;
                                                            double debit = 0;
                                                            drpaid["Sno"] = Convert.ToString(rowCnt++);
                                                            drpaid["Header_Name"] = Convert.ToString(dvpaids[row]["Ledgername"]);
                                                            double.TryParse(Convert.ToString(dvpaids[row]["debit"]), out credit);
                                                            double.TryParse(Convert.ToString(dvpaids[row]["credit"]), out debit);
                                                            drpaid["Credit"] = Convert.ToString(credit);
                                                            drpaid["Debit"] = Convert.ToString(debit);
                                                            tempCredit += credit;
                                                            tempDebit += debit;
                                                            hdCredit += credit;
                                                            hdDebit += debit;
                                                            indivBankCredit += credit;
                                                            indivBankDebit += debit;
                                                            if (TransTypeVal != 3)
                                                            {
                                                                if (!htSubTot.ContainsKey("Credit"))
                                                                    htSubTot.Add("Credit", credit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                                    amount += credit;
                                                                    htSubTot.Remove("Credit");
                                                                    htSubTot.Add("Credit", Convert.ToString(amount));
                                                                }

                                                                if (!htSubTot.ContainsKey("Debit"))
                                                                    htSubTot.Add("Debit", debit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                                    amount += debit;
                                                                    htSubTot.Remove("Debit");
                                                                    htSubTot.Add("Debit", Convert.ToString(amount));
                                                                }
                                                                //paymode
                                                                if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                                    htpaymode.Add(payModeText + "-" + "CR", credit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                                    amount += credit;
                                                                    htpaymode.Remove(payModeText + "-" + "CR");
                                                                    htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                                }
                                                                if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                                    htpaymode.Add(payModeText + "-" + "DR", debit);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                                    amount += debit;
                                                                    htpaymode.Remove(payModeText + "-" + "DR");
                                                                    htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                                }
                                                            }

                                                            dtpaid.Rows.Add(drpaid);
                                                        }
                                                        #endregion
                                                    }
                                                }
                                                if (hdCredit != 0 || hdDebit != 0)//every header total
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    drpaid["Sno"] = "Header Total" + "-" + "*";
                                                    drpaid["Credit"] = Convert.ToString(hdCredit);
                                                    drpaid["Debit"] = Convert.ToString(hdDebit);
                                                    dtpaid.Rows.Add(drpaid);
                                                }
                                            }
                                            if (indivBankCredit != 0 || indivBankDebit != 0)//total
                                            {
                                                drpaid = dtpaid.NewRow();
                                                if (TransTypeVal != 3)
                                                    drpaid["Sno"] = "Total" + "-" + "*";
                                                else
                                                    drpaid["Sno"] = "Journal Total" + "-" + "*";
                                                drpaid["Credit"] = Convert.ToString(indivBankCredit);
                                                drpaid["Debit"] = Convert.ToString(indivBankDebit);
                                                dtpaid.Rows.Add(drpaid);
                                            }

                                        }
                                    }
                                    #endregion
                                }
                            }
                            if (tempCredit != 0 || tempDebit != 0)//total
                            {
                                drpaid = dtpaid.NewRow();
                                if (TransTypeVal != 3)
                                    drpaid["Sno"] = "Total" + "-" + "*";
                                else
                                    drpaid["Sno"] = "Journal Total" + "-" + "*";
                                drpaid["Credit"] = Convert.ToString(tempCredit);
                                drpaid["Debit"] = Convert.ToString(tempDebit);
                                dtpaid.Rows.Add(drpaid);
                            }
                        }
                    }
                    if (htpaymode.Count > 0)
                    {
                        #region
                        double fnlmodecredit = 0;
                        double fnlmodedebit = 0;
                        for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                        {
                            if (chkl_paid.Items[mode].Selected)
                            {
                                double modecredit = 0;
                                double modedebit = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = chkl_paid.Items[mode].Text + "-" + "*";
                                double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "CR"]), out modecredit);
                                double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "DR"]), out modedebit);
                                if (modecredit != 0 || modedebit != 0)
                                {
                                    drpaid["Credit"] = Convert.ToString(modecredit);
                                    drpaid["Debit"] = Convert.ToString(modedebit);
                                    dtpaid.Rows.Add(drpaid);
                                    fnlmodecredit += modecredit;
                                    fnlmodedebit += modedebit;
                                }
                            }
                        }
                        if (fnlmodecredit != 0 || fnlmodedebit != 0)
                        {
                            drpaid = dtpaid.NewRow();
                            drpaid["Sno"] = "Paymode Total" + "-" + "*";
                            drpaid["Credit"] = Convert.ToString(fnlmodecredit);
                            drpaid["Debit"] = Convert.ToString(fnlmodedebit);
                            dtpaid.Rows.Add(drpaid);
                        }
                        #endregion
                    }
                    if (htSubTot.Count > 0)
                    {
                        #region
                        //final receipt and payment amount
                        double rcptAmt = 0;
                        double payAmt = 0;
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Receipt" + "-" + "*";
                        double.TryParse(Convert.ToString(htSubTot["Credit"]), out rcptAmt);
                        drpaid["Credit"] = Convert.ToString(rcptAmt);
                        dtpaid.Rows.Add(drpaid);

                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Payment" + "-" + "*";
                        double.TryParse(Convert.ToString(htSubTot["Debit"]), out payAmt);
                        drpaid["Debit"] = Convert.ToString(payAmt);
                        dtpaid.Rows.Add(drpaid);
                        //balance 
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Balance" + "-" + "*";
                        drpaid["Credit"] = Convert.ToString(rcptAmt - payAmt);
                        dtpaid.Rows.Add(drpaid);
                        #endregion
                    }
                    firstDs += 3;//dataset count increased here based on memttype
                    SeondDs += 3;
                    thirdDs += 3;
                }
            }
        }
        catch { dtpaid.Clear(); }
        return dtpaid;
    }

    protected DataTable loadPaidDetailsOthers(DataSet dspaid, ref Hashtable htpayMode)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            ArrayList arTranstype = new ArrayList();
            arTranstype.Add("1");
            arTranstype.Add("2");
            arTranstype.Add("3");
            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("Header_Name");
            dtpaid.Columns.Add("Credit");
            dtpaid.Columns.Add("Debit");
            DataRow drpaid;
            int rowCnt = 0;
            if (dtpaid.Columns.Count > 0)
            {
                int firstDs = 0;
                int SeondDs = 1;
                int thirdDs = 2;
                for (int mem = 0; mem < cblmem.Items.Count; mem++)
                {
                    if (!cblmem.Items[mem].Selected)
                        continue;
                    string memText = Convert.ToString(cblmem.Items[mem].Text);
                    Hashtable htSubTot = new Hashtable();
                    Hashtable htpaymode = new Hashtable();
                    for (int ar = 0; ar < arTranstype.Count; ar++)
                    {
                        int TransTypeVal = 0;
                        int.TryParse(Convert.ToString(arTranstype[ar]), out TransTypeVal);
                        for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                        {
                            double tempCredit = 0;
                            double tempDebit = 0;
                            if (!chkl_paid.Items[mode].Selected)
                                continue;
                            string payMode = Convert.ToString(chkl_paid.Items[mode].Value);
                            string payModeText = Convert.ToString(chkl_paid.Items[mode].Text);
                            if (!htpayMode.ContainsKey(chkl_paid.Items[mode].Value))
                                htpayMode.Add(chkl_paid.Items[mode].Value, chkl_paid.Items[mode].Text);
                            if (payMode.Trim() != "2" && payMode.Trim() != "3")
                            {
                                #region cash,online,card

                                dspaid.Tables[firstDs].DefaultView.RowFilter = "paymode='" + payMode + "' and Transtype='" + TransTypeVal + "'";
                                DataView dvpaid = dspaid.Tables[firstDs].DefaultView;
                                if (dvpaid.Count > 0)
                                {
                                    string dispText = string.Empty;
                                    string transText = TransTypeVal == 3 ? "Journal Entry" : "";
                                    if (transText != "")
                                        dispText = payModeText + "-(" + transText + ")";
                                    else
                                        dispText = payModeText;
                                    drpaid = dtpaid.NewRow();
                                    drpaid["Sno"] = memText + "~" + dispText + "#" + "Mode";
                                    dtpaid.Rows.Add(drpaid);
                                    for (int row = 0; row < dvpaid.Count; row++)
                                    {
                                        drpaid = dtpaid.NewRow();
                                        double credit = 0;
                                        double debit = 0;
                                        drpaid["Sno"] = Convert.ToString(rowCnt++);
                                        drpaid["Header_Name"] = Convert.ToString(dvpaid[row]["headerName"]);
                                        double.TryParse(Convert.ToString(dvpaid[row]["debit"]), out credit);
                                        double.TryParse(Convert.ToString(dvpaid[row]["credit"]), out debit);
                                        drpaid["Credit"] = Convert.ToString(credit);
                                        drpaid["Debit"] = Convert.ToString(debit);
                                        tempCredit += credit;
                                        tempDebit += debit;

                                        if (TransTypeVal != 3)
                                        {
                                            if (!htSubTot.ContainsKey("Credit"))
                                                htSubTot.Add("Credit", credit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                amount += credit;
                                                htSubTot.Remove("Credit");
                                                htSubTot.Add("Credit", Convert.ToString(amount));
                                            }

                                            if (!htSubTot.ContainsKey("Debit"))
                                                htSubTot.Add("Debit", debit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                amount += debit;
                                                htSubTot.Remove("Debit");
                                                htSubTot.Add("Debit", Convert.ToString(amount));
                                            }
                                            //paymode
                                            if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                htpaymode.Add(payModeText + "-" + "CR", credit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                amount += credit;
                                                htpaymode.Remove(payModeText + "-" + "CR");
                                                htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                            }
                                            if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                htpaymode.Add(payModeText + "-" + "DR", debit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                amount += debit;
                                                htpaymode.Remove(payModeText + "-" + "DR");
                                                htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                            }
                                        }

                                        dtpaid.Rows.Add(drpaid);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region dd,cheque
                                bool boolMode = false;
                                for (int bkrow = 0; bkrow < dspaid.Tables[thirdDs].Rows.Count; bkrow++)
                                {
                                    bool boolBank = false;
                                    dspaid.Tables[SeondDs].DefaultView.RowFilter = "paymode='" + payMode + "' and Deposite_BankFK='" + dspaid.Tables[thirdDs].Rows[bkrow]["textcode"] + "' and Transtype='" + arTranstype[ar] + "'";
                                    DataView dvpaid = dspaid.Tables[SeondDs].DefaultView;
                                    if (dvpaid.Count > 0)
                                    {
                                        if (!boolMode)
                                        {
                                            string dispText = string.Empty;
                                            string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                            if (transText != "")
                                                dispText = payModeText + "-(" + transText + ")";
                                            else
                                                dispText = payModeText;
                                            drpaid = dtpaid.NewRow();
                                            drpaid["Sno"] = memText + "~" + dispText + "#" + "Mode";
                                            dtpaid.Rows.Add(drpaid);
                                            boolMode = true;
                                        }
                                        if (!boolBank)
                                        {
                                            drpaid = dtpaid.NewRow();
                                            drpaid["Sno"] = Convert.ToString(dspaid.Tables[2].Rows[bkrow]["textval"]) + "$" + "BankName";
                                            dtpaid.Rows.Add(drpaid);
                                            boolBank = true;
                                        }
                                        #region
                                        double indivBankCredit = 0;
                                        double indivBankDebit = 0;
                                        for (int row = 0; row < dvpaid.Count; row++)
                                        {
                                            drpaid = dtpaid.NewRow();
                                            double credit = 0;
                                            double debit = 0;
                                            drpaid["Sno"] = Convert.ToString(rowCnt++);
                                            drpaid["Header_Name"] = Convert.ToString(dvpaid[row]["headerName"]);
                                            double.TryParse(Convert.ToString(dvpaid[row]["debit"]), out credit);
                                            double.TryParse(Convert.ToString(dvpaid[row]["credit"]), out debit);
                                            drpaid["Credit"] = Convert.ToString(credit);
                                            drpaid["Debit"] = Convert.ToString(debit);
                                            tempCredit += credit;
                                            tempDebit += debit;
                                            indivBankCredit += credit;
                                            indivBankDebit += debit;

                                            if (TransTypeVal != 3)
                                            {
                                                if (!htSubTot.ContainsKey("Credit"))
                                                    htSubTot.Add("Credit", credit);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htSubTot["Credit"]), out amount);
                                                    amount += credit;
                                                    htSubTot.Remove("Credit");
                                                    htSubTot.Add("Credit", Convert.ToString(amount));
                                                }

                                                if (!htSubTot.ContainsKey("Debit"))
                                                    htSubTot.Add("Debit", debit);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htSubTot["Debit"]), out amount);
                                                    amount += debit;
                                                    htSubTot.Remove("Debit");
                                                    htSubTot.Add("Debit", Convert.ToString(amount));
                                                }
                                                //paymode
                                                if (!htpaymode.ContainsKey(payModeText + "-" + "CR"))
                                                    htpaymode.Add(payModeText + "-" + "CR", credit);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "CR"]), out amount);
                                                    amount += credit;
                                                    htpaymode.Remove(payModeText + "-" + "CR");
                                                    htpaymode.Add(payModeText + "-" + "CR", Convert.ToString(amount));
                                                }
                                                if (!htpaymode.ContainsKey(payModeText + "-" + "DR"))
                                                    htpaymode.Add(payModeText + "-" + "DR", debit);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htpaymode[payModeText + "-" + "DR"]), out amount);
                                                    amount += debit;
                                                    htpaymode.Remove(payModeText + "-" + "DR");
                                                    htpaymode.Add(payModeText + "-" + "DR", Convert.ToString(amount));
                                                }
                                            }

                                            dtpaid.Rows.Add(drpaid);
                                        }
                                        if (indivBankCredit != 0 || indivBankDebit != 0)//individual bankwise total
                                        {
                                            drpaid = dtpaid.NewRow();
                                            if (TransTypeVal != 3)
                                                drpaid["Sno"] = "Total" + "-" + "*";
                                            else
                                                drpaid["Sno"] = "Journal Total" + "-" + "*";
                                            drpaid["Credit"] = Convert.ToString(indivBankCredit);
                                            drpaid["Debit"] = Convert.ToString(indivBankDebit);
                                            dtpaid.Rows.Add(drpaid);
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                            if (tempCredit != 0 || tempDebit != 0)//total
                            {
                                drpaid = dtpaid.NewRow();
                                if (TransTypeVal != 3)
                                    drpaid["Sno"] = "Total" + "-" + "*";
                                else
                                    drpaid["Sno"] = "Journal Total" + "-" + "*";
                                drpaid["Credit"] = Convert.ToString(tempCredit);
                                drpaid["Debit"] = Convert.ToString(tempDebit);
                                dtpaid.Rows.Add(drpaid);
                            }
                        }
                    }
                    if (htpaymode.Count > 0)
                    {
                        #region
                        double fnlmodecredit = 0;
                        double fnlmodedebit = 0;
                        for (int mode = 0; mode < chkl_paid.Items.Count; mode++)
                        {
                            if (chkl_paid.Items[mode].Selected)
                            {
                                double modecredit = 0;
                                double modedebit = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = chkl_paid.Items[mode].Text + "-" + "*";
                                double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "CR"]), out modecredit);
                                double.TryParse(Convert.ToString(htpaymode[chkl_paid.Items[mode].Text + "-" + "DR"]), out modedebit);
                                if (modecredit != 0 || modedebit != 0)
                                {
                                    drpaid["Credit"] = Convert.ToString(modecredit);
                                    drpaid["Debit"] = Convert.ToString(modedebit);
                                    dtpaid.Rows.Add(drpaid);
                                    fnlmodecredit += modecredit;
                                    fnlmodedebit += modedebit;
                                }
                            }
                        }
                        if (fnlmodecredit != 0 || fnlmodedebit != 0)
                        {
                            drpaid = dtpaid.NewRow();
                            drpaid["Sno"] = "Paymode Total" + "-" + "*";
                            drpaid["Credit"] = Convert.ToString(fnlmodecredit);
                            drpaid["Debit"] = Convert.ToString(fnlmodedebit);
                            dtpaid.Rows.Add(drpaid);
                        }
                        #endregion
                    }
                    if (htSubTot.Count > 0)
                    {
                        #region
                        //final receipt and payment amount
                        double rcptAmt = 0;
                        double payAmt = 0;
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Receipt" + "-" + "*";
                        double.TryParse(Convert.ToString(htSubTot["Credit"]), out rcptAmt);
                        drpaid["Credit"] = Convert.ToString(rcptAmt);
                        dtpaid.Rows.Add(drpaid);

                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Payment" + "-" + "*";
                        double.TryParse(Convert.ToString(htSubTot["Debit"]), out payAmt);
                        drpaid["Debit"] = Convert.ToString(payAmt);
                        dtpaid.Rows.Add(drpaid);
                        //balance 
                        drpaid = dtpaid.NewRow();
                        drpaid["Sno"] = "Balance" + "-" + "*";
                        drpaid["Credit"] = Convert.ToString(rcptAmt - payAmt);
                        dtpaid.Rows.Add(drpaid);
                        #endregion
                    }
                    firstDs += 3;//dataset count increased here based on memttype
                    SeondDs += 3;
                    thirdDs += 3;
                }
            }
        }
        catch { dtpaid.Clear(); }
        return dtpaid;
    }

    protected void getOLd()
    {
        #region MyRegion

        //if (!cbDate.Checked)
        //{
        //    #region
        //    SelQ = " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,paymode,isnull(f.transtype,'0') as transtype" + selFinYr + " from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,registration r where f.app_no=r.app_no and h.headerpk=f.headerfk   and h.collegecode in('" + collegecode + "') and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and  l.ledgername in('" + ldText + "') " + strInclude + " " + incJournal + " " + finlYrStr + " group by " + strhdNameGroup + ",paymode,f.transtype" + selFinYrGrpBy + " " + strOrderBy + "";//and isnull(f.debit,'0')>0
        //    //only dd,check
        //    SelQ += " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype" + selFinYr + "  from ft_findailytransaction f,fm_headermaster h ,FM_LedgerMaster l ,registration r where f.app_no=r.app_no  and h.headerpk=f.headerfk  and h.collegecode in('" + collegecode + "') and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and  l.ledgername in('" + ldText + "') " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') and isnull(deposite_bankfk,'0')<>'0' " + incJournal + "  " + finlYrStr + " group by " + strhdNameGroup + ",paymode,Deposite_BankFK,f.transtype" + selFinYrGrpBy + " " + strOrderBy + "";//and isnull(f.debit,'0')>0
        //    //distinct bank name                     
        //    SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,registration r,FM_FinBankMaster bk where f.app_no=r.app_no and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk   and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK and  l.ledgername in('" + ldText + "') " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') and isnull(deposite_bankfk,'0')<>'0' " + finlYrStr + " " + incJournal + " ";
        //    //and r.college_code=bk.collegecode and h.collegecode in('" + collegecode + "')
        //    #endregion
        //}
        //else
        //{
        //    #region
        //    SelQ = " select convert(varchar(10),transdate,103) as headerName ,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,paymode,isnull(f.transtype,'0') as transtype" + selFinYr + " from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,registration r where f.app_no=r.app_no and h.headerpk=f.headerfk   and h.collegecode in('" + collegecode + "') and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and  l.ledgername in('" + ldText + "') " + strInclude + " " + incJournal + "  " + finlYrStr + " group by transdate,paymode, transtype" + selFinYrGrpBy + " order by paymode,transdate";//and isnull(f.debit,'0')>0
        //    //only dd,check
        //    SelQ += " select convert(varchar(10),transdate,103) as headerName,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,paymode,Deposite_BankFK,isnull(f.transtype,'0') as transtype" + selFinYr + " from ft_findailytransaction f,fm_headermaster h ,FM_LedgerMaster l ,registration r where f.app_no=r.app_no  and h.headerpk=f.headerfk  and h.collegecode in('" + collegecode + "') and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and  l.ledgername in('" + ldText + "') " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') and isnull(deposite_bankfk,'0')<>'0' " + incJournal + " " + finlYrStr + "  group by transdate,paymode,Deposite_BankFK, transtype " + selFinYrGrpBy + " order by paymode,transdate";//and isnull(f.debit,'0')>0
        //    //distinct bank name                    
        //    SelQ += "   select  distinct (accno+'-'+bankname) as TextVal,bankpk as TextCode from ft_findailytransaction f,fm_headermaster h,FT_FinBankTransaction fb,FM_LedgerMaster l ,registration r,FM_FinBankMaster bk where f.app_no=r.app_no and h.headerpk=f.headerfk  and f.transcode=fb.dailytransid and f.paymode=fb.paymode and f.entryusercode=fb.entryusercode and fb.bankfk=bk.bankpk  and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and f.transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' and l.LedgerPK =f.LedgerFK and l.HeaderFK =h.HeaderPK and  l.ledgername in('" + ldText + "') " + strInclude + " and f.Deposite_BankFK in('" + bankFk + "') and isnull(deposite_bankfk,'0')<>'0' " + incJournal + " " + finlYrStr + " ";//and r.college_code=bk.collegecode and h.collegecode in('" + collegecode + "')
        //    //and isnull(f.debit,'0')>0                   
        //    #endregion
        //}
        #endregion
    }

    public void getAcademicYear()
    {
        try
        {
            string fnalyr = "";
            // string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
            string getfinanceyear = "SELECT distinct ACD_YEAR FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD WHERE  AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK  AND  ACD_COLLEGE_CODE IN('" + collegecode + "') order by ACD_YEAR desc";
            ds.Dispose();
            ds.Reset();
            ddlAcademic.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["ACD_YEAR"].ToString();
                    ddlAcademic.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected Dictionary<string, string> getOldSettings(string acdYears)
    {
        Dictionary<string, string> htAcademic = new Dictionary<string, string>();
        try
        {
            string settingType = string.Empty;
            if (rblTypeNew.SelectedIndex == 0)
                settingType = "0";
            else if (rblTypeNew.SelectedIndex == 1)
                settingType = "1";
            else if (rblTypeNew.SelectedIndex == 2)
                settingType = "2";
            string collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string selQ = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
            DataSet dsPrevAMount = d2.select_method_wo_parameter(selQ, "Text");
            if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
            {
                DataTable dtAcdYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_COLLEGE_CODE", "collname");
                DataTable dtBatchYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_BATCH_YEAR", "ACD_COLLEGE_CODE");
                DataTable dtFeecat = dsPrevAMount.Tables[0].DefaultView.ToTable();
                if (dtAcdYear.Rows.Count > 0)
                {
                    int Sno = 0;
                    for (int row = 0; row < dtAcdYear.Rows.Count; row++)
                    {
                        Sno++;
                        string acdYear = Convert.ToString(dtAcdYear.Rows[row]["ACD_YEAR"]);
                        string clgCode = Convert.ToString(dtAcdYear.Rows[row]["ACD_COLLEGE_CODE"]);
                        dtBatchYear.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                        DataTable dtBatch = dtBatchYear.DefaultView.ToTable();
                        if (dtBatch.Rows.Count > 0)
                        {
                            for (int bat = 0; bat < dtBatch.Rows.Count; bat++)
                            {
                                string acdBatchYear = Convert.ToString(dtBatch.Rows[bat]["ACD_BATCH_YEAR"]);
                                dtFeecat.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_BATCH_YEAR='" + acdBatchYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                                DataTable dtFee = dtFeecat.DefaultView.ToTable();
                                if (dtFee.Rows.Count > 0)
                                {
                                    StringBuilder sbSem = new StringBuilder();
                                    StringBuilder sbSemStr = new StringBuilder();
                                    for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                                    {
                                        string feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                                        string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                                        sbSem.Append(feecaT + ",");
                                        // sbSemStr.Append(feecaTStr + ",");
                                    }
                                    if (sbSem.Length > 0)
                                        sbSem.Remove(sbSem.Length - 1, 1);
                                    if (!htAcademic.ContainsKey(clgCode + "$" + acdBatchYear))
                                        htAcademic.Add(clgCode + "$" + acdBatchYear, Convert.ToString(sbSem));
                                    //if (sbSemStr.Length > 0)
                                    //    sbSemStr.Remove(sbSemStr.Length - 1, 1);                              
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return htAcademic;

    }

    #region Added by saranya on 12March2018 for Monthwise abstract report

    public void cbAcdYear_OnCheckedChanged(object ob, EventArgs e)
    {


    }

    public void cbMonth_OnCheckedChanged(object ob, EventArgs e)
    {
        if (cbMonth.Checked == true)
        {
            rblMemType.Items[1].Enabled = false;
        }
        if (cbMonth.Checked == true && cbAcdYear.Checked == true)
        {
            cbIncOthers.Checked = false;
        }
    }

    public void loadSpreadMonthwise()
    {

        #region GridDetails design
        dicColumnVisible.Clear();
        dicColumnAlignment.Clear();
        DataTable dtMonthWiseReport = new DataTable();
        DataRow drowInst = null;
        arrColHdrNames.Add("S.No");
        dtMonthWiseReport.Columns.Add("col0");
        arrColHdrNames.Add(rblmode.SelectedItem.Text);
        dtMonthWiseReport.Columns.Add("col1");

        int MonthVal = 0;
        string year = string.Empty;
        int rowCnt = 0;
        int ColValue = 1;
        for (int MonthYear = 0; MonthYear < ds.Tables[2].Rows.Count; MonthYear++)
        {
            ColValue++;
            string month = string.Empty;
            MonthVal = Convert.ToInt32(ds.Tables[2].Rows[MonthYear]["month"]);
            year = Convert.ToString(ds.Tables[2].Rows[MonthYear]["year"]);
            string yearVal = string.Empty;
            switch (MonthVal)
            {
                case 1:
                    month = "Jan";
                    break;
                case 2:
                    month = "Feb";
                    break;
                case 3:
                    month = "Mar";
                    break;
                case 4:
                    month = "Apr";
                    break;
                case 5:
                    month = "May";
                    break;
                case 6:
                    month = "Jun";
                    break;
                case 7:
                    month = "Jul";
                    break;
                case 8:
                    month = "Aug";
                    break;
                case 9:
                    month = "Sep";
                    break;
                case 10:
                    month = "Oct";
                    break;
                case 11:
                    month = "Nov";
                    break;
                case 12:
                    month = "Dec";
                    break;
            }
            yearVal = year.Substring(2, 2);

            arrColHdrNames.Add(month + "'" + yearVal);
            dtMonthWiseReport.Columns.Add(Convert.ToString("col" + ColValue));
            dicColumnAlignment.Add(ColValue, "Col");
            ColValue++;
            arrColHdrNames.Add(MonthVal + "$" + year);
            dtMonthWiseReport.Columns.Add(Convert.ToString("col" + ColValue));
            dicColumnVisible.Add(ColValue, "Col");
        }
        ColValue++;
        arrColHdrNames.Add("Total");
        dtMonthWiseReport.Columns.Add(Convert.ToString("col" + ColValue));
        dicColumnAlignment.Add(ColValue, "Col");
        DataRow drHdr1 = dtMonthWiseReport.NewRow();
        for (int grCol = 0; grCol < dtMonthWiseReport.Columns.Count; grCol++)
        {
            drHdr1["col" + grCol] = arrColHdrNames[grCol];
        }
        dtMonthWiseReport.Rows.Add(drHdr1);

        #endregion

        if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0 || ds.Tables[3].Rows.Count > 0))
        {
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();
            DataTable dtpaid = new DataTable();
            double schollSet = checkSchoolSetting();
            string MonthYearTot = string.Empty;

            if (cbAcdYear.Checked)
            {
                Dictionary<string, string> getAcdYear = new Dictionary<string, string>();

                #region Academic Year
                DataSet dsNornaml = ds.Copy();
                DataSet dsFinal = new DataSet();
                try
                {
                    string clgCode = Convert.ToString(getCblSelectedValue(cblclg));
                    string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                    getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);

                    if (getAcdYear.Count > 0)
                    {
                        bool boolDs = false;
                        foreach (KeyValuePair<string, string> getVal in getAcdYear)
                        {
                            string feeCate = getVal.Value.Replace(",", "','");
                            if (checkSchoolSetting() != 0)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";////abarna 8.03
                            }
                            else
                            {
                                ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "'  and feecategory in('" + feeCate + "')";//and batch_year='" + getVal.Key.Split('$')[1] + "'//abarna 8.03
                            }
                            DataTable dtFirst = ds.Tables[0].DefaultView.ToTable();


                            if (!boolDs)
                            {
                                dsFinal.Reset();
                                dsFinal.Tables.Add(dtFirst);
                                boolDs = true;
                            }
                            else
                            {
                                dsFinal.Merge(dtFirst);

                            }
                        }
                    }
                    ds.Reset();
                    if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                    {
                        string flTName = string.Empty;
                        string flThName = string.Empty;
                        if (rblmode.SelectedIndex == 0)
                        {
                            flTName = "headerName";
                        }
                        else
                        {
                            flTName = "headerName";
                            flThName = "ledgername";
                        }
                        DataTable dtPertbl = new DataTable();
                        if (rblmode.SelectedIndex == 0)
                        {
                            DataTable dtColumns = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, "actualfinyearfk");//change by abarna 22.1.2018
                            DataTable tempTbl = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, "debit", "credit", "actualfinyearfk");//change by abarna 22.1.2018
                            dtPertbl = tempTbl.DefaultView.ToTable();
                            dtPertbl.Rows.Clear();
                            foreach (DataRow drRow in dtColumns.Rows)
                            {
                                tempTbl.DefaultView.RowFilter = "headername='" + drRow[flTName] + "' and actualfinyearfk='" + drRow["actualfinyearfk"] + "'";//change by abarna 22.1.2018
                                DataRow drPer = dtPertbl.NewRow();
                                drPer[flTName] = drRow[flTName];
                                drPer["debit"] = tempTbl.DefaultView.ToTable().Compute("SUM(debit)", "");
                                drPer["credit"] = tempTbl.DefaultView.ToTable().Compute("SUM(credit)", "");

                                drPer["actualfinyearfk"] = drRow["actualfinyearfk"];//change by abarna 22.1.2018
                                dtPertbl.Rows.Add(drPer);
                            }
                        }
                        else
                        {
                            DataTable dtColumns = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, flThName, "actualfinyearfk");//change by abarna 22.1.2018
                            DataTable tempTbl = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, flThName, "debit", "credit", "actualfinyearfk");//change by abarna 22.1.2018
                            dtPertbl = tempTbl.DefaultView.ToTable();
                            dtPertbl.Rows.Clear();
                            foreach (DataRow drRow in dtColumns.Rows)
                            {
                                tempTbl.DefaultView.RowFilter = "headername='" + drRow[flTName] + "' and ledgername='" + drRow[flThName] + "' and actualfinyearfk='" + drRow["actualfinyearfk"] + "'";//change by abarna 22.1.2018
                                DataRow drPer = dtPertbl.NewRow();
                                drPer[flTName] = drRow[flTName];
                                drPer[flThName] = drRow[flThName];
                                drPer["debit"] = tempTbl.DefaultView.ToTable().Compute("SUM(debit)", "");
                                drPer["credit"] = tempTbl.DefaultView.ToTable().Compute("SUM(credit)", "");
                                drPer["actualfinyearfk"] = drRow["actualfinyearfk"];//change by abarna 22.1.2018
                                dtPertbl.Rows.Add(drPer);
                            }
                        }
                        ds.Reset();
                        ds.Tables.Add(dtPertbl);
                        //if (cbIncOthers.Checked)
                        //{
                        //    ds.Tables.Add(dsNornaml.Tables[1].DefaultView.ToTable());
                        //    ds.Tables.Add(dsNornaml.Tables[2].DefaultView.ToTable());
                        //    ds.Tables.Add(dsNornaml.Tables[3].DefaultView.ToTable());
                        //}

                    }

                }
                catch
                {
                    ds.Reset();
                    ds = dsNornaml.Copy();
                }
                #endregion

                if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                {
                    #region Header wise for Academic year
                    if (rblmode.SelectedIndex == 0)
                    {
                        double finalTot = 0;
                        string headername = string.Empty;
                        string Month = string.Empty;
                        string Year = string.Empty;
                        double HeaderWiseTot = 0;
                        for (int headCnt = 0; headCnt < chkl_studhed.Items.Count; headCnt++)
                        {
                            int col = 1;
                            double MonthHashTot = 0;
                            if (chkl_studhed.Items[headCnt].Selected == true)
                            {
                                headername = chkl_studhed.Items[headCnt].Value.ToString();
                                drowInst = dtMonthWiseReport.NewRow();
                                drowInst[0] = Convert.ToString(++rowCnt);
                                drowInst[1] = Convert.ToString(headername);

                                for (int monYear = 0; monYear < ds.Tables[2].Rows.Count; monYear++)
                                {
                                    col++;
                                    double monthTotal = 0;
                                    Month = Convert.ToString(ds.Tables[2].Rows[monYear]["month"]);
                                    Year = Convert.ToString(ds.Tables[2].Rows[monYear]["year"]);
                                    if (dsFinal.Tables[0].Rows.Count > 0)
                                    {
                                        dsFinal.Tables[0].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and headerName ='" + headername + "'";
                                        DataView dvHeaderwiseStu = dsFinal.Tables[0].DefaultView;
                                        for (int k = 0; k < dvHeaderwiseStu.Count; k++)
                                        {
                                            double stuTotal = 0;
                                            MonthHashTot = 0;
                                            stuTotal = Convert.ToDouble(dvHeaderwiseStu[k]["debit"]);
                                            monthTotal += stuTotal;
                                            MonthHashTot += stuTotal;
                                            HeaderWiseTot += stuTotal;
                                            //MonthYearTot = headername + "#" + Month + "$" + Year;
                                            //MonthYearTot = Month + "$" + Year;
                                            MonthYearTot = "col" + col;
                                            if (!htTotal.ContainsKey(MonthYearTot))
                                                htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                amount += MonthHashTot;
                                                htTotal.Remove(MonthYearTot);
                                                htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                            }
                                        }
                                        if (dvHeaderwiseStu.Count == 0)
                                        {
                                            MonthHashTot = 0;
                                            //MonthYearTot = Month + "$" + Year;
                                            MonthYearTot = "col" + col;
                                            if (!htTotal.ContainsKey(MonthYearTot))
                                                htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                amount += MonthHashTot;
                                                htTotal.Remove(MonthYearTot);
                                                htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                            }
                                        }
                                    }
                                    drowInst[col] = monthTotal;
                                    col++;
                                }
                                col++;
                                drowInst[col] = Convert.ToString(HeaderWiseTot);
                                finalTot += HeaderWiseTot;
                                HeaderWiseTot = 0;
                                dtMonthWiseReport.Rows.Add(drowInst);
                            }
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            drowInst = dtMonthWiseReport.NewRow();
                            drowInst[0] = Convert.ToString(++rowCnt);
                            drowInst[1] = "Advance Amount (Excess Amount)";
                            int ColCountFinal = 1;
                            double ExcessHashTot = 0;
                            for (int monYear = 0; monYear < ds.Tables[2].Rows.Count; monYear++)
                            {
                                ColCountFinal++;
                                Month = Convert.ToString(ds.Tables[2].Rows[monYear]["month"]);
                                Year = Convert.ToString(ds.Tables[2].Rows[monYear]["year"]);
                                ds.Tables[1].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "'";
                                DataView dvHeaderwiseExcess = ds.Tables[1].DefaultView;
                                double monthTotal = 0;
                                if (dvHeaderwiseExcess.Count > 0)
                                {
                                    for (int k = 0; k < dvHeaderwiseExcess.Count; k++)
                                    {
                                        double stuTotal = 0;
                                        stuTotal = Convert.ToDouble(dvHeaderwiseExcess[k]["debit"]);
                                        monthTotal += stuTotal;
                                        ExcessHashTot += stuTotal;
                                        //MonthYearTot = headername + "#" + Month + "$" + Year;
                                        //MonthYearTot = Month + "$" + Year;
                                        MonthYearTot = "col" + ColCountFinal;
                                        if (!htTotal.ContainsKey(MonthYearTot))
                                            htTotal.Add(MonthYearTot, Convert.ToString(monthTotal));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                            amount += monthTotal;
                                            htTotal.Remove(MonthYearTot);
                                            htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                        }
                                        drowInst[ColCountFinal] = monthTotal;
                                        ColCountFinal++;
                                    }
                                }
                                else
                                {
                                    drowInst[ColCountFinal] = "0";
                                    ColCountFinal++;
                                }
                            }
                            ColCountFinal++;
                            drowInst[ColCountFinal] = Convert.ToString(ExcessHashTot);
                            finalTot += ExcessHashTot;
                            dtMonthWiseReport.Rows.Add(drowInst);
                        }

                        //Grand total
                        drowInst = dtMonthWiseReport.NewRow();
                        drowInst[0] = "GrandTotal";
                        int FinalTotCol = 0;
                        for (int i = 2; i < dtMonthWiseReport.Columns.Count; i++)
                        {
                            string colName = Convert.ToString(dtMonthWiseReport.Columns[i]);
                            if (htTotal.Contains(colName))
                            {
                                drowInst[i] = Convert.ToString(htTotal[colName]);
                                FinalTotCol = i;
                            }
                        }
                        drowInst[FinalTotCol + 2] = Convert.ToString(finalTot);
                        dtMonthWiseReport.Rows.Add(drowInst);
                    }
                    #endregion

                    #region Ledger Wise for academic year
                    if (rblmode.SelectedIndex == 1)
                    {
                        double finalTot = 0;
                        string ledgername = string.Empty;
                        string Month = string.Empty;
                        string Year = string.Empty;
                        double HeaderWiseTot = 0;
                        for (int ledCnt = 0; ledCnt < chkl_studled.Items.Count; ledCnt++)
                        {
                            double MonthHashTot = 0;
                            int col = 1;

                            if (chkl_studled.Items[ledCnt].Selected == true)
                            {
                                ledgername = chkl_studled.Items[ledCnt].Value.ToString();
                                drowInst = dtMonthWiseReport.NewRow();
                                drowInst[0] = Convert.ToString(++rowCnt);
                                drowInst[1] = Convert.ToString(ledgername);
                                for (int monYear = 0; monYear < ds.Tables[2].Rows.Count; monYear++)
                                {
                                    col++;
                                    double monthTotal = 0;
                                    Month = Convert.ToString(ds.Tables[2].Rows[monYear]["month"]);
                                    Year = Convert.ToString(ds.Tables[2].Rows[monYear]["year"]);
                                    //for (int i = 0; i < dsFinal.Tables[0].Rows.Count; i++)
                                    if (dsFinal.Tables[0].Rows.Count > 0)
                                    {
                                        dsFinal.Tables[0].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and ledgername ='" + ledgername + "'";
                                        DataView dvHeaderwiseStu = dsFinal.Tables[0].DefaultView;
                                        for (int k = 0; k < dvHeaderwiseStu.Count; k++)
                                        {
                                            double stuTotal = 0;
                                            MonthHashTot = 0;
                                            stuTotal = Convert.ToDouble(dvHeaderwiseStu[k]["debit"]);
                                            monthTotal += stuTotal;
                                            MonthHashTot += stuTotal;
                                            HeaderWiseTot += stuTotal;
                                            //MonthYearTot = headername + "#" + Month + "$" + Year;
                                            //MonthYearTot = Month + "$" + Year;
                                            MonthYearTot = "col" + col;
                                            if (!htTotal.ContainsKey(MonthYearTot))
                                                htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                amount += MonthHashTot;
                                                htTotal.Remove(MonthYearTot);
                                                htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                            }
                                        }
                                        if (dvHeaderwiseStu.Count == 0)
                                        {
                                            MonthHashTot = 0;
                                            //MonthYearTot = Month + "$" + Year;
                                            MonthYearTot = "col" + col;
                                            if (!htTotal.ContainsKey(MonthYearTot))
                                                htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                amount += MonthHashTot;
                                                htTotal.Remove(MonthYearTot);
                                                htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                            }
                                        }
                                    }
                                    drowInst[col] = Convert.ToString(monthTotal);
                                    col++;
                                }
                                col++;
                                drowInst[col] = Convert.ToString(HeaderWiseTot);
                                finalTot += HeaderWiseTot;
                                HeaderWiseTot = 0;
                                dtMonthWiseReport.Rows.Add(drowInst);
                            }
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            drowInst = dtMonthWiseReport.NewRow();
                            drowInst[0] = Convert.ToString(++rowCnt);
                            drowInst[1] = "Advance Amount (Excess Amount)";
                            int ColCountFinal = 1;
                            double ExcessHashTot = 0;
                            for (int monYear = 0; monYear < ds.Tables[2].Rows.Count; monYear++)
                            {
                                ColCountFinal++;
                                Month = Convert.ToString(ds.Tables[2].Rows[monYear]["month"]);
                                Year = Convert.ToString(ds.Tables[2].Rows[monYear]["year"]);
                                ds.Tables[1].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "'";
                                DataView dvLedgerwiseExcess = ds.Tables[1].DefaultView;
                                double monthTotal = 0;
                                if (dvLedgerwiseExcess.Count > 0)
                                {
                                    for (int k = 0; k < dvLedgerwiseExcess.Count; k++)
                                    {
                                        double stuTotal = 0;
                                        stuTotal = Convert.ToDouble(dvLedgerwiseExcess[k]["debit"]);
                                        monthTotal += stuTotal;
                                        ExcessHashTot += stuTotal;
                                        //MonthYearTot = headername + "#" + Month + "$" + Year;
                                        //MonthYearTot = Month + "$" + Year;
                                        MonthYearTot = "col" + ColCountFinal;
                                        if (!htTotal.ContainsKey(MonthYearTot))
                                            htTotal.Add(MonthYearTot, Convert.ToString(monthTotal));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                            amount += monthTotal;
                                            htTotal.Remove(MonthYearTot);
                                            htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                        }

                                        drowInst[ColCountFinal] = Convert.ToString(monthTotal);
                                        ColCountFinal++;
                                    }
                                }
                                else
                                {
                                    drowInst[ColCountFinal] = "0";
                                    ColCountFinal++;
                                }
                            }
                            ColCountFinal++;
                            drowInst[ColCountFinal] = Convert.ToString(ExcessHashTot);
                            finalTot += ExcessHashTot;
                            dtMonthWiseReport.Rows.Add(drowInst);
                        }
                        //Grand total
                        drowInst = dtMonthWiseReport.NewRow();
                        drowInst[0] = "GrandTotal";
                        int FinalTotCol = 0;
                        for (int i = 2; i < dtMonthWiseReport.Columns.Count; i++)
                        {
                            string colName = Convert.ToString(dtMonthWiseReport.Columns[i]);
                            if (htTotal.Contains(colName))
                            {
                                drowInst[i] = Convert.ToString(htTotal[colName]);
                                FinalTotCol = i;
                            }
                        }
                        drowInst[FinalTotCol + 2] = Convert.ToString(finalTot);
                        dtMonthWiseReport.Rows.Add(drowInst);
                    }
                    #endregion

                }
            }

            if (!cbAcdYear.Checked)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    #region Headerwise

                    if (rblmode.SelectedIndex == 0)
                    {
                        double finalTot = 0;
                        string headername = string.Empty;
                        string Month = string.Empty;
                        string Year = string.Empty;
                        double HeaderWiseTot = 0;
                        for (int headCnt = 0; headCnt < chkl_studhed.Items.Count; headCnt++)
                        {
                            int col = 1;
                            double MonthHashTot = 0;
                            if (chkl_studhed.Items[headCnt].Selected == true)
                            {
                                headername = chkl_studhed.Items[headCnt].Value.ToString();
                                drowInst = dtMonthWiseReport.NewRow();
                                drowInst[0] = Convert.ToString(++rowCnt);
                                drowInst[1] = Convert.ToString(headername);
                                for (int monYear = 0; monYear < ds.Tables[2].Rows.Count; monYear++)
                                {
                                    col++;
                                    double monthTotal = 0;
                                    MonthHashTot = 0;
                                    Month = Convert.ToString(ds.Tables[2].Rows[monYear]["month"]);
                                    Year = Convert.ToString(ds.Tables[2].Rows[monYear]["year"]);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and headerName ='" + headername + "'";
                                        DataView dvHeaderwiseStu = ds.Tables[0].DefaultView;
                                        for (int k = 0; k < dvHeaderwiseStu.Count; k++)
                                        {
                                            double stuTotal = 0;
                                            stuTotal = Convert.ToDouble(dvHeaderwiseStu[k]["debit"]);
                                            monthTotal += stuTotal;
                                            MonthHashTot += stuTotal;
                                            HeaderWiseTot += stuTotal;
                                            //MonthYearTot = headername + "#" + Month + "$" + Year;
                                            //MonthYearTot = Month + "$" + Year;
                                            MonthYearTot = "col" + col;
                                            if (!htTotal.ContainsKey(MonthYearTot))
                                                htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                amount += MonthHashTot;
                                                htTotal.Remove(MonthYearTot);
                                                htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                            }
                                        }
                                        if (dvHeaderwiseStu.Count == 0)
                                        {
                                            MonthHashTot = 0;
                                            //MonthYearTot = Month + "$" + Year;
                                            MonthYearTot = "col" + col;
                                            if (!htTotal.ContainsKey(MonthYearTot))
                                                htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                amount += MonthHashTot;
                                                htTotal.Remove(MonthYearTot);
                                                htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                            }
                                        }
                                    }
                                    if (cbIncOthers.Checked)
                                    {
                                        if (ds.Tables[3].Rows.Count > 0)
                                        {
                                            ds.Tables[3].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and headerName ='" + headername + "'";
                                            DataView dvHeaderwiseStaff = ds.Tables[3].DefaultView;
                                            for (int k = 0; k < dvHeaderwiseStaff.Count; k++)
                                            {
                                                double stuTotal = 0;
                                                MonthHashTot = 0;
                                                stuTotal = Convert.ToDouble(dvHeaderwiseStaff[k]["debit"]);
                                                monthTotal += stuTotal;
                                                MonthHashTot += stuTotal;
                                                HeaderWiseTot += stuTotal;
                                                //MonthYearTot = headername + "#" + Month + "$" + Year;
                                                //MonthYearTot = Month + "$" + Year;
                                                MonthYearTot = "col" + col;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                            if (dvHeaderwiseStaff.Count == 0)
                                            {
                                                MonthHashTot = 0;
                                                // MonthYearTot = Month + "$" + Year;
                                                MonthYearTot = "col" + col;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                        }
                                        if (ds.Tables[4].Rows.Count > 0)
                                        {
                                            ds.Tables[4].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and headerName ='" + headername + "'";
                                            DataView dvHeaderwiseVendor = ds.Tables[4].DefaultView;
                                            for (int k = 0; k < dvHeaderwiseVendor.Count; k++)
                                            {
                                                double stuTotal = 0;
                                                MonthHashTot = 0;
                                                stuTotal = Convert.ToDouble(dvHeaderwiseVendor[k]["debit"]);
                                                monthTotal += stuTotal;
                                                MonthHashTot += stuTotal;
                                                HeaderWiseTot += stuTotal;
                                                //MonthYearTot = headername + "#" + Month + "$" + Year;
                                                //MonthYearTot = Month + "$" + Year;
                                                MonthYearTot = "col" + col;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                            if (dvHeaderwiseVendor.Count == 0)
                                            {
                                                MonthHashTot = 0;
                                                //MonthYearTot = Month + "$" + Year;
                                                MonthYearTot = "col" + col;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                        }
                                        if (ds.Tables[5].Rows.Count > 0)
                                        {
                                            ds.Tables[5].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and headerName ='" + headername + "'";
                                            DataView dvHeaderwiseOther = ds.Tables[5].DefaultView;
                                            for (int k = 0; k < dvHeaderwiseOther.Count; k++)
                                            {
                                                double stuTotal = 0;
                                                MonthHashTot = 0;
                                                stuTotal = Convert.ToDouble(dvHeaderwiseOther[k]["debit"]);
                                                monthTotal += stuTotal;
                                                MonthHashTot += stuTotal;
                                                HeaderWiseTot += stuTotal;
                                                //MonthYearTot = headername + "#" + Month + "$" + Year;
                                                //MonthYearTot = Month + "$" + Year;
                                                MonthYearTot = "col" + col;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                            if (dvHeaderwiseOther.Count == 0)
                                            {
                                                MonthHashTot = 0;
                                                //MonthYearTot = Month + "$" + Year;
                                                MonthYearTot = "col" + col;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                        }
                                    }
                                    drowInst[col] = Convert.ToString(monthTotal);
                                    col++;
                                }
                                col++;
                                drowInst[col] = Convert.ToString(HeaderWiseTot);
                                finalTot += HeaderWiseTot;
                                HeaderWiseTot = 0;
                                dtMonthWiseReport.Rows.Add(drowInst);
                            }
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            drowInst = dtMonthWiseReport.NewRow();
                            drowInst[0] = Convert.ToString(++rowCnt);
                            drowInst[1] = "Advance Amount (Excess Amount)";
                            int ColCountFinal = 1;
                            double ExcessHashTot = 0;
                            for (int monYear = 0; monYear < ds.Tables[2].Rows.Count; monYear++)
                            {
                                ColCountFinal++;
                                Month = Convert.ToString(ds.Tables[2].Rows[monYear]["month"]);
                                Year = Convert.ToString(ds.Tables[2].Rows[monYear]["year"]);
                                ds.Tables[1].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "'";
                                DataView dvHeaderwiseExcess = ds.Tables[1].DefaultView;
                                double monthTotal = 0;

                                if (dvHeaderwiseExcess.Count > 0)
                                {
                                    for (int k = 0; k < dvHeaderwiseExcess.Count; k++)
                                    {
                                        double stuTotal = 0;
                                        stuTotal = Convert.ToDouble(dvHeaderwiseExcess[k]["debit"]);
                                        monthTotal += stuTotal;
                                        ExcessHashTot += stuTotal;
                                        //MonthYearTot = headername + "#" + Month + "$" + Year;
                                        //MonthYearTot = Month + "$" + Year;
                                        MonthYearTot = "col" + ColCountFinal;
                                        if (!htTotal.ContainsKey(MonthYearTot))
                                            htTotal.Add(MonthYearTot, Convert.ToString(monthTotal));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                            amount += monthTotal;
                                            htTotal.Remove(MonthYearTot);
                                            htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                        }
                                        drowInst[ColCountFinal] = Convert.ToString(monthTotal);
                                        ColCountFinal++;
                                    }
                                }
                                else
                                {
                                    drowInst[ColCountFinal] = "0";
                                    ColCountFinal++;
                                }
                            }
                            ColCountFinal++;
                            drowInst[ColCountFinal] = Convert.ToString(ExcessHashTot);
                            finalTot += ExcessHashTot;
                            dtMonthWiseReport.Rows.Add(drowInst);
                        }
                        //Grand total
                        drowInst = dtMonthWiseReport.NewRow();
                        drowInst[0] = "GrandTotal";
                        int FinalTotCol = 0;
                        for (int i = 2; i < dtMonthWiseReport.Columns.Count; i++)
                        {
                            string colName = Convert.ToString(dtMonthWiseReport.Columns[i]);
                            if (htTotal.Contains(colName))
                            {
                                drowInst[i] = Convert.ToString(htTotal[colName]);
                                FinalTotCol = i;
                            }
                        }
                        drowInst[FinalTotCol + 2] = Convert.ToString(finalTot);
                        dtMonthWiseReport.Rows.Add(drowInst);
                    }
                    #endregion

                    #region ledger wise
                    if (rblmode.SelectedIndex == 1)
                    {
                        string Month = string.Empty;
                        string Year = string.Empty;
                        string ledgername = string.Empty;
                        double finalTot = 0;
                        double HeaderWiseTot = 0;
                        for (int ledCnt = 0; ledCnt < chkl_studled.Items.Count; ledCnt++)
                        {
                            int col = 1;
                            double MonthHashTot = 0;
                            if (chkl_studled.Items[ledCnt].Selected == true)
                            {
                                ledgername = chkl_studled.Items[ledCnt].Value.ToString();
                                drowInst = dtMonthWiseReport.NewRow();
                                drowInst[0] = Convert.ToString(++rowCnt);
                                drowInst[1] = Convert.ToString(ledgername);

                                for (int monYear = 0; monYear < ds.Tables[2].Rows.Count; monYear++)
                                {
                                    col++;
                                    double monthTotal = 0;
                                    MonthHashTot = 0;
                                    Month = Convert.ToString(ds.Tables[2].Rows[monYear]["month"]);
                                    Year = Convert.ToString(ds.Tables[2].Rows[monYear]["year"]);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and ledgername ='" + ledgername + "'";
                                        DataView dvHeaderwiseStu = ds.Tables[0].DefaultView;
                                        for (int k = 0; k < dvHeaderwiseStu.Count; k++)
                                        {
                                            double stuTotal = 0;
                                            stuTotal = Convert.ToDouble(dvHeaderwiseStu[k]["debit"]);
                                            monthTotal += stuTotal;
                                            MonthHashTot += stuTotal;
                                            HeaderWiseTot += stuTotal;
                                            //MonthYearTot = Month + "$" + Year;
                                            MonthYearTot = "col" + col;
                                            if (!htTotal.ContainsKey(MonthYearTot))
                                                htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                amount += MonthHashTot;
                                                htTotal.Remove(MonthYearTot);
                                                htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                            }
                                        }
                                        if (dvHeaderwiseStu.Count == 0)
                                        {
                                            MonthHashTot = 0;
                                            MonthYearTot = "col" + col;
                                            //MonthYearTot = Month + "$" + Year;
                                            if (!htTotal.ContainsKey(MonthYearTot))
                                                htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                amount += MonthHashTot;
                                                htTotal.Remove(MonthYearTot);
                                                htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                            }
                                        }

                                    }
                                    if (cbIncOthers.Checked)
                                    {
                                        if (ds.Tables[3].Rows.Count > 0)
                                        {
                                            ds.Tables[3].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and ledgername ='" + ledgername + "'";
                                            DataView dvHeaderwiseStaff = ds.Tables[3].DefaultView;
                                            for (int k = 0; k < dvHeaderwiseStaff.Count; k++)
                                            {
                                                double stuTotal = 0;
                                                MonthHashTot = 0;
                                                stuTotal = Convert.ToDouble(dvHeaderwiseStaff[k]["debit"]);
                                                monthTotal += stuTotal;
                                                MonthHashTot += stuTotal;
                                                HeaderWiseTot += stuTotal;
                                                MonthYearTot = "col" + col;
                                                //MonthYearTot = Month + "$" + Year;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                            if (dvHeaderwiseStaff.Count == 0)
                                            {
                                                MonthHashTot = 0;
                                                MonthYearTot = "col" + col;
                                                //MonthYearTot = Month + "$" + Year;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                        }
                                        if (ds.Tables[4].Rows.Count > 0)
                                        {
                                            ds.Tables[4].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and ledgername ='" + ledgername + "'";
                                            DataView dvHeaderwiseVendor = ds.Tables[4].DefaultView;
                                            for (int k = 0; k < dvHeaderwiseVendor.Count; k++)
                                            {
                                                double stuTotal = 0;
                                                MonthHashTot = 0;
                                                stuTotal = Convert.ToDouble(dvHeaderwiseVendor[k]["debit"]);
                                                monthTotal += stuTotal;
                                                MonthHashTot += stuTotal;
                                                HeaderWiseTot += stuTotal;
                                                //MonthYearTot = headername + "#" + Month + "$" + Year;
                                                //MonthYearTot = Month + "$" + Year;
                                                MonthYearTot = "col" + col;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                            if (dvHeaderwiseVendor.Count == 0)
                                            {
                                                MonthHashTot = 0;
                                                //MonthYearTot = Month + "$" + Year;
                                                MonthYearTot = "col" + col;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                        }
                                        if (ds.Tables[5].Rows.Count > 0)
                                        {
                                            ds.Tables[5].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "' and ledgername ='" + ledgername + "'";
                                            DataView dvHeaderwiseOther = ds.Tables[5].DefaultView;
                                            for (int k = 0; k < dvHeaderwiseOther.Count; k++)
                                            {
                                                double stuTotal = 0;
                                                MonthHashTot = 0;
                                                stuTotal = Convert.ToDouble(dvHeaderwiseOther[k]["debit"]);
                                                monthTotal += stuTotal;
                                                MonthHashTot += stuTotal;
                                                HeaderWiseTot += stuTotal;
                                                //MonthYearTot = headername + "#" + Month + "$" + Year;
                                                //MonthYearTot = Month + "$" + Year;
                                                MonthYearTot = "col" + col;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                            if (dvHeaderwiseOther.Count == 0)
                                            {
                                                MonthHashTot = 0;
                                                MonthYearTot = "col" + col;
                                                //MonthYearTot = Month + "$" + Year;
                                                if (!htTotal.ContainsKey(MonthYearTot))
                                                    htTotal.Add(MonthYearTot, Convert.ToString(MonthHashTot));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                                    amount += MonthHashTot;
                                                    htTotal.Remove(MonthYearTot);
                                                    htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                                }
                                            }
                                        }
                                    }
                                    drowInst[col] = Convert.ToString(monthTotal);
                                    col++;
                                }
                                col++;
                                drowInst[col] = Convert.ToString(HeaderWiseTot);
                                finalTot += HeaderWiseTot;
                                HeaderWiseTot = 0;
                                dtMonthWiseReport.Rows.Add(drowInst);
                            }
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            drowInst = dtMonthWiseReport.NewRow();
                            drowInst[0] = Convert.ToString(++rowCnt);
                            drowInst[1] = "Advance Amount (Excess Amount)";
                            int ColCountFinal = 1;
                            double ExcessHashTot = 0;
                            for (int monYear = 0; monYear < ds.Tables[2].Rows.Count; monYear++)
                            {
                                ColCountFinal++;
                                Month = Convert.ToString(ds.Tables[2].Rows[monYear]["month"]);
                                Year = Convert.ToString(ds.Tables[2].Rows[monYear]["year"]);
                                ds.Tables[1].DefaultView.RowFilter = "month='" + Month + "' and year='" + Year + "'";
                                DataView dvHeaderwiseExcess = ds.Tables[1].DefaultView;
                                double monthTotal = 0;
                                if (dvHeaderwiseExcess.Count > 0)
                                {
                                    for (int k = 0; k < dvHeaderwiseExcess.Count; k++)
                                    {
                                        double stuTotal = 0;
                                        stuTotal = Convert.ToDouble(dvHeaderwiseExcess[k]["debit"]);
                                        monthTotal += stuTotal;
                                        ExcessHashTot += stuTotal;
                                        //MonthYearTot = headername + "#" + Month + "$" + Year;
                                        //MonthYearTot = Month + "$" + Year;
                                        MonthYearTot = "col" + ColCountFinal;
                                        if (!htTotal.ContainsKey(MonthYearTot))
                                            htTotal.Add(MonthYearTot, Convert.ToString(monthTotal));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[MonthYearTot]), out amount);
                                            amount += monthTotal;
                                            htTotal.Remove(MonthYearTot);
                                            htTotal.Add(MonthYearTot, Convert.ToString(amount));
                                        }
                                        drowInst[ColCountFinal] = Convert.ToString(monthTotal);
                                        ColCountFinal++;
                                    }
                                }
                                else
                                {
                                    drowInst[ColCountFinal] = "0";
                                    ColCountFinal++;
                                }
                            }
                            ColCountFinal++;
                            drowInst[ColCountFinal] = Convert.ToString(ExcessHashTot);
                            finalTot += ExcessHashTot;
                            dtMonthWiseReport.Rows.Add(drowInst);
                        }
                        //Grand total
                        drowInst = dtMonthWiseReport.NewRow();
                        drowInst[0] = "GrandTotal";
                        int FinalTotCol = 0;
                        for (int i = 2; i < dtMonthWiseReport.Columns.Count; i++)
                        {
                            string colName = Convert.ToString(dtMonthWiseReport.Columns[i]);
                            if (htTotal.Contains(colName))
                            {
                                drowInst[i] = Convert.ToString(htTotal[colName]);
                                FinalTotCol = i;
                            }
                        }
                        drowInst[FinalTotCol + 2] = Convert.ToString(finalTot);
                        dtMonthWiseReport.Rows.Add(drowInst);
                    }
                    #endregion

                }
            }
        }
        GrdMonthwise.DataSource = dtMonthWiseReport;
        GrdMonthwise.DataBind();
        GrdMonthwise.Visible = true;
        foreach (GridViewRow gvrow in GrdMonthwise.Rows)
        {
            int RowCnt = Convert.ToInt32(gvrow.RowIndex);
            string sNoVal = Convert.ToString(GrdMonthwise.Rows[RowCnt].Cells[0].Text);
            if (Convert.ToString(sNoVal).All(char.IsNumber))
            {
            }
            else
            {
                if (sNoVal == "GrandTotal")
                    GrdMonthwise.Rows[RowCnt].BackColor = Color.Green;
            }
        }
        // lblvalidation1.Text = "";
        // div1.Visible = true;
        txtexcelname.Text = "";
        grdHeaderWiseCollection.Visible = false;
        print.Visible = true;
    }

    protected void GrdMonthwise_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            foreach (KeyValuePair<int, string> dr in dicColumnVisible)
            {
                int rowcnt = dr.Key;
                e.Row.Cells[rowcnt].Visible = false;
            }
            foreach (KeyValuePair<int, string> dr in dicColumnAlignment)
            {
                int rowcnt = dr.Key;
                e.Row.Cells[rowcnt].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.BackColor = Color.FromArgb(12, 166, 202);
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Width = 200;
                e.Row.Font.Bold = true;
            }

            foreach (KeyValuePair<int, string> dr in dicColumnVisible)
            {
                int rowcnt = dr.Key;
                e.Row.Cells[rowcnt].Visible = false;
            }
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            foreach (KeyValuePair<int, string> dr in dicColumnAlignment)
            {
                int rowcnt = dr.Key;
                e.Row.Cells[rowcnt].HorizontalAlign = HorizontalAlign.Right;
            }
        }
    }

    #endregion

    protected void grdHeaderWiseCollection_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.BackColor = Color.FromArgb(12, 166, 202);
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Width = 200;
                e.Row.Font.Bold = true;
            }
            else
            {
                e.Row.Cells[0].Width = 50;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            }
        }
    }
}