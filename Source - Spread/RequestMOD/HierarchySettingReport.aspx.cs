using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Collections;
using AjaxControlToolkit;
using System.Configuration;
using System.Drawing;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using System.Web.UI.DataVisualization;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.DataVisualization.Charting.ChartTypes;
public partial class HierarchySettingReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string staffcodesession = "";
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    static string con_txt = "";
    static string pri_txt = "";
    static string mulicollg = "";
    static string name = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            string PageLogOut = "";
            string sess = Convert.ToString(Session["IsLogin"]);
            PageLogOut = Convert.ToString(Session["PageLogout"]);
            if (sess == "")
            {
            }
            else
            {
                if (!Request.FilePath.Contains("ReqM"))
                {
                    string strPreviousPage = "";
                    if (Request.UrlReferrer != null)
                    {
                        strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                    }
                    if (strPreviousPage == "")
                    {
                        string redrURI = ConfigurationManager.AppSettings["Request"].Trim();
                        Response.Redirect(redrURI, false);
                        return;
                    }
                }
            }
            if (Session["collegecode"] == null)
            {
                string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
                Response.Redirect(redrURI, false);
                return;
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            staffcodesession = Session["Staff_Code"].ToString();
            if (!IsPostBack)
            {
                BindReqName();
                BindCollege();
                bindstaffdept1();
                bind_stafType1();
                bindstaffdesg();
                loadstaffdep1(ddlcollege.SelectedItem.Value);
                bind_stafType2();
                bind_design1();
                loadfsstaff();
                mulicollg = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    #region Bind Methods

    public void BindReqName()
    {
        try
        {
            string query = "";
            string Master1 = "";
            //string[] reqname = { "Item Request", "Service", "Visitor Appointment", "Complaints", "Leave Request", "GatePass Request", "Event Request", "Payment Request", "Purchase Request" };
            //for (int i = 0; i < 9; i++)
            //{

            //    ddl_reqname.Items.Add(new ListItem(reqname[i], Convert.ToString(i + 1)));

            //}
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='Request Hierarchy Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='Request Hierarchy Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    for (int j = 0; j < split.Length; j++)
                    {
                        string v = Convert.ToString(split[j]);
                        requestname(v);
                        ddl_reqname.Items.Add(new System.Web.UI.WebControls.ListItem(name, Convert.ToString(v)));
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    public void BindCollege()
    {
        try
        {
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";

            ds = da.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollegestaff.DataSource = ds;
                ddlcollegestaff.DataTextField = "collname";
                ddlcollegestaff.DataValueField = "college_code";
                ddlcollegestaff.DataBind();
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindstaffdept1()
    {
        try
        {
            string query = "select distinct dept_code,dept_name from hrdept_master where college_code='" + ddlcollegestaff.SelectedValue.ToString() + "' order by dept_name";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chldeptstaff.DataSource = ds;
                chldeptstaff.DataTextField = "dept_name";
                chldeptstaff.DataValueField = "dept_code";
                chldeptstaff.DataBind();
                chkdeptstaff.Checked = true;
                if (chldeptstaff.Items.Count > 0)
                {
                    for (int i = 0; i < chldeptstaff.Items.Count; i++)
                    {
                        chldeptstaff.Items[i].Selected = true;
                    }
                    txtstaffDepart.Text = "Dept(" + chldeptstaff.Items.Count + ")";
                }

            }
        }

        catch (Exception ex)
        {
        }
    }

    public void bind_stafType1()
    {
        try
        {
            string query = "SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code=" + ddlcollegestaff.SelectedValue.ToString() + "";
            ds = da.select_method_wo_parameter(query, "Text");
            {
                chlstafftpyenew.Items.Clear();
                chlstafftpyenew.DataSource = ds;
                chlstafftpyenew.DataTextField = "StfType";
                chlstafftpyenew.DataValueField = "StfType";
                chlstafftpyenew.DataBind();
                chkstafftypenew.Checked = true;
                if (chlstafftpyenew.Items.Count > 0)
                {
                    for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
                    {
                        chlstafftpyenew.Items[i].Selected = true;
                    }
                    txtstaff_type.Text = "Staff Type(" + chlstafftpyenew.Items.Count + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindstaffdesg()
    {
        try
        {
            string itemheader = "";
            for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
            {
                if (chlstafftpyenew.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + chlstafftpyenew.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + chlstafftpyenew.Items[i].Value.ToString() + "";
                    }
                }
            }
            string query = "SELECT distinct Desig_Name,t.desig_code FROM StaffTrans T,staffmaster m,Desig_Master G WHERE t.staff_code = m.staff_code and T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + Session["collegecode"] + " and stftype in('" + itemheader + "') ";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklststaff.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklststaff.Items.Clear();
                    chklststaff.DataSource = ds;
                    chklststaff.DataTextField = "Desig_Name";
                    chklststaff.DataValueField = "desig_code";
                    chklststaff.DataBind();
                    chksatff.Checked = true;
                    if (chklststaff.Items.Count > 0)
                    {
                        for (int i = 0; i < chklststaff.Items.Count; i++)
                        {
                            chklststaff.Items[i].Selected = true;
                        }
                        txtstaff.Text = "Desig(" + chklststaff.Items.Count + ")";
                    }
                }
            }
            else
            {
                chksatff.Checked = false;
                for (int i = 0; i < chklststaff.Items.Count; i++)
                {
                    chklststaff.Items[i].Selected = false;

                }
                txtstaff.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bind_design1()
    {
        try
        {
            string sql = string.Empty;

            string itemheader = "";
            for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
            {
                if (cbl_staff_type111.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_staff_type111.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_staff_type111.Items[i].Value.ToString() + "";
                    }
                }
            }

            sql = "SELECT distinct Desig_Name FROM StaffTrans T,staffmaster m,Desig_Master G WHERE t.staff_code = m.staff_code and T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + Session["collegecode"] + " and stftype in('" + itemheader + "')";


            ds.Clear();
            ds = da.select_method_wo_parameter(sql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staff_desn11.DataSource = ds;
                cbl_staff_desn11.DataTextField = "Desig_Name";
                cbl_staff_desn11.DataValueField = "Desig_Name";
                cbl_staff_desn11.DataBind();
                if (cbl_staff_desn11.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staff_desn11.Items.Count; i++)
                    {
                        cbl_staff_desn11.Items[i].Selected = true;
                    }
                    txt_staff_desg111.Text = "Designation(" + cbl_staff_desn11.Items.Count + ")";
                    cb_staff_desn11.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    void bind_stafType2()
    {
        try
        {
            string srisql = "SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code='" + ddlcollege.SelectedItem.Value + "' ";
            ds.Clear();
            ds = da.select_method_wo_parameter(srisql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staff_type111.DataSource = ds;
                cbl_staff_type111.DataTextField = "StfType";
                cbl_staff_type111.DataValueField = "StfType";
                cbl_staff_type111.DataBind();
                if (cbl_staff_type111.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
                    {
                        cbl_staff_type111.Items[i].Selected = true;
                    }
                    txt_staff_type11.Text = "Staff Type(" + cbl_staff_type111.Items.Count + ")";
                    cb_staff_type111.Checked = true;
                }

            }
        }
        catch
        {
        }
    }

    public void loadstaffdep1(string collegecode)
    {
        try
        {
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);

            string srisql = "select distinct dept_name,dept_code from hrdept_master where college_code=" + collegecode + "";

            ds.Clear();
            ds = da.select_method_wo_parameter(srisql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staff_dept11.DataSource = ds;
                cbl_staff_dept11.DataTextField = "dept_name";
                cbl_staff_dept11.DataValueField = "dept_code";
                cbl_staff_dept11.DataBind();
                if (cbl_staff_dept11.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staff_dept11.Items.Count; i++)
                    {
                        cbl_staff_dept11.Items[i].Selected = true;
                    }
                    txt_staff_dept11.Text = "Dept(" + cbl_staff_dept11.Items.Count + ")";
                    cb_staff_dept11.Checked = true;
                }

            }
        }
        catch
        {
        }
    }

    #endregion

    public void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);


    }

    public void ddl_reqname_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlcollegestaff_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindstaffdept1();
    }

    public void requestname(string val)
    {
        if (val == "1")
        {
            name = "Item Request";
        }
        if (val == "2")
        {
            name = "Service";
        }
        if (val == "3")
        {
            name = "Visitor Appointment";
        }
        if (val == "4")
        {
            name = "Complaints";
        }
        if (val == "5")
        {
            name = "Leave Request";
        }
        if (val == "6")
        {
            name = "GatePass Request";
        }
        if (val == "7")
        {
            name = "Event Request";
        }
        if (val == "8")
        {
            name = "Payment Request";
        }
        if (val == "9")
        {
            name = "Purchase Request";
        }
        if (val == "10")
        {
            name = "Student Leave Request";
        }
        if (val == "11")
        {
            name = "Certificate Request";
        }
        if (val == "12")
        {
            name = "Inward Request";
        }
        if (val == "13")//sd8/19
        {
            name = "Book Request";
        }
        if (val == "14")//sd22/19
        {
            name = "Vacated Request";
        }
    }

    public void btn_maingo_Click(object sender, EventArgs e)
    {
        bindgo();
    }

    public void bindgo()
    {
        Printcontrol.Visible = false;
        string reqstaffname = "";
        string reqdept = "";
        string reqdesign = "";
        string Appstaffname = "";
        string Appdept = "";
        string Appdesign = "";
        string stage = "";
        string query = "";
        string itemheader = "";
        string designation = "";
        string dept = "";
        string staffcode = "";
        string appstaffcode = "";
        int count = 0;
        FarPoint.Web.Spread.StyleInfo darkstyle111 = new FarPoint.Web.Spread.StyleInfo();
        darkstyle111.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle111.ForeColor = System.Drawing.Color.Black;
        darkstyle111.HorizontalAlign = HorizontalAlign.Center;
        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle111;
        Fpspread1.Sheets[0].RowHeader.Visible = false;
        Fpspread1.CommandBar.Visible = false;
        Fpspread1.Sheets[0].AutoPostBack = false;
        Fpspread1.Sheets[0].RowCount = 0;
        for (int i = 0; i < chldeptstaff.Items.Count; i++)
        {
            if (chldeptstaff.Items[i].Selected == true)
            {
                if (dept == "")
                {
                    dept = "" + chldeptstaff.Items[i].Value.ToString() + "";
                }
                else
                {
                    dept = dept + "'" + "," + "" + "'" + chldeptstaff.Items[i].Value.ToString() + "";
                }
            }
        }
        if (dept == "")
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select Any Department";
            return;
        }
        for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
        {
            if (chlstafftpyenew.Items[i].Selected == true)
            {
                if (itemheader == "")
                {
                    itemheader = "" + chlstafftpyenew.Items[i].Value.ToString() + "";
                }
                else
                {
                    itemheader = itemheader + "'" + "," + "" + "'" + chlstafftpyenew.Items[i].Value.ToString() + "";
                }
            }
        }
        if (dept == "")
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select Any Staff Type";
            return;
        }
        for (int i = 0; i < chklststaff.Items.Count; i++)
        {
            if (chklststaff.Items[i].Selected == true)
            {
                if (designation == "")
                {
                    designation = "" + chklststaff.Items[i].Value.ToString() + "";
                }
                else
                {
                    designation = designation + "'" + "," + "" + "'" + chklststaff.Items[i].Value.ToString() + "";
                }
            }
        }
        if (dept == "")
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select Any Designation";
            return;
        }
        if (txtstaffDepart.Text != "---Select---" && txtstaff_type.Text == "---Select---" && txtstaff.Text == "---Select---")
        {
            query = "select * from RQ_RequestHierarchy where RequestType='" + ddl_reqname.SelectedItem.Value + "' and CollegeCode='" + ddlcollegestaff.SelectedItem.Value + "' order by ReqStaffAppNo,ReqApproveStage";
        }
        else
        {
            if (dept != "")
            {
                if (itemheader != "")
                {
                    if (designation != "")
                    {
                        if (ddl_reqname.SelectedItem.Value != "14")
                            query = "select  * from RQ_RequestHierarchy R,staffmaster s,staff_appl_master Sa,stafftrans t where s.appl_no =sa.appl_no  and r.ReqStaffAppNo =sa.appl_id and t.staff_code =s.staff_code and latestrec =1 and resign =0 and settled =0 and t.desig_code in('" + designation + "') and sa.dept_code in ('" + dept + "') and RequestType ='" + ddl_reqname.SelectedItem.Value + "' order by s.staff_code,ReqAppPriority,ReqApproveStage";
                        //(select appl_name  from staff_appl_master where appl_id =r.ReqAppStaffAppNo )
                        else
                            query = "select  * from RQ_RequestHierarchy R,staffmaster s,staff_appl_master Sa,stafftrans t where s.appl_no =sa.appl_no  and r.ReqAppStaffAppNo =sa.appl_id and t.staff_code =s.staff_code and latestrec =1 and resign =0 and settled =0 and t.desig_code in('" + designation + "') and sa.dept_code in ('" + dept + "') and RequestType ='" + ddl_reqname.SelectedItem.Value + "' order by s.staff_code,ReqAppPriority,ReqApproveStage";
                    }
                }
            }
        }

        ds = d2.select_method_wo_parameter(query, "Text");

        if (ds.Tables[0].Rows.Count > 0)
        {
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].ColumnCount = 11;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Column.Width = 44;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Requested Staff Code";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Column.Width = 80;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Requested Staff Name";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Column.Width = 130;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Requested Staff Department";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Column.Width = 120;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Requested Staff Designation";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Column.Width = 100;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Approval Staff Code";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Column.Width = 80;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Approval Staff Name";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Column.Width = 130;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Approval Staff Department";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Column.Width = 100;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Approval Staff Designation";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Column.Width = 100;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Stage";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Column.Width = 40;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Update";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Column.Width = 57;
            lbl_error_shown.Visible = false;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                staffcode = d2.GetFunction("select staff_code from staffmaster s,staff_appl_master sm where s.appl_no=sm.appl_no and sm.appl_id='" + Convert.ToString(ds.Tables[0].Rows[i]["ReqStaffAppNo"]) + "'");
                reqstaffname = d2.GetFunction("select appl_name from staff_appl_master where appl_id='" + Convert.ToString(ds.Tables[0].Rows[i]["ReqStaffAppNo"]) + "'");
                reqdept = d2.GetFunction("select s.dept_name as dept from staff_appl_master s,staffmaster m,stafftrans t,hrdept_master h,desig_master d where s.appl_no = m.appl_no and m.staff_code = t.staff_code and t.dept_code = h.dept_code and t.desig_code = d.desig_code and m.college_code = 13 and t.latestrec = 1 and m.resign = 0 and settled = 0 and s.appl_id='" + Convert.ToString(ds.Tables[0].Rows[i]["ReqStaffAppNo"]) + "'");
                reqdesign = d2.GetFunction("select d.desig_name as design from staff_appl_master s,staffmaster m,stafftrans t,hrdept_master h,desig_master d where s.appl_no = m.appl_no and m.staff_code = t.staff_code and t.dept_code = h.dept_code and t.desig_code = d.desig_code and m.college_code = 13 and t.latestrec = 1 and m.resign = 0 and settled = 0 and s.appl_id='" + Convert.ToString(ds.Tables[0].Rows[i]["ReqStaffAppNo"]) + "'");
                Appstaffname = d2.GetFunction("select appl_name from staff_appl_master where appl_id='" + Convert.ToString(ds.Tables[0].Rows[i]["ReqAppStaffAppNo"]) + "'");
                appstaffcode = d2.GetFunction("select staff_code from staffmaster s,staff_appl_master sm where s.appl_no=sm.appl_no and sm.appl_id='" + Convert.ToString(ds.Tables[0].Rows[i]["ReqAppStaffAppNo"]) + "'");
                Appdept = d2.GetFunction("select s.dept_name as dept from staff_appl_master s,staffmaster m,stafftrans t,hrdept_master h,desig_master d where s.appl_no = m.appl_no and m.staff_code = t.staff_code and t.dept_code = h.dept_code and t.desig_code = d.desig_code and m.college_code = 13 and t.latestrec = 1 and m.resign = 0 and settled = 0 and s.appl_id='" + Convert.ToString(ds.Tables[0].Rows[i]["ReqAppStaffAppNo"]) + "'");
                Appdesign = d2.GetFunction("select d.desig_name as design from staff_appl_master s,staffmaster m,stafftrans t,hrdept_master h,desig_master d where s.appl_no = m.appl_no and m.staff_code = t.staff_code and t.dept_code = h.dept_code and t.desig_code = d.desig_code and m.college_code = 13 and t.latestrec = 1 and m.resign = 0 and settled = 0 and s.appl_id='" + Convert.ToString(ds.Tables[0].Rows[i]["ReqAppStaffAppNo"]) + "'");
                string stagecount = Convert.ToString(ds.Tables[0].Rows[i]["ReqApproveStage"]);
                pri_txt = Convert.ToString(ds.Tables[0].Rows[i]["ReqAppPriority"]);
                if (Appdept == "0")
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "The Staff " + Appstaffname + " was Relived,Please Change The Hierarchy Setting";

                }
                if (reqdept == "0")
                {
                    reqdept = "";
                }
                if (reqdesign == "0")
                {
                    reqdesign = "";
                }
                abc();
                stage = stagecount + "-" + con_txt;
                Fpspread1.Sheets[0].RowCount++;
                count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Locked = true;


                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["RequestHierarchyPK"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Locked = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;


                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Locked = true;


                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Locked = true;


                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Locked = true;

                if (ddl_reqname.SelectedItem.Value != "14")
                {
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = staffcode;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = reqstaffname;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = reqdept;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = reqdesign;
                }
                else
                {
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = "";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = "";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = "";

                }
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = appstaffcode;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Locked = true;

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Appstaffname;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Locked = true;

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Appdept;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Locked = true;

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Appdesign;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ReqApproveStage"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Locked = true;

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = stage;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ReqAppPriority"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Locked = true;

                FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                btn.Text = "Update";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].CellType = btn;
            }
            Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            Fpspread1.Visible = true;
            div_report.Visible = true;
            Fpspread1.Width = 990;
            Fpspread1.Height = 500;
        }
        else
        {
            Fpspread1.Visible = false;
            lbl_error_shown.Visible = true;
            lbl_error_shown.Text = "No Records Found";
            div_report.Visible = false;
            return;
        }

    }

    protected void fpspread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = "";
        string activecol = "";
        activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
        activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
        string actrow = e.SheetView.ActiveRow.ToString();
        string actcol = e.SheetView.ActiveColumn.ToString();
        if (Convert.ToInt32(activecol) == 10)
        {
            int commcount = 0;

            popview.Visible = true;


            loadstaffdep1(Convert.ToString(ddlcollege.SelectedItem.Value));
            bind_stafType2();
            bind_design1();
            loadfsstaff();

        }
    }

    public void updatestaff()
    {
        string activerow = "";
        activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
        string reqnumb = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
        string pri = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Tag);
        string stage = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Tag);

        string activerow1 = fsstaff.ActiveSheetView.ActiveRow.ToString();
        if (Convert.ToInt32(activerow1.ToString()) > 1)
        {
            string name_active = fsstaff.Sheets[0].Cells[Convert.ToInt32(activerow1), 1].Text;
            string des_active = fsstaff.Sheets[0].Cells[Convert.ToInt32(activerow1), 2].Text;
            string appno = d2.GetFunction("select sm.appl_id from staff_appl_master sm, staffmaster m where sm.appl_no=m.appl_no and m.staff_code='" + des_active + "'");

            string update_query = "update RQ_RequestHierarchy set ReqAppStaffAppNo='" + appno + "' where RequestHierarchyPK='" + reqnumb + "' and CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
            int s = d2.update_method_wo_parameter(update_query, "Text");
            if (s == 1)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Updated Successfully";
                bindgo();
            }
        }

    }

    public void btn_popclose_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
    }

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        string collegcodee = Convert.ToString(ddlcollege.SelectedItem.Value);
        loadstaffdep1(collegcodee);
        bind_stafType2();
        bind_design1();
        fsstaff.Visible = false;
        lbl_totalstaffcount.Visible = false;
        mulicollg = Convert.ToString(ddlcollege.SelectedItem.Value);
    }

    public void cb_staff_dept11_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_staff_dept11.Text = "--Select--";
        if (cb_staff_dept11.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_staff_dept11.Items.Count; i++)
            {
                cbl_staff_dept11.Items[i].Selected = true;
            }
            txt_staff_dept11.Text = "Dept(" + (cbl_staff_dept11.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_staff_dept11.Items.Count; i++)
            {
                cbl_staff_dept11.Items[i].Selected = false;
            }
        }
    }

    public void cbl_staff_dept11_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_staff_dept11.Text = "--Select--";
        for (i = 0; i < cbl_staff_dept11.Items.Count; i++)
        {
            if (cbl_staff_dept11.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_staff_dept11.Checked = false;
            }
        }

        if (commcount > 0)
        {
            if (commcount == cbl_staff_dept11.Items.Count)
            {
                cb_staff_dept11.Checked = true;
            }
            txt_staff_dept11.Text = "Dept(" + commcount.ToString() + ")";
        }
    }

    public void cb_staff_type111_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_staff_type11.Text = "--Select--";
        if (cb_staff_type111.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
            {
                cbl_staff_type111.Items[i].Selected = true;
            }
            txt_staff_type11.Text = "Staff Type(" + (cbl_staff_type111.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
            {
                cbl_staff_type111.Items[i].Selected = false;
            }
        }
        bind_design1();
    }

    public void cb_staff_type111_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_staff_type11.Text = "--Select--";
        for (i = 0; i < cbl_staff_type111.Items.Count; i++)
        {
            if (cbl_staff_type111.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_staff_type111.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_staff_type111.Items.Count)
            {
                cb_staff_type111.Checked = true;
            }
            txt_staff_type11.Text = "Staff Type(" + commcount.ToString() + ")";
        }
        bind_design1();
    }

    public void cb_staff_desn11_CheckedChanged(object sender, EventArgs e)
    {

        int cout = 0;
        txt_staff_desg111.Text = "--Select--";
        if (cb_staff_desn11.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_staff_desn11.Items.Count; i++)
            {
                cbl_staff_desn11.Items[i].Selected = true;
            }
            txt_staff_desg111.Text = "Designation(" + (cbl_staff_desn11.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_staff_desn11.Items.Count; i++)
            {
                cbl_staff_desn11.Items[i].Selected = false;
            }
        }
    }

    public void cbl_staff_desn11_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_staff_desg111.Text = "--Select--";
        for (i = 0; i < cbl_staff_desn11.Items.Count; i++)
        {
            if (cbl_staff_desn11.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_staff_desn11.Checked = false;
            }
        }

        if (commcount > 0)
        {
            if (commcount == cbl_staff_desn11.Items.Count)
            {
                cb_staff_desn11.Checked = true;
            }
            txt_staff_desg111.Text = "Designation(" + commcount.ToString() + ")";
        }
    }

    public void fsstaff_CellClick(object sender, EventArgs e)
    {
    }

    public void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlstaff.SelectedItem.Value == "0")
        {
            txt_search.Visible = true;
            txt_search1.Visible = false;
            txt_search1.Text = "";
        }
        else
        {
            txt_search.Visible = false;
            txt_search1.Visible = true;
            txt_search.Text = "";
        }
        loadfsstaff();
    }

    protected void loadfsstaff()
    {
        try
        {
            //.Sheets[0].c
            ermsg.Visible = false;
            string sql = "";
            fsstaff.Sheets[0].RowCount = 0;
            fsstaff.SaveChanges();
            fsstaff.Visible = true;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
            fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);

            string bindspread = sql;
            string itemheader = "";
            string designation = "";
            string dept = "";
            for (int i = 0; i < cbl_staff_dept11.Items.Count; i++)
            {
                if (cbl_staff_dept11.Items[i].Selected == true)
                {
                    if (dept == "")
                    {
                        dept = "" + cbl_staff_dept11.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        dept = dept + "'" + "," + "" + "'" + cbl_staff_dept11.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
            {
                if (cbl_staff_type111.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_staff_type111.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_staff_type111.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_staff_desn11.Items.Count; i++)
            {
                if (cbl_staff_desn11.Items[i].Selected == true)
                {
                    if (designation == "")
                    {
                        designation = "" + cbl_staff_desn11.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        designation = designation + "'" + "," + "" + "'" + cbl_staff_desn11.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (txt_search.Text != "" || txt_search1.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search1.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
                }
                else if (ddlcollege.SelectedIndex != -1)
                {
                    sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
                }

                else
                {
                    sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0";

                }
            }
            else
            {
                if (dept != "")
                {
                    if (itemheader != "")
                    {
                        if (designation != "")
                        {

                            sql = "select distinct s.staff_code,s.staff_name,appl_id,h.dept_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and h.dept_code in('" + dept + "')  and d.desig_name in('" + designation + "') and s.college_code='" + ddlcollege.SelectedValue.ToString() + "'   and stftype in('" + itemheader + "') and resign = 0 and settled = 0 and latestrec=1";
                        }
                        else
                        {
                            fsstaff.Visible = false;
                            ermsg.Visible = true;
                            ermsg.Text = "Select Any Designation";
                            lbl_totalstaffcount.Visible = false;
                            btnstaffadd.Visible = false;
                            btnexitpop.Visible = false;
                        }
                    }
                    else
                    {
                        fsstaff.Visible = false;
                        ermsg.Visible = true;
                        ermsg.Text = "Select Any Staff Type";
                        lbl_totalstaffcount.Visible = false;
                        btnstaffadd.Visible = false;
                        btnexitpop.Visible = false;
                    }
                }
                else
                {
                    fsstaff.Visible = false;
                    ermsg.Visible = true;
                    ermsg.Text = "Select Any Department";
                    lbl_totalstaffcount.Visible = false;
                    btnstaffadd.Visible = false;
                    btnexitpop.Visible = false;
                }
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(sql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                fsstaff.Sheets[0].AutoPostBack = true;
                fsstaff.CommandBar.Visible = false;

                FarPoint.Web.Spread.StyleInfo darkstyle111 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle111.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle111.ForeColor = System.Drawing.Color.Black;
                darkstyle111.HorizontalAlign = HorizontalAlign.Center;
                fsstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle111;

                fsstaff.Sheets[0].AllowTableCorner = true;
                fsstaff.Sheets[0].RowHeader.Visible = false;

                //fsstaff.Sheets[0].AutoPostBack = true;
                fsstaff.Sheets[0].ColumnCount = 3;
                fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Name";
                fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Code";

                fsstaff.Sheets[0].Columns[0].Width = 80;
                fsstaff.Sheets[0].Columns[1].Width = 200;
                fsstaff.Sheets[0].Columns[2].Width = 100;

                fsstaff.Sheets[0].Columns[0].Locked = true;
                fsstaff.Sheets[0].Columns[1].Locked = true;
                fsstaff.Sheets[0].Columns[2].Locked = true;
                int sno = 0;
                lbl_totalstaffcount.Visible = true;
                lbl_totalstaffcount.Text = "Total Staff Count: " + Convert.ToString(ds.Tables[0].Rows.Count);
                for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    string name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    string code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();
                    fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
                    fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = name;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = code;
                    fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    fsstaff.Sheets[0].AutoPostBack = false;
                }
                int rowcount = fsstaff.Sheets[0].RowCount;
                fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                fsstaff.SaveChanges();
                lbl_totalstaffcount.Visible = true;
                btnstaffadd.Visible = true;
                btnexitpop.Visible = true;
            }
            else
            {

                fsstaff.Visible = false;
                ermsg.Visible = true;
                ermsg.Text = "No Records Found";
                lbl_totalstaffcount.Visible = false;
                btnstaffadd.Visible = false;
                btnexitpop.Visible = false;

            }
            txt_search.Text = "";
            txt_search1.Text = "";
        }
        catch
        {
        }
    }

    public void btnstaffadd_Click(object sender, EventArgs e)
    {
        updatestaff();
    }

    public void exitpop_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
    }

    public void btn_gostaff_Click(object sender, EventArgs e)
    {
        loadfsstaff();
    }

    public void txt_search_TextChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }

    public void txt_search1_TextChanged(object sender, EventArgs e)
    {
        loadfsstaff();
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffname1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";


        name = ws.Getname(query);

        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffcode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and s.college_code='" + mulicollg + "' and resign =0 and s.staff_code like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";


        name = ws.Getname(query);

        return name;
    }

    public void abc()
    {
        if (pri_txt == "1")
        {
            con_txt = "A";
        }
        if (pri_txt == "2")
        {
            con_txt = "B";
        }
        if (pri_txt == "3")
        {
            con_txt = "C";
        }
        if (pri_txt == "4")
        {
            con_txt = "D";
        }
        if (pri_txt == "5")
        {
            con_txt = "E";
        }
        if (pri_txt == "6")
        {
            con_txt = "F";
        }
        if (pri_txt == "7")
        {
            con_txt = "G";
        }
        if (pri_txt == "8")
        {
            con_txt = "H";
        }
        if (pri_txt == "9")
        {
            con_txt = "I";
        }
        if (pri_txt == "10")
        {
            con_txt = "J";
        }
        if (pri_txt == "11")
        {
            con_txt = "K";
        }
        if (pri_txt == "12")
        {
            con_txt = "L";
        }
        if (pri_txt == "13")
        {
            con_txt = "M";
        }
        if (pri_txt == "14")
        {
            con_txt = "N";
        }
        if (pri_txt == "15")
        {
            con_txt = "O";
        }
        if (pri_txt == "16")
        {
            con_txt = "P";
        }
        if (pri_txt == "17")
        {
            con_txt = "Q";
        }
        if (pri_txt == "18")
        {
            con_txt = "R";
        }
        if (pri_txt == "19")
        {
            con_txt = "S";
        }
        if (pri_txt == "20")
        {
            con_txt = "T";
        }
        if (pri_txt == "21")
        {
            con_txt = "U";
        }
        if (pri_txt == "22")
        {
            con_txt = "V";
        }
        if (pri_txt == "23")
        {
            con_txt = "W";
        }
        if (pri_txt == "24")
        {
            con_txt = "X";
        }
        if (pri_txt == "25")
        {
            con_txt = "Y";
        }
        if (pri_txt == "26")
        {
            con_txt = "Z";
        }
    }

    protected void chkdeptstaff_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdeptstaff.Checked == true)
        {
            for (int i = 0; i < chldeptstaff.Items.Count; i++)
            {
                chldeptstaff.Items[i].Selected = true;
                txtstaffDepart.Text = "Dept (" + (chldeptstaff.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chldeptstaff.Items.Count; i++)
            {
                chldeptstaff.Items[i].Selected = false;
                txtstaffDepart.Text = "---Select---";
            }
        }
    }

    protected void chldeptstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        chkdeptstaff.Checked = false;
        for (int i = 0; i < chldeptstaff.Items.Count; i++)
        {
            if (chldeptstaff.Items[i].Selected == true)
            {
                batchcount = batchcount + 1;
            }
        }
        if (batchcount > 0)
        {
            txtstaffDepart.Text = "Dept (" + batchcount.ToString() + ")";
            if (batchcount == chldeptstaff.Items.Count)
            {
                chkdeptstaff.Checked = true;
            }
        }
        else
        {
            txtstaffDepart.Text = "---Select---";
        }
    }

    protected void chkstafftypenew_CheckedChanged(object sender, EventArgs e)
    {
        if (chkstafftypenew.Checked == true)
        {
            for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
            {
                chlstafftpyenew.Items[i].Selected = true;
                txtstaff_type.Text = "Type (" + (chlstafftpyenew.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
            {
                chlstafftpyenew.Items[i].Selected = false;
                txtstaff_type.Text = "---Select---";
            }
        }
        bindstaffdesg();
    }

    protected void chlstafftpyenew_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        chkstafftypenew.Checked = false;
        for (int i = 0; i < chlstafftpyenew.Items.Count; i++)
        {
            if (chlstafftpyenew.Items[i].Selected == true)
            {
                batchcount = batchcount + 1;

            }
        }
        bindstaffdesg();
        if (batchcount > 0)
        {
            txtstaff_type.Text = "Type (" + batchcount.ToString() + ")";
            if (batchcount == chlstafftpyenew.Items.Count)
            {
                chkstafftypenew.Checked = true;
            }
        }
        else
        {
            txtstaff_type.Text = "---Select---";
        }
    }

    protected void chksatff_CheckedChanged(object sender, EventArgs e)
    {
        if (chksatff.Checked == true)
        {
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                chklststaff.Items[i].Selected = true;
                txtstaff.Text = "Desig (" + (chklststaff.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                chklststaff.Items[i].Selected = false;
                txtstaff.Text = "---Select---";
            }
        }
    }

    protected void chklststaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        chksatff.Checked = false;
        for (int i = 0; i < chklststaff.Items.Count; i++)
        {
            if (chklststaff.Items[i].Selected == true)
            {
                batchcount = batchcount + 1;
            }
        }
        if (batchcount > 0)
        {
            txtstaff.Text = "Desig (" + batchcount.ToString() + ")";
            if (batchcount == chklststaff.Items.Count)
            {
                chksatff.Checked = true;
            }
        }
        else
        {
            txtstaff.Text = "---Select---";
        }
    }

    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
            if (txt_excelname.Text == "")
            {
                lbl_norec.Visible = true;
            }
            else
            {
                lbl_norec.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                lbl_norec.Visible = false;
                d2.printexcelreport(Fpspread1, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }

        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }

    }

    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "HierarchySetting Report";
            string pagename = "HierarchySettingReport.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, attendance);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
}