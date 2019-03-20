using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class Staff_FingerPrintReg : System.Web.UI.Page
{
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string collcode = string.Empty;

    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    int ik = 0;

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

            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcoll.Items.Count > 0)
                {
                    collcode = Convert.ToString(ddlcoll.SelectedValue);
                }
                binddept();
                staffcategory();
                stafflist();
            }
            if (ddlcoll.Items.Count > 0)
            {
                collcode = Convert.ToString(ddlcoll.SelectedValue);
            }
            lblsmserror.Visible = false;
            lblerr.Visible = false;
            lblpoperr.Visible = false;
        }
        catch (Exception ex) { }
    }

    private void bindcollege()
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
            ddlpopclg.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcoll.Enabled = true;
                ddlcoll.DataSource = ds;
                ddlcoll.DataTextField = "collname";
                ddlcoll.DataValueField = "college_code";
                ddlcoll.DataBind();

                ddlpopclg.Enabled = true;
                ddlpopclg.DataSource = ds;
                ddlpopclg.DataTextField = "collname";
                ddlpopclg.DataValueField = "college_code";
                ddlpopclg.DataBind();
            }
        }
        catch (Exception e) { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetMacID(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct MachineNo from DeviceInfo where College_Code='" + collcode + "' and MachineNo like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    private void bindpopdept()
    {
        try
        {
            ds.Clear();
            ddldept.Items.Clear();
            string collcode = Convert.ToString(ddlpopclg.SelectedValue);
            string item = "select distinct dept_code,dept_name from hrdept_master where college_code = '" + collcode + "' order by dept_name";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldept.DataSource = ds;
                ddldept.DataTextField = "dept_name";
                ddldept.DataValueField = "dept_code";
                ddldept.DataBind();
                ddldept.Items.Insert(0, "All");
            }
            else
            {
                ddldept.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    private void staffpopcategory()
    {
        try
        {
            ds.Clear();
            ddlstaffcat.Items.Clear();
            string collcode = Convert.ToString(ddlpopclg.SelectedValue);
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + collcode + "' order by category_Name";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstaffcat.DataSource = ds;
                ddlstaffcat.DataTextField = "category_Name";
                ddlstaffcat.DataValueField = "category_code";
                ddlstaffcat.DataBind();
                ddlstaffcat.Items.Insert(0, "All");
            }
            else
            {
                ddlstaffcat.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    private void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            ddldept.Items.Clear();
            string item = "select distinct dept_code,dept_name from hrdept_master where college_code = '" + collcode + "' order by dept_name";
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
                    txtdept.Text = "Department (" + cbl_dept.Items.Count + ")";
                    cb_dept.Checked = true;
                }

                ddldept.DataSource = ds;
                ddldept.DataTextField = "dept_name";
                ddldept.DataValueField = "dept_code";
                ddldept.DataBind();
                ddldept.Items.Insert(0, "All");
            }
            else
            {
                txtdept.Text = "--Select--";
                cb_dept.Checked = false;
                ddldept.Items.Insert(0, "Select");
            }
            stafflist();
        }
        catch { }
    }

    private void staffcategory()
    {
        try
        {
            ds.Clear();
            cbl_staffc.Items.Clear();
            ddlstaffcat.Items.Clear();
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + collcode + "' order by category_Name";
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

                ddlstaffcat.DataSource = ds;
                ddlstaffcat.DataTextField = "category_Name";
                ddlstaffcat.DataValueField = "category_code";
                ddlstaffcat.DataBind();
                ddlstaffcat.Items.Insert(0, "All");
            }
            else
            {
                txt_staffc.Text = "--Select--";
                cb_staffc.Checked = false;
                ddlstaffcat.Items.Insert(0, "Select");
            }
            stafflist();
        }
        catch { }
    }

    private void stafflist()
    {
        try
        {
            ds.Clear();
            ddlstafflst.Items.Clear();
            string dept = "";
            string catcode = "";

            dept = GetSelectedItemsValueAsString(cbl_dept);

            catcode = GetSelectedItemsValueAsString(cbl_staffc);

            string selq = "select (s.staff_name+' - '+s.staff_Code) as Staff,s.staff_code from staffmaster s,stafftrans st,staffcategorizer sc,hrdept_master h where s.staff_code=st.staff_code and latestrec='1' and sc.category_code=st.category_code and h.dept_code=st.dept_Code and s.college_code=sc.college_code and s.college_code=h.college_code and st.category_code in('" + catcode + "') and st.dept_code in('" + dept + "') and s.college_code='" + collcode + "'";  //and s.resign='0' and s.settled='0' 
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlstafflst.DataSource = ds;
                ddlstafflst.DataTextField = "Staff";
                ddlstafflst.DataValueField = "staff_code";
                ddlstafflst.DataBind();
                ddlstafflst.Items.Insert(0, "Select");
            }
            else
            {
                ddlstafflst.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    protected void ddlcoll_Change(object sender, EventArgs e)
    {
        binddept();
        staffcategory();
        stafflist();
        txt_macid.Text = "";
        lblerr.Visible = false;
        txt_macid_Change(sender, e);
        FpSpread.Sheets[0].Rows.Count = 0;
        FpSpread.Visible = false;
        btnsave.Visible = false;
    }

    protected void ddlpopclg_change(object sender, EventArgs e)
    {
        bindpopdept();
        staffpopcategory();
        Fpspreadpop.Visible = false;
        lblpoperr.Visible = false;
        rprint.Visible = false;
        divpopspr.Visible = false;
        chkincrel.Checked = false;
        rbfingerid.Checked = true;
        rbnofingerid.Checked = false;
    }

    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txtdept, "Department");
        stafflist();
    }

    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txtdept, "Department");
        stafflist();
    }

    protected void cb_staffc_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_staffc, cbl_staffc, txt_staffc, "Category");
        stafflist();
    }

    protected void cbl_staffc_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_staffc, cbl_staffc, txt_staffc, "Category");
        stafflist();
    }

    protected void txt_macid_Change(object sender, EventArgs e)
    {
        try
        {
            ddlfingerid.Items.Clear();
            int txtval = 0;
            Int32.TryParse(txt_macid.Text.Trim(), out txtval);
            if (txt_macid.Text.Trim() != "" || txtval != 0)
            {
                //Cmd By SaranyaDevi 16.4.2018

                //string selq = "select distinct cast(Enrollno as bigint) as Enrollno from bio..enrollments where Branchid='" + txt_macid.Text.Trim() + "' order by cast(Enrollno as bigint) asc";

                //Added By Saranyadevi 16.4.2018
                string selq = "select distinct cast(Enrollno as varchar) as Enrollno from bio..enrollments where Branchid='" + txt_macid.Text.Trim() + "' order by cast(Enrollno as varchar) asc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlfingerid.DataSource = ds;
                    ddlfingerid.DataTextField = "Enrollno";
                    ddlfingerid.DataValueField = "Enrollno";
                    ddlfingerid.DataBind();
                    ddlfingerid.Items.Insert(0, "Select");
                }
                else
                {
                    ddlfingerid.Items.Insert(0, "Select");
                }
            }
            else
            {
                ddlfingerid.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    protected void ddlstafflst_change(object sender, EventArgs e)
    {
        txt_macid.Text = "";
        txt_macid_Change(sender, e);
    }

    protected bool checkstaffcode()
    {
        bool chkspr = true;
        string staffcode = Convert.ToString(ddlstafflst.SelectedValue);
        string sprstaffcode = "";
        FpSpread.SaveChanges();
        try
        {
            for (ik = 0; ik < FpSpread.Sheets[0].RowCount; ik++)
            {
                sprstaffcode = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 1].Text);
                if (staffcode == sprstaffcode)
                    chkspr = false;
            }
        }
        catch { }
        return chkspr;
    }

    protected bool checkvalue()
    {
        bool chkspr = true;
        string sprstaffcode = "";
        FpSpread.SaveChanges();
        try
        {
            for (ik = 0; ik < FpSpread.Sheets[0].RowCount; ik++)
            {
                sprstaffcode = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 0].Text);
                if (sprstaffcode.Trim() == "")
                    chkspr = false;
            }
        }
        catch { }
        return chkspr;
    }

    private void loadsprcolumns()
    {
        try
        {
            FpSpread.Sheets[0].RowCount = 0;
            FpSpread.Sheets[0].ColumnCount = 5;
            FpSpread.CommandBar.Visible = false;
            FpSpread.RowHeader.Visible = false;
            FpSpread.Sheets[0].AutoPostBack = false;
            FpSpread.Sheets[0].ColumnHeader.RowCount = 1;

            FarPoint.Web.Spread.TextCellType textcell = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread.Columns[0].Locked = true;
            FpSpread.Columns[0].Width = 50;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread.Columns[1].Locked = true;
            FpSpread.Columns[1].Width = 150;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread.Columns[2].Locked = true;
            FpSpread.Columns[2].Width = 300;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Device ID";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread.Columns[3].Locked = true;
            FpSpread.Columns[3].Width = 150;

            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Finger ID";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].CellType = textcell;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread.Columns[4].Locked = true;
            FpSpread.Columns[4].Width = 150;
        }
        catch { }
    }

    private void loadrepsprcolumns()
    {
        try
        {
            Fpspreadpop.Sheets[0].RowCount = 0;
            Fpspreadpop.Sheets[0].ColumnCount = 7;
            Fpspreadpop.CommandBar.Visible = false;
            Fpspreadpop.RowHeader.Visible = false;
            Fpspreadpop.Sheets[0].AutoPostBack = false;
            Fpspreadpop.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspreadpop.Sheets[0].FrozenRowCount = 1;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Fpspreadpop.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[0].Locked = true;
            Fpspreadpop.Columns[0].Width = 50;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[1].Width = 75;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[2].Locked = true;
            if (rbnofingerid.Checked == true)
                Fpspreadpop.Columns[2].Width = 165;
            else
                Fpspreadpop.Columns[2].Width = 115;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[3].Locked = true;
            if (rbnofingerid.Checked == true)
                Fpspreadpop.Columns[3].Width = 215;
            else
                Fpspreadpop.Columns[3].Width = 165;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[4].Locked = true;
            if (rbnofingerid.Checked == true)
                Fpspreadpop.Columns[4].Width = 240;
            else
                Fpspreadpop.Columns[4].Width = 190;

            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[5].Locked = true;
            if (rbnofingerid.Checked == true)
                Fpspreadpop.Columns[5].Width = 240;
            else
                Fpspreadpop.Columns[5].Width = 190;


            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Finger ID";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            Fpspreadpop.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            Fpspreadpop.Columns[6].Locked = true;
            Fpspreadpop.Columns[6].Width = 115;

            if (rbnofingerid.Checked == true)
            {
                Fpspreadpop.Columns[1].Visible = false;
                Fpspreadpop.Columns[6].Visible = false;
                btndelete.Visible = false;
            }
            else
            {
                Fpspreadpop.Columns[1].Visible = true;
                Fpspreadpop.Columns[6].Visible = true;
                btndelete.Visible = true;
            }
        }
        catch { }
    }

    protected void btnmatch_click(object sender, EventArgs e)
    {
        try
        {
            string getnamecode = Convert.ToString(ddlstafflst.SelectedItem.Text);
            string staffname = "";
            if (getnamecode.Trim() != "Select")
                staffname = getnamecode.Split('-')[0];
            if ((FpSpread.Sheets[0].RowCount == 3 && checkvalue() == false) || FpSpread.Sheets[0].RowCount == 0)
                loadsprcolumns();

            if (checkstaffcode() == false)
            {
                lblerr.Visible = true;
                lblerr.Text = "Staff Already Exists!";
                return;
            }
            else if (ddlstafflst.SelectedIndex == 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Select Staff!";
                return;
            }
            else if (txt_macid.Text.Trim() == "")
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Enter MachineID!";
                return;
            }
            else if (ddlfingerid.SelectedIndex == 0)
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Select FingerID!";
                return;
            }
            else
            {
                FarPoint.Web.Spread.TextCellType textcell = new FarPoint.Web.Spread.TextCellType();
                FpSpread.Sheets[0].RowCount++;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread.Sheets[0].RowCount);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ddlstafflst.SelectedValue);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(staffname);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(txt_macid.Text);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].CellType = textcell;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ddlfingerid.SelectedValue);
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                FpSpread.Visible = true;
                lblerr.Visible = false;
                btnsave.Visible = true;
            }
        }
        catch { }
    }

    protected void btnsave_click(object sender, EventArgs e)
    {
        try
        {
            if (checkvalue() == true && FpSpread.Sheets[0].RowCount > 0)
            {
                string staffcode = "";
                string fingerid = "";
                string deviceid = "";
                string collcode = Convert.ToString(ddlcoll.SelectedItem.Value);
                string updq = "";
                int upcount = 0;
                for (ik = 0; ik < FpSpread.Sheets[0].RowCount; ik++)
                {
                    staffcode = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 1].Text);
                    fingerid = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 4].Text);
                    deviceid = Convert.ToString(FpSpread.Sheets[0].Cells[ik, 3].Text);
                    updq = "update staffmaster set Fingerprint1='" + fingerid + "',DeviceID='" + deviceid + "' where staff_code='" + staffcode + "'";
                    int inscount = d2.update_method_wo_parameter(updq, "Text");
                    if (inscount > 0)
                        upcount++;
                }
                if (upcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Saved Successfully!";
                }
            }
        }
        catch { }
    }

    protected void Fpspreadpop_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Fpspreadpop.SaveChanges();
        try
        {
            byte check = Convert.ToByte(Fpspreadpop.Sheets[0].Cells[0, 1].Value);
            if (check == 1)
            {
                for (ik = 1; ik < Fpspreadpop.Sheets[0].RowCount; ik++)
                {
                    Fpspreadpop.Sheets[0].Cells[ik, 1].Value = 1;
                }
            }
            else
            {
                for (ik = 1; ik < Fpspreadpop.Sheets[0].RowCount; ik++)
                {
                    Fpspreadpop.Sheets[0].Cells[ik, 1].Value = 0;
                }
            }
        }
        catch { }
    }

    private bool checkedspr()
    {
        bool ok = false;
        Fpspreadpop.SaveChanges();
        try
        {
            for (ik = 0; ik < Fpspreadpop.Sheets[0].RowCount; ik++)
            {
                byte check = Convert.ToByte(Fpspreadpop.Sheets[0].Cells[ik, 1].Value);
                if (check == 1)
                    ok = true;
            }
        }
        catch { }
        return ok;
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkedspr())
            {
                lblpoperr.Visible = false;
                Fpspreadpop.SaveChanges();
                string delq = "";
                int delcount = 0;
                for (ik = 0; ik < Fpspreadpop.Sheets[0].RowCount; ik++)
                {
                    byte check = Convert.ToByte(Fpspreadpop.Sheets[0].Cells[ik, 1].Value);
                    if (check == 1)
                    {
                        string staffcode = Convert.ToString(Fpspreadpop.Sheets[0].Cells[ik, 2].Text);
                        delq = "update staffmaster set Fingerprint1='' where staff_code='" + staffcode + "'";
                        int upcount = d2.update_method_wo_parameter(delq, "Text");
                        if (upcount > 0)
                            delcount++;
                    }
                }
                if (delcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Deleted Successfully!";
                }
            }
            else
            {
                lblpoperr.Visible = true;
                lblpoperr.Text = "Please Select Any Staff!";
            }
        }
        catch { }
    }

    protected void btnreport_click(object sender, EventArgs e)
    {
        poperrjs.Visible = true;
        bindcollege();
        bindpopdept();
        staffpopcategory();
    }

    protected void imagebtnpopcloseadd_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }

    protected void btnexit_click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            string selq = "";
            string collcode = Convert.ToString(ddlpopclg.SelectedItem.Value);

            if (ddldept.SelectedItem.Text != "Select" && ddlstaffcat.SelectedItem.Text != "Select")
            {
                if (ddldept.SelectedItem.Text == "All" && ddlstaffcat.SelectedItem.Text == "All")
                {
                    selq = " select sm.staff_code,staff_name,Fingerprint1,dept_name,desig_name,Fingerprint1 as MachineNo from staffmaster sm,hrdept_master h,desig_master d,stafftrans st,staffcategorizer sc where sm.staff_code=st.staff_code and sc.category_code=st.category_code and sc.college_code=sm.college_code and sm.college_code=h.college_code and sm.college_code=d.collegeCode and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.latestrec='1' and sm.college_code='" + collcode + "'";
                }
                else if (ddldept.SelectedItem.Text == "All" && ddlstaffcat.SelectedItem.Text != "All")
                {
                    selq = " select sm.staff_code,staff_name,Fingerprint1,dept_name,desig_name,Fingerprint1 as MachineNo from staffmaster sm,hrdept_master h,desig_master d,stafftrans st,staffcategorizer sc where sm.staff_code=st.staff_code and sc.category_code=st.category_code and sc.college_code=sm.college_code and sm.college_code=h.college_code and sm.college_code=d.collegeCode and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.latestrec='1' and sm.college_code='" + collcode + "' and st.category_code='" + ddlstaffcat.SelectedItem.Value + "'";
                }
                else if (ddldept.SelectedItem.Text != "All" && ddlstaffcat.SelectedItem.Text == "All")
                {
                    selq = " select sm.staff_code,staff_name,Fingerprint1,dept_name,desig_name,Fingerprint1 as MachineNo from staffmaster sm,hrdept_master h,desig_master d,stafftrans st,staffcategorizer sc where sm.staff_code=st.staff_code and sc.category_code=st.category_code and sc.college_code=sm.college_code and sm.college_code=h.college_code and sm.college_code=d.collegeCode and st.dept_code=h.dept_code and st.desig_code=d.desig_code  and st.latestrec='1' and sm.college_code='" + collcode + "' and st.dept_code='" + ddldept.SelectedItem.Value + "'";
                }
                else if (ddldept.SelectedItem.Text != "All" && ddlstaffcat.SelectedItem.Text != "All")
                {
                    selq = " select sm.staff_code,staff_name,Fingerprint1,dept_name,desig_name,Fingerprint1 as MachineNo from staffmaster sm,hrdept_master h,desig_master d,stafftrans st,staffcategorizer sc where sm.staff_code=st.staff_code and sc.category_code=st.category_code and sc.college_code=sm.college_code and sm.college_code=h.college_code and sm.college_code=d.collegeCode and st.dept_code=h.dept_code and st.desig_code=d.desig_code and st.latestrec='1' and sm.college_code='" + collcode + "' and st.dept_code='" + ddldept.SelectedItem.Value + "' and st.category_code='" + ddlstaffcat.SelectedItem.Value + "'";
                }
                if (chkincrel.Checked == false)
                    selq = selq + " and ((sm.resign='0' and sm.settled='0') or sm.Discontinue='0')";
                if (rbfingerid.Checked == true)
                    selq = selq + " and ((sm.Fingerprint1 is not null) and (cast(sm.FingerPrint1 as varchar)<>''))";
                else
                    selq = selq + " and ((sm.Fingerprint1 is null) or (cast(sm.FingerPrint1 as varchar)=''))";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadrepsprcolumns();
                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                cb.AutoPostBack = false;
                FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                cball.AutoPostBack = true;
                FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();

                Fpspreadpop.Sheets[0].RowCount++;
                Fpspreadpop.Sheets[0].Cells[0, 1].CellType = cball;
                Fpspreadpop.Sheets[0].Cells[0, 1].Value = 0;
                Fpspreadpop.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspreadpop.Sheets[0].Cells[0, 1].Font.Name = "Book Antiqua";

                for (ik = 0; ik < ds.Tables[0].Rows.Count; ik++)
                {
                    Fpspreadpop.Sheets[0].RowCount++;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ik + 1);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].CellType = cb;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[ik]["staff_Code"]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].CellType = txtcell;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[ik]["staff_name"]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[ik]["desig_name"]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[ik]["dept_name"]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 6].CellType = txtcell;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[ik]["Fingerprint1"]);
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

                    //Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[ik]["MachineNo"]);
                    //Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    //Fpspreadpop.Sheets[0].Cells[Fpspreadpop.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                }

                Fpspreadpop.Sheets[0].PageSize = Fpspreadpop.Sheets[0].RowCount;
                Fpspreadpop.Visible = true;
                lblpoperr.Visible = false;
                rprint.Visible = true;
                divpopspr.Visible = true;
            }
            else
            {
                Fpspreadpop.Visible = false;
                lblpoperr.Visible = true;
                lblpoperr.Text = "No Records Found!";
                rprint.Visible = false;
                divpopspr.Visible = false;
            }
        }
        catch { }
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            Fpspreadpop.SaveChanges();
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                txtexcel.Text = "";
                d2.printexcelreport(Fpspreadpop, reportname);
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
            string degreedetails = "Staff FingerPrint Report";
            string pagename = "Staff_FingerPrintReg.aspx";
            Printcontrol.loadspreaddetails(Fpspreadpop, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
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