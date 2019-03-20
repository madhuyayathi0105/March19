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
public partial class ReceiptAmountAdjust : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
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
            collegecode = Session["collegecode"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                // ledgerload();
                setLabelText();
                loadcollege();
                if (ddlcollege.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                rbrcptmode_Selected(sender, e);
                txt_date1.Attributes.Add("readonly", "readonly");
                txt_date1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ddlcollege.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        { }
    }
    public void ledgerload(ArrayList arap)
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string appNo = string.Empty;
            string feeCat = string.Empty;
            for (int i = 0; i < arap.Count; i++)
            {
                string[] splval = arap[i].ToString().Split(',');
                if (splval.Length > 0)
                {
                    if (appNo == "")
                        appNo = Convert.ToString(splval[0]);
                    if (feeCat == "")
                        feeCat = Convert.ToString(splval[1]);
                    else
                        feeCat += "'" + "," + "'" + Convert.ToString(splval[1]);
                }
            }
            if (!string.IsNullOrEmpty(appNo) && !string.IsNullOrEmpty(feeCat))
            {
                cblledger.Items.Clear();
                cbledger.Checked = false;
                txtledger.Text = "--Select--";
                string query1 = " select l.LedgerPK,l.ledgername from ft_feeallot f ,fm_ledgermaster l where f.ledgerfk=l.ledgerpk and l.headerfk=f.headerfk and f.app_no='" + appNo + "' and f.feecategory in('" + feeCat + "') and l.collegecode='" + collegecode + "' order by len(isnull(l.priority,1000)) , l.priority asc ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblledger.DataSource = ds;
                    cblledger.DataTextField = "LedgerName";
                    cblledger.DataValueField = "LedgerPK";
                    cblledger.DataBind();
                    for (int i = 0; i < cblledger.Items.Count; i++)
                    {
                        cblledger.Items[i].Selected = true;
                    }
                    txtledger.Text = "Ledger(" + cblledger.Items.Count + ")";
                    cbledger.Checked = true; ;

                }
                else
                {
                    for (int i = 0; i < cblledger.Items.Count; i++)
                    {
                        cblledger.Items[i].Selected = false;
                    }
                    txtledger.Text = "--Select--";
                    cbledger.Checked = false; ;
                }
            }

        }
        catch
        {
        }
    }
    public void cbledger_OnCheckedChanged(object sender, EventArgs e)
    {
        reuse.CallCheckBoxChangedEvent(cblledger, cbledger, txtledger, "Ledger");
    }
    public void cblledger_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        reuse.CallCheckBoxListChangedEvent(cblledger, cbledger, txtledger, "Ledger");

    }

    #region sem

    protected void bindsem()
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            ddlsem.Items.Clear();
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(collegecode, usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsem.DataSource = ds;
                ddlsem.DataTextField = "TextVal";
                ddlsem.DataValueField = "TextCode";
                ddlsem.DataBind();
            }
        }
        catch { }
    }


    #endregion

    protected void txtrcpt_Changed(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count > 0)
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string recptNo = Convert.ToString(txtrcpt.Text);
        if (!string.IsNullOrEmpty(recptNo))
        {
            string SelQ = string.Empty;
            if (rbladmission.SelectedIndex == 0)
            {
                SelQ = " select distinct f.headerfk,f.ledgerfk,l.ledgername,Transcode,feecategory,f.app_no,sum(debit) as debit,f.finyearfk, convert(varchar(10),Transdate,103)as Transdate,paymode from ft_findailytransaction f,fm_ledgermaster l,registration r where r.app_no=f.app_no and f.ledgerfk=l.ledgerpk and l.headerfk=f.headerfk and Transcode='" + recptNo + "' and r.college_code='" + collegecode + "' and isnull(iscanceled,'0')='0' group by f.headerfk,f.ledgerfk,Transcode,f.app_no,feecategory,l.ledgername,f.finyearfk,Transdate,paymode";
            }
            else
            {
                SelQ = " select distinct f.headerfk,f.ledgerfk,l.ledgername,Transcode,feecategory,f.app_no,sum(debit) as debit,f.finyearfk, convert(varchar(10),Transdate,103)as Transdate,paymode from ft_findailytransaction f,fm_ledgermaster l,applyn r where r.app_no=f.app_no and f.ledgerfk=l.ledgerpk and l.headerfk=f.headerfk and Transcode='" + recptNo + "' and r.college_code='" + collegecode + "' and isnull(iscanceled,'0')='0' group by f.headerfk,f.ledgerfk,Transcode,f.app_no,feecategory,l.ledgername,f.finyearfk,Transdate,paymode";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                detailofReceipt(ds);
            }
            else
                txtrcpt.Text = string.Empty;
        }
        else
        {
            gdrcpt.Visible = false;
            tblledg.Visible = false;
            fldrcpt.Visible = false;
            divledger.Visible = false;
        }
    }

    private void detailofReceipt(DataSet ds)
    {
        try
        {
            string appNo = string.Empty;
            string feeCat = string.Empty;
            ArrayList arap = new ArrayList();
            DataTable dtrcpt = new DataTable();
            dtrcpt.Columns.Add("Sno");
            dtrcpt.Columns.Add("appno");
            dtrcpt.Columns.Add("Ledgerfk");
            dtrcpt.Columns.Add("Ledger Name");
            dtrcpt.Columns.Add("feecat");
            dtrcpt.Columns.Add("PaidAmount");
            dtrcpt.Columns.Add("finyearfk");
            dtrcpt.Columns.Add("transdate");
            dtrcpt.Columns.Add("paymode");
            //dtrcpt.Columns.Add("");
            // dtrcpt.Columns.Add("");
            DataRow drrcpt;
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                drrcpt = dtrcpt.NewRow();
                drrcpt["Sno"] = Convert.ToString(row + 1);
                drrcpt["appno"] = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                drrcpt["Ledgerfk"] = Convert.ToString(ds.Tables[0].Rows[row]["ledgerfk"]);
                drrcpt["Ledger Name"] = Convert.ToString(ds.Tables[0].Rows[row]["ledgername"]);
                drrcpt["feecat"] = Convert.ToString(ds.Tables[0].Rows[row]["feecategory"]);
                drrcpt["PaidAmount"] = Convert.ToString(ds.Tables[0].Rows[row]["debit"]);
                drrcpt["finyearfk"] = Convert.ToString(ds.Tables[0].Rows[row]["finyearfk"]);
                drrcpt["transdate"] = Convert.ToString(ds.Tables[0].Rows[row]["Transdate"]);
                drrcpt["paymode"] = Convert.ToString(ds.Tables[0].Rows[row]["paymode"]);
                dtrcpt.Rows.Add(drrcpt);

                appNo = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                feeCat = Convert.ToString(ds.Tables[0].Rows[row]["feecategory"]);
                if (!arap.Contains(appNo + "," + feeCat))
                {
                    arap.Add(appNo + "," + feeCat);
                }
            }
            if (dtrcpt.Rows.Count > 0)
            {
                gdrcpt.DataSource = dtrcpt;
                gdrcpt.DataBind();
                gdrcpt.Visible = true;
                fldrcpt.Visible = true;
                tblledg.Visible = true;
                ledgerload(arap);
                bindsem();
                // fldrcpt.Attributes.Add("Style", "height:350px");
                // gdrcpt.Attributes.Add("Style", "height:250px");
            }
        }
        catch { gdrcpt.DataSource = null; }
    }

    protected void gdrcpt_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[1].Visible = false;
            //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //e.Row.Cells[1].Visible = false;
            //e.Row.Cells[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //e.Row.Cells[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //e.Row.Cells[2].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //e.Row.Cells[3].BackColor = ColorTranslator.FromHtml("#0CA6CA");
        }
    }

    protected void btnadjust_Click(object sender, EventArgs e)
    {
        divledger.Visible = false;
        getAdjustMentDetails();
    }

    protected string getPaidLedger()
    {
        string strLdFK = string.Empty;
        try
        {
            StringBuilder sbLdFK = new StringBuilder();
            foreach (GridViewRow gdrow in gdrcpt.Rows)
            {
                CheckBox cbsel = (CheckBox)gdrow.FindControl("cbsel");
                if (cbsel.Checked)
                {
                    Label ldFK = (Label)gdrow.FindControl("lblledgerfk");
                    sbLdFK.Append(ldFK.Text.Trim() + "','");
                }
            }
            if (sbLdFK.Length > 0)
            {
                sbLdFK.Remove(sbLdFK.Length - 3, 3);
                strLdFK = Convert.ToString(sbLdFK);
            }
        }
        catch { strLdFK = string.Empty; }
        return strLdFK;
    }

    private void getAdjustMentDetails()
    {
        double totFnlAmt = 0;
        DataTable dtadj = new DataTable();
        dtadj.Columns.Add("Sno");
        dtadj.Columns.Add("appno");
        dtadj.Columns.Add("headerfk");
        dtadj.Columns.Add("Ledgerfk");
        dtadj.Columns.Add("Ledger Name");
        dtadj.Columns.Add("feecat");
        dtadj.Columns.Add("TotalAmount");
        dtadj.Columns.Add("PaidAmount");
        dtadj.Columns.Add("PaidAmountold");
        dtadj.Columns.Add("ToBePaid");
        dtadj.Columns.Add("BalAmount");
        dtadj.Columns.Add("finyearfk");
        DataRow dradj;
        lbladjamt.Text = string.Empty;
        string appNo = string.Empty;
        string paidLed = getPaidLedger();
        string ledger = string.Empty;
        StringBuilder sbld = new StringBuilder();
        for (int ld = 0; ld < cblledger.Items.Count; ld++)
        {
            if (cblledger.Items[ld].Selected)
            {
                if (!paidLed.Contains(cblledger.Items[ld].Value))
                {
                    sbld.Append(cblledger.Items[ld].Value + "','");
                }
            }
        }
        if (sbld.Length > 0)
        {
            sbld.Remove(sbld.Length - 3, 3);
            ledger = Convert.ToString(sbld);
        }

        // string ledger = getCblSelectedValue(cblledger);

        string feeCat = string.Empty;
        if (ddlsem.Items.Count > 0)
            feeCat = Convert.ToString(ddlsem.SelectedItem.Value);
        double paidAdjAmt = getAdjustAmount(ref appNo);
        totFnlAmt = paidAdjAmt;
        string alert = validate(ledger, feeCat, paidAdjAmt, appNo);
        if (ddlcollege.Items.Count > 0)
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        if (alert == string.Empty)
        {
            string SelQ = "  select f.app_no,f.headerfk,l.LedgerPK,l.ledgername,sum(totalamount) as totalamount,sum(paidamount) as paidamount,sum(balamount) as balamount,f.feecategory,f.finyearfk from ft_feeallot f ,fm_ledgermaster l where f.ledgerfk=l.ledgerpk and l.headerfk=f.headerfk and f.app_no='" + appNo + "' and f.feecategory in('" + feeCat + "') and f.ledgerfk in('" + ledger + "') and l.collegecode='" + collegecode + "' group by l.LedgerPK,l.ledgername,f.app_no,f.feecategory,f.headerfk,f.finyearfk having sum(totalamount)<>sum(paidamount) and sum(balamount)<>'0' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                double totamAmout = 0;
                double paidAmount = 0;
                double balAmount = 0;
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    //if (paidAdjAmt != 0)
                    //{
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["totalamount"]), out totamAmout);
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["paidamount"]), out paidAmount);
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["balamount"]), out balAmount);
                    //if (totamAmout > paidAmount)
                    //{
                    //double tempAmt = totamAmout - paidAmount;
                    //if (paidAdjAmt > tempAmt)
                    //{
                    //    paidAdjAmt = paidAdjAmt - tempAmt;
                    //    balAmount = 0;
                    //}
                    //else if (paidAdjAmt < tempAmt)
                    //{
                    //    totamAmout = paidAdjAmt;
                    //    balAmount = tempAmt - paidAdjAmt;
                    //    paidAdjAmt = 0;

                    //}

                    //  paidAdjAmt = paidAdjAmt - tempAmt;
                    dradj = dtadj.NewRow();
                    dradj["Sno"] = Convert.ToString(row + 1);
                    dradj["appno"] = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                    dradj["headerfk"] = Convert.ToString(ds.Tables[0].Rows[row]["headerfk"]);
                    dradj["Ledgerfk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerPK"]);
                    dradj["Ledger Name"] = Convert.ToString(ds.Tables[0].Rows[row]["ledgername"]);
                    dradj["feecat"] = Convert.ToString(ds.Tables[0].Rows[row]["feecategory"]);
                    dradj["TotalAmount"] = Convert.ToString(totamAmout);
                    dradj["PaidAmount"] = Convert.ToString(paidAmount);
                    dradj["PaidAmountold"] = Convert.ToString(paidAmount);
                    dradj["ToBePaid"] = "";
                    dradj["BalAmount"] = Convert.ToString(balAmount);
                    dradj["finyearfk"] = Convert.ToString(ds.Tables[0].Rows[row]["finyearfk"]);
                    dtadj.Rows.Add(dradj);
                    // }
                    // }
                }
                if (dtadj.Rows.Count > 0)
                {
                    gdledger.DataSource = dtadj;
                    gdledger.DataBind();
                    gdledger.Visible = true;
                    btnsave.Visible = true;
                    divledger.Visible = true;
                    lbltotselamt.Text = Convert.ToString(totFnlAmt);
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Found";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = alert;
        }
    }
    private double getAdjustAmount(ref string appNo)
    {
        double totalAmount = 0;
        try
        {
            Label lblappno = new Label();
            foreach (GridViewRow gdrow in gdrcpt.Rows)
            {
                CheckBox cbsel = (CheckBox)gdrow.FindControl("cbsel");
                if (cbsel.Checked)
                {
                    lblappno = (Label)gdrow.FindControl("lblappno");
                    double Amount = 0;
                    Label lblpaidamt = (Label)gdrow.FindControl("lblpaidamt");
                    double.TryParse(Convert.ToString(lblpaidamt.Text), out Amount);
                    totalAmount += Amount;
                }
                appNo = Convert.ToString(lblappno.Text);
            }
        }
        catch { }
        return totalAmount;
    }

    private string validate(string ledger, string feeCat, double paidAdjAmt, string appNo)
    {
        string alert = string.Empty;
        if (paidAdjAmt != 0 && !string.IsNullOrEmpty(appNo))
        {
            if (!string.IsNullOrEmpty(ledger))
            {
                if (!string.IsNullOrEmpty(feeCat))
                    alert = string.Empty;
                else
                    alert = "Please Select Any One Feecategory";
            }
            else
                alert = "Please Select Any One Ledger At The Bottom";
        }
        else
        {
            if (paidAdjAmt == 0)
                alert = "Please Select Any One Adjustment Ledger";
            else
                alert = "Receipt No Not Valid";
        }

        return alert;
    }
    //    if (!string.IsNullOrEmpty(ledger) && !string.IsNullOrEmpty(feeCat) && paidAdjAmt != 0)
    //        alert = string.Empty;
    //    else
    //    {
    //        if (string.IsNullOrEmpty(ledger) && string.IsNullOrEmpty(feeCat) && paidAdjAmt == 0)
    //            alert = "Please Select Any One Adjustment Ledger on The Grid And Select New Ledger and Select Semester";
    //    }
    //}

    protected void txtpaid_Changed(object sender, EventArgs e)
    {
        try
        {
            int rowindex = rowIndxClicked();
            if (gdledger.Rows.Count > 0)
            {
                int rowcnt = 0;
                double fnlAmount = 0;
                double fnltotal = 0;
                double fnloldPaidAmt = 0;
                double.TryParse(Convert.ToString(lbltotselamt.Text), out fnltotal);
                lbladjamt.Text = string.Empty;
                foreach (GridViewRow gdrow in gdledger.Rows)
                {
                    double totalAmount = 0;
                    double paidAmount = 0;
                    double balAmount = 0;
                    double toBePaidAmt = 0;
                    // double oldPaidAmount = 0;
                    Label lbltot = (Label)gdrow.FindControl("lbtotamt");
                    TextBox lblpaidamt = (TextBox)gdrow.FindControl("txtpaid");
                    Label lblbal = (Label)gdrow.FindControl("lbbalamt");
                    TextBox txtTobePaid = (TextBox)gdrow.FindControl("txttobepaid");
                    //Label lbloldpaid = (Label)gdrow.FindControl("lbloldamt");

                    double.TryParse(Convert.ToString(lbltot.Text), out totalAmount);
                    double.TryParse(Convert.ToString(lblpaidamt.Text), out paidAmount);
                    double.TryParse(Convert.ToString(txtTobePaid.Text), out toBePaidAmt);
                    //  double.TryParse(Convert.ToString(lbloldpaid.Text), out oldPaidAmount);
                    if (rowindex == rowcnt)
                    {
                        balAmount = totalAmount - (paidAmount + toBePaidAmt);
                        lblbal.Text = Convert.ToString(balAmount);
                    }
                    // fnlAmount += paidAmount;
                    fnloldPaidAmt += toBePaidAmt;
                    rowcnt++;
                }
                lbladjamt.Text = Convert.ToString(fnloldPaidAmt);
            }
        }
        catch
        { }
    }

    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[3].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }

        return rownumber;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        saveDetails();
    }

    private void saveDetails()
    {
        try
        {
            bool save = false;
            double totselAmt = 0;
            double adjAmt = 0;
            double oldAmt = 0;
            double.TryParse(Convert.ToString(lbltotselamt.Text), out totselAmt);
            double.TryParse(Convert.ToString(lbladjamt.Text), out adjAmt);
            //  double.TryParse(Convert.ToString(lbl.t), out oldAmt);
            string ledger = getCblSelectedValue(cblledger);
            string feeCat = string.Empty;
            if (ddlsem.Items.Count > 0)
                feeCat = Convert.ToString(ddlsem.SelectedItem.Value);

            string transcode = Convert.ToString(txtrcpt.Text);
            string transdate = string.Empty;
            string paymode = string.Empty;
            if (totselAmt == adjAmt)
            {
                bool chkFlg = false;
                Label lbltrans = new Label();
                Label lblpaymode = new Label();
                foreach (GridViewRow gdrow in gdrcpt.Rows)
                {
                    CheckBox cbsel = (CheckBox)gdrow.FindControl("cbsel");
                    if (cbsel.Checked)
                    {
                        Label lblappno = (Label)gdrow.FindControl("lblappno");
                        Label lblledger = (Label)gdrow.FindControl("lblledgerfk");
                        Label lblfeecat = (Label)gdrow.FindControl("lblfeecat");
                        Label lblfinyr = (Label)gdrow.FindControl("lblfinyr");
                        lbltrans = (Label)gdrow.FindControl("lbltransdate");
                        lblpaymode = (Label)gdrow.FindControl("lblpaymode");
                        Label lblPaidamount = (Label)gdrow.FindControl("lblpaidamt");
                        if (lblappno.Text != "" && lblledger.Text != "" && lblfeecat.Text != "" && lblfinyr.Text != "")
                        {
                            string UpdQ = "  update ft_findailytransaction set iscanceled='1' where app_no='" + lblappno.Text + "' and feecategory='" + lblfeecat.Text + "' and ledgerfk='" + lblledger.Text + "' and transcode='" + transcode + "' and finyearfk='" + lblfinyr.Text + "'";
                            UpdQ += " update ft_feeallot set  Paidamount=Paidamount -'" + lblPaidamount.Text + "',balAmount=BalAmount + '" + lblPaidamount.Text + "' from ft_feeallot where app_no='" + lblappno.Text + "' and ledgerfk='" + lblledger.Text + "' and feecategory='" + lblfeecat.Text + "'";
                             int upd = d2.update_method_wo_parameter(UpdQ, "Text");
                            chkFlg = true;
                        }
                    }
                    transdate = lbltrans.Text;
                    paymode = lblpaymode.Text;
                }
                if (chkFlg)
                {
                    foreach (GridViewRow gdrow in gdledger.Rows)
                    {
                        double paidamount = 0;
                        double allotAmount = 0;
                        double tempPaidAmt = 0;
                        double toBePaidAmt = 0;
                        Label lblappno = (Label)gdrow.FindControl("lbappno");
                        Label lblheader = (Label)gdrow.FindControl("lbheaderfk");
                        Label lblledger = (Label)gdrow.FindControl("lbledgerfk");
                        Label lblfeecat = (Label)gdrow.FindControl("lbfeecat");
                        Label lblallotamt = (Label)gdrow.FindControl("lbtotamt");
                        TextBox lblpaidamt = (TextBox)gdrow.FindControl("txtpaid");
                        TextBox txttobepaid = (TextBox)gdrow.FindControl("txttobepaid");
                        // Label lbltransdate = (Label)gdrow.FindControl("lbfeecat");
                        Label lblfinyearfk = (Label)gdrow.FindControl("lbfinyearfk");
                        string[] dt = transdate.Split('/');
                        string date = dt[1] + "/" + dt[0] + "/" + dt[2];
                        double.TryParse(Convert.ToString(lblallotamt.Text), out allotAmount);
                        double.TryParse(Convert.ToString(lblpaidamt.Text), out paidamount);
                        double.TryParse(Convert.ToString(txttobepaid.Text), out toBePaidAmt);
                        if (toBePaidAmt != 0)
                        {
                            double tempAllotAmt = allotAmount - paidamount;
                            if (tempAllotAmt >= toBePaidAmt)
                            {
                                tempPaidAmt = toBePaidAmt;
                                allotAmount = 0;
                            }
                            else
                            {
                                tempPaidAmt = (allotAmount - paidamount);
                                allotAmount = toBePaidAmt - tempPaidAmt;
                            }
                            //if (allotAmount >= toBePaidAmt)
                            //{
                            //    tempPaidAmt = toBePaidAmt;
                            //    allotAmount = 0;
                            //}
                            //else
                            //{
                            //    tempPaidAmt = (allotAmount - paidamount);
                            //    allotAmount = (toBePaidAmt - allotAmount) + paidamount;
                            //}
                            string InsQ = " if exists (select * from FT_FinDailyTransaction where transcode='" + transcode + "' and App_No='" + lblappno.Text + "' and FeeCategory='" + lblfeecat.Text + "' and LedgerFK='" + lblledger.Text + "' and headerfk='" + lblheader.Text + "' and isnull(iscanceled,'0')='0' )update FT_FinDailyTransaction set debit=isnull(debit,'0')+'" + tempPaidAmt + "' where transcode='" + transcode + "' and App_No='" + lblappno.Text + "' and FeeCategory='" + lblfeecat.Text + "' and LedgerFK='" + lblledger.Text + "' and headerfk='" + lblheader.Text + "'  and isnull(iscanceled,'0')='0' else INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected,CollectedDate,IsDeposited,DepositedDate) VALUES('" + date + "','" + DateTime.Now.ToLongTimeString() + "','" + transcode + "', '" + paymode + "', " + lblappno.Text + ", " + lblledger.Text + ", " + lblheader.Text + ", " + lblfeecat.Text + ", '0', '" + tempPaidAmt + "','1', '1', '0', 0, '', '0', '0', '0', 0, " + usercode + ", " + lblfinyearfk.Text + ",'3','1','','1','')";
                            InsQ += " update ft_feeallot set  Paidamount=isnull(Paidamount,'0')+'" + tempPaidAmt + "',balAmount=isnull(BalAmount,'0') -'" + tempPaidAmt + "' where app_no='" + lblappno.Text + "' and headerfk='" + lblheader.Text + "' and ledgerfk='" + lblledger.Text + "' and feecategory='" + lblfeecat.Text + "'";
                             d2.update_method_wo_parameter(InsQ, "Text");
                            if (allotAmount != 0)
                            {
                                 bool check = movetoExcess(lblappno.Text, lblledger.Text, lblheader.Text, lblfeecat.Text, transcode, lblfinyearfk.Text, date, allotAmount);
                            }
                             save = true;
                        }
                    }
                }
                if (save == true)
                {

                    divledger.Visible = false;
                    fldrcpt.Visible = false;
                    tblledg.Visible = false;
                    txtrcpt.Text = "";
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Updated Successfully";
                    // Response.Redirect("ReceiptAmountAdjust.aspx");
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Total Select Ledger Amount Not Equal to Adjust Amount So Can't Save";
            }
        }
        catch { }
    }

    protected bool movetoExcess(string appNo, string ldFK, string hdFK, string feeCat, string transCode, string finyearfk, string date, double allotAmount)
    {
        bool boolCheck = false;
        try
        {
            string insExcess = "  if exists(select * from ft_excessdet where excesstransdate='" + date + "' and dailytranscode='" + transCode + "'  and excesstype='2' and app_no='" + appNo + "')update ft_excessdet set excessamt=isnull(excessamt,'0')+'" + allotAmount + "',adjamt=isnull(adjamt,'0')+'0',balanceamt=isnull(balanceamt,'0')+'" + allotAmount + "' where excesstransdate='" + date + "' and dailytranscode='" + transCode + "'  and excesstype='2' and app_no='" + appNo + "' else insert into ft_excessdet(excesstransdate,dailytranscode,app_no,memtype,excesstype,excessamt,adjamt,balanceamt,finyearfk,feecategory) values('" + date + "','" + transCode + "','" + appNo + "','1','2','" + allotAmount + "','0','" + allotAmount + "','" + finyearfk + "','" + feeCat + "')";
            int insex = d2.update_method_wo_parameter(insExcess, "Text");//and finyearfk='" + finyearfk + "'  and feecategory='" + feeCat + "' and feecategory='" + feeCat + "' and finyearfk='" + finyearfk + "'
            if (insex == 1)
            {
                string excesspk = d2.GetFunction(" select excessdetPk from ft_excessdet where excesstransdate='" + date + "' and dailytranscode='" + transCode + "' and feecategory='" + feeCat + "' and finyearfk='" + finyearfk + "' and excesstype='2'");
                if (excesspk != "0")
                {
                    string insledexces = "  if exists(select * from FT_ExcessLedgerDet where headerfk='" + hdFK + "' and ledgerfk='" + ldFK + "' and excessdetfk='" + excesspk + "' ) update FT_ExcessLedgerDet set excessamt=isnull(excessamt,'0')+'" + allotAmount + "',adjamt=isnull(adjamt,'0')+'0',balanceamt=isnull(balanceamt,'0')+'" + allotAmount + "'  where headerfk='" + hdFK + "' and ledgerfk='" + ldFK + "' and excessdetfk='" + excesspk + "'  else insert into FT_ExcessLedgerDet(headerfk,ledgerfk,excessamt,adjamt,balanceamt,excessdetfk,feecategory,finyearfk) values('" + hdFK + "','" + ldFK + "','" + allotAmount + "','0','" + allotAmount + "','" + excesspk + "','" + feeCat + "','" + finyearfk + "')";
                    int insexs = d2.update_method_wo_parameter(insledexces, "Text");
                    //and feecategory='" + feeCat + "' and finyearfk='" + finyearfk + "'
                    //and feecategory='" + feeCat + "' and finyearfk='" + finyearfk + "' 
                    //if (insexs > 0)
                    //{
                    //    string insExcessRcpt = " if exists(select * from ft_excessReceiptdet where app_no='" + appNo + "' and receiptno='" + transCode + "' and ledgerfk='" + ldFK + "' and excesstype='1') update ft_excessReceiptdet set amount=isnull(amount,'0')+'" + allotAmount + "'  where app_no='" + appNo + "' and receiptno='" + transCode + "' and ledgerfk='" + ldFK + "' and excesstype='1' else   insert into ft_excessReceiptdet (app_no , amount , receiptno ,rcptdate,ledgerfk,excesstype ) values ('" + appNo + "', '" + allotAmount + "', '" + transCode + "','" + date + "','" + ldFK + "','1')";
                    //    int inse = d2.update_method_wo_parameter(insExcessRcpt, "Text");
                    //}
                    boolCheck = true;
                }
            }
        }
        catch { }
        return boolCheck;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
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

    //receipt mode
    protected void rbrcptmode_Selected(object sender, EventArgs e)
    {
        if (rbrcptmode.SelectedIndex == 0)
        {
            divpaymode.Visible = true;
            divamount.Visible = false;
            txtfrcptno.Text = string.Empty;
            txtfrcptno_Changed(sender, e);
        }
        else
        {
            divpaymode.Visible = false;
            divamount.Visible = true;
            txtrcpt.Text = string.Empty;
            txtrcpt_Changed(sender, e);
        }
    }

    //receipt Paymode
    protected void txtfrcptno_Changed(object sender, EventArgs e)
    {
        string collegecode = string.Empty;
        if (ddlcollege.Items.Count > 0)
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string recptNo = Convert.ToString(txtfrcptno.Text);
        if (!string.IsNullOrEmpty(recptNo))
        {
            if (getStudentDetails(recptNo, collegecode))
            {
                getPaidDetails(recptNo, collegecode);
                ddlpaymode_OnSelected(sender, e);
                //tblpayfld
            }
        }
        else
        {
            Clear();
            tddet.Visible = false;
        }

    }
    protected bool getStudentDetails(string recptNo, string collegecode)
    {
        bool boolcheck = false;
        string SelQ = string.Empty;
        if (rbladmission.SelectedIndex == 0)
        {
            Label3.Text = "Addmission no";
            SelQ = " select distinct f.app_no,r.roll_admit,r.stud_name,r.degree_code,r.college_code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,cl.collname,r.batch_year from registration r,ft_findailytransaction f,Degree d,Department dt,Course c,collinfo cl where r.app_no=f.app_no and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.degree_code=d.degree_code and cl.college_code=r.college_code and cl.college_code = d.college_code and f.transcode='" + recptNo + "' and d.college_code='" + collegecode + "' ";
        }
        else
        {
            Label3.Text = "Application no";
            SelQ = " select distinct f.app_no,r.app_formno as roll_admit,r.stud_name,r.degree_code,r.college_code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,cl.collname,r.batch_year from applyn r,ft_findailytransaction f,Degree d,Department dt,Course c,collinfo cl where r.app_no=f.app_no and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.degree_code=d.degree_code and cl.college_code=r.college_code and cl.college_code = d.college_code and f.transcode='" + recptNo + "' and d.college_code='" + collegecode + "' ";
        }
        DataSet dsst = d2.select_method_wo_parameter(SelQ, "Text");
        if (dsst.Tables.Count > 0 && dsst.Tables[0].Rows.Count > 0)
        {
            lblroll.Text = Convert.ToString(dsst.Tables[0].Rows[0]["roll_admit"]);
            lblstudname.Text = Convert.ToString(dsst.Tables[0].Rows[0]["stud_name"]);
            lblbatch.Text = Convert.ToString(dsst.Tables[0].Rows[0]["batch_year"]);
            lbldept.Text = Convert.ToString(dsst.Tables[0].Rows[0]["degreename"]);
            lblclg.Text = Convert.ToString(dsst.Tables[0].Rows[0]["collname"]);
            collegecode = Convert.ToString(dsst.Tables[0].Rows[0]["college_code"]);
            boolcheck = true;
            tddet.Visible = true;
            //loadpaid(collegecode);
            BindPaymodeToCheckboxList(ddlpaymode, usercode, collegecode);
            bank(collegecode);
            cardType();
        }
        else
        {
            Clear();
            tddet.Visible = false;
        }
        return boolcheck;
    }

    protected void getPaidDetails(string recptNo, string collegecode)
    {

        if (!string.IsNullOrEmpty(recptNo))
        {
            string SelQ = string.Empty;
            if (rbladmission.SelectedIndex == 0)
            {
                SelQ = "  select distinct f.headerfk,f.ledgerfk,h.headername,l.ledgername,Transcode,feecategory,f.app_no,sum(debit) as debit,f.finyearfk, convert(varchar(10),Transdate,103)as Transdate, case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan'  when paymode='6' then 'Card' end paymode,paymode as paymodeval from ft_findailytransaction f,fm_ledgermaster l,fm_headermaster h,registration r where r.app_no=f.app_no and f.headerfk=h.headerpk and h.headerpk=l.headerfk and f.ledgerfk=l.ledgerpk and l.headerfk=f.headerfk and Transcode='" + recptNo + "' and r.college_code='" + collegecode + "' and isnull(iscanceled,'0')<>'1' group by f.headerfk,f.ledgerfk,Transcode,f.app_no,feecategory,h.headername,l.ledgername,f.finyearfk,Transdate,paymode";
            }
            else
            {
                SelQ = "  select distinct f.headerfk,f.ledgerfk,h.headername,l.ledgername,Transcode,feecategory,f.app_no,sum(debit) as debit,f.finyearfk, convert(varchar(10),Transdate,103)as Transdate, case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan'  when paymode='6' then 'Card' end paymode,paymode as paymodeval from ft_findailytransaction f,fm_ledgermaster l,fm_headermaster h,applyn r where r.app_no=f.app_no and f.headerfk=h.headerpk and h.headerpk=l.headerfk and f.ledgerfk=l.ledgerpk and l.headerfk=f.headerfk and Transcode='" + recptNo + "' and r.college_code='" + collegecode + "' and isnull(iscanceled,'0')<>'1' group by f.headerfk,f.ledgerfk,Transcode,f.app_no,feecategory,h.headername,l.ledgername,f.finyearfk,Transdate,paymode";
            }
            SelQ = SelQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                getReceiptDet(ds);
            else
                tblpaymode.Visible = false;
        }
        else
        {
            gdfrcpt.Visible = false;
            tblpayfld.Visible = false;
        }
    }

    private void getReceiptDet(DataSet ds)
    {
        try
        {
            string appNo = string.Empty;
            string feeCat = string.Empty;
            ArrayList arap = new ArrayList();
            Dictionary<string, string> dtpaymode = new Dictionary<string, string>();
            DataTable dtrcpt = new DataTable();
            dtrcpt.Columns.Add("Sno");
            dtrcpt.Columns.Add("appno");
            dtrcpt.Columns.Add("Transdate");
            dtrcpt.Columns.Add("Transcode");
            dtrcpt.Columns.Add("feecat");
            dtrcpt.Columns.Add("Feecategory");
            dtrcpt.Columns.Add("headerfk");
            dtrcpt.Columns.Add("Header");
            dtrcpt.Columns.Add("Ledgerfk");
            dtrcpt.Columns.Add("Ledger");
            dtrcpt.Columns.Add("Paymode");
            dtrcpt.Columns.Add("Paymodeval");
            dtrcpt.Columns.Add("PaidAmount");
            dtrcpt.Columns.Add("finyearfk");
            // dtrcpt.Columns.Add("bank");
            DataRow drrcpt;
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                drrcpt = dtrcpt.NewRow();
                drrcpt["Sno"] = Convert.ToString(row + 1);
                drrcpt["appno"] = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                drrcpt["Transdate"] = Convert.ToString(ds.Tables[0].Rows[row]["Transdate"]);
                drrcpt["Transcode"] = Convert.ToString(ds.Tables[0].Rows[row]["Transcode"]);
                string feevalue = Convert.ToString(ds.Tables[0].Rows[row]["feecategory"]);
                drrcpt["feecat"] = feevalue;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "TextCode='" + feevalue + "'";
                    DataView Dview = ds.Tables[1].DefaultView;
                    if (Dview.Count > 0)
                        drrcpt["Feecategory"] = Convert.ToString(Dview[0]["TextVal"]);
                }
                drrcpt["headerfk"] = Convert.ToString(ds.Tables[0].Rows[row]["headerfk"]);
                drrcpt["header"] = Convert.ToString(ds.Tables[0].Rows[row]["ledgername"]);
                drrcpt["Ledgerfk"] = Convert.ToString(ds.Tables[0].Rows[row]["ledgerfk"]);
                drrcpt["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["ledgername"]);
                drrcpt["paymode"] = Convert.ToString(ds.Tables[0].Rows[row]["paymode"]);
                drrcpt["paymodeval"] = Convert.ToString(ds.Tables[0].Rows[row]["paymodeval"]);
                drrcpt["PaidAmount"] = Convert.ToString(ds.Tables[0].Rows[row]["debit"]);
                drrcpt["finyearfk"] = Convert.ToString(ds.Tables[0].Rows[row]["finyearfk"]);
                // drrcpt["bank"] = Convert.ToString(ds.Tables[0].Rows[row]["finyearfk"]);
                dtrcpt.Rows.Add(drrcpt);
                if (!dtpaymode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[row]["paymodeval"])))
                {
                    dtpaymode.Add(Convert.ToString(ds.Tables[0].Rows[row]["paymodeval"]), Convert.ToString(ds.Tables[0].Rows[row]["paymode"]));
                }
            }
            if (dtrcpt.Rows.Count > 0)
            {

                gdfrcpt.DataSource = dtrcpt;
                gdfrcpt.DataBind();
                gdfrcpt.Visible = true;
                tblpayfld.Visible = true;
                loadtemppaymode(dtpaymode);
            }
        }
        catch { gdfrcpt.DataSource = null; }
    }

    protected void gdfrcpt_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[11].Visible = false;
            e.Row.Cells[13].Visible = false;
            // e.Row.Cells[14].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[11].Visible = false;
            e.Row.Cells[13].Visible = false;
            //e.Row.Cells[14].Visible = false;
        }
    }

    #region paymentmode
    protected void loadtemppaymode(Dictionary<string, string> dtpaymode)
    {
        if (dtpaymode.Count > 0)
        {
            chkl_paid.Items.Clear();
            foreach (KeyValuePair<string, string> pay in dtpaymode)
            {
                chkl_paid.Items.Add(new ListItem(pay.Value, pay.Key));
            }
            for (int i = 0; i < chkl_paid.Items.Count; i++)
            {
                chkl_paid.Items[i].Selected = true;
            }
            txt_paid.Text = "Paid(" + chkl_paid.Items.Count + ")";
            chk_paid.Checked = true;
        }
    }
    public void loadpaid(string collegecode)
    {
        try
        {
            chkl_paid.Items.Clear();
            ddlpaymode.Items.Clear();
            //d2.BindPaymodeToCheckboxList(chkl_paid, usercode, collegecode);
            if (chkl_paid.Items.Count > 0)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    chkl_paid.Items[i].Selected = true;
                    ddlpaymode.Items.Add(new ListItem(chkl_paid.Items[i].Text, chkl_paid.Items[i].Value));
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
        reuse.CallCheckBoxChangedEvent(chkl_paid, chk_paid, txt_paid, "Paid");
    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        reuse.CallCheckBoxListChangedEvent(chkl_paid, chk_paid, txt_paid, "Paid");
    }

    public void BindPaymodeToCheckboxList(DropDownList cblpaymode, string usercode, string collegecode)
    {
        try
        {
            cblpaymode.Items.Clear();
            int inclpayRights = 0;
            string payValue = string.Empty;
            Dictionary<int, string> dtpaymode = new Dictionary<int, string>();
            inclpayRights = paymodeRightsCheck(usercode, collegecode, ref  payValue);
            if (inclpayRights == 1 && payValue != "0")
            {
                string[] splvalue = payValue.Split(',');
                if (splvalue.Length > 0)
                {
                    dtpaymode = dtPaymodeValue();
                    for (int row = 0; row < splvalue.Length; row++)
                    {
                        if (dtpaymode.ContainsKey(Convert.ToInt32(splvalue[row])))
                        {
                            string modestr = dtpaymode[Convert.ToInt32(splvalue[row])];
                            cblpaymode.Items.Add(new System.Web.UI.WebControls.ListItem(modestr, Convert.ToString(splvalue[row])));
                        }
                    }
                }
                if (cblpaymode.Items.Count > 0)
                    cblpaymode.SelectedIndex = 0;
            }
            else
                cblpaymode.Items.Clear();
        }
        catch { cblpaymode.Items.Clear(); }
    }

    private int paymodeRightsCheck(string usercode, string collegecode, ref string payValue)
    {
        int paymodRghts = 0;
        Int32.TryParse(Convert.ToString(d2.GetFunction("select Linkvalue from New_InsSettings where LinkName='IncludePaymodeSettings' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'")), out paymodRghts);
        if (paymodRghts == 1)
        {
            payValue = Convert.ToString(d2.GetFunction("select Linkvalue from New_InsSettings where LinkName='IncludePaymodeSettingsValue' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' "));
        }
        return paymodRghts;
    }

    private Dictionary<int, string> dtPaymodeValue()
    {
        Dictionary<int, string> dtpaymode = new Dictionary<int, string>();
        dtpaymode.Add(1, "Cash");
        dtpaymode.Add(2, "Cheque");
        dtpaymode.Add(3, "DD");
        dtpaymode.Add(4, "Challan");
        dtpaymode.Add(5, "Online");
        dtpaymode.Add(6, "Card");
        return dtpaymode;
    }

    #endregion
    protected void ddlpaymode_OnSelected(object sender, EventArgs e)
    {
        if (ddlpaymode.Items.Count > 0)
        {
            if (ddlpaymode.SelectedItem.Text == "Cash")
            {
                div_card.Visible = false;
                div_cheque.Visible = false;
                div_sndcheque.Visible = false;
                txt_chqno.Visible = false;
                txt_ddno.Visible = false;
                txt_ddnar.Visible = false;
                btnpaysave.Visible = true;
                tblpaymode.Visible = true;
            }
            if (ddlpaymode.SelectedItem.Text == "Cheque")
            {
                div_card.Visible = false;
                div_cheque.Visible = true;
                div_sndcheque.Visible = true;
                txt_chqno.Visible = true;
                txt_ddno.Visible = false;
                txt_ddnar.Visible = false;
                btnpaysave.Visible = true;
                tblpaymode.Visible = true;
            }
            if (ddlpaymode.SelectedItem.Text == "DD")
            {
                div_card.Visible = false;
                div_cheque.Visible = true;
                div_sndcheque.Visible = true;
                txt_chqno.Visible = false;
                txt_ddno.Visible = true;
                txt_ddnar.Visible = true;
                btnpaysave.Visible = true;
                tblpaymode.Visible = true;
            }
            if (ddlpaymode.SelectedItem.Text == "Challan")
            {
                div_card.Visible = false;
                div_cheque.Visible = false;
                div_sndcheque.Visible = false;
                txt_chqno.Visible = false;
                txt_ddnar.Visible = false;
                btnpaysave.Visible = true;
                tblpaymode.Visible = true;
            }
            if (ddlpaymode.SelectedItem.Text == "Online")
            {
                div_card.Visible = false;
                div_cheque.Visible = false;
                div_sndcheque.Visible = false;
                txt_chqno.Visible = false;
                txt_ddno.Visible = false;
                txt_ddnar.Visible = false;
                btnpaysave.Visible = true;
                tblpaymode.Visible = true;
            }
            if (ddlpaymode.SelectedItem.Text == "Card")
            {
                div_card.Visible = true;
                div_cheque.Visible = false;
                div_sndcheque.Visible = false;
                txt_chqno.Visible = false;
                txt_ddno.Visible = false;
                txt_ddnar.Visible = false;
                btnpaysave.Visible = true;
                tblpaymode.Visible = true;
            }
        }
    }

    protected void btnpaysave_Click(object sender, EventArgs e)
    {
        string paymode = string.Empty;
        string bank = string.Empty;
        string branch = string.Empty;
        string chqorddno = string.Empty;
        string date = string.Empty;
        string ddnarrat = string.Empty;
        string oldpaymode = string.Empty;
        ArrayList arpaymode = new ArrayList();
        if (chkl_paid.Items.Count > 0)
        {
            //oldpaymode = getCblSelectedValue(chkl_paid);
            for (int i = 0; i < chkl_paid.Items.Count; i++)
            {
                if (chkl_paid.Items[i].Selected)
                {
                    if (!arpaymode.Contains(chkl_paid.Items[i].Value))
                        arpaymode.Add(chkl_paid.Items[i].Value);
                }
            }
        }

        if (ddlpaymode.Items.Count > 0)
            paymode = Convert.ToString(ddlpaymode.SelectedItem.Value);
        if (ddlpaymode.SelectedItem.Text == "Cheque")
        {
            chqorddno = Convert.ToString(txt_chqno.Text);
            if (ddl_bkname.Items.Count > 0)
                bank = Convert.ToString(ddl_bkname.SelectedItem.Value);
            branch = Convert.ToString(txt_branch.Text);
            date = Convert.ToString(txt_date1.Text);
            string[] spldate = date.Split('/');
            if (spldate.Length > 0)
                date = spldate[1] + "/" + spldate[0] + "/" + spldate[2];
        }
        else if (ddlpaymode.SelectedItem.Text == "DD")
        {
            if (ddl_bkname.Items.Count > 0)
                bank = Convert.ToString(ddl_bkname.SelectedItem.Value);
            branch = Convert.ToString(txt_branch.Text);
            date = Convert.ToString(txt_date1.Text);
            chqorddno = Convert.ToString(txt_ddno.Text);
            ddnarrat = Convert.ToString(txt_ddnar.Text);
            string[] spldate = date.Split('/');
            if (spldate.Length > 0)
                date = spldate[1] + "/" + spldate[0] + "/" + spldate[2];
        }
        else if (ddlpaymode.SelectedItem.Text == "Card")
        {
            if (ddlCardType.Items.Count > 0)
            {
                bank = Convert.ToString(ddlCardType.SelectedItem.Value);
                branch = Convert.ToString(ddlCardType.SelectedItem.Text);
            }
            chqorddno = Convert.ToString(txtLast4No.Text);
        }
        if (validate(paymode, chqorddno, bank, branch, date, ddnarrat, arpaymode))
        {
            savePaymodeDetails(paymode, chqorddno, bank, branch, date, ddnarrat, arpaymode);
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Provide The Details";
        }
    }
    private bool validate(string paymode, string chqorddno, string bank, string branch, string date, string ddnarrat, ArrayList arpaymode)
    {
        bool check = false;
        if (ddlpaymode.SelectedItem.Text == "Cheque")
        {
            if (!string.IsNullOrEmpty(paymode) && !string.IsNullOrEmpty(chqorddno) && !string.IsNullOrEmpty(bank) && bank != "Select" && !string.IsNullOrEmpty(branch) && !string.IsNullOrEmpty(date) && arpaymode.Count > 0)
                check = true;
            else
                check = false;
        }
        else if (ddlpaymode.SelectedItem.Text == "DD")
        {
            if (!string.IsNullOrEmpty(paymode) && !string.IsNullOrEmpty(chqorddno) && !string.IsNullOrEmpty(bank) && bank != "Select" && !string.IsNullOrEmpty(branch) && !string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(ddnarrat) && arpaymode.Count > 0)
                check = true;
            else
                check = false;
        }
        else if (ddlpaymode.SelectedItem.Text == "Card")
        {
            if (!string.IsNullOrEmpty(paymode) && !string.IsNullOrEmpty(chqorddno) && !string.IsNullOrEmpty(bank) && bank != "Select" && arpaymode.Count > 0)
                check = true;
            else
                check = false;
        }
        else
        {
            if (!string.IsNullOrEmpty(paymode) && arpaymode.Count > 0)
                check = true;
            else
                check = false;
        }
        return check;

    }
    private void savePaymodeDetails(string newpaymode, string chqorddno, string bank, string branch, string date, string ddnarrat, ArrayList arpaymode)
    {
        try
        {
            bool save = false;

            foreach (GridViewRow gdrow in gdfrcpt.Rows)
            {
                string dates = "";
                string appno = gdrow.Cells[1].Text;
                string transdate = gdrow.Cells[2].Text;
                string transcode = gdrow.Cells[3].Text;
                string feecat = gdrow.Cells[4].Text;
                string hdfk = gdrow.Cells[6].Text;
                string ldfk = gdrow.Cells[8].Text;
                string paymode = gdrow.Cells[11].Text;
                string finyrfk = gdrow.Cells[13].Text;
                string amount = gdrow.Cells[12].Text;
                string[] spldt = transdate.Split('/');
                dates = spldt[1] + "/" + spldt[0] + "/" + spldt[2];
                if (arpaymode.Contains(paymode))
                {
                    if (newpaymode == "2" || newpaymode == "3")
                    {
                        string collectClr = ",IsDeposited ='0',IsCollected='0'";

                        string UpdQ = "update ft_findailytransaction set paymode='" + newpaymode + "',ddno='" + chqorddno + "',dddate='" + date + "',ddbankcode='" + bank + "',ddbankbranch='" + branch + "'" + collectClr + " where app_no='" + appno + "' and feecategory='" + feecat + "' and ledgerfk='" + ldfk + "' and headerfk='" + hdfk + "' and transcode='" + transcode + "' and transdate='" + dates + "' and paymode='" + paymode + "' and finyearfk='" + finyrfk + "'";
                        int upd = d2.update_method_wo_parameter(UpdQ, "Text");
                    }
                    else if (newpaymode == "6")
                    {
                        string UpdQ = "update ft_findailytransaction set paymode='" + newpaymode + "',ddno='" + chqorddno + "',dddate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',ddbankcode='" + bank + "',ddbankbranch='" + branch + "' where app_no='" + appno + "' and feecategory='" + feecat + "' and ledgerfk='" + ldfk + "' and headerfk='" + hdfk + "' and transcode='" + transcode + "' and transdate='" + dates + "' and paymode='" + paymode + "' and finyearfk='" + finyrfk + "'";
                        int upd = d2.update_method_wo_parameter(UpdQ, "Text");
                    }
                    else
                    {
                        string UpdQ = "update ft_findailytransaction set paymode='" + newpaymode + "' where app_no='" + appno + "' and feecategory='" + feecat + "' and ledgerfk='" + ldfk + "' and headerfk='" + hdfk + "' and transcode='" + transcode + "' and transdate='" + dates + "' and paymode='" + paymode + "' and finyearfk='" + finyrfk + "'";
                        int upd = d2.update_method_wo_parameter(UpdQ, "Text");
                        //transdate='" + dates + "' and
                        string delQ = "if exists(select * from ft_finbanktransaction where  dailytransid='" + transcode + "' and finyearfk='" + finyrfk + "' and paymode='" + paymode + "')  delete from ft_finbanktransaction where  dailytransid='" + transcode + "' and finyearfk='" + finyrfk + "' and paymode='" + paymode + "' ";
                        int delqry = d2.update_method_wo_parameter(delQ, "Text");
                    }
                    if (newpaymode == "2" || newpaymode == "3")
                    {
                        // string UpdatQ = " if exists(select * from ft_finbanktransaction where transdate='" + dates + "' and dailytransid='" + transcode + "' and finyearfk='" + finyrfk + "' and paymode='" + paymode + "' and bankfk='" + bank + "') update ft_finbanktransaction set paymode='" + newpaymode + "',debit=isnull(debit,'0')+'" + amount + "' where transdate='" + dates + "' and dailytransid='" + transcode + "' and finyearfk='" + finyrfk + "' and paymode='" + paymode + "' and bankfk='" + bank + "'  else insert into ft_finbanktransaction(transdate,transtime,bankfk,paymode,dailytransid,isdeposited,iscleared,isbounced,debit,finyearfk) values('" + dates + "','" + DateTime.Now.ToLongTimeString() + "','" + bank + "','" + newpaymode + "','" + transcode + "','1','1','0','" + amount + "','" + finyrfk + "')";
                        // int upds = d2.update_method_wo_parameter(UpdatQ, "Text");
                    }
                    save = true;
                }
            }
            if (save == true)
            {
                Clear();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Updated Successfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Paymode Not Matched";
            }
        }
        catch { }
    }


    protected string isCollectedForDD()
    {
        string value = "0";
        string ddCollected = "select LinkValue from New_InsSettings where linkname = 'AutomaticallyClearDD' and user_code ='" + usercode + "' --and college_code ='" + collegecode + "'";
        value = d2.GetFunction(ddCollected).Trim();
        return value;
    }
    protected string AutoClearCheck()
    {
        string value = "0";
        string chqCleared = "select LinkValue from New_InsSettings where linkname = 'AutomaticallyClearCheque' and user_code ='" + usercode + "' --and college_code ='" + collegecode + "'";
        value = d2.GetFunction(chqCleared).Trim();
        return value;
    }
    public void bank(string collegecode)
    {
        try
        {
            ddl_bkname.Items.Clear();
            string queru = "select TextCode,TextVal  from textvaltable where TextCriteria = 'BName' and college_code='" + collegecode + "'";
            DataSet dsBank = d2.select_method_wo_parameter(queru, "Text");

            if (dsBank.Tables.Count > 0 && dsBank.Tables[0].Rows.Count > 0)
            {
                ddl_bkname.DataSource = dsBank;
                ddl_bkname.DataTextField = "TextVal";
                ddl_bkname.DataValueField = "TextCode";
                ddl_bkname.DataBind();
            }
            ddl_bkname.Items.Insert(0, "Select");
            ddl_bkname.Items.Insert(ddl_bkname.Items.Count, "Others");
        }
        catch (Exception ex) { }
    }
    public void cardType()
    {
        try
        {
            ddlCardType.Items.Clear();
            string queru = "select TextCode,TextVal  from textvaltable where TextCriteria = 'CardT'";
            DataSet dsCard = d2.select_method_wo_parameter(queru, "Text");

            if (dsCard.Tables.Count > 0 && dsCard.Tables[0].Rows.Count > 0)
            {
                ddlCardType.DataSource = dsCard;
                ddlCardType.DataTextField = "TextVal";
                ddlCardType.DataValueField = "TextCode";
                ddlCardType.DataBind();
            }
            ddlCardType.Items.Insert(0, "Select");
            ddlCardType.Items.Insert(ddlCardType.Items.Count, "Others");
        }
        catch (Exception ex) { }
    }

    protected void Clear()
    {
        txtfrcptno.Text = string.Empty;
        lblroll.Text = string.Empty;
        lblstudname.Text = string.Empty;
        lblbatch.Text = string.Empty;
        lbldept.Text = string.Empty;
        lblclg.Text = string.Empty;
        gdfrcpt.Visible = false;
        txt_chqno.Text = string.Empty;
        //ddl_bkname.SelectedIndex = 0;
        txt_branch.Text = string.Empty;
        txt_date1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_ddno.Text = string.Empty;
        txt_ddnar.Text = string.Empty;
        //ddlCardType.SelectedIndex = 0;
        txtLast4No.Text = string.Empty;
        tddet.Visible = false;
        gdfrcpt.Visible = false;
        tblpayfld.Visible = false;
        tblpaymode.Visible = false;
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

        lbl.Add(lbl_collegename);
        fields.Add(0);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    protected void ddlsem_Selcted(object sender, EventArgs e)
    {
        // txtrcpt_Changed(sender, e);
    }
}