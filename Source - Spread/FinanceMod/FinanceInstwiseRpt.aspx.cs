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

public partial class FinanceInstwiseRpt : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static bool usBasedRights = false;
    static Dictionary<int, string> dicColumnAlignment = new Dictionary<int, string>();
    Dictionary<int, string> dicHeaderWise = new Dictionary<int, string>();
    int grdRow = 0;

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
                bindCollege();
                setLabelText();
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
                string query = " select distinct ledgername from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK =h.HeaderPK and   l.CollegeCode in('" + collegecode + "' ) and h.HeaderName in('" + headercode + "' )";
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

    //dataset 

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
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            hdText = Convert.ToString(getCblSelectedText(chkl_studhed));
            ldText = Convert.ToString(getCblSelectedText(chkl_studled));
            payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
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
            string strInclude = string.Empty;
            string strHdNameEx = string.Empty;
            string strhdNameGroupEx = string.Empty;
            if (rblmode.SelectedIndex == 0)
            {
                strHdName = " headerName as headerName";
                strhdNameGroup = " headerName";
                strOrderBy = " order by headerName";

                strHdNameEx = " headerName";//+'-'+('Excess/Advance') as headerName
                strhdNameGroupEx = " headerName";
            }
            else
            {
                strHdName = " headerName,ledgerName";
                strhdNameGroup = " headerName,ledgerName";
                strOrderBy = " order by headerName";

                strHdNameEx = " headerName,ledgerName";//+'-'+('Excess/Advance') as ledgerName
                strhdNameGroupEx = " headerName,ledgerName";
            }


            strInclude = getStudCategory();


            #endregion

            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText) && !string.IsNullOrEmpty(payMode))
            {
                string selFinYr = string.Empty;
                string selFinYrGrpBy = string.Empty;
                string finlYrStr = string.Empty;
                string finlYrStrEx = string.Empty;
                if (checkSchoolSetting() == 0)//school
                {
                    #region
                    selFinYr = " ,(select convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)) from fm_finyearmaster where finyearpk=isnull(actualfinyearfk,'0'))as actualfinyearfk";
                    selFinYrGrpBy = " ,actualfinyearfk";
                    finlYrStrEx = " ,''actualfinyearfk";

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
                    //  selCol = "f.paymode," + selectCol + ",f.app_no,isnull(f.transtype,'0') as transtype" + selFinYr + "";
                    // GrpselCol = "f.paymode," + groupStr + ",f.app_no,f.transtype,actualfinyearfk";
                    #endregion
                }
                #region Query
                string SelQ = string.Empty;
                if (rblMemType.SelectedIndex == 0)
                {
                    if (!cbAcdYear.Checked)
                    {
                        SelQ = " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,paymode,h.collegecode,isnull(transtype,'0') as transtype" + selFinYr + " from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,collinfo cl,registration r where f.app_no=r.app_no  and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=cl.college_code and h.collegecode in('" + collegecode + "') and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' " + strInclude + " " + finlYrStr + " group by " + strhdNameGroup + ",paymode,h.collegecode,transtype" + selFinYrGrpBy + " ";//and r.college_code=cl.college_code
                        //" + strOrderBy + "
                        SelQ += " union all select " + strHdNameEx + ",sum(isnull(exl.excessamt,'0')) as debit,'0' as credit,ex_paymode as paymode,h.collegecode,'1' as transtype" + finlYrStrEx + " from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1'  and h.collegecode in('" + collegecode + "') and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and ex.ex_paymode in('" + payMode + "') and excesstransdate between '" + fromdate + "' and '" + todate + "'  group by  " + strhdNameGroupEx + ",ex_paymode,h.collegecode  " + strOrderBy + "";
                    }
                    else
                    {
                        SelQ = " select " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,paymode,h.collegecode,isnull(transtype,'0') as transtype" + selFinYr + ",f.feecategory,r.batch_year from ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l,collinfo cl,registration r where f.app_no=r.app_no and r.college_code=cl.college_code and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.collegecode=cl.college_code and h.collegecode in('" + collegecode + "') and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' and isnull(iscanceled,'0')='0' " + strInclude + " " + finlYrStr + " group by " + strhdNameGroup + ",paymode,h.collegecode,transtype,f.feecategory,r.batch_year" + selFinYrGrpBy + " ";

                        SelQ += " union all select " + strHdNameEx + ",sum(isnull(exl.excessamt,'0')) as debit,'0' as credit,ex_paymode as paymode,h.collegecode,'1' as transtype" + finlYrStrEx + ",ex.feecategory,r.batch_year from ft_excessdet ex,ft_excessledgerdet exl,registration r,fm_headermaster h,fm_ledgermaster l where ex.app_no=r.app_no and ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and memtype='1'  and h.collegecode in('" + collegecode + "') and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and ex.ex_paymode in('" + payMode + "') and excesstransdate between '" + fromdate + "' and '" + todate + "'  group by  " + strhdNameGroupEx + ",ex_paymode,h.collegecode,ex.feecategory,r.batch_year  " + strOrderBy + "";
                    }
                    if (cbIncOthers.Checked)
                    {
                        if (checkSchoolSetting() == 0)//school
                        {
                            selFinYr = " ,(select convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)) from fm_finyearmaster where finyearpk=isnull(actualfinyearfk,'0'))as actualfinyearfk";
                            selFinYrGrpBy = " ,actualfinyearfk";
                        }
                        #region staff
                        SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,h.collegecode,isnull(transtype,'0') as transtype" + selFinYr + " from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK  and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2' ";
                        SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + strhdNameGroup + ",f.paymode,h.collegecode,transtype" + selFinYrGrpBy + "" + strOrderBy + "";
                        // SelQ += "  order by Transcode";                //and sm.college_code in('" + collegecode + "')        
                        #endregion

                        #region Vendor
                        SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,h.collegecode,isnull(transtype,'0') as transtype" + selFinYr + " from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f ,fm_headermaster h,FM_LedgerMaster l where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3' ";
                        SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + strhdNameGroup + ",f.paymode,h.collegecode,transtype" + selFinYrGrpBy + "" + strOrderBy + "";
                        // SelQ += "  order by Transcode";
                        #endregion

                        #region other
                        //other detail
                        SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,h.collegecode,isnull(transtype,'0') as transtype" + selFinYr + " from CO_VendorMaster vm,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4' ";
                        SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += "group by " + strhdNameGroup + ",f.paymode,h.collegecode,transtype" + selFinYrGrpBy + "" + strOrderBy + "";
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
                        SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,h.collegecode from staffmaster sm,staff_appl_master sa,stafftrans st,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where sm.appl_no=sa.appl_no and  sm.staff_code=st.staff_code and latestrec='1' and sm.college_code=sa.college_code and sa.appl_id=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and sm.college_code in('" + collegecode + "') and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='2' " + strMemtypeValue + " ";
                        if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + strhdNameGroup + ",f.paymode,h.collegecode" + strOrderBy + "";
                        // SelQ += "  order by Transcode";                       
                        #endregion
                    }
                    if ((totSelcount != 1 && selectedName.Contains("Vendor")) || (totSelcount == 1 && memName == "Vendor"))
                    {
                        #region Vendor
                        SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,h.collegecode from CO_VendorMaster vm,IM_VendorContactMaster vc,ft_findailytransaction f ,fm_headermaster h,FM_LedgerMaster l where vm.vendorpk=vc.vendorfk and VendorType =1 and vc.VendorContactPK=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='3' " + strMemtypeValue + " ";
                        if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += " group by " + strhdNameGroup + ",f.paymode,h.collegecode" + strOrderBy + "";
                        // SelQ += "  order by Transcode";


                        #endregion
                    }
                    if ((totSelcount != 1 && selectedName.Contains("Others")) || (totSelcount == 1 && memName == "Others"))
                    {
                        #region other
                        //other detail
                        SelQ += " select  " + strHdName + ",sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,f.paymode,h.collegecode from CO_VendorMaster vm,ft_findailytransaction f,fm_headermaster h,FM_LedgerMaster l where  VendorType ='-5' and vm.vendorpk=f.app_no and h.headerpk=f.headerfk and l.HeaderFK =h.HeaderPK and l.LedgerPK =f.LedgerFK and h.headername in('" + hdText + "') and  l.ledgername in('" + ldText + "') and f.paymode in('" + payMode + "')   and isnull(iscanceled,'0')='0' and transcode<>'' and memtype='4' " + strMemtypeValue + " ";
                        if (string.IsNullOrEmpty(strMemtypeValue))
                            SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'";
                        SelQ += "group by " + strhdNameGroup + ",f.paymode,h.collegecode" + strOrderBy + "";

                        #endregion
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

    //college data table binding here

    protected DataTable loaddetails(DataSet ds, ref Hashtable htpayMode, ref Dictionary<string, string> htClgAcr)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            Dictionary<string, string> arTrans = getTransType();
            Dictionary<string, string> reportType = getreportType();
            Hashtable htsubtotal = new Hashtable();
            Hashtable htfnltotal = new Hashtable();
            dtpaid = getTableClgAcr(collegecode, ref htClgAcr);//columns added
            DataRow drpaid = dtpaid.NewRow();
            ArrayList arMemType = getMemType();

            if (dtpaid.Columns.Count > 0)
            {
                int tblFirst = 0;
                foreach (string memType in arMemType)
                {
                    string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                    foreach (KeyValuePair<string, string> trans in arTrans)
                    {
                        #region
                        bool boolrptType = false;
                        foreach (KeyValuePair<string, string> dtVal in reportType)
                        {
                            bool boolrptName = false;
                            string rptValue = Convert.ToString(dtVal.Key) + "~" + trans.Key;
                            string reptfltVal = Convert.ToString(dtVal.Value);
                            #region
                            for (int pay = 0; pay < chkl_paid.Items.Count; pay++)
                            {
                                if (chkl_paid.Items[pay].Selected)
                                {
                                    bool boolpayText = false;
                                    string payModeText = Convert.ToString(chkl_paid.Items[pay].Text);
                                    if (!htpayMode.ContainsKey(chkl_paid.Items[pay].Value))
                                        htpayMode.Add(chkl_paid.Items[pay].Value, chkl_paid.Items[pay].Text);
                                    for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                    {
                                        if (chkl_studhed.Items[hd].Selected)
                                        {
                                            bool boolHeader = false;
                                            double hdfnlTotal = 0;
                                            string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                            for (int clg = 0; clg < cblclg.Items.Count; clg++)
                                            {
                                                int rowCnt = 0;
                                                if (cblclg.Items[clg].Selected)
                                                {
                                                    #region college
                                                    double paidAmt = 0;
                                                    string clgCode = Convert.ToString(cblclg.Items[clg].Value);
                                                    //dividing credit and debit values                                           
                                                    string typeName = string.Empty;
                                                    string strflter = "paymode='" + chkl_paid.Items[pay].Value + "' and headername='" + hdName + "' and collegecode='" + clgCode + "' and transtype='" + trans.Value + "'";
                                                    //   ds.Tables[0].DefaultView.RowFilter = strflter;
                                                    //DataView dvpaid = new DataView();
                                                    DataTable dvpaid = new DataTable();
                                                    if (!boolrptType)
                                                    {
                                                        typeName = "Debit";
                                                        DataTable temptabledt = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "debit", "paymode", "collegecode", "transtype");
                                                        temptabledt.DefaultView.RowFilter = strflter + reptfltVal;
                                                        dvpaid = temptabledt.DefaultView.ToTable();
                                                    }
                                                    else
                                                    {
                                                        typeName = "Credit";
                                                        DataTable temptablecr = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "credit", "paymode", "collegecode", "transtype");
                                                        temptablecr.DefaultView.RowFilter = strflter + reptfltVal;
                                                        dvpaid = temptablecr.DefaultView.ToTable();
                                                    }
                                                    if (dvpaid.Rows.Count > 0)
                                                    {
                                                        if (!boolrptName)//receipt type
                                                        {
                                                            drpaid = dtpaid.NewRow();
                                                            drpaid["Sno"] = strMemType + "~" + rptValue + "@" + "Type";
                                                            dtpaid.Rows.Add(drpaid);
                                                            boolrptName = true;
                                                        }
                                                        if (!boolpayText)//paymode text added
                                                        {
                                                            drpaid = dtpaid.NewRow();
                                                            drpaid["Sno"] = payModeText + "#" + "Mode";
                                                            dtpaid.Rows.Add(drpaid);
                                                            boolpayText = true;
                                                        }
                                                        if (!boolHeader)
                                                        {
                                                            drpaid = dtpaid.NewRow();
                                                            boolHeader = true;
                                                            rowCnt++;
                                                            drpaid["Sno"] = Convert.ToString(rowCnt);
                                                            drpaid["Header"] = hdName;
                                                        }
                                                        string collName = Convert.ToString(htClgAcr[clgCode]);
                                                        double.TryParse(Convert.ToString(dvpaid.Compute("sum(" + typeName + ")", "")), out paidAmt);
                                                        //double.TryParse(Convert.ToString(dvpaid.Rows[0][typeName]), out paidAmt);

                                                        drpaid[collName] = Convert.ToString(paidAmt);
                                                        hdfnlTotal += paidAmt;//headerwise final total
                                                        if (!htsubtotal.ContainsKey(collName))
                                                            htsubtotal.Add(collName, paidAmt);
                                                        else
                                                        {
                                                            double amount = 0;
                                                            double.TryParse(Convert.ToString(htsubtotal[collName]), out amount);
                                                            amount += paidAmt;
                                                            htsubtotal.Remove(collName);
                                                            htsubtotal.Add(collName, Convert.ToString(amount));
                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                            //end of the header total
                                            if (hdfnlTotal != 0)
                                            {
                                                drpaid["Total"] = Convert.ToString(hdfnlTotal);
                                                dtpaid.Rows.Add(drpaid);
                                            }
                                        }
                                    }
                                    //paymode wise sub total
                                    if (htsubtotal.Count > 0)
                                    {
                                        double fnltempAmt = 0;
                                        drpaid = dtpaid.NewRow();
                                        drpaid["Sno"] = "Total" + "-" + "*";
                                        foreach (DictionaryEntry htRow in htsubtotal)
                                        {
                                            double tempAmt = 0;
                                            double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                            drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                            fnltempAmt += tempAmt;
                                            if (!htfnltotal.ContainsKey(htRow.Key))
                                                htfnltotal.Add(htRow.Key, tempAmt);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htfnltotal[htRow.Key]), out amount);
                                                amount += tempAmt;
                                                htfnltotal.Remove(htRow.Key);
                                                htfnltotal.Add(htRow.Key, Convert.ToString(amount));
                                            }
                                        }
                                        drpaid["Total"] = Convert.ToString(fnltempAmt);
                                        dtpaid.Rows.Add(drpaid);
                                        if (!htfnltotal.ContainsKey("Total"))
                                            htfnltotal.Add("Total", fnltempAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htfnltotal["Total"]), out amount);
                                            amount += fnltempAmt;
                                            htfnltotal.Remove("Total");
                                            htfnltotal.Add("Total", Convert.ToString(amount));
                                        }
                                        htsubtotal.Clear();
                                    }
                                }
                            }
                            if (htfnltotal.Count > 0)
                            {
                                double fnltempAmt = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = "Grand Total" + "-" + "*";
                                foreach (DictionaryEntry htRow in htfnltotal)
                                {
                                    if (Convert.ToString(htRow.Key) != "Total")
                                    {
                                        double tempAmt = 0;
                                        double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                        drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                        fnltempAmt += tempAmt;
                                    }
                                }
                                drpaid["Total"] = Convert.ToString(fnltempAmt);
                                dtpaid.Rows.Add(drpaid);
                                htfnltotal.Clear();
                            }
                            #endregion
                            boolrptType = true;
                        }
                        #endregion
                    }
                    tblFirst += 1;
                }
            }
        }
        catch { }
        return dtpaid;
    }

    protected DataTable loaddetailsLedger(DataSet ds, ref Hashtable htpayMode, ref Dictionary<string, string> htClgAcr)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            Dictionary<string, string> reportType = getreportType();
            Dictionary<string, string> arTrans = getTransType();
            Hashtable htsubtotal = new Hashtable();
            Hashtable htfnltotal = new Hashtable();
            Hashtable hthdtotal = new Hashtable();
            dtpaid = getTableClgAcr(collegecode, ref htClgAcr);//columns added
            DataRow drpaid = dtpaid.NewRow();
            ArrayList arMemType = getMemType();
            if (dtpaid.Columns.Count > 0)
            {
                int tblFirst = 0;
                foreach (string memType in arMemType)
                {
                    string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                    foreach (KeyValuePair<string, string> trans in arTrans)
                    {
                        #region
                        bool boolrptType = false;
                        foreach (KeyValuePair<string, string> dtVal in reportType)
                        {
                            bool boolrptName = false;
                            string rptValue = Convert.ToString(dtVal.Key) + "~" + trans.Key;
                            string reptfltVal = Convert.ToString(dtVal.Value);
                            #region
                            for (int pay = 0; pay < chkl_paid.Items.Count; pay++)
                            {
                                if (chkl_paid.Items[pay].Selected)
                                {
                                    bool boolpayText = false;
                                    string payModeText = Convert.ToString(chkl_paid.Items[pay].Text);
                                    if (!htpayMode.ContainsKey(chkl_paid.Items[pay].Value))
                                        htpayMode.Add(chkl_paid.Items[pay].Value, chkl_paid.Items[pay].Text);
                                    for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                    {
                                        bool boolHeaderName = false;
                                        if (chkl_studhed.Items[pay].Selected)
                                        {

                                            DataView dvpaids = new DataView();

                                            string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                            if (!boolrptType)
                                            {
                                                DataTable temptabledt = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "ledgername", "transtype");
                                                temptabledt.DefaultView.RowFilter = "headername='" + hdName + "' and transtype='" + trans.Value + "'";
                                                dvpaids = temptabledt.DefaultView;
                                            }
                                            else
                                            {
                                                DataTable temptablecr = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "ledgername", "transtype");
                                                temptablecr.DefaultView.RowFilter = "headername='" + hdName + "' and transtype='" + trans.Value + "'";
                                                dvpaids = temptablecr.DefaultView;
                                            }
                                            if (dvpaids.Count > 0)
                                            {
                                                for (int pd = 0; pd < dvpaids.Count; pd++)
                                                {
                                                    bool boolHeader = false;
                                                    double hdfnlTotal = 0;
                                                    for (int clg = 0; clg < cblclg.Items.Count; clg++)
                                                    {
                                                        int rowCnt = 0;
                                                        if (cblclg.Items[clg].Selected)
                                                        {
                                                            #region college
                                                            double paidAmt = 0;
                                                            string clgCode = Convert.ToString(cblclg.Items[clg].Value);
                                                            //dividing credit and debit values                                           
                                                            string typeName = string.Empty;
                                                            string strflter = "paymode='" + chkl_paid.Items[pay].Value + "' and ledgerName='" + dvpaids[pd]["ledgername"] + "' and collegecode='" + clgCode + "' and transtype='" + trans.Value + "' ";
                                                            //   ds.Tables[0].DefaultView.RowFilter = strflter;
                                                            //DataView dvpaid = new DataView();
                                                            DataTable dvpaid = new DataTable();
                                                            if (!boolrptType)
                                                            {
                                                                typeName = "Debit";
                                                                DataTable temptabledt = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "ledgername", "debit", "paymode", "collegecode", "transtype");
                                                                // DataTable temptabledt = dvpaids.ToTable();
                                                                temptabledt.DefaultView.RowFilter = strflter + reptfltVal;
                                                                dvpaid = temptabledt.DefaultView.ToTable();
                                                            }
                                                            else
                                                            {
                                                                typeName = "Credit";
                                                                DataTable temptablecr = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "ledgername", "credit", "paymode", "collegecode", "transtype");
                                                                // DataTable temptablecr = dvpaids.ToTable();
                                                                temptablecr.DefaultView.RowFilter = strflter + reptfltVal;
                                                                dvpaid = temptablecr.DefaultView.ToTable();
                                                            }
                                                            if (dvpaid.Rows.Count > 0)
                                                            {
                                                                if (!boolrptName)//receipt type
                                                                {
                                                                    drpaid = dtpaid.NewRow();
                                                                    drpaid["Sno"] = strMemType + "~" + rptValue + "@" + "Type";
                                                                    dtpaid.Rows.Add(drpaid);
                                                                    boolrptName = true;
                                                                }
                                                                if (!boolpayText)//paymode text added
                                                                {
                                                                    drpaid = dtpaid.NewRow();
                                                                    drpaid["Sno"] = payModeText + "#" + "Mode";
                                                                    dtpaid.Rows.Add(drpaid);
                                                                    boolpayText = true;
                                                                }
                                                                if (!boolHeaderName)//headerName bind to data table
                                                                {
                                                                    drpaid = dtpaid.NewRow();
                                                                    drpaid["Sno"] = hdName + "!" + "HeaderName";
                                                                    dtpaid.Rows.Add(drpaid);
                                                                    boolHeaderName = true;
                                                                }
                                                                if (!boolHeader)
                                                                {
                                                                    drpaid = dtpaid.NewRow();
                                                                    boolHeader = true;
                                                                    rowCnt++;
                                                                    drpaid["Sno"] = Convert.ToString(rowCnt);
                                                                    drpaid["Header"] = dvpaid.Rows[0]["LedgerName"];
                                                                }
                                                                string collName = Convert.ToString(htClgAcr[clgCode]);
                                                                double.TryParse(Convert.ToString(dvpaid.Compute("sum(" + typeName + ")", "")), out paidAmt);
                                                                //double.TryParse(Convert.ToString(dvpaid[0][typeName]), out paidAmt);
                                                                drpaid[collName] = Convert.ToString(paidAmt);
                                                                hdfnlTotal += paidAmt;//headerwise final total
                                                                if (!htsubtotal.ContainsKey(collName))
                                                                    htsubtotal.Add(collName, paidAmt);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(htsubtotal[collName]), out amount);
                                                                    amount += paidAmt;
                                                                    htsubtotal.Remove(collName);
                                                                    htsubtotal.Add(collName, Convert.ToString(amount));
                                                                }
                                                                if (!hthdtotal.ContainsKey(collName))
                                                                    hthdtotal.Add(collName, paidAmt);
                                                                else
                                                                {
                                                                    double amount = 0;
                                                                    double.TryParse(Convert.ToString(hthdtotal[collName]), out amount);
                                                                    amount += paidAmt;
                                                                    hthdtotal.Remove(collName);
                                                                    hthdtotal.Add(collName, Convert.ToString(amount));
                                                                }

                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                    //end of the header total
                                                    if (hdfnlTotal != 0)
                                                    {
                                                        drpaid["Total"] = Convert.ToString(hdfnlTotal);
                                                        dtpaid.Rows.Add(drpaid);
                                                    }
                                                }
                                                ////end of the header total
                                                //if (hdfnlTotal != 0)
                                                //{
                                                //    drpaid["Total"] = Convert.ToString(hdfnlTotal);
                                                //    dtpaid.Rows.Add(drpaid);
                                                //}
                                            }
                                        }
                                        //every header end total
                                        if (hthdtotal.Count > 0)
                                        {
                                            double fnltempAmt = 0;
                                            drpaid = dtpaid.NewRow();
                                            drpaid["Sno"] = "Header Total" + "-" + "*";
                                            foreach (DictionaryEntry htRow in hthdtotal)
                                            {
                                                double tempAmt = 0;
                                                double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                                drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                                fnltempAmt += tempAmt;
                                            }
                                            drpaid["Total"] = Convert.ToString(fnltempAmt);
                                            dtpaid.Rows.Add(drpaid);
                                            hthdtotal.Clear();
                                        }
                                    }
                                    //paymode wise sub total
                                    if (htsubtotal.Count > 0)
                                    {
                                        double fnltempAmt = 0;
                                        drpaid = dtpaid.NewRow();
                                        drpaid["Sno"] = "Total" + "-" + "*";
                                        foreach (DictionaryEntry htRow in htsubtotal)
                                        {
                                            double tempAmt = 0;
                                            double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                            drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                            fnltempAmt += tempAmt;
                                            if (!htfnltotal.ContainsKey(htRow.Key))
                                                htfnltotal.Add(htRow.Key, tempAmt);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htfnltotal[htRow.Key]), out amount);
                                                amount += tempAmt;
                                                htfnltotal.Remove(htRow.Key);
                                                htfnltotal.Add(htRow.Key, Convert.ToString(amount));
                                            }
                                        }
                                        drpaid["Total"] = Convert.ToString(fnltempAmt);
                                        dtpaid.Rows.Add(drpaid);
                                        if (!htfnltotal.ContainsKey("Total"))
                                            htfnltotal.Add("Total", fnltempAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htfnltotal["Total"]), out amount);
                                            amount += fnltempAmt;
                                            htfnltotal.Remove("Total");
                                            htfnltotal.Add("Total", Convert.ToString(amount));
                                        }
                                        htsubtotal.Clear();
                                    }
                                }
                            }
                            if (htfnltotal.Count > 0)
                            {
                                double fnltempAmt = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = "Grand Total" + "-" + "*";
                                foreach (DictionaryEntry htRow in htfnltotal)
                                {
                                    if (Convert.ToString(htRow.Key) != "Total")
                                    {
                                        double tempAmt = 0;
                                        double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                        drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                        fnltempAmt += tempAmt;
                                    }
                                }
                                drpaid["Total"] = Convert.ToString(fnltempAmt);
                                dtpaid.Rows.Add(drpaid);
                                htfnltotal.Clear();
                            }
                            #endregion
                            boolrptType = true;
                        }
                        #endregion
                    }
                    tblFirst += 1;
                }
            }
        }
        catch { }
        return dtpaid;
    }

    //school data table binding here

    protected DataTable loaddetailsSchool(DataSet ds, ref Hashtable htpayMode, ref Dictionary<string, string> htClgAcr)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            Dictionary<string, string> reportType = getreportType();
            Hashtable htsubtotal = new Hashtable();
            Hashtable htfnltotal = new Hashtable();
            ArrayList arFnlYear = getSelFinlDate();
            dtpaid = getTableClgAcr(collegecode, ref htClgAcr);//columns added
            DataRow drpaid = dtpaid.NewRow();
            ArrayList arMemType = getMemType();
            if (dtpaid.Columns.Count > 0)
            {
                int tblFirst = 0;
                foreach (string memType in arMemType)
                {
                    string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                    foreach (string fnlYear in arFnlYear)
                    {
                        #region
                        bool boolrptType = false;
                        foreach (KeyValuePair<string, string> dtVal in reportType)
                        {
                            bool boolrptName = false;
                            string rptValue = Convert.ToString(dtVal.Key);
                            string reptfltVal = Convert.ToString(dtVal.Value);
                            #region
                            for (int pay = 0; pay < chkl_paid.Items.Count; pay++)
                            {
                                if (!chkl_paid.Items[pay].Selected)
                                    continue;
                                bool boolpayText = false;
                                string payModeText = Convert.ToString(chkl_paid.Items[pay].Text);
                                if (!htpayMode.ContainsKey(chkl_paid.Items[pay].Value))
                                    htpayMode.Add(chkl_paid.Items[pay].Value, chkl_paid.Items[pay].Text);
                                for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                {
                                    if (!chkl_studhed.Items[pay].Selected)
                                        continue;
                                    bool boolHeader = false;
                                    double hdfnlTotal = 0;
                                    string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                    for (int clg = 0; clg < cblclg.Items.Count; clg++)
                                    {
                                        int rowCnt = 0;
                                        if (!cblclg.Items[clg].Selected)
                                            continue;
                                        #region college
                                        double paidAmt = 0;
                                        string clgCode = Convert.ToString(cblclg.Items[clg].Value);
                                        //dividing credit and debit values                                           
                                        string typeName = string.Empty;
                                        string strflter = "paymode='" + chkl_paid.Items[pay].Value + "' and headername='" + hdName + "' and collegecode='" + clgCode + "' and actualfinyearfk='" + fnlYear + "'";
                                        //   ds.Tables[0].DefaultView.RowFilter = strflter;
                                        // DataView dvpaid = new DataView();
                                        DataTable dvpaid = new DataTable();
                                        if (!boolrptType)
                                        {
                                            typeName = "Debit";
                                            DataTable temptabledt = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "debit", "paymode", "collegecode", "actualfinyearfk");
                                            temptabledt.DefaultView.RowFilter = strflter + reptfltVal;
                                            dvpaid = temptabledt.DefaultView.ToTable();
                                        }
                                        else
                                        {
                                            typeName = "Credit";
                                            DataTable temptablecr = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "credit", "paymode", "collegecode", "actualfinyearfk");
                                            temptablecr.DefaultView.RowFilter = strflter + reptfltVal;
                                            dvpaid = temptablecr.DefaultView.ToTable();
                                        }
                                        if (dvpaid.Rows.Count > 0)
                                        {
                                            if (!boolrptName)//receipt type
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = strMemType + "~" + fnlYear + "~" + rptValue + "@" + "Type";
                                                dtpaid.Rows.Add(drpaid);
                                                boolrptName = true;
                                            }
                                            if (!boolpayText)//paymode text added
                                            {
                                                drpaid = dtpaid.NewRow();
                                                drpaid["Sno"] = payModeText + "#" + "Mode";
                                                dtpaid.Rows.Add(drpaid);
                                                boolpayText = true;
                                            }
                                            if (!boolHeader)
                                            {
                                                drpaid = dtpaid.NewRow();
                                                boolHeader = true;
                                                rowCnt++;
                                                drpaid["Sno"] = Convert.ToString(rowCnt);
                                                drpaid["Header"] = hdName;
                                            }
                                            string collName = Convert.ToString(htClgAcr[clgCode]);
                                            double.TryParse(Convert.ToString(dvpaid.Compute("sum(" + typeName + ")", "")), out paidAmt);
                                            // double.TryParse(Convert.ToString(dvpaid[0][typeName]), out paidAmt);
                                            drpaid[collName] = Convert.ToString(paidAmt);
                                            hdfnlTotal += paidAmt;//headerwise final total
                                            if (!htsubtotal.ContainsKey(collName))
                                                htsubtotal.Add(collName, paidAmt);
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htsubtotal[collName]), out amount);
                                                amount += paidAmt;
                                                htsubtotal.Remove(collName);
                                                htsubtotal.Add(collName, Convert.ToString(amount));
                                            }
                                        }
                                        #endregion
                                    }
                                    //end of the header total
                                    if (hdfnlTotal != 0)
                                    {
                                        drpaid["Total"] = Convert.ToString(hdfnlTotal);
                                        dtpaid.Rows.Add(drpaid);
                                    }
                                }
                                //paymode wise sub total
                                if (htsubtotal.Count > 0)
                                {
                                    #region
                                    double fnltempAmt = 0;
                                    drpaid = dtpaid.NewRow();
                                    drpaid["Sno"] = "Total" + "-" + "*";
                                    foreach (DictionaryEntry htRow in htsubtotal)
                                    {
                                        double tempAmt = 0;
                                        double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                        drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                        fnltempAmt += tempAmt;
                                        if (!htfnltotal.ContainsKey(htRow.Key))
                                            htfnltotal.Add(htRow.Key, tempAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htfnltotal[htRow.Key]), out amount);
                                            amount += tempAmt;
                                            htfnltotal.Remove(htRow.Key);
                                            htfnltotal.Add(htRow.Key, Convert.ToString(amount));
                                        }
                                    }
                                    drpaid["Total"] = Convert.ToString(fnltempAmt);
                                    dtpaid.Rows.Add(drpaid);
                                    if (!htfnltotal.ContainsKey("Total"))
                                        htfnltotal.Add("Total", fnltempAmt);
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htfnltotal["Total"]), out amount);
                                        amount += fnltempAmt;
                                        htfnltotal.Remove("Total");
                                        htfnltotal.Add("Total", Convert.ToString(amount));
                                    }
                                    htsubtotal.Clear();
                                    #endregion
                                }
                            }
                            if (htfnltotal.Count > 0)
                            {
                                #region
                                double fnltempAmt = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = "Grand Total" + "-" + "*";
                                foreach (DictionaryEntry htRow in htfnltotal)
                                {
                                    if (Convert.ToString(htRow.Key) != "Total")
                                    {
                                        double tempAmt = 0;
                                        double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                        drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                        fnltempAmt += tempAmt;
                                    }
                                }
                                drpaid["Total"] = Convert.ToString(fnltempAmt);
                                dtpaid.Rows.Add(drpaid);
                                htfnltotal.Clear();
                                #endregion
                            }
                            #endregion
                            boolrptType = true;
                        }
                        #endregion
                    }
                    tblFirst += 1;
                }
            }
        }
        catch { }
        return dtpaid;
    }

    protected DataTable loaddetailsLedgerSchool(DataSet ds, ref Hashtable htpayMode, ref Dictionary<string, string> htClgAcr)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            Dictionary<string, string> reportType = getreportType();
            Hashtable htsubtotal = new Hashtable();
            Hashtable htfnltotal = new Hashtable();
            Hashtable hthdtotal = new Hashtable();
            ArrayList arFnlYear = getSelFinlDate();
            dtpaid = getTableClgAcr(collegecode, ref htClgAcr);//columns added
            DataRow drpaid = dtpaid.NewRow();
            ArrayList arMemType = getMemType();
            if (dtpaid.Columns.Count > 0)
            {
                int tblFirst = 0;
                foreach (string memType in arMemType)
                {
                    string strMemType = memType == "1" ? "Student" : memType == "2" ? "Staff" : memType == "3" ? "Vendor" : memType == "4" ? "Other" : "";
                    foreach (string fnlYear in arFnlYear)
                    {
                        #region
                        bool boolrptType = false;
                        foreach (KeyValuePair<string, string> dtVal in reportType)
                        {
                            bool boolrptName = false;
                            string rptValue = Convert.ToString(dtVal.Key);
                            string reptfltVal = Convert.ToString(dtVal.Value);
                            #region
                            for (int pay = 0; pay < chkl_paid.Items.Count; pay++)
                            {
                                if (!chkl_paid.Items[pay].Selected)
                                    continue;
                                bool boolpayText = false;
                                string payModeText = Convert.ToString(chkl_paid.Items[pay].Text);
                                if (!htpayMode.ContainsKey(chkl_paid.Items[pay].Value))
                                    htpayMode.Add(chkl_paid.Items[pay].Value, chkl_paid.Items[pay].Text);
                                for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                {
                                    bool boolHeaderName = false;
                                    if (!chkl_studhed.Items[pay].Selected)
                                        continue;
                                    DataView dvpaids = new DataView();
                                    string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                    if (!boolrptType)
                                    {
                                        DataTable temptabledt = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "ledgername", "actualfinyearfk");
                                        temptabledt.DefaultView.RowFilter = "headername='" + hdName + "' and actualfinyearfk='" + fnlYear + "'";
                                        dvpaids = temptabledt.DefaultView;
                                    }
                                    else
                                    {
                                        DataTable temptablecr = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "ledgername", "actualfinyearfk");
                                        temptablecr.DefaultView.RowFilter = "headername='" + hdName + "' and actualfinyearfk='" + fnlYear + "'";
                                        dvpaids = temptablecr.DefaultView;
                                    }
                                    if (dvpaids.Count > 0)
                                    {
                                        for (int pd = 0; pd < dvpaids.Count; pd++)
                                        {
                                            bool boolHeader = false;
                                            double hdfnlTotal = 0;
                                            for (int clg = 0; clg < cblclg.Items.Count; clg++)
                                            {
                                                int rowCnt = 0;
                                                if (!cblclg.Items[clg].Selected)
                                                    continue;
                                                #region college
                                                double paidAmt = 0;
                                                string clgCode = Convert.ToString(cblclg.Items[clg].Value);
                                                //dividing credit and debit values                                           
                                                string typeName = string.Empty;
                                                string strflter = "paymode='" + chkl_paid.Items[pay].Value + "' and ledgerName='" + dvpaids[pd]["ledgername"] + "' and collegecode='" + clgCode + "' and actualfinyearfk='" + fnlYear + "'";
                                                //   ds.Tables[0].DefaultView.RowFilter = strflter;
                                                // DataView dvpaid = new DataView();
                                                DataTable dvpaid = new DataTable();
                                                if (!boolrptType)
                                                {
                                                    typeName = "Debit";
                                                    DataTable temptabledt = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "ledgername", "debit", "paymode", "collegecode", "actualfinyearfk");
                                                    // DataTable temptabledt = dvpaids.ToTable();
                                                    temptabledt.DefaultView.RowFilter = strflter + reptfltVal;
                                                    dvpaid = temptabledt.DefaultView.ToTable();
                                                }
                                                else
                                                {
                                                    typeName = "Credit";
                                                    DataTable temptablecr = ds.Tables[tblFirst].DefaultView.ToTable(true, "headerName", "ledgername", "credit", "paymode", "collegecode", "actualfinyearfk");
                                                    // DataTable temptablecr = dvpaids.ToTable();
                                                    temptablecr.DefaultView.RowFilter = strflter + reptfltVal;
                                                    dvpaid = temptablecr.DefaultView.ToTable();
                                                }
                                                if (dvpaid.Rows.Count > 0)
                                                {
                                                    if (!boolrptName)//receipt type
                                                    {
                                                        drpaid = dtpaid.NewRow();
                                                        drpaid["Sno"] = strMemType + "~" + fnlYear + "~" + rptValue + "@" + "Type";
                                                        dtpaid.Rows.Add(drpaid);
                                                        boolrptName = true;
                                                    }
                                                    if (!boolpayText)//paymode text added
                                                    {
                                                        drpaid = dtpaid.NewRow();
                                                        drpaid["Sno"] = payModeText + "#" + "Mode";
                                                        dtpaid.Rows.Add(drpaid);
                                                        boolpayText = true;
                                                    }
                                                    if (!boolHeaderName)//headerName bind to data table
                                                    {
                                                        drpaid = dtpaid.NewRow();
                                                        drpaid["Sno"] = hdName + "!" + "HeaderName";
                                                        dtpaid.Rows.Add(drpaid);
                                                        boolHeaderName = true;
                                                    }
                                                    if (!boolHeader)
                                                    {
                                                        drpaid = dtpaid.NewRow();
                                                        boolHeader = true;
                                                        rowCnt++;
                                                        drpaid["Sno"] = Convert.ToString(rowCnt);
                                                        drpaid["Header"] = dvpaid.Rows[0]["LedgerName"];
                                                    }
                                                    string collName = Convert.ToString(htClgAcr[clgCode]);
                                                    //double.TryParse(Convert.ToString(dvpaid[0][typeName]), out paidAmt);
                                                    double.TryParse(Convert.ToString(dvpaid.Compute("sum(" + typeName + ")", "")), out paidAmt);
                                                    drpaid[collName] = Convert.ToString(paidAmt);
                                                    hdfnlTotal += paidAmt;//headerwise final total
                                                    if (!htsubtotal.ContainsKey(collName))
                                                        htsubtotal.Add(collName, paidAmt);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htsubtotal[collName]), out amount);
                                                        amount += paidAmt;
                                                        htsubtotal.Remove(collName);
                                                        htsubtotal.Add(collName, Convert.ToString(amount));
                                                    }
                                                    if (!hthdtotal.ContainsKey(collName))
                                                        hthdtotal.Add(collName, paidAmt);
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(hthdtotal[collName]), out amount);
                                                        amount += paidAmt;
                                                        hthdtotal.Remove(collName);
                                                        hthdtotal.Add(collName, Convert.ToString(amount));
                                                    }

                                                }
                                                #endregion
                                            }
                                            //end of the header total
                                            if (hdfnlTotal != 0)
                                            {
                                                drpaid["Total"] = Convert.ToString(hdfnlTotal);
                                                dtpaid.Rows.Add(drpaid);
                                            }
                                        }
                                    }
                                    //every header end total
                                    if (hthdtotal.Count > 0)
                                    {
                                        double fnltempAmt = 0;
                                        drpaid = dtpaid.NewRow();
                                        drpaid["Sno"] = "Header Total" + "-" + "*";
                                        foreach (DictionaryEntry htRow in hthdtotal)
                                        {
                                            double tempAmt = 0;
                                            double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                            drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                            fnltempAmt += tempAmt;
                                        }
                                        drpaid["Total"] = Convert.ToString(fnltempAmt);
                                        dtpaid.Rows.Add(drpaid);
                                        hthdtotal.Clear();
                                    }
                                }
                                //paymode wise sub total
                                if (htsubtotal.Count > 0)
                                {
                                    #region
                                    double fnltempAmt = 0;
                                    drpaid = dtpaid.NewRow();
                                    drpaid["Sno"] = "Total" + "-" + "*";
                                    foreach (DictionaryEntry htRow in htsubtotal)
                                    {
                                        double tempAmt = 0;
                                        double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                        drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                        fnltempAmt += tempAmt;
                                        if (!htfnltotal.ContainsKey(htRow.Key))
                                            htfnltotal.Add(htRow.Key, tempAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htfnltotal[htRow.Key]), out amount);
                                            amount += tempAmt;
                                            htfnltotal.Remove(htRow.Key);
                                            htfnltotal.Add(htRow.Key, Convert.ToString(amount));
                                        }
                                    }
                                    drpaid["Total"] = Convert.ToString(fnltempAmt);
                                    dtpaid.Rows.Add(drpaid);
                                    if (!htfnltotal.ContainsKey("Total"))
                                        htfnltotal.Add("Total", fnltempAmt);
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htfnltotal["Total"]), out amount);
                                        amount += fnltempAmt;
                                        htfnltotal.Remove("Total");
                                        htfnltotal.Add("Total", Convert.ToString(amount));
                                    }
                                    htsubtotal.Clear();
                                    #endregion
                                }

                            }
                            if (htfnltotal.Count > 0)
                            {
                                double fnltempAmt = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = "Grand Total" + "-" + "*";
                                foreach (DictionaryEntry htRow in htfnltotal)
                                {
                                    if (Convert.ToString(htRow.Key) != "Total")
                                    {
                                        double tempAmt = 0;
                                        double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                        drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                        fnltempAmt += tempAmt;
                                    }
                                }
                                drpaid["Total"] = Convert.ToString(fnltempAmt);
                                dtpaid.Rows.Add(drpaid);
                                htfnltotal.Clear();
                            }
                            #endregion
                            boolrptType = true;
                        }
                        #endregion
                    }
                    tblFirst += 1;
                }
            }
        }
        catch { }
        return dtpaid;
    }

    protected DataTable getTableClgAcr(string collegecode, ref Dictionary<string, string> htClgAcr)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            dtpaid.Columns.Add("Sno");
            dtpaid.Columns.Add("Header");
            string selQ = " select collname,college_code,coll_acronymn as acr from collinfo where college_code in('" + collegecode + "') order by college_code";
            DataSet dsclg = d2.select_method_wo_parameter(selQ, "Text");
            if (dsclg.Tables.Count > 0 && dsclg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsclg.Tables[0].Rows.Count; row++)
                {
                    dtpaid.Columns.Add(Convert.ToString(dsclg.Tables[0].Rows[row]["acr"]));
                    htClgAcr.Add(Convert.ToString(dsclg.Tables[0].Rows[row]["college_code"]), Convert.ToString(dsclg.Tables[0].Rows[row]["acr"]));
                }
                if (dtpaid.Columns.Count > 0)
                    dtpaid.Columns.Add("Total");
            }
        }
        catch { dtpaid.Clear(); }
        return dtpaid;
    }

    //college spread bind here

    protected void loadSpreadDetails(DataTable dtpaid, ref Hashtable htpayMode, ref Dictionary<string, string> htClgAcr)
    {
        try
        {
            #region design

            dicColumnAlignment.Clear();
            DataTable dtHeaderWiseReport = new DataTable();
            DataRow drowInst;
            ArrayList arrColHdrNames = new ArrayList();
            arrColHdrNames.Add("S.No");
            dtHeaderWiseReport.Columns.Add("col0");
            arrColHdrNames.Add(rblmode.SelectedItem.Text);
            dtHeaderWiseReport.Columns.Add("col1");
            int colHeader = 1;

            foreach (KeyValuePair<string, string> htrow in htClgAcr)
            {
                colHeader++;
                arrColHdrNames.Add(htrow.Value);
                dtHeaderWiseReport.Columns.Add("col" + colHeader);
                dicColumnAlignment.Add(colHeader, "Col");
            }
            colHeader++;
            arrColHdrNames.Add("Total");
            dtHeaderWiseReport.Columns.Add("col" + colHeader);
            dicColumnAlignment.Add(colHeader, "Col");
            DataRow drHdr1 = dtHeaderWiseReport.NewRow();
            for (int grCol = 0; grCol < dtHeaderWiseReport.Columns.Count; grCol++)
            {
                drHdr1["col" + grCol] = arrColHdrNames[grCol];
            }
            dtHeaderWiseReport.Rows.Add(drHdr1);

            #endregion

            #region value
            dicHeaderWise.Clear();
            string payType = string.Empty;
            int rowCnt = 0;
            int height = 0;
            for (int row = 0; row < dtpaid.Rows.Count; row++)
            {
                height += 10;
                string payModeText = Convert.ToString(dtpaid.Rows[row]["Sno"]);
                if (payModeText.Trim().Contains("@"))
                {
                    payType = payModeText.Split('@')[0];
                    drowInst = dtHeaderWiseReport.NewRow();
                    grdRow = dtHeaderWiseReport.Rows.Count;
                    drowInst[0] = payType;
                    dicHeaderWise.Add(grdRow, payType.Split('@')[0]);
                    dtHeaderWiseReport.Rows.Add(drowInst);
                    continue;
                }
                if (!payModeText.Trim().Contains("*"))
                {
                    bool boolcheck = false;
                    if (payModeText.Trim().Contains("#"))
                    {
                        payType = payModeText.Split('#')[0];

                        drowInst = dtHeaderWiseReport.NewRow();
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        drowInst[0] = payType;
                        dicHeaderWise.Add(grdRow, payType.Split('#')[0]);
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
                    }
                    if (!boolcheck)
                    {
                        drowInst = dtHeaderWiseReport.NewRow();
                        drowInst[0] = ++rowCnt;
                        drowInst[1] = Convert.ToString(dtpaid.Rows[row]["Header"]);
                        for (int col = 2; col < dtpaid.Columns.Count; col++)
                        {
                            drowInst[col] = Convert.ToString(dtpaid.Rows[row][col]);
                        }
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
                    if (payModeText.Split('*')[0].TrimEnd('-').Trim() == "Total")
                    {
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        dicHeaderWise.Add(grdRow, "Total");
                    }
                    if (payModeText.Split('*')[0].TrimEnd('-').Trim() == "Grand Total")
                    {
                        grdRow = dtHeaderWiseReport.Rows.Count;
                        dicHeaderWise.Add(grdRow, "Grand Total");
                    }
                    for (int col = 2; col < dtpaid.Columns.Count; col++)
                    {
                        drowInst[col] = Convert.ToString(dtpaid.Rows[row][col]);
                    }
                    dtHeaderWiseReport.Rows.Add(drowInst);
                }
            }
            grdInstWiseCollectionReport.DataSource = dtHeaderWiseReport;
            grdInstWiseCollectionReport.DataBind();
            grdInstWiseCollectionReport.Visible = true;

            #region Grid ColSpan and Color

            foreach (KeyValuePair<int, string> dr in dicHeaderWise)
            {
                int rowcnt = dr.Key;
                int d = Convert.ToInt32(dtHeaderWiseReport.Columns.Count);
                string payModeVal = dr.Value.ToString();

                if (payModeVal != "Total" && payModeVal != "Grand Total" && payModeVal != "Header Total" && payModeVal != "HeaderName")
                {
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].Font.Bold = true;
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].Font.Size = 13;
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].ColumnSpan = d;
                    if (payModeVal == "Cash")
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#F08080");
                    else if (payModeVal == "Cheque")
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                    else if (payModeVal == "DD")
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FFA500");
                    else if (payModeVal == "Online")
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#90EE90");
                    else if (payModeVal == "Card")
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                    for (int a = 1; a < d; a++)
                    {
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[a].Visible = false;
                    }
                }

                if (payModeVal == "Total")
                {
                    for (int gridCol = 0; gridCol < dtHeaderWiseReport.Columns.Count; gridCol++)
                    {
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].BackColor = Color.Green;
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].HorizontalAlign = HorizontalAlign.Right;
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].Font.Size = 13;
                    }
                    int PayTot = 2;
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].ColumnSpan = PayTot;
                    for (int Coltot = 1; Coltot < PayTot; Coltot++)
                    {
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[Coltot].Visible = false;
                    }
                }
                if (payModeVal == "Grand Total")
                {
                    for (int gridCol = 0; gridCol < dtHeaderWiseReport.Columns.Count; gridCol++)
                    {
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].BackColor = Color.YellowGreen;
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].HorizontalAlign = HorizontalAlign.Right;
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].Font.Size = 13;
                    }
                    int PayTot = 2;
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].ColumnSpan = PayTot;
                    for (int Coltot = 1; Coltot < PayTot; Coltot++)
                    {
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[Coltot].Visible = false;
                    }
                }
                if (payModeVal == "Header Total")
                {
                    for (int gridCol = 0; gridCol < dtHeaderWiseReport.Columns.Count; gridCol++)
                    {
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].BackColor = ColorTranslator.FromHtml("#1A80D8");
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].Font.Bold = true;
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].HorizontalAlign = HorizontalAlign.Right;
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[gridCol].Font.Size = 13;
                    }
                    int PayTot = 2;
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].ColumnSpan = PayTot;
                    for (int Coltot = 1; Coltot < PayTot; Coltot++)
                    {
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[Coltot].Visible = false;
                    }
                }
                if (payModeVal == "HeaderName")
                {
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].Font.Bold = true;
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].Font.Size = 13;
                    grdInstWiseCollectionReport.Rows[rowcnt].Cells[0].ColumnSpan = d;
                    for (int a = 1; a < d; a++)
                    {
                        grdInstWiseCollectionReport.Rows[rowcnt].Cells[a].Visible = false;
                    }
                }
            }
            #endregion

            txtexcelname.Text = "";
            print.Visible = true;
            payModeLabels(htpayMode);
            #endregion
        }

        catch { }
    }

    protected void grdInstWiseCollectionReport_RowDataBound(object sender, GridViewRowEventArgs e)
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
            e.Row.Cells[0].Width = 50;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            if (e.Row.RowIndex != 0)
            {
                foreach (KeyValuePair<int, string> dr in dicColumnAlignment)
                {
                    int rowcnt = dr.Key;
                    e.Row.Cells[rowcnt].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }
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
        lblNeft.Visible = false;//Added by saranya on 13/02/2018
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
            //Added by saranya on 13/02/2018
            if (row.Key.ToString() == "7")
                lblNeft.Visible = true;
        }
        divlabl.Visible = true;
    }

    protected Dictionary<string, string> getreportType()
    {
        Dictionary<string, string> rptType = new Dictionary<string, string>();
        try
        {
            rptType.Add("RECEIPT", " and debit>0");
            rptType.Add("PAYMENTS", "and credit>0");
        }
        catch { }
        return rptType;
    }

    protected Dictionary<string, string> getTransType()
    {
        Dictionary<string, string> rptType = new Dictionary<string, string>();
        try
        {
            rptType.Add("RECEIPT", "1");
            rptType.Add("Journal", "3");
        }
        catch { }
        return rptType;
    }

    protected bool getTableValidation(DataSet ds)
    {
        bool boolCheck = false;
        try
        {
            if (rblMemType.SelectedIndex == 0)
            {
                if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0 || ds.Tables[3].Rows.Count > 0))
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
        ds.Clear();
        ds = loadDetails();
        if (getTableValidation(ds))
        {
            Hashtable htpayMode = new Hashtable();
            Dictionary<string, string> htClgAcr = new Dictionary<string, string>();
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
                                ds.Tables[0].DefaultView.RowFilter = "collegeCode='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                                DataTable dtFirst = ds.Tables[0].DefaultView.ToTable();
                                //ds.Tables[1].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                                //DataTable dtSecond = ds.Tables[1].DefaultView.ToTable();
                                //ds.Tables[2].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                                //DataTable dtThird = ds.Tables[2].DefaultView.ToTable();

                                if (!boolDs)
                                {
                                    dsFinal.Reset();
                                    dsFinal.Tables.Add(dtFirst);
                                    // dsFinal.Tables.Add(dtSecond);
                                    // dsFinal.Tables.Add(dtThird);
                                    boolDs = true;
                                }
                                else
                                {
                                    dsFinal.Merge(dtFirst);
                                    //dsFinal.Merge(dtSecond);
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
                                DataTable dtColumns = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, "paymode", "transtype", "collegecode");
                                DataTable tempTbl = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, "debit", "credit", "paymode", "transtype", "collegecode");
                                dtPertbl = tempTbl.DefaultView.ToTable();
                                dtPertbl.Rows.Clear();
                                foreach (DataRow drRow in dtColumns.Rows)
                                {
                                    tempTbl.DefaultView.RowFilter = "headername='" + drRow[flTName] + "' and paymode='" + drRow["paymode"] + "' and transtype='" + drRow["transtype"] + "' and collegecode='" + drRow["collegecode"] + "'";
                                    DataRow drPer = dtPertbl.NewRow();
                                    drPer[flTName] = drRow[flTName];
                                    drPer["debit"] = tempTbl.DefaultView.ToTable().Compute("SUM(debit)", "");
                                    drPer["credit"] = tempTbl.DefaultView.ToTable().Compute("SUM(credit)", "");
                                    drPer["paymode"] = drRow["paymode"];
                                    drPer["transtype"] = drRow["transtype"];
                                    drPer["collegecode"] = drRow["collegecode"];
                                    dtPertbl.Rows.Add(drPer);
                                }
                            }
                            else
                            {
                                DataTable dtColumns = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, flThName, "paymode", "transtype", "collegecode");
                                DataTable tempTbl = dsFinal.Tables[0].DefaultView.ToTable(true, flTName, flThName, "debit", "credit", "paymode", "transtype", "collegecode");
                                dtPertbl = tempTbl.DefaultView.ToTable();
                                dtPertbl.Rows.Clear();
                                foreach (DataRow drRow in dtColumns.Rows)
                                {
                                    tempTbl.DefaultView.RowFilter = "headername='" + drRow[flTName] + "' and ledgername='" + drRow[flThName] + "' and paymode='" + drRow["paymode"] + "' and transtype='" + drRow["transtype"] + "' and collegecode='" + drRow["collegecode"] + "'";
                                    DataRow drPer = dtPertbl.NewRow();
                                    drPer[flTName] = drRow[flTName];
                                    drPer[flThName] = drRow[flThName];
                                    drPer["debit"] = tempTbl.DefaultView.ToTable().Compute("SUM(debit)", "");
                                    drPer["credit"] = tempTbl.DefaultView.ToTable().Compute("SUM(credit)", "");
                                    drPer["paymode"] = drRow["paymode"];
                                    drPer["transtype"] = drRow["transtype"];
                                    drPer["collegecode"] = drRow["collegecode"];
                                    dtPertbl.Rows.Add(drPer);
                                }
                            }

                            //  var varGroup = from row in tempTbl.AsEnumerable() group row by row.Field<string>("headerName") into grp select new { headerName = grp.Key, debit = grp.Sum(r => r.Field<int>("debit")) };
                            //  DataTable tempTblOne = dsFinal.Tables[1].DefaultView.ToTable();
                            // DataTable dtThird = ds.Tables[2].DefaultView.ToTable();
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
                            //  ds.Tables.Add(tempTblOne);
                            // ds.Tables.Add(dtThird);
                            if (cbIncOthers.Checked)
                            {
                                ds.Tables.Add(dsNornaml.Tables[3].DefaultView.ToTable());
                                //  ds.Tables.Add(dsNornaml.Tables[4].DefaultView.ToTable());
                                //  ds.Tables.Add(dsNornaml.Tables[5].DefaultView.ToTable());

                                // ds.Tables.Add(dsNornaml.Tables[6].DefaultView.ToTable());
                                //  ds.Tables.Add(dsNornaml.Tables[7].DefaultView.ToTable());
                                //  ds.Tables.Add(dsNornaml.Tables[8].DefaultView.ToTable());

                                // ds.Tables.Add(dsNornaml.Tables[9].DefaultView.ToTable());
                                //  ds.Tables.Add(dsNornaml.Tables[10].DefaultView.ToTable());
                                //  ds.Tables.Add(dsNornaml.Tables[11].DefaultView.ToTable());
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

                if (rblmode.SelectedIndex == 0)
                {
                    if (schollSet == 0)//school
                        dtpaid = loaddetailsSchool(ds, ref htpayMode, ref htClgAcr);
                    else//college
                        dtpaid = loaddetails(ds, ref htpayMode, ref htClgAcr);
                }
                else
                {
                    if (schollSet == 0)//school
                        dtpaid = loaddetailsLedgerSchool(ds, ref htpayMode, ref htClgAcr);
                    else//college
                        dtpaid = loaddetailsLedger(ds, ref htpayMode, ref htClgAcr);
                }
            }
            else//mem type
            {
                if (rblmode.SelectedIndex == 0)
                    dtpaid = loaddetailsOthers(ds, ref htpayMode, ref htClgAcr);
                else
                    dtpaid = loaddetailsLedgerOthers(ds, ref htpayMode, ref htClgAcr);
            }
            if (dtpaid.Rows.Count > 0)
            {
                loadSpreadDetails(dtpaid, ref htpayMode, ref  htClgAcr);
            }
            else
            {
                txtexcelname.Text = string.Empty;
                grdInstWiseCollectionReport.Visible = false;
                print.Visible = false;
                divlabl.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
        }
        else
        {
            //lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            grdInstWiseCollectionReport.Visible = false;
            print.Visible = false;
            divlabl.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            //lbl_alert.Text = "No Record Found";
            //imgdiv2.Visible = true;
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
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdInstWiseCollectionReport, reportname);
                //d2.printexcelreport(spreadDet, reportname);
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

            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            degreedetails = "Institutionwise Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
            pagename = "MulInstHdCollection.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed.loadspreaddetails(grdInstWiseCollectionReport, pagename, degreedetails, 0, ss);
            //.loadspreaddetails(spreadDet, pagename, degreedetails);
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
            string selQ = " select collname,college_code,acr from collinfo where college_code in('" + collegecode + "')";
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

    #endregion

    //added by sudhagar 19.05.2017

    protected void rblmode_Selected(object sender, EventArgs e)
    {
        txtexcelname.Text = string.Empty;
        grdInstWiseCollectionReport.Visible = false;
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



    //added by abarna 4.12.2017

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

    /// <summary>
    /// school option included here 05.08.2017 by sudhagar
    /// </summary>
    /// <returns></returns>

    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
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
        chklsfyear.Items.Clear();
        tdlblfnl.Visible = false;
        tdfnl.Visible = false;
        if (checkSchoolSetting() == 0)
        {
            loadfinanceyear();
        }
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
        grdInstWiseCollectionReport.Visible = false;
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
        lbldisp.Text = string.Empty;
        lbldisp.Visible = false;
        lblval.Text = string.Empty;
        tdOthers.Visible = false;
        if (rblMemType.SelectedIndex == 0)//for others option
        {
            tdlblStudCat.Visible = true;
            tdvalStudCat.Visible = true;
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

    //memtype others
    //college data table binding here

    protected DataTable loaddetailsOthers(DataSet ds, ref Hashtable htpayMode, ref Dictionary<string, string> htClgAcr)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            Dictionary<string, string> reportType = getreportType();
            dtpaid = getTableClgAcr(collegecode, ref htClgAcr);//columns added
            DataRow drpaid = dtpaid.NewRow();
            if (dtpaid.Columns.Count > 0)
            {
                int firstDs = 0;
                for (int mem = 0; mem < cblmem.Items.Count; mem++)
                {
                    Hashtable htsubtotal = new Hashtable();
                    Hashtable htfnltotal = new Hashtable();
                    if (!cblmem.Items[mem].Selected)
                        continue;
                    string memText = Convert.ToString(cblmem.Items[mem].Text);
                    bool boolrptType = false;
                    foreach (KeyValuePair<string, string> dtVal in reportType)
                    {
                        bool boolrptName = false;
                        string rptValue = Convert.ToString(dtVal.Key);
                        string reptfltVal = Convert.ToString(dtVal.Value);
                        #region
                        for (int pay = 0; pay < chkl_paid.Items.Count; pay++)
                        {
                            if (!chkl_paid.Items[pay].Selected)
                                continue;
                            bool boolpayText = false;
                            string payModeText = Convert.ToString(chkl_paid.Items[pay].Text);
                            if (!htpayMode.ContainsKey(chkl_paid.Items[pay].Value))
                                htpayMode.Add(chkl_paid.Items[pay].Value, chkl_paid.Items[pay].Text);
                            for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                            {
                                if (!chkl_studhed.Items[hd].Selected)
                                    continue;
                                bool boolHeader = false;
                                double hdfnlTotal = 0;
                                string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                                {
                                    int rowCnt = 0;
                                    if (!cblclg.Items[clg].Selected)
                                        continue;
                                    #region college
                                    double paidAmt = 0;
                                    string clgCode = Convert.ToString(cblclg.Items[clg].Value);
                                    //dividing credit and debit values                                           
                                    string typeName = string.Empty;
                                    string strflter = "paymode='" + chkl_paid.Items[pay].Value + "' and headername='" + hdName + "' and collegecode='" + clgCode + "' ";
                                    //   ds.Tables[0].DefaultView.RowFilter = strflter;
                                    DataView dvpaid = new DataView();
                                    if (!boolrptType)
                                    {
                                        typeName = "Debit";
                                        DataTable temptabledt = ds.Tables[firstDs].DefaultView.ToTable(true, "headerName", "debit", "paymode", "collegecode");
                                        temptabledt.DefaultView.RowFilter = strflter + reptfltVal;
                                        dvpaid = temptabledt.DefaultView;
                                    }
                                    else
                                    {
                                        typeName = "Credit";
                                        DataTable temptablecr = ds.Tables[firstDs].DefaultView.ToTable(true, "headerName", "credit", "paymode", "collegecode");
                                        temptablecr.DefaultView.RowFilter = strflter + reptfltVal;
                                        dvpaid = temptablecr.DefaultView;
                                    }
                                    if (dvpaid.Count > 0)
                                    {
                                        if (!boolrptName)//receipt type
                                        {
                                            drpaid = dtpaid.NewRow();
                                            drpaid["Sno"] = memText + "~" + rptValue + "@" + "Type";
                                            dtpaid.Rows.Add(drpaid);
                                            boolrptName = true;
                                        }
                                        if (!boolpayText)//paymode text added
                                        {
                                            drpaid = dtpaid.NewRow();
                                            drpaid["Sno"] = payModeText + "#" + "Mode";
                                            dtpaid.Rows.Add(drpaid);
                                            boolpayText = true;
                                        }
                                        if (!boolHeader)
                                        {
                                            drpaid = dtpaid.NewRow();
                                            boolHeader = true;
                                            rowCnt++;
                                            drpaid["Sno"] = Convert.ToString(rowCnt);
                                            drpaid["Header"] = hdName;
                                        }
                                        string collName = Convert.ToString(htClgAcr[clgCode]);
                                        double.TryParse(Convert.ToString(dvpaid[0][typeName]), out paidAmt);
                                        drpaid[collName] = Convert.ToString(paidAmt);
                                        hdfnlTotal += paidAmt;//headerwise final total
                                        if (!htsubtotal.ContainsKey(collName))
                                            htsubtotal.Add(collName, paidAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htsubtotal[collName]), out amount);
                                            amount += paidAmt;
                                            htsubtotal.Remove(collName);
                                            htsubtotal.Add(collName, Convert.ToString(amount));
                                        }
                                    }
                                    #endregion
                                }
                                //end of the header total
                                if (hdfnlTotal != 0)
                                {
                                    drpaid["Total"] = Convert.ToString(hdfnlTotal);
                                    dtpaid.Rows.Add(drpaid);
                                }
                            }
                            //paymode wise sub total
                            if (htsubtotal.Count > 0)
                            {
                                #region
                                double fnltempAmt = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = "Total" + "-" + "*";
                                foreach (DictionaryEntry htRow in htsubtotal)
                                {
                                    double tempAmt = 0;
                                    double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                    drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                    fnltempAmt += tempAmt;
                                    if (!htfnltotal.ContainsKey(htRow.Key))
                                        htfnltotal.Add(htRow.Key, tempAmt);
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htfnltotal[htRow.Key]), out amount);
                                        amount += tempAmt;
                                        htfnltotal.Remove(htRow.Key);
                                        htfnltotal.Add(htRow.Key, Convert.ToString(amount));
                                    }
                                }
                                drpaid["Total"] = Convert.ToString(fnltempAmt);
                                dtpaid.Rows.Add(drpaid);
                                if (!htfnltotal.ContainsKey("Total"))
                                    htfnltotal.Add("Total", fnltempAmt);
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htfnltotal["Total"]), out amount);
                                    amount += fnltempAmt;
                                    htfnltotal.Remove("Total");
                                    htfnltotal.Add("Total", Convert.ToString(amount));
                                }
                                htsubtotal.Clear();
                                #endregion
                            }
                        }
                        if (htfnltotal.Count > 0)
                        {
                            double fnltempAmt = 0;
                            drpaid = dtpaid.NewRow();
                            drpaid["Sno"] = "Grand Total" + "-" + "*";
                            foreach (DictionaryEntry htRow in htfnltotal)
                            {
                                if (Convert.ToString(htRow.Key) != "Total")
                                {
                                    double tempAmt = 0;
                                    double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                    drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                    fnltempAmt += tempAmt;
                                }
                            }
                            drpaid["Total"] = Convert.ToString(fnltempAmt);
                            dtpaid.Rows.Add(drpaid);
                            htfnltotal.Clear();
                        }
                        #endregion
                        boolrptType = true;
                    }
                    firstDs++;
                }
            }
        }
        catch { }
        return dtpaid;
    }

    protected DataTable loaddetailsLedgerOthers(DataSet ds, ref Hashtable htpayMode, ref Dictionary<string, string> htClgAcr)
    {
        DataTable dtpaid = new DataTable();
        try
        {
            Dictionary<string, string> reportType = getreportType();
            dtpaid = getTableClgAcr(collegecode, ref htClgAcr);//columns added
            DataRow drpaid = dtpaid.NewRow();
            if (dtpaid.Columns.Count > 0)
            {
                int firstDs = 0;
                for (int mem = 0; mem < cblmem.Items.Count; mem++)
                {
                    Hashtable htsubtotal = new Hashtable();
                    Hashtable htfnltotal = new Hashtable();
                    Hashtable hthdtotal = new Hashtable();
                    if (!cblmem.Items[mem].Selected)
                        continue;
                    string memText = Convert.ToString(cblmem.Items[mem].Text);
                    bool boolrptType = false;
                    foreach (KeyValuePair<string, string> dtVal in reportType)
                    {
                        bool boolrptName = false;
                        string rptValue = Convert.ToString(dtVal.Key);
                        string reptfltVal = Convert.ToString(dtVal.Value);
                        #region
                        for (int pay = 0; pay < chkl_paid.Items.Count; pay++)
                        {
                            if (!chkl_paid.Items[pay].Selected)
                                continue;
                            bool boolpayText = false;
                            string payModeText = Convert.ToString(chkl_paid.Items[pay].Text);
                            if (!htpayMode.ContainsKey(chkl_paid.Items[pay].Value))
                                htpayMode.Add(chkl_paid.Items[pay].Value, chkl_paid.Items[pay].Text);
                            for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                            {
                                bool boolHeaderName = false;
                                if (!chkl_studhed.Items[pay].Selected)
                                    continue;
                                DataView dvpaids = new DataView();
                                string hdName = Convert.ToString(chkl_studhed.Items[hd].Text);
                                if (!boolrptType)
                                {
                                    DataTable temptabledt = ds.Tables[firstDs].DefaultView.ToTable(true, "headerName", "ledgername");
                                    temptabledt.DefaultView.RowFilter = "headername='" + hdName + "'";
                                    dvpaids = temptabledt.DefaultView;
                                }
                                else
                                {
                                    DataTable temptablecr = ds.Tables[firstDs].DefaultView.ToTable(true, "headerName", "ledgername");
                                    temptablecr.DefaultView.RowFilter = "headername='" + hdName + "'";
                                    dvpaids = temptablecr.DefaultView;
                                }
                                if (dvpaids.Count > 0)
                                {
                                    for (int pd = 0; pd < dvpaids.Count; pd++)
                                    {
                                        bool boolHeader = false;
                                        double hdfnlTotal = 0;
                                        for (int clg = 0; clg < cblclg.Items.Count; clg++)
                                        {
                                            int rowCnt = 0;
                                            if (!cblclg.Items[clg].Selected)
                                                continue;
                                            #region college
                                            double paidAmt = 0;
                                            string clgCode = Convert.ToString(cblclg.Items[clg].Value);
                                            //dividing credit and debit values                                           
                                            string typeName = string.Empty;
                                            string strflter = "paymode='" + chkl_paid.Items[pay].Value + "' and ledgerName='" + dvpaids[pd]["ledgername"] + "' and collegecode='" + clgCode + "' ";
                                            //   ds.Tables[0].DefaultView.RowFilter = strflter;
                                            DataView dvpaid = new DataView();
                                            if (!boolrptType)
                                            {
                                                typeName = "Debit";
                                                DataTable temptabledt = ds.Tables[firstDs].DefaultView.ToTable(true, "headerName", "ledgername", "debit", "paymode", "collegecode");
                                                // DataTable temptabledt = dvpaids.ToTable();
                                                temptabledt.DefaultView.RowFilter = strflter + reptfltVal;
                                                dvpaid = temptabledt.DefaultView;
                                            }
                                            else
                                            {
                                                typeName = "Credit";
                                                DataTable temptablecr = ds.Tables[firstDs].DefaultView.ToTable(true, "headerName", "ledgername", "credit", "paymode", "collegecode");
                                                // DataTable temptablecr = dvpaids.ToTable();
                                                temptablecr.DefaultView.RowFilter = strflter + reptfltVal;
                                                dvpaid = temptablecr.DefaultView;
                                            }
                                            if (dvpaid.Count > 0)
                                            {
                                                if (!boolrptName)//receipt type
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    drpaid["Sno"] = memText + "~" + rptValue + "@" + "Type";
                                                    dtpaid.Rows.Add(drpaid);
                                                    boolrptName = true;
                                                }
                                                if (!boolpayText)//paymode text added
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    drpaid["Sno"] = payModeText + "#" + "Mode";
                                                    dtpaid.Rows.Add(drpaid);
                                                    boolpayText = true;
                                                }
                                                if (!boolHeaderName)//headerName bind to data table
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    drpaid["Sno"] = hdName + "!" + "HeaderName";
                                                    dtpaid.Rows.Add(drpaid);
                                                    boolHeaderName = true;
                                                }
                                                if (!boolHeader)
                                                {
                                                    drpaid = dtpaid.NewRow();
                                                    boolHeader = true;
                                                    rowCnt++;
                                                    drpaid["Sno"] = Convert.ToString(rowCnt);
                                                    drpaid["Header"] = dvpaid[0]["LedgerName"];
                                                }
                                                string collName = Convert.ToString(htClgAcr[clgCode]);
                                                double.TryParse(Convert.ToString(dvpaid[0][typeName]), out paidAmt);
                                                drpaid[collName] = Convert.ToString(paidAmt);
                                                hdfnlTotal += paidAmt;//headerwise final total
                                                if (!htsubtotal.ContainsKey(collName))
                                                    htsubtotal.Add(collName, paidAmt);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htsubtotal[collName]), out amount);
                                                    amount += paidAmt;
                                                    htsubtotal.Remove(collName);
                                                    htsubtotal.Add(collName, Convert.ToString(amount));
                                                }
                                                if (!hthdtotal.ContainsKey(collName))
                                                    hthdtotal.Add(collName, paidAmt);
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(hthdtotal[collName]), out amount);
                                                    amount += paidAmt;
                                                    hthdtotal.Remove(collName);
                                                    hthdtotal.Add(collName, Convert.ToString(amount));
                                                }

                                            }
                                            #endregion
                                        }
                                        //end of the header total
                                        if (hdfnlTotal != 0)
                                        {
                                            drpaid["Total"] = Convert.ToString(hdfnlTotal);
                                            dtpaid.Rows.Add(drpaid);
                                        }
                                    }
                                }
                                //every header end total
                                if (hthdtotal.Count > 0)
                                {
                                    double fnltempAmt = 0;
                                    drpaid = dtpaid.NewRow();
                                    drpaid["Sno"] = "Header Total" + "-" + "*";
                                    foreach (DictionaryEntry htRow in hthdtotal)
                                    {
                                        double tempAmt = 0;
                                        double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                        drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                        fnltempAmt += tempAmt;
                                    }
                                    drpaid["Total"] = Convert.ToString(fnltempAmt);
                                    dtpaid.Rows.Add(drpaid);
                                    hthdtotal.Clear();
                                }
                            }
                            //paymode wise sub total
                            if (htsubtotal.Count > 0)
                            {
                                #region
                                double fnltempAmt = 0;
                                drpaid = dtpaid.NewRow();
                                drpaid["Sno"] = "Total" + "-" + "*";
                                foreach (DictionaryEntry htRow in htsubtotal)
                                {
                                    double tempAmt = 0;
                                    double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                    drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                    fnltempAmt += tempAmt;
                                    if (!htfnltotal.ContainsKey(htRow.Key))
                                        htfnltotal.Add(htRow.Key, tempAmt);
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htfnltotal[htRow.Key]), out amount);
                                        amount += tempAmt;
                                        htfnltotal.Remove(htRow.Key);
                                        htfnltotal.Add(htRow.Key, Convert.ToString(amount));
                                    }
                                }
                                drpaid["Total"] = Convert.ToString(fnltempAmt);
                                dtpaid.Rows.Add(drpaid);
                                if (!htfnltotal.ContainsKey("Total"))
                                    htfnltotal.Add("Total", fnltempAmt);
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htfnltotal["Total"]), out amount);
                                    amount += fnltempAmt;
                                    htfnltotal.Remove("Total");
                                    htfnltotal.Add("Total", Convert.ToString(amount));
                                }
                                htsubtotal.Clear();
                                #endregion
                            }
                        }
                        if (htfnltotal.Count > 0)
                        {
                            double fnltempAmt = 0;
                            drpaid = dtpaid.NewRow();
                            drpaid["Sno"] = "Grand Total" + "-" + "*";
                            foreach (DictionaryEntry htRow in htfnltotal)
                            {
                                if (Convert.ToString(htRow.Key) != "Total")
                                {
                                    double tempAmt = 0;
                                    double.TryParse(Convert.ToString(htRow.Value), out tempAmt);
                                    drpaid[Convert.ToString(htRow.Key)] = Convert.ToString(tempAmt);
                                    fnltempAmt += tempAmt;
                                }
                            }
                            drpaid["Total"] = Convert.ToString(fnltempAmt);
                            dtpaid.Rows.Add(drpaid);
                            htfnltotal.Clear();
                        }
                        #endregion
                        boolrptType = true;
                    }
                    firstDs++;
                }
            }
        }
        catch { }
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

    //added by sudhagar 23.09.2017

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

}