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
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using Gios.Pdf;
using System.Text.RegularExpressions;

public partial class HR_Finance : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DataSet ds11 = new DataSet();
    string dte = "";
    string college_code = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";

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
            college_code = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!IsPostBack)
            {
                //string str = "select PayMonth,PayMonthNum from HrPayMonths where College_Code='" + college_code + "' and selstatus='1'";
                string str = "select distinct PayMonth,PayMonthNum from HrPayMonths where College_Code='" + college_code + "' order by PayMonthNum asc";
                ds = da.select_method_wo_parameter(str, "Text");
                ddlmonth.DataSource = ds;
                ddlmonth.DataTextField = "PayMonth";
                ddlmonth.DataValueField = "PayMonthNum";
                ddlmonth.DataBind();
                ddlmonth.Items.Insert(0, "---Select---");
                ddltomonth.Items.Insert(0, "---Select---");

                BindDepartment();
                Binddesig();
                bindcate();
                year(dte);
                year1(dte);
            }
        }
        catch (Exception ex) { }
    }

    public void year(string date)
    {
        try
        {
            ds11.Clear();
            ddlyear.Items.Clear();
            string year = "";
            if (date.Trim() == "")
            {
                year = "select distinct PayYear as year from HrPayMonths  where College_Code='" + college_code + "' order by year asc";
            }
            else
            {
                year = "select distinct PayYear as year from HrPayMonths  where College_Code='" + college_code + "' and PayMonthNum =" + date + " order by year asc";
            }
            ds11 = da.select_method_wo_parameter(year, "text");
            if (ds11.Tables[0].Rows.Count > 0)
            {
                ddlyear.DataSource = ds11;
                ddlyear.DataTextField = "year";
                ddlyear.DataValueField = "year";
                ddlyear.DataBind();

                ddlyear2.DataSource = ds11;
                ddlyear2.DataTextField = "year";
                ddlyear2.DataValueField = "year";
                ddlyear2.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    public void year1(string date)
    {
        try
        {
            ds11.Clear();
            ddlyear.Items.Clear();
            string year = "";
            if (date.Trim() == "")
            {
                year = "select distinct PayYear as year from HrPayMonths  where College_Code='" + college_code + "' order by year asc";
            }
            else
            {
                year = "select distinct PayYear as year from HrPayMonths  where College_Code='" + college_code + "' and PayMonthNum =" + date + " order by year asc";
            }
            ds11 = da.select_method_wo_parameter(year, "text");
            if (ds11.Tables[0].Rows.Count > 0)
            {
                ddlyear.DataSource = ds11;
                ddlyear.DataTextField = "year";
                ddlyear.DataValueField = "year";
                ddlyear.DataBind();

                ddlyear2.DataSource = ds11;
                ddlyear2.DataTextField = "year";
                ddlyear2.DataValueField = "year";
                ddlyear2.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void ddlyear22(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    public void BindDepartment()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            chkls_dept.Items.Clear();
            txt_dept.Text = "---Select---";
            chk_deptall.Checked = false;
            string deptquery = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') ";
            }

            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["college_code"] + "') ";
            }

            if (deptquery != "")
            {
                ds = da.select_method_wo_parameter(deptquery, "Text");
                chkls_dept.DataSource = ds;
                chkls_dept.DataTextField = "dept_name";
                chkls_dept.DataValueField = "Dept_Code";
                chkls_dept.DataBind();
                for (int item = 0; item < chkls_dept.Items.Count; item++)
                {
                    chkls_dept.Items[item].Selected = true;
                    chk_deptall.Checked = true;
                }
                if (chkls_dept.Items.Count > 0)
                {
                    txt_dept.Text = "Department(" + chkls_dept.Items.Count + ")";
                    chk_deptall.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    public void Binddesig()
    {
        try
        {
            clear();
            ds.Clear();
            cbl_Designation.Visible = true;
            cbl_Designation.Items.Clear();

            string col = college_code;
            if (col == "---Select---")
            {
                col = Session["college_code"].ToString();
            }
            txt_designation.Text = "---Select---";
            cb_Designation.Checked = false;
            ds = da.loaddesignation(college_code);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Designation.DataSource = ds;
                cbl_Designation.DataTextField = "desig_name";
                cbl_Designation.DataValueField = "Desig_Code";
                cbl_Designation.DataBind();

                for (int i = 0; i < cbl_Designation.Items.Count; i++)
                {
                    cbl_Designation.Items[i].Selected = true;
                }
                txt_designation.Text = "Designation(" + cbl_Designation.Items.Count + ")";
                cb_Designation.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void chk_deptall_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            //chkls_dept.Items.Clear();
            txt_dept.Text = "---Select---";
            if (chk_deptall.Checked == true)
            {
                for (int item = 0; item < chkls_dept.Items.Count; item++)
                {
                    chkls_dept.Items[item].Selected = true;
                }
                txt_dept.Text = "Department (" + chkls_dept.Items.Count + ")";
            }
            else
            {
                for (int item = 0; item < chkls_dept.Items.Count; item++)
                {
                    chkls_dept.Items[item].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void chkls_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int commcount = 0;
            txt_dept.Text = "--Select--";
            //chkls_dept.Items.Clear();
            for (int i = 0; i < chkls_dept.Items.Count; i++)
            {
                if (chkls_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txt_dept.Text = "Department (" + commcount.ToString() + ")";
                }
                chk_deptall.Checked = false;
            }
            if (commcount == 0)
            {
                txt_dept.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();

            ddltomonth.Items.Clear();

         //   string str = "select PayMonth,PayMonthNum,From_Date from HrPayMonths where College_Code='" + college_code + "'";
            string str = "select distinct PayMonth,PayMonthNum from HrPayMonths where College_Code='" + college_code + "' order by PayMonthNum asc ";
            ds = da.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //string mon = ds.Tables[0].Rows[i]["PayMonth"].ToString();
                    //if (ddlmonth.SelectedItem.Text.ToString() == mon)
                    //{
                    //    string date = Convert.ToString(ddlmonth.SelectedItem.Value);
                    //    for (int j = i; j < ds.Tables[0].Rows.Count; j++)
                    //    {
                    //        ddltomonth.Items.Insert(count, new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[j]["PayMonth"].ToString(), ds.Tables[0].Rows[j]["PayMonthNum"].ToString()));
                    //        count++;
                    //    }
                    //   // year(date);//delsi1403
                    //}
                    ddltomonth.DataSource = ds;
                    ddltomonth.DataTextField = "PayMonth";
                    ddltomonth.DataValueField = "PayMonthNum";
                    ddltomonth.DataBind();
                    ddltomonth.Items.Insert(0, "---Select---");
                   
                }
                ddltomonth.Items.Insert(0, "---Select---");
            }
            //ddltomonth.Items.Clear();
            //ddltomonth.Items.Insert(0, "---Select---");
            //int selvalue = Convert.ToInt32(ddlmonth.SelectedIndex.ToString());
            //int itempos = 1;
            //for (int i = selvalue; i < ddlmonth.Items.Count; i++)
            //{
            //    ddltomonth.Items.Insert(itempos, ddlmonth.Items[i].Text.ToString());
            //    ddltomonth.Items[itempos].Value = ddlmonth.Items[i].Value.ToString();
            //    itempos++;
            //}
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void ddltomonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string frm = ddlmonth.SelectedItem.Value;
            string tom = ddltomonth.SelectedItem.Value;

            if (Convert.ToInt32(frm) > Convert.ToInt32(tom))
            {
                if (ddlyear2.Items.Count > 2)
                {
                    ddlyear2.SelectedIndex = 1;
                   // ddlyear2.Enabled = false;
                    ddlyear2.Enabled = true;
                }
                else
                {
                    ddlyear2.SelectedIndex = 1;
                   // ddlyear2.Enabled = false;
                    ddlyear2.Enabled = true;
                }
            }
            else if (Convert.ToInt32(frm) == Convert.ToInt32(tom))
            {
                if (ddlyear.SelectedIndex == 0)
                {
                    ddlyear2.SelectedIndex = 0;
                   // ddlyear2.Enabled = false;
                    ddlyear2.Enabled = true;
                }
                else
                {
                    ddlyear2.SelectedIndex = 0;
                    //ddlyear2.Enabled = false;
                    ddlyear2.Enabled = false;
                }
            }
            //else delsi 1909
            //{
            //    ddlyear2.SelectedIndex = 0;
            //    ddlyear2.Enabled = false;
            //}

            //string frm = ddlmonth.SelectedItem.Value;
            //string tom = ddltomonth.SelectedItem.Value;
            //string fryr = ddlyear.SelectedItem.Value;
            //string toyr = ddlyear2.SelectedItem.Value;

            //if (Convert.ToInt32(frm) > Convert.ToInt32(tom) && Convert.ToInt32(fryr) != Convert.ToInt32(toyr))
            //{
            //    if (ddlyear2.Items.Count > 2)
            //    {
            //        ddlyear2.SelectedIndex = 1;
            //        ddlyear2.Enabled = false;
            //        lblerrormsg.Visible = false;
            //    }
            //    else
            //    {
            //        ddlyear2.SelectedIndex = 1;
            //        ddlyear2.Enabled = false;
            //        lblerrormsg.Visible = false;
            //    }
            //}
            //else if (Convert.ToInt32(frm) == Convert.ToInt32(tom))
            //{
            //    if (ddlyear.SelectedIndex == 0)
            //    {
            //        ddlyear2.SelectedIndex = 0;
            //        ddlyear2.Enabled = false;
            //        lblerrormsg.Visible = false;
            //    }
            //    else
            //    {
            //        ddlyear2.SelectedIndex = 0;
            //        ddlyear2.Enabled = false;
            //        lblerrormsg.Visible = false;
            //    }
            //}
            //else
            //{
            //    ddlyear2.SelectedIndex = 0;
            //    ddlyear2.Enabled = false;
            //    lblerrormsg.Visible = false;
            //}
        }
        catch (Exception ex)
        {
            //lblerrormsg.Visible = true;
            //lblerrormsg.Text = ex.ToString();
        }
    }

    protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();

            ddltomonth.SelectedIndex = 0;
            ddlyear2.Items.Clear();

            string str = "select distinct year(From_Date) as year from HrPayMonths where College_Code='" + college_code + "'  order by year asc";
            ds = da.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var mon = ds.Tables[0].Rows[i]["year"].ToString();
                    if (ddlyear.SelectedItem.Text.ToString() == mon)
                    {
                        for (int j = i; j < ds.Tables[0].Rows.Count; j++)
                        {
                            ddlyear2.Items.Add(ds.Tables[0].Rows[j]["year"].ToString());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void cb_Designation_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            //cbl_Designation.Items.Clear();
            if (cb_Designation.Checked == true)
            {
                for (int i = 0; i < cbl_Designation.Items.Count; i++)
                {
                    cbl_Designation.Items[i].Selected = true;
                    txt_designation.Text = "Designation(" + (cbl_Designation.Items.Count) + ")";
                }
                panel_Designation.Focus();
            }
            else
            {
                for (int i = 0; i < cbl_Designation.Items.Count; i++)
                {
                    cbl_Designation.Items[i].Selected = false;
                    txt_designation.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void cbl_Designation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            //cbl_Designation.Items.Clear();
            panel_Designation.Focus();
            int desigcount = 0;
            for (int i = 0; i < cbl_Designation.Items.Count; i++)
            {
                if (cbl_Designation.Items[i].Selected == true)
                {
                    desigcount = desigcount + 1;
                    txt_designation.Text = "Designation(" + desigcount.ToString() + ")";
                }
                cb_Designation.Checked = false;
            }

            if (desigcount == 0)
            {
                txt_designation.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void go_Click(object sender, EventArgs e)
    {
        try
        {
            clear();

            DataTable dt = new DataTable();
            DataRow dr = null;
            ArrayList arr = new ArrayList();

            string main_desgvalue = "";

            if (cbl_Designation.Items.Count > 0)
            {
                for (int desg = 0; desg < cbl_Designation.Items.Count; desg++)
                {
                    if (cbl_Designation.Items[desg].Selected == true)
                    {
                        string subvlaue = cbl_Designation.Items[desg].Value;
                        if (main_desgvalue.Trim() == "")
                        {
                            main_desgvalue = subvlaue;
                        }
                        else
                        {
                            main_desgvalue = main_desgvalue + "'" + "," + "'" + subvlaue;
                        }
                    }
                }
            }

            string main_catevalue = "";

            if (cblcategory.Items.Count > 0)
            {
                for (int desg = 0; desg < cblcategory.Items.Count; desg++)
                {
                    if (cblcategory.Items[desg].Selected == true)
                    {
                        string subvlaue = cblcategory.Items[desg].Value;
                        if (main_catevalue.Trim() == "")
                        {
                            main_catevalue = subvlaue;
                        }
                        else
                        {
                            main_catevalue = main_catevalue + "'" + "," + "'" + subvlaue;
                        }
                    }
                }
            }
            DataSet ds1 = new DataSet();
            ArrayList addarray = new ArrayList();
            Hashtable ht = new Hashtable();

            dt.Columns.Add("S.No", typeof(string));
            dt.Columns.Add("Staff Code", typeof(string));
            dt.Columns.Add("Staff Name", typeof(string));
            dt.Columns.Add("Designation", typeof(string));

            int f_month = Convert.ToInt32(ddlmonth.SelectedItem.Value);
            int t_month = Convert.ToInt32(ddltomonth.SelectedItem.Value);
            int frmyr = Convert.ToInt32(ddlyear.SelectedItem.Text);
            int toyr = Convert.ToInt32(ddlyear2.SelectedItem.Text);
            string relivddate_check = "";
            int check = 0;
            int cnt = 0;

            if (f_month <= t_month && frmyr == toyr)
            {
                string query_Value = "select CONVERT(varchar(20), From_Date,101) as From_Date,CONVERT(varchar(20), To_Date,101) as To_Date,(left (PayMonth,3)+' '+ CONVERT(varchar(10),year(To_date))) as PayMonth from HrPayMonths where (PayMonthNum >= " + f_month + " and year(to_date) = " + frmyr + ") and (PayMonthNum <= " + t_month + " and year(to_date) =  " + toyr + ") and College_Code=" + Session["collegecode"].ToString() + "";

                ds.Clear();
                ds = da.select_method_wo_parameter(query_Value, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int col = 0; col < ds.Tables[0].Rows.Count; col++)
                    {
                        check++;
                        dt.Columns.Add(ds.Tables[0].Rows[col]["paymonth"].ToString());

                        addarray.Add(Convert.ToString(ds.Tables[0].Rows[col]["From_Date"]) + "," + Convert.ToString(ds.Tables[0].Rows[col]["To_Date"]));
                        if (check == ds.Tables[0].Rows.Count)
                        {
                            relivddate_check = Convert.ToString(ds.Tables[0].Rows[col]["From_Date"]);
                        }
                    }
                }
            }
            else if (f_month <= t_month && frmyr != toyr || f_month >= t_month && frmyr != toyr)
            {
                string query_Value = "select CONVERT(varchar(20), From_Date,101) as From_Date,CONVERT(varchar(20), To_Date,101) as To_Date,(left (PayMonth,3)+' '+ CONVERT(varchar(10),year(To_Date))) as PayMonth from HrPayMonths where PayMonthNum >= " + f_month + " and year(To_Date) = " + frmyr + " union all select CONVERT(varchar(20), From_Date,101) as From_Date,CONVERT(varchar(20), To_Date,101) as To_Date,(left (PayMonth,3)+' '+ CONVERT(varchar(10),year(To_Date))) as PayMonth from HrPayMonths where PayMonthNum <=" + t_month + " and year(To_Date) = " + toyr + " and College_Code =" + Session["collegecode"].ToString() + "";
                ds.Clear();
                ds = da.select_method_wo_parameter(query_Value, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int col = 0; col < ds.Tables[0].Rows.Count; col++)
                    {
                        cnt++;
                        if (!ht.ContainsKey(ds.Tables[0].Rows[col]["paymonth"].ToString()))
                        {
                            dt.Columns.Add(ds.Tables[0].Rows[col]["paymonth"].ToString());
                            ht.Add(ds.Tables[0].Rows[col]["paymonth"].ToString(), cnt);
                            check++;
                            addarray.Add(Convert.ToString(ds.Tables[0].Rows[col]["From_Date"]) + "," + Convert.ToString(ds.Tables[0].Rows[col]["To_Date"]));
                        }
                        if (check == ds.Tables[0].Rows.Count)
                        {
                            relivddate_check = Convert.ToString(ds.Tables[0].Rows[col]["From_Date"]);
                        }
                    }
                }
            }
            else if (f_month >= t_month && frmyr == toyr)
            {
                lblerrormsg.Visible = true;
                lblerrormsg.Text = "Please Choose Correct HR-Year";
                fpspread.Visible = false;
                lblerrorxl.Visible = false;
                lblexportxl.Visible = false;
                txtexcell.Visible = false;
                butexcel.Visible = false;
                butpdf.Visible = false;
                Printcontrol.Visible = false;
                clear();
            }//Gross Salary Report
            if (report.Text == "Gross Salary Report")
            {
                dt.Columns.Add("Grand Total", typeof(string));
            }

            string main_deptvalue = "";
            int sno = 1;
            ArrayList add = new ArrayList();
            string title = "";
            string dummytitle = "";

            if (f_month <= t_month && frmyr != toyr || f_month >= t_month && frmyr != toyr || f_month <= t_month && frmyr == toyr)
            {
                if (chkls_dept.Items.Count > 0)
                {
                    for (int desg = 0; desg < chkls_dept.Items.Count; desg++)
                    {
                        if (chkls_dept.Items[desg].Selected == true)
                        {
                            string subvlaue = chkls_dept.Items[desg].Value;
                            //if (main_deptvalue.Trim() == "")
                            //{
                            main_deptvalue = subvlaue;
                            //}
                            //else
                            //{
                            //    main_deptvalue = main_deptvalue + "'" + "," + "'" + subvlaue;
                            //}
                            //        }
                            //    }
                            //}

                            string selectquery = "select  s.staff_code,s.staff_name,h.dept_code,h.dept_name,d.desig_name,d.desig_code from  staffmaster s,hrdept_master h,stafftrans t,desig_master d  where s.staff_code = t.staff_code and t.dept_code = h.dept_code   and t.desig_code = d.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code in ('" + main_deptvalue + "') and d.desig_code in ('" + main_desgvalue + "') and t.category_code in ('" + main_catevalue + "') and t.latestrec=1 and s.college_code=" + Session["collegecode"].ToString() + " and resign =0 and settled =0";
                            selectquery = selectquery + " union (select  s.staff_code,s.staff_name,h.dept_code,h.dept_name,d.desig_name,d.desig_code from  staffmaster s,hrdept_master h,stafftrans t,desig_master d  where s.staff_code = t.staff_code and t.dept_code = h.dept_code   and t.desig_code = d.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code in ('" + main_deptvalue + "') and d.desig_code in ('" + main_desgvalue + "') and t.category_code in ('" + main_catevalue + "') and t.latestrec=1 and s.college_code=" + Session["collegecode"].ToString() + " and relieve_date >='" + relivddate_check + "')";

                            ds.Clear();
                            ds = da.select_method_wo_parameter(selectquery, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                butpdf.Visible = true;
                                butexcel.Visible = true;

                                string report_name = report.SelectedItem.Text;

                                if (report_name == "Gross Salary Report")
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        dummytitle = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);

                                        if (dummytitle != title)
                                        {
                                            title = dummytitle;
                                            DataRow dr11 = null;
                                            dr11 = dt.NewRow();
                                            dr11[0] = dummytitle;
                                            dt.Rows.Add(dr11);
                                            add.Add(dt.Rows.Count);
                                        }

                                        dr = dt.NewRow();
                                        dr[0] = sno;
                                        sno++;
                                        dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                                        dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);

                                        string staff_Code = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);

                                        if (addarray.Count > 0)
                                        {
                                            double overalltot = 0;
                                            int count = 3;
                                            int overallcount = 0;//delsi04042018
                                            overallcount = (count + addarray.Count)+1;//delsi04042018
                                            for (int row = 0; row < addarray.Count; row++)
                                            {
                                               
                                                count++;
                                                string date = Convert.ToString(addarray[row]);
                                                string[] split = date.Split(',');
                                                if (split.Length > 0)
                                                {
                                                    string firstdate = Convert.ToString(split[0]);
                                                    string seconddate = Convert.ToString(split[1]);

                                                    string queryvalue = "select SUM(netaddact) as NetAddAct  from monthlypay where staff_code='" + staff_Code + "' and fdate='" + firstdate + "' and tdate ='" + seconddate + "' and college_code=" + Session["collegecode"].ToString() + "";
                                                    ds1.Clear();
                                                    ds1 = da.select_method_wo_parameter(queryvalue, "text");
                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        string salary = Convert.ToString(ds1.Tables[0].Rows[0]["NetAddAct"]);
                                                        if (salary.Trim() != "")
                                                        {
                                                            dr[count] = Convert.ToString(Math.Round(Convert.ToDouble(salary)));
                                                            overalltot = overalltot + Convert.ToDouble(salary);
                                                        }
                                                        else
                                                        {
                                                            dr[count] = "";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dr[count] = "";
                                                    }

                                                }
                                            }
                                            dr[overallcount] = Convert.ToString(Math.Round(Convert.ToDouble(overalltot)));
                                        }

                                        dt.Rows.Add(dr);

                                    }
                                    sno = 1;
                                }

                                else if (report_name == "Income Tax Report")
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        dummytitle = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);

                                        if (dummytitle != title)
                                        {
                                            title = dummytitle;
                                            DataRow dr11 = null;
                                            dr11 = dt.NewRow();
                                            dr11[0] = dummytitle;
                                            dt.Rows.Add(dr11);
                                            add.Add(dt.Rows.Count);
                                        }

                                        dr = dt.NewRow();
                                        dr[0] = sno;
                                        sno++;
                                        dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                                        dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);

                                        string staff_Code = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);

                                        if (addarray.Count > 0)
                                        {
                                            int count = 3;
                                            for (int row = 0; row < addarray.Count; row++)
                                            {
                                                count++;
                                                string date = Convert.ToString(addarray[row]);
                                                string[] split = date.Split(',');

                                                if (split.Length > 0)
                                                {
                                                    string firstdate = Convert.ToString(split[0]);
                                                    string seconddate = Convert.ToString(split[1]);

                                                    string queryvalue = "select netadd,NetAddAct,pf,netsal,deductions from monthlypay where staff_code='" + staff_Code + "' and fdate='" + firstdate + "' and tdate ='" + seconddate + "' and college_code=" + Session["collegecode"].ToString() + "";

                                                    ds1.Clear();
                                                    ds1 = da.select_method_wo_parameter(queryvalue, "text");
                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        butpdf.Visible = true;
                                                        butexcel.Visible = true;
                                                        string salary = Convert.ToString(ds1.Tables[0].Rows[0]["deductions"]);
                                                        string[] splitsalary = salary.Split('\\');
                                                        if (splitsalary.Length > 0)
                                                        {
                                                            for (int val = 0; val <= splitsalary.GetUpperBound(0); val++)
                                                            {
                                                                string secondval = Convert.ToString(splitsalary[val]);
                                                                if (secondval.Trim() != "")
                                                                {
                                                                    string[] secondval_split = secondval.Split(';');
                                                                    if (secondval_split.Length > 0)
                                                                    {
                                                                        string mainvalue = Convert.ToString(secondval_split[0]);
                                                                        if (mainvalue == "IT")
                                                                        {
                                                                            dr[count] = Convert.ToString(Math.Round(Convert.ToDouble(secondval_split[3])));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    sno = 1;
                                }

                                else if (report_name == "Pf Report")
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        dummytitle = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);

                                        if (dummytitle != title)
                                        {
                                            title = dummytitle;
                                            DataRow dr11 = null;
                                            dr11 = dt.NewRow();
                                            dr11[0] = dummytitle;
                                            dt.Rows.Add(dr11);
                                            add.Add(dt.Rows.Count);
                                        }

                                        dr = dt.NewRow();
                                        dr[0] = sno;
                                        sno++;
                                        dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                                        dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);

                                        string staff_Code = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);

                                        if (addarray.Count > 0)
                                        {
                                            int count = 3;
                                            for (int row = 0; row < addarray.Count; row++)
                                            {
                                                count++;
                                                string date = Convert.ToString(addarray[row]);
                                                string[] split = date.Split(',');
                                                if (split.Length > 0)
                                                {
                                                    string firstdate = Convert.ToString(split[0]);
                                                    string seconddate = Convert.ToString(split[1]);

                                                    string queryvalue = "select netadd,NetAddAct,pf,netsal  from monthlypay where staff_code='" + staff_Code + "' and fdate='" + firstdate + "' and tdate ='" + seconddate + "' and college_code=" + Session["collegecode"].ToString() + "";
                                                    ds1.Clear();
                                                    ds1 = da.select_method_wo_parameter(queryvalue, "text");
                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        butpdf.Visible = true;
                                                        butexcel.Visible = true;
                                                        string salary = Convert.ToString(ds1.Tables[0].Rows[0]["pf"]);
                                                        dr[count] = Convert.ToString(Math.Round(Convert.ToDouble(salary)));
                                                    }
                                                }
                                            }
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    sno = 1;
                                }

                                else if (report_name == "Education Deduction Report")
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        dummytitle = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);

                                        if (dummytitle != title)
                                        {
                                            title = dummytitle;
                                            DataRow dr11 = null;
                                            dr11 = dt.NewRow();
                                            dr11[0] = dummytitle;
                                            dt.Rows.Add(dr11);
                                            add.Add(dt.Rows.Count);
                                        }

                                        dr = dt.NewRow();
                                        dr[0] = sno;
                                        sno++;
                                        dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                                        dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);

                                        string staff_Code = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);

                                        if (addarray.Count > 0)
                                        {
                                            int count = 3;
                                            for (int row = 0; row < addarray.Count; row++)
                                            {
                                                count++;
                                                string date = Convert.ToString(addarray[row]);
                                                string[] split = date.Split(',');

                                                if (split.Length > 0)
                                                {
                                                    string firstdate = Convert.ToString(split[0]);
                                                    string seconddate = Convert.ToString(split[1]);

                                                    string queryvalue = "select netadd,NetAddAct,pf,netsal,deductions from monthlypay where staff_code='" + staff_Code + "' and fdate='" + firstdate + "' and tdate ='" + seconddate + "' and college_code=" + Session["collegecode"].ToString() + "";
                                                    ds1.Clear();
                                                    ds1 = da.select_method_wo_parameter(queryvalue, "text");
                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        butpdf.Visible = true;
                                                        butexcel.Visible = true;
                                                        string salary = Convert.ToString(ds1.Tables[0].Rows[0]["deductions"]);
                                                        string[] splitsalary = salary.Split('\\');
                                                        if (splitsalary.Length > 0)
                                                        {
                                                            for (int val = 0; val <= splitsalary.GetUpperBound(0); val++)
                                                            {
                                                                string secondval = Convert.ToString(splitsalary[val]);
                                                                if (secondval.Trim() != "")
                                                                {
                                                                    string[] secondval_split = secondval.Split(';');
                                                                    if (secondval_split.Length > 0)
                                                                    {
                                                                        string mainvalue = Convert.ToString(secondval_split[0]);
                                                                        if (mainvalue.Trim() == "EDU")
                                                                        {
                                                                            dr[count] = Convert.ToString(Math.Round(Convert.ToDouble(secondval_split[3])));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    sno = 1;
                                }

                                else if (report_name == "Other Deduction Report")
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        dummytitle = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);

                                        if (dummytitle != title)
                                        {
                                            title = dummytitle;
                                            DataRow dr11 = null;
                                            dr11 = dt.NewRow();
                                            dr11[0] = dummytitle;
                                            dt.Rows.Add(dr11);
                                            add.Add(dt.Rows.Count);
                                        }

                                        dr = dt.NewRow();
                                        dr[0] = sno;
                                        sno++;
                                        dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                                        dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);

                                        string staff_Code = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);

                                        if (addarray.Count > 0)
                                        {
                                            int count = 3;
                                            for (int row = 0; row < addarray.Count; row++)
                                            {
                                                count++;
                                                string date = Convert.ToString(addarray[row]);
                                                string[] split = date.Split(',');

                                                if (split.Length > 0)
                                                {
                                                    string firstdate = Convert.ToString(split[0]);
                                                    string seconddate = Convert.ToString(split[1]);

                                                    string queryvalue = "select netadd,NetAddAct,pf,netsal,deductions from monthlypay where staff_code='" + staff_Code + "' and fdate='" + firstdate + "' and tdate ='" + seconddate + "' and college_code=" + Session["collegecode"].ToString() + "";
                                                    ds1.Clear();
                                                    ds1 = da.select_method_wo_parameter(queryvalue, "text");
                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        butpdf.Visible = true;
                                                        butexcel.Visible = true;
                                                        string salary = Convert.ToString(ds1.Tables[0].Rows[0]["deductions"]);
                                                        string[] splitsalary = salary.Split('\\');
                                                        if (splitsalary.Length > 0)
                                                        {
                                                            for (int val = 0; val <= splitsalary.GetUpperBound(0); val++)
                                                            {
                                                                string secondval = Convert.ToString(splitsalary[val]);
                                                                if (secondval.Trim() != "")
                                                                {
                                                                    string[] secondval_split = secondval.Split(';');
                                                                    if (secondval_split.Length > 0)
                                                                    {
                                                                        string mainvalue = Convert.ToString(secondval_split[0]);
                                                                        if (mainvalue.Trim() == "OTHERS")
                                                                        {
                                                                            dr[count] = Convert.ToString(Math.Round(Convert.ToDouble(secondval_split[3])));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    sno = 1;
                                }

                                else if (report_name == "Hostel Deduction Report")
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        dummytitle = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);

                                        if (dummytitle != title)
                                        {
                                            title = dummytitle;
                                            DataRow dr11 = null;
                                            dr11 = dt.NewRow();
                                            dr11[0] = dummytitle;
                                            dt.Rows.Add(dr11);
                                            add.Add(dt.Rows.Count);
                                        }

                                        dr = dt.NewRow();
                                        dr[0] = sno;
                                        sno++;
                                        dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                                        dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);

                                        string staff_Code = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);

                                        if (addarray.Count > 0)
                                        {
                                            int count = 3;
                                            for (int row = 0; row < addarray.Count; row++)
                                            {
                                                count++;
                                                string date = Convert.ToString(addarray[row]);
                                                string[] split = date.Split(',');

                                                if (split.Length > 0)
                                                {
                                                    string firstdate = Convert.ToString(split[0]);
                                                    string seconddate = Convert.ToString(split[1]);

                                                    string queryvalue = "select netadd,NetAddAct,pf,netsal,deductions from monthlypay where staff_code='" + staff_Code + "' and fdate='" + firstdate + "' and tdate ='" + seconddate + "' and college_code=" + Session["collegecode"].ToString() + "";
                                                    ds1.Clear();
                                                    ds1 = da.select_method_wo_parameter(queryvalue, "text");
                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        butpdf.Visible = true;
                                                        butexcel.Visible = true;
                                                        string salary = Convert.ToString(ds1.Tables[0].Rows[0]["deductions"]);
                                                        string[] splitsalary = salary.Split('\\');
                                                        if (splitsalary.Length > 0)
                                                        {
                                                            for (int val = 0; val <= splitsalary.GetUpperBound(0); val++)
                                                            {
                                                                string secondval = Convert.ToString(splitsalary[val]);
                                                                if (secondval.Trim() != "")
                                                                {
                                                                    string[] secondval_split = secondval.Split(';');
                                                                    if (secondval_split.Length > 0)
                                                                    {
                                                                        string mainvalue = Convert.ToString(secondval_split[0]);
                                                                        if (mainvalue.Trim() == "HOSTEL")
                                                                        {
                                                                            dr[count] = Convert.ToString(Math.Round(Convert.ToDouble(secondval_split[3])));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    sno = 1;
                                }

                                else if (report_name == "Net Salary Report")
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        dummytitle = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);

                                        if (dummytitle != title)
                                        {
                                            title = dummytitle;
                                            DataRow dr11 = null;
                                            dr11 = dt.NewRow();
                                            dr11[0] = dummytitle;
                                            dt.Rows.Add(dr11);
                                            add.Add(dt.Rows.Count);
                                        }

                                        dr = dt.NewRow();
                                        dr[0] = sno;
                                        sno++;
                                        dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                                        dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);

                                        string staff_Code = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);

                                        if (addarray.Count > 0)
                                        {
                                            int count = 3;
                                            for (int row = 0; row < addarray.Count; row++)
                                            {
                                                count++;
                                                string date = Convert.ToString(addarray[row]);
                                                string[] split = date.Split(',');
                                                if (split.Length > 0)
                                                {
                                                    string firstdate = Convert.ToString(split[0]);
                                                    string seconddate = Convert.ToString(split[1]);

                                                    string queryvalue = "select netadd,NetAddAct,pf,netsal  from monthlypay where staff_code='" + staff_Code + "' and fdate='" + firstdate + "' and tdate ='" + seconddate + "' and college_code=" + Session["collegecode"].ToString() + "";
                                                    ds1.Clear();
                                                    ds1 = da.select_method_wo_parameter(queryvalue, "text");
                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        butpdf.Visible = true;
                                                        butexcel.Visible = true;
                                                        string salary = Convert.ToString(ds1.Tables[0].Rows[0]["netsal"]);
                                                        dr[count] = Convert.ToString(Math.Round(Convert.ToDouble(salary)));
                                                    }
                                                }
                                            }
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    sno = 1;
                                }
                            }
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>() select dc.ColumnName).ToArray();

                    fpspread.Sheets[0].ColumnHeader.RowCount = 3;
                    fpspread.Sheets[0].RowCount = 0;
                    fpspread.Height = 500;
                    fpspread.CommandBar.Visible = false;
                    fpspread.Sheets[0].SheetCorner.ColumnCount = 0;
                    fpspread.Sheets[0].RowHeader.Visible = false;

                    fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                    fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    fpspread.Sheets[0].DefaultStyle.Font.Bold = false;

                    FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                    style2.Font.Size = 13;
                    style2.Font.Name = "Book Antiqua";
                    style2.Font.Bold = true;
                    style2.HorizontalAlign = HorizontalAlign.Center;
                    style2.ForeColor = System.Drawing.Color.Black;
                    style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    fpspread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                    fpspread.Sheets[0].AutoPostBack = true;

                    fpspread.Sheets[0].ColumnCount = columnNames.GetUpperBound(0) + 1;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Financial Year Report: " + ddlyear.SelectedItem.Text + " - " + ddlyear2.SelectedItem.Text;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].ColumnHeader.Cells[1, 0].Text = report.SelectedItem.Text;
                    fpspread.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, columnNames.GetUpperBound(0) + 1);
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, columnNames.GetUpperBound(0) + 1);

                    for (int i = 0; i <= columnNames.GetUpperBound(0); i++)
                    {
                        fpspread.Sheets[0].ColumnHeader.Cells[2, i].Text = columnNames[i].ToString();
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        fpspread.Sheets[0].Rows.Count++;
                        for (int j = 0; j < fpspread.Sheets[0].ColumnCount; j++)
                        {

                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].Rows.Count - 1, j].Text = dt.Rows[i][j].ToString();
                            fpspread.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                            fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                            fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                            fpspread.Sheets[0].Columns[1].Width = 70;
                            fpspread.Sheets[0].Columns[2].Width = 150;
                            fpspread.Sheets[0].Columns[3].Width = 150;

                            if (j > 3)
                            {
                                fpspread.Sheets[0].Columns[j].HorizontalAlign = HorizontalAlign.Right;
                                fpspread.Sheets[0].Columns[j].Width = 80;
                            }
                        }
                    }

                    if (add.Count > 0)
                    {
                        for (int a = 0; a < add.Count; a++)
                        {
                            string row = Convert.ToString(add[a]);
                            int row1 = 0;
                            row1 = Convert.ToInt32(row) - 1;

                            for (int mr = 1; mr <= fpspread.Sheets[0].ColumnCount; mr++)
                            {
                                if (mr >= 2)
                                {
                                    fpspread.Sheets[0].SpanModel.Add(row1, 0, 1, mr);
                                    fpspread.Sheets[0].Cells[row1, 0].ForeColor = System.Drawing.Color.Brown;
                                    fpspread.Sheets[0].Cells[row1, 0].BackColor = System.Drawing.Color.Gainsboro;
                                }
                            }
                        }
                    }
                    else
                    {
                        lblerrormsg.Visible = true;
                        lblerrormsg.Text = "No Records Found";
                    }

                    fpspread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    fpspread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                    fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                    fpspread.SaveChanges();
                    fpspread.Visible = true;
                    lblerrorxl.Visible = false;
                    lblexportxl.Visible = true;
                    txtexcell.Visible = true;
                    butexcel.Visible = true;
                    butpdf.Visible = true;
                    Printcontrol.Visible = false;
                    lblerrormsg.Visible = false;
                }
                else
                {
                    clear();

                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                }
            }
            else
            {
                lblerrormsg.Visible = true;
                lblerrormsg.Text = "Please Choose Correct HR-Year";
                fpspread.Visible = false;
                lblerrorxl.Visible = false;
                lblexportxl.Visible = false;
                txtexcell.Visible = false;
                butexcel.Visible = false;
                butpdf.Visible = false;
                Printcontrol.Visible = false;
                //clear();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void butpdf_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = ""; //"Subjectwise Mark and Grade Report" + '@' + "        " + "Batch: " + ddlbatch.SelectedItem.ToString() + "        " + "Degree: " + ddldegree.SelectedItem.ToString() + "        " + "Branch: " + ddldept.SelectedItem.ToString() + "        " + "Semester: " + ddlsem.SelectedItem.ToString() + "        " + "Subject Name: " + ddlsubject.SelectedItem.Text.ToString();
            string pagename = "HR Finance Report.aspx";
            Printcontrol.loadspreaddetails(fpspread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void butexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcell.Text;

            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(fpspread, reportname);
                lblerrorxl.Visible = false;
            }
            else
            {
                lblerrorxl.Text = "Please Enter Your Report Name";
                lblerrorxl.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    public void bindcate()
    {
        try
        {
            clear();
            ds.Clear();
            cbcategory.Visible = true;
            string col = college_code;

            if (col == "---Select---")
            {
                col = Session["college_code"].ToString();
            }
            txt_category.Text = "---Select---";
            cbcategory.Checked = false;
            ds = da.loadcategory(college_code);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblcategory.DataSource = ds;
                cblcategory.DataTextField = "category_name";
                cblcategory.DataValueField = "Category_Code";
                cblcategory.DataBind();

                for (int i = 0; i < cblcategory.Items.Count; i++)
                {
                    cblcategory.Items[i].Selected = true;

                }
                txt_category.Text = "Category(" + cblcategory.Items.Count + ")";
                cbcategory.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void cbcategory_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (cbcategory.Checked == true)
            {
                for (int i = 0; i < cblcategory.Items.Count; i++)
                {
                    cblcategory.Items[i].Selected = true;
                    txt_category.Text = "Category(" + (cblcategory.Items.Count) + ")";
                }
                panelcategory.Focus();
            }
            else
            {
                for (int i = 0; i < cblcategory.Items.Count; i++)
                {
                    cblcategory.Items[i].Selected = false;
                    txt_category.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void cblcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            panelcategory.Focus();
            int catcount = 0;
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                if (cblcategory.Items[i].Selected == true)
                {
                    catcount = catcount + 1;
                    txt_category.Text = "Category(" + catcount.ToString() + ")";
                }
                cbcategory.Checked = false;
            }
            if (catcount == 0)
            {
                txt_category.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    protected void report_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    public void clear()
    {
        fpspread.Visible = false;
        lblerrorxl.Visible = false;
        lblexportxl.Visible = false;
        txtexcell.Visible = false;
        butexcel.Visible = false;
        butpdf.Visible = false;
        Printcontrol.Visible = false;
        lblerrormsg.Visible = false;
    }
}

