using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using System.Drawing;
using System.Configuration;

public partial class Designation_Master_Alter : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    string maincol = string.Empty;
    string popcol = string.Empty;
    string newcol = string.Empty;
    static string autocol = string.Empty;
    bool spreadclick = false;
    bool spreaddeptclick = false;
    bool flag_true = false;
    bool flagdept = false;

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
            if (!IsPostBack)
            {
                bindcollege();
                if (ddl_col.Items.Count > 0)
                {
                    maincol = Convert.ToString(ddl_col.SelectedItem.Value);
                    autocol = Convert.ToString(ddl_col.SelectedItem.Value);
                }
                if (ddlcoldept.Items.Count > 0)
                {
                    popcol = Convert.ToString(ddlcoldept.SelectedItem.Value);
                }
                if (ddlnewcol.Items.Count > 0)
                {
                    newcol = Convert.ToString(ddlnewcol.SelectedItem.Value);
                    autocol = Convert.ToString(ddlnewcol.SelectedItem.Value);
                }
                loadstream();
                loadstreamlst();
                bindaddreason();
                btngo_click(sender, e);
            }
            if (ddl_col.Items.Count > 0)
            {
                maincol = Convert.ToString(ddl_col.SelectedItem.Value);
                autocol = Convert.ToString(ddl_col.SelectedItem.Value);
            }
            if (ddlcoldept.Items.Count > 0)
            {
                popcol = Convert.ToString(ddlcoldept.SelectedItem.Value);
            }
            if (ddlnewcol.Items.Count > 0)
            {
                newcol = Convert.ToString(ddlnewcol.SelectedItem.Value);
                autocol = Convert.ToString(ddlnewcol.SelectedItem.Value);
            }
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    [WebMethod]
    public static string CheckDesAcronym(string desAcronym)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string desacr_name = desAcronym;
            if (desacr_name.Trim() != "" && desacr_name != null)
            {
                string queryacr = dd.GetFunction("select distinct desig_acronym from desig_master where desig_acronym='" + desacr_name + "' and collegeCode='" + autocol + "'");
                if (queryacr.Trim() == "" || queryacr == null || queryacr == "0" || queryacr == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    [WebMethod]
    public static string CheckDesName(string desName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string des_name = desName;
            if (des_name.Trim() != "" && des_name != null)
            {
                string queryacr = dd.GetFunction("select distinct desig_name from desig_master where desig_name='" + des_name + "' and collegeCode='" + autocol + "'");
                if (queryacr.Trim() == "" || queryacr == null || queryacr == "0" || queryacr == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetDesig(string prefixText)
    {
        DAccess2 da = new DAccess2();
        DataSet das = new DataSet();
        List<string> lstheader = new List<string>();
        string getheader = "select distinct desig_name from desig_master where desig_name like '" + prefixText + "%' and collegeCode='" + autocol + "'";

        das = da.select_method_wo_parameter(getheader, "Text");
        if (das.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < das.Tables[0].Rows.Count; i++)
            {
                lstheader.Add(das.Tables[0].Rows[i]["desig_name"].ToString());
            }
        }
        return lstheader;
    }

    protected void cb_stream_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_stream, cbl_stream, txt_stream, "Stream");
    }

    protected void cbl_stream_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_stream, cbl_stream, txt_stream, "Stream");
    }

    protected void cb_dept_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txtdept, "Department");
    }

    protected void cbl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txtdept, "Department");
    }

    protected void btn_addreason_Click(object sender, EventArgs e)
    {
        try
        {
            if (lbl_addreason.Text == "Add Staff Type")
            {
                if (txt_addreason.Text != "")
                {
                    txt_addreason.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_addreason.Text);
                    string getexist = "select * from TextValTable where TextVal ='" + txt_addreason.Text + "' and TextCriteria ='Stype' and college_code ='" + newcol + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(getexist, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Already Exist!";
                    }
                    else
                    {
                        string sql = "insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_addreason.Text + "','Stype','" + newcol + "')";
                        int insert = d2.update_method_wo_parameter(sql, "Text");
                        if (insert != 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Saved Successfully";
                            txt_addreason.Text = "";
                            plusdiv.Visible = false;
                            panel_addreason.Visible = false;
                        }
                        bindaddreason();
                        txt_addreason.Text = "";
                    }
                }
                else
                {
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Enter the Reason";
                }
            }
        }
        catch { }
    }

    protected void btn_exitaddreason_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
    }

    protected void btnstaf_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addreason.Visible = true;
        lbl_addreason.Visible = true;
        lbl_addreason.Text = "Add Staff Type";
        lblerror.Visible = false;
    }

    protected void btnstafmin_Click(object sender, EventArgs e)
    {
        if (ddlstaftyp.SelectedIndex == 0)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "No Record Selected";
        }
        else
        {
            imgDiv1.Visible = true;
            lblconfirm.Visible = true;
            lblconfirm.Text = "Do you want to delete this Reason?";
        }
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlstaftyp.SelectedIndex != 0)
            {
                string selquery = "";
                selquery = "select staffcategory from desig_master where collegeCode='" + newcol + "' and staffcategory='" + ddlstaftyp.SelectedItem.Value.ToString() + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selquery, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "You can't delete this Record!";
                    }
                    else
                    {
                        string sql = "delete from TextValTable where TextCode='" + ddlstaftyp.SelectedItem.Value.ToString() + "' and TextCriteria='Stype' and college_code='" + newcol + "'";
                        int delete = d2.update_method_wo_parameter(sql, "Text");
                        if (delete != 0)
                        {
                            bindaddreason();
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Deleted Successfully";

                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Selected";
                        }
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Selected";
                }
                imgDiv1.Visible = false;
                lblconfirm.Visible = false;
                ddlstaftyp.SelectedItem.Value = Convert.ToString(ds.Tables[0].Rows[0]["staffcategory"]);
            }
        }
        catch { }
    }

    protected void btnno_Click(object sender, EventArgs e)
    {
        imgDiv1.Visible = false;
        lblconfirm.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void imgdept_Click(object sender, EventArgs e)
    {
        popdept.Visible = false;
    }

    protected void lb_deptpr_click(object sender, EventArgs e)
    {
        popdept.Visible = true;
        ddlcoldept.SelectedIndex = ddlcoldept.Items.IndexOf(ddlcoldept.Items.FindByValue(ddl_col.SelectedItem.Value));
        popcol = Convert.ToString(ddlcoldept.SelectedItem.Value);
        binddept();
        Fpspreadpopdept.Visible = false;
        chkdeptpriority1.Checked = false;
        chkdeptpriority.Checked = false;
        divdept.Visible = false;
        btnsetdeptpriority.Visible = false;
        btnresetdeptpriority.Visible = false;
        btnexitdept.Visible = false;
    }

    protected void ddl_col_Change(object sender, EventArgs e)
    {
        maincol = Convert.ToString(ddl_col.SelectedItem.Value);
        autocol = Convert.ToString(ddl_col.SelectedItem.Value);

        div1.Visible = false;
        Fpspread1.Visible = false;
        rportprint.Visible = false;
        txt_desname.Text = "";
        btnsetpriority.Visible = false;
        btnresetpriority.Visible = false;
        chkpriority.Checked = false;
        lblerrgo.Visible = false;
        //lblerrgo.Text = "No Record Found";
    }

    protected void ddlcoldept_Change(object sender, EventArgs e)
    {
        popcol = Convert.ToString(ddlcoldept.SelectedItem.Value);
        binddept();
        Fpspreadpopdept.Visible = false;
        divdept.Visible = false;
        btnsetdeptpriority.Visible = false;
        btnresetdeptpriority.Visible = false;
        btnexitdept.Visible = false;
        chkdeptpriority.Checked = false;
        chkdeptpriority1.Checked = false;
    }

    protected void ddlnewcol_Change(object sender, EventArgs e)
    {
        newcol = Convert.ToString(ddlnewcol.SelectedItem.Value);
        autocol = Convert.ToString(ddlnewcol.SelectedItem.Value);
        loadstream();
        loadstreamlst();
        bindaddreason();
        btnaddnew_click(sender, e);
    }

    protected void chkpriority_Change(object sender, EventArgs e)
    {
        btngo_click(sender, e);
    }

    protected void chkdeptpriority_Change(object sender, EventArgs e)
    {
        btnpopdeptgo_click(sender, e);
    }

    protected void chkdeptpriority1_Change(object sender, EventArgs e)
    {
        btnpopdeptgo_click(sender, e);
    }

    protected void btnsetpriority_Click(object sender, EventArgs e)
    {
        try
        {
            alertpopwindow.Visible = true;
            if (Fpspread1.Sheets[0].Rows.Count > 0 && chkpriority.Checked)
            {
                for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
                {
                    string priority = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 5].Text.Trim());
                    string desigcode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Tag);
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        int insQ = d2.update_method_wo_parameter("update desig_master set priority=" + priority + " where desig_code=" + desigcode + "  and collegecode=" + maincol + "", "Text");
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
            if (Fpspread1.Sheets[0].Rows.Count > 0 && chkpriority.Checked)
            {
                for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
                {
                    Fpspread1.Sheets[0].Cells[i, 4].Locked = false;
                    Fpspread1.Sheets[0].Cells[i, 4].Value = 0;
                    string desigcode = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Tag);
                    Fpspread1.Sheets[0].Cells[i, 5].Text = "";
                    int insupd = d2.update_method_wo_parameter("update desig_master set priority=NULL where desig_code=" + desigcode + "  and collegecode=" + maincol + "", "Text");
                }
            }
            Fpspread1.SaveChanges();
        }
        catch { }
    }

    protected void btnsetdeptpriority_click(object sender, EventArgs e)
    {
        try
        {
            alertpopwindow.Visible = true;
            lblalerterr.Visible = true;
            int upcount = 0;

            if (Fpspreadpopdept.Sheets[0].Rows.Count > 0 && (chkdeptpriority.Checked && chkdeptpriority1.Checked))
            {
                for (int i = 0; i < Fpspreadpopdept.Sheets[0].Rows.Count; i++)
                {
                    string updquery = "";
                    bool entry_flag = false;
                    string deptcode = Convert.ToString(Fpspreadpopdept.Sheets[0].Cells[i, 2].Tag);
                    string priority = Convert.ToString(Fpspreadpopdept.Sheets[0].Cells[i, 4].Text.Trim());
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        entry_flag = true;
                        updquery = "update hrdept_master set priority='" + priority + "'";
                    }
                    string priority1 = Convert.ToString(Fpspreadpopdept.Sheets[0].Cells[i, 6].Text.Trim());
                    if (priority1.Trim() != "" && priority1.Trim() != "0")
                    {
                        if (entry_flag == true)
                        {
                            updquery = updquery + " ,Priority1='" + priority1 + "'";
                        }
                        else
                        {
                            updquery = "update hrdept_master set Priority1='" + priority1 + "'";
                        }
                    }
                    if (updquery.Trim() != "")
                    {
                        updquery = updquery + " where dept_code='" + deptcode + "' and college_code=" + popcol + "";
                        int insQ = d2.update_method_wo_parameter(updquery, "Text");
                        if (insQ > 0)
                        {
                            upcount++;
                        }
                    }
                }
            }
            else if (Fpspreadpopdept.Sheets[0].RowCount > 0 && chkdeptpriority.Checked)
            {
                for (int i = 0; i < Fpspreadpopdept.Sheets[0].Rows.Count; i++)
                {
                    string priority = Convert.ToString(Fpspreadpopdept.Sheets[0].Cells[i, 4].Text.Trim());
                    string deptcode = Convert.ToString(Fpspreadpopdept.Sheets[0].Cells[i, 2].Tag);
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        int insQ = d2.update_method_wo_parameter("update hrdept_master set priority='" + priority + "' where dept_code='" + deptcode + "'  and college_code=" + popcol + "", "Text");
                        if (insQ > 0)
                        {
                            upcount++;
                        }
                    }
                }
            }
            else if (Fpspreadpopdept.Sheets[0].RowCount > 0 && chkdeptpriority1.Checked)
            {
                for (int i = 0; i < Fpspreadpopdept.Sheets[0].Rows.Count; i++)
                {
                    string priority1 = Convert.ToString(Fpspreadpopdept.Sheets[0].Cells[i, 6].Text.Trim());
                    string deptcode = Convert.ToString(Fpspreadpopdept.Sheets[0].Cells[i, 2].Tag);
                    if (priority1.Trim() != "" && priority1.Trim() != "0")
                    {
                        int insQ = d2.update_method_wo_parameter("update hrdept_master set Priority1='" + priority1 + "' where dept_code='" + deptcode + "'  and college_code=" + popcol + "", "Text");
                        if (insQ > 0)
                        {
                            upcount++;
                        }
                    }
                }
            }
            else
            {
                lblalerterr.Text = "Priority Not Assigned";
            }
            if (upcount > 0)
            {
                lblalerterr.Text = "Priority Assigned";
            }
            else
            {
                lblalerterr.Text = "Priority Not Assigned";
            }
        }
        catch { lblalerterr.Text = "Priority Not Assigned"; }
    }

    protected void btnresetdeptpriority_click(object sender, EventArgs e)
    {
        try
        {
            if (Fpspreadpopdept.Sheets[0].Rows.Count > 0 && (chkdeptpriority.Checked || chkdeptpriority1.Checked))
            {
                for (int i = 0; i < Fpspreadpopdept.Sheets[0].Rows.Count; i++)
                {
                    Fpspreadpopdept.Sheets[0].Cells[i, 3].Locked = false;
                    Fpspreadpopdept.Sheets[0].Cells[i, 3].Value = 0;
                    Fpspreadpopdept.Sheets[0].Cells[i, 4].Text = "";
                    Fpspreadpopdept.Sheets[0].Cells[i, 5].Locked = false;
                    Fpspreadpopdept.Sheets[0].Cells[i, 5].Value = 0;
                    Fpspreadpopdept.Sheets[0].Cells[i, 6].Text = "";
                    string dept_Code = Convert.ToString(Fpspreadpopdept.Sheets[0].Cells[i, 2].Tag);
                    if (chkdeptpriority.Checked == true)
                    {
                        int insup = d2.update_method_wo_parameter("update hrdept_master set priority = NULL where dept_code='" + dept_Code + "'  and college_code='" + popcol + "'", "Text");
                    }
                    if (chkdeptpriority1.Checked == true)
                    {
                        int insup = d2.update_method_wo_parameter("update hrdept_master set Priority1=NULL where dept_code='" + dept_Code + "'  and college_code='" + popcol + "'", "Text");
                    }
                }
            }
            Fpspreadpopdept.SaveChanges();
        }
        catch { }
    }

    protected void btnexitdept_click(object sender, EventArgs e)
    {
        popdept.Visible = false;
    }

    protected void btnpopdeptgo_click(object sender, EventArgs e)
    {
        try
        {
            if (chkdeptpriority.Checked == false && chkdeptpriority1.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please select any one Priority!";
                Fpspreadpopdept.Visible = false;
                divdept.Visible = false;
                btnsetdeptpriority.Visible = false;
                btnresetdeptpriority.Visible = false;
                btnexitdept.Visible = false;
                return;
            }
            else
            {
                string deptcode = "";
                if (txtdept.Text.Trim() != "--Select--")
                {
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int cbd = 0; cbd < cbl_dept.Items.Count; cbd++)
                        {
                            if (cbl_dept.Items[cbd].Selected == true)
                            {
                                if (deptcode.Trim() == "")
                                {
                                    deptcode = Convert.ToString(cbl_dept.Items[cbd].Value);
                                }
                                else
                                {
                                    deptcode = deptcode + "'" + "," + "'" + Convert.ToString(cbl_dept.Items[cbd].Value);
                                }
                            }
                        }
                    }
                }
                bool Newcheckflag = false;
                if (cb_dept.Checked == true)
                {
                    Newcheckflag = true;
                }
                string seldept = "select dept_code,dept_acronym,dept_name,priority,Priority1 from hrdept_master where dept_code in('" + deptcode + "') and college_code = '" + popcol + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(seldept, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspreadpopdept.Sheets[0].RowCount = 0;
                        Fpspreadpopdept.Sheets[0].ColumnCount = 0;
                        Fpspreadpopdept.CommandBar.Visible = false;
                        Fpspreadpopdept.Sheets[0].AutoPostBack = false;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspreadpopdept.Sheets[0].RowHeader.Visible = false;
                        Fpspreadpopdept.Sheets[0].ColumnCount = 7;

                        FarPoint.Web.Spread.CheckBoxCellType cbdeptpriority = new FarPoint.Web.Spread.CheckBoxCellType();
                        cbdeptpriority.AutoPostBack = true;
                        FarPoint.Web.Spread.CheckBoxCellType cbdeptpriority1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        cbdeptpriority1.AutoPostBack = true;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.Font.Name = "Book Antiqua";
                        darkstyle.Font.Size = FontUnit.Medium;
                        darkstyle.Font.Bold = true;
                        darkstyle.Border.BorderSize = 1;
                        darkstyle.HorizontalAlign = HorizontalAlign.Center;
                        darkstyle.VerticalAlign = VerticalAlign.Middle;
                        darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                        Fpspreadpopdept.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Columns[0].Locked = true;
                        Fpspreadpopdept.Columns[0].Width = 30;

                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department Acronym";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Columns[1].Locked = true;
                        Fpspreadpopdept.Columns[1].Width = 50;

                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department Name";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Columns[2].Locked = true;
                        Fpspreadpopdept.Columns[2].Width = 50;

                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Set Priority";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Columns[3].Locked = true;

                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Columns[4].Locked = true;

                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Set Priority1";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Columns[5].Locked = true;

                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Priority1";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreadpopdept.Sheets[0].ColumnHeader.Columns[6].Locked = true;

                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspreadpopdept.Sheets[0].RowCount++;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["dept_acronym"]);
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["dept_name"]);
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["dept_code"]);
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 3].CellType = cbdeptpriority;
                            if (chkdeptpriority.Checked == true)
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[row]["priority"]).Trim() != "")
                                {
                                    Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 3].Value = 1;
                                    Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 3].Locked = true;
                                }
                                else
                                {
                                    Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 3].Value = 0;
                                    Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 3].Locked = false;
                                }
                            }

                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["priority"]);
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 5].CellType = cbdeptpriority1;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            if (chkdeptpriority1.Checked == true)
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[row]["Priority1"]).Trim() != "")
                                {
                                    Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 5].Value = 1;
                                    Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 5].Locked = true;
                                }
                                else
                                {
                                    Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 5].Value = 0;
                                    Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 5].Locked = false;
                                }
                            }

                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Priority1"]);
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspreadpopdept.Sheets[0].Cells[Fpspreadpopdept.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        }
                        Fpspreadpopdept.Visible = true;
                        divdept.Visible = true;
                        btnsetdeptpriority.Visible = true;
                        btnresetdeptpriority.Visible = true;
                        btnexitdept.Visible = true;
                        Fpspreadpopdept.Sheets[0].PageSize = Fpspreadpopdept.Sheets[0].RowCount;
                        if (chkdeptpriority.Checked == true && chkdeptpriority1.Checked == true)
                        {
                            Fpspreadpopdept.Sheets[0].Columns[0].Width = 50;
                            Fpspreadpopdept.Sheets[0].Columns[0].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[1].Width = 100;
                            Fpspreadpopdept.Sheets[0].Columns[1].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[2].Width = 190;
                            Fpspreadpopdept.Sheets[0].Columns[2].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[3].Width = 70;
                            Fpspreadpopdept.Sheets[0].Columns[4].Width = 95;
                            Fpspreadpopdept.Sheets[0].Columns[4].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[5].Width = 70;
                            Fpspreadpopdept.Sheets[0].Columns[6].Width = 95;
                            Fpspreadpopdept.Sheets[0].Columns[6].Locked = true;
                            Fpspreadpopdept.Width = 650;
                            Fpspreadpopdept.Height = 325;
                        }
                        else if (chkdeptpriority.Checked == true && chkdeptpriority1.Checked == false)
                        {
                            Fpspreadpopdept.Sheets[0].Columns[0].Width = 75;
                            Fpspreadpopdept.Sheets[0].Columns[0].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[1].Width = 170;
                            Fpspreadpopdept.Sheets[0].Columns[1].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[2].Width = 260;
                            Fpspreadpopdept.Sheets[0].Columns[2].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[3].Width = 70;
                            Fpspreadpopdept.Sheets[0].Columns[4].Width = 95;
                            Fpspreadpopdept.Sheets[0].Columns[4].Locked = true;
                            Fpspreadpopdept.Width = 650;
                            Fpspreadpopdept.Height = 325;
                            Fpspreadpopdept.Sheets[0].Columns[5].Visible = false;
                            Fpspreadpopdept.Sheets[0].Columns[6].Visible = false;
                        }
                        else if (chkdeptpriority.Checked == false && chkdeptpriority1.Checked == true)
                        {
                            Fpspreadpopdept.Sheets[0].Columns[0].Width = 75;
                            Fpspreadpopdept.Sheets[0].Columns[0].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[1].Width = 170;
                            Fpspreadpopdept.Sheets[0].Columns[1].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[2].Width = 260;
                            Fpspreadpopdept.Sheets[0].Columns[2].Locked = true;
                            Fpspreadpopdept.Sheets[0].Columns[5].Width = 70;
                            Fpspreadpopdept.Sheets[0].Columns[6].Width = 95;
                            Fpspreadpopdept.Sheets[0].Columns[6].Locked = true;
                            Fpspreadpopdept.Width = 650;
                            Fpspreadpopdept.Height = 325;
                            Fpspreadpopdept.Sheets[0].Columns[3].Visible = false;
                            Fpspreadpopdept.Sheets[0].Columns[4].Visible = false;
                        }
                        if (Newcheckflag == true)
                        {
                            DptPriorityDiv.Visible = true;
                        }
                        else
                        {
                            DptPriorityDiv.Visible = false;
                        }
                    }
                    else
                    {
                        Fpspreadpopdept.Visible = false;
                        divdept.Visible = false;
                    }
                }
                else
                {
                    Fpspreadpopdept.Visible = false;
                    divdept.Visible = false;
                }
                Fpspreadpopdept.SaveChanges();
            }
        }
        catch { }
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            if (collegecode.Trim() != null)
            {
                string strmcode = "";
                int count = 0;
                for (int i = 0; i < cbl_stream.Items.Count; i++)
                {
                    if (cbl_stream.Items[i].Selected == true)
                    {
                        if (strmcode == "")
                        {
                            strmcode = "" + cbl_stream.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            strmcode = strmcode + "'" + "," + "'" + cbl_stream.Items[i].Value.ToString() + "";
                        }
                    }
                    if (txt_stream.Text.Trim() == cbl_stream.Items[i].Text.ToString())
                    {
                        count = count + 1;
                    }
                }
                bool checkflage = false;
                string selquery = "";
                if (txt_desname.Text.Trim() != "")
                {
                    selquery = " Select desig_code,priority,desig_acronym,desig_name,staffcategory,dept_code, collegeCode from desig_master where desig_name='" + Convert.ToString(txt_desname.Text) + "' and  collegeCode='" + maincol + "'";
                    checkflage = true;
                }
                else
                {
                    selquery = "Select desig_code,desig_acronym,desig_name,priority,staffcategory,collegeCode,dept_code from desig_master where  collegeCode='" + maincol + "' ";

                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selquery, "Text");

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = true;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = 6;

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
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[0].Width = 30;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Designation Acronym";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[1].Width = 50;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation Name";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[2].Width = 50;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Type";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Set Priority";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Priority";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;


                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_acronym"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["dept_code"]);


                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["staffcategory"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = cbpriority;
                            if (Convert.ToString(ds.Tables[0].Rows[row]["priority"]).Trim() != "")
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Value = 1;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Locked = true;
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Value = 0;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Locked = false;
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["priority"]);

                            string selq = "select TextVal from TextValTable where TextCriteria='Stype' and TextVal='" + Convert.ToString(ds.Tables[0].Rows[row]["staffcategory"]) + "' and college_code='" + maincol + "'";
                            DataSet dsgetstaff = new DataSet();
                            dsgetstaff.Clear();
                            dsgetstaff = d2.select_method_wo_parameter(selq, "Text");
                            if (dsgetstaff.Tables.Count > 0)
                            {
                                if (dsgetstaff.Tables[0].Rows.Count > 0)
                                {
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dsgetstaff.Tables[0].Rows[0]["TextVal"]);
                                }
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        }
                        Fpspread1.Visible = true;
                        rportprint.Visible = true;
                        div1.Visible = true;
                        txt_desname.Text = "";
                        lblerrgo.Visible = false;
                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        if (checkflage == true)
                        {
                            PriorityDiv.Visible = false;
                        }
                        else
                        {
                            PriorityDiv.Visible = true;
                        }

                        if (chkpriority.Checked == true)
                        {
                            Fpspread1.Sheets[0].AutoPostBack = false;
                            Fpspread1.Sheets[0].Columns[4].Visible = true;
                            Fpspread1.Sheets[0].Columns[0].Width = 50;
                            Fpspread1.Sheets[0].Columns[0].Locked = true;
                            Fpspread1.Sheets[0].Columns[1].Width = 100;
                            Fpspread1.Sheets[0].Columns[1].Locked = true;
                            Fpspread1.Sheets[0].Columns[2].Width = 200;
                            Fpspread1.Sheets[0].Columns[2].Locked = true;
                            Fpspread1.Sheets[0].Columns[3].Width = 150;
                            Fpspread1.Sheets[0].Columns[3].Locked = true;
                            Fpspread1.Sheets[0].Columns[4].Width = 100;
                            Fpspread1.Sheets[0].Columns[5].Width = 100;
                            Fpspread1.Sheets[0].Columns[5].Locked = true;
                            Fpspread1.Width = 750;
                            Fpspread1.Height = 300;
                            if (Fpspread1.Sheets[0].RowCount > 0)
                            {
                                btnsetpriority.Visible = true;
                                btnresetpriority.Visible = true;
                            }
                            else
                            {
                                btnsetpriority.Visible = false;
                                btnresetpriority.Visible = false;
                            }
                        }
                        else
                        {
                            Fpspread1.Sheets[0].AutoPostBack = true;
                            Fpspread1.Sheets[0].Columns[4].Visible = false;
                            btnsetpriority.Visible = false;
                            btnresetpriority.Visible = false;
                        }
                    }
                    else
                    {
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        rportprint.Visible = false;
                        txt_desname.Text = "";
                        lblerrgo.Visible = true;
                        lblerrgo.Text = "No Record Found";
                    }
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rportprint.Visible = false;
                    txt_desname.Text = "";
                    lblerrgo.Visible = true;
                    lblerrgo.Text = "Please select any one Stream";
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, maincol, "Designation_Master_Alter.aspx");
        }
    }

    public void loaddeptspread()
    {
        try
        {
            FarPoint.Web.Spread.StyleInfo darknewstyle = new FarPoint.Web.Spread.StyleInfo();
            darknewstyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
            darknewstyle.ForeColor = System.Drawing.Color.Black;
            darknewstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darknewstyle.ForeColor = Color.Black;
            darknewstyle.HorizontalAlign = HorizontalAlign.Center;
            Fpspreaddept.ActiveSheetView.ColumnHeader.DefaultStyle = darknewstyle;
            Fpspreaddept.Sheets[0].ColumnCount = 4;
            Fpspreaddept.Sheets[0].RowCount = 0;
            Fpspreaddept.Sheets[0].RowHeader.Visible = false;
            Fpspreaddept.CommandBar.Visible = false;
            Fpspreaddept.Sheets[0].AutoPostBack = false;

            FarPoint.Web.Spread.CheckBoxCellType chksel = new FarPoint.Web.Spread.CheckBoxCellType();
            chksel.AutoPostBack = false;
            string type = ddl_streamlst.SelectedItem.Text.ToString();

            string selqdept = "select Dept_Code,Dept_name from hrdept_master where college_code ='" + newcol + "'";

            if (type.Trim() != "" && type.Trim() != "All")
            {
                selqdept = selqdept + " and c.type ='" + type + "'";
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selqdept, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.Black;
                    Fpspreaddept.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreaddept.Columns[0].Width = 75;
                    Fpspreaddept.Columns[0].Locked = true;

                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department Code";
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreaddept.Columns[1].Width = 100;
                    Fpspreaddept.Columns[1].Locked = true;

                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department Name";
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreaddept.Columns[2].Width = 300;
                    Fpspreaddept.Columns[2].Locked = true;

                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspreaddept.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreaddept.Columns[3].Width = 100;

                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                    cb.AutoPostBack = true;
                    FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();

                    Fpspreaddept.Sheets[0].RowCount++;
                    Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 3].CellType = cb;
                    Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspreaddept.Sheets[0].RowCount++;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Code"]);
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Dept_name"]);
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 3].CellType = chksel;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspreaddept.Sheets[0].Cells[Fpspreaddept.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    }
                    Fpspreaddept.Visible = true;
                    Fpspreaddept.Width = 600;
                    Fpspreaddept.Height = 200;
                    Fpspreaddept.Sheets[0].PageSize = Fpspreaddept.Sheets[0].RowCount;
                }
            }
        }
        catch { }
    }

    protected void Fpspreaddept_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string value = Convert.ToString(Fpspreaddept.Sheets[0].Cells[0, 3].Value);
        if (value == "1")
        {
            for (int K = 1; K < Fpspreaddept.Sheets[0].Rows.Count; K++)
            {
                Fpspreaddept.Sheets[0].Cells[K, 3].Value = 1;
            }
        }
        else
        {
            for (int K = 1; K < Fpspreaddept.Sheets[0].Rows.Count; K++)
            {
                Fpspreaddept.Sheets[0].Cells[K, 3].Value = 0;
            }
        }
    }

    public bool checkedOK()
    {
        bool Ok = false;
        Fpspreaddept.SaveChanges();
        for (int i = 1; i < Fpspreaddept.Sheets[0].Rows.Count; i++)
        {
            string check = Convert.ToString(Fpspreaddept.Sheets[0].Cells[i, 3].Value);
            if (check == "1")
            {
                Ok = true;
            }
        }
        return Ok;
    }

    protected void Fpspreaddept_OnUpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }

    protected void Cellpopdept_Click(object sender, EventArgs e)
    {
        spreaddeptclick = true;
    }

    protected void Fpspreadpopdept_buttoncommand(object sender, EventArgs e)
    {
        try
        {
            Fpspreadpopdept.SaveChanges();
            string activerow = Fpspreadpopdept.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspreadpopdept.ActiveSheetView.ActiveColumn.ToString();
            if (activecol == "3")
            {
                int act1 = Convert.ToInt32(activerow);
                int act2 = Convert.ToInt16(activecol);
                if (Fpspreadpopdept.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
                {
                    flagdept = true;
                    Fpspreadpopdept.Sheets[0].Cells[act1, act2 + 1].Text = "";
                }
                else
                {
                    flagdept = false;
                }
            }
            if (activecol == "5")
            {
                int act1 = Convert.ToInt32(activerow);
                int act2 = Convert.ToInt16(activecol);
                if (Fpspreadpopdept.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
                {
                    flagdept = true;
                    Fpspreadpopdept.Sheets[0].Cells[act1, act2 + 1].Text = "";
                }
                else
                {
                    flagdept = false;
                }
            }
            Fpspreadpopdept.SaveChanges();
        }
        catch { }
    }

    protected void Fpspreadpopdept_render(object sender, EventArgs e)
    {
        if (flagdept == true)
        {
            Fpspreadpopdept.SaveChanges();
            string activrow = "";
            activrow = Fpspreadpopdept.Sheets[0].ActiveRow.ToString();
            string activecol = Fpspreadpopdept.Sheets[0].ActiveColumn.ToString();
            int actcol = Convert.ToInt16(activecol);
            int hy_order = 0;
            for (int i = 0; i <= Convert.ToInt16(Fpspreadpopdept.Sheets[0].RowCount) - 1; i++)
            {
                int isval = Convert.ToInt32(Fpspreadpopdept.Sheets[0].Cells[i, actcol].Value);
                if (isval == 1)
                {
                    hy_order++;
                    Fpspreadpopdept.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;
                }
            }
            Fpspreadpopdept.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = hy_order.ToString();
        }
    }

    protected void Cellcont_Click(object sender, EventArgs e)
    {
        spreadclick = true;
    }

    protected void Fpspread1_buttoncommand(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            if (activecol == "4")
            {
                int act1 = Convert.ToInt32(activerow);
                int act2 = Convert.ToInt16(activecol);
                if (Fpspread1.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
                {
                    flag_true = true;
                    Fpspread1.Sheets[0].Cells[act1, act2 + 1].Text = "";
                }
                else
                {
                    flag_true = false;
                }
            }
            Fpspread1.SaveChanges();
        }
        catch { }
    }

    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (flag_true == true)
            {
                Fpspread1.SaveChanges();
                string activrow = "";
                activrow = Fpspread1.Sheets[0].ActiveRow.ToString();
                string activecol = Fpspread1.Sheets[0].ActiveColumn.ToString();
                int actcol = Convert.ToInt16(activecol);
                int hy_order = 0;
                for (int i = 0; i <= Convert.ToInt16(Fpspread1.Sheets[0].RowCount) - 1; i++)
                {
                    int isval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, actcol].Value);
                    if (isval == 1)
                    {
                        hy_order++;
                        Fpspread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;
                    }
                }
                Fpspread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = hy_order.ToString();
            }
            else
            {
                if (spreadclick == true)
                {
                    ddlnewcol.SelectedIndex = ddlnewcol.Items.IndexOf(ddlnewcol.Items.FindByValue(ddl_col.SelectedItem.Value));
                    newcol = Convert.ToString(ddlnewcol.SelectedItem.Value);
                    autocol = Convert.ToString(ddlnewcol.SelectedItem.Value);
                    ddlnewcol.Enabled = false;
                    loadstream();
                    loadstreamlst();
                    bindaddreason();
                    loaddeptspread();
                    poppernew.Visible = true;
                    btndel.Visible = true;
                    btn_update.Visible = true;
                    btn_save.Visible = false;
                    int count = 0;
                    string actrow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                    string actcol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                    if (actrow.Trim() != "")
                    {
                        string desigacr = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
                        string designame = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                        string desigid = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                        string stafftype = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
                        string streamcount = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].Tag);
                        string dept = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
                        string[] semicol1 = dept.Split(';');
                        if (semicol1.Length >= 0)
                        {
                            for (int dpt = 0; dpt < semicol1.Length; dpt++)
                            {
                                string dept_code = Convert.ToString(semicol1[dpt]);
                                for (int spdpt = 0; spdpt < Fpspreaddept.Sheets[0].Rows.Count; spdpt++)
                                {
                                    if (Convert.ToString(Fpspreaddept.Sheets[0].Cells[Convert.ToInt32(spdpt), 1].Text) == Convert.ToString(dept_code))
                                    {
                                        Fpspreaddept.Sheets[0].Cells[Convert.ToInt32(spdpt), 3].Value = 1;
                                        count++;
                                    }
                                }
                            }
                            if (count == Fpspreaddept.Sheets[0].Rows.Count - 1)
                            {
                                Fpspreaddept.Sheets[0].Cells[0, 3].Value = 1;
                            }
                            else
                            {
                                Fpspreaddept.Sheets[0].Cells[0, 3].Value = 0;
                            }
                        }

                        txt_desigacr.Text = Convert.ToString(desigacr);
                        txt_designame.Text = Convert.ToString(designame);
                        txtdescode.Text = Convert.ToString(desigid);

                        loadstreamlst();
                        ddl_streamlst.SelectedIndex = ddl_streamlst.Items.IndexOf(ddl_streamlst.Items.FindByValue(streamcount));
                        bindaddreason();
                        ddlstaftyp.SelectedIndex = ddlstaftyp.Items.IndexOf(ddlstaftyp.Items.FindByText(stafftype));
                    }
                }
            }
        }
        catch { }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch { }
        txtexcelname.Text = "";
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Designation Master";
            string pagename = "Designation_Master_Alter.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch { }
    }

    protected void btnaddnew_click(object sender, EventArgs e)
    {
        try
        {
            newcol = Convert.ToString(ddlnewcol.SelectedItem.Value);
            autocol = Convert.ToString(ddlnewcol.SelectedItem.Value);
            ddlnewcol.Enabled = true;
            loaddeptspread();
            poppernew.Visible = true;
            clear();
            btn_save.Visible = true;
            btn_update.Visible = false;
            btndel.Visible = false;

            if (ddl_streamlst.Items.Count == 0)
            {
                ddl_streamlst.Enabled = false;
            }
            if (Fpspreaddept.Sheets[0].Rows.Count > 0)
            {
                for (int k = 0; k < Fpspreaddept.Sheets[0].Rows.Count; k++)
                {
                    Fpspreaddept.Sheets[0].Cells[Convert.ToInt32(k), 3].Value = 0;
                }
            }
            string getacr = "select GeneralAcr,StartNo,SerialSize,SettingValues from HRS_CodeSettings where SettingField='3' and CollegeCode='" + newcol + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(getacr, "Text");
            string getexist = d2.GetFunction("select value from Master_Settings where settings='CodeSetting Rights' and value is not null and value<>''");
            string desigcod = getcatcode(getexist, ds);
            txtdescode.Text = desigcod;
        }
        catch { }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
        if (Fpspreaddept.Sheets[0].Rows.Count > 0)
        {
            for (int k = 0; k < Fpspreaddept.Sheets[0].Rows.Count; k++)
            {
                Fpspreaddept.Sheets[0].Cells[Convert.ToInt32(k), 3].Value = 0;
            }
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkedOK())
            {
                string actrow = Fpspreaddept.ActiveSheetView.ActiveRow.ToString();
                string actcol = Fpspreaddept.ActiveSheetView.ActiveColumn.ToString();
                int actcolumn = Convert.ToInt32(actcol);
                string desigacr = Convert.ToString(txt_desigacr.Text);
                string designame = Convert.ToString(txt_designame.Text);
                string stafftype = Convert.ToString(ddlstaftyp.SelectedItem.Text);
                stafftype = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(stafftype);
                string staffacr = Convert.ToString(txt_staffacr.Text);
                designame = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(designame);
                string deptfk = "";
                string desigcod = Convert.ToString(txtdescode.Text);
                string insquery = "";
                if (ddlstaftyp.SelectedItem.Text == "Select" && ddlstaftyp.SelectedIndex == 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Select Staff Type!";
                    return;
                }
                for (int K = 1; K < Fpspreaddept.Sheets[0].Rows.Count; K++)
                {
                    string check = Convert.ToString(Fpspreaddept.Sheets[0].Cells[K, 3].Value);
                    if (check == "1")
                    {
                        if (Convert.ToInt32(actcolumn) > 0)
                        {
                            if (Convert.ToInt32(Fpspreaddept.Sheets[0].Cells[Convert.ToInt32(K), Convert.ToInt32(actcol)].Value) == 1)
                            {
                                if (deptfk == "")
                                {
                                    deptfk = Convert.ToString(Fpspreaddept.Sheets[0].Cells[Convert.ToInt32(K), 1].Text);
                                }
                                else
                                {
                                    deptfk = deptfk + ";" + Convert.ToString(Fpspreaddept.Sheets[0].Cells[Convert.ToInt32(K), 1].Text);
                                }
                            }
                        }
                    }
                }
                if (String.IsNullOrEmpty(deptfk))
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Select any one Department!";
                    return;
                }
                if (!String.IsNullOrEmpty(desigcod) && desigcod.Trim() != "0")
                {
                    insquery = "Insert into desig_master (desig_acronym,desig_name,staffcategory,desig_code,collegeCode,dept_code) values ('" + desigacr.ToUpper() + "','" + designame + "','" + stafftype + "','" + desigcod + "','" + newcol + "','" + deptfk + "')";

                    int newinscount = d2.update_method_wo_parameter(insquery, "Text");
                    if (newinscount > 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully";
                        clear();
                        loadstreamlst();
                        poppernew.Visible = false;
                        btngo_click(sender, e);
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Set Code Settings First!";
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select any one Department!";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, newcol, "Designation_Master_Alter.aspx");
        }
    }

    public bool checkedOKup()
    {
        bool Ok = false;
        Fpspreaddept.SaveChanges();
        for (int i = 1; i < Fpspreaddept.Sheets[0].Rows.Count; i++)
        {
            string check = Convert.ToString(Fpspreaddept.Sheets[0].Cells[i, 3].Value);
            if (check == "1")
            {
                Ok = true;
            }
        }
        return Ok;
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();

            string actrow = Fpspreaddept.ActiveSheetView.ActiveRow.ToString();
            string actcol = Fpspreaddept.ActiveSheetView.ActiveColumn.ToString();
            int actcolumn = Convert.ToInt32(actcol);
            string deptfk = "";
            string upquery = "";
            int upcount = 0;

            if (checkedOKup())
            {
                string desigid = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                string desigacr = Convert.ToString(txt_desigacr.Text);
                string designame = Convert.ToString(txt_designame.Text);
                string stafftype = Convert.ToString(ddlstaftyp.SelectedItem.Text);
                stafftype = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(stafftype);
                string staffacr = Convert.ToString(txt_staffacr.Text);
                designame = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(designame);

                if (stafftype == "Select")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Select Staff Type";
                    return;
                }

                for (int i = 0; i < Fpspreaddept.Sheets[0].Rows.Count; i++)
                {
                    string check = Convert.ToString(Fpspreaddept.Sheets[0].Cells[i, 3].Value);
                    if (check == "1")
                    {

                        if (deptfk == "")
                        {
                            deptfk = Convert.ToString(Fpspreaddept.Sheets[0].Cells[i, 1].Text);
                        }
                        else
                        {
                            deptfk = deptfk + ";" + Convert.ToString(Fpspreaddept.Sheets[0].Cells[i, 1].Text);
                        }
                    }
                }
                if (activerow.Trim() != "")
                {
                    upquery = "if exists (select * from desig_master where desig_code ='" + desigid + "' and collegeCode='" + newcol + "') update desig_master set desig_name='" + designame + "',desig_acronym='" + desigacr.ToUpper() + "',staffcategory='" + stafftype + "',dept_code='" + deptfk + "'  where desig_code ='" + desigid + "'  and collegeCode='" + newcol + "' else Insert Into desig_master(desig_name,desig_acronym,staffcategory,collegeCode,dept_code) values('" + designame + "','" + desigacr.ToUpper() + "','" + stafftype + "','" + newcol + "','" + deptfk + "' )";
                    upcount = d2.update_method_wo_parameter(upquery, "Text");

                    if (upcount > 0)
                    {
                        alertpopwindow.Visible = true;
                        btngo_click(sender, e);
                        loadstream();
                        bindcollege();
                        lblalerterr.Text = "Updated Successfully";
                        poppernew.Visible = false;
                    }
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Department";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, newcol, "Designation_Master_Alter.aspx");
        }
    }

    protected void btndel_Click(object sender, EventArgs e)
    {
        imgDivdel.Visible = true;
        lblconfirmdel.Visible = true;
        lblconfirmdel.Text = "Do you want to delete this record?";
    }

    protected void btnyesdel_Click(object sender, EventArgs e)
    {
        try
        {
            string actrow = Convert.ToString(Fpspread1.ActiveSheetView.ActiveRow);
            string actcol = Convert.ToString(Fpspread1.ActiveSheetView.ActiveColumn);
            string clgcode = Convert.ToString(ddl_col.SelectedItem.Value);
            string desigid = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);

            if (actrow.Trim() != "")
            {
                string query2 = "delete from desig_master where desig_code='" + desigid + "' and collegeCode='" + newcol + "'";
                int iv = d2.update_method_wo_parameter(query2, "Text");
                if (iv != 0)
                {
                    alertpopwindow.Visible = true;
                    btngo_click(sender, e);
                    loadstream();
                    bindcollege();
                    lblalerterr.Text = "Deleted Successfully";
                    imgDivdel.Visible = false;
                    lblconfirmdel.Visible = false;
                    poppernew.Visible = false;
                }
            }
        }
        catch { }
    }

    protected void btnnodel_Click(object sender, EventArgs e)
    {
        imgDivdel.Visible = false;
        lblconfirmdel.Visible = false;
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
        if (Fpspreaddept.Sheets[0].Rows.Count > 0)
        {
            for (int k = 0; k < Fpspreaddept.Sheets[0].Rows.Count; k++)
            {
                Fpspreaddept.Sheets[0].Cells[Convert.ToInt32(k), 3].Value = 0;
            }
        }
    }

    public void loadstream()
    {
        try
        {
            cbl_stream.Items.Clear();
            string deptquery = " select distinct type from Course where type<>'' and type is not null and college_code  in ('" + newcol + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_stream.DataSource = ds;
                    cbl_stream.DataTextField = "type";
                    cbl_stream.DataBind();
                    ddl_streamlst.Items.Insert(0, "All");
                    if (cbl_stream.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_stream.Items.Count; i++)
                        {
                            cbl_stream.Items[i].Selected = true;
                        }
                        txt_stream.Text = "Stream(" + cbl_stream.Items.Count + ")";
                        cb_stream.Checked = true;
                    }
                }
            }
            else
            {
                txt_stream.Text = "--Select--";
            }
        }
        catch { }
    }

    public void loadstreamlst()
    {
        try
        {
            ddl_streamlst.Items.Clear();
            string deptquery = " select distinct type from Course where type<>'' and type is not null and college_code in ('" + newcol + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_streamlst.DataSource = ds;
                ddl_streamlst.DataTextField = "type";
                ddl_streamlst.DataBind();
                ddl_streamlst.Items.Insert(0, "All");
            }
            else
            {
                ddl_streamlst.Items.Insert(0, "All");
            }
        }
        catch { }
    }

    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddl_col.Items.Clear();
            ddlcoldept.Items.Clear();
            ddlnewcol.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_col.DataSource = ds;
                ddl_col.DataTextField = "collname";
                ddl_col.DataValueField = "college_code";
                ddl_col.DataBind();

                ddlcoldept.DataSource = ds;
                ddlcoldept.DataTextField = "collname";
                ddlcoldept.DataValueField = "college_code";
                ddlcoldept.DataBind();

                ddlnewcol.DataSource = ds;
                ddlnewcol.DataTextField = "collname";
                ddlnewcol.DataValueField = "college_code";
                ddlnewcol.DataBind();
            }
        }
        catch { }
    }

    protected void bindaddreason()
    {
        try
        {
            ddlstaftyp.Items.Clear();
            ds.Clear();
            string sql = "select distinct TextCode,TextVal from TextValTable where TextCriteria ='Stype' and college_code ='" + newcol + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstaftyp.DataSource = ds;
                ddlstaftyp.DataTextField = "TextVal";
                ddlstaftyp.DataValueField = "TextCode";
                ddlstaftyp.DataBind();
                ddlstaftyp.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlstaftyp.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch { }
    }

    public void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string item = "select dept_code,dept_name from hrdept_master where college_code = '" + popcol + "'";
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
            }
            else
            {
                txtdept.Text = "--Select--";
                cb_dept.Checked = false;
            }
        }
        catch { }
    }

    public void clear()
    {
        txt_desigacr.Text = "";
        txt_designame.Text = "";
        ddlstaftyp.SelectedIndex = 0;
        txt_staffacr.Text = "";
    }

    private string getcatcode(string setting, DataSet dsacr)
    {
        DataSet dnew = new DataSet();
        string[] aplval = new string[5];
        string[] splval = new string[5];
        string code = "";
        string catacr = "";
        string getdsval = "";
        int startno = 0;
        int size = 0;
        string getval = "";
        try
        {
            if (setting.Trim() != "0" && setting.Trim() != "" && setting.Trim() != null && dsacr.Tables.Count > 0 && dsacr.Tables[0].Rows.Count > 0)
            {
                aplval = setting.Split(',');
                if (aplval.Contains("3"))
                {
                    getdsval = Convert.ToString(dsacr.Tables[0].Rows[0]["SettingValues"]);
                    if (getdsval.Trim() != "")
                    {
                        splval = getdsval.Split(';');
                        if (splval.Length > 0)
                        {
                            for (int ik = 0; ik < splval.Length; ik++)
                            {
                                if (splval[ik].Trim() == "1")
                                {
                                    string getcolacr = d2.GetFunction("select Coll_acronymn from collinfo where college_code='" + newcol + "'");
                                    if (getcolacr.Trim() != "0" && getcolacr.Trim() != "" && getcolacr.Trim() != null)
                                    {
                                        catacr = catacr + getcolacr;
                                    }
                                }
                                if (splval[ik].Trim() == "3")
                                {
                                    catacr = catacr + Convert.ToString(dsacr.Tables[0].Rows[0]["GeneralAcr"]).ToUpper();
                                }
                            }
                        }

                        Int32.TryParse(Convert.ToString(dsacr.Tables[0].Rows[0]["StartNo"]), out startno);
                        Int32.TryParse(Convert.ToString(dsacr.Tables[0].Rows[0]["SerialSize"]), out size);
                        int startlen = Convert.ToString(startno).Trim().Length;
                        int totsize = size - startlen;
                      //  string selectquery = "select desig_code from desig_master where desig_code like '" + catacr + "%' and collegeCode='" + newcol + "' order by CAST(desig_code as numeric) desc";//order by LEN(desig_code),desig_code 17.11.17 

                        string selectquery = "select desig_code from desig_master where desig_code like '" + catacr + "%' and collegeCode='" + newcol + "'";


                        dnew = d2.select_method_wo_parameter(selectquery, "Text");
                        if (dnew.Tables[0].Rows.Count > 0)
                        {
                            string concadnew = Convert.ToString(dnew.Tables[0].Rows[dnew.Tables[0].Rows.Count - 1][0]);
                            string concad = "";
                            for (int i = 0; i < catacr.Length; i++)
                            {
                                char a = concadnew[i];
                                concad = concad + a;
                            }
                            string input = concadnew;
                            string[] stringSeparators = new string[] { concad };

                            var result = concadnew.Split(stringSeparators, StringSplitOptions.None);
                            string catcode = result[1];
                            int catcode1 = Convert.ToInt32(catcode);
                            catcode1 = catcode1 + 1;
                            getval = Convert.ToString(catcode1);
                            for (int ik = 0; ik < totsize; ik++)
                            {
                                getval = Convert.ToString("0") + getval;
                            }
                            code = concad + Convert.ToString(getval);
                        }
                        else
                        {
                            for (int ik = 0; ik < totsize; ik++)
                            {
                                getval = getval + Convert.ToString("0");
                            }
                            code = Convert.ToString(catacr) + getval + Convert.ToString(startno);
                        }
                    }
                }
                else
                {
                    string desigfk = d2.GetFunction("select top 1 desig_code from desig_master where collegeCode='" + newcol + "' ");

                    ds.Clear();

                    if (desigfk.Trim() != "" && desigfk.Trim() != "0")
                    {

                        code = Convert.ToString(Convert.ToInt32(desigfk) + 1);
                        string query = "select desig_code from desig_master where collegeCode='" + newcol + "' ";
                        ds = d2.select_method_wo_parameter(query, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string getCode = Convert.ToString(ds.Tables[0].Rows[i]["desig_code"]);
                                if (getCode == code)
                                {
                                    int codeval = Convert.ToInt32(code) + 1;
                                    code = Convert.ToString(codeval);
                                    i = 0;
                                }

                            }

                        }
                    }
                    else
                    {
                        code = "1";
                    }
                   // string desigfk = d2.GetFunction("select top 1 desig_code from desig_master where collegeCode='" + newcol + "'  order by CAST(desig_code as numeric) desc");//order by desig_code desc 17.11.17 
                    //string desigfk = d2.GetFunction("select top 1 desig_code from desig_master where collegeCode='" + newcol + "' ");


                    //if (desigfk.Trim() != "" && desigfk.Trim() != "0")
                    //    code = Convert.ToString(Convert.ToInt32(desigfk) + 1);
                    //else
                    //    code = "1";
                    //if (desigfk.Length != code.Length)
                    //{
                    //    for (int ik = 0; ik < desigfk.Length; ik++)
                    //    {
                    //        if (desigfk.Length != code.Length)
                    //        {
                    //            code = Convert.ToString("0") + code;
                    //        }
                    //        else
                    //        {
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            else
            {
               // string desigfk = d2.GetFunction("select top 1 desig_code from desig_master where collegeCode='" + newcol + "'  order by CAST(desig_code as numeric) desc");//order by desig_code desc 17.11.17 
                string desigfk=d2.GetFunction("select top 1 desig_code from desig_master where collegeCode='" + newcol + "' ");

                ds.Clear();

                if (desigfk.Trim() != "" && desigfk.Trim() != "0")
                {

                    code = Convert.ToString(Convert.ToInt32(desigfk) + 1);
                    string query = "select desig_code from desig_master where collegeCode='" + newcol + "' ";
                    ds = d2.select_method_wo_parameter(query, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string getCode = Convert.ToString(ds.Tables[0].Rows[i]["desig_code"]);
                            if (getCode == code)
                            {
                                int codeval = Convert.ToInt32(code) + 1;
                                code = Convert.ToString(codeval);
                                i = 0;
                            }
                        
                        }

                    }
                }
                else
                {
                    code = "1";
                }
                    
                //    code = Convert.ToString(Convert.ToInt32(desigfk) + 1);
                //else
                //    code = "1";
                //if (desigfk.Length != code.Length)
                //{
                //    for (int ik = 0; ik < desigfk.Length; ik++)
                //    {
                //        if (desigfk.Length != code.Length)
                //        {
                //            code = Convert.ToString("0") + code;
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //}
            }
        }
        catch { }
        return code;
    }

    private void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
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

    private void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
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
}