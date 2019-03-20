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
public partial class HRSalComparativeReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods reuse = new ReuasableMethods();
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
                bindCollege();
                if (cblclg.Items.Count > 0)
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                bindMonthandYear();
                loadallowance();
                loaddeduction();
            }
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
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
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    }
    #endregion
    
    #region month
    public void bindMonthandYear()
    {
        try
        {
            ddlmonth.Items.Clear();
            ddlmonth.Items.Add(new ListItem("January", "1"));
            ddlmonth.Items.Add(new ListItem("February", "2"));
            ddlmonth.Items.Add(new ListItem("March", "3"));
            ddlmonth.Items.Add(new ListItem("April", "4"));
            ddlmonth.Items.Add(new ListItem("May", "5"));
            ddlmonth.Items.Add(new ListItem("June", "6"));
            ddlmonth.Items.Add(new ListItem("July", "7"));
            ddlmonth.Items.Add(new ListItem("August", "8"));
            ddlmonth.Items.Add(new ListItem("September", "9"));
            ddlmonth.Items.Add(new ListItem("October", "10"));
            ddlmonth.Items.Add(new ListItem("November", "11"));
            ddlmonth.Items.Add(new ListItem("December", "12"));
            int year;
            year = Convert.ToInt16(DateTime.Today.Year);
            ddlyear.Items.Clear();
            for (int l = 0; l <= 7; l++)
            {
                ddlyear.Items.Add(Convert.ToString(year - l));
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
        CallCheckboxChange(cballow, cblallow, txtallow, "Allowance", "--Select--");
    }
    protected void cblallow_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cballow, cblallow, txtallow, "Allowance", "--Select--");
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
        CallCheckboxChange(cbdeduct, cbldeduct, txtdeduct, "Deduction", "--Select--");
    }
    protected void cbldeduct_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbdeduct, cbldeduct, txtdeduct, "Deduction", "--Select--");
    }
    #endregion
    protected void btngo_Click(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        Printcontrolhed.Visible = false;
        string colMonth = string.Empty;
        ds.Clear();
        ds = getDetails(ref colMonth);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            Hashtable CumulativeHash = new Hashtable();
            loadCumulativeMonth(ref colMonth, ref CumulativeHash);
            if (cbDifference.Checked)//if it's difference available this function will be available
            {
                DeductionBindMethod(CumulativeHash);
            }
            else { Deduction.Visible = false; }
        }
        else
        {
            //txtexcelname.Text = string.Empty;
            //spreadDet.Visible = false;
            //print.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }
    protected DataSet getDetails(ref string colMonth)
    {
        DataSet dsload = new DataSet();
        try
        {
            string allow = Convert.ToString(getCblSelectedValue(cblallow));
            string deduct = Convert.ToString(getCblSelectedValue(cbldeduct));
            string month = Convert.ToString(ddlmonth.SelectedValue);
            string yeaR = Convert.ToString(ddlyear.SelectedValue);
            string monthYear = getMonthYear(month, Convert.ToInt32(yeaR), ref colMonth);
            string selQ = string.Empty;
            if (true)
            {
                selQ = "select sum(netaddact)netaddact,college_code from monthlypay where (PayMonth = '" + monthYear.Split('$')[0].Split('-')[0] + "' and PayYear = '" + monthYear.Split('$')[0].Split('-')[1] + "') group by college_code";
                selQ += " select sum(netaddact)netaddact,college_code from monthlypay where (PayMonth = '" + monthYear.Split('$')[1].Split('-')[0] + "' and PayYear = '" + monthYear.Split('$')[1].Split('-')[1] + "') group by college_code";
                selQ += " select sum(netadd)netadd,college_code from monthlypay where (PayMonth = '" + monthYear.Split('$')[0].Split('-')[0] + "' and PayYear = '" + monthYear.Split('$')[0].Split('-')[1] + "') group by college_code";
                selQ += " select sum(netadd)netadd,college_code from monthlypay where (PayMonth = '" + monthYear.Split('$')[1].Split('-')[0] + "' and PayYear = '" + monthYear.Split('$')[1].Split('-')[1] + "') group by college_code";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selQ, "Text");
            }
        }
        catch { }
        return dsload;
    }
    protected string getMonthYear(string Month, int Year, ref string strMonth)
    {
        string monthCode = string.Empty;
        try
        {
            switch (Month)
            {
                case "1":
                    monthCode = "12" + "-" + Convert.ToString(Year - 1) + "$" + Month + "-" + Year;
                    strMonth = getMonth("12") + "-" + Convert.ToString(Year - 1) + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "2":
                    monthCode = "1" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("1") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "3":
                    monthCode = "2" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("2") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "4":
                    monthCode = "3" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth(Month) + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "5":
                    monthCode = "4" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("4") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "6":
                    monthCode = "5" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("5") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "7":
                    monthCode = "6" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("6") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "8":
                    monthCode = "7" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("7") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "9":
                    monthCode = "8" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("8") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "10":
                    monthCode = "9" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("9") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "11":
                    monthCode = "10" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("10") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
                case "12":
                    monthCode = "11" + "-" + Year + "$" + Month + "-" + Year;
                    strMonth = getMonth("11") + "-" + Year + "$" + getMonth(Month) + "-" + Year;
                    break;
            }
        }
        catch { }
        return monthCode;
    }
    protected string getMonth(string montH)
    {
        string strMonth = string.Empty;
        try
        {
            switch (montH)
            {
                case "1":
                    strMonth = "Jan";
                    break;
                case "2":
                    strMonth = "Feb";
                    break;
                case "3":
                    strMonth = "Mar";
                    break;
                case "4":
                    strMonth = "Apr";
                    break;
                case "5":
                    strMonth = "May";
                    break;
                case "6":
                    strMonth = "Jun";
                    break;
                case "7":
                    strMonth = "Jul";
                    break;
                case "8":
                    strMonth = "Aug";
                    break;
                case "9":
                    strMonth = "Sep";
                    break;
                case "10":
                    strMonth = "Oct";
                    break;
                case "11":
                    strMonth = "Nov";
                    break;
                case "12":
                    strMonth = "Dec";
                    break;
            }
        }
        catch { }
        return strMonth;
    }
    protected void loadCumulativeMonth(ref string monthYear, ref Hashtable htTotal)
    {
        try
        {
            #region design
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            ArrayList arColName = new ArrayList();
            arColName.Add("Sno");
            arColName.Add(lblclg.Text);
            arColName.Add(monthYear.Split('$')[0] + "Actual");
            arColName.Add(monthYear.Split('$')[1] + "Actual");
            arColName.Add("Difference" + "Actual");
            arColName.Add(monthYear.Split('$')[0] + "Net");
            arColName.Add(monthYear.Split('$')[1] + "Net");
            arColName.Add("Difference" + "Net");
            if (arColName.Count > 0)
            {
                foreach (string colName in arColName)
                {
                    spreadDet.Sheets[0].ColumnCount++;
                    int col = spreadDet.Sheets[0].ColumnCount - 1;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = colName;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            #endregion
            #region Value bind
            int rowCnt = 0;
            for (int clg = 0; clg < cblclg.Items.Count; clg++)
            {
                if (cblclg.Items[clg].Selected)
                {
                    #region basic salary
                    double fstMnthAmt = 0;
                    double sndMnthAmt = 0;
                    ds.Tables[0].DefaultView.RowFilter = "college_code='" + cblclg.Items[clg].Value + "'";
                    DataTable dtFrst = ds.Tables[0].DefaultView.ToTable();
                    if (dtFrst.Rows.Count > 0)
                        double.TryParse(Convert.ToString(dtFrst.Rows[0]["netaddact"]), out fstMnthAmt);
                    if (!htTotal.ContainsKey(2))
                        htTotal.Add(2, Convert.ToString(fstMnthAmt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[2]), out amount);
                        amount += fstMnthAmt;
                        htTotal.Remove(2);
                        htTotal.Add(2, Convert.ToString(amount));
                    }
                    ds.Tables[1].DefaultView.RowFilter = "college_code='" + cblclg.Items[clg].Value + "'";
                    DataTable dtSnd = ds.Tables[1].DefaultView.ToTable();
                    if (dtSnd.Rows.Count > 0)
                        double.TryParse(Convert.ToString(dtSnd.Rows[0]["netaddact"]), out sndMnthAmt);
                    if (!htTotal.ContainsKey(3))
                        htTotal.Add(3, Convert.ToString(sndMnthAmt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[3]), out amount);
                        amount += sndMnthAmt;
                        htTotal.Remove(3);
                        htTotal.Add(3, Convert.ToString(amount));
                    }
                    //difference
                    double tempDiffamt = fstMnthAmt - sndMnthAmt;
                    if (!htTotal.ContainsKey(4))
                        htTotal.Add(4, Convert.ToString(tempDiffamt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[4]), out amount);
                        amount += tempDiffamt;
                        htTotal.Remove(4);
                        htTotal.Add(4, Convert.ToString(amount));
                    }
                    #endregion
                    #region net salary
                    double fstNetMnthAmt = 0;
                    double sndNetMnthAmt = 0;
                    ds.Tables[2].DefaultView.RowFilter = "college_code='" + cblclg.Items[clg].Value + "'";
                    DataTable dtNetFrst = ds.Tables[2].DefaultView.ToTable();
                    if (dtNetFrst.Rows.Count > 0)
                        double.TryParse(Convert.ToString(dtNetFrst.Rows[0]["netadd"]), out fstNetMnthAmt);
                    if (!htTotal.ContainsKey(5))
                        htTotal.Add(5, Convert.ToString(fstNetMnthAmt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[5]), out amount);
                        amount += fstNetMnthAmt;
                        htTotal.Remove(5);
                        htTotal.Add(5, Convert.ToString(amount));
                    }
                    ds.Tables[3].DefaultView.RowFilter = "college_code='" + cblclg.Items[clg].Value + "'";
                    DataTable dtNetSnd = ds.Tables[3].DefaultView.ToTable();
                    if (dtNetSnd.Rows.Count > 0)
                        double.TryParse(Convert.ToString(dtNetSnd.Rows[0]["netadd"]), out sndNetMnthAmt);
                    if (!htTotal.ContainsKey(6))
                        htTotal.Add(6, Convert.ToString(sndNetMnthAmt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[6]), out amount);
                        amount += sndNetMnthAmt;
                        htTotal.Remove(6);
                        htTotal.Add(6, Convert.ToString(amount));
                    }
                    //difference
                    double tempNetDiffamt = fstNetMnthAmt - sndNetMnthAmt;
                    if (!htTotal.ContainsKey(7))
                        htTotal.Add(7, Convert.ToString(tempNetDiffamt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htTotal[7]), out amount);
                        amount += tempNetDiffamt;
                        htTotal.Remove(7);
                        htTotal.Add(7, Convert.ToString(amount));
                    }
                    #endregion
                    spreadDet.Sheets[0].RowCount++;
                    int row = spreadDet.Sheets[0].RowCount - 1;
                    spreadDet.Sheets[0].Cells[row, 0].Text = Convert.ToString(++rowCnt);
                    spreadDet.Sheets[0].Cells[row, 1].Text = Convert.ToString(cblclg.Items[clg].Text);
                    spreadDet.Sheets[0].Cells[row, 2].Text = Convert.ToString(fstMnthAmt);
                    spreadDet.Sheets[0].Cells[row, 3].Text = Convert.ToString(sndMnthAmt);
                    spreadDet.Sheets[0].Cells[row, 4].Text = Convert.ToString(tempDiffamt);
                    spreadDet.Sheets[0].Cells[row, 5].Text = Convert.ToString(fstNetMnthAmt);
                    spreadDet.Sheets[0].Cells[row, 6].Text = Convert.ToString(sndNetMnthAmt);
                    spreadDet.Sheets[0].Cells[row, 7].Text = Convert.ToString(tempNetDiffamt);
                }
            }
            if (htTotal.Count > 0)
            {
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.White;
                double grandvalues = 0;
                for (int j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                }
                spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                // payModeLabels(htPayCol);
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                spreadDet.Visible = true;
                print.Visible = true;
                getPrintSettings();
                //  spreadDet.Height = 200 + height;
                spreadDet.SaveChanges();
            }
            #endregion
        }
        catch { }
    }
    protected void DeductionBindMethod(Hashtable CumulativeHash)
    {
        try
        {
            #region Bind Columns Name
            DataTable deductionDt = new DataTable();
            if (cbldeduct.Items.Count > 0)
            {
                deductionDt.Columns.Add("collegecode", typeof(double));
                deductionDt.Columns.Add("Paymonth", typeof(int));
                deductionDt.Columns.Add("Payyear", typeof(int));
                deductionDt.Columns.Add("LOP", typeof(double));
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
            string prevMonth = string.Empty;
            string prevYear = string.Empty;
            string prevMonthText = string.Empty;
            if (ddlmonth.SelectedIndex - 1 != -1)
            {
                prevMonth = Convert.ToString(ddlmonth.Items[ddlmonth.SelectedIndex - 1].Value);
                prevYear = Convert.ToString(ddlyear.SelectedValue);
                prevMonthText = Convert.ToString(ddlmonth.Items[ddlmonth.SelectedIndex - 1].Text);
            }
            else
            {
                prevMonthText = Convert.ToString(ddlmonth.Items[11].Text);
                prevMonth = Convert.ToString(ddlmonth.Items[11].Value);
                prevYear = Convert.ToString(Convert.ToInt32(ddlyear.SelectedValue) - 1);
            }
            string collegecode = getCblSelectedValue(cblclg);
            string Qry = "select deductions,college_code,PayMonth,PayYear,Tot_lop from monthlypay where ((PayMonth >= '" + prevMonth + "' and PayYear = '" + prevYear + "') or (PayMonth <='" + ddlmonth.SelectedItem.Value + "' and PayYear = '" + ddlyear.SelectedItem.Text + "' ))  and college_code in('" + collegecode + "')  and convert(varchar(max), deductions)<>''";
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
            if (deductionDt.Rows.Count > 0)
            {
                #region HeaderValues
                DeductionDetSp.Sheets[0].RowCount = 0;
                DeductionDetSp.Sheets[0].RowCount = 0;
                DeductionDetSp.Sheets[0].ColumnCount = 0;
                DeductionDetSp.CommandBar.Visible = false;
                DeductionDetSp.Sheets[0].AutoPostBack = true;
                DeductionDetSp.Sheets[0].ColumnHeader.RowCount = 2;
                DeductionDetSp.Sheets[0].RowHeader.Visible = false;
                DeductionDetSp.Sheets[0].Columns.Count = 2;
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
                DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Institution Name";
                DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                DeductionDetSp.Columns[1].Width = 200;
                DeductionDetSp.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                DeductionDetSp.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                bool colfirstEntry = false;
                if (cbldeduct.Items.Count > 0)
                {
                    DeductionDetSp.Sheets[0].ColumnCount++;
                    for (int i = 0; i < cbldeduct.Items.Count; i++)
                    {
                        if (cbldeduct.Items[i].Selected == true)
                        {
                            if (colfirstEntry)
                            {
                                DeductionDetSp.Sheets[0].ColumnCount++;
                            }
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbldeduct.Items[i].Text);
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbldeduct.Items[i].Value);
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Text = prevMonthText;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(prevMonth + "-" + prevYear);
                            DeductionDetSp.Sheets[0].ColumnCount++;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Text = ddlmonth.SelectedItem.Text;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ddlmonth.SelectedItem.Value + "-" + ddlyear.SelectedItem.Value);
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbldeduct.Items[i].Value);
                            DeductionDetSp.Sheets[0].ColumnHeaderSpanModel.Add(0, DeductionDetSp.Sheets[0].ColumnCount - 2, 1, 2);
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            colfirstEntry = true;
                        }
                    }
                    DeductionDetSp.Sheets[0].ColumnCount++;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Text = "LOP";
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Tag = "LOP";
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Text = prevMonthText;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(prevMonth + "-" + prevYear);
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    DeductionDetSp.Sheets[0].ColumnCount++;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, DeductionDetSp.Sheets[0].ColumnCount - 1].Tag = "LOP";
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Text = ddlmonth.SelectedItem.Text;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ddlmonth.SelectedItem.Value + "-" + ddlyear.SelectedItem.Value);
                    DeductionDetSp.Sheets[0].ColumnHeaderSpanModel.Add(0, DeductionDetSp.Sheets[0].ColumnCount - 2, 1, 2);
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, DeductionDetSp.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                }
                #endregion
                double val = 0;
                Hashtable TotalHash = new Hashtable();
                if (DeductionDetSp.Sheets[0].ColumnCount > 0)
                {
                    if (cblclg.Items.Count > 0)
                    {
                        #region Bind Values
                        for (int c = 0; c < cblclg.Items.Count; c++)
                        {
                            if (cblclg.Items[c].Selected == true)
                            {
                                DeductionDetSp.Sheets[0].RowCount++;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(DeductionDetSp.Sheets[0].RowCount);
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = cblclg.Items[c].Text;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                for (int i = 2; i < DeductionDetSp.Sheets[0].ColumnCount; i++)
                                {
                                    string[] payMonthYear = Convert.ToString(DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, i].Tag).Split('-');
                                    string deductioncode = Convert.ToString(DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                                    string payMonth = string.Empty;
                                    string payYear = string.Empty;
                                    if (payMonthYear.Length == 2)
                                    {
                                        payMonth = Convert.ToString(payMonthYear[0]);
                                        payYear = Convert.ToString(payMonthYear[1]);
                                    }
                                    double value = 0;
                                    double.TryParse(Convert.ToString(deductionDt.Compute("Sum([" + deductioncode + "])", " collegecode='" + cblclg.Items[c].Value + "' and  PayMonth='" + payMonth + "' and PayYear='" + payYear + "'")), out value);
                                    DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].Text = Convert.ToString(value);
                                    DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Right;
                                    DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].Font.Size = FontUnit.Medium;
                                    DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].ForeColor = Color.Brown;
                                    val = 0;
                                    string payMonthYearHeader = Convert.ToString(DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, i].Tag);
                                    if (!TotalHash.Contains(deductioncode + "-" + payMonthYearHeader))
                                        TotalHash.Add(deductioncode + "-" + payMonthYearHeader, value);
                                    else
                                    {
                                        val = 0;
                                        double.TryParse(Convert.ToString(TotalHash[deductioncode + "-" + payMonthYearHeader]), out val);
                                        TotalHash.Remove(deductioncode + "-" + payMonthYearHeader);
                                        TotalHash.Add(deductioncode + "-" + payMonthYearHeader, val + value);
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Grand Total
                        DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Total";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        int k = 3;
                        DeductionDetSp.Sheets[0].RowCount++;
                        ArrayList DiffenceTotal = new ArrayList();
                        for (int i = 2; i < DeductionDetSp.Sheets[0].ColumnCount; i++)
                        {
                            string payMonthYear = Convert.ToString(DeductionDetSp.Sheets[0].ColumnHeader.Cells[1, i].Tag);
                            string deductioncode = Convert.ToString(DeductionDetSp.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                            double.TryParse(Convert.ToString(TotalHash[deductioncode + "-" + payMonthYear]), out val);
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 2, i].Text = Convert.ToString(val);
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 2, i].HorizontalAlign = HorizontalAlign.Right;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 2, i].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 2, i].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 2, i].ForeColor = Color.Maroon;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 2, i].Font.Bold = true;
                            DeductionDetSp.SaveChanges();
                            if (i == k)
                            {
                                double currentMonTotal = 0;
                                double prevMonTotal = 0;
                                double.TryParse(Convert.ToString(DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 2, i].Text), out currentMonTotal);
                                double.TryParse(Convert.ToString(DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 2, i - 1].Text), out prevMonTotal);
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].Text = Convert.ToString(currentMonTotal - prevMonTotal);
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Right;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].Font.Size = FontUnit.Medium;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].Font.Name = "Book Antiqua";
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].ForeColor = Color.Maroon;
                                DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, i].Font.Bold = true;
                                k += 2;
                                DiffenceTotal.Add(deductioncode + "$" + Convert.ToString(currentMonTotal - prevMonTotal));
                            }
                        }
                        DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Net Salary";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        double NetDifference = 0;
                        double NetCurrentSalary = 0;
                        double NetPreviousSalary = 0;
                        double.TryParse(Convert.ToString(CumulativeHash[5]), out NetPreviousSalary);
                        double.TryParse(Convert.ToString(CumulativeHash[6]), out NetCurrentSalary);
                        double.TryParse(Convert.ToString(CumulativeHash[7]), out NetDifference);
                        DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(prevMonthText + " - " + prevYear);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(NetPreviousSalary);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ddlmonth.SelectedItem.Text + " - " + ddlyear.SelectedItem.Value);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(NetCurrentSalary);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Difference";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(NetDifference);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        DeductionDetSp.Sheets[0].RowCount++; DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = "Net Difference";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(NetDifference);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        double TotolDiff = 0;
                        TotolDiff += NetDifference;
                        foreach (object ar in DiffenceTotal)
                        {
                            DeductionDetSp.Sheets[0].RowCount++;
                            string[] diffTotal = Convert.ToString(ar).Split('$');
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(diffTotal[0]);
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(diffTotal[1]);
                            val = 0;
                            double.TryParse(Convert.ToString(diffTotal[1]), out val);
                            TotolDiff += val;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].ForeColor = Color.Maroon;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        }
                        DeductionDetSp.Sheets[0].RowCount++;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(TotolDiff);
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].ForeColor = Color.Maroon;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        DeductionDetSp.Sheets[0].Cells[DeductionDetSp.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        DeductionDetSp.Sheets[0].PageSize = DeductionDetSp.Sheets[0].RowCount;
                        Deduction.Visible = true;
                        #endregion
                    }
                }
            }
            else
            {
                Deduction.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
        }
        catch { }
    }
    #region Common Checkbox and Checkboxlist Event
    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }
    #endregion
    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
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
            TextBox1.Text = "";
            string degreedetails = string.Empty;
            string pagename;
            // degreedetails = "Daily Fees Structure Report" + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "HRSalComparativeReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    protected void btnExcel_Click1(object sender, EventArgs e)
    {
        try
        {
            string reportname = TextBox1.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(DeductionDetSp, reportname);
                Label3.Visible = false;
            }
            else
            {
                Label3.Text = "Please Enter Your Report Name";
                Label3.Visible = true;
                TextBox1.Focus();
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
            string degreedetails = txtexcelname.Text;
            string pagename = "HRSalComparativeReport.aspx";
            Printcontrol.loadspreaddetails(DeductionDetSp, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
            #region Excel print settings
            string usertype = "";
            if (usercode.Trim() != "")
                usertype = " and usercode='" + usercode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch { }
    }
    #endregion
}