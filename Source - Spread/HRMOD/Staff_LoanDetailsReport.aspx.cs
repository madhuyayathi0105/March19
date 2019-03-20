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

public partial class Staff_LoanDetailsReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dsYr = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataSet ds11 = new DataSet();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    static string clgcode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    string d = "";

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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                binddept();
                designation();
                category();
                stafftype();
                cbLoanType.Checked = true;
                txtLoanType.Text = "Loan Type (2)";
                ddlMon.Items.Clear();
                string str = "select PayMonth,PayMonthNum from HrPayMonths where College_Code='" + collegecode + "' and SelStatus='1' order by From_Date";
                ds.Clear();
                ds = d2.select_method_wo_parameter(str, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int mon = 0; mon < ds.Tables[0].Rows.Count; mon++)
                    {
                        ddlMon.Items.Add(new ListItem(GetMonTxt(Convert.ToInt32(ds.Tables[0].Rows[mon]["PayMonthNum"])), Convert.ToString(ds.Tables[0].Rows[mon]["PayMonthNum"])));
                    }
                }
                ddlMon.Items.Insert(0, "---Select---");
                ddlToMon.Items.Insert(0, "---Select---");
                year(d);
                year1(d);
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            lblMainErr.Visible = false;
            lblsmserror.Visible = false;
        }
        catch (Exception ex) { }
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

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
        binddept();
        designation();
        category();
        stafftype();
        ddlMon.Items.Clear();
        string str = "select PayMonth,PayMonthNum from HrPayMonths where College_Code='" + clgcode + "' and SelStatus='1' order by From_Date";
        ds.Clear();
        ds = d2.select_method_wo_parameter(str, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int mon = 0; mon < ds.Tables[0].Rows.Count; mon++)
            {
                ddlMon.Items.Add(new ListItem(GetMonTxt(Convert.ToInt32(ds.Tables[0].Rows[mon]["PayMonthNum"])), Convert.ToString(ds.Tables[0].Rows[mon]["PayMonthNum"])));
            }
        }
        ddlMon.Items.Insert(0, "---Select---");
        ddlToMon.Items.Insert(0, "---Select---");
        year(d);
        year1(d);
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
        chkchange(cb_stype, cbl_stype, txt_stype, "Staff Type");
    }

    protected void cbl_stype_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stype, cbl_stype, txt_stype, "Staff Type");
    }

    protected void cbLoanType_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbLoanType, cblLoanType, txtLoanType, "Loan Type");
    }

    protected void cblLoanType_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbLoanType, cblLoanType, txtLoanType, "Loan Type");
    }

    protected void ddlMon_Change(object sender, EventArgs e)
    {
        try
        {
            ddlToMon.Items.Clear();
            string str = "select PayMonth,PayMonthNum,From_Date from HrPayMonths where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and SelStatus='1'";
            ds = d2.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string mon = ds.Tables[0].Rows[i]["PayMonthNum"].ToString();
                    if (ddlMon.SelectedItem.Value.ToString() == mon)
                    {
                        string date = Convert.ToString(ddlMon.SelectedItem.Value);
                        for (int j = i; j < ds.Tables[0].Rows.Count; j++)
                        {
                            ddlToMon.Items.Insert(count, new ListItem(GetMonTxt(Convert.ToInt32(ds.Tables[0].Rows[j]["PayMonthNum"].ToString())), ds.Tables[0].Rows[j]["PayMonthNum"].ToString()));
                            count++;
                        }
                        year(date);
                    }
                }
                ddlToMon.Items.Insert(0, "---Select---");
            }
        }
        catch (Exception ex) { }
    }

    protected void ddlYear_Change(object sender, EventArgs e)
    {
        try
        {
            ddlToYear.Items.Clear();
            string str = "select distinct year(To_Date) as year from HrPayMonths  where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and SelStatus='1' order by year asc";
            ds = d2.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var mon = ds.Tables[0].Rows[i]["year"].ToString();
                    if (ddlYear.SelectedItem.Text.ToString() == mon)
                    {
                        for (int j = i; j < ds.Tables[0].Rows.Count; j++)
                        {
                            ddlToYear.Items.Add(ds.Tables[0].Rows[j]["year"].ToString());
                        }
                        ddlToYear.Items.Insert(0, "Select");
                    }
                }

            }
        }
        catch (Exception ex) { }
    }

    protected void ddlToMon_Change(object sender, EventArgs e)
    {
        try
        {
            year1(ddlToMon.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }

    protected void txt_scode_Change(object sender, EventArgs e)
    {
        txt_sname.Text = "";
    }

    protected void txt_sname_Change(object sender, EventArgs e)
    {
        txt_scode.Text = "";
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread.Visible = false;
            lblMainErr.Visible = false;
            rprint.Visible = false;

            string ClgCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string DeptCode = GetSelectedItemsValueAsString(cbl_dept);
            string DesigCode = GetSelectedItemsValueAsString(cbl_desig);
            string CatCode = GetSelectedItemsValueAsString(cbl_staffc);
            string StfType = GetSelectedItemsText(cbl_stype);
            string FrmMon = Convert.ToString(ddlMon.SelectedItem.Value);
            string FrmYear = Convert.ToString(ddlYear.SelectedItem.Text);
            string ToMon = Convert.ToString(ddlToMon.SelectedItem.Value);
            string ToYear = Convert.ToString(ddlToYear.SelectedItem.Text);
            string LoanType = GetSelectedItemsValueAsString(cblLoanType);
            string SelQ = string.Empty;
            DataView dvLoanDet = new DataView();
            DataView dvLoanName = new DataView();
            DataView dvLoanAmnt = new DataView();
            DataView dvLoanPayDet = new DataView();
            Double PaidLoanAmnt = 0;
            Double TotPaidAmnt = 0;
            Double LoanAmount = 0;
            Double TotAmnt = 0;
            Double GrandTotAmnt = 0;

            if (String.IsNullOrEmpty(LoanType))
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Any Loan Type!";
                return;
            }

            if (ddlMon.SelectedIndex != 0 && ddlYear.Items.Count > 0 && ddlToMon.SelectedIndex != 0 && ddlToYear.Items.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(txt_scode.Text.Trim())))
                {
                    SelQ = "select sm.staff_name,sc.category_name,st.stftype,sm.staff_Code from staffmaster sm,stafftrans st,hrdept_master h,desig_master de,staffcategorizer sc where sm.staff_code=st.staff_code and sm.college_code=h.college_code and sm.college_code=de.collegeCode and sm.college_code=sc.college_code and st.dept_code=h.dept_code and st.desig_code=de.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code='" + ClgCode + "' and sm.staff_code='" + Convert.ToString(txt_scode.Text) + "'";
                    SelQ = SelQ + " select Staff_Code,LoanType,LoanName,PolicyName,LoanCode,LoanAmount,PolicyAmt,IsInterest from StaffLoanDet where IsActive='1' and LoanType in('" + LoanType + "') and Staff_Code='" + Convert.ToString(txt_scode.Text) + "'";
                    SelQ = SelQ + "  select TextCode,TextVal from TextValTable where (TextCode in(select PolicyName from StaffLoanDet) or TextCode in(select LoanName from StaffLoanDet)) and college_code='" + ClgCode + "'";
                    SelQ = SelQ + " select Staff_Code,EMIAmt,IntAmt,PayMonth,PayYear,LoanCode from StaffLoanPayDet";
                }
                else if (!String.IsNullOrEmpty(Convert.ToString(txt_sname.Text.Trim())))
                {
                    SelQ = "select sm.staff_name,sc.category_name,st.stftype,sm.staff_Code from staffmaster sm,stafftrans st,hrdept_master h,desig_master de,staffcategorizer sc where sm.staff_code=st.staff_code and sm.college_code=h.college_code and sm.college_code=de.collegeCode and sm.college_code=sc.college_code and st.dept_code=h.dept_code and st.desig_code=de.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code='" + ClgCode + "' and sm.staff_name='" + Convert.ToString(txt_sname.Text) + "'";
                    SelQ = SelQ + " select Staff_Code,LoanType,LoanName,PolicyName,LoanCode,LoanAmount,PolicyAmt,IsInterest from StaffLoanDet where IsActive='1' and LoanType in('" + LoanType + "')";
                    SelQ = SelQ + "  select TextCode,TextVal from TextValTable where (TextCode in(select PolicyName from StaffLoanDet) or TextCode in(select LoanName from StaffLoanDet)) and college_code='" + ClgCode + "'";
                    SelQ = SelQ + " select Staff_Code,EMIAmt,IntAmt,PayMonth,PayYear,LoanCode from StaffLoanPayDet";
                }
                else
                {
                    if (String.IsNullOrEmpty(DeptCode.Trim()))
                    {
                        lblMainErr.Visible = true;
                        lblMainErr.Text = "Please Select Any Department!";
                        return;
                    }
                    if (String.IsNullOrEmpty(DesigCode.Trim()))
                    {
                        lblMainErr.Visible = true;
                        lblMainErr.Text = "Please Select Any Designation!";
                        return;
                    }
                    if (String.IsNullOrEmpty(CatCode.Trim()))
                    {
                        lblMainErr.Visible = true;
                        lblMainErr.Text = "Please Select Any Category!";
                        return;
                    }
                    if (String.IsNullOrEmpty(StfType.Trim()))
                    {
                        lblMainErr.Visible = true;
                        lblMainErr.Text = "Please Select Any Staff Type!";
                        return;
                    }
                    SelQ = "select sm.staff_name,sc.category_name,st.stftype,sm.staff_Code from staffmaster sm,stafftrans st,hrdept_master h,desig_master de,staffcategorizer sc where sm.staff_code=st.staff_code and sm.college_code=h.college_code and sm.college_code=de.collegeCode and sm.college_code=sc.college_code and st.dept_code=h.dept_code and st.desig_code=de.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and h.dept_Code in('" + DeptCode + "') and de.desig_Code in('" + DesigCode + "') and st.category_Code in('" + CatCode + "') and st.StfType in('" + StfType + "') and sm.college_code='" + ClgCode + "'";
                    SelQ = SelQ + " select Staff_Code,LoanType,LoanName,PolicyName,LoanCode,LoanAmount,PolicyAmt,IsInterest from StaffLoanDet where IsActive='1' and LoanType in('" + LoanType + "')";
                    SelQ = SelQ + "  select TextCode,TextVal from TextValTable where (TextCode in(select PolicyName from StaffLoanDet) or TextCode in(select LoanName from StaffLoanDet)) and college_code='" + ClgCode + "'";
                    SelQ = SelQ + " select Staff_Code,EMIAmt,IntAmt,PayMonth,PayYear,LoanCode from StaffLoanPayDet";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    LoadHeader();
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        PaidLoanAmnt = 0;
                        TotPaidAmnt = 0;
                        LoanAmount = 0;
                        FpSpread.Sheets[0].RowCount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Staff_Name"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["StfType"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["category_name"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                        ds.Tables[1].DefaultView.RowFilter = " Staff_Code='" + Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]) + "'";
                        dvLoanDet = ds.Tables[1].DefaultView;
                        if (dvLoanDet.Count > 0)
                        {
                            if (dvLoanDet.Count == 1)
                            {
                                if (Convert.ToString(dvLoanDet[0]["LoanType"]) == "0")
                                {
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = "Loan";
                                    ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvLoanDet[0]["LoanName"]) + "'";
                                    dvLoanName = ds.Tables[2].DefaultView;
                                }
                                else if (Convert.ToString(dvLoanDet[0]["LoanType"]) == "1")
                                {
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = "Policy";
                                    ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvLoanDet[0]["PolicyName"]) + "'";
                                    dvLoanName = ds.Tables[2].DefaultView;
                                }
                                else
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = "";

                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                                if (dvLoanName.Count > 0)
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvLoanName[0]["TextVal"]);
                                else
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = "";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                                if (Convert.ToString(dvLoanDet[0]["LoanType"]) == "0")
                                {
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dvLoanDet[0]["LoanAmount"]);
                                    Double.TryParse(Convert.ToString(dvLoanDet[0]["LoanAmount"]), out LoanAmount);
                                }
                                else if (Convert.ToString(dvLoanDet[0]["LoanType"]) == "1")
                                {
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dvLoanDet[0]["PolicyAmt"]);
                                    Double.TryParse(Convert.ToString(dvLoanDet[0]["PolicyAmt"]), out LoanAmount);
                                }
                                else
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = "0";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

                                for (int fpcol = 7; fpcol < FpSpread.Sheets[0].ColumnCount - 2; fpcol++)
                                {
                                    string GetMonYear = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, fpcol].Tag);
                                    string[] splMonYr = new string[2];
                                    if (!String.IsNullOrEmpty(GetMonYear) && GetMonYear.Contains(','))
                                    {
                                        splMonYr = GetMonYear.Split(',');
                                        ds.Tables[3].DefaultView.RowFilter = " Staff_Code='" + Convert.ToString(dvLoanDet[0]["Staff_Code"]) + "' and PayMonth='" + splMonYr[0] + "' and PayYear='" + splMonYr[1] + "' and LoanCode='" + Convert.ToString(dvLoanDet[0]["LoanCode"]) + "'";
                                        dvLoanAmnt = ds.Tables[3].DefaultView;
                                        if (dvLoanAmnt.Count > 0)
                                        {
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].Text = Convert.ToString(dvLoanAmnt[0]["EMIAmt"]);
                                            Double.TryParse(Convert.ToString(dvLoanAmnt[0]["EMIAmt"]), out PaidLoanAmnt);
                                            TotPaidAmnt = TotPaidAmnt + PaidLoanAmnt;
                                        }
                                        else
                                        {
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].Text = "";
                                            PaidLoanAmnt = 0;
                                        }
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].Font.Name = "Book Antiqua";
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].Font.Size = FontUnit.Medium;
                                    }
                                }
                                if (TotPaidAmnt > 0)
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Text = Convert.ToString(TotPaidAmnt);
                                else
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Text = "";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;

                                if (LoanAmount > TotPaidAmnt)
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(LoanAmount - TotPaidAmnt);
                                else
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Text = "";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            }
                            else
                            {
                                for (int mydv1 = 0; mydv1 < dvLoanDet.Count; mydv1++)
                                {
                                    PaidLoanAmnt = 0;
                                    TotPaidAmnt = 0;
                                    LoanAmount = 0;
                                    if (mydv1 > 0)
                                        FpSpread.Sheets[0].RowCount++;

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Staff_Name"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["StfType"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["category_name"]);
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                                    if (Convert.ToString(dvLoanDet[mydv1]["LoanType"]) == "0")
                                    {
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = "Loan";
                                        ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvLoanDet[mydv1]["LoanName"]) + "'";
                                        dvLoanName = ds.Tables[2].DefaultView;
                                    }
                                    else if (Convert.ToString(dvLoanDet[mydv1]["LoanType"]) == "1")
                                    {
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = "Policy";
                                        ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvLoanDet[mydv1]["PolicyName"]) + "'";
                                        dvLoanName = ds.Tables[2].DefaultView;
                                    }
                                    else
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = "";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                                    if (dvLoanName.Count > 0)
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvLoanName[0]["TextVal"]);
                                    else
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = "";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                                    if (Convert.ToString(dvLoanDet[mydv1]["LoanType"]) == "0")
                                    {
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dvLoanDet[mydv1]["LoanAmount"]);
                                        Double.TryParse(Convert.ToString(dvLoanDet[mydv1]["LoanAmount"]), out LoanAmount);
                                    }
                                    else if (Convert.ToString(dvLoanDet[mydv1]["LoanType"]) == "1")
                                    {
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dvLoanDet[mydv1]["PolicyAmt"]);
                                        Double.TryParse(Convert.ToString(dvLoanDet[mydv1]["PolicyAmt"]), out LoanAmount);
                                    }
                                    else
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = "0";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

                                    for (int fpcol = 7; fpcol < FpSpread.Sheets[0].ColumnCount - 2; fpcol++)
                                    {
                                        string GetMonYear = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, fpcol].Tag);
                                        string[] splMonYr = new string[2];
                                        if (!String.IsNullOrEmpty(GetMonYear) && GetMonYear.Contains(','))
                                        {
                                            splMonYr = GetMonYear.Split(',');
                                            ds.Tables[3].DefaultView.RowFilter = " Staff_Code='" + Convert.ToString(dvLoanDet[mydv1]["Staff_Code"]) + "' and PayMonth='" + splMonYr[0] + "' and PayYear='" + splMonYr[1] + "' and LoanCode='" + Convert.ToString(dvLoanDet[mydv1]["LoanCode"]) + "'";
                                            dvLoanAmnt = ds.Tables[3].DefaultView;
                                            if (dvLoanAmnt.Count > 0)
                                            {
                                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].Text = Convert.ToString(dvLoanAmnt[0]["EMIAmt"]);
                                                Double.TryParse(Convert.ToString(dvLoanAmnt[0]["EMIAmt"]), out PaidLoanAmnt);
                                                TotPaidAmnt = TotPaidAmnt + PaidLoanAmnt;
                                            }
                                            else
                                            {
                                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].Text = "";
                                                PaidLoanAmnt = 0;
                                            }
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].Font.Name = "Book Antiqua";
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, fpcol].Font.Size = FontUnit.Medium;
                                        }
                                    }
                                    if (TotPaidAmnt > 0)

                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Text = Convert.ToString(TotPaidAmnt);
                                    else
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Text = "";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;

                                    if (LoanAmount > TotPaidAmnt)
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(LoanAmount - TotPaidAmnt);
                                    else
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Text = "";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                }
                            }
                        }
                    }
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Rows[FpSpread.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                    FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount - 1, 0, 1, 6);
                    for (int col = 6; col < FpSpread.Sheets[0].ColumnCount; col++)
                    {
                        TotAmnt = 0;
                        GrandTotAmnt = 0;
                        for (int roc = 0; roc < FpSpread.Sheets[0].RowCount - 2; roc++)
                        {
                            double.TryParse(Convert.ToString(FpSpread.Sheets[0].Cells[roc, col].Text), out TotAmnt);
                            GrandTotAmnt = GrandTotAmnt + TotAmnt;
                        }
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString(GrandTotAmnt);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Bold = true;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                    }
                    FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                    FpSpread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread.Visible = true;
                    rprint.Visible = true;
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "No Record(s) Found!";
                }
            }
            else
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select From Month and To Month!";
            }
        }
        catch (Exception ex) { lblMainErr.Visible = true; lblMainErr.Text = ex.ToString(); }
    }

    private void LoadHeader()
    {
        try
        {
            FpSpread.Sheets[0].AutoPostBack = false;
            FpSpread.Sheets[0].RowHeader.Visible = false;
            FpSpread.CommandBar.Visible = false;
            FpSpread.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread.Sheets[0].RowCount = 0;
            FpSpread.Sheets[0].ColumnHeader.Columns.Count = 7;
            string SelYearQ = string.Empty;
            DataSet dsHeader = new DataSet();

            FarPoint.Web.Spread.StyleInfo darkStyle = new FarPoint.Web.Spread.StyleInfo();
            darkStyle.Font.Bold = true;
            darkStyle.Font.Name = "Book Antiqua";
            darkStyle.Font.Size = FontUnit.Medium;
            darkStyle.HorizontalAlign = HorizontalAlign.Center;
            darkStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkStyle.ForeColor = Color.Black;
            FpSpread.Sheets[0].ColumnHeader.DefaultStyle = darkStyle;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread.Columns[0].Width = 50;
            FpSpread.Columns[0].Locked = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
            FpSpread.Columns[1].Width = 150;
            FpSpread.Columns[1].Locked = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Type";
            FpSpread.Columns[2].Width = 150;
            FpSpread.Columns[2].Locked = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Category";
            FpSpread.Columns[3].Width = 150;
            FpSpread.Columns[3].Locked = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Loan Type";
            FpSpread.Columns[4].Width = 100;
            FpSpread.Columns[4].Locked = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Loan/Policy Name";
            FpSpread.Columns[5].Width = 150;
            FpSpread.Columns[5].Locked = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Loan/Policy Amount";
            FpSpread.Columns[6].Width = 100;
            FpSpread.Columns[6].Locked = true;

            string MyCollCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            int from_month = Convert.ToInt32(ddlMon.SelectedItem.Value);
            int to_month = Convert.ToInt32(ddlToMon.SelectedItem.Value);
            int year = Convert.ToInt32(ddlYear.SelectedItem.Text);
            int yearto = Convert.ToInt32(ddlToYear.SelectedItem.Text);
            if (from_month < to_month)
            {
                if (year <= yearto)
                {
                    SelYearQ = "select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum,SUBSTRING(PayYear,3,2) as PayYear,PayYear as SelYear from HrPayMonths where (PayMonthNum >= " + from_month + " and PayYear between '" + year + "' and '" + yearto + "') and (PayMonthNum <=" + to_month + " and PayYear between '" + year + "' and '" + yearto + "') and College_Code='" + MyCollCode + "' and SelStatus='1'";
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Please Select Correct Month & Year!";
                    return;
                }
            }
            else if (from_month == to_month)
            {
                if (year <= yearto)
                {
                    SelYearQ = "select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum,SUBSTRING(PayYear,3,2) as PayYear,PayYear as SelYear from HrPayMonths where (PayMonthNum >=" + from_month + " and PayYear between '" + year + "' and '" + yearto + "') and (PayMonthNum <=" + to_month + " and PayYear between '" + year + "' and '" + yearto + "') and College_Code='" + MyCollCode + "' and SelStatus='1'";
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Please Select Correct Month & Year!";
                    return;
                }
            }
            else
            {
                if (year != yearto)
                {
                    SelYearQ = " select PayMonth,CONVERT(varchar(20), From_Date,101)as From_Date ,CONVERT(varchar(20), To_Date,101)as To_Date,PayMonthNum,SUBSTRING(PayYear,3,2) as PayYear,PayYear as SelYear from HrPayMonths where (PayMonthNum >='" + from_month + "' and PayYear between '" + year + "' and '" + yearto + "') or (PayMonthNum <='" + to_month + "' and PayYear between '" + year + "' and '" + yearto + "') and College_Code='" + MyCollCode + "' and SelStatus='1'";
                }
                else
                {
                    lblMainErr.Visible = true;
                    lblMainErr.Text = "Please Select Correct Month & Year!";
                    return;
                }
            }
            dsHeader.Clear();
            dsHeader = d2.select_method_wo_parameter(SelYearQ, "Text");
            if (dsHeader.Tables.Count > 0 && dsHeader.Tables[0].Rows.Count > 0)
            {
                for (int icol = 0; icol < dsHeader.Tables[0].Rows.Count; icol++)
                {
                    FpSpread.Sheets[0].ColumnCount++;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = GetMonTxt(Convert.ToInt32(dsHeader.Tables[0].Rows[icol]["PayMonthNum"])) + "-" + Convert.ToString(dsHeader.Tables[0].Rows[icol]["PayYear"]);
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dsHeader.Tables[0].Rows[icol]["PayMonthNum"]) + "," + Convert.ToString(dsHeader.Tables[0].Rows[icol]["SelYear"]);
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                }
            }
            FpSpread.Sheets[0].ColumnCount++;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Paid Amount";
            FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
            FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;

            FpSpread.Sheets[0].ColumnCount++;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "OutStanding";
            FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
            FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
        }
        catch (Exception ex) { lblMainErr.Visible = true; lblMainErr.Text = ex.ToString(); }
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
            string degreedetails = "Staff Loan Details Report";
            string pagename = "Staff_LoanDetailsReport.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }

    public void year(string date)
    {
        try
        {
            ds11.Clear();
            string year = "";
            if (date.Trim() == "")
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and SelStatus='1' order by year asc";
            }
            else
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and PayMonthNum =" + date + " and SelStatus='1' order by year asc";
            }
            ds11 = d2.select_method_wo_parameter(year, "text");
            if (ds11.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds11;
                ddlYear.DataTextField = "year";
                ddlYear.DataValueField = "year";
                ddlYear.DataBind();

            }
        }
        catch (Exception ex) { }
    }

    public void year1(string date)
    {
        try
        {
            ds11.Clear();
            string year = "";
            if (date.Trim() == "")
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and SelStatus='1' order by year asc";
            }
            else
            {
                year = "select distinct year(From_Date) as year from HrPayMonths  where College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and PayMonthNum =" + date + " and SelStatus='1' order by year asc";
            }
            ds11 = d2.select_method_wo_parameter(year, "text");
            if (ds11.Tables[0].Rows.Count > 0)
            {
                ddlToYear.DataSource = ds11;
                ddlToYear.DataTextField = "year";
                ddlToYear.DataValueField = "year";
                ddlToYear.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    private string GetMonTxt(int Month)
    {
        string myMon = string.Empty;
        try
        {
            switch (Month)
            {
                case 1:
                    myMon = "Jan";
                    break;
                case 2:
                    myMon = "Feb";
                    break;
                case 3:
                    myMon = "Mar";
                    break;
                case 4:
                    myMon = "Apr";
                    break;
                case 5:
                    myMon = "May";
                    break;
                case 6:
                    myMon = "June";
                    break;
                case 7:
                    myMon = "July";
                    break;
                case 8:
                    myMon = "Aug";
                    break;
                case 9:
                    myMon = "Sep";
                    break;
                case 10:
                    myMon = "Oct";
                    break;
                case 11:
                    myMon = "Nov";
                    break;
                case 12:
                    myMon = "Dec";
                    break;
            }
        }
        catch { }
        return myMon;
    }

    private void bindFrmMonth(string Selmonth1, string Selyear1)
    {
        try
        {
            string collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            ddlMon.Items.Clear();
            ddlToMon.Items.Clear();
            string str = "select PayMonth,PayMonthNum from HrPayMonths where College_Code='" + collegecode1 + "' and SelStatus='1' and (From_Date<='" + Selmonth1 + "/01/" + Selyear1 + "' or To_Date<='" + Selmonth1 + "/01/" + Selyear1 + "')";
            ds = d2.select_method_wo_parameter(str, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int myk = 0; myk < ds.Tables[0].Rows.Count; myk++)
                {
                    ddlMon.Items.Insert(myk, new ListItem(GetMonTxt(Convert.ToInt32(ds.Tables[0].Rows[myk]["PayMonthNum"])), Convert.ToString(ds.Tables[0].Rows[myk]["PayMonthNum"])));
                }
                ddlMon.Items.Insert(0, "---Select---");
                ddlToMon.Items.Insert(0, "---Select---");
            }
            else
            {
                ddlMon.Items.Insert(0, "---Select---");
                ddlToMon.Items.Insert(0, "---Select---");
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
                    txt_desig.Text = "Designation (" + cbl_desig.Items.Count + ")";
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
                    txt_staffc.Text = "Category (" + cbl_staffc.Items.Count + ")";
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
                txtchange.Text = label + " (" + Convert.ToString(chklstchange.Items.Count) + ")";
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
                txtchange.Text = label + " (" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }
}