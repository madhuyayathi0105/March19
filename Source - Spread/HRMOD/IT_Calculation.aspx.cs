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

public partial class IT_Calculation : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();

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
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
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
                txtdocdate.Attributes.Add("readonly", "readonly");
                txtchqdt.Attributes.Add("readonly", "readonly");
                txtdeddocdate.Attributes.Add("readonly", "readonly");
                txtdedchqdt.Attributes.Add("readonly", "readonly");
                bindallowance(); binddeduction(); bindcity();
                btn_allowance_go_Click(sender, e);
                btn_deduction_go_Click(sender, e);

                cal_docdate.EndDate = DateTime.Now;
                cal_chqdt.EndDate = DateTime.Now;
                cal_deddt.EndDate = DateTime.Now;
                cal_dedchqdt.EndDate = DateTime.Now;
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            lblyearval.Visible = false;
            lblsmserror.Visible = false;
            lbl_alert.Visible = false;
            lblerryear.Visible = false;
        }
        catch (Exception ex) { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null)) and staff_name like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_name";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null)) and staff_code like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_code";
        name = ws.Getname(query);
        return name;
    }

    [WebMethod]
    public static string checkdedgrpname(string dedgrpname)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string allded_acr = dedgrpname;
            if (allded_acr.Trim() != "" && allded_acr != null)
            {
                string queryalldedacr = dd.GetFunction("select distinct ITHeaderID,ITHeaderName from ITHeaderSettings where ITHeaderName='" + allded_acr + "'");
                if (queryalldedacr.Trim() == "" || queryalldedacr == null || queryalldedacr == "0" || queryalldedacr == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
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
        txtdocdate.Attributes.Add("readonly", "readonly");
        txtchqdt.Attributes.Add("readonly", "readonly");
        txtdeddocdate.Attributes.Add("readonly", "readonly");
        txtdedchqdt.Attributes.Add("readonly", "readonly");
        bindallowance(); binddeduction(); bindcity();
        btn_allowance_go_Click(sender, e);
        btn_deduction_go_Click(sender, e);

        cal_docdate.EndDate = DateTime.Now;
        cal_chqdt.EndDate = DateTime.Now;
        cal_deddt.EndDate = DateTime.Now;
        cal_dedchqdt.EndDate = DateTime.Now;

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

            dept = GetSelectedItemsText(cbl_dept);

            desig = GetSelectedItemsText(cbl_desig);

            category = GetSelectedItemsText(cbl_staffc);

            stype = GetSelectedItemsText(cbl_stype);

            status = GetSelectedItemsText(cbl_stat);

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
                selectquery = "select t.staff_code,t.stfstatus,staff_name,dept_name,desig_name,stftype,category_name,bsalary,grade_pay,pay_band,IsMPFAmt,MPFAmount,MPFPer from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c where t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and settled =0 and resign =0 and ISNULL(Discontinue,'0')='0' and m.college_code = '" + collegecode1 + "' and h.dept_name in('" + dept + "') and g.desig_name in('" + desig + "') and c.category_name in('" + category + "') and t.stftype in('" + stype + "') and t.stfstatus in('" + status + "')";
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

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
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
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "IT_Calculation.aspx");
        }
    }

    private void binditaccyear()
    {
        ds.Clear();
        ds.Dispose();
        ddl_ityear.Items.Clear();
        ddl_accyear.Items.Clear();
        string frmyear = Convert.ToString(ddl_accfrmyear.SelectedItem.Text);
        string toyear = Convert.ToString(ddl_acctoyear.SelectedItem.Text);
        ds = d2.select_method_wo_parameter("select distinct PayYear from HrPayMonths where College_Code='" + collegecode1 + "' and PayYear between '" + frmyear + "' and '" + toyear + "' order by PayYear", "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddl_ityear.DataSource = ds;
            ddl_ityear.DataTextField = "PayYear";
            ddl_ityear.DataValueField = "PayYear";
            ddl_ityear.DataBind();
            ddl_ityear.Items.Insert(0, "Select");

            ddl_accyear.DataSource = ds;
            ddl_accyear.DataTextField = "PayYear";
            ddl_accyear.DataValueField = "PayYear";
            ddl_accyear.DataBind();
            ddl_accyear.Items.Insert(0, "Select");
        }
        else
        {
            ddl_ityear.Items.Insert(0, "Select");
            ddl_accyear.Items.Insert(0, "Select");
        }
    }

    protected void ddl_accfrmyear_Change(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddl_accfrmyear.SelectedItem.Text) > Convert.ToInt32(ddl_acctoyear.SelectedItem.Text))
            {
                ddl_accfrmyear.SelectedIndex = 0;
                ddl_acctoyear.SelectedIndex = 0;
            }
            binditaccyear();
            ddl_ityear.SelectedIndex = ddl_ityear.Items.IndexOf(ddl_ityear.Items.FindByText(Convert.ToString(ddl_accfrmyear.SelectedItem.Text)));
            ddl_accyear.SelectedIndex = ddl_accyear.Items.IndexOf(ddl_accyear.Items.FindByText(Convert.ToString(ddl_accfrmyear.SelectedItem.Text)));
        }
        catch { }
    }

    protected void ddl_acctoyear_Change(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddl_acctoyear.SelectedItem.Text) < Convert.ToInt32(ddl_accfrmyear.SelectedItem.Text))
            {
                ddl_accfrmyear.SelectedIndex = 0;
                ddl_acctoyear.SelectedIndex = 0;
            }
            binditaccyear();
            ddl_ityear.SelectedIndex = ddl_ityear.Items.IndexOf(ddl_ityear.Items.FindByText(Convert.ToString(ddl_accfrmyear.SelectedItem.Text)));
            ddl_accyear.SelectedIndex = ddl_accyear.Items.IndexOf(ddl_accyear.Items.FindByText(Convert.ToString(ddl_accfrmyear.SelectedItem.Text)));
        }
        catch { }
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

    protected void btnokyear_click(object sender, EventArgs e)
    {
        if (ddl_accfrmyear.SelectedItem.Text.Trim() == "Select" || ddl_acctoyear.SelectedItem.Text.Trim() == "Select")
        {
            lblerryear.Visible = true;
            lblerryear.Text = "Please Select Accessment Year!";
            return;
        }
        if (ddl_ityear.SelectedItem.Text.Trim() == "Select" || ddl_accyear.SelectedItem.Text.Trim() == "Select")
        {
            lblerryear.Visible = true;
            lblerryear.Text = "Please Select IT and Acc Year!";
            return;
        }
        if (Convert.ToInt32(ddl_accfrmyear.SelectedItem.Text) > Convert.ToInt32(ddl_acctoyear.SelectedItem.Text))
        {
            lblerryear.Visible = true;
            lblerryear.Text = "Access To Year should be greater than From Year!";
        }
        else
        {
            ViewState["accfrmyear"] = Convert.ToString(ddl_accfrmyear.SelectedItem.Text);
            ViewState["acctoyear"] = Convert.ToString(ddl_acctoyear.SelectedItem.Text);
            lblerryear.Visible = false;
            poperrjs.Visible = true;
            cbaddinc.Checked = true;
            btn_addinc.Visible = true;
            cbdeduction.Checked = true;
            btnadddeduction.Visible = true;
            Session["dtded"] = null;
            Session["dtadd"] = null;
            bindaccincgrd();
            bindgrdded();
            bindgrpded();
            lbl_alert.Visible = false;
            divyearpop.Visible = false;
        }
    }

    protected void btnexityear_click(object sender, EventArgs e)
    {
        divyearpop.Visible = false;
    }

    protected void imgyear_Click(object sender, EventArgs e)
    {
        divyearpop.Visible = false;
    }

    protected void btn_st_Click(object sender, EventArgs e)
    {
        if (checkedOK())
        {
            loadfromyear();
            divyearpop.Visible = true;
            lblerryear.Visible = false;
        }
        else
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = "Please Select Any Staff!";
        }
    }

    protected void ddlamntorper_change(object sender, EventArgs e)
    {
        txtamount.Text = "";
    }

    protected void ddldedamnt_change(object sender, EventArgs e)
    {
        txtdedamnt.Text = "";
    }

    protected void ddlfrmmon_Change(object sender, EventArgs e)
    {
        try
        {
            lblyearval.Visible = false;
            ArrayList arryear = new ArrayList();
            arryear.Clear();
            string[] spl = new string[2];
            string endmon = "";
            string endyear = "";
            string enddate = "";
            int day = 1;
            int mon = Convert.ToInt32(ddlfrmmon.SelectedIndex + 1);
            int year = Convert.ToInt32(ddlfrmyear.SelectedItem.Text);
            DateTime dt = new DateTime();
            DateTime dtnew = new DateTime();
            dt = Convert.ToDateTime(Convert.ToString(mon) + "/" + Convert.ToString(day) + "/" + Convert.ToString(year));
            dtnew = dt.AddMonths(12).AddMonths(-1);
            if (dtnew != null)
            {
                enddate = Convert.ToString(dtnew);
                spl = enddate.Split('/');
                endmon = Convert.ToString(spl[0]);
                endyear = Convert.ToString(spl[2]).Split(' ')[0];
            }
            for (int selyear = 0; selyear < ddltoyear.Items.Count; selyear++)
            {
                arryear.Add(Convert.ToString(ddltoyear.Items[selyear].Text));
            }
            if (arryear.Contains(endyear))
            {
                ddltomon.SelectedIndex = ddltomon.Items.IndexOf(ddltomon.Items.FindByValue(endmon));
                ddltoyear.SelectedIndex = ddltoyear.Items.IndexOf(ddltoyear.Items.FindByValue(endyear));
            }
            else
            {
                ddlfrmmon.SelectedIndex = 0;
                ddlfrmyear.SelectedIndex = 0;
                ddltomon.SelectedIndex = 0;
                ddltoyear.SelectedIndex = 0;
            }
        }
        catch { }
    }

    protected void ddltomon_Change(object sender, EventArgs e)
    {
        try
        {
            lblyearval.Visible = false;
            ArrayList arryear = new ArrayList();
            arryear.Clear();
            string[] spl = new string[2];
            string endmon = "";
            string endyear = "";
            string enddate = "";
            int day = 1;
            int mon = Convert.ToInt32(ddltomon.SelectedIndex + 1);
            int year = Convert.ToInt32(ddltoyear.SelectedItem.Text);
            DateTime dt = new DateTime();
            DateTime dtnew = new DateTime();
            dt = Convert.ToDateTime(Convert.ToString(mon) + "/" + Convert.ToString(day) + "/" + Convert.ToString(year));
            dtnew = dt.AddMonths(-12).AddMonths(1);
            if (dtnew != null)
            {
                enddate = Convert.ToString(dtnew);
                spl = enddate.Split('/');
                endmon = Convert.ToString(spl[0]);
                endyear = Convert.ToString(spl[2]).Split(' ')[0];
            }
            for (int selyear = 0; selyear < ddlfrmyear.Items.Count; selyear++)
            {
                arryear.Add(Convert.ToString(ddlfrmyear.Items[selyear].Text));
            }
            if (arryear.Contains(endyear))
            {
                ddlfrmmon.SelectedIndex = ddlfrmmon.Items.IndexOf(ddlfrmmon.Items.FindByValue(endmon));
                ddlfrmyear.SelectedIndex = ddlfrmyear.Items.IndexOf(ddlfrmyear.Items.FindByValue(endyear));
            }
            else
            {
                ddlfrmmon.SelectedIndex = 0;
                ddlfrmyear.SelectedIndex = 0;
                ddltomon.SelectedIndex = 0;
                ddltoyear.SelectedIndex = 0;
            }
        }
        catch { }
    }

    protected void ddlfrmyear_change(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlfrmyear.SelectedItem.Text) > Convert.ToInt32(ddltoyear.SelectedItem.Text))
            {
                lblyearval.Visible = true;
                lblyearval.Text = "Start Year Should be less than End Year!";
            }
            else
            {
                lblyearval.Visible = false;
                ArrayList arryear = new ArrayList();
                arryear.Clear();
                string[] spl = new string[2];
                string endmon = "";
                string endyear = "";
                string enddate = "";
                int day = 1;
                int mon = Convert.ToInt32(ddlfrmmon.SelectedIndex + 1);
                int year = Convert.ToInt32(ddlfrmyear.SelectedItem.Text);
                DateTime dt = new DateTime();
                DateTime dtnew = new DateTime();
                dt = Convert.ToDateTime(Convert.ToString(mon) + "/" + Convert.ToString(day) + "/" + Convert.ToString(year));
                dtnew = dt.AddMonths(12).AddMonths(-1);
                if (dtnew != null)
                {
                    enddate = Convert.ToString(dtnew);
                    spl = enddate.Split('/');
                    endmon = Convert.ToString(spl[0]);
                    endyear = Convert.ToString(spl[2]).Split(' ')[0];
                }
                for (int selyear = 0; selyear < ddltoyear.Items.Count; selyear++)
                {
                    arryear.Add(Convert.ToString(ddltoyear.Items[selyear].Text));
                }
                if (arryear.Contains(endyear))
                {
                    ddltomon.SelectedIndex = ddltomon.Items.IndexOf(ddltomon.Items.FindByValue(endmon));
                    ddltoyear.SelectedIndex = ddltoyear.Items.IndexOf(ddltoyear.Items.FindByValue(endyear));
                }
                else
                {
                    ddlfrmmon.SelectedIndex = 0;
                    ddlfrmyear.SelectedIndex = 0;
                    ddltomon.SelectedIndex = 0;
                    ddltoyear.SelectedIndex = 0;
                }
            }
        }
        catch { }
    }

    protected void ddltoyear_change(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddltoyear.SelectedItem.Text) < Convert.ToInt32(ddlfrmyear.SelectedItem.Text))
            {
                lblyearval.Visible = true;
                lblyearval.Text = "End Year Should be greater than Start Year!";
            }
            else
            {
                lblyearval.Visible = false;
                ArrayList arryear = new ArrayList();
                arryear.Clear();
                string[] spl = new string[2];
                string endmon = "";
                string endyear = "";
                string enddate = "";
                int day = 1;
                int mon = Convert.ToInt32(ddltomon.SelectedIndex + 1);
                int year = Convert.ToInt32(ddltoyear.SelectedItem.Text);
                DateTime dt = new DateTime();
                DateTime dtnew = new DateTime();
                dt = Convert.ToDateTime(Convert.ToString(mon) + "/" + Convert.ToString(day) + "/" + Convert.ToString(year));
                dtnew = dt.AddMonths(-12).AddMonths(1);
                if (dtnew != null)
                {
                    enddate = Convert.ToString(dtnew);
                    spl = enddate.Split('/');
                    endmon = Convert.ToString(spl[0]);
                    endyear = Convert.ToString(spl[2]).Split(' ')[0];
                }
                for (int selyear = 0; selyear < ddlfrmyear.Items.Count; selyear++)
                {
                    arryear.Add(Convert.ToString(ddlfrmyear.Items[selyear].Text));
                }
                if (arryear.Contains(endyear))
                {
                    ddlfrmmon.SelectedIndex = ddlfrmmon.Items.IndexOf(ddlfrmmon.Items.FindByValue(endmon));
                    ddlfrmyear.SelectedIndex = ddlfrmyear.Items.IndexOf(ddlfrmyear.Items.FindByValue(endyear));
                }
                else
                {
                    ddlfrmmon.SelectedIndex = 0;
                    ddlfrmyear.SelectedIndex = 0;
                    ddltomon.SelectedIndex = 0;
                    ddltoyear.SelectedIndex = 0;
                }
            }
        }
        catch { }
    }

    protected void ddlamntorpersec_change(object sender, EventArgs e)
    {
        txtuptosec.Text = "";
    }

    protected void grd_addinc_rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_addinc, "Index$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_addinc, "Index$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_addinc, "Index$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_addinc, "Index$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_addinc, "Index$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_addinc, "Index$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_addinc, "Index$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_addinc, "Index$" + e.Row.RowIndex);
            e.Row.Cells[8].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_addinc, "Index$" + e.Row.RowIndex);
        }
    }

    protected void grd_addinc_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            loadfromyear();
            bindaddreason();
            lbl_allowalert.Visible = false;
            string asstyear = "";
            string inchead = "";
            string itmonyear = "";
            string accmonyear = "";
            string amnt = "";
            string docno = "";
            string docdt = "";
            string chqno = "";
            string chqdt = "";
            string[] splyear = new string[2];

            for (int rem = 0; rem < grd_addinc.Rows.Count; rem++)
            {
                grd_addinc.Rows[rem].BackColor = Color.White;
            }

            int idx = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Index")
            {
                addinc_div.Visible = true;
                divgrdaddinc.Visible = false;
                grd_addinc.Visible = false;
                btn_allincupdate.Visible = true;
                btn_allincsave.Visible = false;
                btn_allincdelete.Visible = true;

                asstyear = (grd_addinc.Rows[idx].FindControl("lbl_allassyear") as Label).Text;
                splyear = asstyear.Split('-');
                ddl_accfrmyear.SelectedIndex = ddl_accfrmyear.Items.IndexOf(ddl_accfrmyear.Items.FindByText(Convert.ToString(splyear[0])));
                ddl_acctoyear.SelectedIndex = ddl_acctoyear.Items.IndexOf(ddl_acctoyear.Items.FindByText(Convert.ToString(splyear[1])));
                binditaccyear();
                inchead = (grd_addinc.Rows[idx].FindControl("lbl_inchead") as Label).Text;
                ddl_detre.SelectedIndex = ddl_detre.Items.IndexOf(ddl_detre.Items.FindByText(inchead));
                itmonyear = (grd_addinc.Rows[idx].FindControl("lbl_allitmonyear") as Label).Text;
                splyear = itmonyear.Split('/');
                ddl_itmon.SelectedIndex = ddl_itmon.Items.IndexOf(ddl_itmon.Items.FindByValue(Convert.ToString(splyear[0])));
                ddl_ityear.SelectedIndex = ddl_ityear.Items.IndexOf(ddl_ityear.Items.FindByText(Convert.ToString(splyear[1])));
                accmonyear = (grd_addinc.Rows[idx].FindControl("lbl_allacmonyear") as Label).Text;
                splyear = accmonyear.Split('/');
                ddl_accmon.SelectedIndex = ddl_accmon.Items.IndexOf(ddl_accmon.Items.FindByValue(Convert.ToString(splyear[0])));
                ddl_accyear.SelectedIndex = ddl_accyear.Items.IndexOf(ddl_accyear.Items.FindByText(Convert.ToString(splyear[1])));
                docno = (grd_addinc.Rows[idx].FindControl("lbl_alldocno") as Label).Text;
                txtdocno.Text = Convert.ToString(docno);
                docdt = (grd_addinc.Rows[idx].FindControl("lbl_alldocdate") as Label).Text;
                txtdocdate.Text = Convert.ToString(docdt);
                amnt = (grd_addinc.Rows[idx].FindControl("lbl_allamnt") as Label).Text;
                txtamount.Text = Convert.ToString(amnt);
                chqno = (grd_addinc.Rows[idx].FindControl("lbl_allchqno") as Label).Text;
                txtchqno.Text = Convert.ToString(chqno);
                chqdt = (grd_addinc.Rows[idx].FindControl("lbl_allchqdt") as Label).Text;
                txtchqdt.Text = Convert.ToString(chqdt);
                grd_addinc.Rows[idx].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }

    protected void btn_allincsave_Click(object sender, EventArgs e)
    {
        try
        {
            int savecount = 0;
            if (txtdocno.Text.Trim() != "" && txtdocdate.Text.Trim() != "" && txtamount.Text.Trim() != "" && txtchqno.Text.Trim() != "" && txtchqdt.Text.Trim() != "" && ddl_detre.SelectedIndex != 0)
            {
                string asstyear = Convert.ToString(ViewState["accfrmyear"]) + "-" + Convert.ToString(ViewState["acctoyear"]);
                string inchead = Convert.ToString(ddl_detre.SelectedItem.Text);
                string itmonyear = Convert.ToString(ddl_itmon.SelectedIndex + 1) + "/" + Convert.ToString(ddl_ityear.SelectedItem.Text);
                string accmonyear = Convert.ToString(ddl_accmon.SelectedIndex + 1) + "/" + Convert.ToString(ddl_accyear.SelectedItem.Text);
                //if (Convert.ToInt32(ViewState["accfrmyear"]) >= Convert.ToInt32(ViewState["acctoyear"]))
                //{
                //    lbl_allowalert.Visible = true;
                //    lbl_allowalert.Text = "Access To Year Should be greater than From Year!";
                //    return;
                //}
                if (Convert.ToInt32(ViewState["accfrmyear"]) > Convert.ToInt32(ddl_accyear.SelectedItem.Text))
                {
                    lbl_allowalert.Visible = true;
                    lbl_allowalert.Text = "Acc Year should be greater than or Equal to Access Year!";
                    return;
                }
                if (Convert.ToInt32(ViewState["accfrmyear"]) > Convert.ToInt32(ddl_ityear.SelectedItem.Text))
                {
                    lbl_allowalert.Visible = true;
                    lbl_allowalert.Text = "IT Year should be greater than or Equal to Access Year!";
                    return;
                }
                string amnt = Convert.ToString(txtamount.Text);
                string docno = Convert.ToString(txtdocno.Text);
                docno = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(docno);
                string docdt = Convert.ToString(txtdocdate.Text);
                string chqno = Convert.ToString(txtchqno.Text);
                chqno = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(chqno);
                string chqdt = Convert.ToString(txtchqdt.Text);
                int rocount = 0;

                divgrdaddinc.Visible = true;
                grd_addinc.Visible = true;
                addinc_div.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add("allassyear");
                dt.Columns.Add("inchead");
                dt.Columns.Add("allitmonyear");
                dt.Columns.Add("allacmonyear");
                dt.Columns.Add("allamnt");
                dt.Columns.Add("alldocno");
                dt.Columns.Add("alldocdate");
                dt.Columns.Add("allchqno");
                dt.Columns.Add("allchqdt");

                DataRow dr;
                if (Session["dtadd"] != null)
                {
                    DataTable dnew = (DataTable)Session["dtadd"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int newro = 0; newro < dt.Rows.Count; newro++)
                        {
                            if (Convert.ToString(dt.Rows[newro][1]) == Convert.ToString(inchead))
                            {
                                lbl_allowalert.Visible = true;
                                lbl_allowalert.Text = "Income Head Already Exists!";
                                divgrdaddinc.Visible = false;
                                grd_addinc.Visible = false;
                                addinc_div.Visible = true;
                                return;
                            }
                            else
                            {
                                rocount++;
                            }
                        }
                    }
                    if (rocount == dt.Rows.Count)
                    {
                        dr = dt.NewRow();
                        dr["allassyear"] = Convert.ToString(asstyear);
                        dr["inchead"] = Convert.ToString(inchead);
                        dr["allitmonyear"] = Convert.ToString(itmonyear);
                        dr["allacmonyear"] = Convert.ToString(accmonyear);
                        dr["allamnt"] = Convert.ToString(amnt);
                        dr["alldocno"] = Convert.ToString(docno);
                        dr["alldocdate"] = Convert.ToString(docdt);
                        dr["allchqno"] = Convert.ToString(chqno);
                        dr["allchqdt"] = Convert.ToString(chqdt);
                        dt.Rows.Add(dr);
                        Session["dtadd"] = dt;
                    }
                }
                else
                {
                    dr = dt.NewRow();
                    dr["allassyear"] = Convert.ToString(asstyear);
                    dr["inchead"] = Convert.ToString(inchead);
                    dr["allitmonyear"] = Convert.ToString(itmonyear);
                    dr["allacmonyear"] = Convert.ToString(accmonyear);
                    dr["allamnt"] = Convert.ToString(amnt);
                    dr["alldocno"] = Convert.ToString(docno);
                    dr["alldocdate"] = Convert.ToString(docdt);
                    dr["allchqno"] = Convert.ToString(chqno);
                    dr["allchqdt"] = Convert.ToString(chqdt);
                    dt.Rows.Add(dr);
                    Session["dtadd"] = dt;
                }

                if (dt.Rows.Count > 0)
                {
                    savecount++;
                    grd_addinc.DataSource = dt;
                    grd_addinc.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grd_addinc.DataBind();
                    for (int i = 0; i < grd_addinc.Columns.Count; i++)
                    {
                        grd_addinc.Columns[i].HeaderStyle.Width = 100;
                        grd_addinc.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grd_addinc.DataSource = dt;
                    grd_addinc.DataBind();
                }
                if (savecount > 0)
                {
                    lbl_allowalert.Visible = true;
                    lbl_allowalert.Text = "Added Successfully!";
                    grd_addinc.Visible = false;
                    divgrdaddinc.Visible = false;
                    addinc_div.Visible = true;
                    allincclear();
                    btnsaveincome.Visible = true;
                }
            }
            else
            {
                lbl_allowalert.Visible = true;
                lbl_allowalert.Text = "Please Fill all the Values!";
            }
        }
        catch { }
    }

    protected void btn_allincupdate_Click(object sender, EventArgs e)
    {
        try
        {
            int upcount = 0;
            if (txtdocno.Text.Trim() != "" && txtdocdate.Text.Trim() != "" && txtamount.Text.Trim() != "" && txtchqno.Text.Trim() != "" && txtchqdt.Text.Trim() != "" && ddl_detre.SelectedIndex != 0)
            {
                string asstyear = Convert.ToString(ViewState["accfrmyear"]) + "-" + Convert.ToString(ViewState["acctoyear"]);
                string inchead = Convert.ToString(ddl_detre.SelectedItem.Text);
                string itmonyear = Convert.ToString(ddl_itmon.SelectedIndex + 1) + "/" + Convert.ToString(ddl_ityear.SelectedItem.Text);
                string accmonyear = Convert.ToString(ddl_accmon.SelectedIndex + 1) + "/" + Convert.ToString(ddl_accyear.SelectedItem.Text);
                //if (Convert.ToInt32(ViewState["accfrmyear"]) >= Convert.ToInt32(ViewState["acctoyear"]))
                //{
                //    lbl_allowalert.Visible = true;
                //    lbl_allowalert.Text = "Access From Year Should be greater than to Year!";
                //    return;
                //}
                if (Convert.ToInt32(ViewState["accfrmyear"]) > Convert.ToInt32(ddl_accyear.SelectedItem.Text))
                {
                    lbl_allowalert.Visible = true;
                    lbl_allowalert.Text = "Acc Year should be greater than or Equal to Access Year!";
                    return;
                }
                if (Convert.ToInt32(ViewState["accfrmyear"]) > Convert.ToInt32(ddl_ityear.SelectedItem.Text))
                {
                    lbl_allowalert.Visible = true;
                    lbl_allowalert.Text = "IT Year should be greater than or Equal to Access Year!";
                    return;
                }
                string amnt = Convert.ToString(txtamount.Text);
                string docno = Convert.ToString(txtdocno.Text);
                docno = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(docno);
                string docdt = Convert.ToString(txtdocdate.Text);
                string chqno = Convert.ToString(txtchqno.Text);
                chqno = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(chqno);
                string chqdt = Convert.ToString(txtchqdt.Text);

                divgrdaddinc.Visible = true;
                grd_addinc.Visible = true;
                addinc_div.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add("allassyear");
                dt.Columns.Add("inchead");
                dt.Columns.Add("allitmonyear");
                dt.Columns.Add("allacmonyear");
                dt.Columns.Add("allamnt");
                dt.Columns.Add("alldocno");
                dt.Columns.Add("alldocdate");
                dt.Columns.Add("allchqno");
                dt.Columns.Add("allchqdt");

                DataRow dr;
                if (Session["dtadd"] != null)
                {
                    DataTable dnew = (DataTable)Session["dtadd"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int newro = 0; newro < dt.Rows.Count; newro++)
                        {
                            if (Convert.ToString(dt.Rows[newro][1]) == Convert.ToString(inchead))
                            {
                                dt.Rows.Remove(dt.Rows[newro]);
                            }
                        }
                    }

                    dr = dt.NewRow();
                    dr["allassyear"] = Convert.ToString(asstyear);
                    dr["inchead"] = Convert.ToString(inchead);
                    dr["allitmonyear"] = Convert.ToString(itmonyear);
                    dr["allacmonyear"] = Convert.ToString(accmonyear);
                    dr["allamnt"] = Convert.ToString(amnt);
                    dr["alldocno"] = Convert.ToString(docno);
                    dr["alldocdate"] = Convert.ToString(docdt);
                    dr["allchqno"] = Convert.ToString(chqno);
                    dr["allchqdt"] = Convert.ToString(chqdt);
                    dt.Rows.Add(dr);
                    Session["dtadd"] = dt;
                }

                if (dt.Rows.Count > 0)
                {
                    upcount++;
                    grd_addinc.DataSource = dt;
                    grd_addinc.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grd_addinc.DataBind();
                    for (int i = 0; i < grd_addinc.Columns.Count; i++)
                    {
                        grd_addinc.Columns[i].HeaderStyle.Width = 100;
                        grd_addinc.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grd_addinc.DataSource = dt;
                    grd_addinc.DataBind();
                }
                if (upcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Updated Successfully!";
                    grd_addinc.Visible = true;
                    divgrdaddinc.Visible = true;
                    addinc_div.Visible = false;
                    allincclear();
                }
            }
            else
            {
                lbl_allowalert.Visible = true;
                lbl_allowalert.Text = "Please Fill all the Values!";
            }
        }
        catch { }
    }

    protected void btn_allincdelete_Click(object sender, EventArgs e)
    {
        try
        {
            int delcount = 0;
            divgrdaddinc.Visible = true;
            grd_addinc.Visible = true;
            addinc_div.Visible = false;

            DataTable dt = new DataTable();
            dt.Columns.Add("allassyear");
            dt.Columns.Add("inchead");
            dt.Columns.Add("allitmonyear");
            dt.Columns.Add("allacmonyear");
            dt.Columns.Add("allamnt");
            dt.Columns.Add("alldocno");
            dt.Columns.Add("alldocdate");
            dt.Columns.Add("allchqno");
            dt.Columns.Add("allchqdt");

            string inchead = Convert.ToString(ddl_detre.SelectedItem.Text);

            DataRow dr;
            if (Session["dtadd"] != null)
            {
                DataTable dnew = (DataTable)Session["dtadd"];
                if (dnew.Rows.Count > 0)
                {
                    for (int ro = 0; ro < dnew.Rows.Count; ro++)
                    {
                        dr = dt.NewRow();
                        for (int col = 0; col < dnew.Columns.Count; col++)
                        {
                            dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    for (int newro = 0; newro < dt.Rows.Count; newro++)
                    {
                        if (Convert.ToString(dt.Rows[newro][1]) == Convert.ToString(inchead))
                        {
                            dt.Rows.Remove(dt.Rows[newro]);
                            delcount++;
                        }
                    }
                }
                Session["dtadd"] = dt;
            }
            if (dt.Rows.Count > 0)
            {
                grd_addinc.DataSource = dt;
                grd_addinc.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grd_addinc.DataBind();
                for (int i = 0; i < grd_addinc.Columns.Count; i++)
                {
                    grd_addinc.Columns[i].HeaderStyle.Width = 100;
                    grd_addinc.Columns[i].ItemStyle.Width = 100;
                }
            }
            else
            {
                grd_addinc.DataSource = dt;
                grd_addinc.DataBind();
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully!";
                grd_addinc.Visible = true;
                divgrdaddinc.Visible = true;
                addinc_div.Visible = false;
                allincclear();
                if (grd_addinc.Rows.Count == 0 && grdded.Rows.Count == 0)
                {
                    btnsaveincome.Visible = false;
                }
            }
        }
        catch { }
    }

    protected void btn_allincexit_Click(object sender, EventArgs e)
    {
        addinc_div.Visible = false;
        divgrdaddinc.Visible = true;
        grd_addinc.Visible = true;
        allincclear();
    }

    public void bindaccincgrd()
    {
        try
        {
            string staffcode = "";
            string inccode = "";
            string itmonyear = "";
            string accmonyear = "";
            if (checkedOK())
            {
                FpSpread.SaveChanges();
                for (int sco = 0; sco < FpSpread.Sheets[0].RowCount; sco++)
                {
                    byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (check == 1)
                    {
                        staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[sco, 2].Text);
                        divgrdaddinc.Visible = true;
                        grd_addinc.Visible = true;
                        addinc_div.Visible = false;

                        DataTable dt = new DataTable();
                        dt.Columns.Add("allassyear");
                        dt.Columns.Add("inchead");
                        dt.Columns.Add("allitmonyear");
                        dt.Columns.Add("allacmonyear");
                        dt.Columns.Add("allamnt");
                        dt.Columns.Add("alldocno");
                        dt.Columns.Add("alldocdate");
                        dt.Columns.Add("allchqno");
                        dt.Columns.Add("allchqdt");

                        string selq = "select * from ITAddAllowDedDetails where Staff_Code='" + staffcode + "' and IsAllow = 1";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr;
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    dr = dt.NewRow();
                                    dr["allassyear"] = Convert.ToString(ds.Tables[0].Rows[row]["AsstYear"]);
                                    inccode = d2.GetFunction("select TextVal from TextValTable where TextCriteria='IncHe' and college_code='" + collegecode1 + "' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[row]["AllowDedDesc"]) + "'");
                                    dr["inchead"] = Convert.ToString(inccode);
                                    itmonyear = Convert.ToString(ds.Tables[0].Rows[row]["ITMon"]) + "/" + Convert.ToString(ds.Tables[0].Rows[row]["ITYear"]);
                                    dr["allitmonyear"] = Convert.ToString(itmonyear);
                                    accmonyear = Convert.ToString(ds.Tables[0].Rows[row]["AccMon"]) + "/" + Convert.ToString(ds.Tables[0].Rows[row]["AccYear"]);
                                    dr["allacmonyear"] = Convert.ToString(accmonyear);
                                    dr["allamnt"] = Convert.ToString(ds.Tables[0].Rows[row]["AllowDedAmount"]);
                                    dr["alldocno"] = Convert.ToString(ds.Tables[0].Rows[row]["DocNo"]);
                                    dr["alldocdate"] = Convert.ToString(ds.Tables[0].Rows[row]["DocDate"]);
                                    dr["allchqno"] = Convert.ToString(ds.Tables[0].Rows[row]["ChqNo"]);
                                    dr["allchqdt"] = Convert.ToString(ds.Tables[0].Rows[row]["ChqDate"]);
                                    dt.Rows.Add(dr);
                                }
                            }
                            Session["dtadd"] = dt;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            grd_addinc.DataSource = dt;
                            grd_addinc.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            grd_addinc.DataBind();
                            for (int i = 0; i < grd_addinc.Columns.Count; i++)
                            {
                                grd_addinc.Columns[i].HeaderStyle.Width = 100;
                                grd_addinc.Columns[i].ItemStyle.Width = 100;
                            }
                        }
                        else
                        {
                            grd_addinc.DataSource = dt;
                            grd_addinc.DataBind();
                        }
                    }
                }
            }
        }
        catch { }
    }

    protected void cbaddinc_CheckedChange(object sender, EventArgs e)
    {
        if (cbaddinc.Checked == true)
        {
            divgrdaddinc.Visible = true;
            grd_addinc.Visible = true;
            addinc_div.Visible = false;
        }
        else
        {
            divgrdaddinc.Visible = false;
            grd_addinc.Visible = false;
            addinc_div.Visible = false;
        }
    }

    protected void btn_addinc_click(object sender, EventArgs e)
    {
        try
        {
            string staffcode = "";
            if (checkedOK())
            {
                FpSpread.SaveChanges();
                for (int sco = 0; sco < FpSpread.Sheets[0].RowCount; sco++)
                {
                    byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (check == 1)
                    {
                        staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[sco, 2].Text);
                        addinc_div.Visible = true;
                        grd_addinc.Visible = false;
                        divgrdaddinc.Visible = false;
                        btn_allincsave.Visible = true;
                        btn_allincupdate.Visible = false;
                        btn_allincdelete.Visible = false;
                        bindaddreason();
                        allincclear();
                        lbl_allowalert.Visible = false;
                    }
                }
            }
        }
        catch { }
    }

    public void allincclear()
    {
        ddl_detre.SelectedIndex = 0;
        txtdocno.Text = "";
        txtdocdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtamount.Text = "";
        txtchqno.Text = "";
        txtchqdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    protected void bindaddreason()
    {
        try
        {
            ddl_detre.Items.Clear();
            ds.Clear();
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='IncHe' and college_code ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_detre.DataSource = ds;
                ddl_detre.DataTextField = "TextVal";
                ddl_detre.DataValueField = "TextCode";
                ddl_detre.DataBind();
                ddl_detre.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddl_detre.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch { }
    }

    protected void binddedreason()
    {
        try
        {
            ddldedhead.Items.Clear();
            ds.Clear();
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedAd' and college_code ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldedhead.DataSource = ds;
                ddldedhead.DataTextField = "TextVal";
                ddldedhead.DataValueField = "TextCode";
                ddldedhead.DataBind();
                ddldedhead.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddldedhead.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch { }
    }

    protected void btn_plus_detre_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addreason.Visible = true;
        lbl_addreason.Text = "Add Income Head";
        lblerror.Visible = false;
    }

    protected void btn_minus_detre_Click(object sender, EventArgs e)
    {
        imgDiv1.Visible = true;
        lblconfirm.Visible = true;
        lblconfirm.Text = "Do you want to delete this Record?";
        ViewState["delclick"] = "1";
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(ViewState["delclick"]) == "1")
            {
                if (ddl_detre.SelectedIndex != -1)
                {
                    if (ddl_detre.SelectedIndex != 0)
                    {
                        string sql = "delete from TextValTable where TextCode='" + ddl_detre.SelectedItem.Value.ToString() + "' and TextCriteria='IncHe' and college_code='" + collegecode1 + "' ";
                        int delete = d2.update_method_wo_parameter(sql, "Text");
                        if (delete != 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Deleted Successfully";
                            imgDiv1.Visible = false;
                            lblconfirm.Visible = false;
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Selected";
                            imgDiv1.Visible = false;
                            lblconfirm.Visible = false;
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Selected";
                        imgDiv1.Visible = false;
                        lblconfirm.Visible = false;
                    }
                }
            }
            if (Convert.ToString(ViewState["delclick"]) == "2")
            {
                if (ddldedhead.SelectedIndex != -1)
                {
                    if (ddldedhead.SelectedIndex != 0)
                    {
                        string sql = "delete from TextValTable where TextCode='" + ddldedhead.SelectedItem.Value.ToString() + "' and TextCriteria='DedAd' and college_code='" + collegecode1 + "' ";
                        int delete = d2.update_method_wo_parameter(sql, "Text");
                        if (delete != 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Deleted Successfully";
                            imgDiv1.Visible = false;
                            lblconfirm.Visible = false;
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Selected";
                            imgDiv1.Visible = false;
                            lblconfirm.Visible = false;
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Selected";
                        imgDiv1.Visible = false;
                        lblconfirm.Visible = false;
                    }
                }
            }
            ViewState["delclick"] = null;
            bindaddreason();
            binddedreason();
        }
        catch { }
    }

    protected void btnno_Click(object sender, EventArgs e)
    {
        imgDiv1.Visible = false;
        lblconfirm.Visible = false;
    }

    protected void btn_addreason_Click(object sender, EventArgs e)
    {
        try
        {
            if (lbl_addreason.Text == "Add Income Head")
            {
                if (txt_addreason.Text != "")
                {
                    txt_addreason.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_addreason.Text);
                    string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_addreason.Text + "' and TextCriteria ='IncHe' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_addreason.Text + "' where TextVal ='" + txt_addreason.Text + "' and TextCriteria ='IncHe' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_addreason.Text + "','IncHe','" + collegecode1 + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully!";
                        txt_addreason.Text = "";
                        plusdiv.Visible = false;
                        panel_addreason.Visible = false;
                    }
                    bindaddreason();
                    txt_addreason.Text = "";
                }
                else
                {
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Enter the Income Head";
                }
            }
            else if (lbl_addreason.Text == "Add Deduction")
            {
                if (txt_addreason.Text != "")
                {
                    txt_addreason.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_addreason.Text);
                    string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_addreason.Text + "' and TextCriteria ='DedAd' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_addreason.Text + "' where TextVal ='" + txt_addreason.Text + "' and TextCriteria ='DedAd' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_addreason.Text + "','DedAd','" + collegecode1 + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully!";
                        txt_addreason.Text = "";
                        plusdiv.Visible = false;
                        panel_addreason.Visible = false;
                    }
                    binddedreason();
                    txt_addreason.Text = "";
                }
                else
                {
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Enter the Deduction";
                }
            }
        }
        catch { }
    }

    protected void btn_exitaddreason_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addreason.Visible = false;
        txt_addreason.Text = "";
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void grdded_rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
            e.Row.Cells[8].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
            e.Row.Cells[9].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdded, "Index$" + e.Row.RowIndex);
        }
    }

    protected void grdded_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            loadfromyear();
            binddedreason();
            lbldederr.Visible = false;
            string asstyear = "";
            string dedhead = "";
            string itmonyear = "";
            string accmonyear = "";
            string amnt = "";
            string docno = "";
            string docdt = "";
            string chqno = "";
            string chqdt = "";
            string dedgrp = "";
            string[] splyear = new string[2];

            for (int rem = 0; rem < grdded.Rows.Count; rem++)
            {
                grdded.Rows[rem].BackColor = Color.White;
            }

            int idx = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Index")
            {
                deduction_div.Visible = true;
                divgrdded.Visible = false;
                grdded.Visible = false;
                btndedupdate.Visible = true;
                btndedsave.Visible = false;
                btndeddelete.Visible = true;

                asstyear = (grdded.Rows[idx].FindControl("lbl_dedassyear") as Label).Text;
                splyear = asstyear.Split('-');
                ddl_accfrmyear.SelectedIndex = ddl_accfrmyear.Items.IndexOf(ddl_accfrmyear.Items.FindByText(Convert.ToString(splyear[0])));
                ddl_acctoyear.SelectedIndex = ddl_acctoyear.Items.IndexOf(ddl_acctoyear.Items.FindByText(Convert.ToString(splyear[1])));
                binditaccyear();
                dedhead = (grdded.Rows[idx].FindControl("lbl_dedinchead") as Label).Text;
                ddldedhead.SelectedIndex = ddldedhead.Items.IndexOf(ddldedhead.Items.FindByText(dedhead));
                itmonyear = (grdded.Rows[idx].FindControl("lbl_deditmonyear") as Label).Text;
                splyear = itmonyear.Split('/');
                ddl_itmon.SelectedIndex = ddl_itmon.Items.IndexOf(ddl_itmon.Items.FindByValue(Convert.ToString(splyear[0])));
                ddl_ityear.SelectedIndex = ddl_ityear.Items.IndexOf(ddl_ityear.Items.FindByText(Convert.ToString(splyear[1])));
                accmonyear = (grdded.Rows[idx].FindControl("lbl_dedacmonyear") as Label).Text;
                splyear = accmonyear.Split('/');
                ddl_accmon.SelectedIndex = ddl_accmon.Items.IndexOf(ddl_accmon.Items.FindByValue(Convert.ToString(splyear[0])));
                ddl_accyear.SelectedIndex = ddl_accyear.Items.IndexOf(ddl_accyear.Items.FindByText(Convert.ToString(splyear[1])));
                docno = (grdded.Rows[idx].FindControl("lbl_deddocno") as Label).Text;
                txtdeddocno.Text = Convert.ToString(docno);
                docdt = (grdded.Rows[idx].FindControl("lbl_deddocdate") as Label).Text;
                txtdeddocdate.Text = Convert.ToString(docdt);
                amnt = (grdded.Rows[idx].FindControl("lbl_dedamnt") as Label).Text;
                txtdedamnt.Text = Convert.ToString(amnt);
                chqno = (grdded.Rows[idx].FindControl("lbl_dedchqno") as Label).Text;
                txtdedchqno.Text = Convert.ToString(chqno);
                chqdt = (grdded.Rows[idx].FindControl("lbl_dedchqdt") as Label).Text;
                txtdedchqdt.Text = Convert.ToString(chqdt);
                dedgrp = (grdded.Rows[idx].FindControl("lbl_dedgrp") as Label).Text;
                ddldedgrp.SelectedIndex = ddldedgrp.Items.IndexOf(ddldedgrp.Items.FindByText(Convert.ToString(dedgrp)));
                grdded.Rows[idx].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }

    protected void btndedsave_Click(object sender, EventArgs e)
    {
        try
        {
            int savecount = 0;
            if (txtdeddocno.Text.Trim() != "" && txtdeddocdate.Text.Trim() != "" && txtdedamnt.Text.Trim() != "" && txtdedchqno.Text.Trim() != "" && txtdedchqdt.Text.Trim() != "" && ddldedhead.SelectedIndex != 0 && ddldedgrp.SelectedIndex != 0)
            {
                string asstyear = Convert.ToString(ViewState["accfrmyear"]) + "-" + Convert.ToString(ViewState["acctoyear"]);
                string inchead = Convert.ToString(ddldedhead.SelectedItem.Text);
                string itmonyear = Convert.ToString(ddl_itmon.SelectedIndex + 1) + "/" + Convert.ToString(ddl_ityear.SelectedItem.Text);
                string accmonyear = Convert.ToString(ddl_accmon.SelectedIndex + 1) + "/" + Convert.ToString(ddl_accyear.SelectedItem.Text);
                //if (Convert.ToInt32(ViewState["accfrmyear"]) >= Convert.ToInt32(ViewState["acctoyear"]))
                //{
                //    lbldederr.Visible = true;
                //    lbldederr.Text = "Access To Year should be greater than Start Year!";
                //    return;
                //}
                if (Convert.ToInt32(ViewState["accfrmyear"]) > Convert.ToInt32(ddl_accyear.SelectedItem.Text))
                {
                    lbldederr.Visible = true;
                    lbldederr.Text = "Acc Year should be greater than or Equal to Access Year!";
                    return;
                }
                if (Convert.ToInt32(ViewState["accfrmyear"]) > Convert.ToInt32(ddl_ityear.SelectedItem.Text))
                {
                    lbldederr.Visible = true;
                    lbldederr.Text = "IT Year should be greater than or Equal to Access Year!";
                    return;
                }
                string amnt = Convert.ToString(txtdedamnt.Text);
                string docno = Convert.ToString(txtdeddocno.Text);
                docno = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(docno);
                string docdt = Convert.ToString(txtdeddocdate.Text);
                string chqno = Convert.ToString(txtdedchqno.Text);
                chqno = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(chqno);
                string chqdt = Convert.ToString(txtdedchqdt.Text);
                string dedgrp = Convert.ToString(ddldedgrp.SelectedItem.Text);
                int rocount = 0;

                divgrdded.Visible = true;
                grdded.Visible = true;
                deduction_div.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add("dedassyear");
                dt.Columns.Add("dedinchead");
                dt.Columns.Add("deditmonyear");
                dt.Columns.Add("dedacmonyear");
                dt.Columns.Add("dedamnt");
                dt.Columns.Add("deddocno");
                dt.Columns.Add("deddocdate");
                dt.Columns.Add("dedchqno");
                dt.Columns.Add("dedchqdt");
                dt.Columns.Add("dedgrp");

                DataRow dr;
                if (Session["dtded"] != null)
                {
                    DataTable dnew = (DataTable)Session["dtded"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int newro = 0; newro < dt.Rows.Count; newro++)
                        {
                            if (Convert.ToString(dt.Rows[newro][1]) == Convert.ToString(inchead))
                            {
                                lbldederr.Visible = true;
                                lbldederr.Text = "Deduction Head Already Exists!";
                                deduction_div.Visible = true;
                                divgrdded.Visible = false;
                                grdded.Visible = false;
                                return;
                            }
                            else
                            {
                                rocount++;
                            }
                        }
                    }

                    if (rocount == dt.Rows.Count)
                    {
                        dr = dt.NewRow();
                        dr["dedassyear"] = Convert.ToString(asstyear);
                        dr["dedinchead"] = Convert.ToString(inchead);
                        dr["deditmonyear"] = Convert.ToString(itmonyear);
                        dr["dedacmonyear"] = Convert.ToString(accmonyear);
                        dr["dedamnt"] = Convert.ToString(amnt);
                        dr["deddocno"] = Convert.ToString(docno);
                        dr["deddocdate"] = Convert.ToString(docdt);
                        dr["dedchqno"] = Convert.ToString(chqno);
                        dr["dedchqdt"] = Convert.ToString(chqdt);
                        dr["dedgrp"] = Convert.ToString(dedgrp);
                        dt.Rows.Add(dr);
                        Session["dtded"] = dt;
                    }
                }
                else
                {
                    dr = dt.NewRow();
                    dr["dedassyear"] = Convert.ToString(asstyear);
                    dr["dedinchead"] = Convert.ToString(inchead);
                    dr["deditmonyear"] = Convert.ToString(itmonyear);
                    dr["dedacmonyear"] = Convert.ToString(accmonyear);
                    dr["dedamnt"] = Convert.ToString(amnt);
                    dr["deddocno"] = Convert.ToString(docno);
                    dr["deddocdate"] = Convert.ToString(docdt);
                    dr["dedchqno"] = Convert.ToString(chqno);
                    dr["dedchqdt"] = Convert.ToString(chqdt);
                    dr["dedgrp"] = Convert.ToString(dedgrp);
                    dt.Rows.Add(dr);
                    Session["dtded"] = dt;
                }

                if (dt.Rows.Count > 0)
                {
                    savecount++;
                    grdded.DataSource = dt;
                    grdded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdded.DataBind();
                    for (int i = 0; i < grdded.Columns.Count; i++)
                    {
                        grdded.Columns[i].HeaderStyle.Width = 100;
                        grdded.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdded.DataSource = dt;
                    grdded.DataBind();
                }
                if (savecount > 0)
                {
                    lbldederr.Visible = true;
                    lbldederr.Text = "Added Successfully!";
                    grdded.Visible = false;
                    divgrdded.Visible = false;
                    deduction_div.Visible = true;
                    dedclear();
                    btnsaveincome.Visible = true;
                }
            }
            else
            {
                lbldederr.Visible = true;
                lbldederr.Text = "Please Fill all the Values!";
            }
        }
        catch { }
    }

    protected void btndedupdate_Click(object sender, EventArgs e)
    {
        try
        {
            int upcount = 0;
            if (txtdeddocno.Text.Trim() != "" && txtdeddocdate.Text.Trim() != "" && txtdedamnt.Text.Trim() != "" && txtdedchqno.Text.Trim() != "" && txtdedchqdt.Text.Trim() != "" && ddldedhead.SelectedIndex != 0 && ddldedgrp.SelectedIndex != 0)
            {
                string asstyear = Convert.ToString(ViewState["accfrmyear"]) + "-" + Convert.ToString(ViewState["acctoyear"]);
                string inchead = Convert.ToString(ddldedhead.SelectedItem.Text);
                string itmonyear = Convert.ToString(ddl_itmon.SelectedIndex + 1) + "/" + Convert.ToString(ddl_ityear.SelectedItem.Text);
                string accmonyear = Convert.ToString(ddl_accmon.SelectedIndex + 1) + "/" + Convert.ToString(ddl_accyear.SelectedItem.Text);
                //if (Convert.ToInt32(ViewState["accfrmyear"]) >= Convert.ToInt32(ViewState["acctoyear"]))
                //{
                //    lbldederr.Visible = true;
                //    lbldederr.Text = "Access From Year Should be greater than To Year!";
                //    return;
                //}
                if (Convert.ToInt32(ViewState["accfrmyear"]) > Convert.ToInt32(ddl_accyear.SelectedItem.Text))
                {
                    lbldederr.Visible = true;
                    lbldederr.Text = "Acc Year should be greater than or Equal to Access Year!";
                    return;
                }
                if (Convert.ToInt32(ViewState["accfrmyear"]) > Convert.ToInt32(ddl_ityear.SelectedItem.Text))
                {
                    lbldederr.Visible = true;
                    lbldederr.Text = "IT Year should be greater than or Equal to Access Year!";
                    return;
                }
                string amnt = Convert.ToString(txtdedamnt.Text);
                string docno = Convert.ToString(txtdeddocno.Text);
                docno = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(docno);
                string docdt = Convert.ToString(txtdeddocdate.Text);
                string chqno = Convert.ToString(txtdedchqno.Text);
                chqno = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(chqno);
                string chqdt = Convert.ToString(txtdedchqdt.Text);
                string dedgrp = Convert.ToString(ddldedgrp.SelectedItem.Text);

                divgrdded.Visible = true;
                grdded.Visible = true;
                deduction_div.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add("dedassyear");
                dt.Columns.Add("dedinchead");
                dt.Columns.Add("deditmonyear");
                dt.Columns.Add("dedacmonyear");
                dt.Columns.Add("dedamnt");
                dt.Columns.Add("deddocno");
                dt.Columns.Add("deddocdate");
                dt.Columns.Add("dedchqno");
                dt.Columns.Add("dedchqdt");
                dt.Columns.Add("dedgrp");

                DataRow dr;
                if (Session["dtded"] != null)
                {
                    DataTable dnew = (DataTable)Session["dtded"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int newro = 0; newro < dt.Rows.Count; newro++)
                        {
                            if (Convert.ToString(dt.Rows[newro][1]) == Convert.ToString(inchead))
                            {
                                dt.Rows.Remove(dt.Rows[newro]);
                            }
                        }
                    }

                    dr = dt.NewRow();
                    dr["dedassyear"] = Convert.ToString(asstyear);
                    dr["dedinchead"] = Convert.ToString(inchead);
                    dr["deditmonyear"] = Convert.ToString(itmonyear);
                    dr["dedacmonyear"] = Convert.ToString(accmonyear);
                    dr["dedamnt"] = Convert.ToString(amnt);
                    dr["deddocno"] = Convert.ToString(docno);
                    dr["deddocdate"] = Convert.ToString(docdt);
                    dr["dedchqno"] = Convert.ToString(chqno);
                    dr["dedchqdt"] = Convert.ToString(chqdt);
                    dr["dedgrp"] = Convert.ToString(dedgrp);
                    dt.Rows.Add(dr);
                    Session["dtded"] = dt;
                }
                if (dt.Rows.Count > 0)
                {
                    upcount++;
                    grdded.DataSource = dt;
                    grdded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdded.DataBind();
                    for (int i = 0; i < grdded.Columns.Count; i++)
                    {
                        grdded.Columns[i].HeaderStyle.Width = 100;
                        grdded.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdded.DataSource = dt;
                    grdded.DataBind();
                }
                if (upcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Updated Successfully!";
                    divgrdded.Visible = true;
                    grdded.Visible = true;
                    deduction_div.Visible = false;
                    dedclear();
                }
            }
            else
            {
                lbldederr.Visible = true;
                lbldederr.Text = "Please Fill all the Values!";
            }
        }
        catch { }
    }

    protected void btndeddelete_Click(object sender, EventArgs e)
    {
        try
        {
            int delcount = 0;
            divgrdded.Visible = true;
            grdded.Visible = true;
            deduction_div.Visible = false;

            DataTable dt = new DataTable();
            dt.Columns.Add("dedassyear");
            dt.Columns.Add("dedinchead");
            dt.Columns.Add("deditmonyear");
            dt.Columns.Add("dedacmonyear");
            dt.Columns.Add("dedamnt");
            dt.Columns.Add("deddocno");
            dt.Columns.Add("deddocdate");
            dt.Columns.Add("dedchqno");
            dt.Columns.Add("dedchqdt");
            dt.Columns.Add("dedgrp");

            string dedhead = Convert.ToString(ddldedhead.SelectedItem.Text);

            DataRow dr;
            if (Session["dtded"] != null)
            {
                DataTable dnew = (DataTable)Session["dtded"];
                if (dnew.Rows.Count > 0)
                {
                    for (int ro = 0; ro < dnew.Rows.Count; ro++)
                    {
                        dr = dt.NewRow();
                        for (int col = 0; col < dnew.Columns.Count; col++)
                        {
                            dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    for (int newro = 0; newro < dt.Rows.Count; newro++)
                    {
                        if (Convert.ToString(dt.Rows[newro][1]) == Convert.ToString(dedhead))
                        {
                            dt.Rows.Remove(dt.Rows[newro]);
                            delcount++;
                        }
                    }
                }
                Session["dtded"] = dt;
            }
            if (dt.Rows.Count > 0)
            {
                grdded.DataSource = dt;
                grdded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdded.DataBind();

                for (int i = 0; i < grdded.Columns.Count; i++)
                {
                    grdded.Columns[i].HeaderStyle.Width = 100;
                    grdded.Columns[i].ItemStyle.Width = 100;
                }
            }
            else
            {
                grdded.DataSource = dt;
                grdded.DataBind();
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully!";
                divgrdded.Visible = true;
                grdded.Visible = true;
                deduction_div.Visible = false;
                dedclear();
                if (grd_addinc.Rows.Count == 0 && grdded.Rows.Count == 0)
                {
                    btnsaveincome.Visible = false;
                }
            }
        }
        catch { }
    }

    protected void btndedexit_Click(object sender, EventArgs e)
    {
        deduction_div.Visible = false;
        divgrdded.Visible = true;
        grdded.Visible = true;
        dedclear();
    }

    public void bindgrdded()
    {
        try
        {
            string staffcode = "";
            string inccode = "";
            string itmonyear = "";
            string accmonyear = "";
            if (checkedOK())
            {
                for (int sco = 0; sco < FpSpread.Sheets[0].RowCount; sco++)
                {
                    byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (check == 1)
                    {
                        staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[sco, 2].Text);
                        divgrdded.Visible = true;
                        grdded.Visible = true;
                        deduction_div.Visible = false;

                        DataTable dt = new DataTable();
                        dt.Columns.Add("dedassyear");
                        dt.Columns.Add("dedinchead");
                        dt.Columns.Add("deditmonyear");
                        dt.Columns.Add("dedacmonyear");
                        dt.Columns.Add("dedamnt");
                        dt.Columns.Add("deddocno");
                        dt.Columns.Add("deddocdate");
                        dt.Columns.Add("dedchqno");
                        dt.Columns.Add("dedchqdt");
                        dt.Columns.Add("dedgrp");

                        string selq = "select * from ITAddAllowDedDetails where Staff_Code='" + staffcode + "' and IsAllow = 0";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr;
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    dr = dt.NewRow();
                                    dr["dedassyear"] = Convert.ToString(ds.Tables[0].Rows[row]["AsstYear"]);
                                    inccode = d2.GetFunction("select TextVal from TextValTable where TextCriteria='DedAd' and college_code='" + collegecode1 + "' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[row]["AllowDedDesc"]) + "'");
                                    dr["dedinchead"] = Convert.ToString(inccode);
                                    itmonyear = Convert.ToString(ds.Tables[0].Rows[row]["ITMon"]) + "/" + Convert.ToString(ds.Tables[0].Rows[row]["ITYear"]);
                                    dr["deditmonyear"] = Convert.ToString(itmonyear);
                                    accmonyear = Convert.ToString(ds.Tables[0].Rows[row]["AccMon"]) + "/" + Convert.ToString(ds.Tables[0].Rows[row]["AccYear"]);
                                    dr["dedacmonyear"] = Convert.ToString(accmonyear);
                                    dr["dedamnt"] = Convert.ToString(ds.Tables[0].Rows[row]["AllowDedAmount"]);
                                    dr["deddocno"] = Convert.ToString(ds.Tables[0].Rows[row]["DocNo"]);
                                    dr["deddocdate"] = Convert.ToString(ds.Tables[0].Rows[row]["DocDate"]);
                                    dr["dedchqno"] = Convert.ToString(ds.Tables[0].Rows[row]["ChqNo"]);
                                    dr["dedchqdt"] = Convert.ToString(ds.Tables[0].Rows[row]["ChqDate"]);
                                    string getdedgrp = d2.GetFunction("select ITHeaderName from ITHeaderSettings where ITHeaderID='" + Convert.ToString(ds.Tables[0].Rows[row]["ITHeaderFK"]) + "'");
                                    if (getdedgrp.Trim() != "" && getdedgrp.Trim() != "0")
                                    {
                                        dr["dedgrp"] = Convert.ToString(getdedgrp);
                                    }
                                    else
                                    {
                                        dr["dedgrp"] = "";
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                            Session["dtded"] = dt;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            grdded.DataSource = dt;
                            grdded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            grdded.DataBind();
                            for (int i = 0; i < grd_addinc.Columns.Count; i++)
                            {
                                grdded.Columns[i].HeaderStyle.Width = 100;
                                grdded.Columns[i].ItemStyle.Width = 100;
                            }
                        }
                        else
                        {
                            grdded.DataSource = dt;
                            grdded.DataBind();
                        }
                    }
                }
            }
        }
        catch { }
    }

    protected void cbdeduction_CheckedChange(object sender, EventArgs e)
    {
        if (cbdeduction.Checked == true)
        {
            divgrdded.Visible = true;
            grdded.Visible = true;
            deduction_div.Visible = false;
        }
        else
        {
            divgrdded.Visible = false;
            grdded.Visible = false;
            deduction_div.Visible = false;
        }
    }

    protected void btnadddeduction_click(object sender, EventArgs e)
    {
        string staffcode = "";
        if (checkedOK())
        {
            for (int sco = 0; sco < FpSpread.Sheets[0].RowCount; sco++)
            {
                byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                if (check == 1)
                {
                    staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[sco, 2].Text);
                    deduction_div.Visible = true;
                    grdded.Visible = false;
                    divgrdded.Visible = false;
                    btndedsave.Visible = true;
                    btndedupdate.Visible = false;
                    btndeddelete.Visible = false;
                    binddedreason();
                    txtdeddocno.Text = "";
                    txtdeddocdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtdedamnt.Text = "";
                    txtdedchqno.Text = "";
                    txtdedchqdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    ddldedgrp.SelectedIndex = 0;
                    lbldederr.Visible = false;
                }
            }
        }
    }

    public void dedclear()
    {
        ddldedhead.SelectedIndex = 0;
        ddldedgrp.SelectedIndex = 0;
        txtdeddocno.Text = "";
        txtdeddocdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtdedamnt.Text = "";
        txtdedchqno.Text = "";
        txtdedchqdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    protected void btndedplus_detre_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addreason.Visible = true;
        lbl_addreason.Text = "Add Deduction";
        lblerror.Visible = false;
    }

    protected void btndedmin_detre_Click(object sender, EventArgs e)
    {
        imgDiv1.Visible = true;
        lblconfirm.Visible = true;
        lblconfirm.Text = "Do you want to delete this Record?";
        ViewState["delclick"] = "2";
    }

    protected void lnkitsetting_click(object sender, EventArgs e)
    {
        txtfrmsecamnt.Text = "";
        txttosecamnt.Text = "";
        txtuptosec.Text = "";
        ddlamntorpersec.SelectedIndex = 0;
        btnupdategrd.Visible = false;
        btndeletegrd.Visible = false;
        string[] splyear = new string[2];
        string[] splmonyearfrst = new string[2];
        string[] splmonyearsec = new string[2];
        divitcalset.Visible = true;
        btnview.Visible = false;
        lblyearval.Visible = false;
        loadfromyear();
        string selyear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IT Calculation Settings' and college_code='" + collegecode1 + "' and user_code='" + usercode + "'");
        if (selyear.Trim() != "0")
        {
            splyear = selyear.Split('-');
            splmonyearfrst = splyear[0].Split(',');
            splmonyearsec = splyear[1].Split(',');
            string frmmon = Convert.ToString(Convert.ToInt32(splmonyearfrst[0]));
            ddlfrmmon.SelectedIndex = Convert.ToInt32(frmmon) - 1;
            ddlfrmyear.SelectedIndex = ddlfrmyear.Items.IndexOf(ddlfrmyear.Items.FindByText(Convert.ToString(splmonyearfrst[1])));
            string tomon = Convert.ToString(Convert.ToInt32(splmonyearsec[0]));
            ddltomon.SelectedIndex = Convert.ToInt32(tomon) - 1;
            ddltoyear.SelectedIndex = ddltoyear.Items.IndexOf(ddltoyear.Items.FindByText(Convert.ToString(splmonyearsec[1])));
        }

        string selitcal = "select * from HR_ITCalculationSettings where collegeCode='" + collegecode1 + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selitcal, "Text");
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    divgrditset.Visible = true;
                    grditset.Visible = true;
                    bindgrditset();
                }
                else
                {
                    Session["dtitset"] = null;
                    divgrditset.Visible = false;
                    grditset.Visible = false;
                }
            }
            else
            {
                Session["dtitset"] = null;
                divgrditset.Visible = false;
                grditset.Visible = false;
            }
        }
        binddedgrp();
        if (grddedgrp.Rows.Count > 0)
        {
            grddedgrp.Visible = true;
        }
        bindgrpded();
        bindallowance();
        binddeduction();
        btn_allowance_go_Click(sender, e);
        btn_deduction_go_Click(sender, e);
    }

    protected void imgitsetpopcloseadd_Click(object sender, EventArgs e)
    {
        divitcalset.Visible = false;
    }

    protected void btnexitincome_click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
        btn_go_Click(sender, e);
    }

    protected void btnsaveincome_click(object sender, EventArgs e)
    {
        try
        {
            if (checkedOK())
            {
                string scode = "";
                string[] splmonyear = new string[2];
                string accyear = "";
                string inchead = "";
                string allitmonyear = "";
                string allaccyear = "";
                string allamnt = "";
                string alldocno = "";
                string alldocdate = "";
                string allchqno = "";
                string allchqdt = "";
                string allitmon = "";
                string allityear = "";
                string allaccmon = "";
                string allacyear = "";

                string dedyear = "";
                string dedhead = "";
                string deditmonyear = "";
                string dedaccyear = "";
                string deddocno = "";
                string deddocdate = "";
                string dedamnt = "";
                string dedchqno = "";
                string dedchqdt = "";
                string deditmon = "";
                string dedityear = "";
                string dedaccmon = "";
                string dedacyear = "";
                string dedheaderid = "";

                int inscount = 0;
                for (int sco = 0; sco < FpSpread.Sheets[0].RowCount; sco++)
                {
                    byte Check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (Check == 1)
                    {
                        scode = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(sco), 2].Text);
                        if (grd_addinc.Rows.Count > 0)
                        {
                            string delrows = "delete from ITAddAllowDedDetails where Staff_Code='" + scode + "' and IsAllow='1'";
                            int delcount = d2.update_method_wo_parameter(delrows, "Text");
                            for (int allro = 0; allro < grd_addinc.Rows.Count; allro++)
                            {
                                string isall = "1";
                                Label lblallyear = (Label)grd_addinc.Rows[allro].FindControl("lbl_allassyear");
                                Label lblityear = (Label)grd_addinc.Rows[allro].FindControl("lbl_allitmonyear");
                                Label lblaccyear = (Label)grd_addinc.Rows[allro].FindControl("lbl_allacmonyear");
                                Label lblallinchead = (Label)grd_addinc.Rows[allro].FindControl("lbl_inchead");
                                Label lblalldocno = (Label)grd_addinc.Rows[allro].FindControl("lbl_alldocno");
                                Label lblalldocdt = (Label)grd_addinc.Rows[allro].FindControl("lbl_alldocdate");
                                Label lblallamnt = (Label)grd_addinc.Rows[allro].FindControl("lbl_allamnt");
                                Label lblallchqno = (Label)grd_addinc.Rows[allro].FindControl("lbl_allchqno");
                                Label lblallchqdt = (Label)grd_addinc.Rows[allro].FindControl("lbl_allchqdt");

                                accyear = Convert.ToString(lblallyear.Text);
                                allitmonyear = Convert.ToString(lblityear.Text);
                                if (allitmonyear.Trim() != "")
                                {
                                    splmonyear = allitmonyear.Split('/');
                                    allitmon = splmonyear[0];
                                    allityear = splmonyear[1];
                                }
                                allaccyear = Convert.ToString(lblaccyear.Text);
                                if (allaccyear.Trim() != "")
                                {
                                    splmonyear = allaccyear.Split('/');
                                    allaccmon = splmonyear[0];
                                    allacyear = splmonyear[1];
                                }
                                allamnt = Convert.ToString(lblallamnt.Text);
                                alldocno = Convert.ToString(lblalldocno.Text);
                                alldocdate = Convert.ToString(lblalldocdt.Text);
                                inchead = Convert.ToString(lblallinchead.Text);
                                allchqno = Convert.ToString(lblallchqno.Text);
                                allchqdt = Convert.ToString(lblallchqdt.Text);

                                string getallcode = d2.GetFunction("select TextCode from TextValTable where TextCriteria='IncHe' and TextVal='" + inchead + "' and college_code='" + collegecode1 + "'");
                                string insquery = "if exists(select * from ITAddAllowDedDetails where Staff_Code='" + scode + "' and IsAllow='" + isall + "' and AllowDedDesc='" + getallcode + "') update ITAddAllowDedDetails set AsstYear='" + accyear + "',IsAllow='" + isall + "',AllowDedAmount='" + allamnt + "',DocNo='" + alldocno + "',DocDate='" + alldocdate + "',ITMon='" + allitmon + "',ITYear='" + allityear + "',AccYear='" + allacyear + "',AccMon='" + allaccmon + "',ChqNo='" + allchqno + "',ChqDate='" + allchqdt + "' where Staff_Code='" + scode + "' and IsAllow='" + isall + "' and AllowDedDesc='" + getallcode + "' else Insert into ITAddAllowDedDetails(Staff_Code,AsstYear,IsAllow,AllowDedDesc,AllowDedAmount,DocNo,DocDate,ITMon,ITYear,AccYear,AccMon,ChqNo,ChqDate) values ('" + scode + "','" + accyear + "','" + isall + "','" + getallcode + "','" + allamnt + "','" + alldocno + "','" + alldocdate + "','" + allitmon + "','" + allityear + "','" + allacyear + "','" + allaccmon + "','" + allchqno + "','" + allchqdt + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }

                        if (grdded.Rows.Count > 0)
                        {
                            string delrows = "delete from ITAddAllowDedDetails where Staff_Code='" + scode + "' and IsAllow='0'";
                            int delcount = d2.update_method_wo_parameter(delrows, "Text");
                            for (int allro = 0; allro < grdded.Rows.Count; allro++)
                            {
                                string isall = "0";
                                Label lbldedyear = (Label)grdded.Rows[allro].FindControl("lbl_dedassyear");
                                Label lbldedityear = (Label)grdded.Rows[allro].FindControl("lbl_deditmonyear");
                                Label lbldedaccyear = (Label)grdded.Rows[allro].FindControl("lbl_dedacmonyear");
                                Label lbldedinchead = (Label)grdded.Rows[allro].FindControl("lbl_dedinchead");
                                Label lbldeddocno = (Label)grdded.Rows[allro].FindControl("lbl_deddocno");
                                Label lbldeddocdt = (Label)grdded.Rows[allro].FindControl("lbl_deddocdate");
                                Label lbldedamnt = (Label)grdded.Rows[allro].FindControl("lbl_dedamnt");
                                Label lbldedchqno = (Label)grdded.Rows[allro].FindControl("lbl_dedchqno");
                                Label lbldedchqdt = (Label)grdded.Rows[allro].FindControl("lbl_dedchqdt");
                                Label lbldedgrp = (Label)grdded.Rows[allro].FindControl("lbl_dedgrp");

                                dedyear = Convert.ToString(lbldedyear.Text);
                                deditmonyear = Convert.ToString(lbldedityear.Text);
                                if (deditmonyear.Trim() != "")
                                {
                                    splmonyear = deditmonyear.Split('/');
                                    deditmon = splmonyear[0];
                                    dedityear = splmonyear[1];
                                }
                                dedaccyear = Convert.ToString(lbldedaccyear.Text);
                                if (dedaccyear.Trim() != "")
                                {
                                    splmonyear = dedaccyear.Split('/');
                                    dedaccmon = splmonyear[0];
                                    dedacyear = splmonyear[1];
                                }
                                dedamnt = Convert.ToString(lbldedamnt.Text);
                                deddocno = Convert.ToString(lbldeddocno.Text);
                                deddocdate = Convert.ToString(lbldeddocdt.Text);
                                dedhead = Convert.ToString(lbldedinchead.Text);
                                dedchqno = Convert.ToString(lbldedchqno.Text);
                                dedchqdt = Convert.ToString(lbldedchqdt.Text);
                                string getdedgrp = d2.GetFunction("select ITHeaderID from ITHeaderSettings where ITHeaderName ='" + lbldedgrp.Text + "'");
                                if (getdedgrp.Trim() != "" && getdedgrp.Trim() != "0")
                                {
                                    dedheaderid = Convert.ToString(getdedgrp);
                                }

                                string getallcode = d2.GetFunction("select TextCode from TextValTable where TextCriteria='DedAd' and TextVal='" + dedhead + "' and college_code='" + collegecode1 + "'");
                                string insquery = "if exists(select * from ITAddAllowDedDetails where Staff_Code='" + scode + "' and IsAllow='" + isall + "' and AllowDedDesc='" + getallcode + "') update ITAddAllowDedDetails set AsstYear='" + dedyear + "',IsAllow='" + isall + "',AllowDedAmount='" + dedamnt + "',DocNo='" + deddocno + "',DocDate='" + deddocdate + "',ITMon='" + deditmon + "',ITYear='" + dedityear + "',AccYear='" + dedacyear + "',ITHeaderFK='" + dedheaderid + "',AccMon='" + dedaccmon + "',ChqNo='" + dedchqno + "',ChqDate='" + dedchqdt + "' where Staff_Code='" + scode + "' and IsAllow='" + isall + "' and AllowDedDesc='" + getallcode + "' else Insert into ITAddAllowDedDetails(Staff_Code,AsstYear,IsAllow,AllowDedDesc,AllowDedAmount,DocNo,DocDate,ITMon,ITYear,AccYear,AccMon,ChqNo,ChqDate,ITHeaderFK) values ('" + scode + "','" + dedyear + "','" + isall + "','" + getallcode + "','" + dedamnt + "','" + deddocno + "','" + deddocdate + "','" + deditmon + "','" + dedityear + "','" + dedacyear + "','" + dedaccmon + "','" + dedchqno + "','" + dedchqdt + "','" + dedheaderid + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                    }
                }
                if (inscount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Details Saved Successfully!";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "IT_Calculation.aspx");
        }
    }

    protected void chkdedgrp_changed(object sender, EventArgs e)
    {
        chkchange(chkdedgrp, chklstdedgrp, txtdedgrp, "Deduction Group");
    }

    protected void chklstdedgrp_changed(object sender, EventArgs e)
    {
        chklstchange(chkdedgrp, chklstdedgrp, txtdedgrp, "Deduction Group");
    }

    protected void btndedgrpadd_click(object sender, EventArgs e)
    {
        divdedgrppop.Visible = true;
        txtadddedgrp.Text = "";
        txtdedgrpamnt.Text = "";
        btn_adddedgrp.Visible = true;
        btn_upddedgrp.Visible = false;
        btn_deldedgrp.Visible = false;
    }

    protected void btndedgrpdel_click(object sender, EventArgs e)
    {

    }

    protected void btn_adddedgrp_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtadddedgrp.Text.Trim() != "" && txtdedgrpamnt.Text.Trim() != "")
            {
                string dedgrp = Convert.ToString(txtadddedgrp.Text);
                dedgrp = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dedgrp);
                double dedamnt = 0;
                double.TryParse(Convert.ToString(txtdedgrpamnt.Text), out dedamnt);
                string insq = "insert into ITHeaderSettings(ITHeaderName,ITMaxAmount) Values ('" + dedgrp + "','" + dedamnt + "')";
                int inscount = d2.update_method_wo_parameter(insq, "Text");
                if (inscount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Saved Successfully!";
                    txtadddedgrp.Text = "";
                    txtdedgrpamnt.Text = "";
                }
            }
        }
        catch { }
    }

    protected void btn_upddedgrp_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtadddedgrp.Text.Trim() != "" && txtdedgrpamnt.Text.Trim() != "")
            {
                string dedgrp = Convert.ToString(txtadddedgrp.Text);
                dedgrp = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dedgrp);
                double dedamnt = 0;
                double.TryParse(Convert.ToString(txtdedgrpamnt.Text), out dedamnt);
                string dedid = Convert.ToString(txtdedid.Text);

                string insq = "Update ITHeaderSettings set ITHeaderName='" + dedgrp + "',ITMaxAmount='" + dedamnt + "' where ITHeaderID='" + dedid + "'";
                int inscount = d2.update_method_wo_parameter(insq, "Text");
                if (inscount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Updated Successfully!";
                    txtadddedgrp.Text = "";
                    txtdedgrpamnt.Text = "";
                    divdedgrppop.Visible = false;
                    binddedgrp();
                }
            }
        }
        catch { }
    }

    protected void btn_deldedgrp_Click(object sender, EventArgs e)
    {
        try
        {
            string dedheaderid = Convert.ToString(txtdedid.Text);
            string selq = "select * from ITAddAllowDedDetails where ITHeaderFK='" + dedheaderid + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "You can't have Permission to Delete!";
            }
            else
            {
                string delq = "Delete from ITHeaderSettings where ITHeaderID='" + dedheaderid + "'";
                int delcount = d2.update_method_wo_parameter(delq, "Text");
                if (delcount > 0)
                {
                    divdedgrppop.Visible = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Deleted Successfully!";
                    binddedgrp();
                    bindgrpded();
                }
            }
        }
        catch { }
    }

    protected void grddedgrp_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grddedgrp, "index$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grddedgrp, "index$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grddedgrp, "index$" + e.Row.RowIndex);
            }
        }
        catch { }
    }

    protected void grddedgrp_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string dedgrpname = "";
            string dedgrpamnt = "";
            string dedgrpid = "";

            for (int rem = 0; rem < grddedgrp.Rows.Count; rem++)
            {
                grddedgrp.Rows[rem].BackColor = Color.White;
            }

            int idx = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "index")
            {
                divdedgrppop.Visible = true;
                btn_upddedgrp.Visible = true;
                btn_adddedgrp.Visible = false;
                btn_deldedgrp.Visible = true;

                dedgrpname = (grddedgrp.Rows[idx].FindControl("lbl_dedgrpname") as Label).Text;
                txtadddedgrp.Text = dedgrpname;
                dedgrpamnt = (grddedgrp.Rows[idx].FindControl("lbl_maxdedamnt") as Label).Text;
                txtdedgrpamnt.Text = Convert.ToString(dedgrpamnt);
                dedgrpid = (grddedgrp.Rows[idx].FindControl("lbl_dedgrpid") as Label).Text;
                txtdedid.Text = dedgrpid;
                grddedgrp.Rows[idx].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }

    public void binddedgrp()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("sno");
            dt.Columns.Add("dedgrp");
            dt.Columns.Add("maxdedamnt");
            dt.Columns.Add("dedgrpid");

            string selitset = "select * from ITHeaderSettings order by ITHeaderName";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selitset, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        dr["sno"] = Convert.ToString(i + 1);
                        dr["dedgrp"] = Convert.ToString(ds.Tables[0].Rows[i]["ITHeaderName"]);
                        dr["maxdedamnt"] = Convert.ToString(ds.Tables[0].Rows[i]["ITMaxAmount"]);
                        dr["dedgrpid"] = Convert.ToString(ds.Tables[0].Rows[i]["ITHeaderID"]);
                        dt.Rows.Add(dr);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                grddedgrp.DataSource = dt;
                grddedgrp.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grddedgrp.DataBind();
            }
            grddedgrp.Columns[0].HeaderStyle.Width = 50;
            grddedgrp.Columns[0].ItemStyle.Width = 50;
            grddedgrp.Columns[1].HeaderStyle.Width = 200;
            grddedgrp.Columns[1].ItemStyle.Width = 200;
            grddedgrp.Columns[2].HeaderStyle.Width = 200;
            grddedgrp.Columns[2].ItemStyle.Width = 200;
        }
        catch { }
    }

    protected void btngodedgrp_click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("sno");
            dt.Columns.Add("dedgrp");
            dt.Columns.Add("maxdedamnt");
            dt.Columns.Add("dedgrpid");
            string headerid = "";
            for (int ik = 0; ik < chklstdedgrp.Items.Count; ik++)
            {
                if (chklstdedgrp.Items[ik].Selected == true)
                {
                    if (headerid.Trim() == "")
                    {
                        headerid = Convert.ToString(chklstdedgrp.Items[ik].Value);
                    }
                    else
                    {
                        headerid = headerid + "'" + "," + "'" + Convert.ToString(chklstdedgrp.Items[ik].Value);
                    }
                }
            }

            string selitset = "select * from ITHeaderSettings where ITHeaderID in('" + headerid + "') order by ITHeaderName";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selitset, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["sno"] = Convert.ToString(i + 1);
                    dr["dedgrp"] = Convert.ToString(ds.Tables[0].Rows[i]["ITHeaderName"]);
                    dr["maxdedamnt"] = Convert.ToString(ds.Tables[0].Rows[i]["ITMaxAmount"]);
                    dr["dedgrpid"] = Convert.ToString(ds.Tables[0].Rows[i]["ITHeaderID"]);
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                grddedgrp.DataSource = dt;
                grddedgrp.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grddedgrp.DataBind();
            }
            grddedgrp.Columns[0].HeaderStyle.Width = 50;
            grddedgrp.Columns[0].ItemStyle.Width = 50;
            grddedgrp.Columns[1].HeaderStyle.Width = 200;
            grddedgrp.Columns[1].ItemStyle.Width = 200;
            grddedgrp.Columns[2].HeaderStyle.Width = 200;
            grddedgrp.Columns[2].ItemStyle.Width = 200;
        }
        catch { }
    }

    protected void btnexitdedgrp_Click(object sender, EventArgs e)
    {
        divdedgrppop.Visible = false;
        grddedgrp.Visible = true;
        bindgrpded();
        binddedgrp();
    }

    public void bindgrpded()
    {
        try
        {
            chklstdedgrp.Items.Clear();
            ddldedgrp.Items.Clear();
            string selded = "";
            selded = "select ITHeaderID,ITHeaderName from ITHeaderSettings order by ITHeaderName";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selded, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklstdedgrp.DataSource = ds;
                chklstdedgrp.DataTextField = "ITHeaderName";
                chklstdedgrp.DataValueField = "ITHeaderID";
                chklstdedgrp.DataBind();

                ddldedgrp.DataSource = ds;
                ddldedgrp.DataTextField = "ITHeaderName";
                ddldedgrp.DataValueField = "ITHeaderID";
                ddldedgrp.DataBind();
                ddldedgrp.Items.Insert(0, "Select");

                for (int i = 0; i < chklstdedgrp.Items.Count; i++)
                {
                    chklstdedgrp.Items[i].Selected = true;
                }
                chkdedgrp.Checked = true;
                txtdedgrp.Text = "Deduction Group(" + Convert.ToString(chklstdedgrp.Items.Count) + ")";
            }
            else
            {
                txtdedgrp.Text = "--Select--";
                chkdedgrp.Checked = false;
                ddldedgrp.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    public string monthdays(string month1, string year)
    {
        string pay_end = "";
        try
        {
            switch (month1)
            {
                case "1":
                    pay_end = "31";
                    break;
                case "2":
                    int yyear = Convert.ToInt32(year);
                    if ((yyear % 4) == 0)
                    {
                        pay_end = "29";
                    }
                    else
                    {
                        pay_end = "28";
                    }
                    break;
                case "3":
                    pay_end = "31";
                    break;
                case "4":
                    pay_end = "30";
                    break;
                case "5":
                    pay_end = "31";
                    break;
                case "6":
                    pay_end = "30";
                    break;
                case "7":
                    pay_end = "31";
                    break;
                case "8":
                    pay_end = "31";
                    break;
                case "9":
                    pay_end = "30";
                    break;
                case "10":
                    pay_end = "31";
                    break;
                case "11":
                    pay_end = "30";
                    break;
                case "12":
                    pay_end = "31";
                    break;
            }
        }
        catch { }
        return pay_end;
    }

    protected void txttosecamnt_change(object sender, EventArgs e)
    {
        try
        {
            if (Session["entryit"] == null)
            {
                string getfrstamnt = "";
                string getsecamnt = "";
                if (txtfrmsecamnt.Text == "")
                {
                    txttosecamnt.Text = "";
                    lblyearval.Visible = true;
                    lblyearval.Text = "Please Enter From Amount!";
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("sno");
                    dt.Columns.Add("itfrmamnt");
                    dt.Columns.Add("ittoamnt");
                    dt.Columns.Add("itmode");
                    dt.Columns.Add("itamntorper");

                    DataTable dnew = new DataTable();
                    dnew = (DataTable)Session["dtitset"];
                    DataRow dr;

                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            dr[0] = Convert.ToString(ro + 1);
                            for (int col = 1; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int newro = 0; newro < dt.Rows.Count; newro++)
                        {
                            for (int col = 0; col < dt.Columns.Count; col++)
                            {
                                getfrstamnt = Convert.ToString(Convert.ToString(dt.Rows[newro][1]).Trim());
                                getsecamnt = Convert.ToString(Convert.ToString(dt.Rows[newro][2]).Trim());
                                if ((Convert.ToDouble(txtfrmsecamnt.Text) >= Convert.ToDouble(getfrstamnt) && Convert.ToDouble(txttosecamnt.Text) <= Convert.ToDouble(getsecamnt)) || (Convert.ToDouble(txtfrmsecamnt.Text) <= Convert.ToDouble(getsecamnt) && Convert.ToDouble(txttosecamnt.Text) >= Convert.ToDouble(getfrstamnt)))
                                {
                                    lblyearval.Visible = true;
                                    lblyearval.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";
                                    txtuptosec.Text = "";
                                    ddlamntorpersec.SelectedIndex = 0;
                                    return;
                                }
                                if (Convert.ToDouble(getfrstamnt) == Convert.ToDouble(txtfrmsecamnt.Text))
                                {
                                    lblyearval.Visible = true;
                                    lblyearval.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";
                                    txtuptosec.Text = "";
                                    ddlamntorpersec.SelectedIndex = 0;
                                    return;
                                }
                                if (Convert.ToDouble(getsecamnt) == Convert.ToDouble(txttosecamnt.Text))
                                {
                                    lblyearval.Visible = true;
                                    lblyearval.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";
                                    txtuptosec.Text = "";
                                    ddlamntorpersec.SelectedIndex = 0;
                                    return;
                                }
                            }
                        }
                    }
                    lblyearval.Visible = false;
                }
            }
            if (Session["entryit"] == "1")
            {
                if (txtfrmsecamnt.Text.Trim() != "" && txttosecamnt.Text.Trim() != "" && txtuptosec.Text.Trim() != "")
                {
                    string amntfrmto = "";
                    string getfrstamnt = "";
                    string getsecamnt = "";
                    string frmamntlimit = Convert.ToString(ViewState["frmamnt"]);
                    string toamntlimit = Convert.ToString(ViewState["toamnt"]);

                    string modeabovefromto = Convert.ToString(ddlamntorpersec.SelectedItem.Text);
                    if (modeabovefromto.Trim() == "Amount")
                    {
                        amntfrmto = Convert.ToString(txtuptosec.Text);
                    }
                    else
                    {
                        amntfrmto = Convert.ToString(txtuptosec.Text) + "%";
                    }

                    divgrditset.Visible = true;
                    grditset.Visible = true;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("sno");
                    dt.Columns.Add("itfrmamnt");
                    dt.Columns.Add("ittoamnt");
                    dt.Columns.Add("itmode");
                    dt.Columns.Add("itamntorper");
                    DataRow dr;

                    if (Session["dtitset"] != null)
                    {
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["dtitset"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                dr[0] = Convert.ToString(ro + 1);
                                for (int col = 1; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (int newro = 0; newro < dt.Rows.Count; newro++)
                            {
                                if (Convert.ToString(dt.Rows[newro][1]).Trim() == Convert.ToString(frmamntlimit).Trim())
                                {
                                    dt.Rows.Remove(dt.Rows[newro]);
                                }
                            }
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dt.Rows.Count; ro++)
                            {
                                for (int co = 0; co < dt.Columns.Count; co++)
                                {
                                    getfrstamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][1]).Trim());
                                    getsecamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][2]).Trim());
                                    if ((Convert.ToDouble(getfrstamnt) <= Convert.ToDouble(txtfrmsecamnt.Text.Trim()) && Convert.ToDouble(getsecamnt) >= Convert.ToDouble(txttosecamnt.Text.Trim())) || (Convert.ToDouble(txtfrmsecamnt.Text) <= Convert.ToDouble(getsecamnt) && Convert.ToDouble(txttosecamnt.Text) >= Convert.ToDouble(getfrstamnt)))
                                    {
                                        lblyearval.Visible = true;
                                        lblyearval.Text = "Amount Range Already Exists!";
                                        txtfrmsecamnt.Text = "";
                                        txttosecamnt.Text = "";
                                        txtuptosec.Text = "";
                                        ddlamntorpersec.SelectedIndex = 0;
                                        return;
                                    }
                                    else if (Convert.ToDouble(getfrstamnt) == Convert.ToDouble(txtfrmsecamnt.Text.Trim()))
                                    {
                                        lblyearval.Visible = true;
                                        lblyearval.Text = "Amount Range Already Exists!";
                                        txtfrmsecamnt.Text = "";
                                        txttosecamnt.Text = "";
                                        txtuptosec.Text = "";
                                        ddlamntorpersec.SelectedIndex = 0;
                                        return;
                                    }
                                    else if (Convert.ToDouble(getsecamnt) == Convert.ToDouble(txttosecamnt.Text.Trim()))
                                    {
                                        lblyearval.Visible = true;
                                        lblyearval.Text = "Amount Range Already Exists!";
                                        txtfrmsecamnt.Text = "";
                                        txttosecamnt.Text = "";
                                        txtuptosec.Text = "";
                                        ddlamntorpersec.SelectedIndex = 0;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                Session["entryit"] = null;
            }
        }
        catch { }
    }

    protected void grditset_rowbound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grditset, "Index$" + e.Row.RowIndex);
        }
    }

    protected void grditset_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string itfrmrange = "";
            string ittorange = "";
            string mode = "";
            string amountorper = "";
            string gender = "";
            Session["entryit"] = "1";

            for (int rem = 0; rem < grditset.Rows.Count; rem++)
            {
                grditset.Rows[rem].BackColor = Color.White;
            }

            int idx = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Index")
            {
                divgrditset.Visible = true;
                grditset.Visible = true;
                btnupdategrd.Visible = true;
                btnsaveitset.Visible = false;
                btndeletegrd.Visible = true;

                itfrmrange = (grditset.Rows[idx].FindControl("lbl_frmamnt") as Label).Text;
                txtfrmsecamnt.Text = itfrmrange;
                ViewState["frmamnt"] = Convert.ToString(itfrmrange);
                ittorange = (grditset.Rows[idx].FindControl("lbl_toamnt") as Label).Text;
                txttosecamnt.Text = Convert.ToString(ittorange);
                ViewState["toamnt"] = Convert.ToString(ittorange);
                mode = (grditset.Rows[idx].FindControl("lbl_itmode") as Label).Text;
                ddlamntorpersec.SelectedIndex = ddlamntorpersec.Items.IndexOf(ddlamntorpersec.Items.FindByText(mode));
                amountorper = (grditset.Rows[idx].FindControl("lbl_itamntorper") as Label).Text;
                txtuptosec.Text = Convert.ToString(amountorper);
                gender = (grditset.Rows[idx].FindControl("lbl_gender") as Label).Text;
                ddlgender.SelectedIndex = ddlgender.Items.IndexOf(ddlgender.Items.FindByText(gender));
                grditset.Rows[idx].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }

    protected void btnadditset_click(object sender, EventArgs e)
    {
        int savecount = 0;
        try
        {
            if (txtfrmsecamnt.Text.Trim() != "" && txttosecamnt.Text.Trim() != "" && txtuptosec.Text.Trim() != "")
            {
                string getfrstamnt = "";
                string getsecamnt = "";
                string amntfrmto = "";
                string getgender = "";
                int rocount = 0;
                string frmamntlimit = Convert.ToString(txtfrmsecamnt.Text);
                string toamntlimit = Convert.ToString(txttosecamnt.Text);
                string gender = Convert.ToString(ddlgender.SelectedItem.Text).Trim();

                string modeabovefromto = Convert.ToString(ddlamntorpersec.SelectedItem.Text);
                if (modeabovefromto.Trim() == "Amount")
                {
                    amntfrmto = Convert.ToString(txtuptosec.Text);
                }
                else
                {
                    amntfrmto = Convert.ToString(txtuptosec.Text) + "%";
                }

                divgrditset.Visible = true;
                grditset.Visible = true;

                DataTable dt = new DataTable();
                dt.Columns.Add("sno");
                dt.Columns.Add("itfrmamnt");
                dt.Columns.Add("ittoamnt");
                dt.Columns.Add("itmode");
                dt.Columns.Add("itamntorper");
                dt.Columns.Add("gender");
                DataRow dr;

                if (Session["dtitset"] != null)
                {
                    DataTable dnew = new DataTable();
                    dnew = (DataTable)Session["dtitset"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            dr[0] = Convert.ToString(ro + 1);
                            for (int col = 1; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dt.Rows.Count; ro++)
                        {
                            for (int co = 0; co < dt.Columns.Count; co++)
                            {
                                getfrstamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][1]).Trim());
                                getsecamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][2]).Trim());
                                getgender = Convert.ToString(Convert.ToString(dt.Rows[ro][5]).Trim());
                                if ((Convert.ToDouble(getfrstamnt) <= Convert.ToDouble(txtfrmsecamnt.Text.Trim()) && Convert.ToDouble(getsecamnt) >= Convert.ToDouble(txttosecamnt.Text.Trim()) && getgender == gender) || (Convert.ToDouble(txtfrmsecamnt.Text) <= Convert.ToDouble(getsecamnt) && Convert.ToDouble(txttosecamnt.Text) >= Convert.ToDouble(getfrstamnt) && getgender == gender))
                                {
                                    lblyearval.Visible = true;
                                    lblyearval.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";
                                    txtuptosec.Text = "";
                                    ddlamntorpersec.SelectedIndex = 0;
                                    return;
                                }
                                else if (Convert.ToDouble(getfrstamnt) == Convert.ToDouble(txtfrmsecamnt.Text.Trim()) && getgender == gender)
                                {
                                    lblyearval.Visible = true;
                                    lblyearval.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";
                                    txtuptosec.Text = "";
                                    ddlamntorpersec.SelectedIndex = 0;
                                    return;
                                }
                                else if (Convert.ToDouble(getsecamnt) == Convert.ToDouble(txttosecamnt.Text.Trim()) && getgender == gender)
                                {
                                    lblyearval.Visible = true;
                                    lblyearval.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";
                                    txtuptosec.Text = "";
                                    ddlamntorpersec.SelectedIndex = 0;
                                    return;
                                }
                                else
                                {
                                    rocount++;
                                }
                            }
                        }
                    }

                    if (rocount == (dt.Rows.Count * dt.Columns.Count))
                    {
                        dr = dt.NewRow();
                        dr["sno"] = Convert.ToString(dt.Rows.Count + 1);
                        dr["itfrmamnt"] = Convert.ToString(frmamntlimit);
                        dr["ittoamnt"] = Convert.ToString(toamntlimit);
                        dr["itmode"] = Convert.ToString(modeabovefromto);
                        dr["itamntorper"] = Convert.ToString(amntfrmto);
                        dr["gender"] = Convert.ToString(gender);
                        dt.Rows.Add(dr);
                        Session["dtitset"] = dt;
                    }
                }
                else
                {
                    dr = dt.NewRow();
                    dr["sno"] = Convert.ToString("1");
                    dr["itfrmamnt"] = Convert.ToString(frmamntlimit);
                    dr["ittoamnt"] = Convert.ToString(toamntlimit);
                    dr["itmode"] = Convert.ToString(modeabovefromto);
                    dr["itamntorper"] = Convert.ToString(amntfrmto);
                    dr["gender"] = Convert.ToString(gender);
                    dt.Rows.Add(dr);
                    Session["dtitset"] = dt;
                }
                if (dt.Rows.Count > 0)
                {
                    savecount++;
                    grditset.DataSource = dt;
                    grditset.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grditset.DataBind();
                    txtfrmsecamnt.Text = "";
                    txttosecamnt.Text = "";
                    ddlamntorpersec.SelectedIndex = 0;
                    txtuptosec.Text = "";
                    btndeletegrd.Visible = false;
                    btnupdategrd.Visible = false;

                    grditset.Columns[0].HeaderStyle.Width = 75;
                    grditset.Columns[0].ItemStyle.Width = 75;
                    grditset.Columns[1].HeaderStyle.Width = 125;
                    grditset.Columns[1].ItemStyle.Width = 125;
                    grditset.Columns[2].HeaderStyle.Width = 125;
                    grditset.Columns[2].ItemStyle.Width = 125;
                    grditset.Columns[3].HeaderStyle.Width = 100;
                    grditset.Columns[3].ItemStyle.Width = 100;
                    grditset.Columns[4].HeaderStyle.Width = 125;
                    grditset.Columns[4].ItemStyle.Width = 125;
                    grditset.Columns[5].HeaderStyle.Width = 100;
                    grditset.Columns[5].ItemStyle.Width = 100;
                }
                else
                {
                    grditset.DataSource = dt;
                    grditset.DataBind();
                }
                if (savecount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Saved Successfully!";
                    grditset.Visible = true;
                    divgrditset.Visible = true;
                }
            }
            else
            {
                lblyearval.Visible = true;
                lblyearval.Text = "Please Fill All the Values!";
            }
        }
        catch { }
    }

    protected void btnsaveitset_Click(object sender, EventArgs e)
    {

    }

    protected void btnupdategrd_Click(object sender, EventArgs e)
    {
        try
        {
            int upcount = 0;
            if (txtfrmsecamnt.Text.Trim() != "" && txttosecamnt.Text.Trim() != "" && txtuptosec.Text.Trim() != "")
            {
                string amntfrmto = "";
                string getfrstamnt = "";
                string getsecamnt = "";
                string getgender = "";
                int rocount = 0;
                string frmamntlimit = Convert.ToString(ViewState["frmamnt"]);
                string toamntlimit = Convert.ToString(ViewState["toamnt"]);
                string gender = Convert.ToString(ddlgender.SelectedItem.Text).Trim();

                string modeabovefromto = Convert.ToString(ddlamntorpersec.SelectedItem.Text);
                if (modeabovefromto.Trim() == "Amount")
                {
                    amntfrmto = Convert.ToString(txtuptosec.Text);
                }
                else
                {
                    amntfrmto = Convert.ToString(txtuptosec.Text) + "%";
                }

                divgrditset.Visible = true;
                grditset.Visible = true;

                DataTable dt = new DataTable();
                dt.Columns.Add("sno");
                dt.Columns.Add("itfrmamnt");
                dt.Columns.Add("ittoamnt");
                dt.Columns.Add("itmode");
                dt.Columns.Add("itamntorper");
                dt.Columns.Add("gender");
                DataRow dr;

                if (Session["dtitset"] != null)
                {
                    DataTable dnew = new DataTable();
                    dnew = (DataTable)Session["dtitset"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            dr[0] = Convert.ToString(ro + 1);
                            for (int col = 1; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int newro = 0; newro < dt.Rows.Count; newro++)
                        {
                            if (Convert.ToString(dt.Rows[newro][1]).Trim() == Convert.ToString(frmamntlimit).Trim())
                            {
                                dt.Rows.Remove(dt.Rows[newro]);
                            }
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dt.Rows.Count; ro++)
                        {
                            for (int co = 0; co < dt.Columns.Count; co++)
                            {
                                getfrstamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][1]).Trim());
                                getsecamnt = Convert.ToString(Convert.ToString(dt.Rows[ro][2]).Trim());
                                getgender = Convert.ToString(Convert.ToString(dt.Rows[ro][5]).Trim());
                                if ((Convert.ToDouble(getfrstamnt) <= Convert.ToDouble(txtfrmsecamnt.Text.Trim()) && Convert.ToDouble(getsecamnt) >= Convert.ToDouble(txttosecamnt.Text.Trim()) && getgender == gender) || (Convert.ToDouble(txtfrmsecamnt.Text) <= Convert.ToDouble(getsecamnt) && Convert.ToDouble(txttosecamnt.Text) >= Convert.ToDouble(getfrstamnt) && getgender == gender))
                                {
                                    lblyearval.Visible = true;
                                    lblyearval.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";
                                    txtuptosec.Text = "";
                                    ddlamntorpersec.SelectedIndex = 0;
                                    return;
                                }
                                else if (Convert.ToDouble(getfrstamnt) == Convert.ToDouble(txtfrmsecamnt.Text.Trim()) && getgender == gender)
                                {
                                    lblyearval.Visible = true;
                                    lblyearval.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";
                                    txtuptosec.Text = "";
                                    ddlamntorpersec.SelectedIndex = 0;
                                    return;
                                }
                                else if (Convert.ToDouble(getsecamnt) == Convert.ToDouble(txttosecamnt.Text.Trim()) && getgender == gender)
                                {
                                    lblyearval.Visible = true;
                                    lblyearval.Text = "Amount Range Already Exists!";
                                    txtfrmsecamnt.Text = "";
                                    txttosecamnt.Text = "";
                                    txtuptosec.Text = "";
                                    ddlamntorpersec.SelectedIndex = 0;
                                    return;
                                }
                                else
                                {
                                    rocount++;
                                }
                            }
                        }
                    }

                    if (rocount == (dt.Rows.Count * dt.Columns.Count))
                    {
                        dr = dt.NewRow();
                        dr["sno"] = Convert.ToString(dt.Rows.Count + 1);
                        dr["itfrmamnt"] = Convert.ToString(txtfrmsecamnt.Text.Trim());
                        dr["ittoamnt"] = Convert.ToString(txttosecamnt.Text.Trim());
                        dr["itmode"] = Convert.ToString(modeabovefromto);
                        dr["itamntorper"] = Convert.ToString(amntfrmto);
                        dr["gender"] = Convert.ToString(gender);
                        dt.Rows.Add(dr);
                        Session["dtitset"] = dt;
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    upcount++;
                    DataTable dnext = new DataTable();
                    dnext.Columns.Add("sno");
                    dnext.Columns.Add("itfrmamnt");
                    dnext.Columns.Add("ittoamnt");
                    dnext.Columns.Add("itmode");
                    dnext.Columns.Add("itamntorper");
                    dnext.Columns.Add("gender");
                    DataRow drov;

                    for (int ik = 0; ik < dt.Rows.Count; ik++)
                    {
                        drov = dnext.NewRow();
                        drov[0] = Convert.ToString(ik + 1);
                        for (int col = 1; col < dt.Columns.Count; col++)
                        {
                            drov[col] = Convert.ToString(dt.Rows[ik][col]);
                        }
                        dnext.Rows.Add(drov);
                    }

                    grditset.DataSource = dnext;
                    grditset.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grditset.DataBind();
                    txtfrmsecamnt.Text = "";
                    txttosecamnt.Text = "";
                    ddlamntorpersec.SelectedIndex = 0;
                    btndeletegrd.Visible = false;
                    btnupdategrd.Visible = false;
                    txtuptosec.Text = "";

                    grditset.Columns[0].HeaderStyle.Width = 75;
                    grditset.Columns[0].ItemStyle.Width = 75;
                    grditset.Columns[1].HeaderStyle.Width = 125;
                    grditset.Columns[1].ItemStyle.Width = 125;
                    grditset.Columns[2].HeaderStyle.Width = 125;
                    grditset.Columns[2].ItemStyle.Width = 125;
                    grditset.Columns[3].HeaderStyle.Width = 100;
                    grditset.Columns[3].ItemStyle.Width = 100;
                    grditset.Columns[4].HeaderStyle.Width = 125;
                    grditset.Columns[4].ItemStyle.Width = 125;
                    grditset.Columns[5].HeaderStyle.Width = 100;
                    grditset.Columns[5].ItemStyle.Width = 100;
                }
                else
                {
                    grditset.DataSource = dt;
                    grditset.DataBind();
                }
                if (upcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Updated Successfully!";
                    divgrditset.Visible = true;
                    grditset.Visible = true;
                }
            }
            else
            {
                lblyearval.Visible = true;
                lblyearval.Text = "Please Fill All the Values!";
            }
        }
        catch { }
    }

    protected void btndeletegrd_Click(object sender, EventArgs e)
    {
        try
        {
            int delcount = 0;
            divgrditset.Visible = true;
            grditset.Visible = true;

            DataTable dt = new DataTable();
            dt.Columns.Add("sno");
            dt.Columns.Add("itfrmamnt");
            dt.Columns.Add("ittoamnt");
            dt.Columns.Add("itmode");
            dt.Columns.Add("itamntorper");
            dt.Columns.Add("gender");
            DataRow dr;
            string frmamntlimit = Convert.ToString(txtfrmsecamnt.Text);

            if (Session["dtitset"] != null)
            {
                DataTable dnew = new DataTable();
                dnew = (DataTable)Session["dtitset"];
                if (dnew.Rows.Count > 0)
                {
                    for (int ro = 0; ro < dnew.Rows.Count; ro++)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(ro + 1);
                        for (int col = 1; col < dnew.Columns.Count; col++)
                        {
                            dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    for (int newro = 0; newro < dt.Rows.Count; newro++)
                    {
                        if (Convert.ToString(dt.Rows[newro][1]).Trim() == Convert.ToString(frmamntlimit).Trim())
                        {
                            dt.Rows.Remove(dt.Rows[newro]);
                            delcount++;
                        }
                    }
                }
                Session["dtitset"] = dt;
            }

            if (dt.Rows.Count > 0)
            {
                DataTable dnext = new DataTable();
                dnext.Columns.Add("sno");
                dnext.Columns.Add("itfrmamnt");
                dnext.Columns.Add("ittoamnt");
                dnext.Columns.Add("itmode");
                dnext.Columns.Add("itamntorper");
                dnext.Columns.Add("gender");
                DataRow drownxt;

                for (int ik = 0; ik < dt.Rows.Count; ik++)
                {
                    drownxt = dnext.NewRow();
                    drownxt[0] = Convert.ToString(ik + 1);
                    for (int col = 1; col < dt.Columns.Count; col++)
                    {
                        drownxt[col] = Convert.ToString(dt.Rows[ik][col]);
                    }
                    dnext.Rows.Add(drownxt);
                }
                grditset.DataSource = dnext;
                grditset.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grditset.DataBind();

                grditset.Columns[0].HeaderStyle.Width = 75;
                grditset.Columns[0].ItemStyle.Width = 75;
                grditset.Columns[1].HeaderStyle.Width = 125;
                grditset.Columns[1].ItemStyle.Width = 125;
                grditset.Columns[2].HeaderStyle.Width = 125;
                grditset.Columns[2].ItemStyle.Width = 125;
                grditset.Columns[3].HeaderStyle.Width = 100;
                grditset.Columns[3].ItemStyle.Width = 100;
                grditset.Columns[4].HeaderStyle.Width = 125;
                grditset.Columns[4].ItemStyle.Width = 125;
                grditset.Columns[5].HeaderStyle.Width = 100;
                grditset.Columns[5].ItemStyle.Width = 100;
                txtfrmsecamnt.Text = "";
                txttosecamnt.Text = "";
                ddlamntorpersec.SelectedIndex = 0;
                txtuptosec.Text = "";
                btndeletegrd.Visible = false;
                btnupdategrd.Visible = false;
            }
            else
            {
                grditset.DataSource = dt;
                grditset.DataBind();
                btndeletegrd.Visible = false;
                btnupdategrd.Visible = false;
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully!";
                txtfrmsecamnt.Text = "";
                txttosecamnt.Text = "";
                txtuptosec.Text = "";
                ddlamntorpersec.SelectedIndex = 0;
                grditset.Visible = true;
                divgrditset.Visible = true;
            }
        }
        catch { }
    }

    protected void btnexititallset_Click(object sender, EventArgs e)
    {
        divitcalset.Visible = false;
    }

    protected void btnview_click(object sender, EventArgs e)
    {
        divgrditset.Visible = true;
        grditset.Visible = true;
        btnview.Visible = false;
    }

    protected void btnsaveallitset_Click(object sender, EventArgs e)
    {
        try
        {
            string linkvalue = "";
            string splvaldt = "";
            string modeval = "";
            int insertcount = 0;
            DateTime frmdt = new DateTime();
            DateTime todt = new DateTime();
            TimeSpan valdate = new TimeSpan();
            string startday = "1";
            if (ddlfrmyear.SelectedItem.Text.Trim() == "Select" || ddltoyear.SelectedItem.Text.Trim() == "Select")
            {
                lblyearval.Visible = true;
                lblyearval.Text = "Please Select IT Year!";
                return;
            }
            string frmmon = Convert.ToString(ddlfrmmon.SelectedIndex + 1);
            string frmyear = Convert.ToString(ddlfrmyear.SelectedItem.Text);
            frmdt = Convert.ToDateTime(frmmon + "/" + startday + "/" + frmyear);
            string tomon = Convert.ToString(ddltomon.SelectedIndex + 1);
            string toyear = Convert.ToString(ddltoyear.SelectedItem.Text);
            string today = monthdays(tomon, toyear);
            todt = Convert.ToDateTime(tomon + "/" + today + "/" + toyear);
            valdate = todt - frmdt;
            splvaldt = Convert.ToString(valdate).Split('.')[0];

            string frmrange = "";
            string torange = "";
            string mode = "";
            string amountorper = "";
            string totamnt = "";
            string gender = "";

            if (Convert.ToInt32(splvaldt) < 364)
            {
                lblyearval.Visible = true;
                lblyearval.Text = "Year Should be Exactly one year!";
                return;
            }
            else if (Convert.ToInt32(frmyear) > Convert.ToInt32(toyear))
            {
                lblyearval.Visible = true;
                lblyearval.Text = "Start Year Should be greater than End Year!";
                return;
            }
            else if ((Convert.ToInt32(frmyear) == Convert.ToInt32(toyear)) && Convert.ToInt32(frmmon) >= Convert.ToInt32(tomon))
            {
                lblyearval.Visible = true;
                lblyearval.Text = "Please Select a Month!";
                return;
            }
            else
            {
                linkvalue = Convert.ToString(frmmon + "," + frmyear + "-" + tomon + "," + toyear);
                string linkname = "IT Calculation Settings";
                string insq = "if exists(select * from New_InsSettings where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + linkvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkValue,LinkName,user_code,college_code) values ('" + linkvalue + "','" + linkname + "','" + usercode + "','" + collegecode1 + "')";

                string delq = "delete from HR_ITCalculationSettings where collegeCode='" + collegecode1 + "'";
                int delcount = d2.update_method_wo_parameter(delq, "Text");

                if (grditset.Rows.Count > 0)
                {
                    for (int ro = 0; ro < grditset.Rows.Count; ro++)
                    {
                        Label lblfrmrange = (Label)grditset.Rows[ro].FindControl("lbl_frmamnt");
                        Label lbltorange = (Label)grditset.Rows[ro].FindControl("lbl_toamnt");
                        Label lblmode = (Label)grditset.Rows[ro].FindControl("lbl_itmode");
                        Label lblamntorper = (Label)grditset.Rows[ro].FindControl("lbl_itamntorper");
                        Label lblgender = (Label)grditset.Rows[ro].FindControl("lbl_gender");

                        frmrange = Convert.ToString(lblfrmrange.Text);
                        torange = Convert.ToString(lbltorange.Text);
                        mode = Convert.ToString(lblmode.Text);
                        gender = Convert.ToString(lblgender.Text);
                        if (mode == "Amount")
                        {
                            modeval = "0";
                        }
                        else
                        {
                            modeval = "1";
                        }
                        amountorper = Convert.ToString(lblamntorper.Text);
                        if (amountorper.Contains("%"))
                        {
                            totamnt = amountorper.Split('%')[0];
                        }
                        else
                        {
                            totamnt = amountorper;
                        }

                        string insertq = "insert into HR_ITCalculationSettings (FromRange,ToRange,Mode,Amount,collegeCode,sex) values ('" + frmrange + "','" + torange + "','" + modeval + "','" + totamnt + "','" + collegecode1 + "','" + gender + "')";
                        insertcount = d2.update_method_wo_parameter(insertq, "Text");
                    }
                }

                int inscount = d2.update_method_wo_parameter(insq, "Text");
                if (inscount > 0 && insertcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "IT Settings Saved Successfully!";
                    lblyearval.Visible = false;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the From Range and To Range Amount!";
                    lblyearval.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "IT_Calculation.aspx");
        }
    }

    public void bindgrditset()
    {
        try
        {
            string modeval = "";
            string amntorperval = "";
            string gender = "";

            DataTable dt = new DataTable();
            dt.Columns.Add("sno");
            dt.Columns.Add("itfrmamnt");
            dt.Columns.Add("ittoamnt");
            dt.Columns.Add("itmode");
            dt.Columns.Add("itamntorper");
            dt.Columns.Add("gender");
            gender = Convert.ToString(ddlgender.SelectedItem.Text);

            string selitset = "select * from HR_ITCalculationSettings where collegeCode='" + collegecode1 + "' order by FromRange";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selitset, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        dr["sno"] = Convert.ToString(i + 1);
                        dr["itfrmamnt"] = Convert.ToString(ds.Tables[0].Rows[i]["FromRange"]);
                        dr["ittoamnt"] = Convert.ToString(ds.Tables[0].Rows[i]["ToRange"]);
                        if (Convert.ToString(ds.Tables[0].Rows[i]["Mode"]) == "False")
                        {
                            modeval = "Amount";
                        }
                        else
                        {
                            modeval = "Percent";
                        }
                        dr["itmode"] = Convert.ToString(modeval);
                        if (modeval.Trim() == "Amount")
                        {
                            amntorperval = Convert.ToString(ds.Tables[0].Rows[i]["Amount"]);
                        }
                        else
                        {
                            amntorperval = Convert.ToString(ds.Tables[0].Rows[i]["Amount"]) + "%";
                        }
                        dr["itamntorper"] = Convert.ToString(amntorperval);
                        dr["gender"] = Convert.ToString(ds.Tables[0].Rows[i]["sex"]);
                        dt.Rows.Add(dr);
                    }
                    Session["dtitset"] = dt;
                }
            }
            if (dt.Rows.Count > 0)
            {
                grditset.DataSource = dt;
                grditset.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grditset.DataBind();
            }
            grditset.Columns[0].HeaderStyle.Width = 75;
            grditset.Columns[0].ItemStyle.Width = 75;
            grditset.Columns[1].HeaderStyle.Width = 125;
            grditset.Columns[1].ItemStyle.Width = 125;
            grditset.Columns[2].HeaderStyle.Width = 125;
            grditset.Columns[2].ItemStyle.Width = 125;
            grditset.Columns[3].HeaderStyle.Width = 100;
            grditset.Columns[3].ItemStyle.Width = 100;
            grditset.Columns[4].HeaderStyle.Width = 125;
            grditset.Columns[4].ItemStyle.Width = 125;
            grditset.Columns[5].HeaderStyle.Width = 100;
            grditset.Columns[5].ItemStyle.Width = 100;
        }
        catch { }
    }

    public void divitclear()
    {
        txtfrmsecamnt.Text = "";
        txttosecamnt.Text = "";
        ddlamntorpersec.SelectedIndex = 0;
        txtuptosec.Text = "";
    }

    protected void btnexititset_Click(object sender, EventArgs e)
    {
        divgrditset.Visible = false;
        grditset.Visible = false;
        btnview.Visible = true;
    }

    protected void ddlgender_change(object sender, EventArgs e)
    {
        //bindgrditset();
    }

    protected void imagebtnpopcloseadd_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
        btn_go_Click(sender, e);
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

    public void loadfromyear()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            ddl_accfrmyear.Items.Clear();
            ddl_acctoyear.Items.Clear();
            ddlfrmyear.Items.Clear();
            ddltoyear.Items.Clear();
            ds = d2.select_method_wo_parameter("select distinct PayYear from HrPayMonths where College_Code='" + collegecode1 + "' order by PayYear", "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_accfrmyear.DataSource = ds;
                ddl_accfrmyear.DataTextField = "PayYear";
                ddl_accfrmyear.DataValueField = "PayYear";
                ddl_accfrmyear.DataBind();
                ddl_accfrmyear.Items.Insert(0, "Select");

                ddl_acctoyear.DataSource = ds;
                ddl_acctoyear.DataTextField = "PayYear";
                ddl_acctoyear.DataValueField = "PayYear";
                ddl_acctoyear.DataBind();
                ddl_acctoyear.Items.Insert(0, "Select");

                ddlfrmyear.DataSource = ds;
                ddlfrmyear.DataTextField = "PayYear";
                ddlfrmyear.DataValueField = "PayYear";
                ddlfrmyear.DataBind();
                ddlfrmyear.Items.Insert(0, "Select");

                ddltoyear.DataSource = ds;
                ddltoyear.DataTextField = "PayYear";
                ddltoyear.DataValueField = "PayYear";
                ddltoyear.DataBind();
                ddltoyear.Items.Insert(0, "Select");
            }
            else
            {
                ddl_accfrmyear.Items.Insert(0, "Select");
                ddl_acctoyear.Items.Insert(0, "Select");
                ddlfrmyear.Items.Insert(0, "Select");
                ddltoyear.Items.Insert(0, "Select");
            }
            binditaccyear();
        }
        catch (Exception ex) { }
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

    protected void cb_stat_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_stat, cbl_stat, txt_stat, "Status");
    }

    protected void cbl_stat_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stat, cbl_stat, txt_stat, "Status");
    }

    protected void cbl_allowancemultiple_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_allowancemultiple, cbl_allowancemultiple, txt_allowancemultiple, "Allowance");
    }
    protected void cb_allowancemultiple_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_allowancemultiple, cbl_allowancemultiple, txt_allowancemultiple, "Allowance");
    }

    protected void cb_includecity_change(object sender, EventArgs e)
    {
        if (cb_includecity.Checked == true)
        {
            txt_city.Enabled = true;
            bindcity();
        }
        else
        {
            txt_city.Enabled = false;
            cbl_city.Items.Clear();
            txt_city.Text = "--Select--";
        }
    }

    protected void cb_city_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_city, cbl_city, txt_city, "City");
    }

    protected void cbl_city_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_city, cbl_city, txt_city, "City");
    }

    protected void cbl_deduction_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_deduction, cbl_deduction, txt_deduction, "Deduction");
    }

    protected void cb_deduction_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_deduction, cbl_deduction, txt_deduction, "Deduction");
    }

    protected void bindallowance()
    {
        try
        {
            ds.Clear();
            cbl_allowancemultiple.Items.Clear();
            string item = "select allowances from incentives_master where college_code = '" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_allowancemultiple.DataSource = ds;
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    string stafftype = split1[0];
                    if (stafftype.Trim() != "")
                    {
                        cbl_allowancemultiple.Items.Add(stafftype);
                    }
                }
                if (cbl_allowancemultiple.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_allowancemultiple.Items.Count; i++)
                    {
                        cbl_allowancemultiple.Items[i].Selected = true;
                    }
                    txt_allowancemultiple.Text = "Allowance (" + cbl_allowancemultiple.Items.Count + ")";
                    cb_allowancemultiple.Checked = true;
                }
            }
            else
            {
                txt_allowancemultiple.Text = "--Select--";
                cb_allowancemultiple.Checked = false;
            }
        }
        catch { }
    }

    protected void binddeduction()
    {
        try
        {
            ds.Clear();
            cbl_deduction.Items.Clear();
            string item = "select deductions from incentives_master  where college_code = '" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' ";
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
                        cbl_deduction.Items.Add(stafftype);
                    }
                }
                if (cbl_deduction.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_deduction.Items.Count; i++)
                    {
                        cbl_deduction.Items[i].Selected = true;
                    }
                    txt_deduction.Text = "Deduction (" + cbl_deduction.Items.Count + ")";
                    cb_deduction.Checked = true;
                }
            }
            else
            {
                txt_deduction.Text = "--Select--";
                cb_deduction.Checked = false;
            }
        }
        catch { }
    }

    protected void bindcity()
    {
        try
        {
            cbl_city.Items.Clear();
            string q1 = "select textval from textvaltable where  TextCriteria='city' and textval<>'' order by textval";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_city.DataSource = ds;
                cbl_city.DataTextField = "textval";
                cbl_city.DataValueField = "textval";
                cbl_city.DataBind();
            }
        }
        catch { }
    }

    protected void btn_allowance_go_Click(object sender, EventArgs e)
    {
        try
        {
            bindcity();
            string header = "S.No-50/Select-100/Allowances-150/Percentage from Salary-100";
            Fpreadheaderbindmethod(header, FpSpread2, "false");
            int j = 0;
            if (cbl_allowancemultiple.Items.Count > 0)
            {
                for (int i = 0; i < cbl_allowancemultiple.Items.Count; i++)
                {
                    if (cbl_allowancemultiple.Items[i].Selected == true)
                    {
                        j++;
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        cb.AutoPostBack = false;

                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(j);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Columns[0].Locked = true;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = cb;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(cbl_allowancemultiple.Items[i].Text);
                        FpSpread2.Columns[2].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        string getper = d2.GetFunction("select IncludeSalaryPercent from Tax_Calculation_CommonAllowDeduct where AllowDeductType='1' and AllowDeduct='" + Convert.ToString(cbl_allowancemultiple.Items[i].Text) + "'");
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txt;
                        if (getper.Trim() != "")
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = getper;
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "";
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    }
                }
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                FpSpread2.Sheets[0].FrozenRowCount = 0;
                FpSpread2.SaveChanges();
                FpSpread2.Visible = true;
                lbl_errorallowan.Visible = false;
                cb_includecity.Checked = false;
                txt_city.Enabled = false;
            }
            else
            {
                FpSpread2.Visible = false;
                lbl_errorallowan.Visible = false;
                lbl_errorallowan.Text = "Please Select Allowances";
            }
        }
        catch { }
    }

    protected void btn_deduction_go_Click(object sender, EventArgs e)
    {
        try
        {
            string header = "S.No-50/Select-100/Deduction-150";///Percentage from Salary-100
            Fpreadheaderbindmethod(header, FpSpread3, "false");
            int k = 0;
            if (cbl_deduction.Items.Count > 0)
            {
                for (int i = 0; i < cbl_deduction.Items.Count; i++)
                {
                    if (cbl_deduction.Items[i].Selected == true)
                    {
                        k++;
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        cb.AutoPostBack = false;

                        FpSpread3.Sheets[0].RowCount++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread3.Columns[0].Locked = true;

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = cb;

                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(cbl_deduction.Items[i].Text);
                        FpSpread3.Columns[2].Locked = true;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                FpSpread3.Sheets[0].FrozenRowCount = 0;
                FpSpread3.Visible = true;
                lbl_errorallowan.Visible = false;
            }
            else
            {
                FpSpread3.Visible = false;
                lbl_errorallowan.Visible = false;
                lbl_errorallowan.Text = "Please Select Deduction";
            }
        }
        catch { }
    }

    protected void btn_allowancesave_Click(object sender, EventArgs e)
    {
        try
        {
            bool insertcheck = false;
            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                FpSpread2.SaveChanges(); int insert = 0;
                for (int row = 0; row < FpSpread2.Sheets[0].RowCount; row++)
                {
                    int checkval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[row, 1].Value);
                    if (checkval == 1)
                    {
                        string allowdedcut = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 2].Text);
                        string includesalary = Convert.ToString(FpSpread2.Sheets[0].Cells[row, 3].Text);
                        string q1 = "if exists(select * from Tax_Calculation_CommonAllowDeduct where AllowDeductType='1' and AllowDeduct='" + allowdedcut + "') update Tax_Calculation_CommonAllowDeduct set IncludeSalaryPercent='" + includesalary + "' where AllowDeduct='" + allowdedcut + "' and AllowDeductType='1' else insert into Tax_Calculation_CommonAllowDeduct (AllowDeductType,AllowDeduct,IncludeSalaryPercent) values ('1','" + allowdedcut + "','" + includesalary + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                        if (insert != 0)
                        {
                            insertcheck = true;
                        }
                    }
                }
                if (cb_includecity.Checked == true)
                {
                    if (cbl_city.Items.Count > 0)
                    {
                        if (txt_city.Text.Trim() != "")
                        {
                            for (int i = 0; i < cbl_city.Items.Count; i++)
                            {
                                if (cbl_city.Items[i].Selected == true)
                                {
                                    string allowdedcut = Convert.ToString(cbl_city.Items[i].Text);
                                    Double cityper = 0;
                                    Double.TryParse(txt_citypercent.Text.Trim(), out cityper);
                                    string q1 = "if exists(select * from Tax_Calculation_CommonAllowDeduct where AllowDeductType='3' and AllowDeduct='" + allowdedcut + "') update Tax_Calculation_CommonAllowDeduct set IncludeSalaryPercent='" + cityper + "' where AllowDeduct='" + allowdedcut + "' and AllowDeductType='3' else insert into Tax_Calculation_CommonAllowDeduct (AllowDeductType,AllowDeduct,IncludeSalaryPercent) values ('3','" + allowdedcut + "','" + cityper + "')";
                                    insert = d2.update_method_wo_parameter(q1, "Text");
                                    if (insert != 0)
                                    {
                                        insertcheck = true;
                                    }
                                }
                            }
                        }
                    }
                }
                alertpopwindow.Visible = false;
                lblalerterr.Visible = false;
            }
            if (insertcheck == true)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Saved SuccessFully";
                lnkitsetting_click(sender, e);
                txt_citypercent.Text = "";
                txt_city.Enabled = false;
                cb_includecity.Checked = false;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please Select any one Allowance!";
            }
        }
        catch { }
    }

    protected void btn_deductionsave_Click(object sender, EventArgs e)
    {
        try
        {
            bool insertcheck = false;
            if (FpSpread3.Sheets[0].RowCount > 0)
            {
                FpSpread3.SaveChanges(); int insert = 0;
                for (int row = 0; row < FpSpread3.Sheets[0].RowCount; row++)
                {
                    int checkval = Convert.ToInt32(FpSpread3.Sheets[0].Cells[row, 1].Value);
                    if (checkval == 1)
                    {
                        string allowdedcut = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 2].Text);
                        string q1 = "if exists(select * from Tax_Calculation_CommonAllowDeduct where AllowDeductType='2' and AllowDeduct='" + allowdedcut + "') update Tax_Calculation_CommonAllowDeduct set AllowDeduct='" + allowdedcut + "' where AllowDeduct='" + allowdedcut + "' and AllowDeductType='2' else insert into Tax_Calculation_CommonAllowDeduct (AllowDeductType,AllowDeduct) values ('2','" + allowdedcut + "')";
                        insert = d2.update_method_wo_parameter(q1, "Text");
                        if (insert != 0)
                        {
                            insertcheck = true;
                        }
                    }
                }
                alertpopwindow.Visible = false;
                lblalerterr.Visible = false;
            }
            if (insertcheck == true)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Saved SuccessFully";
            }

        }
        catch { }
    }

    public void Fpreadheaderbindmethod(string headername, FarPoint.Web.Spread.FpSpread spreadname, string AutoPostBack)
    {
        try
        {
            string[] header = headername.Split('/');
            int k = 0;
            if (AutoPostBack.Trim().ToUpper() == "TRUE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = true;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (head.Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 50;
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 200;
                        }
                    }
                }
            }
            else if (AutoPostBack.Trim().ToUpper() == "FALSE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = false;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        string[] width = head.Split('-');
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (Convert.ToString(width[0]).Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            lblalerterr.Font.Size = FontUnit.Smaller;
            lblalerterr.Text = ex.ToString();
        }
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
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
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
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsTextnew(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[j].Text));
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