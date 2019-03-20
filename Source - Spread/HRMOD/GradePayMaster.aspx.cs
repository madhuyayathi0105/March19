using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Configuration;
public partial class GradePayMaster : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    string allownew = "";
    string dednew = "";
    string staffcodespr = "";
    string[] splallnew = new string[50];
    string[] splallamnt = new string[15];
    string[] spldednew = new string[50];
    string[] spldedamnt = new string[15];
    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
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
                    clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                binddept();
                designation();
                category();
                stafftype();
                staffstatus();
                //allowance();
                //deduction();
                //Leave();
            }
            if (ddlcollege.Items.Count > 0)
                clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
            lblsmserror.Visible = false;
            com_err.Visible = false;
            lbl_alert.Visible = false;
        }
        catch (Exception ex) { }
    }
    protected void ddlcollege_change(object sender, EventArgs e)
    {
        try
        {
            binddept();
            designation();
            category();
            stafftype();
            allowance();
            deduction();
            sp_div.Visible = false;
            FpSpread.Visible = false;
            lbl_alert.Visible = false;
            rprint.Visible = false;
            //Leave();
        }
        catch { }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '%" + prefixText + "%' and college_code='" + clgcode + "'";
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
            string degreedetails = "Grade Pay Master";
            string pagename = "GradePayMaster.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }
    protected void cbismpfamnt_change(object sender, EventArgs e)
    {
        try
        {
            if (cbismpfamnt.Checked == true)
            {
                lblismpf.Visible = true;
                txtismpf.Visible = true;
                txtismpf.Text = "";
                txtismpfper.Text = "";
                lblismpfper.Visible = true;
                txtismpfper.Visible = true;
            }
            else
            {
                lblismpf.Visible = false;
                txtismpf.Visible = false;
                txtismpf.Text = "";
                txtismpfper.Text = "";
                lblismpfper.Visible = false;
                txtismpfper.Visible = false;
            }
        }
        catch { }
    }
    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string item = "select distinct dept_code,dept_name from hrdept_master where college_code = '" + clgcode + "' order by dept_name";
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
        try
        {
            ds.Clear();
            cbl_desig.Items.Clear();
            string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + clgcode + "' order by desig_name";
            ds = da.select_method_wo_parameter(statequery, "Text");
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
        catch { }
    }
    protected void category()
    {
        try
        {
            ds.Clear();
            cbl_staffc.Items.Clear();
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + clgcode + "' order by category_Name";
            ds = da.select_method_wo_parameter(statequery, "Text");
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
        catch { }
    }
    protected void stafftype()
    {
        try
        {
            ds.Clear();
            cbl_stype.Items.Clear();
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + clgcode + "' order by stftype";
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
            string item = "select distinct stfstatus from stafftrans where stfstatus is not null and stfstatus <>'' order by stfstatus";
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
    protected void allowance()
    {
        try
        {
            ds.Clear();
            cbl_allow.Items.Clear();
            lb_allowhdr.Items.Clear();
            lb_allowhdrs.Items.Clear();//delsi
            lb_selallow.Items.Clear();
            lb_selallows.Items.Clear();//delsi
            string item = "select allowances from incentives_master where college_code = '" + clgcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_allow.DataSource = ds;
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    if (split1.Length > 1)
                    {
                        string stafftype = split1[0];
                        cbl_allow.Items.Add(stafftype);
                        lb_allowhdr.Items.Add(new ListItem(stafftype, Convert.ToString(row + 2)));
                        lb_allowhdrs.Items.Add(new ListItem(stafftype, Convert.ToString(row + 2)));//delsi
                    }
                }
                if (cbl_allow.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_allow.Items.Count; i++)
                    {
                        cbl_allow.Items[i].Selected = true;
                    }
                    txt_allow.Text = "Allowance (" + cbl_allow.Items.Count + ")";
                    cb_allow.Checked = true;
                }
            }
            else
            {
                txt_allow.Text = "--Select--";
                cb_allow.Checked = false;
            }
        }
        catch { }
    }
    protected void deduction()
    {
        try
        {
            ds.Clear();
            cbl_deduction.Items.Clear();
            string item = "select deductions from incentives_master  where college_code = '" + clgcode + "' ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string st = Convert.ToString(ds.Tables[0].Rows[0]["deductions"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    if (split1.Length > 1)
                    {
                        string stafftype = split1[0];
                        cbl_deduction.Items.Add(stafftype);
                    }
                }
                if (cbl_deduction.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_deduction.Items.Count; i++)
                    {
                        cbl_deduction.Items[i].Selected = true;
                    }
                    txt_deduct.Text = "Deduction (" + cbl_deduction.Items.Count + ")";
                    cb_deduction.Checked = true;
                }
            }
            else
            {
                txt_deduct.Text = "--Select--";
                cb_deduction.Checked = false;
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
    protected void cb_stype_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_stype, cbl_stype, txt_stype, "StaffType");
    }
    protected void cbl_stype_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stype, cbl_stype, txt_stype, "StaffType");
    }
    protected void cb_stat_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_stat, cbl_stat, txt_stat, "Status");
    }
    protected void cbl_stat_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stat, cbl_stat, txt_stat, "Status");
    }
    protected void cb_allow_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_allow, cbl_allow, txt_allow, "Allowance");
    }
    protected void cbl_allow_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_allow, cbl_allow, txt_allow, "Allowance");
    }
    protected void cb_deduction_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_deduction, cbl_deduction, txt_deduct, "Deduction");
    }
    protected void cbl_deduction_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_deduction, cbl_deduction, txt_deduct, "Deduction");
    }
    protected void ddl_mode_indexchanged(object sender, EventArgs e)
    {
        try
        {
            cb_lop.Checked = false;
            cb_special.Checked = false;
            ddl_round.SelectedIndex = 0;
            if (ddl_mode.SelectedItem.Text == "Amount")
            {
                txt_val.Text = "0.00";
                txt_val.Enabled = true;
                chkalldisable();
            }
            else if (ddl_mode.SelectedItem.Text == "Percent")
            {
                txt_val.Text = "";
                txt_val.Enabled = true;
                chkallenable();
            }
            else
            {
                txt_val.Text = "";
                txt_val.Enabled = false;
                chkallenable();
            }
        }
        catch { }
    }
    public void chkallenable()
    {
        cb_lop.Checked = true;
        cb_lop.Enabled = true;
        cb_fbasic.Checked = false;
        cb_fbasic.Enabled = true;
        cb_fbgp.Checked = false;
        cb_fbgp.Enabled = true;
        cb_special.Checked = false;
        cb_special.Enabled = true;
        cb_agp.Checked = false;
        cb_agp.Enabled = true;
        cb_fromallallow.Checked = false;//delsi
        cb_fromallallow.Enabled = true;//delsi

    }
    public void chkalldisable()
    {
        cb_lop.Checked = false;
        cb_lop.Enabled = true;
        cb_fbasic.Checked = false;
        cb_fbasic.Enabled = false;
        cb_fbgp.Checked = false;
        cb_fbgp.Enabled = false;
        cb_special.Checked = false;
        cb_special.Enabled = true;
        cb_agp.Checked = false;
        cb_agp.Enabled = false;
        cb_fromallallow.Checked = false;
        cb_fromallallow.Enabled = false;
        txt_all_allowVal.Enabled = false;
        txt_all_allowVal.Text = "";
        cb_fromallallow.Checked = false;//delsi
        cb_fromallallow.Enabled = false;//delsi
    }
    protected void ddl_dmode_indexchanged(object sender, EventArgs e)
    {
        cb_mcal.Checked = false;
        txt_mamt.Text = "";
        //txt_damt.Text = "";
        cb_ilop.Checked = false;
        ddl_rt.SelectedIndex = 0;
        if (ddl_dmode.SelectedItem.Text == "Amount")
        {
            txt_dval.Text = "0.00";
            txt_dval.Enabled = true;
            chkdeddisable();
        }
        else if (ddl_dmode.SelectedItem.Text == "Percent")
        {
            txt_dval.Text = "";
            txt_dval.Enabled = true;
            chkdedenable();
        }
        else
        {
            txt_dval.Text = "";
            txt_dval.Enabled = false;
            chkdedenable();
        }
    }
    public void chkdedenable()//delsi2004
    {
        cb_fg.Checked = true;
        cb_fg.Enabled = true;
        cb_fbda.Checked = false;
        cb_fbda.Enabled = true;
        cb_ilop.Checked = false;
        cb_ilop.Enabled = true;
        cb_fbgpda.Checked = false;
        cb_fbgpda.Enabled = true;
        cb_fb.Checked = false;
        cb_fb.Enabled = true;
        cb_fbdp.Checked = false;
        cb_fbdp.Enabled = true;
        cb_fp.Checked = false;
        cb_fp.Enabled = true;
        cb_fbarr.Checked = false;
        cb_fbarr.Enabled = true;
        cb_mcal.Checked = false;
        cb_mcal.Enabled = true;
        cb_fbas.Checked = false;
        cb_fbas.Enabled = true;
        cb_fallow.Checked = false;
        cb_fallow.Enabled = true;
        rb_frmnet.Checked = false;
        rb_frmnet.Enabled = true;
        txtcomded.Text = "";
    }
    public void chkdeddisable()
    {
        cb_fg.Checked = false;
        cb_fg.Enabled = false;
        cb_fbda.Checked = false;
        cb_fbda.Enabled = false;
        cb_ilop.Checked = false;
        cb_ilop.Enabled = true;
        cb_fbgpda.Checked = false;
        cb_fbgpda.Enabled = false;
        cb_fb.Checked = false;
        cb_fb.Enabled = false;
        cb_fbdp.Checked = false;
        cb_fbdp.Enabled = false;
        cb_fp.Checked = false;
        cb_fp.Enabled = false;
        cb_fbarr.Checked = false;
        cb_fbarr.Enabled = false;
        cb_mcal.Checked = false;
        cb_mcal.Enabled = true;
        cb_fbas.Checked = false;
        cb_fbas.Enabled = false;
        cb_fallow.Checked = false;
        cb_fallow.Enabled = false;
        rb_frmnet.Checked = false;
        rb_frmnet.Enabled = false;
        txtcomded.Text = "";
    }
    protected void txt_scode_Change(object sender, EventArgs e)
    {
        txt_sname.Text = "";
    }
    protected void txt_sname_Change(object sender, EventArgs e)
    {
        txt_scode.Text = "";
    }
    protected void chk_allvis_change(object sender, EventArgs e)
    {
        if (chk_allvis.Checked == true)
        {
            txt_allow.Enabled = true;
            allowance();
        }
        if (chk_allvis.Checked == false)
        {
            txt_allow.Enabled = false;
            cbl_allow.Items.Clear();
            txt_allow.Text = "--Select--";
        }
    }
    protected void chk_dedvis_change(object sender, EventArgs e)
    {
        if (chk_dedvis.Checked == true)
        {
            txt_deduct.Enabled = true;
            deduction();
        }
        if (chk_dedvis.Checked == false)
        {
            txt_deduct.Enabled = false;
            cbl_deduction.Items.Clear();
            txt_deduct.Text = "--Select--";
        }
    }
    protected void FpSpread_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        FpSpread.SaveChanges();
        byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[0, 1].Value);
        if (check == 1)
        {
            for (int ik = 1; ik < FpSpread.Sheets[0].RowCount; ik++)
            {
                FpSpread.Sheets[0].Cells[ik, 1].Value = 1;
            }
        }
        else
        {
            for (int ik = 1; ik < FpSpread.Sheets[0].RowCount; ik++)
            {
                FpSpread.Sheets[0].Cells[ik, 1].Value = 0;
            }
        }
    }
    public void btn_go_Click(object sender, EventArgs e)
    {
        string college = ddlcollege.SelectedValue;
        try
        {
            ArrayList arrallow = new ArrayList();
            ArrayList arrded = new ArrayList();
            arrallow.Add("Mode");
            arrallow.Add("Amnt/Per");
            arrallow.Add("IncLop");
            arrallow.Add("From Basic");
            arrallow.Add("From Basic+GP");
            arrallow.Add("Is Spl Allow");
            arrallow.Add("From Basic+AGP");
            arrallow.Add("Round Type");
            arrded.Add("Mode");
            arrded.Add("Amnt/Per");
            arrded.Add("Round Type");
            arrded.Add("From Gross");
            arrded.Add("From Basic+DA");
            arrded.Add("Inc Lop");
            arrded.Add("From Basic+GP+DA");
            arrded.Add("From Basic");
            arrded.Add("From Basic+DP");
            arrded.Add("From Petty");
            arrded.Add("IsMax Cal");
            arrded.Add("Max Amnt");
            arrded.Add("Deduct Amnt");
            arrded.Add("From Basic+Arrear");
            arrded.Add("From Basic+Arrear+SA");
            arrded.Add("From Allow");
            arrded.Add("From Net Amount");
            Printcontrol.Visible = false;
            FpSpread.Sheets[0].RowCount = 0;
            FpSpread.Sheets[0].ColumnCount = 11;
            string selectquery = "";
            string scode = txt_scode.Text;
            string sname = txt_sname.Text;
            string dept = "";
            string desig = "";
            string category = "";
            string stype = "";
            string status = "";
            string allowance = "";
            string deduction = "";
            dept = GetSelectedItemsText(cbl_dept);
            desig = GetSelectedItemsText(cbl_desig);
            category = GetSelectedItemsValueAsString(cbl_staffc);
            stype = GetSelectedItemsText(cbl_stype);
            status = GetSelectedItemsText(cbl_stat);
            allowance = GetSelectedItemsText(cbl_allow);
            deduction = GetSelectedItemsText(cbl_deduction);
            if (txt_scode.Text != "")
            {
                selectquery = "select t.staff_code,t.stfstatus,staff_name,h.dept_name,g.desig_name,stftype,category_name,c.category_code,bsalary,basicpay,grade_pay,pay_band ,IsMPFAmt,MPFAmount,MPFPer,t.allowances,t.deductions from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c, staff_appl_master sa where m.appl_no=sa.appl_no and t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and m.college_code = '" + college + "' and t.staff_code='" + scode + "' and sa.interviewstatus ='appointed'";
            }
            else if (txt_sname.Text != "")
            {
                selectquery = "select t.staff_code,t.stfstatus,staff_name,h.dept_name,g.desig_name,stftype,category_name,c.category_code,bsalary,basicpay,grade_pay,pay_band ,IsMPFAmt,MPFAmount,MPFPer,t.allowances,t.deductions from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c, staff_appl_master sa where m.appl_no=sa.appl_no and t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and m.college_code = '" + college + "' and staff_name='" + sname + "' and sa.interviewstatus ='appointed'";
            }
            else
            {
                selectquery = "select t.staff_code,t.stfstatus,staff_name,h.dept_name,g.desig_name,stftype,category_name,c.category_code,bsalary,basicpay,grade_pay,pay_band ,IsMPFAmt,MPFAmount,MPFPer,t.allowances,t.deductions from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c, staff_appl_master sa where m.appl_no=sa.appl_no and t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and m.college_code = '" + college + "' and h.dept_name in('" + dept + "') and g.desig_name in('" + desig + "') and c.category_code in('" + category + "') and t.stftype in('" + stype + "') and t.stfstatus in('" + status + "') and sa.interviewstatus ='appointed'";
            }
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int colcount = 0;
                int coldedcount = 0;
                int colnew = 0;
                int coldednew = 0;
                Hashtable allcol = new Hashtable();
                allcol.Clear();
                Hashtable dedcol = new Hashtable();
                dedcol.Clear();
                sp_div.Visible = true;
                FpSpread.Visible = true;
                FpSpread.Sheets[0].RowCount = 0;
                FpSpread.Sheets[0].ColumnCount = 0;
                FpSpread.CommandBar.Visible = false;
                FpSpread.Sheets[0].AutoPostBack = false;
                FpSpread.Sheets[0].ColumnHeader.RowCount = 3;
                FpSpread.Sheets[0].FrozenRowCount = 1;
                FpSpread.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                DataSet dsgetall = new DataSet();
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "S.No";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 50;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Select";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 80;
                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell.AutoPostBack = false;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Staff Code";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 75;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Staff Name";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 175;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Department";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 150;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Designation";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 150;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Staff Category";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 125;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Staff Type";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 75;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Grade Pay";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Basic Pay";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Pay Band";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
                FpSpread.Sheets[0].ColumnCount++;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Staff Status";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 3, 1);
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
                if (chk_allvis.Checked == true && txt_allow.Text.Trim() != "--Select--")
                {
                    for (int cb = 0; cb < cbl_allow.Items.Count; cb++)
                    {
                        if (cbl_allow.Items[cb].Selected == true)
                        {
                            FpSpread.Sheets[0].ColumnCount++;
                            colcount = Convert.ToInt32(FpSpread.Sheets[0].ColumnCount - 1);
                            for (int ar = 0; ar < arrallow.Count; ar++)
                            {
                                if (ar != 0)
                                    FpSpread.Sheets[0].ColumnCount++;
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(arrallow[ar]);
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(2, FpSpread.Sheets[0].ColumnCount - 1, 1, 1);
                                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                                colnew++;
                            }
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, colcount].Text = Convert.ToString(cbl_allow.Items[cb].Text);
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount, 1, 8);
                            if (!allcol.ContainsKey(Convert.ToString(cbl_allow.Items[cb].Text)))
                                allcol.Add(Convert.ToString(cbl_allow.Items[cb].Text), Convert.ToString(colcount));
                            FpSpread.Columns[colcount].Locked = true;
                        }
                    }
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - colnew].Text = "Allowances";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - colnew].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - colnew].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - colnew].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - colnew].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - colnew, 1, colnew);
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - colnew].Locked = true;
                }
                if (chk_dedvis.Checked == true && txt_deduct.Text.Trim() != "--Select--")
                {
                    for (int cb = 0; cb < cbl_deduction.Items.Count; cb++)
                    {
                        if (cbl_deduction.Items[cb].Selected == true)
                        {
                            FpSpread.Sheets[0].ColumnCount++;
                            coldedcount = Convert.ToInt32(FpSpread.Sheets[0].ColumnCount - 1);
                            for (int dar = 0; dar < arrded.Count; dar++)
                            {
                                if (dar != 0)
                                    FpSpread.Sheets[0].ColumnCount++;
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(arrded[dar]);
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread.Sheets[0].ColumnHeader.Cells[2, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(2, FpSpread.Sheets[0].ColumnCount - 1, 1, 1);
                                FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                                coldednew++;
                            }
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, coldedcount].Text = Convert.ToString(cbl_deduction.Items[cb].Text);
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, coldedcount].Font.Bold = true;
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, coldedcount].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, coldedcount].Font.Name = "Book Antiqua";
                            FpSpread.Sheets[0].ColumnHeader.Cells[1, coldedcount].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(1, coldedcount, 1, 17);
                            if (!dedcol.ContainsKey(Convert.ToString(cbl_deduction.Items[cb].Text)))
                                dedcol.Add(Convert.ToString(cbl_deduction.Items[cb].Text), Convert.ToString(coldedcount));
                            FpSpread.Columns[coldedcount].Locked = true;
                        }
                    }
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - coldednew].Text = "Deductions";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - coldednew].Font.Bold = true;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - coldednew].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - coldednew].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - coldednew].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - coldednew, 1, coldednew);
                    FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - coldednew].Locked = true;
                }
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkall;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    allownew = "";
                    dednew = "";
                    staffcodespr = "";
                    string frmgross = "";
                    string basic = "";
                    int col = 1;
                    double allamnt = 0;
                    double dedamnt = 0;
                    double allnewamnt = 0;
                    double dednewamnt = 0;
                    double basamnt = 0;
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].CellType = txtcell;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["category_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[i]["category_code"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["stftype"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["grade_pay"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["bsalary"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["pay_band"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[i]["stfstatus"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                    staffcodespr = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                    allownew = Convert.ToString(ds.Tables[0].Rows[i]["allowances"]);
                    dednew = Convert.ToString(ds.Tables[0].Rows[i]["deductions"]);
                    basic = Convert.ToString(ds.Tables[0].Rows[i]["bsalary"]);
                    if (chk_allvis.Checked == true && txt_allow.Text.Trim() != "--Select--")
                    {
                        for (int k = 0; k < cbl_allow.Items.Count; k++)
                        {
                            if (cbl_allow.Items[k].Selected == true)
                            {
                                bool entryflag = false;
                                col = Convert.ToInt32(allcol[Convert.ToString(cbl_allow.Items[k].Text)]);
                                if (allownew.Trim() != "" && allownew.Trim() != "0")
                                {
                                    splallnew = allownew.Split('\\');
                                    if (splallnew.Length > 0)
                                    {
                                        for (int all = 0; all < splallnew.Length; all++)
                                        {
                                            if (entryflag == false)
                                            {
                                                splallamnt = splallnew[all].Split(';');
                                                if (splallamnt.Length >= 8)
                                                {
                                                    if (splallamnt[0].Trim() != "" && splallamnt[0].Trim() != "0")
                                                    {
                                                        if (splallamnt[0].Trim() == cbl_allow.Items[k].Text)
                                                        {
                                                            entryflag = true;
                                                            if (splallamnt[1].Trim() != "" && splallamnt[1].Trim() != "0")
                                                            {
                                                                if (splallamnt[2].Trim() != "")
                                                                {
                                                                    col++;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString(splallamnt[1].Trim());
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (splallamnt[1].Trim() == "Amount" || splallamnt[1].Trim() == "Slab")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString(splallamnt[2].Trim());
                                                                    }
                                                                    else if (splallamnt[1].Trim() == "Percent")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString(splallamnt[2].Trim()) + "%";
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    //col++;
                                                                    //if (Convert.ToString(splallamnt[3].Trim()) == "1")
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = "Yes";
                                                                    //}
                                                                    //else if (Convert.ToString(splallamnt[3].Trim()) == "0")
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = "No";
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = "";
                                                                    //}
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (splallamnt[4].Trim() == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("Yes");
                                                                    }
                                                                    else if (splallamnt[4].Trim() == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("No");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("");
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    //col++;
                                                                    //if (splallamnt[5].Trim() == "1")
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("Yes");
                                                                    //}
                                                                    //else if (splallamnt[5].Trim() == "0")
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("No");
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("");
                                                                    //}
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (splallamnt[6].Trim() == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("Yes");
                                                                    }
                                                                    else if (splallamnt[6].Trim() == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("No");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("");
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    if (splallamnt.Length >= 9)
                                                                    {
                                                                        col++;
                                                                        if (splallamnt[8].Trim() == "1")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("Yes");
                                                                        }
                                                                        else if (splallamnt[8].Trim() == "0")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("No");
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("");
                                                                        }
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    if (splallamnt.Length >= 10)
                                                                    {
                                                                        col++;
                                                                        if (splallamnt[9].Trim() == "1")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("Yes");
                                                                        }
                                                                        else if (splallamnt[9].Trim() == "0")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("No");
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("");
                                                                        }
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    if (splallamnt.Length >= 11)
                                                                    {
                                                                        col++;
                                                                        if (splallamnt[10].Trim() == "1")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("Yes");
                                                                        }
                                                                        else if (splallamnt[10].Trim() == "0")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("No");
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString("");
                                                                        }
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    if (splallamnt.Length >= 12)
                                                                    {
                                                                        col++;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Text = Convert.ToString(splallamnt[11]);
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (chk_dedvis.Checked == true && txt_deduct.Text.Trim() != "--Select--")
                    {
                        for (int k = 0; k < cbl_deduction.Items.Count; k++)
                        {
                            if (cbl_deduction.Items[k].Selected == true)
                            {
                                bool dedentry = false;
                                col = Convert.ToInt32(dedcol[Convert.ToString(cbl_deduction.Items[k].Text)]);
                                if (dednew.Trim() != "" && dednew.Trim() != "0")
                                {
                                    spldednew = dednew.Split('\\');
                                    if (spldednew.Length > 0)
                                    {
                                        for (int all = 0; all < spldednew.Length; all++)
                                        {
                                            if (dedentry == false)
                                            {
                                                spldedamnt = spldednew[all].Split(';');
                                                if (spldedamnt.Length >= 15)
                                                {
                                                    if (spldedamnt[0].Trim() != "" && spldedamnt[0].Trim() != "0")
                                                    {
                                                        if (spldedamnt[0].Trim() == cbl_deduction.Items[k].Text)
                                                        {
                                                            dedentry = true;
                                                            if (spldedamnt[1].Trim() != "" && spldedamnt[1].Trim() != "0")
                                                            {
                                                                if (spldedamnt[2].Trim() != "")
                                                                {
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString(spldedamnt[1].Trim());
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[1].Trim() == "Amount" || spldedamnt[1].Trim() == "Slab")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString(spldedamnt[2].Trim());
                                                                    }
                                                                    else if (spldedamnt[1].Trim() == "Percent")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString(spldedamnt[2].Trim()) + "%";
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (Convert.ToString(spldedamnt[11].Trim()) != "")
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString(spldedamnt[11]);
                                                                    else
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = "";
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (Convert.ToString(spldedamnt[3].Trim()) == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = "Yes";
                                                                    }
                                                                    else if (Convert.ToString(spldedamnt[3].Trim()) == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = "No";
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = "";
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[4].Trim() == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                    }
                                                                    else if (spldedamnt[4].Trim() == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    //col++;
                                                                    //if (spldedamnt[5].Trim() == "1")
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                    //}
                                                                    //else if (spldedamnt[5].Trim() == "0")
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    //}
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[6].Trim() == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                    }
                                                                    else if (spldedamnt[6].Trim() == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[7].Trim() == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                    }
                                                                    else if (spldedamnt[7].Trim() == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[8].Trim() == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                    }
                                                                    else if (spldedamnt[8].Trim() == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[9].Trim() == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                    }
                                                                    else if (spldedamnt[9].Trim() == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[10].Trim() == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                    }
                                                                    else if (spldedamnt[10].Trim() == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    //col++;
                                                                    //if (spldedamnt[11].Trim() == "1")
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                    //}
                                                                    //else if (spldedamnt[11].Trim() == "0")
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    //}
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    //FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[12].Trim() == "1")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                    }
                                                                    else if (spldedamnt[12].Trim() == "0")
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    }
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[13].Trim() != "")
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString(spldedamnt[13]);
                                                                    else
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    col++;
                                                                    if (spldedamnt[14].Trim() != "")
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString(spldedamnt[14]);
                                                                    else
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    if (spldedamnt.Length >= 16)
                                                                    {
                                                                        col++;
                                                                        if (spldedamnt[15].Trim() == "1")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                        }
                                                                        else if (spldedamnt[15].Trim() == "0")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                        }
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    }
                                                                    if (spldedamnt.Length >= 17)
                                                                    {
                                                                        col++;
                                                                        if (spldedamnt[16].Trim() == "1")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                        }
                                                                        else if (spldedamnt[16].Trim() == "0")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                        }
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    }
                                                                    if (spldedamnt.Length >= 18)
                                                                    {
                                                                        col++;
                                                                        if (spldedamnt[17].Trim() != "")
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString(spldedamnt[17]);
                                                                        else
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    }
                                                                    if (spldedamnt.Length >= 20)
                                                                    {
                                                                        col++;
                                                                        if (spldedamnt[19].Trim() == "1")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("Yes");
                                                                        }
                                                                        else if (spldedamnt[19].Trim() == "0")
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("No");
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Text = Convert.ToString("");
                                                                        }
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                                        FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                FpSpread.Visible = true;
                lbl_alert.Visible = false;
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                FpSpread.SaveChanges();
                rprint.Visible = true;
            }
            else
            {
                FpSpread.Visible = false;
                rprint.Visible = false;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Records Found!";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, college, "GradePayMaster.aspx");
        }
    }
    protected void grdall_rowbound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
            e.Row.Cells[8].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
            e.Row.Cells[9].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grd_all, "index$" + e.Row.RowIndex);
        }
    }
    protected void grdall_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            lbl_allowalert.Visible = false;
            string lblallow = "";
            string alltype = "";
            string mode = "";
            string value = "";
            string inclop = "";
            string frmbasic = "";
            string frmbasgp = "";
            string issplall = "";
            string frmbasicagp = "";
            string round = "";
            string fromallallow = "";
            for (int rem = 0; rem < grd_all.Rows.Count; rem++)
            {
                grd_all.Rows[rem].BackColor = Color.White;
            }
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "index")
            {
                grd_all.Visible = false;
                divgrdall.Visible = false;
                allow_div.Visible = true;
                btn_allowsave.Visible = false;
                btn_allowupdate.Visible = true;
                btn_allowdelete.Visible = true;
                int chkcount = 0;
                alltype = (grd_all.Rows[row].FindControl("lbl_alltype") as Label).Text;
                mode = (grd_all.Rows[row].FindControl("lbl_mode") as Label).Text;
                if (mode == "Amount")
                {
                    ddl_mode.SelectedIndex = 0;
                    chkalldisable();
                    txt_val.Enabled = true;
                }
                else if (mode == "Percent")
                {
                    ddl_mode.SelectedIndex = 1;
                    chkallenable();
                    txt_val.Enabled = true;
                }
                else if (mode == "Slab")
                {
                    ddl_mode.SelectedIndex = 2;
                    chkallenable();
                    txt_val.Enabled = false;
                }
                value = (grd_all.Rows[row].FindControl("lbl_val") as Label).Text;
                if (value.Trim() != "" && value.Trim() != "0.00" && value.Trim() != "0")
                    txt_val.Text = Convert.ToString(value);
                else
                    txt_val.Text = "";
                inclop = (grd_all.Rows[row].FindControl("lbl_lop") as Label).Text;
                if (inclop.Trim() == "Yes")
                    cb_lop.Checked = true;
                else
                    cb_lop.Checked = false;
                frmbasic = (grd_all.Rows[row].FindControl("lbl_frmbasic") as Label).Text;
                if (frmbasic.Trim() == "Yes")
                    cb_fbasic.Checked = true;
                else
                    cb_fbasic.Checked = false;
                frmbasgp = (grd_all.Rows[row].FindControl("lbl_frmbasgp") as Label).Text;
                if (frmbasgp.Trim() == "Yes")
                    cb_fbgp.Checked = true;
                else
                    cb_fbgp.Checked = false;
                issplall = (grd_all.Rows[row].FindControl("lbl_isspl") as Label).Text;
                if (issplall.Trim() == "Yes")
                    cb_special.Checked = true;
                else
                    cb_special.Checked = false;
                frmbasicagp = (grd_all.Rows[row].FindControl("lbl_frmbasagp") as Label).Text;
                if (frmbasicagp.Trim() == "Yes")
                    cb_agp.Checked = true;
                else
                    cb_agp.Checked = false;
                round = (grd_all.Rows[row].FindControl("lbl_roundtype") as Label).Text;

                fromallallow = (grd_all.Rows[row].FindControl("lbl_fromallallow") as Label).Text;
                if (fromallallow != "")//delsi03ref
                {
                    cb_fromallallow.Checked = true;
                    txt_all_allowVal.Text = Convert.ToString(fromallallow);
                    txt_all_allowVal.Enabled = false;

                }
                else
                {
                    txt_all_allowVal.Text = "";

                }
                if (round.Trim() != "")
                    ddl_round.SelectedIndex = ddl_round.Items.IndexOf(ddl_round.Items.FindByText(round));
                else
                    ddl_round.SelectedIndex = 0;
                for (int ik = 0; ik < cbl_popallowance.Items.Count; ik++)
                {
                    if (alltype.Trim() == cbl_popallowance.Items[ik].Text)
                    {
                        cbl_popallowance.Items[ik].Selected = true;
                        lblallow = cbl_popallowance.Items[ik].Text;
                        chkcount++;
                    }
                    else
                    {
                        cbl_popallowance.Items[ik].Selected = false;
                    }
                }
                txt_popallow.Text = "Allowance(" + Convert.ToString(chkcount) + ")";
                cb_popallowance.Checked = false;
                lbl_header1.Text = "Allowances -" + " " + lblallow;
                grd_all.Rows[row].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }
    public void btn_allowexit_Click(object sender, EventArgs e)
    {
        allow_div.Visible = false;
        divgrdall.Visible = true;
        grd_all.Visible = true;
        int chkcount = 0;
        for (int chk = 0; chk < cbl_popallowance.Items.Count; chk++)
        {
            cbl_popallowance.Items[chk].Selected = true;
            chkcount++;
        }
        txt_popallow.Text = "Allowance(" + Convert.ToString(chkcount) + ")";
        cb_popallowance.Checked = true;
    }
    public void btn_allowsave_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_allowalert.Visible = false;
            Session["alltype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (cb_popallow.Checked == true)
            {
                string overallall = "";
                if (txt_val.Text != "" || ddl_mode.SelectedItem.Text == "Slab")
                {
                    overallall = getoverallallow();
                    string newcolvalue = "";
                    if (Session["alltype"] == null)
                    {
                        if (cbl_popallowance.Items.Count > 0)
                            newcolvalue = GetSelectedItemsTextnew(cbl_popallowance);
                        Session["alltype"] = newcolvalue;
                    }
                    divgrdall.Visible = true;
                    grd_all.Visible = true;
                    allow_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtallheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = overallall.Split('\\');
                    string colvalue = "";
                    string[] splcol = new string[15];
                    colvalue = Convert.ToString(Session["alltype"]);
                    if (colvalue.Trim() != "")
                        splcol = colvalue.Split(',');
                    if (Session["alltype"] == null)
                    {
                        lbl_allowalert.Visible = true;
                        lbl_allowalert.Text = "Please Select Any Allowance!";
                        divgrdall.Visible = false;
                        grd_all.Visible = false;
                        allow_div.Visible = true;
                        return;
                    }
                    else
                    {
                        if (Session["dt"] != null)
                        {
                            DataRow dr;
                            DataTable dnew = new DataTable();
                            dnew = (DataTable)Session["dt"];
                            if (dnew.Rows.Count > 0)
                            {
                                for (int ro = 0; ro < dnew.Rows.Count; ro++)
                                {
                                    dr = dt.NewRow();
                                    for (int col = 0; col < dnew.Columns.Count; col++)
                                    {
                                        dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                            if (splcol.Length > 0)
                            {
                                for (int l = 0; l < splcol.Length; l++)
                                {
                                    for (int k = 0; k < dt.Rows.Count; k++)
                                    {
                                        if (Convert.ToString(dt.Rows[k][0]) == Convert.ToString(splcol[l]))
                                        {
                                            dt.Rows.Remove(dt.Rows[k]);
                                        }
                                    }
                                }
                            }
                            dt = getallval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                            Session["dt"] = dt;
                        }
                        else
                        {
                            DataRow dr;
                            dt = getallval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                            Session["dt"] = null;
                            Session["dt"] = dt;
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grd_all.DataSource = dt;
                        grd_all.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grd_all.DataBind();
                        for (int i = 0; i < grd_all.Columns.Count; i++)
                        {
                            grd_all.Columns[i].HeaderStyle.Width = 100;
                            grd_all.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grd_all.DataSource = dt;
                        grd_all.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            if (savecount > 0)
            {
                lbl_allowalert.Visible = true;
                lbl_allowalert.Text = "Added Successfully!";
                allow_div.Visible = true;
                divgrdall.Visible = false;
                grd_all.Visible = false;
            }
            if (errcount > 0)
            {
                lbl_allowalert.Visible = true;
                lbl_allowalert.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btn_allowupdate_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_allowalert.Visible = false;
            Session["alltype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (cb_popallow.Checked == true)
            {
                string overallall = "";
                if (txt_val.Text != "" || ddl_mode.SelectedItem.Text == "Slab")
                {
                    overallall = getoverallallow();
                    divgrdall.Visible = true;
                    grd_all.Visible = true;
                    allow_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtallheader(dt);
                    string newcolvalue = "";
                    if (Session["alltype"] == null)
                    {
                        if (cbl_popallowance.Items.Count > 0)
                            newcolvalue = GetSelectedItemsTextnew(cbl_popallowance);
                        Session["alltype"] = newcolvalue;
                    }
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = overallall.Split('\\');
                    string colvalue = "";
                    string[] splcol = new string[15];
                    colvalue = Convert.ToString(Session["alltype"]);
                    if (colvalue.Trim() != "")
                        splcol = colvalue.Split(',');
                    if (Session["alltype"] == null)
                    {
                        lbl_allowalert.Visible = true;
                        lbl_allowalert.Text = "Please Select Any Allowance!";
                        divgrdall.Visible = false;
                        grd_all.Visible = false;
                        allow_div.Visible = true;
                        return;
                    }
                    else
                    {
                        if (Session["dt"] != null)
                        {
                            DataRow dr;
                            DataTable dnew = new DataTable();
                            dnew = (DataTable)Session["dt"];
                            if (dnew.Rows.Count > 0)
                            {
                                for (int ro = 0; ro < dnew.Rows.Count; ro++)
                                {
                                    dr = dt.NewRow();
                                    for (int col = 0; col < dnew.Columns.Count; col++)
                                    {
                                        dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                            if (splcol.Length > 0)
                            {
                                for (int l = 0; l < splcol.Length; l++)
                                {
                                    for (int k = 0; k < dt.Rows.Count; k++)
                                    {
                                        if (Convert.ToString(dt.Rows[k][0]) == Convert.ToString(splcol[l]))
                                        {
                                            dt.Rows.Remove(dt.Rows[k]);
                                        }
                                    }
                                }
                            }
                            dt = getallval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                            Session["dt"] = dt;
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grd_all.DataSource = dt;
                        grd_all.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grd_all.DataBind();
                        for (int i = 0; i < grd_all.Columns.Count; i++)
                        {
                            grd_all.Columns[i].HeaderStyle.Width = 100;
                            grd_all.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grd_all.DataSource = dt;
                        grd_all.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            if (savecount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = " Updated Successfully! ";
                allow_div.Visible = false;
                divgrdall.Visible = true;
                grd_all.Visible = true;
            }
            if (errcount > 0)
            {
                lbl_allowalert.Visible = true;
                lbl_allowalert.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btn_allowdelete_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_allowalert.Visible = false;
            Session["alltype"] = null;
            int delcount = 0;
            if (cb_popallow.Checked == true)
            {
                divgrdall.Visible = true;
                grd_all.Visible = true;
                allow_div.Visible = false;
                DataTable dt = new DataTable();
                dtallheader(dt);
                string newcolvalue = "";
                if (Session["alltype"] == null)
                {
                    if (cbl_popallowance.Items.Count > 0)
                        newcolvalue = GetSelectedItemsTextnew(cbl_popallowance);
                    Session["alltype"] = newcolvalue;
                }
                string colvalue = "";
                string[] splcol = new string[15];
                colvalue = Convert.ToString(Session["alltype"]);
                if (colvalue.Trim() != "")
                    splcol = colvalue.Split(',');
                if (Session["alltype"] == null)
                {
                    lbl_allowalert.Visible = true;
                    lbl_allowalert.Text = "Please Select any Allowance!";
                    divgrdall.Visible = false;
                    grd_all.Visible = false;
                    allow_div.Visible = true;
                    return;
                }
                else
                {
                    if (Session["dt"] != null)
                    {
                        DataRow dr;
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["dt"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                for (int col = 0; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        if (splcol.Length > 0)
                        {
                            for (int l = 0; l < splcol.Length; l++)
                            {
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    if (Convert.ToString(dt.Rows[k][0]) == Convert.ToString(splcol[l]))
                                    {
                                        dt.Rows.Remove(dt.Rows[k]);
                                        delcount++;
                                    }
                                }
                            }
                        }
                        Session["dt"] = dt;
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    grd_all.DataSource = dt;
                    grd_all.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grd_all.DataBind();
                    for (int i = 0; i < grd_all.Columns.Count; i++)
                    {
                        grd_all.Columns[i].HeaderStyle.Width = 100;
                        grd_all.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grd_all.DataSource = dt;
                    grd_all.DataBind();
                }
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully!";
                divgrdall.Visible = true;
                grd_all.Visible = true;
                allow_div.Visible = false;
            }
        }
        catch { }
    }
    public void bindgridall()
    {
        Session["dt"] = null;
        try
        {
            string staffcode = "";
            string allowance = "";
            if (checkedOK())
            {
                for (int sco = 0; sco < FpSpread.Sheets[0].RowCount; sco++)
                {
                    byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (check == 1)
                    {
                        staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[sco, 2].Text);
                        divgrdall.Visible = true;
                        grd_all.Visible = true;
                        allow_div.Visible = false;
                        DataTable dt = new DataTable();
                        dtallheader(dt);
                        string selq = "select st.staff_code,st.allowances,s.staff_name from stafftrans st,staffmaster s where st.staff_code=s.staff_code and st.staff_code='" + staffcode + "' and latestrec = 1 and college_code='" + clgcode + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    allowance = ds.Tables[0].Rows[0]["allowances"].ToString().Trim();
                                    string[] allowanmce_arr1;
                                    string alowancesplit;
                                    allowanmce_arr1 = allowance.Split('\\');
                                    for (int i = 0; i < allowanmce_arr1.Length; i++)
                                    {
                                        DataRow dr;
                                        alowancesplit = allowanmce_arr1[i];
                                        if (alowancesplit.Trim() != "")
                                        {
                                            string[] allowanceda;
                                            allowanceda = alowancesplit.Split(';');
                                            if (allowanceda[2].Trim() != "")
                                            {
                                                for (int ik = 0; ik < cbl_popallowance.Items.Count; ik++)
                                                {
                                                    if (cbl_popallowance.Items[ik].Selected == true)
                                                    {
                                                        if (allowanceda[0] == cbl_popallowance.Items[ik].Text)
                                                        {
                                                            dr = dt.NewRow();
                                                            dr["alltype"] = Convert.ToString(allowanceda[0]);
                                                            dr["mode"] = Convert.ToString(allowanceda[1]);
                                                            if (allowanceda[1] == "Amount")
                                                                dr["value"] = Convert.ToString(allowanceda[2]);
                                                            if (allowanceda[1] == "Percent")
                                                                dr["value"] = Convert.ToString(allowanceda[2]) + "%";
                                                            if (allowanceda[1] == "Slab")
                                                                dr["value"] = Convert.ToString(allowanceda[2]);
                                                            if (Convert.ToString(allowanceda[4]) == "1")
                                                                dr["inclop"] = "Yes";
                                                            else
                                                                dr["inclop"] = "No";
                                                            if (Convert.ToString(allowanceda[6]) == "1")
                                                                dr["frmbasic"] = "Yes";
                                                            else
                                                                dr["frmbasic"] = "No";
                                                            if (allowanceda.Length >= 9)
                                                            {
                                                                if (Convert.ToString(allowanceda[8]) == "1")
                                                                    dr["frmbasgp"] = "Yes";
                                                                else
                                                                    dr["frmbasgp"] = "No";
                                                            }
                                                            if (allowanceda.Length >= 10)
                                                            {
                                                                if (Convert.ToString(allowanceda[9]) == "1")
                                                                    dr["isspl"] = "Yes";
                                                                else
                                                                    dr["isspl"] = "No";
                                                            }
                                                            if (allowanceda.Length >= 11)
                                                            {
                                                                if (Convert.ToString(allowanceda[10]) == "1")
                                                                    dr["frmbasagp"] = "Yes";
                                                                else
                                                                    dr["frmbasagp"] = "No";
                                                            }
                                                            if (allowanceda.Length >= 12)
                                                                dr["roundval"] = Convert.ToString(allowanceda[11]);

                                                            if (allowanceda.Length > 13)//delsi0405
                                                            {
                                                                dr["FromAllow"] = Convert.ToString(allowanceda[13]);
                                                            }
                                                            dt.Rows.Add(dr);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        Session["dt"] = dt;
                                    }
                                }
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            grd_all.DataSource = dt;
                            grd_all.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            grd_all.DataBind();
                            for (int i = 0; i < grd_all.Columns.Count; i++)
                            {
                                grd_all.Columns[i].HeaderStyle.Width = 100;
                                grd_all.Columns[i].ItemStyle.Width = 100;
                            }
                        }
                        else
                        {
                            grd_all.DataSource = dt;
                            grd_all.DataBind();
                        }
                    }
                }
            }
        }
        catch { }
    }
    public void allclear()
    {
        ddl_mode.SelectedIndex = 0;
        txt_val.Text = "";
        ddl_round.SelectedIndex = 0;
        chkalldisable();
    }
    protected void grid_ded_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[8].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[9].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[10].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[11].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[12].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[13].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[14].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[15].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
                e.Row.Cells[16].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grid_ded, "index$" + e.Row.RowIndex);
            }
        }
        catch { }
    }
    protected void grid_ded_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            lbl_dedalert.Visible = false;
            string lblded = "";
            string dedtype = "";
            string mode = "";
            string value = "";
            string frmgross = "";
            string frmbasda = "";
            string inclop = "";
            string frmbasgpda = "";
            string frmbasic = "";
            string frmbasdp = "";
            string frmpetty = "";
            string frmbasarr = "";
            string ismaxcal = "";
            string maxamnt = "";
            string dedamnt = "";
            string frmbasarrsa = "";
            string frmall = "";
            string roundval = "";
            string frmnet = "";
            for (int rem = 0; rem < grid_ded.Rows.Count; rem++)
            {
                grid_ded.Rows[rem].BackColor = Color.White;
            }
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "index")
            {
                grid_ded.Visible = false;
                divgrdded.Visible = false;
                deduct_div.Visible = true;
                btn_deductsave.Visible = false;
                btn_deductupdate.Visible = true;
                btn_deductdelete.Visible = true;
                int chkcount = 0;
                dedtype = (grid_ded.Rows[row].FindControl("lbl_deducttype") as Label).Text;
                mode = (grid_ded.Rows[row].FindControl("lbl_dedmode") as Label).Text;
                if (mode.Trim() == "Amount")
                {
                    ddl_dmode.SelectedIndex = 0;
                    chkdeddisable();
                    txt_dval.Enabled = true;
                }
                else if (mode.Trim() == "Percent")
                {
                    ddl_dmode.SelectedIndex = 1;
                    chkdedenable();
                    txt_dval.Enabled = true;
                }
                else
                {
                    ddl_dmode.SelectedIndex = 2;
                    chkdedenable();
                    txt_dval.Enabled = false;
                }
                value = (grid_ded.Rows[row].FindControl("lbl_dedval") as Label).Text;
                if (value.Trim() != "" && value.Trim() != "0" && value.Trim() != "0.00" && value.Trim() != "0.0000")
                    txt_dval.Text = Convert.ToString(value);
                else
                    txt_dval.Text = "";
                roundval = (grid_ded.Rows[row].FindControl("lbl_rounddedroundtype") as Label).Text;
                if (roundval.Trim() != "" && roundval.Trim() != "0")
                    ddl_rt.SelectedIndex = ddl_rt.Items.IndexOf(ddl_rt.Items.FindByText(roundval));
                else
                    ddl_rt.SelectedIndex = 0;
                frmgross = (grid_ded.Rows[row].FindControl("lbl_dedfrmgross") as Label).Text;
                if (frmgross.Trim() == "Yes")
                    cb_fg.Checked = true;
                else
                    cb_fg.Checked = false;
                frmbasda = (grid_ded.Rows[row].FindControl("lbl_frmbasicda") as Label).Text;
                if (frmbasda.Trim() == "Yes")
                    cb_fbda.Checked = true;
                else
                    cb_fbda.Checked = false;
                inclop = (grid_ded.Rows[row].FindControl("lbl_dedfrmlop") as Label).Text;
                if (inclop.Trim() == "Yes")
                    cb_ilop.Checked = true;
                else
                    cb_ilop.Checked = false;
                frmbasgpda = (grid_ded.Rows[row].FindControl("lbl_dedgpda") as Label).Text;
                if (frmbasgpda.Trim() == "Yes")
                    cb_fbgpda.Checked = true;
                else
                    cb_fbgpda.Checked = false;
                frmbasic = (grid_ded.Rows[row].FindControl("lbl_dedfrmbas") as Label).Text;
                if (frmbasic.Trim() == "Yes")
                    cb_fb.Checked = true;
                else
                    cb_fb.Checked = false;
                frmbasdp = (grid_ded.Rows[row].FindControl("lbl_dedfrmbasdp") as Label).Text;
                if (frmbasdp.Trim() == "Yes")
                    cb_fbdp.Checked = true;
                else
                    cb_fbdp.Checked = false;
                frmpetty = (grid_ded.Rows[row].FindControl("lbl_dedfrmpetty") as Label).Text;
                if (frmpetty.Trim() == "Yes")
                    cb_fp.Checked = true;
                else
                    cb_fp.Checked = false;
                frmbasarr = (grid_ded.Rows[row].FindControl("lbl_dedfrmbasarr") as Label).Text;
                if (frmbasarr.Trim() == "Yes")
                    cb_fbarr.Checked = true;
                else
                    cb_fbarr.Checked = false;
                ismaxcal = (grid_ded.Rows[row].FindControl("lbl_dedismaxcal") as Label).Text;
                if (ismaxcal.Trim() == "Yes")
                    cb_mcal.Checked = true;
                else
                    cb_mcal.Checked = false;
                maxamnt = (grid_ded.Rows[row].FindControl("lbl_maxamt") as Label).Text;
                if (maxamnt.Trim() != "" && maxamnt.Trim() != "0" && maxamnt.Trim() != "0.00" && maxamnt.Trim() != "0.0000")
                    txt_mamt.Text = Convert.ToString(maxamnt);
                else
                    txt_mamt.Text = "";
                dedamnt = (grid_ded.Rows[row].FindControl("lbl_dedamt") as Label).Text;
                if (dedamnt.Trim() != "" && dedamnt.Trim() != "0" && dedamnt.Trim() != "0.00" && dedamnt.Trim() != "0.0000")
                    txt_damt.Text = Convert.ToString(dedamnt);
                else
                    txt_damt.Text = "";
                frmbasarrsa = (grid_ded.Rows[row].FindControl("lbl_dedfrmbasarrsa") as Label).Text;
                if (frmbasarrsa.Trim() == "Yes")
                    cb_fbas.Checked = true;
                else
                    cb_fbas.Checked = false;
                frmall = (grid_ded.Rows[row].FindControl("lbl_dedfrmallow") as Label).Text;
                if (frmall.Trim() != "")
                {
                    cb_fallow.Checked = true;
                    radBtn_grosswithlop.Visible = true;
                    radBtn_grosswithlop.Checked = true;
                    txtcomded.Text = frmall;
                }
                else
                {
                    cb_fallow.Checked = false;
                    txtcomded.Text = "";
                }
                frmnet = (grid_ded.Rows[row].FindControl("lbl_frmnetamnt") as Label).Text;
                if (frmnet.Trim() == "Yes")
                    rb_frmnet.Checked = true;
                else
                    rb_frmnet.Checked = false;
                for (int ik = 0; ik < cbl_popdd.Items.Count; ik++)
                {
                    if (dedtype.Trim() == cbl_popdd.Items[ik].Text)
                    {
                        cbl_popdd.Items[ik].Selected = true;
                        lblded = cbl_popdd.Items[ik].Text;
                        chkcount++;
                    }
                    else
                    {
                        cbl_popdd.Items[ik].Selected = false;
                    }
                }
                txt_popdeduct.Text = "Deduction(" + Convert.ToString(chkcount) + ")";
                cb_popdd.Checked = false;
                lbl_h2.Text = "Deductions -" + " " + lblded;
                grid_ded.Rows[row].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }
    public void btn_deductexit_Click(object sender, EventArgs e)
    {
        int chkcount = 0;
        deduct_div.Visible = false;
        divgrdded.Visible = true;
        grid_ded.Visible = true;
        for (int chk = 0; chk < cbl_popdd.Items.Count; chk++)
        {
            cbl_popdd.Items[chk].Selected = true;
            chkcount++;
        }
        txt_popdeduct.Text = "Deduction(" + Convert.ToString(chkcount) + ")";
    }
    public void btn_deductsave_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_dedalert.Visible = false;
            Session["dedtype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (cb_popdeduct.Checked == true)
            {
                string overallded = "";
                if (cb_mcal.Checked == true && txt_mamt.Text.Trim() == "")
                {
                    lbl_dedalert.Visible = true;
                    lbl_dedalert.Text = " Please Enter Max Amount! ";
                    deduct_div.Visible = true;
                    divgrdded.Visible = false;
                    grid_ded.Visible = false;
                    return;
                }
                if (txt_dval.Text != "" || ddl_dmode.SelectedItem.Text == "Slab")
                {
                    overallded = getoverallded();
                    string newcol = "";
                    if (Session["dedtype"] == null)
                    {
                        if (cbl_popdd.Items.Count > 0)
                        {
                            newcol = GetSelectedItemsTextnew(cbl_popdd);
                            Session["dedtype"] = newcol;
                        }
                    }
                    string colvalue = "";
                    string[] splcol = new string[20];
                    colvalue = Convert.ToString(Session["dedtype"]);
                    if (colvalue.Trim() != "")
                        splcol = colvalue.Split(',');
                    divgrdded.Visible = true;
                    grid_ded.Visible = true;
                    deduct_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtdedheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = ""; ;
                    allowanmce_arr1 = overallded.Split('\\');
                    if (Session["dedtype"] == null)
                    {
                        lbl_dedalert.Visible = true;
                        lbl_dedalert.Text = "Please Select Any Deduction!";
                        divgrdded.Visible = false;
                        grid_ded.Visible = false;
                        deduct_div.Visible = true;
                        return;
                    }
                    else
                    {
                        if (Session["dtded"] != null)
                        {
                            DataRow dr;
                            DataTable dnew = new DataTable();
                            dnew = (DataTable)Session["dtded"];
                            if (dnew.Rows.Count > 0)
                            {
                                for (int ro = 0; ro < dnew.Rows.Count; ro++)
                                {
                                    dr = dt.NewRow();
                                    for (int col = 0; col < dnew.Columns.Count; col++)
                                    {
                                        dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                            if (splcol.Length > 0)
                            {
                                for (int l = 0; l < splcol.Length; l++)
                                {
                                    for (int k = 0; k < dt.Rows.Count; k++)
                                    {
                                        if (Convert.ToString(dt.Rows[k][0]) == Convert.ToString(splcol[l]))
                                        {
                                            dt.Rows.Remove(dt.Rows[k]);
                                        }
                                    }
                                }
                            }
                            dt = getdedval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                            Session["dtded"] = dt;
                        }
                        else
                        {
                            DataRow dr;
                            dt = getdedval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                            Session["dtded"] = null;
                            Session["dtded"] = dt;
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grid_ded.DataSource = dt;
                        grid_ded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grid_ded.DataBind();
                        for (int i = 0; i < grid_ded.Columns.Count; i++)
                        {
                            grid_ded.Columns[i].HeaderStyle.Width = 100;
                            grid_ded.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grid_ded.DataSource = dt;
                        grid_ded.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            if (savecount > 0)
            {
                lbl_dedalert.Visible = true;
                lbl_dedalert.Text = " Added Successfully! ";
                deduct_div.Visible = true;
                divgrdded.Visible = false;
                grid_ded.Visible = false;
            }
            if (errcount > 0)
            {
                lbl_dedalert.Visible = true;
                lbl_dedalert.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btn_deductupdate_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_dedalert.Visible = false;
            Session["dedtype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (cb_popdeduct.Checked == true)
            {
                string overallded = "";
                if (cb_mcal.Checked == true && txt_mamt.Text.Trim() == "")
                {
                    lbl_dedalert.Visible = true;
                    lbl_dedalert.Text = " Please Enter Max Amount! ";
                    deduct_div.Visible = true;
                    divgrdded.Visible = false;
                    grid_ded.Visible = false;
                    return;
                }
                if (txt_dval.Text != "" || ddl_dmode.SelectedItem.Text == "Slab")
                {
                    overallded = getoverallded();
                    string newcol = "";
                    if (Session["dedtype"] == null)
                    {
                        if (cbl_popdd.Items.Count > 0)
                        {
                            newcol = GetSelectedItemsTextnew(cbl_popdd);
                            Session["dedtype"] = newcol;
                        }
                    }
                    string colvalue = "";
                    string[] splcol = new string[20];
                    colvalue = Convert.ToString(Session["dedtype"]);
                    if (colvalue.Trim() != "")
                        splcol = colvalue.Split(',');
                    divgrdded.Visible = true;
                    grid_ded.Visible = true;
                    deduct_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtdedheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = overallded.Split('\\');
                    if (Session["dedtype"] == null)
                    {
                        lbl_dedalert.Visible = true;
                        lbl_dedalert.Text = "Please Select Any Deduction!";
                        divgrdded.Visible = false;
                        grid_ded.Visible = false;
                        deduct_div.Visible = true;
                        return;
                    }
                    else
                    {
                        if (Session["dtded"] != null)
                        {
                            DataRow dr;
                            DataTable dnew = new DataTable();
                            dnew = (DataTable)Session["dtded"];
                            if (dnew.Rows.Count > 0)
                            {
                                for (int ro = 0; ro < dnew.Rows.Count; ro++)
                                {
                                    dr = dt.NewRow();
                                    for (int col = 0; col < dnew.Columns.Count; col++)
                                    {
                                        dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                            if (splcol.Length > 0)
                            {
                                for (int l = 0; l < splcol.Length; l++)
                                {
                                    for (int k = 0; k < dt.Rows.Count; k++)
                                    {
                                        if (Convert.ToString(dt.Rows[k][0]) == Convert.ToString(splcol[l]))
                                        {
                                            dt.Rows.Remove(dt.Rows[k]);
                                        }
                                    }
                                }
                            }
                            dt = getdedval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                            Session["dtded"] = dt;
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grid_ded.DataSource = dt;
                        grid_ded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grid_ded.DataBind();
                        for (int i = 0; i < grid_ded.Columns.Count; i++)
                        {
                            grid_ded.Columns[i].HeaderStyle.Width = 100;
                            grid_ded.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grid_ded.DataSource = dt;
                        grid_ded.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            if (savecount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Updated Successfully!";
                deduct_div.Visible = false;
                divgrdded.Visible = true;
                grid_ded.Visible = true;
            }
            if (errcount > 0)
            {
                lbl_dedalert.Visible = true;
                lbl_dedalert.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btn_deductdelete_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_dedalert.Visible = false;
            Session["dedtype"] = null;
            int delcount = 0;
            if (cb_popdeduct.Checked == true)
            {
                string newcol = "";
                if (Session["dedtype"] == null)
                {
                    if (cbl_popdd.Items.Count > 0)
                    {
                        newcol = GetSelectedItemsTextnew(cbl_popdd);
                        Session["dedtype"] = newcol;
                    }
                }
                string colvalue = "";
                string[] splcol = new string[20];
                colvalue = Convert.ToString(Session["dedtype"]);
                if (colvalue.Trim() != "")
                    splcol = colvalue.Split(',');
                divgrdded.Visible = true;
                grid_ded.Visible = true;
                deduct_div.Visible = false;
                DataTable dt = new DataTable();
                dtdedheader(dt);
                if (Session["dedtype"] == null)
                {
                    lbl_dedalert.Visible = true;
                    lbl_dedalert.Text = "Please Select Any Deduction!";
                    divgrdded.Visible = false;
                    grid_ded.Visible = false;
                    deduct_div.Visible = true;
                    return;
                }
                else
                {
                    if (Session["dtded"] != null)
                    {
                        DataRow dr;
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["dtded"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                for (int col = 0; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        if (splcol.Length > 0)
                        {
                            for (int l = 0; l < splcol.Length; l++)
                            {
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    if (Convert.ToString(dt.Rows[k][0]) == Convert.ToString(splcol[l]))
                                    {
                                        dt.Rows.Remove(dt.Rows[k]);
                                        delcount++;
                                    }
                                }
                            }
                        }
                        Session["dtded"] = dt;
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    grid_ded.DataSource = dt;
                    grid_ded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grid_ded.DataBind();
                    for (int i = 0; i < grid_ded.Columns.Count; i++)
                    {
                        grid_ded.Columns[i].HeaderStyle.Width = 100;
                        grid_ded.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grid_ded.DataSource = dt;
                    grid_ded.DataBind();
                }
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully!";
                deduct_div.Visible = false;
                divgrdded.Visible = true;
                grid_ded.Visible = true;
            }
        }
        catch { }
    }
    public void bindgridded()
    {
        try
        {
            Session["dtded"] = null;
            string staffcode = "";
            string deduction = "";
            if (checkedOK())
            {
                for (int sco = 0; sco < FpSpread.Sheets[0].RowCount; sco++)
                {
                    byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (check == 1)
                    {
                        staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[sco, 2].Text);
                        divgrdded.Visible = true;
                        grid_ded.Visible = true;
                        DataTable dt = new DataTable();
                        dtdedheader(dt);
                        string selq = "select st.staff_code,st.deductions,s.staff_name from stafftrans st,staffmaster s where st.staff_code=s.staff_code and st.staff_code='" + staffcode + "' and latestrec = 1 and college_code='" + clgcode + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    deduction = ds.Tables[0].Rows[0]["deductions"].ToString().Trim();
                                    string[] allowanmce_arr1;
                                    string alowancesplit;
                                    allowanmce_arr1 = deduction.Split('\\');
                                    for (int i = 0; i < allowanmce_arr1.Length; i++)
                                    {
                                        DataRow dr;
                                        alowancesplit = allowanmce_arr1[i];
                                        if (alowancesplit.Trim() != "")
                                        {
                                            string[] allowanceda;
                                            allowanceda = alowancesplit.Split(';');
                                            if (allowanceda[2].Trim() != "")
                                            {
                                                for (int ik = 0; ik < cbl_popdd.Items.Count; ik++)
                                                {
                                                    if (cbl_popdd.Items[ik].Selected == true)
                                                    {
                                                        if (allowanceda[0] == cbl_popdd.Items[ik].Text)
                                                        {
                                                            dr = dt.NewRow();
                                                            dr["dedtype"] = Convert.ToString(allowanceda[0]);
                                                            dr["mode"] = Convert.ToString(allowanceda[1]);
                                                            if (allowanceda[1] == "Amount")
                                                                dr["value"] = Convert.ToString(allowanceda[2]);
                                                            if (allowanceda[1] == "Percent")
                                                                dr["value"] = Convert.ToString(allowanceda[2]) + "%";
                                                            if (allowanceda[1] == "Slab")
                                                                dr["value"] = Convert.ToString(allowanceda[2]);
                                                            dr["dedround"] = Convert.ToString(allowanceda[11]);
                                                            if (Convert.ToString(allowanceda[3]) == "1")
                                                                dr["frmgross"] = "Yes";
                                                            else
                                                                dr["frmgross"] = "No";
                                                            if (Convert.ToString(allowanceda[4]) == "1")
                                                                dr["frmbasicda"] = "Yes";
                                                            else
                                                                dr["frmbasicda"] = "No";
                                                            if (Convert.ToString(allowanceda[6]) == "1")
                                                                dr["inclop"] = "Yes";
                                                            else
                                                                dr["inclop"] = "No";
                                                            if (Convert.ToString(allowanceda[7]) == "1")
                                                                dr["frmbasgpda"] = "Yes";
                                                            else
                                                                dr["frmbasgpda"] = "No";
                                                            if (Convert.ToString(allowanceda[8]) == "1")
                                                                dr["frmbas"] = "Yes";
                                                            else
                                                                dr["frmbas"] = "No";
                                                            if (Convert.ToString(allowanceda[9]) == "1")
                                                                dr["frmbasdp"] = "Yes";
                                                            else
                                                                dr["frmbasdp"] = "No";
                                                            if (Convert.ToString(allowanceda[10]) == "1")
                                                                dr["frmpetty"] = "Yes";
                                                            else
                                                                dr["frmpetty"] = "No";
                                                            if (Convert.ToString(allowanceda[12]) == "1")
                                                                dr["ismaxcal"] = "Yes";
                                                            else
                                                                dr["ismaxcal"] = "No";
                                                            dr["maxamnt"] = Convert.ToString(allowanceda[13]);
                                                            dr["dedamt"] = Convert.ToString(allowanceda[14]);
                                                            if (allowanceda.Length >= 16)
                                                            {
                                                                if (Convert.ToString(allowanceda[15]) == "1")
                                                                    dr["frmbasarr"] = "Yes";
                                                                else
                                                                    dr["frmbasarr"] = "No";
                                                            }
                                                            if (allowanceda.Length >= 17)
                                                            {
                                                                if (Convert.ToString(allowanceda[16]) == "1")
                                                                    dr["frmbasarrsa"] = "Yes";
                                                                else
                                                                    dr["frmbasarrsa"] = "No";
                                                            }
                                                            if (allowanceda.Length >= 18)
                                                            {
                                                                dr["frmallow"] = Convert.ToString(allowanceda[17]);
                                                            }
                                                            if (allowanceda.Length >= 20)
                                                            {
                                                                if (Convert.ToString(allowanceda[19]) == "1")
                                                                    dr["frmnetamnt"] = "Yes";
                                                                else
                                                                    dr["frmnetamnt"] = "No";
                                                            }
                                                            if (allowanceda.Length >= 21)
                                                            {
                                                                if (Convert.ToString(allowanceda[20]) == "1")
                                                                    dr["GrossLOP"] = "Yes";
                                                                else
                                                                    dr["GrossLOP"] = "No";
                                                            }
                                                            dt.Rows.Add(dr);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    Session["dtded"] = dt;
                                }
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            grid_ded.DataSource = dt;
                            grid_ded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            grid_ded.DataBind();
                            for (int i = 0; i < grid_ded.Columns.Count; i++)
                            {
                                grid_ded.Columns[i].HeaderStyle.Width = 100;
                                grid_ded.Columns[i].ItemStyle.Width = 100;
                            }
                        }
                        else
                        {
                            grid_ded.DataSource = dt;
                            grid_ded.DataBind();
                        }
                    }
                }
            }
        }
        catch { }
    }
    public void dedclear()
    {
        ddl_dmode.SelectedIndex = 0;
        txt_dval.Text = "";
        txt_mamt.Text = "";
        txt_damt.Text = "";
        chkdeddisable();
    }
    protected void grdlev_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[7].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdlev, "index$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdlev, "index$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdlev, "index$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdlev, "index$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdlev, "index$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdlev, "index$" + e.Row.RowIndex);
                e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdlev, "index$" + e.Row.RowIndex);
                e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdlev, "index$" + e.Row.RowIndex);
                e.Row.Cells[7].Visible = false;
            }
        }
        catch { }
    }
    protected void grdlev_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            lbl_ltypealert.Visible = false;
            string lblleave = "";
            string levtype = "";
            string yrlev = "";
            string monlev = "";
            string incsunday = "";
            string incholiday = "";
            string moncarry = "";
            string yrcarry = "";
            string MaxmnthLeave = "";

            for (int rem = 0; rem < grdlev.Rows.Count; rem++)
            {
                grdlev.Rows[rem].BackColor = Color.White;
            }
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "index")
            {
                grdlev.Visible = false;
                divgrdlev.Visible = false;
                ltype_div.Visible = true;
                btn_ltypesave.Visible = false;
                btn_ltypeupdate.Visible = true;
                btn_ltypedelete.Visible = true;
                int chkcount = 0;
                levtype = (grdlev.Rows[row].FindControl("lbl_levtype") as Label).Text;
                yrlev = (grdlev.Rows[row].FindControl("lbl_yrlev") as Label).Text;
                if (yrlev.Trim() != "" && yrlev.Trim() != "0")
                    txt_yl.Text = Convert.ToString(yrlev);
                else
                    txt_yl.Text = "";
                monlev = (grdlev.Rows[row].FindControl("lbl_monlev") as Label).Text;
                if (monlev.Trim() != "" && monlev.Trim() != "0")
                    txt_ml.Text = Convert.ToString(monlev);
                else
                    txt_ml.Text = "";
                incsunday = (grdlev.Rows[row].FindControl("lbl_incsunday") as Label).Text;
                if (incsunday.Trim() == "Yes")
                    cb_sunday.Checked = true;
                else
                    cb_sunday.Checked = false;
                incholiday = (grdlev.Rows[row].FindControl("lbl_incholiday") as Label).Text;
                if (incholiday.Trim() == "Yes")
                    cb_holiday.Checked = true;
                else
                    cb_holiday.Checked = false;
                moncarry = (grdlev.Rows[row].FindControl("lbl_moncarry") as Label).Text;
                if (moncarry.Trim() == "Yes")
                    cb_mco.Checked = true;
                else
                    cb_mco.Checked = false;
                yrcarry = (grdlev.Rows[row].FindControl("lbl_yrcarry") as Label).Text;
                if (yrcarry.Trim() == "Yes")
                    cb_yco.Checked = true;
                else
                    cb_yco.Checked = false;
                MaxmnthLeave = (grdlev.Rows[row].FindControl("lbl_MonthlyMaxLeave") as Label).Text;
                DataSet paymonth = new DataSet();
                GV1.Visible = false;
                if (yrlev != "")
                {

                    string selmaxLeavCount = d2.GetFunction("select value from Master_Settings where settings='StaffMaxLeavePerMonth' and usercode='" + usercode + "'");
                    if (selmaxLeavCount.Trim() != "" && selmaxLeavCount.Trim() != "0" && selmaxLeavCount.Trim() == "1")
                    {
                        if (txt_yl.Text != "")
                        {
                            string queryObject = "select * from hrpaymonths where college_code='" + Session["collegecode"] + "' and SelStatus='1'";
                            paymonth = d2.select_method_wo_parameter(queryObject, "Text");
                            DataTable dtmaxleave = new DataTable();
                            if (paymonth.Tables.Count > 0 && paymonth.Tables[0].Rows.Count > 0)
                            {

                                dtmaxleave.Columns.Add("Lblmonth");
                                dtmaxleave.Columns.Add("ddlmax");
                                dtmaxleave.Columns.Add("txtfdate");
                                dtmaxleave.Columns.Add("txttdate");
                               
                                DataRow dr = null;
                                for (int val = 0; val < paymonth.Tables[0].Rows.Count; val++)
                                {
                                    string paymonthval = Convert.ToString(paymonth.Tables[0].Rows[val]["PayMonth"]);
                                    DateTime fromdates = Convert.ToDateTime(paymonth.Tables[0].Rows[val]["From_Date"]);
                                    DateTime todates = Convert.ToDateTime(paymonth.Tables[0].Rows[val]["To_Date"]);

                                    dr = dtmaxleave.NewRow();
                                    dr["Lblmonth"] = paymonthval;
                                    dr["txtfdate"] = fromdates.ToString("dd/MM/yyyy");
                                    dr["txttdate"] = todates.ToString("dd/MM/yyyy");


                                    dtmaxleave.Rows.Add(dr);

                                }

                            }
                            if (dtmaxleave.Rows.Count > 0)
                            {
                                GV1.DataSource = dtmaxleave;
                                GV1.DataBind();
                                GV1.Visible = true;

                                foreach (GridViewRow gr in GV1.Rows)
                                {
                                    DropDownList ddlmax = (gr.FindControl("ddlmaxleave") as DropDownList);

                                    for (int i = 0; i <= Convert.ToInt32(txt_yl.Text); i++)
                                    {
                                        ddlmax.Items.Insert(i, Convert.ToString(i) == "0" ? "Select" : Convert.ToString(i));
                                    }


                                }

                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Enter Yearly Leave')", true);
                        }
                    }

                }
                for (int ik = 0; ik < cbl_poplt.Items.Count; ik++)
                {
                    if (levtype.Trim() == cbl_poplt.Items[ik].Text)
                    {
                        cbl_poplt.Items[ik].Selected = true;
                        lblleave = cbl_poplt.Items[ik].Text;
                        chkcount++;
                    }
                    else
                    {
                        cbl_poplt.Items[ik].Selected = false;
                    }
                }
                txt_popltype.Text = "Leave Type(" + Convert.ToString(chkcount) + ")";
                cb_poplt.Checked = false;
                lbl_h3.Text = "Leave Type -" + " " + lblleave;
                grdlev.Rows[row].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }
    public void btn_ltypeexit_Click(object sender, EventArgs e)
    {
        int chkcount = 0;
        ltype_div.Visible = false;
        divgrdlev.Visible = true;
        grdlev.Visible = true;
        for (int chk = 0; chk < cbl_poplt.Items.Count; chk++)
        {
            cbl_poplt.Items[chk].Selected = true;
            chkcount++;
        }
        txt_popltype.Text = "Leave Type(" + Convert.ToString(chkcount) + ")";
    }
    public void btn_ltypesave_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_ltypealert.Visible = false;
            Session["levtype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (cb_popltype.Checked == true)
            {
                string leave = "";
                string college = ddlcollege.SelectedValue;
                if (txt_yl.Text != "")
                {
                    leave = getoveralllev();
                    string newcol = "";
                    if (Session["levtype"] == null)
                    {
                        if (cbl_poplt.Items.Count > 0)
                        {
                            newcol = GetSelectedItemsTextnew(cbl_poplt);
                            Session["levtype"] = newcol;
                        }
                    }
                    string colvalue = "";
                    string[] splcol = new string[15];
                    colvalue = Convert.ToString(Session["levtype"]);
                    if (colvalue.Trim() != "")
                        splcol = colvalue.Split(',');
                    divgrdlev.Visible = true;
                    grdlev.Visible = true;
                    ltype_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtltypeheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = leave.Split('\\');
                    if (Session["levtype"] == null)
                    {
                        lbl_ltypealert.Visible = true;
                        lbl_ltypealert.Text = "Please Select Any Leave Type!";
                        divgrdlev.Visible = false;
                        grdlev.Visible = false;
                        ltype_div.Visible = true;
                        return;
                    }
                    else
                    {
                        if (Session["dtlev"] != null)
                        {
                            DataRow dr;
                            DataTable dnew = new DataTable();
                            dnew = (DataTable)Session["dtlev"];
                            if (dnew.Rows.Count > 0)
                            {
                                for (int ro = 0; ro < dnew.Rows.Count; ro++)
                                {
                                    dr = dt.NewRow();
                                    for (int col = 0; col < dnew.Columns.Count; col++)
                                    {
                                        dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                            if (splcol.Length > 0)
                            {
                                for (int l = 0; l < splcol.Length; l++)
                                {
                                    for (int k = 0; k < dt.Rows.Count; k++)
                                    {
                                        if (Convert.ToString(dt.Rows[k][0]) == Convert.ToString(splcol[l]))
                                        {
                                            dt.Rows.Remove(dt.Rows[k]);
                                        }
                                    }
                                }
                            }
                            dt = getlevval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                            Session["dtlev"] = dt;
                        }
                        else
                        {
                            DataRow dr;
                            dt = getlevval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                            Session["dtlev"] = null;
                            Session["dtlev"] = dt;
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grdlev.DataSource = dt;
                        grdlev.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdlev.DataBind();
                        for (int i = 0; i < grdlev.Columns.Count; i++)
                        {
                            grdlev.Columns[i].HeaderStyle.Width = 100;
                            grdlev.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grdlev.DataSource = dt;
                        grdlev.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            if (savecount > 0)
            {
                lbl_ltypealert.Visible = true;
                lbl_ltypealert.Text = " Added Successfully! ";
                ltype_div.Visible = true;
                divgrdlev.Visible = false;
                grdlev.Visible = false;
            }
            if (errcount > 0)
            {
                lbl_ltypealert.Visible = true;
                lbl_ltypealert.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btn_ltypeupdate_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_ltypealert.Visible = false;
            Session["levtype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (cb_popltype.Checked == true)
            {
                string leave = "";
                string college = ddlcollege.SelectedValue;
                if (txt_yl.Text != "")
                {
                    leave = getoveralllev();
                    string newcol = "";
                    if (Session["levtype"] == null)
                    {
                        if (cbl_poplt.Items.Count > 0)
                        {
                            newcol = GetSelectedItemsTextnew(cbl_poplt);
                            Session["levtype"] = newcol;
                        }
                    }
                    string colvalue = "";
                    string[] splcol = new string[15];
                    colvalue = Convert.ToString(Session["levtype"]);
                    if (colvalue.Trim() != "")
                        splcol = colvalue.Split(',');
                    divgrdlev.Visible = true;
                    grdlev.Visible = true;
                    ltype_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtltypeheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = leave.Split('\\');
                    if (Session["levtype"] == null)
                    {
                        lbl_ltypealert.Visible = true;
                        lbl_ltypealert.Text = "Please Select Any Leave Type!";
                        divgrdlev.Visible = false;
                        grdlev.Visible = false;
                        ltype_div.Visible = true;
                        return;
                    }
                    else
                    {
                        if (Session["dtlev"] != null)
                        {
                            DataRow dr;
                            DataTable dnew = new DataTable();
                            dnew = (DataTable)Session["dtlev"];
                            if (dnew.Rows.Count > 0)
                            {
                                for (int ro = 0; ro < dnew.Rows.Count; ro++)
                                {
                                    dr = dt.NewRow();
                                    for (int col = 0; col < dnew.Columns.Count; col++)
                                    {
                                        dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                            if (splcol.Length > 0)
                            {
                                for (int l = 0; l < splcol.Length; l++)
                                {
                                    for (int k = 0; k < dt.Rows.Count; k++)
                                    {
                                        if (Convert.ToString(dt.Rows[k][0]) == Convert.ToString(splcol[l]))
                                        {
                                            dt.Rows.Remove(dt.Rows[k]);
                                        }
                                    }
                                }
                            }
                            dt = getlevval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                            Session["dtlev"] = dt;
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grdlev.DataSource = dt;
                        grdlev.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdlev.DataBind();
                        for (int i = 0; i < grdlev.Columns.Count; i++)
                        {
                            grdlev.Columns[i].HeaderStyle.Width = 100;
                            grdlev.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grdlev.DataSource = dt;
                        grdlev.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            if (savecount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = " Updated Successfully! ";
                ltype_div.Visible = false;
                divgrdlev.Visible = true;
                grdlev.Visible = true;
            }
            if (errcount > 0)
            {
                lbl_ltypealert.Visible = true;
                lbl_ltypealert.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btn_ltypedelete_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_ltypealert.Visible = false;
            Session["levtype"] = null;
            int delcount = 0;
            if (cb_popltype.Checked == true)
            {
                string newcol = "";
                if (Session["levtype"] == null)
                {
                    if (cbl_poplt.Items.Count > 0)
                    {
                        newcol = GetSelectedItemsTextnew(cbl_poplt);
                        Session["levtype"] = newcol;
                    }
                }
                string colvalue = "";
                string[] splcol = new string[15];
                colvalue = Convert.ToString(Session["levtype"]);
                if (colvalue.Trim() != "")
                    splcol = colvalue.Split(',');
                divgrdlev.Visible = true;
                grdlev.Visible = true;
                ltype_div.Visible = false;
                DataTable dt = new DataTable();
                dtltypeheader(dt);
                if (Session["levtype"] == null)
                {
                    lbl_ltypealert.Visible = true;
                    lbl_ltypealert.Text = "Please Select Any Leave Type!";
                    divgrdlev.Visible = false;
                    grdlev.Visible = false;
                    ltype_div.Visible = true;
                    return;
                }
                else
                {
                    if (Session["dtlev"] != null)
                    {
                        DataRow dr;
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["dtlev"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                for (int col = 0; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        if (splcol.Length > 0)
                        {
                            for (int l = 0; l < splcol.Length; l++)
                            {
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    if (Convert.ToString(dt.Rows[k][0]) == Convert.ToString(splcol[l]))
                                    {
                                        dt.Rows.Remove(dt.Rows[k]);
                                        delcount++;
                                    }
                                }
                            }
                        }
                        Session["dtlev"] = dt;
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    grdlev.DataSource = dt;
                    grdlev.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdlev.DataBind();
                    for (int i = 0; i < grdlev.Columns.Count; i++)
                    {
                        grdlev.Columns[i].HeaderStyle.Width = 100;
                        grdlev.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdlev.DataSource = dt;
                    grdlev.DataBind();
                }
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = " Deleted Successfully!";
                ltype_div.Visible = false;
                divgrdlev.Visible = true;
                grdlev.Visible = true;
            }
        }
        catch { }
    }
    public void bindgridlev()
    {
        try
        {
            Session["dtlev"] = null;
            string staffcode = "";
            string leavetype = "";
            string Monthlymaxleave = string.Empty;
            if (checkedOK())
            {
                for (int sco = 0; sco < FpSpread.Sheets[0].RowCount; sco++)//delsi
                {
                    byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (check == 1)
                    {
                        staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[sco, 2].Text);
                        divgrdlev.Visible = true;
                        grdlev.Visible = true;
                        DataTable dt = new DataTable();
                        dtltypeheader(dt);
                        string selq = "select l.leavetype,MaxMonthlyLeave,s.staff_name,s.staff_code from individual_Leave_type l,staffmaster s where l.staff_code=s.staff_code and s.staff_code='" + staffcode + "' and l.college_code='" + clgcode + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr;
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    leavetype = ds.Tables[0].Rows[0]["leavetype"].ToString().Trim();
                                  

                                    string[] allowanmce_arr1;
                                    string alowancesplit;
                                    allowanmce_arr1 = leavetype.Split('\\');
                                    for (int i = 0; i < allowanmce_arr1.Length; i++)
                                    {
                                        dr = dt.NewRow();
                                        alowancesplit = allowanmce_arr1[i];
                                        if (alowancesplit.Trim() != "")
                                        {
                                            string[] allowanceda;
                                            allowanceda = alowancesplit.Split(';');
                                            if (allowanceda[1].Trim() != "")
                                            {
                                                for (int ik = 0; ik < cbl_poplt.Items.Count; ik++)
                                                {
                                                    if (cbl_poplt.Items[ik].Selected == true)
                                                    {
                                                        if (allowanceda[0].ToUpper().Trim() == cbl_poplt.Items[ik].Text.ToUpper().Trim())//delsi19/05
                                                        {
                                                            dr["levtype"] = Convert.ToString(allowanceda[0]);
                                                            dr["yrlev"] = Convert.ToString(allowanceda[1]);
                                                            dr["monlev"] = Convert.ToString(allowanceda[2]);
                                                            if (Convert.ToString(allowanceda[4]) == "1")
                                                                dr["incsunday"] = "Yes";
                                                            else
                                                                dr["incsunday"] = "No";
                                                            if (Convert.ToString(allowanceda[5]) == "1")
                                                                dr["incholiday"] = "Yes";
                                                            else
                                                                dr["incholiday"] = "No";
                                                            if (Convert.ToString(allowanceda[6]) == "1")
                                                                dr["moncarry"] = "Yes";
                                                            else
                                                                dr["moncarry"] = "No";
                                                            if (Convert.ToString(allowanceda[7]) == "1")
                                                                dr["yrcarry"] = "Yes";
                                                            else
                                                                dr["yrcarry"] = "No";

                                                            Monthlymaxleave = ds.Tables[0].Rows[0]["MaxMonthlyLeave"].ToString().Trim();
                                                            if (Monthlymaxleave != "")
                                                            {
                                                                string[] maxLeave = Monthlymaxleave.Split('@');
                                                                for (int val = 0; val < maxLeave.Count(); val++)
                                                                {
                                                                    string gettext = Convert.ToString(maxLeave[val]);
                                                                    if (gettext.Contains(':'))
                                                                    {
                                                                        string[] splitTypeLeave = gettext.Split(':');
                                                                        string getLveType = Convert.ToString(splitTypeLeave[0]);
                                                                        if (allowanceda[0].ToUpper().Trim() == getLveType.ToUpper().Trim())
                                                                        {
                                                                            dr["MonthlyMaxLeave"] = Convert.ToString(splitTypeLeave[1]);
                                                                        }
                                                                    
                                                                    }
                                                                   
                                                                    
                                                                
                                                                }
                                                                
                                                            }
                                                            else
                                                            {
                                                                dr["MonthlyMaxLeave"] = "";
                                                            }
                                                            dt.Rows.Add(dr);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    Session["dtlev"] = dt;
                                }
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            grdlev.DataSource = dt;
                            grdlev.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            grdlev.DataBind();
                            for (int i = 0; i < grdlev.Columns.Count; i++)
                            {
                                grdlev.Columns[i].HeaderStyle.Width = 100;
                                grdlev.Columns[i].ItemStyle.Width = 100;
                            }
                        }
                        else
                        {
                            grdlev.DataSource = dt;
                            grdlev.DataBind();
                        }
                    }
                }
            }
        }
        catch { }
    }
    public void levclear()
    {
        txt_yl.Text = "";
        txt_ml.Text = "";
    }
    protected void grdcom_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdcom, "index$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdcom, "index$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdcom, "index$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdcom, "index$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdcom, "index$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdcom, "index$" + e.Row.RowIndex);
                e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdcom, "index$" + e.Row.RowIndex);
                e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdcom, "index$" + e.Row.RowIndex);
            }
        }
        catch { }
    }
    protected void grdcom_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            lblerrco.Visible = false;
            string gradepay = "";
            string basicpay = "";
            string payband = "";
            string ismpf = "";
            string ismpfper = "";
            string ismpfamnt = "";
            string isautogp = "";
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "index")
            {
                grdcom.Visible = false;
                divgrdcom.Visible = false;
                divcommon.Visible = true;
                btnsavecom.Visible = false;
                btnupdatecom.Visible = true;
                btndelcom.Visible = true;
                gradepay = (grdcom.Rows[row].FindControl("lbl_grad") as Label).Text;
                if (gradepay.Trim() != "" && gradepay.Trim() != "0")
                    txtgrad.Text = Convert.ToString(gradepay);
                else
                    txtgrad.Text = "";
                basicpay = (grdcom.Rows[row].FindControl("lbl_basicpay") as Label).Text;
                if (basicpay.Trim() != "" && basicpay.Trim() != "0.0" && basicpay.Trim() != "0" && basicpay.Trim() != "0.0000")
                    txtbasiccom.Text = Convert.ToString(basicpay);
                else
                    txtbasiccom.Text = "";
                payband = (grdcom.Rows[row].FindControl("lbl_payband") as Label).Text;
                if (payband.Trim() != "" && payband.Trim() != "0")
                    txtpayband.Text = Convert.ToString(payband);
                else
                    txtpayband.Text = "";
                ismpfamnt = (grdcom.Rows[row].FindControl("lbl_ismpfamnt") as Label).Text;
                if (ismpfamnt.Trim() == "Yes")
                    cbismpfamnt.Checked = true;
                else
                    cbismpfamnt.Checked = false;
                if (ismpfamnt.Trim() == "Yes")
                {
                    lblismpf.Visible = true;
                    txtismpf.Visible = true;
                    lblismpfper.Visible = true;
                    txtismpfper.Visible = true;
                }
                else
                {
                    lblismpf.Visible = false;
                    txtismpf.Visible = false;
                    lblismpfper.Visible = false;
                    txtismpfper.Visible = false;
                }
                ismpf = (grdcom.Rows[row].FindControl("lbl_ismpf") as Label).Text;
                if (ismpf.Trim() != "" && ismpf.Trim() != "0" && ismpf.Trim() != "0.00")
                    txtismpf.Text = Convert.ToString(ismpf);
                else
                    txtismpf.Text = "";
                ismpfper = (grdcom.Rows[row].FindControl("lbl_ismpfper") as Label).Text;
                if (ismpfper.Trim() != "" && ismpfper.Trim() != "0" && ismpfper.Trim() != "0.00")
                    txtismpfper.Text = Convert.ToString(ismpfper);
                else
                    txtismpfper.Text = "";
                isautogp = (grdcom.Rows[row].FindControl("lbl_isautogp") as Label).Text;
                if (isautogp.Trim() == "Yes")
                    cbisautogp.Checked = true;
                else
                    cbisautogp.Checked = false;
            }
        }
        catch { }
    }
    protected void btnexitcom_Click(object sender, EventArgs e)
    {
        divcommon.Visible = false;
        grdcom.Visible = true;
        divgrdcom.Visible = true;
    }
    protected void btnsavecom_Click(object sender, EventArgs e)
    {
        try
        {
            int savecount = 0;
            int errcount = 0;
            if (txtbasiccom.Text.Trim() != "")
            {
                string gradepay = Convert.ToString(txtgrad.Text);
                string basicpay = Convert.ToString(txtbasiccom.Text);
                string payband = Convert.ToString(txtpayband.Text);
                string ismpf = Convert.ToString(txtismpf.Text);
                string ismpfper = Convert.ToString(txtismpfper.Text);
                string ismpfamnt = "";
                string isautogp = "";
                if (cbismpfamnt.Checked == true)
                    ismpfamnt = "1";
                if (cbisautogp.Checked == true)
                    isautogp = "1";
                divgrdcom.Visible = true;
                grdcom.Visible = true;
                divcommon.Visible = false;
                DataTable dt = new DataTable();
                dtcomheader(dt);
                DataRow dr;
                if (Session["dtcom"] != null)
                {
                    DataTable dnew = (DataTable)Session["dtcom"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    dt.Rows.Clear();
                    dt = getcomval(dt, dt.NewRow(), gradepay, basicpay, payband, ismpf, ismpfper, ismpfamnt, isautogp);
                    Session["dtcom"] = dt;
                }
                else
                {
                    dt = getcomval(dt, dt.NewRow(), gradepay, basicpay, payband, ismpf, ismpfper, ismpfamnt, isautogp);
                    Session["dtcom"] = null;
                    Session["dtcom"] = dt;
                }
                if (dt.Rows.Count > 0)
                {
                    savecount++;
                    grdcom.DataSource = dt;
                    grdcom.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdcom.DataBind();
                    for (int i = 0; i < grdcom.Columns.Count; i++)
                    {
                        grdcom.Columns[i].HeaderStyle.Width = 100;
                        grdcom.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdcom.DataSource = dt;
                    grdcom.DataBind();
                }
            }
            else
            {
                errcount++;
            }
            if (savecount > 0)
            {
                lblerrco.Visible = true;
                lblerrco.Text = "Added Successfully!";
                divgrdcom.Visible = false;
                divcommon.Visible = true;
                grdcom.Visible = false;
            }
            if (errcount > 0)
            {
                lblerrco.Visible = true;
                lblerrco.Text = "Please Fill all the Values!";
            }
        }
        catch { }
    }
    protected void btnupdatecom_Click(object sender, EventArgs e)
    {
        try
        {
            int savecount = 0;
            int errcount = 0;
            if (txtbasiccom.Text.Trim() != "")
            {
                string gradepay = Convert.ToString(txtgrad.Text);
                string basicpay = Convert.ToString(txtbasiccom.Text);
                string payband = Convert.ToString(txtpayband.Text);
                string ismpf = Convert.ToString(txtismpf.Text);
                string ismpfper = Convert.ToString(txtismpfper.Text);
                string ismpfamnt = "";
                string isautogp = "";
                if (cbismpfamnt.Checked == true)
                    ismpfamnt = "1";
                if (cbisautogp.Checked == true)
                    isautogp = "1";
                divgrdcom.Visible = true;
                grdcom.Visible = true;
                divcommon.Visible = false;
                DataTable dt = new DataTable();
                dtcomheader(dt);
                DataRow dr;
                if (Session["dtcom"] != null)
                {
                    DataTable dnew = (DataTable)Session["dtcom"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    dt.Rows.Clear();
                    dt = getcomval(dt, dt.NewRow(), gradepay, basicpay, payband, ismpf, ismpfper, ismpfamnt, isautogp);
                    Session["dtcom"] = dt;
                }
                if (dt.Rows.Count > 0)
                {
                    savecount++;
                    grdcom.DataSource = dt;
                    grdcom.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdcom.DataBind();
                    for (int i = 0; i < grdcom.Columns.Count; i++)
                    {
                        grdcom.Columns[i].HeaderStyle.Width = 100;
                        grdcom.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdcom.DataSource = dt;
                    grdcom.DataBind();
                }
            }
            else
            {
                errcount++;
            }
            if (savecount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Updated Successfully!";
                divgrdcom.Visible = true;
                divcommon.Visible = false;
                grdcom.Visible = true;
            }
            if (errcount > 0)
            {
                lblerrco.Visible = true;
                lblerrco.Text = "Please Fill all the Values!";
            }
        }
        catch { }
    }
    protected void btndelcom_Click(object sender, EventArgs e)
    {
        try
        {
            int delcount = 0;
            divgrdcom.Visible = true;
            grdcom.Visible = true;
            divcommon.Visible = false;
            DataTable dt = new DataTable();
            dtcomheader(dt);
            DataRow dr;
            if (Session["dtcom"] != null)
            {
                DataTable dnew = (DataTable)Session["dtcom"];
                if (dnew.Rows.Count > 0)
                {
                    for (int ro = 0; ro < dnew.Rows.Count; ro++)
                    {
                        dr = dt.NewRow();
                        for (int col = 0; col < dnew.Columns.Count; col++)
                        {
                            dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                        }
                        dt.Rows.Add(dr);
                    }
                }
                dt.Rows.Clear();
                delcount++;
                Session["dtcom"] = dt;
            }
            if (dt.Rows.Count > 0)
            {
                grdcom.DataSource = dt;
                grdcom.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdcom.DataBind();
                for (int i = 0; i < grdcom.Columns.Count; i++)
                {
                    grdcom.Columns[i].HeaderStyle.Width = 100;
                    grdcom.Columns[i].ItemStyle.Width = 100;
                }
            }
            else
            {
                grdcom.DataSource = dt;
                grdcom.DataBind();
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully!";
                divgrdcom.Visible = true;
                grdcom.Visible = true;
                divcommon.Visible = false;
            }
        }
        catch { }
    }
    public void bindgridcom()
    {
        try
        {
            Session["dtcom"] = null;
            string staffcode = "";
            if (checkedOK())
            {
                for (int sco = 0; sco < FpSpread.Sheets[0].RowCount; sco++)
                {
                    byte Check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (Check == 1)
                    {
                        staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[sco, 2].Text);
                        divgrdcom.Visible = true;
                        grdcom.Visible = true;
                        Double gradepay = 0;
                        Double basicpay = 0;
                        Double Payband = 0;
                        Double mpfamnt = 0;
                        DataTable dt = new DataTable();
                        dtcomheader(dt);
                        string selq = "Select grade_pay,MPFAmount,MPFPer,IsMPFAmt,bsalary,pay_band,IsAutoGP from stafftrans where staff_code='" + staffcode + "' and latestrec='1'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    dr = dt.NewRow();
                                    if (ds.Tables[0].Rows[i]["grade_pay"] == "" || ds.Tables[0].Rows[i]["grade_pay"] == null)
                                    {
                                        dr["gradepay"] = "0";
                                    }
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["grade_pay"]), out gradepay);
                                        gradepay = Math.Round(gradepay, 2, MidpointRounding.AwayFromZero);
                                        dr["gradepay"] = Convert.ToString(gradepay);
                                    }
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["bsalary"]) == "" || ds.Tables[0].Rows[i]["bsalary"] == null)
                                    {
                                        dr["basicpay"] = "0";
                                    }
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["bsalary"]), out basicpay);
                                        basicpay = Math.Round(basicpay, 2, MidpointRounding.AwayFromZero);
                                        dr["basicpay"] = Convert.ToString(basicpay);
                                    }
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["pay_band"]) == "" || ds.Tables[0].Rows[i]["pay_band"] == null)
                                    {
                                        dr["payband"] = "0";
                                    }
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["pay_band"]), out Payband);
                                        Payband = Math.Round(Payband, 2, MidpointRounding.AwayFromZero);
                                        dr["payband"] = Convert.ToString(Payband);
                                    }
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["MPFAmount"]) == "" || ds.Tables[0].Rows[i]["MPFAmount"] == null)
                                    {
                                        dr["ismpf"] = "0";
                                    }
                                    else
                                    {
                                        Double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["MPFAmount"]), out mpfamnt);
                                        mpfamnt = Math.Round(mpfamnt, 2, MidpointRounding.AwayFromZero);
                                        dr["ismpf"] = Convert.ToString(mpfamnt);
                                    }
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["MPFPer"]) == "" || ds.Tables[0].Rows[i]["MPFPer"] == null)
                                        dr["ismpfper"] = "0";
                                    else
                                        dr["ismpfper"] = Convert.ToString(ds.Tables[0].Rows[i]["MPFPer"]);
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["IsMPFAmt"]).ToUpper() == "TRUE")
                                        dr["ismpfamnt"] = "Yes";
                                    else if (Convert.ToString(ds.Tables[0].Rows[i]["IsMPFAmt"]).ToUpper() == "FALSE")
                                        dr["ismpfamnt"] = "No";
                                    else
                                        dr["ismpfamnt"] = "";
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["IsAutoGP"]).ToUpper() == "TRUE")
                                        dr["isautogp"] = "Yes";
                                    else if (Convert.ToString(ds.Tables[0].Rows[i]["IsAutoGP"]).ToUpper() == "FALSE")
                                        dr["isautogp"] = "No";
                                    else
                                        dr["isautogp"] = "";
                                    dt.Rows.Add(dr);
                                }
                                Session["dtcom"] = dt;
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            grdcom.DataSource = dt;
                            grdcom.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            grdcom.DataBind();
                            for (int i = 0; i < grdcom.Columns.Count; i++)
                            {
                                grdcom.Columns[i].HeaderStyle.Width = 100;
                                grdcom.Columns[i].ItemStyle.Width = 100;
                            }
                        }
                        if (dt.Rows.Count == 0)
                        {
                            grdcom.DataSource = dt;
                            grdcom.DataBind();
                        }
                    }
                }
            }
        }
        catch
        { }
    }
    public void comclear()
    {
        txtgrad.Text = "";
        txtismpf.Text = "";
        txtismpfper.Text = "";
        txtbasiccom.Text = "";
        txtpayband.Text = "";
        cbismpfamnt.Checked = false;
        cbisautogp.Checked = false;
    }
    protected void btngradeexit_click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
        btn_go_Click(sender, e);
    }
    protected void btngrade_click(object sender, EventArgs e)
    {
        try
        {
            string allowance = "";
            string deduction = "";
            string leavetype = "";
            string MonthlyMaxLeave = "";
            if (grd_all.Rows.Count > 0)
            {
                for (int ro = 0; ro < grd_all.Rows.Count; ro++)
                {
                    string allvalue = "";
                    string inclop = "";
                    string frmbasic = "";
                    string frmbasgp = "";
                    string isspl = "";
                    string frmbasagp = "";
                    Label lblall = (Label)grd_all.Rows[ro].FindControl("lbl_alltype");
                    Label lblmode = (Label)grd_all.Rows[ro].FindControl("lbl_mode");
                    Label lblval = (Label)grd_all.Rows[ro].FindControl("lbl_val");
                    if (lblval.Text.Trim().Contains("%"))
                        allvalue = Convert.ToString(lblval.Text).Split('%')[0];
                    else
                        allvalue = Convert.ToString(lblval.Text);
                    Label lblinclop = (Label)grd_all.Rows[ro].FindControl("lbl_lop");
                    if (lblinclop.Text == "Yes")
                        inclop = "1";
                    Label lblfrmbasic = (Label)grd_all.Rows[ro].FindControl("lbl_frmbasic");
                    if (lblfrmbasic.Text == "Yes")
                        frmbasic = "1";
                    Label lblfrmbasgp = (Label)grd_all.Rows[ro].FindControl("lbl_frmbasgp");
                    if (lblfrmbasgp.Text == "Yes")
                        frmbasgp = "1";
                    Label lblisspl = (Label)grd_all.Rows[ro].FindControl("lbl_isspl");
                    if (lblisspl.Text == "Yes")
                        isspl = "1";
                    Label lblfrmbasagp = (Label)grd_all.Rows[ro].FindControl("lbl_frmbasagp");
                    if (lblfrmbasagp.Text == "Yes")
                        frmbasagp = "1";
                    Label lblround = (Label)grd_all.Rows[ro].FindControl("lbl_roundtype");
                    Label frmAllAllow = (Label)grd_all.Rows[ro].FindControl("lbl_fromallallow");

                    if (allowance.Trim() == "")
                        allowance = Convert.ToString(lblall.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(allvalue) + ";;" + inclop + ";;" + frmbasic + ";;" + frmbasgp + ";" + isspl + ";" + frmbasagp + ";" + Convert.ToString(lblround.Text) + ";;" + Convert.ToString(frmAllAllow.Text) + ";";
                    else
                        allowance = allowance + "\\" + Convert.ToString(lblall.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(allvalue) + ";;" + inclop + ";;" + frmbasic + ";;" + frmbasgp + ";" + isspl + ";" + frmbasagp + ";" + Convert.ToString(lblround.Text) + ";;" + Convert.ToString(frmAllAllow.Text) + ";";
                }
                if (allowance.Trim() != "")
                    allowance = allowance + "\\";
            }
            if (grid_ded.Rows.Count > 0)
            {
                for (int ro = 0; ro < grid_ded.Rows.Count; ro++)
                {
                    string frmgross = "";
                    string frmbasicda = "";
                    string incdedlop = "";
                    string frmbasgpda = "";
                    string frmbas = "";
                    string frmbasdp = "";
                    string frmpetty = "";
                    string frmbasarr = "";
                    string ismaxcal = "";
                    string frmbasarrsa = "";
                    string frmallow = "";
                    string dedvalue = "";
                    string proftax = "";
                    string gross_MinusLOP = string.Empty;
                    Label lblded = (Label)grid_ded.Rows[ro].FindControl("lbl_deducttype");
                    Label lblmode = (Label)grid_ded.Rows[ro].FindControl("lbl_dedmode");
                    Label lblval = (Label)grid_ded.Rows[ro].FindControl("lbl_dedval");
                    if (lblval.Text.Trim().Contains("%"))
                        dedvalue = Convert.ToString(lblval.Text).Split('%')[0];
                    else
                        dedvalue = Convert.ToString(lblval.Text);
                    Label lblround = (Label)grid_ded.Rows[ro].FindControl("lbl_rounddedroundtype");
                    Label lblfrmgross = (Label)grid_ded.Rows[ro].FindControl("lbl_dedfrmgross");
                    if (lblfrmgross.Text == "Yes")
                        frmgross = "1";
                    Label lblfrmbasicda = (Label)grid_ded.Rows[ro].FindControl("lbl_frmbasicda");
                    if (lblfrmbasicda.Text == "Yes")
                        frmbasicda = "1";
                    Label lblfrmlop = (Label)grid_ded.Rows[ro].FindControl("lbl_dedfrmlop");
                    if (lblfrmlop.Text == "Yes")
                        incdedlop = "1";
                    Label lbldedgpda = (Label)grid_ded.Rows[ro].FindControl("lbl_dedgpda");
                    if (lbldedgpda.Text == "Yes")
                        frmbasgpda = "1";
                    Label lbldedfrmbas = (Label)grid_ded.Rows[ro].FindControl("lbl_dedfrmbas");
                    if (lbldedfrmbas.Text == "Yes")
                        frmbas = "1";
                    Label lblfrmbasdp = (Label)grid_ded.Rows[ro].FindControl("lbl_dedfrmbasdp");
                    if (lblfrmbasdp.Text == "yes")
                        frmbasdp = "1";
                    Label lbldedfrmpetty = (Label)grid_ded.Rows[ro].FindControl("lbl_dedfrmpetty");
                    if (lbldedfrmpetty.Text == "Yes")
                        frmpetty = "1";
                    Label lbldedfrmbasarr = (Label)grid_ded.Rows[ro].FindControl("lbl_dedfrmbasarr");
                    if (lbldedfrmbasarr.Text == "Yes")
                        frmbasarr = "1";
                    Label lbldedismaxcal = (Label)grid_ded.Rows[ro].FindControl("lbl_dedismaxcal");
                    if (lbldedismaxcal.Text == "Yes")
                        ismaxcal = "1";
                    Label lblmax = (Label)grid_ded.Rows[ro].FindControl("lbl_maxamt");
                    Label lbldedamt = (Label)grid_ded.Rows[ro].FindControl("lbl_dedamt");
                    Label lblfrmbasarrsa = (Label)grid_ded.Rows[ro].FindControl("lbl_dedfrmbasarrsa");
                    if (lblfrmbasarrsa.Text == "Yes")
                        frmbasarrsa = "1";
                    Label lbldedfrmallo = (Label)grid_ded.Rows[ro].FindControl("lbl_dedfrmallow");
                    Label lblcomval = (Label)grid_ded.Rows[ro].FindControl("lbl_frmnetamnt");
                    if (lblcomval.Text == "Yes")
                        frmallow = "1";
                    if (cb_professionaltax.Checked == true) //poo 24.10.17
                        proftax = "1";
                    if (radBtn_grosswithlop.Checked == true)
                        gross_MinusLOP = "1";
                    if (deduction.Trim() == "")
                        deduction = Convert.ToString(lblded.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(dedvalue) + ";" + frmgross + ";" + frmbasicda + ";;" + incdedlop + ";" + frmbasgpda + ";" + frmbas + ";" + frmbasdp + ";" + frmpetty + ";" + Convert.ToString(lblround.Text) + ";" + ismaxcal + ";" + Convert.ToString(lblmax.Text) + ";" + Convert.ToString(lbldedamt.Text) + ";" + frmbasarr + ";" + frmbasarrsa + ";" + Convert.ToString(lbldedfrmallo.Text) + ";;" + frmallow + ";" + proftax + ";" + gross_MinusLOP;
                    else
                        deduction = deduction + "\\" + Convert.ToString(lblded.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(dedvalue) + ";" + frmgross + ";" + frmbasicda + ";;" + incdedlop + ";" + frmbasgpda + ";" + frmbas + ";" + frmbasdp + ";" + frmpetty + ";" + Convert.ToString(lblround.Text) + ";" + ismaxcal + ";" + Convert.ToString(lblmax.Text) + ";" + Convert.ToString(lbldedamt.Text) + ";" + frmbasarr + ";" + frmbasarrsa + ";" + Convert.ToString(lbldedfrmallo.Text) + ";;" + frmallow + ";" + proftax + ";" + gross_MinusLOP;
                }
                if (deduction.Trim() != "")
                    deduction = deduction + "\\";
            }
            if (grdlev.Rows.Count > 0)
            {
                for (int ro = 0; ro < grdlev.Rows.Count; ro++)
                {
                    string incsun = "";
                    string incholy = "";
                    string moncarry = "";
                    string yrcarry = "";
                    Label lbllev = (Label)grdlev.Rows[ro].FindControl("lbl_levtype");
                    Label lblyrlev = (Label)grdlev.Rows[ro].FindControl("lbl_yrlev");
                    Label lblmonlev = (Label)grdlev.Rows[ro].FindControl("lbl_monlev");
                    Label lblincsun = (Label)grdlev.Rows[ro].FindControl("lbl_incsunday");

                    if (lblincsun.Text == "Yes")
                        incsun = "1";
                    Label lblincholy = (Label)grdlev.Rows[ro].FindControl("lbl_incholiday");
                    if (lblincholy.Text == "Yes")
                        incholy = "1";
                    Label lblmoncarry = (Label)grdlev.Rows[ro].FindControl("lbl_moncarry");
                    if (lblmoncarry.Text == "Yes")
                        moncarry = "1";
                    Label lblyrcarry = (Label)grdlev.Rows[ro].FindControl("lbl_yrcarry");
                    if (lblyrcarry.Text == "Yes")
                        yrcarry = "1";
                    Label monthMaxLeave = (Label)grdlev.Rows[ro].FindControl("lbl_MonthlyMaxLeave");
                    if (monthMaxLeave.Text != "")
                    {
                        if (MonthlyMaxLeave == "")
                        {
                            MonthlyMaxLeave = Convert.ToString(lbllev.Text) + ":" + monthMaxLeave.Text;
                        }
                        else
                        {
                            MonthlyMaxLeave = MonthlyMaxLeave + "@" + Convert.ToString(lbllev.Text) + ":" + monthMaxLeave.Text;

                        }
                    }

                    if (leavetype.Trim() == "")
                        leavetype = Convert.ToString(lbllev.Text) + ";" + Convert.ToString(lblyrlev.Text) + ";" + Convert.ToString(lblmonlev.Text) + ";;" + incsun + ";" + incholy + ";" + moncarry + ";" + yrcarry + ";";
                    else
                        leavetype = leavetype + "\\" + Convert.ToString(lbllev.Text) + ";" + Convert.ToString(lblyrlev.Text) + ";" + Convert.ToString(lblmonlev.Text) + ";;" + incsun + ";" + incholy + ";" + moncarry + ";" + yrcarry + ";";
                }
                if (MonthlyMaxLeave.Trim() != "")
                    MonthlyMaxLeave = MonthlyMaxLeave + "@";
                if (leavetype.Trim() != "")
                    leavetype = leavetype + "\\";
            }
            double gradepay = 0;
            double basicpay = 0.0;
            double payband = 0.0;
            double ismpf = 0.0;
            double ismpfper = 0;
            string ismpfamnt = "";
            string isautogp = "";
            if (grdcom.Rows.Count > 0)
            {
                for (int ro = 0; ro < grdcom.Rows.Count; ro++)
                {
                    Label lblgrad = (Label)grdcom.Rows[ro].FindControl("lbl_grad");
                    Label lblbasic = (Label)grdcom.Rows[ro].FindControl("lbl_basicpay");
                    Label lblpayband = (Label)grdcom.Rows[ro].FindControl("lbl_payband");
                    Label lblismpf = (Label)grdcom.Rows[ro].FindControl("lbl_ismpf");
                    Label lblmpfper = (Label)grdcom.Rows[ro].FindControl("lbl_ismpfper");
                    Label lblismpfamnt = (Label)grdcom.Rows[ro].FindControl("lbl_ismpfamnt");
                    if (lblismpfamnt.Text == "Yes")
                        ismpfamnt = "1";
                    else
                        ismpfamnt = "0";
                    Label lblisautogp = (Label)grdcom.Rows[ro].FindControl("lbl_isautogp");
                    if (lblisautogp.Text == "Yes")
                        isautogp = "1";
                    else
                        isautogp = "0";
                    double.TryParse(lblgrad.Text, out gradepay);
                    double.TryParse(lblbasic.Text, out basicpay);
                    double.TryParse(lblpayband.Text, out payband);
                    double.TryParse(lblismpf.Text, out ismpf);
                    double.TryParse(lblmpfper.Text, out ismpfper);
                }
            }
            if (checkedOK())
            {
                string scode = "";
                string catcode = "";
                int inscount = 0;
                FpSpread.SaveChanges();
                for (int sco = 1; sco < FpSpread.Sheets[0].RowCount; sco++)
                {
                    byte Check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (Check == 1)
                    {
                        scode = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(sco), 2].Text);
                        catcode = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(sco), 6].Tag);
                        //string emppf = "0";
                        //string incstat = "";
                        //string isconsolid = "";
                        //string ismanual = "";
                        //string isdaily = "";
                        //string certtype = "";
                        //string timecategory = "0";
                        //string saltype = "0";
                        //string parea = "0";
                        //string dall = "0";
                        //string paymod = "0"; string BasicTyp = "0"; string Bas = "0"; string agp = "0"; string incramnt = "0"; string incrtime = "0"; string basinc = "0"; string bankacc = "0"; string isinc = "0";
                        string insquery = "if exists(select * from stafftrans where staff_code='" + scode + "' and latestrec='1' and category_code='" + catcode + "') Update stafftrans set allowances='" + allowance.Trim() + "',deductions='" + deduction.Trim() + "',grade_pay='" + Convert.ToString(gradepay) + "',bsalary='" + Convert.ToString(basicpay) + "',pay_band='" + Convert.ToString(payband) + "',IsMPFAmt='" + ismpfamnt + "',MPFAmount='" + Convert.ToString(ismpf) + "',MPFPer='" + Convert.ToString(ismpfper) + "',IsAutoGP='" + isautogp + "' where staff_code='" + scode + "' and latestrec='1' and category_code='" + catcode + "'";
                        //else Insert into stafftrans (Staff_code,allowances,deductions,grade_pay,bsalary,pay_band,IsMPFAmt,MPFAmount,MPFPer,latestrec,category_code,IsAutoGP,emp_pf,inc_status,IsConsolid,IsManualLOP,IsDailyWages,cert_type,time_category,saltype,pareaval,daallow,PayMode,BasicType,Basic,AGP,IncrementAmt,IncrementTime,BasicInc,BankAccType,Isincrement) Values ('" + scode + "','" + allowance.Trim() + "','" + deduction.Trim() + "','" + Convert.ToString(gradepay) + "','" + Convert.ToString(basicpay) + "','" + Convert.ToString(payband) + "','" + ismpfamnt + "','" + Convert.ToString(ismpf) + "','" + Convert.ToString(ismpfper) + "','1','" + catcode + "','" + isautogp + "','" + emppf + "','" + incstat + "','" + isconsolid + "','" + ismanual + "','" + isdaily + "','" + certtype + "','" + timecategory + "','" + saltype + "','" + parea + "','" + dall + "','" + paymod + "','" + BasicTyp + "','" + Bas + "','" + agp + "','" + incramnt + "','" + incrtime + "','" + basinc + "','" + bankacc + "','" + isinc + "')
                        insquery = insquery + " if exists(select * from individual_Leave_type where college_code='" + clgcode + "' and staff_code='" + scode + "' and category_code='" + catcode + "') Update individual_Leave_type set leavetype='" + leavetype.Trim() + "',MaxMonthlyLeave='" + MonthlyMaxLeave + "' where staff_code='" + scode + "' and college_code='" + clgcode + "' and category_code='" + catcode + "' else insert into individual_Leave_type (staff_code,leavetype,college_code,category_code,MaxMonthlyLeave) Values ('" + scode + "','" + leavetype.Trim() + "','" + clgcode + "','" + catcode + "','" + MonthlyMaxLeave + "')";
                        inscount = d2.update_method_wo_parameter(insquery, "Text");
                    }
                }
                if (inscount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Grade Pay Updated Successfully!";
                    //ViewState["strall"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, clgcode, "GradePayMaster.aspx");
        }
    }
    protected void cb_popallow_CheckedChange(object sender, EventArgs e)
    {
        if (cb_popallow.Checked == true)
        {
            allow_div.Visible = false;
            btn_addall.Visible = true;
            grd_all.Visible = true;
            divgrdall.Visible = true;
            //bindgridall();
        }
        else
        {
            allow_div.Visible = false;
            grd_all.Visible = false;
            divgrdall.Visible = false;
            btn_addall.Visible = false;
        }
    }
    protected void btnaddall_click(object sender, EventArgs e)
    {
        string lblallow = "";
        int count = 0;
        lbl_allowalert.Visible = false;
        allow_div.Visible = true;
        grd_all.Visible = false;
        divgrdall.Visible = false;
        btn_allowsave.Visible = true;
        btn_allowupdate.Visible = false;
        btn_allowdelete.Visible = false;
        txt_val.Enabled = true;
        cb_fromallallow.Enabled = false;//delsi
        cb_fromallallow.Checked = false;//delsi
        if (cbl_popallowance.Items.Count > 0)
        {
            for (int i = 0; i < cbl_popallowance.Items.Count; i++)
            {
                if (cbl_popallowance.Items[i].Selected == true)
                {
                    count++;
                    if (lblallow.Trim() == "")
                        lblallow = cbl_popallowance.Items[i].Text;
                    else
                        lblallow = lblallow + "," + cbl_popallowance.Items[i].Text;
                }
            }
        }
        if (count == cbl_popallowance.Items.Count)
            lbl_header1.Text = "Allowances -" + " " + "All";
        else
            lbl_header1.Text = "Allowances -" + " " + lblallow;
        allclear();
    }
    protected void btnaddded_click(object sender, EventArgs e)
    {
        lbl_dedalert.Visible = false;
        string lbldeduct = "";
        int count = 0;
        deduct_div.Visible = true;
        //popallowance1();
        grid_ded.Visible = false;
        divgrdded.Visible = false;
        btn_deductsave.Visible = true;
        btn_deductupdate.Visible = false;
        btn_deductdelete.Visible = false;
        txt_dval.Enabled = true;
        if (cbl_popdd.Items.Count > 0)
        {
            for (int i = 0; i < cbl_popdd.Items.Count; i++)
            {
                if (cbl_popdd.Items[i].Selected == true)
                {
                    count++;
                    if (lbldeduct.Trim() == "")
                        lbldeduct = cbl_popdd.Items[i].Text;
                    else
                        lbldeduct = lbldeduct + "," + cbl_popdd.Items[i].Text;
                }
            }
        }
        if (count == cbl_popdd.Items.Count)
            lbl_h2.Text = "Deductions -" + " " + "All";
        else
            lbl_h2.Text = "Deductions -" + " " + lbldeduct;
        dedclear();
    }
    protected void btnlevadd_click(object sender, EventArgs e)
    {
        GV1.Visible = false;
        lbl_ltypealert.Visible = false;
        string lbllev = "";
        int count = 0;
        ltype_div.Visible = true;
        grdlev.Visible = false;
        divgrdlev.Visible = false;
        btn_ltypesave.Visible = true;
        btn_ltypeupdate.Visible = false;
        btn_ltypedelete.Visible = false;
        if (cbl_poplt.Items.Count > 0)
        {
            for (int i = 0; i < cbl_poplt.Items.Count; i++)
            {
                if (cbl_poplt.Items[i].Selected == true)
                {
                    count++;
                    if (lbllev.Trim() == "")
                        lbllev = cbl_poplt.Items[i].Text;
                    else
                        lbllev = lbllev + "," + cbl_poplt.Items[i].Text;
                }
            }
        }
        if (count == cbl_poplt.Items.Count)
            lbl_h3.Text = "Leave Type -" + " " + "All";
        else
            lbl_h3.Text = "Leave Type -" + " " + lbllev;
        levclear();
    }
    protected void btnaddcom_click(object sender, EventArgs e)
    {
        lblerrco.Visible = false;
        divcommon.Visible = true;
        grdcom.Visible = false;
        divgrdcom.Visible = false;
        btnsavecom.Visible = true;
        btnupdatecom.Visible = false;
        btndelcom.Visible = false;
        txtismpf.Visible = false;
        txtismpfper.Visible = false;
        cbismpfamnt.Checked = false;
        lblismpf.Visible = false;
        lblismpfper.Visible = false;
        comclear();
    }
    protected void cb_fallow_CheckedChange(object sender, EventArgs e)//delsiref
    {
        if (cb_fallow.Checked == true)
        {
            divallhead.Visible = true;
            lblheaderr.Visible = false;
            lblheadset.Text = lbl_h2.Text;
            chkdeddisable();
            cb_ilop.Checked = true;
            cb_ilop.Enabled = true;
            cb_mcal.Checked = true;
            cb_mcal.Enabled = true;
            cb_fallow.Checked = true;
            cb_fallow.Enabled = true;
            radBtn_grosswithlop.Visible = true;
            radBtn_grosswithlop.Checked = false;

            allowance();
        }
        else
        {
            chkdedenable();
        }
        if (cb_fallow.Checked == false)
        {
            radBtn_grosswithlop.Visible = false;
        }
    }
    protected void cb_professionaltax_CheckedChanged(object sender, EventArgs e) /*poomalar 24.10.17*/
    {

    }
    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lb_selbasgrad.Items.Count > 0 && lb_selbasgrad.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_selallow.Items.Count; j++)
                {
                    if (lb_selallow.Items[j].Value == lb_selbasgrad.SelectedItem.Value)
                        ok = false;
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lb_selbasgrad.SelectedItem.Text, lb_selbasgrad.SelectedItem.Value);
                    lb_selallow.Items.Add(lst);
                }
            }
            bool nxtok = true;
            if (lb_allowhdr.Items.Count > 0 && lb_allowhdr.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_selallow.Items.Count; j++)
                {
                    if (lb_selallow.Items[j].Value == lb_allowhdr.SelectedItem.Value)
                        nxtok = false;
                }
                if (nxtok)
                {
                    ListItem lstnew = new ListItem(lb_allowhdr.SelectedItem.Text, lb_allowhdr.SelectedItem.Value);
                    lb_selallow.Items.Add(lstnew);
                }
            }
        }
        catch { }
    }
    protected void btnMvTwoRt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_selallow.Items.Clear();
            if (lb_selbasgrad.Items.Count > 0)
            {
                for (int j = 0; j < lb_selbasgrad.Items.Count; j++)
                {
                    lb_selallow.Items.Add(new ListItem(lb_selbasgrad.Items[j].Text.ToString(), lb_selbasgrad.Items[j].Value.ToString()));
                }
            }
            if (lb_allowhdr.Items.Count > 0)
            {
                for (int j = 0; j < lb_allowhdr.Items.Count; j++)
                {
                    lb_selallow.Items.Add(new ListItem(lb_allowhdr.Items[j].Text.ToString(), lb_allowhdr.Items[j].Value.ToString()));
                }
            }
        }
        catch { }
    }
    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        if (lb_selallow.Items.Count > 0 && lb_selallow.SelectedItem.Value != "")
            lb_selallow.Items.RemoveAt(lb_selallow.SelectedIndex);
    }
    protected void btnMvTwoLt_Click(object sender, EventArgs e)
    {
        lb_selallow.Items.Clear();
    }
    protected void btnokall_click(object sender, EventArgs e)
    {
        try
        {
            string getallval = "";
            if (lb_selallow.Items.Count > 0)
            {
                lblheaderr.Visible = false;
                for (int ro = 0; ro < lb_selallow.Items.Count; ro++)
                {
                    if (getallval.Trim() == "")
                        getallval = Convert.ToString(lb_selallow.Items[ro].Text);
                    else
                        getallval = getallval + "+" + Convert.ToString(lb_selallow.Items[ro].Text);
                }
                //ViewState["strall"] = getallval;
                txtcomded.Text = getallval;
                txtoverdedall.Text = getallval;
                divallhead.Visible = false;
            }
            else
            {
                lblheaderr.Visible = true;
                lblheaderr.Text = "Please select any one Header!";
            }
        }
        catch { }
    }
    protected void btnexitallow_click(object sender, EventArgs e)
    {
        divallhead.Visible = false;
    }
    protected void imgallhead_Click(object sender, EventArgs e)
    {
        divallhead.Visible = false;
    }
    protected void cb_popdeduct_CheckedChange(object sender, EventArgs e)
    {
        if (cb_popdeduct.Checked == true)
        {
            deduct_div.Visible = false;
            btnaddded.Visible = true;
            grid_ded.Visible = true;
            divgrdded.Visible = true;
            //bindgridded();
        }
        else
        {
            deduct_div.Visible = false;
            grid_ded.Visible = false;
            divgrdded.Visible = false;
            btnaddded.Visible = false;
        }
    }
    protected void cb_popltype_CheckedChange(object sender, EventArgs e)
    {
        if (cb_popltype.Checked == true)
        {
            ltype_div.Visible = false;
            btnlevadd.Visible = true;
            grdlev.Visible = true;
            divgrdlev.Visible = true;
            //bindgridlev();
        }
        else
        {
            ltype_div.Visible = false;
            grdlev.Visible = false;
            divgrdlev.Visible = false;
            btnlevadd.Visible = false;
        }
    }
    protected void chk_common_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_common.Checked)
            {
                divcommon.Visible = false;
                btnaddcom.Visible = true;
                grdcom.Visible = true;
                divgrdcom.Visible = true;
                //bindgridcom();
            }
            else
            {
                divcommon.Visible = false;
                divgrdcom.Visible = false;
                btnaddcom.Visible = false;
                grdcom.Visible = false;
            }
        }
        catch { }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
        //allowance
        txt_val.Text = "";
        cb_lop.Checked = false;
        cb_fbasic.Checked = false;
        cb_fbgp.Checked = false;
        cb_special.Checked = false;
        cb_agp.Checked = false;
        //Deduction
        txt_dval.Text = "";
        cb_fg.Checked = false;
        cb_fbda.Checked = false;
        cb_ilop.Checked = false;
        cb_fbgpda.Checked = false;
        cb_fb.Checked = false;
        cb_fbdp.Checked = false;
        cb_fp.Checked = false;
        cb_fbarr.Checked = false;
        cb_mcal.Checked = false;
        cb_fbas.Checked = false;
        cb_fallow.Checked = false;
        //Leave
        txt_yl.Text = "";
        txt_ml.Text = "";
        cb_sunday.Checked = false;
        cb_holiday.Checked = false;
        cb_mco.Checked = false;
        cb_yco.Checked = false;
    }
    public bool checkedOK()
    {
        bool Ok = false;
        FpSpread.SaveChanges();
        for (int i = 1; i < FpSpread.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
                Ok = true;
        }
        return Ok;
    }
    public void btn_gp_Click(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        radBtn_grosswithlop.Visible = false;
        if (checkedOK())
        {
            poperrjs.Visible = true;
            popallowance();
            popdeduction();
            popLeave();
            bindgridall();
            bindgridded();
            bindgridlev();
            bindgridcom();
            grd_all.Visible = true;
            divgrdall.Visible = true;
            grdlev.Visible = true;
            divgrdlev.Visible = true;
            grid_ded.Visible = true;
            divgrdded.Visible = true;
            divgrdcom.Visible = true;
            grdcom.Visible = true;
            btn_addall.Visible = true;
            btnaddded.Visible = true;
            btnlevadd.Visible = true;
            btnaddcom.Visible = true;
            cb_popallow.Checked = true;
            cb_popdeduct.Checked = true;
            cb_popltype.Checked = true;
            chk_common.Checked = true;
            allow_div.Visible = false;
            divcommon.Visible = false;
            deduct_div.Visible = false;
            ltype_div.Visible = false;
            lbl_alert.Visible = false;
            rdbmainind.Checked = true;
            rdbmainoverall.Checked = false;
            ddloverall.Enabled = false;
            divind.Visible = true;
            divoverall.Visible = false;
        }
        else
        {
            lbl_alert.Visible = true;
            lbl_alert.Text = "Please Select any one Staff! ";
        }
    }
    protected void imagebtnpopcloseadd_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
        btn_go_Click(sender, e);
    }
    protected void popallowance()
    {
        try
        {
            ds.Clear();
            cbl_popallowance.Items.Clear();
            string item = "select allowances  from incentives_master where college_code='" + clgcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_popallowance.DataSource = ds;
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    if (split1.Length > 1)
                    {
                        string stafftype = split1[0];
                        cbl_popallowance.Items.Add(stafftype);
                    }
                }
                if (cbl_popallowance.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_popallowance.Items.Count; i++)
                    {
                        cbl_popallowance.Items[i].Selected = true;
                    }
                    txt_popallow.Text = "Allowance (" + cbl_popallowance.Items.Count + ")";
                    cb_popallowance.Checked = true;
                }
            }
            else
            {
                txt_popallow.Text = "--Select--";
                cb_popallowance.Checked = false;
            }
        }
        catch { }
    }
    protected void popdeduction()
    {
        try
        {
            ds.Clear();
            cbl_popdd.Items.Clear();
            string item = "select deductions from incentives_master where college_code='" + clgcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string st = Convert.ToString(ds.Tables[0].Rows[0]["deductions"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    if (split1.Length > 1)
                    {
                        string stafftyp = split1[1];
                        string stafftype = split1[0];
                        cbl_popdd.Items.Add(new ListItem(stafftype, stafftyp));
                    }
                }
                if (cbl_popdd.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_popdd.Items.Count; i++)
                    {
                        cbl_popdd.Items[i].Selected = true;
                    }
                    txt_popdeduct.Text = "Deduction (" + cbl_popdd.Items.Count + ")";
                    cb_popdd.Checked = true;
                }
            }
            else
            {
                txt_popdeduct.Text = "--Select--";
                cb_popdd.Checked = false;
            }
        }
        catch { }
    }
    protected void popLeave()
    {
        try
        {
            ds.Clear();
            cbl_poplt.Items.Clear();
            string college = ddlcollege.SelectedValue;
            string item = "select category,shortname  from leave_category  where college_code = '" + clgcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_poplt.DataSource = ds;
                cbl_poplt.DataTextField = "category";
                cbl_poplt.DataValueField = "shortname";
                cbl_poplt.DataBind();
                if (cbl_poplt.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_poplt.Items.Count; i++)
                    {
                        cbl_poplt.Items[i].Selected = true;
                    }
                    txt_popltype.Text = "LeaveType (" + cbl_poplt.Items.Count + ")";
                    cb_poplt.Checked = true;
                }
            }
            else
            {
                txt_popltype.Text = "--Select--";
                cb_poplt.Checked = false;
            }
        }
        catch { }
    }
    protected void cb_popallowance_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_popallowance, cbl_popallowance, txt_popallow, "Allowance");
    }
    protected void cbl_popallowance_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_popallowance, cbl_popallowance, txt_popallow, "Allowance");
    }
    protected void cb_popdd_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_popdd, cbl_popdd, txt_popdeduct, "Deduction");
    }
    protected void cbl_popdd_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_popdd, cbl_popdd, txt_popdeduct, "Deduction");
    }
    protected void cb_poplt_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_poplt, cbl_poplt, txt_popltype, "LeaveType");
    }
    protected void cbl_poplt_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_poplt, cbl_poplt, txt_popltype, "LeaveType");
    }
    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.GridView)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[1].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }
        return rownumber;
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
    private string GetSelectedItemsTextnew(CheckBoxList cblSelected)
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
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[j].Text));
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
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }
    private DataTable dtallheader(DataTable getdt)
    {
        getdt.Columns.Add("alltype");
        getdt.Columns.Add("mode");
        getdt.Columns.Add("value");
        getdt.Columns.Add("inclop");
        getdt.Columns.Add("frmbasic");
        getdt.Columns.Add("frmbasgp");
        getdt.Columns.Add("isspl");
        getdt.Columns.Add("frmbasagp");
        getdt.Columns.Add("roundval");
        getdt.Columns.Add("FromAllow");
        return getdt;
    }
    private DataTable dtdedheader(DataTable getdt)
    {
        getdt.Columns.Add("dedtype");
        getdt.Columns.Add("mode");
        getdt.Columns.Add("value");
        getdt.Columns.Add("dedround");
        getdt.Columns.Add("frmgross");
        getdt.Columns.Add("frmbasicda");
        getdt.Columns.Add("inclop");
        getdt.Columns.Add("frmbasgpda");
        getdt.Columns.Add("frmbas");
        getdt.Columns.Add("frmbasdp");
        getdt.Columns.Add("frmpetty");
        getdt.Columns.Add("frmbasarr");
        getdt.Columns.Add("ismaxcal");
        getdt.Columns.Add("maxamnt");
        getdt.Columns.Add("dedamt");
        getdt.Columns.Add("frmbasarrsa");
        getdt.Columns.Add("frmallow");
        getdt.Columns.Add("frmnetamnt");
        getdt.Columns.Add("GrossLOP");
        getdt.Columns.Add("GrossMinusLOP");
        return getdt;
    }
    private DataTable dtltypeheader(DataTable getdt)
    {
        getdt.Columns.Add("levtype");
        getdt.Columns.Add("yrlev");
        getdt.Columns.Add("monlev");
        getdt.Columns.Add("incsunday");
        getdt.Columns.Add("incholiday");
        getdt.Columns.Add("moncarry");
        getdt.Columns.Add("yrcarry");
        getdt.Columns.Add("MonthlyMaxLeave");//delsi2912
        return getdt;
    }
    private DataTable dtcomheader(DataTable getdt)
    {
        getdt.Columns.Add("gradepay");
        getdt.Columns.Add("basicpay");
        getdt.Columns.Add("payband");
        getdt.Columns.Add("ismpf");
        getdt.Columns.Add("ismpfper");
        getdt.Columns.Add("ismpfamnt");
        getdt.Columns.Add("isautogp");
        return getdt;
    }
    private string getoverallallow()
    {
        string getoverallall = "";
        string value1 = "";
        string commonval = "";
        try
        {
            for (int i = 0; i < cbl_popallowance.Items.Count; i++)
            {
                if (cbl_popallowance.Items[i].Selected == true)
                {
                    string value = cbl_popallowance.Items[i].Text;
                    string round = "";
                    if (ddl_round.SelectedIndex != 0)
                        round = Convert.ToString(ddl_round.SelectedItem);
                    for (int j = 0; j < ddl_mode.Items.Count; j++)
                    {
                        if (ddl_mode.Items[j].Selected == true)
                            value1 = Convert.ToString(ddl_mode.Items[j].Text);
                    }
                    string value2 = txt_val.Text;
                    if (value2.Trim() == "")
                        value2 = "0";
                    string[] splval = new string[2];
                    string firstval = "";
                    string secondval = "";
                    Double val2 = 0;
                    if (value2.Trim() != "")
                    {
                        splval = value2.Split('.');
                        if (splval.Length > 0)
                        {
                            firstval = Convert.ToString(splval[0]);
                            if (value2.Contains('.'))
                            {
                                secondval = Convert.ToString(splval[1]);
                                if (secondval.Length > 0)
                                {
                                    if (round == ">=5")
                                    {
                                        if (secondval.Length >= 5)
                                            val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                        else
                                            val2 = Convert.ToDouble(value2);
                                    }
                                    else if (round == "<=5")
                                    {
                                        if (secondval.Length <= 5)
                                            val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                        else
                                            val2 = Convert.ToDouble(value2);
                                    }
                                    else if (round == ">=1")
                                    {
                                        if (secondval.Length >= 1)
                                            val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                        else
                                            val2 = Convert.ToDouble(value2);
                                    }
                                    else if (round == "=1")
                                    {
                                        if (secondval.Length == 1)
                                            val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                        else
                                            val2 = Convert.ToDouble(value2);
                                    }
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                            }
                            else
                                val2 = Convert.ToDouble(value2);
                        }
                    }
                    commonval = Convert.ToString(txt_all_allowVal.Text);//delsi0405
                    string lop = "";
                    if (cb_lop.Checked == true)
                        lop = "1";
                    string basic = "";
                    if (cb_fbasic.Checked == true)
                        basic = "1";
                    string bgp = "";
                    if (cb_fbgp.Checked == true)
                        bgp = "1";
                    string special = "";
                    if (cb_special.Checked == true)
                        special = "1";
                    string agp = "";
                    if (cb_agp.Checked == true)
                        agp = "1";

                    if (!getoverallall.Contains(value))
                    {
                        if (getoverallall.Trim() == "")
                            getoverallall = value + ";" + value1 + ";" + val2 + ";;" + lop + ";;" + basic + ";;" + bgp + ";" + special + ";" + agp + ";" + round + ";;" + commonval + ";";
                        else
                            getoverallall = getoverallall + "\\" + value + ";" + value1 + ";" + val2 + ";;" + lop + ";;" + basic + ";;" + bgp + ";" + special + ";" + agp + ";" + round + ";;" + commonval + ";";//delsi
                    }
                }
            }
        }
        catch { }
        return getoverallall;
    }
    private string getoverallded()
    {
        string overallded = "";
        string mode1 = "";
        string commonval = "";
        try
        {
            for (int i = 0; i < cbl_popdd.Items.Count; i++)
            {
                if (cbl_popdd.Items[i].Selected == true)
                {
                    for (int j = 0; j < ddl_dmode.Items.Count; j++)
                    {
                        if (ddl_dmode.Items[j].Selected == true)
                            mode1 = Convert.ToString(ddl_dmode.Items[j].Text);
                    }
                    string round = Convert.ToString(ddl_rt.SelectedItem);
                    string value = cbl_popdd.Items[i].Text;
                    string value2 = txt_dval.Text;
                    if (value2 == "")
                        value2 = "0";
                    string[] splval = new string[2];
                    string firstval = "";
                    string secondval = "";
                    string proTax = string.Empty;
                    Double val2 = 0;
                    if (value2.Trim() != "")
                    {
                        splval = value2.Split('.');
                        if (splval.Length > 0)
                        {
                            firstval = Convert.ToString(splval[0]);
                            if (value2.Contains('.'))
                            {
                                secondval = Convert.ToString(splval[1]);
                                if (secondval.Length > 0)
                                {
                                    if (round == ">=5")
                                    {
                                        if (secondval.Length >= 5)
                                            val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                        else
                                            val2 = Convert.ToDouble(value2);
                                    }
                                    else if (round == "<=5")
                                    {
                                        if (secondval.Length <= 5)
                                            val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                        else
                                            val2 = Convert.ToDouble(value2);
                                    }
                                    else if (round == ">=1")
                                    {
                                        if (secondval.Length >= 1)
                                            val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                        else
                                            val2 = Convert.ToDouble(value2);
                                    }
                                    else if (round == "=1")
                                    {
                                        if (secondval.Length == 1)
                                            val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                        else
                                            val2 = Convert.ToDouble(value2);
                                    }
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                            }
                            else
                                val2 = Convert.ToDouble(value2);
                        }
                    }
                    string maxamt = txt_mamt.Text;
                    string damt = txt_damt.Text;
                    //commonval = Convert.ToString(ViewState["strall"]);
                    commonval = Convert.ToString(txtcomded.Text);
                    string fgross = "";
                    if (cb_fg.Checked == true)
                        fgross = "1";
                    string fbda = "";
                    if (cb_fbda.Checked == true)
                        fbda = "1";
                    string ilop = "";
                    if (cb_ilop.Checked == true)
                        ilop = "1";
                    string fbgpda = "";
                    if (cb_fbgpda.Checked == true)
                        fbgpda = "1";
                    string fbasic = "";
                    if (cb_fb.Checked == true)
                        fbasic = "1";
                    string fbdp = "";
                    if (cb_fbdp.Checked == true)
                        fbdp = "1";
                    string fp = "";
                    if (cb_fp.Checked == true)
                        fp = "1";
                    string fbarr = "";
                    if (cb_fbarr.Checked == true)
                        fbarr = "1";
                    string maxcal = "";
                    if (cb_mcal.Checked == true)
                        maxcal = "1";
                    string fbarrs = "";
                    if (cb_fbas.Checked == true)
                        fbarrs = "1";
                    string fallow = "";
                    if (rb_frmnet.Checked == true)
                        fallow = "1";
                    if (cb_professionaltax.Checked)
                        proTax = "1";
                    string fromgrossWithlop = string.Empty;
                    if (radBtn_grosswithlop.Checked)//delsi1604
                        fromgrossWithlop = "1";


                    if (!overallded.Contains(value))
                    {
                        if (overallded.Trim() == "")
                            overallded = value + ";" + mode1 + ";" + val2 + ";" + fgross + ";" + fbda + ";;" + ilop + ";" + fbgpda + ";" + fbasic + ";" + fbdp + ";" + fp + ";" + round + ";" + maxcal + ";" + maxamt + ";" + damt + ";" + fbarr + ";" + fbarrs + ";" + commonval + ";;" + fallow + ";" + proTax + ";" + fromgrossWithlop;//poo
                        else
                            overallded = overallded + "\\" + value + ";" + mode1 + ";" + val2 + ";" + fgross + ";" + fbda + ";;" + ilop + ";" + fbgpda + ";" + fbasic + ";" + fbdp + ";" + fp + ";" + round + ";" + maxcal + ";" + maxamt + ";" + damt + ";" + fbarr + ";" + fbarrs + ";" + commonval + ";;" + fallow + ";" + proTax + ";" + fromgrossWithlop;//poo
                    }
                }
            }
        }
        catch { }
        return overallded;
    }
    private string getoveralllev()
    {
        string overallleave = "";
        try
        {
            for (int i = 0; i < cbl_poplt.Items.Count; i++)
            {
                if (cbl_poplt.Items[i].Selected == true)
                {
                    string value = cbl_poplt.Items[i].Text;
                    string yleave = txt_yl.Text;
                    string mleave = txt_ml.Text;
                    string sunday = "";
                    if (cb_sunday.Checked == true)
                        sunday = "1";
                    string holiday = "";
                    if (cb_holiday.Checked == true)
                        holiday = "1";
                    string mco = "";
                    if (cb_mco.Checked == true)
                        mco = "1";
                    string yco = "";
                    if (cb_yco.Checked == true)
                        yco = "1";
                    string maxMonthLeave = string.Empty;
                    if (GV1.Rows.Count > 0)
                    {

                        foreach (GridViewRow grid in GV1.Rows)
                        {
                            string GetMonth = (grid.FindControl("lblmonth") as Label).Text;
                            string Maxmonth = (grid.FindControl("ddlmaxleave") as DropDownList).Text;
                            string getFromdate = (grid.FindControl("txtfromdate") as TextBox).Text;
                            string getTodate = (grid.FindControl("txttodate") as TextBox).Text;
                            if (Maxmonth != "Select")
                            {
                                if (maxMonthLeave == "")
                                {
                                    maxMonthLeave = GetMonth + "-" + Maxmonth + "-" + getFromdate + "-" + getTodate;
                                }
                                else
                                {
                                    maxMonthLeave = maxMonthLeave + "+" + GetMonth + "-" + Maxmonth + "-" + getFromdate + "-" + getTodate;
                                }
                            }

                        }
                    }
                    if (!overallleave.Contains(value))
                    {
                        if (overallleave == "")
                            overallleave = value + ";" + yleave + ";" + mleave + ";" + sunday + ";" + holiday + ";" + mco + ";" + yco + maxMonthLeave;
                        else
                            overallleave = overallleave + "\\" + value + ";" + yleave + ";" + mleave + ";" + sunday + ";" + holiday + ";" + mco + ";" + yco + maxMonthLeave;
                    }
                }
            }
        }
        catch { }
        return overallleave;
    }
    #region Add Overall Allowance,Deduction,Leave Type and Common
    private DataTable dtoverallheader(DataTable getdt)
    {
        getdt.Columns.Add("overalltype");
        getdt.Columns.Add("overallmode");
        getdt.Columns.Add("overallvalue");
        getdt.Columns.Add("overallinclop");
        getdt.Columns.Add("overallfrmbasic");
        getdt.Columns.Add("overallfrmbasgp");
        getdt.Columns.Add("overallisspl");
        getdt.Columns.Add("overallfrmbasagp");
        getdt.Columns.Add("overallroundval");
        return getdt;
    }
    private DataTable dtoverdedheader(DataTable getdt)
    {
        getdt.Columns.Add("overdedtype");
        getdt.Columns.Add("overdedmode");
        getdt.Columns.Add("overdedvalue");
        getdt.Columns.Add("overdedround");
        getdt.Columns.Add("overdedfrmgross");
        getdt.Columns.Add("overdedfrmbasicda");
        getdt.Columns.Add("overdedinclop");
        getdt.Columns.Add("overdedfrmbasgpda");
        getdt.Columns.Add("overdedfrmbas");
        getdt.Columns.Add("overdedfrmbasdp");
        getdt.Columns.Add("overdedfrmpetty");
        getdt.Columns.Add("overdedfrmbasarr");
        getdt.Columns.Add("overdedismaxcal");
        getdt.Columns.Add("overdedmaxamnt");
        getdt.Columns.Add("overdedamt");
        getdt.Columns.Add("overdedfrmbasarrsa");
        getdt.Columns.Add("overdedfrmallow");
        getdt.Columns.Add("overdedfrmnetamnt");
        return getdt;
    }
    private DataTable dtoverltypeheader(DataTable getdt)
    {
        getdt.Columns.Add("overlevtype");
        getdt.Columns.Add("overyrlev");
        getdt.Columns.Add("overmonlev");
        getdt.Columns.Add("overincsunday");
        getdt.Columns.Add("overincholiday");
        getdt.Columns.Add("overmoncarry");
        getdt.Columns.Add("overyrcarry");
        return getdt;
    }
    private DataTable dtovercomheader(DataTable getdt)
    {
        getdt.Columns.Add("overgradepay");
        getdt.Columns.Add("overbasicpay");
        getdt.Columns.Add("overpayband");
        getdt.Columns.Add("overismpf");
        getdt.Columns.Add("overismpfper");
        getdt.Columns.Add("overismpfamnt");
        getdt.Columns.Add("overisautogp");
        return getdt;
    }
    private string getoverallallowoverall()
    {
        string getoverallall = "";
        string value1 = "";
        try
        {
            if (ddloverallallow.SelectedItem.Text.Trim() != "Select")
            {
                string value = ddloverallallow.SelectedItem.Text;
                string round = "";
                if (ddlroundoverall.SelectedIndex != 0)
                    round = Convert.ToString(ddlroundoverall.SelectedItem);
                for (int j = 0; j < ddloverallmode.Items.Count; j++)
                {
                    if (ddloverallmode.Items[j].Selected == true)
                        value1 = Convert.ToString(ddloverallmode.Items[j].Text);
                }
                string value2 = txtoverallval.Text;
                if (value2.Trim() == "")
                    value2 = "0";
                string[] splval = new string[2];
                string firstval = "";
                string secondval = "";
                Double val2 = 0;
                if (value2.Trim() != "")
                {
                    splval = value2.Split('.');
                    if (splval.Length > 0)
                    {
                        firstval = Convert.ToString(splval[0]);
                        if (value2.Contains('.'))
                        {
                            secondval = Convert.ToString(splval[1]);
                            if (secondval.Length > 0)
                            {
                                if (round == ">=5")
                                {
                                    if (secondval.Length >= 5)
                                        val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                                else if (round == "<=5")
                                {
                                    if (secondval.Length <= 5)
                                        val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                                else if (round == ">=1")
                                {
                                    if (secondval.Length >= 1)
                                        val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                                else if (round == "=1")
                                {
                                    if (secondval.Length == 1)
                                        val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                                else
                                    val2 = Convert.ToDouble(value2);
                            }
                        }
                        else
                            val2 = Convert.ToDouble(value2);
                    }
                }
                string lop = "";
                if (cbinclopoverall.Checked == true)
                    lop = "1";
                string basic = "";
                if (rbfrmbasoverall.Checked == true)
                    basic = "1";
                string bgp = "";
                if (rbfrmbasgpoverall.Checked == true)
                    bgp = "1";
                string special = "";
                if (cbissploverall.Checked == true)
                    special = "1";
                string agp = "";
                if (rbfrmbasagp.Checked == true)
                    agp = "1";
                if (!getoverallall.Contains(value))
                {
                    if (getoverallall.Trim() == "")
                        getoverallall = value + ";" + value1 + ";" + val2 + ";;" + lop + ";;" + basic + ";;" + bgp + ";" + special + ";" + agp + ";" + round + ";;";
                    else
                        getoverallall = getoverallall + "\\" + value + ";" + value1 + ";" + val2 + ";;" + lop + ";;" + basic + ";;" + bgp + ";" + special + ";" + agp + ";" + round + ";;";
                }
            }
        }
        catch { }
        return getoverallall;
    }
    private string getoveralldedoverall()
    {
        string overallded = "";
        string mode1 = "";
        string commonval = "";
        try
        {
            if (ddloverallded.SelectedItem.Text.Trim() != "Select")
            {
                for (int j = 0; j < ddloverdedmode.Items.Count; j++)
                {
                    if (ddloverdedmode.Items[j].Selected == true)
                        mode1 = Convert.ToString(ddloverdedmode.Items[j].Text);
                }
                string round = Convert.ToString(ddloverdedround.SelectedItem);
                string value = ddloverallded.SelectedItem.Text;
                string value2 = txtoverdedval.Text;
                if (value2 == "")
                    value2 = "0";
                string[] splval = new string[2];
                string firstval = "";
                string secondval = "";
                Double val2 = 0;
                if (value2.Trim() != "")
                {
                    splval = value2.Split('.');
                    if (splval.Length > 0)
                    {
                        firstval = Convert.ToString(splval[0]);
                        if (value2.Contains('.'))
                        {
                            secondval = Convert.ToString(splval[1]);
                            if (secondval.Length > 0)
                            {
                                if (round == ">=5")
                                {
                                    if (secondval.Length >= 5)
                                        val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                                else if (round == "<=5")
                                {
                                    if (secondval.Length <= 5)
                                        val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                                else if (round == ">=1")
                                {
                                    if (secondval.Length >= 1)
                                        val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                                else if (round == "=1")
                                {
                                    if (secondval.Length == 1)
                                        val2 = Math.Round(Convert.ToDouble(value2), 0, MidpointRounding.AwayFromZero);
                                    else
                                        val2 = Convert.ToDouble(value2);
                                }
                                else
                                    val2 = Convert.ToDouble(value2);
                            }
                        }
                        else
                            val2 = Convert.ToDouble(value2);
                    }
                }
                string maxamt = txtoverdedmaxamnt.Text;
                string damt = txtoverdeddedamnt.Text;
                commonval = Convert.ToString(txtoverdedall.Text);
                string fgross = "";
                if (rdboverdedfrmgross.Checked == true)
                    fgross = "1";
                string fbda = "";
                if (rdboverdedfrmbasda.Checked == true)
                    fbda = "1";
                string ilop = "";
                if (cbinclopoverded.Checked == true)
                    ilop = "1";
                string fbgpda = "";
                if (rdboverdedfrmbasgpda.Checked == true)
                    fbgpda = "1";
                string fbasic = "";
                if (rdboverdedfrmbas.Checked == true)
                    fbasic = "1";
                string fbdp = "";
                if (rdboverdedfrmbasdp.Checked == true)
                    fbdp = "1";
                string fp = "";
                if (rdboverdedfrmpetty.Checked == true)
                    fp = "1";
                string fbarr = "";
                if (rdboverdedfrmbasarr.Checked == true)
                    fbarr = "1";
                string maxcal = "";
                if (cbmaxcaloverded.Checked == true)
                    maxcal = "1";
                string fbarrs = "";
                if (rdboverdedfrmbasarrsa.Checked == true)
                    fbarrs = "1";
                string fallow = "";
                if (rdboverdedfrmnet.Checked == true)
                    fallow = "1";
                if (!overallded.Contains(value))
                {
                    if (overallded.Trim() == "")
                        overallded = value + ";" + mode1 + ";" + val2 + ";" + fgross + ";" + fbda + ";;" + ilop + ";" + fbgpda + ";" + fbasic + ";" + fbdp + ";" + fp + ";" + round + ";" + maxcal + ";" + maxamt + ";" + damt + ";" + fbarr + ";" + fbarrs + ";" + commonval + ";;" + fallow;
                    else
                        overallded = overallded + "\\" + value + ";" + mode1 + ";" + val2 + ";" + fgross + ";" + fbda + ";;" + ilop + ";" + fbgpda + ";" + fbasic + ";" + fbdp + ";" + fp + ";" + round + ";" + maxcal + ";" + maxamt + ";" + damt + ";" + fbarr + ";" + fbarrs + ";" + commonval + ";;" + fallow;
                }
            }
        }
        catch { }
        return overallded;
    }
    private string getoveralllevoverall()
    {
        string overallleave = "";
        try
        {
            if (ddloverlev.SelectedItem.Text.Trim() != "Select")
            {
                string value = Convert.ToString(ddloverlev.SelectedItem.Text);
                string yleave = txtoveryrlev.Text;
                string mleave = txtovermonlev.Text;
                string sunday = "";
                if (cboversuninc.Checked == true)
                    sunday = "1";
                string holiday = "";
                if (cboverholinc.Checked == true)
                    holiday = "1";
                string mco = "";
                if (cbovermonco.Checked == true)
                    mco = "1";
                string yco = "";
                if (cboveryrco.Checked == true)
                    yco = "1";


                if (!overallleave.Contains(value))
                {
                    if (overallleave == "")
                        overallleave = value + ";" + yleave + ";" + mleave + ";" + sunday + ";" + holiday + ";" + mco + ";" + yco;
                    else
                        overallleave = overallleave + "\\" + value + ";" + yleave + ";" + mleave + ";" + sunday + ";" + holiday + ";" + mco + ";" + yco;
                }
            }
        }
        catch { }
        return overallleave;
    }
    protected void overallallowance()
    {
        try
        {
            ds.Clear();
            ddloverallallow.Items.Clear();
            string item = "select allowances  from incentives_master where college_code='" + clgcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    if (split1.Length > 1)
                    {
                        string stafftype = split1[0];
                        ddloverallallow.Items.Add(stafftype);
                    }
                }
                ddloverallallow.DataBind();
                ddloverallallow.Items.Insert(0, "Select");
            }
            else
            {
                ddloverallallow.Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    protected void overalldeduction()
    {
        try
        {
            ds.Clear();
            ddloverallded.Items.Clear();
            string item = "select deductions from incentives_master where college_code='" + clgcode + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string st = Convert.ToString(ds.Tables[0].Rows[0]["deductions"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    if (split1.Length > 1)
                    {
                        string stafftyp = split1[1];
                        string stafftype = split1[0];
                        ddloverallded.Items.Add(new ListItem(stafftype, stafftyp));
                    }
                }
                ddloverallded.DataBind();
                ddloverallded.Items.Insert(0, "Select");
            }
            else
            {
                ddloverallded.Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    protected void overallLeave()
    {
        try
        {
            ds.Clear();
            ddloverlev.Items.Clear();
            string college = ddlcollege.SelectedValue;
            string item = "select category,shortname  from leave_category  where college_code = '" + college + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddloverlev.DataSource = ds;
                ddloverlev.DataTextField = "category";
                ddloverlev.DataValueField = "shortname";
                ddloverlev.DataBind();
                ddloverlev.Items.Insert(0, "Select");
            }
            else
            {
                ddloverlev.Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    protected void rdbmainind_change(object sender, EventArgs e)
    {
        divind.Visible = true;
        divoverall.Visible = false;
        btn_gp_Click(sender, e);
        ddloverall.Enabled = false;
    }
    protected void ddloverall_change(object sender, EventArgs e)
    {
        allowover_div.Visible = false;
        dedover_div.Visible = false;
        ltype_overlevdiv.Visible = false;
        divovercom.Visible = false;
        divoverdedgrd.Visible = false;
        grdoverded.Visible = false;
        divoverallgrd.Visible = false;
        grdoverall.Visible = false;
        divoveralllev.Visible = false;
        grdoveralllev.Visible = false;
        divovergrdcom.Visible = false;
        grdovercom.Visible = false;
        com_err.Visible = false;
        if (ddloverall.SelectedItem.Value == "0")
        {
            if (grdoverded.Rows.Count == 0 && grdoveralllev.Rows.Count == 0 && grdovercom.Rows.Count == 0)
            {
                lblovercom.Visible = false;
                lbloverallow.Visible = true;
                ddloverallallow.Visible = true;
                lbloverded.Visible = false;
                ddloverallded.Visible = false;
                lbloverleav.Visible = false;
                ddloverlev.Visible = false;
                overallallowance();
                divoverallgrd.Visible = true;
                grdoverall.Visible = true;
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
                divovergrdcom.Visible = false;
                grdovercom.Visible = false;
                lblerroverall.Visible = false;
            }
            else
            {
                if (grdoverded.Rows.Count > 0)
                {
                    divoverdedgrd.Visible = true;
                    grdoverded.Visible = true;
                    ddloverall.SelectedIndex = 1;
                    com_err.Visible = true;
                    com_err.Text = "Deduction Details Already Exist!";
                }
                if (grdoveralllev.Rows.Count > 0)
                {
                    grdoveralllev.Visible = true;
                    divoveralllev.Visible = true;
                    ddloverall.SelectedIndex = 2;
                    com_err.Visible = true;
                    com_err.Text = "Leave Details Already Exist!";
                }
                if (grdovercom.Rows.Count > 0)
                {
                    divovergrdcom.Visible = true;
                    grdovercom.Visible = true;
                    ddloverall.SelectedIndex = 3;
                    com_err.Visible = true;
                    com_err.Text = "Common Details Already Exist!";
                }
            }
        }
        else if (ddloverall.SelectedItem.Value == "1")
        {
            if (grdoverall.Rows.Count == 0 && grdoveralllev.Rows.Count == 0 && grdovercom.Rows.Count == 0)
            {
                lblovercom.Visible = false;
                lbloverded.Visible = true;
                ddloverallded.Visible = true;
                lbloverallow.Visible = false;
                ddloverallallow.Visible = false;
                lbloverleav.Visible = false;
                ddloverlev.Visible = false;
                overalldeduction();
                divoverdedgrd.Visible = true;
                grdoverded.Visible = true;
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
                divovergrdcom.Visible = false;
                grdovercom.Visible = false;
                lbloverdederr.Visible = false;
            }
            else
            {
                if (grdoverall.Rows.Count > 0)
                {
                    divoverallgrd.Visible = true;
                    grdoverall.Visible = true;
                    ddloverall.SelectedIndex = 0;
                    com_err.Visible = true;
                    com_err.Text = "Allowance Details Already Exist!";
                }
                if (grdoveralllev.Rows.Count > 0)
                {
                    grdoveralllev.Visible = true;
                    divoveralllev.Visible = true;
                    ddloverall.SelectedIndex = 2;
                    com_err.Visible = true;
                    com_err.Text = "Leave Details Already Exist!";
                }
                if (grdovercom.Rows.Count > 0)
                {
                    divovergrdcom.Visible = true;
                    grdovercom.Visible = true;
                    ddloverall.SelectedIndex = 3;
                    com_err.Visible = true;
                    com_err.Text = "Common Details Already Exist!";
                }
            }
        }
        else if (ddloverall.SelectedItem.Value == "2")
        {
            if (grdoverall.Rows.Count == 0 && grdoverded.Rows.Count == 0 && grdovercom.Rows.Count == 0)
            {
                lblovercom.Visible = false;
                lbloverleav.Visible = true;
                ddloverlev.Visible = true;
                lbloverded.Visible = false;
                ddloverallded.Visible = false;
                lbloverallow.Visible = false;
                ddloverallallow.Visible = false;
                overallLeave();
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
                divoveralllev.Visible = true;
                grdoveralllev.Visible = true;
                divovergrdcom.Visible = false;
                grdovercom.Visible = false;
                lbloverleverr.Visible = false;
            }
            else
            {
                if (grdoverall.Rows.Count > 0)
                {
                    divoverallgrd.Visible = true;
                    grdoverall.Visible = true;
                    ddloverall.SelectedIndex = 0;
                    com_err.Visible = true;
                    com_err.Text = "Allowance Details Already Exist!";
                }
                if (grdoverded.Rows.Count > 0)
                {
                    grdoverded.Visible = true;
                    divoverdedgrd.Visible = true;
                    ddloverall.SelectedIndex = 1;
                    com_err.Visible = true;
                    com_err.Text = "Deduction Details Already Exist!";
                }
                if (grdovercom.Rows.Count > 0)
                {
                    divovergrdcom.Visible = true;
                    grdovercom.Visible = true;
                    ddloverall.SelectedIndex = 3;
                    com_err.Visible = true;
                    com_err.Text = "Common Details Already Exist!";
                }
            }
        }
        else if (ddloverall.SelectedItem.Value == "3")
        {
            if (grdoverall.Rows.Count == 0 && grdoverded.Rows.Count == 0 && grdoveralllev.Rows.Count == 0)
            {
                lblovercom.Visible = true;
                lbloverleav.Visible = false;
                ddloverlev.Visible = false;
                lbloverded.Visible = false;
                ddloverallded.Visible = false;
                lbloverallow.Visible = false;
                ddloverallallow.Visible = false;
                divovercom.Visible = true;
                overcomclear();
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
                lblovercomerr.Visible = false;
            }
            else
            {
                if (grdoverall.Rows.Count > 0)
                {
                    divoverallgrd.Visible = true;
                    grdoverall.Visible = true;
                    ddloverall.SelectedIndex = 0;
                    com_err.Visible = true;
                    com_err.Text = "Allowance Details Already Exist!";
                }
                if (grdoverded.Rows.Count > 0)
                {
                    grdoverded.Visible = true;
                    divoverdedgrd.Visible = true;
                    ddloverall.SelectedIndex = 1;
                    com_err.Visible = true;
                    com_err.Text = "Deduction Details Already Exist!";
                }
                if (grdoveralllev.Rows.Count > 0)
                {
                    divoveralllev.Visible = true;
                    grdoveralllev.Visible = true;
                    ddloverall.SelectedIndex = 2;
                    com_err.Visible = true;
                    com_err.Text = "Leave Details Already Exist!";
                }
            }
        }
    }
    protected void rdbmainoverall_change(object sender, EventArgs e)
    {
        divind.Visible = false;
        divoverall.Visible = true;
        ddloverall.Enabled = true;
        ddloverall.SelectedIndex = 0;
        allowover_div.Visible = false;
        dedover_div.Visible = false;
        ltype_overlevdiv.Visible = false;
        divovercom.Visible = false;
        divoverallgrd.Visible = false;
        grdoverall.Visible = false;
        divoverdedgrd.Visible = false;
        grdoverded.Visible = false;
        divoveralllev.Visible = false;
        grdoveralllev.Visible = false;
        divovergrdcom.Visible = false;
        grdovercom.Visible = false;
        com_err.Visible = false;
        Session["overalldt"] = null;
        Session["overdtded"] = null;
        Session["overdtlev"] = null;
        Session["overdtcom"] = null;
        grdoverall.DataSource = null;
        grdoverall.DataBind();
        grdoverded.DataSource = null;
        grdoverded.DataBind();
        grdoveralllev.DataSource = null;
        grdoveralllev.DataBind();
        grdovercom.DataSource = null;
        grdovercom.DataBind();
        if (ddloverall.SelectedItem.Value == "0")
        {
            //if (grdoverded.Rows.Count == 0 && grdoveralllev.Rows.Count == 0 && grdovercom.Rows.Count == 0)
            //{
            lblovercom.Visible = false;
            lbloverallow.Visible = true;
            ddloverallallow.Visible = true;
            lbloverded.Visible = false;
            ddloverallded.Visible = false;
            lbloverleav.Visible = false;
            ddloverlev.Visible = false;
            overallallowance();
            divoverallgrd.Visible = true;
            grdoverall.Visible = true;
            divoverdedgrd.Visible = false;
            grdoverded.Visible = false;
            divoveralllev.Visible = false;
            grdoveralllev.Visible = false;
            divovergrdcom.Visible = false;
            grdovercom.Visible = false;
            lblerroverall.Visible = false;
            //}
            //else
            //{
            //    if (grdoverded.Rows.Count > 0)
            //    {
            //        divoverdedgrd.Visible = true;
            //        grdoverded.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Deduction Details Already Exist!";
            //    }
            //    if (grdoveralllev.Rows.Count > 0)
            //    {
            //        grdoveralllev.Visible = true;
            //        divoveralllev.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Leave Details Already Exist!";
            //    }
            //    if (grdovercom.Rows.Count > 0)
            //    {
            //        divovergrdcom.Visible = true;
            //        grdovercom.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Common Details Already Exist!";
            //    }
            //}
        }
        else if (ddloverall.SelectedItem.Value == "1")
        {
            //if (grdoverall.Rows.Count == 0 && grdoveralllev.Rows.Count == 0 && grdovercom.Rows.Count == 0)
            //{
            lblovercom.Visible = false;
            lbloverded.Visible = true;
            ddloverallded.Visible = true;
            lbloverallow.Visible = false;
            ddloverallallow.Visible = false;
            lbloverleav.Visible = false;
            ddloverlev.Visible = false;
            overalldeduction();
            divoverdedgrd.Visible = true;
            grdoverded.Visible = true;
            divoverallgrd.Visible = false;
            grdoverall.Visible = false;
            divoveralllev.Visible = false;
            grdoveralllev.Visible = false;
            divovergrdcom.Visible = false;
            grdovercom.Visible = false;
            lbloverdederr.Visible = false;
            //}
            //else
            //{
            //    if (grdoverall.Rows.Count > 0)
            //    {
            //        divoverallgrd.Visible = true;
            //        grdoverall.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Allowance Details Already Exist!";
            //    }
            //    if (grdoveralllev.Rows.Count > 0)
            //    {
            //        grdoveralllev.Visible = true;
            //        divoveralllev.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Leave Details Already Exist!";
            //    }
            //    if (grdovercom.Rows.Count > 0)
            //    {
            //        divovergrdcom.Visible = true;
            //        grdovercom.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Common Details Already Exist!";
            //    }
            //}
        }
        else if (ddloverall.SelectedItem.Value == "2")
        {
            //if (grdoverall.Rows.Count == 0 && grdoverded.Rows.Count == 0 && grdovercom.Rows.Count == 0)
            //{
            lblovercom.Visible = false;
            lbloverleav.Visible = true;
            ddloverlev.Visible = true;
            lbloverded.Visible = false;
            ddloverallded.Visible = false;
            lbloverallow.Visible = false;
            ddloverallallow.Visible = false;
            overallLeave();
            divoverdedgrd.Visible = false;
            grdoverded.Visible = false;
            divoverallgrd.Visible = false;
            grdoverall.Visible = false;
            divoveralllev.Visible = true;
            grdoveralllev.Visible = true;
            divovergrdcom.Visible = false;
            grdovercom.Visible = false;
            lbloverleverr.Visible = false;
            //}
            //else
            //{
            //    if (grdoverall.Rows.Count > 0)
            //    {
            //        divoverallgrd.Visible = true;
            //        grdoverall.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Allowance Details Already Exist!";
            //    }
            //    if (grdoverded.Rows.Count > 0)
            //    {
            //        grdoverded.Visible = true;
            //        divoverdedgrd.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Deduction Details Already Exist!";
            //    }
            //    if (grdovercom.Rows.Count > 0)
            //    {
            //        divovergrdcom.Visible = true;
            //        grdovercom.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Common Details Already Exist!";
            //    }
            //}
        }
        else if (ddloverall.SelectedItem.Value == "3")
        {
            //if (grdoverall.Rows.Count == 0 && grdoverded.Rows.Count == 0 && grdoveralllev.Rows.Count == 0)
            //{
            lblovercom.Visible = true;
            lbloverleav.Visible = false;
            ddloverlev.Visible = false;
            lbloverded.Visible = false;
            ddloverallded.Visible = false;
            lbloverallow.Visible = false;
            ddloverallallow.Visible = false;
            divovercom.Visible = true;
            overcomclear();
            divoverdedgrd.Visible = false;
            grdoverded.Visible = false;
            divoverallgrd.Visible = false;
            grdoverall.Visible = false;
            divoveralllev.Visible = false;
            grdoveralllev.Visible = false;
            lblovercomerr.Visible = false;
            //}
            //else
            //{
            //    if (grdoverall.Rows.Count > 0)
            //    {
            //        divoverallgrd.Visible = true;
            //        grdoverall.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Allowance Details Already Exist!";
            //    }
            //    if (grdoverded.Rows.Count > 0)
            //    {
            //        grdoverded.Visible = true;
            //        divoverdedgrd.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Deduction Details Already Exist!";
            //    }
            //    if (grdoveralllev.Rows.Count > 0)
            //    {
            //        divoveralllev.Visible = true;
            //        grdoveralllev.Visible = true;
            //        com_err.Visible = true;
            //        com_err.Text = "Leave Details Already Exist!";
            //    }
            //}
        }
    }
    #region Overall Allowance
    protected void grdoverall_rowbound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverall, "index$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverall, "index$" + e.Row.RowIndex);
            e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverall, "index$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverall, "index$" + e.Row.RowIndex);
            e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverall, "index$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverall, "index$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverall, "index$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverall, "index$" + e.Row.RowIndex);
            e.Row.Cells[8].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverall, "index$" + e.Row.RowIndex);
        }
    }
    protected void grdoverall_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            com_err.Visible = false;
            lblerroverall.Visible = false;
            string alltype = "";
            string mode = "";
            string value = "";
            string inclop = "";
            string frmbasic = "";
            string frmbasgp = "";
            string issplall = "";
            string frmbasicagp = "";
            string round = "";
            for (int rem = 0; rem < grdoverall.Rows.Count; rem++)
            {
                grdoverall.Rows[rem].BackColor = Color.White;
            }
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "index")
            {
                grdoverall.Visible = false;
                divoverallgrd.Visible = false;
                allowover_div.Visible = true;
                btn_overallsave.Visible = false;
                btn_overalldelete.Visible = true;
                btn_overallupdate.Visible = true;
                alltype = (grdoverall.Rows[row].FindControl("lbl_overalltype") as Label).Text;
                mode = (grdoverall.Rows[row].FindControl("lbl_overallmode") as Label).Text;
                if (mode == "Amount")
                {
                    ddloverallmode.SelectedIndex = 0;
                    chkoveralldisable();
                    txtoverallval.Enabled = true;
                }
                else if (mode == "Percent")
                {
                    ddloverallmode.SelectedIndex = 1;
                    chkoverrallenable();
                    txtoverallval.Enabled = true;
                }
                else if (mode == "Slab")
                {
                    ddloverallmode.SelectedIndex = 2;
                    chkoverrallenable();
                    txtoverallval.Enabled = false;
                }
                value = (grdoverall.Rows[row].FindControl("lbl_overallval") as Label).Text;
                if (value.Trim() != "" && value.Trim() != "0.00" && value.Trim() != "0")
                    txtoverallval.Text = Convert.ToString(value);
                else
                    txtoverallval.Text = "";
                inclop = (grdoverall.Rows[row].FindControl("lbl_overalllop") as Label).Text;
                if (inclop.Trim() == "Yes")
                    cbinclopoverall.Checked = true;
                else
                    cbinclopoverall.Checked = false;
                frmbasic = (grdoverall.Rows[row].FindControl("lbl_overallfrmbasic") as Label).Text;
                if (frmbasic.Trim() == "Yes")
                    rbfrmbasoverall.Checked = true;
                else
                    rbfrmbasoverall.Checked = false;
                frmbasgp = (grdoverall.Rows[row].FindControl("lbl_overallfrmbasgp") as Label).Text;
                if (frmbasgp.Trim() == "Yes")
                    rbfrmbasgpoverall.Checked = true;
                else
                    rbfrmbasgpoverall.Checked = false;
                issplall = (grdoverall.Rows[row].FindControl("lbl_overallisspl") as Label).Text;
                if (issplall.Trim() == "Yes")
                    cbissploverall.Checked = true;
                else
                    cbissploverall.Checked = false;
                frmbasicagp = (grdoverall.Rows[row].FindControl("lbl_overallfrmbasagp") as Label).Text;
                if (frmbasicagp.Trim() == "Yes")
                    rbfrmbasagp.Checked = true;
                else
                    rbfrmbasagp.Checked = false;
                round = (grdoverall.Rows[row].FindControl("lbl_overallroundtype") as Label).Text;
                if (round.Trim() != "")
                    ddlroundoverall.SelectedIndex = ddlroundoverall.Items.IndexOf(ddlroundoverall.Items.FindByText(round));
                else
                    ddlroundoverall.SelectedIndex = 0;
                ddloverallallow.SelectedIndex = ddloverallallow.Items.IndexOf(ddloverallallow.Items.FindByText(alltype));
                lblalllabel.Text = "Allowances -" + " " + alltype;
                grdoverall.Rows[row].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }
    public void chkoveralldisable()
    {
        cbinclopoverall.Checked = false;
        cbinclopoverall.Enabled = true;
        rbfrmbasoverall.Checked = false;
        rbfrmbasoverall.Enabled = false;
        rbfrmbasgpoverall.Checked = false;
        rbfrmbasgpoverall.Enabled = false;
        cbissploverall.Checked = false;
        cbissploverall.Enabled = true;
        rbfrmbasagp.Checked = false;
        rbfrmbasagp.Enabled = false;
    }
    public void chkoverrallenable()
    {
        cbinclopoverall.Checked = true;
        cbinclopoverall.Enabled = true;
        rbfrmbasoverall.Checked = false;
        rbfrmbasoverall.Enabled = true;
        rbfrmbasgpoverall.Checked = false;
        rbfrmbasgpoverall.Enabled = true;
        cbissploverall.Checked = false;
        cbissploverall.Enabled = true;
        rbfrmbasagp.Checked = false;
        rbfrmbasagp.Enabled = true;
    }
    protected void ddloverallmode_indexchanged(object sender, EventArgs e)
    {
        try
        {
            cbinclopoverall.Checked = false;
            cbissploverall.Checked = false;
            ddlroundoverall.SelectedIndex = 0;
            if (ddloverallmode.SelectedItem.Text == "Amount")
            {
                txtoverallval.Text = "0.00";
                txtoverallval.Enabled = true;
                chkoveralldisable();
            }
            else if (ddloverallmode.SelectedItem.Text == "Percent")
            {
                txtoverallval.Text = "";
                txtoverallval.Enabled = true;
                chkoverrallenable();
            }
            else
            {
                txtoverallval.Text = "";
                txtoverallval.Enabled = false;
                chkoverrallenable();
            }
        }
        catch { }
    }
    protected void ddloverallallow_change(object sender, EventArgs e)
    {
        try
        {
            if (ddloverallallow.SelectedItem.Text.Trim() != "Select")
            {
                allowover_div.Visible = true;
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
                divovergrdcom.Visible = false;
                grdovercom.Visible = false;
                lblerroverall.Visible = false;
                chkoveralldisable();
                txtoverallval.Text = "0.00";
                txtoverallval.Enabled = true;
                ddloverallmode.SelectedIndex = 0;
                ddlroundoverall.SelectedIndex = 1;
                btn_overallsave.Visible = true;
                btn_overallupdate.Visible = false;
                btn_overalldelete.Visible = false;
                lblalllabel.Text = "";
                lblalllabel.Text = "Allowance - " + Convert.ToString(ddloverallallow.SelectedItem.Text);
            }
            else
            {
                allowover_div.Visible = false;
                divoverallgrd.Visible = true;
                grdoverall.Visible = true;
            }
        }
        catch { }
    }
    protected void btn_overallsave_Click(object sender, EventArgs e)
    {
        try
        {
            lblerroverall.Visible = false;
            Session["overalltype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (ddloverallallow.SelectedItem.Text.Trim() != "Select")
            {
                string overallall = "";
                if (txtoverallval.Text != "" || ddloverallmode.SelectedItem.Text == "Slab")
                {
                    overallall = getoverallallowoverall();
                    string newcolvalue = "";
                    if (Session["overalltype"] == null)
                    {
                        newcolvalue = Convert.ToString(ddloverallallow.SelectedItem.Text);
                        Session["overalltype"] = newcolvalue;
                    }
                    divoverallgrd.Visible = true;
                    grdoverall.Visible = true;
                    allowover_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtoverallheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = overallall.Split('\\');
                    string colvalue = "";
                    colvalue = Convert.ToString(Session["overalltype"]);
                    if (Session["overalldt"] != null)
                    {
                        DataRow dr;
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["overalldt"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                for (int col = 0; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (Convert.ToString(dt.Rows[k][0]) == colvalue)
                                dt.Rows.Remove(dt.Rows[k]);
                        }
                        dt = getoverallval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                        Session["overalldt"] = dt;
                    }
                    else
                    {
                        DataRow dr;
                        dt = getoverallval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                        Session["overalldt"] = null;
                        Session["overalldt"] = dt;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grdoverall.DataSource = dt;
                        grdoverall.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdoverall.DataBind();
                        for (int i = 0; i < grdoverall.Columns.Count; i++)
                        {
                            grdoverall.Columns[i].HeaderStyle.Width = 100;
                            grdoverall.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grdoverall.DataSource = dt;
                        grdoverall.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            else
            {
                lblerroverall.Visible = true;
                lblerroverall.Text = "Please Select Any Allowance!";
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
                allowover_div.Visible = true;
                return;
            }
            if (savecount > 0)
            {
                lblerroverall.Visible = true;
                lblerroverall.Text = "Added Successfully!";
                allowover_div.Visible = true;
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
            }
            if (errcount > 0)
            {
                lblerroverall.Visible = true;
                lblerroverall.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btn_overallupdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblerroverall.Visible = false;
            Session["overalltype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (ddloverallallow.SelectedItem.Text.Trim() != "Select")
            {
                string overallall = "";
                if (txtoverallval.Text != "" || ddloverallmode.SelectedItem.Text == "Slab")
                {
                    overallall = getoverallallowoverall();
                    divoverallgrd.Visible = true;
                    grdoverall.Visible = true;
                    allowover_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtoverallheader(dt);
                    string newcolvalue = "";
                    if (Session["overalltype"] == null)
                    {
                        newcolvalue = Convert.ToString(ddloverallallow.SelectedItem.Text);
                        Session["overalltype"] = newcolvalue;
                    }
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = overallall.Split('\\');
                    string colvalue = "";
                    colvalue = Convert.ToString(Session["overalltype"]);
                    if (Session["overalldt"] != null)
                    {
                        DataRow dr;
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["overalldt"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                for (int col = 0; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (Convert.ToString(dt.Rows[k][0]) == colvalue)
                                dt.Rows.Remove(dt.Rows[k]);
                        }
                        dt = getoverallval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                        Session["overalldt"] = dt;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grdoverall.DataSource = dt;
                        grdoverall.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdoverall.DataBind();
                        for (int i = 0; i < grdoverall.Columns.Count; i++)
                        {
                            grdoverall.Columns[i].HeaderStyle.Width = 100;
                            grdoverall.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grdoverall.DataSource = dt;
                        grdoverall.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            else
            {
                lblerroverall.Visible = true;
                lblerroverall.Text = "Please Select Any Allowance!";
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
                allowover_div.Visible = true;
                return;
            }
            if (savecount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = " Updated Successfully! ";
                allowover_div.Visible = false;
                divoverallgrd.Visible = true;
                grdoverall.Visible = true;
            }
            if (errcount > 0)
            {
                lblerroverall.Visible = true;
                lblerroverall.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btn_overalldelete_click(object sender, EventArgs e)
    {
        try
        {
            lblerroverall.Visible = false;
            Session["overalltype"] = null;
            int delcount = 0;
            if (ddloverallallow.SelectedItem.Text.Trim() != "Select")
            {
                divoverallgrd.Visible = true;
                grdoverall.Visible = true;
                allowover_div.Visible = false;
                DataTable dt = new DataTable();
                dtoverallheader(dt);
                string newcolvalue = "";
                if (Session["overalltype"] == null)
                {
                    newcolvalue = Convert.ToString(ddloverallallow.SelectedItem.Text);
                    Session["overalltype"] = newcolvalue;
                }
                string colvalue = "";
                colvalue = Convert.ToString(Session["overalltype"]);
                if (Session["overalldt"] != null)
                {
                    DataRow dr;
                    DataTable dnew = new DataTable();
                    dnew = (DataTable)Session["overalldt"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (Convert.ToString(dt.Rows[k][0]) == colvalue)
                        {
                            dt.Rows.Remove(dt.Rows[k]);
                            delcount++;
                        }
                    }
                    Session["overalldt"] = dt;
                }
                if (dt.Rows.Count > 0)
                {
                    grdoverall.DataSource = dt;
                    grdoverall.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdoverall.DataBind();
                    for (int i = 0; i < grdoverall.Columns.Count; i++)
                    {
                        grdoverall.Columns[i].HeaderStyle.Width = 100;
                        grdoverall.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdoverall.DataSource = dt;
                    grdoverall.DataBind();
                }
            }
            else
            {
                lblerroverall.Visible = true;
                lblerroverall.Text = "Please Select any Allowance!";
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
                allowover_div.Visible = true;
                return;
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully!";
                divoverallgrd.Visible = true;
                grdoverall.Visible = true;
                allowover_div.Visible = false;
            }
        }
        catch { }
    }
    protected void btn_overallexit_Click(object sender, EventArgs e)
    {
        allowover_div.Visible = false;
        divoverallgrd.Visible = true;
        grdoverall.Visible = true;
        overallallowance();
    }
    #endregion
    #region Overall Deduction
    protected void grdoverded_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[8].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[9].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[10].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[11].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[12].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[13].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[14].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[15].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
                e.Row.Cells[16].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoverded, "index$" + e.Row.RowIndex);
            }
        }
        catch { }
    }
    protected void grdoverded_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            com_err.Visible = false;
            lbloverdederr.Visible = false;
            string dedtype = "";
            string mode = "";
            string value = "";
            string frmgross = "";
            string frmbasda = "";
            string inclop = "";
            string frmbasgpda = "";
            string frmbasic = "";
            string frmbasdp = "";
            string frmpetty = "";
            string frmbasarr = "";
            string ismaxcal = "";
            string maxamnt = "";
            string dedamnt = "";
            string frmbasarrsa = "";
            string frmall = "";
            string roundval = "";
            string frmnet = "";
            for (int rem = 0; rem < grdoverded.Rows.Count; rem++)
            {
                grdoverded.Rows[rem].BackColor = Color.White;
            }
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "index")
            {
                grdoverded.Visible = false;
                divoverdedgrd.Visible = false;
                dedover_div.Visible = true;
                btnoverdedsave.Visible = false;
                btnoverdedupdate.Visible = true;
                btnoverdeddel.Visible = true;
                dedtype = (grdoverded.Rows[row].FindControl("lbl_overdeducttype") as Label).Text;
                mode = (grdoverded.Rows[row].FindControl("lbl_overdedmode") as Label).Text;
                if (mode.Trim() == "Amount")
                {
                    ddloverdedmode.SelectedIndex = 0;
                    chkoverdeddisable();
                    txtoverdedval.Enabled = true;
                }
                else if (mode.Trim() == "Percent")
                {
                    ddloverdedmode.SelectedIndex = 1;
                    chkoverdedenable();
                    txtoverdedval.Enabled = true;
                }
                else
                {
                    ddloverdedmode.SelectedIndex = 2;
                    chkoverdedenable();
                    txtoverdedval.Enabled = false;
                }
                value = (grdoverded.Rows[row].FindControl("lbl_overdedval") as Label).Text;
                if (value.Trim() != "" && value.Trim() != "0" && value.Trim() != "0.00" && value.Trim() != "0.0000")
                    txtoverdedval.Text = Convert.ToString(value);
                else
                    txtoverdedval.Text = "";
                roundval = (grdoverded.Rows[row].FindControl("lbl_overrounddedroundtype") as Label).Text;
                if (roundval.Trim() != "" && roundval.Trim() != "0")
                    ddloverdedround.SelectedIndex = ddloverdedround.Items.IndexOf(ddloverdedround.Items.FindByText(roundval));
                else
                    ddloverdedround.SelectedIndex = 0;
                frmgross = (grdoverded.Rows[row].FindControl("lbl_overdedfrmgross") as Label).Text;
                if (frmgross.Trim() == "Yes")
                    rdboverdedfrmgross.Checked = true;
                else
                    rdboverdedfrmgross.Checked = false;
                frmbasda = (grdoverded.Rows[row].FindControl("lbl_overdedfrmbasicda") as Label).Text;
                if (frmbasda.Trim() == "Yes")
                    rdboverdedfrmbasda.Checked = true;
                else
                    rdboverdedfrmbasda.Checked = false;
                inclop = (grdoverded.Rows[row].FindControl("lbl_overdedfrmlop") as Label).Text;
                if (inclop.Trim() == "Yes")
                    cbinclopoverded.Checked = true;
                else
                    cbinclopoverded.Checked = false;
                frmbasgpda = (grdoverded.Rows[row].FindControl("lbl_overdedgpda") as Label).Text;
                if (frmbasgpda.Trim() == "Yes")
                    rdboverdedfrmbasgpda.Checked = true;
                else
                    rdboverdedfrmbasgpda.Checked = false;
                frmbasic = (grdoverded.Rows[row].FindControl("lbl_overdeddedfrmbas") as Label).Text;
                if (frmbasic.Trim() == "Yes")
                    rdboverdedfrmbas.Checked = true;
                else
                    rdboverdedfrmbas.Checked = false;
                frmbasdp = (grdoverded.Rows[row].FindControl("lbl_overdedfrmbasdp") as Label).Text;
                if (frmbasdp.Trim() == "Yes")
                    rdboverdedfrmbasdp.Checked = true;
                else
                    rdboverdedfrmbasdp.Checked = false;
                frmpetty = (grdoverded.Rows[row].FindControl("lbl_overdedfrmpetty") as Label).Text;
                if (frmpetty.Trim() == "Yes")
                    rdboverdedfrmpetty.Checked = true;
                else
                    rdboverdedfrmpetty.Checked = false;
                frmbasarr = (grdoverded.Rows[row].FindControl("lbl_overdedfrmbasarr") as Label).Text;
                if (frmbasarr.Trim() == "Yes")
                    rdboverdedfrmbasarr.Checked = true;
                else
                    rdboverdedfrmbasarr.Checked = false;
                ismaxcal = (grdoverded.Rows[row].FindControl("lbl_overdedismaxcal") as Label).Text;
                if (ismaxcal.Trim() == "Yes")
                    cbmaxcaloverded.Checked = true;
                else
                    cbmaxcaloverded.Checked = false;
                maxamnt = (grdoverded.Rows[row].FindControl("lbl_overdedmaxamt") as Label).Text;
                if (maxamnt.Trim() != "" && maxamnt.Trim() != "0" && maxamnt.Trim() != "0.00" && maxamnt.Trim() != "0.0000")
                    txtoverdedmaxamnt.Text = Convert.ToString(maxamnt);
                else
                    txtoverdedmaxamnt.Text = "";
                dedamnt = (grdoverded.Rows[row].FindControl("lbl_overdedamt") as Label).Text;
                if (dedamnt.Trim() != "" && dedamnt.Trim() != "0" && dedamnt.Trim() != "0.00" && dedamnt.Trim() != "0.0000")
                    txtoverdeddedamnt.Text = Convert.ToString(dedamnt);
                else
                    txtoverdeddedamnt.Text = "";
                frmbasarrsa = (grdoverded.Rows[row].FindControl("lbl_overdedfrmbasarrsa") as Label).Text;
                if (frmbasarrsa.Trim() == "Yes")
                    rdboverdedfrmbasarrsa.Checked = true;
                else
                    rdboverdedfrmbasarrsa.Checked = false;
                frmall = (grdoverded.Rows[row].FindControl("lbl_overdedfrmallow") as Label).Text;
                if (frmall.Trim() != "")
                {
                    cbfallowoverded.Checked = true;
                    txtoverdedall.Text = frmall;
                }
                else
                {
                    cbfallowoverded.Checked = false;
                    txtoverdedall.Text = "";
                }
                frmnet = (grdoverded.Rows[row].FindControl("lbl_overdedfrmnetamnt") as Label).Text;
                if (frmnet.Trim() == "Yes")
                    rdboverdedfrmnet.Checked = true;
                else
                    rdboverdedfrmnet.Checked = false;
                ddloverallded.SelectedIndex = ddloverallded.Items.IndexOf(ddloverallded.Items.FindByText(dedtype));
                lbldedalllbl.Text = "Deductions -" + " " + dedtype;
                grdoverded.Rows[row].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }
    public void chkoverdedenable()
    {
        rdboverdedfrmgross.Checked = true;
        rdboverdedfrmgross.Enabled = true;
        rdboverdedfrmbasda.Checked = false;
        rdboverdedfrmbasda.Enabled = true;
        cbinclopoverded.Checked = false;
        cbinclopoverded.Enabled = true;
        rdboverdedfrmbasgpda.Checked = false;
        rdboverdedfrmbasgpda.Enabled = true;
        rdboverdedfrmbas.Checked = false;
        rdboverdedfrmbas.Enabled = true;
        rdboverdedfrmbasdp.Checked = false;
        rdboverdedfrmbasdp.Enabled = true;
        rdboverdedfrmpetty.Checked = false;
        rdboverdedfrmpetty.Enabled = true;
        rdboverdedfrmbasarr.Checked = false;
        rdboverdedfrmbasarr.Enabled = true;
        cbmaxcaloverded.Checked = false;
        cbmaxcaloverded.Enabled = true;
        rdboverdedfrmbasarrsa.Checked = false;
        rdboverdedfrmbasarrsa.Enabled = true;
        cbfallowoverded.Checked = false;
        cbfallowoverded.Enabled = true;
        rdboverdedfrmnet.Checked = false;
        rdboverdedfrmnet.Enabled = true;
        txtoverdedall.Text = "";
    }
    public void chkoverdeddisable()
    {
        rdboverdedfrmgross.Checked = false;
        rdboverdedfrmgross.Enabled = false;
        rdboverdedfrmbasda.Checked = false;
        rdboverdedfrmbasda.Enabled = false;
        cbinclopoverded.Checked = false;
        cbinclopoverded.Enabled = true;
        rdboverdedfrmbasgpda.Checked = false;
        rdboverdedfrmbasgpda.Enabled = false;
        rdboverdedfrmbas.Checked = false;
        rdboverdedfrmbas.Enabled = false;
        rdboverdedfrmbasdp.Checked = false;
        rdboverdedfrmbasdp.Enabled = false;
        rdboverdedfrmpetty.Checked = false;
        rdboverdedfrmpetty.Enabled = false;
        rdboverdedfrmbasarr.Checked = false;
        rdboverdedfrmbasarr.Enabled = false;
        cbmaxcaloverded.Checked = false;
        cbmaxcaloverded.Enabled = true;
        rdboverdedfrmbasarrsa.Checked = false;
        rdboverdedfrmbasarrsa.Enabled = false;
        cbfallowoverded.Checked = false;
        cbfallowoverded.Enabled = false;
        rdboverdedfrmnet.Checked = false;
        rdboverdedfrmnet.Enabled = false;
        txtoverdedall.Text = "";
    }
    protected void ddloverdedmode_indexchanged(object sender, EventArgs e)
    {
        try
        {
            cbinclopoverded.Checked = false;
            ddloverdedround.SelectedIndex = 0;
            if (ddloverdedmode.SelectedItem.Text == "Amount")
            {
                txtoverdedval.Text = "0.00";
                txtoverdedval.Enabled = true;
                chkoverdeddisable();
            }
            else if (ddloverdedmode.SelectedItem.Text == "Percent")
            {
                txtoverdedval.Text = "";
                txtoverdedval.Enabled = true;
                chkoverdedenable();
            }
            else
            {
                txtoverdedval.Text = "";
                txtoverdedval.Enabled = false;
                chkoverdedenable();
            }
        }
        catch { }
    }
    protected void cbfallowoverded_CheckedChange(object sender, EventArgs e)
    {
        if (cbfallowoverded.Checked == true)
        {
            divallhead.Visible = true;
            lblheaderr.Visible = false;
            lblheadset.Text = "Deductions";
            chkoverdeddisable();
            cbinclopoverded.Checked = true;
            cbinclopoverded.Enabled = true;
            cbmaxcaloverded.Checked = true;
            cbmaxcaloverded.Enabled = true;
            cbfallowoverded.Checked = true;
            cbfallowoverded.Enabled = true;
            allowance();
        }
        else
        {
            chkoverdedenable();
        }
    }
    protected void ddloverallded_change(object sender, EventArgs e)
    {
        try
        {
            if (ddloverallded.SelectedItem.Text.Trim() != "Select")
            {
                dedover_div.Visible = true;
                chkoverdeddisable();
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
                divovergrdcom.Visible = false;
                grdovercom.Visible = false;
                lbloverdederr.Visible = false;
                txtoverdedval.Text = "0.00";
                txtoverdedval.Enabled = true;
                ddloverdedmode.SelectedIndex = 0;
                ddloverdedround.SelectedIndex = 1;
                btnoverdedsave.Visible = true;
                btnoverdeddel.Visible = false;
                btnoverdedupdate.Visible = false;
                lbldedalllbl.Text = "";
                lbldedalllbl.Text = "Deduction - " + Convert.ToString(ddloverallded.SelectedItem.Text);
            }
            else
            {
                dedover_div.Visible = false;
                divoverdedgrd.Visible = true;
                grdoverded.Visible = true;
            }
        }
        catch { }
    }
    protected void btnoverdedsave_Click(object sender, EventArgs e)
    {
        try
        {
            lbloverdederr.Visible = false;
            Session["overdedtype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (ddloverallded.SelectedItem.Text.Trim() != "Select")
            {
                string overallded = "";
                if (cbmaxcaloverded.Checked == true && txtoverdedmaxamnt.Text.Trim() == "")
                {
                    lbloverdederr.Visible = true;
                    lbloverdederr.Text = " Please Enter Max Amount! ";
                    dedover_div.Visible = true;
                    divoverdedgrd.Visible = false;
                    grdoverded.Visible = false;
                    return;
                }
                if (txtoverdedval.Text != "" || ddloverdedmode.SelectedItem.Text == "Slab")
                {
                    overallded = getoveralldedoverall();
                    string newcol = "";
                    if (Session["overdedtype"] == null)
                    {
                        newcol = Convert.ToString(ddloverallded.SelectedItem.Text);
                        Session["overdedtype"] = newcol;
                    }
                    string colvalue = "";
                    colvalue = Convert.ToString(Session["overdedtype"]);
                    divoverdedgrd.Visible = true;
                    grdoverded.Visible = true;
                    dedover_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtoverdedheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = overallded.Split('\\');
                    if (Session["overdtded"] != null)
                    {
                        DataRow dr;
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["overdtded"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                for (int col = 0; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (Convert.ToString(dt.Rows[k][0]) == colvalue)
                                dt.Rows.Remove(dt.Rows[k]);
                        }
                        dt = getoverdedval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                        Session["overdtded"] = dt;
                    }
                    else
                    {
                        DataRow dr;
                        dt = getoverdedval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                        Session["overdtded"] = null;
                        Session["overdtded"] = dt;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grdoverded.DataSource = dt;
                        grdoverded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdoverded.DataBind();
                        for (int i = 0; i < grdoverded.Columns.Count; i++)
                        {
                            grdoverded.Columns[i].HeaderStyle.Width = 100;
                            grdoverded.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grdoverded.DataSource = dt;
                        grdoverded.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            else
            {
                lbloverdederr.Visible = true;
                lbloverdederr.Text = "Please Select Any Deduction!";
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
                dedover_div.Visible = true;
                return;
            }
            if (savecount > 0)
            {
                lbloverdederr.Visible = true;
                lbloverdederr.Text = " Added Successfully! ";
                dedover_div.Visible = true;
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
            }
            if (errcount > 0)
            {
                lbloverdederr.Visible = true;
                lbloverdederr.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btnoverdedupdate_Click(object sender, EventArgs e)
    {
        try
        {
            lbloverdederr.Visible = false;
            Session["overdedtype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (ddloverallded.SelectedItem.Text.Trim() != "Select")
            {
                string overallded = "";
                if (cbmaxcaloverded.Checked == true && txtoverdedmaxamnt.Text.Trim() == "")
                {
                    lbloverdederr.Visible = true;
                    lbloverdederr.Text = " Please Enter Max Amount! ";
                    dedover_div.Visible = true;
                    divoverdedgrd.Visible = false;
                    grdoverded.Visible = false;
                    return;
                }
                if (txtoverdedval.Text != "" || ddloverdedmode.SelectedItem.Text == "Slab")
                {
                    overallded = getoveralldedoverall();
                    string newcol = "";
                    if (Session["overdedtype"] == null)
                    {
                        newcol = Convert.ToString(ddloverallded.SelectedItem.Text);
                        Session["overdedtype"] = newcol;
                    }
                    string colvalue = "";
                    colvalue = Convert.ToString(Session["overdedtype"]);
                    divoverdedgrd.Visible = true;
                    grdoverded.Visible = true;
                    dedover_div.Visible = false;
                    DataTable dt = new DataTable();
                    dtoverdedheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = overallded.Split('\\');
                    if (Session["overdtded"] != null)
                    {
                        DataRow dr;
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["overdtded"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                for (int col = 0; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (Convert.ToString(dt.Rows[k][0]) == colvalue)
                                dt.Rows.Remove(dt.Rows[k]);
                        }
                        dt = getoverdedval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                        Session["dtded"] = dt;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grdoverded.DataSource = dt;
                        grdoverded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdoverded.DataBind();
                        for (int i = 0; i < grdoverded.Columns.Count; i++)
                        {
                            grdoverded.Columns[i].HeaderStyle.Width = 100;
                            grdoverded.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grdoverded.DataSource = dt;
                        grdoverded.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            else
            {
                lbloverdederr.Visible = true;
                lbloverdederr.Text = "Please Select Any Deduction!";
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
                dedover_div.Visible = true;
                return;
            }
            if (savecount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Updated Successfully!";
                dedover_div.Visible = false;
                divoverdedgrd.Visible = true;
                grdoverded.Visible = true;
            }
            if (errcount > 0)
            {
                lbloverdederr.Visible = true;
                lbloverdederr.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btnoverdeddel_Click(object sender, EventArgs e)
    {
        try
        {
            lbloverdederr.Visible = false;
            Session["overdedtype"] = null;
            int delcount = 0;
            if (ddloverallded.SelectedItem.Text.Trim() != "Select")
            {
                string newcol = "";
                if (Session["overdedtype"] == null)
                {
                    newcol = Convert.ToString(ddloverallded.SelectedItem.Text);
                    Session["overdedtype"] = newcol;
                }
                string colvalue = "";
                colvalue = Convert.ToString(Session["overdedtype"]);
                divoverdedgrd.Visible = true;
                grdoverded.Visible = true;
                dedover_div.Visible = false;
                DataTable dt = new DataTable();
                dtoverdedheader(dt);
                if (Session["overdtded"] != null)
                {
                    DataRow dr;
                    DataTable dnew = new DataTable();
                    dnew = (DataTable)Session["overdtded"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (Convert.ToString(dt.Rows[k][0]) == colvalue)
                        {
                            dt.Rows.Remove(dt.Rows[k]);
                            delcount++;
                        }
                    }
                    Session["overdtded"] = dt;
                }
                if (dt.Rows.Count > 0)
                {
                    grdoverded.DataSource = dt;
                    grdoverded.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdoverded.DataBind();
                    for (int i = 0; i < grdoverded.Columns.Count; i++)
                    {
                        grdoverded.Columns[i].HeaderStyle.Width = 100;
                        grdoverded.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdoverded.DataSource = dt;
                    grdoverded.DataBind();
                }
            }
            else
            {
                lbloverdederr.Visible = true;
                lbloverdederr.Text = "Please Select Any Deduction!";
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
                dedover_div.Visible = true;
                return;
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully!";
                dedover_div.Visible = false;
                divoverdedgrd.Visible = true;
                grdoverded.Visible = true;
            }
        }
        catch { }
    }
    protected void btnoverdedexit_Click(object sender, EventArgs e)
    {
        dedover_div.Visible = false;
        divoverdedgrd.Visible = true;
        grdoverded.Visible = true;
        overalldeduction();
    }
    #endregion
    #region Overall Leave Type
    protected void grdoveralllev_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoveralllev, "index$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoveralllev, "index$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoveralllev, "index$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoveralllev, "index$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoveralllev, "index$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoveralllev, "index$" + e.Row.RowIndex);
                e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdoveralllev, "index$" + e.Row.RowIndex);
            }
        }
        catch { }
    }
    protected void grdoveralllev_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            com_err.Visible = false;
            lbloverleverr.Visible = false;
            string lblleave = "";
            string levtype = "";
            string yrlev = "";
            string monlev = "";
            string incsunday = "";
            string incholiday = "";
            string moncarry = "";
            string yrcarry = "";
            for (int rem = 0; rem < grdoveralllev.Rows.Count; rem++)
            {
                grdoveralllev.Rows[rem].BackColor = Color.White;
            }
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "index")
            {
                grdoveralllev.Visible = false;
                divoveralllev.Visible = false;
                ltype_overlevdiv.Visible = true;
                btnltypeoversave.Visible = false;
                btnltypeoverupdate.Visible = true;
                btnltypeoverdel.Visible = true;
                int chkcount = 0;
                levtype = (grdoveralllev.Rows[row].FindControl("lbl_overlevtype") as Label).Text;
                yrlev = (grdoveralllev.Rows[row].FindControl("lbl_overyrlev") as Label).Text;
                if (yrlev.Trim() != "" && yrlev.Trim() != "0")
                    txtoveryrlev.Text = Convert.ToString(yrlev);
                else
                    txtoveryrlev.Text = "";
                monlev = (grdoveralllev.Rows[row].FindControl("lbl_overmonlev") as Label).Text;
                if (monlev.Trim() != "" && monlev.Trim() != "0")
                    txtovermonlev.Text = Convert.ToString(monlev);
                else
                    txtovermonlev.Text = "";
                incsunday = (grdoveralllev.Rows[row].FindControl("lbl_overincsunday") as Label).Text;
                if (incsunday.Trim() == "Yes")
                    cboversuninc.Checked = true;
                else
                    cboversuninc.Checked = false;
                incholiday = (grdoveralllev.Rows[row].FindControl("lbl_overincholiday") as Label).Text;
                if (incholiday.Trim() == "Yes")
                    cboverholinc.Checked = true;
                else
                    cboverholinc.Checked = false;
                moncarry = (grdoveralllev.Rows[row].FindControl("lbl_overmoncarry") as Label).Text;
                if (moncarry.Trim() == "Yes")
                    cbovermonco.Checked = true;
                else
                    cbovermonco.Checked = false;
                yrcarry = (grdoveralllev.Rows[row].FindControl("lbl_overyrcarry") as Label).Text;
                if (yrcarry.Trim() == "Yes")
                    cboveryrco.Checked = true;
                else
                    cboveryrco.Checked = false;
                ddloverlev.SelectedIndex = ddloverlev.Items.IndexOf(ddloverlev.Items.FindByText(levtype));
                lbloverlev.Text = "Leave Type -" + " " + levtype;
                grdoveralllev.Rows[row].BackColor = Color.LightGreen;
            }
        }
        catch { }
    }
    public void overlevclear()
    {
        txtoveryrlev.Text = "";
        txtovermonlev.Text = "";
        cboversuninc.Checked = false;
        cboverholinc.Checked = false;
        cbovermonco.Checked = false;
        cboveryrco.Checked = false;
    }
    protected void ddloverlev_change(object sender, EventArgs e)
    {
        try
        {
            if (ddloverlev.SelectedItem.Text.Trim() != "Select")
            {
                ltype_overlevdiv.Visible = true;
                overlevclear();
                divoverallgrd.Visible = false;
                grdoverall.Visible = false;
                divoverdedgrd.Visible = false;
                grdoverded.Visible = false;
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
                divovergrdcom.Visible = false;
                grdovercom.Visible = false;
                lbloverleverr.Visible = false;
                btnltypeoversave.Visible = true;
                btnltypeoverupdate.Visible = false;
                btnltypeoverdel.Visible = false;
                lbloverlev.Text = "";
                lbloverlev.Text = "Leave Type - " + Convert.ToString(ddloverlev.SelectedItem.Text);
            }
            else
            {
                ltype_overlevdiv.Visible = false;
                divoveralllev.Visible = true;
                grdoveralllev.Visible = true;
            }
        }
        catch { }
    }
    protected void btnltypeoversave_Click(object sender, EventArgs e)
    {
        try
        {
            lbloverleverr.Visible = false;
            Session["overlevtype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (ddloverlev.SelectedItem.Text.Trim() != "Select")
            {
                string leave = "";
                string college = ddlcollege.SelectedValue;
                if (txtoveryrlev.Text != "")
                {
                    leave = getoveralllevoverall();
                    string newcol = "";
                    if (Session["overlevtype"] == null)
                    {
                        newcol = Convert.ToString(ddloverlev.SelectedItem.Text);
                        Session["overlevtype"] = newcol;
                    }
                    string colvalue = "";
                    colvalue = Convert.ToString(Session["overlevtype"]);
                    divoveralllev.Visible = true;
                    grdoveralllev.Visible = true;
                    ltype_overlevdiv.Visible = false;
                    DataTable dt = new DataTable();
                    dtoverltypeheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = leave.Split('\\');
                    if (Session["overdtlev"] != null)
                    {
                        DataRow dr;
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["overdtlev"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                for (int col = 0; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (Convert.ToString(dt.Rows[k][0]) == colvalue)
                                dt.Rows.Remove(dt.Rows[k]);
                        }
                        dt = getoverlevval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                        Session["overdtlev"] = dt;
                    }
                    else
                    {
                        DataRow dr;
                        dt = getoverlevval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                        Session["overdtlev"] = null;
                        Session["overdtlev"] = dt;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grdoveralllev.DataSource = dt;
                        grdoveralllev.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdoveralllev.DataBind();
                        for (int i = 0; i < grdoveralllev.Columns.Count; i++)
                        {
                            grdoveralllev.Columns[i].HeaderStyle.Width = 100;
                            grdoveralllev.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grdoveralllev.DataSource = dt;
                        grdoveralllev.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            else
            {
                lbloverleverr.Visible = true;
                lbloverleverr.Text = "Please Select Any Leave Type!";
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
                ltype_overlevdiv.Visible = true;
                return;
            }
            if (savecount > 0)
            {
                lbloverleverr.Visible = true;
                lbloverleverr.Text = " Added Successfully! ";
                ltype_overlevdiv.Visible = true;
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
            }
            if (errcount > 0)
            {
                lbloverleverr.Visible = true;
                lbloverleverr.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btnltypeoverupdate_Click(object sender, EventArgs e)
    {
        try
        {
            lbloverleverr.Visible = false;
            Session["overlevtype"] = null;
            int savecount = 0;
            int errcount = 0;
            if (ddloverlev.SelectedItem.Text.Trim() != "Select")
            {
                string leave = "";
                string college = ddlcollege.SelectedValue;
                if (txtoveryrlev.Text != "")
                {
                    leave = getoveralllevoverall();
                    string newcol = "";
                    if (Session["overlevtype"] == null)
                    {
                        newcol = Convert.ToString(ddloverlev.SelectedItem.Text);
                        Session["overlevtype"] = newcol;
                    }
                    string colvalue = "";
                    colvalue = Convert.ToString(Session["overlevtype"]);
                    divoveralllev.Visible = true;
                    grdoveralllev.Visible = true;
                    ltype_overlevdiv.Visible = false;
                    DataTable dt = new DataTable();
                    dtoverltypeheader(dt);
                    string[] allowanmce_arr1;
                    string alowancesplit = "";
                    allowanmce_arr1 = leave.Split('\\');
                    if (Session["overdtlev"] != null)
                    {
                        DataRow dr;
                        DataTable dnew = new DataTable();
                        dnew = (DataTable)Session["overdtlev"];
                        if (dnew.Rows.Count > 0)
                        {
                            for (int ro = 0; ro < dnew.Rows.Count; ro++)
                            {
                                dr = dt.NewRow();
                                for (int col = 0; col < dnew.Columns.Count; col++)
                                {
                                    dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (Convert.ToString(dt.Rows[k][0]) == colvalue)
                                dt.Rows.Remove(dt.Rows[k]);
                        }
                        dt = getoverlevval(dt, allowanmce_arr1, alowancesplit, dt.NewRow());
                        Session["overdtlev"] = dt;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        savecount++;
                        grdoveralllev.DataSource = dt;
                        grdoveralllev.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdoveralllev.DataBind();
                        for (int i = 0; i < grdoveralllev.Columns.Count; i++)
                        {
                            grdoveralllev.Columns[i].HeaderStyle.Width = 100;
                            grdoveralllev.Columns[i].ItemStyle.Width = 100;
                        }
                    }
                    else
                    {
                        grdoveralllev.DataSource = dt;
                        grdoveralllev.DataBind();
                    }
                }
                else
                {
                    errcount++;
                }
            }
            else
            {
                lbloverleverr.Visible = true;
                lbloverleverr.Text = "Please Select Any Leave Type!";
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
                ltype_overlevdiv.Visible = true;
                return;
            }
            if (savecount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = " Updated Successfully! ";
                ltype_overlevdiv.Visible = false;
                divoveralllev.Visible = true;
                grdoveralllev.Visible = true;
            }
            if (errcount > 0)
            {
                lbloverleverr.Visible = true;
                lbloverleverr.Text = "Please fill all fields!";
            }
        }
        catch { }
    }
    protected void btnltypeoverdel_Click(object sender, EventArgs e)
    {
        try
        {
            lbloverleverr.Visible = false;
            Session["overlevtype"] = null;
            int delcount = 0;
            if (ddloverlev.SelectedItem.Text.Trim() != "Select")
            {
                string newcol = "";
                if (Session["overlevtype"] == null)
                {
                    newcol = Convert.ToString(ddloverlev.SelectedItem.Text);
                    Session["overlevtype"] = newcol;
                }
                string colvalue = "";
                colvalue = Convert.ToString(Session["overlevtype"]);
                divoveralllev.Visible = true;
                grdoveralllev.Visible = true;
                ltype_overlevdiv.Visible = false;
                DataTable dt = new DataTable();
                dtoverltypeheader(dt);
                if (Session["overdtlev"] != null)
                {
                    DataRow dr;
                    DataTable dnew = new DataTable();
                    dnew = (DataTable)Session["overdtlev"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (Convert.ToString(dt.Rows[k][0]) == colvalue)
                        {
                            dt.Rows.Remove(dt.Rows[k]);
                            delcount++;
                        }
                    }
                    Session["overdtlev"] = dt;
                }
                if (dt.Rows.Count > 0)
                {
                    grdoveralllev.DataSource = dt;
                    grdoveralllev.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdoveralllev.DataBind();
                    for (int i = 0; i < grdoveralllev.Columns.Count; i++)
                    {
                        grdoveralllev.Columns[i].HeaderStyle.Width = 100;
                        grdoveralllev.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdoveralllev.DataSource = dt;
                    grdoveralllev.DataBind();
                }
            }
            else
            {
                lbloverleverr.Visible = true;
                lbloverleverr.Text = "Please Select Any Leave Type!";
                divoveralllev.Visible = false;
                grdoveralllev.Visible = false;
                ltype_overlevdiv.Visible = true;
                return;
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = " Deleted Successfully!";
                ltype_overlevdiv.Visible = false;
                divoveralllev.Visible = true;
                grdoveralllev.Visible = true;
            }
        }
        catch { }
    }
    protected void btnltypeoverexit_Click(object sender, EventArgs e)
    {
        ltype_overlevdiv.Visible = false;
        divoveralllev.Visible = true;
        grdoveralllev.Visible = true;
        overallLeave();
    }
    #endregion
    #region Overall Common
    protected void grdovercom_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdovercom, "index$" + e.Row.RowIndex);
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdovercom, "index$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdovercom, "index$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdovercom, "index$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdovercom, "index$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdovercom, "index$" + e.Row.RowIndex);
                e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.grdovercom, "index$" + e.Row.RowIndex);
            }
        }
        catch { }
    }
    protected void grdovercom_rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            com_err.Visible = false;
            lblovercomerr.Visible = false;
            string gradepay = "";
            string basicpay = "";
            string payband = "";
            string ismpf = "";
            string ismpfper = "";
            string ismpfamnt = "";
            string isautogp = "";
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "index")
            {
                grdovercom.Visible = false;
                divovergrdcom.Visible = false;
                divovercom.Visible = true;
                btnovercomsave.Visible = false;
                btnovercomupdate.Visible = true;
                btnovercomdel.Visible = true;
                gradepay = (grdovercom.Rows[row].FindControl("lbl_overgrad") as Label).Text;
                if (gradepay.Trim() != "" && gradepay.Trim() != "0")
                    txtovergrade.Text = Convert.ToString(gradepay);
                else
                    txtovergrade.Text = "";
                basicpay = (grdovercom.Rows[row].FindControl("lbl_overbasicpay") as Label).Text;
                if (basicpay.Trim() != "" && basicpay.Trim() != "0.0" && basicpay.Trim() != "0" && basicpay.Trim() != "0.0000")
                    txtoverbasic.Text = Convert.ToString(basicpay);
                else
                    txtoverbasic.Text = "";
                payband = (grdovercom.Rows[row].FindControl("lbl_overpayband") as Label).Text;
                if (payband.Trim() != "" && payband.Trim() != "0")
                    txtoverpayband.Text = Convert.ToString(payband);
                else
                    txtoverpayband.Text = "";
                ismpfamnt = (grdovercom.Rows[row].FindControl("overismpfamnt") as Label).Text;
                if (ismpfamnt.Trim() == "Yes")
                    cbismpfover.Checked = true;
                else
                    cbismpfover.Checked = false;
                if (ismpfamnt.Trim() == "Yes")
                {
                    lblismpfamntover.Visible = true;
                    txtismpfamntover.Visible = true;
                    lblismpfperover.Visible = true;
                    txtismpfperover.Visible = true;
                }
                else
                {
                    lblismpfamntover.Visible = false;
                    txtismpfamntover.Visible = false;
                    lblismpfperover.Visible = false;
                    txtismpfperover.Visible = false;
                }
                ismpf = (grdovercom.Rows[row].FindControl("lbl_overismpf") as Label).Text;
                if (ismpf.Trim() != "" && ismpf.Trim() != "0" && ismpf.Trim() != "0.00")
                    txtismpfamntover.Text = Convert.ToString(ismpf);
                else
                    txtismpfamntover.Text = "";
                ismpfper = (grdovercom.Rows[row].FindControl("lbl_overismpfper") as Label).Text;
                if (ismpfper.Trim() != "" && ismpfper.Trim() != "0" && ismpfper.Trim() != "0.00")
                    txtismpfperover.Text = Convert.ToString(ismpfper);
                else
                    txtismpfperover.Text = "";
                isautogp = (grdovercom.Rows[row].FindControl("lbl_overisautogp") as Label).Text;
                if (isautogp.Trim() == "Yes")
                    cbisautogpover.Checked = true;
                else
                    cbisautogpover.Checked = false;
            }
        }
        catch { }
    }
    public void overcomclear()
    {
        txtovergrade.Text = "";
        txtismpfamntover.Text = "";
        txtismpfperover.Text = "";
        txtoverbasic.Text = "";
        txtoverpayband.Text = "";
        cbismpfover.Checked = false;
        cbisautogpover.Checked = false;
        lblismpfamntover.Visible = false;
        txtismpfamntover.Visible = false;
        lblismpfperover.Visible = false;
        txtismpfperover.Visible = false;
        btnovercomsave.Visible = true;
        btnovercomdel.Visible = false;
        btnovercomupdate.Visible = false;
    }
    protected void cbismpfover_change(object sender, EventArgs e)
    {
        try
        {
            if (cbismpfover.Checked == true)
            {
                lblismpfamntover.Visible = true;
                txtismpfamntover.Visible = true;
                txtismpfamntover.Text = "";
                txtismpfperover.Text = "";
                lblismpfperover.Visible = true;
                txtismpfperover.Visible = true;
            }
            else
            {
                lblismpfamntover.Visible = false;
                txtismpfamntover.Visible = false;
                txtismpfamntover.Text = "";
                txtismpfperover.Text = "";
                lblismpfperover.Visible = false;
                txtismpfperover.Visible = false;
            }
        }
        catch { }
    }
    protected void btnovercomsave_Click(object sender, EventArgs e)
    {
        try
        {
            int savecount = 0;
            int errcount = 0;
            if (txtoverbasic.Text.Trim() != "")
            {
                string gradepay = Convert.ToString(txtovergrade.Text);
                string basicpay = Convert.ToString(txtoverbasic.Text);
                string payband = Convert.ToString(txtoverpayband.Text);
                string ismpf = Convert.ToString(txtismpfamntover.Text);
                string ismpfper = Convert.ToString(txtismpfperover.Text);
                string ismpfamnt = "";
                string isautogp = "";
                if (cbismpfover.Checked == true)
                    ismpfamnt = "1";
                if (cbisautogpover.Checked == true)
                    isautogp = "1";
                divovergrdcom.Visible = true;
                grdovercom.Visible = true;
                divovercom.Visible = false;
                DataTable dt = new DataTable();
                dtovercomheader(dt);
                DataRow dr;
                if (Session["overdtcom"] != null)
                {
                    DataTable dnew = (DataTable)Session["overdtcom"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    dt.Rows.Clear();
                    dt = getovercomval(dt, dt.NewRow(), gradepay, basicpay, payband, ismpf, ismpfper, ismpfamnt, isautogp);
                    Session["overdtcom"] = dt;
                }
                else
                {
                    dt = getovercomval(dt, dt.NewRow(), gradepay, basicpay, payband, ismpf, ismpfper, ismpfamnt, isautogp);
                    Session["overdtcom"] = null;
                    Session["overdtcom"] = dt;
                }
                if (dt.Rows.Count > 0)
                {
                    savecount++;
                    grdovercom.DataSource = dt;
                    grdovercom.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdovercom.DataBind();
                    for (int i = 0; i < grdovercom.Columns.Count; i++)
                    {
                        grdovercom.Columns[i].HeaderStyle.Width = 100;
                        grdovercom.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdovercom.DataSource = dt;
                    grdovercom.DataBind();
                }
            }
            else
            {
                errcount++;
            }
            if (savecount > 0)
            {
                lblovercomerr.Visible = true;
                lblovercomerr.Text = "Added Successfully!";
                divovergrdcom.Visible = false;
                divovercom.Visible = true;
                grdovercom.Visible = false;
            }
            if (errcount > 0)
            {
                lblovercomerr.Visible = true;
                lblovercomerr.Text = "Please Fill all the Values!";
            }
        }
        catch { }
    }
    protected void btnovercomupdate_Click(object sender, EventArgs e)
    {
        try
        {
            int savecount = 0;
            int errcount = 0;
            if (txtoverbasic.Text.Trim() != "")
            {
                string gradepay = Convert.ToString(txtovergrade.Text);
                string basicpay = Convert.ToString(txtoverbasic.Text);
                string payband = Convert.ToString(txtoverpayband.Text);
                string ismpf = Convert.ToString(txtismpfamntover.Text);
                string ismpfper = Convert.ToString(txtismpfperover.Text);
                string ismpfamnt = "";
                string isautogp = "";
                if (cbismpfover.Checked == true)
                    ismpfamnt = "1";
                if (cbisautogpover.Checked == true)
                    isautogp = "1";
                divovergrdcom.Visible = true;
                grdovercom.Visible = true;
                divovercom.Visible = false;
                DataTable dt = new DataTable();
                dtovercomheader(dt);
                DataRow dr;
                if (Session["overdtcom"] != null)
                {
                    DataTable dnew = (DataTable)Session["overdtcom"];
                    if (dnew.Rows.Count > 0)
                    {
                        for (int ro = 0; ro < dnew.Rows.Count; ro++)
                        {
                            dr = dt.NewRow();
                            for (int col = 0; col < dnew.Columns.Count; col++)
                            {
                                dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    dt.Rows.Clear();
                    dt = getovercomval(dt, dt.NewRow(), gradepay, basicpay, payband, ismpf, ismpfper, ismpfamnt, isautogp);
                    Session["overdtcom"] = dt;
                }
                if (dt.Rows.Count > 0)
                {
                    savecount++;
                    grdovercom.DataSource = dt;
                    grdovercom.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    grdovercom.DataBind();
                    for (int i = 0; i < grdovercom.Columns.Count; i++)
                    {
                        grdovercom.Columns[i].HeaderStyle.Width = 100;
                        grdovercom.Columns[i].ItemStyle.Width = 100;
                    }
                }
                else
                {
                    grdovercom.DataSource = dt;
                    grdovercom.DataBind();
                }
            }
            else
            {
                errcount++;
            }
            if (savecount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Updated Successfully!";
                divovergrdcom.Visible = true;
                divovercom.Visible = false;
                grdovercom.Visible = true;
            }
            if (errcount > 0)
            {
                lblovercomerr.Visible = true;
                lblovercomerr.Text = "Please Fill all the Values!";
            }
        }
        catch { }
    }
    protected void btnovercomdel_Click(object sender, EventArgs e)
    {
        try
        {
            int delcount = 0;
            divovergrdcom.Visible = true;
            grdovercom.Visible = true;
            divovercom.Visible = false;
            DataTable dt = new DataTable();
            dtovercomheader(dt);
            DataRow dr;
            if (Session["overdtcom"] != null)
            {
                DataTable dnew = (DataTable)Session["overdtcom"];
                if (dnew.Rows.Count > 0)
                {
                    for (int ro = 0; ro < dnew.Rows.Count; ro++)
                    {
                        dr = dt.NewRow();
                        for (int col = 0; col < dnew.Columns.Count; col++)
                        {
                            dr[col] = Convert.ToString(dnew.Rows[ro][col]);
                        }
                        dt.Rows.Add(dr);
                    }
                }
                dt.Rows.Clear();
                delcount++;
                Session["overdtcom"] = dt;
            }
            if (dt.Rows.Count > 0)
            {
                grdovercom.DataSource = dt;
                grdovercom.HeaderStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdovercom.DataBind();
                for (int i = 0; i < grdcom.Columns.Count; i++)
                {
                    grdovercom.Columns[i].HeaderStyle.Width = 100;
                    grdovercom.Columns[i].ItemStyle.Width = 100;
                }
            }
            else
            {
                grdovercom.DataSource = dt;
                grdovercom.DataBind();
            }
            if (delcount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully!";
                divovergrdcom.Visible = true;
                grdovercom.Visible = true;
                divovercom.Visible = false;
            }
        }
        catch { }
    }
    protected void btnovercomexit_Click(object sender, EventArgs e)
    {
        divovercom.Visible = false;
        grdovercom.Visible = true;
        divovergrdcom.Visible = true;
    }
    #endregion
    protected void btnoversetgrade_click(object sender, EventArgs e)
    {
        try
        {
            string allowance = "";
            string deduction = "";
            string leavetype = "";
            SortedDictionary<string, string> dicall = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dicall2 = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dicded = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dicded2 = new SortedDictionary<string, string>();
            SortedDictionary<string, string> diclev = new SortedDictionary<string, string>();
            SortedDictionary<string, string> diclev2 = new SortedDictionary<string, string>();
            dicall.Clear();
            dicded.Clear();
            diclev.Clear();
            string[] splall = new string[50];
            string[] splallval = new string[20];
            if (grdoverall.Rows.Count > 0)
            {
                for (int ro = 0; ro < grdoverall.Rows.Count; ro++)
                {
                    string allvalue = "";
                    string inclop = "";
                    string frmbasic = "";
                    string frmbasgp = "";
                    string isspl = "";
                    string frmbasagp = "";
                    Label lblall = (Label)grdoverall.Rows[ro].FindControl("lbl_overalltype");
                    Label lblmode = (Label)grdoverall.Rows[ro].FindControl("lbl_overallmode");
                    Label lblval = (Label)grdoverall.Rows[ro].FindControl("lbl_overallval");
                    if (lblval.Text.Trim().Contains("%"))
                        allvalue = Convert.ToString(lblval.Text).Split('%')[0];
                    else
                        allvalue = Convert.ToString(lblval.Text);
                    Label lblinclop = (Label)grdoverall.Rows[ro].FindControl("lbl_overalllop");
                    if (lblinclop.Text == "Yes")
                        inclop = "1";
                    Label lblfrmbasic = (Label)grdoverall.Rows[ro].FindControl("lbl_overallfrmbasic");
                    if (lblfrmbasic.Text == "Yes")
                        frmbasic = "1";
                    Label lblfrmbasgp = (Label)grdoverall.Rows[ro].FindControl("lbl_overallfrmbasgp");
                    if (lblfrmbasgp.Text == "Yes")
                        frmbasgp = "1";
                    Label lblisspl = (Label)grdoverall.Rows[ro].FindControl("lbl_overallisspl");
                    if (lblisspl.Text == "Yes")
                        isspl = "1";
                    Label lblfrmbasagp = (Label)grdoverall.Rows[ro].FindControl("lbl_overallfrmbasagp");
                    if (lblfrmbasagp.Text == "Yes")
                        frmbasagp = "1";
                    Label lblround = (Label)grdoverall.Rows[ro].FindControl("lbl_overallroundtype");
                    if (allowance.Trim() == "")
                        allowance = Convert.ToString(lblall.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(allvalue) + ";;" + inclop + ";;" + frmbasic + ";;" + frmbasgp + ";" + isspl + ";" + frmbasagp + ";" + Convert.ToString(lblround.Text) + ";;";
                    else
                        allowance = allowance + "\\" + Convert.ToString(lblall.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(allvalue) + ";;" + inclop + ";;" + frmbasic + ";;" + frmbasgp + ";" + isspl + ";" + frmbasagp + ";" + Convert.ToString(lblround.Text) + ";;";
                    if (!dicall.ContainsKey(Convert.ToString(lblall.Text)))
                        dicall.Add(Convert.ToString(lblall.Text), Convert.ToString(lblall.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(allvalue) + ";;" + inclop + ";;" + frmbasic + ";;" + frmbasgp + ";" + isspl + ";" + frmbasagp + ";" + Convert.ToString(lblround.Text) + ";;");
                }
                if (allowance.Trim() != "")
                    allowance = allowance + "\\";
            }
            if (grdoverded.Rows.Count > 0)
            {
                for (int ro = 0; ro < grdoverded.Rows.Count; ro++)
                {
                    string frmgross = "";
                    string frmbasicda = "";
                    string incdedlop = "";
                    string frmbasgpda = "";
                    string frmbas = "";
                    string frmbasdp = "";
                    string frmpetty = "";
                    string frmbasarr = "";
                    string ismaxcal = "";
                    string frmbasarrsa = "";
                    string frmallow = "";
                    string dedvalue = "";
                    Label lblded = (Label)grdoverded.Rows[ro].FindControl("lbl_overdeducttype");
                    Label lblmode = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedmode");
                    Label lblval = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedval");
                    if (lblval.Text.Trim().Contains("%"))
                        dedvalue = Convert.ToString(lblval.Text).Split('%')[0];
                    else
                        dedvalue = Convert.ToString(lblval.Text);
                    Label lblround = (Label)grdoverded.Rows[ro].FindControl("lbl_overrounddedroundtype");
                    Label lblfrmgross = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedfrmgross");
                    if (lblfrmgross.Text == "Yes")
                        frmgross = "1";
                    Label lblfrmbasicda = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedfrmbasicda");
                    if (lblfrmbasicda.Text == "Yes")
                        frmbasicda = "1";
                    Label lblfrmlop = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedfrmlop");
                    if (lblfrmlop.Text == "Yes")
                        incdedlop = "1";
                    Label lbldedgpda = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedgpda");
                    if (lbldedgpda.Text == "Yes")
                        frmbasgpda = "1";
                    Label lbldedfrmbas = (Label)grdoverded.Rows[ro].FindControl("lbl_overdeddedfrmbas");
                    if (lbldedfrmbas.Text == "Yes")
                        frmbas = "1";
                    Label lblfrmbasdp = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedfrmbasdp");
                    if (lblfrmbasdp.Text == "yes")
                        frmbasdp = "1";
                    Label lbldedfrmpetty = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedfrmpetty");
                    if (lbldedfrmpetty.Text == "Yes")
                        frmpetty = "1";
                    Label lbldedfrmbasarr = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedfrmbasarr");
                    if (lbldedfrmbasarr.Text == "Yes")
                        frmbasarr = "1";
                    Label lbldedismaxcal = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedismaxcal");
                    if (lbldedismaxcal.Text == "Yes")
                        ismaxcal = "1";
                    Label lblmax = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedmaxamt");
                    Label lbldedamt = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedamt");
                    Label lblfrmbasarrsa = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedfrmbasarrsa");
                    if (lblfrmbasarrsa.Text == "Yes")
                        frmbasarrsa = "1";
                    Label lbldedfrmallo = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedfrmallow");
                    Label lblcomval = (Label)grdoverded.Rows[ro].FindControl("lbl_overdedfrmnetamnt");
                    if (lblcomval.Text == "Yes")
                        frmallow = "1";
                    if (deduction.Trim() == "")
                        deduction = Convert.ToString(lblded.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(dedvalue) + ";" + frmgross + ";" + frmbasicda + ";;" + incdedlop + ";" + frmbasgpda + ";" + frmbas + ";" + frmbasdp + ";" + frmpetty + ";" + Convert.ToString(lblround.Text) + ";" + ismaxcal + ";" + Convert.ToString(lblmax.Text) + ";" + Convert.ToString(lbldedamt.Text) + ";" + frmbasarr + ";" + frmbasarrsa + ";" + Convert.ToString(lbldedfrmallo.Text) + ";;" + frmallow;
                    else
                        deduction = deduction + "\\" + Convert.ToString(lblded.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(dedvalue) + ";" + frmgross + ";" + frmbasicda + ";;" + incdedlop + ";" + frmbasgpda + ";" + frmbas + ";" + frmbasdp + ";" + frmpetty + ";" + Convert.ToString(lblround.Text) + ";" + ismaxcal + ";" + Convert.ToString(lblmax.Text) + ";" + Convert.ToString(lbldedamt.Text) + ";" + frmbasarr + ";" + frmbasarrsa + ";" + Convert.ToString(lbldedfrmallo.Text) + ";;" + frmallow;
                    if (!dicded.ContainsKey(Convert.ToString(lblded.Text)))
                        dicded.Add(Convert.ToString(lblded.Text), Convert.ToString(lblded.Text) + ";" + Convert.ToString(lblmode.Text) + ";" + Convert.ToString(dedvalue) + ";" + frmgross + ";" + frmbasicda + ";;" + incdedlop + ";" + frmbasgpda + ";" + frmbas + ";" + frmbasdp + ";" + frmpetty + ";" + Convert.ToString(lblround.Text) + ";" + ismaxcal + ";" + Convert.ToString(lblmax.Text) + ";" + Convert.ToString(lbldedamt.Text) + ";" + frmbasarr + ";" + frmbasarrsa + ";" + Convert.ToString(lbldedfrmallo.Text) + ";;" + frmallow);
                }
                if (deduction.Trim() != "")
                    deduction = deduction + "\\";
            }
            if (grdoveralllev.Rows.Count > 0)
            {
                for (int ro = 0; ro < grdoveralllev.Rows.Count; ro++)
                {
                    string incsun = "";
                    string incholy = "";
                    string moncarry = "";
                    string yrcarry = "";
                    Label lbllev = (Label)grdoveralllev.Rows[ro].FindControl("lbl_overlevtype");
                    Label lblyrlev = (Label)grdoveralllev.Rows[ro].FindControl("lbl_overyrlev");
                    Label lblmonlev = (Label)grdoveralllev.Rows[ro].FindControl("lbl_overmonlev");
                    Label lblincsun = (Label)grdoveralllev.Rows[ro].FindControl("lbl_overincsunday");
                    if (lblincsun.Text == "Yes")
                        incsun = "1";
                    Label lblincholy = (Label)grdoveralllev.Rows[ro].FindControl("lbl_overincholiday");
                    if (lblincholy.Text == "Yes")
                        incholy = "1";
                    Label lblmoncarry = (Label)grdoveralllev.Rows[ro].FindControl("lbl_overmoncarry");
                    if (lblmoncarry.Text == "Yes")
                        moncarry = "1";
                    Label lblyrcarry = (Label)grdoveralllev.Rows[ro].FindControl("lbl_overyrcarry");
                    if (lblyrcarry.Text == "Yes")
                        yrcarry = "1";
                    if (leavetype.Trim() == "")
                        leavetype = Convert.ToString(lbllev.Text) + ";" + Convert.ToString(lblyrlev.Text) + ";" + Convert.ToString(lblmonlev.Text) + ";;" + incsun + ";" + incholy + ";" + moncarry + ";" + yrcarry + ";";
                    else
                        leavetype = leavetype + "\\" + Convert.ToString(lbllev.Text) + ";" + Convert.ToString(lblyrlev.Text) + ";" + Convert.ToString(lblmonlev.Text) + ";;" + incsun + ";" + incholy + ";" + moncarry + ";" + yrcarry + ";";
                    if (!diclev.ContainsKey(Convert.ToString(lbllev.Text)))
                        diclev.Add(Convert.ToString(lbllev.Text), Convert.ToString(lbllev.Text) + ";" + Convert.ToString(lblyrlev.Text) + ";" + Convert.ToString(lblmonlev.Text) + ";;" + incsun + ";" + incholy + ";" + moncarry + ";" + yrcarry + ";");
                }
                if (leavetype.Trim() != "")
                    leavetype = leavetype + "\\";
            }
            double gradepay = 0;
            double basicpay = 0.0;
            double payband = 0.0;
            double ismpf = 0.0;
            double ismpfper = 0;
            string ismpfamnt = "";
            string isautogp = "";
            if (grdovercom.Rows.Count > 0)
            {
                for (int ro = 0; ro < grdovercom.Rows.Count; ro++)
                {
                    Label lblgrad = (Label)grdovercom.Rows[ro].FindControl("lbl_overgrad");
                    Label lblbasic = (Label)grdovercom.Rows[ro].FindControl("lbl_overbasicpay");
                    Label lblpayband = (Label)grdovercom.Rows[ro].FindControl("lbl_overpayband");
                    Label lblismpf = (Label)grdovercom.Rows[ro].FindControl("lbl_overismpf");
                    Label lblmpfper = (Label)grdovercom.Rows[ro].FindControl("lbl_overismpfper");
                    Label lblismpfamnt = (Label)grdovercom.Rows[ro].FindControl("lbl_overismpfamnt");
                    if (lblismpfamnt.Text == "Yes")
                        ismpfamnt = "1";
                    else
                        ismpfamnt = "0";
                    Label lblisautogp = (Label)grdovercom.Rows[ro].FindControl("lbl_overisautogp");
                    if (lblisautogp.Text == "Yes")
                        isautogp = "1";
                    else
                        isautogp = "0";
                    double.TryParse(lblgrad.Text, out gradepay);
                    double.TryParse(lblbasic.Text, out basicpay);
                    double.TryParse(lblpayband.Text, out payband);
                    double.TryParse(lblismpf.Text, out ismpf);
                    double.TryParse(lblmpfper.Text, out ismpfper);
                }
            }
            if (checkedOK())
            {
                string scode = "";
                string catcode = "";
                int inscount = 0;
                DataSet dsgetall = new DataSet();
                DataSet dsgetded = new DataSet();
                DataSet dsgetlev = new DataSet();
                FpSpread.SaveChanges();
                for (int sco = 1; sco < FpSpread.Sheets[0].RowCount; sco++)
                {
                    byte check = Convert.ToByte(FpSpread.Sheets[0].Cells[sco, 1].Value);
                    if (check == 1)
                    {
                        inscount = 0;
                        string newallowance = "";
                        string newdeduction = "";
                        string newleavetype = "";
                        dicall2.Clear();
                        dicded2.Clear();
                        diclev2.Clear();
                        scode = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(sco), 2].Text);
                        catcode = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(sco), 6].Tag);
                        string getallow = "select allowances from stafftrans where staff_Code='" + scode + "' and category_code='" + catcode + "' and latestrec='1'";
                        dsgetall.Clear();
                        dsgetall = d2.select_method_wo_parameter(getallow, "Text");
                        string getdeduction = "select deductions from stafftrans where staff_Code='" + scode + "' and category_code='" + catcode + "' and latestrec='1'";
                        dsgetded.Clear();
                        dsgetded = d2.select_method_wo_parameter(getdeduction, "Text");
                        string getleavetype = "select leavetype from individual_Leave_type where college_code='" + clgcode + "' and staff_code='" + scode + "' and category_code='" + catcode + "'";
                        dsgetlev.Clear();
                        dsgetlev = d2.select_method_wo_parameter(getleavetype, "Text");
                        string insquery = "";
                        if (grdoverall.Rows.Count > 0)
                        {
                            if (dsgetall.Tables.Count > 0 && dsgetall.Tables[0].Rows.Count > 0)
                            {
                                splall = Convert.ToString(dsgetall.Tables[0].Rows[0]["allowances"]).Split('\\');
                                if (splall.Length > 0)
                                {
                                    for (int ik = 0; ik < splall.Length; ik++)
                                    {
                                        splallval = splall[ik].Split(';');
                                        if (splallval.Length > 0 && splallval[0].Trim() != "")
                                        {
                                            if (dicall.ContainsKey(splallval[0]))
                                            {
                                                if (newallowance.Trim() == "")
                                                    newallowance = Convert.ToString(dicall[Convert.ToString(splallval[0])]);
                                                else
                                                    newallowance = newallowance + "\\" + Convert.ToString(dicall[Convert.ToString(splallval[0])]);
                                                if (!dicall2.ContainsKey(Convert.ToString(splallval[0])))
                                                    dicall2.Add(Convert.ToString(splallval[0]), Convert.ToString(dicall[Convert.ToString(splallval[0])]));
                                            }
                                            else
                                            {
                                                if (!dicall2.ContainsKey(Convert.ToString(splallval[0])))
                                                {
                                                    dicall2.Add(Convert.ToString(splallval[0]), Convert.ToString(splall[ik]));
                                                    if (newallowance.Trim() == "")
                                                        newallowance = Convert.ToString(splall[ik]);
                                                    else
                                                        newallowance = newallowance + "\\" + Convert.ToString(splall[ik]);
                                                }
                                            }
                                        }
                                    }
                                    foreach (KeyValuePair<string, string> dr in dicall)
                                    {
                                        if (!dicall2.ContainsKey(dr.Key))
                                        {
                                            dicall2.Add(Convert.ToString(dr.Key), Convert.ToString(dr.Value));
                                            if (newallowance.Trim() == "")
                                                newallowance = Convert.ToString(dr.Value);
                                            else
                                                newallowance = newallowance + "\\" + Convert.ToString(dr.Value);
                                        }
                                    }
                                }
                                if (newallowance.Trim() != "")
                                    newallowance = newallowance + "\\";
                            }
                            insquery = "if exists(select * from stafftrans where staff_code='" + scode + "' and latestrec='1' and category_code='" + catcode + "') Update stafftrans set allowances='" + newallowance.Trim() + "' where staff_code='" + scode + "' and latestrec='1' and category_code='" + catcode + "'";
                            int updallow = d2.update_method_wo_parameter(insquery, "Text");
                            if (updallow > 0)
                            {
                                inscount++;
                            }
                        }
                        if (grdoverded.Rows.Count > 0)
                        {
                            if (dsgetded.Tables.Count > 0 && dsgetded.Tables[0].Rows.Count > 0)
                            {
                                splall = Convert.ToString(dsgetded.Tables[0].Rows[0]["deductions"]).Split('\\');
                                if (splall.Length > 0)
                                {
                                    for (int ik = 0; ik < splall.Length; ik++)
                                    {
                                        splallval = splall[ik].Split(';');
                                        if (splallval.Length > 0 && splallval[0].Trim() != "")
                                        {
                                            if (dicded.ContainsKey(splallval[0]))
                                            {
                                                if (newdeduction.Trim() == "")
                                                    newdeduction = Convert.ToString(dicded[Convert.ToString(splallval[0])]);
                                                else
                                                    newdeduction = newdeduction + "\\" + Convert.ToString(dicded[Convert.ToString(splallval[0])]);
                                                if (!dicded2.ContainsKey(Convert.ToString(splallval[0])))
                                                    dicded2.Add(Convert.ToString(splallval[0]), Convert.ToString(dicded[Convert.ToString(splallval[0])]));
                                            }
                                            else
                                            {
                                                if (!dicded2.ContainsKey(Convert.ToString(splallval[0])))
                                                {
                                                    dicded2.Add(Convert.ToString(splallval[0]), Convert.ToString(splall[ik]));
                                                    if (newdeduction.Trim() == "")
                                                        newdeduction = Convert.ToString(splall[ik]);
                                                    else
                                                        newdeduction = newdeduction + "\\" + Convert.ToString(splall[ik]);
                                                }
                                            }
                                        }
                                    }
                                    foreach (KeyValuePair<string, string> dr in dicded)
                                    {
                                        if (!dicded2.ContainsKey(dr.Key))
                                        {
                                            dicded2.Add(Convert.ToString(dr.Key), Convert.ToString(dr.Value));
                                            if (newdeduction.Trim() == "")
                                                newdeduction = Convert.ToString(dr.Value);
                                            else
                                                newdeduction = newdeduction + "\\" + Convert.ToString(dr.Value);
                                        }
                                    }
                                }
                                if (newdeduction.Trim() != "")
                                    newdeduction = newdeduction + "\\";
                            }
                            insquery = "if exists(select * from stafftrans where staff_code='" + scode + "' and latestrec='1' and category_code='" + catcode + "') Update stafftrans set deductions='" + newdeduction.Trim() + "' where staff_code='" + scode + "' and latestrec='1' and category_code='" + catcode + "'";
                            int updded = d2.update_method_wo_parameter(insquery, "Text");
                            if (updded > 0)
                            {
                                inscount++;
                            }
                        }
                        if (grdoveralllev.Rows.Count > 0) // Modify by jairam 20-07-2017 
                        {
                            if (dsgetlev.Tables.Count > 0 && dsgetlev.Tables[0].Rows.Count > 0)
                            {
                                splall = Convert.ToString(dsgetlev.Tables[0].Rows[0]["leavetype"]).Split('\\');
                                if (splall.Length > 0)
                                {
                                    for (int ik = 0; ik < splall.Length; ik++)
                                    {
                                        splallval = splall[ik].Split(';');
                                        if (splallval.Length > 0 && splallval[0].Trim() != "")
                                        {
                                            if (diclev.ContainsKey(splallval[0]))
                                            {
                                                if (newleavetype.Trim() == "")
                                                    newleavetype = Convert.ToString(diclev[Convert.ToString(splallval[0])]);
                                                else
                                                    newleavetype = newleavetype + "\\" + Convert.ToString(diclev[Convert.ToString(splallval[0])]);
                                                if (!diclev2.ContainsKey(Convert.ToString(splallval[0])))
                                                    diclev2.Add(Convert.ToString(splallval[0]), Convert.ToString(diclev[Convert.ToString(splallval[0])]));
                                            }
                                            else
                                            {
                                                if (!diclev2.ContainsKey(Convert.ToString(splallval[0])))
                                                {
                                                    diclev2.Add(Convert.ToString(splallval[0]), Convert.ToString(splall[ik]));
                                                    if (newleavetype.Trim() == "")
                                                        newleavetype = Convert.ToString(splall[ik]);
                                                    else
                                                        newleavetype = newleavetype + "\\" + Convert.ToString(splall[ik]);
                                                }
                                            }
                                        }
                                    }
                                    foreach (KeyValuePair<string, string> dr in diclev)
                                    {
                                        if (!diclev2.ContainsKey(dr.Key))
                                        {
                                            diclev2.Add(Convert.ToString(dr.Key), Convert.ToString(dr.Value));
                                            if (newleavetype.Trim() == "")
                                                newleavetype = Convert.ToString(dr.Value);
                                            else
                                                newleavetype = newleavetype + "\\" + Convert.ToString(dr.Value);
                                        }
                                    }
                                }
                                if (newleavetype.Trim() != "")
                                    newleavetype = newleavetype + "\\";
                            }
                            insquery = " if exists(select * from individual_Leave_type where college_code='" + clgcode + "' and staff_code='" + scode + "' and category_code='" + catcode + "') Update individual_Leave_type set leavetype='" + newleavetype.Trim() + "' where staff_code='" + scode + "' and college_code='" + clgcode + "' and category_code='" + catcode + "' else insert into individual_Leave_type (staff_code,leavetype,college_code,category_code) Values ('" + scode + "','" + newleavetype.Trim() + "','" + clgcode + "','" + catcode + "')";
                            int updlev = d2.update_method_wo_parameter(insquery, "Text");
                            if (updlev > 0)
                            {
                                inscount++;
                            }
                        }
                        if (grdovercom.Rows.Count > 0)
                        {
                            insquery = "if exists(select * from stafftrans where staff_code='" + scode + "' and latestrec='1' and category_code='" + catcode + "') Update stafftrans set grade_pay='" + Convert.ToString(gradepay) + "',bsalary='" + Convert.ToString(basicpay) + "',pay_band='" + Convert.ToString(payband) + "',IsMPFAmt='" + ismpfamnt + "',MPFAmount='" + Convert.ToString(ismpf) + "',MPFPer='" + Convert.ToString(ismpfper) + "',IsAutoGP='" + isautogp + "' where staff_code='" + scode + "' and latestrec='1' and category_code='" + catcode + "'";
                            int updcom = d2.update_method_wo_parameter(insquery, "Text");
                            if (updcom > 0)
                            {
                                inscount++;
                            }
                        }
                    }
                }
                if (inscount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Grade Pay Updated Successfully!";
                    //ViewState["strall"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, clgcode, "GradePayMaster.aspx");
        }
    }
    protected void btnoversetgrade_exit_click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
        btn_go_Click(sender, e);
    }
    public DataTable getallval(DataTable dt, string[] allowval, string alowancesplit, DataRow dr)
    {
        try
        {
            for (int i = 0; i < allowval.Length; i++)
            {
                alowancesplit = allowval[i];
                if (alowancesplit.Trim() != "")
                {
                    string[] allowanceda;
                    allowanceda = alowancesplit.Split(';');
                    if (allowanceda[2].Trim() != "")
                    {
                        dr = dt.NewRow();
                        dr["alltype"] = Convert.ToString(allowanceda[0]);
                        dr["mode"] = Convert.ToString(allowanceda[1]);
                        if (allowanceda[1] == "Amount")
                            dr["value"] = Convert.ToString(allowanceda[2]);
                        if (allowanceda[1] == "Percent")
                            dr["value"] = Convert.ToString(allowanceda[2]) + "%";
                        if (allowanceda[1] == "Slab")
                            dr["value"] = Convert.ToString(allowanceda[2]);
                        if (Convert.ToString(allowanceda[4]) == "1")
                            dr["inclop"] = "Yes";
                        else
                            dr["inclop"] = "No";
                        if (Convert.ToString(allowanceda[6]) == "1")
                            dr["frmbasic"] = "Yes";
                        else
                            dr["frmbasic"] = "No";
                        if (allowanceda.Length >= 9)
                        {
                            if (Convert.ToString(allowanceda[8]) == "1")
                                dr["frmbasgp"] = "Yes";
                            else
                                dr["frmbasgp"] = "No";
                        }
                        if (allowanceda.Length >= 10)
                        {
                            if (Convert.ToString(allowanceda[9]) == "1")
                                dr["isspl"] = "Yes";
                            else
                                dr["isspl"] = "No";
                        }
                        if (allowanceda.Length >= 11)
                        {
                            if (Convert.ToString(allowanceda[10]) == "1")
                                dr["frmbasagp"] = "Yes";
                            else
                                dr["frmbasagp"] = "No";
                        }
                        if (allowanceda.Length >= 12)
                            dr["roundval"] = Convert.ToString(allowanceda[11]);
                        if (allowanceda.Length >= 13)
                        {

                            dr["FromAllow"] = Convert.ToString(allowanceda[13]);//delsi0405
                        }

                        dt.Rows.Add(dr);
                    }
                }
            }
        }
        catch { }
        return dt;
    }
    public DataTable getdedval(DataTable dt, string[] allowval, string alowancesplit, DataRow dr)
    {
        try
        {
            for (int i = 0; i < allowval.Length; i++)
            {
                alowancesplit = allowval[i];
                if (alowancesplit.Trim() != "")
                {
                    string[] allowanceda;
                    allowanceda = alowancesplit.Split(';');
                    if (allowanceda[2].Trim() != "")
                    {
                        dr = dt.NewRow();
                        dr["dedtype"] = Convert.ToString(allowanceda[0]);
                        dr["mode"] = Convert.ToString(allowanceda[1]);
                        if (allowanceda[1] == "Amount")
                            dr["value"] = Convert.ToString(allowanceda[2]);
                        if (allowanceda[1] == "Percent")
                            dr["value"] = Convert.ToString(allowanceda[2]) + "%";
                        if (allowanceda[1] == "Slab")
                            dr["value"] = Convert.ToString(allowanceda[2]);
                        dr["dedround"] = Convert.ToString(allowanceda[11]);
                        if (Convert.ToString(allowanceda[3]) == "1")
                            dr["frmgross"] = "Yes";
                        else
                            dr["frmgross"] = "No";
                        if (Convert.ToString(allowanceda[4]) == "1")
                            dr["frmbasicda"] = "Yes";
                        else
                            dr["frmbasicda"] = "No";
                        if (Convert.ToString(allowanceda[6]) == "1")
                            dr["inclop"] = "Yes";
                        else
                            dr["inclop"] = "No";
                        if (Convert.ToString(allowanceda[7]) == "1")
                            dr["frmbasgpda"] = "Yes";
                        else
                            dr["frmbasgpda"] = "No";
                        if (Convert.ToString(allowanceda[8]) == "1")
                            dr["frmbas"] = "Yes";
                        else
                            dr["frmbas"] = "No";
                        if (Convert.ToString(allowanceda[9]) == "1")
                            dr["frmbasdp"] = "Yes";
                        else
                            dr["frmbasdp"] = "No";
                        if (Convert.ToString(allowanceda[10]) == "1")
                            dr["frmpetty"] = "Yes";
                        else
                            dr["frmpetty"] = "No";
                        if (Convert.ToString(allowanceda[12]) == "1")
                            dr["ismaxcal"] = "Yes";
                        else
                            dr["ismaxcal"] = "No";
                        dr["maxamnt"] = Convert.ToString(allowanceda[13]);
                        dr["dedamt"] = Convert.ToString(allowanceda[14]);
                        if (allowanceda.Length >= 16)
                        {
                            if (Convert.ToString(allowanceda[15]) == "1")
                                dr["frmbasarr"] = "Yes";
                            else
                                dr["frmbasarr"] = "No";
                        }
                        if (allowanceda.Length >= 17)
                        {
                            if (Convert.ToString(allowanceda[16]) == "1")
                                dr["frmbasarrsa"] = "Yes";
                            else
                                dr["frmbasarrsa"] = "No";
                        }
                        if (allowanceda.Length >= 18)
                            dr["frmallow"] = Convert.ToString(allowanceda[17]);
                        if (allowanceda.Length >= 20)
                        {
                            if (Convert.ToString(allowanceda[19]) == "1")
                                dr["frmnetamnt"] = "Yes";
                            else
                                dr["frmnetamnt"] = "No";
                        }
                        if (allowanceda.Length >= 21) //poomalar 24.10.17
                        {
                            if (Convert.ToString(allowanceda[20]) == "1")
                                dr["GrossLOP"] = "Yes";
                            else
                                dr["GrossLOP"] = "No";
                        }
                        if (allowanceda.Length >= 22)
                        {
                            if (Convert.ToString(allowanceda[21]) == "1")
                                dr["GrossMinusLOP"] = "Yes";
                            else
                                dr["GrossMinusLOP"] = "No";

                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
        }
        catch { }
        return dt;
    }
    public DataTable getlevval(DataTable dt, string[] allowval, string alowancesplit, DataRow dr)
    {
        try
        {
            for (int i = 0; i < allowval.Length; i++)
            {
                alowancesplit = allowval[i];
                if (alowancesplit.Trim() != "")
                {
                    string[] allowanceda;
                    allowanceda = alowancesplit.Split(';');
                    if (allowanceda[1].Trim() != "")
                    {
                        dr = dt.NewRow();
                        dr["levtype"] = Convert.ToString(allowanceda[0]);
                        dr["yrlev"] = Convert.ToString(allowanceda[1]);
                        dr["monlev"] = Convert.ToString(allowanceda[2]);
                        if (Convert.ToString(allowanceda[3]) == "1")
                            dr["incsunday"] = "Yes";
                        else
                            dr["incsunday"] = "No";
                        if (Convert.ToString(allowanceda[4]) == "1")
                            dr["incholiday"] = "Yes";
                        else
                            dr["incholiday"] = "No";
                        if (Convert.ToString(allowanceda[5]) == "1")
                            dr["moncarry"] = "Yes";
                        else
                            dr["moncarry"] = "No";
                        if (Convert.ToString(allowanceda[6]) == "1")
                            dr["yrcarry"] = "Yes";
                        else
                            dr["yrcarry"] = "No";
                        dr["MonthlyMaxLeave"] = Convert.ToString(allowanceda[6]);
                        dt.Rows.Add(dr);
                    }
                }
            }
        }
        catch { }
        return dt;
    }
    public DataTable getcomval(DataTable dt, DataRow dr, string gradepay, string basicpay, string payband, string ismpf, string ismpfper, string ismpfamnt, string isautogp)
    {
        try
        {
            dr = dt.NewRow();
            dr["gradepay"] = Convert.ToString(gradepay);
            dr["basicpay"] = Convert.ToString(basicpay);
            dr["payband"] = Convert.ToString(payband);
            dr["ismpf"] = Convert.ToString(ismpf);
            dr["ismpfper"] = Convert.ToString(ismpfper);
            if (ismpfamnt == "1")
                dr["ismpfamnt"] = "Yes";
            else
                dr["ismpfamnt"] = "No";
            if (isautogp == "1")
                dr["isautogp"] = "Yes";
            else
                dr["isautogp"] = "No";
            dt.Rows.Add(dr);
        }
        catch { }
        return dt;
    }
    public DataTable getoverallval(DataTable dt, string[] allowval, string alowancesplit, DataRow dr)
    {
        try
        {
            for (int i = 0; i < allowval.Length; i++)
            {
                alowancesplit = allowval[i];
                if (alowancesplit.Trim() != "")
                {
                    string[] allowanceda;
                    allowanceda = alowancesplit.Split(';');
                    if (allowanceda[2].Trim() != "")
                    {
                        dr = dt.NewRow();
                        dr["overalltype"] = Convert.ToString(allowanceda[0]);
                        dr["overallmode"] = Convert.ToString(allowanceda[1]);
                        if (allowanceda[1] == "Amount")
                            dr["overallvalue"] = Convert.ToString(allowanceda[2]);
                        if (allowanceda[1] == "Percent")
                            dr["overallvalue"] = Convert.ToString(allowanceda[2]) + "%";
                        if (allowanceda[1] == "Slab")
                            dr["overallvalue"] = Convert.ToString(allowanceda[2]);
                        if (Convert.ToString(allowanceda[4]) == "1")
                            dr["overallinclop"] = "Yes";
                        else
                            dr["overallinclop"] = "No";
                        if (Convert.ToString(allowanceda[6]) == "1")
                            dr["overallfrmbasic"] = "Yes";
                        else
                            dr["overallfrmbasic"] = "No";
                        if (allowanceda.Length >= 9)
                        {
                            if (Convert.ToString(allowanceda[8]) == "1")
                                dr["overallfrmbasgp"] = "Yes";
                            else
                                dr["overallfrmbasgp"] = "No";
                        }
                        if (allowanceda.Length >= 10)
                        {
                            if (Convert.ToString(allowanceda[9]) == "1")
                                dr["overallisspl"] = "Yes";
                            else
                                dr["overallisspl"] = "No";
                        }
                        if (allowanceda.Length >= 11)
                        {
                            if (Convert.ToString(allowanceda[10]) == "1")
                                dr["overallfrmbasagp"] = "Yes";
                            else
                                dr["overallfrmbasagp"] = "No";
                        }
                        if (allowanceda.Length >= 12)
                            dr["overallroundval"] = Convert.ToString(allowanceda[11]);
                        dt.Rows.Add(dr);
                    }
                }
            }
        }
        catch { }
        return dt;
    }
    public DataTable getoverdedval(DataTable dt, string[] allowval, string alowancesplit, DataRow dr)
    {
        try
        {
            for (int i = 0; i < allowval.Length; i++)
            {
                alowancesplit = allowval[i];
                if (alowancesplit.Trim() != "")
                {
                    string[] allowanceda;
                    allowanceda = alowancesplit.Split(';');
                    if (allowanceda[2].Trim() != "")
                    {
                        dr = dt.NewRow();
                        dr["overdedtype"] = Convert.ToString(allowanceda[0]);
                        dr["overdedmode"] = Convert.ToString(allowanceda[1]);
                        if (allowanceda[1] == "Amount")
                            dr["overdedvalue"] = Convert.ToString(allowanceda[2]);
                        if (allowanceda[1] == "Percent")
                            dr["overdedvalue"] = Convert.ToString(allowanceda[2]) + "%";
                        if (allowanceda[1] == "Slab")
                            dr["overdedvalue"] = Convert.ToString(allowanceda[2]);
                        dr["overdedround"] = Convert.ToString(allowanceda[11]);
                        if (Convert.ToString(allowanceda[3]) == "1")
                            dr["overdedfrmgross"] = "Yes";
                        else
                            dr["overdedfrmgross"] = "No";
                        if (Convert.ToString(allowanceda[4]) == "1")
                            dr["overdedfrmbasicda"] = "Yes";
                        else
                            dr["overdedfrmbasicda"] = "No";
                        if (Convert.ToString(allowanceda[6]) == "1")
                            dr["overdedinclop"] = "Yes";
                        else
                            dr["overdedinclop"] = "No";
                        if (Convert.ToString(allowanceda[7]) == "1")
                            dr["overdedfrmbasgpda"] = "Yes";
                        else
                            dr["overdedfrmbasgpda"] = "No";
                        if (Convert.ToString(allowanceda[8]) == "1")
                            dr["overdedfrmbas"] = "Yes";
                        else
                            dr["overdedfrmbas"] = "No";
                        if (Convert.ToString(allowanceda[9]) == "1")
                            dr["overdedfrmbasdp"] = "Yes";
                        else
                            dr["overdedfrmbasdp"] = "No";
                        if (Convert.ToString(allowanceda[10]) == "1")
                            dr["overdedfrmpetty"] = "Yes";
                        else
                            dr["overdedfrmpetty"] = "No";
                        if (Convert.ToString(allowanceda[12]) == "1")
                            dr["overdedismaxcal"] = "Yes";
                        else
                            dr["overdedismaxcal"] = "No";
                        dr["overdedmaxamnt"] = Convert.ToString(allowanceda[13]);
                        dr["overdedamt"] = Convert.ToString(allowanceda[14]);
                        if (allowanceda.Length >= 16)
                        {
                            if (Convert.ToString(allowanceda[15]) == "1")
                                dr["overdedfrmbasarr"] = "Yes";
                            else
                                dr["overdedfrmbasarr"] = "No";
                        }
                        if (allowanceda.Length >= 17)
                        {
                            if (Convert.ToString(allowanceda[16]) == "1")
                                dr["overdedfrmbasarrsa"] = "Yes";
                            else
                                dr["overdedfrmbasarrsa"] = "No";
                        }
                        if (allowanceda.Length >= 18)
                            dr["overdedfrmallow"] = Convert.ToString(allowanceda[17]);
                        if (allowanceda.Length >= 20)
                        {
                            if (Convert.ToString(allowanceda[19]) == "1")
                                dr["overdedfrmnetamnt"] = "Yes";
                            else
                                dr["overdedfrmnetamnt"] = "No";
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
        }
        catch { }
        return dt;
    }
    public DataTable getoverlevval(DataTable dt, string[] allowval, string alowancesplit, DataRow dr)
    {
        try
        {
            for (int i = 0; i < allowval.Length; i++)
            {
                alowancesplit = allowval[i];
                if (alowancesplit.Trim() != "")
                {
                    string[] allowanceda;
                    allowanceda = alowancesplit.Split(';');
                    if (allowanceda[1].Trim() != "")
                    {
                        dr = dt.NewRow();
                        dr["overlevtype"] = Convert.ToString(allowanceda[0]);
                        dr["overyrlev"] = Convert.ToString(allowanceda[1]);
                        dr["overmonlev"] = Convert.ToString(allowanceda[2]);
                        if (Convert.ToString(allowanceda[3]) == "1")
                            dr["overincsunday"] = "Yes";
                        else
                            dr["overincsunday"] = "No";
                        if (Convert.ToString(allowanceda[4]) == "1")
                            dr["overincholiday"] = "Yes";
                        else
                            dr["overincholiday"] = "No";
                        if (Convert.ToString(allowanceda[5]) == "1")
                            dr["overmoncarry"] = "Yes";
                        else
                            dr["overmoncarry"] = "No";
                        if (Convert.ToString(allowanceda[6]) == "1")
                            dr["overyrcarry"] = "Yes";
                        else
                            dr["overyrcarry"] = "No";
                        dt.Rows.Add(dr);
                    }
                }
            }
        }
        catch { }
        return dt;
    }
    public DataTable getovercomval(DataTable dt, DataRow dr, string gradepay, string basicpay, string payband, string ismpf, string ismpfper, string ismpfamnt, string isautogp)
    {
        try
        {
            dr = dt.NewRow();
            dr["overgradepay"] = Convert.ToString(gradepay);
            dr["overbasicpay"] = Convert.ToString(basicpay);
            dr["overpayband"] = Convert.ToString(payband);
            dr["overismpf"] = Convert.ToString(ismpf);
            dr["overismpfper"] = Convert.ToString(ismpfper);
            if (ismpfamnt == "1")
                dr["overismpfamnt"] = "Yes";
            else
                dr["overismpfamnt"] = "No";
            if (isautogp == "1")
                dr["overisautogp"] = "Yes";
            else
                dr["overisautogp"] = "No";
            dt.Rows.Add(dr);
        }
        catch { }
        return dt;
    }
    #endregion



    protected void cb_fromallallow_CheckedChange(object sender, EventArgs e)//delsi0405
    {
        if (cb_fromallallow.Checked == true)
        {
            //  divallhead.Visible = true;
            //   lblheadset.Text = lbl_h2.Text;
            divallHeadallow.Visible = true;

            lblheaderrs.Visible = false;

            lblheadsetallow.Text = lbl_header1.Text;
            //  chkdeddisable();
            chkalldisable();

            cb_fromallallow.Checked = true;
            cb_fromallallow.Enabled = true;


            allowance();
        }
        else
        {
            //chkdedenable();
            chkalldisable();
        }

    }

    protected void imgallheadallow_Click(object sender, EventArgs e)
    {
        divallHeadallow.Visible = false;
    }


    protected void btnMvOneRt_Click_allow(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lb_selbasgrads.Items.Count > 0 && lb_selbasgrads.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_selallows.Items.Count; j++)
                {
                    if (lb_selallows.Items[j].Value == lb_selbasgrads.SelectedItem.Value)
                        ok = false;
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lb_selbasgrads.SelectedItem.Text, lb_selbasgrads.SelectedItem.Value);
                    lb_selallows.Items.Add(lst);
                }
            }
            bool nxtok = true;
            if (lb_allowhdrs.Items.Count > 0 && lb_allowhdrs.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_selallows.Items.Count; j++)
                {
                    if (lb_selallows.Items[j].Value == lb_allowhdrs.SelectedItem.Value)
                        nxtok = false;
                }
                if (nxtok)
                {
                    ListItem lstnew = new ListItem(lb_allowhdrs.SelectedItem.Text, lb_allowhdrs.SelectedItem.Value);
                    lb_selallows.Items.Add(lstnew);
                }
            }
        }
        catch { }
    }

    protected void btnMvTwoRt_Click_allow(object sender, EventArgs e)
    {
        try
        {
            lb_selallows.Items.Clear();
            if (lb_selbasgrads.Items.Count > 0)
            {
                for (int j = 0; j < lb_selbasgrads.Items.Count; j++)
                {
                    lb_selallows.Items.Add(new ListItem(lb_selbasgrads.Items[j].Text.ToString(), lb_selbasgrads.Items[j].Value.ToString()));
                }
            }
            if (lb_allowhdrs.Items.Count > 0)
            {
                for (int j = 0; j < lb_allowhdrs.Items.Count; j++)
                {
                    lb_selallows.Items.Add(new ListItem(lb_allowhdrs.Items[j].Text.ToString(), lb_allowhdrs.Items[j].Value.ToString()));
                }
            }
        }
        catch { }
    }
    protected void btnMvOneLt_Click_allow(object sender, EventArgs e)
    {
        if (lb_selallows.Items.Count > 0 && lb_selallows.SelectedItem.Value != "")
            lb_selallows.Items.RemoveAt(lb_selallows.SelectedIndex);
    }

    protected void btnMvTwoLt_Click_allow(object sender, EventArgs e)
    {
        lb_selallows.Items.Clear();
    }

    protected void btnokall_click_allow(object sender, EventArgs e)
    {
        try
        {
            string getallval = "";
            if (lb_selallows.Items.Count > 0)
            {
                lblheaderrs.Visible = false;
                for (int ro = 0; ro < lb_selallows.Items.Count; ro++)
                {
                    if (getallval.Trim() == "")
                        getallval = Convert.ToString(lb_selallows.Items[ro].Text);
                    else
                        getallval = getallval + "+" + Convert.ToString(lb_selallows.Items[ro].Text);
                }

                txt_all_allowVal.Text = getallval;
                // txtoverdedall.Text = getallval;
                divallHeadallow.Visible = false;
            }
            else
            {
                lblheaderrs.Visible = true;
                lblheaderrs.Text = "Please select any one Header!";
            }
        }
        catch { }
    }
    protected void btnexitallow_click_allow(object sender, EventArgs e)
    {
        divallHeadallow.Visible = false;
    }
    protected void lnk_note_click(object sender, EventArgs e)
    {
        try
        {
            DivNote.Visible = true;
            noteid.Text = "* From Selected allow(LOP)-> Calculating Deduction Amount eg(EPF/ESI) From Selected allow and LOP Calculates from Selected allow";

        }
        catch (Exception ex)
        {

        }

    }
    protected void img_note_Click(object sender, EventArgs e)
    {
        DivNote.Visible = false;
    }
    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";

            }
        }
    }
    protected void OnRowCreate_deduct(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";

            }

        }

    }
    protected void OnRowCreated_Leave(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";

            }

        }

    }
    protected void OnRowCreated_Common(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";

            }

        }

    }
    protected void OnRowDataBound_gv1(object sender, GridViewRowEventArgs e)
    {
    }

    protected void txt_y1_txtchange(object sender, EventArgs e)
    {
        try
        {
            DataSet paymonth = new DataSet();
            GV1.Visible = false;
            if (txt_yl.Text != "")
            {

                string selmaxLeavCount = d2.GetFunction("select value from Master_Settings where settings='StaffMaxLeavePerMonth' and usercode='" + usercode + "'");
                if (selmaxLeavCount.Trim() != "" && selmaxLeavCount.Trim() != "0" && selmaxLeavCount.Trim() == "1")
                {
                    if (txt_yl.Text != "")
                    {
                        string queryObject = "select * from hrpaymonths where college_code='" + Session["collegecode"] + "' and SelStatus='1'";
                        paymonth = d2.select_method_wo_parameter(queryObject, "Text");
                        DataTable dtmaxleave = new DataTable();
                        if (paymonth.Tables.Count > 0 && paymonth.Tables[0].Rows.Count > 0)
                        {

                            dtmaxleave.Columns.Add("Lblmonth");
                            dtmaxleave.Columns.Add("ddlmax");
                            dtmaxleave.Columns.Add("txtfdate");
                            dtmaxleave.Columns.Add("txttdate");
                           
                            DataRow dr = null;
                            for (int val = 0; val < paymonth.Tables[0].Rows.Count; val++)
                            {
                                string paymonthval = Convert.ToString(paymonth.Tables[0].Rows[val]["PayMonth"]);
                                DateTime fromdates = Convert.ToDateTime(paymonth.Tables[0].Rows[val]["From_Date"]);
                                DateTime todates = Convert.ToDateTime(paymonth.Tables[0].Rows[val]["To_Date"]);

                                dr = dtmaxleave.NewRow();
                                dr["Lblmonth"] = paymonthval;
                                dr["txtfdate"] = fromdates.ToString("dd/MM/yyyy");
                                dr["txttdate"] = todates.ToString("dd/MM/yyyy");


                                dtmaxleave.Rows.Add(dr);

                            }

                        }
                        if (dtmaxleave.Rows.Count > 0)
                        {
                            GV1.DataSource = dtmaxleave;
                            GV1.DataBind();
                            GV1.Visible = true;

                            foreach (GridViewRow gr in GV1.Rows)
                            {
                                DropDownList ddlmax = (gr.FindControl("ddlmaxleave") as DropDownList);

                                for (int i = 0; i <= Convert.ToInt32(txt_yl.Text); i++)
                                {
                                    ddlmax.Items.Insert(i, Convert.ToString(i) == "0" ? "Select" : Convert.ToString(i));
                                }


                            }

                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Enter Yearly Leave')", true);
                    }
                }

            }
        }
        catch (Exception ex)
        {

        }

    }
}