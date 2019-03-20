using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;
public partial class HRMOD_JoinedRelievedStaffDetails : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
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
            usercode = Session["usercode"].ToString();
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
                bindyear();
                bindmonth();
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            lbl_alert.Visible = false;
            lblerror.Text = "";
        }
        catch (Exception ex) { }
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
        lbl_alert.Visible = false;
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
        string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + collegecode1 + "' ";
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
    protected void bindyear()
    {
        int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
        for (int l = 0; l < 15; l++)
        {
            ddl_year.Items.Add(Convert.ToString(year));
            year--;
        }
    }
    protected void bindmonth()
    {
        DateTime dt = new DateTime(2000, 1, 1);
        for (int m = 0; m < 12; m++)
        {
            ddl_month.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
        }
    }
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
    protected void cb_stat_CheckedChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_stat, cb_stat, txt_stat, "Status");
    }
    protected void cbl_stat_SelectedIndexChange(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_stat, cb_stat, txt_stat, "Status");
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where ( (Discontinue=0 or Discontinue is null)) and staff_name like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_name";//(resign =0 and settled =0) and
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where ( (Discontinue=0 or Discontinue is null)) and staff_code like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_code";//(resign =0 and settled =0) and
        name = ws.Getname(query);
        return name;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        string departmentcode = rs.GetSelectedItemsValueAsString(cbl_dept);
        string designationcode = rs.GetSelectedItemsValueAsString(cbl_desig);
        string category = rs.GetSelectedItemsValueAsString(cbl_staffc);
        string stafftype = rs.GetSelectedItemsValueAsString(cbl_stype);
        string JoiningDateF = ddl_month.SelectedItem.Value + "/01/" + ddl_year.SelectedItem.Text;
        int totaldays = DateTime.DaysInMonth(Convert.ToInt32(ddl_year.SelectedItem.Value), Convert.ToInt32(ddl_month.SelectedItem.Value));
        string JoiningDateT = ddl_month.SelectedItem.Value + "/" + totaldays + "/" + ddl_year.SelectedItem.Text;
        string prevMonth = string.Empty;
        string prevYear = string.Empty;
        if (ddl_month.SelectedIndex - 1 != -1)
        {
            prevMonth = Convert.ToString(ddl_month.Items[ddl_month.SelectedIndex - 1].Value);
            prevYear = Convert.ToString(ddl_year.SelectedValue);
        }
        else
        {
            prevMonth = Convert.ToString(ddl_month.Items[11].Value);
            prevYear = Convert.ToString(Convert.ToInt32(ddl_year.SelectedValue) - 1);
        }
        string staffnameQ = string.Empty;
        string staffcodeQ = string.Empty;
        if (txt_sname.Text.Trim() != "")
            staffnameQ = " and m.staff_name='" + txt_sname.Text.Trim() + "'";
        if (txt_scode.Text.Trim() != "")
            staffcodeQ = " and m.staff_code='" + txt_scode.Text.Trim() + "'";
        string Qry = " select m.resign,m.settled,CONVERT(varchar(10),join_date,103)join_date,p.NetAddAct,p.PayYear,p.PayMonth,t.staff_code,t.stfstatus,m.staff_name,h.dept_name, g.desig_name, t.stftype,category_name,t.bsalary,t.grade_pay,h.dept_code,c.category_code from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c,monthlypay p where p.staff_code=t.staff_code and p.staff_code=m.staff_code and t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1  and ISNULL(Discontinue,'0')='0' and ISNULL(PayYear,-1)<>-1 and ISNULL(PayMonth,-1)<>-1  and m.college_code = '" + collegecode1 + "' and h.dept_code in('" + departmentcode + "') and g.desig_code in('" + designationcode + "') and c.category_code in('" + category + "') and t.stftype in('" + stafftype + "') and ((p.PayMonth >= '" + prevMonth + "' and p.PayYear = '" + prevYear + "') or (p.PayMonth <='" + ddl_month.SelectedItem.Value + "' and p.PayYear = '" + ddl_year.SelectedItem.Text + "' ))  " + staffnameQ + "" + staffcodeQ + "  and m.join_date between '" + JoiningDateF + "' and '" + JoiningDateT + "' order by c.category_code";//  and t.stfstatus in('" + status + "')
        ds = d2.select_method_wo_parameter(Qry, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string CurrentMonthYear = Convert.ToString(ddl_month.SelectedItem.Text + "  " + ddl_year.SelectedItem.Text);
            string previousMonthyear = string.Empty;
            if (ddl_month.SelectedIndex - 1 != -1)
                previousMonthyear = Convert.ToString(ddl_month.Items[ddl_month.SelectedIndex - 1].Text) + " " + ddl_year.SelectedItem.Text;
            else
                previousMonthyear = Convert.ToString(ddl_month.Items[11].Text) + " " + Convert.ToString(Convert.ToInt32(ddl_year.SelectedValue) - 1);
            string headername = "S.No-50/Staff Name-180/Designation-200/Department-200/" + previousMonthyear + "-120/" + CurrentMonthYear + "-120/Date of Joining-100";
            rs.Fpreadheaderbindmethod(headername, FpSpread, "False");
            FpSpread.Sheets[0].Columns[0].Locked = true;
            FpSpread.Sheets[0].Columns[1].Locked = true;
            FpSpread.Sheets[0].Columns[2].Locked = true;
            FpSpread.Sheets[0].Columns[3].Locked = true;
            FpSpread.Sheets[0].Columns[4].Locked = true;
            FpSpread.Sheets[0].Columns[5].Locked = true;
            Hashtable NewjoinCatagoryNameAmtHash = new Hashtable();
            #region Newjoin
            DataView newjoinDv = new DataView();
            ds.Tables[0].DefaultView.RowFilter = " settled =0 and resign =0 and PayYear='" + ddl_year.SelectedItem.Text + "' and PayMonth='" + ddl_month.SelectedItem.Value + "'";
            newjoinDv = ds.Tables[0].DefaultView;
            double newStaffGrandCurrentMonthTotal = 0;
            double newStaffGrandPrevMonthTotal = 0;
            bool NewjoinRowsBind = Bindvalues(newjoinDv, prevMonth, prevYear, ref newStaffGrandCurrentMonthTotal, ref newStaffGrandPrevMonthTotal, ref NewjoinCatagoryNameAmtHash);
            #endregion
            #region Relieved Staff
            FpSpread.Sheets[0].RowCount++;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Relieved Staff Details" + ddl_month.SelectedItem.Text + " & " + ddl_month.SelectedItem.Text;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount - 1, 1, 1, 5);
            DataView RelievedDv = new DataView();
            ds.Tables[0].DefaultView.RowFilter = " settled =1 and resign =1 ";
            RelievedDv = ds.Tables[0].DefaultView;
            double RelivedStaffGrandCurrentMonthTotal = 0;
            double RelivedStaffGrandPrevMonthTotal = 0;
            Hashtable RelivedCatagoryNameAmt = new Hashtable();
            bool RelievedRowsBind = Bindvalues(RelievedDv, prevMonth, prevYear, ref RelivedStaffGrandCurrentMonthTotal, ref RelivedStaffGrandPrevMonthTotal, ref RelivedCatagoryNameAmt, 1);
            #endregion
            if (RelievedRowsBind || NewjoinRowsBind)
            {
                #region Final Calculation
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = "Newly Joined";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = "Relived";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = "Difference";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Net Difference";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                double DifferenceTotalAmt = (newStaffGrandCurrentMonthTotal - newStaffGrandPrevMonthTotal) - (RelivedStaffGrandPrevMonthTotal - RelivedStaffGrandCurrentMonthTotal);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString((newStaffGrandCurrentMonthTotal - newStaffGrandPrevMonthTotal));
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString((RelivedStaffGrandPrevMonthTotal - RelivedStaffGrandCurrentMonthTotal));
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(DifferenceTotalAmt);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Newly Joined";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkGreen;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                #region Newly Joined
                foreach (DictionaryEntry newjoin in NewjoinCatagoryNameAmtHash)
                {
                    string[] catagoryName = Convert.ToString(newjoin.Key).Split('-');
                    string catagoryValue = Convert.ToString(newjoin.Value);
                    if (Convert.ToString(catagoryName[1]) != "D")
                    {
                        FpSpread.Sheets[0].RowCount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(catagoryName[2]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.Brown;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = catagoryValue;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].ForeColor = Color.Brown;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    }
                }
                string NewjoingTotal = Convert.ToString(NewjoinCatagoryNameAmtHash["0-D"]);
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = NewjoingTotal;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                #endregion
                #region Relived staff
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Relieved Staff";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkGreen;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                foreach (DictionaryEntry newjoin in RelivedCatagoryNameAmt)
                {
                    string[] catagoryName = Convert.ToString(newjoin.Key).Split('-');
                    string catagoryValue = Convert.ToString(newjoin.Value);
                    if (Convert.ToString(catagoryName[1]) != "D")
                    {
                        FpSpread.Sheets[0].RowCount++;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(catagoryName[2]);
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.Brown;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = catagoryValue;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].ForeColor = Color.Brown;
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    }
                }
                string relivedTotal = Convert.ToString(RelivedCatagoryNameAmt["1-D"]);
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = relivedTotal;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                #endregion
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = "Difference";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(DifferenceTotalAmt);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Brown;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                #endregion
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                FpSpread.Height = FpSpread.Sheets[0].RowCount * 25 + 200;
                sp_div.Visible = true;
                rprint.Visible = true;
                txt_sname.Text = "";
                txt_scode.Text = "";
            }
            else
            {
                rprint.Visible = false;
                sp_div.Visible = false;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No records founds";
            }
        }
        else
        {
            rprint.Visible = false;
            sp_div.Visible = false;
            lbl_alert.Visible = true;
            lbl_alert.Text = "No records founds";
        }
    }
    protected bool Bindvalues(DataView dataviewName, string PreviousMonth, string Previousyear, ref double CurrentgrandTotal, ref double PrevGrandTotal, ref Hashtable CatagoryNameAmt, byte Type = 0)
    {
        CatagoryNameAmt.Clear();
        double total = 0;
        double prevTotal = 0;
        bool RowIsCountChk = false;
        //double difference = 0;
        string prevCategory = string.Empty;
        int rowno = 0; double val = 0;
        foreach (DataRowView dr in dataviewName)
        {
            RowIsCountChk = true;
            string Categoryname = Convert.ToString(dr["category_name"]);
            if (Categoryname != prevCategory)
            {
                if (!string.IsNullOrEmpty(prevCategory))
                {
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = "Total";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Maroon;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(prevTotal);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].ForeColor = Color.Maroon;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].ForeColor = Color.Maroon;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = "Difference";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Maroon;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                    if (Type == 0)
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total - prevTotal);
                    else
                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(prevTotal - total);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].ForeColor = Color.Maroon;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                    CurrentgrandTotal += total;
                    PrevGrandTotal += prevTotal;
                    total = 0;
                    prevTotal = 0;
                }
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Categoryname;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].ForeColor = Color.Brown;
                FpSpread.Sheets[0].SpanModel.Add(FpSpread.Sheets[0].RowCount - 1, 1, 1, 2);
            }
            prevCategory = Convert.ToString(dr["category_name"]);
            FpSpread.Sheets[0].RowCount++;
            rowno++;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowno);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["staff_name"]);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dr["staff_code"]);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["desig_name"]);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["dept_name"]);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
            double prevMonSalary = 0;
            if (ddl_month.SelectedItem.Value != PreviousMonth && ddl_year.SelectedItem.Value != Previousyear)
            {
                DataTable resignStaffDT = dataviewName.ToTable();
                resignStaffDT.DefaultView.RowFilter = " dept_code='" + Convert.ToString(dr["dept_code"]) + "' and category_code='" + Convert.ToString(dr["category_code"]) + "'  and staff_code='" + Convert.ToString(dr["staff_code"]) + "' and ((PayMonth >= '" + PreviousMonth + "' and PayYear = '" + Previousyear + "') or (PayMonth <='" + ddl_month.SelectedItem.Value + "' and PayYear = '" + ddl_year.SelectedItem.Value + "' ))";
                DataView prevMonSalaryDv = resignStaffDT.DefaultView;
                if (prevMonSalaryDv.Count > 0)
                {
                    double.TryParse(Convert.ToString(prevMonSalaryDv[0]["NetAddAct"]), out prevMonSalary);
                }
            }
            prevTotal += prevMonSalary;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(prevMonSalary);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dr["NetAddAct"]);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dr["join_date"]);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
            double salaryAmt = 0;
            double.TryParse(Convert.ToString(dr["NetAddAct"]), out salaryAmt);
            total += salaryAmt;
            val = 0;
            if (!CatagoryNameAmt.Contains(Type + "-C-" + Categoryname))
                CatagoryNameAmt.Add(Type + "-C-" + Categoryname, salaryAmt);
            else
            {
                val = 0;
                double.TryParse(Convert.ToString(CatagoryNameAmt[Type + "-C-" + Categoryname]), out val);
                CatagoryNameAmt.Remove(Type + "-C-" + Categoryname);
                CatagoryNameAmt.Add(Type + "-C-" + Categoryname, val + salaryAmt);
            }
            if (!CatagoryNameAmt.Contains(Type + "-P-" + Categoryname))
                CatagoryNameAmt.Add(Type + "-P-" + Categoryname, prevTotal);
            else
            {
                val = 0;
                double.TryParse(Convert.ToString(CatagoryNameAmt[Type + "-P-" + Categoryname]), out val);
                CatagoryNameAmt.Remove(Type + "-P-" + Categoryname);
                CatagoryNameAmt.Add(Type + "-P-" + Categoryname, val + prevMonSalary);
            }
        }
        if (RowIsCountChk)
        {
            FpSpread.Sheets[0].RowCount++;
            CurrentgrandTotal += total;
            PrevGrandTotal += prevTotal;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = "Total";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(prevTotal);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
            FpSpread.Sheets[0].RowCount++;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = "Difference";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
            if (Type == 0)
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total - prevTotal);
            else
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(prevTotal - total);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
            FpSpread.Sheets[0].RowCount++;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = "Grand Total";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(PrevGrandTotal);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(CurrentgrandTotal);
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
            FpSpread.Sheets[0].RowCount++;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = "Difference";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
            double grandDifference = 0;
            if (Type == 0)
            {
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(CurrentgrandTotal - PrevGrandTotal);
                grandDifference = CurrentgrandTotal - PrevGrandTotal;
            }
            else
            {
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(PrevGrandTotal - CurrentgrandTotal);
                grandDifference = PrevGrandTotal - CurrentgrandTotal;
            }
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].ForeColor = Color.Maroon;
            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
            if (!CatagoryNameAmt.Contains(Type + "-D"))
                CatagoryNameAmt.Add(Type + "-D", grandDifference);
            total = 0;
            prevTotal = 0;
        }
        return RowIsCountChk;
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
            string degreedetails = "Newly Joined & Relieved Staff Details";
            string pagename = "JoinedRelievedStaffDetails.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }
}