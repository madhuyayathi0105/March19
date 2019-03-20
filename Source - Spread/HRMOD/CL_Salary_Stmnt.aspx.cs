using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

public partial class CL_Salary_Stmnt : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dsYr = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataSet ds11 = new DataSet();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    static string clgcode = string.Empty;
    static string user_code = string.Empty;
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
            user_code = Session["usercode"].ToString();
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
                bindLeaveType();
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
        //  string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '" + prefixText + "%' and college_code='" + clgcode + "'"; delsi 0207

        string query = "select distinct s.staff_name from staffmaster s,hr_privilege hp,stafftrans st where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and s.staff_code like '" + prefixText + "%' and s.college_code='" + clgcode + "' and s.college_code=hp.college_code and s.staff_code=st.staff_code and user_code ='" + user_code + "' and hp.dept_code=st.dept_code";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
      //  string query = "select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '" + prefixText + "%' and college_code='" + clgcode + "'"; delsi 0207

        string query = "select distinct s.staff_code from staffmaster s,hr_privilege hp,stafftrans st where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and s.staff_code like '" + prefixText + "%' and s.college_code='" + clgcode + "' and s.college_code=hp.college_code and s.staff_code=st.staff_code and user_code ='" + user_code + "' and hp.dept_code=st.dept_code";
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
        bindLeaveType();
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
        //GetPrimeCountAsync();
        //Task myTask = Task.Factory.StartNew(new Action(LoadSpread));
        //myTask.Wait();
        LoadSpread();
    }

    //public void GetPrimeCountAsync()
    //{
    //    ManualResetEvent evt = new ManualResetEvent(false);
    //    WaitCallback wait = new WaitCallback((x) =>
    //    {
    //        try
    //        {
    //            LoadSpread();
    //        }
    //        catch (Exception ex)
    //        {
    //            //ToDo handle the exception
    //        }
    //        finally
    //        {
    //            evt.Set();
    //        }
    //    });
    //    ThreadPool.QueueUserWorkItem(wait);
    //    evt.WaitOne();
    //    evt.Close();
    //}

    private void LoadSpread()
    {
        try
        {
            FpSpread.Visible = false;
            lblMainErr.Visible = false;
            rprint.Visible = false;

            DataView dvnew = new DataView();
            DataView dvnew1 = new DataView();
            double CLCount = 0;
            double TotCLCount = 0;
            double TotAmnt = 0;
            double GrandTotal = 0;
            double TotGrandTotal = 0;
            double GetAllotCL = 0;
            string leavetype = string.Empty;
            string ClgCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string DeptCode = GetSelectedItemsValueAsString(cbl_dept);
            string DesigCode = GetSelectedItemsValueAsString(cbl_desig);
            string CatCode = GetSelectedItemsValueAsString(cbl_staffc);
            string StfType = GetSelectedItemsText(cbl_stype);
            string FrmMon = Convert.ToString(ddlMon.SelectedItem.Value);
            string FrmYear = Convert.ToString(ddlYear.SelectedItem.Text);
            string ToMon = Convert.ToString(ddlToMon.SelectedItem.Value);
            string ToYear = Convert.ToString(ddlToYear.SelectedItem.Text);
            string SelQ = string.Empty;

            if (ddlLeaveType.SelectedIndex == 0)
            {
                lblMainErr.Visible = true;
                lblMainErr.Text = "Please Select Leave Type!";
                return;
            }
            if (ddlMon.SelectedIndex != 0 && ddlYear.Items.Count > 0 && ddlToMon.SelectedIndex != 0 && ddlToYear.Items.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(txt_scode.Text.Trim())))
                {
                    SelQ = "select sm.staff_name,sc.category_name,st.stftype,sm.staff_Code,sc.category_code from staffmaster sm,stafftrans st,hrdept_master h,desig_master de,staffcategorizer sc where sm.staff_code=st.staff_code and sm.college_code=h.college_code and sm.college_code=de.collegeCode and sm.college_code=sc.college_code and st.dept_code=h.dept_code and st.desig_code=de.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code='" + ClgCode + "' and sm.staff_code='" + Convert.ToString(txt_scode.Text) + "'";
                    SelQ = SelQ + " select staff_code,leavetype,category_code from individual_Leave_type where college_code='" + ClgCode + "' and staff_code='" + Convert.ToString(txt_scode.Text.Trim()) + "' and (leavetype like '" + ddlLeaveType.SelectedItem.Text + "%' or leavetype like '%" + ddlLeaveType.SelectedItem.Text + "%' or leavetype like '%" + ddlLeaveType.SelectedItem.Text + "')";
                    SelQ = SelQ + " select PerDay_Salary,Staff_Code,PayMonth,PayYear from monthlypay where college_code='" + ClgCode + "'";
                }
                else if (!String.IsNullOrEmpty(Convert.ToString(txt_sname.Text.Trim())))
                {
                    SelQ = "select sm.staff_name,sc.category_name,st.stftype,sm.staff_Code,sc.category_code from staffmaster sm,stafftrans st,hrdept_master h,desig_master de,staffcategorizer sc where sm.staff_code=st.staff_code and sm.college_code=h.college_code and sm.college_code=de.collegeCode and sm.college_code=sc.college_code and st.dept_code=h.dept_code and st.desig_code=de.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and sm.college_code='" + ClgCode + "' and sm.staff_name='" + Convert.ToString(txt_sname.Text) + "'";
                    SelQ = SelQ + " select staff_code,leavetype,category_code from individual_Leave_type where college_code='" + ClgCode + "' and (leavetype like '" + ddlLeaveType.SelectedItem.Text + "%' or leavetype like '%" + ddlLeaveType.SelectedItem.Text + "%' or leavetype like '%" + ddlLeaveType.SelectedItem.Text + "')";
                    SelQ = SelQ + " select PerDay_Salary,Staff_Code,PayMonth,PayYear from monthlypay where college_code='" + ClgCode + "'";
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
                    SelQ = "select sm.staff_name,sc.category_name,st.stftype,sm.staff_Code,sc.category_code from staffmaster sm,stafftrans st,hrdept_master h,desig_master de,staffcategorizer sc where sm.staff_code=st.staff_code and sm.college_code=h.college_code and sm.college_code=de.collegeCode and sm.college_code=sc.college_code and st.dept_code=h.dept_code and st.desig_code=de.desig_code and st.category_code=sc.category_code and st.latestrec='1' and sm.resign='0' and sm.settled='0' and ISNULL(Discontinue,'0')='0' and h.dept_Code in('" + DeptCode + "') and de.desig_Code in('" + DesigCode + "') and st.category_Code in('" + CatCode + "') and st.StfType in('" + StfType + "') and sm.college_code='" + ClgCode + "'";
                    SelQ = SelQ + " select staff_code,leavetype,category_code from individual_Leave_type where college_code='" + ClgCode + "' and (leavetype like '" + ddlLeaveType.SelectedItem.Text + "%' or leavetype like '%" + ddlLeaveType.SelectedItem.Text + "%' or leavetype like '%" + ddlLeaveType.SelectedItem.Text + "')";
                    SelQ = SelQ + " select PerDay_Salary,Staff_Code,PayMonth,PayYear from monthlypay where college_code='" + ClgCode + "'";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    LoadHeader();
                    TotGrandTotal = 0;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        FpSpread.Sheets[0].RowCount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Staff_Name"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Staff_Code"]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                        TotCLCount = 0;
                        TotAmnt = 0;
                        GrandTotal = 0;
                        GetAllotCL = 0;
                        leavetype = "";
                        ds.Tables[1].DefaultView.RowFilter = " staff_code='" + Convert.ToString(ds.Tables[0].Rows[row]["Staff_Code"]) + "' and category_code='" + Convert.ToString(ds.Tables[0].Rows[row]["category_code"]) + "'";
                        dvnew = ds.Tables[1].DefaultView;
                        if (dvnew.Count > 0)
                            leavetype = Convert.ToString(dvnew[0]["leavetype"]);
                        if (!String.IsNullOrEmpty(leavetype) && leavetype.Trim() != "0")
                            GetAllotCLCount(out GetAllotCL, leavetype);
                        for (int sprcol = 3; sprcol < FpSpread.Sheets[0].ColumnCount - 2; sprcol++)
                        {
                            string GetMonYear = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, sprcol].Tag);
                            if (!String.IsNullOrEmpty(GetMonYear) && GetMonYear.Contains('/'))
                            {
                                CLCount = 0;
                                GetCLCount(out CLCount, Convert.ToString(ds.Tables[0].Rows[row]["Staff_Code"]), GetMonYear, ClgCode);
                                TotCLCount = TotCLCount + CLCount;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Text = Convert.ToString(CLCount);
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Font.Size = FontUnit.Medium;
                            }
                            else
                            {
                                if (GetAllotCL == 0)
                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Text = "0";
                                else if (GetAllotCL > 0 && (CLCount == 0 || CLCount == 0.5))
                                {
                                    GetMonYear = Convert.ToString(FpSpread.Sheets[0].ColumnHeader.Cells[0, sprcol - 1].Tag);
                                    if (!String.IsNullOrEmpty(GetMonYear) && GetMonYear.Contains('/'))
                                    {
                                        string[] splMonYr = GetMonYear.Split('/');
                                        ds.Tables[2].DefaultView.RowFilter = " staff_code='" + Convert.ToString(ds.Tables[0].Rows[row]["Staff_Code"]) + "' and PayMonth='" + splMonYr[0] + "' and PayYear='" + splMonYr[1] + "'";
                                        dvnew1 = ds.Tables[2].DefaultView;
                                        if (dvnew1.Count > 0)
                                        {
                                            double.TryParse(Convert.ToString(dvnew1[0]["PerDay_Salary"]), out TotAmnt);
                                            TotAmnt = Math.Round(TotAmnt, 0, MidpointRounding.AwayFromZero);
                                            if (CLCount == 0)
                                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Text = Convert.ToString(TotAmnt);
                                            else if (CLCount == 0.5)
                                            {
                                                TotAmnt = TotAmnt / 2;
                                                TotAmnt = Math.Round(TotAmnt, 0, MidpointRounding.AwayFromZero);
                                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Text = Convert.ToString(TotAmnt);
                                            }
                                            else
                                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Text = "0";
                                            GrandTotal = GrandTotal + TotAmnt;
                                        }
                                        else
                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Text = "0";
                                    }
                                }
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, sprcol].Font.Size = FontUnit.Medium;
                            }
                        }
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Text = Convert.ToString(TotCLCount);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;

                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(GrandTotal);
                        TotGrandTotal = TotGrandTotal + GrandTotal;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    }
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Rows[FpSpread.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                    FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount - 1, 0, 1, FpSpread.Sheets[0].ColumnCount - 1);

                    TotGrandTotal = Math.Round(TotGrandTotal, 0, MidpointRounding.AwayFromZero);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(TotGrandTotal);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
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
            FpSpread.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread.Sheets[0].RowCount = 0;
            FpSpread.Sheets[0].ColumnHeader.Columns.Count = 3;
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
            FpSpread.Columns[1].Width = 200;
            FpSpread.Columns[1].Locked = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
            FpSpread.Columns[2].Width = 100;
            FpSpread.Columns[2].Locked = true;

            FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

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
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dsHeader.Tables[0].Rows[icol]["PayMonthNum"]) + "/" + Convert.ToString(dsHeader.Tables[0].Rows[icol]["SelYear"]);

                    FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ddlLeaveType.SelectedItem.Value);
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 75;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;

                    FpSpread.Sheets[0].ColumnCount++;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Amt";
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                    FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 2, 1);
                }
            }
            FpSpread.Sheets[0].ColumnCount++;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Total";
            FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ddlLeaveType.SelectedItem.Value);
            FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 75;
            FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;

            FpSpread.Sheets[0].ColumnCount++;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Total Amount";
            FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
            FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
            FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 2, 1);
        }
        catch (Exception ex) { lblMainErr.Visible = true; lblMainErr.Text = ex.ToString(); }
    }

    private void GetAllotCLCount(out double GetAllotCL, string LeaveType)
    {
        GetAllotCL = 0;
        try
        {
            string[] splGetLev = LeaveType.Split('\\');
            if (splGetLev.Length > 0)
            {
                for (int frst = 0; frst < splGetLev.Length; frst++)
                {
                    if (splGetLev[frst].Contains(Convert.ToString(ddlLeaveType.SelectedItem.Text)))
                    {
                        string[] splLevVal = splGetLev[frst].Split(';');
                        if (splLevVal.Length > 1)
                        {
                            double.TryParse(Convert.ToString(splLevVal[2]), out GetAllotCL);
                            break;
                        }
                    }
                }
            }
        }
        catch { }
    }

    private void GetCLCount(out double CLCount, string Staff_Code, string mon_year, string ClgCode)
    {
        CLCount = 0;
        DateTime dtFrm = new DateTime();
        DateTime dtTo = new DateTime();
        DataSet dsGetDate = new DataSet();
        string[] splMonYr = new string[2];
        string[] splAtt = new string[2];
        try
        {
            splMonYr = mon_year.Split('/');
            string GetFrmToDt = "select Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date from HrPayMonths where PayMonthNum='" + splMonYr[0] + "' and PayYear='" + splMonYr[1] + "' and College_Code='" + ClgCode + "' and SelStatus='1'";
            GetFrmToDt = GetFrmToDt + " select * from Staff_attnd where Staff_Code='" + Staff_Code + "' and mon_year='" + mon_year + "'";
            dsGetDate.Clear();
            dsGetDate = d2.select_method_wo_parameter(GetFrmToDt, "Text");
            if (dsGetDate.Tables.Count > 0 && dsGetDate.Tables[0].Rows.Count > 0 && dsGetDate.Tables[1].Rows.Count > 0)
            {
                dtFrm = Convert.ToDateTime(dsGetDate.Tables[0].Rows[0]["From_Date"]);
                dtTo = Convert.ToDateTime(dsGetDate.Tables[0].Rows[0]["To_Date"]);
                while (dtFrm <= dtTo)
                {
                    string GetAtt = d2.GetFunction("select [" + dtFrm.Day + "] from staff_attnd where mon_year='" + dtFrm.Month + "/" + dtFrm.Year + "' and staff_code='" + Staff_Code + "' and ([" + dtFrm.Day + "] like '%" + Convert.ToString(ddlLeaveType.SelectedItem.Value) + "' or [" + dtFrm.Day + "] like '" + Convert.ToString(ddlLeaveType.SelectedItem.Value) + "%')");
                    if (!String.IsNullOrEmpty(GetAtt) && GetAtt.Trim() != "0" && GetAtt.Contains('-'))
                    {
                        splAtt = GetAtt.Split('-');
                        if (splAtt.Length > 1)
                        {
                            if (splAtt[0] == Convert.ToString(ddlLeaveType.SelectedItem.Value))
                                CLCount += 1;
                            if (splAtt[1] == Convert.ToString(ddlLeaveType.SelectedItem.Value))
                                CLCount += 1;
                        }
                    }
                    dtFrm = dtFrm.AddDays(1);
                }
            }
        }
        catch { }
        CLCount = CLCount / 2;
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
            ds.Clear();
            string group_user = "";
            string cmd = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + clgcode + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + clgcode + "') order by dept_name";
            }

            ds = d2.select_method_wo_parameter(cmd, "Text");
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

    private void bindLeaveType()
    {
        try
        {
            ddlLeaveType.Items.Clear();
            string CollCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string SelQ = "select category,shortname from leave_category where college_code='" + CollCode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlLeaveType.DataSource = ds;
                ddlLeaveType.DataTextField = "category";
                ddlLeaveType.DataValueField = "shortname";
                ddlLeaveType.DataBind();
                ddlLeaveType.Items.Insert(0, "Select");
            }
            else
            {
                ddlLeaveType.Items.Insert(0, "Select");
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