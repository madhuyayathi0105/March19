using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Drawing;
using System.Configuration;


public partial class FinanceMod_StaffVendorOtherFeeEdit : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string selectQuery = string.Empty;
    static string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int ledgerorheader = 0;
    static string collegecodestat = string.Empty;
    static string usercodestat = string.Empty;
    static string vencontcode = "-1";


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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
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
                loadcollege();
                headerbind();
                ledgerbind();

            }
            if (ddl_college.Items.Count > 0)
                collegecode = Convert.ToString(ddl_college.SelectedValue);
            collegecodestat = collegecode;
            usercodestat = usercode;
            if (ddl_ledgeSearch.SelectedIndex == 0)
            {
                ledgerorheader = 0;
            }
            else
            {
                ledgerorheader = 1;
            }

            if (rbl_rollnoNew.SelectedIndex == 2 && txtroll_vendor.Text.Trim() != "")
            {
                //this.Form.DefaultButton = "btnGO_vendor";
                try
                {
                    vencontcode = txtroll_vendor.Text.Trim().Split('-')[2];
                }
                catch { vencontcode = "-1"; }
            }
            else
            {
                vencontcode = "-1";
            }
        }
        catch (Exception ex) { }
    }

    #region college
    public void loadcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ddl_college.Items.Clear();
            ds.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
            if (ddl_college.Items.Count > 0)
                collegecode = Convert.ToString(ddl_college.SelectedValue);
        }
        catch
        { }
    }
    #endregion
    //staff
    protected void txtroll_staff_Changed(object sender, EventArgs e)
    {
        string name = string.Empty;
        string degree = string.Empty;
        string college = string.Empty;
        string staffId = Convert.ToString(txtroll_staff.Text.Trim());
        //img_stud.ImageUrl = "";
        //img_stud.Visible = false;

        if (staffId != "")
        {
            if (rbl_rollnoNew.Text == "Staff")
            {
                //string name = string.Empty;
                //string degree = string.Empty;

                // string query = "select staff_name,appl_no,ISNULL( Stream,'') as type from staffmaster where resign<>1 and college_code="+collegecode1+" and staff_code='" + staffId + "'";

                string query = " select appl_id ,h.dept_name,h.dept_code,s.staff_name,s.staff_code,c.collname  from collinfo c,staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code and s.staff_code ='" + staffId + "' and s.college_Code in('" + collegecode + "') ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            name = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                            degree = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            //lbltype.Text = Convert.ToString(ds.Tables[0].Rows[i]["type"]);      
                            college = Convert.ToString(ds.Tables[0].Rows[i]["collname"]);
                        }
                    }
                }

                txtname_staff.Text = name;
                txtDept_staff.Text = degree;

                //img_stud.ImageUrl = "~/Handler/ghStaffFoto.ashx?QSstaff_id=" + staffId;
                //img_stud.Visible = true;
            }

        }


    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            //staff query
            query = " select staff_code from staffmaster where resign<>1 and staff_code like '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_code asc";


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        string query = " select top 100 staff_name+'-'+staff_code from staffmaster where resign<>1 and staff_name like '" + prefixText + "%' and college_code='" + collegecode + "'  order by staff_name asc";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }
    protected void cb_HeaderPop_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_HeaderPop, cb_HeaderPop, txt_HeaderPop, "Header");
        ledgerbind();
        setSearchHeaders();
    }
    protected void cbl_HeaderPop_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_HeaderPop, cb_HeaderPop, txt_HeaderPop, "Header");
        ledgerbind();
        setSearchHeaders();
    }
    public void headerbind()
    {
        try
        {
            txt_HeaderPop.Text = "Header";
            cb_HeaderPop.Checked = false;
            cbl_HeaderPop.Items.Clear();

            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode + "   ";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_HeaderPop.DataSource = ds;
                cbl_HeaderPop.DataTextField = "HeaderName";
                cbl_HeaderPop.DataValueField = "HeaderPK";
                cbl_HeaderPop.DataBind();
                for (int i = 0; i < cbl_HeaderPop.Items.Count; i++)
                {
                    cbl_HeaderPop.Items[i].Selected = true;
                }
                txt_HeaderPop.Text = "Header(" + cbl_HeaderPop.Items.Count + ")";
                cb_HeaderPop.Checked = true;
            }

        }
        catch (Exception ex) { }
    }
    protected void cb_ledgerpop_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_ledgerpop, cb_ledgerpop, txt_Ledgerpop, "Ledger");
    }
    protected void cbl_ledgerpop_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_ledgerpop, cb_ledgerpop, txt_Ledgerpop, "Ledger");
    }
    public void ledgerbind()
    {
        try
        {
            txt_Ledgerpop.Text = "Ledger";
            cb_ledgerpop.Checked = false;
            string itemheadercode = "";
            for (int i = 0; i < cbl_HeaderPop.Items.Count; i++)
            {
                if (cbl_HeaderPop.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_HeaderPop.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "" + "," + "" + cbl_HeaderPop.Items[i].Value.ToString() + "";
                    }
                }
            }

            cbl_ledgerpop.Items.Clear();

            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";

            string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode  and l.LedgerMode=0   AND P. UserCode = " + usercode + " AND L.CollegeCode = " + collegecode + " and L.HeaderFK in (" + itemheadercode + ")";

            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ledgerpop.DataSource = ds;
                cbl_ledgerpop.DataTextField = "LedgerName";
                cbl_ledgerpop.DataValueField = "LedgerPK";
                cbl_ledgerpop.DataBind();
                for (int i = 0; i < cbl_ledgerpop.Items.Count; i++)
                {
                    cbl_ledgerpop.Items[i].Selected = true;
                }
                txt_Ledgerpop.Text = "Ledger(" + cbl_ledgerpop.Items.Count + ")";
                cb_ledgerpop.Checked = true;
            }
        }
        catch (Exception ex) { }
    }
    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }
    protected void ddl_ledgeSearch_Change(object sender, EventArgs e)
    {
        setSearchHeaders();
        //this.Form.DefaultButton = "btn_ledgesearch";
    }
    static string srchSelHead = string.Empty;
    public void setSearchHeaders()
    {
        StringBuilder sbSelHead = new StringBuilder();
        for (int i = 0; i < cbl_HeaderPop.Items.Count; i++)
        {
            if (cbl_HeaderPop.Items[i].Selected)
            {
                if (sbSelHead.Length == 0)
                {
                    sbSelHead.Append(cbl_HeaderPop.Items[i].Value);
                }
                else
                {
                    sbSelHead.Append("','" + cbl_HeaderPop.Items[i].Value);
                }
            }
        }
        srchSelHead = sbSelHead.ToString();
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetLegerName(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            WebService ws = new WebService();
            string query = " ";

            if (ledgerorheader == 0)
            {
                query = "SELECT  top 100 HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercodestat + " AND H.CollegeCode = " + collegecodestat + " and HeaderName Like '" + prefixText + "%' and H.HeaderPK in ('" + srchSelHead + "')   order by HeaderName asc ";

            }
            else
            {
                query = "SELECT  top 100 LedgerName,LedgerPK FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK  and l.LedgerMode=0     AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercodestat + " AND L.CollegeCode = " + collegecodestat + "  and L.HeaderFK in ('" + srchSelHead + "')  and LedgerName like '" + prefixText + "%'   order by LedgerName asc";
            }


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    protected void grid_HeaderLedger_OnDataBound(object sender, EventArgs e)
    {

        try
        {
            double paiColor = 0;
            double totColor = 0;
            for (int i = 0; i < grid_HeaderLedger.Rows.Count; i++)
            {
                TextBox txttotamt = (TextBox)grid_HeaderLedger.Rows[i].FindControl("txt_NewLedger");
                TextBox txtpaiamt = (TextBox)grid_HeaderLedger.Rows[i].FindControl("txt_paid");

                if (txttotamt.Text.Trim() != "")
                {
                    totColor = Convert.ToDouble(txttotamt.Text.Trim());
                }
                //if (txtpaiamt.Text.Trim() != "")
                //{
                //    paiColor = Convert.ToDouble(txtpaiamt.Text.Trim());
                //}

                Color clr = new Color();
                if (paiColor == totColor)
                {
                    //Full fees paid 
                    clr = Color.FromArgb(144, 238, 144);
                }
                else if (paiColor > 0 && totColor > 0)
                {
                    //If Partial Paid
                    clr = Color.FromArgb(255, 182, 193);
                }
                else
                {
                    clr = Color.White;
                }
                for (int j = 0; j < grid_HeaderLedger.Columns.Count; j++)
                {
                    grid_HeaderLedger.Rows[i].Cells[j].BackColor = clr;
                }
            }
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode1, "ChallanReceipt"); 
        }
    }
    protected void btn_ledgesearch_Click(object sender, EventArgs e)
    {
        try
        {
            //this.Form.DefaultButton = "btn_ledgesearch";
            DataTable tbl_Ledger = new DataTable();
            tbl_Ledger.Columns.Add("HeaderName");
            tbl_Ledger.Columns.Add("HeaderPK");
            tbl_Ledger.Columns.Add("LedgerPK");
            tbl_Ledger.Columns.Add("LedgerName");
            tbl_Ledger.Columns.Add("Total");


            string itemheadercode = GetSelectedItemsValue(cbl_HeaderPop);

            string feecode = GetSelectedItemsValue(cbl_ledgerpop);

            string query = " select  HeaderName,HeaderPK,LedgerPK,priority ,LedgerName from  FM_LedgerMaster L,FS_LedgerPrivilage P,FM_HeaderMaster H,FS_HeaderPrivilage Ph WHERE L.LedgerPK = P.LedgerFK and H.HeaderPK = Ph.HeaderFK and p.HeaderFK=H.HeaderPK  and l.LedgerMode=0 and ph.UserCode =p.UserCode  and p.UserCode =" + usercode + "  and h.CollegeCode=" + collegecode + " and L.LedgerPK in (" + feecode + ") and H.HeaderPK in (" + itemheadercode + ")  order by Headerpk,LedgerName asc --  order by case when priority is null then 1 else 0 end, priority";
            if (txt_ledgeSearch.Text.Trim() != "")
            {
                if (ddl_ledgeSearch.SelectedIndex == 0)
                {
                    query = " select  HeaderName,HeaderPK,LedgerPK,priority ,LedgerName from  FM_LedgerMaster L,FS_LedgerPrivilage P,FM_HeaderMaster H,FS_HeaderPrivilage Ph WHERE L.LedgerPK = P.LedgerFK and H.HeaderPK = Ph.HeaderFK and p.HeaderFK=H.HeaderPK  and l.LedgerMode=0  and ph.UserCode =p.UserCode  and p.UserCode =" + usercode + "   and h.CollegeCode=" + collegecode + " and  H.HeaderName in ('" + txt_ledgeSearch.Text + "')  order by Headerpk,LedgerName asc --  order by case when priority is null then 1 else 0 end, priority";
                }
                else
                {
                    query = " select  HeaderName,HeaderPK,LedgerPK,priority ,LedgerName from  FM_LedgerMaster L,FS_LedgerPrivilage P,FM_HeaderMaster H,FS_HeaderPrivilage Ph WHERE L.LedgerPK = P.LedgerFK and H.HeaderPK = Ph.HeaderFK and p.HeaderFK=H.HeaderPK  and l.LedgerMode=0  and ph.UserCode =p.UserCode  and p.UserCode =" + usercode + "   and h.CollegeCode=" + collegecode + " and L.LedgerName in ('" + txt_ledgeSearch.Text + "') order by Headerpk,LedgerName asc -- order by case when priority is null then 1 else 0 end, priority";
                }
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow drLedger = tbl_Ledger.NewRow();
                        drLedger["HeaderName"] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                        drLedger["HeaderPK"] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderPK"]);
                        drLedger["LedgerPK"] = Convert.ToString(ds.Tables[0].Rows[i]["LedgerPK"]);
                        drLedger["LedgerName"] = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                        //drLedger["Total"] = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                        tbl_Ledger.Rows.Add(drLedger);
                    }

                    grid_HeaderLedger.DataSource = tbl_Ledger;
                    grid_HeaderLedger.DataBind();
                    grid_HeaderLedger.Visible = true;
                    btn_ledgersave.Visible = true;
                    // btn_ledgerExit.Visible = true;
                }
                else
                {
                    grid_HeaderLedger.DataSource = null;
                    grid_HeaderLedger.DataBind();
                    btn_ledgersave.Visible = false;
                    //btn_ledgerExit.Visible = false;
                    // imgAlert.Visible = true;
                    //this.Form.DefaultButton = "btn_alertclose";
                    // lbl_alert.Text = "No Records Found";
                }
            }
            else
            {
                grid_HeaderLedger.DataSource = null;
                grid_HeaderLedger.DataBind();
                btn_ledgersave.Visible = false;
                //   btn_ledgerExit.Visible = false;
                // imgAlert.Visible = true;
                //this.Form.DefaultButton = "btn_alertclose";
                // lbl_alert.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "ChallanReceipt");

            grid_HeaderLedger.DataSource = null;
            grid_HeaderLedger.DataBind();
            btn_ledgersave.Visible = false;
            // btn_ledgerExit.Visible = false;
        }
    }
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch (Exception ex) { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    protected void btnpopLedgersave_Click(object sender, EventArgs e)
    {
        int insok = 0;
        bool insOK = false;
        bool isVal = false;
        if (grid_HeaderLedger.Rows.Count > 0)
        {
            double payAMTVal = 0.00;
            double FeeAllot = 0;
            double PaidAmt = 0;
            double BalanceAmt = 0;
            double finalAllot = 0;

            #region staff
            if (rbl_rollnoNew.SelectedIndex == 0)
            {
                //Year or sem
                string clgcode = string.Empty;
                clgcode = getClgCode();
                string rollno = txtroll_staff.Text.Trim();
                string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode);
                string selectQuery = "";
                string updateQuery = "";
                if (rollno != "")
                {
                    for (int row = 0; row < grid_HeaderLedger.Rows.Count; row++)
                    {
                        Label lbl_headerId = (Label)grid_HeaderLedger.Rows[row].FindControl("lbl_headeridpop");
                        Label lbl_fee_code = (Label)grid_HeaderLedger.Rows[row].FindControl("lbl_ledgeridpop");
                        Label lbl_fee_cat = (Label)grid_HeaderLedger.Rows[row].FindControl("lbl_legerpop");
                        TextBox txtNewLedger = (TextBox)grid_HeaderLedger.Rows[row].FindControl("txt_NewLedger");

                        string hid = lbl_headerId.Text;
                        string lid = lbl_fee_code.Text;
                        string lname = lbl_fee_cat.Text;
                        string amt = txtNewLedger.Text;

                        string text_circode = string.Empty;
                        string appno = string.Empty;
                        string cursem = "0";
                        appno = d2.GetFunction("select a.appl_id from staffmaster s,staff_appl_master a where s.appl_no=a.appl_no and s.staff_code='" + rollno + "' and s.college_code in('" + clgcode + "') ");

                        if (amt != "" && Convert.ToDouble(amt) != null && amt != "0")
                        {
                            if (appno != "" && appno != "0")
                            {
                                text_circode = cursem;
                                string memtype = "1";
                                switch (rbl_rollnoNew.SelectedIndex)
                                {
                                    case 0:
                                        memtype = "2";
                                        break;
                                    case 1:
                                        memtype = "3";
                                        break;
                                    case 2:
                                        memtype = "4"; ;
                                        break;
                                }
                                //if (amt == "")
                                //{
                                //    string delquery = "delete from ft_feeallot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and App_No in('" + appno + "')  and isnull(istransfer,'0')='0'";
                                //    insok = d2.update_method_wo_parameter(delquery, "Text");
                                //    insOK = true;
                                //}
                                string insertQuery = " INSERT INTO FT_FeeAllot(AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK,FromGovtAmt, DeductReason) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "'," + memtype + ",1," + appno + "," + lid + "," + hid + "," + amt + "," + amt + "," + text_circode + "," + amt + "," + finYeaid + ",0,0) ";

                                selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                                //Added by saranya on 25/5/2018
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQuery, "text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FeeAllot = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"]);
                                    PaidAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["PaidAmount"]);
                                    BalanceAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]);
                                    double FinalFeeAllot = Convert.ToDouble(FeeAllot) + Convert.ToDouble(amt);
                                    double FinalBalAmt = FinalFeeAllot - PaidAmt;
                                    updateQuery = " update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=" + memtype + ",FeeAmount=" + FinalFeeAllot + ",BalAmount=" + FinalBalAmt + ",TotalAmount=" + FinalFeeAllot + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";//isnull(TotalAmount,0)+
                                }
                                else
                                {
                                    updateQuery = " update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=" + memtype + ",FeeAmount=" + amt + ",BalAmount=" + amt + ",TotalAmount=" + amt + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                                }

                                string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                                insok = d2.update_method_wo_parameter(finalQuery, "Text");
                                insOK = true;

                                double amtVal = Convert.ToDouble(amt);
                                payAMTVal += amtVal;
                            }
                        }
                        //Added by saranya on 25/5/2018
                        if (amt == "0")
                        {
                            selectQuery = "select * from ft_feeallot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and App_No in('" + appno + "')";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQuery, "text");
                            FeeAllot = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"]);
                            PaidAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["PaidAmount"]);
                            BalanceAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]);
                            double Amount = FeeAllot - BalanceAmt;
                            if (Amount == PaidAmt)
                            {
                                updateQuery = "update FT_FeeAllot set  AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',FeeAmount=" + Amount + ",BalAmount=" + amt + ",TotalAmount=" + Amount + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                                insok = d2.update_method_wo_parameter(updateQuery, "Text");
                                insOK = true;
                            }
                        }
                    }
                }
            }

            #endregion
            #region vendor
            if (rbl_rollnoNew.SelectedIndex == 1)
            {
                //Year or sem
                string rollno = txtname_vendor.Text.Trim();
                try
                {
                    rollno = rollno.Split('-')[2];
                }
                catch { rollno = ""; }
                string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode);

                string selectQuery = "";
                string updateQuery = "";

                if (rollno != "")
                {
                    for (int row = 0; row < grid_HeaderLedger.Rows.Count; row++)
                    {
                        Label lbl_headerId = (Label)grid_HeaderLedger.Rows[row].FindControl("lbl_headeridpop");
                        Label lbl_fee_code = (Label)grid_HeaderLedger.Rows[row].FindControl("lbl_ledgeridpop");
                        Label lbl_fee_cat = (Label)grid_HeaderLedger.Rows[row].FindControl("lbl_legerpop");
                        TextBox txtNewLedger = (TextBox)grid_HeaderLedger.Rows[row].FindControl("txt_NewLedger");

                        string hid = lbl_headerId.Text;
                        string lid = lbl_fee_code.Text;
                        string lname = lbl_fee_cat.Text;
                        string amt = txtNewLedger.Text;

                        string text_circode = string.Empty;
                        string appno = string.Empty;
                        string cursem = "0";

                        appno = rollno;
                        if (amt != "" && Convert.ToDouble(amt) != null && Convert.ToDouble(amt) != 0 && amt != "0")
                        {
                            if (appno != "" && appno != "0")
                            {
                                text_circode = cursem;
                                string memtype = "1";
                                switch (rbl_rollnoNew.SelectedIndex)
                                {
                                    case 0:
                                        memtype = "1";
                                        break;
                                    case 1:
                                        memtype = "2";
                                        break;
                                    case 2:
                                        memtype = "3";
                                        break;
                                    case 3:
                                        memtype = "4"; ;
                                        break;
                                }

                                string insertQuery = " INSERT INTO FT_FeeAllot(AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK,FromGovtAmt, DeductReason) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "'," + memtype + ",1," + appno + "," + lid + "," + hid + "," + amt + "," + amt + "," + text_circode + "," + amt + "," + finYeaid + ",0,0) ";

                                selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";

                                //Added by saranya on 25/5/2018
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQuery, "text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FeeAllot = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"]);
                                    PaidAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["PaidAmount"]);
                                    BalanceAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]);
                                    double FinalFeeAllot = Convert.ToDouble(FeeAllot) + Convert.ToDouble(amt);
                                    double FinalBalAmt = FinalFeeAllot - PaidAmt;
                                    updateQuery = " update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=" + memtype + ",FeeAmount=" + FinalFeeAllot + ",BalAmount=" + FinalBalAmt + ",TotalAmount=" + FinalFeeAllot + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";//isnull(TotalAmount,0)+
                                }
                                else
                                {
                                    updateQuery = "  update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=" + memtype + ",FeeAmount=" + amt + ",BalAmount=isnull(BalAmount,0)+" + amt + ",TotalAmount=isnull(TotalAmount,0)+" + amt + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";//isnull(FeeAmount,0)+"
                                }

                                
                                string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                                insok = d2.update_method_wo_parameter(finalQuery, "Text");
                                insOK = true;

                                double amtVal = Convert.ToDouble(amt);
                                payAMTVal += amtVal;
                            }
                        }
                        //Added by saranya on 25/5/2018
                        if (amt == "0")
                        {
                            selectQuery = "select * from ft_feeallot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and App_No in('" + appno + "')";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selectQuery, "text");
                            FeeAllot = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"]);
                            PaidAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["PaidAmount"]);
                            BalanceAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]);
                            double Amount = FeeAllot - BalanceAmt;
                            if (Amount == PaidAmt)
                            {
                                updateQuery = "update FT_FeeAllot set  AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',FeeAmount=" + Amount + ",BalAmount=" + amt + ",TotalAmount=" + Amount + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                                insok = d2.update_method_wo_parameter(updateQuery, "Text");
                                insOK = true;
                            }
                        }
                    }
                }
            }
            #endregion
            #region other
            if (rbl_rollnoNew.SelectedIndex == 2)
            {
                string newVenCode = generateVendorCode().Trim();
                string staffId = Convert.ToString(txtroll_other.Text.Trim());
                string staffMob = Convert.ToString(txt_otherMobile.Text.Trim());
                string selectQuery = "";
                string updateQuery = "";
                try
                {
                    d2.update_method_wo_parameter("if not exists (select VendorCode from co_vendormaster where vendorname='" + staffId + "' and VendorMobileNo='" + staffMob + "'  and VendorType=-5) insert into co_vendormaster  (VendorCode,vendorname,VendorMobileNo,VendorCompName,VendorType) values ('" + newVenCode + "','" + staffId + "','" + staffMob + "','" + txtname_other.Text.Trim() + "',-5) else update co_vendormaster set VendorCompName='" + txtname_other.Text.Trim() + "'  where vendorname='" + staffId + "' and VendorMobileNo='" + staffMob + "'  and VendorType=-5 ", "Text");
                }
                catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "staffedit"); }
                newVenCode = d2.GetFunction("select VendorCode from co_vendormaster where vendorname='" + staffId + "' and VendorMobileNo='" + staffMob + "'  and VendorType=-5").Trim();
                if (newVenCode != "" && newVenCode != "0")
                {
                    #region Add Ledgers For Others
                    //Year or sem

                    string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode);
                    string venPk = d2.GetFunction("select VendorPK from co_vendormaster where VendorCode='" + newVenCode + "'  and VendorType=-5").Trim();
                    if (venPk != "" && venPk != "0")
                    {
                        for (int row = 0; row < grid_HeaderLedger.Rows.Count; row++)
                        {
                            Label lbl_headerId = (Label)grid_HeaderLedger.Rows[row].FindControl("lbl_headeridpop");
                            Label lbl_fee_code = (Label)grid_HeaderLedger.Rows[row].FindControl("lbl_ledgeridpop");
                            Label lbl_fee_cat = (Label)grid_HeaderLedger.Rows[row].FindControl("lbl_legerpop");
                            TextBox txtNewLedger = (TextBox)grid_HeaderLedger.Rows[row].FindControl("txt_NewLedger");

                            string hid = lbl_headerId.Text;
                            string lid = lbl_fee_code.Text;
                            string lname = lbl_fee_cat.Text;
                            string amt = txtNewLedger.Text;

                            string text_circode = string.Empty;
                            string appno = string.Empty;
                            string cursem = "0";

                            if (amt != "" && Convert.ToDouble(amt) != null && Convert.ToDouble(amt) != 0)
                            {
                                appno = venPk;

                                if (appno != "" && appno != "0")
                                {
                                    text_circode = cursem;
                                    string memtype = "1";
                                    switch (rbl_rollnoNew.SelectedIndex)
                                    {
                                        case 0:
                                            memtype = "1";
                                            break;
                                        case 1:
                                            memtype = "2";
                                            break;
                                        case 2:
                                            memtype = "3";
                                            break;
                                        case 3:
                                            memtype = "4"; ;
                                            break;
                                    }
                                    string insertQuery = " INSERT INTO FT_FeeAllot(AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK,FromGovtAmt, DeductReason) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "'," + memtype + ",1," + appno + "," + lid + "," + hid + "," + amt + "," + amt + "," + text_circode + "," + amt + "," + finYeaid + ",0,0) ";

                                    selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                                    //Added by saranya on 25/5/2018
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(selectQuery, "text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        FeeAllot = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"]);
                                        PaidAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["PaidAmount"]);
                                        BalanceAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]);
                                        double FinalFeeAllot = Convert.ToDouble(FeeAllot) + Convert.ToDouble(amt);
                                        double FinalBalAmt = FinalFeeAllot - PaidAmt;
                                        updateQuery = " update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=" + memtype + ",FeeAmount=" + FinalFeeAllot + ",BalAmount=" + FinalBalAmt + ",TotalAmount=" + FinalFeeAllot + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";//isnull(TotalAmount,0)+
                                    }
                                    else
                                    {
                                        updateQuery = "  update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType=" + memtype + ",FeeAmount=" + amt + ",TotalAmount=" + amt + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";//BalAmount=" + amt + ",
                                    }
                                    string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                                    insok = d2.update_method_wo_parameter(finalQuery, "Text");
                                    insOK = true;
                                    double amtVal = Convert.ToDouble(amt);
                                    payAMTVal += amtVal;
                                }
                            }
                            //Added by saranya on 25/5/2018
                            if (amt == "0")
                            {
                                selectQuery = "select * from ft_feeallot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and App_No in('" + appno + "')";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selectQuery, "text");
                                FeeAllot = Convert.ToDouble(ds.Tables[0].Rows[0]["TotalAmount"]);
                                PaidAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["PaidAmount"]);
                                BalanceAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["BalAmount"]);
                                double Amount = FeeAllot - BalanceAmt;
                                if (Amount == PaidAmt)
                                {
                                    updateQuery = "update FT_FeeAllot set  AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',FeeAmount=" + Amount + ",BalAmount=" + amt + ",TotalAmount=" + Amount + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "')  and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";
                                    insok = d2.update_method_wo_parameter(updateQuery, "Text");
                                    insOK = true;
                                }
                            }
                        }
                    }
                    #endregion

                }
            }

            #endregion
            imgAlert.Visible = true;
            if (insOK)
            {
                lbl_alert.Text = "Saved Sucessfully";
            }

        }

    }
    protected string getClgCode()
    {
        string clgCode = string.Empty;
        try
        {
            StringBuilder sbClg = new StringBuilder();
            for (int row = 0; row < ddl_college.Items.Count; row++)
            {
                sbClg.Append(Convert.ToString(ddl_college.Items[row].Value) + "','");
            }
            if (sbClg.Length > 0)
            {
                clgCode = Convert.ToString(sbClg.Remove(sbClg.Length - 3, 3));
            }
        }
        catch { }
        return clgCode;
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;

    }
    protected void btnedit_Click(object sender, EventArgs e)
    {
        if (rbl_rollnoNew.SelectedIndex == 0)
        {
            loadGridStaff();
        }
        if (rbl_rollnoNew.SelectedIndex == 1)
        {
            loadGridVendor();
        }
        if (rbl_rollnoNew.SelectedIndex == 2)
        {
            loadGridOthers();
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
    public void loadGridStaff()
    {
        //for different staff from multiple college
        string clgcode = string.Empty;
        string finYearFk = string.Empty;
        string curfinYearid = d2.getCurrentFinanceYear(usercode, collegecode);
        //if (chklsfyear.Items.Count > 0)
        //    finYearFk = Convert.ToString(getCblSelectedValue(chklsfyear));
        clgcode = getClgCode();
        //else
        //    clgcode = collegecode1;
        // loadScholarship("-1");
        //fromScript = false;
        try
        {

            double totamt = 0;
            double paid = 0;
            string roll_no = string.Empty;
            string semyear = string.Empty;
            string appnoNew = string.Empty;
            string degcode = string.Empty;
            roll_no = txtroll_staff.Text.Trim();

            DataTable tbl_Student = new DataTable();
            tbl_Student.Columns.Add("Roll_No");
            tbl_Student.Columns.Add("Reg_No");
            tbl_Student.Columns.Add("Stud_Name");
            tbl_Student.Columns.Add("Degree");
            tbl_Student.Columns.Add("TextVal");
            tbl_Student.Columns.Add("TextCode");
            tbl_Student.Columns.Add("HeaderPK");
            tbl_Student.Columns.Add("HeaderName");
            tbl_Student.Columns.Add("LedgerPK");
            tbl_Student.Columns.Add("LedgerName");
            tbl_Student.Columns.Add("Fee_Amount");
            tbl_Student.Columns.Add("Deduct");
            tbl_Student.Columns.Add("Total");
            tbl_Student.Columns.Add("PaidAmt");
            //tbl_Student.Columns.Add("BalAmt");
            //tbl_Student.Columns.Add("ToBePaid");
            //tbl_Student.Columns.Add("Monthly");
            //tbl_Student.Columns.Add("ChlTaken");
            //tbl_Student.Columns.Add("Scholar");
            //tbl_Student.Columns.Add("CautionDep");
            //tbl_Student.Columns.Add("MonwiseMon");
            //tbl_Student.Columns.Add("MonwiseYear");
            //tbl_Student.Columns.Add("FeeallotPk");
            //tbl_Student.Columns.Add("finyearfk");
            //tbl_Student.Columns.Add("finyear");
            string selectQuery = "";

            string queryRollApp = " select appl_id ,h.dept_name,h.dept_code,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code and s.staff_code ='" + roll_no + "' and s.college_Code in('" + clgcode + "') ";

            DataSet dsRollApp = new DataSet();
            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
            if (dsRollApp.Tables.Count > 0)
            {
                if (dsRollApp.Tables[0].Rows.Count > 0)
                {

                    roll_no = Convert.ToString(dsRollApp.Tables[0].Rows[0]["staff_code"]);
                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["appl_id"]);
                    degcode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["dept_code"]);

                    //lblstaticrollno.Text = roll_no;

                    //img_stud.ImageUrl = "~/Handler/ghStaffFoto.ashx?QSstaff_id=" + roll_no;
                    //img_stud.Visible = true;
                }
                else
                {
                    roll_no = "";
                    appnoNew = "";
                    //lblstaticrollno.Text = "";
                    //img_stud.Visible = false;
                }
            }
            else
            {
                roll_no = "";
                //lblstaticrollno.Text = "";
                //img_stud.Visible = false;
            }


            selectQuery = "  SELECT A.HeaderFK,HeaderName,A.LedgerFK,LedgerName,Priority,isnull(FeeAmount,0) as FeeAmount,isnull(DeductAmout,0)  as DeductAmount,isnull(TotalAmount,0) as TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,isnull(PaidAmount,0) as PaidAmount,isnull(TotalAmount,0)-isnull(PaidAmount,0)  as BalAmount,( select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend))) as finyearfk from fm_finyearmaster fm where a.finyearfk=fm.finyearpk )as finyear,a.finyearfk FROM FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK  and l.LedgerMode=0 and MemType=2 AND A.App_No = " + appnoNew + " and isnull(istransfer,'0')='0' and BalAmount != 0";// and T.TextCode in('" + semyear + "')

            string headercode = getCblSelectedValue(cbl_HeaderPop);
            string ledgercode = getCblSelectedValue(cbl_ledgerpop);
            //Header
            selectQuery += "  and A.HeaderFK in ('" + headercode + "') ";

            //Ledger
            selectQuery += "  and A.LedgerFK  in ('" + ledgercode + "')  ";


            //selectQuery += "  order by case when priority is null then 1 else 0 end, priority,a.finyearfk asc";

            DataSet ds_stud = new DataSet();
            ds_stud.Clear();
            ds_stud = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds_stud.Tables.Count > 0)
            {
                if (ds_stud.Tables[0].Rows.Count > 0)
                {




                    double excessamtValue = 0;
                    //  double.TryParse(excessamtQ, out excessamtValue);
                    double fineAmount = 0;
                    for (int i = 0; i < ds_stud.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr_Student = tbl_Student.NewRow();

                        //dr_Student["Roll_No"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Roll_No"]);
                        //dr_Student["Reg_No"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Reg_No"]);
                        // dr_Student["Stud_Name"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Stud_Name"]);
                        // dr_Student["Degree"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Degree"]);
                        dr_Student["TextVal"] = "0"; // Convert.ToString(ds_stud.Tables[0].Rows[i]["TextVal"]);
                        dr_Student["TextCode"] = "0"; // Convert.ToString(ds_stud.Tables[0].Rows[i]["TextCode"]);
                        dr_Student["HeaderPK"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderFK"]);
                        dr_Student["HeaderName"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderName"]);
                        dr_Student["LedgerPK"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerFK"]);
                        dr_Student["LedgerName"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerName"]);

                        //string finyearfk = Convert.ToString(ds_stud.Tables[0].Rows[i]["finyearfk"]);
                        //string finyear = Convert.ToString(ds_stud.Tables[0].Rows[i]["finyear"]);
                        //appnoNew = Convert.ToString(ds_stud.Tables[0].Rows[i]["App_No"]);


                        double feeamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["FeeAmount"]);
                        double deductamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["DeductAmount"]);
                        totamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["TotalAmount"]);
                        paid = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["PaidAmount"]);
                        double balamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["BalAmount"]);

                        //dr_Student["ToBePaid"] = "0";
                        dr_Student["Fee_Amount"] = feeamt;
                        dr_Student["Deduct"] = deductamt;
                        dr_Student["Total"] = balamt;
                        dr_Student["PaidAmt"] = paid;
                        //dr_Student["BalAmt"] = balamt;
                        //// dr_Student["finyearfk"] = "&";
                        //dr_Student["finyearfk"] = finyearfk;
                        //dr_Student["finyear"] = finyear;
                        tbl_Student.Rows.Add(dr_Student);





                        #region Fine Calculation
                        if (balamt > 0)
                        {
                            string fineQ = "select FineMasterPK, FineType, FromDay, ToDay, isnull(FineAmount,0) as FineAmt, DueDate, F.HeaderFk, Ledgerfk, Feecatgory, Degreecode, F.collegecode, LedgerName, HeaderName,( select (convert(varchar(10),finyearstart,103)+'-'+convert(varchar(10),finyearend,103)) as finyearfk from fm_finyearmaster fm where f.finyearfk=fm.finyearpk)as finyear,f.finyearfk,Stud_FineSettingType from Fm_FInemaster F,FM_LedgerMaster L,FM_HeaderMaster H where f.CollegeCode=L.CollegeCode and F.CollegeCode=h.CollegeCode and f.HeaderFK=h.HeaderPK and f.HeaderFK=l.HeaderFK and f.LedgerFK=l.LedgerPK and Duedate<GETDATE() and F.CollegeCode=" + collegecode + " and DegreeCode='" + degcode + "' and H.Headerpk ='" + Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderFK"]) + "' and L.LedgerPK='" + Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerFK"]) + "'";// and FeeCatgory in ('" + Convert.ToString(ds_stud.Tables[0].Rows[i]["TextCode"]) + "'

                            DataSet dsFine = new DataSet();
                            dsFine = d2.select_method_wo_parameter(fineQ, "Text");
                            if (dsFine.Tables.Count > 0)
                            {
                                if (dsFine.Tables[0].Rows.Count > 0)
                                {
                                    for (int fn = 0; fn < dsFine.Tables[0].Rows.Count; fn++)
                                    {
                                        string fineType = Convert.ToString(dsFine.Tables[0].Rows[fn]["FineType"]);
                                        DateTime due = Convert.ToDateTime(dsFine.Tables[0].Rows[fn]["DueDate"]);
                                        DateTime curDate = DateTime.Now;
                                        if (fineType == "1")
                                        {
                                            fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                        }
                                        else if (fineType == "2")
                                        {
                                            for (; due <= curDate; due = due.AddDays(1))
                                            {
                                                fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                            }
                                        }
                                        else if (fineType == "3")
                                        {
                                            TimeSpan td = curDate - due;
                                            int difference = td.Days;
                                            int fromday = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["FromDay"]);
                                            int to_day = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["ToDay"]);

                                            if (difference <= to_day && difference >= fromday)
                                            {
                                                fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #region Fine Adjustment
                    Dictionary<string, double> dtfintFeecat = new Dictionary<string, double>();
                    Dictionary<string, string> dtfeecat = new Dictionary<string, string>();
                    Dictionary<string, string> dtfinfk = new Dictionary<string, string>();
                    string fineLegHedQ = d2.GetFunction(" select Linkvalue from New_InsSettings where LinkName='FineLedgerValue' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                    if (fineLegHedQ != "0" && fineAmount > 0 && dtfintFeecat.Count > 0)
                    {
                        string fineHdrId = fineLegHedQ.Split(',')[0];
                        string fineLgrId = fineLegHedQ.Split(',')[1];
                        string fineHdrName = d2.GetFunction(" select headername from fm_headermaster where headerpk=" + fineHdrId + " and CollegeCode=" + collegecode + "");
                        string fineLgrName = d2.GetFunction("  select ledgername from fm_ledgermaster where ledgerpk=" + fineLgrId + " and HeaderFK=" + fineHdrId + " and CollegeCode=" + collegecode + "");
                        fineAmount = 0;
                        foreach (KeyValuePair<string, double> fine in dtfintFeecat)
                        {
                            string sbfine = string.Empty;
                            string feestr = string.Empty;
                            string finfks = string.Empty;
                            sbfine = Convert.ToString(fine.Key + "$" + fine.Value);
                            fineAmount = fine.Value;
                            if (dtfeecat.ContainsKey(fine.Key))
                                feestr = Convert.ToString(dtfeecat[fine.Key]);
                            if (dtfinfk.ContainsKey(fine.Key))
                                finfks = Convert.ToString(dtfinfk[fine.Key]);
                            string finyears = d2.GetFunction("select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend))) as finyearfk from fm_finyearmaster fm where finyearpk='" + finfks + "'");
                            DataRow drFine = tbl_Student.NewRow();
                            drFine["HeaderPK"] = fineHdrId;
                            drFine["HeaderName"] = fineHdrName;
                            drFine["LedgerPK"] = fineLgrId;
                            drFine["LedgerName"] = fineLgrName;
                            //drFine["ChlTaken"] = "0";
                            //drFine["TextVal"] = "FINE";
                            //drFine["TextCode"] = "-1";
                            //drFine["Fee_Amount"] = fineAmount;
                            //drFine["Deduct"] = "0";
                            drFine["Total"] = fineAmount;
                            //drFine["PaidAmt"] = "0";
                            //drFine["BalAmt"] = fineAmount;
                            //drFine["ToBePaid"] = "0";
                            //drFine["finyear"] = finyears;
                            tbl_Student.Rows.Add(drFine);
                        }
                    #endregion
                    }


                    if (tbl_Student.Rows.Count > 0)
                    {
                        //  txt_exfees.Text = d2.GetFunction("select distinct isnull(BalanceAmt,0) from FT_ExcessDet where App_No='" + appnoNew + "'");
                        //txt_chltakn.Text = d2.GetFunction(chlnTakenQ);

                        grid_HeaderLedger.DataSource = tbl_Student;
                        grid_HeaderLedger.DataBind();
                        grid_HeaderLedger.Visible = true;
                        btn_ledgersave.Visible = true;
                        //if (totamt == paid)
                        //{
                        //    lbl_alert.Text = "Cannot Edit";
                        //}

                    }
                    else
                    {
                        grid_HeaderLedger.DataSource = null;
                        grid_HeaderLedger.DataBind();
                        grid_HeaderLedger.Visible = false;
                        imgAlert.Visible = true;
                        //this.Form.DefaultButton = "btn_alertclose";
                        lbl_alert.Text = "Please Add Fees";
                        btn_ledgersave.Visible = false;
                        //img_stud.ImageUrl = "";
                        //img_stud.Visible = false;

                    }
                }
                else
                {
                    grid_HeaderLedger.DataSource = null;
                    grid_HeaderLedger.DataBind();
                    grid_HeaderLedger.Visible = false;
                    imgAlert.Visible = true;
                    //this.Form.DefaultButton = "btn_alertclose";
                    lbl_alert.Text = "Please Add Fees";
                    btn_ledgersave.Visible = false;
                    //img_stud.ImageUrl = "";
                    //img_stud.Visible = false;

                }
                Session["appNo"] = appnoNew;

            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "ChallanReceipt");
            grid_HeaderLedger.DataSource = null;
            grid_HeaderLedger.DataBind();
            grid_HeaderLedger.Visible = false;
            imgAlert.Visible = true;
            //this.Form.DefaultButton = "btn_alertclose";
            lbl_alert.Text = "No Records Found";
            btn_ledgersave.Visible = false;
            //img_stud.ImageUrl = "";
            //img_stud.Visible = false;
            //btn_print.Visible = false;
            Session["appNo"] = "";
        }



    }
    protected void rbl_rollnoNew_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbl_rollnoNew.SelectedIndex == 0)
        {
            staff.Visible = true;
            vendor.Visible = false;
            other.Visible = false;
        }
        if (rbl_rollnoNew.SelectedIndex == 1)
        {
            staff.Visible = false;
            vendor.Visible = true;
            other.Visible = false;
        }
        if (rbl_rollnoNew.SelectedIndex == 2)
        {
            staff.Visible = false;
            vendor.Visible = false;
            other.Visible = true;

        }
    }
    protected void txtroll_vendor_Changed(object sender, EventArgs e)
    {
        if (txtroll_vendor.Text.Trim() != "")
        {
            // string staffid = Convert.ToString(txtname_staff.Text);

            // if (staffid != "")
            // {
            //     try
            //     {
            //         staffid = staffid.Split('-')[1];
            //     }
            //     catch { staffid = ""; }
            // }
            //// txtroll_staff.Text = staffid;
            txtname_vendor_Changed(sender, e);
            imgAlert.Visible = false;
        }

    }

    protected void txtname_vendor_Changed(object sender, EventArgs e)
    {

        if (txtroll_vendor.Text.Trim() == "")
        {
            txtname_vendor.Text = "";
        }
        string staffId = Convert.ToString(txtname_vendor.Text.Trim());
        try
        {
            staffId = staffId.Split('-')[2];
        }
        catch { staffId = ""; }


        if (staffId != "")
        {
            string name = string.Empty;
            string degree = string.Empty;

            string query = " SELECT VendorContactPK, VenContactType, VenContactName, VenContactDesig, VenContactDept, VendorPhoneNo, VendorExtNo, VendorMobileNo, VendorEmail,VendorFK FROM IM_VendorContactMaster WHERE    VendorContactPK = '" + staffId + "' ";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");


            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        name = Convert.ToString(ds.Tables[0].Rows[i]["VenContactName"]);
                        degree = Convert.ToString(ds.Tables[0].Rows[i]["VenContactDesig"]);
                        //lbltype.Text = Convert.ToString(ds.Tables[0].Rows[i]["type"]);                            
                    }
                }
            }

            // txtname_staff.Text = name;
            txtDept_vendor.Text = degree;

            //img_stud.ImageUrl = "~/Handler/ghStaffFoto.ashx?QSstaff_id=" + staffId;
            //img_stud.Visible = true;



        }


    }
    public void loadGridVendor()
    {
        try
        {
            string finYearFk = string.Empty;
            string curfinYearid = d2.getCurrentFinanceYear(usercode, collegecode);
            //if (chklsfyear.Items.Count > 0)
            //    finYearFk = Convert.ToString(getCblSelectedValue(chklsfyear));


            string roll_no = string.Empty;
            string semyear = string.Empty;
            string appnoNew = string.Empty;
            string degcode = string.Empty;
            roll_no = txtname_vendor.Text.Trim();

            try
            {
                roll_no = roll_no.Split('-')[2];
            }
            catch { roll_no = "-1"; }

            DataTable tbl_Student = new DataTable();
            tbl_Student.Columns.Add("Roll_No");
            tbl_Student.Columns.Add("Reg_No");
            tbl_Student.Columns.Add("Stud_Name");
            tbl_Student.Columns.Add("Degree");
            tbl_Student.Columns.Add("TextVal");
            tbl_Student.Columns.Add("TextCode");
            tbl_Student.Columns.Add("HeaderPK");
            tbl_Student.Columns.Add("HeaderName");
            tbl_Student.Columns.Add("LedgerPK");
            tbl_Student.Columns.Add("LedgerName");
            tbl_Student.Columns.Add("Fee_Amount");
            tbl_Student.Columns.Add("Deduct");
            tbl_Student.Columns.Add("Total");
            tbl_Student.Columns.Add("PaidAmt");
            //tbl_Student.Columns.Add("BalAmt");
            //tbl_Student.Columns.Add("ToBePaid");
            //tbl_Student.Columns.Add("Monthly");
            //tbl_Student.Columns.Add("ChlTaken");
            //tbl_Student.Columns.Add("Scholar");
            //tbl_Student.Columns.Add("CautionDep");
            //tbl_Student.Columns.Add("MonwiseMon");
            //tbl_Student.Columns.Add("MonwiseYear");
            //tbl_Student.Columns.Add("FeeallotPk");
            //tbl_Student.Columns.Add("finyearfk");
            //tbl_Student.Columns.Add("finyear");
            string selectQuery = "";

            string queryRollApp = " SELECT VendorContactPK, VenContactType, VenContactName, VenContactDesig, VenContactDept, VendorPhoneNo, VendorExtNo, VendorMobileNo, VendorEmail,VendorFK FROM IM_VendorContactMaster WHERE    VendorContactPK = '" + roll_no + "' ";

            DataSet dsRollApp = new DataSet();
            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
            if (dsRollApp.Tables.Count > 0)
            {
                if (dsRollApp.Tables[0].Rows.Count > 0)
                {

                    roll_no = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorContactPK"]);
                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorContactPK"]);
                    degcode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VenContactDept"]);


                    //img_stud.ImageUrl = "~/Handler/ghStaffFoto.ashx?QSstaff_id=" + roll_no;
                    // img_stud.Visible = true;
                }

            }




            selectQuery = " SELECT A.HeaderFK,HeaderName,A.LedgerFK,LedgerName,Priority,isnull(FeeAmount,0) as FeeAmount,isnull(DeductAmout,0)  as DeductAmount,isnull(TotalAmount,0) as TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,isnull(PaidAmount,0) as PaidAmount,isnull(TotalAmount,0)-isnull(PaidAmount,0)  as BalAmount,( select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend))) as finyearfk from fm_finyearmaster fm where a.finyearfk=fm.finyearpk )as finyear,a.finyearfk FROM FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK  and l.LedgerMode=0 and MemType=3  AND A.App_No = " + appnoNew + " and isnull(istransfer,'0')='0' and BalAmount != 0";// and T.TextCode in('" + semyear + "')
            string headercode = getCblSelectedValue(cbl_HeaderPop);
            string ledgercode = getCblSelectedValue(cbl_ledgerpop);
            //Header
            selectQuery += "  and A.HeaderFK in ('" + headercode + "') ";

            //Ledger
            selectQuery += "  and A.LedgerFK  in ('" + ledgercode + "')  ";

            DataSet ds_stud = new DataSet();
            ds_stud.Clear();
            ds_stud = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds_stud.Tables.Count > 0)
            {
                if (ds_stud.Tables[0].Rows.Count > 0)
                {


                    double fineAmount = 0;
                    for (int i = 0; i < ds_stud.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr_Student = tbl_Student.NewRow();

                        //dr_Student["Roll_No"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Roll_No"]);
                        //dr_Student["Reg_No"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Reg_No"]);
                        // dr_Student["Stud_Name"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Stud_Name"]);
                        // dr_Student["Degree"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Degree"]);
                        dr_Student["TextVal"] = "0";// Convert.ToString(ds_stud.Tables[0].Rows[i]["TextVal"]);
                        dr_Student["TextCode"] = "0";// Convert.ToString(ds_stud.Tables[0].Rows[i]["TextCode"]);
                        dr_Student["HeaderPK"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderFK"]);
                        dr_Student["HeaderName"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderName"]);
                        dr_Student["LedgerPK"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerFK"]);
                        dr_Student["LedgerName"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerName"]);
                        //dr_Student["ChlTaken"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["ChlTakAmt"]);
                        //string finyearfk = Convert.ToString(ds_stud.Tables[0].Rows[i]["finyearfk"]);
                        //string finyear = Convert.ToString(ds_stud.Tables[0].Rows[i]["finyear"]);
                        //appnoNew = Convert.ToString(ds_stud.Tables[0].Rows[i]["App_No"]);

                        double feeamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["FeeAmount"]);
                        double deductamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["DeductAmount"]);
                        double totamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["TotalAmount"]);
                        double paid = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["PaidAmount"]);
                        double balamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["BalAmount"]);
                        double curExcess = 0;



                        dr_Student["Fee_Amount"] = feeamt;
                        dr_Student["Deduct"] = deductamt;
                        dr_Student["Total"] = totamt;
                        dr_Student["PaidAmt"] = paid;
                        dr_Student["BalAmt"] = balamt;
                        //dr_Student["finyearfk"] = "&";

                        tbl_Student.Rows.Add(dr_Student);

                        #region Fine Calculation
                        if (balamt > 0)
                        {
                            string fineQ = "select FineMasterPK, FineType, FromDay, ToDay, isnull(FineAmount,0) as FineAmt, DueDate, F.HeaderFk, Ledgerfk, Feecatgory, Degreecode, F.collegecode, LedgerName, HeaderName,( select (convert(varchar(10),finyearstart,103)+'-'+convert(varchar(10),finyearend,103)) as finyearfk from fm_finyearmaster fm where f.finyearfk=fm.finyearpk)as finyear,f.finyearfk,Stud_FineSettingType from Fm_FInemaster F,FM_LedgerMaster L,FM_HeaderMaster H where f.CollegeCode=L.CollegeCode and F.CollegeCode=h.CollegeCode and f.HeaderFK=h.HeaderPK and f.HeaderFK=l.HeaderFK and f.LedgerFK=l.LedgerPK and Duedate<GETDATE() and F.CollegeCode=" + collegecode + " and DegreeCode='" + degcode + "' and H.Headerpk ='" + Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderFK"]) + "' and L.LedgerPK='" + Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerFK"]) + "' ";//and FeeCatgory in ('" + Convert.ToString(ds_stud.Tables[0].Rows[i]["TextCode"]) + "'

                            DataSet dsFine = new DataSet();
                            dsFine = d2.select_method_wo_parameter(fineQ, "Text");
                            if (dsFine.Tables.Count > 0)
                            {
                                if (dsFine.Tables[0].Rows.Count > 0)
                                {
                                    for (int fn = 0; fn < dsFine.Tables[0].Rows.Count; fn++)
                                    {
                                        string fineType = Convert.ToString(dsFine.Tables[0].Rows[fn]["FineType"]);
                                        DateTime due = Convert.ToDateTime(dsFine.Tables[0].Rows[fn]["DueDate"]);
                                        DateTime curDate = DateTime.Now;
                                        if (fineType == "1")
                                        {
                                            fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                        }
                                        else if (fineType == "2")
                                        {
                                            for (; due <= curDate; due = due.AddDays(1))
                                            {
                                                fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                            }
                                        }
                                        else if (fineType == "3")
                                        {
                                            TimeSpan td = curDate - due;
                                            int difference = td.Days;
                                            int fromday = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["FromDay"]);
                                            int to_day = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["ToDay"]);

                                            if (difference <= to_day && difference >= fromday)
                                            {
                                                fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #region Fine Adjustment
                    Dictionary<string, double> dtfintFeecat = new Dictionary<string, double>();
                    Dictionary<string, string> dtfeecat = new Dictionary<string, string>();
                    Dictionary<string, string> dtfinfk = new Dictionary<string, string>();
                    string fineLegHedQ = d2.GetFunction(" select Linkvalue from New_InsSettings where LinkName='FineLedgerValue' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                    if (fineLegHedQ != "0" && fineAmount > 0 && dtfintFeecat.Count > 0)
                    {
                        string fineHdrId = fineLegHedQ.Split(',')[0];
                        string fineLgrId = fineLegHedQ.Split(',')[1];
                        string fineHdrName = d2.GetFunction(" select headername from fm_headermaster where headerpk=" + fineHdrId + " and CollegeCode=" + collegecode + "");
                        string fineLgrName = d2.GetFunction("  select ledgername from fm_ledgermaster where ledgerpk=" + fineLgrId + " and HeaderFK=" + fineHdrId + " and CollegeCode=" + collegecode + "");
                        foreach (KeyValuePair<string, double> fine in dtfintFeecat)
                        {
                            string sbfine = string.Empty;
                            string feestr = string.Empty;
                            string finfks = string.Empty;
                            sbfine = Convert.ToString(fine.Key + "$" + fine.Value);
                            fineAmount = fine.Value;
                            if (dtfeecat.ContainsKey(fine.Key))
                                feestr = Convert.ToString(dtfeecat[fine.Key]);
                            if (dtfinfk.ContainsKey(fine.Key))
                                finfks = Convert.ToString(dtfinfk[fine.Key]);
                            string finyears = d2.GetFunction("select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend))) as finyearfk from fm_finyearmaster fm where finyearpk='" + finfks + "'");
                            DataRow drFine = tbl_Student.NewRow();
                            drFine["HeaderPK"] = fineHdrId;
                            drFine["HeaderName"] = fineHdrName;
                            drFine["LedgerPK"] = fineLgrId;
                            drFine["LedgerName"] = fineLgrName;
                            //drFine["ChlTaken"] = "0";
                            drFine["TextVal"] = "FINE";
                            drFine["TextCode"] = "-1";
                            drFine["Fee_Amount"] = fineAmount;
                            //drFine["Deduct"] = "0";
                            //drFine["Total"] = fineAmount;
                            //drFine["PaidAmt"] = "0";
                            //drFine["BalAmt"] = fineAmount;
                            //drFine["ToBePaid"] = "0";
                            tbl_Student.Rows.Add(drFine);
                        }
                    #endregion
                    }
                }

                if (tbl_Student.Rows.Count > 0)
                {
                    //  txt_exfees.Text = d2.GetFunction("select distinct isnull(BalanceAmt,0) from FT_ExcessDet where App_No='" + appnoNew + "'");
                    //txt_chltakn.Text = d2.GetFunction(chlnTakenQ);

                    grid_HeaderLedger.DataSource = tbl_Student;
                    grid_HeaderLedger.DataBind();
                    grid_HeaderLedger.Visible = true;
                    btn_ledgersave.Visible = true;


                }
                else
                {
                    grid_HeaderLedger.DataSource = null;
                    grid_HeaderLedger.DataBind();
                    grid_HeaderLedger.Visible = false;
                    imgAlert.Visible = true;
                    //this.Form.DefaultButton = "btn_alertclose";
                    lbl_alert.Text = "Please Add Fees";
                    btn_ledgersave.Visible = false;
                    //img_stud.ImageUrl = "";
                    //img_stud.Visible = false;

                }
            }
            else
            {
                grid_HeaderLedger.DataSource = null;
                grid_HeaderLedger.DataBind();
                grid_HeaderLedger.Visible = false;
                imgAlert.Visible = true;
                //this.Form.DefaultButton = "btn_alertclose";
                lbl_alert.Text = "Please Add Fees";
                btn_ledgersave.Visible = false;
                //img_stud.ImageUrl = "";
                //img_stud.Visible = false;

            }
            Session["appNo"] = appnoNew;

        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Staff/Vendor/other Fee Edit");
            grid_HeaderLedger.DataSource = null;
            grid_HeaderLedger.DataBind();
            grid_HeaderLedger.Visible = false;
            imgAlert.Visible = true;
            //this.Form.DefaultButton = "btn_alertclose";
            lbl_alert.Text = "No Records Found";
            btn_ledgersave.Visible = false;
            //img_stud.ImageUrl = "";
            //img_stud.Visible = false;

            Session["appNo"] = "";
        }


    }
    protected void txtroll_other_Changed(object sender, EventArgs e)
    {

        //try
        //{
        txt_otherMobile.Text = "";//    txt_otherMobile.Text = txtroll_other.Text.Split('-')[1].Trim();
        //    txtroll_other.Text = txtroll_other.Text.Split('-')[0].Trim();
        //}
        //catch { }

        string staffId = Convert.ToString(txtroll_other.Text.Trim());
        string staffMob = Convert.ToString(txt_otherMobile.Text.Trim());


        if (staffId != "")//&& staffMob != ""
        {
            string ifAlreadyExist = d2.GetFunction("select VendorCode from co_vendormaster where vendorname='" + staffId + "' and VendorMobileNo='" + staffMob + "'  and VendorType=-5").Trim();

            string name = string.Empty;
            string compname = string.Empty;
            string Add1 = string.Empty;
            string Add2 = string.Empty;
            string mobiNo = string.Empty;
            if (ifAlreadyExist == "1")
            {
                string query = " select VendorName,VendorMobileNo,VendorCode,VendorAddress+'-'+VendorStreet as Add1,VendorCity,VendorCompName from co_vendormaster where vendorname='" + staffId + "' and VendorMobileNo='" + staffMob + "'  and VendorType=-5";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    name = Convert.ToString(ds.Tables[0].Rows[0]["VendorName"]);
                    compname = Convert.ToString(ds.Tables[0].Rows[0]["VendorCompName"]);
                    Add1 = Convert.ToString(ds.Tables[0].Rows[0]["Add1"]);
                    Add2 = Convert.ToString(ds.Tables[0].Rows[0]["VendorCity"]);
                    mobiNo = Convert.ToString(ds.Tables[0].Rows[0]["VendorMobileNo"]);
                }

                txtname_other.Text = compname;

                txt_otherMobile.Text = mobiNo;
            }



        }

    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetVendorno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            //staff query
            query = " select VendorCompName+'-'+VendorCode+'-'+Convert(varchar(10),vendorpk)  from CO_VendorMaster where VendorType =1";

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetVendorName(string prefixText)
    {
        WebService ws = new WebService();
        string query = " select (VenContactName+'-'+VenContactDesig+'-'+ CONVERT(varchar(10), VendorContactPK)) as contactname from IM_VendorContactMaster where VendorFK ='" + vencontcode + "' ";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetVendorno1(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            //staff query
            query = " select VendorCode  from CO_VendorMaster where VendorType =1  order by VendorCode asc";

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetVendorName1(string prefixText)
    {
        WebService ws = new WebService();
        string query = " select VendorCompName  from CO_VendorMaster where VendorType =1 order by VendorCompName asc";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetOthername(string prefixText)
    {
        WebService ws = new WebService();
        //string query = " select vendorname+'-'+VendorMobileNo from co_vendormaster where vendorname like '" + prefixText + "%' and VendorType=-5";
        string query = " select vendorname from co_vendormaster where vendorname like '" + prefixText + "%' and VendorType=-5";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }
    public void loadGridOthers()
    {

        if (generateVendorCode().Trim() != "")
        {

            try
            {

                string roll_no = string.Empty;
                string semyear = string.Empty;
                string appnoNew = string.Empty;
                string degcode = string.Empty;
                roll_no = txtroll_other.Text.Trim();

                DataTable tbl_Student = new DataTable();
                tbl_Student.Columns.Add("Roll_No");
                tbl_Student.Columns.Add("Reg_No");
                tbl_Student.Columns.Add("Stud_Name");
                tbl_Student.Columns.Add("Degree");
                tbl_Student.Columns.Add("TextVal");
                tbl_Student.Columns.Add("TextCode");
                tbl_Student.Columns.Add("HeaderPK");
                tbl_Student.Columns.Add("HeaderName");
                tbl_Student.Columns.Add("LedgerPK");
                tbl_Student.Columns.Add("LedgerName");
                tbl_Student.Columns.Add("Fee_Amount");
                tbl_Student.Columns.Add("Deduct");
                tbl_Student.Columns.Add("Total");
                tbl_Student.Columns.Add("PaidAmt");
                //tbl_Student.Columns.Add("BalAmt");
                //tbl_Student.Columns.Add("ToBePaid");
                //tbl_Student.Columns.Add("Monthly");
                //tbl_Student.Columns.Add("ChlTaken");
                //tbl_Student.Columns.Add("Scholar");
                //tbl_Student.Columns.Add("CautionDep");
                //tbl_Student.Columns.Add("MonwiseMon");
                //tbl_Student.Columns.Add("MonwiseYear");
                //tbl_Student.Columns.Add("FeeallotPk");
                //tbl_Student.Columns.Add("finyearfk");
                //tbl_Student.Columns.Add("finyear");
                string selectQuery = "";

                string name = string.Empty;
                string compname = string.Empty;
                string Add1 = string.Empty;
                string Add2 = string.Empty;
                string mobiNo = string.Empty;

                string staffId = Convert.ToString(txtroll_other.Text.Trim());
                string staffMob = Convert.ToString(txt_otherMobile.Text.Trim());
                string queryRollApp = "  select VendorName, VendorMobileNo, VendorCode,VendorAddress ,VendorCity,VendorCompName,VendorPK from co_vendormaster where vendorname='" + staffId + "' and VendorMobileNo='" + staffMob + "'  and VendorType=-5 ";

                DataSet dsRollApp = new DataSet();
                dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                if (dsRollApp.Tables.Count > 0)
                {
                    if (dsRollApp.Tables[0].Rows.Count > 0)
                    {

                        roll_no = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorName"]);
                        appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorPK"]);
                        // degcode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["dept_code"]);

                        name = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorName"]);
                        compname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorCompName"]);
                        Add1 = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorAddress"]);
                        Add2 = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorCity"]);
                        mobiNo = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorMobileNo"]);

                    }
                    else
                    {
                        roll_no = "";
                        appnoNew = "";

                    }
                }
                else
                {
                    roll_no = "";
                    appnoNew = "";

                }
                txtname_other.Text = compname;

                //txt_otherMobile.Text = mobiNo;

                if (appnoNew.Trim() != "")
                {
                    selectQuery = " SELECT A.HeaderFK,HeaderName,A.LedgerFK,LedgerName,Priority,isnull(FeeAmount,0) as FeeAmount,isnull(DeductAmout,0)  as DeductAmount,isnull(TotalAmount,0) as TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,isnull(PaidAmount,0) as PaidAmount,isnull(TotalAmount,0)-isnull(PaidAmount,0)  as BalAmount,( select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend))) as finyearfk from fm_finyearmaster fm where a.finyearfk=fm.finyearpk )as finyear,a.finyearfk  FROM FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK and l.LedgerMode=0 and MemType=4  AND A.App_No = " + appnoNew + " and isnull(istransfer,'0')='0' and  BalAmount != 0";//and T.TextCode in('" + semyear + "')
                    string headercode = getCblSelectedValue(cbl_HeaderPop);
                    string ledgercode = getCblSelectedValue(cbl_ledgerpop);
                    //Header
                    selectQuery += "  and A.HeaderFK in ('" + headercode + "') ";

                    //Ledger
                    selectQuery += "  and A.LedgerFK  in ('" + ledgercode + "')  ";


                    DataSet ds_stud = new DataSet();
                    ds_stud.Clear();
                    ds_stud = d2.select_method_wo_parameter(selectQuery, "Text");
                    if (ds_stud.Tables.Count > 0)
                    {
                        if (ds_stud.Tables[0].Rows.Count > 0)
                        {

                            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
                            string excessamtQ = d2.GetFunction("select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt from FT_ExcessDet WHERE  App_No=" + appnoNew + " ");
                            string excessTypeQ = "select LinkValue from New_InsSettings where LinkName='ExcessFeesType' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
                            int excessTypeValue = 0;
                            try { excessTypeValue = Convert.ToInt32(Convert.ToString(d2.GetFunction(excessTypeQ))); }
                            catch { }


                            double excessamtValue = 0;
                            double.TryParse(excessamtQ, out excessamtValue);
                            double fineAmount = 0;
                            for (int i = 0; i < ds_stud.Tables[0].Rows.Count; i++)
                            {
                                DataRow dr_Student = tbl_Student.NewRow();

                                //dr_Student["Roll_No"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Roll_No"]);
                                //dr_Student["Reg_No"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Reg_No"]);
                                // dr_Student["Stud_Name"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Stud_Name"]);
                                // dr_Student["Degree"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["Degree"]);
                                dr_Student["TextVal"] = "0";// Convert.ToString(ds_stud.Tables[0].Rows[i]["TextVal"]);
                                dr_Student["TextCode"] = "0";//Convert.ToString(ds_stud.Tables[0].Rows[i]["TextCode"]);
                                dr_Student["HeaderPK"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderFK"]);
                                dr_Student["HeaderName"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderName"]);
                                dr_Student["LedgerPK"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerFK"]);
                                dr_Student["LedgerName"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerName"]);
                                // dr_Student["ChlTaken"] = Convert.ToString(ds_stud.Tables[0].Rows[i]["ChlTakAmt"]);

                                //appnoNew = Convert.ToString(ds_stud.Tables[0].Rows[i]["App_No"]);
                                string finyearfk = Convert.ToString(ds_stud.Tables[0].Rows[i]["finyearfk"]);
                                string finyear = Convert.ToString(ds_stud.Tables[0].Rows[i]["finyear"]);
                                double feeamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["FeeAmount"]);
                                double deductamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["DeductAmount"]);
                                double totamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["TotalAmount"]);
                                double paid = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["PaidAmount"]);
                                double balamt = Convert.ToDouble(ds_stud.Tables[0].Rows[i]["BalAmount"]);
                                double curExcess = 0;


                                dr_Student["Fee_Amount"] = feeamt;
                                dr_Student["Deduct"] = deductamt;
                                dr_Student["Total"] = totamt;
                                dr_Student["PaidAmt"] = paid;
                                //dr_Student["BalAmt"] = balamt;
                                //   dr_Student["finyearfk"] = "&";
                                //dr_Student["finyearfk"] = finyearfk;
                                //dr_Student["finyear"] = finyear;
                                tbl_Student.Rows.Add(dr_Student);

                                #region Fine Calculation
                                if (balamt > 0)
                                {
                                    string fineQ = "select FineMasterPK, FineType, FromDay, ToDay, isnull(FineAmount,0) as FineAmt, DueDate, F.HeaderFk, Ledgerfk, Feecatgory, Degreecode, F.collegecode, LedgerName, HeaderName,( select (convert(varchar(10),finyearstart,103)+'-'+convert(varchar(10),finyearend,103)) as finyearfk from fm_finyearmaster fm where f.finyearfk=fm.finyearpk)as finyear,f.finyearfk,Stud_FineSettingType from Fm_FInemaster F,FM_LedgerMaster L,FM_HeaderMaster H where f.CollegeCode=L.CollegeCode and F.CollegeCode=h.CollegeCode and f.HeaderFK=h.HeaderPK and f.HeaderFK=l.HeaderFK and f.LedgerFK=l.LedgerPK and Duedate<GETDATE() and F.CollegeCode=" + collegecode + " and DegreeCode='" + degcode + "' and H.Headerpk ='" + Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderFK"]) + "' and L.LedgerPK='" + Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerFK"]) + "'  and f.finyearfk in('" + finyearfk + "')";// and FeeCatgory in ('" + Convert.ToString(ds_stud.Tables[0].Rows[i]["TextCode"]) + "'

                                    DataSet dsFine = new DataSet();
                                    dsFine = d2.select_method_wo_parameter(fineQ, "Text");
                                    if (dsFine.Tables.Count > 0)
                                    {
                                        if (dsFine.Tables[0].Rows.Count > 0)
                                        {
                                            for (int fn = 0; fn < dsFine.Tables[0].Rows.Count; fn++)
                                            {
                                                string fineType = Convert.ToString(dsFine.Tables[0].Rows[fn]["FineType"]);
                                                DateTime due = Convert.ToDateTime(dsFine.Tables[0].Rows[fn]["DueDate"]);
                                                DateTime curDate = DateTime.Now;
                                                if (fineType == "1")
                                                {
                                                    fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                }
                                                else if (fineType == "2")
                                                {
                                                    for (; due <= curDate; due = due.AddDays(1))
                                                    {
                                                        fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                    }
                                                }
                                                else if (fineType == "3")
                                                {
                                                    TimeSpan td = curDate - due;
                                                    int difference = td.Days;
                                                    int fromday = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["FromDay"]);
                                                    int to_day = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["ToDay"]);

                                                    if (difference <= to_day && difference >= fromday)
                                                    {
                                                        fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            #region Fine Adjustment
                            Dictionary<string, double> dtfintFeecat = new Dictionary<string, double>();
                            Dictionary<string, string> dtfeecat = new Dictionary<string, string>();
                            Dictionary<string, string> dtfinfk = new Dictionary<string, string>();
                            string fineLegHedQ = d2.GetFunction(" select Linkvalue from New_InsSettings where LinkName='FineLedgerValue' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                            if (fineLegHedQ != "0" && fineAmount > 0 && dtfintFeecat.Count > 0)
                            {
                                string fineHdrId = fineLegHedQ.Split(',')[0];
                                string fineLgrId = fineLegHedQ.Split(',')[1];
                                string fineHdrName = d2.GetFunction(" select headername from fm_headermaster where headerpk=" + fineHdrId + " and CollegeCode=" + collegecode + "");
                                string fineLgrName = d2.GetFunction("  select ledgername from fm_ledgermaster where ledgerpk=" + fineLgrId + " and HeaderFK=" + fineHdrId + " and CollegeCode=" + collegecode + "");
                                foreach (KeyValuePair<string, double> fine in dtfintFeecat)
                                {
                                    string sbfine = string.Empty;
                                    string feestr = string.Empty;
                                    string finfks = string.Empty;
                                    sbfine = Convert.ToString(fine.Key + "$" + fine.Value);
                                    fineAmount = fine.Value;
                                    if (dtfeecat.ContainsKey(fine.Key))
                                        feestr = Convert.ToString(dtfeecat[fine.Key]);
                                    if (dtfinfk.ContainsKey(fine.Key))
                                        finfks = Convert.ToString(dtfinfk[fine.Key]);
                                    string finyears = d2.GetFunction("select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend))) as finyearfk from fm_finyearmaster fm where finyearpk='" + finfks + "'");
                                    DataRow drFine = tbl_Student.NewRow();
                                    drFine["HeaderPK"] = fineHdrId;
                                    drFine["HeaderName"] = fineHdrName;
                                    drFine["LedgerPK"] = fineLgrId;
                                    drFine["LedgerName"] = fineLgrName;



                                    drFine["TextVal"] = "FINE";
                                    drFine["TextCode"] = "-1";
                                    drFine["Fee_Amount"] = fineAmount;
                                    drFine["Deduct"] = "0";
                                    drFine["Total"] = fineAmount;
                                    //drFine["PaidAmt"] = "0";
                                    //drFine["BalAmt"] = fineAmount;
                                    //drFine["ToBePaid"] = "0";
                                    //drFine["finyear"] = finyears;
                                    tbl_Student.Rows.Add(drFine);
                                }
                            #endregion
                            }
                        }

                        if (tbl_Student.Rows.Count > 0)
                        {
                            //  txt_exfees.Text = d2.GetFunction("select distinct isnull(BalanceAmt,0) from FT_ExcessDet where App_No='" + appnoNew + "'");


                            grid_HeaderLedger.DataSource = tbl_Student;
                            grid_HeaderLedger.DataBind();
                            grid_HeaderLedger.Visible = true;
                            btn_ledgersave.Visible = true;


                        }
                        else
                        {
                            grid_HeaderLedger.DataSource = null;
                            grid_HeaderLedger.DataBind();
                            grid_HeaderLedger.Visible = false;
                            imgAlert.Visible = true;
                            //this.Form.DefaultButton = "btn_alertclose";
                            lbl_alert.Text = "Please Add Fees";
                            btn_ledgersave.Visible = false;
                            //img_stud.ImageUrl = "";
                            //img_stud.Visible = false;

                        }
                    }
                    else
                    {
                        grid_HeaderLedger.DataSource = null;
                        grid_HeaderLedger.DataBind();
                        grid_HeaderLedger.Visible = false;
                        imgAlert.Visible = true;
                        //this.Form.DefaultButton = "btn_alertclose";
                        lbl_alert.Text = "Please Add Fees";
                        btn_ledgersave.Visible = false;
                        //img_stud.ImageUrl = "";
                        //img_stud.Visible = false;

                    }
                    Session["appNo"] = appnoNew;
                }
                else
                {
                    grid_HeaderLedger.DataSource = null;
                    grid_HeaderLedger.DataBind();
                    grid_HeaderLedger.Visible = false;
                    imgAlert.Visible = true;
                    //this.Form.DefaultButton = "btn_alertclose";
                    lbl_alert.Text = "Please Add Fees";
                    btn_ledgersave.Visible = false;
                    //img_stud.ImageUrl = "";
                    //img_stud.Visible = false;
                    //btn_print.Visible = false;
                }

            }
            catch (Exception ex)
            {
                d2.sendErrorMail(ex, collegecode, "stafffeeedit");
                grid_HeaderLedger.DataSource = null;
                grid_HeaderLedger.DataBind();
                grid_HeaderLedger.Visible = false;
                imgAlert.Visible = true;
                //this.Form.DefaultButton = "btn_alertclose";
                lbl_alert.Text = "No Records Found";
                btn_ledgersave.Visible = false;
                //img_stud.ImageUrl = "";
                //img_stud.Visible = false;
                //btn_print.Visible = false;
                Session["appNo"] = "";
            }

        }
        else
        {
            imgAlert.Visible = true;
            //this.Form.DefaultButton = "btn_alertclose";
            lbl_alert.Text = "Please Set Inventory Code Settings";
        }
    }
    public string generateVendorCode()
    {
        string newitemcode = string.Empty;
        try
        {
            string selectquery = "select VenAcr,VenStNo,VenSize  from IM_CodeSettings  order by startdate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["VenAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["VenStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["VenSize"]);
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "") // Added by jairam
                {
                    selectquery = " select distinct top (1) VendorCode,vendorPK  from CO_VendorMaster where VendorCode like '" + Convert.ToString(itemacronym) + "%' order by vendorPK desc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["VendorCode"]);
                        string itemacr = Convert.ToString(itemacronym);
                        int len = itemacr.Length;
                        itemcode = itemcode.Remove(0, len);
                        int len1 = Convert.ToString(itemcode).Length;
                        string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                        len = Convert.ToString(newnumber).Length;
                        len1 = len1 - len;
                        if (len1 == 2)
                        {
                            newitemcode = "00" + newnumber;
                        }
                        else if (len1 == 1)
                        {
                            newitemcode = "0" + newnumber;
                        }
                        else if (len1 == 3)
                        {
                            newitemcode = "000" + newnumber;
                        }
                        else if (len1 == 4)
                        {
                            newitemcode = "0000" + newnumber;
                        }
                        else if (len1 == 5)
                        {
                            newitemcode = "00000" + newnumber;
                        }
                        else if (len1 == 6)
                        {
                            newitemcode = "000000" + newnumber;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(newnumber);
                        }
                        if (newitemcode.Trim() != "")
                        {
                            newitemcode = itemacr + "" + newitemcode;
                        }
                    }
                    else
                    {
                        string itemacr = Convert.ToString(itemstarno);
                        int len = itemacr.Length;
                        string items = Convert.ToString(itemsize);
                        int len1 = Convert.ToInt32(items);
                        int size = len1 - len;
                        if (size == 2)
                        {
                            newitemcode = "00" + itemstarno;
                        }
                        else if (size == 1)
                        {
                            newitemcode = "0" + itemstarno;
                        }
                        else if (size == 3)
                        {
                            newitemcode = "000" + itemstarno;
                        }
                        else if (size == 4)
                        {
                            newitemcode = "0000" + itemstarno;
                        }
                        else if (size == 5)
                        {
                            newitemcode = "00000" + itemstarno;
                        }
                        else if (size == 6)
                        {
                            newitemcode = "000000" + itemstarno;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(itemstarno);
                        }
                        newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                    }
                }
            }
        }
        catch (Exception ex) { newitemcode = string.Empty; }
        return newitemcode;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }
    protected void clear()
    {


        txtroll_staff.Text = string.Empty;
        txtname_staff.Text = string.Empty;
        txtDept_staff.Text = string.Empty;

        txtroll_vendor.Text = string.Empty;
        txtname_vendor.Text = string.Empty;
        txtDept_vendor.Text = string.Empty;

        txtroll_other.Text = string.Empty;
        txtname_other.Text = string.Empty;

    }
    protected void btn_exitstaff_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = false;
    }
    protected void btn_staffLook_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = true;
        ddlsearch1_OnSelectedIndexChanged(sender, e);
        btn_staffOK.Visible = false;
        btn_exitstaff.Visible = false;
        Fpspread2.Visible = false;
        lbl_errormsgstaff.Visible = false;
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
            //Label1.Text = "Search By Name";
        }
        else
        {
            txtsearch1c.Visible = true;
            //Label1.Text = "Search By Code";
        }
    }
    protected void btn_go2Staff_Click(object sender, EventArgs e)
    {
        try
        {
            string clgcode = string.Empty;
            clgcode = getClgCode();
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
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + clgcode + "') and staff_name like '" + Convert.ToString(sname) + "%'";
                }
                else if (txtsearch1c.Text.Trim() != "")
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + clgcode + "') and staff_code='" + Convert.ToString(txtsearch1c.Text) + "'";
                }
                else
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + clgcode + "') order by PrintPriority";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread2.Sheets[0].RowCount = 0;
                    Fpspread2.Sheets[0].ColumnCount = 0;
                    Fpspread2.CommandBar.Visible = false;
                    Fpspread2.Sheets[0].AutoPostBack = false;
                    Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread2.Sheets[0].RowHeader.Visible = false;
                    Fpspread2.Sheets[0].ColumnCount = 3;
                    Fpspread2.Sheets[0].Columns[0].Width = 60;
                    Fpspread2.Sheets[0].Columns[1].Width = 170;
                    Fpspread2.Sheets[0].Columns[2].Width = 360;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    FarPoint.Web.Spread.TextCellType chkall = new FarPoint.Web.Spread.TextCellType();


                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = chkall;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]);

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["appl_id"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    }
                    Fpspread2.Visible = true;
                    // div2.Visible = true;
                    lbl_errormsgstaff.Visible = false;
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Width = 620;
                    Fpspread2.Height = 210;
                    if (Fpspread2.Sheets[0].RowCount > 0)
                    {
                        btn_staffOK.Visible = true;
                        btn_exitstaff.Visible = true;
                    }
                    else
                    {
                        btn_staffOK.Visible = false;
                        btn_exitstaff.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //  d2.sendErrorMail(ex, collegecode, "staffedit"); 
        }
    }
    protected void Fpspread2staff_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread2.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread2.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread2.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "staffedit"); }
    }
    protected void btn_staffOK_Click(object sender, EventArgs e)
    {
        try
        {

            string actrow = "";
            string actcol = "";
            actrow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
            actcol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
            if (actrow.Trim() != "" && actrow.Trim() != "-1")
            {
                string staff = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                string appno = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
                string staffcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
                txtroll_staff.Text = staffcode;
                txtroll_staff_Changed(sender, e);

            }
            div_staffLook.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "staffedit"); }
    }

    protected void btn_exitvendor_Click(object sender, EventArgs e)
    {
        div_vendorLook.Visible = false;
    }
    protected void ddlsearch2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtsearch2.Text = "";
        txtsearch2c.Text = "";
        txtsearch2c.Visible = false;
        txtsearch2.Visible = false;
        if (ddlsearch2.SelectedIndex == 0)
        {
            txtsearch2.Visible = true;
            Label2.Text = "Search By Name";
        }
        else
        {
            txtsearch2c.Visible = true;
            Label2.Text = "Search By Code";
        }
    }
    protected void btn_goVendor_Click(object sender, EventArgs e)
    {
        try
        {
            btnvendor_ok.Visible = false;
            btnExit_vendor.Visible = false;
            div_vendorLook.Visible = true;
            Fpspread3.Visible = false;
            if (collegecode != null)
            {
                string selq = "";
                if (txtsearch2.Text.Trim() != "")
                {
                    string sname = string.Empty;
                    try
                    {
                        sname = txtsearch2.Text.Trim().Split('-')[0];
                    }
                    catch { sname = txtsearch2.Text.Trim(); }
                    selq = "select VendorCompName,VendorCode ,VendorPK  from CO_VendorMaster where VendorType =1 and VendorCompName like '" + Convert.ToString(sname) + "%'";
                }
                else if (txtsearch2c.Text.Trim() != "")
                {
                    selq = "select VendorCompName,VendorCode ,VendorPK  from CO_VendorMaster where VendorType =1 and Vendorcode= '" + Convert.ToString(txtsearch2c.Text) + "'";
                }
                else
                {
                    selq = "select VendorCompName,VendorCode ,VendorPK  from CO_VendorMaster where VendorType =1 ";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread3.Sheets[0].RowCount = 0;
                        Fpspread3.Sheets[0].ColumnCount = 0;
                        Fpspread3.CommandBar.Visible = false;
                        Fpspread3.Sheets[0].AutoPostBack = false;
                        Fpspread3.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread3.Sheets[0].RowHeader.Visible = false;
                        Fpspread3.Sheets[0].ColumnCount = 3;
                        Fpspread3.Sheets[0].Columns[0].Width = 60;
                        Fpspread3.Sheets[0].Columns[1].Width = 170;
                        Fpspread3.Sheets[0].Columns[2].Width = 360;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vendor Code";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Name";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        FarPoint.Web.Spread.TextCellType chkall = new FarPoint.Web.Spread.TextCellType();


                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspread3.Sheets[0].RowCount++;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].CellType = chkall;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Vendorcode"]);

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["VendorPK"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["VendorCompName"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        }
                        Fpspread3.Visible = true;
                        // div2.Visible = true;
                        lbl_errormsgvendor.Visible = false;
                        Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                        Fpspread3.Width = 620;
                        Fpspread3.Height = 210;
                        if (Fpspread3.Sheets[0].RowCount > 0)
                        {
                            btnvendor_ok.Visible = true;
                            btnExit_vendor.Visible = true;
                        }

                    }
                    else
                    {
                        imgAlert.Visible = true;
                        //this.Form.DefaultButton = "btn_alertclose";
                        lbl_alert.Text = "No Records Found";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    //this.Form.DefaultButton = "btn_alertclose";
                    lbl_alert.Text = "No Records Found";
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "staffedit"); 
        }
    }

    protected void btn_vendorLook_Click(object sender, EventArgs e)
    {
        div_vendorLook.Visible = true;
        ddlsearch2_OnSelectedIndexChanged(sender, e);
        btnvendor_ok.Visible = false;
        btnExit_vendor.Visible = false;
        Fpspread3.Visible = false;
        lbl_errormsgvendor.Visible = false;
    }
    protected void btnvendor_ok_Click(object sender, EventArgs e)
    {
        try
        {

            string actrow = "";
            string actcol = "";
            actrow = Fpspread3.ActiveSheetView.ActiveRow.ToString();
            actcol = Fpspread3.ActiveSheetView.ActiveColumn.ToString();
            if (actrow.Trim() != "" && actrow.Trim() != "-1")
            {
                string vendor = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                string appno = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
                string vendorcode = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
                txtroll_vendor.Text = vendor + "-" + vendorcode + "-" + appno;
                txtroll_vendor_Changed(sender, e);

            }
            div_vendorLook.Visible = false;
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "ChallanReceipt"); 
        }
    }

    protected void Fpspread3vendor_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread3.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread3.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread3.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread3.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread3.Sheets[0].RowCount; i++)
                        {
                            Fpspread3.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread3.Sheets[0].RowCount; i++)
                        {
                            Fpspread3.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "staffedit"); }
    }
}