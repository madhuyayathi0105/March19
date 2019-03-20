using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Drawing;
using AjaxControlToolkit;
using System.Globalization;
public partial class SalaryHoldSet : System.Web.UI.Page
{

    static string clgcode1 = string.Empty;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string dtfromdate = string.Empty;
    string dt1todate = string.Empty;
    string m = string.Empty;
    DataSet ds = new DataSet();
    DataSet ss = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet dsYr = new DataSet();
    bool genchk = false;
    bool check = false;
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    DateTime joindate = new DateTime();
    DataSet dnew = new DataSet();
    string dtaccessdate = DateTime.Now.ToString();
    string dtaccesstime = DateTime.Now.ToLongTimeString();
    int days = 0;
    string hoscode = "";
    string clgcode = "";
    string hoscode1 = "";
    string messcode = "";
    string monthvalue = "";
    bool isbate = false;
    string lblgetscode = "";
    int missedcount = 0;
    int gencount = 0;
    int deductionval = 0;//delsi
    Hashtable rebetedays_hash = new Hashtable();
    Hashtable rebeteamt_hash = new Hashtable();
    Hashtable grantday_hash = new Hashtable();
    Hashtable grantamt_hash = new Hashtable();
    Hashtable guestrebetedays_hash = new Hashtable();
    Hashtable guestgrant_hash = new Hashtable();
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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (clgcode1 == "")
            {
                if (ddlcollege.Items.Count > 0)
                    clgcode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                binddept();
                designation();
                bindstftype();
                staffcategory();
                bindyear();

            }
        }
        catch (Exception ex) { }
    }
    protected void ddlcollege_change(object sender, EventArgs e)
    {
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        //bindyear();
      //  Fpspread1.Visible = false;
       // lbl_error.Visible = false;
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
    protected void cbdeptcom_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbdeptcom, cbldeptcom, txtdeptcom, "Department");
        designation();
        //if (cbDesig.Checked == true)
        //{
        //    designation();
        //}
    }
    protected void cbldeptcom_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbdeptcom, cbldeptcom, txtdeptcom, "Department");
        int countval = 0;

        string depart_code = GetSelectedItemsValueAsString(cbldeptcom, out countval);
       // designation();
       
        //if (cbDesig.Checked == true)
        //{
        //    designation();
        //}
    }

    protected void binddept()//delsi
    {
        try
        {
            ds.Clear();
           
            cbldeptcom.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct Dept_Code,Dept_Name from hrdept_master where college_code='" + collcode + "' order by Dept_Name ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
              
                cbldeptcom.DataSource = ds;
                cbldeptcom.DataTextField = "Dept_Name";
                cbldeptcom.DataValueField = "Dept_Code";
                cbldeptcom.DataBind();
                
                if (cbldeptcom.Items.Count > 0)
                {
                    for (int i = 0; i < cbldeptcom.Items.Count; i++)
                    {
                        cbldeptcom.Items[i].Selected = true;
                    }
                    txtdeptcom.Text = "Department (" + cbldeptcom.Items.Count + ")";
                    cbdeptcom.Checked = true;
                }
               // designation();
            }
            else
            {
                
                txtdeptcom.Text = "--Select--";
                
                cbdeptcom.Checked = false;
            }
        }
        catch { }
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
    protected void designation()
    {
        try
        {
           // if (chkdept.Checked == true)
            //{
                //cbl_desig.Items.Clear();
                //txt_desig.Text = "--Select--";
                //cb_desig.Checked = false;
                //Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
                //dicgetcode.Clear();
                //Dictionary<string, string> dicdescode = new Dictionary<string, string>();
                //dicdescode.Clear();
                //string collcode = Convert.ToString(ddlcollege.SelectedValue);
                //if (cbldeptcom.Items.Count > 0)
                //{
                //    for (int ik = 0; ik < cbldeptcom.Items.Count; ik++)
                //    {
                //        if (cbldeptcom.Items[ik].Selected == true)
                //        {
                //            if (!dicgetcode.ContainsKey(Convert.ToString(cbldeptcom.Items[ik].Value)))
                //            {
                //                string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbldeptcom.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbldeptcom.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbldeptcom.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbldeptcom.Items[ik].Value) + "'))";
                //                ds.Clear();
                //                ds = d2.select_method_wo_parameter(selq, "Text");
                //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //                {
                //                    for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
                //                    {
                //                        if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])))
                //                        {
                //                            cbl_desig.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])));
                //                            dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]));
                //                        }
                //                    }
                //                }
                //                dicgetcode.Add(Convert.ToString(cbldeptcom.Items[ik].Value), Convert.ToString(cbldeptcom.Items[ik].Text));
                //            }
                //        }
                //    }
                //}
                //if (cbl_desig.Items.Count > 0)
                //{
                //    for (int i = 0; i < cbl_desig.Items.Count; i++)
                //    {
                //        cbl_desig.Items[i].Selected = true;
                //    }
                //    txt_desig.Text = "Designation (" + cbl_desig.Items.Count + ")";
                //    cb_desig.Checked = true;
                //}
           // }
           // if (chkdept.Checked == false)
            //{
                ds.Clear();
                cbl_desig.Items.Clear();

                string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' order by desig_name";
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
            //}
        }
        catch (Exception ex)
        {
        }
    }
    //protected void chkdept_change(object sender, EventArgs e)
    //{
    //    if (chkdept.Checked == true)
    //    {
    //        binddept();
    //        txtdeptcom.Enabled = true;
    //    }
    //    else
    //    {
    //        cbldeptcom.Items.Clear();
    //        txtdeptcom.Text = "--Select--";
    //        txtdeptcom.Enabled = false;
    //    }
    //    designation();
    //}
    //protected void cbDesigChange(object sender, EventArgs e)
    //{
    //    if (cbDesig.Checked == true)
    //    {
    //        txt_desig.Enabled = true;
    //        //binddept();
    //        designation();
    //    }
    //    else
    //    {
    //        cbl_desig.Items.Clear();
    //        txt_desig.Text = "--Select--";
    //        txt_desig.Enabled = false;
    //    }
    //}
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected, out int count)
    {
        count = 0;
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    count++;
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    protected void cb_desig_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cbl_desig_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cbstftypecom_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbstftypecom, cblstftypecom, txtstftypecom, "Staff Type");
    }
    protected void cblstftypecom_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbstftypecom, cblstftypecom, txtstftypecom, "Staff Type");
    }
    protected void bindstftype()
    {
        try
        {
            ds.Clear();
            
            cblstftypecom.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
              
                cblstftypecom.DataSource = ds;
                cblstftypecom.DataTextField = "stftype";
                cblstftypecom.DataBind();
               
                if (cblstftypecom.Items.Count > 0)
                {
                    for (int i = 0; i < cblstftypecom.Items.Count; i++)
                    {
                        cblstftypecom.Items[i].Selected = true;
                    }
                    txtstftypecom.Text = "Staff Type (" + cblstftypecom.Items.Count + ")";
                    cbstftypecom.Checked = true;
                }
            }
            else
            {
                
                txtstftypecom.Text = "--Select--";
               
                cbstftypecom.Checked = false;
            }
        }
        catch { }
    }
    protected void cbscatcom_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbscatcom, cblscatcom, txtscatcom, "Staff Category");
    }
    protected void cblscatcom_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbscatcom, cblscatcom, txtscatcom, "Staff Category");
    }

    protected void staffcategory()
    {
        try
        {
            ds.Clear();
            
            cblscatcom.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct category_name,category_code from staffcategorizer where college_code= '" + collcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                
                cblscatcom.DataSource = ds;
                cblscatcom.DataTextField = "category_name";
                cblscatcom.DataValueField = "category_code";
                cblscatcom.DataBind();
               
                if (cblscatcom.Items.Count > 0)
                {
                    for (int i = 0; i < cblscatcom.Items.Count; i++)
                    {
                        cblscatcom.Items[i].Selected = true;
                    }
                    txtscatcom.Text = "Staff Category (" + cblscatcom.Items.Count + ")";
                    cbscatcom.Checked = true;
                }
            }
            else
            {
               
                txtscatcom.Text = "--Select--";
               
                cbscatcom.Checked = false;
            }
        }
        catch { }
    }

  

   

    public static List<string> GetStaffName(string prefixText)
    {
        //string collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (!String.IsNullOrEmpty(prefixText.Trim()))
        {
            if (clgcode1 != "")
            {
                string query = "select staff_name  from staffmaster where (resign =0 and settled =0 and isnull (Discontinue,'0') ='0') and staff_name like  '%" + prefixText + "%' and college_code='" + clgcode1 + "'";
                name = ws.Getname(query);
            }
        }
        return name;
    }

    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (!String.IsNullOrEmpty(prefixText.Trim()))
        {
            if (clgcode1 != "")
            {
                string query = "select staff_code from staffmaster where (resign =0 and settled =0 and isnull (Discontinue,'0') ='0') and staff_code like  '%" + prefixText + "%' and college_code='" + clgcode1 + "'";
                name = ws.Getname(query);
            }
        }
        return name;
    }
    protected void ddl_mon_Change(object sender, EventArgs e)
    {
        bindyear();
    }
    public void bindyear()
    {
        try
        {
            collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            ddl_year.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("select distinct year(To_Date) as year from HrPayMonths where College_Code ='" + collegecode1 + "' order by year asc", "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_year.DataSource = ds;
                    ddl_year.DataTextField = "year";
                    ddl_year.DataValueField = "year";
                    ddl_year.DataBind();
                }
            }
        }
        catch { }
    }
    protected void grdstaffhold_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdstaffhold.PageIndex = e.NewPageIndex;
        btnshow_click(sender, e);
    }
    protected void btnshow_click(object sendet, EventArgs e)
    { 
        collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
        try
        {
            string dept = "";
            string stftype = "";
            string desig = "";
            string staffcategory = "";
            int depcount = 0;
            int desigcount = 0;
            int stfcount = 0;
            int catcount = 0;
            dept = GetSelectedItemsValueAsString(cbldeptcom, out depcount);
            desig = GetSelectedItemsValueAsString(cbl_desig, out desigcount);
            stftype = GetSelectedItemsText(cblstftypecom, out stfcount);
            staffcategory = GetSelectedItemsValueAsString(cblscatcom, out catcount);
            string year1 = "";
            string month1 = "";
            year1 = ddl_year.SelectedItem.Value;

            int monthIndex;
            string desiredMonth = ddl_mon.SelectedItem.Text;
            string[] MonthNames=CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            monthIndex = Array.IndexOf(MonthNames, desiredMonth) + 1;
            month1 =Convert.ToString( monthIndex);
            string selq = "select m.staff_code,s.staff_name,m.netadd,m.netded,m.netsal,m.paymonth,m.payyear from monthlypay m,staffmaster s,staff_appl_master sa,stafftrans st,hrdept_master h,desig_master d,staffcategorizer sc where m.college_code=s.college_code and m.college_code=h.college_code and m.college_code=d.collegeCode and m.college_code=sc.college_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=sc.college_code and m.staff_code=s.staff_code and m.staff_code=st.staff_code and s.staff_code=st.staff_code and s.appl_no=sa.appl_no and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.category_code=sc.category_code and m.category_code=sc.category_code and m.category_code=st.category_code and st.latestrec='1' and ((s.resign='0' and s.settled='0') and (Discontinue='0' or Discontinue is null)) and PayMonth='" + month1 + "' and PayYear='" + year1 + "' and m.college_code='" + collegecode1 + "'";
            if (dept.Trim() != "")
                selq = selq + " and st.dept_code in('" + dept + "')";
            if (desig.Trim() != "")
                selq = selq + " and st.desig_code in('" + desig + "')";
            if (stftype.Trim() != "")
                selq = selq + " and st.stftype in('" + stftype + "')";
            if (staffcategory.Trim() != "")
                selq = selq + " and st.category_code in('" + staffcategory + "')";
            //if (!string.IsNullOrEmpty(staffCode))
            //    selq = selq + " and s.staff_code in('" + staffCode + "')";//staffName
            //if (!string.IsNullOrEmpty(staffName))
            //    selq = selq + " and s.staff_name in('" + staffName + "')";//staffCode
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = new DataView();
                DataSet dskit = new DataSet();
                DataTable dtstaffhold = new DataTable();
                DataRow drow;
                dtstaffhold.Columns.Add("Staff Code", typeof(string));
                dtstaffhold.Columns.Add("Staff Name", typeof(string));
                dtstaffhold.Columns.Add("PayMonth", typeof(string));
                dtstaffhold.Columns.Add("PayYear", typeof(string));


                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    drow = dtstaffhold.NewRow();
                    drow["Staff Code"] = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]).Trim();
                    drow["Staff Name"] = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]).Trim();
                    drow["PayMonth"] = Convert.ToString(ds.Tables[0].Rows[row]["paymonth"]).Trim();
                    drow["PayYear"] = Convert.ToString(ds.Tables[0].Rows[row]["payyear"]).Trim();
                    dtstaffhold.Rows.Add(drow);
                }
                grdstaffhold.DataSource = dtstaffhold;
                grdstaffhold.DataBind();
                grdstaffhold.Visible = true;
                btnsave.Visible = true;

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No records Found')", true);
            
            }
        }
        catch (Exception ex)
        { 
        
        }

    
    }
    protected void grdstaffhold_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((CheckBox)e.Row.FindControl("allchk")).Attributes.Add("onclick",
                "javascript:SelectAll('" +
                ((CheckBox)e.Row.FindControl("allchk")).ClientID + "')");
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;
           
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;

        }
    }

    private string GetSelectedItemsText(CheckBoxList cblSelected, out int count)
    {
        count = 0;
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    count++;
                    if (sbSelected.Length == 0)
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    else
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    protected void btn_save_click(object sender, EventArgs e)
    {
        try
        {
            collegecode1 = ddlcollege.SelectedItem.Value;
            if (grdstaffhold.Rows.Count > 0)
            {
                int checkcount = 0;
                int select_count = 0;
                foreach (GridViewRow gvrow in grdstaffhold.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        select_count++;
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        string staffcode = Convert.ToString(grdstaffhold.Rows[RowCnt].Cells[2].Text);
                        string staffname = Convert.ToString(grdstaffhold.Rows[RowCnt].Cells[3].Text);
                        string pay_month = Convert.ToString(grdstaffhold.Rows[RowCnt].Cells[4].Text);
                        string pay_year = Convert.ToString(grdstaffhold.Rows[RowCnt].Cells[5].Text);
                        int val = 0;
                        if (rdbhold.Checked == true)
                        {
                            val = 1;
                        }
                        if (rdbUnhold.Checked == true)
                        {
                            val = 2;

                        }

                        string upqury = "update monthlypay set staffholdSet='" + val + "' where staff_code='" + staffcode + "' and PayMonth='" + pay_month + "' and PayYear='" + pay_year + "'and college_code='" + collegecode1 + "'";
                        int val1 = d2.update_method_wo_parameter(upqury, "text");
                        if (val1 > 0)
                        {
                            checkcount++;

                        }

                    }
                    
                }
                if (checkcount > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

                }
               
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
            
            }

        }
        catch (Exception ex)
        { 
        
        
        }
    
    
    }
    protected void rdbhold_check(object sender, EventArgs e)
    {
        if (rdbhold.Checked == true)
        {
            rdbUnhold.Checked = false;
        
        }
        else  if (rdbUnhold.Checked == true)
        {
            rdbhold.Checked = false;
        
        }
    
    }
    protected void rdbunhold_check(object sender, EventArgs e)
    {
        if (rdbUnhold.Checked == true)
        {
            rdbhold.Checked = false;
        
        }
        else if (rdbhold.Checked == true)
        {
            rdbUnhold.Checked = false;
        }
    }
    //protected void chkstftype_change(object sender, EventArgs e)
    //{
    //    if (chkstftype.Checked == true)
    //    {
    //        bindstftype();
    //        txtstftypecom.Enabled = true;
    //    }
    //    else
    //    {
    //        cblstftypecom.Items.Clear();
    //        txtstftypecom.Text = "--Select--";
    //        txtstftypecom.Enabled = false;
    //    }
    //}
}

