using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class HRMOD_AllowanceAndDeductionReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode = string.Empty;
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
            //if (Session["collegecode"] == null)
            //{
            //    Response.Redirect("~/Default.aspx");
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
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindCollege();
                if (cblclg.Items.Count > 0)
                    collegecode = rs.GetSelectedItemsValueAsString(cblclg);
                bindMonthandYear();
                loadallowance();
                loaddeduction();
                category();
                stafftype();
            }
            if (cblclg.Items.Count > 0)
                collegecode = rs.GetSelectedItemsValueAsString(cblclg);
        }
        catch (Exception ex) { }
    }
    #region college
    protected void bindCollege()
    {
        cblclg.Items.Clear();
        cbclg.Checked = false;
        txtclg.Text = "--Select--";
        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
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
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
            }
        }
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblclg, cbclg, txtclg, lblclg.Text);
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cblclg, cbclg, txtclg, lblclg.Text);
    }
    #endregion
    #region month
    public void bindMonthandYear()
    {
        try
        {
            int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
            for (int l = 0; l < 15; l++)
            {
                ddlyear.Items.Add(Convert.ToString(year));
                year--;
            }
            DateTime dt = new DateTime(2000, 1, 1);
            for (int m = 0; m < 12; m++)
            {
                ddlmonth.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
            }
        }
        catch { }
    }
    #endregion
    #region allowance
    protected void loadallowance()
    {
        try
        {
            ds.Clear();
            cblallow.Items.Clear();
            txtallow.Text = "--Select--";
            cballow.Checked = false;
            collegecode = rs.GetSelectedItemsValueAsString(cblclg);
            string item = "select allowances from incentives_master where college_code in('" + collegecode + "')";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblallow.DataSource = ds;
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    string stafftype = split1[0];
                    if (stafftype.Trim() != "")
                    {
                        cblallow.Items.Add(stafftype);
                    }
                }
                if (cblallow.Items.Count > 0)
                {
                    for (int i = 0; i < cblallow.Items.Count; i++)
                    {
                        cblallow.Items[i].Selected = true;
                    }
                    txtallow.Text = "Allowance (" + cblallow.Items.Count + ")";
                    cballow.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cballow_OnCheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblallow, cballow, txtallow, "Allowance");
    }
    protected void cblallow_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cblallow, cballow, txtallow, "Allowance");
    }
    #endregion
    #region deduction
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
    protected void cbdeduct_OnCheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbldeduct, cbdeduct, txtdeduct, "Deduction");
    }
    protected void cbldeduct_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbldeduct, cbdeduct, txtdeduct, "Deduction");
    }
    #endregion
    #region Catagory and staffType
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
    #endregion
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(DeductionDetSp, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }
    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            string pagename;
            pagename = "AllowanceAndDeductionReport.aspx";
            Printcontrolhed.loadspreaddetails(DeductionDetSp, pagename, txtexcelname.Text);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrolhed.Visible = false;
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
            if (cblallow.Items.Count > 0)
            {
                for (int sel = 0; sel < cblallow.Items.Count; sel++)
                {
                    if (cblallow.Items[sel].Selected == true)
                    {
                        if (cblallow.Items[sel].Text != "")
                        {
                            if (!deductionDt.Columns.Contains(cblallow.Items[sel].Text))
                                deductionDt.Columns.Add(cblallow.Items[sel].Text, typeof(double));
                        }
                    }
                }
            }

            string collegecode = rs.GetSelectedItemsValueAsString(cblclg);
            string CatagoryCode = rs.GetSelectedItemsValueAsString(cbl_staffc);
            string StaffType = rs.GetSelectedItemsValueAsString(cbl_stype);

            DateTime FromDateDt = new DateTime();
            DateTime ToDateDt = new DateTime();
            DateTime.TryParse(Convert.ToString(ddlmonth.SelectedItem.Value + "/01/" + ddlyear.SelectedItem.Text), out FromDateDt);
            string endDate = Convert.ToString(DateTime.DaysInMonth(Convert.ToInt32(ddlyear.SelectedItem.Text), Convert.ToInt32(ddlmonth.SelectedItem.Value)));
            DateTime.TryParse(Convert.ToString(ddlmonth.SelectedItem.Value + "/" + endDate + "/" + ddlyear.SelectedItem.Text), out ToDateDt);
            if (Radio_cumulative.Checked == true)
            {
                string Qry = " select m.deductions,m.allowances,m.college_code,m.netadd,PayMonth,PayYear,Tot_lop from stafftrans t,monthlypay m,staffmaster sm where sm.staff_code=t.staff_code and t.staff_code = m.staff_code and t.category_code=m.category_code and t.latestrec = 1  and m.PayMonth ='" + ddlmonth.SelectedItem.Value + "' and m.PayYear = '" + ddlyear.SelectedItem.Text + "' and m.college_code in('" + collegecode + "')  and convert(varchar(max), m.deductions)<>''";
                if (!string.IsNullOrEmpty(CatagoryCode))
                    Qry += " and m.category_code in('" + CatagoryCode + "')";
                if (!string.IsNullOrEmpty(StaffType))
                    Qry += " and t.stftype in('" + StaffType + "') ";
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
                            for (int count = 0; count <= split_main.GetUpperBound(0); count++)
                            {
                                string secondvlaue = Convert.ToString(split_main[count]);
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
                            for (int count = 0; count <= split_main.GetUpperBound(0); count++)
                            {
                                string secondvlaue = Convert.ToString(split_main[count]);
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
                if (!string.IsNullOrEmpty(collegecode))
                {
                    if (deductionDt.Rows.Count > 0)
                    {
                        #region Header
                        DeductionDetSp.Sheets[0].RowCount = 0;
                        DeductionDetSp.Sheets[0].RowCount = 0;
                        DeductionDetSp.Sheets[0].ColumnCount = 0;
                        DeductionDetSp.CommandBar.Visible = false;
                        DeductionDetSp.Sheets[0].AutoPostBack = true;
                        DeductionDetSp.Sheets[0].ColumnHeader.RowCount = 1;
                        DeductionDetSp.Sheets[0].ColumnHeader.Height = 30;
                        DeductionDetSp.Sheets[0].RowHeader.Visible = false;
                        DeductionDetSp.Sheets[0].Columns.Count = 5;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        DeductionDetSp.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        DeductionDetSp.Columns[0].Width = 50;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Allowances";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        DeductionDetSp.Columns[1].Width = 200;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Amount";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        DeductionDetSp.Columns[2].Width = 150;

                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Deductions";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        DeductionDetSp.Columns[3].Width = 200;

                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Amount";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        DeductionDetSp.Columns[4].Width = 150;
                        #endregion
                        int sno = 1;
                        int allowenceCount = getCblSelectedCount(cblallow);
                        int deductionCount = getCblSelectedCount(cbldeduct);
                        int MaxCount = allowenceCount < deductionCount ? deductionCount : allowenceCount;
                        for (int i = 0; i < MaxCount; i++)
                        {
                            DeductionDetSp.Sheets[0].RowCount++;
                            DeductionDetSp.Sheets[0].Cells[i, 0].Text = Convert.ToString(sno++);
                            DeductionDetSp.Sheets[0].Cells[i, 0].Font.Bold = true;
                            DeductionDetSp.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                        }
                        double AllowanceTotal = 0;
                        double DeductionTotal = 0;
                        double value = 0;
                        int row = 0;
                        if (cblallow.Items.Count > 0)
                        {
                            #region Allowance
                            for (int i = 0; i < cblallow.Items.Count; i++)
                            {
                                if (cblallow.Items[i].Selected == true)
                                {
                                    DeductionDetSp.Sheets[0].Cells[row, 1].Text = Convert.ToString(cblallow.Items[i].Text);
                                    DeductionDetSp.Sheets[0].Cells[row, 1].Tag = Convert.ToString(cblallow.Items[i].Value);
                                    DeductionDetSp.Sheets[0].Cells[row, 1].Font.Bold = true;
                                    DeductionDetSp.Sheets[0].Cells[row, 1].Font.Name = "Book Antiqua";
                                    DeductionDetSp.Sheets[0].Cells[row, 1].Font.Size = FontUnit.Medium;
                                    DeductionDetSp.Sheets[0].Cells[row, 1].HorizontalAlign = HorizontalAlign.Left;
                                    value = 0;
                                    double.TryParse(Convert.ToString(deductionDt.Compute("Sum([" + Convert.ToString(cblallow.Items[i].Value) + "])", " collegecode in('" + collegecode + "') and  PayMonth='" + Convert.ToString(ddlmonth.SelectedItem.Value) + "' and PayYear='" + Convert.ToString(ddlyear.SelectedItem.Value) + "'")), out value);
                                    int AllowanceRoundAmt = Convert.ToInt32(Math.Round(value, 0, MidpointRounding.AwayFromZero));
                                    AllowanceTotal += AllowanceRoundAmt;
                                    DeductionDetSp.Sheets[0].Cells[row, 2].Text = Convert.ToString(AllowanceRoundAmt);
                                    DeductionDetSp.Sheets[0].Cells[row, 2].Font.Bold = true;
                                    DeductionDetSp.Sheets[0].Cells[row, 2].Font.Name = "Book Antiqua";
                                    DeductionDetSp.Sheets[0].Cells[row, 2].Font.Size = FontUnit.Medium;
                                    DeductionDetSp.Sheets[0].Cells[row, 2].HorizontalAlign = HorizontalAlign.Right;
                                    row++;
                                }
                            }
                            #endregion
                        }
                        if (cbldeduct.Items.Count > 0)
                        {
                            #region Duduction
                            row = 0;
                            for (int i = 0; i < cbldeduct.Items.Count; i++)
                            {
                                if (cbldeduct.Items[i].Selected == true)
                                {
                                    DeductionDetSp.Sheets[0].Cells[row, 3].Text = Convert.ToString(cbldeduct.Items[i].Text);
                                    DeductionDetSp.Sheets[0].Cells[row, 3].Tag = Convert.ToString(cbldeduct.Items[i].Value);
                                    DeductionDetSp.Sheets[0].Cells[row, 3].Font.Bold = true;
                                    DeductionDetSp.Sheets[0].Cells[row, 3].Font.Name = "Book Antiqua";
                                    DeductionDetSp.Sheets[0].Cells[row, 3].Font.Size = FontUnit.Medium;
                                    DeductionDetSp.Sheets[0].Cells[row, 3].HorizontalAlign = HorizontalAlign.Left;
                                    value = 0;
                                    double.TryParse(Convert.ToString(deductionDt.Compute("Sum([" + Convert.ToString(cbldeduct.Items[i].Value) + "])", " collegecode in('" + collegecode + "') and  PayMonth='" + Convert.ToString(ddlmonth.SelectedItem.Value) + "' and PayYear='" + Convert.ToString(ddlyear.SelectedItem.Value) + "'")), out value);
                                    int DeductionRoundAmt = Convert.ToInt32(Math.Round(value, 0, MidpointRounding.AwayFromZero));
                                    DeductionTotal += DeductionRoundAmt;
                                    DeductionDetSp.Sheets[0].Cells[row, 4].Text = Convert.ToString(DeductionRoundAmt);
                                    DeductionDetSp.Sheets[0].Cells[row, 4].Font.Bold = true;
                                    DeductionDetSp.Sheets[0].Cells[row, 4].Font.Name = "Book Antiqua";
                                    DeductionDetSp.Sheets[0].Cells[row, 4].Font.Size = FontUnit.Medium;
                                    DeductionDetSp.Sheets[0].Cells[row, 4].HorizontalAlign = HorizontalAlign.Right;
                                    row++;
                                }
                            }
                            #endregion
                        }
                        #region Footer Total
                        DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Total Allowance";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(AllowanceTotal);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Text = "Total Deduction";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(DeductionTotal);
                        DeductionDetSp.Sheets[0].Rows[DeductionDetSp.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].ForeColor = Color.Maroon;

                        DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].RowCount++;

                        value = 0;
                        double.TryParse(Convert.ToString(deductionDt.Compute("Sum([Netadd])", " collegecode in('" + collegecode + "') and  PayMonth='" + Convert.ToString(ddlmonth.SelectedItem.Value) + "' and PayYear='" + Convert.ToString(ddlyear.SelectedItem.Value) + "'")), out value);
                        int GrossAmount = Convert.ToInt32(Math.Round(value, 0, MidpointRounding.AwayFromZero));
                        double NetAmt = GrossAmount - DeductionTotal;
                        //double NetAmt = AllowanceTotal - DeductionTotal;
                        int NetAmtNumber = Convert.ToInt32(Math.Round(NetAmt, 0, MidpointRounding.AwayFromZero));
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Net = " + (NetAmtNumber);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Rows[DeductionDetSp.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                        DeductionDetSp.Sheets[0].RowCount++;

                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "( " + ConvertNumbertoWords(NetAmtNumber) + " )";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].SpanModel.Add(DeductionDetSp.Sheets[0].RowCount - 1, 1, 1, 4);
                        DeductionDetSp.Sheets[0].RowCount++;
                        #endregion
                        DeductionDetSp.Sheets[0].PageSize = DeductionDetSp.Sheets[0].RowCount;
                        Deduction.Visible = true;
                    }
                    else
                    {
                        Deduction.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                    }
                }
                else
                {
                    Deduction.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please select all fields!')", true);
                }
            }
            if (Radio_detail.Checked == true)
            {
                #region Header
                            DeductionDetSp.Sheets[0].RowCount = 0;
                            DeductionDetSp.Sheets[0].RowCount = 0;
                            DeductionDetSp.Sheets[0].ColumnCount = 0;
                            DeductionDetSp.CommandBar.Visible = false;
                            DeductionDetSp.Sheets[0].AutoPostBack = true;
                            DeductionDetSp.Sheets[0].ColumnHeader.RowCount = 1;
                            DeductionDetSp.Sheets[0].ColumnHeader.Height = 30;
                            DeductionDetSp.Sheets[0].RowHeader.Visible = false;
                            DeductionDetSp.Sheets[0].Columns.Count = 5;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            DeductionDetSp.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            DeductionDetSp.Columns[0].Width = 50;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Allowances";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            DeductionDetSp.Columns[1].Width = 200;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Amount";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            DeductionDetSp.Columns[2].Width = 150;

                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Deductions";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            DeductionDetSp.Columns[3].Width = 200;

                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Amount";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            DeductionDetSp.Columns[4].Width = 150;
                            #endregion

                            int row = 0;
                            int val = 0;
                            double grand_allow = 0;
                            double grand_deduct = 0;
                            double grand_tot = 0; int grandtotal = 0;
                for (int catval = 0; catval < cbl_staffc.Items.Count; catval++)
                {
                    string stf_Catergorycode = string.Empty;
                    if (cbl_staffc.Items[catval].Selected == true)
                    {
                        stf_Catergorycode = Convert.ToString(cbl_staffc.Items[catval].Value);


                        string Qry = " select m.deductions,m.allowances,m.college_code,m.netadd,PayMonth,PayYear,Tot_lop from stafftrans t,monthlypay m,staffmaster sm where sm.staff_code=t.staff_code and t.staff_code = m.staff_code and t.category_code=m.category_code and t.latestrec = 1  and m.PayMonth ='" + ddlmonth.SelectedItem.Value + "' and m.PayYear = '" + ddlyear.SelectedItem.Text + "' and m.college_code in('" + collegecode + "')  and convert(varchar(max), m.deductions)<>''";
                        if (!string.IsNullOrEmpty(CatagoryCode))
                            Qry += " and m.category_code in('" + stf_Catergorycode + "')";
                        if (!string.IsNullOrEmpty(StaffType))
                            Qry += " and t.stftype in('" + StaffType + "') ";
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
                                    for (int count = 0; count <= split_main.GetUpperBound(0); count++)
                                    {
                                        string secondvlaue = Convert.ToString(split_main[count]);
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
                                    for (int count = 0; count <= split_main.GetUpperBound(0); count++)
                                    {
                                        string secondvlaue = Convert.ToString(split_main[count]);
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
                    }

                    if (!string.IsNullOrEmpty(collegecode))
                    {
                        if (deductionDt.Rows.Count > 0)
                        {
                            
                            int sno = 1;
                            int allowenceCount = getCblSelectedCount(cblallow);
                            int deductionCount = getCblSelectedCount(cbldeduct);
                            int MaxCount = allowenceCount < deductionCount ? deductionCount : allowenceCount;
                            for (int i = 0; i < MaxCount; i++)
                            {
                                DeductionDetSp.Sheets[0].RowCount++;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            }
                            double AllowanceTotal = 0;
                            double DeductionTotal = 0;
                            double value = 0;
                            
                            if (cblallow.Items.Count > 0)
                            {
                                #region Allowance
                                for (int i = 0; i < cblallow.Items.Count; i++)
                                {
                                    if (cblallow.Items[i].Selected == true)
                                    {
                                        DeductionDetSp.Sheets[0].Cells[row, 1].Text = Convert.ToString(cblallow.Items[i].Text);
                                        DeductionDetSp.Sheets[0].Cells[row, 1].Tag = Convert.ToString(cblallow.Items[i].Value);
                                        DeductionDetSp.Sheets[0].Cells[row, 1].Font.Bold = true;
                                        DeductionDetSp.Sheets[0].Cells[row, 1].Font.Name = "Book Antiqua";
                                        DeductionDetSp.Sheets[0].Cells[row, 1].Font.Size = FontUnit.Medium;
                                        DeductionDetSp.Sheets[0].Cells[row, 1].HorizontalAlign = HorizontalAlign.Left;
                                        value = 0;
                                        double.TryParse(Convert.ToString(deductionDt.Compute("Sum([" + Convert.ToString(cblallow.Items[i].Value) + "])", " collegecode in('" + collegecode + "') and  PayMonth='" + Convert.ToString(ddlmonth.SelectedItem.Value) + "' and PayYear='" + Convert.ToString(ddlyear.SelectedItem.Value) + "'")), out value);
                                        int AllowanceRoundAmt = Convert.ToInt32(Math.Round(value, 0, MidpointRounding.AwayFromZero));
                                        AllowanceTotal += AllowanceRoundAmt;
                                        DeductionDetSp.Sheets[0].Cells[row, 2].Text = Convert.ToString(AllowanceRoundAmt);
                                        DeductionDetSp.Sheets[0].Cells[row, 2].Font.Bold = true;
                                        DeductionDetSp.Sheets[0].Cells[row, 2].Font.Name = "Book Antiqua";
                                        DeductionDetSp.Sheets[0].Cells[row, 2].Font.Size = FontUnit.Medium;
                                        DeductionDetSp.Sheets[0].Cells[row, 2].HorizontalAlign = HorizontalAlign.Right;
                                        row++;
                                    }
                                }
                                #endregion
                            }
                            if (cbldeduct.Items.Count > 0)
                            {
                                #region Duduction
                                row = 0;
                                row = val;
                                for (int i = 0; i < cbldeduct.Items.Count; i++)
                                {
                                    if (cbldeduct.Items[i].Selected == true)
                                    {
                                        DeductionDetSp.Sheets[0].Cells[row, 3].Text = Convert.ToString(cbldeduct.Items[i].Text);
                                        DeductionDetSp.Sheets[0].Cells[row, 3].Tag = Convert.ToString(cbldeduct.Items[i].Value);
                                        DeductionDetSp.Sheets[0].Cells[row, 3].Font.Bold = true;
                                        DeductionDetSp.Sheets[0].Cells[row, 3].Font.Name = "Book Antiqua";
                                        DeductionDetSp.Sheets[0].Cells[row, 3].Font.Size = FontUnit.Medium;
                                        DeductionDetSp.Sheets[0].Cells[row, 3].HorizontalAlign = HorizontalAlign.Left;
                                        value = 0;
                                        double.TryParse(Convert.ToString(deductionDt.Compute("Sum([" + Convert.ToString(cbldeduct.Items[i].Value) + "])", " collegecode in('" + collegecode + "') and  PayMonth='" + Convert.ToString(ddlmonth.SelectedItem.Value) + "' and PayYear='" + Convert.ToString(ddlyear.SelectedItem.Value) + "'")), out value);
                                        int DeductionRoundAmt = Convert.ToInt32(Math.Round(value, 0, MidpointRounding.AwayFromZero));
                                        DeductionTotal += DeductionRoundAmt;
                                        DeductionDetSp.Sheets[0].Cells[row, 4].Text = Convert.ToString(DeductionRoundAmt);
                                        DeductionDetSp.Sheets[0].Cells[row, 4].Font.Bold = true;
                                        DeductionDetSp.Sheets[0].Cells[row, 4].Font.Name = "Book Antiqua";
                                        DeductionDetSp.Sheets[0].Cells[row, 4].Font.Size = FontUnit.Medium;
                                        DeductionDetSp.Sheets[0].Cells[row, 4].HorizontalAlign = HorizontalAlign.Right;
                                        row++;
                                    }
                                }
                                #endregion
                            }
                            double allow = 0; double deduc = 0; double tot_amount = 0; 
                            #region Footer Total
                            DeductionDetSp.Sheets[0].RowCount++;
                            DeductionDetSp.Sheets[0].RowCount++;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Total Allowance";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(AllowanceTotal);
                            allow = Convert.ToDouble(AllowanceTotal);
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Text = "Total Deduction";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(DeductionTotal);
                            deduc = Convert.ToDouble(DeductionTotal);
                            DeductionDetSp.Sheets[0].Rows[DeductionDetSp.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].ForeColor = Color.Maroon;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].ForeColor = Color.Maroon;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].ForeColor = Color.Maroon;

                            DeductionDetSp.Sheets[0].RowCount++;
                            DeductionDetSp.Sheets[0].RowCount++;
                            grand_allow = grand_allow + allow;
                            grand_deduct = grand_deduct + deduc;
                           
                            value = 0;
                            double.TryParse(Convert.ToString(deductionDt.Compute("Sum([Netadd])", " collegecode in('" + collegecode + "') and  PayMonth='" + Convert.ToString(ddlmonth.SelectedItem.Value) + "' and PayYear='" + Convert.ToString(ddlyear.SelectedItem.Value) + "'")), out value);
                            int GrossAmount = Convert.ToInt32(Math.Round(value, 0, MidpointRounding.AwayFromZero));
                            double NetAmt = GrossAmount - DeductionTotal;
                            //double NetAmt = AllowanceTotal - DeductionTotal;
                            int NetAmtNumber = Convert.ToInt32(Math.Round(NetAmt, 0, MidpointRounding.AwayFromZero));
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Net = " + (NetAmtNumber);
                            tot_amount = Convert.ToDouble(NetAmtNumber);
                            grand_tot = grand_tot + tot_amount;
                            grandtotal = Convert.ToInt32(grand_tot);

                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                            DeductionDetSp.Sheets[0].Rows[DeductionDetSp.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                            DeductionDetSp.Sheets[0].RowCount++;
                           

                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "( " + ConvertNumbertoWords(NetAmtNumber) + " )";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                            DeductionDetSp.Sheets[0].SpanModel.Add(DeductionDetSp.Sheets[0].RowCount - 1, 1, 1, 4);
                            DeductionDetSp.Sheets[0].RowCount++;
                            row = DeductionDetSp.Sheets[0].RowCount++;
                            row++;
                             val= row;
                            
                            
                            #endregion
                            
                        }
                       
                    }
                    else
                    {
                        Deduction.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please select all fields!')", true);
                    }
                    deductionDt.Clear();

                }
                DeductionDetSp.Sheets[0].RowCount++;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Grand Total Allowance";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(grand_allow);
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Text = "Grand Total Deduction";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(grand_deduct);
                DeductionDetSp.Sheets[0].Rows[DeductionDetSp.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Green;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].ForeColor = Color.Green;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 3].ForeColor = Color.Green;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 4].ForeColor = Color.Green;

                DeductionDetSp.Sheets[0].RowCount++;
                DeductionDetSp.Sheets[0].RowCount++;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Grand Net = " + (grand_tot);
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Green;
                DeductionDetSp.Sheets[0].Rows[DeductionDetSp.Sheets[0].RowCount - 1].BackColor = Color.Bisque;
                DeductionDetSp.Sheets[0].RowCount++;

                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "( " + ConvertNumbertoWords(grandtotal) + " )";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Green;
                DeductionDetSp.Sheets[0].SpanModel.Add(DeductionDetSp.Sheets[0].RowCount - 1, 1, 1, 4);
                DeductionDetSp.Sheets[0].RowCount++;

                DeductionDetSp.Sheets[0].PageSize = DeductionDetSp.Sheets[0].RowCount;
                Deduction.Visible = true;
                       

            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "AllowanceAndDeductionReport");
        }
    }
    private int getCblSelectedCount(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        int selectCount = 0;
        for (int sel = 0; sel < cblSelected.Items.Count; sel++)
        {
            if (cblSelected.Items[sel].Selected == true)
            {
                selectCount++;
            }
        }
        return selectCount;
    }
    public string ConvertNumbertoWords(int number)
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
    protected void radioCumCheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (Radio_cumulative.Checked == true)
            {
                Radio_detail.Checked = false;

            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "AllowanceAndDeductionReport");

        }
    }

    protected void radioDetCheckedChange(object sender, EventArgs e)
    {

        try
        {
            if (Radio_detail.Checked == true)
            {
                Radio_cumulative.Checked = false;
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "AllowanceAndDeductionReport");

        }
    }
}