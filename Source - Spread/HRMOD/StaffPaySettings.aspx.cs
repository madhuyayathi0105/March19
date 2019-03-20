using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class StaffPaySettings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    static string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    int i = 0;

    Hashtable hat = new Hashtable();

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
            clgcode = Convert.ToString(Session["collegecode"]);

            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                binddept();
                designation();
                category();
                stafftype();
            }
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            lblsmserror.Visible = false;
            lbl_alert.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        try
        {
            collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            binddept();
            designation();
            category();
            stafftype();
            lbl_alert.Visible = false;
            FpSpread.Visible = false;
            rprint.Visible = false;
            txt_scode.Text = "";
            txt_sname.Text = "";
        }
        catch { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like '" + prefixText + "%' and college_code='" + collegecode + "'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and staff_code like '" + prefixText + "%' and college_code='" + collegecode + "'";
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

    protected void btnexcel_Click(object sender, EventArgs e)
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

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "Staff Pay Settings";
        string pagename = "StaffPaySettings.aspx";
        Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
        Printcontrol.Visible = true;
        btnprintmaster.Focus();
    }

    protected void txt_scode_Change(object sender, EventArgs e)
    {
        txt_sname.Text = "";
    }

    protected void txt_sname_Change(object sender, EventArgs e)
    {
        txt_scode.Text = "";
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

    protected void cb_stype_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_stype, cbl_stype, txt_stype, "StaffType");
    }

    protected void cbl_stype_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stype, cbl_stype, txt_stype, "StaffType");
    }

    protected void chkallow_changed(object sender, EventArgs e)
    {
        chkchange(chkallow, chklstallow, txtallow, "Allowance");
    }

    protected void chklstallow_onselectedchanged(object sender, EventArgs e)
    {
        chklstchange(chkallow, chklstallow, txtallow, "Allowance");
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string selectquery = "";
            string scode = txt_scode.Text;
            string sname = txt_sname.Text;
            string dept = "";
            string desig = "";
            string category = "";
            string stype = "";

            dept = GetSelectedItemsText(cbl_dept);

            desig = GetSelectedItemsText(cbl_desig);

            category = GetSelectedItemsValueAsString(cbl_staffc);

            stype = GetSelectedItemsText(cbl_stype);

            if (txt_scode.Text != "")
            {
                selectquery = "select t.staff_code,t.stfstatus,staff_name,dept_name,desig_name,stftype,category_name,c.category_code,bsalary,basicpay,grade_pay,pay_band ,IsMPFAmt,MPFAmount,MPFPer,t.allowances,t.deductions,t.Gross_Sal from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c where t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and m.college_code = '" + collegecode1 + "' and t.staff_code='" + scode + "'";

            }
            else if (txt_sname.Text != "")
            {
                selectquery = "select t.staff_code,t.stfstatus,staff_name,dept_name,desig_name,stftype,category_name,c.category_code,bsalary,basicpay,grade_pay,pay_band ,IsMPFAmt,MPFAmount,MPFPer,t.allowances,t.deductions,t.Gross_Sal from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c where t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and m.college_code = '" + collegecode1 + "' and staff_name='" + sname + "'";

            }
            else
            {
                selectquery = "select t.staff_code,t.stfstatus,staff_name,dept_name,desig_name,stftype,category_name,c.category_code,bsalary,basicpay,grade_pay,pay_band,IsMPFAmt,MPFAmount,MPFPer,t.allowances,t.deductions,t.Gross_Sal from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c where t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and settled =0 and resign =0 and m.college_code = '" + collegecode1 + "' and h.dept_name in('" + dept + "') and g.desig_name in('" + desig + "') and c.category_code in('" + category + "') and t.stftype in('" + stype + "')";
            }
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int colcount = 0;
                sp_div.Visible = true;
                FpSpread.Visible = true;
                FpSpread.Sheets[0].RowCount = 0;
                FpSpread.Sheets[0].ColumnCount = 0;
                FpSpread.CommandBar.Visible = false;
                FpSpread.Sheets[0].AutoPostBack = false;
                FpSpread.Sheets[0].RowHeader.Visible = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "S.No";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 50;

                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Select";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 80;

                FarPoint.Web.Spread.CheckBoxCellType chkind = new FarPoint.Web.Spread.CheckBoxCellType();
                chkind.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = false;
                FarPoint.Web.Spread.DoubleCellType doublecell = new FarPoint.Web.Spread.DoubleCellType();
                doublecell.MaximumValue = 500000;
                doublecell.ErrorMessage = "Allow Only Numerics!";

                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Staff Code";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 75;

                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Staff Name";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 175;

                if (txt_dept.Text.Trim() != "--Select--" && dept.Trim() != "")
                {
                    FpSpread.Sheets[0].ColumnCount++;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Department";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 150;
                }

                if (txt_desig.Text.Trim() != "--Select--" && desig.Trim() != "")
                {
                    FpSpread.Sheets[0].ColumnCount++;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Designation";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 150;
                }

                if (txt_staffc.Text.Trim() != "--Select--" && category.Trim() != "")
                {
                    FpSpread.Sheets[0].ColumnCount++;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Staff Category";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 125;
                }

                if (txt_stype.Text.Trim() != "--Select--" && stype.Trim() != "")
                {
                    FpSpread.Sheets[0].ColumnCount++;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Staff Type";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 75;
                }

                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Gross Amount";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = false;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;

                FpSpread.SaveChanges();
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkind;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
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

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    colcount = 4;
                    if (dept.Trim() != "")
                    {
                        colcount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Name = "Book Antiqua";
                    }

                    if (desig.Trim() != "")
                    {
                        colcount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Name = "Book Antiqua";
                    }

                    if (category.Trim() != "")
                    {
                        colcount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["category_name"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["category_code"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Name = "Book Antiqua";
                    }

                    if (stype.Trim() != "")
                    {
                        colcount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["stftype"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Name = "Book Antiqua";
                    }

                    colcount++;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[i]["Gross_Sal"])))
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Gross_Sal"]);
                    else
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Text = "";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].CellType = doublecell;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, colcount - 1].Font.Name = "Book Antiqua";
                }
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                FpSpread.Sheets[0].FrozenRowCount = 1;
                rprint.Visible = true;
                lbl_alert.Visible = false;
            }
            else
            {
                FpSpread.Visible = false;
                rprint.Visible = false;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Records Found!";
            }
        }
        catch { }
    }

    protected void FpSpread_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            byte Check = Convert.ToByte(FpSpread.Sheets[0].Cells[0, 1].Value);
            if (Check == 1)
            {
                for (int chs = 1; chs < FpSpread.Sheets[0].RowCount; chs++)
                {
                    FpSpread.Sheets[0].Cells[chs, 1].Value = 1;
                }
            }
            else
            {
                for (int chs = 1; chs < FpSpread.Sheets[0].RowCount; chs++)
                {
                    FpSpread.Sheets[0].Cells[chs, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    protected void btnpopgo_click(object sender, EventArgs e)
    {
        try
        {
            loadspreadpop();
        }
        catch { }
    }

    protected void btn_setallow_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            if (checkedOK(FpSpread))
            {
                if (checkedstrOK(FpSpread))
                {
                    poperrjs.Visible = true;
                    allowance();
                    loadspreadpop();
                    lbl_alert.Visible = false;
                }
                else
                {
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Please Enter the Amount for Selected Staff!";
                }
            }
            else
            {
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select any one Staff! ";
            }
        }
        catch { }
    }

    public void loadspreadpop()
    {
        try
        {
            Fpspreadpop.Sheets[0].RowCount = 0;
            Fpspreadpop.Sheets[0].ColumnCount = 4;
            Fpspreadpop.Sheets[0].AutoPostBack = false;
            Fpspreadpop.CommandBar.Visible = false;
            Fpspreadpop.Sheets[0].RowHeader.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Fpspreadpop.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.DoubleCellType dblcell = new FarPoint.Web.Spread.DoubleCellType();
            dblcell.MaximumValue = 100;
            dblcell.ErrorMessage = "Allow only Numerics & Allow Percent Limit!";
            int k = 0;

            if (txtallow.Text.Trim() != "--Select--")
            {
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspreadpop.Columns[0].Locked = true;
                Fpspreadpop.Columns[0].Width = 50;

                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspreadpop.Columns[1].Width = 100;

                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell.AutoPostBack = false;
                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Allowances";
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspreadpop.Columns[2].Locked = true;
                Fpspreadpop.Columns[2].Width = 200;

                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Percentage";
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspreadpop.Columns[3].Width = 100;

                Fpspreadpop.SaveChanges();
                Fpspreadpop.Sheets[0].RowCount++;
                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].CellType = chkall;
                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Value = 0;
                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                for (i = 0; i < chklstallow.Items.Count; i++)
                {
                    if (chklstallow.Items[i].Selected == true)
                    {
                        k++;
                        Fpspreadpop.Sheets[0].RowCount++;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k);
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Value = 0;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(chklstallow.Items[i].Text);
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].CellType = dblcell;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    }
                }
                Fpspreadpop.Sheets[0].PageSize = Fpspreadpop.Sheets[0].RowCount;
                Fpspreadpop.Sheets[0].FrozenRowCount = 1;
                Fpspreadpop.Visible = true;
                poperr.Visible = false;
                btnsavepop.Visible = true;
                chkbasic.Checked = false;
                txtbasic.Visible = false;
            }
            else
            {
                Fpspreadpop.Visible = false;
                poperr.Visible = true;
                poperr.Text = "Please Select Any one Allowance!";
                btnsavepop.Visible = false;
            }
        }
        catch { }
    }

    protected void Fpspreadpop_Command(object sender, EventArgs e)
    {
        try
        {
            Fpspreadpop.SaveChanges();
            byte Check = Convert.ToByte(Fpspreadpop.Sheets[0].Cells[0, 1].Value);
            if (Check == 1)
            {
                for (int chs = 1; chs < Fpspreadpop.Sheets[0].RowCount; chs++)
                {
                    Fpspreadpop.Sheets[0].Cells[chs, 1].Value = 1;
                }
            }
            else
            {
                for (int chs = 1; chs < Fpspreadpop.Sheets[0].RowCount; chs++)
                {
                    Fpspreadpop.Sheets[0].Cells[chs, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    protected void imagebtnpopcloseadd_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
        btn_go_Click(sender, e);
    }

    protected void btnsavepop_click(object sender, EventArgs e)
    {
        try
        {
            if (checkedOK(Fpspreadpop))
            {
                if (checkedstrOK(Fpspreadpop))
                {
                    if (checkper())
                    {
                        Double grossamnt = 0;
                        int newupcount = 0;
                        for (int ik = 0; ik < FpSpread.Sheets[0].Rows.Count; ik++)
                        {
                            FpSpread.SaveChanges();
                            byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[ik, 1].Value);
                            if (check == 1)
                            {
                                string staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 2].Text);
                                Double.TryParse(Convert.ToString(FpSpread.Sheets[0].Cells[ik, FpSpread.Sheets[0].ColumnCount - 1].Text), out grossamnt);
                                Double basper = 0;
                                Double basamnt = 0;
                                Double spramnt = 0;
                                Double sprper = 0;
                                string alltext = "";
                                string allowance = "";
                                int upcount = 0;
                                string updquery = "";

                                if (chkbasic.Checked == true)
                                {
                                    Double.TryParse(txtbasic.Text, out basper);
                                    if (basper == 0)
                                    {
                                        poperr.Visible = true;
                                        poperr.Text = "Please Enter the Basic Percent!";
                                        return;
                                    }
                                    else
                                    {
                                        basamnt = (basper / 100) * grossamnt;
                                    }
                                }
                                for (int jk = 0; jk < Fpspreadpop.Sheets[0].Rows.Count; jk++)
                                {
                                    Fpspreadpop.SaveChanges();
                                    byte popcheck = Convert.ToByte(Fpspreadpop.Sheets[0].Cells[jk, 1].Value);
                                    if (popcheck == 1)
                                    {
                                        alltext = Convert.ToString(Fpspreadpop.Sheets[0].Cells[jk, 2].Text);
                                        double.TryParse(Convert.ToString(Fpspreadpop.Sheets[0].Cells[jk, Fpspreadpop.Sheets[0].ColumnCount - 1].Text), out sprper);
                                        spramnt = (sprper / 100) * grossamnt;
                                        if (allowance.Trim() == "")
                                        {
                                            allowance = alltext + ";" + "Amount" + ";" + Convert.ToString(spramnt) + ";;;;;;;;;;;";
                                        }
                                        else
                                        {
                                            allowance = allowance + "\\" + alltext + ";" + "Amount" + ";" + Convert.ToString(spramnt) + ";;;;;;;;;;;";
                                        }
                                    }
                                }
                                if (chkbasic.Checked == true)
                                {
                                    updquery = "update stafftrans set allowances='" + allowance + "',bsalary='" + basamnt + "',Gross_Sal='" + grossamnt + "' where staff_code='" + staffcode + "' and latestrec='1'";
                                }
                                else
                                {
                                    updquery = "update stafftrans set allowances='" + allowance + "',Gross_Sal='" + grossamnt + "' where staff_code='" + staffcode + "' and latestrec='1'";
                                }
                                upcount = d2.update_method_wo_parameter(updquery, "Text");
                                if (upcount > 0)
                                {
                                    newupcount++;
                                }
                            }
                        }
                        if (newupcount > 0)
                        {
                            imgdiv2.Visible = true;
                            lblalerterr.Text = "Allowance Updated Successfully!";
                            loadspreadpop();
                        }
                    }
                    else
                    {
                        poperr.Visible = true;
                        poperr.Text = "Percent Limit Exceeds!";
                    }
                }
                else
                {
                    poperr.Visible = true;
                    poperr.Text = "Please Enter the Percentage for Selected Allowance!";
                }
            }
            else
            {
                poperr.Visible = true;
                poperr.Text = "Please Select any one Allowance!";
            }
        }
        catch { }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void btnexitpop_click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
        btn_go_Click(sender, e);
    }

    protected void chkbasic_change(object sender, EventArgs e)
    {
        try
        {
            if (chkbasic.Checked == true)
            {
                txtbasic.Visible = true;
            }
            else
            {
                txtbasic.Visible = false;
            }
        }
        catch { }
    }

    public bool checkedOK(FarPoint.Web.Spread.FpSpread spread)
    {
        bool Ok = false;
        spread.SaveChanges();
        for (i = 1; i < spread.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(spread.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                Ok = true;
            }
        }
        return Ok;
    }

    public bool checkedstrOK(FarPoint.Web.Spread.FpSpread spread)
    {
        bool Ok = false;
        int amnt = 0;
        spread.SaveChanges();
        for (i = 1; i < spread.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(spread.Sheets[0].Cells[i, 1].Value);
            string amntval = Convert.ToString(spread.Sheets[0].Cells[i, spread.Sheets[0].ColumnCount - 1].Text);
            Int32.TryParse(amntval, out amnt);
            if (check == 1 && amntval.Trim() != "")
            {
                if (amnt != 0)
                {
                    Ok = true;
                }
                else
                {
                    Ok = false;
                    goto jump;
                }
            }
            else if (check == 1 && amntval.Trim() == "")
            {
                Ok = false;
                goto jump;
            }
        }
    jump:
        return Ok;
    }

    public bool checkper()
    {
        bool chkok = false;
        int totamnt = 0;
        int basamnt = 0;
        if (chkbasic.Checked)
        {
            Int32.TryParse(txtbasic.Text, out basamnt);
        }
        Fpspreadpop.SaveChanges();
        try
        {
            for (i = 1; i < Fpspreadpop.Sheets[0].Rows.Count; i++)
            {
                byte check = Convert.ToByte(Fpspreadpop.Sheets[0].Cells[i, 1].Value);
                if (check == 1)
                {
                    totamnt = totamnt + Convert.ToInt32(Fpspreadpop.Sheets[0].Cells[i, Fpspreadpop.Sheets[0].ColumnCount - 1].Text);
                }
            }
            totamnt = totamnt + basamnt;
            if (totamnt == 100)
            {
                chkok = true;
            }
        }
        catch { }
        return chkok;
    }

    public void binddept()
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
                    for (i = 0; i < cbl_dept.Items.Count; i++)
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
                for (i = 0; i < cbl_desig.Items.Count; i++)
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
                for (i = 0; i < cbl_staffc.Items.Count; i++)
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
            string item = "select distinct stftype from stafftrans t,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stype.DataSource = ds;
                cbl_stype.DataTextField = "stftype";
                cbl_stype.DataBind();
                if (cbl_stype.Items.Count > 0)
                {
                    for (i = 0; i < cbl_stype.Items.Count; i++)
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

    protected void allowance()
    {
        try
        {
            ds.Clear();
            chklstallow.Items.Clear();
            string item = "select allowances  from incentives_master where college_code = '" + collegecode1 + "'  ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklstallow.DataSource = ds;
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (i = 0; i < split.Length; i++)
                {
                    string staff = split[i];
                    string[] split1 = staff.Split('\\');
                    if (split1[0].Trim() != "")
                    {
                        string stafftype = split1[0];
                        chklstallow.Items.Add(stafftype);
                    }
                }
                if (chklstallow.Items.Count > 0)
                {
                    for (i = 0; i < chklstallow.Items.Count; i++)
                    {
                        chklstallow.Items[i].Selected = true;
                    }
                    txtallow.Text = "Allowance (" + chklstallow.Items.Count + ")";
                    chkallow.Checked = true;
                }
            }
            else
            {
                txtallow.Text = "--Select--";
                chkallow.Checked = false;
            }
        }
        catch { }
    }

    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (i = 0; i < cblSelected.Items.Count; i++)
            {
                if (cblSelected.Items[i].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[i].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[i].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (i = 0; i < cblSelected.Items.Count; i++)
            {
                if (cblSelected.Items[i].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[i].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[i].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
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
}