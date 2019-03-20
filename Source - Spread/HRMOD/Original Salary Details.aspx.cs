using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Configuration;

public partial class Original_Salary_Details : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string collegecode = "";
    int commcount = 0;
    string usercode = "";
    Boolean flag_true = false;

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
            errmsg.Visible = false;
            lblgenerror.Visible = false;
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                loaddept();
                BindDesignation();
                Bindcategory();
                loadstaff();
                Bindallowance();
                loadfromyear();

                ddlfmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
                ddlfmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlfmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlfmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlfmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlfmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlfmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlfmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlfmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlfmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlfmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlfmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlfmonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                ddltmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
                ddltmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddltmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddltmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddltmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddltmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddltmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddltmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddltmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddltmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddltmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddltmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddltmonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                rbconsolidate.Checked = true;
                lblstaff.Visible = false;
                txtstaff.Visible = false;
                pstaff.Visible = false;

                clear();
                for (int c = 0; c < chklscolumn.Items.Count; c++)
                {
                    chklscolumn.Items[c].Selected = true;
                }
            }
            errmsg.Visible = false;
        }
        catch (Exception ex) { }
    }

    public void clear()
    {
        FpSalaryReport.Visible = false;
        FpMonthOverall.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        lblexcel.Visible = false;
        txtexcel.Visible = false;
        txtexcel.Text = "";
        btnexcel.Visible = false;
        btngenerate.Visible = false;

        lblexcel1.Visible = false;
        txtexcel1.Visible = false;
        txtexcel1.Text = "";
        btnexcel1.Visible = false;
        btnprint1.Visible = false;
        Printmaster1.Visible = false;
    }

    public void loadfromyear()
    {
        try
        {

            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("select distinct PayYear from HrPayMonths where College_Code='" + collegecode + "' order by PayYear", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlfyear.DataSource = ds;
                ddlfyear.DataTextField = "PayYear";
                ddlfyear.DataValueField = "PayYear";
                ddlfyear.DataBind();

                ddltyear.DataSource = ds;
                ddltyear.DataTextField = "PayYear";
                ddltyear.DataValueField = "PayYear";
                ddltyear.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }

    }

    protected void ddlfyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            ddltyear.Items.Clear();
            ddlfmonth.Items.Clear();
            ddltmonth.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("select distinct PayYear from HrPayMonths where College_Code='" + collegecode + "' and PayYear>='" + ddlfyear.SelectedValue.ToString() + "' order by PayYear", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltyear.DataSource = ds;
                ddltyear.DataTextField = "PayYear";
                ddltyear.DataValueField = "PayYear";
                ddltyear.DataBind();


                ddlfmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
                ddlfmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlfmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlfmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlfmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlfmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlfmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlfmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlfmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlfmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlfmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlfmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlfmonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                ddltmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
                ddltmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddltmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddltmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddltmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddltmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddltmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddltmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddltmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddltmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddltmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddltmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddltmonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void ddlfmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void ddltyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void ddltmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void loaddept()
    {
        try
        {
            commcount = 0;
            chklsdept.Items.Clear();
            chkdept.Checked = false;
            txtdept.Text = "---Select---";

            ds.Dispose();
            ds.Reset();
            ds = d2.loaddepartment(collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsdept.DataSource = ds;
                chklsdept.DataTextField = "dept_name";
                chklsdept.DataValueField = "Dept_Code";
                chklsdept.DataBind();

                for (int i = 0; i < chklsdept.Items.Count; i++)
                {
                    chklsdept.Items[i].Selected = true;
                }
                chkdept.Checked = true;
                txtdept.Text = "Dept (" + chklsdept.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkdept_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkdept.Checked == true)
            {
                for (int i = 0; i < chklsdept.Items.Count; i++)
                {
                    chklsdept.Items[i].Selected = true;
                }
                txtdept.Text = "Dept (" + chklsdept.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < chklsdept.Items.Count; i++)
                {
                    chklsdept.Items[i].Selected = false;
                }
                txtdept.Text = "--Select--";
            }
            loadstaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklsdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            commcount = 0;
            txtdept.Text = "---Select---";
            chkdept.Checked = false;
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdept.Text = "Dept (" + commcount.ToString() + ")";
                if (chklsdept.Items.Count == commcount)
                {
                    chkdept.Checked = true;
                }
            }
            loadstaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindDesignation()
    {
        try
        {
            txtdesign.Text = "---Select---";
            chkdesign.Checked = false;
            chklsdesign.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.binddesi(collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsdesign.DataSource = ds;
                chklsdesign.DataValueField = "desig_code";
                chklsdesign.DataTextField = "desig_name";
                chklsdesign.DataBind();

                for (int i = 0; i < chklsdesign.Items.Count; i++)
                {
                    chklsdesign.Items[i].Selected = true;
                }
                chkdesign.Checked = true;
                txtdesign.Text = "Design (" + chklsdesign.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkdesign_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkdesign.Checked == true)
            {
                for (int i = 0; i < chklsdesign.Items.Count; i++)
                {
                    chklsdesign.Items[i].Selected = true;
                }
                txtdesign.Text = "Design (" + chklsdesign.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < chklsdesign.Items.Count; i++)
                {
                    chklsdesign.Items[i].Selected = false;
                }
                txtdesign.Text = "--Select--";
            }
            loadstaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklsdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            commcount = 0;
            txtdesign.Text = "---Select---";
            chkdesign.Checked = false;
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                if (chklsdesign.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdesign.Text = "Design (" + commcount.ToString() + ")";
                if (chklsdept.Items.Count == commcount)
                {
                    chkdesign.Checked = true;
                }
            }
            loadstaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void Bindcategory()
    {
        try
        {
            txtcategory.Text = "---Select---";
            chkcategory.Checked = false;
            chklscategory.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("select  distinct category_code,category_name from  staffcategorizer where college_code='" + Session["collegecode"] + "' order by category_name", "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklscategory.DataSource = ds;
                chklscategory.DataValueField = "category_code";
                chklscategory.DataTextField = "category_name";
                chklscategory.DataBind();

                for (int i = 0; i < chklscategory.Items.Count; i++)
                {
                    chklscategory.Items[i].Selected = true;
                }
                chkcategory.Checked = true;
                txtcategory.Text = "Category (" + chklscategory.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkcategory_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkcategory.Checked == true)
            {
                for (int i = 0; i < chklscategory.Items.Count; i++)
                {
                    chklscategory.Items[i].Selected = true;
                }
                txtcategory.Text = "Category (" + chklscategory.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < chklscategory.Items.Count; i++)
                {
                    chklscategory.Items[i].Selected = false;
                }
                txtcategory.Text = "--Select--";
            }
            loadstaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklscategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            commcount = 0;
            txtcategory.Text = "---Select---";
            chkcategory.Checked = false;
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtcategory.Text = "Category (" + commcount.ToString() + ")";
                if (chklscategory.Items.Count == commcount)
                {
                    chkcategory.Checked = true;
                }
            }
            loadstaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void Bindallowance()
    {
        try
        {
            txtallowance.Text = "---Select---";
            chkallowance.Checked = false;
            chklsallowance.Items.Clear();

            txtdeduction.Text = "---Select---";
            chkdeduction.Checked = false;
            chklsdeduction.Items.Clear();

            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("Select * from incentives_master where college_code=" + Session["collegecode"] + "", "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string allowance = ds.Tables[0].Rows[0]["allowances"].ToString();
                string dedcution = ds.Tables[0].Rows[0]["deductions"].ToString();

                string[] allowanmce_arr = allowance.Split(';');
                for (int i = 0; i <= allowanmce_arr.GetUpperBound(0); i++)
                {
                    string[] spalwal = allowanmce_arr[i].ToString().Split('\\');
                    if (spalwal[0].ToString().Trim() != "")
                    {
                        chklsallowance.Items.Add(spalwal[0].ToString());
                    }
                }

                for (int i = 0; i < chklsallowance.Items.Count; i++)
                {
                    chklsallowance.Items[i].Selected = true;
                }
                chkallowance.Checked = true;
                txtallowance.Text = "Allowance (" + chklsallowance.Items.Count + ")";

                allowanmce_arr = dedcution.Split(';');
                for (int i = 0; i <= allowanmce_arr.GetUpperBound(0); i++)
                {
                    string[] spalwal = allowanmce_arr[i].ToString().Split('\\');
                    if (spalwal[0].ToString().Trim() != "")
                    {
                        chklsdeduction.Items.Add(spalwal[0].ToString());
                    }
                }

                for (int i = 0; i < chklsdeduction.Items.Count; i++)
                {
                    chklsdeduction.Items[i].Selected = true;
                }
                chkdeduction.Checked = true;
                txtdeduction.Text = "Deduction (" + chklsdeduction.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chlallowance_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkallowance.Checked == true)
            {
                for (int i = 0; i < chklsallowance.Items.Count; i++)
                {
                    chklsallowance.Items[i].Selected = true;
                }
                txtallowance.Text = "Allowance (" + chklsallowance.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < chklsallowance.Items.Count; i++)
                {
                    chklsallowance.Items[i].Selected = false;
                }
                txtallowance.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkallowance_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            commcount = 0;
            txtallowance.Text = "---Select---";
            chkallowance.Checked = false;
            for (int i = 0; i < chklsallowance.Items.Count; i++)
            {
                if (chklsallowance.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtallowance.Text = "Allowance (" + commcount.ToString() + ")";
                if (chklsallowance.Items.Count == commcount)
                {
                    chkallowance.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkdeduction_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkdeduction.Checked == true)
            {
                for (int i = 0; i < chklsdeduction.Items.Count; i++)
                {
                    chklsdeduction.Items[i].Selected = true;
                }
                txtdeduction.Text = "Deduction (" + chklsdeduction.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < chklsdeduction.Items.Count; i++)
                {
                    chklsdeduction.Items[i].Selected = false;
                }
                txtdeduction.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklsdeduction_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            commcount = 0;
            txtdeduction.Text = "---Select---";
            chkdeduction.Checked = false;
            for (int i = 0; i < chklsdeduction.Items.Count; i++)
            {
                if (chklsdeduction.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdeduction.Text = "Deduction (" + commcount.ToString() + ")";
                if (chklsdeduction.Items.Count == commcount)
                {
                    chkdeduction.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    public void loadstaff()
    {
        //try
        //{
        //    txtstaff.Text = "---Select---";
        //    chkstaff.Checked = false;
        //    chklsstaff.Items.Clear();
        //    ds.Dispose();
        //    ds.Reset();
        //    string deptcode = "";
        //    for (int i = 0; i < chklsdept.Items.Count; i++)
        //    {
        //        if (chklsdept.Items[i].Selected == true)
        //        {
        //            if (deptcode == "")
        //            {
        //                deptcode = "'" + chklsdept.Items[i].Value.ToString() + "'";
        //            }
        //            else
        //            {
        //                deptcode = deptcode + ",'" + chklsdept.Items[i].Value.ToString() + "'";
        //            }
        //        }
        //    }
        //    if (deptcode.Trim() != "")
        //    {
        //        deptcode = " and st.dept_code in (" + deptcode + ")";
        //    }

        //    string design = "";
        //    for (int i = 0; i < chklsdesign.Items.Count; i++)
        //    {
        //        if (chklsdesign.Items[i].Selected == true)
        //        {
        //            if (design == "")
        //            {
        //                design = "'" + chklsdesign.Items[i].Value.ToString() + "'";
        //            }
        //            else
        //            {
        //                design = design + ",'" + chklsdesign.Items[i].Value.ToString() + "'";
        //            }
        //        }
        //    }
        //    if (design.Trim() != "")
        //    {
        //        design = " and st.desig_code in (" + design + ")";
        //    }

        //    string cateory = "";
        //    for (int i = 0; i < chklscategory.Items.Count; i++)
        //    {
        //        if (chklscategory.Items[i].Selected == true)
        //        {
        //            if (cateory == "")
        //            {
        //                cateory = "'" + chklscategory.Items[i].Value.ToString() + "'";
        //            }
        //            else
        //            {
        //                cateory = cateory + ",'" + chklscategory.Items[i].Value.ToString() + "'";
        //            }
        //        }
        //    }
        //    if (cateory.Trim() != "")
        //    {
        //        cateory = " and st.category_code in (" + cateory + ") ";
        //    }
        //    if (deptcode.Trim() != "" && design.Trim() != "" && cateory.Trim() != "")
        //    {
        //        ds = d2.select_method_wo_parameter("select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode " + deptcode + " " + design + " " + cateory + " and s.college_code='" + collegecode + "' and resign = 0 and settled = 0 and latestrec=1 order by s.staff_name", "text");
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            chklsstaff.DataSource = ds;
        //            chklsstaff.DataValueField = "staff_code";
        //            chklsstaff.DataTextField = "staff_name";
        //            chklsstaff.DataBind();

        //            for (int i = 0; i < chklsstaff.Items.Count; i++)
        //            {
        //                chklsstaff.Items[i].Selected = true;
        //            }
        //            chkstaff.Checked = true;
        //            txtstaff.Text = "Staff (" + chklsstaff.Items.Count + ")";
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    errmsg.Text = ex.ToString();
        //    errmsg.Visible = true;
        //}
    }
    protected void chkstaff_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkstaff.Checked == true)
            {
                for (int i = 0; i < chklsstaff.Items.Count; i++)
                {
                    chklsstaff.Items[i].Selected = true;
                }
                txtstaff.Text = "Staff (" + chklsstaff.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < chklsstaff.Items.Count; i++)
                {
                    chklsstaff.Items[i].Selected = false;
                }
                txtstaff.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklsstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            commcount = 0;
            txtstaff.Text = "---Select---";
            chkstaff.Checked = false;
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                if (chklsstaff.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtstaff.Text = "Staff (" + commcount.ToString() + ")";
                if (chklsstaff.Items.Count == commcount)
                {
                    chkstaff.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (rbconsolidate.Checked == true)
            {
                consolidatereport();
            }
            else
            {
                staffwisereport();
            }
            FpMonthOverall.Sheets[0].PageSize = FpMonthOverall.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void consolidatereport()
    {
        try
        {
            string fromyear = ddlfyear.SelectedValue.ToString();
            string frommonth = ddlfmonth.SelectedValue.ToString();
            string toyear = ddltyear.SelectedValue.ToString();
            string tomonth = ddltmonth.SelectedValue.ToString();

            if (frommonth.Trim() == "0")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The From Month And Then Proceed";
                return;
            }

            if (tomonth.Trim() == "0")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The TO Month And Then Proceed";
                return;
            }

            int fromyearval = (Convert.ToInt32(fromyear) * 12) + Convert.ToInt32(frommonth);
            int toyearval = (Convert.ToInt32(toyear) * 12) + Convert.ToInt32(tomonth);
            if (fromyearval > toyearval)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The To Month And Year Must Be Equal To Greater Than From Month And Year";
                return;
            }

            string deptcode = "";
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        deptcode = deptcode + ",'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (deptcode.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Department And Then Proceed";
                return;
            }

            string design = "";
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                if (chklsdesign.Items[i].Selected == true)
                {
                    if (design == "")
                    {
                        design = "'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        design = design + ",'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (design.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Designation And Then Proceed";
                return;
            }

            string cateory = "";
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    if (cateory == "")
                    {
                        cateory = "'" + chklscategory.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        cateory = cateory + ",'" + chklscategory.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (cateory.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Category And Then Proceed";
                return;
            }

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

            Dictionary<int, Double> dicmontotal = new Dictionary<int, double>();
            Double dicgetsetval = 0;

            string strhryearquery = "select PayMonthNum,PayYear,From_Date,To_Date,year(from_date) fyear,year(to_date) tyear from HrPayMonths where College_Code='" + collegecode + "'";
            DataSet dshryear = d2.select_method_wo_parameter(strhryearquery, "Text");

            FpMonthOverall.CommandBar.Visible = false;
            FpMonthOverall.Sheets[0].SheetCorner.ColumnCount = 0;
            FpMonthOverall.Sheets[0].ColumnCount = 0;
            FpMonthOverall.Sheets[0].RowCount = 0;
            FpMonthOverall.Sheets[0].ColumnHeader.RowCount = 0;

            FpMonthOverall.Sheets[0].ColumnHeader.RowCount = 2;
            FpMonthOverall.Sheets[0].ColumnCount = 7;


            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpMonthOverall.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[0].Width = 50;
            FpMonthOverall.Sheets[0].Columns[0].Locked = true;
            FpMonthOverall.Sheets[0].Columns[0].CellType = txt;

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpMonthOverall.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[1].Width = 50;

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Year - Month";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpMonthOverall.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[2].Width = 150;
            FpMonthOverall.Sheets[0].Columns[2].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[2].Locked = true;

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total No.of Staff";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpMonthOverall.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[3].Width = 100;
            FpMonthOverall.Sheets[0].Columns[3].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[3].Locked = true;
            FpMonthOverall.Sheets[0].Columns[3].Visible = true;
            if (chklscolumn.Items[0].Selected == false)
            {
                FpMonthOverall.Sheets[0].Columns[3].Visible = false;
            }
            dicmontotal.Add(3, 0);

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Basic Pay";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpMonthOverall.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[4].Width = 100;
            FpMonthOverall.Sheets[0].Columns[4].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[4].Locked = true;
            FpMonthOverall.Sheets[0].Columns[4].Visible = true;
            if (chklscolumn.Items[1].Selected == false)
            {
                FpMonthOverall.Sheets[0].Columns[4].Visible = false;
            }
            dicmontotal.Add(4, 0);

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Grade Pay";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            FpMonthOverall.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[5].Width = 100;
            FpMonthOverall.Sheets[0].Columns[5].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[5].Locked = true;

            FpMonthOverall.Sheets[0].Columns[5].Visible = true;
            if (chklscolumn.Items[2].Selected == false)
            {
                FpMonthOverall.Sheets[0].Columns[5].Visible = false;
            }
            dicmontotal.Add(5, 0);

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Pay Band";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            FpMonthOverall.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[6].Width = 100;
            FpMonthOverall.Sheets[0].Columns[6].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[6].Locked = true;

            FpMonthOverall.Sheets[0].Columns[6].Visible = true;
            if (chklscolumn.Items[2].Selected == false)
            {
                FpMonthOverall.Sheets[0].Columns[6].Visible = false;
            }
            dicmontotal.Add(6, 0);

            int spart = 0;
            commcount = 0;
            for (int i = 0; i < chklsallowance.Items.Count; i++)
            {
                FpMonthOverall.Sheets[0].ColumnCount++;
                FpMonthOverall.Sheets[0].ColumnHeader.Cells[1, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = chklsallowance.Items[i].Text.ToString();
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;
                dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);
                if (chklsallowance.Items[i].Selected == true)
                {
                    commcount++;
                    if (spart == 0)
                    {
                        spart = FpMonthOverall.Sheets[0].ColumnCount - 1;
                    }
                }
                else
                {
                    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = false;
                }
            }
            if (commcount > 0)
            {
                FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, spart].Text = "Allowance";
                FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, spart, 1, commcount);


                FpMonthOverall.Sheets[0].ColumnCount++;
                FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = "Total Allowance";
                FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, FpMonthOverall.Sheets[0].ColumnCount - 1, 2, 1);
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;

                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = true;
                if (chklscolumn.Items[4].Selected == false)
                {
                    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = false;
                }
                dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);
            }

            FpMonthOverall.Sheets[0].ColumnCount++;
            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = "Gross Amount";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, FpMonthOverall.Sheets[0].ColumnCount - 1, 2, 1);
            FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
            FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;
            FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = true;
            if (chklscolumn.Items[5].Selected == false)
            {
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = false;
            }
            dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);

            int decstrcolu = FpMonthOverall.Sheets[0].ColumnCount;
            int setdeuctcolun = 0;
            commcount = 0;
            for (int i = 0; i < chklsdeduction.Items.Count; i++)
            {
                FpMonthOverall.Sheets[0].ColumnCount++;
                FpMonthOverall.Sheets[0].ColumnHeader.Cells[1, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = chklsdeduction.Items[i].Text.ToString();
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;
                dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);
                if (chklsdeduction.Items[i].Selected == true)
                {
                    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = true;
                    commcount++;
                    if (setdeuctcolun == 0)
                    {
                        setdeuctcolun = FpMonthOverall.Sheets[0].ColumnCount - 1;
                    }
                }
                else
                {
                    if (commcount > 0)
                    {
                        commcount++;
                    }
                    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = false;
                }
            }
            if (setdeuctcolun > 0)
            {
                FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, setdeuctcolun].Text = "Deduction";
                FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, setdeuctcolun, 1, commcount);

                FpMonthOverall.Sheets[0].ColumnCount++;
                FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = "Total Deduction";
                FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, FpMonthOverall.Sheets[0].ColumnCount - 1, 2, 1);
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;
                FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = true;
                if (chklscolumn.Items[6].Selected == false)
                {
                    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = false;
                }
                dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);
            }

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpMonthOverall.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpMonthOverall.Sheets[0].SheetName = " ";
            FpMonthOverall.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpMonthOverall.Sheets[0].DefaultStyle.Font.Size = FontUnit.Large;
            FpMonthOverall.Sheets[0].DefaultStyle.Font.Bold = false;
            FpMonthOverall.Width = 1200;
            FpMonthOverall.Visible = true;
            FpMonthOverall.Sheets[0].AutoPostBack = false;
            int srno = 0;

            for (int yemonval = fromyearval; yemonval <= toyearval; yemonval++)
            {
                srno++;
                FpMonthOverall.Sheets[0].RowCount++;
                if ((srno % 2) == 1)
                {
                    FpMonthOverall.Sheets[0].Rows[FpMonthOverall.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                }

                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 1].CellType = chk;
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                int montext = yemonval % 12;
                if (montext == 0)
                {
                    montext = 12;
                }
                string monthname = "";
                switch (montext)
                {
                    case 1:
                        monthname = "January";
                        break;
                    case 2:
                        monthname = "February";
                        break;
                    case 3:
                        monthname = "March";
                        break;
                    case 4:
                        monthname = "April";
                        break;
                    case 5:
                        monthname = "May";
                        break;
                    case 6:
                        monthname = "June";
                        break;
                    case 7:
                        monthname = "July";
                        break;
                    case 8:
                        monthname = "August";
                        break;
                    case 9:
                        monthname = "September";
                        break;
                    case 10:
                        monthname = "October";
                        break;
                    case 11:
                        monthname = "November";
                        break;
                    case 12:
                        monthname = "December";
                        break;
                }
                int yeartext = yemonval / 12;
                if (montext == 12)
                {
                    yeartext--;
                }

                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].Text = yeartext + " - " + monthname.ToString();
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].Tag = yeartext + "-" + montext;
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Large;
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                dshryear.Tables[0].DefaultView.RowFilter = "PayMonthNum='" + montext + "' and tyear='" + yeartext + "'";//fyeardelsi1403 changed to tyear
                DataView dvhryear = dshryear.Tables[0].DefaultView;
                if (dvhryear.Count > 0)
                {
                    btngenerate.Visible = true;
                    string fdate = dvhryear[0]["From_Date"].ToString();
                    string tdate = dvhryear[0]["To_Date"].ToString();

                    DateTime dtf = Convert.ToDateTime(fdate);
                    DateTime dtt = Convert.ToDateTime(tdate);


                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 3].Tag = dtf + "@" + dtt;

                    string strquery = "select h.dept_name,h.dept_code,d.desig_name,d.desig_code,st.stftype,st.category_code,sm.staff_name,sm.staff_code,m.PayYear,m.PayMonth,m.fdate,m.tdate,m.basic_alone,m.grade_pay,m.pay_band,m.allowances,m.deductions,m.NetAddAct as gross,m.netded as totaldeduction,st.IsConsolid,m.bsalary";
                    strquery = strquery + " from monthlypay m,stafftrans st,staffmaster sm,hrdept_master h,desig_master d where sm.staff_code=st.staff_code and st.staff_code=m.staff_code and sm.staff_code=m.staff_code and st.dept_code =h.dept_code and st.desig_code=d.desig_code and h.college_code=sm.college_code ";
                    strquery = strquery + " and h.dept_code in(" + deptcode + ") and d.desig_code in(" + design + ") and st.category_code in(" + cateory + ") and sm.college_code=m.college_code and sm.college_code = d.collegecode";
                    //strquery = strquery + "  and st.latestrec=1 and ((sm.resign = 0 and settled = 0) or (sm.resign = 1 and settled = 1 and relieve_date < '" + dtt.ToString("MM/dd/yyyy") + "')) and m.fdate = '" + dtf.ToString("MM/dd/yyyy") + "' order by h.dept_code,d.desig_code,sm.staff_code";
                    strquery = strquery + "  and st.latestrec=1 and ((sm.resign = 0 and settled = 0) or (sm.resign = 1 and settled = 1 and relieve_date >= '" + dtf.ToString("MM/dd/yyyy") + "' and relieve_date <= '" + dtt.ToString("MM/dd/yyyy") + "')) and m.fdate = '" + dtf.ToString("MM/dd/yyyy") + "' order by h.dept_code,d.desig_code,sm.staff_code";
                    ds.Dispose();
                    ds.Reset();
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].Tag = "Pay Process";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string staffcode = ds.Tables[0].Rows[i]["staff_code"].ToString();
                            string stftname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                            string depcode = ds.Tables[0].Rows[i]["dept_code"].ToString();
                            string depname = ds.Tables[0].Rows[i]["dept_name"].ToString();
                            string catcode = ds.Tables[0].Rows[i]["category_code"].ToString();
                            string catname = ds.Tables[0].Rows[i]["stftype"].ToString();
                            string desname = ds.Tables[0].Rows[i]["desig_name"].ToString();
                            string descode = ds.Tables[0].Rows[i]["desig_code"].ToString();
                            string basic = ds.Tables[0].Rows[i]["basic_alone"].ToString();
                            string grade = ds.Tables[0].Rows[i]["grade_pay"].ToString();
                            string payband = ds.Tables[0].Rows[i]["pay_band"].ToString();
                            string allownce = ds.Tables[0].Rows[i]["allowances"].ToString();
                            string deduction = ds.Tables[0].Rows[i]["deductions"].ToString();
                            string IsConsolid = ds.Tables[0].Rows[i]["IsConsolid"].ToString();

                            if (basic.Trim().ToLower() == "")
                            {
                                basic = "0";
                            }
                            Double bascicam = Convert.ToDouble(basic);
                            bascicam = Math.Round(bascicam, 0, MidpointRounding.AwayFromZero);

                            dicgetsetval = dicmontotal[4] + bascicam;
                            dicmontotal[4] = dicgetsetval;

                            if (grade.Trim().ToLower() == "")
                            {
                                grade = "0";
                            }
                            Double gradeam = Convert.ToDouble(grade);
                            gradeam = Math.Round(gradeam, 0, MidpointRounding.AwayFromZero);

                            dicgetsetval = dicmontotal[5] + gradeam;
                            dicmontotal[5] = dicgetsetval;

                            if (payband.Trim().ToLower() == "")
                            {
                                payband = "0";
                            }
                            Double paybandamount = Convert.ToDouble(payband);
                            paybandamount = Math.Round(paybandamount, 0, MidpointRounding.AwayFromZero);

                            dicgetsetval = dicmontotal[6] + paybandamount;
                            dicmontotal[6] = dicgetsetval;

                            Double allototal = 0;
                            string[] spval = allownce.Split('\\');
                            int c = 6;
                            Double amountvalue = 0;
                            int strcol = 6;
                            for (c = strcol; c < decstrcolu; c++)
                            {
                                strcol++;
                                string hval = FpMonthOverall.Sheets[0].ColumnHeader.Cells[1, c].Text.ToString().Trim().ToLower();
                                for (int j = 0; j <= spval.GetUpperBound(0); j++)
                                {
                                    string[] spgb = spval[j].Split(';');
                                    if (spgb.GetUpperBound(0) > 2)
                                    {
                                        if (spgb[0].ToString().Trim().ToLower() == hval)
                                        {
                                            amountvalue = 0;
                                            if (spgb[2].ToString().Trim() != "")
                                            {
                                                string binval = "";
                                                string splval = spgb[2].ToString();
                                                string[] get = splval.Split('-');
                                                if (spgb[1].Trim() == "Amount")
                                                {
                                                    binval = get[0];
                                                }
                                                else if (spgb[1].Trim() == "Percent" || spgb[1].Trim() == "Slab")
                                                {
                                                    if (get.Length == 2)
                                                    {
                                                        binval = get[1];
                                                    }
                                                }
                                                if (binval.ToString().Trim() != "")
                                                {
                                                    amountvalue = Convert.ToDouble(binval);
                                                    amountvalue = Math.Round(amountvalue, 0, MidpointRounding.AwayFromZero);
                                                }

                                                dicgetsetval = dicmontotal[c] + amountvalue;
                                                dicmontotal[c] = dicgetsetval;

                                                allototal = allototal + amountvalue;
                                                allototal = Math.Round(allototal, 0, MidpointRounding.AwayFromZero);
                                                //j = spval.GetUpperBound(0) + 1;
                                            }
                                        }
                                    }
                                }
                            }
                            strcol = decstrcolu - 2;
                            dicgetsetval = dicmontotal[strcol] + allototal;
                            dicmontotal[strcol] = dicgetsetval;

                            Double grossamount = allototal + bascicam + gradeam + paybandamount;
                            if (IsConsolid != "1")
                            {
                                grossamount = allototal + bascicam;
                            }

                            strcol++;
                            dicgetsetval = dicmontotal[strcol] + grossamount;
                            dicmontotal[strcol] = dicgetsetval;

                            Double deducttotal = 0;
                            spval = deduction.Split('\\');
                            for (c = decstrcolu; c < FpMonthOverall.Sheets[0].ColumnCount - 1; c++)
                            {
                                string hval = FpMonthOverall.Sheets[0].ColumnHeader.Cells[1, c].Text.ToString().Trim().ToLower();
                                for (int j = 0; j <= spval.GetUpperBound(0); j++)
                                {
                                    string[] spgb = spval[j].Split(';');
                                    if (spgb.GetUpperBound(0) > 2)
                                    {
                                        if (spgb[0].ToString().Trim().ToLower() == hval)
                                        {
                                            if (spgb[3].ToString().Trim() != "")
                                            {
                                                amountvalue = 0;
                                                string binval = "";
                                                string splval = spgb[2].ToString();
                                                string[] get = splval.Split('-');
                                                if (spgb[1].Trim() == "Amount")
                                                {
                                                    binval = get[0];
                                                }
                                                else if (spgb[1].Trim() == "Percent" || spgb[1].Trim() == "Slab")
                                                {
                                                    if (get.Length == 2)
                                                    {
                                                        binval = get[1];
                                                    }
                                                }
                                                if (binval.ToString().Trim() != "")
                                                {
                                                    amountvalue = Convert.ToDouble(binval);
                                                }
                                                j = spval.GetUpperBound(0) + 1;

                                                amountvalue = Math.Round(amountvalue, 0, MidpointRounding.AwayFromZero);
                                                dicgetsetval = dicmontotal[c] + amountvalue;
                                                dicmontotal[c] = dicgetsetval;

                                                deducttotal = deducttotal + amountvalue;
                                                deducttotal = Math.Round(deducttotal, 0, MidpointRounding.AwayFromZero);
                                            }
                                        }
                                    }
                                }
                            }
                            strcol = FpMonthOverall.Sheets[0].ColumnCount - 1;
                            dicgetsetval = dicmontotal[strcol] + deducttotal;
                            dicmontotal[strcol] = dicgetsetval;
                        }
                    }
                    else
                    {
                        strquery = "Select h.dept_name,h.dept_code,d.desig_name,d.desig_code,st.stftype,st.category_code,sm.staff_name,sm.staff_code,st.bsalary,st.pay_band,st.grade_pay,st.allowances,st.deductions,sm.bankaccount,st.IsConsolid";
                        strquery = strquery + " from staffmaster sm,stafftrans st,hrdept_master h,desig_master d where sm.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and sm.college_code=d.collegeCode and sm.college_code=h.college_code  and sm.college_code = d.collegecode";
                        // strquery = strquery + " and h.dept_code in(" + deptcode + ") and d.desig_code in(" + design + ") and st.category_code in(" + cateory + ")  and st.latestrec=1 and ((sm.resign = 0 and settled = 0) or (sm.resign = 1 and settled = 1 and relieve_date < '" + dtt.ToString("MM/dd/yyyy") + "')) order by h.dept_code,d.desig_code,sm.staff_code";
                        strquery = strquery + " and h.dept_code in(" + deptcode + ") and d.desig_code in(" + design + ") and st.category_code in(" + cateory + ")  and st.latestrec=1 and ((sm.resign = 0 and settled = 0) or (sm.resign = 1 and settled = 1 and relieve_date >= '" + dtf.ToString("MM/dd/yyyy") + "' and relieve_date <= '" + dtt.ToString("MM/dd/yyyy") + "')) order by h.dept_code,d.desig_code,sm.staff_code";
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method_wo_parameter(strquery, "Text");

                        FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].Tag = "Master Entry";

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string staffcode = ds.Tables[0].Rows[i]["staff_code"].ToString();
                            string stftname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                            string depcode = ds.Tables[0].Rows[i]["dept_code"].ToString();
                            string depname = ds.Tables[0].Rows[i]["dept_name"].ToString();
                            string catcode = ds.Tables[0].Rows[i]["category_code"].ToString();
                            string catname = ds.Tables[0].Rows[i]["stftype"].ToString();
                            string desname = ds.Tables[0].Rows[i]["desig_name"].ToString();
                            string descode = ds.Tables[0].Rows[i]["desig_code"].ToString();
                            string basic = ds.Tables[0].Rows[i]["bsalary"].ToString();
                            string grade = ds.Tables[0].Rows[i]["grade_pay"].ToString();
                            string payband = ds.Tables[0].Rows[i]["pay_band"].ToString();
                            string allownce = ds.Tables[0].Rows[i]["allowances"].ToString();
                            string deduction = ds.Tables[0].Rows[i]["deductions"].ToString();
                            string IsConsolid = ds.Tables[0].Rows[i]["IsConsolid"].ToString();

                            if (basic.Trim().ToLower() == "")
                            {
                                basic = "0";
                            }
                            Double bascicam = Convert.ToDouble(basic);
                            bascicam = Math.Round(bascicam, 0, MidpointRounding.AwayFromZero);

                            dicgetsetval = dicmontotal[4] + bascicam;
                            dicmontotal[4] = dicgetsetval;

                            if (grade.Trim().ToLower() == "")
                            {
                                grade = "0";
                            }
                            Double gradeam = Convert.ToDouble(grade);
                            gradeam = Math.Round(gradeam, 0, MidpointRounding.AwayFromZero);

                            dicgetsetval = dicmontotal[5] + gradeam;
                            dicmontotal[5] = dicgetsetval;

                            if (payband.Trim().ToLower() == "")
                            {
                                payband = "0";
                            }
                            Double paybandamount = Convert.ToDouble(payband);
                            paybandamount = Math.Round(paybandamount, 0, MidpointRounding.AwayFromZero);

                            dicgetsetval = dicmontotal[6] + paybandamount;
                            dicmontotal[6] = dicgetsetval;

                            Double allototal = 0;
                            Double daamount = 0;
                            string[] spval = allownce.Split('\\');
                            int c = 6;
                            Double amountvalue = 0;
                            int strcol = 6;
                            for (c = strcol; c < decstrcolu; c++)
                            {
                                strcol++;
                                string hval = FpMonthOverall.Sheets[0].ColumnHeader.Cells[1, c].Text.ToString().Trim().ToLower();
                                for (int j = 0; j <= spval.GetUpperBound(0); j++)
                                {
                                    string[] spgb = spval[j].Split(';');
                                    if (spgb.GetUpperBound(0) > 2)
                                    {
                                        if (spgb[0].ToString().Trim().ToLower() == hval)
                                        {
                                            amountvalue = 0;
                                            if (spgb[2].ToString().Trim() != "")
                                            {
                                                if (spgb[1].ToString().Trim().ToLower() == "amount")
                                                {
                                                    string binval = spgb[2].ToString();
                                                    string[] get = binval.Split('-');
                                                    binval = get[0];
                                                    if (binval.ToString().Trim() != "")
                                                    {
                                                        amountvalue = Convert.ToDouble(binval);
                                                    }
                                                    // amountvalue = Convert.ToDouble(spgb[2].ToString());
                                                }
                                                else
                                                {
                                                    if (spgb.GetUpperBound(0) >= 8)
                                                    {
                                                        if (spgb[6].ToString().Trim().ToLower() == "1")
                                                        {
                                                            amountvalue = bascicam * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                        }
                                                        else if (spgb[8].ToString().Trim().ToLower() == "1")
                                                        {
                                                            amountvalue = bascicam + gradeam;
                                                            amountvalue = amountvalue * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                        }
                                                    }
                                                }

                                                amountvalue = Math.Round(amountvalue, 0, MidpointRounding.AwayFromZero);
                                                dicgetsetval = dicmontotal[c] + amountvalue;
                                                dicmontotal[c] = dicgetsetval;

                                                if (hval.Trim().ToLower() == "da")
                                                {
                                                    daamount = amountvalue;
                                                }
                                                allototal = allototal + amountvalue;
                                                allototal = Math.Round(allototal, 0, MidpointRounding.AwayFromZero);
                                                j = spval.GetUpperBound(0) + 1;
                                            }
                                        }
                                    }
                                }
                            }
                            strcol = decstrcolu - 2;
                            dicgetsetval = dicmontotal[strcol] + allototal;
                            dicmontotal[strcol] = dicgetsetval;

                            Double grossamount = allototal + bascicam + gradeam + paybandamount;
                            if (IsConsolid != "1")
                            {
                                grossamount = allototal + bascicam;
                            }
                            strcol++;
                            dicgetsetval = dicmontotal[strcol] + grossamount;
                            dicmontotal[strcol] = dicgetsetval;

                            Double deducttotal = 0;
                            spval = deduction.Split('\\');
                            for (c = decstrcolu; c < FpMonthOverall.Sheets[0].ColumnCount; c++)
                            {
                                string hval = FpMonthOverall.Sheets[0].ColumnHeader.Cells[1, c].Text.ToString().Trim().ToLower();
                                for (int j = 0; j <= spval.GetUpperBound(0); j++)
                                {
                                    string[] spgb = spval[j].Split(';');
                                    if (spgb.GetUpperBound(0) > 2)
                                    {
                                        if (spgb[0].ToString().Trim().ToLower() == hval)
                                        {
                                            amountvalue = 0;
                                            if (spgb[1].ToString().Trim().ToLower() == "amount")
                                            {
                                                string binval = spgb[2].ToString();
                                                string[] get = binval.Split('-');
                                                binval = get[0];
                                                if (binval.ToString().Trim() != "")
                                                {
                                                    amountvalue = Convert.ToDouble(binval);
                                                }
                                                // amountvalue = Convert.ToDouble(spgb[2].ToString());
                                            }
                                            else
                                            {
                                                if (spgb.GetUpperBound(0) >= 8)
                                                {
                                                    if (spgb[3].ToString().Trim().ToLower() == "1")
                                                    {
                                                        amountvalue = grossamount * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                    }
                                                    else if (spgb[4].ToString().Trim().ToLower() == "1")
                                                    {
                                                        amountvalue = bascicam + daamount;
                                                        amountvalue = amountvalue * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                    }
                                                    else if (spgb[7].ToString().Trim().ToLower() == "1")
                                                    {
                                                        amountvalue = bascicam + gradeam + daamount;
                                                    }
                                                    else if (spgb[8].ToString().Trim().ToLower() == "1")
                                                    {
                                                        amountvalue = bascicam * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                    }
                                                }
                                            }
                                            amountvalue = Math.Round(amountvalue, 0, MidpointRounding.AwayFromZero);
                                            dicgetsetval = dicmontotal[c] + amountvalue;
                                            dicmontotal[c] = dicgetsetval;

                                            deducttotal = deducttotal + amountvalue;
                                            deducttotal = Math.Round(deducttotal, 0, MidpointRounding.AwayFromZero);
                                            j = spval.GetUpperBound(0) + 1;
                                        }
                                    }
                                }
                            }
                            strcol = FpMonthOverall.Sheets[0].ColumnCount - 1;
                            dicgetsetval = dicmontotal[strcol] + deducttotal;
                            dicmontotal[strcol] = dicgetsetval;

                        }
                    }
                    dicmontotal[3] = ds.Tables[0].Rows.Count;
                    for (int c = 3; c < FpMonthOverall.Sheets[0].ColumnCount; c++)
                    {
                        if (dicmontotal.ContainsKey(c))
                        {
                            dicgetsetval = dicmontotal[c];

                            FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, c].CellType = txt;
                            FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, c].Text = dicgetsetval.ToString();
                            FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                            FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Large;
                            dicmontotal[c] = 0;
                        }
                    }
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 1].Locked = false;
                }
                else
                {
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 1].Locked = true;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 3].Text = yeartext + " - " + monthname.ToString() + " Hr Year Details Not Found";
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 3].ForeColor = Color.Red;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Large;
                    FpMonthOverall.Sheets[0].SpanModel.Add(FpMonthOverall.Sheets[0].RowCount - 1, 3, 1, FpMonthOverall.Sheets[0].ColumnCount);
                }
            }
            if (btngenerate.Visible == true)
            {
                lblexcel1.Visible = true;
                txtexcel1.Visible = true;
                btnexcel1.Visible = true;
                btnprint1.Visible = true;

                FpMonthOverall.Sheets[0].RowCount++;
                FpMonthOverall.Sheets[0].Rows[FpMonthOverall.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].Text = "Total";
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                FpMonthOverall.Sheets[0].SpanModel.Add(FpMonthOverall.Sheets[0].RowCount - 1, 0, 1, 4);
                for (int c = 4; c < FpMonthOverall.Sheets[0].ColumnCount; c++)
                {
                    Double setval = 0;
                    for (int r = 0; r < FpMonthOverall.Sheets[0].RowCount; r++)
                    {
                        string strval = FpMonthOverall.Sheets[0].Cells[r, c].Text.ToString();
                        if (strval.Trim() != "")
                        {
                            setval = setval + Convert.ToDouble(strval);
                        }
                    }

                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, c].CellType = txt;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, c].Text = setval.ToString();
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, c].Font.Bold = true;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Large;
                }
            }

            FpMonthOverall.Sheets[0].PageSize = FpMonthOverall.Sheets[0].RowCount;

            ds.Dispose();
            ds.Reset();
            ds = null;

            dshryear.Dispose();
            dshryear.Reset();
            dshryear = null;

            dicmontotal.Clear();
            dicmontotal = null;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void staffwisereport()
    {
        try
        {
            clear();


            string fromyear = ddlfyear.SelectedValue.ToString();
            string frommonth = ddlfmonth.SelectedValue.ToString();
            string toyear = ddltyear.SelectedValue.ToString();
            string tomonth = ddltmonth.SelectedValue.ToString();

            if (frommonth.Trim() == "0")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The From Month And Then Proceed";
                return;
            }

            if (tomonth.Trim() == "0")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The TO Month And Then Proceed";
                return;
            }

            int fromyearval = (Convert.ToInt32(fromyear) * 12) + Convert.ToInt32(frommonth);
            int toyearval = (Convert.ToInt32(toyear) * 12) + Convert.ToInt32(tomonth);
            if (fromyearval > toyearval)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The To Month And Year Must Be Equal To Greater Than From Month And Year";
                return;
            }

            string deptcode = "";
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        deptcode = deptcode + ",'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (deptcode.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Department And Then Proceed";
                return;
            }

            string design = "";
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                if (chklsdesign.Items[i].Selected == true)
                {
                    if (design == "")
                    {
                        design = "'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        design = design + ",'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (design.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Designation And Then Proceed";
                return;
            }

            string cateory = "";
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    if (cateory == "")
                    {
                        cateory = "'" + chklscategory.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        cateory = cateory + ",'" + chklscategory.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (cateory.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Category And Then Proceed";
                return;
            }

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();

            string strhryearquery = "select PayMonthNum,PayYear,From_Date,To_Date,year(from_date) fyear,year(to_date) tyear from HrPayMonths where College_Code='" + collegecode + "'";
            DataSet dshryear = d2.select_method_wo_parameter(strhryearquery, "Text");

            FpMonthOverall.CommandBar.Visible = false;
            FpMonthOverall.Sheets[0].SheetCorner.ColumnCount = 0;
            FpMonthOverall.Sheets[0].ColumnCount = 0;
            FpMonthOverall.Sheets[0].RowCount = 0;
            FpMonthOverall.Sheets[0].ColumnHeader.RowCount = 0;

            FpMonthOverall.Sheets[0].ColumnHeader.RowCount = 2;
            FpMonthOverall.Sheets[0].ColumnCount = 7;

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpMonthOverall.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[0].Width = 50;
            FpMonthOverall.Sheets[0].Columns[0].Locked = true;
            FpMonthOverall.Sheets[0].Columns[0].CellType = txt;

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpMonthOverall.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[1].Width = 50;

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpMonthOverall.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[2].Width = 150;
            FpMonthOverall.Sheets[0].Columns[2].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[2].Locked = true;
            FpMonthOverall.Sheets[0].Columns[2].Visible = true;
            if (chklscolumn.Items[7].Selected == false)
            {
                FpMonthOverall.Sheets[0].Columns[2].Visible = false;
            }

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpMonthOverall.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[3].Width = 100;
            FpMonthOverall.Sheets[0].Columns[3].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[3].Locked = true;

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpMonthOverall.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[4].Width = 100;
            FpMonthOverall.Sheets[0].Columns[4].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[4].Locked = true;
            FpMonthOverall.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpMonthOverall.Sheets[0].Columns[4].Visible = true;
            if (chklscolumn.Items[8].Selected == false)
            {
                FpMonthOverall.Sheets[0].Columns[4].Visible = false;
            }

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Designation";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            FpMonthOverall.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[6].Width = 100;
            FpMonthOverall.Sheets[0].Columns[5].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[5].Locked = true;
            FpMonthOverall.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpMonthOverall.Sheets[0].Columns[5].Visible = true;
            if (chklscolumn.Items[9].Selected == false)
            {
                FpMonthOverall.Sheets[0].Columns[5].Visible = false;
            }

            FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Category";
            FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            FpMonthOverall.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Columns[6].Width = 100;
            FpMonthOverall.Sheets[0].Columns[6].CellType = txt;
            FpMonthOverall.Sheets[0].Columns[6].Locked = true;
            FpMonthOverall.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpMonthOverall.Sheets[0].Columns[6].Visible = true;
            if (chklscolumn.Items[10].Selected == false)
            {
                FpMonthOverall.Sheets[0].Columns[6].Visible = false;
            }

            //FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Basic Pay";
            //FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
            //FpMonthOverall.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
            //FpMonthOverall.Sheets[0].Columns[7].Width = 100;
            //FpMonthOverall.Sheets[0].Columns[7].CellType = txt;
            //FpMonthOverall.Sheets[0].Columns[7].Locked = true;
            ////dicmontotal.Add(4, 0);

            //FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Grade Pay";
            //FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
            //FpMonthOverall.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
            //FpMonthOverall.Sheets[0].Columns[8].Width = 100;
            //FpMonthOverall.Sheets[0].Columns[8].CellType = txt;
            //FpMonthOverall.Sheets[0].Columns[8].Locked = true;
            ////dicmontotal.Add(5, 0);

            //FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Pay Band";
            //FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
            //FpMonthOverall.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
            //FpMonthOverall.Sheets[0].Columns[9].Width = 100;
            //FpMonthOverall.Sheets[0].Columns[9].CellType = txt;
            //FpMonthOverall.Sheets[0].Columns[9].Locked = true;
            ////dicmontotal.Add(6, 0);

            //int spart = 0;
            //commcount = 0;
            //for (int i = 0; i < chklsallowance.Items.Count; i++)
            //{
            //    FpMonthOverall.Sheets[0].ColumnCount++;
            //    FpMonthOverall.Sheets[0].ColumnHeader.Cells[1, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = chklsallowance.Items[i].Text.ToString();
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;
            //    //dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);
            //    if (chklsallowance.Items[i].Selected == true)
            //    {
            //        commcount++;
            //        if (spart == 0)
            //        {
            //            spart = FpMonthOverall.Sheets[0].ColumnCount - 1;
            //        }
            //    }
            //    else
            //    {
            //        FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = false;
            //    }
            //}
            //if (commcount > 0)
            //{
            //    FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, spart].Text = "Allowance";
            //    FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, spart, 1, commcount);


            //    FpMonthOverall.Sheets[0].ColumnCount++;
            //    FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = "Total Allowance";
            //    FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, FpMonthOverall.Sheets[0].ColumnCount - 1, 2, 1);
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;
            //    //dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);
            //}

            //FpMonthOverall.Sheets[0].ColumnCount++;
            //FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = "Gross Amount";
            //FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, FpMonthOverall.Sheets[0].ColumnCount - 1, 2, 1);
            //FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            //FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
            //FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
            //FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;
            ////dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);

            //int decstrcolu = FpMonthOverall.Sheets[0].ColumnCount;
            //int setdeuctcolun = 0;
            //commcount = 0;
            //for (int i = 0; i < chklsdeduction.Items.Count; i++)
            //{
            //    FpMonthOverall.Sheets[0].ColumnCount++;
            //    FpMonthOverall.Sheets[0].ColumnHeader.Cells[1, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = chklsdeduction.Items[i].Text.ToString();
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;
            //    //dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);
            //    if (chklsdeduction.Items[i].Selected == true)
            //    {
            //        FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = true;
            //        commcount++;
            //        if (setdeuctcolun == 0)
            //        {
            //            setdeuctcolun = FpMonthOverall.Sheets[0].ColumnCount - 1;
            //        }
            //    }
            //    else
            //    {
            //        if (commcount > 0)
            //        {
            //            commcount++;
            //        }
            //        FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Visible = false;
            //    }
            //}
            //if (setdeuctcolun > 0)
            //{
            //    FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, setdeuctcolun].Text = "Deduction";
            //    FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, setdeuctcolun, 1, commcount);

            //    FpMonthOverall.Sheets[0].ColumnCount++;
            //    FpMonthOverall.Sheets[0].ColumnHeader.Cells[0, FpMonthOverall.Sheets[0].ColumnCount - 1].Text = "Total Deduction";
            //    FpMonthOverall.Sheets[0].ColumnHeaderSpanModel.Add(0, FpMonthOverall.Sheets[0].ColumnCount - 1, 2, 1);
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Width = 80;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].CellType = txt;
            //    FpMonthOverall.Sheets[0].Columns[FpMonthOverall.Sheets[0].ColumnCount - 1].Locked = true;
            //    //dicmontotal.Add(FpMonthOverall.Sheets[0].ColumnCount - 1, 0);
            //}

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpMonthOverall.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpMonthOverall.Sheets[0].SheetName = " ";
            FpMonthOverall.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpMonthOverall.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpMonthOverall.Sheets[0].DefaultStyle.Font.Size = FontUnit.Large;
            FpMonthOverall.Sheets[0].DefaultStyle.Font.Bold = false;
            FpMonthOverall.Width = 800;
            FpMonthOverall.Visible = true;
            FpMonthOverall.Sheets[0].AutoPostBack = false;
            int srno = 0;

            ds.Dispose();
            ds.Reset();
            string strquery = "select h.dept_name,h.dept_code,d.desig_name,d.desig_code,st.stftype,st.category_code,sm.staff_name,sm.staff_code,len(sm.staff_code),sm.bankaccount,h.priority,d.priority,sm.join_date,sm.PrintPriority,h.priority1,d.print_pri from staffmaster sm,hrdept_master h,desig_master d,stafftrans st where sm.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and sm.college_code = h.college_code and sm.college_code = d.collegecode and h.dept_code in(" + deptcode + ") and d.desig_code in(" + design + ") and st.category_code in(" + cateory + ") and sm.college_code='" + collegecode + "' and sm.resign = 0 and sm.settled = 0 and latestrec=1";
            if (ddlorder.SelectedItem.Text.ToString() == "Priority")
            {
                strquery = strquery + " order by h.priority,d.priority,h.dept_name,sm.join_date,len(sm.staff_code),sm.staff_code ";
            }
            else if (ddlorder.SelectedItem.Text.ToString() == "Print Priority-1")
            {
                strquery = strquery + " order by h.priority,d.priority,sm.PrintPriority,sm.join_date,len(sm.staff_code),sm.staff_code";
            }
            else if (ddlorder.SelectedItem.Text.ToString() == "Print Priority-2")
            {
                strquery = strquery + " order by h.priority1,d.print_pri,sm.PrintPriority,sm.join_date,len(sm.staff_code),sm.staff_code";
            }
            else if (ddlorder.SelectedItem.Text.ToString() == "Account No")
            {
                strquery = strquery + " order by case when sm.bankaccount is null then 1 else 0 end,len(sm.bankaccount),sm.bankaccount";
            }
            else if (ddlorder.SelectedItem.Text.ToString() == "Staff Wise Priority")
            {
                strquery = strquery + " order by isnull(sm.PrintPriority,0) ";
            }
            else
            {
                strquery = strquery + " order by h.dept_name,len(sm.staff_code),sm.staff_code";
            }
            ds = d2.select_method_wo_parameter(strquery, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                FpMonthOverall.Sheets[0].RowCount++;
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 1].CellType = chkall;
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 1].Locked = false;
                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].Locked = true;
                FpMonthOverall.Sheets[0].SpanModel.Add(FpMonthOverall.Sheets[0].RowCount - 1, 2, 1, FpMonthOverall.Sheets[0].ColumnCount);

                btngenerate.Visible = true;

                //btngenerate.Visible = true;
                lblexcel1.Visible = false;
                txtexcel1.Visible = false;
                btnexcel1.Visible = false;
                btnprint1.Visible = false;

                for (int yemonval = 0; yemonval < ds.Tables[0].Rows.Count; yemonval++)
                {
                    string staffcode = ds.Tables[0].Rows[yemonval]["staff_code"].ToString();
                    string stftname = ds.Tables[0].Rows[yemonval]["staff_name"].ToString();
                    string depcode = ds.Tables[0].Rows[yemonval]["dept_code"].ToString();
                    string depname = ds.Tables[0].Rows[yemonval]["dept_name"].ToString();
                    string catcode = ds.Tables[0].Rows[yemonval]["category_code"].ToString();
                    string catname = ds.Tables[0].Rows[yemonval]["stftype"].ToString();
                    string desname = ds.Tables[0].Rows[yemonval]["desig_name"].ToString();
                    string descode = ds.Tables[0].Rows[yemonval]["desig_code"].ToString();

                    srno++;
                    FpMonthOverall.Sheets[0].RowCount++;
                    if ((srno % 2) == 1)
                    {
                        FpMonthOverall.Sheets[0].Rows[FpMonthOverall.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                    }

                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 1].CellType = chk;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].Text = staffcode.ToString();
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Large;

                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 3].Text = stftname.ToString();
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Large;

                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 4].Text = depname.ToString();
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 4].Tag = depcode;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Large;

                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 5].Text = desname.ToString();
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 5].Tag = descode;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Large;

                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 6].Text = catname.ToString();
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 6].Tag = catcode;
                    FpMonthOverall.Sheets[0].Cells[FpMonthOverall.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Large;
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            FpMonthOverall.Sheets[0].PageSize = FpMonthOverall.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void btnprint1_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Original Salary Details Report";
            string pagename = "Original Salary Details Entry.aspx";

            Printmaster1.loadspreaddetails(FpMonthOverall, pagename, degreedetails);
            Printmaster1.Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void btnexcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpMonthOverall, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Original Salary Details Report";
            string pagename = "Original Salary Details Report.aspx";

            Printcontrol.loadspreaddetails(FpSalaryReport, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSalaryReport, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void btngenerate_Click(object sender, EventArgs e)
    {
        try
        {
            if (rbconsolidate.Checked == true)
            {
                loadconsolidatereportdetails();
            }
            else
            {
                if (ddlformatewise.SelectedItem.Value == "0")
                {

                    staffpayprocessprint();
                }
                if (ddlformatewise.SelectedItem.Value == "1")
                {
                    staffSalaryCertificate();//delsi0102

                }

            }
        }
        catch (Exception ex)
        {
            lblgenerror.Text = ex.ToString();
            lblgenerror.Visible = true;
        }

    }

    public void loadconsolidatereportdetails()
    {
        try
        {
            int noofmonth = 0;
            FpMonthOverall.SaveChanges();
            for (int i = 0; i < FpMonthOverall.Sheets[0].RowCount; i++)
            {
                int isval = Convert.ToInt32(FpMonthOverall.Sheets[0].Cells[i, 1].Value);
                if (isval == 1)
                {
                    noofmonth++;
                }
            }
            if (noofmonth == 0)
            {
                lblgenerror.Visible = true;
                lblgenerror.Text = "Please Select The Year - Month And Then Proceed";
                return;
            }
            else if (noofmonth > 1)
            {
                lblgenerror.Visible = true;
                lblgenerror.Text = "Please Select The Only One Year - Month And Then Proceed";
                return;
            }

            string deptcode = "";
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        deptcode = deptcode + ",'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (deptcode.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Department And Then Proceed";
                return;
            }

            string design = "";
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                if (chklsdesign.Items[i].Selected == true)
                {
                    if (design == "")
                    {
                        design = "'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        design = design + ",'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (design.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Designation And Then Proceed";
                return;
            }

            string cateory = "";
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    if (cateory == "")
                    {
                        cateory = "'" + chklscategory.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        cateory = cateory + ",'" + chklscategory.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (cateory.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Category And Then Proceed";
                return;
            }

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;
            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            for (int r = 0; r < FpMonthOverall.Sheets[0].RowCount; r++)
            {
                int isval = Convert.ToInt32(FpMonthOverall.Sheets[0].Cells[r, 1].Value);
                if (isval == 1)
                {
                    string[] spdate = FpMonthOverall.Sheets[0].Cells[r, 3].Tag.ToString().Split('@');
                    DateTime dtf = Convert.ToDateTime(spdate[0]);
                    DateTime dtt = Convert.ToDateTime(spdate[1]);

                    string payprocesstext = FpMonthOverall.Sheets[0].Cells[r, 2].Tag.ToString();

                    string strquery = "Select h.dept_name,h.dept_code,d.desig_name,d.desig_code,st.stftype,st.category_code,sm.staff_name,sm.staff_code,st.bsalary,st.pay_band,st.grade_pay,st.allowances,st.deductions,st.IsConsolid,len(sm.staff_code),sm.bankaccount,h.priority,d.priority,sm.join_date,sm.PrintPriority,h.priority1,d.print_pri ";
                    strquery = strquery + " from staffmaster sm,stafftrans st,hrdept_master h,desig_master d where sm.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and sm.college_code=d.collegeCode and sm.college_code=h.college_code  and sm.college_code = d.collegecode";
                    // strquery = strquery + " and h.dept_code in(" + deptcode + ") and d.desig_code in(" + design + ") and st.category_code in(" + cateory + ") and st.latestrec=1 and ((sm.resign = 0 and settled = 0) or (sm.resign = 1 and settled = 1 and relieve_date < '" + dtt.ToString("MM/dd/yyyy") + "')) order by h.dept_code,d.desig_code,sm.staff_code";
                    strquery = strquery + " and h.dept_code in(" + deptcode + ") and d.desig_code in(" + design + ") and st.category_code in(" + cateory + ") and st.latestrec=1 and ((sm.resign = 0 and settled = 0) or (sm.resign = 1 and settled = 1 and relieve_date >= '" + dtf.ToString("MM/dd/yyyy") + "' and relieve_date <= '" + dtt.ToString("MM/dd/yyyy") + "'))";
                    if (payprocesstext == "Pay Process")
                    {
                        strquery = "select h.dept_name,h.dept_code,d.desig_name,d.desig_code,st.stftype,st.category_code,sm.staff_name,sm.staff_code,m.PayYear,m.PayMonth,m.fdate,m.tdate,m.basic_alone,m.grade_pay,m.pay_band,m.allowances,m.deductions,m.NetAddAct as gross,m.netded as totaldeduction,st.IsConsolid,len(sm.staff_code),sm.bankaccount,h.priority,d.priority,sm.join_date,sm.PrintPriority,h.priority1,d.print_pri ";
                        strquery = strquery + " from monthlypay m,stafftrans st,staffmaster sm,hrdept_master h,desig_master d where sm.staff_code=st.staff_code and st.staff_code=m.staff_code and sm.staff_code=m.staff_code and st.dept_code =h.dept_code and st.desig_code=d.desig_code and h.college_code=sm.college_code  and sm.college_code = d.collegecode";
                        strquery = strquery + " and h.dept_code in(" + deptcode + ") and d.desig_code in(" + design + ") and st.category_code in(" + cateory + ") and sm.college_code=m.college_code";
                        //strquery = strquery + "  and st.latestrec=1 and ((sm.resign = 0 and settled = 0) or (sm.resign = 1 and settled = 1 and relieve_date < '" + dtt.ToString("MM/dd/yyyy") + "')) and m.fdate = '" + dtf.ToString("MM/dd/yyyy") + "' order by h.dept_code,d.desig_code,sm.staff_code";
                        strquery = strquery + "  and st.latestrec=1 and ((sm.resign = 0 and settled = 0) or (sm.resign = 1 and settled = 1  and relieve_date >= '" + dtf.ToString("MM/dd/yyyy") + "' and relieve_date <= '" + dtt.ToString("MM/dd/yyyy") + "')) and m.fdate = '" + dtf.ToString("MM/dd/yyyy") + "'";
                    }

                    if (ddlorder.SelectedItem.Text.ToString() == "Priority")
                    {
                        strquery = strquery + " order by h.priority,d.priority,h.dept_name,sm.join_date,len(sm.staff_code),sm.staff_code ";
                    }
                    else if (ddlorder.SelectedItem.Text.ToString() == "Print Priority-1")
                    {
                        strquery = strquery + " order by h.priority,d.priority,sm.PrintPriority,sm.join_date,len(sm.staff_code),sm.staff_code";
                    }
                    else if (ddlorder.SelectedItem.Text.ToString() == "Print Priority-2")
                    {
                        strquery = strquery + " order by h.priority1,d.print_pri,sm.PrintPriority,sm.join_date,len(sm.staff_code),sm.staff_code";
                    }
                    else if (ddlorder.SelectedItem.Text.ToString() == "Account No")
                    {
                        strquery = strquery + " order by case when sm.bankaccount is null then 1 else 0 end,len(sm.bankaccount), sm.bankaccount";
                    }
                    else if (ddlorder.SelectedItem.Text.ToString() == "Staff Wise Priority")
                    {
                        strquery = strquery + " order by isnull(sm.PrintPriority,0) ";
                    }
                    else
                    {
                        strquery = strquery + " order by h.dept_name,len(sm.staff_code),sm.staff_code";
                    }
                    ds.Dispose();
                    ds.Reset();
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSalaryReport.CommandBar.Visible = false;
                        FpSalaryReport.Sheets[0].SheetCorner.ColumnCount = 0;
                        FpSalaryReport.Sheets[0].ColumnCount = 0;
                        FpSalaryReport.Sheets[0].RowCount = 0;
                        FpSalaryReport.Sheets[0].ColumnHeader.RowCount = 0;

                        FpSalaryReport.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSalaryReport.Sheets[0].ColumnCount = 10;


                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[0].Width = 50;
                        FpSalaryReport.Sheets[0].Columns[0].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[0].Locked = true;

                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[1].Width = 50;
                        FpSalaryReport.Sheets[0].Columns[1].Visible = false;

                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[2].Width = 150;
                        FpSalaryReport.Sheets[0].Columns[2].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[2].Locked = true;
                        FpSalaryReport.Sheets[0].Columns[2].Visible = true;
                        if (chklscolumn.Items[7].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[2].Visible = false;
                        }

                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[3].Width = 150;
                        FpSalaryReport.Sheets[0].Columns[3].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[3].Locked = true;

                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[4].Width = 150;
                        FpSalaryReport.Sheets[0].Columns[4].CellType = txt;
                        FpSalaryReport.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSalaryReport.Sheets[0].Columns[4].Locked = true;
                        FpSalaryReport.Sheets[0].Columns[4].Visible = true;
                        if (chklscolumn.Items[8].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[4].Visible = false;
                        }

                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Designation";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[5].Width = 100;
                        FpSalaryReport.Sheets[0].Columns[5].CellType = txt;
                        FpSalaryReport.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSalaryReport.Sheets[0].Columns[5].Locked = true;
                        FpSalaryReport.Sheets[0].Columns[4].Visible = true;
                        if (chklscolumn.Items[9].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[9].Visible = false;
                        }

                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Category";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[6].Width = 200;
                        FpSalaryReport.Sheets[0].Columns[6].CellType = txt;
                        FpSalaryReport.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSalaryReport.Sheets[0].Columns[6].Locked = true;
                        FpSalaryReport.Sheets[0].Columns[4].Visible = true;
                        if (chklscolumn.Items[10].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[10].Visible = false;
                        }

                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Basic Pay";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[7].Width = 100;
                        FpSalaryReport.Sheets[0].Columns[7].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[7].Locked = true;

                        FpSalaryReport.Sheets[0].Columns[7].Visible = true;
                        if (chklscolumn.Items[1].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[7].Visible = false;
                        }

                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Grade Pay";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[8].Width = 100;
                        FpSalaryReport.Sheets[0].Columns[8].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[8].Locked = true;

                        FpSalaryReport.Sheets[0].Columns[8].Visible = true;
                        if (chklscolumn.Items[2].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[8].Visible = false;
                        }

                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Pay Band";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[9].Width = 100;
                        FpSalaryReport.Sheets[0].Columns[9].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[9].Locked = true;

                        FpSalaryReport.Sheets[0].Columns[9].Visible = true;
                        if (chklscolumn.Items[3].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[9].Visible = false;
                        }

                        int spart = 0;
                        commcount = 0;
                        for (int i = 0; i < chklsallowance.Items.Count; i++)
                        {
                            FpSalaryReport.Sheets[0].ColumnCount++;
                            FpSalaryReport.Sheets[0].ColumnHeader.Cells[1, FpSalaryReport.Sheets[0].ColumnCount - 1].Text = chklsallowance.Items[i].Text.ToString();
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Width = 80;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].CellType = txt;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Locked = true;
                            if (chklsallowance.Items[i].Selected == true)
                            {
                                commcount++;
                                if (spart == 0)
                                {
                                    spart = FpSalaryReport.Sheets[0].ColumnCount - 1;
                                }
                            }
                            else
                            {
                                if (commcount > 0)
                                {
                                    commcount++;
                                }
                                FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = false;
                            }
                        }
                        if (commcount > 0)
                        {
                            FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, spart].Text = "Allowance";
                            FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, spart, 1, commcount);


                            FpSalaryReport.Sheets[0].ColumnCount++;
                            FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, FpSalaryReport.Sheets[0].ColumnCount - 1].Text = "Total Allowance";
                            FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSalaryReport.Sheets[0].ColumnCount - 1, 2, 1);
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Width = 80;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].CellType = txt;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Locked = true;

                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = true;
                            if (chklscolumn.Items[4].Selected == false)
                            {
                                FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = false;
                            }
                        }

                        FpSalaryReport.Sheets[0].ColumnCount++;
                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, FpSalaryReport.Sheets[0].ColumnCount - 1].Text = "Gross Amount";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSalaryReport.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Width = 80;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Locked = true;

                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = true;
                        if (chklscolumn.Items[5].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = false;
                        }

                        int decstrcolu = FpSalaryReport.Sheets[0].ColumnCount;
                        int decstrcoluset = 0;
                        commcount = 0;
                        for (int i = 0; i < chklsdeduction.Items.Count; i++)
                        {
                            FpSalaryReport.Sheets[0].ColumnCount++;
                            FpSalaryReport.Sheets[0].ColumnHeader.Cells[1, FpSalaryReport.Sheets[0].ColumnCount - 1].Text = chklsdeduction.Items[i].Text.ToString();
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Width = 80;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].CellType = txt;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Locked = true;
                            if (chklsdeduction.Items[i].Selected == true)
                            {
                                commcount++;
                                if (decstrcoluset == 0)
                                {
                                    decstrcoluset = FpSalaryReport.Sheets[0].ColumnCount - 1;
                                }
                            }
                            else
                            {
                                if (commcount > 0)
                                {
                                    commcount++;
                                }
                                FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = false;
                            }
                        }
                        if (commcount > 0)
                        {
                            FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, decstrcoluset].Text = "Deduction";
                            FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, decstrcoluset, 1, commcount);

                            FpSalaryReport.Sheets[0].ColumnCount++;
                            FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, FpSalaryReport.Sheets[0].ColumnCount - 1].Text = "Total Deduction";
                            FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSalaryReport.Sheets[0].ColumnCount - 1, 2, 1);
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Width = 80;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].CellType = txt;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Locked = true;
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = true;
                            if (chklscolumn.Items[6].Selected == false)
                            {
                                FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = false;
                            }
                        }

                        FpSalaryReport.Sheets[0].ColumnCount++;
                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, FpSalaryReport.Sheets[0].ColumnCount - 1].Text = "Net Pay";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSalaryReport.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Width = 80;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = false;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Locked = true;
                        if (chklscolumn.Items[12].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = false;
                        }

                        FpSalaryReport.Sheets[0].ColumnCount++;
                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, FpSalaryReport.Sheets[0].ColumnCount - 1].Text = "A/C No";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSalaryReport.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Width = 80;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = false;

                        FpSalaryReport.Sheets[0].ColumnCount++;
                        FpSalaryReport.Sheets[0].ColumnHeader.Cells[0, FpSalaryReport.Sheets[0].ColumnCount - 1].Text = "Remarks";
                        FpSalaryReport.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSalaryReport.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Width = 80;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].CellType = txt;
                        FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Locked = true;
                        if (chklscolumn.Items[13].Selected == false)
                        {
                            FpSalaryReport.Sheets[0].Columns[FpSalaryReport.Sheets[0].ColumnCount - 1].Visible = false;
                        }

                        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                        style2.Font.Size = 13;
                        style2.Font.Name = "Book Antiqua";
                        style2.Font.Bold = true;
                        style2.HorizontalAlign = HorizontalAlign.Center;
                        style2.ForeColor = System.Drawing.Color.Black;
                        style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        FpSalaryReport.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

                        FpSalaryReport.Sheets[0].SheetName = " ";
                        FpSalaryReport.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                        FpSalaryReport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                        FpSalaryReport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Large;
                        FpSalaryReport.Sheets[0].DefaultStyle.Font.Bold = false;
                        FpSalaryReport.Width = 1000;
                        FpSalaryReport.Visible = true;
                        FpSalaryReport.Sheets[0].AutoPostBack = false;

                        //FpSalaryReport.Sheets[0].RowCount++;
                        //FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 1].CellType = chkall;
                        //FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 1].Locked = false;
                        //FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        //FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 2].Locked = true;
                        //FpSalaryReport.Sheets[0].SpanModel.Add(FpSalaryReport.Sheets[0].RowCount - 1, 2, 1, FpSalaryReport.Sheets[0].ColumnCount);

                        int srno = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string staffcode = ds.Tables[0].Rows[i]["staff_code"].ToString();
                            string stftname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                            string depcode = ds.Tables[0].Rows[i]["dept_code"].ToString();
                            string depname = ds.Tables[0].Rows[i]["dept_name"].ToString();
                            string catcode = ds.Tables[0].Rows[i]["category_code"].ToString();
                            string catname = ds.Tables[0].Rows[i]["stftype"].ToString();
                            string desname = ds.Tables[0].Rows[i]["desig_name"].ToString();
                            string descode = ds.Tables[0].Rows[i]["desig_code"].ToString();
                            string basic = "";
                            if (payprocesstext == "Pay Process")
                            {
                                basic = ds.Tables[0].Rows[i]["basic_alone"].ToString();
                            }
                            else
                            {
                                basic = ds.Tables[0].Rows[i]["bsalary"].ToString();
                            }
                            string grade = ds.Tables[0].Rows[i]["grade_pay"].ToString();
                            string payband = ds.Tables[0].Rows[i]["pay_band"].ToString();
                            string allownce = ds.Tables[0].Rows[i]["allowances"].ToString();
                            string deduction = ds.Tables[0].Rows[i]["deductions"].ToString();
                            string bankaccno = ds.Tables[0].Rows[i]["bankaccount"].ToString();
                            string IsConsolid = ds.Tables[0].Rows[i]["IsConsolid"].ToString();

                            if (basic.Trim().ToLower() == "")
                            {
                                basic = "0";
                            }
                            Double bascicam = Convert.ToDouble(basic);
                            bascicam = Math.Round(bascicam, 0, MidpointRounding.AwayFromZero);

                            if (grade.Trim().ToLower() == "")
                            {
                                grade = "0";
                            }
                            Double gradeam = Convert.ToDouble(grade);
                            gradeam = Math.Round(gradeam, 0, MidpointRounding.AwayFromZero);

                            if (payband.Trim().ToLower() == "")
                            {
                                payband = "0";
                            }
                            Double paybandamount = Convert.ToDouble(payband);
                            paybandamount = Math.Round(paybandamount, 0, MidpointRounding.AwayFromZero);

                            FpSalaryReport.Sheets[0].RowCount++;
                            srno++;
                            if ((srno % 2) == 0)
                            {
                                FpSalaryReport.Sheets[0].Rows[FpSalaryReport.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                            }
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 1].CellType = chk;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 2].Text = staffcode.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Large;

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 3].Text = stftname.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Large;

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 4].Text = depname.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Large;

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 5].Text = desname.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Large;

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 6].Text = catname.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Large;

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 7].Text = bascicam.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Large;

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 8].Text = gradeam.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Right;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Large;

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 9].Text = paybandamount.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Large;


                            Double allototal = 0;
                            Double daamount = 0;
                            string[] spval = allownce.Split('\\');
                            int c = 9;
                            Double amountvalue = 0;
                            int strcol = 10;
                            for (c = strcol; c < decstrcolu; c++)
                            {
                                strcol++;
                                string hval = FpSalaryReport.Sheets[0].ColumnHeader.Cells[1, c].Text.ToString().Trim().ToLower();
                                for (int j = 0; j <= spval.GetUpperBound(0); j++)
                                {
                                    string[] spgb = spval[j].Split(';');
                                    if (spgb.GetUpperBound(0) > 2)
                                    {
                                        if (spgb[0].ToString().Trim().ToLower() == hval)
                                        {
                                            amountvalue = 0;
                                            if (payprocesstext == "Pay Process")
                                            {
                                                string binval = spgb[2].ToString();
                                                string[] get = binval.Split('-');
                                                binval = get[0];
                                                if (binval.ToString().Trim() != "")
                                                {
                                                    amountvalue = Convert.ToDouble(binval.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (spgb[2].ToString().Trim() != "")
                                                {
                                                    if (spgb[1].ToString().Trim().ToLower() == "amount")
                                                    {
                                                        string binval = spgb[2].ToString();
                                                        string[] get = binval.Split('-');
                                                        binval = get[0];
                                                        if (binval.ToString().Trim() != "")
                                                        {
                                                            amountvalue = Convert.ToDouble(binval.ToString());
                                                        }
                                                        // amountvalue = Convert.ToDouble(spgb[2].ToString());
                                                    }
                                                    else
                                                    {
                                                        if (spgb.GetUpperBound(0) >= 8)
                                                        {
                                                            if (spgb[6].ToString().Trim().ToLower() == "1")
                                                            {
                                                                amountvalue = bascicam * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                            }
                                                            else if (spgb[8].ToString().Trim().ToLower() == "1")
                                                            {
                                                                amountvalue = bascicam + gradeam;
                                                                amountvalue = amountvalue * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            amountvalue = Math.Round(amountvalue, 0, MidpointRounding.AwayFromZero);
                                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].Text = amountvalue.ToString();
                                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Large;
                                            if (hval.Trim().ToLower() == "da")
                                            {
                                                daamount = amountvalue;
                                            }
                                            allototal = allototal + amountvalue;
                                            allototal = Math.Round(allototal, 0, MidpointRounding.AwayFromZero);
                                            j = spval.GetUpperBound(0) + 1;
                                        }
                                    }
                                }
                            }

                            strcol = decstrcolu - 2;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Text = allototal.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].HorizontalAlign = HorizontalAlign.Right;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Font.Size = FontUnit.Large;

                            Double grossamount = allototal + bascicam + gradeam + paybandamount;
                            if (IsConsolid != "1")
                            {
                                grossamount = allototal + bascicam;
                            }

                            strcol++;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Text = grossamount.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].HorizontalAlign = HorizontalAlign.Right;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Font.Size = FontUnit.Large;

                            Double deducttotal = 0;
                            spval = deduction.Split('\\');
                            for (c = decstrcolu; c < FpSalaryReport.Sheets[0].ColumnCount - 1; c++)
                            {
                                //strcol++;
                                string hval = FpSalaryReport.Sheets[0].ColumnHeader.Cells[1, c].Text.ToString().Trim().ToLower();
                                for (int j = 0; j <= spval.GetUpperBound(0); j++)
                                {
                                    string[] spgb = spval[j].Split(';');
                                    if (spgb.GetUpperBound(0) > 2)
                                    {
                                        if (spgb[0].ToString().Trim().ToLower() == hval)
                                        {
                                            amountvalue = 0;
                                            if (payprocesstext == "Pay Process")
                                            {
                                                string binval = spgb[3].ToString();
                                                string[] get = binval.Split('-');
                                                binval = get[0];
                                                if (binval.ToString().Trim() != "")
                                                {
                                                    amountvalue = Convert.ToDouble(binval.ToString());
                                                }
                                                //if (spgb[3].ToString().Trim() != "")
                                                //{
                                                //    amountvalue = Convert.ToDouble(spgb[3].ToString());
                                                //}
                                            }
                                            else
                                            {
                                                if (spgb[1].ToString().Trim().ToLower() == "amount")
                                                {
                                                    string binval = spgb[2].ToString();
                                                    string[] get = binval.Split('-');
                                                    binval = get[0];
                                                    if (binval.ToString().Trim() != "")
                                                    {
                                                        amountvalue = Convert.ToDouble(binval.ToString());
                                                    }
                                                    //amountvalue = Convert.ToDouble(spgb[2].ToString());
                                                }
                                                else
                                                {
                                                    if (spgb.GetUpperBound(0) >= 8)
                                                    {
                                                        if (spgb[3].ToString().Trim().ToLower() == "1")
                                                        {
                                                            amountvalue = grossamount * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                        }
                                                        else if (spgb[4].ToString().Trim().ToLower() == "1")
                                                        {
                                                            amountvalue = bascicam + daamount;
                                                            amountvalue = amountvalue * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                        }
                                                        else if (spgb[7].ToString().Trim().ToLower() == "1")
                                                        {
                                                            amountvalue = bascicam + gradeam + daamount;
                                                        }
                                                        else if (spgb[8].ToString().Trim().ToLower() == "1")
                                                        {
                                                            amountvalue = bascicam * (Convert.ToDouble(spgb[2].ToString()) / 100);
                                                        }
                                                    }
                                                }
                                            }
                                            amountvalue = Math.Round(amountvalue, 0, MidpointRounding.AwayFromZero);
                                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].Text = amountvalue.ToString();
                                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Large;

                                            deducttotal = deducttotal + amountvalue;
                                            deducttotal = Math.Round(deducttotal, 0, MidpointRounding.AwayFromZero);

                                            j = spval.GetUpperBound(0) + 1;
                                        }
                                    }
                                }
                            }
                            strcol = FpSalaryReport.Sheets[0].ColumnCount - 4;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Text = deducttotal.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].HorizontalAlign = HorizontalAlign.Right;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Font.Size = FontUnit.Large;

                            //strcol = FpSalaryReport.Sheets[0].ColumnCount - 2;
                            //grossamount = grossamount - deducttotal;
                            //FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Text = grossamount.ToString();
                            //FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].HorizontalAlign = HorizontalAlign.Right;
                            //FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Font.Size = FontUnit.Large;

                            strcol = FpSalaryReport.Sheets[0].ColumnCount - 2;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Text = bankaccno;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].HorizontalAlign = HorizontalAlign.Right;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, strcol].Font.Size = FontUnit.Large;
                        }
                        FpSalaryReport.Sheets[0].RowCount++;
                        FpSalaryReport.Sheets[0].Rows[FpSalaryReport.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 0].Text = "Total";
                        FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                        FpSalaryReport.Sheets[0].SpanModel.Add(FpSalaryReport.Sheets[0].RowCount - 1, 0, 1, 7);
                        for (int c = 7; c < FpSalaryReport.Sheets[0].ColumnCount - 2; c++)
                        {
                            Double setval = 0;
                            for (int ROW = 0; ROW < FpSalaryReport.Sheets[0].RowCount; ROW++)
                            {
                                string strval = FpSalaryReport.Sheets[0].Cells[ROW, c].Text.ToString();
                                if (strval.Trim() != "")
                                {
                                    setval = setval + Convert.ToDouble(strval);
                                }
                            }

                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].Text = setval.ToString();
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].Font.Bold = true;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                            FpSalaryReport.Sheets[0].Cells[FpSalaryReport.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Large;
                        }
                        lblexcel.Visible = true;
                        txtexcel.Visible = true;
                        txtexcel.Text = "";
                        btnexcel.Visible = true;
                        btnprintmaster.Visible = true;
                    }
                    else
                    {
                        clear();
                        errmsg.Visible = true;
                        errmsg.Text = "No Records Found";
                    }
                }
            }
            FpSalaryReport.Sheets[0].PageSize = FpSalaryReport.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblgenerror.Text = ex.ToString();
            lblgenerror.Visible = true;
        }
    }

    public void staffpayprocessprint()
    {
        try
        {
            int noofmonth = 0;
            FpMonthOverall.SaveChanges();
            if (txtallowance.Text.Trim() == "--Select--" && chklsallowance.Items.Count != 0)
            {
                lblgenerror.Text = "Please Select Any one Allowance!";
                lblgenerror.Visible = true;
                return;
            }
            if (txtdeduction.Text.Trim() == "--Select--" && chklsdeduction.Items.Count != 0)
            {
                lblgenerror.Text = "Please Select Any one Deduction!";
                lblgenerror.Visible = true;
                return;
            }
            for (int i = 1; i < FpMonthOverall.Sheets[0].RowCount; i++)
            {
                int isval = Convert.ToInt32(FpMonthOverall.Sheets[0].Cells[i, 1].Value);
                if (isval == 1)
                {
                    noofmonth++;
                }
            }

            if (noofmonth == 0)
            {
                lblgenerror.Text = "Please Select The Staff And Then Proceed";
                lblgenerror.Visible = true;
                return;
            }

            string fromyear = ddlfyear.SelectedValue.ToString();
            string frommonth = ddlfmonth.SelectedValue.ToString();
            string toyear = ddltyear.SelectedValue.ToString();
            string tomonth = ddltmonth.SelectedValue.ToString();

            if (frommonth.Trim() == "0")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The From Month And Then Proceed";
                return;
            }

            if (tomonth.Trim() == "0")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The TO Month And Then Proceed";
                return;
            }

            int fromyearval = (Convert.ToInt32(fromyear) * 12) + Convert.ToInt32(frommonth);
            int toyearval = (Convert.ToInt32(toyear) * 12) + Convert.ToInt32(tomonth);
            if (fromyearval > toyearval)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The To Month And Year Must Be Equal To Greater Than From Month And Year";
                return;
            }
            int toalnoofrows = toyearval - fromyearval;
            toalnoofrows++;

            string deptcode = "";
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        deptcode = deptcode + ",'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (deptcode.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Department And Then Proceed";
                return;
            }

            string design = "";
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                if (chklsdesign.Items[i].Selected == true)
                {
                    if (design == "")
                    {
                        design = "'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        design = design + ",'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (design.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Designation And Then Proceed";
                return;
            }

            string cateory = "";
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    if (cateory == "")
                    {
                        cateory = "'" + chklscategory.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        cateory = cateory + ",'" + chklscategory.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (cateory.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Category And Then Proceed";
                return;
            }


            Font Fontbold1 = new Font("Book Antiqua", 20, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 14, FontStyle.Bold);
            Font Fontbold3 = new Font("Book Antiqua", 14, FontStyle.Regular);
            Font Fontsmall = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 11, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(60, 40));

            string collname = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string pincode = "";
            string address = "";

            string str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            str = str + " ;select * from incentives_master where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet ds = d2.select_method_wo_parameter(str, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                collname = ds.Tables[0].Rows[0]["collname"].ToString();
                address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                pincode = ds.Tables[0].Rows[0]["pincode"].ToString();

                if (address1.Trim() != "" && address1 != null)
                {
                    address = address1;
                }
                if (address2.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address2;
                    }
                    else
                    {
                        address = address2;
                    }
                }
                if (address3.Trim() != "" && address1 != null)
                {
                    if (address != "")
                    {
                        address = address + ',' + address3;
                    }
                    else
                    {
                        address = address3;
                    }
                }
                if (pincode.Trim() != "" && pincode != null)
                {
                    if (address != "")
                    {
                        address = address + '-' + pincode;
                    }
                    else
                    {
                        address = pincode;
                    }
                }
            }
            string allowmaster = "";
            string deductmaster = "";
            string binval = "";
            int grosscolumn = 0;
            int totdeductiocolumn = 0;
            string IsConsolid = "";

            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            string[] spdet = deductmaster.Split(';');
            for (int d = 0; d <= spdet.GetUpperBound(0); d++)
            {
                string[] spdedet = spdet[d].Split('\\');
                if (spdedet.GetUpperBound(0) >= 1)
                {
                    string val = spdedet[0];
                    string val1 = spdedet[1];
                    dict.Add(val, val1);
                }
            }
            Gios.Pdf.PdfTablePage tblpage;
            Gios.Pdf.PdfTable tblpayprocess;
            Dictionary<int, Double> dicyeartotal = new Dictionary<int, double>();
            Double getvalamount = 0;
            int noofallowan = 0;
            int noofdecction = 0;
            string pfnumber = "";

            string strhryearquery = "select PayMonthNum,PayYear,From_Date,To_Date,year(from_date) fyear,year(to_date) tyear from HrPayMonths where College_Code='" + collegecode + "'";
            DataSet dshryear = d2.select_method_wo_parameter(strhryearquery, "Text");

            string strquery = "select s.staff_name,s.staff_code,s.pangirnumber,h.dept_name,d.desig_name,st.stftype,h.dept_code,d.desig_code,m.fdate,m.tdate,st.category_code,s.pfnumber,s.bankaccount,m.basic_alone,m.grade_pay,m.pay_band,m.allowances,m.deductions,m.netded,m.PayMonth,m.PayYear,st.allowances actall,st.deductions actdeduct,m.lop,m.netded,m.netsal,st.IsConsolid from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d ";
            strquery = strquery + " where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.latestrec = 1 and st.dept_code in(" + deptcode + ") and st.desig_code in(" + design + ") and st.category_code in(" + cateory + ")";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            int coltop = 0;
            for (int i = 1; i < FpMonthOverall.Sheets[0].RowCount; i++)
            {
                int isval = Convert.ToInt32(FpMonthOverall.Sheets[0].Cells[i, 1].Value);
                if (isval == 1)
                {
                    dicyeartotal.Clear();
                    coltop = 0;
                    string staffcode = FpMonthOverall.Sheets[0].Cells[i, 2].Text.ToString();
                    string staffname = FpMonthOverall.Sheets[0].Cells[i, 3].Text.ToString();
                    string department = FpMonthOverall.Sheets[0].Cells[i, 4].Text.ToString();

                    Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 25, 25, 400);
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 1600, 25, 400);
                    }

                    coltop = coltop + 10;
                    PdfTextArea ptacoll = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                    coltop = coltop + 30;
                    PdfTextArea ptarep = new PdfTextArea(Fontbold3, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "Annual Salary Report");


                    coltop = coltop + 60;
                    PdfTextArea ptascode = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 50, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Staff  Code");


                    PdfTextArea ptascodeval = new PdfTextArea(Fontbold3, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 140, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, ": " + staffcode);



                    coltop = coltop + 20;
                    PdfTextArea ptasname = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 50, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Staff  Name");


                    PdfTextArea ptasnameval = new PdfTextArea(Fontbold3, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 140, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, ": " + staffname);


                    //PdfTextArea ptafyaer = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                    //                                                   new PdfArea(mydocument, 600, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Financial Year");



                    //PdfTextArea ptafyaerval = new PdfTextArea(Fontbold3, System.Drawing.Color.Black,
                    //                                                   new PdfArea(mydocument, 700, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, ": " + staffcode);

                    coltop = coltop + 20;
                    PdfTextArea ptadep = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 50, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Department");

                    PdfTextArea ptadepval = new PdfTextArea(Fontbold3, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 140, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, ": " + department);

                    PdfTextArea ptadeppan = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                 new PdfArea(mydocument, 1400, coltop, 250, 50), System.Drawing.ContentAlignment.MiddleLeft, "PAN No");

                    ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                    DataView dvstaffpf = ds.Tables[0].DefaultView;
                    if (dvstaffpf.Count > 0)
                    {
                        pfnumber = dvstaffpf[0]["pangirnumber"].ToString();
                    }
                    PdfTextArea ptadeppanval = new PdfTextArea(Fontbold3, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 1460, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, ": " + pfnumber);


                    mypdfpage.Add(ptacoll);
                    mypdfpage.Add(ptascode);
                    mypdfpage.Add(ptascodeval);
                    mypdfpage.Add(ptasname);
                    mypdfpage.Add(ptasnameval);
                    //  mypdfpage.Add(ptafyaer);
                    mypdfpage.Add(ptadep);
                    mypdfpage.Add(ptadepval);
                    mypdfpage.Add(ptadeppan);
                    mypdfpage.Add(ptadeppanval);
                    mypdfpage.Add(ptarep);

                    ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                    DataView dvstaff = ds.Tables[0].DefaultView;

                    if (dvstaff.Count > 0)
                    {
                        allowmaster = dvstaff[0]["actall"].ToString();
                        string[] actallo = allowmaster.Split('\\');
                        int noofcolumn = 9;

                        //for (int a = 0; a < actallo.GetUpperBound(0); a++)
                        //{
                        //    string[] stva = actallo[a].Split(';');
                        //    if (stva[0].Trim() != "")
                        //    {
                        for (int ik = 0; ik < chklsallowance.Items.Count; ik++)
                        {
                            if (chklsallowance.Items[ik].Selected == true)
                            {
                                //if (chklsallowance.Items[ik].Text.Trim() == stva[0].Trim())
                                //{
                                noofcolumn++;
                                //}
                            }
                        }
                        //    }
                        //}
                        IsConsolid = dvstaff[0]["IsConsolid"].ToString();
                        deductmaster = dvstaff[0]["actdeduct"].ToString();
                        actallo = deductmaster.Split('\\');
                        //for (int a = 0; a < actallo.GetUpperBound(0); a++)
                        //{
                        //    string[] stva = actallo[a].Split(';');
                        //    if (stva[0].Trim() != "")
                        //    {
                        for (int ik = 0; ik < chklsdeduction.Items.Count; ik++)
                        {
                            if (chklsdeduction.Items[ik].Selected == true)
                            {
                                //if (chklsdeduction.Items[ik].Text.Trim() == stva[0].Trim())
                                //{
                                noofcolumn++;
                                //}
                            }
                        }
                        //    }
                        //}

                        int noorwo = toalnoofrows - 0;
                        if (noorwo > 12)
                        {
                            noorwo = 12;
                        }
                        noorwo = noorwo + 3;
                        tblpayprocess = mydocument.NewTable(Fontsmall, noorwo, noofcolumn, 2);
                        tblpayprocess.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        tblpayprocess.VisibleHeaders = false;

                        tblpayprocess.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(0, 0).SetContent("S.No");
                        tblpayprocess.Cell(0, 0).SetFont(Fontsmall1);

                        foreach (PdfCell pc in tblpayprocess.CellRange(0, 0, 0, 0).Cells)
                        {
                            pc.RowSpan = 2;
                        }

                        tblpayprocess.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(0, 1).SetContent("Year - Month");
                        tblpayprocess.Cell(0, 1).SetFont(Fontsmall1);

                        foreach (PdfCell pc in tblpayprocess.CellRange(0, 1, 0, 1).Cells)
                        {
                            pc.RowSpan = 2;
                        }

                        tblpayprocess.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(0, 2).SetContent("Actual Basic");
                        tblpayprocess.Cell(0, 2).SetFont(Fontsmall1);
                        dicyeartotal.Add(2, 0);
                        foreach (PdfCell pc in tblpayprocess.CellRange(0, 2, 0, 2).Cells)
                        {
                            pc.RowSpan = 2;
                        }
                        FpMonthOverall.Sheets[0].Columns[1].Visible = true;


                        tblpayprocess.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(0, 3).SetContent("Actual Grade Pay");
                        tblpayprocess.Cell(0, 3).SetFont(Fontsmall1);
                        dicyeartotal.Add(3, 0);
                        foreach (PdfCell pc in tblpayprocess.CellRange(0, 3, 0, 3).Cells)
                        {
                            pc.RowSpan = 2;
                        }

                        tblpayprocess.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(0, 4).SetContent("Actual Pay Band");
                        tblpayprocess.Cell(0, 4).SetFont(Fontsmall1);
                        dicyeartotal.Add(4, 0);
                        foreach (PdfCell pc in tblpayprocess.CellRange(0, 4, 0, 4).Cells)
                        {
                            pc.RowSpan = 2;
                        }
                        noofallowan = 0;
                        int col = 4;
                        actallo = allowmaster.Split('\\');
                        //for (int a = 0; a < actallo.GetUpperBound(0); a++)
                        //{
                        //    string[] stva = actallo[a].Split(';');
                        //    if (stva[0].Trim() != "")
                        //    {
                        for (int ik = 0; ik < chklsallowance.Items.Count; ik++)
                        {
                            if (chklsallowance.Items[ik].Selected == true)
                            {
                                //if (chklsallowance.Items[ik].Text.Trim() == stva[0].Trim())
                                //{
                                noofallowan++;
                                col++;
                                tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                //tblpayprocess.Cell(0, col).SetContent(stva[0].ToString());
                                tblpayprocess.Cell(0, col).SetContent(chklsallowance.Items[ik].Text.ToString());
                                tblpayprocess.Cell(0, col).SetFont(Fontsmall1);
                                dicyeartotal.Add(col, 0);
                                //}
                            }
                        }
                        //    }
                        //}

                        if (noofallowan > 0)
                        {
                            tblpayprocess.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tblpayprocess.Cell(0, 5).SetContent("Allowance");
                            tblpayprocess.Cell(0, 5).SetFont(Fontsmall1);

                            foreach (PdfCell pc in tblpayprocess.CellRange(0, 5, 0, 5).Cells)
                            {
                                pc.ColSpan = noofallowan;
                            }
                        }

                        col++;
                        grosscolumn = col;
                        tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(0, col).SetContent("Actual Gross Amount");
                        tblpayprocess.Cell(0, col).SetFont(Fontsmall1);
                        dicyeartotal.Add(col, 0);
                        foreach (PdfCell pc in tblpayprocess.CellRange(0, col, 0, col).Cells)
                        {
                            pc.RowSpan = 2;
                        }
                        noofdecction = 0;
                        actallo = deductmaster.Split('\\');
                        //for (int a = 0; a < actallo.GetUpperBound(0); a++)
                        //{
                        //    string[] stva = actallo[a].Split(';');
                        //    if (stva[0].Trim() != "")
                        //    {
                        for (int ik = 0; ik < chklsdeduction.Items.Count; ik++)
                        {
                            if (chklsdeduction.Items[ik].Selected == true)
                            {
                                //if (chklsdeduction.Items[ik].Text.Trim() == stva[0].Trim())
                                //{
                                noofdecction++;
                                col++;
                                tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                //tblpayprocess.Cell(0, col).SetContent(stva[0].ToString());
                                tblpayprocess.Cell(0, col).SetContent(chklsdeduction.Items[ik].Text.ToString());
                                tblpayprocess.Cell(0, col).SetFont(Fontsmall1);
                                dicyeartotal.Add(col, 0);
                                //}
                            }
                        }
                        //    }
                        //}
                        col++;
                        totdeductiocolumn = col;
                        tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(0, col).SetContent("Lop");
                        tblpayprocess.Cell(0, col).SetFont(Fontsmall1);
                        dicyeartotal.Add(col, 0);
                        foreach (PdfCell pc in tblpayprocess.CellRange(0, col, 0, col).Cells)
                        {
                            pc.RowSpan = 2;
                        }

                        col++;
                        tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(0, col).SetContent("Total Deduction");
                        tblpayprocess.Cell(0, col).SetFont(Fontsmall1);
                        dicyeartotal.Add(col, 0);
                        foreach (PdfCell pc in tblpayprocess.CellRange(0, col, 0, col).Cells)
                        {
                            pc.RowSpan = 2;
                        }

                        col++;
                        tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(0, col).SetContent("Net Amount");
                        tblpayprocess.Cell(0, col).SetFont(Fontsmall1);
                        dicyeartotal.Add(col, 0);
                        foreach (PdfCell pc in tblpayprocess.CellRange(0, col, 0, col).Cells)
                        {
                            pc.RowSpan = 2;
                        }

                        int row = 1;
                        int norec = 0;
                        for (int stryemont = fromyearval; stryemont <= toyearval; stryemont++)
                        {

                            if ((norec % 12) == 0)
                            {
                                row++;
                                tblpayprocess.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(row, 0).SetContent("Total");
                                tblpayprocess.Cell(row, 0).SetFont(Fontsmall1);

                                foreach (PdfCell pc in tblpayprocess.CellRange(row, 0, row, 0).Cells)
                                {
                                    pc.ColSpan = 2;
                                }

                                for (int c = 2; c < noofcolumn; c++)
                                {
                                    getvalamount = 0;
                                    if (dicyeartotal.ContainsKey(c))
                                    {
                                        getvalamount = dicyeartotal[c];
                                        dicyeartotal[c] = 0;
                                    }
                                    tblpayprocess.Cell(row, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, c).SetContent(getvalamount);
                                    tblpayprocess.Cell(row, c).SetFont(Fontsmall1);
                                }
                                row = 1;
                                if (norec > 11)
                                {
                                    coltop = 150;
                                    tblpage = tblpayprocess.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 5, coltop, 832, 1000));
                                    mypdfpage.Add(tblpage);
                                    mypdfpage.SaveToDocument();

                                    mypdfpage = mydocument.NewPage();
                                    mypdfpage.Add(ptacoll);
                                    mypdfpage.Add(ptascode);
                                    mypdfpage.Add(ptascodeval);
                                    mypdfpage.Add(ptasname);
                                    mypdfpage.Add(ptasnameval);
                                    //mypdfpage.Add(ptafyaer);
                                    mypdfpage.Add(ptadep);
                                    mypdfpage.Add(ptadepval);
                                    mypdfpage.Add(ptadeppan);
                                    mypdfpage.Add(ptadeppanval);
                                    mypdfpage.Add(ptarep);
                                }
                                noorwo = toalnoofrows - norec;
                                if (noorwo > 12)
                                {
                                    noorwo = 12;
                                }
                                noorwo = noorwo + 3;
                                tblpayprocess = mydocument.NewTable(Fontsmall, noorwo, noofcolumn, 2);
                                tblpayprocess.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                tblpayprocess.VisibleHeaders = false;

                                tblpayprocess.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(0, 0).SetContent("S.No");
                                tblpayprocess.Cell(0, 0).SetFont(Fontsmall1);
                                tblpayprocess.Columns[0].SetWidth(3);
                                foreach (PdfCell pc in tblpayprocess.CellRange(0, 0, 0, 0).Cells)
                                {
                                    pc.RowSpan = 2;
                                }

                                tblpayprocess.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(0, 1).SetContent("Year - Month");
                                tblpayprocess.Cell(0, 1).SetFont(Fontsmall1);
                                tblpayprocess.Columns[1].SetWidth(5);
                                foreach (PdfCell pc in tblpayprocess.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pc.RowSpan = 2;
                                }

                                tblpayprocess.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(0, 2).SetContent("Actual Basic");
                                tblpayprocess.Cell(0, 2).SetFont(Fontsmall1);
                                foreach (PdfCell pc in tblpayprocess.CellRange(0, 2, 0, 2).Cells)
                                {
                                    pc.RowSpan = 2;
                                }


                                tblpayprocess.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(0, 3).SetContent("Actual Grade Pay");
                                tblpayprocess.Cell(0, 3).SetFont(Fontsmall1);
                                foreach (PdfCell pc in tblpayprocess.CellRange(0, 3, 0, 3).Cells)
                                {
                                    pc.RowSpan = 2;
                                }

                                tblpayprocess.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(0, 4).SetContent("Actual Pay Band");
                                tblpayprocess.Cell(0, 4).SetFont(Fontsmall1);
                                foreach (PdfCell pc in tblpayprocess.CellRange(0, 4, 0, 4).Cells)
                                {
                                    pc.RowSpan = 2;
                                }
                                noofallowan = 0;
                                col = 4;
                                actallo = allowmaster.Split('\\');
                                //for (int a = 0; a < actallo.GetUpperBound(0); a++)
                                //{
                                //    string[] stva = actallo[a].Split(';');
                                //    if (stva[0].Trim() != "")
                                //    {
                                for (int ik = 0; ik < chklsallowance.Items.Count; ik++)
                                {
                                    if (chklsallowance.Items[ik].Selected == true)
                                    {
                                        //if (chklsallowance.Items[ik].Text.Trim() == stva[0].Trim())
                                        //{
                                        noofallowan++;
                                        col++;
                                        tblpayprocess.Cell(1, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //tblpayprocess.Cell(1, col).SetContent(stva[0].ToString());
                                        tblpayprocess.Cell(1, col).SetContent(chklsallowance.Items[ik].Text.ToString());
                                        tblpayprocess.Cell(1, col).SetFont(Fontsmall1);
                                        //}
                                    }
                                }
                                //    }
                                //}

                                if (noofallowan > 0)
                                {
                                    tblpayprocess.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(0, 5).SetContent("Allowance");
                                    tblpayprocess.Cell(0, 5).SetFont(Fontsmall1);

                                    foreach (PdfCell pc in tblpayprocess.CellRange(0, 5, 0, 5).Cells)
                                    {
                                        pc.ColSpan = noofallowan;
                                    }
                                }

                                col++;
                                tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(0, col).SetContent("Actual Gross Amount");
                                tblpayprocess.Cell(0, col).SetFont(Fontsmall1);

                                foreach (PdfCell pc in tblpayprocess.CellRange(0, col, 0, col).Cells)
                                {
                                    pc.RowSpan = 2;
                                }

                                noofdecction = 0;
                                actallo = deductmaster.Split('\\');
                                //for (int a = 0; a < actallo.GetUpperBound(0); a++)
                                //{
                                //    string[] stva = actallo[a].Split(';');
                                //    if (stva[0].Trim() != "")
                                //    {
                                for (int ik = 0; ik < chklsdeduction.Items.Count; ik++)
                                {
                                    if (chklsdeduction.Items[ik].Selected == true)
                                    {
                                        //if (chklsdeduction.Items[ik].Text.Trim() == stva[0].Trim())
                                        //{
                                        noofdecction++;
                                        col++;
                                        tblpayprocess.Cell(1, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //tblpayprocess.Cell(1, col).SetContent(stva[0].ToString());
                                        tblpayprocess.Cell(1, col).SetContent(chklsdeduction.Items[ik].Text.ToString());
                                        tblpayprocess.Cell(1, col).SetFont(Fontsmall1);
                                        //}
                                    }
                                }
                                //    }
                                //}

                                if (noofdecction > 0)
                                {
                                    int deducto = col - noofdecction;
                                    deducto++;
                                    tblpayprocess.Cell(0, deducto).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(0, deducto).SetContent("Deductions");
                                    tblpayprocess.Cell(0, deducto).SetFont(Fontsmall1);

                                    foreach (PdfCell pc in tblpayprocess.CellRange(0, deducto, 0, deducto).Cells)
                                    {
                                        pc.ColSpan = noofdecction;
                                    }
                                }

                                col++;
                                tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(0, col).SetContent("Lop");
                                tblpayprocess.Cell(0, col).SetFont(Fontsmall1);
                                foreach (PdfCell pc in tblpayprocess.CellRange(0, col, 0, col).Cells)
                                {
                                    pc.RowSpan = 2;
                                }

                                col++;
                                tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(0, col).SetContent("Total Deduction");
                                tblpayprocess.Cell(0, col).SetFont(Fontsmall1);
                                foreach (PdfCell pc in tblpayprocess.CellRange(0, col, 0, col).Cells)
                                {
                                    pc.RowSpan = 2;
                                }

                                col++;
                                tblpayprocess.Cell(0, col).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tblpayprocess.Cell(0, col).SetContent("Net Amount");
                                tblpayprocess.Cell(0, col).SetFont(Fontsmall1);
                                foreach (PdfCell pc in tblpayprocess.CellRange(0, col, 0, col).Cells)
                                {
                                    pc.RowSpan = 2;
                                }
                            }
                            norec++;

                            int montext = stryemont % 12;
                            if (montext == 0)
                            {
                                montext = 12;
                            }
                            string monthname = "";
                            switch (montext)
                            {
                                case 1:
                                    monthname = "January";
                                    break;
                                case 2:
                                    monthname = "February";
                                    break;
                                case 3:
                                    monthname = "March";
                                    break;
                                case 4:
                                    monthname = "April";
                                    break;
                                case 5:
                                    monthname = "May";
                                    break;
                                case 6:
                                    monthname = "June";
                                    break;
                                case 7:
                                    monthname = "July";
                                    break;
                                case 8:
                                    monthname = "August";
                                    break;
                                case 9:
                                    monthname = "September";
                                    break;
                                case 10:
                                    monthname = "October";
                                    break;
                                case 11:
                                    monthname = "November";
                                    break;
                                case 12:
                                    monthname = "December";
                                    break;
                            }
                            int yeartext = stryemont / 12;
                            if (montext == 12)
                            {
                                yeartext--;
                            }
                            row++;

                            tblpayprocess.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tblpayprocess.Cell(row, 0).SetContent(norec);
                            tblpayprocess.Cell(row, 0).SetFont(Fontsmall);

                            tblpayprocess.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tblpayprocess.Cell(row, 1).SetContent(yeartext + " - " + monthname);
                            tblpayprocess.Cell(row, 1).SetFont(Fontsmall);

                            dshryear.Tables[0].DefaultView.RowFilter = "PayMonthNum='" + montext + "' and fyear='" + yeartext + "'";
                            DataView dvhryear = dshryear.Tables[0].DefaultView;
                            if (dvhryear.Count > 0)
                            {
                                string fdate = dvhryear[0]["From_Date"].ToString();
                                string tdate = dvhryear[0]["To_Date"].ToString();

                                ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "' and fdate='" + fdate + "'";
                                DataView dvhddetails = ds.Tables[0].DefaultView;
                                if (dvhddetails.Count > 0)
                                {
                                    binval = dvhddetails[0]["basic_alone"].ToString();
                                    Double basicpay = 0;
                                    if (binval.Trim() != "")
                                    {
                                        basicpay = Convert.ToDouble(binval);
                                    }
                                    basicpay = Math.Round(basicpay, 0, MidpointRounding.AwayFromZero);

                                    if (dicyeartotal.ContainsKey(2))
                                    {
                                        getvalamount = dicyeartotal[2] + basicpay;
                                        dicyeartotal[2] = getvalamount;
                                    }

                                    tblpayprocess.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, 2).SetContent(basicpay);
                                    tblpayprocess.Cell(row, 2).SetFont(Fontsmall);

                                    binval = dvhddetails[0]["grade_pay"].ToString();
                                    Double gradpay = 0;
                                    if (binval.Trim() != "")
                                    {
                                        gradpay = Convert.ToDouble(binval);
                                    }
                                    gradpay = Math.Round(gradpay, 0, MidpointRounding.AwayFromZero);
                                    if (dicyeartotal.ContainsKey(3))
                                    {
                                        getvalamount = dicyeartotal[3] + gradpay;
                                        dicyeartotal[3] = getvalamount;
                                    }

                                    tblpayprocess.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, 3).SetContent(gradpay);
                                    tblpayprocess.Cell(row, 3).SetFont(Fontsmall);

                                    binval = dvhddetails[0]["pay_band"].ToString();
                                    Double payband = 0;
                                    if (binval.Trim() != "")
                                    {
                                        payband = Convert.ToDouble(binval);
                                    }
                                    payband = Math.Round(payband, 0, MidpointRounding.AwayFromZero);

                                    if (dicyeartotal.ContainsKey(4))
                                    {
                                        getvalamount = dicyeartotal[4] + payband;
                                        dicyeartotal[4] = getvalamount;
                                    }

                                    tblpayprocess.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, 4).SetContent(payband);
                                    tblpayprocess.Cell(row, 4).SetFont(Fontsmall);

                                    col = 4;
                                    Double allototal = 0;
                                    string[] spvd = dvhddetails[0]["allowances"].ToString().Split('\\');
                                    for (int sp = 0; sp <= spvd.GetUpperBound(0); sp++)
                                    {
                                        string[] spdvalo = spvd[sp].Split(';');
                                        if (spdvalo.GetUpperBound(0) >= 1)
                                        {
                                            for (int c = 5; c <= grosscolumn; c++)
                                            {
                                                string strhval = Convert.ToString(tblpayprocess.Cell(1, c).Content);
                                                if (spdvalo[0].ToString().Trim().ToLower() == strhval.Trim().ToLower())
                                                {
                                                    binval = spdvalo[2].ToString();
                                                    Double allow = 0;
                                                    string[] get = binval.Split('-');
                                                    if (spdvalo[1].Trim() == "Amount")
                                                    {
                                                        binval = get[0];
                                                    }
                                                    else if (spdvalo[1].Trim() == "Percent" || spdvalo[1].Trim() == "Slab")
                                                    {
                                                        if (get.Length == 2)
                                                        {
                                                            binval = get[1];
                                                        }
                                                    }
                                                    if (binval.Trim() != "")
                                                    {
                                                        allow = Convert.ToDouble(binval);
                                                    }
                                                    allow = Math.Round(allow, 0, MidpointRounding.AwayFromZero);
                                                    if (dicyeartotal.ContainsKey(c))
                                                    {
                                                        getvalamount = dicyeartotal[c] + allow;
                                                        dicyeartotal[c] = getvalamount;
                                                    }

                                                    allototal = allototal + allow;
                                                    tblpayprocess.Cell(row, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tblpayprocess.Cell(row, c).SetContent(allow);
                                                    tblpayprocess.Cell(row, c).SetFont(Fontsmall);
                                                    c = grosscolumn + 2;
                                                }
                                            }
                                        }
                                    }
                                    Double grossamount = allototal + basicpay + gradpay + payband;
                                    if (IsConsolid != "1")
                                    {
                                        grossamount = allototal + basicpay;
                                    }
                                    if (dicyeartotal.ContainsKey(grosscolumn))
                                    {
                                        getvalamount = dicyeartotal[grosscolumn] + grossamount;
                                        dicyeartotal[grosscolumn] = getvalamount;
                                    }
                                    tblpayprocess.Cell(row, grosscolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, grosscolumn).SetContent(grossamount);
                                    tblpayprocess.Cell(row, grosscolumn).SetFont(Fontsmall);

                                    spvd = dvhddetails[0]["deductions"].ToString().Split('\\');
                                    for (int sp = 0; sp <= spvd.GetUpperBound(0); sp++)
                                    {
                                        string[] spdvalo = spvd[sp].Split(';');
                                        if (spdvalo.GetUpperBound(0) >= 1)
                                        {
                                            for (int c = grosscolumn; c <= totdeductiocolumn; c++)
                                            {
                                                string strhval = Convert.ToString(tblpayprocess.Cell(1, c).Content);
                                                if (spdvalo[0].ToString().Trim().ToLower() == strhval.Trim().ToLower())
                                                {
                                                    binval = spdvalo[3].ToString();
                                                    string[] get = binval.Split('-');
                                                    if (spdvalo[1].Trim() == "Amount")
                                                    {
                                                        binval = get[0];
                                                    }
                                                    else if (spdvalo[1].Trim() == "Percent" || spdvalo[1].Trim() == "Slab")
                                                    {
                                                        if (get.Length == 2)
                                                        {
                                                            binval = get[1];
                                                        }
                                                    }
                                                    // binval = get[0];
                                                    Double allow = 0;
                                                    if (binval.Trim() != "")
                                                    {
                                                        allow = Convert.ToDouble(binval);
                                                    }
                                                    allow = Math.Round(allow, 0, MidpointRounding.AwayFromZero);
                                                    if (dicyeartotal.ContainsKey(c))
                                                    {
                                                        getvalamount = dicyeartotal[c] + allow;
                                                        dicyeartotal[c] = getvalamount;
                                                    }
                                                    tblpayprocess.Cell(row, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tblpayprocess.Cell(row, c).SetContent(allow);
                                                    tblpayprocess.Cell(row, c).SetFont(Fontsmall);
                                                    c = totdeductiocolumn + 2;
                                                }
                                            }
                                        }
                                    }

                                    binval = dvhddetails[0]["lop"].ToString();
                                    Double lop = 0;
                                    if (binval.Trim() != "")
                                    {
                                        lop = Convert.ToDouble(binval);
                                    }
                                    lop = Math.Round(lop, 0, MidpointRounding.AwayFromZero);
                                    if (dicyeartotal.ContainsKey(totdeductiocolumn))
                                    {
                                        getvalamount = dicyeartotal[totdeductiocolumn] + lop;
                                        dicyeartotal[totdeductiocolumn] = getvalamount;
                                    }

                                    tblpayprocess.Cell(row, totdeductiocolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, totdeductiocolumn).SetContent(lop);
                                    tblpayprocess.Cell(row, totdeductiocolumn).SetFont(Fontsmall);

                                    binval = dvhddetails[0]["netded"].ToString();
                                    Double totdeuct = 0;
                                    if (binval.Trim() != "")
                                    {
                                        totdeuct = Convert.ToDouble(binval) + Convert.ToDouble(lop);
                                    }
                                    totdeuct = Math.Round(totdeuct, 0, MidpointRounding.AwayFromZero);

                                    if (dicyeartotal.ContainsKey(totdeductiocolumn + 1))
                                    {
                                        getvalamount = dicyeartotal[totdeductiocolumn + 1] + totdeuct;
                                        dicyeartotal[totdeductiocolumn + 1] = getvalamount;
                                    }
                                    tblpayprocess.Cell(row, totdeductiocolumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, totdeductiocolumn + 1).SetContent(totdeuct);
                                    tblpayprocess.Cell(row, totdeductiocolumn + 1).SetFont(Fontsmall);

                                    Double netsal = grossamount - totdeuct;
                                    netsal = Math.Round(netsal, 0, MidpointRounding.AwayFromZero);
                                    if (dicyeartotal.ContainsKey(totdeductiocolumn + 2))
                                    {
                                        getvalamount = dicyeartotal[totdeductiocolumn + 2] + netsal;
                                        dicyeartotal[totdeductiocolumn + 2] = getvalamount;
                                    }
                                    tblpayprocess.Cell(row, totdeductiocolumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, totdeductiocolumn + 2).SetContent(netsal);
                                    tblpayprocess.Cell(row, totdeductiocolumn + 2).SetFont(Fontsmall);
                                }
                            }
                            else
                            {
                                ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "' and PayMonth='" + montext + "' and PayYear='" + yeartext + "'";
                                DataView dvhddetails = ds.Tables[0].DefaultView;
                                if (dvhddetails.Count > 0)
                                {
                                    binval = dvhddetails[0]["basic_alone"].ToString();
                                    Double basicpay = 0;
                                    if (binval.Trim() != "")
                                    {
                                        basicpay = Convert.ToDouble(binval);
                                    }
                                    basicpay = Math.Round(basicpay, 0, MidpointRounding.AwayFromZero);

                                    if (dicyeartotal.ContainsKey(2))
                                    {
                                        getvalamount = dicyeartotal[2] + basicpay;
                                        dicyeartotal[2] = getvalamount;
                                    }

                                    tblpayprocess.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, 2).SetContent(basicpay);
                                    tblpayprocess.Cell(row, 2).SetFont(Fontsmall);

                                    binval = dvhddetails[0]["grade_pay"].ToString();
                                    Double gradpay = 0;
                                    if (binval.Trim() != "")
                                    {
                                        gradpay = Convert.ToDouble(binval);
                                    }
                                    gradpay = Math.Round(gradpay, 0, MidpointRounding.AwayFromZero);
                                    if (dicyeartotal.ContainsKey(3))
                                    {
                                        getvalamount = dicyeartotal[3] + gradpay;
                                        dicyeartotal[3] = getvalamount;
                                    }

                                    tblpayprocess.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, 3).SetContent(gradpay);
                                    tblpayprocess.Cell(row, 3).SetFont(Fontsmall);

                                    binval = dvhddetails[0]["pay_band"].ToString();
                                    Double payband = 0;
                                    if (binval.Trim() != "")
                                    {
                                        payband = Convert.ToDouble(binval);
                                    }
                                    payband = Math.Round(payband, 0, MidpointRounding.AwayFromZero);

                                    if (dicyeartotal.ContainsKey(4))
                                    {
                                        getvalamount = dicyeartotal[4] + payband;
                                        dicyeartotal[4] = getvalamount;
                                    }

                                    tblpayprocess.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, 4).SetContent(payband);
                                    tblpayprocess.Cell(row, 4).SetFont(Fontsmall);

                                    col = 4;
                                    Double allototal = 0;
                                    string[] spvd = dvhddetails[0]["allowances"].ToString().Split('\\');
                                    for (int sp = 0; sp <= spvd.GetUpperBound(0); sp++)
                                    {
                                        string[] spdvalo = spvd[sp].Split(';');
                                        if (spdvalo.GetUpperBound(0) >= 1)
                                        {
                                            for (int c = 5; c <= grosscolumn; c++)
                                            {
                                                string strhval = Convert.ToString(tblpayprocess.Cell(1, c).Content);
                                                if (spdvalo[0].ToString().Trim().ToLower() == strhval.Trim().ToLower())
                                                {
                                                    binval = spdvalo[2].ToString();
                                                    Double allow = 0;
                                                    string[] get = binval.Split('-');
                                                    if (spdvalo[1].Trim() == "Amount")
                                                    {
                                                        binval = get[0];
                                                    }
                                                    else if (spdvalo[1].Trim() == "Percent" || spdvalo[1].Trim() == "Slab")
                                                    {
                                                        if (get.Length == 2)
                                                        {
                                                            binval = get[1];
                                                        }
                                                    }
                                                    //binval = get[0];
                                                    if (binval.Trim() != "")
                                                    {
                                                        allow = Convert.ToDouble(binval);
                                                    }
                                                    allow = Math.Round(allow, 0, MidpointRounding.AwayFromZero);
                                                    if (dicyeartotal.ContainsKey(c))
                                                    {
                                                        getvalamount = dicyeartotal[c] + allow;
                                                        dicyeartotal[c] = getvalamount;
                                                    }

                                                    allototal = allototal + allow;
                                                    tblpayprocess.Cell(row, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tblpayprocess.Cell(row, c).SetContent(allow);
                                                    tblpayprocess.Cell(row, c).SetFont(Fontsmall);
                                                    c = grosscolumn + 2;
                                                }
                                            }
                                        }
                                    }
                                    Double grossamount = allototal + basicpay + gradpay + payband;
                                    if (IsConsolid != "1")
                                    {
                                        grossamount = allototal + basicpay;
                                    }
                                    if (dicyeartotal.ContainsKey(grosscolumn))
                                    {
                                        getvalamount = dicyeartotal[grosscolumn] + grossamount;
                                        dicyeartotal[grosscolumn] = getvalamount;
                                    }
                                    tblpayprocess.Cell(row, grosscolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, grosscolumn).SetContent(grossamount);
                                    tblpayprocess.Cell(row, grosscolumn).SetFont(Fontsmall);

                                    spvd = dvhddetails[0]["deductions"].ToString().Split('\\');
                                    for (int sp = 0; sp <= spvd.GetUpperBound(0); sp++)
                                    {
                                        string[] spdvalo = spvd[sp].Split(';');
                                        if (spdvalo.GetUpperBound(0) >= 1)
                                        {
                                            for (int c = grosscolumn; c <= totdeductiocolumn; c++)
                                            {
                                                string strhval = Convert.ToString(tblpayprocess.Cell(1, c).Content);
                                                if (spdvalo[0].ToString().Trim().ToLower() == strhval.Trim().ToLower())
                                                {
                                                    binval = spdvalo[3].ToString();
                                                    string[] get = binval.Split('-');
                                                    if (spdvalo[1].Trim() == "Amount")
                                                    {
                                                        binval = get[0];
                                                    }
                                                    else if (spdvalo[1].Trim() == "Percent" || spdvalo[1].Trim() == "Slab")
                                                    {
                                                        if (get.Length == 2)
                                                        {
                                                            binval = get[1];
                                                        }
                                                    }
                                                    //binval = get[0];
                                                    Double allow = 0;
                                                    if (binval.Trim() != "")
                                                    {
                                                        allow = Convert.ToDouble(binval);
                                                    }
                                                    allow = Math.Round(allow, 0, MidpointRounding.AwayFromZero);
                                                    if (dicyeartotal.ContainsKey(c))
                                                    {
                                                        getvalamount = dicyeartotal[c] + allow;
                                                        dicyeartotal[c] = getvalamount;
                                                    }
                                                    tblpayprocess.Cell(row, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tblpayprocess.Cell(row, c).SetContent(allow);
                                                    tblpayprocess.Cell(row, c).SetFont(Fontsmall);
                                                    c = totdeductiocolumn + 2;
                                                }
                                            }
                                        }
                                    }

                                    binval = dvhddetails[0]["lop"].ToString();
                                    Double lop = 0;
                                    if (binval.Trim() != "")
                                    {
                                        lop = Convert.ToDouble(binval);
                                    }
                                    lop = Math.Round(lop, 0, MidpointRounding.AwayFromZero);
                                    if (dicyeartotal.ContainsKey(totdeductiocolumn))
                                    {
                                        getvalamount = dicyeartotal[totdeductiocolumn] + lop;
                                        dicyeartotal[totdeductiocolumn] = getvalamount;
                                    }

                                    tblpayprocess.Cell(row, totdeductiocolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, totdeductiocolumn).SetContent(lop);
                                    tblpayprocess.Cell(row, totdeductiocolumn).SetFont(Fontsmall);

                                    binval = dvhddetails[0]["netded"].ToString();
                                    Double totdeuct = 0;
                                    if (binval.Trim() != "")
                                    {
                                        totdeuct = Convert.ToDouble(binval) + Convert.ToDouble(lop);
                                    }
                                    totdeuct = Math.Round(totdeuct, 0, MidpointRounding.AwayFromZero);

                                    if (dicyeartotal.ContainsKey(totdeductiocolumn + 1))
                                    {
                                        getvalamount = dicyeartotal[totdeductiocolumn + 1] + totdeuct;
                                        dicyeartotal[totdeductiocolumn + 1] = getvalamount;
                                    }
                                    tblpayprocess.Cell(row, totdeductiocolumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, totdeductiocolumn + 1).SetContent(totdeuct);
                                    tblpayprocess.Cell(row, totdeductiocolumn + 1).SetFont(Fontsmall);

                                    Double netsal = grossamount - totdeuct;
                                    netsal = Math.Round(netsal, 0, MidpointRounding.AwayFromZero);
                                    if (dicyeartotal.ContainsKey(totdeductiocolumn + 2))
                                    {
                                        getvalamount = dicyeartotal[totdeductiocolumn + 2] + netsal;
                                        dicyeartotal[totdeductiocolumn + 2] = getvalamount;
                                    }
                                    tblpayprocess.Cell(row, totdeductiocolumn + 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tblpayprocess.Cell(row, totdeductiocolumn + 2).SetContent(netsal);
                                    tblpayprocess.Cell(row, totdeductiocolumn + 2).SetFont(Fontsmall);
                                }
                            }
                        }
                        row++;
                        tblpayprocess.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tblpayprocess.Cell(row, 0).SetContent("Total");
                        tblpayprocess.Cell(row, 0).SetFont(Fontsmall1);

                        foreach (PdfCell pc in tblpayprocess.CellRange(row, 0, row, 0).Cells)
                        {
                            pc.ColSpan = 2;
                        }

                        for (int c = 2; c < noofcolumn; c++)
                        {
                            getvalamount = 0;
                            if (dicyeartotal.ContainsKey(c))
                            {
                                getvalamount = dicyeartotal[c];
                                dicyeartotal[c] = 0;
                            }
                            tblpayprocess.Cell(row, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tblpayprocess.Cell(row, c).SetContent(getvalamount);
                            tblpayprocess.Cell(row, c).SetFont(Fontsmall1);
                        }
                        coltop += 50;
                        tblpage = tblpayprocess.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 25, coltop, mydocument.PageWidth - 50, 1200));
                        mypdfpage.Add(tblpage);
                    }
                    mypdfpage.SaveToDocument();
                }
            }

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = DateTime.Now.ToString("ddMMyyyyhhmmsstt") + "PaySlip.pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            lblgenerror.Text = ex.ToString();
            lblgenerror.Visible = true;
        }
    }

    protected void FpSalaryReport_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string ar = e.CommandArgument.ToString();

            string[] spitval = ar.Split(',');
            string[] spitrow = spitval[0].Split('=');
            string actrow = spitrow[1].ToString();
            string[] spiticol = spitval[1].Split('=');
            string[] spitvn = spiticol[1].Split('}');
            string actcol = spitvn[0].ToString();
            if (flag_true == false && actrow == "0")
            {
                int s = Convert.ToInt16(FpSalaryReport.Sheets[0].Cells[0, 1].Value);

                for (int j = 1; j < Convert.ToInt16(FpSalaryReport.Sheets[0].RowCount - 1); j++)
                {
                    FpSalaryReport.Sheets[0].Cells[j, 1].Value = s;
                }
                flag_true = true;
            }
        }
        catch (Exception ex)
        {
            lblgenerror.Text = ex.ToString();
            lblgenerror.Visible = true;
        }
    }

    protected void FpMonthOverall_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            if (rbreport.Checked == true)
            {
                string ar = e.CommandArgument.ToString();

                string[] spitval = ar.Split(',');
                string[] spitrow = spitval[0].Split('=');
                string actrow = spitrow[1].ToString();
                string[] spiticol = spitval[1].Split('=');
                string[] spitvn = spiticol[1].Split('}');
                string actcol = spitvn[0].ToString();
                if (flag_true == false && actrow == "0")
                {
                    int s = Convert.ToInt16(FpMonthOverall.Sheets[0].Cells[0, 1].Value);

                    for (int j = 1; j < Convert.ToInt16(FpMonthOverall.Sheets[0].RowCount); j++)
                    {
                        FpMonthOverall.Sheets[0].Cells[j, 1].Value = s;
                    }
                    flag_true = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblgenerror.Text = ex.ToString();
            lblgenerror.Visible = true;
        }
    }

    protected void rbreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblstaff.Visible = false;
            txtstaff.Visible = false;
            pstaff.Visible = false;
            clear();
            //if (rbconsolidate.Checked == true)
            //{
            //    lblstaff.Visible = false;
            //    txtstaff.Visible = false;
            //    pstaff.Visible = false;
            //}
            //else
            //{
            //    lblstaff.Visible = true;
            //    txtstaff.Visible = true;
            //    pstaff.Visible = true;
            //}
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklscolumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void ddl_formatewiseSelectIndexChange(object sender, EventArgs e)
    {


    }

    public void staffSalaryCertificate()
    {
        try
        {
            if (rdbindividual.Checked == true)
            {
                Hashtable PayLastMonthAllowanceHash = new Hashtable();
                Hashtable PayLastMonthDeductionHash = new Hashtable();
                double PayLastMonthAllowance = 0;
                double PayLastMonthDeduction = 0;
                int noofmonth = 0;
                FpMonthOverall.SaveChanges();
                if (txtallowance.Text.Trim() == "--Select--" && chklsallowance.Items.Count != 0)
                {
                    lblgenerror.Text = "Please Select Any one Allowance!";
                    lblgenerror.Visible = true;
                    return;
                }
                if (txtdeduction.Text.Trim() == "--Select--" && chklsdeduction.Items.Count != 0)
                {
                    lblgenerror.Text = "Please Select Any one Deduction!";
                    lblgenerror.Visible = true;
                    return;
                }
                for (int i = 1; i < FpMonthOverall.Sheets[0].RowCount; i++)
                {
                    int isval = Convert.ToInt32(FpMonthOverall.Sheets[0].Cells[i, 1].Value);
                    if (isval == 1)
                    {
                        noofmonth++;
                    }
                }

                if (noofmonth == 0)
                {
                    lblgenerror.Text = "Please Select The Staff And Then Proceed";
                    lblgenerror.Visible = true;
                    return;
                }

                string fromyear = ddlfyear.SelectedValue.ToString();

                string frommonth = ddlfmonth.SelectedValue.ToString();

                int FromMonthNo = Convert.ToInt32(frommonth);
                DateTime dtFDate = new DateTime(2000, FromMonthNo, 1);
                string sFromMonthName = dtFDate.ToString("MMM");
                // string sMonthFullName = dtDate.ToString("MMMM"); 
                string toyear = ddltyear.SelectedValue.ToString();
                string tomonth = ddltmonth.SelectedValue.ToString();

                int ToMonthNo = Convert.ToInt32(tomonth);
                DateTime dtTDate = new DateTime(2000, ToMonthNo, 1);
                string sToMonthName = dtTDate.ToString("MMM");

                if (frommonth.Trim() == "0")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The From Month And Then Proceed";
                    return;
                }

                if (tomonth.Trim() == "0")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The TO Month And Then Proceed";
                    return;
                }

                int fromyearval = (Convert.ToInt32(fromyear) * 12) + Convert.ToInt32(frommonth);
                int toyearval = (Convert.ToInt32(toyear) * 12) + Convert.ToInt32(tomonth);
                if (fromyearval > toyearval)
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The To Month And Year Must Be Equal To Greater Than From Month And Year";
                    return;
                }
                int toalnoofrows = toyearval - fromyearval;
                if (toalnoofrows > 6)
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select Only Six Months or Below Than Six Months For Certificate";
                    return;

                }
                toalnoofrows++;

                string deptcode = "";
                for (int i = 0; i < chklsdept.Items.Count; i++)
                {
                    if (chklsdept.Items[i].Selected == true)
                    {
                        if (deptcode == "")
                        {
                            deptcode = "'" + chklsdept.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            deptcode = deptcode + ",'" + chklsdept.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                if (deptcode.Trim() == "")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Department And Then Proceed";
                    return;
                }

                string design = "";
                for (int i = 0; i < chklsdesign.Items.Count; i++)
                {
                    if (chklsdesign.Items[i].Selected == true)
                    {
                        if (design == "")
                        {
                            design = "'" + chklsdesign.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            design = design + ",'" + chklsdesign.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                if (design.Trim() == "")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Designation And Then Proceed";
                    return;
                }

                string cateory = "";
                for (int i = 0; i < chklscategory.Items.Count; i++)
                {
                    if (chklscategory.Items[i].Selected == true)
                    {
                        if (cateory == "")
                        {
                            cateory = "'" + chklscategory.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            cateory = cateory + ",'" + chklscategory.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                if (cateory.Trim() == "")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Category And Then Proceed";
                    return;
                }

                Font Fontbold1 = new Font("Book Antiqua", 20, FontStyle.Bold);
                Font Fontbold2 = new Font("Book Antiqua", 14, FontStyle.Bold);
                Font Fontbold3 = new Font("Book Antiqua", 14, FontStyle.Regular);
                Font Fontsmall = new Font("Book Antiqua", 12, FontStyle.Regular);
                Font Fontsmall1 = new Font("Book Antiqua", 11, FontStyle.Bold);
                //  Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(60, 40));

                Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                int left1 = 70;
                Gios.Pdf.PdfTablePage tblpage;
                Gios.Pdf.PdfTable tblpayprocess;

                //string strhryearquery = "select PayMonthNum,PayYear,From_Date,To_Date,year(from_date) fyear,year(to_date) tyear from HrPayMonths where College_Code='" + collegecode + "'";

                string strquery = "select s.staff_name,s.staff_code,s.pangirnumber,h.dept_name,d.desig_name,st.stftype,h.dept_code,d.desig_code,m.fdate,m.tdate,st.category_code,s.pfnumber,s.bankaccount,m.basic_alone,m.grade_pay,m.pay_band,m.allowances,m.deductions,m.netded,m.PayMonth,m.PayYear,st.allowances actall,st.deductions actdeduct,m.lop,m.netded,m.netsal,st.IsConsolid from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d ";
                strquery = strquery + " where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.latestrec = 1 and st.dept_code in(" + deptcode + ") and st.desig_code in(" + design + ") and st.category_code in(" + cateory + ")";
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                int coltop = 0;
                string footerdetails = "";
                string footercontent = "";
                string printcontent = "";

                for (int i = 1; i < FpMonthOverall.Sheets[0].RowCount; i++)
                {
                    int isval = Convert.ToInt32(FpMonthOverall.Sheets[0].Cells[i, 1].Value);
                    if (isval == 1)
                    {

                        coltop = 0;
                        string staffcode = FpMonthOverall.Sheets[0].Cells[i, 2].Text.ToString();
                        string staffname = FpMonthOverall.Sheets[0].Cells[i, 3].Text.ToString();
                        string department = FpMonthOverall.Sheets[0].Cells[i, 4].Text.ToString();
                        string Designation = FpMonthOverall.Sheets[0].Cells[i, 5].Text.ToString();

                        Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

                        coltop = coltop + 10;
                        PdfTextArea ptacoll = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "SALARY CERTIFICATE");
                        coltop = coltop + 30;

                        PdfTextArea ptasnameval = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, left1 + 30, coltop + 5, mydocument.PageWidth - 50, 50), System.Drawing.ContentAlignment.MiddleLeft, "Following are the pay particular drawn by " + staffname + ",");
                        mypdfpage.Add(ptasnameval);
                        coltop = coltop + 23;
                        ptasnameval = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, left1 - 35, coltop + 5, mydocument.PageWidth - 50, 50), System.Drawing.ContentAlignment.MiddleLeft, " " + Designation + ", " + "Department of" + " " + department + " " + "of our college for the period from" + " " + sFromMonthName + " " + fromyear + " " + "to" + " " + sToMonthName + " " + toyear + ":");
                        mypdfpage.Add(ptasnameval);

                        ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                        DataView dvstaffpf = ds.Tables[0].DefaultView;
                        mypdfpage.Add(ptacoll);


                        ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                        DataView dvstaff = ds.Tables[0].DefaultView;
                        int lastpayMonth = 0;
                        int lastpayYear = 0;
                        string strhryearquery = "select paymonth,payyear,netsal,netaddact,netadd,addd,deddd,grade_pay,convert(varchar(max), allowances)as allowances,convert(varchar(max),deductions)as deductions,bsalary from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code='" + staffcode + "'  group by payyear,paymonth,netsal,netaddact,netadd,addd,deddd,grade_pay,convert(varchar(max), allowances),convert(varchar(max),deductions),bsalary order by year(payyear),year(paymonth)";

                        DataSet dshryear = d2.select_method_wo_parameter(strhryearquery, "Text");
                        DataTable dt = new DataTable();
                        DataTable dts = new DataTable();
                        DataRow dr;
                        // dts.Columns.Add("EARNINGS");
                        dts.Columns.Add(" ");
                        dt = dshryear.Tables[0];

                        for (int datasetcount = 0; datasetcount < dshryear.Tables[0].Rows.Count; datasetcount++)
                        {
                            if (dshryear.Tables != null && dshryear.Tables[0].Rows.Count > 0)
                            {
                                double lastMonthSalary = 0;
                                double.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["netaddact"]), out lastMonthSalary);
                                int.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["paymonth"]), out lastpayMonth);
                                int.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["payyear"]), out lastpayYear);
                                double.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["addd"]), out PayLastMonthAllowance);
                                double.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["deddd"]), out PayLastMonthDeduction);

                                DateTime dtFromDate = new DateTime(2000, lastpayMonth, 1);
                                string FromMonthName = dtFromDate.ToString("MMM");
                                string Month_year = FromMonthName + "-" + lastpayYear;
                                dts.Columns.Add(Month_year);
                                string AllowanceValue = Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["allowances"]);
                                int Month = Convert.ToInt32(dshryear.Tables[0].Rows[datasetcount]["paymonth"]);
                                string year = Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["payyear"]);

                                string[] SplitFirst = AllowanceValue.Split('\\');
                                if (SplitFirst.Length > 0)
                                {
                                    for (int intc = 0; intc < SplitFirst.Length; intc++)
                                    {
                                        if (!string.IsNullOrEmpty(SplitFirst[intc].Trim()))
                                        {
                                            string[] SecondSplit = SplitFirst[intc].Split(';');
                                            if (SecondSplit.Length > 0)
                                            {
                                                double AllowTaeknValue = 0;
                                                string Allow = string.Empty;
                                                string takenValue = SecondSplit[2].ToString();
                                                if (takenValue.Trim().Contains('-'))
                                                {
                                                    Allow = takenValue.Split('-')[1];
                                                    if (Allow.Trim() != "")
                                                    {
                                                        double.TryParse(Allow, out AllowTaeknValue);
                                                    }
                                                }
                                                else
                                                {
                                                    Allow = SecondSplit[3].ToString();
                                                    if (Allow.Trim() != "")
                                                    {
                                                        double.TryParse(Allow, out AllowTaeknValue);
                                                    }
                                                }
                                                if (!PayLastMonthAllowanceHash.ContainsKey(SecondSplit[0].Trim()))
                                                {
                                                    PayLastMonthAllowanceHash.Add(SecondSplit[0].Trim(), Month_year + "," + AllowTaeknValue);
                                                }
                                                else
                                                {
                                                    string GetValue = Convert.ToString(PayLastMonthAllowanceHash[SecondSplit[0].Trim()]);
                                                    GetValue = GetValue + ";" + Month_year + "," + AllowTaeknValue;
                                                    PayLastMonthAllowanceHash.Remove(SecondSplit[0].Trim());
                                                    PayLastMonthAllowanceHash.Add(SecondSplit[0].Trim(), GetValue);
                                                }
                                            }
                                        }
                                    }
                                }
                                string deductionValue = Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["deductions"]);
                                string[] SplitFirsts = deductionValue.Split('\\');
                                if (SplitFirst.Length > 0)
                                {
                                    for (int intc = 0; intc < SplitFirsts.Length; intc++)
                                    {
                                        if (!string.IsNullOrEmpty(SplitFirsts[intc].Trim()))
                                        {
                                            string[] SecondSplit = SplitFirsts[intc].Split(';');
                                            if (SecondSplit.Length > 0)
                                            {
                                                double AllowTaeknValue = 0;
                                                string Allow = string.Empty;
                                                string takenValue = SecondSplit[2].ToString();
                                                if (takenValue.Trim().Contains('-'))
                                                {
                                                    Allow = takenValue.Split('-')[1];
                                                    if (Allow.Trim() != "")
                                                    {
                                                        double.TryParse(Allow, out AllowTaeknValue);
                                                    }
                                                }
                                                else
                                                {
                                                    Allow = SecondSplit[3].ToString();
                                                    if (Allow.Trim() != "")
                                                    {
                                                        double.TryParse(Allow, out AllowTaeknValue);
                                                    }
                                                }
                                                if (!PayLastMonthDeductionHash.ContainsKey(SecondSplit[0].Trim()))
                                                {
                                                    PayLastMonthDeductionHash.Add(SecondSplit[0].Trim(), Month_year + "," + AllowTaeknValue);
                                                }
                                                else
                                                {
                                                    string GetValue = Convert.ToString(PayLastMonthDeductionHash[SecondSplit[0].Trim()]);
                                                    GetValue = GetValue + ";" + Month_year + "," + AllowTaeknValue;
                                                    PayLastMonthDeductionHash.Remove(SecondSplit[0].Trim());
                                                    PayLastMonthDeductionHash.Add(SecondSplit[0].Trim(), GetValue);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (dshryear.Tables[0].Rows.Count > 0)
                        {
                            dr = dts.NewRow();
                            for (int header = 0; header < dts.Columns.Count; header++)
                            {
                                string ColumnName = Convert.ToString(dts.Columns[header].ColumnName);
                                dr[header] = Convert.ToString(ColumnName);
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            for (int header = 1; header < dts.Columns.Count; header++)
                            {
                                string ColumnName = Convert.ToString(dts.Columns[header].ColumnName);

                                dr[header] = Convert.ToString("------------");
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("EARNINGS");
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("Basic Pay");
                            for (int basic = 0; basic < dshryear.Tables[0].Rows.Count; basic++)
                            {
                                string bsalary = Convert.ToString(dshryear.Tables[0].Rows[basic]["bsalary"]);
                                double amount = Convert.ToDouble(bsalary);
                                amount = Math.Round(amount, 0, MidpointRounding.AwayFromZero);
                                int val = basic + 1;
                                dr[val] = Convert.ToString(amount);
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("Grade Pay");
                            for (int grad = 0; grad < dshryear.Tables[0].Rows.Count; grad++)
                            {
                                string gradepay = Convert.ToString(dshryear.Tables[0].Rows[grad]["grade_pay"]);
                                double gradeamount = Convert.ToDouble(gradepay);
                                gradeamount = Math.Round(gradeamount, 0, MidpointRounding.AwayFromZero);
                                int val = grad + 1;
                                dr[val] = Convert.ToString(gradeamount);
                            }
                            dts.Rows.Add(dr);
                            foreach (DictionaryEntry item in PayLastMonthAllowanceHash)
                            {
                                dr = dts.NewRow();
                                string key = Convert.ToString(item.Key);
                                dr[0] = Convert.ToString(item.Key);
                                string value = Convert.ToString(item.Value);
                                if (value.Contains(";"))
                                {
                                    string[] arr = value.Split(';');
                                    for (int montval = 0; montval < arr.Length; montval++)
                                    {
                                        string month = arr[montval];
                                        string[] splitval = month.Split(',');
                                        string month_years = splitval[0];
                                        string val = Convert.ToString(splitval[1]);
                                        if (dts.Columns.Contains(month_years))
                                        {
                                            dr[month_years] = Convert.ToString(val);
                                        }
                                    }
                                    dts.Rows.Add(dr);
                                }
                            }
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString(" ");
                            for (int header = 1; header < dts.Columns.Count; header++)
                            {
                                string ColumnName = Convert.ToString(dts.Columns[header].ColumnName);
                                dr[header] = Convert.ToString("---------");
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("GROSS");
                            for (int gross = 0; gross < dshryear.Tables[0].Rows.Count; gross++)
                            {
                                string grosssalary = Convert.ToString(dshryear.Tables[0].Rows[gross]["netadd"]);
                                double grossamount = Convert.ToDouble(grosssalary);
                                grossamount = Math.Round(grossamount, 0, MidpointRounding.AwayFromZero);
                                int val = gross + 1;
                                dr[val] = Convert.ToString(grossamount);
                            }

                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("DEDUCTIONS");
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString(" ");
                            dts.Rows.Add(dr);
                            foreach (DictionaryEntry item in PayLastMonthDeductionHash)
                            {
                                dr = dts.NewRow();
                                string key = Convert.ToString(item.Key);
                                dr[0] = Convert.ToString(item.Key);
                                string value = Convert.ToString(item.Value);
                                if (value.Contains(";"))
                                {
                                    string[] arr = value.Split(';');
                                    for (int montval = 0; montval < arr.Length; montval++)
                                    {
                                        string month = arr[montval];
                                        string[] splitval = month.Split(',');
                                        string month_years = splitval[0];
                                        string val = Convert.ToString(splitval[1]);
                                        if (dts.Columns.Contains(month_years))
                                        {
                                            dr[month_years] = Convert.ToString(val);
                                        }
                                    }
                                    dts.Rows.Add(dr);
                                }
                            }
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString(" ");
                            dr = dts.NewRow();
                            for (int header = 1; header < dts.Columns.Count; header++)
                            {
                                string ColumnName = Convert.ToString(dts.Columns[header].ColumnName);
                                dr[header] = Convert.ToString("---------");
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("TOTAL");
                            for (int deduct = 0; deduct < dshryear.Tables[0].Rows.Count; deduct++)
                            {
                                string deduction = Convert.ToString(dshryear.Tables[0].Rows[deduct]["deddd"]);
                                double deductionamount = Convert.ToDouble(deduction);
                                deductionamount = Math.Round(deductionamount, 0, MidpointRounding.AwayFromZero);
                                int val = deduct + 1;
                                dr[val] = Convert.ToString(deductionamount);

                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("NET");
                            for (int net = 0; net < dshryear.Tables[0].Rows.Count; net++)
                            {
                                string netpay = Convert.ToString(dshryear.Tables[0].Rows[net]["netsal"]);
                                double netamount = Convert.ToDouble(netpay);
                                netamount = Math.Round(netamount, 0, MidpointRounding.AwayFromZero);
                                int val = net + 1;
                                dr[val] = Convert.ToString(netamount);

                            }

                            dts.Rows.Add(dr);

                            int no_ofColumns = Convert.ToInt32(dts.Columns.Count);
                            int no_ofRows = Convert.ToInt32(dts.Rows.Count);
                            tblpayprocess = mydocument.NewTable(Fontsmall, no_ofRows, no_ofColumns, 2);
                            //  tblpayprocess.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            tblpayprocess.SetBorders(Color.Black, 1, BorderType.None);
                            tblpayprocess.VisibleHeaders = false;
                            for (int tabval = 0; tabval < dts.Rows.Count; tabval++)
                            {
                                int values = 0;
                                for (int coulumnval = 0; coulumnval < dts.Columns.Count; coulumnval++)
                                {
                                    if (coulumnval == 0)
                                    {
                                        tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontbold3);

                                        tblpayprocess.Columns[coulumnval].SetWidth(50);
                                        string RowName = Convert.ToString(dts.Rows[tabval][coulumnval]);
                                        if (RowName.ToUpper() == "GROSS" || RowName.ToUpper() == "TOTAL" || RowName.ToUpper() == "NET")
                                        {
                                            tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleRight);
                                            tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontbold2);
                                        }
                                        if (RowName.ToUpper() == "DEDUCTIONS" || RowName.ToUpper() == "EARNINGS")
                                        {
                                            tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontbold2);
                                        }
                                    }
                                    else
                                    {
                                        tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleRight);
                                        tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontsmall1);
                                        tblpayprocess.Columns[coulumnval].SetWidth(30);
                                    }
                                    if (tabval == 0 && coulumnval == values || tabval == 1 && coulumnval == values)
                                    {
                                        tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontbold2);
                                        tblpayprocess.Columns[coulumnval].SetWidth(30);
                                    }
                                    values++;

                                    tblpayprocess.Cell(tabval, coulumnval).SetContent(Convert.ToString(dts.Rows[tabval][coulumnval]));
                                }
                            }
                            footerdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='printpdf_footersetting' and college_code='" + Convert.ToString(collegecode) + "' and user_Code='" + Convert.ToString(Session["usercode"]) + "'");

                            if (footerdetails != "0")
                            {
                                if (footerdetails.Contains(","))
                                {
                                    string[] splitval = footerdetails.Split(',');
                                    footercontent = Convert.ToString(splitval[0]);
                                    printcontent = Convert.ToString(splitval[1]);
                                }
                                else
                                {
                                    footercontent = footerdetails;
                                    printcontent = "";
                                }
                            }
                            if (footerdetails == "0")
                            {
                                footerdetails = "";
                                printcontent = "";
                            }
                            PdfTextArea footer = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 50, mydocument.PageHeight - 160, mydocument.PageWidth, 40),
                     System.Drawing.ContentAlignment.MiddleLeft, printcontent);
                            mypdfpage.Add(footer);
                            PdfTextArea ptadate = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 10, mydocument.PageHeight - 110, mydocument.PageWidth, 40),
                     System.Drawing.ContentAlignment.MiddleLeft, "Date" + ":" + DateTime.Now.ToString("dd/MM/yyyy"));
                            mypdfpage.Add(ptadate);
                            PdfTextArea ptaadmin = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                           new PdfArea(mydocument, mydocument.PageWidth - 100, mydocument.PageHeight - 110, mydocument.PageWidth, 40),
                    System.Drawing.ContentAlignment.MiddleLeft, footercontent);
                            mypdfpage.Add(ptaadmin);
                            FpMonthOverall.Sheets[0].Columns[1].Visible = true;
                            if (dvstaff.Count > 0)
                            {
                                coltop += 50;
                                tblpage = tblpayprocess.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop + 20, mydocument.PageWidth - 50, 1200));
                                mypdfpage.Add(tblpage);
                            }
                            mypdfpage.SaveToDocument();
                        }
                    }//isval
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "OriginalSalaryDetails" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    Response.Buffer = true;
                    Response.Clear();
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            if (rdmappingbased.Checked == true)//Added by Saranya on 17/08/2018
            {
                string college = Session["collegecode"].ToString();
                Hashtable PayLastMonthAllowanceHash = new Hashtable();
                Hashtable PayLastMonthDeductionHash = new Hashtable();
                Hashtable HsCommonDeductionHead = new Hashtable();
                DataSet dtDeductionHead = new DataSet();
                double PayLastMonthAllowance = 0;
                double PayLastMonthDeduction = 0;
                int noofmonth = 0;
                FpMonthOverall.SaveChanges();
                if (txtallowance.Text.Trim() == "--Select--" && chklsallowance.Items.Count != 0)
                {
                    lblgenerror.Text = "Please Select Any one Allowance!";
                    lblgenerror.Visible = true;
                    return;
                }
                if (txtdeduction.Text.Trim() == "--Select--" && chklsdeduction.Items.Count != 0)
                {
                    lblgenerror.Text = "Please Select Any one Deduction!";
                    lblgenerror.Visible = true;
                    return;
                }
                for (int i = 1; i < FpMonthOverall.Sheets[0].RowCount; i++)
                {
                    int isval = Convert.ToInt32(FpMonthOverall.Sheets[0].Cells[i, 1].Value);
                    if (isval == 1)
                    {
                        noofmonth++;
                    }
                }
                if (noofmonth == 0)
                {
                    lblgenerror.Text = "Please Select The Staff And Then Proceed";
                    lblgenerror.Visible = true;
                    return;
                }
                string fromyear = ddlfyear.SelectedValue.ToString();
                string frommonth = ddlfmonth.SelectedValue.ToString();
                int FromMonthNo = Convert.ToInt32(frommonth);
                DateTime dtFDate = new DateTime(2000, FromMonthNo, 1);
                string sFromMonthName = dtFDate.ToString("MMM");
                // string sMonthFullName = dtDate.ToString("MMMM"); 
                string toyear = ddltyear.SelectedValue.ToString();
                string tomonth = ddltmonth.SelectedValue.ToString();
                int ToMonthNo = Convert.ToInt32(tomonth);
                DateTime dtTDate = new DateTime(2000, ToMonthNo, 1);
                string sToMonthName = dtTDate.ToString("MMM");
                if (frommonth.Trim() == "0")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The From Month And Then Proceed";
                    return;
                }

                if (tomonth.Trim() == "0")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The TO Month And Then Proceed";
                    return;
                }

                int fromyearval = (Convert.ToInt32(fromyear) * 12) + Convert.ToInt32(frommonth);
                int toyearval = (Convert.ToInt32(toyear) * 12) + Convert.ToInt32(tomonth);
                if (fromyearval > toyearval)
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The To Month And Year Must Be Equal To Greater Than From Month And Year";
                    return;
                }
                int toalnoofrows = toyearval - fromyearval;
                if (toalnoofrows > 6)
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select Only Six Months or Below Than Six Months For Certificate";
                    return;

                }
                toalnoofrows++;

                string deptcode = "";
                for (int i = 0; i < chklsdept.Items.Count; i++)
                {
                    if (chklsdept.Items[i].Selected == true)
                    {
                        if (deptcode == "")
                        {
                            deptcode = "'" + chklsdept.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            deptcode = deptcode + ",'" + chklsdept.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                if (deptcode.Trim() == "")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Department And Then Proceed";
                    return;
                }

                string design = "";
                for (int i = 0; i < chklsdesign.Items.Count; i++)
                {
                    if (chklsdesign.Items[i].Selected == true)
                    {
                        if (design == "")
                        {
                            design = "'" + chklsdesign.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            design = design + ",'" + chklsdesign.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                if (design.Trim() == "")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Designation And Then Proceed";
                    return;
                }

                string cateory = "";
                for (int i = 0; i < chklscategory.Items.Count; i++)
                {
                    if (chklscategory.Items[i].Selected == true)
                    {
                        if (cateory == "")
                        {
                            cateory = "'" + chklscategory.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            cateory = cateory + ",'" + chklscategory.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                if (cateory.Trim() == "")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select The Category And Then Proceed";
                    return;
                }

                string SelQry = "select * from IT_OtherAllowanceDeducation where ittype=3 and collegecode='" + college + "'";

                Font Fontbold1 = new Font("Book Antiqua", 20, FontStyle.Bold);
                Font Fontbold2 = new Font("Book Antiqua", 14, FontStyle.Bold);
                Font Fontbold3 = new Font("Book Antiqua", 14, FontStyle.Regular);
                Font Fontsmall = new Font("Book Antiqua", 12, FontStyle.Regular);
                Font Fontsmall1 = new Font("Book Antiqua", 11, FontStyle.Bold);
                Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfTablePage tblpage;
                Gios.Pdf.PdfTable tblpayprocess;
                int left1 = 70;

                //string strhryearquery = "select PayMonthNum,PayYear,From_Date,To_Date,year(from_date) fyear,year(to_date) tyear from HrPayMonths where College_Code='" + collegecode + "'";
                string strquery = "select s.staff_name,s.staff_code,s.pangirnumber,h.dept_name,d.desig_name,st.stftype,h.dept_code,d.desig_code,m.fdate,m.tdate,st.category_code,s.pfnumber,s.bankaccount,m.basic_alone,m.grade_pay,m.pay_band,m.allowances,m.deductions,m.netded,m.PayMonth,m.PayYear,st.allowances actall,st.deductions actdeduct,m.lop,m.netded,m.netsal,st.IsConsolid from monthlypay m,staffmaster s,stafftrans st,hrdept_master h,desig_master d ";
                strquery = strquery + " where s.staff_code=m.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and m.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.latestrec = 1 and st.dept_code in(" + deptcode + ") and st.desig_code in(" + design + ") and st.category_code in(" + cateory + ")";
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                int coltop = 0;
                string footerdetails = "";
                string footercontent = "";
                string printcontent = "";

                for (int i = 1; i < FpMonthOverall.Sheets[0].RowCount; i++)
                {
                    int isval = Convert.ToInt32(FpMonthOverall.Sheets[0].Cells[i, 1].Value);
                    if (isval == 1)
                    {
                        coltop = 0;
                        string staffcode = FpMonthOverall.Sheets[0].Cells[i, 2].Text.ToString();
                        string staffname = FpMonthOverall.Sheets[0].Cells[i, 3].Text.ToString();
                        string department = FpMonthOverall.Sheets[0].Cells[i, 4].Text.ToString();
                        string Designation = FpMonthOverall.Sheets[0].Cells[i, 5].Text.ToString();
                        Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
                        coltop = coltop + 10;
                        PdfTextArea ptacoll = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "SALARY CERTIFICATE");
                        coltop = coltop + 30;
                        PdfTextArea ptasnameval = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, left1 + 30, coltop + 5, mydocument.PageWidth - 50, 50), System.Drawing.ContentAlignment.MiddleLeft, "Following are the pay particular drawn by " + staffname + ",");
                        mypdfpage.Add(ptasnameval);
                        coltop = coltop + 23;
                        ptasnameval = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, left1 - 35, coltop + 5, mydocument.PageWidth - 50, 50), System.Drawing.ContentAlignment.MiddleLeft, " " + Designation + ", " + "Department of" + " " + department + " " + "of our college for the period from" + " " + sFromMonthName + " " + fromyear + " " + "to" + " " + sToMonthName + " " + toyear + ":");
                        mypdfpage.Add(ptasnameval);

                        ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                        DataView dvstaffpf = ds.Tables[0].DefaultView;
                        mypdfpage.Add(ptacoll);

                        ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "'";
                        DataView dvstaff = ds.Tables[0].DefaultView;
                        int lastpayMonth = 0;
                        int lastpayYear = 0;
                        string strhryearquery = "select paymonth,payyear,netsal,netaddact,netadd,addd,deddd,grade_pay,convert(varchar(max), allowances)as allowances,convert(varchar(max),deductions)as deductions,bsalary from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code='" + staffcode + "'  group by payyear,paymonth,netsal,netaddact,netadd,addd,deddd,grade_pay,convert(varchar(max), allowances),convert(varchar(max),deductions),bsalary order by year(payyear),year(paymonth)";

                        DataSet dshryear = d2.select_method_wo_parameter(strhryearquery, "Text");
                        DataTable dt = new DataTable();
                        DataTable dts = new DataTable();
                        DataRow dr;
                        // dts.Columns.Add("EARNINGS");
                        dts.Columns.Add(" ");
                        dt = dshryear.Tables[0];
                        Dictionary<string, Hashtable> dicData = new Dictionary<string, Hashtable>();
                        for (int datasetcount = 0; datasetcount < dshryear.Tables[0].Rows.Count; datasetcount++)
                        {
                            Hashtable htEachMonth = new Hashtable();
                            PayLastMonthDeductionHash.Clear();
                            if (dshryear.Tables != null && dshryear.Tables[0].Rows.Count > 0)
                            {
                                double lastMonthSalary = 0;
                                double.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["netaddact"]), out lastMonthSalary);
                                int.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["paymonth"]), out lastpayMonth);
                                int.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["payyear"]), out lastpayYear);
                                double.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["addd"]), out PayLastMonthAllowance);
                                double.TryParse(Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["deddd"]), out PayLastMonthDeduction);

                                DateTime dtFromDate = new DateTime(2000, lastpayMonth, 1);
                                string FromMonthName = dtFromDate.ToString("MMM");
                                string Month_year = FromMonthName + "-" + lastpayYear;
                                dts.Columns.Add(Month_year);
                                string AllowanceValue = Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["allowances"]);
                                int Month = Convert.ToInt32(dshryear.Tables[0].Rows[datasetcount]["paymonth"]);
                                string year = Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["payyear"]);

                                string[] SplitFirst = AllowanceValue.Split('\\');
                                if (SplitFirst.Length > 0)
                                {
                                    for (int intc = 0; intc < SplitFirst.Length; intc++)
                                    {
                                        if (!string.IsNullOrEmpty(SplitFirst[intc].Trim()))
                                        {
                                            string[] SecondSplit = SplitFirst[intc].Split(';');
                                            if (SecondSplit.Length > 0)
                                            {
                                                double AllowTaeknValue = 0;
                                                string Allow = string.Empty;
                                                string takenValue = SecondSplit[2].ToString();
                                                if (takenValue.Trim().Contains('-'))
                                                {
                                                    Allow = takenValue.Split('-')[1];
                                                    if (Allow.Trim() != "")
                                                    {
                                                        double.TryParse(Allow, out AllowTaeknValue);
                                                    }
                                                }
                                                else
                                                {
                                                    Allow = SecondSplit[3].ToString();
                                                    if (Allow.Trim() != "")
                                                    {
                                                        double.TryParse(Allow, out AllowTaeknValue);
                                                    }
                                                }
                                                if (!PayLastMonthAllowanceHash.ContainsKey(SecondSplit[0].Trim()))
                                                {
                                                    PayLastMonthAllowanceHash.Add(SecondSplit[0].Trim(), Month_year + "," + AllowTaeknValue);
                                                }
                                                else
                                                {
                                                    string GetValue = Convert.ToString(PayLastMonthAllowanceHash[SecondSplit[0].Trim()]);
                                                    GetValue = GetValue + ";" + Month_year + "," + AllowTaeknValue;
                                                    PayLastMonthAllowanceHash.Remove(SecondSplit[0].Trim());
                                                    PayLastMonthAllowanceHash.Add(SecondSplit[0].Trim(), GetValue);
                                                }
                                            }
                                        }
                                    }
                                }
                                string SelectQry = "select * from IT_OtherAllowanceDeducation where ittype=3 and collegecode='" + college + "'";
                                dtDeductionHead.Clear();
                                dtDeductionHead = d2.select_method_wo_parameter(SelectQry, "text");
                                string deductionValue = Convert.ToString(dshryear.Tables[0].Rows[datasetcount]["deductions"]);
                                string[] Splits = deductionValue.Split('\\');
                                if (Splits.Length > 0)
                                {
                                    for (int intc = 0; intc < Splits.Length; intc++)
                                    {
                                        if (!string.IsNullOrEmpty(Splits[intc].Trim()))
                                        {
                                            string[] SecondSplit = Splits[intc].Split(';');
                                            if (SecondSplit.Length > 0)
                                            {
                                                double AllowTaeknValue = 0;
                                                string Allow = string.Empty;
                                                string takenValue = SecondSplit[2].ToString();
                                                if (takenValue.Trim().Contains('-'))
                                                {
                                                    Allow = takenValue.Split('-')[1];
                                                    if (Allow.Trim() != "")
                                                    {
                                                        double.TryParse(Allow, out AllowTaeknValue);
                                                    }
                                                }
                                                else
                                                {
                                                    Allow = SecondSplit[3].ToString();
                                                    if (Allow.Trim() != "")
                                                    {
                                                        double.TryParse(Allow, out AllowTaeknValue);
                                                    }
                                                }
                                                if (!PayLastMonthDeductionHash.ContainsKey(SecondSplit[0].Trim()))
                                                {
                                                    PayLastMonthDeductionHash.Add(SecondSplit[0].Trim(), Month_year + "," + AllowTaeknValue);
                                                }
                                                else
                                                {
                                                    string GetValue = Convert.ToString(PayLastMonthDeductionHash[SecondSplit[0].Trim()]);
                                                    GetValue = GetValue + ";" + Month_year + "," + AllowTaeknValue;
                                                    PayLastMonthDeductionHash.Remove(SecondSplit[0].Trim());
                                                    PayLastMonthDeductionHash.Add(SecondSplit[0].Trim(), GetValue);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (dtDeductionHead.Tables[0].Rows.Count > 0)
                                {
                                    for (int ded = 0; ded < dtDeductionHead.Tables[0].Rows.Count; ded++)
                                    {
                                        string ComDedductName = Convert.ToString(dtDeductionHead.Tables[0].Rows[ded]["ITAllowDeductName"]);
                                        string DeductionName = Convert.ToString(dtDeductionHead.Tables[0].Rows[ded]["ITCommonValue"]);
                                        if (!HsCommonDeductionHead.ContainsKey(ComDedductName))
                                            HsCommonDeductionHead.Add(ComDedductName, Convert.ToString(DeductionName));
                                        else
                                        {
                                            HsCommonDeductionHead.Remove(ComDedductName);
                                            HsCommonDeductionHead.Add(ComDedductName, Convert.ToString(DeductionName));
                                        }
                                    }
                                    foreach (DictionaryEntry DicHead in HsCommonDeductionHead)
                                    {
                                        string Head_Name = Convert.ToString(DicHead.Key);

                                        string val = Convert.ToString(DicHead.Value);
                                        string[] Split_First = val.Split(',');
                                        double amt = 0;
                                        if (Split_First.Length > 0)
                                        {
                                            for (int intc = 0; intc < Split_First.Length; intc++)
                                            {
                                                string deductVal = Split_First[intc];
                                                if (PayLastMonthDeductionHash.ContainsKey(deductVal))
                                                {
                                                    string aa = (PayLastMonthDeductionHash[deductVal]).ToString().Split(',')[1];
                                                    amt = amt + Convert.ToDouble(aa);
                                                }
                                            }
                                        }
                                        htEachMonth.Add(Head_Name, amt);
                                    }
                                    dicData.Add(Month_year, htEachMonth);
                                }
                            }
                        }
                        if (dshryear.Tables[0].Rows.Count > 0)
                        {
                            dr = dts.NewRow();
                            for (int header = 0; header < dts.Columns.Count; header++)
                            {
                                string ColumnName = Convert.ToString(dts.Columns[header].ColumnName);
                                dr[header] = Convert.ToString(ColumnName);
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            for (int header = 1; header < dts.Columns.Count; header++)
                            {
                                string ColumnName = Convert.ToString(dts.Columns[header].ColumnName);

                                dr[header] = Convert.ToString("------------");
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("EARNINGS");
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("Basic Pay");
                            for (int basic = 0; basic < dshryear.Tables[0].Rows.Count; basic++)
                            {
                                string bsalary = Convert.ToString(dshryear.Tables[0].Rows[basic]["bsalary"]);
                                double amount = Convert.ToDouble(bsalary);
                                amount = Math.Round(amount, 0, MidpointRounding.AwayFromZero);
                                int val = basic + 1;
                                dr[val] = Convert.ToString(amount);
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            double gradeamount = 0;
                            dr[0] = Convert.ToString("Grade Pay");
                            for (int grad = 0; grad < dshryear.Tables[0].Rows.Count; grad++)
                            {
                                string gradepay = Convert.ToString(dshryear.Tables[0].Rows[grad]["grade_pay"]);
                                gradeamount = Convert.ToDouble(gradepay);
                                gradeamount = Math.Round(gradeamount, 0, MidpointRounding.AwayFromZero);
                                int val = grad + 1;
                                dr[val] = Convert.ToString(gradeamount);
                            }
                            if (gradeamount!=0)
                            dts.Rows.Add(dr);
                           
                            
                            foreach (DictionaryEntry item in PayLastMonthAllowanceHash)
                            {
                                dr = dts.NewRow();
                                string key = Convert.ToString(item.Key);
                                dr[0] = Convert.ToString(item.Key);
                                string value = Convert.ToString(item.Value);
                                double checkval = 0;
                                if (value.Contains(";"))
                                {
                                    string[] arr = value.Split(';');
                                    for (int montval = 0; montval < arr.Length; montval++)
                                    {
                                        string month = arr[montval];
                                        string[] splitval = month.Split(',');
                                        string month_years = splitval[0];
                                        string val = Convert.ToString(splitval[1]);
                                        checkval = checkval + Convert.ToDouble(val);
                                        if (dts.Columns.Contains(month_years))
                                        {
                                            dr[month_years] = Convert.ToString(val);
                                        }
                                    }
                                    if (checkval!=0)
                                    dts.Rows.Add(dr);
                                }
                            }
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString(" ");
                            for (int header = 1; header < dts.Columns.Count; header++)
                            {
                                string ColumnName = Convert.ToString(dts.Columns[header].ColumnName);
                                dr[header] = Convert.ToString("---------");
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("GROSS");
                            for (int gross = 0; gross < dshryear.Tables[0].Rows.Count; gross++)
                            {
                                string grosssalary = Convert.ToString(dshryear.Tables[0].Rows[gross]["netadd"]);
                                double grossamount = Convert.ToDouble(grosssalary);
                                grossamount = Math.Round(grossamount, 0, MidpointRounding.AwayFromZero);
                                int val = gross + 1;
                                dr[val] = Convert.ToString(grossamount);
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("DEDUCTIONS");
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString(" ");
                            dts.Rows.Add(dr);

                            foreach (DictionaryEntry items in HsCommonDeductionHead)
                            {
                                dr = dts.NewRow();
                                string key = Convert.ToString(items.Key);
                                dr[0] = Convert.ToString(items.Key);
                                double checkval = 0;
                                foreach (KeyValuePair<string, Hashtable> item in dicData)
                                {
                                    string monthName = item.Key;
                                    Hashtable htValue = item.Value;
                                    if (htValue.ContainsKey(key))
                                    {
                                        string AmountVal = Convert.ToString(htValue[key]);
                                        checkval = checkval + Convert.ToDouble(AmountVal);
                                        dr[monthName] = Convert.ToString(AmountVal);
                                    }
                                }
                                if (checkval!=0)//delsi2409
                                dts.Rows.Add(dr);
                            }


                            dr = dts.NewRow();
                            dr[0] = Convert.ToString(" ");
                            dr = dts.NewRow();
                            for (int header = 1; header < dts.Columns.Count; header++)
                            {
                                string ColumnName = Convert.ToString(dts.Columns[header].ColumnName);
                                dr[header] = Convert.ToString("---------");
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("TOTAL");
                            for (int deduct = 0; deduct < dshryear.Tables[0].Rows.Count; deduct++)
                            {
                                string deduction = Convert.ToString(dshryear.Tables[0].Rows[deduct]["deddd"]);
                                double deductionamount = Convert.ToDouble(deduction);
                                deductionamount = Math.Round(deductionamount, 0, MidpointRounding.AwayFromZero);
                                int val = deduct + 1;
                                dr[val] = Convert.ToString(deductionamount);
                            }
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dts.Rows.Add(dr);
                            dr = dts.NewRow();
                            dr[0] = Convert.ToString("NET");
                            for (int net = 0; net < dshryear.Tables[0].Rows.Count; net++)
                            {
                                string netpay = Convert.ToString(dshryear.Tables[0].Rows[net]["netsal"]);
                                double netamount = Convert.ToDouble(netpay);
                                netamount = Math.Round(netamount, 0, MidpointRounding.AwayFromZero);
                                int val = net + 1;
                                dr[val] = Convert.ToString(netamount);
                            }
                            dts.Rows.Add(dr);
                            int no_ofColumns = Convert.ToInt32(dts.Columns.Count);
                            int no_ofRows = Convert.ToInt32(dts.Rows.Count);
                            tblpayprocess = mydocument.NewTable(Fontsmall, no_ofRows, no_ofColumns, 2);
                            //  tblpayprocess.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            tblpayprocess.SetBorders(Color.Black, 1, BorderType.None);
                            tblpayprocess.VisibleHeaders = false;
                            for (int tabval = 0; tabval < dts.Rows.Count; tabval++)
                            {
                                int values = 0;
                                for (int coulumnval = 0; coulumnval < dts.Columns.Count; coulumnval++)
                                {
                                    if (coulumnval == 0)
                                    {
                                        tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontbold3);

                                        tblpayprocess.Columns[coulumnval].SetWidth(50);
                                        string RowName = Convert.ToString(dts.Rows[tabval][coulumnval]);
                                        if (RowName.ToUpper() == "GROSS" || RowName.ToUpper() == "TOTAL" || RowName.ToUpper() == "NET")
                                        {
                                            tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleRight);
                                            tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontbold2);
                                        }
                                        if (RowName.ToUpper() == "DEDUCTIONS" || RowName.ToUpper() == "EARNINGS")
                                        {
                                            tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontbold2);
                                        }
                                    }
                                    else
                                    {
                                        tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleRight);
                                        tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontsmall1);
                                        tblpayprocess.Columns[coulumnval].SetWidth(30);
                                    }
                                    if (tabval == 0 && coulumnval == values || tabval == 1 && coulumnval == values)
                                    {
                                        tblpayprocess.Cell(tabval, coulumnval).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tblpayprocess.Cell(tabval, coulumnval).SetFont(Fontbold2);
                                        tblpayprocess.Columns[coulumnval].SetWidth(30);
                                    }
                                    values++;

                                    tblpayprocess.Cell(tabval, coulumnval).SetContent(Convert.ToString(dts.Rows[tabval][coulumnval]));
                                }
                            }
                            footerdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='printpdf_footersetting' and college_code='" + Convert.ToString(collegecode) + "' and user_Code='" + Convert.ToString(Session["usercode"]) + "'");

                            if (footerdetails != "0")
                            {
                                if (footerdetails.Contains(","))
                                {
                                    string[] splitval = footerdetails.Split(',');
                                    footercontent = Convert.ToString(splitval[0]);
                                    printcontent = Convert.ToString(splitval[1]);
                                }
                                else
                                {
                                    footercontent = footerdetails;
                                    printcontent = "";
                                }
                            }
                            if (footerdetails == "0")
                            {
                                footerdetails = "";
                                printcontent = "";
                            }
                            PdfTextArea footer = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 50, mydocument.PageHeight - 160, mydocument.PageWidth, 40),
                     System.Drawing.ContentAlignment.MiddleLeft, printcontent);
                            mypdfpage.Add(footer);
                            PdfTextArea ptadate = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                            new PdfArea(mydocument, 10, mydocument.PageHeight - 110, mydocument.PageWidth, 40),
                     System.Drawing.ContentAlignment.MiddleLeft, "Date" + ":" + DateTime.Now.ToString("dd/MM/yyyy"));
                            mypdfpage.Add(ptadate);
                            PdfTextArea ptaadmin = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                                           new PdfArea(mydocument, mydocument.PageWidth - 100, mydocument.PageHeight - 110, mydocument.PageWidth, 40),
                    System.Drawing.ContentAlignment.MiddleLeft, footercontent);
                            mypdfpage.Add(ptaadmin);
                            FpMonthOverall.Sheets[0].Columns[1].Visible = true;
                            if (dvstaff.Count > 0)
                            {
                                coltop += 50;
                                tblpage = tblpayprocess.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop + 20, mydocument.PageWidth - 50, 1200));
                                mypdfpage.Add(tblpage);
                            }
                            mypdfpage.SaveToDocument();
                        }
                    }//isval
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "SalaryCertificate" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    Response.Buffer = true;
                    Response.Clear();
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode, "Original Salary Details");

        }
    }

    protected void lnk_btn_print_click(object sender, EventArgs e)
    {
        try
        {
            printpopup.Visible = true;

            string footerdetails = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='printpdf_footersetting' and college_code='" + Convert.ToString(collegecode) + "' and user_Code='" + Convert.ToString(Session["usercode"]) + "'");

            if (footerdetails.Contains(','))
            {
                string[] splitval = footerdetails.Split(',');
                txt_print.Text = Convert.ToString(splitval[0]);
                txt_certificate.Text = Convert.ToString(splitval[1]);

            }
            else
            {
                txt_print.Text = footerdetails;
                if (footerdetails == "0")
                {
                    txt_print.Text = "";
                }
            }
        }
        catch (Exception ex)
        {


        }

    }

    protected void btnsavePrint_Click(object sender, EventArgs e)
    {
        try
        {
            string GetName = Convert.ToString(txt_print.Text);

            string getcontent = Convert.ToString(txt_certificate.Text);

            if (getcontent != "")//delsi 3107
            {
                GetName = GetName + "," + getcontent;

            }

            string insquer = "if exists(select * from New_InsSettings where LinkName='printpdf_footersetting' and user_code='" + Convert.ToString(Session["usercode"]) + "' and college_code='" + Convert.ToString(collegecode) + "') update New_InsSettings set LinkValue='" + GetName + "' where LinkName='printpdf_footersetting' and user_code='" + Convert.ToString(Session["usercode"]) + "' and college_code='" + Convert.ToString(collegecode) + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('printpdf_footersetting','" + GetName + "','" + Convert.ToString(Session["usercode"]) + "','" + Convert.ToString(collegecode) + "')";

            int inscount = d2.update_method_wo_parameter(insquer, "Text");
            if (inscount > 0)
            {

                img_div1.Visible = true;
                lblsavealert.Visible = true;
                lblsavealert.Text = "Saved Successfully!";
            }
        }
        catch (Exception ex)
        {


        }
    }

    protected void btnexitPrint_Click(object sender, EventArgs e)
    {
        printpopup.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        img_div1.Visible = false;
    }
}

