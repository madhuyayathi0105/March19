using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;
public partial class HRMOD_SalaryBill : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    static string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                collegecode = rs.GetSelectedItemsValueAsString(cblclg);
                binddept();
                designation();
                category();
                stafftype();
                staffstatus();
                bindyear();
                bindmonth();
            }
            lbl_alert.Visible = false;
            lblerror.Text = "";
        }
        catch (Exception ex) { }
    }
    #region Bind Methods
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
            cblclg.Items.Clear();
            cbclg.Checked = false;
            txtclg.Text = "--Select--";
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
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
                    txtclg.Text = lbl_college.Text + "(" + cblclg.Items.Count + ")";
                }
            }
        }
        catch (Exception e) { }
    }
    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string item = "select distinct dept_code,dept_name from hrdept_master where college_code in( '" + collegecode + "')";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "dept_name";
                cbl_dept.DataValueField = "dept_code";
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
        catch { }
    }
    protected void designation()
    {
        ds.Clear();
        cbl_desig.Items.Clear();
        string statequery = "select distinct desig_code,desig_name from desig_master where collegeCode in( '" + collegecode + "')";
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
    protected void category()
    {
        ds.Clear();
        cbl_staffc.Items.Clear();
        string statequery = "select distinct category_code,category_Name from staffcategorizer where college_code in( '" + collegecode + "')";
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
    protected void stafftype()
    {
        try
        {
            ds.Clear();
            cbl_stype.Items.Clear();
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code in( '" + collegecode + "')";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stype.DataSource = ds;
                cbl_stype.DataTextField = "stftype";
                cbl_stype.DataBind();
                if (cbl_stype.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stype.Items.Count; i++)
                    {
                        cbl_stype.Items[i].Selected = true;
                    }
                    txt_stype.Text = "StaffType (" + cbl_stype.Items.Count + ")";
                    cb_stype.Checked = true;
                }
            }
            else
            {
                txt_stype.Text = "--Select--";
                cb_stype.Checked = false;
            }
        }
        catch { }
    }
    protected void staffstatus()
    {
        try
        {
            ds.Clear();
            cbl_stat.Items.Clear();
            string item = "select distinct stfstatus from stafftrans where stfstatus is not null and stfstatus<>''";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stat.DataSource = ds;
                cbl_stat.DataTextField = "stfstatus";
                cbl_stat.DataBind();
                if (cbl_stat.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stat.Items.Count; i++)
                    {
                        cbl_stat.Items[i].Selected = true;
                    }
                    txt_stat.Text = "Status (" + cbl_stat.Items.Count + ")";
                    cb_stat.Checked = true;
                }
            }
            else
            {
                txt_stat.Text = "--Select--";
                cb_stat.Checked = false;
            }
        }
        catch { }
    }
    protected void bindyear()
    {
        int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
        for (int l = 0; l < 15; l++)
        {
            ddl_year.Items.Add(Convert.ToString(year));
            year--;
        }
    }
    protected void bindmonth()
    {
        DateTime dt = new DateTime(2000, 1, 1);
        for (int m = 0; m < 12; m++)
        {
            ddl_month.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
        }
    }
    #endregion
    #region Checkbox events
    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_dept, cb_dept, txt_dept, "Department");
    }
    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_dept, cb_dept, txt_dept, "Department");
    }
    protected void cb_desig_CheckedChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_desig, cb_desig, txt_desig, "Designation");
    }
    protected void cbl_desig_SelectedIndexChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_desig, cb_desig, txt_desig, "Designation");
    }
    protected void cb_staffc_CheckedChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_staffc, cb_staffc, txt_staffc, "Category");
    }
    protected void cbl_staffc_SelectedIndexChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_staffc, cb_staffc, txt_staffc, "Category");
    }
    protected void cb_stype_CheckedChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_stype, cb_stype, txt_stype, "StaffType");
    }
    protected void cbl_stype_SelectedIndexChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_stype, cb_stype, txt_stype, "StaffType");
    }
    protected void cb_stat_CheckedChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_stat, cb_stat, txt_stat, "Status");
    }
    protected void cbl_stat_SelectedIndexChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_stat, cb_stat, txt_stat, "Status");
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblclg, cbclg, txtclg, lbl_college.Text);
        collegecode = rs.GetSelectedItemsValueAsString(cblclg);
        binddept();
        designation();
        category();
        stafftype();
        staffstatus();
        lbl_alert.Visible = false;
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cblclg, cbclg, txtclg, lbl_college.Text);
        collegecode = rs.GetSelectedItemsValueAsString(cblclg);
        binddept();
        designation();
        category();
        stafftype();
        staffstatus();
        lbl_alert.Visible = false;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where ( (Discontinue=0 or Discontinue is null)) and staff_name like  '" + prefixText + "%'  order by staff_name";//(resign =0 and settled =0) and and college_code='" + collegecode + "'
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where ( (Discontinue=0 or Discontinue is null)) and staff_code like  '" + prefixText + "%'  order by staff_code";//(resign =0 and settled =0) and and college_code='" + collegecode + "'
        name = ws.Getname(query);
        return name;
    }
    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                txtexcel.Text = "";
                d2.printexcelreport(FpSpread, reportname);
                lblerror.Visible = false;
            }
            else
            {
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
            btnprintmaster.Focus();
        }
        catch { }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Newly Joined & Relieved Staff Details";
            string pagename = "salarybill.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }
    protected void btnExcel_Click1(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel1.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(DeductionDetSp, reportname);
                Label3.Visible = false;
            }
            else
            {
                Label3.Text = "Please Enter Your Report Name";
                Label3.Visible = true;
                txtexcel1.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click1(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = txtexcel1.Text;
            string pagename = "SalaryBill.aspx";
            Printcontrol.loadspreaddetails(DeductionDetSp, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    #endregion
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string departmentcode = rs.GetSelectedItemsValueAsString(cbl_dept);
            string designationcode = rs.GetSelectedItemsValueAsString(cbl_desig);
            string category = rs.GetSelectedItemsValueAsString(cbl_staffc);
            string stafftype = rs.GetSelectedItemsValueAsString(cbl_stype);
            string collegecode = rs.GetSelectedItemsValueAsString(cblclg);
            string JoiningDateF = string.Empty;
            string JoiningDateT = string.Empty;
            DateTime JoiningDateFDt = new DateTime();
            DateTime JoiningDateTDt = new DateTime();
            DateTime.TryParse(ddl_month.SelectedItem.Value + "/01/" + ddl_year.SelectedItem.Text, out JoiningDateFDt);
            int totaldays = DateTime.DaysInMonth(Convert.ToInt32(ddl_year.SelectedItem.Value), Convert.ToInt32(ddl_month.SelectedItem.Value));
            DateTime.TryParse(ddl_month.SelectedItem.Value + "/" + totaldays + "/" + ddl_year.SelectedItem.Text, out JoiningDateTDt);
            JoiningDateF = Convert.ToString(JoiningDateFDt.ToString("MM/dd/yyyy"));
            JoiningDateT = Convert.ToString(JoiningDateTDt.ToString("MM/dd/yyyy"));
            string prevMonth = string.Empty;
            string prevYear = string.Empty;
            if (ddl_month.SelectedIndex - 1 != -1)
            {
                prevMonth = Convert.ToString(ddl_month.Items[ddl_month.SelectedIndex - 1].Value);
                prevYear = Convert.ToString(ddl_year.SelectedValue);
            }
            else
            {
                prevMonth = Convert.ToString(ddl_month.Items[11].Value);
                prevYear = Convert.ToString(Convert.ToInt32(ddl_year.SelectedValue) - 1);
            }
            string staffnameQ = string.Empty;
            string staffcodeQ = string.Empty;
            string Catagory = string.Empty;
            double value = 0;
            double val = 0;
            if (txt_sname.Text.Trim() != "")
                staffnameQ = " and m.staff_name='" + txt_sname.Text.Trim() + "'";
            if (txt_scode.Text.Trim() != "")
                staffcodeQ = " and m.staff_code='" + txt_scode.Text.Trim() + "'";
            string Qry = "select sum(netaddact)netaddact,count(t.staff_code)staffCount, p.category_code,p.PayMonth,p.PayYear,CONVERT(varchar(10),m.join_date,103) join_date,m.join_date as join_dateD,m.relieve_date,ISNULL(m.settled,0)settled,ISNULL(m.resign,0)resign from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c,monthlypay p where p.staff_code=t.staff_code and p.staff_code=m.staff_code and t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1  and ISNULL(Discontinue,'0')='0' and ISNULL(PayYear,-1)<>-1 and ISNULL(PayMonth,-1)<>-1 and m.college_code in( '" + collegecode + "') and h.dept_code in('" + departmentcode + "') and g.desig_code in('" + designationcode + "') and c.category_code in('" + category + "') and t.stftype in('" + stafftype + "') and ((PayMonth >= '" + prevMonth + "' and PayYear = '" + prevYear + "') or (PayMonth <='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Text + "' )) " + staffnameQ + "" + staffcodeQ + " group by p.category_code ,p.PayMonth,p.PayYear ,join_date,m.settled,m.resign,m.relieve_date";
            ds = d2.select_method_wo_parameter(Qry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string CurrentMonthYear = Convert.ToString(ddl_month.SelectedItem.Text + "  " + ddl_year.SelectedItem.Text);
                string previousMonthyear = string.Empty;
                if (ddl_month.SelectedIndex - 1 != -1)
                    previousMonthyear = Convert.ToString(ddl_month.Items[ddl_month.SelectedIndex - 1].Text) + " " + ddl_year.SelectedItem.Text;
                else
                    previousMonthyear = Convert.ToString(ddl_month.Items[11].Text) + " " + Convert.ToString(Convert.ToInt32(ddl_year.SelectedValue) - 1);
                #region SalaryBill
                string headername = "S.No/Particulars";
                rs.Fpreadheaderbindmethod(headername, FpSpread, "true");
                bool colfirstEntry = false;
                FpSpread.Sheets[0].ColumnCount++;
                if (cbl_staffc.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staffc.Items.Count; i++)
                    {
                        if (cbl_staffc.Items[i].Selected == true)
                        {
                            if (colfirstEntry)
                            {
                                FpSpread.Sheets[0].ColumnCount++;
                            }
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_staffc.Items[i].Text);
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_staffc.Items[i].Value);
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            colfirstEntry = true;
                        }
                    }
                    FpSpread.Sheets[0].ColumnCount++;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Total";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Tag = "T";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread.Sheets[0].RowCount);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Salary Earned (Actual)";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    string categoryF = string.Empty;
                    for (int i = 2; i < FpSpread.Sheets[0].ColumnCount; i++)
                    {
                        Catagory = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                        if (Catagory != "T")
                            categoryF = " and category_code='" + Catagory + "'";
                        else
                            categoryF = "";
                        double categorywiseSalary = 0;
                        double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([NetAddAct])", "PayMonth ='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Text + "'" + categoryF)), out categorywiseSalary);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, i].Text = Convert.ToString(categorywiseSalary);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                    }
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Deductions";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    DataTable deductionDt = new DataTable();
                    deductionDetails(collegecode, ref deductionDt);
                    if (deductionDt.Rows.Count > 0)
                    {
                        int k = 0;
                        Hashtable TotalHash = new Hashtable();
                        foreach (DataColumn DeductionColumnName in deductionDt.Columns)
                        {
                            if (Convert.ToString(DeductionColumnName) != "category_code")
                            {
                                k++;
                                FpSpread.Sheets[0].RowCount++;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(DeductionColumnName);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                for (int i = 2; i < FpSpread.Sheets[0].ColumnCount; i++)
                                {
                                    Catagory = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                                    if (Catagory != "T")
                                        categoryF = "category_code='" + Catagory + "'";
                                    else
                                        categoryF = "";
                                    double.TryParse(Convert.ToString(deductionDt.Compute("Sum([" + DeductionColumnName + "])", categoryF)), out value);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, i].Text = Convert.ToString(value);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Right;
                                    if (!TotalHash.Contains(Catagory))
                                        TotalHash.Add(Catagory, value);
                                    else
                                    {
                                        val = 0;
                                        double.TryParse(Convert.ToString(TotalHash[Catagory]), out val);
                                        TotalHash.Remove(Catagory);
                                        TotalHash.Add(Catagory, val + value);
                                    }
                                }
                            }
                        }
                        FpSpread.Sheets[0].RowCount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Total Deductions";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        FpSpread.Sheets[0].RowCount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Net Salary";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        for (int i = 2; i < FpSpread.Sheets[0].ColumnCount; i++)
                        {
                            Catagory = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                            double.TryParse(Convert.ToString(TotalHash[Catagory]), out val);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 2, i].Text = Convert.ToString(val);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 2, i].HorizontalAlign = HorizontalAlign.Right;
                            if (Catagory != "T")
                                categoryF = " and category_code='" + Catagory + "'";
                            else
                                categoryF = "";
                            double categorywiseSalary = 0;
                            double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([NetAddAct])", "PayMonth ='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Text + "'" + categoryF)), out categorywiseSalary);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, i].Text = Convert.ToString(categorywiseSalary - val);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, i].ForeColor = Color.Maroon;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, i].Font.Bold = true;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 2, i].ForeColor = Color.Maroon;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 2, i].Font.Bold = true;
                        }
                    }
                }
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                FpSpread.Height = FpSpread.Sheets[0].RowCount * 25 + 200;
                sp_div.Visible = true;
                txt_sname.Text = "";
                txt_scode.Text = "";
                #endregion
                #region Comparative Statement
                headername = "S.No/Details/Previous Month/Current Month/Difference";
                rs.Fpreadheaderbindmethod(headername, DeductionDetSp, "true");
                DeductionDetSp.Sheets[0].RowCount++;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(DeductionDetSp.Sheets[0].RowCount);
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Total Salary";
                double prevSalary = 0;
                double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([NetAddAct])", "PayMonth ='" + prevMonth + "' and PayYear = '" + prevYear + "'")), out prevSalary);
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(prevSalary);
                double currentSalary = 0;
                double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([NetAddAct])", "PayMonth ='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Value + "'")), out currentSalary);
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(currentSalary);
                double salaryMax = prevSalary > currentSalary ? prevSalary : currentSalary;
                double salaryMin = prevSalary < currentSalary ? prevSalary : currentSalary;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(salaryMax - salaryMin);
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                DeductionDetSp.Sheets[0].RowCount++;
                for (int i = 0; i < cbl_staffc.Items.Count; i++)
                {
                    if (cbl_staffc.Items[i].Selected == true)
                    {
                        DeductionDetSp.Sheets[0].RowCount++;
                        double StaffCount = 0;
                        Catagory = Convert.ToString(cbl_staffc.Items[i].Value);
                        double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([staffCount])", " settled =0 and resign =0 and PayMonth ='" + prevMonth + "' and PayYear = '" + prevYear + "' and category_code='" + Catagory + "'")), out StaffCount);
                        double NewStaffCount = 0;
                        double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([staffCount])", " settled =0 and resign =0 and PayMonth ='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Text + "' and join_dateD >= '" + JoiningDateFDt + "'  and join_dateD <= '" + JoiningDateTDt + "'  and category_code='" + Catagory + "'")), out NewStaffCount);
                        double RelievedStaffCount = 0;
                        double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([staffCount])", " settled =1 and resign =1 and PayMonth ='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Text + "' and relieve_date >= '" + JoiningDateFDt + "'  and relieve_date <= '" + JoiningDateTDt + "' and category_code='" + Catagory + "'")), out RelievedStaffCount);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(Convert.ToString(cbl_staffc.Items[i].Text));

                        string staffCountDet = Convert.ToString(StaffCount) + " + " + Convert.ToString(NewStaffCount) + " - " + Convert.ToString(RelievedStaffCount);
                        double currentStaffCount = StaffCount + NewStaffCount - RelievedStaffCount;
                        double DStaffMax = StaffCount > currentStaffCount ? StaffCount : currentStaffCount;
                        double DStaffMin = StaffCount < currentStaffCount ? StaffCount : currentStaffCount;
                        double diffStaffCount = DStaffMax - DStaffMin;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = staffCountDet;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(currentStaffCount);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(diffStaffCount);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                    }
                }
                double NewJoiningCurrentSalary = 0;
                double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([NetAddAct])", " settled =0 and resign =0 and PayMonth ='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Text + "' and join_dateD >= '" + JoiningDateFDt + "'  and join_dateD <= '" + JoiningDateTDt + "'")), out NewJoiningCurrentSalary);
                double relievedCurrentSalary = 0;
                double relievedPrevSalary = 0;
                double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([NetAddAct])", " settled =1 and resign =1 and PayMonth ='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Text + "' and relieve_date >= '" + JoiningDateFDt + "'  and relieve_date <= '" + JoiningDateTDt + "'")), out relievedCurrentSalary);
                double.TryParse(Convert.ToString(ds.Tables[0].Compute("Sum([NetAddAct])", " settled =1 and resign =1 and PayMonth ='" + prevMonth + "' and PayYear = '" + prevYear + "' and relieve_date >= '" + JoiningDateFDt.AddMonths(-1) + "'  and relieve_date <= '" + JoiningDateTDt.AddMonths(-1) + "'")), out relievedPrevSalary);
                double relievedStaffDifference = relievedPrevSalary - relievedCurrentSalary;
                double DsalaryMax = NewJoiningCurrentSalary > relievedStaffDifference ? NewJoiningCurrentSalary : relievedStaffDifference;
                double DsalaryMin = NewJoiningCurrentSalary < relievedStaffDifference ? NewJoiningCurrentSalary : relievedStaffDifference;
                double difference = DsalaryMax - DsalaryMin;
                DeductionDetSp.Sheets[0].RowCount++;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Text = "Newly Joined";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(NewJoiningCurrentSalary);
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                DeductionDetSp.Sheets[0].RowCount++;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Text = "Relieved Staff";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(relievedStaffDifference);
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                DeductionDetSp.Sheets[0].RowCount++;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Text = "Difference";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].ForeColor = Color.Maroon;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(difference);
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].ForeColor = Color.Maroon;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                DeductionDetSp.Sheets[0].PageSize = DeductionDetSp.Sheets[0].RowCount;
                DeductionDetSp.Height = DeductionDetSp.Sheets[0].RowCount * 25 + 200;
                sp_div.Visible = true;
                Deduction.Visible = true;
                #endregion
            }
            else
            {
                Deduction.Visible = false;
                sp_div.Visible = false;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No records founds";
            }
        }
        catch (Exception ex)
        {
            Deduction.Visible = false;
            sp_div.Visible = false;
            lbl_alert.Visible = true;
            lbl_alert.Text = ex.ToString();
        }
    }
    protected bool deductionDetails(string collegecode, ref DataTable deductionDt)
    {
        bool success = false;
        #region Bind Columns Name
        if (cbl_staffc.Items.Count > 0)
        {
            deductionDt.Columns.Add("category_code", typeof(string));
            deductionDt.Columns.Add("LOP", typeof(double));
            string item = "select distinct convert(varchar(max),deductions)deductions from incentives_master where college_code in('" + collegecode + "')";
            DataSet CommondeductionDs = d2.select_method_wo_parameter(item, "Text");
            if (CommondeductionDs.Tables[0].Rows.Count > 0)
            {
                string st = Convert.ToString(CommondeductionDs.Tables[0].Rows[0]["deductions"]);
                string[] split = st.Split(';');
                for (int sel = 0; sel < split.Length; sel++)
                {
                    string staff = split[sel];
                    string[] split1 = staff.Split('\\');
                    string stafftype = split1[0];
                    if (stafftype.Trim() != "")
                    {
                        if (!deductionDt.Columns.Contains(stafftype))
                            deductionDt.Columns.Add(stafftype, typeof(double));
                    }
                }
            }
        }
        string prevMonth = string.Empty;
        string prevYear = string.Empty;
        string prevMonthText = string.Empty;
        if (ddl_month.SelectedIndex - 1 != -1)
        {
            prevMonth = Convert.ToString(ddl_month.Items[ddl_month.SelectedIndex - 1].Value);
            prevYear = Convert.ToString(ddl_year.SelectedValue);
            prevMonthText = Convert.ToString(ddl_month.Items[ddl_month.SelectedIndex - 1].Text);
        }
        else
        {
            prevMonthText = Convert.ToString(ddl_month.Items[11].Text);
            prevMonth = Convert.ToString(ddl_month.Items[11].Value);
            prevYear = Convert.ToString(Convert.ToInt32(ddl_year.SelectedValue) - 1);
        }
        string Qry = "select category_code,deductions,college_code,PayMonth,PayYear,Tot_lop from monthlypay where PayMonth ='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Text + "' and college_code in('" + collegecode + "') and convert(varchar(max), deductions)<>'' ";
        DataSet deductionDs = d2.select_method_wo_parameter(Qry, "text");
        if (deductionDs.Tables[0].Rows.Count > 0)
        {
            DataRow dr;
            foreach (DataRow Data in deductionDs.Tables[0].Rows)
            {
                string[] split_main = Convert.ToString(Data["deductions"]).Split('\\');
                if (split_main.Length > 0)
                {
                    dr = deductionDt.NewRow();
                    dr["LOP"] = Convert.ToString(Data["Tot_lop"]);
                    for (int count = 0; count <= split_main.GetUpperBound(0); count++)
                    {
                        string secondvlaue = Convert.ToString(split_main[count]);
                        if (!string.IsNullOrEmpty(secondvlaue.Trim()))
                        {
                            string[] second_split_value = secondvlaue.Split(';');
                            if (second_split_value.Length > 0)
                            {
                                string Columnname = Convert.ToString(second_split_value[0]);
                                string Columnvalue = Convert.ToString(second_split_value[3]);
                                if (deductionDt.Columns.Contains(Columnname))
                                {
                                    dr[Columnname] = Columnvalue;
                                    dr["category_code"] = Convert.ToString(Data["category_code"]);
                                }
                            }
                        }
                    }
                    deductionDt.Rows.Add(dr);
                }
            }
        }
        if (deductionDt.Rows.Count > 0)
            success = true;
        #endregion
        return success;
    }
}