using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Web.Security;
using System.Drawing;
using System.Configuration;

public partial class StaffPriority : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    string maincol = string.Empty;
    string usercode = string.Empty;
    string groupcode = string.Empty;
    int i;
    Hashtable hat = new Hashtable();
    bool flag_true = false;

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
            collegecode = Convert.ToString(Session["collegecode"]);
            usercode = Convert.ToString(Session["usercode"]);
            groupcode = Convert.ToString(Session["groupcode"]);

            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcoll.Items.Count > 0)
                {
                    maincol = Convert.ToString(ddlcoll.SelectedItem.Value);
                }
                binddept();
                designation();
                stafftype();
            }
            if (ddlcoll.Items.Count > 0)
            {
                maincol = Convert.ToString(ddlcoll.SelectedItem.Value);
            }
            lblsmserror.Visible = false;
        }
        catch (Exception ex) { }
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
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Staff Priority";
            string pagename = "StaffPriority.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch
        {

        }
    }

    protected void ddlcoll_Change(object sender, EventArgs e)
    {
        try
        {
            binddept();
            designation();
            stafftype();
            FpSpread.Visible = false;
            lblspreaderr.Visible = false;
            rprint.Visible = false;
            btnsetpriority.Visible = false;
            btnresetpriority.Visible = false;
        }
        catch
        {

        }
    }

    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        if (cb_dept.Checked == true)
        {
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                cbl_dept.Items[i].Selected = true;
            }
            txtdept.Text = "Department (" + cbl_dept.Items.Count + ")";
        }
        else
        {
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                cbl_dept.Items[i].Selected = false;
            }
            txtdept.Text = "--Select--";
        }
    }

    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        txtdept.Text = "--Select--";
        cb_dept.Checked = false;
        int count = 0;
        for (i = 0; i < cbl_dept.Items.Count; i++)
        {
            if (cbl_dept.Items[i].Selected == true)
            {
                count = count + 1;
            }
        }
        if (count > 0)
        {
            txtdept.Text = "Department (" + count + ")";
            if (count == cbl_dept.Items.Count)
            {
                cb_dept.Checked = true;
            }
        }
    }

    protected void cb_desig_CheckedChange(object sender, EventArgs e)
    {
        if (cb_desig.Checked == true)
        {
            for (i = 0; i < cbl_desig.Items.Count; i++)
            {
                cbl_desig.Items[i].Selected = true;
            }
            txtdesig.Text = "Designation (" + cbl_desig.Items.Count + ")";
        }
        else
        {
            for (i = 0; i < cbl_desig.Items.Count; i++)
            {
                cbl_desig.Items[i].Selected = false;
            }
            txtdesig.Text = "--Select--";
        }
    }

    protected void cbl_desig_SelectedIndexChange(object sender, EventArgs e)
    {
        txtdesig.Text = "--Select--";
        cb_desig.Checked = false;
        int count = 0;
        for (i = 0; i < cbl_desig.Items.Count; i++)
        {
            if (cbl_desig.Items[i].Selected == true)
            {
                count = count + 1;
            }
        }
        if (count > 0)
        {
            txtdesig.Text = "Designation (" + count + ")";
            if (count == cbl_desig.Items.Count)
            {
                cb_desig.Checked = true;
            }
        }
    }

    protected void cb_stftype_CheckedChange(object sender, EventArgs e)
    {
        if (cb_stftype.Checked == true)
        {
            for (i = 0; i < cbl_stftype.Items.Count; i++)
            {
                cbl_stftype.Items[i].Selected = true;
            }
            txtstftype.Text = "StaffType (" + cbl_stftype.Items.Count + ")";
        }
        else
        {
            for (i = 0; i < cbl_stftype.Items.Count; i++)
            {
                cbl_stftype.Items[i].Selected = false;
            }
            txtstftype.Text = "--Select--";
        }
    }

    protected void cbl_stftype_SelectedIndexChange(object sender, EventArgs e)
    {
        txtstftype.Text = "--Select--";
        cb_stftype.Checked = false;
        int count = 0;
        for (i = 0; i < cbl_stftype.Items.Count; i++)
        {
            if (cbl_stftype.Items[i].Selected == true)
            {
                count = count + 1;
            }
        }
        if (count > 0)
        {
            txtstftype.Text = "StaffType (" + count + ")";
            if (count == cbl_stftype.Items.Count)
            {
                cb_stftype.Checked = true;
            }
        }
    }

    protected void Fpspread_buttoncommand(object sender, EventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            string activerow = FpSpread.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread.ActiveSheetView.ActiveColumn.ToString();
            if (activecol == "3")
            {
                int act1 = Convert.ToInt32(activerow);
                int act2 = Convert.ToInt16(activecol);
                if (FpSpread.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
                {
                    flag_true = true;
                    FpSpread.Sheets[0].Cells[act1, act2 + 1].Text = "";
                }
                else
                {
                    flag_true = false;
                }
            }
            FpSpread.SaveChanges();
        }
        catch
        {

        }
    }

    protected void Fpspread_render(object sender, EventArgs e)
    {
        try
        {
            if (flag_true == true)
            {
                FpSpread.SaveChanges();
                string activrow = "";
                activrow = FpSpread.Sheets[0].ActiveRow.ToString();
                string activecol = FpSpread.Sheets[0].ActiveColumn.ToString();
                int actcol = Convert.ToInt16(activecol);
                int hy_order = 0;
                for (i = 0; i <= Convert.ToInt16(FpSpread.Sheets[0].RowCount) - 1; i++)
                {
                    int isval = Convert.ToInt32(FpSpread.Sheets[0].Cells[i, actcol].Value);
                    if (isval == 1)
                    {
                        hy_order++;
                        FpSpread.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;
                    }
                }
                FpSpread.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = hy_order.ToString();
            }
        }
        catch
        {

        }
    }

    protected void btnsetpriority_Click(object sender, EventArgs e)
    {
        try
        {
            alertpopwindow.Visible = true;
            string collcode = Convert.ToString(ddlcoll.SelectedValue);
            if (FpSpread.Sheets[0].Rows.Count > 0 && chkpriority.Checked)
            {
                for (i = 0; i < FpSpread.Sheets[0].Rows.Count; i++)
                {
                    string priority = Convert.ToString(FpSpread.Sheets[0].Cells[i, 4].Text.Trim());
                    string staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Text);
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        int insQ = d2.update_method_wo_parameter("update staffmaster set PrintPriority=" + priority + " where staff_code='" + staffcode + "'  and college_code=" + collcode + "", "Text");
                    }
                }
                lblalerterr.Text = "Priority Assigned";
            }
            else
            {
                lblalerterr.Text = "Priority Not Assigned";
            }
        }
        catch { lblalerterr.Text = "Priority Not Assigned"; }
    }

    protected void btnresetpriority_Click(object sender, EventArgs e)
    {
        try
        {
            if (FpSpread.Sheets[0].Rows.Count > 0 && chkpriority.Checked)
            {
                for (i = 0; i < FpSpread.Sheets[0].Rows.Count; i++)
                {
                    string collcode = Convert.ToString(ddlcoll.SelectedValue);
                    string priority = Convert.ToString(FpSpread.Sheets[0].Cells[i, 4].Text.Trim());
                    string staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Text);
                    FpSpread.Sheets[0].Cells[i, 3].Locked = false;
                    FpSpread.Sheets[0].Cells[i, 3].Value = 0;
                    FpSpread.Sheets[0].Cells[i, 4].Text = "";
                   
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        priority = "0";
                        int insQ = d2.update_method_wo_parameter("update staffmaster set PrintPriority=" + priority + " where staff_code='" + staffcode + "'  and college_code=" + collcode + "", "Text");
                    }
                }
            }
            FpSpread.SaveChanges();
        }
        catch { }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void chkpriority_change(object sender, EventArgs e)
    {
        btngo_click(sender, e);
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            if (maincol.Trim() != "")
            {
                string deptcode = "";
                string desigcode = "";
                string stftype = "";
                string collcode = Convert.ToString(ddlcoll.SelectedValue);

                if (txtdept.Text.Trim() != "--Select--")
                {
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            if (cbl_dept.Items[i].Selected == true)
                            {
                                if (deptcode.Trim() == "")
                                {
                                    deptcode = "" + Convert.ToString(cbl_dept.Items[i].Value) + "";
                                }
                                else
                                {
                                    deptcode = deptcode + "'" + "," + "'" + Convert.ToString(cbl_dept.Items[i].Value) + "";
                                }
                            }
                        }
                    }
                }

                if (txtdesig.Text.Trim() != "--Select--")
                {
                    if (cbl_desig.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_desig.Items.Count; i++)
                        {
                            if (cbl_desig.Items[i].Selected == true)
                            {
                                if (desigcode.Trim() == "")
                                {
                                    desigcode = "" + Convert.ToString(cbl_desig.Items[i].Value) + "";
                                }
                                else
                                {
                                    desigcode = desigcode + "'" + "," + "'" + Convert.ToString(cbl_desig.Items[i].Value) + "";
                                }
                            }
                        }
                    }
                }

                if (txtstftype.Text.Trim() != "--Select--")
                {
                    if (cbl_stftype.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_stftype.Items.Count; i++)
                        {
                            if (cbl_stftype.Items[i].Selected == true)
                            {
                                if (stftype.Trim() == "")
                                {
                                    stftype = "" + Convert.ToString(cbl_stftype.Items[i].Text) + "";
                                }
                                else
                                {
                                    stftype = stftype + "'" + "," + "'" + Convert.ToString(cbl_stftype.Items[i].Text) + "";
                                }
                            }
                        }
                    }
                }

                string selq = "select distinct s.staff_code,staff_name,PrintPriority,dept.dept_name,desig.desig_name from staffmaster s,stafftrans st,staff_appl_master a,hrdept_master dept,desig_master desig where s.staff_code=st.staff_code and s.appl_no=a.appl_no and st.dept_code=dept.dept_code and st.desig_code=desig.desig_code and s.college_code=dept.college_code and s.college_Code=desig.collegeCode and s.resign='0' and s.settled='0' and ISNULL(Discontinue,'0')='0' and st.latestrec='1' and s.college_Code='" + collcode + "'";

                if (deptcode.Trim() != "")
                {
                    selq = selq + " and dept.dept_code in('" + deptcode + "')";
                }
                if (deptcode.Trim() != "")
                {
                    selq = selq + " and desig.desig_code in('" + desigcode + "')";
                }
                if (stftype.Trim() != "")
                {
                    selq = selq + " and st.stftype in('" + stftype + "')";
                }

                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread.Sheets[0].RowCount = 0;
                        FpSpread.Sheets[0].ColumnCount = 0;
                        FpSpread.CommandBar.Visible = false;
                        FpSpread.Sheets[0].AutoPostBack = false;
                        FpSpread.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread.Sheets[0].RowHeader.Visible = false;
                        FpSpread.Sheets[0].ColumnCount = 5;

                        FarPoint.Web.Spread.CheckBoxCellType cbpriority = new FarPoint.Web.Spread.CheckBoxCellType();
                        cbpriority.AutoPostBack = true;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.Font.Name = "Book Antiqua";
                        darkstyle.Font.Size = FontUnit.Medium;
                        darkstyle.Font.Bold = true;
                        darkstyle.Border.BorderSize = 1;
                        darkstyle.HorizontalAlign = HorizontalAlign.Center;
                        darkstyle.VerticalAlign = VerticalAlign.Middle;
                        darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                        FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Columns[0].Locked = true;
                        FpSpread.Columns[0].Width = 75;

                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Columns[1].Locked = true;
                        if (chkpriority.Checked)
                            FpSpread.Columns[1].Width = 150;
                        else
                            FpSpread.Columns[1].Width = 100;

                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Columns[2].Locked = true;
                        if (chkpriority.Checked)
                            FpSpread.Columns[2].Width = 275;
                        else
                            FpSpread.Columns[2].Width = 225;

                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Set Priority";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Columns[3].Width = 110;

                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Columns[4].Locked = true;
                        FpSpread.Columns[4].Width = 110;

                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread.Sheets[0].RowCount++;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].CellType = cbpriority;
                            if (Convert.ToString(ds.Tables[0].Rows[i]["PrintPriority"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[i]["PrintPriority"]).Trim() != "0")//delsi0313
                            {
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Value = 1;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Locked = true;
                            }
                            else
                            {
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Value = 0;
                                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Locked = false;
                            }
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["PrintPriority"]);
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        }

                        FpSpread.Visible = true;
                        divspr.Visible = true;
                        rprint.Visible = true;
                        lblspreaderr.Visible = false;
                        FpSpread.Width = 750;
                        FpSpread.Height = 300;
                        FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                        if (chkpriority.Checked == true)
                        {
                            FpSpread.Sheets[0].Columns[3].Visible = true;
                            FpSpread.Sheets[0].AutoPostBack = false;
                            btnsetpriority.Visible = true;
                            btnresetpriority.Visible = true;
                        }
                        else
                        {
                            FpSpread.Sheets[0].Columns[3].Visible = false;
                            FpSpread.Sheets[0].AutoPostBack = true;
                            btnsetpriority.Visible = false;
                            btnresetpriority.Visible = false;
                        }
                    }
                    else
                    {
                        FpSpread.Visible = false;
                        divspr.Visible = false;
                        rprint.Visible = false;
                        lblspreaderr.Visible = true;
                        lblspreaderr.Text = "No Records Found!";
                    }
                }
                else
                {
                    FpSpread.Visible = false;
                    divspr.Visible = false;
                    rprint.Visible = false;
                    lblspreaderr.Visible = true;
                    lblspreaderr.Text = "No Records Found!";
                }
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
            ddlcoll.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcoll.Enabled = true;
                ddlcoll.DataSource = ds;
                ddlcoll.DataTextField = "collname";
                ddlcoll.DataValueField = "college_code";
                ddlcoll.DataBind();
            }
            //  ddlcollege.Items.Insert(0, "---Select---");
        }
        catch (Exception e)
        {

        }
    }

    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
          //  string item = "select distinct Dept_Code,Dept_Name from Department where college_code = '" + maincol + "'";
            string item = "select distinct dept_code,dept_name from hrdept_master where college_code = '" + maincol + "' order by dept_name";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "dept_name";
                cbl_dept.DataValueField = "dept_code";
                cbl_dept.DataBind();
                if (cbl_dept.Items.Count > 0)
                {
                    for (i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        cbl_dept.Items[i].Selected = true;
                    }
                    txtdept.Text = "Department (" + cbl_dept.Items.Count + ")";
                    cb_dept.Checked = true;
                }
            }
            else
            {
                txtdept.Text = "--Select--";
                cb_dept.Checked = false;
            }

        }
        catch
        {

        }
    }
    protected void designation()
    {
        //string dept = "";
        //for (int i = 0; i < cbl_dept.Items.Count; i++)
        //{
        //    if (cbl_dept.Items[i].Selected == true)
        //    {
        //        if (dept == "")
        //        {
        //            dept = "" + cbl_dept.Items[i].Value.ToString() + "";
        //        }
        //        else
        //        {
        //            dept = dept + "'" + "," + "'" + cbl_dept.Items[i].Value.ToString() + "";
        //        }
        //    }
        //}
        ds.Clear();
        cbl_desig.Items.Clear();
        string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + maincol + "'";
        // where dept_code in ('" + dept + "')
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
                for (i = 0; i < cbl_desig.Items.Count; i++)
                {
                    cbl_desig.Items[i].Selected = true;
                }
                txtdesig.Text = "Designation (" + cbl_desig.Items.Count + ")";
                cb_desig.Checked = true;
            }
        }
        else
        {
            txtdesig.Text = "--Select--";
            cb_desig.Checked = false;
        }
    }

    protected void stafftype()
    {
        try
        {
            ds.Clear();
            cbl_stftype.Items.Clear();
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + maincol + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stftype.DataSource = ds;
                cbl_stftype.DataTextField = "stftype";
                //cbl_stype.DataValueField = "Dept_Code";
                cbl_stftype.DataBind();
                if (cbl_stftype.Items.Count > 0)
                {
                    for (i = 0; i < cbl_stftype.Items.Count; i++)
                    {
                        cbl_stftype.Items[i].Selected = true;
                    }
                    txtstftype.Text = "StaffType (" + cbl_stftype.Items.Count + ")";
                    cb_stftype.Checked = true;
                }
            }
            else
            {
                txtstftype.Text = "--Select--";
                cb_stftype.Checked = false;
            }

        }
        catch
        {

        }
    }
}