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

public partial class ExtensionReport : System.Web.UI.Page
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
    DataTable dt = new DataTable();
    DataRow dr = null;
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

    }

    public void rdbduelist_checkedChanged(object sender, EventArgs e)
    {
    }

    public void rdbCumulative_checkedChanged(object sender, EventArgs e)
    {

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
    public void btngo_Click(object sender, EventArgs e)
    {
        string hdText = string.Empty;
        string payMode = string.Empty;
        string ldText = string.Empty;
        string strInclude = string.Empty;
        if (cblclg.Items.Count > 0)
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
        hdText = Convert.ToString(getCblSelectedValue(chkl_studhed));
        ldText = Convert.ToString(getCblSelectedValue(chkl_studled));
        string fromdate = txt_fromdate.Text;
        string todate = txt_todate.Text;
        int sno = 0;
        string[] frdate = fromdate.Split('/');
        if (frdate.Length == 3)
            fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
        string[] tdate = todate.Split('/');
        if (tdate.Length == 3)
            todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
        string hdFK = getHeaderFK(hdText, collegecode);
        string ldFK = getLedgerFK(ldText, collegecode);
        string query = string.Empty;

        query = "select Roll_no,Reg_no,Stud_name,l.HeaderFK,LedgerFK,HeaderName,LedgerName,FeeCategory,textval,DueAmount ,ExtDueAmount ,Convert(varchar,DueDate ,103) as Duedate ,Convert(varchar,ExtDueDate  ,103) as ExtensionDuedate ,r.college_code   from Registration r,FeesDueExt f,FM_HeaderMaster h,FM_LedgerMaster l,textvaltable t where r.App_No=f.App_No  and l.HeaderFK =h.HeaderPK and f.HeaderFK =l.HeaderFK and f.LedgerFK =l.LedgerPK and t.TextCode =f.FeeCategory and f.HeaderFK in('" + hdFK + "') and f.LedgerFK in('" + ldFK + "') and r.college_code  in('" + collegecode + "')";
        ds = d2.select_method_wo_parameter(query, "Text");
        print.Visible = true;
        lblrptname.Visible = true;
        txtexcelname.Visible = true;
        btnExcel.Visible = true;
        btnprintmasterhed.Visible = true;
        dt.Columns.Add("Sno", typeof(string));
        dt.Columns.Add("Reg No", typeof(string));
        dt.Columns.Add("Roll No", typeof(string));
        dt.Columns.Add("Student Name", typeof(string));
        dt.Columns.Add("Header Name", typeof(string));
        dt.Columns.Add("Ledger Name", typeof(string));
        dt.Columns.Add("Semester", typeof(string));
        dt.Columns.Add("DueAmount", typeof(string));
        dt.Columns.Add("Extension DueAmount", typeof(string));
        dt.Columns.Add("DueDate", typeof(string));
        dt.Columns.Add("Extension DueDate", typeof(string));
        dr = dt.NewRow();
        dr["SNo"] = "SNo";
        dr["Reg No"] = "Reg No";
        dr["Roll No"] = "Roll No";
        dr["Student Name"] = "Student Name";
        dr["Header Name"] = "Header Name";
        dr["Ledger Name"] = "Ledger Name";
        dr["Semester"] = "Semester";
        dr["DueAmount"] = "DueAmount";
        dr["Extension DueAmount"] = "Extension DueAmount";
        dr["DueDate"] = "DueDate";
        dr["Extension DueDate"] = "Extension DueDate";
        dt.Rows.Add(dr);
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                sno++;
                dr = dt.NewRow();
                dr["SNo"] = Convert.ToString(sno);
                dr["Reg No"] = Convert.ToString(ds.Tables[0].Rows[i]["reg_no"]);
                dr["Roll No"] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_no"]);
                dr["Student Name"] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_name"]);
                dr["Header Name"] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                dr["Ledger Name"] = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                dr["Semester"] = Convert.ToString(ds.Tables[0].Rows[i]["textval"]);
                dr["DueAmount"] = Convert.ToString(ds.Tables[0].Rows[i]["DueAmount"]);
                dr["Extension DueAmount"] = Convert.ToString(ds.Tables[0].Rows[i]["ExtDueAmount"]);
                dr["DueDate"] = Convert.ToString(ds.Tables[0].Rows[i]["Duedate"]);
                dr["Extension DueDate"] = Convert.ToString(ds.Tables[0].Rows[i]["ExtensionDuedate"]);
                dt.Rows.Add(dr);

            }
        }
        GridExtentionRpt.DataSource = dt;
        GridExtentionRpt.DataBind();
        GridExtentionRpt.Visible = true;
        RowHead(GridExtentionRpt);
    }

    protected void RowHead(GridView GridExtentionRpt)
    {
        for (int head = 0; head < 1; head++)
        {
            GridExtentionRpt.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            GridExtentionRpt.Rows[head].Font.Bold = true;
            GridExtentionRpt.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
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
                lbl_alert.Visible = false;
                d2.printexcelreportgrid(GridExtentionRpt, reportname);
            }
            else
            {
                txtexcelname.Focus();
                lbl_alert.Text = "Please Enter Your Report Name";
                lbl_alert.Visible = true;
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }



    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            // lblvalidation1.Text = "";
            string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            //   string counterName = getCounterName(Convert.ToString(getCblSelectedValue(cbluser)));


            string degreedetails;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            degreedetails = "Individual Student Daybook Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
            //  degreedetails = "Individual Student Daybook Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "User/Counter : " + counterName;
            pagename = "ExtensionReport.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed.loadspreaddetails(GridExtentionRpt, pagename, degreedetails, 0, ss);
            //Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails, 0, Convert.ToString(Session["usercode"]));
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

    private double checkSchoolSetting()//delsi
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
}