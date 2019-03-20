using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.Configuration;

public partial class LibraryMod_UtilizationReport : System.Web.UI.Page
{

    #region Field Declaration

    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;


    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    DataTable dtdept = new DataTable();
    DataSet dsreport = new DataSet();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();

    string collegecode = string.Empty;
    string department = string.Empty;
    string reporttype = string.Empty;
    string library = string.Empty;
    string bookutiliselectqry = string.Empty;
    string qrylibraryFilter = string.Empty;
    string qryaccnofilter = string.Empty;
    string qryaccnofilter1 = string.Empty;
    string qrydatefilter = string.Empty;
    string qrytitlefilter = string.Empty;
    string fromdate = string.Empty;
    string todate = string.Empty;
    string qrycountequalFilter = string.Empty;
    string qrycountgraterFilter = string.Empty;
    string qrycountlessFilter = string.Empty;
    string qrytitlefilter1 = string.Empty;
    string gateutiliselectqry = string.Empty;
    string qryrollstufilter = string.Empty;
    string qryrollstafilter = string.Empty;
    string libraryutiliselectqry = string.Empty;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("LibraryHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/LibraryMod/LibraryHome.aspx");
                    return;
                }
            }

            userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

            if (!IsPostBack)
            {
                Bindcollege();
                getLibPrivil();
                BookDepartment();
                ReportType();
                Count();
                Studstaff();
                Accesstitle();
                showreport1.Visible = false;
                //getPrintSettings();


            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    #region College

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    #endregion

    #region Library

    public void getLibPrivil()
    {
        try
        {
            Hashtable hsLibcode = new Hashtable();
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlCollege.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            if (singleUser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + userCode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupUserCode.Split(';');
                if (groupUser.Length > 0)
                {
                    if (groupUser.Length == 1)
                    {
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code=" + groupUser[0] + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                    if (groupUser.Length > 1)
                    {
                        for (int i = 0; i < groupUser.Length; i++)
                        {
                            GrpUserVal = groupUser[i];
                            if (!GrpCode.Contains(GrpUserVal))
                            {
                                if (GrpCode == "")
                                    GrpCode = GrpUserVal;
                                else
                                    GrpCode = GrpCode + "','" + GrpUserVal;
                            }
                        }
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code in ('" + GrpCode + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                }

            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                libcodecollection = "WHERE lib_code IN (-1)";
                goto aa;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string codeCollection = Convert.ToString(ds.Tables[0].Rows[i]["lib_code"]);
                    if (!hsLibcode.Contains(codeCollection))
                    {
                        hsLibcode.Add(codeCollection, "LibCode");
                        if (libcodecollection == "")
                            libcodecollection = codeCollection;
                        else
                            libcodecollection = libcodecollection + "','" + codeCollection;
                    }
                }
            }
            //libcodecollection = Left(libcodecollection, Len(libcodecollection) - 1);
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;
            Library(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void Library(string libcode)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds.Clear();
                ds = da.select_method_wo_parameter(lib_name, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();
                    ddllibrary.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }


    }

    #endregion

    #region Department

    public void BookDepartment()
    {
        try
        {
            Hashtable hat = new Hashtable();
            cbl_department.Items.Clear();
            cb_department.Checked = false;
            txt_department.Text = "---Select---";
            string College = ddlCollege.SelectedValue.ToString();
            string libName = Convert.ToString(ddllibrary.SelectedItem.Text);
            if (!string.IsNullOrEmpty(College))
            {

                hat.Add("collegecode", College);
                ds.Clear();
                string sql = "SELECT DISTINCT Dept_Name FROM Journal_Dept WHERE College_Code ='" + College + "' ORDER BY Dept_Name ";
                if (libName != "All")
                {

                    sql = sql + "AND Lib_Code ='" + Convert.ToString(ddllibrary.SelectedValue) + "' ORDER BY Dept_Name ";
                }

                ds = da.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_department.DataSource = ds;
                    cbl_department.DataTextField = "Dept_Name";
                    cbl_department.DataValueField = "Dept_Name";
                    cbl_department.DataBind();
                    if (cbl_department.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_department.Items.Count; i++)
                        {

                            cbl_department.Items[i].Selected = true;
                        }
                        txt_department.Text = "Department(" + cbl_department.Items.Count + ")";
                        cb_department.Checked = true;
                    }
                }

            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }

    }

    public void librarygateDepartment()
    {
        try
        {
            ds.Clear();
            cbl_department.Items.Clear();
            string newcollcode = Convert.ToString(ddlCollege.SelectedItem.Value);
            string item = "select dept_code,dept_name from hrdept_master where college_code='" + newcollcode + "' order by dept_name";
            ds.Clear();
            string group_user = "";
            string cmd = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + newcollcode + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + newcollcode + "') order by dept_name";
            }

            ds = d2.select_method_wo_parameter(cmd, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_department.DataSource = ds;
                cbl_department.DataTextField = "dept_name";
                cbl_department.DataValueField = "dept_code";
                cbl_department.DataBind();
                if (cbl_department.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_department.Items.Count; i++)
                    {
                        cbl_department.Items[i].Selected = true;
                    }
                    txt_department.Text = "Department (" + cbl_department.Items.Count + ")";
                    cb_department.Checked = true;
                }
            }
            else
            {
                txt_department.Text = "--Select--";
                cb_department.Checked = false;
            }

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }

    }

    protected void cb_department_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {

            d2.CallCheckboxChange(cb_department, cbl_department, txt_department, "Department", "--Select--");
            showreport1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    protected void cbl_department_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            d2.CallCheckboxListChange(cb_department, cbl_department, txt_department, "Department", "--Select--");
            showreport1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    #endregion

    #region ReprotType

    public void ReportType()
    {
        try
        {
            ddlreporttype.Items.Add("Books Utilization");
            ddlreporttype.Items.Add("Gate Utilization");
            ddlreporttype.Items.Add("Library Utilization");

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }
    #endregion

    #region Count
    public void Count()
    {
        try
        {
            ddlcount.Items.Add("Equal To");
            ddlcount.Items.Add("Greater Than");
            ddlcount.Items.Add("Less Than");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }
    #endregion

    #region RadioStudeStaff

    public void Studstaff()
    {
        try
        {
            rblmembertype.Items.Add("Student");
            rblmembertype.Items.Add("Staff");
            rblmembertype.Items.Add("Both");
            //rblmembertype.Items.FindByText("Student").Selected = true;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }

    }
    #endregion

    #region RadioAccessTitle
    public void Accesstitle()
    {
        try
        {
            rbltype.Items.Add("By AccessNo");
            rbltype.Items.Add("By Title");
            rbltype.Items.FindByText("By AccessNo").Selected = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }
    #endregion

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedItem.Value);
                ddlCollege.SelectedIndex = ddlCollege.Items.IndexOf(ddlCollege.Items.FindByValue(collegecode));
                getLibPrivil();
                BookDepartment();
                showreport1.Visible = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }

    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    protected void cbdate_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;
            if (cbdate.Checked == true)
            {
                txt_fromdate.Enabled = true;
                txt_todate.Enabled = true;

            }
            else
            {

                txt_fromdate.Enabled = false;
                txt_todate.Enabled = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    protected void ddlreporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;

            if (ddlreporttype.SelectedIndex == 0)
            {
                BookDepartment();
                rblmembertype.Enabled = false;
                rbltype.Enabled = true;
                rbltype.Items.FindByText("By AccessNo").Selected = true;
                rblmembertype.Items.FindByText("Student").Selected = false;
                rblmembertype.Items.FindByText("Staff").Selected = false;
                rblmembertype.Items.FindByText("Both").Selected = false;
                lbl_RollNo.Visible = false;
                txtrollno.Visible = false;

                if (rbltype.SelectedIndex == 0)
                {
                    lbl_AccessNo.Visible = true;
                    txtaccessno.Visible = true;
                }
                if (rbltype.SelectedIndex == 1)
                {
                    lbl_Title.Visible = true;
                    txttitle.Visible = true;
                }

            }
            if (ddlreporttype.SelectedIndex == 1)
            {
                librarygateDepartment();
                rblmembertype.Enabled = true;
                rbltype.Enabled = false;
                rbltype.Items.FindByText("By AccessNo").Selected = false;
                rbltype.Items.FindByText("By Title").Selected = false;
                rblmembertype.Items.FindByText("Student").Selected = true;
                lbl_RollNo.Visible = true;
                lbl_Title.Visible = false;
                lbl_AccessNo.Visible = false;
                txtrollno.Visible = true;
                txttitle.Visible = false;
                txtaccessno.Visible = false;


            }
            if (ddlreporttype.SelectedIndex == 2)
            {
                librarygateDepartment();
                rblmembertype.Enabled = true;
                rbltype.Enabled = false;
                rbltype.Items.FindByText("By AccessNo").Selected = false;
                rbltype.Items.FindByText("By Title").Selected = false;
                rblmembertype.Items.FindByText("Student").Selected = true;
                lbl_RollNo.Visible = true;
                lbl_Title.Visible = false;
                lbl_AccessNo.Visible = false;
                txtrollno.Visible = true;
                txttitle.Visible = false;
                txtaccessno.Visible = false;


            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    protected void ddlcount_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }


    protected void rblmembertype_Selected(object sender, EventArgs e)
    {
        try
        {
            txtcount.Text = "";
            txtrollno.Text = "";
            showreport1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    protected void rbltype_Selected(object sender, EventArgs e)
    {
        try
        {
            txtcount.Text = "";
            txtaccessno.Text = "";
            showreport1.Visible = false;
            if (rbltype.SelectedIndex == 0)
            {
                lbl_RollNo.Visible = false;
                lbl_Title.Visible = false;
                lbl_AccessNo.Visible = true;
                txtrollno.Visible = false;
                txttitle.Visible = false;
                txtaccessno.Visible = true;
            }
            if (rbltype.SelectedIndex == 1)
            {
                lbl_RollNo.Visible = false;
                lbl_Title.Visible = true;
                lbl_AccessNo.Visible = false;
                txtrollno.Visible = false;
                txttitle.Visible = true;
                txtaccessno.Visible = false;

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }



    #endregion Index Changed Events

    #region Go

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlreporttype.SelectedIndex == 0)
            {
                dsreport = BookReport();
                if (dsreport.Tables.Count > 0 && dsreport.Tables[0].Rows.Count > 0)
                {
                    loadspreadbookutiliDetails(dsreport);
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found!";
                    showreport1.Visible = false;
                }
            }
            if (ddlreporttype.SelectedIndex == 1)
            {
                dsreport.Clear();
                dsreport = GateReport();
                if (dsreport.Tables.Count > 0 && dsreport.Tables[0].Rows.Count > 0)
                {
                    loadspreadgateutiliDetails(dsreport);
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found!";
                    showreport1.Visible = false;

                }
            }
            if (ddlreporttype.SelectedIndex == 2)
            {
                dsreport.Clear();
                dsreport = LibraryReport();
                if (dsreport.Tables.Count > 0 && dsreport.Tables[0].Rows.Count > 0)
                {
                    loadspreadlibraryutiliDetails(dsreport);

                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found!";
                    showreport1.Visible = false;

                }
            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    #endregion

    protected void grdManualExit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void grdManualExit_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdManualExit.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    #region Report

    //BOOK UTILIZATION
    #region GetReportDetailsforBOOKUTILIZATION

    public DataSet BookReport()
    {
        DataSet dsbookutilireport = new DataSet();
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                department = Convert.ToString(d2.getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            string Count = txtcount.Text;
            string AccNo = string.Empty;
            string title = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(library))
            {
                //library
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and L.lib_code='" + library + "'";
                }
                //AccNo
                if (txtaccessno.Text.Trim() != "")
                {
                    qryaccnofilter = "AND b.Acc_No ='" + txtaccessno.Text + "'";
                    qryaccnofilter1 = "AND R.Acc_No ='" + txtaccessno.Text + "'";
                }
                //Title
                if (txttitle.Text.Trim() != "")
                {
                    qrytitlefilter = "AND R.Title LIKE '" + txttitle.Text + "%'";
                    qrytitlefilter1 = "AND b.Title LIKE '" + txttitle.Text + "%'";
                }
                //date
                if (cbdate.Checked)
                {
                    string fromDate = txt_fromdate.Text;
                    string toDate = txt_todate.Text;
                    string[] from = fromDate.Split('/');
                    string[] to = toDate.Split('/');
                    if (from.Length == 3)
                        fromdate = from[2].ToString() + "-" + from[1].ToString() + "-" + from[0].ToString();
                    if (to.Length == 3)
                        todate = to[2].ToString() + "-" + to[1].ToString() + "-" + to[0].ToString();
                    qrydatefilter = "AND Borrow_Date between'" + fromdate + "'and '" + todate + "'";
                }
                //Count
                if (rbltype.SelectedIndex == 0)
                {
                    if (txtcount.Text.Trim() != "")
                    {
                        if (ddlcount.SelectedIndex == 0)
                        {
                            qrycountequalFilter = "HAVING COUNT(R.Acc_No) = '" + txtcount.Text + "'";
                        }
                        if (ddlcount.SelectedIndex == 1)
                        {
                            qrycountgraterFilter = "HAVING COUNT(R.Acc_No) > '" + txtcount.Text + "'";
                        }
                        if (ddlcount.SelectedIndex == 2)
                        {
                            qrycountlessFilter = "HAVING COUNT(R.Acc_No) < '" + txtcount.Text + "'";
                        }
                    }
                    if (txtcount.Text.Trim() == "")
                    {
                        string countempty = "0";
                        if (ddlcount.SelectedIndex == 0)
                        {
                            qrycountequalFilter = "HAVING COUNT(R.Acc_No) = '" + countempty + "'";
                        }
                        if (ddlcount.SelectedIndex == 1)
                        {
                            qrycountgraterFilter = "HAVING COUNT(R.Acc_No) > '" + countempty + "'";
                        }
                        if (ddlcount.SelectedIndex == 2)
                        {
                            qrycountlessFilter = "HAVING COUNT(R.Acc_No) < '" + countempty + "'";
                        }
                    }
                }
                if (rbltype.SelectedIndex == 1)
                {
                    if (txtcount.Text.Trim() != "")
                    {
                        if (ddlcount.SelectedIndex == 0)
                        {
                            qrycountequalFilter = "HAVING COUNT(R.Title)  = '" + txtcount.Text + "'";
                        }
                        if (ddlcount.SelectedIndex == 1)
                        {
                            qrycountgraterFilter = "HAVING COUNT(R.Title)  > '" + txtcount.Text + "'";
                        }
                        if (ddlcount.SelectedIndex == 2)
                        {
                            qrycountlessFilter = "HAVING COUNT(R.Title)  < '" + txtcount.Text + "'";
                        }
                    }
                    if (txtcount.Text.Trim() == "")
                    {
                        string countempty = "0";
                        if (ddlcount.SelectedIndex == 0)
                        {
                            qrycountequalFilter = "HAVING COUNT(R.Title)  = '" + countempty + "'";
                        }
                        if (ddlcount.SelectedIndex == 1)
                        {
                            qrycountgraterFilter = "HAVING COUNT(R.Title)  > '" + countempty + "'";
                        }
                        if (ddlcount.SelectedIndex == 2)
                        {
                            qrycountlessFilter = "HAVING COUNT(R.Title)  < '" + countempty + "'";
                        }
                    }
                }
                if (rbltype.SelectedIndex == 0)
                {
                    if ((ddlcount.SelectedIndex == 0 && Count == "0") || (ddlcount.SelectedIndex == 0 && Count == ""))
                    {
                        bookutiliselectqry = "SELECT B.Acc_No,b.Title,COUNT(B.Acc_No) AS 'Usage Count' FROM BookDetails B,Library L, borrow br WHERE B.Lib_Code = L.Lib_Code  and B .Dept_Code in('" + department + "') " + qrylibraryFilter + qryaccnofilter + "  and b.Acc_No=br.acc_no GROUP BY B.Lib_Code,b.Title,B.Acc_No ORDER BY Count(B.Acc_No) Desc";
                    }
                    else
                    {
                        bookutiliselectqry = "SELECT DISTINCT R.Acc_No as 'Acc_No',R.Title,COUNT(R.Acc_No) AS 'Usage Count'  FROM Borrow R,BookDetails B,Library L  WHERE R.Lib_Code = B.Lib_Code AND R.Acc_No = B.Acc_No AND B.Lib_Code = L.Lib_Code  and B .Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitlefilter1 + qrytitlefilter + qrydatefilter + " GROUP BY R.Lib_Code,R.Acc_No,R.Title " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + " ORDER BY Count(R.Acc_No) Desc,R.Acc_No";
                    }
                }
                if (rbltype.SelectedIndex == 1)
                {
                    if ((ddlcount.SelectedIndex == 0 && Count == "0") || (ddlcount.SelectedIndex == 0 && Count == ""))
                    {
                        bookutiliselectqry = "SELECT DISTINCT b.Title as 'Title',COUNT(B.Title) AS 'Usage Count' FROM BookDetails B,Library L, borrow br WHERE B.Lib_Code = L.Lib_Code AND  b.Acc_No=br.acc_no and B.Dept_Code in('" + department + "') " + qrytitlefilter1 + qrylibraryFilter + "  GROUP BY B.Lib_Code,b.Title ORDER BY b.Title,";
                    }
                    else
                    {
                        bookutiliselectqry = "SELECT DISTINCT R.Title as 'Title',COUNT(R.Title) AS 'Usage Count' FROM Borrow R,BookDetails B,Library L WHERE R.Lib_Code = B.Lib_Code AND R.Acc_No = B.Acc_No  AND B.Lib_Code = L.Lib_Code            and B .Dept_Code in('" + department + "')" + qrylibraryFilter + qryaccnofilter + qrytitlefilter + qrydatefilter + " GROUP BY R.Lib_Code,R.Title " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + " ORDER BY Count(R.Title) Desc";
                    }
                }
                dsbookutilireport.Clear();
                dsbookutilireport = d2.select_method_wo_parameter(bookutiliselectqry, "Text");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
        return dsbookutilireport;
    }

    #endregion

    #region BookLoadDetails
    private void loadspreadbookutiliDetails(DataSet dsbookutilizationreport)
    {
        try
        {

            string AccessNO = string.Empty;//BookUtilization
            string Title = string.Empty;
            string usage = string.Empty;
            int noofrecords = 0;
            int noofcopies = 0;

            int sno = 0;

            print.Visible = true;
            showreport1.Visible = true;
            grdManualExit.DataSource = dsbookutilizationreport;
            grdManualExit.DataBind();
            grdManualExit.Visible = true;

        }



        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }

    }

    #endregion


    #region GetReportDetailsforGATEUTILIZATION

    public DataSet GateReport()
    {
        DataSet dsgateutilireport = new DataSet();
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                department = Convert.ToString(d2.getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            string Count = txtcount.Text;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(library))
            {
                //library
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and lib_code='" + library + "'";
                }
                //date
                if (cbdate.Checked)
                {
                    string fromDate = txt_fromdate.Text;
                    string toDate = txt_todate.Text;
                    string[] from = fromDate.Split('/');
                    string[] to = toDate.Split('/');
                    if (from.Length == 3)
                        fromdate = from[2].ToString() + "-" + from[1].ToString() + "-" + from[0].ToString();
                    if (to.Length == 3)
                        todate = to[2].ToString() + "-" + to[1].ToString() + "-" + to[0].ToString();
                    qrydatefilter = "AND Entry_Date between'" + fromdate + "'and '" + todate + "'";
                }
                //RollNo
                if (txtrollno.Text.Trim() != "")
                {
                    qryrollstufilter = "AND Roll_No ='" + txtrollno.Text + "'";
                    qryrollstafilter = "AND M.Staff_Code ='" + txtrollno.Text + "'";
                }
                //Count

                if (txtcount.Text.Trim() != "")
                {
                    if (ddlcount.SelectedIndex == 0)
                    {
                        qrycountequalFilter = "HAVING COUNT(U.Roll_No) = '" + txtcount.Text + "'";
                    }
                    if (ddlcount.SelectedIndex == 1)
                    {
                        qrycountgraterFilter = "HAVING COUNT(U.Roll_No) > '" + txtcount.Text + "'";
                    }
                    if (ddlcount.SelectedIndex == 2)
                    {
                        qrycountlessFilter = "HAVING COUNT(U.Roll_No) < '" + txtcount.Text + "'";
                    }
                }
                if (txtcount.Text.Trim() == "")
                {
                    string countempty = "0";
                    if (ddlcount.SelectedIndex == 0)
                    {
                        qrycountequalFilter = "HAVING COUNT(U.Roll_No) = '" + countempty + "'";
                    }
                    if (ddlcount.SelectedIndex == 1)
                    {
                        qrycountgraterFilter = "HAVING COUNT(U.Roll_No) > '" + countempty + "'";
                    }
                    if (ddlcount.SelectedIndex == 2)
                    {
                        qrycountlessFilter = "HAVING COUNT(U.Roll_No) < '" + countempty + "'";
                    }
                }



                if ((ddlcount.SelectedIndex == 0 && Count == "0") || (ddlcount.SelectedIndex == 0 && Count == ""))
                {
                    if (rblmembertype.SelectedIndex == 0)
                    {
                        gateutiliselectqry = " SELECT Roll_No as 'Roll_No',Stud_Name as Stud_Name,Dept_Name,'0' AS 'No. of Time Used'  FROM Registration R,Degree G,Course C,Department D  WHERE R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID  AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  AND R.College_Code ='" + collegecode + "' AND D.Dept_Code IN ('" + department + "')" + qryrollstufilter + " AND Roll_No NOT IN (SELECT Roll_No FROM LibUsers WHERE UserCat = 'Student' " + qrydatefilter + qrylibraryFilter + qryrollstufilter + " GROUP BY Lib_Code,Roll_No ) GROUP BY Roll_No,Stud_Name,Dept_Name  ORDER BY LEN(Roll_No),Roll_No";
                    }
                    if (rblmembertype.SelectedIndex == 1)
                    {
                        gateutiliselectqry = "SELECT M.Staff_Code as 'Roll_No',Staff_Name as Stud_Name,Dept_Name,'0' AS 'No. of Time Used' FROM StaffMaster M,StaffTrans T,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code ='" + collegecode + "' AND T.Latestrec = 1   AND T.Dept_Code IN ('" + department + "')" + qryrollstafilter + " AND M.Staff_Code NOT IN (SELECT Roll_No FROM LibUsers  WHERE UserCat = 'Staff' " + qrydatefilter + qrylibraryFilter + " GROUP BY Lib_Code,Roll_No ) GROUP BY M.Staff_Code,Staff_Name,Dept_Name ORDER BY LEN(M.Staff_Code),M.Staff_Code";

                    }
                    if (rblmembertype.SelectedIndex == 2)
                    {
                        gateutiliselectqry = "SELECT Roll_No as 'Roll_No',Stud_Name as Stud_Name,Dept_Name,'0' AS 'No. of Time Used'  FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID  AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND R.College_Code ='" + collegecode + "' AND D.Dept_Code IN ('" + department + "')" + qryrollstufilter + "  AND Roll_No NOT IN (SELECT Roll_No FROM LibUsers WHERE UserCat = 'Student' " + qrydatefilter + qrylibraryFilter + qryrollstufilter + " GROUP BY Lib_Code,Roll_No ) GROUP BY Roll_No,Stud_Name,Dept_Name   UNION ALL   SELECT M.Staff_Code as Roll_No,Staff_Name as Stud_Name,Dept_Name,'0' AS 'No. of Time Used' FROM StaffMaster M,StaffTrans T,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code ='" + collegecode + "'AND T.Latestrec = 1 AND T.Dept_Code IN ('" + department + "')" + qryrollstafilter + " AND M.Staff_Code NOT IN (SELECT Roll_No FROM LibUsers  WHERE UserCat = 'Staff' " + qrydatefilter + qrylibraryFilter + " GROUP BY Lib_Code,Roll_No ) GROUP BY  M.Staff_Code,Staff_Name,Dept_Name ORDER BY Roll_No";

                    }

                }
                else
                {
                    if (rblmembertype.SelectedIndex == 0)
                    {
                        gateutiliselectqry = "SELECT U.Roll_No as 'Roll_No',R.Stud_Name as Stud_Name,D.Dept_Name,COUNT(U.Roll_No) AS 'No. of Time Used'  FROM LibUsers U,Registration R,Degree G,Course C,Department D  WHERE U.Roll_No = R.Roll_No AND R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID  AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  AND R.College_Code ='" + collegecode + "' AND D.Dept_Code IN ('" + department + "')  AND UserCat = 'Student' " + qrydatefilter + qrylibraryFilter + qryrollstufilter + "  GROUP BY Lib_Code,U.Roll_No,R.Stud_Name,D.Dept_Name " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + " ORDER BY Count(U.Roll_No) Desc,U.Roll_No";
                    }
                    if (rblmembertype.SelectedIndex == 1)
                    {
                        gateutiliselectqry = "SELECT U.Roll_No as 'Roll_No',M.Staff_Name as Stud_Name,D.Dept_Name,COUNT(U.Roll_No) AS 'No. of Time Used' FROM LibUsers U,StaffMaster M,StaffTrans T,HrDept_Master D WHERE U.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code ='" + collegecode + "' AND T.Dept_Code IN ('" + department + "')AND UserCat = 'Staff' AND T.Latestrec = 1  " + qrydatefilter + qrylibraryFilter + qryrollstufilter + " GROUP BY Lib_Code,U.Roll_No,M.Staff_Name,D.Dept_Name " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + " ORDER BY Count(U.Roll_No) Desc,U.Roll_No ";

                    }
                    if (rblmembertype.SelectedIndex == 2)
                    {
                        gateutiliselectqry = "SELECT U.Roll_No as 'Roll_No',R.Stud_Name as Stud_Name,D.Dept_Name,COUNT(U.Roll_No) AS 'No. of Time Used'  FROM LibUsers U,Registration R,Degree G,Course C,Department D  WHERE U.Roll_No = R.Roll_No AND R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID  AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  AND R.College_Code ='" + collegecode + "' AND D.Dept_Code IN ('" + department + "')  AND UserCat = 'Student' " + qrydatefilter + qrylibraryFilter + qryrollstufilter + "  GROUP BY Lib_Code,U.Roll_No,R.Stud_Name,D.Dept_Name " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + " UNION ALL SELECT U.Roll_No as 'Roll_No',M.Staff_Name as Stud_Name,D.Dept_Name,COUNT(U.Roll_No) AS 'No. of Time Used' FROM LibUsers U,StaffMaster M,StaffTrans T,HrDept_Master D WHERE U.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code ='" + collegecode + "' AND T.Dept_Code IN ('" + department + "')AND UserCat = 'Staff' AND T.Latestrec = 1  " + qrydatefilter + qrylibraryFilter + qryrollstufilter + " GROUP BY Lib_Code,U.Roll_No,M.Staff_Name,D.Dept_Name " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + " ORDER BY Count(U.Roll_No) Desc,U.Roll_No ";

                    }

                }
                dsgateutilireport.Clear();
                dsgateutilireport = d2.select_method_wo_parameter(gateutiliselectqry, "Text");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
        return dsgateutilireport;
    }
    #endregion

    #region GateLoadDetails
    private void loadspreadgateutiliDetails(DataSet dsgateutilizationreport)
    {
        try
        {
            // GateloadHeader();
            string rollno = string.Empty;
            string name = string.Empty;
            string department = string.Empty;
            string usage = string.Empty;
            int noofrecords = 0;


            print.Visible = true;
            showreport1.Visible = true;
            grdManualExit.DataSource = dsgateutilizationreport;
            grdManualExit.DataBind();
            grdManualExit.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }

    }

    #endregion



    #region GetReportDetailsforLIBRARYUTILIZATION

    public DataSet LibraryReport()
    {
        DataSet dslibraryutilireport = new DataSet();
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                department = Convert.ToString(d2.getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            string Count = txtcount.Text;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(library))
            {
                //library
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and lib_code='" + library + "'";
                }
                //date
                if (cbdate.Checked)
                {
                    string fromDate = txt_fromdate.Text;
                    string toDate = txt_todate.Text;
                    string[] from = fromDate.Split('/');
                    string[] to = toDate.Split('/');
                    if (from.Length == 3)
                        fromdate = from[2].ToString() + "-" + from[1].ToString() + "-" + from[0].ToString();
                    if (to.Length == 3)
                        todate = to[2].ToString() + "-" + to[1].ToString() + "-" + to[0].ToString();
                    qrydatefilter = "AND Borrow_Date between'" + fromdate + "'and '" + todate + "'";
                }
                //RollNo
                if (txtrollno.Text.Trim() != "")
                {
                    qryrollstufilter = "AND Roll_No ='" + txtrollno.Text + "'";
                    qryrollstafilter = "AND M.Staff_Code ='" + txtrollno.Text + "'";
                }
                //Count

                if (txtcount.Text.Trim() != "")
                {
                    if (ddlcount.SelectedIndex == 0)
                    {
                        qrycountequalFilter = "HAVING COUNT(B.Roll_No) = '" + txtcount.Text + "'";
                    }
                    if (ddlcount.SelectedIndex == 1)
                    {
                        qrycountgraterFilter = "HAVING COUNT(B.Roll_No) > '" + txtcount.Text + "'";
                    }
                    if (ddlcount.SelectedIndex == 2)
                    {
                        qrycountlessFilter = "HAVING COUNT(B.Roll_No) < '" + txtcount.Text + "'";
                    }
                }
                if (txtcount.Text.Trim() == "")
                {
                    string countempty = "0";
                    if (ddlcount.SelectedIndex == 0)
                    {
                        qrycountequalFilter = "HAVING COUNT(B.Roll_No) = '" + countempty + "'";
                    }
                    if (ddlcount.SelectedIndex == 1)
                    {
                        qrycountgraterFilter = "HAVING COUNT(B.Roll_No) > '" + countempty + "'";
                    }
                    if (ddlcount.SelectedIndex == 2)
                    {
                        qrycountlessFilter = "HAVING COUNT(B.Roll_No) < '" + countempty + "'";
                    }
                }



                if ((ddlcount.SelectedIndex == 0 && Count == "0") || (ddlcount.SelectedIndex == 0 && Count == ""))
                {
                    if (rblmembertype.SelectedIndex == 0)
                    {
                        libraryutiliselectqry = "SELECT Roll_No as 'Roll_No',Stud_Name as Stud_Name,Dept_Name,'0' AS 'No. of Time Used' FROM Registration R,Degree G,Course C,Department D  WHERE R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code  AND G.Course_ID = C.Course_ID  AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  AND R.College_Code='" + collegecode + "'  AND D.Dept_Code IN ('" + department + "')" + qryrollstufilter + " AND Roll_No NOT IN (SELECT Roll_No FROM Borrow WHERE Is_Staff = 0 " + qrydatefilter + qrylibraryFilter + "GROUP BY Lib_Code,Roll_No) GROUP BY Roll_No,Stud_Name,Dept_Name ORDER BY LEN(Roll_No),Roll_No ";
                    }
                    if (rblmembertype.SelectedIndex == 1)
                    {
                        libraryutiliselectqry = "SELECT M.Staff_Code as 'Roll_No',Staff_Name as Stud_Name,Dept_Name,'0' AS 'No. of Time Used'  FROM StaffMaster M,StaffTrans T,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code  AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code ='" + collegecode + "' AND T.Latestrec = 1  AND T.Dept_Code IN ('" + department + "') " + qryrollstafilter + " AND M.Staff_Code NOT IN (SELECT Roll_No FROM Borrow WHERE Is_Staff = 1 " + qrydatefilter + qrylibraryFilter + " GROUP BY Lib_Code,Roll_No ) GROUP BY M.Staff_Code,Staff_Name,Dept_Name ORDER BY LEN(M.Staff_Code),M.Staff_Code";

                    }
                    if (rblmembertype.SelectedIndex == 2)
                    {
                        libraryutiliselectqry = "SELECT Roll_No as 'Roll_No',Stud_Name as Stud_Name,Dept_Name,'0' AS 'No. of Time Used'  FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID  AND G.College_Code = C.College_Code  AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND R.College_Code ='" + collegecode + "' AND D.Dept_Code IN ('" + department + "') " + qryrollstufilter + " AND Roll_No NOT IN (SELECT Roll_No FROM Borrow WHERE Is_Staff = 0 " + qrydatefilter + qrylibraryFilter + " GROUP BY Lib_Code,Roll_No ) GROUP BY Roll_No,Stud_Name,Dept_Name  UNION ALL SELECT M.Staff_Code as 'Roll_No',Staff_Name as Stud_Name,Dept_Name,'0' AS 'No. of Time Used' FROM StaffMaster M,StaffTrans T,HrDept_Master D WHERE M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code AND M.College_Code ='" + collegecode + "' AND T.Latestrec = 1 AND T.Dept_Code IN ('" + department + "')" + qryrollstafilter + " AND M.Staff_Code NOT IN (SELECT Roll_No FROM Borrow  WHERE Is_Staff = 1 " + qrydatefilter + qrylibraryFilter + " GROUP BY Lib_Code,Roll_No ) GROUP BY M.Staff_Code,Staff_Name,Dept_Name  ORDER BY Roll_No";

                    }

                }
                else
                {
                    if (rblmembertype.SelectedIndex == 0)
                    {
                        libraryutiliselectqry = "SELECT B.Roll_No as 'Roll_No',R.Stud_Name as Stud_Name,D.Dept_Name,COUNT(B.Roll_No) AS 'No. of Time Used'  FROM Borrow B,Registration R,Degree G,Course C,Department D WHERE B.Roll_No = R.Roll_No AND R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID  AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code  AND R.College_Code ='" + collegecode + "' AND D.Dept_Code IN ('" + department + "') AND Is_Staff = 0 " + qrydatefilter + qryrollstufilter + qrylibraryFilter + "GROUP BY B.Roll_No,R.Stud_Name,D.Dept_Name " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + " ORDER BY Count(B.Roll_No) Desc,B.Roll_No";
                    }
                    if (rblmembertype.SelectedIndex == 1)
                    {
                        libraryutiliselectqry = "SELECT B.Roll_No as 'Roll_No',M.Staff_Name as Stud_Name,D.Dept_Name,COUNT(B.Roll_No) AS 'No. of Time Used' FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D WHERE B.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code  AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code  AND M.College_Code ='" + collegecode + "' AND T.Dept_Code IN ('" + department + "') AND Is_staff = 1 AND T.Latestrec = 1 " + qrydatefilter + qryrollstafilter + qrylibraryFilter + " GROUP BY B.Roll_No,M.Staff_Name,D.Dept_Name " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + "  ORDER BY Count(B.Roll_No) Desc,B.Roll_No";

                    }
                    if (rblmembertype.SelectedIndex == 2)
                    {
                        libraryutiliselectqry = "SELECT B.Roll_No as 'Roll_No',R.Stud_Name as Stud_Name,D.Dept_Name,COUNT(B.Roll_No) AS 'No. of Time Used' FROM Borrow B,Registration R,Degree G,Course C,Department D WHERE B.Roll_No = R.Roll_No AND R.Degree_Code = G.Degree_Code AND R.College_Code = G.College_Code AND G.Course_ID = C.Course_ID  AND G.College_Code = C.College_Code  AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND R.College_Code ='" + collegecode + "' AND D.Dept_Code IN ('" + department + "') AND Is_Staff = 0 " + qrydatefilter + qryrollstufilter + qrylibraryFilter + " GROUP BY B.Roll_No,R.Stud_Name,D.Dept_Name " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + "  UNION ALL SELECT B.Roll_No as 'Roll_No',M.Staff_Name as Stud_Name,D.Dept_Name,COUNT(B.Roll_No) AS 'No. of Time Used' FROM Borrow B,StaffMaster M,StaffTrans T,HrDept_Master D WHERE B.Roll_No = M.Staff_Code AND M.Staff_Code = T.Staff_Code AND T.Dept_Code = D.Dept_Code AND M.College_Code = D.College_Code  AND M.College_Code ='" + collegecode + "' AND T.Dept_Code IN ('" + department + "') AND Is_staff = 1 AND T.Latestrec = 1 " + qrydatefilter + qryrollstafilter + qrylibraryFilter + "  GROUP BY B.Roll_No,M.Staff_Name,D.Dept_Name  " + qrycountequalFilter + qrycountgraterFilter + qrycountlessFilter + " ORDER BY Count(B.Roll_No) Desc,B.Roll_No";

                    }

                }
                dslibraryutilireport.Clear();
                dslibraryutilireport = d2.select_method_wo_parameter(libraryutiliselectqry, "Text");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
        return dslibraryutilireport;
    }
    #endregion

    #region LibraryLoadDetails
    private void loadspreadlibraryutiliDetails(DataSet dslibutilizationreport)
    {
        try
        {

            string rollno = string.Empty;
            string name = string.Empty;
            string department = string.Empty;
            string usage = string.Empty;
            int noofrecords = 0;


            showreport1.Visible = true;
            print.Visible = true;
            grdManualExit.DataSource = dslibutilizationreport;
            grdManualExit.DataBind();
            grdManualExit.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }

    }

    #endregion


    #endregion

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdManualExit, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Utilization Report";
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "UtilizationReport.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed.loadspreaddetails(grdManualExit, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }{ }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
            #region Excel print settings
            string usertype = "";
            if (userCode.Trim() != "")
                usertype = " and usercode='" + userCode + "'";
            else if (groupUserCode.Trim() != "")
                usertype = " and group_code='" + groupUserCode + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }{ }
    }

    #endregion

    #region alertclose

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Utilization Report"); }
    }

    #endregion


}