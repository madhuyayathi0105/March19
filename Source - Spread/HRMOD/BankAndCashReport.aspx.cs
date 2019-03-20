using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class HRMOD_BankAndCashReport : System.Web.UI.Page
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
            //****************************************************//

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
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            setLabelText();
            lbl_college.Text = lbl_clgT.Text;
            lbl_dept.Text = lbl_deptT.Text;
            lbl_desig.Text = lbl_desigT.Text;
            //lbl_branch.Text = lbl_branchT.Text;
            //lbl_org_sem.Text = lbl_semT.Text;
            if (!IsPostBack)
            {
                bindcollege();
                collegecode = rs.GetSelectedItemsValueAsString(cblclg);
                binddept();
                designation();
                category();
                stafftype();
                loaddeduction();
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
    protected void loaddeduction()
    {
        try
        {
            ds.Clear();
            cbldeduct.Items.Clear();
            txtdeduct.Text = "--Select--";
            cbdeduct.Checked = false;
            collegecode = rs.GetSelectedItemsValueAsString(cblclg);
            string item = "select deductions from incentives_master where college_code in('" + collegecode + "')";
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
                        cbldeduct.Items.Add(stafftype);
                    }
                }
                if (cbldeduct.Items.Count > 0)
                {
                    for (int i = 0; i < cbldeduct.Items.Count; i++)
                    {
                        cbldeduct.Items[i].Selected = true;
                    }
                    txtdeduct.Text = "Deduction (" + cbldeduct.Items.Count + ")";
                    cbdeduct.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void bindyear()
    {
        int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
        for (int l = 0; l < 15; l++)
        {
            cbl_year.Items.Add(Convert.ToString(year));
            year--;
        }
        if (cbl_year.Items.Count > 0)
        {
            for (int row = 0; row < cbl_year.Items.Count; row++)
            {
                cbl_year.Items[row].Selected = true;
            }
            cb_year.Checked = true;
            txt_year.Text = lbl_year.Text + "(" + cbl_year.Items.Count + ")";
        }
    }
    protected void bindmonth()
    {
        DateTime dt = new DateTime(2000, 1, 1);
        for (int m = 0; m < 12; m++)
        {
            cbl_month.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
        }
        if (cbl_month.Items.Count > 0)
        {
            for (int row = 0; row < cbl_month.Items.Count; row++)
            {
                cbl_month.Items[row].Selected = true;
            }
            cb_month.Checked = true;
            txt_month.Text = lbl_month.Text + "(" + cbl_month.Items.Count + ")";
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
    protected void cb_month_CheckedChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_month, cb_month, txt_month, "Month");
    }
    protected void cbl_month_SelectedIndexChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_month, cb_month, txt_month, "Month");
    }
    protected void cb_year_CheckedChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_year, cb_year, txt_year, "Year");
    }
    protected void cbl_year_SelectedIndexChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_year, cb_year, txt_year, "Year");
    }
    protected void radFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (radFormat.SelectedIndex == 0)
        //{
        //    FpSpread.Visible = true;
        //    FpSpread.Visible = false;
        //}
        //else if (radFormat.SelectedIndex == 1)
        //{
        //    FpSpread.Visible = false;
        //    FpSpread.Visible = true;
        //}

    }
    protected void cbdeduct_OnCheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbldeduct, cbdeduct, txtdeduct, "Deduction");
    }
    protected void cbldeduct_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbldeduct, cbdeduct, txtdeduct, "Deduction");
    }

    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblclg, cbclg, txtclg, lbl_college.Text);
        collegecode = rs.GetSelectedItemsValueAsString(cblclg);
        binddept();
        designation();
        category();
        stafftype();
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
        lbl_alert.Visible = false;
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
            string degreedetails = "Bank And Cash Report";
            string pagename = "salarybill.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }

    #endregion
    
    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lbl_clgT);
        lbl.Add(lbl_deptT);
        lbl.Add(lbl_desigT);
        lbl.Add(lbl_semT);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        lbl.Add(lbl_semT);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }


    protected void btn_go_Click(object sender, EventArgs e)
    {
        string departmentcode = rs.GetSelectedItemsValueAsString(cbl_dept);
        string designationcode = rs.GetSelectedItemsValueAsString(cbl_desig);
        string category = rs.GetSelectedItemsValueAsString(cbl_staffc);
        string stafftype = rs.GetSelectedItemsValueAsString(cbl_stype);
        string collegecode = rs.GetSelectedItemsValueAsString(cblclg);
        string month = rs.GetSelectedItemsValueAsString(cbl_month);
        string year = rs.GetSelectedItemsValueAsString(cbl_year);
        int SelectClgCount = GetSelectedItemsValueCount(cblclg);
        if (radFormat.Text == "Cumulative" || radFormat.SelectedIndex == 0)
        {
            # region cumulative
            try
            {

                Dictionary<int, double> dictotal = new Dictionary<int, double>();
                string Qry = " select distinct PayMode, case when PayMode=0 then 'Cash' when PayMode=1 then 'Cheque' when PayMode=2 then 'Credit' end payModeText from stafftrans where paymode is not null";
                ds = d2.select_method_wo_parameter(Qry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    #region Load Spread Header
                    int count = 0;
                    FpSpread.Sheets[0].RowCount = 0;
                    FpSpread.Sheets[0].ColumnCount = 0;
                    FpSpread.CommandBar.Visible = false;
                    FpSpread.Sheets[0].AutoPostBack = true;
                    FpSpread.Sheets[0].ColumnHeader.RowCount = 2;
                    FpSpread.Sheets[0].RowHeader.Visible = false;
                    FpSpread.Sheets[0].ColumnCount = 2;
                    int sno = 0;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    int countval = 0;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Institution";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Columns[1].Visible = true;
                    FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                    int intCol = 0;
                    if (cbl_staffc.Items.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            countval = 0;
                            for (int s = 0; s < cbl_staffc.Items.Count; s++)
                            {
                                if (cbl_staffc.Items[s].Selected == true)
                                {
                                    if (countval == 0)
                                        intCol = FpSpread.Sheets[0].ColumnCount;
                                    FpSpread.Sheets[0].ColumnCount++;
                                    string paymode = ds.Tables[0].Rows[i]["paymodetext"].ToString();
                                    string paymodevalue = ds.Tables[0].Rows[i]["paymode"].ToString();
                                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = paymode;
                                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Tag = paymodevalue;

                                    FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_staffc.Items[s].Text);
                                    FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_staffc.Items[s].Value);
                                    countval++;
                                }
                            }
                            FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, intCol, 1, countval);
                            FpSpread.Sheets[0].ColumnCount++;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Total";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 2, 1);
                        }

                    }
                    FpSpread.Sheets[0].ColumnCount++;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Grand Total";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 2, 1);
                    #endregion

                    # region load rows spread
                    string query = "select sum(NetAdd)NetAdd, p.category_code,case when t.PayMode=0 then 'Cash' when t.PayMode=1 then 'Cheque' when t.PayMode=2 then 'Credit' end payModeText,payMode,m.college_code,cl.collname from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c,monthlypay p,collinfo cl where cl.college_code=m.college_code and cl.college_code=m.college_code and c.college_code=cl.college_code and p.staff_code=t.staff_code and p.staff_code=m.staff_code and t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1  and ISNULL(Discontinue,'0')='0' and ISNULL(PayYear,-1)<>-1 and ISNULL(PayMonth,-1)<>-1 and m.college_code in( '" + collegecode + "') and h.dept_code in('" + departmentcode + "') and g.desig_code in('" + designationcode + "') and c.category_code in('" + category + "') and t.stftype in('" + stafftype + "') and p.paymonth in('" + month + "') and p.payyear in ('" + year + "') group by p.category_code,t.PayMode,m.college_code,cl.collname";
                    DataSet dsrow = new DataSet();

                    dsrow = d2.select_method_wo_parameter(query, "Text");
                    if (cblclg.Items.Count > 0)
                    {
                        for (int i = 0; i < cblclg.Items.Count; i++)
                        {
                            if (cblclg.Items[i].Selected == true)
                            {
                                FpSpread.Sheets[0].RowCount++;

                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread.Sheets[0].RowCount);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cblclg.Items[i].Text);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                for (int p = 2; p < FpSpread.Sheets[0].ColumnCount; p++)
                                {
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(cblclg.Items[i].Value);
                                    string college = string.Empty;
                                    string Catagorytag = string.Empty;
                                    string Paymode = string.Empty;
                                    Catagorytag = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[1, p].Tag);
                                    Paymode = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, p].Tag);
                                    if (Catagorytag != "" && Paymode != "")
                                    {
                                        double categoryWiseAmt = 0;
                                        double.TryParse(Convert.ToString(dsrow.Tables[0].Compute("Sum([NetAdd])", "category_code ='" + Catagorytag + "' and college_code ='" + Convert.ToString(cblclg.Items[i].Value) + "' and payMode ='" + Paymode + "'")), out categoryWiseAmt);

                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].Text = Convert.ToString(categoryWiseAmt);
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].Font.Size = FontUnit.Medium;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].Font.Name = "Book Antiqua";
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].Font.Bold = true;
                                        if (dictotal.ContainsKey(p))
                                            dictotal[p] += categoryWiseAmt;
                                        else
                                            dictotal.Add(p, categoryWiseAmt);
                                    }
                                    else
                                    {
                                        double TotalAmt = 0;
                                        Paymode = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, p - 1].Tag);
                                        if (!string.IsNullOrEmpty(Paymode))
                                        {
                                            double.TryParse(Convert.ToString(dsrow.Tables[0].Compute("Sum([NetAdd])", "category_code in('" + category + "') and college_code ='" + Convert.ToString(cblclg.Items[i].Value) + "' and payMode ='" + Paymode + "'")), out TotalAmt);
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].Text = Convert.ToString(TotalAmt);
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].Font.Size = FontUnit.Medium;
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].Font.Name = "Book Antiqua";
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, p].Font.Bold = true;
                                            if (dictotal.ContainsKey(p))
                                                dictotal[p] += TotalAmt;
                                            else
                                                dictotal.Add(p, TotalAmt);
                                        }
                                    }
                                }
                                double grandTotal = 0;
                                double.TryParse(Convert.ToString(dsrow.Tables[0].Compute("Sum([NetAdd])", "category_code in('" + category + "') and college_code ='" + Convert.ToString(cblclg.Items[i].Value) + "' ")), out grandTotal);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(grandTotal);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                if (dictotal.ContainsKey(FpSpread.Sheets[0].ColumnCount - 1))
                                    dictotal[FpSpread.Sheets[0].ColumnCount - 1] += grandTotal;
                                else
                                    dictotal.Add(FpSpread.Sheets[0].ColumnCount - 1, grandTotal);
                            }
                        }
                    }
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Total";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.MediumSlateBlue;

                    if (dictotal.Count > 0)
                    {
                        for (int dic = 0; dic < dictotal.Count + 2; dic++)
                        {
                            if (dictotal.ContainsKey(dic))
                            {
                                string total = Convert.ToString(dictotal[dic]);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].Text = total;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].ForeColor = Color.Peru;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].Font.Bold = true;
                            }
                        }
                    }

                    FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                    FpSpread.Height = FpSpread.Sheets[0].RowCount;
                    sp_div.Visible = true;
                    # endregion
                }


                else
                {
                    sp_div.Visible = false;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "No records founds";
                }
            }
            catch (Exception ex)
            {
                sp_div.Visible = false;
                lbl_alert.Visible = true;
                lbl_alert.Text = ex.ToString();
            }

            # endregion
        }
        if (radFormat.SelectedIndex == 1 || radFormat.Text == "Detailed")
        {
            # region Detailed
            try
            {
                DateTime FromDateDts = new DateTime();
                DateTime ToDateDts = new DateTime();
                string getmonth = string.Empty;
                string getyear = string.Empty;

                for (int mon = 0; mon < cbl_month.Items.Count; mon++)
                {
                    if (cbl_month.Items[mon].Selected == true && cbl_month.Items.Count > 0)
                    {
                        getmonth = Convert.ToString(cbl_month.Items[mon].Value);
                    }
                }

                for (int yr = 0; yr < cbl_year.Items.Count; yr++)
                {
                    if (cbl_year.Items[yr].Selected == true && cbl_year.Items.Count > 0)
                    {
                        getyear = Convert.ToString(cbl_year.Items[yr].Text);
                    }
                
                }

                DateTime.TryParse(Convert.ToString(getmonth + "/01/" + getyear), out FromDateDts);
                string endDates = Convert.ToString(DateTime.DaysInMonth(Convert.ToInt32(getyear), Convert.ToInt32(getmonth)));
                DateTime.TryParse(Convert.ToString(getmonth + "/" + endDates + "/" + getyear), out ToDateDts);

             

                string qry = "select sum(NetAdd)NetAdd, p.category_code,case when t.PayMode=0 then 'Cash' when t.PayMode=1 then 'Cheque' when t.PayMode=2 then 'Credit' end payModeText,payMode,m.college_code,cl.collname,p.TransferBankFK,clgbankcode from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c,monthlypay p,collinfo cl where cl.college_code=m.college_code and cl.college_code=m.college_code and c.college_code=cl.college_code and p.staff_code=t.staff_code and p.staff_code=m.staff_code and t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1  and ISNULL(Discontinue,'0')='0' and ISNULL(PayYear,-1)<>-1 and ISNULL(PayMonth,-1)<>-1 and m.college_code in( '" + collegecode + "') and h.dept_code in('" + departmentcode + "') and g.desig_code in('" + designationcode + "') and c.category_code in('" + category + "') and t.stftype in('" + stafftype + "') and p.paymonth in('" + month + "') and p.payyear in ('" + year + "')" ;
                qry += " and ((resign=0 or settled=0) or (resign=1 and relieve_date>='" + ToDateDts.ToString("MM/dd/yyyy") + "') or (resign=1 and relieve_date between '" + FromDateDts.ToString("MM/dd/yyyy") + "' and '" + ToDateDts.ToString("MM/dd/yyyy") + "'))";
                
               qry +=" group by p.category_code,t.PayMode,m.college_code,cl.collname,TransferBankFK,clgbankcode";

                DataSet ds1row = new DataSet();
                ds1row = d2.select_method_wo_parameter(qry, "Text");
                if (ds1row.Tables.Count > 0 && ds1row.Tables[0].Rows.Count > 0)
                {
                    #region Load Spread1 Header
                    int count = 0;
                    int bn;
                    double DeductionTotal = 0;
                    //int co = 0;
                    double dedutotal = 0;
                    double bankvalue = 0;
                    double banktotal = 0;
                    double cashtotal = 0;
                    string bankfkspread = string.Empty;
                    string bankname = string.Empty;

                    FpSpread.Sheets[0].RowCount = 0;
                    FpSpread.Sheets[0].ColumnCount = 0;
                    FpSpread.CommandBar.Visible = false;
                    FpSpread.Sheets[0].AutoPostBack = true;
                    FpSpread.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread.Sheets[0].RowHeader.Visible = false;
                    FpSpread.Sheets[0].ColumnCount = 3;
                    int sno = 0;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Dictionary<int, double> diccoltotal = new Dictionary<int, double>();
                    int countval = 0;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "CategoryWiseInstitution";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Gross Pay";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    for (int d = 0; d < cbldeduct.Items.Count; d++)
                    {
                        if (cbldeduct.Items[d].Selected == true && cbldeduct.Items.Count > 0)
                        {
                            FpSpread.Sheets[0].ColumnCount++;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbldeduct.Items[d].Text);
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbldeduct.Items[d].Value);

                        }
                    }
                    FpSpread.Sheets[0].ColumnCount++;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Total";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    string query = "select f.BankName,F.AccNo,F.BankPK from FM_FinBankMaster f,monthlypay m where f.BankPK=m.TransferBankFK AND F.CollegeCode in('" + collegecode + "')";
                    DataSet ds1 = new DataSet();
                    ds1 = d2.select_method_wo_parameter(query, "Text");
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int b = 0; b < ds1.Tables[0].Rows.Count; b++)
                        {
                            FpSpread.Sheets[0].ColumnCount++;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds1.Tables[0].Rows[b]["BankName"] + "-" + ds1.Tables[0].Rows[b]["AccNo"]);
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds1.Tables[0].Rows[b]["BankPK"]);
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Note = Convert.ToString(ds1.Tables[0].Rows[b]["AccNo"]);
                        }

                    }
                   

                    string bankquery = "select hb.bankpk,BankName from hr_bankrights  hb,FM_FinBankMaster fb where hb.bankpk=fb.BankPK and hb.college_code=fb.CollegeCode and hb.college_code in('" + collegecode + "')";//delsi1106
                    DataSet bankds = new DataSet();
                    int columncountbank = 0;
                    int countvalue = 0;

                    bankds = d2.select_method_wo_parameter(bankquery, "text");
                    if (bankds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < bankds.Tables[0].Rows.Count; i++)
                        {

                            FpSpread.Sheets[0].ColumnCount++;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(bankds.Tables[0].Rows[i]["BankName"]);
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(bankds.Tables[0].Rows[i]["bankpk"]);
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;


                        }

                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Cash";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        columncountbank = FpSpread.Sheets[0].ColumnCount;
                        countvalue++;
                    }
                    else
                    {

                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Cash";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                    
                    }



                    # endregion


                    # region load Spread1 Rows
                 

                    if (cblclg.Items.Count > 0 && cbl_staffc.Items.Count > 0)
                    {
                        for (int c = 0; c < cblclg.Items.Count; c++)
                        {
                            if (cblclg.Items[c].Selected == true)
                            {
                                for (int sc = 0; sc < cbl_staffc.Items.Count; sc++)
                                {
                                    if (cbl_staffc.Items[sc].Selected == true)
                                    {
                                        FpSpread.Sheets[0].RowCount++;

                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread.Sheets[0].RowCount);
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cblclg.Items[c].Text + "-" + cbl_staffc.Items[sc].Text);
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(cbl_staffc.Items[sc].Value);
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(cblclg.Items[c].Value);
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;

                                        double grosspay = 0;
                                        double.TryParse(Convert.ToString(ds1row.Tables[0].Compute("Sum([NetAdd])", "category_code in('" + cbl_staffc.Items[sc].Value + "') and college_code ='" + Convert.ToString(cblclg.Items[c].Value) + "' ")), out grosspay);
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(grosspay);
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        if (diccoltotal.ContainsKey(2))
                                            diccoltotal[2] += grosspay;
                                        else
                                            diccoltotal.Add(2, grosspay);
                                        string deduct = rs.GetSelectedItemsValueAsString(cbldeduct);
                                        if (cbldeduct.Items.Count > 0 && deduct!="")
                                        {
                                            #region Duduction
                                            #region Bind Columns Name
                                            DataTable deductionDt = new DataTable();
                                            deductionDt.Columns.Add("collegecode", typeof(double));
                                            deductionDt.Columns.Add("Paymonth", typeof(int));
                                            deductionDt.Columns.Add("Payyear", typeof(int));
                                            deductionDt.Columns.Add("Netadd", typeof(double));
                                            if (cbldeduct.Items.Count > 0)
                                            {
                                                for (int sel = 0; sel < cbldeduct.Items.Count; sel++)
                                                {
                                                    if (cbldeduct.Items[sel].Selected == true)
                                                    {
                                                        if (cbldeduct.Items[sel].Text != "")
                                                        {
                                                            if (!deductionDt.Columns.Contains(cbldeduct.Items[sel].Text))
                                                                deductionDt.Columns.Add(cbldeduct.Items[sel].Text, typeof(double));
                                                        }
                                                    }
                                                }
                                            }


                                            DateTime FromDateDt = new DateTime();
                                            DateTime ToDateDt = new DateTime();
                                            DateTime.TryParse(Convert.ToString(cbl_month.Items[sc].Value + "/01/" + cbl_year.Items[sc].Text), out FromDateDt);
                                            string endDate = Convert.ToString(DateTime.DaysInMonth(Convert.ToInt32(cbl_year.Items[sc].Text), Convert.ToInt32(cbl_month.Items[sc].Value)));
                                            DateTime.TryParse(Convert.ToString(cbl_month.Items[sc].Value + "/" + endDate + "/" + cbl_year.Items[sc].Text), out ToDateDt);
                                            string Qry = " select m.deductions,m.allowances,m.college_code,m.netadd,PayMonth,PayYear,Tot_lop from stafftrans t,monthlypay m,staffmaster sm,hrdept_master hm where t.dept_code=hm.dept_code and sm.staff_code=t.staff_code and t.staff_code = m.staff_code and t.category_code=m.category_code and t.latestrec = 1  and m.PayMonth in('" + month + "') and m.PayYear in( '" + year + "') and m.college_code in('" + collegecode + "')  and convert(varchar(max), m.deductions)<>'' and t.dept_code in('" + departmentcode + "') ";
                                            if (!string.IsNullOrEmpty(category))
                                                //  Qry += " and m.category_code in('" + category + "')";
                                                Qry += " and m.category_code in('" + cbl_staffc.Items[sc].Value + "')";

                                            if (!string.IsNullOrEmpty(stafftype))
                                                Qry += " and t.stftype in('" + stafftype + "') ";
                                            Qry += " and ((sm.resign=0 or sm.settled=0) or (sm.resign=1 and sm.relieve_date>='" + ToDateDt.ToString("MM/dd/yyyy") + "') or (sm.resign=1 and sm.relieve_date between '" + FromDateDt.ToString("MM/dd/yyyy") + "' and '" + ToDateDt.ToString("MM/dd/yyyy") + "'))";
                                            DataSet deductionDs = d2.select_method_wo_parameter(Qry, "text");
                                            bool grandpay = false;
                                            if (deductionDs.Tables[0].Rows.Count > 0)
                                            {
                                                DataRow dr;
                                                foreach (DataRow Data in deductionDs.Tables[0].Rows)
                                                {
                                                    string[] split_main = Convert.ToString(Data["deductions"]).Split('\\');
                                                    if (split_main.Length > 0)
                                                    {
                                                        dr = deductionDt.NewRow();
                                                        dr["netadd"] = Convert.ToString(Data["netadd"]); grandpay = true;
                                                        for (int ct = 0; ct <= split_main.GetUpperBound(0); ct++)
                                                        {
                                                            string secondvlaue = Convert.ToString(split_main[ct]);
                                                            if (!string.IsNullOrEmpty(secondvlaue.Trim()))
                                                            {
                                                                string[] second_split_value = secondvlaue.Split(';');
                                                                if (second_split_value.Length > 0)
                                                                {
                                                                    string Columnname = Convert.ToString(second_split_value[0]);
                                                                    //string Columnvalue = Convert.ToString(second_split_value[3]);
                                                                    double Amount = 0;
                                                                    double.TryParse(Convert.ToString(second_split_value[3]), out Amount);
                                                                    if (deductionDt.Columns.Contains(Columnname))
                                                                    {
                                                                        //double.TryParse(Convert.ToString(Math.Round(Amount, 0, MidpointRounding.AwayFromZero)), out Amount);
                                                                        dr[Columnname] = Amount;
                                                                        dr["collegecode"] = Convert.ToString(Data["college_code"]);
                                                                        dr["Paymonth"] = Convert.ToString(Data["PayMonth"]);
                                                                        dr["Payyear"] = Convert.ToString(Data["PayYear"]);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        deductionDt.Rows.Add(dr);
                                                    }
                                                }
                                                foreach (DataRow Data in deductionDs.Tables[0].Rows)
                                                {
                                                    string[] split_main = Convert.ToString(Data["allowances"]).Split('\\');
                                                    if (split_main.Length > 0)
                                                    {
                                                        dr = deductionDt.NewRow();
                                                        if (!grandpay)
                                                            dr["netadd"] = Convert.ToString(Data["netadd"]);
                                                        for (int ct1 = 0; ct1 <= split_main.GetUpperBound(0); ct1++)
                                                        {
                                                            string secondvlaue = Convert.ToString(split_main[ct1]);
                                                            if (!string.IsNullOrEmpty(secondvlaue.Trim()))
                                                            {
                                                                string[] second_split_value = secondvlaue.Split(';');
                                                                if (second_split_value.Length > 0)
                                                                {
                                                                    string Columnname = Convert.ToString(second_split_value[0]);
                                                                    double Amount = 0;
                                                                    double.TryParse(Convert.ToString(second_split_value[3]), out Amount);
                                                                    if (deductionDt.Columns.Contains(Columnname))
                                                                    {
                                                                        double.TryParse(Convert.ToString(Math.Round(Amount, 0, MidpointRounding.AwayFromZero)), out Amount);
                                                                        dr[Columnname] = Amount;
                                                                        dr["collegecode"] = Convert.ToString(Data["college_code"]);
                                                                        dr["Paymonth"] = Convert.ToString(Data["PayMonth"]);
                                                                        dr["Payyear"] = Convert.ToString(Data["PayYear"]);

                                                                    }
                                                                }
                                                            }
                                                        }
                                                        deductionDt.Rows.Add(dr);
                                                    }
                                                }
                                            }
                                            #endregion

                                            string sql = " select TransferBankFK,b.bankname,b.AccNo,m.deductions,m.allowances,m.college_code,m.netadd,Paymonth,payyear,Tot_lop,m.category_code from stafftrans t,staffmaster sm,monthlypay m left join FM_FinBankMaster B on m.TransferBankFK=b.BankPK where sm.staff_code=t.staff_code and t.staff_code = m.staff_code and t.category_code=m.category_code and t.latestrec = 1 and m.PayMonth in('" + month + "') and m.PayYear in('" + year + "') and m.college_code in('" + collegecode + "') and m.category_code in('" + category + "') and t.stftype in('" + stafftype + "') and ((sm.resign=0 or sm.settled=0) or (sm.resign=1 and sm.relieve_date>='" + ToDateDt + "') or (sm.resign=1 and sm.relieve_date between '" + FromDateDt + "' and '" + ToDateDt + "'))";
                                            DataSet dsdeduc = new DataSet();
                                            dsdeduc = d2.select_method_wo_parameter(sql, "Text");
                                            int sp;
                                            Double deduction = 0;
                                            string college = Convert.ToString(FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Note);
                                            string staffcategory = Convert.ToString(FpSpread.Sheets[0].Cells[sc, 1].Tag);
                                            DeductionTotal = 0;
                                            int co = 0;
                                            for (sp = 3; sp < cbldeduct.Items.Count + 3; sp++)
                                            {
                                                if (cbldeduct.Items[co].Selected == true)
                                                {
                                                    double value = 0;
                                                    double.TryParse(Convert.ToString(deductionDt.Compute("Sum([" + Convert.ToString(cbldeduct.Items[co].Value) + "])", " collegecode in('" + college + "')")), out value);
                                                    int DeductionRoundAmt = Convert.ToInt32(Math.Round(value, 0, MidpointRounding.AwayFromZero));
                                                    DeductionTotal += DeductionRoundAmt;
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Text = Convert.ToString(DeductionRoundAmt);
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Font.Bold = true;
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Font.Name = "Book Antiqua";
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Font.Size = FontUnit.Medium;
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].HorizontalAlign = HorizontalAlign.Right;
                                                    co++;
                                                    deduction = Convert.ToDouble(FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Text);
                                                    //dedutotal = dedutotal + deduction;
                                                    deduction += deduction;
                                                    if (diccoltotal.ContainsKey(sp))
                                                        diccoltotal[sp] += DeductionRoundAmt;
                                                    else
                                                        diccoltotal.Add(sp, DeductionRoundAmt);
                                                }

                                            }
                                            double overalltot = grosspay - DeductionTotal;
                                           // FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Text = Convert.ToString(DeductionTotal);
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Text = Convert.ToString(overalltot);
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Font.Bold = true;
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Font.Name = "Book Antiqua";
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].Font.Size = FontUnit.Medium;
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sp].HorizontalAlign = HorizontalAlign.Right;
                                            if (diccoltotal.ContainsKey(sp))
                                                diccoltotal[sp] += DeductionTotal;
                                            else
                                                diccoltotal.Add(sp, DeductionTotal);
                                            #endregion
                                            count = sp + 1;
                                            for (bn = sp + 1; bn < FpSpread.Sheets[0].ColumnCount - 1; bn++)
                                            {
                                                bankfkspread = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, bn].Tag);
                                                bankname = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, bn].Text);

                                                int fk = 0;
                                                if (bankname != "")
                                                {
                                                    int.TryParse(bankfkspread, out fk);
                                                    double.TryParse(Convert.ToString(ds1row.Tables[0].Compute("sum([netadd])", "category_code in('" + staffcategory + "') and college_code in('" + college + "') and PayMode='2' and clgbankcode='" + bankfkspread + "'")), out bankvalue);
                                                    banktotal += bankvalue;
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].Text = Convert.ToString(bankvalue);
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].Font.Bold = true;
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].Font.Name = "Book Antiqua";
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].Font.Size = FontUnit.Medium;
                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].HorizontalAlign = HorizontalAlign.Right;

                                                    if (diccoltotal.ContainsKey(bn))
                                                        diccoltotal[bn] += bankvalue;
                                                    else
                                                        diccoltotal.Add(bn, bankvalue);
                                                }
                                            }
                                            double cash = 0;
                                            double.TryParse(Convert.ToString(ds1row.Tables[0].Compute("Sum([NetAdd])", "category_code in('" + staffcategory + "') and college_code ='" + Convert.ToString(cblclg.Items[c].Value) + "' and payMode ='0'")), out cash);
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].Text = Convert.ToString(cash);
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].Font.Bold = true;
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].Font.Name = "Book Antiqua";
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].Font.Size = FontUnit.Medium;
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, bn].HorizontalAlign = HorizontalAlign.Right;
                                            cashtotal += cash;
                                            if (diccoltotal.ContainsKey(bn))
                                                diccoltotal[bn] += cash;
                                            else
                                                diccoltotal.Add(bn, cash);
                                                }
                                                else
                                                {
                                                    sp_div.Visible = false;
                                                    lbl_alert.Visible = true;
                                                    lbl_alert.Text = "Please select Deduction";
                                                }
                                      
                                    }
                                    
                                }
                            }
                        }

                    }
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Total";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.MediumSlateBlue;

                    if (diccoltotal.Count > 0)
                    {
                        for (int dic = 0; dic < diccoltotal.Count + 3; dic++)
                        {
                            if (diccoltotal.ContainsKey(dic))
                            {
                                string total = Convert.ToString(diccoltotal[dic]);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].Text = total;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].ForeColor = Color.Peru;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, dic].Font.Bold = true;
                            }
                        }
                    }
                    int bank = 0;
                    double grandbanktotal = 0;
                    double banktotalvalue = 0;
                    //FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount, 0, 1, FpSpread.Sheets[0].ColumnCount);
                    //FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount, 0, 1, FpSpread.Sheets[0].ColumnCount-1);
                    if (bankname != "")
                    {
                        for (bank = count; bank < FpSpread.Sheets[0].ColumnCount - 1; bank++)
                        {
                            countval++;
                            string bankname1 = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, bank].Text);
                            string bankvalue1 = Convert.ToString(FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - countval, bank].Text);
                            double.TryParse(bankvalue1, out banktotalvalue);
                            FpSpread.Sheets[0].RowCount++;
                           // FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount - 1, 0, 1, FpSpread.Sheets[0].ColumnCount);
                            FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount - 1, 0, 1, FpSpread.Sheets[0].ColumnCount -5);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = "Salary Paid Through Bank" + "-" + bankname1;
                            FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 5, 1, FpSpread.Sheets[0].ColumnCount);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 5].Text = "Rs:" + banktotalvalue;
                            grandbanktotal += banktotalvalue;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkBlue;

                        }
                        FpSpread.Sheets[0].RowCount++;
                        FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount - 1, 0, 1, FpSpread.Sheets[0].ColumnCount);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = "Total" + "   Rs:   " + grandbanktotal;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].ForeColor = Color.BlueViolet;
                    }

                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount - 1, 0, 1, FpSpread.Sheets[0].ColumnCount);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = "Salary Paid By Cash" + "   Rs:   " + cashtotal;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].ForeColor = Color.CornflowerBlue;

                    # endregion
                    FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                    FpSpread.Height = FpSpread.Sheets[0].RowCount;
                    sp_div.Visible = true;
                }
                else
                {
                    sp_div.Visible = false;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "No records founds";
                }
                
            }
            catch (Exception ex)
            {
                sp_div.Visible = false;
                lbl_alert.Visible = true;
                lbl_alert.Text = ex.ToString();
            }
        }
            # endregion
    }
    public int GetSelectedItemsValueCount(CheckBoxList cblSelected)
    {
        int count = 0;
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    count++;
                }
            }
        }
        catch { count = 0; }
        return count;
    }
}
