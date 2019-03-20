using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Drawing;
using System.Collections;
using System.Text;



//---------------Last Modified By Jeyaprakash on Jan 6th,2017--------------//
//---------------PDF Image Issues----------------------------//
public partial class Individual_SalaryReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static string collegecode = string.Empty;
    string collegecode1 = string.Empty;
    string usercode = string.Empty;
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static ArrayList colord = new ArrayList();
    ArrayList alleavetype = new ArrayList();
    Hashtable hatpre = new Hashtable();
    Hashtable splallow = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable ColumnWidth = new Hashtable();
    Hashtable ColumnAdjWid = new Hashtable();
    static Hashtable getcol = new Hashtable();
    static string[] spll_alll_tag_arry = new string[15];
    static string[] splallw_arry = new string[15];
    static string[] allow_arry = new string[15];
    static string[] ded_array = new string[50];
    int colheder;
    int colgross;
    int col;
    string sql;
    string sql1 = "";
    string strdept = "";
    string strcategory = "";
    int rowstr;
    int ccountforallow = 0;
    double IntMTotal;
    double IntMTemp;
    string adv_amount = "";
    double month_amount;
    double collect_amount;
    int total_amount = 0;
    bool Cellclick = false;
    int smssentcount = 0;
    DataSet dssmssalary = new DataSet();
    SortedDictionary<string, string> deduct = new SortedDictionary<string, string>();
    string alloworder = "";
    string tenure = ""; //poo 28.10.17
    string loandet = "";
    string selpaidcount = "";
    string isstaff = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            //collegecode = Convert.ToString(Session["collegecode"]);
            usercode = Convert.ToString(Session["usercode"]);
            isstaff = d2.GetFunction("select is_staff from usermaster where User_code='" + usercode + "'");
            ViewState["isstaffcheck"] = Convert.ToString(isstaff);
            lblsmserror.Text = "";
            lblnorec.Text = "";
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                binddept();
                binddesignation();
                loadstafftype();
                loadcategory();
                bindpaymode();
                bindyear();
                bindreason();
                bindpurpose();
                loadClgBank();
                //loadallowance();
                //loaddeduction();
                //loadleave();
                rprint.Visible = false;
                chkShowPF.Visible = false;
                tborder.Visible = false;
                tborder.Text = "";
                lstcolorder.Items.Clear();
                loadcolumns(sender, e);
                btnpayslip.Visible = false;
                chkIncLopAmnt.Visible = false;
                btnsalcer.Visible = false;
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                FpSpread2.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                lblsmstype.Visible = false;
                ddlsmstype.Visible = false;
                lblnorec.Visible = false;
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            lblnorec.Visible = false;
        }
        catch (Exception ex) { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_code";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_name";
        name = ws.Getname(query);
        return name;
    }
    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e) { }
    }
    protected void ddlcollege_Change(object sender, EventArgs e)
    {
        collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        binddept();
        binddesignation();
        loadstafftype();
        loadcategory();
        bindpaymode();
        ddl_mon.SelectedIndex = 0;
        bindyear();
        bindreason();
        bindpurpose();
        loadClgBank();
        //loadallowance();
        //loaddeduction();
        //loadleave();
        cb_all.Checked = false;
        cb_deduct.Checked = false;
        cbleavecat.Checked = false;
        cbl_allow.Items.Clear();
        cbl_ded.Items.Clear();
        chklstlev.Items.Clear();
        chk_amnt.Checked = false;
        chk_loandet.Checked = false;
        chksms.Checked = false;
        chkmail.Checked = false;
        ddladdreason.SelectedIndex = 0;
        txt_allow.Text = "--Select--";
        txt_ded.Text = "--Select--";
        txtleavecat.Text = "--Select--";
        txtstaffcode.Text = "";
        txtstaffname.Text = "";
        txt_allow.Enabled = false;
        txt_ded.Enabled = false;
        txtleavecat.Enabled = false;
        rprint.Visible = false;
        chkShowPF.Visible = false;
        tborder.Visible = false;
        tborder.Text = "";
        lstcolorder.Items.Clear();
        loadcolumns(sender, e);
        btnpayslip.Visible = false;
        chkIncLopAmnt.Visible = false;
        btnsalcer.Visible = false;
        lblpurpose1.Visible = false;
        ddlpurpose.Visible = false;
        FpSpread2.Visible = false;
        btnaddtemplate.Visible = false;
        btndeletetemplate.Visible = false;
        lblsmstype.Visible = false;
        ddlsmstype.Visible = false;
        fpsalary.Visible = false;
        lblnorec.Visible = false;
    }
    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(fpsalary, reportname);
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Visible = true;
                lblsmserror.Text = "Please Enter Your Report Name!";
                txtexcel.Focus();
            }
        }
        catch { }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        degreedetails = "Overall Monthly Salary Report@month : " + ddl_mon.SelectedItem.ToString() + " and college_code = '" + collegecode1 + "'";
        //    string year = d2.GetFunction("select CONVERT(CHAR(4), to_date, 120) from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "' and college_Code=" + collegecode1 + "");//delsi0806
        string year = d2.GetFunction("select CONVERT(CHAR(4), to_date, 120) from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "' and  PayYear='" + ddl_year.SelectedItem.Value.ToString() + "' and college_Code=" + collegecode1 + "");

        if (year.Trim() != "" && year != null && year.Trim() != "0")
        {
            degreedetails = "Overall Monthly Salary Report@ Year / Month : " + year + " / " + ddl_mon.SelectedItem.ToString();
        }
        else
        {
            degreedetails = "Overall Monthly Salary Report@ Month : " + ddl_mon.SelectedItem.ToString() + "";
        }
        Printcontrol.loadspreaddetails(fpsalary, "Individual_SalaryReport.aspx", degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnPrintNew_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        degreedetails = "Overall Monthly Salary Report@month : " + ddl_mon.SelectedItem.ToString() + " and college_code = '" + collegecode1 + "'";
        //    string year = d2.GetFunction("select CONVERT(CHAR(4), to_date, 120) from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "' and college_Code=" + collegecode1 + "");//delsi0806
        string year = d2.GetFunction("select CONVERT(CHAR(4), to_date, 120) from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "' and  PayYear='" + ddl_year.SelectedItem.Value.ToString() + "' and college_Code=" + collegecode1 + "");

        if (year.Trim() != "" && year != null && year.Trim() != "0")
        {
            degreedetails = "Overall Monthly Salary Report@ Year / Month : " + year + " / " + ddl_mon.SelectedItem.ToString();
        }
        else
        {
            degreedetails = "Overall Monthly Salary Report@ Month : " + ddl_mon.SelectedItem.ToString() + "";
        }
        GridPrintmaster.loadspreaddetails(fpsalary, "Individual_SalaryReport.aspx", degreedetails);
        GridPrintmaster.Visible = true;
    }


    protected void cb_allow_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_allow, cbl_allow, txt_allow, "Allowance");
    }
    protected void cbl_allow_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_allow, cbl_allow, txt_allow, "Allowance");
    }
    protected void cb_ded_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_ded, cbl_ded, txt_ded, "Deduction");
    }
    protected void cbl_ded_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_ded, cbl_ded, txt_ded, "Deduction");
    }
    protected void cb_dept_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    protected void cbl_dept_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    protected void cb_desig_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cbl_desig_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cb_staffcat_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_staffcat, cbl_staffcat, txt_staffcat, "Category");
    }
    protected void cbl_staffcat_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_staffcat, cbl_staffcat, txt_staffcat, "Category");
    }
    protected void cb_stafftyp_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_stafftyp, cbl_stafftyp, txt_stafftyp, "StaffType");
    }
    protected void cbl_stafftyp_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_stafftyp, cbl_stafftyp, txt_stafftyp, "StaffType");
    }
    protected void chklev_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(chklev, chklstlev, txtleavecat, "Leave Category");
    }
    protected void chklstlev_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(chklev, chklstlev, txtleavecat, "Leave Category");
    }
    protected void cb_paymode_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_paymode, cbl_paymode, txtpaymode, "Pay Mode");
    }
    protected void cbl_paymode_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_paymode, cbl_paymode, txtpaymode, "Pay Mode");
    }
    protected void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = (cb_partimestaff.Checked == true ? cblcolumnorder.Items.Count : cblcolumnorder.Items.Count - 3);
            if (CheckBox_column.Checked == true)
            {
                lstcolorder.Items.Clear();
                tborder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
                    lstcolorder.Items.Add(new ListItem(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value));
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                }
                tborder.Text = colname12;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    lstcolorder.Items.Clear();
                }
                tborder.Text = "";
                tborder.Visible = false;
            }
            tborder.Visible = true;
        }
        catch (Exception ex) { }
    }
    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        cblcolumnorder.ClearSelection();
        CheckBox_column.Checked = false;
        lnk_columnorder.Visible = false;
        ItemList.Clear();
        Itemindex.Clear();
        lstcolorder.Items.Clear();
        tborder.Text = "";
        tborder.Visible = true;
    }
    protected void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                    lstcolorder.Items.Add(new ListItem(cblcolumnorder.Items[index].Text, cblcolumnorder.Items[index].Value));
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
                lstcolorder.Items.Remove(new ListItem(cblcolumnorder.Items[index].Text, cblcolumnorder.Items[index].Value));
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[38].Selected == true)
                {
                    cblcolumnorder.Items[51].Enabled = true;
                }
                else
                {
                    //cblcolumnorder.Items[51].Enabled = false;
                }
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
            }
            tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
            tborder.Visible = true;
        }
        catch (Exception ex) { }
    }
    protected void cb_all_checkedchanged(object sender, EventArgs e)
    {
        if (cb_all.Checked == true)
        {
            txt_allow.Enabled = true;
            loadallowance();
        }
        if (cb_all.Checked == false)
        {
            txt_allow.Enabled = false;
            cbl_allow.Items.Clear();
            txt_allow.Text = "--Select--";
        }
    }
    protected void cb_deduct_checkedchanged(object sender, EventArgs e)
    {
        if (cb_deduct.Checked == true)
        {
            txt_ded.Enabled = true;
            loaddeduction();
        }
        if (cb_deduct.Checked == false)
        {
            txt_ded.Enabled = false;
            cbl_ded.Items.Clear();
            txt_ded.Text = "--Select--";
        }
    }
    protected void cbleavecat_checkedchanged(object sender, EventArgs e)
    {
        if (cbleavecat.Checked == true)
        {
            txtleavecat.Enabled = true;
            loadleave();
        }
        if (cbleavecat.Checked == false)
        {
            txtleavecat.Enabled = false;
            chklstlev.Items.Clear();
            txtleavecat.Text = "--Select--";
        }
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
    }
    //protected void fpsalary_render(object sender, EventArgs e)
    //{
    //}
    protected void fpsalary_ButtonCommand(object sender, EventArgs e)
    {
        try
        {
            string activerow = fpsalary.ActiveSheetView.ActiveRow.ToString();
            string activecol = fpsalary.ActiveSheetView.ActiveColumn.ToString();
            if (activerow == "0" && fpsalary.Sheets[0].ColumnHeader.Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text == "SELECT")
            {
                int isval = 0;
                isval = Convert.ToInt32(fpsalary.Sheets[0].Cells[0, Convert.ToInt32(activecol)].Value);
                int val = 0;
                if (isval == 1)
                {
                    val = 1;
                }
                if (fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL" && cblcolumnorder.Items[53].Selected == true)
                {
                    for (int i = 1; i < fpsalary.Sheets[0].RowCount - 2; i++)
                    {
                        fpsalary.Sheets[0].Cells[i, Convert.ToInt32(activecol)].Value = val;
                    }
                }
                else
                {
                    for (int i = 1; i < fpsalary.Sheets[0].RowCount - 1; i++)
                    {
                        fpsalary.Sheets[0].Cells[i, Convert.ToInt32(activecol)].Value = val;
                    }
                }
            }
        }
        catch { }
    }
    protected void btnpayslip_click(object sender, EventArgs e)
    {
        loadpayslip();
    }
    public void loadpayslip()
    {
        try
        {
            lblnorec.Visible = false;
            Printcontrol.Visible = false;
            lblsmserror.Visible = false;
            fpsalary.SaveChanges();
            string payscale = "";// added by Jeyaprakash 12 Mar 2016
            string collname = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string pincode = "";
            string address = "";
            Font Fontbold = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 20, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 13, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfTable tableforfooter;
            string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + collegecode1 + "'";
            str = str + " ;select * from incentives_master where college_code='" + collegecode1 + "'";
            str = str + " ;select distinct CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "'and PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and college_Code=" + collegecode1 + "";
            DataSet ds = d2.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                collname = ds.Tables[0].Rows[0]["collname"].ToString();
                address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                if (address1.Trim() != "" && address1 != null)
                {
                    address = address1;
                }
                if (address2.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address2;
                    }
                    else
                    {
                        address = address2;
                    }
                }
                if (address3.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address3;
                    }
                    else
                    {
                        address = address3;
                    }
                }
                if (pincode.Trim() != "" && pincode != null)
                {
                    if (address != "")
                    {
                        address = address + '-' + pincode;
                    }
                    else
                    {
                        address = pincode;
                    }
                }
            }
            string allowmaster = "";
            string deductmaster = "";
            if (ds.Tables[1].Rows.Count > 0)
            {
                allowmaster = ds.Tables[1].Rows[0]["allowances"].ToString();
                deductmaster = ds.Tables[1].Rows[0]["deductions"].ToString();
            }
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] spdet = deductmaster.Split(';');
            for (int d = 0; d <= spdet.GetUpperBound(0); d++)
            {
                string[] spdedet = spdet[d].Split('\\');
                if (spdedet.GetUpperBound(0) >= 1)
                {
                    string val = spdedet[0];
                    string val1 = spdedet[1];
                    if (!dict.ContainsKey(val))
                    {
                        dict.Add(val, val1);
                    }
                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                string date3 = Convert.ToString(ds.Tables[2].Rows[0]["from_date"]);
                string datefrom3;
                int monthname1;
                string monyear;
                string monthname2 = "";
                int monthnamenum;
                string yearto = "";
                string monyearto = "";
                string[] split = date3.Split(new Char[] { '/' });
                int totlastaff = 0;
                int left1 = 20;
                int left1a = 135;
                int left2 = 145;
                int left3 = 423;
                int left4 = 570;
                int incrre = 1;
                int rowcount = 0;
                if (cblcolumnorder.Items[53].Selected == true && fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL")
                {
                    rowcount = fpsalary.Sheets[0].RowCount - 2;
                }
                if (fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Text == "TOTAL" && cblcolumnorder.Items[53].Selected == false)
                {
                    rowcount = fpsalary.Sheets[0].RowCount - 1;
                }
                for (int res = incrre; res < rowcount; res = res + incrre)
                {
                    for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                    {
                        if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT" && fpsalary.Sheets[0].Rows[res].Visible == true)
                        {
                            string text = fpsalary.Sheets[0].Cells[res, lst].Text.ToString();
                            if (text != "" && text != null)
                            {
                                int isval = Convert.ToInt32(fpsalary.Sheets[0].Cells[res, lst].Value);
                                if (isval == 1)
                                {
                                    totlastaff++;
                                }
                            }
                        }
                    }
                }
                datefrom3 = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                string date4 = Convert.ToString(ds.Tables[2].Rows[0]["to_date"]);
                int year3 = Convert.ToInt16(split[2].ToString());
                string[] split1 = date4.Split(new Char[] { '/' });
                string dateto4 = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                monthname2 = split1[0].ToString();
                monthnamenum = Convert.ToInt32(monthname2.ToString());
                yearto = split1[2].ToString();
                string mnmae = split[1].ToString();
                monthname1 = Convert.ToInt16(mnmae);
                monthname2 = split1[1].ToString();
                monthnamenum = Convert.ToInt16(monthname2);
                string year = split[2].ToString();
                monyear = monthname1.ToString() + "/" + year;
                monyearto = monthnamenum.ToString() + "/" + yearto;
                Boolean getvalflag = false;
                int dayfrm = 0;
                string dayto;
                int daytonum;
                string lopdates = "";
                string[] leavetype = new string[50];
                sql = "select shortname from leave_category where college_code=" + Session["collegecode"] + "";
                DataSet dsleave = d2.select_method_wo_parameter(sql, "Text");
                int lev = 0;
                for (int le = 0; le < dsleave.Tables[0].Rows.Count; le++)
                {
                    lev++;
                    string levatype = dsleave.Tables[0].Rows[le]["shortname"].ToString().Trim().ToLower();
                    leavetype[lev] = levatype;
                }
                sql = "select m.*,s.staff_name,s.pfnumber,s.ESI_No,s.bankaccount,s.pangirnumber ,IsManualLOP,st.payscale as pay_scalenew , st.allowances as actallowance,h.dept_name as deptname,d.desig_name as designame from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.latestrec = 1 and m.college_code=s.college_code and m.college_code =h.college_code and m.college_code=d.collegeCode and s.college_code='" + collegecode1 + "' and m.PayYear='" + ddl_year.SelectedValue.ToString() + "' and m.PayMonth='" + ddl_mon.SelectedValue.ToString() + "'";
                sql = sql + " ; select convert(nvarchar(15),sa.date_of_birth,103) as dob,sm.staff_code,convert(nvarchar(15),sm.retr_date ,103) as retir,sm.staff_name,CONVERT(varchar(10),sm.join_date,103) as join_date from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no ";
                DataSet dspay = d2.select_method_wo_parameter(sql, "text");
                DataTable dtpay = dspay.Tables[0];
                DataView dvpay = new DataView();
                DataView dvapp = new DataView();
                int gettop = 0;
                int rec = 0;
                Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
                string getlink = d2.GetFunction("select linkvalue from inssettings where linkname='Calculate LOP from Attendance' and college_code='" + collegecode1 + "'");
                sql1 = "select * from staff_attnd where mon_year between '" + monyear + "' and '" + monyearto + "' ";
                DataSet dsstaffattendance = d2.select_method_wo_parameter(sql1, "text");
                DataView dvstaffattendance = new DataView();
                int genpdfcount = 0;
                //****************Added By Jeyaprakash 10.04.2016************************//
                #region for format1
                for (int res = 1; res < rowcount; res++)
                {
                    if (fpsalary.Sheets[0].Rows[res].Visible == true)
                    {
                        for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                        {
                            if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT")
                            {
                                string text = fpsalary.Sheets[0].Cells[res, lst].Text.ToString();
                                if (text != "" && text != null)
                                {
                                    int isval = Convert.ToInt32(fpsalary.Sheets[0].Cells[res, lst].Value);
                                    if (isval == 1)
                                    {
                                        rec++;
                                        getvalflag = true;
                                        if (rec % 2 != 0)
                                        {
                                            if (rec != 1)
                                            {
                                                mypdfpage = mydocument.NewPage();
                                            }
                                            gettop = 15;
                                        }
                                        else
                                        {
                                            if (gettop < 629)
                                            {
                                                //  gettop = 550;
                                                gettop = 560;
                                            }
                                            else
                                            {
                                                mypdfpage = mydocument.NewPage();
                                                gettop = 15;
                                            }
                                        }
                                        string staffcode = "";
                                        string designation = "";
                                        string deptname = "";
                                        for (int chklst = 0; chklst < lstcolorder.Items.Count; chklst++)
                                        {
                                            if (lstcolorder.Items[chklst].Text == "STAFF CODE")
                                            {
                                                staffcode = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DESIGN")
                                            {
                                                designation = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DEPT")
                                            {
                                                deptname = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "LOP DATES")
                                            {
                                                lopdates = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                        }
                                        string staffname = "";
                                        string bankno = "";
                                        string pfno = "";
                                        string esino = "";
                                        string panno = "";
                                        string Allowances = "";
                                        string lopamount = "";
                                        if (staffcode.Trim() == "")
                                        {
                                            lblnorec.Visible = true;
                                            lblnorec.Text = "Please select Staff Code!";
                                            return;
                                        }
                                        Double totdection = 0;
                                        dtpay.DefaultView.RowFilter = " staff_code='" + staffcode + "'";
                                        dvpay = dtpay.DefaultView;
                                        if (dvpay.Count > 0)
                                        {
                                            staffname = dvpay[0]["staff_name"].ToString();
                                            bankno = dvpay[0]["bankaccount"].ToString();
                                            pfno = dvpay[0]["pfnumber"].ToString();
                                            esino = dvpay[0]["ESI_No"].ToString();
                                            payscale = dvpay[0]["pay_scalenew"].ToString();// added by Jeyaprakash 12 Mar 2016
                                            panno = dvpay[0]["pangirnumber"].ToString();
                                            Allowances = dvpay[0]["allowances"].ToString();
                                            lopamount = dvpay[0]["Tot_lop"].ToString();
                                            designation = dvpay[0]["designame"].ToString();
                                            deptname = dvpay[0]["deptname"].ToString();
                                            if (pfno.Trim().ToLower() == "" || pfno == null || pfno.Trim() == "0")
                                            {
                                                pfno = "-";
                                            }
                                            string setyear = dvpay[0]["PayYear"].ToString();
                                            if (setyear.Trim() == "" || setyear == null)
                                            {
                                                setyear = yearto;
                                            }
                                            string noofpresent = "";
                                            string presnt = "";
                                            string workdays = "";
                                            string leavedays = "";
                                            string nooflop = "";
                                            noofpresent = dvpay[0]["leavedetail"].ToString();
                                            string[] presplit = noofpresent.Split(';');
                                            if (presplit.Length >= 7)
                                            {
                                                presnt = presplit[1].ToString();
                                                workdays = presplit[0].ToString();
                                                leavedays = presplit[2].ToString();
                                                nooflop = presplit[6].ToString();
                                            }
                                            int starttop = gettop;
                                            int coltop = gettop;
                                            int rowspace = 20;
                                            try
                                            {
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                                {
                                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                                    mypdfpage.Add(LogoImage, 25, coltop + 10, 400);
                                                }
                                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                                {
                                                    string leftlogo = "Left_Logo(" + collegecode1 + ")";
                                                    MemoryStream memoryStream = new MemoryStream();
                                                    string sellogo = "select logo1 from collinfo where college_code='" + collegecode1 + "'";
                                                    ds.Clear();
                                                    ds = d2.select_method_wo_parameter(sellogo, "Text");
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                                        memoryStream.Write(file, 0, file.Length);
                                                        if (file.Length > 0)
                                                        {
                                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"));
                                                            mypdfpage.Add(LogoImage, 25, coltop + 10, 400);
                                                        }
                                                        memoryStream.Dispose();
                                                        memoryStream.Close();
                                                    }
                                                }
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                                {
                                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                                    mypdfpage.Add(LogoImage, 750, coltop + 10, 350);
                                                }
                                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                                {
                                                    string rightlogo = "Right_Logo(" + collegecode1 + ")";
                                                    MemoryStream memoryStream = new MemoryStream();
                                                    string sellogo = "select logo2 from collinfo where college_code='" + collegecode1 + "'";
                                                    ds.Clear();
                                                    ds = d2.select_method_wo_parameter(sellogo, "Text");
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                                        memoryStream.Write(file, 0, file.Length);
                                                        if (file.Length > 0)
                                                        {
                                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"));
                                                            mypdfpage.Add(LogoImage, 750, coltop + 10, 350);
                                                        }
                                                        memoryStream.Dispose();
                                                        memoryStream.Close();
                                                    }
                                                }
                                            }
                                            catch { }
                                            PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 0, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                            mypdfpage.Add(ptc);
                                            coltop = coltop + rowspace;
                                            PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop + 10, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                            mypdfpage.Add(pts);
                                            coltop = coltop + rowspace;
                                            PdfTextArea ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop + 10, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, "Pay Slip for the month of " + ddl_mon.SelectedItem.Text + " " + "  " + setyear + "");
                                            mypdfpage.Add(ptsp);
                                            string stl = "";
                                            //Double lop = Convert.ToDouble(dvpay[0]["lop"].ToString());
                                            Double lop = 0;
                                            Double.TryParse(dvpay[0]["Tot_lop"].ToString(), out lop);
                                            double lop11 = lop;
                                            double tot_lop = 0;
                                            Double.TryParse(nooflop, out tot_lop);
                                            int maxdays = getmaxdays(monthname1, year3);
                                            double[] clleave = new double[50];
                                            dayfrm = Convert.ToInt32(split[0]);
                                            dayto = split1[0].ToString();
                                            daytonum = Convert.ToInt32(dayto);
                                            double abs = 0;
                                            double abscount = 0;
                                            dsstaffattendance.Tables[0].DefaultView.RowFilter = " mon_year ='" + monyear + "'  and staff_code='" + staffcode + "'";
                                            dvstaffattendance = dsstaffattendance.Tables[0].DefaultView;
                                            if (monthname1 == monthnamenum)
                                            {
                                                for (int day = dayfrm; dayfrm <= daytonum; dayfrm++)
                                                {
                                                    int day5 = 0;
                                                    day5 = 0 + dayfrm;
                                                    if (dsleave.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (dvstaffattendance.Count > 0)
                                                        {
                                                            int day6;
                                                            string attnda = "";
                                                            day6 = 3 + day5;
                                                            attnda = dvstaffattendance[0][day6].ToString();
                                                            if (attnda != "")
                                                            {
                                                                string[] split100 = attnda.Split(new char[] { '-' });
                                                                string mor = "";
                                                                string eve = "";
                                                                mor = split100[0].ToString();
                                                                eve = split100[1].ToString();
                                                                if (mor.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                if (eve.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                                {
                                                                    string levatype = leavetype[le];
                                                                    if (mor.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                    if (eve.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                }
                                                                string[] lave = new string[15];
                                                            }
                                                        }
                                                    }
                                                }
                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                {
                                                    if (leavetype[le] != "LA")
                                                    {
                                                        if (clleave[le] != 0)
                                                        {
                                                            stl = stl + leavetype[le].ToUpper() + "-" + clleave[le] + ", ";
                                                        }
                                                    }
                                                }
                                                if (stl != "")
                                                {
                                                    stl = stl.Substring(0, stl.Length - 2);
                                                }
                                                Array.Clear(clleave, 0, clleave.Length);
                                            }
                                            else
                                            {
                                                dsstaffattendance.Tables[0].DefaultView.RowFilter = " mon_year ='" + monyear + "'  and staff_code='" + staffcode + "'";
                                                dvstaffattendance = dsstaffattendance.Tables[0].DefaultView;
                                                for (int day = dayfrm; dayfrm <= maxdays; dayfrm++)
                                                {
                                                    int day5 = 0;
                                                    day5 = 0 + dayfrm;
                                                    if (dsleave.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (dvstaffattendance.Count > 0)
                                                        {
                                                            int day6;
                                                            string attnda = "";
                                                            day6 = 3 + day5;
                                                            attnda = dvstaffattendance[0][day6].ToString();
                                                            if (attnda != "")
                                                            {
                                                                string[] split100 = attnda.Split(new char[] { '-' });
                                                                string mor = "";
                                                                string eve = "";
                                                                mor = Convert.ToString(split100[0]);
                                                                eve = Convert.ToString(split100[1]);
                                                                if (mor.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                if (eve.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                                {
                                                                    string levatype = leavetype[le];
                                                                    if (mor.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                    if (eve.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                }
                                                                string[] lave = new string[15];
                                                            }
                                                        }
                                                    }
                                                }
                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                {
                                                    if (leavetype[le] != "LA")
                                                    {
                                                        if (clleave[le] != 0)
                                                        {
                                                            stl = stl + leavetype[le].ToUpper() + "-" + clleave[le] + ", ";
                                                        }
                                                    }
                                                }
                                                dsstaffattendance.Tables[0].DefaultView.RowFilter = " mon_year='" + monyearto + "' and staff_code='" + staffcode + "'";
                                                dvstaffattendance = dsstaffattendance.Tables[0].DefaultView;
                                                for (int day = 1; day <= daytonum; day++)
                                                {
                                                    int day5 = 0;
                                                    day5 = 0 + day;
                                                    if (dsleave.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (dvstaffattendance.Count > 0)
                                                        {
                                                            int day6;
                                                            string attnda = "";
                                                            day6 = 3 + day5;
                                                            attnda = dvstaffattendance[0][day6].ToString();
                                                            if (attnda != "")
                                                            {
                                                                string[] split100 = attnda.Split(new char[] { '-' });
                                                                string mor = "";
                                                                string eve = "";
                                                                mor = Convert.ToString(split100[0]);
                                                                eve = Convert.ToString(split100[1]);
                                                                if (mor.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                if (eve.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                                {
                                                                    string levatype = leavetype[le];
                                                                    if (mor.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                    if (eve.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                }
                                                                string[] lave = new string[15];
                                                            }
                                                        }
                                                    }
                                                }
                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                {
                                                    if (leavetype[le] != "LA")
                                                    {
                                                        if (clleave[le] != 0)
                                                        {
                                                            stl = stl + leavetype[le].ToUpper() + "-" + clleave[le] + ", ";
                                                        }
                                                    }
                                                }
                                                if (stl != "")
                                                {
                                                    stl = stl.Substring(0, stl.Length - 2);
                                                }
                                            }
                                            Array.Clear(clleave, 0, clleave.Length);
                                            double totalpreset = 0;
                                            if (workdays.Trim() != "" && nooflop.Trim() != "")
                                            {
                                                double.TryParse(presnt, out totalpreset);// Convert.ToDouble(workdays) - Convert.ToDouble(nooflop);23.12.17 barath
                                            }
                                            abscount = abs / 2;
                                            //totalpreset = totalpreset - abscount;
                                            coltop = coltop + 60;
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Staff Code");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, staffcode);
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Department                    :");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, deptname);
                                            mypdfpage.Add(ptsp);
                                            coltop = coltop + rowspace;
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "NAME");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, staffname);
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Designation                    :");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, designation);
                                            mypdfpage.Add(ptsp);
                                            if (cblcolumnorder.Items[21].Selected == true)
                                            {
                                                coltop = coltop + rowspace;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "PF A/C No");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, pfno);
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Bank A/C No                  :");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, bankno);
                                                mypdfpage.Add(ptsp);
                                            }
                                            if (cblcolumnorder.Items[22].Selected == true)
                                            {
                                                coltop = coltop + rowspace;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "ESI NO");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(esino));
                                                mypdfpage.Add(ptsp);
                                                if (cblcolumnorder.Items[21].Selected == false)
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Bank A/C No                  :");
                                                    mypdfpage.Add(ptsp);
                                                    ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, bankno);
                                                    mypdfpage.Add(ptsp);
                                                }
                                            }
                                            //else
                                            //{
                                            //    coltop = ((coltop-20)+ rowspace);
                                            //}
                                            if (cblcolumnorder.Items[21].Selected == false && cblcolumnorder.Items[22].Selected == false)
                                            {
                                                if (cblcolumnorder.Items[20].Selected == true)
                                                {
                                                    coltop = coltop + rowspace;
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Bank A/C No                  :");
                                                    mypdfpage.Add(ptsp);
                                                    ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, bankno);
                                                    mypdfpage.Add(ptsp);
                                                }
                                                else
                                                {
                                                    coltop = ((coltop - 20) + rowspace);
                                                }
                                            }
                                            string dob1 = "select convert(nvarchar(15),sa.date_of_birth,103) as dob from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sm.staff_code='" + staffcode + "'";
                                            DataSet sk1 = new DataSet();
                                            DAccess2 ddsd = new DAccess2();
                                            sk1 = ddsd.select_method_wo_parameter(dob1, "Text");
                                            string dob2 = sk1.Tables[0].Rows[0]["dob"].ToString();
                                            string[] group_semi5 = lopdates.Split(',');
                                            string doj8 = "";
                                            for (int i = 0; i <= group_semi5.GetUpperBound(0); i++)
                                            {
                                                string group_semi4 = group_semi5[i].ToString();
                                                string[] splitgroup_semi4 = group_semi4.Split('/');
                                                if (doj8 == "")
                                                {
                                                    doj8 = splitgroup_semi4[0].ToString();
                                                }
                                                else
                                                {
                                                    doj8 = doj8 + "," + splitgroup_semi4[0].ToString();
                                                }
                                            }
                                            if (cblcolumnorder.Items[30].Selected == true)
                                            {
                                                coltop = coltop + rowspace;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "LOP DATES");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                                mypdfpage.Add(ptsp);
                                                if (lopdates != "")
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, doj8);
                                                }
                                                else
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "-");
                                                }
                                            }
                                            mypdfpage.Add(ptsp);
                                            if (cblcolumnorder.Items[51].Selected == true)
                                            {
                                                coltop = coltop + 20;
                                                if (payscale != "")
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Pay Scale                      :");
                                                    mypdfpage.Add(ptsp);
                                                    ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, payscale);
                                                    mypdfpage.Add(ptsp);
                                                }
                                            }
                                            if (panno != "")
                                            {
                                                if (cblcolumnorder.Items[23].Selected == true)
                                                {
                                                    coltop = coltop + rowspace;
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "PAN No                          :");
                                                    mypdfpage.Add(ptsp);
                                                    ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, panno);
                                                    mypdfpage.Add(ptsp);
                                                }
                                                else
                                                {
                                                    coltop = ((coltop - 20) + rowspace);

                                                }
                                            }
                                            string joindate = "";
                                            string dofjoin = d2.GetFunction("select CONVERT(varchar(10),sm.join_date,103) as join_date from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sm.staff_code='" + staffcode + "'");
                                            if (dofjoin.Trim() != "" && dofjoin.Trim() != "0")
                                            {
                                                joindate = dofjoin;
                                            }
                                            else
                                            {
                                                joindate = "-";
                                            }
                                            coltop = coltop + rowspace;
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date of Join");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, joindate);
                                            mypdfpage.Add(ptsp);

                                            if (cblcolumnorder.Items[31].Selected == true)//added if condition
                                            {
                                                coltop = coltop + rowspace;
                                                string totdays = "";
                                                if (workdays == "1")
                                                {
                                                    totdays = " Day";
                                                }
                                                else if (workdays != "0" && workdays != "1")
                                                {
                                                    totdays = " Days";
                                                }
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "No.of Days");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                                mypdfpage.Add(ptsp);
                                                if (workdays != "0")
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, workdays.ToString() + totdays);
                                                }
                                                else
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "-");
                                                }
                                                mypdfpage.Add(ptsp);
                                            }
                                            else
                                            {
                                                coltop = ((coltop - 20) + rowspace);

                                            }
                                            string absday = "";
                                            if (cblcolumnorder.Items[32].Selected == true)
                                            {
                                                if (abscount == 1)
                                                {
                                                    absday = " Day";
                                                }
                                                else if (abscount != 0 && abscount != 1)
                                                {
                                                    absday = " Days";
                                                }
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Absent Days                   :");
                                                mypdfpage.Add(ptsp);
                                                if (abscount != 0)
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(abscount) + absday);
                                                    mypdfpage.Add(ptsp);
                                                }
                                                else
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString("-"));
                                                    mypdfpage.Add(ptsp);
                                                }
                                            }
                                            string presdays = "";
                                            if (totalpreset == 1)
                                            {
                                                presdays = " Day";
                                            }
                                            else if (totalpreset != 0 && totalpreset != 1)
                                            {
                                                presdays = " Days";
                                            }

                                            if (cblcolumnorder.Items[33].Selected == true)
                                            {
                                                coltop = coltop + rowspace;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "No.of Days Present ");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                                mypdfpage.Add(ptsp);
                                                if (totalpreset != 0)
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, totalpreset.ToString() + presdays);
                                                }
                                                else
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "-");
                                                }
                                                mypdfpage.Add(ptsp);
                                            }
                                            else
                                            {
                                                coltop = ((coltop - 20) + rowspace);

                                            }
                                            string lopdays = "";
                                            if (cblcolumnorder.Items[29].Selected == true)
                                            {
                                                if (tot_lop == 1)
                                                {
                                                    lopdays = "Day";
                                                }
                                                else
                                                {
                                                    lopdays = "Days";
                                                }
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "LOP Days                       :");
                                                mypdfpage.Add(ptsp);
                                                if (tot_lop != 0)
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, nooflop.ToString() + " " + lopdays);
                                                    mypdfpage.Add(ptsp);
                                                }
                                                else
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString("-"));
                                                    mypdfpage.Add(ptsp);
                                                }
                                            }
                                            coltop = coltop + rowspace + rowspace;
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Earnings");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 270, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Amount in Rs.");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Deductions");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 720, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Amount in Rs.");
                                            mypdfpage.Add(ptsp);
                                            int dedu = coltop + 10;
                                            int allo = coltop + 10;
                                            double payband = 0;
                                            double gradepay = 0;
                                            double basic_pay3 = 0;
                                            string allowance3 = "";
                                            string deduction3 = "";
                                            double lopd = 0;
                                            double bassicd = 0;
                                            double DblAllowLOP = 0;
                                            double DblAllowLOP1 = 0;
                                            double G_pay = 0;
                                            Double actbasic = 0;
                                            if (dvpay.Count > 0)
                                            {
                                                //payband = Convert.ToDouble(dvpay[0]["pay_band"].ToString());
                                                Double.TryParse(dvpay[0]["pay_band"].ToString(), out payband);
                                                //gradepay = Convert.ToDouble(dvpay[0]["grade_pay"].ToString());
                                                double.TryParse(dvpay[0]["grade_pay"].ToString(), out gradepay);
                                                //basic_pay3 = Convert.ToDouble(dvpay[0]["bsalary"].ToString());
                                                double.TryParse(dvpay[0]["bsalary"].ToString(), out basic_pay3);
                                                //G_pay = Convert.ToDouble(dvpay[0]["G_Pay"].ToString());
                                                double.TryParse(dvpay[0]["G_Pay"].ToString(), out G_pay);
                                                //actbasic = Convert.ToDouble(dvpay[0]["basic_alone"].ToString());
                                                double.TryParse(dvpay[0]["basic_alone"].ToString(), out actbasic);
                                                //lop = Convert.ToDouble(dvpay[0]["lop"].ToString());
                                                double.TryParse(dvpay[0]["lop"].ToString(), out lop);
                                                if (lop != 0)
                                                {
                                                    lopd = Convert.ToDouble(lop);
                                                }
                                                else
                                                {
                                                    lopd = 0;
                                                }
                                                bassicd = Convert.ToDouble(basic_pay3);
                                                if (chkIncLopAmnt.Checked == true)
                                                {
                                                    if (bassicd != 0)
                                                    {
                                                        allo = allo + rowspace;
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "BASIC PAY");
                                                        mypdfpage.Add(ptsp);
                                                        bassicd = Math.Round(bassicd, 2, MidpointRounding.AwayFromZero);
                                                        string alowamount = "";
                                                        alowamount = bassicd.ToString();
                                                        alowamount = String.Format("{0:0.00}", alowamount);
                                                        double myValue = 0;
                                                        double.TryParse(alowamount, out myValue);
                                                        myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                        mypdfpage.Add(ptsp);
                                                    }
                                                    if (gradepay != 0)
                                                    {
                                                        allo = allo + rowspace;
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "GRADE PAY");
                                                        mypdfpage.Add(ptsp);
                                                        gradepay = Math.Round(gradepay, 2, MidpointRounding.AwayFromZero);
                                                        string alowamount = "";
                                                        alowamount = gradepay.ToString();
                                                        alowamount = String.Format("{0:0.00}", alowamount);
                                                        double myValue = 0;
                                                        double.TryParse(alowamount, out myValue);
                                                        myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                        mypdfpage.Add(ptsp);
                                                    }
                                                    if (payband != 0)
                                                    {
                                                        allo = allo + rowspace;
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "PAY BAND");
                                                        mypdfpage.Add(ptsp);
                                                        payband = Math.Round(payband, 2, MidpointRounding.AwayFromZero);
                                                        string alowamount = payband.ToString();
                                                        alowamount = String.Format("{0:0.00}", alowamount);
                                                        double myValue = 0;
                                                        double.TryParse(alowamount, out myValue);
                                                        myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                        mypdfpage.Add(ptsp);
                                                    }
                                                }
                                                else
                                                {
                                                    if (bassicd != 0 || actbasic != 0)
                                                    {
                                                        allo = allo + rowspace;
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "BASIC PAY");
                                                        mypdfpage.Add(ptsp);
                                                        bassicd = Math.Round(bassicd, 2, MidpointRounding.AwayFromZero);
                                                        string alowamount = "";
                                                        if (cblcolumnorder.Items[35].Selected == true)
                                                        {
                                                            alowamount = actbasic.ToString();
                                                        }
                                                        else
                                                        {
                                                            alowamount = bassicd.ToString();
                                                        }
                                                        alowamount = String.Format("{0:0.00}", alowamount);
                                                        double myValue = 0;
                                                        double.TryParse(alowamount, out myValue);
                                                        myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                        mypdfpage.Add(ptsp);
                                                    }
                                                    if (gradepay != 0 || G_pay != 0)
                                                    {
                                                        allo = allo + rowspace;
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "GRADE PAY");
                                                        mypdfpage.Add(ptsp);
                                                        gradepay = Math.Round(gradepay, 2, MidpointRounding.AwayFromZero);
                                                        string alowamount = "";
                                                        if (cblcolumnorder.Items[35].Selected == false)//true
                                                        {
                                                            alowamount = G_pay.ToString();
                                                        }
                                                        else
                                                        {
                                                            alowamount = gradepay.ToString();
                                                        }
                                                        alowamount = String.Format("{0:0.00}", alowamount);
                                                        double myValue = 0;
                                                        double.TryParse(alowamount, out myValue);
                                                        myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                        mypdfpage.Add(ptsp);
                                                    }
                                                    if (payband != 0)
                                                    {
                                                        allo = allo + rowspace;
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "PAY BAND");
                                                        mypdfpage.Add(ptsp);
                                                        payband = Math.Round(payband, 2, MidpointRounding.AwayFromZero);
                                                        string alowamount = payband.ToString();
                                                        alowamount = String.Format("{0:0.00}", alowamount);
                                                        double myValue = 0;
                                                        double.TryParse(alowamount, out myValue);
                                                        myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                        mypdfpage.Add(ptsp);
                                                    }
                                                }
                                                //Alter By Jeyaprakash on May 30th
                                                allowance3 = dvpay[0]["allowances"].ToString();
                                                string[] allowanmce_arr1;
                                                string alowancesplit;
                                                int exatval = 0;
                                                allowanmce_arr1 = allowance3.Split('\\');
                                                for (int i = 0; i < allowanmce_arr1.GetUpperBound(0); i++)
                                                {
                                                    exatval = allowanmce_arr1.GetUpperBound(0);
                                                    alowancesplit = allowanmce_arr1[i];
                                                    string[] allowanceda = alowancesplit.Split(';');
                                                    if (allowanceda.GetUpperBound(0) >= 3)
                                                    {
                                                        string da = allowanceda[0];
                                                        string mode = Convert.ToString(allowanceda[1]);
                                                        string daac = "";
                                                        string da3 = "";
                                                        if (allowanceda[2].Trim() != "")
                                                        {
                                                            string[] spval = allowanceda[2].Split('-');
                                                            if (spval.Length == 2)
                                                            {
                                                                if (mode.Trim().ToUpper() == "PERCENT")
                                                                {
                                                                    da3 = Convert.ToString(spval[1]);
                                                                    if (cblcolumnorder.Items[35].Selected == true)//barath 19.01.18
                                                                        da3 = Convert.ToString(allowanceda[3]);
                                                                }
                                                                else
                                                                {
                                                                    da3 = Convert.ToString(spval[0]);
                                                                    if (cblcolumnorder.Items[35].Selected == true)//barath 19.01.18
                                                                        da3 = Convert.ToString(allowanceda[3]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                da3 = Convert.ToString(allowanceda[3]);
                                                            }
                                                        }
                                                        daac = Convert.ToString(allowanceda[3]);
                                                        double da2 = 0;
                                                        Double.TryParse(daac, out da2);
                                                        double ds3 = 0;
                                                        double.TryParse(da3, out ds3);
                                                        ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                                        string DblAllowActVal = daac;
                                                        string DblAllowVal = allowanceda[3];
                                                        DblAllowLOP = (Convert.ToDouble(DblAllowActVal) - Convert.ToDouble(DblAllowVal));
                                                        DblAllowLOP = Math.Round(DblAllowLOP);
                                                        DblAllowLOP1 = DblAllowLOP1 + DblAllowLOP;
                                                        if (ds3 != 0)
                                                        {
                                                            allo = allo + rowspace;
                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left1, allo, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, da.ToString());
                                                            mypdfpage.Add(ptsp);
                                                            string alowamount = ds3.ToString();
                                                            alowamount = String.Format("{0:0.00}", alowamount);
                                                            double myValue = 0;
                                                            double.TryParse(alowamount, out myValue);
                                                            myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                            mypdfpage.Add(ptsp);
                                                        }
                                                        //i = allowanmce_arr1.GetUpperBound(0);
                                                    }
                                                }
                                                deduction3 = dvpay[0]["deductions"].ToString();//delsiref
                                                string[] deduction_arr1;
                                                string deductionsplit;
                                                int exactval = 0;
                                                deduction_arr1 = deduction3.Split('\\');
                                                for (int i = 0; i < deduction_arr1.GetUpperBound(0); i++)
                                                {
                                                    exatval = deduction_arr1.GetUpperBound(0);
                                                    deductionsplit = deduction_arr1[i];
                                                    string[] deductionda = deductionsplit.Split(';');
                                                    if (deductionda.GetUpperBound(0) >= 3)
                                                    {
                                                        string da = deductionda[0];
                                                        string mode = Convert.ToString(deductionda[1]);
                                                        string daac = "";
                                                        string da3 = "";
                                                        string activeloan = "";
                                                        //poo 28.10.17
                                                        # region for calculate tenure
                                                        string selpaytotmon = "select LoanCode,PayTotMon from staffloandet where isactive = 1 and isclose = 0 and staff_code = '" + staffcode + "' and dedname = '" + da.ToString() + "' and LoanType='0'";
                                                        selpaytotmon += " select isactive from staffloandet where isactive = 1 and isclose = 0 and staff_code = '" + staffcode + "' and dedname = '" + da.ToString() + "' and LoanType='0'";
                                                        DataSet loanduration = new DataSet();
                                                        loanduration = d2.select_method_wo_parameter(selpaytotmon, "Text");
                                                        if (loanduration.Tables.Count > 0 && loanduration.Tables[1].Rows.Count > 0)
                                                        {
                                                            activeloan = loanduration.Tables[1].Rows[0]["isactive"].ToString();
                                                        }
                                                        if (loanduration.Tables.Count > 0 && loanduration.Tables[0].Rows.Count > 0 && activeloan == "True")
                                                        {
                                                            for (int lk = 0; lk < loanduration.Tables[0].Rows.Count; lk++)
                                                            {
                                                                selpaidcount = d2.GetFunction("select count(staff_code) as paidcount from staffloanpaydet where LoanCode='" + Convert.ToString(loanduration.Tables[0].Rows[lk]["LoanCode"]) + "' and Staff_Code='" + staffcode + "'");
                                                                loandet = Convert.ToString(loanduration.Tables[0].Rows[lk]["PayTotMon"]);
                                                                tenure = "(" + selpaidcount + "/" + loandet + ")";//poo 28.10.17

                                                            }
                                                        }
                                                        else
                                                        {
                                                            tenure = " ";
                                                        }
                                                        # endregion
                                                        if (deductionda[2].Trim() != "")
                                                        {
                                                            string[] spval = deductionda[2].Split('-');
                                                            if (spval.Length == 2)
                                                            {
                                                                if (mode.Trim().ToUpper() == "PERCENT")
                                                                {
                                                                    da3 = Convert.ToString(spval[1]);
                                                                }
                                                                else
                                                                {
                                                                    da3 = Convert.ToString(spval[0]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                da3 = Convert.ToString(deductionda[3]);
                                                            }
                                                        }
                                                        daac = Convert.ToString(deductionda[3]);
                                                        double da2 = 0;
                                                        Double.TryParse(daac, out da2);
                                                        double ds3 = 0;
                                                        double.TryParse(da3, out ds3);
                                                        ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                                        string DblAllowActVal = daac;
                                                        string DblAllowVal = deductionda[3];
                                                        DblAllowLOP = (Convert.ToDouble(DblAllowActVal) - Convert.ToDouble(DblAllowVal));
                                                        DblAllowLOP = Math.Round(DblAllowLOP);
                                                        DblAllowLOP1 = DblAllowLOP1 + DblAllowLOP;
                                                        if (ds3 != 0)
                                                        {
                                                            dedu = dedu + rowspace; //28 poo
                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, left3, dedu, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, da.ToString() + tenure); //poo 28.10.17
                                                            mypdfpage.Add(ptsp);
                                                            string alowamount = ds3.ToString();
                                                            alowamount = String.Format("{0:0.00}", alowamount);
                                                            double myValue = 0;
                                                            double.TryParse(alowamount, out myValue);
                                                            myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mydocument, 770, dedu, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                            mypdfpage.Add(ptsp);
                                                        }
                                                        //i = allowanmce_arr1.GetUpperBound(0);
                                                    }
                                                }
                                            }
                                            if (allo > dedu)
                                            {
                                                if (coltop < allo)
                                                {
                                                    coltop = allo;
                                                }
                                            }
                                            else
                                            {
                                                if (coltop < dedu)
                                                {
                                                    coltop = dedu;
                                                }
                                            }
                                            string lopamnt = "";
                                            if (cblcolumnorder.Items[35].Selected == true)
                                            {
                                                if (lop != 0)
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mydocument, left3, coltop + 15, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "LOP AMOUNT");
                                                    mypdfpage.Add(ptsp);
                                                    lopamnt = String.Format("{0:0.00}", lop11);
                                                    double myValue = 0;
                                                    double.TryParse(lopamnt, out myValue);
                                                    myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mydocument, 770, coltop + 15, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                    mypdfpage.Add(ptsp);
                                                    coltop = coltop + 20;
                                                }
                                            }
                                            double grossamnt = 0;
                                            coltop = coltop + rowspace + rowspace;
                                            Double nesal3 = Convert.ToDouble(dvpay[0]["netsal"].ToString());
                                            nesal3 = Math.Round(nesal3, 0, MidpointRounding.AwayFromZero);
                                            //Double totall = Convert.ToDouble(dvpay[0]["NetAddAct"].ToString());
                                            Double totall = 0;
                                            if (chkIncLopAmnt.Checked == true)
                                            {
                                                totall = Convert.ToDouble(dvpay[0]["NetAdd"].ToString());
                                            }
                                            else
                                            {
                                                if (cblcolumnorder.Items[35].Selected == false)
                                                    totall = Convert.ToDouble(dvpay[0]["NetAdd"].ToString());
                                                else
                                                    totall = Convert.ToDouble(dvpay[0]["NetAddAct"].ToString());
                                            }
                                            totall = Math.Round(totall, 2, MidpointRounding.AwayFromZero);
                                            double netded = Convert.ToDouble(dvpay[0]["netded"].ToString());
                                            netded = Math.Round(netded, 2, MidpointRounding.AwayFromZero);
                                            Double lqw = Convert.ToDouble(lopamount);
                                            lqw = Math.Round(totall, 2, MidpointRounding.AwayFromZero);
                                            string finalamount = totall.ToString();
                                            finalamount = String.Format("{0:0.00}", finalamount);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "Gross Total");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 360, coltop - 25, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "___________");
                                            mypdfpage.Add(ptsp);
                                            double newValue = 0;
                                            double.TryParse(finalamount, out newValue);
                                            newValue = Math.Round(newValue, 0, MidpointRounding.AwayFromZero);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 320, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(newValue) + ".00");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 360, coltop + 10, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "___________");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 790, coltop - 25, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "___________");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Total Deductions");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 790, coltop + 10, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "___________");
                                            mypdfpage.Add(ptsp);
                                            Double lopamt = 0;
                                            Double dedamount = 0; // poo 16.11.17//19.1.18
                                            if (cblcolumnorder.Items[35].Selected == true)
                                            {
                                                lopamt = Convert.ToDouble(lopamount);
                                                lopamt = Math.Round(lopamt, 2, MidpointRounding.AwayFromZero);
                                                dedamount = Convert.ToDouble(lopamt) + Convert.ToDouble(netded) - Convert.ToDouble(lopamt);
                                            }
                                            else
                                            {
                                                lopamt = Convert.ToDouble(lopamount);
                                                lopamt = Math.Round(lopamt, 2, MidpointRounding.AwayFromZero);
                                                dedamount = Convert.ToDouble(netded) - Convert.ToDouble(lopamt);
                                            }

                                            //Double dedamount = 0; // poo 16.11.17
                                            //if (netded == 0) // poo 16.11.17
                                            //{
                                            //    dedamount = Convert.ToDouble(lopamt) + Convert.ToDouble(netded); //poo 06.11.17
                                            //}
                                            //else
                                            //{
                                            //    dedamount = Convert.ToDouble(netded); //poo 06.11.17
                                            //}

                                            dedamount = Math.Round(dedamount, 2, MidpointRounding.AwayFromZero);
                                            finalamount = String.Format("{0:0.00}", finalamount);
                                            double isValue = 0;
                                            double.TryParse(Convert.ToString(dedamount), out isValue);
                                            isValue = Math.Round(isValue, 0, MidpointRounding.AwayFromZero);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 770, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, isValue.ToString() + ".00");
                                            mypdfpage.Add(ptsp);
                                            coltop = coltop + rowspace + rowspace;
                                            finalamount = String.Format("{0:0.00}", finalamount);
                                            int totnet = 0;
                                            Double finamnt = 0;
                                            Double.TryParse(finalamount, out finamnt);
                                            finamnt = Math.Round(finamnt, 0, MidpointRounding.AwayFromZero);
                                            dedamount = Math.Round(dedamount, 0, MidpointRounding.AwayFromZero);
                                            totnet = Convert.ToInt32(finamnt) - Convert.ToInt32(dedamount);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left1, coltop, 500, 50), System.Drawing.ContentAlignment.MiddleLeft, "Net Pay : ");
                                            mypdfpage.Add(ptsp);
                                            double hasValue = 0;
                                            double.TryParse(Convert.ToString(totnet), out hasValue);
                                            hasValue = Math.Round(hasValue, 0, MidpointRounding.AwayFromZero);
                                            //if (chkIncLopAmnt.Checked == true)
                                            //{
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 50, coltop, 150, 50), System.Drawing.ContentAlignment.MiddleRight, "Rs . " + nesal3.ToString() + ".00");
                                            mypdfpage.Add(ptsp);
                                            //}
                                            //else
                                            //{
                                            //}
                                            string word = ConvertNumbertoWords(Convert.ToInt32(nesal3)); // added by jairam 01-12-2014
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 150, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleRight, " (Rupees " + word + " Only)");
                                            mypdfpage.Add(ptsp);
                                            string getfooter = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Footer Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
                                            if (getfooter.Trim() != "" && getfooter.Trim() != "0")
                                            {
                                                string[] spiltfooterdetails = getfooter.Split(',');
                                                tableforfooter = mydocument.NewTable(Fontbold, 3, spiltfooterdetails.Length, 3);
                                                tableforfooter.SetBorders(Color.Black, 1, BorderType.None);
                                                for (int i = 0; i <= spiltfooterdetails.GetUpperBound(0); i++)
                                                {
                                                    string collfooter = spiltfooterdetails[i].ToString();
                                                    tableforfooter.Cell(0, i).SetContent(collfooter);
                                                    tableforfooter.Cell(2, 0).SetContentAlignment(ContentAlignment.BottomRight);
                                                }
                                                coltop = coltop + 50;//added delsi 10/05/2018
                                                Gios.Pdf.PdfTablePage pdftabpage = tableforfooter.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50));
                                                mypdfpage.Add(pdftabpage);//delsi changed coltop + 90 to coltop + 50
                                            }
                                            if (rec % 2 != 0)
                                            {
                                                string getfooters = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Footer Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
                                                if (getfooter.Trim() != "" && getfooter.Trim() != "0")
                                                {
                                                    PdfArea tete = new PdfArea(mydocument, 10, starttop, 830, coltop - 20);//changed value
                                                    PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                                    mypdfpage.Add(pr1);
                                                    if (totlastaff <= rec)
                                                    {
                                                        mypdfpage.SaveToDocument();
                                                    }
                                                }
                                                else
                                                {

                                                    PdfArea tete = new PdfArea(mydocument, 10, starttop, 830, coltop + 20);//changed value
                                                    PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                                    mypdfpage.Add(pr1);
                                                    if (totlastaff <= rec)
                                                    {
                                                        mypdfpage.SaveToDocument();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                string getfooters = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Footer Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
                                                if (getfooter.Trim() != "" && getfooter.Trim() != "0")
                                                {
                                                    PdfArea tete = new PdfArea(mydocument, 10, starttop, 830, coltop - gettop + 8);//changed value
                                                    PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                                    mypdfpage.Add(pr1);
                                                    mypdfpage.SaveToDocument();
                                                }
                                                else
                                                {
                                                    PdfArea tete = new PdfArea(mydocument, 10, starttop, 830, coltop - gettop + 33);//changed value
                                                    PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                                    mypdfpage.Add(pr1);
                                                    mypdfpage.SaveToDocument();

                                                }
                                            }
                                            gettop = coltop + 100;
                                        }
                                    }
                                }
                            }
                        }
                #endregion
                        //*****************End****************************//
                        if (getvalflag == true)
                        {
                            genpdfcount++;
                            lblnorec.Visible = false;
                            lblnorec.Text = "";
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";
                                string szFile = DateTime.Now.ToString("ddMMyyyyhhmmsstt") + "PaySlip.pdf";
                                mydocument.SaveToFile(szPath + szFile);
                                Response.ClearHeaders();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(szPath + szFile);
                            }
                        }
                    }
                }
                if (genpdfcount > 0)
                {
                    lblnorec.Visible = false;
                    lblnorec.Text = "";
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Select Staff and then Proceed!";
                }
                fpsalary.SaveChanges();
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Staff and then Proceed!";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Individual_SalaryReport.aspx");
        }
    }
    public void loadpayslipnew()
    {
        try
        {
            lblnorec.Visible = false;
            Printcontrol.Visible = false;
            lblsmserror.Visible = false;
            fpsalary.SaveChanges();
            string payscale = "";// added by Jeyaprakash 12 Mar 2016
            string collname = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string pincode = "";
            string address = "";
            Font Fontbold = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 20, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 13, FontStyle.Bold);
            string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + collegecode1 + "'";
            str = str + " ;select * from incentives_master where college_code='" + collegecode1 + "'";
            str = str + " ;select distinct CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "'and PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and college_Code=" + collegecode1 + "";
            DataSet ds = d2.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                collname = ds.Tables[0].Rows[0]["collname"].ToString();
                address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                if (address1.Trim() != "" && address1 != null)
                {
                    address = address1;
                }
                if (address2.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address2;
                    }
                    else
                    {
                        address = address2;
                    }
                }
                if (address3.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address3;
                    }
                    else
                    {
                        address = address3;
                    }
                }
                if (pincode.Trim() != "" && pincode != null)
                {
                    if (address != "")
                    {
                        address = address + '-' + pincode;
                    }
                    else
                    {
                        address = pincode;
                    }
                }
            }
            string allowmaster = "";
            string deductmaster = "";
            if (ds.Tables[1].Rows.Count > 0)
            {
                allowmaster = ds.Tables[1].Rows[0]["allowances"].ToString();
                deductmaster = ds.Tables[1].Rows[0]["deductions"].ToString();
            }
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] spdet = deductmaster.Split(';');
            for (int d = 0; d <= spdet.GetUpperBound(0); d++)
            {
                string[] spdedet = spdet[d].Split('\\');
                if (spdedet.GetUpperBound(0) >= 1)
                {
                    string val = spdedet[0];
                    string val1 = spdedet[1];
                    if (!dict.ContainsKey(val))
                    {
                        dict.Add(val, val1);
                    }
                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                string date3 = Convert.ToString(ds.Tables[2].Rows[0]["from_date"]);
                string datefrom3;
                int monthname1;
                string monyear;
                string monthname2 = "";
                int monthnamenum;
                string yearto = "";
                string monyearto = "";
                string[] split = date3.Split(new Char[] { '/' });
                int totlastaff = 0;
                int left1 = 20;
                int left1a = 135;
                int left2 = 145;
                int left3 = 423;
                int left4 = 570;
                int incrre = 1;
                int rowcount = 0;
                if (cblcolumnorder.Items[53].Selected == true && fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL")
                {
                    rowcount = fpsalary.Sheets[0].RowCount - 2;
                }
                if (fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Text == "TOTAL" && cblcolumnorder.Items[53].Selected == false)
                {
                    rowcount = fpsalary.Sheets[0].RowCount - 1;
                }
                for (int res = incrre; res < rowcount; res = res + incrre)
                {
                    for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                    {
                        if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT" && fpsalary.Sheets[0].Rows[res].Visible == true)
                        {
                            string text = fpsalary.Sheets[0].Cells[res, lst].Text.ToString();
                            if (text != "" && text != null)
                            {
                                int isval = Convert.ToInt32(fpsalary.Sheets[0].Cells[res, lst].Value);
                                if (isval == 1)
                                {
                                    totlastaff++;
                                }
                            }
                        }
                    }
                }
                datefrom3 = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                string date4 = Convert.ToString(ds.Tables[2].Rows[0]["to_date"]);
                int year3 = Convert.ToInt16(split[2].ToString());
                string[] split1 = date4.Split(new Char[] { '/' });
                string dateto4 = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                monthname2 = split1[0].ToString();
                monthnamenum = Convert.ToInt32(monthname2.ToString());
                yearto = split1[2].ToString();
                string mnmae = split[1].ToString();
                monthname1 = Convert.ToInt16(mnmae);
                monthname2 = split1[1].ToString();
                monthnamenum = Convert.ToInt16(monthname2);
                string year = split[2].ToString();
                monyear = monthname1.ToString() + "/" + year;
                monyearto = monthnamenum.ToString() + "/" + yearto;
                Boolean getvalflag = false;
                int dayfrm = 0;
                string dayto;
                int daytonum;
                string lopdates = "";
                string[] leavetype = new string[50];
                sql = "select shortname from leave_category where college_code=" + Session["collegecode"] + "";
                DataSet dsleave = d2.select_method_wo_parameter(sql, "Text");
                int lev = 0;
                for (int le = 0; le < dsleave.Tables[0].Rows.Count; le++)
                {
                    lev++;
                    string levatype = dsleave.Tables[0].Rows[le]["shortname"].ToString().Trim().ToLower();
                    leavetype[lev] = levatype;
                }
                sql = "select m.*,s.staff_name,s.pfnumber,s.ESI_No,s.bankaccount,s.pangirnumber ,st.payscale as pay_scalenew , st.allowances as actallowance,h.dept_name as deptname,d.desig_name as designame from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.latestrec = 1 and m.college_code=s.college_code and m.college_code =h.college_code and m.college_code=d.collegeCode and s.college_code='" + collegecode1 + "' and m.PayYear='" + ddl_year.SelectedValue.ToString() + "' and m.PayMonth='" + ddl_mon.SelectedValue.ToString() + "'";
                sql = sql + " ; select convert(nvarchar(15),sa.date_of_birth,103) as dob,sm.staff_code,convert(nvarchar(15),sm.retr_date ,103) as retir,sm.staff_name,CONVERT(varchar(10),sm.join_date,103) as join_date from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no ";
                DataSet dspay = d2.select_method_wo_parameter(sql, "text");
                DataTable dtpay = dspay.Tables[0];
                DataView dvpay = new DataView();
                DataView dvapp = new DataView();
                int gettop = 0;
                int rec = 0;
                string getlink = d2.GetFunction("select linkvalue from inssettings where linkname='Calculate LOP from Attendance' and college_code='" + collegecode1 + "'");
                sql1 = "select * from staff_attnd where mon_year between '" + monyear + "' and '" + monyearto + "' ";
                DataSet dsstaffattendance = d2.select_method_wo_parameter(sql1, "text");
                DataView dvstaffattendance = new DataView();
                int genpdfcount = 0;
                //****************Added By Jeyaprakash 10.04.2016************************//
                #region for format1
                for (int res = 1; res < rowcount; res++)
                {
                    if (fpsalary.Sheets[0].Rows[res].Visible == true)
                    {
                        for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                        {
                            if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT")
                            {
                                string text = fpsalary.Sheets[0].Cells[res, lst].Text.ToString();
                                if (text != "" && text != null)
                                {
                                    int isval = Convert.ToInt32(fpsalary.Sheets[0].Cells[res, lst].Value);
                                    if (isval == 1)
                                    {
                                        string staffcode = "";
                                        string designation = "";
                                        string deptname = "";
                                        for (int chklst = 0; chklst < lstcolorder.Items.Count; chklst++)
                                        {
                                            if (lstcolorder.Items[chklst].Text == "STAFF CODE")
                                            {
                                                staffcode = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DESIGN")
                                            {
                                                designation = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DEPT")
                                            {
                                                deptname = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "LOP DATES")
                                            {
                                                lopdates = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                        }
                                        string staffname = "";
                                        string bankno = "";
                                        string pfno = "";
                                        string esino = "";
                                        string panno = "";
                                        string Allowances = "";
                                        string lopamount = "";
                                        if (staffcode.Trim() == "")
                                        {
                                            lblnorec.Visible = true;
                                            lblnorec.Text = "Please select Staff Code!";
                                            return;
                                        }
                                        Double totdection = 0;
                                        dtpay.DefaultView.RowFilter = " staff_code='" + staffcode + "'";
                                        dvpay = dtpay.DefaultView;
                                        if (dvpay.Count > 0)
                                        {
                                            staffname = dvpay[0]["staff_name"].ToString();
                                            bankno = dvpay[0]["bankaccount"].ToString();
                                            pfno = dvpay[0]["pfnumber"].ToString();
                                            esino = dvpay[0]["ESI_No"].ToString();
                                            payscale = dvpay[0]["pay_scalenew"].ToString();// added by Jeyaprakash 12 Mar 2016
                                            panno = dvpay[0]["pangirnumber"].ToString();
                                            Allowances = dvpay[0]["allowances"].ToString();
                                            lopamount = dvpay[0]["Tot_lop"].ToString();
                                            designation = dvpay[0]["designame"].ToString();
                                            deptname = dvpay[0]["deptname"].ToString();
                                            if (pfno.Trim().ToLower() == "" || pfno == null || pfno.Trim() == "0")
                                            {
                                                pfno = "-";
                                            }
                                            string setyear = dvpay[0]["PayYear"].ToString();
                                            if (setyear.Trim() == "" || setyear == null)
                                            {
                                                setyear = yearto;
                                            }
                                            string noofpresent = "";
                                            string presnt = "";
                                            string workdays = "";
                                            string leavedays = "";
                                            string nooflop = "";
                                            noofpresent = dvpay[0]["leavedetail"].ToString();
                                            string[] presplit = noofpresent.Split(';');
                                            if (presplit.Length >= 7)
                                            {
                                                presnt = presplit[1].ToString();
                                                workdays = presplit[0].ToString();
                                                leavedays = presplit[2].ToString();
                                                nooflop = presplit[6].ToString();
                                            }
                                            gettop = 20;
                                            rec = 0;
                                            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
                                            Gios.Pdf.PdfTable tableforfooter;
                                            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
                                            int starttop = gettop;
                                            int coltop = gettop;
                                            int rowspace = 20;
                                            try
                                            {
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                                {
                                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                                    mypdfpage.Add(LogoImage, 25, coltop + 10, 400);
                                                }
                                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                                {
                                                    string leftlogo = "Left_Logo(" + collegecode1 + ")";
                                                    MemoryStream memoryStream = new MemoryStream();
                                                    string sellogo = "select logo1 from collinfo where college_code='" + collegecode1 + "'";
                                                    ds.Clear();
                                                    ds = d2.select_method_wo_parameter(sellogo, "Text");
                                                    if (ds.Tables.Count > 0)
                                                    {
                                                        if (ds.Tables[0].Rows.Count > 0)
                                                        {
                                                            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                                            memoryStream.Write(file, 0, file.Length);
                                                            if (file.Length > 0)
                                                            {
                                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"));
                                                                mypdfpage.Add(LogoImage, 25, coltop + 10, 400);
                                                            }
                                                            memoryStream.Dispose();
                                                            memoryStream.Close();
                                                        }
                                                    }
                                                }
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                                {
                                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                                    mypdfpage.Add(LogoImage, 750, coltop + 10, 350);
                                                }
                                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                                {
                                                    string rightlogo = "Right_Logo(" + collegecode1 + ")";
                                                    MemoryStream memoryStream = new MemoryStream();
                                                    string sellogo = "select logo2 from collinfo where college_code='" + collegecode1 + "'";
                                                    ds.Clear();
                                                    ds = d2.select_method_wo_parameter(sellogo, "Text");
                                                    if (ds.Tables.Count > 0)
                                                    {
                                                        if (ds.Tables[0].Rows.Count > 0)
                                                        {
                                                            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                                            memoryStream.Write(file, 0, file.Length);
                                                            if (file.Length > 0)
                                                            {
                                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"));
                                                                mypdfpage.Add(LogoImage, 750, coltop + 10, 350);
                                                            }
                                                            memoryStream.Dispose();
                                                            memoryStream.Close();
                                                        }
                                                    }
                                                }
                                            }
                                            catch { }
                                            PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 0, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                            mypdfpage.Add(ptc);
                                            coltop = coltop + rowspace;
                                            PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop + 10, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                            mypdfpage.Add(pts);
                                            coltop = coltop + rowspace;
                                            PdfTextArea ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop + 10, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, "Pay Slip for the month of " + ddl_mon.SelectedItem.Text + " " + "  " + setyear + "");
                                            mypdfpage.Add(ptsp);
                                            string stl = "";
                                            //Double lop = Convert.ToDouble(dvpay[0]["lop"].ToString());
                                            Double lop = 0;
                                            Double.TryParse(dvpay[0]["Tot_lop"].ToString(), out lop);
                                            double lop11 = lop;
                                            double tot_lop = 0;
                                            Double.TryParse(nooflop, out tot_lop);
                                            int maxdays = getmaxdays(monthname1, year3);
                                            double[] clleave = new double[50];
                                            dayfrm = Convert.ToInt32(split[0]);
                                            dayto = split1[0].ToString();
                                            daytonum = Convert.ToInt32(dayto);
                                            int abs = 0;
                                            int abscount = 0;
                                            dsstaffattendance.Tables[0].DefaultView.RowFilter = " mon_year ='" + monyear + "'  and staff_code='" + staffcode + "'";
                                            dvstaffattendance = dsstaffattendance.Tables[0].DefaultView;
                                            if (monthname1 == monthnamenum)
                                            {
                                                for (int day = dayfrm; dayfrm <= daytonum; dayfrm++)
                                                {
                                                    int day5 = 0;
                                                    day5 = 0 + dayfrm;
                                                    if (dsleave.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (dvstaffattendance.Count > 0)
                                                        {
                                                            int day6;
                                                            string attnda = "";
                                                            day6 = 3 + day5;
                                                            attnda = dvstaffattendance[0][day6].ToString();
                                                            if (attnda != "")
                                                            {
                                                                string[] split100 = attnda.Split(new char[] { '-' });
                                                                string mor = "";
                                                                string eve = "";
                                                                mor = split100[0].ToString();
                                                                eve = split100[1].ToString();
                                                                if (mor.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                if (eve.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                                {
                                                                    string levatype = leavetype[le];
                                                                    if (mor.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                    if (eve.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                }
                                                                string[] lave = new string[15];
                                                            }
                                                        }
                                                    }
                                                }
                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                {
                                                    if (leavetype[le] != "LA")
                                                    {
                                                        if (clleave[le] != 0)
                                                        {
                                                            stl = stl + leavetype[le].ToUpper() + "-" + clleave[le] + ", ";
                                                        }
                                                    }
                                                }
                                                if (stl != "")
                                                {
                                                    stl = stl.Substring(0, stl.Length - 2);
                                                }
                                                Array.Clear(clleave, 0, clleave.Length);
                                            }
                                            else
                                            {
                                                dsstaffattendance.Tables[0].DefaultView.RowFilter = " mon_year ='" + monyear + "'  and staff_code='" + staffcode + "'";
                                                dvstaffattendance = dsstaffattendance.Tables[0].DefaultView;
                                                for (int day = dayfrm; dayfrm <= maxdays; dayfrm++)
                                                {
                                                    int day5 = 0;
                                                    day5 = 0 + dayfrm;
                                                    if (dsleave.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (dvstaffattendance.Count > 0)
                                                        {
                                                            int day6;
                                                            string attnda = "";
                                                            day6 = 3 + day5;
                                                            attnda = dvstaffattendance[0][day6].ToString();
                                                            if (attnda != "")
                                                            {
                                                                string[] split100 = attnda.Split(new char[] { '-' });
                                                                string mor = "";
                                                                string eve = "";
                                                                mor = Convert.ToString(split100[0]);
                                                                eve = Convert.ToString(split100[1]);
                                                                if (mor.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                if (eve.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                                {
                                                                    string levatype = leavetype[le];
                                                                    if (mor.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                    if (eve.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                }
                                                                string[] lave = new string[15];
                                                            }
                                                        }
                                                    }
                                                }
                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                {
                                                    if (leavetype[le] != "LA")
                                                    {
                                                        if (clleave[le] != 0)
                                                        {
                                                            stl = stl + leavetype[le].ToUpper() + "-" + clleave[le] + ", ";
                                                        }
                                                    }
                                                }
                                                dsstaffattendance.Tables[0].DefaultView.RowFilter = " mon_year='" + monyearto + "' and staff_code='" + staffcode + "'";
                                                dvstaffattendance = dsstaffattendance.Tables[0].DefaultView;
                                                for (int day = 1; day <= daytonum; day++)
                                                {
                                                    int day5 = 0;
                                                    day5 = 0 + day;
                                                    if (dsleave.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (dvstaffattendance.Count > 0)
                                                        {
                                                            int day6;
                                                            string attnda = "";
                                                            day6 = 3 + day5;
                                                            attnda = dvstaffattendance[0][day6].ToString();
                                                            if (attnda != "")
                                                            {
                                                                string[] split100 = attnda.Split(new char[] { '-' });
                                                                string mor = "";
                                                                string eve = "";
                                                                mor = Convert.ToString(split100[0]);
                                                                eve = Convert.ToString(split100[1]);
                                                                if (mor.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                if (eve.Trim() == "A")
                                                                {
                                                                    abs++;
                                                                }
                                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                                {
                                                                    string levatype = leavetype[le];
                                                                    if (mor.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                    if (eve.Trim().ToLower() == levatype)
                                                                    {
                                                                        clleave[le] = clleave[le] + 0.5;
                                                                    }
                                                                }
                                                                string[] lave = new string[15];
                                                            }
                                                        }
                                                    }
                                                }
                                                for (int le = 1; le <= dsleave.Tables[0].Rows.Count; le++)
                                                {
                                                    if (leavetype[le] != "LA")
                                                    {
                                                        if (clleave[le] != 0)
                                                        {
                                                            stl = stl + leavetype[le].ToUpper() + "-" + clleave[le] + ", ";
                                                        }
                                                    }
                                                }
                                                if (stl != "")
                                                {
                                                    stl = stl.Substring(0, stl.Length - 2);
                                                }
                                            }
                                            Array.Clear(clleave, 0, clleave.Length);
                                            double totalpreset = 0;
                                            if (workdays.Trim() != "" && nooflop.Trim() != "")
                                            {
                                                totalpreset = Convert.ToDouble(workdays) - Convert.ToDouble(nooflop);
                                            }
                                            abscount = abs / 2;
                                            //totalpreset = totalpreset - abscount;
                                            coltop = coltop + 60;
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Staff Code");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, staffcode);
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Department                    :");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, deptname);
                                            mypdfpage.Add(ptsp);
                                            coltop = coltop + rowspace;
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "NAME");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, staffname);
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Designation                    :");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, designation);
                                            mypdfpage.Add(ptsp);
                                            if (cblcolumnorder.Items[21].Selected == true)
                                            {
                                                coltop = coltop + rowspace;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "PF A/C No");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, pfno);
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Bank A/C No                  :");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, bankno);
                                                mypdfpage.Add(ptsp);
                                            }
                                            if (cblcolumnorder.Items[22].Selected == true)
                                            {
                                                coltop = coltop + rowspace;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "ESI NO");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(esino));
                                                mypdfpage.Add(ptsp);
                                                if (cblcolumnorder.Items[21].Selected == false)
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Bank A/C No                  :");
                                                    mypdfpage.Add(ptsp);
                                                    ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, bankno);
                                                    mypdfpage.Add(ptsp);
                                                }
                                            }
                                            if (cblcolumnorder.Items[21].Selected == false && cblcolumnorder.Items[22].Selected == false)
                                            {
                                                coltop = coltop + rowspace;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Bank A/C No                  :");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, bankno);
                                                mypdfpage.Add(ptsp);
                                            }
                                            string dob1 = "select convert(nvarchar(15),sa.date_of_birth,103) as dob from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sm.staff_code='" + staffcode + "'";
                                            DataSet sk1 = new DataSet();
                                            DAccess2 ddsd = new DAccess2();
                                            sk1 = ddsd.select_method_wo_parameter(dob1, "Text");
                                            string dob2 = sk1.Tables[0].Rows[0]["dob"].ToString();
                                            string[] group_semi5 = lopdates.Split(',');
                                            string doj8 = "";
                                            for (int i = 0; i <= group_semi5.GetUpperBound(0); i++)
                                            {
                                                string group_semi4 = group_semi5[i].ToString();
                                                string[] splitgroup_semi4 = group_semi4.Split('/');
                                                if (doj8 == "")
                                                {
                                                    doj8 = splitgroup_semi4[0].ToString();
                                                }
                                                else
                                                {
                                                    doj8 = doj8 + "," + splitgroup_semi4[0].ToString();
                                                }
                                            }
                                            if (cblcolumnorder.Items[30].Selected == true)
                                            {
                                                coltop = coltop + rowspace;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "LOP DATES");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                                mypdfpage.Add(ptsp);
                                                if (lopdates != "")
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, doj8);
                                                }
                                                else
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "-");
                                                }
                                            }
                                            mypdfpage.Add(ptsp);
                                            if (cblcolumnorder.Items[51].Selected == true)
                                            {
                                                coltop = coltop + 20;
                                                if (payscale != "")
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Pay Scale                      :");
                                                    mypdfpage.Add(ptsp);
                                                    ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, payscale);
                                                    mypdfpage.Add(ptsp);
                                                }
                                            }
                                            if (panno != "")
                                            {
                                                coltop = coltop + rowspace;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "PAN No                          :");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, panno);
                                                mypdfpage.Add(ptsp);
                                            }
                                            string joindate = "";
                                            string dofjoin = d2.GetFunction("select CONVERT(varchar(10),sm.join_date,103) as join_date from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sm.staff_code='" + staffcode + "'");
                                            if (dofjoin.Trim() != "" && dofjoin.Trim() != "0")
                                            {
                                                joindate = dofjoin;
                                            }
                                            else
                                            {
                                                joindate = "-";
                                            }
                                            coltop = coltop + rowspace;
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date of Join");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, joindate);
                                            mypdfpage.Add(ptsp);
                                            coltop = coltop + rowspace;
                                            string totdays = "";
                                            if (workdays == "1")
                                            {
                                                totdays = " Day";
                                            }
                                            else if (workdays != "0" && workdays != "1")
                                            {
                                                totdays = " Days";
                                            }
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "No.of Days");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                            mypdfpage.Add(ptsp);
                                            if (workdays != "0")
                                            {
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, workdays.ToString() + totdays);
                                            }
                                            else
                                            {
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "-");
                                            }
                                            mypdfpage.Add(ptsp);
                                            string absday = "";
                                            if (abscount == 1)
                                            {
                                                absday = " Day";
                                            }
                                            else if (abscount != 0 && abscount != 1)
                                            {
                                                absday = " Days";
                                            }
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Absent Days                   :");
                                            mypdfpage.Add(ptsp);
                                            if (abscount != 0)
                                            {
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(abscount) + absday);
                                                mypdfpage.Add(ptsp);
                                            }
                                            else
                                            {
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString("-"));
                                                mypdfpage.Add(ptsp);
                                            }
                                            string presdays = "";
                                            if (totalpreset == 1)
                                            {
                                                presdays = " Day";
                                            }
                                            else if (totalpreset != 0 && totalpreset != 1)
                                            {
                                                presdays = " Days";
                                            }
                                            coltop = coltop + rowspace;
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "No.of Days Present ");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                            mypdfpage.Add(ptsp);
                                            if (totalpreset != 0)
                                            {
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, totalpreset.ToString() + presdays);
                                            }
                                            else
                                            {
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "-");
                                            }
                                            mypdfpage.Add(ptsp);
                                            string lopdays = "";
                                            if (tot_lop == 1)
                                            {
                                                lopdays = "Day";
                                            }
                                            else
                                            {
                                                lopdays = "Days";
                                            }
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "LOP Days                       :");
                                            mypdfpage.Add(ptsp);
                                            if (tot_lop != 0)
                                            {
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, nooflop.ToString() + " " + lopdays);
                                                mypdfpage.Add(ptsp);
                                            }
                                            else
                                            {
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString("-"));
                                                mypdfpage.Add(ptsp);
                                            }
                                            coltop = coltop + rowspace + rowspace;
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Earnings");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 270, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Amount in Rs.");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Deductions");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 720, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Amount in Rs.");
                                            mypdfpage.Add(ptsp);
                                            int dedu = coltop + 10;
                                            int allo = coltop + 10;
                                            double payband = 0;
                                            double gradepay = 0;
                                            double basic_pay3 = 0;
                                            string allowance3 = "";
                                            string deduction3 = "";
                                            double lopd = 0;
                                            double bassicd = 0;
                                            double DblAllowLOP = 0;
                                            double DblAllowLOP1 = 0;
                                            double G_pay = 0;
                                            Double actbasic = 0;
                                            if (dvpay.Count > 0)
                                            {
                                                //payband = Convert.ToDouble(dvpay[0]["pay_band"].ToString());
                                                Double.TryParse(dvpay[0]["pay_band"].ToString(), out payband);
                                                //gradepay = Convert.ToDouble(dvpay[0]["grade_pay"].ToString());
                                                double.TryParse(dvpay[0]["grade_pay"].ToString(), out gradepay);
                                                //basic_pay3 = Convert.ToDouble(dvpay[0]["bsalary"].ToString());
                                                double.TryParse(dvpay[0]["bsalary"].ToString(), out basic_pay3);
                                                //G_pay = Convert.ToDouble(dvpay[0]["G_Pay"].ToString());
                                                double.TryParse(dvpay[0]["G_Pay"].ToString(), out G_pay);
                                                //actbasic = Convert.ToDouble(dvpay[0]["basic_alone"].ToString());
                                                double.TryParse(dvpay[0]["basic_alone"].ToString(), out actbasic);
                                                //lop = Convert.ToDouble(dvpay[0]["lop"].ToString());
                                                double.TryParse(dvpay[0]["lop"].ToString(), out lop);
                                                if (lop != 0)
                                                {
                                                    lopd = Convert.ToDouble(lop);
                                                }
                                                else
                                                {
                                                    lopd = 0;
                                                }
                                                bassicd = Convert.ToDouble(basic_pay3);
                                                if (bassicd != 0 || actbasic != 0)
                                                {
                                                    allo = allo + rowspace;
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "BASIC PAY");
                                                    mypdfpage.Add(ptsp);
                                                    bassicd = Math.Round(bassicd, 2, MidpointRounding.AwayFromZero);
                                                    string alowamount = "";
                                                    if (cblcolumnorder.Items[35].Selected == true)
                                                    {
                                                        alowamount = actbasic.ToString();
                                                    }
                                                    else
                                                    {
                                                        alowamount = bassicd.ToString();
                                                    }
                                                    alowamount = String.Format("{0:0.00}", alowamount);
                                                    double myValue = 0;
                                                    double.TryParse(alowamount, out myValue);
                                                    myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                    mypdfpage.Add(ptsp);
                                                }
                                                if (gradepay != 0 || G_pay != 0)
                                                {
                                                    allo = allo + rowspace;
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "GRADE PAY");
                                                    mypdfpage.Add(ptsp);
                                                    gradepay = Math.Round(gradepay, 2, MidpointRounding.AwayFromZero);
                                                    string alowamount = "";
                                                    if (cblcolumnorder.Items[35].Selected == true)
                                                    {
                                                        alowamount = G_pay.ToString();
                                                    }
                                                    else
                                                    {
                                                        alowamount = gradepay.ToString();
                                                    }
                                                    alowamount = String.Format("{0:0.00}", alowamount);
                                                    double myValue = 0;
                                                    double.TryParse(alowamount, out myValue);
                                                    myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                    mypdfpage.Add(ptsp);
                                                }
                                                if (payband != 0)
                                                {
                                                    allo = allo + rowspace;
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "PAY BAND");
                                                    mypdfpage.Add(ptsp);
                                                    payband = Math.Round(payband, 2, MidpointRounding.AwayFromZero);
                                                    string alowamount = payband.ToString();
                                                    alowamount = String.Format("{0:0.00}", alowamount);
                                                    double myValue = 0;
                                                    double.TryParse(alowamount, out myValue);
                                                    myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                    mypdfpage.Add(ptsp);
                                                }
                                                //Alter By Jeyaprakash on May 30th
                                                allowance3 = dvpay[0]["allowances"].ToString();
                                                string[] allowanmce_arr1;
                                                string alowancesplit;
                                                int exatval = 0;
                                                allowanmce_arr1 = allowance3.Split('\\');
                                                for (int i = 0; i < allowanmce_arr1.GetUpperBound(0); i++)
                                                {
                                                    exatval = allowanmce_arr1.GetUpperBound(0);
                                                    alowancesplit = allowanmce_arr1[i];
                                                    string[] allowanceda = alowancesplit.Split(';');
                                                    if (allowanceda.GetUpperBound(0) >= 3)
                                                    {
                                                        string da = allowanceda[0];
                                                        string mode = Convert.ToString(allowanceda[1]);
                                                        string daac = "";
                                                        string da3 = "";
                                                        if (allowanceda[2].Trim() != "")
                                                        {
                                                            string[] spval = allowanceda[2].Split('-');
                                                            if (spval.Length == 2)
                                                            {
                                                                if (mode.Trim().ToUpper() == "PERCENT")
                                                                {
                                                                    da3 = Convert.ToString(spval[1]);
                                                                }
                                                                else
                                                                {
                                                                    da3 = Convert.ToString(spval[0]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                da3 = Convert.ToString(allowanceda[3]);
                                                            }
                                                        }
                                                        daac = Convert.ToString(allowanceda[3]);
                                                        double da2 = 0;
                                                        Double.TryParse(daac, out da2);
                                                        double ds3 = 0;
                                                        double.TryParse(da3, out ds3);
                                                        ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                                        string DblAllowActVal = daac;
                                                        string DblAllowVal = allowanceda[3];
                                                        DblAllowLOP = (Convert.ToDouble(DblAllowActVal) - Convert.ToDouble(DblAllowVal));
                                                        DblAllowLOP = Math.Round(DblAllowLOP);
                                                        DblAllowLOP1 = DblAllowLOP1 + DblAllowLOP;
                                                        if (ds3 != 0)
                                                        {
                                                            allo = allo + rowspace;
                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left1, allo, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, da.ToString());
                                                            mypdfpage.Add(ptsp);
                                                            string alowamount = ds3.ToString();
                                                            alowamount = String.Format("{0:0.00}", alowamount);
                                                            double myValue = 0;
                                                            double.TryParse(alowamount, out myValue);
                                                            myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                            mypdfpage.Add(ptsp);
                                                        }
                                                        //i = allowanmce_arr1.GetUpperBound(0);
                                                    }
                                                }
                                                deduction3 = dvpay[0]["deductions"].ToString();
                                                string[] deduction_arr1;
                                                string deductionsplit;
                                                int exactval = 0;
                                                deduction_arr1 = deduction3.Split('\\');
                                                for (int i = 0; i < deduction_arr1.GetUpperBound(0); i++)//delsiref2
                                                {
                                                    exatval = deduction_arr1.GetUpperBound(0);
                                                    deductionsplit = deduction_arr1[i];
                                                    string[] deductionda = deductionsplit.Split(';');
                                                    if (deductionda.GetUpperBound(0) >= 3)
                                                    {
                                                        string da = deductionda[0];
                                                        string mode = Convert.ToString(deductionda[1]);
                                                        string daac = "";
                                                        string da3 = "";
                                                        //delsi 02.04.2018
                                                        string activeloan = "";

                                                        # region for calculate tenure
                                                        string selpaytotmon = "select LoanCode,PayTotMon from staffloandet where isactive = 1 and isclose = 0 and staff_code = '" + staffcode + "' and dedname = '" + da.ToString() + "' and LoanType='0'";
                                                        selpaytotmon += " select isactive from staffloandet where isactive = 1 and isclose = 0 and staff_code = '" + staffcode + "' and dedname = '" + da.ToString() + "' and LoanType='0'";
                                                        DataSet loanduration = new DataSet();
                                                        loanduration = d2.select_method_wo_parameter(selpaytotmon, "Text");
                                                        if (loanduration.Tables.Count > 0 && loanduration.Tables[1].Rows.Count > 0)
                                                        {
                                                            activeloan = loanduration.Tables[1].Rows[0]["isactive"].ToString();
                                                        }
                                                        if (loanduration.Tables.Count > 0 && loanduration.Tables[0].Rows.Count > 0 && activeloan == "True")
                                                        {
                                                            for (int lk = 0; lk < loanduration.Tables[0].Rows.Count; lk++)
                                                            {
                                                                selpaidcount = d2.GetFunction("select count(staff_code) as paidcount from staffloanpaydet where LoanCode='" + Convert.ToString(loanduration.Tables[0].Rows[lk]["LoanCode"]) + "' and Staff_Code='" + staffcode + "'");
                                                                loandet = Convert.ToString(loanduration.Tables[0].Rows[lk]["PayTotMon"]);
                                                                tenure = "(" + selpaidcount + "/" + loandet + ")";//poo 28.10.17

                                                            }
                                                        }
                                                        else
                                                        {
                                                            tenure = " ";
                                                        }
                                                        # endregion


                                                        if (deductionda[2].Trim() != "")
                                                        {
                                                            string[] spval = deductionda[2].Split('-');
                                                            if (spval.Length == 2)
                                                            {
                                                                if (mode.Trim().ToUpper() == "PERCENT")
                                                                {
                                                                    da3 = Convert.ToString(spval[1]);
                                                                }
                                                                else
                                                                {
                                                                    da3 = Convert.ToString(spval[0]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                da3 = Convert.ToString(deductionda[3]);
                                                            }
                                                        }
                                                        daac = Convert.ToString(deductionda[3]);
                                                        double da2 = 0;
                                                        Double.TryParse(daac, out da2);
                                                        double ds3 = 0;
                                                        double.TryParse(da3, out ds3);
                                                        ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                                        string DblAllowActVal = daac;
                                                        string DblAllowVal = deductionda[3];
                                                        DblAllowLOP = (Convert.ToDouble(DblAllowActVal) - Convert.ToDouble(DblAllowVal));
                                                        DblAllowLOP = Math.Round(DblAllowLOP);
                                                        DblAllowLOP1 = DblAllowLOP1 + DblAllowLOP;
                                                        if (ds3 != 0)
                                                        {
                                                            dedu = dedu + rowspace;
                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, left3, dedu, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, da.ToString() + tenure);
                                                            mypdfpage.Add(ptsp);
                                                            string alowamount = ds3.ToString();
                                                            alowamount = String.Format("{0:0.00}", alowamount);
                                                            double myValue = 0;
                                                            double.TryParse(alowamount, out myValue);
                                                            myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mydocument, 770, dedu, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                            mypdfpage.Add(ptsp);
                                                        }
                                                        //i = allowanmce_arr1.GetUpperBound(0);
                                                    }
                                                }
                                            }
                                            if (allo > dedu)
                                            {
                                                if (coltop < allo)
                                                {
                                                    coltop = allo;
                                                }
                                            }
                                            else
                                            {
                                                if (coltop < dedu)
                                                {
                                                    coltop = dedu;
                                                }
                                            }
                                            string lopamnt = "";
                                            if (cblcolumnorder.Items[35].Selected == true)
                                            {
                                                if (lop != 0)
                                                {
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mydocument, left3, coltop + 15, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "LOP AMOUNT");
                                                    mypdfpage.Add(ptsp);
                                                    lopamnt = String.Format("{0:0.00}", lop11);
                                                    double myValue = 0;
                                                    double.TryParse(lopamnt, out myValue);
                                                    myValue = Math.Round(myValue, 0, MidpointRounding.AwayFromZero);
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mydocument, 770, coltop + 15, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(myValue) + ".00");
                                                    mypdfpage.Add(ptsp);
                                                    coltop = coltop + 20;
                                                }
                                            }
                                            double grossamnt = 0;
                                            coltop = coltop + rowspace + rowspace;
                                            Double nesal3 = Convert.ToDouble(dvpay[0]["netsal"].ToString());
                                            nesal3 = Math.Round(nesal3, 2, MidpointRounding.AwayFromZero);
                                            //Double totall = Convert.ToDouble(dvpay[0]["NetAddAct"].ToString());
                                            Double totall = 0;
                                            if (cblcolumnorder.Items[35].Selected == false)
                                            {
                                                totall = Convert.ToDouble(dvpay[0]["NetAdd"].ToString());
                                            }
                                            else
                                            {
                                                totall = Convert.ToDouble(dvpay[0]["NetAddAct"].ToString());
                                            }
                                            totall = Math.Round(totall, 2, MidpointRounding.AwayFromZero);
                                            double netded = Convert.ToDouble(dvpay[0]["netded"].ToString());
                                            netded = Math.Round(netded, 2, MidpointRounding.AwayFromZero);
                                            Double lqw = Convert.ToDouble(lopamount);
                                            lqw = Math.Round(totall, 2, MidpointRounding.AwayFromZero);
                                            string finalamount = totall.ToString();
                                            finalamount = String.Format("{0:0.00}", finalamount);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "Gross Total");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 360, coltop - 25, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "___________");
                                            mypdfpage.Add(ptsp);
                                            double newValue = 0;
                                            double.TryParse(finalamount, out newValue);
                                            newValue = Math.Round(newValue, 0, MidpointRounding.AwayFromZero);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 320, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(newValue) + ".00");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 360, coltop + 10, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "___________");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 790, coltop - 25, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "___________");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Total Deductions");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 790, coltop + 10, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "___________");
                                            mypdfpage.Add(ptsp);
                                            Double lopamt = 0;
                                            if (cblcolumnorder.Items[35].Selected == true)
                                            {
                                                lopamt = Convert.ToDouble(lopamount);
                                            }
                                            lopamt = Math.Round(lopamt, 2, MidpointRounding.AwayFromZero);
                                            Double dedamount = Convert.ToDouble(lopamt) + Convert.ToDouble(netded);
                                            dedamount = Math.Round(dedamount, 2, MidpointRounding.AwayFromZero);
                                            finalamount = String.Format("{0:0.00}", finalamount);
                                            double isValue = 0;
                                            double.TryParse(Convert.ToString(dedamount), out isValue);
                                            isValue = Math.Round(isValue, 0, MidpointRounding.AwayFromZero);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 770, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, isValue.ToString() + ".00");
                                            mypdfpage.Add(ptsp);
                                            coltop = coltop + rowspace + rowspace;
                                            finalamount = String.Format("{0:0.00}", finalamount);
                                            int totnet = 0;
                                            Double finamnt = 0;
                                            Double.TryParse(finalamount, out finamnt);
                                            finamnt = Math.Round(finamnt, 0, MidpointRounding.AwayFromZero);
                                            dedamount = Math.Round(dedamount, 0, MidpointRounding.AwayFromZero);
                                            totnet = Convert.ToInt32(finamnt) - Convert.ToInt32(dedamount);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left1, coltop, 500, 50), System.Drawing.ContentAlignment.MiddleLeft, "Net Pay : ");
                                            mypdfpage.Add(ptsp);
                                            double hasValue = 0;
                                            double.TryParse(Convert.ToString(totnet), out hasValue);
                                            hasValue = Math.Round(hasValue, 0, MidpointRounding.AwayFromZero);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 50, coltop, 150, 50), System.Drawing.ContentAlignment.MiddleRight, "Rs . " + hasValue.ToString() + ".00");
                                            mypdfpage.Add(ptsp);
                                            string word = ConvertNumbertoWords(Convert.ToInt32(totnet)); // added by jairam 01-12-2014
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, 150, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleRight, " (Rupees " + word + " Only)");
                                            mypdfpage.Add(ptsp);
                                            string getfooter = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Footer Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
                                            if (getfooter.Trim() != "" && getfooter.Trim() != "0")
                                            {
                                                string[] spiltfooterdetails = getfooter.Split(',');
                                                tableforfooter = mydocument.NewTable(Fontbold, 3, spiltfooterdetails.Length, 3);
                                                tableforfooter.SetBorders(Color.Black, 1, BorderType.None);
                                                for (int i = 0; i <= spiltfooterdetails.GetUpperBound(0); i++)
                                                {
                                                    string collfooter = spiltfooterdetails[i].ToString();
                                                    tableforfooter.Cell(0, i).SetContent(collfooter);
                                                    tableforfooter.Cell(2, 0).SetContentAlignment(ContentAlignment.BottomRight);
                                                }
                                                Gios.Pdf.PdfTablePage pdftabpage = tableforfooter.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 0, coltop + 90, mydocument.PageWidth, 50));
                                                mypdfpage.Add(pdftabpage);
                                            }
                                            if (rec % 2 != 0)
                                            {
                                                PdfArea tete = new PdfArea(mydocument, 10, starttop, 830, coltop + 30);
                                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                                mypdfpage.Add(pr1);
                                                if (totlastaff <= rec)
                                                {
                                                    mypdfpage.SaveToDocument();
                                                }
                                            }
                                            else
                                            {
                                                PdfArea tete = new PdfArea(mydocument, 10, starttop, 830, coltop - gettop + 50);
                                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                                mypdfpage.Add(pr1);
                                                mypdfpage.SaveToDocument();
                                            }
                                            gettop = coltop + 100;
                                            genpdfcount++;
                                            lblnorec.Visible = false;
                                            lblnorec.Text = "";
                                            string appPath = HttpContext.Current.Server.MapPath("~");
                                            string strquery = d2.GetFunction("select Coll_acronymn from collinfo where college_code='" + collegecode1 + "'");
                                            string details = staffname + staffcode;
                                            details = details.Replace(' ', '_');//23.09.17
                                            if (appPath != "")
                                            {
                                                string szPath = appPath + "/Report/";
                                                string szFile = details + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                                                mydocument.SaveToFile(szPath + szFile);
                                                double strquery1 = 0;
                                                if (staffcode.Trim() != "" && staffcode.Trim() != null)
                                                {
                                                    dssmssalary.Tables[2].DefaultView.RowFilter = " PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and PayMonth='" + ddl_mon.SelectedValue.ToString() + "' and staff_code='" + staffcode + "'";
                                                    DataView dvnetsal = dssmssalary.Tables[2].DefaultView;
                                                    if (dvnetsal.Count > 0)
                                                    {
                                                        strquery1 = Convert.ToDouble(dvnetsal[0]["netsal"].ToString());
                                                    }
                                                    txtsubject.Text = "Your Net Salary of Rs." + strquery1 + " for the month of " + ddl_mon.SelectedItem.Text + " " + year + " has been credited to your S.B.account.-" + strquery + "";
                                                    string email = "";
                                                    dssmssalary.Tables[6].DefaultView.RowFilter = " staff_code='" + staffcode.ToString() + "'";
                                                    DataView dvemail = dssmssalary.Tables[6].DefaultView;
                                                    if (dvemail.Count > 0)
                                                    {
                                                        email = dvemail[0]["email"].ToString();
                                                        staffname = dvemail[0]["staff_name"].ToString();
                                                    }
                                                    string query = txtsubject.Text;
                                                    if (email.Trim() != "" && email != null)
                                                    {
                                                        sendemail(email, query, details);
                                                    }
                                                }
                                                // Response.ClearHeaders();
                                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                                //Response.ContentType = "application/pdf";
                                                //Response.WriteFile(szPath + szFile);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                #endregion
                        //*****************End****************************//
                    }
                }
                if (genpdfcount > 0)
                {
                    lblnorec.Visible = false;
                    lblnorec.Text = "";
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Select Staff and then Proceed!";
                }
                fpsalary.SaveChanges();
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Staff and then Proceed!";
            }
        }
        catch (Exception ex)//delsiref
        {
            //d2.sendErrorMail(ex, collegecode1, "Individual_SalaryReport.aspx");
            d2.sendErrorMail(ex, collegecode1, "Individual_SalaryReport.aspx");
            //imgdiv2.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }
    protected void btnsalcer_click(object sender, EventArgs e)
    {
        loadsalcert();
    }
    public void loadsalcert()//delsi
    {
        try
        {
            lblnorec.Visible = false;
            Printcontrol.Visible = false;
            lblsmserror.Visible = false;
            fpsalary.SaveChanges();
            string collname = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string pincode = "";
            string address = "";
            Font Fontbold = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 20, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 13, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfTable tableforfooter;
            string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + collegecode1 + "'";
            str = str + " ;select * from incentives_master where college_code='" + collegecode1 + "'";
            str = str + " ;select distinct CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "'and PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and college_Code=" + collegecode1 + "";
            DataSet ds = d2.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                collname = ds.Tables[0].Rows[0]["collname"].ToString();
                address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                if (address1.Trim() != "" && address1 != null)
                {
                    address = address1;
                }
                if (address2.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address2;
                    }
                    else
                    {
                        address = address2;
                    }
                }
                if (address3.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address3;
                    }
                    else
                    {
                        address = address3;
                    }
                }
                if (pincode.Trim() != "" && pincode != null)
                {
                    if (address != "")
                    {
                        address = address + '-' + pincode;
                    }
                    else
                    {
                        address = pincode;
                    }
                }
            }
            string allowmaster = "";
            string deductmaster = "";
            if (ds.Tables[1].Rows.Count > 0)
            {
                allowmaster = ds.Tables[1].Rows[0]["allowances"].ToString();
                deductmaster = ds.Tables[1].Rows[0]["deductions"].ToString();
            }
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] spdet = deductmaster.Split(';');
            for (int d = 0; d <= spdet.GetUpperBound(0); d++)
            {
                string[] spdedet = spdet[d].Split('\\');
                if (spdedet.GetUpperBound(0) >= 1)
                {
                    string val = spdedet[0];
                    string val1 = spdedet[1];
                    if (!dict.ContainsKey(val))
                    {
                        dict.Add(val, val1);
                    }
                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                string date3 = Convert.ToString(ds.Tables[2].Rows[0]["from_date"]);
                string datefrom3;
                int monthname1;
                string monyear;
                string monthname2 = "";
                int monthnamenum;
                string yearto = "";
                string monyearto = "";
                string[] split = date3.Split(new Char[] { '/' });
                int totlastaff = 0;
                int left1 = 90;
                int incrre = 1;
                int rowcount = 0;
                if (cblcolumnorder.Items[53].Selected == true && fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL")
                {
                    rowcount = fpsalary.Sheets[0].RowCount - 2;
                }
                if (fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Text == "TOTAL" && cblcolumnorder.Items[53].Selected == false)
                {
                    rowcount = fpsalary.Sheets[0].RowCount - 1;
                }
                for (int res = incrre; res < rowcount; res = res + incrre)
                {
                    for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                    {
                        if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT" && fpsalary.Sheets[0].Rows[res].Visible == true)
                        {
                            string text = fpsalary.Sheets[0].Cells[res, lst].Text.ToString();
                            if (text != "" && text != null)
                            {
                                int isval = Convert.ToInt32(fpsalary.Sheets[0].Cells[res, lst].Value);
                                if (isval == 1)
                                {
                                    totlastaff++;
                                }
                            }
                        }
                    }
                }
                datefrom3 = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                string date4 = Convert.ToString(ds.Tables[2].Rows[0]["to_date"]);
                int year3 = Convert.ToInt16(split[2].ToString());
                string[] split1 = date4.Split(new Char[] { '/' });
                string dateto4 = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                monthname2 = split1[0].ToString();
                monthnamenum = Convert.ToInt32(monthname2.ToString());
                yearto = split1[2].ToString();
                string mnmae = split[1].ToString();
                monthname1 = Convert.ToInt16(mnmae);
                monthname2 = split1[1].ToString();
                monthnamenum = Convert.ToInt16(monthname2);
                string year = split[2].ToString();
                monyear = monthname1.ToString() + "/" + year;
                monyearto = monthnamenum.ToString() + "/" + yearto;
                Boolean getvalflag = false;
                string lopdates = "";
                string[] leavetype = new string[50];
                sql = "select shortname from leave_category where college_code=" + Session["collegecode"] + "";
                DataSet dsleave = d2.select_method_wo_parameter(sql, "Text");
                int lev = 0;
                for (int le = 0; le < dsleave.Tables[0].Rows.Count; le++)
                {
                    lev++;
                    string levatype = dsleave.Tables[0].Rows[le]["shortname"].ToString().Trim().ToLower();
                    leavetype[lev] = levatype;
                }
                sql = "select m.*,s.staff_name,s.pfnumber,s.ESI_No,s.bankaccount,s.pangirnumber ,st.payscale as pay_scalenew , st.allowances as actallowance,h.dept_name as deptname,d.desig_name as designame from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.latestrec = 1 and m.college_code=s.college_code and m.college_code =h.college_code and m.college_code=d.collegeCode and s.college_code='" + collegecode1 + "' and m.PayYear='" + ddl_year.SelectedValue.ToString() + "' and m.PayMonth='" + ddl_mon.SelectedValue.ToString() + "' ";
                sql = sql + " ; select convert(nvarchar(15),sa.date_of_birth,103) as dob,sm.staff_code,convert(nvarchar(15),sm.retr_date ,103) as retir,sm.staff_name,CONVERT(varchar(10),sm.join_date,103) as join_date from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no ";
                DataSet dspay = d2.select_method_wo_parameter(sql, "text");
                DataTable dtpay = dspay.Tables[0];
                DataView dvpay = new DataView();
                DataView dvapp = new DataView();
                int gettop = 0;
                int rec = 0;
                Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

                string getlink = d2.GetFunction("select linkvalue from inssettings where linkname='Calculate LOP from Attendance' and college_code='" + collegecode1 + "'");
                sql1 = "select * from staff_attnd where mon_year between '" + monyear + "' and '" + monyearto + "' ";
                DataSet dsstaffattendance = d2.select_method_wo_parameter(sql1, "text");
                DataView dvstaffattendance = new DataView();

                int genpdfcount = 0;
                for (int res = 1; res < rowcount; res++)
                {
                    if (fpsalary.Sheets[0].Rows[res].Visible == true)
                    {
                        for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                        {
                            if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT")
                            {
                                string text = fpsalary.Sheets[0].Cells[res, lst].Text.ToString();
                                if (text != "" && text != null)
                                {
                                    bool isval = Convert.ToBoolean(fpsalary.Sheets[0].Cells[res, lst].Value);
                                    if (isval == true)
                                    {
                                        rec++;
                                        getvalflag = true;
                                        gettop = 15;
                                        int coltop = gettop;
                                        int incval = 220;
                                        mypdfpage = mydocument.NewPage();
                                        string staffcode = "";
                                        string designation = "";
                                        string deptname = "";
                                        Double basicpay = 0.0;
                                        Double gradepay = 0.0;
                                        Double lopamnt = 0.0;
                                        int starttop = gettop;
                                        // int coltop = gettop;
                                        int rowspace = 20;
                                        try
                                        {
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                            {
                                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                                mypdfpage.Add(LogoImage, 25, coltop + 10, 400);
                                            }
                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                            {
                                                string leftlogo = "Left_Logo(" + collegecode1 + ")";
                                                MemoryStream memoryStream = new MemoryStream();
                                                string sellogo = "select logo1 from collinfo where college_code='" + collegecode1 + "'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(sellogo, "Text");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                                    memoryStream.Write(file, 0, file.Length);
                                                    if (file.Length > 0)
                                                    {
                                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"));
                                                        mypdfpage.Add(LogoImage, 25, coltop + 10, 400);
                                                    }
                                                    memoryStream.Dispose();
                                                    memoryStream.Close();
                                                }
                                            }
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                            {
                                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                                mypdfpage.Add(LogoImage, 750, coltop + 10, 350);
                                            }
                                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                            {
                                                string rightlogo = "Right_Logo(" + collegecode1 + ")";
                                                MemoryStream memoryStream = new MemoryStream();
                                                string sellogo = "select logo2 from collinfo where college_code='" + collegecode1 + "'";//nedd to change
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(sellogo, "Text");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                                    memoryStream.Write(file, 0, file.Length);
                                                    if (file.Length > 0)
                                                    {
                                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"));
                                                        mypdfpage.Add(LogoImage, 750, coltop + 10, 350);
                                                    }
                                                    memoryStream.Dispose();
                                                    memoryStream.Close();
                                                }
                                            }
                                        }
                                        catch { }

                                        PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 0, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                        mypdfpage.Add(ptc);
                                        coltop = coltop + rowspace;
                                        PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 0, coltop + 10, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                        mypdfpage.Add(pts);
                                        coltop = coltop + rowspace;

                                        for (int chklst = 0; chklst < lstcolorder.Items.Count; chklst++)
                                        {
                                            if (lstcolorder.Items[chklst].Text == "STAFF CODE")
                                            {
                                                staffcode = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DESIGN")
                                            {
                                                designation = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DEPT")
                                            {
                                                deptname = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "LOP DATES")
                                            {
                                                lopdates = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                        }
                                        string staffname = "";
                                        if (staffcode.Trim() == "")
                                        {
                                            lblnorec.Visible = true;
                                            lblnorec.Text = "Please select Staff Code!";
                                            return;
                                        }

                                        Double totdection = 0;
                                        dtpay.DefaultView.RowFilter = " staff_code='" + staffcode + "'";
                                        dvpay = dtpay.DefaultView;


                                        if (dvpay.Count > 0)
                                        {
                                            staffname = dvpay[0]["staff_name"].ToString();
                                            designation = dvpay[0]["designame"].ToString();
                                            deptname = dvpay[0]["deptname"].ToString();
                                            Double.TryParse(Convert.ToString(dvpay[0]["Actual_Basic"]), out basicpay);
                                            Double.TryParse(Convert.ToString(dvpay[0]["grade_pay"]), out gradepay);
                                            Double.TryParse(Convert.ToString(dvpay[0]["Tot_lop"]), out lopamnt);
                                            //int starttop = gettop;

                                            string setyear = dvpay[0]["PayYear"].ToString();
                                            coltop = coltop + rowspace;
                                            PdfTextArea ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 0, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, "SALARY CERTIFICATE ");
                                            mypdfpage.Add(ptsp);
                                            coltop = coltop + 60;
                                            coltop = coltop + rowspace;
                                            coltop = coltop + rowspace + rowspace;
                                            string month = ddl_mon.SelectedItem.Text;
                                            string submonth = month.Substring(0, 3);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                             new PdfArea(mydocument, left1 + 125, 130, 650, 50), System.Drawing.ContentAlignment.TopLeft, "Following are the pay particulars of " + staffname + "," + designation + ",");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, left1 + 125, 150, 650, 50), System.Drawing.ContentAlignment.TopLeft, "Department of " + deptname + " of our college for the month of " + submonth + " " + yearto + ":");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "EARNINGS");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 350, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Actual Amount");
                                            mypdfpage.Add(ptsp);   //----- whether include
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 630, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "AMOUNT");
                                            mypdfpage.Add(ptsp);
                                            if (basicpay != 0)
                                            {
                                                coltop = coltop + 40;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Basic Pay");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                               new PdfArea(mydocument, 390, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, basicpay.ToString());
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mydocument, 640, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, basicpay.ToString());
                                                mypdfpage.Add(ptsp);
                                            }
                                            if (gradepay != 0)
                                            {
                                                coltop = coltop + 20;
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Grade Pay");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                               new PdfArea(mydocument, 390, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, gradepay.ToString());
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mydocument, 640, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, gradepay.ToString());
                                                mypdfpage.Add(ptsp);
                                            }

                                            int allo = coltop;
                                            int dedu = 0;
                                            if (dvpay.Count > 0)
                                            {
                                                double payband = 0;
                                                string allowance3 = "";
                                                string deduction3 = "";
                                                payband = Convert.ToDouble(dvpay[0]["pay_band"].ToString());
                                                Dictionary<string, string> dicactaloo = new Dictionary<string, string>();
                                                string[] spallow = allowmaster.Split(';');
                                                string da = "";
                                                string daac = "";
                                                for (int alo = 0; alo <= spallow.GetUpperBound(0); alo++)
                                                {
                                                    string[] splalot = spallow[alo].Split('\\');
                                                    if (splalot.GetUpperBound(0) >= 2)
                                                    {
                                                        string orallo = splalot[0].ToString();
                                                        int r = 0;
                                                        allowance3 = dvpay[0]["allowances"].ToString();
                                                        int c = 0;
                                                        string[] allowanmce_arr1;
                                                        string alowancesplit;
                                                        int exatval = 0;
                                                        allowanmce_arr1 = allowance3.Split('\\');
                                                        for (int i = c; i < allowanmce_arr1.GetUpperBound(0); i++)
                                                        {
                                                            exatval = allowanmce_arr1.GetUpperBound(0);
                                                            alowancesplit = allowanmce_arr1[i];
                                                            string[] allowanceda = alowancesplit.Split(';');
                                                            if (allowanceda.GetUpperBound(0) >= 3)
                                                            {
                                                                da = allowanceda[0];
                                                                string allow = "";
                                                                daac = "";
                                                                string mode = Convert.ToString(allowanceda[1]);
                                                                if (allowanceda[2].Trim() != "")
                                                                {
                                                                    string[] spval = allowanceda[2].Split('-');
                                                                    if (spval.Length == 2)
                                                                    {
                                                                        if (mode.Trim().ToUpper() == "PERCENT")
                                                                        {
                                                                            allow = Convert.ToString(spval[1]);
                                                                        }
                                                                        else
                                                                        {
                                                                            allow = Convert.ToString(spval[0]);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        allow = Convert.ToString(allowanceda[3]);
                                                                    }
                                                                }
                                                                daac = Convert.ToString(allowanceda[3]);
                                                                if (orallo.Trim().ToLower() == da.Trim().ToLower())
                                                                {
                                                                    string alowamount = "";
                                                                    if (spll_alll_tag_arry.GetUpperBound(0) >= r)
                                                                    {
                                                                        if (spll_alll_tag_arry[r] != da)
                                                                        {
                                                                            allo = allo + 20;
                                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mydocument, left1, allo, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, splalot[0].ToString());
                                                                            mypdfpage.Add(ptsp);
                                                                            if (mode.Trim().ToUpper() == "AMOUNT")
                                                                            {
                                                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                               new PdfArea(mydocument, 390, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, daac);
                                                                                mypdfpage.Add(ptsp);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (dicactaloo.ContainsKey(da.ToString().Trim().ToLower()))
                                                                                {
                                                                                    daac = dicactaloo[da.ToString().Trim().ToLower()];
                                                                                }
                                                                                ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                          new PdfArea(mydocument, 390, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, daac);
                                                                                mypdfpage.Add(ptsp);
                                                                            }
                                                                            if (!alowamount.Contains('.'))
                                                                            {
                                                                                alowamount = alowamount + ".00";
                                                                            }
                                                                            else
                                                                            {
                                                                                string[] sp = alowamount.Split('.');
                                                                                if (sp[1].Length < 2)
                                                                                {
                                                                                    alowamount = alowamount + "0";
                                                                                }
                                                                            }
                                                                            ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mydocument, 640, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, allow);
                                                                            mypdfpage.Add(ptsp);
                                                                        }
                                                                    }
                                                                    i = allowanmce_arr1.GetUpperBound(0);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }











                                                ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, left1, allo + 80, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "DEDUCTIONS");
                                                mypdfpage.Add(ptsp);
                                                ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 630, allo + 80, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "AMOUNT");
                                                mypdfpage.Add(ptsp);
                                                dedu = allo + 100;
                                                if (lopamnt != 0)
                                                {
                                                    dedu = dedu + 20;
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mydocument, left1, dedu, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "LOP Amount");
                                                    mypdfpage.Add(ptsp);
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                           new PdfArea(mydocument, 640, dedu, 50, 50), System.Drawing.ContentAlignment.MiddleRight, lopamnt.ToString());
                                                    mypdfpage.Add(ptsp);
                                                }
                                                foreach (var kvp in dict)
                                                {
                                                    string setval = kvp.Key.ToString();
                                                    string setval1 = kvp.Value.ToString();
                                                    deduction3 = dvpay[0]["deductions"].ToString();
                                                    string[] deduction_arr1;
                                                    string deductionsplit1;
                                                    deduction_arr1 = deduction3.Split('\\');
                                                    for (int sp = 0; sp < deduction_arr1.GetUpperBound(0); sp++)
                                                    {
                                                        deductionsplit1 = deduction_arr1[sp];
                                                        string[] deductionda;
                                                        deductionda = deductionsplit1.Split(';');
                                                        if (deductionda.GetUpperBound(0) >= 3)
                                                        {
                                                            string de = deductionda[0];
                                                            string de3 = "";
                                                            string mode = Convert.ToString(deductionda[1]);

                                                            if (deductionda[2].Trim() != "")
                                                            {
                                                                string[] spval = deductionda[2].Split('-');
                                                                if (spval.Length == 2)
                                                                {
                                                                    if (mode.Trim().ToUpper() == "PERCENT")
                                                                    {
                                                                        de3 = Convert.ToString(spval[1]);
                                                                    }
                                                                    else
                                                                    {
                                                                        de3 = Convert.ToString(spval[0]);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    de3 = Convert.ToString(deductionda[3]);
                                                                }
                                                            }
                                                            double de3d = Convert.ToDouble(de3);
                                                            de3d = Math.Round(de3d, 2, MidpointRounding.AwayFromZero);
                                                            if (setval == de)
                                                            {
                                                                if (de3d != 0)
                                                                {
                                                                    dedu = dedu + 20;
                                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, left1, dedu, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, setval.ToString());
                                                                    mypdfpage.Add(ptsp);
                                                                    totdection = totdection + de3d;
                                                                    string deamount = de3d.ToString();
                                                                    if (!deamount.Contains('.'))
                                                                    {
                                                                        deamount = deamount + ".00";
                                                                    }
                                                                    else
                                                                    {
                                                                        string[] spd = deamount.Split('.');
                                                                        if (spd[1].Length < 2)
                                                                        {
                                                                            deamount = deamount + "0";
                                                                        }
                                                                    }
                                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mydocument, 640, dedu, 50, 50), System.Drawing.ContentAlignment.MiddleRight, deamount);
                                                                    mypdfpage.Add(ptsp);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            Double nesal3 = Convert.ToDouble(dvpay[0]["netsal"].ToString());
                                            nesal3 = Math.Round(nesal3, 2, MidpointRounding.AwayFromZero);
                                            Double totall = Convert.ToDouble(dvpay[0]["netaddact"].ToString());
                                            totall = Math.Round(totall, 2, MidpointRounding.AwayFromZero);
                                            double netded = Convert.ToDouble(dvpay[0]["netded"].ToString());
                                            netded = netded + lopamnt;
                                            netded = Math.Round(netded, 2, MidpointRounding.AwayFromZero);
                                            string finalamount = totall.ToString();
                                            if (!finalamount.Contains('.'))
                                            {
                                                finalamount = finalamount + ".00";
                                            }
                                            else
                                            {
                                                string[] sp = finalamount.Split('.');
                                                if (sp[1].Length < 2)
                                                {
                                                    finalamount = finalamount + "0";
                                                }
                                            }
                                            finalamount = netded.ToString();
                                            if (!finalamount.Contains('.'))
                                            {
                                                finalamount = finalamount + ".00";
                                            }
                                            else
                                            {
                                                string[] sp = finalamount.Split('.');
                                                if (sp[1].Length < 2)
                                                {
                                                    finalamount = finalamount + "0";
                                                }
                                            }
                                            finalamount = nesal3.ToString();
                                            if (!finalamount.Contains('.'))
                                            {
                                                finalamount = finalamount + ".00";
                                            }
                                            else
                                            {
                                                string[] sp = finalamount.Split('.');
                                                if (sp[1].Length < 2)
                                                {
                                                    finalamount = finalamount + "0";
                                                }
                                            }
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, left1, allo + 40, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "TOTAL");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 640, allo + 25, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "------------");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 640, allo + 40, 50, 50), System.Drawing.ContentAlignment.MiddleRight, totall.ToString());
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 640, allo + 50, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "------------");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 640, dedu + 25, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "------------");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, dedu + 40, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "TOTAL");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                              new PdfArea(mydocument, 640, dedu + 50, 50, 50), System.Drawing.ContentAlignment.MiddleRight, "------------");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 640, dedu + 40, 50, 50), System.Drawing.ContentAlignment.MiddleRight, netded.ToString());
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, left1, dedu + 80, 500, 50), System.Drawing.ContentAlignment.MiddleLeft, "NET : ");
                                            mypdfpage.Add(ptsp);
                                            string word = ConvertNumbertoWords(Convert.ToInt32(Math.Round(nesal3)));
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 640, dedu + 80, 50, 50), System.Drawing.ContentAlignment.MiddleRight, finalamount);  //netded.ToString()
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, left1, dedu + 120, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, " (Rupees " + word + " Only)");  //netded.ToString()
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left1 + 85, 130, 650, 50), System.Drawing.ContentAlignment.TopRight, DateTime.Now.ToString("dd/MMM/yyyy"));
                                            mypdfpage.Add(ptsp);

                                            //delsi1704

                                            string footerdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='printpdfCertificate_footer' and college_code='" + Convert.ToString(collegecode) + "' and user_Code='" + Convert.ToString(Session["usercode"]) + "'");

                                            if (footerdetails == "0")
                                            {
                                                footerdetails = "";

                                            }


                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 50, 750, 800, 50), System.Drawing.ContentAlignment.BottomLeft, footerdetails);
                                            mypdfpage.Add(ptsp);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 240, 900, 500, 50), System.Drawing.ContentAlignment.MiddleRight, "PRINCIPAL");
                                            mypdfpage.Add(ptsp);
                                            mypdfpage.SaveToDocument();
                                            gettop = coltop + 100;
                                        }
                                    }
                                }
                            }
                        }


                        //ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                                    new PdfArea(mydocument, left1 + 125, 130, 650, 50), System.Drawing.ContentAlignment.TopLeft, "Following are the pay particulars of " + staffname + "," + designation + ",");
                        //mypdfpage.Add(ptsp);

                        if (getvalflag == true)
                        {
                            genpdfcount++;
                            lblnorec.Visible = false;
                            lblnorec.Text = "";
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";
                                string szFile = DateTime.Now.ToString("ddMMyyyyhhmmsstt") + "Salary_Certificate.pdf";
                                mydocument.SaveToFile(szPath + szFile);
                                Response.ClearHeaders();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(szPath + szFile);
                            }
                        }
                    }
                }
                if (genpdfcount > 0)
                {
                    lblnorec.Visible = false;
                    lblnorec.Text = "";
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Select Staff and then Proceed!";
                }
                fpsalary.SaveChanges();
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Staff and then Proceed!";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Individual_SalaryReport.aspx");
        }
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 10000000) > 0)
        {
            if (ConvertNumbertoWords(number / 10000000).Trim().ToUpper() == "ONE")
                words += ConvertNumbertoWords(number / 10000000) + " Crore ";
            else
                words += ConvertNumbertoWords(number / 10000000) + " Crores ";
            number %= 10000000;
        }
        if ((number / 100000) > 0)
        {
            if (ConvertNumbertoWords(number / 100000).Trim().ToUpper() == "ONE")
                words += ConvertNumbertoWords(number / 100000) + " Lakh ";
            else
                words += ConvertNumbertoWords(number / 100000) + " Lakhs ";
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
    public int getmaxdays(int mno, int year)
    {
        int maxdays = 0;
        if ((mno == 2) && (year % 4 == 0))
        {
            maxdays = 29;
            return maxdays;
        }
        else if ((mno == 1) || (mno == 3) || (mno == 5) || (mno == 7) || (mno == 8) || (mno == 10) || (mno == 12))
        {
            maxdays = 31;
            return maxdays;
        }
        else if ((mno == 4) || (mno == 6) || (mno == 9) || (mno == 11))
        {
            maxdays = 30;
            return maxdays;
        }
        else if ((mno == 2) || (year % 4) != 0)
        {
            maxdays = 28;
            return maxdays;
        }
        return maxdays;
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
    protected void ddladdreason_Change(object sender, EventArgs e)
    {
        try
        {
            string linkname = "";
            bool entryflag = false;
            cblcolumnorder.ClearSelection();
            if (ddladdreason.SelectedIndex != 0)
                linkname = Convert.ToString(ddladdreason.SelectedItem.Text);
            else
                linkname = "Individual Staff Salary";
            int count = 0;
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selcol, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string value = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
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
                        }
                    }
                    if (count == cblcolumnorder.Items.Count)
                        CheckBox_column.Checked = true;
                    else
                        CheckBox_column.Checked = false;
                }
            }
        }
        catch { }
    }
    public void loadcolumns(object sender, EventArgs e)
    {
        try
        {
            string linkname = "";
            if (ddladdreason.SelectedIndex == 0)
                linkname = "Individual Staff Salary";
            else
                linkname = Convert.ToString(ddladdreason.SelectedItem.Text);
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            lstcolorder.Items.Clear();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            int parttimecount = (cb_partimestaff.Checked == true ? cblcolumnorder.Items.Count : cblcolumnorder.Items.Count - 3);
            if (columncount() == true)
            {
                if (cblcolumnorder.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < parttimecount; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                            if (columnvalue == "")
                            {
                                columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                                lstcolorder.Items.Add(new ListItem(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value));
                            }
                            else
                            {
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                                lstcolorder.Items.Add(new ListItem(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value));
                            }
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0 && dscol.Tables[0].Rows.Count > 0)
            {
                colord.Clear();
                lstcolorder.Items.Clear();
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
                            lstcolorder.Items.Add(new ListItem(cblcolumnorder.Items[Convert.ToInt32(valuesplit[k])].Text, cblcolumnorder.Items[Convert.ToInt32(valuesplit[k])].Value));
                        }
                    }
                }
            }
            else
            {
                colord.Clear();
                for (int i = 0; i < parttimecount; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                    if (columnvalue == "")
                    {
                        columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                    }
                    else
                    {
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                    }
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
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
                                for (int k = 0; k < parttimecount; k++)
                                {
                                    if (val == cblcolumnorder.Items[k].Value)
                                    {
                                        cblcolumnorder.Items[k].Selected = true;
                                        count++;
                                    }
                                }
                            }
                            if (count == cblcolumnorder.Items.Count)
                            {
                                CheckBox_column.Checked = true;
                            }
                            else
                            {
                                CheckBox_column.Checked = false;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Individual_SalaryReport.aspx");
        }
    }
    protected void chk_pfrepo_checked(object sender, EventArgs e)
    {
        if (chk_pfrepo.Checked)
        {
            lblbasic.Visible = true;
            txtbasic.Visible = true;
        }
        else
        {
            lblbasic.Visible = false;
            txtbasic.Visible = false;
        }
    }
    protected void txtstaff_txtchanged(object sender, EventArgs e)
    {
        txtstaffname.Text = "";
    }
    protected void txtname_txtchanged(object sender, EventArgs e)
    {
        txtstaffcode.Text = "";
    }
    protected void ddl_mon_Change(object sender, EventArgs e)
    {
        bindyear();
    }
    protected void btnaddreason_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtaddpage.Text != "")
            {
                string sql = "if exists ( select * from TextValTable where TextVal ='" + txtaddpage.Text + "' and TextCriteria ='PgNme' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txtaddpage.Text + "' where TextVal ='" + txtaddpage.Text + "' and TextCriteria ='PgNme' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txtaddpage.Text + "','PgNme','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved sucessfully";
                    txtaddpage.Text = "";
                    popdiv.Visible = false;
                }
                bindreason();
            }
            else
            {
                popdiv.Visible = true;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Enter the description";
            }
        }
        catch (Exception ex) { }
    }
    protected void btnexitreason_Click(object sender, EventArgs e)
    {
        popdiv.Visible = false;
    }
    protected void btnplus_click(object sender, EventArgs e)
    {
        popdiv.Visible = true;
    }
    protected void btnminus_click(object sender, EventArgs e)
    {
        popconfirm.Visible = true;
        lblalertconfirm.Visible = true;
        lblalertconfirm.Text = "Do you want to delete this Record?";
    }
    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddladdreason.SelectedIndex != 0)
            {
                string sql = "delete from TextValTable where TextCode='" + ddladdreason.SelectedItem.Value.ToString() + "' and TextCriteria='PgNme' and college_code='" + collegecode1 + "' ";
                sql = sql + " delete from New_InsSettings where LinkName='" + ddladdreason.SelectedItem.Text.ToString() + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Deleted Successfully";
                    popconfirm.Visible = false;
                    lblalertconfirm.Visible = false;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record Selected";
                    popconfirm.Visible = false;
                    lblalertconfirm.Visible = false;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Selected";
                popconfirm.Visible = false;
                lblalertconfirm.Visible = false;
            }
            bindreason();
        }
        catch { }
    }
    protected void btnno_Click(object sender, EventArgs e)
    {
        popconfirm.Visible = false;
        lblalertconfirm.Visible = false;
    }
    public void bindreason()
    {
        try
        {
            ddladdreason.Items.Clear();
            ds.Clear();
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='PgNme' and college_code ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddladdreason.DataSource = ds;
                ddladdreason.DataTextField = "TextVal";
                ddladdreason.DataValueField = "TextCode";
                ddladdreason.DataBind();
                ddladdreason.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddladdreason.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch { }
    }
    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            #region for columnheader
            fpsalary.SaveChanges();
            Printcontrol.Visible = false;
            lblsmserror.Visible = false;
            chkShowPF.Checked = false;
            ColumnAdjWid.Clear();
            int countded = 0;
            int gratuitucolcount = 0;
            int yrofexpcount = 0;
            int spreadcount = 0;
            int levcount = 0;
            getcol.Clear();
            for (int i = 0; i < cbl_allow.Items.Count; i++)
            {
                if (cbl_allow.Items[i].Selected == true)
                {
                    ccountforallow++;
                }
            }
            for (int i = 0; i < cbl_ded.Items.Count; i++)
            {
                if (cbl_ded.Items[i].Selected == true)
                {
                    countded++;
                }
            }
            for (int i = 0; i < chklstlev.Items.Count; i++)
            {
                if (chklstlev.Items[i].Selected == true)
                {
                    levcount++;
                }
            }
            if (cb_all.Checked == true && ccountforallow == 0)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Allowance!";
                div1.Visible = false;
                rprint.Visible = false;
                chkShowPF.Visible = false;
                return;
            }
            if (cb_deduct.Checked == true && countded == 0)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Deduction!";
                div1.Visible = false;
                rprint.Visible = false;
                chkShowPF.Visible = false;
                return;
            }
            if (cbleavecat.Checked == true && levcount == 0)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Leave!";
                div1.Visible = false;
                rprint.Visible = false;
                chkShowPF.Visible = false;
                return;
            }
            string namewithbaslop = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Include Basic With LOP' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'");
            loadcolumns(sender, e);
            cb_parttimestaff_checkedchange(sender, e);
            getcol.Add("1", "staff_code");
            getcol.Add("2", "staff_name");
            getcol.Add("3", "designame");
            getcol.Add("4", "deptname");
            getcol.Add("5", "dept_acronym");
            getcol.Add("6", "desig_acronym");
            getcol.Add("7", "dateofbirth");
            getcol.Add("8", "appointdate");
            getcol.Add("9", "joindate");
            getcol.Add("10", "resigndate");
            getcol.Add("13", "category_name");
            getcol.Add("14", "stftype");
            getcol.Add("15", "PayMode");
            getcol.Add("16", "bank_name");
            getcol.Add("17", "branch_name");
            getcol.Add("18", "BankAccType");
            getcol.Add("19", "ifsc_code");
            getcol.Add("20", "bankaccount");
            getcol.Add("21", "pfnumber");
            getcol.Add("22", "ESI_No");
            getcol.Add("23", "pangirnumber");
            getcol.Add("24", "lic_no");
            getcol.Add("25", "adharcardno");
            getcol.Add("26", "loan_no");
            getcol.Add("27", "gpfnumber");
            getcol.Add("28", "UAN_No");
            getcol.Add("34", "IncrementTime");
            getcol.Add("35", "Tot_LOP");
            getcol.Add("36", "PF_Salary");
            getcol.Add("37", "ESI_Salary");
            getcol.Add("38", "AdvanceAmt1");
            getcol.Add("40", "DA");
            getcol.Add("41", "AGP");
            getcol.Add("42", "IncrementAmt1");
            getcol.Add("43", "Actual_Basic");
            getcol.Add("44", "bsalary");
            getcol.Add("45", "pay_band");
            getcol.Add("46", "grade_pay");
            getcol.Add("47", "G_Pay");
            getcol.Add("48", "netded");
            getcol.Add("49", "netadd");
            getcol.Add("50", "netaddact");
            getcol.Add("51", "payscale");
            getcol.Add("52", "TITLE");
            getcol.Add("53", "netsal");
            getcol.Add("54", "bankaccount");
            getcol.Add("57", "clgbankcode");
            getcol.Add("58", "amnt_per_hrs"); //parttimestaff
            getcol.Add("59", "CONVENES_EXP");
            getcol.Add("60", "LUNCH_EXP");




            fpsalary.Sheets[0].RowCount = 0;
            fpsalary.Sheets[0].ColumnCount = 1;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            lblnorec.Visible = false;
            fpsalary.Sheets[0].PageSize = 11;
            fpsalary.Sheets[0].RowHeader.Visible = false;
            fpsalary.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fpsalary.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            fpsalary.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fpsalary.Sheets[0].SheetCorner.Columns[0].Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            darkstyle.Border.BorderColor = Color.Black;
            fpsalary.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            fpsalary.CommandBar.Visible = false;
            fpsalary.Sheets[0].ColumnHeader.RowCount = 2;
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpsalary.Sheets[0].Columns[0].Width = 50;
            fpsalary.Sheets[0].Columns[0].Locked = true;
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FarPoint.Web.Spread.CheckBoxCellType chkcom = new FarPoint.Web.Spread.CheckBoxCellType();
            chkcom.AutoPostBack = true;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            chkcell1.AutoPostBack = false;
            FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.DoubleCellType doublecell = new FarPoint.Web.Spread.DoubleCellType();
            string datefrom = "";
            string dateto = "";
            string date1 = "";
            string date2 = "";
            fpsalary.Visible = true;
            rprint.Visible = true;
            chkShowPF.Visible = true;
            Hashtable TotalIndexValue = new Hashtable();
            TotalIndexValue.Clear();
            for (int ilst = 0; ilst < lstcolorder.Items.Count; ilst++)
            {
                if (getcol.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Value)))
                {
                    if (lstcolorder.Items[ilst].Text == "LOP AMOUNT")
                    {
                        if (cb_deduct.Checked == false && cb_all.Checked == false)
                        {
                            fpsalary.Sheets[0].ColumnCount++;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(lstcolorder.Items[ilst].Text);
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                            fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                            if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                            {
                                TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                            }
                        }
                    }
                    else if (lstcolorder.Items[ilst].Text == "TOT DED Rs.")
                    {
                        if (cb_deduct.Checked == false && cb_all.Checked == false)
                        {
                            fpsalary.Sheets[0].ColumnCount++;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(lstcolorder.Items[ilst].Text);
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                            fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                            if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                            {
                                TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                            }
                        }
                    }
                    else if (lstcolorder.Items[ilst].Text == "NET PAY")
                    {
                        if (cb_deduct.Checked == false && cb_all.Checked == false)
                        {
                            fpsalary.Sheets[0].ColumnCount++;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(lstcolorder.Items[ilst].Text);
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                            fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                            if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                            {
                                TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                            }
                        }
                    }
                    else
                    {
                        if (lstcolorder.Items[ilst].Text != "SIGNATURE" && lstcolorder.Items[ilst].Text != "BANK FORMAT" && lstcolorder.Items[ilst].Text != "CONVENES EXP" && lstcolorder.Items[ilst].Text != "LUNCH EXP")
                        {
                            if (lstcolorder.Items[ilst].Text == "ACTUAL GROSS SALARY")
                            {
                                if (cb_all.Checked == false || cb_deduct.Checked == false)
                                {
                                    fpsalary.Sheets[0].ColumnCount++;
                                    fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(lstcolorder.Items[ilst].Text);
                                    fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                                    fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                                    if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                                    {
                                        TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                                    }
                                }
                            }
                            else
                            {
                                fpsalary.Sheets[0].ColumnCount++;

                                // fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].ColumnCount - 1, 1].CellType = txt;
                                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(lstcolorder.Items[ilst].Text);


                                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                                fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                                if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                                {
                                    TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                                }

                            }
                        }
                    }
                }
                else
                {
                    if (lstcolorder.Items[ilst].Text != "SIGNATURE" && lstcolorder.Items[ilst].Text != "BANK FORMAT")
                    {
                        if (lstcolorder.Items[ilst].Text == "ACTUAL GROSS SALARY")
                        {
                            if (cb_all.Checked == false || cb_deduct.Checked == false)
                            {
                                fpsalary.Sheets[0].ColumnCount++;
                                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(lstcolorder.Items[ilst].Text);
                                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                                if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                                {
                                    TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                                }
                                if (lstcolorder.Items[ilst].Text == "SELECT")
                                {
                                    fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = false;
                                }
                                else
                                {
                                    fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                                }
                            }
                        }
                        else
                        {
                            fpsalary.Sheets[0].ColumnCount++;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(lstcolorder.Items[ilst].Text);
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                            if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                            {
                                TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                            }
                            if (lstcolorder.Items[ilst].Text == "SELECT")
                            {
                                fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = false;
                            }
                            else
                            {
                                fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                            }
                        }
                    }
                }
            }
            string bankquery = "select hb.bankpk,BankName from hr_bankrights  hb,FM_FinBankMaster fb where hb.bankpk=fb.BankPK and hb.college_code=fb.CollegeCode and hb.college_code='" + ddlcollege.SelectedItem.Value + "'";//delsi1106
            DataSet bankds = new DataSet();
            int columncountbank = 0;
            int countvalue = 0;

            bankds = d2.select_method_wo_parameter(bankquery, "text");
            if (bankds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < bankds.Tables[0].Rows.Count; i++)
                {

                    fpsalary.Sheets[0].ColumnCount++;
                    fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(bankds.Tables[0].Rows[i]["BankName"]);
                    fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(bankds.Tables[0].Rows[i]["bankpk"]);
                    fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);


                }

                fpsalary.Sheets[0].ColumnCount++;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "Cash";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                columncountbank = fpsalary.Sheets[0].ColumnCount;
                countvalue++;
            }


            sql1 = "select * from incentives_master where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            sql1 = sql1 + " ;select distinct CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "'and PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and college_Code=" + collegecode1 + "";
            sql1 = sql1 + " ;select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + collegecode1 + "'";
            sql1 = sql1 + " ;select shortname from leave_category where status<>'pres' and college_code='" + collegecode1 + "'";
            sql1 = sql1 + " ;select shortname from leave_category where status<>'comp' and college_code='" + collegecode1 + "'";
            string getvaluedigits = d2.GetFunction("select value from Master_Settings where settings='Bank Foramte Fixed Digits'");
            DataSet dsset = d2.select_method_wo_parameter(sql1, "Text");
            int colstatic = 0;
            int allcol = 0;
            int dedcolstatic = 0;
            int dedcol = 0;
            int levcolstatic = 0;
            int levcol = 0;
            Hashtable LeaveHash = new Hashtable();
            Hashtable AllowanceHash = new Hashtable();
            Hashtable DeducationHash = new Hashtable();
            if (cbleavecat.Checked == true)
            {
                for (int lev = 0; lev < chklstlev.Items.Count; lev++)
                {
                    if (chklstlev.Items[lev].Selected == true)
                    {
                        fpsalary.Sheets[0].ColumnCount++;
                        if (levcol == 0)
                        {
                            levcolstatic = fpsalary.Sheets[0].ColumnCount - 1;
                        }
                        levcol++;
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chklstlev.Items[lev].Text);
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                        if (!LeaveHash.ContainsKey(Convert.ToString(chklstlev.Items[lev].Value)))
                        {
                            LeaveHash.Add(Convert.ToString(chklstlev.Items[lev].Value), fpsalary.Sheets[0].ColumnCount - 1);
                        }
                    }
                }
                fpsalary.Sheets[0].ColumnHeader.Cells[0, levcolstatic].Text = "Leave Category";
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, levcolstatic, 1, levcol);
                fpsalary.Sheets[0].ColumnHeader.Cells[0, levcolstatic].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, levcolstatic].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, levcolstatic].Font.Name = "Book Antiqua";
            }
            if (cb_all.Checked == true)
            {
                for (int allow = 0; allow < cbl_allow.Items.Count; allow++)
                {
                    if (cbl_allow.Items[allow].Selected == true)
                    {
                        fpsalary.Sheets[0].ColumnCount++;
                        if (allcol == 0)
                        {
                            colstatic = fpsalary.Sheets[0].ColumnCount - 1;
                        }
                        allcol++;
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].Text = (Convert.ToString(cbl_allow.Items[allow].Text) == "Da") ? "DA" : Convert.ToString(cbl_allow.Items[allow].Text) + " " + "Rs.";
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                        if (!AllowanceHash.ContainsKey(Convert.ToString(cbl_allow.Items[allow].Text)))
                        {
                            AllowanceHash.Add(Convert.ToString(cbl_allow.Items[allow].Text) == "Da" ? "DA" : Convert.ToString(cbl_allow.Items[allow].Text), fpsalary.Sheets[0].ColumnCount - 1);
                        }
                    }
                }
                fpsalary.Sheets[0].ColumnHeader.Cells[0, colstatic].Text = "Allowance";
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, colstatic, 1, allcol);
                fpsalary.Sheets[0].ColumnHeader.Cells[0, colstatic].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, colstatic].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, colstatic].Font.Name = "Book Antiqua";
            }
            for (int ilst = 0; ilst < lstcolorder.Items.Count; ilst++)
            {
                if (getcol.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Value)))
                {
                    if (lstcolorder.Items[ilst].Text == "ACTUAL GROSS SALARY")
                    {
                        if (cb_all.Checked == true && cb_deduct.Checked == true)
                        {
                            fpsalary.Sheets[0].ColumnCount++;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "ACTUAL GROSS SALARY";
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                            fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                            if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                            {
                                TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                            }
                        }
                    }
                }
            }
            if (cb_deduct.Checked == true)
            {
                for (int allow = 0; allow < cbl_ded.Items.Count; allow++)
                {
                    if (cbl_ded.Items[allow].Selected == true)
                    {
                        fpsalary.Sheets[0].ColumnCount++;
                        if (dedcol == 0)
                        {
                            dedcolstatic = fpsalary.Sheets[0].ColumnCount - 1;
                        }
                        dedcol++;
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_ded.Items[allow].Text) + " " + "Rs.";
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        fpsalary.Sheets[0].ColumnHeader.Cells[1, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                        if (!DeducationHash.ContainsKey(Convert.ToString(cbl_ded.Items[allow].Text)))
                        {
                            DeducationHash.Add(Convert.ToString(cbl_ded.Items[allow].Text), fpsalary.Sheets[0].ColumnCount - 1);
                        }
                    }
                }
                fpsalary.Sheets[0].ColumnHeader.Cells[0, dedcolstatic].Text = "Deduction";
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, dedcolstatic, 1, dedcol);
                fpsalary.Sheets[0].ColumnHeader.Cells[0, dedcolstatic].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, dedcolstatic].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, dedcolstatic].Font.Name = "Book Antiqua";
            }
            for (int ilst = 0; ilst < lstcolorder.Items.Count; ilst++)
            {
                if (getcol.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Value)))
                {
                    if (lstcolorder.Items[ilst].Text == "LOP AMOUNT")
                    {
                        if (cb_all.Checked == true || cb_deduct.Checked == true)
                        {
                            fpsalary.Sheets[0].ColumnCount++;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "LOP AMOUNT";
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                            fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                            if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                            {
                                TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                            }
                        }
                    }
                    if (lstcolorder.Items[ilst].Text == "TOT DED Rs.")
                    {
                        if (cb_all.Checked == true || cb_deduct.Checked == true)
                        {
                            fpsalary.Sheets[0].ColumnCount++;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "TOT DED Rs.";
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                            fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                            if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                            {
                                TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                            }
                        }
                    }
                    if (lstcolorder.Items[ilst].Text == "NET PAY")
                    {
                        if (cb_all.Checked == true || cb_deduct.Checked == true)
                        {
                            fpsalary.Sheets[0].ColumnCount++;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "NET PAY";
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                            fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                            if (!TotalIndexValue.ContainsKey(Convert.ToString(lstcolorder.Items[ilst].Text)))
                            {
                                TotalIndexValue.Add(Convert.ToString(lstcolorder.Items[ilst].Text), fpsalary.Sheets[0].ColumnCount - 1);
                            }
                        }
                    }
                }
            }
            if (cblcolumnorder.Items[59].Selected == true)
            {
                fpsalary.Sheets[0].ColumnCount++;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "CONVENES EXP";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                if (!TotalIndexValue.ContainsKey(Convert.ToString("CONVENES EXP")))
                {
                    TotalIndexValue.Add(Convert.ToString("CONVENES EXP"), fpsalary.Sheets[0].ColumnCount - 1);
                }
            }
            if (cblcolumnorder.Items[60].Selected == true)
            {
                fpsalary.Sheets[0].ColumnCount++;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "LUNCH EXP";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                if (!TotalIndexValue.ContainsKey(Convert.ToString("LUNCH EXP")))
                {
                    TotalIndexValue.Add(Convert.ToString("LUNCH EXP"), fpsalary.Sheets[0].ColumnCount - 1);
                }
            }
            if (cblcolumnorder.Items[54].Selected == true)
            {
                fpsalary.Sheets[0].ColumnCount++;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "BANK FORMAT";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                if (!TotalIndexValue.ContainsKey(Convert.ToString("BANK FORMAT")))
                {
                    TotalIndexValue.Add(Convert.ToString("BANK FORMAT"), fpsalary.Sheets[0].ColumnCount - 1);
                }
            }
            if (cblcolumnorder.Items[61].Selected == true)
            {

                fpsalary.Sheets[0].ColumnCount++;
                gratuitucolcount = fpsalary.Sheets[0].ColumnCount;

                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "GRATUITY";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                if (!TotalIndexValue.ContainsKey(Convert.ToString("GRATUITY")))
                {
                    TotalIndexValue.Add(Convert.ToString("GRATUITY"), fpsalary.Sheets[0].ColumnCount - 1);
                }
            }

            if (cblcolumnorder.Items[62].Selected == true)
            {

                fpsalary.Sheets[0].ColumnCount++;
                yrofexpcount = fpsalary.Sheets[0].ColumnCount;

                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "YEAR OF EXPERIENCE";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                if (!TotalIndexValue.ContainsKey(Convert.ToString("YEAR OF EXPERIENCE")))
                {
                    TotalIndexValue.Add(Convert.ToString("YEAR OF EXPERIENCE"), fpsalary.Sheets[0].ColumnCount - 1);
                }
            }

            if (cblcolumnorder.Items[55].Selected == true)
            {
                fpsalary.Sheets[0].ColumnCount++;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Text = "SIGNATURE";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - 1].Font.Bold = true;
                fpsalary.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalary.Sheets[0].ColumnCount - 1, 2, 1);
                fpsalary.Sheets[0].Columns[fpsalary.Sheets[0].ColumnCount - 1].Locked = true;
                if (!TotalIndexValue.ContainsKey(Convert.ToString("SIGNATURE")))
                {
                    TotalIndexValue.Add(Convert.ToString("SIGNATURE"), fpsalary.Sheets[0].ColumnCount - 1);
                }
            }

            #endregion

            #region for modifying
            string fromdate = "";
            string todate = "";
            string[] split = new string[2];
            string[] split1 = new string[2];
            int dedrowcount = 0;
            int allrowcount = 0;
            double convenexp = 0;
            double lunchexp = 0;
            double newconvenexp = 0;
            double newlunchexp = 0;


            if (dsset.Tables[1].Rows.Count > 0)
            {
                fromdate = dsset.Tables[1].Rows[0]["from_date"].ToString();
                todate = dsset.Tables[1].Rows[0]["to_date"].ToString();
            }
            date1 = fromdate;
            date2 = todate;
            if (date1.Trim() == "" || date2.Trim() == "")
            {
                fpsalary.Visible = false;
                div1.Visible = false;
                rprint.Visible = false;
                chkShowPF.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found!";
                btnpayslip.Visible = false;
                chkIncLopAmnt.Visible = false;
                btnsalcer.Visible = false;
                return;
            }
            if (date1.Trim() != "")
            {
                split = date1.Split(new Char[] { '/' });
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            }
            if (date2.Trim() != "")
            {
                split1 = date2.Split(new Char[] { '/' });
                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            }
            string coll_name = "";
            string coll_address1 = "";
            string coll_address2 = "";
            string coll_address3 = "";
            string pin_code = "";
            if (dsset.Tables[2].Rows.Count > 0)
            {
                coll_name = dsset.Tables[2].Rows[0]["collname"].ToString();
                coll_address1 = dsset.Tables[2].Rows[0]["address1"].ToString();
                coll_address2 = dsset.Tables[2].Rows[0]["address2"].ToString();
                coll_address3 = dsset.Tables[2].Rows[0]["address3"].ToString();
                pin_code = dsset.Tables[2].Rows[0]["pincode"].ToString();
            }
            fpsalary.Sheets[0].SetColumnWidth(fpsalary.Sheets[0].ColumnCount - 1, 100);
            fpsalary.Sheets[0].SetColumnWidth(0, 100);
            fpsalary.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
            fpsalary.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
            fpsalary.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
            fpsalary.Sheets[0].FrozenColumnCount = 3;
            alleavetype.Clear();
            if (dsset.Tables[3].Rows.Count > 0)
            {
                for (int dtsval = 0; dtsval < dsset.Tables[3].Rows.Count; dtsval++)
                {
                    alleavetype.Add(Convert.ToString(dsset.Tables[3].Rows[dtsval]["shortname"]));
                }
            }
            hatpre.Clear();
            hatpre.Add("P", 0);
            hatpre.Add("LA", 0);
            hatpre.Add("PER", 0);
            hatpre.Add("H", 0);
            for (int prc = 0; prc < dsset.Tables[4].Rows.Count; prc++)
            {
                if (!hatpre.Contains(dsset.Tables[4].Rows[prc]["shortname"].ToString()))
                {
                    hatpre.Add(dsset.Tables[4].Rows[prc]["shortname"].ToString(), 0);
                }
            }
            goto l10; /* column paytype added by poomalar 23.10.17*/

        l10: sql = " SELECT m.*,st.dept_code,m.deductions as deductions,m.allowances as allowances,isnull(m.netsal,0) as netsal,isnull(m.payscale,0) as payscale,ISNULL(m.Actual_Basic,0) as Actual_Basic,ISNULL(m.IncrementAmt,0) as IncrementAmt1,ISNULL(m.DAWithLOP,0) as DALop,m.IncrementTime as IncrementTime,m.Pre_Lop as Pre_Lop,m.Cur_Lop as Cur_Lop,m.stftype as stftype,m.leavedetail as leavedetail,ISNULL(m.basic_alone,0) as basic_alone,ISNULL(m.DAAmt,0) as DA,m.Basic as Basic,ISNULL(m.AGP,0) as AGP,ISNULL(m.bsalary,0) as bsalary,isnull(m.NetAddAct,0) as netaddact,isnull(m.netded,0) as netded,sm.ESI_No as ESI_No,dm.priority,dept_acronym,staff_name,bankaccount,pfnumber,CONVERT(VARCHAR(10),sm.join_date,103) as joindate,ISNULL(m.pay_band,0) as pay_band,ISNULL(m.grade_pay,0) as grade_pay,m.pf as pf,dm.desig_acronym,ISNULL(m.LOP,0) as LOP,len(sm.staff_code),sm.staff_code,dm.print_pri,ISNULL(m.PF_Salary,0) as PF_Salary,ISNULL(m.ESI_Salary,0) as ESI_Salary,hd.dept_name as deptname,dm.desig_name as designame,sm.resign,Convert(varchar(10),sm.appointed_date,103) as appointdate,Convert(varchar(10),sm.retr_date,103) as resigndate,Convert(varchar(10),sm.relieve_date,103) as relieve_date,Convert(varchar(10),sa.date_of_birth,103) as dateofbirth,title,convert(decimal ,isnull(m.AdvanceAmt,0)) as AdvanceAmt1,sc.category_name,case when st.PayMode='0' then 'Cash' when st.PayMode='1' then 'Cheque' when st.PayMode='2' then 'Credit'  end as PayMode,case when st.BankAccType='1' then 'Own Account' when st.BankAccType='2' then 'Nominee Account' end as BankAccType,sm.ifsc_code,sm.bank_name,isnull(sm.clgbankcode,'') as clgbankcode,sm.branch_name,sm.pangirnumber,sm.lic_no,sm.adharcardno,sm.loan_no, sm.gpfnumber,sm.UAN_No,hwp.tot_hrs,hwp.amnt_per_hrs ,'' CONVENES_EXP,'' LUNCH_EXP from monthlypay m,desig_master dm,staffmaster sm,hrdept_master hd,staff_appl_master sa,staffcategorizer sc,stafftrans st left join HourWise_PaySettings hwp on st.desig_code=hwp.desig_code and st.dept_code=hwp.dept_code and st.staff_code=hwp.staffcode and isnull(hwp.PayType,0)='1' Where sa.appl_no=sm.appl_no and m.staff_code=st.staff_code and m.staff_code=sm.staff_code and sm.staff_code=st.staff_code and st.latestrec=1 and st.desig_code=dm.desig_code and hd.dept_code=st.dept_code and sc.category_code=st.category_code and sm.college_code=dm.collegecode and sm.college_code=sc.college_code and sm.college_code=m.college_code and ((sm.resign=0 or sm.settled=0) or (sm.resign=1 and sm.relieve_date>='" + dateto + "') or (sm.resign=1 and sm.relieve_date between '" + datefrom + "' and '" + dateto + "')) and m.PayMonth ='" + ddl_mon.SelectedValue.ToString() + "' and m.PayYear ='" + ddl_year.SelectedValue.ToString() + "' and sm.college_code='" + collegecode1 + "'";
            string staffcheck = Convert.ToString(ViewState["isstaffcheck"]);//delsi2205
            if (staffcheck.ToUpper() == "TRUE" || staffcheck == "1")
            {

                string stfCode = d2.GetFunction("select staff_code from usermaster where User_code='" + usercode + "'");
                if (stfCode != "0" || stfCode != "")
                {
                    sql = sql + " and sm.staff_code='" + stfCode + "'";
                }
            }
            if (cb_relived.Checked == true)//delsi050/5/2018
            {
                sql = " SELECT m.*,st.dept_code,m.deductions as deductions,m.allowances as allowances,isnull(m.netsal,0) as netsal,isnull(m.payscale,0) as payscale,ISNULL(m.Actual_Basic,0) as Actual_Basic,ISNULL(m.IncrementAmt,0) as IncrementAmt1,ISNULL(m.DAWithLOP,0) as DALop,m.IncrementTime as IncrementTime,m.Pre_Lop as Pre_Lop,m.Cur_Lop as Cur_Lop,m.stftype as stftype,m.leavedetail as leavedetail,ISNULL(m.basic_alone,0) as basic_alone,ISNULL(m.DAAmt,0) as DA,m.Basic as Basic,ISNULL(m.AGP,0) as AGP,ISNULL(m.bsalary,0) as bsalary,isnull(m.NetAddAct,0) as netaddact,isnull(m.netded,0) as netded,sm.ESI_No as ESI_No,dm.priority,dept_acronym,staff_name,bankaccount,pfnumber,CONVERT(VARCHAR(10),sm.join_date,103) as joindate,Convert(varchar(10),sm.relieve_date,103) as relieve_date,ISNULL(m.pay_band,0) as pay_band,ISNULL(m.grade_pay,0) as grade_pay,m.pf as pf,dm.desig_acronym,ISNULL(m.LOP,0) as LOP,len(sm.staff_code),sm.staff_code,dm.print_pri,ISNULL(m.PF_Salary,0) as PF_Salary,ISNULL(m.ESI_Salary,0) as ESI_Salary,hd.dept_name as deptname,dm.desig_name as designame,sm.resign,Convert(varchar(10),sm.appointed_date,103) as appointdate,Convert(varchar(10),sm.retr_date,103) as resigndate,Convert(varchar(10),sa.date_of_birth,103) as dateofbirth,title,convert(decimal ,isnull(m.AdvanceAmt,0)) as AdvanceAmt1,sc.category_name,case when st.PayMode='0' then 'Cash' when st.PayMode='1' then 'Cheque' when st.PayMode='2' then 'Credit'  end as PayMode,case when st.BankAccType='1' then 'Own Account' when st.BankAccType='2' then 'Nominee Account' end as BankAccType,sm.ifsc_code,sm.bank_name,sm.branch_name,sm.pangirnumber,sm.lic_no,sm.adharcardno,sm.loan_no, sm.gpfnumber,sm.UAN_No,hwp.tot_hrs,hwp.amnt_per_hrs ,'' CONVENES_EXP,'' LUNCH_EXP from monthlypay m,desig_master dm,staffmaster sm,hrdept_master hd,staff_appl_master sa,staffcategorizer sc,stafftrans st left join HourWise_PaySettings hwp on st.desig_code=hwp.desig_code and st.dept_code=hwp.dept_code and st.staff_code=hwp.staffcode and isnull(hwp.PayType,0)='1' Where sa.appl_no=sm.appl_no and m.staff_code=st.staff_code and m.staff_code=sm.staff_code and sm.staff_code=st.staff_code and st.latestrec=1 and st.desig_code=dm.desig_code and hd.dept_code=st.dept_code and sc.category_code=st.category_code and sm.college_code=dm.collegecode and sm.college_code=sc.college_code and sm.college_code=m.college_code and ((sm.resign=0 or sm.settled=0) or (sm.resign=1 and sm.relieve_date>='" + dateto + "') or (sm.resign=1 and sm.relieve_date between '" + datefrom + "' and '" + dateto + "')) and relieve_date between '" + datefrom + "' and '" + dateto + "' and m.PayMonth ='" + ddl_mon.SelectedValue.ToString() + "' and m.PayYear ='" + ddl_year.SelectedValue.ToString() + "' and sm.college_code='" + collegecode1 + "'";
                string staffchecks = Convert.ToString(ViewState["isstaffcheck"]);//delsi2205
                if (staffchecks.ToUpper() == "TRUE" || staffchecks == "1")
                {

                    string stfCode = d2.GetFunction("select staff_code from usermaster where User_code='" + usercode + "'");
                    if (stfCode != "0" || stfCode != "")
                    {
                        sql = sql + " and sm.staff_code='" + stfCode + "'";
                    }
                }
                btn_finalsettlement.Visible = true;

            }

            int clgbankval = Convert.ToInt32(ddl_colbank.SelectedItem.Value);

            if (txtstaffname.Text.Trim() != "")
            {
                sql = sql + " and sm.staff_name ='" + txtstaffname.Text + "'";
            }
            else if (txtstaffcode.Text.Trim() != "")
            {
                sql = sql + " and sm.staff_code ='" + txtstaffcode.Text + "'";
            }
            else
            {
                if (txt_dept.Text != "---Select---")
                {
                    int itemcount = 0;
                    for (itemcount = 0; itemcount < cbl_dept.Items.Count; itemcount++)
                    {
                        if (cbl_dept.Items[itemcount].Selected == true)
                        {
                            if (strdept == "")
                                strdept = "'" + cbl_dept.Items[itemcount].Value.ToString() + "'";
                            else
                                strdept = strdept + "," + "'" + cbl_dept.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (strdept != "")
                    {
                        sql = sql + " and hd.dept_code in(" + strdept + ")";
                    }
                }
                string strdesig = "";
                for (int idesig = 0; idesig < cbl_desig.Items.Count; idesig++)
                {
                    if (cbl_desig.Items[idesig].Selected == true)
                    {
                        if (strdesig == "")
                        {
                            strdesig = "'" + cbl_desig.Items[idesig].Value.ToString() + "'";
                        }
                        else
                        {
                            strdesig = strdesig + ",'" + cbl_desig.Items[idesig].Value.ToString() + "'";
                        }
                    }
                }
                if (strdesig != "")
                {
                    sql = sql + " and dm.desig_code in(" + strdesig + ")";
                }
            }
            string strtypeval = "";
            for (int idesig = 0; idesig < cbl_stafftyp.Items.Count; idesig++)
            {
                if (cbl_stafftyp.Items[idesig].Selected == true)
                {
                    if (strtypeval == "")
                    {
                        strtypeval = "'" + cbl_stafftyp.Items[idesig].Value.ToString() + "'";
                    }
                    else
                    {
                        strtypeval = strtypeval + ",'" + cbl_stafftyp.Items[idesig].Value.ToString() + "'";
                    }
                }
            }
            if (strtypeval != "")
            {
                sql = sql + " and st.stftype in(" + strtypeval + ")";
            }
            if (txt_staffcat.Text != "---Select---")
            {
                int itemcount1 = 0;
                for (itemcount1 = 0; itemcount1 < cbl_staffcat.Items.Count; itemcount1++)
                {
                    if (cbl_staffcat.Items[itemcount1].Selected == true)
                    {
                        if (strcategory == "")
                            strcategory = "'" + cbl_staffcat.Items[itemcount1].Value.ToString() + "'";
                        else
                            strcategory = strcategory + "," + "'" + cbl_staffcat.Items[itemcount1].Value.ToString() + "'";
                    }
                }
                if (strcategory != "")
                {
                    sql = sql + " and st.category_code in(" + strcategory + ")";
                }
            }
            string strpaymode = "";
            if (txtpaymode.Text.Trim() != "--Select--")
            {
                for (int ik = 0; ik < cbl_paymode.Items.Count; ik++)
                {
                    if (cbl_paymode.Items[ik].Selected == true)
                    {
                        if (strpaymode.Trim() == "")
                        {
                            strpaymode = "'" + cbl_paymode.Items[ik].Value.ToString() + "'";
                        }
                        else
                        {
                            strpaymode = strpaymode + "," + "'" + cbl_paymode.Items[ik].Value.ToString() + "'";
                        }
                    }
                }
                if (strpaymode != "")
                {
                    sql = sql + " and ISNULL(st.PayMode,'') in(''," + strpaymode + ")";
                }
            }
            if (ddl_colbank.Text.Trim().ToUpper() != "SELECT")
            {
                if (clgbankval != 0)
                {
                    sql = sql + " and isnull(sm.clgbankcode,'') ='" + clgbankval + "'";
                }

            }
            if (cb_partimestaff.Checked)//part time staff delsi0811
                sql = sql + " and (isnull(st.stfnature,0)='1' or isnull(st.stfnature,'')='part')";
            else
                sql = sql + " and isnull(st.stfnature,0)<>'1'";

            if (cb_hold.Checked == true)//delsi 1002
            {
                sql = sql + " and isnull(m.staffholdSet,'') in('','1','2')";
            }
            else
            {
                sql = sql + " and isnull(m.staffholdSet,'') in('','2')";
            }

            if (ddlorder.SelectedItem.Text.ToString() == "Priority") // 22.01.18 barath
                sql = sql + " order by hd.priority,dm.priority,hd.dept_name,sm.join_date,len(sm.staff_code),sm.staff_code  ";
            else if (ddlorder.SelectedItem.Text.ToString() == "Print Priority-1")
                sql = sql + "  order by hd.priority, dm.priority,  sm.PrintPriority,sm.join_date,len(sm.staff_code),sm.staff_code";
            else if (ddlorder.SelectedItem.Text.ToString() == "Print Priority-2")
                sql = sql + "  order by hd.priority1,  dm.print_pri, sm.PrintPriority,sm.join_date,len(sm.staff_code),sm.staff_code";
            else if (ddlorder.SelectedItem.Text.ToString() == "Account No")
                sql = sql + "  order by case when sm.bankaccount is null then 1 else 0 end,len(sm.bankaccount), sm.bankaccount";
            else if (ddlorder.SelectedItem.Text.ToString() == "Staff Wise Priority")
                sql = sql + "order by isnull(sm.PrintPriority,0) desc";
            else
                sql = sql + " order by hd.dept_name,len(sm.staff_code),sm.staff_code";

            //if (chkAccNo.Checked == true)
            //{
            //    sql = sql + " order by bankaccount,ISNULL(hd.Priority,100),dm.priority,len(sm.staff_code),sm.staff_code";
            //}
            //else
            //{
            //    sql = sql + " order by ISNULL(hd.Priority,100),dm.priority,len(sm.staff_code),sm.staff_code";
            //}
            sql = sql + " select sa.per_mobileno,sa.email,sm.staff_code,allowances,deductions from staff_appl_master sa,staffmaster sm, stafftrans st  where sm.appl_no=sa.appl_no and ((sm.settled=0 and sm.resign=0) and ISNULL(sm.Discontinue,'0')='0') and sm.staff_code=st.staff_code  and st.latestrec=1";
            sql = sql + "  select shortname from leave_category where status ='comp' and college_code =" + collegecode1 + "";
            sql = sql + " select * from incentives_master where college_code='" + collegecode1 + "'";
            string df = fromdate;
            string dt = todate;
            string[] stf = df.Split('/');
            string[] stt = dt.Split('/');
            DateTime dtf = Convert.ToDateTime(stf[1] + '/' + stf[0] + '/' + stf[2]);
            DateTime dtt = Convert.ToDateTime(stt[1] + '/' + stt[0] + '/' + stt[2]);
            string qry = " select * from staff_attnd where mon_year between '" + dtf.ToString("M/yyyy") + "' and '" + dtt.ToString("M/yyyy") + "'";
            qry += " select linkvalue from new_inssettings where linkName='Parttime Staff Convenes Expance' and college_code='" + collegecode1 + "'";
            DataSet dsstaffatt = d2.select_method_wo_parameter(qry, "Text");
            ArrayList addleave = new ArrayList();
            fpsalary.Sheets[0].RowCount += 1;
            dsset.Reset();
            dsset.Dispose();
            dsset = d2.select_method_wo_parameter(sql, "Text");
            int postbackcount = 0;
            if (dsset.Tables[0].Rows.Count > 0)
            {
                txtexcel.Visible = true;
                btnexcel.Visible = true;
                lblexcel.Visible = true;
                btnprintmaster.Visible = true;
                btnPrintNew.Visible = true;
                string monyear;
                int dayfrm = 0;
                string dayto;
                int daytonum;
                int year3;
                string datefrom3 = "";
                string monthname2 = "";
                int monthnamenum;
                int monthname1;
                string yearto = "";
                string monyearto = "";
                string staffcode1 = "";
                string joindate = "";
                string reliv = "";
                double actualbasic = 0.0;
                double lopamt = 0.0;
                double pfcont = 0.0;
                double esicont = 0.0;
                double daper = 0.0;
                double basicpay = 0.0;
                double incinrs = 0.0;
                double Actgross = 0.0;
                double grosspay = 0.0;
                double Advrs = 0.0;
                double collectedamt = 0.0;
                double totded = 0.0;
                double payband = 0.0;
                double actgradeamnt = 0.0;
                double gradeamnt = 0.0;
                double netamnt = 0.0;
                if (dsset.Tables[2].Rows.Count > 0)
                {
                    for (int leave = 0; leave < dsset.Tables[2].Rows.Count; leave++)
                    {
                        addleave.Add(Convert.ToString(dsset.Tables[2].Rows[leave]["shortname"]));
                    }
                }
                int rowviscount = 0;
                int dedrowviscount = 0;
                bool dedrowcheck = false;
                bool rowcheck = false;
                Double basamnt = 0;
                string getbas = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Basic Pay' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'");
                string graduity = string.Empty;
                graduity = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='gratuity' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'");
                for (int sal = 0; sal < dsset.Tables[0].Rows.Count; sal++)
                {
                    string workingdays = "";
                    string newabsdays = "";
                    string newpresdays = "";
                    double absdays = 0.0;
                    Session["absent"] = "";
                    double Present = 0.0;
                    Session["present"] = "";
                    Session["lopdays"] = "";
                    double presentdays = 0.0;
                    string actu_basicpay = "";
                    string allowance = "";
                    string gradepay = "";
                    double newbasicpay = 0;
                    double newactualbasic = 0;
                    double newlopamt = 0;
                    double newpfcont = 0;
                    double newesicont = 0;
                    double newdaper = 0;
                    double newincinrs = 0;
                    double newActgross = 0;
                    double newgrosspay = 0;
                    double newAdvrs = 0;
                    double newcollectedamt = 0;
                    double newtotded = 0;
                    double newpayband = 0;
                    double newactgradeamnt = 0;
                    double newgradeamnt = 0;
                    double newnetamnt = 0;
                    double grosssal = 0;
                    double totdeduct = 0;
                    double netsalary = 0;

                    int gratuityval = 0;
                    int getnetsal = 0;
                    int gettotallow = 0;
                    string date3 = fromdate.ToString();
                    string[] split5 = date3.Split(new Char[] { '/' });
                    staffcode1 = dsset.Tables[0].Rows[sal]["staff_code"].ToString();
                    joindate = dsset.Tables[0].Rows[sal]["joindate"].ToString();
                    reliv = dsset.Tables[0].Rows[sal]["relieve_date"].ToString();
                    if (reliv != "")
                    {
                        if (reliv.Contains('/'))
                        {
                            string[] splitrelv = reliv.Split('/');
                            reliv = Convert.ToString(splitrelv[1] + "/" + splitrelv[0] + "/" + splitrelv[2]);

                        }

                    }

                    if (joindate.Contains('/'))
                    {
                        string[] splitjoin = joindate.Split('/');
                        joindate = Convert.ToString(splitjoin[1] + "/" + splitjoin[0] + "/" + splitjoin[2]);

                    }
                    DateTime join_datetime = Convert.ToDateTime(joindate);

                    DateTime TodayData = DateTime.Now;
                    if (reliv != "")
                    {
                        TodayData = Convert.ToDateTime(reliv);
                    }
                    int Years = TodayData.Year - join_datetime.Year;
                    int month = TodayData.Month - join_datetime.Month;
                    string leavedetail = dsset.Tables[0].Rows[sal]["leavedetail"].ToString();
                    string[] spiltleavedetail = leavedetail.Split(new Char[] { ';' });
                    workingdays = spiltleavedetail[0].ToString();
                    actu_basicpay = dsset.Tables[0].Rows[sal]["Actual_Basic"].ToString();
                    gradepay = dsset.Tables[0].Rows[sal]["grade_pay"].ToString();
                    string netsal = dsset.Tables[0].Rows[sal]["netsal"].ToString();
                    double totlop = 0;
                    double basicamnt = 0;
                    Double.TryParse(actu_basicpay, out basicamnt);
                    basicamnt = Math.Round(basicamnt, 0, MidpointRounding.AwayFromZero);

                    double oneday_sal = 0;
                    if (workingdays != "")
                    {
                        oneday_sal = Convert.ToDouble(actu_basicpay) / Convert.ToDouble(workingdays);
                    }
                    oneday_sal = Math.Round(oneday_sal, 0, MidpointRounding.AwayFromZero);


                    double netsalaryIncludezero = 0;
                    bool NetSalNotZero = false;
                    double.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["netsal"]), out netsalaryIncludezero);
                    if (chk_amnt.Checked == false)
                    {
                        if (netsalaryIncludezero == 0)
                            NetSalNotZero = true;
                    }
                    if (!NetSalNotZero)//delsref
                    {
                        if (spiltleavedetail.Length >= 2)
                        {
                            newpresdays = Convert.ToString(spiltleavedetail[1]);
                        }
                        if (spiltleavedetail.Length >= 3)
                        {
                            newabsdays = Convert.ToString(spiltleavedetail[2]);
                        }
                        if (spiltleavedetail.Length >= 7)
                        {
                            Double.TryParse(Convert.ToString(spiltleavedetail[6]), out totlop);
                        }
                        string holidaycount = d2.GetFunction("select COUNT(*) as holcount from holidayStaff where holiday_date between '" + dtf.ToString("MM/dd/yyyy") + "' and '" + dtt.ToString("MM/dd/yyyy") + "' and StfType='" + Convert.ToString(dsset.Tables[0].Rows[sal]["stftype"]) + "' and category_code='" + Convert.ToString(dsset.Tables[0].Rows[sal]["category_code"]) + "' and dept_code='" + Convert.ToString(dsset.Tables[0].Rows[sal]["dept_code"]) + "' and college_code='" + collegecode1 + "'");
                        int holcount = 0;
                        Int32.TryParse(holidaycount, out holcount);
                        //if (workingdays.Trim() != "")//delsi1304
                        //{
                        //    workingdays = Convert.ToString(Convert.ToDouble(workingdays) - holcount);
                        //}
                        allowance = "";
                        fpsalary.Sheets[0].RowCount++;
                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        if (chk_amnt.Checked == false)
                        {
                        }
                        else
                        {
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sal + 1);
                        }
                        for (int lstcol = 0; lstcol < lstcolorder.Items.Count; lstcol++)
                        {
                            if (getcol.ContainsKey(Convert.ToString(lstcolorder.Items[lstcol].Value)))
                            {
                                int colidx = 0;
                                Int32.TryParse(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]), out colidx);
                                if (colidx != 0)
                                {
                                    if (lstcolorder.Items[lstcol].Text != "LOP AMOUNT" && lstcolorder.Items[lstcol].Text != "TOT DED Rs." && lstcolorder.Items[lstcol].Text != "ACTUAL GROSS SALARY" && lstcolorder.Items[lstcol].Text != "NET PAY")
                                    {
                                        if (lstcolorder.Items[lstcol].Text == "SELECT")
                                        {
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].CellType = chkcell1;
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Value = 0;
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                        }
                                        else
                                        {
                                            if (lstcolorder.Items[lstcol].Text == "BASIC PAY Rs.")
                                            {
                                                if (getbas.Trim() != "0" && getbas.Trim() != "0.00" && getbas.Trim() != "")
                                                {
                                                    Double.TryParse(getbas, out basamnt);
                                                    if (Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]) >= basamnt)
                                                    {
                                                        basamnt = Math.Round(basamnt, 0, MidpointRounding.AwayFromZero);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(basamnt);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                                    }
                                                    else
                                                    {
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                                    }
                                                }
                                                else
                                                {
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                                }
                                            }
                                            else if (lstcolorder.Items[lstcol].Text == "CONVENES EXP")//17.08.17
                                            {
                                                //if (grosssal == 0) // poo 25.10.17
                                                //    convenexp = 0;
                                                if (dsstaffatt.Tables[1].Rows.Count > 0)
                                                    double.TryParse(Convert.ToString(dsstaffatt.Tables[1].Rows[0]["linkvalue"]), out convenexp);
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(convenexp);
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                                                newconvenexp = newconvenexp + convenexp;
                                            }

                                            else if (lstcolorder.Items[lstcol].Text == "LUNCH EXP")
                                            {
                                                Double.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["netaddact"]), out grosssal);
                                                //double convenexp = 0;
                                                if (dsstaffatt.Tables[1].Rows.Count > 0)
                                                    double.TryParse(Convert.ToString(dsstaffatt.Tables[1].Rows[0]["linkvalue"]), out convenexp);
                                                lunchexp = grosssal - convenexp;
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(lunchexp);
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                newlunchexp = newlunchexp + lunchexp;
                                            }

                                            else
                                            {
                                                if (lstcolorder.Items[lstcol].Text == "NAME")
                                                {
                                                    if (namewithbaslop.Trim() == "1")
                                                    {
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])] + ", BASIC PAY - " + Convert.ToString(basicamnt) + ", LOP DAYS - " + Convert.ToString(totlop));
                                                    }
                                                    else
                                                    {
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                                    }
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                                }
                                                else
                                                {
                                                    if (lstcolorder.Items[lstcol].Text == "BANK NAME" || lstcolorder.Items[lstcol].Text == "BRANCH NAME" || lstcolorder.Items[lstcol].Text == "PAY SCALE" || lstcolorder.Items[lstcol].Text == "COLLEGE BANK")//delsi23
                                                    {
                                                        if (Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]) == "0" || Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]) == "Select")
                                                        {
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = "";
                                                        }
                                                        else
                                                        {
                                                            if (lstcolorder.Items[lstcol].Text == "COLLEGE BANK")
                                                            {
                                                                string coll_Bankcode = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items
[lstcol].Value])]);
                                                                string getName = d2.GetFunction("select BankName from FM_FinBankMaster where CollegeCode='" + collegecode1 + "' and BankPK='" + coll_Bankcode + "'");
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(getName);

                                                            }
                                                            else
                                                            {
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].CellType = txt;

                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);

                                                    }
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);

                                                }
                                            }
                                        }
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "BANK ACCOUNT NO")
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].CellType = txtcell;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "BANK FORMAT")
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].CellType = txtcell;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                        string accno = Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                        if (getvaluedigits.Trim() != "0" && getvaluedigits.Trim() != "")
                                        {
                                            if (accno.Trim() != "" && Convert.ToString(netsal).Trim() != "")
                                            {
                                                string addval = "";
                                                string[] splitdec = new string[2];
                                                string getval = "";
                                                if (netsal.Contains("."))
                                                {
                                                    splitdec = netsal.Split('.');
                                                    getval = Convert.ToString(splitdec[0]);
                                                }
                                                else
                                                {
                                                    getval = Convert.ToString(netsal);
                                                }
                                                if (Convert.ToInt32(getvaluedigits) > (accno.Length + getval.Length))
                                                {
                                                    int remaindig = Convert.ToInt32(getvaluedigits) - (accno.Length + getval.Length);
                                                    if (remaindig > 0)
                                                    {
                                                        Double getav = 0;
                                                        if (remaindig > 0)
                                                        {
                                                            getav = remaindig / 2;
                                                            for (int sla = 0; sla < getav; sla++)
                                                            {
                                                                addval = addval + "0";
                                                            }
                                                        }
                                                        getav = remaindig % 2;
                                                        if (getav == 0)
                                                        {
                                                            addval = accno + addval + getval + addval;
                                                        }
                                                        else
                                                        {
                                                            addval = accno + addval + "0" + getval + addval;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        addval = accno + getval;
                                                    }
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(addval);
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(addval);
                                                }
                                                else
                                                {
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(accno);
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(accno);
                                                }
                                            }
                                            else
                                            {
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(accno);
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(accno);
                                            }
                                        }
                                        else
                                        {
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(accno);
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = Convert.ToString(accno);
                                        }
                                    }
                                    FarPoint.Web.Spread.DateTimeCellType dtcell = new FarPoint.Web.Spread.DateTimeCellType("dd/MM/yyyy");
                                    FarPoint.Web.Spread.TextCellType txts = new FarPoint.Web.Spread.TextCellType();

                                    if (lstcolorder.Items[lstcol].Text == "DATE OF BIRTH")
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].CellType = dtcell;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "DATE OF APPOINTED")
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].CellType = dtcell;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "DATE OF JOINING")
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].CellType = dtcell;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "DATE OF RETIREMENT")
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].CellType = dtcell;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "ACTUAL BASIC")
                                    {
                                        actualbasic = actualbasic + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newactualbasic = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "LOP AMOUNT")
                                    {
                                        lopamt = lopamt + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newlopamt = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "PF Salary")
                                    {
                                        pfcont = pfcont + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newpfcont = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "ESI Salary")
                                    {
                                        esicont = esicont + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newesicont = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "DA %")
                                    {
                                        daper = daper + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newdaper = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "INCREMENT IN RS.")
                                    {
                                        incinrs = incinrs + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newincinrs = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "BASIC PAY Rs.")
                                    {
                                        if (getbas.Trim() != "0" && getbas.Trim() != "0.00" && getbas.Trim() != "")
                                        {
                                            Double.TryParse(getbas, out basamnt);
                                            if (Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]) >= basamnt)
                                            {
                                                basicpay = basicpay + basamnt;
                                                newbasicpay = basamnt;
                                            }
                                            else
                                            {
                                                basicpay = basicpay + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                                newbasicpay = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                            }
                                        }
                                        else
                                        {
                                            basicpay = basicpay + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                            newbasicpay = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        }
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "GROSS PAY Rs.")
                                    {
                                        grosspay = grosspay + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newgrosspay = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "ACTUAL GROSS SALARY")
                                    {
                                        Actgross = Actgross + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newActgross = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "ADV RS.")
                                    {
                                        Advrs = Advrs + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newAdvrs = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "COLLECTED AMT")
                                    {
                                        collectedamt = collectedamt + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newcollectedamt = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "TOT DED Rs.")
                                    {
                                        totded = totded + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newtotded = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "PAY BAND")
                                    {
                                        payband = payband + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newpayband = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "ACTUAL GRADE")
                                    {
                                        actgradeamnt = actgradeamnt + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newactgradeamnt = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "GRADE PAY")
                                    {
                                        gradeamnt = gradeamnt + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newgradeamnt = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "NET PAY")
                                    {
                                        netamnt = netamnt + Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                        newnetamnt = Math.Round((Convert.ToDouble(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])])), 0, MidpointRounding.AwayFromZero);
                                    }

                                    if (lstcolorder.Items[lstcol].Text == "ACTUAL BASIC" || lstcolorder.Items[lstcol].Text == "LOP AMOUNT" || lstcolorder.Items[lstcol].Text == "PF Salary" || lstcolorder.Items[lstcol].Text == "ESI Salary" || lstcolorder.Items[lstcol].Text == "DA %" || lstcolorder.Items[lstcol].Text == "BASIC PAY Rs." || lstcolorder.Items[lstcol].Text == "ACTUAL GROSS SALARY" || lstcolorder.Items[lstcol].Text == "GROSS PAY Rs." || lstcolorder.Items[lstcol].Text == "ADV RS." || lstcolorder.Items[lstcol].Text == "COLLECTED AMT" || lstcolorder.Items[lstcol].Text == "TOT DED Rs." || lstcolorder.Items[lstcol].Text == "PAY BAND" || lstcolorder.Items[lstcol].Text == "GRADE PAY" || lstcolorder.Items[lstcol].Text == "ACTUAL GRADE" || lstcolorder.Items[lstcol].Text == "PAY SCALE" || lstcolorder.Items[lstcol].Text == "NET PAY" || lstcolorder.Items[lstcol].Text == "AGP" || lstcolorder.Items[lstcol].Text == "INCREMENT IN RS.")
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Right;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].CellType = doublecell;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "NAME")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 275;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "DESIGN")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 100;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "DEPT")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 150;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "CATEGORY")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "STAFF TYPE")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "PAY MODE")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "BANK NAME")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].CellType = txts;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "COLLEGE BANK")//delsi2305
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 200;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].CellType = txts;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Left;
                                    }


                                    if (lstcolorder.Items[lstcol].Text == "BRANCH NAME")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "IFSC CODE")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].CellType = txts;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "PF NO")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "ESI NO")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "PAN NO")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "LIC NO")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "AADHAR NO")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 135;

                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].CellType = txts;

                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "LOAN NO")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "GPF NO")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "UAN NO")
                                    {
                                        fpsalary.Sheets[0].Columns[lstcol + 1].Width = 125;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].CellType = txts;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (lstcolorder.Items[lstcol].Text == "DATE OF BIRTH" || lstcolorder.Items[lstcol].Text == "DATE OF APPOINTED" || lstcolorder.Items[lstcol].Text == "DATE OF JOINING" || lstcolorder.Items[lstcol].Text == "DATE OF INCREMENT" || lstcolorder.Items[lstcol].Text == "DATE OF RETIREMENT" || lstcolorder.Items[lstcol].Text == "NO. OF INCREMENT")
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].HorizontalAlign = HorizontalAlign.Center;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, lstcol + 1].Font.Name = "Book Antiqua";
                                    }
                                }
                            }
                            else
                            {
                                if (lstcolorder.Items[lstcol].Text == "SELECT")
                                {
                                    int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                                    fpsalary.Sheets[0].Cells[0, colidx].CellType = chkcom;
                                    fpsalary.Sheets[0].Cells[0, colidx].HorizontalAlign = HorizontalAlign.Center;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].CellType = chkcell1;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Value = 0;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Center;
                                    postbackcount++;
                                }
                                if (lstcolorder.Items[lstcol].Text == "LOP DAYS")
                                {
                                    int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = totlop.ToString();
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Center;
                                    Session["lopdays"] = totlop;
                                }
                                if (lstcolorder.Items[lstcol].Text == "LOP DATES")
                                {
                                    datefrom3 = split5[1].ToString() + "/" + split5[0].ToString() + "/" + split5[2].ToString();
                                    year3 = Convert.ToInt16(split5[2]);
                                    string date4 = todate.ToString();
                                    string[] split6 = date4.Split(new Char[] { '/' });
                                    string dateto4 = split6[1].ToString() + "/" + split6[0].ToString() + "/" + split6[2].ToString();
                                    monthname2 = split6[0].ToString();
                                    monthnamenum = Convert.ToInt32(monthname2.ToString());
                                    yearto = split1[2].ToString();
                                    int fromday = 0;
                                    fromday = Convert.ToInt16(split6[0].ToString());
                                    string mnmae = split5[1].ToString();
                                    monthname1 = Convert.ToInt16(mnmae);
                                    monthname2 = split1[1].ToString();
                                    monthnamenum = Convert.ToInt16(monthname2);
                                    string year = split5[2].ToString();
                                    monyear = monthname1.ToString() + "/" + year;
                                    monyearto = monthnamenum.ToString() + "/" + yearto;
                                    string dayfrom;
                                    int today = 0;
                                    today = Convert.ToInt16(split6[0].ToString());
                                    dayfrom = split5[0].ToString();
                                    dayfrm = Convert.ToInt32(dayfrom);
                                    dayto = split6[0].ToString();
                                    daytonum = Convert.ToInt32(dayto);
                                    double lopdaysmor = 0;
                                    double lopdayseveg = 0;
                                    string date = "";
                                    int morpresent = 0;
                                    int evepresent = 0;
                                    int maxdays = getmaxdays(monthname1, year3);
                                    int absentdays = 0;
                                    if (monthname1 == monthnamenum)
                                    {
                                        dsstaffatt.Tables[0].DefaultView.RowFilter = " staff_code='" + staffcode1 + "' and mon_year='" + monyear + "'";
                                        DataView dvstaffatt = dsstaffatt.Tables[0].DefaultView;
                                        if (dvstaffatt.Count > 0)
                                        {
                                            for (int day = dayfrm; dayfrm <= daytonum; dayfrm++)
                                            {
                                                int boll4 = 0;
                                                int day5 = 0;
                                                day5 = 0 + dayfrm;
                                                int day6;
                                                string attnda = "";
                                                day6 = 3 + day5;
                                                attnda = dvstaffatt[0][day6].ToString();
                                                if (attnda != "")
                                                {
                                                    string[] split100 = attnda.Split(new char[] { '-' });
                                                    string mor = "";
                                                    string eve = "";
                                                    mor = split100[0].ToString();
                                                    eve = split100[1].ToString();
                                                    if ((mor.Trim() == "A"))
                                                    {
                                                        absentdays++;
                                                        lopdaysmor = lopdaysmor + 0.5;
                                                        boll4 = 1;
                                                    }
                                                    else
                                                    {
                                                        if (addleave.Count > 0)
                                                        {
                                                            if (addleave.Contains(mor) == true)
                                                            {
                                                                lopdaysmor = lopdaysmor + 0.5;
                                                                boll4 = 1;
                                                            }
                                                        }
                                                        if (hatpre.Contains(mor))
                                                        {
                                                            morpresent = morpresent + 1;
                                                            Present = Present + 0.5;
                                                        }
                                                    }
                                                    if ((eve.Trim() == "A"))
                                                    {
                                                        absentdays++;
                                                        lopdayseveg = lopdayseveg + 0.5;
                                                    }
                                                    else
                                                    {
                                                        if (addleave.Count > 0)
                                                        {
                                                            if (addleave.Contains(eve) == true)
                                                            {
                                                                lopdayseveg = lopdayseveg + 0.5;
                                                                boll4 = 1;
                                                            }
                                                        }
                                                        if (hatpre.Contains(eve))
                                                        {
                                                            evepresent = evepresent + 1;
                                                            Present = Present + 0.5;
                                                        }
                                                    }
                                                    //if ((mor.Trim() == "P"))
                                                    //{
                                                    //    Present = Present + 0.5;
                                                    //}
                                                    //if ((eve.Trim() == "P"))
                                                    //{
                                                    //    Present = Present + 0.5;
                                                    //}
                                                    if (boll4 == 1)
                                                    {
                                                        if (date == "")
                                                        {
                                                            date = day5 + "/" + monyear;
                                                        }
                                                        else
                                                        {
                                                            date = date + "," + day5 + "/" + monyear;
                                                        }
                                                    }
                                                    string[] lave = new string[15];
                                                }
                                            }
                                        }
                                        int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = date;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Tag = date;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Center;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Locked = true;
                                        absdays = Convert.ToDouble(absentdays / 2);
                                        Session["absent"] = Convert.ToString(absdays);
                                        presentdays = Present;
                                        Session["present"] = Convert.ToString(presentdays);
                                    }
                                    else
                                    {
                                        dsstaffatt.Tables[0].DefaultView.RowFilter = " mon_year= '" + monyear + "' and staff_code='" + staffcode1 + "'";
                                        DataView dvstaffatt = dsstaffatt.Tables[0].DefaultView;
                                        if (dvstaffatt.Count > 0)
                                        {
                                            for (int day = dayfrm; dayfrm <= maxdays; dayfrm++)
                                            {
                                                //fpsalary.Sheets[0].Columns[10].Visible = true;
                                                //fpsalary.Sheets[0].Columns[11].Visible = true;
                                                //fpsalary.Sheets[0].Columns[9].Visible = false;
                                                int day5 = 0;
                                                day5 = 0 + dayfrm;
                                                int bol = 0;
                                                int day6;
                                                string attnda = "";
                                                day6 = 3 + day5;
                                                attnda = dvstaffatt[0][day6].ToString();
                                                if (attnda != "")
                                                {
                                                    string[] split100 = attnda.Split(new char[] { '-' });
                                                    string mor = "";
                                                    string eve = "";
                                                    mor = split100[0].ToString();
                                                    eve = split100[1].ToString();
                                                    if ((mor.Trim() == "A"))
                                                    {
                                                        absentdays++;
                                                        lopdaysmor = lopdaysmor + 0.5;
                                                        bol = 1;
                                                    }
                                                    else
                                                    {
                                                        if (addleave.Count > 0)
                                                        {
                                                            if (addleave.Contains(mor) == true)
                                                            {
                                                                lopdaysmor = lopdaysmor + 0.5;
                                                                bol = 1;
                                                            }
                                                        }
                                                        if (hatpre.Contains(mor))
                                                        {
                                                            morpresent = morpresent + 1;
                                                            Present = Present + 0.5;
                                                        }
                                                    }
                                                    if ((eve.Trim() == "A"))
                                                    {
                                                        absentdays++;
                                                        lopdayseveg = lopdayseveg + 0.5;
                                                        bol = 1;
                                                    }
                                                    else
                                                    {
                                                        if (addleave.Count > 0)
                                                        {
                                                            if (addleave.Contains(eve) == true)
                                                            {
                                                                lopdayseveg = lopdayseveg + 0.5;
                                                                bol = 1;
                                                            }
                                                        }
                                                        if (hatpre.Contains(eve))
                                                        {
                                                            evepresent = evepresent + 1;
                                                            Present = Present + 0.5;
                                                        }
                                                    }
                                                    if (bol == 1)
                                                    {
                                                        if (date == "")
                                                        {
                                                            date = day5 + "/" + monyear;
                                                        }
                                                        else
                                                        {
                                                            date = date + "," + day5 + "/" + monyear;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        lopdayseveg = 0;
                                        lopdaysmor = 0;
                                        dsstaffatt.Tables[0].DefaultView.RowFilter = " mon_year= '" + monyearto + "' and staff_code='" + staffcode1 + "'";
                                        dvstaffatt = dsstaffatt.Tables[0].DefaultView;
                                        if (dvstaffatt.Count > 0)
                                        {
                                            for (int day = 1; day <= today; day++)
                                            {
                                                int day5 = 0;
                                                day5 = 0 + day;
                                                int day6;
                                                int bol2 = 0;
                                                string attnda = "";
                                                day6 = 3 + day5;
                                                attnda = dvstaffatt[0][day6].ToString();
                                                if (attnda != "")
                                                {
                                                    string[] split100 = attnda.Split(new char[] { '-' });
                                                    string mor = "";
                                                    string eve = "";
                                                    mor = split100[0].ToString();
                                                    eve = split100[1].ToString();
                                                    if ((mor.Trim() == "A"))
                                                    {
                                                        absentdays++;
                                                        lopdaysmor = lopdaysmor + 0.5;
                                                        bol2 = 1;
                                                    }
                                                    else
                                                    {
                                                        if (addleave.Count > 0)
                                                        {
                                                            if (addleave.Contains(mor) == true)
                                                            {
                                                                lopdaysmor = lopdaysmor + 0.5;
                                                                bol2 = 1;
                                                            }
                                                        }
                                                        if (hatpre.Contains(mor))
                                                        {
                                                            morpresent = morpresent + 1;
                                                            Present = Present + 0.5;
                                                        }
                                                    }
                                                    if ((eve.Trim() == "A"))
                                                    {
                                                        absentdays++;
                                                        lopdayseveg = lopdayseveg + 0.5;
                                                        bol2 = 1;
                                                    }
                                                    else
                                                    {
                                                        if (addleave.Count > 0)
                                                        {
                                                            if (addleave.Contains(eve) == true)
                                                            {
                                                                lopdayseveg = lopdayseveg + 0.5;
                                                                bol2 = 1;
                                                            }
                                                        }
                                                        if (hatpre.Contains(eve))
                                                        {
                                                            evepresent = evepresent + 1;
                                                            Present = Present + 0.5;
                                                        }
                                                    }
                                                    if (bol2 == 1)
                                                    {
                                                        if (date == "")
                                                        {
                                                            date = day5 + "/" + monyearto;
                                                        }
                                                        else
                                                        {
                                                            date = date + "," + day5 + "/" + monyearto;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = date;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Center;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Locked = true;
                                        absdays = Convert.ToDouble(absentdays / 2);
                                        Session["absent"] = Convert.ToString(absdays);
                                        presentdays = Present;
                                        Session["present"] = Convert.ToString(presentdays);
                                    }
                                }
                                if (lstcolorder.Items[lstcol].Text == "WORKING DAYS")
                                {
                                    int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = workingdays;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Center;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                }


                                if (lstcolorder.Items[lstcol].Text == "One Day Salary")//delsi01/05/2018
                                {
                                    int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(oneday_sal);
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Center;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                }
                                if (lstcolorder.Items[lstcol].Text == "COLLECTED AMT")
                                {
                                    int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                                    int check = 0;
                                    string collect_amt = "";
                                    string common_value = "";
                                    collect_amt = "select * from stafftrans where staff_code = '" + staffcode1 + "' and latestrec = '1'";
                                    SqlDataAdapter dec_Collect = new SqlDataAdapter(collect_amt, con1);
                                    DataSet dsload2 = new DataSet();
                                    dec_Collect.Fill(dsload2);
                                    if (dsload2.Tables[0].Rows.Count > 0)
                                    {
                                        collect_amount = 0;
                                        for (int i2 = 0; i2 < dsload2.Tables[0].Rows.Count; i2++)
                                        {
                                            string staff_collect_amt = "";
                                            staff_collect_amt = dsload2.Tables[0].Rows[i2]["deductions"].ToString();
                                            string[] spli_staff_collect_amt = staff_collect_amt.Split('\\');
                                            int count = 0;
                                            count = spli_staff_collect_amt.Length;
                                            for (int i = 0; i < count; i++)
                                            {
                                                string final_month_Collect = spli_staff_collect_amt[i];
                                                string[] spli_salary_Amt = final_month_Collect.Split(';');
                                                if (check == 0)
                                                {
                                                    if (spli_salary_Amt.Length > 10)
                                                    {
                                                        if (spli_salary_Amt[10] != "0" && spli_salary_Amt[10] != "")
                                                        {
                                                            common_value = spli_salary_Amt[0];
                                                            check++;
                                                            string collect_New_Amt = "";
                                                            collect_New_Amt = "select * from monthlypay where staff_code = '" + staffcode1 + "' and latestrec = '0' and fdate < '" + datefrom + "'";
                                                            DataSet dsload3 = new DataSet();
                                                            SqlDataAdapter dec_Collect1 = new SqlDataAdapter(collect_New_Amt, con1);
                                                            dec_Collect1.Fill(dsload3);
                                                            if (dsload3.Tables[0].Rows.Count > 0)
                                                            {
                                                                int ch = 0;
                                                                for (int i1 = 0; i1 < dsload3.Tables[0].Rows.Count; i1++)
                                                                {
                                                                    string staff_amount = "";
                                                                    staff_amount = dsload3.Tables[0].Rows[i1]["deductions"].ToString();
                                                                    string[] spli_staff_collect_amt_new = staff_amount.Split('\\');
                                                                    int length_spli_value = 0;
                                                                    length_spli_value = spli_staff_collect_amt_new.Length;
                                                                    for (int k = 0; k < length_spli_value; k++)
                                                                    {
                                                                        string final_month_Collect_amt = spli_staff_collect_amt_new[k];
                                                                        string[] spli_salary_Amt_Final = final_month_Collect_amt.Split(';');
                                                                        if (common_value == spli_salary_Amt_Final[0])
                                                                        {
                                                                            if (ch == 0)
                                                                            {
                                                                                ch++;
                                                                                collect_amount = Convert.ToDouble(spli_salary_Amt_Final[3]);
                                                                            }
                                                                            else
                                                                            {
                                                                                collect_amount = collect_amount + Convert.ToDouble(spli_salary_Amt_Final[3]);
                                                                            }
                                                                            collect_amount = Math.Round(collect_amount, 0, MidpointRounding.AwayFromZero);
                                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = collect_amount.ToString();
                                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Locked = true;
                                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = "0";
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Locked = true;
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = "0";
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Locked = true;
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                                    }
                                                }
                                                else
                                                {
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = "0";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Locked = true;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                    }
                                }
                                if (lstcolorder.Items[lstcol].Text == "ABSENT DAYS")
                                {
                                    int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                                    //if (newabsdays.Trim() != "")
                                    //{
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = newabsdays;
                                    //}
                                    //else
                                    //{
                                    //    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(Session["absent"]);
                                    //}//Convert.ToString(Session["absent"]);
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Locked = true;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Center;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                }
                                if (lstcolorder.Items[lstcol].Text == "PRESENT DAYS")
                                {
                                    int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                                    //if (newpresdays.Trim() != "")
                                    //{
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = newpresdays;
                                    //}
                                    //else
                                    //{
                                    //    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(Session["present"]);
                                    //}//Convert.ToString(Session["present"]);
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Locked = true;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Center;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                                }
                            }
                            if (lstcolorder.Items[lstcol].Text == "ACTUAL GROSS SALARY")
                            {
                                int NewIndex = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                                Double.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["netaddact"]), out grosssal);
                                grosssal = Math.Round(grosssal, 0, MidpointRounding.AwayFromZero);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Text = Convert.ToString(grosssal);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].HorizontalAlign = HorizontalAlign.Right;
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Font.Name = "Book Antiqua";
                                if (grosssal == 0) // poo 25.10.17
                                    convenexp = 0;
                            }
                            if (lstcolorder.Items[lstcol].Text == "LOP AMOUNT")
                            {
                                int NewIndex = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                                Double.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["Tot_LOP"]), out totdeduct);
                                totdeduct = Math.Round(totdeduct, 0, MidpointRounding.AwayFromZero);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Text = Convert.ToString(totdeduct);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].HorizontalAlign = HorizontalAlign.Right;
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Font.Name = "Book Antiqua";
                            }
                            if (lstcolorder.Items[lstcol].Text == "TOT DED Rs.")
                            {
                                int NewIndex = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                                Double.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["netded"]), out totdeduct);
                                totdeduct = Math.Round(totdeduct, 0, MidpointRounding.AwayFromZero);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Text = Convert.ToString(totdeduct);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].HorizontalAlign = HorizontalAlign.Right;
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Font.Name = "Book Antiqua";
                            }
                            if (lstcolorder.Items[lstcol].Text == "NET PAY")
                            {
                                int NewIndex = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                                Double.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["netsal"]), out netsalary);
                                netsalary = Math.Round(netsalary, 0, MidpointRounding.AwayFromZero);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Text = Convert.ToString(netsalary);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].HorizontalAlign = HorizontalAlign.Right;
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Font.Name = "Book Antiqua";
                            }

                            if (lstcolorder.Items[lstcol].Text == "HOUR AMT")
                            {
                                //8 X4 =32 X 250
                                int NewIndex = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                                int totalhours = 0;
                                int.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["tot_hrs"]), out totalhours);
                                int total = totalhours * 4;
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Text = totalhours + " X 4 = " + total + " X " + Convert.ToString(dsset.Tables[0].Rows[sal][Convert.ToString(getcol[lstcolorder.Items[lstcol].Value])]);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].HorizontalAlign = HorizontalAlign.Right;
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Font.Name = "Book Antiqua";
                            }

                            //if (lstcolorder.Items[lstcol].Text == "CONVENES EXP")
                            //{
                            //    int NewIndex = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                            //    double convenexp = 0;
                            //    if (dsstaffatt.Tables[1].Rows.Count > 0)
                            //        double.TryParse(Convert.ToString(dsstaffatt.Tables[1].Rows[0]["linkvalue"]), out convenexp);
                            //    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Text = Convert.ToString(convenexp);
                            //    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].HorizontalAlign = HorizontalAlign.Right;
                            //    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Font.Name = "Book Antiqua";
                            //}
                            //if (lstcolorder.Items[lstcol].Text == "LUNCH EXP")
                            //{
                            //    int NewIndex = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                            //    Double.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["netaddact"]), out grosssal);
                            //    double convenexp = 0;
                            //    if (dsstaffatt.Tables[1].Rows.Count > 0)
                            //        double.TryParse(Convert.ToString(dsstaffatt.Tables[1].Rows[0]["linkvalue"]), out convenexp);
                            //    double lunchexp = grosssal - convenexp;
                            //    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Text = Convert.ToString(lunchexp);
                            //    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].HorizontalAlign = HorizontalAlign.Right;
                            //    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, NewIndex].Font.Name = "Book Antiqua";delsi
                            //}



                        }



                        DataSet dslev = new DataSet();
                        Hashtable hslevacr = new Hashtable();
                        hslevacr.Clear();
                        string[] splval = new string[2];
                        int levacrcount = 0;
                        int myLeaveCount = 0;
                        string getlevidx = "";
                        double totlevcount = 0;
                        if (cbleavecat.Checked == true)
                        {
                            if (txtleavecat.Text.Trim() != "--Select--")
                            {
                                DateTime dfTemp = new DateTime();
                                DateTime dtTemp = new DateTime();
                                string newmonyear = Convert.ToString(ddl_mon.SelectedItem.Value) + "/" + Convert.ToString(ddl_year.SelectedItem.Text);
                                string selq = "select * from staff_attnd where mon_year='" + newmonyear + "' and staff_code='" + staffcode1 + "'";
                                dfTemp = dtf;
                                dtTemp = dtt;
                                dslev.Clear();
                                dslev = d2.select_method_wo_parameter(selq, "Text");
                                if (dslev.Tables.Count > 0 && dslev.Tables[0].Rows.Count > 0)
                                {
                                    while (dfTemp <= dtTemp)
                                    {
                                        string getdayval = d2.GetFunction("select [" + dfTemp.Day + "] from staff_attnd where mon_year='" + newmonyear + "' and staff_code='" + staffcode1 + "'");
                                        if (getdayval.Trim() != "" && !getdayval.Contains('-'))
                                        {
                                            if (getdayval.Trim() != "")
                                            {
                                                if (hslevacr.ContainsKey(getdayval))
                                                {
                                                    Int32.TryParse(Convert.ToString(hslevacr[getdayval]), out myLeaveCount);
                                                    myLeaveCount = myLeaveCount + 1;
                                                    hslevacr.Remove(getdayval);
                                                    hslevacr.Add(getdayval, Convert.ToString(myLeaveCount));
                                                }
                                                if (!hslevacr.ContainsKey(getdayval))
                                                {
                                                    hslevacr.Add(getdayval, Convert.ToString(1));
                                                }
                                            }
                                        }
                                        if (getdayval.Trim() != "" && getdayval.Contains('-'))
                                        {
                                            splval = getdayval.Split('-');
                                            if (splval[0].Trim() != "")
                                            {
                                                if (hslevacr.ContainsKey(splval[0]))
                                                {
                                                    Int32.TryParse(Convert.ToString(hslevacr[splval[0]]), out myLeaveCount);
                                                    myLeaveCount = myLeaveCount + 1;
                                                    hslevacr.Remove(splval[0]);
                                                    hslevacr.Add(splval[0], Convert.ToString(myLeaveCount));
                                                }
                                                if (!hslevacr.ContainsKey(splval[0]))
                                                {
                                                    hslevacr.Add(splval[0], Convert.ToString(1));
                                                }
                                            }
                                            if (splval[1].Trim() != "")
                                            {
                                                if (hslevacr.ContainsKey(splval[1]))
                                                {
                                                    Int32.TryParse(Convert.ToString(hslevacr[splval[1]]), out myLeaveCount);
                                                    myLeaveCount = myLeaveCount + 1;
                                                    hslevacr.Remove(splval[1]);
                                                    hslevacr.Add(splval[1], Convert.ToString(myLeaveCount));
                                                }
                                                if (!hslevacr.ContainsKey(splval[1]))
                                                {
                                                    hslevacr.Add(splval[1], Convert.ToString(1));
                                                }
                                            }
                                        }
                                        dfTemp = dfTemp.AddDays(1);
                                    }
                                    foreach (DictionaryEntry dr in hslevacr)
                                    {
                                        if (LeaveHash.ContainsKey(dr.Key))
                                        {
                                            getlevidx = Convert.ToString(LeaveHash[dr.Key]);
                                            double.TryParse(Convert.ToString(dr.Value), out totlevcount);
                                            totlevcount = totlevcount / 2;
                                            totlevcount = Math.Round(totlevcount, 1, MidpointRounding.AwayFromZero);
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(getlevidx)].Text = Convert.ToString(totlevcount);
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(getlevidx)].HorizontalAlign = HorizontalAlign.Center;
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(getlevidx)].Font.Name = "Book Antiqua";
                                        }
                                    }
                                    foreach (DictionaryEntry dr in LeaveHash)
                                    {
                                        if (!hslevacr.ContainsKey(dr.Key))
                                        {
                                            getlevidx = Convert.ToString(LeaveHash[dr.Key]);
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(getlevidx)].Text = Convert.ToString("0");
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(getlevidx)].HorizontalAlign = HorizontalAlign.Center;
                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(getlevidx)].Font.Name = "Book Antiqua";
                                        }
                                    }
                                }
                            }
                        }
                        rowcheck = false;
                        if (cb_all.Checked == true)
                        {
                            if (txt_allow.Text.Trim() != "--Select--")
                            {
                                Double allamnt = 0;
                                Dictionary<string, string> dicactaloo = new Dictionary<string, string>();
                                dsset.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode1 + "'";
                                DataView dvallow = dsset.Tables[0].DefaultView;
                                if (dvallow.Count > 0)
                                {
                                    string actall = dvallow[0]["allowances"].ToString();
                                    string[] spactalo = actall.Split('\\');
                                    for (int i = 0; i < spactalo.GetUpperBound(0); i++)
                                    {
                                        string getactal = spactalo[i];
                                        if (getactal.Trim() != "" && getactal != null)
                                        {
                                            string[] actallspv = getactal.Split(';');
                                            if (actallspv.GetUpperBound(0) >= 8)
                                            {
                                                if (actallspv[1].ToString().Trim().ToLower() == "percent")
                                                {
                                                    Double getrealsalry = 0;
                                                    Double getperce = 0;
                                                    string getval = "0";
                                                    if (actallspv[6].ToString().Trim() == "1")
                                                    {
                                                        if (actu_basicpay.Trim() != "" && actu_basicpay != null)
                                                        {
                                                            getrealsalry = Convert.ToDouble(actu_basicpay);
                                                            getrealsalry = Math.Round(getrealsalry, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        if (actallspv[2].ToString().Trim() != "" && actallspv[2].ToString() != null)
                                                        {
                                                            string[] getnew = actallspv[2].Split('-');
                                                            if (getnew.Length > 0)
                                                            {
                                                                if (getnew[0].Trim() != "")
                                                                {
                                                                    getperce = Convert.ToDouble(getnew[0].ToString());
                                                                }
                                                            }
                                                            getperce = Math.Round(getperce, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        Double val = getrealsalry / 100 * getperce;
                                                        val = Math.Round(val, 0, MidpointRounding.AwayFromZero);
                                                        getval = val.ToString();
                                                    }
                                                    else if (actallspv[8].ToString().Trim() == "1")
                                                    {
                                                        if (actu_basicpay.Trim() != "" && actu_basicpay != null)
                                                        {
                                                            getrealsalry = Convert.ToDouble(actu_basicpay);
                                                            getrealsalry = Math.Round(getrealsalry, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        if (gradepay.Trim() != "" && gradepay != null)
                                                        {
                                                            getrealsalry = getrealsalry + Convert.ToDouble(gradepay);
                                                            getrealsalry = Math.Round(getrealsalry, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        if (actallspv[2].ToString().Trim() != "" && actallspv[2].ToString() != null)
                                                        {
                                                            string[] getnew = actallspv[2].Split('-');
                                                            if (getnew.Length > 0)
                                                            {
                                                                if (getnew[0].Trim() != "")
                                                                {
                                                                    getperce = Convert.ToDouble(getnew[0].ToString());
                                                                }
                                                            }
                                                            getperce = Math.Round(getperce, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        Double val = getrealsalry / 100 * getperce;
                                                        val = Math.Round(val, 0, MidpointRounding.AwayFromZero);
                                                        getval = val.ToString();
                                                    }
                                                    if (!dicactaloo.ContainsKey(actallspv[0].ToString()))
                                                    {
                                                        dicactaloo.Add(actallspv[0].ToString().Trim().ToLower(), getval);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                allowance = dsset.Tables[0].Rows[sal]["allowances"].ToString();
                                string[] allowanmce_arr1;
                                string alowancesplit;
                                double dblallowact2 = 0;
                                rowviscount++;
                                dedrowviscount++;
                                allowanmce_arr1 = allowance.Split('\\');
                                for (int i = 0; i < allowanmce_arr1.GetUpperBound(0); i++)
                                {
                                    alowancesplit = allowanmce_arr1[i];
                                    string[] allowanceda;
                                    allowanceda = alowancesplit.Split(';');
                                    string da;
                                    string da1 = "";
                                    string da3 = "";
                                    double ds4 = 0;
                                    string da2;
                                    double ds3 = 0;
                                    double ds2 = 0;
                                    string dblallowact;
                                    double dblallowact1 = 0;
                                    dblallowact2 = 0;
                                    da = allowanceda[0];
                                    da1 = allowanceda[1];
                                    da2 = allowanceda[2];
                                    dblallowact = allowanceda[3];
                                    if (da2.Trim() != "")
                                    {
                                        string[] spval = da2.Split('-');
                                        if (spval.Length == 2)
                                        {
                                            //if (da1.Trim().ToUpper() == "PERCENT")
                                            //    da3 = spval[1];
                                            //else
                                            //    da3 = Convert.ToString(dblallowact); //spval[0]; 04.09.17 bb
                                            if (da1.Trim().ToUpper() == "PERCENT")//26.12.17 barath
                                                da3 = Convert.ToString(spval[1]);
                                            else if (da1.Trim().ToUpper() == "SLAB")
                                                da3 = Convert.ToString(spval[1]);
                                            else
                                                da3 = Convert.ToString(spval[0]);
                                        }
                                        else
                                        {
                                            da3 = Convert.ToString(dblallowact);
                                        }
                                        da2 = spval[0];
                                        if (da2.Trim() == "" || da2 == null)
                                        {
                                            da2 = "0";
                                        }
                                    }
                                    double.TryParse(da3, out ds4);
                                    double.TryParse(dblallowact, out dblallowact1);
                                    dblallowact1 = Math.Round(dblallowact1, 0);
                                    dblallowact2 = dblallowact2 + (ds3 - dblallowact1);
                                    //da = da.ToUpper();
                                    string GetIndex = Convert.ToString(AllowanceHash[da]);//delsi
                                    if (GetIndex.Trim() != "")
                                    {
                                        if (dicactaloo.Count > 0)
                                        {
                                            if (dicactaloo.ContainsKey(da.ToString().Trim().ToLower()))
                                            {
                                                if (chk_amnt.Checked == true)
                                                {
                                                    Double.TryParse(Convert.ToString(dicactaloo[da.ToString().Trim().ToLower()]), out allamnt);
                                                    allamnt = Math.Round(allamnt, 0, MidpointRounding.AwayFromZero);
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(allamnt);
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                }
                                                else
                                                {
                                                    if (dicactaloo[da.ToString().Trim().ToLower()] == "0" || dicactaloo[da.ToString().Trim().ToLower()] == "" && rowcheck == false)
                                                    {
                                                        fpsalary.Sheets[0].Rows[fpsalary.Sheets[0].RowCount - 1].Visible = false;
                                                        rowcheck = true;
                                                        rowviscount--;
                                                        allrowcount++;
                                                    }
                                                    else
                                                    {
                                                        Double.TryParse(Convert.ToString(dicactaloo[da.ToString().Trim().ToLower()]), out allamnt);
                                                        allamnt = Math.Round(allamnt, 0, MidpointRounding.AwayFromZero);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(allamnt);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (chk_amnt.Checked == true)
                                            {
                                                Double.TryParse(Convert.ToString(da3), out allamnt);
                                                allamnt = Math.Round(allamnt, 0, MidpointRounding.AwayFromZero);
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(allamnt);
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                            }
                                            else
                                            {
                                                if (Convert.ToString(ds3) == "0" || Convert.ToString(ds3) == "" && rowcheck == false)
                                                {
                                                    fpsalary.Sheets[0].Rows[fpsalary.Sheets[0].RowCount - 1].Visible = false;
                                                    rowcheck = true;
                                                    rowviscount--;
                                                    allrowcount++;
                                                }
                                                else
                                                {
                                                    Double.TryParse(Convert.ToString(da3), out allamnt);
                                                    allamnt = Math.Round(allamnt, 0, MidpointRounding.AwayFromZero);
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(allamnt);
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        dedrowcheck = false;
                        if (cb_deduct.Checked == true)
                        {
                            if (txt_ded.Text.Trim() != "--Select--")
                            {
                                double dedamnt = 0;
                                Dictionary<string, string> dicactded = new Dictionary<string, string>();
                                dsset.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode1 + "'";
                                DataView dvallow = dsset.Tables[0].DefaultView;
                                if (dvallow.Count > 0)
                                {
                                    string actall = dvallow[0]["deductions"].ToString();
                                    string[] spactalo = actall.Split('\\');
                                    for (int i = 0; i < spactalo.GetUpperBound(0); i++)
                                    {
                                        string getactal = spactalo[i];
                                        if (getactal.Trim() != "" && getactal != null)
                                        {
                                            string[] actallspv = getactal.Split(';');
                                            if (actallspv.GetUpperBound(0) >= 8)
                                            {
                                                if (actallspv[1].ToString().Trim().ToLower() == "percent")
                                                {
                                                    Double getrealsalry = 0;
                                                    Double getperce = 0;
                                                    string getval = "0";
                                                    if (actallspv[6].ToString().Trim() == "1")
                                                    {
                                                        if (actu_basicpay.Trim() != "" && actu_basicpay != null)
                                                        {
                                                            //getrealsalry = Convert.ToDouble(actu_basicpay);
                                                            double.TryParse(actu_basicpay, out getrealsalry);
                                                            getrealsalry = Math.Round(getrealsalry, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        if (actallspv[2].ToString().Trim() != "" && actallspv[2].ToString() != null)
                                                        {
                                                            //getperce = Convert.ToDouble(actallspv[2].ToString());
                                                            double.TryParse(actallspv[2].ToString(), out getperce);
                                                            getperce = Math.Round(getperce, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        Double val = getrealsalry / 100 * getperce;
                                                        val = Math.Round(val, 0, MidpointRounding.AwayFromZero);
                                                        getval = val.ToString();
                                                    }
                                                    else if (actallspv[8].ToString().Trim() == "1")
                                                    {
                                                        if (actu_basicpay.Trim() != "" && actu_basicpay != null)
                                                        {
                                                            //getrealsalry = Convert.ToDouble(actu_basicpay);
                                                            double.TryParse(actu_basicpay, out getrealsalry);
                                                            getrealsalry = Math.Round(getrealsalry, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        if (gradepay.Trim() != "" && gradepay != null)
                                                        {
                                                            getrealsalry = getrealsalry + Convert.ToDouble(gradepay);
                                                            getrealsalry = Math.Round(getrealsalry, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        if (actallspv[2].ToString().Trim() != "" && actallspv[2].ToString() != null)
                                                        {
                                                            getperce = Convert.ToDouble(actallspv[2].ToString());
                                                            getperce = Math.Round(getperce, 0, MidpointRounding.AwayFromZero);
                                                        }
                                                        Double val = getrealsalry / 100 * getperce;
                                                        val = Math.Round(val, 0, MidpointRounding.AwayFromZero);
                                                        getval = val.ToString();
                                                    }
                                                    if (!dicactded.ContainsKey(actallspv[0].ToString()))
                                                    {
                                                        dicactded.Add(actallspv[0].ToString().Trim().ToLower(), getval);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                string deduction = "";
                                deduction = dsset.Tables[0].Rows[sal]["deductions"].ToString();
                                string[] deduction_arr1;
                                string deductionsplit1;
                                deduction_arr1 = deduction.Split('\\');
                                string de2 = "";
                                for (int sp = 0; sp < deduction_arr1.GetUpperBound(0); sp++)
                                {
                                    deductionsplit1 = deduction_arr1[sp];
                                    string[] deductionda;
                                    deductionda = deductionsplit1.Split(';');
                                    string de;
                                    string de1 = "";
                                    string de3 = "";
                                    de = deductionda[0];
                                    de1 = deductionda[1];
                                    de2 = deductionda[2];
                                    if (de2.Trim() != "")
                                    {
                                        string[] dedspl = de2.Split('-');
                                        if (dedspl.Length == 2)
                                        {
                                            if (de1.Trim().ToUpper() == "PERCENT")
                                                de3 = Convert.ToString(dedspl[1]);
                                            else if (de1.Trim().ToUpper() == "SLAB")
                                                de3 = Convert.ToString(dedspl[1]);
                                            else
                                                de3 = Convert.ToString(dedspl[0]);
                                        }
                                        else
                                            de3 = Convert.ToString(deductionda[3]);
                                    }
                                    //de3 = deductionda[2];
                                    string GetIndex = "";
                                    //string loandet = "";
                                    string loanvalue = "";
                                    Dictionary<string, string> dicloandet = new Dictionary<string, string>();
                                    dicloandet.Clear();
                                    GetIndex = Convert.ToString(DeducationHash[de]);
                                    //string selpaidcount = "";
                                    string selpaytotmon = "select LoanCode,PayTotMon from staffloandet where isactive = 1 and isclose = 0 and staff_code = '" + staffcode1 + "' and dedname = '" + de.ToString() + "' and LoanType='0'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(selpaytotmon, "Text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        for (int lk = 0; lk < ds.Tables[0].Rows.Count; lk++)
                                        {
                                            selpaidcount = d2.GetFunction("select count(staff_code) as paidcount from staffloanpaydet where LoanCode='" + Convert.ToString(ds.Tables[0].Rows[lk]["LoanCode"]) + "' and Staff_Code='" + staffcode1 + "'");
                                            loandet = Convert.ToString(ds.Tables[0].Rows[lk]["PayTotMon"]);
                                            if (!dicloandet.ContainsKey(Convert.ToString(ds.Tables[0].Rows[lk]["LoanCode"])))
                                            {
                                                //tenure = "(" + selpaidcount + "/" + loandet + ")";//poo 28.10.17
                                                dicloandet.Add(Convert.ToString(ds.Tables[0].Rows[lk]["LoanCode"]), "(" + selpaidcount + "/" + loandet + ")");
                                            }
                                        }
                                    }
                                    if (GetIndex.Trim() != "")
                                    {
                                        if (dicactded.Count > 0)
                                        {
                                            if (dicactded.ContainsKey(de.ToString().Trim().ToLower()))
                                            {
                                                if (chk_amnt.Checked == true)
                                                {
                                                    if (chk_loandet.Checked == true)
                                                    {
                                                        if (selpaidcount.Trim() != "0" && selpaidcount.Trim() != "")
                                                        {
                                                            foreach (KeyValuePair<string, string> dr in dicloandet)
                                                            {
                                                                if (de3.ToString() != "0" && de3.ToString() != "")
                                                                    if (loanvalue.Trim() == "")
                                                                        loanvalue = dicactded[de.ToString().Trim().ToLower()] + dr.Value;
                                                                    else
                                                                        loanvalue = loanvalue + " , " + dicactded[de.ToString().Trim().ToLower()] + dr.Value;
                                                            }
                                                            if (string.IsNullOrEmpty(loanvalue))//barath 21.12.17
                                                                loanvalue = de3.ToString();
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = loanvalue;
                                                            //dicactded[de.ToString().Trim().ToLower()] + "(" + selpaidcount + "/" + Convert.ToString(ds.Tables[0].Rows[0]["PayTotMon"]) + ")";
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                        }
                                                        else
                                                        {
                                                            Double.TryParse(Convert.ToString(dicactded[de.ToString().Trim().ToLower()]), out dedamnt);
                                                            dedamnt = Math.Round(dedamnt, 0, MidpointRounding.AwayFromZero);
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(dedamnt);
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Double.TryParse(Convert.ToString(dicactded[de.ToString().Trim().ToLower()]), out dedamnt);
                                                        dedamnt = Math.Round(dedamnt, 0, MidpointRounding.AwayFromZero);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(dedamnt);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                    }
                                                }
                                                else
                                                {
                                                    if (dicactded[de.ToString().Trim().ToLower()] == "0" || dicactded[de.ToString().Trim().ToLower()] == "" && dedrowcheck == false)
                                                    {
                                                        fpsalary.Sheets[0].Rows[fpsalary.Sheets[0].RowCount - 1].Visible = false;
                                                        dedrowcheck = true;
                                                        dedrowviscount--;
                                                        dedrowcount++;
                                                    }
                                                    else
                                                    {
                                                        if (chk_loandet.Checked == true)
                                                        {
                                                            if (selpaidcount.Trim() != "0" && selpaidcount.Trim() != "")
                                                            {
                                                                foreach (KeyValuePair<string, string> dr in dicloandet)
                                                                {
                                                                    if (de3.ToString() != "0" && de3.ToString() != "")
                                                                        if (loanvalue.Trim() == "")
                                                                            loanvalue = dicactded[de.ToString().Trim().ToLower()] + dr.Value;
                                                                        else
                                                                            loanvalue = loanvalue + " , " + dicactded[de.ToString().Trim().ToLower()] + dr.Value;
                                                                }
                                                                if (string.IsNullOrEmpty(loanvalue))//barath 21.12.17
                                                                    loanvalue = de3.ToString();
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = loanvalue;
                                                                //dicactded[de.ToString().Trim().ToLower()] + "(" + selpaidcount + "/" + Convert.ToString(ds.Tables[0].Rows[0]["PayTotMon"]) + ")";
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                            }
                                                            else
                                                            {
                                                                Double.TryParse(Convert.ToString(dicactded[de.ToString().Trim().ToLower()]), out dedamnt);
                                                                dedamnt = Math.Round(dedamnt, 0, MidpointRounding.AwayFromZero);
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(dedamnt);
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Double.TryParse(Convert.ToString(dicactded[de.ToString().Trim().ToLower()]), out dedamnt);
                                                            dedamnt = Math.Round(dedamnt, 0, MidpointRounding.AwayFromZero);
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(dedamnt);
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (chk_amnt.Checked == true)
                                            {
                                                if (chk_loandet.Checked == true)
                                                {
                                                    if (selpaidcount.Trim() != "0" && selpaidcount.Trim() != "")
                                                    {
                                                        foreach (KeyValuePair<string, string> dr in dicloandet)
                                                        {
                                                            if (de3.ToString() != "0" && de3.ToString() != "")
                                                                if (loanvalue.Trim() == "")
                                                                    loanvalue = de3.ToString() + dr.Value;
                                                                else
                                                                    loanvalue = loanvalue + " , " + de3.ToString() + dr.Value;
                                                        }
                                                        if (string.IsNullOrEmpty(loanvalue))//barath 21.12.17
                                                            loanvalue = de3.ToString();
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = loanvalue;
                                                        //de3.ToString() + "(" + selpaidcount + "/" + Convert.ToString(ds.Tables[0].Rows[0]["PayTotMon"]) + ")";
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";

                                                    }
                                                    else
                                                    {
                                                        Double.TryParse(Convert.ToString(de3), out dedamnt);
                                                        dedamnt = Math.Round(dedamnt, 0, MidpointRounding.AwayFromZero);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(dedamnt);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                    }
                                                }
                                                else //poo 15.11.17
                                                {
                                                    Double.TryParse(Convert.ToString(de3), out dedamnt);
                                                    dedamnt = Math.Round(dedamnt, 0, MidpointRounding.AwayFromZero);
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(dedamnt);
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                }
                                            }
                                            else
                                            {
                                                if (de3.ToString() == "0" || de3.ToString() == "" && dedrowcheck == false)
                                                {
                                                    fpsalary.Sheets[0].Rows[fpsalary.Sheets[0].RowCount - 1].Visible = false;
                                                    dedrowcheck = true;
                                                    dedrowviscount--;
                                                    dedrowcount++;
                                                }
                                                else
                                                {
                                                    if (chk_loandet.Checked == true)
                                                    {
                                                        if (selpaidcount.Trim() != "" && selpaidcount.Trim() != "0")
                                                        {
                                                            foreach (KeyValuePair<string, string> dr in dicloandet)
                                                            {
                                                                if (de3.ToString() != "0" && de3.ToString() != "")
                                                                    if (loanvalue.Trim() == "")
                                                                        loanvalue = de3.ToString() + dr.Value;
                                                                    else
                                                                        loanvalue = loanvalue + " , " + de3.ToString() + dr.Value;
                                                            }
                                                            if (string.IsNullOrEmpty(loanvalue))//barath 21.12.17
                                                                loanvalue = de3.ToString();
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = loanvalue;
                                                            //de3.ToString() + "(" + selpaidcount + "/" + Convert.ToString(ds.Tables[0].Rows[0]["PayTotMon"]) + ")";
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                        }
                                                        else
                                                        {
                                                            Double.TryParse(Convert.ToString(de3), out dedamnt);
                                                            dedamnt = Math.Round(dedamnt, 0, MidpointRounding.AwayFromZero);
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(dedamnt);
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Double.TryParse(Convert.ToString(de3), out dedamnt);
                                                        dedamnt = Math.Round(dedamnt, 0, MidpointRounding.AwayFromZero);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Text = Convert.ToString(dedamnt);
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].HorizontalAlign = HorizontalAlign.Right;
                                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, Convert.ToInt32(GetIndex)].Font.Name = "Book Antiqua";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        string bankquerys = "select hb.bankpk,BankName from hr_bankrights  hb,FM_FinBankMaster fb where hb.bankpk=fb.BankPK and hb.college_code=fb.CollegeCode and hb.college_code='" + ddlcollege.SelectedItem.Value + "'";//delsi1106
                        DataSet bankdss = new DataSet();

                        bankds = d2.select_method_wo_parameter(bankquerys, "text");


                        if (bankds.Tables[0].Rows.Count > 0)
                        {
                            int totalbankcount = (bankds.Tables[0].Rows.Count) + 1;
                            string getpaymode = Convert.ToString(dsset.Tables[0].Rows[sal]["PayMode"]);


                            int clgBankcodee = Convert.ToInt32(dsset.Tables[0].Rows[sal]["clgbankcode"]);

                            for (int i = 0; i < bankds.Tables[0].Rows.Count; i++)
                            {

                                //int headertag = Convert.ToInt32(fpsalary.Sheets[0].ColumnHeader.Cells[0, fpsalary.Sheets[0].ColumnCount - totalbankcount + i].Tag);
                                int headertag = Convert.ToInt32(fpsalary.Sheets[0].ColumnHeader.Cells[0, columncountbank - totalbankcount + i].Tag);
                                if (getpaymode.Trim().ToUpper() == "CREDIT")
                                {
                                    if (clgBankcodee == headertag)
                                    {
                                        double netsalarys = 0;
                                        Double.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["netsal"]), out netsalarys);
                                        netsalary = Math.Round(netsalary, 0, MidpointRounding.AwayFromZero);
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - totalbankcount + i].Text = Convert.ToString(netsalarys);
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - totalbankcount + i].HorizontalAlign = HorizontalAlign.Right;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - totalbankcount + i].Font.Name = "Book Antiqua";
                                    }
                                    else
                                    {
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - totalbankcount + i].Text = "";
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - totalbankcount + i].HorizontalAlign = HorizontalAlign.Right;
                                        fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - totalbankcount + i].Font.Name = "Book Antiqua";
                                    }
                                }
                                else
                                {
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - totalbankcount + i].Text = "";
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - totalbankcount + i].HorizontalAlign = HorizontalAlign.Right;
                                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - totalbankcount + i].Font.Name = "Book Antiqua";

                                }

                            }
                            if (getpaymode.Trim().ToUpper() == "CASH")
                            {
                                double netsalarys = 0;
                                Double.TryParse(Convert.ToString(dsset.Tables[0].Rows[sal]["netsal"]), out netsalarys);
                                netsalary = Math.Round(netsalary, 0, MidpointRounding.AwayFromZero);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - 1].Text = Convert.ToString(netsalary);
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - 1].HorizontalAlign = HorizontalAlign.Right;
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - 1].Font.Name = "Book Antiqua";
                            }
                            else
                            {
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - 1].Text = "";
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - 1].HorizontalAlign = HorizontalAlign.Right;
                                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, columncountbank - 1].Font.Name = "Book Antiqua";

                            }

                        }
                        Hashtable allowhash = new Hashtable();
                        if (cblcolumnorder.Items[61].Selected == true)
                        {
                            getnetsal = Convert.ToInt32(dsset.Tables[0].Rows[sal]["netadd"]);
                            if (graduity != "0")
                            {
                                if (graduity.Contains('+'))
                                {
                                    string[] splitval = graduity.Split('+');

                                    for (int val = 0; val < splitval.Count(); val++)
                                    {
                                        string gratuitytxt = Convert.ToString(splitval[val]);
                                        if (gratuitytxt != "Basic")
                                        {

                                            string allowance3 = "";
                                            allowance3 = Convert.ToString(dsset.Tables[0].Rows[sal]["allowances"]);
                                            string[] allowanmce_arr1;
                                            string alowancesplit;
                                            int exatval1 = 0;

                                            double DblAllowLOP = 0;
                                            double DblAllowLOP1 = 0;
                                            allowanmce_arr1 = allowance3.Split('\\');
                                            for (int i = 0; i < allowanmce_arr1.GetUpperBound(0); i++)
                                            {
                                                exatval1 = allowanmce_arr1.GetUpperBound(0);
                                                alowancesplit = allowanmce_arr1[i];
                                                string[] allowanceda = alowancesplit.Split(';');
                                                if (allowanceda.GetUpperBound(0) >= 3)
                                                {
                                                    string da = allowanceda[0];
                                                    if (gratuitytxt == da)
                                                    {

                                                        string mode = Convert.ToString(allowanceda[1]);
                                                        string daac = "";
                                                        string da3 = "";
                                                        if (allowanceda[2].Trim() != "")
                                                        {
                                                            string[] spval = allowanceda[2].Split('-');
                                                            if (spval.Length == 2)
                                                            {
                                                                if (mode.Trim().ToUpper() == "PERCENT")
                                                                {
                                                                    da3 = Convert.ToString(spval[1]);
                                                                }
                                                                else
                                                                {
                                                                    da3 = Convert.ToString(spval[0]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                da3 = Convert.ToString(allowanceda[3]);
                                                            }
                                                        }
                                                        daac = Convert.ToString(allowanceda[3]);
                                                        double da2 = 0;
                                                        Double.TryParse(daac, out da2);
                                                        double ds3 = 0;
                                                        double.TryParse(da3, out ds3);
                                                        ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                                        string DblAllowActVal = daac;
                                                        string DblAllowVal = allowanceda[3];
                                                        DblAllowLOP = (Convert.ToDouble(DblAllowActVal) - Convert.ToDouble(DblAllowVal));
                                                        DblAllowLOP = Math.Round(DblAllowLOP);
                                                        DblAllowLOP1 = DblAllowLOP1 + DblAllowLOP;
                                                        if (ds3 != 0)
                                                        {
                                                            if (!allowhash.Contains(da))
                                                            {
                                                                allowhash.Add(da, da3);

                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (gratuitytxt == "Basic")
                                        {
                                            double basic_pay3 = 0;
                                            double.TryParse(dsset.Tables[0].Rows[sal]["bsalary"].ToString(), out basic_pay3);
                                            if (!allowhash.Contains("Basic"))
                                            {
                                                allowhash.Add("Basic", basic_pay3);
                                            }


                                        }
                                    }

                                    foreach (DictionaryEntry item in allowhash)
                                    {
                                        string key = Convert.ToString(item.Key);

                                        string value = Convert.ToString(item.Value);

                                        gettotallow = gettotallow + Convert.ToInt32(value);
                                    }
                                    gratuityval = (Convert.ToInt32(gettotallow) * 15 * Years / 26);
                                }
                                else
                                {
                                    if (graduity.Trim() == "Gross Pay")
                                    {
                                        gratuityval = (Convert.ToInt32(getnetsal) * 15 * Years / 26);
                                    }
                                }

                            }

                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, gratuitucolcount - 1].Text = Convert.ToString(gratuityval);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, gratuitucolcount - 1].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, gratuitucolcount - 1].Font.Name = "Book Antiqua";


                        }
                        if (cblcolumnorder.Items[62].Selected)//delsi1710
                        {
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, yrofexpcount - 1].Text = Convert.ToString(Years) + "Yrs";
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, yrofexpcount - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, yrofexpcount - 1].Font.Name = "Book Antiqua";


                        }
                        if (dedrowcheck == true || rowcheck == true)
                        {
                            basicpay = basicpay - newbasicpay;
                            actualbasic = actualbasic - newactualbasic;
                            lopamt = lopamt - newlopamt;
                            pfcont = pfcont - newpfcont;
                            esicont = esicont - newesicont;
                            daper = daper - newdaper;
                            incinrs = incinrs - newincinrs;
                            Actgross = Actgross - newActgross;
                            grosspay = grosspay - newgrosspay;
                            Advrs = Advrs - newAdvrs;
                            collectedamt = collectedamt - newcollectedamt;
                            totded = totded - newtotded;
                            payband = payband - newpayband;
                            actgradeamnt = actgradeamnt - newactgradeamnt;
                            gradeamnt = gradeamnt - newgradeamnt;
                            netamnt = netamnt - newnetamnt;
                        }
                    }
                }
                fpsalary.Sheets[0].RowCount++;
                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Text = "";
                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Text = "TOTAL";
                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                fpsalary.Sheets[0].Rows[fpsalary.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                fpsalary.Sheets[0].Rows[fpsalary.Sheets[0].RowCount - 1].Locked = true;
                fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                fpsalary.Sheets[0].SpanModel.Add(fpsalary.Sheets[0].RowCount - 1, 1, 1, 1);
                for (int lstcol = 0; lstcol < lstcolorder.Items.Count; lstcol++)
                {
                    if (getcol.ContainsKey(Convert.ToString(lstcolorder.Items[lstcol].Value)))
                    {
                        if (lstcolorder.Items[lstcol].Text == "ACTUAL BASIC")
                        {
                            int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                            actualbasic = Math.Round(actualbasic, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("ACTUAL BASIC"))
                            {
                                ColumnAdjWid.Add("ACTUAL BASIC", Convert.ToString(actualbasic));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(actualbasic);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "LOP AMOUNT")
                        {
                            int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                            lopamt = Math.Round(lopamt, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("LOP AMOUNT"))
                            {
                                ColumnAdjWid.Add("LOP AMOUNT", Convert.ToString(lopamt));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(lopamt);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "PF Salary")
                        {
                            int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                            pfcont = Math.Round(pfcont, 0, MidpointRounding.AwayFromZero);
                            Session["PFSal"] = Convert.ToString(pfcont);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(pfcont);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "ESI Salary")
                        {
                            int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                            esicont = Math.Round(esicont, 0, MidpointRounding.AwayFromZero);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(esicont);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "DA %")
                        {
                            int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                            daper = Math.Round(daper, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("DA %"))
                            {
                                ColumnAdjWid.Add("DA %", Convert.ToString(daper));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(daper);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "INCREMENT IN RS.")
                        {
                            int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                            incinrs = Math.Round(incinrs, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("INCREMENT IN RS."))
                            {
                                ColumnAdjWid.Add("INCREMENT IN RS.", Convert.ToString(incinrs));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(incinrs);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "BASIC PAY Rs.")
                        {
                            int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                            basicpay = Math.Round(basicpay, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("BASIC PAY Rs."))
                            {
                                ColumnAdjWid.Add("BASIC PAY Rs.", Convert.ToString(basicpay));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(basicpay);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "GROSS PAY Rs.")
                        {
                            int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                            grosspay = Math.Round(grosspay, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("GROSS PAY Rs."))
                            {
                                ColumnAdjWid.Add("GROSS PAY Rs.", Convert.ToString(grosspay));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(grosspay);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "ACTUAL GROSS SALARY")
                        {
                            int colidx = Convert.ToInt32(Convert.ToString(TotalIndexValue[lstcolorder.Items[lstcol].Text]));
                            Actgross = Math.Round(Actgross, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("ACTUAL GROSS SALARY"))
                            {
                                ColumnAdjWid.Add("ACTUAL GROSS SALARY", Convert.ToString(Actgross));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(Actgross);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "ADV RS.")
                        {
                            int colidx = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                            Advrs = Math.Round(Advrs, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("ADV RS."))
                            {
                                ColumnAdjWid.Add("ADV RS.", Convert.ToString(Advrs));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(Advrs);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "COLLECTED AMT")
                        {
                            int colidx = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                            collectedamt = Math.Round(collectedamt, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("COLLECTED AMT"))
                            {
                                ColumnAdjWid.Add("COLLECTED AMT", Convert.ToString(collectedamt));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(collectedamt);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "TOT DED Rs.")
                        {
                            int colidx = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                            totded = Math.Round(totded, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("TOT DED Rs."))
                            {
                                ColumnAdjWid.Add("TOT DED Rs.", Convert.ToString(totded));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(totded);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "PAY BAND")
                        {
                            int colidx = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                            payband = Math.Round(payband, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("PAY BAND"))
                            {
                                ColumnAdjWid.Add("PAY BAND", Convert.ToString(payband));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(payband);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "ACTUAL GRADE")
                        {
                            int colidx = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                            actgradeamnt = Math.Round(actgradeamnt, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("ACTUAL GRADE"))
                            {
                                ColumnAdjWid.Add("ACTUAL GRADE", Convert.ToString(actgradeamnt));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(actgradeamnt);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "GRADE PAY")
                        {
                            int colidx = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                            gradeamnt = Math.Round(gradeamnt, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("GRADE PAY"))
                            {
                                ColumnAdjWid.Add("GRADE PAY", Convert.ToString(gradeamnt));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(gradeamnt);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "NET PAY")
                        {
                            int colidx = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]);
                            netamnt = Math.Round(netamnt, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey("NET PAY"))
                            {
                                ColumnAdjWid.Add("NET PAY", Convert.ToString(netamnt));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(netamnt);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "CONVENES EXP")//poo 27.10.17
                        {
                            int colidx = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]); fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(newconvenexp);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                        if (lstcolorder.Items[lstcol].Text == "LUNCH EXP")//poo 27.10.17
                        {
                            int colidx = Convert.ToInt32(TotalIndexValue[Convert.ToString(lstcolorder.Items[lstcol].Text)]); fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(newlunchexp);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Center;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }

                    }
                }
                if (cb_all.Checked == true)
                {
                    for (int allow = 0; allow < cbl_allow.Items.Count; allow++)
                    {
                        if (cbl_allow.Items[allow].Selected == true)
                        {
                            double alltot = 0.0;
                            double getval = 0.0;
                            int colidx = Convert.ToInt32(AllowanceHash[(Convert.ToString(cbl_allow.Items[allow].Text) == "Da") ? "DA" : Convert.ToString(cbl_allow.Items[allow].Text)]);
                            for (int rowcount = 1; rowcount < fpsalary.Sheets[0].RowCount - 1; rowcount++)
                            {
                                if (fpsalary.Sheets[0].Rows[rowcount].Visible == true)
                                {
                                    if (fpsalary.Sheets[0].Cells[rowcount, colidx].Text == "")
                                    {
                                        getval = 0.0;
                                    }
                                    else
                                    {
                                        double.TryParse(fpsalary.Sheets[0].Cells[rowcount, colidx].Text, out getval);
                                    }
                                    alltot = alltot + Convert.ToDouble(getval);
                                }
                            }
                            alltot = Math.Round(alltot, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey(Convert.ToString(cbl_allow.Items[allow].Text + " " + "Rs.")))
                            {
                                ColumnAdjWid.Add(Convert.ToString(cbl_allow.Items[allow].Text + " " + "Rs."), Convert.ToString(alltot));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(alltot);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                    }
                }
                if (cb_deduct.Checked == true)
                {
                    for (int allow = 0; allow < cbl_ded.Items.Count; allow++)
                    {
                        if (cbl_ded.Items[allow].Selected == true)
                        {
                            double alltot = 0.0;
                            double getval = 0.0;
                            string splval = "";
                            int colidx = Convert.ToInt32(DeducationHash[Convert.ToString(cbl_ded.Items[allow].Text)]);
                            for (int rowcount = 1; rowcount < fpsalary.Sheets[0].RowCount - 1; rowcount++)
                            {
                                if (fpsalary.Sheets[0].Rows[rowcount].Visible == true)
                                {
                                    if (fpsalary.Sheets[0].Cells[rowcount, colidx].Text == "")
                                    {
                                        getval = 0.0;
                                    }
                                    else
                                    {
                                        splval = Convert.ToString(fpsalary.Sheets[0].Cells[rowcount, colidx].Text);
                                        if (chk_loandet.Checked == true)
                                        {
                                            if (splval.Contains('('))
                                            {
                                                double.TryParse(Convert.ToString(splval.Split('(')[0]), out getval);
                                            }
                                            else
                                            {
                                                double.TryParse(fpsalary.Sheets[0].Cells[rowcount, colidx].Text, out getval);
                                            }
                                        }
                                        else
                                        {
                                            double.TryParse(fpsalary.Sheets[0].Cells[rowcount, colidx].Text, out getval);
                                        }
                                    }
                                    alltot = alltot + Convert.ToDouble(getval);
                                }
                            }
                            alltot = Math.Round(alltot, 0, MidpointRounding.AwayFromZero);
                            if (!ColumnAdjWid.ContainsKey(Convert.ToString(cbl_ded.Items[allow].Text + " " + "Rs.")))
                            {
                                ColumnAdjWid.Add(Convert.ToString(cbl_ded.Items[allow].Text + " " + "Rs."), Convert.ToString(alltot));
                            }
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Text = Convert.ToString(alltot);
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Bold = true;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].HorizontalAlign = HorizontalAlign.Right;
                            fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, colidx].Font.Name = "Book Antiqua";
                        }
                    }
                }
                if (cblcolumnorder.Items[53].Selected == true)
                {
                    fpsalary.Sheets[0].RowCount++;
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    int netvaldkm = Convert.ToInt32(Math.Round(netamnt, 0, MidpointRounding.AwayFromZero));
                    string netword = ConvertNumbertoWords(netvaldkm);
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].Text = "NET SALARY : " + "RUPEES " + netword.ToUpper() + " " + "ONLY" + " ";
                    fpsalary.Sheets[0].SpanModel.Add(fpsalary.Sheets[0].RowCount - 1, 0, 1, fpsalary.Sheets[0].ColumnCount);
                }
                if (chk_amnt.Checked == false)
                {
                    int j = 0;
                    for (int iro = 1; iro < fpsalary.Sheets[0].RowCount - 2; iro++)//dsset.Tables[0].Rows.Count 22.01.17 barath
                    {
                        j++;
                        if (fpsalary.Sheets[0].Rows[iro].Visible == true)
                        {
                            fpsalary.Sheets[0].Cells[iro, 0].Text = Convert.ToString(j);
                        }
                        else
                        {
                            fpsalary.Sheets[0].Cells[iro, 0].Text = Convert.ToString(j - 1);
                            j = j - 1;
                        }
                    }
                }

                if (fpsalary.Sheets[0].Cells[1, 0].Text.Trim() == "" || fpsalary.Sheets[0].Cells[1, 0].Text.Trim() == "0")
                {
                    fpsalary.Visible = false;
                    div1.Visible = false;
                    rprint.Visible = false;
                    chkShowPF.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Records Found!";
                    btnpayslip.Visible = false;
                    chkIncLopAmnt.Visible = false;
                    btnsalcer.Visible = false;
                }
                else
                {
                    if (postbackcount > 0)
                    {
                        fpsalary.Sheets[0].AutoPostBack = false;
                        fpsalary.Sheets[0].PageSize = fpsalary.Sheets[0].RowCount;
                        fpsalary.Width = 900;
                        fpsalary.Height = 500;
                        fpsalary.Visible = true;
                        div1.Visible = true;
                        btnpayslip.Visible = true;
                        chkIncLopAmnt.Visible = true;
                        btnsalcer.Visible = true;
                    }
                    else
                    {
                        fpsalary.Sheets[0].AutoPostBack = true;
                        fpsalary.Sheets[0].PageSize = fpsalary.Sheets[0].RowCount;
                        fpsalary.Visible = true;
                        div1.Visible = true;
                        btnpayslip.Visible = true;
                        chkIncLopAmnt.Visible = true;
                        btnsalcer.Visible = true;
                    }
                    lblpurpose1.Visible = false;
                    ddlpurpose.Visible = false;
                    FpSpread2.Visible = false;
                    btnaddtemplate.Visible = false;
                    btndeletetemplate.Visible = false;
                    btnsendsms.Visible = false;
                    txtsms.Visible = false;
                    lblsmstype.Visible = false;
                    ddlsmstype.Visible = false;
                    if (fpsalary.Visible == true)
                    {
                        if (chksms.Checked == true)
                        {
                            Spread2Go();
                            bindpurpose();
                            btnsendsms.Visible = true;
                            txtsms.Visible = true;
                            lblsmstype.Visible = true;
                            ddlsmstype.Visible = true;
                            if (ddlsmstype.SelectedValue.ToString() == "Automatic")
                            {
                                string year = d2.GetFunction("select CONVERT(CHAR(4), to_date, 120) from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "' and college_Code=" + collegecode1 + "");
                                string settext = "Your salary for the month of " + ddl_mon.SelectedItem.Text + " - " + year + "- Rs.$Salary$ has been credited to your account-$Account No$";
                                txtsms.Enabled = false;
                                txtsms.Text = settext;
                            }
                            else
                            {
                                lblpurpose1.Visible = true;
                                ddlpurpose.Visible = true;
                                FpSpread2.Visible = true;
                                btnaddtemplate.Visible = true;
                                btndeletetemplate.Visible = true;
                                txtsms.Enabled = true;
                            }
                        }
                        if (chksms.Checked == true || chkmail.Checked == true)
                        {
                            btnsendsms.Visible = true;
                        }
                    }
                    lblnorec.Visible = false;
                    lblnorec.Text = "";
                    Session["myDict"] = ColumnAdjWid;
                }
            }
            else
            {
                fpsalary.Visible = false;
                div1.Visible = false;
                rprint.Visible = false;
                chkShowPF.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found!";
                btnpayslip.Visible = false;
                chkIncLopAmnt.Visible = false;
                btnsalcer.Visible = false;
            }
            fpsalary.SaveChanges();
            #endregion
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "Individual_SalaryReport.aspx");
        }
    }
    public void Spread2Go()
    {
        try
        {
            FpSpread2.Sheets[0].ColumnHeaderVisible = false;
            FpSpread2.Sheets[0].SheetCorner.Columns[0].Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].RowCount = 1;
            FpSpread2.Sheets[0].ColumnCount = 2;
            FpSpread2.Columns[1].Width = 900;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = "Template";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
            string spread2query1 = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template where college_code='" + collegecode1 + "'";
            ds = d2.select_method(spread2query1, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
                }
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.SaveChanges();
        }
        catch { }
    }
    protected void btnsendsms_Click(object sender, EventArgs e)
    {
        try
        {
            int staffcount = 0;
            int staffmailcount = 0;
            string date3 = "";
            string date4 = "";
            bool sentflag = false;
            string sql = "select distinct CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "'and PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and college_Code=" + collegecode1 + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                date3 = Convert.ToString(ds.Tables[0].Rows[0]["from_date"]);
                date4 = Convert.ToString(ds.Tables[0].Rows[0]["to_date"]);
            }
            string datefrom3;
            int monthname1;
            string monyear;
            string monthname2 = "";
            int monthnamenum;
            string yearto = "";
            string monyearto = "";
            string[] split = date3.Split(new Char[] { '/' });
            datefrom3 = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            int year3 = Convert.ToInt16(split[2].ToString());
            string[] split1 = date4.Split(new Char[] { '/' });
            string dateto4 = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            monthname2 = split1[0].ToString();
            monthnamenum = Convert.ToInt32(monthname2.ToString());
            yearto = split1[2].ToString();
            string mnmae = split[1].ToString();
            monthname1 = Convert.ToInt16(mnmae);
            monthname2 = split1[1].ToString();
            monthnamenum = Convert.ToInt16(monthname2);
            string year = split[2].ToString();
            monyear = monthname1.ToString() + "/" + year;
            monyearto = monthnamenum.ToString() + "/" + yearto;
            string strsqlsms = " select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode,college_code from collinfo where college_code='" + collegecode1 + "'";
            strsqlsms = strsqlsms + " select shortname,college_code from leave_category where college_code=" + collegecode1 + " ;";
            strsqlsms = strsqlsms + " select * from monthlypay where PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and PayMonth='" + ddl_mon.SelectedValue.ToString() + "'  and college_code=" + collegecode1 + " ;";
            strsqlsms = strsqlsms + " select linkvalue,college_code from inssettings where linkname='Calculate LOP from Attendance' and college_code='" + collegecode1 + "' ;";
            strsqlsms = strsqlsms + " select * from staff_attnd where mon_year between '" + monyear + "' and '" + monyearto + "';";
            strsqlsms = strsqlsms + " select sm.staff_name,sm.staff_code,hd.dept_name,dm.desig_name,sm.pfnumber,sm.bankaccount,ct.category_name,st.allowances as actallowance  from staffmaster sm,stafftrans st,hrdept_master hd,desig_master dm,staffcategorizer ct where sm.college_code=hd.college_code and sm.college_code=dm.collegeCode and sm.college_code=ct.college_code and sm.staff_code=st.staff_code and hd.dept_code=st.dept_code and st.desig_code=dm.desig_code and st.category_code=ct.category_code and ((sm.resign=0 and sm.settled=0) and ISNULL(sm.Discontinue,'0')='0') and st.latestrec=1 and sm.college_code='" + collegecode1 + "' ;";
            strsqlsms = strsqlsms + " select sm.staff_code,convert(nvarchar(15),sa.date_of_birth,103) as dob,convert(nvarchar(15),sm.relieve_date,103) as  retier,sm.staff_name,sa.email,sa.per_mobileno from staffmaster sm,staff_appl_master sa where sm.appl_no=sa.appl_no  ;";
            strsqlsms = strsqlsms + " select * from incentives_master where college_code='" + collegecode1 + "'";
            dssmssalary.Reset();
            dssmssalary.Dispose();
            dssmssalary = d2.select_method_wo_parameter(strsqlsms, "Text");
            string strquery = d2.GetFunction("select Coll_acronymn from collinfo where college_code='" + collegecode1 + "'");
            double strquery1 = 0;
            double strquery2 = 0;
            if (dssmssalary.Tables[7].Rows.Count > 0)
            {
                alloworder = dssmssalary.Tables[7].Rows[0]["allowances"].ToString();
                string deductval = dssmssalary.Tables[7].Rows[0]["deductions"].ToString();
                string[] spdet = deductval.Split(';');
                for (int d = 0; d <= spdet.GetUpperBound(0); d++)
                {
                    string[] spdedet = spdet[d].Split('\\');
                    if (spdedet.GetUpperBound(0) >= 1)
                    {
                        string val = spdedet[0];
                        string val1 = spdedet[1];
                        if (!deduct.ContainsKey(val))
                        {
                            deduct.Add(val, val1);
                        }
                    }
                }
            }
            fpsalary.SaveChanges();
            if (chksms.Checked == true)
            {
                for (int i = 1; i < fpsalary.Sheets[0].RowCount; i++)
                {
                    if (fpsalary.Sheets[0].Rows[i].Visible == true)
                    {
                        for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                        {
                            if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT")
                            {
                                string text = fpsalary.Sheets[0].Cells[i, lst].Text.ToString();
                                if (text != "" && text != null)
                                {
                                    string va = Convert.ToString(fpsalary.Sheets[0].Cells[i, lst].Value);
                                    if (va == "1")
                                    {
                                        staffcount++;
                                        string staffcode = "";
                                        string designation = "";
                                        string deptname = "";
                                        string lopdates = "";
                                        string lopdays = "";
                                        for (int chklst = 0; chklst < lstcolorder.Items.Count; chklst++)
                                        {
                                            if (lstcolorder.Items[chklst].Text == "STAFF CODE")
                                            {
                                                staffcode = Convert.ToString(fpsalary.Sheets[0].Cells[i, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DESIGN")
                                            {
                                                designation = Convert.ToString(fpsalary.Sheets[0].Cells[i, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DEPT")
                                            {
                                                deptname = Convert.ToString(fpsalary.Sheets[0].Cells[i, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "LOP DATES")
                                            {
                                                lopdates = Convert.ToString(fpsalary.Sheets[0].Cells[i, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "LOP DAYS")
                                            {
                                                lopdays = Convert.ToString(fpsalary.Sheets[0].Cells[i, chklst + 1].Tag);
                                            }
                                        }
                                        dssmssalary.Tables[2].DefaultView.RowFilter = " PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and PayMonth='" + ddl_mon.SelectedValue.ToString() + "' and staff_code='" + staffcode + "'";
                                        DataView dvnetsal = dssmssalary.Tables[2].DefaultView;
                                        if (dvnetsal.Count > 0)
                                        {
                                            strquery1 = Convert.ToDouble(dvnetsal[0]["netsal"].ToString());
                                            strquery2 = Convert.ToDouble(dvnetsal[0]["netsal"].ToString());
                                        }
                                        txtsms.Visible = true;
                                        string mobileno = "";
                                        dssmssalary.Tables[6].DefaultView.RowFilter = " staff_code='" + staffcode.ToString() + "'";
                                        DataView dvemail = dssmssalary.Tables[6].DefaultView;
                                        if (dvemail.Count > 0)
                                        {
                                            mobileno = dvemail[0]["per_mobileno"].ToString();
                                        }
                                        string sqlnew = txtsms.Text;
                                        if (ddlsmstype.SelectedValue.ToString() == "Automatic")
                                        {
                                            sqlnew = "Your Net Salary of Rs." + strquery1 + " for the month of " + ddl_mon.SelectedItem.Text + " " + year + "  has been credited to your S.B.account.-" + strquery + "";
                                        }
                                        else if (ddlsmstype.SelectedValue.ToString() == "Template With Salary")
                                        {
                                            sqlnew = sqlnew + ".Your Net Salary of Rs." + strquery1 + "";
                                        }
                                        else if (ddlsmstype.SelectedValue.ToString() == "Template With LOP")
                                        {
                                            string lopdatesss = "";
                                            lopdatesss = lopdates;
                                            string lopnooo = "";
                                            lopnooo = lopdays;
                                            sqlnew = "Your salary for the month of " + ddl_mon.SelectedItem.Text + " " + year + " is Rs." + strquery1 + " with " + lopnooo + " LOP(s) " + lopdatesss + ".";
                                        }
                                        staffmailcount = sendsms(mobileno, sqlnew);
                                        if (staffmailcount > 0)
                                        {
                                            sentflag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (sentflag == true)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Message has been sent Successfully!";
                    smssentcount = 0;
                }
                if (staffcount == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please select any one staff!";
                }
            }
            if (chkmail.Checked == true)
            {
                loadpayslipnew();
                // for (int i = 1; i < fpsalary.Sheets[0].RowCount; i++)
                //{
                //    if (fpsalary.Sheets[0].Rows[i].Visible == true)
                //    {
                //        for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                //        {
                //            if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT")
                //            {
                //                string text = fpsalary.Sheets[0].Cells[i, lst].Text.ToString();
                //                if (text != "" && text != null)
                //                {
                //                    string va = Convert.ToString(fpsalary.Sheets[0].Cells[i, lst].Value);
                //                    if (va == "1")
                //                    {
                //                        staffmailcount++;
                //                        btnsendsms.Visible = true;
                //                        btnsendsms.Focus();
                //                        string staffcode = "";
                //                        for (int chklst = 0; chklst < lstcolorder.Items.Count; chklst++)
                //                        {
                //                            if (lstcolorder.Items[chklst].Text == "STAFF CODE")
                //                            {
                //                                staffcode = Convert.ToString(fpsalary.Sheets[0].Cells[i, chklst + 1].Tag);
                //                            }
                //                        }
                //                        if (staffcode.Trim() != "" && staffcode.Trim() != null)
                //                        {
                //                            dssmssalary.Tables[2].DefaultView.RowFilter = " PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and PayMonth='" + ddl_mon.SelectedValue.ToString() + "' and staff_code='" + staffcode + "'";
                //                            DataView dvnetsal = dssmssalary.Tables[2].DefaultView;
                //                            if (dvnetsal.Count > 0)
                //                            {
                //                                strquery1 = Convert.ToDouble(dvnetsal[0]["netsal"].ToString());
                //                            }
                //                            txtsubject.Text = "Your Net Salary of Rs." + strquery1 + " for the month of " + ddl_mon.SelectedItem.Text + " " + year + " has been credited to your S.B.account.-" + strquery + "";
                //                            string email = "";
                //                            string staffname = "";
                //                            dssmssalary.Tables[6].DefaultView.RowFilter = " staff_code='" + staffcode.ToString() + "'";
                //                            DataView dvemail = dssmssalary.Tables[6].DefaultView;
                //                            if (dvemail.Count > 0)
                //                            {
                //                                email = dvemail[0]["email"].ToString();
                //                                staffname = dvemail[0]["staff_name"].ToString();
                //                            }
                //                            string query = txtsubject.Text;
                //                            string details = staffname + staffcode;
                //                            if (email.Trim() != "" && email != null)
                //                            {
                //                                loadindstaffpayslip(staffcode);
                //                                sendemail(email, query, details);
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //if (staffmailcount == 0)
                //{
                //    imgdiv2.Visible = true;
                //    lbl_alert.Text = "Please select any one staff!";
                //}
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Individual_SalaryReport.aspx");

        }
    }
    string SenderID = "";
    string Password = "";
    string strmsg = "";
    public int sendsms(string mobile, string sms)
    {
        int smscount = 0;
        try
        {
            lblsmserror.Visible = false;
            bool sendflag = false;
            string no = mobile;
            strmsg = sms;
            if (chksms.Checked == true)
            {
                string user_id = "";
                string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + collegecode1 + "'";
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(strsenderquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                }
                smscount = d2.send_sms(user_id, collegecode1, usercode, mobile, sms, "1");
                //if (smscount > 0)
                //{
                //    imgdiv2.Visible = true;
                //    lbl_alert.Text = "Message has been Sent Successfully!";
                //}
                //else
                //{
                //    imgdiv2.Visible = true;
                //    lbl_alert.Text = "You Dont have enough Credits!";
                //}
                #region OldSMS
                //string getval = d2.GetUserapi(user_id);
                //string[] spret = getval.Split('-');
                //if (spret.GetUpperBound(0) == 1)
                //{
                //    SenderID = spret[0].ToString();
                //    Password = spret[0].ToString();
                //    Session["api"] = user_id;
                //    Session["senderid"] = SenderID;
                //}
                //int smssendingcount = 0;
                //for (int i = 1; i < fpsalary.Sheets[0].RowCount; i++)
                //{
                //    if (fpsalary.Sheets[0].Rows[i].Visible == true)
                //    {
                //        for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                //        {
                //            if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT")
                //            {
                //                string text = fpsalary.Sheets[0].Cells[i, lst].Text.ToString();
                //                if (text != "" && text != null)
                //                {
                //                    string va = Convert.ToString(fpsalary.Sheets[0].Cells[i, lst].Value);
                //                    if (va == "1")
                //                    {
                //                        smssendingcount++;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //WebRequest request = WebRequest.Create("http://pr.airsmsmarketing.info/api/checkbalance.php?user=" + user_id + "&pass=" + Password + "");
                //WebResponse response = request.GetResponse();
                //Stream data = response.GetResponseStream();
                //StreamReader sr = new StreamReader(data);
                //string strvel = sr.ReadToEnd();
                //int num = 0;
                //if (int.TryParse(strvel, out num))
                //{
                //    num = Convert.ToInt32(strvel);
                //    if (num > smssendingcount)
                //    {
                //        sendflag = true;
                //        smscount++;
                //        if (no.Trim() != "" && no != null && no.Trim() != "0")
                //        {
                //            string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + no + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                //            smsreport(strpath, no);
                //        }
                //    }
                //    if (sendflag == true)
                //    {
                //        imgdiv2.Visible = true;
                //        lbl_alert.Text = "Message has been Sent Successfully!";
                //        smssentcount++;
                //    }
                //    else
                //    {
                //        imgdiv2.Visible = true;
                //        lbl_alert.Text = "You Dont have enough Credits!";
                //    }
                //}
                //else
                //{
                //    if (strvel != "")
                //    {
                //        if (Convert.ToInt32(strvel.ToString()) > smssendingcount)
                //        {
                //            sendflag = true;
                //            smscount++;
                //            if (no.Trim() != "" && no != null && no.Trim() != "0")
                //            {
                //                string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + no + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                //                smsreport(strpath, no);
                //            }
                //        }
                //        if (sendflag == true)
                //        {
                //            imgdiv2.Visible = true;
                //            lbl_alert.Text = "Message has been Sent Successfully!";
                //            smssentcount++;
                //        }
                //        else
                //        {
                //            imgdiv2.Visible = true;
                //            lbl_alert.Text = "You Dont have enough Credits!";
                //        }
                //    }
                //    else
                //    {
                //        imgdiv2.Visible = true;
                //        lbl_alert.Text = "You Dont have enough Credits!";
                //    }
                //}
                #endregion
            }
        }
        catch { }
        return smscount;
    }
    public void loadindstaffpayslip(string staffcode)
    {
        try
        {
            DataView dvdetails = new DataView();
            DataView dvholi = new DataView();
            DataView dvstaffattendance = new DataView();
            DataView dvstaffdet = new DataView();
            DataView dvmonthlpayt = new DataView();
            DataView dvpay = new DataView();
            DataView dvdate = new DataView();
            string collname = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string pincode = "";
            string address = "";
            string designation = "";
            string deptname = "";
            string staffname = "";
            string bankno = "";
            string pfno = "";
            Font Fontbold = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 20, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 13, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            dssmssalary.Tables[0].DefaultView.RowFilter = "college_code='" + collegecode1 + "'";
            dvdetails = dssmssalary.Tables[0].DefaultView;
            if (dvdetails.Count > 0)
            {
                collname = dvdetails[0]["collname"].ToString();
                address1 = dvdetails[0]["address1"].ToString();
                address2 = dvdetails[0]["address2"].ToString();
                address3 = dvdetails[0]["address3"].ToString();
                pincode = dvdetails[0]["pincode"].ToString();
                if (address1.Trim() != "" && address1 != null)
                {
                    address = address1;
                }
                if (address2.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address2;
                    }
                    else
                    {
                        address = address2;
                    }
                }
                if (address3.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address3;
                    }
                    else
                    {
                        address = address3;
                    }
                }
                if (pincode.Trim() != "" && pincode != null)
                {
                    if (address != "")
                    {
                        address = address + '-' + pincode;
                    }
                    else
                    {
                        address = pincode;
                    }
                }
            }
            string date3 = "";
            string date4 = "";
            string sql = "select distinct CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "'and PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and college_Code=" + collegecode1 + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                date3 = Convert.ToString(ds.Tables[0].Rows[0]["from_date"]);
                date4 = Convert.ToString(ds.Tables[0].Rows[0]["to_date"]);
            }
            string datefrom3;
            int monthname1;
            string monyear;
            string monthname2 = "";
            int monthnamenum;
            string yearto = "";
            string monyearto = "";
            string[] split = date3.Split(new Char[] { '/' });
            int totlastaff = 0;
            int left1 = 20;
            int left2 = 145;
            int left3 = 423;
            int left4 = 570;
            int left1a = 135;
            datefrom3 = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            int year3 = Convert.ToInt16(split[2].ToString());
            string[] split1 = date4.Split(new Char[] { '/' });
            string dateto4 = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            monthname2 = split1[0].ToString();
            monthnamenum = Convert.ToInt32(monthname2.ToString());
            yearto = split1[2].ToString();
            string mnmae = split[1].ToString();
            monthname1 = Convert.ToInt16(mnmae);
            monthname2 = split1[1].ToString();
            monthnamenum = Convert.ToInt16(monthname2);
            string year = split[2].ToString();
            monyear = monthname1.ToString() + "/" + year;
            monyearto = monthnamenum.ToString() + "/" + yearto;
            int dayfrm = 0;
            string dayto;
            int daytonum;
            string dayfrom = split[0].ToString();
            string[] leavetype = new string[50];
            dssmssalary.Tables[1].DefaultView.RowFilter = "college_code='" + collegecode1 + "'";
            dvholi = dssmssalary.Tables[1].DefaultView;
            int lev = 0;
            for (int le = 0; le < dvholi.Count; le++)
            {
                lev++;
                string levatype = dvholi[le]["shortname"].ToString().Trim().ToLower();
                leavetype[lev] = levatype;
            }
            dssmssalary.Tables[2].DefaultView.RowFilter = "fdate ='" + datefrom3 + "' and tdate ='" + dateto4 + "' and staff_code='" + staffcode + "'";
            dvmonthlpayt = dssmssalary.Tables[2].DefaultView;
            int gettop = 0;
            int rec = 0;
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            string getlink = "";
            dssmssalary.Tables[3].DefaultView.RowFilter = "college_code='" + collegecode1 + "'";
            dvdetails = dssmssalary.Tables[3].DefaultView;
            if (dvdetails.Count > 0)
            {
                getlink = dvdetails[0]["linkvalue"].ToString();
            }
            if (dvmonthlpayt.Count > 0)
            {
                gettop = 15;
                mypdfpage = mydocument.NewPage();
                dssmssalary.Tables[5].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                dvstaffdet = dssmssalary.Tables[5].DefaultView;
                if (dvstaffdet.Count > 0)
                {
                    designation = dvstaffdet[0]["desig_name"].ToString();
                    deptname = dvstaffdet[0]["dept_name"].ToString();
                    staffname = dvstaffdet[0]["staff_name"].ToString();
                    bankno = dvstaffdet[0]["bankaccount"].ToString();
                    pfno = dvstaffdet[0]["pfnumber"].ToString();
                    if (pfno.Trim() == "" || pfno == null)
                    {
                        pfno = " - ";
                    }
                    if (bankno.Trim() == "" || bankno == null)
                    {
                        bankno = " - ";
                    }
                }
                Double totdection = 0;
                dssmssalary.Tables[4].DefaultView.RowFilter = " staff_code='" + staffcode + "'";
                dvpay = dssmssalary.Tables[4].DefaultView;
                if (dvmonthlpayt.Count > 0)
                {
                    string noofpresent = "";
                    string presnt = "";
                    string workdays = "";
                    string leavedays = "";
                    noofpresent = dvmonthlpayt[0]["leavedetail"].ToString();
                    string[] presplit = noofpresent.Split(';');
                    presnt = presplit[1].ToString();
                    workdays = presplit[0].ToString();
                    leavedays = presplit[2].ToString();
                    int starttop = gettop;
                    int coltop = gettop;
                    int rowspace = 15;
                    string setyear = dvmonthlpayt[0]["PayYear"].ToString();
                    if (setyear.Trim() == "" || setyear == null)
                    {
                        setyear = yearto;
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 25, coltop + 10, 400);
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 750, coltop + 10, 400);
                    }
                    PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 0, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                    mypdfpage.Add(ptc);
                    coltop = coltop + rowspace;
                    PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 0, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                    mypdfpage.Add(pts);
                    coltop = coltop + rowspace;
                    PdfTextArea ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 0, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, "Pay Slip for the month of " + ddl_mon.SelectedItem.Text + " " + "  " + setyear + "");
                    mypdfpage.Add(ptsp);
                    string stl = "";
                    Double lop = Convert.ToDouble(dvmonthlpayt[0]["lop"].ToString());
                    string get_pre_lop = dvmonthlpayt[0]["pre_lop"].ToString();
                    string get_cur_lop = dvmonthlpayt[0]["cur_lop"].ToString();
                    double get_pre = 0;
                    double get_cur = 0;
                    double tot_lop = 0;
                    if (get_pre_lop != "")
                    {
                        get_pre = Convert.ToDouble(get_pre_lop);
                    }
                    if (get_cur_lop != "")
                    {
                        get_cur = Convert.ToDouble(get_cur_lop);
                    }
                    tot_lop = get_pre + get_cur;
                    double totalpreset = Convert.ToDouble(workdays) - tot_lop;
                    int maxdays = getmaxdays(monthname1, year3);
                    double[] clleave = new double[50];
                    dayfrm = Convert.ToInt32(dayfrom);
                    dayto = split1[0].ToString();
                    daytonum = Convert.ToInt32(dayto);
                    dssmssalary.Tables[4].DefaultView.RowFilter = " mon_year ='" + monyear + "'  and staff_code='" + staffcode + "'";
                    dvstaffattendance = dssmssalary.Tables[4].DefaultView;
                    if (monthname1 == monthnamenum)
                    {
                        for (int day = dayfrm; dayfrm <= daytonum; dayfrm++)
                        {
                            int day5 = 0;
                            day5 = 0 + dayfrm;
                            if (dvholi.Count > 0)
                            {
                                if (dvstaffattendance.Count > 0)
                                {
                                    int day6;
                                    string attnda = "";
                                    day6 = 3 + day5;
                                    attnda = dvstaffattendance[0][day6].ToString();
                                    if (attnda != "")
                                    {
                                        string[] split100 = attnda.Split(new char[] { '-' });
                                        string mor = "";
                                        string eve = "";
                                        mor = split100[0].ToString();
                                        eve = split100[1].ToString();
                                        for (int le = 1; le <= dvholi.Count; le++)
                                        {
                                            string levatype = leavetype[le];
                                            if (mor.Trim().ToLower() == levatype)
                                            {
                                                clleave[le] = clleave[le] + 0.5;
                                            }
                                            if (eve.Trim().ToLower() == levatype)
                                            {
                                                clleave[le] = clleave[le] + 0.5;
                                            }
                                        }
                                        string[] lave = new string[15];
                                    }
                                }
                            }
                        }
                        for (int le = 1; le <= dvholi.Count; le++)
                        {
                            if (leavetype[le] != "LA")
                            {
                                if (clleave[le] != 0)
                                {
                                    stl = stl + leavetype[le].ToUpper() + "-" + clleave[le] + ", ";
                                }
                            }
                        }
                        if (stl != "")
                        {
                            stl = stl.Substring(0, stl.Length - 2);
                        }
                        Array.Clear(clleave, 0, clleave.Length);
                    }
                    else
                    {
                        dssmssalary.Tables[4].DefaultView.RowFilter = " mon_year ='" + monyear + "'  and staff_code='" + staffcode + "'";
                        dvstaffattendance = dssmssalary.Tables[4].DefaultView;
                        for (int day = dayfrm; dayfrm <= maxdays; dayfrm++)
                        {
                            int day5 = 0;
                            day5 = 0 + dayfrm;
                            if (dvholi.Count > 0)
                            {
                                if (dvstaffattendance.Count > 0)
                                {
                                    int day6;
                                    string attnda = "";
                                    day6 = 3 + day5;
                                    attnda = dvstaffattendance[0][day6].ToString();
                                    if (attnda != "")
                                    {
                                        string[] split100 = attnda.Split(new char[] { '-' });
                                        string mor = "";
                                        string eve = "";
                                        mor = split100[0].ToString();
                                        eve = split100[1].ToString();
                                        for (int le = 1; le <= dvholi.Count; le++)
                                        {
                                            string levatype = leavetype[le];
                                            if (mor.Trim().ToLower() == levatype)
                                            {
                                                clleave[le] = clleave[le] + 0.5;
                                            }
                                            if (eve.Trim().ToLower() == levatype)
                                            {
                                                clleave[le] = clleave[le] + 0.5;
                                            }
                                        }
                                        string[] lave = new string[15];
                                    }
                                }
                            }
                        }
                        for (int le = 1; le <= dvholi.Count; le++)
                        {
                            if (leavetype[le] != "LA")
                            {
                                if (clleave[le] != 0)
                                {
                                    stl = stl + leavetype[le].ToUpper() + "-" + clleave[le] + ", ";
                                }
                            }
                        }
                        dssmssalary.Tables[4].DefaultView.RowFilter = " mon_year='" + monyearto + "' and staff_code='" + staffcode + "'";
                        dvstaffattendance = dssmssalary.Tables[4].DefaultView;
                        for (int day = 1; day <= daytonum; day++)
                        {
                            int day5 = 0;
                            day5 = 0 + dayfrm;
                            if (dvholi.Count > 0)
                            {
                                if (dvstaffattendance.Count > 0)
                                {
                                    int day6;
                                    string attnda = "";
                                    day6 = 3 + day5;
                                    attnda = dvstaffattendance[0][day6].ToString();
                                    if (attnda != "")
                                    {
                                        string[] split100 = attnda.Split(new char[] { '-' });
                                        string mor = "";
                                        string eve = "";
                                        mor = split100[0].ToString();
                                        eve = split100[1].ToString();
                                        for (int le = 1; le <= dvholi.Count; le++)
                                        {
                                            string levatype = leavetype[le];
                                            if (mor.Trim().ToLower() == levatype)
                                            {
                                                clleave[le] = clleave[le] + 0.5;
                                            }
                                            if (eve.Trim().ToLower() == levatype)
                                            {
                                                clleave[le] = clleave[le] + 0.5;
                                            }
                                        }
                                        string[] lave = new string[15];
                                    }
                                }
                            }
                        }
                        for (int le = 1; le <= dvholi.Count; le++)
                        {
                            if (leavetype[le] != "LA")
                            {
                                if (clleave[le] != 0)
                                {
                                    stl = stl + leavetype[le].ToUpper() + "-" + clleave[le] + ", ";
                                }
                            }
                        }
                        if (stl != "")
                        {
                            stl = stl.Substring(0, stl.Length - 2);
                        }
                    }
                    Array.Clear(clleave, 0, clleave.Length);
                    coltop = coltop + 60;
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Staff Code");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, staffcode);
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Department                    :");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, deptname);
                    mypdfpage.Add(ptsp);
                    coltop = coltop + rowspace;
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Staff Name");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, staffname);
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Designation                    :");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, designation);
                    mypdfpage.Add(ptsp);
                    coltop = coltop + rowspace;
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "PF A/C No");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, pfno);
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Bank A/C No                  :");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, bankno);
                    mypdfpage.Add(ptsp);
                    coltop = coltop + rowspace;
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "D.O.B");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1a, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleLeft, ":");
                    mypdfpage.Add(ptsp);
                    string dob = "-";
                    dssmssalary.Tables[6].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                    dvdate = dssmssalary.Tables[6].DefaultView;
                    if (dvdate.Count > 0)
                    {
                        dob = dvdate[0]["dob"].ToString();
                    }
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, dob);
                    mypdfpage.Add(ptsp);
                    coltop = coltop + rowspace;
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "No.of Days Present  :");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, totalpreset.ToString());
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "No.of Working Days       :");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, workdays.ToString());
                    mypdfpage.Add(ptsp);
                    coltop = coltop + rowspace;
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "No.of Leave Taken   :");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, stl);
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "No.of Days Absent/Lop  :");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, tot_lop.ToString() + "/" + lop.ToString());
                    mypdfpage.Add(ptsp);
                    coltop = coltop + rowspace + rowspace;
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Earning");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 190, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Actual Amount");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 310, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Amount");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Deduction");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 760, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Amount");
                    mypdfpage.Add(ptsp);
                    int dedu = coltop + 10;
                    int allo = coltop + 10;
                    double payband = 0;
                    double gradepay = 0;
                    double basic_pay3 = 0;
                    string allowance3 = "";
                    string deduction3 = "";
                    double lopd = 0;
                    double bassicd = 0;
                    double DblAllowLOP = 0;
                    double DblAllowLOP1 = 0;
                    payband = Convert.ToDouble(dvmonthlpayt[0]["pay_band"].ToString());
                    gradepay = Convert.ToDouble(dvmonthlpayt[0]["grade_pay"].ToString());
                    basic_pay3 = Convert.ToDouble(dvmonthlpayt[0]["bsalary"].ToString());
                    double actbasic = Convert.ToDouble(dvmonthlpayt[0]["basic_alone"].ToString());
                    lop = Convert.ToDouble(dvmonthlpayt[0]["lop"].ToString());
                    if (lop != 0)
                    {
                        lopd = Convert.ToDouble(lop);
                    }
                    else
                    {
                        lopd = 0;
                    }
                    bassicd = Convert.ToDouble(basic_pay3);
                    if (bassicd != 0)
                    {
                        allo = allo + rowspace;
                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Basic Pay");
                        mypdfpage.Add(ptsp);
                        actbasic = Math.Round(actbasic, 2, MidpointRounding.AwayFromZero);
                        string alowamount1 = actbasic.ToString();
                        if (!alowamount1.Contains('.'))
                        {
                            alowamount1 = alowamount1 + ".00";
                        }
                        else
                        {
                            string[] sp = alowamount1.Split('.');
                            if (sp[1].Length < 2)
                            {
                                alowamount1 = alowamount1 + "0";
                            }
                        }
                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 230, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, actbasic.ToString());
                        mypdfpage.Add(ptsp);
                        bassicd = Math.Round(bassicd, 2, MidpointRounding.AwayFromZero);
                        string alowamount = bassicd.ToString();
                        if (!alowamount.Contains('.'))
                        {
                            alowamount = alowamount + ".00";
                        }
                        else
                        {
                            string[] sp = alowamount.Split('.');
                            if (sp[1].Length < 2)
                            {
                                alowamount = alowamount + "0";
                            }
                        }
                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, alowamount);
                        mypdfpage.Add(ptsp);
                    }
                    if (gradepay != 0)
                    {
                        allo = allo + rowspace;
                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Grade Pay");
                        mypdfpage.Add(ptsp);
                        gradepay = Math.Round(gradepay, 2, MidpointRounding.AwayFromZero);
                        string alowamount = gradepay.ToString();
                        if (!alowamount.Contains('.'))
                        {
                            alowamount = alowamount + ".00";
                        }
                        else
                        {
                            string[] sp = alowamount.Split('.');
                            if (sp[1].Length < 2)
                            {
                                alowamount = alowamount + "0";
                            }
                        }
                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, alowamount);
                        mypdfpage.Add(ptsp);
                    }
                    if (payband != 0)
                    {
                        allo = allo + rowspace;
                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, left1, allo, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Pay Band");
                        mypdfpage.Add(ptsp);
                        payband = Math.Round(payband, 2, MidpointRounding.AwayFromZero);
                        string alowamount = payband.ToString();
                        if (!alowamount.Contains('.'))
                        {
                            alowamount = alowamount + ".00";
                        }
                        else
                        {
                            string[] sp = alowamount.Split('.');
                            if (sp[1].Length < 2)
                            {
                                alowamount = alowamount + "0";
                            }
                        }
                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 310, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, alowamount);
                        mypdfpage.Add(ptsp);
                    }
                    //******************************Added by srinath 12/1/2015*****************************
                    Dictionary<string, string> dicactaloo = new Dictionary<string, string>();
                    dssmssalary.Tables[5].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                    DataView dvallow = dssmssalary.Tables[5].DefaultView;
                    if (dvallow.Count > 0)
                    {
                        string actall = dvallow[0]["actallowance"].ToString();
                        string[] spactalo = actall.Split('\\');
                        for (int i = 0; i <= spactalo.GetUpperBound(0); i++)
                        {
                            string getactal = spactalo[i];
                            if (getactal.Trim() != "" && getactal != null)
                            {
                                string[] actallspv = getactal.Split(';');
                                if (actallspv.GetUpperBound(0) >= 8)
                                {
                                    if (actallspv[1].ToString().Trim().ToLower() == "percent")
                                    {
                                        Double getrealsalry = 0;
                                        Double getperce = 0;
                                        string getval = "0";
                                        if (actallspv[6].ToString().Trim() == "1")
                                        {
                                            if (actbasic > 0)
                                            {
                                                getrealsalry = Convert.ToDouble(actbasic);
                                            }
                                            if (actallspv[2].ToString().Trim() != "" && actallspv[2].ToString() != null)
                                            {
                                                getperce = Convert.ToDouble(actallspv[2].ToString());
                                            }
                                            Double val = getrealsalry / 100 * getperce;
                                            val = Math.Round(val, 0, MidpointRounding.AwayFromZero);
                                            getval = val.ToString();
                                        }
                                        else if (actallspv[8].ToString().Trim() == "1")
                                        {
                                            if (actbasic > 0)
                                            {
                                                getrealsalry = Convert.ToDouble(actbasic);
                                            }
                                            if (gradepay > 0)
                                            {
                                                getrealsalry = getrealsalry + Convert.ToDouble(gradepay);
                                            }
                                            if (actallspv[2].ToString().Trim() != "" && actallspv[2].ToString() != null)
                                            {
                                                getperce = Convert.ToDouble(actallspv[2].ToString());
                                            }
                                            Double val = getrealsalry / 100 * getperce;
                                            val = Math.Round(val, 0, MidpointRounding.AwayFromZero);
                                            getval = val.ToString();
                                        }
                                        if (!dicactaloo.ContainsKey(actallspv[0].ToString()))
                                        {
                                            dicactaloo.Add(actallspv[0].ToString().Trim().ToLower(), getval);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //*******************END********************************************************************
                    string[] spallow = alloworder.Split(';');
                    for (int alo = 0; alo <= spallow.GetUpperBound(0); alo++)
                    {
                        string[] splalot = spallow[alo].Split('\\');
                        if (splalot.GetUpperBound(0) >= 2)
                        {
                            string orallo = splalot[0].ToString();
                            string setvalu = splalot[2].ToString();
                            int r = 0;
                            allowance3 = dvmonthlpayt[0]["allowances"].ToString();
                            int c = 0;
                            string[] allowanmce_arr1;
                            string alowancesplit;
                            int exatval = 0;
                            allowanmce_arr1 = allowance3.Split('\\');
                            for (int i = c; i <= allowanmce_arr1.GetUpperBound(0); i++)
                            {
                                exatval = allowanmce_arr1.GetUpperBound(0);
                                alowancesplit = allowanmce_arr1[i];
                                string[] allowanceda = alowancesplit.Split(';');
                                if (allowanceda.GetUpperBound(0) >= 3)
                                {
                                    string da = allowanceda[0];
                                    if (da.Trim().ToLower() == orallo.Trim().ToLower())
                                    {
                                        string daac = allowanceda[2];
                                        string[] spval = daac.Split('-');
                                        daac = spval[0];
                                        if (daac.Trim() == "" || daac == null)
                                        {
                                            daac = "0";
                                        }
                                        string da3 = allowanceda[3];
                                        double ds3 = Convert.ToDouble(da3);
                                        ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                        string DblAllowActVal = daac;
                                        string DblAllowVal = allowanceda[3];
                                        DblAllowLOP = (Convert.ToDouble(DblAllowActVal) - Convert.ToDouble(DblAllowVal));
                                        DblAllowLOP = Math.Round(DblAllowLOP);
                                        DblAllowLOP1 = DblAllowLOP1 + DblAllowLOP;
                                        if (ds3 != 0)
                                        {
                                            if (spll_alll_tag_arry.GetUpperBound(0) >= r)
                                            {
                                                if (spll_alll_tag_arry[r] != da)
                                                {
                                                    allo = allo + rowspace;
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, left1, allo, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, setvalu.ToString());
                                                    mypdfpage.Add(ptsp);
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydocument, 230, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, daac);
                                                    mypdfpage.Add(ptsp);
                                                    string alowamount = ds3.ToString();
                                                    if (!alowamount.Contains('.'))
                                                    {
                                                        alowamount = alowamount + ".00";
                                                    }
                                                    else
                                                    {
                                                        string[] sp = alowamount.Split('.');
                                                        if (sp[1].Length < 2)
                                                        {
                                                            alowamount = alowamount + "0";
                                                        }
                                                    }
                                                    ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 320, allo, 50, 50), System.Drawing.ContentAlignment.MiddleRight, alowamount);
                                                    mypdfpage.Add(ptsp);
                                                }
                                                else
                                                {
                                                    if (splallow.Contains(da))
                                                    {
                                                        splallow[da] = da3.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    deduction3 = dvmonthlpayt[0]["deductions"].ToString();
                    foreach (var kvp in deduct)
                    {
                        string setval = kvp.Key.ToString();
                        string setvalva = kvp.Value.ToString();
                        string[] deduction_arr1;
                        string deductionsplit1;
                        deduction_arr1 = deduction3.Split('\\');
                        for (int sp = 0; sp <= deduction_arr1.GetUpperBound(0); sp++)
                        {
                            deductionsplit1 = deduction_arr1[sp];
                            string[] deductionda;
                            deductionda = deductionsplit1.Split(';');
                            if (deductionda.GetUpperBound(0) >= 3)
                            {
                                string de = deductionda[0];
                                string de3 = deductionda[3];
                                double de3d = Convert.ToDouble(de3);
                                de3d = Math.Round(de3d, 2, MidpointRounding.AwayFromZero);
                                if (setval.Trim().ToLower() == de.Trim().ToLower())
                                {
                                    if (de3d != 0)
                                    {
                                        dedu = dedu + rowspace;
                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left3, dedu, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, setval.ToString());
                                        mypdfpage.Add(ptsp);
                                        totdection = totdection + de3d;
                                        string deamount = de3d.ToString();
                                        if (!deamount.Contains('.'))
                                        {
                                            deamount = deamount + ".00";
                                        }
                                        else
                                        {
                                            string[] spd = deamount.Split('.');
                                            if (spd[1].Length < 2)
                                            {
                                                deamount = deamount + "0";
                                            }
                                        }
                                        ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, 770, dedu, 50, 50), System.Drawing.ContentAlignment.MiddleRight, deamount);
                                        mypdfpage.Add(ptsp);
                                    }
                                    sp = deduction_arr1.GetUpperBound(0) + 1;
                                }
                            }
                        }
                    }
                    if (lop != 0)
                        lop = lop + DblAllowLOP1;
                    if (allo > dedu)
                    {
                        if (coltop < allo)
                        {
                            coltop = allo;
                        }
                    }
                    else
                    {
                        if (coltop < dedu)
                        {
                            coltop = dedu;
                        }
                    }
                    coltop = coltop + rowspace + rowspace;
                    Double nesal3 = Convert.ToDouble(dvmonthlpayt[0]["netsal"].ToString());
                    nesal3 = Math.Round(nesal3, 2, MidpointRounding.AwayFromZero);
                    Double totall = Convert.ToDouble(dvmonthlpayt[0]["netadd"].ToString());
                    totall = Math.Round(totall, 2, MidpointRounding.AwayFromZero);
                    double netded = Convert.ToDouble(dvmonthlpayt[0]["netded"].ToString());
                    netded = Math.Round(netded, 2, MidpointRounding.AwayFromZero);
                    string finalamount = totall.ToString();
                    if (!finalamount.Contains('.'))
                    {
                        finalamount = finalamount + ".00";
                    }
                    else
                    {
                        string[] sp = finalamount.Split('.');
                        if (sp[1].Length < 2)
                        {
                            finalamount = finalamount + "0";
                        }
                    }
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 100, 50), System.Drawing.ContentAlignment.MiddleLeft, "Gross Total");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 320, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, finalamount);
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Total Deduction");
                    mypdfpage.Add(ptsp);
                    finalamount = netded.ToString();
                    if (!finalamount.Contains('.'))
                    {
                        finalamount = finalamount + ".00";
                    }
                    else
                    {
                        string[] sp = finalamount.Split('.');
                        if (sp[1].Length < 2)
                        {
                            finalamount = finalamount + "0";
                        }
                    }
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 770, coltop, 50, 50), System.Drawing.ContentAlignment.MiddleRight, finalamount);
                    mypdfpage.Add(ptsp);
                    coltop = coltop + rowspace + rowspace;
                    finalamount = nesal3.ToString();
                    if (!finalamount.Contains('.'))
                    {
                        finalamount = finalamount + ".00";
                    }
                    else
                    {
                        string[] sp = finalamount.Split('.');
                        if (sp[1].Length < 2)
                        {
                            finalamount = finalamount + "0";
                        }
                    }
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left3, coltop, 500, 50), System.Drawing.ContentAlignment.MiddleLeft, "Net Pay : ");
                    mypdfpage.Add(ptsp);
                    ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 670, coltop, 150, 50), System.Drawing.ContentAlignment.MiddleRight, "Rs : " + finalamount);
                    mypdfpage.Add(ptsp);
                    if (rec % 2 != 0)
                    {
                        PdfArea tete = new PdfArea(mydocument, 10, starttop, 830, coltop + 30);
                        PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                        mypdfpage.Add(pr1);
                        if (totlastaff <= rec)
                        {
                            mypdfpage.SaveToDocument();
                        }
                    }
                    else
                    {
                        PdfArea tete = new PdfArea(mydocument, 10, starttop, 830, coltop - gettop + 50);
                        PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                        mypdfpage.Add(pr1);
                        mypdfpage.SaveToDocument();
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = staffname + staffcode + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydocument.SaveToFile(szPath + szFile);
            }
        }
        catch { }
    }
    public void sendemail(string mail, string text, string staffdet)
    {
        try
        {
            bool mailflag = false;
            int countemail = 0;
            if (chkmail.Checked == true)
            {
                string id = mail;
                string msg = text;
                string salary = "Salary";
                strmsg = "";
                string send_mail = "";
                string send_pw = "";
                string to_mail = "";
                string strstuname = "";
                bool flagstudent;
                string strquery = "select massemail,masspwd from collinfo where college_code = " + collegecode1 + " ";
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    send_mail = Convert.ToString(ds.Tables[0].Rows[0]["massemail"]);
                    send_pw = Convert.ToString(ds.Tables[0].Rows[0]["masspwd"]);
                }
                if (id.Trim() != "" && id != null && send_mail.Trim() != "" && send_mail != null && send_pw.Trim() != null && send_pw != null)
                {
                    mailflag = true;
                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                    MailMessage mailmsg = new MailMessage();
                    MailAddress mfrom = new MailAddress(send_mail);
                    mailmsg.From = mfrom;
                    mailmsg.To.Add(id);
                    mailmsg.Subject = salary;
                    mailmsg.IsBodyHtml = true;
                    mailmsg.Body = txtbodycontant.Text;
                    mailmsg.Body = mailmsg.Body + msg;
                    mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = staffdet + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                        string attachementpath = szPath + szFile;
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/Report/" + szFile + "")))
                        {
                            Attachment data = new Attachment(attachementpath);
                            mailmsg.Attachments.Add(data);
                        }
                    }
                    Mail.EnableSsl = true;
                    //ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;

                    NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                    Mail.UseDefaultCredentials = false;
                    Mail.Credentials = credentials;
                    Mail.Send(mailmsg);
                    flagstudent = true;
                    countemail++;
                }
                if (mailflag == true)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Mail has been sent successfully!";
                    return;
                }
                lblsmserror.Visible = false;
            }
        }
        catch (Exception ex)
        {
            //imgdiv2.Visible = true;
            //lbl_alert.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Individual_SalaryReport.aspx");
        }
    }
    public void smsreport(string uril, string mobilenos)
    {
        try
        {
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = "";
            groupmsgid = strvel.Trim().ToString();
            int sms = 0;
            string smsreportinsert = "";
            string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + strmsg + "','" + collegecode1 + "','1','" + date + "' ,'" + Session["UserCode"].ToString() + "')";
                sms = d2.update_method_wo_parameter(smsreportinsert, "Text");
            }
        }
        catch { }
    }
    protected void ddlsmstype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string settext = "";
            if (ddlsmstype.SelectedValue == "Automatic")
            {
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                FpSpread2.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                string year = d2.GetFunction("select CONVERT(CHAR(4), to_date, 120) from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "' and college_Code=" + collegecode1 + "");
                settext = "Your salary for the month of " + ddl_mon.SelectedItem.Text + " - " + year + "- Rs.$Salary$ has been credited to your account-$Account No$";
                txtsms.Enabled = false;
            }
            else if (ddlsmstype.SelectedValue == "Template With Salary")
            {
                lblpurpose1.Visible = true;
                ddlpurpose.Visible = true;
                FpSpread2.Visible = true;
                btnaddtemplate.Visible = true;
                btndeletetemplate.Visible = true;
                txtsms.Enabled = true;
            }
            else if (ddlsmstype.SelectedValue == "Template With Out Salary")
            {
                lblpurpose1.Visible = true;
                ddlpurpose.Visible = true;
                FpSpread2.Visible = true;
                btnaddtemplate.Visible = true;
                btndeletetemplate.Visible = true;
                txtsms.Enabled = true;
            }
            else if (ddlsmstype.SelectedValue == "Template With LOP")
            {
                lblpurpose1.Visible = false;
                ddlpurpose.Visible = false;
                FpSpread2.Visible = false;
                btnaddtemplate.Visible = false;
                btndeletetemplate.Visible = false;
                string year = d2.GetFunction("select CONVERT(CHAR(4), to_date, 120) from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "' and college_Code=" + collegecode1 + "");
                settext = "Your salary for the month of " + ddl_mon.SelectedItem.Text + " - " + year + " is Rs. $Salary$ with  $ Lop Days $ LOP(s) - (LOP Date(s)).";
                txtsms.Enabled = false;
            }
            txtsms.Text = settext;
        }
        catch { }
    }
    protected void ddlpurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread2.Visible = true;
        try
        {
            FpSpread2.Sheets[0].ColumnHeaderVisible = false;
            FpSpread2.Sheets[0].SheetCorner.Columns[0].Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Visible = true;
            ddlpurpose.Visible = true;
            FpSpread2.Sheets[0].RowCount = 1;
            FpSpread2.Sheets[0].ColumnCount = 2;
            FpSpread2.Columns[1].Width = 900;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = "Template";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
            string gfg = ddlpurpose.SelectedValue.ToString();
            string gfvgj = ddlpurposemsg.Text;
            if (gfg == " ")
            {
                ds.Dispose();
                ds.Reset();
                string spread2query = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template";
                ds = d2.select_method(spread2query, hat, "Text");
            }
            else
            {
                string spread2query1 = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from sms_template where temp_code = " + ddlpurpose.SelectedValue + "";
                ds = d2.select_method(spread2query1, hat, "Text");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
                }
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.SaveChanges();
        }
        catch { }
    }
    public void bindpurpose()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            ddlpurpose.Items.Clear();
            ddlpurposemsg.Items.Clear();
            string strpurposename = "select purpose,temp_code from sms_purpose where college_code = '" + collegecode1 + "'";
            ds = d2.select_method(strpurposename, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpurpose.DataSource = ds;
                ddlpurpose.DataTextField = "Purpose";
                ddlpurpose.DataValueField = "temp_code";
                ddlpurpose.DataBind();
                ddlpurpose.Items.Add(" ");
                ddlpurpose.Text = " ";
                ddlpurposemsg.DataSource = ds;
                ddlpurposemsg.DataTextField = "Purpose";
                ddlpurposemsg.DataValueField = "temp_code";
                ddlpurposemsg.DataBind();
                ddlpurposemsg.Items.Add(" ");
                ddlpurposemsg.Text = " ";
            }
        }
        catch { }
    }
    protected void btnsum_Click(object sender, EventArgs e)
    {
        templatepanel.Enabled = false;
        purposepanel.Visible = true;
        lblpurposecaption.Visible = true;
        txtpurposecaption.Visible = true;
        btnpurposeadd.Visible = true;
        btnpurposeexit.Visible = true;
    }
    protected void btndiff_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string strdelpurpose = "Delete from sms_purpose where temp_code = '" + ddlpurposemsg.SelectedValue + "'";
            i = d2.insert_method(strdelpurpose, hat, "Text");
            if (i == 1)
            {
                lblerrorpur.Text = "Purpose deleted Successfully";
                lblerrorpur.Visible = true;
                bindpurpose();
            }
            else
            {
                lblerrorpur.Text = "Purpose deleted Failed";
                lblerrorpur.Visible = true;
            }
        }
        catch { }
    }
    protected void txtpurposemsg_TextChanged(object sender, EventArgs e)
    {
    }
    protected void btnsavepur_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string strsavequery = "insert into sms_template (temp_code,Template,college_code)values( '" + ddlpurposemsg.SelectedValue.ToString() + "','" + txtpurposemsg.Text.ToString() + "','" + collegecode1 + "')";
            i = d2.insert_method(strsavequery, hat, "Text");
            if (i == 1)
            {
                lblerrorpur.Visible = true;
                lblerrorpur.Text = "Template added Succefully";
                bindpurpose();
            }
            else
            {
                lblerrorpur.Text = "Template added failed";
            }
        }
        catch { }
    }
    protected void btnexitpur_Click(object sender, EventArgs e)
    {
        templatepanel.Visible = false;
        purposepanel.Visible = false;
        bindpurpose();
    }
    protected void btnpurposeadd_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string strtxtpurpose = string.Empty;
            strtxtpurpose = txtpurposecaption.Text;
            if (strtxtpurpose != "")
            {
                string strinsertpurpose = "insert into sms_purpose (Purpose,college_code) values ( '" + strtxtpurpose + "','" + collegecode1 + "')";
                i = d2.insert_method(strinsertpurpose, hat, "Text");
                if (i == 1)
                {
                    lblerrorpur.Text = "Purpose added Successfully";
                    lblerrorpur.Visible = true;
                    bindpurpose();
                    ddlpurposemsg.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
                    ddlpurpose.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
                }
                else
                {
                    lblerrorpur.Text = "Purpose added failed";
                    lblerrorpur.Visible = true;
                }
            }
            else
            {
                //lblsendmail.Text = "Please Enter the Purpose";
                //lblsendmail.Visible = true;
            }
            txtpurposecaption.Text = "";
            Spread2Go();
        }
        catch { }
    }
    protected void btnpurposeexit_Click(object sender, EventArgs e)
    {
        templatepanel.Enabled = true;
        purposepanel.Visible = false;
    }
    protected void btnaddtemplate_Click(object sender, EventArgs e)
    {
        FpSpread2.Visible = true;
        templatepanel.Visible = true;
        lblpurpose.Visible = true;
        btnplus.Visible = true;
        btnminus.Visible = true;
        ddlpurpose.Visible = true;
        txtpurposemsg.Visible = true;
        btnsavepur.Visible = true;
        btnexitpur.Visible = true;
        lblerrorpur.Visible = false;
        bindpurpose();
    }
    protected void btndeletetemplate_Click(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
            if (Cellclick == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());
                if (ar != -1)
                {
                    string msg = FpSpread2.Sheets[0].GetText(ar, 1);
                    string strdeletequery = "delete   sms_template where Template='" + msg + "'";
                    int vvv = d2.insert_method(strdeletequery, hat, "");
                    if (vvv == 1)
                    {
                        lblerrorpur.Visible = true;
                        lblerrorpur.Text = "Delete Template Succefully";
                    }
                    else
                    {
                        lblerrorpur.Text = "Delete Template  failed";
                    }
                }
                Spread2Go();
                Cellclick = false;
            }
        }
        catch { }
    }
    protected void FpSpread2_CellClick(object sender, EventArgs e)
    {
        Cellclick = true;
    }
    protected void FpSpread2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());
                if (ar != -1 && ar != 0)
                {
                    txtsms.Text = FpSpread2.Sheets[0].GetText(ar, 1);
                }
                else
                {
                    txtsms.Text = "";
                }
                Cellclick = false;
            }
        }
        catch { }
    }
    protected void btnemailok_Click(object sender, EventArgs e)
    {
        txtsubject.Text = "";
        txtbodycontant.Text = "";
    }
    protected void btnemailcancel_Click(object sender, EventArgs e)
    {
        modalpoppupemail.Hide();
    }
    protected void btnsmsok_Click(object sender, EventArgs e)
    {
    }
    protected void btnsmscancel_Click(object sender, EventArgs e)
    {
        modalpopupex1.Hide();
    }
    protected void btnprintset_click(object sender, EventArgs e)
    {
        try
        {
            NewFunt();
            Printcontrol.Visible = false;
            lblsmserror.Visible = false;
            int headalign = 550;
            int pdfheight = 600;
            Font Fonthead;
            Font FontBodyhead;
            Font FontBody;
            Font Fonttablehead;
            string pagenamedet = "";
            DataSet dsimg = new DataSet();
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument();
            string pagerowcount = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Pagecount Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
            if (ddladdreason.SelectedIndex != 0)
            {
                pagenamedet = Convert.ToString(ddladdreason.SelectedItem.Text);
            }
            else
            {
                pagenamedet = "";
            }
            string printdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Pagesize Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
            string pageno = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Include Page No' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
            string showheader = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Show Header All' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
            string showfooter = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Show Footer All' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
            string showtot = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Begin Grand Total' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
            string setpadding = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Set Cell Padding' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
            if (printdetails == "0")
            {
                mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            }
            if (printdetails == "1")
            {
                mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(60, 40));
                headalign = 1655;
                pdfheight = 1000;
            }
            else
            {
                mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            }
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontboldnew = new System.Drawing.Font("Book Antiqua", 13, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold16 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Fonthead = new Font("Book Antiqua", 7, FontStyle.Bold);
            FontBody = new Font("Book Antiqua", 6, FontStyle.Regular);
            FontBodyhead = new Font("Book Antiqua", 6, FontStyle.Bold);
            Fonttablehead = new Font("Book Antiqua", 10, FontStyle.Bold);
            Gios.Pdf.PdfPage mypdfpage;
            PdfTextArea collinfo1 = new PdfTextArea(Fontbold, Color.Blue, new PdfArea(mydoc, 0, 0, 20, 20), ContentAlignment.TopCenter, "");
            Gios.Pdf.PdfTable tableforfooter;
            Gios.Pdf.PdfTable tableforspread;
            Gios.Pdf.PdfTablePage pdfspreadtab;
            int coltop = 0;
            int xpos = 0;
            int imgpos = 0;
            int getxpos = 0;
            int pfsal = 0;
            if (chkShowPF.Checked == true)
                Int32.TryParse(Convert.ToString(Session["PFSal"]), out pfsal);
            double headwidth = 0;
            int spreadrowcount = 0;
            string Collvalue = "";
            coltop = coltop + 50;
            int splitrows = 0;
            int finalrows = 0;
            int visrowcount = 0;
            int tabheight = 0;
            int rows = 0;
            if (pagerowcount != "0" && pagerowcount != "")
            {
                rows = Convert.ToInt32(pagerowcount);
            }
            else
            {
                rows = 16;
            }
            for (int i = 1; i < fpsalary.Sheets[0].RowCount; i++)
            {
                if (fpsalary.Sheets[0].Rows[i].Visible == true)
                {
                    visrowcount++;
                }
            }
            if (cblcolumnorder.Items[53].Selected == true && fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL")
            {
                spreadrowcount = visrowcount - 2;
            }
            else if (fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Text == "TOTAL" && cblcolumnorder.Items[53].Selected == false)
            {
                spreadrowcount = visrowcount - 1;
            }
            else
            {
                spreadrowcount = fpsalary.Sheets[0].RowCount;
            }
            if (spreadrowcount >= rows)
            {
                splitrows = spreadrowcount / rows;
                finalrows = spreadrowcount % rows;
            }
            int startrow = 0;
            int st = 1;
            bool firstPagePrev = false;
            bool headerchk = false;
            bool footerchk = false;
            int rowcount = 0;
            int a = 0;
            int padding = 0;
            int pagenum = 0;
            int inithead = 0;
            Hashtable htPgoverTotal = new Hashtable();
            string strquery = "Select * from Collinfo where college_code=" + collegecode1 + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string collegedetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Header Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
            if (printdetails == "0")
            {
                if (setpadding.Trim() != "" && setpadding.Trim() != "0")
                {
                    Int32.TryParse(setpadding, out padding);
                }
                else
                {
                    padding = 5;
                }
                headwidth = mydoc.PageWidth;
            }
            if (printdetails == "1")
            {
                if (setpadding.Trim() != "" && setpadding.Trim() != "0")
                {
                    Int32.TryParse(setpadding, out padding);
                }
                else
                {
                    padding = 10;
                }
                headwidth = mydoc.PageWidth;
            }
            #region for Multiple Pages
            if (splitrows > 0)
            {
                st = 1;
                for (int ro = 0; ro < splitrows; ro++)
                {
                    pagenum++;
                    int widthcount = 30;
                    coltop = 30;
                    mypdfpage = mydoc.NewPage();
                    if (printdetails == "0")
                    {
                        xpos = 0;
                        imgpos = 480;
                    }
                    if (printdetails == "1")
                    {
                        xpos = 450;
                        imgpos = 1600;
                    }
                    else
                    {
                        xpos = 0;
                        imgpos = 480;
                    }
                    #region for HeaderCheck
                    if (collegedetails.Trim() != "" && collegedetails.Trim() != "0")
                    {
                        if (showheader == "0" && headerchk == false)
                        {
                            headerchk = true;
                            //bindcollegedetails(mypdfpage, collegedetails, mydoc, xpos, coltop, headwidth, collinfo1, Fontbold, Fontbolda, getxpos, imgpos);
                            string[] spiltcollegedetails = collegedetails.Split(',');
                            for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
                            {
                                getxpos = spiltcollegedetails.GetUpperBound(0) * 15 + 15;
                                coltop = coltop + 15;
                                string collinfo = spiltcollegedetails[i].ToString();
                                string collname = Convert.ToString(chkcollege.Items[Convert.ToInt32(collinfo)].Text);
                                if (collname == "College Name")
                                {
                                    collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["collname"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "University")
                                {
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["university"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Affliated By")
                                {
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Address")
                                {
                                    string address1 = ds.Tables[0].Rows[0]["Address1"].ToString();
                                    string address2 = ds.Tables[0].Rows[0]["Address2"].ToString();
                                    string address3 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                    {
                                        Collvalue = address1;
                                    }
                                    if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + address2;
                                        }
                                        else
                                        {
                                            Collvalue = address2;
                                        }
                                    }
                                    if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + address3;
                                        }
                                        else
                                        {
                                            Collvalue = address3;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "City")
                                {
                                    string address1 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                    {
                                        Collvalue = address1;
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "District & State & Pincode")
                                {
                                    string district = ds.Tables[0].Rows[0]["district"].ToString();
                                    string state = ds.Tables[0].Rows[0]["State"].ToString();
                                    string pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                                    if (district.Trim() != "" && district != null && district.Length > 1)
                                    {
                                        Collvalue = district;
                                    }
                                    if (state.Trim() != "" && state != null && state.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + state;
                                        }
                                        else
                                        {
                                            Collvalue = state;
                                        }
                                    }
                                    if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + '-' + pincode;
                                        }
                                        else
                                        {
                                            Collvalue = pincode;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Phone No & Fax")
                                {
                                    string phone = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                                    string fax = ds.Tables[0].Rows[0]["Faxno"].ToString();
                                    if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                    {
                                        Collvalue = "Phone :" + phone;
                                    }
                                    if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + " , Fax : " + fax;
                                        }
                                        else
                                        {
                                            Collvalue = "Fax :" + fax;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Email & Web Site")
                                {
                                    string email = ds.Tables[0].Rows[0]["Email"].ToString();
                                    string website = ds.Tables[0].Rows[0]["Website"].ToString();
                                    if (email.Trim() != "" && email != null && email.Length > 1)
                                    {
                                        Collvalue = "Email :" + email;
                                    }
                                    if (website.Trim() != "" && website != null && website.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + " , Web Site : " + website;
                                        }
                                        else
                                        {
                                            Collvalue = "Web Site :" + website;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Left Logo")
                                {
                                    try
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                        {
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                            mypdfpage.Add(LogoImage, 25, 25, 400);
                                        }
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                        {
                                            string leftlogo = "Left_Logo(" + collegecode1 + ")";
                                            MemoryStream memoryStream = new MemoryStream();
                                            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                            mypdfpage.Add(LogoImage, 25, 25, 400);
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                    catch { }
                                }
                                else if (collname == "Right Logo")
                                {
                                    try
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                        {
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                            mypdfpage.Add(LogoImage, imgpos, 25, 400);
                                        }
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                        {
                                            string rightlogo = "Right_Logo(" + collegecode1 + ")";
                                            MemoryStream memoryStream = new MemoryStream();
                                            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                            mypdfpage.Add(LogoImage, imgpos, 25, 400);
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                        else if (showheader == "1")
                        {
                            string[] spiltcollegedetails = collegedetails.Split(',');
                            for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
                            {
                                getxpos = spiltcollegedetails.GetUpperBound(0) * 15 + 15;
                                coltop = coltop + 15;
                                string collinfo = spiltcollegedetails[i].ToString();
                                string collname = Convert.ToString(chkcollege.Items[Convert.ToInt32(collinfo)].Text);
                                if (collname == "College Name")
                                {
                                    collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["collname"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "University")
                                {
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["university"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Affliated By")
                                {
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Address")
                                {
                                    string address1 = ds.Tables[0].Rows[0]["Address1"].ToString();
                                    string address2 = ds.Tables[0].Rows[0]["Address2"].ToString();
                                    string address3 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                    {
                                        Collvalue = address1;
                                    }
                                    if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + address2;
                                        }
                                        else
                                        {
                                            Collvalue = address2;
                                        }
                                    }
                                    if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + address3;
                                        }
                                        else
                                        {
                                            Collvalue = address3;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "City")
                                {
                                    string address1 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                    {
                                        Collvalue = address1;
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "District & State & Pincode")
                                {
                                    string district = ds.Tables[0].Rows[0]["district"].ToString();
                                    string state = ds.Tables[0].Rows[0]["State"].ToString();
                                    string pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                                    if (district.Trim() != "" && district != null && district.Length > 1)
                                    {
                                        Collvalue = district;
                                    }
                                    if (state.Trim() != "" && state != null && state.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + ',' + state;
                                        }
                                        else
                                        {
                                            Collvalue = state;
                                        }
                                    }
                                    if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + '-' + pincode;
                                        }
                                        else
                                        {
                                            Collvalue = pincode;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Phone No & Fax")
                                {
                                    string phone = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                                    string fax = ds.Tables[0].Rows[0]["Faxno"].ToString();
                                    if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                    {
                                        Collvalue = "Phone :" + phone;
                                    }
                                    if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + " , Fax : " + fax;
                                        }
                                        else
                                        {
                                            Collvalue = "Fax :" + fax;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Email & Web Site")
                                {
                                    string email = ds.Tables[0].Rows[0]["Email"].ToString();
                                    string website = ds.Tables[0].Rows[0]["Website"].ToString();
                                    if (email.Trim() != "" && email != null && email.Length > 1)
                                    {
                                        Collvalue = "Email :" + email;
                                    }
                                    if (website.Trim() != "" && website != null && website.Length > 1)
                                    {
                                        if (Collvalue.Trim() != "" && Collvalue != null)
                                        {
                                            Collvalue = Collvalue + " , Web Site : " + website;
                                        }
                                        else
                                        {
                                            Collvalue = "Web Site :" + website;
                                        }
                                    }
                                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                    mypdfpage.Add(collinfo1);
                                }
                                else if (collname == "Left Logo")
                                {
                                    try
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                        {
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                            mypdfpage.Add(LogoImage, 25, 25, 400);
                                        }
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                        {
                                            string leftlogo = "Left_Logo(" + collegecode1 + ")";
                                            MemoryStream memoryStream = new MemoryStream();
                                            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                            mypdfpage.Add(LogoImage, 25, 25, 400);
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                    catch { }
                                }
                                else if (collname == "Right Logo")
                                {
                                    try
                                    {
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                        {
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                            mypdfpage.Add(LogoImage, imgpos, 25, 400);
                                        }
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                        {
                                            string rightlogo = "Right_Logo(" + collegecode1 + ")";
                                            MemoryStream memoryStream = new MemoryStream();
                                            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                            mypdfpage.Add(LogoImage, imgpos, 25, 400);
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    #endregion
                    if (pagenamedet != "")
                    {
                        if (inithead > 0)
                        {
                            coltop = coltop + 15;
                            collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop + 15, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + pagenamedet + "");
                            mypdfpage.Add(collinfo1);
                        }
                        else
                        {
                            coltop = getxpos + 15;
                            collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop + 15, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + pagenamedet + "");
                            mypdfpage.Add(collinfo1);
                        }
                    }
                    if (pageno != "0" && pageno != "")
                    {
                        if (inithead > 0)
                        {
                            if (printdetails == "1")
                            {
                                xpos = 1575;
                            }
                            else
                            {
                                xpos = 490;
                            }
                            coltop = coltop + 45;
                            collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, xpos, coltop, 80, 50), System.Drawing.ContentAlignment.TopRight, "Page :" + "" + pagenum + "");
                            mypdfpage.Add(collinfo1);
                        }
                        else
                        {
                            if (printdetails == "1")
                            {
                                xpos = 1575;
                            }
                            else
                            {
                                xpos = 490;
                            }
                            coltop = getxpos + 75;
                            collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, xpos, coltop, 80, 50), System.Drawing.ContentAlignment.TopRight, "Page :" + "" + pagenum + "");
                            mypdfpage.Add(collinfo1);
                        }
                    }
                    if (inithead > 0)
                    {
                        coltop = coltop + 60;
                    }
                    else
                    {
                        coltop = getxpos + 90;
                    }
                    if (firstPagePrev == false)
                    {
                        rowcount = rows;
                        a = 0;
                        rowcount = rows + 1;
                        a = a + 1;
                        rowcount = rowcount + 1;
                        a = a + 1;
                    }
                    else
                    {
                        rowcount = rows;
                        a = 0;
                        rowcount = rows + 2;
                        a = a + 2;
                        if (showtot == "1")
                        {
                            rowcount = rowcount + 1;
                            a = a + 1;
                        }
                    }
                    tableforspread = mydoc.NewTable(Fontsmall, rowcount, fpsalary.Sheets[0].ColumnCount, padding);
                    tableforspread.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    tableforspread.VisibleHeaders = false;
                    string spreadval = "";
                    Hashtable htPgTotal = new Hashtable();
                    for (int i = startrow; i <= rows; i++)
                    {
                        if (st < fpsalary.Sheets[0].RowCount)
                        {
                            tabheight += 50;
                            for (int j = 0; j < fpsalary.Sheets[0].ColumnCount; j++)
                            {
                                if (fpsalary.Sheets[0].Columns[j].Visible == true)
                                {
                                    if (i == 0)
                                    {
                                        spreadval = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[i, j].Text).Trim();
                                        if (spreadval == "Allowance" || spreadval == "Deduction" || spreadval == "")
                                        {
                                            spreadval = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[1, j].Text).Trim();
                                        }
                                        else if (spreadval == "S.No")
                                        {
                                            tableforspread.Columns[0].SetWidth(60);
                                        }
                                        if (spreadval.Trim() != "")
                                        {
                                            if (ColumnWidth.ContainsKey(spreadval))
                                            {
                                                tableforspread.Columns[j].SetWidth(Convert.ToInt32(Convert.ToString(ColumnWidth[spreadval])));
                                                widthcount += Convert.ToInt32(Convert.ToString(ColumnWidth[spreadval]));
                                            }
                                        }
                                        tableforspread.Cell(i, j).SetContent(spreadval);
                                        tableforspread.Cell(i, j).SetContentAlignment(ContentAlignment.TopCenter);
                                        tableforspread.Cell(i, j).SetFont(Fontboldnew);
                                        tableforspread.Cell(i, j).SetCellPadding(5);
                                    }
                                    else if (i == 1)
                                    {
                                        string colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[1, j].Text).Trim();
                                        if (colname.Trim() == "")
                                        {
                                            colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                        }
                                        string empt = "";
                                        bool EntryBF = false;
                                        bool EntryCO = false;
                                        if (st > 1)
                                        {
                                            empt = Convert.ToString(htPgoverTotal[colname]);
                                            if (firstPagePrev == true)
                                            {
                                                if (showtot == "1")
                                                {
                                                    if (empt == "0")
                                                    {
                                                        tableforspread.Cell((i + a) - 3, j).SetContent("-");
                                                    }
                                                    else
                                                    {
                                                        tableforspread.Cell((i + a) - 3, j).SetContent(empt);
                                                    }
                                                    tableforspread.Cell((i + a) - 3, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                    tableforspread.Cell((i + a) - 3, 1).SetContent("B/F");
                                                    EntryBF = true;
                                                    tableforspread.Cell((i + a) - 3, 1).SetFont(Fontbold);
                                                    tableforspread.Cell((i + a) - 3, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                                    tableforspread.Rows[(i + a) - 3].SetCellPadding(5);
                                                    foreach (PdfCell pc in tableforspread.CellRange((i + a) - 3, 1, (i + a) - 3, 1).Cells)
                                                    {
                                                        pc.ColSpan = 1;
                                                    }
                                                }
                                            }
                                        }
                                        if (fpsalary.Sheets[0].Rows[st].Visible == true)
                                        {
                                            spreadval = Convert.ToString(fpsalary.Sheets[0].Cells[st, j].Text).Trim();
                                            string spreadval1 = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                            if (spreadval1.Trim() != "SIGNATURE")
                                            {
                                                if (spreadval.Trim() != "" && spreadval.Trim() != "0")
                                                {
                                                    tableforspread.Cell((i + a) - 2, j).SetContent(spreadval);
                                                }
                                                else
                                                {
                                                    tableforspread.Cell((i + a) - 2, j).SetContent("-");
                                                }
                                            }
                                            if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE")
                                            {
                                                tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopCenter);
                                            }
                                            else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                            {
                                                tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopLeft);
                                            }
                                            else
                                            {
                                                if (j != 0)
                                                {
                                                    if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE")
                                                    {
                                                        tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                    }
                                                    else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                                    {
                                                        tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopLeft);
                                                    }
                                                    else
                                                    {
                                                        tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                    }
                                                }
                                            }
                                            if (EntryBF == true && showtot.Trim() == "1" && cblcolumnorder.Items[55].Selected == true)
                                                tableforspread.Rows[(i + a) - 2].SetCellPadding(5);
                                            else if (EntryBF == false && showtot.Trim() == "1" && cblcolumnorder.Items[55].Selected == true)
                                                tableforspread.Rows[(i + a) - 2].SetRowHeight(100);
                                            else
                                                tableforspread.Rows[(i + a) - 2].SetCellPadding(padding);
                                        }
                                        else
                                        {
                                            i--;
                                            goto nextline;
                                        }
                                        if (j > 0)
                                        {
                                            double amnt = 0;
                                            double prevAmt = 0;
                                            double prevOvamt = 0;
                                            double.TryParse(spreadval, out amnt);
                                            double.TryParse(Convert.ToString(htPgTotal[colname]), out prevAmt);
                                            if (htPgTotal.Contains(colname))
                                                htPgTotal.Remove(colname);
                                            htPgTotal.Add(colname, (amnt + prevAmt));
                                            if (htPgTotal.ContainsKey("STAFF CODE") || htPgTotal.ContainsKey("NAME") || htPgTotal.ContainsKey("DESIGN") || htPgTotal.ContainsKey("DEPT") || htPgTotal.ContainsKey("DEPT ACR") || htPgTotal.ContainsKey("DESIGN ACR") || htPgTotal.ContainsKey("DATE OF BIRTH") || htPgTotal.ContainsKey("DATE OF APPOINTED") || htPgTotal.ContainsKey("DATE OF JOINING") || htPgTotal.ContainsKey("Date of Resigned") || htPgTotal.ContainsKey("DATE OF INCREMENT") || htPgTotal.ContainsKey("") || htPgTotal.ContainsKey("CATEGORY") || htPgTotal.ContainsKey("STAFF TYPE") || htPgTotal.ContainsKey("BANK ACCOUNT NO") || htPgTotal.ContainsKey("PF NO") || htPgTotal.ContainsKey("ESI NO") || htPgTotal.ContainsKey("LOP DAYS") || htPgTotal.ContainsKey("LOP DATES") || htPgTotal.ContainsKey("WORKING DAYS") || htPgTotal.ContainsKey("ABSENT DAYS") || htPgTotal.ContainsKey("PRESENT DAYS") || htPgTotal.ContainsKey("NO. OF INCREMENT") || htPgTotal.ContainsKey("PAY SCALE") || htPgTotal.ContainsKey("TITLE") || htPgTotal.ContainsKey("BANK FORMAT") || htPgTotal.ContainsKey("SIGNATURE"))
                                                htPgTotal.Remove(colname);
                                            double.TryParse(Convert.ToString(htPgoverTotal[colname]), out prevOvamt);
                                            if (htPgoverTotal.Contains(colname))
                                                htPgoverTotal.Remove(colname);
                                            htPgoverTotal.Add(colname, (amnt + prevOvamt));
                                            if (htPgoverTotal.ContainsKey("STAFF CODE") || htPgoverTotal.ContainsKey("NAME") || htPgoverTotal.ContainsKey("DESIGN") || htPgoverTotal.ContainsKey("DEPT") || htPgoverTotal.ContainsKey("DEPT ACR") || htPgoverTotal.ContainsKey("DESIGN ACR") || htPgoverTotal.ContainsKey("DATE OF BIRTH") || htPgoverTotal.ContainsKey("DATE OF APPOINTED") || htPgoverTotal.ContainsKey("DATE OF JOINING") || htPgoverTotal.ContainsKey("Date of Resigned") || htPgoverTotal.ContainsKey("DATE OF INCREMENT") || htPgoverTotal.ContainsKey("") || htPgoverTotal.ContainsKey("CATEGORY") || htPgoverTotal.ContainsKey("STAFF TYPE") || htPgoverTotal.ContainsKey("BANK ACCOUNT NO") || htPgoverTotal.ContainsKey("PF NO") || htPgoverTotal.ContainsKey("ESI NO") || htPgoverTotal.ContainsKey("LOP DAYS") || htPgoverTotal.ContainsKey("LOP DATES") || htPgoverTotal.ContainsKey("WORKING DAYS") || htPgoverTotal.ContainsKey("ABSENT DAYS") || htPgoverTotal.ContainsKey("PRESENT DAYS") || htPgoverTotal.ContainsKey("NO. OF INCREMENT") || htPgoverTotal.ContainsKey("PAY SCALE") || htPgoverTotal.ContainsKey("TITLE") || htPgoverTotal.ContainsKey("BANK FORMAT") || htPgoverTotal.ContainsKey("SIGNATURE"))
                                                htPgoverTotal.Remove(colname);
                                        }
                                        if (i == rows)
                                        {
                                            if (j > 0)
                                            {
                                                if (st == spreadrowcount)
                                                {
                                                    if (Convert.ToString(htPgoverTotal[colname]) == "0")
                                                    {
                                                        tableforspread.Cell(i + a - 1, j).SetContent("-");
                                                    }
                                                    else
                                                    {
                                                        tableforspread.Cell(i + a - 1, j).SetContent(Convert.ToString(htPgoverTotal[colname]));
                                                    }
                                                    tableforspread.Cell(i + a - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                    tableforspread.Cell(i + a - 1, 1).SetContent("C/O");
                                                    EntryCO = true;
                                                    tableforspread.Cell(i + a - 1, 1).SetFont(Fontbold);
                                                    tableforspread.Cell(i + a - 1, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                                    tableforspread.Rows[(i + a) - 1].SetCellPadding(padding);
                                                    foreach (PdfCell pc in tableforspread.CellRange(i + a - 1, 1, i + a - 1, 1).Cells)
                                                    {
                                                        pc.ColSpan = 1;
                                                    }
                                                }
                                            }
                                        }
                                        if ((EntryCO == true || EntryBF == true) && showtot.Trim() == "1" && cblcolumnorder.Items[55].Selected == true)
                                            tableforspread.Rows[1].SetCellPadding(5);
                                        else if (EntryBF == false && EntryCO == false && showtot.Trim() == "1" && cblcolumnorder.Items[55].Selected == true)
                                            tableforspread.Rows[1].SetRowHeight(100);
                                        else
                                            tableforspread.Cell(1, j).SetCellPadding(padding);
                                    }
                                    else
                                    {
                                        if (fpsalary.Sheets[0].Rows[st].Visible == true)
                                        {
                                            string colname = "";
                                            colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[1, j].Text).Trim();
                                            if (colname.Trim() == "")
                                            {
                                                colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                            }
                                            spreadval = Convert.ToString(fpsalary.Sheets[0].Cells[st, j].Text).Trim();
                                            string spreadval1 = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                            if (spreadval1.Trim() != "SIGNATURE")
                                            {
                                                if (spreadval.Trim() != "" && spreadval.Trim() != "0")
                                                {
                                                    tableforspread.Cell((i + a) - 2, j).SetContent(spreadval);
                                                }
                                                else
                                                {
                                                    tableforspread.Cell((i + a) - 2, j).SetContent("-");
                                                }
                                            }
                                            if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE" || spreadval1 == "DATE OF APPOINTED" || spreadval1 == "DATE OF BIRTH" || spreadval1 == "DATE OF JOINING" || spreadval1 == "Date of Resigned" || spreadval1 == "DATE OF INCREMENT")
                                            {
                                                tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopCenter);
                                            }
                                            else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                            {
                                                tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopLeft);
                                            }
                                            else
                                            {
                                                if (j != 0)
                                                {
                                                    if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE" || spreadval1 == "DATE OF APPOINTED" || spreadval1 == "DATE OF BIRTH" || spreadval1 == "DATE OF JOINING" || spreadval1 == "Date of Resigned" || spreadval1 == "DATE OF INCREMENT")
                                                    {
                                                        tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                    }
                                                    else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                                    {
                                                        tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopLeft);
                                                    }
                                                    else
                                                    {
                                                        tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                    }
                                                }
                                            }
                                            if (showtot.Trim() == "1" && cblcolumnorder.Items[55].Selected == true)
                                                tableforspread.Rows[(i + a) - 2].SetRowHeight(100);
                                            else
                                                tableforspread.Rows[(i + a) - 2].SetCellPadding(padding);
                                            if (j > 0)
                                            {
                                                double amnt = 0;
                                                double prevAmt = 0;
                                                double prevOvamt = 0;
                                                double.TryParse(spreadval, out amnt);
                                                double.TryParse(Convert.ToString(htPgTotal[colname]), out prevAmt);
                                                if (htPgTotal.Contains(colname))
                                                    htPgTotal.Remove(colname);
                                                htPgTotal.Add(colname, (amnt + prevAmt));
                                                if (htPgTotal.ContainsKey("STAFF CODE") || htPgTotal.ContainsKey("NAME") || htPgTotal.ContainsKey("DESIGN") || htPgTotal.ContainsKey("DEPT") || htPgTotal.ContainsKey("DEPT ACR") || htPgTotal.ContainsKey("DESIGN ACR") || htPgTotal.ContainsKey("DATE OF BIRTH") || htPgTotal.ContainsKey("DATE OF APPOINTED") || htPgTotal.ContainsKey("DATE OF JOINING") || htPgTotal.ContainsKey("Date of Resigned") || htPgTotal.ContainsKey("DATE OF INCREMENT") || htPgTotal.ContainsKey("") || htPgTotal.ContainsKey("CATEGORY") || htPgTotal.ContainsKey("STAFF TYPE") || htPgTotal.ContainsKey("BANK ACCOUNT NO") || htPgTotal.ContainsKey("PF NO") || htPgTotal.ContainsKey("ESI NO") || htPgTotal.ContainsKey("LOP DAYS") || htPgTotal.ContainsKey("LOP DATES") || htPgTotal.ContainsKey("WORKING DAYS") || htPgTotal.ContainsKey("ABSENT DAYS") || htPgTotal.ContainsKey("PRESENT DAYS") || htPgTotal.ContainsKey("NO. OF INCREMENT") || htPgTotal.ContainsKey("PAY SCALE") || htPgTotal.ContainsKey("TITLE") || htPgTotal.ContainsKey("BANK FORMAT") || htPgTotal.ContainsKey("SIGNATURE"))
                                                    htPgTotal.Remove(colname);
                                                double.TryParse(Convert.ToString(htPgoverTotal[colname]), out prevOvamt);
                                                if (htPgoverTotal.Contains(colname))
                                                    htPgoverTotal.Remove(colname);
                                                htPgoverTotal.Add(colname, (amnt + prevOvamt));
                                                if (htPgoverTotal.ContainsKey("STAFF CODE") || htPgoverTotal.ContainsKey("NAME") || htPgoverTotal.ContainsKey("DESIGN") || htPgoverTotal.ContainsKey("DEPT") || htPgoverTotal.ContainsKey("DEPT ACR") || htPgoverTotal.ContainsKey("DESIGN ACR") || htPgoverTotal.ContainsKey("DATE OF BIRTH") || htPgoverTotal.ContainsKey("DATE OF APPOINTED") || htPgoverTotal.ContainsKey("DATE OF JOINING") || htPgoverTotal.ContainsKey("Date of Resigned") || htPgoverTotal.ContainsKey("DATE OF INCREMENT") || htPgoverTotal.ContainsKey("") || htPgoverTotal.ContainsKey("CATEGORY") || htPgoverTotal.ContainsKey("STAFF TYPE") || htPgoverTotal.ContainsKey("BANK ACCOUNT NO") || htPgoverTotal.ContainsKey("PF NO") || htPgoverTotal.ContainsKey("ESI NO") || htPgoverTotal.ContainsKey("LOP DAYS") || htPgoverTotal.ContainsKey("LOP DATES") || htPgoverTotal.ContainsKey("WORKING DAYS") || htPgoverTotal.ContainsKey("ABSENT DAYS") || htPgoverTotal.ContainsKey("PRESENT DAYS") || htPgoverTotal.ContainsKey("NO. OF INCREMENT") || htPgoverTotal.ContainsKey("PAY SCALE") || htPgoverTotal.ContainsKey("TITLE") || htPgoverTotal.ContainsKey("BANK FORMAT") || htPgoverTotal.ContainsKey("SIGNATURE"))
                                                    htPgoverTotal.Remove(colname);
                                            }
                                            if (st != rowcount)
                                            {
                                                if (i == rows)
                                                {
                                                    if (j > 0)
                                                    {
                                                        if (showtot == "1")
                                                        {
                                                            if (Convert.ToString(htPgoverTotal[colname]) == "0")
                                                            {
                                                                tableforspread.Cell(i + a - 1, j).SetContent("-");
                                                            }
                                                            else
                                                            {
                                                                tableforspread.Cell(i + a - 1, j).SetContent(Convert.ToString(htPgoverTotal[colname]));
                                                            }
                                                            tableforspread.Cell(i + a - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                            tableforspread.Cell(i + a - 1, 1).SetContent("C/O");
                                                            tableforspread.Cell(i + a - 1, 1).SetFont(Fontbold);
                                                            tableforspread.Cell(i + a - 1, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                                            tableforspread.Rows[(i + a) - 1].SetCellPadding(5);
                                                            foreach (PdfCell pc in tableforspread.CellRange(i + a - 1, 1, i + a - 1, 1).Cells)
                                                            {
                                                                pc.ColSpan = 1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToString(htPgTotal[colname]) == "0")
                                                            {
                                                                tableforspread.Cell(i + a - 1, j).SetContent("-");
                                                            }
                                                            else
                                                            {
                                                                tableforspread.Cell(i + a - 1, j).SetContent(Convert.ToString(htPgTotal[colname]));
                                                            }
                                                            tableforspread.Cell(i + a - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                            tableforspread.Cell(i + a - 1, 1).SetContent("C/O");
                                                            tableforspread.Cell(i + a - 1, 1).SetFont(Fontbold);
                                                            tableforspread.Cell(i + a - 1, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                                            tableforspread.Rows[(i + a) - 1].SetCellPadding(5);
                                                            foreach (PdfCell pc in tableforspread.CellRange(i + a - 1, 1, i + a - 1, 1).Cells)
                                                            {
                                                                pc.ColSpan = 1;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            tableforspread.Cell(i, j).SetCellPadding(padding);
                                        }
                                        else
                                        {
                                            i--;
                                            goto nextline;
                                        }
                                    }
                                }
                            }
                        }
                    nextline:
                        if (startrow != 0)
                        {
                            st++;
                        }
                        startrow++;
                        continue;
                    }
                    startrow = 0;
                    if (widthcount > headalign)
                    {
                        pdfspreadtab = tableforspread.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, headalign, pdfheight));
                        mypdfpage.Add(pdfspreadtab);
                    }
                    else
                    {
                        Double leftarrange = Math.Round(Convert.ToDouble((headalign - widthcount) / 2), 0);
                        pdfspreadtab = tableforspread.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, coltop, widthcount, pdfheight));
                        mypdfpage.Add(pdfspreadtab);
                    }
                    Double tblheight = 0;
                    if (printdetails == "0")
                    {
                        tblheight = pdfspreadtab.Area.Height + coltop + 50;
                    }
                    else
                    {
                        tblheight = pdfspreadtab.Area.Height + coltop;
                    }
                    if (ro == splitrows - 1 && finalrows == 0)
                    {
                        if (cblcolumnorder.Items[53].Selected == true && fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL")
                        {
                            string[] spl = new string[2];
                            string strnetsal = Convert.ToString(fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].Text);
                            if (widthcount > headalign)
                            {
                                collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, tblheight, headalign, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString(strnetsal));
                                mypdfpage.Add(collinfo1);
                            }
                            else
                            {
                                Double leftarrange = Math.Round(Convert.ToDouble((headalign - widthcount) / 2), 0);
                                collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, leftarrange, tblheight, widthcount, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString(strnetsal));
                                mypdfpage.Add(collinfo1);
                            }
                            if (pfsal > 0)
                            {
                                collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, tblheight + 50, headalign, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString("PF Salary  :  " + ConvertNumbertoWords(pfsal)));
                                mypdfpage.Add(collinfo1);
                            }
                        }
                        else
                        {
                            if (pfsal > 0)
                            {
                                collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, tblheight, headalign, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString("PF Salary  :  " + ConvertNumbertoWords(pfsal)));
                                mypdfpage.Add(collinfo1);
                            }
                        }
                    }
                    coltop = Convert.ToInt32(tblheight) + 100;
                    string footerdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Footer Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
                    if (footerdetails.Trim() != "" && footerdetails.Trim() != "0")
                    {
                        if (showfooter == "1")
                        {
                            string[] spiltfooterdetails = footerdetails.Split(',');
                            tableforfooter = mydoc.NewTable(Fontbold16, 3, spiltfooterdetails.Length, 3);
                            tableforfooter.SetBorders(Color.Black, 1, BorderType.None);
                            for (int i = 0; i <= spiltfooterdetails.GetUpperBound(0); i++)
                            {
                                string collfooter = spiltfooterdetails[i].ToString();
                                tableforfooter.Cell(0, i).SetContent(collfooter);
                                tableforfooter.Cell(2, 0).SetContentAlignment(ContentAlignment.BottomRight);
                            }
                            Gios.Pdf.PdfTablePage pdftabpage = tableforfooter.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 0, coltop, headalign, 400));
                            mypdfpage.Add(pdftabpage);
                        }
                        else
                        {
                            if (finalrows == 0 && st == rowcount)
                            {
                                string[] spiltfooterdetails = footerdetails.Split(',');
                                tableforfooter = mydoc.NewTable(Fontbold16, 3, spiltfooterdetails.Length, 3);
                                tableforfooter.SetBorders(Color.Black, 1, BorderType.None);
                                for (int i = 0; i <= spiltfooterdetails.GetUpperBound(0); i++)
                                {
                                    string collfooter = spiltfooterdetails[i].ToString();
                                    tableforfooter.Cell(0, i).SetContent(collfooter);
                                    tableforfooter.Cell(2, 0).SetContentAlignment(ContentAlignment.BottomRight);
                                }
                                Gios.Pdf.PdfTablePage pdftabpage = tableforfooter.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 0, coltop, headalign, 400));
                                mypdfpage.Add(pdftabpage);
                            }
                        }
                    }
                    mypdfpage.SaveToDocument();
                    firstPagePrev = true;
                    if (showheader == "0")
                    {
                        inithead++;
                    }
                }
            }
            #endregion
            #region for Final Page
            if (finalrows > 0)
            {
                pagenum++;
                int widthcount = 30;
                coltop = 30;
                mypdfpage = mydoc.NewPage();
                if (printdetails == "0")
                {
                    xpos = 0;
                    imgpos = 480;
                }
                if (printdetails == "1")
                {
                    xpos = 450;
                    imgpos = 1600;
                }
                else
                {
                    xpos = 0;
                    imgpos = 480;
                }
                tabheight = 0;
                #region for HeaderCheck
                if (collegedetails.Trim() != "" && collegedetails.Trim() != "0")
                {
                    if (showheader == "1")
                    {
                        string[] spiltcollegedetails = collegedetails.Split(',');
                        for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
                        {
                            getxpos = spiltcollegedetails.GetUpperBound(0) * 15 + 15;
                            coltop = coltop + 15;
                            string collinfo = spiltcollegedetails[i].ToString();
                            string collname = Convert.ToString(chkcollege.Items[Convert.ToInt32(collinfo)].Text);
                            if (collname == "College Name")
                            {
                                collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["collname"].ToString() + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "University")
                            {
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["university"].ToString() + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Affliated By")
                            {
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Address")
                            {
                                string address1 = ds.Tables[0].Rows[0]["Address1"].ToString();
                                string address2 = ds.Tables[0].Rows[0]["Address2"].ToString();
                                string address3 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                {
                                    Collvalue = address1;
                                }
                                if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + address2;
                                    }
                                    else
                                    {
                                        Collvalue = address2;
                                    }
                                }
                                if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + address3;
                                    }
                                    else
                                    {
                                        Collvalue = address3;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "City")
                            {
                                string address1 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                {
                                    Collvalue = address1;
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "District & State & Pincode")
                            {
                                string district = ds.Tables[0].Rows[0]["district"].ToString();
                                string state = ds.Tables[0].Rows[0]["State"].ToString();
                                string pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                                if (district.Trim() != "" && district != null && district.Length > 1)
                                {
                                    Collvalue = district;
                                }
                                if (state.Trim() != "" && state != null && state.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + state;
                                    }
                                    else
                                    {
                                        Collvalue = state;
                                    }
                                }
                                if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + '-' + pincode;
                                    }
                                    else
                                    {
                                        Collvalue = pincode;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Phone No & Fax")
                            {
                                string phone = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                                string fax = ds.Tables[0].Rows[0]["Faxno"].ToString();
                                if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                {
                                    Collvalue = "Phone :" + phone;
                                }
                                if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + " , Fax : " + fax;
                                    }
                                    else
                                    {
                                        Collvalue = "Fax :" + fax;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Email & Web Site")
                            {
                                string email = ds.Tables[0].Rows[0]["Email"].ToString();
                                string website = ds.Tables[0].Rows[0]["Website"].ToString();
                                if (email.Trim() != "" && email != null && email.Length > 1)
                                {
                                    Collvalue = "Email :" + email;
                                }
                                if (website.Trim() != "" && website != null && website.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + " , Web Site : " + website;
                                    }
                                    else
                                    {
                                        Collvalue = "Web Site :" + website;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Left Logo")
                            {
                                try
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                    {
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                        mypdfpage.Add(LogoImage, 25, 25, 400);
                                    }
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                    {
                                        string leftlogo = "Left_Logo(" + collegecode1 + ")";
                                        MemoryStream memoryStream = new MemoryStream();
                                        byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                        mypdfpage.Add(LogoImage, 25, 25, 400);
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                                catch { }
                            }
                            else if (collname == "Right Logo")
                            {
                                try
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                    {
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                        mypdfpage.Add(LogoImage, imgpos, 25, 400);
                                    }
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                    {
                                        string rightlogo = "Right_Logo(" + collegecode1 + ")";
                                        MemoryStream memoryStream = new MemoryStream();
                                        byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                        mypdfpage.Add(LogoImage, imgpos, 25, 400);
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
                #endregion
                if (pagenamedet != "")
                {
                    if (inithead > 0)
                    {
                        //if (printdetails == "1")
                        //{
                        //    xpos = 450;
                        //}
                        //else
                        //{
                        //    xpos = 0;
                        //}
                        coltop = coltop + 15;
                        collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop + 15, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + pagenamedet + "");
                        mypdfpage.Add(collinfo1);
                    }
                    else
                    {
                        coltop = getxpos + 15;
                        collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop + 15, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + pagenamedet + "");
                        mypdfpage.Add(collinfo1);
                    }
                }
                if (pageno != "0" && pageno != "")
                {
                    if (inithead > 0)
                    {
                        if (printdetails == "1")
                        {
                            xpos = 1575;
                        }
                        else
                        {
                            xpos = 490;
                        }
                        coltop = coltop + 45;
                        collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, xpos, coltop, 80, 50), System.Drawing.ContentAlignment.TopRight, "Page :" + "" + pagenum + "");
                        mypdfpage.Add(collinfo1);
                    }
                    else
                    {
                        if (printdetails == "1")
                        {
                            xpos = 1575;
                        }
                        else
                        {
                            xpos = 490;
                        }
                        coltop = getxpos + 75;
                        collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, xpos, coltop, 80, 50), System.Drawing.ContentAlignment.TopRight, "Page :" + "" + pagenum + "");
                        mypdfpage.Add(collinfo1);
                    }
                }
                if (inithead > 0)
                {
                    coltop = coltop + 60;
                }
                else
                {
                    coltop = getxpos + 90;
                }
                rowcount = finalrows;
                a = 0;
                rowcount = finalrows + 1;
                a = a + 1;
                if (showtot == "1")
                {
                    rowcount = rowcount + 1;
                    a = a + 1;
                }
                if (cblcolumnorder.Items[53].Selected == true && fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL")
                {
                    rowcount = rowcount + 1;
                }
                if (fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Text == "TOTAL" && cblcolumnorder.Items[53].Selected == false)
                {
                    rowcount = rowcount + 1;
                }
                tableforspread = mydoc.NewTable(Fontsmall, rowcount, fpsalary.Sheets[0].ColumnCount, padding);
                tableforspread.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                tableforspread.VisibleHeaders = false;
                string spreadval = "";
                int dynamicrow = 0;
                Hashtable htPgTotal = new Hashtable();
                bool chk = false;
                for (int i = startrow; i < rowcount; i++)
                {
                    if (st < fpsalary.Sheets[0].RowCount)
                    {
                        tabheight += 50;
                        for (int j = 0; j < fpsalary.Sheets[0].ColumnCount; j++)
                        {
                            if (i == 0)
                            {
                                spreadval = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                if (spreadval == "Allowance" || spreadval == "Deduction" || spreadval == "")
                                {
                                    spreadval = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[1, j].Text).Trim();
                                }
                                if (spreadval == "S.No")
                                {
                                    tableforspread.Columns[0].SetWidth(60);
                                }
                                if (spreadval.Trim() != "")
                                {
                                    if (ColumnWidth.ContainsKey(spreadval))
                                    {
                                        //tableforspread.Columns[j].SetWidth(Convert.ToInt32(fpsalary.Sheets[0].Columns[j].Width));
                                        //widthcount += Convert.ToInt32(fpsalary.Sheets[0].Columns[j].Width);
                                        //if (lastval.Trim().Length >= 8)
                                        //{
                                        //    tableforspread.Columns[j].SetWidth(100);
                                        //    widthcount += 100;
                                        //}
                                        //else
                                        //{
                                        tableforspread.Columns[j].SetWidth(Convert.ToInt32(Convert.ToString(ColumnWidth[spreadval])));
                                        widthcount += Convert.ToInt32(Convert.ToString(ColumnWidth[spreadval]));
                                        //}
                                    }
                                }
                                tableforspread.Cell(0, j).SetContent(spreadval);
                                tableforspread.Cell(0, j).SetContentAlignment(ContentAlignment.TopCenter);
                                tableforspread.Cell(0, j).SetFont(Fontboldnew);
                                tableforspread.Cell(i, j).SetCellPadding(5);
                            }
                            else if (i == 1)
                            {
                                bool EntryBF = false;
                                string colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[1, j].Text).Trim();
                                if (colname.Trim() == "")
                                {
                                    colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                }
                                string empt = "";
                                if (st > 1)
                                {
                                    empt = Convert.ToString(htPgoverTotal[colname]);
                                    if (showtot == "1")
                                    {
                                        if (empt == "0")
                                        {
                                            tableforspread.Cell((i + a) - 2, j).SetContent("-");
                                        }
                                        else
                                        {
                                            tableforspread.Cell((i + a) - 2, j).SetContent(empt);
                                        }
                                        tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tableforspread.Cell((i + a) - 2, 1).SetContent("B/F");
                                        tableforspread.Cell((i + a) - 2, 1).SetFont(Fontbold);
                                        tableforspread.Cell((i + a) - 2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tableforspread.Rows[(i + a) - 2].SetCellPadding(5);
                                        foreach (PdfCell pc in tableforspread.CellRange((i + a) - 2, 1, (i + a) - 2, 1).Cells)
                                        {
                                            pc.ColSpan = 1;
                                        }
                                    }
                                    else
                                    {
                                        spreadval = Convert.ToString(fpsalary.Sheets[0].Cells[st, j].Text).Trim();
                                        string spreadval1 = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                        if (spreadval1.Trim() != "SIGNATURE")
                                        {
                                            if (spreadval.Trim() != "" && spreadval.Trim() != "0")
                                            {
                                                tableforspread.Cell((i + a) - 1, j).SetContent(spreadval);
                                            }
                                            else
                                            {
                                                tableforspread.Cell((i + a) - 1, j).SetContent("-");
                                            }
                                        }
                                        if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE" || spreadval1 == "DATE OF APPOINTED" || spreadval1 == "DATE OF BIRTH" || spreadval1 == "DATE OF JOINING" || spreadval1 == "Date of Resigned" || spreadval1 == "DATE OF INCREMENT")
                                        {
                                            tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                        }
                                        else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                        {
                                            tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopLeft);
                                        }
                                        else
                                        {
                                            if (j != 0)
                                            {
                                                if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE" || spreadval1 == "DATE OF APPOINTED" || spreadval1 == "DATE OF BIRTH" || spreadval1 == "DATE OF JOINING" || spreadval1 == "Date of Resigned" || spreadval1 == "DATE OF INCREMENT")
                                                {
                                                    tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                }
                                                else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                                {
                                                    tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopLeft);
                                                }
                                                else
                                                {
                                                    tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                }
                                            }
                                        }
                                        if (EntryBF == true && showtot.Trim() == "1" && cblcolumnorder.Items[55].Selected == true)
                                            tableforspread.Rows[(i + a) - 1].SetCellPadding(5);
                                        else if (EntryBF == false && showtot.Trim() == "1" && cblcolumnorder.Items[55].Selected == true)
                                            tableforspread.Rows[(i + a) - 1].SetRowHeight(100);
                                        else
                                            tableforspread.Rows[(i + a) - 1].SetCellPadding(padding);
                                    }
                                    if (j > 0)
                                    {
                                        double amnt = 0;
                                        double prevAmt = 0;
                                        double prevOvamt = 0;
                                        double.TryParse(spreadval, out amnt);
                                        double.TryParse(Convert.ToString(htPgTotal[colname]), out prevAmt);
                                        if (htPgTotal.Contains(colname))
                                            htPgTotal.Remove(colname);
                                        htPgTotal.Add(colname, (amnt + prevAmt));
                                        if (htPgTotal.ContainsKey("STAFF CODE") || htPgTotal.ContainsKey("NAME") || htPgTotal.ContainsKey("DESIGN") || htPgTotal.ContainsKey("DEPT") || htPgTotal.ContainsKey("DEPT ACR") || htPgTotal.ContainsKey("DESIGN ACR") || htPgTotal.ContainsKey("DATE OF BIRTH") || htPgTotal.ContainsKey("DATE OF APPOINTED") || htPgTotal.ContainsKey("DATE OF JOINING") || htPgTotal.ContainsKey("Date of Resigned") || htPgTotal.ContainsKey("DATE OF INCREMENT") || htPgTotal.ContainsKey("") || htPgTotal.ContainsKey("CATEGORY") || htPgTotal.ContainsKey("STAFF TYPE") || htPgTotal.ContainsKey("BANK ACCOUNT NO") || htPgTotal.ContainsKey("PF NO") || htPgTotal.ContainsKey("ESI NO") || htPgTotal.ContainsKey("LOP DAYS") || htPgTotal.ContainsKey("LOP DATES") || htPgTotal.ContainsKey("WORKING DAYS") || htPgTotal.ContainsKey("ABSENT DAYS") || htPgTotal.ContainsKey("PRESENT DAYS") || htPgTotal.ContainsKey("NO. OF INCREMENT") || htPgTotal.ContainsKey("PAY SCALE") || htPgTotal.ContainsKey("TITLE") || htPgTotal.ContainsKey("BANK FORMAT") || htPgTotal.ContainsKey("SIGNATURE"))
                                            htPgTotal.Remove(colname);
                                        double.TryParse(Convert.ToString(htPgoverTotal[colname]), out prevOvamt);
                                        if (htPgoverTotal.Contains(colname))
                                            htPgoverTotal.Remove(colname);
                                        htPgoverTotal.Add(colname, (amnt + prevOvamt));
                                        if (htPgoverTotal.ContainsKey("STAFF CODE") || htPgoverTotal.ContainsKey("NAME") || htPgoverTotal.ContainsKey("DESIGN") || htPgoverTotal.ContainsKey("DEPT") || htPgoverTotal.ContainsKey("DEPT ACR") || htPgoverTotal.ContainsKey("DESIGN ACR") || htPgoverTotal.ContainsKey("DATE OF BIRTH") || htPgoverTotal.ContainsKey("DATE OF APPOINTED") || htPgoverTotal.ContainsKey("DATE OF JOINING") || htPgoverTotal.ContainsKey("Date of Resigned") || htPgoverTotal.ContainsKey("DATE OF INCREMENT") || htPgoverTotal.ContainsKey("") || htPgoverTotal.ContainsKey("CATEGORY") || htPgoverTotal.ContainsKey("STAFF TYPE") || htPgoverTotal.ContainsKey("BANK ACCOUNT NO") || htPgoverTotal.ContainsKey("PF NO") || htPgoverTotal.ContainsKey("ESI NO") || htPgoverTotal.ContainsKey("LOP DAYS") || htPgoverTotal.ContainsKey("LOP DATES") || htPgoverTotal.ContainsKey("WORKING DAYS") || htPgoverTotal.ContainsKey("ABSENT DAYS") || htPgoverTotal.ContainsKey("PRESENT DAYS") || htPgoverTotal.ContainsKey("NO. OF INCREMENT") || htPgoverTotal.ContainsKey("PAY SCALE") || htPgoverTotal.ContainsKey("TITLE") || htPgoverTotal.ContainsKey("BANK FORMAT") || htPgoverTotal.ContainsKey("SIGNATURE"))
                                            htPgoverTotal.Remove(colname);
                                    }
                                }
                                if (EntryBF == true && showtot.Trim() == "1" && cblcolumnorder.Items[55].Selected == true)
                                    tableforspread.Rows[1].SetCellPadding(5);
                                else if (EntryBF == false && showtot.Trim() == "1" && cblcolumnorder.Items[55].Selected == true)
                                    tableforspread.Rows[1].SetRowHeight(100);
                                else
                                    tableforspread.Cell(1, j).SetCellPadding(padding);
                            }
                            else
                            {
                                if (fpsalary.Sheets[0].Rows[st].Visible == true)
                                {
                                    string colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[1, j].Text).Trim();
                                    if (colname.Trim() == "")
                                    {
                                        colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                    }
                                    if (showtot == "1")
                                    {
                                        if (chk == false)
                                        {
                                            st = st - 1;
                                            chk = true;
                                        }
                                    }
                                    spreadval = Convert.ToString(fpsalary.Sheets[0].Cells[st, j].Text).Trim();
                                    string spreadval1 = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                    if (spreadval1.Trim() != "SIGNATURE")
                                    {
                                        if (spreadval.Trim() != "" && spreadval.Trim() != "0")
                                        {
                                            if (showtot == "0")
                                            {
                                                tableforspread.Cell((i + a) - 1, j).SetContent(spreadval);
                                            }
                                            else
                                            {
                                                tableforspread.Cell((i + a) - 2, j).SetContent(spreadval);
                                            }
                                        }
                                        else
                                        {
                                            if (showtot == "0")
                                            {
                                                tableforspread.Cell((i + a) - 1, j).SetContent("-");
                                            }
                                            else
                                            {
                                                tableforspread.Cell((i + a) - 2, j).SetContent("-");
                                            }
                                        }
                                    }
                                    if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE")
                                    {
                                        if (showtot == "0")
                                        {
                                            tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                        }
                                        else
                                        {
                                            tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopCenter);
                                        }
                                    }
                                    else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                    {
                                        if (showtot == "0")
                                        {
                                            tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopLeft);
                                        }
                                        else
                                        {
                                            tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopLeft);
                                        }
                                    }
                                    else
                                    {
                                        if (j != 0)
                                        {
                                            if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE")
                                            {
                                                if (showtot == "0")
                                                {
                                                    tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                }
                                                else
                                                {
                                                    tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                }
                                            }
                                            else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                            {
                                                if (showtot == "0")
                                                {
                                                    tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopLeft);
                                                }
                                                else
                                                {
                                                    tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopLeft);
                                                }
                                            }
                                            else
                                            {
                                                if (showtot == "0")
                                                {
                                                    tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                }
                                                else
                                                {
                                                    tableforspread.Cell((i + a) - 2, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                }
                                            }
                                        }
                                    }
                                    if (showtot == "0" && cblcolumnorder.Items[55].Selected == true)
                                    {
                                        tableforspread.Rows[(i + a) - 1].SetCellPadding(padding);
                                    }
                                    else if (showtot == "1" && cblcolumnorder.Items[55].Selected == true)
                                    {
                                        if (i < rowcount - 1)
                                            tableforspread.Rows[(i + a) - 2].SetRowHeight(100);
                                        else
                                            tableforspread.Rows[(i + a) - 2].SetCellPadding(padding);
                                    }
                                    if (i == rowcount - 1)
                                    {
                                        tableforspread.Rows[i].SetCellPadding(5);
                                        tableforspread.Rows[i].SetFont(Fontboldnew);
                                    }
                                    if (j > 0)
                                    {
                                        double amnt = 0;
                                        double prevAmt = 0;
                                        double prevOvamt = 0;
                                        double.TryParse(spreadval, out amnt);
                                        double.TryParse(Convert.ToString(htPgTotal[colname]), out prevAmt);
                                        if (htPgTotal.Contains(colname))
                                            htPgTotal.Remove(colname);
                                        htPgTotal.Add(colname, (amnt + prevAmt));
                                        if (htPgTotal.ContainsKey("STAFF CODE") || htPgTotal.ContainsKey("NAME") || htPgTotal.ContainsKey("DESIGN") || htPgTotal.ContainsKey("DEPT") || htPgTotal.ContainsKey("DEPT ACR") || htPgTotal.ContainsKey("DESIGN ACR") || htPgTotal.ContainsKey("DATE OF BIRTH") || htPgTotal.ContainsKey("DATE OF APPOINTED") || htPgTotal.ContainsKey("DATE OF JOINING") || htPgTotal.ContainsKey("Date of Resigned") || htPgTotal.ContainsKey("DATE OF INCREMENT") || htPgTotal.ContainsKey("") || htPgTotal.ContainsKey("CATEGORY") || htPgTotal.ContainsKey("STAFF TYPE") || htPgTotal.ContainsKey("BANK ACCOUNT NO") || htPgTotal.ContainsKey("PF NO") || htPgTotal.ContainsKey("ESI NO") || htPgTotal.ContainsKey("LOP DAYS") || htPgTotal.ContainsKey("LOP DATES") || htPgTotal.ContainsKey("WORKING DAYS") || htPgTotal.ContainsKey("ABSENT DAYS") || htPgTotal.ContainsKey("PRESENT DAYS") || htPgTotal.ContainsKey("NO. OF INCREMENT") || htPgTotal.ContainsKey("PAY SCALE") || htPgTotal.ContainsKey("TITLE") || htPgTotal.ContainsKey("BANK FORMAT") || htPgTotal.ContainsKey("SIGNATURE"))
                                            htPgTotal.Remove(colname);
                                        double.TryParse(Convert.ToString(htPgoverTotal[colname]), out prevOvamt);
                                        if (htPgoverTotal.Contains(colname))
                                            htPgoverTotal.Remove(colname);
                                        htPgoverTotal.Add(colname, (amnt + prevOvamt));
                                        if (htPgoverTotal.ContainsKey("STAFF CODE") || htPgoverTotal.ContainsKey("NAME") || htPgoverTotal.ContainsKey("DESIGN") || htPgoverTotal.ContainsKey("DEPT") || htPgoverTotal.ContainsKey("DEPT ACR") || htPgoverTotal.ContainsKey("DESIGN ACR") || htPgoverTotal.ContainsKey("DATE OF BIRTH") || htPgoverTotal.ContainsKey("DATE OF APPOINTED") || htPgoverTotal.ContainsKey("DATE OF JOINING") || htPgoverTotal.ContainsKey("Date of Resigned") || htPgoverTotal.ContainsKey("DATE OF INCREMENT") || htPgoverTotal.ContainsKey("") || htPgoverTotal.ContainsKey("CATEGORY") || htPgoverTotal.ContainsKey("STAFF TYPE") || htPgoverTotal.ContainsKey("BANK ACCOUNT NO") || htPgoverTotal.ContainsKey("PF NO") || htPgoverTotal.ContainsKey("ESI NO") || htPgoverTotal.ContainsKey("LOP DAYS") || htPgoverTotal.ContainsKey("LOP DATES") || htPgoverTotal.ContainsKey("WORKING DAYS") || htPgoverTotal.ContainsKey("ABSENT DAYS") || htPgoverTotal.ContainsKey("PRESENT DAYS") || htPgoverTotal.ContainsKey("NO. OF INCREMENT") || htPgoverTotal.ContainsKey("PAY SCALE") || htPgoverTotal.ContainsKey("TITLE") || htPgoverTotal.ContainsKey("BANK FORMAT") || htPgoverTotal.ContainsKey("SIGNATURE"))
                                            htPgoverTotal.Remove(colname);
                                    }
                                    //tableforspread.Cell(i, j).SetCellPadding(padding);
                                }
                                else
                                {
                                    i--;
                                    goto Outer;
                                }
                            }
                        }
                    }
                Outer:
                    if (startrow != 0)
                    {
                        st++;
                    }
                    startrow++;
                    dynamicrow++;
                    continue;
                }
                startrow = 0;
                if (widthcount > headalign)
                {
                    pdfspreadtab = tableforspread.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, headalign, pdfheight));
                    mypdfpage.Add(pdfspreadtab);
                }
                else
                {
                    Double leftarrange = Math.Round(Convert.ToDouble((headalign - widthcount) / 2), 0);
                    pdfspreadtab = tableforspread.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, coltop, widthcount, pdfheight));
                    mypdfpage.Add(pdfspreadtab);
                }
                Double tblheight = 0;
                if (printdetails == "0")
                {
                    tblheight = pdfspreadtab.Area.Height + coltop + 50;
                }
                else
                {
                    tblheight = pdfspreadtab.Area.Height + coltop + 50;
                }
                if (cblcolumnorder.Items[53].Selected == true && fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL")
                {
                    string[] spl = new string[2];
                    string strnetsal = Convert.ToString(fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].Text);
                    if (widthcount > headalign)
                    {
                        collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, tblheight, headalign, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString(strnetsal));
                        mypdfpage.Add(collinfo1);
                    }
                    else
                    {
                        Double leftarrange = Math.Round(Convert.ToDouble((headalign - widthcount) / 2), 0);
                        collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, leftarrange, tblheight, widthcount, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString(strnetsal));
                        mypdfpage.Add(collinfo1);
                    }
                    if (pfsal > 0)
                    {
                        collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, tblheight + 50, headalign, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString("PF Salary  :  " + ConvertNumbertoWords(pfsal)));
                        mypdfpage.Add(collinfo1);
                    }
                }
                else
                {
                    if (pfsal > 0)
                    {
                        collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, tblheight, headalign, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString("PF Salary  :  " + ConvertNumbertoWords(pfsal)));
                        mypdfpage.Add(collinfo1);
                    }
                }
                coltop = Convert.ToInt32(tblheight) + 100;
                string footerdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Footer Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
                if (footerdetails.Trim() != "" && footerdetails.Trim() != "0")
                {
                    string[] spiltfooterdetails = footerdetails.Split(',');
                    tableforfooter = mydoc.NewTable(Fontbold16, 3, spiltfooterdetails.Length, 3);
                    tableforfooter.SetBorders(Color.Black, 1, BorderType.None);
                    for (int i = 0; i <= spiltfooterdetails.GetUpperBound(0); i++)
                    {
                        string collfooter = spiltfooterdetails[i].ToString();
                        tableforfooter.Cell(0, i).SetContent(collfooter);
                        tableforfooter.Cell(2, 0).SetContentAlignment(ContentAlignment.BottomRight);
                    }
                    Gios.Pdf.PdfTablePage pdftabpage = tableforfooter.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 0, coltop, headalign, 400));
                    mypdfpage.Add(pdftabpage);
                }
                mypdfpage.SaveToDocument();
                if (showheader == "0")
                {
                    inithead++;
                }
            }
            #endregion
            #region for Single Page
            if (splitrows == 0 && finalrows == 0)
            {
                if (spreadrowcount > 0)
                {
                    pagenum++;
                    int widthcount = 30;
                    coltop = 30;
                    mypdfpage = mydoc.NewPage();
                    if (printdetails == "0")
                    {
                        xpos = 0;
                        imgpos = 480;
                    }
                    if (printdetails == "1")
                    {
                        xpos = 450;
                        imgpos = 1600;
                    }
                    else
                    {
                        xpos = 0;
                        imgpos = 480;
                    }
                    tabheight = 0;
                    if (collegedetails.Trim() != "" && collegedetails.Trim() != "0")
                    {
                        string[] spiltcollegedetails = collegedetails.Split(',');
                        for (int i = 0; i <= spiltcollegedetails.GetUpperBound(0); i++)
                        {
                            getxpos = spiltcollegedetails.GetUpperBound(0) * 15 + 15;
                            coltop = coltop + 15;
                            string collinfo = spiltcollegedetails[i].ToString();
                            string collname = Convert.ToString(chkcollege.Items[Convert.ToInt32(collinfo)].Text);
                            if (collname == "College Name")
                            {
                                collinfo1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["collname"].ToString() + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "University")
                            {
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["university"].ToString() + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Affliated By")
                            {
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Address")
                            {
                                string address1 = ds.Tables[0].Rows[0]["Address1"].ToString();
                                string address2 = ds.Tables[0].Rows[0]["Address2"].ToString();
                                string address3 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                {
                                    Collvalue = address1;
                                }
                                if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + address2;
                                    }
                                    else
                                    {
                                        Collvalue = address2;
                                    }
                                }
                                if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + address3;
                                    }
                                    else
                                    {
                                        Collvalue = address3;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "City")
                            {
                                string address1 = ds.Tables[0].Rows[0]["Address3"].ToString();
                                if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                                {
                                    Collvalue = address1;
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "District & State & Pincode")
                            {
                                string district = ds.Tables[0].Rows[0]["district"].ToString();
                                string state = ds.Tables[0].Rows[0]["State"].ToString();
                                string pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                                if (district.Trim() != "" && district != null && district.Length > 1)
                                {
                                    Collvalue = district;
                                }
                                if (state.Trim() != "" && state != null && state.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + ',' + state;
                                    }
                                    else
                                    {
                                        Collvalue = state;
                                    }
                                }
                                if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + '-' + pincode;
                                    }
                                    else
                                    {
                                        Collvalue = pincode;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Phone No & Fax")
                            {
                                string phone = ds.Tables[0].Rows[0]["Phoneno"].ToString();
                                string fax = ds.Tables[0].Rows[0]["Faxno"].ToString();
                                if (phone.Trim() != "" && phone != null && phone.Length > 1)
                                {
                                    Collvalue = "Phone :" + phone;
                                }
                                if (fax.Trim() != "" && fax != null && fax.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + " , Fax : " + fax;
                                    }
                                    else
                                    {
                                        Collvalue = "Fax :" + fax;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Email & Web Site")
                            {
                                string email = ds.Tables[0].Rows[0]["Email"].ToString();
                                string website = ds.Tables[0].Rows[0]["Website"].ToString();
                                if (email.Trim() != "" && email != null && email.Length > 1)
                                {
                                    Collvalue = "Email :" + email;
                                }
                                if (website.Trim() != "" && website != null && website.Length > 1)
                                {
                                    if (Collvalue.Trim() != "" && Collvalue != null)
                                    {
                                        Collvalue = Collvalue + " , Web Site : " + website;
                                    }
                                    else
                                    {
                                        Collvalue = "Web Site :" + website;
                                    }
                                }
                                collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                                mypdfpage.Add(collinfo1);
                            }
                            else if (collname == "Left Logo")
                            {
                                try
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                    {
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                        mypdfpage.Add(LogoImage, 25, 25, 400);
                                    }
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg")))
                                    {
                                        string leftlogo = "Left_Logo(" + collegecode1 + ")";
                                        MemoryStream memoryStream = new MemoryStream();
                                        byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + collegecode1 + ").jpeg"));
                                        mypdfpage.Add(LogoImage, 25, 25, 400);
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                                catch { }
                            }
                            else if (collname == "Right Logo")
                            {
                                try
                                {
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                    {
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                        mypdfpage.Add(LogoImage, imgpos, 25, 400);
                                    }
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg")))
                                    {
                                        string rightlogo = "Right_Logo(" + collegecode1 + ")";
                                        MemoryStream memoryStream = new MemoryStream();
                                        byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo(" + collegecode1 + ").jpeg"));
                                        mypdfpage.Add(LogoImage, imgpos, 25, 400);
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    if (pagenamedet != "")
                    {
                        if (inithead > 0)
                        {
                            //if (printdetails == "1")
                            //{
                            //    xpos = 450;
                            //}
                            //else
                            //{
                            //    xpos = 0;
                            //}
                            coltop = coltop + 15;
                            collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop + 15, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + pagenamedet + "");
                            mypdfpage.Add(collinfo1);
                        }
                        else
                        {
                            coltop = getxpos + 15;
                            collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop + 15, headwidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + pagenamedet + "");
                            mypdfpage.Add(collinfo1);
                        }
                    }
                    if (pageno != "0" && pageno != "")
                    {
                        if (inithead > 0)
                        {
                            if (printdetails == "1")
                            {
                                xpos = 1575;
                            }
                            else
                            {
                                xpos = 490;
                            }
                            coltop = coltop + 45;
                            collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, xpos, coltop, 80, 50), System.Drawing.ContentAlignment.TopRight, "Page :" + "" + pagenum + "");
                            mypdfpage.Add(collinfo1);
                        }
                        else
                        {
                            if (printdetails == "1")
                            {
                                xpos = 1575;
                            }
                            else
                            {
                                xpos = 490;
                            }
                            coltop = getxpos + 75;
                            collinfo1 = new PdfTextArea(Fontboldnew, System.Drawing.Color.Black, new PdfArea(mydoc, xpos, coltop, 80, 50), System.Drawing.ContentAlignment.TopRight, "Page :" + "" + pagenum + "");
                            mypdfpage.Add(collinfo1);
                        }
                    }
                    if (inithead > 0)
                    {
                        coltop = coltop + 60;
                    }
                    else
                    {
                        coltop = getxpos + 90;
                    }
                    rowcount = spreadrowcount;
                    a = 0;
                    rowcount = spreadrowcount + 1;
                    a = a + 1;
                    rowcount = rowcount + 1;
                    st = 1;
                    tableforspread = mydoc.NewTable(Fontsmall, rowcount, fpsalary.Sheets[0].ColumnCount, padding);
                    tableforspread.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    tableforspread.VisibleHeaders = false;
                    string spreadval = "";
                    Hashtable htPgTotal = new Hashtable();
                    for (int i = startrow; i < rowcount; i++)
                    {
                        if (st < fpsalary.Sheets[0].RowCount)
                        {
                            tabheight += 50;
                            for (int j = 0; j < fpsalary.Sheets[0].ColumnCount; j++)
                            {
                                if (i == 0)
                                {
                                    spreadval = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[i, j].Text).Trim();
                                    if (spreadval.Trim() == "Allowance" || spreadval.Trim() == "Deduction" || spreadval.Trim() == "")
                                    {
                                        spreadval = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[1, j].Text).Trim();
                                    }
                                    if (spreadval == "S.No")
                                    {
                                        tableforspread.Columns[0].SetWidth(60);
                                    }
                                    if (spreadval.Trim() != "")
                                    {
                                        if (ColumnWidth.ContainsKey(spreadval))
                                        {
                                            //tableforspread.Columns[j].SetWidth(Convert.ToInt32(fpsalary.Sheets[0].Columns[j].Width));
                                            //widthcount += Convert.ToInt32(fpsalary.Sheets[0].Columns[j].Width);
                                            //if (lastval.Trim().Length >= 8)
                                            //{
                                            //    tableforspread.Columns[j].SetWidth(100);
                                            //    widthcount += 100;
                                            //}
                                            //else
                                            //{
                                            tableforspread.Columns[j].SetWidth(Convert.ToInt32(Convert.ToString(ColumnWidth[spreadval])));
                                            widthcount += Convert.ToInt32(Convert.ToString(ColumnWidth[spreadval]));
                                            //}
                                        }
                                    }
                                    tableforspread.Cell(i, j).SetContent(spreadval);
                                    tableforspread.Cell(i, j).SetContentAlignment(ContentAlignment.TopCenter);
                                    tableforspread.Cell(i, j).SetFont(Fontboldnew);
                                    tableforspread.Cell(i, j).SetCellPadding(5);
                                }
                                else if (i == 1)
                                {
                                    spreadval = Convert.ToString(fpsalary.Sheets[0].Cells[st, j].Text).Trim();
                                    string spreadval1 = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                    if (spreadval1.Trim() != "SIGNATURE")
                                    {
                                        if (spreadval.Trim() != "" && spreadval.Trim() != "0")
                                        {
                                            tableforspread.Cell((i + a) - 1, j).SetContent(spreadval);
                                        }
                                        else
                                        {
                                            tableforspread.Cell((i + a) - 1, j).SetContent("-");
                                        }
                                    }
                                    if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE" || spreadval1 == "DATE OF APPOINTED" || spreadval1 == "DATE OF BIRTH" || spreadval1 == "DATE OF JOINING" || spreadval1 == "Date of Resigned" || spreadval1 == "DATE OF INCREMENT")
                                    {
                                        tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                    }
                                    else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                    {
                                        tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopLeft);
                                    }
                                    else
                                    {
                                        if (j != 0)
                                        {
                                            if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE" || spreadval1 == "DATE OF APPOINTED" || spreadval1 == "DATE OF BIRTH" || spreadval1 == "DATE OF JOINING" || spreadval1 == "Date of Resigned" || spreadval1 == "DATE OF INCREMENT")
                                            {
                                                tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                            }
                                            else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                            {
                                                tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopLeft);
                                            }
                                            else
                                            {
                                                tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                            }
                                        }
                                    }
                                    tableforspread.Rows[(i + a) - 1].SetCellPadding(padding);
                                    tableforspread.Cell(1, j).SetCellPadding(padding);
                                }
                                else
                                {
                                    if (fpsalary.Sheets[0].Rows[st].Visible == true)
                                    {
                                        string colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[1, j].Text).Trim();
                                        if (colname.Trim() == "")
                                        {
                                            colname = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                        }
                                        spreadval = Convert.ToString(fpsalary.Sheets[0].Cells[st, j].Text).Trim();
                                        string spreadval1 = Convert.ToString(fpsalary.Sheets[0].ColumnHeader.Cells[0, j].Text).Trim();
                                        if (spreadval1.Trim() != "SIGNATURE")
                                        {
                                            if (spreadval.Trim() != "" && spreadval.Trim() != "0")
                                            {
                                                tableforspread.Cell((i + a) - 1, j).SetContent(spreadval);
                                            }
                                            else
                                            {
                                                tableforspread.Cell((i + a) - 1, j).SetContent("-");
                                            }
                                        }
                                        if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE")
                                        {
                                            tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                        }
                                        else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                        {
                                            tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopLeft);
                                        }
                                        else
                                        {
                                            if (j != 0)
                                            {
                                                if (spreadval1 == "S.No" || spreadval1 == "STAFF CODE")
                                                {
                                                    tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                }
                                                else if (spreadval1 == "NAME" || spreadval1 == "DESIGN" || spreadval1 == "DEPT")
                                                {
                                                    tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopLeft);
                                                }
                                                else
                                                {
                                                    tableforspread.Cell((i + a) - 1, j).SetContentAlignment(ContentAlignment.TopCenter);
                                                }
                                            }
                                        }
                                        tableforspread.Rows[(i + a) - 1].SetCellPadding(padding);
                                        tableforspread.Cell(i, j).SetCellPadding(padding);
                                    }
                                    else
                                    {
                                        i--;
                                        goto outer;
                                    }
                                }
                            }
                        }
                    outer:
                        if (startrow != 0)
                        {
                            st++;
                        }
                        startrow++;
                    }
                    startrow = 0;
                    if (widthcount > headalign)
                    {
                        pdfspreadtab = tableforspread.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, headalign, pdfheight));
                        mypdfpage.Add(pdfspreadtab);
                    }
                    else
                    {
                        Double leftarrange = Math.Round(Convert.ToDouble((headalign - widthcount) / 2), 0);
                        pdfspreadtab = tableforspread.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, leftarrange, coltop, widthcount, pdfheight));
                        mypdfpage.Add(pdfspreadtab);
                    }
                    Double tblheight = 0;
                    if (printdetails == "0")
                    {
                        tblheight = pdfspreadtab.Area.Height + coltop + 50;
                    }
                    else
                    {
                        tblheight = pdfspreadtab.Area.Height + coltop;
                    }
                    if (cblcolumnorder.Items[53].Selected == true && fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL")
                    {
                        string[] spl = new string[2];
                        string strnetsal = Convert.ToString(fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].Text);
                        if (widthcount > headalign)
                        {
                            collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, tblheight, headalign, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString(strnetsal));
                            mypdfpage.Add(collinfo1);
                        }
                        else
                        {
                            Double leftarrange = Math.Round(Convert.ToDouble((headalign - widthcount) / 2), 0);
                            collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, leftarrange, tblheight, widthcount, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString(strnetsal));
                            mypdfpage.Add(collinfo1);
                        }
                        if (pfsal > 0)
                        {
                            collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, tblheight + 50, headalign, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString("PF Salary  :  " + ConvertNumbertoWords(pfsal)));
                            mypdfpage.Add(collinfo1);
                        }
                    }
                    else
                    {
                        if (pfsal > 0)
                        {
                            collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydoc, 0, tblheight, headalign, 50), System.Drawing.ContentAlignment.BottomCenter, Convert.ToString("PF Salary  :  " + ConvertNumbertoWords(pfsal)));
                            mypdfpage.Add(collinfo1);
                        }
                    }
                    coltop = Convert.ToInt32(tblheight) + 100;
                    string footerdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Salary Footer Settings' and college_code='" + collegecode1 + "' and user_Code='" + usercode + "'");
                    if (footerdetails.Trim() != "" && footerdetails.Trim() != "0")
                    {
                        string[] spiltfooterdetails = footerdetails.Split(',');
                        tableforfooter = mydoc.NewTable(Fontbold16, 3, spiltfooterdetails.Length, 3);
                        tableforfooter.SetBorders(Color.Black, 1, BorderType.None);
                        for (int i = 0; i <= spiltfooterdetails.GetUpperBound(0); i++)
                        {
                            string collfooter = spiltfooterdetails[i].ToString();
                            tableforfooter.Cell(0, i).SetContent(collfooter);
                            tableforfooter.Cell(2, 0).SetContentAlignment(ContentAlignment.BottomRight);
                        }
                        Gios.Pdf.PdfTablePage pdftabpage = tableforfooter.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 0, coltop, headalign, 400));
                        mypdfpage.Add(pdftabpage);
                    }
                    mypdfpage.SaveToDocument();
                    if (showheader == "0")
                    {
                        inithead++;
                    }
                }
            }
            #endregion
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Individual_Salary_Report" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                Response.Buffer = true;
                Response.Clear();
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "Individual_SalaryReport.aspx");
        }
    }
    protected void chkselall_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            if (chkselall.Checked == true)
            {
                for (int parent = 0; parent < chkcollege.Items.Count; parent++)
                {
                    chkcollege.Items[parent].Selected = true;
                }
            }
            else
            {
                for (int parent = 0; parent < chkcollege.Items.Count; parent++)
                {
                    chkcollege.Items[parent].Selected = false;
                }
            }
        }
        catch { }
    }
    protected void lb_header_Click(object sender, EventArgs e)
    {
        try
        {
            popheader.Visible = true;
            string Linkname = "Salary Header Settings";
            string colvalue = "";
            string selq = "select LinkValue from New_InsSettings where LinkName='" + Linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string[] colval = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]).Split(',');
                    if (colval.Length > 0)
                    {
                        for (int i = 0; i <= colval.Length; i++)
                        {
                            colvalue = colval[i].ToString();
                            chkcollege.Items[Convert.ToInt32(colvalue)].Selected = true;
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void ImageButton2_Click(object sender, EventArgs e)
    {
        popheader.Visible = false;
    }
    protected void btnsavehead_Click(object sender, EventArgs e)
    {
        try
        {
            string columnvalue = "";
            int inscount = 0;
            for (int i = 0; i < chkcollege.Items.Count; i++)
            {
                if (chkcollege.Items[i].Selected == true)
                {
                    if (columnvalue.Trim() == "")
                    {
                        columnvalue = Convert.ToString(chkcollege.Items[i].Value);
                    }
                    else
                    {
                        columnvalue = columnvalue + "," + Convert.ToString(chkcollege.Items[i].Value) + "";
                    }
                }
            }
            string Linkname = "Salary Header Settings";
            string insquery = " if exists(select * from New_InsSettings where LinkName='" + Linkname + "' and college_code='" + collegecode1 + "' and user_code='" + usercode + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + Linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + Linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
            inscount = d2.update_method_wo_parameter(insquery, "Text");
            if (inscount > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Saved Successfully";
            }
        }
        catch { }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btnexithead_Click(object sender, EventArgs e)
    {
        popheader.Visible = false;
    }
    protected void lb_footer_Click(object sender, EventArgs e)
    {
        try
        {
            popfooter.Visible = true;
            string Linkname = "Salary Footer Settings";
            string colvalue = "";
            TextBox txtroll = new TextBox();
            txtroll.ID = "txtfoot";
            string txtrollcode = txtroll.ID;
            string textcont = "";
            string selq = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + Linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'");
            if (selq.Trim() != "" && selq.Trim() != "0")
            {
                string[] colval = selq.Split(',');
                if (colval.Length > 0)
                {
                    for (int ik = 0; ik <= colval.Length; ik++)
                    {
                        textcont = txtrollcode + ik.ToString();
                        colvalue = colval[ik].ToString();
                        TextBox txtnew = (TextBox)FindControl(textcont);
                        txtnew.Text = colvalue;
                    }
                }
            }
        }
        catch { }
    }
    protected void btnsavefoot_Click(object sender, EventArgs e)
    {
        try
        {
            string columnvalue = "";
            string foot1 = "";
            string foot2 = "";
            string foot3 = "";
            string foot4 = "";
            string foot5 = "";
            if (txtfoot0.Text.Trim() != "")
            {
                //foot1 = txtfoot0.Text.Trim();
                foot1 = Convert.ToString(txtfoot0.Text.First().ToString().ToUpper() + txtfoot0.Text.Substring(1));
                foot1 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(foot1);//delsi
                if (columnvalue == "")
                {
                    columnvalue = foot1;
                }
                else
                {
                    columnvalue = columnvalue + "," + foot1;
                }
            }
            if (txtfoot1.Text.Trim() != "")
            {

                //foot2 = txtfoot1.Text.Trim();
                foot2 = Convert.ToString(txtfoot1.Text.First().ToString().ToUpper() + txtfoot1.Text.Substring(1));
                foot2 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(foot2);
                if (columnvalue == "")
                {
                    columnvalue = foot2;
                }
                else
                {
                    columnvalue = columnvalue + "," + foot2;
                }
            }
            if (txtfoot2.Text.Trim() != "")
            {

                //foot3 = txtfoot2.Text.Trim();

                foot3 = Convert.ToString(txtfoot2.Text.First().ToString().ToUpper() + txtfoot2.Text.Substring(1));
                foot3 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(foot3);
                if (columnvalue == "")
                {
                    columnvalue = foot3;
                }
                else
                {
                    columnvalue = columnvalue + "," + foot3;
                }
            }
            if (txtfoot3.Text.Trim() != "")
            {

                // foot4 = txtfoot3.Text.Trim();
                foot4 = Convert.ToString(txtfoot3.Text.First().ToString().ToUpper() + txtfoot3.Text.Substring(1));
                foot4 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(foot4);
                if (columnvalue == "")
                {
                    columnvalue = foot4;
                }
                else
                {
                    columnvalue = columnvalue + "," + foot4;
                }
            }
            if (txtfoot4.Text.Trim() != "")
            {

                //foot5 = txtfoot4.Text.Trim();
                foot5 = Convert.ToString(txtfoot4.Text.First().ToString().ToUpper() + txtfoot4.Text.Substring(1));
                foot5 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(foot5);
                if (columnvalue == "")
                {
                    columnvalue = foot5;
                }
                else
                {
                    columnvalue = columnvalue + "," + foot5;
                }
            }
            string Linkname = "Salary Footer Settings";
            string insquery = " if exists(select * from New_InsSettings where LinkName='" + Linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + Linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + Linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
            int inscount = d2.update_method_wo_parameter(insquery, "Text");
            if (inscount > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Saved Successfully";
            }
        }
        catch { }
    }
    protected void btnexitfoot_Click(object sender, EventArgs e)
    {
        popfooter.Visible = false;
    }
    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        popfooter.Visible = false;
    }
    protected void lb_print_Click(object sender, EventArgs e)
    {
        try
        {
            popprint.Visible = true;
            string checkedno = "";
            string Linkpagesize = "Salary Pagesize Settings";
            string linkpagename = "Salary Pagename Settings";
            string linkpagecount = "Salary Pagecount Settings";
            string linkchkpageno = "Salary Include Page No";
            string linkchkheader = "Salary Show Header All";
            string linkchkfooter = "Salary Show Footer All";
            string linkchkgrandtot = "Salary Begin Grand Total";
            string linkbasicamnt = "Salary Basic Pay";
            string linkbaslop = "Salary Include Basic With LOP";
            string linkpadd = "Salary Set Cell Padding";
            string selq = "select LinkValue from New_InsSettings where LinkName='" + Linkpagesize + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            selq = selq + " select LinkValue from New_InsSettings where LinkName='" + linkpagename + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            selq = selq + " select LinkValue from New_InsSettings where LinkName='" + linkpagecount + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            selq = selq + " select LinkValue from New_InsSettings where LinkName='" + linkchkpageno + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            selq = selq + " select LinkValue from New_InsSettings where LinkName='" + linkchkheader + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            selq = selq + " select LinkValue from New_InsSettings where LinkName='" + linkchkfooter + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            selq = selq + " select LinkValue from New_InsSettings where LinkName='" + linkchkgrandtot + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            selq = selq + " select LinkValue from New_InsSettings where LinkName='" + linkbasicamnt + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            selq = selq + " select LinkValue from New_InsSettings where LinkName='" + linkbaslop + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            selq = selq + " select LinkValue from New_InsSettings where LinkName='" + linkpadd + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_pagesize.SelectedIndex = Convert.ToInt16(Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]));
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    txt_pagename.Text = Convert.ToString(ds.Tables[1].Rows[0]["LinkValue"]);
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    txt_pagecount.Text = Convert.ToString(ds.Tables[2].Rows[0]["LinkValue"]);
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    checkedno = "";
                    checkedno = Convert.ToString(ds.Tables[3].Rows[0]["LinkValue"]);
                    if (checkedno.Trim() == "1")
                    {
                        chk_pageno.Checked = true;
                    }
                    else
                    {
                        chk_pageno.Checked = false;
                    }
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    checkedno = "";
                    checkedno = Convert.ToString(ds.Tables[4].Rows[0]["LinkValue"]);
                    if (checkedno.Trim() == "1")
                    {
                        chk_showheader.Checked = true;
                    }
                    else
                    {
                        chk_showheader.Checked = false;
                    }
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    checkedno = "";
                    checkedno = Convert.ToString(ds.Tables[5].Rows[0]["LinkValue"]);
                    if (checkedno.Trim() == "1")
                    {
                        chk_showfooter.Checked = true;
                    }
                    else
                    {
                        chk_showfooter.Checked = false;
                    }
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    checkedno = "";
                    checkedno = Convert.ToString(ds.Tables[6].Rows[0]["LinkValue"]);
                    if (checkedno.Trim() == "1")
                    {
                        chk_grandtot.Checked = true;
                    }
                    else
                    {
                        chk_grandtot.Checked = false;
                    }
                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    txtbasic.Text = Convert.ToString(ds.Tables[7].Rows[0]["LinkValue"]);
                }
                if (ds.Tables[8].Rows.Count > 0)
                {
                    checkedno = "";
                    checkedno = Convert.ToString(ds.Tables[8].Rows[0]["LinkValue"]);
                    if (checkedno.Trim() == "1")
                    {
                        cbincbaslop.Checked = true;
                    }
                    else
                    {
                        cbincbaslop.Checked = false;
                    }
                }
                if (ds.Tables[9].Rows.Count > 0)
                {
                    txtsetpadd.Text = Convert.ToString(ds.Tables[9].Rows[0]["LinkValue"]);
                }
            }
        }
        catch { }
    }
    protected void ImageButton3_Click(object sender, EventArgs e)
    {
        popprint.Visible = false;
    }
    protected void btnprintsave_Click(object sender, EventArgs e)
    {
        try
        {
            string chkpageno = "";
            string chkheader = "";
            string chkfooter = "";
            string chkgrandtot = "";
            string chkbaslop = "";
            string pagesize = Convert.ToString(ddl_pagesize.SelectedItem.Value);
            string pagename = Convert.ToString(txt_pagename.Text);
            string pagecount = Convert.ToString(txt_pagecount.Text);
            string basicamnt = Convert.ToString(txtbasic.Text);
            string padding = Convert.ToString(txtsetpadd.Text);
            if (chk_pageno.Checked == true)
            {
                chkpageno = "1";
            }
            else
            {
                chkpageno = "0";
            }
            if (chk_showheader.Checked == true)
            {
                chkheader = "1";
            }
            else
            {
                chkheader = "0";
            }
            if (chk_showfooter.Checked == true)
            {
                chkfooter = "1";
            }
            else
            {
                chkfooter = "0";
            }
            if (chk_grandtot.Checked == true)
            {
                chkgrandtot = "1";
            }
            else
            {
                chkgrandtot = "0";
            }
            if (cbincbaslop.Checked == true)
            {
                chkbaslop = "1";
            }
            else
            {
                chkbaslop = "0";
            }
            string Linkpagesize = "Salary Pagesize Settings";
            string linkpagename = "Salary Pagename Settings";
            string linkpagecount = "Salary Pagecount Settings";
            string linkchkpageno = "Salary Include Page No";
            string linkchkheader = "Salary Show Header All";
            string linkchkfooter = "Salary Show Footer All";
            string linkchkgrandtot = "Salary Begin Grand Total";
            string linkbasicamnt = "Salary Basic Pay";
            string linkbaslop = "Salary Include Basic With LOP";
            string linkpadd = "Salary Set Cell Padding";
            string insquery = "";
            insquery = "if exists(select * from New_InsSettings where LinkName='" + Linkpagesize + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + pagesize + "' where LinkName='" + Linkpagesize + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + Linkpagesize + "','" + pagesize + "','" + usercode + "','" + collegecode1 + "')";
            insquery = insquery + " if exists(select * from New_InsSettings where LinkName='" + linkpagename + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + pagename + "' where LinkName='" + linkpagename + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkpagename + "','" + pagename + "','" + usercode + "','" + collegecode1 + "')";
            insquery = insquery + " if exists(select * from New_InsSettings where LinkName='" + linkpagecount + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + pagecount + "' where LinkName='" + linkpagecount + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkpagecount + "','" + pagecount + "','" + usercode + "','" + collegecode1 + "')";
            insquery = insquery + " if exists(select * from New_InsSettings where LinkName='" + linkbasicamnt + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + basicamnt + "' where LinkName='" + linkbasicamnt + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkbasicamnt + "','" + basicamnt + "','" + usercode + "','" + collegecode1 + "')";
            insquery = insquery + " if exists(select * from New_InsSettings where LinkName='" + linkchkpageno + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + chkpageno + "' where LinkName='" + linkchkpageno + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkchkpageno + "','" + chkpageno + "','" + usercode + "','" + collegecode1 + "')";
            insquery = insquery + " if exists(select * from New_InsSettings where LinkName='" + linkchkheader + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + chkheader + "' where LinkName='" + linkchkheader + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkchkheader + "','" + chkheader + "','" + usercode + "','" + collegecode1 + "')";
            insquery = insquery + " if exists(select * from New_InsSettings where LinkName='" + linkchkfooter + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + chkfooter + "' where LinkName='" + linkchkfooter + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkchkfooter + "','" + chkfooter + "','" + usercode + "','" + collegecode1 + "')";
            insquery = insquery + " if exists(select * from New_InsSettings where LinkName='" + linkchkgrandtot + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + chkgrandtot + "' where LinkName='" + linkchkgrandtot + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkchkgrandtot + "','" + chkgrandtot + "','" + usercode + "','" + collegecode1 + "')";
            insquery = insquery + " if exists(select * from New_InsSettings where LinkName='" + linkbaslop + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + chkbaslop + "' where LinkName='" + linkbaslop + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkbaslop + "','" + chkbaslop + "','" + usercode + "','" + collegecode1 + "')";
            insquery = insquery + " if exists(select * from New_InsSettings where LinkName='" + linkpadd + "' and user_Code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + padding + "' where LinkName='" + linkpadd + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkpadd + "','" + padding + "','" + usercode + "','" + collegecode1 + "')";
            int inscount = d2.update_method_wo_parameter(insquery, "Text");
            if (inscount > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Saved Successfully";
            }
        }
        catch { }
    }
    protected void btnprintexit_Click(object sender, EventArgs e)
    {
        popprint.Visible = false;
    }
    protected void loadallowance()
    {
        try
        {
            ds.Clear();
            cbl_allow.Items.Clear();
            string item = "select allowances from incentives_master where college_code='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_allow.DataSource = ds;
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    string stafftype = split1[0];
                    if (stafftype.Trim() != "")
                    {
                        cbl_allow.Items.Add(stafftype);
                    }
                }
                if (cbl_allow.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_allow.Items.Count; i++)
                    {
                        cbl_allow.Items[i].Selected = true;
                    }
                    txt_allow.Text = "Allowance (" + cbl_allow.Items.Count + ")";
                    cb_allow.Checked = true;
                }
            }
            else
            {
                txt_allow.Text = "--Select--";
                cb_allow.Checked = false;
            }
        }
        catch { }
    }
    protected void loaddeduction()
    {
        try
        {
            ds.Clear();
            cbl_ded.Items.Clear();
            string item = "select deductions from incentives_master where college_code='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string st = Convert.ToString(ds.Tables[0].Rows[0]["deductions"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    string stafftype = split1[0];
                    if (stafftype.Trim() != "")
                    {
                        cbl_ded.Items.Add(stafftype);
                    }
                }
                if (cbl_ded.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_ded.Items.Count; i++)
                    {
                        cbl_ded.Items[i].Selected = true;
                    }
                    txt_ded.Text = "Deduction (" + cbl_ded.Items.Count + ")";
                    cb_ded.Checked = true;
                }
            }
            else
            {
                txt_ded.Text = "--Select--";
                cb_ded.Checked = false;
            }
        }
        catch { }
    }
    protected void loadleave()
    {
        try
        {
            ds.Clear();
            chklstlev.Items.Clear();
            Hashtable hslevadd = new Hashtable();
            hslevadd.Clear();
            string item = "select category,shortname  from leave_category  where college_code = '" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklstlev.DataSource = ds;
                chklstlev.DataTextField = "category";
                chklstlev.DataValueField = "shortname";
                chklstlev.DataBind();
                if (chklstlev.Items.Count > 0)
                {
                    for (int i = 0; i < chklstlev.Items.Count; i++)
                    {
                        if (!hslevadd.ContainsKey(chklstlev.Items[i].Value))
                        {
                            hslevadd.Add(Convert.ToString(chklstlev.Items[i].Value), Convert.ToString(chklstlev.Items[i].Text));
                        }
                    }
                    if (!hslevadd.ContainsKey("LA"))
                    {
                        chklstlev.Items.Add(new ListItem("Late", "LA"));
                    }
                    if (!hslevadd.ContainsKey("PER"))
                    {
                        chklstlev.Items.Add(new ListItem("Permission", "PER"));
                    }
                    if (!hslevadd.ContainsKey("P"))
                    {
                        chklstlev.Items.Add(new ListItem("Present", "P"));
                    }
                    if (!hslevadd.ContainsKey("A"))
                    {
                        chklstlev.Items.Add(new ListItem("Absent", "A"));
                    }
                    for (int i = 0; i < chklstlev.Items.Count; i++)
                    {
                        chklstlev.Items[i].Selected = true;
                    }
                    txtleavecat.Text = "Leave Category(" + chklstlev.Items.Count + ")";
                    chklev.Checked = true;
                }
            }
            else
            {
                txtleavecat.Text = "--Select--";
                chklev.Checked = false;
            }
        }
        catch { }
    }
    protected void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            ds.Clear();
            string group_user = "";
            string cmd = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode1 + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode1 + "') order by dept_name";
            }
            ds = d2.select_method_wo_parameter(cmd, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds.Tables[0];
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "dept_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
                else
                {
                    txt_dept.Text = "--Select--";
                    cb_dept.Checked = false;
                }
            }
        }
        catch { }
    }
    protected void binddesignation()
    {
        ds.Clear();
        cbl_desig.Items.Clear();
        string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + collegecode1 + "' order by desig_name";
        ds = d2.select_method_wo_parameter(statequery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_desig.DataSource = ds;
            cbl_desig.DataTextField = "desig_name";
            cbl_desig.DataValueField = "desig_code";
            cbl_desig.DataBind();
            cbl_desig.Visible = true;
            if (cbl_desig.Items.Count > 0)
            {
                for (int i = 0; i < cbl_desig.Items.Count; i++)
                {
                    cbl_desig.Items[i].Selected = true;
                }
                txt_desig.Text = "Designation(" + cbl_desig.Items.Count + ")";
                cb_desig.Checked = true;
            }
        }
        else
        {
            txt_desig.Text = "--Select--";
            cb_desig.Checked = false;
        }
    }
    protected void loadstafftype()
    {
        try
        {
            ds.Clear();
            cbl_stafftyp.Items.Clear();
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stafftyp.DataSource = ds;
                cbl_stafftyp.DataTextField = "stftype";
                cbl_stafftyp.DataBind();
                if (cbl_stafftyp.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stafftyp.Items.Count; i++)
                    {
                        cbl_stafftyp.Items[i].Selected = true;
                    }
                    txt_stafftyp.Text = "StaffType (" + cbl_stafftyp.Items.Count + ")";
                    cb_stafftyp.Checked = true;
                }
            }
            else
            {
                txt_stafftyp.Text = "--Select--";
                cb_stafftyp.Checked = false;
            }
        }
        catch { }
    }
    protected void loadcategory()
    {
        ds.Clear();
        cbl_staffcat.Items.Clear();
        string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + collegecode1 + "' ";
        ds = d2.select_method_wo_parameter(statequery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_staffcat.DataSource = ds;
            cbl_staffcat.DataTextField = "category_Name";
            cbl_staffcat.DataValueField = "category_code";
            cbl_staffcat.DataBind();
            cbl_staffcat.Visible = true;
            if (cbl_staffcat.Items.Count > 0)
            {
                for (int i = 0; i < cbl_staffcat.Items.Count; i++)
                {
                    cbl_staffcat.Items[i].Selected = true;
                }
                txt_staffcat.Text = "Category(" + cbl_staffcat.Items.Count + ")";
                cb_staffcat.Checked = true;
            }
        }
        else
        {
            txt_staffcat.Text = "--Select--";
            cb_staffcat.Checked = false;
        }
    }
    protected void bindpaymode()
    {
        cbl_paymode.Items.Clear();
        cbl_paymode.Items.Add(new ListItem("Cash", "0"));
        cbl_paymode.Items.Add(new ListItem("Cheque", "1"));
        cbl_paymode.Items.Add(new ListItem("Credit", "2"));
        cbl_paymode.DataBind();
        for (int ro = 0; ro < cbl_paymode.Items.Count; ro++)
        {
            cbl_paymode.Items[ro].Selected = true;
        }
        cb_paymode.Checked = true;
        txtpaymode.Text = "Pay Mode(" + cbl_paymode.Items.Count + ")";
    }
    public void bindyear()
    {
        try
        {
            ddl_year.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("select distinct year(To_Date) as year from HrPayMonths where College_Code ='" + collegecode1 + "' order by year asc", "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_year.DataSource = ds;
                    ddl_year.DataTextField = "year";
                    ddl_year.DataValueField = "year";
                    ddl_year.DataBind();
                }
            }
        }
        catch { }
    }
    public void NewFunt()
    {
        int width = 0;
        int Length = 0;
        ColumnAdjWid = (Hashtable)Session["myDict"];
        ColumnWidth.Add("STAFF CODE", "75");
        ColumnWidth.Add("NAME", "275");
        ColumnWidth.Add("DESIGN", "125");
        ColumnWidth.Add("DEPT", "125");
        ColumnWidth.Add("DEPT ACR", "75");
        ColumnWidth.Add("DESIGN ACR", "100");
        ColumnWidth.Add("DATE OF BIRTH", "75");
        ColumnWidth.Add("DATE OF APPOINTED", "75");
        ColumnWidth.Add("DATE OF JOINING", "75");
        ColumnWidth.Add("Date of Resigned", "75");
        ColumnWidth.Add("DATE OF INCREMENT", "75");
        ColumnWidth.Add("", "0");
        ColumnWidth.Add("CATEGORY", "100");
        ColumnWidth.Add("STAFF TYPE", "100");
        ColumnWidth.Add("BANK ACCOUNT NO", "175");
        ColumnWidth.Add("Check no", "100");
        ColumnWidth.Add("PF NO", "185");
        ColumnWidth.Add("ESI NO", "125");
        ColumnWidth.Add("PAN NO", "125");
        ColumnWidth.Add("LIC NO", "125");
        ColumnWidth.Add("AADHAR NO", "125");
        ColumnWidth.Add("LOAN NO", "125");
        ColumnWidth.Add("GPF NO", "125");
        ColumnWidth.Add("UAN NO", "125");
        ColumnWidth.Add("LOP DAYS", "75");
        ColumnWidth.Add("LOP DATES", "75");
        ColumnWidth.Add("WORKING DAYS", "75");
        ColumnWidth.Add("ABSENT DAYS", "75");
        ColumnWidth.Add("PRESENT DAYS", "75");
        ColumnWidth.Add("NO. OF INCREMENT", "75");
        ColumnWidth.Add("LOP AMOUNT", "75");
        ColumnWidth.Add("PF Salary", "75");
        ColumnWidth.Add("ESI Salary", "75");
        ColumnWidth.Add("ADV RS.", "75");
        ColumnWidth.Add("COLLECTED AMT", "75");
        ColumnWidth.Add("DA %", "75");
        ColumnWidth.Add("Basic", "75");
        ColumnWidth.Add("AGP", "75");
        ColumnWidth.Add("INCREMENT IN RS.", "75");
        ColumnWidth.Add("ACTUAL BASIC", "75");
        ColumnWidth.Add("BASIC PAY Rs.", "75");
        ColumnWidth.Add("PAY BAND", "75");
        ColumnWidth.Add("ACTUAL GRADE", "75");
        ColumnWidth.Add("GRADE PAY", "75");
        ColumnWidth.Add("TOT DED Rs.", "90");
        ColumnWidth.Add("ACTUAL GROSS SALARY", "75");
        ColumnWidth.Add("GROSS PAY Rs.", "75");
        ColumnWidth.Add("NET PAY", "75");
        ColumnWidth.Add("PAY SCALE", "100");
        ColumnWidth.Add("Department With Pay Scale", "75");
        ColumnWidth.Add("TITLE", "50");
        ColumnWidth.Add("BANK FORMAT", "175");
        ColumnWidth.Add("SIGNATURE", "150");
        if (cb_all.Checked == true)
        {
            int count = 0;
            for (int i = 0; i < cbl_allow.Items.Count; i++)
            {
                if (cbl_allow.Items[i].Selected == true)
                {
                    count++;
                    //if (Convert.ToString(cbl_allow.Items[i].Text).Trim().Length <= 5)
                    //{
                    if (!ColumnWidth.ContainsKey(Convert.ToString(cbl_allow.Items[i].Text) + " " + "Rs."))
                    {
                        string[] splheader = Convert.ToString(cbl_allow.Items[i].Text + " " + "Rs.").Split(' ');
                        if (splheader.Length > 0)
                        {
                            bool EntryLen = false;
                            for (int my = 0; my < splheader.Length; my++)
                            {
                                Length = Convert.ToString(splheader[my]).Trim().Length;
                                if (Length >= 8 && EntryLen == false)
                                {
                                    EntryLen = true;
                                    ColumnWidth.Remove(Convert.ToString(cbl_allow.Items[i].Text) + " " + "Rs.");
                                    ColumnWidth.Add(Convert.ToString(cbl_allow.Items[i].Text) + " " + "Rs.", "90");
                                }
                            }
                            if (EntryLen == false)
                            {
                                ColumnWidth.Add(Convert.ToString(cbl_allow.Items[i].Text) + " " + "Rs.", "75");
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    if (!ColumnWidth.ContainsKey(Convert.ToString(cbl_allow.Items[i].Text) + " " + "Rs."))
                    //    {
                    //        ColumnWidth.Add(Convert.ToString(cbl_allow.Items[i].Text) + " " + "Rs.", "75");
                    //    }
                    //}
                }
            }
            ColumnWidth.Add("Allowance", Convert.ToString(count * 75));
        }
        if (cb_deduct.Checked == true)
        {
            int count = 0;
            for (int i = 0; i < cbl_ded.Items.Count; i++)
            {
                if (cbl_ded.Items[i].Selected == true)
                {
                    count++;
                    //if (Convert.ToString(cbl_ded.Items[i].Text).Trim().Length <= 5)
                    //{
                    if (!ColumnWidth.ContainsKey(Convert.ToString(cbl_ded.Items[i].Text) + " " + "Rs."))
                    {
                        string[] splheader = Convert.ToString(cbl_ded.Items[i].Text + " " + "Rs.").Split(' ');
                        if (splheader.Length > 0)
                        {
                            bool EntryLen = false;
                            for (int my = 0; my < splheader.Length; my++)
                            {
                                Length = Convert.ToString(splheader[my]).Trim().Length;
                                if (Length >= 8 && EntryLen == false)
                                {
                                    EntryLen = true;
                                    ColumnWidth.Remove(Convert.ToString(cbl_ded.Items[i].Text) + " " + "Rs.");
                                    ColumnWidth.Add(Convert.ToString(cbl_ded.Items[i].Text) + " " + "Rs.", "90");
                                }
                            }
                            if (EntryLen == false)
                            {
                                ColumnWidth.Add(Convert.ToString(cbl_ded.Items[i].Text) + " " + "Rs.", "75");
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    if (!ColumnWidth.ContainsKey(Convert.ToString(cbl_ded.Items[i].Text) + " " + "Rs."))
                    //    {
                    //        ColumnWidth.Add(Convert.ToString(cbl_ded.Items[i].Text) + " " + "Rs.", "75");
                    //    }
                    //}
                }
            }
            ColumnWidth.Add("Deduction", Convert.ToString(count * 75));
        }
        foreach (DictionaryEntry dr in ColumnAdjWid)
        {
            if (ColumnWidth.ContainsKey(dr.Key))
            {
                Int32.TryParse(Convert.ToString(ColumnWidth[dr.Key]), out width);
                if (Convert.ToString(dr.Value).Trim().Length >= 8 && width < 100)
                {
                    ColumnWidth.Remove(Convert.ToString(dr.Key));
                    ColumnWidth.Add(Convert.ToString(dr.Key), "100");
                }
            }
        }
    }
    private void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }
    private void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }
    ///parttime staff
    protected void cb_parttimestaff_checkedchange(object sender, EventArgs e)
    {
        if (cb_partimestaff.Checked)
        {
            cblcolumnorder.Items[58].Enabled = true;
            cblcolumnorder.Items[59].Enabled = true;
            cblcolumnorder.Items[60].Enabled = true;
        }
        else
        {
            cblcolumnorder.Items[58].Enabled = false;
            cblcolumnorder.Items[59].Enabled = false;
            cblcolumnorder.Items[60].Enabled = false;
            cblcolumnorder.Items[58].Selected = false;
            cblcolumnorder.Items[59].Selected = false;
            cblcolumnorder.Items[60].Selected = false;
        }
    }

    protected void lnk_btn_print_click(object sender, EventArgs e)
    {
        try
        {
            printpopup.Visible = true;

            string footerdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='printpdfCertificate_footer' and college_code='" + Convert.ToString(collegecode) + "' and user_Code='" + Convert.ToString(Session["usercode"]) + "'");
            txt_print.Text = footerdetails;
            if (footerdetails == "0")
            {
                txt_print.Text = "";
            }
        }
        catch (Exception ex)
        {


        }

    }

    protected void btnsavePrint_Click(object sender, EventArgs e)
    {
        try
        {
            string GetName = Convert.ToString(txt_print.Text);

            string insquer = "if exists(select * from New_InsSettings where LinkName='printpdfCertificate_footer' and user_code='" + Convert.ToString(Session["usercode"]) + "' and college_code='" + Convert.ToString(collegecode) + "') update New_InsSettings set LinkValue='" + GetName + "' where LinkName='printpdfCertificate_footer' and user_code='" + Convert.ToString(Session["usercode"]) + "' and college_code='" + Convert.ToString(collegecode) + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('printpdfCertificate_footer','" + GetName + "','" + Convert.ToString(Session["usercode"]) + "','" + Convert.ToString(collegecode) + "')";

            int inscount = d2.update_method_wo_parameter(insquer, "Text");
            if (inscount > 0)
            {
                //alertpopwindow.Visible = false;
                img_div1.Visible = true;
                lblsavealert.Visible = true;
                lblsavealert.Text = "Saved Successfully!";


            }
        }
        catch (Exception ex)
        {


        }

    }
    protected void btnexitPrint_Click(object sender, EventArgs e)
    {
        printpopup.Visible = false;
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        img_div1.Visible = false;
    }
    protected void cb_relived_checkedchange(object sender, EventArgs e)
    {

    }

    protected void btnFinalSettlement_click(object sender, EventArgs e)//delsi05/05/2018
    {
        loadFullandFinalSettlement();
    }
    public void loadFullandFinalSettlement()
    {
        try
        {
            lblnorec.Visible = false;
            Printcontrol.Visible = false;
            lblsmserror.Visible = false;
            fpsalary.SaveChanges();
            Font Fontbold = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 20, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 13, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            // Gios.Pdf.PdfTable tableforfooter;

            string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + collegecode1 + "'";
            str = str + " ;select * from incentives_master where college_code='" + collegecode1 + "'";
            str = str + " ;select distinct CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + ddl_mon.SelectedItem.Value.ToString() + "'and PayYear='" + ddl_year.SelectedItem.Text.ToString() + "' and college_Code=" + collegecode1 + "";
            DataSet ds = d2.select_method_wo_parameter(str, "Text");

            string allowmaster = "";
            string deductmaster = "";
            if (ds.Tables[1].Rows.Count > 0)
            {
                allowmaster = ds.Tables[1].Rows[0]["allowances"].ToString();
                deductmaster = ds.Tables[1].Rows[0]["deductions"].ToString();
            }
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] spdet = deductmaster.Split(';');
            for (int d = 0; d <= spdet.GetUpperBound(0); d++)
            {
                string[] spdedet = spdet[d].Split('\\');
                if (spdedet.GetUpperBound(0) >= 1)
                {
                    string val = spdedet[0];
                    string val1 = spdedet[1];
                    if (!dict.ContainsKey(val))
                    {
                        dict.Add(val, val1);
                    }
                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                string date3 = Convert.ToString(ds.Tables[2].Rows[0]["from_date"]);
                string datefrom3;
                int monthname1;
                string monyear;
                string monthname2 = "";
                int monthnamenum;
                string yearto = "";
                string monyearto = "";
                string[] split = date3.Split(new Char[] { '/' });
                int totlastaff = 0;
                int left1 = 20;
                int left1a = 135;
                int left2 = 145;
                int left3 = 423;
                int left4 = 570;
                int incrre = 1;
                int rowcount = 0;


                if (cblcolumnorder.Items[53].Selected == true && fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 2, 1].Text == "TOTAL")
                {
                    rowcount = fpsalary.Sheets[0].RowCount - 2;
                }
                if (fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Text == "TOTAL" && cblcolumnorder.Items[53].Selected == false)
                {
                    rowcount = fpsalary.Sheets[0].RowCount - 1;
                }
                for (int res = incrre; res < rowcount; res = res + incrre)
                {
                    for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                    {
                        if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT" && fpsalary.Sheets[0].Rows[res].Visible == true)
                        {
                            string text = fpsalary.Sheets[0].Cells[res, lst].Text.ToString();
                            if (text != "" && text != null)
                            {
                                int isval = Convert.ToInt32(fpsalary.Sheets[0].Cells[res, lst].Value);
                                if (isval == 1)
                                {
                                    totlastaff++;
                                }
                            }
                        }
                    }
                }

                datefrom3 = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                string date4 = Convert.ToString(ds.Tables[2].Rows[0]["to_date"]);
                int year3 = Convert.ToInt16(split[2].ToString());
                string[] split1 = date4.Split(new Char[] { '/' });
                string dateto4 = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                monthname2 = split1[0].ToString();
                monthnamenum = Convert.ToInt32(monthname2.ToString());
                yearto = split1[2].ToString();
                string mnmae = split[1].ToString();
                monthname1 = Convert.ToInt16(mnmae);
                monthname2 = split1[1].ToString();
                monthnamenum = Convert.ToInt16(monthname2);
                string year = split[2].ToString();
                monyear = monthname1.ToString() + "/" + year;
                monyearto = monthnamenum.ToString() + "/" + yearto;
                Boolean getvalflag = false;
                int dayfrm = 0;
                string dayto;
                int daytonum;
                string lopdates = "";
                string[] leavetype = new string[50];
                sql = "select shortname from leave_category where college_code=" + Session["collegecode"] + "";
                DataSet dsleave = d2.select_method_wo_parameter(sql, "Text");
                int lev = 0;
                for (int le = 0; le < dsleave.Tables[0].Rows.Count; le++)
                {
                    lev++;
                    string levatype = dsleave.Tables[0].Rows[le]["shortname"].ToString().Trim().ToLower();
                    leavetype[lev] = levatype;
                }


                int Pay_year = Convert.ToInt32(ddl_year.SelectedItem.Value);
                int pay_month = Convert.ToInt32(ddl_mon.SelectedItem.Value);

                if (pay_month == 1)
                {
                    pay_month = 12;
                    Pay_year = Pay_year - 1;

                }
                else
                {
                    pay_month = pay_month - 1;
                }



                sql = "select m.*,s.staff_name,s.pfnumber,s.ESI_No,s.bankaccount,s.pangirnumber ,IsManualLOP,st.payscale as pay_scalenew , st.allowances as actallowance,h.dept_name as deptname,d.desig_name as designame from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.latestrec = 1 and m.college_code=s.college_code and m.college_code =h.college_code and m.college_code=d.collegeCode and s.college_code='" + collegecode1 + "' and m.PayYear='" + Convert.ToString(Pay_year) + "' and m.PayMonth='" + Convert.ToString(pay_month) + "'";
                sql = sql + " ; select convert(nvarchar(15),sa.date_of_birth,103) as dob,sm.staff_code,convert(nvarchar(15),sm.retr_date ,103) as retir,sm.staff_name,CONVERT(varchar(10),sm.join_date,103) as join_date from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no ";


                string query = "select m.*,s.staff_name,s.pfnumber,s.ESI_No,s.bankaccount,s.pangirnumber ,IsManualLOP,st.payscale as pay_scalenew , st.allowances as actallowance,h.dept_name as deptname,d.desig_name as designame from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.latestrec = 1 and m.college_code=s.college_code and m.college_code =h.college_code and m.college_code=d.collegeCode and s.college_code='" + collegecode1 + "' and m.PayYear='" + ddl_year.SelectedValue.ToString() + "' and m.PayMonth='" + ddl_mon.SelectedValue.ToString() + "'";


                DataSet dspay = d2.select_method_wo_parameter(sql, "text");
                DataSet dsstaff = d2.select_method_wo_parameter(query, "text");
                DataTable dtpay = dspay.Tables[0];
                DataTable dtstaffinfo = dspay.Tables[1];
                DataView dvpay = new DataView();
                DataView dvstaffinfo = new DataView();
                DataView dvapp = new DataView();
                DataView currentmonth = new DataView();
                Hashtable deductHash = new Hashtable();
                Hashtable deducthash1 = new Hashtable();


                DateTime dtDate = new DateTime(2000, pay_month, 1);
                string sMonthName = dtDate.ToString("MMM");

                DateTime dtDates = new DateTime(2000, Convert.ToInt32(ddl_mon.SelectedValue), 1);
                string shortMonthName = dtDates.ToString("MMM");
                string previousyear = Convert.ToString(Pay_year).Substring(Convert.ToString(Pay_year).Length - 2);
                string currentyear = ddl_year.SelectedValue.ToString().Substring(ddl_year.SelectedValue.ToString().Length - 2);


                for (int res = 1; res < rowcount; res++)
                {
                    if (fpsalary.Sheets[0].Rows[res].Visible == true)
                    {
                        for (int lst = 0; lst < fpsalary.Sheets[0].ColumnCount; lst++)
                        {
                            if (fpsalary.Sheets[0].ColumnHeader.Cells[0, lst].Text == "SELECT")
                            {
                                string text = fpsalary.Sheets[0].Cells[res, lst].Text.ToString();
                                if (text != "" && text != null)
                                {

                                    int isval = Convert.ToInt32(fpsalary.Sheets[0].Cells[res, lst].Value);
                                    if (isval == 1)
                                    {

                                        int gettop = 0;
                                        int rec = 0;
                                        int coltop = 0;

                                        Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
                                        PdfTextArea ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mydocument, 0, gettop + 5, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, "FULL & FINAL SETTLEMENT SHEET");
                                        mypdfpage.Add(ptsp);
                                        ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, 0, gettop + 5, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, "________________________________");

                                        mypdfpage.Add(ptsp);
                                        coltop = gettop + 30;

                                        ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mydocument, 0, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, "RETIREMENT/ RESIGNATION / TERMINATION");
                                        mypdfpage.Add(ptsp);

                                        ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mydocument, 0, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleCenter, "_______________________________________");
                                        mypdfpage.Add(ptsp);

                                        ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mydocument, 650, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date :");
                                        mypdfpage.Add(ptsp);

                                        string todaysdate = Convert.ToString(DateTime.Now);

                                        if (todaysdate.Contains(' '))
                                        {
                                            string[] dateToday = todaysdate.Split(' ');
                                            string[] split2 = dateToday[0].Split('/');
                                            string formatedDate = Convert.ToString(split2[1]) + "/" + Convert.ToString(split2[0]) + "/" + Convert.ToString(split2[2]);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mydocument, 700, coltop, 900, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(formatedDate));
                                            mypdfpage.Add(ptsp);

                                        }
                                        string staffcode = "";

                                        string designation = "";
                                        string deptname = "";
                                        for (int chklst = 0; chklst < lstcolorder.Items.Count; chklst++)
                                        {
                                            if (lstcolorder.Items[chklst].Text == "STAFF CODE")
                                            {
                                                staffcode = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DESIGN")
                                            {
                                                designation = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "DEPT")
                                            {
                                                deptname = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                            if (lstcolorder.Items[chklst].Text == "LOP DATES")
                                            {
                                                lopdates = Convert.ToString(fpsalary.Sheets[0].Cells[res, chklst + 1].Tag);
                                            }
                                        }
                                        if (staffcode.Trim() == "")
                                        {
                                            lblnorec.Visible = true;
                                            lblnorec.Text = "Please select Staff Code!";
                                            return;
                                        }
                                        string staffname = "";
                                        string netsalary = "";
                                        string bankno = "";
                                        string pfno = "";
                                        string esino = "";
                                        string panno = "";
                                        string Allowances = "";
                                        string lopamount = "";
                                        Double totdection = 0;
                                        dtpay.DefaultView.RowFilter = " staff_code='" + staffcode + "'";
                                        dvpay = dtpay.DefaultView;
                                        if (dvpay.Count > 0)
                                        {
                                            deductHash.Clear();
                                            deducthash1.Clear();
                                            staffname = dvpay[0]["staff_name"].ToString();
                                            bankno = dvpay[0]["bankaccount"].ToString();
                                            pfno = dvpay[0]["pfnumber"].ToString();
                                            esino = dvpay[0]["ESI_No"].ToString();
                                            panno = dvpay[0]["pangirnumber"].ToString();
                                            Allowances = dvpay[0]["allowances"].ToString();
                                            lopamount = dvpay[0]["Tot_lop"].ToString();
                                            designation = dvpay[0]["designame"].ToString();
                                            deptname = dvpay[0]["deptname"].ToString();
                                            netsalary = Convert.ToString(dvpay[0]["netadd"]);

                                            netsalary = String.Format("{0:0.00}", netsalary);
                                            if (pfno.Trim().ToLower() == "" || pfno == null || pfno.Trim() == "0")
                                            {
                                                pfno = "-";
                                            }
                                            string setyear = dvpay[0]["PayYear"].ToString();
                                            if (setyear.Trim() == "" || setyear == null)
                                            {
                                                setyear = yearto;
                                            }
                                            string noofpresent = "";
                                            string presnt = "";
                                            string workdays = "";
                                            string leavedays = "";
                                            string nooflop = "";
                                            noofpresent = dvpay[0]["leavedetail"].ToString();
                                            string[] presplit = noofpresent.Split(';');
                                            if (presplit.Length >= 7)
                                            {
                                                presnt = presplit[1].ToString();
                                                workdays = presplit[0].ToString();
                                                leavedays = presplit[2].ToString();
                                                nooflop = presplit[6].ToString();
                                            }

                                            dtstaffinfo.DefaultView.RowFilter = " staff_code='" + staffcode + "'";
                                            dvstaffinfo = dtstaffinfo.DefaultView;
                                            dsstaff.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";

                                            currentmonth = dsstaff.Tables[0].DefaultView;

                                            string presnts = "";
                                            string workdayss = "";
                                            string leavedayss = "";
                                            string nooflops = "";

                                            string noofpresents = currentmonth[0]["leavedetail"].ToString();
                                            string[] presplits = noofpresents.Split(';');
                                            if (presplits.Length >= 7)
                                            {
                                                presnts = presplits[1].ToString();
                                                workdayss = presplits[0].ToString();
                                                leavedayss = presplits[2].ToString();
                                                nooflops = presplits[6].ToString();
                                            }

                                            string joindate = string.Empty;
                                            string lastworkingday = string.Empty;
                                            string resignationDate = string.Empty;

                                            if (dvstaffinfo.Count > 0)
                                            {
                                                joindate = Convert.ToString(dvstaffinfo[0]["join_date"]);
                                                resignationDate = Convert.ToString(dvstaffinfo[0]["retir"]);
                                            }
                                            Gios.Pdf.PdfTable table = mydocument.NewTable(Fontbold, 11, 3, 1);
                                            table.SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.VisibleHeaders = false;
                                            table.SetBorders(Color.Black, 1, BorderType.Bounds);

                                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 0).ColSpan = 3;
                                            table.Cell(0, 0).SetContent("STAFF INFORMATION");
                                            table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(1, 0).SetContent("NAME OF THE STAFF");

                                            table.Cell(1, 0).SetFont(Fontsmall);
                                            table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 1).SetContent(":");
                                            table.Cell(1, 1).SetFont(Fontsmall);
                                            table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(1, 2).SetContent(Convert.ToString(staffname));
                                            table.Cell(1, 2).SetFont(Fontsmall);


                                            table.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(2, 0).SetContent("DESIGNATION");
                                            table.Cell(2, 0).SetFont(Fontsmall);


                                            table.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(2, 1).SetContent(":");
                                            table.Cell(2, 1).SetFont(Fontsmall);

                                            table.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(2, 2).SetContent(Convert.ToString(designation));
                                            table.Cell(2, 2).SetFont(Fontsmall);

                                            table.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(3, 0).SetContent("DEPARTMENT");
                                            table.Cell(3, 0).SetFont(Fontsmall);

                                            table.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(3, 1).SetContent(":");
                                            table.Cell(3, 1).SetFont(Fontsmall);

                                            table.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(3, 2).SetContent(Convert.ToString(deptname));
                                            table.Cell(3, 2).SetFont(Fontsmall);

                                            table.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(4, 0).SetContent("DATE OF JOINING");
                                            table.Cell(4, 0).SetFont(Fontsmall);

                                            table.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(4, 1).SetContent(":");
                                            table.Cell(4, 1).SetFont(Fontsmall);

                                            table.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(4, 2).SetContent(Convert.ToString(joindate));
                                            table.Cell(4, 2).SetFont(Fontsmall);


                                            table.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(5, 0).SetContent("DATE OF RESIGNATION");
                                            table.Cell(5, 0).SetFont(Fontsmall);
                                            table.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(5, 1).SetContent(":");
                                            table.Cell(5, 1).SetFont(Fontsmall);

                                            table.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(6, 0).SetContent("DATE OF LAST WORKING DAY");
                                            table.Cell(6, 0).SetFont(Fontsmall);
                                            table.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(6, 1).SetContent(":");
                                            table.Cell(6, 1).SetFont(Fontsmall);

                                            table.Cell(6, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(6, 2).SetContent(Convert.ToString(resignationDate));
                                            table.Cell(6, 2).SetFont(Fontsmall);


                                            table.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(7, 0).SetContent("GROSS SALARY P.M");
                                            table.Cell(7, 0).SetFont(Fontsmall);


                                            table.Cell(7, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(7, 1).SetContent(":");
                                            table.Cell(7, 1).SetFont(Fontsmall);


                                            table.Cell(7, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(7, 2).SetContent("Rs." + Convert.ToString(netsalary));
                                            table.Cell(7, 2).SetFont(Fontsmall);

                                            table.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(8, 0).SetContent("EARNED LEAVE BALANCE");
                                            table.Cell(8, 0).SetFont(Fontsmall);
                                            table.Cell(8, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(8, 1).SetContent(":");
                                            table.Cell(8, 1).SetFont(Fontsmall);

                                            table.Cell(9, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(9, 0).SetContent("GRATUITY ELIGIBILITY");
                                            table.Cell(9, 0).SetFont(Fontsmall);

                                            table.Cell(9, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(9, 1).SetContent(":");
                                            table.Cell(9, 1).SetFont(Fontsmall);

                                            table.Cell(10, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(10, 0).SetContent("TOTAL PAYABLE DAYS FOR THE MONTH" + " " + sMonthName.ToUpper() + "" + previousyear);
                                            table.Cell(10, 0).SetFont(Fontsmall1);

                                            table.Cell(10, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(10, 1).SetContent(":");
                                            table.Cell(10, 1).SetFont(Fontsmall1);

                                            table.Cell(10, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(10, 2).SetContent(Convert.ToString(workdays) + "" + "Days");
                                            table.Cell(10, 2).SetFont(Fontsmall1);

                                            coltop = coltop + 50;
                                            Gios.Pdf.PdfTablePage PdfTable = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, coltop, 800, 700));
                                            mypdfpage.Add(PdfTable);


                                            string deduction3 = "";
                                            deduction3 = dvpay[0]["deductions"].ToString();
                                            string[] deduction_arr1;
                                            string deductionsplit;

                                            deduction_arr1 = deduction3.Split('\\');
                                            int exatval = 0;
                                            int countval = 0;
                                            for (int i = 0; i < deduction_arr1.GetUpperBound(0); i++)
                                            {

                                                exatval = deduction_arr1.GetUpperBound(0);
                                                deductionsplit = deduction_arr1[i];
                                                string[] deductionda = deductionsplit.Split(';');
                                                if (deductionda.GetUpperBound(0) >= 3)
                                                {
                                                    string da = deductionda[0];
                                                    string mode = Convert.ToString(deductionda[1]);
                                                    string daac = "";
                                                    string da3 = "";


                                                    if (deductionda[2].Trim() != "")
                                                    {
                                                        string[] spval = deductionda[2].Split('-');
                                                        if (spval.Length == 2)
                                                        {
                                                            if (mode.Trim().ToUpper() == "PERCENT")
                                                            {
                                                                da3 = Convert.ToString(spval[1]);
                                                            }
                                                            else
                                                            {
                                                                da3 = Convert.ToString(spval[0]);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            da3 = Convert.ToString(deductionda[3]);
                                                        }
                                                    }
                                                    daac = Convert.ToString(deductionda[3]);
                                                    double da2 = 0;
                                                    Double.TryParse(daac, out da2);
                                                    double ds3 = 0;
                                                    double.TryParse(da3, out ds3);
                                                    ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                                    string DblAllowActVal = daac;
                                                    string DblAllowVal = deductionda[3];


                                                    if (ds3 != 0)
                                                    {
                                                        if (!deductHash.Contains(da))
                                                        {
                                                            deductHash.Add(da, ds3);

                                                        }
                                                        countval++;

                                                    }

                                                }
                                            }
                                            Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, countval + 5, 4, 1);
                                            table1.VisibleHeaders = false;
                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            //curY += 5 + (int)addtabletopage1.Area.Height;
                                            coltop = coltop + (int)PdfTable.Area.Height;


                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 0).SetContent("SALARY FOR THE MONTH OF" + " " + sMonthName.ToUpper() + "" + previousyear);
                                            table1.Cell(0, 0).ColSpan = 4;

                                            table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(1, 0).SetContent("EARNINGS");
                                            table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(2, 0).SetContent("CONSOLIDATE PAY");
                                            table1.Cell(2, 0).SetFont(Fontsmall);

                                            table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(1, 1).SetContent("AMOUNT");

                                            table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table1.Cell(2, 1).SetContent("Rs." + Convert.ToString(netsalary));
                                            table1.Cell(2, 1).SetFont(Fontsmall);

                                            table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(1, 2).SetContent("DEDUCTIONS");


                                            table1.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(1, 3).SetContent("Amount");

                                            int dedu = 2;
                                            double totaldeductPre = 0;
                                            foreach (DictionaryEntry item in deductHash)
                                            {

                                                string key = Convert.ToString(item.Key);

                                                string value = Convert.ToString(item.Value);
                                                table1.Cell(dedu, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(dedu, 2).SetContent(Convert.ToString(key));
                                                table1.Cell(dedu, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                                table1.Cell(dedu, 3).SetContent("Rs." + Convert.ToString(value) + ".00");
                                                totaldeductPre = totaldeductPre + Convert.ToDouble(value);
                                                table1.Cell(dedu, 2).SetFont(Fontsmall);
                                                table1.Cell(dedu, 3).SetFont(Fontsmall);
                                                dedu++;

                                            }



                                            table1.Cell(dedu, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(dedu, 0).SetContent("GROSS PAY");

                                            table1.Cell(dedu, 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table1.Cell(dedu, 1).SetContent("Rs." + Convert.ToString(netsalary));

                                            table1.Cell(dedu, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(dedu, 2).SetContent("TOTAL");

                                            table1.Cell(dedu, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table1.Cell(dedu, 3).SetContent("Rs." + Convert.ToString(totaldeductPre) + ".00");
                                            dedu++;

                                            table1.Cell(dedu, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(dedu, 0).SetContent("NET SALARY PAYABLE  FOR THE MONTH" + " " + sMonthName.ToUpper() + "" + previousyear);
                                            table1.Cell(dedu, 0).ColSpan = 3;
                                            double netprevious = Convert.ToDouble(netsalary) - totaldeductPre;

                                            table1.Cell(dedu, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table1.Cell(dedu, 3).SetContent("Rs." + Convert.ToString(netprevious) + ".00");

                                            dedu++;

                                            table1.Cell(dedu, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(dedu, 0).SetContent("TOTAL PAYABLE DAYS FOR THE MONTH" + " " + shortMonthName.ToUpper() + "" + currentyear);
                                            table1.Cell(dedu, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(dedu, 3).SetContent(Convert.ToString(workdayss) + "" + "Days");

                                            table1.Cell(dedu, 0).ColSpan = 3;
                                            dedu++;

                                            Gios.Pdf.PdfTablePage PdfTable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, coltop, 800, 700));
                                            mypdfpage.Add(PdfTable1);


                                            string deduction3s = "";
                                            double currentnet = 0;
                                            double currenttot = 0;
                                            deduction3s = currentmonth[0]["deductions"].ToString();//delsiref
                                            currentnet = Convert.ToDouble(currentmonth[0]["netadd"]);
                                            string[] deduction_arr1s;
                                            string deductionsplits;

                                            deduction_arr1s = deduction3s.Split('\\');
                                            int exatvals = 0;
                                            int countvals = 0;
                                            for (int i = 0; i < deduction_arr1s.GetUpperBound(0); i++)
                                            {

                                                exatvals = deduction_arr1s.GetUpperBound(0);
                                                deductionsplits = deduction_arr1s[i];
                                                string[] deductiondas = deductionsplits.Split(';');
                                                if (deductiondas.GetUpperBound(0) >= 3)
                                                {
                                                    string da = deductiondas[0];
                                                    string mode = Convert.ToString(deductiondas[1]);
                                                    string daac = "";
                                                    string da3 = "";


                                                    if (deductiondas[2].Trim() != "")
                                                    {
                                                        string[] spval = deductiondas[2].Split('-');
                                                        if (spval.Length == 2)
                                                        {
                                                            if (mode.Trim().ToUpper() == "PERCENT")
                                                            {
                                                                da3 = Convert.ToString(spval[1]);
                                                            }
                                                            else
                                                            {
                                                                da3 = Convert.ToString(spval[0]);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            da3 = Convert.ToString(deductiondas[3]);
                                                        }
                                                    }
                                                    daac = Convert.ToString(deductiondas[3]);
                                                    double da2 = 0;
                                                    Double.TryParse(daac, out da2);
                                                    double ds3 = 0;
                                                    double.TryParse(da3, out ds3);
                                                    ds3 = Math.Round(ds3, 2, MidpointRounding.AwayFromZero);
                                                    string DblAllowActVal = daac;
                                                    string DblAllowVal = deductiondas[3];


                                                    if (ds3 != 0)
                                                    {
                                                        if (!deducthash1.Contains(da))
                                                        {
                                                            deducthash1.Add(da, ds3);

                                                        }
                                                        countvals++;

                                                    }

                                                }
                                            }


                                            Gios.Pdf.PdfTable table2 = mydocument.NewTable(Fontbold, countvals + 3, 4, 1);

                                            table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(0, 0).ColSpan = 4;
                                            table2.Cell(0, 0).SetContent("SALARY FOR THE MONTH OF" + " " + shortMonthName.ToUpper() + "" + currentyear);

                                            table2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(1, 0).SetContent("EARNINGS");

                                            table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table2.Cell(2, 0).SetContent("CONSOLIDATE PAY");
                                            table2.Cell(2, 0).SetFont(Fontsmall);
                                            table2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table2.Cell(2, 1).SetContent("Rs." + Convert.ToString(currentnet) + ".00");
                                            table2.Cell(2, 1).SetFont(Fontsmall);

                                            table2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(1, 1).SetContent("AMOUNT");
                                            table2.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(1, 2).SetContent("DEDUCTIONS");
                                            table2.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(1, 3).SetContent("AMOUNT");

                                            coltop = coltop + (int)PdfTable1.Area.Height;
                                            table2.VisibleHeaders = false;
                                            table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);


                                            int dedus = 2;
                                            foreach (DictionaryEntry item in deducthash1)
                                            {

                                                string key = Convert.ToString(item.Key);

                                                string value = Convert.ToString(item.Value);
                                                table2.Cell(dedus, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table2.Cell(dedus, 2).SetContent(Convert.ToString(key));
                                                table2.Cell(dedus, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                                table2.Cell(dedus, 3).SetContent("Rs." + Convert.ToString(value) + ".00");
                                                currenttot = currenttot + Convert.ToDouble(value);
                                                table2.Cell(dedus, 2).SetFont(Fontsmall);
                                                table2.Cell(dedus, 3).SetFont(Fontsmall);
                                                dedus++;

                                            }

                                            Double currentmonthnetpay = Convert.ToDouble(currentnet) - currenttot;
                                            table2.Cell(dedus, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table2.Cell(dedus, 0).SetContent("GROSS PAY");

                                            table2.Cell(dedus, 1).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table2.Cell(dedus, 1).SetContent("Rs." + Convert.ToString(currentnet) + ".00");

                                            table2.Cell(dedus, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table2.Cell(dedus, 2).SetContent("TOTAL");

                                            table2.Cell(dedus, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                            table2.Cell(dedus, 3).SetContent(Convert.ToString("Rs." + currenttot) + ".00");
                                            dedus++;

                                            Gios.Pdf.PdfTablePage PdfTable2 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, coltop, 800, 700));
                                            mypdfpage.Add(PdfTable2);


                                            Gios.Pdf.PdfTable table3 = mydocument.NewTable(Fontbold, 7, 4, 1);
                                            table3.VisibleHeaders = false;
                                            table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                            coltop = coltop + (int)PdfTable2.Area.Height;

                                            table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table3.Cell(0, 0).ColSpan = 3;
                                            table3.Cell(0, 0).SetContent("NET SALARY PAYABLE FOR THE MONTH" + " " + shortMonthName.ToUpper() + "" + currentyear);

                                            table3.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleRight);

                                            table3.Cell(0, 3).SetContent("Rs." + Convert.ToString(currentmonthnetpay) + ".oo");

                                            table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table3.Cell(1, 0).ColSpan = 3;
                                            table3.Cell(1, 0).SetContent("GRATUITY AMOUNT( will be claimed from LIC of India)");


                                            table3.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleRight);

                                            table3.Cell(1, 3).SetContent("Rs.");

                                            table3.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table3.Cell(2, 0).ColSpan = 3;
                                            table3.Cell(2, 0).SetContent("EARNED LEAVE PAYMENT ( as per College norms)");


                                            table3.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleRight);

                                            table3.Cell(2, 3).SetContent("Rs.");

                                            table3.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table3.Cell(3, 0).ColSpan = 3;
                                            table3.Cell(3, 0).SetContent("NOTICE PAY (ONE MONTH)");

                                            table3.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleRight);

                                            table3.Cell(3, 3).SetContent("Rs.");

                                            double totalpayment = Convert.ToDouble(currentmonthnetpay) + Convert.ToDouble(netprevious);
                                            table3.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table3.Cell(4, 0).ColSpan = 3;
                                            table3.Cell(4, 0).SetContent("TOTAL AMOUNT");

                                            table3.Cell(4, 3).SetContentAlignment(ContentAlignment.MiddleRight);

                                            table3.Cell(4, 3).SetContent("Rs." + Convert.ToString(totalpayment) + ".00");

                                            table3.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table3.Cell(5, 0).ColSpan = 3;
                                            table3.Cell(5, 0).SetContent("(-) LOAN FROM COLLEGE");


                                            table3.Cell(5, 3).SetContentAlignment(ContentAlignment.MiddleRight);

                                            table3.Cell(5, 3).SetContent("Rs.");
                                            string word = ConvertNumbertoWords(Convert.ToInt32(totalpayment));
                                            table3.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(6, 0).ColSpan = 4;
                                            table3.Cell(6, 0).SetContent("RUPEES IN WORDS :" + word);



                                            Gios.Pdf.PdfTablePage PdfTable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, coltop, 800, 700));
                                            mypdfpage.Add(PdfTable3);
                                            coltop = coltop + (int)PdfTable3.Area.Height;
                                            coltop = coltop + 10;
                                            PdfArea tete = new PdfArea(mydocument, 20, coltop, 400, 150);
                                            PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                            mypdfpage.Add(pr1);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mydocument, 23, coltop - 25, 400, 150), System.Drawing.ContentAlignment.MiddleLeft, "No dues Clearance done from all dept:");
                                            mypdfpage.Add(ptsp);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                               new PdfArea(mydocument, 23, coltop + 10, 400, 150), System.Drawing.ContentAlignment.MiddleLeft, "Prepared By:");
                                            mypdfpage.Add(ptsp);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 20, coltop - 25, 400, 150), System.Drawing.ContentAlignment.MiddleRight, "Yes/No.");
                                            mypdfpage.Add(ptsp);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                           new PdfArea(mydocument, 20, coltop + 10, 400, 150), System.Drawing.ContentAlignment.MiddleRight, "Checked By:");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mydocument, 20, coltop + 43, 400, 150), System.Drawing.ContentAlignment.MiddleCenter, "_______________");
                                            mypdfpage.Add(ptsp);
                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mydocument, 20, coltop + 60, 400, 150), System.Drawing.ContentAlignment.MiddleCenter, "Signature - MFA");
                                            mypdfpage.Add(ptsp);
                                            PdfArea tete2 = new PdfArea(mydocument, 425, coltop, 400, 150);
                                            PdfRectangle pr3 = new PdfRectangle(mydocument, tete2, Color.Black);
                                            mypdfpage.Add(pr3);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mydocument, 430, coltop + 60, 400, 150), System.Drawing.ContentAlignment.MiddleCenter, "BURSER");
                                            mypdfpage.Add(ptsp);

                                            coltop = coltop + (int)pr1.RectangleArea.Height;

                                            PdfArea tete1 = new PdfArea(mydocument, 20, coltop + 5, 400, 150);
                                            PdfRectangle pr2 = new PdfRectangle(mydocument, tete1, Color.Black);
                                            mypdfpage.Add(pr2);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                               new PdfArea(mydocument, 23, coltop - 55, 400, 150), System.Drawing.ContentAlignment.MiddleCenter, "------- for Accounts Department only-------");
                                            mypdfpage.Add(ptsp);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 23, coltop - 25, 400, 150), System.Drawing.ContentAlignment.MiddleLeft, "Payment vide Cheque No. :  .....................................");
                                            mypdfpage.Add(ptsp);


                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 23, coltop, 400, 150), System.Drawing.ContentAlignment.MiddleLeft, "Date of Payment :  ....................................................");
                                            mypdfpage.Add(ptsp);


                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                 new PdfArea(mydocument, 23, coltop + 20, 400, 150), System.Drawing.ContentAlignment.MiddleLeft, "Name of Bank  :  .......................................................");
                                            mypdfpage.Add(ptsp);


                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 23, coltop + 50, 400, 150), System.Drawing.ContentAlignment.MiddleLeft, "Date:______________");
                                            mypdfpage.Add(ptsp);



                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 20, coltop + 50, 400, 150), System.Drawing.ContentAlignment.MiddleRight, "Accounts Department");
                                            mypdfpage.Add(ptsp);


                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 20, coltop + 70, 400, 150), System.Drawing.ContentAlignment.MiddleRight, "(Authorised Signatory)");
                                            mypdfpage.Add(ptsp);
                                            PdfArea tete3 = new PdfArea(mydocument, 425, coltop + 5, 400, 150);
                                            PdfRectangle pr4 = new PdfRectangle(mydocument, tete3, Color.Black);
                                            mypdfpage.Add(pr4);



                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mydocument, 430, coltop - 25, 400, 150), System.Drawing.ContentAlignment.MiddleCenter, "I hereby agree and confirm having received the above amount as full and final payment from the Madras Christian College before signing this settlement document. I will prefer no other claim form the College.");
                                            mypdfpage.Add(ptsp);

                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                          new PdfArea(mydocument, 425, coltop + 80, 400, 150), System.Drawing.ContentAlignment.TopLeft, "_____________________________________________");
                                            mypdfpage.Add(ptsp);



                                            ptsp = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                          new PdfArea(mydocument, 440, coltop + 135, 400, 150), System.Drawing.ContentAlignment.TopLeft, "(Signature of Staff)");
                                            mypdfpage.Add(ptsp);

                                            mypdfpage.SaveToDocument();
                                        }
                                    }
                                }
                            }
                        }

                        string appPath = HttpContext.Current.Server.MapPath("~");
                        if (appPath != "")
                        {
                            string szPath = appPath + "/Report/";
                            string szFile = DateTime.Now.ToString("ddMMyyyyhhmmsstt") + "FullandFinalSettlement.pdf";
                            mydocument.SaveToFile(szPath + szFile);
                            Response.ClearHeaders();
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                            Response.ContentType = "application/pdf";
                            Response.WriteFile(szPath + szFile);
                        }
                    }
                }
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Staff and then Proceed!";


            }

        }
        catch (Exception ex)
        {


        }
    }
    protected void loadClgBank()//popdesignationapt delsi2305
    {

        ds.Clear();
        ddl_colbank.Items.Clear();
        string collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
        string statequery = "select distinct BankPK,BankName from FM_FinBankMaster where CollegeCode='" + collegecode1 + "'";
        ds = d2.select_method_wo_parameter(statequery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_colbank.DataSource = ds;
            ddl_colbank.DataTextField = "BankName";
            ddl_colbank.DataValueField = "BankPK";
            ddl_colbank.DataBind();
            ddl_colbank.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_colbank.Items.Insert(0, new ListItem("Select", "0"));

        }
    }
    protected void SalaryHold_checkedchange(object sender, EventArgs e)
    {

    }

}
#region LOP DATES on btngo_click
//datefrom3 = split5[0].ToString() + "/" + split5[1].ToString() + "/" + split5[2].ToString();
//year3 = Convert.ToInt16(split5[2]);
//string date4 = todate.ToString();
//string[] split6 = date4.Split(new Char[] { '/' });
//string dateto4 = split6[1].ToString() + "/" + split6[0].ToString() + "/" + split6[2].ToString();
//monthname2 = split6[0].ToString();
//monthnamenum = Convert.ToInt32(monthname2.ToString());
//yearto = split1[2].ToString();
//string mnmae = split5[0].ToString();
//monthname1 = Convert.ToInt16(mnmae);
//monthname2 = split1[1].ToString();
//monthnamenum = Convert.ToInt16(monthname2);
//string year = split5[2].ToString();
//monyear = monthname1.ToString() + "/" + year;
//monyearto = monthnamenum.ToString() + "/" + yearto;
//string dayfrom;
//int today = 0;
//today = Convert.ToInt16(split6[0].ToString());
//dayfrom = split5[1].ToString();
//dayfrm = Convert.ToInt32(dayfrom);
//dayto = split6[0].ToString();
//daytonum = Convert.ToInt32(dayto);
//double lopdaysmor = 0;
//double lopdayseveg = 0;
//string date = "";
//int morpresent = 0;
//int evepresent = 0;
//int maxdays = getmaxdays(monthname1, year3);
//int absentdays = 0;
#endregion
//---------------Last Modified By Jeyaprakash on Nov 2nd,2016--------------//
//---------------PDF Alignment for SREC Changes----------------------------//