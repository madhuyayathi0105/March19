using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Configuration;
public partial class HRMOD_Quaterly_Report : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    int i = 0;
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    DAccess2 d2 = new DAccess2();
    Boolean cellclick = false;
    string q1 = "";
    string activerow = "";
    string activecol = "";
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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            //lblvalidation1.Text = "";
            if (!IsPostBack)
            {
                ViewState["DateType"] = null;
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                binddept();
                designation();
                category();
                bindmonth();
                bindyear();
                //getmonth((int monthvalue);
                //bindallowance();
                //binddeduction();
            }
        }
        catch (Exception ex) { }
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
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string item = "select distinct Dept_Code,Dept_Name from Department where college_code = '" + clgcode + "' order by Dept_Name";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "Dept_Name";
                cbl_dept.DataValueField = "Dept_Code";
                cbl_dept.DataBind();
                if (cbl_dept.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        cbl_dept.Items[i].Selected = true;
                    }
                    txt_dept.Text = "Department (" + cbl_dept.Items.Count + ")";
                    cb_dept.Checked = true;
                }
            }
            else
            {
                txt_dept.Text = "--Select--";
                cb_dept.Checked = false;
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void designation()
    {
        try
        {
            ds.Clear();
            cbl_desig.Items.Clear();
            string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + clgcode + "' order by desig_name";
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
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void category()
    {
        try
        {
            ds.Clear();
            cbl_staffc.Items.Clear();
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + clgcode + "' order by category_Name";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staffc.DataSource = ds;
                cbl_staffc.DataTextField = "category_Name";
                cbl_staffc.DataValueField = "category_code";
                cbl_staffc.DataBind();
                cbl_staffc.Visible = true;
                if (cbl_staffc.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staffc.Items.Count; i++)
                    {
                        cbl_staffc.Items[i].Selected = true;
                    }
                    txt_staffc.Text = "Category(" + cbl_staffc.Items.Count + ")";
                    cb_staffc.Checked = true;
                }
            }
            else
            {
                txt_staffc.Text = "--Select--";
                cb_staffc.Checked = false;
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected string getmonth(int monthvalue)
    {
        string month = "";
        try
        {
            DateTime dt = new DateTime(2000, 1, 1);
            month = Convert.ToString(dt.AddMonths(monthvalue - 1).ToString("MMMM"));
        }
        catch { }
        return month;
    }
    protected int getmonthvalue(string monthvalue)
    {
        int i = 0;
        try
        {
            i = DateTime.ParseExact(monthvalue, "MMMM", CultureInfo.CurrentCulture).Month;
        }
        catch { }
        return i;
    }
    protected void bindyear()
    {
        int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy")) + 1;
        for (int l = 0; l < 15; l++)
        {
            ddl_toyear.Items.Add(Convert.ToString(year));
            ddl_fromyear.Items.Add(Convert.ToString(year));
            year--;
        }
        int years = Convert.ToInt32(System.DateTime.Now.ToString("yyyy")) + 1;
        for (int j = 0; j < 15; j++)
        {
            toyear.Items.Add(Convert.ToString(years));
            Fromyear.Items.Add(Convert.ToString(years));
            years--;
        }
    }
    protected void bindmonth()
    {
        DateTime dt = new DateTime(2000, 1, 1);
        for (int m = 0; m < 12; m++)
        {
            ddl_frommonth.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
            ddl_tomonth.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
        }
        for (int n = 0; n < 12; n++)
        {
            frommonth.Items.Add(new ListItem(dt.AddMonths(n).ToString("MMMM"), (n + 1).ToString().TrimStart('0')));
            tomonth.Items.Add(new ListItem(dt.AddMonths(n).ToString("MMMM"), (n + 1).ToString().TrimStart('0')));
        }
    }
    protected void ddlcollege_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            binddept();
            designation();
            category();
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    protected void cb_desig_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cbl_desig_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cb_staffc_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_staffc, cbl_staffc, txt_staffc, "Category");
    }
    protected void cbl_staffc_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_staffc, cbl_staffc, txt_staffc, "Category");
    }
    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
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
    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
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
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '%" + prefixText + "%' and college_code='" + clgcode + "'";
        name = ws.Getname(query);
        return name;
    }
    protected void Year_click(object sender, EventArgs e)
    {
        string frommonth = "";
        string tomonth = "";
        string fromyear = "";
        string toyear = "";
        frommonth = Convert.ToString(ddl_frommonth.SelectedItem.Value);
        tomonth = Convert.ToString(ddl_tomonth.SelectedItem.Value);
        fromyear = Convert.ToString(ddl_fromyear.SelectedItem.Value);
        toyear = Convert.ToString(ddl_toyear.SelectedItem.Value);
        DateTime dt = new DateTime(Convert.ToInt32(fromyear), Convert.ToInt32(frommonth), 28);
        DateTime dts = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
        if (dts < dt)
        {
            ermsg.Visible = true;
            ermsg.Text = "Select Year/Month Greater Than the Current Year/Month";
        }
        else
        {
            ermsg.Visible = false;
            ermsg.Text = "";
        }
    }
    protected void Month_click(object sender, EventArgs e)
    {
        Year_click(sender, e);
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            Hashtable DeductionHash = new Hashtable();
            int sno = 0;
            string query = "";
            string deptcode = rs.GetSelectedItemsValueAsString(cbl_dept);
            string designation = rs.GetSelectedItemsValueAsString(cbl_desig);
            string stafftype = rs.GetSelectedItemsValueAsString(cbl_staffc);
            Fpspread1.Sheets[0].Visible = true;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].ColumnCount = 13;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            // string deductiondates = Convert.ToString(txt_deduction.Text);
            //string[] deductiondates = txt_deduction.Text.Split('/');
            //string month = Convert.ToString(deductiondates[1].ToString()); 
            string frommonth = "";
            string tomonth = "";
            string fromyear = "";
            string toyear = "";
            frommonth = Convert.ToString(ddl_frommonth.SelectedItem.Value);
            tomonth = Convert.ToString(ddl_tomonth.SelectedItem.Value);
            fromyear = Convert.ToString(ddl_fromyear.SelectedItem.Value);
            toyear = Convert.ToString(ddl_toyear.SelectedItem.Value);
            query = "select  s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber,m.netadd,convert(varchar(max),m.deductions)deductions,m.paymonth,m.payyear from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm,monthlypay m where  m.staff_code=s.staff_code and m.staff_code=st.staff_code and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and h.dept_code in('" + deptcode + "')  and s.college_code='" + ddlcollege.SelectedItem.Value + "' and resign = 0 and settled = 0 and ISNULL(Discontinue,'0')='0' and st.latestrec=1  and CAST(CONVERT(varchar(20),m.PayMonth)+'/01/'+CONVERT(varchar(20),m.PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) group by m.payyear,m.paymonth, LEN(s.staff_code),s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber,m.netadd,convert(varchar(max),m.deductions)order by  year(m.payyear),month(m.paymonth)";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "text");
            DataSet data = new DataSet();
            string collegecode = ddlcollege.SelectedItem.Value;
            string query1 = "select MasterCriteria1,MasterValue from CO_MasterValues where MasterCriteria='Quarterly Report Date' and collegecode='" + collegecode + "'";
            query1 += " select MasterCriteria1,MasterCriteriaValue1,MasterCriteriaValue2,MasterValue  from CO_MasterValues where MasterCriteria='Quarterly Report DepositDate' and collegecode='" + collegecode + "'";
            data = d2.select_method_wo_parameter(query1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ermsg.Visible = false;
                Fpspread1.Visible = true;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].ColumnHeader.Columns[0].Label = "Month";
                Fpspread1.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Columns[1].Label = "Employee No";
                Fpspread1.Sheets[0].ColumnHeader.Columns[2].Label = "PAN No";
                Fpspread1.Sheets[0].ColumnHeader.Columns[3].Label = "Name of the Employee";
                Fpspread1.Sheets[0].ColumnHeader.Columns[4].Label = "Date of the payment/ credited";
                Fpspread1.Sheets[0].ColumnHeader.Columns[5].Label = "Taxable amount on which tax Deducted";
                Fpspread1.Sheets[0].ColumnHeader.Columns[6].Label = "TDS";
                Fpspread1.Sheets[0].ColumnHeader.Columns[7].Label = "Education CESS";
                Fpspread1.Sheets[0].ColumnHeader.Columns[8].Label = "Total Tax Deducted";
                Fpspread1.Sheets[0].ColumnHeader.Columns[9].Label = "Total Tax Deposited";
                Fpspread1.Sheets[0].ColumnHeader.Columns[10].Label = "Date of Deduction";
                Fpspread1.Sheets[0].ColumnHeader.Columns[11].Label = "Date of Deposit";
                Fpspread1.Sheets[0].ColumnHeader.Columns[12].Label = "CHALLAN No";
                //  Fpspread1.Sheets[0].Columns[0].Width = 100;
                Fpspread1.Sheets[0].Columns[0].Width = 80;
                Fpspread1.Sheets[0].Columns[1].Width = 100;
                Fpspread1.Sheets[0].Columns[2].Width = 150;
                Fpspread1.Sheets[0].Columns[3].Width = 250;
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Sheets[0].Columns[1].Locked = true;
                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Sheets[0].Columns[3].Locked = true;
                Fpspread1.Sheets[0].Columns[4].Locked = true;
                Fpspread1.Sheets[0].Columns[5].Locked = true;
                Fpspread1.Sheets[0].Columns[6].Locked = true;
                Fpspread1.Sheets[0].Columns[7].Locked = true;
                Fpspread1.Sheets[0].Columns[8].Locked = true;
                Fpspread1.Sheets[0].Columns[9].Locked = true;
                Fpspread1.Sheets[0].Columns[10].Locked = true;
                Fpspread1.Sheets[0].Columns[11].Locked = true;
                Fpspread1.Sheets[0].Columns[12].Locked = true;
                ArrayList arrPayMonthYear = new ArrayList();
                string Amt = string.Empty;
                string preMon = string.Empty;
                for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    string paymonth = ds.Tables[0].Rows[rolcount]["paymonth"].ToString();
                    string payYear = ds.Tables[0].Rows[rolcount]["payyear"].ToString();
                    int months = Convert.ToInt32(paymonth);
                    string monthyear = Convert.ToString(months) + "/" + payYear;
                    if (monthyear != preMon)//barath 17.01.18
                        sno = 0;
                    preMon = monthyear;
                    paymonth = getmonth(months);
                    string datemon = string.Empty;
                    string deductdate = string.Empty;
                    //int year = Convert.ToInt32(payYear);
                    DataView dv = new DataView();
                    data.Tables[0].DefaultView.RowFilter = " MasterValue='" + monthyear + "'";
                    dv = data.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        datemon = Convert.ToString(dv[0]["MasterCriteria1"]);
                        deductdate = datemon + "/" + monthyear;
                    }
                    else
                    {
                        deductdate = string.Empty;
                    }
                    if (!arrPayMonthYear.Contains(Convert.ToString(paymonth + "@" + payYear).Trim().ToLower()))
                    {
                        if (arrPayMonthYear.Count > 0)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                            Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].ForeColor = Color.DarkRed;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = "Total";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            for (int Totalcol = 5; Totalcol < 10; Totalcol++)
                            {
                                Amt = string.Empty;
                                if (DeductionHash.ContainsKey(Totalcol))
                                    Amt = Convert.ToString(DeductionHash[Totalcol]);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Totalcol].Text = Amt;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Totalcol].HorizontalAlign = HorizontalAlign.Right;
                            }
                            DeductionHash.Clear();
                        }
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(paymonth) + "-" + payYear;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 12);
                        arrPayMonthYear.Add(Convert.ToString(paymonth + "@" + payYear).Trim().ToLower());
                    }
                    sno++;
                    string name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    string code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();
                    string panno = ds.Tables[0].Rows[rolcount]["pangirnumber"].ToString();
                    double gross = 0;
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[rolcount]["netadd"]), out gross);
                    string tds = ds.Tables[0].Rows[rolcount]["deductions"].ToString();
                    string incTax = "";
                    string deductions = Convert.ToString(ds.Tables[0].Rows[rolcount]["deductions"]);
                    string[] deductionlist = deductions.Split('\\');
                    for (int k = 0; k < deductionlist.GetUpperBound(0); k++)
                    {
                        string getactal = deductionlist[k];
                        if (getactal.Trim() != "" && getactal != null)
                        {
                            string[] actallspv = getactal.Split(';');
                            if (actallspv.GetUpperBound(0) >= 3)
                            {
                                if (actallspv[0].ToString().Trim().ToLower() == "inc tax" || actallspv[0].ToString().Trim().ToLower() == "income tax")
                                {
                                    string de = actallspv[0];
                                    string de1 = actallspv[1];
                                    string de2 = actallspv[2];
                                    string[] dedspl = de2.Split('-');
                                    if (dedspl.Length == 2)
                                    {
                                        if (de1.Trim().ToUpper() == "PERCENT")
                                            incTax = Convert.ToString(dedspl[1]);
                                        else if (de1.Trim().ToUpper() == "SLAB")
                                            incTax = Convert.ToString(dedspl[1]);
                                        else
                                            incTax = Convert.ToString(dedspl[0]);
                                    }
                                    else
                                    {
                                        incTax = Convert.ToString(actallspv[3]);
                                    }
                                    //double InctaxAmt = 0;
                                    //double.TryParse(incTax, out InctaxAmt);
                                    // TotalTaxAmt = InctaxAmt;
                                }
                            }
                        }
                    }
                    string date = "";
                    double educationcess = 0;
                    double tax = 0;
                    double.TryParse(incTax, out tax);
                    educationcess = Math.Round(tax / 100 * 3);
                    double totaltaxdeduct = Math.Round(tax + educationcess);
                    //string challan = "";
                    Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                    Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Font.Bold = false;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = code;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = panno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = name;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = date;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(gross);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = incTax;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(educationcess);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(totaltaxdeduct);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(totaltaxdeduct);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].CellType = txt;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = deductdate;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;

                    //barath 18.12.17

                    for (int Totalcol = 5; Totalcol < 10; Totalcol++)
                    {
                        double PrevAmt = 0;
                        double Amount = 0;
                        double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Totalcol].Text), out Amount);
                        if (!DeductionHash.ContainsKey(Totalcol))
                            DeductionHash.Add(Totalcol, Amount);
                        else
                        {
                            PrevAmt = 0;
                            double.TryParse(Convert.ToString(DeductionHash[Totalcol]), out PrevAmt);
                            Amount += PrevAmt;
                            DeductionHash[Totalcol] = Amount;
                        }
                    }
                    string depositDate = string.Empty;
                    string challanNo = string.Empty;
                    string monyears = string.Empty;
                    DataView dv1 = new DataView();
                    data.Tables[1].DefaultView.RowFilter = " MasterValue='" + monthyear + "'";
                    dv1 = data.Tables[1].DefaultView;
                    if (dv1.Count > 0)
                    {
                        monyears = Convert.ToString(dv1[0]["MasterCriteriavalue2"]);
                        challanNo = Convert.ToString(dv1[0]["MasterCriteriavalue1"]);
                        datemon = Convert.ToString(dv1[0]["MasterCriteria1"]);
                       // depositDate = datemon + "/" + monthyear;
                        depositDate = datemon + "/" + monyears;
                    }
                    else
                    {
                        depositDate = string.Empty;
                        challanNo = string.Empty;
                    }
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].CellType = txt;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Text = depositDate;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].CellType = txt;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].Text = challanNo;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Left;
                }
                //barath 18.12.17
                Amt = string.Empty;
                if (DeductionHash.Count > 0)
                {
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                    Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].ForeColor = Color.DarkRed;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = "Total";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    for (int Totalcol = 5; Totalcol < 10; Totalcol++)
                    {
                        Amt = string.Empty;
                        if (DeductionHash.ContainsKey(Totalcol))
                            Amt = Convert.ToString(DeductionHash[Totalcol]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Totalcol].Text = Amt;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Totalcol].HorizontalAlign = HorizontalAlign.Right;
                    }
                }
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Width = 1000;
                Fpspread1.Height = 500;
                rptprint.Visible = true;
            }
            else
            {
                if ((txt_dept.Text == "--Select--") || (txt_desig.Text == "--Select--") || (txt_staffc.Text == "--Select--"))
                {
                    ermsg.Visible = true;
                    ermsg.Text = "Please Select Atleast One Item";
                }
                else
                {
                    Fpspread1.Visible = false;
                    ermsg.Visible = true;
                    ermsg.Text = "No Records Found";
                    rptprint.Visible = false;
                    //btnprintmaster.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        cellclick = true;
    }
    protected void Fpspread1_ButtonCommand(object sender, EventArgs e)
    {
        try
        {
            if (Fpspread1.Sheets[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 1].Value) == 1)
                {
                    for (int intF = 0; intF < Fpspread1.Sheets[0].Rows.Count; intF++)
                    {
                        Fpspread1.Sheets[0].Cells[intF, 1].Value = 1;
                    }
                }
                else
                {
                    for (int intF = 0; intF < Fpspread1.Sheets[0].Rows.Count; intF++)
                    {
                        Fpspread1.Sheets[0].Cells[intF, 1].Value = 0;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Quarterly Report";
            string pagename = "Quaterly_Report.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
            lblvalidation1.Visible = false;
        }
        catch
        {
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }

    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        popupwindow.Visible = false;
    }
    protected void btnsavedateclick_Click(object sender, EventArgs e)
    {
        try
        {
            string fromyr = Fromyear.SelectedItem.Value;
            string toyr = toyear.SelectedItem.Value;
            string frommon = frommonth.SelectedItem.Value;
            string tomon = tomonth.SelectedItem.Value;
            DateTime FCalYearDT = new DateTime(Convert.ToInt32(fromyr), Convert.ToInt32(frommon), 28);
            DateTime TCalYearDT = new DateTime(Convert.ToInt32(toyr), Convert.ToInt32(tomon), 28);
            DateTime DummyDT = new DateTime();
            DummyDT = FCalYearDT;
            DummyDT = DummyDT.AddMonths(1);
            TCalYearDT = TCalYearDT.AddMonths(1);
            DataTable dtCol = new DataTable();
            dtCol.Columns.Add("S.No");
            dtCol.Columns.Add("MonthYear");
            dtCol.Columns.Add("Date");
            dtCol.Columns.Add("ChallanNo");
            dtCol.Columns.Add("MonthYearDb");
            DummyDT = DummyDT.AddMonths(-1);
            while (DummyDT < TCalYearDT)
            {
                DataRow dr = dtCol.NewRow();
                dr["MonthYear"] = DummyDT.ToString("yyyy") + "-" + DummyDT.ToString("MM");
                dr["MonthYearDb"] = DummyDT.ToString("MM").TrimStart('0') + "/" + DummyDT.ToString("yyyy");
                dtCol.Rows.Add(dr);
                DummyDT = DummyDT.AddMonths(1);
            }
            if (Convert.ToString(ViewState["DateType"]) == "2")
                griddate.Columns[3].Visible = true;
                
            else
                griddate.Columns[3].Visible = false;
            btnsave.Visible = true;
            griddate.Visible = true;
            griddate.DataSource = dtCol;
            griddate.DataBind();
           
            ds.Clear();
            ds = d2.select_method_wo_parameter("select MasterCriteria1,MasterCriteriaValue1,MasterCriteriaValue2,MasterValue,MasterCriteria from CO_MasterValues where MasterCriteria in('Quarterly Report DepositDate','Quarterly Report Date') and collegecode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' ", "text");
            string monYear = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                string LinkName = string.Empty;
                if (Convert.ToString(ViewState["DateType"]) == "1")
                    LinkName = "Quarterly Report Date";
                if (Convert.ToString(ViewState["DateType"]) == "2")
                    LinkName = "Quarterly Report DepositDate";
                foreach (GridViewRow dr in griddate.Rows)
                {
                    monYear = Convert.ToString((dr.FindControl("lbl_monthyear1") as Label).Text);
                    DataView dv = new DataView();
                    ds.Tables[0].DefaultView.RowFilter = " MasterValue='" + monYear + "' and MasterCriteria='" + LinkName + "'";
                    dv = ds.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        for (int day = 0; day < dv.Count; day++)
                        {
                            if(LinkName== "Quarterly Report DepositDate")
                            {
                                string monthyearval = Convert.ToString(dv[0]["MasterCriteriaValue2"]);
                                string monyeartxtval=string.Empty;
                                if(monthyearval.Contains('/'))
                                {
                                   string[] splitval=monthyearval.Split('/');
                                    monyeartxtval=Convert.ToString(splitval[1])+"-"+Convert.ToString(splitval[0]);

                                
                                }
                                TextBox txtmonyear = dr.FindControl("lbl_monthyear") as TextBox;
                                txtmonyear.Text = Convert.ToString(monyeartxtval);
                                string Date = Convert.ToString(dv[0]["MasterCriteria1"]);
                                DropDownList ddl = dr.FindControl("dddate") as DropDownList;
                                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByText(Date));

                                TextBox txtChallan = dr.FindControl("txtChallanNo") as TextBox;
                                txtChallan.Text = Convert.ToString(dv[0]["MasterCriteriaValue1"]);
                            }
                            else
                            {

                            string Date = Convert.ToString(dv[0]["MasterCriteria1"]);
                            DropDownList ddl = dr.FindControl("dddate") as DropDownList;
                            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByText(Date));

                            TextBox txtChallan = dr.FindControl("txtChallanNo") as TextBox;
                            txtChallan.Text = Convert.ToString(dv[0]["MasterCriteriaValue1"]);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnsavedate_Click(object sender, EventArgs e)
    {
        // string deductiondates = Convert.ToString(txt_deduction.Text);
        //string[] deductiondates = txt_deduction.Text.Split('/');
        //string month = Convert.ToString(deductiondates[1].ToString()); 
        try
        {
            DataSet ds = new DataSet();
            if (griddate.Rows.Count > 0)
            {
                string LinkName = string.Empty;
                if (Convert.ToString(ViewState["DateType"]) == "1")
                    LinkName = "Quarterly Report Date";
                if (Convert.ToString(ViewState["DateType"]) == "2")
                    LinkName = "Quarterly Report DepositDate";
                for (int i = 0; i < griddate.Rows.Count; i++)
                {
                    string clgcod = Convert.ToString(ddlcollege.SelectedItem.Value);
                   // string monthyear = Convert.ToString((griddate.Rows[i].FindControl("lbl_monthyear") as Label).Text);
                    string monthyear = Convert.ToString((griddate.Rows[i].FindControl("lbl_monthyear") as TextBox).Text);
                    string monthyear1 = Convert.ToString((griddate.Rows[i].FindControl("lbl_monthyear1") as Label).Text);
                    string[] monthyearsplit = monthyear.Split('-');
                   
                    string month = Convert.ToString(monthyearsplit[1].ToString().TrimStart('0'));
                   
                    string year = Convert.ToString(monthyearsplit[0].ToString());
                   
                    string date = Convert.ToString((griddate.Rows[i].FindControl("dddate") as DropDownList).Text);
                    string monthdate = month + "/" + year;
                   
                    string ChallanNo = Convert.ToString((griddate.Rows[i].FindControl("txtChallanNo") as TextBox).Text);
                    //string query = "if exists (select MasterValue from CO_MasterValues where MasterCriteria='" + LinkName + "' and collegecode='" + clgcod + "' and MasterValue='" + monthdate + "') update CO_MasterValues set MasterCriteria1='" + date + "',MasterCriteriaValue1='" + ChallanNo + "'  where MasterCriteria='" + LinkName + "' and collegecode='" + clgcod + "' and MasterValue='" + monthdate + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode,MasterCriteria1,MasterCriteriaValue1) values('" + monthdate + "','" + LinkName + "','" + clgcod + "','" + date + "','" + ChallanNo + "')";
                    string query =string.Empty;
                    if (LinkName == "Quarterly Report DepositDate")
                    {
                        query = "if exists (select MasterValue from CO_MasterValues where MasterCriteria='" + LinkName + "' and collegecode='" + clgcod + "' and MasterValue='" + monthyear1 + "') update CO_MasterValues set MasterCriteria1='" + date + "',MasterCriteriaValue1='" + ChallanNo + "',MasterCriteriaValue2='" + monthdate + "'  where MasterCriteria='" + LinkName + "' and collegecode='" + clgcod + "' and MasterValue='" + monthyear1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode,MasterCriteria1,MasterCriteriaValue1,MasterCriteriaValue2) values('" + monthyear1 + "','" + LinkName + "','" + clgcod + "','" + date + "','" + ChallanNo + "','" + monthdate + "')";
                    }
                    else
                    {
                         query = "if exists (select MasterValue from CO_MasterValues where MasterCriteria='" + LinkName + "' and collegecode='" + clgcod + "' and MasterValue='" + monthdate + "') update CO_MasterValues set MasterCriteria1='" + date + "',MasterCriteriaValue1='" + ChallanNo + "'  where MasterCriteria='" + LinkName + "' and collegecode='" + clgcod + "' and MasterValue='" + monthdate + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode,MasterCriteria1,MasterCriteriaValue1) values('" + monthdate + "','" + LinkName + "','" + clgcod + "','" + date + "','" + ChallanNo + "')";
                    
                    }

                    int val = d2.update_method_wo_parameter(query, "text");
                }
                alertmessage.Visible = true;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Saved Successfully";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }
    protected void linkbtn_Click(object sender, EventArgs e)
    {
        ViewState["DateType"] = "1";
        popupwindow.Visible = true;
        btnsave.Visible = false;
        griddate.Visible = false;
    }
    protected void lnkDepositClick(object sender, EventArgs e)
    {
        ViewState["DateType"] = "2";
        popupwindow.Visible = true;
        btnsave.Visible = false;
        griddate.Visible = false;
    }
}