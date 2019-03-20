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
using Gios.Pdf;
using System.IO;


public partial class staffattendance_report : System.Web.UI.Page
{
    string college = "";
    string orderattendence = "";
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable hascount = new Hashtable();
    Hashtable hat1 = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    // SortedDictionary<string, string> diclev = new SortedDictionary<string, string>();
    //SortedDictionary<string, double> diccatwiselev = new SortedDictionary<string, double>();
    Dictionary<string, string> diclev = new Dictionary<string, string>();//delsi0604
    Dictionary<string, double> diccatwiselev = new Dictionary<string, double>();//delsi0604

    Dictionary<string, string> diccat = new Dictionary<string, string>();
    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    ArrayList leave = new ArrayList();
    string capvalue = "";
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    int height = 0;
    double conducatday = 0;

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
            college = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                college = ddlcollege.SelectedItem.Value.ToString();
                binddept();
                binddesig();
                bindstaffcata();
                bindstafftype();
                bindleavetype();
                txtfromdate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");
                txtfromdate.Text = System.DateTime.Now.ToString("d/MM/yyyy");
                txttodate.Text = System.DateTime.Now.ToString("d/MM/yyyy");
                FpSpread2.Sheets[0].AutoPostBack = false;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Font.Bold = true;
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpread2.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread2.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread2.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                FpSpread2.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread2.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].RowHeader.Visible = false;
                FpSpread2.Visible = false;
                FpSpread3.Visible = false;
                lblcatwise.Visible = false;
                FpSpread2.Sheets[0].ColumnHeader.RowCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 0;
                FpSpread2.Sheets[0].RowCount = 0;
                Itemindex.Clear();
                ItemList.Clear();
                calcutale1();
            }
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
            //ddlcollege.Items.Insert(0, "---Select---");
        }
        catch (Exception e)
        {
        }
    }

    public void binddept()
    {
        try
        {
            height = 0;
            txt_Department.Text = "---Select---";
            string college = Convert.ToString(ddlcollege.SelectedItem.Value);
            cbl_Department.Visible = true;
            cbl_Department.Items.Clear();
            ds.Clear();
            string deptquery = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code in ('" + college + "')) order by dept_name";
            }
            else
            {
                string group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code in('" + college + "')) order by dept_name";
            }
            if (deptquery != "")
            {
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                cbl_Department.DataSource = ds;
                cbl_Department.DataTextField = "dept_name";
                cbl_Department.DataValueField = "Dept_Code";
                cbl_Department.DataBind();
            }
            //for (int i = 0; i < cbl_Department.Items.Count; i++)
            //{
            cbl_Department.Items[0].Selected = true;
            txt_Department.Text = "Department(1)";
            // height++;
            //}
            //if (height > 10)
            //{
            //    panel_Department.Height = 300;
            //}
            //else
            //{
            //    panel_Department.Height = 100;
            //}
        }
        catch (Exception e)
        {
        }
    }

    public void binddesig()
    {
        try
        {
            height = 0;
            cbl_Designation.Visible = true;
            cbl_Designation.Items.Clear();
            ds.Clear();
            txt_designation.Text = "---Select---";
            string college = Convert.ToString(ddlcollege.SelectedItem.Value);
            ds = d2.loaddesignation(college);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Designation.DataSource = ds;
                cbl_Designation.DataTextField = "desig_name";
                cbl_Designation.DataValueField = "Desig_Code";
                cbl_Designation.DataBind();
            }
            //for (int i = 0; i < cbl_Designation.Items.Count; i++)
            //{
            cbl_Designation.Items[0].Selected = true;
            txt_designation.Text = "Designation(1)";
            //    height++;
            //}
            if (height > 10)
            {
                panel_Designation.Height = 300;
            }
            else
            {
                panel_Designation.Height = 100;
            }
        }
        catch (Exception e)
        {
        }
    }

    public void bindstaffcata()
    {
        try
        {
            txt_Category.Text = "---Select---";
            string college = Convert.ToString(ddlcollege.SelectedItem.Value);
            height = 0;
            cbl_Category.Visible = true;
            cbl_Category.Items.Clear();
            ds.Clear();
            ds = d2.loadcategory(college);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Category.DataSource = ds;
                cbl_Category.DataTextField = "category_name";
                cbl_Category.DataValueField = "Category_Code";
                cbl_Category.DataBind();
            }
            //for (int i = 0; i < cbl_Category.Items.Count; i++)
            //{
            cbl_Category.Items[0].Selected = true;
            txt_Category.Text = "Category(1)";
            //    height++;
            //}
            if (height > 10)
            {
                panel_Category.Height = 300;
            }
            else
            {
                panel_Category.Height = 200;
            }
        }
        catch (Exception)
        {
        }
    }

    public void bindstafftype()
    {
        try
        {
            string college = Convert.ToString(ddlcollege.SelectedItem.Value);
            height = 0;
            cblstafftype.Visible = true;
            cblstafftype.Items.Clear();
            ds.Clear();
            ds = d2.loadstafftype(college);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstafftype.DataSource = ds;
                cblstafftype.DataTextField = "StfType";
                cblstafftype.DataValueField = "StfType";
                cblstafftype.DataBind();
            }
            //for (int i = 0; i < cblstafftype.Items.Count; i++)
            //{
            cblstafftype.Items[0].Selected = true;
            txt_stafftype.Text = "StaffType(1)";
            //height++;
            //}
            if (height > 10)
            {
                panel_stafftype.Height = 300;
            }
            else
            {
                panel_stafftype.Height = 100;
            }
        }
        catch (Exception)
        {
        }
    }

    public void bindleavetype()
    {
        try
        {
            string college = Convert.ToString(ddlcollege.SelectedItem.Value);
            height = 0;
            cblleave.ClearSelection();
            cblleave.Items.Clear();
            ds.Clear();
            string query = "Select category ,shortname  from leave_category where college_code in('" + college + "') and shortname <> 'LA'and shortname <>'PER' and shortname <> 'OD'and shortname <>'RL'and shortname <>'NA' and shortname <>'RL'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblleave.DataSource = ds;
                cblleave.DataTextField = "category";
                cblleave.DataValueField = "shortname";
                cblleave.DataBind();
            }
            for (int k = 0; k < cblleave.Items.Count; k++)
            {
                cblleave.Items[k].Selected = true;
                height++;
            }
            if (height > 10)
            {
                panelleave.Height = 300;
            }
            else
            {
                panelleave.Height = 100;
            }
            cblleave.Items.Insert(0, "P");
            cblleave.Items[0].Value = "P";
            cblleave.Items.Insert(1, "A");
            cblleave.Items[1].Value = "A";
            cblleave.Items.Insert(2, "PER");
            cblleave.Items[2].Value = "PER";
            cblleave.Items.Insert(3, "LA");
            cblleave.Items[3].Value = "LA";
            cblleave.Items.Insert(4, "OD");
            cblleave.Items[4].Value = "OD";
            cblleave.Items.Insert(5, "RL");
            cblleave.Items[5].Value = "RL";
            cblleave.Items.Insert(6, "NA");
            cblleave.Items[6].Value = "NA";
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        string college = ddlcollege.SelectedItem.Value.ToString();
        binddept();
        binddesig();
        bindstaffcata();
        bindstafftype();
        bindleavetype();
    }

    protected void cb_Department_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_Department.Checked == true)
        {
            for (int i = 0; i < cbl_Department.Items.Count; i++)
            {
                cbl_Department.Items[i].Selected = true;
                txt_Department.Text = "Department(" + (cbl_Department.Items.Count) + ")";
            }
            panel_Department.Focus();
        }
        else
        {
            for (int i = 0; i < cbl_Department.Items.Count; i++)
            {
                cbl_Department.Items[i].Selected = false;
            }
            txt_Department.Text = "---Select---";
        }
    }

    protected void cbl_Department_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_Department.Focus();
        int desigcount = 0;
        for (int i = 0; i < cbl_Department.Items.Count; i++)
        {
            if (cbl_Department.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txt_Department.Text = "Department(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txt_Department.Text = "---Select---";
        }
        cb_Department.Checked = false;
    }

    protected void cb_Designation_CheckedChanged(object sender, EventArgs e)
    {
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


    protected void cbl_Designation_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_Designation.Focus();
        int desigcount = 0;
        for (int i = 0; i < cbl_Designation.Items.Count; i++)
        {
            if (cbl_Designation.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txt_designation.Text = "Designation(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txt_designation.Text = "---Select---";
        }
        cb_Designation.Checked = false;
    }

    protected void cb_Category_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_Category.Checked == true)
        {
            for (int i = 0; i < cbl_Category.Items.Count; i++)
            {
                cbl_Category.Items[i].Selected = true;
                txt_Category.Text = "Category(" + (cbl_Category.Items.Count) + ")";
            }
            panel_Category.Focus();
        }
        else
        {
            for (int i = 0; i < cbl_Category.Items.Count; i++)
            {
                cbl_Category.Items[i].Selected = false;
                txt_Category.Text = "---Select---";
            }
        }
    }

    protected void cbl_Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_Category.Focus();
        int desigcount = 0;
        for (int i = 0; i < cbl_Category.Items.Count; i++)
        {
            if (cbl_Category.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txt_Category.Text = "Category(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txt_Category.Text = "---Select---";
        }
        cb_Category.Checked = false;
    }

    protected void cbstafftype_CheckedChanged(object sender, EventArgs e)
    {
        if (cbstafftype.Checked == true)
        {
            for (int i = 0; i < cblstafftype.Items.Count; i++)
            {
                cblstafftype.Items[i].Selected = true;
                txt_stafftype.Text = "Staff Type(" + (cblstafftype.Items.Count) + ")";
            }
            panel_stafftype.Focus();
        }
        else
        {
            for (int i = 0; i < cblstafftype.Items.Count; i++)
            {
                cblstafftype.Items[i].Selected = false;
                txt_stafftype.Text = "---Select---";
            }
        }
    }

    protected void cblstafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_stafftype.Focus();
        int desigcount = 0;
        for (int i = 0; i < cblstafftype.Items.Count; i++)
        {
            if (cblstafftype.Items[i].Selected == true)
            {
                desigcount = desigcount + 1;
                txt_stafftype.Text = "Staff Type(" + desigcount.ToString() + ")";
            }
        }
        if (desigcount == 0)
        {
            txt_stafftype.Text = "---Select---";
        }
        cbstafftype.Checked = false;
    }

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        if (txtfromdate.Text == "")
        {
            lblError.Text = "Enter to date";
            lblError.Visible = true;
        }
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        if (txtfromdate.Text == "")
        {
            txttodate.Text = "";
            lblError.Visible = true;
            lblError.Text = "Enter from date first";
        }
        else
        {
            lblError.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = txtfromdate.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;
                string date2ad;
                string datetoad;
                string yr5, m5, d5;
                date2ad = txttodate.Text.ToString();
                string[] split5 = date2ad.Split(new Char[] { '/' });
                if (split5.Length == 3)
                {
                    datetoad = split5[0].ToString() + "/" + split5[1].ToString() + "/" + split5[2].ToString();
                    yr5 = split5[2].ToString();
                    m5 = split5[1].ToString();
                    d5 = split5[0].ToString();
                    datetoad = m5 + "/" + d5 + "/" + yr5;
                    DateTime dt1 = Convert.ToDateTime(dtfromad);
                    DateTime dt2 = Convert.ToDateTime(datetoad);
                    TimeSpan ts = dt2 - dt1;
                    int days = ts.Days;
                    if (days < 0)
                    {
                        lblError.Text = "From Date Should Be Less Than To Date";
                        lblError.Visible = true;
                    }
                    if (dt1 > DateTime.Today)
                    {
                        lblError.Text = "You can not mark attendance for the date greater than today";
                        lblError.Visible = true;
                    }
                    if (dt2 > DateTime.Today)
                    {
                        lblError.Text = "You can not mark attendance for the date greater than today";
                        lblError.Visible = true;
                    }
                }
            }
        }
    }

    protected void lnk_columnorder_Click(object sender, EventArgs e)
    {
        cblcolumnorder.ClearSelection();
        Cbcolumn.Checked = false;
        lnk_columnorder.Visible = false;
        ItemList.Clear();
        Itemindex.Clear();
        txt_order.Text = "";
        txt_order.Visible = false;
    }

    protected void Cbcolumn_CheckedChanged(object sender, EventArgs e)
    {
        ItemList.Clear();
        Itemindex.Clear();
        if (Cbcolumn.Checked == true)
        {
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                string si = Convert.ToString(i);
                cblcolumnorder.Items[i].Selected = true;
                lnk_columnorder.Visible = true;
                ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
                Itemindex.Add(si);
            }
            lnk_columnorder.Visible = true;
            txt_order.Visible = true;
            txt_order.Text = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                txt_order.Text = txt_order.Text + ItemList[i].ToString();
                txt_order.Text = txt_order.Text + "(" + (i + 1).ToString() + ")  ";
            }
        }
        else
        {
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                cblcolumnorder.Items[i].Selected = false;
                lnk_columnorder.Visible = false;
                ItemList.Clear();
                Itemindex.Clear();
            }
            txt_order.Visible = false;
            lnk_columnorder.Visible = false;
        }
    }

    protected void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        string value = "";
        int index;
        value = string.Empty;
        string result = Request.Form["__EVENTTARGET"];
        string[] checkedBox = result.Split('$');
        index = int.Parse(checkedBox[checkedBox.Length - 1]);
        string sindex = Convert.ToString(index);
        if (cblcolumnorder.Items[index].Selected)
        {
            if (cblcolumnorder.Items[index].Text != "In Out Time" && cblcolumnorder.Items[index].Text != "Include Holiday")
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
        }
        else
        {
            ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
            Itemindex.Remove(sindex);
        }
        for (int i = 0; i < cblcolumnorder.Items.Count; i++)
        {
            if (cblcolumnorder.Items[i].Selected == false)
            {
                sindex = Convert.ToString(i);
                ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
                Itemindex.Remove(sindex);
            }
        }
        lnk_columnorder.Visible = true;
        txt_order.Visible = true;
        txt_order.Text = "";
        for (int i = 0; i < ItemList.Count; i++)
        {
            txt_order.Text = txt_order.Text + ItemList[i].ToString();
            txt_order.Text = txt_order.Text + "(" + (i + 1).ToString() + ")  ";
        }
        if (ItemList.Count == 0)
        {
            txt_order.Visible = false;
            lnk_columnorder.Visible = false;
        }
    }

    protected void cbleave_CheckedChanged(object sender, EventArgs e)
    {
        if (cbleave.Checked == true)
        {
            for (int i = 0; i < cblleave.Items.Count; i++)
            {
                cblleave.Items[i].Selected = true;
                txt_leavetype.Text = "Leave Type(" + (cblleave.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cblleave.Items.Count; i++)
            {
                cblleave.Items[i].Selected = false;
                txt_leavetype.Text = "---Select---";
            }
        }
        panelleave.Focus();
    }

    protected void cblleave_SelectedIndexChanged(object sender, EventArgs e)
    {
        panelleave.Focus();
        int value = 0;
        for (int i = 0; i < cblleave.Items.Count; i++)
        {
            if (cblleave.Items[i].Selected == true)
            {
                value = value + 1;
                txt_leavetype.Text = "Leave Type(" + value.ToString() + ")";
            }
        }
        if (value == 0)
        {
            txt_leavetype.Text = "---Select---";
        }
        cbleave.Checked = false;
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        diclev.Clear();
        diccatwiselev.Clear();
        diccat.Clear();
        bindstaffattendacereport();
    }

    public void bindstaffattendacereport()
    {
        try
        {
            string myCategory = "";
            Double myCatCount = 0;
            string deptvalue = rs.GetSelectedItemsValueAsString(cbl_Department);
            string desigvalue = rs.GetSelectedItemsValueAsString(cbl_Designation);
            string catevalue = rs.GetSelectedItemsValueAsString(cbl_Category);
            string stypevalue = rs.GetSelectedItemsValueAsString(cblstafftype);
            string session = Convert.ToString(ddlsession.SelectedValue);
            Boolean IncludeHoliFlag = false;
            Boolean Inoutflag = false;
            #region staffattendance report
            if (!string.IsNullOrEmpty(deptvalue) && !string.IsNullOrEmpty(desigvalue) && !string.IsNullOrEmpty(catevalue) && !string.IsNullOrEmpty(stypevalue))
            {
                if (ddlcollege.Text != "---Select---")
                {
                    for (int col = 0; col < cblcolumnorder.Items.Count; col++)
                    {
                        if (cblcolumnorder.Items[col].Selected == true)
                        {
                            string value = cblcolumnorder.Items[col].Text;
                            if (value != "In Out Time" && value != "Include Holiday")
                            {
                                if (ItemList.Contains(value) == false)
                                {
                                    ItemList.Add(value.ToString());
                                }
                            }
                            else if (value == "In Out Time")
                            {
                                Inoutflag = true;

                            }
                            else if (value == "Include Holiday")
                            {
                                IncludeHoliFlag = true;
                            }
                        }
                    }
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                    }
                    string invalue = "";
                    for (int i = 0; i < ItemList.Count; i++)
                    {
                        string value1 = ItemList[i].ToString();
                        string value = Convert.ToString(hat[value1]);
                        if (value != "InoutTime" && value != "Include Holiday")
                        {
                            if (value != "")
                            {
                                if (invalue == "")
                                {
                                    invalue = value;
                                }
                                else
                                {
                                    invalue = invalue + "," + value;
                                }
                            }
                        }
                    }
                    if (invalue == "")
                    {
                        for (int col = 0; col < 3; col++)
                        {
                            cblcolumnorder.Items[col].Selected = true;
                            string value = cblcolumnorder.Items[col].Value;
                            ItemList.Add(cblcolumnorder.Items[col].Text);
                            if (invalue == "")
                            {
                                invalue = value;
                            }
                            else
                            {
                                invalue = invalue + "," + value;
                            }
                        }
                    }
                    string attendencequery = "";
                    if (invalue != "")
                    {
                        attendencequery = "select m.staff_code, " + invalue + ",s.category_name,t.category_code from staffmaster m,stafftrans t,desig_master d,hrdept_master h,staffcategorizer s where  t.staff_code=m.staff_code  and t.desig_code=d.desig_code and h.dept_code=t.dept_code and s.category_code=t.category_code and m.college_code = d.collegeCode and m.college_code = h.college_code and s.college_code = m.college_code and m.resign='0' and m.settled='0'  and t.latestrec = 1";
                        lblError.Visible = false;
                        attendencequery = attendencequery + " and m.college_code in('" + ddlcollege.SelectedItem.Value.ToString() + "')";

                        if (!string.IsNullOrEmpty(deptvalue))
                        {
                            attendencequery += " and h.dept_code in('" + deptvalue + "')";
                        }

                        if (desigvalue != "")
                        {
                            attendencequery += " and d.desig_code in('" + desigvalue + "')";
                        }

                        if (catevalue != "")
                        {
                            attendencequery += " and s.category_code in('" + catevalue + "')";
                        }

                        if (stypevalue != "")
                        {
                            attendencequery += " and t.stftype in('" + stypevalue + "')";
                        }
                        if (attendencequery != "")
                        {
                            attendencequery += " order by s.categoryid,h.priority,d.priority,join_date";
                        }
                        if (!string.IsNullOrEmpty(attendencequery))
                        {
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(attendencequery, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                FpSpread2.Sheets[0].AutoPostBack = true;
                                FpSpread2.Sheets[0].RowCount = 1;
                                FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
                                FpSpread2.Sheets[0].ColumnHeader.Columns.Count = 2;
                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                                if (ItemList.Count > 0)
                                {
                                    for (int i = 1; i <= ItemList.Count; i++)
                                    {
                                        if (ItemList[i - 1].ToString() != "In Out Time" && ItemList[i - 1].ToString() != "Include Holiday")
                                        {
                                            FpSpread2.Sheets[0].ColumnHeader.Columns.Count++;
                                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, i + 1, 2, 1);
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Text = ItemList[i - 1].ToString();
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Locked = true;
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Font.Size = FontUnit.Medium;
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Font.Bold = true;
                                        }
                                    }
                                }
                                //FpSpread2.Sheets[0].ColumnHeader.Columns.Count++;
                                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Session";
                                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Locked = true;
                                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                                //FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;
                                //FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                                string[] dtfrom;
                                string[] dttodate;
                                dtfrom = txtfromdate.Text.Split('/');
                                dttodate = txttodate.Text.Split('/');
                                DateTime strstartdate = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]).Date;
                                DateTime strstartdate1 = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]).Date;
                                DateTime strenddate = Convert.ToDateTime(dttodate[1] + '/' + dttodate[0] + '/' + dttodate[2]).Date;
                                TimeSpan t = strenddate.Subtract(strstartdate);
                                long days = t.Days;
                                if (strstartdate <= strenddate)
                                {
                                    if (days == 0 && strstartdate.ToString("dddd") == "Sunday")
                                    {
                                        lblError.Text = "Selected Day is Sunday";
                                        lblError.Visible = true;
                                        FpSpread2.Visible = false;
                                        FpSpread3.Visible = false;
                                        lblcatwise.Visible = false;
                                        FpSpread2.Sheets[0].Visible = false;
                                        lblvalidation.Visible = false;
                                        lblrptname.Visible = false;
                                        txtexcelname.Visible = false;
                                        btnxl.Visible = false;
                                        btnprintmaster.Visible = false;
                                        lblvalidation1.Visible = false;
                                        lblspr3validation.Visible = false;
                                        lblspr3rptname.Visible = false;
                                        txtspr3rpt.Visible = false;
                                        btnspr3expexcel.Visible = false;
                                        btnspr3prnt.Visible = false;
                                        btnPrintpdf.Visible = false;
                                    }
                                    else if (strstartdate > DateTime.Today)
                                    {
                                        lblError.Text = "You can not mark attendance for the date greater than today";
                                        lblError.Visible = true;
                                        FpSpread2.Visible = false;
                                        FpSpread3.Visible = false;
                                        lblcatwise.Visible = false;
                                        FpSpread2.Sheets[0].Visible = false;
                                        lblvalidation.Visible = false;
                                        lblrptname.Visible = false;
                                        txtexcelname.Visible = false;
                                        btnxl.Visible = false;
                                        btnprintmaster.Visible = false;
                                        lblvalidation1.Visible = false;
                                        lblspr3validation.Visible = false;
                                        lblspr3rptname.Visible = false;
                                        txtspr3rpt.Visible = false;
                                        btnspr3expexcel.Visible = false;
                                        btnspr3prnt.Visible = false;
                                        btnPrintpdf.Visible = false;
                                    }
                                    else if (strenddate > DateTime.Today)
                                    {
                                        lblError.Text = "You can not mark attendance for the date greater than today";
                                        lblError.Visible = true;
                                        FpSpread2.Visible = false;
                                        FpSpread3.Visible = false;
                                        lblcatwise.Visible = false;
                                        FpSpread2.Sheets[0].Visible = false;
                                        lblvalidation.Visible = false;
                                        lblrptname.Visible = false;
                                        txtexcelname.Visible = false;
                                        btnxl.Visible = false;
                                        btnprintmaster.Visible = false;
                                        lblvalidation1.Visible = false;
                                        lblspr3validation.Visible = false;
                                        lblspr3rptname.Visible = false;
                                        txtspr3rpt.Visible = false;
                                        btnspr3expexcel.Visible = false;
                                        btnspr3prnt.Visible = false;
                                        btnPrintpdf.Visible = false;
                                    }
                                    else
                                    {
                                        string errordate = "";
                                        while (strstartdate <= strenddate)
                                        {
                                            if (IncludeHoliFlag == false)
                                            {
                                                if (days >= 0 && strstartdate.ToString("dddd") != "Sunday")
                                                {
                                                    if (strstartdate1 == strenddate)//delsi 2007
                                                    {
                                                        string date = strstartdate.ToString("dd/MM/yyyy");
                                                        if (Inoutflag == true)
                                                        {
                                                            FpSpread2.Sheets[0].ColumnHeader.Columns.Count = FpSpread2.Sheets[0].ColumnHeader.Columns.Count + 3;
                                                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                                        }
                                                        else
                                                        {
                                                            FpSpread2.Sheets[0].ColumnHeader.Columns.Count = FpSpread2.Sheets[0].ColumnHeader.Columns.Count + 2;
                                                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2, 1, 2);
                                                        }

                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = date.ToString();
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                                        if (Inoutflag == true)
                                                        {
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "In/Out Time";
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Bold = true;

                                                        }
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Entry";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                                        if (chkincreason.Checked == true)
                                                        {
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Reason";
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;
                                                            FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Visible = true;
                                                        }
                                                        else
                                                            FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Visible = false;
                                                    }
                                                    else//delsi 2007
                                                    {

                                                        string date = strstartdate.ToString("dd/MM/yyyy");
                                                        string[] splitdate = date.Split('/');
                                                        string getmonyear = splitdate[0] + "/" + splitdate[1];
                                                        FpSpread2.Sheets[0].ColumnHeader.Columns.Count = FpSpread2.Sheets[0].ColumnHeader.Columns.Count + 2;
                                                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2, 1, 2);
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = Convert.ToString(getmonyear);// date.ToString();
                                                        if (Inoutflag == true)
                                                        {
                                                            FpSpread2.Sheets[0].ColumnHeader.Columns.Count = FpSpread2.Sheets[0].ColumnHeader.Columns.Count + 1;
                                                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Bold = true;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "In/Out Time";
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Bold = true;

                                                        }
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Entry";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                                        if (chkincreason.Checked == true)
                                                        {
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Reason";
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;
                                                            FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Visible = true;
                                                        }
                                                        else
                                                            FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Visible = false;

                                                    }
                                                }
                                                else
                                                {
                                                    if (errordate == "")
                                                    {
                                                        errordate = "" + strstartdate.ToString("dd/MM/yyyy");
                                                    }
                                                    else
                                                    {
                                                        errordate = errordate + " , " + strstartdate.ToString("dd/MM/yyyy");
                                                    }
                                                }
                                            }
                                            else if (IncludeHoliFlag == true)
                                            {
                                                if (strstartdate1 == strenddate)//delsi 2007
                                                {
                                                    string date = strstartdate.ToString("dd/MM/yyyy");
                                                    if (Inoutflag == true)
                                                    {
                                                        FpSpread2.Sheets[0].ColumnHeader.Columns.Count = FpSpread2.Sheets[0].ColumnHeader.Columns.Count + 3;
                                                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);

                                                    }
                                                    else
                                                    {
                                                        FpSpread2.Sheets[0].ColumnHeader.Columns.Count = FpSpread2.Sheets[0].ColumnHeader.Columns.Count + 2;
                                                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2, 1, 2);
                                                    }
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = date.ToString();
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                                    if (Inoutflag == true)
                                                    {
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "In/Out Time";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;

                                                    }
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Entry";
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                                    if (chkincreason.Checked == true)
                                                    {
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Reason";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;
                                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Visible = true;
                                                    }
                                                    else
                                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Visible = false;
                                                }
                                                else//delsi 2007
                                                {

                                                    string date = strstartdate.ToString("dd/MM/yyyy");
                                                    string[] splitdate = date.Split('/');
                                                    string getmonyear = splitdate[0] + "/" + splitdate[1];
                                                    FpSpread2.Sheets[0].ColumnHeader.Columns.Count = FpSpread2.Sheets[0].ColumnHeader.Columns.Count + 2;
                                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2, 1, 2);
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = Convert.ToString(getmonyear);// date.ToString();
                                                    if (Inoutflag == true)
                                                    {
                                                        FpSpread2.Sheets[0].ColumnHeader.Columns.Count = FpSpread2.Sheets[0].ColumnHeader.Columns.Count + 1;
                                                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Bold = true;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "In/Out Time";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 3].Font.Bold = true;

                                                    }
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Entry";
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                                    if (chkincreason.Checked == true)
                                                    {
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Reason";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;
                                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Visible = true;
                                                    }
                                                    else
                                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Visible = false;

                                                }
                                            }
                                            strstartdate = strstartdate.AddDays(1);
                                            FpSpread2.SaveChanges();
                                        }
                                        if (errordate != "")
                                        {
                                            lblError.Text = errordate.ToString() + " Day is Sunday";
                                            lblError.Visible = true;
                                        }
                                        ArrayList leave1 = new ArrayList();
                                        for (int value = 0; value < cblleave.Items.Count; value++)
                                        {
                                            if (cblleave.Items[value].Selected == true)
                                            {
                                                leave1.Add(cblleave.Items[value].Value);
                                            }
                                        }
                                        int sno111 = 0;
                                        if (leave1.Count > 0)
                                        {
                                            FpSpread2.Sheets[0].RowCount = 0;
                                            ArrayList arrayceck = new ArrayList();
                                            string date1 = txtfromdate.Text.ToString();
                                            string[] split = date1.Split(new Char[] { '/' });
                                            string datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                                            string date2 = txttodate.Text.ToString();
                                            string[] split1 = date2.Split(new Char[] { '/' });
                                            string dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                            DateTime sdate = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]).Date;
                                            DateTime senddate = Convert.ToDateTime(split1[1] + '/' + split1[0] + '/' + split1[2]).Date;
                                            while (sdate <= senddate)
                                            {
                                                if (IncludeHoliFlag == false)
                                                {
                                                    if (sdate.ToString("dddd") != "Sunday")
                                                    {
                                                        arrayceck.Add(sdate.ToString("d/MM/yyyy"));
                                                    }
                                                    sdate = sdate.AddDays(1);
                                                }
                                                else
                                                {
                                                    arrayceck.Add(sdate.ToString("d/MM/yyyy"));
                                                    sdate = sdate.AddDays(1);
                                                }
                                            }
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                for (int st = 0; st < ds.Tables[0].Rows.Count; st++)
                                                {
                                                    string staff_code = ds.Tables[0].Rows[st]["staff_code"].ToString();
                                                    myCategory = Convert.ToString(ds.Tables[0].Rows[st]["category_name"]);
                                                    string CategoryCode = Convert.ToString(ds.Tables[0].Rows[st]["category_code"]);
                                                    if (!diccat.ContainsKey(myCategory))
                                                        diccat.Add(myCategory, myCategory);
                                                    myCatCount = 0;
                                                    if (arrayceck.Count > 0)
                                                    {
                                                        bool flage = false;
                                                        for (int date = 0; date < arrayceck.Count; date++)
                                                        {
                                                            string date11 = arrayceck[date].ToString();
                                                            string[] split_d = date11.Split(new Char[] { '/' });
                                                            string strdate = split_d[0].ToString();
                                                            string Atmonth = split_d[1].ToString();
                                                            string Atyear = split_d[2].ToString();
                                                            string atmonth1 = (Atmonth.TrimStart('0'));
                                                            string monyear = atmonth1 + "/" + Atyear;
                                                            string strdate1 = (strdate.TrimStart('0'));
                                                            string resondate = atmonth1 + "/" + strdate1 + "/" + Atyear;
                                                            string countquery = "";
                                                            DataSet ds2 = new DataSet();
                                                            countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                            countquery += " select  (select mastervalue from co_mastervalues where mastercode=gatereqreason)Reason ,convert(varchar(10),leavefrom,103)leavefrom,convert(varchar(10),leaveto,103)leaveto,datediff(dd,leavefrom,leaveto) LeaveDaysCount from rq_requisition rq,staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sa.appl_id=rq.ReqAppNo and requesttype='5' and rq.reqAppstatus=1 AND sm.staff_code='" + staff_code + "' and '" + resondate + "' between leavefrom and leaveto";
                                                            ds2.Clear();
                                                            ds2 = d2.select_method_wo_parameter(countquery, "Text");
                                                            if (ds2.Tables[0].Rows.Count > 0)
                                                            {
                                                                string Morning_value = "";
                                                                string Evening_Value = "";
                                                                string Moring_reason_value = "";
                                                                string evening_reason_value = "";
                                                                string attendavalue = Convert.ToString(ds2.Tables[0].Rows[0][0]);
                                                                string reasonvalue = Convert.ToString(ds2.Tables[0].Rows[0][1]);

                                                                if (attendavalue.Trim() != "" && attendavalue.ToString().Trim() != null && attendavalue.ToString() != "-")
                                                                {
                                                                    string[] split_Attendavlue = attendavalue.Split('-');
                                                                    if (split_Attendavlue.Length > 0)
                                                                    {
                                                                        Morning_value = split_Attendavlue[0].ToString();
                                                                        Evening_Value = split_Attendavlue[1].ToString();
                                                                    }
                                                                }
                                                                if (reasonvalue.Trim() != "" && reasonvalue.ToString().Trim() != null && reasonvalue.ToString() != "-")
                                                                {
                                                                    //string[] split_reansonvlaue = reasonvalue.Split('-'); delsi2104
                                                                    //if (split_reansonvlaue.Length > 0)
                                                                    //{
                                                                    //    Moring_reason_value = split_reansonvlaue[0].ToString();
                                                                    //    evening_reason_value = split_reansonvlaue[1].ToString();
                                                                    //}
                                                                    if (ds2.Tables[1].Rows.Count > 0)
                                                                    {
                                                                        string reasonvalues = Convert.ToString(ds2.Tables[1].Rows[0][0]);
                                                                        Moring_reason_value = Convert.ToString(reasonvalues);
                                                                        evening_reason_value = Convert.ToString(reasonvalues);
                                                                    }
                                                                    else if (ds2.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        if (reasonvalue.Contains('-'))
                                                                        {
                                                                            string[] split_reansonvlaue = reasonvalue.Split('-');// delsi1707
                                                                            if (split_reansonvlaue.Length > 0)
                                                                            {
                                                                                Moring_reason_value = split_reansonvlaue[0].ToString();
                                                                                evening_reason_value = split_reansonvlaue[1].ToString();
                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (ds2.Tables != null)//barath 08.01.18
                                                                        if (ds2.Tables[1].Rows.Count > 0)
                                                                        {
                                                                            int numberofleave = 0;
                                                                            int.TryParse(Convert.ToString(ds2.Tables[1].Rows[0]["LeaveDaysCount"]), out numberofleave);
                                                                            if (numberofleave > 0)
                                                                                //Moring_reason_value = Convert.ToString(ds2.Tables[1].Rows[0]["Reason"]) + " (" + Convert.ToString(ds2.Tables[1].Rows[0]["leavefrom"]) + " - " + Convert.ToString(ds2.Tables[1].Rows[0]["leaveto"]) + ") ";//delsi21/05/2018
                                                                                Moring_reason_value = Convert.ToString(ds2.Tables[1].Rows[0]["Reason"]);
                                                                            else
                                                                                Moring_reason_value = Convert.ToString(ds2.Tables[1].Rows[0]["Reason"]);
                                                                        }
                                                                }
                                                                if (session == "M")
                                                                {
                                                                    if (Morning_value.Trim() != "" && Morning_value.ToString().Trim() != "-" && Morning_value.ToString() != null)
                                                                    {
                                                                        if (leave1.Contains(Morning_value) == true)
                                                                        {
                                                                            flage = true;
                                                                            if (!diclev.ContainsKey(Morning_value.Trim()))
                                                                                diclev.Add(Morning_value.Trim(), Morning_value.Trim());
                                                                            if (!diccatwiselev.ContainsKey(myCategory + "-" + Morning_value.Trim()))
                                                                                diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), 2);
                                                                            else
                                                                            {
                                                                                Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Morning_value.Trim()]), out myCatCount);
                                                                                diccatwiselev.Remove(myCategory + "-" + Morning_value.Trim());
                                                                                diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), myCatCount + 2);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (session == "E")
                                                                {
                                                                    if (Evening_Value.Trim() != "" && Evening_Value.ToString().Trim() != "-" && Evening_Value.ToString() != null)
                                                                    {
                                                                        if (leave1.Contains(Evening_Value) == true)
                                                                        {
                                                                            flage = true;
                                                                            if (!diclev.ContainsKey(Evening_Value.Trim()))
                                                                                diclev.Add(Evening_Value.Trim(), Evening_Value.Trim());
                                                                            if (!diccatwiselev.ContainsKey(myCategory + "-" + Evening_Value.Trim()))
                                                                                diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), 2);
                                                                            else
                                                                            {
                                                                                Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Evening_Value.Trim()]), out myCatCount);
                                                                                diccatwiselev.Remove(myCategory + "-" + Evening_Value.Trim());
                                                                                diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), myCatCount + 2);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (session == "All")
                                                                {
                                                                    if (Morning_value.Trim() != "" && Morning_value.ToString().Trim() != "-" && Morning_value.ToString() != null)
                                                                    {
                                                                        if (leave1.Contains(Morning_value) == true)
                                                                        {
                                                                            flage = true;
                                                                            if (!diclev.ContainsKey(Morning_value.Trim()))
                                                                                diclev.Add(Morning_value.Trim(), Morning_value.Trim());
                                                                            if (!diccatwiselev.ContainsKey(myCategory + "-" + Morning_value.Trim()))
                                                                                diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), 1);
                                                                            else
                                                                            {
                                                                                Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Morning_value.Trim()]), out myCatCount);
                                                                                diccatwiselev.Remove(myCategory + "-" + Morning_value.Trim());
                                                                                diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), myCatCount + 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    if (Evening_Value.Trim() != "" && Evening_Value.ToString().Trim() != "-" && Evening_Value.ToString() != null)
                                                                    {
                                                                        if (leave1.Contains(Evening_Value) == true)
                                                                        {
                                                                            flage = true;
                                                                            if (!diclev.ContainsKey(Evening_Value.Trim()))
                                                                                diclev.Add(Evening_Value.Trim(), Evening_Value.Trim());
                                                                            if (!diccatwiselev.ContainsKey(myCategory + "-" + Evening_Value.Trim()))
                                                                                diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), 1);
                                                                            else
                                                                            {
                                                                                Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Evening_Value.Trim()]), out myCatCount);
                                                                                diccatwiselev.Remove(myCategory + "-" + Evening_Value.Trim());
                                                                                diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), myCatCount + 1);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        int col = 0;
                                                        int val = 0;
                                                        if (flage == true)
                                                        {
                                                            if (session == "All")
                                                            {
                                                                sno111++;
                                                                FpSpread2.Sheets[0].RowCount += 2;
                                                                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, 0, 2, 1);
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = sno111.ToString();
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Note = CategoryCode;//bb 06.01.18
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Locked = true;
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Font.Name = "Book Antiqua";
                                                            }
                                                            if (session == "M" || session == "E")
                                                            {
                                                                sno111++;
                                                                FpSpread2.Sheets[0].RowCount += 1;
                                                                //FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, 0, 2, 1);
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Note = CategoryCode;//bb 06.01.18
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno111.ToString();
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                            }
                                                            #region Spread Row Bind
                                                            if (session == "All")
                                                            {
                                                                for (int list = 0; list < ItemList.Count; list++)
                                                                {
                                                                    val++;
                                                                    string value = ItemList[list].ToString();
                                                                    if (value == "Staff Code")
                                                                    {
                                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Name = "Book Antiqua";
                                                                        // FpSpread2.Sheets[0].Columns[st + 2].Width = 100;
                                                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                    }
                                                                    else if (value == "Staff Name")
                                                                    {
                                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Name = "Book Antiqua";
                                                                        // FpSpread2.Sheets[0].Columns[st + 2].Width = 200;
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Department")
                                                                    {
                                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Name = "Book Antiqua";
                                                                        //   FpSpread2.Sheets[0].Columns[st + 2].Width = 200;
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Department Acr")
                                                                    {
                                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Name = "Book Antiqua";
                                                                        //   FpSpread2.Sheets[0].Columns[st + 2].Width = 200;
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Designation")
                                                                    {
                                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Name = "Book Antiqua";
                                                                        //   FpSpread2.Sheets[0].Columns[st + 2].Width = 200;
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Staff Category")
                                                                    {
                                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Name = "Book Antiqua";
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        // FpSpread2.Sheets[0].Columns[st + 2].Width = 150;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Staff Type")
                                                                    {
                                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Locked = true;
                                                                        //  FpSpread2.Sheets[0].Columns[st + 2].Width = 150;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Name = "Book Antiqua";
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Session")
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Text = "M";
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Name = "Book Antiqua";

                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = "E";
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";

                                                                    }

                                                                    col++;
                                                                }
                                                            }
                                                            if (session == "M" || session == "E")
                                                            {

                                                                for (int list = 0; list < ItemList.Count; list++)
                                                                {
                                                                    val++;
                                                                    string value = ItemList[list].ToString();
                                                                    if (value == "Staff Code")
                                                                    {
                                                                        //FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        // FpSpread2.Sheets[0].Columns[st + 2].Width = 100;
                                                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                    }
                                                                    else if (value == "Staff Name")
                                                                    {
                                                                        //FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        // FpSpread2.Sheets[0].Columns[st + 2].Width = 200;
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Department")
                                                                    {
                                                                        //FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        //   FpSpread2.Sheets[0].Columns[st + 2].Width = 200;
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Department Acr")
                                                                    {
                                                                        //FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        //   FpSpread2.Sheets[0].Columns[st + 2].Width = 200;
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Designation")
                                                                    {
                                                                        //FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        //   FpSpread2.Sheets[0].Columns[st + 2].Width = 200;
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Staff Category")
                                                                    {
                                                                        //FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        // FpSpread2.Sheets[0].Columns[st + 2].Width = 150;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Staff Type")
                                                                    {
                                                                        // FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, col + 2, 2, 1);
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = ds.Tables[0].Rows[st][val].ToString();
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Locked = true;
                                                                        //  FpSpread2.Sheets[0].Columns[st + 2].Width = 150;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].VerticalAlign = VerticalAlign.Middle;
                                                                    }
                                                                    else if (value == "Session")
                                                                    {
                                                                        if (session == "All")//modified by delsi 01.nov.2017
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Text = "M";
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Size = FontUnit.Medium;
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, col + 2].Font.Name = "Book Antiqua";
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = "E";
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        }
                                                                        if (session == "M")
                                                                        {

                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = "M";
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        }
                                                                        if (session == "E")
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Text = "E";
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                                        }
                                                                    }
                                                                    col++;
                                                                }
                                                            }
                                                            #endregion
                                                            //int daytime = Convert.ToInt32(ItemList.Count); 05.01.17 bb
                                                            //daytime = daytime + 2;
                                                            //if (session == "All")//modified by delsi 01.nov.2017
                                                            //{

                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, daytime].Text = "M";
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, daytime].HorizontalAlign = HorizontalAlign.Center;
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, daytime].Font.Size = FontUnit.Medium;
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, daytime].Font.Name = "Book Antiqua";
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].Text = "E";
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].HorizontalAlign = HorizontalAlign.Center;
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].Font.Size = FontUnit.Medium;
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].Font.Name = "Book Antiqua";
                                                            //}
                                                            //if (session == "M")
                                                            //{

                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].Text = "M";
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].HorizontalAlign = HorizontalAlign.Center;
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].Font.Size = FontUnit.Medium;
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].Font.Name = "Book Antiqua";
                                                            //}
                                                            //if (session == "E")
                                                            //{
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].Text = "E";
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].HorizontalAlign = HorizontalAlign.Center;
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].Font.Size = FontUnit.Medium;
                                                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime].Font.Name = "Book Antiqua";
                                                            //}

                                                            int countvalue = Convert.ToInt32(ItemList.Count);
                                                            //05.01.17 bb change 3 to 2
                                                            if (Inoutflag == true)
                                                            {

                                                                countvalue = countvalue + 3;
                                                            }
                                                            else
                                                            {
                                                                countvalue = countvalue + 2;

                                                            }
                                                            for (int date = 0; date < arrayceck.Count; date++)
                                                            {
                                                                string date11 = arrayceck[date].ToString();
                                                                string[] split_d = date11.Split(new Char[] { '/' });
                                                                string strdate = split_d[0].ToString();
                                                                string Atmonth = split_d[1].ToString();
                                                                string Atyear = split_d[2].ToString();
                                                                string atmonth1 = (Atmonth.TrimStart('0'));
                                                                string monyear = atmonth1 + "/" + Atyear;
                                                                string strdate1 = (strdate.TrimStart('0'));
                                                                string resondate = atmonth1 + "/" + strdate1 + "/" + Atyear;
                                                                string countquery = "";
                                                                DataSet ds2 = new DataSet();
                                                                countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                                countquery += " select  (select mastervalue from co_mastervalues where mastercode=gatereqreason)Reason ,convert(varchar(10),leavefrom,103)leavefrom,convert(varchar(10),leaveto,103)leaveto,datediff(dd,leavefrom,leaveto) LeaveDaysCount from rq_requisition rq,staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sa.appl_id=rq.ReqAppNo and requesttype='5' and rq.reqAppstatus=1 AND sm.staff_code='" + staff_code + "' and '" + resondate + "' between leavefrom and leaveto";

                                                                countquery += " select * from bio_attendance where access_date='" + resondate + "' and roll_no='" + staff_code + "'";

                                                                ds2.Clear();
                                                                ds2 = d2.select_method_wo_parameter(countquery, "Text");
                                                                string intime = string.Empty;
                                                                string outtime = string.Empty;
                                                                DateTime in_time = new DateTime();
                                                                DateTime out_time = new DateTime();
                                                                if (ds2.Tables[2].Rows.Count > 0)
                                                                {
                                                                    intime = Convert.ToString(ds2.Tables[2].Rows[0]["Time_in"].ToString());
                                                                    outtime = Convert.ToString(ds2.Tables[2].Rows[0]["Time_Out"].ToString());

                                                                    if (intime != "")
                                                                    {
                                                                        in_time = Convert.ToDateTime(ds2.Tables[2].Rows[0]["Time_in"]);
                                                                        if ("12:00 AM" == in_time.ToString("hh:mm tt"))
                                                                            intime = "";
                                                                        else
                                                                            intime = in_time.ToString("hh:mm tt");
                                                                    }

                                                                    if (outtime != "")
                                                                    {
                                                                        out_time = Convert.ToDateTime(ds2.Tables[2].Rows[0]["Time_Out"]);
                                                                        if ("12:00 AM" == out_time.ToString("hh:mm tt"))
                                                                            outtime = "";
                                                                        else
                                                                            outtime = out_time.ToString("hh:mm tt");
                                                                    }

                                                                }
                                                                if (ds2.Tables[0].Rows.Count > 0)
                                                                {
                                                                    string Morning_value = "";
                                                                    string Evening_Value = "";
                                                                    string Moring_reason_value = "";
                                                                    string evening_reason_value = "";
                                                                    string attendavalue = Convert.ToString(ds2.Tables[0].Rows[0][0]);
                                                                    string reasonvalue = Convert.ToString(ds2.Tables[0].Rows[0][1]);
                                                                    if (attendavalue.Trim() != "" && attendavalue.ToString().Trim() != null && attendavalue.ToString() != "-")
                                                                    {
                                                                        string[] split_Attendavlue = attendavalue.Split('-');
                                                                        if (split_Attendavlue.Length > 0)
                                                                        {
                                                                            Morning_value = split_Attendavlue[0].ToString();
                                                                            Evening_Value = split_Attendavlue[1].ToString();
                                                                        }
                                                                    }
                                                                    if (reasonvalue.Trim() != "" && reasonvalue.ToString().Trim() != null && reasonvalue.ToString() != "-")
                                                                    {
                                                                        if (ds2.Tables[1].Rows.Count > 0)//delsi2704
                                                                        {
                                                                            string reasonvalues = Convert.ToString(ds2.Tables[1].Rows[0][0]);

                                                                            //string[] split_reansonvlaue = reasonvalue.Split('-'); delsi2104
                                                                            //if (split_reansonvlaue.Length > 0)
                                                                            //{
                                                                            //    Moring_reason_value = split_reansonvlaue[0].ToString();
                                                                            //    evening_reason_value = split_reansonvlaue[1].ToString();
                                                                            //}
                                                                            Moring_reason_value = Convert.ToString(reasonvalues);
                                                                            evening_reason_value = Convert.ToString(reasonvalues);
                                                                        }

                                                                        else if (ds2.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            if (reasonvalue.Contains('-'))
                                                                            {
                                                                                string[] split_reansonvlaue = reasonvalue.Split('-');// delsi1707
                                                                                if (split_reansonvlaue.Length > 0)
                                                                                {
                                                                                    Moring_reason_value = split_reansonvlaue[0].ToString();
                                                                                    evening_reason_value = split_reansonvlaue[1].ToString();
                                                                                }
                                                                            }

                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        if (ds2.Tables != null)//barath 08.01.18
                                                                            if (ds2.Tables[1].Rows.Count > 0)
                                                                            {
                                                                                int numberofleave = 0;
                                                                                int.TryParse(Convert.ToString(ds2.Tables[1].Rows[0]["LeaveDaysCount"]), out numberofleave);
                                                                                if (numberofleave > 0)
                                                                                    //Moring_reason_value = Convert.ToString(ds2.Tables[1].Rows[0]["Reason"]) + " (" + Convert.ToString(ds2.Tables[1].Rows[0]["leavefrom"]) + " - " + Convert.ToString(ds2.Tables[1].Rows[0]["leaveto"]) + ") ";//delsi21/05/2018
                                                                                    Moring_reason_value = Convert.ToString(ds2.Tables[1].Rows[0]["Reason"]);
                                                                                else
                                                                                    Moring_reason_value = Convert.ToString(ds2.Tables[1].Rows[0]["Reason"]);
                                                                            }
                                                                    }
                                                                    if (session == "All")
                                                                    {
                                                                        if (Morning_value.Trim() != "" && Morning_value.ToString().Trim() != "-" && Morning_value.ToString() != null)
                                                                        {
                                                                            if (leave1.Contains(Morning_value) == true)
                                                                            {
                                                                                if (Inoutflag == true)
                                                                                {

                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue - 1].Text = Convert.ToString(intime);
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue - 1].HorizontalAlign = HorizontalAlign.Center;

                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue].Text = Morning_value.ToString();
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue].HorizontalAlign = HorizontalAlign.Center;
                                                                                    if (!diclev.ContainsKey(Morning_value.Trim()))
                                                                                        diclev.Add(Morning_value.Trim(), Morning_value.Trim());
                                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + Morning_value.Trim()))
                                                                                        diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), 1);
                                                                                    else
                                                                                    {
                                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Morning_value.Trim()]), out myCatCount);
                                                                                        diccatwiselev.Remove(myCategory + "-" + Morning_value.Trim());
                                                                                        diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), myCatCount + 1);
                                                                                    }
                                                                                    if (Moring_reason_value.Trim() != "" && Moring_reason_value.ToString().Trim() != "-" && Moring_reason_value.ToString() != null)
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue + 1].Text = Moring_reason_value.ToString();
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;


                                                                                    }
                                                                                    if (Inoutflag == true)
                                                                                    {
                                                                                        if (Moring_reason_value.ToString() == "")
                                                                                        {
                                                                                            string getreason = d2.GetFunction("Select TextVal from TextValTable where TextCriteria='ReaMp' and TextCriteria2='" + Morning_value.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'");
                                                                                            if (getreason != "")
                                                                                            {
                                                                                                if (intime != "")
                                                                                                {
                                                                                                    if (getreason.Contains('-'))
                                                                                                    {
                                                                                                        string[] splitval = getreason.Split('-');
                                                                                                        getreason = Convert.ToString(splitval[0]);
                                                                                                    }
                                                                                                    Moring_reason_value = getreason;
                                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue + 1].Text = Moring_reason_value.ToString();
                                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;

                                                                                                }


                                                                                            }
                                                                                        }
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue].Text = Morning_value.ToString();
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue].HorizontalAlign = HorizontalAlign.Center;
                                                                                    if (!diclev.ContainsKey(Morning_value.Trim()))
                                                                                        diclev.Add(Morning_value.Trim(), Morning_value.Trim());
                                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + Morning_value.Trim()))
                                                                                        diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), 1);
                                                                                    else
                                                                                    {
                                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Morning_value.Trim()]), out myCatCount);
                                                                                        diccatwiselev.Remove(myCategory + "-" + Morning_value.Trim());
                                                                                        diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), myCatCount + 1);
                                                                                    }
                                                                                    if (Moring_reason_value.Trim() != "" && Moring_reason_value.ToString().Trim() != "-" && Moring_reason_value.ToString() != null)
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue + 1].Text = Moring_reason_value.ToString();
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        if (Evening_Value.Trim() != "" && Evening_Value.ToString().Trim() != "-" && Evening_Value.ToString() != null)
                                                                        {
                                                                            if (leave1.Contains(Evening_Value) == true)
                                                                            {
                                                                                if (Inoutflag == true)
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue - 1].Text = Convert.ToString(outtime);
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue - 1].HorizontalAlign = HorizontalAlign.Center;

                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].Text = Evening_Value.ToString();
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].HorizontalAlign = HorizontalAlign.Center;
                                                                                    if (!diclev.ContainsKey(Evening_Value.Trim()))
                                                                                        diclev.Add(Evening_Value.Trim(), Evening_Value.Trim());
                                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + Evening_Value.Trim()))
                                                                                        diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), 1);
                                                                                    else
                                                                                    {
                                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Evening_Value.Trim()]), out myCatCount);
                                                                                        diccatwiselev.Remove(myCategory + "-" + Evening_Value.Trim());
                                                                                        diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), myCatCount + 1);
                                                                                    }
                                                                                    if (evening_reason_value.Trim() != "" && evening_reason_value.ToString().Trim() != "-" && evening_reason_value.ToString() != null)
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].Text = evening_reason_value.ToString();
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }
                                                                                    if (Inoutflag == true)
                                                                                    {
                                                                                        if (evening_reason_value.ToString() == "")
                                                                                        {
                                                                                            string getreason = d2.GetFunction("Select TextVal from TextValTable where TextCriteria='ReaMp' and TextCriteria2='" + Evening_Value.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'");
                                                                                            if (getreason != "")
                                                                                            {
                                                                                                if (outtime != "")
                                                                                                {
                                                                                                    if (getreason.Contains('-'))
                                                                                                    {
                                                                                                        string[] splitval = getreason.Split('-');
                                                                                                        getreason = Convert.ToString(splitval[0]);
                                                                                                    }
                                                                                                    evening_reason_value = getreason;
                                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].Text = evening_reason_value.ToString();
                                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;

                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].Text = Evening_Value.ToString();
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].HorizontalAlign = HorizontalAlign.Center;
                                                                                    if (!diclev.ContainsKey(Evening_Value.Trim()))
                                                                                        diclev.Add(Evening_Value.Trim(), Evening_Value.Trim());
                                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + Evening_Value.Trim()))
                                                                                        diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), 1);
                                                                                    else
                                                                                    {
                                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Evening_Value.Trim()]), out myCatCount);
                                                                                        diccatwiselev.Remove(myCategory + "-" + Evening_Value.Trim());
                                                                                        diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), myCatCount + 1);
                                                                                    }
                                                                                    if (evening_reason_value.Trim() != "" && evening_reason_value.ToString().Trim() != "-" && evening_reason_value.ToString() != null)
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].Text = evening_reason_value.ToString();
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }

                                                                                }

                                                                            }
                                                                        }
                                                                        if (Inoutflag == true)
                                                                        {
                                                                            countvalue += 3;
                                                                        }
                                                                        else
                                                                        {
                                                                            countvalue += 2;
                                                                        }

                                                                    }
                                                                    if (session == "M")
                                                                    {
                                                                        if (Morning_value.Trim() != "" && Morning_value.ToString().Trim() != "-" && Morning_value.ToString() != null)
                                                                        {
                                                                            if (leave1.Contains(Morning_value) == true)
                                                                            {
                                                                                if (Inoutflag == true)
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue - 1].Text = intime + "-" + outtime;
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue - 1].HorizontalAlign = HorizontalAlign.Center;

                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].Text = Morning_value.ToString();
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].HorizontalAlign = HorizontalAlign.Center;
                                                                                    if (!diclev.ContainsKey(Morning_value.Trim()))
                                                                                        diclev.Add(Morning_value.Trim(), Morning_value.Trim());
                                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + Morning_value.Trim()))
                                                                                        diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), 1);
                                                                                    else
                                                                                    {
                                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Morning_value.Trim()]), out myCatCount);
                                                                                        diccatwiselev.Remove(myCategory + "-" + Morning_value.Trim());
                                                                                        diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), myCatCount);
                                                                                    }
                                                                                    if (Moring_reason_value.Trim() != "" && Moring_reason_value.ToString().Trim() != "-" && Moring_reason_value.ToString() != null)
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].Text = Moring_reason_value.ToString();
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }
                                                                                    if (Inoutflag == true)
                                                                                    {
                                                                                        if (Moring_reason_value.ToString() == "")
                                                                                        {
                                                                                            string getreason = d2.GetFunction("Select TextVal from TextValTable where TextCriteria='ReaMp' and TextCriteria2='" + Morning_value.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'");
                                                                                            if (getreason != "")
                                                                                            {
                                                                                                if (intime != "")
                                                                                                {
                                                                                                    if (getreason.Contains('-'))
                                                                                                    {
                                                                                                        string[] splitval = getreason.Split('-');
                                                                                                        getreason = Convert.ToString(splitval[0]);
                                                                                                    }
                                                                                                    Moring_reason_value = getreason;
                                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].Text = Moring_reason_value.ToString();
                                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;

                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].Text = Morning_value.ToString();
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].HorizontalAlign = HorizontalAlign.Center;
                                                                                    if (!diclev.ContainsKey(Morning_value.Trim()))
                                                                                        diclev.Add(Morning_value.Trim(), Morning_value.Trim());
                                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + Morning_value.Trim()))
                                                                                        diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), 1);
                                                                                    else
                                                                                    {
                                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Morning_value.Trim()]), out myCatCount);
                                                                                        diccatwiselev.Remove(myCategory + "-" + Morning_value.Trim());
                                                                                        diccatwiselev.Add(myCategory + "-" + Morning_value.Trim(), myCatCount);
                                                                                    }
                                                                                    if (Moring_reason_value.Trim() != "" && Moring_reason_value.ToString().Trim() != "-" && Moring_reason_value.ToString() != null)
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].Text = Moring_reason_value.ToString();
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        if (Inoutflag == true)
                                                                        {
                                                                            countvalue += 3;
                                                                        }
                                                                        else
                                                                        {
                                                                            countvalue += 2;
                                                                        }
                                                                    }
                                                                    if (session == "E")
                                                                    {
                                                                        if (Evening_Value.Trim() != "" && Evening_Value.ToString().Trim() != "-" && Evening_Value.ToString() != null)
                                                                        {
                                                                            if (leave1.Contains(Evening_Value) == true)
                                                                            {
                                                                                if (Inoutflag == true)
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue - 1].Text = intime + "-" + outtime;
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue - 1].HorizontalAlign = HorizontalAlign.Center;


                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].Text = Evening_Value.ToString();
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].HorizontalAlign = HorizontalAlign.Center;
                                                                                    if (!diclev.ContainsKey(Evening_Value.Trim()))
                                                                                        diclev.Add(Evening_Value.Trim(), Evening_Value.Trim());
                                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + Evening_Value.Trim()))
                                                                                        diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), 1);
                                                                                    else
                                                                                    {
                                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Evening_Value.Trim()]), out myCatCount);
                                                                                        diccatwiselev.Remove(myCategory + "-" + Evening_Value.Trim());
                                                                                        diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), myCatCount);
                                                                                    }
                                                                                    if (evening_reason_value.Trim() != "" && evening_reason_value.ToString().Trim() != "-" && evening_reason_value.ToString() != null)
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].Text = evening_reason_value.ToString();
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }

                                                                                    if (Inoutflag == true)
                                                                                    {
                                                                                        if (evening_reason_value.ToString() == "")
                                                                                        {
                                                                                            string getreason = d2.GetFunction("Select TextVal from TextValTable where TextCriteria='ReaMp' and TextCriteria2='" + Evening_Value.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'");
                                                                                            if (getreason != "")
                                                                                            {
                                                                                                if (outtime != "")
                                                                                                {
                                                                                                    if (getreason.Contains('-'))
                                                                                                    {
                                                                                                        string[] splitval = getreason.Split('-');
                                                                                                        getreason = Convert.ToString(splitval[0]);
                                                                                                    }

                                                                                                    evening_reason_value = getreason;
                                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].Text = evening_reason_value.ToString();
                                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;

                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {

                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].Text = Evening_Value.ToString();
                                                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue].HorizontalAlign = HorizontalAlign.Center;
                                                                                    if (!diclev.ContainsKey(Evening_Value.Trim()))
                                                                                        diclev.Add(Evening_Value.Trim(), Evening_Value.Trim());
                                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + Evening_Value.Trim()))
                                                                                        diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), 1);
                                                                                    else
                                                                                    {
                                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + Evening_Value.Trim()]), out myCatCount);
                                                                                        diccatwiselev.Remove(myCategory + "-" + Evening_Value.Trim());
                                                                                        diccatwiselev.Add(myCategory + "-" + Evening_Value.Trim(), myCatCount);
                                                                                    }
                                                                                    if (evening_reason_value.Trim() != "" && evening_reason_value.ToString().Trim() != "-" && evening_reason_value.ToString() != null)
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].Text = evening_reason_value.ToString();
                                                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, countvalue + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        if (Inoutflag == true)
                                                                        {
                                                                            countvalue += 3;
                                                                        }
                                                                        else
                                                                        {
                                                                            countvalue += 2;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int sno = 0;
                                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                            {
                                                myCatCount = 0;
                                                myCategory = Convert.ToString(ds.Tables[0].Rows[i]["category_name"]);
                                                string CategoryCode = Convert.ToString(ds.Tables[0].Rows[i]["category_code"]);
                                                if (!diccat.ContainsKey(myCategory))
                                                    diccat.Add(myCategory, myCategory);
                                                sno++;
                                                FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 2;
                                                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, 0, 2, 1);
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = sno.ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Note = myCategory;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Tag = ds.Tables[0].Rows[i][0].ToString();
                                                FpSpread2.Sheets[0].Columns[0].Width = 50;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Font.Name = "Book Antiqua";
                                                //  FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].BackColor = Color.LightYellow;
                                                FpSpread2.Sheets[0].Rows[0].Visible = false;
                                                FpSpread2.Sheets[0].Columns[1].Visible = false;
                                                int var = 2;
                                                int col = 0;
                                                #region Spread Row Bind
                                                for (int k = 0; k < ds.Tables[0].Columns.Count - 1; k++)
                                                {
                                                    var++;
                                                    col++;
                                                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, 1, 2, 1);
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].Font.Size = FontUnit.Medium;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].Font.Name = "Book Antiqua";
                                                    // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].BackColor = Color.MistyRose;
                                                    string value = ItemList[k].ToString();
                                                    if (value == "Staff Code")
                                                    {
                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].Columns[k + 2].Width = 100;
                                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                    }
                                                    else if (value == "Staff Name")
                                                    {
                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].Columns[k + 2].Width = 200;
                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                    }
                                                    else if (value == "Department")
                                                    {
                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].Columns[k + 2].Width = 200;
                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                    }
                                                    else if (value == "Department Acronym")
                                                    {
                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].Columns[k + 2].Width = 200;
                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                    }
                                                    else if (value == "Designation")
                                                    {
                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                        FpSpread2.Sheets[0].Columns[k + 2].Width = 200;
                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                    }
                                                    else if (value == "Staff Category")
                                                    {
                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                        FpSpread2.Sheets[0].Columns[k + 2].Width = 150;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                    }
                                                    else if (value == "Staff Type")
                                                    {
                                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                        FpSpread2.Sheets[0].Columns[k + 2].Width = 150;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                    }
                                                    else if (value == "Session")
                                                    {
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = "M";
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";

                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, k + 2].Text = "E";
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, k + 2].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, k + 2].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, k + 2].Font.Name = "Book Antiqua";

                                                    }
                                                }
                                                #endregion
                                            }
                                            //int daytime = Convert.ToInt32(ItemList.Count);//bb
                                            //daytime = daytime + 2;
                                            //for (int i = 1; i < FpSpread2.Sheets[0].RowCount; i++)
                                            //{
                                            //    FpSpread2.Sheets[0].Cells[i, daytime].Text = "M";
                                            //    FpSpread2.Sheets[0].Cells[i, daytime].HorizontalAlign = HorizontalAlign.Center;
                                            //    FpSpread2.Sheets[0].Cells[i, daytime].Font.Size = FontUnit.Medium;
                                            //    FpSpread2.Sheets[0].Cells[i, daytime].Font.Name = "Book Antiqua";
                                            //    i++;
                                            //    FpSpread2.Sheets[0].Cells[i, daytime].Text = "E";
                                            //    FpSpread2.Sheets[0].Cells[i, daytime].HorizontalAlign = HorizontalAlign.Center;
                                            //    FpSpread2.Sheets[0].Cells[i, daytime].Font.Size = FontUnit.Medium;
                                            //    FpSpread2.Sheets[0].Cells[i, daytime].Font.Name = "Book Antiqua";
                                            //}
                                            ArrayList arrayceck = new ArrayList();
                                            string date1 = txtfromdate.Text.ToString();
                                            string[] split = date1.Split(new Char[] { '/' });
                                            string datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                                            string date2 = txttodate.Text.ToString();
                                            string[] split1 = date2.Split(new Char[] { '/' });
                                            string dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                            DateTime sdate = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]).Date;
                                            DateTime senddate = Convert.ToDateTime(split1[1] + '/' + split1[0] + '/' + split1[2]).Date;
                                            while (sdate <= senddate)
                                            {
                                                if (sdate.ToString("dddd") != "Sunday")
                                                {
                                                    arrayceck.Add(sdate.ToString("d/MM/yyyy"));
                                                }
                                                sdate = sdate.AddDays(1);
                                            }
                                            int countvalue = Convert.ToInt32(ItemList.Count);
                                            countvalue = countvalue + 2;
                                            string monyear = "";
                                            string resondate = "";
                                            string staff_code = "";
                                            int flag = 0;
                                            int seriolno = 0;
                                            int row_flag = FpSpread2.Sheets[0].RowCount;
                                            calcutale1();
                                            ds.Clear();
                                            string linkvaluequery = "";
                                            string linkvalue = "";
                                            linkvaluequery = "select * from InsSettings where LinkName like 'Staff Holiday By Staff Type' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                                            ds = d2.select_method_wo_parameter(linkvaluequery, "Text");
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["Linkvalue"]);
                                            }
                                            for (int i = 1; i < FpSpread2.Sheets[0].RowCount; i += 2)
                                            {
                                                int s = 0;
                                                flag = 0;
                                                staff_code = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 0].Tag);
                                                int row = i;
                                                for (int k = countvalue; k < FpSpread2.Sheets[0].ColumnCount - 1; k += 2)
                                                {
                                                    string date = arrayceck[s].ToString();
                                                    string[] split_d = date.Split(new Char[] { '/' });
                                                    string strdate = split_d[0].ToString();
                                                    string Atmonth = split_d[1].ToString();
                                                    string Atyear = split_d[2].ToString();
                                                    string atmonth1 = (Atmonth.TrimStart('0'));
                                                    monyear = atmonth1 + "/" + Atyear;
                                                    string strdate1 = (strdate.TrimStart('0'));
                                                    resondate = atmonth1 + "/" + strdate1 + "/" + Atyear;
                                                    ds.Clear();
                                                    string holidayquery = "";
                                                    if (linkvalue == "0")
                                                    {
                                                        //holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + catevalue + "') and t.staff_code ='" + staff_code + "'";//delsi1606

                                                        holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + catevalue + "') and t.staff_code ='" + staff_code + "' and h.dept_code in('" + deptvalue + "') and t.dept_code=h.dept_code and latestrec='1'";

                                                    }
                                                    if (linkvalue == "1")
                                                    {
                                                        //holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stypevalue + "') ";

                                                        holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidaystaff where holiday_date='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stypevalue + "') and dept_code in('" + deptvalue + "')";
                                                    }
                                                    ds = d2.select_method_wo_parameter(holidayquery, "Text");
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        string holday_desc = Convert.ToString(ds.Tables[0].Rows[0]["holiday_desc"]);
                                                        string halforfull = Convert.ToString(ds.Tables[0].Rows[0]["halforfull"]);
                                                        if (halforfull == "False")
                                                        {
                                                            string countquery = "";
                                                            countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                            ds.Clear();
                                                            ds = d2.select_method_wo_parameter(countquery, "Text");
                                                            if (ds.Tables[0].Rows.Count > 0)
                                                            {
                                                                string attndsplit = ds.Tables[0].Rows[0][0].ToString();
                                                                string reasonsplit = ds.Tables[0].Rows[0][1].ToString();
                                                                if (attndsplit != "-" && attndsplit != null && attndsplit != "")
                                                                {
                                                                    string[] splitarray = attndsplit.Split('-');
                                                                    string[] reason_split = reasonsplit.Split('-');
                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = splitarray[0].ToString();
                                                                    FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    if (splitarray[0].ToString() == "H")
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                        if (!diclev.ContainsKey(splitarray[0].ToString()))
                                                                            diclev.Add(splitarray[0].ToString(), splitarray[0].ToString());
                                                                        if (!diccatwiselev.ContainsKey(myCategory + "-" + splitarray[0].ToString().Trim()))
                                                                            diccatwiselev.Add(myCategory + "-" + splitarray[0].ToString().Trim(), 1);
                                                                        else
                                                                        {
                                                                            Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + splitarray[0].ToString().Trim()]), out myCatCount);
                                                                            diccatwiselev.Remove(myCategory + "-" + splitarray[0].ToString().Trim());
                                                                            diccatwiselev.Add(myCategory + "-" + splitarray[0].ToString().Trim(), myCatCount + 1);
                                                                        }
                                                                    }
                                                                    if (reasonsplit != "-" && reasonsplit != null && reasonsplit != "")
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(reason_split[0].ToString());
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].Text = "";
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    }
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Text = splitarray[1].ToString();
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    if (splitarray[1].ToString() == "H")
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                        if (!diclev.ContainsKey(splitarray[1].ToString()))
                                                                            diclev.Add(splitarray[1].ToString(), splitarray[1].ToString());
                                                                        if (!diccatwiselev.ContainsKey(myCategory + "-" + splitarray[1].ToString().Trim()))
                                                                            diccatwiselev.Add(myCategory + "-" + splitarray[1].ToString().Trim(), 1);
                                                                        else
                                                                        {
                                                                            Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + splitarray[1].ToString().Trim()]), out myCatCount);
                                                                            diccatwiselev.Remove(myCategory + "-" + splitarray[1].ToString().Trim());
                                                                            diccatwiselev.Add(myCategory + "-" + splitarray[1].ToString().Trim(), myCatCount + 1);
                                                                        }
                                                                    }
                                                                    if (reasonsplit != "-" && reasonsplit != null && reasonsplit != "")
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reason_split[1].ToString());
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = "H";
                                                                    if (!diclev.ContainsKey("H"))
                                                                        diclev.Add("H", "H");
                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + "H"))
                                                                        diccatwiselev.Add(myCategory + "-" + "H", 1);
                                                                    else
                                                                    {
                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + "H"]), out myCatCount);
                                                                        diccatwiselev.Remove(myCategory + "-" + "H");
                                                                        diccatwiselev.Add(myCategory + "-" + "H", myCatCount + 1);
                                                                    }
                                                                    FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Text = "H";
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                FpSpread2.Sheets[0].Cells[row, k].Text = "H";
                                                                if (!diclev.ContainsKey("H"))
                                                                    diclev.Add("H", "H");
                                                                if (!diccatwiselev.ContainsKey(myCategory + "-" + "H"))
                                                                    diccatwiselev.Add(myCategory + "-" + "H", 1);
                                                                else
                                                                {
                                                                    Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + "H"]), out myCatCount);
                                                                    diccatwiselev.Remove(myCategory + "-" + "H");
                                                                    diccatwiselev.Add(myCategory + "-" + "H", myCatCount + 1);
                                                                }
                                                                FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                FpSpread2.Sheets[0].Cells[row + 1, k].Text = "H";
                                                                FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (halforfull == "True")
                                                            {
                                                                string morning = Convert.ToString(ds.Tables[0].Rows[0]["morning"]);
                                                                string evening = Convert.ToString(ds.Tables[0].Rows[0]["evening"]);
                                                                if (morning == "False")
                                                                {
                                                                    string countquery = "";
                                                                    countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                                    ds.Clear();
                                                                    ds = d2.select_method_wo_parameter(countquery, "Text");
                                                                    if (ds.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        string attndsplit = ds.Tables[0].Rows[0][0].ToString();
                                                                        string reasonsplit = ds.Tables[0].Rows[0][1].ToString();
                                                                        if (attndsplit != "-" && attndsplit != null && attndsplit != "")
                                                                        {
                                                                            string[] splitarray = attndsplit.Split('-');
                                                                            FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                                            if (!diclev.ContainsKey(splitarray[0]))
                                                                                diclev.Add(splitarray[0], splitarray[0]);
                                                                            if (!diccatwiselev.ContainsKey(myCategory + "-" + splitarray[0]))
                                                                                diccatwiselev.Add(myCategory + "-" + splitarray[0], 1);
                                                                            else
                                                                            {
                                                                                Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + splitarray[0]]), out myCatCount);
                                                                                diccatwiselev.Remove(myCategory + "-" + splitarray[0].ToString().Trim());
                                                                                diccatwiselev.Add(myCategory + "-" + splitarray[0], myCatCount + 1);
                                                                            }
                                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                            if (reasonsplit != "" && reasonsplit != null && reasonsplit != "-")
                                                                            {
                                                                                string[] reasonarray = reasonsplit.Split('-');
                                                                                if (reasonsplit[0].ToString() != "")
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(reasonarray[0].ToString());
                                                                                }
                                                                                else
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].Text = "";
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (morning == "True")
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row, k].Text = "H";
                                                                        if (!diclev.ContainsKey("H"))
                                                                            diclev.Add("H", "H");
                                                                        if (!diccatwiselev.ContainsKey(myCategory + "-" + "H"))
                                                                            diccatwiselev.Add(myCategory + "-" + "H", 1);
                                                                        else
                                                                        {
                                                                            Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + "H"]), out myCatCount);
                                                                            diccatwiselev.Remove(myCategory + "-" + "H");
                                                                            diccatwiselev.Add(myCategory + "-" + "H", myCatCount + 1);
                                                                        }
                                                                        FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                    }
                                                                }
                                                                if (evening == "False")
                                                                {
                                                                    string countquery = "";
                                                                    countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                                    ds.Clear();
                                                                    ds = d2.select_method_wo_parameter(countquery, "Text");
                                                                    if (ds.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        string attndsplit = ds.Tables[0].Rows[0][0].ToString();
                                                                        string reasonsplit = ds.Tables[0].Rows[0][1].ToString();
                                                                        if (attndsplit != "-" && attndsplit != null && attndsplit != "")
                                                                        {
                                                                            string[] splitarray = attndsplit.Split('-');
                                                                            flag++;
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                            if (!diclev.ContainsKey(splitarray[1]))
                                                                                diclev.Add(splitarray[1], splitarray[1]);
                                                                            if (!diccatwiselev.ContainsKey(myCategory + "-" + splitarray[1]))
                                                                                diccatwiselev.Add(myCategory + "-" + splitarray[1], 1);
                                                                            else
                                                                            {
                                                                                Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + splitarray[1]]), out myCatCount);
                                                                                diccatwiselev.Remove(myCategory + "-" + splitarray[1].ToString().Trim());
                                                                                diccatwiselev.Add(myCategory + "-" + splitarray[1], myCatCount + 1);
                                                                            }
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                            if (reasonsplit != "" && reasonsplit != null)
                                                                            {
                                                                                string[] reasonarray = reasonsplit.Split('-');
                                                                                if (reasonsplit[1].ToString() != "")
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                                }
                                                                                else
                                                                                {
                                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (evening == "True")
                                                                    {
                                                                        flag++;
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Text = "H";
                                                                        if (!diclev.ContainsKey("H"))
                                                                            diclev.Add("H", "H");
                                                                        if (!diccatwiselev.ContainsKey(myCategory + "-" + "H"))
                                                                            diccatwiselev.Add(myCategory + "-" + "H", 1);
                                                                        else
                                                                        {
                                                                            Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + "H"]), out myCatCount);
                                                                            diccatwiselev.Remove(myCategory + "-" + "H");
                                                                            diccatwiselev.Add(myCategory + "-" + "H", myCatCount + 1);
                                                                        }
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string countquery = "";
                                                        countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                        ds.Clear();
                                                        ds = d2.select_method_wo_parameter(countquery, "Text");
                                                        if (ds.Tables[0].Rows.Count > 0)
                                                        {
                                                            string attndsplit = ds.Tables[0].Rows[0][0].ToString();
                                                            string reasonsplit = ds.Tables[0].Rows[0][1].ToString();
                                                            if (attndsplit != "" && attndsplit != null)
                                                            {
                                                                string[] splitarray = attndsplit.Split('-');
                                                                if (leave.Contains(splitarray[0].ToString()) == false)
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                                    if (!diclev.ContainsKey(splitarray[0]))
                                                                        diclev.Add(splitarray[0], splitarray[0]);
                                                                    FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    if (reasonsplit != "" && reasonsplit != null)
                                                                    {
                                                                        string[] reasonarray = reasonsplit.Split('-');
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(reasonarray[0].ToString());
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                                    if (!diclev.ContainsKey(splitarray[0]))
                                                                        diclev.Add(splitarray[0], splitarray[0]);
                                                                    FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    if (reasonsplit != "" && reasonsplit != null)
                                                                    {
                                                                        string[] reasonarray = reasonsplit.Split('-');
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(reasonarray[0].ToString());
                                                                    }
                                                                    else
                                                                    {
                                                                        ds.Clear();
                                                                        string query = "";
                                                                        query = "select remarks  from staff_leave_details where staff_code='" + staff_code + "' and adate='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                                                                        ds = d2.select_method_wo_parameter(query, "Text");
                                                                        if (ds.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            string reasonapply = Convert.ToString(ds.Tables[0].Rows[0]["remarks"]);
                                                                            if (reasonapply.Trim() != "" && reasonapply != null)
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(reasonapply.ToString());
                                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Text = "";
                                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!diccatwiselev.ContainsKey(myCategory + "-" + splitarray[0]))
                                                                    diccatwiselev.Add(myCategory + "-" + splitarray[0], 1);
                                                                else
                                                                {
                                                                    Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + splitarray[0]]), out myCatCount);
                                                                    diccatwiselev.Remove(myCategory + "-" + splitarray[0].ToString().Trim());
                                                                    diccatwiselev.Add(myCategory + "-" + splitarray[0], myCatCount + 1);
                                                                }
                                                                if (leave.Contains(splitarray[1].ToString()) == false)
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    if (reasonsplit != "" && reasonsplit != null)
                                                                    {
                                                                        string[] reasonarray = reasonsplit.Split('-');
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                    if (!diclev.ContainsKey(splitarray[1]))
                                                                        diclev.Add(splitarray[1], splitarray[1]);
                                                                    if (!diccatwiselev.ContainsKey(myCategory + "-" + splitarray[1]))
                                                                        diccatwiselev.Add(myCategory + "-" + splitarray[1], 1);
                                                                    else
                                                                    {
                                                                        Double.TryParse(Convert.ToString(diccatwiselev[myCategory + "-" + splitarray[1]]), out myCatCount);
                                                                        diccatwiselev.Remove(myCategory + "-" + splitarray[1].ToString().Trim());
                                                                        diccatwiselev.Add(myCategory + "-" + splitarray[1], myCatCount + 1);
                                                                    }
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                    // FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    if (reasonsplit != "" && reasonsplit != null)
                                                                    {
                                                                        string[] reasonarray = reasonsplit.Split('-');
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                    }
                                                                    else
                                                                    {
                                                                        ds.Clear();
                                                                        string query = "";
                                                                        query = "select remarks  from staff_leave_details where staff_code='" + staff_code + "' and adate='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                                                                        ds = d2.select_method_wo_parameter(query, "Text");
                                                                        if (ds.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            string reasonapply = Convert.ToString(ds.Tables[0].Rows[0]["remarks"]);
                                                                            if (reasonapply.Trim() != "" && reasonapply != null)
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonapply.ToString());
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    s++;
                                                }
                                            }
                                        }
                                        FpSpread2.Sheets[0].FrozenColumnCount = 4;
                                        FpSpread2.SaveChanges();
                                        FpSpread2.Sheets[0].Columns[1].Visible = false;
                                        int widt = 0;
                                        for (int col = 0; col < FpSpread2.Sheets[0].ColumnCount; col++)
                                        {
                                            widt = widt + FpSpread2.Sheets[0].Columns[col].Width;
                                            widt = widt + FpSpread2.Sheets[0].RowHeader.Width + 15;
                                        }
                                        if (widt > 900)
                                        {
                                            FpSpread2.Width = 1000;
                                        }
                                        else
                                        {
                                            FpSpread2.Width = widt;
                                            FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                            FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                            FpSpread2.SaveChanges();
                                        }
                                        //int height = 62;
                                        //{
                                        //    for (int j = 1; j < row_flag; j++)
                                        //    {
                                        //        height = height + FpSpread2.Sheets[0].Rows[j].Height;
                                        //    }
                                        //    FpSpread2.Height = height;
                                        //    FpSpread2.SaveChanges();
                                        //    FpSpread2.Sheets[0].PageSize = row_flag;
                                        //}
                                        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                                        bindcatwisereport();
                                        if (FpSpread2.Sheets[0].RowCount == 0)
                                        {
                                            FpSpread2.Sheets[0].Visible = false;
                                            FpSpread2.Visible = false;
                                            FpSpread3.Visible = false;
                                            lblcatwise.Visible = false;
                                            lblError.Text = "No Records Found";
                                            lblError.Visible = true;
                                            lblvalidation.Visible = false;
                                            lblrptname.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                            btnprintmaster.Visible = false;
                                            lblvalidation1.Visible = false;
                                            lblspr3validation.Visible = false;
                                            lblspr3rptname.Visible = false;
                                            txtspr3rpt.Visible = false;
                                            btnspr3expexcel.Visible = false;
                                            btnspr3prnt.Visible = false;
                                            btnPrintpdf.Visible = false;
                                        }
                                        else
                                        {
                                            lblvalidation.Visible = false;
                                            lblrptname.Visible = true;
                                            txtexcelname.Visible = true;
                                            btnxl.Visible = true;
                                            btnprintmaster.Visible = true;
                                            lblvalidation1.Visible = false;
                                            FpSpread2.Sheets[0].Visible = true;
                                            FpSpread2.Visible = true;
                                            FpSpread3.Visible = true;
                                            lblcatwise.Visible = true;
                                            lblspr3validation.Visible = false;
                                            lblspr3rptname.Visible = true;
                                            txtspr3rpt.Visible = true;
                                            btnspr3expexcel.Visible = true;
                                            btnspr3prnt.Visible = true;
                                            btnPrintpdf.Visible = true;
                                            txtspr3rpt.Text = "";
                                            txtexcelname.Text = "";
                                        }
                                    }
                                }
                                else
                                {
                                    lblError.Text = "From Date Should Be Less Than To Date";
                                    lblError.Visible = true;
                                    FpSpread2.Sheets[0].Visible = false;
                                    FpSpread2.Visible = false;
                                    FpSpread3.Visible = false;
                                    lblcatwise.Visible = false;
                                    lblvalidation.Visible = false;
                                    lblrptname.Visible = false;
                                    txtexcelname.Visible = false;
                                    btnxl.Visible = false;
                                    btnprintmaster.Visible = false;
                                    lblvalidation1.Visible = false;
                                    lblspr3validation.Visible = false;
                                    lblspr3rptname.Visible = false;
                                    txtspr3rpt.Visible = false;
                                    btnspr3expexcel.Visible = false;
                                    btnspr3prnt.Visible = false;
                                    btnPrintpdf.Visible = false;
                                }
                            }
                            else
                            {
                                lblError.Text = "No Records Found";
                                lblError.Visible = true;
                                FpSpread2.Sheets[0].Visible = false;
                                FpSpread2.Visible = false;
                                FpSpread3.Visible = false;
                                lblcatwise.Visible = false;
                                lblvalidation.Visible = false;
                                lblrptname.Visible = false;
                                txtexcelname.Visible = false;
                                btnxl.Visible = false;
                                btnprintmaster.Visible = false;
                                lblvalidation1.Visible = false;
                                lblspr3validation.Visible = false;
                                lblspr3rptname.Visible = false;
                                txtspr3rpt.Visible = false;
                                btnspr3expexcel.Visible = false;
                                btnspr3prnt.Visible = false;
                                btnPrintpdf.Visible = false;
                            }
                        }
                    }
                }
                else
                {
                    lblError.Text = "Please Select Any One College";
                    lblError.Visible = true;
                    FpSpread2.Visible = false;
                    FpSpread3.Visible = false;
                    lblcatwise.Visible = false;
                    FpSpread2.Sheets[0].Visible = false;
                    lblvalidation.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    lblvalidation1.Visible = false;
                    lblspr3validation.Visible = false;
                    lblspr3rptname.Visible = false;
                    txtspr3rpt.Visible = false;
                    btnspr3expexcel.Visible = false;
                    btnspr3prnt.Visible = false;
                    btnPrintpdf.Visible = false;
                }
            }
            else
            {
                lblError.Text = "Please Select All Fields";
                lblError.Visible = true;
                FpSpread2.Visible = false;
                FpSpread3.Visible = false;
                lblcatwise.Visible = false;
                FpSpread2.Sheets[0].Visible = false;
                lblvalidation.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblvalidation1.Visible = false;
                lblspr3validation.Visible = false;
                lblspr3rptname.Visible = false;
                txtspr3rpt.Visible = false;
                btnspr3expexcel.Visible = false;
                btnspr3prnt.Visible = false;
                btnPrintpdf.Visible = false;
            }
            #endregion
        }
        //loadorder();
        catch (Exception ex)
        {
        }
    }

    public void bindcatwisereport()
    {
        try
        {
            double percentage_tot = 0;
            double totstaff = 0; double tot = 0;
            double myAmnt = 0;
            double rowwisetot = 0;
            int myWidth = 0;
            int ik = 1;
            double verttot = 0;
            double colwisetot = 0;
            string session = Convert.ToString(ddlsession.SelectedValue);
            FpSpread3.Sheets[0].AutoPostBack = false;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread3.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread3.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread3.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
            FpSpread3.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread3.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.Visible = false;
            FpSpread3.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread3.Sheets[0].ColumnHeader.Columns.Count = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount++;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "S.NO.";
            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 50;
            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
            FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - 1, 2, 1);
            myWidth += 50;
            FpSpread3.Sheets[0].ColumnCount++;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "Category";//CATEGORY
            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 200;
            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
            FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - 1, 2, 1);
            myWidth += 200;
            FpSpread3.Sheets[0].ColumnCount++;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "Staff Strength";
            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 200;
            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
            FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - 1, 2, 1);
            myWidth += 150;
            int columncount = 0;
            int prensentcolCnt = 0;
            int AbsentColCnt = 0;
            foreach (KeyValuePair<string, string> dr in diclev)
            {
                if (dr.Key.ToUpper() == "P")
                {
                    columncount++;
                    FpSpread3.Sheets[0].ColumnCount++;
                    prensentcolCnt = FpSpread3.Sheets[0].ColumnCount - 1;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "No. Present";
                    FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 75;
                    FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
                    myWidth += 75;
                }
                if (dr.Key.ToUpper() == "OD")
                {
                    columncount++;
                    FpSpread3.Sheets[0].ColumnCount++;
                    AbsentColCnt = FpSpread3.Sheets[0].ColumnCount - 1;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "No. On Duty";
                    FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 75;
                    FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
                    myWidth += 75;
                }
            }
            //barath 03.01.18
            Boolean GroupLeaveCheck = false;
            DataSet ds1 = new DataSet();
            ds1 = d2.select_method_wo_parameter("select MasterValue,MasterCode,MasterCriteriaValue1 from CO_MasterValues where MasterCriteria='Leave Group Mapping' and CollegeCode in('" + Convert.ToString(ddlcollege.SelectedItem.Value) + "') order by MasterCriteria1 ", "text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                GroupLeaveCheck = true;
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    FpSpread3.Sheets[0].ColumnCount++;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "TYPE OF LEAVE";
                    FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
                    columncount++;
                    //AbsentColCnt = FpSpread3.Sheets[0].ColumnCount - 1;
                    FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dr["MasterValue"]);
                    FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dr["MasterCode"]);
                    FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Note = Convert.ToString(dr["MasterCriteriaValue1"]);
                    //FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 100;
                    FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
                    myWidth += 75;
                }
                FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, columncount + 3 - ds1.Tables[0].Rows.Count, 1, ds1.Tables[0].Rows.Count);
            }
            else
            {
                #region Normal leaveType Bind
                foreach (KeyValuePair<string, string> dr in diclev)
                {
                    if (dr.Key.ToUpper() != "P" && dr.Key.ToUpper() != "OD" && dr.Key.ToUpper() != "OOD")
                    {
                        FpSpread3.Sheets[0].ColumnCount++;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "TYPE OF LEAVE";
                        FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;

                        if (dr.Key == "A")
                        {
                            FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Text = "Absent";
                            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 75;
                            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
                            myWidth += 75;
                        }
                        else
                        {
                            FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dr.Key);
                            //FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 50;
                            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
                            myWidth += 50;
                        }
                    }
                }
                if (diclev.Count > 1)
                    FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + columncount, 1, FpSpread3.Sheets[0].ColumnCount - (3 + columncount));
                #endregion
            }
            //if (diclev.Count > 1)
            //FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + columncount, 1, FpSpread3.Sheets[0].ColumnCount - (3 + columncount));
            FpSpread3.Sheets[0].ColumnCount++;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "Total";
            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 75;
            FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
            FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - 1, 2, 1);
            if (cbIncludepercent.Checked)
            {
                FpSpread3.Sheets[0].ColumnCount++;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "% On Leave";
                FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Width = 100;
                FpSpread3.Columns[FpSpread3.Sheets[0].ColumnCount - 1].Locked = true;
                FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - 1, 2, 1);
            }
            myWidth += 75;
            string deptcode = rs.GetSelectedItemsValueAsString(cbl_Department);
            string desigcode = rs.GetSelectedItemsValueAsString(cbl_Designation);
            string categorycode = rs.GetSelectedItemsValueAsString(cbl_Category);
            string stypecode = rs.GetSelectedItemsValueAsString(cblstafftype);

            DateTime FromDate = ReturnDate(txtfromdate.Text);
            DateTime ToDate = ReturnDate(txttodate.Text);
            TimeSpan ts = ToDate - FromDate;
            int Diffmonth = ts.Days + 1;
            if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(desigcode) && !string.IsNullOrEmpty(categorycode) && !string.IsNullOrEmpty(stypecode))
            {
                string Q1 = " select c.category_code,c.category_name,COUNT(c.category_name)StaffCount from StaffCategorizer c,StaffMaster M,StaffTrans T,Desig_Master G,HrDept_Master D WHERE  M.Staff_Code = T.Staff_Code AND T.Desig_Code = G.Desig_Code AND M.College_Code = G.CollegeCode AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND T.Latestrec = 1 and c.category_code=t.category_code and m.resign='0' and m.settled='0' AND M.College_Code ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and t.dept_code in('" + deptcode + "') and t.desig_code in('" + desigcode + "') and t.category_code in('" + categorycode + "') and t.stftype in('" + stypecode + "') group by c.category_code,c.category_name,c.categoryid order by c.categoryid";
                DataSet CategoryCountDS = d2.select_method_wo_parameter(Q1, "Text");
                foreach (KeyValuePair<string, string> dr in diccat)
                {
                    rowwisetot = 0;
                    FpSpread3.Sheets[0].RowCount++;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ik++);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr.Key);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    if (CategoryCountDS.Tables != null && CategoryCountDS.Tables[0].Rows.Count > 0)
                    {
                        DataView CategoryDV = new DataView();
                        CategoryCountDS.Tables[0].DefaultView.RowFilter = " category_name='" + Convert.ToString(dr.Key) + "'";
                        CategoryDV = CategoryCountDS.Tables[0].DefaultView;
                        if (CategoryDV.Count > 0)
                        {
                            string Staffcount = Convert.ToString(CategoryDV[0]["StaffCount"]);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(CategoryDV[0]["category_code"]);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Staffcount);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            tot = Convert.ToDouble(Staffcount);
                        }
                        totstaff += tot;
                    }
                    int colidx = 3 + columncount;
                    percentage_tot = 0;
                    double presntper = 0;
                    foreach (KeyValuePair<string, string> mydr in diclev)
                    {
                        if (mydr.Key.ToUpper() == "P" || mydr.Key.ToUpper() == "PER")//delsi added per 3007
                        {
                            if (diccatwiselev.ContainsKey(Convert.ToString(dr.Key + "-" + mydr.Key)))
                            {
                                Double.TryParse(Convert.ToString(diccatwiselev[Convert.ToString(dr.Key + "-" + mydr.Key)]), out myAmnt);
                                myAmnt = myAmnt / 2;
                                presntper = presntper + myAmnt;
                                //rowwisetot += myAmnt;
                                //  FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, prensentcolCnt].Text = Convert.ToString(myAmnt);
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, prensentcolCnt].Text = Convert.ToString(presntper);//delsi 3007
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, prensentcolCnt].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, prensentcolCnt].Font.Name = "Book Antiqua";
                            }
                        }
                        else if (mydr.Key.ToUpper() == "OD" || mydr.Key.ToUpper() == "OOD")//delsi
                        {
                            if (diccatwiselev.ContainsKey(Convert.ToString(dr.Key + "-" + mydr.Key)))
                            {
                                Double.TryParse(Convert.ToString(diccatwiselev[Convert.ToString(dr.Key + "-" + mydr.Key)]), out myAmnt);
                                myAmnt = myAmnt / 2;
                                //rowwisetot += myAmnt;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, AbsentColCnt].Text = Convert.ToString(myAmnt);
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, AbsentColCnt].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, AbsentColCnt].Font.Name = "Book Antiqua";
                                percentage_tot = myAmnt;
                            }
                        }
                    }
                    if (GroupLeaveCheck)
                    {
                        for (int i = 2; i < FpSpread3.Sheets[0].ColumnCount; i++)
                        {
                            string LeaveTypeCode = Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[1, i].Tag);
                            string LeaveType = Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[1, i].Note);
                            if (!string.IsNullOrEmpty(LeaveType))
                            {
                                string[] AllowanceName = LeaveType.Split(',');
                                double total = 0;
                                myAmnt = 0;
                                foreach (var item in AllowanceName)
                                {
                                    if (!string.IsNullOrEmpty(item))
                                    {
                                        if (diccatwiselev.ContainsKey(Convert.ToString(dr.Key + "-" + item.Trim())))
                                        {
                                            total = 0;
                                            double.TryParse(Convert.ToString(diccatwiselev[dr.Key + "-" + item.Trim()]), out total);
                                            myAmnt += total;
                                        }
                                    }
                                }
                                //colidx++;
                                myAmnt = myAmnt / 2;
                                rowwisetot += myAmnt;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].Text = Convert.ToString(myAmnt);
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].Font.Name = "Book Antiqua";
                            }
                        }
                    }
                    if (!GroupLeaveCheck)
                    {
                        #region Normal Leavetype
                        foreach (KeyValuePair<string, string> mydr in diclev)
                        {
                            if (mydr.Key.ToUpper() != "P" && mydr.Key.ToUpper() != "OD")
                            {
                                colidx++;
                                if (diccatwiselev.ContainsKey(Convert.ToString(dr.Key + "-" + mydr.Key)))
                                {
                                    Double.TryParse(Convert.ToString(diccatwiselev[Convert.ToString(dr.Key + "-" + mydr.Key)]), out myAmnt);
                                    myAmnt = myAmnt / 2;
                                    rowwisetot += myAmnt;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Text = Convert.ToString(myAmnt);
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Font.Name = "Book Antiqua";
                                }
                                else
                                {
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Text = "0";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Font.Name = "Book Antiqua";
                                }
                            }
                            //else
                            //{
                            //    if (mydr.Key.ToUpper() == "P")//delsi
                            //    {
                            //        if (diccatwiselev.ContainsKey(Convert.ToString(dr.Key + "-" + mydr.Key)))
                            //        {
                            //            Double.TryParse(Convert.ToString(diccatwiselev[Convert.ToString(dr.Key + "-" + mydr.Key)]), out myAmnt);
                            //            myAmnt = myAmnt / 2;
                            //            //rowwisetot += myAmnt;
                            //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, prensentcolCnt].Text = Convert.ToString(myAmnt);
                            //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, prensentcolCnt].HorizontalAlign = HorizontalAlign.Center;
                            //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, prensentcolCnt].Font.Name = "Book Antiqua";
                            //        }

                            //    }
                            //    else if (mydr.Key.ToUpper() == "OD")
                            //    {
                            //        if (diccatwiselev.ContainsKey(Convert.ToString(dr.Key + "-" + mydr.Key)))
                            //        {
                            //            Double.TryParse(Convert.ToString(diccatwiselev[Convert.ToString(dr.Key + "-" + mydr.Key)]), out myAmnt);
                            //            myAmnt = myAmnt / 2;
                            //            //rowwisetot += myAmnt;
                            //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, AbsentColCnt].Text = Convert.ToString(myAmnt);
                            //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, AbsentColCnt].HorizontalAlign = HorizontalAlign.Center;
                            //            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, AbsentColCnt].Font.Name = "Book Antiqua";
                            //            percentage_tot = myAmnt;
                            //        }
                            //    }
                            //}
                        }
                        #endregion
                    }
                    //total 26.09.17
                    colidx++;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Text = Convert.ToString(rowwisetot);
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Font.Name = "Book Antiqua";
                    //percentage 26.09.17
                    if (session == "All")
                    {
                        if (cbIncludepercent.Checked)
                        {
                            double TotalStaffCategorywise = 0;
                            double TotalStaffpresent = 0;
                            double.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text), out TotalStaffCategorywise);
                            double.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, prensentcolCnt].Text), out TotalStaffpresent);
                            TotalStaffCategorywise *= Diffmonth;//11.01.18
                            double Percent = (TotalStaffCategorywise - (TotalStaffpresent / 2)) / TotalStaffCategorywise * 100;
                            colidx++;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Text = Convert.ToString(Math.Round(Percent, 2));
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Font.Name = "Book Antiqua";
                        }
                    }
                    if (session == "M" || session == "E")
                    {
                        if (cbIncludepercent.Checked)
                        {
                            double TotalStaffCategorywise = 0;
                            double TotalStaffpresent = 0;

                            double.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text), out TotalStaffCategorywise);
                            double.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, prensentcolCnt].Text), out TotalStaffpresent);
                            TotalStaffCategorywise *= Diffmonth;//11.01.18
                            // double Percent = (TotalStaffCategorywise - (TotalStaffpresent + percentage_tot)) / TotalStaffCategorywise * 100;//Commented by saranya on 27Aug2018
                            //========Added by saranya on 27Aug2018=============//
                            double totalLeave = 0;
                            double.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text), out totalLeave);
                            double Percent = (totalLeave / TotalStaffCategorywise) * 100;
                            //=================================================//
                            colidx++;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Text = Convert.ToString(Math.Round(Percent, 2));
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colidx - 1].Font.Name = "Book Antiqua";
                        }
                    }
                }
                FpSpread3.Sheets[0].RowCount++;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString("Total");
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totstaff);
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                if (Diffmonth != 0)
                    totstaff *= Diffmonth;
                for (int j = 3; j < FpSpread3.Sheets[0].ColumnCount; j++)
                {
                    if (j != FpSpread3.Sheets[0].ColumnCount - 1)
                    {
                        colwisetot = 0;
                        for (int ro = 0; ro < FpSpread3.Sheets[0].RowCount; ro++)
                        {
                            Double.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[ro, j].Text), out verttot);
                            colwisetot += verttot;
                        }
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, j].Text = Convert.ToString(colwisetot);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, j].Font.Bold = true;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, j].Font.Name = "Book Antiqua";
                    }
                    else
                    {
                        //    colwisetot = 0;
                        //    for (int ro = 0; ro < FpSpread3.Sheets[0].RowCount - 1; ro++)
                        //    {
                        //        Double.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[ro, j].Text), out verttot);
                        //        colwisetot += verttot;
                        //    }
                        //Double.TryParse(Convert.ToString(FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, j].Text), out verttot);
                        //verttot / totstaff;
                        //    int row = FpSpread3.Sheets[0].RowCount;
                        //    double Grandpercent = colwisetot;
                        //  //  double Grandpercent = colwisetot / (row - 1);
                        if (session == "All")
                            colwisetot /= 2;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, j].Text = Convert.ToString(Math.Round(colwisetot / totstaff * 100, 2));
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, j].Font.Bold = true;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, j].Font.Name = "Book Antiqua";
                    }
                }
                FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                FpSpread3.Visible = true;
                lblcatwise.Visible = true;
                if (myWidth >= 900)
                    FpSpread3.Width = 900;
                else
                    FpSpread3.Width = myWidth;
                FpSpread3.Height = 300;
            }
        }
        catch { }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread2, reportname);
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
        {
        }
    }

    protected void btnspr3expexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtspr3rpt.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread3, reportname);
                lblspr3validation.Visible = false;
            }
            else
            {
                lblspr3validation.Text = "Please Enter Your Report Name";
                lblspr3validation.Visible = true;
                txtspr3rpt.Focus();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnprintmaster_Clcik(object sender, EventArgs e)
    {
        lblvalidation1.Visible = false;
        lblvalidation.Visible = false;
        string degreedetails = "Staff Attendance Report" + '@' + "Date: From " + txtfromdate.Text + " To " + txttodate.Text + "";
        string pagename = "staffattendance_report.aspx";
        Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnspr3prnt_Clcik(object sender, EventArgs e)
    {
        lblspr3validation.Visible = false;
        lblvalidation.Visible = false;
        string degreedetails = "Staff CategoryWise Attendance Report" + '@' + "Date: From " + txtfromdate.Text + " To " + txttodate.Text + "";
        string pagename = "staffattendance_report.aspx";
        Printcontrol.loadspreaddetails(FpSpread3, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    public void calcutale1()
    {
        ds.Clear();
        string attendquery = "";
        attendquery = "Select distinct status,shortname   from leave_category where status<>'' and college_code='" + ddlcollege.SelectedItem.Value + "'";
        ds = d2.select_method_wo_parameter(attendquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int attnd = 0; attnd < ds.Tables[0].Rows.Count; attnd++)
            {
                if (leave.Contains(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim()) == false)
                {
                    hat1.Add(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim(), ds.Tables[0].Rows[attnd]["status"].ToString().Trim());
                    leave.Add(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim());
                }
            }
        }
    }

    protected void ddlsession_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    //barath 03.01.18
    protected void Lnkbtn_groupSettingsOnclick(object sender, EventArgs e)
    {
        try
        {
            bindtype(); bindleavetype1();
            ddl_coltypeadd_selectedindexchange(sender, e);
            LeaveGroupSettings.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); }
    }

    protected void cbLeave1Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbLeave1.Checked == true)
            {
                for (int i = 0; i < cblLeave1.Items.Count; i++)
                {

                    cblLeave1.Items[i].Selected = true;
                    txtLeave.Text = "Leave Type(" + (cblLeave1.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblLeave1.Items.Count; i++)
                {
                    cblLeave1.Items[i].Selected = false;
                    txtLeave.Text = "--Select--";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); }
    }

    protected void cblLeave1SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cbLeave1.Checked = false;
            for (int i = 0; i < cblLeave1.Items.Count; i++)
            {
                if (cblLeave1.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            txtLeave.Text = "Leave Type(" + seatcount.ToString() + ")";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        LeaveGroupSettings.Visible = false;
    }

    protected void ddl_coltypeadd_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            cblLeave1.ClearSelection();
            txtLeave.Text = "--Select--";
            if (ddl_coltypeadd.Items.Count > 0)
            {
                string q1 = " select mastercriteriavalue1,mastercriteria1 from co_mastervalues where MasterCriteria ='Leave Group Mapping' and  CollegeCode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and MasterCode='" + Convert.ToString(ddl_coltypeadd.SelectedItem.Value) + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string[] value = Convert.ToString(ds.Tables[0].Rows[0][0]).Split(',');
                    int count = 0;
                    if (value[0].ToString() != "")
                    {
                        foreach (var val in value)
                        {
                            cblLeave1.Items.FindByValue(val).Selected = true;
                            count++;
                        }
                        txtLeave.Text = "Leave Type(" + count + ")";
                    }
                    string val1 = Convert.ToString(ds.Tables[0].Rows[0][1]);
                    if (val1.Trim() != "" && val1.Trim() != null)
                    {
                        ddl_priority.SelectedIndex = ddl_priority.Items.IndexOf(ddl_priority.Items.FindByText(val1));
                    }
                }
                else
                {
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); }
    }

    protected void btn_addtype_OnClick(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }

    protected void btndescpopadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='Leave Group Mapping' and CollegeCode ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='Leave Group Mapping' and CollegeCode ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','Leave Group Mapping','" + Convert.ToString(ddlcollege.SelectedItem.Value) + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                    txt_description11.Text = "";
                    bindtype();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Enter the description\");", true);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); }
    }

    protected void btnsavegroupbt_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_coltypeadd.SelectedItem.Value != "" && txtLeave.Text != "--Select--")
            {
                string setgroupby = "";
                for (int m = 0; m < cblLeave1.Items.Count; m++)
                {
                    if (cblLeave1.Items[m].Selected == true)
                    {
                        string addrelival1 = cblLeave1.Items[m].Value.ToString();
                        if (setgroupby == "")
                        {
                            setgroupby = addrelival1;
                        }
                        else
                        {
                            setgroupby = setgroupby + "," + addrelival1;
                        }
                    }
                }
                string sql = " if exists(select*from CO_MasterValues where MasterCriteria ='Leave Group Mapping' and MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' )  update CO_MasterValues set MasterCriteriaValue1='" + setgroupby + "',mastercriteria1='" + ddl_priority.SelectedItem.Value.ToString() + "' where  MasterCriteria ='Leave Group Mapping' and MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and  CollegeCode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); }
    }

    protected void bindtype()
    {
        try
        {
            string q1 = "  select mastervalue,mastercode,mastercriteria1 from CO_MasterValues where MasterCriteria ='Leave Group Mapping' and CollegeCode ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_coltypeadd.DataSource = ds;
                ddl_coltypeadd.DataTextField = "mastervalue";
                ddl_coltypeadd.DataValueField = "mastercode";
                ddl_coltypeadd.DataBind();
                ddl_priority.Items.Clear();
                for (int m = 1; m <= ddl_coltypeadd.Items.Count; m++)
                {
                    ddl_priority.Items.Add(m.ToString());
                }
                //string value = dt.GetFunction(" select mastercriteria1 from CO_MasterValues where MasterCriteria ='Leave Group Mapping' and MasterCode='" + Convert.ToString(ddl_coltypeadd.SelectedItem.Value) + "'");
                //if (value.Trim() != "" && value.Trim() != null)
                //{
                //    ddl_priority.Items.Remove(value);
                //}
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); }
    }

    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv33.Visible = false;
    }

    protected void btn_deltype_OnClick(object sender, EventArgs e)
    {
        try
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='Leave Group Mapping' and CollegeCode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Deleted Successfully\");", true);
                bindtype();
            }
            else
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No records found\");", true);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); }
    }

    public void bindleavetype1()
    {
        try
        {
            string college = Convert.ToString(ddlcollege.SelectedItem.Value);
            height = 0;
            cblLeave1.ClearSelection();
            cblLeave1.Items.Clear();
            ds.Clear();
            string query = "Select category ,shortname  from leave_category where college_code in('" + college + "') and shortname <> 'LA'and shortname <>'PER' and shortname <> 'OD'and shortname <>'RL'and shortname <>'NA' and shortname <>'RL' order by category";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblLeave1.DataSource = ds;
                cblLeave1.DataTextField = "category";
                cblLeave1.DataValueField = "shortname";
                cblLeave1.DataBind();
            }
            for (int k = 0; k < cblLeave1.Items.Count; k++)
            {
                //cblLeave1.Items[k].Selected = true;
                height++;
            }
            if (height > 10)
                panelleave.Height = 300;
            else
                panelleave.Height = 100;
            //cblLeave1.Items.Insert(0, "P");
            //cblLeave1.Items[0].Value = "P";
            cblLeave1.Items.Insert(1, "A");
            cblLeave1.Items[1].Value = "A";
            cblLeave1.Items.Insert(2, "PER");
            cblLeave1.Items[2].Value = "PER";
            cblLeave1.Items.Insert(3, "LA");
            cblLeave1.Items[3].Value = "LA";
            cblLeave1.Items.Insert(4, "OD");
            cblLeave1.Items[4].Value = "OD";
            cblLeave1.Items.Insert(5, "RL");
            cblLeave1.Items[5].Value = "RL";
            cblLeave1.Items.Insert(6, "NA");
            cblLeave1.Items[6].Value = "NA";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); }
    }

    /* protected void btnPrintpdfClcik(object sender, EventArgs e)
     {
         try
         {
             Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
             Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
             //Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
             System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
             System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
             System.Drawing.Font Fontbold16 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
             System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
             System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
             System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
             System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
             System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
             System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
             System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
             System.Drawing.Font Fontboldu = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Underline);
             Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
             PdfTextArea collinfo1;
             mypdfpage = mydocument.NewPage();

             #region header Content
             int coltop = 0;
             string Collvalue = string.Empty;
             DataSet ds1 = new DataSet();
             ds1 = d2.select_method_wo_parameter("Select * from Collinfo where college_code=" + Convert.ToString(ddlcollege.SelectedItem.Value) + "", "Text");
             if (ds1.Tables[0].Rows.Count > 0)
             {
                 string collinfo = Convert.ToString(ds1.Tables[0].Rows[0]["collname"]);
                 string university = Convert.ToString(ds1.Tables[0].Rows[0]["university"]);
                 string affliatedby = Convert.ToString(ds1.Tables[0].Rows[0]["affliatedby"]);
                 string address1 = ds1.Tables[0].Rows[0]["Address1"].ToString();
                 string address2 = ds1.Tables[0].Rows[0]["Address2"].ToString();
                 string address3 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                 string district = ds1.Tables[0].Rows[0]["district"].ToString();
                 string state = ds1.Tables[0].Rows[0]["State"].ToString();
                 string pincode = ds1.Tables[0].Rows[0]["Pincode"].ToString();
                 string phone = ds1.Tables[0].Rows[0]["Phoneno"].ToString();
                 string fax = ds1.Tables[0].Rows[0]["Faxno"].ToString();
                 string email = ds1.Tables[0].Rows[0]["Email"].ToString();
                 string website = ds1.Tables[0].Rows[0]["Website"].ToString();
                 if (collinfo != "")
                 {
                     //if (collinfo.Contains("Gnanamani"))
                     //    collinfo = "Gnanamani Educational Institutions";
                     collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 25, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + collinfo);
                     mypdfpage.Add(collinfo1);
                 }
                 //if (university != "")
                 //{
                 //    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["university"].ToString() + "");
                 //    mypdfpage.Add(collinfo1);
                 //}
                 //else if (affliatedby != "")
                 //{
                 //    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                 //    mypdfpage.Add(collinfo1);
                 //}
                 if (address1 != "" || address1 != "" || address3 != "")
                 {
                     if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                         Collvalue = address1;
                     if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                     {
                         if (Collvalue.Trim() != "" && Collvalue != null)
                             Collvalue = Collvalue + ',' + ' ' + address2;
                         else
                             Collvalue = address2;
                     }
                     if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                     {
                         if (Collvalue.Trim() != "" && Collvalue != null)
                             Collvalue = Collvalue + ',' + ' ' + address3;
                         else
                             Collvalue = address3;
                     }
                     collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                     mypdfpage.Add(collinfo1);
                 }
                 else if (address3 != "")
                 {
                     string address11 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                     if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                         Collvalue = address1;

                     collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                     mypdfpage.Add(collinfo1);
                 }
                 if (district != "" || pincode != "")
                 {
                     if (district.Trim() != "" && district != null && district.Length > 1)
                     {
                         Collvalue = district;
                     }
                     if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                     {
                         if (Collvalue.Trim() != "" && Collvalue != null)
                             Collvalue = Collvalue + ' ' + '-' + ' ' + pincode;
                         else
                             Collvalue = pincode;
                     }
                     collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                     mypdfpage.Add(collinfo1);
                 }
                 if (phone != "" || fax != "")
                 {
                     if (phone.Trim() != "" && phone != null && phone.Length > 1)
                         Collvalue = "Phone :" + phone;
                     if (fax.Trim() != "" && fax != null && fax.Length > 1)
                     {
                         if (Collvalue.Trim() != "" && Collvalue != null)
                             Collvalue = Collvalue + " , Fax : " + fax;
                         else
                             Collvalue = "Fax :" + fax;
                     }
                     collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                     mypdfpage.Add(collinfo1);
                 }
                 if (email != "" || website != "")
                 {
                     if (email.Trim() != "" && email != null && email.Length > 1)
                         Collvalue = "Email :" + email;
                     if (website.Trim() != "" && website != null && website.Length > 1)
                     {
                         if (Collvalue.Trim() != "" && Collvalue != null)
                             Collvalue = Collvalue + " , Web Site : " + website;
                         else
                             Collvalue = "Web Site :" + website;
                     }
                     collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                     mypdfpage.Add(collinfo1);
                 }
                 if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                 {
                     PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                     mypdfpage.Add(LogoImage, 50, 20, 450);
                 }
             }
             #endregion
             FpSpread3.SaveChanges();
             PdfTextArea ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 0, 90, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "DAILY ATTENDANCE REPORT");
             mypdfpage.Add(ptc);
             ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                          new PdfArea(mydocument, -50, 90, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleRight, txtfromdate.Text);
             mypdfpage.Add(ptc);
             #region CategoryWise Count Report
             Gios.Pdf.PdfTable table = mydocument.NewTable(Fontsmall1bold, FpSpread3.Sheets[0].RowCount + 2, FpSpread3.Sheets[0].ColumnCount - 1, 1);
             table.VisibleHeaders = false;
             table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
             for (int col = 1; col < FpSpread3.Sheets[0].ColumnCount; col++)
             {
                 string ColumnHeaderName = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[1, col].Text));
                 string ColumnHeaderName1 = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[0, col].Text));
                 table.Columns[col - 1].SetWidth(80);
                 table.CellRange(0, 0, 0, 2).SetFont(Fontsmall1bold);
                 if (!string.IsNullOrEmpty(ColumnHeaderName))
                 {
                     //table.Cell(0, col - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                     //table.Cell(0, col - 1).SetContent(ColumnHeaderName1);
                 }
                 table.Cell(1, col - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                 table.Cell(1, col - 1).SetContent(ColumnHeaderName == "" ? ColumnHeaderName1 : ColumnHeaderName);
             }
             for (int row = 0; row < FpSpread3.Sheets[0].RowCount; row++)
             {
                 for (int col = 1; col < FpSpread3.Sheets[0].ColumnCount; col++)
                 {
                     string ColumnHeaderName = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].Cells[row, col].Text));
                     string ColumnHeaderName1 = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].Cells[row, col].Text));
                     table.Cell(row + 2, col - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                     table.Cell(row + 2, col - 1).SetContent(ColumnHeaderName == "" ? ColumnHeaderName1 : ColumnHeaderName);
                 }
             }
             Gios.Pdf.PdfTablePage PdfTable = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, 125, 550, 700));
             mypdfpage.Add(PdfTable);
             coltop += Convert.ToInt32(PdfTable.Area.Height) + 10;
             #endregion
             #region Staff Details
             ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 0, coltop + 105, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "DETAILS OF STAFF OF LEAVE / ABSENT");
             mypdfpage.Add(ptc);
             ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                          new PdfArea(mydocument, 0, coltop + 105, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "___________________________________________");
             mypdfpage.Add(ptc);
             ds1.Clear();
             ds1 = d2.select_method_wo_parameter("select MasterValue,MasterCode,MasterCriteriaValue1 from CO_MasterValues where MasterCriteria='Leave Group Mapping' and CollegeCode in('" + Convert.ToString(ddlcollege.SelectedItem.Value) + "') order by isnull(MasterCriteria1,1000) ", "text");
             coltop += 100;
             Gios.Pdf.PdfTable StaffAttendanceDetTbl;
             int tblRow = 0;
             int sno = 0;
             int ColCount = 0;
             for (int col = 0; col < FpSpread2.Sheets[0].ColumnCount; col++)
                 if (FpSpread2.Sheets[0].Columns[col].Visible == true)
                     ColCount++;
             int FalseCol = 0;
             bool visiblefalseCol = false;
             int rowHeightValue = 0;
             for (int row = 0; row < FpSpread3.Sheets[0].RowCount - 1; row++)
             {
                 tblRow = 0; sno = 0;
                 int ODColumnIndex = 0;
                 int ODRowCount = 0;
                 int LeaveTypeColumnCount = 0;
                 string CategoryCode = string.Empty;
                 for (int col = 1; col < FpSpread3.Sheets[0].ColumnCount; col++)
                 {
                     string ColumnCellValue = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].Cells[row, col].Text));
                     CategoryCode = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 0].Note);
                     string ColumnHeaderName = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[1, col].Text));
                     string ColumnHeaderName1 = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[1, col].Note));
                     string ColumnHeaderName2 = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[0, col].Text));
                     string CatagoryValue = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].Cells[row, 1].Text));
                     if (ColumnHeaderName2.ToUpper() == "NO. ON DUTY")
                     {
                         ODColumnIndex = col;
                         int ODrow = 0;
                         int.TryParse(ColumnCellValue, out ODrow);
                         ODRowCount += ODrow;
                     }
                     if (!string.IsNullOrEmpty(ColumnHeaderName))
                     {
                         tblRow = 0; sno = 0;
                         if (!string.IsNullOrEmpty(ColumnCellValue) && ColumnCellValue != "0")
                         {
                             coltop += 15;
                             int AbsentCnt = 0;
                             int.TryParse(ColumnCellValue, out AbsentCnt);
                             ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, ColumnHeaderName + "-" + CatagoryValue);
                             mypdfpage.Add(ptc);
                             coltop += 10;
                             #region StaffLeave Header
                             StaffAttendanceDetTbl = mydocument.NewTable(Fontsmall1bold, AbsentCnt + 1, ColCount, 1);
                             StaffAttendanceDetTbl.VisibleHeaders = false;
                             StaffAttendanceDetTbl.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                             visiblefalseCol = false;
                             FalseCol = 0;
                             LeaveTypeColumnCount = 0;
                             for (int c = 0; c < FpSpread2.Sheets[0].ColumnCount; c++)
                             {
                                 if (FpSpread2.Sheets[0].Columns[c].Visible == true)
                                 {
                                     string SpreadColumnHeaderName = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[1, c].Text);
                                     string SpreadColumnHeaderName1 = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, c].Text);
                                     if (SpreadColumnHeaderName.ToUpper() == "REASON")
                                         StaffAttendanceDetTbl.Columns[c + FalseCol].SetWidth(150);
                                     else
                                         StaffAttendanceDetTbl.Columns[c + FalseCol].SetWidth(60);
                                     if (SpreadColumnHeaderName == "Entry")
                                     {
                                         LeaveTypeColumnCount = c;
                                         StaffAttendanceDetTbl.Columns[c + FalseCol].SetWidth(50);
                                     }
                                     SpreadColumnHeaderName = SpreadColumnHeaderName == "Entry" ? "LeaveType" : SpreadColumnHeaderName;
                                     if (visiblefalseCol)
                                         FalseCol += -1;

                                     StaffAttendanceDetTbl.CellRange(0, 0, 0, c + FalseCol).SetFont(Fontsmall1bold);
                                     StaffAttendanceDetTbl.Cell(0, c + FalseCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                     StaffAttendanceDetTbl.Cell(0, c + FalseCol).SetContent(SpreadColumnHeaderName == "" ? SpreadColumnHeaderName1 : SpreadColumnHeaderName);
                                     visiblefalseCol = false;
                                 }
                                 else
                                     visiblefalseCol = true;
                             }
                             int Colwidth = 0;
                             if (!string.IsNullOrEmpty(ColumnHeaderName1))
                             {
                                 if (!string.IsNullOrEmpty(ColumnHeaderName1))
                                 {
                                     string[] LeaveType = ColumnHeaderName1.Split(',');
                                     foreach (var item in LeaveType)
                                     {
                                         for (int r = 0; r < FpSpread2.Sheets[0].RowCount; r++)
                                         {
                                             if (!string.IsNullOrEmpty(item))
                                             {
                                                 string CellValue = Convert.ToString(FpSpread2.Sheets[0].Cells[r, LeaveTypeColumnCount].Text);

                                                 string StaffDetCategoryCode = Convert.ToString(FpSpread2.Sheets[0].Cells[r, 0].Note);
                                                 int reasonColumncount = 0;
                                                 if (StaffDetCategoryCode == CategoryCode)
                                                 {
                                                     if (item == CellValue)
                                                     {
                                                         tblRow++; FalseCol = 0; sno++;
                                                         if (rowHeightValue + 300 > 841)
                                                         {
                                                             int createRowTbl = 0;
                                                             if (tblRow > 1)
                                                             {
                                                                 try
                                                                 {
                                                                     //createRowTbl = 0;
                                                                     //if (tblRow + 31 > AbsentCnt)
                                                                     //    createRowTbl = 31;
                                                                     //else
                                                                     //    createRowTbl = AbsentCnt + 2 - tblRow;
                                                                     foreach (PdfCell pr in StaffAttendanceDetTbl.CellRange(tblRow - 1, 0, AbsentCnt - 2, ColCount - 2).Cells)
                                                                     {
                                                                         pr.RowSpan = 2;
                                                                     }
                                                                 }
                                                                 catch { }
                                                                 PdfTable = StaffAttendanceDetTbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop + 20, 560, 700));
                                                                 mypdfpage.Add(PdfTable);
                                                             }
                                                             mypdfpage.SaveToDocument();
                                                             mypdfpage = mydocument.NewPage();
                                                             //createRowTbl = 0;
                                                             //if (tblRow + 31 > AbsentCnt)
                                                             //    createRowTbl = 31;
                                                             //else
                                                             //    createRowTbl = AbsentCnt + 2 - tblRow;
                                                             StaffAttendanceDetTbl = mydocument.NewTable(Fontsmall1bold, AbsentCnt + 2 - tblRow, ColCount, 1);
                                                             StaffAttendanceDetTbl.VisibleHeaders = false;
                                                             StaffAttendanceDetTbl.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                             visiblefalseCol = false;
                                                             FalseCol = 0;
                                                             LeaveTypeColumnCount = 0;
                                                             coltop += 10;
                                                             //         ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                             //new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, ColumnHeaderName + "-" + CatagoryValue);
                                                             //         mypdfpage.Add(ptc);
                                                             for (int c = 0; c < FpSpread2.Sheets[0].ColumnCount; c++)
                                                             {
                                                                 if (FpSpread2.Sheets[0].Columns[c].Visible == true)
                                                                 {
                                                                     string SpreadColumnHeaderName = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[1, c].Text);
                                                                     string SpreadColumnHeaderName1 = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, c].Text);
                                                                     if (SpreadColumnHeaderName == "Entry")
                                                                     {
                                                                         LeaveTypeColumnCount = c;
                                                                         StaffAttendanceDetTbl.Columns[c + FalseCol].SetWidth(50);
                                                                     }
                                                                     if (SpreadColumnHeaderName.ToUpper() == "REASON")
                                                                     {
                                                                         reasonColumncount = c + FalseCol;
                                                                         StaffAttendanceDetTbl.Columns[c + FalseCol].SetWidth(150);
                                                                     }
                                                                     else
                                                                         StaffAttendanceDetTbl.Columns[c + FalseCol].SetWidth(60);

                                                                     SpreadColumnHeaderName = SpreadColumnHeaderName == "Entry" ? "LeaveType" : SpreadColumnHeaderName;
                                                                     if (visiblefalseCol)
                                                                         FalseCol += -1;

                                                                     StaffAttendanceDetTbl.CellRange(0, 0, 0, c + FalseCol).SetFont(Fontsmall1bold);
                                                                     StaffAttendanceDetTbl.Cell(0, c + FalseCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                     StaffAttendanceDetTbl.Cell(0, c + FalseCol).SetContent(SpreadColumnHeaderName == "" ? SpreadColumnHeaderName1 : SpreadColumnHeaderName);
                                                                     visiblefalseCol = false;
                                                                 }
                                                                 else
                                                                     visiblefalseCol = true;
                                                             }
                                                             rowHeightValue = 0;
                                                             coltop = 20;
                                                             tblRow = 1;
                                                         }
                                                         visiblefalseCol = false;
                                                         FalseCol = 0;
                                                         for (int tblCol = 0; tblCol < FpSpread2.Sheets[0].ColumnCount; tblCol++)
                                                         {
                                                             if (FpSpread2.Sheets[0].Columns[tblCol].Visible == true)
                                                             {
                                                                 string tblCellValue = Convert.ToString(FpSpread2.Sheets[0].Cells[r, tblCol].Text);
                                                                 if (visiblefalseCol)
                                                                     FalseCol += -1;
                                                                 int.TryParse(Convert.ToString(FpSpread2.Sheets[0].Columns[tblCol].Width), out Colwidth);
                                                                 StaffAttendanceDetTbl.CellRange(tblRow, 0, tblRow, tblCol + FalseCol).SetFont(Fontsmall1);
                                                                 if (tblCol != 0)
                                                                 {
                                                                     if (LeaveTypeColumnCount == tblCol)
                                                                     {
                                                                         StaffAttendanceDetTbl.Cell(tblRow, tblCol + FalseCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                         StaffAttendanceDetTbl.Columns[tblCol + FalseCol].SetWidth(50);
                                                                     }
                                                                     else
                                                                     {
                                                                         StaffAttendanceDetTbl.Cell(tblRow, tblCol + FalseCol).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                         StaffAttendanceDetTbl.Columns[tblCol + FalseCol].SetWidth(60);
                                                                     }
                                                                     if (reasonColumncount == tblCol)
                                                                         StaffAttendanceDetTbl.Columns[tblCol + FalseCol].SetWidth(150);
                                                                     StaffAttendanceDetTbl.Cell(tblRow, tblCol + FalseCol).SetContent(tblCellValue);
                                                                 }
                                                                 else
                                                                 {
                                                                     StaffAttendanceDetTbl.Cell(tblRow, tblCol + FalseCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                     StaffAttendanceDetTbl.Cell(tblRow, tblCol + FalseCol).SetContent(sno);
                                                                     StaffAttendanceDetTbl.Columns[tblCol].SetWidth(25);
                                                                 }
                                                                 visiblefalseCol = false;
                                                             }
                                                             else
                                                                 visiblefalseCol = true;
                                                         }
                                                         rowHeightValue += 18;
                                                     }
                                                 }
                                             }
                                         }
                                     }
                                 }
                             }
                             PdfTable = StaffAttendanceDetTbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop + 20, 560, 700));
                             mypdfpage.Add(PdfTable);
                             coltop += Convert.ToInt32(PdfTable.Area.Height);
                             #endregion
                         }
                     }
                 }
                 #region OD Details
                 if (ODColumnIndex != 0 && ODRowCount > 0)
                 {
                     coltop += 10;
                     Gios.Pdf.PdfTable ODtbl = mydocument.NewTable(Fontsmall1bold, ODRowCount + 1, ColCount, 1);
                     if (rowHeightValue + 300 > 800)
                     {
                         PdfTable = ODtbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop + 20, 560, 700));
                         mypdfpage.Add(PdfTable);
                         mypdfpage.SaveToDocument();
                         mypdfpage = mydocument.NewPage();
                         ODtbl = mydocument.NewTable(Fontsmall1bold, ODRowCount + 1, ColCount, 1);
                         rowHeightValue = 0;
                         coltop = 20;
                     }
                     ODtbl.VisibleHeaders = false;
                     ODtbl.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                     tblRow = 0;
                     ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "ON DUTY");
                     mypdfpage.Add(ptc);
                     coltop += 10;
                     for (int r = 0; r < FpSpread2.Sheets[0].RowCount; r++)
                     {
                         visiblefalseCol = false;
                         FalseCol = 0;
                         string StaffDetCategoryCode = Convert.ToString(FpSpread2.Sheets[0].Cells[r, 0].Note);
                         int reasonColumncount = 0;
                         if (r == 0)
                         {
                             if (rowHeightValue + 300 > 800)
                             {
                                 PdfTable = ODtbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop + 20, 560, 700));
                                 mypdfpage.Add(PdfTable);
                                 mypdfpage.SaveToDocument();
                                 mypdfpage = mydocument.NewPage();
                                 ODtbl = mydocument.NewTable(Fontsmall1bold, ODRowCount + 1, ColCount, 1);
                                 rowHeightValue = 0;
                                 coltop = 20;
                             }
                             ODtbl.VisibleHeaders = false;
                             ODtbl.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                             for (int c = 0; c < FpSpread2.Sheets[0].ColumnCount; c++)
                             {
                                 if (FpSpread2.Sheets[0].Columns[c].Visible == true)
                                 {
                                     string SpreadColumnHeaderName = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[1, c].Text);
                                     string SpreadColumnHeaderName1 = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, c].Text);
                                     if (SpreadColumnHeaderName == "Entry")
                                         LeaveTypeColumnCount = c;
                                     SpreadColumnHeaderName = SpreadColumnHeaderName == "Entry" ? "LeaveType" : SpreadColumnHeaderName;
                                     if (visiblefalseCol)
                                         FalseCol += -1;
                                     if (SpreadColumnHeaderName.ToUpper() == "REASON")
                                         ODtbl.Columns[c + FalseCol].SetWidth(150);
                                     else
                                         ODtbl.Columns[c + FalseCol].SetWidth(60);
                                     if (c == 0)
                                         ODtbl.Columns[c + FalseCol].SetWidth(25);
                                     ODtbl.CellRange(0, 0, 0, c + FalseCol).SetFont(Fontsmall1bold);
                                     ODtbl.Cell(0, c + FalseCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                     ODtbl.Cell(0, c + FalseCol).SetContent(SpreadColumnHeaderName == "" ? SpreadColumnHeaderName1 : SpreadColumnHeaderName);
                                     visiblefalseCol = false;
                                 }
                                 else
                                     visiblefalseCol = true;
                             }
                             rowHeightValue += 18;
                         }
                         if (StaffDetCategoryCode == CategoryCode)
                         {
                             FalseCol = 0;
                             string CellValue = Convert.ToString(FpSpread2.Sheets[0].Cells[r, LeaveTypeColumnCount].Text);
                             if (CellValue.ToUpper() == "OD" || CellValue.ToUpper() == "OOD")
                             {
                                 tblRow++;
                                 for (int tblCol = 0; tblCol < FpSpread2.Sheets[0].ColumnCount; tblCol++)
                                 {
                                     if (FpSpread2.Sheets[0].Columns[tblCol].Visible == true)
                                     {
                                         string tblCellValue = Convert.ToString(FpSpread2.Sheets[0].Cells[r, tblCol].Text);
                                         if (visiblefalseCol)
                                             FalseCol += -1;
                                         ODtbl.CellRange(tblRow, 0, tblRow, tblCol + FalseCol).SetFont(Fontsmall1);
                                         if (tblCol != 0)
                                         {
                                             if (LeaveTypeColumnCount == tblCol)
                                                 ODtbl.Cell(tblRow, tblCol + FalseCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                             else
                                                 ODtbl.Cell(tblRow, tblCol + FalseCol).SetContentAlignment(ContentAlignment.MiddleLeft);
                                             ODtbl.Cell(tblRow, tblCol + FalseCol).SetContent(tblCellValue);
                                             ODtbl.Columns[tblCol + FalseCol].SetWidth(60);
                                         }
                                         else
                                         {
                                             ODtbl.Cell(tblRow, tblCol + FalseCol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                             ODtbl.Cell(tblRow, tblCol + FalseCol).SetContent(tblRow);
                                             ODtbl.Columns[tblCol].SetWidth(25);
                                         }
                                         visiblefalseCol = false;
                                     }
                                     else
                                         visiblefalseCol = true;
                                 }
                             }
                         }
                     }
                     PdfTable = ODtbl.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop + 20, 560, 700));
                     mypdfpage.Add(PdfTable);
                     coltop += Convert.ToInt32(PdfTable.Area.Height);
                 }
                 #endregion

             }
             ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, 20, coltop + 30, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.BottomLeft, "HRD");
             mypdfpage.Add(ptc);

             ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                             new PdfArea(mydocument, -50, coltop + 30, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.BottomRight, "PRINCIPAL");
             mypdfpage.Add(ptc);
             #endregion
             mypdfpage.SaveToDocument();
             string appPath = HttpContext.Current.Server.MapPath("~");
             if (appPath != "")
             {
                 string szPath = appPath + "/Report/";
                 string szFile = "DailyAttendanceReport" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                 Response.Buffer = true;
                 Response.Clear();
                 mydocument.SaveToFile(szPath + szFile);
                 Response.ClearHeaders();
                 Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                 Response.ContentType = "application/pdf";
                 Response.WriteFile(szPath + szFile);
             }
         }
         catch (Exception ex)
         { //d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report"); 
         }
     }*/

    protected DateTime ReturnDate(string Date)
    {
        DateTime dateTime = new DateTime();
        try
        {
            string[] Splitdate = Date.Split('/');
            if (Splitdate.Length >= 3)
            {
                dateTime = Convert.ToDateTime(Splitdate[1] + "/" + Splitdate[0] + "/" + Splitdate[2]);
            }
        }
        catch { return dateTime; }
        return dateTime;
    }

    protected void btnPrintpdfClcik(object sender, EventArgs e)
    {
        try
        {
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            //Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold16 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            //  System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
            System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Cambria", 9, FontStyle.Regular);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            System.Drawing.Font Fontboldu = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Underline);
            Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
            PdfTextArea collinfo1;
            mypdfpage = mydocument.NewPage();

            #region header Content
            int coltop = 0;
            string Collvalue = string.Empty;
            DataSet ds1 = new DataSet();
            ds1 = d2.select_method_wo_parameter("Select * from Collinfo where college_code=" + Convert.ToString(ddlcollege.SelectedItem.Value) + "", "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string collinfo = Convert.ToString(ds1.Tables[0].Rows[0]["collname"]);
                string university = Convert.ToString(ds1.Tables[0].Rows[0]["university"]);
                string affliatedby = Convert.ToString(ds1.Tables[0].Rows[0]["affliatedby"]);
                string address1 = ds1.Tables[0].Rows[0]["Address1"].ToString();
                string address2 = ds1.Tables[0].Rows[0]["Address2"].ToString();
                string address3 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                string district = ds1.Tables[0].Rows[0]["district"].ToString();
                string state = ds1.Tables[0].Rows[0]["State"].ToString();
                string pincode = ds1.Tables[0].Rows[0]["Pincode"].ToString();
                string phone = ds1.Tables[0].Rows[0]["Phoneno"].ToString();
                string fax = ds1.Tables[0].Rows[0]["Faxno"].ToString();
                string email = ds1.Tables[0].Rows[0]["Email"].ToString();
                string website = ds1.Tables[0].Rows[0]["Website"].ToString();
                if (collinfo != "")
                {
                    //if (collinfo.Contains("Gnanamani"))
                    //    collinfo = "Gnanamani Educational Institutions";
                    collinfo1 = new PdfTextArea(Fontbold16, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 25, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.TopCenter, "" + collinfo);
                    mypdfpage.Add(collinfo1);
                }
                //if (university != "")
                //{
                //    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["university"].ToString() + "");
                //    mypdfpage.Add(collinfo1);
                //}
                //else if (affliatedby != "")
                //{
                //    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds1.Tables[0].Rows[0]["affliatedby"].ToString() + "");
                //    mypdfpage.Add(collinfo1);
                //}
                if (address1 != "" || address1 != "" || address3 != "")
                {
                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                        Collvalue = address1;
                    if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                            Collvalue = Collvalue + ',' + ' ' + address2;
                        else
                            Collvalue = address2;
                    }
                    if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                            Collvalue = Collvalue + ',' + ' ' + address3;
                        else
                            Collvalue = address3;
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypdfpage.Add(collinfo1);
                }
                else if (address3 != "")
                {
                    string address11 = ds1.Tables[0].Rows[0]["Address3"].ToString();
                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                        Collvalue = address1;

                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 45, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypdfpage.Add(collinfo1);
                }
                if (district != "" || pincode != "")
                {
                    if (district.Trim() != "" && district != null && district.Length > 1)
                    {
                        Collvalue = district;
                    }
                    if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                            Collvalue = Collvalue + ' ' + '-' + ' ' + pincode;
                        else
                            Collvalue = pincode;
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 60, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypdfpage.Add(collinfo1);
                }
                if (phone != "" || fax != "")
                {
                    if (phone.Trim() != "" && phone != null && phone.Length > 1)
                        Collvalue = "Phone :" + phone;
                    if (fax.Trim() != "" && fax != null && fax.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                            Collvalue = Collvalue + " , Fax : " + fax;
                        else
                            Collvalue = "Fax :" + fax;
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 75, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypdfpage.Add(collinfo1);
                }
                if (email != "" || website != "")
                {
                    if (email.Trim() != "" && email != null && email.Length > 1)
                        Collvalue = "Email :" + email;
                    if (website.Trim() != "" && website != null && website.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                            Collvalue = Collvalue + " , Web Site : " + website;
                        else
                            Collvalue = "Web Site :" + website;
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydocument, 0, 90, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypdfpage.Add(collinfo1);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 50, 20, 450);
                }
            }
            #endregion
            FpSpread3.SaveChanges();
            PdfTextArea ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, 0, 90, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "DAILY ATTENDANCE REPORT");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                         new PdfArea(mydocument, -50, 90, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleRight, txtfromdate.Text);
            mypdfpage.Add(ptc);
            DataTable Btbl = new DataTable();
            DataColumn dc;
            DataRow dr;
            #region CategoryWise Count Report
            Gios.Pdf.PdfTable table = mydocument.NewTable(Fontsmall1bold, FpSpread3.Sheets[0].RowCount + 1, FpSpread3.Sheets[0].ColumnCount - 1, 1);
            table.VisibleHeaders = false;
            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            for (int col = 1; col < FpSpread3.Sheets[0].ColumnCount; col++)
            {
                dc = new DataColumn();
                string ColumnHeaderName = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[1, col].Text));
                string ColumnHeaderName1 = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[0, col].Text));
                table.Columns[col - 1].SetWidth(80);
                table.CellRange(0, 0, 0, 2).SetFont(Fontsmall1bold);
                if (!string.IsNullOrEmpty(ColumnHeaderName))
                {
                    //table.Cell(0, col - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    //table.Cell(0, col - 1).SetContent(ColumnHeaderName1);
                }
                table.Cell(0, col - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, col - 1).SetContent(ColumnHeaderName == "" ? ColumnHeaderName1 : ColumnHeaderName);
                dc.ColumnName = (ColumnHeaderName == "" ? ColumnHeaderName1 : ColumnHeaderName);
                Btbl.Columns.Add(dc);
            }
            for (int row = 0; row < FpSpread3.Sheets[0].RowCount; row++)
            {
                dr = Btbl.NewRow();
                for (int col = 1; col < FpSpread3.Sheets[0].ColumnCount; col++)
                {
                    string ColumnHeaderName = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].Cells[row, col].Text));
                    string ColumnHeaderName1 = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].Cells[row, col].Text));
                    table.Cell(row + 1, col - 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(row + 1, col - 1).SetContent(ColumnHeaderName == "" ? ColumnHeaderName1 : ColumnHeaderName);
                    dr[col - 1] = (ColumnHeaderName == "" ? ColumnHeaderName1 : ColumnHeaderName);
                }
                Btbl.Rows.Add(dr);
            }
            Gios.Pdf.PdfTablePage PdfTable = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, 125, 550, 700));
            mypdfpage.Add(PdfTable);
            coltop += Convert.ToInt32(PdfTable.Area.Height) + 20;

            #endregion
            #region Staff Details
            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 0, coltop + 100, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "DETAILS OF STAFF OF LEAVE / ABSENT");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black,
                                         new PdfArea(mydocument, 0, coltop + 100, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "_______________________________________");
            mypdfpage.Add(ptc);
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter("select MasterValue,MasterCode,MasterCriteriaValue1 from CO_MasterValues where MasterCriteria='Leave Group Mapping' and CollegeCode in('" + Convert.ToString(ddlcollege.SelectedItem.Value) + "') order by isnull(MasterCriteria1,1000) ", "text");
            //coltop += 100;

            Btbl = new DataTable();
            for (int c = 0; c < FpSpread2.Sheets[0].ColumnCount; c++)
            {
                if (FpSpread2.Sheets[0].Columns[c].Visible == true)
                {
                    dc = new DataColumn();
                    string SpreadColumnHeaderName = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[1, c].Text);
                    string SpreadColumnHeaderName1 = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, c].Text);

                    string headerName = (SpreadColumnHeaderName == "" ? SpreadColumnHeaderName1 : SpreadColumnHeaderName);
                    SpreadColumnHeaderName = (headerName == "Entry" ? "LeaveType" : headerName);
                    dc.ColumnName = SpreadColumnHeaderName;
                    Btbl.Columns.Add(dc);
                }
            }
            int tblRow = 0;
            int sno = 0;
            int ColCount = 0;
            for (int col = 0; col < FpSpread2.Sheets[0].ColumnCount; col++)
                if (FpSpread2.Sheets[0].Columns[col].Visible == true)
                    ColCount++;
            int FalseCol = 0;
            bool visiblefalseCol = false;
            int rowHeightValue = 0;
            for (int row = 0; row < FpSpread3.Sheets[0].RowCount - 1; row++)
            {
                tblRow = 0; sno = 0;
                int ODColumnIndex = 0;
                int ODRowCount = 0;
                int LeaveTypeColumnCount = 0;
                string CategoryCode = string.Empty;
                for (int col = 1; col < FpSpread3.Sheets[0].ColumnCount; col++)
                {
                    string ColumnCellValue = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].Cells[row, col].Text));
                    CategoryCode = Convert.ToString(FpSpread3.Sheets[0].Cells[row, 0].Note);
                    string ColumnHeaderName = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[1, col].Text));
                    string ColumnHeaderName1 = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[1, col].Note));
                    string ColumnHeaderName2 = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[0, col].Text));
                    string CatagoryValue = Convert.ToString(Convert.ToString(FpSpread3.Sheets[0].Cells[row, 1].Text));
                    if (ColumnHeaderName2.ToUpper() == "NO. ON DUTY")
                    {
                        ODColumnIndex = col;
                        int ODrow = 0;
                        int.TryParse(ColumnCellValue, out ODrow);
                        ODRowCount += ODrow;
                    }
                    if (!string.IsNullOrEmpty(ColumnHeaderName))
                    {
                        tblRow = 0; sno = 0;
                        if (!string.IsNullOrEmpty(ColumnCellValue) && ColumnCellValue != "0")
                        {
                            int AbsentCnt = 0;
                            int.TryParse(ColumnCellValue, out AbsentCnt);
                            dr = Btbl.NewRow();
                            dr[0] = ColumnHeaderName + "-" + CatagoryValue;
                            Btbl.Rows.Add(dr);
                            #region StaffLeave Header
                            visiblefalseCol = false;
                            FalseCol = 0;
                            LeaveTypeColumnCount = 0;
                            for (int c = 0; c < FpSpread2.Sheets[0].ColumnCount; c++)
                            {
                                if (FpSpread2.Sheets[0].Columns[c].Visible == true)
                                {
                                    string SpreadColumnHeaderName = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[1, c].Text);
                                    string SpreadColumnHeaderName1 = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, c].Text);
                                    if (SpreadColumnHeaderName == "Entry")
                                    {
                                        LeaveTypeColumnCount = c;
                                    }
                                    SpreadColumnHeaderName = SpreadColumnHeaderName == "Entry" ? "LeaveType" : SpreadColumnHeaderName;
                                    if (visiblefalseCol)
                                        FalseCol += -1;
                                    visiblefalseCol = false;
                                }
                                else
                                    visiblefalseCol = true;
                            }
                            int Colwidth = 0;
                            if (!string.IsNullOrEmpty(ColumnHeaderName1))
                            {
                                if (!string.IsNullOrEmpty(ColumnHeaderName1))
                                {
                                    string[] LeaveType = ColumnHeaderName1.Split(',');
                                    foreach (var item in LeaveType)
                                    {
                                        for (int r = 0; r < FpSpread2.Sheets[0].RowCount; r++)
                                        {
                                            if (!string.IsNullOrEmpty(item))
                                            {
                                                string CellValue = Convert.ToString(FpSpread2.Sheets[0].Cells[r, LeaveTypeColumnCount].Text);

                                                string StaffDetCategoryCode = Convert.ToString(FpSpread2.Sheets[0].Cells[r, 0].Note);
                                                int reasonColumncount = 0;
                                                if (StaffDetCategoryCode == CategoryCode)
                                                {
                                                    if (item == CellValue)
                                                    {
                                                        tblRow++; FalseCol = 0; sno++;
                                                        //if (rowHeightValue + 300 > 841)
                                                        //{
                                                        //    visiblefalseCol = false;
                                                        //    FalseCol = 0;
                                                        //    LeaveTypeColumnCount = 0;
                                                        //    for (int c = 0; c < FpSpread2.Sheets[0].ColumnCount; c++)
                                                        //    {
                                                        //        if (FpSpread2.Sheets[0].Columns[c].Visible == true)
                                                        //        {
                                                        //            string SpreadColumnHeaderName = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[1, c].Text);
                                                        //            string SpreadColumnHeaderName1 = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, c].Text);
                                                        //            if (SpreadColumnHeaderName == "Entry")
                                                        //            {
                                                        //                LeaveTypeColumnCount = c;
                                                        //            }
                                                        //            if (SpreadColumnHeaderName.ToUpper() == "REASON")
                                                        //            {
                                                        //                reasonColumncount = c + FalseCol;
                                                        //            }
                                                        //            SpreadColumnHeaderName = SpreadColumnHeaderName == "Entry" ? "LeaveType" : SpreadColumnHeaderName;
                                                        //            if (visiblefalseCol)
                                                        //                FalseCol += -1;
                                                        //            visiblefalseCol = false;
                                                        //        }
                                                        //        else
                                                        //            visiblefalseCol = true;
                                                        //    }
                                                        //    rowHeightValue = 0;
                                                        //    tblRow = 1;
                                                        //}
                                                        visiblefalseCol = false;
                                                        FalseCol = 0;
                                                        dr = Btbl.NewRow();
                                                        for (int tblCol = 0; tblCol < FpSpread2.Sheets[0].ColumnCount; tblCol++)
                                                        {
                                                            if (FpSpread2.Sheets[0].Columns[tblCol].Visible == true)
                                                            {
                                                                string tblCellValue = Convert.ToString(FpSpread2.Sheets[0].Cells[r, tblCol].Text);
                                                                if (visiblefalseCol)
                                                                    FalseCol += -1;
                                                                int.TryParse(Convert.ToString(FpSpread2.Sheets[0].Columns[tblCol].Width), out Colwidth);
                                                                if (tblCol != 0)
                                                                {
                                                                    dr[tblCol + FalseCol] = tblCellValue;
                                                                }
                                                                else
                                                                {
                                                                    dr[tblCol + FalseCol] = sno;
                                                                }
                                                                visiblefalseCol = false;
                                                            }
                                                            else
                                                                visiblefalseCol = true;
                                                        }
                                                        Btbl.Rows.Add(dr);
                                                        rowHeightValue += 18;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                #region OD Details
                if (ODColumnIndex != 0 && ODRowCount > 0)
                {
                    tblRow = 0; sno = 0;
                    dr = Btbl.NewRow();
                    dr[0] = "ON DUTY";
                    Btbl.Rows.Add(dr);
                    for (int r = 0; r < FpSpread2.Sheets[0].RowCount; r++)
                    {
                        dr = Btbl.NewRow();
                        visiblefalseCol = false;
                        FalseCol = 0;
                        string StaffDetCategoryCode = Convert.ToString(FpSpread2.Sheets[0].Cells[r, 0].Note);
                        if (StaffDetCategoryCode == CategoryCode)
                        {
                            FalseCol = 0;
                            string CellValue = Convert.ToString(FpSpread2.Sheets[0].Cells[r, LeaveTypeColumnCount].Text);
                            if (CellValue.ToUpper() == "OD" || CellValue.ToUpper() == "OOD")
                            {
                                tblRow++; sno++;
                                for (int tblCol = 0; tblCol < FpSpread2.Sheets[0].ColumnCount; tblCol++)
                                {
                                    if (FpSpread2.Sheets[0].Columns[tblCol].Visible == true)
                                    {
                                        string tblCellValue = Convert.ToString(FpSpread2.Sheets[0].Cells[r, tblCol].Text);
                                        if (visiblefalseCol)
                                            FalseCol += -1;
                                        if (tblCol != 0)
                                        {
                                            dr[tblCol + FalseCol] = tblCellValue;
                                        }
                                        else
                                        {
                                            dr[tblCol + FalseCol] = sno;
                                        }
                                        visiblefalseCol = false;
                                    }
                                    else
                                        visiblefalseCol = true;
                                }
                                Btbl.Rows.Add(dr);
                            }
                        }
                    }
                }
                #endregion
            }
            #region Bind StaffDetails
            Gios.Pdf.PdfTable StaffAttendanceDet;
            Gios.Pdf.PdfTablePage newpdftabpage2;
            coltop += Convert.ToInt32(PdfTable.Area.Height) + 20;
            int tblcount = 0;

            int rowPrintCount = 35;
            if (txtPrint.Text != "")
                int.TryParse(txtPrint.Text, out rowPrintCount);
            //  rowPrintCount -= 10;
            if (Btbl.Rows.Count > rowPrintCount)
                tblcount = rowPrintCount;
            else
                tblcount = Btbl.Rows.Count + 1;
            int val = rowPrintCount;
            int tableCount = 0;
            StaffAttendanceDet = mydocument.NewTable(Fontsmall1bold, tblcount, Btbl.Columns.Count, 2);
            StaffAttendanceDet.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
            StaffAttendanceDet.VisibleHeaders = false;
            for (int c = 0; c < Btbl.Columns.Count; c++)
            {
                string ColumnName = Convert.ToString(Btbl.Columns[c].ColumnName);
                if (ColumnName.ToUpper() == "REASON")
                    StaffAttendanceDet.Columns[c].SetWidth(150);
                else if (ColumnName.ToUpper() == "LEAVETYPE")
                    StaffAttendanceDet.Columns[c].SetWidth(50);
                else if (ColumnName.ToUpper() == "S.NO")
                    StaffAttendanceDet.Columns[c].SetWidth(30);
                else if (ColumnName.ToUpper() == "STAFF NAME" || ColumnName.ToUpper() == "DEPARTMENT" || ColumnName.ToUpper() == "DESIGNATION")
                    StaffAttendanceDet.Columns[c].SetWidth(100);
                else if (ColumnName.ToUpper() == "SESSION")
                    StaffAttendanceDet.Columns[c].SetWidth(40);
                else
                    StaffAttendanceDet.Columns[c].SetWidth(60);
                StaffAttendanceDet.Cell(0, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                StaffAttendanceDet.Cell(0, c).SetContent(ColumnName);
            }
            int tblheight = 0;

            if (Btbl.Rows.Count > 0)
            {

                coltop += 70;
                int row = 1;
                int modcount = 1;
                bool checkcon = true;
                for (int m = 1; m <= Btbl.Rows.Count; m++)
                {
                    val--;
                    //  if (row - 1 == rowPrintCount )
                    //  checkcon = true;
                    // if (m % rowPrintCount == 0)//&& checkcon
                    //{
                    if (val == 0)//delsi1902
                    {
                        val = rowPrintCount + 10;
                        newpdftabpage2 = StaffAttendanceDet.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 560, 800));
                        mypdfpage.Add(newpdftabpage2);
                        tblheight = (int)newpdftabpage2.Area.Height;
                        coltop += (int)tblheight + 25;
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydocument.NewPage();
                        coltop = 40;
                        //if (checkcon)
                        //    rowPrintCount += 10;
                        //checkcon = false;

                        //if (Btbl.Rows.Count + 1 - (modcount * rowPrintCount - 1) > rowPrintCount)
                        //{
                        //    tblcount = rowPrintCount;
                        //}
                        //else
                        //{
                        //    tblcount = Btbl.Rows.Count + 1 - (modcount * rowPrintCount);
                        //}
                        int tab_val = 0;
                        tab_val = Btbl.Rows.Count - tableCount;

                        if (tab_val > val)
                        {
                            tblcount = val;
                        }
                        else
                        {
                            tblcount = tab_val;
                        }

                        StaffAttendanceDet = mydocument.NewTable(Fontsmall1bold, tblcount + 1, Btbl.Columns.Count, 2);
                        StaffAttendanceDet.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
                        StaffAttendanceDet.VisibleHeaders = false;
                        for (int c = 0; c < Btbl.Columns.Count; c++)
                        {
                            string ColumnName = Convert.ToString(Btbl.Columns[c].ColumnName);
                            if (ColumnName.ToUpper() == "REASON")
                                StaffAttendanceDet.Columns[c].SetWidth(150);
                            else if (ColumnName.ToUpper() == "LEAVETYPE")
                                StaffAttendanceDet.Columns[c].SetWidth(50);
                            else if (ColumnName.ToUpper() == "S.NO")
                                StaffAttendanceDet.Columns[c].SetWidth(30);
                            else if (ColumnName.ToUpper() == "STAFF NAME" || ColumnName.ToUpper() == "DEPARTMENT" || ColumnName.ToUpper() == "DESIGNATION")
                                StaffAttendanceDet.Columns[c].SetWidth(100);
                            else if (ColumnName.ToUpper() == "SESSION")
                                StaffAttendanceDet.Columns[c].SetWidth(40);
                            else
                                StaffAttendanceDet.Columns[c].SetWidth(60);

                            StaffAttendanceDet.Cell(0, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                            StaffAttendanceDet.Cell(0, c).SetContent(ColumnName);
                        }
                        row = 1;
                        modcount++;
                    }
                    for (int c = 0; c < Btbl.Columns.Count; c++)
                    {
                        string Rowvalue = Convert.ToString(Btbl.Rows[m - 1][c]);
                        string ColumnName = Convert.ToString(Btbl.Columns[c].ColumnName);
                        StaffAttendanceDet.Cell(row, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                        if (ColumnName.ToUpper() == "S.NO")
                            StaffAttendanceDet.Cell(row, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                        else if (ColumnName.ToUpper() == "LEAVETYPE")
                            StaffAttendanceDet.Cell(row, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                        StaffAttendanceDet.Cell(row, c).SetContent(Rowvalue);
                        StaffAttendanceDet.Cell(row, c).SetFont(Fontsmall1bold);
                        if (c == 1 && string.IsNullOrEmpty(Rowvalue))
                            foreach (PdfCell pr in StaffAttendanceDet.CellRange(row, 0, row, 0).Cells)
                                pr.ColSpan = Btbl.Columns.Count;
                    }

                    row++;
                    tableCount++;

                }
                newpdftabpage2 = StaffAttendanceDet.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 560, 800));
                mypdfpage.Add(newpdftabpage2);
                tblheight = (int)newpdftabpage2.Area.Height;
                coltop += (int)tblheight;
            }
            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 20, coltop + 30, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.BottomLeft, "HRD");
            mypdfpage.Add(ptc);

            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            new PdfArea(mydocument, -50, coltop + 30, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.BottomRight, "PRINCIPAL");
            mypdfpage.Add(ptc);
            #endregion

            #endregion
            mypdfpage.SaveToDocument();
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "DailyAttendanceReport" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                Response.Buffer = true;
                Response.Clear();
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "StaffAttendance_Report");
        }
    }

}
