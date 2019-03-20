using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class StaffSalarydetails : System.Web.UI.Page
{
    #region "Basic Details"
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string group_user = "";
    string collegecode = "";

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
            collegecode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                BindYear();
                BindDept();
                BindDesignation();
                BindCategory();
                BindStaffType();
                clear();
            }
        }
        catch (Exception ex) { }
    }
    public void clear()
    {
        lblexcel.Visible = false;
        txtexcel.Visible = false;
        btnexcel.Visible = false;
        txtexcel.Text = "";
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        errmsg.Visible = false;
        fpsalary.Visible = false;
    }
    public void BindYear()
    {
        try
        {
            ddlyear.Items.Clear();
            string strquery = d2.GetFunction(" select top 1 year(join_date) from staffmaster order by join_date");
            if (strquery.Trim() != "" && strquery != null)
            {
                int sy = Convert.ToInt32(strquery);
                int ty = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                for (int sys = ty; sys >= sy; sys--)
                {
                    ddlyear.Items.Add(sys.ToString());
                }
            }
        }
        catch
        {
        }
    }

    public void BindDept()
    {
        try
        {
            txtdept.Text = "---Select---";
            chkdept.Checked = false;
            chklsdept.Items.Clear();
            ds.Clear();
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
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') ";
            }
            if (deptquery != "")
            {
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                chklsdept.DataSource = ds.Tables[0];
                chklsdept.DataTextField = "dept_name";
                chklsdept.DataValueField = "dept_code";
                chklsdept.DataBind();
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < chklsdept.Items.Count; i++)
                {
                    chklsdept.Items[i].Selected = true;
                }
                txtdept.Text = "Dept(" + chklsdept.Items.Count.ToString() + ")";
                chkdept.Checked = true;
            }
        }
        catch
        {
        }
    }

    public void BindCategory()
    {
        try
        {
            txtcategory.Text = "---Select---";
            chkcategory.Checked = false;
            chklscategory.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("select  distinct category_code,category_name from staffcategorizer where college_code='" + Session["collegecode"] + "' order by category_code", "Text");
            chklscategory.DataSource = ds.Tables[0];
            chklscategory.DataTextField = "category_name";
            chklscategory.DataValueField = "category_code";
            chklscategory.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < chklscategory.Items.Count; i++)
                {
                    chklscategory.Items[i].Selected = true;
                }
                txtcategory.Text = "Category (" + chklscategory.Items.Count.ToString() + ")";
                chkcategory.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void BindStaffType()
    {
        txtstafftype.Text = "---Select---";
        chkstafftype.Checked = false;
        chklsstafftype.Items.Clear();
        ds.Clear();
        ds = d2.select_method_wo_parameter("select distinct stftype from stafftrans st, staffmaster sm where st.staff_code=sm.staff_code and college_code='" + Session["collegecode"] + "' order by stftype desc", "Text");
        chklsstafftype.DataSource = ds.Tables[0];
        chklsstafftype.DataTextField = "stftype";
        chklsstafftype.DataValueField = "stftype";
        chklsstafftype.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < chklsstafftype.Items.Count; i++)
            {
                chklsstafftype.Items[i].Selected = true;
            }
            txtstafftype.Text = "Staff Type(" + chklsstafftype.Items.Count.ToString() + ")";
            chkstafftype.Checked = true;
        }
    }

    public void BindDesignation()
    {
        txtdesgination.Text = "---Select---";
        chkdesgination.Checked = false;
        ds = d2.binddesi(collegecode);
        chklsdesgination.DataSource = ds;
        chklsdesgination.DataValueField = "desig_code";
        chklsdesgination.DataTextField = "desig_name";
        chklsdesgination.DataBind();
        for (int i = 0; i < chklsdesgination.Items.Count; i++)
        {
            chklsdesgination.Items[i].Selected = true;
        }
        if (chklsdesgination.Items.Count > 0)
        {
            txtdesgination.Text = "Designation (" + chklsdesgination.Items.Count + ")";
            chkdesgination.Checked = true;
        }
    }

    protected void chkdept_CheckedChanged(object sender, EventArgs e)
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
            txtdept.Text = "---Select---";
        }
    }
    protected void chklsdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        txtdept.Text = "---Select---";
        chkdept.Checked = false;
        int cou = 0;
        for (int i = 0; i < chklsdept.Items.Count; i++)
        {
            if (chklsdept.Items[i].Selected == true)
            {
                cou++;
            }
        }
        if (cou > 0)
        {
            txtdept.Text = "Dept (" + cou + ")";
            if (cou == chklsdept.Items.Count)
            {
                chkdept.Checked = true;
            }
        }
    }

    protected void chkdesgination_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (chkdesgination.Checked == true)
        {
            for (int i = 0; i < chklsdesgination.Items.Count; i++)
            {
                chklsdesgination.Items[i].Selected = true;
            }
            txtdesgination.Text = "Designation (" + chklsdept.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsdesgination.Items.Count; i++)
            {
                chklsdesgination.Items[i].Selected = false;
            }
            txtdesgination.Text = "---Select---";
        }
    }
    protected void chklsdesgination_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        txtdesgination.Text = "---Select---";
        chkdesgination.Checked = false;
        int cou = 0;
        for (int i = 0; i < chklsdesgination.Items.Count; i++)
        {
            if (chklsdesgination.Items[i].Selected == true)
            {
                cou++;
            }
        }
        if (cou > 0)
        {
            txtdesgination.Text = "Designation (" + cou + ")";
            if (cou == chklsdesgination.Items.Count)
            {
                chkdesgination.Checked = true;
            }
        }
    }

    protected void chkcategory_CheckedChanged(object sender, EventArgs e)
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
            txtcategory.Text = "---Select---";
        }
    }
    protected void chklscategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        txtcategory.Text = "---Select---";
        chkcategory.Checked = false;
        int cou = 0;
        for (int i = 0; i < chklscategory.Items.Count; i++)
        {
            if (chklscategory.Items[i].Selected == true)
            {
                cou++;
            }
        }
        if (cou > 0)
        {
            txtcategory.Text = "Category (" + cou + ")";
            if (cou == chklscategory.Items.Count)
            {
                chkcategory.Checked = true;
            }
        }
    }

    protected void chkstafftype_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (chkstafftype.Checked == true)
        {
            for (int i = 0; i < chklsstafftype.Items.Count; i++)
            {
                chklsstafftype.Items[i].Selected = true;
            }
            txtstafftype.Text = "Staff Type (" + chklsstafftype.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsstafftype.Items.Count; i++)
            {
                chklsstafftype.Items[i].Selected = false;
            }
            txtstafftype.Text = "---Select---";
        }
    }
    protected void chklsstafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        txtstafftype.Text = "---Select---";
        chkstafftype.Checked = false;
        int cou = 0;
        for (int i = 0; i < chklsstafftype.Items.Count; i++)
        {
            if (chklsstafftype.Items[i].Selected == true)
            {
                cou++;
            }
        }
        if (cou > 0)
        {
            txtstafftype.Text = "Staff Type (" + cou + ")";
            if (cou == chklsstafftype.Items.Count)
            {
                chkstafftype.Checked = true;
            }
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "NEW ";
        if (ddltype.Text.ToString() == "Relived Staff List")
        {
            degreedetails = "RELIEVED ";
        }

        degreedetails = degreedetails + " STAFF LIST FOR THE MONTH OF " + ddlmonth.SelectedItem.ToString().ToUpper() + " - " + ddlyear.SelectedItem.ToString();
        string pagename = "staffsalarydetails.aspx";
        Printcontrol.loadspreaddetails(fpsalary, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(fpsalary, reportname);
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
        }
    }
    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            fpsalary.CommandBar.Visible = false;
            fpsalary.Sheets[0].RowCount = 0;
            fpsalary.Sheets[0].ColumnCount = 0;
            fpsalary.Sheets[0].SheetCorner.ColumnCount = 0;
            fpsalary.Sheets[0].ColumnCount = 15;
            fpsalary.Sheets[0].ColumnHeader.RowCount = 1;

            fpsalary.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;

            fpsalary.Sheets[0].SheetName = " ";
            fpsalary.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            fpsalary.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            fpsalary.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            fpsalary.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fpsalary.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fpsalary.Sheets[0].DefaultStyle.Font.Bold = false;
            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = Color.Black;
            style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fpsalary.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            fpsalary.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            fpsalary.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            fpsalary.Sheets[0].AllowTableCorner = true;
            fpsalary.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            fpsalary.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            fpsalary.Pager.Align = HorizontalAlign.Right;
            fpsalary.Pager.Font.Bold = true;
            fpsalary.Pager.Font.Name = "Book Antiqua";
            fpsalary.Pager.ForeColor = Color.DarkGreen;
            fpsalary.Pager.BackColor = Color.Beige;
            fpsalary.Pager.BackColor = Color.AliceBlue;
            fpsalary.Pager.PageCount = 5;
            fpsalary.CommandBar.Visible = false;

            fpsalary.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Category";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Type";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Date Of Joining";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 8].Text = "PF Join Date";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Date Of Relive";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Father/Husband Name";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Date Of Birth";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Age";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 13].Text = "PF A/C Number";
            fpsalary.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Remarks";

            fpsalary.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpsalary.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Left;
            fpsalary.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Left;

            fpsalary.Sheets[0].Columns[0].Width = 50;
            fpsalary.Sheets[0].Columns[1].Width = 200;
            fpsalary.Sheets[0].Columns[2].Width = 200;
            fpsalary.Sheets[0].Columns[3].Width = 200;
            fpsalary.Sheets[0].Columns[4].Width = 200;
            fpsalary.Sheets[0].Columns[5].Width = 200;
            fpsalary.Sheets[0].Columns[6].Width = 200;
            fpsalary.Sheets[0].Columns[7].Width = 80;
            fpsalary.Sheets[0].Columns[8].Width = 80;
            fpsalary.Sheets[0].Columns[9].Width = 80;
            fpsalary.Sheets[0].Columns[10].Width = 200;
            fpsalary.Sheets[0].Columns[11].Width = 80;
            fpsalary.Sheets[0].Columns[12].Width = 200;
            fpsalary.Sheets[0].Columns[13].Width = 80;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            fpsalary.Sheets[0].Columns[0].CellType = txt;
            fpsalary.Sheets[0].Columns[1].CellType = txt;
            fpsalary.Sheets[0].Columns[2].CellType = txt;
            fpsalary.Sheets[0].Columns[3].CellType = txt;
            fpsalary.Sheets[0].Columns[4].CellType = txt;
            fpsalary.Sheets[0].Columns[5].CellType = txt;
            fpsalary.Sheets[0].Columns[6].CellType = txt;
            fpsalary.Sheets[0].Columns[7].CellType = txt;
            fpsalary.Sheets[0].Columns[8].CellType = txt;
            fpsalary.Sheets[0].Columns[9].CellType = txt;
            fpsalary.Sheets[0].Columns[10].CellType = txt;
            fpsalary.Sheets[0].Columns[11].CellType = txt;
            fpsalary.Sheets[0].Columns[12].CellType = txt;
            fpsalary.Sheets[0].Columns[13].CellType = txt;

            fpsalary.Width = 1200;

            string strdeptcode = "";
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    if (strdeptcode == "")
                    {
                        strdeptcode = "'" + chklsdept.Items[i].Value + "'";
                    }
                    else
                    {
                        strdeptcode = strdeptcode + ",'" + chklsdept.Items[i].Value + "'";
                    }
                }
            }
            if (strdeptcode.Trim() != "")
            {
                strdeptcode = " and st.dept_code in(" + strdeptcode + ")";
            }

            string strdesignquery = "";
            for (int i = 0; i < chklsdesgination.Items.Count; i++)
            {
                if (chklsdesgination.Items[i].Selected == true)
                {
                    if (strdesignquery == "")
                    {
                        strdesignquery = "'" + chklsdesgination.Items[i].Value + "'";
                    }
                    else
                    {
                        strdesignquery = strdesignquery + ",'" + chklsdesgination.Items[i].Value + "'";
                    }
                }
            }
            if (strdesignquery.Trim() != "")
            {
                strdesignquery = " and st.desig_code in(" + strdesignquery + ")";
            }

            string strcatequery = "";
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    if (strcatequery == "")
                    {
                        strcatequery = "'" + chklscategory.Items[i].Value + "'";
                    }
                    else
                    {
                        strcatequery = strcatequery + ",'" + chklscategory.Items[i].Value + "'";
                    }
                }
            }
            if (strcatequery.Trim() != "")
            {
                strcatequery = " and st.category_code in(" + strcatequery + ")";
            }

            string strtypequery = "";
            for (int i = 0; i < chklsstafftype.Items.Count; i++)
            {
                if (chklsstafftype.Items[i].Selected == true)
                {
                    if (strtypequery == "")
                    {
                        strtypequery = "'" + chklsstafftype.Items[i].Value + "'";
                    }
                    else
                    {
                        strtypequery = strtypequery + ",'" + chklsstafftype.Items[i].Value + "'";
                    }
                }
            }
            if (strtypequery.Trim() != "")
            {
                strtypequery = " and st.stftype in(" + strtypequery + ")";
            }

            string strquery = "select s.staff_code,LEN(s.staff_code),s.staff_name,sa.father_name,sa.husband_name,sa.sex,convert(nvarchar(15),sa.date_of_birth,103) dob,convert(nvarchar(15),s.relieve_date,103) relivedate,convert(nvarchar(15),s.join_date,103) joindate,h.dept_name,d.desig_name,sc.category_name,st.stftype,s.Is_PF,s.pfnumber from staffmaster s,stafftrans st,hrdept_master h,desig_master d,staffcategorizer sc,staff_appl_master sa  where sa.appl_no=s.appl_no and s.staff_code=st.staff_code and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.category_code=sc.category_code and st.latestrec=1 ";
            if (ddltype.Text.ToString() == "New Staff List")
            {
                fpsalary.Sheets[0].Columns[9].Visible = false;
                fpsalary.Sheets[0].Columns[7].Visible = true;
                fpsalary.Sheets[0].Columns[8].Visible = true;
                fpsalary.Sheets[0].Columns[11].Visible = true;
                fpsalary.Sheets[0].Columns[12].Visible = true;
                strquery = strquery + " and MONTH(s.join_date) = '" + ddlmonth.SelectedValue.ToString() + "' AND YEAR(s.join_date) = '" + ddlyear.SelectedItem.ToString() + "'";
            }
            else
            {
                fpsalary.Sheets[0].Columns[9].Visible = true;
                fpsalary.Sheets[0].Columns[7].Visible = false;
                fpsalary.Sheets[0].Columns[8].Visible = false;
                fpsalary.Sheets[0].Columns[11].Visible = false;
                fpsalary.Sheets[0].Columns[12].Visible = false;
                strquery = strquery + " and MONTH(s.relieve_date) = '" + ddlmonth.SelectedValue.ToString() + "' AND YEAR(s.relieve_date) = '" + ddlyear.SelectedItem.ToString() + "' and ((s.resign=1 and s.settled=1) or (s.Discontinue=1))";
            }
            strquery = strquery + strdeptcode + strdesignquery + strcatequery + strtypequery + " order by s.join_date,LEN(s.staff_code),s.staff_code,s.staff_name";
            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                fpsalary.Visible = true;
                lblexcel.Visible = true;
                txtexcel.Visible = true;
                btnprintmaster.Visible = true;
                btnexcel.Visible = true;
                int srno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    string staffcode = ds.Tables[0].Rows[i]["staff_code"].ToString();
                    string name = ds.Tables[0].Rows[i]["staff_name"].ToString();
                    string dob = ds.Tables[0].Rows[i]["dob"].ToString();
                    string jdate = ds.Tables[0].Rows[i]["joindate"].ToString();
                    string deptname = ds.Tables[0].Rows[i]["dept_name"].ToString();
                    string designname = ds.Tables[0].Rows[i]["desig_name"].ToString();
                    string catname = ds.Tables[0].Rows[i]["category_name"].ToString();
                    string stftypenme = ds.Tables[0].Rows[i]["stftype"].ToString();
                    string fname = ds.Tables[0].Rows[i]["father_name"].ToString();
                    string hname = ds.Tables[0].Rows[i]["husband_name"].ToString();
                    if (hname.Trim() != "" && hname == null)
                    {
                        fname = hname;
                    }
                    string pfnumber = ds.Tables[0].Rows[i]["pfnumber"].ToString();
                    string relivedate = ds.Tables[0].Rows[i]["relivedate"].ToString();
                    string age = "";
                    string[] spdate = dob.Split('/');
                    if (spdate.GetUpperBound(0) == 2)
                    {
                        int curemonth = 0, cureyear;
                        DateTime dtdob = Convert.ToDateTime(spdate[1] + '/' + spdate[0] + '/' + spdate[2]);
                        DateTime dtnow = DateTime.Now;
                        int cury = Convert.ToInt32(dtnow.ToString("yyyy"));
                        int jyear = Convert.ToInt32(dtdob.ToString("yyyy"));
                        cureyear = cury - jyear;

                        int curmon = Convert.ToInt32(dtnow.ToString("MM"));
                        int jmon = Convert.ToInt32(dtdob.ToString("MM"));
                        if (curmon < jmon)
                        {
                            curemonth = (curmon + 12) - jmon;
                            cureyear--;
                        }
                        else
                        {
                            curemonth = curmon - jmon;
                        }
                        age = cureyear.ToString() + " Years ";
                        if (curemonth > 0)
                        {
                            age = age + curemonth + " Months";
                        }
                    }

                    fpsalary.Sheets[0].RowCount++;
                    srno++;
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 1].Text = staffcode.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 2].Text = name.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 3].Text = deptname.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 4].Text = designname.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 5].Text = catname.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 6].Text = stftypenme.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 7].Text = jdate.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 8].Text = jdate.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 9].Text = relivedate.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 10].Text = fname.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 11].Text = dob.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 12].Text = age.ToString();
                    fpsalary.Sheets[0].Cells[fpsalary.Sheets[0].RowCount - 1, 13].Text = pfnumber.ToString();
                }
            }
            else
            {
                errmsg.Text = "No Records Found";
                errmsg.Visible = true;
            }
            fpsalary.Sheets[0].PageSize = fpsalary.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
            d2.sendErrorMail(ex, "13", "StaffSalarydetails.aspx");
        }
    }
}