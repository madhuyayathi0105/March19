/*
 * Author : Mohamed Idhris Sheik Dawood
 * Date Created : 02-11-2016
 * Purpose : Ledgerwise cumulative and detailed report for Fee Collection
 */

using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web.UI;
using System.Configuration;

public partial class FinanceCollectionReport : System.Web.UI.Page
{
    DAccess2 DA = new DAccess2();
    string collegeCode = "";
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    ArrayList colord = new ArrayList();
    string collegecode = string.Empty;
    static int chosedmode = 0;
    static int personmode = 0;
    static string memType = string.Empty;
    static ArrayList armemType = new ArrayList();
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    ArrayList colvisfalse = new ArrayList();

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
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                setCollegeCode();
                loadmemtype();
                loadpaid();
                setdate();
                loadheader();
                loadledger();
                LoadFromSettings();
            }
            setCollegeCode();
        }
        catch (Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string members = "";
            if (cblmem.Items[0].Selected)
                members = "STUDENTS,";
            if (cblmem.Items[1].Selected)
                members += "STAFFS,";
            if (cblmem.Items[2].Selected)
                members += "VENDORS,";
            if (cblmem.Items[3].Selected)
                members += "OTHERS,";

            members = members.TrimEnd(',');
            string degreedetails = "COLLECTION REPORT (Summary) FROM " + txt_fromdate.Text + " TO " + txt_todate.Text + " $ RECEIVED FROM " + members;
            string pagename = "FinanceCollectionReport.aspx";
            if (rbledgmode.SelectedIndex == 0)
            {
                //Printcontrol.loadspreaddetails(spreadReport, pagename, degreedetails);
                string ss = Session["usercode"].ToString();
                Printcontrol.loadspreaddetails(gridview1, pagename, degreedetails, 0, ss);
                Printcontrol.Visible = true;
            }
            else
            {
                //Printcontrol.loadspreaddetails(spreadlegdet, pagename, degreedetails);
                string ss = Session["usercode"].ToString();
                Printcontrol.loadspreaddetails(gridview2, pagename, degreedetails, 0, ss);
                Printcontrol.Visible = true;
            }
        }
        catch (Exception ex) { }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (rbledgmode.SelectedIndex == 0)
                {
                    
                    DA.printexcelreportgrid(gridview1, reportname);
                    lbl_validation.Visible = false;
                }
                else
                {
                    DA.printexcelreportgrid(gridview2, reportname);
                    lbl_validation.Visible = false;
                }

            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch (Exception ex) { }

    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        setCollegeCode();
        loadheader();
        loadledger();
    }
    private void setCollegeCode()
    {
        collegeCode = ddl_collegename.Items.Count > 0 ? ddl_collegename.SelectedValue : "";
        collegecode = ddl_collegename.Items.Count > 0 ? ddl_collegename.SelectedValue : "";
        collegecode1 = ddl_collegename.Items.Count > 0 ? ddl_collegename.SelectedValue : "";
    }
    public void loadcollege()
    {
        try
        {
            ddl_collegename.Items.Clear();
            DataSet dsCol = new DataSet();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            dsCol = DA.select_method_wo_parameter(Query, "Text");
            if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = dsCol;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
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
    private void loadmemtype()
    {
        try
        {
            cblmem.Items.Clear();
            cblmem.Items.Add(new ListItem("Student", "1"));
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
                LoadFromSettings();
            }
        }
        catch { }
    }
    protected void cbmem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbmem, cblmem, txtmem, "MemType", "--Select--");
        txtno.Text = "";
        lblappno.Text = "";
        LoadFromSettings();
    }
    protected void cblmem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbmem, cblmem, txtmem, "MemType", "--Select--");
        txtno.Text = "";
        lblappno.Text = "";
        LoadFromSettings();
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
    public void loadpaid()
    {
        try
        {
            chkl_paid.Items.Clear();
            //chkl_paid.Items.Add(new ListItem("Cash", "1"));
            //chkl_paid.Items.Add(new ListItem("Cheque", "2"));
            //chkl_paid.Items.Add(new ListItem("DD", "3"));
            //chkl_paid.Items.Add(new ListItem("Challan", "4"));
            //chkl_paid.Items.Add(new ListItem("Online", "5"));
            //  chkl_paid.Items.Add(new ListItem("Total Paid", "6"));
            DA.BindPaymodeToCheckboxList(chkl_paid, usercode, collegeCode);
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
        try
        {
            CallCheckboxChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");
        }
        catch
        { }
    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");
        }
        catch
        { }
    }
    private void setdate()
    {
        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fromdate.Attributes.Add("readonly", "readonly");
        txt_todate.Attributes.Add("readonly", "readonly");
    }
    public void loadheader()
    {
        try
        {
            chkl_studhed.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegeCode + "  ";

            DataSet dshdr = DA.select_method_wo_parameter(query, "Text");
            if (dshdr.Tables.Count > 0 && dshdr.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = dshdr;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderPK";
                chkl_studhed.DataBind();
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;
                }
                txt_studhed.Text = "Header(" + chkl_studhed.Items.Count + ")";
                chk_studhed.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void loadledger()
    {
        try
        {
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


            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegeCode + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            DataSet dslgr = new DataSet();
            dslgr = DA.select_method_wo_parameter(query1, "Text");
            if (dslgr.Tables.Count > 0 && dslgr.Tables[0].Rows.Count > 0)
            {
                chkl_studled.DataSource = dslgr;
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
        try
        {
            CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
            loadledger();
        }
        catch (Exception ex)
        { }
    }
    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, "Header", "--Select--");
            loadledger();
        }
        catch (Exception ex)
        {

        }
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");

        }
        catch (Exception ex)
        { }
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (rbledgmode.SelectedIndex == 0)
            {
                loadSpread();
            }
            else
            {
                loadSpreadDetail();
            }
        }
        catch
        {
            lblappno.Text = "";
            txtno.Text = "";
            imgAlert.Visible = true;
            lbl_alert.Text = "No records Found";
        }
    }
    private void loadSpread()
    {
        #region Spread Header Set
        Dictionary<string, string> dtpaymode = new Dictionary<string, string>();
        rptprint.Visible = false;
        gridview1.Visible = false;

        dtl.Clear();
        dtrow = dtl.NewRow();
        dtl.Rows.Add(dtrow);
        colvisfalse.Clear();
        

        for (int col = 0; col < 9; col++)
        {
            dtl.Columns.Add("", typeof(string));

        }

        

        if (chkl_paid.Items.Count > 0)
        {
            for (int i = 0; i < chkl_paid.Items.Count; i++)
            {
                if (chkl_paid.Items[i].Selected)
                {
                    dtpaymode.Add(Convert.ToString(chkl_paid.Items[i].Text).ToUpper(), Convert.ToString(chkl_paid.Items[i].Value));
                }
            }
        }

        

        dtl.Rows[0][0] = "S.No";
        colvisfalse.Add("true");

        

        dtl.Rows[0][1] = "NAME OF THE FEE";
        colvisfalse.Add("true");

        
        dtl.Rows[0][2] = "TOTAL";
        colvisfalse.Add("true");


        

        dtl.Rows[0][3] = "CASH";
        

        if (dtpaymode.ContainsKey("CASH"))
        {
            
            colvisfalse.Add("true");
        }
        else
        {
            
            colvisfalse.Add("false");
        }

        

        dtl.Rows[0][4] = "CHEQUE";
        
        if (dtpaymode.ContainsKey("CHEQUE"))
        {
            
            colvisfalse.Add("true");
        }
        else
        {
            
            colvisfalse.Add("false");
        }

       

        

        dtl.Rows[0][5] = "DD";

        if (dtpaymode.ContainsKey("DD"))
        {
            
            colvisfalse.Add("true");
        }
        else
        {
            
            colvisfalse.Add("false");
        }

        

        dtl.Rows[0][6] = "CHALLAN";

        if (dtpaymode.ContainsKey("CHALLAN"))
        {
            
            colvisfalse.Add("true");
        }
        else
        {
            
            colvisfalse.Add("false");
        }

        

        dtl.Rows[0][7] = "ONLINE";

        if (dtpaymode.ContainsKey("ONLINE"))
        {
            
            colvisfalse.Add("true");
        }
        else
        {
            
            colvisfalse.Add("false");
        }
        

        dtl.Rows[0][8] = "NEFT";

        if (dtpaymode.ContainsKey("NEFT"))
        {
            
            colvisfalse.Add("true");
        }
        else
        {
            
            colvisfalse.Add("false");
        }
        #endregion

        #region Search Input and Data retrieve

        DataTable dtHdrLeg = new DataTable();
        DataSet dsHeaderLedgerDetails = new DataSet();
        dsHeaderLedgerDetails = DA.select_method_wo_parameter("select LedgerPK,headerfk from FM_LedgerMaster", "Text");
        if (dsHeaderLedgerDetails.Tables.Count > 0)
        {
            dtHdrLeg = dsHeaderLedgerDetails.Tables[0];
        }

        string fromDate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
        string toDate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

        DataTable dtNewTable = new DataTable();
        dtNewTable.Columns.Add("Header");
        dtNewTable.Columns.Add("Ledger");
        StringBuilder sbLedger = new StringBuilder();
        for (int lgr = 0; lgr < chkl_studled.Items.Count; lgr++)
        {
            if (chkl_studled.Items[lgr].Selected)
            {
                string ledPk = chkl_studled.Items[lgr].Value;
                sbLedger.Append(ledPk + ",");
                dtHdrLeg.DefaultView.RowFilter = "ledgerpk='" + ledPk + "'";
                DataView dv = dtHdrLeg.DefaultView;
                if (dv.Count == 1)
                {
                    dtNewTable.Rows.Add(dv[0][1].ToString(), ledPk);
                }
            }
        }
        if (sbLedger.Length > 0)
        {
            sbLedger.Remove(sbLedger.Length - 1, 1);
            DataView dv = dtNewTable.DefaultView;
            dv.Sort = "Header asc";
            dtNewTable = dv.ToTable();
        }
        StringBuilder sbMemtype = new StringBuilder();
        for (int mem = 0; mem < cblmem.Items.Count; mem++)
        {
            if (cblmem.Items[mem].Selected)
            {
                sbMemtype.Append(cblmem.Items[mem].Value + ",");
            }
        }
        if (sbMemtype.Length > 0)
        {
            sbMemtype.Remove(sbMemtype.Length - 1, 1);
        }
        StringBuilder sbPaymode = new StringBuilder();
        for (int pmode = 0; pmode < chkl_paid.Items.Count; pmode++)
        {
            if (chkl_paid.Items[pmode].Selected)
            {
                sbPaymode.Append(chkl_paid.Items[pmode].Value + ",");
            }
        }
        if (sbPaymode.Length > 0)
        {
            sbPaymode.Remove(sbPaymode.Length - 1, 1);
        }

        DataSet dsFees = new DataSet();
        string selQ = string.Empty;
        if (txtno.Text.Trim() == string.Empty)
        {
            selQ = "select f.headerfk, f.ledgerfk,f.PayMode,sum(debit) as Total from ft_findailytransaction f where isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and f.LedgerFK in (" + sbLedger + ") and f.MemType in (" + sbMemtype + ") and f.PayMode in(" + sbPaymode + ") and transdate between '" + fromDate + "' and '" + toDate + "' group by f.headerfk, f.ledgerfk,f.PayMode order by f.Headerfk,f.LedgerFK";
        }
        else
        {
            string appNo = lblappno.Text;
            if (!string.IsNullOrEmpty(appNo))
            {
                if (ddladmit.SelectedIndex == 0)
                {
                    selQ = "select f.headerfk, f.ledgerfk,f.PayMode,sum(debit) as Total from ft_findailytransaction f where isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and f.LedgerFK in (" + sbLedger + ") and f.app_no='" + appNo + "' and f.PayMode in(" + sbPaymode + ") and transdate between '" + fromDate + "' and '" + toDate + "' group by f.headerfk, f.ledgerfk,f.PayMode order by f.Headerfk,f.LedgerFK";
                }
                else
                {
                    selQ = "select f.headerfk, f.ledgerfk,f.PayMode,sum(debit) as Total from ft_findailytransaction f where isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and f.LedgerFK in (" + sbLedger + ")  and f.Transcode='" + appNo + "' and f.PayMode in(" + sbPaymode + ") and transdate between '" + fromDate + "' and '" + toDate + "' group by f.headerfk, f.ledgerfk,f.PayMode order by f.Headerfk,f.LedgerFK";
                }
            }
        }

        selQ = selQ + "  select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + ddl_collegename.SelectedItem.Value + "' order by isnull(l.priority,1000), l.ledgerName asc";
        dsFees = DA.select_method_wo_parameter(selQ, "Text");
        #endregion
        if (dsFees.Tables.Count > 0 && dsFees.Tables[0].Rows.Count > 0)
        {
            int sNo = 0;
            int sRow = 0;

            double grndtotal = 0;
            double grndcash = 0;
            double grndcheque = 0;
            double grnddd = 0;
            double grndchallan = 0;
            double grndonline = 0;
            double grndneft = 0;
            for (int hdr = 0; hdr < chkl_studhed.Items.Count; hdr++)
            {
                if (chkl_studhed.Items[hdr].Selected)
                {
                    dtNewTable.DefaultView.RowFilter = "Header = " + chkl_studhed.Items[hdr].Value + "";
                    DataView dvLedger = dtNewTable.DefaultView;
                    if (dvLedger.Count > 0)
                    {
                        double hdrtotal = 0;
                        double hdrcash = 0;
                        double hdrcheque = 0;
                        double hdrdd = 0;
                        double hdrchallan = 0;
                        double hdronline = 0;
                        double hdrneft = 0;//abarna
                        bool headerOk = true;
                        for (int fRow = 0; fRow < dvLedger.Count; fRow++)
                        {
                            string ledgerID = dvLedger[fRow]["Ledger"].ToString();
                            dsFees.Tables[0].DefaultView.RowFilter = "ledgerfk = " + ledgerID + "";
                            DataView dvRec = dsFees.Tables[0].DefaultView;
                            if (dvRec.Count > 0)
                            {
                                if (headerOk)
                                {
                                    string headerID = dvLedger[0]["Header"].ToString();
                                    

                                    dtrow = dtl.NewRow();
                                    dtl.Rows.Add(dtrow);

                                    
                                    dtl.Rows[sRow + 1][1] = "A/C Name : " + chkl_studhed.Items.FindByValue(headerID).Text + "^#39ABC6";

                                    
                                    
                                    sRow++;
                                    headerOk = false;
                                }
                                

                                dtrow = dtl.NewRow();
                                dtl.Rows.Add(dtrow);

                                

                                dtl.Rows[sRow + 1][0] = (++sNo).ToString();

                                dsFees.Tables[1].DefaultView.RowFilter = "ledgerPk = " + ledgerID + "";
                                DataView dvhdname = dsFees.Tables[1].DefaultView;
                                if (dvhdname.Count > 0)
                                {
                                    
                                    dtl.Rows[sRow + 1][1] = Convert.ToString(dvhdname[0]["LedgerName"]);
                                }

                                double total = 0;
                                double cash = 0;
                                double cheque = 0;
                                double dd = 0;
                                double challan = 0;
                                double online = 0;
                                double neft = 0;//added by abarna
                                if (dtpaymode.ContainsKey("CASH"))
                                {
                                    dsFees.Tables[0].DefaultView.RowFilter = "ledgerfk = " + ledgerID + " and paymode='1'";
                                    DataView dvCash = dsFees.Tables[0].DefaultView;
                                    if (dvCash.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvCash[0]["Total"]), out cash);
                                    }
                                    total += cash;
                                }
                                if (dtpaymode.ContainsKey("CHEQUE"))
                                {
                                    dsFees.Tables[0].DefaultView.RowFilter = "ledgerfk = " + ledgerID + " and paymode='2'";
                                    DataView dvcheque = dsFees.Tables[0].DefaultView;
                                    if (dvcheque.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvcheque[0]["Total"]), out cheque);
                                    }
                                    total += cheque;
                                }
                                if (dtpaymode.ContainsKey("DD"))
                                {
                                    dsFees.Tables[0].DefaultView.RowFilter = "ledgerfk = " + ledgerID + " and paymode='3'";
                                    DataView dvdd = dsFees.Tables[0].DefaultView;
                                    if (dvdd.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvdd[0]["Total"]), out dd);
                                    }
                                    total += dd;
                                }
                                if (dtpaymode.ContainsKey("CHALLAN"))
                                {
                                    dsFees.Tables[0].DefaultView.RowFilter = "ledgerfk = " + ledgerID + " and paymode='4'";
                                    DataView dvchallan = dsFees.Tables[0].DefaultView;
                                    if (dvchallan.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvchallan[0]["Total"]), out challan);
                                    }
                                    total += challan;
                                }
                                if (dtpaymode.ContainsKey("ONLINE"))
                                {
                                    dsFees.Tables[0].DefaultView.RowFilter = "ledgerfk = " + ledgerID + " and paymode='5'";
                                    DataView dvonline = dsFees.Tables[0].DefaultView;
                                    if (dvonline.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvonline[0]["Total"]), out online);
                                    }
                                    total += online;
                                }
                                if (dtpaymode.ContainsKey("NEFT"))//added by abarna
                                {
                                    dsFees.Tables[0].DefaultView.RowFilter = "ledgerfk = " + ledgerID + " and paymode='7'";
                                    DataView dvneft = dsFees.Tables[0].DefaultView;
                                    if (dvneft.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvneft[0]["Total"]), out neft);
                                    }
                                    total += neft;
                                }
                                hdrtotal += total;
                                hdrcash += cash;
                                hdrcheque += cheque;
                                hdrdd += dd;
                                hdrchallan += challan;
                                hdronline += online;
                                hdrneft += neft;//added by abarna 
                                

                                dtl.Rows[sRow + 1][2] = total.ToString();
                                dtl.Rows[sRow + 1][3] = cash.ToString();
                                dtl.Rows[sRow + 1][4] = cheque.ToString();
                                dtl.Rows[sRow + 1][5] = dd.ToString();
                                dtl.Rows[sRow + 1][6] = challan.ToString();
                                dtl.Rows[sRow + 1][7] = online.ToString();
                                dtl.Rows[sRow + 1][8] = neft.ToString();
                                sRow++;
                            }
                        }
                        if (!headerOk)
                        {
                           

                            dtrow = dtl.NewRow();
                            dtl.Rows.Add(dtrow); 


                            

                            dtl.Rows[sRow + 1][1] = "A/C Total^#FF6000";
                            dtl.Rows[sRow + 1][2] = hdrtotal.ToString();
                            dtl.Rows[sRow + 1][3] = hdrcash.ToString();
                            dtl.Rows[sRow + 1][4] = hdrcheque.ToString();
                            dtl.Rows[sRow + 1][5] = hdrdd.ToString();
                            dtl.Rows[sRow + 1][6] = hdrchallan.ToString();
                            dtl.Rows[sRow + 1][7] = hdronline.ToString();
                            dtl.Rows[sRow + 1][8] = hdrneft.ToString();

                            sRow++;


                        }

                        grndtotal += hdrtotal;
                        grndcash += hdrcash;
                        grndcheque += hdrcheque;
                        grnddd += hdrdd;
                        grndchallan += hdrchallan;
                        grndonline += hdronline;
                        grndneft += hdrneft;//added by abarna
                    }
                }
            }

            

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow); 

            

            dtl.Rows[sRow + 1][1] = "Overall Total^#6CC137";
            dtl.Rows[sRow + 1][2] = grndtotal.ToString();
            dtl.Rows[sRow + 1][3] = grndcash.ToString();
            dtl.Rows[sRow + 1][4] = grndcheque.ToString();
            dtl.Rows[sRow + 1][5] = grnddd.ToString();
            dtl.Rows[sRow + 1][6] = grndchallan.ToString();
            dtl.Rows[sRow + 1][7] = grndonline.ToString();
            dtl.Rows[sRow + 1][8] = grndneft.ToString();
            


            //spreadReport.Width = 925;
            //spreadReport.Height = 400;
            
            rptprint.Visible = true;

            if (dtl.Rows.Count > 1)
            {
                gridview1.DataSource = dtl;
                gridview1.DataBind();
                gridview1.Visible = true;
                int colcout = 0;
                for (int i = 0; i < gridview1.Rows.Count; i++)
                {
                    //colcout = 0;
                    for (int j = 0; j < gridview1.HeaderRow.Cells.Count; j++)
                    {
                        if (colvisfalse[j].ToString() == "false" || colvisfalse[j].ToString() == "False")
                        {
                            gridview1.Rows[i].Cells[j].Visible = false;

                            //if (j < 5)
                            //    colcout++;
                        }
                    }
                }

                for (int i = 0; i < gridview1.Rows.Count; i++)
                {
                    for (int j = 0; j < gridview1.HeaderRow.Cells.Count; j++)
                    {
                        gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Right;
                        if (j == 0)
                        {
                            gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            gridview1.Rows[i].Cells[j].Width = 60;

                        }
                        else if (j == 1)
                        {
                            gridview1.Rows[i].Cells[j].Width = 240;
                            gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                        }
                        else if (j == 2)
                        {
                            gridview1.Rows[i].Cells[j].Width = 100;
                        }
                        else if (j == 3)
                        {
                            gridview1.Rows[i].Cells[j].Width = 100;
                        }
                        else if (j == 4)
                        {
                            gridview1.Rows[i].Cells[j].Width = 100;
                        }
                        else if (j == 5)
                        {
                            gridview1.Rows[i].Cells[j].Width = 240;
                        }
                        else if (j == 6)
                        {
                            gridview1.Rows[i].Cells[j].Width = 100;
                        }
                        else if (j == 7)
                        {
                            gridview1.Rows[i].Cells[j].Width = 100;
                        }
                        else if (j == 8)
                        {
                            gridview1.Rows[i].Cells[j].Width = 100;
                        }
                        if (i == 0)
                        {
                            gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            gridview1.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            gridview1.Rows[i].Cells[j].BorderColor = Color.Black;
                            gridview1.Rows[i].Cells[j].Font.Bold = true;
                            gridview1.Rows[i].Cells[j].Font.Name = "Book Antiqua";
                            gridview1.Rows[i].Cells[j].Font.Size = FontUnit.Medium;



                        }
                        else
                        {
                            string value = gridview1.Rows[i].Cells[j].Text.ToString();
                            string[] splitval = value.Split('^');
                            if (splitval.Length > 1)
                            {
                                gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                gridview1.Rows[i].Cells[j].Text = splitval[0];
                                gridview1.Rows[i].BackColor = ColorTranslator.FromHtml(splitval[1]);
                                gridview1.Rows[i].Font.Bold = true;
                                

                            }
                            int colspan = 1;

                            if (j == 1)
                            {
                                while (gridview1.Rows[i].Cells[j].Text != "&nbsp;" && gridview1.Rows[i].Cells[j + colspan].Text == "&nbsp;")
                                {
                                    colspan++;
                                    if (gridview1.HeaderRow.Cells.Count-1 == j + colspan)
                                        break;

                                }
                            }

                            if (colspan != 1)
                            {
                                gridview1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                gridview1.Rows[i].Cells[j].ColumnSpan = colspan;
                                for (int a = j + 1; a < j + colspan; a++)
                                    gridview1.Rows[i].Cells[a].Visible = false;
                            }




                        }



                    }

                }



            }
        }
        else
        {
            lblappno.Text = "";
            txtno.Text = "";
            imgAlert.Visible = true;
            lbl_alert.Text = "No records Found";
        }

    }
    //added by sudhagar
    protected void rbledgmode_Selected(object sender, EventArgs e)
    {
        if (rbledgmode.SelectedIndex == 0)
        {
            gridview1.Visible = false;
            rptprint.Visible = false;
            //
            divlegdet.Visible = false;
            pnlhead.Visible = false;
            pnlcolhed.Visible = false;
            gridview2.Visible = false;
        }
        else
        {
            loadColOrder();
            gridview1.Visible = false;
            rptprint.Visible = false;
            //
            divlegdet.Visible = false;
            pnlhead.Visible = false;
            pnlcolhed.Visible = false;
            gridview2.Visible = false;
        }
    }

    //btn search
    private void loadSpreadDetail()
    {
        try
        {
            #region Spread Header Set
            loaddailycolumns();
            rptprint.Visible = false;
            gridview2.Visible = false;
            

            dtl.Clear();
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            colvisfalse.Clear();

            

            for (int col = 0; col < 12; col++)
            {
                dtl.Columns.Add("", typeof(string));

            }

            

            dtl.Rows[0][0] = "S.No";
            colvisfalse.Add("true");

            // =================Added by saranya on 14112017 and 15112017=============================//

            

            dtl.Rows[0][1] = "Reg No";
            colvisfalse.Add("true");

            if (!colord.Contains("2"))
            {
                
                colvisfalse[1] = false;
            }
            if (colord.Count == 0)
            {
                
                colvisfalse[1] = true;
            }

            

            dtl.Rows[0][2] = "Department";
            colvisfalse.Add("true");

            if (!colord.Contains("9"))
            {
                
                colvisfalse[2] = false;
            }
            if (colord.Count == 0)
            {
                
                colvisfalse[2] = true;
            }
            //=============================================================================//

            

            dtl.Rows[0][3] = "Receipt No";
            colvisfalse.Add("true");

           

            if (!colord.Contains("1"))
            {
                
                colvisfalse[3] = false;
            }
            if (colord.Count == 0)
            {
                
                colvisfalse[3] = true;
            }

            

            dtl.Rows[0][4] = "Date";
            colvisfalse.Add("true");

            if (!colord.Contains("3"))
            {
                
                colvisfalse[4] = false;
            }
            if (colord.Count == 0)
            {
               
                colvisfalse[4] = true;
            }

            

            dtl.Rows[0][5] = "Received From";
            colvisfalse.Add("true");

            if (!colord.Contains("4"))
            {
                
                colvisfalse[5] = false;
            }
            if (colord.Count == 0)
            {
                
                colvisfalse[5] = true;
            }

            

            dtl.Rows[0][6] = "Mode of Payment";
            colvisfalse.Add("true");


            if (!colord.Contains("5"))
            {
                
                colvisfalse[6] = false;
            }
            if (colord.Count == 0)
            {
                
                colvisfalse[6] = true;
            }

           

            dtl.Rows[0][7] = "DD/Cheque No";
            colvisfalse.Add("true");

            if (!colord.Contains("6"))
            {
                
                colvisfalse[7] = false;
            }
            if (colord.Count == 0)
            {
                
                colvisfalse[7] = true;
            }

            

            dtl.Rows[0][8] = "Amount";
            colvisfalse.Add("true");

            if (!colord.Contains("7"))
            {
                
                colvisfalse[8] = false;
            }
            if (colord.Count == 0)
            {
                
                colvisfalse[8] = true;
            }


            

            dtl.Rows[0][9] = "Remarks";
            colvisfalse.Add("true");

            if (!colord.Contains("8"))
            {
                
                colvisfalse[9] = false;
            }
            if (colord.Count == 0)
            {
                
                colvisfalse[9] = true;
            }

            

            dtl.Rows[0][10] = "Bank Name";
            colvisfalse.Add("true");

            if (!colord.Contains("10"))
            {
                
                colvisfalse[10] = false;
            }
            if (colord.Count == 0)
            {
                
                colvisfalse[10] = true;
            }

            

            dtl.Rows[0][11] = "Bank BranchName";
            colvisfalse.Add("true");

            if (!colord.Contains("11"))
            {
                
                colvisfalse[11] = false;
            }
            if (colord.Count == 0)
            {
               
                colvisfalse[11] = true;
            }


            #endregion

            #region Search Input and Data retrieve

            DataTable dtHdrLeg = new DataTable();
            DataSet dsHeaderLedgerDetails = new DataSet();
            dsHeaderLedgerDetails = DA.select_method_wo_parameter("select LedgerPK,headerfk from FM_LedgerMaster", "Text");
            if (dsHeaderLedgerDetails.Tables.Count > 0)
            {
                dtHdrLeg = dsHeaderLedgerDetails.Tables[0];
            }

            string fromDate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
            string toDate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

            DataTable dtNewTable = new DataTable();
            dtNewTable.Columns.Add("Header");
            dtNewTable.Columns.Add("Ledger");
            StringBuilder sbLedger = new StringBuilder();
            for (int lgr = 0; lgr < chkl_studled.Items.Count; lgr++)
            {
                if (chkl_studled.Items[lgr].Selected)
                {
                    string ledPk = chkl_studled.Items[lgr].Value;
                    sbLedger.Append(ledPk + ",");
                    dtHdrLeg.DefaultView.RowFilter = "ledgerpk='" + ledPk + "'";
                    DataView dv = dtHdrLeg.DefaultView;
                    if (dv.Count == 1)
                    {
                        dtNewTable.Rows.Add(dv[0][1].ToString(), ledPk);
                    }
                }
            }
            if (sbLedger.Length > 0)
            {
                sbLedger.Remove(sbLedger.Length - 1, 1);
                DataView dv = dtNewTable.DefaultView;
                dv.Sort = "Header asc";
                dtNewTable = dv.ToTable();
            }
            StringBuilder sbMemtype = new StringBuilder();
            for (int mem = 0; mem < cblmem.Items.Count; mem++)
            {
                if (cblmem.Items[mem].Selected)
                {
                    sbMemtype.Append(cblmem.Items[mem].Value + ",");
                }
            }
            if (sbMemtype.Length > 0)
            {
                sbMemtype.Remove(sbMemtype.Length - 1, 1);
            }
            StringBuilder sbPaymode = new StringBuilder();
            for (int pmode = 0; pmode < chkl_paid.Items.Count; pmode++)
            {
                if (chkl_paid.Items[pmode].Selected)
                {
                    sbPaymode.Append(chkl_paid.Items[pmode].Value + ",");
                }
            }
            if (sbPaymode.Length > 0)
            {
                sbPaymode.Remove(sbPaymode.Length - 1, 1);
            }

            DataSet dsFees = new DataSet();
            string selQ = string.Empty;

            if (txtno.Text.Trim() == string.Empty)
            {
            
                    selQ = "select h.headername,f.headerfk ,l.LedgerName, f.ledgerfk,f.PayMode,sum(debit) as Total,f.app_no,f.memtype,f.Narration,f.Transcode,convert(varchar(10),f.Transdate,103) as Transdate,f.ddno as ddorchequeno,r.Reg_No,dr.Dept_Name from ft_findailytransaction f,Fm_LedgerMaster l,FM_HeaderMaster h,Registration r,Department dr where f.HeaderFK=h.HeaderPK and f.ledgerfk=l.ledgerpk and l.headerfk=f.headerfk and r.App_No=f.App_No and r.degree_code=dr.Dept_Code and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and f.LedgerFK in (" + sbLedger + ") and f.MemType in (" + sbMemtype + ") and f.PayMode in(" + sbPaymode + ") and transdate between '" + fromDate + "' and '" + toDate + "' group by f.ledgerfk,f.headerfk,l.LedgerName,f.PayMode,h.headername,f.app_no,f.memtype,f.Narration,f.Transcode,f.Transdate,f.ddno,r.Reg_No,dr.Dept_Name  order by f.Headerfk,f.LedgerFK,f.Transdate ";

                    selQ = "select h.headername,f.headerfk ,l.LedgerName, f.ledgerfk,f.PayMode,sum(debit) as Total,f.app_no,f.memtype,f.Narration,f.Transcode,convert(varchar(10),f.Transdate,103) as Transdate,f.ddno as ddorchequeno,f.DDBankCode,f.DDBankBranch  from ft_findailytransaction f,Fm_LedgerMaster l,FM_HeaderMaster h where f.HeaderFK=h.HeaderPK and f.ledgerfk=l.ledgerpk and l.headerfk=f.headerfk and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and f.LedgerFK in (" + sbLedger + ") and f.MemType in (" + sbMemtype + ") and f.PayMode in(" + sbPaymode + ") and transdate between '" + fromDate + "' and '" + toDate + "' group by f.ledgerfk,f.headerfk,l.LedgerName,f.PayMode,h.headername,f.app_no,f.memtype,f.Narration,f.Transcode,f.Transdate,f.ddno,f.DDBankCode,f.DDBankBranch   order by f.Headerfk,f.LedgerFK,f.Transdate ";
               
                //stud
                    selQ += " select distinct r.Reg_No,dt.dept_name,r.Stud_Name,r.app_no,ft.memtype from FT_FinDailyTransaction ft,Registration r,department dt,applyn p,degree d where ft.App_No=p.app_no and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and r.App_No=ft.App_No and r.App_No=p.app_no AND P.IsConfirm = 1 AND Admission_Status = 1 and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + ddl_collegename.SelectedItem.Value + "'  and TransCode<>'' and ft.memtype in(" + sbMemtype + ") and ft.Paymode in (" + sbPaymode + ") and LedgerFK in(" + sbLedger + ") AND  ft.TransDate between '" + fromDate + "' and '" + toDate + "'";//modified
                //staff
                selQ += " select distinct ft.App_no,s.staff_code,s.staff_name,ft.memtype from FT_FinDailyTransaction ft,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and ft.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and d.collegeCode='" + ddl_collegename.SelectedItem.Value + "' and ft.memtype in(" + sbMemtype + ") and ft.Paymode in (" + sbPaymode + ") and LedgerFK in(" + sbLedger + ") AND  ft.TransDate between '" + fromDate + "' and '" + toDate + "'";
                //vendor
                selQ += "  SELECT distinct p.VendorCode,ft.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,ft.memtype FROM FT_FinDailyTransaction ft,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =ft.App_No and P.VendorType ='1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and ft.memtype in(" + sbMemtype + ") and ft.Paymode in (" + sbPaymode + ") and LedgerFK in(" + sbLedger + ") AND  ft.TransDate between '" + fromDate + "' and '" + toDate + "'";
                //Others
                selQ += "  SELECT distinct p.vendorname,p.VendorCode,ft.App_no,p.VendorCompName,ft.memtype FROM FT_FinDailyTransaction ft,CO_VendorMaster P WHERE p.VendorPK=ft.App_No and P.VendorType ='-5' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and ft.memtype in(" + sbMemtype + ") and ft.Paymode in (" + sbPaymode + ") and LedgerFK in(" + sbLedger + ") AND  ft.TransDate between '" + fromDate + "' and '" + toDate + "'";
            }
            else
            {
                string strappno = string.Empty;
                string appNo = lblappno.Text;
                if (!string.IsNullOrEmpty(appNo))
                {
                    selQ = "select h.headername,f.headerfk ,l.LedgerName, f.ledgerfk,f.PayMode,sum(debit) as Total,f.app_no,f.memtype,f.Narration,f.Transcode,convert(varchar(10),f.Transdate,103) as Transdate,f.ddno as ddorchequeno from ft_findailytransaction f,Fm_LedgerMaster l,FM_HeaderMaster h where f.HeaderFK=h.HeaderPK and f.ledgerfk=l.ledgerpk and l.headerfk=f.headerfk and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and f.LedgerFK in (" + sbLedger + ") and f.MemType in (" + sbMemtype + ") and f.PayMode in(" + sbPaymode + ") and transdate between '" + fromDate + "' and '" + toDate + "' ";
                    if (ddladmit.SelectedIndex == 0)
                        selQ += " and f.app_no='" + appNo + "'";
                    else
                        selQ += " and  f.Transcode='" + appNo + "'";
                    selQ += "  group by f.ledgerfk,f.headerfk,l.LedgerName,f.PayMode,h.headername,f.app_no,f.memtype,f.Narration,f.Transcode,f.Transdate,f.ddno  order by f.Headerfk,f.LedgerFK,f.Transdate ";
                    //stud
                    selQ += " select distinct r.Reg_No,dt.dept_name,r.Stud_Name,r.app_no,ft.memtype from FT_FinDailyTransaction ft,Registration r,department dt,applyn p,degree d where ft.App_No=p.app_no and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and r.App_No=ft.App_No and r.App_No=p.app_no AND P.IsConfirm = 1 AND Admission_Status = 1 and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + ddl_collegename.SelectedItem.Value + "'  and TransCode<>'' and ft.memtype in(" + sbMemtype + ") and ft.Paymode in (" + sbPaymode + ") and LedgerFK in(" + sbLedger + ") AND  ft.TransDate between '" + fromDate + "' and '" + toDate + "'";//modified by abarna 12.04.2018
                    if (ddladmit.SelectedIndex == 0)
                        selQ += " and  ft.app_no='" + appNo + "'";
                    else
                        selQ += " and  ft.Transcode='" + appNo + "'";
                    //staff
                    selQ += " select distinct ft.App_no,s.staff_code,s.staff_name,ft.memtype from FT_FinDailyTransaction ft,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and ft.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and d.collegeCode='" + ddl_collegename.SelectedItem.Value + "' and ft.memtype in(" + sbMemtype + ") and ft.Paymode in (" + sbPaymode + ") and LedgerFK in(" + sbLedger + ") AND  ft.TransDate between '" + fromDate + "' and '" + toDate + "'";
                    if (ddladmit.SelectedIndex == 0)
                        selQ += " and  ft.app_no='" + appNo + "'";
                    else
                        selQ += " and  ft.Transcode='" + appNo + "'";
                    //vendor
                    selQ += "  SELECT distinct p.VendorCode,ft.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,ft.memtype FROM FT_FinDailyTransaction ft,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =ft.App_No and P.VendorType ='1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and ft.memtype in(" + sbMemtype + ") and ft.Paymode in (" + sbPaymode + ") and LedgerFK in(" + sbLedger + ") AND  ft.TransDate between '" + fromDate + "' and '" + toDate + "'";
                    if (ddladmit.SelectedIndex == 0)
                        selQ += " and  ft.app_no='" + appNo + "'";
                    else
                        selQ += " and  ft.Transcode='" + appNo + "'";
                    //Others
                    selQ += "  SELECT distinct p.vendorname,p.VendorCode,ft.App_no,p.VendorCompName,ft.memtype FROM FT_FinDailyTransaction ft,CO_VendorMaster P WHERE p.VendorPK=ft.App_No and P.VendorType ='-5' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and ft.memtype in(" + sbMemtype + ") and ft.Paymode in (" + sbPaymode + ") and LedgerFK in(" + sbLedger + ") AND  ft.TransDate between '" + fromDate + "' and '" + toDate + "'";
                    if (ddladmit.SelectedIndex == 0)
                        selQ += " and  ft.app_no='" + appNo + "'";
                    else
                        selQ += " and  ft.Transcode='" + appNo + "'";

                }
            }

            dsFees = DA.select_method_wo_parameter(selQ, "Text");
            #endregion

            #region bind values

            if (dsFees.Tables.Count > 0 && dsFees.Tables[0].Rows.Count > 0)
            {
                int sNo = 0;
                int sRow = 0;

                double grndtotal = 0;
                double grndcash = 0;
                double grndcheque = 0;
                double grnddd = 0;
                double grndchallan = 0;
                double grndonline = 0;
                FarPoint.Web.Spread.TextCellType txtddno = new FarPoint.Web.Spread.TextCellType();
                for (int hdr = 0; hdr < chkl_studhed.Items.Count; hdr++)
                {
                    if (chkl_studhed.Items[hdr].Selected)
                    {
                        dtNewTable.DefaultView.RowFilter = "Header = " + chkl_studhed.Items[hdr].Value + "";
                        DataView dvLedger = dtNewTable.DefaultView;
                        if (dvLedger.Count > 0)
                        {
                            bool headerOk = true;
                            double tempTot = 0;
                            for (int fRow = 0; fRow < dvLedger.Count; fRow++)
                            {
                                bool ledgerOk = true;
                                double total = 0;
                                string ledgerID = dvLedger[fRow]["Ledger"].ToString();
                                dsFees.Tables[0].DefaultView.RowFilter = "ledgerfk = " + ledgerID + "";
                                DataView dvRec = dsFees.Tables[0].DefaultView;
                                if (dvRec.Count > 0)
                                {
                                    for (int ledcnt = 0; ledcnt < dvRec.Count; ledcnt++)
                                    {
                                        if (headerOk)
                                        {
                                            string headerID = dvLedger[0]["Header"].ToString();
                                            
                                            dtrow = dtl.NewRow();
                                            dtl.Rows.Add(dtrow);
                                            

                                            dtl.Rows[sRow + 1][1] = "A/C Name : " + chkl_studhed.Items.FindByValue(headerID).Text +"^#39ABC6^7";

                                            sRow++;
                                            headerOk = false;
                                        }

                                        if (ledgerOk)
                                        {
                                            // string headerID = dvLedger[0]["Header"].ToString();
                                            string ledgername = Convert.ToString(dvRec[ledcnt]["LedgerName"]); ;
                                            

                                            dtrow = dtl.NewRow();
                                            dtl.Rows.Add(dtrow);

                                            

                                            dtl.Rows[sRow + 1][1] = ledgername + "^#39ABC6^3";

                                            sRow++;
                                            ledgerOk = false;
                                        }
                                        DataView dvname = new DataView();
                                        dvname = dsFees.Tables[1].DefaultView;
                                        

                                        dtrow = dtl.NewRow();
                                        dtl.Rows.Add(dtrow);

                                        

                                        dtl.Rows[sRow + 1][0] = (++sNo).ToString();

                                        dtl.Rows[sRow + 1][3] = Convert.ToString(dvRec[ledcnt]["TransCode"]);

                                        dtl.Rows[sRow + 1][4] = Convert.ToString(dvRec[ledcnt]["Transdate"]);

                                        //spreadlegdet.Sheets[0].Cells[sRow, 4].Text = Convert.ToString(dvname[0]["Stud_Name"]);
                                        //name
                                        string Name = "";
                                        
                                        string Memtype = Convert.ToString(dvRec[ledcnt]["memtype"]);
                                    
                                        if (Memtype == "1")
                                        {
                                            dsFees.Tables[1].DefaultView.RowFilter = "App_no='" + Convert.ToString(dvRec[ledcnt]["app_no"]) + "' and memtype='" + Memtype + "'";
                                            dvname = dsFees.Tables[1].DefaultView;
                                            if (dvname.Count > 0)
                                            {
                                                Name = Convert.ToString(dvname[0]["Stud_Name"]);
                                                //changed  by saranya on 8/02/2018
                                               
                                                dtl.Rows[sRow + 1][1] = Convert.ToString(dvname[0]["Reg_No"]);

                                                dtl.Rows[sRow + 1][2] = Convert.ToString(dvname[0]["dept_name"]);
                                                //================================//
                                            }
                                               
                                               
                                        }
                                        else if (Memtype == "2")
                                        {
                                            dsFees.Tables[2].DefaultView.RowFilter = "App_no='" + Convert.ToString(dvRec[ledcnt]["app_no"]) + "' and memtype='" + Memtype + "'";
                                            dvname = dsFees.Tables[2].DefaultView;
                                            if (dvname.Count > 0)
                                                Name = Convert.ToString(dvname[0]["staff_name"]);
                                            
                                        }
                                        else if (Memtype == "3")
                                        {
                                            dsFees.Tables[3].DefaultView.RowFilter = "App_no='" + Convert.ToString(dvRec[ledcnt]["app_no"]) + "' and memtype='" + Memtype + "'";
                                            dvname = dsFees.Tables[3].DefaultView;
                                            if (dvname.Count > 0)
                                                Name = Convert.ToString(dvname[0]["VenContactName"]);
                                        }
                                        else if (Memtype == "4")
                                        {
                                            dsFees.Tables[4].DefaultView.RowFilter = "App_no='" + Convert.ToString(dvRec[ledcnt]["app_no"]) + "' and memtype='" + Memtype + "'";
                                            dvname = dsFees.Tables[4].DefaultView;
                                            if (dvname.Count > 0)
                                            {
                                                Name = Convert.ToString(dvname[0]["vendorname"]);
                                            }
                                        }
                                        

                                        dtl.Rows[sRow + 1][5] = Name;

                                        //paymode
                                        string paymode = Convert.ToString(dvRec[ledcnt]["paymode"]);
                                        string mode = "";
                                        if (paymode == "1")
                                            mode = "Cash";
                                        else if (paymode == "2")
                                            mode = "Cheque";
                                        else if (paymode == "3")
                                            mode = "DD";
                                        else if (paymode == "4")
                                            mode = "Challan";
                                        else if (paymode == "5")
                                            mode = "Online";
                                        else if (paymode == "6")
                                            mode = "Card";

                                        

                                        dtl.Rows[sRow + 1][6] = mode;

                                        string ddorcheq = Convert.ToString(dvRec[ledcnt]["ddorchequeno"]);
                                        //------------------------------------------------------------
                                        string bankcode = Convert.ToString(dvRec[ledcnt]["DDBankCode"]);//abarna
                                        string branchname = Convert.ToString(dvRec[ledcnt]["DDBankBranch"]);
                                        string bankname = string.Empty;
                                        if(bankcode =="" || bankcode =="0")
                                        {
                                            bankname = "";
                                        }
                                        else
                                        {
                                            bankname = DA.GetFunction("select textval from textvaltable where textcode='" + bankcode + "'");
                                            if (bankname == "" || bankname =="0")
                                            {
                                                bankname = DA.GetFunction("select bankname from FM_FinBankMaster where bankpk='" + bankcode + "'");
                                            }
                                        }
                                        //--------------------------------------------------------------------
                                        

                                        

                                        if (!string.IsNullOrEmpty(ddorcheq))
                                        {
                                            
                                            dtl.Rows[sRow + 1][7] = Convert.ToString(dvRec[ledcnt]["ddorchequeno"]);
                                        }
                                        else
                                        {
                                            
                                            dtl.Rows[sRow + 1][7] = "-";
                                        }
                                        double Amt = 0;
                                        double.TryParse(Convert.ToString(dvRec[ledcnt]["Total"]), out Amt);
                                        total += Amt;
                                        

                                        dtl.Rows[sRow + 1][8] = Convert.ToString(Amt);
                                        dtl.Rows[sRow + 1][9] = Convert.ToString(dvRec[ledcnt]["Narration"]);
                                        dtl.Rows[sRow + 1][10] = Convert.ToString(bankname);
                                        dtl.Rows[sRow + 1][11] = Convert.ToString(branchname);
                                        sRow++;
                                    }
                                    if (!ledgerOk)
                                    {
                                        

                                        dtrow = dtl.NewRow();
                                        dtl.Rows.Add(dtrow);

                                        

                                        dtl.Rows[sRow + 1][1] = "Total^#FF6000";

                                        

                                        dtl.Rows[sRow + 1][8] = total.ToString();

                                        sRow++;
                                    }
                                    tempTot += total;
                                }
                            }
                            if (!headerOk)
                            {
                                

                                dtrow = dtl.NewRow();
                                dtl.Rows.Add(dtrow);

                               

                                dtl.Rows[sRow + 1][1] = "Header Total^YellowGreen";

                                
                                //spreadlegdet.Sheets[0].Rows[sRow].BackColor = ColorTranslator.FromHtml("#FF8000");
                                

                                dtl.Rows[sRow + 1][8] = tempTot.ToString();

                                sRow++;
                            }
                            grndtotal += tempTot;
                        }
                    }
                }
                

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                

                dtl.Rows[sRow + 1][1] = "Overall Total^Green";

                
                dtl.Rows[sRow + 1][8] = grndtotal.ToString();

               
                //spreadlegdet.Sheets[0].Rows[sRow].BackColor = ColorTranslator.FromHtml("#6CC137");
                
                //spreadlegdet.Width = 925;
                //spreadlegdet.Height = 400;
                
                divlegdet.Visible = true;
                pnlhead.Visible = true;
                pnlcolhed.Visible = true;
                gridview2.Visible = true;
                rptprint.Visible = true;

                

               

                if (dtl.Rows.Count > 1)
                {
                    gridview2.DataSource = dtl;
                    gridview2.DataBind();
                    gridview2.Visible = true;
                    int colcoutin7 = 0;
                    int colcoutin3 = 0;

                    int dtrowcount = dtl.Rows.Count;
                    int rowspanstart2 = 0;
                    int rowspanstart4 = 0;

                    for (int i = 0; i < gridview2.Rows.Count; i++)
                    {
                        colcoutin7 = 0;
                        colcoutin3 = 0;
                        for (int j = 0; j < gridview2.HeaderRow.Cells.Count; j++)
                        {
                            if (colvisfalse[j].ToString() == "false" || colvisfalse[j].ToString() == "False")
                            {
                                gridview2.Rows[i].Cells[j].Visible = false;

                                if (j < 8)
                                    colcoutin7++;
                                if (j < 4)
                                    colcoutin3++;
                            }
                        }
                    }

                    for (int i = 0; i < gridview2.Rows.Count; i++)
                    {

                        int rowspancount2 = 0;
                        int rowspancount4 = 0;
                        


                        if (i != dtrowcount - 1)
                        {
                            if (rowspanstart2 == i)
                            {
                                for (int k = rowspanstart2 + 1; gridview2.Rows[i].Cells[2].Text == gridview2.Rows[k].Cells[2].Text && gridview2.Rows[k].Cells[2].Text != "&nbsp;"; k++)
                                {
                                    rowspancount2++;
                                    if (k == dtrowcount - 1)
                                        break;
                                }
                                rowspanstart2++;
                            }
                            if (rowspanstart4 == i)
                            {
                                for (int k = rowspanstart4 + 1; gridview2.Rows[i].Cells[4].Text == gridview2.Rows[k].Cells[4].Text && gridview2.Rows[k].Cells[4].Text != "&nbsp;"; k++)
                                {
                                    rowspancount4++;
                                    if (k == dtrowcount - 1)
                                        break;
                                }
                                rowspanstart4++;
                            }
                            


                            if (rowspancount2 != 0)
                            {
                                rowspanstart2 = rowspanstart2 + rowspancount2;
                                gridview2.Rows[i].Cells[2].VerticalAlign=VerticalAlign.Top;
                                gridview2.Rows[i].Cells[2].RowSpan = rowspancount2 + 1;
                                for (int a = i; a < rowspanstart2 - 1; a++)
                                    gridview2.Rows[a + 1].Cells[2].Visible = false;

                            }
                            if (rowspancount4 != 0)
                            {
                                rowspanstart4 = rowspanstart4 + rowspancount4;
                                gridview2.Rows[i].Cells[4].VerticalAlign = VerticalAlign.Top;
                                gridview2.Rows[i].Cells[4].RowSpan = rowspancount4 + 1;
                                for (int a = i; a < rowspanstart4 - 1; a++)
                                    gridview2.Rows[a + 1].Cells[4].Visible = false;

                            }

                           





                        }
                        for (int j = 0; j < gridview2.HeaderRow.Cells.Count; j++)
                        {
                            
                            if (j == 0)
                            {
                                gridview2.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                gridview2.Rows[i].Cells[j].Width = 40;

                            }
                            else if (j == 1)
                            {
                                gridview2.Rows[i].Cells[j].Width = 130;
                                
                            }
                            else if (j == 2)
                            {
                                gridview2.Rows[i].Cells[j].Width = 500;
                            }
                            else if (j == 3)
                            {
                                gridview2.Rows[i].Cells[j].Width = 130;
                            }
                            else if (j == 4)
                            {
                                gridview2.Rows[i].Cells[j].Width = 100;
                                gridview2.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (j == 5)
                            {
                                gridview2.Rows[i].Cells[j].Width = 400;
                            }
                            else if (j == 6)
                            {
                                gridview2.Rows[i].Cells[j].Width = 100;
                            }
                            else if (j == 7)
                            {
                                gridview2.Rows[i].Cells[j].Width = 100;
                            }
                            else if (j == 8)
                            {
                                gridview2.Rows[i].Cells[j].Width = 90;
                                gridview2.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Right;
                            }
                            else if (j == 9)
                            {
                                gridview2.Rows[i].Cells[j].Width = 300;
                            }
                            else if (j == 10)
                            {
                                gridview2.Rows[i].Cells[j].Width = 300;
                            }
                            else if (j == 11)
                            {
                                gridview2.Rows[i].Cells[j].Width = 300;
                            }
                            if (i == 0)
                            {
                                gridview2.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                gridview2.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                gridview2.Rows[i].Cells[j].BorderColor = Color.Black;
                                gridview2.Rows[i].Cells[j].Font.Bold = true;
                                gridview2.Rows[i].Cells[j].Font.Name = "Book Antiqua";
                                gridview2.Rows[i].Cells[j].Font.Size = FontUnit.Medium;



                            }
                            else
                            {
                                string value = gridview2.Rows[i].Cells[j].Text.ToString();
                                string[] splitval = value.Split('^');
                                if (splitval.Length > 1)
                                {
                                    gridview2.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                    gridview2.Rows[i].Cells[j].Text = splitval[0];
                                    gridview2.Rows[i].BackColor = ColorTranslator.FromHtml(splitval[1]);
                                    gridview2.Rows[i].Font.Bold = true;


                                }
                                //int colspan = 1;

                                //if (j == 1)
                                //{
                                //    while (gridview2.Rows[i].Cells[j].Text != "&nbsp;" && gridview2.Rows[i].Cells[j + colspan].Text == "&nbsp;")
                                //    {
                                //        colspan++;
                                //        if (gridview2.HeaderRow.Cells.Count - 1 == j + colspan)
                                //            break;

                                //    }
                                //}

                                if (splitval.Length > 2)
                                {
                                    if (gridview2.Rows[i].Cells[j].Visible == true)
                                    {
                                        gridview2.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                        if (Convert.ToInt32(splitval[2]) == 3)
                                        {
                                            gridview2.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                                            gridview2.Rows[i].Cells[j].ColumnSpan = Convert.ToInt32(splitval[2]) - colcoutin3;
                                        }
                                        else if (Convert.ToInt32(splitval[2]) == 7)
                                            gridview2.Rows[i].Cells[j].ColumnSpan = Convert.ToInt32(splitval[2]) - colcoutin7;
                                        for (int a = j + 1; a < j + Convert.ToInt32(splitval[2]); a++)
                                            gridview2.Rows[i].Cells[a].Visible = false;
                                    }
                                }




                            }



                        }

                    }



                }
            }
            else
            {
                txtno.Text = "";
                lblappno.Text = "";
                imgAlert.Visible = true;
                lbl_alert.Text = "No records Found";
            }
            #endregion
        }
        catch { }

    }

    #region column order
    protected void cbllegdet_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cblegdet.Checked == true)
            {
                for (int i = 0; i < cbllegdet.Items.Count; i++)
                {
                    cbllegdet.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < cbllegdet.Items.Count; i++)
                {
                    cbllegdet.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    protected void cbllegdet_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ArrayList ht = new ArrayList();
            for (int i = 0; i < cbllegdet.Items.Count; i++)
            {
                if (cbllegdet.Items[i].Selected == true)
                {
                    ht.Add(Convert.ToString(cbllegdet.Items[i].Value));
                }
            }
            ViewState["colvalue"] = Convert.ToString(ht);
        }
        catch { }
    }
    protected void loadColOrder()
    {
        try
        {
            cbllegdet.Items.Clear();
            cbllegdet.Items.Add(new ListItem("Receipt No", "1"));
            cbllegdet.Items.Add(new ListItem("Reg No","2"));   
            cbllegdet.Items.Add(new ListItem("Date", "3"));
            cbllegdet.Items.Add(new ListItem("Received From", "4"));
            cbllegdet.Items.Add(new ListItem("Mode of Payment", "5"));
            cbllegdet.Items.Add(new ListItem("DD/Cheque No", "6"));
            cbllegdet.Items.Add(new ListItem("Amount", "7"));
            cbllegdet.Items.Add(new ListItem("Remarks", "8"));
            cbllegdet.Items.Add(new ListItem("Department", "9"));
            cbllegdet.Items.Add(new ListItem("Bank Name", "10"));//abarna
            cbllegdet.Items.Add(new ListItem("Bank BranchName", "11"));//abarna
        }
        catch
        { }
    }
    public bool dailycolumncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cbllegdet.Items.Count; i++)
            {
                if (cbllegdet.Items[i].Selected == true)
                {
                    colorder = true;
                }
            }
        }
        catch { }
        return colorder;
    }
    public void loaddailycolumns()
    {
        try
        {
            string linkname = "Fee Collection Report column order settings";
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + ddl_collegename.SelectedItem.Value + "' ";
            dscol.Clear();
            dscol = DA.select_method_wo_parameter(selcol, "Text");
            if (dailycolumncount() == true)
            {
                if (cbllegdet.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < cbllegdet.Items.Count; i++)
                    {
                        if (cbllegdet.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cbllegdet.Items[i].Value));
                            if (columnvalue == "")
                            {
                                columnvalue = Convert.ToString(cbllegdet.Items[i].Value);
                            }
                            else
                            {
                                columnvalue = columnvalue + ',' + Convert.ToString(cbllegdet.Items[i].Value);
                            }
                            // columnvalue = Convert.ToString(ViewState["colvalue"]);
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0 && dscol.Tables[0].Rows.Count > 0)
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
                            {
                                columnvalue = Convert.ToString(valuesplit[k]);
                            }
                            else
                            {
                                columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                            }
                        }
                    }
                }
            }
            else
            {
                colord.Clear();
                for (int i = 0; i < cbllegdet.Items.Count; i++)
                {
                    cbllegdet.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cbllegdet.Items[i].Value));
                    if (columnvalue == "")
                    {
                        columnvalue = Convert.ToString(cbllegdet.Items[i].Value);
                    }
                    else
                    {
                        columnvalue = columnvalue + ',' + Convert.ToString(cbllegdet.Items[i].Value);
                    }
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + ddl_collegename.SelectedItem.Value + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + ddl_collegename.SelectedItem.Value + "')";
                clsupdate = DA.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + ddl_collegename.SelectedItem.Value + "' ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = DA.select_method_wo_parameter(sel, "Text");
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
                                for (int k = 0; k < cbllegdet.Items.Count; k++)
                                {
                                    if (val == cbllegdet.Items[k].Value)
                                    {
                                        cbllegdet.Items[k].Selected = true;
                                        count++;
                                    }
                                    if (count == cbllegdet.Items.Count)
                                    {
                                        cblegdet.Checked = true;
                                    }
                                    else
                                    {
                                        cblegdet.Checked = false;
                                    }
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

    #region auto search
    public void LoadFromSettings()
    {
        try
        {
            getMemType();
            txtno.Text = "";
            //System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("Roll No", "0");
            //System.Web.UI.WebControls.ListItem lst2 = new System.Web.UI.WebControls.ListItem("Reg No", "1");
            //System.Web.UI.WebControls.ListItem lst3 = new System.Web.UI.WebControls.ListItem("Admission No", "2");
            //System.Web.UI.WebControls.ListItem lst4 = new System.Web.UI.WebControls.ListItem("App No", "3");
            //System.Web.UI.WebControls.ListItem lst5 = new System.Web.UI.WebControls.ListItem("Name", "4");
            System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("Name", "0");
            System.Web.UI.WebControls.ListItem lst2 = new System.Web.UI.WebControls.ListItem("Receipt No", "1");
            #region old
            //Roll Number or Reg Number or Admission No or Application Number
            //ddladmit.Items.Clear();
            //string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";

            //int save1 = Convert.ToInt32(DA.GetFunction(insqry1));

            //if (save1 == 1)
            //{
            //    //Roll No
            //    ddladmit.Items.Add(lst1);
            //}


            //insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
            //save1 = Convert.ToInt32(DA.GetFunction(insqry1));
            //if (save1 == 1)
            //{
            //    //RegNo
            //    ddladmit.Items.Add(lst2);
            //}

            //insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
            //save1 = Convert.ToInt32(DA.GetFunction(insqry1));
            //if (save1 == 1)
            //{
            //    //Admission No - Roll Admit
            //    ddladmit.Items.Add(lst3);
            //}

            //insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            //save1 = Convert.ToInt32(DA.GetFunction(insqry1));

            //if (save1 == 1)
            //{
            //    //App Form Number - Application Number
            //    ddladmit.Items.Add(lst4);

            //}
            //if (ddladmit.Items.Count == 0)
            //{
            //    ddladmit.Items.Add(lst1);
            //}
            //ddladmit.Items.Add(lst5);
            //switch (Convert.ToUInt32(ddladmit.SelectedItem.Value))
            //{
            //    case 0:
            //        txtno.Attributes.Add("placeholder", "Roll No");
            //        chosedmode = 0;
            //        break;
            //    case 1:
            //        txtno.Attributes.Add("placeholder", "Reg No");
            //        chosedmode = 1;
            //        break;
            //    case 2:
            //        txtno.Attributes.Add("placeholder", "Admin No");
            //        chosedmode = 2;
            //        break;
            //    case 3:
            //        txtno.Attributes.Add("placeholder", "App No");
            //        chosedmode = 3;
            //        break;
            //    case 4:
            //        txtno.Attributes.Add("placeholder", "");
            //        chosedmode = 4;
            //        break;
            //}
            #endregion
            ddladmit.Items.Clear();
            ddladmit.Items.Add(lst1);
            ddladmit.Items.Add(lst2);
            switch (Convert.ToUInt32(ddladmit.SelectedItem.Value))
            {
                case 0:
                    txtno.Attributes.Add("placeholder", "Name");
                    chosedmode = 0;
                    break;
                case 1:
                    txtno.Attributes.Add("placeholder", "Receipt No");
                    chosedmode = 1;
                    break;
                //case 2:
                //    txtno.Attributes.Add("placeholder", "Admin No");
                //    chosedmode = 2;
                //    break;
                //case 3:
                //    txtno.Attributes.Add("placeholder", "App No");
                //    chosedmode = 3;
                //    break;
                //case 4:
                //    txtno.Attributes.Add("placeholder", "");
                //    chosedmode = 4;
                //    break;
            }
        }
        catch { }
    }
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
                ////student query
                //if (chosedmode == 0)
                //{                   
                //    query = "select top 100 Roll_No from Registration where Roll_No like '" + prefixText + "%' and college_code=" + collegecode1 + " order by Roll_No asc";
                //}
                //else if (chosedmode == 1)
                //{                   
                //    query = "select  top 100 Reg_No from Registration where Reg_No like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Reg_No asc";
                //}
                //else if (chosedmode == 2)
                //{
                //    query = "select  top 100 Roll_admit from Registration where Roll_admit like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";
                //}
                if (chosedmode == 0)
                {
                    if (armemType.Count > 0)
                    {
                        if (armemType.Contains("Student"))
                        {
                            query = "select Stud_Name+'$'+Roll_No+'$'+(select c.Course_Name+'$'+dept_name from Department dt,Degree d,course c where c.Course_Id=d.Course_Id and dt.Dept_Code =d.Dept_Code and d.Degree_Code=r.degree_code) as Roll_admit from Registration r where Stud_Name like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";
                        }
                        if (armemType.Contains("Staff"))
                        {
                            query += " select distinct staff_Name+'$'+s.staff_code as staff from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
                        }
                        if (armemType.Contains("Vendor"))
                        {
                            query += " select VendorCompName+'$'+VendorCode as vendorcodename ,VendorPK  from CO_VendorMaster where VendorType =1 and VendorCompName like '" + prefixText + "%' ";
                        }
                        if (armemType.Contains("Others"))
                        {
                            query += " select (VendorName +'$'+ VendorCode) as VendorName from CO_VendorMaster  where VendorType='-5' and VendorName like '%' ";
                        }

                    }
                }
                else
                {
                    query = "select distinct top 100 Transcode from FT_FinDailyTransaction where memtype in('" + memType + "') and Transcode like '" + prefixText + "%' order by Transcode asc";
                }
                //else
                //{
                //    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by app_formno asc";
                //}
            }
            else if (personmode == 1)
            {
                //staff query
            }
            else if (personmode == 2)
            {
                //Vendor query
            }
            else
            {
                //Others query
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    #endregion

    protected void getMemType()
    {
        armemType.Clear();
        for (int row = 0; row < cblmem.Items.Count; row++)
        {
            if (cblmem.Items[row].Selected)
            {
                if (memType == string.Empty)
                    memType = Convert.ToString(cblmem.Items[row].Value);
                else
                    memType += "'" + "," + "'" + Convert.ToString(cblmem.Items[row].Value); ;
                armemType.Add(Convert.ToString(cblmem.Items[row].Text));
            }
        }
    }

    protected void txtno_TextChanged(object sender, EventArgs e)
    {
        string gettext = Convert.ToString(txtno.Text);
        if (!string.IsNullOrEmpty(gettext))
        {
            getappno(gettext);
        }
    }
    protected void getappno(string gettext)
    {
        try
        {
            lblappno.Text = string.Empty;
            string AppNo = string.Empty;
            if (ddladmit.SelectedIndex == 0)
            {
                string number = gettext.Split('$')[1];
                if (!string.IsNullOrEmpty(number))
                {
                    AppNo = DA.GetFunction("select App_No from Registration where Roll_No='" + number + "' and college_code='" + collegecode1 + "' ");
                    if (AppNo == "0")
                        AppNo = DA.GetFunction("select sa.appl_id  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.staff_code ='" + number + "'");
                    if (AppNo == "0")
                        AppNo = DA.GetFunction("select VendorContactPK from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and v.VendorCode='" + number + "' and vendorType='1'");
                    if (AppNo == "0")
                        AppNo = DA.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + number + "' and vendorType='-5'");
                    if (AppNo != "0")
                    {
                        txtno.Text = Convert.ToString(gettext.Split('$')[0]);
                        lblappno.Text = AppNo;
                    }
                }
            }
            else
            {
                txtno.Text = Convert.ToString(gettext);
                lblappno.Text = gettext;
            }
        }
        catch { }
    }
    protected void ddladmit_SelectedIndexChanged(object sender, EventArgs e)
    {
        getMemType();
        txtno.Text = "";
        lblnum.Text = ddladmit.SelectedItem.ToString();
        switch (Convert.ToUInt32(ddladmit.SelectedItem.Value))
        {
            case 0:
                txtno.Attributes.Add("placeholder", "Name");
                chosedmode = 0;
                break;
            case 1:
                txtno.Attributes.Add("placeholder", "Receipt No");
                chosedmode = 1;
                break;
            //case 2:
            //    txtno.Attributes.Add("placeholder", "Admin No");
            //    chosedmode = 2;
            //    break;
            //case 3:
            //    txtno.Attributes.Add("placeholder", "App No");
            //    chosedmode = 3;
            //    break;
            //case 4:
            //    txtno.Attributes.Add("placeholder", "");
            //    chosedmode = 4;
            //    break;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }

}
/*
 * Last Modified by Sudhagar  - 03-11-2016
 */