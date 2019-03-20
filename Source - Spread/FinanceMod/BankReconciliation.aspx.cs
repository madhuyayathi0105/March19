using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class BankReconciliation : System.Web.UI.Page
{
    ArrayList ItemList = new ArrayList();
    ArrayList Itemindex = new ArrayList();
    ArrayList ItemEmpty = new ArrayList();
    Boolean Cellclick = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string stcollegecode = string.Empty;
    int count = 0;
    Hashtable ht = new Hashtable();
    Hashtable htable = new Hashtable();
    Hashtable hasvalue = new Hashtable();

    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    static int personmode = 0;
    static int chosedmode = 0;
    static int MemType = 0;
    bool usBasedRights = false;
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
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                usercode = Session["group_code"].ToString();
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                usercode = Session["usercode"].ToString();
            }
            if (!IsPostBack)
            {
                // getidadress();
                setLabelText();
                binddate();
                loadcollege();
                bindbankname();
                bindOtherbankname();
                txt_selectDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_selectDate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                // btn_go_Click(sender, e);
                pheaderfilter.Visible = false;
                loadsetting();
                loadBank();
                rbstud_OnCheckedChanged(sender, e);
                sett();
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = ddl_collegename.SelectedItem.Value;
                collegecode = ddl_collegename.SelectedItem.Value;
                stcollegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }
    public void loadcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddl_collegename.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }

        }
        catch
        {
        }
    }
    public void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbankname();
        bindOtherbankname();
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string sclType = d2.GetFunction("select value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'");
            string text = "";
            string textval = "";
            string txtgp = "";
            if (sclType == "0")
            {
                text = "Admission No";
                textval = " ,Roll_admit as no";
                txtgp = ",Roll_admit ";
            }
            else
            {
                text = "Roll No";
                textval = " ,roll_no as no";
                txtgp = ",roll_no ";
            }
            ViewState["rolltext"] = text;
            if (ViewState["ItemList"] != null)
            {
                ItemList = (ArrayList)ViewState["ItemList"];
            }
            if (ViewState["Itemindex"] != null)
            {
                Itemindex = (ArrayList)ViewState["Itemindex"];
            }
            loadColumnOrder();
            UserbasedRights();
            Hashtable htdt = new Hashtable();
            lblerr.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            Boolean cblcouunt = false;
            string fromdate = "";
            string todate = "";

            hasvalue.Clear();
            string yearstart = "";
            string yearend = "";
            string selquery = "select LinkValue from InsSettings where LinkName like 'Current%'  and college_code='" + collegecode1 + "'";
            string acctid = d2.GetFunction(selquery);
            string selq = "select FinYearStart,FinYearEnd from FM_FinYearMaster where FinYearPK='" + acctid + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                yearstart = Convert.ToString(ds.Tables[0].Rows[0]["FinYearStart"]);
                yearend = Convert.ToString(ds.Tables[0].Rows[0]["FinYearEnd"]);
            }
            string orderby = "";
            //yearstart = txt_fromdate.Text.ToString();
            //yearend = Txt_Todate.Text.ToString();
            fromdate = Convert.ToString(txt_fromdate.Text);
            todate = Convert.ToString(txt_todate.Text);
            if (fromdate != "" && todate != "")
            {
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                {
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                }
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                {
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                }
            }

            #region columnorder

            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == true)
                {
                    ht.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                    string colvalue = cblcolumnorder.Items[i].Text;
                    if (ItemList.Contains(colvalue) == false)
                    {
                        ItemList.Add(cblcolumnorder.Items[i].Text);

                    }
                    tborder.Text = "";
                    for (int j = 0; j < ItemList.Count; j++)
                    {
                        tborder.Text = tborder.Text + "  " + ItemList[j].ToString();
                        tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")";

                    }
                }
                else
                {
                    ItemList.Remove(cblcolumnorder.Items[i].Text);
                }
                cblcolumnorder.Items[0].Enabled = false;
            }
            #endregion

            ItemEmpty.Clear();
            if (rbtodeposit.Checked == true)
            {
                string colvalue = rbdeposit.Text;
                if (ItemEmpty.Contains(colvalue) == false)
                {
                    ItemEmpty.Add(colvalue);
                    orderby = "";
                }
            }
            if (rbdeposit.Checked == true)
            {
                string colvalue = rbdeposit.Text;
                if (ItemEmpty.Contains(colvalue) == false)
                {
                    ItemEmpty.Add(colvalue);
                    orderby = " order by transdate";
                }
            }
            if (rbbounce.Checked == true)
            {
                string colvalue = rbbounce.Text;
                if (ItemEmpty.Contains(colvalue) == false)
                {
                    ItemEmpty.Add(colvalue);
                    orderby = "";
                }
            }
            if (rbclear.Checked == true)
            {
                string colvalue = rbclear.Text;
                if (ItemEmpty.Contains(colvalue) == false)
                {
                    ItemEmpty.Add(colvalue);
                    orderby = "";
                }
            }
            int valueval = 0;

            if (rbentry.Checked == true)
                valueval = 1;
            else
                valueval = 0;


            // ItemList.Clear();
            #region ItemList

            if (ItemList.Count == 0)
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    ht.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                    string colvalue = cblcolumnorder.Items[i].Text;
                    if (ItemList.Contains(colvalue) == false)
                    {
                        ItemList.Add(cblcolumnorder.Items[i].Text);

                    }
                    tborder.Text = "";
                    for (int j = 0; j < ItemList.Count; j++)
                    {
                        tborder.Text = tborder.Text + "  " + ItemList[j].ToString();
                        tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";

                    }
                }
            }
            #endregion

            #region appno

            string appno = "";
            string appval = "";
            string rollno = Convert.ToString(txt_rollno.Text.ToString());
            if (rbstud.Checked == true)
            {
                #region student

                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    appno = d2.GetFunction("select App_No  from Registration where Roll_No='" + rollno + "' and college_code='" + stcollegecode + "'");
                    appval = " and  t.App_No='" + appno + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    appno = d2.GetFunction("select App_No  from Registration where reg_no='" + rollno + "' and college_code='" + stcollegecode + "'");
                    appval = " and  t.App_No='" + appno + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    appno = d2.GetFunction("select App_No  from Registration where Roll_Admit='" + rollno + "' and college_code='" + stcollegecode + "'");
                    appval = " and  t.App_No='" + appno + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                {
                    appno = d2.GetFunction(" select app_no from applyn where app_formno='" + rollno + "' and college_code='" + stcollegecode + "'");
                    appval = " and  t.App_No='" + appno + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 4)
                {
                    // appno = d2.GetFunction(" select app_no from applyn where app_formno='" + rollno + "'");
                    appval = " and  t.DDNo='" + rollno + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 5)
                {
                    //appno = d2.GetFunction(" select app_no from applyn where app_formno='" + rollno + "'");
                    appval = " and  t.DDNo='" + rollno + "'";
                }
                #endregion
            }
            else if (rbstaff.Checked == true)
            {
                #region staff
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    appno = d2.GetFunction("select sa.appl_id  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.staff_code ='" + rollno + "'");
                    appval = " and  sa.appl_id='" + appno + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    appval = " and  f.DDNo='" + rollno + "'";

                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    appval = " and  f.DDNo='" + rollno + "'";

                #endregion
            }
            else if (rbvendor.Checked == true)
            {
                #region vendor
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    appno = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + rollno + "' and vendortype='1'");
                    appval = " and  p.vendorPK='" + appno + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    appval = " and  f.DDNo='" + rollno + "'";

                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    appval = " and  f.DDNo='" + rollno + "'";

                #endregion
            }
            else
            {
                #region vendor
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    appno = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + rollno + "' and vendortype='-5'");
                    appval = " and  p.vendorPK='" + appno + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    appval = " and  f.DDNo='" + rollno + "'";

                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    appval = " and  f.DDNo='" + rollno + "'";

                #endregion
            }

            #endregion

            #region paymode

            string selquery1 = "";
            string paymod = "";
            if (rbstud.Checked == true)
            {
                if (cb_cheque.Checked == true && cb_dd.Checked == true)
                    paymod = "and t.PayMode  in(2,3)";

                else if (cb_cheque.Checked == true)
                    paymod = "and t.PayMode  in(2)";

                else if (cb_dd.Checked == true)
                    paymod = "and t.PayMode  in(3)";

                else
                    paymod = "and t.PayMode  in(2,3)";
            }
            else
            {
                if (cb_cheque.Checked == true && cb_dd.Checked == true)
                    paymod = "and f.PayMode  in(2,3)";

                else if (cb_cheque.Checked == true)
                    paymod = "and f.PayMode  in(2)";

                else if (cb_dd.Checked == true)
                    paymod = "and f.PayMode  in(3)";

                else
                    paymod = "and f.PayMode  in(2,3)";
            }
            #endregion

            #region selected list report

            string addsubquery = "";
            string type = "";
            for (int sel = 0; sel < cbltype.Items.Count; sel++)
            {
                if (cbltype.Items[sel].Selected == true)
                {
                    if (rbstud.Checked == true)
                    {
                        if (cbltype.Items[sel].Text.Trim() == "Deposited")
                            type = " (ISNULL(t.IsDeposited,'0')='1' and ISNULL(t.IsBounced,0)='0' and ISNULL(IsCollected,0)='0')";
                        if (cbltype.Items[sel].Text.Trim() == "Bounced")
                        {
                            if (type != "")
                                type += " or (ISNULL(t.IsDeposited,'0')='1' and ISNULL(t.IsBounced,0)='1' and ISNULL(IsCollected,0)='0')";
                            else
                                type = " (ISNULL(t.IsDeposited,'0')='1' and ISNULL(t.IsBounced,0)='1' and ISNULL(IsCollected,0)='0')";
                        }
                        if (cbltype.Items[sel].Text.Trim() == "Cleared")
                        {
                            if (type != "")
                                type += " or ( ISNULL(t.IsDeposited,'0')='1' and ISNULL(t.IsBounced,0)='0' and ISNULL(IsCollected,0)='1' )";
                            else
                                type += "( ISNULL(t.IsDeposited,'0')='1' and ISNULL(t.IsBounced,0)='0' and ISNULL(IsCollected,0)='1' )";
                        }
                    }
                    else
                    {
                        if (cbltype.Items[sel].Text.Trim() == "Deposited")
                            type = " (ISNULL(f.IsDeposited,'0')='1' and ISNULL(f.IsBounced,0)='0' and ISNULL(IsCollected,0)='0')";
                        if (cbltype.Items[sel].Text.Trim() == "Bounced")
                        {
                            if (type != "")
                                type += " or (ISNULL(f.IsDeposited,'0')='1' and ISNULL(f.IsBounced,0)='1' and ISNULL(IsCollected,0)='0')";
                            else
                                type = " (ISNULL(f.IsDeposited,'0')='1' and ISNULL(f.IsBounced,0)='1' and ISNULL(IsCollected,0)='0')";
                        }
                        if (cbltype.Items[sel].Text.Trim() == "Cleared")
                        {
                            if (type != "")
                                type += " or ( ISNULL(f.IsDeposited,'0')='1' and ISNULL(f.IsBounced,0)='0' and ISNULL(IsCollected,0)='1' )";
                            else
                                type += "( ISNULL(f.IsDeposited,'0')='1' and ISNULL(f.IsBounced,0)='0' and ISNULL(IsCollected,0)='1' )";
                        }
                    }
                }
            }
            #endregion

            DataSet dsload = new DataSet();
            string strdate = "";
            string strdt = "";
            string iscancel = "";
            string bankfk = Convert.ToString(getCblSelectedValue(cblbank));
            //clearDDtoBounce()
            #region condition apply
            if (valueval == 1)
            {
                if (rbstud.Checked == true)
                {
                    if (rbtodeposit.Checked == true)
                    {
                        strdate = " and t.TransDate between '" + fromdate + "' and '" + todate + "'";
                        addsubquery = "   and ( ISNULL(t.IsDeposited,'0')='0' ) and ISNULL(IsCanceled,'0')<>'1'";
                        strdt = " and t.TransDate between '" + fromdate + "' and '" + todate + "'";
                    }
                    else if (rbdeposit.Checked == true)
                    {
                        strdate = " and t.DepositedDate between '" + fromdate + "' and '" + todate + "'";
                        addsubquery = " and(" + type + ") and( ISNULL(IsCanceled,'0')<>'1' or ISNULL(IsCanceled,'0')='1')";
                    }
                    else if (rbbounce.Checked == true)
                    {
                        strdate = " and t.DepositedDate between '" + fromdate + "' and '" + todate + "'";
                        //ISNULL(IsCollected,0)=1 or
                        addsubquery = "   and ( ISNULL(t.IsDeposited,'0')='1' and ISNULL( t.IsBounced,0)=0 and( ";
                        if (clearDDtoBounce() == 1)
                            addsubquery += " ISNULL(IsCollected,0)=1 or";

                        addsubquery += " ISNULL(IsCollected,0)=0) ) and ISNULL(IsCanceled,'0')<>'1'";
                    }
                    else if (rbclear.Checked == true)
                    {
                        strdate = " and t.DepositedDate between '" + fromdate + "' and '" + todate + "'";
                        addsubquery = "   and ( ISNULL(t.IsDeposited,'0')='1' and ISNULL( t.IsBounced,0)=0 and ISNULL(IsCollected,0)=0)  and ISNULL(IsCanceled,'0')<>'1'";
                    }
                }
                else
                {
                    if (rbtodeposit.Checked == true)
                    {
                        strdate = " and f.TransDate between '" + fromdate + "' and '" + todate + "'";
                        addsubquery = "   and ( ISNULL(f.IsDeposited,'0')='0' ) and ISNULL(IsCanceled,'0')<>'1'";
                        strdt = " and f.TransDate between '" + fromdate + "' and '" + todate + "'";
                    }
                    else if (rbdeposit.Checked == true)
                    {
                        strdate = " and f.DepositedDate between '" + fromdate + "' and '" + todate + "'";
                        addsubquery = " and(" + type + ") and( ISNULL(IsCanceled,'0')<>'1' or ISNULL(IsCanceled,'0')='1')";
                    }
                    else if (rbbounce.Checked == true)
                    {
                        strdate = " and f.DepositedDate between '" + fromdate + "' and '" + todate + "'";
                        //  addsubquery = "   and ( f.IsDeposited='1' and ISNULL( f.IsBounced,0)=0 and( ISNULL(IsCollected,0)=1 or ISNULL(IsCollected,0)=0) ) and ISNULL(IsCanceled,'0')<>'1'";
                        addsubquery = "   and ( ISNULL(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and( ";
                        if (clearDDtoBounce() == 1)
                            addsubquery += " ISNULL(IsCollected,0)=1 or";

                        addsubquery += " ISNULL(IsCollected,0)=0) ) and ISNULL(IsCanceled,'0')<>'1'";
                    }
                    else if (rbclear.Checked == true)
                    {
                        strdate = " and f.DepositedDate between '" + fromdate + "' and '" + todate + "'";
                        addsubquery = "   and ( ISNULL(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=0)  and ISNULL(IsCanceled,'0')<>'1'";
                    }
                }
            }
            #endregion

            #region memtype


            if (rbstud.Checked == true)
            {
                //student
                #region Query

                string bkfk = "";
                if (txt_rollno.Text.Trim() == "")
                {
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 = "select convert(varchar(10),transdate,103) as transdate,transcode" + textval + ",stud_name,case when paymode=2 then 'Cheque' when paymode='3' then 'DD' end paymode,paymode as pay,(select TextVal from textvaltable b where b.TextCode = t.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = t.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(debit) as Amount,ISNULL( IsDeposited,'0') as IsDeposited,ISNULL( IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared  from FT_FinDailyTransaction t,registration r  where t.App_No = r.App_No and TransType =1 " + strdate + "";

                        selquery1 += "  and college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += "  and college_code='" + collegecode1 + "'";
                        bkfk = "";

                    }
                    else
                    {
                        selquery1 = " select f.bankfk, convert(varchar(10),t.transdate,103) as transdate,transcode" + textval + ",stud_name,case when t.paymode=2 then 'Cheque' when t.paymode='3' then 'DD' end paymode,t.paymode as pay,(select TextVal from textvaltable b where b.TextCode = t.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = t.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(t.debit) as Amount,ISNULL( t.IsDeposited,'0') as IsDeposited,ISNULL( t.IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared from FT_FinDailyTransaction t,Registration r,FT_FinBankTransaction f,FM_FinBankMaster b where t.TransCode =f.DailyTransID and t.App_No =r.App_No and b.BankPK =f.BankFK and f.paymode=t.paymode and TransType =1  " + strdate + " and college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and f.BankFK in('" + bankfk + "')";
                        bkfk = " ,f.bankfk";
                    }
                }
                else if (txt_rollno.Text.Trim() != "")
                {
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 = "select convert(varchar(10),transdate,103) as transdate,transcode" + textval + ",stud_name,case when paymode=2 then 'Cheque' when paymode='3' then 'DD' end paymode,paymode as pay,(select TextVal from textvaltable b where b.TextCode = t.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = t.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(debit) as Amount,ISNULL( IsDeposited,'0') as IsDeposited,ISNULL( IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared  from FT_FinDailyTransaction t,registration r  where t.App_No = r.App_No and TransType =1 " + appval + " " + strdate + "";
                        selquery1 += " and college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += "  and college_code='" + collegecode1 + "'";

                        bkfk = "";
                    }
                    else
                    {
                        selquery1 = " select f.bankfk, convert(varchar(10),t.transdate,103) as transdate,transcode" + textval + ",stud_name,case when t.paymode=2 then 'Cheque' when t.paymode='3' then 'DD' end paymode,t.paymode as pay,(select TextVal from textvaltable b where b.TextCode = t.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = t.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(t.debit) as Amount,ISNULL( t.IsDeposited,'0') as IsDeposited,ISNULL( t.IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared from FT_FinDailyTransaction t,Registration r,FT_FinBankTransaction f,FM_FinBankMaster b where t.TransCode =f.DailyTransID and t.App_No =r.App_No and b.BankPK =f.BankFK and f.paymode=t.paymode and TransType =1  " + appval + " " + strdate + " and college_code='" + collegecode1 + "'";

                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and f.BankFK in('" + bankfk + "')";
                        bkfk = " ,f.bankfk";
                    }
                }
                selquery1 = selquery1 + "  " + paymod + "";
                if (addsubquery.Trim() != "")
                {
                    selquery1 = selquery1 + " " + addsubquery + "";
                }
                selquery1 = selquery1 + "   group by transcode,t.transdate" + txtgp + ",stud_name,t.paymode,DDBankCode,DepositBankCode, ddno,dddate ,t.IsDeposited,t.IsBounced,IsCollected,depositedDate,BouncedDate,CollectedDate " + bkfk + " " + orderby + "";

                // selquery1 += " select distinct convert(varchar(10),transdate,103) as transdate from FT_FinDailyTransaction t,registration r,FT_FinBankTransaction fb  where t.App_No = r.App_No and fb.DailyTransID =t.TransCode and TransType =1 " + strdate + " and college_code='" + collegecode1 + "'";
                if (txt_rollno.Text.Trim() == "")
                {
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 += "select distinct convert(varchar(10),transdate,103) as transdate from FT_FinDailyTransaction t,registration r  where t.App_No = r.App_No and TransType =1 " + strdate + "";
                        selquery1 += " and college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    else
                    {
                        selquery1 += " select distinct convert(varchar(10),t.transdate,103) as transdate from FT_FinDailyTransaction t,Registration r,FT_FinBankTransaction f,FM_FinBankMaster b where t.TransCode =f.DailyTransID and t.App_No =r.App_No and b.BankPK =f.BankFK and TransType =1  " + strdate + "";
                        selquery1 += " and college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                }
                else if (txt_rollno.Text.Trim() != "")
                {
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 += "select distinct convert(varchar(10),transdate,103) as transdate from FT_FinDailyTransaction t,registration r  where t.App_No = r.App_No and TransType =1 " + appval + " " + strdate + "";
                        selquery1 += " and college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    else
                    {
                        selquery1 += " select distinct convert(varchar(10),t.transdate,103) as transdate from FT_FinDailyTransaction t,Registration r,FT_FinBankTransaction f,FM_FinBankMaster b where t.TransCode =f.DailyTransID and t.App_No =r.App_No and b.BankPK =f.BankFK and TransType =1 " + appval + " " + strdate + "";
                        selquery1 += " and college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";

                    }
                }
                selquery1 += "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster";

                #endregion
            }
            else if (rbstaff.Checked == true)
            {
                //staff
                #region Query

                string bkfk = "";
                if (txt_rollno.Text.Trim() == "")
                {
                    #region without textbox

                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 = " select convert(varchar(10),transdate,103) as transdate,transcode,f.App_no,sa.appl_id,s.staff_code,s.staff_name ,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(debit) as Amount,ISNULL( IsDeposited,'0') as IsDeposited,ISNULL( IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1'   " + strdate + " ";
                        selquery1 += "  and s.college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";

                            selquery1 += "  and s.college_code='" + collegecode1 + "'";
                        bkfk = "";
                    }
                    else
                    {

                        selquery1 = " select ft.bankfk, convert(varchar(10),f.transdate,103) as transdate,transcode,f.App_no,sa.appl_id,s.staff_code,s.staff_name ,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(f.debit) as Amount,ISNULL( f.IsDeposited,'0') as IsDeposited,ISNULL( f.IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T,FT_FinBankTransaction ft,FM_FinBankMaster bm where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and bm.BankPK =ft.BankFK and f.Transcode=ft.DailyTransId and T.latestrec ='1' " + strdate + "  ";
                        selquery1 += "  and s.college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += "  and s.college_code='" + collegecode1 + "'";
                        selquery1 += " and ft.BankFK in('" + bankfk + "')";
                        bkfk = " ,ft.bankfk";
                    }
                    #endregion
                }
                else if (txt_rollno.Text.Trim() != "")
                {
                    #region with textbox

                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 = " select convert(varchar(10),transdate,103) as transdate,transcode,f.App_no,sa.appl_id,s.staff_code,s.staff_name ,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(debit) as Amount,ISNULL( IsDeposited,'0') as IsDeposited,ISNULL( IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1'   " + appval + " " + strdate + "";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";

                            selquery1 += "  and s.college_code='" + collegecode1 + "'";
                        bkfk = "";
                    }
                    else
                    {

                        selquery1 = " select ft.bankfk, convert(varchar(10),f.transdate,103) as transdate,transcode,f.App_no,sa.appl_id,s.staff_code,s.staff_name ,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(f.debit) as Amount,ISNULL( f.IsDeposited,'0') as IsDeposited,ISNULL( f.IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T,FT_FinBankTransaction ft,FM_FinBankMaster bm where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and bm.BankPK =ft.BankFK  and f.Transcode=ft.DailyTransId and T.latestrec ='1' " + appval + " " + strdate + "  ";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += "  and s.college_code='" + collegecode1 + "'";
                        selquery1 += " and ft.BankFK in('" + bankfk + "')";
                        bkfk = " ,ft.bankfk";
                    }
                    #endregion
                }
                selquery1 = selquery1 + "  " + paymod + "";
                if (addsubquery.Trim() != "")
                {
                    selquery1 = selquery1 + " " + addsubquery + "";
                }
                selquery1 = selquery1 + "   group by transcode,f.transdate,f.App_no,sa.appl_id,s.staff_code,s.staff_name,f.paymode,DDBankCode,DepositBankCode, ddno,dddate ,f.IsDeposited,f.IsBounced,IsCollected,depositedDate,BouncedDate,CollectedDate " + bkfk + " " + orderby + "";

                //total transdate
                if (txt_rollno.Text.Trim() == "")
                {
                    #region without textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 += " select distinct convert(varchar(10),transdate,103) as transdate from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and f.TransType =1 " + strdate + " ";
                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and s.college_code='" + collegecode1 + "'";
                    }
                    else
                    {
                        selquery1 += " select distinct convert(varchar(10),f.transdate,103) as transdate from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T,FT_FinBankTransaction ft,FM_FinBankMaster bm where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and bm.BankPK =ft.BankFK and T.latestrec ='1' and TransType =1 " + strdate + "  ";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and s.college_code='" + collegecode1 + "'";
                    }
                    #endregion
                }
                else if (txt_rollno.Text.Trim() != "")
                {
                    #region with textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 += " select distinct convert(varchar(10),transdate,103) as transdate from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and f.TransType =1 " + appval + " " + strdate + " ";
                        if (usBasedRights == true)
                            //selquery1 += " and t.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and s.college_code='" + collegecode1 + "'";
                    }
                    else
                    {
                        selquery1 += " select distinct convert(varchar(10),f.transdate,103) as transdate from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T,FT_FinBankTransaction ft,FM_FinBankMaster bm where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and bm.BankPK =ft.BankFK and T.latestrec ='1' and TransType =1 " + appval + " " + strdate + "  ";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and s.college_code='" + collegecode1 + "'";
                    }
                    #endregion
                }
                selquery1 += "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster";

                #endregion
            }
            else if (rbvendor.Checked == true)
            {
                //vendor
                #region Query

                string bkfk = "";
                if (txt_rollno.Text.Trim() == "")
                {
                    #region without textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 = "  SELECT p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,TransCode,CONVERT(varchar(20),TransDate,103) as  TransDate,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(debit) as Amount,ISNULL( IsDeposited,'0') as IsDeposited,ISNULL( IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared   FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =f.App_No and P.VendorType ='1' " + strdate + "";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";

                            selquery1 += "  and college_code='" + collegecode1 + "'";
                        bkfk = "";
                    }
                    else
                    {
                        selquery1 = "  SELECT fb.bankfk,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,TransCode,CONVERT(varchar(20),f.TransDate,103) as  TransDate,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(f.debit) as Amount,ISNULL( f.IsDeposited,'0') as IsDeposited,ISNULL( f.IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared   FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC,FT_FinBankTransaction fb,FM_FinBankMaster bm WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =f.App_No and bm.BankPK =fb.BankFK and f.Transcode=fb.DailyTransId and P.VendorType ='1' " + strdate + "";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and fb.BankFK in('" + bankfk + "')";
                        bkfk = " ,fb.bankfk";
                        // and college_code='" + collegecode1 + "'
                    }
                    #endregion
                }
                else if (txt_rollno.Text.Trim() != "")
                {
                    #region with textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 = "  SELECT p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,TransCode,CONVERT(varchar(20),TransDate,103) as  TransDate,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(debit) as Amount,ISNULL( IsDeposited,'0') as IsDeposited,ISNULL( IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared   FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =f.App_No and P.VendorType ='1' " + appval + " " + strdate + "";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += "  and college_code='" + collegecode1 + "'";
                        bkfk = "";
                    }
                    else
                    {
                        selquery1 = "  SELECT fb.bankfk,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,TransCode,CONVERT(varchar(20),f.TransDate,103) as  TransDate,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(f.debit) as Amount,ISNULL( f.IsDeposited,'0') as IsDeposited,ISNULL( f.IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared   FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC,FT_FinBankTransaction fb,FM_FinBankMaster bm WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =f.App_No and bm.BankPK =fb.BankFK and f.Transcode=fb.DailyTransId and P.VendorType ='1' " + appval + " " + strdate + " ";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and fb.BankFK in('" + bankfk + "')";
                        bkfk = " ,fb.bankfk";
                        //and college_code='" + collegecode1 + "'
                    }
                    #endregion
                }
                selquery1 = selquery1 + "  " + paymod + "";
                if (addsubquery.Trim() != "")
                {
                    selquery1 = selquery1 + " " + addsubquery + "";
                }
                selquery1 = selquery1 + " group by f.TransCode,f.TransDate,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,f.paymode,DDBankCode,DepositBankCode, ddno,dddate ,f.IsDeposited,f.IsBounced,IsCollected,depositedDate,BouncedDate,CollectedDate " + bkfk + " " + orderby + "";
                if (txt_rollno.Text.Trim() == "")
                {
                    #region without textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 += " SELECT distinct convert(varchar(10),transdate,103) as transdate  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =f.App_No and P.VendorType ='1' " + strdate + "";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    else
                    {
                        selquery1 += "  SELECT distinct convert(varchar(10),f.transdate,103) as transdate FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC,FT_FinBankTransaction fb,FM_FinBankMaster bm WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =f.App_No and bm.BankPK =fb.BankFK and f.Transcode=fb.DailyTransId and P.VendorType ='1' " + strdate + " ";
                        if (usBasedRights == true)
                            //    selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    #endregion
                }
                else if (txt_rollno.Text.Trim() != "")
                {
                    #region with textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 += " SELECT distinct convert(varchar(10),transdate,103) as transdate  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =f.App_No and P.VendorType ='1' " + appval + " " + strdate + "";
                        if (usBasedRights == true)
                            //    selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    else
                    {
                        selquery1 += "  SELECT distinct convert(varchar(10),f.transdate,103) as transdate FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC,FT_FinBankTransaction fb,FM_FinBankMaster bm WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =f.App_No and bm.BankPK =fb.BankFK and f.Transcode=fb.DailyTransId and P.VendorType ='1' " + appval + " " + strdate + " ";
                        if (usBasedRights == true)
                            //    selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    #endregion
                }
                selquery1 += "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster";

                #endregion
            }
            else
            {
                //Other
                #region Query

                string bkfk = "";
                if (txt_rollno.Text.Trim() == "")
                {
                    #region without textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 = "  SELECT p.VendorCode,f.App_no,p.VendorCompName,TransCode,CONVERT(varchar(20),TransDate,103) as  TransDate,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(debit) as Amount,ISNULL( IsDeposited,'0') as IsDeposited,ISNULL( IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared   FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE  p.VendorPK  =f.App_No and P.VendorType ='-5' " + strdate + "";
                        if (usBasedRights == true)
                            //    selquery1 += " and f.EntryUserCode in('" + usercode + "')";

                            selquery1 += "  and college_code='" + collegecode1 + "'";
                        bkfk = "";
                    }
                    else
                    {
                        selquery1 = "  SELECT fb.bankfk,p.VendorCode,f.App_no,p.VendorCompName,TransCode,CONVERT(varchar(20),f.TransDate,103) as  TransDate,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(f.debit) as Amount,ISNULL( f.IsDeposited,'0') as IsDeposited,ISNULL( f.IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared   FROM FT_FinDailyTransaction f,CO_VendorMaster P,FT_FinBankTransaction fb,FM_FinBankMaster bm WHERE p.VendorPK =f.App_No and bm.BankPK =fb.BankFK and f.Transcode=fb.DailyTransId and P.VendorType ='-5' " + strdate + " ";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and fb.BankFK in('" + bankfk + "')";
                        bkfk = " ,fb.bankfk";
                        //selquery1 += "  and college_code='" + collegecode1 + "'";
                        ////and college_code='" + collegecode1 + "'
                    }
                    #endregion
                }
                else if (txt_rollno.Text.Trim() != "")
                {
                    #region with textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 = "  SELECT p.VendorCode,f.App_no,p.VendorCompName,TransCode,CONVERT(varchar(20),TransDate,103) as  TransDate,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(debit) as Amount,ISNULL( IsDeposited,'0') as IsDeposited,ISNULL( IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared   FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE  p.VendorPK =f.App_No and P.VendorType ='-5' " + appval + " " + strdate + "";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += "  and college_code='" + collegecode1 + "'";
                        bkfk = "";
                    }
                    else
                    {
                        selquery1 = "  SELECT fb.bankfk,p.VendorCode,f.App_no,p.VendorCompName,TransCode,CONVERT(varchar(20),f.TransDate,103) as  TransDate,case when f.paymode=2 then 'Cheque' when f.paymode='3' then 'DD' end paymode,f.paymode as pay,(select TextVal from textvaltable b where b.TextCode = f.DDBankCode) bankname,(select BankName from FM_FinBankMaster b where b.BankPK = f.DepositBankCode) depositbankname,CONVERT(varchar(10), depositedDate,103) as depositedDate,CONVERT(varchar(10),BouncedDate,103) as BouncedDate,CONVERT(varchar(10),CollectedDate,103) as CollectedDate ,ddno,convert(varchar(10),dddate,103) as dddate,SUM(f.debit) as Amount,ISNULL( f.IsDeposited,'0') as IsDeposited,ISNULL( f.IsBounced,'0') as IsBounced ,ISNULL( IsCollected,'0') as Cleared   FROM FT_FinDailyTransaction f,CO_VendorMaster P,FT_FinBankTransaction fb,FM_FinBankMaster bm WHERE p.VendorPK =f.App_No and bm.BankPK =fb.BankFK and f.Transcode=fb.DailyTransId and P.VendorType ='-5' " + appval + " " + strdate + " and college_code='" + collegecode1 + "'";
                        if (usBasedRights == true)
                            //selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and fb.BankFK in('" + bankfk + "')";
                        bkfk = " ,fb.bankfk";
                    }
                    #endregion
                }
                selquery1 = selquery1 + "  " + paymod + "";
                if (addsubquery.Trim() != "")
                {
                    selquery1 = selquery1 + " " + addsubquery + "";
                }
                selquery1 = selquery1 + " group by f.TransCode,f.TransDate,p.VendorCode,f.App_no,p.VendorCompName,f.paymode,DDBankCode,DepositBankCode, ddno,dddate ,f.IsDeposited,f.IsBounced,IsCollected,depositedDate,BouncedDate,CollectedDate " + bkfk + " " + orderby + "";
                if (txt_rollno.Text.Trim() == "")
                {
                    #region without textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 += " SELECT distinct convert(varchar(10),transdate,103) as transdate  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE  p.VendorPK =f.App_No and P.VendorType ='-5' " + strdate + "";
                        if (usBasedRights == true)
                            //    selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    else
                    {
                        selquery1 += "  SELECT distinct convert(varchar(10),f.transdate,103) as transdate FROM FT_FinDailyTransaction f,CO_VendorMaster P,FT_FinBankTransaction fb,FM_FinBankMaster bm WHERE  p.VendorPK =f.App_No and bm.BankPK =fb.BankFK and f.Transcode=fb.DailyTransId and P.VendorType ='-5' " + strdate + " ";
                        if (usBasedRights == true)
                            //    selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    #endregion
                }
                else if (txt_rollno.Text.Trim() != "")
                {
                    #region with textbox
                    if (rbtodeposit.Checked == true)
                    {
                        selquery1 += " SELECT distinct convert(varchar(10),transdate,103) as transdate  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE  p.VendorPK =f.App_No and P.VendorType ='-5' " + appval + " " + strdate + "";
                        if (usBasedRights == true)
                            //    selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    else
                    {
                        selquery1 += "  SELECT distinct convert(varchar(10),f.transdate,103) as transdate FROM FT_FinDailyTransaction f,CO_VendorMaster P,FT_FinBankTransaction fb,FM_FinBankMaster bm WHERE p.VendorPK =f.App_No and bm.BankPK =fb.BankFK and f.Transcode=fb.DailyTransId and P.VendorType ='-5' " + appval + " " + strdate + " ";
                        if (usBasedRights == true)
                            //    selquery1 += " and f.EntryUserCode in('" + usercode + "')";
                            selquery1 += " and college_code='" + collegecode1 + "'";
                    }
                    #endregion
                }
                selquery1 += "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster";

                #endregion
            }
            #endregion
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selquery1, "Text");
            DataView dv = new DataView();
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 1;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Columns[0].Locked = true;


                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.Black;
                style2.BackColor = Color.AliceBlue;
                FarPoint.Web.Spread.CheckBoxCellType selall = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                selall.AutoPostBack = true;
                cb.AutoPostBack = true;
                FarPoint.Web.Spread.TextCellType txtdd = new FarPoint.Web.Spread.TextCellType();
                string name = "";


                #region Item list

                for (int i = 0; i < ItemList.Count; i++)
                {
                    string value1 = ItemList[i].ToString();
                    int a = value1.Length;
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = ItemList[i].ToString();
                    if (rbentry.Checked == true)
                    {
                        if (rbtodeposit.Checked == true)
                        {
                            if (Convert.ToString(ItemList[i]) == "Deposited Date" || Convert.ToString(ItemList[i]) == "Bounced Date" || Convert.ToString(ItemList[i]) == "Cleared Date")
                            {
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                            }
                        }
                        if (rbdeposit.Checked == true)
                        {
                            if (Convert.ToString(ItemList[i]) == "Deposited Date" || Convert.ToString(ItemList[i]) == "Bounced Date" || Convert.ToString(ItemList[i]) == "Cleared Date")
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                        }
                        if (rbbounce.Checked == true)
                        {
                            if (Convert.ToString(ItemList[i]) == "Deposited Date" || Convert.ToString(ItemList[i]) == "Bounced Date" || Convert.ToString(ItemList[i]) == "Cleared Date")
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                        }
                        if (rbclear.Checked == true)
                        {
                            if (Convert.ToString(ItemList[i]) == "Deposited Date" || Convert.ToString(ItemList[i]) == "Bounced Date" || Convert.ToString(ItemList[i]) == "Cleared Date")
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                        }
                    }
                    else
                    {
                        if (rbdeposit.Checked == false)
                        {
                            if (Convert.ToString(ItemList[i]) == "Deposited Date")
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                        }
                        if (rbbounce.Checked == false)
                        {
                            if (Convert.ToString(ItemList[i]) == "Bounced Date")
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                        }
                        if (rbclear.Checked == false)
                        {
                            if (Convert.ToString(ItemList[i]) == "Cleared Date")
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                        }
                    }
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                    if (i == 0 || i == 1)
                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 225;
                    else
                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 125;
                }
                #endregion

                #region fpread header

                int coutval = 0;
                for (int i = 0; i < ItemEmpty.Count; i++)
                {
                    string value1 = ItemEmpty[i].ToString();
                    int a = value1.Length;
                    FpSpread1.Sheets[0].ColumnCount++;
                    coutval++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = ItemEmpty[i].ToString();
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = ItemEmpty[i].ToString();

                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    if (!hasvalue.ContainsKey(value1))
                    {
                        hasvalue.Add(value1, FpSpread1.Sheets[0].ColumnCount);
                    }
                }

                #endregion
                ViewState["hasvalue"] = hasvalue;
                ArrayList arbank = new ArrayList();
                for (int sel = 0; sel < dsload.Tables[1].Rows.Count; sel++)
                {
                    dsload.Tables[0].DefaultView.RowFilter = "transdate='" + Convert.ToString(dsload.Tables[1].Rows[sel]["transdate"]) + "'";
                    dv = dsload.Tables[0].DefaultView;
                    DataSet dsdata = new DataSet();
                    DataTable dtft = new DataTable();
                    if (dv.Count > 0)
                    {
                        dtft = dv.ToTable();
                        dsdata.Clear();
                        dsdata.Tables.Add(dtft.Copy());

                        #region dataset

                        for (int i = 0; i < dsdata.Tables[0].Rows.Count; i++)
                        {
                            if (count == 0)
                            {
                                if (hasvalue.Contains("Deposited"))
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    int col = Convert.ToInt32(hasvalue["Deposited"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col - 1].CellType = selall;
                                }
                                if (hasvalue.Contains("Bounce"))
                                {
                                    // FpSpread1.Sheets[0].RowCount++;
                                    // int col = Convert.ToInt32(hasvalue["Bounce"]);
                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col - 1].CellType = selall;
                                }
                                if (hasvalue.Contains("Cleared"))
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    int col = Convert.ToInt32(hasvalue["Cleared"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col - 1].CellType = selall;
                                }
                            }

                            #region bankfk

                            string bkname = "";
                            bool bkflag = false;
                            if (rbtodeposit.Checked == false)
                            {
                                DataView Dview = new DataView();
                                if (dsload.Tables[2].Rows.Count > 0)
                                {
                                    dsload.Tables[2].DefaultView.RowFilter = "BankPK='" + Convert.ToString(dsdata.Tables[0].Rows[i]["BankFK"]) + "'";
                                    Dview = dsload.Tables[2].DefaultView;
                                    if (Dview.Count > 0)
                                    {
                                        bkname = Convert.ToString(Dview[0]["BankName"]);
                                        if (!arbank.Contains(Convert.ToString(Dview[0]["BankPK"])))
                                        {
                                            arbank.Add(Convert.ToString(Dview[0]["BankPK"]));
                                            bkflag = true;
                                        }
                                    }
                                }
                            }

                            if (rbdeposit.Checked == true)
                            {
                                if (bkflag == true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = bkname;
                                    //  FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.White;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Bold.ToString();
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);

                                }
                            }
                            if (rbbounce.Checked == true)
                            {
                                if (bkflag == true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = bkname;
                                    //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.White;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                }
                            }
                            if (rbclear.Checked == true)
                            {
                                if (bkflag == true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = bkname;
                                    // FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.White;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                }
                            }

                            #endregion

                            FpSpread1.Sheets[0].RowCount++;
                            count++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dsdata.Tables[0].Rows[i]["pay"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                            int c = 0;
                            for (int j = 0; j < ItemList.Count; j++)
                            {

                                string k = Convert.ToString(ItemList[j]);
                                string l = Convert.ToString(ht[k].ToString()).ToUpperInvariant();
                                c++;
                                FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                FpSpread1.Sheets[0].Columns[1].CellType = textcel_type;

                                if (Convert.ToString(ItemList[j]) == "Receipt Date")
                                {
                                    if (!htdt.ContainsKey(Convert.ToString(dsdata.Tables[0].Rows[i]["transdate"])))
                                    {
                                        htdt.Add(Convert.ToString(dsdata.Tables[0].Rows[i]["transdate"]), Convert.ToString(dsdata.Tables[0].Rows[i]["Amount"]));
                                    }
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htdt[Convert.ToString(dsdata.Tables[0].Rows[i]["transdate"])]), out amount);
                                        amount += Convert.ToDouble(dsdata.Tables[0].Rows[i]["Amount"]);
                                        htdt.Remove(Convert.ToString(dsdata.Tables[0].Rows[i]["transdate"]));
                                        htdt.Add(Convert.ToString(dsdata.Tables[0].Rows[i]["transdate"]), Convert.ToString(amount));
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dsdata.Tables[0].Rows[i]["transdate"]);
                                }

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = dsdata.Tables[0].Rows[i][l].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Tag = dsdata.Tables[0].Rows[i]["transcode"].ToString();
                                string txtddno = Convert.ToString(dsdata.Tables[0].Rows[i]["ddno"]);
                                if (k.Trim() == "DD/Cheque No")
                                    FpSpread1.Sheets[0].Columns[c].CellType = txtdd;


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                            }
                            int bounvalue = 0;
                            int clervalue = 0;
                            int depostevalue = 0;
                            bool bouncecheck = false;
                            bool clearedcheck = false;
                            int bounctcol = 0;
                            int cleareadcod = 0;
                            if (rbentry.Checked == true)
                            {
                                #region  entry

                                for (int ik = 0; ik < ItemEmpty.Count; ik++)
                                {
                                    c++;
                                    int value = 0;
                                    string newgetvalue = "";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].CellType = cb;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                    if (Convert.ToString(ItemEmpty[ik]) == "ToBe Deposited")
                                    {
                                        newgetvalue = Convert.ToString(dsdata.Tables[0].Rows[i]["IsDeposited"]);
                                        bounvalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["IsBounced"]);
                                        clervalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["Cleared"]);
                                        //   depositedDate,BouncedDate,CollectedDate
                                        cb.AutoPostBack = false;
                                        if (newgetvalue.Trim() == "False" || newgetvalue.Trim() == "0")
                                        {
                                            //depostevalue = 1;
                                            value = 0;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Value = value;
                                            // FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                                            if (value == 1)
                                                FpSpread1.Sheets[0].Rows[i + 1].BackColor = ColorTranslator.FromHtml("#e5e5ff");
                                        }
                                        if (depostevalue == 1)
                                            FpSpread1.Sheets[0].Rows[i + 1].BackColor = ColorTranslator.FromHtml("#e5e5ff");

                                        divbtn.Visible = true;
                                        lbl_bankname.Visible = true;
                                        ddl_bankname.Visible = true;
                                        ddlotherBank.Visible = true;
                                        //dept = 1;
                                    }
                                    if (Convert.ToString(ItemEmpty[ik]) == "Deposited")
                                    {
                                        newgetvalue = Convert.ToString(dsdata.Tables[0].Rows[i]["IsDeposited"]);
                                        bounvalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["IsBounced"]);
                                        clervalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["Cleared"]);
                                        //   depositedDate,BouncedDate,CollectedDate
                                        cb.AutoPostBack = false;
                                        if (newgetvalue.Trim() != "False" && newgetvalue.Trim() != "0")
                                        {
                                            depostevalue = 1;
                                            value = 1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Value = value;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                                            divbtn.Visible = false;
                                            //if (value == 1)
                                            //  FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#e598ff");
                                            if (bounvalue == 1)
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                            else if (clervalue == 1)
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#bfff00");
                                            else
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#e598ff");

                                        }
                                        else
                                        {
                                            divbtn.Visible = true;
                                        }
                                        //if (depostevalue == 1)
                                        //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#e598ff");


                                        lbl_bankname.Visible = true;
                                        ddl_bankname.Visible = true;
                                        ddlotherBank.Visible = true;
                                        tdseldt.Visible = true;
                                        tdseltxtdt.Visible = true;
                                        //  dept = 1;
                                    }

                                    if (Convert.ToString(ItemEmpty[ik]) == "Bounce")
                                    {
                                        bounctcol = c;
                                        newgetvalue = Convert.ToString(dsdata.Tables[0].Rows[i]["IsBounced"]);
                                        clervalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["Cleared"]);
                                        cb.AutoPostBack = true;
                                        if (newgetvalue.Trim() != "False" && newgetvalue.Trim() != "0")
                                        {
                                            value = 1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Value = value;
                                            if (value == 1)
                                            {
                                                FpSpread1.Sheets[0].Rows[i + 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;

                                            }
                                        }
                                        //else
                                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;


                                        if (depostevalue == 1)
                                        {
                                            if (value == 1)
                                            {
                                                bouncecheck = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                            }
                                            else
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = false;

                                        }
                                        if (clervalue == 1)
                                        {
                                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#bfff00");
                                        }
                                        else if (newgetvalue == "0" || newgetvalue == "False")
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#e598ff");

                                        //if (clervalue == 1)
                                        //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");

                                        divbtn.Visible = true;
                                        lbl_bankname.Visible = false;
                                        ddl_bankname.Visible = false;
                                        ddlotherBank.Visible = false;
                                        tdseldt.Visible = false;
                                        tdseltxtdt.Visible = false;
                                        // boun = 1;
                                    }
                                    if (Convert.ToString(ItemEmpty[ik]) == "Cleared")
                                    {
                                        cleareadcod = c;
                                        clervalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["IsDeposited"]);
                                        newgetvalue = Convert.ToString(dsdata.Tables[0].Rows[i]["Cleared"]);
                                        bounvalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["IsBounced"]);

                                        cb.AutoPostBack = false;
                                        if (newgetvalue.Trim() == "False" || newgetvalue.Trim() == "0")
                                        {
                                            value = 0;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Value = value;
                                            if (value == 1)
                                            {
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#bfff00");
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                            }
                                        }
                                        else
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;


                                        if (depostevalue == 1)
                                        {
                                            if (value == 1)
                                            {
                                                clearedcheck = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#bfff00");
                                            }
                                            else
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = false;

                                        }
                                        if (bounvalue == 1)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                        }
                                        else
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#e598ff");
                                        divbtn.Visible = true;
                                        lbl_bankname.Visible = false;
                                        ddl_bankname.Visible = false;
                                        ddlotherBank.Visible = false;
                                        tdseldt.Visible = false;
                                        tdseltxtdt.Visible = false;
                                        // boun = 1;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region report

                                for (int ik = 0; ik < ItemEmpty.Count; ik++)
                                {
                                    c++;
                                    int value = 0;
                                    string newgetvalue = "";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].CellType = cb;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                    if (rbtodeposit.Checked == true)
                                    {
                                        if (Convert.ToString(ItemEmpty[ik]) == "Deposited")
                                        {
                                            newgetvalue = Convert.ToString(dsdata.Tables[0].Rows[i]["IsDeposited"]);
                                            bounvalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["IsBounced"]);
                                            clervalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["Cleared"]);
                                            //   depositedDate,BouncedDate,CollectedDate
                                            if (newgetvalue.Trim() == "False" || newgetvalue.Trim() == "0")
                                            {
                                                //depostevalue = 1;
                                                value = 0;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Value = value;
                                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                                                if (value == 1)
                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#e5e5ff");
                                            }
                                            if (depostevalue == 1)
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#e5e5ff");

                                            divbtn.Visible = false;
                                            lbl_bankname.Visible = true;
                                            ddl_bankname.Visible = true;
                                            ddlotherBank.Visible = true;
                                            //dept = 1;
                                        }

                                    }
                                    if (rbtodeposit.Checked == false)
                                    {
                                        if (Convert.ToString(ItemEmpty[ik]) == "Deposited")
                                        {
                                            newgetvalue = Convert.ToString(dsdata.Tables[0].Rows[i]["IsDeposited"]);
                                            bounvalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["IsBounced"]);
                                            clervalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["Cleared"]);

                                            //   depositedDate,BouncedDate,CollectedDate
                                            if (newgetvalue.Trim() != "False" && newgetvalue.Trim() != "0")
                                            {
                                                // depostevalue = 1;
                                                value = 1;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Value = value;
                                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                                                if (value == 1)
                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#e598ff");
                                            }
                                            if (depostevalue == 1)
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#e598ff");

                                            divbtn.Visible = false;
                                            //lbl_bankname.Visible = true;
                                            //ddl_bankname.Visible = true;
                                            //dept = 1;
                                        }
                                    }
                                    if (Convert.ToString(ItemEmpty[ik]) == "Bounce")
                                    {
                                        bounctcol = c;
                                        newgetvalue = Convert.ToString(dsdata.Tables[0].Rows[i]["IsBounced"]);
                                        clervalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["Cleared"]);
                                        if (newgetvalue.Trim() != "False" && newgetvalue.Trim() != "0")
                                        {
                                            value = 1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Value = value;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                                            if (value == 1)
                                            {
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;

                                            }
                                        }
                                        else
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;


                                        if (depostevalue == 1)
                                        {
                                            if (value == 1)
                                            {
                                                bouncecheck = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                            }
                                            else
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = false;

                                        }
                                        //if (clervalue == 1)
                                        //{
                                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                        //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#bfff00");
                                        //}
                                        divbtn.Visible = false;
                                        lbl_bankname.Visible = false;
                                        ddl_bankname.Visible = false;
                                        ddlotherBank.Visible = false;
                                        //boun = 1;
                                    }
                                    if (Convert.ToString(ItemEmpty[ik]) == "Cleared")
                                    {
                                        cleareadcod = c;
                                        newgetvalue = Convert.ToString(dsdata.Tables[0].Rows[i]["Cleared"]);
                                        bounvalue = Convert.ToInt32(dsdata.Tables[0].Rows[i]["IsBounced"]);
                                        if (newgetvalue.Trim() != "False" && newgetvalue.Trim() != "0")
                                        {
                                            value = 1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Value = value;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                                            if (value == 1)
                                            {
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#bfff00");
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                            }
                                        }
                                        else
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;


                                        if (depostevalue == 1)
                                        {
                                            if (value == 1)
                                            {
                                                clearedcheck = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#bfff00");
                                            }
                                            else
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = false;

                                        }
                                        if (bounvalue == 1)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Locked = true;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                        }
                                        divbtn.Visible = false;
                                        lbl_bankname.Visible = false;
                                        ddl_bankname.Visible = false;
                                        ddlotherBank.Visible = false;
                                        //  boun = 1;
                                    }
                                }
                                #endregion
                            }
                            if (bouncecheck == true || clearedcheck == true)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, bounctcol].Locked = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cleareadcod].Locked = true;
                            }
                        }

                        #endregion

                        #region total

                        if (rbtodeposit.Checked == false)
                        {
                            if (hasvalue.ContainsKey("Deposited"))
                            {
                                FpSpread1.Sheets[0].Rows.Count++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Green;
                                double grandvalue = 0;
                                foreach (DictionaryEntry amt in htdt)
                                {
                                    double.TryParse(Convert.ToString(amt.Value), out grandvalue);
                                    string date = amt.Key.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = date;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 12].Text = Convert.ToString(grandvalue);
                                }
                                htdt.Clear();
                            }
                        }
                        #endregion
                    }
                    if (rbentry.Checked == true)
                    {
                        if (rbtodeposit.Checked == true)
                        {
                            lbl_bankname.Visible = true;
                            ddl_bankname.Visible = true;
                            ddlotherBank.Visible = true;
                        }
                        else
                        {
                            lbl_bankname.Visible = false;
                            ddl_bankname.Visible = false;
                            ddlotherBank.Visible = false;
                        }
                    }
                    else
                    {
                        lbl_bankname.Visible = false;
                        ddl_bankname.Visible = false;
                        ddlotherBank.Visible = false;
                    }
                    for (int m = 0; m < FpSpread1.Sheets[0].Columns.Count; m++)
                    {
                        FpSpread1.Sheets[0].Columns[m].HorizontalAlign = HorizontalAlign.Center;
                        if (m == 0 || m == 4)
                            if (m == 0)
                                FpSpread1.Sheets[0].Columns[m].Width = 40;
                            else
                                FpSpread1.Sheets[0].Columns[m].Width = 60;

                        else
                            FpSpread1.Sheets[0].Columns[m].Width = 100;
                    }
                    FpSpread1.Sheets[0].Columns[1].Width = 81;
                    FpSpread1.Sheets[0].Columns[2].Width = 160;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Width = 70;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 80;
                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                    // FpSpread1.Width = 800;
                    FpSpread1.Height = 470;
                    FpSpread1.ShowHeaderSelection = false;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    // int cnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    // FpSpread1.Sheets[0].FrozenColumnCount = 2;
                    FpSpread1.SaveChanges();
                    //  div1.Visible = true;
                    FpSpread1.Visible = true;
                    pheaderfilter.Visible = true;
                    pcolumnorder.Visible = true;
                    //divbtn.Visible = true;
                    btn_save.Visible = true;
                    print.Visible = true;
                    lblvalidation1.Text = "";
                    txtexcelname.Text = "";
                    fldtot.Visible = true;
                    divlbl.Visible = true;
                    sumOfAmt();
                }
            }
            else
            {
                divbtn.Visible = false;
                btn_save.Visible = false;
                pheaderfilter.Visible = false;
                FpSpread1.Visible = false;
                // div1.Visible = false;
                fldtot.Visible = false;
                divlbl.Visible = false;
                fldtot.Visible = false;
                print.Visible = false;
                lbl_alert.Text = "No Record found";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;

            }
        }
        catch
        {
        }

    }

    protected void sumOfAmt()
    {
        try
        {
            double tobeAmt = 0;
            double deptAmt = 0;
            double bounAmt = 0;
            double clrAmt = 0;
            txttobe.Text = "";
            txtdept.Text = "";
            txtboun.Text = "";
            txtclr.Text = "";
            trtobe.Visible = false;
            trdept.Visible = false;
            trboun.Visible = false;
            trclr.Visible = false;
            int toval = 0;
            int deptval = 0;
            int bounval = 0;
            int clrval = 0;
            if (rbtodeposit.Checked == true)
                toval = 1;
            else if (rbdeposit.Checked == true)
                deptval = 1;
            else if (rbbounce.Checked == true)
                bounval = 1;
            else if (rbclear.Checked == true)
                clrval = 1;

            if (FpSpread1.Sheets[0].Rows.Count > 0)
            {
                int tobe = 0;
                int dept = 0;
                int boun = 0;
                int clr = 0;
                double tobecolvalue = 0;
                double deptcol = 0;
                double bouncol = 0;
                double clrcol = 0;
                if (ViewState["hasvalue"] != null)
                    hasvalue = (Hashtable)ViewState["hasvalue"];

                if (hasvalue.Count > 0)
                {
                    if (hasvalue.ContainsKey("Deposited"))
                        dept = Convert.ToInt32(hasvalue["Deposited"]);

                    if (hasvalue.ContainsKey("Bounce"))
                        boun = Convert.ToInt32(hasvalue["Bounce"]);

                    if (hasvalue.ContainsKey("Cleared"))
                        clr = Convert.ToInt32(hasvalue["Cleared"]);

                    for (int fp = 0; fp < FpSpread1.Sheets[0].Rows.Count; fp++)
                    {
                        if (fp == 0)
                            continue;
                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 12].Value), out tobecolvalue);
                        if (dept != 0)
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[fp, dept - 1].Value), out deptcol);
                        if (boun != 0)
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[fp, boun - 1].Value), out bouncol);
                        if (clr != 0)
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[fp, clr - 1].Value), out clrcol);

                        if (deptcol == 1)
                            deptAmt += tobecolvalue;

                        if (bouncol == 1)
                            bounAmt += tobecolvalue;

                        if (clrcol == 1)
                            clrAmt += tobecolvalue;

                        // if (deptcol == 0 && bouncol == 0 && clrcol == 0)
                        tobeAmt += tobecolvalue;
                    }
                    txttobe.Text = Convert.ToString(tobeAmt);
                    txtdept.Text = Convert.ToString(deptAmt);
                    txtboun.Text = Convert.ToString(bounAmt);
                    txtclr.Text = Convert.ToString(clrAmt);
                    #region old
                    //if (chkselall.Checked == true)
                    //{
                    //    trtobe.Visible = true;
                    //    trdept.Visible = true;
                    //    trboun.Visible = true;
                    //    trclr.Visible = true;
                    //}
                    //else
                    //{
                    //    if (toval == 1 && deptval == 1 && bounval == 1)
                    //    {
                    //        trtobe.Visible = true;
                    //        trdept.Visible = true;
                    //        trboun.Visible = true;
                    //    }
                    //    else if (toval == 1 && deptval == 1 && clrval == 1)
                    //    {
                    //        trtobe.Visible = true;
                    //        trdept.Visible = true;
                    //        trclr.Visible = true;
                    //    }
                    //    else if (deptval == 1 && bounval == 1 && clrval == 1)
                    //    {
                    //        trdept.Visible = true;
                    //        trboun.Visible = true;
                    //        trclr.Visible = true;
                    //    }
                    //    else if (toval == 1 && bounval == 1 && clrval == 1)
                    //    {
                    //        trtobe.Visible = true;
                    //        trboun.Visible = true;
                    //        trclr.Visible = true;
                    //    }
                    //    else if (toval == 1 && deptval == 1)
                    //    {
                    //        trtobe.Visible = true;
                    //        trdept.Visible = true;
                    //    }
                    //    else if (toval == 1 && bounval == 1)
                    //    {
                    //        trtobe.Visible = true;
                    //        trboun.Visible = true;
                    //    }
                    //    else if (toval == 1 && clrval == 1)
                    //    {
                    //        trtobe.Visible = true;
                    //        trclr.Visible = true;
                    //    }
                    //    else if (deptval == 1 && bounval == 1)
                    //    {
                    //        trdept.Visible = true;
                    //        trboun.Visible = true;
                    //    }
                    //    else if (deptval == 1 && clrval == 1)
                    //    {
                    //        trdept.Visible = true;
                    //        trclr.Visible = true;
                    //    }
                    //    else if (bounval == 1 && clrval == 1)
                    //    {
                    //        trboun.Visible = true;
                    //        trclr.Visible = true;
                    //    }
                    //    else if (toval == 1)
                    //    {
                    //        trtobe.Visible = true;
                    //    }
                    //    else if (deptval == 1)
                    //    {
                    //        trdept.Visible = true;
                    //    }
                    //    else if (bounval == 1)
                    //    {
                    //        trboun.Visible = true;
                    //    }
                    //    else if (clrval == 1)
                    //    {
                    //        trclr.Visible = true;
                    //    }
                    //}
                    #endregion

                    if (rbentry.Checked == true)
                    {
                        if (rbtodeposit.Checked == true)
                        {
                            if (hasvalue.ContainsKey("Deposited"))
                            {
                                if (toval == 1)
                                    trtobe.Visible = true;
                                else
                                {
                                    trdept.Visible = false;
                                    trboun.Visible = false;
                                    trclr.Visible = false;
                                }
                            }
                        }
                        if (rbtodeposit.Checked == false)
                        {
                            if (hasvalue.ContainsKey("Deposited"))
                            {
                                if (deptval == 1)
                                    trdept.Visible = true;
                                else
                                {
                                    trtobe.Visible = false;
                                    trboun.Visible = false;
                                    trclr.Visible = false;
                                }
                            }
                        }
                        if (hasvalue.ContainsKey("Bounce"))
                        {
                            if (bounval == 1)
                                trboun.Visible = true;
                            else
                            {
                                trtobe.Visible = false;
                                trdept.Visible = false;
                                trclr.Visible = false;
                            }
                        }
                        if (hasvalue.ContainsKey("Cleared"))
                        {
                            if (clrval == 1)
                                trclr.Visible = true;
                            else
                            {
                                trtobe.Visible = false;
                                trdept.Visible = false;
                                trboun.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (hasvalue.ContainsKey("ToBe Deposited"))
                        {
                            if (toval == 1)
                                trtobe.Visible = true;
                            else
                            {
                                trdept.Visible = false;
                                trboun.Visible = false;
                                trclr.Visible = false;
                            }
                        }
                        if (hasvalue.ContainsKey("Deposited"))
                        {
                            if (deptval == 1)
                                trdept.Visible = true;
                            else
                            {
                                trtobe.Visible = false;
                                trboun.Visible = false;
                                trclr.Visible = false;
                            }
                        }
                        if (hasvalue.ContainsKey("Bounce"))
                        {
                            if (bounval == 1)
                            {
                                trboun.Visible = true;
                                // trdept.Visible = true;
                            }
                            else
                            {
                                trtobe.Visible = false;
                                trdept.Visible = false;
                                trclr.Visible = false;
                            }
                        }
                        if (hasvalue.ContainsKey("Cleared"))
                        {
                            if (clrval == 1)
                            {
                                // trdept.Visible = true;
                                trclr.Visible = true;
                            }
                            else
                            {
                                trtobe.Visible = false;
                                trdept.Visible = false;
                                trboun.Visible = false;
                            }
                        }
                    }
                    fldtot.Visible = true;
                }
            }
        }
        catch { }
    }

    public void FpSpread1_OnUpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int a = Convert.ToInt32(actrow);
            int b = Convert.ToInt32(actcol);
            int valco = 0;
            if (actrow.Trim() != "" && actcol.Trim() != "")
            {
                int get = 0;
                int getnew = 0;
                if (ViewState["hasvalue"] != null)
                    hasvalue = (Hashtable)ViewState["hasvalue"];

                if (hasvalue.ContainsKey("Bounce"))
                {
                    get = Convert.ToInt32(hasvalue["Bounce"]);
                }
                if (hasvalue.ContainsKey("Cleared"))
                {
                    getnew = Convert.ToInt32(hasvalue["Cleared"]);
                }

                if (getnew != 0 && get != 0)
                {
                    if (getnew - 1 == Convert.ToInt32(b))
                    {
                        FpSpread1.Sheets[0].Cells[a, get - 1].Value = 0;
                    }
                    if (get - 1 == Convert.ToInt32(b))
                    {
                        FpSpread1.Sheets[0].Cells[a, getnew - 1].Value = 0;
                    }
                }
            }

        }
        catch
        { }
    }
    protected void FpSpread1_OnButtonCommand(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
        string actcol = FpSpread1.Sheets[0].ActiveRow.ToString();
        string value = "";
        string position = "";
        string deptvalue = "";
        //if (ViewState["hasvalue"] != null)
        //    hasvalue = (Hashtable)ViewState["hasvalue"];
        if (actrow != "" && actcol != "")
        {
            if (hasvalue.ContainsKey("Deposited"))
            {
                position = Convert.ToString(hasvalue["Deposited"]);
                if (Convert.ToInt32(actrow) == 0)
                {
                    value = Convert.ToString(FpSpread1.Sheets[0].Cells[0, Convert.ToInt32(position) - 1].Value);
                    if (value == "1")
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            //if (i == 1)
                            //    continue;
                            if (FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Value = 1;
                                // sumOfAmt();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            //if (i == 1)
                            //    continue;
                            if (FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Value = 0;
                                // sumOfAmt();
                            }
                        }
                    }
                }
            }
            if (hasvalue.ContainsKey("Bounce"))
            {
                position = Convert.ToString(hasvalue["Bounce"]);
                if (Convert.ToInt32(actrow) == 0)
                {
                    value = Convert.ToString(FpSpread1.Sheets[0].Cells[0, Convert.ToInt32(position) - 1].Value);
                    if (value == "1")
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            if (i == 1)
                                continue;
                            if (FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Value = 1;
                                // sumOfAmt();
                            }
                        }
                        //btn_save_Onclick(sender, e);
                    }
                    else
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            if (i == 1)
                                continue;
                            if (FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Value = 0;
                                // sumOfAmt();
                            }
                        }

                    }
                }
                else
                {
                    // btn_save_Onclick(sender, e);
                    actRowValueCellclick();
                }
            }
            if (hasvalue.ContainsKey("Cleared"))
            {
                position = Convert.ToString(hasvalue["Cleared"]);
                if (Convert.ToInt32(actrow) == 0)
                {
                    value = Convert.ToString(FpSpread1.Sheets[0].Cells[0, Convert.ToInt32(position) - 1].Value);
                    if (value == "1")
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            if (i == 1)
                                continue;

                            if (FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Value = 1;
                                // sumOfAmt();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            if (i == 1)
                                continue;
                            if (FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(position) - 1].Value = 0;
                                // sumOfAmt();
                            }
                        }
                    }
                }
            }
            sumOfAmt();

        }
    }
    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {

    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {

        string reportname = Convert.ToString(txtexcelname.Text);
        if (reportname.ToString().Trim() != "")
        {
            d2.printexcelreport(FpSpread1, reportname);
            lblvalidation1.Visible = false;
        }
        else
        {
            lblvalidation1.Text = "Please Enter Your Report Name";
            lblvalidation1.Visible = true;
            txtexcelname.Focus();
        }

    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "";
            string pagename = "";
            lblvalidation1.Text = "";
            degreedetails = "Bank Reconciliation";
            pagename = "BankReconciliation.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch { }

    }
    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["ItemList"] != null)
            {
                ItemList = (ArrayList)ViewState["ItemList"];
            }
            if (ViewState["Itemindex"] != null)
            {
                Itemindex = (ArrayList)ViewState["Itemindex"];
            }
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    if (tborder.Text == "")
                    {
                        ItemList.Add("Roll No");
                    }
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Remove(sindex);

                }
            }

            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                tborder.Text = tborder.Text + ItemList[i].ToString();

                tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList.Count == 22)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }

            //  Button2.Focus();
            ViewState["ItemList"] = ItemList;
            ViewState["Itemindex"] = Itemindex;
        }
        catch (Exception ex)
        {

        }
    }
    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["ItemList"] != null)
            {
                ItemList = (ArrayList)ViewState["ItemList"];
            }
            if (ViewState["Itemindex"] != null)
            {
                Itemindex = (ArrayList)ViewState["Itemindex"];
            }

            if (CheckBox_column.Checked == true)
            {
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    tborder.Text = tborder.Text + ItemList[i].ToString();

                    tborder.Text = tborder.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    cblcolumnorder.Items[0].Enabled = false;
                }

                tborder.Text = "";
                tborder.Visible = false;
            }
            ViewState["ItemList"] = ItemList;
            ViewState["Itemindex"] = Itemindex;
            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbankname()
    {
        ddl_bankname.Items.Clear();
        string selquery = "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster where collegecode='" + ddl_collegename.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_bankname.DataSource = ds;
            ddl_bankname.DataTextField = "BankName";
            ddl_bankname.DataValueField = "BankPK";
            ddl_bankname.DataBind();
        }
    }

    public void bindOtherbankname()
    {
        ddlotherBank.Items.Clear();
        string selquery = "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster where collegecode<>'" + ddl_collegename.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlotherBank.DataSource = ds;
            ddlotherBank.DataTextField = "BankName";
            ddlotherBank.DataValueField = "BankPK";
            ddlotherBank.DataBind();
            ddlotherBank.Items.Insert(0, "Select");
        }
        else
            ddlotherBank.Items.Insert(0, "Select");
    }

    public void binddate()
    {
        try
        {
            string selquery = "select LinkValue from InsSettings where LinkName like 'Current%'  and college_code='" + collegecode1 + "'";
            string acctid = d2.GetFunction(selquery);
            string selq = "select FinYearStart,FinYearEnd from FM_FinYearMaster where FinYearPK='" + acctid + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string yearstart = ds.Tables[0].Rows[0]["FinYearStart"].ToString();
                string yearend = ds.Tables[0].Rows[0]["FinYearEnd"].ToString();

                string[] split;
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                split = yearstart.Split('/');
                dt = Convert.ToDateTime(split[0] + "/" + split[1] + "/" + split[2]);
                split = yearend.Split('/');
                dt1 = Convert.ToDateTime(split[0] + "/" + split[1] + "/" + split[2]);

                txt_fromdate.Text = dt.ToString("dd/MM/yyyy");
                txt_todate.Text = dt1.ToString("dd/MM/yyyy");
            }
        }
        catch
        {

        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        btn_go_Click(sender, e);
        imgdiv2.Visible = false;
        //btn_go_Click(sender, e);
    }
    protected void btnalert_Click(object sender, EventArgs e)
    {
        Div2.Visible = false;
        btn_go_Click(sender, e);
    }

    protected void actRowValueCellclick()
    {
        try
        {
            double bounAmt = 0;
            if (FpSpread1.Sheets[0].Rows.Count > 0)
            {
                int boun = 0;
                double tobecolvalue = 0;
                double bouncol = 0;
                if (ViewState["hasvalue"] != null)
                    hasvalue = (Hashtable)ViewState["hasvalue"];

                if (hasvalue.ContainsKey("Bounce"))
                    boun = Convert.ToInt32(hasvalue["Bounce"]);

                bool value = false;
                string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
                string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
                if (actrow != "" && actcol != "")
                {
                    int arow = Convert.ToInt32(actrow);
                    int acol = Convert.ToInt32(actcol);
                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[arow, 12].Value), out tobecolvalue);
                    if (boun != 0)
                    {
                        if (FpSpread1.Sheets[0].Cells[arow, boun - 1].Locked == false)
                        {
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[arow, boun - 1].Value), out bouncol);
                            value = true;
                        }
                    }
                }
                if (value == true && bouncol == 1)
                    bounAmt += tobecolvalue;
            }
            if (hasvalue.ContainsKey("Bounce"))
            {
                if (bounAmt != 0 || bounAmt != 0.00)
                {
                    rbcancel.Checked = false;
                    rbredept.Checked = false;
                    rbcancel.Visible = true;
                    rbredept.Visible = true;
                    btnsavebounce.Visible = true;
                    btnsavebn.Visible = false;
                    Label4.Text = Convert.ToString(bounAmt);
                    // Label2.Text = "Do You Want To Continue Yes/No";
                    Label2.Visible = false;
                    divbounce.Visible = true;
                }
            }
        }
        catch { }
    }

    public void btn_save_Onclick(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            double deptAmt = 0;
            double bounAmt = 0;
            double clrAmt = 0;
            double totamt = 0;
            if (FpSpread1.Sheets[0].Rows.Count > 0)
            {
                int dept = 0;
                int boun = 0;
                int clr = 0;
                double tobecolvalue = 0;
                double deptcol = 0;
                double bouncol = 0;
                double clrcol = 0;
                int amtcol = 0;
                if (ViewState["hasvalue"] != null)
                    hasvalue = (Hashtable)ViewState["hasvalue"];

                if (hasvalue.ContainsKey("Deposited"))
                {
                    dept = Convert.ToInt32(hasvalue["Deposited"]);
                    amtcol = dept - 2;
                }
                if (hasvalue.ContainsKey("Bounce"))
                {
                    boun = Convert.ToInt32(hasvalue["Bounce"]);
                    amtcol = boun - 2;
                }
                if (hasvalue.ContainsKey("Cleared"))
                {
                    clr = Convert.ToInt32(hasvalue["Cleared"]);
                    amtcol = clr - 2;
                }
                for (int fp = 0; fp < FpSpread1.Sheets[0].Rows.Count; fp++)
                {
                    bool value = false;
                    if (fp == 0)
                        continue;

                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 12].Value), out tobecolvalue);
                    if (dept != 0)
                    {
                        if (FpSpread1.Sheets[0].Cells[fp, dept - 1].Locked == false)
                        {
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[fp, dept - 1].Value), out deptcol);
                            value = true;
                        }
                    }
                    if (boun != 0)
                    {
                        if (FpSpread1.Sheets[0].Cells[fp, boun - 1].Locked == false)
                        {
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[fp, boun - 1].Value), out bouncol);
                            value = true;
                        }
                    }
                    if (clr != 0)
                    {
                        if (FpSpread1.Sheets[0].Cells[fp, clr - 1].Locked == false)
                        {
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[fp, clr - 1].Value), out clrcol);
                            value = true;
                        }
                    }
                    if (value == true && bouncol == 1)
                        bounAmt += tobecolvalue;

                    if (deptcol == 1 && value == true)
                        deptAmt += tobecolvalue;

                    if (clrcol == 1 && value == true)
                        clrAmt += tobecolvalue;

                }
            }
            if (hasvalue.ContainsKey("Deposited"))
            {
                string bankname = "";
                if (ddl_bankname.Items.Count > 0)
                {
                    if (ddlotherBank.Items.Count > 0 && ddlotherBank.SelectedItem.Text != "Select")
                        bankname = Convert.ToString(ddlotherBank.SelectedItem.Text);
                    else
                        bankname = Convert.ToString(ddl_bankname.SelectedItem.Text);
                }
                if (deptAmt != 0 || deptAmt != 0.00)
                {
                    tbltot.Visible = true;
                    lbldtxt.Text = "Deposit Amount:";
                    lbldtxtamt.Text = Convert.ToString(deptAmt);
                    lblbkvalue.Text = bankname;
                    lblsave.Text = "Do You Want To Deposit OK/Cancel";
                    lblsave.Visible = true;
                    divsave.Visible = true;
                    Div4.Visible = true;
                }
                else
                {
                    lblalertmsg.Text = "Please Select Any One Record!";
                    Div2.Visible = true;
                }
            }
            if (hasvalue.ContainsKey("Bounce"))
            {
                if (bounAmt != 0 || bounAmt != 0.00)
                {
                    rbcancel.Visible = false;
                    rbredept.Visible = false;
                    btnsavebounce.Visible = false;
                    btnsavebn.Visible = true;
                    Label4.Text = Convert.ToString(bounAmt);
                    Label2.Text = "Do You Want To Continue OK/Cancel";
                    Label2.Visible = true;
                    divbounce.Visible = true;
                }
            }
            if (hasvalue.ContainsKey("Cleared"))
            {
                if (clrAmt != 0 && clrAmt != 0.00)
                {
                    tbltot.Visible = true;
                    lbldtxt.Text = "Clear  Amount:";
                    lbldtxtamt.Text = Convert.ToString(clrAmt);
                    lblbkname.Text = "";
                    lblsave.Text = "";
                    lblbkvalue.Text = "";
                    lblsave.Text = "Do You Want To Save OK/Cancel";
                    lblsave.Visible = true;
                    divsave.Visible = true;
                    Div4.Visible = true;
                }
            }
        }
        catch { }
    }


    protected void Savedetails()
    {
        try
        {
            bool ddtsave = false;
            string dterrmsg = "";
            string paymode = "";
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string selquery = "select LinkValue from InsSettings where LinkName like 'Current%'  and college_code='" + collegecode1 + "'";
            string acctid = d2.GetFunction(selquery);

            DateTime dtsel = new DateTime();
            string seldate = txt_selectDate.Text.ToString();
            string[] split;
            split = seldate.Split('/');
            dtsel = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            string yearstart = txt_fromdate.Text.ToString();
            string time = System.DateTime.Now.ToString();
            // string yearend = txt_todate.Text.ToString();
            string[] splityr;
            string[] splittm;
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            splityr = yearstart.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            string bankvalue = string.Empty;
            if (ddl_bankname.Items.Count > 0)
            {
                if (ddlotherBank.Items.Count > 0 && ddlotherBank.SelectedItem.Text != "Select")
                    bankvalue = Convert.ToString(ddlotherBank.SelectedItem.Value);
                else
                    bankvalue = Convert.ToString(ddl_bankname.SelectedItem.Value);
            }
            FpSpread1.SaveChanges();
            string updateqry = "";
            int flag = 0;
            int flag1 = 0;
            int depvalue = 0;
            int bounce = 0;
            int cleared = 0;
            Boolean bank = false;
            bool allsave = false;
            string bankfk = "";
            //string paymode = "";
            if (ViewState["hasvalue"] != null)
                hasvalue = (Hashtable)ViewState["hasvalue"];

            for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
            {

                string empty = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Text).Trim();
                paymode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag);
                if (empty == "")
                    continue;
                string transcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                string amt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 12].Text);
                string rcptDate = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Note);
                if (!string.IsNullOrEmpty(rcptDate))
                    rcptDate = rcptDate.Split('/')[1] + "/" + rcptDate.Split('/')[0] + "/" + rcptDate.Split('/')[2];

                // depvalue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 11].Text);


                #region code added by Idhris -- 01-07-2016
                bool CHK_or_DD_clr = false;//For clearance
                string collectClr = string.Empty;
                if (paymode.Trim() == "2")
                {
                    if (AutoClearCheck() == "1")
                    {
                        collectClr = " ,IsCollected='1',CollectedDate='" + dtsel.ToString("MM/dd/yyyy") + "' ";
                        CHK_or_DD_clr = true;
                    }
                }
                else if (paymode.Trim() == "3")
                {
                    if (isCollectedForDD() == "1")
                    {
                        collectClr = " ,IsCollected='1',CollectedDate='" + dtsel.ToString("MM/dd/yyyy") + "' ";
                        CHK_or_DD_clr = true;
                    }
                }
                #endregion

                bool boolBank = false;
                if (hasvalue.Count > 0)
                {
                    if (hasvalue.ContainsKey("Deposited"))
                    {
                        DateTime dtcol = new DateTime();
                        int colvalue = Convert.ToInt32(hasvalue["Deposited"]);
                        depvalue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(colvalue) - 1].Value);
                        string date = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 11].Text);
                        string[] spldate = date.Split('/');
                        dtcol = Convert.ToDateTime(spldate[1] + "/" + spldate[0] + "/" + spldate[2]);

                        //dtcol = Convert.ToDateTime(date.ToString("MM/dd/yyyy"));
                        if (FpSpread1.Sheets[0].Cells[i, colvalue - 1].Locked == false)
                        {
                            if (depvalue == 1)
                            {
                                if (dt >= dtcol)
                                {
                                    updateqry = "update FT_FinDailyTransaction set IsDeposited ='1' ,DepositedDate ='" + dtsel.ToString("MM/dd/yyyy") + "'" + collectClr + ",Deposite_BankFK='" + bankvalue + "' where TransCode ='" + transcode + "' and PayMode in('" + paymode + "') and transdate='" + rcptDate + "'";
                                    int updqry = d2.update_method_wo_parameter(updateqry, "Text");
                                    bank = true;

                                    string updEx = "   if exists(select * from ft_excessdet where dailytranscode='" + transcode + "' and ex_paymode='" + paymode + "')update ft_excessdet set Ex_Deposite_BankFk='" + bankvalue + "'  where dailytranscode='" + transcode + "' and ex_paymode='" + paymode + "'";
                                    updEx += "  if exists(select * from ft_excessreceiptdet where receiptno='" + transcode + "' and Ex_Rpt_paymode='" + paymode + "')update ft_excessreceiptdet set ExRcpt_Deposite_BankFk ='" + bankvalue + "'  where receiptno='" + transcode + "' and Ex_Rpt_paymode='" + paymode + "'";
                                    int updqrys = d2.update_method_wo_parameter(updEx, "Text");
                                }
                                else
                                {
                                    ddtsave = true;
                                    dterrmsg = "Deposit Date Must be Equal or Less than Select Date";
                                }
                            }
                        }
                        else
                        {
                            allsave = true;
                        }

                    }
                    if (hasvalue.ContainsKey("Bounce"))
                    {
                        #region old save
                        //int colvalue = Convert.ToInt32(hasvalue["Bounce"]);
                        //bounce = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, colvalue - 1].Value);
                        //if (FpSpread1.Sheets[0].Cells[i, colvalue - 1].Locked == false)
                        //{
                        //    if (bounce == 1)
                        //    {

                        //        updateqry = "update FT_FinDailyTransaction set IsBounced ='1' ,BouncedDate ='" + dtsel.ToString("MM/dd/yyyy") + "' where TransCode ='" + transcode + "' and PayMode in('" + paymode + "')";
                        //        int updqry = d2.update_method_wo_parameter(updateqry, "Text");
                        //        bank = true;
                        //        string headerfk = "";
                        //        string ledgerfk = "";
                        //        string feecat = "";
                        //        string debitamt = "";
                        //        string appno = "";
                        //        string selfee = " select HeaderFK,LedgerFK,FeeCategory,Debit,App_No  from FT_FinDailyTransaction where TransCode ='" + transcode + "' and PayMode in('" + paymode + "') ";
                        //        ds.Clear();
                        //        ds = da.select_method_wo_parameter(selfee, "Text");
                        //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        //            {
                        //                headerfk = Convert.ToString(ds.Tables[0].Rows[k]["HeaderFK"]);
                        //                ledgerfk = Convert.ToString(ds.Tables[0].Rows[k]["LedgerFK"]);
                        //                feecat = Convert.ToString(ds.Tables[0].Rows[k]["FeeCategory"]);
                        //                debitamt = Convert.ToString(ds.Tables[0].Rows[k]["Debit"]);
                        //                appno = Convert.ToString(ds.Tables[0].Rows[k]["App_No"]);

                        //                string upfee = " update FT_FeeAllot set PaidAmount =PaidAmount -'" + debitamt + "',BalAmount =BalAmount +'" + debitamt + "' where App_No ='" + appno + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "' and FeeCategory ='" + feecat + "'";
                        //                int updatefee = d2.update_method_wo_parameter(upfee, "Text");

                        //            }
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    allsave = true;
                        //}
                        #endregion
                        ReceiptCancel();
                    }
                    if (hasvalue.ContainsKey("Cleared"))
                    {
                        int colvalue = Convert.ToInt32(hasvalue["Cleared"]);
                        cleared = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, colvalue - 1].Value);
                        if (FpSpread1.Sheets[0].Cells[i, colvalue - 1].Locked == false)
                        {
                            if (cleared == 1)
                            {
                                updateqry = "update FT_FinDailyTransaction set IsCollected ='1' ,CollectedDate ='" + dtsel.ToString("MM/dd/yyyy") + "' where TransCode ='" + transcode + "' and PayMode in('" + paymode + "') and transdate='" + rcptDate + "'";
                                int updqry = d2.update_method_wo_parameter(updateqry, "Text");
                                FpSpread1.Sheets[0].Cells[i, 1].Note = Convert.ToString(-6);
                                bank = true;
                            }

                        }
                        else
                        {
                            allsave = true;
                        }
                    }
                    if (bank == true)
                    {
                        if (CHK_or_DD_clr)
                        {
                            cleared = 1;
                        }

                        string insqry = "if exists ( select * from FT_FinBankTransaction where DailyTransID ='" + transcode + "' and FinYearFK ='" + acctid + "' and PayMode in('" + paymode + "') and EntryUserCode='" + usercode + "') update FT_FinBankTransaction set TransDate='" + dt.ToString("MM/dd/yyyy") + "',TransTime='" + dtaccesstime + "',IsDeposited='1',IsCleared='" + cleared + "',IsBounced='" + bounce + "' where DailyTransID ='" + transcode + "' and PayMode in('" + paymode + "') and FinYearFK ='" + acctid + "' and EntryUserCode='" + usercode + "' else insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK,EntryUserCode) values ('" + dt.ToString("MM/dd/yyyy") + "','" + dtaccesstime + "','" + bankvalue + "','" + paymode + "','" + transcode + "','" + depvalue + "','" + cleared + "','" + bounce + "','0','" + amt + "','" + acctid + "','" + usercode + "')";
                        int bankupd = d2.update_method_wo_parameter(insqry, "Text");
                        string Currenttime = Convert.ToString(DateTime.Now.ToLongTimeString());

                        string insert = "if exists (select * from FT_FinCashContraDet where BankFK='" + bankvalue + "' and TransDate ='" + dtsel.ToString("MM/dd/yyyy") + "' and FinYearFK='" + acctid + "' ) update FT_FinCashContraDet set TransTime='" + Currenttime + "',Credit='0',Debit=Debit+'" + amt + "',IsHand='0',IsPetty='0',IsBank='1'  where BankFK='" + bankvalue + "' and TransDate ='" + dtsel.ToString("MM/dd/yyyy") + "' and FinYearFK='" + acctid + "'  else insert into FT_FinCashContraDet (TransDate,TransTime,Credit,Debit,IsHand,IsPetty,IsBank,FinYearFK,BankFK)values ('" + dtsel.ToString("MM/dd/yyyy") + "','" + Currenttime + "','0','" + amt + "','0','0','1','" + acctid + "','" + bankvalue + "')";
                        int insertvalue1 = d2.update_method_wo_parameter(insert, "Text");
                        //updateqry = "update FT_FinDailyTransaction set Deposite_BankFK='" + bankvalue + "' where TransCode ='" + transcode + "' and PayMode in('" + paymode + "')";
                        //int updqry = d2.update_method_wo_parameter(updateqry, "Text");
                        bank = true;
                    }

                }

            }
            if (bank == true)
            {
                if (ddtsave == false)
                {
                    lblalertmsg.Text = "Saved Successfully";
                    lblalertmsg.Visible = true;
                    Div2.Visible = true;
                    divsave.Visible = false;
                    Div4.Visible = false;
                    // btn_go_Click(sender, e);
                }
                else
                {
                    lblalertmsg.Text = dterrmsg;
                    lblalertmsg.Visible = true;
                    Div2.Visible = true;
                    divsave.Visible = false;
                    Div4.Visible = false;
                }
            }
            else if (allsave == true)
            {
                lblalertmsg.Text = "Already Saved";
                lblalertmsg.Visible = true;
                Div2.Visible = true;
                divsave.Visible = false;
                Div4.Visible = false;
                //btn_go_Click(sender, e);
            }
        }
        catch
        {
        }
    }
    protected void chkselall_OnCheckedChanged(object sender, EventArgs e)
    {
    }


    #region auto search
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            if (personmode == 0)
            {
                //student query
                if (MemType == 0)
                {
                    #region student

                    if (chosedmode == 0)
                        query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code='" + stcollegecode + "' order by Roll_No asc ";

                    else if (chosedmode == 1)
                        query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code='" + stcollegecode + "' order by Reg_No asc ";

                    else if (chosedmode == 2)
                        query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code='" + stcollegecode + "' order by Roll_admit asc ";

                    else if (chosedmode == 3)
                        query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code='" + stcollegecode + "' order by app_formno asc ";

                    else if (chosedmode == 4)
                        query = "select distinct top 100 DDNo from FT_FinDailyTransaction where memtype='1' and  PayMode='3' and DDNo like '" + prefixText + "%' order by DDNo asc";
                    else if (chosedmode == 5)
                        query = "select distinct top 100 DDNo from FT_FinDailyTransaction where memtype='1' and PayMode='2' and DDNo like '" + prefixText + "%' order by DDNo asc";

                    #endregion
                }
                else if (MemType == 1)
                {
                    #region staff

                    if (chosedmode == 0)
                        query = "select distinct top (50) s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code like '" + prefixText + "%'";

                    else if (chosedmode == 1)
                        query = "select top 100 DDNo from FT_FinDailyTransaction where memtype='2' and PayMode='3' and DDNo like '" + prefixText + "%'";
                    else if (chosedmode == 2)
                        query = "select top 100 DDNo from FT_FinDailyTransaction where memtype='2' and PayMode='2' and DDNo like '" + prefixText + "%'";

                    #endregion
                }
                else if (MemType == 2)
                {
                    #region vendor

                    if (chosedmode == 0)
                        query = "select top (100) VendorCode   from CO_VendorMaster where VendorType =1 and VendorCompName like '" + prefixText + "%' ";

                    else if (chosedmode == 1)
                        query = "select top 100 DDNo from FT_FinDailyTransaction where memtype='3' and PayMode='3' and DDNo like '" + prefixText + "%'";
                    else if (chosedmode == 2)
                        query = "select top 100 DDNo from FT_FinDailyTransaction where memtype='3' and PayMode='2' and DDNo like '" + prefixText + "%'";

                    #endregion
                }
                else if (MemType == 3)
                {
                    #region other
                    if (chosedmode == 0)
                        query = "select top 100 VendorCode from CO_VendorMaster where VendorType ='-5' and VendorCompName like '" + prefixText + "%' ";

                    if (chosedmode == 1)
                        query = "select top 100 DDNo from FT_FinDailyTransaction where memtype='4' and PayMode='3' and DDNo like '" + prefixText + "%'";
                    else if (chosedmode == 2)
                        query = "select top 100 DDNo from FT_FinDailyTransaction where memtype='4' and PayMode='2' and DDNo like '" + prefixText + "%'";

                    #endregion
                }
            }


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    public void loadsetting()
    {
        try
        {
            rbl_rollno.Items.Clear();
            ListItem list1;
            ListItem list2;
            ListItem list3;
            ListItem list4;
            ListItem list5;
            ListItem list6;
            if (rbstud.Checked == true)
            {
                #region student

                list1 = new ListItem("Roll No", "0");
                list2 = new ListItem("Reg No", "1");
                list3 = new ListItem("Admission No", "2");
                list4 = new ListItem("App No", "3");
                list5 = new ListItem("DD No", "4");
                list6 = new ListItem("Cheque No", "5");

                rbl_rollno.Items.Clear();
                string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";

                int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

                if (save1 == 1)
                {
                    rbl_rollno.Items.Add(list1);
                }


                insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                if (save1 == 1)
                {
                    rbl_rollno.Items.Add(list2);
                }

                insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                if (save1 == 1)
                {
                    rbl_rollno.Items.Add(list3);
                }

                insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
                save1 = Convert.ToInt32(d2.GetFunction(insqry1));

                if (save1 == 1)
                {
                    rbl_rollno.Items.Add(list4);
                }
                if (rbl_rollno.Items.Count == 0)
                {
                    rbl_rollno.Items.Add(list1);
                }
                if (rbl_rollno.Items.Count > 0)
                {
                    rbl_rollno.Items.Add(list5);
                    rbl_rollno.Items.Add(list6);
                }
                MemType = 0;
                switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
                {
                    case 0:
                        txt_rollno.Attributes.Add("placeholder", "Roll No");
                        chosedmode = 0;
                        break;
                    case 1:
                        txt_rollno.Attributes.Add("placeholder", "Reg No");
                        chosedmode = 1;
                        break;
                    case 2:
                        txt_rollno.Attributes.Add("placeholder", "Admin No");
                        chosedmode = 2;
                        break;
                    case 3:
                        txt_rollno.Attributes.Add("placeholder", "App No");
                        chosedmode = 3;
                        break;
                    case 4:
                        txt_rollno.Attributes.Add("placeholder", "DD No");
                        chosedmode = 4;
                        break;
                    case 5:
                        txt_rollno.Attributes.Add("placeholder", "Cheque No");
                        chosedmode = 5;
                        break;
                }
                #endregion
            }
            else if (rbstaff.Checked == true)
            {
                #region staff

                list1 = new ListItem("Staff Code", "0");
                list5 = new ListItem("DD No", "1");
                list6 = new ListItem("Cheque No", "2");
                rbl_rollno.Items.Add(list1);
                rbl_rollno.Items.Add(list5);
                rbl_rollno.Items.Add(list6);
                MemType = 1;
                switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
                {
                    case 0:
                        txt_rollno.Attributes.Add("placeholder", "Staff Code");
                        chosedmode = 0;
                        break;
                    case 1:
                        txt_rollno.Attributes.Add("placeholder", "DD No");
                        chosedmode = 1;
                        break;
                    case 2:
                        txt_rollno.Attributes.Add("placeholder", "Cheque No");
                        chosedmode = 2;
                        break;
                }
                #endregion
            }
            else if (rbvendor.Checked == true)
            {
                #region vendor

                list1 = new ListItem("Vendor Code", "0");
                list5 = new ListItem("DD No", "1");
                list6 = new ListItem("Cheque No", "2");
                rbl_rollno.Items.Add(list1);
                rbl_rollno.Items.Add(list5);
                rbl_rollno.Items.Add(list6);
                MemType = 2;
                switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
                {
                    case 0:
                        txt_rollno.Attributes.Add("placeholder", "Vendor Code");
                        chosedmode = 0;
                        break;
                    case 1:
                        txt_rollno.Attributes.Add("placeholder", "DD No");
                        chosedmode = 1;
                        break;
                    case 2:
                        txt_rollno.Attributes.Add("placeholder", "Cheque No");
                        chosedmode = 2;
                        break;
                }
                #endregion
            }
            else
            {
                #region others

                list1 = new ListItem("Others Code", "0");
                list5 = new ListItem("DD No", "1");
                list6 = new ListItem("Cheque No", "2");
                rbl_rollno.Items.Add(list1);
                rbl_rollno.Items.Add(list5);
                rbl_rollno.Items.Add(list6);
                MemType = 3;
                switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
                {
                    case 0:
                        txt_rollno.Attributes.Add("placeholder", "Others Code");
                        chosedmode = 0;
                        break;
                    case 1:
                        txt_rollno.Attributes.Add("placeholder", "DD No");
                        chosedmode = 1;
                        break;
                    case 2:
                        txt_rollno.Attributes.Add("placeholder", "Cheque No");
                        chosedmode = 2;
                        break;
                }
                #endregion
            }
        }
        catch { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_rollno.Text = "";
            if (rbstud.Checked == true)
            {
                #region stud
                MemType = 0;
                switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
                {
                    case 0:
                        txt_rollno.Attributes.Add("Placeholder", "Roll No");
                        chosedmode = 0;
                        break;
                    case 1:
                        txt_rollno.Attributes.Add("Placeholder", "Reg No");
                        chosedmode = 1;
                        break;
                    case 2:
                        txt_rollno.Attributes.Add("Placeholder", "Admin No");
                        chosedmode = 2;
                        break;
                    case 3:
                        txt_rollno.Attributes.Add("Placeholder", "App No");
                        chosedmode = 3;
                        break;
                    case 4:
                        txt_rollno.Attributes.Add("Placeholder", "DD No");
                        chosedmode = 4;
                        break;
                    case 5:
                        txt_rollno.Attributes.Add("Placeholder", "Cheque No");
                        chosedmode = 5;
                        break;
                }
                #endregion
            }
            else if (rbstaff.Checked == true)
            {
                #region staff
                MemType = 1;
                switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
                {
                    case 0:
                        txt_rollno.Attributes.Add("Placeholder", "Staff Code");
                        chosedmode = 0;
                        break;
                    case 1:
                        txt_rollno.Attributes.Add("Placeholder", "DD No");
                        chosedmode = 1;
                        break;
                    case 2:
                        txt_rollno.Attributes.Add("Placeholder", "Cheque No");
                        chosedmode = 2;
                        break;
                }

                #endregion
            }
            else if (rbvendor.Checked == true)
            {
                #region vendor
                MemType = 2;
                switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
                {
                    case 0:
                        txt_rollno.Attributes.Add("Placeholder", "Vendor Code");
                        chosedmode = 0;
                        break;
                    case 1:
                        txt_rollno.Attributes.Add("Placeholder", "DD No");
                        chosedmode = 1;
                        break;
                    case 2:
                        txt_rollno.Attributes.Add("Placeholder", "Cheque No");
                        chosedmode = 2;
                        break;
                }

                #endregion
            }
            else
            {
                #region vendor
                MemType = 3;
                switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
                {
                    case 0:
                        txt_rollno.Attributes.Add("Placeholder", "Others Code");
                        chosedmode = 0;
                        break;
                    case 1:
                        txt_rollno.Attributes.Add("Placeholder", "DD No");
                        chosedmode = 1;
                        break;
                    case 2:
                        txt_rollno.Attributes.Add("Placeholder", "Cheque No");
                        chosedmode = 2;
                        break;
                }

                #endregion
            }
        }
        catch { }
    }
    #endregion

    //Code Added by Idhris - 01-07-2016
    protected string isCollectedForDD()
    {
        string value = "0";
        string ddCollected = "select LinkValue from New_InsSettings where linkname = 'AutomaticallyClearDD' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
        value = d2.GetFunction(ddCollected).Trim();
        return value;
    }
    protected string AutoClearCheck()
    {
        string value = "0";
        string chqCleared = "select LinkValue from New_InsSettings where linkname = 'AutomaticallyClearCheque' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
        value = d2.GetFunction(chqCleared).Trim();
        return value;
    }
    //Last modified by Idhris -01-07-2016



    //added by sudhagar 02-07-2016
    protected void btnsavealert_Click(object sender, EventArgs e)
    {
        Savedetails();
        btn_go_Click(sender, e);
    }

    protected void btncan_Click(object sender, EventArgs e)
    {
        try
        {
            int boun = 0;
            int dept = 0;
            if (ViewState["hasvalue"] != null)
                hasvalue = (Hashtable)ViewState["hasvalue"];

            if (hasvalue.ContainsKey("Deposited"))
                dept = Convert.ToInt32(hasvalue["Deposited"]);

            if (hasvalue.ContainsKey("Cleared"))
                boun = Convert.ToInt32(hasvalue["Cleared"]);
            if (hasvalue.Count > 0)
            {

                if (rbclear.Checked == true)
                {
                    for (int fp = 0; fp < FpSpread1.Sheets[0].Rows.Count; fp++)
                    {
                        FpSpread1.Sheets[0].Cells[fp, boun - 1].Value = 0;
                    }
                }
                if (rbtodeposit.Checked == true)
                {
                    for (int fp = 0; fp < FpSpread1.Sheets[0].Rows.Count; fp++)
                    {
                        FpSpread1.Sheets[0].Cells[fp, dept - 1].Value = 0;
                    }
                }
            }
            divsave.Visible = false;
        }
        catch { }
    }

    //last added by sudhagar 04-07
    protected void rbtodeposit_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        FpSpread1.Visible = false;
        //  div1.Visible = false;
        print.Visible = false;
        divlbl.Visible = false;
        divbtn.Visible = false;
        divlbl.Visible = false;
        //  rbreport.Visible = false;
        rbentry.Checked = true;
        rbentry.Text = "Entry";

        //dt
        tdseldt.Visible = true;
        tdseltxtdt.Visible = true;

        //bk
        tdbk.Visible = false;
        tdcblbk.Visible = false;
        tdtype.Visible = false;
    }
    protected void rbdeposit_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        FpSpread1.Visible = false;
        //  div1.Visible = false;
        print.Visible = false;
        divlbl.Visible = false;
        divbtn.Visible = false;
        divlbl.Visible = false;
        // rbreport.Visible = false;
        rbentry.Checked = true;
        rbentry.Text = "Report";

        //dt
        tdseldt.Visible = false;
        tdseltxtdt.Visible = false;

        //bk
        tdbk.Visible = true;
        tdcblbk.Visible = true;
        loadType();
        tdtype.Visible = true;
    }
    protected void rbbounce_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        FpSpread1.Visible = false;
        //  div1.Visible = false;
        print.Visible = false;
        divlbl.Visible = false;
        divbtn.Visible = false;
        divlbl.Visible = false;
        //  rbreport.Visible = false;
        rbentry.Text = "Entry";

        //dt
        tdseldt.Visible = false;
        tdseltxtdt.Visible = false;

        //bk
        tdbk.Visible = true;
        tdcblbk.Visible = true;
        tdtype.Visible = false;
    }
    protected void rbclear_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        FpSpread1.Visible = false;
        // div1.Visible = false;
        print.Visible = false;
        divlbl.Visible = false;
        divbtn.Visible = false;
        divlbl.Visible = false;
        // rbreport.Visible = false;
        rbentry.Text = "Entry";

        //dt
        tdseldt.Visible = false;
        tdseltxtdt.Visible = false;

        //bk
        tdbk.Visible = true;
        tdcblbk.Visible = true;
        tdtype.Visible = false;
    }
    protected void rbentry_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        FpSpread1.Visible = false;
        // div1.Visible = false;
        print.Visible = false;
        divlbl.Visible = false;
        divbtn.Visible = false;
        divlbl.Visible = false;

        //rbtodeposit.Checked = true;
        //rbdeposit.Checked = false;
        //rbbounce.Checked = false;
        //rbclear.Checked = false;
    }
    protected void rbreport_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        FpSpread1.Visible = false;
        // div1.Visible = false;
        print.Visible = false;
        divlbl.Visible = false;
        divbtn.Visible = false;
        divlbl.Visible = false;

        //dt
        tdseldt.Visible = false;
        tdseltxtdt.Visible = false;

        //rbtodeposit.Checked = true;
        //rbdeposit.Checked = false;
        //rbbounce.Checked = false;
        //rbclear.Checked = false;
    }
    protected void btnsavebounce_Click(object sender, EventArgs e)
    {
        try
        {
            bool Ok = false;
            string appno = "";
            string activerow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (activerow != "" && activecol != "")
            {
                int actrow = Convert.ToInt32(activerow);
                int actcol = Convert.ToInt32(activecol);
                // string clearstate = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 1].Note);
                string paymode = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 0].Tag);
                string transcode = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 1].Tag);
                string rollno = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 1].Text);

                if (sclSett() == "0")
                {
                    if (rollno != "")
                        appno = d2.GetFunction("select App_No  from Registration where roll_admit='" + rollno + "'");
                }
                else
                {
                    if (rollno != "")
                        appno = d2.GetFunction("select App_No  from Registration where Roll_No='" + rollno + "'");
                }
                //if (rollno != "")
                //    appno = d2.GetFunction("select App_No  from Registration where Roll_No='" + rollno + "'");
                if (rbcancel.Checked == true)
                {
                    FpSpread1.Sheets[0].Cells[actrow, 0].Note = Convert.ToString(-1);
                    FpSpread1.Sheets[0].Cells[actrow, 1].Note = Convert.ToString(-6);
                    Ok = true;
                }
                else
                {
                    FpSpread1.Sheets[0].Cells[actrow, 0].Note = Convert.ToString(2);
                    FpSpread1.Sheets[0].Cells[actrow, 1].Note = Convert.ToString(-6);
                    Ok = true;
                }
            }
            if (Ok == true)
                divbounce.Visible = false;
            else
            {
                lbl_alert.Text = "Please Select Any One Option";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
            }


        }
        catch { }
    }
    protected void btncancelbounce_Click(object sender, EventArgs e)
    {
        int boun = 0;
        if (ViewState["hasvalue"] != null)
            hasvalue = (Hashtable)ViewState["hasvalue"];

        if (hasvalue.ContainsKey("Bounce"))
            boun = Convert.ToInt32(hasvalue["Bounce"]);

        if (rbbounce.Checked == true)
        {
            for (int fp = 0; fp < FpSpread1.Sheets[0].Rows.Count; fp++)
            {
                if (fp == 0)
                    continue;
                FpSpread1.Sheets[0].Cells[fp, boun - 1].Value = 0;
            }
        }
        divbounce.Visible = false;
        //  btncancelbounce.Text = "";
    }
    // btn_go_Click(sender, e);

    protected void ReceiptCancel()
    {
        try
        {
            double colval = 0;
            int UpQu = 0;
            bool save = false;
            string tme = DateTime.Now.ToLongTimeString();
            string dt = "";
            string seldate = Convert.ToString(txt_selectDate.Text);
            string[] sdt = seldate.Split('/');
            if (sdt.Length > 0)
                dt = sdt[1] + "/" + sdt[0] + "/" + sdt[2];
            string acctid = d2.GetFunction("select LinkValue from InsSettings where LinkName like 'Current%'  and college_code='" + collegecode1 + "'");

            string activerow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            double fnlAmt = 0;
            double BankAmt = 0;
            double Amount = 0;
            if (activecol != "")
            {
                int col = Convert.ToInt32(activecol);
                for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    if (i == 0)
                        continue;
                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, col].Value), out colval);
                    if (colval == 1)
                    {
                        string appno = "";
                        int upd = 0;
                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 12].Text), out Amount);
                        string cancelrcept = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Note);
                        string paymode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag);
                        string transcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                        string rollno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Text);
                        string clearstate = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Note);
                        if (rbstud.Checked == true)
                        {
                            if (sclSett() == "0")
                            {
                                if (rollno != "")
                                    appno = d2.GetFunction("select App_No  from Registration where roll_admit='" + rollno + "'");
                            }
                            else
                            {
                                if (rollno != "")
                                    appno = d2.GetFunction("select App_No  from Registration where Roll_No='" + rollno + "'");
                            }
                        }
                        else if (rbstaff.Checked == true)
                        {
                            if (rollno != "")
                                appno = d2.GetFunction("select sa.appl_id  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.staff_code ='" + rollno + "'");
                        }
                        else if (rbvendor.Checked == true)
                        {
                            if (rollno != "")
                                appno = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + rollno + "' and vendortype='1'");
                        }
                        else
                        {
                            if (rollno != "")
                                appno = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + rollno + "' and vendortype='-5'");
                        }

                        if (appno != "0" && transcode != "" && paymode != "" && dt != "")
                        {
                            string InsQ = "";
                            if (clearstate == "-6")
                            {
                                if (cancelrcept == "-1")
                                {
                                    bool value = CancelReceipt(transcode, appno, dt, Amount);

                                    if (value == true)
                                    {
                                        string InQ = "update FT_FinBankTransaction set IsDeposited='0',IsCleared='0',IsBounced='0' where DailyTransID ='" + transcode + "' and PayMode in('" + paymode + "') and FinYearFK ='" + acctid + "' and TransDate='" + dt + "' and EntryUserCode='" + usercode + "' ";
                                        UpQu = d2.update_method_wo_parameter(InQ, "Text");
                                    }
                                    else
                                        UpQu = -2;
                                    // }
                                }
                                else
                                {
                                    InsQ = "  update FT_FinDailyTransaction set IsDeposited='0',DepositedDate='', IsCollected='0',CollectedDate='',IsCanceled='0',CancelledDate='' where App_No='" + appno + "' and TransCode='" + transcode + "' and PayMode='" + paymode + "'";
                                    upd = d2.update_method_wo_parameter(InsQ, "Text");
                                }
                            }
                            else
                            {
                                if (cancelrcept == "-1")
                                {
                                    InsQ = "update FT_FinDailyTransaction set IsCanceled='1',CancelledDate='" + dt + "',IsBounced='1',BouncedDate='" + dt + "' where App_No='" + appno + "' and TransCode='" + transcode + "' and PayMode='" + paymode + "'";
                                    upd = d2.update_method_wo_parameter(InsQ, "Text");
                                }
                                else
                                {
                                    InsQ = "  update FT_FinDailyTransaction set IsDeposited='0',DepositedDate='', IsCollected='0',CollectedDate='',IsCanceled='0',CancelledDate='' where App_No='" + appno + "' and TransCode='" + transcode + "' and PayMode='" + paymode + "'";
                                    upd = d2.update_method_wo_parameter(InsQ, "Text");
                                }
                            }
                            if (UpQu != -2)
                            {
                                if (upd > 0 || UpQu > 0)
                                {
                                    save = true;
                                }
                            }
                            else
                                save = false;
                        }
                    }
                }
                if (save == true)
                {
                    //  btn_go_Click(sender, e);
                    divbounce.Visible = false;
                    lbl_alert.Text = "Saved Successfully";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                }
                else
                {
                    divbounce.Visible = false;
                    lbl_alert.Text = "You Have Paid Excess Amount So Can't Cancel Receipt";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                }
            }
            else
            {
                //  btn_go_Click(sender, e);
                divbounce.Visible = false;
                lbl_alert.Text = "Please Select Any One";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
            }

        }
        catch { }
    }
    protected void btnsavebn_Click(object sender, EventArgs e)
    {
        ReceiptCancel();
    }
    protected bool CancelReceipt(string chlnNo, string AppNo, string dt, double Amount)
    {
        bool success = false;
        try
        {
            bool valuechk = false;
            bool ftexcheck = false;
            bool OK = false;
            if (chlnNo != null && AppNo != "" && dt != "0")
            {
                //string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,isnull(TakenAmt,0) as TakenAmt  from FT_ChallanDet where challanNo='" + chlnNo + "'";
                string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,isnull(debit,0) as TakenAmt,DailyTransPk,transcode from FT_FinDailyTransaction where TransCode='" + chlnNo + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)<>1";
                DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                if (dsDet.Tables.Count > 0)
                {
                    if (dsDet.Tables[0].Rows.Count > 0)
                    {
                        for (int n = 0; n < dsDet.Tables[0].Rows.Count; n++)
                        {
                            string ledger = Convert.ToString(dsDet.Tables[0].Rows[n]["LedgerFK"]);
                            string header = Convert.ToString(dsDet.Tables[0].Rows[n]["HeaderFk"]);
                            string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[n]["FeeCategory"]);
                            string creditamt = Convert.ToString(dsDet.Tables[0].Rows[n]["TakenAmt"]);
                            string DailyTransPk = Convert.ToString(dsDet.Tables[0].Rows[n]["DailyTransPk"]);
                            string transcode = Convert.ToString(dsDet.Tables[0].Rows[n]["transcode"]);
                            //excess amount check
                            #region excess amount check

                            double Excercptamt = 0;
                            double.TryParse(Convert.ToString(d2.GetFunction("select amount from ft_excessreceiptdet where receiptno='" + transcode + "'")), out Excercptamt);
                            if (Excercptamt != 0)
                            {
                                Amount = Excercptamt;
                                string Selq = " select sum(excessamt)as excessamt,sum(adjamt)as adjamt,sum(balanceamt)as balanceamt from ft_excessdet where dailytranscode='" + transcode + "' ";
                                DataSet dsn = new DataSet();
                                dsn.Clear();
                                dsn = d2.select_method_wo_parameter(Selq, "Text");
                                if (dsn.Tables.Count > 0 && dsn.Tables[0].Rows.Count > 0)
                                {
                                    double exAmt = 0;
                                    double exadjamt = 0;
                                    double exbalamt = 0;
                                    double.TryParse(Convert.ToString(Convert.ToString(dsn.Tables[0].Rows[0]["excessamt"])), out exAmt);
                                    double.TryParse(Convert.ToString(Convert.ToString(dsn.Tables[0].Rows[0]["adjamt"])), out exadjamt);
                                    double.TryParse(Convert.ToString(Convert.ToString(dsn.Tables[0].Rows[0]["balanceamt"])), out exbalamt);
                                    if (Amount <= exbalamt)
                                    {
                                        string transpk = d2.GetFunction("select excessdetPk from ft_excessdet where dailytranscode='" + transcode + "'");
                                        if (transpk != "0")
                                        {
                                            // select sum(excessamt)as excessamt,sum(adjamt)as adjamt,sum(balanceamt)as balanceamt from ft_excessreceiptdet where excessdetfk='273'
                                            string Selqval = " select sum(excessamt)as excessamt,sum(adjamt)as adjamt,sum(balanceamt)as balanceamt from ft_excessreceiptdet where excessdetfk='" + transpk + "' ";
                                            DataSet dsval = new DataSet();
                                            dsval.Clear();
                                            dsval = d2.select_method_wo_parameter(Selq, "Text");
                                            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                                            {
                                                for (int sel = 0; sel < dsval.Tables[0].Rows.Count; sel++)
                                                {
                                                    if (Amount != 0)
                                                    {
                                                        double exAmts = 0;
                                                        double exadjamts = 0;
                                                        double exbalamts = 0;
                                                        double fnlamt = 0;
                                                        double.TryParse(Convert.ToString(Convert.ToString(dsval.Tables[0].Rows[sel]["excessamt"])), out exAmts);
                                                        double.TryParse(Convert.ToString(Convert.ToString(dsval.Tables[0].Rows[sel]["adjamt"])), out exadjamts);
                                                        double.TryParse(Convert.ToString(Convert.ToString(dsval.Tables[0].Rows[sel]["balanceamt"])), out exbalamts);
                                                        if (Amount <= exbalamts)
                                                        {
                                                            fnlamt = Amount;
                                                            Amount = 0;
                                                        }
                                                        else
                                                        {
                                                            fnlamt = exbalamts;
                                                            double balval = Amount - exbalamts;
                                                            Amount = balval;
                                                        }
                                                        if (fnlamt != 0)
                                                        {
                                                            string updQry = " update ft_excessledgerdet set excessamt=isnull(excessamt,0)-'" + fnlamt + "',balanceamt=isnull(balanceamt,0)-'" + fnlamt + "' where excessdetfk='" + transpk + "' ";
                                                            int upds = d2.update_method_wo_parameter(updQry, "Text");
                                                            if (upds > 0)
                                                                valuechk = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (valuechk == true)
                                        {
                                            Amount = Excercptamt;
                                            string updQ = "update ft_excessdet set excessamt=isnull(excessamt,0)-'" + Amount + "',balanceamt=isnull(balanceamt,0)-'" + Amount + "' where dailytranscode='" + transcode + "' ";
                                            int upds = d2.update_method_wo_parameter(updQ, "Text");
                                            if (upds > 0)
                                                ftexcheck = true;
                                        }
                                    }

                                }
                                if (ftexcheck == true)
                                {
                                    string updQs = "delete from ft_excessreceiptdet where receiptno='" + transcode + "' and excesstype='1' ";
                                    int upds = d2.update_method_wo_parameter(updQs, "Text");
                                    if (upds > 0)
                                        OK = true;
                                }
                            }
                            else
                                OK = true;

                            #endregion


                            if (OK == true)
                            {
                                string upTrans = "UPDATE FT_FinDailyTransaction SET IsCanceled =1,CancelledDate = '" + dt + "',IsBounced='1',BouncedDate='" + dt + "',IsCollected='0',CollectedDate='',CancelUserCode = '" + usercode + "' WHERE TransCode = '" + chlnNo + "' AND App_No = " + AppNo + " AND FeeCategory = " + FeeCategory + " and HeaderFk=" + header + " and LedgerFk=" + ledger + " and DailyTransPK=" + DailyTransPk + "";

                                #region Monthwise cancel

                                //string selFeeAllotPkQ = "select FeeAllotPk from ft_feeallot  where FeeCategory ='" + FeeCategory + "' and HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "' and App_No='" + AppNo + "' ";
                                //Int64 FeeAllotPk = 0;
                                //Int64.TryParse(d2.GetFunction(selFeeAllotPkQ).Trim(), out FeeAllotPk);

                                //DataSet dsMonWiseDet = new DataSet();
                                //string monWiseDetQ = "select Monthvalue,Yearvalue,Debit from FT_FinDailyTransactionDetailMonthWise where Dailytransfk=" + DailyTransPk + " and isCancel='0'";
                                //dsMonWiseDet = d2.select_method_wo_parameter(monWiseDetQ, "Text");
                                //if (dsMonWiseDet.Tables.Count > 0 && dsMonWiseDet.Tables[0].Rows.Count > 0)
                                //{
                                //    int monWisemon = 0;
                                //    int monWiseyea = 0;
                                //    double debAmt = 0;
                                //    int.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[0]["Monthvalue"]), out monWisemon);
                                //    int.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[0]["Yearvalue"]), out monWiseyea);
                                //    double.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[0]["Debit"]), out debAmt);
                                //    if (monWisemon > 0 && monWiseyea > 0)
                                //    {
                                //        string upDailyTransMonwiseQ = "update FT_FinDailyTransactionDetailMonthWise set isCancel='1' where DailyTransFK=" + DailyTransPk + "";

                                //        string upFeeAllotMonwiseQ = "update FT_FeeallotMonthly set  paidamount=ISNULL(paidamount,0)-" + debAmt + ",BalAmount=ISNULL(BalAmount,0)+" + debAmt + " where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseyea + "";

                                //        d2.update_method_wo_parameter(upDailyTransMonwiseQ, "Text");
                                //        d2.update_method_wo_parameter(upFeeAllotMonwiseQ, "Text");
                                //    }
                                //}

                                #endregion


                                string updateCHlTkn = " UPDATE FT_FeeAllot SET PaidStatus = 0,PaidAmount = PaidAmount - " + creditamt + ",BalAmount = BalAmount + " + creditamt + "  where FeeCategory ='" + FeeCategory + "' and HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "' and App_No='" + AppNo + "' ";

                                #region Scholaship Cancel
                                //DataSet dtSchlAmt = new DataSet();
                                //string strinSchlQ = "select ISNULL(Adjusamount,0) as Amt,LedgerFk,HeaderFk,Feecategory,Reasoncode from FT_FinScholarshipAdjusDet  WHERE  App_No=" + AppNo + " and collegecode=" + collegecode1 + " and Feecategory=" + FeeCategory + " and Ledgerfk=" + ledger + " and headerfk=" + header + " and transcode='" + chlnNo.Trim() + "' and Isnull(iscancelled,0)<>1 ";
                                //dtSchlAmt = d2.select_method_wo_parameter(strinSchlQ, "Text");
                                //if (dtSchlAmt.Tables.Count > 0 && dtSchlAmt.Tables[0].Rows.Count > 0)
                                //{
                                //    for (int rea = 0; rea < dtSchlAmt.Tables[0].Rows.Count; rea++)
                                //    {
                                //        string reasoncode = Convert.ToString(dtSchlAmt.Tables[0].Rows[rea]["Reasoncode"]);
                                //        double amt = 0;
                                //        double.TryParse(Convert.ToString(dtSchlAmt.Tables[0].Rows[rea]["Amt"]), out amt);
                                //        if (amt > 0)
                                //        {
                                //            if (ScholarTypeValue == 1)
                                //            {
                                //                //Ledgerwise
                                //                string updateGovt = " UPDATE FT_FinScholarship SET AdjusAmount=isnull(AdjusAmount,0.00)-" + amt + " WHERE  App_No=" + AppNo + " and ledgerfk=" + ledger + " and Headerfk=" + header + " and FeeCategory=" + FeeCategory + " and collegecode=" + collegecode1 + " and Reasoncode=" + reasoncode + "";
                                //                d2.update_method_wo_parameter(updateGovt, "Text");

                                //                string ledUpdQ = "UPDATE FT_FinScholarshipAdjusDet SET IsCancelled='1',Adjusdate='" + canceldate.Date + "'  WHERE  App_No=" + AppNo + " and collegecode=" + collegecode1 + " and Feecategory=" + FeeCategory + " and Ledgerfk=" + ledger + " and headerfk=" + header + " and transcode='" + chlnNo.Trim() + "' and Isnull(iscancelled,0)<>1 and Reasoncode=" + reasoncode + "";
                                //                d2.update_method_wo_parameter(ledUpdQ, "Text");
                                //            }
                                //            else
                                //            {
                                //                //common
                                //                string ledUpdQ = "UPDATE FT_FinScholarshipAdjusDet SET IsCancelled='1',Adjusdate='" + canceldate.Date + "'  WHERE  App_No=" + AppNo + " and collegecode=" + collegecode1 + " and Feecategory=" + FeeCategory + " and Ledgerfk=" + ledger + " and headerfk=" + header + " and transcode='" + chlnNo.Trim() + "' and Isnull(iscancelled,0)<>1 and Reasoncode=" + reasoncode + "";
                                //                d2.update_method_wo_parameter(ledUpdQ, "Text");

                                //                string selDistLedge = " select isnull(adjusamount,0) as adjamt,Feecategory,Reasoncode, Headerfk,Ledgerfk from FT_FinScholarship where App_no=" + AppNo + " and collegecode=" + collegecode1 + " and isnull(adjusamount,0)>0";
                                //                DataSet dsDistLedge = new DataSet();
                                //                dsDistLedge = d2.select_method_wo_parameter(selDistLedge, "Text");
                                //                if (dsDistLedge.Tables.Count > 0 && dsDistLedge.Tables[0].Rows.Count > 0)
                                //                {
                                //                    for (int dsled = 0; dsled < dsDistLedge.Tables[0].Rows.Count; dsled++)
                                //                    {
                                //                        double upamt = 0;
                                //                        double curAmt = 0;
                                //                        double.TryParse(Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["adjamt"]), out curAmt);
                                //                        string ledg = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Ledgerfk"]);
                                //                        string heade = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Headerfk"]);
                                //                        string feec = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Feecategory"]);
                                //                        string reasc = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Reasoncode"]);
                                //                        if (amt <= curAmt)
                                //                        {
                                //                            upamt = amt;
                                //                            amt = 0;
                                //                        }
                                //                        else
                                //                        {
                                //                            upamt = curAmt;
                                //                            amt -= curAmt;
                                //                        }
                                //                        if (upamt > 0)
                                //                        {
                                //                            string updateGovt = " UPDATE FT_FinScholarship SET AdjusAmount=isnull(AdjusAmount,0.00)-" + upamt + " WHERE  App_No=" + AppNo + " and ledgerfk=" + ledg + " and Headerfk=" + heade + " and FeeCategory=" + feec + " and collegecode=" + collegecode1 + " and Reasoncode=" + reasc + "";
                                //                            d2.update_method_wo_parameter(updateGovt, "Text");
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                                #endregion

                                #region update CashTransaction

                                //string upCashTrans = "  if exists (select * from FT_FinCashTransaction where TransDate ='" + canceldate.Date + "' and FinYearFK ='" + finYearid + "') update FT_FinCashTransaction set TransTime ='" + DateTime.Now.ToLongTimeString() + "', Debit =isnull(Debit,0) -" + creditamt + " where FinYearFK ='" + finYearid + "' and TransDate ='" + canceldate.Date + "' ";

                                #endregion

                                int up2OK = d2.update_method_wo_parameter(updateCHlTkn, "Text");

                                int up1OK = d2.update_method_wo_parameter(upTrans, "Text");
                                success = true;
                            }
                            else
                            {
                                success = false;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
        return success;
    }

    //bank cbl
    protected void loadBank()
    {
        try
        {
            cblbank.Items.Clear();
            string selquery = "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster";
            ds = d2.select_method_wo_parameter(selquery, "Text");
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
                txtbank.Text = "Bank(" + cblbank.Items.Count + ")";
                cbbank.Checked = true;
            }
        }
        catch { }
    }

    protected void cbbank_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbbank, cblbank, txtbank, "Bank", "--Select--");
        }
        catch { }
    }
    protected void cblbank_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbbank, cblbank, txtbank, "Bank", "--Select--");
        }
        catch { }
    }

    //deposite type
    protected void loadType()
    {
        try
        {
            List<string> type = new List<string>();
            type.Add("Deposited");
            type.Add("Bounced");
            type.Add("Cleared");
            cbltype.DataSource = type;
            cbltype.DataBind();
            for (int i = 0; i < cbltype.Items.Count; i++)
            {
                cbltype.Items[i].Selected = true;
            }
            txttype.Text = "Type(" + cbltype.Items.Count + ")";
            cbtype.Checked = true;
        }
        catch { }
    }

    protected void cbtype_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbtype, cbltype, txttype, "Type", "--Select--");
        }
        catch { }
    }
    protected void cbltype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbtype, cbltype, txttype, "Type", "--Select--");
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


    protected void htmlTable()
    {
        try
        {

        }
        catch { }
    }

    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;

    }

    protected void getidadress()
    {
        string ipadress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (ipadress == "" || ipadress == null)
            ipadress = Request.ServerVariables["REMOTE_ADDR"];




        string ipaddress;

        ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (ipaddress == "" || ipaddress == null)

            ipaddress = Request.ServerVariables["REMOTE_ADDR"];

    }

    private delegate void mydelegate(string text);
    protected void btndelg_Click(object sender, EventArgs e)
    {
        //mydelegate m = new mydelegate(savemethod);
        //m += savemethod;
        funct fun = new funct(addmethod);
        savme(fun, 1, 2, 3);
    }
    public delegate double funct(double x, double y);
    public double savme(funct f, double x, double y, double z)
    {
        return f(x, y) + z;
    }
    protected void savemethod(string text)
    {
    }
    protected double addmethod(double x, double y)
    {
        return x + y;
    }


    //public delegate void newemployeeeventhandler(object sender, NewEmployeeEventArgs e);
    //class hr
    //{
    //    public event newemployeeeventhandler NewEmployee;
    //    protected virtual void onnewemployee(NewEmployeeEventArgs e)
    //    {
    //    }
    //}

    //added by sudhagar memtype
    protected void rbstud_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        pcolumnorder.Visible = false;
        FpSpread1.Visible = false;
        //  div1.Visible = false;
        print.Visible = false;
        divbtn.Visible = false;
        loadColumnOrder();
        MemType = 0;
        loadsetting();
    }
    protected void rbstaff_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        pcolumnorder.Visible = false;
        FpSpread1.Visible = false;
        //  div1.Visible = false;
        print.Visible = false;
        divbtn.Visible = false;
        loadColumnOrder();
        MemType = 1;
        loadsetting();
    }
    protected void rbvendor_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        pcolumnorder.Visible = false;
        FpSpread1.Visible = false;
        //  div1.Visible = false;
        print.Visible = false;
        divbtn.Visible = false;
        MemType = 2;
        loadColumnOrder();
        loadsetting();
    }
    protected void rnother_OnCheckedChanged(object sender, EventArgs e)
    {
        fldtot.Visible = false;
        pheaderfilter.Visible = false;
        pcolumnorder.Visible = false;
        FpSpread1.Visible = false;
        //  div1.Visible = false;
        print.Visible = false;
        divbtn.Visible = false;
        MemType = 3;
        loadColumnOrder();
        loadsetting();
    }

    protected void loadColumnOrder()
    {
        cblcolumnorder.Items.Clear();
        if (rbstud.Checked == true)
        {
            string text = "";
            string textval = "";
            if (ViewState["rolltext"] != null)
            {
                text = Convert.ToString(ViewState["rolltext"]);
                if (text.Trim() == "Roll No")
                    textval = "no";
                else
                    textval = "no";
            }

            cblcolumnorder.Items.Add(new ListItem(text, textval));
            cblcolumnorder.Items.Add(new ListItem("Name", "stud_name"));
        }
        else if (rbstaff.Checked == true)
        {
            cblcolumnorder.Items.Add(new ListItem("Staff Code", "staff_code"));
            cblcolumnorder.Items.Add(new ListItem("Staff Name", "staff_name"));
        }
        else if (rbvendor.Checked == true)
        {
            cblcolumnorder.Items.Add(new ListItem("Vendor Code", "VendorCode"));
            cblcolumnorder.Items.Add(new ListItem("Vendor Contact Name", "VenContactName"));
        }
        else
        {
            cblcolumnorder.Items.Add(new ListItem("Vendor Code", "VendorCode"));
            cblcolumnorder.Items.Add(new ListItem("Vendor Company Name", "VendorCompName"));
        }
        //VendorCompName
        cblcolumnorder.Items.Add(new ListItem("Bank Name", "bankname"));
        cblcolumnorder.Items.Add(new ListItem("Mode", "paymode"));
        cblcolumnorder.Items.Add(new ListItem("Receipt No", "transcode"));
        cblcolumnorder.Items.Add(new ListItem("Receipt Date", "transdate"));
        cblcolumnorder.Items.Add(new ListItem("DD/Cheque No", "ddno"));
        cblcolumnorder.Items.Add(new ListItem("DD/Cheque Date", "dddate"));
        cblcolumnorder.Items.Add(new ListItem("Deposited Date", "depositedDate"));
        cblcolumnorder.Items.Add(new ListItem("Bounced Date", "BouncedDate"));
        cblcolumnorder.Items.Add(new ListItem("Cleared Date", "CollectedDate"));
        cblcolumnorder.Items.Add(new ListItem("Amount", "Amount"));

    }

    //  and f.Transcode=fb.DailyTransId

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
        lbl.Add(lbl_collegename);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    // last modified 04-10-2016 sudhagar

    protected void sett()
    {
        try
        {
            string sclType = d2.GetFunction("select value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'");
            string text = "";
            string textval = "";
            string txtgp = "";
            if (sclType == "0")
            {
                text = "Admission No";
                textval = " ,Roll_admit as no";
                txtgp = ",Roll_admit ";
            }
            else
            {
                text = "Roll No";
                textval = " ,roll_no as no";
                txtgp = ",roll_no ";
            }
            ViewState["rolltext"] = text;
        }
        catch { }
    }

    protected string sclSett()
    {
        string sclType = string.Empty;
        try
        {
            sclType = d2.GetFunction("select value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'");

        }
        catch { }
        return sclType;
    }

    protected double clearDDtoBounce()
    {
        double chkVal = 0;
        try
        {
            double.TryParse(Convert.ToString(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Clear DD to Bounce' and user_code ='" + usercode + "' and college_code ='" + ddl_collegename.SelectedItem.Value + "' ")), out chkVal);
        }
        catch { }
        return chkVal;

    }

}