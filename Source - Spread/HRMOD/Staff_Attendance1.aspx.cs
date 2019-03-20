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
// Last Modified By Jeyaprakash Add Holiday Updation on Apr 13th,2017
public partial class Staff_Attendance1 : System.Web.UI.Page
{
    static string clgcode1 = string.Empty;
    string college = "";
    string usercode = "";
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable hat1 = new Hashtable();
    Hashtable hascount = new Hashtable();
    string capvalue = "";
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static Hashtable newhash = new Hashtable();
    ArrayList holidaydate = new ArrayList();
    ArrayList leave = new ArrayList();
    static ArrayList arrHolDays = new ArrayList();
    Boolean flag_true = false;
    Boolean count = false;
    int sunday_flage_add = 0;
    int holiday_flage_add = 0;
    static Boolean leave_apply_flage;
    static bool conformationflage;
    static int leave_apply = 0;
    bool dateflag = false;
    int countadd = 0;
    double present = 0;
    double absent = 0;
    static int flag_updatesave = 0;
    int height = 0;
    string[] sarray1;
    string[] sarray2 = new string[5];
    string[] sarray3;
    string[] sarray4;
    string[] sarray5 = new string[5];
    FarPoint.Web.Spread.ComboBoxCellType cb1 = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType cb3 = new FarPoint.Web.Spread.ComboBoxCellType();//delsi

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.MaintainScrollPositionOnPostBack = true;
            //if (Session["collegecode"] == null)
            //{
            //    Response.Redirect("~/Default.aspx");
            //}
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
            college = Session["collegecode"].ToString();
            lblerror3.Visible = false;
            lblError.Visible = false;
            if (clgcode1 == "")
            {
                if (ddlcollege.Items.Count > 0)
                    clgcode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            if (!IsPostBack)
            {
                bindcollege();
                binddept();
                binddesig(college);
                bindstaffcata(college);
                bindstafftype(college);
                txtfromdate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");
                txtfromdate.Text = System.DateTime.Now.ToString("d/MM/yyyy");
                txttodate.Text = System.DateTime.Now.ToString("d/MM/yyyy");
                newhash.Clear();
                FpSpread2.Sheets[0].AutoPostBack = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.Font.Bold = true;
                FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpread2.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread2.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread2.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                FpSpread2.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread2.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].RowHeader.Visible = false;
                FpSpread2.Visible = false;
                FpSpread2.Sheets[0].ColumnHeader.RowCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 0;
                FpSpread2.Sheets[0].RowCount = 0;
                //ItemList.Clear();
                //Itemindex.Clear();
                ddlleavereason.Attributes.Add("onfocus", "reason()");
                loadleatype();
                bindsearchstapp();
                txt_StaffCode.Visible = false;
                txt_staffname.Visible = false;
            }
            calcutale1();
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
            //  ddlcollege.Items.Insert(0, "---Select---");
        }
        catch (Exception e)
        {
        }
    }
    public void binddept()
    {
        try
        {
            int height = 0;

            cbl_Department.Items.Clear();
            ds.Clear();
            string group_user = "";
            string cmd = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + ddlcollege.SelectedItem.Value + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + ddlcollege.SelectedItem.Value + "') order by dept_name";
            }


            ds = d2.select_method_wo_parameter(cmd, "text");

            // ds = d2.loaddepartment(scollege);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Department.DataSource = ds;
                cbl_Department.DataTextField = "dept_name";
                cbl_Department.DataValueField = "Dept_Code";
                cbl_Department.DataBind();
            }
            for (int i = 0; i < cbl_Department.Items.Count; i++)
            {
                cbl_Department.Items[i].Selected = true;
                height++;
            }
            txt_Department.Text = "Department(" + cbl_Department.Items.Count + ")";
            cb_Department.Checked = true;
            if (height > 10)
            {
                panel_Department.Height = 300;
            }
            else
            {
                panel_Department.Height = 150;
            }
        }
        catch (Exception e)
        {
        }
    }
    public void binddesig(string coll)
    {
        try
        {
            height = 0;
            cbl_Designation.Visible = true;
            cbl_Designation.Items.Clear();
            ds.Clear();
            string col = coll;
            if (col == "---Select---")
            {
                col = Session["collegecode"].ToString();
            }
            txt_designation.Text = "---Select---";
            cb_Designation.Checked = false;
            ds = d2.loaddesignation(col);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Designation.DataSource = ds;
                cbl_Designation.DataTextField = "desig_name";
                cbl_Designation.DataValueField = "Desig_Code";
                cbl_Designation.DataBind();
                for (int i = 0; i < cbl_Designation.Items.Count; i++)
                {
                    cbl_Designation.Items[i].Selected = true;
                    height++;
                }
                txt_designation.Text = "Designation(" + cbl_Designation.Items.Count + ")";
                cb_Designation.Checked = true;
            }
            if (height > 10)
            {
                panel_Designation.Height = 300;
            }
            else
            {
                panel_Designation.Height = 150;
            }
        }
        catch (Exception e)
        {
        }
    }
    public void bindstaffcata(string college)
    {
        try
        {
            txt_Category.Text = "---Select---";
            cb_Category.Checked = false;
            string collvalue = college;
            if (collvalue == "---Select---")
            {
                collvalue = Session["collegecode"].ToString();
            }
            height = 0;
            cbl_Category.Items.Clear();
            ds.Clear();
            ds = d2.loadcategory(collvalue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Category.DataSource = ds;
                cbl_Category.DataTextField = "category_name";
                cbl_Category.DataValueField = "Category_Code";
                cbl_Category.DataBind();
                for (int i = 0; i < cbl_Category.Items.Count; i++)
                {
                    cbl_Category.Items[i].Selected = true;
                    height++;
                }
                txt_Category.Text = "Category(" + cbl_Category.Items.Count + ")";
                cb_Category.Checked = true;
            }
            if (height > 10)
            {
                panel_Category.Height = 300;
            }
            else
            {
                panel_Category.Height = 150;
            }
        }
        catch (Exception)
        {
        }
    }
    public void bindstafftype(string college)
    {
        try
        {
            txt_stafftype.Text = "---Select---";
            cbstafftype.Checked = false;
            string collvalue = college;
            if (collvalue == "---Select---")
            {
                collvalue = Session["collegecode"].ToString();
            }
            height = 0;
            cblstafftype.Items.Clear();
            ds.Clear();
            ds = d2.loadstafftype(collvalue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstafftype.DataSource = ds;
                cblstafftype.DataTextField = "StfType";
                cblstafftype.DataValueField = "StfType";
                cblstafftype.DataBind();
                for (int i = 0; i < cblstafftype.Items.Count; i++)
                {
                    cblstafftype.Items[i].Selected = true;
                    height++;
                }
                txt_stafftype.Text = "Staff Type(" + cblstafftype.Items.Count + ")";
                cbstafftype.Checked = true;
            }
            if (height > 10)
            {
                panel_stafftype.Height = 300;
            }
            else
            {
                panel_stafftype.Height = 150;
            }
        }
        catch (Exception)
        {
        }
    }
    public void loadleatype()
    {
        try
        {
            chklsattendance.Items.Clear();
            chkattendance.Checked = false;
            string leavequery = "";
            string groupcode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupcode = group_semi[0].ToString();
                }
                leavequery = "select Rights from Staff_Attendance_Setting where group_code=" + groupcode + "";
            }
            else
            {
                leavequery = "select Rights from Staff_Attendance_Setting where usercode ='" + Session["usercode"] + "'";
            }
            Hashtable hatle = new Hashtable();
            chklsattendance.Items.Add("P");
            chklsattendance.Items.Add("A");
            chklsattendance.Items.Add("PER");
            chklsattendance.Items.Add("OD");
            chklsattendance.Items.Add("LA");
            chklsattendance.Items.Add("RL");
            chklsattendance.Items.Add("NA");
            chklsattendance.Items.Add("H");
            hatle.Add("P", "P");
            hatle.Add("A", "A");
            hatle.Add("PER", "PER");
            hatle.Add("OD", "OD");
            hatle.Add("LA", "LA");
            hatle.Add("RL", "RL");
            hatle.Add("NA", "NA");
            hatle.Add("H", "H");
            ds.Clear();
            ds = d2.select_method_wo_parameter(leavequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Rights"].ToString().Trim() != "Empty")
                {
                    leave_apply_flage = true;
                    for (int leavecode = 0; leavecode < ds.Tables[0].Rows.Count; leavecode++)
                    {
                        string leavetype = Convert.ToString(ds.Tables[0].Rows[leavecode]["Rights"]);
                        if (leavetype != "")
                        {
                            string[] splitleave = leavetype.Split(',');
                            for (int split = 0; split <= splitleave.GetUpperBound(0); split++)
                            {
                                if (splitleave[split].Trim() != "" && splitleave[split] != null)
                                {
                                    if (!hatle.Contains(splitleave[split]))
                                    {
                                        chklsattendance.Items.Add(splitleave[split]);
                                        hatle.Add(splitleave[split], splitleave[split]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void bindleavetype()
    {
        try
        {
            sarray1 = new string[9];
            sarray3 = new string[9];
            sarray4 = new string[9];
            sarray1[0] = "";
            sarray3[0] = "";
            sarray4[0] = "";
            string leavequery = "";
            string groupcode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupcode = group_semi[0].ToString();
                }
                leavequery = "select Rights from Staff_Attendance_Setting where group_code=" + groupcode + "";
            }
            else
            {
                leavequery = "select Rights from Staff_Attendance_Setting where usercode ='" + Session["usercode"] + "'";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(leavequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Rights"].ToString().Trim() != "Empty")
                {
                    leave_apply_flage = true;
                    int length = 9;
                    for (int leavecode = 0; leavecode < ds.Tables[0].Rows.Count; leavecode++)
                    {
                        string leavetype = Convert.ToString(ds.Tables[0].Rows[leavecode]["Rights"]);
                        if (leavetype != "")
                        {
                            string[] splitleave = leavetype.Split(',');
                            sarray1 = new string[10 + splitleave.Length];//26.09.17
                            sarray3 = new string[10 + splitleave.Length];
                            sarray4 = new string[10 + splitleave.Length];
                            sarray1[0] = "Select For All";
                            sarray1[1] = " ";
                            sarray1[2] = "P";
                            sarray1[3] = "A";
                            sarray1[4] = "PER";
                            sarray1[5] = "OD";
                            //sarray1[6] = "RES";
                            sarray1[6] = "LA";
                            sarray1[7] = "RL";
                            sarray1[8] = "NA";
                            sarray1[9] = "H";
                            sarray3[0] = "";
                            sarray3[1] = "  ";
                            sarray3[2] = "P";
                            sarray3[3] = "A";
                            sarray3[4] = "PER";
                            sarray3[5] = "OD";
                            // sarray3[6] = "RES";
                            sarray3[6] = "LA";
                            sarray3[7] = "RL";
                            sarray3[8] = "NA";
                            sarray3[9] = "H";
                            sarray4[0] = "";
                            sarray4[1] = "  ";
                            sarray4[2] = "P";
                            sarray4[3] = "A";
                            sarray4[4] = "PER";
                            sarray4[5] = "OD";
                            // sarray4[6] = "RES";
                            sarray4[6] = "LA";
                            sarray4[7] = "RL";
                            sarray4[8] = "NA";
                            sarray4[9] = "H";
                            for (int split = 0; split <= splitleave.GetUpperBound(0); split++)
                            {
                                length++;
                                if (sarray1.Contains(splitleave[split].ToString()) == false)
                                {
                                    sarray1[length] = splitleave[split].ToString();//26.09.17 barath
                                    sarray3[length] = splitleave[split].ToString();
                                    sarray4[length] = splitleave[split].ToString();
                                }
                            }
                            cb1 = new FarPoint.Web.Spread.ComboBoxCellType(sarray4);
                            cb1.AutoPostBack = true;
                            cb1.UseValue = true;
                            cb1.ShowButton = true;
                        }
                    }
                }
                else
                {
                    sarray1[0] = "Select For All";
                    sarray1[1] = " ";
                    sarray1[2] = "P";
                    sarray1[3] = "A";
                    sarray1[4] = "PER";
                    sarray1[5] = "OD";
                    //sarray1[6] = "RES";
                    sarray1[6] = "LA";
                    sarray1[7] = "RL";
                    sarray1[8] = "NA";
                    sarray3[0] = "";
                    sarray3[1] = "  ";
                    sarray3[2] = "P";
                    sarray3[3] = "A";
                    sarray3[4] = "PER";
                    sarray3[5] = "OD";
                    // sarray3[6] = "RES";
                    sarray3[6] = "LA";
                    sarray3[7] = "RL";
                    sarray3[8] = "NA";
                    sarray4[0] = "";
                    sarray4[1] = "  ";
                    sarray4[2] = "P";
                    sarray4[3] = "A";
                    sarray4[4] = "PER";
                    sarray4[5] = "OD";
                    // sarray4[6] = "RES";
                    sarray4[6] = "LA";
                    sarray4[7] = "RL";
                    sarray4[8] = "NA";
                    cb1 = new FarPoint.Web.Spread.ComboBoxCellType(sarray4);
                    cb1.AutoPostBack = true;
                    cb1.UseValue = true;
                    cb1.ShowButton = false;
                }
            }
            else
            {
                sarray1[0] = "Select For All";
                sarray1[1] = " ";
                sarray1[2] = "P";
                sarray1[3] = "A";
                sarray1[4] = "PER";
                sarray1[5] = "OD";
                //sarray1[6] = "RES";
                sarray1[6] = "LA";
                sarray1[7] = "RL";
                sarray1[8] = "NA";
                sarray3[0] = "";
                sarray3[1] = "  ";
                sarray3[2] = "P";
                sarray3[3] = "A";
                sarray3[4] = "PER";
                sarray3[5] = "OD";
                // sarray3[6] = "RES";
                sarray3[6] = "LA";
                sarray3[7] = "RL";
                sarray3[8] = "NA";
                sarray4[0] = "";
                sarray4[1] = "  ";
                sarray4[2] = "P";
                sarray4[3] = "A";
                sarray4[4] = "PER";
                sarray4[5] = "OD";
                // sarray4[6] = "RES";
                sarray4[6] = "LA";
                sarray4[7] = "RL";
                sarray4[8] = "NA";
                cb1 = new FarPoint.Web.Spread.ComboBoxCellType(sarray4);
                cb1.AutoPostBack = true;
                cb1.UseValue = true;
                cb1.ShowButton = false;
            }
        }
        catch
        {
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        string coll = ddlcollege.SelectedItem.Value.ToString();
        binddept();
        binddesig(coll);
        bindstafftype(coll);
        bindstaffcata(coll);
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
                txt_Department.Text = "---Select---";
            }
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
        try
        {
            FpSpread2.Visible = false;
            panelsecond.Visible = false;
            lblreasonleave.Visible = false;
            ddlleavereason.Visible = false;
            btnselectall.Visible = false;
            btndeselectall.Visible = false;
            btnsave.Visible = false;
            btnupdate.Visible = false;
            lblerror1.Visible = false;
            string[] spf = txtfromdate.Text.ToString().Split('/');
            string[] spt = txttodate.Text.ToString().Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            DateTime dtnow = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
            if (dtf > dtt)
            {
                lblError.Visible = true;
                lblError.Text = "From Date Should Be Less Than To Date";
            }
            // poomalar 29.11.17
            int Settings = 0;
            string usercode = Session["usercode"].ToString();
            int.TryParse(Convert.ToString(d2.GetFunction(" select LinkValue from New_InsSettings where LinkName='Allow Future Attendance' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'")), out Settings);
            if (Settings != 1) // if added by poomalar 29.11.17
            {
                if (dtf > dtnow)
                {
                    lblError.Visible = true;
                    lblError.Text = "You Can Not Mark Attendance For The Date Greater Than Today";
                }
                if (dtt > dtnow)
                {
                    lblError.Visible = true;
                    lblError.Text = "You Can Not Mark Attendance For The Date Greater Than Today";
                }
            }
        }
        catch
        {
        }
    }
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.Visible = false;
            panelsecond.Visible = false;
            lblreasonleave.Visible = false;
            ddlleavereason.Visible = false;
            btnselectall.Visible = false;
            btndeselectall.Visible = false;
            btnsave.Visible = false;
            btnupdate.Visible = false;
            lblerror1.Visible = false;
            string[] spf = txtfromdate.Text.ToString().Split('/');
            string[] spt = txttodate.Text.ToString().Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            DateTime dtnow = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
            if (dtf > dtt)
            {
                lblError.Visible = true;
                lblError.Text = "From Date Should Be Less Than To Date";
            }
            // poomalar 29.11.17
            int Settings = 0;
            string usercode = Session["usercode"].ToString();
            int.TryParse(Convert.ToString(d2.GetFunction(" select LinkValue from New_InsSettings where LinkName='Allow Future Attendance' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'")), out Settings);
            if (Settings != 1) // if added by poomalar 29.11.17
            {
                if (dtf > dtnow)
                {
                    lblError.Visible = true;
                    lblError.Text = "You Can Not Mark Attendance For The Date Greater Than Today";
                }
                if (dtt > dtnow)
                {
                    lblError.Visible = true;
                    lblError.Text = "You Can Not Mark Attendance For The Date Greater Than Today";
                }
            }
        }
        catch
        {
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
                if (cblcolumnorder.Items[i].Text != "Reason")
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Add(si);
                }
                cblcolumnorder.Items[i].Selected = true;
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
        index = Convert.ToInt32(checkedBox[checkedBox.Length - 1]);
        string sindex = Convert.ToString(index);
        if (cblcolumnorder.Items[index].Selected)
        {
            if (!Itemindex.Contains(sindex))
            {
                if (cblcolumnorder.Items[index].Text != "Reason")
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
        Cbcolumn.Checked = false;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        bindreason();
        //bindleavetype();
        loardspread();
    }
    public void loardspread()
    {
        try
        {
            arrHolDays.Clear();
            Boolean noofstaffflag = false;
            Boolean checkreason = false;
            int spancolumn = 0;
            int nofostaff = 0;
            if (ddlcollege.Text != "---Select---")
            {
                bindleavetype();
                hat.Clear();
                for (int col = 0; col < cblcolumnorder.Items.Count; col++)
                {
                    if (cblcolumnorder.Items[col].Selected == true)
                    {
                        if (cblcolumnorder.Items[col].Text != "Reason")//delsi 1407
                        {
                            string value = cblcolumnorder.Items[col].Text;
                            if (ItemList.Contains(value) == false)
                            {
                                ItemList.Add(value.ToString());
                            }
                        }
                        if (cblcolumnorder.Items[col].Text == "Reason")
                        {
                            checkreason = true;
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
                    attendencequery = "select m.staff_code, " + invalue + " from staffmaster m,stafftrans t,desig_master d,hrdept_master h,staffcategorizer s where  t.staff_code=m.staff_code  and t.desig_code=d.desig_code and h.dept_code=t.dept_code and s.category_code=t.category_code and m.college_code = d.collegeCode and m.college_code = h.college_code and s.college_code = m.college_code   and t.latestrec = 1 and ((resign=0 and settled =0) and (Discontinue =0 or Discontinue is null))";
                    attendencequery = attendencequery + " and m.college_code in('" + ddlcollege.SelectedItem.Value.ToString() + "')";
                    string deptvalue = "";
                    if (cbl_Department.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_Department.Items.Count; i++)
                        {
                            if (cbl_Department.Items[i].Selected == true)
                            {
                                string value = cbl_Department.Items[i].Value;
                                if (deptvalue == "")
                                {
                                    deptvalue = value;
                                }
                                else
                                {
                                    deptvalue = deptvalue + "'" + "," + "'" + value;
                                }
                            }
                        }
                    }
                    if (deptvalue != "")
                    {
                        attendencequery = attendencequery + " and h.dept_code in('" + deptvalue + "')";
                    }
                    string desigvalue = "";
                    if (cbl_Designation.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_Designation.Items.Count; i++)
                        {
                            if (cbl_Designation.Items[i].Selected == true)
                            {
                                string value = cbl_Designation.Items[i].Value;
                                if (desigvalue == "")
                                {
                                    desigvalue = value;
                                }
                                else
                                {
                                    desigvalue = desigvalue + "'" + "," + "'" + value;
                                }
                            }
                        }
                    }
                    if (desigvalue != "")
                    {
                        attendencequery = attendencequery + " and d.desig_code in('" + desigvalue + "')";
                    }
                    string catevalue = "";
                    if (cbl_Category.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_Category.Items.Count; i++)
                        {
                            if (cbl_Category.Items[i].Selected == true)
                            {
                                string value = cbl_Category.Items[i].Value;
                                if (catevalue == "")
                                {
                                    catevalue = value;
                                }
                                else
                                {
                                    catevalue = catevalue + "'" + "," + "'" + value;
                                }
                            }
                        }
                    }
                    if (catevalue != "")
                    {
                        attendencequery = attendencequery + " and s.category_code in('" + catevalue + "')";
                    }
                    string stypevalue = "";
                    if (cblstafftype.Items.Count > 0)
                    {
                        for (int i = 0; i < cblstafftype.Items.Count; i++)
                        {
                            if (cblstafftype.Items[i].Selected == true)
                            {
                                string value = cblstafftype.Items[i].Value;
                                if (stypevalue == "")
                                {
                                    stypevalue = value;
                                }
                                else
                                {
                                    stypevalue = stypevalue + "'" + "," + "'" + value;
                                }
                            }
                        }
                    }
                    if (stypevalue != "")
                    {
                        attendencequery = attendencequery + " and t.stftype in('" + stypevalue + "')";
                    }
                    string Staffcode = string.Empty;//delsi01/05/2018
                    Staffcode = Convert.ToString(txt_StaffCode.Text);
                    string StfName = string.Empty;
                    StfName = Convert.ToString(txt_staffname.Text);
                    if (!string.IsNullOrEmpty(Staffcode))
                        attendencequery = attendencequery + " and m.staff_code in('" + Staffcode + "')";
                    if (!string.IsNullOrEmpty(StfName))
                        attendencequery = attendencequery + " and staff_name in('" + StfName + "')";

                    if (attendencequery != "")
                    {
                        attendencequery = attendencequery + " order by h.priority,d.priority,LEN(T.STAFF_CODE),T.STAFF_CODE";//LEN(T.STAFF_CODE),T.STAFF_CODE, order by h.priority,d.priority,join_date
                    }


                    Boolean attflag = false;
                    Dictionary<string, string> dicattval = new Dictionary<string, string>();
                    if (txtattendance.Text.ToString() != "---Select---")
                    {
                        for (int atywe = 0; atywe < chklsattendance.Items.Count; atywe++)
                        {
                            if (chklsattendance.Items[atywe].Selected == true)
                            {
                                string aytb = chklsattendance.Items[atywe].Text.ToString().Trim().ToLower();
                                if (!dicattval.ContainsKey(aytb))
                                {
                                    dicattval.Add(aytb, aytb);
                                    attflag = true;
                                }
                            }
                        }
                    }
                    if (attendencequery != "")
                    {
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(attendencequery, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            panelsecond.Visible = false;
                            FpSpread2.Sheets[0].AutoPostBack = false;
                            FpSpread2.Sheets[0].ColumnCount = 0;
                            FpSpread2.Sheets[0].RowCount = 0;
                            FpSpread2.Sheets[0].ColumnHeader.RowCount = 0;
                            FpSpread2.Sheets[0].RowCount = 1;
                            FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
                            FpSpread2.Sheets[0].ColumnCount = 2;
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
                            if (ItemList.Count > 0)
                            {
                                for (int i = 1; i <= ItemList.Count; i++)
                                {
                                    FpSpread2.Sheets[0].ColumnHeader.Columns.Count++;
                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, i + 1, 2, 1);
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Text = ItemList[i - 1].ToString();
                                    FpSpread2.Sheets[0].Cells[0, i + 1].CellType = tb;
                                    FpSpread2.Sheets[0].Cells[0, i + 1].Text = " ";
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Locked = true;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, i + 1].Font.Bold = true;
                                }
                            }
                            FpSpread2.Sheets[0].ColumnHeader.Columns.Count++;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Session";
                            FpSpread2.Sheets[0].Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].CellType = tb;
                            FpSpread2.Sheets[0].Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = " ";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Locked = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                            FpSpread2.Sheets[0].SpanModel.Add(0, 1, 1, FpSpread2.Sheets[0].ColumnCount - 1);
                            spancolumn = FpSpread2.Sheets[0].ColumnCount;
                            if (ddlleavereason.Items.Count > 0)
                            {
                                int o = 0;
                                sarray5 = new string[ddlleavereason.Items.Count + 1];
                                sarray2 = new string[ddlleavereason.Items.Count + 1];
                                for (int k = 0; k < ddlleavereason.Items.Count; k++)
                                {
                                    o++;
                                    if (ddlleavereason.Items[k].Text != "---Select---")
                                    {
                                        sarray5[o] = ddlleavereason.Items[k].Text;
                                        sarray2[o] = ddlleavereason.Items[k].Text;
                                    }
                                }
                                //   sarray2[0] = "Select For All";
                                sarray2[1] = " ";
                                sarray5[0] = " ";
                            }
                            FpSpread2.SaveChanges();
                            ArrayList arrayceck = new ArrayList();
                            string[] dtfrom;
                            string[] dttodate;
                            dtfrom = txtfromdate.Text.Split('/');
                            dttodate = txttodate.Text.Split('/');
                            DateTime strstartdate = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]).Date;
                            DateTime strenddate = Convert.ToDateTime(dttodate[1] + '/' + dttodate[0] + '/' + dttodate[2]).Date;
                            TimeSpan t = strenddate.Subtract(strstartdate);
                            long days = t.Days;
                            Boolean hrflag = false;
                            string hrdayquery = "select * from HrPayMonths where College_Code='" + ddlcollege.SelectedItem.Value + "' order by PayYear";
                            DataSet dshrpayquery = d2.select_method_wo_parameter(hrdayquery, "text");
                            if (dshrpayquery.Tables[0].Rows.Count > 0)
                            {
                                string hrfdate = dshrpayquery.Tables[0].Rows[0]["From_Date"].ToString();
                                string tdate = dshrpayquery.Tables[0].Rows[dshrpayquery.Tables[0].Rows.Count - 1]["To_Date"].ToString();
                                DateTime dts = Convert.ToDateTime(hrfdate);
                                DateTime dte = Convert.ToDateTime(tdate);
                                if (dts <= strstartdate && dte >= strenddate)
                                {
                                    hrflag = true;
                                }
                            }
                            // poomalar 29.11.17
                            int Settings = 0;
                            string usercode = Session["usercode"].ToString();
                            int.TryParse(Convert.ToString(d2.GetFunction(" select LinkValue from New_InsSettings where LinkName='Allow Future Attendance' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'")), out Settings);
                            if (Settings != 1) // if added by poomalar 29.11.17
                            {
                                if (strstartdate > DateTime.Today)
                                {
                                    //lblError.Text = "You can not mark attendance for the date greater than today";
                                    //lblError.Visible = true;
                                    FpSpread2.Visible = false;
                                    FpSpread2.Sheets[0].Visible = false;
                                    panelsecond.Visible = false;
                                    lblreasonleave.Visible = false;
                                    ddlleavereason.Visible = false;
                                    btnselectall.Visible = false;
                                    btndeselectall.Visible = false;
                                    btnsave.Visible = false;
                                    btnupdate.Visible = false;
                                }
                                else if (strenddate > DateTime.Today)
                                {
                                    //lblError.Text = "You can not mark attendance for the date greater than today";
                                    //lblError.Visible = true;
                                    FpSpread2.Visible = false;
                                    FpSpread2.Sheets[0].Visible = false;
                                    panelsecond.Visible = false;
                                    lblreasonleave.Visible = false;
                                    ddlleavereason.Visible = false;
                                    btnselectall.Visible = false;
                                    btndeselectall.Visible = false;
                                    btnsave.Visible = false;
                                    btnupdate.Visible = false;
                                }
                            }
                            if (strstartdate <= strenddate)
                            {
                                //if (days == 0 && strstartdate.ToString("dddd") == "Sunday")
                                //{
                                //    lblError.Text = "Selected Day is Sunday";
                                //    lblError.Visible = true;
                                //    FpSpread2.Visible = false;
                                //    FpSpread2.Sheets[0].Visible = false;
                                //    panelsecond.Visible = false;
                                //    lblreasonleave.Visible = false;
                                //    ddlleavereason.Visible = false;
                                //    btnselectall.Visible = false;
                                //    btndeselectall.Visible = false;
                                //    btnsave.Visible = false;
                                //    btnupdate.Visible = false;
                                //}
                                //else
                                if (hrflag == false)
                                {
                                    lblerror1.Visible = false;
                                    lblError.Text = "Please Update Hr Year";
                                    lblError.Visible = true;
                                    FpSpread2.Visible = false;
                                    FpSpread2.Sheets[0].Visible = false;
                                    panelsecond.Visible = false;
                                    lblreasonleave.Visible = false;
                                    ddlleavereason.Visible = false;
                                    btnselectall.Visible = false;
                                    btndeselectall.Visible = false;
                                    btnsave.Visible = false;
                                    btnupdate.Visible = false;
                                }
                                else
                                {
                                    int checkcount = 0;
                                    FarPoint.Web.Spread.ComboBoxCellType cb2 = new FarPoint.Web.Spread.ComboBoxCellType();
                                    cb2 = new FarPoint.Web.Spread.ComboBoxCellType(sarray5);
                                    cb2.AutoPostBack = true;
                                    cb2.UseValue = true;
                                    // cb2.BackColor = Color.MistyRose;
                                    cb2.ShowButton = false;
                                    string errordate = "";
                                    while (strstartdate <= strenddate)
                                    {
                                        //if (days >= 0 && strstartdate.ToString("dddd") != "Sunday")
                                        //{
                                        string date = strstartdate.ToString("dd/MM/yyyy");
                                        arrayceck.Add(strstartdate.ToString("d/MM/yyyy"));
                                        FpSpread2.Sheets[0].ColumnHeader.Columns.Count = FpSpread2.Sheets[0].ColumnHeader.Columns.Count + 2;
                                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2, 1, 2);
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = date.ToString();
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Tag = date.ToString();
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Attendance Entry";
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                                        if (checkreason == true)//delsi 1407
                                        {
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Reason Entry";
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;
                                        }
                                        else if (checkreason == false)
                                        {
                                            FpSpread2.Sheets[0].ColumnHeader.Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Visible = false;
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Reason Entry";
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].Font.Bold = true;

                                        }
                                        //  cb1.BackColor = Color.MistyRose;
                                        FpSpread2.SaveChanges();
                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].CellType = cb1;
                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].CellType = cb2;
                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnHeader.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                        //}
                                        //else
                                        //{
                                        //    if (errordate == "")
                                        //    {
                                        //        errordate = "" + strstartdate.ToString("dd-MM-yyyy");
                                        //        arrHolDays.Add(strstartdate.ToString("MM/dd/yyyy"));
                                        //    }
                                        //    else
                                        //    {
                                        //        errordate = errordate + "," + strstartdate.ToString("dd-MM-yyyy");
                                        //        arrHolDays.Add(strstartdate.ToString("MM/dd/yyyy"));
                                        //    }
                                        //}
                                        strstartdate = strstartdate.AddDays(1);
                                        FpSpread2.SaveChanges();
                                        checkcount++;
                                    }
                                    if (errordate != "")
                                    {
                                        dateflag = true;
                                        lblerror1.Text = errordate.ToString() + "  Day is Sunday";
                                        lblerror1.Visible = true;
                                    }
                                    int row1 = 1;
                                    for (int lock1 = 0; lock1 < ItemList.Count + 1; lock1++)
                                    {
                                        row1++;
                                        FpSpread2.Sheets[0].Cells[0, row1].Locked = true;
                                    }
                                    FpSpread2.Sheets[0].Cells[0, 0].Locked = true;
                                    FpSpread2.Sheets[0].Cells[0, 1].Locked = true;
                                    int sno = 0;
                                    cb3 = new FarPoint.Web.Spread.ComboBoxCellType(sarray3);
                                    cb3.AutoPostBack = true;
                                    cb3.UseValue = true;
                                    //cb3.BackColor = Color.MistyRose;
                                    cb3.ShowButton = true;
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        sno++;
                                        FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 2;
                                        if (attflag == true)
                                        {
                                            FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Visible = false;
                                            FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Visible = false;
                                        }
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["Staff_code"].ToString() + " - E";
                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, 0, 2, 1);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = sno.ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Tag = ds.Tables[0].Rows[i]["Staff_code"].ToString() + " - M";
                                        FpSpread2.Sheets[0].Columns[0].Width = 50;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].BackColor = Color.LightYellow;
                                        int var = 2;
                                        int col = 0;
                                        for (int k = 0; k < ds.Tables[0].Columns.Count - 1; k++)
                                        {
                                            var++;
                                            col++;
                                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, 1, 2, 1);
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].CellType = cb3;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].VerticalAlign = VerticalAlign.Middle;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].Font.Size = FontUnit.Medium;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].Font.Name = "Book Antiqua";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].BackColor = Color.MistyRose;
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
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                //FpSpread2.Sheets[0].Columns[k + 2].CellType = tb;
                                            }
                                            else if (value == "Staff Name")
                                            {
                                                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                FpSpread2.Sheets[0].Columns[k + 2].Width = 200;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                            }
                                            else if (value == "Department")
                                            {
                                                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, k + 2].Text = "";
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                FpSpread2.Sheets[0].Columns[k + 2].Width = 150; //chnged width before 200
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                            }
                                            else if (value == "Designation")
                                            {
                                                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, k + 2].Text = "";
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                FpSpread2.Sheets[0].Columns[k + 2].Width = 200;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                            }
                                            else if (value == "Staff Category")
                                            {
                                                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, k + 2].Text = "";
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                                FpSpread2.Sheets[0].Columns[k + 2].Width = 200;
                                            }
                                            else if (value == "Staff Type")
                                            {
                                                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, k + 2, 2, 1);
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, k + 2].Text = "";
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Text = ds.Tables[0].Rows[i][col].ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Locked = true;
                                                FpSpread2.Sheets[0].Columns[k + 2].Width = 200;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Size = FontUnit.Medium;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].Font.Name = "Book Antiqua";
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, k + 2].BackColor = Color.LightYellow;
                                            }
                                        }
                                    }
                                    Session["item_Value"] = Convert.ToInt32(ItemList.Count);
                                    int daytime = Convert.ToInt32(Session["item_Value"]);
                                    daytime = daytime + 2;
                                    for (int i = 1; i < FpSpread2.Sheets[0].RowCount; i++)
                                    {
                                        FpSpread2.Sheets[0].Cells[i, daytime].Text = "M";
                                        FpSpread2.Sheets[0].Cells[i, daytime].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[i, daytime].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[i, daytime].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[i, daytime].BackColor = Color.MistyRose;
                                        FpSpread2.Sheets[0].Cells[i, daytime].Locked = true;
                                        i++;
                                        FpSpread2.Sheets[0].Cells[i, daytime].Text = "E";
                                        FpSpread2.Sheets[0].Cells[i, daytime].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[i, daytime].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[i, daytime].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Cells[i, daytime].BackColor = Color.MistyRose;
                                        FpSpread2.Sheets[0].Cells[i, daytime].Locked = true;
                                    }
                                    FarPoint.Web.Spread.ComboBoxCellType cb4 = new FarPoint.Web.Spread.ComboBoxCellType();
                                    cb4 = new FarPoint.Web.Spread.ComboBoxCellType(sarray2);
                                    cb4.AutoPostBack = true;
                                    cb4.UseValue = true;
                                    cb4.BackColor = Color.MistyRose;
                                    cb4.ShowButton = true;
                                    cb1 = new FarPoint.Web.Spread.ComboBoxCellType(sarray1);
                                    cb1.AutoPostBack = true;
                                    cb1.UseValue = true;
                                    cb1.BackColor = Color.MistyRose;
                                    cb1.ShowButton = true;
                                    int countcolvalue = Convert.ToInt32(Session["item_Value"]);
                                    countcolvalue = countcolvalue + 3;
                                    //for (int j = 0; j < countcolvalue; j++)
                                    //{
                                    //    FpSpread2.Sheets[0].Cells[0, j].Text = "";
                                    //    j++;
                                    //    FpSpread2.Sheets[0].Cells[0, j].Text = "";
                                    //}
                                    for (int j = countcolvalue; j < FpSpread2.Sheets[0].ColumnCount; j++)
                                    {
                                        FpSpread2.Sheets[0].Cells[0, j].CellType = cb1;
                                        j++;
                                        FpSpread2.Sheets[0].Cells[0, j].CellType = cb4;
                                    }
                                    calcutale1();
                                    FpSpread2.SaveChanges();
                                    int countvalue = Convert.ToInt32(Session["item_Value"]);
                                    countvalue = countvalue + 3;
                                    string monyear = "";
                                    string resondate = "";
                                    string app_id = "";
                                    string staff_code = "";
                                    ds.Clear();
                                    string linkvaluequery = "";
                                    string linkvalue = "";
                                    //linkvaluequery = "select * from InsSettings where LinkName like 'Staff Holiday By Staff Type' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                                    linkvaluequery = d2.GetFunction("select value from Master_Settings where settings='HR_PanelSettings' and usercode='" + Convert.ToString(Session["usercode"]) + "'");
                                    //ds = d2.select_method_wo_parameter(linkvaluequery, "Text");
                                    //if (ds.Tables[0].Rows.Count > 0)
                                    //{
                                    //linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["Linkvalue"]);
                                    //}
                                    if (linkvaluequery.Contains('3'))
                                        linkvalue = "1";
                                    else
                                        linkvalue = "0";
                                    DataSet ds2 = new DataSet();
                                    DataView dv1 = new DataView();
                                    DataView dv2 = new DataView();
                                    string fistpreviousquery = "";
                                    string query = "";
                                    //*************************Modified By Srinath 22/jan/2014**********************************
                                    //fistpreviousquery = "select lt_taken,remarks from staff_leave_details where college_code ='" + ddlcollege.SelectedItem.Value + "'";
                                    // zzz 2/5/16
                                    fistpreviousquery = "select LeaveMasterFK from RQ_Requisition where RequestType =5 and ReqAppStatus=1 ";
                                    fistpreviousquery = fistpreviousquery + " select * from leave_category where college_code ='" + ddlcollege.SelectedItem.Value + "'";
                                    ds2.Clear();
                                    ds2 = d2.select_method_wo_parameter(fistpreviousquery, "Text");
                                    if (attflag == true)
                                    {
                                        nofostaff++;
                                    }
                                    for (int i = 1; i < FpSpread2.Sheets[0].RowCount; i += 2)
                                    {
                                        int s = 0;
                                        string[] split_staff_code = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(i), 0].Tag).Split('-');
                                        if (split_staff_code.Length > 0)
                                        {
                                            staff_code = Convert.ToString(split_staff_code[0]);
                                            app_id = d2.GetFunction("select sm.appl_id from staff_appl_master sm, staffmaster s where sm.appl_no=s.appl_no and s.staff_code='" + staff_code + "'");
                                        }
                                        //  staff_code = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 0].Tag);
                                        int row = i;
                                        if (attflag == false)
                                        {
                                            FpSpread2.Sheets[0].Rows[row].Visible = true;
                                            FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                            //if (i > 1)
                                            //{
                                            nofostaff++;
                                            //}
                                            FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                        }
                                        for (int k = countvalue; k < FpSpread2.Sheets[0].ColumnCount - 1; k += 2)
                                        {
                                            string date = arrayceck[s].ToString();
                                            string[] split_d = date.Split(new Char[] { '/' });
                                            string strdate = split_d[0].ToString();
                                            string Atmonth = split_d[1].ToString();
                                            string Atyear = split_d[2].ToString();
                                            string atmonth1 = (Atmonth.TrimStart('0'));
                                            string strdate1 = (strdate.TrimStart('0'));
                                            monyear = atmonth1 + "/" + Atyear;
                                            resondate = atmonth1 + "/" + strdate1 + "/" + Atyear;
                                            ds.Clear();
                                            string holidayquery = "";
                                            if (linkvalue == "0")
                                            {
                                                //holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + catevalue + "') and t.staff_code ='" + staff_code + "'";
                                                holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + catevalue + "') and t.staff_code ='" + staff_code + "' and h.dept_code in('" + deptvalue + "') and t.dept_code=h.dept_code and latestrec='1'";
                                            }
                                            if (linkvalue == "1")
                                            {
                                                //holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stypevalue + "') ";

                                                holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stypevalue + "') and dept_code in('" + deptvalue + "')";
                                            }
                                            if (holidayquery != "")
                                            {
                                                ds = d2.select_method_wo_parameter(holidayquery, "Text");
                                            }
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                string holday_desc = Convert.ToString(ds.Tables[0].Rows[0]["holiday_desc"]);
                                                string halforfull = Convert.ToString(ds.Tables[0].Rows[0]["halforfull"]);
                                                if (halforfull == "False")
                                                {
                                                    countadd++;
                                                    string countquery = "";
                                                    countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                    ds.Clear();
                                                    ds = d2.select_method_wo_parameter(countquery, "Text");
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        string attndsplit = ds.Tables[0].Rows[0][0].ToString();
                                                        string reasonsplit = ds.Tables[0].Rows[0][1].ToString();
                                                        if (attndsplit != "-" && attndsplit != null && attndsplit != "")
                                                        {
                                                            query = "select LeaveMasterFK,(Select MasterValue FROM CO_MasterValues T WHERE GateReqReason = t.MasterCode) GateReqReason from RQ_Requisition where ReqAppNo ='" + app_id + "' and '" + resondate + "' between LeaveFrom and LeaveTo and RequestType =5 and ReqAppStatus='1'";
                                                            ds.Clear();
                                                            ds = d2.select_method_wo_parameter(query, "Text");
                                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                            {
                                                                string[] splitarray = attndsplit.Split('-');
                                                                string[] reason_split = reasonsplit.Split('-');
                                                                string leavetaken = ds.Tables[0].Rows[0]["LeaveMasterFK"].ToString();
                                                                string remaks = Convert.ToString(ds.Tables[0].Rows[0]["GateReqReason"]);
                                                                ds2.Tables[1].DefaultView.RowFilter = "LeaveMasterPK='" + leavetaken + "'";
                                                                dv2 = ds2.Tables[1].DefaultView;
                                                                if (dv2.Count > 0)
                                                                {
                                                                    string leave_short = dv2[0]["shortname"].ToString();
                                                                    if (splitarray.GetUpperBound(0) >= 1)
                                                                    {
                                                                        if (leave_short.ToString() == splitarray[0].ToString())
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[row, k].Text = splitarray[0].ToString();
                                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                            if (splitarray[0].ToString() == "H")
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row, k].Locked = false;
                                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Locked = false;
                                                                            }
                                                                            FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(remaks);
                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[row, k].Text = "H";
                                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                        }
                                                                        if (leave_short.ToString() == splitarray[1].ToString())
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Text = splitarray[1].ToString();
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                            if (splitarray[1].ToString() == "H")
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k].Locked = false;
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = false;
                                                                            }
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(remaks);
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Text = "H";
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                        }
                                                                        if (attflag == true)
                                                                        {
                                                                            if (dicattval.ContainsKey(splitarray[0].ToString().ToLower()) || dicattval.ContainsKey(splitarray[1].ToString().ToLower()))
                                                                            {
                                                                                FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                                FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                                FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                    if (reason_split.GetUpperBound(0) >= 1)
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(reason_split[0].ToString());
                                                                        FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reason_split[1].ToString());
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    }
                                                                    else
                                                                    {
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = "H";
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
                                                                    if (attflag == true)
                                                                    {
                                                                        if (dicattval.ContainsKey("h") || dicattval.ContainsKey("h"))
                                                                        {
                                                                            FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                            FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                            FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!linkvaluequery.Contains('2'))//delsi 1707//linkvaluequery != "2"
                                                                {

                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = "H";
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
                                                                else
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = "H";
                                                                    FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    // FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    //  FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Text = "H";
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    // FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    //FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;

                                                                }
                                                                if (attflag == true)
                                                                {
                                                                    if (dicattval.ContainsKey("h") || dicattval.ContainsKey("h"))
                                                                    {
                                                                        FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                        FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                        FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            FpSpread2.Sheets[0].Cells[row, k].Text = "H";
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
                                                            if (attflag == true)
                                                            {
                                                                if (dicattval.ContainsKey("h") || dicattval.ContainsKey("h"))
                                                                {
                                                                    FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                    FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (linkvaluequery.Contains('2'))//delsi 1707
                                                        {
                                                            FpSpread2.Sheets[0].Cells[row, k].Text = "H";
                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                            // FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                            FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                            //   FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Text = "H";
                                                            FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                            //  FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                            //  FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                        }
                                                        else
                                                        {

                                                            FpSpread2.Sheets[0].Cells[row, k].Text = "H";
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
                                                        if (attflag == true)
                                                        {
                                                            if (dicattval.ContainsKey("h") || dicattval.ContainsKey("h"))
                                                            {
                                                                FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (halforfull == "True")
                                                    {
                                                        countadd++;
                                                        string morning = Convert.ToString(ds.Tables[0].Rows[0]["morning"]);
                                                        string evening = Convert.ToString(ds.Tables[0].Rows[0]["evening"]);
                                                        if (morning == "False")
                                                        {
                                                            string countquery = "";
                                                            countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                            ds.Clear();
                                                            ds = d2.select_method_wo_parameter(countquery, "Text");
                                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                            {
                                                                string attndsplit = ds.Tables[0].Rows[0][0].ToString();
                                                                string reasonsplit = ds.Tables[0].Rows[0][1].ToString();
                                                                if (attndsplit != "-" && attndsplit != null && attndsplit != "")
                                                                {
                                                                    string[] splitarray = attndsplit.Split('-');
                                                                    if (splitarray.GetUpperBound(0) >= 1)
                                                                    {
                                                                        if (leave.Contains(splitarray[0].ToString()) == false)
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                            if (attflag == true)
                                                                            {
                                                                                if (dicattval.ContainsKey(splitarray[0].ToString().ToLower()))
                                                                                {
                                                                                    FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                                    FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                                }
                                                                            }
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
                                                                        else
                                                                        {
                                                                            //*************************Modified By Srinath 22/jan/2014**********************************
                                                                            //  query = "select remarks  from staff_leave_details where staff_code='" + staff_code + "' and adate='" + resondate + "' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                                                                            //query = "select lt_taken,remarks from staff_leave_details where college_code ='" + ddlcollege.SelectedItem.Value + "' and '" + resondate + "' between fdate and tdate and staff_code ='" + staff_code + "' and apply_approve = 1 ";
                                                                            query = "select LeaveMasterFK,(Select MasterValue FROM CO_MasterValues T WHERE GateReqReason = t.MasterCode) GateReqReason  from RQ_Requisition where ReqAppNo ='" + app_id + "' and '" + resondate + "' between LeaveFrom and LeaveTo and RequestType =5 and ReqAppStatus='1'";
                                                                            ds.Clear();
                                                                            ds = d2.select_method_wo_parameter(query, "Text");
                                                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                string leavetaken = ds.Tables[0].Rows[0]["LeaveMasterFK"].ToString();
                                                                                ds2.Tables[1].DefaultView.RowFilter = "LeaveMasterPK='" + leavetaken + "'";
                                                                                dv2 = ds2.Tables[1].DefaultView;
                                                                                if (dv2.Count > 0)
                                                                                {
                                                                                    string leave_short = dv2[0]["shortname"].ToString();
                                                                                    if (leave_short.ToString() == splitarray[0].ToString())
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                                                        FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                                        FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                                        string reasonapply = Convert.ToString(ds.Tables[0].Rows[0]["GateReqReason"]);
                                                                                        if (reasonapply.Trim() != "" && reasonapply != null)
                                                                                        {
                                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(reasonapply.ToString());
                                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Text = "";
                                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                                                        FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                                        if (reasonsplit != "" && reasonsplit != null && reasonsplit != "-")
                                                                                        {
                                                                                            string[] reasonarray = reasonsplit.Split('-');
                                                                                            if (reasonsplit[0].ToString() != "")
                                                                                            {
                                                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(reasonarray[0].ToString());
                                                                                                FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                FpSpread2.Sheets[0].Cells[row, k + 1].Text = "";
                                                                                                FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
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
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!linkvaluequery.Contains('2'))//delsi 1707
                                                            {
                                                                if (morning == "True")
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = "H";
                                                                    FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                    if (attflag == true)
                                                                    {
                                                                        if (dicattval.ContainsKey("h"))
                                                                        {
                                                                            FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                            FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                            FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (morning == "True")
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = "H";
                                                                    FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    //  FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                    FpSpread2.Sheets[0].Cells[row, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    //FpSpread2.Sheets[0].Cells[row, k + 1].Locked = true;
                                                                    if (attflag == true)
                                                                    {
                                                                        if (dicattval.ContainsKey("h"))
                                                                        {
                                                                            FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                            FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                            FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                        }
                                                        if (evening == "False")
                                                        {
                                                            string countquery = "";
                                                            countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                            ds.Clear();
                                                            ds = d2.select_method_wo_parameter(countquery, "Text");
                                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                            {
                                                                string attndsplit = ds.Tables[0].Rows[0][0].ToString();
                                                                string reasonsplit = ds.Tables[0].Rows[0][1].ToString();
                                                                if (attndsplit != "-" && attndsplit != null && attndsplit != "")
                                                                {
                                                                    string[] splitarray = attndsplit.Split('-');
                                                                    if (leave.Contains(splitarray[1].ToString()) == false)
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                        if (attflag == true)
                                                                        {
                                                                            if (dicattval.ContainsKey(splitarray[1].ToString().ToLower()))
                                                                            {
                                                                                FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                                FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                                FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                            }
                                                                        }
                                                                        if (reasonsplit != "" && reasonsplit != null)
                                                                        {
                                                                            string[] reasonarray = reasonsplit.Split('-');
                                                                            if (reasonarray.GetUpperBound(0) >= 1)
                                                                            {
                                                                                if (reasonarray[1].ToString() != "")
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
                                                                    else
                                                                    {
                                                                        //*************************Modified By Srinath 22/jan/2014**********************************
                                                                        //query = "select lt_taken,remarks from staff_leave_details where college_code ='" + ddlcollege.SelectedItem.Value + "' and '" + resondate + "' between fdate and tdate and staff_code ='" + staff_code + "' and apply_approve = 1 ";
                                                                        query = "select LeaveMasterFK,(Select MasterValue FROM CO_MasterValues T WHERE GateReqReason = t.MasterCode) GateReqReason from RQ_Requisition where ReqAppNo ='" + app_id + "' and '" + resondate + "' between LeaveFrom and LeaveTo and RequestType =5  and ReqAppStatus='1'";
                                                                        ds.Clear();
                                                                        ds = d2.select_method_wo_parameter(query, "Text");
                                                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            string leavetaken = ds.Tables[0].Rows[0]["LeaveMasterFK"].ToString();
                                                                            string[] split_half_value = leavetaken.Split('@');
                                                                            string getleavm = split_half_value[0].ToString();
                                                                            string getleava = split_half_value[0].ToString();
                                                                            if (leavetaken.Trim() != "" && leavetaken != null)
                                                                            {
                                                                                ds2.Tables[1].DefaultView.RowFilter = "LeaveMasterPK='" + getleavm + "'";
                                                                                dv2 = ds2.Tables[1].DefaultView;
                                                                                if (dv2.Count > 0)
                                                                                {
                                                                                    string leave_short = dv2[0]["shortname"].ToString();
                                                                                    if (leave_short.ToString() == splitarray[1].ToString())
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                                        if (attflag == true)
                                                                                        {
                                                                                            if (dicattval.ContainsKey(splitarray[1].ToString().ToLower()))
                                                                                            {
                                                                                                FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                                                FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                                                FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                                            }
                                                                                        }
                                                                                        string reasonapply = Convert.ToString(ds.Tables[0].Rows[0]["GateReqReason"]);
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
                                                                                    else
                                                                                    {
                                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                                        //FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                                        if (reasonsplit != "" && reasonsplit != null)
                                                                                        {
                                                                                            string[] reasonarray = reasonsplit.Split('-');
                                                                                            if (reasonsplit[1].ToString() != "")
                                                                                            {
                                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                                                //  FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                                                                // FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                            if (attflag == true)
                                                                            {
                                                                                if (dicattval.ContainsKey(splitarray[1].ToString().ToLower()))
                                                                                {
                                                                                    FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                                    FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                                }
                                                                            }
                                                                            if (reasonsplit != "" && reasonsplit != null)
                                                                            {
                                                                                string[] reasonarray = reasonsplit.Split('-');
                                                                                if (reasonarray.GetUpperBound(0) >= 1)
                                                                                {
                                                                                    if (reasonarray[1].ToString() != "")
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
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!linkvaluequery.Contains('2'))//delsi 1707
                                                            {
                                                                if (evening == "True")
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Text = "H";
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                    if (attflag == true)
                                                                    {
                                                                        if (dicattval.ContainsKey("h"))
                                                                        {
                                                                            FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                            FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                            FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (evening == "True")
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Text = "H";
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                    // FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(holday_desc.ToString());
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                    //   FpSpread2.Sheets[0].Cells[row + 1, k + 1].Locked = true;
                                                                    if (attflag == true)
                                                                    {
                                                                        if (dicattval.ContainsKey("h"))
                                                                        {
                                                                            FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                            FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                            FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
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
                                                #region Marked Attendance show added Barath 29.12.17
                                                string countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(countquery, "Text");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    string attndsplit = ds.Tables[0].Rows[0][0].ToString();
                                                    string reasonsplit = ds.Tables[0].Rows[0][1].ToString();
                                                    if (attndsplit != "-" && attndsplit != null && attndsplit != "")
                                                    {
                                                        countadd++;
                                                        string[] splitarray = attndsplit.Split('-');
                                                        if (attflag == true)
                                                        {
                                                            if (splitarray.GetUpperBound(0) == 1)
                                                            {
                                                                string setval = splitarray[0].ToString().Trim().ToLower();
                                                                string setval1 = splitarray[1].ToString().Trim().ToLower();
                                                                if (dicattval.ContainsKey(setval) || dicattval.ContainsKey(setval1))
                                                                {
                                                                    FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                    FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                }
                                                            }
                                                        }
                                                        Boolean leaveflag = false;
                                                        query = "select LeaveMasterFK,(Select MasterValue FROM CO_MasterValues T WHERE GateReqReason = t.MasterCode) GateReqReason ,ishalfday,leaveSession from RQ_Requisition where ReqAppNo ='" + app_id + "' and '" + resondate + "' between LeaveFrom and LeaveTo and RequestType =5 and ReqAppStatus='1' ";
                                                        ds.Clear();
                                                        ds = d2.select_method_wo_parameter(query, "Text");
                                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                            leaveflag = true;
                                                        if (leave.Contains(splitarray[0].ToString()) == false && leaveflag == false)
                                                        {
                                                            FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                            if (attflag == true)
                                                            {
                                                                if (dicattval.ContainsKey(splitarray[0].ToString().ToLower()))
                                                                {
                                                                    FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                    FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                }
                                                            }
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
                                                        else
                                                        {
                                                            FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
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
                                                        if (leave.Contains(Convert.ToString(splitarray[1])) == false && leaveflag == false)
                                                        {
                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                            FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                            if (attflag == true)
                                                            {
                                                                if (dicattval.ContainsKey(splitarray[1].ToString().ToLower()))
                                                                {
                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                    FpSpread2.Sheets[0].Cells[row + 1, 0].Text = nofostaff.ToString();
                                                                }
                                                            }
                                                            if (reasonsplit != "" && reasonsplit != null && reasonsplit != "-")
                                                            {
                                                                string[] reasonarray = reasonsplit.Split('-');
                                                                if (reasonsplit[1].ToString() != "")
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                else
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                            FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                            if (reasonsplit != "" && reasonsplit != null && reasonsplit != "-")
                                                            {
                                                                string[] reasonarray = reasonsplit.Split('-');
                                                                if (reasonsplit[1].ToString() != "")
                                                                {
                                                                    if (reasonarray.Length>1)
                                                                  
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                }
                                                                else
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                                }
                                                            }
                                                        }
                                                        if (leaveflag)
                                                        {   //barath 29.12.17
                                                            Boolean morningflag = false;
                                                            Boolean eveningflag = false;
                                                            foreach (DataRow dr in ds.Tables[0].Rows)
                                                            {
                                                                string LeaveSession = Convert.ToString(dr["leaveSession"]);//barath 29.12.17
                                                                string leavetaken = Convert.ToString(dr["LeaveMasterFK"]);
                                                                ds2.Tables[1].DefaultView.RowFilter = "LeaveMasterPK='" + leavetaken + "'";
                                                                dv2 = ds2.Tables[1].DefaultView;
                                                                if (Convert.ToString(dr["ishalfday"]).ToUpper() == "FALSE" || Convert.ToString(dr["ishalfday"]).ToUpper() == "0")
                                                                    LeaveSession = "1";
                                                                if (LeaveSession == "1")
                                                                {
                                                                    if (dv2.Count > 0)
                                                                    {
                                                                        string leave_short = dv2[0]["shortname"].ToString();
                                                                        //if (leave_short.ToString() == splitarray[1].ToString())
                                                                        //{
                                                                        FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(leave_short);//splitarray[0]);
                                                                        FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                        morningflag = true;
                                                                        string reasonapply = Convert.ToString(dr["GateReqReason"]);
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
                                                                if (Convert.ToString(dr["ishalfday"]).ToUpper() == "FALSE" || Convert.ToString(dr["ishalfday"]).ToUpper() == "0")
                                                                    LeaveSession = "2";
                                                                if (LeaveSession == "2")
                                                                {
                                                                    if (dv2.Count > 0)
                                                                    {
                                                                        string leave_short = dv2[0]["shortname"].ToString();
                                                                        //if (leave_short.ToString() == splitarray[1].ToString())
                                                                        //{
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(leave_short);//splitarray[0]);
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                        eveningflag = true;
                                                                        string reasonapply = Convert.ToString(dr["GateReqReason"]);
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

                                                                if (!morningflag)
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                                    if (reasonsplit != "" && reasonsplit != null && reasonsplit != "-")
                                                                    {
                                                                        string[] reasonarray = reasonsplit.Split('-');
                                                                        if (reasonsplit[0].ToString() != "")
                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Text = Convert.ToString(reasonarray[0].ToString());
                                                                        else
                                                                            FpSpread2.Sheets[0].Cells[row, k + 1].Text = "";
                                                                    }
                                                                }
                                                                if (!eveningflag)
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                    if (reasonsplit != "" && reasonsplit != null && reasonsplit != "-")
                                                                    {
                                                                        string[] reasonarray = reasonsplit.Split('-');

                                                                        if (reasonsplit[1].ToString() != "")
                                                                            if (reasonarray.Length>1)
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                        else
                                                                            FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                            if (attflag == true)
                                                            {
                                                                if (dicattval.ContainsKey(splitarray[0].ToString().ToLower()))
                                                                {
                                                                    FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                    FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                }
                                                            }
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



                                                #endregion


                                                /*
                                                string countquery = "";
                                                countquery = "select a.[" + strdate + "] ,l.[" + strdate + "] from staff_attnd a left outer join Staff_Leavereason l on (a.staff_code=l.staff_code and a.mon_year=l.monyear)  where a.staff_code ='" + staff_code + "' and mon_year = '" + monyear + "'";
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(countquery, "Text");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    string attndsplit = ds.Tables[0].Rows[0][0].ToString();
                                                    string reasonsplit = ds.Tables[0].Rows[0][1].ToString();
                                                    if (attndsplit != "-" && attndsplit != null && attndsplit != "")
                                                    {
                                                        countadd++;
                                                        string[] splitarray = attndsplit.Split('-');
                                                        if (attflag == true)
                                                        {
                                                            if (splitarray.GetUpperBound(0) == 1)
                                                            {
                                                                string setval = splitarray[0].ToString().Trim().ToLower();
                                                                string setval1 = splitarray[1].ToString().Trim().ToLower();
                                                                if (dicattval.ContainsKey(setval) || dicattval.ContainsKey(setval1))
                                                                {
                                                                    FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                    FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                }
                                                            }
                                                        }
                                                        Boolean leaveflag = false;
                                                        query = "select LeaveMasterFK,(Select MasterValue FROM CO_MasterValues T WHERE GateReqReason = t.MasterCode) GateReqReason from RQ_Requisition where ReqAppNo ='" + app_id + "' and '" + resondate + "' between LeaveFrom and LeaveTo and RequestType =5 and ReqAppStatus='1' ";
                                                        ds.Clear();
                                                        ds = d2.select_method_wo_parameter(query, "Text");
                                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                        {
                                                            leaveflag = true;
                                                        }
                                                        if (leave.Contains(splitarray[0].ToString()) == false && leaveflag == false)
                                                        {
                                                            FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                            FpSpread2.Sheets[0].Cells[row, k].HorizontalAlign = HorizontalAlign.Center;
                                                            if (attflag == true)
                                                            {
                                                                if (dicattval.ContainsKey(splitarray[0].ToString().ToLower()))
                                                                {
                                                                    FpSpread2.Sheets[0].Rows[row].Visible = true;
                                                                    FpSpread2.Sheets[0].Rows[row + 1].Visible = true;
                                                                    FpSpread2.Sheets[0].Cells[row, 0].Text = nofostaff.ToString();
                                                                }
                                                            }
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
                                                        else
                                                        {//*************************Modified By Srinath 22/jan/2014**********************************
                                                            //query = "select lt_taken,remarks from staff_leave_details where college_code ='" + ddlcollege.SelectedItem.Value + "' and '" + resondate + "' between fdate and tdate and staff_code ='" + staff_code + "' and apply_approve = 1 ";
                                                            //query = "select lt_taken,remarks from staff_leave_details where college_code ='" + ddlcollege.SelectedItem.Value + "' and '" + resondate + "' between fdate and tdate and staff_code ='" + staff_code + "' and apply_approve = 1 and isnull(directapply,0)<>1";
                                                            //ds.Clear();
                                                            //ds = d2.select_method_wo_parameter(query, "Text");
                                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                            {
                                                                string leavetaken = ds.Tables[0].Rows[0]["LeaveMasterFK"].ToString();
                                                                ds2.Tables[1].DefaultView.RowFilter = "LeaveMasterPK='" + leavetaken + "'";
                                                                dv2 = ds2.Tables[1].DefaultView;
                                                                if (dv2.Count > 0)
                                                                {
                                                                    string leave_short = dv2[0]["shortname"].ToString();
                                                                    if (leave_short.ToString() == splitarray[1].ToString())
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                                        FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                        string reasonapply = Convert.ToString(ds.Tables[0].Rows[0]["GateReqReason"]);
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
                                                                    else
                                                                    {
                                                                        //*********************Modified By Srinath 21/1/2014*********************
                                                                        Boolean setflag = false;
                                                                        leave_short = dv2[0]["shortname"].ToString();
                                                                        if (leave_short.ToString() == splitarray[0].ToString())
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
                                                                            FpSpread2.Sheets[0].Cells[row, k].Locked = true;
                                                                            setflag = true;
                                                                            string reasonapply = Convert.ToString(ds.Tables[0].Rows[0]["GateReqReason"]);
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
                                                                        if (setflag == false)
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
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
                                                                        //*********************End*******************************************
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                FpSpread2.Sheets[0].Cells[row, k].Text = Convert.ToString(splitarray[0]);
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
                                                        if (leave.Contains(splitarray[1].ToString()) == false)
                                                        {
                                                            FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                            FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                            if (reasonsplit != "" && reasonsplit != null && reasonsplit != "-")
                                                            {
                                                                string[] reasonarray = reasonsplit.Split('-');
                                                                if (reasonsplit[1].ToString() != "")
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                }
                                                                else
                                                                {
                                                                    FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {//*************************Modified By Srinath 22/jan/2014**********************************
                                                            //query = "select lt_taken,remarks from staff_leave_details where college_code ='" + ddlcollege.SelectedItem.Value + "' and '" + resondate + "' between fdate and tdate and staff_code ='" + staff_code + "' and apply_approve = 1 ";
                                                            query = "select LeaveMasterFK,(Select MasterValue FROM CO_MasterValues T WHERE GateReqReason = t.MasterCode) GateReqReason  from RQ_Requisition where ReqAppNo ='" + app_id + "' and '" + resondate + "' between LeaveFrom and LeaveTo and RequestType =5  and ReqAppStatus='1'";
                                                            ds.Clear();
                                                            ds = d2.select_method_wo_parameter(query, "Text");
                                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                            {
                                                                string leavetaken = ds.Tables[0].Rows[0]["LeaveMasterFK"].ToString();
                                                                ds2.Tables[1].DefaultView.RowFilter = "LeaveMasterPK='" + leavetaken + "'";
                                                                dv2 = ds2.Tables[1].DefaultView;
                                                                if (dv2.Count > 0)
                                                                {
                                                                    string leave_short = dv2[0]["shortname"].ToString();
                                                                    if (leave_short.ToString() == splitarray[1].ToString())
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Locked = true;
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                        string reasonapply = Convert.ToString(ds.Tables[0].Rows[0]["GateReqReason"]);
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
                                                                    else
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                        if (reasonsplit != "" && reasonsplit != null && reasonsplit != "-")
                                                                        {
                                                                            string[] reasonarray = reasonsplit.Split('-');
                                                                            if (reasonsplit[1].ToString() != "")
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                            }
                                                                            else
                                                                            {
                                                                                FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                FpSpread2.Sheets[0].Cells[row + 1, k].Text = Convert.ToString(splitarray[1]);
                                                                FpSpread2.Sheets[0].Cells[row + 1, k].HorizontalAlign = HorizontalAlign.Center;
                                                                if (reasonsplit != "" && reasonsplit != null && reasonsplit != "-")
                                                                {
                                                                    string[] reasonarray = reasonsplit.Split('-');
                                                                    if (reasonsplit[1].ToString() != "")
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = Convert.ToString(reasonarray[1].ToString());
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread2.Sheets[0].Cells[row + 1, k + 1].Text = "";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }*/
                                            }
                                            s++;
                                        }
                                        if (attflag == true)
                                        {
                                            if (FpSpread2.Sheets[0].Rows[row].Visible == true)
                                            {
                                                noofstaffflag = true;
                                                nofostaff++;
                                            }
                                        }
                                    }
                                    if (attflag == false)
                                    {
                                        noofstaffflag = true;
                                    }
                                    int daytime1 = Convert.ToInt32(Session["item_Value"]);
                                    daytime1 = daytime1 + 1;
                                    for (int a = 0; a < 2; a++)
                                    {
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 1, 1, daytime1 + 1);
                                    }
                                    //Modified by srinath 18/2/2015 1 to 2
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = "No Of Staff(s) Morning Present:";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Locked = true;
                                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, 0, 1, 4);
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "No Of Staff(s) Morning Absent:";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 4);
                                    FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 2;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = "No Of Staff(s) Evening Present:";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Locked = true;
                                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 2, 0, 1, 4);
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "No Of Staff(s) Evening Absent:";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 4);
                                    FarPoint.Web.Spread.TextCellType tb1 = new FarPoint.Web.Spread.TextCellType();
                                    int daytime2 = Convert.ToInt32(Session["item_Value"]);
                                    daytime2 = daytime2 + 3;
                                    for (; daytime2 < FpSpread2.Sheets[0].ColumnCount; daytime2 += 2)
                                    {
                                        int row = 0;
                                        present = 0;
                                        absent = 0;
                                        int epresent = 0;
                                        int eabsent = 0;
                                        for (int i = 1; i < FpSpread2.Sheets[0].RowCount - 4; i = i + 2)
                                        {
                                            row++;
                                            if (FpSpread2.Sheets[0].Rows[row].Visible == true)
                                            {
                                                string vlauecount = FpSpread2.Sheets[0].Cells[row, daytime2].Text;
                                                // FpSpread2.Sheets[0].Cells[row, daytime2].Locked = true;
                                                if (vlauecount.Trim() != "")
                                                {
                                                    if (vlauecount != "A")
                                                    {
                                                        // zzz 28/4/16
                                                        if (vlauecount != "H" && vlauecount != "NA" && vlauecount != "RL")
                                                        {
                                                            //string attndpresent = Convert.ToString(hat1[vlauecount.ToString().Trim()]);
                                                            //if (attndpresent.Trim() != "comp")
                                                            //{
                                                            if (hat1.Contains(vlauecount.ToString().Trim()))
                                                            {
                                                                present++;
                                                            }
                                                            else
                                                            {
                                                                absent++;
                                                            }
                                                            //}
                                                            //else
                                                            //{
                                                            //    absent++;
                                                            //}
                                                        }
                                                    }
                                                    else
                                                    {
                                                        absent++;
                                                    }
                                                }
                                            }
                                            row++;
                                            if (FpSpread2.Sheets[0].Rows[row].Visible == true)
                                            {
                                                string vlauecount = FpSpread2.Sheets[0].Cells[row, daytime2].Text;
                                                if (vlauecount.Trim() != "")
                                                {
                                                    if (vlauecount != "A")
                                                    {
                                                        // zzz 28/4/16
                                                        if (vlauecount != "H" && vlauecount != "NA" && vlauecount != "RL")
                                                        {
                                                            //string attndpresent = Convert.ToString(hat1[vlauecount.ToString().Trim()]);
                                                            //if (attndpresent.Trim() != "comp")
                                                            //{
                                                            //    epresent++;
                                                            //}
                                                            //else
                                                            //{
                                                            //    eabsent++;
                                                            //}
                                                            if (hat1.Contains(vlauecount.ToString().Trim()))
                                                            {
                                                                epresent++;
                                                            }
                                                            else
                                                            {
                                                                eabsent++;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        eabsent++;
                                                    }
                                                }
                                            }
                                        }
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, daytime2].Text = present.ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, daytime2].CellType = tb1;
                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 4].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, daytime2].Text = absent.ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, daytime2].CellType = tb1;
                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 3].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, daytime2].Text = epresent.ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, daytime2].CellType = tb1;
                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 2].Locked = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime2].Text = eabsent.ToString();
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, daytime2].CellType = tb1;
                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Locked = true;
                                    }
                                    if (countadd == 0)
                                    {
                                        btnsave.Visible = true;
                                        btnupdate.Visible = false;
                                    }
                                    else
                                    {
                                        if (countadd != 0)
                                        {
                                            btnsave.Visible = false;
                                            btnupdate.Visible = true;
                                        }
                                    }
                                    FpSpread2.SaveChanges();
                                    if (Convert.ToInt32(Session["item_Value"]) == 1)
                                    {
                                        FpSpread2.Width = 550;
                                        FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                        FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                        FpSpread2.Page.MaintainScrollPositionOnPostBack = false;

                                        FpSpread2.SaveChanges();
                                    }
                                    else if (Convert.ToInt32(Session["item_Value"]) == 2)
                                    {
                                        FpSpread2.Width = 700;
                                    }
                                    else
                                    {
                                        // FpSpread2.Width = 900;
                                        if (checkcount == 1 || checkcount == 2)
                                        {
                                            FpSpread2.Width = 900;

                                        }
                                        else
                                        {
                                            FpSpread2.Width = 1320;
                                        }

                                    }
                                    int height = 100;
                                    {
                                        for (int j = 1; j < FpSpread2.Sheets[0].RowCount; j++)
                                        {
                                            height = height + FpSpread2.Sheets[0].Rows[j].Height;
                                        }
                                        if (height > 600)
                                        {
                                            FpSpread2.Height = 400;
                                        }
                                        else if (height > 500)
                                        {
                                            FpSpread2.Height = height - 200;
                                        }
                                        else if (height > 400)
                                        {
                                            FpSpread2.Height = height - 100;
                                        }
                                        else
                                        {
                                            FpSpread2.Height = height;
                                        }
                                        FpSpread2.SaveChanges();
                                        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                                    }
                                    int count1 = Convert.ToInt32(Session["item_Value"]);
                                    if (count1 > 2)
                                    {
                                        FpSpread2.Sheets[0].FrozenColumnCount = 4;
                                    }
                                    if (spancolumn > 0)
                                    {
                                        FpSpread2.Sheets[0].SpanModel.Add(0, 1, 1, spancolumn - 1);
                                        for (int cl = 0; cl < spancolumn; cl++)
                                        {
                                            FpSpread2.Sheets[0].Cells[0, cl].Text = " ";
                                        }
                                    }
                                    if (noofstaffflag == false)
                                    {
                                        FpSpread2.Sheets[0].Visible = false;
                                        FpSpread2.Visible = false;
                                        panelsecond.Visible = false;
                                        lblreasonleave.Visible = false;
                                        ddlleavereason.Visible = false;
                                        btnselectall.Visible = false;
                                        btndeselectall.Visible = false;
                                        btnsave.Visible = false;
                                        btnupdate.Visible = false;
                                        lblerror1.Visible = false;
                                        lblerror3.Text = "No Records Found";
                                        lblerror3.Visible = true;
                                        lblError.Visible = false;
                                        //lblheaderAttendance.Visible = false;
                                        //imagelabel.Visible = false;
                                        //panelsecond.Attributes.Add("style", "BackColor:white;");
                                    }
                                    else
                                    {
                                        FpSpread2.SaveChanges();
                                        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                                        FpSpread2.CommandBar.Visible = false;
                                        FpSpread2.Sheets[0].Visible = true;
                                        FpSpread2.Visible = true;
                                        lblreasonleave.Visible = true;
                                        ddlleavereason.Visible = true;
                                        panelsecond.Visible = true;
                                        btnselectall.Visible = true;
                                        btndeselectall.Visible = true;
                                        lblError.Visible = false;
                                        if (dateflag == true)
                                        {
                                            lblerror1.Visible = true;
                                        }
                                        if (dateflag == false)
                                        {
                                            lblerror1.Visible = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lblError.Text = "From Date Should Be Less Than To Date";
                                lblError.Visible = true;
                                FpSpread2.Sheets[0].Visible = false;
                                FpSpread2.Visible = false;
                                panelsecond.Visible = false;
                                lblreasonleave.Visible = false;
                                ddlleavereason.Visible = false;
                                btnselectall.Visible = false;
                                btndeselectall.Visible = false;
                                btnsave.Visible = false;
                                btnupdate.Visible = false;
                                lblerror1.Visible = false;
                            }
                        }
                        else
                        {
                            lblError.Text = "No Records Found";
                            lblError.Visible = true;
                            FpSpread2.Sheets[0].Visible = false;
                            FpSpread2.Visible = false;
                            panelsecond.Visible = false;
                            lblreasonleave.Visible = false;
                            ddlleavereason.Visible = false;
                            btnselectall.Visible = false;
                            btndeselectall.Visible = false;
                            btnsave.Visible = false;
                            btnupdate.Visible = false;
                            lblerror1.Visible = false;
                        }
                    }
                }
                else
                {
                    lblError.Text = "Please Select Any One Column Order";
                    lblError.Visible = true;
                    FpSpread2.Sheets[0].Visible = false;
                    FpSpread2.Visible = false;
                    panelsecond.Visible = false;
                    lblreasonleave.Visible = false;
                    ddlleavereason.Visible = false;
                    btnselectall.Visible = false;
                    btndeselectall.Visible = false;
                    btnsave.Visible = false;
                    btnupdate.Visible = false;
                    lblerror1.Visible = false;
                    cblcolumnorder.ClearSelection();
                }
            }
            else
            {
                lblError.Text = "Please Select Any One College";
                lblError.Visible = true;
                FpSpread2.Visible = false;
                FpSpread2.Sheets[0].Visible = false;
                panelsecond.Visible = false;
                lblreasonleave.Visible = false;
                ddlleavereason.Visible = false;
                btnselectall.Visible = false;
                btndeselectall.Visible = false;
                btnsave.Visible = false;
                btnupdate.Visible = false;
                lblerror1.Visible = false;
            }
            if (FpSpread2.Sheets[0].ColumnCount > 1)//Added by srinath 18/2/2015
            {
                FpSpread2.Sheets[0].Columns[1].Visible = false;
            }
            if (nofostaff == 0)
            {
                panelsecond.Visible = false;
            }
            MyClass ms = new MyClass();
            ms.Dispose();
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.ToString();
            lblError.Visible = true;
            d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "Staff_Attendance1.aspx");
        }
    }
    protected void ddlleavereason_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btnaddreason_Click(object sender, EventArgs e)
    {
        panelreason.Visible = true;
        txt_reason.Text = "";
        // panelreason.Attributes.Add("style", "width:200px; height:100px; top:475px; left:756px; position: absolute;");
        capreason.InnerHtml = "Leave Reason";
        btnaddreason.Focus();
    }
    protected void btnremovereason_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlleavereason.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string reason = ddlleavereason.SelectedItem.ToString();
                if (reason != "---Select---")
                {
                    if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                    {
                        string strquery = "delete textvaltable where TextVal='" + reason + "' and TextCriteria='lres' and college_code='" + collegecode + "'";
                        int a = d2.update_method_wo_parameter(strquery, "Text");
                        bindreason();
                    }
                    lblError.Visible = false;
                }
                else
                {
                    lblError.Text = "Select Leave Reason Then Delete";
                    lblError.Visible = true;
                }
            }
            panelreason.Visible = false;
            btnremovereason.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnadd1_Click(object sender, EventArgs e)
    {
        try
        {
            Page.MaintainScrollPositionOnPostBack = true;
            ArrayList testarray = new ArrayList();
            if (ddlleavereason.SelectedValue != "All")
            {
                hat.Clear();
                capvalue = capreason.InnerHtml;
                panelreason.Visible = false;
                if (capvalue == "Leave Reason")
                {
                    if (txt_reason.Text != "")
                    {
                        if (ddlleavereason.Items.Count == 0)
                        {
                            string value = txt_reason.Text;
                            string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + value + "','lres','" + college + "')";
                            int a = d2.insert_method(strquery, hat, "Text");
                            txt_reason.Text = "";
                            bindreason();
                            lblError.Visible = false;
                            if (dateflag == true)
                            {
                                lblerror1.Visible = true;
                            }
                            else
                            {
                                lblerror1.Visible = false;
                            }
                        }
                        else
                        {
                            if (ddlleavereason.Items.Count > 0)
                            {
                                for (int i = 0; i < ddlleavereason.Items.Count; i++)
                                {
                                    testarray.Add(ddlleavereason.Items[i].ToString());
                                }
                                string typevalue = txt_reason.Text;
                                if (testarray.Contains(typevalue) == false)
                                {
                                    string value = txt_reason.Text;
                                    string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + value + "','lres','" + college + "')";
                                    int a = d2.insert_method(strquery, hat, "Text");
                                    txt_reason.Text = "";
                                    bindreason();
                                    lblError.Visible = false;
                                    if (dateflag == true)
                                    {
                                        lblerror1.Visible = true;
                                    }
                                    else
                                    {
                                        lblerror1.Visible = false;
                                    }
                                }
                                else
                                {
                                    lblError.Text = "Entered Leave Reason is Already Exists";
                                    lblError.Visible = true;
                                    lblerror1.Visible = false;
                                    panelreason.Visible = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        lblError.Text = "Please Entre Leave Reason";
                        lblError.Visible = true;
                        lblerror1.Visible = false;
                        panelreason.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindreason()
    {
        height = 0;
        ddlleavereason.Items.Clear();
        string collegecode = Session["collegecode"].ToString();
        string query = "select TextCode,Textval from textvaltable where TextCriteria='lres' and college_code=" + collegecode + "";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlleavereason.DataSource = ds;
            ddlleavereason.DataTextField = "Textval";
            ddlleavereason.DataValueField = "TextCode";
            ddlleavereason.DataBind();
        }
        if (ddlleavereason.Items.Count > 0)
        {
            for (int i = 0; i < ddlleavereason.Items.Count; i++)
            {
                height++;
            }
        }
        sarray2 = new string[height + 2];
        sarray5 = new string[height + 1];
        ddlleavereason.Items.Insert(0, "---Select---");
        panelreason.Visible = false;
    }
    protected void btnexit1_Click(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;
        panelreason.Visible = false;
        lblError.Visible = false;
        if (dateflag == true)
        {
            lblerror1.Visible = true;
        }
        else
        {
            lblerror1.Visible = false;
        }
        //
    }
    protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {

            FpSpread2.SaveChanges();
            Dictionary<string, string> diclea = new Dictionary<string, string>();
            int countvalue1 = Convert.ToInt32(Session["item_Value"]);
            countvalue1 = countvalue1 + 3;
            string actrow = FpSpread2.Sheets[0].ActiveRow.ToString();  //e.SheetView.ActiveRow.ToString();
            string actcol = FpSpread2.Sheets[0].ActiveColumn.ToString();  //e.SheetView.ActiveColumn.ToString();
            string txtval = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(actcol)].Text);
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
                if (!diclea.ContainsKey("P"))
                    diclea.Add("P", "P");
                if (!diclea.ContainsKey("A"))
                    diclea.Add("A", "A");
            }
            if (leave_apply_flage == true)
            {
                if (Convert.ToInt32(actcol) >= countvalue1)
                {
                    if (flag_true == false && actrow != "0")
                    {
                        //string seltext = "";
                        //seltext = e.EditValues[Convert.ToInt32(actcol)].ToString();
                        //if (seltext != "System.Object" && seltext.Trim() != "Select For All" && seltext.Trim() != "")
                        //{
                        if (txtval != "" && txtval != "Select For All")
                        {
                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt16(actcol)].Text = txtval; //seltext.ToString();
                        }
                        if (diclea.ContainsKey(txtval.ToString().Trim().ToLower()))
                        {
                            if (leave_apply_flage == true)
                            {
                                leave_apply = 0;
                                string mode_value = "Half";
                                flag_true = true;
                                string m_temp = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(Session["item_Value"]) + 2].Text);
                                bindapplydate(actrow, mode_value, m_temp);
                                if (leave_apply != 0)
                                {
                                    e.Handled = true;
                                    FpSpread2.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt16(actcol)].Text = "  ";
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Enter leave is not available for this HR year')", true);
                                }
                                else
                                {
                                }
                            }
                        }
                    }
                }
                //string seltext = "";
                //string value = Convert.ToString (FpSpread2.Sheets[0].Cells[Convert.ToInt32 (actrow), Convert.ToInt32 (actcol)].Text);
                //seltext = e.EditValues[Convert.ToInt32(actcol)].ToString();
            }
            if (flag_true == false && actrow == "0")
            {
                //string seltext = "";
                for (int j = 1; j < Convert.ToInt16(FpSpread2.Sheets[0].RowCount - 4); j++)
                {
                    actcol = e.SheetView.ActiveColumn.ToString();
                    //string value = e.EditValues[0].ToString();
                    //e.Handled = true;
                    //seltext = e.EditValues[Convert.ToInt32(actcol)].ToString();
                    //if (seltext != "System.Object" && seltext.Trim() != "Select For All")
                    //{
                    if (txtval != "" && txtval != "Select For All")
                    {
                        string applyvalue1 = "";
                        if (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Locked == false)
                        {
                            applyvalue1 = Convert.ToString(FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)));
                            if (applyvalue1 != "" && applyvalue1 != null)
                            {
                                //Hiodden by srinath 3/2/2015
                                if (applyvalue1 != "OD")
                                {
                                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = txtval;  //seltext.ToString();
                                }
                            }
                            else
                            {
                                FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = txtval;  //seltext.ToString();
                            }
                        }
                    }
                }
                flag_true = true;
            }
            if (flag_true == false && actcol == "1")
            {
                //int colcnt;
                //int r = (int)e.CommandArgument;
                //colcnt = e.EditValues.Count - 1;
                actcol = FpSpread2.Sheets[0].ActiveColumn.ToString();
                //string seltext = e.EditValues[1].ToString();
                //e.Handled = true;
                int j = Convert.ToInt32(Session["item_Value"]);
                j = j + 3;
                for (; j <= Convert.ToInt16(FpSpread2.Sheets[0].ColumnCount - 1); j += 2)
                {
                    if (txtval != "Select For All" && txtval != "")
                    {
                        if (txtval == "P" || txtval == "A" || txtval == "PER" || txtval == "LA")
                        {
                            flag_true = true;
                        }
                        int row1 = Convert.ToInt32(actrow);
                        int row = Convert.ToInt16(actrow);
                        row++;
                        string applyvalue = "";
                        string applyvalue1 = "";
                        applyvalue = Convert.ToString(FpSpread2.Sheets[0].GetText(Convert.ToInt16(row1), j));
                        if (applyvalue != "" && applyvalue != null)
                        {
                            if (FpSpread2.Sheets[0].Cells[Convert.ToInt16(row1), j].Locked == false)
                            {
                                if (applyvalue != "OD")//Hiodden by srinath 3/2/2015
                                {
                                    FpSpread2.Sheets[0].Cells[Convert.ToInt16(row1), j].Text = txtval.ToString();
                                }
                            }
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[Convert.ToInt16(row1), j].Text = txtval.ToString();
                        }
                        applyvalue1 = Convert.ToString(FpSpread2.Sheets[0].GetText(Convert.ToInt16(row), j));
                        if (applyvalue1 != "" && applyvalue1 != null)
                        {
                            if (FpSpread2.Sheets[0].Cells[Convert.ToInt16(row), j].Locked == false)
                            {//Hiodden by srinath 3/2/2015
                                if (applyvalue1 != "OD")
                                {
                                    FpSpread2.Sheets[0].Cells[row, j].Text = txtval.ToString();
                                }
                            }
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[row, j].Text = txtval.ToString();
                        }
                    }
                }
                if (leave_apply_flage == true)
                {
                    if (flag_true == false)
                    {
                        if (txtval != "Select For All" && txtval.Trim() != "")
                        {
                            flag_true = true;
                            string mode_value = "Full";
                            string m_temp = "M";
                            bindapplydate(actrow, mode_value, m_temp);
                            if (leave_apply != 0)
                            {
                                int jk = Convert.ToInt32(Session["item_Value"]);
                                jk = jk + 3;
                                for (; jk <= Convert.ToInt16(FpSpread2.Sheets[0].ColumnCount - 1); jk += 2)
                                {
                                    int row1 = Convert.ToInt32(actrow);
                                    int row = Convert.ToInt16(actrow);
                                    row++;
                                    string applyvalue111 = "";
                                    applyvalue111 = Convert.ToString(FpSpread2.Sheets[0].GetText(Convert.ToInt16(row1), jk));
                                    if (FpSpread2.Sheets[0].Cells[Convert.ToInt16(row1), jk].Locked == false)
                                    {
                                        if (applyvalue111 != "OD")
                                        {
                                            FpSpread2.Sheets[0].Cells[row1, jk].Text = "";
                                        }
                                    }
                                    string app = "";
                                    app = Convert.ToString(FpSpread2.Sheets[0].GetText(Convert.ToInt16(row), jk));
                                    if (FpSpread2.Sheets[0].Cells[Convert.ToInt16(row), jk].Locked == false)
                                    {
                                        if (app != "OD")
                                        {
                                            FpSpread2.Sheets[0].Cells[row, jk].Text = "";
                                        }
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Enter leave is not available for this HR year')", true);
                            }
                        }
                    }
                }
            }
            if (count == false)
            {
                int daytime = Convert.ToInt32(Session["item_Value"]);
                daytime = daytime + 3;
                if (actcol == "1")
                {
                    if (actrow == last)
                    {
                        e.Handled = true;
                    }
                }
                for (int i = daytime; i < FpSpread2.Sheets[0].ColumnCount; i += 2)
                {
                    present = 0;
                    absent = 0;
                    int epresnt = 0;
                    int eabsent = 0;
                    string actrowj = FpSpread2.ActiveSheetView.ActiveRow.ToString();
                    string actcolj = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
                    if (FpSpread2.Sheets[0].ColumnHeader.Cells[1, Convert.ToInt32(actcolj)].Text != "Reason Entry")
                    {
                        for (int j = 1; j < FpSpread2.Sheets[0].RowCount - 4; j = j + 2)
                        {
                            if (FpSpread2.Sheets[0].Rows[j].Visible == true)
                            {
                                string countvalue = FpSpread2.Sheets[0].Cells[j, i].Text;
                                if (countvalue.Trim() != "")
                                {
                                    if (countvalue != "H" && countvalue != "NA" && countvalue != "RL")
                                    {
                                        if (countvalue != "A")
                                        {
                                            //string attndvalue = Convert.ToString(hat1[countvalue.ToString()]);
                                            //if (attndvalue != "comp")
                                            //{
                                            if (hat1.Contains(countvalue.ToString())) // zzz 28/4/16
                                            {
                                                present++;
                                            }
                                            else
                                            {
                                                absent++;
                                            }
                                        }
                                        else
                                        {
                                            absent++;
                                        }
                                    }
                                }
                            }
                            if (FpSpread2.Sheets[0].Rows[j + 1].Visible == true)
                            {
                                string countvalue = FpSpread2.Sheets[0].Cells[j + 1, i].Text;
                                if (countvalue.Trim() != "")
                                {
                                    if (countvalue != "H" && countvalue != "NA" && countvalue != "RL")
                                    {
                                        if (countvalue != "A")
                                        {
                                            //string attndvalue = Convert.ToString(hat1[countvalue.ToString()]);
                                            //if (attndvalue != "comp")
                                            //{
                                            if (hat1.Contains(countvalue.ToString())) // zzz 28/4/16
                                            {
                                                epresnt++;
                                            }
                                            else
                                            {
                                                eabsent++;
                                            }
                                        }
                                        else
                                        {
                                            eabsent++;
                                        }
                                    }
                                }
                            }
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, Convert.ToInt32(i)].Text = present.ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, Convert.ToInt32(i)].Text = absent.ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, Convert.ToInt32(i)].Text = epresnt.ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(i)].Text = eabsent.ToString();
                    }
                }
                count = true;
            }
            Page.MaintainScrollPositionOnPostBack = false;//delsi1306
            FpSpread2.Page.MaintainScrollPositionOnPostBack = false;//delsi1406


        }
        catch (Exception ex)
        {
        }
    }
    public void bindleavespread()
    {
        if (FpSpread2.Sheets[0].RowCount > 0)
        {
            for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
            {
                string leavevalue = FpSpread2.Sheets[0].GetText(i + 1, 1).ToString();
            }
        }
    }
    protected void btnselectall_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.SaveChanges();
            leave.Clear();
            string attendquery = "Select distinct status,shortname  from leave_category where college_code='" + ddlcollege.SelectedItem.Value + "' and ISNULL(status,'0')<>'0'";
            ds = d2.select_method_wo_parameter(attendquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int attnd = 0; attnd < ds.Tables[0].Rows.Count; attnd++)
                {
                    if (leave.Contains(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim()) == false)
                    {
                        if (ds.Tables[0].Rows[attnd]["status"].ToString().Trim().ToLower() == "0")//Added by srinath 23/jan2014
                        {
                            hat1.Add(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim(), ds.Tables[0].Rows[attnd]["status"].ToString().Trim());
                        }
                        leave.Add(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim());
                    }
                }
            }
            if (!hat1.ContainsKey("P"))
                hat1.Add("P", "0");
            if (!hat1.ContainsKey("A"))
                hat1.Add("A", "2");
            int daytime = Convert.ToInt32(Session["item_Value"]);
            daytime = daytime + 3;
            for (int i = daytime; i < FpSpread2.Sheets[0].ColumnCount; i += 2)
            {
                present = 0;
                absent = 0;
                for (int j = 1; j < FpSpread2.Sheets[0].RowCount - 4; j++)
                {
                    if (FpSpread2.Sheets[0].Rows[j].Visible == true)
                    {
                        string odvalue = FpSpread2.Sheets[0].Cells[j, i].Text;
                        if (odvalue != "" && odvalue != null)
                        {
                            //if (odvalue != "H")
                            //{
                            //string result = Convert.ToString(hat1[odvalue]);
                            //if (result != "comp")
                            //{
                            //    if (leave.Contains(odvalue) == false)
                            //    {
                            //if (odvalue != "OD")
                            //{
                            if (FpSpread2.Sheets[0].Cells[j, i].Locked == false)
                            {
                                FpSpread2.Sheets[0].Cells[j, i].Text = "P";
                                present++;
                            }
                            //else
                            //{
                            //    present++;
                            //}
                            //}
                            //else
                            //{
                            //    present++;
                            //}
                            //    }
                            //    else
                            //    {
                            //        // present++;
                            //        absent++;
                            //    }
                            //}
                            //else
                            //{
                            //    absent++;
                            //}
                            //}
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[j, i].Text = "P";
                            present++;
                        }
                    }
                }
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, i].Text = (present / 2).ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, i].Text = (absent / 2).ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, i].Text = (present / 2).ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, i].Text = (absent / 2).ToString();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btndeselectall_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.SaveChanges();
            int daytime = Convert.ToInt32(Session["item_Value"]);
            daytime = daytime + 3;
            string attendquery = "Select distinct status,shortname  from leave_category where college_code='" + ddlcollege.SelectedItem.Value + "' and ISNULL(status,'0')<>'0'";
            ds = d2.select_method_wo_parameter(attendquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int attnd = 0; attnd < ds.Tables[0].Rows.Count; attnd++)
                {
                    if (leave.Contains(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim()) == false)
                    {
                        if (ds.Tables[0].Rows[attnd]["status"].ToString().Trim().ToLower() == "0")//Added by srinath 23/jan2014
                        {
                            hat1.Add(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim(), ds.Tables[0].Rows[attnd]["status"].ToString().Trim());
                        }
                        leave.Add(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim());
                    }
                }
            }
            if (!hat1.ContainsKey("P"))
                hat1.Add("P", "0");
            if (!hat1.ContainsKey("A"))
                hat1.Add("A", "2");
            for (int i = daytime; i < FpSpread2.Sheets[0].ColumnCount; i += 2)
            {
                present = 0;
                absent = 0;
                for (int j = 1; j < FpSpread2.Sheets[0].RowCount - 4; j++)
                {
                    if (FpSpread2.Sheets[0].Rows[j].Visible == true)
                    {
                        string odvalue = FpSpread2.Sheets[0].Cells[j, i].Text;
                        if (odvalue != "")
                        {
                            if (odvalue != "H")
                            {
                                //if (odvalue != "OD")
                                //{
                                if (FpSpread2.Sheets[0].Cells[j, i].Locked == false)
                                {
                                    FpSpread2.Sheets[0].Cells[j, i].Text = "";
                                    FpSpread2.Sheets[0].Cells[j, i + 1].Text = "";
                                }
                                else
                                {
                                    present++;
                                }
                                //}
                                //else
                                //{
                                //    present++;
                                //}
                            }
                        }
                    }
                }
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, i].Text = (present / 2).ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, i].Text = (absent / 2).ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, i].Text = (present / 2).ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, i].Text = (absent / 2).ToString();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            int not = 0;
            FpSpread2.SaveChanges();
            flag_updatesave = 0;
            leave_apply = 0;
            conformationflage = false;
            string mattndvalue = "";
            string eattndvalue = "";
            int countvalue = Convert.ToInt32(Session["item_Value"]);
            countvalue = countvalue + 3;
            int notupdate = 0;
            for (int i = countvalue; i < FpSpread2.Sheets[0].ColumnCount; i += 2)
            {
                count = 0;
                not = 0;
                notupdate = 0;
                for (int k = 1; k < FpSpread2.Sheets[0].RowCount - 5; k += 2)
                {
                    if (FpSpread2.Sheets[0].Rows[k].Visible == true)
                    {
                        not++;
                        mattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[k, i].Text);
                        eattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[k, i + 1].Text);
                        if (mattndvalue.Trim() != "" || eattndvalue.Trim() != "")
                        {
                            conformationflage = true;
                            count++;
                        }
                        else
                        {
                            notupdate++;
                        }
                    }
                }
            }
            if (conformationflage == false)
            {
                lblMessage.Text = "Please Update Attendance";
                mpemsgboxdelete.Show();
            }
            else if (conformationflage == true)
            {
                if (not == count)
                {
                    flag_updatesave = 1;
                    lblMessage.Text = "Do You Want Save " + count + " Staff Attendance ?";
                    mpemsgboxdelete.Show();
                }
                else
                {
                    flag_updatesave = 1;
                    lblMessage.Text = " " + notupdate + " Staff Attendance Are not Updated. Do You Want Save The Attendance ?";
                    mpemsgboxdelete.Show();
                }
            }
        }
        catch
        {
        }
    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            int not = 0;
            leave_apply = 0;
            FpSpread2.SaveChanges();
            flag_updatesave = 0;
            conformationflage = false;
            string mattndvalue = "";
            string eattndvalue = "";
            int countvalue = Convert.ToInt32(Session["item_Value"]);
            countvalue = countvalue + 3;
            int notupdate = 0;
            FpSpread2.SaveChanges();
            for (int i = countvalue; i < FpSpread2.Sheets[0].ColumnCount; i += 2)
            {
                count = 0;
                not = 0;
                notupdate = 0;
                for (int k = 1; k < FpSpread2.Sheets[0].RowCount - 5; k += 2)
                {
                    if (FpSpread2.Sheets[0].Rows[k].Visible == true)
                    {
                        not++;
                        mattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[k, i].Text);
                        eattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[k, i + 1].Text);
                        if (mattndvalue.Trim() != "" || eattndvalue.Trim() != "")
                        {
                            conformationflage = true;
                            count++;
                        }
                        else
                        {
                            notupdate++;
                        }
                    }
                }
            }
            if (conformationflage == false)
            {
                lblMessage.Text = "Please Update Attendance";
                mpemsgboxdelete.Show();
            }
            else if (conformationflage == true)
            {
                if (not == count)
                {
                    flag_updatesave = 2;
                    lblMessage.Text = "Do You Want Update " + count + " Staff Attendance ?";
                    mpemsgboxdelete.Show();
                }
                else
                {
                    flag_updatesave = 2;
                    lblMessage.Text = " " + notupdate + " Staff Attendance Are not Updated. Do You Want Save The Attendance ?";
                    mpemsgboxdelete.Show();
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "Staff_Attendance1.aspx");
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //if (conformationflage == true)
        //{
        FpSpread2.SaveChanges();
        bindsaveupdate();
        loardspread();
        //}
        //else if (conformationflage == false)
        //{
        mpemsgboxdelete.Hide();
        //}
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpemsgboxdelete.Hide();
    }
    public void bindsaveupdate()
    {
        try
        {
            FpSpread2.SaveChanges();
            ArrayList arrayreson = new ArrayList();
            ArrayList addleavetype = new ArrayList();
            ArrayList hrpaymonth = new ArrayList();
            Hashtable hr_pay_date = new Hashtable();
            ArrayList sunday_add = new ArrayList();
            ArrayList add_leave_array = new ArrayList();
            DataSet ds55 = new DataSet();
            string daycount = "";
            string monyear = "";
            string staffcode = "";
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
                //if (sdate.ToString("dddd") != "Sunday")
                //{
                arrayreson.Add(sdate.ToString("d/MM/yyyy"));
                //}
                //else
                //{
                //    sunday_add.Add(sdate.ToString("d/MM/yyyy"));
                //}
                sdate = sdate.AddDays(1);
            }
            //Added by srinath 3/2/2015
            //string getlevetype = "Select distinct status,shortname,category from leave_category where status<>'' and college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            string getlevetype = "Select distinct status,shortname,category from leave_category where college_code='" + ddlcollege.SelectedValue.ToString() + "'";
            DataSet dslave = d2.select_method_wo_parameter(getlevetype, "Text");
            Dictionary<string, string> dicleave = new Dictionary<string, string>();
            for (int le = 0; le < dslave.Tables[0].Rows.Count; le++)
            {
                if (!dicleave.ContainsKey(dslave.Tables[0].Rows[le]["shortname"].ToString()))
                {
                    dicleave.Add(dslave.Tables[0].Rows[le]["shortname"].ToString(), dslave.Tables[0].Rows[le]["category"].ToString());
                }
            }
            if (!dicleave.ContainsKey("P"))
                dicleave.Add("P", "Present");
            if (!dicleave.ContainsKey("A"))
                dicleave.Add("A", "Absent");
            //************************ENd
            string mattndvalue = "";
            string mreason = "";
            string eattndvalue = "";
            string ereason = "";
            string joinvalue = "";
            string joinreason = "";
            int countvalue = Convert.ToInt32(Session["item_Value"]);
            countvalue = countvalue + 3;
            int s = 0;
            for (int i = countvalue; i < FpSpread2.Sheets[0].ColumnCount; i += 2)//Modified by srinath 3Feb2015
            {
                double count_check_attend = 0;
                //for (int s = 0; s < arrayreson.Count; s++)
                if (s < arrayreson.Count)
                {
                    string date = arrayreson[s].ToString();
                    string[] split_d = date.Split(new Char[] { '/' });
                    string strdate = split_d[0].ToString();
                    string Atmonth = split_d[1].ToString();
                    string Atyear = split_d[2].ToString();
                    string atmonth1 = (Atmonth.TrimStart('0'));
                    monyear = atmonth1 + "/" + Atyear;
                    string getdate = atmonth1 + '/' + strdate + '/' + Atyear;
                    DateTime frodate = new DateTime();
                    frodate = Convert.ToDateTime(getdate);
                    FpSpread2.SaveChanges();
                    for (int k = 1; k < FpSpread2.Sheets[0].RowCount - 5; k++)
                    {
                        if (FpSpread2.Sheets[0].Rows[k].Visible == true)
                        {
                            string applno = string.Empty;
                            string applid = string.Empty;
                            string[] split_staff_code = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(k), 0].Tag).Split('-');
                            if (split_staff_code.Length > 0)
                            {
                                staffcode = Convert.ToString(split_staff_code[0]);
                            }
                            //   staffcode = Convert.ToString(FpSpread2.Sheets[0].Cells[k, 0].Tag);
                            // applno = d2.GetFunction("select appl_no from staffmaster where staff_code='" + staffcode + "'");
                            // applid = d2.GetFunction("select appl_id from staff_appl_master where appl_no='" + applno + "'");

                            mattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[k, i].Text);
                            FpSpread2.SaveChanges();
                            mreason = Convert.ToString(FpSpread2.Sheets[0].Cells[k, i + 1].Text);
                            k++;
                            eattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[k, i].Text);
                            ereason = Convert.ToString(FpSpread2.Sheets[0].Cells[k, i + 1].Text);
                            //**********************Added by srinath 3/2/2015*** For Leave Apply**************************
                            Boolean morleva = false;
                            Boolean eveleva = false;
                            Boolean fullleave = false;
                            Boolean leavflga = false;
                            Double getleav = 0;
                            string leavemode = "";
                            string halfday = null;
                            if (dicleave.ContainsKey(mattndvalue))
                            {
                                morleva = true;
                                leavflga = true;
                                getleav = getleav + 0.5;
                                leavemode = dicleave[mattndvalue];
                            }
                            if (dicleave.ContainsKey(eattndvalue))
                            {
                                leavflga = true;
                                eveleva = true;
                                getleav = getleav + 0.5;
                                leavemode = dicleave[eattndvalue];
                                if (morleva == true)
                                {
                                    fullleave = true;
                                }
                            }
                            if (fullleave == false)
                            {
                                halfday = "1";
                                if (morleva == true)
                                {
                                    leavemode = "HalfDay@fh@" + leavemode + "";
                                }
                                else if (eveleva == true)
                                {
                                    leavemode = "HalfDay@sh@" + leavemode + "";
                                }
                            }
                            //*********************************End********************************
                            if (mattndvalue != "" && eattndvalue != "")
                            {
                                joinvalue = mattndvalue + "-" + eattndvalue;
                                joinreason = mreason + "-" + ereason;
                            }
                            else if (mattndvalue == "" && eattndvalue == "")
                            {
                                joinvalue = "";
                                joinreason = "";
                            }
                            else
                            {
                                if (mattndvalue != "" && eattndvalue == "")
                                {
                                    joinvalue = mattndvalue + "-" + eattndvalue;
                                    joinreason = mreason + "-" + "";
                                }
                                else if (mattndvalue == "" && eattndvalue != "")
                                {
                                    joinvalue = mattndvalue + "-" + eattndvalue;
                                    joinreason = "" + "-" + ereason;
                                }
                            }
                            //delsi changing procedure 09022019
                            string updatequery = "";
                            //updatequery = "if Not EXISTS(select * from staff_attnd where staff_code in('" + staffcode + "')and mon_year ='" + monyear + "')";
                            //updatequery = updatequery + " Begin";
                            //updatequery = updatequery + " insert into staff_attnd(staff_code,mon_year,lastWD,[" + strdate + "]) values('" + staffcode + "','" + monyear + "'," + strdate + ",'" + joinvalue + "')";
                            //updatequery = updatequery + " End";
                            //updatequery = updatequery + " Else";
                            //updatequery = updatequery + " Begin";
                            //updatequery = updatequery + " update staff_attnd set lastWD=" + strdate + " , [" + strdate + "]='" + joinvalue + "' where staff_code='" + staffcode + "' and mon_year ='" + monyear + "'";
                            //updatequery = updatequery + " End";
                            string updatequery1 = "";
                           
                            //delsi changing procedure 09022019
                            hat.Clear();
                            hat.Add("staff_codes ", staffcode);
                            hat.Add("mon_years", monyear);
                            hat.Add("dates", strdate);
                            hat.Add("values", joinvalue);

                            int a = d2.update_method_with_parameter("staff_attndproc", hat, "sp");
                          

                            //string query1 = "SELECT distinct r.ReqApproveStage,r.ReqAppNo,ReqAppStatus,CASE WHEN r.RequestType = 5 THEN 'Leave Request' END RequestType,RequestCode,CONVERT(VARCHAR(11),HalfDate,103) as HalfDate,CONVERT(VARCHAR(11),RequestDate,103) as RequestDate,CONVERT(VARCHAR(11),LeaveFrom,103) as LeaveFrom,CONVERT(VARCHAR(11),LeaveTo,103) as LeaveTo,CASE WHEN IsHalfDay = 1 THEN 'Half Day' ELSE 'Full Day' END Leave,case when LeaveSession=1 then 'Morning' when LeaveSession=2 then 'Evening' else 'Full Day' end LeaveSession,m.Staff_Code,Staff_Name,(Select MasterValue FROM CO_MasterValues T WHERE r.GateReqReason = t.MasterCode) GateReqReason,(select category from  leave_category l where r.LeaveMasterFK=l.LeaveMasterPK) LeaveMasterFK FROM RQ_Requisition R,staff_appl_master A,staffmaster M,leave_category C  WHERE R.ReqAppNo = A.appl_id AND A.appl_no = M.appl_no and r.LeaveMasterFK = LeaveMasterPK  and r.RequestType=5 and  r.ReqAppStatus=1 and r.college_code='" + ddlcollege.SelectedItem.Value + "' and R.ReqAppNo='" + applid + "'  and LeaveFrom='" + getdate + "'";

                            //reqds = d2.select_method_wo_parameter(query1, "text");
                            //if (reqds.Tables[0].Rows.Count > 0)//delsi 2807
                            //{
                            //    leavflga = false;

                            //}
                            //Hashtable hat11 = new Hashtable();
                            //hat11.Clear();
                            //hat11.Add("staff_codes ", staffcode);
                            //hat11.Add("mon_years", monyear);
                            //hat11.Add("dates", strdate);
                            //hat11.Add("values", joinreason);
                            //int b = d2.update_method_with_parameter("staff_attndLeaveReason", hat11, "sp");


                            updatequery1 = "if Not EXISTS(Select * from Staff_Leavereason where staff_code='" + staffcode + "'and monyear='" + monyear + "')";
                            updatequery1 = updatequery1 + " Begin";
                            updatequery1 = updatequery1 + " insert into Staff_Leavereason (staff_code,monyear,[" + strdate + "]) values ('" + staffcode + "','" + monyear + "','" + joinreason + "')";
                            updatequery1 = updatequery1 + " End";
                            updatequery1 = updatequery1 + " Else";
                            updatequery1 = updatequery1 + " Begin";
                            updatequery1 = updatequery1 + " update Staff_Leavereason set [" + strdate + "]='" + joinreason + "'  where staff_code='" + staffcode + "' and monyear ='" + monyear + "'";
                            updatequery1 = updatequery1 + " End";
                            hat.Clear();
                          //  int a = d2.insert_method(updatequery, hat, "Text");
                            int b = d2.insert_method(updatequery1, hat, "Text");
                            //**********************Added by srinath 3/2/2015*** For Leave Apply**************************
                            if (leavflga == true)
                            {
                                //string strupdateapplyn = "if Not Exists( select * from staff_leave_details where staff_code='" + staffcode + "' and '" + getdate + "' between fdate and tdate)";
                                //strupdateapplyn = strupdateapplyn + " insert into staff_leave_details (staff_code,fdate,tdate,adate,lt_taken,status,college_code,apply_approve,no_days,Half_Days,half,directapply)";
                                //strupdateapplyn = strupdateapplyn + "values('" + staffcode + "','" + getdate + "','" + getdate + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + leavemode + "','A','" + ddlcollege.SelectedValue.ToString() + "','1','" + getleav + "','" + halfday + "','" + halfday + "','1')";
                                //b = d2.insert_method(strupdateapplyn, hat, "Text");
                            }
                            //******************End***********************
                        }
                    }
                    s++;
                }
            }
            if (leave_apply_flage == true)
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
                    columnfield = " group_code='" + group_code + "'";
                }
                else
                {
                    columnfield = " usercode='" + Session["usercode"] + "'";
                }
                string value_query = "select Rights from Staff_Attendance_Setting where " + columnfield + "";
                ds55.Clear();
                ds55 = d2.select_method_wo_parameter(value_query, "Text");
                if (ds55.Tables[0].Rows.Count > 0)
                {
                    string leave_Value = ds55.Tables[0].Rows[0]["Rights"].ToString();
                    if (leave_Value.Contains(',') == true)
                    {
                        string[] split_leave_value = leave_Value.Split(',');
                        if (leave_Value.Length > 0)
                        {
                            for (int le = 0; le <= split_leave_value.GetUpperBound(0); le++)
                            {
                                if (split_leave_value[le].ToString().Trim() != "")
                                {
                                    add_leave_array.Add(split_leave_value[le].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        add_leave_array.Add(leave_Value.ToString());
                    }
                }
                #region Add Sunday
                //if (sunday_add.Count > 0)
                //{
                //    for (int k = 1; k < FpSpread2.Sheets[0].RowCount - 5; k += 2)
                //    {
                //        if (FpSpread2.Sheets[0].Rows[k].Visible == true)
                //        {
                //            string[] split_staff_code = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(k), 0].Tag).Split('-');
                //            if (split_staff_code.Length > 0)
                //            {
                //                staffcode = Convert.ToString(split_staff_code[0]);
                //            }
                //            string getval = d2.GetFunction("select Leavetype from individual_Leave_type where staff_code='" + staffcode + "'");
                //            if (newhash.ContainsKey(staffcode) == true)
                //            {
                //                string befor_m = "";
                //                string befor_e = "";
                //                string after_m = "";
                //                string after_e = "";
                //                for (int sun = 0; sun < sunday_add.Count; sun++)
                //                {
                //                    string hashdate = sunday_add[sun].ToString();
                //                    string[] split_hashdate = hashdate.Split('/');
                //                    DateTime date_time_con = Convert.ToDateTime(split_hashdate[1].ToString() + "/" + split_hashdate[0].ToString() + "/" + split_hashdate[2].ToString());
                //                    DateTime date_time_con1 = date_time_con.AddDays(-1);
                //                    string date = date_time_con1.ToString("dd/MM/yyyy");
                //                    string[] split_d = date.Split(new Char[] { '/' });
                //                    string strdate = split_d[0].ToString();
                //                    strdate = strdate.TrimStart('0');
                //                    strdate = "[" + strdate + "]";
                //                    string Atmonth = split_d[1].ToString();
                //                    string Atyear = split_d[2].ToString();
                //                    string atmonth1 = (Atmonth.TrimStart('0'));
                //                    monyear = atmonth1 + "/" + Atyear;
                //                    string selectquery = "";
                //                    selectquery = "select " + strdate + " from staff_attnd where staff_code='" + staffcode + "' and mon_year ='" + monyear + "'";
                //                    ds55.Clear();
                //                    ds55 = d2.select_method_wo_parameter(selectquery, "Text");
                //                    if (ds55.Tables[0].Rows.Count > 0)
                //                    {
                //                        string attendace_value = ds55.Tables[0].Rows[0][0].ToString();
                //                        if (attendace_value.Trim() != "")
                //                        {
                //                            string[] split_attendance_value = attendace_value.Split('-');
                //                            if (split_attendance_value.Length > 0)
                //                            {
                //                                befor_m = Convert.ToString(split_attendance_value[0]);
                //                                befor_e = Convert.ToString(split_attendance_value[1]);
                //                            }
                //                        }
                //                    }
                //                    DateTime date_time_con2 = date_time_con.AddDays(1);
                //                    string date11 = date_time_con2.ToString("dd/MM/yyyy");
                //                    string[] split_d1 = date11.Split(new Char[] { '/' });
                //                    string strdate1 = split_d1[0].ToString();
                //                    strdate1 = strdate1.TrimStart('0');
                //                    strdate1 = "[" + strdate1 + "]";
                //                    string Atmonth1 = split_d1[1].ToString();
                //                    string Atyear1 = split_d1[2].ToString();
                //                    string atmonth11 = (Atmonth1.TrimStart('0'));
                //                    monyear = atmonth11 + "/" + Atyear1;
                //                    string selectquery1 = "";
                //                    selectquery1 = "select " + strdate1 + " from staff_attnd where staff_code='" + staffcode + "' and mon_year ='" + monyear + "'";
                //                    ds55.Clear();
                //                    ds55 = d2.select_method_wo_parameter(selectquery1, "Text");
                //                    if (ds55.Tables[0].Rows.Count > 0)
                //                    {
                //                        string attendace_value = ds55.Tables[0].Rows[0][0].ToString();
                //                        if (attendace_value.Trim() != "")
                //                        {
                //                            string[] split_attendance_value = attendace_value.Split('-');
                //                            if (split_attendance_value.Length > 0)
                //                            {
                //                                after_m = Convert.ToString(split_attendance_value[0]);
                //                                after_e = Convert.ToString(split_attendance_value[1]);
                //                            }
                //                        }
                //                    }
                //                    if (befor_e.Trim() != "" && befor_m.Trim() != "" || after_m.Trim() != "" && after_e.Trim() != "")
                //                    {
                //                        if (befor_m.ToString() == after_m.ToString() && befor_e.ToString() == after_e.ToString())
                //                        {
                //                            if (add_leave_array.Contains(befor_m) == true && add_leave_array.Contains(befor_e) == true)
                //                            {
                //                                string gettype = dicleave[befor_m].ToString();
                //                                string[] stv = getval.Split('\\');
                //                                for (int suh = 0; suh <= stv.GetUpperBound(0); suh++)
                //                                {
                //                                    string[] sphe = stv[suh].Split(';');
                //                                    if (sphe[0].Trim().ToLower() == gettype.Trim().ToLower())
                //                                    {
                //                                        if (sphe[4] == "1")
                //                                        {
                //                                            string seconddate = date_time_con.ToString("dd");
                //                                            string month123 = date_time_con.ToString("MM");
                //                                            string year123 = date_time_con.ToString("yyyy");
                //                                            string atmonth1111 = (month123.TrimStart('0'));
                //                                            monyear = atmonth1111 + "/" + year123;
                //                                            seconddate = seconddate.TrimStart('0');
                //                                            seconddate = "[" + seconddate + "]";
                //                                            string join_value = after_m + "-" + after_e;
                //                                            string updatequery = "if not exists (select " + seconddate + " from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monyear + "') insert into staff_attnd (" + seconddate + ",staff_code,mon_year) values ('" + join_value + "','" + staffcode + "','" + monyear + "') else update staff_attnd set " + seconddate + "='" + join_value + "' where staff_code='" + staffcode + "' and mon_year='" + monyear + "'";
                //                                            int result_Value = d2.update_method_wo_parameter(updatequery, "Text");
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion
                //else
                //{
                #region Sunday Holiday Entry
                //if (d452.ToString("dddd") == "Sunday")
                //{
                //    for (int k = 1; k < FpSpread2.Sheets[0].RowCount - 5; k += 2)
                //    {
                //        if (FpSpread2.Sheets[0].Rows[k].Visible == true)
                //        {
                //            string[] split_staff_code = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(k), 0].Tag).Split('-');
                //            if (split_staff_code.Length > 0)
                //            {
                //                staffcode = Convert.ToString(split_staff_code[0]);
                //            }
                //            string getval = d2.GetFunction("select Leavetype from individual_Leave_type where staff_code='" + staffcode + "'");
                //            if (newhash.ContainsKey(staffcode) == true)
                //            {
                //                string befor_m = "";
                //                string befor_e = "";
                //                string after_m = "";
                //                string after_e = "";
                //                DateTime date_time_con1 = d452.AddDays(-1);
                //                string date = date_time_con1.ToString("dd/MM/yyyy");
                //                string[] split_d = date.Split(new Char[] { '/' });
                //                string strdate = split_d[0].ToString();
                //                strdate = strdate.TrimStart('0');
                //                strdate = "[" + strdate + "]";
                //                string Atmonth = split_d[1].ToString();
                //                string Atyear = split_d[2].ToString();
                //                string atmonth1 = (Atmonth.TrimStart('0'));
                //                monyear = atmonth1 + "/" + Atyear;
                //                string selectquery = "";
                //                selectquery = "select " + strdate + " from staff_attnd where staff_code='" + staffcode + "' and mon_year ='" + monyear + "'";
                //                ds55.Clear();
                //                ds55 = d2.select_method_wo_parameter(selectquery, "Text");
                //                if (ds55.Tables[0].Rows.Count > 0)
                //                {
                //                    string attendace_value = ds55.Tables[0].Rows[0][0].ToString();
                //                    if (attendace_value.Trim() != "")
                //                    {
                //                        string[] split_attendance_value = attendace_value.Split('-');
                //                        if (split_attendance_value.Length > 0)
                //                        {
                //                            befor_m = Convert.ToString(split_attendance_value[0]);
                //                            befor_e = Convert.ToString(split_attendance_value[1]);
                //                        }
                //                    }
                //                }
                //                string date11 = senddate.ToString("dd/MM/yyyy");
                //                string[] split_d1 = date11.Split(new Char[] { '/' });
                //                string strdate1 = split_d1[0].ToString();
                //                strdate1 = strdate1.TrimStart('0');
                //                strdate1 = "[" + strdate1 + "]";
                //                string Atmonth1 = split_d1[1].ToString();
                //                string Atyear1 = split_d1[2].ToString();
                //                string atmonth11 = (Atmonth1.TrimStart('0'));
                //                monyear = atmonth11 + "/" + Atyear1;
                //                string selectquery1 = "";
                //                selectquery1 = "select " + strdate1 + " from staff_attnd where staff_code='" + staffcode + "' and mon_year ='" + monyear + "'";
                //                ds55.Clear();
                //                ds55 = d2.select_method_wo_parameter(selectquery1, "Text");
                //                if (ds55.Tables[0].Rows.Count > 0)
                //                {
                //                    string attendace_value = ds55.Tables[0].Rows[0][0].ToString();
                //                    if (attendace_value.Trim() != "")
                //                    {
                //                        string[] split_attendance_value = attendace_value.Split('-');
                //                        if (split_attendance_value.Length > 0)
                //                        {
                //                            after_m = Convert.ToString(split_attendance_value[0]);
                //                            after_e = Convert.ToString(split_attendance_value[1]);
                //                        }
                //                    }
                //                }
                //                if (befor_e.Trim() != "" && befor_m.Trim() != "" || after_m.Trim() != "" && after_e.Trim() != "")
                //                {
                //                    if (befor_m.ToString() == after_m.ToString() && befor_e.ToString() == after_e.ToString())
                //                    {
                //                        if (add_leave_array.Contains(befor_m) == true && add_leave_array.Contains(befor_e) == true)
                //                        {
                //                            string gettype = dicleave[befor_m].ToString();
                //                            string[] stv = getval.Split('\\');
                //                            for (int suh = 0; suh <= stv.GetUpperBound(0); suh++)
                //                            {
                //                                string[] sphe = stv[suh].Split(';');
                //                                if (sphe[0].Trim().ToLower() == gettype.Trim().ToLower())
                //                                {
                //                                    if (sphe[4] == "1")
                //                                    {
                //                                        d452 = senddate.AddDays(-1);
                //                                        string seconddate = d452.ToString("dd");
                //                                        string month123 = d452.ToString("MM");
                //                                        string year123 = d452.ToString("yyyy");
                //                                        string atmonth1111 = (month123.TrimStart('0'));
                //                                        monyear = atmonth1111 + "/" + year123;
                //                                        seconddate = seconddate.TrimStart('0');
                //                                        seconddate = "[" + seconddate + "]";
                //                                        string join_value = after_m + "-" + after_e;
                //                                        string updatequery = "if not exists (select " + seconddate + " from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monyear + "') insert into staff_attnd (" + seconddate + ",staff_code,mon_year) values ('" + join_value + "','" + staffcode + "','" + monyear + "') else update staff_attnd set " + seconddate + "='" + join_value + "' where staff_code='" + staffcode + "' and mon_year='" + monyear + "'";
                //                                        int result_Value = d2.update_method_wo_parameter(updatequery, "Text");
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion
                //else
                //{
                DateTime d452 = senddate.AddDays(-1);
                string stypevalue = "";
                if (cblstafftype.Items.Count > 0)
                {
                    for (int i = 0; i < cblstafftype.Items.Count; i++)
                    {
                        if (cblstafftype.Items[i].Selected == true)
                        {
                            string value = cblstafftype.Items[i].Value;
                            if (stypevalue == "")
                            {
                                stypevalue = value;
                            }
                            else
                            {
                                stypevalue = stypevalue + "'" + "," + "'" + value;
                            }
                        }
                    }
                }
                string catevalue = "";
                if (cbl_Category.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_Category.Items.Count; i++)
                    {
                        if (cbl_Category.Items[i].Selected == true)
                        {
                            string value = cbl_Category.Items[i].Value;
                            if (catevalue == "")
                            {
                                catevalue = value;
                            }
                            else
                            {
                                catevalue = catevalue + "'" + "," + "'" + value;
                            }
                        }
                    }
                }





                string deptvalue = "";
                if (cbl_Department.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_Department.Items.Count; i++)
                    {
                        if (cbl_Department.Items[i].Selected == true)
                        {
                            string value = cbl_Department.Items[i].Value;
                            if (deptvalue == "")
                            {
                                deptvalue = value;
                            }
                            else
                            {
                                deptvalue = deptvalue + "'" + "," + "'" + value;
                            }
                        }
                    }
                }
                DataSet holday = new DataSet();
                //string linkvaluequery = d2.GetFunction("select Linkvalue from InsSettings where LinkName like 'Staff Holiday By Staff Type' and college_code='" + ddlcollege.SelectedItem.Value + "'");
                string linkvaluequery = d2.GetFunction("select value from Master_Settings where settings='HR_PanelSettings' and usercode='" + Convert.ToString(Session["usercode"]) + "'");
                string holidayquery = "";
                if (linkvaluequery.Trim().Contains('3'))
                {
                    holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + d452 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stypevalue + "') and dept_code in('" + deptvalue + "')";
                }
                else
                {
                    //    holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + d452 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + catevalue + "') and t.staff_code ='" + staffcode + "'";

                    holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + d452 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + catevalue + "') and t.staff_code ='" + staffcode + "' and h.dept_code in('" + deptvalue + "') and t.dept_code=h.dept_code and latestrec='1'";


                }
                holday.Clear();
                holday = d2.select_method_wo_parameter(holidayquery, "Text");
                if (holday.Tables[0].Rows.Count > 0)
                {
                    string holiday_value = Convert.ToString(holday.Tables[0].Rows[0]["halforfull"]);
                    if (holiday_value == "False")
                    {
                        for (int k = 1; k < FpSpread2.Sheets[0].RowCount - 5; k += 2)
                        {
                            if (FpSpread2.Sheets[0].Rows[k].Visible == true)
                            {
                                string[] split_staff_code = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(k), 0].Tag).Split('-');
                                if (split_staff_code.Length > 0)
                                {
                                    staffcode = Convert.ToString(split_staff_code[0]);
                                }
                                bool flagetest = false;
                                ArrayList addholiday_value = new ArrayList();
                                if (newhash.ContainsKey(staffcode) == true)
                                {
                                    string date_splitvalue = Convert.ToString(d452);
                                    string[] date_value1 = date_splitvalue.Split('/');
                                    string befor_m = "";
                                    string befor_e = "";
                                    string after_m = "";
                                    string after_e = "";
                                    DateTime d123 = Convert.ToDateTime(date_value1[0].ToString() + "/" + date_value1[1].ToString() + "/" + date_value1[2].ToString());
                                    DateTime dcheck;
                                    d123 = d123.AddDays(-1);
                                    dcheck = d123;
                                    while (flagetest == false)
                                    {
                                        if (linkvaluequery.Trim().Contains('3'))
                                        {
                                            //linkvaluequery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stypevalue + "') ";

                                            linkvaluequery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stypevalue + "')  and dept_code in('" + deptvalue + "') ";
                                        }
                                        else
                                        {
                                            //holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + catevalue + "') and t.staff_code ='" + staffcode + "'";

                                            holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + catevalue + "') and t.staff_code ='" + staffcode + "' and h.dept_code in('" + deptvalue + "') and t.dept_code=h.dept_code and latestrec='1'";
                                        }
                                        holday.Clear();
                                        holday = d2.select_method_wo_parameter(holidayquery, "Text");
                                        if (holday.Tables[0].Rows.Count > 0)
                                        {
                                            string holiday_value1 = Convert.ToString(holday.Tables[0].Rows[0]["halforfull"]);
                                            if (holiday_value1 == "False")
                                            {
                                                addholiday_value.Add(dcheck.ToString("MM/dd/yyyy"));
                                                dcheck = dcheck.AddDays(-1);
                                            }
                                            else
                                            {
                                                flagetest = true;
                                            }
                                        }
                                        else
                                        {
                                            flagetest = true;
                                        }
                                    }
                                    string firstdate = dcheck.ToString("dd");
                                    firstdate = firstdate.TrimStart('0');
                                    firstdate = "[" + firstdate + "]";
                                    string selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staffcode + "' and mon_year ='" + monyear + "'";
                                    holday.Clear();
                                    holday = d2.select_method_wo_parameter(selectquery, "Text");
                                    if (holday.Tables[0].Rows.Count > 0)
                                    {
                                        string attendace_value = holday.Tables[0].Rows[0][0].ToString();
                                        if (attendace_value.Trim() != "")
                                        {
                                            string[] split_attendance_value = attendace_value.Split('-');
                                            if (split_attendance_value.Length > 0)
                                            {
                                                befor_m = Convert.ToString(split_attendance_value[0]);
                                                befor_e = Convert.ToString(split_attendance_value[1]);
                                            }
                                        }
                                    }
                                    d123 = d123.AddDays(2);
                                    firstdate = d123.ToString("dd");
                                    firstdate = firstdate.TrimStart('0');
                                    firstdate = "[" + firstdate + "]";
                                    selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staffcode + "' and mon_year ='" + monyear + "'";
                                    holday.Clear();
                                    holday = d2.select_method_wo_parameter(selectquery, "Text");
                                    if (holday.Tables[0].Rows.Count > 0)
                                    {
                                        string attendace_value = holday.Tables[0].Rows[0][0].ToString();
                                        if (attendace_value.Trim() != "")
                                        {
                                            string[] split_attendance_value = attendace_value.Split('-');
                                            if (split_attendance_value.Length > 0)
                                            {
                                                after_m = Convert.ToString(split_attendance_value[0]);
                                                after_e = Convert.ToString(split_attendance_value[1]);
                                            }
                                        }
                                    }
                                    if (befor_e.Trim() != "" && befor_m.Trim() != "" || after_m.Trim() != "" && after_e.Trim() != "")
                                    {
                                        if (befor_m.ToString() == after_m.ToString() && befor_e.ToString() == after_e.ToString())
                                        {
                                            if (add_leave_array.Contains(befor_m) == true && add_leave_array.Contains(befor_e) == true)
                                            {
                                                d123 = senddate.AddDays(-1);
                                                string seconddate = d123.ToString("dd");
                                                string month123 = d123.ToString("MM");
                                                string year123 = d123.ToString("yyyy");
                                                string atmonth1111 = (month123.TrimStart('0'));
                                                monyear = atmonth1111 + "/" + year123;
                                                seconddate = seconddate.TrimStart('0');
                                                seconddate = "[" + seconddate + "]";
                                                string join_value = after_m + "-" + after_e;
                                                string updatequery = "if not exists (select " + seconddate + " from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monyear + "') insert into staff_attnd (" + seconddate + ",staff_code,mon_year) values ('" + join_value + "','" + staffcode + "','" + monyear + "') else update staff_attnd set " + seconddate + "='" + join_value + "' where staff_code='" + staffcode + "' and mon_year='" + monyear + "'";
                                                int result_Value = d2.update_method_wo_parameter(updatequery, "Text");
                                                if (addholiday_value.Count > 0)
                                                {
                                                    for (int h = 0; h < addholiday_value.Count; h++)
                                                    {
                                                        string datecheck = Convert.ToString(addholiday_value[h]);
                                                        string[] splitch = datecheck.Split('/');
                                                        seconddate = Convert.ToString(splitch[1]);
                                                        month123 = Convert.ToString(splitch[0]);
                                                        year123 = Convert.ToString(splitch[2]);
                                                        atmonth1111 = (month123.TrimStart('0'));
                                                        monyear = atmonth1111 + "/" + year123;
                                                        seconddate = seconddate.TrimStart('0');
                                                        seconddate = "[" + seconddate + "]";
                                                        join_value = after_m + "-" + after_e;
                                                        updatequery = "if not exists (select " + seconddate + " from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monyear + "') insert into staff_attnd (" + seconddate + ",staff_code,mon_year) values ('" + join_value + "','" + staffcode + "','" + monyear + "') else update staff_attnd set " + seconddate + "='" + join_value + "' where staff_code='" + staffcode + "' and mon_year='" + monyear + "'";
                                                        result_Value = d2.update_method_wo_parameter(updatequery, "Text");
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
            //}
            //}
            //for (int k = 1; k < FpSpread2.Sheets[0].RowCount - 5; k++)
            //{
            //    if (FpSpread2.Sheets[0].Rows[k].Visible == true)
            //    {
            //        string[] split_staff_code = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(k), 0].Tag).Split('-');
            //        if (split_staff_code.Length > 0)
            //            staffcode = Convert.ToString(split_staff_code[0]);
            //        if (arrHolDays.Count > 0)
            //        {
            //            DateTime dtHol = new DateTime();
            //            for (int SplArr = 0; SplArr < arrHolDays.Count; SplArr++)
            //            {
            //                DateTime.TryParse(Convert.ToString(arrHolDays[SplArr]), out dtHol);
            //                if (dtHol.ToString("dd/MM/yyyy") != "01/01/1900")
            //                {
            //                    string myHol = "[" + Convert.ToString(dtHol.Day) + "]";
            //                    string HolUpdQ = "if not exists (select " + myHol + " from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + Convert.ToString(dtHol.Month) + "/" + Convert.ToString(dtHol.Year) + "') insert into staff_attnd (" + myHol + ",staff_code,mon_year) values ('H-H','" + staffcode + "','" + Convert.ToString(dtHol.Month) + "/" + Convert.ToString(dtHol.Year) + "') else update staff_attnd set " + myHol + "='H-H' where staff_code='" + staffcode + "' and mon_year='" + Convert.ToString(dtHol.Month) + "/" + Convert.ToString(dtHol.Year) + "'";
            //                    int UpdC = d2.update_method_wo_parameter(HolUpdQ, "Text");
            //                }
            //            }
            //        }
            //    }
            //}
            if (flag_updatesave == 1)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                return;
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.ToString();
            lblError.Visible = true;
            d2.sendErrorMail(ex, Convert.ToString(ddlcollege.SelectedItem.Value), "Staff_Attendance1.aspx");
        }
    }
    public void bindcell(string ar, string md)
    {
        //FpSpread2.SaveChanges();
        //bindapplydate(ar, md,);
        //FpSpread2.SaveChanges();
    }
    public void bindapplydate(string act, string mode, string type)//delsisref
    {
        try
        {
            leave_apply_flage = true;
            ArrayList arrayreson = new ArrayList();
            ArrayList addleavetype = new ArrayList();
            ArrayList hrpaymonth = new ArrayList();
            Hashtable hr_pay_date = new Hashtable();
            string daycount = "";
            string monyear = "";
            string staffcode = "";
            leave_apply = 0;
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
                    arrayreson.Add(sdate.ToString("dd/MM/yyyy"));
                }
                sdate = sdate.AddDays(1);
            }
            string individualquery = "";
            individualquery = "select * from individual_Leave_type where college_code=" + ddlcollege.SelectedItem.Value + "";
            individualquery = individualquery + " select * from HrPayMonths where College_Code=" + ddlcollege.SelectedItem.Value + "";
            individualquery = individualquery + " select * from leave_category where college_code=" + ddlcollege.SelectedItem.Value + "";
            individualquery = individualquery + " select * from staff_attnd";
            individualquery = individualquery + " select * from holidayStaff";
            individualquery = individualquery + " select * from InsSettings where LinkName like 'Staff Holiday By Staff Type' and college_code='" + ddlcollege.SelectedItem.Value + "'";
            individualquery = individualquery + " select halforfull ,morning,evening,ltype,stftype,dept_code,holiday_desc from holidayStaff";
            ds.Clear();
            ds = d2.select_method_wo_parameter(individualquery, "Text");
            if (ds.Tables[2].Rows.Count > 0)
            {
                for (int p = 0; p < ds.Tables[2].Rows.Count; p++)
                {
                    addleavetype.Add(ds.Tables[2].Rows[p]["shortname"].ToString());
                }
            }
            if (!addleavetype.Contains("P"))
                addleavetype.Add("P");
            if (!addleavetype.Contains("A"))
                addleavetype.Add("A");
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int h = 0; h < ds.Tables[1].Rows.Count; h++)
                {
                    string hr_paysetting = ds.Tables[1].Rows[h]["PayMonthNum"].ToString();
                    string hr_paydate = ds.Tables[1].Rows[h]["PayMonth"].ToString() + "-" + ds.Tables[1].Rows[h]["From_Date"].ToString() + "-" + ds.Tables[1].Rows[h]["To_Date"].ToString();
                    hrpaymonth.Add(hr_paysetting);
                    hr_pay_date.Add(hr_paysetting, hr_paydate);
                }
            }
            string stypevalue = "";
            if (cblstafftype.Items.Count > 0)
            {
                for (int i = 0; i < cblstafftype.Items.Count; i++)
                {
                    if (cblstafftype.Items[i].Selected == true)
                    {
                        string value = cblstafftype.Items[i].Value;
                        if (stypevalue == "")
                        {
                            stypevalue = value;
                        }
                        else
                        {
                            stypevalue = stypevalue + "'" + "," + "'" + value;
                        }
                    }
                }
            }
            string catevalue = "";
            if (cbl_Category.Items.Count > 0)
            {
                for (int i = 0; i < cbl_Category.Items.Count; i++)
                {
                    if (cbl_Category.Items[i].Selected == true)
                    {
                        string value = cbl_Category.Items[i].Value;
                        if (catevalue == "")
                        {
                            catevalue = value;
                        }
                        else
                        {
                            catevalue = catevalue + "'" + "," + "'" + value;
                        }
                    }
                }
            }

            string deptvalue = "";
            if (cbl_Department.Items.Count > 0)
            {
                for (int i = 0; i < cbl_Department.Items.Count; i++)
                {
                    if (cbl_Department.Items[i].Selected == true)
                    {
                        string value = cbl_Department.Items[i].Value;
                        if (deptvalue == "")
                        {
                            deptvalue = value;
                        }
                        else
                        {
                            deptvalue = deptvalue + "'" + "," + "'" + value;
                        }
                    }
                }
            }

            string mattndvalue = "";
            string eattndvalue = "";
            int countvalue = Convert.ToInt32(Session["item_Value"]);
            countvalue = countvalue + 3;
            ArrayList arr_list = new ArrayList();
            ArrayList arr_list_count = new ArrayList();
            ArrayList arr_add_attendance = new ArrayList();
            DataSet dsvalue = new DataSet();
            Hashtable hasdate = new Hashtable();
            hasdate.Clear();
            double add_leave_count = 0;
            double add_leave_count1 = 0;
            string atmonth1 = "";
            string Atyear = "";
            string Atmonth = "";
            string attendance = "";
            string[] split_staff_code = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), 0].Tag).Split('-');
            if (split_staff_code.Length > 0)
            {
                staffcode = Convert.ToString(split_staff_code[0]);
                attendance = Convert.ToString(split_staff_code[1]);
            }
            int s = 0;
            arr_list.Clear();
            arr_list_count.Clear();
            Hashtable addmonthcount = new Hashtable();
            ArrayList arr_list_count_value = new ArrayList();
            ArrayList arr = new ArrayList();
            for (int i = countvalue; i < FpSpread2.Sheets[0].ColumnCount; i += 2)
            {
                string date = arrayreson[s].ToString();
                string[] split_d = date.Split(new Char[] { '/' });
                string strdate = split_d[0].ToString();
                Atmonth = split_d[1].ToString();
                Atyear = split_d[2].ToString();
                atmonth1 = (Atmonth.TrimStart('0'));
                monyear = atmonth1 + "/" + Atyear;
                if (mode == "Half")
                {
                    if (attendance.Trim() == "M")
                    {
                        mattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text);
                        eattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i].Text);
                    }
                    else
                    {
                        mattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i].Text);
                        eattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text);
                    }
                }
                else
                {
                    mattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text);
                    eattndvalue = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i].Text);
                }
                strdate = strdate.TrimStart('0');
                strdate = "[" + strdate + "]";
                string query = "select " + strdate + " from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monyear + "'";
                dsvalue.Clear();
                dsvalue = d2.select_method_wo_parameter(query, "Text");
                string first_attnd_value = "";
                string second_attnd_value = "";
                if (dsvalue.Tables[0].Rows.Count > 0)
                {
                    string value = Convert.ToString(dsvalue.Tables[0].Rows[0][0]);
                    if (value.Trim() != "" && value.Trim() != "-")
                    {
                        string[] splitattnd = value.Split('-');
                        if (splitattnd.Length > 0)
                        {
                            first_attnd_value = splitattnd[0].ToString();
                            second_attnd_value = splitattnd[1].ToString();
                        }
                    }
                }
                hasdate.Add(date, mattndvalue + "-" + eattndvalue);
                if (addleavetype.Contains(mattndvalue) == true)
                {
                    if (!arr_list_count_value.Contains(atmonth1))
                    {
                        if (arr_list_count_value.Count > 0)
                        {
                            arr.Clear();
                        }
                        arr_list_count_value.Add(atmonth1);
                    }
                    if (arr_list.Contains(mattndvalue) == false)
                    {
                        arr_list.Add(mattndvalue);
                    }
                    //if (first_attnd_value.Trim() != mattndvalue.Trim())
                    //{
                    arr_list_count.Add(mattndvalue);
                    arr.Add(mattndvalue);
                    //}
                }
                if (addleavetype.Contains(eattndvalue) == true)
                {
                    if (!arr_list_count_value.Contains(atmonth1))
                    {
                        if (arr_list_count_value.Count > 0)
                        {
                            arr.Clear();
                        }
                        arr_list_count_value.Add(atmonth1);
                    }
                    if (arr_list.Contains(eattndvalue) == false)
                    {
                        arr_list.Add(eattndvalue);
                    }
                    //if (second_attnd_value.Trim() != eattndvalue.Trim())
                    //{
                    arr_list_count.Add(eattndvalue);
                    arr.Add(eattndvalue);
                    // }
                }
                s++;
            }
            if (arr_list.Count > 0)
            {
                for (int ar = 0; ar < arr_list.Count; ar++)
                {
                    if (arr_list_count.Contains(Convert.ToString(arr_list[ar])))
                    {
                        for (int j = 0; j < arr_list_count.Count; j++)
                        {
                            if (arr_list_count[j].ToString() == arr_list[ar].ToString())
                            {
                                add_leave_count = 0.5;
                            }
                        }
                        for (int j = 0; j < arr.Count; j++)
                        {
                            if (arr[j].ToString() == arr_list[ar].ToString())
                            {
                                add_leave_count1 = 0.5;
                            }
                        }
                        DataView dv = new DataView();
                        ds.Tables[2].DefaultView.RowFilter = "shortname ='" + arr_list[ar].ToString() + "'";
                        dv = ds.Tables[2].DefaultView;
                        if (dv.Count > 0)
                        {
                            for (int ik = 0; ik < dv.Count; ik++)
                            {
                                string leavecategory = dv[ik]["category"].ToString();
                                DataView dv1 = new DataView();
                                ds.Tables[0].DefaultView.RowFilter = "staff_code ='" + staffcode + "'";
                                dv1 = ds.Tables[0].DefaultView;
                                if (dv1.Count > 0)
                                {
                                    string leave_type_category = dv1[0]["leavetype"].ToString();
                                    string[] splitleavecategory = leave_type_category.Split('\\');
                                    if (splitleavecategory.Length > 0)
                                    {
                                        for (int upper = 0; upper < splitleavecategory.GetUpperBound(0); upper++)
                                        {
                                            string[] secondsplitleave = splitleavecategory[upper].Split(';');
                                            if (secondsplitleave.Length > 0)
                                            {
                                                arr_add_attendance.Add(secondsplitleave[0].ToString());
                                                if (secondsplitleave.Contains(leavecategory) == true)
                                                {
                                                    string leave_type = secondsplitleave[0].ToString();
                                                    string yearlyleave = secondsplitleave[1].ToString();
                                                    string monthlyleave = secondsplitleave[2].ToString();
                                                    string sundayholiday = secondsplitleave[4].ToString();
                                                    string holidaycary = secondsplitleave[5].ToString();
                                                    string monthlycary = secondsplitleave[6].ToString();
                                                    string sflage = "";
                                                    double count_check_attend = 0;
                                                    int monthcount_total = 0;
                                                    string date = txttodate.Text.ToString();
                                                    Boolean monthlyyear = false;
                                                    Boolean monthlyyear1 = false;
                                                    Boolean year = false;
                                                    Boolean year1 = false;
                                                    Boolean monthandmonth = false;
                                                    Boolean month = false;
                                                    DataView finalview = new DataView();
                                                    DataView checkview = new DataView();
                                                    if (yearlyleave != "0" && yearlyleave != "")
                                                    {
                                                        if (monthlycary != "0" && monthlycary != "")
                                                        {
                                                            if (monthlyleave != "0" && monthlyleave != "")
                                                            {
                                                                if (sundayholiday != "0" && sundayholiday != "" && holidaycary != "0" && holidaycary != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*1");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*1");
                                                                    }
                                                                    sflage = "2";
                                                                    monthlyyear = true;
                                                                }
                                                                else if (sundayholiday != "0" && sundayholiday != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*0");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*0");
                                                                    }
                                                                    sflage = "0";
                                                                    monthlyyear = true;
                                                                }
                                                                else if (holidaycary != "0" && holidaycary != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*2");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*2");
                                                                    }
                                                                    sflage = "1";
                                                                    monthlyyear = true;
                                                                }
                                                                else
                                                                {
                                                                    year = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (sundayholiday != "0" && sundayholiday != "" && holidaycary != "0" && holidaycary != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*1");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*1");
                                                                    }
                                                                    sflage = "2";
                                                                    monthlyyear1 = true;
                                                                }
                                                                else if (sundayholiday != "0" && sundayholiday != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*0");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*0");
                                                                    }
                                                                    sflage = "0";
                                                                    monthlyyear1 = true;
                                                                }
                                                                else if (holidaycary != "0" && holidaycary != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*2");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*2");
                                                                    }
                                                                    sflage = "1";
                                                                    monthlyyear1 = true;
                                                                }
                                                                else
                                                                {
                                                                    year1 = true;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (monthlyleave != "0" && monthlyleave != "")
                                                            {
                                                                if (sundayholiday != "0" && sundayholiday != "" && holidaycary != "0" && holidaycary != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*1");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*1");
                                                                    }
                                                                    sflage = "2";
                                                                    monthandmonth = true;
                                                                }
                                                                else if (sundayholiday != "0" && sundayholiday != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*0");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*0");
                                                                    }
                                                                    sflage = "0";
                                                                    monthandmonth = true;
                                                                }
                                                                else if (holidaycary != "0" && holidaycary != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*2");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*2");
                                                                    }
                                                                    sflage = "1";
                                                                    monthandmonth = true;
                                                                }
                                                                else
                                                                {
                                                                    month = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // same as monthly carry
                                                                if (sundayholiday != "0" && sundayholiday != "" && holidaycary != "0" && holidaycary != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*1");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*1");
                                                                    }
                                                                    sflage = "2";
                                                                    monthlyyear = true;
                                                                }
                                                                else if (sundayholiday != "0" && sundayholiday != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*0");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*0");
                                                                    }
                                                                    sflage = "0";
                                                                    monthlyyear = true;
                                                                }
                                                                else if (holidaycary != "0" && holidaycary != "")
                                                                {
                                                                    if (newhash.ContainsKey(staffcode) == true)
                                                                    {
                                                                        newhash.Remove(staffcode);
                                                                        newhash.Add(staffcode, "1*2");
                                                                    }
                                                                    else
                                                                    {
                                                                        newhash.Add(staffcode, "1*2");
                                                                    }
                                                                    sflage = "1";
                                                                    monthlyyear = true;
                                                                }
                                                                else
                                                                {
                                                                    year1 = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    Double monsetleaave = 0;
                                                    string leavshort = "";
                                                    Hashtable hatstafdate = new Hashtable();
                                                    ds.Tables[2].DefaultView.RowFilter = "category='" + leave_type + "'";
                                                    checkview = ds.Tables[2].DefaultView;
                                                    if (checkview.Count > 0)
                                                    {
                                                        leavshort = Convert.ToString(checkview[0]["shortname"]);
                                                    }
                                                    Double monleav = Convert.ToDouble(monthlyleave);
                                                    if (monthlyleave != "0" && monthlyleave != "")
                                                    {
                                                        if (monthlycary == "1")
                                                        {
                                                            monleav = Convert.ToDouble(monthlyleave) * Convert.ToDouble(d2.GetFunction("select count(*) from HrPayMonths where College_Code='" + ddlcollege.SelectedItem.Value + "' and From_Date<='" + dateto + "'"));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        monleav = Convert.ToDouble(yearlyleave);
                                                    }
                                                    for (int i = countvalue; i < FpSpread2.Sheets[0].ColumnCount; i += 2)
                                                    {
                                                        string[] spd = FpSpread2.Sheets[0].ColumnHeader.Cells[0, i].Text.ToString().Split('/');
                                                        if (spd.GetUpperBound(0) == 2)
                                                        {
                                                            string getvadate = spd[1] + '/' + spd[0] + '/' + spd[2];
                                                            if (!hatstafdate.Contains(getvadate))
                                                            {
                                                                string fhv = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text);
                                                                string shv = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i].Text);
                                                                if (attendance.Trim() != "M")
                                                                {
                                                                    fhv = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i].Text);
                                                                    shv = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text);
                                                                }
                                                                Double dicle = 0;
                                                                if (leavshort.Trim().ToLower() == fhv.Trim().ToLower())
                                                                {
                                                                    dicle = 0.5;
                                                                }
                                                                if (leavshort.Trim().ToLower() == shv.Trim().ToLower())
                                                                {
                                                                    dicle = dicle + 0.5;
                                                                }
                                                                if (dicle > 0)
                                                                {
                                                                    hatstafdate.Add(getvadate, dicle);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    string strmonthquery = "select * from hrpaymonths where college_code='" + ddlcollege.SelectedItem.Value + "' and ('" + datefrom + "' between From_Date and To_Date or '" + dateto + "' between From_Date and To_Date )";
                                                    if (monthlycary == "1")
                                                    {
                                                        strmonthquery = "select * from hrpaymonths where college_code='" + ddlcollege.SelectedItem.Value + "' and From_Date<='" + dateto + "'";
                                                    }
                                                    if (monthlyleave == "0" || monthlyleave == "")
                                                    {
                                                        strmonthquery = "select * from hrpaymonths where college_code='" + ddlcollege.SelectedItem.Value + "' ";
                                                    }
                                                    DataSet dsmonrow = d2.select_method_wo_parameter(strmonthquery, "text");
                                                    Double yearwiseleve = 0;
                                                    for (int mn = 0; mn < dsmonrow.Tables[0].Rows.Count; mn++)//Monthly Loop Start
                                                    {
                                                        Double monwiseleave = 0;
                                                        int sunval = 0;
                                                        string monthfro = dsmonrow.Tables[0].Rows[mn]["From_Date"].ToString();
                                                        string monthtoda = dsmonrow.Tables[0].Rows[mn]["To_Date"].ToString();
                                                        for (DateTime dtnow = Convert.ToDateTime(monthfro); dtnow <= Convert.ToDateTime(monthtoda); dtnow = dtnow.AddDays(1))
                                                        {
                                                            string getdate = dtnow.ToString("MM/dd/yyyy");
                                                            Double daywiseleav = 0;
                                                            if (hatstafdate.Contains(getdate))
                                                            {
                                                                monsetleaave = monsetleaave + Convert.ToDouble(hatstafdate[getdate]);
                                                                monwiseleave = monwiseleave + Convert.ToDouble(hatstafdate[getdate]);
                                                                yearwiseleve = yearwiseleve + Convert.ToDouble(hatstafdate[getdate]);
                                                                daywiseleav = Convert.ToDouble(hatstafdate[getdate]);
                                                            }
                                                            else
                                                            {
                                                                string getday = dtnow.Day.ToString();
                                                                string getmonye = dtnow.ToString("M/yyyy");
                                                                ds.Tables[3].DefaultView.RowFilter = "staff_code='" + staffcode + "'and mon_year ='" + getmonye + "'";
                                                                DataView dvstaffatt = ds.Tables[3].DefaultView;
                                                                if (dvstaffatt.Count > 0)
                                                                {
                                                                    string[] spat = dvstaffatt[0][getday].ToString().Split('-');
                                                                    if (spat.GetUpperBound(0) == 1)
                                                                    {
                                                                        if (leavshort.Trim().ToLower() == spat[0].Trim().ToLower())
                                                                        {
                                                                            monsetleaave = monsetleaave + 0.5;
                                                                            monwiseleave = monwiseleave + 0.5;
                                                                            yearwiseleve = yearwiseleve + 0.5;
                                                                            daywiseleav = daywiseleav + 0.5;
                                                                        }
                                                                        if (leavshort.Trim().ToLower() == spat[1].Trim().ToLower())
                                                                        {
                                                                            monsetleaave = monsetleaave + 0.5;
                                                                            monwiseleave = monwiseleave + 0.5;
                                                                            yearwiseleve = yearwiseleve + 0.5;
                                                                            daywiseleav = daywiseleav + 0.5;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (sundayholiday == "1")
                                                            {
                                                                string dayofweek = dtnow.DayOfWeek.ToString();
                                                                if (dayofweek.ToString().Trim().ToLower() == "saturday" || dayofweek.ToString().Trim().ToLower() == "monday")
                                                                {
                                                                    if (daywiseleav == 1)
                                                                    {
                                                                        sunval = sunval + 1;
                                                                        if (dayofweek.ToString().Trim().ToLower() == "monday")
                                                                        {
                                                                            if (sunval == 2)
                                                                            {
                                                                                monsetleaave = monsetleaave + 1;
                                                                                monwiseleave = monwiseleave + 1;
                                                                                yearwiseleve = yearwiseleve + 1;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else if (dayofweek.ToString().Trim().ToLower() != "sunday")
                                                                {
                                                                    sunval = 0;
                                                                }
                                                            }
                                                        }
                                                        //With Out Monthly Carry Over and Having Monthly Leave
                                                        if (monthlyleave != "0" && monthlyleave != "")
                                                        {
                                                            if (monthlycary != "1")
                                                            {
                                                                if (monleav < monwiseleave)
                                                                {
                                                                    leave_apply++;
                                                                }
                                                            }
                                                        }
                                                    }//Monthly Loop End
                                                    //With Monthly Carry Over and Having Monthly Leave
                                                    if (monthlyleave != "0" && monthlyleave != "")
                                                    {
                                                        if (monthlycary == "1")
                                                        {
                                                            if (monleav < monsetleaave)
                                                            {
                                                                leave_apply++;
                                                            }
                                                        }
                                                    }
                                                    //Yearly Leave Based
                                                    //if (monthlyleave == "0" || monthlyleave == "")
                                                    //{
                                                    if (monleav < yearwiseleve)
                                                    {
                                                        leave_apply++;
                                                    }
                                                    // }
                                                    if (monthandmonth == true)
                                                    {
                                                        updateattendace(date, sflage, staffcode, monyear, mattndvalue, eattndvalue, catevalue, stypevalue, hasdate, act, type, deptvalue);
                                                        int monthlycount = 0;
                                                        if (hrpaymonth.Count > 0)
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "PayMonthNum=" + atmonth1 + "";
                                                            finalview = ds.Tables[1].DefaultView;
                                                            if (finalview.Count > 0)
                                                            {
                                                                string hr_month_Value = finalview[0]["PayMonthNum"].ToString();
                                                                int indexmonth = hrpaymonth.IndexOf(hr_month_Value);
                                                                for (int hr = 0; hr <= indexmonth; hr++)
                                                                {
                                                                    string hr_month1 = Convert.ToString(hrpaymonth[hr]);
                                                                    string hr_month_year = hr_month1 + "/" + Atyear;
                                                                    count_check_attend = 0;
                                                                    ds.Tables[3].DefaultView.RowFilter = "staff_code='" + staffcode + "'and mon_year ='" + hr_month_year + "'";
                                                                    finalview = ds.Tables[3].DefaultView;
                                                                    if (finalview.Count > 0)
                                                                    {
                                                                        for (int check = 4; check < finalview.Table.Columns.Count - 3; check++)
                                                                        {
                                                                            string attend_hr_value = Convert.ToString(finalview[0][check].ToString());
                                                                            if (attend_hr_value.Trim() != "" && attend_hr_value != null && attend_hr_value.Trim() != "-")
                                                                            {
                                                                                string[] split_hr_value_attend = attend_hr_value.Split('-');
                                                                                string first_split_attend = Convert.ToString(split_hr_value_attend[0]);
                                                                                string second_split_attend = Convert.ToString(split_hr_value_attend[1]);
                                                                                ds.Tables[2].DefaultView.RowFilter = "category='" + leave_type + "'";
                                                                                checkview = ds.Tables[2].DefaultView;
                                                                                if (checkview.Count > 0)
                                                                                {
                                                                                    string check_attend_value = Convert.ToString(checkview[0]["shortname"]);
                                                                                    if (check_attend_value == first_split_attend)
                                                                                    {
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                    if (check_attend_value == second_split_attend)
                                                                                    {
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    //  monthlycount = monthlycount + Convert.ToInt32(monthlyleave);
                                                                }
                                                            }
                                                        }
                                                        if (sunday_flage_add != 0)
                                                        {
                                                            add_leave_count = add_leave_count + sunday_flage_add;
                                                        }
                                                        if (holiday_flage_add != 0)
                                                        {
                                                            add_leave_count = holiday_flage_add + add_leave_count;
                                                        }
                                                        if (Convert.ToDouble(monthlyleave) < count_check_attend + add_leave_count)
                                                        {
                                                            leave_apply++;
                                                        }
                                                        if (leave_apply == 0 && holiday_flage_add != 0)
                                                        {
                                                            if (FpSpread2.Sheets[0].Rows.Count > 0)
                                                            {
                                                                for (int i = Convert.ToInt32(Session["item_Value"]); i < FpSpread2.Sheets[0].Columns.Count; i++)
                                                                {
                                                                    string tag_vlue = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                                                                    if (holidaydate.Contains(tag_vlue) == true)
                                                                    {
                                                                        if (type == "M")
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text = mattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i + 1].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i].Text = eattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i + 1].Locked = false;
                                                                        }
                                                                        if (type == "E")
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text = mattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i + 1].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i].Text = eattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i - 1].Locked = false;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (monthlyyear == true)
                                                    {
                                                        double month_count = 0;
                                                        updateattendace(date, sflage, staffcode, monyear, mattndvalue, eattndvalue, catevalue, stypevalue, hasdate, act, type, deptvalue);
                                                        if (hrpaymonth.Count > 0)
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "PayMonthNum=" + atmonth1 + "";
                                                            finalview = ds.Tables[1].DefaultView;
                                                            if (finalview.Count > 0)
                                                            {
                                                                string hr_month_Value = finalview[0]["PayMonthNum"].ToString();
                                                                int indexmonth = hrpaymonth.IndexOf(hr_month_Value);
                                                                // bool testflage = false;
                                                                for (int hr = 0; hr <= indexmonth; hr++)
                                                                {
                                                                    month_count = 0;
                                                                    string hr_month1 = Convert.ToString(hrpaymonth[hr]);
                                                                    string hr_month_year = hr_month1 + "/" + Atyear;
                                                                    //   testflage = false;
                                                                    ds.Tables[3].DefaultView.RowFilter = "staff_code='" + staffcode + "'and mon_year ='" + hr_month_year + "'";
                                                                    finalview = ds.Tables[3].DefaultView;
                                                                    if (finalview.Count > 0)
                                                                    {
                                                                        for (int check = 4; check < finalview.Table.Columns.Count - 3; check++)
                                                                        {
                                                                            string attend_hr_value = Convert.ToString(finalview[0][check].ToString());
                                                                            if (attend_hr_value.Trim() != "" && attend_hr_value != null && attend_hr_value.Trim() != "-")
                                                                            {
                                                                                string[] split_hr_value_attend = attend_hr_value.Split('-');
                                                                                string first_split_attend = Convert.ToString(split_hr_value_attend[0]);
                                                                                string second_split_attend = Convert.ToString(split_hr_value_attend[1]);
                                                                                ds.Tables[2].DefaultView.RowFilter = "category='" + leave_type + "'";
                                                                                checkview = ds.Tables[2].DefaultView;
                                                                                if (checkview.Count > 0)
                                                                                {
                                                                                    string check_attend_value = Convert.ToString(checkview[0]["shortname"]);
                                                                                    if (check_attend_value == first_split_attend)
                                                                                    {
                                                                                        // testflage = true;
                                                                                        month_count = month_count + 0.5;
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                    if (check_attend_value == second_split_attend)
                                                                                    {
                                                                                        //  testflage = true;
                                                                                        month_count = month_count + 0.5;
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    monthcount_total = monthcount_total + Convert.ToInt32(monthlyleave);
                                                                }
                                                            }
                                                        }
                                                        if (sunday_flage_add != 0)
                                                        {
                                                            add_leave_count = add_leave_count + sunday_flage_add;
                                                        }
                                                        if (holiday_flage_add != 0)
                                                        {
                                                            add_leave_count = holiday_flage_add + add_leave_count;
                                                        }
                                                        if (Convert.ToDouble(yearlyleave) < count_check_attend + add_leave_count)
                                                        {
                                                            leave_apply++;
                                                        }
                                                        if (monthcount_total < count_check_attend + add_leave_count)
                                                        {
                                                            leave_apply++;
                                                        }
                                                        if (leave_apply == 0 && holiday_flage_add != 0)
                                                        {
                                                            if (FpSpread2.Sheets[0].Rows.Count > 0)
                                                            {
                                                                for (int i = Convert.ToInt32(Session["item_Value"]); i < FpSpread2.Sheets[0].Columns.Count; i++)
                                                                {
                                                                    string tag_vlue = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                                                                    if (holidaydate.Contains(tag_vlue) == true)
                                                                    {
                                                                        if (type == "M")
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text = mattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i + 1].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i].Text = eattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i + 1].Locked = false;
                                                                        }
                                                                        if (type == "E")
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text = mattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i + 1].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i].Text = eattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i - 1].Locked = false;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (monthlyyear1 == true)
                                                    {
                                                        updateattendace(date, sflage, staffcode, monyear, mattndvalue, eattndvalue, catevalue, stypevalue, hasdate, act, type, deptvalue);
                                                        if (hrpaymonth.Count > 0)
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "PayMonthNum=" + atmonth1 + "";
                                                            finalview = ds.Tables[1].DefaultView;
                                                            if (finalview.Count > 0)
                                                            {
                                                                string hr_month_Value = finalview[0]["PayMonthNum"].ToString();
                                                                int indexmonth = hrpaymonth.IndexOf(hr_month_Value);
                                                                //   bool testflage = false;
                                                                for (int hr = 0; hr <= indexmonth; hr++)
                                                                {
                                                                    string hr_month1 = Convert.ToString(hrpaymonth[hr]);
                                                                    string hr_month_year = hr_month1 + "/" + Atyear;
                                                                    //   testflage = false;
                                                                    ds.Tables[3].DefaultView.RowFilter = "staff_code='" + staffcode + "'and mon_year ='" + hr_month_year + "'";
                                                                    finalview = ds.Tables[3].DefaultView;
                                                                    if (finalview.Count > 0)
                                                                    {
                                                                        for (int check = 4; check < finalview.Table.Columns.Count - 3; check++)
                                                                        {
                                                                            string attend_hr_value = Convert.ToString(finalview[0][check].ToString());
                                                                            if (attend_hr_value.Trim() != "" && attend_hr_value != null && attend_hr_value.Trim() != "-")
                                                                            {
                                                                                string[] split_hr_value_attend = attend_hr_value.Split('-');
                                                                                string first_split_attend = Convert.ToString(split_hr_value_attend[0]);
                                                                                string second_split_attend = Convert.ToString(split_hr_value_attend[1]);
                                                                                ds.Tables[2].DefaultView.RowFilter = "category='" + leave_type + "'";
                                                                                checkview = ds.Tables[2].DefaultView;
                                                                                if (checkview.Count > 0)
                                                                                {
                                                                                    string check_attend_value = Convert.ToString(checkview[0]["shortname"]);
                                                                                    if (check_attend_value == first_split_attend)
                                                                                    {
                                                                                        // testflage = true;
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                    if (check_attend_value == second_split_attend)
                                                                                    {
                                                                                        //  testflage = true;
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    // monthcount_total = monthcount_total + Convert.ToInt32(monthlyleave);
                                                                }
                                                            }
                                                        }
                                                        if (sunday_flage_add != 0)
                                                        {
                                                            add_leave_count = add_leave_count + sunday_flage_add;
                                                        }
                                                        if (holiday_flage_add != 0)
                                                        {
                                                            add_leave_count = holiday_flage_add + add_leave_count;
                                                        }
                                                        if (Convert.ToDouble(yearlyleave) < count_check_attend + add_leave_count)
                                                        {
                                                            leave_apply++;
                                                        }
                                                        if (leave_apply == 0 && holiday_flage_add != 0)
                                                        {
                                                            if (FpSpread2.Sheets[0].Rows.Count > 0)
                                                            {
                                                                for (int i = Convert.ToInt32(Session["item_Value"]); i < FpSpread2.Sheets[0].Columns.Count; i++)
                                                                {
                                                                    string tag_vlue = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                                                                    if (holidaydate.Contains(tag_vlue) == true)
                                                                    {
                                                                        if (type == "M")
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text = mattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i + 1].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i].Text = eattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) + 1, i + 1].Locked = false;
                                                                        }
                                                                        if (type == "E")
                                                                        {
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Text = mattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act), i + 1].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i].Text = eattndvalue.ToString();
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i].Locked = false;
                                                                            FpSpread2.Sheets[0].Cells[Convert.ToInt32(act) - 1, i - 1].Locked = false;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (month == true)
                                                    {
                                                        double monthcount = 0;
                                                        if (hrpaymonth.Count > 0)
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "PayMonthNum=" + atmonth1 + "";
                                                            finalview = ds.Tables[1].DefaultView;
                                                            if (finalview.Count > 0)
                                                            {
                                                                string hr_month_Value = finalview[0]["PayMonthNum"].ToString();
                                                                int indexmonth = hrpaymonth.IndexOf(hr_month_Value);
                                                                for (int hr = 0; hr <= indexmonth; hr++)
                                                                {
                                                                    count_check_attend = 0;
                                                                    string hr_month1 = Convert.ToString(hrpaymonth[hr]);
                                                                    string hr_month_year = hr_month1 + "/" + Atyear;
                                                                    ds.Tables[3].DefaultView.RowFilter = "staff_code='" + staffcode + "'and mon_year ='" + hr_month_year + "'";
                                                                    finalview = ds.Tables[3].DefaultView;
                                                                    if (finalview.Count > 0)
                                                                    {
                                                                        for (int check = 4; check < finalview.Table.Columns.Count - 3; check++)
                                                                        {
                                                                            string attend_hr_value = Convert.ToString(finalview[0][check].ToString());
                                                                            if (attend_hr_value.Trim() != "" && attend_hr_value != null && attend_hr_value.Trim() != "-")
                                                                            {
                                                                                string[] split_hr_value_attend = attend_hr_value.Split('-');
                                                                                string first_split_attend = Convert.ToString(split_hr_value_attend[0]);
                                                                                string second_split_attend = Convert.ToString(split_hr_value_attend[1]);
                                                                                ds.Tables[2].DefaultView.RowFilter = "category='" + leave_type + "'";
                                                                                checkview = ds.Tables[2].DefaultView;
                                                                                if (checkview.Count > 0)
                                                                                {
                                                                                    string check_attend_value = Convert.ToString(checkview[0]["shortname"]);
                                                                                    if (check_attend_value == first_split_attend)
                                                                                    {
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                    if (check_attend_value == second_split_attend)
                                                                                    {
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    if (count_check_attend == Convert.ToDouble(monthlyleave))
                                                                    {
                                                                        monthcount = monthcount + 0;
                                                                    }
                                                                    else if (count_check_attend == 0)
                                                                    {
                                                                        monthcount = monthcount + Convert.ToDouble(monthlyleave);
                                                                    }
                                                                    else if (count_check_attend != 0)
                                                                    {
                                                                        monthcount = monthcount + (Convert.ToDouble(monthlyleave) - count_check_attend);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (Convert.ToDouble(monthlyleave) < count_check_attend + add_leave_count1)
                                                        {
                                                            leave_apply++;
                                                        }
                                                    }
                                                    else if (year == true)
                                                    {
                                                        double month_count = 0;
                                                        if (hrpaymonth.Count > 0)
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "PayMonthNum=" + atmonth1 + "";
                                                            finalview = ds.Tables[1].DefaultView;
                                                            if (finalview.Count > 0)
                                                            {
                                                                string hr_month_Value = finalview[0]["PayMonthNum"].ToString();
                                                                int indexmonth = hrpaymonth.IndexOf(hr_month_Value);
                                                                for (int hr = 0; hr <= indexmonth; hr++)
                                                                {
                                                                    month_count = 0;
                                                                    string hr_month1 = Convert.ToString(hrpaymonth[hr]);
                                                                    string hr_month_year = hr_month1 + "/" + Atyear;
                                                                    ds.Tables[3].DefaultView.RowFilter = "staff_code='" + staffcode + "'and mon_year ='" + hr_month_year + "'";
                                                                    finalview = ds.Tables[3].DefaultView;
                                                                    if (finalview.Count > 0)
                                                                    {
                                                                        for (int check = 4; check < finalview.Table.Columns.Count - 3; check++)
                                                                        {
                                                                            string attend_hr_value = Convert.ToString(finalview[0][check].ToString());
                                                                            if (attend_hr_value.Trim() != "" && attend_hr_value != null && attend_hr_value.Trim() != "-")
                                                                            {
                                                                                string[] split_hr_value_attend = attend_hr_value.Split('-');
                                                                                string first_split_attend = Convert.ToString(split_hr_value_attend[0]);
                                                                                string second_split_attend = Convert.ToString(split_hr_value_attend[1]);
                                                                                ds.Tables[2].DefaultView.RowFilter = "category='" + leave_type + "'";
                                                                                checkview = ds.Tables[2].DefaultView;
                                                                                if (checkview.Count > 0)
                                                                                {
                                                                                    string check_attend_value = Convert.ToString(checkview[0]["shortname"]);
                                                                                    if (check_attend_value == first_split_attend)
                                                                                    {
                                                                                        month_count = month_count + 0.5;
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                    if (check_attend_value == second_split_attend)
                                                                                    {
                                                                                        month_count = month_count + 0.5;
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    monthcount_total = monthcount_total + Convert.ToInt32(monthlyleave);
                                                                }
                                                            }
                                                        }
                                                        if (Convert.ToDouble(yearlyleave) < count_check_attend + add_leave_count)
                                                        {
                                                            leave_apply++;
                                                        }
                                                        if (monthcount_total < count_check_attend + add_leave_count)
                                                        {
                                                            leave_apply++;
                                                        }
                                                    }
                                                    else if (year1 == true)
                                                    {
                                                        if (hrpaymonth.Count > 0)
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "PayMonthNum=" + atmonth1 + "";
                                                            finalview = ds.Tables[1].DefaultView;
                                                            if (finalview.Count > 0)
                                                            {
                                                                string hr_month_Value = finalview[0]["PayMonthNum"].ToString();
                                                                int indexmonth = hrpaymonth.IndexOf(hr_month_Value);
                                                                for (int hr = 0; hr <= indexmonth; hr++)
                                                                {
                                                                    string hr_month1 = Convert.ToString(hrpaymonth[hr]);
                                                                    string hr_month_year = hr_month1 + "/" + Atyear;
                                                                    ds.Tables[3].DefaultView.RowFilter = "staff_code='" + staffcode + "'and mon_year ='" + hr_month_year + "'";
                                                                    finalview = ds.Tables[3].DefaultView;
                                                                    if (finalview.Count > 0)
                                                                    {
                                                                        for (int check = 4; check < finalview.Table.Columns.Count - 3; check++)
                                                                        {
                                                                            string attend_hr_value = Convert.ToString(finalview[0][check].ToString());
                                                                            if (attend_hr_value.Trim() != "" && attend_hr_value != null && attend_hr_value.Trim() != "-")
                                                                            {
                                                                                string[] split_hr_value_attend = attend_hr_value.Split('-');
                                                                                string first_split_attend = Convert.ToString(split_hr_value_attend[0]);
                                                                                string second_split_attend = Convert.ToString(split_hr_value_attend[1]);
                                                                                ds.Tables[2].DefaultView.RowFilter = "category='" + leave_type + "'";
                                                                                checkview = ds.Tables[2].DefaultView;
                                                                                if (checkview.Count > 0)
                                                                                {
                                                                                    string check_attend_value = Convert.ToString(checkview[0]["shortname"]);
                                                                                    if (check_attend_value == first_split_attend)
                                                                                    {
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                    if (check_attend_value == second_split_attend)
                                                                                    {
                                                                                        count_check_attend = count_check_attend + 0.5;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (Convert.ToDouble(yearlyleave) < count_check_attend + add_leave_count)
                                                        {
                                                            leave_apply++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (arr_add_attendance.Contains(leavecategory) == false)
                                        {
                                            leave_apply++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void updateattendace(string date, string st, string staff_code1, string mon_year1, string morning, string evening, string category, string stafftype, Hashtable hat, string row, string type, string deptvalue)
    {
        try
        {
            holiday_flage_add = 0;
            sunday_flage_add = 0;
            DataSet newdataset = new DataSet();
            string test_date = date;
            string[] split_test_date = test_date.Split('/');
            DateTime d1 = Convert.ToDateTime(split_test_date[1].ToString() + "/" + split_test_date[0].ToString() + "/" + split_test_date[2].ToString());
            string[] firt_date = txtfromdate.Text.ToString().Split('/');
            DateTime d7 = Convert.ToDateTime(firt_date[1].ToString() + "/" + firt_date[0].ToString() + "/" + firt_date[2].ToString());
            string firstdate = "";
            string selectquery = "";
            string updatequery = "";
            string join_value = "";
            int result_Value = 0;
            string attendace_value = "";
            string linkvaluequery = "";
            string linkvalue = "";
            if (st == "0")
            {
                while (d7 <= d1)
                {
                    d1 = d1.AddDays(-1);
                    if (d1.ToString("dddd") == "Sunday")
                    {
                        string date_splitvalue = Convert.ToString(d1);
                        string[] date_value1 = date_splitvalue.Split('/');
                        string befor_m = "";
                        string befor_e = "";
                        string after_m = "";
                        string after_e = "";
                        DateTime d123 = Convert.ToDateTime(date_value1[0].ToString() + "/" + date_value1[1].ToString() + "/" + date_value1[2].ToString());
                        d123 = d123.AddDays(-1);
                        if (hat.ContainsKey(d123.ToString("dd/MM/yyyy")) == true)
                        {
                            string att_value_hash = Convert.ToString(hat[d123.ToString("dd/MM/yyyy")]);
                            if (att_value_hash != "")
                            {
                                string[] split_hash_value = att_value_hash.Split('-');
                                if (split_hash_value.Length > 0)
                                {
                                    befor_m = Convert.ToString(split_hash_value[0]);
                                    befor_e = Convert.ToString(split_hash_value[1]);
                                }
                            }
                        }
                        else
                        {
                            firstdate = d123.ToString("dd");
                            firstdate = firstdate.TrimStart('0');
                            firstdate = "[" + firstdate + "]";
                            selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                            newdataset.Clear();
                            newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                            if (newdataset.Tables[0].Rows.Count > 0)
                            {
                                attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                if (attendace_value.Trim() != "")
                                {
                                    string[] split_attendance_value = attendace_value.Split('-');
                                    if (split_attendance_value.Length > 0)
                                    {
                                        befor_m = Convert.ToString(split_attendance_value[0]);
                                        befor_e = Convert.ToString(split_attendance_value[1]);
                                    }
                                }
                            }
                        }
                        d123 = d123.AddDays(2);
                        if (hat.ContainsKey(d123.ToString("dd/MM/yyyy")) == true)
                        {
                            string att_value_hash = Convert.ToString(hat[d123.ToString("dd/MM/yyyy")]);
                            if (att_value_hash != "")
                            {
                                string[] split_hash_value = att_value_hash.Split('-');
                                if (split_hash_value.Length > 0)
                                {
                                    after_m = Convert.ToString(split_hash_value[0]);
                                    after_e = Convert.ToString(split_hash_value[1]);
                                }
                            }
                        }
                        else
                        {
                            firstdate = d123.ToString("dd");
                            firstdate = firstdate.TrimStart('0');
                            firstdate = "[" + firstdate + "]";
                            selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                            newdataset.Clear();
                            newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                            if (newdataset.Tables[0].Rows.Count > 0)
                            {
                                attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                if (attendace_value.Trim() != "")
                                {
                                    string[] split_attendance_value = attendace_value.Split('-');
                                    if (split_attendance_value.Length > 0)
                                    {
                                        after_m = Convert.ToString(split_attendance_value[0]);
                                        after_e = Convert.ToString(split_attendance_value[1]);
                                    }
                                }
                            }
                        }
                        if (befor_m == after_m && befor_e == after_e)
                        {
                            sunday_flage_add++;
                        }
                    }
                }
            }
            else if (st == "1")
            {
                int holidaycount = 0;
                bool checkwile = false;
                while (d7 <= d1)
                {
                    d1 = d1.AddDays(-1);
                    //linkvaluequery = "select * from InsSettings where LinkName like 'Staff Holiday By Staff Type' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                    linkvaluequery = d2.GetFunction("select value from Master_Settings where settings='HR_PanelSettings' and usercode='" + Convert.ToString(Session["usercode"]) + "'");
                    //newdataset.Clear();
                    //newdataset = d2.select_method_wo_parameter(linkvaluequery, "Text");
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    linkvalue = Convert.ToString(newdataset.Tables[0].Rows[0]["Linkvalue"]);
                    //}
                    if (linkvaluequery.Trim().Contains('3'))
                        linkvalue = "1";
                    else
                        linkvalue = "0";
                    string holidayquery = "";
                    if (linkvalue == "0")
                    {
                        //holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + d1 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + category + "') and t.staff_code ='" + staff_code1 + "'";
                        holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + d1 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + category + "') and t.staff_code ='" + staff_code1 + "' and h.dept_code in('" + deptvalue + "') and t.dept_code=h.dept_code and latestrec='1'";
                    }
                    if (linkvalue == "1")
                    {
                        //holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + d1 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stafftype + "') ";

                        holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + d1 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stafftype + "') and and dept_code in('" + deptvalue + "')";
                    }
                    newdataset.Clear();
                    newdataset = d2.select_method_wo_parameter(holidayquery, "Text");
                    if (newdataset.Tables[0].Rows.Count > 0)
                    {
                        string holiday_value = Convert.ToString(newdataset.Tables[0].Rows[0]["halforfull"]);
                        if (holiday_value == "False")
                        {
                            string date_splitvalue = Convert.ToString(d1);
                            string[] date_value1 = date_splitvalue.Split('/');
                            string befor_m = "";
                            string befor_e = "";
                            string after_m = "";
                            string after_e = "";
                            bool chek_calue = false;
                            DateTime d123 = Convert.ToDateTime(date_value1[0].ToString() + "/" + date_value1[1].ToString() + "/" + date_value1[2].ToString());
                            DateTime dcheck;
                            d123 = d123.AddDays(-1);
                            dcheck = d123;
                            while (checkwile == false)
                            {
                                if (linkvalue == "0")
                                {
                                    //holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + category + "') and t.staff_code ='" + staff_code1 + "'";

                                    holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + category + "') and t.staff_code ='" + staff_code1 + "' and h.dept_code in('" + deptvalue + "') and t.dept_code=h.dept_code and latestrec='1'";

                                }
                                if (linkvalue == "1")
                                {
                                    //holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stafftype + "') ";

                                    holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stafftype + "')and dept_code in('" + deptvalue + "') ";
                                }
                                newdataset.Clear();
                                newdataset = d2.select_method_wo_parameter(holidayquery, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    firstdate = dcheck.ToString("dd");
                                    firstdate = firstdate.TrimStart('0');
                                    firstdate = "[" + firstdate + "]";
                                    selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                                    newdataset.Clear();
                                    newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                                    if (newdataset.Tables[0].Rows.Count > 0)
                                    {
                                        attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                        if (attendace_value.Trim() != "")
                                        {
                                            string[] split_attendance_value = attendace_value.Split('-');
                                            if (split_attendance_value.Length > 0)
                                            {
                                                befor_m = Convert.ToString(split_attendance_value[0]);
                                                befor_e = Convert.ToString(split_attendance_value[1]);
                                                if (holidaycount == 0)
                                                {
                                                    holidaycount = 0;
                                                    checkwile = true;
                                                    chek_calue = true;
                                                }
                                                else
                                                {
                                                    holidaycount = holidaycount - 1;
                                                    checkwile = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            holidaycount++;
                                            holidaydate.Add(dcheck.ToString("dd/MM/yyyy"));
                                            dcheck = dcheck.AddDays(-1);
                                        }
                                    }
                                    else
                                    {
                                        holidaycount++;
                                        holidaydate.Add(dcheck.ToString("dd/MM/yyyy"));
                                        dcheck = dcheck.AddDays(-1);
                                    }
                                }
                                else
                                {
                                    checkwile = true;
                                }
                            }
                            if (hat.ContainsKey(dcheck.ToString("dd/MM/yyyy")) == true)
                            {
                                string att_value_hash = Convert.ToString(hat[dcheck.ToString("dd/MM/yyyy")]);
                                if (att_value_hash != "")
                                {
                                    string[] split_hash_value = att_value_hash.Split('-');
                                    if (split_hash_value.Length > 0)
                                    {
                                        befor_m = Convert.ToString(split_hash_value[0]);
                                        befor_e = Convert.ToString(split_hash_value[1]);
                                    }
                                }
                            }
                            else
                            {
                                firstdate = dcheck.ToString("dd");
                                firstdate = firstdate.TrimStart('0');
                                firstdate = "[" + firstdate + "]";
                                selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                                newdataset.Clear();
                                newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                    if (attendace_value.Trim() != "")
                                    {
                                        string[] split_attendance_value = attendace_value.Split('-');
                                        if (split_attendance_value.Length > 0)
                                        {
                                            befor_m = Convert.ToString(split_attendance_value[0]);
                                            befor_e = Convert.ToString(split_attendance_value[1]);
                                        }
                                    }
                                }
                            }
                            d123 = d123.AddDays(2);
                            if (hat.ContainsKey(d123.ToString("dd/MM/yyyy")) == true)
                            {
                                string att_value_hash = Convert.ToString(hat[d123.ToString("dd/MM/yyyy")]);
                                if (att_value_hash != "")
                                {
                                    string[] split_hash_value = att_value_hash.Split('-');
                                    if (split_hash_value.Length > 0)
                                    {
                                        after_m = Convert.ToString(split_hash_value[0]);
                                        after_e = Convert.ToString(split_hash_value[1]);
                                    }
                                }
                            }
                            else
                            {
                                firstdate = d123.ToString("dd");
                                firstdate = firstdate.TrimStart('0');
                                firstdate = "[" + firstdate + "]";
                                selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                                newdataset.Clear();
                                newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                    if (attendace_value.Trim() != "")
                                    {
                                        string[] split_attendance_value = attendace_value.Split('-');
                                        if (split_attendance_value.Length > 0)
                                        {
                                            after_m = Convert.ToString(split_attendance_value[0]);
                                            after_e = Convert.ToString(split_attendance_value[1]);
                                        }
                                    }
                                }
                            }
                            if (befor_m == after_m && befor_e == after_e)
                            {
                                bool flagset = false;
                                holidaydate.Add(d1.ToString("dd/MM/yyyy"));
                                string seconddate = d1.ToString("dd");
                                seconddate = seconddate.TrimStart('0');
                                seconddate = "[" + seconddate + "]";
                                join_value = after_m + "-" + after_e;
                                if (chek_calue == true && holidaycount == 0)
                                {
                                    holiday_flage_add = 0;
                                }
                                else
                                {
                                    holiday_flage_add = holidaycount + 1;
                                }
                            }
                        }
                    }
                }
            }
            else if (st == "2")
            {
                int holidaycount = 0;
                bool checkwile = false;
                Boolean test_falge = false;
                while (d7 <= d1)
                {
                    test_falge = false;
                    d1 = d1.AddDays(-1);
                    //linkvaluequery = "select * from InsSettings where LinkName like 'Staff Holiday By Staff Type' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                    //newdataset.Clear();
                    //newdataset = d2.select_method_wo_parameter(linkvaluequery, "Text");
                    //if (newdataset.Tables[0].Rows.Count > 0)
                    //{
                    //    linkvalue = Convert.ToString(newdataset.Tables[0].Rows[0]["Linkvalue"]);
                    //}
                    linkvaluequery = d2.GetFunction("select value from Master_Settings where settings='HR_PanelSettings' and usercode='" + Convert.ToString(Session["usercode"]) + "'");
                    if (linkvaluequery.Trim().Contains('3'))
                        linkvalue = "1";
                    else
                        linkvalue = "0";
                    string holidayquery = "";
                    if (linkvalue == "0")
                    {
                        //holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + d1 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + category + "') and t.staff_code ='" + staff_code1 + "'";
                        holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + d1 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + category + "') and t.staff_code ='" + staff_code1 + "' and h.dept_code in('" + deptvalue + "') and t.dept_code=h.dept_code and latestrec='1'";

                    }
                    if (linkvalue == "1")
                    {
                        //holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + d1 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stafftype + "')";


                        holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + d1 + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stafftype + "') and dept_code in('" + deptvalue + "')";
                    }
                    newdataset.Clear();
                    newdataset = d2.select_method_wo_parameter(holidayquery, "Text");
                    if (newdataset.Tables[0].Rows.Count > 0)
                    {
                        string holiday_value = Convert.ToString(newdataset.Tables[0].Rows[0]["halforfull"]);
                        if (holiday_value == "False")
                        {
                            string date_splitvalue = Convert.ToString(d1);
                            string[] date_value1 = date_splitvalue.Split('/');
                            string befor_m = "";
                            string befor_e = "";
                            string after_m = "";
                            string after_e = "";
                            bool chek_calue = false;
                            DateTime d123 = Convert.ToDateTime(date_value1[0].ToString() + "/" + date_value1[1].ToString() + "/" + date_value1[2].ToString());
                            DateTime dcheck;
                            d123 = d123.AddDays(-1);
                            dcheck = d123;
                            while (checkwile == false)
                            {
                                if (linkvalue == "0")
                                {
                                    //holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + category + "') and t.staff_code ='" + staff_code1 + "'";

                                    holidayquery = "select distinct halforfull ,morning,evening,ltype,h.stftype,holiday_desc from holidayStaff h,stafftrans t  where h.category_code =t.category_code and holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and h.category_code in ('" + category + "') and t.staff_code ='" + staff_code1 + "' and h.dept_code in('" + deptvalue + "') and t.dept_code=h.dept_code and latestrec='1'";

                                }
                                if (linkvalue == "1")
                                {
                                    //holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stafftype + "') ";


                                    holidayquery = "select halforfull ,morning,evening,ltype,stftype,holiday_desc from holidayStaff where holiday_date='" + dcheck.ToString("MM/dd/yyyy") + "' and college_code='" + ddlcollege.SelectedItem.Value + "' and stftype in ('" + stafftype + "') and dept_code in('" + deptvalue + "')";
                                }
                                newdataset.Clear();
                                newdataset = d2.select_method_wo_parameter(holidayquery, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    firstdate = dcheck.ToString("dd");
                                    firstdate = firstdate.TrimStart('0');
                                    firstdate = "[" + firstdate + "]";
                                    selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                                    newdataset.Clear();
                                    newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                                    if (newdataset.Tables[0].Rows.Count > 0)
                                    {
                                        attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                        if (attendace_value.Trim() != "")
                                        {
                                            string[] split_attendance_value = attendace_value.Split('-');
                                            if (split_attendance_value.Length > 0)
                                            {
                                                befor_m = Convert.ToString(split_attendance_value[0]);
                                                befor_e = Convert.ToString(split_attendance_value[1]);
                                                if (holidaycount == 0)
                                                {
                                                    holidaycount = 0;
                                                    checkwile = true;
                                                    chek_calue = true;
                                                }
                                                else
                                                {
                                                    holidaycount = holidaycount - 1;
                                                    checkwile = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            holidaycount++;
                                            holidaydate.Add(dcheck.ToString("dd/MM/yyyy"));
                                            dcheck = dcheck.AddDays(-1);
                                        }
                                    }
                                    else
                                    {
                                        holidaycount++;
                                        holidaydate.Add(dcheck.ToString("dd/MM/yyyy"));
                                        dcheck = dcheck.AddDays(-1);
                                    }
                                }
                                else
                                {
                                    checkwile = true;
                                }
                            }
                            if (hat.ContainsKey(dcheck.ToString("dd/MM/yyyy")) == true)
                            {
                                string att_value_hash = Convert.ToString(hat[dcheck.ToString("dd/MM/yyyy")]);
                                if (att_value_hash != "")
                                {
                                    string[] split_hash_value = att_value_hash.Split('-');
                                    if (split_hash_value.Length > 0)
                                    {
                                        befor_m = Convert.ToString(split_hash_value[0]);
                                        befor_e = Convert.ToString(split_hash_value[1]);
                                    }
                                }
                            }
                            else
                            {
                                firstdate = dcheck.ToString("dd");
                                firstdate = firstdate.TrimStart('0');
                                firstdate = "[" + firstdate + "]";
                                selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                                newdataset.Clear();
                                newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                    if (attendace_value.Trim() != "")
                                    {
                                        string[] split_attendance_value = attendace_value.Split('-');
                                        if (split_attendance_value.Length > 0)
                                        {
                                            befor_m = Convert.ToString(split_attendance_value[0]);
                                            befor_e = Convert.ToString(split_attendance_value[1]);
                                        }
                                    }
                                }
                            }
                            d123 = d123.AddDays(2);
                            if (hat.ContainsKey(d123.ToString("dd/MM/yyyy")) == true)
                            {
                                string att_value_hash = Convert.ToString(hat[d123.ToString("dd/MM/yyyy")]);
                                if (att_value_hash != "")
                                {
                                    string[] split_hash_value = att_value_hash.Split('-');
                                    if (split_hash_value.Length > 0)
                                    {
                                        after_m = Convert.ToString(split_hash_value[0]);
                                        after_e = Convert.ToString(split_hash_value[1]);
                                    }
                                }
                            }
                            else
                            {
                                firstdate = d123.ToString("dd");
                                firstdate = firstdate.TrimStart('0');
                                firstdate = "[" + firstdate + "]";
                                selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                                newdataset.Clear();
                                newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                    if (attendace_value.Trim() != "")
                                    {
                                        string[] split_attendance_value = attendace_value.Split('-');
                                        if (split_attendance_value.Length > 0)
                                        {
                                            after_m = Convert.ToString(split_attendance_value[0]);
                                            after_e = Convert.ToString(split_attendance_value[1]);
                                        }
                                    }
                                }
                            }
                            if (befor_m == after_m && befor_e == after_e)
                            {
                                bool flagset = false;
                                holidaydate.Add(d1.ToString("dd/MM/yyyy"));
                                string seconddate = d1.ToString("dd");
                                seconddate = seconddate.TrimStart('0');
                                seconddate = "[" + seconddate + "]";
                                join_value = after_m + "-" + after_e;
                                if (chek_calue == true && holidaycount == 0)
                                {
                                    holiday_flage_add = 0;
                                }
                                else
                                {
                                    holiday_flage_add = holidaycount + 1;
                                }
                            }
                        }
                        test_falge = true;
                    }
                    if (test_falge == false)
                    {
                        if (d1.ToString("dddd") == "Sunday")
                        {
                            string date_splitvalue = Convert.ToString(d1);
                            string[] date_value1 = date_splitvalue.Split('/');
                            string befor_m = "";
                            string befor_e = "";
                            string after_m = "";
                            string after_e = "";
                            DateTime d123 = Convert.ToDateTime(date_value1[0].ToString() + "/" + date_value1[1].ToString() + "/" + date_value1[2].ToString());
                            d123 = d123.AddDays(-1);
                            if (hat.ContainsKey(d123.ToString("dd/MM/yyyy")) == true)
                            {
                                string att_value_hash = Convert.ToString(hat[d123.ToString("dd/MM/yyyy")]);
                                if (att_value_hash != "")
                                {
                                    string[] split_hash_value = att_value_hash.Split('-');
                                    if (split_hash_value.Length > 0)
                                    {
                                        befor_m = Convert.ToString(split_hash_value[0]);
                                        befor_e = Convert.ToString(split_hash_value[1]);
                                    }
                                }
                            }
                            else
                            {
                                firstdate = d123.ToString("dd");
                                firstdate = firstdate.TrimStart('0');
                                firstdate = "[" + firstdate + "]";
                                selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                                newdataset.Clear();
                                newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                    if (attendace_value.Trim() != "")
                                    {
                                        string[] split_attendance_value = attendace_value.Split('-');
                                        if (split_attendance_value.Length > 0)
                                        {
                                            befor_m = Convert.ToString(split_attendance_value[0]);
                                            befor_e = Convert.ToString(split_attendance_value[1]);
                                        }
                                    }
                                }
                            }
                            d123 = d123.AddDays(2);
                            if (hat.ContainsKey(d123.ToString("dd/MM/yyyy")) == true)
                            {
                                string att_value_hash = Convert.ToString(hat[d123.ToString("dd/MM/yyyy")]);
                                if (att_value_hash != "")
                                {
                                    string[] split_hash_value = att_value_hash.Split('-');
                                    if (split_hash_value.Length > 0)
                                    {
                                        after_m = Convert.ToString(split_hash_value[0]);
                                        after_e = Convert.ToString(split_hash_value[1]);
                                    }
                                }
                            }
                            else
                            {
                                firstdate = d123.ToString("dd");
                                firstdate = firstdate.TrimStart('0');
                                firstdate = "[" + firstdate + "]";
                                selectquery = "select " + firstdate + " from staff_attnd where staff_code='" + staff_code1 + "' and mon_year ='" + mon_year1 + "'";
                                newdataset.Clear();
                                newdataset = d2.select_method_wo_parameter(selectquery, "Text");
                                if (newdataset.Tables[0].Rows.Count > 0)
                                {
                                    attendace_value = newdataset.Tables[0].Rows[0][0].ToString();
                                    if (attendace_value.Trim() != "")
                                    {
                                        string[] split_attendance_value = attendace_value.Split('-');
                                        if (split_attendance_value.Length > 0)
                                        {
                                            after_m = Convert.ToString(split_attendance_value[0]);
                                            after_e = Convert.ToString(split_attendance_value[1]);
                                        }
                                    }
                                }
                            }
                            if (befor_m == after_m && befor_e == after_e)
                            {
                                sunday_flage_add++;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void calcutale1()
    {
        ds.Clear();
        string attendquery = "";
        //**********Modified By Srinath 22/Jan/2014 For JEPPIEAR
        // attendquery = "Select distinct status,shortname   from leave_category where status<>'' and college_code='" + ddlcollege.SelectedItem.Value + "'";
        attendquery = "Select distinct status,shortname from leave_category where college_code='" + ddlcollege.SelectedItem.Value + "'";
        ds = d2.select_method_wo_parameter(attendquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int attnd = 0; attnd < ds.Tables[0].Rows.Count; attnd++)
            {
                if (leave.Contains(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim()) == false)
                {
                    if (ds.Tables[0].Rows[attnd]["status"].ToString().Trim().ToLower() == "0")//Added by srinath 23/jan2014
                    {
                        hat1.Add(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim(), ds.Tables[0].Rows[attnd]["status"].ToString().Trim());
                    }
                    leave.Add(ds.Tables[0].Rows[attnd]["shortname"].ToString().Trim());
                }
            }
        }
        if (!hat1.ContainsKey("P"))
        {
            hat1.Add("P", "0");
        }
        if (!hat1.ContainsKey("A"))
        {
            hat1.Add("A", "2");
        }
    }
    protected void chkattendance_CheckedChanged(object sender, EventArgs e)
    {
        if (chkattendance.Checked == true)
        {
            for (int i = 0; i < chklsattendance.Items.Count; i++)
            {
                chklsattendance.Items[i].Selected = true;
            }
            txtattendance.Text = "Attendance(" + chklsattendance.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsattendance.Items.Count; i++)
            {
                chklsattendance.Items[i].Selected = false;
            }
            txtattendance.Text = "---Select---";
        }
    }
    protected void chklsattendance_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtattendance.Text = "---Select---";
        chkattendance.Checked = false;
        int copu = 0;
        for (int i = 0; i < chklsattendance.Items.Count; i++)
        {
            if (chklsattendance.Items[i].Selected == true)
            {
                copu++;
            }
        }
        if (copu > 0)
        {
            txtattendance.Text = "Attendance(" + copu + ")";
            if (copu == chklsattendance.Items.Count)
            {
                chkattendance.Checked = true;
            }
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (FpSpread2.Sheets[0].RowCount > 0)
        {
            FpSpread2.Sheets[0].SaveViewState();
        }
    }
    public class MyClass : IDisposable
    {
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // called via myClass.Dispose(). 
                    // OK to use any private object references
                }
                disposed = true;
            }
            disposed = true;
        }
        public void Dispose() // Implement IDisposable
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~MyClass() // the finalizer
        {
            Dispose(false);
        }
    }

    protected void ddlsearchappstf_change(object sender, EventArgs e)//delsi
    {
        if (ddlsearchappstf.SelectedItem.Text == "Staff Name")
        {
            txt_staffname.Visible = true;
            txt_staffname.Enabled = true;
            txt_StaffCode.Visible = false;
        }
        if (ddlsearchappstf.SelectedItem.Text == "Staff Code")
        {
            txt_StaffCode.Enabled = true;
            txt_StaffCode.Visible = true;
            txt_staffname.Visible = false;
        }
        txt_StaffCode.Text = "";
        txt_staffname.Text = "";
        //txtappstfname.Text = "";
    }

    protected void bindsearchstapp()
    {
        ddlsearchappstf.Items.Clear();
        ddlsearchappstf.Items.Add(new ListItem("Select", "0"));
        ddlsearchappstf.Items.Add(new ListItem("Staff Name", "1"));
        ddlsearchappstf.Items.Add(new ListItem("Staff Code", "2"));
        ddlsearchappstf.DataBind();

        if (ddlsearchappstf.SelectedItem.Text == "Select")
        {
            txt_StaffCode.Text = "";
            txt_staffname.Text = "";
        }
    }
    protected void txt_staffname_change(object sender, EventArgs e)
    {
        //txtappstfapplcode.Text = "";
        //txtappstfcode.Text = "";
    }
    protected void txt_staffcode_change(object sender, EventArgs e)
    {
        //txtappstfapplcode.Text = "";
        //txtappstfcode.Text = "";
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        //string collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (!String.IsNullOrEmpty(prefixText.Trim()))
        {
            if (clgcode1 != "")
            {
                string query = "select staff_name  from staffmaster where (resign =0 and settled =0 and isnull (Discontinue,'0') ='0') and staff_name like  '%" + prefixText + "%' and college_code='" + clgcode1 + "'";
                name = ws.Getname(query);
            }
        }
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (!String.IsNullOrEmpty(prefixText.Trim()))
        {
            if (clgcode1 != "")
            {
                string query = "select staff_code from staffmaster where (resign =0 and settled =0 and isnull (Discontinue,'0') ='0') and staff_code like  '%" + prefixText + "%' and college_code='" + clgcode1 + "'";
                name = ws.Getname(query);
            }
        }
        return name;
    }
}
