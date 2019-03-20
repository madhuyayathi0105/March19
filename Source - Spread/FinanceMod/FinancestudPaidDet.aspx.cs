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

public partial class FinancestudPaidDet : System.Web.UI.Page
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
    Dictionary<int, string> dicInstWise = new Dictionary<int, string>();
    int grdRow = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["collegecode"] == null)
        //    Response.Redirect("~/Default.aspx");
        try
        {
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
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

                setLabelText();
                bindCollege();
                if (cblclg.Items.Count > 0)
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                rblMemType_Selected(sender, e);
                //bindheader();
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
            //divcolorder.Attributes.Add("Style", "display:none;");
        }
        catch
        {
        }
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
        //bindheader();
        //loadpaid();
        //loadfinanceUser();
        //columnType();
    }

    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        rblMemType_Selected(sender, e);
        //bindheader();
        //loadpaid();
        //loadfinanceUser();
        //columnType();
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
                bindledger();
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
                string query = " select distinct ledgername from FM_LedgerMaster l,FM_HeaderMaster h,FS_LedgerPrivilage P where l.HeaderFK =h.HeaderPK   and L.LedgerPK = P.LedgerFK and l.CollegeCode in('" + collegecode + "' ) and h.HeaderName in('" + headercode + "' )";
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
            chk_paid.Checked = false;
            txt_paid.Text = "--Select--";
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

    #region finuser year

    public void loadfinanceUser()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string finUser = "   select user_id,user_code from usermaster where fin_user='1' ";//and college_code in('" + collegecode + "')
            string getfin = d2.GetFunction("select fin_user from usermaster where fin_user='1' and user_code='" + usercode.Trim() + "' ");//and college_code in('" + collegecode + "')
            cbuser.Checked = false;
            cbluser.Items.Clear();
            txtuser.Text = "--Select--";
            ds = d2.select_method_wo_parameter(finUser, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbluser.DataSource = ds;
                cbluser.DataTextField = "user_id";
                cbluser.DataValueField = "user_code";
                cbluser.DataBind();
                int cnt = 0;
                string getSngName = string.Empty;
                if (getfin.Trim() == "1")
                {
                    for (int i = 0; i < cbluser.Items.Count; i++)
                    {
                        //cbluser.Items[i].Selected = true;
                        if (cbluser.Items[i].Value.Trim() == usercode.Trim())
                        {
                            cbluser.Items[i].Selected = true;
                            getSngName = cbluser.Items[i].Text;
                            cnt++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cbluser.Items.Count; i++)
                    {
                        cbluser.Items[i].Selected = true;
                        cnt++;
                    }
                }
                if (cbluser.Items.Count == cnt)
                {
                    txtuser.Text = lbluser.Text + "(" + cbluser.Items.Count + ")";
                    cbuser.Checked = true;
                }
                else
                {
                    if (cnt == 1)
                        txtuser.Text = getSngName;
                    else
                        txtuser.Text = lbluser.Text + "(" + cnt + ")";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbuser_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(cbuser, cbluser, txtuser, lbluser.Text, "--Select--");

    }

    protected void cbluser_selected(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbuser, cbluser, txtuser, lbluser.Text, "--Select--");
    }

    #endregion

    protected DataSet loadDetails(string selectCol, ref string groupStr)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            UserbasedRights();
            string hdText = string.Empty;
            string payMode = string.Empty;
            string ldText = string.Empty;
            string strInclude = string.Empty;
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            hdText = Convert.ToString(getCblSelectedText(chkl_studhed));
            ldText = Convert.ToString(getCblSelectedText(chkl_studled));
            payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            string finUser = Convert.ToString(getCblSelectedValue(cbluser));
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();


            // string strReg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            int TransVal = 0;


            //string strInclude = getStudCategory();
            if (cblinclude.Items.Count > 0)
            {
                strInclude = getStudCategory(ref TransVal);
            }
            //string strInclude = getStudCategory(ref TransVal);
            #endregion

            if (selectCol.Contains("headerfk") || selectCol.Contains("ledgerfk"))
            {
                if (selectCol.Contains("headerfk"))
                    selectCol = selectCol.Replace(",headerfk", ",(select distinct headername from fm_headermaster h where h.headerpk=f.headerfk) as headerfk");
                else
                    selectCol = selectCol.Replace(",ledgerfk", ",(select distinct ledgername from fm_ledgermaster h where h.headerfk=f.headerfk and h.ledgerpk=f.ledgerfk) as headerfk");

                if (selectCol.Contains("headerfk"))
                    selectCol += ",headerfk";
                else
                    selectCol += ",ledgerfk";
            }

            string selCol = "f.paymode," + selectCol + ",f.app_no,isnull(f.transtype,'0') as transtype";
            string GrpselCol = "f.paymode,headerfk," + groupStr + ",f.app_no,f.transtype";
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode) && !string.IsNullOrEmpty(finUser))
            {
                #region Query
                string hdFK = getHeaderFK(hdText, collegecode);
                string ldFK = getLedgerFK(ldText, collegecode);
                string incJournal = string.Empty;
                if (cbJournal.Checked)
                    incJournal = " and isnull(f.transtype,'0')='3'";
                string SelQ = string.Empty;
                string finlYrStr = string.Empty;
                string selFinYr = string.Empty;
                string selFinYrEx = string.Empty;
                if (checkSchoolSetting() == 0)//school
                {
                    #region
                    selFinYr = " ,(select convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)) from fm_finyearmaster where finyearpk=isnull(actualfinyearfk,'0'))as actualfinyearfk";
                    selFinYrEx = " ,''actualfinyearfk";
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
                    selCol = "f.paymode," + selectCol + ",f.app_no,isnull(f.transtype,'0') as transtype" + selFinYr + "";
                    GrpselCol = "f.paymode," + groupStr + ",headerfk,f.app_no,f.transtype,actualfinyearfk";
                    #endregion
                }
                if (rblMemType.SelectedIndex == 0)
                {
                    string acdYear = "";
                    string acdYearGp = "";
                    if (!cbAcdYear.Checked)
                    {

                        SelQ = " select distinct " + selCol + ",isnull(f.receipttype,'0') as receipttype from registration r,ft_findailytransaction f where f.app_no=r.app_no and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and f.entryusercode in('" + finUser + "') and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " " + incJournal + " " + finlYrStr + "   group by " + GrpselCol + ",receipttype ";//and r.app_no='7323' 
                    }
                    else
                    {
                        acdYear = ",r.college_code,r.batch_year,f.feecategory";
                        acdYearGp = ",r.college_code,r.batch_year,f.feecategory";
                        SelQ = " select distinct " + selCol + ",r.college_code,r.batch_year,f.feecategory from registration r,ft_findailytransaction f where f.app_no=r.app_no and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and f.entryusercode in('" + finUser + "') and isnull(iscanceled,'0')='0' and transcode<>'' " + strInclude + " " + incJournal + " " + finlYrStr + "     group by " + GrpselCol + ",r.college_code,r.batch_year,f.feecategory ";
                        // SelQ += "  order by Transcode";//and r.app_no='7323'


                    }
                    //if (selectCol.Contains("Transcode"))
                    //    SelQ += " order by Transcode";
                    string hdStr = string.Empty;
                    if (selectCol.Contains("headerfk"))
                        hdStr = ",exl.headerfk";
                    else
                        hdStr = ",exl.ledgerfk";

                    if (selectCol.Contains("headerfk") || selectCol.Contains("ledgerfk"))
                    {
                        if (selectCol.Contains("headerfk") && selectCol.Contains("(select distinct headername from fm_headermaster h where h.headerpk=f.headerfk) as headerfk"))
                            selectCol = selectCol.Replace(",(select distinct headername from fm_headermaster h where h.headerpk=f.headerfk) as headerfk", ",(select distinct headername+'-'+'(Excess/Advance)' from fm_headermaster h where h.headerpk=f.headerfk) as headerfk");
                        else
                            selectCol = selectCol.Replace(",(select distinct ledgername from fm_ledgermaster h where h.headerfk=f.headerfk and h.ledgerpk=f.ledgerfk) as headerfk", ",(select distinct ledgername+'-'+'(Excess/Advance)' from fm_ledgermaster h where h.headerfk=f.headerfk and h.ledgerpk=f.ledgerfk ) as headerfk");
                    }
                    if (selectCol.Contains("Transcode"))
                        selectCol = selectCol.Replace(",Transcode", ",dailyTranscode");
                    if (selectCol.Contains("convert(varchar(10),transdate,103)as transdate"))
                        selectCol = selectCol.Replace(",convert(varchar(10),transdate,103)as transdate", ",convert(varchar(10),excesstransdate,103)as transdate");
                    if (selectCol.Contains("sum(debit) as debit"))
                        selectCol = selectCol.Replace(",sum(debit) as debit", ",sum(f.excessamt) as debit");
                    if (selectCol.Contains("sum(credit) as credit"))
                        selectCol = selectCol.Replace(",sum(credit) as credit", ",'0' as credit");
                    if (selectCol.Contains("feecategory"))
                        selectCol = selectCol.Replace(",feecategory", ",'' as feecategory");//added by abarna 21.02.2018
                    if (selectCol.Contains("narration"))
                        selectCol = selectCol.Replace(",narration", ",'' as narration");//added by abarna 21.02.2018
                    //last column headerfk and ledgerfk added here
                    if (selectCol.Contains("headerfk") || selectCol.Contains("ledgerfk"))
                    {
                        if (selectCol.Contains("headerfk"))
                            selectCol = selectCol.Replace(",headerfk", ",f.headerfk");
                        else
                            selectCol = selectCol.Replace(",ledgerfk", ",f.ledgerfk");
                    }
                    //group by columns
                    if (groupStr.Contains("headerfk") || groupStr.Contains("ledgerfk"))
                    {
                        if (groupStr.Contains("headerfk"))
                            groupStr = groupStr.Replace(",headerfk", ",f.headerfk,headerfk");
                        else
                            groupStr = groupStr.Replace(",ledgerfk", ",f.ledgerfk,headerfk");
                    }
                    if (groupStr.Contains("Transcode"))
                        groupStr = groupStr.Replace(",Transcode", ",dailyTranscode");
                    if (groupStr.Contains("transdate"))
                        groupStr = groupStr.Replace(",transdate", ",excesstransdate");
                    if (groupStr.Contains("feecategory") || groupStr.Contains("narration"))
                    {

                        groupStr = groupStr.Replace(",feecategory,narration", "");
                        groupStr = groupStr.Replace(",feecategory", "");
                        groupStr = groupStr.Replace(",narration", "");
                    }




                    selCol = " Ex_paymode as paymode ," + selectCol + ",ex.app_no,'1'transtype,'3'receipttype " + selFinYrEx + "";

                    GrpselCol = " Ex_paymode," + groupStr + ",ex.app_no";
                    //union  all
                    SelQ += " union all  select " + selCol + " " + acdYear + " from ft_excessdet ex,ft_excessledgerdet f,registration r where ex.app_no=r.app_no and ex.excessdetpk=f.excessdetfk and ex.feecategory=f.feecategory and ex.excesstransdate between '" + fromdate + "' and '" + todate + "' and memtype='1' and r.college_code in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and excesstransdate between '" + fromdate + "' and '" + todate + "' group by " + GrpselCol + " " + acdYearGp + "";

                    if (selectCol.Contains("Transcode"))
                        SelQ += " order by Transcode";
                    if (!cbJournal.Checked && cbIncOthers.Checked)
                    {
                        #region staff,vendor,others

                        int checkVal = 1;
                        selectCol = getSelectedColumn(ref groupStr, checkVal);
                        selCol = "f.paymode," + selectCol + ",f.app_no,isnull(f.transtype,'0') as transtype";
                        GrpselCol = "f.paymode," + groupStr + ",f.app_no,f.transtype";
                        if (checkSchoolSetting() == 0)//school
                        {
                            selCol = "f.paymode," + selectCol + ",f.app_no,isnull(f.transtype,'0') as transtype" + selFinYr + "";
                            GrpselCol = "f.paymode," + groupStr + ",f.app_no,f.transtype,actualfinyearfk";
                        }
                        //  if ((totSelcount != 1 && selectedName.Contains("Staff")) || (totSelcount == 1 && memName == "Staff"))
                        //  {
                        SelQ += " select distinct " + selCol + " from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no  and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')  and f.entryusercode in('" + finUser + "') and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2'  " + finlYrStr + "";//and sm.college_code in('" + collegecode + "')
                        //  if (string.IsNullOrEmpty(strMemtypeValue))
                        SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + GrpselCol + "";
                        SelQ += "  order by Transcode";
                        //  }

                        //vendor details
                        if (selCol.Contains("sm.staff_name"))
                            selCol = selCol.Replace(",sm.staff_name", ",vendorname as staff_name");
                        if (selCol.Contains("sm.staff_code"))
                            selCol = selCol.Replace(",sm.staff_code", ",VendorCode as staff_code");
                        // if (selCol.Contains("sm.staff_code"))
                        //  selCol = selCol.Replace(",sm.staff_code", ",VendorCode as staff_code");
                        if (selCol.Contains("st.dept_code"))
                            selCol = selCol.Replace(",st.dept_code", ",VendorCompName as dept_code");
                        //group by 
                        if (GrpselCol.Contains("sm.staff_name"))
                            GrpselCol = GrpselCol.Replace(",sm.staff_name", ",vendorname");
                        if (GrpselCol.Contains("sm.staff_code"))
                            GrpselCol = GrpselCol.Replace(",sm.staff_code", ",VendorCode");
                        if (GrpselCol.Contains("st.dept_code"))
                            GrpselCol = GrpselCol.Replace(",st.dept_code", ",VendorCompName");
                        //  if ((totSelcount != 1 && selectedName.Contains("Vendor")) || (totSelcount == 1 && memName == "Vendor"))
                        //  {
                        SelQ += " select distinct " + selCol + " from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')  and f.entryusercode in('" + finUser + "') and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3' " + finlYrStr + " ";
                        //  if (string.IsNullOrEmpty(strMemtypeValue))
                        SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + GrpselCol + "";
                        SelQ += "  order by Transcode";
                        //   }
                        //if ((totSelcount != 1 && selectedName.Contains("Others")) || (totSelcount == 1 && memName == "Others"))
                        //{
                        //other details
                        SelQ += " select distinct " + selCol + " from CO_VendorMaster vm,ft_findailytransaction f where  VendorType ='-5' and vm.vendorpk=f.app_no and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')  and f.entryusercode in('" + finUser + "') and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4' " + finlYrStr + " ";
                        // if (string.IsNullOrEmpty(strMemtypeValue))
                        SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + GrpselCol + "";
                        SelQ += "  order by Transcode";
                        //  }
                        #endregion
                    }
                    // }

                }
                else
                {
                    #region Others
                    // staff details
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
                        SelQ += " select distinct " + selCol + " from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and sm.college_code in('" + collegecode + "') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')  and f.entryusercode in('" + finUser + "') and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2' " + strMemtypeValue + " ";
                        if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + GrpselCol + "";
                        SelQ += "  order by Transcode";
                    }

                    //vendor details
                    if (selCol.Contains("sm.staff_name"))
                        selCol = selCol.Replace(",sm.staff_name", ",vendorname as staff_name");
                    if (selCol.Contains("sm.staff_code"))
                        selCol = selCol.Replace(",sm.staff_code", ",VendorCode as staff_code");
                    if (selCol.Contains("st.dept_code"))
                        selCol = selCol.Replace(",st.dept_code", ",VendorCompName as dept_code");
                    //group by 
                    if (GrpselCol.Contains("sm.staff_name"))
                        GrpselCol = GrpselCol.Replace(",sm.staff_name", ",vendorname");
                    if (GrpselCol.Contains("sm.staff_code"))
                        GrpselCol = GrpselCol.Replace(",sm.staff_code", ",VendorCode");
                    if (GrpselCol.Contains("st.dept_code"))
                        GrpselCol = GrpselCol.Replace(",st.dept_code", ",VendorCompName");
                    if ((totSelcount != 1 && selectedName.Contains("Vendor")) || (totSelcount == 1 && memName == "Vendor"))
                    {
                        SelQ += " select distinct " + selCol + " from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')  and f.entryusercode in('" + finUser + "') and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3' " + strMemtypeValue + " ";
                        if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + GrpselCol + "";
                        SelQ += "  order by Transcode";
                    }
                    if ((totSelcount != 1 && selectedName.Contains("Others")) || (totSelcount == 1 && memName == "Others"))
                    {
                        //other details
                        SelQ += " select distinct " + selCol + " from CO_VendorMaster vm,ft_findailytransaction f where  VendorType ='-5' and vm.vendorpk=f.app_no and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + ldFK + "') and f.paymode in('" + payMode + "')  and f.entryusercode in('" + finUser + "') and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4' " + strMemtypeValue + " ";
                        if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + GrpselCol + "";
                        SelQ += "  order by Transcode";
                    }
                    #endregion
                }
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(SelQ, "Text");
                #endregion
            }
        }
        catch { }
        return dsload;
    }

    protected Hashtable getDeptName(int val)
    {
        Hashtable htdtName = new Hashtable();
        try
        {
            // collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            collegecode = getColgCode();
            string SelQ = string.Empty;
            if (val == 0)
                SelQ = " select distinct d.degree_code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from degree d,department dt,course c where c.course_id=d.course_id and d.dept_code=dt.dept_code and d.college_code in('" + collegecode + "')";
            else
                SelQ = " select distinct dept_acronym as dept_acronym,st.dept_code as degree_code from stafftrans st,hrdept_master hm where hm.dept_code=st.dept_code and hm.college_code in('" + collegecode + "')";
            DataSet dsdeg = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsdeg.Tables.Count > 0 && dsdeg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsdeg.Tables[0].Rows.Count; row++)
                {
                    if (!htdtName.ContainsKey(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"])))
                        htdtName.Add(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"]), Convert.ToString(dsdeg.Tables[0].Rows[row]["dept_acronym"]));//degreename --dept acronym changed instead of dept name
                }
            }
        }
        catch { }
        return htdtName;
    }

    protected string getColgCode()
    {
        string clgCode = string.Empty;
        try
        {
            StringBuilder sbclg = new StringBuilder();
            for (int row = 0; row < cblclg.Items.Count; row++)
            {
                sbclg.Append(Convert.ToString(cblclg.Items[row].Value + "','"));
            }
            if (sbclg.Length > 0)
            {
                sbclg.Remove(sbclg.Length - 3, 3);
                clgCode = Convert.ToString(sbclg);
            }
        }
        catch { }
        return clgCode;
    }

    protected Hashtable getHeaderFK()
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
        }
        catch { hthdName.Clear(); }
        return hthdName;
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

    //college go method for student
    protected void loadSpreadDet(DataSet ds, string selColumn)
    {
        try
        {
            #region design

            RollAndRegSettings();
            Hashtable htColHD = htcolumnHeaderValue();
            int val = 0;
            Hashtable htdegName = getDeptName(val);
            Hashtable hthdName = getHeaderFK();
            string spHeadCol = getheadername();
            bool boolSno = false;

            DataTable dtInstWisePaidReport = new DataTable();
            DataRow drowInst;
            ArrayList arrColHdrNames = new ArrayList();
            arrColHdrNames.Add("S.No");
            dtInstWisePaidReport.Columns.Add("col0");

            string[] splVal = spHeadCol.Split(',');

            int colCount = 1;
            for (int row = 0; row < splVal.Length; row++)
            {
                switch (splVal[row].Trim())
                {
                    case "Header/Ledger":
                        if (rblmode.SelectedIndex == 0)
                            splVal[row] = "Header";
                        else
                            splVal[row] = "Ledger";
                        break;
                }
                string headerName = Convert.ToString(splVal[row].Trim());

                if (headerName == "Admission No")
                {
                    if (roll == 0 || roll == 1 || roll == 4 || roll == 6 || roll == 7)
                    {
                        arrColHdrNames.Add(headerName);
                        dtInstWisePaidReport.Columns.Add("col" + colCount);
                        colCount++;
                    }
                }
                else if (headerName == "Roll No")
                {
                    if (roll == 0 || roll == 1 || roll == 2 || roll == 5 || roll == 7)
                    {
                        arrColHdrNames.Add(headerName);
                        dtInstWisePaidReport.Columns.Add("col" + colCount);
                        colCount++;
                    }
                }
                else if (headerName == "Reg No")
                {
                    if (roll == 0 || roll == 1 || roll == 3 || roll == 5 || roll == 6)
                    {
                        arrColHdrNames.Add(headerName);
                        dtInstWisePaidReport.Columns.Add("col" + colCount);
                        colCount++;
                    }
                }
                else
                {
                    arrColHdrNames.Add(headerName);
                    dtInstWisePaidReport.Columns.Add("col" + colCount);
                    colCount++;
                }
            }
            DataRow drHdr1 = dtInstWisePaidReport.NewRow();
            for (int grCol = 0; grCol < dtInstWisePaidReport.Columns.Count; grCol++)
            {
                drHdr1["col" + grCol] = arrColHdrNames[grCol];
            }
            dtInstWisePaidReport.Rows.Add(drHdr1);

            #endregion

            #region value

            int rowCnt = 0;
            Hashtable htpayMode = new Hashtable();
            Hashtable htSubTot = new Hashtable();
            Hashtable htcolCnt = new Hashtable();
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            ArrayList arTranstype = new ArrayList();
            arTranstype.Add("1");
            arTranstype.Add("2");
            arTranstype.Add("3");
            ArrayList arMemType = getMemType();
            int tblCnt = 0;
            int tblCount = 0;

            foreach (string memType in arMemType)
            {
                bool boolMemtype = false;
                tblCount++;
                string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                if (memType == "1")
                {
                    val = 0;
                    htdegName = getDeptName(val);
                }
                else
                {
                    val = 1;
                    htdegName = getDeptName(val);
                }
                DataTable dtExces = new DataTable();
                try
                {
                    dtExces = ds.Tables[1].DefaultView.ToTable();
                }
                catch { }
                for (int ar = 0; ar < arTranstype.Count; ar++)
                {
                    #region

                    for (int pay = 0; pay < chkl_paid.Items.Count; pay++)
                    {
                        if (chkl_paid.Items[pay].Selected)
                        {
                            string paymode = Convert.ToString(chkl_paid.Items[pay].Value);
                            string paymodeStr = Convert.ToString(chkl_paid.Items[pay].Text);
                            if (!htpayMode.ContainsKey(paymode))
                                htpayMode.Add(paymode, paymodeStr);
                            ds.Tables[tblCnt].DefaultView.RowFilter = "paymode='" + paymode + "' and Transtype='" + arTranstype[ar] + "'";
                            DataTable dvpaid = ds.Tables[tblCnt].DefaultView.ToTable();

                            if (dvpaid.Rows.Count > 0)
                            {
                                string dispText = string.Empty;
                                string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                if (transText != "")
                                    dispText = paymodeStr + "-(" + transText + ")";
                                else
                                    dispText = paymodeStr;
                                if (!boolMemtype)
                                {
                                    drowInst = dtInstWisePaidReport.NewRow();
                                    grdRow = dtInstWisePaidReport.Rows.Count;
                                    drowInst[0] = strMemType;
                                    dicInstWise.Add(grdRow, strMemType);
                                    boolMemtype = true;
                                    dtInstWisePaidReport.Rows.Add(drowInst);
                                }
                                drowInst = dtInstWisePaidReport.NewRow();
                                grdRow = dtInstWisePaidReport.Rows.Count;
                                drowInst[0] = dispText;
                                dtInstWisePaidReport.Rows.Add(drowInst);
                                dicInstWise.Add(grdRow, dispText);
                                for (int drow = 0; drow < dvpaid.Rows.Count; drow++)
                                {
                                    drowInst = dtInstWisePaidReport.NewRow();
                                    drowInst[0] = Convert.ToString(++rowCnt);
                                    bool boolTrans = false;
                                    string valueStr = string.Empty;
                                    string transcode = string.Empty;
                                    for (int dcol = 1; dcol < dvpaid.Columns.Count - 4; dcol++)
                                    {
                                        string headerName = Convert.ToString(dvpaid.Columns[dcol].ColumnName);
                                        valueStr = Convert.ToString(dvpaid.Rows[drow][dcol]);
                                        if (headerName.Trim() == "roll_no")
                                        {
                                            string receipt = Convert.ToString(dvpaid.Rows[drow]["receipttype"]);
                                            if (receipt == "3")
                                            {
                                                valueStr = Convert.ToString(dvpaid.Rows[drow]["roll_no"]);
                                            }
                                            else
                                            {
                                                valueStr = Convert.ToString(dvpaid.Rows[drow][dcol]);
                                            }
                                        }
                                        if (headerName.Trim() == "degree_code" || headerName.Trim() == "dept_code")
                                            valueStr = Convert.ToString(htdegName[valueStr]);
                                        if (headerName.Trim() == "headerfk" || headerName.Trim() == "ledgerfk")
                                        {
                                            if (memType == "1")
                                                valueStr = Convert.ToString(dvpaid.Rows[drow]["headerfk"]);
                                            else
                                                valueStr = Convert.ToString(hthdName[valueStr]);
                                        }
                                        if (headerName.Trim() == "feecategory")//added by abarna 21.02.2018
                                        {
                                            string Feecategory = Convert.ToString(dvpaid.Rows[drow]["FeeCategory"]);
                                            valueStr = d2.GetFunction("select textval from textvaltable where TextCode='" + Feecategory + "'");
                                        }

                                        #region subtotal

                                        if (headerName.Trim() == "debit")
                                        {
                                            double debit = 0;
                                            double.TryParse(valueStr, out debit);
                                            if (!htSubTot.ContainsKey("debit"))
                                                htSubTot.Add("debit", debit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htSubTot["debit"]), out amount);
                                                amount += debit;
                                                htSubTot.Remove("debit");
                                                htSubTot.Add("debit", Convert.ToString(amount));
                                            }
                                            if (!htcolCnt.ContainsKey("debit"))
                                                htcolCnt.Add("debit", dcol);
                                        }
                                        if (headerName.Trim() == "credit")
                                        {
                                            double credit = 0;
                                            double.TryParse(valueStr, out credit);
                                            if (!htSubTot.ContainsKey("credit"))
                                                htSubTot.Add("credit", credit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htSubTot["credit"]), out amount);
                                                amount += credit;
                                                htSubTot.Remove("credit");
                                                htSubTot.Add("credit", Convert.ToString(amount));
                                            }
                                            if (!htcolCnt.ContainsKey("credit"))
                                                htcolCnt.Add("credit", dcol);
                                        }
                                        #endregion

                                        drowInst[dcol] = Convert.ToString(valueStr);

                                        if (headerName == "Transcode")
                                        {
                                            boolTrans = true;
                                            transcode = valueStr;
                                        }
                                    }
                                    #region Excess

                                    if (boolTrans && dtExces.Rows.Count > 0)
                                    {
                                        DataTable dvexcess = new DataTable();
                                        try
                                        {
                                            // dtExces
                                            //ds.Tables[1].DefaultView.RowFilter = "dailyTranscode='" + transcode + "'";
                                            //dvexcess = ds.Tables[1].DefaultView.ToTable();
                                            dtExces.DefaultView.RowFilter = " dailyTranscode='" + transcode + "' and paymode='" + paymode + "'";
                                            dvexcess = dtExces.DefaultView.ToTable();
                                        }
                                        catch { }
                                        if (dvexcess.Rows.Count > 0)
                                        {
                                            for (int drows = 0; drows < dvexcess.Rows.Count; drows++)
                                            {
                                                drowInst = dtInstWisePaidReport.NewRow();
                                                drowInst[0] = Convert.ToString(++rowCnt);

                                                for (int dcol = 1; dcol < dvexcess.Columns.Count - 2; dcol++)
                                                {
                                                    string headerName = Convert.ToString(dvexcess.Columns[dcol].ColumnName);
                                                    valueStr = Convert.ToString(dvexcess.Rows[drows][dcol]);
                                                    if (headerName.Trim() == "degree_code" || headerName.Trim() == "dept_code")
                                                        valueStr = Convert.ToString(htdegName[valueStr]);
                                                    if (headerName.Trim() == "headerfk" || headerName.Trim() == "ledgerfk")
                                                        valueStr = Convert.ToString(hthdName[valueStr]) + "(Excess/Advance)";

                                                    #region subtotal
                                                    if (headerName.Trim() == "debit")
                                                    {
                                                        double debit = 0;
                                                        double.TryParse(valueStr, out debit);
                                                        if (!htSubTot.ContainsKey("debit"))
                                                            htSubTot.Add("debit", debit);
                                                        else
                                                        {
                                                            double amount = 0;
                                                            double.TryParse(Convert.ToString(htSubTot["debit"]), out amount);
                                                            amount += debit;
                                                            htSubTot.Remove("debit");
                                                            htSubTot.Add("debit", Convert.ToString(amount));
                                                        }
                                                        if (!htcolCnt.ContainsKey("debit"))
                                                            htcolCnt.Add("debit", dcol);
                                                    }
                                                    if (headerName.Trim() == "credit")
                                                    {
                                                        double credit = 0;
                                                        double.TryParse(valueStr, out credit);
                                                        if (!htSubTot.ContainsKey("credit"))
                                                            htSubTot.Add("credit", credit);
                                                        else
                                                        {
                                                            double amount = 0;
                                                            double.TryParse(Convert.ToString(htSubTot["credit"]), out amount);
                                                            amount += credit;
                                                            htSubTot.Remove("credit");
                                                            htSubTot.Add("credit", Convert.ToString(amount));
                                                        }
                                                        if (!htcolCnt.ContainsKey("credit"))
                                                            htcolCnt.Add("credit", dcol);
                                                    }
                                                    #endregion
                                                    drowInst = dtInstWisePaidReport.NewRow();
                                                    drowInst[dcol] = Convert.ToString(valueStr);
                                                }
                                            }
                                            foreach (DataRow row in dtExces.Rows)
                                            {
                                                if (row["dailyTranscode"].ToString() == transcode)
                                                {
                                                    dtExces.Rows.Remove(row);
                                                    break;
                                                }
                                            }
                                            dtExces.AcceptChanges();
                                        }
                                    }
                                    #endregion
                                    dtInstWisePaidReport.Rows.Add(drowInst);
                                }
                            }
                        }
                        if (htSubTot.Count > 0)
                        {
                            double fnlDebit = 0;
                            double fnlcredit = 0;
                            int debitCnt = 0;
                            int creditCnt = 0;
                            double.TryParse(Convert.ToString(htSubTot["debit"]), out fnlDebit);
                            double.TryParse(Convert.ToString(htSubTot["credit"]), out fnlcredit);
                            int.TryParse(Convert.ToString(htcolCnt["debit"]), out debitCnt);
                            int.TryParse(Convert.ToString(htcolCnt["credit"]), out creditCnt);
                            drowInst = dtInstWisePaidReport.NewRow();
                            grdRow = dtInstWisePaidReport.Rows.Count;
                            drowInst[0] = "Total";
                            dicInstWise.Add(grdRow, "Total");
                            if (debitCnt > 0)
                            {
                                drowInst[debitCnt] = Convert.ToString(fnlDebit);
                            }
                            if (creditCnt > 0)
                            {
                                drowInst[creditCnt] = Convert.ToString(fnlcredit);
                            }
                            htSubTot.Clear();
                            dtInstWisePaidReport.Rows.Add(drowInst);
                        }
                    }
                    #endregion
                }
                if (!cbJournal.Checked && cbIncOthers.Checked)
                {
                    tblCnt++;
                }
            }
            grdInstWisePaidReport.DataSource = dtInstWisePaidReport;
            grdInstWisePaidReport.DataBind();
            grdInstWisePaidReport.Visible = true;
            foreach (KeyValuePair<int, string> dr in dicInstWise)
            {
                int rowcnt = dr.Key;
                int d = Convert.ToInt32(dtInstWisePaidReport.Columns.Count);
                string payModeVal = dr.Value.ToString();
                if (payModeVal != "Total")
                {
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].Font.Bold = true;
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].Font.Size = 13;
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].ColumnSpan = d;
                    if (payModeVal.Contains("Cash"))
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#F08080");
                    else if (payModeVal == "Cheque")
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                    else if (payModeVal == "DD")
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FFA500");
                    else if (payModeVal == "Online")
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#90EE90");
                    else if (payModeVal == "Card")
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                    for (int a = 1; a < d; a++)
                    {
                        grdInstWisePaidReport.Rows[rowcnt].Cells[a].Visible = false;
                    }
                }
                if (payModeVal == "Total")
                {
                    for (int gridCol = 0; gridCol < dtInstWisePaidReport.Columns.Count; gridCol++)
                    {
                        grdInstWisePaidReport.Rows[rowcnt].Cells[gridCol].BackColor = Color.Green;
                        grdInstWisePaidReport.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                        grdInstWisePaidReport.Rows[rowcnt].Cells[gridCol].Font.Size = 13;
                    }
                }
            }
            //lblvalidation1.Text = "";
            txtexcelname.Text = "";
            print.Visible = true;
            payModeLabels(htpayMode);
            #endregion
        }
        catch { }
    }

    //school go method for student
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

    protected void loadSpreadDetSchool(DataSet ds, string selColumn)
    {
        try
        {
            #region design

            RollAndRegSettings();
            Hashtable htColHD = htcolumnHeaderValue();
            int val = 0;
            Hashtable htdegName = getDeptName(val);
            Hashtable hthdName = getHeaderFK();
            string spHeadCol = getheadername();
            bool boolSno = false;
            DataTable dtInstWisePaidReport = new DataTable();
            DataRow drowInst;
            ArrayList arrColHdrNames = new ArrayList();
            arrColHdrNames.Add("S.No");
            dtInstWisePaidReport.Columns.Add("col0");

            string[] splVal = spHeadCol.Split(',');
            int colCount = 1;
            for (int row = 0; row < splVal.Length; row++)
            {
                switch (splVal[row].Trim())
                {
                    case "Header/Ledger":
                        if (rblmode.SelectedIndex == 0)
                            splVal[row] = "Header";
                        else
                            splVal[row] = "Ledger";
                        break;
                }
                string headerName = Convert.ToString(splVal[row].Trim());
                if (headerName == "Admission No")
                {
                    arrColHdrNames.Add(headerName);
                    dtInstWisePaidReport.Columns.Add("col" + colCount);
                    colCount++;
                }
                else if (headerName == "Roll No")
                {
                    arrColHdrNames.Add(headerName);
                    dtInstWisePaidReport.Columns.Add("col" + colCount);
                    colCount++;
                }
                else if (headerName == "Reg No")
                {
                    arrColHdrNames.Add(headerName);
                    dtInstWisePaidReport.Columns.Add("col" + colCount);
                    colCount++;
                }
                else
                {
                    arrColHdrNames.Add(headerName);
                    dtInstWisePaidReport.Columns.Add("col" + colCount);
                    colCount++;
                }
            }

            DataRow drHdr1 = dtInstWisePaidReport.NewRow();
            for (int grCol = 0; grCol < dtInstWisePaidReport.Columns.Count; grCol++)
            {
                drHdr1["col" + grCol] = arrColHdrNames[grCol];
            }
            dtInstWisePaidReport.Rows.Add(drHdr1);

            #endregion

            #region value

            int rowCnt = 0;
            Hashtable htpayMode = new Hashtable();
            Hashtable htSubTot = new Hashtable();
            Hashtable htcolCnt = new Hashtable();
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            ArrayList arTranstype = new ArrayList();
            ArrayList arFnlYear = getSelFinlDate();
            arTranstype.Add("1");
            arTranstype.Add("2");
            arTranstype.Add("3");
            ArrayList arMemType = getMemType();
            int tblCnt = 0;
            int tblCount = 0;
            foreach (string memType in arMemType)
            {
                bool boolMemtype = false;
                tblCount++;
                string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                foreach (string fnlYear in arFnlYear)
                {
                    #region
                    for (int ar = 0; ar < arTranstype.Count; ar++)
                    {
                        for (int pay = 0; pay < chkl_paid.Items.Count; pay++)
                        {
                            bool boolMemName = false;
                            if (!chkl_paid.Items[pay].Selected)
                                continue;
                            string paymode = Convert.ToString(chkl_paid.Items[pay].Value);
                            string paymodeStr = Convert.ToString(chkl_paid.Items[pay].Text);
                            if (!htpayMode.ContainsKey(paymode))
                                htpayMode.Add(paymode, paymodeStr);
                            ds.Tables[tblCnt].DefaultView.RowFilter = "paymode='" + paymode + "' and Transtype='" + arTranstype[ar] + "' and actualfinyearfk='" + fnlYear + "'";
                            DataTable dvpaid = ds.Tables[tblCnt].DefaultView.ToTable();
                            if (dvpaid.Rows.Count > 0)
                            {
                                string dispText = string.Empty;
                                string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                if (transText != "")
                                    dispText = paymodeStr + "-(" + transText + ")";
                                else
                                    dispText = paymodeStr;
                                if (!boolMemName)//financial year added here
                                {
                                    dispText += "-" + fnlYear;
                                    boolMemName = true;
                                }
                                if (!boolMemtype)
                                {
                                    drowInst = dtInstWisePaidReport.NewRow();
                                    grdRow = dtInstWisePaidReport.Rows.Count;
                                    drowInst[0] = strMemType;
                                    dicInstWise.Add(grdRow, strMemType);
                                    boolMemtype = true;
                                    dtInstWisePaidReport.Rows.Add(drowInst);
                                }
                                drowInst = dtInstWisePaidReport.NewRow();
                                grdRow = dtInstWisePaidReport.Rows.Count;
                                drowInst[0] = dispText;
                                dicInstWise.Add(grdRow, dispText);
                                dtInstWisePaidReport.Rows.Add(drowInst);

                                for (int drow = 0; drow < dvpaid.Rows.Count; drow++)
                                {
                                    drowInst = dtInstWisePaidReport.NewRow();
                                    drowInst[0] = Convert.ToString(++rowCnt);

                                    for (int dcol = 1; dcol < dvpaid.Columns.Count - 5; dcol++)
                                    {
                                        string headerName = Convert.ToString(dvpaid.Columns[dcol].ColumnName);
                                        string valueStr = Convert.ToString(dvpaid.Rows[drow][dcol]);
                                        if (headerName.Trim() == "degree_code" || headerName.Trim() == "dept_code")
                                            valueStr = Convert.ToString(htdegName[valueStr]);
                                        if (headerName.Trim() == "headerfk" || headerName.Trim() == "ledgerfk")
                                        {
                                            if (memType == "1")
                                                valueStr = Convert.ToString(dvpaid.Rows[drow]["headerfk"]);
                                            else
                                                valueStr = Convert.ToString(hthdName[valueStr]);
                                        }

                                        #region subtotal
                                        if (headerName.Trim() == "debit")
                                        {
                                            double debit = 0;
                                            double.TryParse(valueStr, out debit);
                                            if (!htSubTot.ContainsKey("debit"))
                                                htSubTot.Add("debit", debit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htSubTot["debit"]), out amount);
                                                amount += debit;
                                                htSubTot.Remove("debit");
                                                htSubTot.Add("debit", Convert.ToString(amount));
                                            }
                                            if (!htcolCnt.ContainsKey("debit"))
                                                htcolCnt.Add("debit", dcol);
                                        }
                                        if (headerName.Trim() == "credit")
                                        {
                                            double credit = 0;
                                            double.TryParse(valueStr, out credit);
                                            if (!htSubTot.ContainsKey("credit"))
                                                htSubTot.Add("credit", credit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htSubTot["credit"]), out amount);
                                                amount += credit;
                                                htSubTot.Remove("credit");
                                                htSubTot.Add("credit", Convert.ToString(amount));
                                            }
                                            if (!htcolCnt.ContainsKey("credit"))
                                                htcolCnt.Add("credit", dcol);
                                        }
                                        #endregion

                                        drowInst[dcol] = Convert.ToString(valueStr);
                                    }
                                    dtInstWisePaidReport.Rows.Add(drowInst);
                                }
                            }
                            if (htSubTot.Count > 0)
                            {
                                #region
                                double fnlDebit = 0;
                                double fnlcredit = 0;
                                int debitCnt = 0;
                                int creditCnt = 0;
                                double.TryParse(Convert.ToString(htSubTot["debit"]), out fnlDebit);
                                double.TryParse(Convert.ToString(htSubTot["credit"]), out fnlcredit);
                                int.TryParse(Convert.ToString(htcolCnt["debit"]), out debitCnt);
                                int.TryParse(Convert.ToString(htcolCnt["credit"]), out creditCnt);
                                drowInst = dtInstWisePaidReport.NewRow();
                                grdRow = dtInstWisePaidReport.Rows.Count;
                                drowInst[0] = "Total";
                                dicInstWise.Add(grdRow, "Total");
                                if (debitCnt > 0)
                                    drowInst[debitCnt] = Convert.ToString(fnlDebit);
                                if (creditCnt > 0)
                                    drowInst[creditCnt] = Convert.ToString(fnlcredit);
                                htSubTot.Clear();
                                dtInstWisePaidReport.Rows.Add(drowInst);
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                //if (tblCount == 1)
                //    tblCnt = 2;
                //else
                tblCnt++;
            }
            grdInstWisePaidReport.DataSource = dtInstWisePaidReport;
            grdInstWisePaidReport.DataBind();
            grdInstWisePaidReport.Visible = true;
            foreach (KeyValuePair<int, string> dr in dicInstWise)
            {
                int rowcnt = dr.Key;
                int d = Convert.ToInt32(dtInstWisePaidReport.Columns.Count);
                string payModeVal = dr.Value.ToString();
                if (payModeVal != "Total")
                {
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].Font.Bold = true;
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].Font.Size = 13;
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].ColumnSpan = d;
                    if (payModeVal.Contains("Cash"))
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#F08080");
                    else if (payModeVal.Contains("Cheque"))
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                    else if (payModeVal.Contains("DD"))
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FFA500");
                    else if (payModeVal.Contains("Online"))
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#90EE90");
                    else if (payModeVal.Contains("Card"))
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                    for (int a = 1; a < d; a++)
                    {
                        grdInstWisePaidReport.Rows[rowcnt].Cells[a].Visible = false;
                    }
                }
                if (payModeVal == "Total")
                {
                    for (int gridCol = 0; gridCol < dtInstWisePaidReport.Columns.Count; gridCol++)
                    {
                        grdInstWisePaidReport.Rows[rowcnt].Cells[gridCol].BackColor = Color.Green;
                        grdInstWisePaidReport.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                        grdInstWisePaidReport.Rows[rowcnt].Cells[gridCol].Font.Size = 13;
                    }
                }
            }
            // lblvalidation1.Text = "";
            txtexcelname.Text = "";
            print.Visible = true;
            payModeLabels(htpayMode);
            #endregion
        }
        catch { }
    }

    protected void loadSpreadDetOthers(DataSet ds, string selColumn)
    {
        try
        {
            #region design

            Hashtable htColHD = htcolumnHeaderValue();
            int val = 1;
            Hashtable htdegName = getDeptName(val);
            Hashtable hthdName = getHeaderFK();
            string spHeadCol = getheadername();
            bool boolSno = false;
            DataTable dtInstWisePaidReport = new DataTable();
            DataRow drowInst;
            ArrayList arrColHdrNames = new ArrayList();
            arrColHdrNames.Add("S.No");
            dtInstWisePaidReport.Columns.Add("col0");
            int colCount = 1;

            string[] splVal = spHeadCol.Split(',');
            for (int row = 0; row < splVal.Length; row++)
            {
                switch (splVal[row].Trim())
                {
                    case "Header/Ledger":
                        if (rblmode.SelectedIndex == 0)
                            splVal[row] = "Header";
                        else
                            splVal[row] = "Ledger";
                        break;
                }
                string headerName = Convert.ToString(splVal[row].Trim());
                arrColHdrNames.Add(headerName);
                dtInstWisePaidReport.Columns.Add("col" + colCount);
                colCount++;
            }
            DataRow drHdr1 = dtInstWisePaidReport.NewRow();
            for (int grCol = 0; grCol < dtInstWisePaidReport.Columns.Count; grCol++)
            {
                drHdr1["col" + grCol] = arrColHdrNames[grCol];
            }
            dtInstWisePaidReport.Rows.Add(drHdr1);

            #endregion

            #region value

            int totcblCnt = 0;
            if (checkSchoolSetting() == 0)
                totcblCnt = 3;
            else
                totcblCnt = 2;
            int rowCnt = 0;
            Hashtable htpayMode = new Hashtable();
            Hashtable htSubTot = new Hashtable();
            Hashtable htcolCnt = new Hashtable();
            Hashtable htGrandcolCnt = new Hashtable();
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            ArrayList arTranstype = new ArrayList();
            arTranstype.Add("1");
            arTranstype.Add("2");
            arTranstype.Add("3");
            string selectedName = getSelMemName(cblmem);
            string[] splMemName = selectedName.Split(',');//get memtype name
            for (int dsCnt = 0; dsCnt < ds.Tables.Count; dsCnt++)
            {
                if (ds.Tables[dsCnt].Rows.Count == 0)//if there is no row available back to continue
                    continue;
                for (int ar = 0; ar < arTranstype.Count; ar++)
                {
                    for (int pay = 0; pay < chkl_paid.Items.Count; pay++)
                    {
                        bool boolMemName = false;
                        if (chkl_paid.Items[pay].Selected)
                        {
                            string paymode = Convert.ToString(chkl_paid.Items[pay].Value);
                            string paymodeStr = Convert.ToString(chkl_paid.Items[pay].Text);
                            if (!htpayMode.ContainsKey(paymode))
                                htpayMode.Add(paymode, paymodeStr);
                            ds.Tables[dsCnt].DefaultView.RowFilter = "paymode='" + paymode + "' and Transtype='" + arTranstype[ar] + "'";
                            DataTable dvpaid = ds.Tables[dsCnt].DefaultView.ToTable();
                            if (dvpaid.Rows.Count > 0)
                            {
                                string dispText = string.Empty;
                                string transText = Convert.ToInt32(arTranstype[ar]) == 3 ? "Journal Entry" : "";
                                if (transText != "")
                                    dispText = paymodeStr + "-(" + transText + ")";
                                else
                                    dispText = paymodeStr;
                                if (!boolMemName && splMemName.Length > 0)//memtype name added here
                                {
                                    dispText += "-" + Convert.ToString(splMemName[dsCnt]);
                                    boolMemName = true;
                                }
                                drowInst = dtInstWisePaidReport.NewRow();
                                grdRow = dtInstWisePaidReport.Rows.Count;
                                drowInst[0] = dispText;
                                dicInstWise.Add(grdRow, dispText);
                                dtInstWisePaidReport.Rows.Add(drowInst);
                                for (int drow = 0; drow < dvpaid.Rows.Count; drow++)
                                {
                                    drowInst = dtInstWisePaidReport.NewRow();
                                    drowInst[0] = Convert.ToString(++rowCnt);
                                    for (int dcol = 1; dcol < dvpaid.Columns.Count - totcblCnt; dcol++)
                                    {
                                        string headerName = Convert.ToString(dvpaid.Columns[dcol].ColumnName);
                                        string valueStr = Convert.ToString(dvpaid.Rows[drow][dcol]);
                                        if (headerName.Trim() == "degree_code" || headerName.Trim() == "dept_code")
                                            valueStr = Convert.ToString(htdegName[valueStr]);
                                        if (headerName.Trim() == "headerfk" || headerName.Trim() == "ledgerfk")
                                            valueStr = Convert.ToString(hthdName[valueStr]);

                                        #region subtotal
                                        if (headerName.Trim() == "debit")
                                        {
                                            double debit = 0;
                                            double.TryParse(valueStr, out debit);
                                            if (!htSubTot.ContainsKey("debit"))
                                                htSubTot.Add("debit", debit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htSubTot["debit"]), out amount);
                                                amount += debit;
                                                htSubTot.Remove("debit");
                                                htSubTot.Add("debit", Convert.ToString(amount));
                                            }
                                            if (!htcolCnt.ContainsKey("debit"))
                                                htcolCnt.Add("debit", dcol);
                                        }
                                        if (headerName.Trim() == "credit")
                                        {
                                            double credit = 0;
                                            double.TryParse(valueStr, out credit);
                                            if (!htSubTot.ContainsKey("credit"))
                                                htSubTot.Add("credit", credit);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htSubTot["credit"]), out amount);
                                                amount += credit;
                                                htSubTot.Remove("credit");
                                                htSubTot.Add("credit", Convert.ToString(amount));
                                            }
                                            if (!htcolCnt.ContainsKey("credit"))
                                                htcolCnt.Add("credit", dcol);
                                        }
                                        #endregion

                                        drowInst[dcol] = Convert.ToString(valueStr);
                                    }
                                    dtInstWisePaidReport.Rows.Add(drowInst);
                                }
                            }
                        }
                        if (htSubTot.Count > 0)
                        {
                            #region total
                            double fnlDebit = 0;
                            double fnlcredit = 0;
                            int debitCnt = 0;
                            int creditCnt = 0;
                            double.TryParse(Convert.ToString(htSubTot["debit"]), out fnlDebit);
                            double.TryParse(Convert.ToString(htSubTot["credit"]), out fnlcredit);
                            int.TryParse(Convert.ToString(htcolCnt["debit"]), out debitCnt);
                            int.TryParse(Convert.ToString(htcolCnt["credit"]), out creditCnt);
                            drowInst = dtInstWisePaidReport.NewRow();
                            grdRow = dtInstWisePaidReport.Rows.Count;
                            drowInst[0] = "Total";
                            dicInstWise.Add(grdRow, "Total");
                            if (debitCnt > 0)
                                drowInst[debitCnt] = Convert.ToString(fnlDebit);
                            if (creditCnt > 0)
                                drowInst[creditCnt] = Convert.ToString(fnlcredit);

                            #region Grand total
                            if (!htGrandcolCnt.ContainsKey("debit"))
                                htGrandcolCnt.Add("debit", fnlDebit);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htGrandcolCnt["debit"]), out amount);
                                amount += fnlDebit;
                                htGrandcolCnt.Remove("debit");
                                htGrandcolCnt.Add("debit", Convert.ToString(amount));
                            }
                            if (!htGrandcolCnt.ContainsKey("credit"))
                                htGrandcolCnt.Add("credit", fnlcredit);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htGrandcolCnt["credit"]), out amount);
                                amount += fnlcredit;
                                htGrandcolCnt.Remove("credit");
                                htGrandcolCnt.Add("credit", Convert.ToString(amount));
                            }

                            #endregion
                            htSubTot.Clear();
                            dtInstWisePaidReport.Rows.Add(drowInst);
                            #endregion
                        }
                    }
                }
            }
            if (htGrandcolCnt.Count > 0)
            {
                #region Grnad total
                double fnlDebit = 0;
                double fnlcredit = 0;
                int debitCnt = 0;
                int creditCnt = 0;
                double.TryParse(Convert.ToString(htGrandcolCnt["debit"]), out fnlDebit);
                double.TryParse(Convert.ToString(htGrandcolCnt["credit"]), out fnlcredit);
                int.TryParse(Convert.ToString(htcolCnt["debit"]), out debitCnt);
                int.TryParse(Convert.ToString(htcolCnt["credit"]), out creditCnt);
                drowInst = dtInstWisePaidReport.NewRow();
                drowInst[0] = "Grand Total";
                if (debitCnt > 0)
                    drowInst[debitCnt] = Convert.ToString(fnlDebit);
                if (creditCnt > 0)
                    drowInst[creditCnt] = Convert.ToString(fnlcredit);
                htGrandcolCnt.Clear();
                dtInstWisePaidReport.Rows.Add(drowInst);
                #endregion
            }
            grdInstWisePaidReport.DataSource = dtInstWisePaidReport;
            grdInstWisePaidReport.DataBind();
            grdInstWisePaidReport.Visible = true;
            foreach (KeyValuePair<int, string> dr in dicInstWise)
            {
                int rowcnt = dr.Key;
                int d = Convert.ToInt32(dtInstWisePaidReport.Columns.Count);
                string payModeVal = dr.Value.ToString();
                if (payModeVal != "Total")
                {
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].Font.Bold = true;
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].Font.Size = 13;
                    grdInstWisePaidReport.Rows[rowcnt].Cells[0].ColumnSpan = d;
                    if (payModeVal == "Cash")
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#F08080");
                    else if (payModeVal == "Cheque")
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                    else if (payModeVal == "DD")
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FFA500");
                    else if (payModeVal == "Online")
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#90EE90");
                    else if (payModeVal == "Card")
                        grdInstWisePaidReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                    for (int a = 1; a < d; a++)
                    {
                        grdInstWisePaidReport.Rows[rowcnt].Cells[a].Visible = false;
                    }
                }
                if (payModeVal == "Total")
                {
                    for (int gridCol = 0; gridCol < dtInstWisePaidReport.Columns.Count; gridCol++)
                    {
                        grdInstWisePaidReport.Rows[rowcnt].Cells[gridCol].BackColor = Color.Green;
                        grdInstWisePaidReport.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                        grdInstWisePaidReport.Rows[rowcnt].Cells[gridCol].Font.Size = 13;
                    }
                }
            }
            // lblvalidation1.Text = "";
            txtexcelname.Text = "";
            print.Visible = true;
            payModeLabels(htpayMode);
            #endregion
        }
        catch { }
    }

    protected void payModeLabels(Hashtable htpay)
    {
        lblcash.Visible = false;
        lblchq.Visible = false;
        lbldd.Visible = false;
        lblchal.Visible = false;
        lblonline.Visible = false;
        lblcard.Visible = false;
        lblNeft.Visible = false;//Added by saranya on 13/2/2018
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
            //Added by saranya on 13/2/2018
            if (row.Key.ToString() == "7")
                lblNeft.Visible = true;
        }
        divlabl.Visible = true;
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

    protected bool getTableValidation(DataSet ds)
    {
        bool boolCheck = false;
        try
        {
            if (rblMemType.SelectedIndex == 0)
            {
                if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0 || ds.Tables[3].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0))
                    boolCheck = true;
            }
            else
            {
                if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0))
                    boolCheck = true;
            }
        }
        catch { }
        return boolCheck;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        bool boolCheck = false;
        //htcolumnValue();//column order original column values
        string groupStr = string.Empty;
        int checkVal = 0;
        if (rblMemType.SelectedIndex == 0)
            checkVal = 0;
        else
            checkVal = 1;
        string selColumn = getSelectedColumn(ref groupStr, checkVal);
        ds.Reset();
        ds = loadDetails(selColumn, ref groupStr);
        if (rblMemType.SelectedIndex == 0 && cbAcdYear.Checked && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)//academic year selected only this function execute
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
                            ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";////abarna 8.03.2018
                        }
                        else
                        {
                            ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "'  and feecategory in('" + feeCate + "')";//and batch_year='" + getVal.Key.Split('$')[1] + "'//abarna 8.03.2018
                        }
                        DataTable dtPaid = ds.Tables[0].DefaultView.ToTable();
                        //ds.Tables[1].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                        //DataTable dtExcess = ds.Tables[1].DefaultView.ToTable();
                        if (!boolDs)
                        {
                            dsFinal.Reset();
                            // dsFinal.Tables.Add(dtFirst);                           
                            dsFinal.Tables.Add(dtPaid);
                            //   dsFinal.Tables.Add(dtExcess);
                            boolDs = true;
                        }
                        else
                        {
                            //dsFinal.Merge(dtPaid);
                            dsFinal.Merge(dtPaid);
                            //dsFinal.Merge(dtExcess);
                        }
                    }
                }
                ds.Reset();
                if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                {
                    DataTable tempTbl = dsFinal.Tables[0].DefaultView.ToTable();
                    //DataTable tempTblOne = dsFinal.Tables[1].DefaultView.ToTable();

                    tempTbl.Columns.Remove("college_code");
                    tempTbl.Columns.Remove("batch_year");
                    tempTbl.Columns.Remove("feecategory");

                    //tempTblOne.Columns.Remove("college_code");
                    // tempTblOne.Columns.Remove("batch_year");
                    //tempTblOne.Columns.Remove("feecategory");
                    ds.Reset();
                    ds.Tables.Add(tempTbl);
                    //  ds.Tables.Add(tempTblOne);
                    if (cbIncOthers.Checked)
                    {
                        ds.Tables.Add(dsNornaml.Tables[1].DefaultView.ToTable());
                        ds.Tables.Add(dsNornaml.Tables[2].DefaultView.ToTable());
                        ds.Tables.Add(dsNornaml.Tables[3].DefaultView.ToTable());
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
        if (getTableValidation(ds))
        {
            if (rblMemType.SelectedIndex == 0)//student
            {
                if (checkSchoolSetting() == 0)//school
                    loadSpreadDetSchool(ds, selColumn);
                else
                    loadSpreadDet(ds, selColumn);
            }
            else
            {
                //add multiple table into single table like staff,vendor,other
                try
                {
                    //DataTable dtTemp = new DataTable();
                    //for (int rowds = 0; rowds < ds.Tables.Count; rowds++)
                    //{
                    //    dtTemp.Merge(ds.Tables[rowds]);
                    //}
                    //ds.Reset();
                    //ds.Tables.Add(dtTemp);
                    //if (getTableValidation(ds))
                    //{
                    loadSpreadDetOthers(ds, selColumn);
                    //}
                    //else
                    //    boolCheck = true;
                }
                catch { boolCheck = true; ds.Reset(); }
            }
        }
        else
            boolCheck = true;
        if (boolCheck)
        {
            //lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            grdInstWisePaidReport.Visible = false;
            //spreadDet.Visible = false;
            print.Visible = false;
            divlabl.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }
    protected void close_click(object sender, EventArgs e)
    {
        divcolorder.Visible = false;
    }
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
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdInstWisePaidReport, reportname);
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

    public override void VerifyRenderingInServerForm(Control control)
    { }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            // lblvalidation1.Text = "";
            string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            string counterName = getCounterName(Convert.ToString(getCblSelectedValue(cbluser)));
            string ss = Session["usercode"].ToString();
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            degreedetails = "Individual Student Daybook Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy") + '@' + "User/Counter : " + counterName;
            //  degreedetails = "Individual Student Daybook Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "User/Counter : " + counterName;
            pagename = "FinanceBalDet.aspx";
            Printcontrolhed.loadspreaddetails(grdInstWisePaidReport, pagename, degreedetails, 0, ss);
            //Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails, 0, Convert.ToString(Session["usercode"]));
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected void btn_print_Click(object sender, EventArgs e)
    {
        try
        {
            // lblvalidation1.Text = "";
            string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            string counterName = getCounterName(Convert.ToString(getCblSelectedValue(cbluser)));

            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Individual Student Daybook Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy") + '@' + "User/Counter : " + counterName;
            pagename = "FinanceBalDet.aspx";
            //Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails, 1, Convert.ToString(Session["usercode"]));
            Printcontrolhed.Visible = true;
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

    protected string getCounterName(string userId)
    {
        string strAcr = string.Empty;
        try
        {
            StringBuilder clgAcr = new StringBuilder();
            string selQ = " select distinct  user_id as acr,user_code from usermaster where fin_user='1' and user_code in('" + userId + "')";
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

    #region colorder

    protected void lnkcolorder_Click(object sender, EventArgs e)
    {
        txtcolorder.Text = string.Empty;
        loadcolumnorder();
        columnType();
        ddlreport_SelectedIndexChanged(sender, e);
        // loadcolumns();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        //divcolorder.Visible = true;
    }

    public void loadcolumnorder()
    {
        cblcolumnorder.Items.Clear();
        if (rblMemType.SelectedIndex == 0)
        {
            cblcolumnorder.Items.Add(new ListItem("Name", "1"));
            cblcolumnorder.Items.Add(new ListItem("Roll No", "2"));
            cblcolumnorder.Items.Add(new ListItem("Reg No", "3"));
            cblcolumnorder.Items.Add(new ListItem("Admission No", "4"));
            cblcolumnorder.Items.Add(new ListItem("Dept Name", "5"));
            cblcolumnorder.Items.Add(new ListItem("Header/Ledger", "6"));
            cblcolumnorder.Items.Add(new ListItem("Receipt No", "7"));
            cblcolumnorder.Items.Add(new ListItem("Date", "8"));
            cblcolumnorder.Items.Add(new ListItem("Credit", "9"));
            cblcolumnorder.Items.Add(new ListItem("Debit", "10"));
            cblcolumnorder.Items.Add(new ListItem("Semester", "11"));//added by abarna 21.02.2018
            cblcolumnorder.Items.Add(new ListItem("Narration", "12"));//added by abarna 21.02.2018
        }
        else
        {
            cblcolumnorder.Items.Add(new ListItem("Name", "1"));
            cblcolumnorder.Items.Add(new ListItem("Roll No", "2"));
            cblcolumnorder.Items.Add(new ListItem("Reg No", "3"));
            cblcolumnorder.Items.Add(new ListItem("Admission No", "4"));
            //cblcolumnorder.Items.Add(new ListItem("ID/Code", "2"));
            cblcolumnorder.Items.Add(new ListItem("Dept Name", "3"));
            cblcolumnorder.Items.Add(new ListItem("Header/Ledger", "4"));
            cblcolumnorder.Items.Add(new ListItem("Receipt No", "5"));
            cblcolumnorder.Items.Add(new ListItem("Date", "6"));
            cblcolumnorder.Items.Add(new ListItem("Credit", "7"));
            cblcolumnorder.Items.Add(new ListItem("Debit", "8"));

        }
    }

    protected Hashtable htcolumnValue(int val)
    {
        Hashtable htcol = new Hashtable();
        try
        {
            if (val == 0)
            {
                htcol.Add("Name", "stud_name");
                htcol.Add("Roll No", "roll_no");
                htcol.Add("Reg No", "reg_no");
                htcol.Add("Admission No", "roll_admit");
                htcol.Add("Dept Name", "degree_code");
            }
            else
            {
                htcol.Add("Name", "sm.staff_name");
                // htcol.Add("ID/Code", "sm.staff_code");
                htcol.Add("Roll No", "sm.staff_code");
                htcol.Add("Reg No", "sm.staff_code");
                htcol.Add("Admission No", "sm.staff_code");
                htcol.Add("Dept Name", "st.dept_code");
            }
            if (rblmode.SelectedIndex == 0)
                htcol.Add("Header/Ledger", "headerfk");
            else
                htcol.Add("Header/Ledger", "ledgerfk");
            htcol.Add("Receipt No", "Transcode");
            htcol.Add("Date", "convert(varchar(10),transdate,103)as transdate");
            htcol.Add("Credit", "sum(debit) as debit");
            htcol.Add("Debit", "sum(credit) as credit");
            htcol.Add("Semester", "feecategory");//added by abarna 21.02.2018
            htcol.Add("Narration", "narration");//added by abarna 21.02.2018
        }
        catch { }
        return htcol;
    }

    protected Hashtable htcolumnHeaderValue()
    {
        Hashtable htcol = new Hashtable();
        try
        {
            if (rblMemType.SelectedIndex == 0)
            {
                htcol.Add("stud_name", "Name");
                htcol.Add("roll_no", "Roll No");
                htcol.Add("reg_no", "Reg No");
                htcol.Add("roll_admit", "Admission No");
                htcol.Add("degree_code", "Dept Name");
            }
            else
            {
                htcol.Add("sm.staff_name", "Name");
                // htcol.Add("sm.staff_code", "ID/Code");
                htcol.Add("sm.staff_code", "Roll No");
                htcol.Add("sm.staff_code", "Reg No");
                htcol.Add("sm.staff_code", "Admission No");
                htcol.Add("st.dept_code", "Dept Name");
            }

            if (rblmode.SelectedIndex == 0)
                htcol.Add("headerfk", "Header/Ledger");
            else
                htcol.Add("ledgerfk", "Header/Ledger");
            htcol.Add("Transcode", "Receipt No");
            htcol.Add("convert(varchar(10),transdate,103)as transdate", "Date");
            htcol.Add("sum(debit) as debit", "Credit");
            htcol.Add("sum(credit) as credit", "Debit");
            htcol.Add("feecategory", "Semester");//added by abarna 21.02.2018
            htcol.Add("narration", "Narration");//added by abarna 21.02.2018

        }
        catch { }
        return htcol;
    }

    protected void btncolorderOK_Click(object sender, EventArgs e)
    {

        // loadcolumns();
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

    public bool columncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == true)
                {
                    colorder = true;
                }
            }
        }
        catch { }
        return colorder;
    }

    public void loadcolumns()
    {
        try
        {
            string linkname = "BillNowise report";
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblcolumnorder.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    colord.Clear();
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colord.Add(Convert.ToString(valuesplit[k]));
                                if (columnvalue == "")
                                    columnvalue = Convert.ToString(valuesplit[k]);
                                else
                                    columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                            }
                        }
                    }
                }

            }
            else
            {
                colord.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                    if (columnvalue == "")
                        columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                    else
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = d2.select_method_wo_parameter(sel, "Text");
                if (dscolor.Tables.Count > 0)
                {
                    int count = 0;
                    if (dscolor.Tables[0].Rows.Count > 0)
                    {
                        string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
                        string[] value1 = value.Split(',');
                        if (value1.Length > 0)
                        {
                            for (int i = 0; i < value1.Length; i++)
                            {
                                string val = value1[i].ToString();
                                for (int k = 0; k < cblcolumnorder.Items.Count; k++)
                                {
                                    if (val == cblcolumnorder.Items[k].Value)
                                    {
                                        cblcolumnorder.Items[k].Selected = true;
                                        count++;
                                    }
                                    if (count == cblcolumnorder.Items.Count)
                                        cb_column.Checked = true;
                                    else
                                        cb_column.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }

    #endregion

    #region report type added dropdown

    protected void btnAdd_OnClick(object sender, EventArgs e)
    {
        divaddtype.Visible = true;
        panel_description11.Visible = true;
    }

    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        selectReportType();
    }

    protected void btnDel_OnClick(object sender, EventArgs e)
    {
        deleteReportType();
    }

    //type save
    protected void btnexittype_Click(object sender, EventArgs e)
    {
        divaddtype.Visible = false;
        panel_description11.Visible = false;
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
            if (rblMemType.SelectedIndex == 0)
                linkCriteria = "FinancePaidDeailsStud";
            else
                linkCriteria = "FinancePaidDeailsOther";
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
            if (rblMemType.SelectedIndex == 0)
                linkCriteria = "FinancePaidDeailsStud";
            else
                linkCriteria = "FinancePaidDeailsOther";
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='" + linkCriteria + "' and CollegeCode='" + Usercollegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlreport.DataSource = ds;
                ddlreport.DataTextField = "MasterValue";
                ddlreport.DataValueField = "MasterCode";
                ddlreport.DataBind();
                // ddlreport.Items.Insert(0, new ListItem("Select", "0"));

                //main search filter
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
            if (rblMemType.SelectedIndex == 0)
                linkCriteria = "FinancePaidDeailsStud";
            else
                linkCriteria = "FinancePaidDeailsOther";
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

    #endregion

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

    protected string getSelectedColumn(ref string groupStr, int checkVal)
    {
        string val = string.Empty;
        try
        {
            StringBuilder strCol = new StringBuilder();
            StringBuilder grpstrCol = new StringBuilder();
            Hashtable htcolumn = htcolumnValue(checkVal);
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

    #region roll,reg,admission setting

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

    protected void spreadColumnVisible(int rollNo, int regNo, int admNo)
    {
        try
        {
            //#region
            //if (roll == 0)
            //{
            //    if (rollNo > 0)
            //        //spreadDet.Columns[rollNo].Visible = true;
            //    if (regNo > 0)
            //        //spreadDet.Columns[regNo].Visible = true;
            //    if (admNo > 0)
            //        //spreadDet.Columns[admNo].Visible = true;
            //}
            //else if (roll == 1)
            //{
            //    if (rollNo > 0)
            //        //spreadDet.Columns[rollNo].Visible = true;
            //    if (regNo > 0)
            //        //spreadDet.Columns[regNo].Visible = true;
            //    if (admNo > 0)
            //        //spreadDet.Columns[admNo].Visible = true;
            //}
            //else if (roll == 2)
            //{
            //    if (rollNo > 0)
            //        //spreadDet.Columns[rollNo].Visible = true;
            //    if (regNo > 0)
            //        //spreadDet.Columns[regNo].Visible = false;
            //    if (admNo > 0)
            //       // spreadDet.Columns[admNo].Visible = false;

            //}
            //else if (roll == 3)
            //{
            //    if (rollNo > 0)
            //        //spreadDet.Columns[rollNo].Visible = false;
            //    if (regNo > 0)
            //        //spreadDet.Columns[regNo].Visible = true;
            //    if (admNo > 0)
            //        //spreadDet.Columns[admNo].Visible = false;
            //}
            //else if (roll == 4)
            //{
            //    if (rollNo > 0)
            //        //spreadDet.Columns[rollNo].Visible = false;
            //    if (regNo > 0)
            //        //spreadDet.Columns[regNo].Visible = false;
            //    if (admNo > 0)
            //       // spreadDet.Columns[admNo].Visible = true;
            //}
            //else if (roll == 5)
            //{
            //    if (rollNo > 0)
            //        spreadDet.Columns[rollNo].Visible = true;
            //    if (regNo > 0)
            //        spreadDet.Columns[regNo].Visible = true;
            //    if (admNo > 0)
            //        spreadDet.Columns[admNo].Visible = false;
            //}
            //else if (roll == 6)
            //{
            //    if (rollNo > 0)
            //        spreadDet.Columns[rollNo].Visible = false;
            //    if (regNo > 0)
            //        spreadDet.Columns[regNo].Visible = true;
            //    if (admNo > 0)
            //        spreadDet.Columns[admNo].Visible = true;
            //}
            //else if (roll == 7)
            //{
            //    if (rollNo > 0)
            //        spreadDet.Columns[rollNo].Visible = true;
            //    if (regNo > 0)
            //        spreadDet.Columns[regNo].Visible = false;
            //    if (admNo > 0)
            //        spreadDet.Columns[admNo].Visible = true;
            //}
            //#endregion
        }
        catch { }
    }

    #endregion

    //added by sudhagar 23.05.2017 cancel and discontinue
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
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("TC Apply", "6"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("TC Issued", "7"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Readmission", "8"));
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
        CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Include", "--Select--");
    }

    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Include", "--Select--");

    }

    #endregion

    //discontinue,delflag
    protected string getStudCategory(ref int TransVal)
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
            string tcapply="";
            string tcissued="";
            string readmit = "";
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
                        if (cblinclude.Items[i].Value == "6")
                            tcapply = " isnull(r.IsTcIssued,'0')=0 and r.tcapply=1";
                        if (cblinclude.Items[i].Value == "7")
                            tcissued = "r.IsTcIssued=1";
                        if (cblinclude.Items[i].Value == "8")
                            readmit = "r.app_no in(select app_no from Readmission)";//abarna
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
                if (tcapply != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "(isnull(r.IsTcIssued,'0')=0 and r.tcapply=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "isnull(r.IsTcIssued,'0')=0 and r.tcapply=1)";
                    }
                }
                if (tcissued != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "(r.IsTcIssued=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.IsTcIssued=1)";
                    }
                }
                if (readmit != "")//abarna
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "(r.app_no in(select app_no from Readmission))";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.app_no in(select app_no from Readmission))";
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
                //strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0 and isnull(r.ProlongAbsent,'0')=0";
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

    //added by sudhagar 31.07.2017
    protected void rblmode_Selected(object sender, EventArgs e)
    {
        txtexcelname.Text = string.Empty;
        grdInstWisePaidReport.Visible = false;
        print.Visible = false;
        divlabl.Visible = false;
        checkdicon.Checked = false;
        divcolorder.Attributes.Add("Style", "display:none;");
    }

    //02.08.2017
    /// <summary>
    /// others option included here like staff,vendor,others 02.08.2017
    /// </summary>
    /// 

    protected void rblMemType_Selected(object sender, EventArgs e)
    {
        bindheader();
        loadpaid();
        loadfinanceUser();
        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fromdate.Attributes.Add("readonly", "readonly");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Attributes.Add("readonly", "readonly");
        getPrintSettings();
        columnType();
        LoadIncludeSetting();
        getAcademicYear();
        txtexcelname.Text = string.Empty;
        grdInstWisePaidReport.Visible = false;
        print.Visible = false;
        divlabl.Visible = false;
        chklsfyear.Items.Clear();
        tdlblfnl.Visible = false;
        tdfnl.Visible = false;
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
        divTreeView.Visible = false;
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

    #endregion

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

    protected void grdInstWisePaidReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.BackColor = Color.FromArgb(12, 166, 202);
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Font.Bold = true;
            }
            else
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }

}