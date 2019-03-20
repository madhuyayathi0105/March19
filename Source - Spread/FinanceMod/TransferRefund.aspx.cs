using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using InsproDataAccess;
using System.Text;
using System.IO;
using System.Configuration;

public partial class TransferRefund : System.Web.UI.Page
{
    static byte BalanceType = 0;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    InsproDirectAccess DirAccess = new InsproDirectAccess();
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    int collegeCode = 0;
    int userCode = 0;
    static string stcollegecode = string.Empty;
    ReuasableMethods reUse = new ReuasableMethods();
    string selectQuery = "";

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    DAccess2 d2 = new DAccess2();
    DAccess2 DA = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt;
    int row;
    int i;
    string batch = "";
    string degree = "";
    static Hashtable studhash = new Hashtable();
    static string seatype = "";
    static int chosedmode = 0;
    static int personmode = 0;
    static int admis = 0;

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
            userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            // collegecode1 = Session["collegecode"].ToString();
            sessstream = Convert.ToString(Session["streamcode"]);
            lbl_str1.Text = sessstream;
            lbl_str2.Text = sessstream;
            lbl_str3.Text = sessstream;
            lbl_str4.Text = sessstream;

            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
                    stcollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                ddladmis_Selected(sender, e);
                bindGrid2();
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txt_date.Attributes.Add("ReadOnly", "ReadOnly");
                txt_rdate.Attributes.Add("ReadOnly", "ReadOnly");

                txt_colg.Attributes.Add("ReadOnly", "ReadOnly");
                txt_strm.Attributes.Add("ReadOnly", "ReadOnly");
                txt_batch.Attributes.Add("ReadOnly", "ReadOnly");
                txt_degree.Attributes.Add("ReadOnly", "ReadOnly");
                txt_dept.Attributes.Add("ReadOnly", "ReadOnly");
                txt_sem.Attributes.Add("ReadOnly", "ReadOnly");
                txt_sec.Attributes.Add("ReadOnly", "ReadOnly");
                txt_seattype.Attributes.Add("ReadOnly", "ReadOnly");

                txt_colg1.Attributes.Add("ReadOnly", "ReadOnly");
                txt_strm1.Attributes.Add("ReadOnly", "ReadOnly");
                txt_batch1.Attributes.Add("ReadOnly", "ReadOnly");
                txt_degree1.Attributes.Add("ReadOnly", "ReadOnly");
                txt_dept1.Attributes.Add("ReadOnly", "ReadOnly");
                txt_sem1.Attributes.Add("ReadOnly", "ReadOnly");
                txt_sec1.Attributes.Add("ReadOnly", "ReadOnly");
                txt_seat_type1.Attributes.Add("ReadOnly", "ReadOnly");

                txt_recolg.Attributes.Add("ReadOnly", "ReadOnly");
                txt_restrm.Attributes.Add("ReadOnly", "ReadOnly");
                txt_rebatch.Attributes.Add("ReadOnly", "ReadOnly");
                txt_redegree.Attributes.Add("ReadOnly", "ReadOnly");
                txt_redept.Attributes.Add("ReadOnly", "ReadOnly");
                txt_resem.Attributes.Add("ReadOnly", "ReadOnly");
                txt_resec.Attributes.Add("ReadOnly", "ReadOnly");
                studhash.Clear();
                bindclg();
                bindBtch();
                bindstream();
                binddeg();
                binddept();
                bindsem();
                bindsect();
                bindSeat();
                todivnotAdmit.Visible = false;
                bindHeader();
                bindLedger();
                bindHeaderRe();
                bindLedgerRe();
                rbl_EnrollRefund.Visible = false;
                tdadmis.Visible = true;
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
                stcollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
          //  transferReceipt("Journal", "17086", "14", "13/06/2017", "JN0002");
        }
        catch(Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }

    public void loadcollege()
    {
        ddlcollege.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollege);
    }

    protected void ddlcollege_indexChanged(object sender, EventArgs e)
    {
        cleargridview1();
        cleargridview2();
        if (ddlcollege.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            stcollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
        lnkindivmap.Enabled = false;
        // btn_transfer.Enabled = false;
        rbl_AdmitTransfer.SelectedIndex = 0;
        rbl_AdmitTransfer_OnSelectedIndexChanged(sender, e);
    }

    protected void rb_transfer_Change(object sender, EventArgs e)
    {
        txt_roll_no.Text = string.Empty;
        div_transfermulti.Visible = false;
        rbl_AdmitTransfer.Visible = true;
        rbl_TranSngMul.Visible = true;
        rbl_AdmitTransfer.SelectedIndex = 0;
        rbl_TranSngMul.SelectedIndex = 0;
        div_transfer.Visible = true;
        div_refund.Visible = false;
        rbl_AdmitTransfer_OnSelectedIndexChanged(sender, e);
        rbl_TranSngMul_OnSelectedIndexChanged(sender, e);
        txt_roll.Text = "";
        txt_roll1.Text = "";
        txt_roll_no1.Text = "";
        rbl_EnrollRefund.Visible = false;
        loadfromsetting();
        tdadmis.Visible = true;
        lnkindivmap.Visible = true;
        lnkindivmap.Enabled = false;
        trnsledgmap.Visible = true;
        tblbtmhd.Visible = false;
        div_gridView1.Visible = false;
        div_gridView2.Visible = false;
        cbwithoutfees.Checked = false;
        cbdisWithoutFees.Visible = false;
    }
    protected void rb_refund_Change(object sender, EventArgs e)
    {
        rbl_EnrollRefund.Visible = true;
        rbl_EnrollRefund.SelectedIndex = 0;
        rbl_AdmitTransfer.Visible = false;
        rbl_TranSngMul.Visible = false;
        div_transfer.Visible = false;
        div_transfermulti.Visible = false;
        div_refund.Visible = true;
        txt_AmtPerc.Text = "";
        txt_rerollno.Text = "";
        txt_rerollno_TextChanged(sender, e);
        loadrefundsetting();
        btn_refund.Text = "Refund";
        tdadmis.Visible = false;
        lnkindivmap.Visible = false;
        trnsledgmap.Visible = false;
        rbl_EnrollRefund_OnSelectedIndexChanged(sender, e);
        cbdisWithoutFees.Visible = false;
    }
    protected void rb_discont_Change(object sender, EventArgs e)
    {
        rbl_EnrollRefund.Visible = false;
        chk_refCommon.Visible = false;
        //rbl_EnrollRefund.SelectedIndex = 0;
        rbl_AdmitTransfer.Visible = false;
        rbl_TranSngMul.Visible = false;
        div_transfer.Visible = false;
        div_transfermulti.Visible = false;
        div_refund.Visible = true;
        txt_AmtPerc.Text = "";
        txt_rerollno.Text = "";
        txt_rerollno_TextChanged(sender, e);
        loadrefundsetting();
        btn_refund.Text = "Discontinue";
        tdadmis.Visible = false;
        lnkindivmap.Visible = false;
        trnsledgmap.Visible = false;
        admis = 2;
        cbdisWithoutFees.Visible = true;
    }

    //applied and not applied
    protected void rbl_AdmitTransfer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_roll_no.Text = "";
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                todivAdmit.Visible = true;
                todivnotAdmit.Visible = false;

            }
            else
            {
                todivAdmit.Visible = false;
                todivnotAdmit.Visible = true;

            }
            txt_roll1.Text = "";
            txt_roll1_TextChanged(sender, e);
            getAdmissionNo();
            txt_roll_no1.Text = "";

            bindFromGrid();
            bindApplideNotGrid1();
            //cleargridview1();
            //cleargridview2();          
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }

    protected void rbl_TranSngMul_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbl_TranSngMul.SelectedIndex == 0)
            {
                div_transfer.Visible = true;
                div_transfermulti.Visible = false;
                tdadmis.Visible = true;
            }
            else
            {
                div_transfer.Visible = false;
                div_transfermulti.Visible = true;
                bindclg1();
                bindType1();
                bindbatch1();
                binddegree1();
                bindbranch1();
                bindsem1();

                btn_TransferMulti.Visible = false;
                tblToTransMulti.Visible = false;

                spreadStudAdd.Visible = false;
                lbl_Total1.Visible = false;
                btn_go1_Click(sender, e);

                bindCollege();
                bindType();
                bindbatch();
                binddegree();
                bindbranch();
                bindseme();
                tdadmis.Visible = false;

            }

        }
        catch (Exception ex) { }
    }
    protected void rbl_EnrollRefund_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_AmtPerc.Text = "";
        if (rbl_EnrollRefund.SelectedItem.Text.Trim() == "Enrolled")
        {
            admis = 2;
            loadrefundsetting();
        }
        else
        {
            loadEnorllapp();
        }
    }
    protected void loadEnorllapp()
    {
        try
        {
            rbl_rerollno.Items.Clear();
            ListItem lst = new ListItem("App No", "3");
            rbl_rerollno.Items.Add(lst);
            txt_rerollno.Attributes.Add("placeholder", "App No");
            personmode = 0;
            chosedmode = 4;
            admis = 1;
        }
        catch { }
    }


    public void bindFromGrid()
    {
        string app_no = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("paymode");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");

        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
            app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedItem.Value + "'");
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
            app_no = d2.GetFunction("select app_no from Registration where Reg_no='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedItem.Value + "'");
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
            app_no = d2.GetFunction("select app_no from Registration where Roll_admit='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedItem.Value + "'");
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
            app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedItem.Value + "'");
        if (app_no != "0")
        {
            string selectQ = "";
            if (ddladmis.SelectedItem.Text.Trim() != "Before Admission")
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode  from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code  and r.App_No=" + app_no + " and r.college_code='" + ddlcollege.SelectedItem.Value + "'  order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
            }
            else
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode  from FT_FeeAllot f,applyn R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code  and r.App_No=" + app_no + " and r.college_code='" + ddlcollege.SelectedItem.Value + "' order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");
            //if (ddlcollege.Items.Count > 0)
            //{
            //    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            //    collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            //}
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                    string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + ddlcollege.SelectedItem.Value + "");
                    dr = dt.NewRow();
                    dr["Sno"] = row + 1;
                    dr["paymode"] = Convert.ToString(ds.Tables[0].Rows[row]["paymode"]);
                    dr["YearSem"] = cursem;
                    dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                    dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                    dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                    dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                    dr["FeeCategory"] = feecat;
                    dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                    dr["Paid"] = Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]);
                    dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                    dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                    dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                    dt.Rows.Add(dr);
                    total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                    balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                    paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);
                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            gridView1.DataSource = dt;
            gridView1.DataBind();
            lbl_grid1_bal.Text = "Rs." + balance.ToString();
            lbl_grid1_paid.Text = "Rs." + paid.ToString();
            lbl_grid1_tot.Text = "Rs." + total.ToString();
            tblgrid1.Visible = true;
            gridView1.Visible = true;
            div_gridView1.Visible = true;
            tblbtmhd.Visible = true;
            if (!cbwithoutfees.Checked)
            {
                if (gridView2.Rows.Count > 0)
                    lnkindivmap.Enabled = true;
            }
            else
            {
                lnkindivmap.Enabled = false;
                gridView1.Visible = false;
                tblgrid1.Visible = false;
                div_gridView1.Visible = false;
                tblbtmhd.Visible = false;
            }
        }
        else
        {
            gridView1.DataSource = null;
            gridView1.DataBind();
            tblgrid1.Visible = false;
            div_gridView1.Visible = false;
            tblbtmhd.Visible = false;
        }
    }

    public void bindApplideNotGrid1()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");

        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;
        string clgcode = "";
        string selectQ = "";
        string stream = "";
        string batch = "";
        string degreeCode = "";
        string dept = "";
        string feecategory = "";
        string section = "";
        if (rb_transfer.Checked)
        {
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                if (txt_roll1.Text.Trim() != "")
                {
                    stream = txt_strm1.Text.Trim();
                    batch = txt_batch1.Text.Trim();
                    degreeCode = lblDegCode.Text;
                    dept = "";
                    feecategory = "";
                    section = "";
                    string SndSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlcollege.SelectedValue + "'  and textval='" + txt_seat_type1.Text.Trim() + "'"));
                    //if (ddlcollege.Items.Count > 0)
                    //{
                    //    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    //    collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
                    //}
                    selectQ = "  select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount  from FT_FeeAllotDegree f,FM_HeaderMaster H,FM_LedgerMaster L where  F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and  L.CollegeCode=H.CollegeCode and L.CollegeCode=" + ddlcollege.SelectedItem.Value + " and F.BatchYear=" + batch + " and F.DegreeCode=" + degreeCode + " ";
                    if (SndSeatCode != "0")
                        selectQ += " and seattype='" + SndSeatCode + "' ";
                    if (stream != "")
                        selectQ += " ";
                    selectQ += " order by isnull(l.priority,1000), l.ledgerName asc";
                }
            }
            else
            {
                if (ddl_colg.Items.Count > 0)
                {
                    clgcode = Convert.ToString(ddl_colg.SelectedItem.Value);
                    if (ddl_strm.Items.Count > 0)
                        stream = Convert.ToString(ddl_strm.SelectedItem.Value);
                    if (ddl_batch.Items.Count > 0)
                        batch = Convert.ToString(ddl_batch.SelectedItem.Value);
                    if (ddl_degree.Items.Count > 0)
                        degreeCode = Convert.ToString(ddl_degree.SelectedItem.Value);
                    if (ddl_dept.Items.Count > 0)
                        dept = Convert.ToString(ddl_dept.SelectedItem.Value);
                    if (ddl_sem.Items.Count > 0)
                        feecategory = Convert.ToString(ddl_sem.SelectedItem.Value);
                    if (ddl_sec.Items.Count > 0)
                        section = Convert.ToString(ddl_sec.SelectedItem.Value);
                    if (ddl_seattype.Items.Count > 0)
                        seatype = Convert.ToString(ddl_seattype.SelectedValue);

                    selectQ = "  select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount   from FT_FeeAllotDegree f,FM_HeaderMaster H,FM_LedgerMaster L where  F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and  L.CollegeCode=H.CollegeCode and L.CollegeCode=" + ddl_colg.SelectedItem.Value + " and F.BatchYear=" + batch + " and F.DegreeCode=" + dept + " and seattype='" + seatype + "' ";
                    if (section != "")
                        selectQ += " ";
                    if (stream != "")
                        selectQ += " ";
                    selectQ += " order by isnull(l.priority,1000), l.ledgerName asc";
                    // order by  f.HeaderFK,f.LedgerFK
                }
            }

        }
        if (selectQ != "")
        {
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");
            if (ds.Tables.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                    string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + ddl_colg.SelectedItem.Value + "");
                    dr = dt.NewRow();
                    dr["Sno"] = row + 1;
                    dr["YearSem"] = cursem;
                    dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                    dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                    dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                    dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                    dr["FeeCategory"] = feecat;
                    dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                    dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                    dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                    dt.Rows.Add(dr);
                    total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);

                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            gridView2.DataSource = dt;
            gridView2.DataBind();
            lbl_grid2_bal.Text = "Rs." + balance.ToString();
            lbl_grid2_paid.Text = "Rs." + paid.ToString();
            lbl_grid2_tot.Text = "Rs." + total.ToString();
            tblgrid2.Visible = true;
            gridView2.Visible = true;
            div_gridView2.Visible = true;
            tblbtmhd.Visible = true;
            if (!cbwithoutfees.Checked)
                lnkindivmap.Enabled = true;
            else
            {
                lnkindivmap.Enabled = false;
                tblgrid2.Visible = false;
                gridView2.Visible = false;
                div_gridView2.Visible = false;
                tblbtmhd.Visible = false;
            }
        }
        else
        {
            gridView2.DataSource = null;
            gridView2.DataBind();
            tblgrid2.Visible = false;
            div_gridView2.Visible = false;
            tblbtmhd.Visible = false;
        }
        if (cbledgmapp.Checked == false)
        {
            #region After GridBind

            double balOvall = 0;
            double paidOvall = 0;
            double excessovall = 0;
            double extraexces = 0;
            double expaid = 0;
            int exceflg1 = 0;
            int exceflg = 0;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (GridViewRow row1 in gridView1.Rows)
            {
                Label hdrid1 = (Label)row1.Cells[1].FindControl("lbl_hdrid");
                Label lgrid1 = (Label)row1.Cells[1].FindControl("lbl_lgrid");
                Label feecat1 = (Label)row1.Cells[1].FindControl("lbl_feecat");
                Label concession1 = (Label)row1.Cells[1].FindControl("lbl_Concess");
                Label paid1 = (Label)row1.Cells[1].FindControl("lbl_paid");

                foreach (GridViewRow row in gridView2.Rows)
                {
                    Label hdrid = (Label)row.Cells[1].FindControl("lbl_hdrid");
                    Label lgrid = (Label)row.Cells[1].FindControl("lbl_lgrid");
                    Label feecat = (Label)row.Cells[1].FindControl("lbl_feecat");
                    if (hdrid1.Text == hdrid.Text && lgrid1.Text == lgrid.Text && feecat1.Text == feecat.Text)
                    {
                        Label feeamt = (Label)row.Cells[1].FindControl("lbl_feeamt");
                        Label totamt = (Label)row.Cells[1].FindControl("lbl_totamt");
                        Label concession = (Label)row.Cells[1].FindControl("lbl_Concess");
                        TextBox txtpaid = (TextBox)row.Cells[1].FindControl("txt_paid");
                        TextBox txtbalance = (TextBox)row.Cells[1].FindControl("txt_bal");
                        TextBox excess = (TextBox)row.Cells[1].FindControl("txt_exGrid2");

                        concession.Text = concession1.Text;
                        double totamont = Convert.ToDouble(totamt.Text);
                        string totalamt = paid1.Text;
                        string paidamt = "";
                        double exce = 0;
                        if (totamont >= Convert.ToDouble(totalamt))
                        {
                            txtpaid.Text = Convert.ToString(totalamt);
                            exce = Convert.ToDouble(totalamt);
                        }
                        else
                        {
                            txtpaid.Text = Convert.ToString(totamont);
                            excess.Text = (Convert.ToDouble(totalamt) - Convert.ToDouble(totamont)).ToString();

                        }
                        paidamt = Convert.ToString(txtpaid.Text);
                        if (totamont != 0 && paidamt != "0")
                        {
                            if (totamont >= Convert.ToDouble(paidamt))
                            {
                                txtbalance.Text = (Convert.ToDouble(totamt.Text) - Convert.ToDouble(txtpaid.Text)).ToString();
                            }
                        }
                        //else
                        //{
                        //    txtbalance.Text = (Convert.ToDouble(totamt.Text)).ToString();
                        //}                    
                        if (txtpaid.Text.Trim() != "")
                        {
                            paidOvall += Convert.ToDouble(txtpaid.Text);
                        }
                        if (txtbalance.Text.Trim() != "")
                        {
                            balOvall += Convert.ToDouble(txtbalance.Text);
                        }
                        if (excess.Text.Trim() != "")
                        {
                            excessovall += Convert.ToDouble(excess.Text);
                        }
                        exceflg = 2;
                    }
                    else
                    {
                        exceflg1 = -1;
                    }
                }
                if (exceflg1 == -1 && exceflg != 2)
                {
                    string hedfk = Convert.ToString(hdrid1.Text);
                    string ledfk = Convert.ToString(lgrid1.Text);
                    string fnlfk = hedfk + "-" + ledfk;
                    double.TryParse(Convert.ToString(paid1.Text), out expaid);
                    extraexces += expaid;
                    if (!dict.ContainsKey(Convert.ToString(fnlfk)))
                    {
                        dict.Add(Convert.ToString(fnlfk), Convert.ToString(expaid));
                    }

                }
                exceflg = 0;
                exceflg1 = 0;
            }
            lbl_grid2_paid.Text = "Rs." + paidOvall.ToString();
            if (lbl_grid2_paid.Text.Trim() != "" && lbl_grid2_tot.Text.Trim() != "")
            {

                double tot = total;
                double paids = paidOvall;
                //lbl_grid2_bal.Text = "Rs."+(Convert.ToDouble(lbl_grid2_tot.Text) - Convert.ToDouble(lbl_grid2_paid.Text)).ToString();
                lbl_grid2_bal.Text = "Rs." + (Convert.ToDouble(total) - Convert.ToDouble(paidOvall)).ToString();
            }
            lbl_grid2_excess.Text = "Rs." + excessovall.ToString();
            if (extraexces != 0)
            {
                lblunmtexcess.Text = "Rs." + Convert.ToString(extraexces);
                Session["excess"] = dict;
            }
            #endregion
        }
        else
        {
            #region After GridBind
            //if (Session["clgcode"] != null)
            //    clgcode = Convert.ToString(Session["clgcode"]);
            //else
            //    clgcode = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            }
            double balOvall = 0;
            double paidOvall = 0;
            double excessovall = 0;
            double extraexces = 0;
            double expaid = 0;
            int exceflg1 = 0;
            int exceflg = 0;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (GridViewRow row1 in gridView1.Rows)
            {
                Label hdrid1 = (Label)row1.Cells[1].FindControl("lbl_hdrid");
                Label lgrid1 = (Label)row1.Cells[1].FindControl("lbl_lgrid");
                Label feecat1 = (Label)row1.Cells[1].FindControl("lbl_feecat");
                Label concession1 = (Label)row1.Cells[1].FindControl("lbl_Concess");
                Label paid1 = (Label)row1.Cells[1].FindControl("lbl_paid");

                string Selid = d2.GetFunction(" select MatchingLedger from fm_ledgermaster where ledgerpk='" + lgrid1.Text + "' ");//and collegecode='" + ddl_colg.SelectedItem.Value + "'
                if (Selid != "0" && Selid != "")
                {
                    string[] ledgid = Selid.Split(',');
                    if (ledgid.Length > 0)
                    {
                        string totalamt = paid1.Text;
                        for (int rw = 0; rw < ledgid.Length; rw++)
                        {
                            string ledgerid = ledgid[rw].ToString();

                            foreach (GridViewRow row in gridView2.Rows)
                            {
                                Label hdrid = (Label)row.Cells[1].FindControl("lbl_hdrid");
                                Label lgrid = (Label)row.Cells[1].FindControl("lbl_lgrid");
                                Label feecat = (Label)row.Cells[1].FindControl("lbl_feecat");

                                if (ledgerid == lgrid.Text && feecat1.Text == feecat.Text)
                                {
                                    Label feeamt = (Label)row.Cells[1].FindControl("lbl_feeamt");
                                    Label totamt = (Label)row.Cells[1].FindControl("lbl_totamt");
                                    Label concession = (Label)row.Cells[1].FindControl("lbl_Concess");
                                    TextBox txtpaid = (TextBox)row.Cells[1].FindControl("txt_paid");
                                    TextBox txtbalance = (TextBox)row.Cells[1].FindControl("txt_bal");
                                    TextBox excess = (TextBox)row.Cells[1].FindControl("txt_exGrid2");

                                    concession.Text = concession1.Text;
                                    double totamont = Convert.ToDouble(totamt.Text);

                                    string paidamt = "";
                                    double exce = 0;
                                    if (totamont >= Convert.ToDouble(totalamt))
                                    {
                                        txtpaid.Text = Convert.ToString(totalamt);
                                        exce = Convert.ToDouble(totalamt);
                                    }
                                    else
                                    {
                                        txtpaid.Text = Convert.ToString(totamont);
                                        totalamt = (Convert.ToDouble(totalamt) - Convert.ToDouble(totamont)).ToString();
                                        // excess.Text = (Convert.ToDouble(totalamt) - Convert.ToDouble(totamont)).ToString();

                                    }
                                    paidamt = Convert.ToString(txtpaid.Text);
                                    if (totamont != 0 && paidamt != "0")
                                    {
                                        if (totamont >= Convert.ToDouble(paidamt))
                                        {
                                            txtbalance.Text = (Convert.ToDouble(totamt.Text) - Convert.ToDouble(txtpaid.Text)).ToString();
                                        }
                                    }
                                    if (txtpaid.Text.Trim() != "")
                                    {
                                        paidOvall += Convert.ToDouble(txtpaid.Text);
                                    }
                                    if (txtbalance.Text.Trim() != "")
                                    {
                                        balOvall += Convert.ToDouble(txtbalance.Text);
                                    }
                                    exceflg = 2;
                                }
                                else
                                {
                                    exceflg1 = -1;
                                }
                            }
                            if (exceflg1 == -1 && exceflg != 2)
                            {
                                string hedfk = Convert.ToString(hdrid1.Text);
                                string ledfk = Convert.ToString(lgrid1.Text);
                                string fnlfk = hedfk + "-" + ledfk;
                                double.TryParse(Convert.ToString(paid1.Text), out expaid);
                                extraexces += expaid;
                                if (!dict.ContainsKey(Convert.ToString(fnlfk)))
                                {
                                    dict.Add(Convert.ToString(fnlfk), Convert.ToString(expaid));
                                }

                            }
                            exceflg = 0;
                            exceflg1 = 0;
                        }
                        excessovall += Convert.ToDouble(totalamt);
                    }
                }
                //if (exceflg1 == -1 && exceflg != 2)
                else
                {
                    string hedfk = Convert.ToString(hdrid1.Text);
                    string ledfk = Convert.ToString(lgrid1.Text);
                    string fnlfk = hedfk + "-" + ledfk;
                    double.TryParse(Convert.ToString(paid1.Text), out expaid);
                    extraexces += expaid;
                    if (!dict.ContainsKey(Convert.ToString(fnlfk)))
                    {
                        dict.Add(Convert.ToString(fnlfk), Convert.ToString(expaid));
                    }

                }
            }
            lbl_grid2_paid.Text = "Rs." + paidOvall.ToString();
            if (lbl_grid2_paid.Text.Trim() != "" && lbl_grid2_tot.Text.Trim() != "")
            {

                double tot = total;
                double paids = paidOvall;
                lbl_grid2_bal.Text = "Rs." + (Convert.ToDouble(total) - Convert.ToDouble(paidOvall)).ToString();
            }
            lbl_grid2_excess.Text = "Rs." + excessovall.ToString();
            if (extraexces != 0)
            {
                lblunmtexcess.Text = "Rs." + Convert.ToString(extraexces);
                Session["excess"] = dict;
            }
            #endregion
        }
    }

    public void bindGrid2()
    {
        string app_no = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");
        dt.Columns.Add("RefundAmt");
        if (ddlcollege.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
        }
        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;
        if (txt_rerollno.Text.Trim() != "")
        {
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
            {
                app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
            }
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
            {
                app_no = d2.GetFunction("select app_no from Registration where Reg_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
            }
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
            {
                app_no = d2.GetFunction("select app_no from Registration where Roll_admit='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
            }
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
            {
                app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
            }

        }
        if (app_no != "")
        {
            string selectQ = "";
            if (rb_refund.Checked == true)
            {
                if (rbl_EnrollRefund.SelectedItem.Text == "Enrolled")
                {

                    selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 )   and r.App_No=" + app_no + " order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
                    // order by F.FeeCategory
                }
                else
                {
                    selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount   from FT_FeeAllot f,applyn R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 )   and r.App_No=" + app_no + " order by isnull(l.priority,1000), l.ledgerName asc, F.FeeCategory";
                }
            }
            else
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 )   and r.App_No=" + app_no + " order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");

            //if (Session["clgcode"] != null)
            //    collegecode1 = Convert.ToString(Session["clgcode"]);
            //else
            //    collegecode1 = Convert.ToString(Session["collegecode"]);

            if (ds.Tables.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                    string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + collegecode1 + "");
                    dr = dt.NewRow();
                    dr["Sno"] = row + 1;
                    dr["YearSem"] = cursem;
                    dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                    dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                    dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                    dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                    dr["FeeCategory"] = feecat;
                    dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                    dr["Paid"] = Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]);
                    dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                    dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                    dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                    dr["RefundAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["refundamount"]);
                    dt.Rows.Add(dr);

                    total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                    balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                    paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);
                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            gridView3.DataSource = dt;
            gridView3.DataBind();
            lbl_grid3_bal.Text = "Rs." + balance.ToString();
            lbl_grid3_paid.Text = "Rs." + paid.ToString();
            lbl_grid3_tot.Text = "Rs." + total.ToString();
            tblgrid3.Visible = true;
            gridView3.Visible = true;
        }
        else
        {
            gridView3.DataSource = null;
            gridView3.DataBind();
            tblgrid3.Visible = false;
            gridView3.Visible = false;
        }
        if (gridView3.Rows.Count > 0)
        {
            foreach (GridViewRow rows in gridView3.Rows)
            {
                TextBox txtAmt = (TextBox)rows.Cells[9].FindControl("txt_refund");
                if (ddl_AmtPerc.SelectedIndex == 0)
                {
                    txtAmt.ReadOnly = false;
                }
                else
                {
                    txtAmt.ReadOnly = false;
                }
            }
        }
    }


    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static List<string> Getrno(string prefixText)
    //{
    //    WebService ws = new WebService();
    //    List<string> name = new List<string>();
    //    string query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
    //    name = ws.Getname(query);
    //    return name;
    //}
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
                //and (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0)
                if (chosedmode == 0)
                {
                    query = "select top 100 Roll_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_No like '" + prefixText + "%' and r.college_code='" + stcollegecode + "' order by  Roll_No asc";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Reg_No like '" + prefixText + "%' and r.college_code='" + stcollegecode + "' order by  Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_admit like '" + prefixText + "%' and r.college_code='" + stcollegecode + "' order by  Roll_admit asc";
                }
                else
                {
                    if (admis == 2)
                    {
                        query = "  select  top 100 app_formno from applyn a ,Registration r where a.app_no=r.App_No and admission_status =1 and selection_status=1 and isconfirm ='1' and DelFlag =0 and app_formno like '" + prefixText + "%' and r.college_code='" + stcollegecode + "' order by  app_formno asc";
                    }
                    else
                    {
                        query = "  select  top 100 app_formno from applyn where isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code='" + stcollegecode + "' order by  app_formno asc";
                    }
                }
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
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetAppFormno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select top 100 app_formno,app_no from applyn where  app_formno like '" + prefixText + "%' and  isconfirm='1' and isnull(admission_status,'0')='0'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";

        studhash = ws.Getnamevalue(query);
        if (studhash.Count > 0)
        {
            foreach (DictionaryEntry p in studhash)
            {
                string studname = Convert.ToString(p.Key);
                name.Add(studname);
            }
        }
        // name = ws.Getname(query);
        return name;
    }


    public void txt_rerollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_reamt.Text = "";
            string rollno = Convert.ToString(txt_rerollno.Text);
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            }
            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and r.college_code='" + collegecode1 + "' ";
            //and r.Roll_no='" + rollno + "'";
            if (rollno != "" && rollno != null)
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    if (rb_refund.Checked == false)
                    {
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                            query = query + "and r.Roll_no='" + rollno + "'  and DelFlag =0 ";
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                            query = query + "and r.Reg_No='" + rollno + "' and  DelFlag =0";
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                            query = query + "and r.Roll_Admit='" + rollno + "' and DelFlag =0 ";

                    }
                    if (rb_refund.Checked == true)
                    {
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                            query = query + "and r.Roll_no='" + rollno + "'  --and DelFlag =0 ";
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                            query = query + "and r.Reg_No='" + rollno + "' --and  DelFlag =0";
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                            query = query + "and r.Roll_Admit='" + rollno + "' --and DelFlag =0 ";
                    }
                }
                else
                {
                    if (rb_refund.Checked == true)
                    {
                        if (rbl_EnrollRefund.SelectedItem.Text == "Enrolled")
                        {
                            query = "select a.batch_year,a.Current_Semester,a.parent_name,r.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code from applyn a,Registration r,Degree d,Department dt,Course c,collinfo co where a.app_no=r.App_No  and  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code and admission_status =1 and selection_status=1 and isconfirm ='1' and app_formno = '" + rollno + "' and r.college_code='" + collegecode1 + "'";
                        }
                        else
                        {
                            query = "  select a.batch_year,a.Current_Semester,a.parent_name,a.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code from applyn a,Degree d,Department dt,Course c,collinfo co where  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code and  isconfirm ='1' and app_formno = '" + rollno + "' and a.college_code='" + collegecode1 + "'";
                        }
                    }
                    else
                    {
                        query = "select a.batch_year,a.Current_Semester,a.parent_name,r.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code from applyn a,Registration r,Degree d,Department dt,Course c,collinfo co where a.app_no=r.App_No  and  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code and admission_status =1 and selection_status=1 and isconfirm ='1' and app_formno = '" + rollno + "' and r.college_code='" + collegecode1 + "'";
                    }
                }

                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        // txt_rerollno.Text = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                        txt_rename.Text = ds1.Tables[0].Rows[i]["stud_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_rebatch.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_redegree.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_redept.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        //  txt_resec.Text = ds1.Tables[0].Rows[i]["Sections"].ToString();
                        txt_resem.Text = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_recolg.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                        txt_restrm.Text = ds1.Tables[0].Rows[i]["type"].ToString(); // jairam
                        Session["clgcode"] = Convert.ToString(ds1.Tables[0].Rows[i]["college_code"]);
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "' and college_code='" + collegecode1 + "' ");
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "' and college_code='" + collegecode1 + "'");
                    image3.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;

                }
            }
            if (ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0)
            {
                txt_rerollno.Text = "";
                txt_rebatch.Text = "";
                txt_redegree.Text = "";
                txt_redept.Text = "";
                txt_resec.Text = "";
                txt_resem.Text = "";
                txt_recolg.Text = "";
                txt_restrm.Text = "";
                txt_rename.Text = "";
                image3.ImageUrl = "";
            }
            txt_AmtPerc.Text = "";
            if (rb_discont.Checked)
            {
                if (cbdisWithoutFees.Checked)
                {
                    gridView3.Visible = false;
                    txt_reamt.Enabled = false;
                    ddl_AmtPerc.Enabled = false;
                    txt_AmtPerc.Enabled = false;
                    tblgrid3.Visible = false;
                }
                else
                {
                    tblgrid3.Visible = true;
                    bindGrid2();
                    txt_reamt.Enabled = true;
                    ddl_AmtPerc.Enabled = true;
                    txt_AmtPerc.Enabled = true;
                }
            }
            else
            {
                tblgrid3.Visible = true;
                bindGrid2();
                txt_reamt.Enabled = true;
                ddl_AmtPerc.Enabled = true;
                txt_AmtPerc.Enabled = true;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }

    public void txt_roll_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string rollno = Convert.ToString(txt_roll.Text);
            string cursem = "";
            if (!string.IsNullOrEmpty(rollno))
            {
                string query = "";
                if (ddladmis.SelectedItem.Text.Trim() != "Before Admission")
                {
                    query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,seattype,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type,r.degree_code   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "'";
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                    {
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                            query = query + "and r.Roll_no='" + rollno + "' ";
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                            query = query + "and r.Reg_No='" + rollno + "' ";
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                            query = query + "and r.Roll_Admit='" + rollno + "' ";
                    }
                    else
                    {
                        query = "select a.batch_year,a.Current_Semester,a.parent_name,r.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code,seattype,''Sections,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type,r.degree_code  from applyn a,Registration r,Degree d,Department dt,Course c,collinfo co where a.app_no=r.App_No and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code and admission_status =0   and isconfirm ='1' and app_formno = '" + rollno + "' and d.college_code='" + ddlcollege.SelectedItem.Value + "'";
                    }
                }
                else
                {
                    query = "select a.batch_year,a.Current_Semester,a.parent_name,a.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code,seattype,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type,''Sections,a.degree_code from applyn a,Degree d,Department dt,Course c,collinfo co where   a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code   and isconfirm ='1' and app_formno = '" + rollno + "' and d.college_code='" + ddlcollege.SelectedItem.Value + "'";
                }
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        txt_name.Text = ds1.Tables[0].Rows[i]["stud_name"].ToString();
                        txt_batch.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_degree.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_dept.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_sec.Text = ds1.Tables[0].Rows[i]["Sections"].ToString();
                        txt_seattype.Text = ds1.Tables[0].Rows[i]["Seat_Type"].ToString();
                        txt_sem.Text = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        cursem = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_colg.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                        txt_strm.Text = ds1.Tables[0].Rows[i]["type"].ToString();
                        seatype = ds1.Tables[0].Rows[i]["seattype"].ToString();
                        Session["seatype"] = seatype;
                        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        //                       
                        lbltempfstclg.Text = ds1.Tables[0].Rows[i]["college_code"].ToString();
                        lbltempfstdeg.Text = ds1.Tables[0].Rows[i]["degree_code"].ToString();
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "' and college_code='" + ddlcollege.SelectedItem.Value + "'");
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "' and college_code='" + ddlcollege.SelectedItem.Value + "'");
                    image2.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;
                    if (!string.IsNullOrEmpty(cursem) && cursem == "1")
                    {
                        if (!cbwithoutfees.Checked)
                            bindFromGrid();
                        else
                            div_gridView1.Visible = false;
                        txt_roll1.Text = "";
                        txt_roll1_TextChanged(sender, e);
                    }
                    else
                    {
                        cleargridview1();
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Can't Transfer";
                        return;
                    }
                }
                else
                    cleargridview1();
            }
            else
                cleargridview1();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }

    protected void cleargridview1()
    {
        txt_roll.Text = "";
        txt_batch.Text = "";
        txt_degree.Text = "";
        txt_dept.Text = "";
        txt_sec.Text = "";
        txt_seattype.Text = "";
        txt_sem.Text = "";
        txt_colg.Text = "";
        txt_strm.Text = "";
        txt_name.Text = "";
        txt_tramt.Text = "";
        image3.ImageUrl = "";
        image2.ImageUrl = "";
        div_gridView1.Visible = false;
        tblbtmhd.Visible = false;
        //
        lbltempfstclg.Text = string.Empty;
        lbltempfstdeg.Text = string.Empty;
        contentDiv.Visible = false;
    }

    public void txt_roll1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string feecatagory = "";
            string rollno = Convert.ToString(txt_roll1.Text);
            if (!string.IsNullOrEmpty(rollno))
            {
                string appno = "";
                string query = "select a.parent_name,a.stud_name, a.Stud_Type,c.Course_Name,dt.Dept_Name,a.degree_code,a.Current_Semester  ,a.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,a.app_no,a.seattype,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type   from applyn a ,Degree d,course c,Department dt,collinfo co where  a.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code  and a.app_formno='" + rollno + "' and d.college_code='" + ddlcollege.SelectedItem.Value + "'";
                ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        txt_batch1.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_degree1.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_dept1.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_sec1.Text = "";// ds1.Tables[0].Rows[i]["Sections"].ToString();
                        txt_sem1.Text = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_seat_type1.Text = ds1.Tables[0].Rows[i]["Seat_Type"].ToString();
                        int fee = Convert.ToInt32(ds1.Tables[0].Rows[i]["Current_Semester"]);
                        string clgName = Convert.ToString(ds1.Tables[0].Rows[i]["collname"]);
                        string clgcode = Convert.ToString(ds1.Tables[0].Rows[i]["college_code"]);
                        txt_colg1.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                        txt_strm1.Text = ds1.Tables[0].Rows[i]["type"].ToString();
                        lblDegCode.Text = ds1.Tables[0].Rows[i]["degree_code"].ToString();
                        appno = ds1.Tables[0].Rows[i]["app_no"].ToString();
                        feecatagory = bindstudsem(fee, clgcode);
                        seatype = ds1.Tables[0].Rows[i]["seattype"].ToString();
                        Session["seatype"] = seatype;
                        if (ddlcollege.Items.Count > 0)
                        {
                            collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                            collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
                        }
                        //
                        lbltempsndclg.Text = ds1.Tables[0].Rows[i]["degree_code"].ToString();
                        lbltempsnddeg.Text = clgcode;
                    }
                }
                if (ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0)
                    cleargridview2();
            }
            else
                cleargridview2();
            if (!cbwithoutfees.Checked)
            {
                bindApplideNotGrid1();
                bindHeader();
                bindLedger();
            }
            else
                div_gridView2.Visible = false;
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode1, "TransferRefund")
        }
    }

    protected void cleargridview2()
    {
        txt_roll1.Text = "";
        txt_batch1.Text = "";
        txt_degree1.Text = "";
        txt_dept1.Text = "";
        txt_sec1.Text = "";
        txt_seat_type1.Text = "";
        txt_sem1.Text = "";
        txt_colg1.Text = "";
        txt_strm1.Text = "";
        lblDegCode.Text = "";
        txt_tramt.Text = "";
        div_gridView2.Visible = false;
        tblbtmhd.Visible = false;
        //
        lbltempsndclg.Text = string.Empty;
        lbltempsnddeg.Text = string.Empty;
        contentDiv.Visible = false;
    }


    protected void ddl_colg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode1 = Convert.ToString(ddl_colg.SelectedItem.Value);
            bindstream();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindSeat();
            bindsect();
            bindFromGrid();
            bindApplideNotGrid1();
            getAdmissionNo();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }

    }



    protected void ddl_strm_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddeg();
        binddept();
        bindsem();
        bindsect();
        bindApplideNotGrid1();
        getAdmissionNo();
    }
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddeg();
            binddept();
            bindsem();
            bindsect();
            bindApplideNotGrid1();
            getAdmissionNo();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void ddl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddept();
            bindsem();
            bindsect();
            bindApplideNotGrid1();
            getAdmissionNo();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        bindsect();
        bindApplideNotGrid1();
        getAdmissionNo();
    }
    protected void ddl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsect();
        bindApplideNotGrid1();
    }
    protected void ddl_seattype_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindApplideNotGrid1();
        getAdmissionNo();
    }
    protected void ddl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindApplideNotGrid1();
    }
    public void bindclg()
    {
        try
        {
            ddl_colg.Items.Clear();
            reuse.bindCollegeToDropDown(usercode, ddl_colg);
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsect();
            bindstream();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void bindBtch()
    {
        try
        {
            ddl_batch.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
            binddeg();
            binddept();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void binddeg()
    {
        try
        {
            ddl_degree.Items.Clear();

            batch = "";
            if (ddl_colg.Items.Count > 0 && ddl_batch.Items.Count > 0)
            {
                batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
                string stream = "";
                stream = Convert.ToString(ddl_strm.SelectedValue.ToString());
                if (batch != "")
                {
                    ds.Clear();

                    string sel = " select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + Convert.ToString(ddl_colg.SelectedValue) + "')  ";
                    if (stream != "")
                    {
                        sel = sel + "  and type in ('" + stream + "')";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sel, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_degree.DataSource = ds;
                        ddl_degree.DataTextField = "course_name";
                        ddl_degree.DataValueField = "course_id";
                        ddl_degree.DataBind();
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void binddept()
    {
        try
        {
            ddl_dept.Items.Clear();
            degree = "";
            if (ddl_degree.Items.Count > 0 && ddl_colg.Items.Count > 0)
            {
                degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

                if (degree != "")
                {
                    //ds.Clear();
                    //ds = d2.BindBranchMultiple(singleuser, group_user, degree, ddl_colg.SelectedItem.Value, usercode);
                    string sel = " select dt.Dept_Name,d.degree_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.Course_Id in('" + degree + "') and d.college_code in('" + ddl_colg.SelectedItem.Value + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sel, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_dept.DataSource = ds;
                        ddl_dept.DataTextField = "dept_name";
                        ddl_dept.DataValueField = "degree_code";
                        ddl_dept.DataBind();

                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void bindsem()
    {
        try
        {
            if (ddl_colg.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_dept.Items.Count > 0)
            {
                DataSet ds3 = new DataSet();
                ddl_sem.Items.Clear();
                Boolean first_year;
                first_year = false;
                int duration = 0;
                int i = 0;


                string sqluery = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code= (" + ddl_dept.SelectedValue.ToString() + ") and batch_year  = (" + ddl_batch.SelectedValue.ToString() + ") and college_code=" + ddl_colg.SelectedValue.ToString() + "";

                ds3 = d2.select_method_wo_parameter(sqluery, "text");
                if (ds3.Tables.Count > 0)
                {
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                        duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["ndurations"]);
                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddl_sem.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddl_sem.Items.Add(i.ToString());
                            }

                        }
                    }
                    else
                    {
                        sqluery = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + ddl_dept.SelectedValue.ToString() + ") and college_code=" + ddl_colg.SelectedValue.ToString() + "";
                        ddl_sem.Items.Clear();
                        ds3 = d2.select_method_wo_parameter(sqluery, "text");
                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                            duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["duration"]);
                            for (i = 1; i <= duration; i++)
                            {
                                if (first_year == false)
                                {

                                    ddl_sem.Items.Add(i.ToString());
                                }
                                else if (first_year == true && i != 2)
                                {

                                    ddl_sem.Items.Add(i.ToString());
                                }
                            }
                        }
                    }
                }
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void bindSeat()
    {
        ddl_seattype.Items.Clear();
        try
        {
            if (ddl_colg.Items.Count > 0)
            {
                DataSet dsSeat = new DataSet();
                dsSeat = d2.select_method_wo_parameter("select TextVal,Textcode from TextValTable where textcriteria='seat' and college_code='" + ddl_colg.SelectedValue + "' order by Textval asc", "Text");
                if (dsSeat.Tables.Count > 0 && dsSeat.Tables[0].Rows.Count > 0)
                {
                    ddl_seattype.DataSource = dsSeat;
                    ddl_seattype.DataTextField = "TextVal";
                    ddl_seattype.DataValueField = "Textcode";
                    ddl_seattype.DataBind();
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public string bindstudsem(int semester, string college)
    {
        string semesterquery = "";

        string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + college + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(settingquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            if (linkvalue == "0")
            {
                semesterquery = d2.GetFunction("select * from textvaltable where TextCriteria = 'FEECA'and textval like '" + semester + " Semester' and textval not like '-1%' and college_code ='" + college + "'");

            }
            else
            {
                semesterquery = d2.GetFunction("select * from textvaltable where TextCriteria = 'FEECA'and textval like '" + semester + " Year' and textval not like '-1%' and college_code ='" + college + "'");

            }
        }

        return semesterquery;
    }
    public void bindsect()
    {
        try
        {
            ddl_sec.Items.Clear();
            if (ddl_colg.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_dept.Items.Count > 0 && ddl_sem.Items.Count > 0)
            {

                string branch = ddl_dept.SelectedValue.ToString();
                string batch = ddl_batch.SelectedValue.ToString();
                ListItem item = new ListItem("Empty", " ");
                string sqlquery = "select distinct sections from registration where batch_year=" + batch + " and degree_code=" + branch + " and college_code=" + ddl_colg.SelectedValue.ToString() + " and Current_Semester=" + ddl_sem.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";

                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(sqlquery, "text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_sec.DataSource = ds;
                        ddl_sec.DataTextField = "sections";
                        ddl_sec.DataValueField = "sections";
                        ddl_sec.DataBind();
                        ddl_sec.Enabled = true;

                    }
                    else
                    {
                        ddl_sec.Enabled = false;
                    }
                }
                else
                {
                    ddl_sec.Enabled = false;
                }
                // ddl_sec.Items.Add(item);
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void bindstream()
    {
        try
        {
            ddl_strm.Items.Clear();

            // string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.college_code=" + ddl_colg.SelectedItem.Value + "  and type<>'' order by type asc";
            string query = " select distinct type  from Course where college_code ='" + ddl_colg.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void bindHeader()
    {
        try
        {
            //if (Session["clgcode"] != null)
            //    collegecode1 = Convert.ToString(Session["clgcode"]);
            //else
            //    collegecode1 = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            }
            ddl_trheader.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster L,FS_HeaderPrivilage P WHERE L.HeaderPK = P.HeaderFK   AND P.CollegeCode = L.CollegeCode  AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_trheader.DataSource = ds;
                    ddl_trheader.DataTextField = "HeaderName";
                    ddl_trheader.DataValueField = "HeaderPK";
                    ddl_trheader.DataBind();

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void bindHeaderRe()
    {
        try
        {
            ddl_refheader.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster L,FS_HeaderPrivilage P WHERE L.HeaderPK = P.HeaderFK   AND P.CollegeCode = L.CollegeCode  AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_refheader.DataSource = ds;
                    ddl_refheader.DataTextField = "HeaderName";
                    ddl_refheader.DataValueField = "HeaderPK";
                    ddl_refheader.DataBind();

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void bindLedger()
    {
        try
        {
            //if (Session["clgcode"] != null)
            //    collegecode1 = Convert.ToString(Session["clgcode"]);
            //else
            //    collegecode1 = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            }
            ddl_trledger.Items.Clear();
            string headerfk = "-1";
            if (ddl_trheader.Items.Count > 0)
            {
                headerfk = Convert.ToString(ddl_trheader.SelectedItem.Value);
            }
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode and l.HeaderFK=" + headerfk + " AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_trledger.DataSource = ds;
                    ddl_trledger.DataTextField = "LedgerName";
                    ddl_trledger.DataValueField = "LedgerPK";
                    ddl_trledger.DataBind();

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void bindLedgerRe()
    {
        try
        {
            ddl_refledger.Items.Clear();
            string headerfk = "-1";
            if (ddl_refheader.Items.Count > 0)
            {
                headerfk = Convert.ToString(ddl_refheader.SelectedItem.Value);
            }
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode and l.HeaderFK=" + headerfk + " AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ddl_refledger.DataSource = ds;
                    ddl_refledger.DataTextField = "LedgerName";
                    ddl_refledger.DataValueField = "LedgerPK";
                    ddl_refledger.DataBind();

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void ddl_trheader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindLedger();
    }
    protected void ddl_refheader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindLedgerRe();
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
        txt_rerollno.Text = "";
        txt_rename.Text = "";
        txt_recolg.Text = "";
        txt_rebatch.Text = "";
        txt_restrm.Text = "";
        txt_redegree.Text = "";
        txt_redept.Text = "";
        txt_resem.Text = "";
        txt_resec.Text = "";
        txt_AmtPerc.Text = "";
        txt_reamt.Text = "";
        image3.ImageUrl = "";
        lnkindivmap.Enabled = false;
        bindGrid2();
        if (rb_transfer.Checked == true)
        {
            if (rbl_AdmitTransfer.SelectedIndex == 1)
            {
                txt_roll.Text = "";
                txt_name.Text = "";
                txt_date.Text = "";
                txt_colg.Text = "";
                txt_strm.Text = "";
                txt_batch.Text = "";
                txt_degree.Text = "";
                txt_dept.Text = "";
                txt_sem.Text = "";
                txt_sec.Text = "";
                txt_seattype.Text = "";
                image2.ImageUrl = "";
                rbl_AdmitTransfer.SelectedIndex = 0;
                rbl_AdmitTransfer_OnSelectedIndexChanged(sender, e);
                txt_tramt.Text = "";
                // bindGrid1();
                bindFromGrid();

            }
        }

    }
    protected void ddl_AmtPerc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_AmtPerc.Text = "";
        bindGrid2();
    }
    protected void chk_refCommon_OnCheckedChanged(object sender, EventArgs e)
    {
        // #region common
        //try
        //{
        //    ddl_AmtPerc.SelectedIndex = 0;
        //    if (txt_rerollno.Text.Trim() != "")
        //    {
        //        txt_AmtPerc.Text = "";
        //        bindGrid2();

        //        if (chk_refCommon.Checked)
        //        {
        //            ddl_AmtPerc.Enabled = false;
        //            txt_AmtPerc.ReadOnly = true;
        //            string stream = txt_restrm.Text.Trim();
        //            string edulevel = "";
        //            string feecat = "";

        //            string selQ = "select r.Current_Semester,C.type,c.Edu_Level   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code  and r.Roll_no='" + txt_rerollno.Text.Trim() + "' and d.college_code=" + collegecode1 + "";
        //            DataSet dss = new DataSet();
        //            dss = d2.select_method_wo_parameter(selQ, "Text");
        //            if (dss.Tables.Count > 0)
        //            {
        //                if (dss.Tables[0].Rows.Count > 0)
        //                {
        //                    edulevel = Convert.ToString(dss.Tables[0].Rows[0]["Edu_Level"]);
        //                    string sem = Convert.ToString(dss.Tables[0].Rows[0]["Current_Semester"]);

        //                    string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");

        //                    if (linkvalue != "")
        //                    {
        //                        if (linkvalue == "0")
        //                        {
        //                            feecat = d2.GetFunction("selECT top 1 TextCode from textvaltable where TextVal='" + sem + " Semester' and college_code=" + collegecode1 + "");
        //                        }
        //                        else
        //                        {
        //                            feecat = d2.GetFunction("selECT top 1 TextCode from textvaltable where TextVal='" + returnYearforSem(sem) + " Year' and college_code=" + collegecode1 + "");
        //                        }
        //                    }
        //                }
        //            }

        //            double totalamt = 0;
        //            foreach (GridViewRow rows in gridView3.Rows)
        //            {
        //                Label hdrid = (Label)rows.Cells[9].FindControl("lbl_hdrid");
        //                Label lgrid = (Label)rows.Cells[9].FindControl("lbl_lgrid");
        //                Label feecatid = (Label)rows.Cells[9].FindControl("lbl_feecat");
        //                Label paidamt = (Label)rows.Cells[9].FindControl("lbl_paid");
        //                TextBox txtAmt = (TextBox)rows.Cells[9].FindControl("txt_refund");


        //                string concessionQ = " select isnull(ConsAmt,0) as ConsAmt,isnull(ConsPer,0) as ConsPer, HeaderFk,LedgerFk,LedPriority from FM_ConcessionRefundSettings where RefMode=2 and stream='" + stream + "' and Edu_Level='" + edulevel + "' and Fee_Category='" + feecatid.Text + "' and HeaderFk=" + hdrid.Text + " and LedgerFk=" + lgrid.Text + "";
        //                ds.Clear();
        //                ds = d2.select_method_wo_parameter(concessionQ, "Text");
        //                if (ds.Tables.Count > 0)
        //                {
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {

        //                        string hdr = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFk"]);
        //                        string lgr = Convert.ToString(ds.Tables[0].Rows[i]["LedgerFk"]);
        //                        double consAmt = Convert.ToDouble(ds.Tables[0].Rows[i]["ConsAmt"]);
        //                        double consPer = Convert.ToDouble(ds.Tables[0].Rows[i]["ConsPer"]);

        //                        byte op = 0;
        //                        if (consAmt > 0)
        //                        {
        //                            //ddl_AmtPerc.SelectedIndex = 0;
        //                            op = 0;
        //                        }
        //                        else if (consPer > 0)
        //                        {
        //                            // ddl_AmtPerc.SelectedIndex = 1;
        //                            op = 1;
        //                        }

        //                        double paidAmt = Convert.ToDouble(paidamt.Text);
        //                        if (op == 0)
        //                        {
        //                            if (consAmt >= paidAmt)
        //                            {
        //                                txtAmt.Text = "0.00";
        //                            }
        //                            else
        //                            {
        //                                txtAmt.Text = (paidAmt - consAmt).ToString();
        //                                totalamt += paidAmt - consAmt;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (consPer > 0 && consPer <= 100)
        //                            {
        //                                txtAmt.Text = ((paidAmt * consPer) / 100).ToString();
        //                                totalamt += ((paidAmt * consPer) / 100);
        //                            }
        //                            else
        //                            {
        //                                txtAmt.Text = "0.00";
        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        txtAmt.Text = paidamt.Text;
        //                        totalamt += Convert.ToDouble(paidamt.Text);

        //                    }
        //                }
        //                else
        //                {
        //                    txtAmt.Text = paidamt.Text;
        //                    totalamt += Convert.ToDouble(paidamt.Text);
        //                }
        //            }
        //            txt_AmtPerc.Text = totalamt.ToString();

        //        }
        //        else
        //        {
        //            ddl_AmtPerc.Enabled = true;
        //            txt_AmtPerc.ReadOnly = false;
        //        }
        //    }
        //    else
        //    {
        //    }
        //}
        //catch { }
        //#endregion
        #region commoncheck
        try
        {
            Hashtable refundsetting = new Hashtable();
            if (txt_rerollno.Text.Trim() != "")
            {
                string stream = txt_restrm.Text.Trim();
                string edulevel = "";
                string sem = "";
                string semesterquery = "";
                string selqyery = "select r.Current_Semester,C.type,c.Edu_Level   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code  and r.Roll_no='" + txt_rerollno.Text.Trim() + "' and d.college_code=" + collegecode1 + "";
                DataSet dss = new DataSet();
                dss = d2.select_method_wo_parameter(selqyery, "Text");
                if (dss.Tables.Count > 0)
                {
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        edulevel = Convert.ToString(dss.Tables[0].Rows[0]["Edu_Level"]);
                        sem = Convert.ToString(dss.Tables[0].Rows[0]["Current_Semester"]);
                    }
                }
                if (chk_refCommon.Checked == true)
                {

                    string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + Session["collegecode"] + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(settingquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                        if (linkvalue == "0")
                        {
                            semesterquery = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '" + sem + " Semester' and textval not like '-1%'");
                        }
                        else
                        {
                            semesterquery = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '" + sem + " Year' and textval not like '-1%'");
                        }
                    }

                    string selectquery = "select HeaderFK,LedgerFK,ConsPer,ConsAmt from FM_ConcessionRefundSettings where RefMode =2 and  Stream in ('" + stream + "') and Edu_Level in('" + edulevel + "')   and Fee_Category in(" + semesterquery + ")";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[row]["ConsAmt"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[row]["ConsAmt"]).Trim() != "0.00")
                            {
                                refundsetting.Add(Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]), Convert.ToString(ds.Tables[0].Rows[row]["ConsAmt"]) + "-1");
                            }
                            if (Convert.ToString(ds.Tables[0].Rows[row]["ConsPer"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[row]["ConsPer"]).Trim() != "0.00")
                            {
                                refundsetting.Add(Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]), Convert.ToString(ds.Tables[0].Rows[row]["ConsPer"]) + "-2");
                            }
                        }
                        double totrefundvalue = 0;
                        double totrefunretun = 0;
                        if (refundsetting.Count > 0)
                        {
                            if (gridView3.Rows.Count > 0)
                            {
                                for (int ro = 0; ro < gridView3.Rows.Count; ro++)
                                {
                                    string getvalue = Convert.ToString((gridView3.Rows[ro].FindControl("lbl_lgrid") as Label).Text);
                                    string gettotamt = Convert.ToString((gridView3.Rows[ro].FindControl("lbl_paid") as Label).Text);
                                    if (getvalue.Trim() != "")
                                    {
                                        double finamt = 0;
                                        double finper = 0;
                                        if (refundsetting.ContainsKey(Convert.ToString(getvalue)))
                                        {
                                            string getamount = Convert.ToString(refundsetting[Convert.ToString(getvalue)]);
                                            string[] split = getamount.Split('-');
                                            string secondvalue = Convert.ToString(split[1]);
                                            if (Convert.ToString(split[1]) == "1")
                                            {

                                                string amonut = Convert.ToString(split[0]);
                                                (gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text = amonut;
                                                if (amonut != "" && gettotamt != "")
                                                {
                                                    if (Convert.ToDouble(gettotamt) >= Convert.ToDouble(amonut))
                                                    {
                                                        finamt = Convert.ToDouble(gettotamt) - Convert.ToDouble(amonut);
                                                        (gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(finamt);
                                                    }
                                                    else
                                                    {
                                                        finamt = 0;
                                                        (gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(finamt);
                                                    }
                                                }

                                            }
                                            else if (Convert.ToString(split[1]) == "2")
                                            {
                                                double refunvalue = 0;
                                                double percent = Convert.ToDouble(split[0]);
                                                (gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text = Convert.ToString(percent);
                                                if (Convert.ToString(percent) != "" && getamount != "")
                                                {
                                                    if (Convert.ToDouble(gettotamt) >= Convert.ToDouble(percent))
                                                    {
                                                        finper = Convert.ToDouble(gettotamt) * Convert.ToDouble(percent) / Convert.ToDouble(100);
                                                        refunvalue = Convert.ToDouble(gettotamt) - Convert.ToDouble(finper);
                                                        (gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(refunvalue);
                                                    }
                                                    else
                                                    {
                                                        finper = 0;
                                                        (gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(finper);
                                                    }

                                                }
                                            }

                                        }
                                        else
                                        {
                                            (gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(gettotamt);
                                        }
                                    }
                                    totrefundvalue = Convert.ToDouble((gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text);
                                    if (totrefundvalue > 0)
                                    {
                                        totrefunretun += Convert.ToDouble(totrefundvalue);

                                    }
                                }
                                txt_reamt.Text = Convert.ToString(totrefunretun);

                            }

                        }

                    }

                }
                else
                {
                    bindGrid2();
                    txt_reamt.Text = "";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
        #endregion
    }
    //popup History
    protected void btnHistory_Click(object sender, EventArgs e)
    {

        div_History.Visible = true;
        headerbind1();
        ledgerbind1();

        gridHist.DataSource = null;
        gridHist.DataBind();
        // btnhisgo.Visible = false;
        btnhisgo_Click(sender, e);
        imgAlert.Visible = false;
    }
    public void headerbind1()
    {
        try
        {
            txtheadr3.Text = "---Select---";
            cbheadr3.Checked = false;
            cblheadr3.Items.Clear();
            //if (Session["clgcode"] != null)
            //    collegecode1 = Convert.ToString(Session["clgcode"]);
            //else
            //    collegecode1 = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            }
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cblheadr3.DataSource = ds;
                cblheadr3.DataTextField = "HeaderName";
                cblheadr3.DataValueField = "HeaderPK";
                cblheadr3.DataBind();
                for (int i = 0; i < cblheadr3.Items.Count; i++)
                {
                    cblheadr3.Items[i].Selected = true;
                }
                txtheadr3.Text = "Header(" + cblheadr3.Items.Count + ")";
                cbheadr3.Checked = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void cbheadr3_ChekedChange(object sender, EventArgs e)
    {
        if (cbheadr3.Checked)
        {
            for (int i = 0; i < cblheadr3.Items.Count; i++)
            {
                cblheadr3.Items[i].Selected = true;
            }
            txtheadr3.Text = "Header(" + cblheadr3.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cblheadr3.Items.Count; i++)
            {
                cblheadr3.Items[i].Selected = false;
            }
            txtheadr3.Text = "---Select---";
        }
        ledgerbind1();
        gridHist.Visible = false;
        // btnhisgo.Visible = false;
        // imgAlert.Visible = true;
        //  lbl_alert.Text = "No Records Found";

    }
    protected void cblheadr3_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtheadr3.Text = "---Select---";
        cbheadr3.Checked = false;

        int cnt = 0;
        for (int i = 0; i < cblheadr3.Items.Count; i++)
        {
            if (cblheadr3.Items[i].Selected == true)
            {
                cnt++;
            }
        }
        txtheadr3.Text = "Header(" + cnt + ")";
        if (cnt == cblheadr3.Items.Count)
        {
            cbheadr3.Checked = true;
        }
        ledgerbind1();
        gridHist.Visible = false;
        // btnhisgo.Visible = false;
        //  imgAlert.Visible = true;
        // lbl_alert.Text = "No Records Found";
    }
    protected void cblgr3_ChekedChange(object sender, EventArgs e)
    {
        if (cblgr3.Checked)
        {
            for (int i = 0; i < cbllgr3.Items.Count; i++)
            {
                cbllgr3.Items[i].Selected = true;
            }
            txtlgr3.Text = "Ledger(" + cbllgr3.Items.Count + ")";

        }
        else
        {
            for (int i = 0; i < cbllgr3.Items.Count; i++)
            {
                cbllgr3.Items[i].Selected = false;
            }
            txtlgr3.Text = "---Select---";
        }
    }
    protected void cbl_lgr3_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtlgr3.Text = "---Select---";
        cblgr3.Checked = false;

        int cnt = 0;
        for (int i = 0; i < cbllgr3.Items.Count; i++)
        {
            if (cbllgr3.Items[i].Selected == true)
            {
                cnt++;
            }
        }
        txtlgr3.Text = "Ledger(" + cnt + ")";
        if (cnt == cbllgr3.Items.Count)
        {
            cblgr3.Checked = true;
        }
    }
    public void ledgerbind1()
    {
        try
        {
            txtlgr3.Text = "---Select---";
            cblgr3.Checked = false;
            string itemheadercode = "";
            for (int i = 0; i < cblheadr3.Items.Count; i++)
            {
                if (cblheadr3.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cblheadr3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "" + "," + "" + cblheadr3.Items[i].Value.ToString() + "";
                    }
                }
            }
            //if (Session["clgcode"] != null)
            //    collegecode1 = Convert.ToString(Session["clgcode"]);
            //else
            //    collegecode1 = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            }
            cbllgr3.Items.Clear();

            //string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " and L.HeaderFK in (" + itemheadercode + ")";
            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode1 + "  and L.HeaderFK in('" + itemheadercode + "') order by isnull(l.priority,1000), l.ledgerName asc";

            ds = d2.select_method_wo_parameter(query1, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbllgr3.DataSource = ds;
                cbllgr3.DataTextField = "LedgerName";
                cbllgr3.DataValueField = "LedgerPK";
                cbllgr3.DataBind();
                for (int i = 0; i < cbllgr3.Items.Count; i++)
                {
                    cbllgr3.Items[i].Selected = true;
                }
                txtlgr3.Text = "Ledger(" + cbllgr3.Items.Count + ")";
                cblgr3.Checked = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void btnhisgo_Click(object sender, EventArgs e)
    {
        try
        {
            string appno = string.Empty;
            string appQ = "select app_no from  Registration  where  ";
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    appQ = appQ + " Roll_no='" + txt_rerollno.Text.Trim() + "'";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    appQ = appQ + " Reg_No='" + txt_rerollno.Text.Trim() + "' ";
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    appQ = appQ + " Roll_Admit='" + txt_rerollno.Text.Trim() + "'";
                }
            }
            else
            {
                appQ = "select app_no from  applyn  where app_formno='" + txt_rerollno.Text.Trim() + "' ";
            }

            appno = d2.GetFunction(appQ);


            DataTable tbl_Ledger = new DataTable();
            tbl_Ledger.Columns.Add("S.No");
            tbl_Ledger.Columns.Add("Date");
            tbl_Ledger.Columns.Add("Receipt No");
            tbl_Ledger.Columns.Add("PaymentMode");
            tbl_Ledger.Columns.Add("Cheque/DD/ ChallanNo");
            tbl_Ledger.Columns.Add("HeaderName");
            tbl_Ledger.Columns.Add("LedgerName");
            tbl_Ledger.Columns.Add("Total");
            tbl_Ledger.Columns.Add("Paid");
            tbl_Ledger.Columns.Add("Balance");
            tbl_Ledger.Columns.Add("Year/Sem");

            string itemheadercode = "";
            for (int i = 0; i < cblheadr3.Items.Count; i++)
            {
                if (cblheadr3.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cblheadr3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "" + "," + "" + cblheadr3.Items[i].Value.ToString() + "";
                    }
                }
            }
            string ledgercode = "";
            for (int i = 0; i < cbllgr3.Items.Count; i++)
            {
                if (cbllgr3.Items[i].Selected == true)
                {
                    if (ledgercode == "")
                    {
                        ledgercode = "" + cbllgr3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        ledgercode = ledgercode + "" + "," + "" + cbllgr3.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheadercode != "" && appno != "")
            {
                string query = " select headername,ledgername,sum(totalamount) allot,sum(BalAmount) bal,a.FeeCategory,a.HeaderFK,l.LedgerPK,l.priority from FT_FeeAllot a, FM_HeaderMaster h, FM_LedgerMaster l where a.HeaderFK = h.HeaderPK  and a.LedgerFK = l.LedgerPK  and h.HeaderPK = l.HeaderFK and a.App_No='" + appno + "' and a.HeaderFk in (" + itemheadercode + ") and a.LedgerFK in (" + ledgercode + ") group by a.App_No,HeaderName,LedgerName,a.FeeCategory,a.HeaderFK,l.LedgerPK,l.priority order by isnull(l.priority,1000), l.ledgerName asc";

                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                int sno = 1;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string hid = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]);
                            string lid = Convert.ToString(ds.Tables[0].Rows[i]["LedgerPK"]);
                            string FeeCategory = Convert.ToString(ds.Tables[0].Rows[i]["FeeCategory"]);

                            query = " select Convert(varchar(10),TransDate,103) as TransDate,TransCode,case when D.PayMode = 1 THEN 'Cash' WHEN D.PayMode = 2 THEN 'Cheque' when D.PayMode = 3 then 'DD' when D.PayMode = 4 then 'Challan' end paymode,SUM(debit) paid,DDNo,l.priority from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l where d.HeaderFK = h.HeaderPK and d.LedgerFK = l.LedgerPK and h.HeaderPK = L.HeaderFK and d.App_No='" + appno + "' and h.HeaderPK in (" + hid + ") and d.LedgerFK =" + lid + " and d.FeeCategory =" + FeeCategory + " group by TransDate,TransCode,D.App_No,HeaderName,LedgerName,D.PayMode,DDNo,l.priority order by isnull(l.priority,1000), l.ledgerName asc";
                            DataSet ds2 = new DataSet();
                            ds2 = d2.select_method_wo_parameter(query, "Text");
                            if (ds2.Tables.Count > 0)
                            {
                                if (ds2.Tables[0].Rows.Count > 0)
                                {

                                    FeeCategory = d2.GetFunction("select textval from TextValTable where TextCode=" + FeeCategory + " and college_code=" + collegecode1 + "");

                                    DataRow drLedger = tbl_Ledger.NewRow();
                                    drLedger["S.No"] = sno;

                                    drLedger["HeaderName"] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                                    drLedger["ledgername"] = Convert.ToString(ds.Tables[0].Rows[i]["ledgername"]);
                                    drLedger["Year/Sem"] = FeeCategory;
                                    drLedger["Total"] = Convert.ToString(ds.Tables[0].Rows[i]["allot"]);

                                    drLedger["Date"] = Convert.ToString(ds2.Tables[0].Rows[0]["TransDate"]);
                                    drLedger["Receipt No"] = Convert.ToString(ds2.Tables[0].Rows[0]["TransCode"]);
                                    drLedger["PaymentMode"] = Convert.ToString(ds2.Tables[0].Rows[0]["paymode"]);
                                    drLedger["Paid"] = Convert.ToString(ds2.Tables[0].Rows[0]["paid"]);
                                    drLedger["Balance"] = Convert.ToString(ds.Tables[0].Rows[i]["bal"]);
                                    drLedger["Cheque/DD/ ChallanNo"] = Convert.ToString(ds2.Tables[0].Rows[0]["ddno"]);

                                    tbl_Ledger.Rows.Add(drLedger);
                                    sno++;
                                }
                            }

                        }

                        gridHist.DataSource = tbl_Ledger;
                        gridHist.DataBind();
                        gridHist.Visible = true;
                        // btnhisgo.Visible = true;
                        if (gridHist.Rows.Count < 1)
                        {
                            gridHist.DataSource = null;
                            gridHist.DataBind();
                            // btnhisgo.Visible = false;
                            imgAlert.Visible = true;
                            lbl_alert.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        gridHist.DataSource = null;
                        gridHist.DataBind();
                        // btnhisgo.Visible = false;
                        imgAlert.Visible = true;
                        lbl_alert.Text = "No Records Found";
                    }
                }
                else
                {
                    gridHist.DataSource = null;
                    gridHist.DataBind();
                    // btnhisgo.Visible = false;
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Records Found";
                }

            }
            else
            {
                gridHist.DataSource = null;
                gridHist.DataBind();
                // btnhisgo.Visible = false;
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Header";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "TransferRefund");
            gridHist.DataSource = null;
            gridHist.DataBind();
            //btnhisgo.Visible = false;
        }
    }
    protected void imagebtnpopHistclose_Click(object sender, EventArgs e)
    {
        div_History.Visible = false;
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }
    //Transfer and Refund 
    protected void txt_roll_noApp_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_roll_no1.Text.Trim() != "")
            {
                string rollNo = d2.GetFunction("select roll_no from Registration where roll_no='" + txt_roll_no1.Text.Trim() + "'").Trim();
                if (rollNo != "0")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Roll No Already Exists";
                    txt_roll_no1.Text = "";
                }
            }
        }
        catch { }
    }
    protected void txt_roll_noNotApp_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_roll_no.Text.Trim() != "")
            {
                string rollNo = d2.GetFunction("select roll_no from Registration where roll_no='" + txt_roll_no.Text.Trim() + "'").Trim();
                if (rollNo != "0")
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Roll No Already Exists";
                    txt_roll_no.Text = "";
                }
            }
        }
        catch { }
    }

    //transfer button
    protected void btn_transfer_Click(object sender, EventArgs e)
    {
        try
        {
            if (validateTransferCheck())
            {
                if (!cbwithoutfees.Checked)
                {
                    #region with fees
                    if (gridView1.Rows.Count > 0 && gridView2.Rows.Count > 0)
                    {
                        string totalamt = lbl_grid2_tot.Text;
                        string pd = lbl_grid2_paid.Text;
                        string bal = lbl_grid2_bal.Text;
                        string exces = lbl_grid2_excess.Text;
                        string unmatch = lblunmtexcess.Text;
                        if (totalamt == "")
                            totalamt = "Rs.0";
                        if (pd == "")
                            pd = "Rs.0";
                        if (bal == "")
                            bal = "Rs.0";
                        if (exces == "")
                            exces = "Rs.0";
                        if (unmatch == "")
                            unmatch = "Rs.0";

                        string tot = totalamt.Split('.')[1];
                        string paid = pd.Split('.')[1];
                        string bala = bal.Split('.')[1];
                        string ex = exces.Split('.')[1];
                        string unex = unmatch.Split('.')[1];

                        Label15.Text = tot;
                        Label17.Text = paid;
                        Label19.Text = bala;
                        Label21.Text = ex;
                        Label23.Text = unex;
                        Label14.Visible = true;
                        Label15.Visible = true;
                        Label16.Visible = true;
                        Label17.Visible = true;
                        Label18.Visible = true;
                        Label19.Visible = true;
                        Label20.Visible = true;
                        Label21.Visible = true;
                        Label22.Visible = true;
                        Label23.Visible = true;
                        Label12.Text = "Do You Want Continue";
                        div11.Visible = true;
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Fill The Values";
                    }
                    #endregion
                }
                else
                {
                    #region without fees
                    string appledval = string.Empty;
                    string clgcode = string.Empty;
                    string rollno = Convert.ToString(txt_roll.Text);
                    if (rbl_AdmitTransfer.SelectedIndex == 0)
                        appledval = Convert.ToString(txt_roll1.Text);
                    else
                        appledval = Convert.ToString(ddl_colg.SelectedItem.Value);
                    if (!string.IsNullOrEmpty(rollno) && !string.IsNullOrEmpty(appledval))
                    {
                        Label12.Text = "Do You Want Continue Without Fees";
                        div11.Visible = true;
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Fill The Details";
                    }
                    #endregion
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Correct Categories";
            }
        }
        catch { }

    }
    protected void button1_Click(object sender, EventArgs e)
    {
        div11.Visible = false;
        Label15.Text = "";
        Label17.Text = "";
        Label19.Text = "";
        Label21.Text = "";
        Label23.Text = "";
        if (!cbwithoutfees.Checked)
            loadtransfermethod();
        else
            transferWithoutFees();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        Label15.Text = "";
        Label17.Text = "";
        Label19.Text = "";
        Label21.Text = "";
        Label23.Text = "";
        div11.Visible = false;
    }

    protected DataSet notAppliedValues(string rollno, string collegecode1)
    {
        DataSet dsnot = new DataSet();
        string query = "select app_no,Stud_Name,Batch_Year,degree_code,college_code,Current_Semester,Sections from Registration where college_code='" + collegecode1 + "'";
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
        {
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                query = query + " and Roll_no='" + rollno + "'";
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                query = query + " and Reg_No='" + rollno + "' ";
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                query = query + " and Roll_Admit='" + rollno + "'";
        }
        else
        {
            query = "select app_no,Stud_Name,Batch_Year,degree_code,college_code,Current_Semester,'' Sections from applyn where app_formno='" + rollno + "' and college_code='" + collegecode1 + "'";
        }
        dsnot.Clear();
        dsnot = d2.select_method_wo_parameter(query, "Text");
        return dsnot;
    }

    protected void loadtransfermethod()
    {
        try
        {
            if (ddl_trheader.Items.Count > 0 && ddl_trledger.Items.Count > 0)
            {
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
                    stcollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                ArrayList htCheckVal = new ArrayList();
                ArrayList NewhtCheckVal = new ArrayList();
                StringBuilder sbOldRecptDate = new StringBuilder();
                StringBuilder sbOldRecptCode = new StringBuilder();
                double oldAmt = 0;
                StringBuilder sbNewRecptDate = new StringBuilder();
                StringBuilder sbNewRecptCode = new StringBuilder();
                double newAmt = 0;
                double newExcessAmt = 0;
                string oldRoll = string.Empty;
                string oldReg = string.Empty;
                string oldRollAdmit = string.Empty;
                string studAdmDate = string.Empty;
                string date = "";
                string currrentdate = "";
                string curdatetime = DateTime.Now.ToString();
                DateTime dt = new DateTime();
                string[] strdatetime = curdatetime.Split(' ');
                if (strdatetime.Length > 0)
                {
                    date = strdatetime[0].ToString();
                    dt = Convert.ToDateTime(date);
                    string[] curdate = date.Split('/');
                    if (curdate.Length > 0)
                        currrentdate = curdate[1].ToString() + "/" + curdate[0].ToString() + "/" + curdate[2].ToString();
                }
                string Currenttime = Convert.ToString(DateTime.Now.ToLongTimeString());
                #region
                if (rbl_AdmitTransfer.SelectedIndex == 0)
                {
                    #region Applied student
                    if (txt_roll1.Text.Trim() != "" && txt_roll.Text.Trim() != "")
                    {
                        string rollno = Convert.ToString(txt_roll1.Text);
                        string _roll_no = txt_roll_no1.Text.Trim();
                        if (string.IsNullOrEmpty(_roll_no))
                            _roll_no = rollno;
                        string appno = "";
                        string batch = "";
                        string degree = "";
                        string dept = "";
                        string degcode = "";
                        string stream = "";
                        string sem = "";
                        string sec = "";
                        string colCode = "";
                        string name = "";
                        string query = "select a.parent_name,a.stud_name, a.Stud_Type,c.Course_Name,dt.Dept_Name,a.degree_code,a.Current_Semester  ,a.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,a.app_no,a.seattype  from applyn a ,Degree d,course c,Department dt,collinfo co where  a.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code  and a.app_formno='" + rollno + "' and d.college_code='" + collegecode1 + "'";
                        ds1 = d2.select_method_wo_parameter(query, "Text");
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            name = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                            batch = Convert.ToString(ds1.Tables[0].Rows[0]["Batch_Year"]);
                            degree = Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]);
                            dept = Convert.ToString(ds1.Tables[0].Rows[0]["Dept_Name"]);
                            // sec = Convert.ToString( ds1.Tables[0].Rows[0]["Sections"]);
                            sec = txt_sec1.Text.Trim();
                            sem = Convert.ToString(ds1.Tables[0].Rows[0]["Current_Semester"]);
                            colCode = Convert.ToString(ds1.Tables[0].Rows[0]["college_code"]);
                            stream = Convert.ToString(ds1.Tables[0].Rows[0]["type"]);
                            degcode = Convert.ToString(ds1.Tables[0].Rows[0]["degree_code"]);
                            appno = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]);
                            string sndSeat = Convert.ToString(ds1.Tables[0].Rows[0]["seattype"]);
                            DateTime transdate = Convert.ToDateTime(txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2]);
                            string finYearid = d2.getCurrentFinanceYear(usercode, colCode);

                            bool updateOK = false;

                            //Update Registration table
                            string updateApp = "update applyn set admission_status =1 where app_no ='" + appno + "'";
                            d2.update_method_wo_parameter(updateApp, "Text");

                            if (ddladmis.SelectedItem.Text.Trim() == "After Admission")
                            {
                                string selQReg = " select roll_no,reg_no,roll_admit,adm_date from registration where app_no='" + appno + "'";
                                DataSet dsReg = d2.select_method_wo_parameter(selQReg, "Text");
                                if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
                                {
                                    oldRoll = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_no"]);
                                    oldReg = Convert.ToString(dsReg.Tables[0].Rows[0]["reg_no"]);
                                    oldRollAdmit = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_admit"]);
                                    studAdmDate = Convert.ToString(dsReg.Tables[0].Rows[0]["adm_date"]);
                                }
                                string updateReg = "update Registration set DelFlag=1 where Roll_No='" + txt_roll.Text.Trim() + "'";
                                d2.update_method_wo_parameter(updateReg, "Text");

                                string insReg = "  insert into Registration (App_No,Adm_Date,Roll_Admit,Roll_No,RollNo_Flag,Reg_No,Stud_Name,Batch_Year,degree_code,college_code,CC,DelFlag,Exam_Elg,mode,Current_Semester,Sections)values ('" + appno + "','" + transdate.ToString("MM/dd/yyyy") + "','" + rollno + "','" + _roll_no + "','1','" + rollno + "','" + name + "','" + batch + "','" + degcode + "','" + colCode + "','0','0','OK',3,1,'" + sec + "')";
                                d2.update_method_wo_parameter(insReg, "Text");
                            }
                            //new insert to studentransfer table
                            string fstClgcode = Convert.ToString(lbltempfstclg.Text);
                            string fstBatchYr = Convert.ToString(txt_batch.Text);
                            string fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
                            string fstSection = Convert.ToString(txt_sec.Text);
                            string fstSeat = Convert.ToString(txt_seattype.Text);
                            string fstSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlcollege.SelectedValue + "'  and textval='" + fstSeat.Trim() + "'"));

                            foreach (GridViewRow row in gridView2.Rows)
                            {
                                string balan = "";
                                Label hdrid = (Label)row.Cells[1].FindControl("lbl_hdrid");
                                Label lgrid = (Label)row.Cells[1].FindControl("lbl_lgrid");
                                Label feecat = (Label)row.Cells[1].FindControl("lbl_feecat");
                                Label feeamt = (Label)row.Cells[1].FindControl("lbl_feeamt");
                                Label totamt = (Label)row.Cells[1].FindControl("lbl_totamt");
                                Label concession = (Label)row.Cells[1].FindControl("lbl_Concess");
                                TextBox paid = (TextBox)row.Cells[1].FindControl("txt_paid");
                                TextBox balance = (TextBox)row.Cells[1].FindControl("txt_bal");
                                if (balance.Text.Trim() != "" && balance.Text.Trim() != "0")
                                    balan = Convert.ToString(balance.Text);
                                else
                                    balan = "0";
                                TextBox excess = (TextBox)row.Cells[1].FindControl("txt_exGrid2");
                                string updateFeeallot = " if exists (select * from FT_FeeAllot where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "'))   update FT_FeeAllot set AllotDate='" + transdate.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount=FeeAmount+" + feeamt.Text + ",DeductAmout=DeductAmout+0,DeductReason='0',FromGovtAmt=FromGovtAmt+0,TotalAmount=TotalAmount+" + totamt.Text + ",RefundAmount=RefundAmount+0,IsFeeDeposit='1',FeeAmountMonthly='',PayMode='1',PayStartDate='',PaidStatus='0',DueDate='',DueAmount=DueAmount+0,FineAmount=FineAmount+0,BalAmount=BalAmount+'" + balan + "' where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + lgrid.Text + "," + hdrid.Text + ",'" + feeamt.Text + "','0','0','0','" + totamt.Text + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + balan + "'," + finYearid + ")";
                                d2.update_method_wo_parameter(updateFeeallot, "Text");
                                if (row.RowIndex == gridView2.Rows.Count - 1)
                                {
                                    double amt = 0;
                                    string ddlhdr = Convert.ToString(ddl_trheader.SelectedItem.Value);
                                    string ddllgr = Convert.ToString(ddl_trledger.SelectedItem.Value);
                                    if (txt_tramt.Text.Trim() != "")
                                    {
                                        amt = Convert.ToDouble(txt_tramt.Text.Trim());
                                    }
                                    string updateTransfer = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ddllgr + "') and HeaderFK in('" + ddlhdr + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "'))   update FT_FeeAllot set AllotDate='" + transdate.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount=FeeAmount+" + amt + ",DeductAmout=DeductAmout+0,DeductReason='0',FromGovtAmt=FromGovtAmt+0,TotalAmount=TotalAmount+" + amt + ",RefundAmount=RefundAmount+0,IsFeeDeposit='1',FeeAmountMonthly='',PayMode='1',PayStartDate='',PaidStatus='0',DueDate='',DueAmount=DueAmount+0,FineAmount=FineAmount+0,BalAmount=BalAmount+" + amt + " where LedgerFK in('" + ddllgr + "') and HeaderFK in('" + ddlhdr + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + ddllgr + "," + ddlhdr + ",'" + amt + "','0','0','0','" + amt + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + amt + "'," + finYearid + ")";
                                    d2.update_method_wo_parameter(updateTransfer, "Text");
                                }
                                updateOK = true;
                            }
                            if (gridView2.Rows.Count > 0)
                            {
                                string excessval = Convert.ToString(lbl_grid2_excess.Text);
                                string[] str = excessval.Split('.');
                                if (str.Length > 0)
                                    excessval = str[1].ToString();
                                if (excessval != "" && excessval != "0")
                                {
                                    string select = "if exists(select * from FT_ExcessDet where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "')update FT_ExcessDet set ExcessAmt=ExcessAmt+'" + excessval + "',BalanceAmt=BalanceAmt+'" + excessval + "' where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "' else insert into FT_ExcessDet (ExcessTransDate,TransTime,MemType,App_No ,ExcessType,ExcessAmt,BalanceAmt,FinYearFK ) values('" + currrentdate + "','" + Currenttime + "','1','" + appno + "','1','" + excessval + "','" + excessval + "','" + finYearid + "')";
                                    d2.update_method_wo_parameter(select, "Text");
                                    double.TryParse(excessval, out newExcessAmt);
                                }
                                string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='1'");
                                for (int i = 0; i < gridView2.Rows.Count; i++)
                                {
                                    Label header = (Label)gridView2.Rows[i].FindControl("lbl_hdrid");
                                    Label ledger = (Label)gridView2.Rows[i].FindControl("lbl_lgrid");
                                    Label totalamt = (Label)gridView2.Rows[i].FindControl("lbl_totamt");
                                    TextBox excessamt = (TextBox)gridView2.Rows[i].FindControl("txt_exGrid2");
                                    if (excessamt.Text.Trim() != "" && excessamt.Text.Trim() != "0")
                                    {
                                        string selqry = " select * from FT_ExcessLedgerDet if  exists(select * from FT_ExcessLedgerDet where  ExcessDetFK='" + getvalue + "' and HeaderFK='" + header.Text + "' and LedgerFK='" + ledger.Text + "' )update FT_ExcessLedgerDet set ExcessAmt=ExcessAmt+'" + excessamt.Text + "',BalanceAmt=BalanceAmt+'" + excessamt.Text + "',HeaderFK ='" + header.Text + "',LedgerFK='" + ledger.Text + "' where ExcessDetFK='" + getvalue + "' and HeaderFK ='" + header.Text + "' and LedgerFK='" + ledger.Text + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK) values('" + header.Text + "','" + ledger.Text + "','" + excessamt.Text + "','" + excessamt.Text + "','" + getvalue + "')";
                                        d2.update_method_wo_parameter(selqry, "Text");
                                    }
                                }
                                lbl_grid2_excess.Text = "";
                            }
                            if (updateOK)
                            {
                                //student transfer details
                                transfer(appno, fstDegreecode, degcode, fstSection, sec, fstClgcode, colCode, batch, fstSeatCode, sndSeat, sbOldRecptDate, sbOldRecptCode, oldAmt, sbNewRecptDate, sbNewRecptCode, newAmt, newExcessAmt, oldRoll, oldReg, oldRollAdmit, studAdmDate);
                                UpdateAdmissionNo(appno);
                                txt_roll1.Text = "";
                                txt_roll_no1.Text = "";
                                txt_roll_no.Text = "";
                                // txt_roll1_TextChanged(sender, e);
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Transferred Sucessfully";
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Not Transferred";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Student Details Not Found";
                        }
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Enter Admission Number";
                    }
                    #endregion
                }
                else
                {
                    #region Not Applied students
                    if (txt_roll.Text.Trim() != "")
                    {
                        string rollno = Convert.ToString(txt_roll.Text);
                        string _roll_no = txt_roll_no.Text.Trim();
                        if (string.IsNullOrEmpty(_roll_no))
                            _roll_no = rollno;
                        string appno = "";
                        string batch = "";
                        string degcode = "";
                        string sem = "";
                        string sec = "";
                        string colCode = "";
                        string Rcptno = "";
                        ds1 = notAppliedValues(rollno, collegecode1);
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            if (ddl_batch.Items.Count > 0)
                                batch = Convert.ToString(ddl_batch.SelectedItem.Value);
                            if (ddl_sec.Items.Count > 0)
                                sec = Convert.ToString(ddl_sec.SelectedItem.Value);
                            if (ddl_sem.Items.Count > 0)
                                sem = Convert.ToString(ddl_sem.SelectedItem.Value);
                            if (ddl_colg.Items.Count > 0)
                                colCode = Convert.ToString(ddl_colg.SelectedItem.Value);
                            if (ddl_dept.Items.Count > 0)
                                degcode = Convert.ToString(ddl_dept.SelectedItem.Value);
                            if (ddl_seattype.Items.Count > 0)
                                seatype = Convert.ToString(ddl_seattype.SelectedItem.Value);
                            appno = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]);
                            DateTime transdate = Convert.ToDateTime(txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2]);
                            string curtime = DateTime.Now.ToShortTimeString();
                            string finYearid = d2.getCurrentFinanceYear(usercode, colCode);
                            bool updateOK = false;
                            Dictionary<string, string> dtReceipt = new Dictionary<string, string>();
                            Dictionary<string, string> arRcptfk = new Dictionary<string, string>();
                            if (colCode != "" && batch != "" && degcode != "" && appno != "")
                            {
                                string hedgid = headerValue();
                                if (!string.IsNullOrEmpty(hedgid))
                                {
                                    hedgid = "'" + hedgid + "'";
                                    if (hedgid != "")
                                        Rcptno = generateJournalNo(hedgid, colCode);
                                    // Rcptno = generateReceiptNo(hedgid, ref dtReceipt, ref arRcptfk);
                                }
                                bool pvs = false;
                                if (!string.IsNullOrEmpty(Rcptno) || dtReceipt.Count > 0)
                                {
                                    string selQReg = " select roll_no,reg_no,roll_admit,adm_date from registration where app_no='" + appno + "'";
                                    DataSet dsReg = d2.select_method_wo_parameter(selQReg, "Text");
                                    if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
                                    {
                                        oldRoll = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_no"]);
                                        oldReg = Convert.ToString(dsReg.Tables[0].Rows[0]["reg_no"]);
                                        oldRollAdmit = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_admit"]);
                                        studAdmDate = Convert.ToString(dsReg.Tables[0].Rows[0]["adm_date"]);
                                    }
                                    applynAndRegistUpdate(degcode, seatype, appno, ddl_colg.SelectedItem.Value, colCode, batch, sem, sec, _roll_no);
                                    //new insert to studentransfer table
                                    string fstClgcode = Convert.ToString(lbltempfstclg.Text);
                                    string fstBatchYr = Convert.ToString(txt_batch.Text);
                                    string fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
                                    string fstSection = Convert.ToString(txt_sec.Text);
                                    string fstSeat = Convert.ToString(txt_seattype.Text);
                                    string fstSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlcollege.SelectedValue + "'  and textval='" + fstSeat.Trim() + "'"));

                                    UpdateAdmissionNo(appno);
                                    string entryUserCode = d2.GetFunction(" select distinct entryusercode from FT_FinDailyTransaction where app_no='" + appno + "'");
                                    //Daily transaction update 
                                    int updpaid = updateCreditAmt(appno, Rcptno, transdate.ToString("MM/dd/yyyy"));
                                    // string Upd = " update FT_FinDailyTransaction set IsCanceled='1' where App_No='" + appno + "'";
                                    //  int updpaid = d2.update_method_wo_parameter(Upd, "Text");
                                    if (updpaid > 0)
                                    {
                                        string selQRegs = " select distinct convert(varchar(10),transdate,103) as transdate,transcode,debit from FT_FinDailyTransaction where App_No='" + appno + "' and IsCanceled='1' ";
                                        DataSet dsOld = d2.select_method_wo_parameter(selQRegs, "Text");
                                        if (dsOld.Tables.Count > 0 && dsOld.Tables[0].Rows.Count > 0)
                                        {
                                            for (int old = 0; old < dsOld.Tables[0].Rows.Count; old++)
                                            {
                                                if (!htCheckVal.Contains(Convert.ToString(dsOld.Tables[0].Rows[old]["transdate"])))
                                                {
                                                    sbOldRecptDate.Append(Convert.ToString(dsOld.Tables[0].Rows[old]["transdate"])
    + ",");
                                                    htCheckVal.Add(Convert.ToString(dsOld.Tables[0].Rows[old]["transdate"]));
                                                }
                                                if (!htCheckVal.Contains(Convert.ToString(dsOld.Tables[0].Rows[old]["transcode"])))
                                                {
                                                    sbOldRecptCode.Append(Convert.ToString(dsOld.Tables[0].Rows[old]["transcode"]) + ",");
                                                    htCheckVal.Add(Convert.ToString(dsOld.Tables[0].Rows[old]["transcode"]));
                                                }
                                                double tempPaidAmt = 0;
                                                double.TryParse(Convert.ToString(dsOld.Tables[0].Rows[old]["debit"]), out tempPaidAmt);
                                                oldAmt += tempPaidAmt;
                                            }
                                        }
                                    }

                                    foreach (GridViewRow row in gridView2.Rows)
                                    {
                                        Label hdrid = (Label)row.Cells[1].FindControl("lbl_hdrid");
                                        Label lgrid = (Label)row.Cells[1].FindControl("lbl_lgrid");
                                        Label feecat = (Label)row.Cells[1].FindControl("lbl_feecat");
                                        Label feeamt = (Label)row.Cells[1].FindControl("lbl_feeamt");
                                        Label totamt = (Label)row.Cells[1].FindControl("lbl_totamt");
                                        Label concession = (Label)row.Cells[1].FindControl("lbl_Concess");
                                        TextBox paid = (TextBox)row.Cells[1].FindControl("txt_paid");
                                        TextBox balance = (TextBox)row.Cells[1].FindControl("txt_bal");
                                        TextBox excess = (TextBox)row.Cells[1].FindControl("txt_exGrid2");

                                        if (feeamt.Text == "")
                                            feeamt.Text = "0";
                                        if (totamt.Text == "")
                                            totamt.Text = "0";
                                        if (concession.Text == "")
                                            concession.Text = "0";
                                        if (paid.Text == "")
                                            paid.Text = "0";
                                        if (balance.Text == "")
                                            balance.Text = "0";
                                        if (excess.Text == "")
                                            excess.Text = "0";

                                        string updateFeeallot = "if exists (select * from FT_FeeAllot where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "')) update FT_FeeAllot set AllotDate='" + transdate.ToString("MM/dd/yyyy") + "',FeeAmount='" + feeamt.Text + "',DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + totamt.Text + "',RefundAmount='0',IsFeeDeposit='1',PayMode='1',FeeCategory='" + feecat.Text + "',PaidStatus='0',DueAmount='0',FineAmount='0',BalAmount='" + balance.Text + "',paidamount='" + paid.Text + "' where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot (AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + lgrid.Text + "," + hdrid.Text + ",'" + feeamt.Text + "','0','0','0','" + totamt.Text + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + balance.Text + "'," + finYearid + ")";
                                        d2.update_method_wo_parameter(updateFeeallot, "Text");
                                        if (dtReceipt.Count > 0)
                                            if (dtReceipt.ContainsKey(Convert.ToString(hdrid.Text)))
                                                Rcptno = dtReceipt[hdrid.Text].ToString();
                                        if (paid.Text != "0")
                                        {
                                            string selQy = "select distinct paymode from ft_findailytransaction where app_no='" + appno + "' and isnull(iscanceled,'0')='1' and debit='" + paid.Text + "'";
                                            string payMode = d2.GetFunction(selQy);
                                            payMode = "1";
                                            if (payMode != "0")
                                            {
                                                string INSdaily = "if exists(select * from FT_FinDailyTransaction where HeaderFK='" + hdrid.Text + "' and LedgerFK='" + lgrid.Text + "' and FeeCategory='" + feecat.Text + "' and TransCode='" + Rcptno + "' and App_No='" + appno + "' and FinYearFK='" + finYearid + "')update FT_FinDailyTransaction set Debit='" + paid.Text + "',TransDate='" + transdate.ToString("MM/dd/yyyy") + "',TransTime='" + curtime + "' ,IsCanceled='0',IsCollected='1',paymode='" + payMode + "' where HeaderFK='" + hdrid.Text + "' and LedgerFK='" + lgrid.Text + "' and FeeCategory='" + feecat.Text + "' and TransCode='" + Rcptno + "' and App_No='" + appno + "'  and FinYearFK='" + finYearid + "' else   insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,Debit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype) values('" + transdate.ToString("MM/dd/yyyy") + "','" + curtime + "','" + Rcptno + "','1','" + lgrid.Text + "','" + hdrid.Text + "','" + feecat.Text + "','" + paid.Text + "','" + finYearid + "','" + appno + "','0','1','" + payMode + "','1','" + entryUserCode + "','3')";
                                                d2.update_method_wo_parameter(INSdaily, "Text");
                                                if (!NewhtCheckVal.Contains(transdate))
                                                {
                                                    sbNewRecptDate.Append(transdate + ",");
                                                    NewhtCheckVal.Add(transdate);
                                                }
                                                if (!NewhtCheckVal.Contains(Rcptno))
                                                {
                                                    sbNewRecptCode.Append(Rcptno + ",");
                                                    NewhtCheckVal.Add(Rcptno);
                                                }

                                                double tempNewPaidAmt = 0;
                                                double.TryParse(Convert.ToString(paid.Text), out tempNewPaidAmt);
                                                newAmt += tempNewPaidAmt;
                                            }
                                        }
                                        if (row.RowIndex == gridView2.Rows.Count - 1)
                                        {
                                            string amt = "";
                                            string ddlhdr = Convert.ToString(ddl_trheader.SelectedItem.Value);
                                            string ddllgr = Convert.ToString(ddl_trledger.SelectedItem.Value);
                                            amt = Convert.ToString(txt_tramt.Text.Trim());
                                            if (ddlhdr != "" && ddllgr != "" && amt != "")
                                            {
                                                string updateTransfer = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ddllgr + "') and HeaderFK in('" + ddlhdr + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "')) update FT_FeeAllot set AllotDate='" + transdate.ToString("MM/dd/yyyy") + "',FeeAmount=ISNULL(FeeAmount,'0')+'" + amt + "',DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount=ISNULL(TotalAmount,'0')+'" + amt + "',RefundAmount='0',IsFeeDeposit='1',PayMode='1',FeeCategory='" + feecat.Text + "',PaidStatus='0',DueAmount='0',FineAmount='0',BalAmount=ISNULL(BalAmount,'0')+'" + amt + "' where LedgerFK in('" + ddllgr + "') and HeaderFK in('" + ddlhdr + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + ddllgr + "," + ddlhdr + ",'" + amt + "','0','0','0','" + amt + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + amt + "'," + finYearid + ")";
                                                d2.update_method_wo_parameter(updateTransfer, "Text");
                                            }
                                        }
                                        #region excess amount

                                        if (gridView2.Rows.Count > 0)
                                        {
                                            string excessval = Convert.ToString(lbl_grid2_excess.Text);
                                            if (excessval != "" && excessval != "0")
                                            {
                                                string[] str = excessval.Split('.');
                                                if (str.Length > 0)
                                                {
                                                    excessval = str[1].ToString();
                                                    if (excessval == "0")
                                                        excessval = "0";
                                                }
                                            }
                                            if (excessval != "0" && excessval != "")
                                            {
                                                string select = "if exists(select * from FT_ExcessDet where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat.Text + "')update FT_ExcessDet set ExcessAmt=ExcessAmt+'" + excessval + "',BalanceAmt=BalanceAmt+'" + excessval + "' where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat.Text + "' else insert into FT_ExcessDet (ExcessTransDate,TransTime,MemType,App_No ,ExcessType,ExcessAmt,BalanceAmt,FinYearFK , FeeCategory) values('" + dt.ToString("MM/dd/yyyy") + "','" + Currenttime + "','1','" + appno + "','1','" + excessval + "','" + excessval + "','" + finYearid + "','" + feecat.Text + "')";
                                                d2.update_method_wo_parameter(select, "Text");
                                                double.TryParse(excessval, out newExcessAmt);

                                                string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='1'");
                                                for (int i = 0; i < gridView2.Rows.Count; i++)
                                                {
                                                    Label header = (Label)gridView2.Rows[i].FindControl("lbl_hdrid");
                                                    Label ledger = (Label)gridView2.Rows[i].FindControl("lbl_lgrid");
                                                    Label feecatg = (Label)gridView2.Rows[i].FindControl("lbl_feecat");
                                                    Label totalamt = (Label)gridView2.Rows[i].FindControl("lbl_totamt");
                                                    TextBox excessamt = (TextBox)gridView2.Rows[i].FindControl("txt_exGrid2");
                                                    if (excessamt.Text.Trim() != "" && excessamt.Text.Trim() != "0")
                                                    {
                                                        string selqry = "select * from FT_ExcessLedgerDet if  exists(select * from FT_ExcessLedgerDet where  ExcessDetFK='" + getvalue + "' and HeaderFK='" + header.Text + "' and LedgerFK='" + ledger.Text + "' and FinYearFK='" + finYearid + "' and FeeCategory in('" + feecatg.Text + "') )update FT_ExcessLedgerDet set ExcessAmt=ExcessAmt+'" + excessamt.Text + "',BalanceAmt=BalanceAmt+'" + excessamt.Text + "',HeaderFK ='" + header.Text + "',LedgerFK='" + ledger.Text + "' where ExcessDetFK='" + getvalue + "' and HeaderFK ='" + header.Text + "' and LedgerFK='" + ledger.Text + "' and FinYearFK='" + finYearid + "' and FeeCategory in('" + feecatg.Text + "') else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FinYearFK,FeeCategory) values('" + header.Text + "','" + ledger.Text + "','" + excessamt.Text + "','" + excessamt.Text + "','" + getvalue + "','" + finYearid + "','" + feecatg.Text + "')";
                                                        d2.update_method_wo_parameter(selqry, "Text");
                                                    }
                                                }
                                            }
                                            lbl_grid2_excess.Text = "";
                                        }
                                        #endregion

                                        #region Unmatched Header Excess Amount
                                        Dictionary<string, string> dictex = new Dictionary<string, string>();
                                        string extraex = Convert.ToString(lblunmtexcess.Text);
                                        if (lblunmtexcess.Text.Trim() != "" && lblunmtexcess.Text.Trim() != "0")
                                        {
                                            string[] exstr = extraex.Split('.');
                                            if (exstr.Length > 0)
                                            {
                                                extraex = exstr[1].ToString();
                                                if (extraex == "0")
                                                    extraex = "0";
                                            }
                                            if (extraex != "" && extraex != "0")
                                            {
                                                if (Session["excess"] != null)
                                                {
                                                    string Sel = "if exists(select * from FT_ExcessDet where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat.Text + "')update FT_ExcessDet set ExcessAmt=ExcessAmt+'" + extraex + "',BalanceAmt=BalanceAmt+'" + extraex + "' where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat.Text + "' else insert into FT_ExcessDet (ExcessTransDate,TransTime,MemType,App_No ,ExcessType,ExcessAmt,BalanceAmt,FinYearFK, FeeCategory ) values('" + dt.ToString("MM/dd/yyyy") + "','" + Currenttime + "','1','" + appno + "','1','" + extraex + "','" + extraex + "','" + finYearid + "','" + feecat.Text + "')";
                                                    d2.update_method_wo_parameter(Sel, "Text");
                                                    double tempextraAmt = 0;
                                                    double.TryParse(extraex, out tempextraAmt);
                                                    newExcessAmt += tempextraAmt;
                                                    string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='1'");
                                                    dictex = (Dictionary<string, string>)Session["excess"];
                                                    foreach (KeyValuePair<string, string> value in dictex)
                                                    {
                                                        string keyval = value.Key.ToString();
                                                        string valueval = value.Value.ToString();
                                                        string[] fkval = keyval.Split('-');
                                                        if (fkval.Length > 0)
                                                        {
                                                            string selqry = " select * from FT_ExcessLedgerDet if  exists(select * from FT_ExcessLedgerDet where  ExcessDetFK='" + getvalue + "' and HeaderFK='" + fkval[0].ToString() + "' and LedgerFK='" + fkval[1].ToString() + "' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat.Text + "' )update FT_ExcessLedgerDet set ExcessAmt=ExcessAmt+'" + valueval + "',BalanceAmt=BalanceAmt+'" + valueval + "',HeaderFK ='" + fkval[0].ToString() + "',LedgerFK='" + fkval[1].ToString() + "' where ExcessDetFK='" + getvalue + "' and HeaderFK ='" + fkval[0].ToString() + "' and LedgerFK='" + fkval[1].ToString() + "' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat.Text + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FinYearFK, FeeCategory) values('" + fkval[0].ToString() + "','" + fkval[1].ToString() + "','" + valueval + "','" + valueval + "','" + getvalue + "' ,'" + finYearid + "','" + feecat.Text + "')";
                                                            d2.update_method_wo_parameter(selqry, "Text");
                                                        }
                                                    }
                                                    lblunmtexcess.Text = "";
                                                }
                                            }
                                        }
                                        #endregion
                                        updateOK = true;
                                        //Rcptno = string.Empty;
                                    }
                                    if (updateOK)
                                    {
                                        bindGrid2();
                                        //student transfer new entry
                                        transfer(appno, fstDegreecode, ddl_dept.SelectedItem.Value, fstSection, sec, fstClgcode, ddl_colg.SelectedItem.Value, ddl_batch.SelectedItem.Value, fstSeatCode, seatype, sbOldRecptDate, sbOldRecptCode, oldAmt, sbNewRecptDate, sbNewRecptCode, newAmt, newExcessAmt, oldRoll, oldReg, oldRollAdmit, studAdmDate);
                                        #region Update Receipt No

                                        if (Convert.ToInt32(Session["save1"]) != 5)
                                        {
                                            string updateRecpt = string.Empty;
                                            //if (Convert.ToInt32(Session["isHeaderwise"]) == 0 || Convert.ToInt32(Session["isHeaderwise"]) == 2)
                                            //{
                                            //    Rcptno = Rcptno.Remove(0, Convert.ToString(Session["acronym"]).Length);
                                            //    updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + Rcptno + "+1 where collegecode =" + ddl_colg.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + ddl_colg.SelectedItem.Value + ")";
                                            //    d2.update_method_wo_parameter(updateRecpt, "Text");
                                            //}
                                            if (Convert.ToInt32(Session["isHeaderwise"]) == 0)
                                            {
                                                Rcptno = Rcptno.Remove(0, Convert.ToString(Session["acronym"]).Length);
                                                updateRecpt = " update FM_FinCodeSettings set JournalStNo=" + Rcptno + "+1 where collegecode =" + ddl_colg.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + ddl_colg.SelectedItem.Value + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                                                d2.update_method_wo_parameter(updateRecpt, "Text");
                                            }
                                            else
                                            {
                                                ArrayList arrcpt = new ArrayList();
                                                foreach (KeyValuePair<string, string> reptUpdate in dtReceipt)
                                                {
                                                    string headerfk = reptUpdate.Key.ToString();
                                                    Rcptno = reptUpdate.Value.ToString();
                                                    if (!arrcpt.Contains(Rcptno))
                                                    {
                                                        string hdFkval = string.Empty;
                                                        if (arRcptfk.ContainsKey(Rcptno))
                                                        {
                                                            hdFkval = arRcptfk[Rcptno].ToString();
                                                            arrcpt.Add(Rcptno);
                                                            Rcptno = Rcptno.Remove(0, Convert.ToString(hdFkval.Split('-')[1]).Length);
                                                            updateRecpt = "update FM_HeaderFinCodeSettings set RcptStNo=" + Rcptno + "+1 where HeaderSettingPK=" + hdFkval.Split('-')[0] + " and FinyearFK=" + finYearid + " and CollegeCode=" + ddl_colg.SelectedItem.Value + " and isnull(onlineRec,'')<>'1'";
                                                            d2.update_method_wo_parameter(updateRecpt, "Text");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        transferReceipt("Journal", appno, ddl_colg.SelectedItem.Value, transdate.ToString("MM/dd/yyyy"), Convert.ToString(sbNewRecptCode));
                                        txt_tramt.Text = "";
                                        txt_roll_no1.Text = "";
                                        txt_roll_no.Text = "";
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Transferred Sucessfully";
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Not Transferred";
                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "ReceiptNo Not Generated";
                                }
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Insufficient To Details";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Student Details Not Found";
                        }
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Enter Roll Number";
                    }
                    #endregion
                }
                #endregion

            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Check Header And Ledger";
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "TransferRefund");
            imgAlert.Visible = true; lbl_alert.Text = "Please Try Later";
        }
    }
    protected int updateCreditAmt(string appNo, string Rcptno, string transdate)
    {
        int updtCnt = 0;
        try
        {
            foreach (GridViewRow row1 in gridView1.Rows)
            {
                Label hdrid1 = (Label)row1.Cells[1].FindControl("lbl_hdrid");
                Label lgrid1 = (Label)row1.Cells[1].FindControl("lbl_lgrid");
                Label feecat1 = (Label)row1.Cells[1].FindControl("lbl_feecat");
                Label concession1 = (Label)row1.Cells[1].FindControl("lbl_Concess");
                Label paid1 = (Label)row1.Cells[1].FindControl("lbl_paid");
                double paidAmt = 0;
                double.TryParse(Convert.ToString(paid1.Text), out paidAmt);
                if (paidAmt != 0)
                {
                    //string Upd = " update FT_FinDailyTransaction set IsCanceled='1' where App_No='" + appno + "'";
                    //string UpdDaily = " update FT_FinDailyTransaction set credit='" + paidAmt + "' where App_No='" + appNo + "' and headerfk='" + hdrid1.Text + "' and ledgerfk='" + lgrid1.Text + "'  and FeeCategory='" + feecat1.Text + "'";
                    //int updpaid = d2.update_method_wo_parameter(UpdDaily, "Text");
                    getOldPayment(appNo, hdrid1.Text, lgrid1.Text, feecat1.Text, Rcptno, paidAmt.ToString(), transdate);
                    updtCnt++;
                }
            }
        }
        catch { }
        return updtCnt;
    }

    protected string headerValue()
    {
        string hedgid = string.Empty;
        ArrayList arrcpt = new ArrayList();
        foreach (GridViewRow hdrow in gridView2.Rows)
        {
            Label hdrid = (Label)hdrow.Cells[1].FindControl("lbl_hdrid");
            if (hedgid == "")
            {
                hedgid = Convert.ToString(hdrid.Text);
                arrcpt.Add(Convert.ToString(hdrid.Text));
            }
            else
            {
                if (!arrcpt.Contains(hdrid.Text))
                {
                    hedgid = hedgid + "'" + "," + "'" + Convert.ToString(hdrid.Text);
                    arrcpt.Add(Convert.ToString(hdrid.Text));
                }
            }
        }
        return hedgid;
    }
    protected void applynAndRegistUpdate(string degcode, string seatype, string appno, string ddl_colg, string colCode, string batch, string sem, string sec, string _roll_no)
    {
        //applyn update
        string AppUpd = "update applyn set degree_code='" + degcode + "',seattype='" + seatype + "' where app_no='" + appno + "'";
        int Aup = d2.update_method_wo_parameter(AppUpd, "Text");
        if (rbl_AdmitTransfer.SelectedItem.Value == "Not Applied")
        {
            string ApUpd = "update applyn set college_code='" + ddl_colg + "' where app_no='" + appno + "'";
            int Ap = d2.update_method_wo_parameter(ApUpd, "Text");
        }
        //Update Registration table
        if (ddladmis.SelectedItem.Text.Trim() == "After Admission")
        {
            string upReg = " update Registration set degree_code='" + degcode + "', college_code=" + ddl_colg + ", batch_year=" + batch + ",Current_Semester='" + sem + "',Sections='" + sec + "',Roll_No='" + _roll_no + "' where App_No=" + appno + "  ";
            d2.update_method_wo_parameter(upReg, "Text");
            if (rbl_AdmitTransfer.SelectedItem.Value == "Not Applied")
            {
                string ApUpd = "update Registration set college_code='" + ddl_colg + "' where app_no='" + appno + "'";
                int Ap = d2.update_method_wo_parameter(ApUpd, "Text");
            }
        }


    }
    protected bool transfer(string app_no, string olddeg, string deptcode, string oldsec, string sec, string oldcolg, string chngeClgCode, string batch, string fstSeat, string sndSeat, StringBuilder sbOldRecptDate, StringBuilder sbOldRecptCode, double oldAmt, StringBuilder sbNewRecptDate, StringBuilder sbNewRecptCode, double newAmt, double newExcessAmt, string oldRoll, string oldReg, string oldRollAdmit, string studAdmDate)
    {
        bool save = false;
        try
        {
            if (sbOldRecptDate.Length > 0)
                sbOldRecptDate.Remove(sbOldRecptDate.Length - 1, 1);
            if (sbOldRecptCode.Length > 0)
                sbOldRecptCode.Remove(sbOldRecptCode.Length - 1, 1);

            if (sbNewRecptDate.Length > 0)
                sbNewRecptDate.Remove(sbNewRecptDate.Length - 1, 1);
            if (sbNewRecptCode.Length > 0)
                sbNewRecptCode.Remove(sbNewRecptCode.Length - 1, 1);

            //string oldRoll = string.Empty;
            //string oldReg = string.Empty;
            //string oldRollAdmit = string.Empty;
            //string studAdmDate = string.Empty;
            //string selQReg = " select roll_no,reg_no,roll_admit,adm_date from registration where app_no='" + app_no + "'";
            //DataSet dsReg = d2.select_method_wo_parameter(selQReg, "Text");
            //if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
            //{
            //    oldRoll = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_no"]);
            //    oldReg = Convert.ToString(dsReg.Tables[0].Rows[0]["reg_no"]);
            //    oldRollAdmit = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_admit"]);
            //    studAdmDate = Convert.ToString(dsReg.Tables[0].Rows[0]["adm_date"]);
            //}
            string transferDate = Convert.ToString(txt_date.Text.Split('/')[1] + "/" + txt_date.Text.Split('/')[0] + "/" + txt_date.Text.Split('/')[2]);
            string insQ = "  insert into ST_Student_Transfer(AppNo,TransferDate,TransferTime,FromDegree,Todegree,FromSection,ToSection,FromCollege,Tocollege,FromSeatType,ToSeatType) values('" + app_no + "','" + transferDate + "','" + DateTime.Now.ToShortTimeString() + "','" + olddeg + "','" + deptcode + "','" + oldsec + "','" + sec + "','" + oldcolg + "','" + chngeClgCode + "','" + fstSeat + "','" + sndSeat + "')";
            int ins = d2.update_method_wo_parameter(insQ, "Text");
            if (ins > 0)
            {
                string StudPK = d2.GetFunction("select studentTransferPK from ST_Student_Transfer where AppNo='" + app_no + "' and TransferDate='" + transferDate + "' and FromDegree='" + olddeg + "' and FromSection='" + oldsec + "' and FromCollege='" + oldcolg + "' and FromSeatType='" + fstSeat + "'");
                if (StudPK != "0")
                {
                    string insStudDetails = " insert into st_student_transfer_details(studentTransferfK,old_rollno,Old_RegNo,Old_RollAdmit,stud_admDate,Old_ReceiptNo,Old_ReceiptDate,Old_Amt,New_ReceiptNo,New_ReceiptDate,New_Amt,New_ExcessAmt) values('" + StudPK + "','" + oldRoll + "','" + oldReg + "','" + oldRollAdmit + "','" + studAdmDate + "','" + Convert.ToString(sbOldRecptCode) + "','" + Convert.ToString(sbOldRecptDate) + "','" + oldAmt + "','" + Convert.ToString(sbNewRecptCode) + "','" + Convert.ToString(sbNewRecptDate) + "','" + newAmt + "','" + newExcessAmt + "')";
                    int inss = d2.update_method_wo_parameter(insStudDetails, "Text");
                    save = true;
                }
            }
        }
        catch { }
        return save;
    }

    protected void transferWithoutFees()
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
                stcollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            StringBuilder sbOldRecptDate = new StringBuilder();
            StringBuilder sbOldRecptCode = new StringBuilder();
            double oldAmt = 0;
            StringBuilder sbNewRecptDate = new StringBuilder();
            StringBuilder sbNewRecptCode = new StringBuilder();
            double newAmt = 0;
            double newExcessAmt = 0;
            string oldRoll = string.Empty;
            string oldReg = string.Empty;
            string oldRollAdmit = string.Empty;
            string studAdmDate = string.Empty;
            string date = "";
            string currrentdate = "";
            string curdatetime = DateTime.Now.ToString();
            DateTime dt = new DateTime();
            string[] strdatetime = curdatetime.Split(' ');
            if (strdatetime.Length > 0)
            {
                date = strdatetime[0].ToString();
                dt = Convert.ToDateTime(date);
                string[] curdate = date.Split('/');
                currrentdate = curdate[1].ToString() + "/" + curdate[0].ToString() + "/" + curdate[2].ToString();
            }
            string Currenttime = Convert.ToString(DateTime.Now.ToLongTimeString());
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                #region applied students
                if (txt_roll1.Text.Trim() != "" && txt_roll.Text.Trim() != "")
                {
                    bool updateOK = false;
                    string rollno = Convert.ToString(txt_roll1.Text);
                    string _roll_no = txt_roll_no1.Text.Trim();
                    if (string.IsNullOrEmpty(_roll_no))
                        _roll_no = rollno;
                    string appno = "";
                    string batch = "";
                    string degree = "";
                    string dept = "";
                    string degcode = "";
                    string stream = "";
                    string sem = "";
                    string sec = "";
                    string colCode = "";
                    string name = "";
                    string query = "select a.parent_name,a.stud_name, a.Stud_Type,c.Course_Name,dt.Dept_Name,a.degree_code,a.Current_Semester  ,a.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,a.app_no,a.seattype   from applyn a ,Degree d,course c,Department dt,collinfo co where  a.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code  and a.app_formno='" + rollno + "' and d.college_code='" + collegecode + "'";
                    ds1 = d2.select_method_wo_parameter(query, "Text");
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        name = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                        batch = Convert.ToString(ds1.Tables[0].Rows[0]["Batch_Year"]);
                        degree = Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]);
                        dept = Convert.ToString(ds1.Tables[0].Rows[0]["Dept_Name"]);
                        // sec = Convert.ToString( ds1.Tables[0].Rows[0]["Sections"]);
                        sec = txt_sec1.Text.Trim();
                        sem = Convert.ToString(ds1.Tables[0].Rows[0]["Current_Semester"]);
                        colCode = Convert.ToString(ds1.Tables[0].Rows[0]["college_code"]);
                        stream = Convert.ToString(ds1.Tables[0].Rows[0]["type"]);
                        degcode = Convert.ToString(ds1.Tables[0].Rows[0]["degree_code"]);
                        appno = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]);
                        string sndSeat = Convert.ToString(ds1.Tables[0].Rows[0]["seattype"]);
                        DateTime transdate = Convert.ToDateTime(txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2]);
                        if (!string.IsNullOrEmpty(appno) && !string.IsNullOrEmpty(degcode))
                        {
                            //Update Registration table
                            string updateApp = "update applyn set admission_status =1 where app_no ='" + appno + "'";
                            d2.update_method_wo_parameter(updateApp, "Text");
                            updateOK = true;
                            if (ddladmis.SelectedItem.Text.Trim() == "After Admission")
                            {
                                string selQReg = " select roll_no,reg_no,roll_admit,adm_date from registration where app_no='" + appno + "'";
                                DataSet dsReg = d2.select_method_wo_parameter(selQReg, "Text");
                                if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
                                {
                                    oldRoll = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_no"]);
                                    oldReg = Convert.ToString(dsReg.Tables[0].Rows[0]["reg_no"]);
                                    oldRollAdmit = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_admit"]);
                                    studAdmDate = Convert.ToString(dsReg.Tables[0].Rows[0]["adm_date"]);
                                }
                                string updateReg = "update Registration set DelFlag=1 where Roll_No='" + txt_roll.Text.Trim() + "'";
                                d2.update_method_wo_parameter(updateReg, "Text");

                                string insReg = "  insert into Registration (App_No,Adm_Date,Roll_Admit,Roll_No,RollNo_Flag,Reg_No,Stud_Name,Batch_Year,degree_code,college_code,CC,DelFlag,Exam_Elg,mode,Current_Semester,Sections)values ('" + appno + "','" + transdate.ToString("MM/dd/yyyy") + "','" + rollno + "','" + _roll_no + "','1','" + rollno + "','" + name + "','" + batch + "','" + degcode + "','" + colCode + "','0','0','OK',3,1,'" + sec + "')";
                                d2.update_method_wo_parameter(insReg, "Text");
                            }
                            //new insert to studentransfer table
                            string fstClgcode = Convert.ToString(lbltempfstclg.Text);
                            string fstBatchYr = Convert.ToString(txt_batch.Text);
                            string fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
                            string fstSection = Convert.ToString(txt_sec.Text);
                            string fstSeat = Convert.ToString(txt_seattype.Text);
                            string fstSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlcollege.SelectedValue + "'  and textval='" + fstSeat.Trim() + "'"));
                            transfer(appno, fstDegreecode, degcode, fstSection, sec, fstClgcode, colCode, batch, fstSeatCode, sndSeat, sbOldRecptDate, sbOldRecptCode, oldAmt, sbNewRecptDate, sbNewRecptCode, newAmt, newExcessAmt, oldRoll, oldReg, oldRollAdmit, studAdmDate);
                            UpdateAdmissionNo(appno);
                        }
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Student Details Not Found";
                    }
                    if (updateOK)
                    {
                        txt_roll1.Text = "";
                        txt_roll_no1.Text = "";
                        txt_roll_no.Text = "";
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Transferred Sucessfully";
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Not Transferred";
                    }
                }
                #endregion
            }
            else
            {
                #region Not applied students
                if (txt_roll.Text.Trim() != "")
                {
                    bool updateOK = false;
                    string rollno = Convert.ToString(txt_roll.Text);
                    string _roll_no = txt_roll_no.Text.Trim();
                    if (string.IsNullOrEmpty(_roll_no))
                        _roll_no = rollno;
                    string appno = "";
                    string batch = "";
                    string degcode = "";
                    string sem = "";
                    string sec = "";
                    string colCode = "";
                    ds1 = notAppliedValues(rollno, collegecode1);
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ddl_batch.Items.Count > 0)
                            batch = Convert.ToString(ddl_batch.SelectedItem.Value);
                        if (ddl_sec.Items.Count > 0)
                            sec = Convert.ToString(ddl_sec.SelectedItem.Value);
                        if (ddl_sem.Items.Count > 0)
                            sem = Convert.ToString(ddl_sem.SelectedItem.Value);
                        if (ddl_colg.Items.Count > 0)
                            colCode = Convert.ToString(ddl_colg.SelectedItem.Value);
                        if (ddl_dept.Items.Count > 0)
                            degcode = Convert.ToString(ddl_dept.SelectedItem.Value);
                        if (ddl_seattype.Items.Count > 0)
                            seatype = Convert.ToString(ddl_seattype.SelectedItem.Value);
                        appno = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]);
                        DateTime transdate = Convert.ToDateTime(txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2]);
                        string curtime = DateTime.Now.ToShortTimeString();
                        string finYearid = d2.getCurrentFinanceYear(usercode, colCode);
                        if (!string.IsNullOrEmpty(degcode) && !string.IsNullOrEmpty(seatype) && !string.IsNullOrEmpty(appno))
                        {
                            string selQReg = " select roll_no,reg_no,roll_admit,adm_date from registration where app_no='" + appno + "'";
                            DataSet dsReg = d2.select_method_wo_parameter(selQReg, "Text");
                            if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
                            {
                                oldRoll = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_no"]);
                                oldReg = Convert.ToString(dsReg.Tables[0].Rows[0]["reg_no"]);
                                oldRollAdmit = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_admit"]);
                                studAdmDate = Convert.ToString(dsReg.Tables[0].Rows[0]["adm_date"]);
                            }
                            applynAndRegistUpdate(degcode, seatype, appno, ddl_colg.SelectedItem.Value, colCode, batch, sem, sec, _roll_no);
                            //new insert to studentransfer table
                            string fstClgcode = Convert.ToString(lbltempfstclg.Text);
                            string fstBatchYr = Convert.ToString(txt_batch.Text);
                            string fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
                            string fstSection = Convert.ToString(txt_sec.Text);
                            string fstSeat = Convert.ToString(txt_seattype.Text);
                            string fstSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlcollege.SelectedValue + "'  and textval='" + fstSeat.Trim() + "'"));
                            transfer(appno, fstDegreecode, ddl_dept.SelectedItem.Value, fstSection, sec, fstClgcode, ddl_colg.SelectedItem.Value, batch, fstSeatCode, seatype, sbOldRecptDate, sbOldRecptCode, oldAmt, sbNewRecptDate, sbNewRecptCode, newAmt, newExcessAmt, oldRoll, oldReg, oldRollAdmit, studAdmDate);
                            UpdateAdmissionNo(appno);
                            updateOK = true;
                        }
                    }
                    if (updateOK)
                    {
                        txt_tramt.Text = "";
                        txt_roll_no1.Text = "";
                        txt_roll_no.Text = "";
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Transferred Sucessfully";
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Not Transferred";
                    }
                }
                #endregion
            }
        }
        catch { }
    }

    protected bool validateTransferCheck()
    {
        bool check = false;
        try
        {
            string fstClgcode = string.Empty;
            string fstBatchYr = string.Empty;
            string fstDegreecode = string.Empty;
            string fstSection = string.Empty;
            string fstSeat = string.Empty;

            string sndClgcode = string.Empty;
            string sndBatchYr = string.Empty;
            string sndDegreecode = string.Empty;
            string sndSection = string.Empty;
            string sndSeat = string.Empty;

            fstClgcode = Convert.ToString(lbltempfstclg.Text);
            fstBatchYr = Convert.ToString(txt_batch.Text);
            fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
            fstSection = Convert.ToString(txt_sec.Text);
            fstSeat = Convert.ToString(txt_seattype.Text);

            string applno = string.Empty;
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                sndClgcode = Convert.ToString(lbltempsndclg.Text);
                sndBatchYr = Convert.ToString(txt_batch1.Text);
                sndDegreecode = Convert.ToString(lbltempsnddeg.Text);
                sndSection = Convert.ToString(txt_sec1.Text);
                sndSeat = Convert.ToString(txt_seat_type1.Text);
            }
            else
            {
                if (ddl_colg.Items.Count > 0)
                    sndClgcode = Convert.ToString(ddl_colg.SelectedItem.Value);
                if (ddl_batch.Items.Count > 0)
                    sndBatchYr = Convert.ToString(ddl_batch.SelectedItem.Value);
                if (ddl_dept.Items.Count > 0)
                    sndDegreecode = Convert.ToString(ddl_dept.SelectedItem.Value);
                if (ddl_sec.Items.Count > 0)
                    sndSection = Convert.ToString(ddl_sec.SelectedItem.Value);
                if (ddl_seattype.Items.Count > 0)
                    sndSeat = Convert.ToString(ddl_seattype.SelectedItem.Text);

            }

            if (!string.IsNullOrEmpty(fstClgcode) && !string.IsNullOrEmpty(fstBatchYr) && !string.IsNullOrEmpty(fstDegreecode) && !string.IsNullOrEmpty(sndClgcode)
&& !string.IsNullOrEmpty(sndBatchYr) && !string.IsNullOrEmpty(sndDegreecode))
            {

                if (fstClgcode == sndClgcode)
                {
                    if (fstBatchYr == sndBatchYr)
                    {
                        if (fstDegreecode == sndDegreecode)
                        {
                            if (!string.IsNullOrEmpty(fstSection) && !string.IsNullOrEmpty(sndSection))
                            {
                                if (fstSection != sndSection)
                                    check = true;
                                else
                                {
                                    if (fstSeat == sndSeat)
                                        check = false;
                                    else
                                        check = true;
                                }
                            }
                            else if (!string.IsNullOrEmpty(fstSection) && string.IsNullOrEmpty(sndSection))
                                check = true;
                            else if (string.IsNullOrEmpty(fstSection) && !string.IsNullOrEmpty(sndSection))
                                check = true;
                            else if (string.IsNullOrEmpty(fstSection) && string.IsNullOrEmpty(sndSection))
                            {
                                if (fstSeat == sndSeat)
                                    check = false;
                                else
                                    check = true;
                            }
                        }
                        else
                            check = true;
                    }
                    else
                        check = false;
                }
                else
                    check = true;
            }
        }
        catch { }
        return check;

    }

    #region RecieptNo Generate

    public string generateReceiptNo(string hdrs, ref Dictionary<string, string> dtrcpt, ref Dictionary<string, string> arRcptfk)
    {
        int isHeaderwise = 0;
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
            Session["isHeaderwise"] = isHeaderwise;
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
            return getHeaderwiseReceiptNo(hdrs, ref dtrcpt, ref arRcptfk);
        }
    }
    private string getCommonReceiptNo()
    {
        string recno = string.Empty;
        //lblaccid.Text = "";
        //lstrcpt.Text = "";
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            //   lblaccid.Text = accountid;
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
                Session["acronym"] = recacr;
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
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    private string getHeaderwiseReceiptNo(string hdrs, ref Dictionary<string, string> dtrcpt, ref Dictionary<string, string> arRcptfk)
    {
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            string hdrSetPK = string.Empty;
            DataSet dsFinHedDet = new DataSet();
            DataView dvcode = new DataView();
            string isheaderFk = hdrs;
            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string selQ = string.Empty;
            selQ = "select distinct HeaderSettingFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode1 + " and FinyearFK=" + finYearid + " and isnull(onlineRec,'')<>'1'";
            selQ += "select distinct HeaderSettingFk, headerfk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode1 + " and FinyearFK=" + finYearid + " and isnull(onlineRec,'')<>'1'";
            dsFinHedDet = d2.select_method_wo_parameter(selQ, "Text");
            if (dsFinHedDet.Tables.Count > 0 && dsFinHedDet.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFinHedDet.Tables[0].Rows.Count; i++)
                {
                    hdrSetPK = Convert.ToString(dsFinHedDet.Tables[0].Rows[i][0]).Trim();
                    string secondreciptqurey = "select * from FM_HeaderFinCodeSettings where HeaderSettingPK =" + Convert.ToString(hdrSetPK) + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1'";
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
                        if (!string.IsNullOrEmpty(recno))
                        {
                            dsFinHedDet.Tables[1].DefaultView.RowFilter = "HeaderSettingFK='" + hdrSetPK + "'";
                            dvcode = dsFinHedDet.Tables[1].DefaultView;
                            if (dvcode.Count > 0)
                            {
                                for (int row = 0; row < dvcode.Count; row++)
                                {
                                    if (!dtrcpt.ContainsKey(Convert.ToString(dvcode[row]["headerfk"])))
                                        dtrcpt.Add(Convert.ToString(dvcode[row]["headerfk"]), Convert.ToString(recno));
                                }
                            }
                        }
                        if (!arRcptfk.ContainsKey(recno))
                            arRcptfk.Add(recno, hdrSetPK + "-" + recacr);
                    }
                }
            }
            if (dtrcpt.Count > 0)
                recno = string.Empty;
            return recno;
        }
        catch (Exception ex) { return recno; }
    }


    #endregion

    #region journal no generate

    public string generateJournalNo(string hdrs, string collegecode1)
    {
        int isHeaderwise = 0;
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
            Session["isHeaderwise"] = isHeaderwise;
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
        return getCommonJournalNo();
    }
    private string getCommonJournalNo()
    {
        string recno = string.Empty;
        //lblaccid.Text = "";
        //lstrcpt.Text = "";
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            //   lblaccid.Text = accountid;
            string secondreciptqurey = "SELECT JournalStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
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
                string acronymquery = d2.GetFunction("SELECT JournalAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;
                Session["acronym"] = recacr;
                int size = Convert.ToInt32(d2.GetFunction("SELECT  JournalSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));
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
        catch (Exception ex) { return recno; }
    }
    #endregion

    protected void btn_refund_Click(object sender, EventArgs e)
    {
        if (txt_rerollno.Text.Trim() != "")
        {

            if (rb_refund.Checked == true)
                refundMethod();
            else if (rb_discont.Checked == true)
                divReuseRoll.Visible = true;

        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Enter Roll Number";
        }
    }
    private void refundMethod()
    {
        try
        {
            int check = 0;
            double refuntaken = 0;
            bool Amt = false;
            foreach (GridViewRow grid in gridView3.Rows)
            {
                TextBox refuntakens = (TextBox)grid.FindControl("txt_refund");
                double.TryParse(Convert.ToString(refuntakens.Text), out refuntaken);
                if (refuntaken != 0)
                    Amt = true;
            }

            string appno = "";
            string rollno = txt_rerollno.Text.Trim();
            if (Amt == true)
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                    {
                        appno = d2.GetFunction("select app_no from Registration where roll_no='" + rollno + "' and cc<>'1' and DelFlag<>1 and college_code='" + collegecode1 + "'");
                        check = 0;
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                    {
                        appno = d2.GetFunction("select app_no from Registration where reg_no='" + rollno + "' and cc<>'1' and DelFlag<>1 and college_code='" + collegecode1 + "'");
                        check = 0;
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                    {
                        appno = d2.GetFunction("select app_no from Registration where Roll_admit='" + rollno + "' and cc<>'1' and DelFlag<>1 and college_code='" + collegecode1 + "'");
                        check = 0;
                    }
                }
                //  if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
                else
                {
                    appno = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' and Admission_Status=1 and selection_status=1 and IsConfirm=1 and college_code='" + collegecode1 + "'");
                    check = 1;
                    // appno = "  and a.app_formno  = '" + rollno + "' ";
                }
                //if (Session["clgcode"] != null)
                //    collegecode1 = Convert.ToString(Session["clgcode"]);
                //else
                //    collegecode1 = Convert.ToString(Session["collegecode"]);
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
                }
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                string[] dtsplit = txt_rdate.Text.Split('/');
                DateTime dtdate = Convert.ToDateTime(dtsplit[1] + "/" + dtsplit[0] + "/" + dtsplit[2]);
                if (appno != "" && appno != "0")
                {
                    if (txt_reamt.Text.Trim() == "")
                        txt_reamt.Text = "0";
                    string upExcess = " if exists (select * from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2' ) update FT_ExcessDet set ExcessAmt =ExcessAmt +'" + txt_reamt.Text + "',BalanceAmt =BalanceAmt +'" + txt_reamt.Text + "' where App_No ='" + appno + "' and ExcessType ='2'  else  INSERT INTO FT_ExcessDet (ExcessTransDate,TransTime ,DailyTransCode,App_No ,MemType ,ExcessType ,ExcessAmt,AdjAmt,BalanceAmt,FinYearFK ) VALUES ('" + dtdate + "','" + DateTime.Now.ToLongTimeString() + "' ,'', " + appno + " , 1 , 2 ," + txt_reamt.Text + ",0," + txt_reamt.Text + "," + finYearid + ")";
                    d2.update_method_wo_parameter(upExcess, "Text");

                    string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2'");
                    foreach (GridViewRow rows in gridView3.Rows)
                    {
                        Label hdrid = (Label)rows.Cells[10].FindControl("lbl_hdrid");
                        Label lgrid = (Label)rows.Cells[10].FindControl("lbl_lgrid");
                        Label feecatid = (Label)rows.Cells[10].FindControl("lbl_feecat");
                        Label paidamt = (Label)rows.Cells[10].FindControl("lbl_paid");
                        TextBox txtAmt = (TextBox)rows.Cells[10].FindControl("txt_refund");
                        TextBox txtrefundAmt = (TextBox)rows.Cells[10].FindControl("txt_refundbal");

                        if (txtrefundAmt.Text != "" && txtrefundAmt.Text != "0")
                        {
                            string upRefundQ = " update FT_FeeAllot set RefundAmount= ISNull(RefundAmount,0)+ " + txtrefundAmt.Text + ", IsRefund='1' where app_no=" + appno + " and HeaderFK=" + hdrid.Text + " and Ledgerfk=" + lgrid.Text + " and FeeCategory=" + feecatid.Text + "";
                            d2.update_method_wo_parameter(upRefundQ, "Text");

                            upExcess = "if exists ( select * from FT_ExcessLedgerDet where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "')update FT_ExcessLedgerDet set ExcessAmt  =ExcessAmt +'" + txtrefundAmt.Text + "',BalanceAmt =BalanceAmt +'" + txtrefundAmt.Text + "' where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK) values ('" + hdrid.Text + "','" + lgrid.Text + "','" + txtrefundAmt.Text + "','" + txtrefundAmt.Text + "','" + getvalue + "')";
                            d2.update_method_wo_parameter(upExcess, "Text");
                        }
                    }
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Refunded Sucessfully";
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Already Refunded";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Enter The Refund Amount";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void btnReuseYes_Click(object sender, EventArgs e)
    {
        divReuseRoll.Visible = false;
        if (cbdisWithoutFees.Checked)
            getDiscontinue(true);
        else
            discontinueMethod(true);
    }
    protected void btnReuseNo_Click(object sender, EventArgs e)
    {
        divReuseRoll.Visible = false;
        //discontinueMethod(false);
    }
    private void discontinueMethod(bool reuseRollNo)
    {
        try
        {

            int check = 0;
            string appno = "";
            double refuntaken = 0;
            bool Amt = false;
            foreach (GridViewRow grid in gridView3.Rows)
            {
                TextBox refuntakens = (TextBox)grid.FindControl("txt_refund");
                double.TryParse(Convert.ToString(refuntakens.Text), out refuntaken);
                if (refuntaken != 0)
                    Amt = true;
            }
            string rollno = txt_rerollno.Text.Trim();
            if (Amt == true)
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                    {
                        appno = d2.GetFunction("select app_no from Registration where roll_no='" + rollno + "' and cc<>'1' and DelFlag<>1 and college_code='" + collegecode1 + "'");
                        check = 0;
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                    {
                        appno = d2.GetFunction("select app_no from Registration where reg_no='" + rollno + "' and cc<>'1' and DelFlag<>1 and college_code='" + collegecode1 + "'");
                        check = 0;
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                    {
                        appno = d2.GetFunction("select app_no from Registration where Roll_admit='" + rollno + "' and cc<>'1' and DelFlag<>1 and college_code='" + collegecode1 + "'");
                        check = 0;
                    }
                }
                //  if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
                else
                {
                    appno = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' and Admission_Status=1 and selection_status=1 and IsConfirm=1 and college_code='" + collegecode1 + "'");
                    check = 1;
                    // appno = "  and a.app_formno  = '" + rollno + "' ";
                }
                //if (Session["clgcode"] != null)
                //    collegecode1 = Convert.ToString(Session["clgcode"]);
                //else
                //    collegecode1 = Convert.ToString(Session["collegecode"]);
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
                }
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                string[] dtsplit = txt_rdate.Text.Split('/');
                DateTime dtdate = Convert.ToDateTime(dtsplit[1] + "/" + dtsplit[0] + "/" + dtsplit[2]);
                if (appno != "" && appno != "0")
                {
                    //update registration                   
                    //if (value == 1)
                    //{
                    string critcode = d2.GetFunction("select criteria_Code  from selectcriteria where app_no='" + appno + "'");
                    string degreecode = d2.GetFunction("select degree_Code  from applyn where app_no='" + appno + "'");
                    //string upRegQ = " update applyn set Admission_Status=0,selection_status=0 where app_no='" + appno + "'";
                    //d2.update_method_wo_parameter(upRegQ, "Text");
                    string upq = " update registration set  DelFlag=1 where app_no='" + appno + "' ";
                    upq = upq + " update applyn set Admission_Status='2',AdmitedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appno + "' ";
                    if (reuseRollNo)
                    {
                        upq = upq + " update registration set roll_no=roll_admit where app_no='" + appno + "' ";
                    }
                    d2.update_method_wo_parameter(upq, "Text");

                    // Criteria code update
                    string CrUpd = "update selectcriteria set admit_confirm='0' where app_no='" + appno + "'";
                    int crup = d2.update_method_wo_parameter(CrUpd, "Text");
                    //admitcolumn update
                    string Adupd = "update admitcolumnset set allot_Confirm =allot_Confirm -1 where setcolumn ='" + degreecode + "' and column_name ='" + critcode + "'";
                    int admit = d2.update_method_wo_parameter(Adupd, "Text");
                    //}
                    if (txt_reamt.Text.Trim() == "")
                        txt_reamt.Text = "0";
                    string upExcess = " if exists (select * from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2' ) update FT_ExcessDet set ExcessAmt =ExcessAmt +'" + txt_reamt.Text + "',BalanceAmt =BalanceAmt +'" + txt_reamt.Text + "' where App_No ='" + appno + "' and ExcessType ='2'  else  INSERT INTO FT_ExcessDet (ExcessTransDate,TransTime ,DailyTransCode,App_No ,MemType ,ExcessType ,ExcessAmt,AdjAmt,BalanceAmt,FinYearFK ) VALUES ('" + dtdate + "','" + DateTime.Now.ToLongTimeString() + "' ,'', " + appno + " , 1 , 2 ," + txt_reamt.Text + ",0," + txt_reamt.Text + "," + finYearid + ")";
                    d2.update_method_wo_parameter(upExcess, "Text");

                    string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2'");
                    foreach (GridViewRow rows in gridView3.Rows)
                    {
                        Label hdrid = (Label)rows.Cells[10].FindControl("lbl_hdrid");
                        Label lgrid = (Label)rows.Cells[10].FindControl("lbl_lgrid");
                        Label feecatid = (Label)rows.Cells[10].FindControl("lbl_feecat");
                        Label paidamt = (Label)rows.Cells[10].FindControl("lbl_paid");
                        TextBox txtAmt = (TextBox)rows.Cells[10].FindControl("txt_refund");
                        TextBox txtrefundAmt = (TextBox)rows.Cells[10].FindControl("txt_refundbal");

                        if (txtrefundAmt.Text != "" && txtrefundAmt.Text != "0")
                        {
                            string upRefundQ = " update FT_FeeAllot set RefundAmount= ISNull(RefundAmount,0)+ " + txtrefundAmt.Text + ", IsRefund='1' where app_no=" + appno + " and HeaderFK=" + hdrid.Text + " and Ledgerfk=" + lgrid.Text + " and FeeCategory=" + feecatid.Text + "";
                            d2.update_method_wo_parameter(upRefundQ, "Text");

                            upExcess = "if exists ( select * from FT_ExcessLedgerDet where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "')update FT_ExcessLedgerDet set ExcessAmt  =ExcessAmt +'" + txtrefundAmt.Text + "',BalanceAmt =BalanceAmt +'" + txtrefundAmt.Text + "' where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK) values ('" + hdrid.Text + "','" + lgrid.Text + "','" + txtrefundAmt.Text + "','" + txtrefundAmt.Text + "','" + getvalue + "')";
                            d2.update_method_wo_parameter(upExcess, "Text");
                        }
                    }
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Discontinued Sucessfully";
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Already Discontinued";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Enter The Amount";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void getDiscontinue(bool reuseRollNo)
    {
        try
        {
            bool boolCheck = false;
            string appno = string.Empty;
            string rollno = Convert.ToString(txt_rerollno.Text);
            if (!string.IsNullOrEmpty(rollno))
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                    {
                        appno = d2.GetFunction("select app_no from Registration where roll_no='" + rollno + "' and cc<>'1' and DelFlag<>1 and college_code='" + collegecode1 + "'"); ;
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                    {
                        appno = d2.GetFunction("select app_no from Registration where reg_no='" + rollno + "' and cc<>'1' and DelFlag<>1 and college_code='" + collegecode1 + "'");
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                    {
                        appno = d2.GetFunction("select app_no from Registration where Roll_admit='" + rollno + "' and cc<>'1' and DelFlag<>1 and college_code='" + collegecode1 + "'");
                    }
                }
                //  if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
                else
                {
                    appno = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' and Admission_Status=1 and selection_status=1 and IsConfirm=1 and college_code='" + collegecode1 + "'");
                }
                if (!string.IsNullOrEmpty(appno) && appno != "0")
                {
                    string critcode = d2.GetFunction("select criteria_Code  from selectcriteria where app_no='" + appno + "'");
                    string degreecode = d2.GetFunction("select degree_Code  from applyn where app_no='" + appno + "'");
                    string upq = " update registration set  DelFlag=1 where app_no='" + appno + "' ";
                    upq = upq + " update applyn set Admission_Status='2',AdmitedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appno + "' ";
                    upq = upq + " update registration set roll_no=roll_admit where app_no='" + appno + "' ";
                    d2.update_method_wo_parameter(upq, "Text");
                    // Criteria code update
                    string CrUpd = "update selectcriteria set admit_confirm='0' where app_no='" + appno + "'";
                    int crup = d2.update_method_wo_parameter(CrUpd, "Text");
                    //admitcolumn update
                    string Adupd = "update admitcolumnset set allot_Confirm =allot_Confirm -1 where setcolumn ='" + degreecode + "' and column_name ='" + critcode + "'";
                    int admit = d2.update_method_wo_parameter(Adupd, "Text");
                    boolCheck = true;

                }
                if (boolCheck)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Discontinued Sucessfully";
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Discontinued Failed";
                }

            }
        }
        catch { }
    }
    public void loadfromsetting()
    {
        try
        {
            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(lst4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    // txt_roll.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    // txt_roll.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    // txt_roll.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    // txt_roll.Text = "App No";
                    chosedmode = 3;
                    break;
            }



        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }

    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_roll.Text = "";
            txt_name.Text = "";
            txt_colg.Text = "";
            txt_strm.Text = "";
            txt_batch.Text = "";
            txt_degree.Text = "";
            txt_dept.Text = "";
            txt_sem.Text = "";
            txt_sec.Text = "";
            txt_seattype.Text = "";
            image2.ImageUrl = "";
            bindFromGrid();

            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {

                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    //  rbl_rollno.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    // rbl_rollno.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    // rbl_rollno.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    // rbl_rollno.Text = "App No";
                    chosedmode = 3;
                    break;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    public void loadrefundsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");
            ListItem lst5 = new ListItem("Smartcard No", "4");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rerollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rerollno.Items.Add(list1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rerollno.Items.Add(list2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rerollno.Items.Add(list3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rerollno.Items.Add(list4);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptSmartNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode1 + ") ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Smartcard No - smart_serial_no
                rbl_rollno.Items.Add(lst5);
            }
            int smartDisp = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' --and college_code in (" + collegecode1 + ")").Trim());

            if (rbl_rerollno.Items.Count == 0)
            {
                rbl_rerollno.Items.Add(list1);
            }
            switch (Convert.ToUInt32(rbl_rerollno.SelectedItem.Value))
            {
                case 0:
                case1:
                    txt_rerollno.Attributes.Add("placeholder", "Roll No");
                    // txt_roll.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                case2:
                    txt_rerollno.Attributes.Add("placeholder", "Reg No");
                    // txt_roll.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                case3:
                    txt_rerollno.Attributes.Add("placeholder", "Admin No");
                    // txt_roll.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                case4:
                    txt_rerollno.Attributes.Add("placeholder", "App No");
                    // txt_roll.Text = "App No";
                    chosedmode = 3;
                    break;
                case 4:
                    txt_rerollno.Attributes.Add("placeholder", "Smartcard No");
                    //txt_roll.Text = "SmartCard No";
                    chosedmode = 4;
                    switch (smartDisp)
                    {
                        case 0:
                            goto case1;
                        case 1:
                            goto case2;
                        case 2:
                            goto case3;
                        case 3:
                            goto case4;
                    }
                    break;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void rbl_rerollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_rerollno.Text = "";
        txt_rename.Text = "";
        txt_recolg.Text = "";
        txt_rebatch.Text = "";
        txt_redegree.Text = "";
        txt_resem.Text = "";
        txt_restrm.Text = "";
        txt_redept.Text = "";
        txt_resec.Text = "";
        txt_AmtPerc.Text = "";
        chk_refCommon.Checked = false;
        txt_reamt.Text = "";
        image3.ImageUrl = "";
        bindGrid2();
        switch (Convert.ToUInt32(rbl_rerollno.SelectedItem.Value))
        {
            case 0:
                txt_rerollno.Attributes.Add("Placeholder", "Roll No");
                chosedmode = 0;
                break;
            case 1:
                txt_rerollno.Attributes.Add("Placeholder", "Reg No");
                chosedmode = 1;
                break;
            case 2:
                txt_rerollno.Attributes.Add("Placeholder", "Admin No");
                chosedmode = 2;
                break;
            case 3:
                txt_rerollno.Attributes.Add("Placeholder", "App No");
                chosedmode = 3;
                break;
        }
    }
    protected void rb_admit_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }
    //Multiple Transfer
    public void bindType1()
    {
        try
        {
            lbl_stream1.Text = useStreamShift();
            ddl_strm1.Items.Clear();
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>''  and r.college_code='" + Convert.ToString(ddl_college1.SelectedValue) + "'  order by type asc";

            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm1.DataSource = ds;
                ddl_strm1.DataTextField = "type";
                ddl_strm1.DataValueField = "type";
                ddl_strm1.DataBind();
                ddl_strm1.Enabled = true;
            }
            else
            {
                ddl_strm1.Enabled = false;
            }
        }
        catch (Exception ex) { }
    }
    public void bindbatch1()
    {
        try
        {
            ddl_batch1.Items.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            DataSet ds = DA.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch1.DataSource = ds;
                ddl_batch1.DataTextField = "batch_year";
                ddl_batch1.DataValueField = "batch_year";
                ddl_batch1.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    public void binddegree1()
    {
        try
        {
            ddl_degree1.Items.Clear();
            string stream = "";
            stream = ddl_strm1.Items.Count > 0 ? ddl_strm1.SelectedValue : "";

            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + Convert.ToString(ddl_college1.SelectedValue) + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + userCode + " ";
            if (ddl_strm1.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_degree1.DataSource = ds;
                ddl_degree1.DataTextField = "course_name";
                ddl_degree1.DataValueField = "course_id";
                ddl_degree1.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    public void bindbranch1()
    {
        try
        {
            ddl_branch1.Items.Clear();
            string degree = "";
            degree = ddl_degree1.Items.Count > 0 ? ddl_degree1.SelectedValue : "";


            string commname = "";
            if (degree != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code ,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + degree + "') and deptprivilages.Degree_code=degree.Degree_code order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000))  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
            }

            DataSet ds = DA.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_branch1.DataSource = ds;
                ddl_branch1.DataTextField = "dept_name";
                ddl_branch1.DataValueField = "degree_code";
                ddl_branch1.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    public void bindsem1()
    {
        try
        {
            ddl_sem1.Items.Clear();

            int duration = 0;
            int i = 0;

            string batch = "";
            batch = ddl_batch1.Items.Count > 0 ? ddl_batch1.SelectedValue : "0";
            string branch = "";
            branch = ddl_branch1.Items.Count > 0 ? ddl_branch1.SelectedValue : "0";

            if (branch.Trim() != "" && batch.Trim() != "")
            {
                string strsql1 = "select distinct duration,first_year_nonsemester  from degree where degree_code in ('" + branch + "') and college_code='" + Convert.ToString(ddl_college1.SelectedValue) + "'";
                DataSet ds = DA.select_method_wo_parameter(strsql1, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string dur = Convert.ToString(ds.Tables[0].Rows[i][0]);
                        if (dur.Trim() != "")
                        {
                            if (duration < Convert.ToInt32(dur))
                            {
                                duration = Convert.ToInt32(dur);
                            }
                        }
                    }
                }
                if (duration != 0)
                {
                    for (i = 1; i <= duration; i++)
                    {
                        ddl_sem1.Items.Add(Convert.ToString(i));
                    }
                }
            }
        }
        catch { }
    }
    public void bindclg1()
    {
        try
        {
            ddl_college1.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code";
            DataSet ds = DA.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_college1.DataSource = ds;
                ddl_college1.DataTextField = "collname";
                ddl_college1.DataValueField = "college_code";
                ddl_college1.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    private string useStreamShift()
    {
        string useStrShft = "Stream";
        string streamcode = DA.GetFunction("select value from Master_Settings where settings='Stream/Shift Rights' and usercode='" + userCode + "'").Trim();

        if (streamcode == "" || streamcode == "0")
        {
            useStrShft = "Stream";
        }
        if (streamcode.Trim() == "1")
        {
            useStrShft = "Shift";
        }
        if (streamcode.Trim() == "2")
        {
            useStrShft = "Stream";
        }
        return useStrShft;
    }
    protected void ddl_college1_OnIndexChange(object sender, EventArgs e)
    {
        bindType1();
        bindbatch1();
        binddegree1();
        bindbranch1();
        bindsem1();
        btn_go1_Click(sender, e);
    }
    protected void ddl_batch1_OnIndexChange(object sender, EventArgs e)
    {
        bindType1();
        binddegree1();
        bindbranch1();
        bindsem1();
        btn_go1_Click(sender, e);
    }
    protected void ddl_strm1_OnIndexChange(object sender, EventArgs e)
    {
        binddegree1();
        bindbranch1();
        bindsem1();
        btn_go1_Click(sender, e);
    }
    protected void ddl_degree1_OnIndexChange(object sender, EventArgs e)
    {
        bindbranch1();
        bindsem1();
        btn_go1_Click(sender, e);
    }
    protected void ddl_branch1_OnIndexChange(object sender, EventArgs e)
    {
        bindsem1();
        btn_go1_Click(sender, e);
    }
    protected void ddl_sem1_OnIndexChange(object sender, EventArgs e)
    {
        btn_go1_Click(sender, e);
    }
    protected void ddl_stType1_OnIndexChange(object sender, EventArgs e)
    {
        btn_go1_Click(sender, e);
    }
    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            btn_TransferMulti.Visible = false;
            tblToTransMulti.Visible = false;

            lbl_errormsg1.Visible = false;
            lbl_Total1.Visible = false;

            string selectquery;

            string branch = ddl_branch1.Items.Count > 0 ? ddl_branch1.SelectedValue : "";

            string degCode = ddl_degree1.Items.Count > 0 ? ddl_degree1.SelectedValue : "";

            string stream = ddl_strm1.Enabled ? ddl_strm1.Items.Count > 0 ? ddl_strm1.SelectedItem.Text.Trim() : "" : "";

            string batch_year = ddl_batch1.Items.Count > 0 ? ddl_batch1.SelectedItem.Text : "";

            string cusem = ddl_sem1.Items.Count > 0 ? ddl_sem1.SelectedItem.Text : "";

            string stdType = string.Empty;
            switch (ddl_stType1.SelectedItem.Text.ToUpper())
            {
                case "APPLIED":
                    stdType = "  and isnull(r.isconfirm,'0')='1' ";
                    break;
                case "SHORTLISTED":
                    stdType = "  and isnull(r.isconfirm,'0')='1' and isnull(r.selection_status,'0')='1'  ";
                    break;
                case "WAIT TO ADMIT":
                    stdType = "  and isnull(r.isconfirm,'0')='1' and isnull(r.selection_status,'0')='1' and isnull(r.admission_status,'0')='1' ";
                    break;
                case "ADMITTED":
                    break;
            }

            DataSet ds = new DataSet();
            if (batch_year != string.Empty && degCode != string.Empty && branch != string.Empty && cusem != string.Empty)
            {
                if (stream != string.Empty)
                {
                    stream = " and c.type in ('" + stream + "')";
                }
                selectquery = "select app_formno,'' Roll_No,'' Roll_Admit,'' smart_serial_no,Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,'' Reg_No,r.App_No,c.type,'' Sections   from applyn r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and Batch_Year =" + batch_year + " and r.degree_code in ('" + branch + "')  and r.current_semester in('" + cusem + "')  " + stream + stdType + " ";
                if (stdType == string.Empty)
                {
                    selectquery = "select app_formno,rg.Roll_No,rg.Roll_Admit,rg.smart_serial_no,rg.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,rg.Reg_No,r.App_No,c.type,isnull(rg.Sections,'') as Sections   from applyn r,registration rg,Degree d,Department dt,Course c where r.app_no=rg.app_no and  r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and rg.Batch_Year =" + batch_year + " and rg.degree_code in ('" + branch + "')  and rg.current_semester in('" + cusem + "')  " + stream + "  and isnull(r.isconfirm,'0')='1' and isnull(r.selection_status,'0')='1' and isnull(r.admission_status,'0')='1' ";
                }
                ds = DA.select_method_wo_parameter(selectquery, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                spreadStudAdd.Sheets[0].RowCount = 1;
                spreadStudAdd.Sheets[0].ColumnCount = 0;
                spreadStudAdd.Sheets[0].ColumnHeader.RowCount = 1;
                spreadStudAdd.CommandBar.Visible = false;
                spreadStudAdd.Sheets[0].ColumnCount = 9;

                spreadStudAdd.Sheets[0].RowHeader.Visible = false;
                spreadStudAdd.Sheets[0].AutoPostBack = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                spreadStudAdd.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[0].Locked = true;
                spreadStudAdd.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[1].Width = 60;
                spreadStudAdd.Sheets[0].Columns[1].Locked = false;
                spreadStudAdd.Sheets[0].Cells[0, 1].CellType = chkall;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Application Number";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[2].Locked = true;
                spreadStudAdd.Columns[2].Width = 150;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[3].Locked = true;
                spreadStudAdd.Columns[3].Width = 100;
                spreadStudAdd.Sheets[0].Columns[3].Visible = false;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[4].Locked = true;
                spreadStudAdd.Columns[4].Width = 100;
                spreadStudAdd.Sheets[0].Columns[4].Visible = false;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Smartcard No";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[5].Locked = true;
                spreadStudAdd.Columns[5].Width = 100;
                spreadStudAdd.Sheets[0].Columns[5].Visible = false;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Name";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[6].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                spreadStudAdd.Sheets[0].Columns[6].Locked = true;
                spreadStudAdd.Columns[6].Width = 300;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].Text = lbldeg.Text;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                spreadStudAdd.Sheets[0].Columns[7].Locked = true;
                spreadStudAdd.Sheets[0].Columns[7].Visible = false;

                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Section";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[8].Font.Name = "Book Antiqua";
                spreadStudAdd.Sheets[0].Columns[8].Font.Size = FontUnit.Medium;
                spreadStudAdd.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                spreadStudAdd.Sheets[0].Columns[8].Locked = true;
                spreadStudAdd.Columns[8].Width = 60;
                spreadStudAdd.Sheets[0].Columns[8].Visible = false;

                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtSmartno = new FarPoint.Web.Spread.TextCellType();

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    spreadStudAdd.Sheets[0].RowCount++;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);

                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 1].CellType = check;

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 2].CellType = txtRollAd;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["App_formNo"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 3].CellType = txtRollno;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 4].CellType = txtRegno;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 5].CellType = txtSmartno;
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["smart_serial_no"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);

                    spreadStudAdd.Sheets[0].Cells[spreadStudAdd.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["Sections"]);
                }
                spreadStudAdd.Visible = true;
                spreadStudAdd.Sheets[0].PageSize = spreadStudAdd.Sheets[0].RowCount;

                spreadStudAdd.Height = 300;
                spreadStudAdd.SaveChanges();
                btn_TransferMulti.Visible = true;
                tblToTransMulti.Visible = true;

                lbl_Total1.Visible = true;
                lbl_Total1.Text = "Total Number Of Students : " + (spreadStudAdd.Sheets[0].RowCount - 1);
            }
            else
            {
                spreadStudAdd.Visible = false;
                lbl_errormsg1.Visible = true;
                lbl_errormsg1.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            spreadStudAdd.Visible = false;
            lbl_errormsg1.Visible = true;
            lbl_errormsg1.Text = "No Records Found"; DA.sendErrorMail(ex, Convert.ToString(ddl_college1.SelectedValue), "SectionAllocation");
        }
    }
    protected void btn_TransferMulti_Click(object sender, EventArgs e)
    {
        try
        {
            int saved = 0;
            int notsaved = 0;

            string batchYr = ddlbatch.Items.Count > 0 ? ddlbatch.SelectedItem.Value : "0";
            string degreeCode = ddl_branch.Items.Count > 0 ? ddl_branch.SelectedItem.Value : "0";
            string clgCode = ddl_college.Items.Count > 0 ? ddl_college.SelectedItem.Value : "0";
            string semester = ddlsem.Items.Count > 0 ? ddlsem.SelectedItem.Value : "1";

            if (batchYr != "0" && degreeCode != "0" && clgCode != "0")
            {
                List<string> appNoList;
                List<string> rollnoList;
                if (checkedOK(out appNoList, out rollnoList))
                {
                    for (int apI = 0; apI < appNoList.Count; apI++)
                    {
                        string appNo = appNoList[apI];
                        string rollNo = rollnoList[apI];

                        string upQ = "update applyn set Batch_Year =" + batchYr + " , degree_code ='" + degreeCode + "'  , college_code='" + clgCode + "',current_semester='" + semester + "'  where app_no='" + appNo + "' ";

                        string stdType = string.Empty;
                        if (ddl_stType1.SelectedItem.Text.ToUpper() == "ADMITTED")
                        {
                            stdType = "  update registration set Batch_Year =" + batchYr + " , degree_code ='" + degreeCode + "'  , college_code='" + clgCode + "',current_semester='" + semester + "'  where app_no='" + appNo + "' ";
                        }

                        int upOk = 0;
                        try
                        {
                            upOk = DA.update_method_wo_parameter(upQ + stdType, "Text");
                        }
                        catch { }
                        finally
                        {
                            if (upOk > 0)
                            {
                                saved++;
                            }
                            else
                            {
                                notsaved++;
                            }
                        }
                    }
                    imgAlert.Visible = true;
                    lbl_alert.Text = String.Format("Transferred : " + saved + ". \t\n Not Transferred : " + notsaved);
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Select Students";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select College, Batch And Branch";
            }
        }
        catch (Exception ex) { DA.sendErrorMail(ex, Convert.ToString(ddl_college1.SelectedValue), "SectionAllocation.aspx"); }
        btn_go1_Click(sender, e);
    }
    public bool checkedOK(out List<string> appNoList, out List<string> rollnoList)
    {
        bool Ok = false;
        appNoList = new List<string>();
        rollnoList = new List<string>();
        spreadStudAdd.SaveChanges();
        for (int i = 1; i < spreadStudAdd.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(spreadStudAdd.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                Ok = true;
                appNoList.Add(Convert.ToString(spreadStudAdd.Sheets[0].Cells[i, 0].Tag));
                rollnoList.Add(Convert.ToString(spreadStudAdd.Sheets[0].Cells[i, 3].Text));
            }
        }
        return Ok;
    }
    protected void spreadStudAdd_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = spreadStudAdd.Sheets[0].ActiveRow.ToString();
            string actcol = spreadStudAdd.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (spreadStudAdd.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(spreadStudAdd.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < spreadStudAdd.Sheets[0].RowCount; i++)
                        {
                            spreadStudAdd.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < spreadStudAdd.Sheets[0].RowCount; i++)
                        {
                            spreadStudAdd.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { }
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
        catch (Exception ex) { ddl_college.Items.Clear(); }
    }
    public void bindType()
    {
        try
        {
            lbl_stream.Text = useStreamShift();
            ddlstrm.Items.Clear();
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>''  and r.college_code='" + Convert.ToInt32(ddl_college.SelectedItem.Value) + "'  order by type asc";

            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlstrm.DataSource = ds;
                ddlstrm.DataTextField = "type";
                ddlstrm.DataValueField = "type";
                ddlstrm.DataBind();
                ddlstrm.Enabled = true;
            }
            else
            {
                ddlstrm.Enabled = false;
            }
        }
        catch (Exception ex) { }
    }
    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            DataSet ds = DA.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    public void binddegree()
    {
        try
        {
            ddldegree.Items.Clear();
            //cbl_degree.Items.Clear();
            //txt_degree.Text = "Degree";
            //cb_degree.Checked = true;
            string stream = "";
            stream = ddlstrm.Items.Count > 0 ? ddlstrm.SelectedValue : "";

            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + Convert.ToInt32(ddl_college.SelectedItem.Value) + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + userCode + " ";
            if (ddl_strm.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

                //cbl_degree.DataSource = ds;
                //cbl_degree.DataTextField = "course_name";
                //cbl_degree.DataValueField = "course_id";
                //cbl_degree.DataBind();
                //CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, "Degree");
            }
        }
        catch (Exception ex) { }
    }
    public void bindbranch()
    {
        try
        {
            ddl_branch.Items.Clear();
            //cbl_branch.Items.Clear();
            //txt_branch.Text = "Branch";
            //cb_branch.Checked = true;
            string degree = "";
            degree = ddldegree.Items.Count > 0 ? ddldegree.SelectedValue : ""; //GetSelectedItemsValueAsString(cbl_degree);


            string commname = "";
            if (degree != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + degree + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym,department.dept_code  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }

            DataSet ds = DA.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_branch.DataSource = ds;
                ddl_branch.DataTextField = "dept_name";
                ddl_branch.DataValueField = "degree_code";
                ddl_branch.DataBind();
                //cbl_branch.DataSource = ds;
                //cbl_branch.DataTextField = "dept_name";
                //cbl_branch.DataValueField = "degree_code";
                //cbl_branch.DataBind();
                //CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, "Branch");
            }
        }
        catch (Exception ex) { }
    }
    public void bindseme()
    {
        try
        {
            ddlsem.Items.Clear();
            //cbl_sem.Items.Clear();
            //cb_sem.Checked = true;
            //txt_sem.Text = "Semester";

            int duration = 0;
            int i = 0;

            string branch = "";
            string batch = "";

            branch = Convert.ToString(ddl_branch.SelectedItem.Value);//GetSelectedItemsValueAsString(cbl_branch);

            batch = Convert.ToString(ddlbatch.SelectedItem.Value);

            if (branch.Trim() != "" && batch.Trim() != "")
            {
                string strsql1 = "select distinct duration,first_year_nonsemester  from degree where degree_code in ('" + branch + "') and college_code='" + Convert.ToInt32(ddl_college.SelectedItem.Value) + "'";
                DataSet ds = DA.select_method_wo_parameter(strsql1, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string dur = Convert.ToString(ds.Tables[0].Rows[i][0]);
                        if (dur.Trim() != "")
                        {
                            if (duration < Convert.ToInt32(dur))
                            {
                                duration = Convert.ToInt32(dur);
                            }
                        }
                    }
                }
                if (duration != 0)
                {
                    for (i = 1; i <= duration; i++)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                        //cbl_sem.Items.Add(Convert.ToString(i));
                    }
                    //CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, "Semester");
                }
            }
        }
        catch { }
    }
    protected void ddl_college_OnIndexChange(object sender, EventArgs e)
    {
        bindType();
        bindbatch();
        binddegree();
        bindbranch();
        bindseme();
    }
    protected void ddl_batch_OnIndexChange(object sender, EventArgs e)
    {
        bindType();
        binddegree();
        bindbranch();
        bindseme();
    }
    protected void ddl_strm_OnIndexChange(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
        bindseme();
    }
    protected void ddl_degree_OnIndexChange(object sender, EventArgs e)
    {
        bindbranch();
        bindseme();
    }
    protected void ddl_branch_OnIndexChange(object sender, EventArgs e)
    {
        bindseme();
    }
    //Last modified by Idhris  -- 30-07-2016

    //added by sudhagar 09-08-2016
    protected void ddladmis_Selected(object sender, EventArgs e)
    {
        if (ddladmis.SelectedItem.Text.Trim() == "Before Admission")
        {
            rbl_rollno.Items.Clear();
            ListItem lst = new ListItem("App No", "3");
            rbl_rollno.Items.Add(lst);
            txt_roll.Attributes.Add("placeholder", "App No");
            chosedmode = 3;
            admis = 1;
            cleargridview1();
            cleargridview2();
        }
        else
        {
            admis = 2;
            loadfromsetting();
            cleargridview1();
            cleargridview2();
        }
    }

    #region ledger mapping

    //link button
    protected void lnkledgmap_Click(object sender, EventArgs e)
    {
        loadFromcollege();
        loadTocollege();
        loadFromLedger();
        loadToLedger();
        divledger.Visible = true;
        lstfrom.Items.Clear();
        lstto.Items.Clear();
        loadequalledger();


    }

    //same ledger checked
    protected void loadequalledger()
    {
        try
        {

            string fcldcode = Convert.ToString(ddlfrclg.SelectedItem.Value);
            string tocldcode = Convert.ToString(ddlfrclg.SelectedItem.Value);
            collegecode = ddltoclg.SelectedItem.Value.ToString();
            List<ListItem> list = new List<ListItem>();
            if (fcldcode == tocldcode)
            {
                cbltoledg.Items.Clear();
                string fledgcode = Convert.ToString(ddlfrledg.SelectedItem.Value);
                // string selq = "select ledgerpk,ledgername from fm_ledgermaster where collegecode='" + collegecode + "' order by isnull(priority,1000), ledgerName asc";

                //added by sudhagar
                string selq = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + "  order by isnull(l.priority,1000), l.ledgerName asc ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[sel]["ledgerpk"]) != fledgcode)
                        {
                            cbltoledg.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[sel]["ledgername"]), Convert.ToString(ds.Tables[0].Rows[sel]["ledgerpk"])));
                        }
                    }
                    if (cbltoledg.Items.Count > 0)
                    {
                        for (i = 0; i < cbltoledg.Items.Count; i++)
                        {
                            cbltoledg.Items[i].Selected = true;
                        }
                        txttoledg.Text = "Ledger(" + cbltoledg.Items.Count + ")";
                        cbtoledg.Checked = true;
                    }
                }
            }
        }
        catch { }
    }

    protected void ddlfrclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddlfrclg.SelectedItem.Value.ToString();
            loadTocollege();
            loadFromLedger();
            loadToLedger();

        }
        catch { }
    }
    protected void ddltoclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddltoclg.SelectedItem.Value.ToString();
            loadToLedger();
        }
        catch { }
    }

    protected void ddlfrledg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        loadequalledger();
    }

    protected void cbtoledg_ChekedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cbtoledg, cbltoledg, txttoledg, "Ledger", "--Select--");
    }
    protected void cbltoledgSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbtoledg, cbltoledg, txttoledg, "Ledger", "--Select--");
    }

    public void loadFromcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlfrclg.DataSource = ds;
                ddlfrclg.DataTextField = "collname";
                ddlfrclg.DataValueField = "college_code";
                ddlfrclg.DataBind();
            }
        }
        catch
        { }
    }

    public void loadTocollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltoclg.DataSource = ds;
                ddltoclg.DataTextField = "collname";
                ddltoclg.DataValueField = "college_code";
                ddltoclg.DataBind();
            }
        }
        catch
        { }
    }

    public void loadFromLedger()
    {
        try
        {
            collegecode = ddlfrclg.SelectedItem.Value.ToString();
            // string selq = "select ledgerpk,ledgername from fm_ledgermaster where collegecode='" + collegecode + "' order by isnull(priority,1000),ledgerName asc";

            //added by sudhagar 09-05-2016
            string selq = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + " order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlfrledg.DataSource = ds;
                ddlfrledg.DataTextField = "ledgername";
                ddlfrledg.DataValueField = "ledgerpk";
                ddlfrledg.DataBind();
            }
        }
        catch
        { }
    }

    public void loadToLedger()
    {
        try
        {
            collegecode = ddltoclg.SelectedItem.Value.ToString();
            //  string selq = "select ledgerpk,ledgername from fm_ledgermaster where collegecode='" + collegecode + "' order by isnull(priority,1000),ledgerName asc";
            string selq = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + "  order by len(isnull(l.priority,1000)) , l.priority asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbltoledg.DataSource = ds;
                cbltoledg.DataTextField = "ledgername";
                cbltoledg.DataValueField = "ledgerpk";
                cbltoledg.DataBind();
                if (cbltoledg.Items.Count > 0)
                {
                    for (i = 0; i < cbltoledg.Items.Count; i++)
                    {
                        cbltoledg.Items[i].Selected = true;
                    }
                    txttoledg.Text = "Ledger(" + cbltoledg.Items.Count + ")";
                    cbtoledg.Checked = true;
                }
            }
        }
        catch { }
    }

    //button go
    protected void btnledggo_Click(object sender, EventArgs e)
    {
        loadfromlist();
    }

    //from list load
    protected void loadfromlist()
    {
        try
        {
            lstfrom.Items.Clear();
            string frledgid = Convert.ToString(ddlfrledg.SelectedItem.Value);
            string fcldcode = Convert.ToString(ddlfrclg.SelectedItem.Value);
            if (frledgid != "")
            {
                string Selid = d2.GetFunction(" select MatchingLedger from fm_ledgermaster where ledgerpk='" + frledgid + "' and collegecode='" + fcldcode + "' order by isnull(priority,1000),ledgerName asc");
                if (Selid != "0" && Selid != "")
                {
                    string[] ledgid = Selid.Split(',');
                    if (ledgid.Length > 0)
                    {
                        lstto.Items.Clear();
                        for (int row = 0; row < ledgid.Length; row++)
                        {
                            string ledgername = d2.GetFunction(" select ledgername from fm_ledgermaster where ledgerpk='" + ledgid[row] + "' and collegecode='" + fcldcode + "'");
                            ListItem lstfr = new ListItem(ledgername, ledgid[row]);
                            lstto.Items.Add(lstfr);
                        }
                    }
                }
                else
                {
                    lstto.Items.Clear();
                }

                for (int sel = 0; sel < cbltoledg.Items.Count; sel++)
                {
                    if (cbltoledg.Items[sel].Selected == true)
                    {
                        ListItem lstfr = new ListItem(cbltoledg.Items[sel].Text, cbltoledg.Items[sel].Value);
                        lstfrom.Items.Add(lstfr);
                        // lstfrom.SelectedItem.Value = frledgid;
                    }
                }
            }
        }
        catch { }
    }

    protected void btnledgsave_Click(object sender, EventArgs e)
    {
        saveDetials();
    }

    protected void saveDetials()
    {
        try
        {
            bool save = false;
            if (lstto.Items.Count > 0)
            {
                string ledid = "";
                string fcldcode = Convert.ToString(ddlfrclg.SelectedItem.Value);
                string tocldcode = Convert.ToString(ddlfrclg.SelectedItem.Value);
                string fromledg = Convert.ToString(ddlfrledg.SelectedItem.Value);
                for (int sel = 0; sel < lstto.Items.Count; sel++)
                {
                    if (ledid == "")
                        ledid = lstto.Items[sel].Value.ToString();
                    else
                        ledid = ledid + "," + lstto.Items[sel].Value.ToString();
                }
                if (ledid != "")
                {
                    string InsQ = "update fm_ledgermaster set MatchingLedger= '" + ledid + "' where ledgerpk='" + fromledg + "' and collegecode='" + fcldcode + "'";
                    int ins = d2.update_method_wo_parameter(InsQ, "Text");
                    save = true;
                }
                if (save == true)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved Sucessfully";
                }

            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Any One Ledger";
            }
        }
        catch { }
    }
    protected void btnledgcancel_Click(object sender, EventArgs e)
    {
        divledger.Visible = false;
    }

    #region list
    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lstfrom.Items.Count > 0 && lstfrom.SelectedItem.Value != "")
            {
                for (int j = 0; j < lstto.Items.Count; j++)
                {
                    if (lstto.Items[j].Value == lstfrom.SelectedItem.Value)
                    {
                        ok = false;
                    }

                }
                if (ok)
                {
                    ListItem lst = new ListItem(lstfrom.SelectedItem.Text, lstfrom.SelectedItem.Value);
                    lstto.Items.Add(lst);
                }
            }
        }
        catch { }
    }
    protected void btnMvTwoRt_Click(object sender, EventArgs e)
    {
        try
        {
            lstto.Items.Clear();
            if (lstfrom.Items.Count > 0)
            {
                for (int j = 0; j < lstfrom.Items.Count; j++)
                {
                    ListItem lst = new ListItem(lstfrom.Items[j].Text.ToString(), lstfrom.Items[j].Value.ToString());
                    lstto.Items.Add(lst);
                }
            }
        }
        catch { }
    }
    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        try
        {
            if (lstto.Items.Count > 0 && lstto.SelectedItem.Value != "")
            {
                lstto.Items.RemoveAt(lstto.SelectedIndex);
            }
        }
        catch { }
    }
    protected void btnMvTwoLt_Click(object sender, EventArgs e)
    {
        try
        {
            lstto.Items.Clear();
        }
        catch { }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        divledger.Visible = false;
    }
    #endregion


    //select ledgerpk,ledgername from fm_ledgermaster where collegecode=13
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

    //last added by sudhagar 22/08
    #region individual mapping
    //link button
    protected void lnkindivmap_Click(object sender, EventArgs e)
    {
        divindi.Visible = true;
        divind.Visible = true;
        bindHeaderind();
        bindLedgerind();
        bindGridInd();
        bindGrid5Ind();
        txtamtind.Text = "";
        btntransind.Enabled = false;
    }

    //img button
    protected void ImageButton2_Click(object sender, EventArgs e)
    {
        divindi.Visible = false;
    }

    protected void ddlhedind_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindLedgerind();
    }
    protected void btntransind_Click(object sender, EventArgs e)
    {
        //bindLedgerind();btntransind_Click
        studTrnsAfterledgerMapping();
    }

    protected void studTrnsAfterledgerMapping()
    {
        try
        {
            #region Not Applied students
            if (txt_roll.Text.Trim() != "")
            {
                ArrayList htCheckVal = new ArrayList();
                ArrayList NewhtCheckVal = new ArrayList();
                StringBuilder sbOldRecptDate = new StringBuilder();
                StringBuilder sbOldRecptCode = new StringBuilder();
                double oldAmt = 0;
                StringBuilder sbNewRecptDate = new StringBuilder();
                StringBuilder sbNewRecptCode = new StringBuilder();
                double newAmt = 0;
                double newExcessAmt = 0;
                string oldRoll = string.Empty;
                string oldReg = string.Empty;
                string oldRollAdmit = string.Empty;
                string studAdmDate = string.Empty;
                string rollno = Convert.ToString(txt_roll.Text);
                string _roll_no = txt_roll_no.Text.Trim();
                if (string.IsNullOrEmpty(_roll_no))
                    _roll_no = rollno;
                string appno = "";
                string batch = "";
                string degcode = "";
                string sem = "";
                string sec = "";
                string colCode = "";
                string Rcptno = "";
                string query = "select app_no,Stud_Name,Batch_Year,degree_code,college_code,Current_Semester,Sections from Registration where college_code='" + ddlcollege.SelectedValue + "'";
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                        query = query + " and  Roll_no='" + rollno + "'";
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        query = query + " and Reg_No='" + rollno + "' ";
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        query = query + " and Roll_Admit='" + rollno + "'";
                }
                else
                {
                    query = "select app_no,Stud_Name,Batch_Year,degree_code,college_code,Current_Semester,'' Sections from applyn where app_formno='" + rollno + "' and college_code='" + ddlcollege.SelectedValue + "'";
                }
                ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    if (ddl_batch.Items.Count > 0)
                        batch = Convert.ToString(ddl_batch.SelectedItem.Value);
                    if (ddl_sec.Items.Count > 0)
                        sec = Convert.ToString(ddl_sec.SelectedItem.Value);
                    if (ddl_sem.Items.Count > 0)
                        sem = Convert.ToString(ddl_sem.SelectedItem.Value);
                    if (ddl_colg.Items.Count > 0)
                        colCode = Convert.ToString(ddl_colg.SelectedItem.Value);
                    if (ddl_dept.Items.Count > 0)
                        degcode = Convert.ToString(ddl_dept.SelectedItem.Value);
                    if (ddl_seattype.Items.Count > 0)
                        seatype = Convert.ToString(ddl_seattype.SelectedItem.Value);
                    appno = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]);
                    DateTime transdate = Convert.ToDateTime(txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2]);
                    string curtime = DateTime.Now.ToShortTimeString();
                    string finYearid = d2.getCurrentFinanceYear(usercode, colCode);

                    bool updateOK = false;
                    if (colCode != "" && batch != "" && degcode != "" && appno != "")
                    {
                        //applyn update
                        string AppUpd = "update applyn set degree_code='" + degcode + "',seattype='" + seatype + "' where app_no='" + appno + "'";
                        int Aup = d2.update_method_wo_parameter(AppUpd, "Text");
                        if (rbl_AdmitTransfer.SelectedItem.Value == "Not Applied")
                        {
                            string ApUpd = "update applyn set college_code='" + ddl_colg.SelectedItem.Value + "' where app_no='" + appno + "'";
                            int Ap = d2.update_method_wo_parameter(ApUpd, "Text");
                        }
                        //Update Registration table
                        if (ddladmis.SelectedItem.Text.Trim() == "After Admission")
                        {

                            string selQReg = " select roll_no,reg_no,roll_admit,adm_date from registration where app_no='" + appno + "'";
                            DataSet dsReg = d2.select_method_wo_parameter(selQReg, "Text");
                            if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
                            {
                                oldRoll = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_no"]);
                                oldReg = Convert.ToString(dsReg.Tables[0].Rows[0]["reg_no"]);
                                oldRollAdmit = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_admit"]);
                                studAdmDate = Convert.ToString(dsReg.Tables[0].Rows[0]["adm_date"]);
                            }
                            string upReg = " update Registration set degree_code='" + degcode + "', college_code=" + colCode + ", batch_year=" + batch + ",Current_Semester='" + sem + "',Sections='" + sec + "',Roll_No='" + _roll_no + "' where App_No=" + appno + "  ";
                            d2.update_method_wo_parameter(upReg, "Text");
                            if (rbl_AdmitTransfer.SelectedItem.Value == "Not Applied")
                            {
                                string ApUpd = "update Registration set college_code='" + ddl_colg.SelectedItem.Value + "' where app_no='" + appno + "'";
                                int Ap = d2.update_method_wo_parameter(ApUpd, "Text");
                            }
                        }
                        //new insert to studentransfer table
                        string fstClgcode = Convert.ToString(lbltempfstclg.Text);
                        string fstBatchYr = Convert.ToString(txt_batch.Text);
                        string fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
                        string fstSection = Convert.ToString(txt_sec.Text);
                        string fstSeat = Convert.ToString(txt_seattype.Text);
                        string fstSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlcollege.SelectedValue + "'  and textval='" + fstSeat.Trim() + "'"));

                        UpdateAdmissionNo(appno);
                        double paymode = 0;
                        string payledg = "";
                        string payfeecat = "";
                        string bankPK = "";
                        string challanType = "";
                        string Chalno = "";
                        foreach (GridViewRow gdrow in gridView4.Rows)
                        {
                            CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                            if (cb.Checked)
                            {
                                Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                                Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                                Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                                //ledgerid
                                if (payledg == "")
                                    payledg = Convert.ToString(lblledg.Text);
                                else
                                    payledg = payledg + "'" + "," + "'" + Convert.ToString(lblledg.Text);
                                payfeecat = lblfeecat.Text;

                            }
                        }
                        bankPK = d2.GetFunction(" select distinct BankFk from ft_challandet where ledgerfk in('" + payledg + "') and app_no='" + appno + "' ");
                        challanType = d2.GetFunction(" select distinct challanType from ft_challandet where ledgerfk in('" + payledg + "') and app_no='" + appno + "' ");
                        Chalno = d2.GetFunction(" select distinct challanno from ft_challandet where ledgerfk in('" + payledg + "') and app_no='" + appno + "'");
                        double.TryParse(Convert.ToString(d2.GetFunction("  select distinct paymode from ft_findailytransaction where app_no='" + appno + "' and feecategory='" + payfeecat + "' and ledgerfk in('" + payledg + "')")), out paymode);
                        string chlledgid = "";

                        string hedgid = ledgermappingheaderValue();
                        if (hedgid != "")
                            Rcptno = generateJournalNo(hedgid, colCode);
                        foreach (GridViewRow gdrow in gridView4.Rows)
                        {
                            CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                            if (cb.Checked)
                            {
                                Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                                Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                                Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                                Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                                Label lblpaid = (Label)gdrow.FindControl("lbl_paid");

                                //double.TryParse(Convert.ToString(lblpay.Text), out paymode);

                                //update daily transaction to cancel the receipt
                                //   string UpdDaily = " update FT_FinDailyTransaction set IsCanceled='1' where App_No='" + appno + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "'  and FeeCategory='" + lblfeecat.Text + "'";
                                //string UpdDaily = " update FT_FinDailyTransaction set credit='" + lblpaid.Text + "' where App_No='" + appno + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "'  and FeeCategory='" + lblfeecat.Text + "'";
                                if (lblpaid.Text != "" && lblpaid.Text != "0")
                                {
                                    getOldPayment(appno, lblhedg.Text, lblledg.Text, lblfeecat.Text, Rcptno, lblpaid.Text, transdate.ToString("MM/dd/yyyy"));
                                    //and paymode='" + lblpay.Text + "'
                                    //  d2.update_method_wo_parameter(UpdDaily, "Text");
                                    //  string selOldQ = " select distinct convert(varchar(10),transdate,103) as transdate,transcode,debit from FT_FinDailyTransaction  where App_No='" + appno + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "'  and FeeCategory='" + lblfeecat.Text + "' and  IsCanceled='1'";
                                    string selOldQ = " select distinct convert(varchar(10),transdate,103) as transdate,transcode,debit from FT_FinDailyTransaction  where App_No='" + appno + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "'  and FeeCategory='" + lblfeecat.Text + "' ";
                                    DataSet dsOld = d2.select_method_wo_parameter(selOldQ, "Text");
                                    if (dsOld.Tables.Count > 0 && dsOld.Tables[0].Rows.Count > 0)
                                    {
                                        for (int old = 0; old < dsOld.Tables[0].Rows.Count; old++)
                                        {
                                            if (!htCheckVal.Contains(Convert.ToString(dsOld.Tables[0].Rows[old]["transdate"])))
                                            {
                                                sbOldRecptDate.Append(Convert.ToString(dsOld.Tables[0].Rows[old]["transdate"]) + ",");
                                                htCheckVal.Add(Convert.ToString(dsOld.Tables[0].Rows[old]["transdate"]));
                                            }
                                            if (!htCheckVal.Contains(Convert.ToString(dsOld.Tables[0].Rows[old]["transcode"])))
                                            {
                                                sbOldRecptCode.Append(Convert.ToString(dsOld.Tables[0].Rows[old]["transcode"]) + ",");
                                                htCheckVal.Add(Convert.ToString(dsOld.Tables[0].Rows[old]["transcode"]));
                                            }
                                            double tempPaidAmt = 0;
                                            double.TryParse(Convert.ToString(dsOld.Tables[0].Rows[old]["debit"]), out tempPaidAmt);
                                            oldAmt += tempPaidAmt;
                                        }
                                    }
                                    //ledgerid
                                    if (chlledgid == "")
                                        chlledgid = Convert.ToString(lblledg.Text);
                                    else
                                        chlledgid = chlledgid + "'" + "," + "'" + Convert.ToString(lblledg.Text);

                                    string Chalnum = d2.GetFunction(" select distinct challanno from ft_challandet where ledgerfk in('" + chlledgid + "') and app_no='" + appno + "'");
                                    if (paymode == 4)
                                    {
                                        //to find challan no 
                                        string Delq = " delete from ft_challandet where app_no='" + appno + "' and ledgerfk in('" + lblledg.Text + "') and headerfk in('" + lblhedg.Text + "') and challanno='" + Chalnum + "' and feecategory in('" + lblfeecat.Text + "') --and finyearfk='" + finYearid + "'";
                                        int id = d2.update_method_wo_parameter(Delq, "Text");
                                    }
                                }
                            }
                        }
                        if (challanType == "0" || challanType == "")
                            challanType = "1";
                        // feeallot insert record
                        #region Allot insert
                        foreach (GridViewRow row in gridView5.Rows)
                        {
                            Label hdrid = (Label)row.Cells[1].FindControl("lbl_hdrid");
                            Label lgrid = (Label)row.Cells[1].FindControl("lbl_lgrid");
                            Label feecat = (Label)row.Cells[1].FindControl("lbl_feecat");
                            Label feeamt = (Label)row.Cells[1].FindControl("lbl_feeamt");
                            Label totamt = (Label)row.Cells[1].FindControl("lbl_totamt");
                            Label concession = (Label)row.Cells[1].FindControl("lbl_Concess");
                            TextBox paid = (TextBox)row.Cells[1].FindControl("txt_paid");
                            TextBox balance = (TextBox)row.Cells[1].FindControl("txt_bal");
                            TextBox excess = (TextBox)row.Cells[1].FindControl("txt_exGrid2");

                            if (feeamt.Text == "")
                                feeamt.Text = "0";
                            if (totamt.Text == "")
                                totamt.Text = "0";
                            if (concession.Text == "")
                                concession.Text = "0";
                            if (paid.Text == "")
                                paid.Text = "0";
                            if (balance.Text == "")
                                balance.Text = "0";
                            if (excess.Text == "")
                                excess.Text = "0";

                            string updateFeeallot = "if exists (select * from FT_FeeAllot where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "')) update FT_FeeAllot set AllotDate='" + transdate.ToString("MM/dd/yyyy") + "',FeeAmount='" + feeamt.Text + "',DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + totamt.Text + "',RefundAmount='0',IsFeeDeposit='1',PayMode='1',FeeCategory='" + feecat.Text + "',PaidStatus='0',DueAmount='0',FineAmount='0',BalAmount='" + balance.Text + "',paidamount='" + paid.Text + "' where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot (AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK,paidamount) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + lgrid.Text + "," + hdrid.Text + ",'" + feeamt.Text + "','0','0','0','" + totamt.Text + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + balance.Text + "'," + finYearid + ",'" + paid.Text + "')";
                            d2.update_method_wo_parameter(updateFeeallot, "Text");

                            if (row.RowIndex == gridView5.Rows.Count - 1)
                            {
                                string amt = "";
                                string ddlhdr = Convert.ToString(ddlhedind.SelectedItem.Value);
                                string ddllgr = Convert.ToString(ddlledind.SelectedItem.Value);
                                amt = Convert.ToString(txtamtind.Text.Trim());
                                if (ddlhdr != "" && ddllgr != "" && amt != "")
                                {
                                    string updateTransfer = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ddllgr + "') and HeaderFK in('" + ddlhdr + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "')) update FT_FeeAllot set AllotDate='" + transdate.ToString("MM/dd/yyyy") + "',FeeAmount=ISNULL(FeeAmount,'0')+'" + amt + "',DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount=ISNULL(TotalAmount,'0')+'" + amt + "',RefundAmount='0',IsFeeDeposit='1',PayMode='1',FeeCategory='" + feecat.Text + "',PaidStatus='0',DueAmount='0',FineAmount='0',BalAmount=ISNULL(BalAmount,'0')+'" + amt + "' where LedgerFK in('" + ddllgr + "') and HeaderFK in('" + ddlhdr + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + ddllgr + "," + ddlhdr + ",'" + amt + "','0','0','0','" + amt + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + amt + "'," + finYearid + ")";
                                    d2.update_method_wo_parameter(updateTransfer, "Text");
                                }

                            }
                            updateOK = true;
                        }

                        #endregion

                        Dictionary<string, string> dtReceipt = new Dictionary<string, string>();
                        Dictionary<string, string> arRcptfk = new Dictionary<string, string>();
                        string entryUserCode = d2.GetFunction(" select distinct entryusercode from FT_FinDailyTransaction where app_no='" + appno + "'");
                        if (paymode == 4)
                        {
                            #region Dailytransaction insert
                            //new receipt code generate

                            //if (hedgid != "")
                            //    Rcptno = generateReceiptNo(hedgid, ref dtReceipt, ref arRcptfk);

                            if (!string.IsNullOrEmpty(Rcptno) || dtReceipt.Count > 0)
                            {
                                foreach (GridViewRow row in gridView5.Rows)
                                {
                                    CheckBox cbsel = (CheckBox)row.FindControl("cblsell");
                                    if (cbsel.Checked)
                                    {
                                        Label hdrid = (Label)row.FindControl("lbl_hdrid");
                                        Label lgrid = (Label)row.FindControl("lbl_lgrid");
                                        Label feecat = (Label)row.FindControl("lbl_feecat");
                                        Label feeamt = (Label)row.FindControl("lbl_feeamt");
                                        Label totamt = (Label)row.FindControl("lbl_totamt");
                                        Label concession = (Label)row.FindControl("lbl_Concess");
                                        TextBox paid = (TextBox)row.FindControl("txt_paid");
                                        double totpaid = 0;
                                        double.TryParse(Convert.ToString(paid.Text), out totpaid);
                                        if (totpaid != 0 && totpaid != 0.00)
                                        {
                                            TextBox balance = (TextBox)row.FindControl("txt_bal");
                                            TextBox excess = (TextBox)row.FindControl("txt_exGrid2");
                                            if (feeamt.Text == "")
                                                feeamt.Text = "0";
                                            if (totamt.Text == "")
                                                totamt.Text = "0";
                                            if (concession.Text == "")
                                                concession.Text = "0";
                                            if (paid.Text == "")
                                                paid.Text = "0";
                                            if (balance.Text == "")
                                                balance.Text = "0";
                                            if (excess.Text == "")
                                                excess.Text = "0";

                                            if (dtReceipt.Count > 0)
                                                if (dtReceipt.ContainsKey(Convert.ToString(hdrid.Text)))
                                                    Rcptno = dtReceipt[hdrid.Text].ToString();
                                            //challandet inset record
                                            string insert = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType,isconfirmed,RcptTranscode,RcptTransDate) VALUES('" + Chalno + "','" + transdate.ToString("MM/dd/yyyy") + "'," + appno + "," + hdrid.Text + "," + feeamt.Text + "," + paid.Text + "," + feecat.Text + "," + finYearid + "," + bankPK + "," + lgrid.Text + "," + challanType + ",'1','" + Rcptno + "','" + transdate.ToString("MM/dd/yyyy") + "')";
                                            d2.update_method_wo_parameter(insert, "Text");


                                            //update for feeallot
                                            string updatQ = " update ft_feeallot set chltaken='" + paid.Text + "' where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "')";
                                            //and  FinYearFK='" + finYearid + "'
                                            d2.update_method_wo_parameter(updatQ, "Text");

                                            if (paid.Text != "0")
                                            {
                                                string selQy = "select distinct paymode from ft_findailytransaction where app_no='" + appno + "' and isnull(iscanceled,'0')='1' and debit='" + paid.Text + "'";
                                                string payMode = d2.GetFunction(selQy);
                                                payMode = "1";
                                                if (payMode != "0")
                                                {
                                                    //daily transaction
                                                    string INSdaily = "if exists(select * from FT_FinDailyTransaction where HeaderFK='" + hdrid.Text + "' and LedgerFK='" + lgrid.Text + "' and FeeCategory='" + feecat.Text + "' and TransCode='" + Rcptno + "' and App_No='" + appno + "' and FinYearFK='" + finYearid + "')update FT_FinDailyTransaction set Debit='" + paid.Text + "',TransDate='" + transdate.ToString("MM/dd/yyyy") + "',TransTime='" + curtime + "' ,IsCanceled='0',IsCollected='1',paymode ='" + payMode + "' where HeaderFK='" + hdrid.Text + "' and LedgerFK='" + lgrid.Text + "' and FeeCategory='" + feecat.Text + "' and TransCode='" + Rcptno + "' and App_No='" + appno + "'  and FinYearFK='" + finYearid + "' else   insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,Debit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype) values('" + transdate.ToString("MM/dd/yyyy") + "','" + curtime + "','" + Rcptno + "','1','" + lgrid.Text + "','" + hdrid.Text + "','" + feecat.Text + "','" + paid.Text + "','" + finYearid + "','" + appno + "','0','1','" + payMode + "','1','" + entryUserCode + "','3')";
                                                    d2.update_method_wo_parameter(INSdaily, "Text");
                                                    if (!NewhtCheckVal.Contains(transdate))
                                                    {
                                                        sbNewRecptDate.Append(transdate + ",");
                                                        NewhtCheckVal.Add(transdate);
                                                    }
                                                    if (!NewhtCheckVal.Contains(Rcptno))
                                                    {
                                                        sbNewRecptCode.Append(Rcptno + ",");
                                                        NewhtCheckVal.Add(Rcptno);
                                                    }
                                                    double tempNewPaidAmt = 0;
                                                    double.TryParse(Convert.ToString(paid.Text), out tempNewPaidAmt);
                                                    newAmt += tempNewPaidAmt;
                                                    if (gridView5.Rows.Count > 0)
                                                    {
                                                        string excessval = Convert.ToString(Label8.Text).Split('.')[1];
                                                        if (excessval == "" || excessval == "0")
                                                            excessval = "0";
                                                        if (excessval != "0")
                                                            excessval = allotExcessAmt(appno, feecat.Text, excessval, finYearid, transdate.ToString("MM/dd/yyyy"), Rcptno);
                                                    }
                                                }
                                            }
                                            updateOK = true;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Dailytransaction insert
                            //new receipt code generate

                            //if (hedgid != "")
                            //    Rcptno = generateReceiptNo(hedgid, ref dtReceipt, ref arRcptfk);

                            if (!string.IsNullOrEmpty(Rcptno) || dtReceipt.Count > 0)
                            {
                                foreach (GridViewRow row in gridView5.Rows)
                                {
                                    CheckBox cbsel = (CheckBox)row.FindControl("cblsell");
                                    if (cbsel.Checked)
                                    {
                                        Label hdrid = (Label)row.FindControl("lbl_hdrid");
                                        Label lgrid = (Label)row.FindControl("lbl_lgrid");
                                        Label feecat = (Label)row.FindControl("lbl_feecat");
                                        Label feeamt = (Label)row.FindControl("lbl_feeamt");
                                        Label totamt = (Label)row.FindControl("lbl_totamt");
                                        Label concession = (Label)row.FindControl("lbl_Concess");
                                        TextBox paid = (TextBox)row.FindControl("txt_paid");
                                        TextBox balance = (TextBox)row.FindControl("txt_bal");
                                        TextBox excess = (TextBox)row.FindControl("txt_exGrid2");
                                        if (feeamt.Text == "")
                                            feeamt.Text = "0";
                                        if (totamt.Text == "")
                                            totamt.Text = "0";
                                        if (concession.Text == "")
                                            concession.Text = "0";
                                        if (paid.Text == "")
                                            paid.Text = "0";
                                        if (balance.Text == "")
                                            balance.Text = "0";
                                        if (excess.Text == "")
                                            excess.Text = "0";
                                        if (dtReceipt.Count > 0)
                                            if (dtReceipt.ContainsKey(Convert.ToString(hdrid.Text)))
                                                Rcptno = dtReceipt[hdrid.Text].ToString();
                                        if (paid.Text != "0")
                                        {
                                            string selQy = "select distinct paymode from ft_findailytransaction where app_no='" + appno + "' and isnull(iscanceled,'0')='1' and debit='" + paid.Text + "'";
                                            string payMode = d2.GetFunction(selQy);
                                            payMode = "1";
                                            if (payMode != "0")
                                            {
                                                //daily transaction
                                                string INSdaily = "if exists(select * from FT_FinDailyTransaction where HeaderFK='" + hdrid.Text + "' and LedgerFK='" + lgrid.Text + "' and FeeCategory='" + feecat.Text + "' and TransCode='" + Rcptno + "' and App_No='" + appno + "' and FinYearFK='" + finYearid + "')update FT_FinDailyTransaction set Debit='" + paid.Text + "',TransDate='" + transdate.ToString("MM/dd/yyyy") + "',TransTime='" + curtime + "',IsCanceled='0',IsCollected='1',paymode ='" + payMode + "' where HeaderFK='" + hdrid.Text + "' and LedgerFK='" + lgrid.Text + "' and FeeCategory='" + feecat.Text + "' and TransCode='" + Rcptno + "' and App_No='" + appno + "'  and FinYearFK='" + finYearid + "' else   insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,Debit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype) values('" + transdate.ToString("MM/dd/yyyy") + "','" + curtime + "','" + Rcptno + "','1','" + lgrid.Text + "','" + hdrid.Text + "','" + feecat.Text + "','" + paid.Text + "','" + finYearid + "','" + appno + "','0','1','" + payMode + "','1','" + entryUserCode + "','3')";
                                                d2.update_method_wo_parameter(INSdaily, "Text");

                                                if (!NewhtCheckVal.Contains(transdate))
                                                {
                                                    sbNewRecptDate.Append(transdate + ",");
                                                    NewhtCheckVal.Add(transdate);
                                                }
                                                if (!NewhtCheckVal.Contains(Rcptno))
                                                {
                                                    sbNewRecptCode.Append(Rcptno + ",");
                                                    NewhtCheckVal.Add(Rcptno);
                                                }
                                                double tempNewPaidAmt = 0;
                                                double.TryParse(Convert.ToString(paid.Text), out tempNewPaidAmt);
                                                newAmt += tempNewPaidAmt;
                                                if (gridView5.Rows.Count > 0)
                                                {
                                                    string excessval = string.Empty;
                                                    if (Label8.Text != "")
                                                        excessval = Convert.ToString(Label8.Text).Split('.')[1];
                                                    if (excessval == "" || excessval == "0")
                                                        excessval = "0";
                                                    if (excessval != "0")
                                                    {
                                                        excessval = allotExcessAmt(appno, feecat.Text, excessval, finYearid, transdate.ToString("MM/dd/yyyy"), Rcptno);
                                                        double.TryParse(excessval, out newExcessAmt);
                                                    }
                                                }
                                            }
                                        }
                                        updateOK = true;
                                    }
                                }
                            }
                            #endregion
                        }
                        if (updateOK)
                        {
                            //new entry to transfer table
                            transfer(appno, fstDegreecode, ddl_dept.SelectedItem.Value, fstSection, sec, fstClgcode, ddl_colg.SelectedItem.Value, ddl_batch.SelectedItem.Value, fstSeatCode, seatype, sbOldRecptDate, sbOldRecptCode, oldAmt, sbNewRecptDate, sbNewRecptCode, newAmt, newExcessAmt, oldRoll, oldReg, oldRollAdmit, studAdmDate);
                            #region Update Receipt No

                            if (Convert.ToInt32(Session["save1"]) != 5)
                            {
                                string updateRecpt = string.Empty;
                                //if (Convert.ToInt32(Session["isHeaderwise"]) == 0 || Convert.ToInt32(Session["isHeaderwise"]) == 2)
                                //{
                                //    Rcptno = Rcptno.Remove(0, Convert.ToString(Session["acronym"]).Length);
                                //    updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + Rcptno + "+1 where collegecode =" + ddl_colg.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + ddl_colg.SelectedItem.Value + ")";
                                //    d2.update_method_wo_parameter(updateRecpt, "Text");
                                //}
                                if (Convert.ToInt32(Session["isHeaderwise"]) == 0)
                                {
                                    Rcptno = Rcptno.Remove(0, Convert.ToString(Session["acronym"]).Length);
                                    updateRecpt = " update FM_FinCodeSettings set JournalStNo=" + Rcptno + "+1 where collegecode =" + ddl_colg.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + ddl_colg.SelectedItem.Value + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                                    d2.update_method_wo_parameter(updateRecpt, "Text");
                                }
                                else
                                {
                                    ArrayList arrcpt = new ArrayList();
                                    foreach (KeyValuePair<string, string> reptUpdate in dtReceipt)
                                    {
                                        string headerfk = reptUpdate.Key.ToString();
                                        Rcptno = reptUpdate.Value.ToString();
                                        if (!arrcpt.Contains(Rcptno))
                                        {
                                            string hdFkval = string.Empty;
                                            if (arRcptfk.ContainsKey(Rcptno))
                                            {
                                                hdFkval = arRcptfk[Rcptno].ToString();
                                                arrcpt.Add(Rcptno);
                                                Rcptno = Rcptno.Remove(0, Convert.ToString(hdFkval.Split('-')[1]).Length);
                                                updateRecpt = "update FM_HeaderFinCodeSettings set RcptStNo=" + Rcptno + "+1 where HeaderSettingPK=" + hdFkval.Split('-')[0] + " and FinyearFK=" + finYearid + " and CollegeCode=" + ddl_colg.SelectedItem.Value + " and isnull(onlineRec,'')<>'1'";
                                                d2.update_method_wo_parameter(updateRecpt, "Text");

                                            }
                                        }
                                    }
                                }

                            }

                            #endregion
                            transferReceipt("Journal", appno, ddl_colg.SelectedItem.Value, transdate.ToString("MM/dd/yyyy"), Convert.ToString(sbNewRecptCode));
                            txt_tramt.Text = "";
                            txt_roll_no1.Text = "";
                            txt_roll_no.Text = "";
                            txtamtind.Text = "";
                            divalert.Visible = true;
                            lbalert.Text = "Transferred Sucessfully";
                        }
                        else
                        {
                            divalert.Visible = true;
                            lbalert.Text = "Not Transferred";
                        }
                    }
                    else
                    {
                        divalert.Visible = true;
                        lbalert.Text = "Insufficient To Details";
                    }
                }
                else
                {
                    divalert.Visible = true;
                    lbalert.Text = "Student Details Not Found";
                }
            }
            else
            {
                divalert.Visible = true;
                lbalert.Text = "Please Enter Roll Number";
            }
            #endregion
        }
        catch { }
    }
    protected void getOldPayment(string appNo, string hdFK, string ldFK, string feeCat, string receiptno, string amt, string dtrcpt)
    {
        string selQ = " select distinct memtype,paymode,ddno,dddate,ddbankcode,ddbankbranch,isdeposited,depositeddate,iscollected,collecteddate,entryusercode,finyearfk,receipttype,actualfinyearfk,deposite_bankfk,narration from ft_findailytransaction where App_No='" + appNo + "' and headerfk='" + hdFK + "' and ledgerfk='" + ldFK + "'  and FeeCategory='" + feeCat + "'";
        DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
        {
            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
            {
                string insertDebit = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected,CollectedDate,IsDeposited,DepositedDate,ActualFinYearFK) VALUES('" + dtrcpt + "','" + DateTime.Now.ToLongTimeString() + "','" + receiptno + "', " + Convert.ToString(dsVal.Tables[0].Rows[row]["memtype"]) + ", " + appNo + ", " + ldFK + ", " + hdFK + ", " + feeCat + ", '" + amt + "','0', " + Convert.ToString(dsVal.Tables[0].Rows[row]["paymode"]) + ", '" + Convert.ToString(dsVal.Tables[0].Rows[row]["ddno"]) + "', '" + Convert.ToString(dsVal.Tables[0].Rows[row]["dddate"]) + "', '" + Convert.ToString(dsVal.Tables[0].Rows[row]["ddbankcode"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[row]["ddbankbranch"]) + "', 3, '0', 0, '" + Convert.ToString(dsVal.Tables[0].Rows[row]["narration"]) + "', '0', '0', '0', 0, " + Convert.ToString(dsVal.Tables[0].Rows[row]["entryusercode"]) + ", " + Convert.ToString(dsVal.Tables[0].Rows[row]["finyearfk"]) + ",'" + Convert.ToString(dsVal.Tables[0].Rows[row]["receipttype"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[row]["isdeposited"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[row]["depositeddate"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[row]["iscollected"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[row]["collecteddate"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[row]["actualfinyearfk"]) + "')";

                d2.update_method_wo_parameter(insertDebit, "Text");
            }
        }
    }
    protected string allotExcessAmt(string appno, string feecat, string excessval, string finYearid, string transdate, string transcode)
    {
        try
        {
            #region excess amount

            if (excessval != "0")
            {
                string select = "if exists(select * from FT_ExcessDet where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat + "')update FT_ExcessDet set ExcessAmt=isnull(ExcessAmt,'0')+'" + excessval + "',BalanceAmt=isnull(BalanceAmt,'0')+'" + excessval + "' where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat + "' else insert into FT_ExcessDet (ExcessTransDate,dailytranscode,TransTime,MemType,App_No ,ExcessType,ExcessAmt,BalanceAmt,FinYearFK , FeeCategory) values('" + transdate + "','" + transcode + "','" + DateTime.Now.ToLongTimeString() + "','1','" + appno + "','1','" + excessval + "','" + excessval + "','" + finYearid + "','" + feecat + "')";
                int exCal = d2.update_method_wo_parameter(select, "Text");
                if (exCal > 0)
                {
                    string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='1'");
                    for (int i = 0; i < gridView5.Rows.Count; i++)
                    {
                        Label header = (Label)gridView5.Rows[i].FindControl("lbl_hdrid");
                        Label ledger = (Label)gridView5.Rows[i].FindControl("lbl_lgrid");
                        Label feecatg = (Label)gridView5.Rows[i].FindControl("lbl_yearsem");
                        Label totalamt = (Label)gridView5.Rows[i].FindControl("lbl_totamt");
                        TextBox excessamt = (TextBox)gridView5.Rows[i].FindControl("txt_exGrid2");
                        double tempExcess = 0;
                        double.TryParse(Convert.ToString(excessamt.Text), out tempExcess);
                        if (tempExcess != 0)
                        {
                            string selqry = "select * from FT_ExcessLedgerDet if  exists(select * from FT_ExcessLedgerDet where  ExcessDetFK='" + getvalue + "' and HeaderFK='" + header.Text + "' and LedgerFK='" + ledger.Text + "' and FinYearFK='" + finYearid + "' and FeeCategory in('" + feecat + "') )update FT_ExcessLedgerDet set ExcessAmt=isnull(ExcessAmt,'0')+'" + tempExcess + "',BalanceAmt=isnull(BalanceAmt,'0')+'" + tempExcess + "',HeaderFK ='" + header.Text + "',LedgerFK='" + ledger.Text + "' where ExcessDetFK='" + getvalue + "' and HeaderFK ='" + header.Text + "' and LedgerFK='" + ledger.Text + "' and FinYearFK='" + finYearid + "' and FeeCategory in('" + feecat + "') else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FinYearFK,FeeCategory) values('" + header.Text + "','" + ledger.Text + "','" + tempExcess + "','" + tempExcess + "','" + getvalue + "','" + finYearid + "','" + feecat + "')";
                            d2.update_method_wo_parameter(selqry, "Text");
                            excessval = "0";
                        }
                    }
                }
            }

            #endregion
        }
        catch { }
        return excessval;
    }

    protected string ledgermappingheaderValue()
    {
        string hedgid = string.Empty;
        ArrayList arrcpt = new ArrayList();
        foreach (GridViewRow hdrow in gridView5.Rows)
        {
            CheckBox cbsnd = (CheckBox)hdrow.FindControl("cblsell");
            if (cbsnd.Checked)
            {
                Label hdrid = (Label)hdrow.Cells[1].FindControl("lbl_hdrid");
                if (hedgid == "")
                {
                    hedgid = Convert.ToString(hdrid.Text);
                    arrcpt.Add(Convert.ToString(hdrid.Text));
                }
                else
                {
                    if (!arrcpt.Contains(hdrid.Text))
                    {
                        hedgid = hedgid + "'" + "," + "'" + Convert.ToString(hdrid.Text);
                        arrcpt.Add(Convert.ToString(hdrid.Text));
                    }
                }
            }
        }
        return hedgid;
    }

    protected void btnadjust_Click(object sender, EventArgs e)
    {
        if (gridFirstCheck())
        {
            if (gridSecondCheck())
            {
                AdjustLedgerDetails();

            }
            else
            {
                // divalert.Visible = true;
                // lbalert.Text = "Please Select Any One Ledger";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Select Any One Ledger')", true);
            }
        }
        else
        {
            //divalert.Visible = true;
            // lbalert.Text = "Please Select Any One Ledger";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Select Any One Ledger')", true);
        }

    }
    protected void AdjustLedgerDetails()
    {
        try
        {
            bool save = false;
            double totAmt = 0;
            double paidAmt = 0;
            double paidOvall = 0;
            double FnlPaidamt = 0;
            double Fnltotalamt = 0;
            double temptotamt = 0;
            double excessAmt = 0;
            string disAmt = "";
            foreach (GridViewRow grow in gridView4.Rows)
            {
                CheckBox cb1 = (CheckBox)grow.FindControl("cbsel");
                Label totamount = (Label)grow.FindControl("lbl_totamt");
                if (cb1.Checked)
                {
                    Label paid1 = (Label)grow.FindControl("lbl_paid");
                    double.TryParse(Convert.ToString(paid1.Text), out  paidAmt);
                    FnlPaidamt += paidAmt;
                }
                double.TryParse(Convert.ToString(totamount.Text), out  temptotamt);
                Fnltotalamt += temptotamt;
            }
            if (FnlPaidamt != 0)
            {
                disAmt = Convert.ToString(FnlPaidamt);
                int index = -1;
                int indextwo = 0;
                foreach (GridViewRow gdsndrow in gridView5.Rows)
                {
                    CheckBox cb2 = (CheckBox)gdsndrow.FindControl("cblsell");
                    if (cb2.Checked)
                    {
                        if (index == -1)
                            index = indextwo;

                        Label hdrid = (Label)gdsndrow.FindControl("lbl_hdrid");
                        Label lgrid = (Label)gdsndrow.FindControl("lbl_lgrid");
                        Label feecat = (Label)gdsndrow.FindControl("lbl_feecat");
                        Label feeamt = (Label)gdsndrow.FindControl("lbl_feeamt");
                        Label totamt = (Label)gdsndrow.FindControl("lbl_totamt");
                        Label concession = (Label)gdsndrow.FindControl("lbl_Concess");
                        TextBox txtpaid = (TextBox)gdsndrow.FindControl("txt_paid");
                        TextBox txtbalance = (TextBox)gdsndrow.FindControl("txt_bal");
                        TextBox excess = (TextBox)gdsndrow.FindControl("txt_exGrid2");
                        excess.Text = "";
                        double.TryParse(Convert.ToString(totamt.Text), out  totAmt);
                        if (totAmt >= FnlPaidamt)
                        {
                            txtpaid.Text = Convert.ToString(FnlPaidamt);
                            paidOvall += FnlPaidamt;
                            FnlPaidamt = 0;
                        }
                        else
                        {
                            txtpaid.Text = Convert.ToString(totAmt);
                            FnlPaidamt = FnlPaidamt - totAmt;
                            paidOvall += totAmt;
                        }
                        txtbalance.Text = (totAmt - Convert.ToDouble(txtpaid.Text)).ToString();
                        save = true;
                    }
                    indextwo++;
                }
                // FnlPaidamt = 4500;
                excessAmt += FnlPaidamt;
                if (excessAmt > 0)
                {
                    TextBox excess = (TextBox)gridView5.Rows[index].FindControl("txt_exGrid2");
                    excess.Text = Convert.ToString(excessAmt);
                }
                Label5.Text = "Rs." + Convert.ToString(paidOvall);
                string totPaid = Label4.Text;
                string paid = totPaid.Split('.')[1];
                double fnltot = 0;
                double.TryParse(Convert.ToString(paid), out fnltot);
                Label6.Text = "Rs." + Convert.ToString(fnltot - paidOvall);
                Label24ex.Text = "Rs." + Convert.ToString(excessAmt);
                Label8.Text = "Rs." + Convert.ToString(excessAmt);
                btntransind.Enabled = true;
                if (save == true)
                {
                    //divalert.Visible = true;
                    //lbalert.Text = "Mapping Successfully";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Mapping Successfully')", true);
                    Label11.Text = disAmt;
                    div7.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Do Not Have Paid Amount')", true);
            }


        }
        catch { }
    }
    protected void btnmapreset_Click(object sender, EventArgs e)
    {
        Label8.Text = "";
        Label6.Text = "";
        Label5.Text = "";
        Label3.Text = "";
        Label2.Text = "";
        Label1.Text = "";
        bindHeaderind();
        bindLedgerind();
        bindGridInd();
        bindGrid5Ind();
        txtamtind.Text = "";
        btntransind.Enabled = false;

    }



    //old
    protected void adjustLedger()
    {
        try
        {
            double totAmt = 0;
            double paidAmt = 0;
            //double balAmt = 0;
            //double consAmt = 0;
            //double balOvall = 0;
            double paidOvall = 0;
            double excessovall = 0;
            double balAmount = 0;
            double SndPaidamt = 0;
            double excessAmt = 0;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (gridView4.Rows.Count > 0 && gridView5.Rows.Count > 0)
            {
                //int index = rowIndxClicked();
                foreach (GridViewRow gdfstrow in gridView4.Rows)
                {
                    CheckBox cb1 = (CheckBox)gdfstrow.FindControl("cbsel");
                    if (cb1.Checked)
                    {
                        Label hdrid1 = (Label)gdfstrow.FindControl("lbl_hdrid");
                        Label lgrid1 = (Label)gdfstrow.FindControl("lbl_lgrid");
                        Label feecat1 = (Label)gdfstrow.FindControl("lbl_feecat");
                        Label concession1 = (Label)gdfstrow.FindControl("lbl_Concess");
                        Label paid1 = (Label)gdfstrow.FindControl("lbl_paid");
                        double.TryParse(Convert.ToString(paid1.Text), out  paidAmt);
                        foreach (GridViewRow gdsndrow in gridView5.Rows)
                        {
                            CheckBox cb2 = (CheckBox)gdsndrow.FindControl("cblsell");
                            if (cb2.Checked)
                            {
                                Label hdrid = (Label)gdsndrow.FindControl("lbl_hdrid");
                                Label lgrid = (Label)gdsndrow.FindControl("lbl_lgrid");
                                Label feecat = (Label)gdsndrow.FindControl("lbl_feecat");
                                Label feeamt = (Label)gdsndrow.FindControl("lbl_feeamt");
                                Label totamt = (Label)gdsndrow.FindControl("lbl_totamt");
                                Label concession = (Label)gdsndrow.FindControl("lbl_Concess");
                                TextBox txtpaid = (TextBox)gdsndrow.FindControl("txt_paid");
                                TextBox txtbalance = (TextBox)gdsndrow.FindControl("txt_bal");
                                TextBox excess = (TextBox)gdsndrow.FindControl("txt_exGrid2");
                                concession.Text = concession1.Text;
                                double.TryParse(Convert.ToString(totamt.Text), out  totAmt);
                                double.TryParse(Convert.ToString(txtbalance.Text), out  balAmount);
                                double.TryParse(Convert.ToString(txtpaid.Text), out  SndPaidamt);
                                double paidamt = 0;
                                if (balAmount == 0)
                                {
                                    if (totAmt >= paidAmt)
                                    {
                                        txtpaid.Text = Convert.ToString(paidAmt);
                                        paidAmt = 0;
                                    }
                                    else
                                    {
                                        txtpaid.Text = Convert.ToString(totAmt);
                                        paidAmt = paidAmt - totAmt;
                                    }
                                    txtbalance.Text = (totAmt - Convert.ToDouble(txtpaid.Text)).ToString();
                                }
                                else
                                {
                                    if (balAmount >= paidAmt)
                                    {
                                        double temppaid = paidAmt;
                                        txtpaid.Text = Convert.ToString(SndPaidamt + temppaid);
                                        paidAmt = 0;
                                    }
                                    else
                                    {
                                        double temppaid = balAmount;
                                        txtpaid.Text = Convert.ToString(SndPaidamt + temppaid);
                                        paidAmt = paidAmt - balAmount;
                                    }
                                    txtbalance.Text = (totAmt - Convert.ToDouble(txtpaid.Text)).ToString();
                                }
                            }
                        }
                        //  excessovall += paidAmt;
                    }
                }
                excessAmt += paidAmt;
                Label5.Text = "Rs." + paidOvall.ToString();
                if (Label4.Text.Trim() != "" && Label5.Text.Trim() != "")
                {
                    double tot = 0;
                    double.TryParse(Convert.ToString(Label4.Text), out tot);
                    double paids = paidOvall;
                    // Label6.Text = "Rs." + (tot - paidOvall).ToString();
                }
                Label8.Text = "Rs." + excessovall.ToString();
            }
        }
        catch { }
    }

    public void bindHeaderind()
    {
        try
        {
            ddlhedind.Items.Clear();
            //if (Session["clgcode"] != null)
            //    collegecode1 = Convert.ToString(Session["clgcode"]);
            //else
            //    collegecode1 = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            }

            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster L,FS_HeaderPrivilage P WHERE L.HeaderPK = P.HeaderFK   AND P.CollegeCode = L.CollegeCode  AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlhedind.DataSource = ds;
                    ddlhedind.DataTextField = "HeaderName";
                    ddlhedind.DataValueField = "HeaderPK";
                    ddlhedind.DataBind();

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }

    public void bindLedgerind()
    {
        try
        {
            //if (Session["clgcode"] != null)
            //    collegecode1 = Convert.ToString(Session["clgcode"]);
            //else
            //    collegecode1 = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            }
            ddlledind.Items.Clear();
            string headerfk = "-1";
            if (ddlhedind.Items.Count > 0)
            {
                headerfk = Convert.ToString(ddlhedind.SelectedItem.Value);
            }
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode and l.HeaderFK=" + headerfk + " AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode1 + " order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlledind.DataSource = ds;
                    ddlledind.DataTextField = "LedgerName";
                    ddlledind.DataValueField = "LedgerPK";
                    ddlledind.DataBind();

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }

    #region grid4 bind

    public void bindGridInd()
    {
        string app_no = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");
        dt.Columns.Add("paymode");

        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
        {
            app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
        {
            app_no = d2.GetFunction("select app_no from Registration where Reg_no='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
        {
            app_no = d2.GetFunction("select app_no from Registration where Roll_admit='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
        {
            app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }

        if (app_no != "")
        {
            string selectQ = "";
            if (ddladmis.SelectedItem.Text.Trim() != "Before Admission")
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code  and r.App_No=" + app_no + " order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
                // order by F.FeeCategory,f.HeaderFK,f.LedgerFK
            }
            else
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode   from FT_FeeAllot f,applyn R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code  and r.App_No=" + app_no + " order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
                //order by F.FeeCategory,f.HeaderFK,f.LedgerFK
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");
            //if (Session["clgcode"] != null)
            //    collegecode1 = Convert.ToString(Session["clgcode"]);
            //else
            //    collegecode1 = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
            }
            if (ds.Tables.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                    string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + ddlcollege.SelectedValue + "");
                    dr = dt.NewRow();
                    dr["Sno"] = row + 1;
                    dr["YearSem"] = cursem;
                    dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                    dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                    dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                    dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                    dr["FeeCategory"] = feecat;
                    dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                    dr["Paid"] = Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]);
                    dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                    dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                    dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                    dr["paymode"] = Convert.ToString(ds.Tables[0].Rows[row]["paymode"]);
                    dt.Rows.Add(dr);

                    total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                    balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                    paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);

                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            gridView4.DataSource = dt;
            gridView4.DataBind();
            Label3.Text = "Rs." + balance.ToString();
            Label2.Text = "Rs." + paid.ToString();
            Label1.Text = "Rs." + total.ToString();
            Table1.Visible = true;
        }
        else
        {
            gridView4.DataSource = null;
            gridView4.DataBind();
            Table1.Visible = false;
        }
    }

    #endregion

    #region grid5
    public void bindGrid5Ind()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");

        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;
        string clgcode = "";

        string selectQ = "";
        string stream = "";
        string batch = "";
        string degreeCode = "";
        string dept = "";
        string feecategory = "";
        string section = "";

        if (rb_transfer.Checked)
        {
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                if (txt_roll1.Text.Trim() != "")
                {
                    stream = txt_strm1.Text.Trim();
                    batch = txt_batch1.Text.Trim();
                    degreeCode = lblDegCode.Text;
                    dept = "";
                    feecategory = "";
                    section = "";
                    string fstSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlcollege.SelectedValue + "'  and textval='" + txt_seat_type1.Text.Trim() + "'"));
                    //if (Session["clgcode"] != null)
                    //    clgcode = Convert.ToString(Session["clgcode"]);
                    //else
                    //    clgcode = Convert.ToString(Session["collegecode"]);
                    if (ddlcollege.Items.Count > 0)
                    {
                        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                        collegeCode = Convert.ToInt32(ddlcollege.SelectedItem.Value);
                    }
                    //selectQ = "  select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.BalAmount,0) as BalAmount   from FT_FeeAllotDegree f,FM_HeaderMaster H,FM_LedgerMaster L where  F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and  L.CollegeCode=H.CollegeCode and L.CollegeCode=" + collegecode1 + " and F.BatchYear=" + batch + " and F.DegreeCode=" + degreeCode + " ";
                    selectQ = "  select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount  from FT_FeeAllotDegree f,FM_HeaderMaster H,FM_LedgerMaster L where  F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and  L.CollegeCode=H.CollegeCode and L.CollegeCode=" + ddlcollege.SelectedValue + " and F.BatchYear=" + batch + " and F.DegreeCode=" + degreeCode + " ";
                    if (fstSeatCode != "0")
                        selectQ += "   and seattype='" + fstSeatCode + "' ";
                    if (stream != "")
                    {
                        selectQ += " ";
                    }
                    selectQ += " order by isnull(l.priority,1000), l.ledgerName asc";
                }
            }
            else
            {
                if (ddl_colg.Items.Count > 0)
                {
                    // collegecode1 = Convert.ToString(ddl_colg.SelectedItem.Value);
                    clgcode = Convert.ToString(ddl_colg.SelectedItem.Value);
                    if (ddl_strm.Items.Count > 0)
                        stream = Convert.ToString(ddl_strm.SelectedItem.Value);
                    if (ddl_batch.Items.Count > 0)
                        batch = Convert.ToString(ddl_batch.SelectedItem.Value);
                    if (ddl_degree.Items.Count > 0)
                        degreeCode = Convert.ToString(ddl_degree.SelectedItem.Value);
                    if (ddl_dept.Items.Count > 0)
                        dept = Convert.ToString(ddl_dept.SelectedItem.Value);
                    if (ddl_sem.Items.Count > 0)
                        feecategory = Convert.ToString(ddl_sem.SelectedItem.Value);
                    if (ddl_sec.Items.Count > 0)
                        section = Convert.ToString(ddl_sec.SelectedItem.Value);
                    if (ddl_seattype.Items.Count > 0)
                        seatype = Convert.ToString(ddl_seattype.SelectedValue);

                    //if (Session["seatype"] != null)
                    //{
                    //    seatype = Convert.ToString(Session["seatype"]);
                    //}

                    //selectQ = "  select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.BalAmount,0) as BalAmount   from FT_FeeAllotDegree f,FM_HeaderMaster H,FM_LedgerMaster L where  F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and  L.CollegeCode=H.CollegeCode and L.CollegeCode=" + collegecode1 + " and F.BatchYear=" + batch + " and F.DegreeCode=" + dept + " and F.FeeCategory=" + feecategory + " ";
                    selectQ = "  select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount   from FT_FeeAllotDegree f,FM_HeaderMaster H,FM_LedgerMaster L where  F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and  L.CollegeCode=H.CollegeCode and L.CollegeCode=" + ddl_colg.SelectedItem.Value + " and F.BatchYear=" + batch + " and F.DegreeCode=" + dept + " and seattype='" + seatype + "' ";
                    //selectQ += " and F.FeeCategory=" + feecategory + " ";
                    if (section != "")
                    {
                        selectQ += " ";
                    }
                    if (stream != "")
                    {
                        selectQ += " ";
                    }
                    selectQ += " order by isnull(l.priority,1000), l.ledgerName asc";
                }
            }

        }

        if (selectQ != "")
        {

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");

            if (ds.Tables.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                    string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + clgcode + "");
                    dr = dt.NewRow();
                    dr["Sno"] = row + 1;
                    dr["YearSem"] = cursem;
                    dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                    dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                    dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                    dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                    dr["FeeCategory"] = feecat;
                    dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                    // dr["Paid"] = Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]);
                    // dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                    dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                    dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                    dt.Rows.Add(dr);

                    total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                    //balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                    // paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);
                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            gridView5.DataSource = dt;
            gridView5.DataBind();
            Label6.Text = "Rs." + balance.ToString();
            Label5.Text = "Rs." + paid.ToString();
            Label4.Text = "Rs." + total.ToString();
            Table2.Visible = true;
        }
        else
        {
            gridView5.DataSource = null;
            gridView5.DataBind();
            Table2.Visible = false;
        }


    }


    #region

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

    #endregion

    #endregion


    //gridrow checkbox checked
    protected bool gridFirstCheck()
    {
        bool checkval = false;
        try
        {
            foreach (GridViewRow gdview in gridView4.Rows)
            {
                CheckBox cb = (CheckBox)gdview.FindControl("cbsel");
                if (cb.Checked)
                {
                    checkval = true;
                }
            }
        }
        catch { }
        return checkval;
    }

    protected bool gridSecondCheck()
    {
        bool checkval = false;
        try
        {
            foreach (GridViewRow gdview in gridView5.Rows)
            {
                CheckBox cb = (CheckBox)gdview.FindControl("cblsell");
                if (cb.Checked)
                {
                    checkval = true;
                }
            }
        }
        catch { }
        return checkval;
    }

    protected void btnalert_Click(object sender, EventArgs e)
    {
        divalert.Visible = false;
        divledger.Visible = false;
        divind.Visible = false;
        divindi.Visible = false;
        //if (rb_transfer.Checked == true)
        //{
        //    if (rbl_AdmitTransfer.SelectedIndex == 1)
        //    {
        txt_roll.Text = "";
        txt_name.Text = "";
        txt_date.Text = "";
        txt_colg.Text = "";
        txt_strm.Text = "";
        txt_batch.Text = "";
        txt_degree.Text = "";
        txt_dept.Text = "";
        txt_sem.Text = "";
        txt_sec.Text = "";
        txt_seattype.Text = "";
        image2.ImageUrl = "";
        rbl_AdmitTransfer.SelectedIndex = 0;
        rbl_AdmitTransfer_OnSelectedIndexChanged(sender, e);
        txt_tramt.Text = "";
        bindFromGrid();
        bindApplideNotGrid1();
        lnkindivmap.Enabled = false;

        //    }
        //}
    }

    protected void clearText()
    {
        divledger.Visible = false;
        divind.Visible = false;
        if (rb_transfer.Checked == true)
        {
            if (rbl_AdmitTransfer.SelectedIndex == 1)
            {
                txt_roll.Text = "";
                txt_name.Text = "";
                txt_date.Text = "";
                txt_colg.Text = "";
                txt_strm.Text = "";
                txt_batch.Text = "";
                txt_degree.Text = "";
                txt_dept.Text = "";
                txt_sem.Text = "";
                txt_sec.Text = "";
                txt_seattype.Text = "";
                image2.ImageUrl = "";
                rbl_AdmitTransfer.SelectedIndex = 0;
                // rbl_AdmitTransfer_OnSelectedIndexChanged(sender, e);
                txt_tramt.Text = "";
                // bindGrid1();
                bindFromGrid();
                lnkindivmap.Enabled = false;

            }
        }
    }

    protected void buttonok_Click(object sender, EventArgs e)
    {
        div7.Visible = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Mapping Successfully')", true);
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        div7.Visible = false;
        btnmapreset_Click(sender, e);
    }

    #endregion

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
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        //
        lbl.Add(lblclgs);
        lbl.Add(lbl_str2);
        lbl.Add(lbldegs);
        lbl.Add(lbldepts);
        lbl.Add(lblsems);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        //
        lbl.Add(lblclgss);
        lbl.Add(lbl_str3);
        lbl.Add(lbldegss);
        lbl.Add(lbldeptss);
        lbl.Add(lblsemss);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        //
        lbl.Add(lblCollege1);
        lbl.Add(lbl_stream1);
        lbl.Add(lbl_degree1);
        lbl.Add(lbl_branch1);
        lbl.Add(lbl_sem1);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        //
        lbl.Add(lblCollege);
        lbl.Add(lbl_stream);
        lbl.Add(lbl_degree);
        lbl.Add(lbl_branch);
        lbl.Add(lbl_Sem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        //
        lbl.Add(lblcoll);
        lbl.Add(lbl_str4);
        lbl.Add(lbldegre);
        lbl.Add(lbldeptms);
        lbl.Add(lblsemests);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 04-10-2016 sudhagar

    protected void gridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = lblsem.Text;
        }
    }
    protected void gridView2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = lblsem.Text;
        }
    }
    protected void gridView3_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = lblsem.Text;
        }
    }
    protected void gridView4_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = lblsem.Text;
        }
    }
    protected void gridView5_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = lblsem.Text;
        }
    }


    protected bool validate()
    {
        bool check = false;
        try
        {
            string rollno = Convert.ToString(txt_roll.Text);
        }
        catch { }
        return check;
    }

    //added by sudhagar 11.05.2017

    private string generateApplNo(string collegecode, int degreecode, string edulevel, string mode, string seattype, string batchyear, out int format)
    {
        string applNo = string.Empty;
        format = 0;
        try
        {
            ApplicationNumberGeneration appGen = new ApplicationNumberGeneration();
            int codeCheck = 0;
            string query = "select LinkValue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + usercode + "' "; //and college_code ='" + collegecode + "'
            codeCheck = DirAccess.selectScalarInt(query);
            if (codeCheck > 0)
            {
                applNo = appGen.getApplicationNumber(collegecode, batchyear, 1);
                format = 1;
            }
            else
            {
                query = "select LinkValue from New_InsSettings where LinkName='EdulevelAdmissionNoRights' and user_code ='" + usercode + "' ";//and college_code ='" + collegecode + "'
                codeCheck = DirAccess.selectScalarInt(query);

                if (codeCheck > 0)
                {
                    applNo = appGen.getApplicationNumber(collegecode, edulevel, batchyear, 1);
                    format = 2;
                }
                else
                {
                    query = "select LinkValue from New_InsSettings where LinkName='DegreeSeatModewiseAdmissionNoRights' and user_code ='" + usercode + "' ";//and college_code ='" + collegecode + "'
                    codeCheck = DirAccess.selectScalarInt(query);
                    if (codeCheck > 0)
                    {
                        applNo = appGen.getApplicationNumber(collegecode, batchyear, degreecode.ToString(), mode, seattype, 1);
                        format = 3;
                    }
                    else
                    {
                        applNo = appGen.getApplicationNumber(collegecode, batchyear, degreecode, 1);
                        format = 0;
                    }
                }
            }
        }
        catch { applNo = string.Empty; }
        return applNo;
    }
    private bool UpdateApplNo(string collegecode, int degreecode, string edulevel, string mode, string seattype, string batchyear, int format)
    {
        bool update = false;

        try
        {
            ApplicationNumberGeneration appGen = new ApplicationNumberGeneration();
            int codeCheck = 0;
            string query = "select LinkValue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + usercode + "' "; //and college_code ='" + collegecode + "'
            codeCheck = DirAccess.selectScalarInt(query);
            if (codeCheck > 0)
            {
                update = appGen.updateApplicationNumber(collegecode, batchyear, 1);

            }
            else
            {
                query = "select LinkValue from New_InsSettings where LinkName='EdulevelAdmissionNoRights' and user_code ='" + usercode + "'"; // and college_code ='" + collegecode + "'
                codeCheck = DirAccess.selectScalarInt(query);

                if (codeCheck > 0)
                {
                    update = appGen.updateApplicationNumber(collegecode, edulevel, batchyear, 1);

                }
                else
                {
                    query = "select LinkValue from New_InsSettings where LinkName='DegreeSeatModewiseAdmissionNoRights' and user_code ='" + usercode + "' "; //and college_code ='" + collegecode + "'
                    codeCheck = DirAccess.selectScalarInt(query);
                    if (codeCheck > 0)
                    {
                        update = appGen.updateApplicationNumber(collegecode, batchyear, degreecode.ToString(), mode, seattype, 1);

                    }
                    else
                    {
                        update = appGen.updateApplicationNumber(collegecode, batchyear, degreecode, 1);

                    }
                }
            }
        }
        catch { update = false; }
        return update;
    }

    protected void getAdmissionNo()
    {
        try
        {
            txt_roll_no.Text = string.Empty;

            if (validateAdmissionCheck())//admission no check
            {
                int format = 0;
                string eduleve = Convert.ToString(d2.GetFunction(" select distinct edu_level,degree_code from degree d,course  c where d.course_id=c.course_id and d.college_code='" + ddl_colg.SelectedValue + "' and d.degree_code='" + ddl_dept.SelectedValue + "'"));
                string Mode = string.Empty;
                string appNo = getappNo();
                if (appNo != "0" && !string.IsNullOrEmpty(appNo))
                    Mode = Convert.ToString(d2.GetFunction(" select mode from applyn where app_no='" + appNo + "' and college_code='" + ddlcollege.SelectedValue + "'"));
                txt_roll_no.Text = generateApplNo(ddl_colg.SelectedValue, Convert.ToInt32(ddl_dept.SelectedValue), eduleve, Mode, ddl_seattype.SelectedValue, ddl_batch.SelectedValue, out format);
            }
        }
        catch { }
    }

    protected void UpdateAdmissionNo(string appNo)
    {
        try
        {
            string rollNo = txt_roll_no.Text.Trim();
            if (!string.IsNullOrEmpty(rollNo) && rollNo != "0")
            {
                int format = 0;
                string eduleve = Convert.ToString(d2.GetFunction(" select distinct edu_level,degree_code from degree d,course  c where d.course_id=c.course_id and d.college_code='" + ddl_colg.SelectedValue + "' and d.degree_code='" + ddl_dept.SelectedValue + "'"));
                string Mode = string.Empty;
                if (appNo != "0" && !string.IsNullOrEmpty(appNo))
                    Mode = Convert.ToString(d2.GetFunction(" select mode from applyn where app_no='" + appNo + "' "));//and college_code='" + ddlcollege.SelectedValue + "'
                UpdateApplNo(ddl_colg.SelectedValue, Convert.ToInt32(ddl_dept.SelectedValue), eduleve, Mode, ddl_seattype.SelectedValue, ddl_batch.SelectedValue, format);
            }
        }
        catch { }
    }

    protected string getappNo()
    {
        string appNo = string.Empty;
        try
        {
            string roll = Convert.ToString(txt_roll.Text);
            string selQ = " select app_no from registration where roll_no='" + roll + "' and college_code='" + ddlcollege.SelectedValue + "'";
            appNo = Convert.ToString(d2.GetFunction(selQ));
            if (appNo == "0")
            {
                selQ = " select app_no from registration where reg_no='" + roll + "' and college_code='" + ddlcollege.SelectedValue + "'";
                appNo = Convert.ToString(d2.GetFunction(selQ));
            }
            if (appNo == "0")
            {
                selQ = " select app_no from registration where roll_admit='" + roll + "' and college_code='" + ddlcollege.SelectedValue + "'";
                appNo = Convert.ToString(d2.GetFunction(selQ));
            }
            if (appNo == "0")
            {
                selQ = " select app_no from applyn where app_fromno='" + roll + "' and college_code='" + ddlcollege.SelectedValue + "'";
                appNo = Convert.ToString(d2.GetFunction(selQ));
            }

        }
        catch { appNo = "0"; }
        return appNo;
    }

    protected bool validateAdmissionCheck()
    {
        bool check = false;
        try
        {
            string fstClgcode = string.Empty;
            string fstBatchYr = string.Empty;
            string fstDegreecode = string.Empty;
            string fstSection = string.Empty;
            string fstSeat = string.Empty;

            string sndClgcode = string.Empty;
            string sndBatchYr = string.Empty;
            string sndDegreecode = string.Empty;
            string sndSection = string.Empty;
            string sndSeat = string.Empty;

            fstClgcode = Convert.ToString(lbltempfstclg.Text);
            fstBatchYr = Convert.ToString(txt_batch.Text);
            fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
            fstSection = Convert.ToString(txt_sec.Text);
            fstSeat = Convert.ToString(txt_seattype.Text);

            string applno = string.Empty;
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                sndClgcode = Convert.ToString(lbltempsndclg.Text);
                sndBatchYr = Convert.ToString(txt_batch1.Text);
                sndDegreecode = Convert.ToString(lbltempsnddeg.Text);
                sndSection = Convert.ToString(txt_sec1.Text);
                sndSeat = Convert.ToString(txt_seat_type1.Text);
            }
            else
            {
                if (ddl_colg.Items.Count > 0)
                    sndClgcode = Convert.ToString(ddl_colg.SelectedItem.Value);
                if (ddl_batch.Items.Count > 0)
                    sndBatchYr = Convert.ToString(ddl_batch.SelectedItem.Value);
                if (ddl_dept.Items.Count > 0)
                    sndDegreecode = Convert.ToString(ddl_dept.SelectedItem.Value);
                if (ddl_sec.Items.Count > 0)
                    sndSection = Convert.ToString(ddl_sec.SelectedItem.Value);
                if (ddl_seattype.Items.Count > 0)
                    sndSeat = Convert.ToString(ddl_seattype.SelectedItem.Text);

            }

            if (!string.IsNullOrEmpty(fstClgcode) && !string.IsNullOrEmpty(fstBatchYr) && !string.IsNullOrEmpty(fstDegreecode) && !string.IsNullOrEmpty(sndClgcode)
&& !string.IsNullOrEmpty(sndBatchYr) && !string.IsNullOrEmpty(sndDegreecode))
            {
                if (fstClgcode == sndClgcode)
                {
                    if (fstDegreecode == sndDegreecode)
                        check = false;
                    else
                        check = true;
                }
                else
                    check = true;
            }
        }
        catch { }
        return check;

    }

    public void transferReceipt(string dupReceipt, string AppNo, string collegecode1, string recptDt, string recptNo)
    {
        //PAVAI College and School

        // FpSpread1.SaveChanges();
        try
        {
            string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
            DataSet dsPri = new DataSet();
            dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
            if (dsPri.Tables.Count > 0 && dsPri.Tables[0].Rows.Count > 0)
            {
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                //  finYearid = Convert.ToString(ddlfinyear.SelectedItem.Value);
                byte ColName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                //Document Settings

                bool createPDFOK = false;

                contentDiv.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();
                //  string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + usercode + "'").Trim();
                string collname = d2.GetFunction("select collname from collinfo where college_code ='" + collegecode1 + "'");
                int heightvar = 0;
                //for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                //{
                sbHtml.Clear();
                //byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                //if (check == 1)
                //{

                int officeCopyHeight = 0;
                //if (heightvar != 0)
                //{
                //    officeCopyHeight = heightvar+250;
                //}
                StringBuilder sbHtmlCopy = new StringBuilder();
                //string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                //string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                {
                    string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDNo,convert(varchar(10),DDDate,103) as DDDate,(select textval from textvaltable where textcode =DDBankCode) as Bank,DDBankBranch  from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                    DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                    if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                    {
                        string rollno = string.Empty;
                        string studname = string.Empty;
                        string receiptno = string.Empty;
                        string name = string.Empty;
                        string batch_year = string.Empty;

                        string app_formno = string.Empty;
                        string appnoNew = string.Empty;
                        string Regno = string.Empty;
                        string Roll_admit = string.Empty;
                        string section = string.Empty;
                        string currentSem = string.Empty;

                        string batchYrSem = string.Empty;

                        string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                        //string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                        string mode = string.Empty;
                        string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                        string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                        string ddNo = Convert.ToString(dsDet.Tables[0].Rows[0]["ddNo"]).Trim();
                        string modePaySng = string.Empty;
                        string dddates = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                        string ddnos = Convert.ToString(dsDet.Tables[0].Rows[0]["DDNo"]);
                        string ddBanks = Convert.ToString(dsDet.Tables[0].Rows[0]["Bank"]);
                        string ddBrans = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);

                        DataTable uniqueCols = dsDet.Tables[0].DefaultView.ToTable(true, "PayMode");
                        if (uniqueCols.Rows.Count > 0)
                        {
                            for (int a = 0; a < uniqueCols.Rows.Count; a++)
                            {
                                switch (Convert.ToString(uniqueCols.Rows[a][0]).Trim())
                                {
                                    case "1":
                                        mode += "Cash,";
                                        break;
                                    case "2":
                                        mode += "Cheque,";
                                        break;
                                    case "3":
                                        mode += "DD,";
                                        break;
                                    case "6":
                                        mode += "Card";
                                        break;
                                }
                            }
                            mode = mode.TrimEnd(',');
                        }
                        else
                        {
                            switch (paymode)
                            {
                                case "1":
                                    mode = "Cash";
                                    break;
                                case "2":
                                    mode = "Cheque";
                                    //mode = "Cheque - No:" + ddNo;
                                    modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                    //mode += modePaySng;
                                    break;
                                case "3":
                                    mode = "DD";
                                    //mode = "DD - No:" + ddNo;
                                    modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                    //mode += modePaySng;
                                    break;
                                case "4":
                                    mode = "Challan";
                                    break;
                                case "5":
                                    mode = "Online Payment";
                                    break;
                                case "6":
                                    mode = "Card";
                                    modePaySng = "\n\nCard : " + ddBanks;
                                    break;
                                default:
                                    mode = "Others";
                                    break;
                            }
                        }

                        string queryRollApp;

                        if (ddladmis.SelectedIndex == 1)
                        {
                            queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name,r.Roll_admit,r.sections,r.batch_year,r.current_semester  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                        }
                        else
                        {
                            queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name,app_formno as Roll_admit,'' sections,batch_year,r.current_Semester  from applyn where app_no='" + AppNo + "'";
                        }
                        DataSet dsRollApp = new DataSet();
                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                        if (dsRollApp.Tables.Count > 0)
                        {
                            if (dsRollApp.Tables[0].Rows.Count > 0)
                            {
                                rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                Roll_admit = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_admit"]);
                                studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                batch_year = Convert.ToString(dsRollApp.Tables[0].Rows[0]["batch_year"]);
                                section = Convert.ToString(dsRollApp.Tables[0].Rows[0]["sections"]).ToUpper();
                                currentSem = Convert.ToString(dsRollApp.Tables[0].Rows[0]["current_Semester"]).ToUpper();
                            }
                            else
                                appnoNew = AppNo;
                        }
                        else
                            appnoNew = AppNo;
                        name = rollno + "-" + studname;

                        //Print Region
                        #region Print Option For Receipt
                        try
                        {
                            //Fields to print

                            #region Settings Input
                            //Header Div Values
                            byte narration = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsNarration"]);

                            byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                            byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                            byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);

                            #endregion

                            #region Students Input


                            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " ";
                            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3 || ddladmis.SelectedIndex == 1)
                            {
                                colquery += " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                            }
                            else
                            {
                                colquery += " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,isnull(r.Sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + " ";
                            }


                            string collegename = "";
                            string add1 = "";
                            string add2 = "";
                            string add3 = "";
                            string univ = "";
                            string deg = "";
                            string cursem = "";
                            string batyr = "";
                            string seatty = "";
                            string board = "";
                            string mothe = "";
                            string fathe = "";
                            string stream = "";
                            double deductionamt = 0;
                            string strMem = string.Empty;
                            string TermOrSem = string.Empty;
                            string classdisplay = "Class Name ";
                            string rollDisplay = string.Empty;
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(colquery, "Text");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                    add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                    add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                    add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                                    univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                }

                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    if (checkSchoolSetting() == 0)
                                    {
                                        classdisplay = "Class Name ";
                                        TermOrSem = "Term";
                                    }
                                    else
                                    {
                                        classdisplay = "Dept Name ";
                                        TermOrSem = "Semester";
                                    }
                                    //if (degACR == 0)
                                    //{
                                    // deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                    //}
                                    //else
                                    //{
                                    deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                    //}
                                    cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                    batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                    seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                    board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                    mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                    fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                    //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                    if (checkSchoolSetting() == 0)
                                    {
                                        strMem = "Admission No";
                                    }
                                    else
                                    {
                                        strMem = rbl_rollno.SelectedItem.Text.Trim();
                                        if (Convert.ToInt32(rbl_rollno.SelectedValue) == 0)
                                        {
                                            Roll_admit = rollno;
                                        }
                                        else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 1)
                                        {
                                            Roll_admit = Regno;
                                        }
                                        else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 2)
                                        {
                                            //Roll_admit = Roll_admit;
                                        }
                                        else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 3)
                                        {
                                            Roll_admit = app_formno;
                                        }
                                    }
                                }
                            }
                            string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                            try
                            {
                                acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                            }
                            catch { }
                            #endregion
                            string degString = string.Empty;
                            //Line3

                            degString = deg;//.Split('-')[0].ToUpper();


                            string[] className = degString.Split('-');
                            if (className.Length > 1)
                            {
                                degString = className[1];
                            }
                            string entryUserCode = d2.GetFunction("select distinct entryusercode from ft_findailytransaction where app_no='" + AppNo + "'");
                            string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + entryUserCode + "'").Trim();
                            #region Receipt Header

                            //  sbHtml.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");

                            sbHtml.Append("<div style='height: 560px;width:795px; border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");
                            sbHtml.Append("<div style=' width:790px; height:100px;'></div>");

                            //sbHtmlCopy.Append("<div style=' width:790px; height:#officeCopyHeight#px;'></div>");
                            //sbHtmlCopy.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");
                            sbHtmlCopy.Append("<div style='height:#officeCopyHeight#px; width:790px;'></div>");
                            if (ColName == 1)
                            {
                                sbHtml.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                sbHtml.Append("<br/>");

                                sbHtmlCopy.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                sbHtmlCopy.Append("<br/>");
                            }
                            sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + "  </td></tr><tr><td style='width:80px; '>Journal No </td><td style='width:240px; '>: " + recptNo + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>" + strMem + " </td><td style='width:160px; '>: " + Roll_admit + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                            sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " </td></tr><tr><td style='width:80px; '>Journal No </td><td style='width:240px; '>: " + recptNo + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>" + strMem + " </td><td style='width:160px; '>: " + Roll_admit + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                            #endregion

                            #region Receipt Body

                            sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                            sbHtmlCopy.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                            selectQuery = "";

                            int sno = 0;
                            int indx = 0;
                            double totalamt = 0;
                            double balanamt = 0;
                            double curpaid = 0;
                            // double paidamount = 0;


                            string selHeadersQ = string.Empty;
                            DataSet dsHeaders = new DataSet();


                            //New
                            selHeadersQ = " select SUM(Credit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0 ";

                            selHeadersQ += "  select distinct r.college_code,f.feecategory,r.degree_code,isnull(monthcode,'0')as monthcode,MonthYear from Fee_degree_match fd,registration r,FT_FinDailyTransaction f  where fd.college_code=r.college_code and f.app_no=r.app_no and f.feecategory=fd.feecategory and r.degree_code=fd.degree_code and r.college_code='" + collegecode1 + "' and r.App_No ='" + AppNo + "'";

                            //fine amount added by sudhagar 31.01.2017
                            selHeadersQ += " select SUM(debit) as TakenAmt,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,h.headername  from FT_FinDailyTransaction d,fm_headermaster h  where d.headerfk=h.headerpk and  d.transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and finefeecategory='-1'  group by D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk ,h.headername";
                            //New End

                            selHeadersQ += " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName having sum(isnull(debit,'0'))>0 and sum(isnull(credit,'0'))=0";
                            DataView dv = new DataView();
                            if (selHeadersQ != string.Empty)
                            {
                                string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                dsHeaders.Clear();
                                dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                if (dsHeaders.Tables.Count > 0)
                                {
                                    if (dsHeaders.Tables[0].Rows.Count > 0)
                                    {
                                        Hashtable htHdrAmt = new Hashtable();
                                        Hashtable htHdrName = new Hashtable();
                                        // Hashtable htfeecat = new Hashtable();
                                        int ledgCnt = 0;
                                        Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                                        Dictionary<string, double> htfeeAmt = new Dictionary<string, double>();
                                        for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                        {
                                            string disphdr = string.Empty;
                                            double allotamt0 = 0;
                                            double deductAmt0 = 0;
                                            double totalAmt0 = 0;
                                            double paidAmt0 = 0;
                                            double balAmt0 = 0;
                                            double creditAmt0 = 0;

                                            creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                            totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                            //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                            //paidAmt0 = totalAmt0 - balAmt0;
                                            deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                            disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                            string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                            string feecode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                            string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                            string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);

                                            string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                            paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                            #region Monthwise
                                            string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                            string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                            int monWisemon = 0;
                                            int monWiseYea = 0;
                                            string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                            string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                            int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                            int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                            if (monWisemon > 0 && monWiseYea > 0)
                                            {
                                                string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                DataSet dsMonwise = new DataSet();
                                                dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                {
                                                    totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                    paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                    disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                    balAmt0 = totalAmt0 - paidAmt0;
                                                }
                                            }
                                            else
                                            {
                                                balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                            }
                                            #endregion

                                            //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                            feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                            sno++;

                                            totalamt += Convert.ToDouble(totalAmt0);
                                            balanamt += Convert.ToDouble(balAmt0);
                                            curpaid += Convert.ToDouble(creditAmt0);

                                            deductionamt += Convert.ToDouble(deductAmt0);

                                            indx++;
                                            createPDFOK = true;
                                            if (disphdr != "")
                                                disphdr += "-" + "(CR_J)";
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                            //officeCopyHeight -= 20;
                                            ledgCnt++;
                                        }

                                        if (BalanceType == 1)
                                        {
                                            balanamt = retBalance(appnoNew);
                                        }

                                        #region DD Narration
                                        string modeMulti = string.Empty;
                                        bool multiCash = false;
                                        bool multiChk = false;
                                        bool multiDD = false;
                                        bool multiCard = false;

                                        DataSet dtMulBnkDetails = new DataSet();
                                        dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  else 'Card' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3,6) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                        string ddnar = string.Empty;
                                        string remarks = string.Empty;
                                        //double modeht = 40;
                                        if (narration != 0)
                                        {
                                            if (dtMulBnkDetails.Tables.Count > 0)
                                            {
                                                int sn = 1;
                                                for (int z = 0; z < dtMulBnkDetails.Tables[0].Rows.Count; z++)
                                                {
                                                    string strMode = Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]);
                                                    if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                                    {
                                                        multiCash = true;
                                                        continue;
                                                    }
                                                    else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                                    {
                                                        multiChk = true;
                                                    }
                                                    else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "DD")
                                                    {
                                                        multiDD = true;
                                                    }
                                                    else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CARD")
                                                    {
                                                        multiCard = true;
                                                        ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                        sn++;
                                                        continue;
                                                    }

                                                    ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                    sn++;
                                                }
                                                //modeht = dtMulBnkDetails.Tables[0].Rows.Count * 15;
                                                //modeht += 20;

                                            }
                                            remarks = d2.GetFunction("select narration from ft_findailytransaction where TransCode='" + recptNo.Trim() + "' and app_no='" + appnoNew + "' and isnull(iscanceled,0)=0");
                                            if (remarks.Trim() == "0")
                                                remarks = string.Empty;
                                            else
                                            {
                                                remarks = "\n" + remarks;
                                            }
                                            ddnar += remarks;

                                            if (excessRemaining(appnoNew) > 0)
                                                ddnar += " Excess Amount Rs. : " + excessRemaining(appnoNew);

                                        }

                                        if (multiCash)
                                        {
                                            modeMulti += "Cash,";
                                        }
                                        if (multiChk)
                                        {
                                            modeMulti += "Cheque,";
                                        }
                                        if (multiDD)
                                        {
                                            modeMulti += "DD,";
                                        }
                                        if (multiCard)
                                        {
                                            modeMulti += "Card";
                                        }
                                        modeMulti = modeMulti.TrimEnd(',');
                                        if (modeMulti != "")
                                        {
                                            mode = modeMulti;
                                        }
                                        //ddnar += remarks;
                                        #endregion

                                        double totalamount = curpaid;
                                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                        //  sbHtml.Append("</table></div><br>");

                                        sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                        //debit
                                        curpaid = 0;
                                        for (int head = 0; head < dsHeaders.Tables[3].Rows.Count; head++)
                                        {
                                            string disphdr = string.Empty;
                                            double allotamt0 = 0;
                                            double deductAmt0 = 0;
                                            double totalAmt0 = 0;
                                            double paidAmt0 = 0;
                                            double balAmt0 = 0;
                                            double creditAmt0 = 0;

                                            creditAmt0 = Convert.ToDouble(dsHeaders.Tables[3].Rows[head]["TakenAmt"]);
                                            totalAmt0 = Convert.ToDouble(dsHeaders.Tables[3].Rows[head]["TotalAmount"]);
                                            //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                            //paidAmt0 = totalAmt0 - balAmt0;
                                            deductAmt0 = Convert.ToDouble(dsHeaders.Tables[3].Rows[head]["DeductAmout"]);
                                            disphdr = Convert.ToString(dsHeaders.Tables[3].Rows[head]["DispName"]);
                                            string feecatcode = Convert.ToString(dsHeaders.Tables[3].Rows[head]["FeeCategory"]);
                                            string feecode = Convert.ToString(dsHeaders.Tables[3].Rows[head]["FeeCategory"]);
                                            string ledgFK = Convert.ToString(dsHeaders.Tables[3].Rows[head]["LedgerFK"]);
                                            string hdrFK = Convert.ToString(dsHeaders.Tables[3].Rows[head]["headerfk"]);

                                            string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                            paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                            #region Monthwise
                                            string DailyTransPk = Convert.ToString(dsHeaders.Tables[3].Rows[head]["DailyTransPk"]);
                                            string FeeAllotPk = Convert.ToString(dsHeaders.Tables[3].Rows[head]["FeeAllotPk"]);
                                            int monWisemon = 0;
                                            int monWiseYea = 0;
                                            string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                            string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                            int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                            int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                            if (monWisemon > 0 && monWiseYea > 0)
                                            {
                                                string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                DataSet dsMonwise = new DataSet();
                                                dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                {
                                                    totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                    paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                    disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                    balAmt0 = totalAmt0 - paidAmt0;
                                                }
                                            }
                                            else
                                            {
                                                balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                            }
                                            #endregion

                                            //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                            feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                            sno++;

                                            totalamt += Convert.ToDouble(totalAmt0);
                                            balanamt += Convert.ToDouble(balAmt0);
                                            curpaid += Convert.ToDouble(creditAmt0);

                                            deductionamt += Convert.ToDouble(deductAmt0);

                                            indx++;
                                            createPDFOK = true;
                                            if (disphdr != "")
                                                disphdr += "-" + "(DR_J)";
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                            //officeCopyHeight -= 20;
                                            // ledgCnt++;
                                        }

                                        if (curpaid != 0)
                                        {
                                            if (BalanceType == 1)
                                            {
                                                balanamt = retBalance(appnoNew);
                                            }

                                            #region DD Narration
                                            modeMulti = string.Empty;
                                            multiCash = false;
                                            multiChk = false;
                                            multiDD = false;
                                            multiCard = false;

                                            dtMulBnkDetails = new DataSet();
                                            dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  else 'Card' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3,6) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                            ddnar = string.Empty;
                                            remarks = string.Empty;
                                            //double modeht = 40;
                                            if (narration != 0)
                                            {
                                                if (dtMulBnkDetails.Tables.Count > 0)
                                                {
                                                    int sn = 1;
                                                    for (int z = 0; z < dtMulBnkDetails.Tables[0].Rows.Count; z++)
                                                    {
                                                        string strMode = Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]);
                                                        if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                                        {
                                                            multiCash = true;
                                                            continue;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                                        {
                                                            multiChk = true;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "DD")
                                                        {
                                                            multiDD = true;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CARD")
                                                        {
                                                            multiCard = true;
                                                            ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                            sn++;
                                                            continue;
                                                        }

                                                        ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                        sn++;
                                                    }
                                                    //modeht = dtMulBnkDetails.Tables[0].Rows.Count * 15;
                                                    //modeht += 20;

                                                }
                                                remarks = d2.GetFunction("select narration from ft_findailytransaction where TransCode='" + recptNo.Trim() + "' and app_no='" + appnoNew + "' and isnull(iscanceled,0)=0");
                                                if (remarks.Trim() == "0")
                                                    remarks = string.Empty;
                                                else
                                                {
                                                    remarks = "\n" + remarks;
                                                }
                                                ddnar += remarks;

                                                if (excessRemaining(appnoNew) > 0)
                                                    ddnar += " Excess Amount Rs. : " + excessRemaining(appnoNew);

                                            }

                                            if (multiCash)
                                            {
                                                modeMulti += "Cash,";
                                            }
                                            if (multiChk)
                                            {
                                                modeMulti += "Cheque,";
                                            }
                                            if (multiDD)
                                            {
                                                modeMulti += "DD,";
                                            }
                                            if (multiCard)
                                            {
                                                modeMulti += "Card";
                                            }
                                            modeMulti = modeMulti.TrimEnd(',');
                                            if (modeMulti != "")
                                            {
                                                mode = modeMulti;
                                            }
                                            //ddnar += remarks;
                                            #endregion


                                            totalamount = curpaid;
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                        }

                                        sbHtml.Append("</table></div><br>");

                                        if (curpaid != 0)
                                        {
                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                        }


                                        //debit amount


                                        if (ledgCnt == 1)
                                            officeCopyHeight += 290; //270;
                                        else if (ledgCnt == 2)
                                            officeCopyHeight += 260; //240;
                                        else if (ledgCnt == 3)
                                            officeCopyHeight += 230;//210;
                                        else if (ledgCnt == 4)
                                            officeCopyHeight += 200;//180;
                                        else if (ledgCnt >= 5)
                                            officeCopyHeight += 155;// 170;// 150;
                                        // heightvar += officeCopyHeight;
                                        sbHtmlCopy.Append("</table></div><br>");
                                        sbHtmlCopy.Replace("#officeCopyHeight#", officeCopyHeight.ToString());
                                    }
                                }
                            }
                            sbHtml.Append((studOffiCopy == 1 ? sbHtmlCopy.ToString() : string.Empty) + "</td></tr></table></div>");
                            #endregion

                            contentDiv.InnerHtml += sbHtml.ToString();
                        }
                        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                        finally
                        {
                        }
                        createPDFOK = true;
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "No Records Found";
                    }
                }
                //    }
                //}
                        #endregion
                #region To print the Receipt
                if (createPDFOK)
                {
                    #region New Print
                    //contentDiv.InnerHtml += sbHtml.ToString();
                    contentDiv.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);

                    #endregion
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Receipt Cannot Be Generated";
                }
                #endregion
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Add Print Settings";
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
        }

    }

    //Reusable Methods
    private double retBalance(string appNo)
    {
        double ovBalAMt = 0;
        if (BalanceType == 1)
        {
            double.TryParse(d2.GetFunction(" select sum(isnull(totalAmount,0)-isnull(paidAmount,0)) as BalanceAmt from ft_feeallot where app_no =" + appNo + ""), out ovBalAMt);
        }
        return ovBalAMt;
    }
    private double excessRemaining(string appnoNew)
    {
        string excessamtQ = d2.GetFunction("select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt from FT_ExcessDet WHERE  App_No=" + appnoNew + " ");

        double excessamtValue = 0;
        double.TryParse(excessamtQ, out excessamtValue);
        return excessamtValue;
    }
    public void isContainsDecimal(double myValue)
    {
        bool hasFractionalPart = (myValue - Math.Round(myValue) != 0);
    }
    public string returnIntegerPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 0)
        {
            strVal = strvalArr[0];
        }
        return strVal;
    }
    public string returnDecimalPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 1)
        {
            strVal = strvalArr[1];
            if (strVal.Length >= 2)
            {
                strVal = strVal.Substring(0, 2);
            }
            else
            {
                while (2 != strVal.Length)
                {
                    strVal = strVal + "0";
                }
            }
        }
        else
        {
            strVal = "00";
        }
        return strVal;
    }
    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "I";
                    break;
                case "2":
                    romanLettervalue = "II";
                    break;
                case "3":
                    romanLettervalue = "III";
                    break;
                case "4":
                    romanLettervalue = "IV";
                    break;
                case "5":
                    romanLettervalue = "V";
                    break;
                case "6":
                    romanLettervalue = "VI";
                    break;
                case "7":
                    romanLettervalue = "VII";
                    break;
                case "8":
                    romanLettervalue = "VIII";
                    break;
                case "9":
                    romanLettervalue = "IX";
                    break;
                case "10":
                    romanLettervalue = "X";
                    break;
            }
        }
        return romanLettervalue;
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakhs";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    public string DecimalToWords(decimal number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + DecimalToWords(Math.Abs(number));

        string words = "";

        int intPortion = (int)number;
        decimal fraction = (number - intPortion) * 100;
        int decPortion = (int)fraction;

        words = ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
        if (decPortion > 0)
        {
            words += " and ";
            words += ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
            words += " Paise ";
        }
        return words;
    }
    public string NumberToWords(int number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }

        return words;
    }

    public string generateBarcode(string barCode)
    {
        string urlImg = Server.MapPath("~/BarCode/" + "barcodeimg" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".Jpeg");
        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
        using (Bitmap bitMap = new Bitmap(barCode.Length * 10, 20))
        {
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                Font oFont = new Font("IDAutomationHC39M", 16);
                PointF point = new PointF(2f, 2f);
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                graphics.DrawString("*" + barCode + "*", oFont, blackBrush, point);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                //bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //byte[] byteImage = ms.ToArray();

                //Convert.ToBase64String(byteImage);
                //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);


                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitMap.Save(urlImg, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return urlImg;
        }

    }
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
}