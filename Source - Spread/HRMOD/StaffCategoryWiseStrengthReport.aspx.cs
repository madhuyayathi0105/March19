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

public partial class HRMOD_StaffCategoryWiseStrengthReport : System.Web.UI.Page
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
                BindStaffCategory();
                BindStaffType();
                BindMonth();
                BindYear();
            }
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
    }

    public void BindStaffCategory()
    {
        try
        {
            cbl_Category.Items.Clear();
            string Collcode = rs.GetSelectedItemsValue(cbl_college);
            string Query = string.Empty;
            if (Collcode.Trim() != "")
            {
                Query = "select distinct Category_name from staffcategorizer where college_Code in (" + Collcode + ")";
            }
            else
            {
                Query = "select distinct Category_name from staffcategorizer";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_Category.DataSource = ds;
                cbl_Category.DataTextField = "Category_name";
                cbl_Category.DataValueField = "Category_name";
                cbl_Category.DataBind();
                cb_Category.Checked = true;
                CallCheckBoxChangedEvent(cbl_Category, cb_Category, txt_Category, "Category");
            }
        }
        catch
        {

        }
    }

    public void BindStaffType()
    {
        try
        {
            cbl_stafftype.Items.Clear();
            string Query = "select distinct stftype from stafftrans";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_stafftype.DataSource = ds;
                cbl_stafftype.DataTextField = "stftype";
                cbl_stafftype.DataValueField = "stftype";
                cbl_stafftype.DataBind();
                cb_stafftype.Checked = true;
                CallCheckBoxChangedEvent(cbl_stafftype, cb_stafftype, txt_stafftype, "Staff Type");
            }
        }
        catch
        {

        }
    }

    public void bindcollege()
    {
        try
        {
            ds.Clear();
            cbl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_college.DataSource = ds;
                cbl_college.DataTextField = "collname";
                cbl_college.DataValueField = "college_code";
                cbl_college.DataBind();
                cb_college.Checked = true;
                CallCheckBoxChangedEvent(cbl_college, cb_college, txt_college, "College");
            }
        }
        catch
        {
        }
    }

    public void BindMonth()
    {
        try
        {
            string Collcode = rs.GetSelectedItemsValue(cbl_college);
            string Query = string.Empty;
            if (Collcode.Trim() != "")
            {
                Query = "select distinct PaymonthNum,Paymonth from hrpaymonths where college_code in (" + Collcode + ")";
            }
            else
            {
                Query = "select distinct PaymonthNum,Paymonth from hrpaymonths";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_Month.DataSource = ds;
                cbl_Month.DataTextField = "Paymonth";
                cbl_Month.DataValueField = "PaymonthNum";
                cbl_Month.DataBind();
                cb_Month.Checked = true;
                CallCheckBoxChangedEvent(cbl_Month, cb_Month, txt_Month, "Month");
            }
        }
        catch
        {

        }
    }

    public void BindYear()
    {
        try
        {
            string Collcode = rs.GetSelectedItemsValue(cbl_college);
            string Query = string.Empty;
            if (Collcode.Trim() != "")
            {
                Query = "select distinct Payyear from hrpaymonths where college_code in (" + Collcode + ") order by Payyear";
            }
            else
            {
                Query = "select distinct Payyear from hrpaymonths order by Payyear";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlyear.DataSource = ds;
                ddlyear.DataTextField = "Payyear";
                ddlyear.DataValueField = "Payyear";
                ddlyear.DataBind();

            }
        }
        catch
        {

        }
    }
    protected void cb_college_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxChangedEvent(cbl_college, cb_college, txt_college, "College");
            BindStaffCategory();
        }
        catch
        {

        }
    }

    protected void cbl_college_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxListChangedEvent(cbl_college, cb_college, txt_college, "College");
            BindStaffCategory();
        }
        catch
        {

        }
    }

    protected void cb_stafftype_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxChangedEvent(cbl_stafftype, cb_stafftype, txt_stafftype, "Staff Type");
        }
        catch
        {

        }
    }

    protected void cbl_stafftype_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxListChangedEvent(cbl_stafftype, cb_stafftype, txt_stafftype, "Staff Type");
        }
        catch
        {

        }
    }

    protected void cb_Category_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxChangedEvent(cbl_Category, cb_Category, txt_Category, "Category");
        }
        catch
        {

        }
    }

    protected void cbl_Category_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxListChangedEvent(cbl_Category, cb_Category, txt_Category, "Category");
        }
        catch
        {

        }
    }

    protected void cb_Month_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxChangedEvent(cbl_Month, cb_Month, txt_Month, "Month");
        }
        catch
        {
        }
    }
    protected void cbl_Month_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxListChangedEvent(cbl_Month, cb_Month, txt_Month, "Month");
        }
        catch
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string College = string.Empty;
            string staffcategory = string.Empty;
            string stafftype = string.Empty;
            string Month = string.Empty;
            string year = string.Empty;
            College = rs.GetSelectedItemsValue(cbl_college);
            staffcategory = rs.GetSelectedItemsText(cbl_Category);
            stafftype = rs.GetSelectedItemsText(cbl_stafftype);
            Month = rs.GetSelectedItemsValue(cbl_Month);
            List<string> Lis = new List<string>();
            Lis = rs.GetSelectedItemsValueList(cbl_Month);
            year = ddlyear.SelectedItem.Text;
            int StafType = 0;
            int Category = 0;
            int MonthNum = 0;
            DataTable DtMain = new DataTable();
            DataTable DFSC = new DataTable();
            DataTable DFST = new DataTable();
            DataView Dv = new DataView();
            Hashtable has = new Hashtable();
            Hashtable RowIndex = new Hashtable();
            bool Check = false;
            if (College.Trim() != "" && staffcategory.Trim() != "" && stafftype.Trim() != "" && Month.Trim() != "")
            {
                //string Query = "select count(s.staff_code) as Total,sc.category_Name,S.college_code,t.stftype,resign,settled,join_date,relieve_date from staffmaster s,stafftrans t,staffcategorizer sc where s.staff_code=t.staff_code and t.category_code=sc.category_code and s.college_code=sc.college_Code and Latestrec='1' and s.college_code in (" + College + ") and sc.category_name in ('" + staffcategory + "') and t.stftype in ('" + stafftype + "') group by sc.category_Name,S.college_code,t.stftype,resign,settled,join_date,relieve_date  order by s.College_code";
                 string Query = "select count(s.staff_code) as Total,sc.category_Name,S.college_code,t.stftype,resign,settled,join_date,relieve_date from  staff_appl_master sa,staffmaster s,stafftrans t,hrdept_master h,desig_master d,staffcategorizer sc where s.staff_code =t.staff_code and s.appl_no =sa.appl_no and h.dept_code =t.dept_code and t.desig_code =d.desig_code and sc.category_code =t.category_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=sc.college_code and Latestrec='1' and s.college_code in (" + College + ") and sc.category_name in ('" + staffcategory + "') and t.stftype in ('" + stafftype + "') group by sc.category_Name,S.college_code,t.stftype,resign,settled,join_date,relieve_date  order by s.College_code";
                            
                Query += " select payMonthNum,PayYear,To_Date,College_code from Hrpaymonths";
                Query += " select collname,coll_acronymn,acr,college_code from collinfo";
                ds.Clear();
                ds = d2.select_method_wo_parameter(Query, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DtMain = ds.Tables[0].DefaultView.ToTable();
                    DFSC = ds.Tables[0].DefaultView.ToTable(true, "category_Name");
                    DFST = ds.Tables[0].DefaultView.ToTable(true, "stftype");

                    Farpont1.Sheets[0].ColumnHeader.RowCount = 3;
                    Farpont1.Sheets[0].ColumnCount = 2;
                    Farpont1.Sheets[0].RowHeader.Visible = false;
                    Farpont1.Sheets[0].AutoPostBack = true;
                    Farpont1.Sheets[0].RowCount = 0;
                    Farpont1.CommandBar.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Farpont1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


                    Farpont1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Farpont1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Farpont1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                    Farpont1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "College Name";
                    Farpont1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Farpont1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                    if (DFST.Rows.Count > 0)
                    {
                        for (int intST = 0; intST < DFST.Rows.Count; intST++)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "stftype='" + Convert.ToString(DFST.Rows[intST]["stftype"]) + "'";
                            Dv = ds.Tables[0].DefaultView;
                            if (Dv.Count > 0)
                            {
                                Farpont1.Sheets[0].ColumnCount++;
                                Farpont1.Sheets[0].ColumnHeader.Cells[0, Farpont1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(DFST.Rows[intST]["stftype"]);
                                Farpont1.Sheets[0].ColumnHeader.Cells[0, Farpont1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                StafType = Farpont1.Sheets[0].ColumnCount - 1;
                                DFSC = Dv.ToTable(true, "category_Name");
                                if (DFSC.Rows.Count > 0)
                                {
                                    for (int intSC = 0; intSC < DFSC.Rows.Count; intSC++)
                                    {
                                        if (intSC != 0)
                                        {
                                            Farpont1.Sheets[0].ColumnCount++;
                                        }
                                        Category = Farpont1.Sheets[0].ColumnCount - 1;

                                        Farpont1.Sheets[0].ColumnHeader.Cells[1, Farpont1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(DFSC.Rows[intSC]["category_Name"]);
                                        Farpont1.Sheets[0].ColumnHeader.Cells[1, Farpont1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        if (cbl_Month.Items.Count > 0)
                                        {
                                            Check = false;
                                            for (int intMont = 0; intMont < cbl_Month.Items.Count; intMont++)
                                            {
                                                if (cbl_Month.Items[intMont].Selected == true)
                                                {
                                                    if (Check == true)
                                                    {
                                                        Farpont1.Sheets[0].ColumnCount++;
                                                    }
                                                    else
                                                    {
                                                        MonthNum = Farpont1.Sheets[0].ColumnCount - 1;
                                                    }
                                                    Farpont1.Sheets[0].ColumnHeader.Cells[2, Farpont1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_Month.Items[intMont].Text + " " + year);
                                                    Farpont1.Sheets[0].ColumnHeader.Cells[2, Farpont1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_Month.Items[intMont].Value + " $" + year);
                                                    Farpont1.Sheets[0].ColumnHeader.Cells[2, Farpont1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(DFSC.Rows[intSC]["category_Name"] + "$" + DFST.Rows[intST]["stftype"]);
                                                    Farpont1.Sheets[0].ColumnHeader.Cells[2, Farpont1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                    Check = true;
                                                }
                                            }
                                        }
                                        Farpont1.Sheets[0].ColumnHeaderSpanModel.Add(1, (Category), 1, ((Farpont1.Sheets[0].ColumnCount) - Category));
                                    }
                                }
                                Farpont1.Sheets[0].ColumnHeaderSpanModel.Add(0, (StafType), 1, ((Farpont1.Sheets[0].ColumnCount) - StafType));
                            }
                        }
                    }
                    int Colcount = Farpont1.Sheets[0].ColumnCount;
                    Farpont1.Sheets[0].ColumnCount++;
                    Farpont1.Sheets[0].ColumnHeader.Cells[0, Farpont1.Sheets[0].ColumnCount - 1].Text = "Grand Total";
                    Farpont1.Sheets[0].ColumnHeader.Cells[0, Farpont1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    int NumberCount = 0;
                    if (cbl_Month.Items.Count > 0)
                    {
                        Check = false;
                        for (int intMont = 0; intMont < cbl_Month.Items.Count; intMont++)
                        {
                            if (cbl_Month.Items[intMont].Selected == true)
                            {
                                NumberCount++;
                                if (Check == true)
                                {
                                    Farpont1.Sheets[0].ColumnCount++;
                                }
                                else
                                {
                                    MonthNum = Farpont1.Sheets[0].ColumnCount - 1;
                                }
                                Farpont1.Sheets[0].ColumnHeader.Cells[2, Farpont1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_Month.Items[intMont].Text + " " + year);
                                Farpont1.Sheets[0].ColumnHeader.Cells[2, Farpont1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_Month.Items[intMont].Value + "$" + year);
                                Farpont1.Sheets[0].ColumnHeader.Cells[2, Farpont1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                has.Add(Convert.ToString(cbl_Month.Items[intMont].Value + "$" + year), Farpont1.Sheets[0].ColumnCount - 1);
                                Check = true;
                            }
                        }
                    }
                    Farpont1.Sheets[0].ColumnHeaderSpanModel.Add(0, Colcount, 2, (Farpont1.Sheets[0].ColumnCount - Colcount));
                    DFSC = DtMain.DefaultView.ToTable(true, "College_code");
                    if (DFSC.Rows.Count > 0)
                    {
                        for (int intD = 0; intD < DFSC.Rows.Count; intD++)
                        {
                            //has.Clear();
                            Farpont1.Sheets[0].Rows.Count++;
                            Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString((intD + 1));
                            Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            ds.Tables[2].DefaultView.RowFilter = "College_code='" + Convert.ToString(DFSC.Rows[intD]["College_code"]) + "'";
                            DataView Dvcol = ds.Tables[2].DefaultView;
                            Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(Dvcol[0]["coll_acronymn"]);
                            Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            for (int intFc = 2; intFc < Farpont1.Sheets[0].ColumnCount - NumberCount; intFc++)
                            {
                                string Mon = Farpont1.Sheets[0].ColumnHeader.Cells[2, intFc].Tag.ToString().Split('$')[0];
                                string Ye = Farpont1.Sheets[0].ColumnHeader.Cells[2, intFc].Tag.ToString().Split('$')[1];
                                string Cat = Farpont1.Sheets[0].ColumnHeader.Cells[2, intFc].Note.ToString().Split('$')[0];
                                string Stf = Farpont1.Sheets[0].ColumnHeader.Cells[2, intFc].Note.ToString().Split('$')[1];
                                string PayMonthDate = string.Empty;
                                ds.Tables[1].DefaultView.RowFilter = "payMonthNum='" + Mon + "' and PayYear ='" + Ye + "' and College_code='" + Convert.ToString(DFSC.Rows[intD]["College_code"]) + "'";
                                DataView dvdate = ds.Tables[1].DefaultView;
                                if (dvdate.Count > 0)
                                {
                                    PayMonthDate = Convert.ToString(dvdate[0]["To_Date"]);
                                }
                                if (PayMonthDate.Trim() != "")
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " join_date <='" + PayMonthDate + "' and category_Name='" + Cat + "' and  stftype ='" + Stf + "' and college_code ='" + Convert.ToString(DFSC.Rows[intD]["College_code"]) + "'";
                                    DataView dvstrength = ds.Tables[0].DefaultView;
                                    DtMain.DefaultView.RowFilter = " relieve_date <='" + PayMonthDate + "' and category_Name='" + Cat + "' and  stftype ='" + Stf + "' and college_code ='" + Convert.ToString(DFSC.Rows[intD]["College_code"]) + "'";
                                    DataView dvRelived = DtMain.DefaultView;
                                    if (dvstrength.Count > 0)
                                    {
                                        DataTable DtST = dvstrength.ToTable();
                                        DataTable DtRt = dvRelived.ToTable();
                                        if (DtST.Rows.Count > 0)
                                        {
                                            int Total = 0;
                                            Total = Convert.ToInt32(DtST.Compute("sum (Total)", ""));
                                            int SubTotal = 0;
                                            if (DtRt.Rows.Count > 0)
                                            {
                                                SubTotal = Convert.ToInt32(DtRt.Compute("sum (Total)", ""));
                                            }
                                            Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, intFc].Text = Convert.ToString(Total - SubTotal);
                                            Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, intFc].HorizontalAlign = HorizontalAlign.Center;
                                            if (has.ContainsKey(Convert.ToString(Mon.Trim() + "$" + Ye.Trim())))
                                            {
                                                int Index = Convert.ToInt32(has[Convert.ToString(Mon.Trim() + "$" + Ye.Trim())]);
                                                int GrandTotal = 0;
                                                int.TryParse(Convert.ToString(Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, Index].Text), out GrandTotal);
                                                GrandTotal += Convert.ToInt32(Total - SubTotal);
                                                Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, Index].Text = Convert.ToString(GrandTotal);
                                                Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, Index].HorizontalAlign = HorizontalAlign.Center;
                                                if (!RowIndex.ContainsKey(Index))
                                                {
                                                    RowIndex.Add(Index, Convert.ToString(Total - SubTotal));
                                                }
                                                else
                                                {
                                                    int OverAllTotal = Convert.ToInt32(RowIndex[Index]);
                                                    RowIndex.Remove(Index);
                                                    RowIndex.Add(Index, ((Total - SubTotal) + OverAllTotal));
                                                }
                                            }
                                            if (!RowIndex.ContainsKey(intFc))
                                            {
                                                RowIndex.Add(intFc, Convert.ToString(Total - SubTotal));
                                            }
                                            else
                                            {
                                                int OverAllTotal = Convert.ToInt32(RowIndex[intFc]);
                                                OverAllTotal += Convert.ToInt32(Total - SubTotal);
                                                RowIndex.Remove(intFc);
                                                RowIndex.Add(intFc, OverAllTotal);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, intFc].Text = Convert.ToString("-");
                                        Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, intFc].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                                else
                                {
                                    Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, intFc].Text = Convert.ToString("-");
                                    Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, intFc].HorizontalAlign = HorizontalAlign.Center;
                                }

                            }
                        }
                        if (RowIndex.Count > 0)
                        {
                            Farpont1.Sheets[0].Rows.Count++;
                            Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Total");
                            Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            for (int intFc = 2; intFc < Farpont1.Sheets[0].ColumnCount; intFc++)
                            {
                                if (RowIndex.ContainsKey(intFc))
                                {
                                    Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, intFc].Text = Convert.ToString(RowIndex[intFc]);
                                    Farpont1.Sheets[0].Cells[Farpont1.Sheets[0].RowCount - 1, intFc].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        Farpont1.Visible = true;
                        print.Visible = true;
                    }
                    else
                    {
                        Farpont1.Visible = false;
                        print.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                    }
                }
                else
                {
                    Farpont1.Visible = false;
                    print.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                }
            }
            else
            {
                Farpont1.Visible = false;
                print.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select All Values!')", true);
            }
        }
        catch
        {

        }
    }

    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }

    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Farpont1, reportname);
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
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Finance Universal Report";
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "FinanceUniversalReport.aspx";
            Printcontrolhed.loadspreaddetails(Farpont1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    #endregion
}