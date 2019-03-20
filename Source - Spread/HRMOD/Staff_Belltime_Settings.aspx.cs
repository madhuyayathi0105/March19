using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class Staff_Belltime_Settings : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;

    DataSet ds = new DataSet();
    InsproDirectAccess DirAccess = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
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
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                    clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                binddept();
                designation();
                category();
                stafftype();
                bindYear();
                txtFrmDt.Attributes.Add("readonly", "readonly");
                txtToDt.Attributes.Add("readonly", "readonly");
                string SelQ = "select Convert(varchar(10),From_Date,103) as From_Date,Convert(varchar(10),To_Date,103) as To_Date from HRPayMonths where PayMonthNum='" + Convert.ToString(ddlMon.SelectedItem.Value) + "' and YEAR(To_Date)='" + Convert.ToString(ddlYear.SelectedItem.Text) + "' and college_code='" + clgcode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtFrmDt.Enabled = true;
                    txtToDt.Enabled = true;
                    txtFrmDt.Text = Convert.ToString(ds.Tables[0].Rows[0]["From_Date"]);
                    txtToDt.Text = Convert.ToString(ds.Tables[0].Rows[0]["To_Date"]);
                    calFrmDt.StartDate = GetMonFrstDate(txtFrmDt.Text);
                    calFrmDt.EndDate = GetMonFrstDate(txtToDt.Text);
                    calToDt.StartDate = GetMonFrstDate(txtFrmDt.Text);
                    calToDt.EndDate = GetMonFrstDate(txtToDt.Text);
                }
                else
                {
                    txtFrmDt.Enabled = false;
                    txtToDt.Enabled = false;
                    txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                ddlMin.Items.Clear();
                for (int i = 0; i <= 59; i++)
                {
                    if (i == 0)
                    {
                        ddlMin.Items.Add("00");
                    }
                    else
                    {
                        if (Convert.ToString(i).Length == 1)
                        {
                            ddlMin.Items.Add("0" + Convert.ToString(i));
                        }
                        else
                        {
                            ddlMin.Items.Add(Convert.ToString(i));
                        }
                    }
                }
            }
            if (ddlcollege.Items.Count > 0)
                clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
            lblMainErr.Visible = false;
            lblsmserror.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        try
        {
            clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
            binddept();
            designation();
            category();
            stafftype();
            bindYear();
            string SelQ = "select Convert(varchar(10),From_Date,103) as From_Date,Convert(varchar(10),To_Date,103) as To_Date from HRPayMonths where PayMonthNum='" + Convert.ToString(ddlMon.SelectedItem.Value) + "' and YEAR(To_Date)='" + Convert.ToString(ddlYear.SelectedItem.Text) + "' and college_code='" + clgcode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtFrmDt.Enabled = true;
                txtToDt.Enabled = true;
                txtFrmDt.Text = Convert.ToString(ds.Tables[0].Rows[0]["From_Date"]);
                txtToDt.Text = Convert.ToString(ds.Tables[0].Rows[0]["To_Date"]);
                calFrmDt.StartDate = GetMonFrstDate(txtFrmDt.Text);
                calFrmDt.EndDate = GetMonFrstDate(txtToDt.Text);
                calToDt.StartDate = GetMonFrstDate(txtFrmDt.Text);
                calToDt.EndDate = GetMonFrstDate(txtToDt.Text);
            }
            else
            {
                txtFrmDt.Enabled = false;
                txtToDt.Enabled = false;
                txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            FpSpread.Visible = false;
            rprint.Visible = false;
            fldTime.Visible = false;
            lblMainErr.Visible = false;
        }
        catch { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '" + prefixText + "%' and college_code='" + clgcode + "'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '" + prefixText + "%' and college_code='" + clgcode + "'";
        name = ws.Getname(query);
        return name;
    }

    private DateTime GetMonFrstDate(string Date)
    {
        DateTime dtGet = new DateTime();
        string[] splDt = new string[2];
        try
        {
            splDt = Date.Split('/');
            dtGet = Convert.ToDateTime(splDt[1] + "/" + splDt[0] + "/" + splDt[2]);
        }
        catch { }
        return dtGet;
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

    protected void ddlMon_Change(object sender, EventArgs e)
    {
        string SelQ = "select Convert(varchar(10),From_Date,103) as From_Date,Convert(varchar(10),To_Date,103) as To_Date from HRPayMonths where PayMonthNum='" + Convert.ToString(ddlMon.SelectedItem.Value) + "' and YEAR(To_Date)='" + Convert.ToString(ddlYear.SelectedItem.Text) + "' and college_code='" + clgcode + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(SelQ, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtFrmDt.Enabled = true;
            txtToDt.Enabled = true;
            txtFrmDt.Text = Convert.ToString(ds.Tables[0].Rows[0]["From_Date"]);
            txtToDt.Text = Convert.ToString(ds.Tables[0].Rows[0]["To_Date"]);
            calFrmDt.StartDate = GetMonFrstDate(txtFrmDt.Text);
            calFrmDt.EndDate = GetMonFrstDate(txtToDt.Text);
            calToDt.StartDate = GetMonFrstDate(txtFrmDt.Text);
            calToDt.EndDate = GetMonFrstDate(txtToDt.Text);
        }
        else
        {
            txtFrmDt.Enabled = false;
            txtToDt.Enabled = false;
            txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    protected void ddlYear_Change(object sender, EventArgs e)
    {
        string SelQ = "select Convert(varchar(10),From_Date,103) as From_Date,Convert(varchar(10),To_Date,103) as To_Date from HRPayMonths where PayMonthNum='" + Convert.ToString(ddlMon.SelectedItem.Value) + "' and YEAR(To_Date)='" + Convert.ToString(ddlYear.SelectedItem.Text) + "' and college_code='" + clgcode + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(SelQ, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtFrmDt.Enabled = true;
            txtToDt.Enabled = true;
            txtFrmDt.Text = Convert.ToString(ds.Tables[0].Rows[0]["From_Date"]);
            txtToDt.Text = Convert.ToString(ds.Tables[0].Rows[0]["To_Date"]);
            calFrmDt.StartDate = GetMonFrstDate(txtFrmDt.Text);
            calFrmDt.EndDate = GetMonFrstDate(txtToDt.Text);
            calToDt.StartDate = GetMonFrstDate(txtFrmDt.Text);
            calToDt.EndDate = GetMonFrstDate(txtToDt.Text);
        }
        else
        {
            txtFrmDt.Enabled = false;
            txtToDt.Enabled = false;
            txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    protected void txt_scode_Change(object sender, EventArgs e)
    {
        txt_sname.Text = "";
    }

    protected void txt_sname_Change(object sender, EventArgs e)
    {
        txt_scode.Text = "";
    }

    protected void txtFrmDt_Change(object sender, EventArgs e)
    {
        try
        {
            if (GetMonFrstDate(txtFrmDt.Text) > GetMonFrstDate(txtToDt.Text))
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "From Date Should be less than To Date!";
                FpSpread.Visible = false;
                rprint.Visible = false;
                fldTime.Visible = false;
            }
        }
        catch { }
    }

    protected void txtToDt_Change(object sender, EventArgs e)
    {
        try
        {
            if (GetMonFrstDate(txtFrmDt.Text) > GetMonFrstDate(txtToDt.Text))
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "To Date Should be greater than From Date!";
                FpSpread.Visible = false;
                rprint.Visible = false;
                fldTime.Visible = false;
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
                for (int sel = 1; sel < FpSpread.Sheets[0].RowCount; sel++)
                {
                    FpSpread.Sheets[0].Cells[sel, 1].Value = 1;
                }
            }
            else
            {
                for (int sel = 1; sel < FpSpread.Sheets[0].RowCount; sel++)
                {
                    FpSpread.Sheets[0].Cells[sel, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread.Visible = false;
            rprint.Visible = false;
            lblMainErr.Visible = false;
            fldTime.Visible = false;

            string selectquery = string.Empty;
            string scode = txt_scode.Text;
            string sname = txt_sname.Text;
            string deptCode = GetSelectedItemsValueAsString(cbl_dept);
            string desigCode = GetSelectedItemsValueAsString(cbl_desig);
            string CatCode = GetSelectedItemsValueAsString(cbl_staffc);
            string StfType = GetSelectedItemsText(cbl_stype);

            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;
            chkcell.AutoPostBack = false;

            if (txtFrmDt.Enabled == false || txtToDt.Enabled == false)
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Valid HR Month!";
                return;
            }

            if (GetMonFrstDate(txtFrmDt.Text) > GetMonFrstDate(txtToDt.Text))
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "From Date Should be less than To Date!";
                return;
            }

            if (txt_scode.Text != "")
            {
                selectquery = "select sm.staff_code,sm.staff_name,h.dept_acronym,desig.desig_acronym,desig.desig_name,st.stftype,sc.category_name,sc.category_code from staffmaster sm,staff_appl_master sa,hrdept_master h,desig_master desig,staffcategorizer sc,stafftrans st where sa.appl_no=sm.appl_no and sm.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=desig.desig_code and st.category_code=sc.category_code and sm.college_code=h.college_code and sm.college_code=desig.collegeCode and sm.college_code=sc.college_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code = '" + clgcode + "' and st.staff_code='" + scode + "'";
            }
            else if (txt_sname.Text != "")
            {
                selectquery = "select sm.staff_code,sm.staff_name,h.dept_acronym,desig.desig_acronym,desig.desig_name,st.stftype,sc.category_name,sc.category_code from staffmaster sm,staff_appl_master sa,hrdept_master h,desig_master desig,staffcategorizer sc,stafftrans st where sa.appl_no=sm.appl_no and sm.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=desig.desig_code and st.category_code=sc.category_code and sm.college_code=h.college_code and sm.college_code=desig.collegeCode and sm.college_code=sc.college_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code = '" + clgcode + "' and sm.staff_name='" + sname + "'";
            }
            else
            {
                selectquery = "select sm.staff_code,sm.staff_name,h.dept_acronym,desig.desig_acronym,desig.desig_name,st.stftype,sc.category_name,sc.category_code from staffmaster sm,staff_appl_master sa,hrdept_master h,desig_master desig,staffcategorizer sc,stafftrans st where sa.appl_no=sm.appl_no and sm.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=desig.desig_code and st.category_code=sc.category_code and sm.college_code=h.college_code and sm.college_code=desig.collegeCode and sm.college_code=sc.college_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code = '" + clgcode + "' and h.dept_code in('" + deptCode + "') and desig.desig_code in('" + desigCode + "') and sc.category_code in('" + CatCode + "') and st.stftype in('" + StfType + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                LoadHeader();

                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkall;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                {
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sk + 1);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[sk]["staff_code"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sk]["staff_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[sk]["dept_acronym"]) + " - " + Convert.ToString(ds.Tables[0].Rows[sk]["desig_name"]) + " - " + Convert.ToString(ds.Tables[0].Rows[sk]["category_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = "";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                }
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                FpSpread.Sheets[0].FrozenRowCount = 1;
                FpSpread.Visible = true;
                rprint.Visible = true;
                fldTime.Visible = true;
            }
            else
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "No Record(s) Found!";
            }
        }
        catch { }
    }

    private void LoadHeader()
    {
        FpSpread.CommandBar.Visible = false;
        FpSpread.RowHeader.Visible = false;
        FpSpread.Sheets[0].AutoPostBack = false;
        FpSpread.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread.Sheets[0].RowCount = 0;
        FpSpread.Sheets[0].ColumnCount = 6;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Font.Bold = true;
        darkstyle.HorizontalAlign = HorizontalAlign.Center;
        darkstyle.ForeColor = Color.Black;
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");

        FpSpread.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
        FpSpread.Columns[0].Width = 75;
        FpSpread.Columns[0].Locked = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
        FpSpread.Columns[1].Width = 75;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
        FpSpread.Columns[2].Width = 100;
        FpSpread.Columns[2].Locked = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
        FpSpread.Columns[3].Width = 250;
        FpSpread.Columns[3].Locked = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
        FpSpread.Columns[4].Width = 300;
        FpSpread.Columns[4].Locked = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "In Time";
        FpSpread.Columns[5].Width = 150;
        FpSpread.Columns[5].Locked = true;
    }

    protected void btnSetTime_Click(object sender, EventArgs e)
    {
        try
        {
            lblMainErr.Visible = false;
            string InTime = string.Empty;
            InTime = Convert.ToString(ddlHr.SelectedItem.Text) + ":" + Convert.ToString(ddlMin.SelectedItem.Text) + " " + Convert.ToString(ddlMer.SelectedItem.Text);
            if (CheckedOK())
            {
                for (int st = 1; st < FpSpread.Sheets[0].RowCount; st++)
                {
                    FpSpread.SaveChanges();
                    byte Check = Convert.ToByte(FpSpread.Sheets[0].Cells[st, 1].Value);
                    if (Check == 1)
                    {
                        FpSpread.Sheets[0].Cells[st, 5].Text = InTime;
                        FpSpread.Sheets[0].Cells[st, 1].Value = 0;
                    }
                }
            }
            else
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any Staff!";
            }
        }
        catch { }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            lblMainErr.Visible = false;
            if (CheckedOK())
            {
                for (int st = 1; st < FpSpread.Sheets[0].RowCount; st++)
                {
                    FpSpread.SaveChanges();
                    byte Check = Convert.ToByte(FpSpread.Sheets[0].Cells[st, 1].Value);
                    if (Check == 1)
                    {
                        FpSpread.Sheets[0].Cells[st, 5].Text = "";
                        FpSpread.Sheets[0].Cells[st, 1].Value = 0;
                    }
                }
            }
            else
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any Staff!";
            }
        }
        catch { }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblMainErr.Visible = false;
            string MyErrTxt = string.Empty;
            string Intime = string.Empty;
            string StaffCode = string.Empty;
            string StaffApplID = string.Empty;
            string InsQ = string.Empty;
            int insCount = 0;
            string PayMonth = Convert.ToString(ddlMon.SelectedItem.Value);
            string PayYear = Convert.ToString(ddlYear.SelectedItem.Text);
            DateTime dtFrm = new DateTime();
            DateTime dtTo = new DateTime();
            dtFrm = GetMonFrstDate(txtFrmDt.Text);
            dtTo = GetMonFrstDate(txtToDt.Text);
            DataSet MyDs = new DataSet();
            DataView dvnew = new DataView();

            if (CheckedOK())
            {
                if (CheckedStr(ref MyErrTxt))
                {
                    string GetApplID = "select appl_id,sm.staff_code from staff_appl_master sa,staffmaster sm,stafftrans st where sa.appl_no=sm.appl_no and st.staff_code=sm.staff_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code='" + clgcode + "'";
                    MyDs.Clear();
                    MyDs = d2.select_method_wo_parameter(GetApplID, "Text");
                    for (int st = 1; st < FpSpread.Sheets[0].RowCount; st++)
                    {
                        FpSpread.SaveChanges();
                        byte Check = Convert.ToByte(FpSpread.Sheets[0].Cells[st, 1].Value);
                        if (Check == 1)
                        {
                            StaffCode = Convert.ToString(FpSpread.Sheets[0].Cells[st, 2].Text);
                            Intime = Convert.ToString(FpSpread.Sheets[0].Cells[st, 5].Text);
                            if (MyDs.Tables.Count > 0 && MyDs.Tables[0].Rows.Count > 0)
                            {
                                MyDs.Tables[0].DefaultView.RowFilter = " Staff_Code='" + StaffCode + "'";
                                dvnew = MyDs.Tables[0].DefaultView;
                                if (dvnew.Count > 0)
                                    StaffApplID = Convert.ToString(dvnew[0]["appl_id"]);
                            }
                            InsQ = "if exists(select * from Staff_InOut_BellTime_Individual where StaffApplID='" + StaffApplID + "' and StaffCode='" + StaffCode + "' and PayMonth='" + PayMonth + "' and Payyear='" + PayYear + "') Update Staff_InOut_BellTime_Individual set IN_Time='" + Intime + "',FromDate='" + dtFrm.ToString("MM/dd/yyyy") + "',Todate='" + dtTo.ToString("MM/dd/yyyy") + "' where StaffApplID='" + StaffApplID + "' and StaffCode='" + StaffCode + "' and PayMonth='" + PayMonth + "' and Payyear='" + PayYear + "' else insert into Staff_InOut_BellTime_Individual (StaffApplID,StaffCode,IN_Time,FromDate,Todate,PayMonth,Payyear) values ('" + StaffApplID + "','" + StaffCode + "','" + Intime + "','" + dtFrm.ToString("MM/dd/yyyy") + "','" + dtTo.ToString("MM/dd/yyyy") + "','" + PayMonth + "','" + PayYear + "')";
                            int myCount = d2.update_method_wo_parameter(InsQ, "Text");
                            if (myCount > 0)
                                insCount += 1;
                        }
                    }
                    if (insCount > 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Bell Time Settings Saved Successfully!";
                    }
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = MyErrTxt;
                }
            }
            else
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any Staff!";
            }
        }
        catch { }
    }

    private bool CheckedOK()
    {
        bool EntryFlag = false;
        try
        {
            FpSpread.SaveChanges();
            for (int s = 1; s < FpSpread.Sheets[0].RowCount; s++)
            {
                byte Check = Convert.ToByte(FpSpread.Sheets[0].Cells[s, 1].Value);
                if (Check == 1)
                    EntryFlag = true;
            }
        }
        catch { }
        return EntryFlag;
    }

    private bool CheckedStr(ref string MyErr)
    {
        bool MyFlag = true;
        try
        {
            FpSpread.SaveChanges();
            for (int s = 1; s < FpSpread.Sheets[0].RowCount; s++)
            {
                byte Check = Convert.ToByte(FpSpread.Sheets[0].Cells[s, 1].Value);
                if (Check == 1)
                {
                    string InTime = Convert.ToString(FpSpread.Sheets[0].Cells[s, 5].Text);
                    string StaffName = Convert.ToString(FpSpread.Sheets[0].Cells[s, 3].Text);
                    if (String.IsNullOrEmpty(InTime))
                    {
                        MyErr = "Please Set the InTime for the Staff '" + StaffName + "'!";
                        MyFlag = false;
                        return MyFlag;
                    }
                }
            }
        }
        catch { }
        return MyFlag;
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
            string degreedetails = "Staff Bell Time Settings";
            string pagename = "Staff_Belltime_Settings.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    private void bindYear()
    {
        try
        {
            ddlYear.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("select distinct year(To_Date) as year from HrPayMonths where College_Code ='" + clgcode + "' order by year asc", "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "year";
                ddlYear.DataValueField = "year";
                ddlYear.DataBind();
            }
        }
        catch { }
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
                columnfield = " and group_code='" + group_code + "'";
            else
                columnfield = " and user_code='" + Session["usercode"] + "'";

            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
            txt_dept.Text = "--Select--";
            cb_dept.Checked = false;
            string item = "select distinct dept_code,dept_name from hrdept_master where college_code = '" + clgcode + "' order by dept_name";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
        }
        catch { }
    }

    protected void designation()
    {
        try
        {
            ds.Clear();
            cbl_desig.Items.Clear();
            txt_desig.Text = "--Select--";
            cb_desig.Checked = false;
            string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + clgcode + "' order by desig_name";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
        }
        catch { }
    }

    protected void category()
    {
        try
        {
            ds.Clear();
            cbl_staffc.Items.Clear();
            txt_staffc.Text = "--Select--";
            cb_staffc.Checked = false;
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + clgcode + "' order by category_Name";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
        }
        catch { }
    }

    protected void stafftype()
    {
        try
        {
            ds.Clear();
            cbl_stype.Items.Clear();
            txt_stype.Text = "--Select--";
            cb_stype.Checked = false;
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + clgcode + "' order by stftype";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Text));
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
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }
}