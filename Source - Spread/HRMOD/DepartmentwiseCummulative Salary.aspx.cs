using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;
public partial class DepartmentwiseCummulative_Salary : System.Web.UI.Page
{
    Hashtable splallow = new Hashtable();
    Hashtable allow = new Hashtable();
    Hashtable hatt = new Hashtable();
    DAccess2 dac = new DAccess2();
    static string[] splallw_arry = new string[15];
    static string[] spll_alll_tag_arry = new string[15];
    static string[] allow_arry = new string[15];
    string gssmcat = "";
    string gssmdept = "";
    string user_code, college_code;
    DataSet ds = new DataSet();
    DataSet dsbind = new DataSet();
    DataSet dset = new DataSet();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlDataAdapter da = new SqlDataAdapter();
    double IntMTotal;
    double IntMTemp;
    double netpaytotal;
    int getval2;
    int col2 = 0;
    string[] seatcode = new string[44];
    int[] seatindex = new int[44];
    int[] bloodindex = new int[44];
    string sql1;
    string[] bloodvalue = new string[55];
    string[] bloodcode = new string[55];
    double[] DblAllowTotal = new double[50];
    double[] deductiontotal = new double[50];
    double[] splAllowTotal = new double[50];
    int colheder;
    double basicpaytotal = 0;
    double newnetpaytotal = 0;
    string[] seatvalue = new string[55];
    string sql;
    int col;
    string mname;
    string[] allowanmce_arr1;
    string gstrdept = "";
    string gstrcateogry = "";
    string strdept = "";
    string strcategory = "";
    string strallallowance = "";
    string stralldeduct = "";
    string da3;
    double DblNetAllowTotal = 0;
    double DblNetDedTotal = 0;
    int getval;
    int j = 0;
    string[] deductioncode = new string[44];
    int[] deductionindex = new int[44];
    string[] allowancecode = new string[44];
    int[] allowanceindex = new int[44];
    string[] allowancevalue = new string[50];
    string[] dedctionvalue = new string[44];
    string group_user = "";
    DAccess2 d2 = new DAccess2();
    string fin_startdate = "", fin_enddate = "";
    static int vl;
    string acct_id = "";
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
            if (!IsPostBack)
            {
                vl = 0;
                fpsalarydemond.Visible = false;
                RptHead.InnerHtml = "";
                grdPF.Visible = false;
                grdPanel.Visible = false;
                btnExport.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                chkpf.Visible = false;
                chkgrouptotal.Visible = false;
                college_code = Session["collegecode"].ToString();
                load_subject();
                string str1 = "select distinct account_info.acct_id from account_info,acctinfo where account_info.acct_id=acctinfo.acct_id and college_code=" + Session["collegecode"].ToString() + " and finyear_start='" + fin_startdate + "' and finyear_end='" + fin_enddate + "'";
                DataSet dsscol1 = d2.select_method_wo_parameter(str1, "Text");
                if (dsscol1.Tables[0].Rows.Count > 0)
                {
                    acct_id = dsscol1.Tables[0].Rows[0][0].ToString();
                }
                string dtchss = DateTime.Today.ToShortDateString();
                string[] dsplitchss = dtchss.Split(new Char[] { '/' });
                DateTime fromdate, todate;
                todate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                fromdate = Convert.ToDateTime(todate);
                fromdate = fromdate - TimeSpan.FromDays(7);
                string today = System.DateTime.Now.ToString();
                string today1;
                string[] split13 = today.Split(new char[] { ' ' });
                string[] split14 = split13[0].Split(new Char[] { '/' });
                today1 = split14[1].ToString() + "/" + split14[0].ToString() + "/" + split14[2].ToString();
                string today2 = System.DateTime.Now.ToString();
                string today3;
                string[] split15 = today.Split(new char[] { ' ' });
                string[] split16 = split13[0].Split(new Char[] { '/' });
                today3 = split16[1].ToString() + "/" + split16[0].ToString() + "/" + split16[2].ToString();
                load_batchyear();
                load_dept();
                load_category();
                load_allowance();
                load_stafftype();
                loadtype();
                college_code = Session["collegecode"].ToString();
                user_code = Session["usercode"].ToString();
                pnldemond.Visible = true;
                btngo.Visible = true;
                fpsalarydemond.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                fpsalarydemond.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            }
            lblnorec.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        System.Web.UI.Control cntUpdateBtnn = fpsalarydemond.FindControl("Update");
        System.Web.UI.Control cntCancelBtnn = fpsalarydemond.FindControl("Cancel");
        System.Web.UI.Control cntCopyBtnn = fpsalarydemond.FindControl("Copy");
        System.Web.UI.Control cntCutBtnn = fpsalarydemond.FindControl("Clear");
        System.Web.UI.Control cntPasteBtnn = fpsalarydemond.FindControl("Paste");
        System.Web.UI.Control cntPageNextBtnn = fpsalarydemond.FindControl("Next");
        System.Web.UI.Control cntPagePreviousBtnn = fpsalarydemond.FindControl("Prev");
        System.Web.UI.Control cntPagePrintBtnn = fpsalarydemond.FindControl("Print");
        if ((cntUpdateBtnn != null))
        {
            TableCell tc = (TableCell)cntUpdateBtnn.Parent;
            TableRow tr = (TableRow)tc.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCancelBtnn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCopyBtnn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCutBtnn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntPasteBtnn.Parent;
            tr.Cells.Remove(tc);
            tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }
    void load_batchyear()
    {
        cblbatchyear.Visible = true;
        ds.Clear();
        ds = d2.select_method_wo_parameter("select distinct year(fdate) as year from monthlypay  order by year desc ", "Text");
        cblbatchyear.DataSource = ds.Tables[0];
        cblbatchyear.DataTextField = "Year";
        cblbatchyear.DataValueField = "year";
        cblbatchyear.DataBind();
    }
    void load_dept()
    {
        try
        {
            tbseattype.Text = "---Select---";
            chkselect.Checked = false;
            cbldepttype.Visible = true;
            cbldepttype.Items.Clear();
            ds.Clear();
            string deptquery = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') ";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') ";
            }
            if (deptquery != "")
            {
                ds = d2.select_method(deptquery, allow, "Text");
                cbldepttype.DataSource = ds.Tables[0];
                cbldepttype.DataTextField = "dept_name";
                cbldepttype.DataValueField = "dept_code";
                cbldepttype.DataBind();
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < cbldepttype.Items.Count; i++)
                {
                    cbldepttype.Items[i].Selected = true;
                }
                tbseattype.Text = "Dept(" + cbldepttype.Items.Count.ToString() + ")";
                chkselect.Checked = true;
            }
        }
        catch
        {
        }
    }
    void load_category()
    {
        try
        {
            tbblood.Text = "---Select---";
            chkcategory.Checked = false;
            cblcategory.Visible = true;
            cblcategory.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("select  distinct category_code,category_name from staffcategorizer where college_code='" + Session["collegecode"] + "' order by category_code", "Text");
            cblcategory.DataSource = ds.Tables[0];
            cblcategory.DataTextField = "category_name";
            cblcategory.DataValueField = "category_code";
            cblcategory.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < cblcategory.Items.Count; i++)
                {
                    cblcategory.Items[i].Selected = true;
                }
                tbblood.Text = "Category(" + cblcategory.Items.Count.ToString() + ")";
                chkcategory.Checked = true;
            }
        }
        catch
        {
        }
    }
    void load_stafftype()
    {
        txtsstafftype.Text = "---Select---";
        chksstafftype.Checked = false;
        chksstafftypelist.Visible = true;
        chksstafftypelist.Items.Clear();
        ds.Clear();
        ds = d2.select_method_wo_parameter("select distinct stftype from stafftrans st, staffmaster sm where st.staff_code=sm.staff_code and college_code='" + Session["collegecode"] + "' order by stftype desc", "Text");
        chksstafftypelist.DataSource = ds.Tables[0];
        chksstafftypelist.DataTextField = "stftype";
        chksstafftypelist.DataValueField = "stftype";
        chksstafftypelist.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < chksstafftypelist.Items.Count; i++)
            {
                chksstafftypelist.Items[i].Selected = true;
            }
            txtsstafftype.Text = "Staff Type(" + chksstafftypelist.Items.Count.ToString() + ")";
            chksstafftype.Checked = true;
        }
    }
    void load_allowance()
    {
        try
        {
            txtallowance.Text = "---Select---";
            chkallowance.Checked = false;
            cblallowance.Items.Clear();
            txtdeduction.Text = "---Select---";
            Chkdeduction.Checked = false;
            cbldeduction.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("Select * from incentives_master where college_code=" + Session["collegecode"] + "", "Text");
            string allowanmce = "";
            string detection = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                allowanmce = ds.Tables[0].Rows[0]["allowances"].ToString();
                detection = ds.Tables[0].Rows[0]["deductions"].ToString();
            }
            string[] allowanmce_arr;
            allowanmce_arr = allowanmce.Split(';');
            for (int i = 0; i < allowanmce_arr.Length; i++)
            {
                string all2 = allowanmce_arr[i];
                string[] splitallo3 = all2.Split('\\');
                if (splitallo3.Length > 0)
                {
                    all2 = splitallo3[0];
                }
                if (all2.Trim() != "")
                {
                    cblallowance.Items.Add(all2);
                    cblallowance.Items[i].Selected = true;
                }
            }
            if (cblallowance.Items.Count > 0)
            {
                txtallowance.Text = "Allowance(" + cblallowance.Items.Count.ToString() + ")";
                chkallowance.Checked = true;
            }
            string[] detection_arr;
            detection_arr = detection.Split(';');
            for (int j = 0; j < detection_arr.Length; j++)
            {
                string all2 = detection_arr[j];
                string[] splitallo3 = all2.Split('\\');
                if (splitallo3.Length > 0)
                {
                    all2 = splitallo3[0];
                    if (all2.Trim() != "")
                    {
                        cbldeduction.Items.Add(all2);
                        cbldeduction.Items[j].Selected = true;
                    }
                }
                else
                {
                    cbldeduction.Items.Add(detection_arr[j]);
                    cbldeduction.Items[j].Selected = true;
                }
            }
            if (cbldeduction.Items.Count > 0)
            {
                txtdeduction.Text = "Deduction(" + cbldeduction.Items.Count.ToString() + ")";
                Chkdeduction.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void loadtype()
    {
        try
        {
            string strqueru = "select distinct stream from staffmaster where Stream<>'' and Stream<>'Select' and college_code='" + Session["collegecode"] + "'";
            //string strqueru = "select Value from Master_Settings where  settings = 'Hrjournal Report'";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strqueru, "Text");
            ddlstream.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataValueField = "stream";
                ddlstream.DataBind();

            }
        }
        catch
        {
        }
    }
    protected void cbldepttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbseattype.Text = "---Select---";
        chkselect.Checked = false;
        int seatcount = 0;
        for (int i = 0; i < cbldepttype.Items.Count; i++)
        {
            if (cbldepttype.Items[i].Selected == true)
            {
                seatcount = seatcount + 1;
            }
        }
        if (seatcount > 0)
        {
            tbseattype.Text = "Department(" + seatcount.ToString() + ")";
            if (seatcount == cbldepttype.Items.Count)
            {
                chkselect.Checked = true;
            }
        }
    }
    protected void cblcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        int bloodcount = 0;
        tbblood.Text = "---Select---";
        chkcategory.Checked = false;
        for (int i = 0; i < cblcategory.Items.Count; i++)
        {
            if (cblcategory.Items[i].Selected == true)
            {
                bloodcount = bloodcount + 1;
            }
        }
        if (bloodcount > 0)
        {
            tbblood.Text = "Category(" + bloodcount.ToString() + ")";
            if (bloodcount == cblcategory.Items.Count)
            {
                chkcategory.Checked = true;
            }
        }
    }
    public string getmonth(string mname)
    {
        string month = "";
        if (mname == "1")
        {
            month = "January";
            return month;
        }
        else if (mname == "2")
        {
            month = "February";
        }
        else if (mname == "3")
        {
            month = "March";
        }
        else if (mname == "4")
        {
            month = "April";
        }
        else if (mname == "5")
        {
            month = "May";
        }
        else if (mname == "6")
        {
            month = "June";
        }
        else if (mname == "7")
        {
            month = "July";
        }
        else if (mname == "8")
        {
            month = "August";
        }
        else if (mname == "9")
        {
            month = "September";
        }
        else if (mname == "10")
        {
            month = "October";
        }
        else if (mname == "11")
        {
            month = "November";
        }
        else if (mname == "12")
        {
            month = "December";
        }
        return month;
    }
    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        if (chkselect.Checked == true)
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbldepttype.Items[i].Selected = true;
            }
            tbseattype.Text = "Department(" + (cbldepttype.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbldepttype.Items[i].Selected = false;
            }
            tbseattype.Text = "---Select---";
        }
    }
    protected void chkcategory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcategory.Checked == true)
        {
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                cblcategory.Items[i].Selected = true;
            }
            tbblood.Text = "Category(" + (cblcategory.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                cblcategory.Items[i].Selected = false;
            }
            tbblood.Text = "---Select---";
        }
    }
    protected void cblallowance_CheckedChanged(object sender, EventArgs e)
    {
        if (chkallowance.Checked == true)
        {
            for (int i = 0; i < cblallowance.Items.Count; i++)
            {
                cblallowance.Items[i].Selected = true;
            }
            txtallowance.Text = "Allowance(" + (cblallowance.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblallowance.Items.Count; i++)
            {
                cblallowance.Items[i].Selected = false;
            }
            txtallowance.Text = "---Select---";
        }
    }
    protected void Chkdeduction_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkdeduction.Checked == true)
        {
            for (int i = 0; i < cbldeduction.Items.Count; i++)
            {
                cbldeduction.Items[i].Selected = true;
            }
            txtdeduction.Text = "Deduction(" + (cbldeduction.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbldeduction.Items.Count; i++)
            {
                cbldeduction.Items[i].Selected = false;
            }
            txtdeduction.Text = "---Select---";
        }
    }
    protected void cblallowance_SelectedIndexChanged(object sender, EventArgs e)
    {
        int allowancecount = 0;
        txtallowance.Text = "---Select---";
        chkallowance.Checked = false;
        for (int i = 0; i < cblallowance.Items.Count; i++)
        {
            if (cblallowance.Items[i].Selected == true)
            {
                allowancecount = allowancecount + 1;
            }
        }
        if (allowancecount > 0)
        {
            txtallowance.Text = "Allowance(" + allowancecount.ToString() + ")";
            if (allowancecount == cblallowance.Items.Count)
            {
                chkallowance.Checked = true;
            }
        }
    }
    protected void cbldeduction_SelectedIndexChanged(object sender, EventArgs e)
    {
        int deductioncount = 0;
        txtdeduction.Text = "---Select---";
        Chkdeduction.Checked = false;
        for (int i = 0; i < cbldeduction.Items.Count; i++)
        {
            if (cbldeduction.Items[i].Selected == true)
            {
                deductioncount = deductioncount + 1;
            }
        }
        if (deductioncount > 0)
        {
            txtdeduction.Text = "Deduction(" + deductioncount.ToString() + ")";
            if (deductioncount == cbldeduction.Items.Count)
            {
                Chkdeduction.Checked = true;
            }
        }
    }
    public void clear()
    {
        pnldemond.Visible = false;
        fpsalarydemond.Visible = false;
        RptHead.InnerHtml = "";
        grdPF.Visible = false;
        grdPanel.Visible = false;
        btnExport.Visible = false;
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = fpsalarydemond.Sheets[0].ColumnHeader.RowCount;
        string degreedetails = string.Empty;
        degreedetails = "Monthly Salary Statement@Year " + cblbatchyear.SelectedItem.ToString() + "@Month: " + cblmonthfrom.SelectedItem.ToString();
        string pagename = "cumulativesalary.aspx";
        Printcontrol.loadspreaddetails(fpsalarydemond, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtxl.Text;
            if (reportname.ToString().Trim() != "")
            {
                lblnorec.Text = "";
                lblnorec.Visible = false;
                d2.printexcelreport(fpsalarydemond, reportname);
            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
            txtxl.Text = "";
            reportname = "";
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }
    protected void cblsstafftype_CheckedChanged(object sender, EventArgs e)
    {
        if (chksstafftype.Checked == true)
        {
            for (int i = 0; i < chksstafftypelist.Items.Count; i++)
            {
                chksstafftypelist.Items[i].Selected = true;
                txtsstafftype.Text = "Staff Type(" + (chksstafftypelist.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chksstafftypelist.Items.Count; i++)
            {
                chksstafftypelist.Items[i].Selected = false;
                txtsstafftype.Text = "---Select---";
            }
        }
    }
    protected void cblsstafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtsstafftype.Text = "---Select---";
        chksstafftype.Checked = false;
        int allowancecount = 0;
        for (int i = 0; i < chksstafftypelist.Items.Count; i++)
        {
            if (chksstafftypelist.Items[i].Selected == true)
            {
                allowancecount = allowancecount + 1;
            }
        }
        if (allowancecount > 0)
        {
            txtsstafftype.Text = "Staff Type(" + allowancecount.ToString() + ")";
            if (allowancecount == chksstafftypelist.Items.Count)
            {
                chksstafftype.Checked = true;
            }
        }
    }
    protected void Btn_group_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            FpSpread2.SaveChanges();
            FpSpread2.Visible = true;
            bool vll = false;
            for (int j = 0; j < FpSpread1.Sheets[0].RowCount; j++)
            {
                vll = Convert.ToBoolean(FpSpread1.Sheets[0].Cells[j, 1].Value);
                if (vll == true)
                {
                    FpSpread1.Sheets[0].Cells[j, 1].Value = 0;
                }
            }
            vl++;
            string group = TextBox1.Text;
            string group1 = ";" + TextBox1.Text + "-" + ddlstream.Text;
            FpSpread2.Sheets[0].RowCount++;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = group;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = group1;
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
        }
    }
    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
    }
    protected void chkgroup_CheckedChanged(object sender, EventArgs e)
    {
        if (chkgroup.Checked == true)
        {
            LinkButton1.Visible = true;
            chkgrouptotal.Visible = true;
        }
        else if (chkgroup.Checked == false)
        {
            LinkButton1.Visible = false;
            chkgrouptotal.Visible = false;
        }
    }
    protected void Btn_Move_Click(object sender, EventArgs e)
    {
        try
        {
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                TextBox1.Text = "";
                int sno = 0;
                bool isval1 = false;
                FpSpread1.SaveChanges();
                FpSpread2.Visible = true;
                FpSpread2.SaveChanges();
                int index = FpSpread2.Sheets[0].ActiveRow;
                string tvl = "";
                if (index >= 0)
                {
                    for (int j = 0; j < FpSpread1.Sheets[0].RowCount; j++)
                    {
                        if (FpSpread1.Sheets[0].Cells[j, 1].Locked == false)
                        {
                            isval1 = Convert.ToBoolean(FpSpread1.Sheets[0].Cells[j, 1].Value);
                            if (isval1 == true)
                            {
                                string subject_code = FpSpread1.Sheets[0].Cells[j, 2].Text;
                                tvl = FpSpread1.Sheets[0].Cells[j, 2].Tag.ToString();
                                string StreamValue = string.Empty;

                                FpSpread2.Sheets[0].Rows.Add(index, 1);
                                //FpSpread2.Sheets[0].Cells[intF2 + 1, 0].Text = sno.ToString();
                                FpSpread2.Sheets[0].Cells[index, 0].HorizontalAlign = HorizontalAlign.Center;
                                FarPoint.Web.Spread.LabelCellType chkcell0 = new FarPoint.Web.Spread.LabelCellType();
                                FpSpread2.Sheets[0].Columns[2].CellType = chkcell0;
                                FpSpread2.Sheets[0].Cells[index, 2].Text = subject_code;
                                FpSpread2.Sheets[0].Cells[index, 2].Tag = tvl;
                                FpSpread1.Sheets[0].Cells[j, 1].Value = 0;
                            }
                        }
                    }
                }
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.SaveChanges();
            FpSpread1.SaveChanges();
        }
        catch (Exception ex)
        {
        }
    }
    protected void Btn_Moveall_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            FpSpread2.Visible = true;
            FarPoint.Web.Spread.CheckBoxCellType chkl1 = new FarPoint.Web.Spread.CheckBoxCellType();
            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                TextBox1.Text = "";
                FpSpread2.Sheets[0].RowCount = 1;
                // FpSpread2.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount;
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = FpSpread1.Sheets[0].Cells[i, 0].Text.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = FpSpread1.Sheets[0].Cells[i, 2].Text.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = FpSpread1.Sheets[0].Cells[i, 2].Tag.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkl1;
                }
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
        }
    }
    protected void Btn_Removeall_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount;
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_Remove_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.SaveChanges();
            int index = FpSpread2.Sheets[0].ActiveRow;
            FpSpread2.Sheets[0].RemoveRows(index, 1);
            FpSpread2.SaveChanges();
        }
        catch
        {

        }
    }

    protected void FpSpread2_CellClick(object sender, EventArgs e)
    {
    }
    protected void Btn_ok_Click(object sender, EventArgs e)
    {
        try
        {
            string gvl = "";
            string gvl1 = "";
            string blv = "";
            FpSpread2.SaveChanges();
            for (int k = 0; k < FpSpread2.Sheets[0].Rows.Count; k++)
            {
                gvl = FpSpread2.Sheets[0].Cells[k, 2].Tag.ToString();
                blv = FpSpread2.Sheets[0].Cells[k, 2].Text;
                if (gvl1 == "")
                {
                    gvl1 = gvl;
                }
                else
                {
                    gvl1 = gvl1 + "," + gvl;
                }
            }
            string sv = "if exists(select * from Master_Settings where  settings = 'Hrjournal Report' )DELETE FROM Master_Settings WHERE settings = 'Hrjournal Report' insert into Master_Settings(settings,value) values('Hrjournal Report','" + gvl1 + "')";
            int g = d2.update_method_wo_parameter(sv, "text");
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
        catch (Exception ex)
        {
        }
    }
    protected void Btn_cancel_Click(object sender, EventArgs e)
    {
        Panel3.Visible = false;
        poppernew.Visible = false;
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Panel3.Visible = true;
        poppernew.Visible = true;
        fpsalarydemond.Visible = false;
        RptHead.InnerHtml = "";
        grdPF.Visible = false;
        grdPanel.Visible = false;
        btnExport.Visible = false;
        lblexcel.Visible = false;
        txtxl.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
    }
    public void load_subject() // Check Value 
    {
        try
        {
            Boolean depflag = false;
            FarPoint.Web.Spread.LabelCellType chkcell0 = new FarPoint.Web.Spread.LabelCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkl = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].Columns[2].CellType = chkcell0;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Height = 1000;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Width = 300;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
            FpSpread1.Sheets[0].Columns[0].Width = 30;
            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Select";
            FpSpread1.Sheets[0].Columns[0].Width = 30;
            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Dept";
            FpSpread1.Sheets[0].Columns[0].Width = 80;
            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            string cm = "SELECT DISTINCT hp.dept_code,dept_name,dept_acronym from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') ";
            DataSet dss = d2.select_method_wo_parameter(cm, "text");
            if (dss.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    FpSpread1.Sheets[0].RowCount = Convert.ToInt32(FpSpread1.Sheets[0].RowCount) + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkl;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dss.Tables[0].Rows[i]["dept_acronym"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dss.Tables[0].Rows[i]["dept_code"].ToString();
                }
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].AutoPostBack = false;
                FpSpread2.Height = 1000;
                FpSpread2.Width = 300;
                FpSpread2.CommandBar.Visible = false;
                FpSpread2.Sheets[0].ColumnCount = 3;
                FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
                FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.RowCount - 1].Visible = false;
                FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Select";
                FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "Dept";
                FpSpread2.Sheets[0].Columns[0].Width = 50;
                FpSpread2.Sheets[0].Columns[1].Width = 50;
                FpSpread2.Sheets[0].Columns[2].Width = 250;
                FpSpread2.Sheets[0].Columns[1].Visible = false;
                FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[FpSpread2.Sheets[0].ColumnHeader.RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Visible = false;
                FpSpread2.Sheets[0].SheetCorner.ColumnCount = 0;
                string getgroup = d2.GetFunction("select value from Master_Settings where  settings = 'Hrjournal Report'");
                if (getgroup.Trim() != "" && getgroup != null && getgroup.Trim() != "0")
                {
                    string[] strspgroup = getgroup.Split(';');
                    for (int gp = 0; gp < strspgroup.Length; gp++)
                    {
                        string getgporupval = strspgroup[gp].ToString();
                        if (getgporupval.Trim() != "" && getgporupval != null && getgporupval.Trim() != "0")
                        {
                            string[] spgoupdepr = getgporupval.Split(',');
                            if (spgoupdepr.Length >= 1)
                            {
                                if (spgoupdepr[0].Trim() != "" && spgoupdepr != null)
                                {
                                    string[] bindname = spgoupdepr[0].Split('-');
                                    if (bindname.Length >= 1)
                                    {
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = bindname[0].ToString();
                                        if (bindname.Length > 1)
                                        {
                                            if (depflag == false)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = bindname[0].ToString() + '-' + bindname[1].ToString();
                                            }
                                            else
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = ";" + bindname[0].ToString() + '-' + bindname[1].ToString();
                                            }
                                        }
                                        depflag = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        int depsrno = 0;
                                        for (int gpd = 1; gpd < spgoupdepr.Length; gpd++)
                                        {
                                            string getdeptcode = spgoupdepr[gpd].ToString();
                                            if (getdeptcode.Trim() != "" && getdeptcode != null)
                                            {
                                                dss.Tables[0].DefaultView.RowFilter = "dept_code='" + spgoupdepr[gpd].ToString() + "'";
                                                DataView dvdept = dss.Tables[0].DefaultView;
                                                if (dvdept.Count > 0)
                                                {
                                                    depsrno++;
                                                    FpSpread2.Sheets[0].RowCount++;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = depsrno.ToString();
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = chkl;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = spgoupdepr[gpd].ToString();
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dvdept[0]["dept_acronym"].ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (depflag == true)
            {
                FpSpread2.Visible = true;
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
            d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "DepartmentWiseCumulative Salary.aspx");
        }
    }
    protected void btndemond_go_Click(object sender, EventArgs e)   //Modified By Jeyaprakash on July 22nd & 23rd
    {
        try
        {
            Panel3.Visible = false;
            poppernew.Visible = false;
            DataSet dstotal = new DataSet();
            DataView dvtaotal = new DataView();
            int rccheckcount = 0;
            int chrowcount = 0;
            Session["strallow"] = "";
            Session["strdeduct"] = "";
            Session["strcategory"] = "";
            Session["strdept"] = "";
            int depcount = 0;
            if (cbldepttype.Items.Count > 0)
            {
                for (int ik = 0; ik < cbldepttype.Items.Count; ik++)
                {
                    if (cbldepttype.Items[ik].Selected == true)
                    {
                        depcount++;
                    }
                }
            }
            if (depcount == 0)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Any one Department!";
                fpsalarydemond.Visible = false;
                RptHead.InnerHtml = "";
                grdPF.Visible = false;
                grdPanel.Visible = false;
                btnExport.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                return;
            }
            fpsalarydemond.Sheets[0].ColumnCount = 5;
            fpsalarydemond.Sheets[0].RowCount = 0;
            fpsalarydemond.Sheets[0].PageSize = 11;
            fpsalarydemond.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            fpsalarydemond.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            fpsalarydemond.Pager.Align = HorizontalAlign.Right;
            fpsalarydemond.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            fpsalarydemond.Pager.Font.Bold = true;
            fpsalarydemond.Pager.Font.Name = "Arial";
            fpsalarydemond.Pager.ForeColor = Color.DarkGreen;
            fpsalarydemond.Pager.BackColor = Color.AliceBlue;
            fpsalarydemond.Pager.PageCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpsalarydemond.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            fpsalarydemond.Sheets[0].SetColumnWidth(0, 100);
            fpsalarydemond.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
            fpsalarydemond.ActiveSheetView.Columns[1].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[1].Font.Name = "Book Antiqua";
            fpsalarydemond.ActiveSheetView.Columns[0].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[0].Font.Name = "Book Antiqua";
            fpsalarydemond.ActiveSheetView.Columns[2].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[2].Font.Name = "Book Antiqua";
            fpsalarydemond.ActiveSheetView.Columns[3].Font.Size = FontUnit.Medium;
            fpsalarydemond.ActiveSheetView.Columns[3].Font.Name = "Book Antiqua";
            fpsalarydemond.Sheets[0].RowHeader.Visible = false;
            fpsalarydemond.Sheets[0].AutoPostBack = false;
            fpsalarydemond.CommandBar.Visible = false;
            if (chkgroup.Checked == true)
            {
                fpsalarydemond.SheetCorner.RowCount = 3;
            }
            else
            {
                fpsalarydemond.SheetCorner.RowCount = 2;
            }
            Dictionary<string, Double> dicallowdec = new Dictionary<string, double>();
            fpsalarydemond.Visible = true;
            btnprintmaster.Visible = true;
            btnxl.Visible = true;
            lblexcel.Visible = true;
            txtxl.Visible = true;
            lblnorec.Visible = false;
            FarPoint.Web.Spread.LabelCellType chkcell = new FarPoint.Web.Spread.LabelCellType();
            fpsalarydemond.Sheets[0].Columns[0].CellType = chkcell;
            fpsalarydemond.Sheets[0].Columns[0].Visible = true;
            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total No Of Staff";
            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Basic Pay";
            fpsalarydemond.Sheets[0].Columns[3].Locked = true;
            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Name";
            fpsalarydemond.Sheets[0].Columns[1].Locked = true;
            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Basic Pay";
            fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Grade Pay";
            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
            fpsalarydemond.Sheets[0].FrozenColumnCount = 1;
            int checkcount = 0, startcount = 0;
            int colcount1;
            colcount1 = 5;
            sql1 = "select * from incentives_master where college_code=" + Session["collegecode"] + "";
            DataSet dsincent = d2.select_method_wo_parameter(sql1, "Text");
            if (dsincent.Tables[0].Rows.Count > 0)
            {
                for (int inc = 0; inc < dsincent.Tables[0].Rows.Count; inc++)
                {
                    int coldivide = 0;
                    string allowncweshead;
                    string detuctionheader;
                    allowncweshead = dsincent.Tables[0].Rows[inc]["allowances"].ToString();
                    string[] allown2;
                    allown2 = allowncweshead.Split(';');
                    getval = allown2.Length;
                    getval = 0;
                    for (int t = 0; t < cblallowance.Items.Count; t++)
                    {
                        if (cblallowance.Items[t].Selected == true)
                        {
                            getval = getval + 1;
                        }
                    }
                    if (chkgroup.Checked == true)
                    {
                        coldivide = getval / 2;
                        if (getval % 2 == 1)
                        {
                            coldivide = coldivide + 1;
                        }
                    }
                    else
                    {
                        coldivide = getval;
                    }
                    fpsalarydemond.Sheets[0].ColumnCount = fpsalarydemond.Sheets[0].ColumnCount + coldivide;
                    colcount1 = 5;
                    fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colcount1].Text = "Earnings";
                    fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount1, 1, coldivide);
                    int count = 0;
                    int hcount = 4;
                    if (allown2[allown2.Length - 1] == "")
                    {
                        count = allown2.Length - 1;
                    }
                    else
                    {
                        count = allown2.Length;
                    }
                    strallallowance = "";
                    int spcount = 0;
                    int d = 0;
                    int uc = 0;
                    int sa = 0;
                    int newc = 0;
                    if (chkgroup.Checked == true)
                    {
                        for (int i = 0; i < coldivide; i++)
                        {
                            for (int j = 1; j < 3; j++)
                            {
                                if (sa < count)
                                {
                                    if (cblallowance.Items[sa].Selected == true)
                                    {
                                        fpsalarydemond.Sheets[0].Columns[colcount1 + i].Visible = true;
                                        string allo2 = allown2[sa];
                                        string[] splitallo3 = allo2.Split('\\');
                                        allo2 = splitallo3[2];
                                        string allotcol = splitallo3[0];
                                        fpsalarydemond.Sheets[0].ColumnHeader.Cells[j, colcount1 + i].Text = allotcol.ToString();
                                        fpsalarydemond.Sheets[0].ColumnHeader.Cells[j, colcount1 + i].Note = allo2.ToString();
                                        fpsalarydemond.Sheets[0].Columns[colcount1 + i].HorizontalAlign = HorizontalAlign.Right;
                                        if (!dicallowdec.ContainsKey(allo2))
                                        {
                                            dicallowdec.Add(allotcol, 0);
                                        }
                                        if (coldivide == i)
                                        {
                                            if (getval % 2 > 0)
                                            {
                                                fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(j, colcount1 + i, 2, 1);
                                            }
                                        }
                                        sa++;
                                    }
                                    else
                                    {
                                        sa++;
                                        j--;
                                    }
                                }
                            }
                        }
                        if (getval % 2 == 1)
                        {
                            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount1 + coldivide - 1, 2, 1);
                        }
                    }
                    else
                    {
                        fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, hcount].Text = "Earnings";
                        for (int i = 0; i < cblallowance.Items.Count; i++)
                        {
                            startcount++;
                            hcount++;
                            if (cblallowance.Items[i].Selected == true)
                            {
                                checkcount++;
                                spcount++;
                                if (strallallowance == "")
                                {
                                    strallallowance = cblallowance.Items[i].Value.ToString();
                                }
                                else
                                {
                                    strallallowance = strallallowance + "," + cblallowance.Items[i].Value.ToString();
                                }
                                //fpsalarydemond.Sheets[0].Columns[colcount1 + i].Visible = true;
                                //string allo2 = "";
                                //allo2 = allown2[i];
                                //string[] splitallo3 = allo2.Split('\\');
                                //allo2 = splitallo3[0];
                                fpsalarydemond.Sheets[0].Columns[colcount1 + newc].Locked = true;
                                fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, colcount1 + newc].Text = Convert.ToString(cblallowance.Items[i].Text);  // .ToString();   
                                fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, colcount1 + newc].HorizontalAlign = HorizontalAlign.Center;
                                fpsalarydemond.ActiveSheetView.Columns[colcount1 + newc].Font.Size = FontUnit.Medium;
                                fpsalarydemond.ActiveSheetView.Columns[colcount1 + newc].Font.Name = "Book Antiqua";
                                newc++;
                            }
                            else
                            {
                                if (i != 0)
                                {
                                    uc++;
                                }
                                //fpsalarydemond.Sheets[0].Columns[colcount1 + i].Visible = false;
                            }
                        }
                    }
                    Session["strallow"] = strallallowance;
                    fpsalarydemond.Sheets[0].ColumnCount = fpsalarydemond.Sheets[0].ColumnCount + 1;
                    colheder = fpsalarydemond.Sheets[0].ColumnCount;
                    fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colheder - 1].Text = "Gross Amt";
                    fpsalarydemond.ActiveSheetView.Columns[colheder - 1].Font.Size = FontUnit.Medium;
                    fpsalarydemond.ActiveSheetView.Columns[colheder - 1].Font.Name = "Book Antiqua";
                    fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colheder - 1].HorizontalAlign = HorizontalAlign.Center;
                    fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, fpsalarydemond.Sheets[0].ColumnCount - 1, 3, 1);
                    detuctionheader = dsincent.Tables[0].Rows[inc]["deductions"].ToString();
                    string[] deduct2;
                    deduct2 = detuctionheader.Split(';');
                    int dedrowcount = 0;
                    Hashtable hatseldeduct = new Hashtable();
                    getval2 = 0;
                    for (int t = 0; t < cbldeduction.Items.Count; t++)
                    {
                        if (cbldeduction.Items[t].Selected == true)
                        {
                            dedrowcount = dedrowcount + 1;
                            string dedva = cbldeduction.Items[t].Text.ToString();
                            if (!hatseldeduct.Contains(dedva))
                            {
                                hatseldeduct.Add(dedva, dedva);
                            }
                        }
                    }
                    int deducount = 0;
                    if (chkgroup.Checked == true)
                    {
                        deducount = dedrowcount / 2;
                        if (dedrowcount % 2 == 1)
                        {
                            deducount = deducount + 1;
                        }
                    }
                    else
                    {
                        deducount = dedrowcount;
                    }
                    getval2 = deducount;
                    col = fpsalarydemond.Sheets[0].ColumnCount;
                    col2 = col;
                    fpsalarydemond.Sheets[0].ColumnCount = fpsalarydemond.Sheets[0].ColumnCount + getval2;
                    colcount1 = fpsalarydemond.Sheets[0].ColumnCount + 1;
                    fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, col].Text = "Deductions";
                    fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, getval2);
                    SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
                    deduct2 = detuctionheader.Split(';');
                    //getval2 = deduct2.Length;
                    for (int def = 0; def < deduct2.Length; def++)
                    {
                        string[] actdedcut = deduct2[def].Split('\\');
                        if (actdedcut.Length >= 2)
                        {
                            string getdeduct = actdedcut[0].ToString();
                            string getdeduct1 = actdedcut[1].ToString();
                            if (!dict.ContainsKey(getdeduct))
                            {
                                dict.Add(getdeduct, getdeduct1);
                            }
                        }
                    }
                    stralldeduct = "";
                    int startcolumn = 0;
                    int st = 0;
                    int endcolumn = 0;
                    int c = 0;
                    int uncheck = 0;
                    Boolean decflag = false;
                    int setcol = 0;
                    int da = 1;
                    int dec = 0;
                    int colval = 0;
                    if (chkgroup.Checked == true)
                    {
                        foreach (var kvp in dict)
                        {
                            string setval = kvp.Key.ToString();
                            string setvalk = kvp.Value.ToString();
                            if (hatseldeduct.Contains(setval))
                            {
                                for (int v = 0; v < deduct2.Length; v++)
                                {
                                    string[] deduct2spilt = deduct2[v].Split(new char[] { '\\' });
                                    string deduction = deduct2spilt[0].ToString();
                                    string deductmatch = deduct2spilt[1].ToString();
                                    if (setval == deduction)
                                    {
                                        dec++;
                                        if (dec > 2)
                                        {
                                            if (dec % 2 == 1)
                                            {
                                                colval++;
                                                da = 1;
                                            }
                                            else
                                            {
                                                da = 2;
                                            }
                                        }
                                        else
                                        {
                                            da = dec;
                                        }
                                        fpsalarydemond.Sheets[0].ColumnHeader.Cells[da, col + colval].Text = deduction;
                                        fpsalarydemond.Sheets[0].ColumnHeader.Cells[da, col + colval].Note = deductmatch;
                                        fpsalarydemond.Sheets[0].Columns[col + colval].HorizontalAlign = HorizontalAlign.Right;
                                        if (!dicallowdec.ContainsKey(deductmatch))
                                        {
                                            dicallowdec.Add(deduction, 0);
                                        }
                                        if (deducount - 1 == colval)
                                        {
                                            if (dedrowcount % 2 == 1)
                                            {
                                                fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(da, col + colval, 2, 1);
                                            }
                                        }
                                        v = deduct2.Length + 1;
                                    }
                                }
                            }
                        }
                        if (dedrowcount % 2 == 1)
                        {
                            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(da, col + colval, 2, 1);
                        }
                    }
                    else
                    {
                        foreach (var kvp in dict)
                        {
                            string setval = kvp.Key.ToString();
                            string setvalk = kvp.Value.ToString();
                            for (int de = 0; de < cbldeduction.Items.Count; de++)
                            {
                                string dropval = cbldeduction.Items[de].Text;
                                if (setval == dropval)
                                {
                                    if (cbldeduction.Items[de].Selected == true)
                                    {
                                        endcolumn++;
                                        if (decflag == false)
                                        {
                                            startcolumn = col;
                                        }
                                        fpsalarydemond.Sheets[0].Columns[col + setcol].Visible = true;
                                        fpsalarydemond.Sheets[0].Columns[col + setcol].Locked = true;
                                        fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col + setcol].Text = setval;
                                        fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col + setcol].Note = setvalk;
                                        fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col + setcol].HorizontalAlign = HorizontalAlign.Center;
                                        fpsalarydemond.ActiveSheetView.Columns[col + setcol].Font.Size = FontUnit.Medium;
                                        fpsalarydemond.ActiveSheetView.Columns[col + setcol].Font.Name = "Book Antiqua";
                                        decflag = true;
                                        setcol++;
                                    }
                                    else
                                    {
                                        //fpsalarydemond.Sheets[0].Columns[col + setcol].Visible = false;
                                    }
                                    de = cbldeduction.Items.Count;
                                }
                            }
                        }
                        if (endcolumn != 0)
                        {
                            fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, startcolumn, 1, endcolumn + uncheck);
                        }
                    }
                    Session["strdeduct"] = stralldeduct;
                    fpsalarydemond.Sheets[0].ColumnCount = fpsalarydemond.Sheets[0].ColumnCount + 2;
                    colcount1 = fpsalarydemond.Sheets[0].ColumnCount;
                    fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colcount1 - 2].Text = "Total Deduction";
                    fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colcount1 - 2].HorizontalAlign = HorizontalAlign.Center;
                    fpsalarydemond.ActiveSheetView.Columns[colcount1 - 2].Font.Size = FontUnit.Medium;
                    fpsalarydemond.ActiveSheetView.Columns[colcount1 - 2].Font.Name = "Book Antiqua";
                    fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount1 - 2, 3, 1);
                    fpsalarydemond.Sheets[0].ColumnHeader.Cells[0, colcount1 - 1].Text = "Net Pay";
                    fpsalarydemond.Sheets[0].ColumnHeader.Columns[colcount1 - 1].Locked = true;
                    fpsalarydemond.Sheets[0].ColumnHeader.Columns[colcount1 - 2].Locked = true;
                    fpsalarydemond.ActiveSheetView.Columns[colcount1 - 1].Font.Size = FontUnit.Medium;
                    fpsalarydemond.ActiveSheetView.Columns[colcount1 - 1].Font.Name = "Book Antiqua";
                    fpsalarydemond.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount1 - 1, 3, 1);
                }
            }
            int month5 = 1;
            string departmentacr = "";
            if (cblmonthfrom.SelectedItem.Text != "")
            {
                string b = cblmonthfrom.SelectedValue.ToString();
                string monthsdate = string.Empty;
                string monthedate = string.Empty;
                string date_1 = string.Empty;
                string date_2 = string.Empty;
                string sqlquery = "select CONVERT(VARCHAR(10),from_date,103) as from_date,convert(VARCHAR(10),to_date,103) as to_date from hrpaymonths where paymonthnum='" + b + "' and College_Code='" + Session["collegecode"] + "'";
                DataTable dt_hrdate = d2.select_method_wop_table(sqlquery, "text");
                if (dt_hrdate.Rows.Count > 0)
                {
                    monthsdate = dt_hrdate.Rows[0]["from_date"].ToString();
                    monthedate = dt_hrdate.Rows[0]["to_date"].ToString();
                }
                string[] split_date = monthsdate.Split(new char[] { '/' });
                date_1 = split_date[1] + "/" + split_date[0] + "/" + split_date[2];
                string[] split_date_2 = monthedate.Split(new char[] { '/' });
                date_2 = split_date_2[1] + "/" + split_date_2[0] + "/" + split_date_2[2];
                Array.Clear(DblAllowTotal, 0, DblAllowTotal.Length);
                Array.Clear(deductiontotal, 0, deductiontotal.Length);
                sql = " SELECT m.*,d.priority,h.dept_acronym,h.dept_name as deptname,s.staff_name,s.bankaccount,s.pfnumber,m.pay_band,m.grade_pay,st.dept_code as deptcode,isnull(s.Is_PF,'0' ) as Is_PF,s.Stream from monthlypay m,desig_master d,stafftrans st,staffmaster s,hrdept_master h Where s.staff_code=st.staff_code and h.dept_code=st.dept_code and st.staff_code=m.staff_code and st.latestrec=1 and d.desig_code=st.desig_code and m.PayMonth='" + cblmonthfrom.SelectedValue.ToString() + "' and m.PayYear='" + cblbatchyear.SelectedItem.Text.ToString() + "' and ((s.resign=0 or s.settled=0) or(s.resign=1 and s.relieve_date>='" + date_2 + "') or (s.resign=1 and s.relieve_date between '" + date_1 + "' and '" + date_2 + "'))  and m.college_code=" + Session["collegecode"] + " and d.collegecode=" + Session["collegecode"] + " and st.staff_code=m.staff_code  ";    //and m.fdate ='" + date_1 + "' and  m.tdate ='" + date_2 + "' 
                if (tbseattype.Text != "---Select---")
                {
                    int itemcount = 0;
                    strdept = "";
                    gssmcat = "";
                    for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
                    {
                        if (cbldepttype.Items[itemcount].Selected == true)
                        {
                            if (strdept == "")
                            {
                                strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                                gssmdept = cbldepttype.Items[itemcount].Value.ToString();
                            }
                            else
                            {
                                strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                                gssmdept = gssmdept + "," + cbldepttype.Items[itemcount].Value.ToString();
                            }
                        }
                    }
                    gstrdept = gssmdept;
                    if (strdept != "")
                    {
                        strdept = " in(" + strdept + ")";
                    }
                    sql = sql + " and h.dept_code " + strdept + "";
                }
                else
                {
                    gstrdept = "all";
                }
                Session["strdept"] = gstrdept;
                if (tbblood.Text != "---Select---")
                {
                    int itemcount1 = 0;
                    strcategory = "";
                    gssmcat = "";
                    for (itemcount1 = 0; itemcount1 < cblcategory.Items.Count; itemcount1++)
                    {
                        if (cblcategory.Items[itemcount1].Selected == true)
                        {
                            if (strcategory == "")
                            {
                                strcategory = "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                gssmcat = cblcategory.Items[itemcount1].Value.ToString();
                            }
                            else
                            {
                                strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount1].Value.ToString() + "'";
                                gssmcat = gssmcat + "," + cblcategory.Items[itemcount1].Value.ToString();
                            }
                        }
                    }
                    gstrcateogry = gssmcat;
                    if (strcategory != "")
                    {
                        strcategory = " in (" + strcategory + ")";
                    }
                    sql = sql + "  and st.category_code" + strcategory + "";
                    strcategory = "";
                }
                else
                {
                    gstrcateogry = "all";
                }
                Session["strcategory"] = gstrcateogry;
                dstotal = d2.select_method_wo_parameter(sql, "Text");
                int m = 0;
                int countstaff = 0;
                string netadd = "";
                double earntotal = 0;
                string netded = "";
                double totaldeduction = 0;
                string netpa = "";
                string gradepay = "";
                Double totalgradepay = 0;
                double totalnetpay = 0;
                Dictionary<string, Double> dicgrandtotal = new Dictionary<string, Double>();
                Dictionary<string, Double> dicgrouptotal = new Dictionary<string, Double>();
                if (dstotal.Tables[0].Rows.Count > 0)
                {
                    int sno = 0;
                    fpsalarydemond.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    fpsalarydemond.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    // fpsalarydemond.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    fpsalarydemond.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                    fpsalarydemond.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                    fpsalarydemond.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                    if (chkgroup.Checked == true)
                    {
                        string grp = "select * from Master_Settings where  settings = 'Hrjournal Report'";
                        DataSet dss1 = new DataSet();
                        dss1 = d2.select_method_wo_parameter(grp, "text");
                        if (dss1.Tables[0].Rows.Count > 0)
                        {
                            string bl = dss1.Tables[0].Rows[0]["value"].ToString();
                            string[] gvll = bl.Split(';');
                            fpsalarydemond.Visible = true;
                            lblexcel.Visible = true;
                            txtxl.Visible = true;
                            btnxl.Visible = true;
                            btnprintmaster.Visible = true;
                            int gsrno = 0;
                            for (int dept1 = 0; dept1 < gvll.Length; dept1++)
                            {
                                Hashtable ht = new Hashtable();
                                int no = 0;
                                string nma = "";
                                string[] gmb = gvll[dept1].Split(',');
                                Boolean totflag = false;
                                Boolean headflag = false;
                                if (gmb.Length > 1)
                                {
                                    string stream = "";
                                    string newstream = "";
                                    string[] spgr = gmb[0].Split('-');
                                    string hed = spgr[0].ToString();
                                    if (spgr.Length > 1)   //Add conditions by Jeyaprakash on July 25th
                                    {
                                        if (spgr[1].Trim() != "" && spgr[1].Trim() != "Select")
                                        {
                                            stream = spgr[1].ToString();
                                            newstream = stream;
                                        }
                                        else if (spgr[0].Trim() != "" && spgr[0].Trim() != "Select")
                                        {
                                            stream = spgr[0].ToString();
                                            newstream = "";
                                        }
                                        else
                                        {
                                            stream = "";
                                            newstream = "";
                                        }
                                    }
                                    for (int dept = 0; dept < gmb.Length; dept++)
                                    {
                                        string depcode1 = gmb[dept].ToString();
                                        countstaff = 0;
                                        departmentacr = "";
                                        dstotal.Tables[0].DefaultView.RowFilter = "deptcode='" + depcode1 + "' and Stream='" + newstream + "'";   //Added By Jeyaprakash on July 25th
                                        dvtaotal = dstotal.Tables[0].DefaultView;
                                        if (dvtaotal.Count > 0)
                                        {
                                            totflag = true;
                                            if (chkgrouptotal.Checked == false)
                                            {
                                                if (headflag == false)
                                                {
                                                    headflag = true;
                                                    fpsalarydemond.Sheets[0].RowCount++;
                                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = hed;
                                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                    fpsalarydemond.Sheets[0].SpanModel.Add(fpsalarydemond.Sheets[0].RowCount - 1, 1, 1, fpsalarydemond.Sheets[0].ColumnCount);
                                                }
                                            }
                                            rccheckcount++;
                                            for (int dsv = 0; dsv < dvtaotal.Count; dsv++)
                                            {
                                                string allowance = "";
                                                string deduction = "";
                                                string basicpay = "";
                                                countstaff = countstaff + 1;
                                                int k = 0;
                                                int p = 4;
                                                int col3 = 0;
                                                col2 = 0;
                                                col3 = col;
                                                col2 = col;
                                                basicpay = dvtaotal[dsv]["bsalary"].ToString();
                                                netadd = dvtaotal[dsv]["netadd"].ToString();
                                                netded = dvtaotal[dsv]["netded"].ToString();
                                                netpa = dvtaotal[dsv]["netsal"].ToString();
                                                gradepay = dvtaotal[dsv]["g_pay"].ToString();

                                                if (chkdept.Checked == true)
                                                {
                                                    departmentacr = Convert.ToString(dvtaotal[dsv]["deptname"]);
                                                    fpsalarydemond.Sheets[0].Columns[1].Width = 200;
                                                }
                                                else
                                                {
                                                    departmentacr = Convert.ToString(dvtaotal[dsv]["dept_acronym"]);
                                                    fpsalarydemond.Sheets[0].Columns[1].Width = 100;
                                                }
                                                totalnetpay = Convert.ToDouble(netpa) + totalnetpay;
                                                totaldeduction = Convert.ToDouble(netded) + totaldeduction;
                                                earntotal = Convert.ToDouble(netadd) + earntotal;
                                                basicpaytotal = Convert.ToDouble(basicpay) + basicpaytotal;
                                                totalgradepay = Convert.ToDouble(gradepay) + totalgradepay;
                                                newnetpaytotal = Convert.ToDouble(netpa) + newnetpaytotal;


                                                allowance = dvtaotal[dsv]["allowances"].ToString();
                                                deduction = dvtaotal[dsv]["Deductions"].ToString();
                                                string[] allowance2;
                                                int g = 0;
                                                string alowancesplit;
                                                allowanmce_arr1 = allowance.Split('\\');
                                                if (allowanmce_arr1.Length > 0)
                                                {
                                                    for (m = 0; m < allowanmce_arr1.Length; m++)
                                                    {
                                                    l2: alowancesplit = allowanmce_arr1[m];
                                                        k = 0;
                                                        p = 5;
                                                        if (alowancesplit != "")
                                                        {
                                                            allowance2 = alowancesplit.Split(';');
                                                            da3 = allowance2[3];
                                                            //Double da3rou = Math.Round(Convert.ToDouble(da3), 0);
                                                            Double da3rou = Math.Round(Convert.ToDouble(da3), 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                            da3 = da3rou.ToString();
                                                            if (allowance2.Length > 0)
                                                            {
                                                            l3: if (p < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                                {
                                                                    string headval = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, p].Text;
                                                                    string headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, p].Note;
                                                                    for (int j = 0; j < allowance2.Length; j++)
                                                                    {
                                                                        for (int hrow = 1; hrow <= 2; hrow++)
                                                                        {
                                                                            headval = fpsalarydemond.Sheets[0].ColumnHeader.Cells[hrow, p].Text;
                                                                            headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[hrow, p].Note;
                                                                            if (headval == allowance2[j])
                                                                            {
                                                                                DblAllowTotal[k] = Convert.ToDouble(DblAllowTotal[k]) + Convert.ToDouble(da3);
                                                                                DblNetAllowTotal = Convert.ToDouble(da3) + Convert.ToDouble(DblNetAllowTotal);
                                                                                if (dicallowdec.ContainsKey(headval))
                                                                                {
                                                                                    Double val = dicallowdec[headval] + Convert.ToDouble(da3);
                                                                                    dicallowdec[headval] = val;
                                                                                }
                                                                                m = m + 1;
                                                                                p = p + 1;
                                                                                k = k + 1;
                                                                                goto l2;
                                                                            }
                                                                            else if (hrow == 1)
                                                                            {
                                                                            }
                                                                            else
                                                                            {
                                                                                p = p + 1;
                                                                                k = k + 1;
                                                                                goto l3;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                string[] deduction_arr1;
                                                string[] deduction2;
                                                k = 0;
                                                string deductionsplit;
                                                deduction_arr1 = deduction.Split('\\');
                                                if (deduction_arr1.Length > 0)
                                                {
                                                    for (m = 0; m < deduction_arr1.Length; m++)
                                                    {
                                                    l2: deductionsplit = deduction_arr1[m];
                                                        col3 = col;
                                                        k = 0;
                                                        if (deductionsplit != "")
                                                        {
                                                            deduction2 = deductionsplit.Split(';');
                                                            da3 = deduction2[3];
                                                            //Double da3rou = Math.Round(Convert.ToDouble(da3), 0);
                                                            Double da3rou = Math.Round(Convert.ToDouble(da3), 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                            da3 = da3rou.ToString();
                                                            if (deduction2.Length > 0)
                                                            {
                                                            l3: if (col3 < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                                {
                                                                    string headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Text;
                                                                    string headval2 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Note;
                                                                    for (int j = 0; j < deduction2.Length; j++)
                                                                    {
                                                                        for (int hrow = 1; hrow <= 2; hrow++)
                                                                        {
                                                                            headval2 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[hrow, col3].Text;
                                                                            if (headval2 == deduction2[j])// if (headval1 == deduction2[j]) //Modified by srinath 6/11/2014
                                                                            {
                                                                                deductiontotal[k] = deductiontotal[k] + Convert.ToDouble(da3);
                                                                                DblNetDedTotal = Convert.ToDouble(DblNetDedTotal) + Convert.ToDouble(da3);
                                                                                if (dicallowdec.ContainsKey(headval2))
                                                                                {
                                                                                    Double val = dicallowdec[headval2] + Convert.ToDouble(da3);
                                                                                    dicallowdec[headval2] = val;
                                                                                }
                                                                                m = m + 1;
                                                                                col3 = col3 + 1;
                                                                                k = k + 1;
                                                                                goto l2;
                                                                            }
                                                                            else if (hrow == 1)
                                                                            {
                                                                            }
                                                                            else
                                                                            {
                                                                                col3 = col3 + 1;
                                                                                k = k + 1;
                                                                                goto l3;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            sno++;
                                            int rowstr1 = 0;
                                            mname = month5.ToString();
                                            string month7 = getmonth(mname);
                                            nma = departmentacr;
                                            if (chkgrouptotal.Checked == false)
                                            {
                                                rowstr1 = fpsalarydemond.Sheets[0].RowCount++;
                                                fpsalarydemond.Sheets[0].RowCount++;
                                                if (no == 0)
                                                {
                                                    no = rowstr1 + 1;
                                                }
                                                ht.Add(rowstr1, departmentacr);
                                                /////////////////////////////////////////////////////////////////////////////////////////
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 0].Text = sno.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1 + 1, 0].Text = sno.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Tag = month5.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Text = departmentacr;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1 + 1, 1].Text = departmentacr;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Tag = nma;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 3].Text = basicpaytotal.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Text = countstaff.ToString();
                                                //fpsalarydemond.Sheets[0].Cells[rowstr1 + 1, 2].Text = countstaff.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Locked = true;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 4].Font.Size = FontUnit.Medium;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 4].Text = totalgradepay.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                            int setrow = fpsalarydemond.Sheets[0].RowCount - 1;
                                            for (int he = 1; he <= 2; he++)
                                            {
                                                for (int hea = 5; hea < col; hea++)
                                                {
                                                    string hetat = fpsalarydemond.Sheets[0].ColumnHeader.Cells[he, hea].Text;
                                                    if (dicallowdec.ContainsKey(hetat))
                                                    {
                                                        Double vali = dicallowdec[hetat];
                                                        if (dicgrouptotal.ContainsKey(hetat))
                                                        {
                                                            vali = vali + dicgrouptotal[hetat];
                                                            dicgrouptotal[hetat] = vali;
                                                        }
                                                        else
                                                        {
                                                            dicgrouptotal.Add(hetat, vali);
                                                        }
                                                        if (chkgrouptotal.Checked == false)
                                                        {
                                                            if (he == 1)
                                                            {
                                                                fpsalarydemond.Sheets[0].Cells[rowstr1, hea].Text = vali.ToString();
                                                                fpsalarydemond.Sheets[0].Cells[rowstr1, hea].Font.Size = FontUnit.Medium;
                                                            }
                                                            else
                                                            {
                                                                fpsalarydemond.Sheets[0].Cells[rowstr1 + 1, hea].Text = vali.ToString();
                                                                fpsalarydemond.Sheets[0].Cells[rowstr1 + 1, hea].Font.Size = FontUnit.Medium;
                                                            }
                                                        }
                                                        dicallowdec[hetat] = 0;
                                                    }
                                                }
                                            }
                                            if (chkgrouptotal.Checked == false)
                                            {
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                            for (int he = 1; he <= 2; he++)
                                            {
                                                for (int hea = col; hea < fpsalarydemond.Sheets[0].ColumnCount; hea++)
                                                {
                                                    string hetat = fpsalarydemond.Sheets[0].ColumnHeader.Cells[he, hea].Text;
                                                    if (dicallowdec.ContainsKey(hetat))
                                                    {
                                                        Double vali = dicallowdec[hetat];
                                                        if (chkgrouptotal.Checked == false)
                                                        {
                                                            if (he == 1)
                                                            {
                                                                fpsalarydemond.Sheets[0].Cells[rowstr1, hea].Text = vali.ToString();
                                                                fpsalarydemond.Sheets[0].Cells[rowstr1, hea].Font.Size = FontUnit.Medium;
                                                            }
                                                            else
                                                            {
                                                                fpsalarydemond.Sheets[0].Cells[rowstr1 + 1, hea].Text = vali.ToString();
                                                                fpsalarydemond.Sheets[0].Cells[rowstr1 + 1, hea].Font.Size = FontUnit.Medium;
                                                            }
                                                        }
                                                        if (dicgrouptotal.ContainsKey(hetat))
                                                        {
                                                            vali = vali + dicgrouptotal[hetat];
                                                            dicgrouptotal[hetat] = vali;
                                                        }
                                                        else
                                                        {
                                                            dicgrouptotal.Add(hetat, vali);
                                                        }
                                                        dicallowdec[hetat] = 0;
                                                    }
                                                }
                                            }
                                            col2 = 0;
                                            col2 = col;
                                            //DblNetDedTotal = Math.Round(DblNetDedTotal, 2);
                                            DblNetDedTotal = Math.Round(DblNetDedTotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                            if (chkgrouptotal.Checked == false)
                                            {
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                            netpaytotal = (basicpaytotal + DblNetAllowTotal + totalgradepay) - DblNetDedTotal;
                                            //netpaytotal = Math.Round(netpaytotal, 2);
                                            //netpaytotal = Math.Round(netpaytotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                            if (chkgrouptotal.Checked == false)
                                            {
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Text = Convert.ToString((basicpaytotal + DblNetAllowTotal + totalgradepay)); //earntotal.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Locked = true;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = Convert.ToString(DblNetDedTotal); //totaldeduction.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Locked = true;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = Convert.ToString(netpaytotal); //totalnetpay.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                            if (dicgrouptotal.ContainsKey("2"))
                                            {
                                                Double allto = dicgrouptotal["2"];
                                                allto = allto + countstaff;
                                                dicgrouptotal["2"] = allto;
                                            }
                                            else
                                            {
                                                dicgrouptotal.Add("2", countstaff);
                                            }
                                            if (dicgrouptotal.ContainsKey("3"))
                                            {
                                                Double allto = dicgrouptotal["3"];
                                                allto = allto + basicpaytotal;
                                                dicgrouptotal["3"] = allto;
                                            }
                                            else
                                            {
                                                dicgrouptotal.Add("3", basicpaytotal);
                                            }
                                            if (dicgrouptotal.ContainsKey("4"))
                                            {
                                                Double allto = dicgrouptotal["4"];
                                                allto = allto + totalgradepay;
                                                dicgrouptotal["4"] = allto;
                                            }
                                            else
                                            {
                                                dicgrouptotal.Add("4", totalgradepay);
                                            }
                                            if (dicgrouptotal.ContainsKey((colheder - 1).ToString()))
                                            {
                                                Double allto = dicgrouptotal[(colheder - 1).ToString()];
                                                // allto = allto + (basicpaytotal + DblNetAllowTotal + totalgradepay);commented delsi3006
                                                allto = allto + (earntotal);//added delsi 3006
                                                //allto = Math.Round(allto, 0, MidpointRounding.AwayFromZero);
                                                dicgrouptotal[(colheder - 1).ToString()] = allto;
                                            }
                                            else
                                            {
                                                // dicgrouptotal.Add((colheder - 1).ToString(), (basicpaytotal + DblNetAllowTotal + totalgradepay));//commented delsi3006
                                                dicgrouptotal.Add((colheder - 1).ToString(), (earntotal));//added delsi3006
                                            } if (dicgrouptotal.ContainsKey((colcount1 - 2).ToString()))
                                            {
                                                Double allto = dicgrouptotal[(colcount1 - 2).ToString()];
                                                allto = allto + DblNetDedTotal;
                                                dicgrouptotal[(colcount1 - 2).ToString()] = allto;
                                            }
                                            else
                                            {
                                                dicgrouptotal.Add((colcount1 - 2).ToString(), DblNetDedTotal);
                                            } if (dicgrouptotal.ContainsKey((colcount1 - 1).ToString()))
                                            {
                                                Double allto = dicgrouptotal[(colcount1 - 1).ToString()];
                                                // allto = allto + netpaytotal; //commented by delsi 3006
                                                allto = allto + totalnetpay;//added by delsi 3006
                                                dicgrouptotal[(colcount1 - 1).ToString()] = allto;
                                            }
                                            else
                                            {
                                                //dicgrouptotal.Add((colcount1 - 1).ToString(), netpaytotal);//commented by delsi 3006
                                                dicgrouptotal.Add((colcount1 - 1).ToString(), totalnetpay);//added by delsi 3006
                                            }
                                            totalnetpay = 0;
                                            totaldeduction = 0;
                                            earntotal = 0;
                                            basicpaytotal = 0;
                                            DblNetDedTotal = 0;
                                            DblNetAllowTotal = 0;
                                            netpaytotal = 0;
                                            totalgradepay = 0;
                                        }
                                    }
                                    if (totflag == true)
                                    {
                                        fpsalarydemond.Sheets[0].RowCount = fpsalarydemond.Sheets[0].RowCount + 2;
                                        gsrno++;
                                        fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 2].Font.Bold = true;
                                        fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, 0].Locked = true;
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                                        if (chkgrouptotal.Checked == false)
                                        {
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, 0].Text = "Total";
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Text = "Total";
                                            fpsalarydemond.Sheets[0].SpanModel.Add(fpsalarydemond.Sheets[0].RowCount - 2, 0, 1, 2);
                                            fpsalarydemond.Sheets[0].SpanModel.Add(fpsalarydemond.Sheets[0].RowCount - 1, 0, 1, 2);
                                        }
                                        else
                                        {
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, 1].Text = hed;
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = hed;
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, 0].Text = gsrno.ToString();
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Text = gsrno.ToString();
                                        }
                                        for (int intColCtr = 2; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                                        {
                                            for (int headrow = 0; headrow < 3; headrow++)
                                            {
                                                int noo = no - 1;
                                                IntMTotal = 0;
                                                string value = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, intColCtr].Text.ToString();
                                                string colvalue = intColCtr.ToString();
                                                if (value != "")
                                                {
                                                    colvalue = fpsalarydemond.Sheets[0].ColumnHeader.Cells[headrow, intColCtr].Text.ToString();
                                                }
                                                if (dicgrouptotal.ContainsKey(colvalue.ToString()))
                                                {
                                                    IntMTotal = dicgrouptotal[colvalue.ToString()];
                                                    dicgrouptotal[colvalue.ToString()] = 0;
                                                    if (value != "")
                                                    {
                                                        if (headrow == 0 || headrow == 1)
                                                        {
                                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].Font.Size = FontUnit.Medium;
                                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].Font.Bold = true;
                                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                                        }
                                                        else
                                                        {
                                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Size = FontUnit.Medium;
                                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Font.Bold = true;
                                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].Font.Size = FontUnit.Medium;
                                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].Font.Bold = true;
                                                        headrow = 3;
                                                    }
                                                }
                                                if (!dicgrandtotal.ContainsKey(colvalue))
                                                {
                                                    dicgrandtotal.Add(colvalue, IntMTotal);
                                                }
                                                else
                                                {
                                                    Double val = dicgrandtotal[colvalue];
                                                    val = val + IntMTotal;
                                                    dicgrandtotal[colvalue] = val;
                                                }
                                                if (intColCtr != 2)
                                                {
                                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                                }
                                                else
                                                {
                                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (rccheckcount != 0)
                            {
                                fpsalarydemond.Sheets[0].RowCount = fpsalarydemond.Sheets[0].RowCount + 2;
                                fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 2].Font.Bold = true;
                                fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, 0].Locked = true;
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, 0].Text = "Grand Total";
                                fpsalarydemond.Sheets[0].SpanModel.Add(fpsalarydemond.Sheets[0].RowCount - 2, 0, 1, 2);
                                for (int intColCtr = 2; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                                {
                                    for (int headrow = 0; headrow < 3; headrow++)
                                    {
                                        string value = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, intColCtr].Text.ToString();
                                        string colvalue = intColCtr.ToString();
                                        if (value != "")
                                        {
                                            colvalue = fpsalarydemond.Sheets[0].ColumnHeader.Cells[headrow, intColCtr].Text.ToString();
                                        }
                                        if (dicgrandtotal.ContainsKey(colvalue))
                                        {
                                            Double val = dicgrandtotal[colvalue];
                                            if (headrow == 0 || headrow == 1)
                                            {
                                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].Text = val > 0 ? val + "" : "-";
                                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 2, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                            else
                                            {
                                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = val > 0 ? val + "" : "-";
                                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                            if (intColCtr == 2)
                                            {
                                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                            dicgrandtotal.Remove(colvalue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (chkgroup.Checked == false)
                    {
                        if (rbtype.SelectedValue == "0")
                        {
                            for (int dept = 0; dept < cbldepttype.Items.Count; dept++)
                            {
                                if (cbldepttype.Items[dept].Selected == true)
                                {
                                    countstaff = 0;
                                    departmentacr = "";
                                    string depcode = cbldepttype.Items[dept].Value;
                                    dstotal.Tables[0].DefaultView.RowFilter = "deptcode='" + depcode + "' ";
                                    dvtaotal = dstotal.Tables[0].DefaultView;
                                    if (dvtaotal.Count > 0)
                                    {
                                        rccheckcount++;
                                        for (int dsv = 0; dsv < dvtaotal.Count; dsv++)
                                        {
                                            string allowance = "";
                                            string deduction = "";
                                            string basicpay = "";
                                            countstaff = countstaff + 1;
                                            int k = 0;
                                            int p = 4;
                                            int col3 = 0;
                                            col2 = 0;
                                            col3 = col;
                                            col2 = col;
                                            basicpay = dvtaotal[dsv]["bsalary"].ToString();
                                            netadd = dvtaotal[dsv]["netadd"].ToString();
                                            netded = dvtaotal[dsv]["netded"].ToString();
                                            netpa = dvtaotal[dsv]["netsal"].ToString();
                                            gradepay = dvtaotal[dsv]["g_pay"].ToString();
                                            //Modified by srinath 2/4/2014
                                            if (chkdept.Checked == true)
                                            {
                                                departmentacr = Convert.ToString(dvtaotal[dsv]["deptname"]);
                                                fpsalarydemond.Sheets[0].Columns[1].Width = 200;
                                            }
                                            else
                                            {
                                                departmentacr = Convert.ToString(dvtaotal[dsv]["dept_acronym"]);
                                                fpsalarydemond.Sheets[0].Columns[1].Width = 100;
                                            }
                                            totalnetpay = Convert.ToDouble(netpa) + totalnetpay;
                                            totaldeduction = Convert.ToDouble(netded) + totaldeduction;
                                            earntotal = Convert.ToDouble(netadd) + earntotal;
                                            basicpaytotal = Convert.ToDouble(basicpay) + basicpaytotal;
                                            totalgradepay = totalgradepay + Convert.ToDouble(gradepay);
                                            newnetpaytotal = Convert.ToDouble(netpa) + newnetpaytotal;
                                            allowance = dvtaotal[dsv]["allowances"].ToString();
                                            deduction = dvtaotal[dsv]["Deductions"].ToString();
                                            string[] allowance2;
                                            int g = 0;
                                            string alowancesplit;
                                            allowanmce_arr1 = allowance.Split('\\');
                                            if (allowanmce_arr1.Length > 0)
                                            {
                                                for (m = 0; m < allowanmce_arr1.Length; m++)
                                                {
                                                l2: alowancesplit = allowanmce_arr1[m];
                                                    k = 0;
                                                    p = 5;
                                                    if (alowancesplit != "")
                                                    {
                                                        allowance2 = alowancesplit.Split(';');
                                                        da3 = allowance2[3];
                                                        if (allowance2.Length > 0)
                                                        {
                                                        l3: if (p < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                            {
                                                                string headval = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, p].Text;
                                                                for (int j = 0; j < allowance2.Length; j++)
                                                                {
                                                                    if (headval == allowance2[j])
                                                                    {
                                                                        DblAllowTotal[k] = Convert.ToDouble(DblAllowTotal[k]) + Convert.ToDouble(da3);
                                                                        DblNetAllowTotal = Convert.ToDouble(da3) + Convert.ToDouble(DblNetAllowTotal);
                                                                        m = m + 1;
                                                                        p = p + 1;
                                                                        k = k + 1;
                                                                        goto l2;
                                                                    }
                                                                    else
                                                                    {
                                                                        p = p + 1;
                                                                        k = k + 1;
                                                                        goto l3;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                            }
                                                        }
                                                    }
                                                }
                                                string[] deduction_arr1;
                                                string[] deduction2;
                                                k = 0;
                                                string deductionsplit;
                                                deduction_arr1 = deduction.Split('\\');
                                                if (deduction_arr1.Length > 0)
                                                {
                                                    for (m = 0; m < deduction_arr1.Length; m++)
                                                    {
                                                    l2: deductionsplit = deduction_arr1[m];
                                                        col3 = col;
                                                        k = 0;
                                                        if (deductionsplit != "")
                                                        {
                                                            deduction2 = deductionsplit.Split(';');
                                                            da3 = deduction2[3];
                                                            if (deduction2.Length > 0)
                                                            {
                                                            l3: if (col3 < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                                {
                                                                    string headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Text;
                                                                    string headval2 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Note;
                                                                    for (int j = 0; j < deduction2.Length; j++)
                                                                    {
                                                                        if (headval1 == deduction2[j])//if (headval1 == deduction2[j])//Modified by srinath 6/11/2014
                                                                        {
                                                                            deductiontotal[k] = deductiontotal[k] + Convert.ToDouble(da3);
                                                                            DblNetDedTotal = Convert.ToDouble(DblNetDedTotal) + Convert.ToDouble(da3);
                                                                            m = m + 1;
                                                                            col3 = col3 + 1;
                                                                            k = k + 1;
                                                                            goto l2;
                                                                        }
                                                                        else
                                                                        {
                                                                            col3 = col3 + 1;
                                                                            k = k + 1;
                                                                            goto l3;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //      
                                                }
                                            }
                                        }
                                        sno++;
                                        int rowstr1 = fpsalarydemond.Sheets[0].RowCount++;
                                        mname = month5.ToString();
                                        string month7 = getmonth(mname);
                                        /////////////////////////////////////////////////////////////////////////////////////////
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 0].Text = sno.ToString();
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Tag = month5.ToString();
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Text = departmentacr;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 2].HorizontalAlign = HorizontalAlign.Center;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 3].Text = basicpaytotal.ToString();
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 4].Text = totalgradepay.ToString();
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 4].Font.Size = FontUnit.Medium;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Text = countstaff.ToString();
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Locked = true;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 4].HorizontalAlign = HorizontalAlign.Right;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                                        int g1 = 5;
                                        for (int i = 0; i < getval; i++)
                                        {
                                            //DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 2);
                                            DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                            fpsalarydemond.Sheets[0].Cells[rowstr1, g1].Text = DblAllowTotal[i].ToString();
                                            fpsalarydemond.Sheets[0].Cells[rowstr1, g1].HorizontalAlign = HorizontalAlign.Right;
                                            g1 = g1 + 1;
                                            DblAllowTotal[i] = 0;
                                        }
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].HorizontalAlign = HorizontalAlign.Right;
                                        for (int y = 0; y < getval2; y++)
                                        {
                                            //deductiontotal[y] = Math.Round(deductiontotal[y], 2);
                                            deductiontotal[y] = Math.Round(deductiontotal[y], 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                            fpsalarydemond.Sheets[0].Cells[rowstr1, col2].Text = deductiontotal[y].ToString();
                                            fpsalarydemond.Sheets[0].Cells[rowstr1, col2].HorizontalAlign = HorizontalAlign.Right;
                                            col2 = col2 + 1;
                                            deductiontotal[y] = 0;
                                        }
                                        col2 = 0;
                                        col2 = col;
                                        //DblNetDedTotal = Math.Round(DblNetDedTotal, 2);
                                        DblNetDedTotal = Math.Round(DblNetDedTotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].HorizontalAlign = HorizontalAlign.Right;
                                        netpaytotal = (basicpaytotal + DblNetAllowTotal + totalgradepay) - DblNetDedTotal;
                                        //netpaytotal = Math.Round(netpaytotal, 2);
                                        netpaytotal = Math.Round(netpaytotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                        // earntotal = basicpaytotal + DblNetAllowTotal;
                                        string fromdbtotal = "";
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Text = Convert.ToString((basicpaytotal + DblNetAllowTotal + totalgradepay));   //earntotal.ToString();  Added By Jeyaprakash on July 22nd
                                        // totaldeduction = DblNetDedTotal;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Locked = true;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = Convert.ToString(DblNetDedTotal); //totaldeduction.ToString();  Added By Jeyaprakash on July 22nd
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Locked = true;
                                        //fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = netpaytotal.ToString();
                                        //fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = Convert.ToString(netpaytotal); //newnetpaytotal.ToString();  Added By Jeyaprakash on July 22nd
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;
                                        fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].HorizontalAlign = HorizontalAlign.Right;
                                        basicpaytotal = 0;
                                        DblNetDedTotal = 0;
                                        DblNetAllowTotal = 0;
                                        totalgradepay = 0;
                                        netpaytotal = 0;
                                        newnetpaytotal = 0;
                                        totaldeduction = 0;
                                        earntotal = 0;
                                    }
                                }
                            }
                            if (rccheckcount != 0)
                            {
                                fpsalarydemond.Sheets[0].RowCount++;
                                fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = "Total";
                                for (int intColCtr = 2; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                                {
                                    IntMTotal = 0;
                                    for (int IntRowCtr = chrowcount; IntRowCtr < fpsalarydemond.Sheets[0].RowCount - 1; IntRowCtr++)
                                    {
                                        IntMTemp = 0;
                                        if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "-")
                                        {
                                            if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "")
                                            {
                                                IntMTemp = Convert.ToDouble(fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text);
                                            }
                                            else
                                            {
                                                IntMTemp = 0;
                                            }
                                        }
                                        else
                                        {
                                            IntMTemp = 0;
                                        }
                                        IntMTotal = IntMTemp + IntMTotal;
                                        //IntMTotal = Math.Round(IntMTotal, 2);
                                        IntMTotal = Math.Round(IntMTotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                    }
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                                    if (intColCtr != 2)
                                    {
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                    else
                                    {
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }
                        }
                        else if (rbtype.SelectedValue == "1")
                        {
                            sno = 0;
                            for (int cat = 0; cat < cblcategory.Items.Count; cat++)
                            {
                                rccheckcount = 0;
                                if (cblcategory.Items[cat].Selected == true)
                                {
                                    chrowcount = 0;
                                    string catecode = cblcategory.Items[cat].Value;
                                    string categoryname = cblcategory.Items[cat].Text;
                                    fpsalarydemond.Sheets[0].RowCount++;
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = categoryname;
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                    fpsalarydemond.Sheets[0].SpanModel.Add(fpsalarydemond.Sheets[0].RowCount - 1, 1, 1, fpsalarydemond.Sheets[0].ColumnCount - 1);
                                    for (int dept = 0; dept < cbldepttype.Items.Count; dept++)
                                    {
                                        if (cbldepttype.Items[dept].Selected == true)
                                        {
                                            countstaff = 0;
                                            departmentacr = "";
                                            string deptcode = cbldepttype.Items[dept].Value;
                                            dstotal.Tables[0].DefaultView.RowFilter = "deptcode='" + deptcode + "' and category_code='" + catecode + "'";
                                            dvtaotal = dstotal.Tables[0].DefaultView;
                                            if (dvtaotal.Count > 0)
                                            {
                                                for (int dsv = 0; dsv < dvtaotal.Count; dsv++)
                                                {
                                                    string allowance = "";
                                                    string deduction = "";
                                                    string basicpay = "";
                                                    countstaff = countstaff + 1;
                                                    int k = 0;
                                                    int p = 4;
                                                    int col3 = 0;
                                                    col2 = 0;
                                                    col3 = col;
                                                    col2 = col;
                                                    basicpay = dvtaotal[dsv]["bsalary"].ToString();
                                                    netadd = dvtaotal[dsv]["netadd"].ToString();
                                                    netded = dvtaotal[dsv]["netded"].ToString();
                                                    netpa = dvtaotal[dsv]["netsal"].ToString();
                                                    gradepay = dvtaotal[dsv]["g_pay"].ToString();
                                                    if (chkdept.Checked == true)
                                                    {
                                                        departmentacr = Convert.ToString(dvtaotal[dsv]["deptname"]);
                                                        fpsalarydemond.Sheets[0].Columns[1].Width = 200;
                                                    }
                                                    else
                                                    {
                                                        departmentacr = Convert.ToString(dvtaotal[dsv]["dept_acronym"]);
                                                        fpsalarydemond.Sheets[0].Columns[1].Width = 100;
                                                    }
                                                    totalnetpay = Convert.ToDouble(netpa) + totalnetpay;
                                                    totaldeduction = Convert.ToDouble(netded) + totaldeduction;
                                                    totalgradepay = totalgradepay + Convert.ToDouble(gradepay);//shree1 gradepay - earning
                                                    earntotal = Convert.ToDouble(netadd) + earntotal;
                                                    basicpaytotal = Convert.ToDouble(basicpay) + basicpaytotal;
                                                    newnetpaytotal = Convert.ToDouble(netpa) + newnetpaytotal;
                                                    allowance = dvtaotal[dsv]["allowances"].ToString();
                                                    deduction = dvtaotal[dsv]["Deductions"].ToString();
                                                    string[] allowance2;
                                                    int g = 0;
                                                    string alowancesplit;
                                                    allowanmce_arr1 = allowance.Split('\\');
                                                    if (allowanmce_arr1.Length > 0)
                                                    {
                                                        for (m = 0; m < allowanmce_arr1.Length; m++)
                                                        {
                                                        l2: alowancesplit = allowanmce_arr1[m];
                                                            k = 0;
                                                            p = 5;
                                                            if (alowancesplit != "")
                                                            {
                                                                allowance2 = alowancesplit.Split(';');
                                                                da3 = allowance2[3];
                                                                if (allowance2.Length > 0)
                                                                {
                                                                l3: if (p < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                                    {
                                                                        string headval = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, p].Text;
                                                                        for (int j = 0; j < allowance2.Length; j++)
                                                                        {
                                                                            if (headval == allowance2[j])
                                                                            {
                                                                                DblAllowTotal[k] = Convert.ToDouble(DblAllowTotal[k]) + Convert.ToDouble(da3);
                                                                                DblNetAllowTotal = Convert.ToDouble(da3) + Convert.ToDouble(DblNetAllowTotal);
                                                                                m = m + 1;
                                                                                p = p + 1;
                                                                                k = k + 1;
                                                                                goto l2;
                                                                            }
                                                                            else
                                                                            {
                                                                                p = p + 1;
                                                                                k = k + 1;
                                                                                goto l3;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    string[] deduction_arr1;
                                                    string[] deduction2;
                                                    k = 0;
                                                    string deductionsplit;
                                                    deduction_arr1 = deduction.Split('\\');
                                                    if (deduction_arr1.Length > 0)
                                                    {
                                                        for (m = 0; m < deduction_arr1.Length; m++)
                                                        {
                                                        l2: deductionsplit = deduction_arr1[m];
                                                            col3 = col;
                                                            k = 0;
                                                            if (deductionsplit != "")
                                                            {
                                                                deduction2 = deductionsplit.Split(';');
                                                                da3 = deduction2[3];
                                                                if (deduction2.Length > 0)
                                                                {
                                                                l3: if (col3 < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                                    {
                                                                        string headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Text;
                                                                        string headval2 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Note;
                                                                        for (int j = 0; j < deduction2.Length; j++)
                                                                        {
                                                                            if (headval1 == deduction2[j] || headval2 == deduction2[j])//Modified by srinath 6/11/2014
                                                                            {
                                                                                deductiontotal[k] = deductiontotal[k] + Convert.ToDouble(da3);
                                                                                DblNetDedTotal = Convert.ToDouble(DblNetDedTotal) + Convert.ToDouble(da3);
                                                                                m = m + 1;
                                                                                col3 = col3 + 1;
                                                                                k = k + 1;
                                                                                goto l2;
                                                                            }
                                                                            else
                                                                            {
                                                                                col3 = col3 + 1;
                                                                                k = k + 1;
                                                                                goto l3;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //}//sri
                                                }
                                                int rowstr1 = fpsalarydemond.Sheets[0].RowCount++;
                                                if (rccheckcount == 0)
                                                {
                                                    chrowcount = rowstr1;
                                                }
                                                rccheckcount++;
                                                mname = month5.ToString();
                                                string month7 = getmonth(mname);
                                                sno++;
                                                /////////////////////////////////////////////////////////////////////////////////////////
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 0].Text = sno.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Tag = month5.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Text = departmentacr;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 3].Text = basicpaytotal.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 4].Text = totalgradepay.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Text = countstaff.ToString();
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Locked = true;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, 4].Font.Size = FontUnit.Medium;
                                                int g1 = 5;
                                                for (int i = 0; i < getval; i++)
                                                {
                                                    //DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 2);
                                                    DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, g1].Text = DblAllowTotal[i].ToString();
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, g1].HorizontalAlign = HorizontalAlign.Right;
                                                    g1 = g1 + 1;
                                                    DblAllowTotal[i] = 0;
                                                }
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].HorizontalAlign = HorizontalAlign.Right;
                                                for (int y = 0; y < getval2; y++)
                                                {
                                                    //deductiontotal[y] = Math.Round(deductiontotal[y], 2);
                                                    deductiontotal[y] = Math.Round(deductiontotal[y], 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, col2].Text = deductiontotal[y].ToString();
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, col2].HorizontalAlign = HorizontalAlign.Right;
                                                    col2 = col2 + 1;
                                                    deductiontotal[y] = 0;
                                                }
                                                col2 = 0;
                                                col2 = col;
                                                //DblNetDedTotal = Math.Round(DblNetDedTotal, 2);
                                                DblNetDedTotal = Math.Round(DblNetDedTotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].HorizontalAlign = HorizontalAlign.Right;
                                                netpaytotal = (basicpaytotal + DblNetAllowTotal + totalgradepay) - DblNetDedTotal;
                                                //netpaytotal = Math.Round(netpaytotal, 2);
                                                netpaytotal = Math.Round(netpaytotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                //earntotal = basicpaytotal + DblNetAllowTotal;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Text = Convert.ToString((basicpaytotal + DblNetAllowTotal + totalgradepay));  //earntotal.ToString();  Added By Jeyaprakash on July 23rd
                                                //  totaldeduction = DblNetDedTotal;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Locked = true;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = Convert.ToString(DblNetDedTotal);  //totaldeduction.ToString()  Added By Jeyaprakash on July 23rd
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Locked = true;
                                                //fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = netpaytotal.ToString();
                                                //fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = Convert.ToString(netpaytotal);  //newnetpaytotal.ToString();  Added By Jeyaprakash on July 23rd
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;
                                                fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].HorizontalAlign = HorizontalAlign.Right;
                                                basicpaytotal = 0;
                                                DblNetDedTotal = 0;
                                                DblNetAllowTotal = 0;
                                                netpaytotal = 0;
                                                totalgradepay = 0;
                                                newnetpaytotal = 0;
                                                totaldeduction = 0;
                                                earntotal = 0;
                                            }
                                        }
                                    }
                                }
                                if (rccheckcount != 0)
                                {
                                    fpsalarydemond.Sheets[0].RowCount++;
                                    fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = "Total";
                                    for (int intColCtr = 2; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                                    {
                                        IntMTotal = 0;
                                        for (int IntRowCtr = chrowcount; IntRowCtr < fpsalarydemond.Sheets[0].RowCount - 1; IntRowCtr++)
                                        {
                                            IntMTemp = 0;
                                            if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "-")
                                            {
                                                if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "")
                                                {
                                                    IntMTemp = Convert.ToDouble(fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text);
                                                }
                                                else
                                                {
                                                    IntMTemp = 0;
                                                }
                                            }
                                            else
                                            {
                                                IntMTemp = 0;
                                            }
                                            IntMTotal = IntMTemp + IntMTotal;
                                            //IntMTotal = Math.Round(IntMTotal, 2);
                                            IntMTotal = Math.Round(IntMTotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                        }
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                                        //string newtotal = "-";
                                        //if (IntMTotal>0)
                                        //{
                                        //    newtotal = Convert.ToString(IntMTotal);
                                        //}
                                        //fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = newtotal;
                                        if (intColCtr != 2)
                                        {
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        //end category
                        // staff type  start
                        else if (rbtype.SelectedValue == "2")
                        {
                            sno = 0;
                            for (int cat = 0; cat < chksstafftypelist.Items.Count; cat++)
                            {
                                rccheckcount = 0;
                                if (chksstafftypelist.Items[cat].Selected == true)
                                {
                                    chrowcount = 0;
                                    string catecode = chksstafftypelist.Items[cat].Value;
                                    string categoryname = chksstafftypelist.Items[cat].Text;
                                    fpsalarydemond.Sheets[0].RowCount++;
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = categoryname;
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                    fpsalarydemond.Sheets[0].SpanModel.Add(fpsalarydemond.Sheets[0].RowCount - 1, 1, 1, fpsalarydemond.Sheets[0].ColumnCount - 1);
                                    for (int dept = 0; dept < cbldepttype.Items.Count; dept++)
                                    {
                                        if (cbldepttype.Items[dept].Selected == true)
                                        {
                                            int pfcount = 1;
                                            if (chkpf.Checked == true)
                                            {
                                                pfcount = 2;
                                            }
                                            for (int pfc = 0; pfc < pfcount; pfc++)
                                            {
                                                countstaff = 0;
                                                departmentacr = "";
                                                string deptcode = cbldepttype.Items[dept].Value;
                                                if (chkpf.Checked == false)
                                                {
                                                    dstotal.Tables[0].DefaultView.RowFilter = "deptcode='" + deptcode + "' and stftype='" + catecode + "'";
                                                }
                                                else
                                                {
                                                    dstotal.Tables[0].DefaultView.RowFilter = "deptcode='" + deptcode + "' and stftype='" + catecode + "' and is_pf='" + pfc + "'";
                                                }
                                                dvtaotal = dstotal.Tables[0].DefaultView;
                                                if (dvtaotal.Count > 0)
                                                {
                                                    for (int dsv = 0; dsv < dvtaotal.Count; dsv++)
                                                    {
                                                        string allowance = "";
                                                        string deduction = "";
                                                        string basicpay = "";
                                                        countstaff = countstaff + 1;
                                                        int k = 0;
                                                        int p = 4;
                                                        int col3 = 0;
                                                        col2 = 0;
                                                        col3 = col;
                                                        col2 = col;
                                                        basicpay = dvtaotal[dsv]["bsalary"].ToString();
                                                        netadd = dvtaotal[dsv]["netadd"].ToString();
                                                        netded = dvtaotal[dsv]["netded"].ToString();
                                                        netpa = dvtaotal[dsv]["netsal"].ToString();
                                                        gradepay = dvtaotal[dsv]["g_pay"].ToString();
                                                        //Modified by srinath 28/4/2014
                                                        if (chkpf.Checked == false)
                                                        {
                                                            if (chkdept.Checked == true)
                                                            {
                                                                departmentacr = Convert.ToString(dvtaotal[dsv]["deptname"]);
                                                                fpsalarydemond.Sheets[0].Columns[1].Width = 200;
                                                            }
                                                            else
                                                            {
                                                                departmentacr = Convert.ToString(dvtaotal[dsv]["dept_acronym"]);
                                                                fpsalarydemond.Sheets[0].Columns[1].Width = 100;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string pfvalue = " / With Out PF";
                                                            if (pfc == 1)
                                                            {
                                                                pfvalue = " / With PF";
                                                            }
                                                            if (chkdept.Checked == true)
                                                            {
                                                                departmentacr = Convert.ToString(dvtaotal[dsv]["deptname"]) + pfvalue;
                                                                fpsalarydemond.Sheets[0].Columns[1].Width = 200;
                                                            }
                                                            else
                                                            {
                                                                departmentacr = Convert.ToString(dvtaotal[dsv]["dept_acronym"]) + pfvalue;
                                                                fpsalarydemond.Sheets[0].Columns[1].Width = 100;
                                                            }
                                                        }
                                                        totalnetpay = Convert.ToDouble(netpa) + totalnetpay;
                                                        totaldeduction = Convert.ToDouble(netded) + totaldeduction;
                                                        totalgradepay = totalgradepay + Convert.ToDouble(gradepay);
                                                        earntotal = Convert.ToDouble(netadd) + earntotal;
                                                        basicpaytotal = Convert.ToDouble(basicpay) + basicpaytotal;
                                                        newnetpaytotal = Convert.ToDouble(netpa) + newnetpaytotal;
                                                        allowance = dvtaotal[dsv]["allowances"].ToString();
                                                        deduction = dvtaotal[dsv]["Deductions"].ToString();
                                                        string[] allowance2;
                                                        int g = 0;
                                                        string alowancesplit;
                                                        allowanmce_arr1 = allowance.Split('\\');
                                                        if (allowanmce_arr1.Length > 0)
                                                        {
                                                            for (m = 0; m < allowanmce_arr1.Length; m++)
                                                            {
                                                            l2: alowancesplit = allowanmce_arr1[m];
                                                                k = 0;
                                                                p = 5;
                                                                if (alowancesplit != "")
                                                                {
                                                                    allowance2 = alowancesplit.Split(';');
                                                                    da3 = allowance2[3];
                                                                    if (allowance2.Length > 0)
                                                                    {
                                                                    l3: if (p < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                                        {
                                                                            string headval = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, p].Text;
                                                                            for (int j = 0; j < allowance2.Length; j++)
                                                                            {
                                                                                if (headval == allowance2[j])
                                                                                {
                                                                                    DblAllowTotal[k] = Convert.ToDouble(DblAllowTotal[k]) + Convert.ToDouble(da3);
                                                                                    DblNetAllowTotal = Convert.ToDouble(da3) + Convert.ToDouble(DblNetAllowTotal);
                                                                                    m = m + 1;
                                                                                    p = p + 1;
                                                                                    k = k + 1;
                                                                                    goto l2;
                                                                                }
                                                                                else
                                                                                {
                                                                                    p = p + 1;
                                                                                    k = k + 1;
                                                                                    goto l3;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            string[] deduction_arr1;
                                                            string[] deduction2;
                                                            k = 0;
                                                            string deductionsplit;
                                                            deduction_arr1 = deduction.Split('\\');
                                                            if (deduction_arr1.Length > 0)
                                                            {
                                                                for (m = 0; m < deduction_arr1.Length; m++)
                                                                {
                                                                l2: deductionsplit = deduction_arr1[m];
                                                                    col3 = col;
                                                                    k = 0;
                                                                    if (deductionsplit != "")
                                                                    {
                                                                        deduction2 = deductionsplit.Split(';');
                                                                        da3 = deduction2[3];
                                                                        if (deduction2[0].Trim() == "E.S.I")//delsi1004
                                                                        {
                                                                            string[] split = deduction2[2].Split('-');
                                                                            if (split.Length > 0)
                                                                            {
                                                                                da3 = split[1];
                                                                            }
                                                                        }

                                                                        if (deduction2.Length > 0)
                                                                        {
                                                                        l3: if (col3 < fpsalarydemond.Sheets[0].ColumnCount - 1)
                                                                            {
                                                                                string headval1 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Text;
                                                                                string headval2 = fpsalarydemond.Sheets[0].ColumnHeader.Cells[1, col3].Note;
                                                                                for (int j = 0; j < deduction2.Length; j++)
                                                                                {
                                                                                    if (headval1 == deduction2[j] || headval2 == deduction2[j])//Modified by srinath 6/11/2014
                                                                                    {
                                                                                        deductiontotal[k] = deductiontotal[k] + Convert.ToDouble(da3);
                                                                                        DblNetDedTotal = Convert.ToDouble(DblNetDedTotal) + Convert.ToDouble(da3);
                                                                                        m = m + 1;
                                                                                        col3 = col3 + 1;
                                                                                        k = k + 1;
                                                                                        goto l2;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        col3 = col3 + 1;
                                                                                        k = k + 1;
                                                                                        goto l3;
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    int rowstr1 = fpsalarydemond.Sheets[0].RowCount++;
                                                    if (rccheckcount == 0)
                                                    {
                                                        chrowcount = rowstr1;
                                                    }
                                                    rccheckcount++;
                                                    mname = month5.ToString();
                                                    string month7 = getmonth(mname);
                                                    sno++;
                                                    /////////////////////////////////////////////////////////////////////////////////////////
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 0].Text = sno.ToString();
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Tag = month5.ToString();
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 1].Text = departmentacr;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 3].Text = basicpaytotal.ToString();
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Text = countstaff.ToString();
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 2].Locked = true;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 4].Text = totalgradepay.ToString();
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 3].HorizontalAlign = HorizontalAlign.Right;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, 4].Font.Size = FontUnit.Medium;
                                                    int g1 = 5;
                                                    for (int i = 0; i < getval; i++)
                                                    {
                                                        //DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 2);
                                                        DblAllowTotal[i] = Math.Round(DblAllowTotal[i], 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                        fpsalarydemond.Sheets[0].Cells[rowstr1, g1].Text = DblAllowTotal[i].ToString();
                                                        fpsalarydemond.Sheets[0].Cells[rowstr1, g1].HorizontalAlign = HorizontalAlign.Right;
                                                        g1 = g1 + 1;
                                                        DblAllowTotal[i] = 0;
                                                    }
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].HorizontalAlign = HorizontalAlign.Right;
                                                    for (int y = 0; y < getval2; y++)
                                                    {
                                                        //deductiontotal[y] = Math.Round(deductiontotal[y], 2);
                                                        deductiontotal[y] = Math.Round(deductiontotal[y], 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                        fpsalarydemond.Sheets[0].Cells[rowstr1, col2].Text = deductiontotal[y].ToString();
                                                        fpsalarydemond.Sheets[0].Cells[rowstr1, col2].HorizontalAlign = HorizontalAlign.Right;
                                                        col2 = col2 + 1;
                                                        deductiontotal[y] = 0;
                                                    }
                                                    col2 = 0;
                                                    col2 = col;
                                                    //DblNetDedTotal = Math.Round(DblNetDedTotal, 2);
                                                    DblNetDedTotal = Math.Round(DblNetDedTotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].HorizontalAlign = HorizontalAlign.Right;
                                                    netpaytotal = (basicpaytotal + DblNetAllowTotal + totalgradepay) - DblNetDedTotal;
                                                    //netpaytotal = Math.Round(netpaytotal, 2);
                                                    netpaytotal = Math.Round(netpaytotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                                    //earntotal = basicpaytotal + DblNetAllowTotal;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Text = Convert.ToString((basicpaytotal + DblNetAllowTotal + totalgradepay));  //earntotal.ToString();  Added By Jeyaprakash on July 23rd
                                                    // totaldeduction = DblNetDedTotal;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colheder - 1].Locked = true;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Text = Convert.ToString(DblNetDedTotal);   //totaldeduction.ToString();  Added By Jeyaprakash on July 23rd
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 2].Locked = true;
                                                    //fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = netpaytotal.ToString();
                                                    //fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Text = Convert.ToString(netpaytotal);  //newnetpaytotal.ToString();  Added By Jeyaprakash on July 23rd
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].Locked = true;
                                                    fpsalarydemond.Sheets[0].Cells[rowstr1, colcount1 - 1].HorizontalAlign = HorizontalAlign.Right;
                                                    basicpaytotal = 0;
                                                    DblNetDedTotal = 0;
                                                    DblNetAllowTotal = 0;
                                                    netpaytotal = 0;
                                                    newnetpaytotal = 0;
                                                    totaldeduction = 0;
                                                    earntotal = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (rccheckcount != 0)
                                {
                                    fpsalarydemond.Sheets[0].RowCount++;
                                    fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 0].Locked = true;
                                    fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = "Total";
                                    for (int intColCtr = 2; intColCtr < fpsalarydemond.Sheets[0].ColumnCount; intColCtr++)
                                    {
                                        IntMTotal = 0;
                                        for (int IntRowCtr = chrowcount; IntRowCtr < fpsalarydemond.Sheets[0].RowCount - 1; IntRowCtr++)
                                        {
                                            IntMTemp = 0;
                                            if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "-")
                                            {
                                                if (fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "")
                                                {
                                                    IntMTemp = Convert.ToDouble(fpsalarydemond.Sheets[0].Cells[IntRowCtr, intColCtr].Text);
                                                }
                                                else
                                                {
                                                    IntMTemp = 0;
                                                }
                                            }
                                            else
                                            {
                                                IntMTemp = 0;
                                            }
                                            IntMTotal = IntMTemp + IntMTotal;
                                            //IntMTotal = Math.Round(IntMTotal, 2);
                                            IntMTotal = Math.Round(IntMTotal, 0, MidpointRounding.AwayFromZero);//barath 06.06.17
                                        }
                                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].Text = IntMTotal > 0 ? IntMTotal + "" : "-";
                                        if (intColCtr != 2)
                                        {
                                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, intColCtr].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    fpsalarydemond.Visible = false;
                    RptHead.InnerHtml = "";
                    grdPF.Visible = false;
                    grdPanel.Visible = false;
                    btnExport.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    lblexcel.Visible = false;
                    txtxl.Visible = false;
                    lblnorec.Visible = true;
                }
            }
            ds.Dispose();
            ds.Clear();
            ds = null;
            dvtaotal = null;
            dicallowdec.Clear();
            dicallowdec = null;
            if (rbtype.SelectedValue == "1")
            {
                fpsalarydemond.Sheets[0].Rows.Count++;
                fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, 1].Text = "Grand Total";
                fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Bold = true;
                fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Locked = true;
                fpsalarydemond.Sheets[0].Rows[fpsalarydemond.Sheets[0].RowCount - 1].Font.Size = FontUnit.Small;
                double gradetotalfinal = 0;
                Boolean istotal = false;
                for (int sv = 2; sv < fpsalarydemond.Sheets[0].Columns.Count; sv++)
                {
                    for (int vs = 0; vs < fpsalarydemond.Sheets[0].Rows.Count; vs++)
                    {
                        string totalname = fpsalarydemond.Sheets[0].Cells[vs, 1].Text.ToString();
                        if (totalname.Trim().ToLower() == "total")
                        {
                            istotal = true;
                            string isnum = fpsalarydemond.Sheets[0].Cells[vs, sv].Text.ToString();
                            if (isnum.Trim() != "-")
                            {
                                gradetotalfinal = gradetotalfinal + Convert.ToDouble(fpsalarydemond.Sheets[0].Cells[vs, sv].Text.ToString());
                            }
                        }
                    }
                    if (istotal == true)
                    {
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, sv].HorizontalAlign = HorizontalAlign.Right;
                        if (sv == 2)
                        {
                            fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, sv].HorizontalAlign = HorizontalAlign.Center;
                        }
                        fpsalarydemond.Sheets[0].Cells[fpsalarydemond.Sheets[0].RowCount - 1, sv].Text = Convert.ToString(gradetotalfinal);
                    }
                    istotal = false;
                    gradetotalfinal = 0;
                }
            }
            Double totalRows = 0;
            fpsalarydemond.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            totalRows = Convert.ToInt32(fpsalarydemond.Sheets[0].RowCount);
            if (totalRows >= 10)
            {
                fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                fpsalarydemond.Height = Convert.ToInt32(totalRows) * 30;
            }
            else if (totalRows == 0)
            {
                fpsalarydemond.Height = 500;
            }
            else
            {
                fpsalarydemond.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                fpsalarydemond.Height = (Convert.ToInt32(totalRows) * 30) + 200;
            }
            Session["totalPages"] = (int)Math.Ceiling(totalRows / fpsalarydemond.Sheets[0].PageSize);
            if (fpsalarydemond.Rows.Count > 0)
            {
                fpsalarydemond.Visible = true;
                lblexcel.Visible = true;
                txtxl.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                LoadPFStmnt();
            }
            else
            {
                fpsalarydemond.Visible = false;
                RptHead.InnerHtml = "";
                grdPF.Visible = false;
                grdPanel.Visible = false;
                btnExport.Visible = false;
                lblexcel.Visible = false;
                txtxl.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found!";
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "DepartmentWiseCummulative Salary.aspx");
        }
    }
    private void LoadPFStmnt()
    {
        try
        {
            string SelQ = string.Empty;
            string CatCode = string.Empty;
            DataSet dsPFStmnt = new DataSet();
            Dictionary<string, double> dicTotal = new Dictionary<string, double>();
            dicTotal.Clear();
            CatCode = GetSelectedItemsValueAsString(cblcategory);
            SelQ = " select COUNT(*) StaffCount,SUM(PF_Salary) PFDedSalary,SUM(PF) PFAmnt,SUM(NetAddAct) GrossSalary,category_name,sc.category_code from staffmaster sm,stafftrans st,staff_appl_master sa,hrdept_master h,desig_master desig,staffcategorizer sc,monthlypay m where sm.staff_code=m.staff_code and sm.staff_code=st.staff_code and sm.appl_no=sa.appl_no and sm.college_code=h.college_code and sm.college_code=desig.collegeCode and sm.college_code=sc.college_code and st.dept_code=h.dept_code and st.desig_code=desig.desig_code and st.category_code=sc.category_code and ISNULL(sm.Is_PF,'0')='1' and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code='" + Convert.ToString(Session["collegecode"]) + "'";
            if (!String.IsNullOrEmpty(CatCode.Trim()) && CatCode.Trim() == "0")
                SelQ = SelQ + " and st.category_code in('" + CatCode + "')";
            SelQ = SelQ + " group by category_name,sc.category_code order by sc.category_code";
            SelQ = SelQ + "   select COUNT(*) StaffCount,SUM(netsal) NetAmnt,sc.category_code,category_name from staffmaster sm,stafftrans st,staff_appl_master sa,hrdept_master h,desig_master desig,staffcategorizer sc,monthlypay m where sm.staff_code=st.staff_code and sm.staff_code=m.staff_code and sm.appl_no=sa.appl_no and sm.college_code=h.college_code and sm.college_code=desig.collegeCode and sm.college_code=sc.college_code and sm.college_code=m.college_code and st.dept_code=h.dept_code and st.desig_code=desig.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(sm.Discontinue,'0')='0' and ISNULL(sm.Is_PF,'0')='0' and sm.college_code='" + Convert.ToString(Session["collegecode"]) + "' and m.netsal>15000";
            if (!String.IsNullOrEmpty(CatCode.Trim()) && CatCode.Trim() == "0")
                SelQ = SelQ + " and st.category_code in('" + CatCode + "')";
            SelQ = SelQ + " group by category_name,sc.category_code order by sc.category_code";
            SelQ = SelQ + "   select COUNT(*) StaffCount,SUM(netsal) NetAmnt,sc.category_code,category_name from staffmaster sm,stafftrans st,staff_appl_master sa,hrdept_master h,desig_master desig,staffcategorizer sc,monthlypay m where sm.staff_code=st.staff_code and sm.staff_code=m.staff_code and sm.appl_no=sa.appl_no and sm.college_code=h.college_code and sm.college_code=desig.collegeCode and sm.college_code=sc.college_code and sm.college_code=m.college_code and st.dept_code=h.dept_code and st.desig_code=desig.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(sm.Discontinue,'0')='0' and ISNULL(sm.Is_PF,'0')='0' and sm.college_code='" + Convert.ToString(Session["collegecode"]) + "' and m.netsal<15000";
            if (!String.IsNullOrEmpty(CatCode.Trim()) && CatCode.Trim() == "0")
                SelQ = SelQ + " and st.category_code in('" + CatCode + "')";
            SelQ = SelQ + " group by category_name,sc.category_code order by sc.category_code";
            dsPFStmnt.Clear();
            dsPFStmnt = d2.select_method_wo_parameter(SelQ, "Text");
            DataView dvVal = new DataView();
            DataView dvGreater = new DataView();
            DataView dvLess = new DataView();
            if (dsPFStmnt.Tables.Count > 0 && dsPFStmnt.Tables[0].Rows.Count > 0)
            {
                double grossAmnt = 0;
                double PfDedSal = 0;
                double StfCount = 0;
                double PfSal = 0;
                double Pf12 = 0;
                double Pf8 = 0;
                double Pf3 = 0;
                double SalGreaterStf = 0;
                double SalLesserStf = 0;
                double SalGreater = 0;
                double SalLesser = 0;
                RptHead.InnerHtml = "PF STATEMENT FOR THE MONTH OF " + Convert.ToString(cblmonthfrom.SelectedItem.Text).ToUpper() + "-" + Convert.ToString(cblbatchyear.SelectedItem.Text) + "";
                DataTable dtPF = new DataTable();
                dtPF.Columns.Add("Category");
                dtPF.Columns.Add("Total Gross Earnings");
                dtPF.Columns.Add("No of PF Members");
                dtPF.Columns.Add("PF Salary(Total)");
                dtPF.Columns.Add("PF Deducting Wages");
                dtPF.Columns.Add("PF-12%");
                dtPF.Columns.Add("PF-8.33%");
                dtPF.Columns.Add("PF-3.67%");
                dtPF.Columns.Add("No. of Exempted Staff NPF>15000");
                dtPF.Columns.Add("Exempted Staff Salary(1)");
                dtPF.Columns.Add("No. of Exempted Staff NPF<15000");
                dtPF.Columns.Add("Exempted Staff Salary(2)");
                DataRow drPF;
                DataTable dtSumPF = new DataTable();
                dtSumPF.Columns.Add("PF Deduct Type");
                dtSumPF.Columns.Add("PF Wages");
                DataRow drSumPF;
                for (int cat = 0; cat < dsPFStmnt.Tables[0].Rows.Count; cat++)
                {
                    grossAmnt = 0;
                    PfDedSal = 0;
                    PfSal = 0;
                    Pf12 = 0;
                    Pf8 = 0;
                    Pf3 = 0;
                    SalGreaterStf = 0;
                    SalLesserStf = 0;
                    SalGreater = 0;
                    SalLesser = 0;
                    StfCount = 0;
                    drPF = dtPF.NewRow();
                    drPF[0] = Convert.ToString(dsPFStmnt.Tables[0].Rows[cat]["category_name"]);
                    dsPFStmnt.Tables[0].DefaultView.RowFilter = " category_code='" + Convert.ToString(dsPFStmnt.Tables[0].Rows[cat]["category_code"]) + "'";
                    dsPFStmnt.Tables[1].DefaultView.RowFilter = " category_code='" + Convert.ToString(dsPFStmnt.Tables[0].Rows[cat]["category_code"]) + "'";
                    dsPFStmnt.Tables[2].DefaultView.RowFilter = " category_code='" + Convert.ToString(dsPFStmnt.Tables[0].Rows[cat]["category_code"]) + "'";
                    dvVal = dsPFStmnt.Tables[0].DefaultView;
                    dvGreater = dsPFStmnt.Tables[1].DefaultView;
                    dvLess = dsPFStmnt.Tables[2].DefaultView;
                    if (dvVal.Count > 0)
                    {
                        drPF[1] = Convert.ToString(dvVal[0]["GrossSalary"]);
                        double.TryParse(Convert.ToString(dvVal[0]["GrossSalary"]), out grossAmnt);
                        grossAmnt = Math.Round(grossAmnt, 0, MidpointRounding.AwayFromZero);
                        if (!dicTotal.ContainsKey("1"))
                            dicTotal.Add("1", grossAmnt);
                        else
                        {
                            double myGross = 0;
                            double.TryParse(Convert.ToString(dicTotal["1"]), out myGross);
                            myGross += grossAmnt;
                            myGross = Math.Round(myGross, 0, MidpointRounding.AwayFromZero);
                            dicTotal["1"] = myGross;
                        }
                        drPF[2] = Convert.ToString(dvVal[0]["StaffCount"]);
                        double.TryParse(Convert.ToString(dvVal[0]["StaffCount"]), out StfCount);
                        if (!dicTotal.ContainsKey("2"))
                            dicTotal.Add("2", StfCount);
                        else
                        {
                            double myStaffC = 0;
                            double.TryParse(Convert.ToString(dicTotal["2"]), out myStaffC);
                            myStaffC += StfCount;
                            dicTotal["2"] = myStaffC;
                        }
                        drPF[3] = Convert.ToString(dvVal[0]["PFAmnt"]);
                        double.TryParse(Convert.ToString(dvVal[0]["PFAmnt"]), out PfSal);
                        PfSal = Math.Round(PfSal, 0, MidpointRounding.AwayFromZero);
                        if (!dicTotal.ContainsKey("3"))
                            dicTotal.Add("3", PfSal);
                        else
                        {
                            double myPfSal = 0;
                            double.TryParse(Convert.ToString(dicTotal["3"]), out myPfSal);
                            myPfSal += PfSal;
                            myPfSal = Math.Round(myPfSal, 0, MidpointRounding.AwayFromZero);
                            dicTotal["3"] = myPfSal;
                        }
                        drPF[4] = Convert.ToString(dvVal[0]["PFDedSalary"]);
                        double.TryParse(Convert.ToString(dvVal[0]["PFDedSalary"]), out PfDedSal);
                        PfDedSal = Math.Round(PfDedSal, 0, MidpointRounding.AwayFromZero);
                        if (!dicTotal.ContainsKey("4"))
                            dicTotal.Add("4", PfDedSal);
                        else
                        {
                            double myPfDedSal = 0;
                            double.TryParse(Convert.ToString(dicTotal["4"]), out myPfDedSal);
                            myPfDedSal += PfDedSal;
                            myPfDedSal = Math.Round(myPfDedSal, 0, MidpointRounding.AwayFromZero);
                            dicTotal["4"] = myPfDedSal;
                        }
                        if (PfDedSal > 0)
                        {
                            double.TryParse(Convert.ToString(((PfDedSal * 12) / 100)), out Pf12);
                            Pf12 = Math.Round(Pf12, 0, MidpointRounding.AwayFromZero);
                            drPF[5] = Convert.ToString(Pf12);
                            if (!dicTotal.ContainsKey("5"))
                                dicTotal.Add("5", Pf12);
                            else
                            {
                                double myPf12 = 0;
                                double.TryParse(Convert.ToString(dicTotal["5"]), out myPf12);
                                myPf12 += Pf12;
                                myPf12 = Math.Round(myPf12, 0, MidpointRounding.AwayFromZero);
                                dicTotal["5"] = myPf12;
                            }
                        }
                        else
                        {
                            drPF[5] = Convert.ToString("0");
                            if (!dicTotal.ContainsKey("5"))
                                dicTotal.Add("5", 0);
                        }
                        if (PfDedSal > 0)
                        {
                            double.TryParse(Convert.ToString(((PfDedSal * 8.33) / 100)), out Pf8);
                            Pf8 = Math.Round(Pf8, 0, MidpointRounding.AwayFromZero);
                            drPF[6] = Convert.ToString(Pf8);
                            if (!dicTotal.ContainsKey("6"))
                                dicTotal.Add("6", Pf8);
                            else
                            {
                                double myPf8 = 0;
                                double.TryParse(Convert.ToString(dicTotal["6"]), out myPf8);
                                myPf8 += Pf8;
                                myPf8 = Math.Round(myPf8, 0, MidpointRounding.AwayFromZero);
                                dicTotal["6"] = myPf8;
                            }
                        }
                        else
                        {
                            drPF[6] = Convert.ToString("0");
                            if (!dicTotal.ContainsKey("6"))
                                dicTotal.Add("6", 0);
                        }
                        if (PfDedSal > 0)
                        {
                            double.TryParse(Convert.ToString(((PfDedSal * 3.67) / 100)), out Pf3);
                            Pf3 = Math.Round(Pf3, 0, MidpointRounding.AwayFromZero);
                            drPF[7] = Convert.ToString(Pf3);
                            if (!dicTotal.ContainsKey("7"))
                                dicTotal.Add("7", Pf3);
                            else
                            {
                                double myPf3 = 0;
                                double.TryParse(Convert.ToString(dicTotal["7"]), out myPf3);
                                myPf3 += Pf3;
                                myPf3 = Math.Round(myPf3, 0, MidpointRounding.AwayFromZero);
                                dicTotal["7"] = myPf3;
                            }
                        }
                        else
                        {
                            drPF[7] = Convert.ToString("0");
                            if (!dicTotal.ContainsKey("7"))
                                dicTotal.Add("7", 0);
                        }
                        if (dvGreater.Count > 0)
                        {
                            double.TryParse(Convert.ToString(dvGreater[0]["StaffCount"]), out SalGreaterStf);
                            double.TryParse(Convert.ToString(dvGreater[0]["NetAmnt"]), out SalGreater);
                            SalGreaterStf = Math.Round(SalGreaterStf, 0, MidpointRounding.AwayFromZero);
                            SalGreater = Math.Round(SalGreater, 0, MidpointRounding.AwayFromZero);
                            drPF[8] = Convert.ToString(SalGreaterStf);
                            if (!dicTotal.ContainsKey("8"))
                                dicTotal.Add("8", SalGreaterStf);
                            else
                            {
                                double GreaterStf = 0;
                                double.TryParse(Convert.ToString(dicTotal["8"]), out GreaterStf);
                                GreaterStf += SalGreaterStf;
                                GreaterStf = Math.Round(GreaterStf, 0, MidpointRounding.AwayFromZero);
                                dicTotal["8"] = GreaterStf;
                            }
                            if (SalGreater > 0)
                                drPF[9] = Convert.ToString(SalGreater);
                            if (!dicTotal.ContainsKey("9"))
                                dicTotal.Add("9", SalGreater);
                            else
                            {
                                double GreaterSal = 0;
                                double.TryParse(Convert.ToString(dicTotal["9"]), out GreaterSal);
                                GreaterSal += SalGreater;
                                GreaterSal = Math.Round(GreaterSal, 0, MidpointRounding.AwayFromZero);
                                dicTotal["9"] = GreaterSal;
                            }
                        }
                        else
                        {
                            drPF[8] = Convert.ToString("0");
                            if (!dicTotal.ContainsKey("8"))
                                dicTotal.Add("8", 0);
                            if (!dicTotal.ContainsKey("9"))
                                dicTotal.Add("9", 0);
                        }
                        if (dvLess.Count > 0)
                        {
                            double.TryParse(Convert.ToString(dvLess[0]["StaffCount"]), out SalLesserStf);
                            double.TryParse(Convert.ToString(dvLess[0]["NetAmnt"]), out SalLesser);
                            SalGreaterStf = Math.Round(SalLesserStf, 0, MidpointRounding.AwayFromZero);
                            SalGreater = Math.Round(SalLesser, 0, MidpointRounding.AwayFromZero);
                            drPF[10] = Convert.ToString(SalLesserStf);
                            if (!dicTotal.ContainsKey("10"))
                                dicTotal.Add("10", SalLesserStf);
                            else
                            {
                                double LesserStf = 0;
                                double.TryParse(Convert.ToString(dicTotal["10"]), out LesserStf);
                                LesserStf += SalLesserStf;
                                LesserStf = Math.Round(LesserStf, 0, MidpointRounding.AwayFromZero);
                                dicTotal["10"] = LesserStf;
                            }
                            if (SalLesser > 0)
                                drPF[11] = Convert.ToString(SalLesser);
                            if (!dicTotal.ContainsKey("11"))
                                dicTotal.Add("11", SalLesser);
                            else
                            {
                                double LesserSal = 0;
                                double.TryParse(Convert.ToString(dicTotal["11"]), out LesserSal);
                                LesserSal += SalLesser;
                                LesserSal = Math.Round(LesserSal, 0, MidpointRounding.AwayFromZero);
                                dicTotal["11"] = LesserSal;
                            }
                        }
                        else
                        {
                            drPF[10] = Convert.ToString("0");
                            if (!dicTotal.ContainsKey("10"))
                                dicTotal.Add("10", 0);
                            if (!dicTotal.ContainsKey("11"))
                                dicTotal.Add("11", 0);
                        }
                        dtPF.Rows.Add(drPF);
                    }
                }
                drPF = dtPF.NewRow();
                drPF[0] = "Total";
                double NetPF = 0;
                double TotNetPF = 0;
                foreach (KeyValuePair<string, double> dic in dicTotal)
                {
                    drPF[Convert.ToInt32(dic.Key)] = Convert.ToString(dic.Value);
                    if (Convert.ToString(dic.Key) == "4")
                    {
                        double.TryParse(Convert.ToString(dic.Value), out NetPF);
                        if (NetPF > 0)
                        {
                            dtSumPF.Rows.Add(Convert.ToString("AC 01"), Convert.ToString(Math.Round(((NetPF * 3.67) / 100), 0, MidpointRounding.AwayFromZero)));
                            dtSumPF.Rows.Add(Convert.ToString("AC 01"), Convert.ToString(Math.Round(((NetPF * 12) / 100), 0, MidpointRounding.AwayFromZero)));
                            dtSumPF.Rows.Add(Convert.ToString("A/C-10"), Convert.ToString(Math.Round(((NetPF * 8.33) / 100), 0, MidpointRounding.AwayFromZero)));
                            dtSumPF.Rows.Add(Convert.ToString("1.1 A/C-02"), Convert.ToString(Math.Round(((NetPF * 0.85) / 100), 0, MidpointRounding.AwayFromZero)));
                            dtSumPF.Rows.Add(Convert.ToString("0.5 A/C-21"), Convert.ToString(Math.Round(((NetPF * 0.5) / 100), 0, MidpointRounding.AwayFromZero)));
                            dtSumPF.Rows.Add(Convert.ToString("0.01 A/C-22"), Convert.ToString(Math.Round(((NetPF * 0.01) / 100), 0, MidpointRounding.AwayFromZero)));
                            TotNetPF = ((NetPF * 3.67) / 100) + ((NetPF * 12) / 100) + ((NetPF * 8.33) / 100) + ((NetPF * 0.85) / 100) + ((NetPF * 0.5) / 100) + ((NetPF * 0.01) / 100);
                            dtSumPF.Rows.Add(Convert.ToString("Total"), Convert.ToString(Math.Round(TotNetPF, 0, MidpointRounding.AwayFromZero)));
                            grdSummary.Visible = true;
                            grdSummary.DataSource = dtSumPF;
                            grdSummary.DataBind();
                            grdLoadColor(grdSummary);
                        }
                        else
                        {
                            grdSummary.Visible = false;
                        }
                    }
                }
                dtPF.Rows.Add(drPF);
                grdPF.Visible = true;
                grdPanel.Visible = true;
                btnExport.Visible = true;
                grdPF.DataSource = dtPF;
                grdPF.DataBind();
                grdLoadColor(grdPF);
            }
            else
            {
                RptHead.InnerHtml = "";
                grdPF.Visible = false;
                grdPanel.Visible = false;
                btnExport.Visible = false;
            }
        }
        catch { }
    }
    private void grdLoadColor(GridView gvGrd)
    {
        try
        {
            gvGrd.Rows[gvGrd.Rows.Count - 1].Font.Bold = true;
            gvGrd.Rows[gvGrd.Rows.Count - 1].Font.Name = "Book Antiqua";
            gvGrd.Rows[gvGrd.Rows.Count - 1].Font.Size = FontUnit.Medium;
            gvGrd.Rows[gvGrd.Rows.Count - 1].HorizontalAlign = HorizontalAlign.Center;
            gvGrd.Rows[gvGrd.Rows.Count - 1].BackColor = Color.LightGreen;
        }
        catch { }
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    protected void rbtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chkpf.Checked = false;
            if (rbtype.SelectedValue == "0")
            {
                chkpf.Visible = false;
            }
            else if (rbtype.SelectedValue == "1")
            {
                chkpf.Visible = false;
            }
            else if (rbtype.SelectedValue == "2")
            {
                chkpf.Visible = true;
            }
        }
        catch
        {
        }
    }
}