using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class HRMOD_ITCalCulationSettings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    static string collegecode = string.Empty;
    string collegecode1 = string.Empty;
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
            //collegecode = Convert.ToString(Session["collegecode"]);
            usercode = Session["usercode"].ToString();
            //collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                binddept();
                designation();
                category();
                stafftype();
                staffstatus();
                txtchqdt.Attributes.Add("readonly", "readonly");
                txtdocdate.Attributes.Add("readonly", "readonly");
                txtchqdt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txtdocdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    #region BindMethods
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
    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string item = "select distinct dept_code,dept_name from hrdept_master where college_code = '" + collegecode1 + "'";
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
        string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + collegecode1 + "'";
        ds = da.select_method_wo_parameter(statequery, "Text");
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
        string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + collegecode1 + "' ";
        ds = da.select_method_wo_parameter(statequery, "Text");
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
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collegecode1 + "'";
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
    public void BindIncomeHeade()
    {
        try
        {
            string Query = "select IT_ID,ITAllowDeductName from IT_OtherAllowanceDeducation where ITType='1' and collegeCode='" + collegecode1 + "'";
            ds.Clear();
            ddlIncomeHead.Items.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlIncomeHead.DataSource = ds;
                ddlIncomeHead.DataTextField = "ITAllowDeductName";
                ddlIncomeHead.DataValueField = "IT_ID";
                ddlIncomeHead.DataBind();
            }
        }
        catch
        {

        }
    }
    public void BindDeductionHead()
    {
        try
        {
            string Query = "select IT_ID,ITAllowDeductName from IT_OtherAllowanceDeducation where ITType='2' and collegeCode='" + collegecode1 + "'";
            ds.Clear();
            ddlIncomeHead.Items.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlIncomeHead.DataSource = ds;
                ddlIncomeHead.DataTextField = "ITAllowDeductName";
                ddlIncomeHead.DataValueField = "IT_ID";
                ddlIncomeHead.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindYear()
    {
        try
        {
            ddlYear.Items.Clear();
            ddlAccYear.Items.Clear();
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy")) + 1;
            for (int intY = Year; intY >= Year - 15; intY--)
            {
                ddlYear.Items.Add(Convert.ToString(intY));
                ddlAccYear.Items.Add(Convert.ToString(intY));
            }
        }
        catch
        {

        }

    }
    public void bindMonth()
    {
        try
        {
            ddlMonth.Items.Clear();
            ddlAccMonth.Items.Clear();
            int Year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            for (int i = 1; i <= 12; i++)
            {
                ddlMonth.Items.Add(new ListItem(System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i), i.ToString()));
                ddlAccMonth.Items.Add(new ListItem(System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i), i.ToString()));
            }

        }
        catch
        {

        }
    }
    #endregion

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

    protected void cb_stype_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_stype, cbl_stype, txt_stype, "StaffType");
    }

    protected void cbl_stype_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stype, cbl_stype, txt_stype, "StaffType");
    }

    protected void cb_stat_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_stat, cbl_stat, txt_stat, "Status");
    }

    protected void cbl_stat_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stat, cbl_stat, txt_stat, "Status");
    }
    protected void ddlcollege_change(object sender, EventArgs e)
    {
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        binddept();
        designation();
        category();
        stafftype();
        staffstatus();

        FpSpread.Visible = false;
        lbl_alert.Visible = false;
        rprint.Visible = false;
        //sp_div.Visible = false;
    }
    protected void txt_scode_Change(object sender, EventArgs e)
    {
        txt_sname.Text = "";
    }

    protected void txt_sname_Change(object sender, EventArgs e)
    {
        txt_scode.Text = "";
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
    #region PrintAction
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
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Text = "Please Enter Your Report Name";
                lblsmserror.Visible = true;
            }
            btnprintmaster.Focus();
        }
        catch { }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "IT Calculation";
            string pagename = "IT_Calculation.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }
    #endregion
    #region AutoSearch StaffCode and StaffName
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null)) and staff_name like  '%" + prefixText + "%' and college_code='" + collegecode + "' order by staff_name";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null)) and staff_code like  '%" + prefixText + "%' and college_code='" + collegecode + "' order by staff_code";
        name = ws.Getname(query);
        return name;
    }
    #endregion
    #region Button Go Event
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {

            FpSpread.SaveChanges();
            lbl_alert.Visible = false;
            FpSpread.Sheets[0].RowCount = 0;
            FpSpread.Sheets[0].ColumnCount = 11;

            string selectquery = "";
            string scode = txt_scode.Text;
            string sname = txt_sname.Text;
            string dept = "";
            string desig = "";
            string category = "";
            string stype = "";
            string status = "";

            if (txt_scode.Text != "")
            {
                selectquery = "select t.staff_code,t.stfstatus,staff_name,dept_name,desig_name,stftype,category_name,bsalary,grade_pay,pay_band ,IsMPFAmt,MPFAmount,MPFPer from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c where t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and settled =0 and resign =0 and ISNULL(Discontinue,'0')='0' and m.college_code = '" + collegecode1 + "' and t.staff_code='" + scode + "'";
            }
            else if (txt_sname.Text != "")
            {
                selectquery = "select t.staff_code,t.stfstatus,staff_name,dept_name,desig_name,stftype,category_name,bsalary,grade_pay,pay_band ,IsMPFAmt,MPFAmount,MPFPer from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c where t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and settled =0 and resign =0 and ISNULL(Discontinue,'0')='0' and m.college_code = '" + collegecode1 + "' and staff_name='" + sname + "'";
            }
            else
            {
                dept = rs.GetSelectedItemsText(cbl_dept);

                desig = rs.GetSelectedItemsText(cbl_desig);

                category = rs.GetSelectedItemsText(cbl_staffc);

                stype = rs.GetSelectedItemsText(cbl_stype);

                status = rs.GetSelectedItemsText(cbl_stat);

                selectquery = "select t.staff_code,t.stfstatus,staff_name,dept_name,desig_name,stftype,category_name,bsalary,grade_pay,pay_band,IsMPFAmt,MPFAmount,MPFPer from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c where t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and settled =0 and resign =0 and ISNULL(Discontinue,'0')='0' and m.college_code = '" + collegecode1 + "' ";
                if (dept.Trim() != "")
                {
                    selectquery += " and h.dept_name in('" + dept + "')";
                }
                if (dept.Trim() != "")
                {
                    selectquery += " and g.desig_name in('" + desig + "')";
                }
                if (dept.Trim() != "")
                {
                    selectquery += " and c.category_name in('" + category + "') ";
                }
                if (dept.Trim() != "")
                {
                    selectquery += " and t.stftype in('" + stype + "') ";
                }
                if (dept.Trim() != "")
                {
                    selectquery += " and t.stfstatus in('" + status + "')";
                }
            }
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                sp_div.Visible = true;
                FpSpread.Visible = true;
                FpSpread.Sheets[0].RowCount = 0;
                FpSpread.Sheets[0].ColumnCount = 0;
                FpSpread.CommandBar.Visible = false;
                FpSpread.Sheets[0].AutoPostBack = false;
                FpSpread.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread.Sheets[0].RowHeader.Visible = false;
                FpSpread.Sheets[0].ColumnCount = 10;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread.Columns[0].Locked = true;
                FpSpread.Columns[0].Width = 50;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread.Columns[1].Width = 80;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = false;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread.Columns[2].Locked = true;
                FpSpread.Columns[2].Width = 75;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread.Columns[3].Locked = true;
                FpSpread.Columns[3].Width = 175;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread.Columns[4].Locked = true;
                FpSpread.Columns[4].Width = 150;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Designation";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread.Columns[5].Locked = true;
                FpSpread.Columns[5].Width = 150;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Staff Category";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread.Columns[6].Locked = true;
                FpSpread.Columns[6].Width = 125;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Basic Pay";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpread.Columns[7].Locked = true;
                FpSpread.Columns[7].Width = 75;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Staff Status";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                FpSpread.Columns[8].Locked = true;
                FpSpread.Columns[8].Width = 100;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Salary";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                FpSpread.Columns[9].Locked = true;
                FpSpread.Columns[9].Width = 100;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkall;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                    lblStaffCode.Text = d2.GetFunction("select appl_id from staff_appl_Master sa,staffmaster m where sa.appl_no=m.appl_no and m.staff_code ='" + Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]) + "'");
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["category_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["bsalary"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["stfstatus"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["pay_band"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                }
                FpSpread.Visible = true;
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                FpSpread.SaveChanges();
                lbl_alert.Visible = false;
                rprint.Visible = true;
            }
            else
            {
                rprint.Visible = false;
                FpSpread.Visible = false;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Records Found!";
            }
        }
        catch { }
    }
    #endregion
    #region IncomeAdd Delete New Exit
    protected void btnSaveIncome_Click(object sender, EventArgs e)
    {
        try
        {
            string Amount = string.Empty;
            string StaffApplId = string.Empty;
            string AllowDeductID = string.Empty;
            string DocDate = string.Empty;
            string DocNo = string.Empty;
            string CheckDDDate = string.Empty;
            string checkDDNo = string.Empty;
            string BankCode = string.Empty;
            string ITMonth = string.Empty;
            string ITyear = string.Empty;
            string ITAccessYear = string.Empty;
            string ITAccessMonth = string.Empty;
            string Remarks = string.Empty;
            string CollegeCode = string.Empty;
            string StaffCode = string.Empty; ;
            string IncomeType = lblIncomeType.Text;
            string challanNo = string.Empty;
            bool SaveFlag = false;
            Amount = txtamount.Text;
            if (ddlIncomeHead.Enabled == true)
            {
                AllowDeductID = ddlIncomeHead.SelectedValue;
            }
            DocDate = txtdocdate.Text;
            string[] SplitDate = DocDate.Split('/');
            DocDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
            DocNo = txtdocno.Text;
            CheckDDDate = txtchqdt.Text;
            SplitDate = CheckDDDate.Split('/');
            CheckDDDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
            checkDDNo = txtchqno.Text;
            BankCode = txtBankCode.Text;
            ITMonth = ddlMonth.SelectedValue;
            ITyear = ddlYear.SelectedItem.Text;
            ITAccessMonth = ddlAccMonth.SelectedValue;
            ITAccessYear = ddlAccYear.SelectedItem.Text;
            Remarks = txt_Remarks.Text;
            CollegeCode = ddlcollege.SelectedValue;
            challanNo = txtchallonNoTransferVoucher.Text;
            string CommonDuduction = Convert.ToString(ddlotherAllowance.SelectedItem.Value);
            string gerpercentage = string.Empty;
            byte checkotherallowance = 0;
            if (txtpercent.Text == "")
            {
                gerpercentage = "";

            }
            else
            {
               gerpercentage= Convert.ToString(txtpercent.Text);
            }
            if (Cb_otherallowance.Checked == true)
            {
                checkotherallowance = 1;
                IncomeType = "4";
                //IncomeType

            }
            else
            {
                checkotherallowance = 0;
            
            }

            for (int i = 0; i < FpSpread.Sheets[0].Rows.Count; i++)
            {
                byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[i, 1].Value);
                if (check == 1)
                {
                    StaffCode = Convert.ToString(FpSpread.Sheets[0].Cells[i, 2].Text);
                    StaffCode = d2.GetFunction("select Appl_Id from staff_appl_Master sa,StaffMaster s where s.appl_no=sa.appl_no and s.college_Code='" + CollegeCode.Trim() + "' and s.staff_code='" + StaffCode.Trim() + "'");
                    if (Amount.Trim() != "" && StaffCode.Trim() != "" && StaffCode.Trim() != "0" && IncomeType.Trim() != "3" && IncomeType.Trim() != "5")//&& Amount.Trim() != "0" barath 02.01.18
                    {//added percentage column delsi 2209
                        string UpdateQuery = "if exists (select IT_StaffIdentity from IT_Staff_AllowanceDeduction_Details where Staff_ApplID='" + StaffCode + "' and AllowDeductID='" + AllowDeductID + "' and collegeCode ='" + CollegeCode + "' and ITAllowdeductType='" + IncomeType + "' and ITMonth='" + ITMonth + "' and ITYear='" + ITyear + "') update IT_Staff_AllowanceDeduction_Details set Amount='" + Amount + "',DocDate='" + DocDate + "',DocNo='" + DocNo + "',CheckDDDate='" + CheckDDDate + "',CheckDDNo='" + checkDDNo + "',BankCode='" + BankCode + "',ITMonth='" + ITMonth + "',ITYear='" + ITyear + "',ITAccessMonth='" + ITAccessMonth + "',ITAccesYear='" + ITAccessYear + "',Remarks='" + Remarks + "' ,ChallanNo='" + challanNo + "',CommonDuduction='" + CommonDuduction + "',percentage='" + gerpercentage + "',checkotherallow='" + checkotherallowance + "' where  Staff_ApplID='" + StaffCode + "' and AllowDeductID='" + AllowDeductID + "' and collegeCode ='" + CollegeCode + "' and ITAllowdeductType='" + IncomeType + "' and ITMonth='" + ITMonth + "' and ITYear='" + ITyear + "' else insert into IT_Staff_AllowanceDeduction_Details (Staff_ApplID,AllowDeductID,Amount,DocDate,DocNo,CheckDDDate,CheckDDNo,BankCode,ITMonth,ITYear,ITAccessMonth,ITAccesYear,Remarks,CollegeCode,ITAllowdeductType,ChallanNo,CommonDuduction,percentage,checkotherallow) values ('" + StaffCode + "','" + AllowDeductID + "','" + Amount + "','" + DocDate + "','" + DocNo + "','" + CheckDDDate + "','" + checkDDNo + "','" + BankCode + "','" + ITMonth + "','" + ITyear + "','" + ITAccessMonth + "','" + ITAccessYear + "','" + Remarks + "','" + CollegeCode + "','" + IncomeType + "','" + challanNo + "','" + CommonDuduction + "','" + gerpercentage + "','" + checkotherallowance + "')";
                        int Inst = d2.update_method_wo_parameter(UpdateQuery, "Text");
                        if (Inst > 0)
                        {
                            SaveFlag = true;
                        }
                    }
                    else if (Amount.Trim() != "" && StaffCode.Trim() != "" && StaffCode.Trim() != "0" && IncomeType.Trim() == "3" ||  IncomeType.Trim() == "5")//&& Amount.Trim() != "0" barath 02.01.18
                    {
                        string UpdateQuery = "if exists (select IT_StaffIdentity from IT_Staff_AllowanceDeduction_Details where Staff_ApplID='" + StaffCode + "' and collegeCode ='" + CollegeCode + "' and ITAllowdeductType='" + IncomeType + "' and ITMonth='" + ITMonth + "' and ITYear='" + ITyear + "') update IT_Staff_AllowanceDeduction_Details set Amount='" + Amount + "',DocDate='" + DocDate + "',DocNo='" + DocNo + "',CheckDDDate='" + CheckDDDate + "',CheckDDNo='" + checkDDNo + "',BankCode='" + BankCode + "',ITMonth='" + ITMonth + "',ITYear='" + ITyear + "',ITAccessMonth='" + ITAccessMonth + "',ITAccesYear='" + ITAccessYear + "',Remarks='" + Remarks + "',ChallanNo='" + challanNo + "',CommonDuduction='" + CommonDuduction + "' where  Staff_ApplID='" + StaffCode + "' and collegeCode ='" + CollegeCode + "' and ITAllowdeductType='" + IncomeType + "' and ITMonth='" + ITMonth + "' and ITYear='" + ITyear + "' else insert into IT_Staff_AllowanceDeduction_Details (Staff_ApplID,Amount,DocDate,DocNo,CheckDDDate,CheckDDNo,BankCode,ITMonth,ITYear,ITAccessMonth,ITAccesYear,Remarks,CollegeCode,ITAllowdeductType,ChallanNo,CommonDuduction) values ('" + StaffCode + "','" + Amount + "','" + DocDate + "','" + DocNo + "','" + CheckDDDate + "','" + checkDDNo + "','" + BankCode + "','" + ITMonth + "','" + ITyear + "','" + ITAccessMonth + "','" + ITAccessYear + "','" + Remarks + "','" + CollegeCode + "','" + IncomeType + "','" + challanNo + "','" + CommonDuduction + "')";
                        int Inst = d2.update_method_wo_parameter(UpdateQuery, "Text");
                        if (Inst > 0)
                        {
                            SaveFlag = true;
                        }
                    }
                }
            }
            if (SaveFlag)
            {
                Clear();
                BindGridValue();
                lbl_allowalert.Text = "Saved Successfully";
                lbl_allowalert.Visible = true;
            }
            else
            {
                lbl_allowalert.Text = "Please Entre Income Amount";
                lbl_allowalert.Visible = true;
            }
        }
        catch
        {

        }
    }
    protected void btnNewIncome_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
        }
        catch
        {

        }
    }
    public void Clear()
    {
        try
        {
            if (lblIncomeType.Text.Trim() == "1")
            {
                BindIncomeHeade();
            }
            else if (lblIncomeType.Text.Trim() == "2")
            {
                BindDeductionHead();
            }
            txtamount.Text = "";
            txtdocno.Text = "";
            txtchqno.Text = "";
            txtBankCode.Text = "";
            ddlMonth.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ddlAccMonth.SelectedIndex = 0;
            ddlAccYear.SelectedIndex = 0;
            txt_Remarks.Text = "";
            txtchallonNoTransferVoucher.Text = "";
            lbl_allowalert.Visible = false;
            txtchqdt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txtdocdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
        }
        catch
        {

        }
    }
    protected void btnDeleteIncome_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            bool checkflag = false;
            for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
            {
                byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (check == 1)
                {
                    string IdColumn = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                    if (IdColumn.Trim() != "")
                    {
                        string Query = "delete from IT_Staff_AllowanceDeduction_Details where IT_Staffidentity='" + IdColumn + "' and collegeCode ='" + collegecode1 + "'";
                        int upd = d2.update_method_wo_parameter(Query, "Text");
                        if (upd > 0)
                        {
                            checkflag = true;
                        }
                    }
                }
            }
            if (checkflag)
            {
                BindGridValue();
                lbl_allowalert.Text = "Deleted Successfully";
                lbl_allowalert.Visible = true;
            }
        }
        catch
        {

        }
    }
    protected void btnExitIncome_Click(object sender, EventArgs e)
    {
        try
        {
            DivAddIncomeHead.Visible = false;
        }
        catch
        {

        }
    }
    public void BindGridValue()
    {
        try
        {
            string Query = string.Empty;
            string HeadText = string.Empty;
            if (lblIncomeType.Text == "1")
            {
                HeadText = "Income Head";
                Query = " select IT_StaffIdentity,ItAllowdeductType,Amount,convert(varchar(10),DocDate,103) as DocDate,convert(varchar(10),CheckDDDate,103) as CheckDDDate,DocNo,CheckDDNo,BankCode,ITMonth,ITYear,ITAccessMonth,ITAccesYear,ITAllowDeductName,I.AllowDeductID from IT_Staff_AllowanceDeduction_Details I,IT_OtherAllowanceDeducation IT where I.AllowDeductID=IT.IT_ID and staff_applid ='" + lblStaffCode.Text + "' and IT.CollegeCode ='" + collegecode1 + "' and ITAllowDeductType in('1','4') order by I.AllowDeductID ";
            }
            else if (lblIncomeType.Text == "2")
            {
                HeadText = "Deduction Head";
                Query = " select IT_StaffIdentity,ItAllowdeductType,Amount,convert(varchar(10),DocDate,103) as DocDate,convert(varchar(10),CheckDDDate,103) as CheckDDDate,DocNo,CheckDDNo,BankCode,ITMonth,ITYear,ITAccessMonth,ITAccesYear,ITAllowDeductName,I.AllowDeductID from IT_Staff_AllowanceDeduction_Details I,IT_OtherAllowanceDeducation IT where I.AllowDeductID=IT.IT_ID and staff_applid ='" + lblStaffCode.Text + "' and IT.CollegeCode ='" + collegecode1 + "' and ITAllowDeductType='2' order by I.AllowDeductID";
            }
            else if (lblIncomeType.Text == "3")
            {
                HeadText = "Deduction Head";
                Query = " select IT_StaffIdentity,ItAllowdeductType,Amount,convert(varchar(10),DocDate,103) as DocDate,convert(varchar(10),CheckDDDate,103) as CheckDDDate,DocNo,CheckDDNo,BankCode,ITMonth,ITYear,ITAccessMonth,ITAccesYear, 'House Rent Amount' ITAllowDeductName,AllowDeductID from IT_Staff_AllowanceDeduction_Details where staff_applid ='" + lblStaffCode.Text + "' and CollegeCode ='" + collegecode1 + "' and ITAllowDeductType='3' order by AllowDeductID";
            }

            else if (lblIncomeType.Text == "5")
            {
                HeadText = "Deduction Head";

               Query = " select IT_StaffIdentity,ItAllowdeductType,Amount,convert(varchar(10),DocDate,103) as DocDate,convert(varchar(10),CheckDDDate,103) as CheckDDDate,DocNo,CheckDDNo,BankCode,ITMonth,ITYear,ITAccessMonth,ITAccesYear, 'Reinvestment' ITAllowDeductName,AllowDeductID from IT_Staff_AllowanceDeduction_Details where staff_applid ='" + lblStaffCode.Text + "' and CollegeCode ='" + collegecode1 + "' and ITAllowDeductType='5' order by AllowDeductID";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 11;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[0].Locked = true;
                FpSpread1.Columns[0].Width = 50;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[1].Width = 80;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = false;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = HeadText.ToString();
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[2].Locked = true;
                FpSpread1.Columns[2].Width = 200;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "IT Month & Year";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[3].Locked = true;
                FpSpread1.Columns[3].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Access Month & Year";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[4].Locked = true;
                FpSpread1.Columns[4].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Doc Date";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[5].Locked = true;
                FpSpread1.Columns[5].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Amount";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[6].Locked = true;
                FpSpread1.Columns[6].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Doc No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[7].Locked = true;
                FpSpread1.Columns[7].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Cheque / DD Date";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[8].Locked = true;
                FpSpread1.Columns[8].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Cheque / DD No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[9].Locked = true;
                FpSpread1.Columns[9].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Bank Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[9].Locked = true;
                FpSpread1.Columns[9].Width = 100;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkall;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ITAllowDeductName"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["IT_StaffIdentity"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ITMonth"]) + "/" + Convert.ToString(ds.Tables[0].Rows[i]["ITYear"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ITAccessMonth"]) + "/" + Convert.ToString(ds.Tables[0].Rows[i]["ITAccesYear"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["DocDate"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Amount"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["DocNo"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["CheckDDDate"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["CheckDDNo"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["BankCode"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                }
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
                FpSpread1.Height = 300;
                FpSpread1.Width = 850;
                lbl_alert.Visible = false;
            }
            else
            {
                FpSpread1.Visible = false;
            }

        }
        catch
        {

        }
    }
    #endregion
    #region BtnAddIncome Details
    protected void btnIncomeHead_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkedOK())
            {
                Cb_otherallowance.Visible = true;
                 txtpercent.Text = "";
                txtpercent.Enabled = false;
                cbincludedpercentage.Checked = false;
                DivAddIncomeHead.Visible = true;
                ddlIncomeHead.Enabled = true;
                BindIncomeHeade();
                bindMonth();
                bindYear();
                // lblStaffCode.Text = "341";
                lblHead.Text = "Income Head";
                lblIncomeType.Text = "1";
                lbl_addincome.Text = "Other Income Head";
                lbl_allowalert.Visible = false;
                Clear();
                BindGridValue();
            }
            else
            {

            }
        }
        catch
        {

        }
    }
    #endregion
    #region BtnAddDeduction Details
    protected void btnDeductionHead_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkedOK())
            {
                Cb_otherallowance.Visible = false;
                txtpercent.Text = "";
                txtpercent.Enabled = false;
                cbincludedpercentage.Checked = false;
                DivAddIncomeHead.Visible = true;
                ddlIncomeHead.Enabled = true;
                BindDeductionHead();
                bindMonth();
                bindYear();
                //lblStaffCode.Text = "341";
                lblHead.Text = "Deduction Head";
                lblIncomeType.Text = "2";
                lbl_addincome.Text = "Other Deduction Head";
                lbl_allowalert.Visible = false;
                Clear();
                BindGridValue();
            }
            else
            {

            }
        }
        catch
        {

        }
    }
    #endregion
    #region ClosePopUp
    protected void imgyear_Click(object sender, EventArgs e)
    {
        try
        {
            DivAddIncomeHead.Visible = false;
        }
        catch
        {

        }
    }
    #endregion

    protected void btnHouseRentpaid_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkedOK())
            {
                Cb_otherallowance.Visible = false;
                txtpercent.Text = "";
                txtpercent.Enabled = false;
                cbincludedpercentage.Checked = false;
                DivAddIncomeHead.Visible = true;
                //BindDeductionHead();
                ddlIncomeHead.Enabled = false;
                bindMonth();
                bindYear();
                //lblStaffCode.Text = "341";
                lblIncomeType.Text = "3";
                lbl_addincome.Text = "House Rent Amount";
                lbl_allowalert.Visible = false;
                Clear();
                BindGridValue();
            }
            else
            {

            }
        }
        catch
        {

        }
    }
    public bool checkedOK()
    {
        bool Ok = false;
        FpSpread.SaveChanges();
        for (int i = 0; i < FpSpread.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                Ok = true;
            }
        }
        return Ok;
    }
    void load_allowance()
    {
        ddlotherAllowance.Items.Clear();
        //cblOtherallowance.Items.Clear();
        ds.Clear();
        string Query = "Select * from incentives_master where college_code=" + ddlcollege.SelectedValue + "";
        ds = d2.select_method_wo_parameter(Query, "Text");
        string allowanmce = "";
        string detection = "";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            allowanmce = ds.Tables[0].Rows[0]["allowances"].ToString();
            detection = ds.Tables[0].Rows[0]["deductions"].ToString();
        }
        //if (rb_allow.Checked)
        //{
        //    string[] allowanmce_arr;
        //    allowanmce_arr = allowanmce.Split(';');

        //    for (int i = 0; i <= allowanmce_arr.GetUpperBound(0); i++)
        //    {
        //        string all2 = allowanmce_arr[i];
        //        string[] splitallo3 = all2.Split('\\');
        //        if (splitallo3.GetUpperBound(0) > 1)
        //        {
        //            //all2 = splitallo3[2];
        //            all2 = splitallo3[0];
        //        }
        //        if (all2.Trim() != "")
        //        {
        //            // ddlotherAllowance.Items.Add(all2);
        //            cblOtherallowance.Items.Add(all2);
        //        }
        //    }
        //}
        //if (rb_deduct.Checked)
        //{
        string[] detection_arr;
        detection_arr = detection.Split(';');
        ddlotherAllowance.Items.Insert(0, "select");
        for (int j = 0; j <= detection_arr.GetUpperBound(0); j++)
        {
            string all2 = detection_arr[j];
            string[] splitallo3 = all2.Split('\\');
            if (splitallo3.GetUpperBound(0) > 0)
            {
                //all2 = splitallo3[1];
                all2 = splitallo3[0];
                if (all2.Trim() != "")
                {
                    ddlotherAllowance.Items.Add(all2);
                    //cblOtherallowance.Items.Add(all2);
                }
            }
        }
        // }
    }
    public void rb_allow_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_splallow.Checked)
        {
            ddlotherAllowance.Enabled = true;
            load_allowance();
        }
        else
        {
            ddlotherAllowance.Enabled = false;
        }
    }
    public void cbincludedpercentage_CheckedChanged(object sender, EventArgs e)
    {
        if (cbincludedpercentage.Checked == true)
        {
            txtpercent.Enabled = true;
            txtpercent.Text =Convert.ToString( 100);
            lbl_allowalert.Visible = false;
            lbl_allowalert.Text = "";

        }
        else if (cbincludedpercentage.Checked == false)
        {
            txtpercent.Enabled = false;
            txtpercent.Text = "";
            lbl_allowalert.Visible = false;
            lbl_allowalert.Text = "";
        }
    }
    public void txt_change(object sender, EventArgs e)
    {
        if (cbincludedpercentage.Checked == true)
        {
            int getTxtval = Convert.ToInt32(txtpercent.Text);
            if (getTxtval > 100 || getTxtval == 0 || getTxtval < 0)
            {
                txtpercent.Text = "";
                lbl_allowalert.Visible = true;
                lbl_allowalert.Text = "Please Enter Values in Percentage";

            }
            else
            {
                lbl_allowalert.Visible = true;
                lbl_allowalert.Text = "";
            }
        }
    
    }
    protected void btnReinvestment_Click(object sender, EventArgs e)
    {
        if (checkedOK())
        {
            Cb_otherallowance.Visible = false;
            txtpercent.Text = "";
            txtpercent.Enabled = false;
            cbincludedpercentage.Checked = false;
            DivAddIncomeHead.Visible = true;
            //BindDeductionHead();
            ddlIncomeHead.Enabled = false;
            bindMonth();
            bindYear();
            //lblStaffCode.Text = "341";
            lblIncomeType.Text = "5";
            lbl_addincome.Text = "Reinvestment Amount";
            lbl_allowalert.Visible = false;
            Clear();
            BindGridValue();

        }
    }
}