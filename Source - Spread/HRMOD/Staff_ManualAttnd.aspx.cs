using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Configuration;

public partial class Staff_ManualAttnd : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode1 = string.Empty;
    static string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    int ik = 0;

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
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                bindyear();
                binddept();
                category();
                stafftype();
                txtfrmdate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            mainerr.Visible = false;
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
        catch (Exception e) { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '" + prefixText + "%' and college_code='" + collegecode + "'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '" + prefixText + "%' and college_code='" + collegecode + "'";
        name = ws.Getname(query);
        return name;
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        FpSpread.Visible = false;
        lbldatespecify.Visible = false;
        btnsave.Visible = false;
        mainerr.Visible = false;
        lbldayscount.Visible = false;
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        bindyear();
        binddept();
        category();
        stafftype();
    }

    protected void ddlmon_change(object sender, EventArgs e)
    {
        try
        {
            string mon = Convert.ToString(ddlmon.SelectedItem.Value);
            //bindyear();
            string year = Convert.ToString(ddlyear.SelectedItem.Text);
            if (ddlyear.SelectedItem.Text.Trim() != "Select")
            {
                string selq = "select Convert(varchar(10),From_Date,103) as fromdate,Convert(varchar(10),To_Date,103) as todate from HRPayMonths where PayMonthNum='" + mon + "' and PayYear='" + year + "' and college_code='" + collegecode1 + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtfrmdate.Text = Convert.ToString(ds.Tables[0].Rows[0]["fromdate"]);
                    txttodate.Text = Convert.ToString(ds.Tables[0].Rows[0]["todate"]);
                }
                else
                {
                    txtfrmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                txtfrmdate.Text = "";
                txttodate.Text = "";
            }
            mainerr.Visible = false;
            lbldayscount.Visible = false;
            FpSpread.Visible = false;
            lbldatespecify.Visible = false;
            btnsave.Visible = false;
        }
        catch { }
    }

    protected void ddlyear_change(object sender, EventArgs e)
    {
        try
        {
            string mon = Convert.ToString(ddlmon.SelectedItem.Value);
            string year = Convert.ToString(ddlyear.SelectedItem.Text);
            if (ddlyear.SelectedItem.Text.Trim() != "Select")
            {
                string selq = "select Convert(varchar(10),From_Date,103) as fromdate,Convert(varchar(10),To_Date,103) as todate from HRPayMonths where PayMonthNum='" + mon + "' and PayYear='" + year + "' and college_code='" + collegecode1 + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtfrmdate.Text = Convert.ToString(ds.Tables[0].Rows[0]["fromdate"]);
                    txttodate.Text = Convert.ToString(ds.Tables[0].Rows[0]["todate"]);
                }
                else
                {
                    txtfrmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                txtfrmdate.Text = "";
                txttodate.Text = "";
            }
            mainerr.Visible = false;
            lbldayscount.Visible = false;
            FpSpread.Visible = false;
            lbldatespecify.Visible = false;
            btnsave.Visible = false;
        }
        catch { }
    }

    private void bindyear()
    {
        try
        {
            ds.Clear();
            ddlyear.Items.Clear();
            //string mon = Convert.ToString(ddlmon.SelectedItem.Value);
            string selq = "select distinct year(To_Date) as year from HRPayMonths where College_Code='" + collegecode1 + "' order by year asc";  //PayMonthNum='" + mon + "' and 
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlyear.DataSource = ds;
                ddlyear.DataTextField = "year";
                ddlyear.DataBind();
                ddlyear.Items.Insert(0, "Select");
            }
            else
            {
                ddlyear.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    private void binddept()
    {
        try
        {
            ds.Clear();
            ddldept.Items.Clear();
            string item = "select distinct dept_code,dept_name from hrdept_master where college_code = '" + collegecode1 + "' order by dept_name";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldept.DataSource = ds;
                ddldept.DataTextField = "dept_name";
                ddldept.DataValueField = "dept_code";
                ddldept.DataBind();
                ddldept.Items.Insert(0, "All");
            }
            else
            {
                ddldept.Items.Insert(0, "All");
            }
        }
        catch { }
    }

    private void category()
    {
        try
        {
            ds.Clear();
            ddlstfcat.Items.Clear();
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + collegecode1 + "' order by category_Name";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstfcat.DataSource = ds;
                ddlstfcat.DataTextField = "category_Name";
                ddlstfcat.DataValueField = "category_code";
                ddlstfcat.DataBind();
                ddlstfcat.Items.Insert(0, "All");
            }
            else
            {
                ddlstfcat.Items.Insert(0, "All");
            }
        }
        catch { }
    }

    private void stafftype()
    {
        try
        {
            ds.Clear();
            ddlstftype.Items.Clear();
            string item = "select distinct stftype from stafftrans t,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collegecode1 + "' order by stftype";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstftype.DataSource = ds;
                ddlstftype.DataTextField = "stftype";
                ddlstftype.DataBind();
                ddlstftype.Items.Insert(0, "All");
            }
            else
            {
                ddlstftype.Items.Insert(0, "All");
            }
        }
        catch { }
    }

    protected void txt_stfcode_change(object sender, EventArgs e)
    {
        txt_stfname.Text = "";
    }

    protected void txt_stfname_change(object sender, EventArgs e)
    {
        txt_stfcode.Text = "";
    }

    protected void btn_go_click(object sender, EventArgs e)
    {
        try
        {
            loadspread();
        }
        catch { }
    }

    public void loadheader()
    {
        string[] spl = new string[2];
        FpSpread.Sheets[0].RowCount = 0;
        FpSpread.Sheets[0].ColumnCount = 12;
        FpSpread.Sheets[0].AutoPostBack = false;
        FpSpread.Sheets[0].RowHeader.Visible = false;
        FpSpread.CommandBar.Visible = false;
        FpSpread.Sheets[0].FrozenRowCount = 1;

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.Black;
        FpSpread.Sheets[0].ColumnHeader.DefaultStyle = darkstyle;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpSpread.Columns[0].Width = 75;
        FpSpread.Columns[0].Locked = true;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        FpSpread.Columns[1].Width = 100;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        FpSpread.Columns[2].Width = 125;
        FpSpread.Columns[2].Locked = true;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        FpSpread.Columns[3].Width = 225;
        FpSpread.Columns[3].Locked = true;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        FpSpread.Columns[4].Width = 300;
        FpSpread.Columns[4].Locked = true;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Previous Month Working Days";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
        FpSpread.Columns[5].Width = 100;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Previous Month Present Days";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
        FpSpread.Columns[6].Width = 100;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Previous Month LOP Days";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
        FpSpread.Columns[7].Width = 100;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Current Month Working Days";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
        FpSpread.Columns[8].Width = 100;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Current Month Present Days";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
        FpSpread.Columns[9].Width = 100;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Current Month LOP Days";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
        FpSpread.Columns[10].Width = 100;

        FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Text = "LOP Date";
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
        FpSpread.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
        FpSpread.Columns[10].Width = 100;

        DateTime dtfrm = new DateTime();
        DateTime dtto = new DateTime();
        if (txtfrmdate.Text.Trim() != "" && txttodate.Text.Trim() != "")
        {
            spl = Convert.ToString(txtfrmdate.Text).Split('/');
            dtfrm = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
            spl = Convert.ToString(txttodate.Text).Split('/');
            dtto = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
        }
        if (dtfrm.Month == dtto.Month)
        {
            FpSpread.Columns[5].Visible = false;
            FpSpread.Columns[6].Visible = false;
            FpSpread.Columns[7].Visible = false;
        }
        else
        {
            FpSpread.Columns[5].Visible = true;
            FpSpread.Columns[6].Visible = true;
            FpSpread.Columns[7].Visible = true;
        }
    }

    public void loadspread()
    {
        try
        {
            FpSpread.SaveChanges();
            string[] spl = new string[2];

            string selq = "select sm.staff_name,sm.staff_code,h.dept_name from staffmaster sm,stafftrans st,hrdept_master h,staffcategorizer sc where sm.staff_Code=st.staff_code and sm.college_code=h.college_code and sc.college_code=sm.college_code and st.dept_code=h.dept_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign=0 and sm.settled=0 and ISNULL(Discontinue,'0')='0' and sm.college_code='" + collegecode1 + "'";
            if (chkmanuallop.Checked == true)
            {
                selq = selq + " and st.IsManualLOP='1'";
            }
            if (txt_stfcode.Text.Trim() != "")
            {
                selq = selq + " and sm.staff_code='" + Convert.ToString(txt_stfcode.Text).Trim() + "'";
            }
            else if (txt_stfname.Text.Trim() != "")
            {
                selq = selq + " and sm.staff_name='" + Convert.ToString(txt_stfname.Text).Trim() + "'";
            }
            else
            {
                if (ddldept.SelectedItem.Text != "All")
                {
                    selq = selq + " and h.dept_code='" + Convert.ToString(ddldept.SelectedItem.Value) + "'";
                }
                if (ddlstfcat.SelectedItem.Text != "All")
                {
                    selq = selq + " and st.category_code='" + Convert.ToString(ddlstfcat.SelectedItem.Value) + "'";
                }
                if (ddlstftype.SelectedItem.Text != "All")
                {
                    selq = selq + " and st.stftype='" + Convert.ToString(ddlstftype.SelectedItem.Value) + "'";
                }
            }
            selq = selq + " order by st.dept_code";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (txtfrmdate.Text.Trim() != "" && txttodate.Text.Trim() != "")
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    loadheader();
                    spl = Convert.ToString(txtfrmdate.Text).Split('/');
                    DateTime dtfrm = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
                    spl = Convert.ToString(txttodate.Text).Split('/');
                    DateTime dtto = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
                    if (dtfrm == dtto)
                    {
                        mainerr.Visible = true;
                        mainerr.Text = "Please Select the Corresponding HR PayMonths!";
                        btnsave.Visible = false;
                        FpSpread.Visible = false;
                        lbldatespecify.Visible = false;
                        lbldayscount.Visible = false;
                        return;
                    }
                    lbldayscount.Visible = true;
                    if (dtfrm.Month == dtto.Month)
                    {
                        lbldayscount.Text = "Current Month Days Count  :  " + Convert.ToString((dtto.Day - dtfrm.Day) + 1) + "";
                    }
                    else
                    {
                        lbldayscount.Text = "Previous Month Days Count  :  " + Convert.ToString((DateTime.DaysInMonth(dtfrm.Year, dtfrm.Month) - dtfrm.Day) + 1) + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Current Month Days Count  :  " + Convert.ToString((dtto.Day - 1) + 1) + "";
                    }

                    FarPoint.Web.Spread.DoubleCellType dblcell = new FarPoint.Web.Spread.DoubleCellType();
                    dblcell.MaximumValue = 31;
                    dblcell.ErrorMessage = "Allow Only Numerics & Allow Days Limit!";

                    FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                    txtcell.Multiline = true;

                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkall.AutoPostBack = true;

                    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                    chkcell.AutoPostBack = false;

                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkall;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    for (ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                    {
                        FpSpread.Sheets[0].RowCount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ik + 1);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[ik]["staff_code"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[ik]["staff_name"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[ik]["dept_name"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].CellType = dblcell;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = "";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].CellType = dblcell;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = "";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].CellType = dblcell;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Text = "";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].CellType = dblcell;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Text = "";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].CellType = dblcell;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Text = "";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].CellType = dblcell;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Text = "";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].CellType = txtcell;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Text = "";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                    }
                    FpSpread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                    FpSpread.Width = 900;
                    FpSpread.Height = 400;
                    FpSpread.Visible = true;
                    lbldatespecify.Visible = true;
                    lbldatespecify.Text = "(Enter the LOP Date like 'dd/m/yyyy,dd/m/yyyy')";
                    mainerr.Visible = false;
                    btnsave.Visible = true;
                }
                else
                {
                    mainerr.Visible = true;
                    mainerr.Text = "No Records Found!";
                    lbldayscount.Visible = false;
                    FpSpread.Visible = false;
                    lbldatespecify.Visible = false;
                    btnsave.Visible = false;
                }
            }
            else
            {
                mainerr.Visible = true;
                mainerr.Text = "Please Update HR Year!";
                lbldayscount.Visible = false;
                FpSpread.Visible = false;
                lbldatespecify.Visible = false;
                btnsave.Visible = false;
            }
        }
        catch { }
    }

    protected void FpSpread_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[0, 1].Value);
            if (check == 1)
            {
                for (int j = 1; j < FpSpread.Sheets[0].RowCount; j++)
                {
                    FpSpread.Sheets[0].Cells[j, 1].Value = 1;
                }
            }
            else
            {
                for (int j = 1; j < FpSpread.Sheets[0].RowCount; j++)
                {
                    FpSpread.Sheets[0].Cells[j, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    public bool checkedOK(FarPoint.Web.Spread.FpSpread spread)
    {
        bool Ok = false;
        spread.SaveChanges();
        for (int i = 0; i < spread.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(spread.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                Ok = true;
            }
        }
        return Ok;
    }

    public bool checkedstrOK(FarPoint.Web.Spread.FpSpread spread, out string lblerr)
    {
        lblerr = "";
        bool Ok = false;
        int prevmonwrkdays = 0;
        int prevmonpresdays = 0;
        int prevmonlopdays = 0;
        int currmonwrkdays = 0;
        int currmonpresdays = 0;
        int currmonlopdays = 0;
        string lopdates = "";
        string frmmonyr = "";
        string tomonyr = "";
        string[] spl = new string[2];
        spl = Convert.ToString(txtfrmdate.Text).Split('/');
        int year = 0;
        Int32.TryParse(Convert.ToString(spl[2]), out year);
        int month = 0;
        Int32.TryParse(Convert.ToString(spl[1]), out month);
        int day = 0;
        Int32.TryParse(Convert.ToString(spl[0]), out day);
        DateTime dtfrm = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
        spl = Convert.ToString(txttodate.Text).Split('/');
        DateTime dtto = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
        frmmonyr = dtfrm.Month + "/" + dtfrm.Year;
        tomonyr = dtto.Month + "/" + dtto.Year;
        int days = DateTime.DaysInMonth(year, month);
        int prevdays = (days - day) + 1;
        int currdays = dtto.Day;
        string[] spldate = new string[31];
        DateTime dttest = new DateTime();

        spread.SaveChanges();
        for (int i = 0; i < spread.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(spread.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                string staffcode = Convert.ToString(spread.Sheets[0].Cells[i, 2].Text);
                string currmonlop = Convert.ToString(spread.Sheets[0].Cells[i, spread.Sheets[0].ColumnCount - 2].Text);
                string currmonpres = Convert.ToString(spread.Sheets[0].Cells[i, spread.Sheets[0].ColumnCount - 3].Text);
                string currmonwrk = Convert.ToString(spread.Sheets[0].Cells[i, spread.Sheets[0].ColumnCount - 4].Text);
                string prevmonlop = Convert.ToString(spread.Sheets[0].Cells[i, spread.Sheets[0].ColumnCount - 5].Text);
                string prevmonpres = Convert.ToString(spread.Sheets[0].Cells[i, spread.Sheets[0].ColumnCount - 6].Text);
                string prevmonwrk = Convert.ToString(spread.Sheets[0].Cells[i, spread.Sheets[0].ColumnCount - 7].Text);
                lopdates = Convert.ToString(spread.Sheets[0].Cells[i, spread.Sheets[0].ColumnCount - 1].Text);
                //if (lopdates.Trim() == "")//19.01.18
                //{
                //Ok = false;
                //lblerr = "Please Enter LOP Dates for Staff  :   " + staffcode;
                //return Ok;
                //}
                //else
                //{
                if (lopdates.Trim() != "")
                {
                    spldate = Convert.ToString(lopdates).Split(',');
                    if (spldate.Length > 0)
                    {
                        for (int my = 0; my < spldate.Length; my++)
                        {
                            if (DateTime.TryParseExact(spldate[my], "dd/M/yyyy", null, DateTimeStyles.None, out dttest) == true)
                            {
                                if (!String.IsNullOrEmpty(spldate[my]) && spldate[my].Contains('/'))
                                {
                                    spl = spldate[my].Split('/');
                                    if (spl.Length < 3 || spl.Length > 3)
                                    {
                                        Ok = false;
                                        lblerr = "Please Enter Valid LOP Date for Staff  :   " + staffcode;
                                        return Ok;
                                    }
                                    else if (spl.Length == 3)
                                    {
                                        if (Convert.ToString(spl[1] + "/" + spl[2]) == Convert.ToString(frmmonyr) || Convert.ToString(spl[1] + "/" + spl[2]) == Convert.ToString(tomonyr))
                                        {

                                        }
                                        else
                                        {
                                            Ok = false;
                                            lblerr = "Please Enter Valid LOP Date for Staff  :   " + staffcode;
                                            return Ok;
                                        }
                                    }
                                }
                                else
                                {
                                    Ok = false;
                                    lblerr = "Please Enter Valid LOP Date for Staff  :   " + staffcode;
                                    return Ok;
                                }
                            }
                            else
                            {
                                Ok = false;
                                lblerr = "Please Enter Valid LOP Date for Staff  :   " + staffcode;
                                return Ok;
                            }
                        }
                    }
                }

                Int32.TryParse(currmonlop, out currmonlopdays);
                Int32.TryParse(currmonpres, out currmonpresdays);
                Int32.TryParse(currmonwrk, out currmonwrkdays);
                Int32.TryParse(prevmonlop, out prevmonlopdays);
                Int32.TryParse(prevmonpres, out prevmonpresdays);
                Int32.TryParse(prevmonwrk, out prevmonwrkdays);

                if (dtfrm.Month == dtto.Month)
                {
                    //if (currmonwrkdays != 0 && currmonpresdays != 0)
                    //{
                    if (days >= currmonwrkdays)
                    {
                        if (currmonwrkdays >= currmonpresdays)
                        {
                            if (currmonlopdays != 0)
                            {
                                if (currmonlopdays == (currmonwrkdays - currmonpresdays))
                                {
                                    Ok = true;
                                }
                                else
                                {
                                    Ok = false;
                                    lblerr = "LOP Days you have entered is Invalid for Staff  :   " + staffcode;
                                    return Ok;
                                }
                            }
                            else
                            {
                                Ok = true;
                            }
                        }
                        else
                        {
                            Ok = false;
                            lblerr = "Present Days Exceed From Working Days for Staff  :   " + staffcode;
                            return Ok;
                        }
                    }
                    else
                    {
                        Ok = false;
                        lblerr = "Working Days Exceed From MonthDays for Staff   :    " + staffcode;
                        return Ok;
                    }
                    //}
                    //else
                    //{
                    //    Ok = false;
                    //    lblerr = "Please Enter Working Days & Present Days for Staff    :     " + staffcode;
                    //    return Ok;
                    //}
                }
                else
                {
                    if (prevmonpresdays != 0 && prevmonwrkdays != 0 && currmonwrkdays != 0 && currmonpresdays != 0)
                    {
                        if (prevdays >= prevmonwrkdays)
                        {
                            if (prevmonwrkdays >= prevmonpresdays)
                            {
                                if (prevmonlopdays != 0)
                                {
                                    if (prevmonlopdays == (prevmonwrkdays - prevmonpresdays))
                                    {
                                        Ok = true;
                                    }
                                    else
                                    {
                                        Ok = false;
                                        lblerr = "LOP Days from Previous Month you have entered is Invalid for Staff  :   " + staffcode;
                                        return Ok;
                                    }
                                }
                                else
                                {
                                    Ok = true;
                                }
                            }
                            else
                            {
                                Ok = false;
                                lblerr = "Previous Month Present Days Exceed From Working Days for Staff    :     " + staffcode;
                                return Ok;
                            }
                        }
                        else
                        {
                            Ok = false;
                            lblerr = "Previous Month Working Days Exceed for    :     " + staffcode;
                            return Ok;
                        }

                        if (currdays >= currmonwrkdays)
                        {
                            if (currmonwrkdays >= currmonpresdays)
                            {
                                if (currmonlopdays != 0)
                                {
                                    if (currmonlopdays == (currmonwrkdays - currmonpresdays))
                                    {
                                        Ok = true;
                                    }
                                    else
                                    {
                                        Ok = false;
                                        lblerr = "LOP Days from Current Month you have entered is Invalid for Staff  :   " + staffcode;
                                        return Ok;
                                    }
                                }
                                else
                                {
                                    Ok = true;
                                }
                            }
                            else
                            {
                                Ok = false;
                                lblerr = "Current Month Present Days Exceed From Working Days for Staff    :     " + staffcode;
                                return Ok;
                            }
                        }
                        else
                        {
                            Ok = false;
                            lblerr = "Current Month Working Days Exceed for    :     " + staffcode;
                            return Ok;
                        }
                    }
                    else
                    {
                        Ok = false;
                        lblerr = "Please Enter Working Days & Present Days for Previous & Current Month for Staff    :     " + staffcode;
                        return Ok;
                    }
                }
            }
        }
        return Ok;
    }

    protected void btnsave_click(object sender, EventArgs e)
    {
        try
        {
            string err = "";
            FpSpread.SaveChanges();
            if (checkedOK(FpSpread))
            {
                if (checkedstrOK(FpSpread, out err))
                {
                    int inscount = 0;
                    mainerr.Visible = false;
                    string[] spl = new string[2];
                    spl = Convert.ToString(txtfrmdate.Text).Split('/');
                    DateTime dt = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
                    spl = Convert.ToString(txttodate.Text).Split('/');
                    DateTime dtend = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
                    DateTime dt1 = new DateTime();
                    DateTime dtend1 = new DateTime();
                    SortedDictionary<string, string> dicDt = new SortedDictionary<string, string>();
                    dicDt.Clear();
                    StringBuilder NewString = new StringBuilder();

                    for (ik = 1; ik < FpSpread.Sheets[0].RowCount; ik++)
                    {
                        byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[ik, 1].Value);
                        if (check == 1)
                        {
                            dt1 = dt;
                            dtend1 = dtend;
                            string staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 2].Text);

                            Double currmonlop = 0.0;
                            Double currmonpres = 0.0;
                            Double currmonwrk = 0.0;
                            Double prevmonlop = 0.0;
                            Double prevmonpres = 0.0;
                            Double prevmonwrk = 0.0;
                            string lopdates = "";

                            Double.TryParse(Convert.ToString(FpSpread.Sheets[0].Cells[ik, FpSpread.Sheets[0].ColumnCount - 2].Text), out currmonlop);
                            Double.TryParse(Convert.ToString(FpSpread.Sheets[0].Cells[ik, FpSpread.Sheets[0].ColumnCount - 3].Text), out currmonpres);
                            Double.TryParse(Convert.ToString(FpSpread.Sheets[0].Cells[ik, FpSpread.Sheets[0].ColumnCount - 4].Text), out currmonwrk);
                            Double.TryParse(Convert.ToString(FpSpread.Sheets[0].Cells[ik, FpSpread.Sheets[0].ColumnCount - 5].Text), out prevmonlop);
                            Double.TryParse(Convert.ToString(FpSpread.Sheets[0].Cells[ik, FpSpread.Sheets[0].ColumnCount - 6].Text), out prevmonpres);
                            Double.TryParse(Convert.ToString(FpSpread.Sheets[0].Cells[ik, FpSpread.Sheets[0].ColumnCount - 7].Text), out prevmonwrk);
                            lopdates = Convert.ToString(FpSpread.Sheets[0].Cells[ik, FpSpread.Sheets[0].ColumnCount - 1].Text);

                            if (lopdates.Trim() != "")
                            {
                                string[] spldate = lopdates.Split(',');
                                Array.Sort(spldate, StringComparer.InvariantCulture);
                                string myinsq = "";
                                int myinscount = 0;
                                string[] splmyDate = new string[2];
                                for (int day = 0; day < spldate.Length; day++)
                                {
                                    splmyDate = spldate[day].Split('/');
                                    if (splmyDate.Length == 3)
                                    {
                                        if (!dicDt.ContainsKey(Convert.ToString(splmyDate[0] + "," + splmyDate[1] + "/" + splmyDate[2])))
                                        {
                                            dicDt.Add(Convert.ToString(splmyDate[0] + "," + splmyDate[1] + "/" + splmyDate[2]), splmyDate[0]);
                                            NewString.Append(Convert.ToString(splmyDate[0] + "/" + splmyDate[1] + "/" + splmyDate[2]) + ",");
                                        }
                                    }
                                }
                                if (NewString.Length > 0)
                                {
                                    NewString.Remove(NewString.Length - 1, 1);
                                }

                                while (dt1 <= dtend1)
                                {
                                    if (dicDt.Count > 0)
                                    {
                                        foreach (KeyValuePair<string, string> dr in dicDt)
                                        {
                                            if (dt1.Day == Convert.ToInt32(dr.Value) && Convert.ToString(dt1.Month + "/" + dt1.Year) == Convert.ToString(dr.Key).Split(',')[1])
                                            {
                                                myinsq = "if exists(select [" + dt1.Day + "] from staff_attnd where staff_code='" + staffcode + "' and ([" + dt1.Day + "]='' or [" + dt1.Day + "]=' - ' or ISNULL([" + dt1.Day + "],'0')='0' )and [" + dt1.Day + "]<>'H-H' and mon_year='" + Convert.ToString(dr.Key).Split(',')[1] + "') update staff_attnd set [" + dt1.Day + "]='A-A' where staff_code='" + staffcode + "' and ([" + dt1.Day + "]='' or [" + dt1.Day + "]=' - ' or ISNULL([" + dt1.Day + "],'0')='0' )and [" + dt1.Day + "]<>'H-H' and mon_year='" + Convert.ToString(dr.Key).Split(',')[1] + "'";

                                                myinscount = d2.update_method_wo_parameter(myinsq, "Text");
                                            }
                                        }
                                    }
                                    myinsq = "if exists(select [" + dt1.Day + "] from staff_attnd where staff_code='" + staffcode + "' and ([" + dt1.Day + "]='' or [" + dt1.Day + "]=' - ' or ISNULL([" + dt1.Day + "],'0')='0' )and [" + dt1.Day + "]<>'H-H' and mon_year='" + Convert.ToString(dt1.Month + "/" + dt1.Year) + "') update staff_attnd set [" + dt1.Day + "]='P-P' where staff_code='" + staffcode + "' and ([" + dt1.Day + "]='' or [" + dt1.Day + "]=' - ' or ISNULL([" + dt1.Day + "],'0')='0' )and [" + dt1.Day + "]<>'H-H' and mon_year='" + Convert.ToString(dt1.Month + "/" + dt1.Year) + "'";

                                    myinscount = d2.update_method_wo_parameter(myinsq, "Text");
                                    dt1 = dt1.AddDays(1);
                                }
                            }
                            string insq = "";
                            insq = "if exists(select * from StaffLOP_Details where staff_code='" + staffcode + "' and PayMonth='" + Convert.ToString(ddlmon.SelectedItem.Value) + "' and PayYear='" + Convert.ToString(ddlyear.SelectedItem.Text) + "' and College_Code='" + collegecode1 + "') update StaffLOP_Details set LOP_FromDate='" + dt.ToString("MM/dd/yyyy") + "',LOP_ToDate='" + dtend.ToString("MM/dd/yyyy") + "',Second_LOP_Days='" + currmonlop + "',Second_WorkDays='" + currmonwrk + "',Second_PresDays='" + currmonpres + "',First_LOP_Days='" + prevmonlop + "',First_WorkDays='" + prevmonwrk + "',First_PresDays='" + prevmonpres + "',LodDate='" + NewString.ToString() + "' where Staff_Code='" + staffcode + "' and College_Code='" + collegecode1 + "' and PayMonth='" + Convert.ToString(ddlmon.SelectedItem.Value) + "' and PayYear='" + Convert.ToString(ddlyear.SelectedItem.Text) + "' else insert into StaffLOP_Details (Staff_Code,LOP_FromDate,LOP_ToDate,Second_LOP_Days,Second_WorkDays,Second_PresDays,First_LOP_Days,First_WorkDays,First_PresDays,College_Code,PayMonth,PayYear,LodDate) values ('" + staffcode + "','" + dt.ToString("MM/dd/yyyy") + "','" + dtend.ToString("MM/dd/yyyy") + "','" + currmonlop + "','" + currmonwrk + "','" + currmonpres + "','" + prevmonlop + "','" + prevmonwrk + "','" + prevmonpres + "','" + collegecode1 + "','" + Convert.ToString(ddlmon.SelectedItem.Value) + "','" + Convert.ToString(ddlyear.SelectedItem.Text) + "','" + NewString.ToString() + "')";
                            int upcount = d2.update_method_wo_parameter(insq, "Text");
                            if (upcount > 0)
                            {
                                inscount++;
                            }
                        }
                    }
                    if (inscount > 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Saved Successfully!";
                    }
                }
                else
                {
                    mainerr.Visible = true;
                    mainerr.Text = err;
                }
            }
            else
            {
                mainerr.Visible = true;
                mainerr.Text = "Please Select Any Staff!";
            }
        }
        catch { }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
}

//------------------------------Last Modified on Oct 21st,2016------------------------//
//-------------LOP Days Column Added on Spread By Jeyaprakash on Oct 21st------------//