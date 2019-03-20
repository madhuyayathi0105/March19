using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class LibraryMod_Book_Lock_Unlock : System.Web.UI.Page
{

    #region Field_Declaration
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Dictionary<string, string> dicStaffList = new Dictionary<string, string>();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    DataTable dtCommon = new DataTable();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collcode = string.Empty;
    string libcode = string.Empty;
    string libname = string.Empty;
    string activerow = "";
    string activecol = "";
    int selectedcount = 0;
    string strID = "";
    string strStaffID = "";
    string type = "";
    string depart = "";
    string StrStartAcr = "";
    string StrEndAcr = "";
    int intStartNo = 0;
    int intEndNo = 0;
    string StrCount = "";
    string Sqlbook = "";
    string libsql = "";
    string deptsql = "";
    string newsql = "";
    int BookAcr = 0;
    string BookAcrm = "";
    string BookAcrmsql = "";
    string Chk_AccNosql = "";
    DataSet dsbooks = new DataSet();
    DataTable boklock = new DataTable();
    DataRow drbokloc;
    Boolean pageno = false;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;
    TextBox txtname = new TextBox();
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
                Department();
                loadType();
                grdBookLock.Visible = false;
                rptprint.Visible = false;
                btn_Lock.Visible = false;
            }
        }
        catch (Exception ex) { }
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }

    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdBookLock.Visible = false;
        rptprint.Visible = false;
        btn_Lock.Visible = false;
    }

    #endregion

    #region Library

    public void Library(string LibCollection)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib = "select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }
    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdBookLock.Visible = false;
            rptprint.Visible = false;
            btn_Lock.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }


    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlCollege.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            Hashtable hsLibcode = new Hashtable();
            if (singleUser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + userCode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "text");
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
                        ds = da.select_method_wo_parameter(sql, "text");
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
                        ds = da.select_method_wo_parameter(sql, "text");
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
    #endregion

    #region Department
    public void Department()
    {
        try
        {

            string College = ddlCollege.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(College))
            {
                //hat.Add("collegecode", College);
                //ds.Clear();
                //ds = da.select_method("LoadJournalDepartment", hat, "sp");
                string loaddept = "Select distinct ISNULL(dept_name,'') dept_name  from journal_dept order by dept_name ";
                ds.Clear();
                ds = da.select_method_wo_parameter(loaddept, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_dept.DataSource = ds;
                    ddl_dept.DataTextField = "Dept_Name";
                    ddl_dept.DataValueField = "Dept_Name";
                    ddl_dept.DataBind();
                    ddl_dept.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }

    }

    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdBookLock.Visible = false;
            rptprint.Visible = false;
            btn_Lock.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }


    }


    #endregion

    #region Type
    public void loadType()
    {
        try
        {
            ddl_type.Items.Add("Book");
            ddl_type.Items.Add("Back Volume");
            ddl_type.Items.Add("NonBook Material");
            ddl_type.Items.Add("Project Book");
            ddl_type.Items.Add("Periodicals");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }

    }

    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdBookLock.Visible = false;
        rptprint.Visible = false;
        btn_Lock.Visible = false;

    }
    #endregion

    #region lockAndUnlock
    protected void rblbook_Selected(object sender, EventArgs e)
    {

        Chk_AccNo.Checked = false;
        txt_from.Text = "";
        txt_to.Text = "";
        grdBookLock.Visible = false;
        rptprint.Visible = false;
        btn_Lock.Visible = false;



    }
    #endregion

    protected void Chk_AccNo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            grdBookLock.Visible = false;
            rptprint.Visible = false;
            btn_Lock.Visible = false;
            if (Chk_AccNo.Checked == true)
            {
                txt_from.Enabled = true;
                txt_to.Enabled = true;
                txt_from.Text = "";
                txt_to.Text = "";
            }
            else
            {
                txt_from.Enabled = false;
                txt_to.Enabled = false;
                txt_from.Text = "";
                txt_to.Text = "";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }


    }

    protected void chkOldSearch_CheckedChanged(object sender, EventArgs e)
    {


    }

    protected void grdBookLock_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdBookLock.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    #region Go
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {

            string text = "";
            //Cmd_OK.Enabled = False
            //If chkOldSearch.value = 0 Then
            //    vaSpread1.MaxRows = 0
            //End If
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_dept.Items.Count > 0)
                depart = Convert.ToString(ddl_dept.SelectedValue);
            if (ddl_type.Items.Count > 0)
                type = Convert.ToString(ddl_type.SelectedValue);
            if (libcode == "" || depart == "" || type == "")
            {
                if (libcode == "")
                    text = "Library";
                if (depart == "")
                    text = "Department";
                if (libcode == "")
                    text = "Type";
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Choose the " + text + " and then proceed";
                return;
            }
            if (Chk_AccNo.Checked == true)
            {
                if (txt_from.Text == "" || txt_to.Text == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Enter the Access No ";
                    return;
                }
                if (txt_from.Text != "" || txt_to.Text != "")
                {
                    int From = Convert.ToInt32(txt_from.Text);
                    int To = Convert.ToInt32(txt_to.Text);
                    if (From > To)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "To Access No. should be less then from Access No.";
                        return;
                    }

                }

            }
            if (libcode != "All")
                libsql = "AND Lib_Code ='" + libcode + "'";
            if (ddl_type.Text == "Book")
            {

                if (depart != "All")
                    deptsql = "and dept_code='" + depart + "'";
                Book();
            }
            else if (ddl_type.Text == "Back Volume")
            {

                if (txt_from.Text == "")
                    newsql = "";
                else
                    newsql = "and access_code between '" + txt_from.Text + "' and '" + txt_to.Text + "'";
                back_volume();
            }
            else if (ddl_type.Text == "NonBook Material")
            {

                if (txt_from.Text == "")
                    newsql = "";
                else
                    newsql = "and acc_no between '" + txt_from.Text + "' and '" + txt_to.Text + "'";
                if (depart != "All")
                    deptsql = "and department='" + depart + "'";
                nonbook_materials();

            }
            else if (ddl_type.Text == "Project Book")
            {

                if (txt_from.Text == "")
                    newsql = "";
                else
                    newsql = "and probook_accno between '" + txt_from.Text + "' and '" + txt_to.Text + "'";

                Project_Book();

            }
            else//Periodicals
            {

                if (txt_from.Text == "")
                    newsql = "";
                else
                    newsql = "and access_code between '" + txt_from.Text + "' and '" + txt_to.Text + "'";
                if (depart != "All")
                    deptsql = "and dept_name='" + depart + "'";

                periodicals();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }

    }
    #endregion

    protected void grdBookLock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ddl_type.SelectedItem.Text == "Book")
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = true;

                }
                else if (ddl_type.SelectedItem.Text == "Back Volume")
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = true;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = true;
                }
                else if (ddl_type.SelectedItem.Text == "NonBook Material")
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = true;
                }
                else if (ddl_type.SelectedItem.Text == "Project Book")
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = true;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = true;
                }
                else if (ddl_type.SelectedItem.Text == "Periodicals")
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddl_type.SelectedItem.Text == "Book")
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = true;
                }
                else if (ddl_type.SelectedItem.Text == "Back Volume")
                {
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = true;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = true;
                }
                else if (ddl_type.SelectedItem.Text == "NonBook Material")
                {
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = true;
                }
                else if (ddl_type.SelectedItem.Text == "Project Book")
                {
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = true;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = true;
                }
                else if (ddl_type.SelectedItem.Text == "Periodicals")
                {
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                }
            }
        }


        catch (Exception ex)
        {
        }

    }


    #region books
    public void Book()
    {
        try
        {
            //  txtname.ID="txtbokloc";
            string remark = "";
            //if (txt_from.Text == "")
            //    newsql = "";
            //else
            //    newsql = "and access_code='" + txt_from.Text + "'";

            if (Txt_BookAcr.Text != "")
            {
                BookAcrm = Convert.ToString(Txt_BookAcr.Text);
                BookAcrmsql = "and left(b.acc_no='" + BookAcrm + "' and ISNUMERIC(substring(b.acc_no," + BookAcrm.Length + 1 + ",1)) = 1 ";
            }
            StrCount = d2.GetFunction("SELECT COUNT(*) FROM BookDetails WHERE IsNumeric(Acc_No) <> 1 " + libsql + "");
            if (Chk_AccNo.Checked == true)
            {
                if (StrCount == "0")
                    Chk_AccNosql = " and cast(B.acc_no as int) between '" + txt_from.Text + "' and '" + txt_to.Text + "'";
                else
                {
                    if (BookAcrm.Length == 0)

                        Chk_AccNosql = " and cast(substring(B.acc_no,1,len(B.acc_no)) as int) between '" + txt_from.Text + "' AND '" + txt_to.Text + "' and ISNUMERIC(b.acc_no) = 1 ";

                    else

                        Chk_AccNosql = " and cast(substring(B.acc_no," + BookAcrm.Length + 1 + ",len(B.acc_no)-1) as int) between '" + txt_from.Text + "' AND '" + txt_to.Text + "' ";
                }
            }

            if (rblbook.SelectedIndex == 1)
            {
                Sqlbook = "select acc_no,title,author,'' as remark from bookdetails b where book_status='Available' " + deptsql + libsql + BookAcrmsql + Chk_AccNosql + " ORDER BY LEN(B.Acc_No),B.Acc_No";
            }
            else
            {
                Sqlbook = "select  acc_no,title,author,remark from bookdetails b where book_status='Locked' " + deptsql + libsql + BookAcrmsql + Chk_AccNosql + " ORDER BY LEN(B.Acc_No),B.Acc_No";
            }
            dsbooks.Clear();
            dsbooks = d2.select_method_wo_parameter(Sqlbook, "Text");
            int i = 0;
            double rowcount = 0.0;
            double pagecn = 0.0;

            int sno = 0;
            if (dsbooks.Tables.Count > 0 && dsbooks.Tables[0].Rows.Count > 0)
            {

                boklock.Columns.Add("Access No");
                boklock.Columns.Add("Title");
                boklock.Columns.Add("Author");
                boklock.Columns.Add("Publisher");
                boklock.Columns.Add("Nameofthestudent");
                boklock.Columns.Add("DepartmentName");
                boklock.Columns.Add("Reason");

                for (int row = i; row < dsbooks.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drbokloc = boklock.NewRow();
                    string acc_no = Convert.ToString(dsbooks.Tables[0].Rows[row]["acc_no"]).Trim();
                    string title = Convert.ToString(dsbooks.Tables[0].Rows[row]["title"]).Trim();
                    string author = Convert.ToString(dsbooks.Tables[0].Rows[row]["author"]).Trim();
                    remark = Convert.ToString(dsbooks.Tables[0].Rows[row]["remark"]);
                    drbokloc["Access No"] = acc_no;
                    drbokloc["Title"] = title;
                    drbokloc["Author"] = author;
                    drbokloc["Publisher"] = "";
                    drbokloc["Nameofthestudent"] = "";
                    drbokloc["DepartmentName"] = "";
                    drbokloc["Reason"] = remark;

                    boklock.Rows.Add(drbokloc);

                }
                grdBookLock.DataSource = boklock;
                grdBookLock.DataBind();
                grdBookLock.Visible = true;
                rptprint.Visible = true;
                for (int l = 0; l < grdBookLock.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdBookLock.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdBookLock.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                           
                        }
                    }
                }
                if (rblbook.SelectedIndex == 1)
                {
                    btn_Lock.Visible = true;
                    btn_Lock.Text = "Lock";
                }
                else
                {
                    btn_Lock.Text = "Unlock";
                    btn_Lock.Visible = true;
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
                grdBookLock.Visible = false;
                rptprint.Visible = false;
                btn_Lock.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }


    }
    #endregion

    #region BackVolume
    public void back_volume()
    {
        try
        {
            int sno = 0;
            if (rblbook.SelectedIndex == 1)
                Sqlbook = "select access_code,title,publisher,'' remarks from back_volume where issue_flag='available' " + newsql + libsql + "";
            else
                Sqlbook = "select access_code,title,publisher,remarks from back_volume where issue_flag='Locked' " + newsql + libsql + "";
            dsbooks.Clear();
            dsbooks = d2.select_method_wo_parameter(Sqlbook, "Text");
            int i = 0;
            double rowcount = 0.0;
            double pagecn = 0.0;

            if (dsbooks.Tables.Count > 0 && dsbooks.Tables[0].Rows.Count > 0)
            {
                boklock.Columns.Add("Access No");
                boklock.Columns.Add("Title");
                boklock.Columns.Add("Author");
                boklock.Columns.Add("Publisher");
                boklock.Columns.Add("Nameofthestudent");
                boklock.Columns.Add("DepartmentName");
                boklock.Columns.Add("Reason");
                for (int row = i; row < dsbooks.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drbokloc = boklock.NewRow();
                    string acc_no = Convert.ToString(dsbooks.Tables[0].Rows[row]["access_code"]).Trim();
                    string title = Convert.ToString(dsbooks.Tables[0].Rows[row]["title"]).Trim();
                    string author = Convert.ToString(dsbooks.Tables[0].Rows[row]["publisher"]).Trim();
                    string remark = Convert.ToString(dsbooks.Tables[0].Rows[row]["remarks"]);

                    drbokloc["Access No"] = acc_no;
                    drbokloc["Title"] = title;
                    drbokloc["Author"] = "";
                    drbokloc["Publisher"] = author;
                    drbokloc["Nameofthestudent"] = "";
                    drbokloc["DepartmentName"] = "";
                    drbokloc["Reason"] = remark;

                    boklock.Rows.Add(drbokloc);


                }
                grdBookLock.DataSource = boklock;
                grdBookLock.DataBind();
                grdBookLock.Visible = true;
                rptprint.Visible = true;
                for (int l = 0; l < grdBookLock.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdBookLock.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdBookLock.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                }
                if (rblbook.SelectedIndex == 1)
                {
                    btn_Lock.Visible = true;
                    btn_Lock.Text = "Lock";
                }
                else
                {
                    btn_Lock.Text = "Unlock";
                    btn_Lock.Visible = true;
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }

    }
    #endregion

    #region nonbook_materials
    public void nonbook_materials()
    {
        try
        {
            int sno = 0;
            if (rblbook.SelectedIndex == 1)
                Sqlbook = "select nonbookmat_no,title,author,'' as remarks from nonbookmat where issue_flag='available' " + newsql + deptsql + libsql + "";
            else
                Sqlbook = "select nonbookmat_no,title,author,remarks from nonbookmat where issue_flag='Locked' " + newsql + deptsql + libsql + "";
            dsbooks.Clear();
            dsbooks = d2.select_method_wo_parameter(Sqlbook, "Text");
            int i = 0;
            double rowcount = 0.0;
            double pagecn = 0.0;

            if (dsbooks.Tables.Count > 0 && dsbooks.Tables[0].Rows.Count > 0)
            {

                boklock.Columns.Add("Access No");
                boklock.Columns.Add("Title");
                boklock.Columns.Add("Author");
                boklock.Columns.Add("Publisher");
                boklock.Columns.Add("Nameofthestudent");
                boklock.Columns.Add("DepartmentName");
                boklock.Columns.Add("Reason");
                for (int row = i; row < dsbooks.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drbokloc = boklock.NewRow();
                    string acc_no = Convert.ToString(dsbooks.Tables[0].Rows[row]["nonbookmat_no"]).Trim();
                    string title = Convert.ToString(dsbooks.Tables[0].Rows[row]["title"]).Trim();
                    string author = Convert.ToString(dsbooks.Tables[0].Rows[row]["author"]).Trim();
                    string remark = Convert.ToString(dsbooks.Tables[0].Rows[row]["remarks"]);

                    drbokloc["Access No"] = acc_no;
                    drbokloc["Title"] = title;
                    drbokloc["Author"] = author;
                    drbokloc["Publisher"] = "";
                    drbokloc["Nameofthestudent"] = "";
                    drbokloc["DepartmentName"] = "";
                    drbokloc["Reason"] = remark;

                    boklock.Rows.Add(drbokloc);


                }
                grdBookLock.DataSource = boklock;
                grdBookLock.DataBind();
                grdBookLock.Visible = true;
                rptprint.Visible = true;
                for (int l = 0; l < grdBookLock.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdBookLock.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdBookLock.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                }
                if (rblbook.SelectedIndex == 1)
                {
                    btn_Lock.Visible = true;
                    btn_Lock.Text = "Lock";
                }
                else
                {
                    btn_Lock.Text = "Unlock";
                    btn_Lock.Visible = true;
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }
    }
    #endregion

    #region Project_Books
    public void Project_Book()
    {
        try
        {
            int i = 0;
            int sno = 0;
            if (rblbook.SelectedIndex == 1)
                Sqlbook = "select probook_accno,title,name,'' as remarks from project_book where issue_flag='available' " + newsql + libsql + "";
            else
                Sqlbook = "select probook_accno,title,name, remarks from project_book where issue_flag='Locked' " + newsql + libsql + "";
            dsbooks.Clear();
            dsbooks = d2.select_method_wo_parameter(Sqlbook, "Text");

            if (dsbooks.Tables.Count > 0 && dsbooks.Tables[0].Rows.Count > 0)
            {

                boklock.Columns.Add("Access No");
                boklock.Columns.Add("Title");
                boklock.Columns.Add("Nameofthestudent");
                boklock.Columns.Add("Author");
                boklock.Columns.Add("Publisher");
                boklock.Columns.Add("Nameofthestudent");
                boklock.Columns.Add("DepartmentName");
                boklock.Columns.Add("Reason");
                for (int row = i; row < dsbooks.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drbokloc = boklock.NewRow();
                    string acc_no = Convert.ToString(dsbooks.Tables[0].Rows[row]["probook_accno"]).Trim();
                    string title = Convert.ToString(dsbooks.Tables[0].Rows[row]["title"]).Trim();
                    string author = Convert.ToString(dsbooks.Tables[0].Rows[row]["name"]).Trim();
                    string remark = Convert.ToString(dsbooks.Tables[0].Rows[row]["remarks"]);
                    drbokloc["Access No"] = acc_no;
                    drbokloc["Title"] = title;
                    drbokloc["Author"] = "";
                    drbokloc["Publisher"] = "";
                    drbokloc["Nameofthestudent"] = author;
                    drbokloc["DepartmentName"] = "";

                    drbokloc["Reason"] = remark;

                    boklock.Rows.Add(drbokloc);


                }
                grdBookLock.DataSource = boklock;
                grdBookLock.DataBind();
                grdBookLock.Visible = true;
                rptprint.Visible = true;

                for (int l = 0; l < grdBookLock.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdBookLock.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdBookLock.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                }
                if (rblbook.SelectedIndex == 1)
                {
                    btn_Lock.Visible = true;
                    btn_Lock.Text = "Lock";
                }
                else
                {
                    btn_Lock.Text = "Unlock";
                    btn_Lock.Visible = true;
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }
    }
    #endregion

    #region periodicals
    public void periodicals()
    {
        try
        {
            int sno = 0;
            if (rblbook.SelectedIndex == 1)
                Sqlbook = "select access_code,title,dept_name,'' as remarks from journal where issue_flag='available' " + newsql + deptsql + libsql + "";
            else
                Sqlbook = "select access_code,title,dept_name,remarks from journal where issue_flag='Locked' " + newsql + deptsql + libsql + "";
            dsbooks.Clear();
            dsbooks = d2.select_method_wo_parameter(Sqlbook, "Text");
            int i = 0;
            double rowcount = 0.0;
            double pagecn = 0.0;

            if (dsbooks.Tables.Count > 0 && dsbooks.Tables[0].Rows.Count > 0)
            {
                boklock.Columns.Add("Access No");
                boklock.Columns.Add("Title");
                boklock.Columns.Add("Author");
                boklock.Columns.Add("Publisher");
                boklock.Columns.Add("Nameofthestudent");
                boklock.Columns.Add("DepartmentName");
                boklock.Columns.Add("Reason");
                for (int row = i; row < dsbooks.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drbokloc = boklock.NewRow();
                    string acc_no = Convert.ToString(dsbooks.Tables[0].Rows[row]["access_code"]).Trim();
                    string title = Convert.ToString(dsbooks.Tables[0].Rows[row]["title"]).Trim();
                    string author = Convert.ToString(dsbooks.Tables[0].Rows[row]["dept_name"]).Trim();
                    string remark = Convert.ToString(dsbooks.Tables[0].Rows[row]["remarks"]);
                    drbokloc["Access No"] = acc_no;
                    drbokloc["Title"] = title;
                    drbokloc["Author"] = "";
                    drbokloc["Publisher"] = "";
                    drbokloc["Nameofthestudent"] = "";
                    drbokloc["DepartmentName"] = author;
                    drbokloc["Reason"] = remark;

                    boklock.Rows.Add(drbokloc);


                }
                grdBookLock.DataSource = boklock;
                grdBookLock.DataBind();
                grdBookLock.Visible = true;
                rptprint.Visible = true;
                for (int l = 0; l < grdBookLock.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdBookLock.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdBookLock.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                }
                if (rblbook.SelectedIndex == 1)
                {
                    btn_Lock.Visible = true;
                    btn_Lock.Text = "Lock";
                }
                else
                {
                    btn_Lock.Text = "Unlock";
                    btn_Lock.Visible = true;
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }
    }
    #endregion

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Book_Lock_Unlock";
            string pagename = "Book_Lock_Unlock.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdBookLock, pagename, degreedetails,0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdBookLock, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock"); }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #endregion

    #region LockAndUnlock

    protected void btn_Lock_Click(object sender, EventArgs e)
    {
        try
        {
           
            string sql_1 = "";
            string sql_2 = "";
            int Lock_Unlock = 0;
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (grdBookLock.Rows.Count > 0)
            {
                if (grdBookLock.Rows.Count > 0)
                {
                    foreach (GridViewRow row in grdBookLock.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            selectedcount++;
                            TextBox reason = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");

                            if (reason.Text.Trim() == "")
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Enter Reasons For Locking";
                                return;
                            }
                        }
                    }
                }

                if (selectedcount == 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please select atleast one record";
                    return;
                }

            }
            if (grdBookLock.Rows.Count > 0)
            {
                string acc_no = "";
                string reason = "";
                if (rblbook.SelectedIndex == 1)//Lock
                {
                    if (ddl_type.Text == "Book")
                    {
                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                if (AccessNo.Text.Trim() != "")
                                {
                                    acc_no = AccessNo.Text.Trim();
                                }
                                TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                sql_1 = "update bookdetails set book_status='Locked',remark='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                            }
                        }
                    }
                    else if (ddl_type.Text == "Back Volume")
                    {

                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                if (AccessNo.Text.Trim() != "")
                                {
                                    acc_no = AccessNo.Text.Trim();
                                }
                                TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                sql_1 = "update back_volume set issue_flag='Locked',remarks='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                            }
                        }

                    }
                    else if (ddl_type.Text == "NonBook Materials")
                    {
                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                if (AccessNo.Text.Trim() != "")
                                {
                                    acc_no = AccessNo.Text.Trim();
                                }

                                TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                sql_1 = "update nonbookmat set issue_flag='Locked',remarks='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                            }
                        }

                    }

                    else if (ddl_type.Text == "Project Book")
                    {
                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                if (AccessNo.Text.Trim() != "")
                                {
                                    acc_no = AccessNo.Text.Trim();
                                }

                                TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                sql_1 = "update project_book set issue_flag='Locked',remarks='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                            }
                        }

                    }

                    else
                    {
                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                if (AccessNo.Text.Trim() != "")
                                {
                                    acc_no = AccessNo.Text.Trim();
                                }

                                TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                sql_1 = "update journal set issue_flag='Locked',remarks='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                            }
                        }

                    }

                }
                else //Unlock
                {
                    if (ddl_type.Text == "Book")
                    {

                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                if (AccessNo.Text.Trim() != "")
                                {
                                    acc_no = AccessNo.Text.Trim();
                                }

                                TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                sql_1 = "update bookdetails set book_status='Available',remark='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                            }

                        }


                    }
                    else if (ddl_type.Text == "Back Volume")
                    {
                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                if (AccessNo.Text.Trim() != "")
                                {
                                    acc_no = AccessNo.Text.Trim();
                                }

                                TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                sql_1 = "update back_volume set issue_flag='Available',remarks='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                            }
                        }

                    }
                    else if (ddl_type.Text == "NonBook Materials")
                    {
                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                if (AccessNo.Text.Trim() != "")
                                {
                                    acc_no = AccessNo.Text.Trim();
                                }

                                TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                sql_1 = "update nonbookmat set issue_flag='Available',remarks='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                            }
                        }

                    }

                    else if (ddl_type.Text == "Project Book")
                    {
                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                             int RowCnt = Convert.ToInt32(row.RowIndex);
                             if (cbsel.Checked == true)
                             {
                                 Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                 if (AccessNo.Text.Trim() != "")
                                 {
                                     acc_no = AccessNo.Text.Trim();
                                 }

                                 TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                 if (reason_Val.Text.Trim() != "")
                                 {
                                     reason = reason_Val.Text.Trim();
                                 }
                                 sql_1 = "update project_book set issue_flag='Available',remarks='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                 Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                             }
                        }

                    }

                    else
                    {

                        foreach (GridViewRow row in grdBookLock.Rows)
                        {
                            CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                            int RowCnt = Convert.ToInt32(row.RowIndex);
                            if (cbsel.Checked == true)
                            {
                                Label AccessNo = (Label)grdBookLock.Rows[RowCnt].FindControl("lbl_accessno");
                                if (AccessNo.Text.Trim() != "")
                                {
                                    acc_no = AccessNo.Text.Trim();
                                }

                                TextBox reason_Val = (TextBox)grdBookLock.Rows[RowCnt].FindControl("lbl_reason");
                                if (reason_Val.Text.Trim() != "")
                                {
                                    reason = reason_Val.Text.Trim();
                                }
                                sql_1 = "update journal set issue_flag='Available',remarks='" + reason + "' where acc_no='" + acc_no + "' and lib_code ='" + libcode + "'";
                                Lock_Unlock = d2.update_method_wo_parameter(sql_1, "Text");
                            }
                        }
                    }


                }


            }

            if (Lock_Unlock > 0)
            {

                if (rblbook.SelectedIndex == 0)
                {
                    Divalert.Visible = true;
                    lblalertmsg.Text = "The selected " + ddl_type.Text + " are UnLocked successfully";

                }
                else
                {
                    Divalert.Visible = true;
                    lblalertmsg.Text = "The selected " + ddl_type.Text + " are locked successfully";

                }
                btngo_Click(sender, e);

            }



        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Book_Lock_Unlock");
        }

    }

    #endregion

    protected void btnerrclose_Click(object sender, EventArgs e)
    {

        alertpopwindow.Visible = false;

    }

    protected void btnerrclose1_Click(object sender, EventArgs e)
    {
        Divalert.Visible = false;

    }


}