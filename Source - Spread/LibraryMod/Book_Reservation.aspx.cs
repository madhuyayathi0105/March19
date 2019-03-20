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

public partial class LibraryMod_Book_Reservation : System.Web.UI.Page
{

    #region Field_declaration
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
    Boolean Cellclick = false;
    static string searchlibcode = string.Empty;
    static string Acclibcode = string.Empty;
    DataTable bokres = new DataTable();
    DataTable studdetails = new DataTable();
    DataRow drdet;
    DataRow dr;
    static int searchby = 0;
    int selectedCellIndex = 0;
    DataTable bokaccess = new DataTable();
    DataTable bokstaff = new DataTable();
    DataRow drbokacc;
    DataRow drbokstaff;
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
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                //txt_time.Attributes.Add("readonly", "readonly");
                text_Date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                text_Date.Attributes.Add("readonly", "readonly");
                Bindcollege();
                getLibPrivil();
                loadsearch();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                grdBkReserve.Visible = false;
                btn_delete.Visible = false;
                btn_cancel_res.Visible = false;
                rptprint.Visible = false;

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearch(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();

        query = "SELECT DISTINCT  TOP  100 title FROM priority_studstaff where title Like '" + prefixText + "%' AND Lib_Code='" + searchlibcode + "'  order by title";


        values = ws.Getname(query);
        return values;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearchvalue(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();
        if (searchby == 1)
            query = "SELECT DISTINCT  TOP  100 acc_no FROM bookdetails where acc_no Like '" + prefixText + "%' and book_status<>'Available' and lib_code='" + Acclibcode + "' order by acc_no";
        else if (searchby == 2)
            query = "SELECT DISTINCT  TOP  100 title FROM bookdetails where title Like '" + prefixText + "%' and book_status<>'Available' and lib_code='" + Acclibcode + "' order by title";
        else if (searchby == 3)
            query = "SELECT DISTINCT  TOP  100 Author FROM bookdetails where Author Like '" + prefixText + "%' and book_status<>'Available' and lib_code='" + Acclibcode + "' order by Author";
        else if (searchby == 4)
            query = "SELECT DISTINCT  TOP  100 publisher FROM bookdetails where publisher Like '" + prefixText + "%' and book_status<>'Available' and lib_code='" + Acclibcode + "' order by publisher";
        else if (searchby == 5)
            query = "SELECT DISTINCT  TOP  100 edition FROM bookdetails where edition Like '" + prefixText + "%' and book_status<>'Available' and lib_code='" + Acclibcode + "' order by edition";
        values = ws.Getname(query);
        return values;
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
        grdBkReserve.Visible = false;

        btn_delete.Visible = false;
        btn_cancel_res.Visible = false;
        rptprint.Visible = false;

    }

    #endregion

    #region Library
    public void bindLibrary(string LibCode)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                dicQueryParameter.Clear();
                SelectQ = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCode + " and college_code in('" + College + "') ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(SelectQ, "text");
                int SelectVal = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();

                    ddl_txt_lib.DataSource = ds;
                    ddl_txt_lib.DataTextField = "lib_name";
                    ddl_txt_lib.DataValueField = "lib_code";
                    ddl_txt_lib.DataBind();
                    searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //select_range.Visible = true;
            grdBkReserve.Visible = false;
            btn_delete.Visible = false;
            btn_cancel_res.Visible = false;
            rptprint.Visible = false;

            searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }
    #endregion

    #region Search
    public void loadsearch()
    {
        try
        {
            ddlsearch.Items.Clear();
            ddlsearch.Items.Add("All");
            ddlsearch.Items.Add("Title");
            ddlsearch.Items.Add("Req Date");
            ddlsearch.Items.Add("Status");

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    public void loadsearchstatus()
    {
        try
        {
            ddl_serach_Wise.Items.Clear();
            ddl_serach_Wise.Items.Add("Reserved");
            ddl_serach_Wise.Items.Add("Cancelled");
            ddl_serach_Wise.Items.Add("Completed");

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void ddlSearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdBkReserve.Visible = false;
            btn_delete.Visible = false;
            btn_cancel_res.Visible = false;
            rptprint.Visible = false;
            //select_range.Visible = true;
            if (ddlsearch.SelectedIndex == 0)
            {
                text_tile.Visible = false;
                text_Date.Visible = false;
                ddl_serach_Wise.Visible = false;
            }
            else if (ddlsearch.SelectedIndex == 1)
            {
                text_tile.Visible = true;
                text_Date.Visible = false;
                ddl_serach_Wise.Visible = false;
            }
            else if (ddlsearch.SelectedIndex == 2)
            {
                text_tile.Visible = false;
                text_Date.Visible = true;
                ddl_serach_Wise.Visible = false;
            }
            else
            {
                loadsearchstatus();
                text_tile.Visible = false;
                text_Date.Visible = false;
                ddl_serach_Wise.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void ddl_serach_Wise_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdBkReserve.Visible = false;
        btn_delete.Visible = false;
        btn_cancel_res.Visible = false;
        rptprint.Visible = false;
        //select_range.Visible = true;
    }

    #endregion

    #region Go

    protected void btngo_Click(object sender, EventArgs e)
    {
        DataSet dsgo = new DataSet();
        try
        {
            #region get Value

            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string getrecord = "";
            string datesearch = "";
            string getdate = Convert.ToString(text_Date.Text);
            string[] date = getdate.Split('/');
            if (date.Length == 3)
                datesearch = date[1].ToString() + "/" + date[0].ToString() + "/" + date[2].ToString();
            if (!string.IsNullOrEmpty(collcode) && !string.IsNullOrEmpty(libcode))
            {
                if (ddlsearch.Text == "All")
                {
                    getrecord = "SELECT access_number,p.title,Author,roll_no,staff_code,cur_date,cur_time,cancel_flag FROM priority_studstaff p,bookdetails b where b.acc_no = p.access_number and b.lib_code = p.lib_code and p.lib_code='" + libcode + "'";
                }
                else
                {
                    if (ddlsearch.Text == "Title")
                    {
                        if (text_tile.Text != "")
                        {
                            getrecord = "SELECT access_number,p.title,author,roll_no,staff_code,cur_date,cur_time,cancel_flag FROM priority_studstaff p,bookdetails b where p.access_number = b.acc_no and b.lib_code = p.lib_code and p.lib_code='" + libcode + "' and p.title like '%" + text_tile.Text + "%'";
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Enter title";
                            return;
                        }
                    }
                    else if (ddlsearch.Text == "Req Date")
                    {
                        getrecord = "SELECT access_number,p.title,author,roll_no,staff_code,cur_date, cur_time,cancel_flag FROM priority_studstaff p,bookdetails b where b.acc_no = p.access_number and b.lib_code = p.lib_code and p.lib_code='" + libcode + "' and cur_date = '" + datesearch + "'";
                    }
                    else
                    {
                        if (ddl_serach_Wise.SelectedIndex == 0)
                        {
                            getrecord = "select  access_number,p.title,author,roll_no,staff_code,cur_date,cur_time,cancel_flag from priority_studstaff p,bookdetails b where b.acc_no = p.access_number and b.lib_code = p.lib_code and p.lib_code='" + libcode + "' and (cancel_flag=0 or cancel_flag is null)";

                        }
                        else if (ddl_serach_Wise.SelectedIndex == 1)
                        {
                            getrecord = "select  access_number,p.title,author,roll_no,staff_code,cur_date,cur_time,cancel_flag from priority_studstaff p,bookdetails b  where p.access_number = b.acc_no and b.lib_code = p.lib_code and p.lib_code='" + libcode + "' and cancel_flag=1";

                        }

                    }
                }
            }
            dsgo.Clear();
            dsgo = d2.select_method_wo_parameter(getrecord, "Text");

            if (dsgo.Tables.Count > 0 && dsgo.Tables[0].Rows.Count > 0)
            {
                bokres.Columns.Add("SNo", typeof(string));
                bokres.Columns.Add("AccessNo", typeof(string));
                bokres.Columns.Add("Title", typeof(string));
                bokres.Columns.Add("Author", typeof(string));
                bokres.Columns.Add("Roll No", typeof(string));
                bokres.Columns.Add("Name", typeof(string));
                bokres.Columns.Add("Req.Date", typeof(string));
                bokres.Columns.Add("Req.Time", typeof(string));
                bokres.Columns.Add("Status", typeof(string));


                dr = bokres.NewRow();
                dr["SNo"] = "SNo";
                dr["AccessNo"] = "AccessNo";
                dr["Title"] = "Title";
                dr["Author"] = "Author";
                dr["Roll No"] = "Roll No";
                dr["Name"] = "Name";
                dr["Req.Date"] = "Req.Date";
                dr["Req.Time"] = "Req.Time";
                dr["Status"] = "Status";
                bokres.Rows.Add(dr);

                int sno = 0;
                string id = "";
                string flag = "";
                string rcurdae = "";
                string stdstaffname = "";
                if (dsgo.Tables.Count > 0 && dsgo.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsgo.Tables[0].Rows.Count; row++)
                    {
                        sno++;
                        dr = bokres.NewRow();

                        string raccno = Convert.ToString(dsgo.Tables[0].Rows[row]["access_number"]).Trim();
                        string rtitle = Convert.ToString(dsgo.Tables[0].Rows[row]["title"]).Trim();
                        string rauthor = Convert.ToString(dsgo.Tables[0].Rows[row]["author"]).Trim();
                        string r_rollno = Convert.ToString(dsgo.Tables[0].Rows[row]["roll_no"]).Trim();
                        string r_stcode = Convert.ToString(dsgo.Tables[0].Rows[row]["staff_code"]).Trim();
                        string rcurdate = Convert.ToString(dsgo.Tables[0].Rows[row]["cur_date"]).Trim();
                        string rcutime = Convert.ToString(dsgo.Tables[0].Rows[row]["cur_time"]).Trim();
                        string rflag = Convert.ToString(dsgo.Tables[0].Rows[row]["cancel_flag"]).Trim();
                        if (rcurdate != "")
                        {
                            string[] cdate = rcurdate.Split(' ');
                            rcurdae = cdate[0];
                        }
                        if (rflag == "0")
                            flag = "Reserved";
                        else if (rflag == "1")
                            flag = "Cancelled";
                        else
                            flag = "Completed";
                        dr["SNo"] = Convert.ToString(sno);
                        dr["AccessNo"] = raccno;
                        dr["Title"] = rtitle;
                        dr["Author"] = rauthor;
                        if (r_rollno != " Nil")
                        {
                            dr["Roll No"] = r_rollno;

                            stdstaffname = d2.GetFunction("Select isnull(stud_name,'') as SName from registration where Roll_No='" + r_rollno + "'");
                            if (stdstaffname == "0")
                                stdstaffname = d2.GetFunction("Select isnull(stud_name,'') as SName from registration where Reg_No='" + r_rollno + "'");
                            else if (stdstaffname == "0")
                                stdstaffname = d2.GetFunction("Select isnull(stud_name,'') as SName from registration where lib_id='" + r_rollno + "'");
                        }
                        else if (r_stcode != "Nil")
                        {
                            dr["Roll No"] = r_stcode;

                            stdstaffname = d2.GetFunction("select staff_name from staffmaster where staffmaster.staff_code= '" + r_stcode + "'or staffmaster.lib_id= '" + r_stcode + "'");
                        }
                        else
                        {
                            dr["Roll No"] = "";
                        }
                        dr["Name"] = stdstaffname;
                        dr["Req.Date"] = rcurdae;
                        dr["Req.Time"] = rcutime;
                        dr["Status"] = flag;
                        bokres.Rows.Add(dr);
                    }
                    chkGridSelectAll.Visible = true;
                    grdBkReserve.DataSource = bokres;
                    grdBkReserve.DataBind();
                    grdBkReserve.Visible = true;

                    for (int l = 0; l < grdBkReserve.Rows.Count; l++)
                    {
                        foreach (GridViewRow row in grdBkReserve.Rows)
                        {
                            foreach (TableCell cell in row.Cells)
                            {
                                grdBkReserve.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                grdBkReserve.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                                grdBkReserve.Rows[l].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    chkGridSelectAll.Visible = true;
                    grdBkReserve.Visible = true;
                    btn_cancel_res.Visible = true;
                    //select_range.Visible = true;;
                    string LinkValDel = d2.GetFunction("select res_dele from lib_user_perm where user_code = '" + userCode + "'");
                    if (LinkValDel == "1")
                    {
                        btn_delete.Visible = true;
                    }
                    else
                    {
                        btn_delete.Visible = true;
                        btn_delete.Enabled = false;
                    }
                    string LinkValPrint = d2.GetFunction("select res_print from lib_user_perm where user_code = '" + userCode + "'");
                    if (LinkValPrint == "1")
                    {
                        rptprint.Visible = true;
                    }
                    else
                    {
                        rptprint.Visible = true;
                        btnExcel.Enabled = false;
                        btnprintmaster.Enabled = false;
                        txtexcelname.Enabled = false;
                    }
                }

                RowHead(grdBkReserve);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }

            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    protected void RowHead(GridView grdBkReserve)
    {
        for (int head = 0; head < 1; head++)
        {
            grdBkReserve.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grdBkReserve.Rows[head].Font.Bold = true;
            grdBkReserve.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    protected void grdBkReserve_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowIndex == 0)
        {
            e.Row.Cells[0].Text = "Select";
        }
    }

    protected void grdBkReserve_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdBkReserve_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            AddpopupReserve.Visible = true;
            btn_save.Visible = true;
            btn_save.Text = "Update";
            btn_exit.Visible = true;
            loaduserEntry();


            string type = "";
            string libname = "";
            string getupdatebookqry = "";
            DataSet dsgetupdatebook = new DataSet();
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            if (ddl_txt_lib.Items.Count > 0)
            {
                libname = Convert.ToString(ddl_txt_lib.SelectedItem.Text);
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            }

            if (Convert.ToString(rowIndex) != "")
            {
                txt_accno.Text = Convert.ToString(grdBkReserve.Rows[rowIndex].Cells[2].Text);
                txt_roll.Text = Convert.ToString(grdBkReserve.Rows[rowIndex].Cells[5].Text);
                txt_name.Text = Convert.ToString(grdBkReserve.Rows[rowIndex].Cells[6].Text);
                txt_title.Text = Convert.ToString(grdBkReserve.Rows[rowIndex].Cells[3].Text);
                txt_date.Text = Convert.ToString(grdBkReserve.Rows[rowIndex].Cells[7].Text);
            }


        }


        catch
        {
        }
    }



    #endregion

    #region Delete
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {

            if (grdBkReserve.Rows.Count > 0)
            {


                foreach (GridViewRow row in grdBkReserve.Rows)
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                    if (!cbsel.Checked)
                        continue;
                    else
                        selectedcount++;
                }
                if (selectedcount == 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select atleast one entry to delete";
                    return;
                }
                else
                {
                    Diveleterecord.Visible = true;
                    lbl_Diveleterecord.Text = "Are you sure to delete the selected record?";
                }

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void btn_cancel_res_Click(object sender, EventArgs e)
    {
        try
        {
            if (grdBkReserve.Rows.Count > 0)
            {


                foreach (GridViewRow row in grdBkReserve.Rows)
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                    if (!cbsel.Checked)
                        continue;
                    else
                        selectedcount++;
                }

                if (selectedcount == 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select atleast one entry to delete";
                    return;
                }
                else
                {
                    Diveleterecord.Visible = true;
                    lbl_Diveleterecord.Text = "Do you want to cancel book reservation?";
                }

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void btn_detele_yes__record_Click(object sender, EventArgs e)
    {
        try
        {
            string getbook = "";
            string getbook1 = "";
            string deletebook = "";
            int deletere = 0;
            string title1 = "";
            string roll_no = "";
            string Std_Acc_no = "";
            string Std_stat = "";
            Diveleterecord.Visible = false;
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (grdBkReserve.Rows.Count > 0)
            {

                if (lbl_Diveleterecord.Text == "Are you sure to delete the selected record?")
                {
                    foreach (GridViewRow row in grdBkReserve.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        if (!cbsel.Checked)
                            continue;

                        title1 = Convert.ToString(row.Cells[3].Text);
                        if (title1 == "&nbsp;")
                        {
                            title1 = "";
                        }
                        roll_no = Convert.ToString(row.Cells[5].Text);
                        if (roll_no == "&nbsp;")
                        {
                            roll_no = "";
                        }
                        //string Staff_code = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag);
                        deletebook = "DELETE FROM priority_studstaff where  roll_no='" + roll_no + "' and title ='" + title1 + "'";
                        deletebook += "DELETE FROM priority_studstaff where  staff_code='" + roll_no + "' and title='" + title1 + "'";
                        deletere = d2.update_method_wo_parameter(deletebook, "Text");

                    }
                }
                else
                {
                    foreach (GridViewRow row in grdBkReserve.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        if (!cbsel.Checked)
                            continue;
                        Std_Acc_no = Convert.ToString(row.Cells[2].Text);
                        roll_no = Convert.ToString(row.Cells[5].Text);
                        if (roll_no == "&nbsp;")
                        {
                            roll_no = "";
                        }
                        Std_stat = Convert.ToString(row.Cells[9].Text);
                        if (Std_stat == "&nbsp;")
                        {
                            Std_stat = "";
                        }
                        if (Std_stat == "1")
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Select the reserved books for cancellation";
                            return;
                        }
                        else
                        {
                            deletebook = "update priority_studstaff set cancel_flag=1,priorityno=0 where roll_no='" + roll_no + "' and access_number ='" + Std_Acc_no + "'";
                            deletebook += "update priority_studstaff set cancel_flag=1,priorityno=0 where staff_code='" + roll_no + "'";
                            deletere = d2.update_method_wo_parameter(deletebook, "Text");
                        }

                    }
                }
            }
            if (deletere > 0)
            {
                if (lbl_Diveleterecord.Text == "Are you sure to delete the selected record?")
                {
                    DivAlertcancel.Visible = true;
                    Labelalerterror.Text = "Record deleted successfully";
                }
                else
                {
                    DivAlertcancel.Visible = true;
                    Labelalerterror.Text = "Reservation successfully cancelled";
                }
                btngo_Click(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    protected void btn_detele_no__recordClick(object sender, EventArgs e)
    {

        try
        {
            Diveleterecord.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }
    #endregion

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Book_Reservation_Report";
            string pagename = "Book_Reservation.aspx";
            string ss = Session["usercode"].ToString();

            Printcontrolhed2.loadspreaddetails(grdBkReserve, pagename, degreedetails, 0, ss);
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdBkReserve, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
    #endregion

    #region Add
    protected void btnadd_Click(object sender, EventArgs e)
    {
        AddpopupReserve.Visible = true;
        loaduserEntry();
        if (ddllibrary.Items.Count > 0)
            libname = Convert.ToString(ddllibrary.SelectedItem.Text);
        Acclibcode = Convert.ToString(ddl_txt_lib.SelectedItem.Text);
        if (libname == "")
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Please Select Library";
            return;
        }

        rblstustaff.Items[0].Selected = true;
        rblstustaff.Items[1].Selected = false;
        lbl_roll.Text = "Roll No:";
        txt_roll.Text = "";
        txt_name.Text = "";
        txt_accno.Text = "";
        txt_title.Text = "";
        btn_save.Visible = true;
        btn_save.Text = "Save";
        //DateTime FromTime = DateTime.Now;
        //MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
        //if (FromTime.ToString("tt") == "AM")
        //{
        //    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
        //}
        //else
        //{
        //    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
        //}
        //txt_time.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
    }
    #endregion

    #region Add_Popup

    public void loaduserEntry()
    {
        try
        {
            ddl_userentry.Items.Clear();
            ddl_userentry.Items.Add("Library ID");
            ddl_userentry.Items.Add("BioMetric");
            ddl_userentry.Items.Add("Register Number");
            ddl_userentry.Items.Add("Roll Number");
            ddl_userentry.Items.Add("Smart Card");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void rblstustaff_Selected(object sender, EventArgs e)
    {
        try
        {
            if (rblstustaff.SelectedIndex == 0)
            {
                lbl_roll.Text = "Roll No:";
                txt_name.Text = "";
                txt_roll.Text = "";
                txt_accno.Text = "";
                txt_title.Text = "";
            }
            else
            {
                lbl_roll.Text = "Staff Code:";
                txt_name.Text = "";
                txt_roll.Text = "";
                txt_accno.Text = "";
                txt_title.Text = "";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void ddl_userentry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rblstustaff.SelectedIndex == 0)
            {
                if (ddl_userentry.Text == "BioMetric" || ddl_userentry.Text == "Smart Card")
                {
                    btn_libid.Enabled = false;
                }
                else if (ddl_userentry.Text == "Library ID")
                {
                    lbl_roll.Text = "Library ID:";
                    btn_libid.Enabled = true;
                }
                else if (ddl_userentry.Text == "Register Number")
                {
                    lbl_roll.Text = "Register Number:";
                    btn_libid.Enabled = true;
                }
                else
                {
                    lbl_roll.Text = "Roll Number:";
                    btn_libid.Enabled = true;
                }

            }
            else
            {
                if (ddl_userentry.Text == "BioMetric" || ddl_userentry.Text == "Smart Card")
                {
                    btn_libid.Enabled = false;
                }
                else
                    btn_libid.Enabled = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void btn_libid_Click(object sender, EventArgs e)
    {
        try
        {
            if (rblstustaff.SelectedIndex == 0)
            {
                popupselectlibid.Visible = true;
                DivpopupStaff.Visible = false;
                grdStudent.Visible = false;

                btn_std_exit1.Visible = false;
                if (ddl_userentry.Text == "Library ID")
                {
                    lbl_popupselectlibid.Text = "Select Library ID";
                    lbl_lib_id.Text = "Library ID";

                }
                if (ddl_userentry.Text == "Register Number")
                {
                    lbl_popupselectlibid.Text = "Select Register Number";
                    lbl_lib_id.Text = "Reg No";

                }
                if (ddl_userentry.Text == "Roll Number")
                {
                    lbl_popupselectlibid.Text = "Select Roll Number";
                    lbl_lib_id.Text = "Roll No";
                }
            }
            else
            {
                loadstaff_dept();
                grdStaff.Visible = false;
                btn_staff_exit1.Visible = false;
                popupselectlibid.Visible = false;
                DivpopupStaff.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    protected void btn_accno_Click(object sender, EventArgs e)
    {
        try
        {
            popupselectlibid.Visible = false;
            popupselectBook.Visible = true;
            load_Search();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetrollno = new DataSet();
            string accessdate = "";
            bool sendsms = false;
            string user_id = string.Empty;
            string ssr = "";
            int stdorstaffsave = 0;
            string StrMsg = "";
            string StrMobileNo = "";
            string StrCollCode = "";
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            string date = Convert.ToString(txt_date.Text);
            string[] adate = date.Split('/');
            if (adate.Length == 3)
                accessdate = adate[2].ToString() + "/" + adate[1].ToString() + "/" + adate[0].ToString();
            string Currentdate = DateTime.Now.ToString("MM/dd/yyyy");
            string Acctime = DateTime.Now.ToString("hh:mm:ss tt");
            double Priority = 0;
            //DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", txt_time.Hour, txt_time.Minute, txt_time.Second, txt_time.AmPm));
            if (txt_accno.Text != "")
            {
                string getaccno = d2.GetFunction("select top 1 PriorityNo from priority_studstaff where access_number='" + txt_accno.Text + "' and cancel_flag=0 order by PriorityNo desc");
                string getaccno1 = d2.GetFunction("select top 1 PriorityNo accnt from priority_studstaff where Otheracc_no='" + txt_accno.Text + "' and cancel_flag=0 order by PriorityNo desc");
                if (getaccno != "" && getaccno != "0")
                {
                    //alertpopwindow.Visible = true;
                    //lblalerterr.Text = "Already Reserved";
                    //return;
                    Priority = Convert.ToDouble(getaccno);
                    Priority = Priority + 1;
                }
                else
                {
                    Priority = Priority + 1;
                }
                // if (getaccno1 != "" && getaccno1 != "0")
                //{
                //    //alertpopwindow.Visible = true;
                //    //lblalerterr.Text = "Already Reserved";
                //    //return;
                //    Priority = Convert.ToDouble(getaccno);
                //    Priority = Priority + 1;
                //}
                //else
                //{
                //    Priority = Priority + 1;
                //}
                //else
                //{
                if (btn_save.Text.ToUpper() == "SAVE")
                {
                    if (rblstustaff.SelectedIndex == 0)
                    {
                        string getstdrollno = "select roll_no,title,lib_code from priority_studstaff where roll_no='" + txt_roll.Text + "' AND Cancel_Flag = 0 ";
                        dsgetrollno.Clear();
                        dsgetrollno = d2.select_method_wo_parameter(getstdrollno, "Text");
                        if (dsgetrollno.Tables[0].Rows.Count == 0)
                        {
                            string stdinsert = "insert into priority_studstaff(cur_date,cur_time,roll_no,staff_code,title,access_date,access_time,lib_code,access_number,cancel_flag,PriorityNo)values('" + accessdate + "','" + txt_time.Text + "','" + txt_roll.Text + "','Nil','" + txt_title.Text + "','" + Currentdate + "','" + Acctime + "','" + libcode + "','" + txt_accno.Text + "',0,'" + Priority + "')";
                            stdorstaffsave = d2.update_method_wo_parameter(stdinsert, "Text");
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Sorry!Only One book can be reserved by a student.Previous reservation details :  Title: " + txt_title.Text + "Library: " + ddl_txt_lib.Items[0].Text + "";
                            return;

                        }
                    }
                    else
                    {
                        string stafftitle = "";
                        string stafftitle1 = "";
                        string getstafcode = "select staff_code,title,lib_code from priority_studstaff where staff_code='" + txt_roll.Text + "'";
                        dsgetrollno.Clear();
                        dsgetrollno = d2.select_method_wo_parameter(getstafcode, "Text");
                        if (dsgetrollno.Tables[0].Rows.Count == 2)
                        {
                            for (int row = 0; row < dsgetrollno.Tables[0].Rows.Count; row++)
                            {
                                string sttitle = Convert.ToString(dsgetrollno.Tables[0].Rows[row]["title"]).Trim();
                                if (stafftitle == "")
                                    stafftitle = sttitle;
                                else
                                    stafftitle1 = stafftitle1 + "," + sttitle;
                            }
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Sorry!Only Two books can be reserved by a staff.Previous reservation details :   Title: " + stafftitle1 + "Library: " + ddl_txt_lib.Items[0].Text + "";
                            return;
                        }
                        else
                        {
                            string getstaffcode = "select staff_code,title,lib_code from priority_studstaff where staff_code='" + txt_roll.Text + "' and title='" + txt_title.Text + "' and cancel_flag<>1";
                            dsgetrollno.Clear();
                            dsgetrollno = d2.select_method_wo_parameter(getstafcode, "Text");
                            if (dsgetrollno.Tables[0].Rows.Count == 0)
                            {
                                string staffinsert = "insert into priority_studstaff(cur_date,cur_time,roll_no,staff_code,title,access_date,access_time,lib_code,access_number,cancel_flag,PriorityNo)values('" + accessdate + "','" + txt_time.Text + "','Nil','" + txt_roll.Text + "','" + txt_title.Text + "','" + Currentdate + "','" + Acctime + "','" + libcode + "','" + txt_accno.Text + "',0,'" + Priority + "')";
                                stdorstaffsave = d2.update_method_wo_parameter(staffinsert, "Text");
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "This Book has been already reserved by the same staff.Reserve some other books.";
                                return;

                            }
                        }

                    }
                    if (stdorstaffsave > 0)
                    {
                        StrMsg = "Book reservation made sucessfully, Access No. " + txt_accno.Text + ",Date =" + accessdate + "";
                        if (rblstustaff.SelectedIndex == 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Student Reservation Saved Successfully";
                            StrMobileNo = d2.GetFunction("SELECT Student_Mobile FROM Registration R,Applyn A WHERE R.App_No = A.App_No AND Roll_No ='" + txt_roll.Text + "' ");
                            StrCollCode = d2.GetFunction("SELECT G.College_Code FROM Registration R,Applyn A,Degree G WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND Roll_No ='" + txt_roll.Text + "'");
                            ssr = "select * from Track_Value where college_code='" + Convert.ToString(userCollegeCode) + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(ssr, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                            }
                            int d = d2.send_sms(user_id, StrCollCode, userCode, StrMobileNo, StrMsg, "0");
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Staff Reservation Saved Successfully";

                            StrMobileNo = d2.GetFunction("SELECT Per_MobileNo FROM Staff_Appl_Master A,StaffMaster M WHERE A.Appl_No = M.Appl_No AND Staff_Code ='" + txt_roll.Text + "' ");
                            StrCollCode = d2.GetFunction("SELECT College_Code FROM StaffMaster M WHERE Staff_Code='" + txt_roll.Text + "'");
                            ssr = "select * from Track_Value where college_code='" + Convert.ToString(userCollegeCode) + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(ssr, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
                            }
                            int d = d2.send_sms(user_id, StrCollCode, userCode, StrMobileNo, StrMsg, "1");

                        }
                    }
                }
                else
                {
                    string Sqlgetre = "";
                    string Sqlupdate = "";
                    int stdorstaffupdate = 0;
                    if (rblstustaff.SelectedIndex == 0)
                    {

                        Sqlgetre = d2.GetFunction("Select tokendetails.stud_name from tokendetails,registration where tokendetails.roll_no='" + txt_roll.Text + "'  and delflag=0 and is_staff=0 ");
                        if (Sqlgetre != "0")
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Check rollno";
                            txt_roll.Text = "";
                            return;
                        }
                        else
                        {
                            Sqlupdate = "update priority_studstaff set cur_date='" + accessdate + "',cur_time='" + txt_time.Text + "',title='" + txt_title.Text + "',access_number='" + txt_accno.Text + "',cancel_flag=0 where roll_no='" + txt_roll.Text + "' and lib_code='" + libcode + "'";
                            stdorstaffupdate = d2.update_method_wo_parameter(Sqlupdate, "Text");

                        }
                    }
                    else
                    {
                        Sqlgetre = d2.GetFunction("Select tokendetails.stud_name from tokendetails,staffmaster where tokendetails.roll_no='" + txt_roll.Text + "' and  resign=0 and is_staff=1");
                        if (Sqlgetre != "0")
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Check Staff Code";
                            txt_roll.Text = "";
                            return;

                        }
                        else
                        {
                            string strcnt = d2.GetFunction("select count(*) from priority_studstaff where staff_code='" + txt_roll.Text + "' and title='" + txt_title.Text + "'");
                            if (Convert.ToInt32(strcnt) > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "This Book has been already reserved by the same staff.Reserve some other books.";
                                return;
                            }
                            else
                            {
                                Sqlupdate = "update priority_studstaff set cur_date='" + accessdate + "',cur_time='" + txt_time.Text + "',title='" + txt_title.Text + "', access_number='" + txt_accno.Text + "',cancel_flag=0 where staff_code='" + txt_roll.Text + "' and lib_code='" + libcode + "'";
                                stdorstaffupdate = d2.update_method_wo_parameter(Sqlupdate, "Text");
                            }

                        }

                    }
                    if (stdorstaffupdate > 0)
                    {
                        if (rblstustaff.SelectedIndex == 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Student Reservation Updated Successfully";

                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Staff Reservation Updated Successfully";



                        }

                    }

                }
                // }

            }
        }
        catch (Exception ex)
        {
            //{ d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
        }

    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        AddpopupReserve.Visible = false;
    }

    #endregion

    #region select_libid_popup_Student

    public void bindbatch()
    {
        try
        {

            ddlbatch.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlbatch.SelectedValue = max_bat.ToString();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            binddegree();
            bindsem();
            bindsec();
            grdStudent.Visible = false;

            btn_std_exit1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    public void binddegree()
    {
        try
        {

            ddldegree.Items.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleUser);
            has.Add("group_code", groupUserCode);
            has.Add("college_code", userCollegeCode);
            has.Add("user_code", userCode);
            ds = da.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            bindsem();
            bindsec();
            grdStudent.Visible = false;

            btn_std_exit1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    public void bindbranch()
    {
        try
        {

            ddlsem.Items.Clear();
            has.Clear();
            userCode = Session["usercode"].ToString();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            singleUser = Session["single_user"].ToString();
            groupUserCode = Session["group_code"].ToString();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = group_semi[0].ToString();
            }
            has.Add("single_user", singleUser);
            has.Add("group_code", groupUserCode);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", userCollegeCode);
            has.Add("user_code", userCode);
            ds = da.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsec();
            grdStudent.Visible = false;

            btn_std_exit1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    public void bindsem()
    {
        try
        {

            ddlsem.Items.Clear();
            string duration = string.Empty;
            Boolean first_year = false;
            has.Clear();
            userCollegeCode = ddlCollege.SelectedItem.Value;
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("batch_year", ddlbatch.SelectedValue.ToString());
            has.Add("college_code", userCollegeCode);
            ds = da.select_method("bind_sem", has, "sp");
            int count3 = ds.Tables[0].Rows.Count;
            if (count3 > 0)
            {
                ddlsem.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                count3 = ds.Tables[1].Rows.Count;
                if (count3 > 0)
                {
                    ddlsem.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    ddlsem.Enabled = false;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            grdStudent.Visible = false;

            btn_std_exit1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    public void bindsec()
    {
        try
        {

            ddlSec.Items.Clear();
            hat.Clear();
            hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
            hat.Add("degree_code", ddlbranch.SelectedValue);
            ds = da.select_method("bind_sec", hat, "sp");
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
            }
            ddlSec.Items.Add("All");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    protected void btn_go_libid_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetbook = new DataSet();
            dsgetbook = getStudentdetails();
            if (dsgetbook.Tables.Count > 0 && dsgetbook.Tables[0].Rows.Count > 0)
            {
                loadspreadstddetails(dsgetbook);
            }
            else
            {
                grdStudent.Visible = false;
                btn_std_exit1.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    private DataSet getStudentdetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value

            string sqlgetstddetails = "";
            string collcode = "";
            string batch = "";
            string courseid = "";
            string bran = "";
            string sem = "";
            string sec = "";
            string Section = "";
            string strID = "";
            string strStaffID = "";
            string stdID = "";
            string txtid = "";
            string txtname = "";


            string value = d2.GetFunction("select * from inssettings where linkname ='Library id'");
            if (value != "")
            {
                if (value == "0")
                {
                    strID = "R.roll_no";
                    strStaffID = "staffmaster.staff_code";
                }
                else
                {
                    strID = "R.lib_id";
                    strStaffID = "staffmaster.lib_id";
                }
            }
            else
            {
                strID = "R.roll_no";
                strStaffID = "staffmaster.staff_code";
            }


            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddlbatch.Items.Count > 0)
                batch = Convert.ToString(ddlbatch.SelectedValue);
            if (ddldegree.Items.Count > 0)
                courseid = Convert.ToString(ddldegree.SelectedValue);
            if (ddlbranch.Items.Count > 0)
                bran = Convert.ToString(ddlbranch.SelectedValue);
            if (ddlsem.Items.Count > 0)
                sem = Convert.ToString(ddlsem.SelectedValue);
            if (ddlSec.Items.Count > 0)
                sec = Convert.ToString(ddlSec.SelectedValue).Trim();

            if (sec == "" || sec == "All")
                Section = "";
            else
                Section = "and R.sections='" + sec + "'";
            if (rblstustaff.SelectedIndex == 0)
            {
                if (lbl_lib_id.Text == "Library ID")
                {
                    stdID = "R.lib_id";
                    if (tx_libid.Text != "")
                        txtid = "and R.lib_id='" + tx_libid.Text + "'";
                    if (tx_libname.Text != "")
                        txtname = "and R.Stud_Name='" + tx_libname.Text + "'";
                }
                else if (lbl_lib_id.Text == "Reg No")
                {
                    stdID = "R.reg_no";
                    if (tx_libid.Text != "")
                        txtid = "and R.reg_no='" + tx_libid.Text + "'";
                    if (tx_libname.Text != "")
                        txtname = "and R.Stud_Name='" + tx_libname.Text + "'";
                }
                else if (lbl_lib_id.Text == "Roll No")
                {
                    stdID = "R.roll_no";
                    if (tx_libid.Text != "")
                        txtid = "and R.roll_no='" + tx_libid.Text + "'";
                    if (tx_libname.Text != "")
                        txtname = "and R.Stud_Name='" + tx_libname.Text + "'";
                }
            }
            if (!string.IsNullOrEmpty(collcode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(courseid) && !string.IsNullOrEmpty(bran) && !string.IsNullOrEmpty(sem))
            {
                sqlgetstddetails = "SELECT distinct " + stdID + ", R.Stud_Name, C.Course_Name + '-' + D.Dept_Name as Degree, R.Current_Semester FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND G.College_Code = D.College_Code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and R.batch_year='" + batch + "' and G.Degree_Code='" + bran + "' AND C.Course_Id='" + courseid + "'  and C.college_code='" + collcode + "' and R.Current_Semester='" + sem + "' " + Section + " " + txtid + txtname + " order by " + stdID + "";

            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(sqlgetstddetails, "Text");


            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


        return dsload;


    }

    public void loadspreadstddetails(DataSet ds)
    {
        try
        {
            if (lbl_lib_id.Text == "Library ID")
                studdetails.Columns.Add("Library ID", typeof(string));
            else if (lbl_lib_id.Text == "Reg No")
                studdetails.Columns.Add("Register No", typeof(string));

            else
                studdetails.Columns.Add("Roll No", typeof(string));

            studdetails.Columns.Add("Name", typeof(string));
            studdetails.Columns.Add("Degree", typeof(string));
            int sno = 0;
            string id = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drdet = studdetails.NewRow();
                    if (lbl_lib_id.Text == "Library ID")
                        id = Convert.ToString(ds.Tables[0].Rows[row]["lib_id"]).Trim();
                    else if (lbl_lib_id.Text == "Reg No")
                        id = Convert.ToString(ds.Tables[0].Rows[row]["reg_no"]).Trim();
                    else
                        id = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]).Trim();

                    string name = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]).Trim();
                    string degre = Convert.ToString(ds.Tables[0].Rows[row]["Degree"]).Trim();
                    if (lbl_lib_id.Text == "Library ID")
                        drdet["Library ID"] = id;

                    else if (lbl_lib_id.Text == "Reg No")
                        drdet["Register No"] = id;
                    else
                        drdet["Roll No"] = id;
                    drdet["Name"] = name;
                    drdet["Degree"] = degre;
                    studdetails.Rows.Add(drdet);
                }
                divRollNo.Visible = true;
                grdStudent.DataSource = studdetails;
                grdStudent.DataBind();
                grdStudent.Visible = true;

                for (int l = 0; l < grdStudent.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdStudent.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdStudent.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
            btn_std_exit1.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    protected void grdStudent_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdStudent_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            selectedCellIndex = int.Parse(this.SelectedGridCell.Value);
            string idorno = grdStudent.Rows[rowIndex].Cells[1].Text;
            string stdname = grdStudent.Rows[rowIndex].Cells[2].Text;
            txt_roll.Text = idorno;
            txt_name.Text = stdname;
            popupselectlibid.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_std_exit1_Click(object sender, EventArgs e)
    {
        popupselectlibid.Visible = false;
    }

    #endregion

    #region Select_staffcode_popup

    public void loadstaff_dept()
    {
        try
        {
            ddl_staffdept.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                SelectQ = "select dept_name,dept_code  from hrdept_master where college_code='" + College + "' order by dept_name";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelectQ, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_staffdept.DataSource = ds;
                    ddl_staffdept.DataTextField = "dept_name";
                    ddl_staffdept.DataValueField = "dept_code";
                    ddl_staffdept.DataBind();
                }
                ddl_staffdept.Items.Insert(0, "All");
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void ddl_staffdept_SelectedIndexChanged(object sendre, EventArgs e)
    {

    }

    protected void btn_staff_Go_Click(object sendre, EventArgs e)
    {
        try
        {

            DataSet dsgetsatff = new DataSet();
            dsgetsatff = getStaffdetails();
            if (dsgetsatff.Tables.Count > 0 && dsgetsatff.Tables[0].Rows.Count > 0)
            {
                loadspreadstaffdetails(dsgetsatff);
            }
            else
            {
                grdStaff.Visible = false;

                btn_staff_exit1.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }

    }

    //protected void grdStaff_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    grdStaff.PageIndex = e.NewPageIndex;
    //    btn_staff_Go_Click(sender, e);
    //}

    private DataSet getStaffdetails()
    {
        DataSet dsload1 = new DataSet();
        try
        {
            #region get Value

            string sqlgetstadetails = "";
            string strStaffID = "";
            string staffdeptcode = "";
            string stafftxt = "";
            string staffdept = "";

            string value = d2.GetFunction("select * from inssettings where linkname ='Library id'");
            if (value != "")
            {
                if (value == "0")
                    strStaffID = "staffmaster.staff_code";

                else
                    strStaffID = "staffmaster.lib_id";
            }
            else
                strStaffID = "staffmaster.staff_code";

            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddl_staffdept.Items.Count > 0)
                staffdeptcode = Convert.ToString(ddl_staffdept.SelectedValue);
            if (staffdeptcode != "" && staffdeptcode != "All")
                staffdept = "and hm.dept_code='" + staffdeptcode + "'";
            if (txt_staffname.Text != "")
            {
                stafftxt = "AND  staff_name='" + txt_staffname.Text + "'";

            }

            if (!string.IsNullOrEmpty(collcode) && !string.IsNullOrEmpty(staffdeptcode))
            {
                sqlgetstadetails = "SELECT distinct sm.staff_code,sm.staff_name,hm.dept_name  From staffmaster sm, stafftrans st, hrdept_master hm WHERE sm.staff_code = st.staff_code AND st.dept_code = hm.dept_code " + stafftxt + "  AND sm.resign = 0 and settled = 0 and latestrec = 1 and sm.college_code='" + collcode + "' " + staffdept + stafftxt + "";
            }
            dsload1.Clear();
            dsload1 = d2.select_method_wo_parameter(sqlgetstadetails, "Text");


            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


        return dsload1;


    }

    public void loadspreadstaffdetails(DataSet ds)
    {
        try
        {
            bokstaff.Columns.Add("Staff Code", typeof(string));
            bokstaff.Columns.Add("Staff Name", typeof(string));
            bokstaff.Columns.Add("Department", typeof(string));

            int sno = 0;
            string id = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drbokstaff = bokstaff.NewRow();
                    id = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]).Trim();
                    string stname = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]).Trim();
                    string dept = Convert.ToString(ds.Tables[0].Rows[row]["dept_name"]).Trim();
                    drbokstaff["Staff Code"] = id;
                    drbokstaff["Staff Name"] = stname;
                    drbokstaff["Department"] = dept;
                    bokstaff.Rows.Add(drbokstaff);
                }
                grdStaff.DataSource = bokstaff;
                grdStaff.DataBind();
                grdStaff.Visible = true;
                btn_staff_exit1.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    protected void grdStaff_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenFieldgrdStaff.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdStaff_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            selectedCellIndex = int.Parse(this.HiddenFieldgrdStaff.Value);
            string staffids = grdStaff.Rows[rowIndex].Cells[1].Text;
            string staname = grdStaff.Rows[rowIndex].Cells[2].Text;
            txt_roll.Text = staffids;
            txt_name.Text = staname;
            DivpopupStaff.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_staff_exit1_Click(object sender, EventArgs e)
    {
        try
        {
            DivpopupStaff.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    #endregion

    #region Select_Accno

    public void load_Search()
    {
        try
        {
            dd_search.Items.Clear();
            dd_search.Items.Add("All");
            dd_search.Items.Add("Access Number");
            dd_search.Items.Add("Title");
            dd_search.Items.Add("Author");
            dd_search.Items.Add("Publisher");
            dd_search.Items.Add("Edition");

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void dd_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdBook.Visible = false;

            btn_Acc_exit1.Visible = false;
            txt_search_book.Text = "";
            if (dd_search.Text == "All")
                txt_search_book.Visible = false;
            else
                txt_search_book.Visible = true;

            searchby = dd_search.SelectedIndex;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void btn_go_book_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetaccno = new DataSet();
            string search1 = "";
            if (dd_search.Items.Count > 0)
                search1 = Convert.ToString(dd_search.SelectedValue);
            if (search1 != "" && search1 != "All")
            {
                if (txt_search_book.Text == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Enter " + search1 + "";
                    return;
                }
            }
            dsgetaccno = getaccessnodetails();
            if (dsgetaccno.Tables.Count > 0 && dsgetaccno.Tables[0].Rows.Count > 0)
            {
                loadspreadaccnodetails(dsgetaccno);
            }
            else
            {
                grdBook.Visible = false;

                btn_Acc_exit1.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    private DataSet getaccessnodetails()
    {
        DataSet dsload2 = new DataSet();
        try
        {
            #region get Value

            string sqlgetaccno = "";
            string search = "";
            string libcode = "";
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (dd_search.Items.Count > 0)
                search = Convert.ToString(dd_search.SelectedValue);
            if (!string.IsNullOrEmpty(collcode) && !string.IsNullOrEmpty(libcode))
            {
                if (search == "All")
                {
                    sqlgetaccno = "select bookdetails.acc_no,bookdetails.title,bookdetails.author,roll_no + '-' + stud_name as stud_name,bookdetails.publisher,bookdetails.edition from bookdetails,borrow where bookdetails.acc_no=borrow.acc_no and borrow.return_flag=0 and bookdetails.lib_code=borrow.lib_code and bookdetails.lib_code='" + libcode + "' ";//bookdetails.book_status not in(select book_status from bookdetails where book_status='Available' and author = bookdetails.author AND lib_code='" + libcode + "' and bookdetails.lib_code='" + libcode + "')
                }
                else
                {
                    if (txt_search_book.Text != "")
                    {
                        if (search == "Edition")
                        {
                            sqlgetaccno = "select bookdetails.acc_no,bookdetails.title,bookdetails.author,roll_no + '-' + stud_name as stud_name,bookdetails.publisher,bookdetails.edition from bookdetails,borrow where bookdetails.acc_no=borrow.acc_no and borrow.return_flag=0 and bookdetails.lib_code=borrow.lib_code  and bookdetails.lib_code='" + libcode + "' and edition='" + txt_search_book.Text + "'";//and bookdetails.acc_no not in(select acc_no from bookdetails where book_status='Available' and author = bookdetails.author AND lib_code='" + libcode + " ')
                        }
                        else if (search == "Access Number")
                        {
                            sqlgetaccno = "select bookdetails.acc_no,bookdetails.title,bookdetails.author,roll_no + '-' + stud_name as stud_name,bookdetails.publisher,bookdetails.edition from bookdetails,borrow where bookdetails.acc_no=borrow.acc_no and borrow.return_flag=0 and bookdetails.lib_code=borrow.lib_code and bookdetails.lib_code='" + libcode + "' and bookdetails.acc_no='" + txt_search_book.Text + "'";//and bookdetails.acc_no not in(select acc_no from bookdetails where book_status='Available' and author = bookdetails.author AND lib_code='" + libcode + " ')
                        }
                        else
                        {
                            sqlgetaccno = "select bookdetails.acc_no,bookdetails.title,bookdetails.author,roll_no + '-' + stud_name as stud_name,bookdetails.publisher,bookdetails.edition from bookdetails,borrow where bookdetails.acc_no=borrow.acc_no and borrow.return_flag=0 and bookdetails.lib_code=borrow.lib_code and bookdetails.lib_code='" + libcode + "' and bookdetails.acc_no='" + txt_search_book.Text + "'";//and bookdetails.acc_no not in(select acc_no from bookdetails where book_status='Available' and author = bookdetails.author AND lib_code='" + libcode + " ')
                        }
                    }
                }
            }
            dsload2.Clear();
            dsload2 = d2.select_method_wo_parameter(sqlgetaccno, "Text");

            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
        return dsload2;
    }

    public void loadspreadaccnodetails(DataSet ds)
    {
        try
        {
            bokaccess.Columns.Add("Access No", typeof(string));
            bokaccess.Columns.Add("Title", typeof(string));
            bokaccess.Columns.Add("Author", typeof(string));
            bokaccess.Columns.Add("BookHolderRollNo", typeof(string));
            bokaccess.Columns.Add("Publisher", typeof(string));
            bokaccess.Columns.Add("Edition", typeof(string));
            int sno = 0;
            string id = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drbokacc = bokaccess.NewRow();
                    string accno = Convert.ToString(ds.Tables[0].Rows[row]["acc_no"]).Trim();
                    string title = Convert.ToString(ds.Tables[0].Rows[row]["title"]).Trim();
                    string author = Convert.ToString(ds.Tables[0].Rows[row]["author"]).Trim();
                    string stdname = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]).Trim();
                    string publish = Convert.ToString(ds.Tables[0].Rows[row]["publisher"]).Trim();
                    string edition = Convert.ToString(ds.Tables[0].Rows[row]["edition"]).Trim();
                    drbokacc["Access No"] = accno;
                    drbokacc["Title"] = title;
                    drbokacc["Author"] = author;
                    drbokacc["BookHolderRollNo"] = stdname;
                    drbokacc["Publisher"] = publish;
                    drbokacc["Edition"] = edition;
                    bokaccess.Rows.Add(drbokacc);
                }
                grdBook.DataSource = bokaccess;
                grdBook.DataBind();
                grdBook.Visible = true;
                btn_Acc_exit1.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }
    }

    protected void grdBook_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdBook_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            selectedCellIndex = int.Parse(this.HiddenFieldgrdBook.Value);
            string txtaccno = grdBook.Rows[rowIndex].Cells[1].Text;
            string txttitle = grdBook.Rows[rowIndex].Cells[2].Text;
            txt_accno.Text = txtaccno;
            txt_title.Text = txttitle;
            popupselectBook.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_Acc_exit1_Click(object sender, EventArgs e)
    {
        try
        {
            popupselectBook.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Reservation"); }


    }

    protected void txt_accno_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string lc = "";
            if (ddl_txt_lib.Items.Count > 0)
                lc = Convert.ToString(ddl_txt_lib.SelectedValue);
            if (txt_accno.Text != "")
            {
                string sqlaccno = "SELECT * FROM BookDetails WHERE Acc_No ='" + txt_accno.Text + "' AND Lib_Code ='" + lc + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlaccno, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string Bookst = Convert.ToString(ds.Tables[0].Rows[0]["Book_Status"]);
                    if (Bookst != "Issued")
                    {
                        txt_title.Text = "";
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Reserve only issued book, select issued book";
                        return;
                    }
                    else
                    {
                        txt_title.Text = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Check Access Number";
                    txt_accno.Text = "";
                    txt_title.Text = "";
                    return;
                }
            }
        }
        catch
        {

        }
    }

    #endregion

    #region TrackmasterPopup

    protected void rbltrack_Selected(object sender, EventArgs e)
    {


    }

    protected void ddl_tracklib_SelectedIndexChanged(object sendre, EventArgs e)
    {

    }

    #endregion

    #region Close
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        AddpopupReserve.Visible = false;
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popupselectlibid.Visible = false;
    }

    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popupselectBook.Visible = false;
    }

    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        DivTrackMaster.Visible = false;
    }

    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        DivpopupStaff.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
        lblalerterr.Text = "";
    }

    protected void btnerrclosecancel_Click(object sender, EventArgs e)
    {
        DivAlertcancel.Visible = false;
    }

    #endregion

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
            bindLibrary(LibCollection);

        }
        catch (Exception ex)
        {
        }
    }

}