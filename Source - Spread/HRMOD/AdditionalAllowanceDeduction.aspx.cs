using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Text;

public partial class HRMOD_AdditionalAllowanceDeduction : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    int i = 0;
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    DAccess2 d2 = new DAccess2();
    Boolean cellclick = false;
    string q1 = "";
    string activerow = "";
    string activecol = "";
    FarPoint.Web.Spread.CheckBoxCellType CheckAll = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType CheckInd = new FarPoint.Web.Spread.CheckBoxCellType();
   
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
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            //txt_amount.Enabled = false;
            lblvalidation1.Text = "";
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                binddept();
                designation();
                category();
                radFormat_SelectedIndexChanged(sender, e);
                bind_ddl_popAllowDeduc();
                loaddeduction(); // poo 30.11.17
                txtchqdt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txtchqdt.Attributes.Add("readonly", "readonly");
            }
        }
        catch (Exception ex) { }
    }
    
    protected void ddlcollege_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            binddept();
            designation();
            category();

        }
        catch (Exception ex)
        {

        }
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
        catch (Exception ex)
        {

        }
    }
    
    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string item = "select distinct Dept_Code,Dept_Name from Department where college_code = '" + clgcode + "' order by Dept_Name";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "Dept_Name";
                cbl_dept.DataValueField = "Dept_Code";
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
        catch (Exception ex)
        {

        }
    }

    protected void designation()
    {
        try
        {
            ds.Clear();
            cbl_desig.Items.Clear();
            string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + clgcode + "' order by desig_name";
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
        catch (Exception ex)
        {

        }
    }

    protected void category()
    {
        try
        {
            ds.Clear();
            cbl_staffc.Items.Clear();
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + clgcode + "' order by category_Name";
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
        catch (Exception ex)
        {

        }
    }

    protected void loaddeduction()
    {
        try
        {
            ds.Clear();
            cbl_ded.Items.Clear(); cb_ded.Checked = false;
            string item = "select deductions from incentives_master where college_code='" + collegecode1 + "'";
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
                        cbl_ded.Items.Add(stafftype);
                    }
                }
                //if (cbl_ded.Items.Count > 0)
                //{
                //    for (int i = 0; i < cbl_ded.Items.Count; i++)
                //    {
                //        cbl_ded.Items[i].Selected = true;
                //    }
                //    txt_ded.Text = "Deduction (" + cbl_ded.Items.Count + ")";
                //    cb_ded.Checked = true;
                //}
            }
            else
            {
                txt_ded.Text = "--Select--";
                cb_ded.Checked = false;
            }

        }
        catch { }
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

    protected void cb_ded_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_ded, cbl_ded, txt_ded, "Deduction");
    }
    protected void cbl_ded_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_ded, cbl_ded, txt_ded, "Deduction");
    }
    protected void radFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (radFormat.SelectedIndex == 0)
        {
            lbl_AddAllowDeduc.Text = "Allowance";
            lbl_allowance.Text = "Allowance";
            lbl_add.Text = "Allowance";
            btn_allowsave.Visible = true; // poo 30.11.17
            //loaddeduction(); // poo 30.11.17

        }
        else if (radFormat.SelectedIndex == 1)
        {
            lbl_AddAllowDeduc.Text = "Deduction";
            lbl_allowance.Text = "Deduction";
            lbl_add.Text = "Deduction";
            btn_allowsave.Visible = false; // poo 30.11.17

        }
        bind_ddl_popAllowDeduc();
    }

    protected void cb_deduct_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_deduct.Checked == true)
            {
                txt_ded.Enabled = true;
                loaddeduction();
                PopAllowDeducSelectedIndexChanged(sender, e);
            }
            if (cb_deduct.Checked == false)
            {
                txt_ded.Enabled = false;
                cbl_ded.Items.Clear();
                txt_ded.Text = "--Select--";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "AdditonalAllowanceDedution"); }
    }

    protected void AllowDeducSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_Allowdeduc.SelectedItem.Text != "Select")
            txt_amount.Enabled = true;
        else
            txt_amount.Enabled = false;
        btn_go_Click(sender, e);
    }
    protected void PopAllowDeducSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbl_ded.ClearSelection(); cb_ded.Checked = false;
            string collegecode1 = ddl_college1.SelectedItem.Value.ToString();
            //string textAllowDeducName = txt_AllowDeducname.Text.ToUpper();
            string allowancename = ddl_popAllowDeduc.SelectedItem.Text;
            string sql = "";
            if (allowancename != "" && allowancename != "Select")
            {
                //string deduname = d2.GetFunction("select isnull(MasterCriteriaValue1,'')MasterCriteriaValue1 from co_mastervalues where mastercriteria in('AdditionalAllowance') and mastervalue='" + Convert.ToString(allowancename) + "' and collegecode='" + collegecode1 + "'");

                DataSet dsAdd = new DataSet();
                dsAdd = d2.select_method_wo_parameter("select isnull(MasterCriteriaValue1,'')MasterCriteriaValue1, mastercriteriavalue2,mastercriteriavalue3,mastercriteriavalue4 from co_mastervalues where mastercriteria in('AdditionalAllowance') and mastervalue='" + Convert.ToString(allowancename) + "' and collegecode='" + collegecode1 + "'", "TEXT");
                if (dsAdd.Tables[0].Rows.Count > 0)
                {
                    string deduname = Convert.ToString(dsAdd.Tables[0].Rows[0]["MasterCriteriaValue1"]);
                    string chequeddno = Convert.ToString(dsAdd.Tables[0].Rows[0]["mastercriteriavalue2"]);
                    string chequedddate = Convert.ToString(dsAdd.Tables[0].Rows[0]["mastercriteriavalue3"]);
                    string challonnotransferno = Convert.ToString(dsAdd.Tables[0].Rows[0]["mastercriteriavalue4"]);

                    if (!string.IsNullOrEmpty(deduname))
                    {
                        string[] dedname = deduname.Split(',');
                        if (dedname.Length > 0)
                        {
                            if (cbl_ded.Items.Count > 0)
                            {
                                for (int y = 0; y < dedname.Length; y++)
                                {
                                    string deductionsel = dedname[y];
                                    cbl_ded.Items.FindByText(deductionsel).Selected = true;
                                    txt_ded.Text = "Deduction (" + dedname.Length + ")";
                                    txtchqno.Text = chequeddno;
                                    txtchqdt.Text = chequedddate;
                                    txtchallonNoTransferVoucher.Text = challonnotransferno;
                                    //cb_ded.Checked = true;
                                }
                            }
                        }
                        else
                        {
                            txt_ded.Text = "--Select--";
                            cb_ded.Checked = false;
                            txtchqno.Text = "";
                            txtchqdt.Text = "";
                            txtchallonNoTransferVoucher.Text = "";
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "AdditonalAllowanceDedution"); }
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

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        btn_popdelete.Visible = false;
        popaddnew.Visible = true;
        bind_ddlCollege();
        PopAllowDeducSelectedIndexChanged(sender, e);

    }
    private void bind_ddlCollege() //to bind college in popup(addnew button) dropdown
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollegebaseonrights(usercode, 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college1.DataSource = ds;
                ddl_college1.DataTextField = "collname";
                ddl_college1.DataValueField = "college_code";
                ddl_college1.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void bind_ddl_popAllowDeduc()
    {
        try
        {
            ddl_Allowdeduc.Items.Clear();
            ddl_popAllowDeduc.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode1.Trim()))
            {
                string sql = "";
                if (radFormat.SelectedIndex == 0)
                {
                    sql = " select mastercode,MasterValue from co_mastervalues where mastercriteria='AdditionalAllowance' and collegecode='" + collegecode1 + "'";
                }
                else if (radFormat.SelectedIndex == 1)
                {
                    sql = " select mastercode,MasterValue from co_mastervalues where mastercriteria='AdditionalDeduction' and collegecode='" + collegecode1 + "'";
                }
                ds = d2.select_method_wo_parameter(sql, "TEXT");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_Allowdeduc.DataSource = ds;
                    ddl_popAllowDeduc.DataSource = ds;
                    ddl_Allowdeduc.DataTextField = "MasterValue";
                    ddl_popAllowDeduc.DataTextField = "MasterValue";
                    ddl_Allowdeduc.DataValueField = "mastercode";
                    ddl_popAllowDeduc.DataValueField = "mastercode";
                    ddl_Allowdeduc.DataBind();
                    ddl_popAllowDeduc.DataBind();

                    //ddl_Allowdeduc.Items.Insert(0, new ListItem("Select", "0"));
                    ddl_popAllowDeduc.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    //ddl_Allowdeduc.Items.Insert(0, new ListItem("Select", "0"));
                    ddl_popAllowDeduc.Items.Insert(0, new ListItem("Select", "0"));
                }

            }

        }
        catch { }

    }

    public void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlstaff.SelectedItem.Value == "0")
        {
            txt_search.Visible = true;
            txt_search1.Visible = false;
            txt_search1.Text = "";
        }
        else
        {
            txt_search.Visible = false;
            txt_search1.Visible = true;
            txt_search.Text = "";
        }
        //loadfsstaff();
    }

    public void txt_search_TextChanged(object sender, EventArgs e)
    {
        //loadfsstaff();
    }
    public void txt_search1_TextChanged(object sender, EventArgs e)
    {
        //loadfsstaff();
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffcode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = " select distinct sm.staff_code  from staffmaster sm,staff_appl_master sa,stafftrans st where sm.appl_no=sa.appl_no and st.latestrec='1' and st.staff_code=sm.staff_code and sm.resign='0' and sm.settled='0' and sm.staff_code like '%" + prefixText + "%' order by sm.staff_code asc";
        //  string query = "select distinct s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and s.college_code='" + mulicollg + "' and resign =0 and s.staff_code like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffname1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = " select distinct sm.staff_name from staffmaster sm,staff_appl_master sa,stafftrans st where sm.appl_no=sa.appl_no and st.latestrec='1' and st.staff_code=sm.staff_code and sm.resign='0' and sm.settled='0' and sm.staff_name like '%" + prefixText + "%' order by sm.staff_name asc";
        // string query = "select distinct (s.staff_name+'-'+s.staff_code)as staff from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            int sno = 0;
            string query = "";
            string deptcode = rs.GetSelectedItemsValueAsString(cbl_dept);
            string designation = rs.GetSelectedItemsValueAsString(cbl_desig);
            string staffcategory = rs.GetSelectedItemsValueAsString(cbl_staffc);
            divspread.Visible = true;
            Fpspread1.Sheets[0].Visible = true;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].RowCount = 0;

            Fpspread1.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
            CheckAll.AutoPostBack = true;
            CheckInd.AutoPostBack = false;
            string ClgCode = Convert.ToString(ddlcollege.SelectedValue);
            string Allow_code = string.Empty;
            string Deduc_code = string.Empty;
            string allowdeduction = string.Empty;
            string allowselected = string.Empty;
            if (ddl_Allowdeduc.Items.Count > 0)
            {
                Allow_code = d2.GetFunction("select MasterCode from CO_MasterValues where mastercriteria='Additionalallowance' and MasterValue='" + ddl_Allowdeduc.SelectedItem.Text + "' and CollegeCode='" + ClgCode + "'");
                Deduc_code = d2.GetFunction("select MasterCode from CO_MasterValues where mastercriteria='Additionaldeduction' and MasterValue='" + ddl_Allowdeduc.SelectedItem.Text + "' and CollegeCode='" + ClgCode + "'");
                allowdeduction = ddl_Allowdeduc.SelectedItem.Text;
                allowselected = d2.GetFunction("select MasterCriteriaValue1 from co_mastervalues where mastercriteria in('AdditionalAllowance','AdditionalDeduction') and mastervalue='" + allowdeduction + "'");
            }

            //if (ddl_Allowdeduc.SelectedIndex==0)
            //{
            //    query = "select distinct s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber,sex from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and  s.college_code='" + ddlcollege.SelectedItem.Value + "' and resign = 0 and settled = 0 and ISNULL(Discontinue,'0')='0' and latestrec=1 ";
            ////}
            //else
            //{
            query = "select distinct s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber,sex from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and h.dept_code in('" + deptcode + "') and d.desig_code in('" + designation + "') and s.college_code='" + ddlcollege.SelectedItem.Value + "' and resign = 0 and settled = 0 and ISNULL(Discontinue,'0')='0' and latestrec=1 ";
            if (!string.IsNullOrEmpty(staffcategory))
                query += " and st.category_code in('" + staffcategory + "')";
            if (txt_search.Text != "")
                query += " and s.staff_name='" + txt_search.Text + "'";
            if (txt_search1.Text != "")
                query += " and s.staff_code='" + txt_search1.Text + "'";
            query += " order by h.dept_name,d.desig_name";
            query += " select * from AdditionalAllowanceAndDeduction where CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
            if (txt_search1.Text != "")
                query += " and staffcode='" + txt_search1.Text + "'";
            if (Allow_code != "")
            {
                query += " and AllowanceCode='" + Allow_code + "'";
            }
            else if (Deduc_code != "")
            {
                query += " and DeductionCode='" + Deduc_code + "'";
            }
            //}
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "TEXT");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ermsg.Visible = false;
                Fpspread1.Visible = true;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;

                Fpspread1.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Columns[1].Label = "Select";
                Fpspread1.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Name";
                Fpspread1.Sheets[0].ColumnHeader.Columns[3].Label = "Staff Code";
                Fpspread1.Sheets[0].ColumnHeader.Columns[4].Label = "Department";
                Fpspread1.Sheets[0].ColumnHeader.Columns[5].Label = "Designation";
                Fpspread1.Sheets[0].ColumnHeader.Columns[6].Label = "Amount";

                if (!string.IsNullOrEmpty(allowselected))
                {
                    string[] splitallow = allowselected.Split(',');
                    for (int spa = 0; spa < splitallow.Length; spa++)
                    {
                        string deduction = splitallow[spa];
                        Fpspread1.Sheets[0].ColumnCount++;
                        Fpspread1.Sheets[0].ColumnHeader.Columns[Fpspread1.Sheets[0].ColumnCount - 1].Label = deduction;
                    }
                }
                Fpspread1.Sheets[0].Columns[0].Width = 50;
                Fpspread1.Sheets[0].Columns[1].Width = 50;
                Fpspread1.Sheets[0].Columns[2].Width = 150;
                Fpspread1.Sheets[0].Columns[3].Width = 75;
                Fpspread1.Sheets[0].Columns[4].Width = 150;
                Fpspread1.Sheets[0].Columns[5].Width = 140;
                Fpspread1.Sheets[0].Columns[6].Width = 70;

                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Sheets[0].Columns[3].Locked = true;
                Fpspread1.Sheets[0].Columns[4].Locked = true;
                Fpspread1.Sheets[0].Columns[5].Locked = true;
                Fpspread1.Sheets[0].Columns[6].Locked = false;

                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = CheckAll;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                string staffcodedb = string.Empty; string staffcodespread = string.Empty;
                for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    string name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    string code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();
                    Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                    Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Font.Bold = false;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = CheckInd;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = name;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FarPoint.Web.Spread.TextCellType TextCellType = new FarPoint.Web.Spread.TextCellType();
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = TextCellType;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = code;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["appl_id"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["sex"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["pangirnumber"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = txt_amount.Text;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        //for (int allow = 0; allow < ds.Tables[1].Rows.Count; allow++)
                        //{
                        //for (int spr = 1; spr < Fpspread1.Sheets[0].RowCount; spr++)
                        //{
                        //    staffcodespread = Convert.ToString(Fpspread1.Sheets[0].Cells[spr, 3].Text);
                        DataView dv = new DataView();
                        ds.Tables[1].DefaultView.RowFilter = " StaffCode='" + code + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            if (Allow_code != "")
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[0]["AllowanceAmt"]);// ds.Tables[1].Rows[0]["AllowanceAmt"]);
                            }
                            else if (Deduc_code != "")
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[0]["DeductionAmt"]);//ds.Tables[1].Rows[0]["DeductionAmt"]);
                            }
                            int dedu = 6;
                            string[] dedamount = Convert.ToString(dv[0]["AllowanceDeductAmt"]).Split(';');
                            for (int i = 7; i < Fpspread1.Sheets[0].ColumnCount; i++)
                            {
                                string HeaderName = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Columns[i].Label);
                                for (int deduction = 0; deduction < dedamount.Length; deduction++)//Fpspread1.Sheets[0].ColumnCount - 7
                                {
                                    string dedsplit = dedamount[deduction];
                                    if (!string.IsNullOrEmpty(dedsplit))
                                    {
                                        if (dedsplit.Contains('-'))
                                        {
                                            string dedamountval = dedsplit.Split('-')[0];
                                            if (dedamountval.ToUpper() == HeaderName.ToUpper())
                                            {
                                                string dedamountsplit = dedsplit.Split('-')[1];
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, i].Text = dedamountsplit;
                                            }
                                        }
                                    }
                                }
                            }
                            //}
                            //}
                        }
                    }
                }
                Fpspread1.SaveChanges();
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Width = 1000;
                Fpspread1.Height = 400;
                rptprint.Visible = true;


            }
            else
            {
                Fpspread1.Visible = false;
                divspread.Visible = false;
                rptprint.Visible = false;
                ermsg.Visible = true;
                ermsg.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnsavespread_Click(object sender, EventArgs e)
    {
        try
        {
            string InsQ = "";
            int MyUpdCount = 0;
            string ClgCode = Convert.ToString(ddlcollege.SelectedValue);
            string Allow_code = string.Empty;
            string Deduc_code = string.Empty;
            string staffcode = ""; string dedname = string.Empty;
            double amount = 0; double dedamount = 0;
            string myErrTxt = string.Empty;
            //if (CheckSpr())
            //{
            //    if (CheckSprVal(ref myErrTxt))
            //    {
            Fpspread1.SaveChanges();
            if (ddl_Allowdeduc.Items.Count > 0)
            {
                for (int myVal = 1; myVal < Fpspread1.Sheets[0].RowCount; myVal++)
                {
                    byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[myVal, 1].Value);
                    if (Check == 1)
                    {
                        staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 3].Text);
                        Allow_code = d2.GetFunction("select MasterCode from CO_MasterValues where mastercriteria='Additionalallowance' and MasterValue='" + ddl_Allowdeduc.SelectedItem.Text + "' and CollegeCode='" + ClgCode + "'");
                        Deduc_code = d2.GetFunction("select MasterCode from CO_MasterValues where mastercriteria='Additionaldeduction' and MasterValue='" + ddl_Allowdeduc.SelectedItem.Text + "' and CollegeCode='" + ClgCode + "'");
                        double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, 6].Text), out amount);
                        string deductionlist = string.Empty;
                        for (int de = 7; de < Fpspread1.Sheets[0].ColumnCount; de++)
                        {
                            double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[myVal, de].Text), out dedamount);
                            dedname = Fpspread1.Sheets[0].ColumnHeader.Columns[de].Label;
                            if (deductionlist == "")
                                deductionlist = dedname + "-" + dedamount;
                            else
                                deductionlist = deductionlist + ";" + dedname + "-" + dedamount;
                        }
                        if (!String.IsNullOrEmpty(staffcode) && !String.IsNullOrEmpty(Allow_code) && !String.IsNullOrEmpty(Deduc_code))//&& amount != 0
                        {
                            if (radFormat.SelectedIndex == 0)
                            {
                                InsQ = "if exists (select * from AdditionalAllowanceAndDeduction where CollegeCode='" + ClgCode + "' and AllowanceCode='" + Allow_code + "' and StaffCode='" + staffcode + "') update AdditionalAllowanceAndDeduction set AllowanceAmt='" + amount + "',AllowanceDeductAmt ='" + deductionlist + "' where CollegeCode='" + ClgCode + "' and AllowanceCode='" + Allow_code + "' and StaffCode='" + staffcode + "' else insert into AdditionalAllowanceAndDeduction (AllowanceCode,AllowanceAmt,StaffCode,CollegeCode,AllowanceDeductAmt) values ('" + Allow_code + "','" + amount + "','" + staffcode + "','" + ClgCode + "','" + deductionlist + "')";
                            }
                            else if (radFormat.SelectedIndex == 1)
                            {
                                InsQ = "if exists (select * from AdditionalAllowanceAndDeduction where CollegeCode='" + ClgCode + "' and DeductionCode='" + Deduc_code + "' and StaffCode='" + staffcode + "') update AdditionalAllowanceAndDeduction set AllowanceAmt='" + amount + "' where CollegeCode='" + ClgCode + "' and DeductionCode='" + Deduc_code + "' and StaffCode='" + staffcode + "' else insert into AdditionalAllowanceAndDeduction (DeductionCode,AllowanceAmt,StaffCode,CollegeCode) values ('" + Deduc_code + "','" + amount + "','" + staffcode + "','" + ClgCode + "')";
                            }
                            int insCount = d2.update_method_wo_parameter(InsQ, "Text");
                            if (insCount > 0)
                            {
                                MyUpdCount += 1;
                            }
                        }
                        else if (amount == 0)
                        {

                        }
                    }
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select Allowance / Deduction Name";
            }
            if (MyUpdCount > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Saved Successfully";
                btn_go_Click(sender, e);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Staff Name";
            }
            //    }

            //    else
            //    {

            //        imgdiv2.Visible = true;
            //        lbl_alert.Text = myErrTxt;
            //    }
            //}
            //else /* poomalar 23.10.17*/
            //{
            //    if (radFormat.SelectedIndex==0)
            //    {
            //        imgdiv2.Visible = true;
            //        lbl_alert.Text = "Please Enter the Allowance Amount";
            //    }
            //    else
            //    {
            //        imgdiv2.Visible = true;
            //        lbl_alert.Text = "Please Enter the Deduction Amount";
            //    }
            //}
        }
        catch { }
    }

    private bool CheckSpr()
    {
        bool EntryFlag = false;
        try
        {
            Fpspread1.SaveChanges();
            for (int mySpr = 1; mySpr < Fpspread1.Sheets[0].RowCount; mySpr++)
            {
                byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[mySpr, 1].Value);
                if (Check == 1)
                    EntryFlag = true;
            }
        }
        catch { }
        return EntryFlag;
    }

    private bool CheckSprVal(ref string ErrText)
    {
        bool CheckFlag = true;
        try
        {
            string ClgCode = Convert.ToString(ddlcollege.SelectedValue);
            string Allow_code = string.Empty;
            string Deduc_code = string.Empty;
            string staffcode = "";
            double amount = 0;
            Fpspread1.SaveChanges();
            for (int mySpr = 1; mySpr < Fpspread1.Sheets[0].RowCount; mySpr++)
            {
                byte Check = Convert.ToByte(Fpspread1.Sheets[0].Cells[mySpr, 1].Value);
                if (Check == 1)
                {
                    staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[mySpr, 3].Text);
                    Allow_code = d2.GetFunction("select AllowanceCode from AdditionalAllowanceAndDeduction where StaffCode='" + staffcode + "' and CollegeCode='" + ClgCode + "'");
                    Deduc_code = d2.GetFunction("select DeductionCode from AdditionalAllowanceAndDeduction where StaffCode='" + staffcode + "' and CollegeCode='" + ClgCode + "'");
                    double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[mySpr, 6].Text), out amount);

                    if (!String.IsNullOrEmpty(staffcode) && !String.IsNullOrEmpty(Allow_code) && !String.IsNullOrEmpty(Deduc_code))
                    {
                        if (amount == 0)
                        {
                            ErrText = "Please Enter No.of Hours for '" + staffcode + "'!";
                            CheckFlag = false;
                            return CheckFlag;
                        }
                    }
                }
            }
        }
        catch { }
        return CheckFlag;
    }

    protected void Fpspread1_ButtonCommand(object sender, EventArgs e)
    {
        try
        {
            if (Fpspread1.Sheets[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 1].Value) == 1)
                {
                    for (int intF = 0; intF < Fpspread1.Sheets[0].Rows.Count; intF++)
                    {
                        Fpspread1.Sheets[0].Cells[intF, 1].Value = 1;
                    }
                    Fpspread1.Sheets[0].AutoPostBack = true;
                }
                else
                {
                    for (int intF = 0; intF < Fpspread1.Sheets[0].Rows.Count; intF++)
                    {
                        Fpspread1.Sheets[0].Cells[intF, 1].Value = 0;
                    }
                    Fpspread1.Sheets[0].AutoPostBack = true;
                }
            }
        }
        catch
        {

        }
    }

    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        cellclick = true;
    }

    protected void savedetails()
    {
        try
        {

            string collegecode1 = ddl_college1.SelectedItem.Value.ToString();
            string housename = Convert.ToString(ddl_popAllowDeduc.SelectedItem.Text);
            if (housename != "" && housename != "select")
            {
                string query = "";
                int iv = d2.update_method_wo_parameter(query, "Text");
                if (iv != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    popaddnew.Visible = false;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Updated Successfully";
                    popaddnew.Visible = false;
                }
            }

            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select any item";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_plus_Click(object sender, EventArgs e)
    {
        txt_AllowDeducname.Text = "";
        imgdiv3.Visible = true;
        panel_reason.Visible = true;
    }
    protected void btn_minus_Click(object sender, EventArgs e)
    {

        if (ddl_popAllowDeduc.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Items found";
        }
        else if (ddl_popAllowDeduc.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any item";
        }
        else if (ddl_popAllowDeduc.SelectedIndex != 0 && btn_minus.Text == "-")
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";

        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No items found";
        }
    }
    protected void btn_popexit_Click(object sender, EventArgs e)
    {
        popaddnew.Visible = false;
        imgdiv3.Visible = false;
        panel_reason.Visible = false;
    }
    protected void btn_allowsave_Click(object sender, EventArgs e) // poo 30.11.17
    {
        try
        {
            string collegecode1 = ddl_college1.SelectedItem.Value.ToString();
            //string textAllowDeducName = txt_AllowDeducname.Text.ToUpper();
            string allowancename = ddl_popAllowDeduc.SelectedItem.Text;
            string sql = "";
            if (allowancename != "" && allowancename != "Select")
            {
                string Allowdeduname = d2.GetFunction("select MasterValue from co_mastervalues where mastercriteria in('AdditionalAllowance') and mastervalue='" + Convert.ToString(allowancename) + "' and collegecode='" + collegecode1 + "'");
                string deductionselect = rs.GetSelectedItemsValueAsString(cbl_ded);
                string deductionlist = string.Empty;

                for (int ded = 0; ded < cbl_ded.Items.Count; ded++)
                {

                    if (cbl_ded.Items[ded].Selected == true)
                    {
                        if (deductionlist == "")
                            deductionlist = cbl_ded.Items[ded].Text;
                        else
                            deductionlist = deductionlist + "," + cbl_ded.Items[ded].Text;
                    }

                }
                if (!string.IsNullOrEmpty(collegecode1.Trim()) && Allowdeduname != "")
                {
                    if (radFormat.SelectedIndex == 0)
                    {
                        if (cb_deduct.Checked == true)
                        {
                            sql = "  if exists( select MasterValue from co_mastervalues where mastercriteria='AdditionalAllowance' and mastervalue='" + Convert.ToString(allowancename) + "' and collegecode='" + collegecode1 + "') update CO_MasterValues set MasterCriteriaValue1='" + Convert.ToString(deductionlist) + "',mastercriteriavalue2 ='" + txtchqno.Text + "',mastercriteriavalue3='" + txtchqdt.Text + "',mastercriteriavalue4='" + txtchallonNoTransferVoucher.Text + "' where mastercriteria='AdditionalAllowance' and collegecode='" + collegecode1 + "' and MasterValue='" + Convert.ToString(allowancename) + "' else  insert into co_mastervalues(mastercriteria,MasterValue,MasterCriteriaValue1,collegecode,mastercriteriavalue2,mastercriteriavalue3,mastercriteriavalue4) values('AdditionalAllowance','" + Convert.ToString(allowancename) + "','" + Convert.ToString(deductionlist) + "','" + collegecode1 + "','" + txtchqno.Text + "','" + txtchqdt.Text + "','" + txtchallonNoTransferVoucher.Text + "')";
                        }
                        else
                        {
                            sql = "  if exists( select MasterValue from co_mastervalues where mastercriteria='AdditionalAllowance' and mastervalue='" + Convert.ToString(allowancename) + "' and collegecode='" + collegecode1 + "') update CO_MasterValues set MasterValue='" + Convert.ToString(allowancename) + "',mastercriteriavalue2 ='" + txtchqno.Text + "',mastercriteriavalue3='" + txtchqdt.Text + "',mastercriteriavalue4='" + txtchallonNoTransferVoucher.Text + "' where mastercriteria='AdditionalAllowance' and collegecode='" + collegecode1 + "' and MasterValue='" + Convert.ToString(allowancename) + "' else  insert into co_mastervalues(mastercriteria,MasterValue,collegecode,mastercriteriavalue2,mastercriteriavalue3,mastercriteriavalue4) values('AdditionalAllowance','" + Convert.ToString(allowancename) + "','" + collegecode1 + "','" + txtchqno.Text + "','" + txtchqdt.Text + "','" + txtchallonNoTransferVoucher.Text + "')";
                        }
                    }

                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        txt_AllowDeducname.Text = "";
                        imgdiv3.Visible = false;
                        panel_reason.Visible = false;
                    }
                    bind_ddl_popAllowDeduc();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Name already exists";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select the Allowance";
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }

    }
    protected void btn_add_Allowname_Click(object sender, EventArgs e) // to add new allowance or deduction in ddl
    {
        try
        {
            string collegecode1 = ddl_college1.SelectedItem.Value.ToString();
            string textAllowDeducName = txt_AllowDeducname.Text.ToUpper();
            string sql = "";
            if (txt_AllowDeducname.Text != "")
            {
                string Allowdeduname = d2.GetFunction("select MasterValue from co_mastervalues where mastercriteria in('AdditionalAllowance' || 'AdditionalDeduction') and mastervalue='" + Convert.ToString(textAllowDeducName) + "' and collegecode='" + collegecode1 + "'");
                if (!string.IsNullOrEmpty(collegecode1.Trim()) && Allowdeduname == "0")
                {
                    if (radFormat.SelectedIndex == 0)
                    {
                        sql = "  if not exists( select MasterValue from co_mastervalues where mastercriteria='AdditionalAllowance' and mastervalue='" + Convert.ToString(textAllowDeducName) + "' and collegecode='" + collegecode1 + "') insert into co_mastervalues(mastercriteria,MasterValue,collegecode) values('AdditionalAllowance','" + Convert.ToString(textAllowDeducName) + "','" + collegecode1 + "')";
                    }
                    else if (radFormat.SelectedIndex == 1)
                    {
                        sql = "  if not exists( select MasterValue from co_mastervalues where mastercriteria='AdditionalDeduction' and mastervalue='" + Convert.ToString(textAllowDeducName) + "' and collegecode='" + collegecode1 + "') insert into co_mastervalues(mastercriteria,MasterValue,collegecode) values('AdditionalDeduction','" + Convert.ToString(textAllowDeducName) + "','" + collegecode1 + "')";
                    }

                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        txt_AllowDeducname.Text = "";
                        imgdiv3.Visible = false;
                        panel_reason.Visible = false;
                    }
                    bind_ddl_popAllowDeduc();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Name already exists";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Enter the Name";
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }

    protected void btn_exit_Allowname_Click(object sender, EventArgs e)
    {
        popaddnew.Visible = true;
        panel_reason.Visible = false;
        imgdiv3.Visible = false;
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popaddnew.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        Delete(sender, e);
        bind_ddl_popAllowDeduc();
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
    }
    protected void Delete(object sender, EventArgs e)
    {
        try
        {
            string collegecode1 = ddl_college1.SelectedItem.Value.ToString();
            string allowdeduc = Convert.ToString(ddl_popAllowDeduc.SelectedItem.Text);
            string query2 = "";
            if (radFormat.SelectedIndex == 0)
            {
                query2 = "delete from AdditionalAllowanceAndDeduction where AllowanceCode ='" + ddl_popAllowDeduc.SelectedItem.Value + "' and CollegeCode='" + collegecode1 + "'";
                query2 += " delete CO_MasterValues where MasterCriteria='AdditionalAllowance' and MasterCode='" + ddl_popAllowDeduc.SelectedItem.Value + "' and MasterValue='" + allowdeduc + "' and CollegeCode='" + collegecode1 + "'";
            }
            else if (radFormat.SelectedIndex == 1)
            {
                query2 = "delete from AdditionalAllowanceAndDeduction where DeductionCode ='" + ddl_popAllowDeduc.SelectedItem.Value + "' and CollegeCode='" + collegecode1 + "'";
                query2 += " delete CO_MasterValues where MasterCriteria='AdditionalDeduction' and MasterCode='" + ddl_popAllowDeduc.SelectedItem.Value + "' and MasterValue='" + allowdeduc + "' and CollegeCode='" + collegecode1 + "'";
            }
            int iv = d2.update_method_wo_parameter(query2, "Text");
            if (iv != 0)
            {
                imgdiv2.Visible = true;
                btn_go_Click(sender, e);
                bind_ddl_popAllowDeduc();
                lbl_alert.Text = "Deleted Successfully";
                popaddnew.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void btn_popdelete_Click(object sender, EventArgs e)
    {
        if (btn_popdelete.Text == "Delete")
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";
        }

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }


        }
        catch (Exception ex)
        {            //alertmessage.Visible = true;
            //lbl_alerterror.Visible = true;
            //lbl_alerterror.Text = ex.ToString();

        }
    }

    protected void btnprintcell_click(object sender, EventArgs e)
    {
        //individualdiv.Visible = true;
        ScriptManager.RegisterStartupScript(this, GetType(), "btnprintcell", "PrtDiv();", true);
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

}