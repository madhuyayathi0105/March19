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

public partial class HRMOD_CompensationReport : System.Web.UI.Page
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
    ReuasableMethods rs = new ReuasableMethods();
    ArrayList leave = new ArrayList();
    //Hashtable hat = new Hashtable();
    Hashtable hat1 = new Hashtable();
    Hashtable hascount = new Hashtable();
    string[] sarray5 = new string[15];
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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindclg();
                binddept();
                designation();
                category();
                stafftype();
                BindStaff();
                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                rprint.Visible = false;
                // txt_CompDate.Text = DateTime.Now.ToString("dd/MM/yyy");
            }
        }
        catch (Exception ex)
        { }
    }
    protected void bindclg()
    {
        try
        {
            ds.Clear();
            cbl_clg.Items.Clear();
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

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_clg.DataSource = ds;
                cbl_clg.DataTextField = "collname";
                cbl_clg.DataValueField = "college_code";
                cbl_clg.DataBind();
                if (cbl_clg.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_clg.Items.Count; i++)
                    {
                        cbl_clg.Items[i].Selected = true;
                    }
                    txt_clg.Text = "College (" + cbl_clg.Items.Count + ")";
                    cb_clg.Checked = true;
                }
            }
            else
            {
                txt_clg.Text = "--Select--";
                cb_clg.Checked = false;
            }
        }
        catch { }
    }
    protected void cb_clg_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_clg, cbl_clg, txt_clg, "College");
        binddept();
        designation();
        category();
        stafftype();
        BindStaff();
    }

    protected void cbl_clg_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_clg, cbl_clg, txt_clg, "College");
        binddept();
        designation();
        category();
        stafftype();
        BindStaff();


    }
    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
        BindStaff();
    }
    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
        BindStaff();
    }

    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string getcolcode = rs.GetSelectedItemsValueAsString(cbl_clg);
            string item = "select distinct dept_code,dept_name from hrdept_master where college_code  in('" + getcolcode + "') order by dept_name";
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
    protected void cb_desig_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void cbl_desig_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_desig, cbl_desig, txt_desig, "Designation");
    }
    protected void designation()
    {
        try
        {
            ds.Clear();
            cbl_desig.Items.Clear();
            string getcolcode = rs.GetSelectedItemsValueAsString(cbl_clg);
            string statequery = "select desig_code,desig_name from desig_master where collegeCode in('" + getcolcode + "') order by desig_name";
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
    protected void cb_staffc_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_staffc, cbl_staffc, txt_staffc, "Category");
        BindStaff();
    }
    protected void cbl_staffc_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_staffc, cbl_staffc, txt_staffc, "Category");
        BindStaff();
    }

    protected void category()
    {
        try
        {
            ds.Clear();
            cbl_staffc.Items.Clear();
            string getcolcode = rs.GetSelectedItemsValueAsString(cbl_clg);
            string statequery = "select category_code,category_Name from staffcategorizer where college_code  in('" + getcolcode + "') order by category_Name";
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
    protected void cb_stype_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_stype, cbl_stype, txt_stype, "StaffType");
        BindStaff();
    }
    protected void cbl_stype_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stype, cbl_stype, txt_stype, "StaffType");
        BindStaff();
    }

    protected void stafftype()
    {
        try
        {
            ds.Clear();
            cbl_stype.Items.Clear();
            string getcolcode = rs.GetSelectedItemsValueAsString(cbl_clg);
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code in('" + getcolcode + "') order by stftype";
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

    protected void cb_staffcode_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_staffCode, cbl_staffCode, txt_staffCode, "Staff Code");
    }
    protected void cbl_staffcode_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_staffCode, cbl_staffCode, txt_staffCode, "Staff Code");
    }

    public void BindStaff()
    {
        try
        {
            string getcolcode = rs.GetSelectedItemsValueAsString(cbl_clg);
            string deptcode = "";
            for (int item = 0; item < cbl_dept.Items.Count; item++)
            {
                if (cbl_dept.Items[item].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = cbl_dept.Items[item].Value;
                    }
                    else
                    {
                        deptcode = deptcode + ',' + cbl_dept.Items[item].Value;
                    }
                }
            }
            if (deptcode != "")
            {
                deptcode = "and st.dept_code in(" + deptcode + ")";
            }

            string designcode = "";
            for (int item = 0; item < cbl_desig.Items.Count; item++)
            {
                if (cbl_desig.Items[item].Selected == true)
                {
                    if (designcode == "")
                    {
                        designcode = "'" + cbl_desig.Items[item].Value + "'";
                    }
                    else
                    {
                        designcode = designcode + ',' + "'" + cbl_desig.Items[item].Value + "'";
                    }
                }
            }
            if (designcode != "")
            {
                designcode = "and st.desig_code in(" + designcode + ")";
            }

            string catecode = "";
            for (int item = 0; item < cbl_staffc.Items.Count; item++)
            {
                if (cbl_staffc.Items[item].Selected == true)
                {
                    if (catecode == "")
                    {
                        catecode = "'" + cbl_staffc.Items[item].Value + "'";
                    }
                    else
                    {
                        catecode = catecode + ',' + "'" + cbl_staffc.Items[item].Value + "'";
                    }
                }
            }
            if (catecode != "")
            {
                catecode = " and st.category_code in(" + catecode + ")";
            }
            string type = "";
            for (int item = 0; item < cbl_stype.Items.Count; item++)
            {
                if (cbl_stype.Items[item].Selected == true)
                {
                    if (type == "")
                    {
                        type = "'" + cbl_stype.Items[item].Value + "'";
                    }
                    else
                    {
                        type = type + ',' + "'" + cbl_stype.Items[item].Value + "'";
                    }
                }
            }
            if (type != "")
            {
                type = " and st.stftype in(" + type + ")";
            }

            string strstaffquery = "select distinct sm.staff_name,st.staff_code from stafftrans st,staffmaster sm where st.staff_code=sm.staff_code and sm.settled=0 and sm.resign=0 and st.latestrec=1 " + deptcode + " " + designcode + " " + catecode + " " + type + " and college_code in('" + getcolcode + "')";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method(strstaffquery, hat, "Text");
            cbl_staffCode.DataSource = ds;
            cbl_staffCode.DataTextField = "staff_name";
            cbl_staffCode.DataValueField = "staff_code";
            cbl_staffCode.DataBind();
            for (int item = 0; item < cbl_staffCode.Items.Count; item++)
            {
                cbl_staffCode.Items[item].Selected = true;
            }

            if (cbl_staffCode.Items.Count > 0)
            {
                cb_staffCode.Checked = true;
                txt_staffCode.Text = "Staff (" + cbl_staffCode.Items.Count + ")";
            }
            else
            {
                cb_staffCode.Checked = false;
                txt_staffCode.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {

        }

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


    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_staffCode.Text != "--Select--")
            {

                DataSet ds = new DataSet();
                ds.Clear();
                string appl_id = string.Empty;
                string staff_code = string.Empty;
                if (cbl_staffCode.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staffCode.Items.Count; i++)
                    {

                        if (cbl_staffCode.Items[i].Selected == true)
                        {
                            string staffCode = Convert.ToString(cbl_staffCode.Items[i].Value);
                            string applno = d2.GetFunction("select appl_no from staffmaster where staff_code='" + staffCode + "'");
                            string applid = d2.GetFunction("select appl_id from staff_appl_master where appl_no='" + applno + "'");
                            if (appl_id == "")
                            {
                                appl_id = applid;
                            }
                            else
                            {
                                appl_id = appl_id + "','" + applid;

                            }


                        }
                    }
                    string getFromDate = Convert.ToString(txtfrom.Text);
                    string getTodate = Convert.ToString(txtto.Text);
                    string[] split_f = getFromDate.Split(new Char[] { '/' });
                    string[] split_t = getTodate.Split(new Char[] { '/' });
                    string fromdate = split_f[1] + "/" + split_f[0] + "/" + split_f[2];
                    string todate = split_t[1] + "/" + split_t[0] + "/" + split_t[2];
                    DateTime fdate = new DateTime();
                    DateTime tdate = new DateTime();
                    fdate = Convert.ToDateTime(fromdate);
                    tdate = Convert.ToDateTime(todate);
                    string getcolcode = rs.GetSelectedItemsValueAsString(cbl_clg);
                    string query = "select * from staff_CompensationLeave where holiday_date between '" + fdate + "' and '" + tdate + "' and appl_id in('" + appl_id + "') and college_code in('" + getcolcode + "') order by holiday_date";
                    ds = d2.select_method_wo_parameter(query, "text");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        divPopAlert.Visible = false;
                        lblAlertMsg.Text = "";

                        sp_div.Visible = true;
                        FpSpread.Visible = true;
                        FpSpread.Sheets[0].RowCount = 0;
                        FpSpread.Sheets[0].ColumnCount = 0;
                        FpSpread.CommandBar.Visible = false;
                        FpSpread.Sheets[0].AutoPostBack = false;
                        FpSpread.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread.Sheets[0].FrozenRowCount = 1;
                        FpSpread.Sheets[0].RowHeader.Visible = false;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "S.No";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 50;

                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Name";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 250;


                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Compensation Date";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 107;

                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Holiday Date";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;


                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Worked On Holiday";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;


                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        //  FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 200;


                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Text = "M";
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;

                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Text = "E";
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;


                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Text = "Compensation Leave";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[0, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 2);
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 200;

                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Text = "M";
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;

                        FpSpread.Sheets[0].ColumnCount++;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Text = "E";
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread.Sheets[0].ColumnHeader.Cells[1, FpSpread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread.Columns[FpSpread.Sheets[0].ColumnCount - 1].Width = 100;
                        FpSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 2);
                        int sno = 0;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            rprint.Visible = true;
                            string appl_ids = Convert.ToString(ds.Tables[0].Rows[i]["appl_id"]);
                            string appl_no = d2.GetFunction("select appl_no from staff_appl_master where appl_id='" + appl_ids + "'");
                            string staffcode = d2.GetFunction("select staff_code from staffmaster where appl_no='" + appl_no + "'");
                            string name = d2.GetFunction("select staff_name from staffmaster where appl_no='" + appl_no + "'");
                            string collegecode = d2.GetFunction("select college_code from staffmaster where appl_no='" + appl_no + "'");
                            string holidaydate = Convert.ToString(ds.Tables[0].Rows[i]["holiday_date"]);
                            string Leave_Morning = Convert.ToString(ds.Tables[0].Rows[i]["LeaveMorning"]);
                            string Leave_evng = Convert.ToString(ds.Tables[0].Rows[i]["LeaveEvening"]);
                            string compensationdate = Convert.ToString(ds.Tables[0].Rows[i]["compensation_date"]);

                            string morn_lve = string.Empty;
                            string evn_lve = string.Empty;
                            if (Leave_Morning != "0")
                            {
                                morn_lve = d2.GetFunction("select  shortname from leave_category where LeaveMasterPK='" + Leave_Morning + "' and college_code='" + collegecode + "'");

                            }
                            if (Leave_evng != "0")
                            {
                                evn_lve = d2.GetFunction("select  shortname from leave_category where LeaveMasterPK='" + Leave_evng + "' and college_code='" + collegecode + "'");

                            }

                            string monyearval = string.Empty;
                            string getdateval = string.Empty;
                            string commonyear = string.Empty;
                            string comgetdate = string.Empty;

                            string[] splitspace = holidaydate.Split(new char[] { ' ' });

                            string[] splitval = splitspace[0].Split(new Char[] { '/' });
                            string monthval = splitval[0].ToString();
                            string dateval = splitval[1].ToString();
                            string yearval = splitval[2].ToString();

                            string dateval1 = (dateval.TrimStart('0'));
                            string monthval1 = (monthval.TrimStart('0'));
                            monyearval = monthval + "/" + yearval;
                            getdateval = dateval + "/" + monthval + "/" + yearval;

                            string[] compdatesplitspace = compensationdate.Split(new char[] { ' ' });
                            string[] compdatesplitval = compdatesplitspace[0].Split(new Char[] { '/' });
                            string compmonval = compdatesplitval[0].ToString();
                            string compdateval = compdatesplitval[1].ToString();
                            string compyearval = compdatesplitval[2].ToString();

                            string compdate = (compdateval.TrimStart('0'));
                            string compmonth = (compmonval.TrimStart('0'));
                            comgetdate = compdateval + "/" + compmonval + "/" + compyearval;

                            dateval1 = "[" + dateval1 + "]";
                            string morning = string.Empty;
                            string evening = string.Empty;
                            string getpresentabs = d2.GetFunction("select " + dateval1 + " from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monyearval + "'");
                            if (getpresentabs != "" || getpresentabs != "0")
                            {
                                if (getpresentabs.Contains('-'))
                                {
                                    string[] mor_evnf = getpresentabs.Split('-');
                                    morning = Convert.ToString(mor_evnf[0]);
                                    evening = Convert.ToString(mor_evnf[1]);


                                }

                            }
                            sno++;
                            FpSpread.Sheets[0].RowCount++;
                            FpSpread.Sheets[0].Cells[i, 0].Text = Convert.ToString(sno);
                            FpSpread.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";

                            FpSpread.Sheets[0].Cells[i, 1].Text = Convert.ToString(name);
                            FpSpread.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread.Sheets[0].Cells[i, 1].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";

                            FpSpread.Sheets[0].Cells[i, 2].Text = Convert.ToString(comgetdate);
                            FpSpread.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";


                            FpSpread.Sheets[0].Cells[i, 3].Text = Convert.ToString(getdateval);
                            FpSpread.Sheets[0].Cells[i, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread.Sheets[0].Cells[i, 3].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[i, 3].Font.Name = "Book Antiqua";

                            if (morning != "")
                                FpSpread.Sheets[0].Cells[i, 4].Text = Convert.ToString(morning);
                            else
                                FpSpread.Sheets[0].Cells[i, 4].Text = "-";
                            FpSpread.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[i, 4].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[i, 4].Font.Name = "Book Antiqua";
                            if (evening != "")

                                FpSpread.Sheets[0].Cells[i, 5].Text = Convert.ToString(evening);
                            else
                                FpSpread.Sheets[0].Cells[i, 5].Text = "-";
                            FpSpread.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[i, 5].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[i, 5].Font.Name = "Book Antiqua";

                            if (morn_lve != "")
                                FpSpread.Sheets[0].Cells[i, 6].Text = Convert.ToString(morn_lve);
                            else
                                FpSpread.Sheets[0].Cells[i, 6].Text = "-";
                            FpSpread.Sheets[0].Cells[i, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[i, 6].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[i, 6].Font.Name = "Book Antiqua";

                            if (evn_lve != "")

                                FpSpread.Sheets[0].Cells[i, 7].Text = Convert.ToString(evn_lve);
                            else
                                FpSpread.Sheets[0].Cells[i, 7].Text = "-";
                            FpSpread.Sheets[0].Cells[i, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread.Sheets[0].Cells[i, 7].Font.Size = FontUnit.Medium;
                            FpSpread.Sheets[0].Cells[i, 7].Font.Name = "Book Antiqua";
                            // FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(staffCode);

                        }
                        FpSpread.SaveChanges();
                        FpSpread.Visible = true;
                        lblAlertMsg.Text = string.Empty;
                        divPopAlert.Visible = false;
                        //  btn_save.Visible = false;
                        FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                        FpSpread.SaveChanges();


                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "No Records Found";
                        // btn_save.Visible = false;

                    }
                }
            }
            else
            {
                //alert message
                //please select Atleast one staff
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Select  Atleast One Staff";
                // btn_save.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void FpSpread_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            Dictionary<string, string> diclea = new Dictionary<string, string>();
            // int countvalue1 = Convert.ToInt32(Session["item_Value"]);
            //  countvalue1 = countvalue1 + 3;
            string actrow = FpSpread.Sheets[0].ActiveRow.ToString();  //e.SheetView.ActiveRow.ToString();
            string actcol = FpSpread.Sheets[0].ActiveColumn.ToString();  //e.SheetView.ActiveColumn.ToString();
            string txtval = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(actcol)].Text);
            string last = e.CommandArgument.ToString();
            if (actrow != "0")
            {
                if (actrow == last)
                {
                    flag_true = false;
                }
                else
                {
                    flag_true = true;
                }
            }
            if (actcol == "1")
            {
                if (actrow == last)
                {
                    flag_true = false;
                }
                else
                {
                    flag_true = true;
                }
            }

            if (flag_true == false)
            {
                string setval = "select * from leave_category where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dsleavval = d2.select_method_wo_parameter(setval, "text");
                for (int li = 0; li < dsleavval.Tables[0].Rows.Count; li++)
                {
                    if (!diclea.ContainsKey(dsleavval.Tables[0].Rows[li]["shortname"].ToString().Trim().ToLower()))
                    {
                        diclea.Add(dsleavval.Tables[0].Rows[li]["shortname"].ToString().Trim().ToLower(), dsleavval.Tables[0].Rows[li]["shortname"].ToString().Trim().ToLower());
                    }
                }

            }

        }
        catch (Exception ex)
        {

        }
    }



    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;

        }

        catch (Exception ex)
        {

        }
    }

    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        string[] fromdatespilt = txtfrom.Text.ToString().Trim().Split('/');
        DateTime fromdate = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
        string[] todatespilt = txtto.Text.ToString().Trim().Split('/');
        DateTime todate = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
        if (fromdate > todate)
        {
            divPopAlert.Visible = true;
            lblAlertMsg.Text = "Please Enter From Date Less Than To Date";

            //errmsg.Text = "Please Enter From Date Less Than To Date";
            //errmsg.Visible = true;
        }
        else
        {
            divPopAlert.Visible = false;
            lblAlertMsg.Text = " ";
            //errmsg.Visible = false;
        }
    }

    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        string[] fromdatespilt = txtfrom.Text.ToString().Trim().Split('/');
        DateTime fromdate = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
        string[] todatespilt = txtto.Text.ToString().Trim().Split('/');
        DateTime todate = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
        if (fromdate > todate)
        {
            divPopAlert.Visible = true;
            lblAlertMsg.Text = "Please Enter From Date Less Than To Date";
            // errmsg.Text = "Please Enter From Date Less Than To Date";
        }
        else
        {
            divPopAlert.Visible = false;
            lblAlertMsg.Text = " ";
            //errmsg.Visible = false;
        }
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread, reportname);
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Visible = true;
                lblsmserror.Text = "Please Enter Your Report Name!";
                txtexcel.Focus();
            }
        }
        catch { }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        string reportname = "Compensation Report";
        Printcontrol.loadspreaddetails(FpSpread, "Individual_SalaryReport.aspx", reportname);
        Printcontrol.Visible = true;
    }

}