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
public partial class Incometaxcalculation_report : System.Web.UI.Page
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
            lblvalidation1.Text = "";
            if (!IsPostBack)
            {
                ViewState["CumlativeHeader"] = null;
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                binddept();
                designation();
                category();
                bindmonth();
                bindyear();
                bindallowance();
                binddeduction();
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
            bindmonth();
            bindyear();
            bindallowance();
            binddeduction();
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
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
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
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
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
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
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
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
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
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
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint.Visible = false;
            if (radFormat.SelectedIndex == 2)
            {
                #region From 16
                int sno = 0;
                string query = "";
                string deptcode = rs.GetSelectedItemsValueAsString(cbl_dept);
                string designation = rs.GetSelectedItemsValueAsString(cbl_desig);
                string stafftype = rs.GetSelectedItemsValueAsString(cbl_staffc);
                Fpspread1.Sheets[0].Visible = true;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].ColumnCount = 6;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.Black;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;

                if (cb_relived.Checked == false)
                {
                    if (txt_sname.Text != "")
                    {
                        query = "select distinct s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and  s.college_code='" + ddlcollege.SelectedItem.Value + "' and resign = 0 and settled = 0 and ISNULL(Discontinue,'0')='0' and latestrec=1 and s.staff_name='" + txt_sname.Text + "'";
                    }
                    else
                    {
                        query = "select distinct s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and h.dept_code in('" + deptcode + "')  and s.college_code='" + ddlcollege.SelectedItem.Value + "' and resign = 0 and settled = 0 and ISNULL(Discontinue,'0')='0' and latestrec=1";
                    }
                }
                if (cb_relived.Checked == true)
                {

                    DateTime frm_date = new DateTime();//delsi 2807
                    DateTime to_date = new DateTime();
                    string getfromdate = string.Empty;
                    string gettodate = string.Empty;

                    string itsetting = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='IT Calculation Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'");
                    if (itsetting.Trim() != "0")
                    {
                        string frommonth = string.Empty;
                        string tomonth = string.Empty;
                        string fromyear = string.Empty;
                        string toyear = string.Empty;
                        string[] linkvalue = itsetting.Split('-');
                        if (linkvalue.Length > 0)
                        {
                            frommonth = linkvalue[0].Split(',')[0];
                            fromyear = linkvalue[0].Split(',')[1];
                            tomonth = linkvalue[1].Split(',')[0];
                            toyear = linkvalue[1].Split(',')[1];
                            getfromdate = frommonth + "/" + "1" + "/" + fromyear;

                            frm_date = Convert.ToDateTime(getfromdate);
                            int mon = Convert.ToInt32(tomonth);
                            int year = Convert.ToInt32(toyear);
                            int daysInmonth = System.DateTime.DaysInMonth(year, mon);
                            string getday = Convert.ToString(daysInmonth);
                            gettodate = tomonth + "/" + getday + "/" + toyear;
                            to_date = Convert.ToDateTime(gettodate);


                        }
                    }
                    else
                    {
                        alertmessage.Visible = true;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please Set IT Calculation Settings";
                        return;
                    }

                    if (txt_sname.Text != "")
                    {
                        query = "select distinct resign,settled,s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and  s.college_code='" + ddlcollege.SelectedItem.Value + "'  and ISNULL(Discontinue,'0')='0' and latestrec=1 and s.staff_name='" + txt_sname.Text + "' and ((resign=0 or settled=0) or (resign=1 and relieve_date>='" + frm_date + "') or (resign=1 and relieve_date between '" + frm_date + "' and '" + to_date + "'))";
                    }
                    else
                    {
                        query = "select distinct resign,settled,s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and h.dept_code in('" + deptcode + "')  and s.college_code='" + ddlcollege.SelectedItem.Value + "' and ISNULL(Discontinue,'0')='0' and latestrec=1 and ((resign=0 or settled=0) or (resign=1 and relieve_date>='" + frm_date + "') or (resign=1 and relieve_date between '" + frm_date + "' and '" + to_date + "'))";
                    }

                }

                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "TEXT");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ermsg.Visible = false;
                    Fpspread1.Visible = true;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = 10;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Arial";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[1].Label = "Select";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Name";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[3].Label = "Staff Code";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[4].Label = "Department";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[5].Label = "Designation";
                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType cbSub = new FarPoint.Web.Spread.CheckBoxCellType();
                    cb.AutoPostBack = true;
                    cbSub.AutoPostBack = false;
                    Fpspread1.Sheets[0].Columns[0].Width = 80;
                    Fpspread1.Sheets[0].Columns[1].Width = 80;
                    Fpspread1.Sheets[0].Columns[2].Width = 300;
                    Fpspread1.Sheets[0].Columns[0].Locked = true;
                    Fpspread1.Sheets[0].Columns[1].Locked = false;
                    Fpspread1.Sheets[0].Columns[2].Locked = true;
                    Fpspread1.Sheets[0].Columns[3].Locked = true;
                    Fpspread1.Sheets[0].Columns[4].Locked = true;
                    Fpspread1.Sheets[0].Columns[5].Locked = true;
                    Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = cb;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                    {
                        sno++;
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        string name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                        string code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();
                        string Applid = ds.Tables[0].Rows[rolcount]["appl_id"].ToString();
                        Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                        Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Font.Bold = false;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = cbSub;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = name;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Applid;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = code;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Column.Width = 150;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Column.Width = 250;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Column.Width = 250;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["pangirnumber"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                        if (cb_relived.Checked == true)
                        {
                            string resign = Convert.ToString(ds.Tables[0].Rows[rolcount]["resign"]);
                            string settle = Convert.ToString(ds.Tables[0].Rows[rolcount]["settled"]);
                            if (resign == "1" || resign == "True" && settle == "1" || settle == "True")
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = Color.MistyRose;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].BackColor = Color.MistyRose;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].BackColor = Color.MistyRose;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].BackColor = Color.MistyRose;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].BackColor = Color.MistyRose;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].BackColor = Color.MistyRose;

                            }

                        }

                    }
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Width = 700;
                    Fpspread1.Height = 400;
                    btnPrint.Visible = true;
                    btnExcel16.Visible = true;
                }
                else
                {
                    Fpspread1.Visible = false;
                    ermsg.Visible = true;
                    ermsg.Text = "No Records Found";
                    btnPrint.Visible = false;
                    btnExcel16.Visible = false;
                }
                #endregion
            }
            else
            {
                #region Format I & Fromat II
                int sno = 0;
                string query = "";
                string deptcode = rs.GetSelectedItemsValueAsString(cbl_dept);
                string designation = rs.GetSelectedItemsValueAsString(cbl_desig);
                string stafftype = rs.GetSelectedItemsValueAsString(cbl_staffc);
                Fpspread1.Sheets[0].Visible = true;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].ColumnCount = 5;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.Black;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                if (cb_relived.Checked == false)
                {

                    if (txt_sname.Text != "")
                    {
                        query = "select distinct s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber,sex from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and  s.college_code='" + ddlcollege.SelectedItem.Value + "' and resign = 0 and settled = 0 and ISNULL(Discontinue,'0')='0' and latestrec=1 and s.staff_name='" + txt_sname.Text + "'";
                    }
                    else// added staff categorizer  and sc.category_code=st.category_code and sc.college_code=sm.college_code 
                    {
                        query = "select distinct s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber,sex from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm,staffcategorizer sc  where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no  and sc.category_code=st.category_code and sc.college_code=sm.college_code  and h.dept_code in('" + deptcode + "')  and s.college_code='" + ddlcollege.SelectedItem.Value + "' and resign = 0 and settled = 0 and ISNULL(Discontinue,'0')='0' and latestrec=1";

                        if (stafftype != "")
                        {
                            query = query + " and sc.category_code in('" + stafftype + "')";

                        }
                        if (designation != "")
                        {
                            query = query + " and d.desig_code in('" + designation + "')";

                        }
                    }



                }

                if (cb_relived.Checked == true)
                {
                    DateTime frm_date = new DateTime();
                    DateTime to_date = new DateTime();
                    string getfromdate = string.Empty;
                    string gettodate = string.Empty;

                    string itsetting = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='IT Calculation Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'");
                    if (itsetting.Trim() != "0")
                    {
                        string frommonth = string.Empty;
                        string tomonth = string.Empty;
                        string fromyear = string.Empty;
                        string toyear = string.Empty;
                        string[] linkvalue = itsetting.Split('-');
                        if (linkvalue.Length > 0)
                        {
                            frommonth = linkvalue[0].Split(',')[0];
                            fromyear = linkvalue[0].Split(',')[1];
                            tomonth = linkvalue[1].Split(',')[0];
                            toyear = linkvalue[1].Split(',')[1];
                            getfromdate = frommonth + "/" + "1" + "/" + fromyear;

                            frm_date = Convert.ToDateTime(getfromdate);
                            int mon = Convert.ToInt32(tomonth);
                            int year = Convert.ToInt32(toyear);
                            int daysInmonth = System.DateTime.DaysInMonth(year, mon);
                            string getday = Convert.ToString(daysInmonth);
                            gettodate = tomonth + "/" + getday + "/" + toyear;
                            to_date = Convert.ToDateTime(gettodate);


                        }
                    }
                    else
                    {
                        alertmessage.Visible = true;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please Set IT Calculation Settings";
                        return;
                    }

                    if (txt_sname.Text != "")
                    {
                        query = "select distinct resign,settled,s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber,sex from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and  s.college_code='" + ddlcollege.SelectedItem.Value + "' and ISNULL(Discontinue,'0')='0' and latestrec=1 and s.staff_name='" + txt_sname.Text + "' and latestrec=1 and ((resign=0 or settled=0) or (resign=1 and relieve_date>='" + frm_date + "') or (resign=1 and relieve_date between '" + frm_date + "' and '" + to_date + "'))";
                    }
                    else
                    {
                        query = "select distinct resign,settled,s.staff_code,s.staff_name,appl_id,h.dept_name,d.desig_name,pangirnumber,sex from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm,staffcategorizer sc where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no  and sc.category_code=st.category_code and sc.college_code=sm.college_code and h.dept_code in('" + deptcode + "')  and s.college_code='" + ddlcollege.SelectedItem.Value + "' and ISNULL(Discontinue,'0')='0' and latestrec=1 and ((resign=0 or settled=0) or (resign=1 and relieve_date>='" + frm_date + "') or (resign=1 and relieve_date between '" + frm_date + "' and '" + to_date + "'))";


                        if (stafftype != "")
                        {
                            query = query + " and sc.category_code in('" + stafftype + "')";

                        }
                        if (designation != "")
                        {
                            query = query + " and d.desig_code in('" + designation + "')";

                        }
                    }


                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "TEXT");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ermsg.Visible = false;
                    Fpspread1.Visible = true;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = 10;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Arial";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Name";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Code";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[3].Label = "Department";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[4].Label = "Designation";
                    Fpspread1.Sheets[0].Columns[0].Width = 80;
                    Fpspread1.Sheets[0].Columns[1].Width = 300;
                    Fpspread1.Sheets[0].Columns[2].Width = 100;
                    Fpspread1.Sheets[0].Columns[0].Locked = true;
                    Fpspread1.Sheets[0].Columns[1].Locked = true;
                    Fpspread1.Sheets[0].Columns[2].Locked = true;
                    for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                    {
                        sno++;
                        string name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                        string code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                        Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].Font.Bold = false;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = name;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = code;
                       
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["appl_id"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["sex"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Column.Width = 150;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Column.Width = 250;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Column.Width = 250;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["pangirnumber"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        if (cb_relived.Checked == true)
                        {
                            string resign = Convert.ToString(ds.Tables[0].Rows[rolcount]["resign"]);
                            string settle = Convert.ToString(ds.Tables[0].Rows[rolcount]["settled"]);
                            if (resign == "1" || resign == "True" && settle == "1" || settle == "True")
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = Color.MistyRose;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].BackColor = Color.MistyRose;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].BackColor = Color.MistyRose;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].BackColor = Color.MistyRose;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].BackColor = Color.MistyRose;

                            }

                        }

                    }
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Width = 700;
                    Fpspread1.Height = 400;
                }
                else
                {
                    Fpspread1.Visible = false;
                    ermsg.Visible = true;
                    ermsg.Text = "No Records Found";
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void btn_popgo_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["CumlativeHeader"] = null;
            Printcontrol.Visible = false;
            filters_tbl.Visible = true;
            individualcumlative_table.Visible = true;
            double PayLastMonthAllowance = 0;
            double PayLastMonthDeduction = 0;
            int DiffenerceMonth = 0;
            double PayLastMonthSalary = 0;
            string IncomeSalary = string.Empty;
            double lastMonthSalary = 0;
            int lastpayMonth = 0;
            int lastpayYear = 0;
            double lastmonsalary;
            Hashtable PayLastMonthAllowanceHash = new Hashtable();
            Hashtable PayLastMonthDeductionHash = new Hashtable();
            bool CalculateAllMonthBool = false;
            int rowcount = 0;
            Fpspread3.Sheets[0].RowCount = 0;
            Fpspread3.Sheets[0].ColumnCount = 0;
            Fpspread3.CommandBar.Visible = false;
            Fpspread3.Sheets[0].AutoPostBack = true;
            Fpspread3.Sheets[0].ColumnHeader.RowCount = 2;
            Fpspread3.Sheets[0].RowHeader.Visible = false;
            Fpspread3.Sheets[0].ColumnCount = 4;
            int month = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Dictionary<string, int> dicheadercount = new Dictionary<string, int>();
            Dictionary<string, string> dictotal = new Dictionary<string, string>();
            string clgname = d2.GetFunction("select cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code and cp.college_code='" + collegecode1 + "'");
            lbl_collegename.Text = clgname;
            lbl_incomeheader.Text = "INCOME PARTICULARS FOR THE YEAR " + Convert.ToString(ddl_fromyear.SelectedItem.Text) + " - " + Convert.ToString(ddl_toyear.SelectedItem.Text);
            int sno = 0;
            int rowheader = 1;
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 0].Text = "S.No";
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 0].Font.Name = "Arial";
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 0].Font.Size = 12;
            //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fpspread3.Columns[0].Width = 50;
            Fpspread3.Columns[0].Locked = true;
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 1].Text = "IT Month";
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 1].Font.Name = "Arial";
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 1].Font.Size = 12;
            //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fpspread3.Columns[1].Width = 100;
            Fpspread3.Columns[1].Locked = true;
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 2].Text = "BPAY";
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 2].Font.Name = "Arial";
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 2].Font.Size = 12;
            //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fpspread3.Columns[2].Width = 100;
            Fpspread3.Columns[2].Locked = true;
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 3].Text = "GDPAY";
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 3].Font.Name = "Arial";
            Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, 3].Font.Size = 12;
            //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fpspread3.Columns[3].Width = 100;
            Fpspread3.Columns[3].Locked = true;
            string selectedtomonth = ddl_tomonth.SelectedItem.Text.ToString();
            string frommonth = ""; string fromyear = ""; string tomonth = ""; string toyear = ""; string staffcode = "";
            string allowance = ""; string deduction = ""; string ApplId = "";
            string itsetting = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='IT Calculation Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'");
            if (itsetting.Trim() != "0")
            {
                string[] linkvalue = itsetting.Split('-');
                if (linkvalue.Length > 0)
                {
                    frommonth = linkvalue[0].Split(',')[0];
                    fromyear = linkvalue[0].Split(',')[1];
                    tomonth = linkvalue[1].Split(',')[0];
                    toyear = linkvalue[1].Split(',')[1];
                    activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                    activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                    if (Convert.ToInt32(activerow) != -1 && Convert.ToInt32(activerow) != -1)
                    {
                        staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                        ApplId = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                    }
                    else
                        return;
                }
                else
                {
                    alertmessage.Visible = true;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Please Set IT Calculation Settings";
                    return;
                }
                #region allowances and deductions
                if (!dicheadercount.ContainsKey("BPAY"))
                {
                    dicheadercount.Add("BPAY", 2);
                }
                if (!dicheadercount.ContainsKey("GDPAY"))
                {
                    dicheadercount.Add("GDPAY", 3);
                }
                if (cbl_allowancemultiple.Items.Count > 0)
                {
                    for (i = 0; i < cbl_allowancemultiple.Items.Count; i++)
                    {
                        if (cbl_allowancemultiple.Items[i].Selected == true)
                        {
                            string alltype = Convert.ToString(cbl_allowancemultiple.Items[i].Text);
                            string alltypeValue = Convert.ToString(cbl_allowancemultiple.Items[i].Value);//13.11.17
                            if (!dicheadercount.ContainsKey(alltypeValue))
                            {
                                Fpspread3.Sheets[0].ColumnCount++;
                                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Text = alltype;
                                //Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Font.Name = "Arial";
                                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Font.Size = 12;
                                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                dicheadercount.Add(alltypeValue, Fpspread3.Sheets[0].ColumnCount - 1);
                            }
                        }
                    }
                }
                Fpspread3.Sheets[0].ColumnCount++;
                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Text = "GROSS";
                //Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Font.Bold = true;
                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Font.Name = "Arial";
                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Font.Size = 12;
                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                dicheadercount.Add("GROSS", Fpspread3.Sheets[0].ColumnCount - 1);
                if (cbl_deduction.Items.Count > 0)
                {
                    for (i = 0; i < cbl_deduction.Items.Count; i++)
                    {
                        if (cbl_deduction.Items[i].Selected == true)
                        {
                            string alltype = Convert.ToString(cbl_deduction.Items[i].Text);
                            string alltypeValue = Convert.ToString(cbl_deduction.Items[i].Value);//13.11.17
                            if (!dicheadercount.ContainsKey(alltypeValue))
                            {
                                Fpspread3.Sheets[0].ColumnCount++;
                                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Text = alltype;
                                // Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Font.Name = "Arial";
                                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].Font.Size = 12;
                                Fpspread3.Sheets[0].ColumnHeader.Cells[rowheader, Fpspread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                dicheadercount.Add(alltypeValue, Fpspread3.Sheets[0].ColumnCount - 1);
                            }
                        }
                    }
                }

                double frmyear = 0;
                double toyr = 0;
                double.TryParse(fromyear, out frmyear);
                double.TryParse(toyear, out toyr);
                frmyear = frmyear + 1; toyr = toyr + 1;

                string HeaderName = Convert.ToString(lbl_incomeheader.Text.Trim() + "@NAME :" + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text).Trim() + "@DEPT  : " + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text) + " - " + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text));// + " " + " ASSESSMENT YEAR " + frmyear + " - " + toyr
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "ASSESSMENT YEAR " + frmyear + " - " + toyr;//Convert.ToString(lbl_incomeheader.Text);
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = 12;
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Arial";
                Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, Fpspread3.Sheets[0].ColumnHeader.Columns.Count);

                //05.02.18
                //string designDeptName = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text) + " DEPT : " + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text) + " - " + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                HeaderName = HeaderName.Replace('&', ' ');
                HeaderName = HeaderName.Replace('$', ' ');

                ViewState["CumlativeHeader"] = HeaderName;

                //Fpspread3.Sheets[0].ColumnHeader.Cells[1, 0].Text = designDeptName;
                //Fpspread3.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread3.Sheets[0].ColumnHeader.Cells[1, 0].Font.Bold = true;
                //Fpspread3.Sheets[0].ColumnHeader.Cells[1, 0].Font.Size = 12;
                //Fpspread3.Sheets[0].ColumnHeader.Cells[1, 0].Font.Name = "Arial";
                //Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, Fpspread3.Sheets[0].ColumnHeader.Columns.Count);

                //Fpspread3.Sheets[0].ColumnHeader.Cells[2, 0].Text = "ASSESSMENT YEAR " + frmyear + " - " + toyr;
                //Fpspread3.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread3.Sheets[0].ColumnHeader.Cells[2, 0].Font.Bold = true;
                //Fpspread3.Sheets[0].ColumnHeader.Cells[2, 0].Font.Size = 12;
                //Fpspread3.Sheets[0].ColumnHeader.Cells[2, 0].Font.Name = "Arial";
                //Fpspread3.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, Fpspread3.Sheets[0].ColumnHeader.Columns.Count);


                #endregion
                string CalculateAllSet = d2.GetFunction("select linkValue from New_InsSettings where LinkName='Form16 Calculate All Month'  and user_code ='" + usercode + "' and college_code ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'");//barath 25.09.17
                if (!string.IsNullOrEmpty(CalculateAllSet) && CalculateAllSet.Trim() != "0")
                {
                    CalculateAllMonthBool = true;
                    string CalculateMonthDetQuery = "select paymonth,payyear,netaddact,netadd,addd,deddd,convert(varchar(max), allowances)as allowances,convert(varchar(max),deductions)as deductions,bsalary from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + staffcode + "' group by payyear,paymonth,netaddact,netadd,addd,deddd,convert(varchar(max), allowances),convert(varchar(max),deductions),bsalary order by year(payyear),year(paymonth) ";
                    //((PayMonth >= '" + frommonth + "' and PayYear = '" + fromyear + "') or (PayMonth <='" + tomonth + "' and PayYear = '" + toyear + "' )) 
                    CalculateMonthDetQuery += " select Amount,itmonth,ityear from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' ))  and CollegeCode='" + ddlcollege.SelectedValue + "' and staff_ApplID='" + ApplId + "' group by ityear,itmonth,Amount order by year(ityear),year(itmonth) ";
                    DataSet CalculateMonthDetDS = d2.select_method_wo_parameter(CalculateMonthDetQuery, "text");
                    if (CalculateMonthDetDS.Tables != null && CalculateMonthDetDS.Tables[0].Rows.Count > 0)
                    {
                        #region payProcessLastMonthSalary
                        double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["netaddact"]), out lastMonthSalary);
                        int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["paymonth"]), out lastpayMonth);
                        int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["payyear"]), out lastpayYear);
                        double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["addd"]), out PayLastMonthAllowance);
                        double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["deddd"]), out PayLastMonthDeduction);
                        PayLastMonthAllowanceHash = PayProcessAllowanceDet(CalculateMonthDetDS, 0, CalculateMonthDetDS.Tables[0].Rows.Count - 1, ref PayLastMonthSalary);
                        PayLastMonthDeductionHash = PayProcessDeductionDet(CalculateMonthDetDS, 0, CalculateMonthDetDS.Tables[0].Rows.Count - 1, ref PayLastMonthSalary);
                        DateTime FYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                        DateTime TYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                        DiffenerceMonth = (TYearDT.Month - FYearDT.Month) + 12 * (TYearDT.Year - FYearDT.Year);
                        double CurrentSalary = 0;
                        double.TryParse(IncomeSalary, out CurrentSalary);
                        CurrentSalary += lastMonthSalary * DiffenerceMonth;
                        IncomeSalary = Convert.ToString(CurrentSalary);
                        lastmonsalary = PayLastMonthSalary * DiffenerceMonth;
                        //PayLastMonthSalary *= DiffenerceMonth;
                        #endregion
                    }
                }
                //barath 14.11.2017
                string professionalTaxSettings = string.Empty;
                string ptstmonth = string.Empty;
                string ptendmonth = string.Empty;
                string ptstyear = string.Empty;
                string ptendyear = string.Empty;
                string pt = string.Empty;
                string slab = string.Empty;
                q1 = "select paymonth,payyear,netaddact,netadd,grade_pay,convert(varchar(max), allowances)as allowances,convert(varchar(max),deductions)as deductions,bsalary from monthlypay where  CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + staffcode + "' group by payyear,paymonth,netaddact,netadd,grade_pay,convert(varchar(max), allowances),convert(varchar(max),deductions),bsalary order by year(payyear),year(paymonth) ";
                q1 += " select LinkValue from New_InsSettings where LinkName='Professional Tax Calculation Month' and college_code ='" + ddlcollege.SelectedItem.Value + "' and user_code ='" + usercode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables != null)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        professionalTaxSettings = Convert.ToString(ds.Tables[1].Rows[0]["LinkValue"]);
                        if (!string.IsNullOrEmpty(professionalTaxSettings.Trim()))
                        {
                            string[] det = professionalTaxSettings.Split(';');
                            string monthyear = det[0];
                            string endmonthyear = det[1];
                            ptstmonth = monthyear.Split('-')[0];
                            ptendmonth = endmonthyear.Split('-')[0];
                            ptstyear = monthyear.Split('-')[1];
                            ptendyear = endmonthyear.Split('-')[1];
                        }
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread3.Sheets[0].RowCount++;
                        sno++;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                        string getmon = getmonth(Convert.ToInt32(ds.Tables[0].Rows[i]["PayMonth"]));
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(getmon + "-" + Convert.ToString(ds.Tables[0].Rows[i]["PayYear"]));
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                        month = i;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["bsalary"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["grade_pay"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                        #region Check Allowance and deduction
                        allowance = ""; deduction = "";
                        allowance = Convert.ToString(ds.Tables[0].Rows[i]["allowances"]);
                        deduction = Convert.ToString(ds.Tables[0].Rows[i]["deductions"]);
                        #region allowance
                        string[] allowanmce_arr1;
                        string alowancesplit;
                        allowanmce_arr1 = allowance.Split('\\');
                        double RoundValue = 0;
                        for (int j = 0; j < allowanmce_arr1.Length; j++)
                        {
                            alowancesplit = allowanmce_arr1[j];
                            if (alowancesplit.Trim() != "")
                            {
                                string[] allowanceda;
                                allowanceda = alowancesplit.Split(';');
                                if (allowanceda[2].Trim() != "")
                                {
                                    string alltype = Convert.ToString(allowanceda[0]);
                                    string allFormat = Convert.ToString(allowanceda[1]);
                                    string allAmount = "";
                                    if (dicheadercount.ContainsKey(alltype))
                                    {
                                        int headercolcount = Convert.ToInt32(dicheadercount[alltype]);
                                        if (headercolcount != 0)
                                        {
                                            string[] spval = allowanceda[2].Split('-');
                                            if (spval.Length == 2)
                                            {
                                                if (allFormat.Trim().ToUpper() == "PERCENT")
                                                    allAmount = spval[1];
                                                else
                                                    allAmount = spval[0];
                                            }
                                            else
                                            {
                                                if (allowanceda.Length > 3)
                                                    allAmount = Convert.ToString(allowanceda[3]);
                                            }
                                            RoundValue = 0;
                                            double.TryParse(allAmount, out RoundValue);//20.01.18 barath
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Text = Convert.ToString(Math.Round(RoundValue, 0));
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Font.Size = 12;
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Font.Name = "Arial";
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        int headerco = Convert.ToInt32(dicheadercount["GROSS"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headerco].Text = Convert.ToString(ds.Tables[0].Rows[i]["netadd"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headerco].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headerco].Font.Name = "Arial";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headerco].Font.Size = 12;
                        #region duduction
                        string[] allowanmce_arr2;
                        string alowancesplit2;
                        allowanmce_arr2 = deduction.Split('\\');
                        for (int l = 0; l < allowanmce_arr2.Length; l++)
                        {
                            alowancesplit2 = allowanmce_arr2[l];
                            if (alowancesplit2.Trim() != "")
                            {
                                string[] allowanceda;
                                allowanceda = alowancesplit2.Split(';');
                                if (allowanceda[2].Trim() != "")
                                {
                                    string dedtype = Convert.ToString(allowanceda[0]).Replace("\r\n", "");
                                    string dedFormat = Convert.ToString(allowanceda[1]);
                                    string dedAmount = "";
                                    if (dicheadercount.ContainsKey(dedtype))
                                    {
                                        int headercolcount = Convert.ToInt32(dicheadercount[dedtype]);
                                        if (headercolcount != 0)
                                        {
                                            string[] spval = allowanceda[2].Split('-');
                                            if (spval.Length == 2)
                                            {
                                                if (dedFormat.Trim().ToUpper() == "PERCENT")
                                                    dedAmount = spval[1];
                                                else
                                                    dedAmount = spval[0];
                                            }
                                            else
                                            {
                                                if (allowanceda.Length > 3)
                                                    dedAmount = Convert.ToString(allowanceda[3]);
                                            }
                                            RoundValue = 0;
                                            double.TryParse(dedAmount, out RoundValue);//20.01.18 barath
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Text = Convert.ToString(Math.Round(RoundValue, 0));
                                            //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Text = Convert.ToString(dedAmount);
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Font.Size = 12;
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Font.Name = "Arial";
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].HorizontalAlign = HorizontalAlign.Right;
                                            rowcount = ds.Tables[0].Rows.Count;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        #endregion
                        if (dicheadercount.Count > 0)
                        {
                            for (int m = 0; m < dicheadercount.Count + 2; m++)
                            {
                                double Amt = 0;
                                if (dicheadercount.ContainsValue(m))
                                {
                                    if (dictotal.ContainsKey(Convert.ToString(m)))
                                    {
                                        string prewamt = Convert.ToString(dictotal[Convert.ToString(m)]);
                                        string total = Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, m].Text);
                                        if (total == "")
                                            total = "0";
                                        if (prewamt == "")
                                            prewamt = "0";
                                        Amt = Convert.ToDouble(prewamt) + Convert.ToDouble(total);
                                        dictotal.Remove(Convert.ToString(m));
                                        dictotal.Add(Convert.ToString(m), Convert.ToString(Amt));
                                    }
                                    else
                                    {
                                        string total = Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, m].Text);
                                        if (total == "")
                                        {
                                            total = "0";
                                        }
                                        dictotal.Add(Convert.ToString(m), total);
                                    }
                                }
                            }
                        }
                    }
                    int Diffmonth = 0;
                    if (lastpayYear != 0 && lastpayMonth != 0)
                    {
                        DateTime FCalYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                        DateTime TCalYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                        Diffmonth = (TCalYearDT.Month - FCalYearDT.Month) + 12 * (TCalYearDT.Year - FCalYearDT.Year);
                        DateTime DummyDT = new DateTime();
                        DummyDT = FCalYearDT;
                        DummyDT = DummyDT.AddMonths(1);
                        TCalYearDT = TCalYearDT.AddMonths(1);
                        if (Diffmonth != 0)
                        {
                            int row = 0;
                            while (DummyDT < TCalYearDT)
                            {
                                row++;
                                lastpayMonth++;
                                Fpspread3.Sheets[0].RowCount++;
                                sno++;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(DummyDT.ToString("MMMM") + "-" + DummyDT.ToString("yyyy"));
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                                //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(PayLastMonthSalary);
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                                //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                                for (int a = 2; a < dicheadercount.Count + 2; a++)
                                {
                                    int lastgrpay = 0;
                                    //int lastgrpay = Convert.ToInt32(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 2, a].Value);
                                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, a].Text = Convert.ToString(lastgrpay);
                                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, a].HorizontalAlign = HorizontalAlign.Right;
                                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, a].Font.Size = 12;
                                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, a].Font.Name = "Arial";
                                    double Amt = 0;
                                    string lastrowtext = string.Empty;
                                    string lastrow1 = string.Empty;
                                    if (dicheadercount.ContainsValue(a))
                                    {
                                        var deductionName = dicheadercount.FirstOrDefault(x => x.Value == a).Key;
                                        deductionName = deductionName.ToUpper().Trim();
                                        if (deductionName.Trim().ToUpper() == "P.TAX" || deductionName.Trim().ToUpper() == "P TAX" || deductionName.Trim().ToUpper() == "PROFESSIONAL TAX" || deductionName.Trim().ToUpper() == "PROFTAX")
                                        {
                                            if (DummyDT.ToString("MM").TrimStart('0') == ptstmonth && DummyDT.ToString("yyyy") == ptstyear || DummyDT.ToString("MM").TrimStart('0') == ptendmonth && DummyDT.ToString("yyyy") == ptendyear)
                                            {
                                                if (dictotal.ContainsKey(Convert.ToString(a)))
                                                {
                                                    Double lastrow = 0;
                                                    double prewamt = 0;
                                                    //double.TryParse(Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 2, a].Value), out lastrow);
                                                    double.TryParse(Convert.ToString(dictotal[Convert.ToString(a)]), out lastrow);
                                                    double.TryParse(Convert.ToString(dictotal[Convert.ToString(a)]), out prewamt);
                                                    if (dictotal.ContainsKey(Convert.ToString(a)))
                                                    {
                                                        prewamt += lastrow;
                                                        dictotal.Remove(Convert.ToString(a));
                                                        dictotal.Add(Convert.ToString(a), Convert.ToString(prewamt));
                                                    }
                                                    int.TryParse(Convert.ToString(lastrow), out lastgrpay);
                                                }
                                            }
                                        }
                                        else if (deductionName == "INC TAX" || deductionName == "I TAX" || deductionName == "INCOME TAX" || deductionName == "ITAX" || deductionName == "TDS" || deductionName=="MISC")//delsi
                                        { }
                                        else
                                        {
                                            if (dictotal.ContainsKey(Convert.ToString(a)))
                                            {
                                                Double lastrow = 0;
                                                double prewamt = 0;
                                                double.TryParse(Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 2, a].Value), out lastrow);
                                                double.TryParse(Convert.ToString(dictotal[Convert.ToString(a)]), out prewamt);
                                                if (dictotal.ContainsKey(Convert.ToString(a)))
                                                {
                                                    prewamt += lastrow;
                                                    dictotal.Remove(Convert.ToString(a));
                                                    dictotal.Add(Convert.ToString(a), Convert.ToString(prewamt));
                                                }
                                                int.TryParse(Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 2, a].Value), out lastgrpay);
                                                lastrowtext = Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 2, row - 1].Text);
                                                lastrow1 = Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 2, row - 1].Value);
                                            }
                                        }
                                    }
                                    //int lastgrpay = Convert.ToInt32(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 2, a].Value);
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, a].Text = Convert.ToString(lastgrpay);
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, a].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, a].Font.Size = 12;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, a].Font.Name = "Arial";
                                    //string lastrowtext = Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 2, row - 1].Text);
                                    //string lastrow1 = Convert.ToString(Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 2, row - 1].Value);
                                    if (dicheadercount.ContainsKey(lastrowtext))
                                    {
                                        int headercolcount = Convert.ToInt32(dicheadercount[lastrowtext]);
                                        if (headercolcount != 0)
                                        {
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Text = Convert.ToString(lastrow1);
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Font.Size = 12;
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].Font.Name = "Arial";
                                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolcount].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                                DummyDT = DummyDT.AddMonths(1);
                            }
                        }
                    }
                    Fpspread3.Sheets[0].RowCount++;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Text = "Total";
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].ForeColor = Color.MediumSlateBlue;
                    if (dictotal.Count > 0)
                    {
                        for (int p = 0; p < dictotal.Count + 2; p++)
                        {
                            if (dictotal.ContainsKey(Convert.ToString(p)))
                            {
                                string total = Convert.ToString(dictotal[Convert.ToString(p)]);
                                if (total != "0")
                                {
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, p].Text = total;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, p].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, p].Font.Size = 12;
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, p].Font.Name = "Arial";
                                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, p].ForeColor = Color.Peru;

                                    // Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, p].Font.Bold = true;
                                }
                                else
                                {
                                    Fpspread3.Sheets[0].Columns[p].Visible = false;//delsi 2409

                                }
                            }
                        }
                    }
                    // poo 04.11.17 for allowance && 04.12.17 for  deduction
                    # region Additional Allowance
                    string AllowQuery = "select m.MasterValue,a.AllowanceAmt,a.AllowanceDeductAmt from AdditionalAllowanceAndDeduction a,CO_MasterValues m where m.MasterCode=AllowanceCode and m.MasterCriteria='additionalallowance' and a.CollegeCode=m.CollegeCode and a.CollegeCode='" + ddlcollege.SelectedItem.Value + "' and a.staffcode='" + staffcode + "'";
                    DataSet allowds = new DataSet();
                    allowds = d2.select_method_wo_parameter(AllowQuery, "Text");
                    int headercolumn = Convert.ToInt32(dicheadercount["GROSS"]);
                    double additionalallow = 0; double addtotal = 0; double pretot = 0; double finaltotal = 0; double finalded = 0;
                    double prevdedtot = 0; double dedtot = 0; int colname = 0; double dedamount = 0;
                    int rowcount3 = Convert.ToInt32(Fpspread3.Sheets[0].RowCount);
                    Hashtable dedhash = new Hashtable();
                    //Dictionary<int, string> dicded = new Dictionary<int, string>();
                    string prevtotal = Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Text;
                    if (allowds.Tables.Count > 0 && allowds.Tables[0].Rows.Count > 0)
                    {
                        for (int allow = 0; allow < allowds.Tables[0].Rows.Count; allow++)
                        {
                            Fpspread3.Sheets[0].RowCount++;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(allowds.Tables[0].Rows[allow]["MasterValue"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                            Fpspread3.Sheets[0].SpanModel.Add(Fpspread3.Sheets[0].RowCount - 1, 0, 1, headercolumn);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Text = Convert.ToString(allowds.Tables[0].Rows[allow]["AllowanceAmt"]);
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Font.Size = 12;
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Font.Name = "Arial";
                            Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].HorizontalAlign = HorizontalAlign.Right;
                            double.TryParse(Convert.ToString(allowds.Tables[0].Rows[allow]["AllowanceAmt"]), out additionalallow);
                            addtotal += additionalallow;
                            string allowdeduc = allowds.Tables[0].Rows[allow]["AllowanceDeductAmt"].ToString();
                            //if (allowdeduc.Contains(";"))
                            //{
                            string[] splitdeduc = allowdeduc.Split(';');
                            for (int spl = 0; spl < splitdeduc.Length; spl++)
                            {
                                if (splitdeduc[spl].Contains('-'))
                                {
                                    string spvalue = allowdeduc.Split(';')[spl].Split('-')[0];
                                    string spamount = allowdeduc.Split(';')[spl].Split('-')[1];
                                    if (dicheadercount.ContainsKey(spvalue))
                                    {
                                        colname = dicheadercount[spvalue];
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, colname].Text = spamount;
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, colname].Font.Size = 12;
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, colname].Font.Name = "Arial";
                                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, colname].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread3.Sheets[0].Columns[colname].Visible = true;
                                        double.TryParse(spamount, out dedamount);
                                        if (dedhash.ContainsKey(colname))
                                        {
                                            dedhash[colname] = dedhash[colname] + "-" + Convert.ToString(dedamount);
                                            dedtot += dedamount;
                                        }
                                        else
                                        {
                                            dedhash.Add(colname, dedamount);
                                            dedtot = dedamount;
                                        }
                                    }
                                }
                            }
                            //}
                        }


                        //delsi2409

                    }
                    Double.TryParse(prevtotal, out pretot);
                    DataSet chkotherallow = new DataSet();
                    string qur = " select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,checkotherallow from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType ='4' and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplId + "' and CollegeCode='" + ddlcollege.SelectedValue + "'  group by AllowdeductID,ITAllowDeductType,checkotherallow";

                    chkotherallow = d2.select_method_wo_parameter(qur, "text");
                    double totalotherallow = 0;
                    if (chkotherallow.Tables[0].Rows.Count > 0)//delsi2409
                    {

                        for (int val = 0; val < chkotherallow.Tables[0].Rows.Count; val++)
                        {
                            double getval = 0;
                            string DirectValue2 = Convert.ToString(chkotherallow.Tables[0].Rows[val]["TotalAmount"]);
                            double.TryParse(DirectValue2, out getval);
                            totalotherallow = totalotherallow + getval;

                        }

                    }

                    if (totalotherallow != 0)
                    {
                        Fpspread3.Sheets[0].RowCount++;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = "Other Allowance";
                        Fpspread3.Sheets[0].SpanModel.Add(Fpspread3.Sheets[0].RowCount - 1, 0, 1, headercolumn); Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                        //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;

                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Text = Convert.ToString(totalotherallow);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Font.Size = 12;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Font.Name = "Arial";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].HorizontalAlign = HorizontalAlign.Right;
                    }

                    //finaltotal = pretot + addtotal;
                    finaltotal = pretot + addtotal + totalotherallow;
                    Fpspread3.Sheets[0].RowCount++;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = "Total";
                    Fpspread3.Sheets[0].SpanModel.Add(Fpspread3.Sheets[0].RowCount - 1, 0, 1, headercolumn); Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].ForeColor = Color.MediumSlateBlue;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Text = Convert.ToString(finaltotal);
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Font.Size = 12;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].Font.Name = "Arial";
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, headercolumn].HorizontalAlign = HorizontalAlign.Right;
                    foreach (DictionaryEntry dr in dedhash)
                    {
                        int DeductionName = Convert.ToInt32(dr.Key);
                        double DeductionAmt = 0;
                        //double.TryParse(Convert.ToString(dr.Value), out DeductionAmt);
                        double.TryParse((dictotal[Convert.ToString(DeductionName)]), out prevdedtot);
                        //prevdedtot = Convert.ToDouble(dictotal[Convert.ToString(DeductionName)]);
                        //finalded = DeductionAmt + prevdedtot; 
                        double finaldeduc = 0;
                        for (int row = 13; row < Fpspread3.Sheets[0].RowCount; row++)
                        {
                            double.TryParse(Fpspread3.Sheets[0].Cells[row, DeductionName].Text, out DeductionAmt);
                            finaldeduc += DeductionAmt;
                        }
                        double totded = 0; totded = prevdedtot + finaldeduc;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, DeductionName].Text = Convert.ToString(totded);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, DeductionName].Font.Size = 12;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, DeductionName].Font.Name = "Arial";
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, DeductionName].HorizontalAlign = HorizontalAlign.Right;
                    }
                    # endregion
                    Fpspread3.SaveChanges();
                    Fpspread3.Height = 500;
                    Fpspread3.Width = 900;
                    Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                    lbl_error.Visible = false;
                    individualdiv.Visible = true;
                    rptprint.Visible = true;
                    btnprintmaster.Visible = true;
                }
                else
                {
                    individualdiv.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Founds";
                    rptprint.Visible = false;
                    btnprintmaster.Visible = false;
                    individualcumlative_table.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void btn_cumulativetax_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            individualcumlative_table.Visible = false;
            lbl_error.Visible = false;
            Fpspread2.Visible = false;
            filters_tbl.Visible = true;
            individualdiv.Visible = false;
            rptprint.Visible = false;
            spread2div.Visible = false;
            btnprintcell.Visible = false;
            string itsetting = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='IT Calculation Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'");
            if (itsetting.Trim() != "0")
            {
                string[] linkvalue = itsetting.Split('-');
                if (linkvalue.Length > 0)
                {
                    string frommonth = linkvalue[0].Split(',')[0];
                    string fromyear = linkvalue[0].Split(',')[1];
                    string tomonth = linkvalue[1].Split(',')[0];
                    string toyear = linkvalue[1].Split(',')[1];
                    ddl_frommonth.SelectedIndex = ddl_frommonth.Items.IndexOf(ddl_frommonth.Items.FindByValue(frommonth));
                    ddl_tomonth.SelectedIndex = ddl_tomonth.Items.IndexOf(ddl_tomonth.Items.FindByValue(tomonth));
                    ddl_fromyear.SelectedIndex = ddl_fromyear.Items.IndexOf(ddl_fromyear.Items.FindByValue(fromyear));
                    ddl_toyear.SelectedIndex = ddl_toyear.Items.IndexOf(ddl_toyear.Items.FindByValue(toyear));
                }
            }
            else
            {
                alertmessage.Visible = true;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Set IncomeTax Calculation Settings";
            }
            btn_popgo_Click(sender, e);
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void imagebtns_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void bindyear()
    {
        int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy")) + 1;
        for (int l = 0; l < 15; l++)
        {
            ddl_toyear.Items.Add(Convert.ToString(year));
            ddl_fromyear.Items.Add(Convert.ToString(year));
            year--;
        }
    }
    protected void bindmonth()
    {
        DateTime dt = new DateTime(2000, 1, 1);
        for (int m = 0; m < 12; m++)
        {
            ddl_frommonth.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
            ddl_tomonth.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
        }
    }
    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        cellclick = true;
    }
    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                ViewState["CumlativeHeader"] = null;
                popwindow.Visible = true;
                Fpspread2.Visible = false;
                filters_tbl.Visible = false;
                if (radFormat.Items[0].Selected == true)
                {
                    btn_cumulativetax.Visible = true;
                    btn_individualincometaxstatus.Visible = true;
                    btn_individualincometaxstatus_Click(sender, e);
                }
                else if (radFormat.Items[1].Selected == true)
                {
                    btn_cumulativetax.Visible = false;
                    btn_individualincometaxstatus.Visible = true;
                    btn_individualincometaxstatus_Click(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void btn_individualincometaxstatus_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            rptprint.Visible = false;
            Fpspread2.Visible = false;
            filters_tbl.Visible = false;
            individualcumlative_table.Visible = false;
            individualdiv.Visible = false;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.Sheets[0].ColumnCount = 5;
            Fpspread2.Sheets[0].ColumnHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            darkstyle.Border.BorderSize = 0;
            darkstyle.Border.BorderColor = Color.White;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Fpspread2.Columns[0].Locked = true;
            Fpspread2.Columns[1].Locked = true;
            Fpspread2.Columns[2].Locked = true;
            Fpspread2.Columns[3].Locked = true;
            Fpspread2.Columns[4].Locked = true;
            Fpspread2.Columns[0].Width = 80;
            Fpspread2.Columns[1].Width = 400;
            Fpspread2.Columns[2].Width = 50;
            Fpspread2.Columns[3].Width = 100;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "`";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "`";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "`";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "`";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "`";
            FarPoint.Web.Spread.StyleInfo darkstyle1 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle1.Border.BorderColor = Color.White;
            Fpspread2.Sheets[0].DefaultStyle = darkstyle1;

            int FontSize = 11;

            #region HeaderPart
            Fpspread2.Sheets[0].RowCount++;
            //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
            Fpspread2.Sheets[0].RowCount++;
            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "INCOME TAX CALCULATION STATEMENT";
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Green;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
            Fpspread2.Sheets[0].RowCount++;
            #region It Calculation Settings
            string frommonth = ""; string fromyear = ""; string tomonth = ""; string toyear = "";
            string itsetting = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='IT Calculation Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'");
            if (itsetting.Trim() != "0")
            {
                string[] linkvalue = itsetting.Split('-');
                if (linkvalue.Length > 0)
                {
                    frommonth = linkvalue[0].Split(',')[0];
                    fromyear = linkvalue[0].Split(',')[1];
                    tomonth = linkvalue[1].Split(',')[0];
                    toyear = linkvalue[1].Split(',')[1];
                }
            }
            else
            {
                alertmessage.Visible = true;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Set IT Calculation Settings";
                return;
            }
            #endregion
            string formmon = getmonth(Convert.ToInt32(frommonth)).ToUpper();
            string tomon = getmonth(Convert.ToInt32(tomonth)).ToUpper();
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "FOR THE PERIOD FROM " + formmon + " - " + fromyear + " TO " + tomon + " - " + toyear;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(itsetting);
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Gray;
            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
            Fpspread2.Sheets[0].RowCount++;
            double frmyear = 0; double toyr = 0; double.TryParse(fromyear, out frmyear); double.TryParse(toyear, out toyr); // poo 12.12.17
            frmyear = frmyear + 1; toyr = toyr + 1; // poo 12.12.17
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "ASSESSMENT YEAR " + frmyear + " - " + toyr; // poo 12.12.17
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Green;
            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
            Fpspread2.Sheets[0].RowCount++;
            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
            activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            if (activerow.Trim() != "-1" && activecol.Trim() != "-1")
            {
                string ApplID = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                string Gender = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                //   string age = d2.GetFunction("select DATEDIFF(yyyy,date_of_birth,getdate()) from staff_appl_master where appl_id='" + ApplID + "'");//delsi0803

                string age = d2.GetFunction("select  DATEDIFF(yy,date_of_birth,getdate())- CASE WHEN  DATEADD(YY,DATEDIFF(YY,date_of_birth,GETDATE()),date_of_birth) >GETDATE()THEN 1 Else 0 END As [Age] from staff_appl_master where appl_id='" + ApplID + "'");//delsi0604
                if (Convert.ToInt32(age) >= 60)//delsi2403
                {
                    if (Gender == "Male" || Gender == "MALE")
                    {

                        Gender = "Senior Citizen Male";
                    }
                    if (Gender == "Female" || Gender == "FEMALE")
                    {
                        Gender = "Senior Citizen Female";

                    }
                    if (Gender == "TransGender" || Gender == "TRANSGENDER")
                    {

                        Gender = "Senior Citizen TransGender";
                    }
                }
                Fpspread2.Sheets[0].RowCount++;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Name";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.MediumSlateBlue;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 2);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = " : " + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.Peru;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 2, 1, 3);
                Fpspread2.Sheets[0].RowCount++;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Designation";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.MediumSlateBlue;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 2);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = " : " + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.Peru;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 2, 1, 3);
                Fpspread2.Sheets[0].RowCount++;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Pan No";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.MediumSlateBlue;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 2);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = " : " + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.Peru;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 2, 1, 3);
            #endregion
                #region IncomeSalary
                Fpspread2.Sheets[0].RowCount++; Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5); Fpspread2.Sheets[0].RowCount++;
                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "1";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].Height = 60;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "SALARY INCOME: Including HRA, Honorarium Taxable allowances, Taxable perquistes (Excluding cash allowance if any)";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";

                string staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                string IncomeSalary = "";
                double HouseRentAmount = 0;
                double incSalary = 0;
                DateTime dtFrm = new DateTime();
                DateTime dtTo = new DateTime();
                int TotMonths = 0;
                //10.09.17 Added Barath
                double PayLastMonthAllowance = 0;
                double PayLastMonthDeduction = 0;
                int DiffenerceMonth = 0;
                double PayLastMonthSalary = 0;
                double lastGradePay = 0;
                int lastpayMonth = 0;
                int lastpayYear = 0;
                double reinvestment = 0;
                Hashtable PayLastMonthAllowanceHash = new Hashtable();
                Hashtable PayLastMonthDeductionHash = new Hashtable();
                if (radFormat.Items[0].Selected == true)
                {
                    IncomeSalary = d2.GetFunction(" select sum(netaddact)netaddact from monthlypay where  CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + staffcode + "'");
                    double.TryParse(d2.GetFunction(" select sum(Amount) as TotalAmount from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and ((ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "')) and staff_ApplID='" + ApplID + "' and CollegeCode='" + ddlcollege.SelectedValue + "'"), out HouseRentAmount);

                    //barath 25.09.17
                    double.TryParse(d2.GetFunction(" select sum(Amount) as TotalAmount from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='5' and ((ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "')) and staff_ApplID='" + ApplID + "' and CollegeCode='" + ddlcollege.SelectedValue + "'"), out reinvestment);//delsi 2509
                    string CalculateAllSet = d2.GetFunction("select linkValue from New_InsSettings where LinkName='Form16 Calculate All Month'  and user_code ='" + usercode + "' and college_code ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'");
                    if (!string.IsNullOrEmpty(CalculateAllSet) && CalculateAllSet.Trim() != "0")
                    {
                        string CalculateMonthDetQuery = "select paymonth,payyear,netaddact,netadd,addd,deddd,convert(varchar(max), allowances)as allowances,convert(varchar(max),deductions)as deductions,bsalary,grade_pay from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + staffcode + "' group by payyear,paymonth,netaddact,netadd,addd,deddd,convert(varchar(max), allowances),convert(varchar(max),deductions),bsalary,grade_pay order by year(payyear),year(paymonth) ";
                        CalculateMonthDetQuery += " select Amount,itmonth,ityear from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' ))  and CollegeCode='" + ddlcollege.SelectedValue + "' and staff_ApplID='" + ApplID + "' group by ityear,itmonth,Amount order by year(ityear),year(itmonth) ";//added percentage column
                        DataSet CalculateMonthDetDS = d2.select_method_wo_parameter(CalculateMonthDetQuery, "text");
                        if (CalculateMonthDetDS.Tables != null && CalculateMonthDetDS.Tables[0].Rows.Count > 0)
                        {
                            #region payProcessLastMonthSalary
                            double lastMonthSalary = 0;
                            double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["netaddact"]), out lastMonthSalary);
                            int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["paymonth"]), out lastpayMonth);
                            int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["payyear"]), out lastpayYear);
                            double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["addd"]), out PayLastMonthAllowance);
                            double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["deddd"]), out PayLastMonthDeduction);
                            double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["grade_pay"]), out lastGradePay);
                            PayLastMonthAllowanceHash = PayProcessAllowanceDet(CalculateMonthDetDS, 0, CalculateMonthDetDS.Tables[0].Rows.Count - 1, ref PayLastMonthSalary);
                            PayLastMonthDeductionHash = PayProcessDeductionDet(CalculateMonthDetDS, 0, CalculateMonthDetDS.Tables[0].Rows.Count - 1, ref PayLastMonthSalary);
                            DateTime FYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                            DateTime TYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                            DiffenerceMonth = (TYearDT.Month - FYearDT.Month) + 12 * (TYearDT.Year - FYearDT.Year);
                            double CurrentSalary = 0;
                            double.TryParse(IncomeSalary, out CurrentSalary);
                            CurrentSalary += lastMonthSalary * DiffenerceMonth;
                            IncomeSalary = Convert.ToString(CurrentSalary);
                            PayLastMonthSalary *= DiffenerceMonth;
                            lastGradePay *= DiffenerceMonth;
                            #endregion
                            #region ItCalculationLastMonthSalary
                            if (CalculateMonthDetDS.Tables[1].Rows.Count > 0)
                            {
                                double lastAllowanceAndDedutionAmt = 0;
                                int lastAllowanceAnddedutionMonth = 0;
                                int lastAllowanceAnddedutionyear = 0;
                                double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[1].Rows[CalculateMonthDetDS.Tables[1].Rows.Count - 1]["Amount"]), out lastAllowanceAndDedutionAmt);
                                int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[1].Rows[CalculateMonthDetDS.Tables[1].Rows.Count - 1]["itmonth"]), out lastAllowanceAnddedutionMonth);
                                int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[1].Rows[CalculateMonthDetDS.Tables[1].Rows.Count - 1]["ityear"]), out lastAllowanceAnddedutionyear);
                                FYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                                TYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                                int HouseRentDiffenerceMonth = (TYearDT.Month - FYearDT.Month) + 12 * (TYearDT.Year - FYearDT.Year);
                                //double HouseRentAmt = 0;05.12.17
                                //double prevHouseRentAmount = 0;
                                //HouseRentAmt += lastAllowanceAndDedutionAmt * HouseRentDiffenerceMonth;
                                //double.TryParse(HouseRentAmount, out prevHouseRentAmount);
                                //HouseRentAmount = Convert.ToString(prevHouseRentAmount + HouseRentAmt);
                                //double HouseRentAmt = 0;
                                //double.TryParse(IncomeSalary, out HouseRentAmt);
                                //HouseRentAmt += lastMonthSalary * HouseRentDiffenerceMonth;
                                //HouseRentAmount = Convert.ToString(HouseRentAmt);
                            }
                            #endregion
                        }
                    }
                }
                else if (radFormat.Items[1].Selected == true)
                {
                    double.TryParse(Convert.ToString(d2.GetFunction(" Select Gross_Sal from stafftrans where staff_code='" + staffcode + "' and latestrec='1'")), out incSalary);
                    if (incSalary > 0)
                    {
                        if (!String.IsNullOrEmpty(frommonth.Trim()) && frommonth.Trim() != "0" && !String.IsNullOrEmpty(fromyear.Trim()) && fromyear.Trim() != "0" && !String.IsNullOrEmpty(tomonth) && tomonth.Trim() != "0" && !String.IsNullOrEmpty(toyear.Trim()) && toyear.Trim() != "0")
                        {
                            dtFrm = Convert.ToDateTime(frommonth + "/01/" + fromyear);
                            dtTo = Convert.ToDateTime(tomonth + "/01/" + toyear);
                            TotMonths = ((dtTo.Month - dtFrm.Month) + 12 * (dtTo.Year - dtFrm.Year)) + 1;
                            incSalary = incSalary * TotMonths;
                            IncomeSalary = Convert.ToString(incSalary);
                        }
                    }
                }
                if (IncomeSalary.Trim() == "")
                {
                    Fpspread2.Visible = false;
                    rptprint.Visible = false;
                    btnprintmaster.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                    return;
                }
                //13.11.17
                double ActualBasicAmount = 0;

                DataSet chkotherallow = new DataSet();
                string qur = " select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,checkotherallow from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType ='4' and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and CollegeCode='" + ddlcollege.SelectedValue + "'  group by AllowdeductID,ITAllowDeductType,checkotherallow";

                chkotherallow = d2.select_method_wo_parameter(qur, "text");
                double totalotherallow = 0;
                if (chkotherallow.Tables[0].Rows.Count > 0)
                {
                    for (int val = 0; val < chkotherallow.Tables[0].Rows.Count; val++)
                    {
                        double getval = 0;
                        string DirectValue1 = Convert.ToString(chkotherallow.Tables[0].Rows[val]["TotalAmount"]);
                        double.TryParse(DirectValue1, out getval);
                        totalotherallow = totalotherallow + getval;

                    }

                }
                double.TryParse(IncomeSalary, out ActualBasicAmount);
                double AdditionAllowance = 0;
                double.TryParse(d2.GetFunction("select sum(a.AllowanceAmt)AllowanceAmt from AdditionalAllowanceAndDeduction a,CO_MasterValues m where m.MasterCode=AllowanceCode and m.MasterCriteria='additionalallowance' and a.CollegeCode=m.CollegeCode and a.CollegeCode='" + ddlcollege.SelectedValue + "' and a.staffcode='" + staffcode + "'"), out AdditionAllowance);
                ActualBasicAmount += (AdditionAllowance + totalotherallow);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ActualBasicAmount);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ActualBasicAmount);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                #endregion
                Hashtable AllowanceHash = new Hashtable();
                Hashtable DeductionHash = new Hashtable();
                Hashtable IncentiveMasterDeductionHash = new Hashtable();
                double TotalBasicAmount = 0;
                double GradePayTotal = 0;
                double CrossSalaryIncome = 0;
                string professionalTaxSettings = string.Empty;
                string ptstmonth = string.Empty;
                string ptendmonth = string.Empty;
                string ptstyear = string.Empty;
                string ptendyear = string.Empty;
                q1 = "select allowances,deductions,bsalary,grade_pay from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + staffcode + "'";
                q1 += " select LinkValue from New_InsSettings where LinkName='Professional Tax Calculation Month' and college_code ='" + ddlcollege.SelectedItem.Value + "' and user_code ='" + usercode + "'";
                q1 += " select deductions from incentives_master  where college_code = '" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables != null)
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        string st = Convert.ToString(ds.Tables[2].Rows[0]["deductions"]);
                        string[] split = st.Split(';');
                        foreach (var item in split)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                string staff = item;
                                string[] split1 = staff.Split('\\');
                                string description = split1[0];
                                string apprivation = split1[1];
                                if (!IncentiveMasterDeductionHash.ContainsKey(description))
                                    IncentiveMasterDeductionHash.Add(apprivation, description);
                            }
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        professionalTaxSettings = Convert.ToString(ds.Tables[1].Rows[0]["LinkValue"]);
                        if (!string.IsNullOrEmpty(professionalTaxSettings.Trim()))
                        {
                            string[] det = professionalTaxSettings.Split(';');
                            string monthyear = det[0];
                            string endmonthyear = det[1];
                            ptstmonth = monthyear.Split('-')[0];
                            ptendmonth = endmonthyear.Split('-')[0];
                            ptstyear = monthyear.Split('-')[1];
                            ptendyear = endmonthyear.Split('-')[1];
                        }
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    #region Allowance Deduction Calculation
                    for (int intds = 0; intds < ds.Tables[0].Rows.Count; intds++)
                    {
                        string AllowanceValue = Convert.ToString(ds.Tables[0].Rows[intds]["allowances"]);
                        string[] SplitFirst = AllowanceValue.Split('\\');
                        if (SplitFirst.Length > 0)
                        {
                            for (int intc = 0; intc < SplitFirst.Length; intc++)
                            {
                                if (SplitFirst[intc].Trim() != "")
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
                                        if (!AllowanceHash.ContainsKey(SecondSplit[0].Trim()))
                                        {
                                            AllowanceHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                                        }
                                        else
                                        {
                                            double GetValue = Convert.ToDouble(AllowanceHash[SecondSplit[0].Trim()]);
                                            GetValue = GetValue + AllowTaeknValue;
                                            AllowanceHash.Remove(SecondSplit[0].Trim());
                                            AllowanceHash.Add(SecondSplit[0].Trim(), GetValue);
                                        }
                                    }
                                }
                            }
                        }
                        AllowanceValue = Convert.ToString(ds.Tables[0].Rows[intds]["deductions"]);
                        SplitFirst = AllowanceValue.Split('\\');
                        if (SplitFirst.Length > 0)
                        {
                            for (int intc = 0; intc < SplitFirst.Length; intc++)
                            {
                                if (SplitFirst[intc].Trim() != "")
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
                                        if (!DeductionHash.ContainsKey(SecondSplit[0].Trim()))
                                        {
                                            DeductionHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                                        }
                                        else
                                        {
                                            double GetValue = Convert.ToDouble(DeductionHash[SecondSplit[0].Trim()]);
                                            GetValue = GetValue + AllowTaeknValue;
                                            DeductionHash.Remove(SecondSplit[0].Trim());
                                            DeductionHash.Add(SecondSplit[0].Trim(), GetValue);
                                        }
                                    }
                                }
                            }
                        }
                        TotalBasicAmount += Convert.ToDouble(ds.Tables[0].Rows[intds]["bsalary"]);
                        GradePayTotal += Convert.ToDouble(ds.Tables[0].Rows[intds]["grade_Pay"]);
                    }
                    #endregion
                    #region allmonth Calculation  15.11.17 barath
                    int Diffmonth = 0;
                    if (lastpayYear != 0 && lastpayMonth != 0)
                    {
                        GradePayTotal += lastGradePay;
                        TotalBasicAmount += PayLastMonthSalary;//25.11.17
                        DateTime FCalYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                        DateTime TCalYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                        Diffmonth = (TCalYearDT.Month - FCalYearDT.Month) + 12 * (TCalYearDT.Year - FCalYearDT.Year);
                        DateTime DummyDT = new DateTime();
                        DummyDT = FCalYearDT;
                        DummyDT = DummyDT.AddMonths(1);
                        TCalYearDT = TCalYearDT.AddMonths(1);
                        if (Diffmonth != 0)
                        {
                            double DeductionAmt = 0;
                            double DeductionValue = 0;
                            while (DummyDT < TCalYearDT)
                            {
                                #region last Month Allowance
                                foreach (DictionaryEntry dr in PayLastMonthAllowanceHash)
                                {
                                    DeductionAmt = 0; DeductionValue = 0;
                                    string AllowanceName = Convert.ToString(dr.Key).Trim();
                                    double.TryParse(Convert.ToString(dr.Value), out DeductionAmt);
                                    if (AllowanceHash.ContainsKey(AllowanceName.Trim()))
                                        double.TryParse(Convert.ToString(AllowanceHash[AllowanceName.Trim()]), out DeductionValue);
                                    DeductionValue += DeductionAmt;
                                    AllowanceHash[AllowanceName.Trim()] = DeductionValue;
                                }
                                #endregion
                                foreach (DictionaryEntry dr in PayLastMonthDeductionHash)
                                {
                                    string DeductionName = Convert.ToString(dr.Key).Trim();
                                    DeductionAmt = 0;
                                    double.TryParse(Convert.ToString(dr.Value), out DeductionAmt);
                                    DeductionValue = 0;
                                    if (!DeductionHash.ContainsKey(DeductionName.Trim()))
                                    {
                                        if (DeductionName.Trim().ToUpper() == "P.TAX" || DeductionName.Trim().ToUpper() == "P TAX" || DeductionName.Trim() == "PROFESSIONAL TAX" || DeductionName.Trim() == "PROFTAX")
                                        {
                                            if (DummyDT.ToString("MM").TrimStart('0') == ptstmonth && DummyDT.ToString("yyyy") == ptstyear || DummyDT.ToString("MM").TrimStart('0') == ptendmonth && DummyDT.ToString("yyyy") == ptendyear)
                                                DeductionHash.Add(DeductionName.Trim(), DeductionAmt);
                                            else
                                                DeductionHash.Add(DeductionName.Trim(), DeductionAmt);
                                        }
                                        else if (DeductionName.Trim().ToUpper() == "INC TAX" || DeductionName.Trim().ToUpper() == "I TAX" || DeductionName.Trim().ToUpper() == "INCOME TAX" || DeductionName.Trim().ToUpper() == "ITAX" || DeductionName.Trim().ToUpper() == "TDS")
                                        { }
                                    }
                                    else
                                    {
                                        if (DeductionName.Trim().ToUpper() == "P.TAX" || DeductionName.Trim().ToUpper() == "P TAX" || DeductionName.Trim() == "PROFESSIONAL TAX" || DeductionName.Trim() == "PROFTAX" || DeductionName.Trim()=="MISC")
                                        {
                                            if (DummyDT.ToString("MM").TrimStart('0') == ptstmonth && DummyDT.ToString("yyyy") == ptstyear || DummyDT.ToString("MM").TrimStart('0') == ptendmonth && DummyDT.ToString("yyyy") == ptendyear)
                                            {
                                                double.TryParse(Convert.ToString(DeductionHash[DeductionName.Trim()]), out DeductionValue);
                                                DeductionValue += DeductionAmt;
                                                DeductionHash[DeductionName.Trim()] = DeductionValue;
                                            }
                                        }
                                        else if (DeductionName.Trim().ToUpper() == "INC TAX" || DeductionName.Trim().ToUpper() == "I TAX" || DeductionName.Trim().ToUpper() == "INCOME TAX" || DeductionName.Trim().ToUpper() == "ITAX" || DeductionName.Trim().ToUpper() == "TDS")
                                        { }
                                        else
                                        {
                                            double.TryParse(Convert.ToString(DeductionHash[DeductionName.Trim()]), out DeductionValue);
                                            DeductionValue += DeductionAmt;
                                            DeductionHash[DeductionName.Trim()] = DeductionValue;
                                        }
                                    }
                                }
                                DummyDT = DummyDT.AddMonths(1);
                            }
                        }
                    }
                    // Added by for Additional deduction in Additional Allowance poomalar 04.12.17
                    #region Addition Deduction
                    string queryded = "select AllowanceDeductAmt from AdditionalAllowanceAndDeduction a,CO_MasterValues m where m.MasterCode=AllowanceCode and m.MasterCriteria='additionalallowance' and a.CollegeCode=m.CollegeCode and a.CollegeCode='" + ddlcollege.SelectedValue + "' and a.staffcode='" + staffcode + "'";
                    DataSet dsded = new DataSet();
                    dsded = d2.select_method_wo_parameter(queryded, "Text");
                    if (dsded.Tables[0].Rows.Count > 0)
                    {
                        for (int ded = 0; ded < dsded.Tables[0].Rows.Count; ded++)
                        {
                            string splded = Convert.ToString(dsded.Tables[0].Rows[ded]["AllowanceDeductAmt"]);
                            string[] spldedname = splded.Split(';'); double dedvalue = 0;
                            if (spldedname.Length > 0)
                            {
                                for (int spld = 0; spld < spldedname.Length; spld++)
                                {
                                    if (spldedname[spld].Contains('-'))
                                    {
                                        string dednameadd = spldedname[spld].Split('-')[0]; //splded.Split(';')[spld].Split('-')[0];
                                        string dedvalueadd = spldedname[spld].Split('-')[1];// splded.Split(';')[spld].Split('-')[1];
                                        double.TryParse(dedvalueadd, out dedvalue);
                                        if (!DeductionHash.ContainsKey(dednameadd))
                                            DeductionHash.Add(dednameadd, dedvalue);
                                        else
                                        {
                                            double value = 0;
                                            double.TryParse(Convert.ToString(DeductionHash[dednameadd]), out value);
                                            value += dedvalue;
                                            DeductionHash[dednameadd] = value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                    #region House Rent Caluculation
                    string Distict = d2.GetFunction("  select MasterValue from staff_appl_master sa,Staffmaster s ,co_mastervalues c where s.appl_no=sa.appl_no and convert(varchar(max), c.MasterCode)=isnull(Pdistrict,0) and staff_code ='" + staffcode + "'");
                    //House Rent Allowance 
                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "2";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "LESS: House Rent Allowance (Sec.10(13A)&Rule2A)";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "A. Actual HRA received of";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "B. Rent paid less 10% of Salary + DA";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "C. Chennai, Mumbai, Calcutta & Delhi Employees 50% of salary, Others 40%";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                    Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].Height = 40;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                    double HouseRent = 0;
                    //double TotalHouseRentAmount = 0;
                    double TotalHRA = 0;
                    double DAAmount = 0;
                    double PercentHouseRent = 0;
                    double RentPaidAmount = 0;
                    double HalfPercentofActualSalary = 0;

                    string SalaryDeductHouseRentName = d2.GetFunction(" select distinct CommonDuduction from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and CommonDuduction is not NULL ");//barath 20.01.18
                    if (!string.IsNullOrEmpty(SalaryDeductHouseRentName))
                    {
                        double SalaryDeductHouseRent = 0;
                        if (DeductionHash.ContainsKey(SalaryDeductHouseRentName))
                            double.TryParse(Convert.ToString(DeductionHash[SalaryDeductHouseRentName]), out SalaryDeductHouseRent);
                        HouseRentAmount += SalaryDeductHouseRent;
                    }
                    if (HouseRentAmount != 0 && TotalBasicAmount != 0)
                    {
                        //double.TryParse(HouseRentAmount, out TotalHouseRentAmount);
                        if (AllowanceHash.ContainsKey("HRA"))
                        {
                            TotalHRA = Convert.ToDouble(AllowanceHash["HRA"]);
                        }
                        if (AllowanceHash.ContainsKey("DA"))
                        {
                            DAAmount = Convert.ToDouble(AllowanceHash["DA"]);
                        }
                        PercentHouseRent = (TotalBasicAmount + GradePayTotal + DAAmount) * 10 / 100;
                        if (PercentHouseRent > HouseRentAmount)
                        {
                            RentPaidAmount = PercentHouseRent - HouseRentAmount;
                        }
                        else
                        {
                            RentPaidAmount = HouseRentAmount - PercentHouseRent;
                        }
                        double gross = 0; double.TryParse(IncomeSalary, out gross); // poo 12.12.17
                        if (Distict.Trim().ToLower() == "chennai" || Distict.Trim().ToLower() == "mumbai" || Distict.Trim().ToLower() == "calcutta" || Distict.Trim().ToLower() == "delhi")
                        {
                            //HalfPercentofActualSalary = (ActualBasicAmount) / 100 * 50; // commented by poo 12.12.17
                            HalfPercentofActualSalary = (gross) / 100 * 50; // poo 12.12.17
                        }
                        else
                        {
                            //HalfPercentofActualSalary = (ActualBasicAmount) / 100 * 40; //commented by poo 12.12.17
                            HalfPercentofActualSalary = (gross) / 100 * 40; // poo 12.12.17
                        }
                        string selHouseRent = d2.GetFunction("select value from Master_Settings where settings='ViewHouseRent' and usercode='" + usercode + "'");
                        double houserentval = 0;
                        if (selHouseRent.Trim() != "" && selHouseRent.Trim() != "0" && selHouseRent.Trim() == "1")
                        {
                            DataSet houserentcheck = new DataSet();
                            string qry = "select ITGroupPK,GroupName,GroupDesc,MaxLimitAmount from IT_GroupMaster where parentCode='0' and collegeCode='" + ddlcollege.SelectedValue + "' order by isnull(Priority,10000) asc";
                            qry = qry + " select ITGroupPK,GroupName,GroupDesc,ParentCode,ITGroupType,IT_IDFK,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue,IsAgeRange,MaxValue,MinValue from IT_GroupMaster IT, IT_GroupMapping IM,IT_OtherAllowanceDeducation AD where IT.ITGroupPK=IM.ITGroupFK and AD.IT_ID=IM.IT_IDFK and IT.CollegeCode='" + ddlcollege.SelectedValue + "'";
                            qry += " select distinct ITGroupPK,GroupName,GroupDesc,MaxLimitAmount,parentCode,isnull(Priority,10000) from IT_GroupMaster IT,IT_GroupMapping IM where IT.ITGroupPk=IM.ITGroupFK and collegeCode='" + ddlcollege.SelectedValue + "' order by isnull(Priority,10000) asc";
                            qry += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType in   (1,2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and CollegeCode='" + ddlcollege.SelectedValue + "'  group by AllowdeductID,ITAllowDeductType,percentage";
                            qry += " select convert(bigint ,round(FromRange,0)) FromRange,convert(bigint ,round (ToRange,0)) ToRange,Amount,mode  from HR_ITCalculationSettings where collegeCode='" + ddlcollege.SelectedValue + "' and sex ='" + Gender.Trim() + "'";
                            qry += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details ID,IT_OtherAllowanceDeducation IA where ID.AllowDeductID=IA.IT_ID and ITAllowdeductType in (2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and IA.CollegeCode='" + ddlcollege.SelectedValue + "' and isnull(IsIncomeTax,'0')='1'  group by AllowdeductID,ITAllowDeductType,percentage";
                            qry += " select IT_ID,ITCommon,ITCommonValue,ITType from IT_OtherAllowanceDeducation  where  isnull(IsIncomeTax,'0')='1'  and CollegeCode='" + ddlcollege.SelectedValue + "'";
                            houserentcheck.Clear();
                            houserentcheck = d2.select_method_wo_parameter(qry, "text");
                           
                            DataView dvnew1 = new DataView();
                            DataView dvh = new DataView();
                            if (houserentcheck.Tables.Count > 1 && houserentcheck.Tables[0].Rows.Count > 0)
                            {
                               
                                for (int k = 0; k < houserentcheck.Tables[0].Rows.Count; k++)
                                {
                                  
                                    #region subMain
                                    houserentcheck.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(houserentcheck.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                                    dvnew1 = houserentcheck.Tables[1].DefaultView;
                                    if (dvnew1.Count > 0)
                                    {
                                        double MaxLimitAmount = 0;
                                        string MaxAmount = Convert.ToString(houserentcheck.Tables[0].Rows[k]["MaxLimitAmount"]);
                                        double.TryParse(MaxAmount, out MaxLimitAmount);
                                        double OverAllTotal = 0;
                                        for (int intCh = 0; intCh < dvnew1.Count; intCh++)
                                        {
                                            string gettext = Convert.ToString(dvnew1[intCh]["ITAllowDeductName"]);
                                            if (gettext == "Less:Deduction For Interest Payable On Housing Loan")
                                            {
                                                string ITType = Convert.ToString(dvnew1[intCh]["ITType"]);
                                                string ITCommon = Convert.ToString(dvnew1[intCh]["ITCommon"]);
                                                string ITCommonValue = Convert.ToString(dvnew1[intCh]["ITCommonValue"]);

                                                houserentcheck.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew1[intCh]["IT_IDFK"]) + "'";
                                                dvh = houserentcheck.Tables[3].DefaultView;
                                                if (dvh.Count > 0)
                                                {
                                                    string DirectValue = Convert.ToString(dvh[0]["TotalAmount"]);

                                                    double.TryParse(DirectValue, out houserentval);
                                                }
                                                if (houserentval > 0)
                                                {
                                                    TotalHRA = 0;
                                                    RentPaidAmount = 0;
                                                    HalfPercentofActualSalary = 0;
                                                }
                                            }
                                           

                                        }
                                    #endregion

                                    }

                                }

                            }

                        }
                        
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 3, 3].Text = Convert.ToString(Math.Round(TotalHRA));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 3, 3].Font.Size = FontSize;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 3, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 3, 3].Font.Name = "Arial";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 2, 3].Text = Convert.ToString(Math.Round(RentPaidAmount));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 2, 3].Font.Size = FontSize;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 2, 3].Font.Name = "Arial";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(HalfPercentofActualSalary));
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontSize;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                        if (TotalHRA < RentPaidAmount && TotalHRA < HalfPercentofActualSalary)
                        {
                            HouseRent = TotalHRA;
                        }
                        else if (RentPaidAmount < TotalHRA && RentPaidAmount < HalfPercentofActualSalary)
                        {
                            HouseRent = RentPaidAmount;
                        }
                        else if (HalfPercentofActualSalary < RentPaidAmount && HalfPercentofActualSalary < TotalHRA)
                        {
                            HouseRent = HalfPercentofActualSalary;
                        }
                    }
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 4, 4].Text = Convert.ToString(Math.Round(HouseRent));
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 4, 4].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 4, 4].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 4, 4].Font.Name = "Arial";
                    double GrossSalary = Convert.ToDouble(ActualBasicAmount) - Math.Round(HouseRent);
                    CrossSalaryIncome = GrossSalary;
                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "TAXABLE SALARY INCOME (1-2)";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(GrossSalary);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                    //10.10.17 barath
                    //Fpspread2.Sheets[0].RowCount++;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "LESS:Professional Tax[u/s 16(III)]";
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                    //double TaxableIncome = 0;
                    //string Lessprofessionaltax = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Less Professional Tax' and college_code='" + ddlcollege.SelectedItem.Value + "'");
                    //if (DeductionHash.Contains("INC TAX"))
                    //    double.TryParse(Convert.ToString(DeductionHash["INC TAX"]), out TaxableIncome);
                    ////double.TryParse(Convert.ToString(Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3]), out TaxableIncome);
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(TaxableIncome);
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                    //Fpspread2.Sheets[0].RowCount++;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "TAXABLE SALARY INCOME [3-4]";
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                    //GrossSalary -= TaxableIncome;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(GrossSalary);
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size =FontSize;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                    #endregion
                    int Count = 2;
                    double lessval = 0;
                    double LicAmt = 0;
                    if (radFormat.Items[0].Selected == true)
                    {
                        double Amt = 0;
                        DataView dv = new DataView();
                        DataView dvnew = new DataView();
                        DataView dAllview = new DataView();
                        DataTable dt = new DataTable();
                        Hashtable settingallow = new Hashtable();
                        string ITType = string.Empty;
                        string ITCommon = string.Empty;
                        string ITCommonValue = string.Empty;
                        string percentageval = string.Empty;

                        string maxAgeValue = string.Empty;
                        string minAgeValue = string.Empty;
                        string agechecked = string.Empty;
                        double maxAge = 0;
                        double minAge = 0;
                        double maxVal = 0;
                        double minVal = 0;

                        q1 = "select ITGroupPK,GroupName,GroupDesc,MaxLimitAmount from IT_GroupMaster where parentCode='0' and collegeCode='" + ddlcollege.SelectedValue + "' order by isnull(Priority,10000) asc";
                        q1 = q1 + " select ITGroupPK,GroupName,GroupDesc,ParentCode,ITGroupType,IT_IDFK,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue,IsAgeRange,MaxValue,MinValue from IT_GroupMaster IT, IT_GroupMapping IM,IT_OtherAllowanceDeducation AD where IT.ITGroupPK=IM.ITGroupFK and AD.IT_ID=IM.IT_IDFK and IT.CollegeCode='" + ddlcollege.SelectedValue + "'";
                        q1 += " select distinct ITGroupPK,GroupName,GroupDesc,MaxLimitAmount,parentCode,isnull(Priority,10000) from IT_GroupMaster IT,IT_GroupMapping IM where IT.ITGroupPk=IM.ITGroupFK and collegeCode='" + ddlcollege.SelectedValue + "' order by isnull(Priority,10000) asc";
                        q1 += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType in   (1,2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and CollegeCode='" + ddlcollege.SelectedValue + "'  group by AllowdeductID,ITAllowDeductType,percentage";
                        q1 += " select convert(bigint ,round(FromRange,0)) FromRange,convert(bigint ,round (ToRange,0)) ToRange,Amount,mode  from HR_ITCalculationSettings where collegeCode='" + ddlcollege.SelectedValue + "' and sex ='" + Gender.Trim() + "'";
                        q1 += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details ID,IT_OtherAllowanceDeducation IA where ID.AllowDeductID=IA.IT_ID and ITAllowdeductType in (2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplID + "' and IA.CollegeCode='" + ddlcollege.SelectedValue + "' and isnull(IsIncomeTax,'0')='1'  group by AllowdeductID,ITAllowDeductType,percentage";
                        q1 += " select IT_ID,ITCommon,ITCommonValue,ITType from IT_OtherAllowanceDeducation  where  isnull(IsIncomeTax,'0')='1'  and CollegeCode='" + ddlcollege.SelectedValue + "'";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(q1, "text");

                        double staff_age = 0;
                        double.TryParse(age, out staff_age);

                        if (ds1.Tables.Count > 1 && ds1.Tables[0].Rows.Count > 0)
                        {
                            int RowHeight = 0;
                            for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                            {
                                double CommomOverAllTotal = 0;
                                double GrandCommonTotal = 0;
                                ds1.Tables[2].DefaultView.RowFilter = "parentCode='" + Convert.ToString(ds1.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                                dv = ds1.Tables[2].DefaultView;
                                if (dv.Count > 0)
                                {
                                    #region Main
                                    Count++;
                                    Fpspread2.Sheets[0].RowCount++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Count);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    if (CbShowDiscription.Checked)
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds1.Tables[0].Rows[k]["GroupDesc"]);
                                    else
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds1.Tables[0].Rows[k]["GroupName"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                    int Cs = 0;
                                    string Commonoverall = Convert.ToString(ds1.Tables[0].Rows[k]["MaxLimitAmount"]);
                                    double.TryParse(Commonoverall, out CommomOverAllTotal);
                                    for (int intn = 0; intn < dv.Count; intn++)
                                    {
                                        ds1.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(dv[intn]["ITGroupPK"]) + "'";
                                        dvnew = ds1.Tables[1].DefaultView;
                                        if (dvnew.Count > 0)
                                        {
                                            Cs++;
                                            Fpspread2.Sheets[0].RowCount++;
                                            if (!CbShowDiscription.Checked)
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "(" + Alpha(Cs) + ")" + " " + Convert.ToString(dv[intn]["GroupName"]);
                                            else
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = "(" + Alpha(Cs) + ")" + " " + Convert.ToString(dv[intn]["GroupDesc"]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                            double MaxLimitAmount = 0;
                                            string MaxAmount = Convert.ToString(dv[intn]["MaxLimitAmount"]);
                                            double.TryParse(MaxAmount, out MaxLimitAmount);
                                            double OverAllTotal = 0;
                                            for (int intCh = 0; intCh < dvnew.Count; intCh++)
                                            {
                                                Fpspread2.Sheets[0].RowCount++;
                                                if (!CbShowDiscription.Checked)
                                                {
                                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvnew[intCh]["ITAllowDeductName"]);
                                                    RowHeight = SpreadExcelHeight(Convert.ToString(dvnew[intCh]["ITAllowDeductName"]));
                                                }
                                                else
                                                {
                                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dvnew[intCh]["ITAllowDeductDiscription"]);
                                                    RowHeight = SpreadExcelHeight(Convert.ToString(dvnew[intCh]["ITAllowDeductDiscription"]));
                                                }
                                                Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].Height = RowHeight;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dvnew[intCh]["IT_IDFK"]);
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                                double AllowAndDeductTotal = 0;
                                                double DirectAllowDeductValue = 0;
                                                double Getvalue = 0;
                                                double AdditionalDeduction = 0;
                                                ITType = Convert.ToString(dvnew[intCh]["ITType"]);
                                                //int it = (int)(dvnew[intCh]["ITType"]);
                                                ITCommon = Convert.ToString(dvnew[intCh]["ITCommon"]);
                                                ITCommonValue = Convert.ToString(dvnew[intCh]["ITCommonValue"]);

                                                agechecked = Convert.ToString(dvnew[intCh]["IsAgeRange"]);
                                                maxAgeValue = Convert.ToString(dvnew[intCh]["MaxValue"]);//delsi0803
                                                minAgeValue = Convert.ToString(dvnew[intCh]["MinValue"]);//delsi0803

                                                //if (agechecked.Trim() == "1" || agechecked.Trim() == "True")//delsi0803
                                                //{
                                                //    string[] maxArr = maxAgeValue.Split('-');
                                                //    string[] minArr = minAgeValue.Split('-');
                                                //    if (maxArr.Length > 1)
                                                //    {
                                                //        double.TryParse(Convert.ToString(maxArr[0]), out maxAge);
                                                //        double.TryParse(Convert.ToString(maxArr[1]), out maxVal);
                                                //    }
                                                //    if (minArr.Length > 1)
                                                //    {
                                                //        double.TryParse(Convert.ToString(minArr[0]), out minAge);
                                                //        double.TryParse(Convert.ToString(minArr[1]), out minVal);
                                                //    }


                                                //}//delsi0803

                                                if (ITCommon.Trim() == "1" || ITCommon.Trim() == "True")
                                                {
                                                    if (ITType.Trim() == "1")
                                                    {
                                                        if (ITCommonValue.Trim() != "")
                                                        {
                                                            //16.12.17 barath
                                                            if (!string.IsNullOrEmpty(ITCommonValue))
                                                            {
                                                                string[] AllowanceName = ITCommonValue.Split(',');
                                                                foreach (var item in AllowanceName)
                                                                {
                                                                    if (!string.IsNullOrEmpty(item))
                                                                    {
                                                                        if (AllowanceHash.ContainsKey(item.Trim()))
                                                                        {
                                                                            double.TryParse(Convert.ToString(AllowanceHash[item.Trim()]), out AdditionalDeduction);
                                                                            AllowAndDeductTotal += AdditionalDeduction;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //Getvalue = Convert.ToString(AllowanceHash[ITCommonValue.Trim()]);
                                                        }
                                                    }
                                                    else if (ITType.Trim() == "2")
                                                    {
                                                        if (ITCommonValue.Trim() != "")
                                                        {
                                                            //16.12.17 barath
                                                            if (!string.IsNullOrEmpty(ITCommonValue))
                                                            {
                                                                string[] DeductionName = ITCommonValue.Split(',');
                                                                foreach (var item in DeductionName)
                                                                {
                                                                    if (!string.IsNullOrEmpty(item))
                                                                    {
                                                                        if (DeductionHash.ContainsKey(item.Trim()))
                                                                        {
                                                                            double.TryParse(Convert.ToString(DeductionHash[item.Trim()]), out AdditionalDeduction);
                                                                            AllowAndDeductTotal += AdditionalDeduction;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //Getvalue = Convert.ToString(DeductionHash[ITCommonValue.Trim()]);
                                                        }
                                                    }
                                                    //double.TryParse(Getvalue, out AllowAndDeductTotal);
                                                }
                                                ds1.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew[intCh]["IT_IDFK"]) + "'";
                                                dAllview = ds1.Tables[3].DefaultView;
                                                if (dAllview.Count > 0)
                                                {
                                                    string DirectValue = Convert.ToString(dAllview[0]["TotalAmount"]);
                                                    double.TryParse(DirectValue, out DirectAllowDeductValue);
                                                    if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIC")//jayaram
                                                        LicAmt = DirectAllowDeductValue;
                                                    else if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIFE INSURANCE")
                                                        LicAmt = DirectAllowDeductValue;
                                                    else if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIFE INSURANCE(LIC)")
                                                        LicAmt = DirectAllowDeductValue;
                                                }
                                                else //barath 21.11.17
                                                {
                                                    if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).Trim().Contains('/'))
                                                    {
                                                        string[] DeductionName = Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).Trim().Split('/');
                                                        double DedutionAmt = 0;
                                                        foreach (string deduct in DeductionName)
                                                        {
                                                            DedutionAmt = 0;
                                                            string deductShort = Convert.ToString(IncentiveMasterDeductionHash[deduct]);
                                                            if (!string.IsNullOrEmpty(deductShort))
                                                            {
                                                                if (DeductionHash.ContainsKey(deductShort))
                                                                    double.TryParse(Convert.ToString(DeductionHash[deductShort]), out DedutionAmt);
                                                            }
                                                            DirectAllowDeductValue += DedutionAmt;
                                                        }
                                                        //DirectAllowDeductValue += DedutionAmt;
                                                        //double dfk = Convert.ToDouble(DeductionHash["DA"]);
                                                    }
                                                }
                                                DirectAllowDeductValue += AllowAndDeductTotal;
                                                if (age != "0")
                                                {

                                                    if (agechecked.Trim() == "1" || agechecked.Trim() == "True")//delsi0803
                                                    {
                                                        string[] maxArr = maxAgeValue.Split('-');
                                                        string[] minArr = minAgeValue.Split('-');
                                                        if (maxArr.Length == 2)
                                                        {
                                                            double.TryParse(Convert.ToString(maxArr[0]), out maxAge);
                                                            double.TryParse(Convert.ToString(maxArr[1]), out maxVal);
                                                        }
                                                        if (minArr.Length == 2)
                                                        {
                                                            double.TryParse(Convert.ToString(minArr[0]), out minAge);
                                                            double.TryParse(Convert.ToString(minArr[1]), out minVal);
                                                        }
                                                        //if (maxAge != 0 && maxAge <= staff_age)
                                                        //    if (maxVal != 0 && maxVal > DirectAllowDeductValue)
                                                        //        DirectAllowDeductValue = maxVal;
                                                        //if (minAge != 0 && minAge > staff_age)
                                                        //    if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                        //        DirectAllowDeductValue = minVal;


                                                        if (maxAge != 0 && maxAge < staff_age)
                                                            if (maxVal != 0 && maxVal < DirectAllowDeductValue)//delsi09ref

                                                                DirectAllowDeductValue = maxVal;
                                                        if (minAge != 0 && minAge > staff_age)
                                                            if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                                DirectAllowDeductValue = minVal;
                                                    }
                                                }
                                                OverAllTotal += DirectAllowDeductValue;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(DirectAllowDeductValue));
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontSize;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                                            }
                                            string MaxWord = string.Empty;
                                            if (MaxLimitAmount != 0)
                                            {
                                                MaxWord = " restricted to Rs." + MaxLimitAmount + "/-";
                                            }
                                            //Fpspread2.Sheets[0].RowCount++;
                                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Total Amount  " + MaxWord + "";//20.11.17 barath
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(OverAllTotal));//20.11.17 barath
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontSize;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                                            if (MaxLimitAmount != 0 && MaxLimitAmount > OverAllTotal)
                                            {
                                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(OverAllTotal));
                                                GrandCommonTotal += OverAllTotal;
                                            }
                                            else if (MaxLimitAmount != 0 && OverAllTotal > MaxLimitAmount)
                                            {
                                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(MaxLimitAmount));
                                                GrandCommonTotal += MaxLimitAmount;
                                            }
                                            else
                                            {
                                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(OverAllTotal));
                                                GrandCommonTotal += OverAllTotal;
                                            }
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                                        }
                                    }
                                    string WordMax = string.Empty;
                                    if (CommomOverAllTotal != 0)
                                    {
                                        WordMax = " restricted to Rs." + CommomOverAllTotal + "/-";
                                    }
                                    Fpspread2.Sheets[0].RowCount++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Grand Total Amount  " + WordMax + "";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(GrandCommonTotal));
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                                    double MainAmount = 0;
                                    if (CommomOverAllTotal != 0 && CommomOverAllTotal > GrandCommonTotal)
                                    {
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(GrandCommonTotal));
                                        MainAmount = GrandCommonTotal;
                                    }
                                    else if (CommomOverAllTotal != 0 && GrandCommonTotal > CommomOverAllTotal)
                                    {
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(CommomOverAllTotal));
                                        MainAmount = CommomOverAllTotal;
                                    }
                                    else
                                    {
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(GrandCommonTotal));
                                        MainAmount = GrandCommonTotal;
                                    }
                                    if (ITType.Trim() == "1")
                                    {
                                        GrossSalary = Convert.ToDouble(CrossSalaryIncome) + Math.Round(MainAmount);
                                        CrossSalaryIncome = GrossSalary;
                                    }
                                    else if (ITType.Trim() == "2")
                                    {
                                        GrossSalary = Convert.ToDouble(CrossSalaryIncome) - Math.Round(MainAmount);
                                        CrossSalaryIncome = GrossSalary;
                                    }
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                                    Fpspread2.Sheets[0].RowCount++;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "TAXABLE SALARY INCOME";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(GrossSalary);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                                    #endregion
                                }
                                else
                                {
                                    #region subMain
                                    ds1.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(ds1.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                                    dvnew = ds1.Tables[1].DefaultView;
                                    if (dvnew.Count > 0)
                                    {
                                        double MaxLimitAmount = 0;
                                        string MaxAmount = Convert.ToString(ds1.Tables[0].Rows[k]["MaxLimitAmount"]);
                                        double.TryParse(MaxAmount, out MaxLimitAmount);
                                        double OverAllTotal = 0;
                                        for (int intCh = 0; intCh < dvnew.Count; intCh++)
                                        {
                                            Fpspread2.Sheets[0].RowCount++;
                                            if (!CbShowDiscription.Checked)
                                            {
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvnew[intCh]["ITAllowDeductName"]);
                                                RowHeight = SpreadExcelHeight(Convert.ToString(dvnew[intCh]["ITAllowDeductName"]));
                                            }
                                            else
                                            {
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dvnew[intCh]["ITAllowDeductDiscription"]);
                                                RowHeight = SpreadExcelHeight(Convert.ToString(dvnew[intCh]["ITAllowDeductDiscription"]));
                                            }
                                            Fpspread2.Sheets[0].Rows[Fpspread2.Sheets[0].RowCount - 1].Height = RowHeight;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dvnew[intCh]["IT_IDFK"]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                            double AllowAndDeductTotal = 0;
                                            double DirectAllowDeductValue = 0;
                                            string Getvalue = string.Empty;
                                            ITType = Convert.ToString(dvnew[intCh]["ITType"]);
                                            ITCommon = Convert.ToString(dvnew[intCh]["ITCommon"]);
                                            ITCommonValue = Convert.ToString(dvnew[intCh]["ITCommonValue"]);


                                            //delsi0803
                                            agechecked = Convert.ToString(dvnew[intCh]["IsAgeRange"]);
                                            maxAgeValue = Convert.ToString(dvnew[intCh]["MaxValue"]);
                                            minAgeValue = Convert.ToString(dvnew[intCh]["MinValue"]);//delsi0803


                                            if (ITCommon.Trim() == "1" || ITCommon.Trim() == "True")
                                            {
                                                if (ITType.Trim() == "1")
                                                {
                                                    if (ITCommonValue.Trim() != "")
                                                    {
                                                        Getvalue = Convert.ToString(AllowanceHash[ITCommonValue.Trim()]);
                                                    }
                                                }
                                                else if (ITType.Trim() == "2")
                                                {
                                                    if (ITCommonValue.Trim() != "")
                                                    {
                                                        Getvalue = Convert.ToString(DeductionHash[ITCommonValue.Trim()]);
                                                    }
                                                }
                                                double.TryParse(Getvalue, out AllowAndDeductTotal);
                                            }
                                            ds1.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew[intCh]["IT_IDFK"]) + "'";
                                            dAllview = ds1.Tables[3].DefaultView;
                                            if (dAllview.Count > 0)
                                            {
                                                string percentage_val = Convert.ToString(dAllview[0]["Percentage"]);//delsi2209
                                                string DirectValue = Convert.ToString(dAllview[0]["TotalAmount"]);
                                                if (percentage_val != "" && percentage_val != null)
                                                {
                                                    double calculatepercent = (Convert.ToDouble(DirectValue) / 100) * Convert.ToDouble(percentage_val);
                                                    DirectValue = Convert.ToString(calculatepercent);

                                                }
                                                double.TryParse(DirectValue, out DirectAllowDeductValue);
                                            }
                                            DirectAllowDeductValue += AllowAndDeductTotal;
                                            if (age != "0")
                                            {
                                                if (agechecked.Trim() == "1" || agechecked.Trim() == "True")//delsi0803
                                                {
                                                    string[] maxArr = maxAgeValue.Split('-');
                                                    string[] minArr = minAgeValue.Split('-');
                                                    if (maxArr.Length == 2)
                                                    {
                                                        double.TryParse(Convert.ToString(maxArr[0]), out maxAge);
                                                        double.TryParse(Convert.ToString(maxArr[1]), out maxVal);
                                                    }
                                                    if (minArr.Length == 2)
                                                    {
                                                        double.TryParse(Convert.ToString(minArr[0]), out minAge);
                                                        double.TryParse(Convert.ToString(minArr[1]), out minVal);
                                                    }
                                                    if (maxAge != 0 && maxAge < staff_age)
                                                        if (maxVal != 0 && maxVal < DirectAllowDeductValue)//delsi09ref

                                                            DirectAllowDeductValue = maxVal;
                                                    if (minAge != 0 && minAge > staff_age)
                                                        if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                            DirectAllowDeductValue = minVal;
                                                }
                                            }
                                            //delsi0803
                                            OverAllTotal += DirectAllowDeductValue;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(DirectAllowDeductValue));
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontSize;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                                        }
                                        double MainAmount = 0;
                                        string MaxWord = string.Empty;
                                        if (MaxLimitAmount != 0)
                                        {
                                            MaxWord = " restricted to Rs." + MaxLimitAmount + "/-";
                                        }
                                        //Fpspread2.Sheets[0].RowCount++;
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Total Amount  " + MaxWord + "";//20.11.17 barath
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(OverAllTotal));//20.11.17 barath
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontSize;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                                        if (MaxLimitAmount != 0 && MaxLimitAmount > OverAllTotal)
                                        {
                                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(OverAllTotal));
                                            MainAmount = OverAllTotal;
                                        }
                                        else if (MaxLimitAmount != 0 && OverAllTotal > MaxLimitAmount)
                                        {
                                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(MaxLimitAmount));
                                            MainAmount = MaxLimitAmount;
                                        }
                                        else
                                        {
                                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(OverAllTotal));
                                            MainAmount = OverAllTotal;
                                        }
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                                        if (ITType.Trim() == "1")
                                        {
                                            GrossSalary = Convert.ToDouble(CrossSalaryIncome) + Math.Round(MainAmount);
                                            CrossSalaryIncome = GrossSalary;
                                        }
                                        else if (ITType.Trim() == "2")
                                        {
                                            GrossSalary = Convert.ToDouble(CrossSalaryIncome) - Math.Round(MainAmount);
                                            CrossSalaryIncome = GrossSalary;
                                        }
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                                        Fpspread2.Sheets[0].RowCount++;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "TAXABLE SALARY INCOME";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(GrossSalary);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                                    }
                                    #endregion
                                }
                            }
                            #region ODD Code
                            //10.10.17 barath
                            /*
                             #region TotalQualifying Amount
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "TOTAL QUALIFYING AMOUNT";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Maximum amount deduction u/s 80C is Rs.1,50,000/-";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             double DeductionAmt = 0;
                             if (DeductionAmt >= 150000)
                             {
                                 DeductionAmt = 150000;
                             }
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(DeductionAmt);
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "(ii) U/s 80CCC: Contribution to Pension Fund";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "(iii) U/s 80C: Contribution to Pension Scheme";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Aggregate amount of deduction u/s 80C,80CCC,80CCD is restricted to Rs.1,50,000/- by Sec. 80CCE";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Subscription to Equity Shares / Debentures or Units (Infrastructure Bond, etc)";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "U/s 80D: Medical insurance premium paid in the name of assessee, spouse, dependent parents or dependent children (Maximum Rs.25,000, for senior citizens Rs.40000/-)";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "U/s 80D: Expenses on medical treatment etc. and deposit made for maintenance or handicapped dependents (Max. Rs.50000/-) incase of severe disabilities Rs.75000/-";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "U/s 80DDB: Medical expenses towards treatment of himself, or a dependent relative for specified diseases and ailments, (Amount actually paid or Rs.40000/-) whichever is less, Form 10 should be enclosed) For senior citizens Rs.60000/- or expenditure incurred.)";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "U/s 80E: Repayment of interest on loan taken for higher education";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "U/s 80G: Donation to approved funds and charitable institutions";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "U/s 80U: Deduction in respect of disabled persons.(Maximum Rs.50000/-, in case of severe disabilities Rs.75000/-)";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             Fpspread2.Sheets[0].RowCount++;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "SUM";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString("");
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                             Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                             #endregion
                             */
                            #endregion
                            #region Footer Calculation
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "TAXABLE INCOME";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(GrossSalary);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                            double RemainAmount = 0;
                            double TotalSalaryAmount = GrossSalary;
                            double FromRange = 0;
                            double ToRange = 0;
                            double BindAmount = 0;
                            double TotalTaxableAmount = 0;
                            #region RangeCalculation
                            if (ds1.Tables.Count > 3)
                            {
                                for (int intd = 0; intd < ds1.Tables[4].Rows.Count; intd++)
                                {
                                    Fpspread2.Sheets[0].RowCount++;
                                    string Bindvalue = "From " + Convert.ToString(ds1.Tables[4].Rows[intd]["FromRange"]) + " To " + Convert.ToString(ds1.Tables[4].Rows[intd]["ToRange"]);
                                    double.TryParse(Convert.ToString(ds1.Tables[4].Rows[intd]["FromRange"]), out FromRange);
                                    double.TryParse(Convert.ToString(ds1.Tables[4].Rows[intd]["ToRange"]), out ToRange);
                                    string Mode = Convert.ToString(ds1.Tables[4].Rows[intd]["mode"]);
                                    string CalCAmount = Convert.ToString(ds1.Tables[4].Rows[intd]["Amount"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Bindvalue.ToString();
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                    if (FromRange < GrossSalary && ToRange < GrossSalary)
                                    {
                                        BindAmount = ToRange - FromRange;
                                        BindAmount += 1;
                                    }
                                    else if (FromRange < GrossSalary && ToRange > GrossSalary)
                                    {
                                        BindAmount = GrossSalary - FromRange;
                                        BindAmount += 1;
                                    }
                                    else
                                    {
                                        BindAmount = 0;
                                    }
                                    double CalCValueAmount = 0;
                                    if (Mode.Trim() == "0" || Mode.Trim() == "False")
                                    {
                                        double.TryParse(CalCAmount, out  CalCValueAmount);
                                    }
                                    else if (Mode.Trim() == "1" || Mode.Trim() == "True")
                                    {
                                        CalCValueAmount = (BindAmount / 100) * Convert.ToDouble(CalCAmount);
                                    }
                                    TotalTaxableAmount += CalCValueAmount;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(BindAmount);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(CalCValueAmount));
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                                }
                            }
                            #endregion
                            double FinalTaxableincome = 0;
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "TAX PAYABLE ON TAXABLE INCOME";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(TotalTaxableAmount));
                            FinalTaxableincome += TotalTaxableAmount;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                            //barath 11.10.17
                            double RebateAmount = 0;
                            double RebateDeductAmt = 0;
                            double RebateDeductAmount = 0;
                            string rebateAmt = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='RebateDeductAmount' and college_code='" + ddlcollege.SelectedItem.Value + "'");
                            string[] Rebate = rebateAmt.Split('-');
                            if (Rebate.Length == 2)
                            {
                                double.TryParse(Convert.ToString(Rebate[0]), out RebateDeductAmt);
                                double.TryParse(Convert.ToString(Rebate[1]), out RebateDeductAmount);
                            }
                            //if (cbRabate.Checked)
                            //    double.TryParse(Convert.ToString(txtRebate.Text), out  RebateAmount);
                            if (TotalSalaryAmount < RebateDeductAmt)//500000FinalTaxableincome //changed from FinalTaxableincome to TotalSalaryAmount by poo 12.12.17
                                RebateAmount = RebateDeductAmount; //5000
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Less: Rebate u/s 86, 89, 90 or 91";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(RebateAmount));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                            FinalTaxableincome -= RebateAmount;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(FinalTaxableincome));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                            Fpspread2.Sheets[0].RowCount++;
                            string cesspercent = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Educess' and college_code='" + ddlcollege.SelectedItem.Value + "'");

                            int cessval = 0;
                            if (cesspercent != "" || cesspercent != "0")
                            {

                                cessval = Convert.ToInt32(cesspercent);
                            }
                            else
                            {
                                cessval = 3;
                            }
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Education Cess @ " + cesspercent + "% on Net Tax Payable";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";

                            // double TaxAmount = (FinalTaxableincome / 100) * 3;//TotalTaxableAmount
                            double TaxAmount = (FinalTaxableincome / 100) * cessval;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(TaxAmount));
                            FinalTaxableincome += TaxAmount;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Total Tax Payable";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(FinalTaxableincome));
                            ViewState["FinalTaxableincome"] = FinalTaxableincome;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                            //double RebateAmount = 0;
                            //Fpspread2.Sheets[0].RowCount++;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Less: Rebate u/s 86, 89, 90 or 91";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size =FontSize;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(RebateAmount));
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size =FontSize;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                            //FinalTaxableincome -= RebateAmount;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(FinalTaxableincome));
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size =FontSize;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                            #region TDS Tax Calculation
                            double TDSAmount = 0;
                            double CheckTds = 0;
                            if (ds1.Tables[5].Rows.Count > 0)
                            {
                                string TdsAmount = Convert.ToString(ds1.Tables[5].Compute("sum(TotalAmount)", ""));
                                double.TryParse(TdsAmount, out CheckTds);
                                TDSAmount += CheckTds;
                            }
                            if (ds1.Tables[6].Rows.Count > 0)
                            {
                                for (int intTds = 0; intTds < ds1.Tables[6].Rows.Count; intTds++)
                                {
                                    string Iscommon = Convert.ToString(ds1.Tables[6].Rows[intTds]["ITCommon"]);
                                    string iscommonvalue = Convert.ToString(ds1.Tables[6].Rows[intTds]["ITCommonValue"]);
                                    if (Iscommon.Trim() == "1" || Iscommon.Trim() == "True")
                                    {
                                        if (DeductionHash.ContainsKey(iscommonvalue.Trim()))
                                        {
                                            string GetCommonValue = Convert.ToString(DeductionHash[iscommonvalue.Trim()]);
                                            double.TryParse(GetCommonValue, out CheckTds);
                                            TDSAmount += CheckTds;
                                        }
                                    }
                                }
                            }
                            Fpspread2.Sheets[0].RowCount++;
                            double ProFxTax = TDSAmount;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Less: Prepaid Taxes (Advance Tax, TDS)";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Math.Round(ProFxTax));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(FinalTaxableincome));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                            #endregion
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Balance of Income Tax to be deducted";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            FinalTaxableincome -= ProFxTax;
                            if (reinvestment != 0 && FinalTaxableincome < 0)//delsi2509
                            {
                                FinalTaxableincome = reinvestment + FinalTaxableincome;
                            }
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(FinalTaxableincome));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "CERTIFICATE";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 5);
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "1. Certified that I am occupying rental house and paying monthly rent of Rs. " + HouseRentAmount + "";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 4);
                            Fpspread2.Sheets[0].RowCount++;
                            double LICAMT = 0;
                            if (DeductionHash.ContainsKey("LIC"))
                                double.TryParse(Convert.ToString(DeductionHash["LIC"]), out LICAMT);
                            else if (DeductionHash.ContainsKey("LIFE INSURANCE"))
                                double.TryParse(Convert.ToString(DeductionHash["LIFE INSURANCE"]), out LICAMT);
                            else if (DeductionHash.ContainsKey("LIFE INSURANCE(LIC)"))
                                double.TryParse(Convert.ToString(DeductionHash["LIFE INSURANCE(LIC)"]), out LICAMT);
                            LicAmt += LICAMT;
                            string licAmount = (LicAmt == 0) ? "" : Convert.ToString(LicAmt);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "2. Certified that I am paying a sum of Rs. " + licAmount + "  Towards LIC premium and the policies are kept alive ";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 4);
                            //Fpspread2.Sheets[0].RowCount++;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Towards LIC premium and the policies are kept alive ";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 4);
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 4);

                            Fpspread2.Sheets[0].RowCount++;//17.01.18
                            Fpspread2.Sheets[0].RowCount++;//17.01.18
                            Fpspread2.Sheets[0].RowCount++;//17.01.18
                            Fpspread2.Sheets[0].RowCount++;//17.01.18
                            Fpspread2.Sheets[0].RowCount++;//17.01.18
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Signature of the staff";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";

                            Fpspread2.Sheets[0].RowCount++;//17.01.18
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Name          : " + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";


                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Principal and Secretary";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 2, 1, 3);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                            Fpspread2.Sheets[0].RowCount++;//17.01.18
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = "Designation : " + Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text); ;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontSize;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 3, 1, 3);
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";


                            // Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(d2.GetFunction("select collname from collinfo where college_code ='" + ddlcollege.SelectedValue + "'"));
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 10;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 2, 1, 4);
                            //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 1, 1, 2); // poo 12.12.17
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = "Designation";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].ForeColor = Color.DarkViolet;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            //Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 2, 1, 3);
                            Fpspread2.Sheets[0].RowCount++;
                            //FarPoint.Web.Spread.IntegerCellType IntegerCellType = new FarPoint.Web.Spread.IntegerCellType();
                            FarPoint.Web.Spread.TextCellType TextCellType = new FarPoint.Web.Spread.TextCellType();
                            TextCellType.AllowWrap = true;
                            TextCellType.CellCssClass = "FpreadtextAlign";
                            TextCellType.CssClass = "FpreadtextAlign";

                            //bool resultOfConversion = Int32.TryParse(sNumber, numStyle, culture, out retValue);
                            //IntegerCellType.NumberFormat.NumberDecimalDigits = NumberStyles.Number; ; // NumberStyles.Number;
                            //Fpspread2.Columns[0].Width = 30;
                            //Fpspread2.Columns[1].Width = 400;
                            //Fpspread2.Columns[2].Width = 50;
                            //Fpspread2.Columns[3].Width = 100;

                            FarPoint.Web.Spread.IntegerCellType IntegerCellType = new FarPoint.Web.Spread.IntegerCellType();
                            IntegerCellType.NumberFormat = new System.Globalization.NumberFormatInfo();
                            IntegerCellType.NumberFormat.NumberGroupSeparator = "";
                            IntegerCellType.NumberFormat.NegativeSign = "-";

                            System.Globalization.CultureInfo modCulture = new System.Globalization.CultureInfo("en-US", false);
                            NumberFormatInfo number = modCulture.NumberFormat;
                            //IntegerCellType.NumberFormat.CurrencyNegativePattern = 1;
                            //IntegerCellType.NumberFormat.CurrencyGroupSeparator = "";
                            //IntegerCellType.NumberFormat.CurrencyDecimalDigits = 2;
                            //IntegerCellType.NumberFormat.GetFormat("@";

                            //IntegerCellType.NumberFormat.CurrencySymbol = "VND";
                            //ict.EditMode.NumberFormat = new System.Globalization.NumberFormatInfo();
                            //ict.EditMode.NumberFormat.NegativeSign = "@";
                            //Fpspread2.Sheets[0].Cells[3, 1].CellType = ict;
                            // Fpspread2.Sheets[0].Cells[3, 1].Value = -12;


                            //cells["B3"].Value = "NumberFormat";
                            //cells["C3"].Value = 1234;
                            //cells["C3"].Style.NumberFormat = "#.##0,00 [$Krakozhian Money Units]";

                            for (int row = 0; row < Fpspread2.Sheets[0].RowCount; row++)
                            {
                                for (int col = 0; col < Fpspread2.Sheets[0].ColumnCount; col++)
                                {
                                    Fpspread2.Sheets[0].Cells[row, col].Font.Name = "Arial";
                                    if (col == 2 && row > 8 && row <= Fpspread2.Sheets[0].RowCount - 10)
                                    {
                                        Fpspread2.Sheets[0].Cells[row, col].Text = ":";
                                        Fpspread2.Sheets[0].Cells[row, col].Font.Bold = true;
                                        Fpspread2.Sheets[0].Cells[row, col].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread2.Sheets[0].Cells[row, col].VerticalAlign = VerticalAlign.Middle;
                                    }
                                }
                                Fpspread2.Sheets[0].Cells[row, 1].Column.Width = 400;
                                Fpspread2.Sheets[0].Cells[row, 1].CellType = TextCellType;
                                Fpspread2.Sheets[0].Cells[row, 2].CellType = TextCellType;

                                Fpspread2.Sheets[0].Cells[row, 3].CellType = TextCellType;//IntegerCellType
                                //Fpspread2.Sheets[0].Cells[row, 3].Formatter.Format("#####");// = "NumberFormat";
                                Fpspread2.Sheets[0].Cells[row, 4].CellType = TextCellType;// IntegerCellType;
                            }
                            #endregion
                        }
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.SaveChanges();
                        Fpspread2.Visible = true;
                        rptprint.Visible = true;
                        btnprintmaster.Visible = true;
                        lbl_error.Visible = false;
                        btnprintcell.Visible = false;
                        spread2div.Visible = true;
                        //btnprintcell.Visible = true;
                    }
                    else
                    {
                        #region Fromat II
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "GROSS SALARY INCOME";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Convert.ToDouble(IncomeSalary) - lessval);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        double grosspay = Convert.ToDouble(ActualBasicAmount) - lessval;//IncomeSalary
                        //double lowest = Convert.ToDouble((HRA_amt < salaryDAten) ? (HRA_amt < salaryfivty ? HRA_amt : salaryfivty) : (salaryDAten < salaryfivty ? salaryDAten : salaryfivty));
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Addition Income Details";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Green;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                        q1 = "select textval,AllowDedAmount from ITAddAllowDedDetails s,textvaltable t where Staff_Code='" + staffcode + "' and IsAllow='1' and s.AllowDedDesc=t.TextCode and TextCriteria='IncHe'";
                        q1 = q1 + " select ITHeaderName,textval,AllowDedAmount,ITHeaderFK,ITMaxAmount  from ITAddAllowDedDetails s,textvaltable t,ITHeaderSettings h where Staff_Code='" + staffcode + "' and IsAllow='0' and s.AllowDedDesc=t.TextCode and TextCriteria='DedAd' and h.ITHeaderID=s.ITHeaderFK ";
                        q1 = q1 + " select ITHeaderID,ITHeaderName,ITMaxAmount from ITHeaderSettings ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(q1, "Text");
                        double sumofaddincome = 0;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[i]["textval"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["AllowDedAmount"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                string ins = Convert.ToString(ds.Tables[0].Rows[i]["AllowDedAmount"]);
                                if (ins.Trim() == "")
                                {
                                    ins = "0";
                                }
                                sumofaddincome = sumofaddincome + Convert.ToDouble(ins);
                            }
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Total Addition Income";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(sumofaddincome);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(grosspay + sumofaddincome);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                        }
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Addition Deduction Details";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Green;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                        double addsumofduction = 0; double additionaldeducttotal = 0;
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                                {
                                    addsumofduction = 0;
                                    DataView dv = new DataView();
                                    ds.Tables[1].DefaultView.RowFilter = "ITHeaderFK='" + Convert.ToString(ds.Tables[2].Rows[j]["ITHeaderID"]) + "'";
                                    dv = ds.Tables[1].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        Fpspread2.Sheets[0].RowCount++;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[2].Rows[j]["ITHeaderName"]);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        for (i = 0; i < dv.Count; i++)
                                        {
                                            Fpspread2.Sheets[0].RowCount++;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[i]["textval"]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[i]["AllowDedAmount"]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                            string duc = Convert.ToString(dv[i]["AllowDedAmount"]);
                                            if (duc.Trim() == "")
                                            {
                                                duc = "0";
                                            }
                                            addsumofduction = addsumofduction + Convert.ToDouble(duc);
                                        }
                                        Fpspread2.Sheets[0].RowCount++;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Total Addition Deduction";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(addsumofduction);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                        Fpspread2.Sheets[0].RowCount++;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Maximum Amount Deduction";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[2].Rows[j]["ITMaxAmount"]);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                        string itmax = Convert.ToString(ds.Tables[2].Rows[j]["ITMaxAmount"]);
                                        if (itmax == "")
                                        { itmax = "0"; } string maxdeduct = "";
                                        if (addsumofduction > Convert.ToDouble(itmax))
                                        {
                                            maxdeduct = itmax;
                                        }
                                        else
                                        {
                                            maxdeduct = Convert.ToString(addsumofduction);
                                        }
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(maxdeduct);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                                        additionaldeducttotal = additionaldeducttotal + Convert.ToDouble(maxdeduct);
                                    }
                                }
                                Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Total Deduction Amount";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(additionaldeducttotal);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                            }
                        }
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "TAXABLE INCOME";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(grosspay - additionaldeducttotal);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                        double taxableincome = grosspay - additionaldeducttotal;
                        double myTaxincome = taxableincome;
                        ds.Clear();
                        string getgender = "";
                        string getappno = d2.GetFunction("select appl_no from staffmaster where staff_code='" + staffcode + "'");
                        if (getappno.Trim() != "" && getappno.Trim() != "0")
                        {
                            getgender = d2.GetFunction("select sex from staff_appl_master where appl_no='" + getappno + "'");
                        }
                        q1 = "select convert(float, fromrange)fromrange,convert(float,torange)torange,convert(float,amount)amount,mode from HR_ITCalculationSettings where collegeCode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and sex='" + getgender + "' order by fromrange";
                        ds = d2.select_method_wo_parameter(q1, "Text");
                        double sumoftax = 0;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = " IncomeTax Salary Range ";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Green;
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "From Rs " + Convert.ToString(ds.Tables[0].Rows[i]["fromrange"]) + " To Rs " + Convert.ToString(ds.Tables[0].Rows[i]["torange"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                int mode = Convert.ToInt32(ds.Tables[0].Rows[i]["mode"]);
                                if (mode == 0)
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["amount"]);
                                }
                                if (mode == 1)
                                {
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["amount"]) + " %";
                                }
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = 12;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Arial";
                                double famt = Convert.ToDouble(ds.Tables[0].Rows[i]["fromrange"]);
                                double tamt = Convert.ToDouble(ds.Tables[0].Rows[i]["torange"]);
                                double amt = Convert.ToDouble(ds.Tables[0].Rows[i]["amount"]);
                                if (mode == 0)
                                {
                                    //if (taxableincome >= famt)
                                    //{
                                    //    if (taxableincome >= tamt)
                                    //    {
                                    if (famt <= myTaxincome && myTaxincome <= tamt)
                                    {
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(amt);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                                        sumoftax = sumoftax + amt;
                                    }
                                    //}
                                    //else
                                    //{
                                    //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(amt);
                                    //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                    //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                                    //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                                    //    sumoftax = sumoftax + amt;
                                    //}
                                    //}
                                }
                                if (mode == 1)
                                {
                                    double peramt = myTaxincome * amt / 100;
                                    //if (peramt >= famt)
                                    //{
                                    //    if (peramt >= tamt)
                                    //    {
                                    if (famt <= myTaxincome && myTaxincome <= tamt)
                                    {
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(peramt);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                                        sumoftax = sumoftax + peramt;
                                    }
                                    //myTaxincome = myTaxincome - tamt;
                                    //    }
                                    //    else
                                    //    {
                                    //        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(peramt);
                                    //        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                    //        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                                    //        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                                    //        sumoftax = sumoftax + peramt;
                                    //    }
                                    //}
                                }
                            }
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Total Tax";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Green;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(sumoftax);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Payable Tax 3% ";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkViolet;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString((sumoftax * 3) / 100);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 4);
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Total Payable Tax";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkRed;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(sumoftax + (sumoftax * 3) / 100);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkRed;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            if (radFormat.Items[0].Selected == true)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].SpanModel.Add(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 4);
                            }
                            else if (radFormat.Items[1].Selected == true)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "Tax Amount Per Month";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = 12;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.DarkRed;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                if (TotMonths > 0)
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString((sumoftax + (sumoftax * 3) / 100) / TotMonths);
                                else
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = "0";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = 12;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].ForeColor = Color.DarkRed;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            }
                        }
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.Visible = true;
                        rptprint.Visible = true;
                        btnprintmaster.Visible = false;
                        lbl_error.Visible = false;
                        #endregion
                    }
                }
                else
                {
                    Fpspread2.Visible = false;
                    rptprint.Visible = false;
                    btnprintmaster.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Founds";
                }
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void btnsavetax_Click(object sender, EventArgs e)
    {
        try
        {
            string actrow = Convert.ToString(Fpspread1.ActiveSheetView.ActiveRow);
            string actcol = Convert.ToString(Fpspread1.ActiveSheetView.ActiveColumn);
            string staffcode = "";
            if (actrow.Trim() != "")
            {
                staffcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
            }
            if (Fpspread2.Visible == true)
            {
                string taxPerMon = Convert.ToString(Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text);
                string gettaxamnt = Convert.ToString(Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 13, 4].Text);
                string asstyear = Convert.ToString(Fpspread2.Sheets[0].Cells[2, 0].Tag);
                //ViewState["FinalTaxableincome"]
                string insq = "";
                if (radFormat.Items[0].Selected == true)
                {
                    insq = "if exists (select * from StaffTaxDetails where Staff_Code='" + staffcode + "' and Asst_Year='" + asstyear + "') update StaffTaxDetails set TaxAmount='" + gettaxamnt + "' where Staff_Code='" + staffcode + "' and Asst_Year='" + asstyear + "' else insert into StaffTaxDetails (Staff_Code,Asst_Year,TaxAmount) Values ('" + staffcode + "','" + asstyear + "','" + gettaxamnt + "')";
                }
                else if (radFormat.Items[1].Selected == true)
                {
                    gettaxamnt = Convert.ToString(Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 2, 3].Text);
                    insq = "if exists (select * from StaffTaxDetails where Staff_Code='" + staffcode + "' and Asst_Year='" + asstyear + "') update StaffTaxDetails set TaxAmount='" + gettaxamnt + "',TaxPerMonth='" + taxPerMon + "' where Staff_Code='" + staffcode + "' and Asst_Year='" + asstyear + "' else insert into StaffTaxDetails (Staff_Code,Asst_Year,TaxAmount,TaxPerMonth) Values ('" + staffcode + "','" + asstyear + "','" + gettaxamnt + "','" + taxPerMon + "')";
                }
                int upscount = d2.update_method_wo_parameter(insq, "Text");
                if (upscount > 0)
                {
                    alertmessage.Visible = true;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Tax Details Saved Successfully!";
                }
            }
        }
        catch { }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            //string reportname = txtexcelname.Text;
            //if (appPath != "")
            //{
            //    string date = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_tt");
            //    strexcelname = reportname.ToString().Trim() + '_' + date.Trim();
            //    appPath = appPath.Replace("\\", "/");
            //    if (strexcelname != "")
            //    {
            //        print = strexcelname;
            //        string szPath = appPath + "/Report/";
            //        string szFile = print + ".xls";
            //        Fpspread2.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
            //        System.Web.HttpContext.Current.Response.Clear();
            //        System.Web.HttpContext.Current.Response.ClearHeaders();
            //        System.Web.HttpContext.Current.Response.ClearContent();
            //        System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
            //        System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //        System.Web.HttpContext.Current.Response.Flush();
            //        System.Web.HttpContext.Current.Response.WriteFile(szPath + szFile);
            //    }
            //}
            string reportname = txtexcelname.Text;
            if (filters_tbl.Visible == true)
            {
                if (reportname.ToString().Trim() != "")
                {
                    d2.printexcelreport(Fpspread3, reportname);
                    lblvalidation1.Visible = false;
                }
                else
                {
                    lblvalidation1.Text = "Please Enter Your Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
            }
            else
            {
                if (reportname.ToString().Trim() != "")
                {
                    d2.printexcelreport(Fpspread2, reportname);
                    lblvalidation1.Visible = false;
                }
                else
                {
                    lblvalidation1.Text = "Please Enter Your Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            if (filters_tbl.Visible == true)
            {
                string degreedetails = Convert.ToString(ViewState["CumlativeHeader"]);// "INCOME TAX CALCULATION STATEMENT";
                string pagename = "Incometaxcalculation_report.aspx";
                Printcontrol.loadspreaddetails(Fpspread3, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else
            {
                string degreedetails = "";//"INCOME TAX CALCULATION STATEMENT";
                string pagename = "Incometaxcalculation_report.aspx";
                Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    /* add by poomalar */
    protected void btnprintcell_click(object sender, EventArgs e)
    {
        //individualdiv.Visible = true;
        ScriptManager.RegisterStartupScript(this, GetType(), "btnprintcell", "PrtDiv();", true);
    }
    protected void cbl_deduction_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_deduction, cbl_deduction, txt_deduction, "Deduction");
    }
    protected void cb_deduction_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_deduction, cbl_deduction, txt_deduction, "Deduction");
    }
    protected void cb_allowancemultiple_checkedchange(object sender, EventArgs e)
    {
        chkchange(cb_allowancemultiple, cbl_allowancemultiple, txt_allowancemultiple, "Allowance");
    }
    protected void cbl_allowancemultiple_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_allowancemultiple, cbl_allowancemultiple, txt_allowancemultiple, "Allowance");
    }
    protected void bindallowance()/* modified by poomalar */
    {
        #region commented
        //try
        //{
        //    ds.Clear();
        //    cbl_allowancemultiple.Items.Clear();
        //    string item = "select allowances from incentives_master where college_code = '" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
        //    ds = d2.select_method_wo_parameter(item, "Text");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        cbl_allowancemultiple.DataSource = ds;
        //        string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
        //        string[] split = st.Split(';');
        //        for (int row = 0; row < split.Length - 1; row++)
        //        {
        //            string staff = split[row];
        //            string[] split1 = staff.Split('\\');
        //            string stafftype = split1[0];
        //            cbl_allowancemultiple.Items.Add(stafftype);
        //        }
        //        if (cbl_allowancemultiple.Items.Count > 0)
        //        {
        //            for (int i = 0; i < cbl_allowancemultiple.Items.Count; i++)
        //            {
        //                cbl_allowancemultiple.Items[i].Selected = true;
        //            }
        //            txt_allowancemultiple.Text = "Allowance (" + cbl_allowancemultiple.Items.Count + ")";
        //            cb_allowancemultiple.Checked = true;
        //        }
        //    }
        //    else
        //    {
        //        txt_allowancemultiple.Text = "--Select--";
        //        cb_allowancemultiple.Checked = false;
        //    }
        //}
        #endregion
        try
        {
            ds.Clear();
            cbl_allowancemultiple.Items.Clear();
            string item = "select Name,Description from ITcalculationAllowanceDeduction where collegeCode = '" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and Type='1'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_allowancemultiple.DataSource = ds;
                cbl_allowancemultiple.DataTextField = "Description";
                cbl_allowancemultiple.DataValueField = "Name";
                cbl_allowancemultiple.DataBind();
                if (cbl_allowancemultiple.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_allowancemultiple.Items.Count; i++)
                    {
                        cbl_allowancemultiple.Items[i].Selected = true;
                    }
                    txt_allowancemultiple.Text = "Allowance (" + cbl_allowancemultiple.Items.Count + ")";
                    cb_allowancemultiple.Checked = true;
                }
            }
            else
            {
                txt_allowancemultiple.Text = "--Select--";
                cb_allowancemultiple.Checked = false;
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected void binddeduction() /* modified by poomalar */
    {
        #region modified by poomalar
        //try
        //{
        //    ds.Clear();
        //    cbl_deduction.Items.Clear();
        //    string item = "select deductions from incentives_master  where college_code = '" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' ";
        //    ds = d2.select_method_wo_parameter(item, "Text");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        string st = Convert.ToString(ds.Tables[0].Rows[0]["deductions"]);
        //        string[] split = st.Split(';');
        //        for (int row = 0; row < split.Length - 1; row++)
        //        {
        //            string staff = split[row];
        //            string[] split1 = staff.Split('\\');
        //            string stafftype = split1[0];
        //            cbl_deduction.Items.Add(stafftype);
        //        }
        //        if (cbl_deduction.Items.Count > 0)
        //        {
        //            for (int i = 0; i < cbl_deduction.Items.Count; i++)
        //            {
        //                cbl_deduction.Items[i].Selected = true;
        //            }
        //            txt_deduction.Text = "Deduction (" + cbl_deduction.Items.Count + ")";
        //            cb_deduction.Checked = true;
        //        }
        //    }
        //    else
        //    {
        //        txt_deduction.Text = "--Select--";
        //        cb_deduction.Checked = false;
        //    }
        //}
        #endregion
        try
        {
            ds.Clear();
            cbl_deduction.Items.Clear();
            string item = "select Name,Description from ITcalculationAllowanceDeduction where collegeCode = '" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and Type='2'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_deduction.DataSource = ds;
                cbl_deduction.DataTextField = "Description";
                cbl_deduction.DataValueField = "Name";
                cbl_deduction.DataBind();
                if (cbl_deduction.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_deduction.Items.Count; i++)
                    {
                        cbl_deduction.Items[i].Selected = true;
                    }
                    txt_deduction.Text = "Deduction (" + cbl_deduction.Items.Count + ")";
                    cb_deduction.Checked = true;
                }
            }
            else
            {
                txt_deduction.Text = "--Select--";
                cb_deduction.Checked = false;
            }
        }
        catch (Exception ex)
        {
            alertmessage.Visible = true;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.ToString();
        }
    }
    protected string getmonth(int monthvalue)
    {
        string month = "";
        try
        {
            DateTime dt = new DateTime(2000, 1, 1);
            month = Convert.ToString(dt.AddMonths(monthvalue - 1).ToString("MMMM"));
        }
        catch { }
        return month;
    }
    protected int getmonthvalue(string monthvalue)
    {
        int i = 0;
        try
        {
            i = DateTime.ParseExact(monthvalue, "MMMM", CultureInfo.CurrentCulture).Month;
        }
        catch { }
        return i;
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
                }
                else
                {
                    for (int intF = 0; intF < Fpspread1.Sheets[0].Rows.Count; intF++)
                    {
                        Fpspread1.Sheets[0].Cells[intF, 1].Value = 0;
                    }
                }
            }
        }
        catch
        {
        }
    }
    #region From16 Print
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            StringBuilder SbHtml = new StringBuilder();
            for (int intF = 0; intF < Fpspread1.Sheets[0].Rows.Count; intF++)
            {
                if (Convert.ToInt32(Fpspread1.Sheets[0].Cells[intF, 1].Value) == 1)
                {
                    string StaffCode = Convert.ToString(Fpspread1.Sheets[0].Cells[intF, 3].Text);
                    string ApplId = Convert.ToString(Fpspread1.Sheets[0].Cells[intF, 2].Tag);
                    if (StaffCode.Trim() != "")
                    {
                        PrintForm16(StaffCode, SbHtml, ApplId);
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void PrintForm16(string StaffCode, StringBuilder SbHtml, string ApplId)
    {
        try
        {
            ds.Clear();
            double HouseRentAmount = 0;
            string frommonth = string.Empty;
            string fromyear = string.Empty;
            string tomonth = string.Empty;
            string toyear = string.Empty;
            string IncomeSalary = string.Empty;
            //25.09.17 Added
            double PayLastMonthAllowance = 0;
            double PayLastMonthDeduction = 0;
            int DiffenerceMonth = 0;
            double PayLastMonthSalary = 0;
            Hashtable PayLastMonthAllowanceHash = new Hashtable();
            Hashtable PayLastMonthDeductionHash = new Hashtable();
            bool CalculateAllMonthBool = false;
            int lastpayMonth = 0;
            int lastpayYear = 0;
            double lastGradePay = 0;
            double reinvestment = 0;
            string itsetting = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='IT Calculation Settings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'");
            if (itsetting.Trim() != "0")
            {
                string[] linkvalue = itsetting.Split('-');
                if (linkvalue.Length > 0)
                {
                    frommonth = linkvalue[0].Split(',')[0];
                    fromyear = linkvalue[0].Split(',')[1];
                    tomonth = linkvalue[1].Split(',')[0];
                    toyear = linkvalue[1].Split(',')[1];
                }
                IncomeSalary = d2.GetFunction(" select sum(netaddact)netaddact from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + StaffCode + "'");
                double.TryParse(d2.GetFunction(" select sum(Amount) as TotalAmount from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplId + "' and CollegeCode='" + ddlcollege.SelectedValue + "'"), out HouseRentAmount);


                double.TryParse(d2.GetFunction(" select sum(Amount) as TotalAmount from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='5' and ((ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "')) and staff_ApplID='" + ApplId + "' and CollegeCode='" + ddlcollege.SelectedValue + "'"), out reinvestment);//delsi 2509

                string CalculateAllSet = d2.GetFunction("select linkValue from New_InsSettings where LinkName='Form16 Calculate All Month'  and user_code ='" + usercode + "' and college_code ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'");//barath 25.09.17
                if (!string.IsNullOrEmpty(CalculateAllSet) && CalculateAllSet.Trim() != "0")
                {
                    CalculateAllMonthBool = true;
                    string CalculateMonthDetQuery = "select paymonth,payyear,netaddact,netadd,addd,deddd,convert(varchar(max), allowances)as allowances,convert(varchar(max),deductions)as deductions,bsalary,grade_pay from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime)  and staff_code = '" + StaffCode + "' group by payyear,paymonth,netaddact,netadd,addd,deddd,convert(varchar(max), allowances),convert(varchar(max),deductions),bsalary ,grade_pay order by year(payyear),year(paymonth) ";
                    CalculateMonthDetQuery += " select Amount,itmonth,ityear from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' ))  and CollegeCode='" + ddlcollege.SelectedValue + "' and staff_ApplID='" + ApplId + "' group by ityear,itmonth,Amount order by year(ityear),year(itmonth) ";
                    DataSet CalculateMonthDetDS = d2.select_method_wo_parameter(CalculateMonthDetQuery, "text");
                    if (CalculateMonthDetDS.Tables != null && CalculateMonthDetDS.Tables[0].Rows.Count > 0)
                    {
                        #region payProcessLastMonthSalary
                        double lastMonthSalary = 0;
                        double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["netaddact"]), out lastMonthSalary);
                        int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["paymonth"]), out lastpayMonth);
                        int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["payyear"]), out lastpayYear);
                        double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["addd"]), out PayLastMonthAllowance);
                        double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["deddd"]), out PayLastMonthDeduction);
                        double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[0].Rows[CalculateMonthDetDS.Tables[0].Rows.Count - 1]["grade_pay"]), out lastGradePay);
                        PayLastMonthAllowanceHash = PayProcessAllowanceDet(CalculateMonthDetDS, 0, CalculateMonthDetDS.Tables[0].Rows.Count - 1, ref PayLastMonthSalary);
                        PayLastMonthDeductionHash = PayProcessDeductionDet(CalculateMonthDetDS, 0, CalculateMonthDetDS.Tables[0].Rows.Count - 1, ref PayLastMonthSalary);
                        DateTime FYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                        DateTime TYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                        DiffenerceMonth = (TYearDT.Month - FYearDT.Month) + 12 * (TYearDT.Year - FYearDT.Year);
                        double CurrentSalary = 0;
                        double.TryParse(IncomeSalary, out CurrentSalary);
                        CurrentSalary += lastMonthSalary * DiffenerceMonth;
                        IncomeSalary = Convert.ToString(CurrentSalary);
                        PayLastMonthSalary *= DiffenerceMonth;
                        lastGradePay *= DiffenerceMonth;
                        #endregion
                        #region ItCalculationLastMonthSalary
                        if (CalculateMonthDetDS.Tables[1].Rows.Count > 0)
                        {
                            double lastAllowanceAndDedutionAmt = 0;
                            int lastAllowanceAnddedutionMonth = 0;
                            int lastAllowanceAnddedutionyear = 0;
                            double.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[1].Rows[CalculateMonthDetDS.Tables[1].Rows.Count - 1]["Amount"]), out lastAllowanceAndDedutionAmt);
                            int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[1].Rows[CalculateMonthDetDS.Tables[1].Rows.Count - 1]["itmonth"]), out lastAllowanceAnddedutionMonth);
                            int.TryParse(Convert.ToString(CalculateMonthDetDS.Tables[1].Rows[CalculateMonthDetDS.Tables[1].Rows.Count - 1]["ityear"]), out lastAllowanceAnddedutionyear);
                            FYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                            TYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                            int HouseRentDiffenerceMonth = (TYearDT.Month - FYearDT.Month) + 12 * (TYearDT.Year - FYearDT.Year);
                            //double HouseRentAmt = 0;
                            //double.TryParse(IncomeSalary, out HouseRentAmt);
                            //HouseRentAmt += lastMonthSalary * HouseRentDiffenerceMonth;
                            //HouseRentAmount = Convert.ToString(HouseRentAmt);
                            //double HouseRentAmt = 0;
                            //double prevHouseRentAmount = 0;
                            //HouseRentAmt += lastAllowanceAndDedutionAmt * HouseRentDiffenerceMonth;
                            //double.TryParse(HouseRentAmount, out prevHouseRentAmount);
                            //HouseRentAmount = Convert.ToString(prevHouseRentAmount + HouseRentAmt);
                        }
                        #endregion
                    }
                }
            }
            else
            {
                alertmessage.Visible = true;
                lbl_alerterror.Visible = true;
                lbl_alerterror.Text = "Please Set IT Calculation Settings";
                return;
            }
            string QueryCheck = " select LinkValue from New_InsSettings where LinkName='IT Calculation PAN / GIR' and college_code='" + ddlcollege.SelectedValue + "' ;  select LinkValue from New_InsSettings where LinkName='IT Calculation TAN' and college_code='" + ddlcollege.SelectedValue + "'";
            string PANNo = string.Empty;
            string TANNo = string.Empty;
            DataSet DnewSet = d2.select_method_wo_parameter(QueryCheck, "Text");
            if (DnewSet.Tables.Count > 1)
            {
                if (DnewSet.Tables[0].Rows.Count > 0)
                {
                    PANNo = Convert.ToString(DnewSet.Tables[0].Rows[0]["LinkValue"]);
                }
                if (DnewSet.Tables[0].Rows.Count > 0)
                {
                    TANNo = Convert.ToString(DnewSet.Tables[1].Rows[0]["LinkValue"]);
                }
            }
            Hashtable AllowanceHash = new Hashtable();
            Hashtable DeductionHash = new Hashtable();
            Hashtable MonthlypayDeductionHash = new Hashtable();
            Hashtable IncentiveMasterDeductionHash = new Hashtable();
            Hashtable otherTaxAmttaxHash = new Hashtable();
            double TotalBasicAmount = 0;
            double ActualBasicAmount = 0;
            double.TryParse(IncomeSalary, out ActualBasicAmount);
            double AdditionAllowance = 0;
            string professionalTaxSettings = string.Empty;
            string ptstmonth = string.Empty;
            string ptendmonth = string.Empty;
            string ptstyear = string.Empty;
            string ptendyear = string.Empty;
            double GradePayTotal = 0;


            DataSet chkotherallow = new DataSet();//delsi 2409
            string qur = " select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,checkotherallow from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType ='4' and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplId + "' and CollegeCode='" + ddlcollege.SelectedValue + "'  group by AllowdeductID,ITAllowDeductType,checkotherallow";

            chkotherallow = d2.select_method_wo_parameter(qur, "text");
            double totalotherallow = 0;
            if (chkotherallow.Tables[0].Rows.Count > 0)
            {
                for (int val = 0; val < chkotherallow.Tables[0].Rows.Count; val++)
                {
                    double getval = 0;
                    string DirectValue1 = Convert.ToString(chkotherallow.Tables[0].Rows[val]["TotalAmount"]);
                    double.TryParse(DirectValue1, out getval);
                    totalotherallow = totalotherallow + getval;

                }

            }

            double.TryParse(d2.GetFunction("select sum(a.AllowanceAmt)AllowanceAmt from AdditionalAllowanceAndDeduction a,CO_MasterValues m where m.MasterCode=AllowanceCode and m.MasterCriteria='additionalallowance' and a.CollegeCode=m.CollegeCode and a.CollegeCode='" + ddlcollege.SelectedValue + "' and a.staffcode='" + StaffCode + "'"), out AdditionAllowance);
            // ActualBasicAmount += AdditionAllowance;
            ActualBasicAmount += (AdditionAllowance + totalotherallow);
            string Query = " select sa.appl_no, s.staff_code,s.staff_name,d.desig_name,t.stftype,c.category_Name,h.dept_name,pangirnumber,upper(sa.sex)sex,sa.father_name from staffmaster s,staff_appl_master sa ,stafftrans t,hrdept_Master h,desig_Master d ,staffcategorizer c where sa.appl_no=s.appl_no and s.staff_code=t.staff_code and t.dept_code=h.dept_code and t.desig_code=d.desig_code and c.category_code=t.category_code and t.latestrec='1' and s.staff_code ='" + StaffCode + "'";
            Query += " select Principal,Collname,(convert(varchar(10), address1)+','+ convert(varchar(10),district)+' '+convert(varchar(10),pincode)) as Address from collinfo where college_code ='" + ddlcollege.SelectedItem.Value + "'";
            Query += " select allowances,deductions,bsalary,grade_pay from monthlypay where CAST(CONVERT(varchar(20),PayMonth)+'/01/'+CONVERT(varchar(20),PayYear) as Datetime) between CAST(CONVERT(varchar(20),'" + frommonth + "')+'/01/'+CONVERT(varchar(20),'" + fromyear + "') as Datetime) and CAST(CONVERT(varchar(20),'" + tomonth + "')+'/01/'+CONVERT(varchar(20),'" + toyear + "') as Datetime) and staff_code = '" + StaffCode + "'";
            Query += " select LinkValue from New_InsSettings where LinkName='Professional Tax Calculation Month' and college_code ='" + ddlcollege.SelectedItem.Value + "' and user_code ='" + usercode + "'";
            Query += " select deductions from incentives_master  where college_code = '" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables != null)
            {
                if (ds.Tables[3].Rows.Count > 0)
                {
                    professionalTaxSettings = Convert.ToString(ds.Tables[3].Rows[0]["LinkValue"]);
                    if (!string.IsNullOrEmpty(professionalTaxSettings.Trim()))
                    {
                        string[] det = professionalTaxSettings.Split(';');
                        string monthyear = det[0];
                        string endmonthyear = det[1];
                        ptstmonth = monthyear.Split('-')[0];
                        ptendmonth = endmonthyear.Split('-')[0];
                        ptstyear = monthyear.Split('-')[1];
                        ptendyear = endmonthyear.Split('-')[1];
                    }
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    string st = Convert.ToString(ds.Tables[4].Rows[0]["deductions"]);
                    string[] split = st.Split(';');
                    foreach (var item in split)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            string staff = item;
                            string[] split1 = staff.Split('\\');
                            string description = split1[0];
                            string apprivation = split1[1];
                            if (!IncentiveMasterDeductionHash.ContainsKey(description))
                                IncentiveMasterDeductionHash.Add(apprivation, description);
                        }
                    }
                }
            }
            if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
            {
                string Gender = Convert.ToString(ds.Tables[0].Rows[0]["sex"]);
                #region Allowance Deduction Calculation
                for (int intds = 0; intds < ds.Tables[2].Rows.Count; intds++)
                {
                    string AllowanceValue = Convert.ToString(ds.Tables[2].Rows[intds]["allowances"]);
                    string[] SplitFirst = AllowanceValue.Split('\\');
                    if (SplitFirst.Length > 0)
                    {
                        for (int intc = 0; intc < SplitFirst.Length; intc++)
                        {
                            if (SplitFirst[intc].Trim() != "")
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
                                    if (!AllowanceHash.ContainsKey(SecondSplit[0].Trim()))
                                    {
                                        AllowanceHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                                    }
                                    else
                                    {
                                        double GetValue = Convert.ToDouble(AllowanceHash[SecondSplit[0].Trim()]);
                                        GetValue = GetValue + AllowTaeknValue;
                                        AllowanceHash.Remove(SecondSplit[0].Trim());
                                        AllowanceHash.Add(SecondSplit[0].Trim(), GetValue);
                                    }
                                }
                            }
                        }
                    }
                    AllowanceValue = Convert.ToString(ds.Tables[2].Rows[intds]["deductions"]);
                    SplitFirst = AllowanceValue.Split('\\');
                    if (SplitFirst.Length > 0)
                    {
                        for (int intc = 0; intc < SplitFirst.Length; intc++)
                        {
                            if (SplitFirst[intc].Trim() != "")
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
                                    if (!DeductionHash.ContainsKey(SecondSplit[0].Trim()))
                                    {
                                        DeductionHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                                    }
                                    else
                                    {
                                        double GetValue = Convert.ToDouble(DeductionHash[SecondSplit[0].Trim()]);
                                        GetValue = GetValue + AllowTaeknValue;
                                        DeductionHash.Remove(SecondSplit[0].Trim());
                                        DeductionHash.Add(SecondSplit[0].Trim(), GetValue);
                                    }
                                }
                            }
                        }
                    }
                    TotalBasicAmount += Convert.ToDouble(ds.Tables[2].Rows[intds]["bsalary"]);
                    GradePayTotal += Convert.ToDouble(ds.Tables[2].Rows[intds]["grade_Pay"]);
                }
                //MonthlypayDeductionHash = DeductionHash;
                //DeductionHash.CopyTo(MonthlypayDeductionHash, 0);
                MonthlypayDeductionHash = (Hashtable)DeductionHash.Clone();
                #endregion
                #region allmonth Calculation  15.11.17 barath
                int Diffmonth = 0;
                if (lastpayYear != 0 && lastpayMonth != 0)
                {
                    GradePayTotal += lastGradePay;
                    DateTime FCalYearDT = new DateTime(Convert.ToInt32(lastpayYear), Convert.ToInt32(lastpayMonth), 28);
                    DateTime TCalYearDT = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                    Diffmonth = (TCalYearDT.Month - FCalYearDT.Month) + 12 * (TCalYearDT.Year - FCalYearDT.Year);
                    DateTime DummyDT = new DateTime();
                    DummyDT = FCalYearDT;
                    DummyDT = DummyDT.AddMonths(1);
                    TCalYearDT = TCalYearDT.AddMonths(1);
                    if (Diffmonth != 0)
                    {
                        double Amt = 0;
                        double Value = 0;
                        while (DummyDT < TCalYearDT)
                        {
                            #region last Month Allowance
                            foreach (DictionaryEntry dr in PayLastMonthAllowanceHash)
                            {
                                Amt = 0; Value = 0;
                                string AllowanceName = Convert.ToString(dr.Key).Trim();
                                double.TryParse(Convert.ToString(dr.Value), out Amt);
                                if (AllowanceHash.ContainsKey(AllowanceName.Trim()))
                                    double.TryParse(Convert.ToString(AllowanceHash[AllowanceName.Trim()]), out Value);
                                Value += Amt;
                                AllowanceHash[AllowanceName.Trim()] = Value;
                            }
                            #endregion
                            #region last Month Deduction
                            foreach (DictionaryEntry dr in PayLastMonthDeductionHash)
                            {
                                string DeductionName = Convert.ToString(dr.Key).Trim();
                                Amt = 0;
                                double.TryParse(Convert.ToString(dr.Value), out Amt);
                                Value = 0;
                                if (!DeductionHash.ContainsKey(DeductionName.Trim()))
                                {
                                    if (DeductionName.Trim().ToUpper() == "P.TAX" || DeductionName.Trim().ToUpper() == "P TAX" || DeductionName.Trim() == "PROFESSIONAL TAX" || DeductionName.Trim() == "PROFTAX")
                                    {
                                        if (DummyDT.ToString("MM").TrimStart('0') == ptstmonth && DummyDT.ToString("yyyy") == ptstyear || DummyDT.ToString("MM").TrimStart('0') == ptendmonth && DummyDT.ToString("yyyy") == ptendyear)
                                            DeductionHash.Add(DeductionName.Trim(), Amt);
                                        else
                                            DeductionHash.Add(DeductionName.Trim(), Amt);
                                    }
                                    else if (DeductionName.Trim().ToUpper() == "INC TAX" || DeductionName.Trim().ToUpper() == "I TAX" || DeductionName.Trim().ToUpper() == "INCOME TAX" || DeductionName.Trim().ToUpper() == "ITAX" || DeductionName.Trim().ToUpper() == "TDS")
                                    { }
                                }
                                else
                                {
                                    if (DeductionName.Trim().ToUpper() == "P.TAX" || DeductionName.Trim().ToUpper() == "P TAX" || DeductionName.Trim() == "PROFESSIONAL TAX" || DeductionName.Trim() == "PROFTAX" || DeductionName.Trim()=="MISC")
                                    {
                                        if (DummyDT.ToString("MM").TrimStart('0') == ptstmonth && DummyDT.ToString("yyyy") == ptstyear || DummyDT.ToString("MM").TrimStart('0') == ptendmonth && DummyDT.ToString("yyyy") == ptendyear)
                                        {
                                            double.TryParse(Convert.ToString(DeductionHash[DeductionName.Trim()]), out Value);
                                            Value += Amt;
                                            DeductionHash[DeductionName.Trim()] = Value;
                                        }
                                    }
                                    else if (DeductionName.Trim().ToUpper() == "INC TAX" || DeductionName.Trim().ToUpper() == "I TAX" || DeductionName.Trim().ToUpper() == "INCOME TAX" || DeductionName.Trim().ToUpper() == "ITAX" || DeductionName.Trim().ToUpper() == "TDS")
                                    { }
                                    else
                                    {
                                        double.TryParse(Convert.ToString(DeductionHash[DeductionName.Trim()]), out Value);
                                        Value += Amt;
                                        DeductionHash[DeductionName.Trim()] = Value;
                                    }
                                }
                            }
                            #endregion
                            DummyDT = DummyDT.AddMonths(1);
                        }
                    }
                }
                #endregion
                #region Addition Deduction barath 07.12.17
                string query = "select AllowanceDeductAmt,mastercriteriavalue2,mastercriteriavalue3,mastercriteriavalue4 from AdditionalAllowanceAndDeduction a,CO_MasterValues m where m.MasterCode=AllowanceCode and m.MasterCriteria='additionalallowance' and a.CollegeCode=m.CollegeCode and a.CollegeCode='" + ddlcollege.SelectedValue + "' and a.staffcode='" + StaffCode + "'";
                DataSet deductionDS = new DataSet();
                deductionDS = d2.select_method_wo_parameter(query, "Text");
                if (deductionDS.Tables[0].Rows.Count > 0)
                {
                    for (int ded = 0; ded < deductionDS.Tables[0].Rows.Count; ded++)
                    {
                        string splded = Convert.ToString(deductionDS.Tables[0].Rows[ded]["AllowanceDeductAmt"]);
                        string[] spldedname = splded.Split(';'); double dedvalue = 0;
                        if (spldedname.Length > 0)
                        {
                            for (int spld = 0; spld < spldedname.Length; spld++)
                            {
                                if (spldedname[spld].Contains('-'))
                                {
                                    double value = 0;
                                    string dednameadd = spldedname[spld].Split('-')[0]; //splded.Split(';')[spld].Split('-')[0];
                                    string dedvalueadd = spldedname[spld].Split('-')[1]; //splded.Split(';')[spld].Split('-')[1];
                                    double.TryParse(dedvalueadd, out dedvalue);
                                    if (!DeductionHash.ContainsKey(dednameadd))
                                        DeductionHash.Add(dednameadd, dedvalue);
                                    else
                                    {
                                        value = 0;
                                        double.TryParse(Convert.ToString(DeductionHash[dednameadd]), out value);
                                        value += dedvalue;
                                        DeductionHash[dednameadd] = value;
                                    }
                                    //21.12.17
                                    if (!otherTaxAmttaxHash.ContainsKey(dednameadd))
                                        otherTaxAmttaxHash.Add(dednameadd, dedvalue);
                                    else
                                    {
                                        value = 0;
                                        double.TryParse(Convert.ToString(otherTaxAmttaxHash[dednameadd]), out value);
                                        value += dedvalue;
                                        otherTaxAmttaxHash[dednameadd] = value;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                double HouseRent = 0;
                //double TotalHouseRentAmount = 0;
                double TotalHRA = 0;
                double DAAmount = 0;
                double PercentHouseRent = 0;
                double RentPaidAmount = 0;
                double HalfPercentofActualSalary = 0;

                string SalaryDeductHouseRentName = d2.GetFunction(" select distinct CommonDuduction from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType='3' and CommonDuduction is not NULL ");//barath 23.01.18
                if (!string.IsNullOrEmpty(SalaryDeductHouseRentName))
                {
                    double SalaryDeductHouseRent = 0;
                    if (DeductionHash.ContainsKey(SalaryDeductHouseRentName))
                        double.TryParse(Convert.ToString(DeductionHash[SalaryDeductHouseRentName]), out SalaryDeductHouseRent);
                    HouseRentAmount += SalaryDeductHouseRent;
                }
                if (HouseRentAmount != 0 && TotalBasicAmount != 0)
                {
                    string Distict = d2.GetFunction("  select MasterValue from staff_appl_master sa,Staffmaster s ,co_mastervalues c   where s.appl_no=sa.appl_no and convert(varchar(max), c.MasterCode)=isnull(Pdistrict,0) and staff_code ='" + StaffCode + "'");
                    //double.TryParse(HouseRentAmount, out TotalHouseRentAmount);
                    if (AllowanceHash.ContainsKey("HRA"))
                    {
                        TotalHRA = Convert.ToDouble(AllowanceHash["HRA"]);
                        //if (CalculateAllMonthBool)//28.09.17 bb
                        //{
                        //    double LastMonthHRA = 0;
                        //    double.TryParse(Convert.ToString(PayLastMonthAllowanceHash["HRA"]), out LastMonthHRA);
                        //    TotalHRA += LastMonthHRA;
                        //}
                    }
                    if (AllowanceHash.ContainsKey("DA"))
                    {
                        DAAmount = Convert.ToDouble(AllowanceHash["DA"]);
                        //if (CalculateAllMonthBool)//28.09.17 bb
                        //{
                        //    double LastMonthDA = 0;
                        //    double.TryParse(Convert.ToString(PayLastMonthAllowanceHash["DA"]), out LastMonthDA);
                        //    DAAmount += LastMonthDA;
                        //}
                    }
                    if (CalculateAllMonthBool)//28.09.17 bb
                        TotalBasicAmount += PayLastMonthSalary;
                    PercentHouseRent = (TotalBasicAmount + GradePayTotal + DAAmount) * 10 / 100;
                    if (PercentHouseRent > HouseRentAmount)
                    {
                        RentPaidAmount = PercentHouseRent - HouseRentAmount;
                    }
                    else
                    {
                        RentPaidAmount = HouseRentAmount - PercentHouseRent;
                    }
                    if (Distict.Trim().ToLower() == "chennai" || Distict.Trim().ToLower() == "mumbai" || Distict.Trim().ToLower() == "calcutta" || Distict.Trim().ToLower() == "delhi")
                    {
                        HalfPercentofActualSalary = (ActualBasicAmount) / 100 * 50;
                    }
                    else
                    {
                        HalfPercentofActualSalary = (ActualBasicAmount) / 100 * 40;
                    }
                    if (TotalHRA < RentPaidAmount && TotalHRA < HalfPercentofActualSalary)
                    {
                        HouseRent = TotalHRA;
                    }
                    else if (RentPaidAmount < TotalHRA && RentPaidAmount < HalfPercentofActualSalary)
                    {
                        HouseRent = RentPaidAmount;
                    }
                    else if (HalfPercentofActualSalary < RentPaidAmount && HalfPercentofActualSalary < TotalHRA)
                    {
                        HouseRent = HalfPercentofActualSalary;
                    }
                }
                string selHouseRent = d2.GetFunction("select value from Master_Settings where settings='ViewHouseRent' and usercode='" + usercode + "'");//delsi0107
                double houserentval = 0;
                if (selHouseRent.Trim() != "" && selHouseRent.Trim() != "0" && selHouseRent.Trim() == "1")
                {
                    DataSet houserentcheck = new DataSet();
                    string qry = "select ITGroupPK,GroupName,GroupDesc,MaxLimitAmount from IT_GroupMaster where parentCode='0' and collegeCode='" + ddlcollege.SelectedValue + "' order by isnull(Priority,10000) asc";
                    qry = qry + " select ITGroupPK,GroupName,GroupDesc,ParentCode,ITGroupType,IT_IDFK,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue,IsAgeRange,MaxValue,MinValue from IT_GroupMaster IT, IT_GroupMapping IM,IT_OtherAllowanceDeducation AD where IT.ITGroupPK=IM.ITGroupFK and AD.IT_ID=IM.IT_IDFK and IT.CollegeCode='" + ddlcollege.SelectedValue + "'";
                    qry += " select distinct ITGroupPK,GroupName,GroupDesc,MaxLimitAmount,parentCode,isnull(Priority,10000) from IT_GroupMaster IT,IT_GroupMapping IM where IT.ITGroupPk=IM.ITGroupFK and collegeCode='" + ddlcollege.SelectedValue + "' order by isnull(Priority,10000) asc";
                    qry += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType in   (1,2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplId + "' and CollegeCode='" + ddlcollege.SelectedValue + "'  group by AllowdeductID,ITAllowDeductType,percentage";
                    qry += " select convert(bigint ,round(FromRange,0)) FromRange,convert(bigint ,round (ToRange,0)) ToRange,Amount,mode  from HR_ITCalculationSettings where collegeCode='" + ddlcollege.SelectedValue + "' and sex ='" + Gender.Trim() + "'";
                    qry += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details ID,IT_OtherAllowanceDeducation IA where ID.AllowDeductID=IA.IT_ID and ITAllowdeductType in (2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplId + "' and IA.CollegeCode='" + ddlcollege.SelectedValue + "' and isnull(IsIncomeTax,'0')='1'  group by AllowdeductID,ITAllowDeductType,percentage";
                    qry += " select IT_ID,ITCommon,ITCommonValue,ITType from IT_OtherAllowanceDeducation  where  isnull(IsIncomeTax,'0')='1'  and CollegeCode='" + ddlcollege.SelectedValue + "'";
                    houserentcheck.Clear();
                    houserentcheck = d2.select_method_wo_parameter(qry, "text");

                    DataView dvnew1 = new DataView();
                    DataView dvh = new DataView();
                    if (houserentcheck.Tables.Count > 1 && houserentcheck.Tables[0].Rows.Count > 0)
                    {

                        for (int k = 0; k < houserentcheck.Tables[0].Rows.Count; k++)
                        {

                            #region subMain
                            houserentcheck.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(houserentcheck.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                            dvnew1 = houserentcheck.Tables[1].DefaultView;
                            if (dvnew1.Count > 0)
                            {
                                double MaxLimitAmount = 0;
                                string MaxAmount = Convert.ToString(houserentcheck.Tables[0].Rows[k]["MaxLimitAmount"]);
                                double.TryParse(MaxAmount, out MaxLimitAmount);
                                double OverAllTotal = 0;
                                for (int intCh = 0; intCh < dvnew1.Count; intCh++)
                                {
                                    string gettext = Convert.ToString(dvnew1[intCh]["ITAllowDeductName"]);
                                    if (gettext == "Less:Deduction For Interest Payable On Housing Loan")
                                    {
                                        string ITTypes = Convert.ToString(dvnew1[intCh]["ITType"]);
                                        string ITCommons = Convert.ToString(dvnew1[intCh]["ITCommon"]);
                                        string ITCommonValues = Convert.ToString(dvnew1[intCh]["ITCommonValue"]);

                                        houserentcheck.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew1[intCh]["IT_IDFK"]) + "'";
                                        dvh = houserentcheck.Tables[3].DefaultView;
                                        if (dvh.Count > 0)
                                        {
                                            string DirectValue = Convert.ToString(dvh[0]["TotalAmount"]);

                                            double.TryParse(DirectValue, out houserentval);
                                        }
                                        if (houserentval > 0)
                                        {
                                            RentPaidAmount = 0;
                                            HouseRent = 0;
                                            HalfPercentofActualSalary = 0;
                                            TotalHRA = 0;
                                        }
                                    }


                                }
                            #endregion

                            }

                        }

                    }

                }
                int cellpadding = 2;
                SbHtml.Append("<div style='height:845px; width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;page-break-after: always;font-size:12px;font-family:Arial;'>");
                #region Header
                SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;font-size:12px;font-family:Arial;'>");
                SbHtml.Append("<table cellspacing='0' cellpadding='" + cellpadding + "' style='width: 645px; border:1px solid gray;font-size:12px;' border='1px'>");
                SbHtml.Append("<tr>");
                SbHtml.Append("<td colspan='2'>");
                SbHtml.Append("NAME AND ADDRESS OF THE EMPLOYER");
                SbHtml.Append("</td>");
                SbHtml.Append("<td colspan='2'>");
                SbHtml.Append("NAME AND DESIGNATION OF THE EMPLOYEE");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr>");
                SbHtml.Append("<td colspan='2'>");
                SbHtml.Append(Convert.ToString(ds.Tables[1].Rows[0]["Principal"]) + "<br>" + Convert.ToString(ds.Tables[1].Rows[0]["Collname"]) + "<br>" + Convert.ToString(ds.Tables[1].Rows[0]["Address"]));
                SbHtml.Append("</td>");
                SbHtml.Append("<td colspan='2'>");
                SbHtml.Append(Convert.ToString(ds.Tables[0].Rows[0]["staff_name"]) + "<br>" + Convert.ToString(ds.Tables[0].Rows[0]["desig_name"]));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("PAN / GIR NO.");
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("TAN");
                SbHtml.Append("</td>");
                SbHtml.Append("<td colspan='2'>");
                SbHtml.Append("PAN / GIR NO.");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td>");
                SbHtml.Append(Convert.ToString(PANNo.Trim()));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append(Convert.ToString(TANNo.Trim()));
                SbHtml.Append("</td>");
                SbHtml.Append("<td colspan='2'>");
                SbHtml.Append(Convert.ToString(ds.Tables[0].Rows[0]["pangirnumber"]));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td rowspan='3'  style='text-align:left; width: 100px;'>");
                SbHtml.Append("TDS Circle where Annual Return/Statement under section 206 is to be filed.");
                SbHtml.Append("</td>");
                SbHtml.Append("<td colspan='2'>");
                SbHtml.Append("PERIOD");
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("ASSESSMENT YEAR");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("From");
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("To");
                SbHtml.Append("</td>");
                SbHtml.Append("<td rowspan='2'>");
                SbHtml.Append((Convert.ToInt32(fromyear) + 1) + "-" + (Convert.ToInt32(toyear) + 1));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("01/" + frommonth + "/" + fromyear);
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("01/" + tomonth + "/" + toyear);
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("</table>");
                SbHtml.Append("</div>");
                #endregion
                #region body
                SbHtml.Append("<div>");
                SbHtml.Append("<table cellspacing='0' cellpadding='" + cellpadding + "' style='width: 645px; border:1px solid gray; margin-left: 5px;font-size:12px;' border='1px'>");
                SbHtml.Append("<tr>");
                SbHtml.Append("<td colspan='4'>");
                SbHtml.Append("<span style ='font-size:12px;font-weight:bold;'>");
                SbHtml.Append("DETAILS OF SALARY PAID AND ANY OTHER INCOME AND TAX DEDUCTED");
                SbHtml.Append("</span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("</table>");
                SbHtml.Append("</div>");
                #endregion
                SbHtml.Append("<div>");
                SbHtml.Append("<table cellspacing='0' cellpadding='" + cellpadding + "' style='width: 645px; border:1px solid gray; margin-left: 5px;font-size:12px;' border='1px'>");
                // I Point 
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append("1");
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("GROSS SALARY <br> (a) Salary as per provisions contained in section 17(1) <br> (b) Value of perquesties under section 17(2) <br> (c) Profits in lieu of salary under section 17(3) <br> (d) TOTAL");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(ActualBasicAmount) + "<br><br><br><br><br>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                // II Point 
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append("2");
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Less: Allowance to the extent exempt under section 10 <br> A. Actual HRA received of <br> B. Rent paid less 10% of Salary + DA <br> C. Chennai, Mumbai, Calcutta & Delhi Employees 50% of salary, Others 40%");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("<br>" + Math.Round(TotalHRA) + "<br>" + Math.Round(RentPaidAmount) + "<br>" + Math.Round(HalfPercentofActualSalary));
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(HouseRent) + "<br><br><br><br>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                ActualBasicAmount = ActualBasicAmount - HouseRent;
                // III Point 
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append("3");
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Balance (1 - 2)");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(ActualBasicAmount) + "");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                DataView dv = new DataView();
                DataView dvnew = new DataView();
                DataView dAllview = new DataView();
                DataTable dt = new DataTable();
                Hashtable settingallow = new Hashtable();
                string ITType = string.Empty;
                string ITCommon = string.Empty;
                string ITCommonValue = string.Empty;
                int Count = 3;
                string headText = string.Empty;
                string SearialNo = string.Empty;
                double HeadValue = 0;//barath 13.10.17 
                double FinalHeadValue = 0;//barath 13.10.17 
                double ActualAmount = 0;
                double HeadTotal = 0;
                //delsi0903
                string maxAgeValue = string.Empty;
                string minAgeValue = string.Empty;
                string agechecked = string.Empty;
                double maxAge = 0;
                double minAge = 0;
                double maxVal = 0;
                double minVal = 0;
                //  string age = d2.GetFunction("select DATEDIFF(yyyy,date_of_birth,getdate()) from staff_appl_master where appl_id='" + ApplId + "'");//delsi0803

                string age = d2.GetFunction("select  DATEDIFF(yy,date_of_birth,getdate())- CASE WHEN  DATEADD(YY,DATEDIFF(YY,date_of_birth,GETDATE()),date_of_birth) >GETDATE()THEN 1 Else 0 END As [Age] from staff_appl_master where appl_id='" + ApplId + "'");//delsi0604
                if (Convert.ToInt32(age) >= 60)//delsi2403
                {
                    if (Gender == "Male" || Gender == "MALE")
                    {

                        Gender = "Senior Citizen Male";
                    }
                    if (Gender == "Female" || Gender == "FAMALE")
                    {
                        Gender = "Senior Citizen Female";

                    }
                    if (Gender == "TransGender" || Gender == "TRANSGENDER")
                    {

                        Gender = "Senior Citizen TransGender";
                    }
                }


                //delsi0903
                q1 = "select ITGroupPK,GroupName,GroupDesc,MaxLimitAmount from IT_GroupMaster where parentCode='0' and collegeCode='" + ddlcollege.SelectedValue + "' order by isnull(Priority,10000) asc";
                q1 += " select ITGroupPK,GroupName,GroupDesc,ParentCode,ITGroupType,IT_IDFK,ITType,ITAllowDeductName,ITAllowDeductDiscription,ITCommon,ITCommonValue,IsAgeRange,MaxValue,MinValue from IT_GroupMaster IT, IT_GroupMapping IM,IT_OtherAllowanceDeducation AD where IT.ITGroupPK=IM.ITGroupFK and AD.IT_ID=IM.IT_IDFK and IT.CollegeCode='" + ddlcollege.SelectedValue + "'";
                q1 += " select distinct ITGroupPK,GroupName,GroupDesc,MaxLimitAmount,parentCode,isnull(Priority,10000) from IT_GroupMaster IT,IT_GroupMapping IM where IT.ITGroupPk=IM.ITGroupFK and collegeCode='" + ddlcollege.SelectedValue + "' order by isnull(Priority,10000) asc";
                q1 += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,percentage from IT_Staff_AllowanceDeduction_Details where ITAllowdeductType in   (1,2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplId + "' and CollegeCode='" + ddlcollege.SelectedValue + "'  group by AllowdeductID,ITAllowDeductType,percentage";
                q1 += " select round (FromRange,0) FromRange,round (ToRange,0) ToRange,Amount,mode  from HR_ITCalculationSettings where collegeCode='" + ddlcollege.SelectedValue + "' and sex='" + Gender + "'";
                q1 += "  select sum(Amount) as TotalAmount,AllowdeductID,ITAllowDeductType,ITMonth,ITYear from IT_Staff_AllowanceDeduction_Details ID,IT_OtherAllowanceDeducation IA where ID.AllowDeductID=IA.IT_ID and ITAllowdeductType in   (2) and ( (ITMonth >= '" + frommonth + "' and ITYear = '" + fromyear + "') or (ITMonth <='" + tomonth + "' and ITYear = '" + toyear + "' )) and staff_ApplID='" + ApplId + "' and IA.CollegeCode='" + ddlcollege.SelectedValue + "' and isnull(IsIncomeTax,'0')='1'  group by AllowdeductID,ITAllowDeductType,ITMonth,ITYear";
                q1 += " select IT_ID,ITCommon,ITCommonValue,ITType from IT_OtherAllowanceDeducation  where  isnull(IsIncomeTax,'0')='1'  and CollegeCode='" + ddlcollege.SelectedValue + "'";
                ds1.Clear();
                // string age = d2.GetFunction("select DATEDIFF(yyyy,date_of_birth,getdate()) from staff_appl_master where appl_id='" + ApplId + "'");//delsi0803
                double staff_age = 0;
                double.TryParse(age, out staff_age);
                ds1 = d2.select_method_wo_parameter(q1, "text");
                if (ds1.Tables.Count > 1 && ds1.Tables[0].Rows.Count > 0)
                {
                    double LicAmt = 0;
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        double CommomOverAllTotal = 0;
                        double GrandCommonTotal = 0;
                        //HeadValue = 0;
                        headText = string.Empty;
                        SearialNo = string.Empty;
                        HeadTotal = 0;
                        FinalHeadValue = 0;
                        ActualAmount = 0;
                        ds1.Tables[2].DefaultView.RowFilter = "parentCode='" + Convert.ToString(ds1.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                        dv = ds1.Tables[2].DefaultView;
                        if (dv.Count > 0)
                        {
                            #region Main
                            //Count++;
                            //HeadValue += "<br>";barath 11.10.2017
                            if (!CbShowDiscription.Checked)
                                headText = Convert.ToString(ds1.Tables[0].Rows[k]["GroupName"]) + "<br>";
                            else
                                headText = Convert.ToString(ds1.Tables[0].Rows[k]["GroupDesc"]) + "<br>";
                            int Cs = 0;
                            string Commonoverall = Convert.ToString(ds1.Tables[0].Rows[k]["MaxLimitAmount"]);
                            double.TryParse(Commonoverall, out CommomOverAllTotal);
                            for (int intn = 0; intn < dv.Count; intn++)
                            {
                                SearialNo = string.Empty; HeadValue = 0;
                                ds1.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(dv[intn]["ITGroupPK"]) + "'";
                                dvnew = ds1.Tables[1].DefaultView;
                                if (dvnew.Count > 0)
                                {
                                    Cs++;
                                    Count++;
                                    SearialNo += Convert.ToString(Count);
                                    string append = string.Empty;
                                    if (intn == 0)
                                    {
                                        if (!CbShowDiscription.Checked)
                                            headText = Convert.ToString(ds1.Tables[0].Rows[k]["GroupName"]) + "<br>" + Convert.ToString(dvnew[0]["GroupName"]) + "<br>";
                                        else
                                            headText = Convert.ToString(ds1.Tables[0].Rows[k]["GroupDesc"]) + "<br>" + Convert.ToString(dvnew[0]["GroupName"]) + "<br>";
                                    }
                                    double MaxLimitAmount = 0;
                                    string MaxAmount = Convert.ToString(dv[intn]["MaxLimitAmount"]);
                                    double.TryParse(MaxAmount, out MaxLimitAmount);
                                    double OverAllTotal = 0;
                                    for (int intCh = 0; intCh < dvnew.Count; intCh++)
                                    {
                                        if (intCh == 0)
                                        {
                                            if (!CbShowDiscription.Checked)
                                                headText += Convert.ToString(dvnew[intCh]["ITAllowDeductName"]) + "<br>";
                                            else
                                                headText += Convert.ToString(dvnew[intCh]["ITAllowDeductDiscription"]) + "<br>";
                                        }
                                        else
                                        {
                                            if (!CbShowDiscription.Checked)
                                                headText = Convert.ToString(dvnew[intCh]["ITAllowDeductName"]) + "<br>";
                                            else
                                                headText = Convert.ToString(dvnew[intCh]["ITAllowDeductDiscription"]) + "<br>";
                                        }
                                        double AllowAndDeductTotal = 0;
                                        double DirectAllowDeductValue = 0;
                                        //string Getvalue = string.Empty;
                                        double Getvalue = 0;
                                        double AdditionalDeduction = 0;
                                        ITType = Convert.ToString(dvnew[intCh]["ITType"]);
                                        //int it = (int)(dvnew[intCh]["ITType"]);
                                        ITCommon = Convert.ToString(dvnew[intCh]["ITCommon"]);
                                        ITCommonValue = Convert.ToString(dvnew[intCh]["ITCommonValue"]);
                                        agechecked = Convert.ToString(dvnew[intCh]["IsAgeRange"]);
                                        maxAgeValue = Convert.ToString(dvnew[intCh]["MaxValue"]);//delsi0803
                                        minAgeValue = Convert.ToString(dvnew[intCh]["MinValue"]);
                                        if (ITCommon.Trim() == "1" || ITCommon.Trim() == "True")
                                        {
                                            if (ITType.Trim() == "1")
                                            {
                                                if (ITCommonValue.Trim() != "")
                                                {
                                                    //16.12.17 barath
                                                    if (!string.IsNullOrEmpty(ITCommonValue))
                                                    {
                                                        string[] DeductionName = ITCommonValue.Split(',');
                                                        foreach (var item in DeductionName)
                                                        {
                                                            if (!string.IsNullOrEmpty(item))
                                                            {
                                                                if (DeductionHash.ContainsKey(item.Trim()))
                                                                {
                                                                    double.TryParse(Convert.ToString(DeductionHash[item.Trim()]), out AdditionalDeduction);
                                                                    AllowAndDeductTotal += AdditionalDeduction;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //Getvalue = Convert.ToString(AllowanceHash[ITCommonValue.Trim()]);
                                                }
                                            }
                                            else if (ITType.Trim() == "2")
                                            {
                                                if (ITCommonValue.Trim() != "")
                                                {
                                                    //16.12.17 barath
                                                    if (!string.IsNullOrEmpty(ITCommonValue))
                                                    {
                                                        string[] DeductionName = ITCommonValue.Split(',');
                                                        foreach (var item in DeductionName)
                                                        {
                                                            if (!string.IsNullOrEmpty(item))
                                                            {
                                                                if (DeductionHash.ContainsKey(item.Trim()))
                                                                {
                                                                    double.TryParse(Convert.ToString(DeductionHash[item.Trim()]), out AdditionalDeduction);
                                                                    AllowAndDeductTotal += AdditionalDeduction;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //Getvalue = Convert.ToString(DeductionHash[ITCommonValue.Trim()]);
                                                }
                                            }
                                            //double.TryParse(Getvalue, out AllowAndDeductTotal);
                                        }
                                        ds1.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew[intCh]["IT_IDFK"]) + "'";
                                        dAllview = ds1.Tables[3].DefaultView;
                                        if (dAllview.Count > 0)
                                        {
                                            string DirectValue = Convert.ToString(dAllview[0]["TotalAmount"]);
                                            double.TryParse(DirectValue, out DirectAllowDeductValue);
                                            if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIC")//jayaram
                                                LicAmt = DirectAllowDeductValue;
                                            else if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIFE INSURANCE")
                                                LicAmt = DirectAllowDeductValue;
                                            else if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).ToUpper() == "LIFE INSURANCE(LIC)")
                                                LicAmt = DirectAllowDeductValue;
                                        }
                                        else //barath 28.11.17
                                        {
                                            if (Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).Trim().Contains('/'))
                                            {
                                                string[] DeductionName = Convert.ToString(dvnew[intCh]["ITAllowDeductName"]).Trim().Split('/');
                                                double DedutionAmt = 0;
                                                foreach (string deduct in DeductionName)
                                                {
                                                    DedutionAmt = 0;
                                                    string deductShort = Convert.ToString(IncentiveMasterDeductionHash[deduct]);
                                                    if (!string.IsNullOrEmpty(deductShort))
                                                    {
                                                        if (DeductionHash.ContainsKey(deductShort))
                                                            double.TryParse(Convert.ToString(DeductionHash[deductShort]), out DedutionAmt);
                                                    }
                                                    DirectAllowDeductValue += DedutionAmt;
                                                }
                                            }
                                        }
                                        DirectAllowDeductValue += AllowAndDeductTotal;
                                        //delsi0803
                                        if (age != "0")
                                        {
                                            if (agechecked.Trim() == "1" || agechecked.Trim() == "True")
                                            {
                                                string[] maxArr = maxAgeValue.Split('-');
                                                string[] minArr = minAgeValue.Split('-');
                                                if (maxArr.Length == 2)
                                                {
                                                    double.TryParse(Convert.ToString(maxArr[0]), out maxAge);
                                                    double.TryParse(Convert.ToString(maxArr[1]), out maxVal);
                                                }
                                                if (minArr.Length == 2)
                                                {
                                                    double.TryParse(Convert.ToString(minArr[0]), out minAge);
                                                    double.TryParse(Convert.ToString(minArr[1]), out minVal);
                                                }
                                                //if (maxAge != 0 && maxAge <= staff_age)
                                                //    if (maxVal != 0 && maxVal > DirectAllowDeductValue)
                                                //        DirectAllowDeductValue = maxVal;
                                                //if (minAge != 0 && minAge > staff_age)
                                                //    if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                //        DirectAllowDeductValue = minVal;


                                                if (maxAge != 0 && maxAge < staff_age)
                                                    if (maxVal != 0 && maxVal < DirectAllowDeductValue)//delsi09ref

                                                        DirectAllowDeductValue = maxVal;
                                                if (minAge != 0 && minAge > staff_age)
                                                    if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                        DirectAllowDeductValue = minVal;
                                            }
                                        }
                                        //delsi0903

                                        OverAllTotal += DirectAllowDeductValue;
                                        //HeadValue += Convert.ToString(Math.Round(DirectAllowDeductValue)) +"<br>"; 
                                        HeadValue = (Math.Round(DirectAllowDeductValue));//barath 11.10.2017
                                        SbHtml.Append("<tr>");
                                        SbHtml.Append("<td style='text-align:center; width:30px;'>");
                                        SbHtml.Append(SearialNo);
                                        SbHtml.Append("</td>");
                                        SbHtml.Append("<td>");
                                        SbHtml.Append(headText);
                                        SbHtml.Append("</td>");
                                        SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                        SbHtml.Append(HeadValue);
                                        SbHtml.Append("</td>");
                                        SbHtml.Append("<td style='text-align:center; width:30px;'>");
                                        SbHtml.Append("");
                                        SbHtml.Append("</td>");
                                        SearialNo = string.Empty;
                                    }
                                    string MaxWord = string.Empty;
                                    if (MaxLimitAmount != 0)
                                    {
                                        MaxWord = " restricted to Rs." + Math.Round(MaxLimitAmount) + "/-";
                                    }
                                    HeadTotal += OverAllTotal;
                                    HeadTotal = Math.Round(HeadTotal);
                                    if (MaxLimitAmount != 0 && MaxLimitAmount > OverAllTotal)
                                    {
                                        GrandCommonTotal += OverAllTotal;
                                        FinalHeadValue += Math.Round(OverAllTotal);
                                    }
                                    else if (MaxLimitAmount != 0 && OverAllTotal > MaxLimitAmount)
                                    {
                                        GrandCommonTotal += MaxLimitAmount;
                                        FinalHeadValue += Math.Round(MaxLimitAmount);
                                    }
                                    else
                                    {
                                        GrandCommonTotal += OverAllTotal;
                                        FinalHeadValue += Math.Round(OverAllTotal);
                                    }
                                    //SbHtml.Append("<tr>");
                                    //SbHtml.Append("<td style='text-align:center; width:30px;'>");
                                    //SbHtml.Append(SearialNo);
                                    //SbHtml.Append("</td>");
                                    //SbHtml.Append("<td>");
                                    //SbHtml.Append(headText);
                                    //SbHtml.Append("</td>");
                                    //SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                    //SbHtml.Append(HeadValue);
                                    //SbHtml.Append("</td>");
                                    //SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                    //SbHtml.Append("");
                                    //SbHtml.Append("</td>");
                                    //SbHtml.Append("</tr>");
                                    SbHtml.Append("<tr>");
                                    SbHtml.Append("<td style='text-align:center; width:30px;'>");
                                    SbHtml.Append("");
                                    SbHtml.Append("</td>");
                                    SbHtml.Append("<td>");
                                    SbHtml.Append("Total " + MaxWord + "");
                                    SbHtml.Append("</td>");
                                    SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                    SbHtml.Append(HeadTotal);
                                    SbHtml.Append("</td>");
                                    SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                    SbHtml.Append("");//FinalHeadValue
                                    SbHtml.Append("</td>");
                                    SbHtml.Append("</tr>");
                                }
                            }
                            string WordMax = string.Empty;
                            double GrandCommontotal = 0;
                            if (CommomOverAllTotal != 0)
                            {
                                WordMax = " restricted to Rs." + Math.Round(CommomOverAllTotal) + "/-";
                            }
                            GrandCommontotal += Math.Round(GrandCommonTotal);
                            double MainAmount = 0;
                            if (CommomOverAllTotal != 0 && CommomOverAllTotal > GrandCommonTotal)
                            {
                                MainAmount = GrandCommonTotal;
                            }
                            else if (CommomOverAllTotal != 0 && GrandCommonTotal > CommomOverAllTotal)
                            {
                                MainAmount = CommomOverAllTotal;
                            }
                            else
                            {
                                MainAmount = GrandCommonTotal;
                            }
                            if (ITType.Trim() == "1")
                            {
                                ActualBasicAmount = Convert.ToDouble(ActualBasicAmount) + Math.Round(MainAmount);
                            }
                            else if (ITType.Trim() == "2")
                            {
                                ActualBasicAmount = Convert.ToDouble(ActualBasicAmount) - Math.Round(MainAmount);
                            }
                            ActualAmount += Math.Round(ActualBasicAmount);
                            SbHtml.Append("<tr>");
                            SbHtml.Append("<td style='text-align:center; width:30px;'>");
                            SbHtml.Append("");
                            SbHtml.Append("</td>");
                            SbHtml.Append("<td>");
                            SbHtml.Append("Grand Total " + WordMax + "");
                            SbHtml.Append("</td>");
                            SbHtml.Append("<td style='text-align:right; width:100px;'>");
                            SbHtml.Append(Math.Round(MainAmount));//MainAmount
                            SbHtml.Append("</td>");
                            SbHtml.Append("<td style='text-align:right; width:100px;'>");
                            SbHtml.Append(ActualAmount);
                            SbHtml.Append("</td>");
                            SbHtml.Append("</tr>");
                            #endregion
                        }
                        else
                        {
                            #region subMain
                            ds1.Tables[1].DefaultView.RowFilter = "ITGroupPK='" + Convert.ToString(ds1.Tables[0].Rows[k]["ITGroupPK"]) + "'";
                            dvnew = ds1.Tables[1].DefaultView;
                            if (dvnew.Count > 0)
                            {
                                double MaxLimitAmount = 0;
                                Count++;
                                SearialNo = Convert.ToString(Count);
                                HeadValue = 0;
                                string MaxAmount = Convert.ToString(ds1.Tables[0].Rows[k]["MaxLimitAmount"]);
                                double.TryParse(MaxAmount, out MaxLimitAmount);
                                double OverAllTotal = 0;
                                for (int intCh = 0; intCh < dvnew.Count; intCh++)
                                {
                                    //Count++;
                                    //SearialNo = Convert.ToString(Count);//barath 12.10.17
                                    if (intCh == 0)
                                    {
                                        if (!CbShowDiscription.Checked)
                                            headText += Convert.ToString(dvnew[intCh]["ITAllowDeductName"]) + "<br>";
                                        else
                                            headText += Convert.ToString(dvnew[intCh]["ITAllowDeductDiscription"]) + "<br>";
                                    }
                                    else
                                    {
                                        if (!CbShowDiscription.Checked)
                                            headText = Convert.ToString(dvnew[intCh]["ITAllowDeductName"]) + "<br>";
                                        else
                                            headText = Convert.ToString(dvnew[intCh]["ITAllowDeductDiscription"]) + "<br>";
                                    }
                                    double AllowAndDeductTotal = 0;
                                    double DirectAllowDeductValue = 0;
                                    string Getvalue = string.Empty;
                                    ITType = Convert.ToString(dvnew[intCh]["ITType"]);
                                    ITCommon = Convert.ToString(dvnew[intCh]["ITCommon"]);
                                    ITCommonValue = Convert.ToString(dvnew[intCh]["ITCommonValue"]);
                                    agechecked = Convert.ToString(dvnew[intCh]["IsAgeRange"]);
                                    maxAgeValue = Convert.ToString(dvnew[intCh]["MaxValue"]);//delsi0803
                                    minAgeValue = Convert.ToString(dvnew[intCh]["MinValue"]);
                                    if (ITCommon.Trim() == "1" || ITCommon.Trim() == "True")
                                    {
                                        if (ITType.Trim() == "1")
                                        {
                                            if (ITCommonValue.Trim() != "")
                                            {
                                                Getvalue = Convert.ToString(AllowanceHash[ITCommonValue.Trim()]);
                                            }
                                        }
                                        else if (ITType.Trim() == "2")
                                        {
                                            if (ITCommonValue.Trim() != "")
                                            {
                                                Getvalue = Convert.ToString(DeductionHash[ITCommonValue.Trim()]);
                                            }
                                        }
                                        double.TryParse(Getvalue, out AllowAndDeductTotal);
                                    }
                                    ds1.Tables[3].DefaultView.RowFilter = "AllowdeductID='" + Convert.ToString(dvnew[intCh]["IT_IDFK"]) + "'";
                                    dAllview = ds1.Tables[3].DefaultView;//delsireff
                                    if (dAllview.Count > 0)
                                    {
                                        string percentage_val = Convert.ToString(dAllview[0]["Percentage"]);//delsi2209
                                        string DirectValue = Convert.ToString(dAllview[0]["TotalAmount"]);
                                        if (percentage_val != "" && percentage_val != null)
                                        {
                                            double calculatepercent = (Convert.ToDouble(DirectValue) / 100) * Convert.ToDouble(percentage_val);
                                            DirectValue = Convert.ToString(calculatepercent);

                                        }
                                        double.TryParse(DirectValue, out DirectAllowDeductValue);
                                    }
                                    DirectAllowDeductValue += AllowAndDeductTotal;
                                    //delsi0803
                                    if (age != "0")
                                    {

                                        if (agechecked.Trim() == "1" || agechecked.Trim() == "True")
                                        {
                                            string[] maxArr = maxAgeValue.Split('-');
                                            string[] minArr = minAgeValue.Split('-');
                                            if (maxArr.Length == 2)
                                            {
                                                double.TryParse(Convert.ToString(maxArr[0]), out maxAge);
                                                double.TryParse(Convert.ToString(maxArr[1]), out maxVal);
                                            }
                                            if (minArr.Length == 2)
                                            {
                                                double.TryParse(Convert.ToString(minArr[0]), out minAge);
                                                double.TryParse(Convert.ToString(minArr[1]), out minVal);
                                            }
                                            //if (maxAge != 0 && maxAge <= staff_age)
                                            //    if (maxVal != 0 && maxVal > DirectAllowDeductValue)
                                            //        DirectAllowDeductValue = maxVal;
                                            //if (minAge != 0 && minAge > staff_age)
                                            //    if (minVal != 0 && DirectAllowDeductValue > minVal)
                                            //        DirectAllowDeductValue = minVal;

                                            if (maxAge != 0 && maxAge < staff_age)
                                                if (maxVal != 0 && maxVal < DirectAllowDeductValue)//delsi09ref

                                                    DirectAllowDeductValue = maxVal;
                                            if (minAge != 0 && minAge > staff_age)
                                                if (minVal != 0 && DirectAllowDeductValue > minVal)
                                                    DirectAllowDeductValue = minVal;
                                        }
                                    }
                                    //delsi0903
                                    OverAllTotal += DirectAllowDeductValue;
                                    //HeadValue += Convert.ToString(Math.Round(DirectAllowDeductValue)) + "<br>";
                                    HeadValue = Math.Round(DirectAllowDeductValue);// +"<br>";barath 13.10.17
                                    SbHtml.Append("<tr>");
                                    SbHtml.Append("<td style='text-align:center; width:30px;'>");
                                    SbHtml.Append(SearialNo);
                                    SbHtml.Append("</td>");
                                    SbHtml.Append("<td>");
                                    SbHtml.Append(headText);
                                    SbHtml.Append("</td>");
                                    SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                    SbHtml.Append(HeadValue);
                                    SbHtml.Append("</td>");
                                    SbHtml.Append("<td style='text-align:center; width:30px;'>");
                                    SbHtml.Append("");
                                    SbHtml.Append("</td>");
                                    SearialNo = string.Empty;
                                }
                                double MainAmount = 0;
                                string MaxWord = string.Empty;
                                if (MaxLimitAmount != 0)
                                {
                                    MaxWord = " restricted to Rs." + MaxLimitAmount + "/-";
                                }
                                HeadTotal += OverAllTotal;
                                //SbHtml.Append("<tr>");
                                //SbHtml.Append("<td style='text-align:center; width:30px;'>");
                                //SbHtml.Append(SearialNo);
                                //SbHtml.Append("</td>");
                                //SbHtml.Append("<td>");
                                //SbHtml.Append(headText);
                                //SbHtml.Append("</td>");
                                //SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                //SbHtml.Append(HeadValue);
                                //SbHtml.Append("</td>");
                                //SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                //SbHtml.Append("");
                                //SbHtml.Append("</td>");
                                //SbHtml.Append("</tr>");
                                if (MaxLimitAmount != 0 && MaxLimitAmount > OverAllTotal)
                                {
                                    MainAmount = OverAllTotal;
                                    FinalHeadValue += Math.Round(OverAllTotal);
                                }
                                else if (MaxLimitAmount != 0 && OverAllTotal > MaxLimitAmount)
                                {
                                    MainAmount = MaxLimitAmount;
                                    FinalHeadValue += Math.Round(MaxLimitAmount);
                                }
                                else
                                {
                                    MainAmount = OverAllTotal;
                                    FinalHeadValue += Math.Round(OverAllTotal);
                                }
                                if (ITType.Trim() == "1")
                                {
                                    ActualBasicAmount = Convert.ToDouble(ActualBasicAmount) + Math.Round(MainAmount);
                                }
                                else if (ITType.Trim() == "2")
                                {
                                    ActualBasicAmount = Convert.ToDouble(ActualBasicAmount) - Math.Round(MainAmount);
                                }
                                ActualAmount += Math.Round(ActualBasicAmount);
                                SbHtml.Append("<tr>");
                                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                                SbHtml.Append("");
                                SbHtml.Append("</td>");
                                SbHtml.Append("<td>");
                                SbHtml.Append("Total " + MaxWord + "");
                                SbHtml.Append("</td>");
                                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                SbHtml.Append(HeadTotal);
                                SbHtml.Append("</td>");
                                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                                SbHtml.Append(ActualAmount);
                                SbHtml.Append("</td>");
                                SbHtml.Append("</tr>");
                            }
                            #endregion
                        }
                    }
                }
                double RemainAmount = 0;
                double TotalSalaryAmount = ActualBasicAmount;
                double FromRange = 0;
                double ToRange = 0;
                double BindAmount = 0;
                double TotalTaxableAmount = 0;
                double FinalTaxableincome = 0;
                #region RangeCalculation
                if (ds1.Tables.Count > 3)
                {
                    for (int intd = 0; intd < ds1.Tables[4].Rows.Count; intd++)
                    {
                        string Bindvalue = "From " + Convert.ToString(ds1.Tables[4].Rows[intd]["FromRange"]) + " To " + Convert.ToString(ds1.Tables[4].Rows[intd]["ToRange"]);
                        double.TryParse(Convert.ToString(ds1.Tables[4].Rows[intd]["FromRange"]), out FromRange);
                        double.TryParse(Convert.ToString(ds1.Tables[4].Rows[intd]["ToRange"]), out ToRange);
                        string Mode = Convert.ToString(ds1.Tables[4].Rows[intd]["mode"]);
                        string CalCAmount = Convert.ToString(ds1.Tables[4].Rows[intd]["Amount"]);
                        if (FromRange < ActualBasicAmount && ToRange < ActualBasicAmount)
                        {
                            BindAmount = ToRange - FromRange;
                            BindAmount += 1;
                        }
                        else if (FromRange < ActualBasicAmount && ToRange > ActualBasicAmount)
                        {
                            BindAmount = ActualBasicAmount - FromRange;
                            BindAmount += 1;
                        }
                        else
                        {
                            BindAmount = 0;
                        }
                        double CalCValueAmount = 0;
                        if (Mode.Trim() == "0" || Mode.Trim() == "False")
                        {
                            double.TryParse(CalCAmount, out  CalCValueAmount);
                        }
                        else if (Mode.Trim() == "1" || Mode.Trim() == "True")
                        {
                            CalCValueAmount = (BindAmount / 100) * Convert.ToDouble(CalCAmount);
                        }
                        TotalTaxableAmount += CalCValueAmount;
                    }
                }
                #endregion
                Count++;
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append(Convert.ToString(Count));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Tax on Total Income");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(TotalTaxableAmount));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                Count++;
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append(Convert.ToString(Count));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Surcharge");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("0");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                double RebateAmount = 0;

                //27.02.18
                double RebateDeductAmt = 0;
                double RebateDeductAmount = 0;
                string rebateAmt = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='RebateDeductAmount' and college_code='" + ddlcollege.SelectedItem.Value + "'");
                string[] Rebate = rebateAmt.Split('-');
                if (Rebate.Length == 2)
                {
                    double.TryParse(Convert.ToString(Rebate[0]), out RebateDeductAmt);
                    double.TryParse(Convert.ToString(Rebate[1]), out RebateDeductAmount);
                }
                if (TotalSalaryAmount < RebateDeductAmt)
                    RebateAmount = RebateDeductAmount;
                //if (TotalSalaryAmount < 500000)//29.11.17 TotalTaxableAmount
                //    RebateAmount = 5000;
                Count++;
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append(Convert.ToString(Count));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Relief under section 89/Rebate");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(RebateAmount));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                Count++;
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append(Convert.ToString(Count));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Tax Payable");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(TotalTaxableAmount));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                TotalTaxableAmount -= RebateAmount;
                Count++;
                string geteducess = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Educess' and college_code='" + ddlcollege.SelectedItem.Value + "'");
                int cessval = 0;
                if (geteducess != "" || geteducess != "0")
                {

                    cessval = Convert.ToInt32(geteducess);
                }
                else
                {
                    cessval = 3;
                }


                double TaxAmount = (TotalTaxableAmount / 100) * cessval;
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append(Convert.ToString(Count));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Education Cess");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(TaxAmount));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                TotalTaxableAmount += TaxAmount;
                Count++;

                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append(Convert.ToString(Count));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Tax payable");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(TotalTaxableAmount));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                #region TDS Tax Calculation
                double TDSAmount = 0;
                double CheckTds = 0;
                if (ds1.Tables[5].Rows.Count > 0)
                {
                    string TdsAmount = Convert.ToString(ds1.Tables[5].Compute("sum(TotalAmount)", ""));
                    double.TryParse(TdsAmount, out CheckTds);
                    TDSAmount += CheckTds;
                }
                if (ds1.Tables[6].Rows.Count > 0)
                {
                    for (int intTds = 0; intTds < ds1.Tables[6].Rows.Count; intTds++)
                    {
                        string Iscommon = Convert.ToString(ds1.Tables[6].Rows[intTds]["ITCommon"]);
                        string iscommonvalue = Convert.ToString(ds1.Tables[6].Rows[intTds]["ITCommonValue"]);
                        if (Iscommon.Trim() == "1" || Iscommon.Trim() == "True")
                        {
                            string GetCommonValue = Convert.ToString(DeductionHash[iscommonvalue.Trim()]);
                            double.TryParse(GetCommonValue, out CheckTds);
                            TDSAmount += CheckTds;
                        }
                    }
                }
                //double incTaxAmt = 0;
                //if (MonthlypayDeductionHash.ContainsKey("INC TAX"))
                //    double.TryParse(Convert.ToString(MonthlypayDeductionHash["INC TAX"]), out incTaxAmt);
                //if (MonthlypayDeductionHash.ContainsKey("INCOME TAX"))
                //    double.TryParse(Convert.ToString(MonthlypayDeductionHash["INCOME TAX"]), out incTaxAmt);
                //if (MonthlypayDeductionHash.ContainsKey("I TAX"))
                //    double.TryParse(Convert.ToString(MonthlypayDeductionHash["I TAX"]), out incTaxAmt);
                //if (MonthlypayDeductionHash.ContainsKey("ITAX"))
                //    double.TryParse(Convert.ToString(MonthlypayDeductionHash["ITAX"]), out incTaxAmt);
                //TDSAmount += incTaxAmt;
                Fpspread2.Sheets[0].RowCount++;
                double ProFxTax = TDSAmount;
                Count++;
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append(Convert.ToString(Count));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Less: (a) Tax deducted at source u/s 192(1) <br> (b) Tax paid by the employer on behalf of the employer u/s");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(ProFxTax));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                TotalTaxableAmount -= ProFxTax;

                if (reinvestment != 0 && TotalTaxableAmount < 0)//delsi2509
                {
                    TotalTaxableAmount = reinvestment + TotalTaxableAmount;
                }

                Count++;
                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; width:30px;'>");
                SbHtml.Append(Convert.ToString(Count));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("Tax Payable / Refundable");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right; width:100px;'>");
                SbHtml.Append(Math.Round(TotalTaxableAmount));
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("</table>");
                #region delsi
                int Diffenerce = 0;
                DateTime fromdate = new DateTime(Convert.ToInt32(fromyear), Convert.ToInt32(frommonth), 28);
                DateTime todate = new DateTime(Convert.ToInt32(toyear), Convert.ToInt32(tomonth), 28);
                Diffenerce = (todate.Month - fromdate.Month) + 12 * (todate.Year - fromdate.Year);
                string sql1 = "select m.PayMonth,m.PayYear,m.staff_code,sa.appl_id,convert(varchar(max),m.deductions)deductions,it.CheckDDNo,convert(varchar(10), it.CheckDDDate,103)CheckDDDate,it.ITMonth,it.ITYear, it.ChallanNo  from   stafftrans st,monthlypay m inner join staffmaster sm on sm.staff_code=m.staff_code  inner join staff_appl_master sa on sa.appl_no=sm.appl_no  left join IT_Staff_AllowanceDeduction_Details it on sa.appl_id=it.Staff_ApplID and m.payyear=it.ityear and m.paymonth=it.itmonth where sa.appl_no=sm.appl_no and sm.staff_code=m.staff_code and st.staff_code=m.staff_code and sm.resign='0' and sm.settled='0' and sa.appl_id='" + ApplId + "' and (m.paymonth >= '" + frommonth + "' and m.payyear = '" + fromyear + "' or m.paymonth <='" + tomonth + "' and m.payyear  = '" + toyear + "') group by m.PayYear,m.PayMonth,m.staff_code,sa.appl_id,convert(varchar(max),m.deductions),it.CheckDDNo,convert(varchar(10), it.CheckDDDate,103),it.ITMonth,it.ITYear,it.ChallanNo order by year(m.payyear),year(m.paymonth)";
                DataSet dsset = d2.select_method_wo_parameter(sql1, "Text");
                SbHtml.Append("<br><br><table cellspacing='0' cellpadding='" + cellpadding + "' style='width: 645px; border:1px solid gray; margin-left: 5px;font-size:12px;' border='1px'>");
                SbHtml.Append("<tr>");
                SbHtml.Append("<td align='center'>");
                SbHtml.Append("SI.No");
                SbHtml.Append("</td>");
                SbHtml.Append("<td align='center'>");
                SbHtml.Append("Total Tax deposited");
                SbHtml.Append("</td>");
                SbHtml.Append("<td align='center'>");
                SbHtml.Append("Cheque/DD No(if any)");
                SbHtml.Append("</td>");
                SbHtml.Append("<td  align='center'>");
                SbHtml.Append("BSR Code Of Bank Branch");
                SbHtml.Append("</td>");
                SbHtml.Append("<td align='center'>");
                SbHtml.Append("Date on which tax deposited");
                SbHtml.Append("</td>");
                SbHtml.Append("<td  align='center'>");
                SbHtml.Append("Transfer voucher/Challan No");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                double TotalTaxAmt = 0;
                for (int i = 0; i <= Diffenerce; i++)
                {
                    SbHtml.Append("<tr>");
                    SbHtml.Append("<td  align='center'>");
                    SbHtml.Append(i + 1);
                    SbHtml.Append("</td>");
                    if (i < dsset.Tables[0].Rows.Count)
                    {
                        SbHtml.Append("<td align='center'>");
                        string incTax = "";
                        string deductions = Convert.ToString(dsset.Tables[0].Rows[i]["deductions"]);
                        string[] deductionlist = deductions.Split('\\');
                        for (int k = 0; k < deductionlist.GetUpperBound(0); k++)
                        {
                            string getactal = deductionlist[k];
                            if (getactal.Trim() != "" && getactal != null)
                            {
                                string[] actallspv = getactal.Split(';');
                                if (actallspv.GetUpperBound(0) >= 3)
                                {
                                    if (actallspv[0].ToString().Trim().ToLower() == "inc tax" || actallspv[0].ToString().Trim().ToLower() == "i tax" || actallspv[0].ToString().Trim().ToLower() == "income tax" || actallspv[0].ToString().Trim().ToLower() == "itax" || actallspv[0].ToString().Trim().ToLower() == "tds")
                                    {
                                        string de = actallspv[0];
                                        string de1 = actallspv[1];
                                        string de2 = actallspv[2];
                                        string[] dedspl = de2.Split('-');
                                        if (dedspl.Length == 2)
                                        {
                                            if (de1.Trim().ToUpper() == "PERCENT")
                                                incTax = Convert.ToString(dedspl[1]);
                                            else if (de1.Trim().ToUpper() == "SLAB")
                                                incTax = Convert.ToString(dedspl[1]);
                                            else
                                                incTax = Convert.ToString(dedspl[0]);
                                        }
                                        else
                                        {
                                            incTax = Convert.ToString(actallspv[3]);
                                        }
                                        double InctaxAmt = 0;
                                        double.TryParse(incTax, out InctaxAmt);
                                        TotalTaxAmt += InctaxAmt;
                                        goto label;
                                    }
                                }
                            }
                        }

                    label:
                        double ITAMT = 0;//21.12.17
                        if (ds1.Tables[5].Rows.Count > 0)
                        {
                            DataView otherIT = new DataView();
                            ds1.Tables[5].DefaultView.RowFilter = " ITMonth='" + Convert.ToString(dsset.Tables[0].Rows[i]["paymonth"]) + "' and ITYear='" + Convert.ToString(dsset.Tables[0].Rows[i]["payyear"]) + "'";
                            otherIT = ds1.Tables[5].DefaultView;
                            if (otherIT.Count > 0)
                                double.TryParse(Convert.ToString(otherIT[0]["TotalAmount"]), out ITAMT);

                        }
                        double IncTAmt = 0;
                        double.TryParse(incTax, out IncTAmt);
                        IncTAmt += ITAMT;
                        TotalTaxAmt += ITAMT;
                        SbHtml.Append(IncTAmt);//21.12.17


                        SbHtml.Append("</td>");
                        SbHtml.Append("<td  align='center' >");
                        string checkno = Convert.ToString(dsset.Tables[0].Rows[i]["CheckDDNo"]);
                        SbHtml.Append(checkno);
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td  align='center' >");
                        SbHtml.Append("");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td  align='center' >");
                        string checkdate = Convert.ToString(dsset.Tables[0].Rows[i]["CheckDDDate"]);
                        SbHtml.Append(checkdate);
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td  align='center' >");
                        string challanno = Convert.ToString(dsset.Tables[0].Rows[i]["ChallanNo"]);
                        SbHtml.Append(challanno);
                        SbHtml.Append("</td>");
                    }
                    else
                    {
                        SbHtml.Append("<td>");
                        SbHtml.Append("");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td>");
                        SbHtml.Append("");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td >");
                        SbHtml.Append("");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td >");
                        SbHtml.Append("");
                        SbHtml.Append("</td>");
                        SbHtml.Append("<td >");
                        SbHtml.Append("");
                        SbHtml.Append("</td>");
                    }
                    //}
                    SbHtml.Append("<tr>");
                }

                double incTaxotherAmt = 0;
                ////21.12.17
                //double incTaxotherAmt = 0;
                //if (otherTaxAmttaxHash.ContainsKey("INC TAX"))
                //    double.TryParse(Convert.ToString(otherTaxAmttaxHash["INC TAX"]), out incTaxotherAmt);
                //if (otherTaxAmttaxHash.ContainsKey("INCOME TAX"))
                //    double.TryParse(Convert.ToString(otherTaxAmttaxHash["INCOME TAX"]), out incTaxotherAmt);
                //if (otherTaxAmttaxHash.ContainsKey("I TAX"))
                //    double.TryParse(Convert.ToString(otherTaxAmttaxHash["I TAX"]), out incTaxotherAmt);
                //if (otherTaxAmttaxHash.ContainsKey("ITAX"))
                //    double.TryParse(Convert.ToString(otherTaxAmttaxHash["ITAX"]), out incTaxotherAmt);
                //if (incTaxotherAmt != 0)
                //{
                //    SbHtml.Append("<tr>");
                //    SbHtml.Append("<td align='center' >");
                //    SbHtml.Append("Other Deductions");
                //    SbHtml.Append("</td>");
                //    SbHtml.Append("<td  align='center'>");
                //    SbHtml.Append(incTaxotherAmt);
                //    SbHtml.Append("</td>");
                //    SbHtml.Append("<td >");
                //    SbHtml.Append("");
                //    SbHtml.Append("</td>");
                //    SbHtml.Append("<td >");
                //    SbHtml.Append("");
                //    SbHtml.Append("</td>");
                //    SbHtml.Append("<td >");
                //    SbHtml.Append("");
                //    SbHtml.Append("</td>");
                //    SbHtml.Append("<td >");
                //    SbHtml.Append("");
                //    SbHtml.Append("</td>");
                //    SbHtml.Append("</tr>");
                //}

                #region Addition Deduction for Income Tax barath 05.01.18
                if (deductionDS.Tables != null)
                {
                    if (deductionDS.Tables[0].Rows.Count > 0)
                    {
                        double taxAmtOther = 0;
                        SbHtml.Append("<tr>");
                        SbHtml.Append("<td align='center' colspan='2'>");
                        SbHtml.Append("Other Deductions");
                        SbHtml.Append("</td>");
                        SbHtml.Append("</tr>");
                        int RowNo = 0;
                        for (int ded = 0; ded < deductionDS.Tables[0].Rows.Count; ded++)
                        {
                            string splded = Convert.ToString(deductionDS.Tables[0].Rows[ded]["AllowanceDeductAmt"]);
                            string chequeddno = Convert.ToString(deductionDS.Tables[0].Rows[ded]["mastercriteriavalue2"]);
                            string chequedddate = Convert.ToString(deductionDS.Tables[0].Rows[ded]["mastercriteriavalue3"]);
                            string challonnotransferno = Convert.ToString(deductionDS.Tables[0].Rows[ded]["mastercriteriavalue4"]);
                            string[] spldedname = splded.Split(';'); double dedvalue = 0;
                            if (spldedname.Length > 0)
                            {
                                for (int spld = 0; spld < spldedname.Length; spld++)
                                {
                                    if (spldedname[spld].Contains('-'))
                                    {
                                        double value = 0;
                                        string dednameadd = Convert.ToString(spldedname[spld].Split('-')[0]).ToUpper();
                                        string dedvalueadd = spldedname[spld].Split('-')[1];
                                        if (dednameadd == "INC TAX" || dednameadd == "I TAX" || dednameadd == "INCOME TAX" || dednameadd == "ITAX" || dednameadd == "TDS")
                                        {
                                            RowNo++;
                                            SbHtml.Append("<tr>");
                                            SbHtml.Append("<td align='center' >");
                                            SbHtml.Append(RowNo);
                                            SbHtml.Append("</td>");
                                            SbHtml.Append("<td  align='center'>");
                                            SbHtml.Append(dedvalueadd);
                                            SbHtml.Append("</td>");
                                            SbHtml.Append("<td align='center'>");
                                            SbHtml.Append(chequeddno);
                                            SbHtml.Append("</td>");
                                            SbHtml.Append("<td align='center'>");
                                            SbHtml.Append("");
                                            SbHtml.Append("</td>");
                                            SbHtml.Append("<td align='center'>");
                                            SbHtml.Append(chequedddate);
                                            SbHtml.Append("</td>");
                                            SbHtml.Append("<td align='center'>");
                                            SbHtml.Append(challonnotransferno);
                                            SbHtml.Append("</td>");
                                            SbHtml.Append("</tr>");
                                            taxAmtOther = 0;
                                            double.TryParse(Convert.ToString(dedvalueadd), out taxAmtOther);
                                            incTaxotherAmt += taxAmtOther;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                TotalTaxAmt += incTaxotherAmt;
                SbHtml.Append("<tr>");
                SbHtml.Append("<td align='center' >");
                SbHtml.Append("Total");
                SbHtml.Append("</td>");
                SbHtml.Append("<td  align='center'>");
                SbHtml.Append(TotalTaxAmt);
                SbHtml.Append("</td>");
                SbHtml.Append("<td >");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td >");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td >");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("<td >");
                SbHtml.Append("");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("</table>");
                #endregion
                #region Footer content
                SbHtml.Append("<br><br><table cellspacing='0' cellpadding='" + cellpadding + "' style='width: 645px; border:none; margin-left: 5px;font-size:12px;' ");
                SbHtml.Append("<tr>");
                SbHtml.Append("<td align='center' colspan='4' style='text-align:justify;'>");
                DataSet footerDataset = d2.select_method_wo_parameter(" select sm.staff_name,sa.father_name,sa.sex,dm.desig_name from stafftrans st,staff_appl_master sa, staffmaster sm,desig_master dm where sa.appl_no=sm.appl_no and sm.staff_code=st.staff_code and st.desig_code=dm.desig_code and latestrec=1 and dm.desig_name='PRINCIPAL' ", "text");//and st.staff_code='" + StaffCode + "'
                string principalName = string.Empty;
                string principalFatherName = string.Empty;
                string principalSex = string.Empty;
                string principalDesign = string.Empty;
                if (footerDataset.Tables[0].Rows.Count > 0)
                {
                    principalName = Convert.ToString(footerDataset.Tables[0].Rows[0]["staff_name"]);
                    principalFatherName = Convert.ToString(footerDataset.Tables[0].Rows[0]["father_name"]);
                    principalSex = Convert.ToString(footerDataset.Tables[0].Rows[0]["sex"]);
                    principalDesign = Convert.ToString(footerDataset.Tables[0].Rows[0]["desig_name"]);
                }
                SbHtml.Append("I " + principalName);//+ "<br>" + Convert.ToString(ds.Tables[0].Rows[0]["desig_name"])
                string mrs = string.Empty;
                if (principalSex.ToUpper() == "MALE")
                    mrs = " son of Mr. ";
                else
                    mrs = " daughter of Mrs. ";
                SbHtml.Append(mrs);
                SbHtml.Append(principalFatherName);
                SbHtml.Append(" working in the capacity of ");
                SbHtml.Append(principalDesign + ("( Designation ) "));
                SbHtml.Append("do hereby certify that a sum of Rs.");
                SbHtml.Append(Math.Round(ProFxTax));
                string word = ConvertNumbertoWords(Convert.ToInt32(Math.Round(ProFxTax)));//06.02.18 barath
                SbHtml.Append(" (" + word + ") ");
                //SbHtml.Append("(in words)");
                SbHtml.Append("has been deducted at source and paid to the credit of the Central Government. I further certify that the information given above is true and correct based on the books.");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr >");
                SbHtml.Append("<td>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");

                SbHtml.Append("<tr>");
                SbHtml.Append("<td style='text-align:center; height:40px;'>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<td>");
                SbHtml.Append("Date:" + Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy")));
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("</td>");
                SbHtml.Append("<td>");
                SbHtml.Append("</td>");
                SbHtml.Append("<td style='text-align:right;'>");
                SbHtml.Append("Principal and Secretary");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                #endregion
                SbHtml.Append("</div>");
                SbHtml.Append("</div>");
                #endregion
                SbHtml.Append("</div>");
                contentDiv.InnerHtml = SbHtml.ToString();
                contentDiv.Visible = true;
                if (Convert.ToString(ViewState["E"]) == "E")
                {
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename=Form16.xls");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.xls";
                    System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                    contentDiv.RenderControl(htmlWrite);
                    Response.Write(stringWrite.ToString());
                    Response.End();
                    ViewState["E"] = null;
                }
                else { ScriptManager.RegisterStartupScript(this, GetType(), "btnPrint", "PrintDiv();", true); }
            }
        }
        catch
        {
        }
    }
    #endregion
    public string Alpha(int Count)
    {
        string value = string.Empty;
        try
        {
            switch (Count)
            {
                case 1:
                    value = "a";
                    break;
                case 2:
                    value = "b";
                    break;
                case 3:
                    value = "c";
                    break;
                case 4:
                    value = "d";
                    break;
                case 5:
                    value = "e";
                    break;
            }
        }
        catch
        {
        }
        return value;
    }
    /// <summary>
    /// Return monthlypay allowance value in Hashtable
    /// </summary>
    /// <param name="AllowanceDetDS"></param>
    /// <param name="tableNo"></param>
    /// <param name="tableRow"></param>
    /// <param name="TotalBasicAmount"></param>
    /// <returns></returns>
    Hashtable PayProcessAllowanceDet(DataSet AllowanceDetDS, int tableNo, int tableRow, ref double TotalBasicAmount)
    {
        Hashtable AllowanceHash = new Hashtable();
        TotalBasicAmount = 0;
        for (int intds = tableRow; intds < AllowanceDetDS.Tables[tableNo].Rows.Count; intds++)
        {
            string AllowanceValue = Convert.ToString(AllowanceDetDS.Tables[tableNo].Rows[intds]["allowances"]);
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
                            if (!AllowanceHash.ContainsKey(SecondSplit[0].Trim()))
                            {
                                AllowanceHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                            }
                            else
                            {
                                double GetValue = Convert.ToDouble(AllowanceHash[SecondSplit[0].Trim()]);
                                GetValue = GetValue + AllowTaeknValue;
                                AllowanceHash.Remove(SecondSplit[0].Trim());
                                AllowanceHash.Add(SecondSplit[0].Trim(), GetValue);
                            }
                        }
                    }
                }
            }
            TotalBasicAmount += Convert.ToDouble(AllowanceDetDS.Tables[tableNo].Rows[intds]["bsalary"]);
        }
        return AllowanceHash;
    }
    /// <summary>
    /// Return monthlypay deduction value in Hashtable
    /// </summary>
    /// <param name="AllowanceDetDS"></param>
    /// <param name="tableNo"></param>
    /// <param name="tableRow"></param>
    /// <param name="TotalBasicAmount"></param>
    /// <returns></returns>
    Hashtable PayProcessDeductionDet(DataSet DeductionDetDS, int tableNo, int tableRow, ref double TotalBasicAmount)
    {
        Hashtable DeductionHash = new Hashtable();
        TotalBasicAmount = 0;
        for (int intds = tableRow; intds < DeductionDetDS.Tables[tableNo].Rows.Count; intds++)
        {
            string deductionValue = Convert.ToString(DeductionDetDS.Tables[tableNo].Rows[intds]["deductions"]);
            string[] SplitFirst = deductionValue.Split('\\');
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
                            if (!DeductionHash.ContainsKey(SecondSplit[0].Trim()))
                            {
                                DeductionHash.Add(SecondSplit[0].Trim(), AllowTaeknValue);
                            }
                            else
                            {
                                double GetValue = Convert.ToDouble(DeductionHash[SecondSplit[0].Trim()]);
                                GetValue = GetValue + AllowTaeknValue;
                                DeductionHash.Remove(SecondSplit[0].Trim());
                                DeductionHash.Add(SecondSplit[0].Trim(), GetValue);
                            }
                        }
                    }
                }
            }
            TotalBasicAmount += Convert.ToDouble(DeductionDetDS.Tables[tableNo].Rows[intds]["bsalary"]);
        }
        return DeductionHash;
    }
    //barath 21.11.17
    protected void btnPrintExcel_Click(object sender, EventArgs e)
    {
        ViewState["E"] = "E";
        btnPrint_Click(sender, e);
        ViewState["E"] = null;
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakh ";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }

    /* static string NumberToWord(int num)
     {
         if (num == 0)
             return "Zero";
         if (num < 0)
             return "Not supported";
         var words = "";
         string[] strones = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
         string[] strtens = { "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
         int crore = 0, lakhs = 0, thousands = 0, hundreds = 0, tens = 0, single = 0;
         crore = num / 10000000; num = num - crore * 10000000;
         lakhs = num / 100000; num = num - lakhs * 100000;
         thousands = num / 1000; num = num - thousands * 1000;
         hundreds = num / 100; num = num - hundreds * 100;
         if (num > 19)
         {
             tens = num / 10; num = num - tens * 10;
         }
         single = num;
         if (crore > 0)
         {
             if (crore > 19)
                 words += NumberToWord(crore) + "Crore ";
             else
                 words += strones[crore - 1] + " Crore ";
         }
         if (lakhs > 0)
         {
             if (lakhs > 19)
                 words += NumberToWord(lakhs) + "Lakh ";
             else
                 words += strones[lakhs - 1] + " Lakh ";
         }
         if (thousands > 0)
         {
             if (thousands > 19)
                 words += NumberToWord(thousands) + "Thousand ";
             else
                 words += strones[thousands - 1] + " Thousand ";
         }
         if (hundreds > 0)
             words += strones[hundreds - 1] + " Hundred ";
         if (tens > 0)
             words += strtens[tens - 2] + " ";
         if (single > 0)
             words += strones[single - 1] + " ";
         return words;
     }
     */
    protected int SpreadExcelHeight(string HeaderName)
    {
        int RowHeight = 20;
        try
        {
            if (!string.IsNullOrEmpty(HeaderName))
            {
                double DeductNameLength = 0;
                double.TryParse(Convert.ToString(HeaderName.Length), out DeductNameLength);
                if (DeductNameLength > 61)
                {
                    double rowHeight = (DeductNameLength / 61);
                    double RowHeightVal = Math.Round(rowHeight, 2) * 35;
                    RowHeight = Convert.ToInt32(Math.Round(RowHeightVal, 0));
                }
            }
        }
        catch
        {
            return RowHeight;
        }
        return RowHeight;
    }
}