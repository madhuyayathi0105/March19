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

public partial class LibraryMod_book_circulation_report : System.Web.UI.Page
{
    string collegecode = string.Empty;
    string usercollegecode = string.Empty;
    string usercode = string.Empty;
    string groupusercode = string.Empty;
    string singleusercode = string.Empty;
    string libr_code = string.Empty;
    DataTable bokcir = new DataTable();
    DataRow drbokreport;
    Hashtable ht = new Hashtable();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    DataTable access = new DataTable();
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
          
                usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                usercollegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                groupusercode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
                singleusercode = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
          
            if (!IsPostBack)
            {
                bindclg();
                getLibPrivil();
                binddept();
            }
            searchby();
        }
        catch
        {
        }

    }

    #region Bindheaders

    public void bindclg()
    {
        try
        {
            ddlCollege.Items.Clear();
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", ht, "sp");
            ddlCollege.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = dsprint;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
            }
        }

        catch
        {
        }
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
            if (singleusercode.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + usercode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupusercode.Split(';');
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
            library(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void library(string libcode)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string College = Convert.ToString(ddlCollege.SelectedValue);
            if (!string.IsNullOrEmpty(College))
            {
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib_name, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddllibrary.DataSource = ds;
                ddllibrary.DataTextField = "lib_name";
                ddllibrary.DataValueField = "lib_name";
                ddllibrary.DataBind();
                ddllibrary.SelectedIndex = 0;
            }
        }
        catch
        {
        }
    }

    public void binddept()
    {
        try
        {
            ddldept.Items.Clear();
            ds.Clear();
            collegecode = Convert.ToString(Session["collegecode"]);
            if (!string.IsNullOrEmpty(collegecode))
            {
                string deptcode = "select distinct Dept_Code from bookdetails";

                ds = da.select_method_wo_parameter(deptcode, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldept.DataSource = ds;
                ddldept.DataTextField = "Dept_Code";
                ddldept.DataValueField = "Dept_Code";
                ddldept.DataBind();
                ddldept.Items.Insert(0, "All");
            }

        }
        catch
        {
        }
    }

    public void title()
    {
        ddltitle.Items.Clear();
        ds.Clear();
        collegecode = Convert.ToString(Session["collegecode"]);
        if (!string.IsNullOrEmpty(collegecode))
        {
            string title1 = "select distinct b.title from borrow b,bookdetails bk where b.title=bk.Title  and b.lib_code=bk.Lib_Code";

            ds = da.select_method_wo_parameter(title1, "text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddltitle.DataSource = ds;
            ddltitle.DataTextField = "title";
            ddltitle.DataValueField = "title";
            ddltitle.DataBind();
            //ddltitle.Items.Insert(0, "All");
        }

    }

    public void author()
    {
        ddlauthor.Items.Clear();
        ds.Clear();

        if (!string.IsNullOrEmpty(collegecode))
        {
            string author1 = "select distinct b.author from borrow b,bookdetails bk where b.author=bk.Author  and b.lib_code=bk.Lib_Code";

            ds = da.select_method_wo_parameter(author1, "text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlauthor.DataSource = ds;
            ddlauthor.DataTextField = "author";
            ddlauthor.DataValueField = "author";
            ddlauthor.DataBind();
            ddlauthor.Items.Insert(0, "All");
        }

    }

    public void searchby()
    {
        if (rblStatus.SelectedIndex == 0)
        {
            lblaccno.Visible = true;
            txtaccess.Visible = true;
            btnaccessno.Visible = true;
            lblcriteria.Visible = false;
            ddlcriteria.Visible = false;
            lblauthor.Visible = false;
            ddlauthor.Visible = false;
            ddltitle.Visible = false;
            lbltitle.Visible = false;
            txttit.Visible = false;
            txtauth.Visible = false;
        }
        else if (rblStatus.SelectedIndex == 1)
        {

            lblaccno.Visible = false;
            txtaccess.Visible = false;
            btnaccessno.Visible = false;
            lblcriteria.Visible = true;
            ddlcriteria.Visible = true;
            lblauthor.Visible = true;
            txtauth.Visible = true;
            txttit.Visible = true;
            lbltitle.Visible = true;
            title();
            author();
        }
        else
        {
            lblaccno.Visible = false;
            txtaccess.Visible = false;
            btnaccessno.Visible = false;
            lblcriteria.Visible = false;
            ddlcriteria.Visible = false;
            lblauthor.Visible = false;
            ddlauthor.Visible = false;
            ddltitle.Visible = false;
            lbltitle.Visible = false;
            txttit.Visible = false;
            txtauth.Visible = false;
        }
    }

    #endregion

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddldept_selectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void cbdate1_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbdate1.Checked == true)
        {
            txt_fromdate1.Enabled = true;
            txt_todate1.Enabled = true;
        }
        else
        {
            txt_todate1.Enabled = false;
            txt_fromdate1.Enabled = false;
        }
    }

    protected void btnaccessno_Click(object sender, EventArgs e)
    {
        divaccessno.Visible = true;
        divaccess.Visible = true;
        txtsearch.Visible = false;

    }

    protected void ddllcriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcriteria.SelectedIndex == 0 || ddlcriteria.SelectedIndex == 1 || ddlcriteria.SelectedIndex == 3)
        {
            txttit.Visible = true;
            txtauth.Visible = true;
            ddltitle.Visible = false;
            ddlauthor.Visible = false;
        }
        else
        {
            txttit.Visible = false;
            txtauth.Visible = false;
            ddltitle.Visible = true;
            ddlauthor.Visible = true;
        }
    }

    protected void ddltitle_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    public void bindtitle()
    {

    }

    protected void ddlauthor_SelectedIndexChanged(object sender, EventArgs e)
    {
        txttit.Visible = false;
        txtauth.Visible = false;

    }

    #region ButtonClick

    protected void grdManualExit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void grdManualExit_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdManualExit.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string qry = string.Empty;
            string deptcod = string.Empty;
            string bokrtn = string.Empty;
            if (ddldept.SelectedIndex == 0)
            {
                if (cbdate1.Checked == true)
                {
                    qry = "select b.acc_no, b.Title,b.author,b.roll_no,b.stud_name,b.token_no,convert(nvarchar,borrow_date,105) as 'Issued_Date',convert(nvarchar,due_date,105) as 'Due_Date', case when return_flag = 1 then convert(nvarchar,Return_date,105) else null end as 'Return_Date',b.book_issuedby,b.book_returnby from  borrow b,bookdetails bk where b.acc_no=bk.Acc_No and bk.Lib_Code=b.lib_code and b.borrow_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "'";
                }
                else
                {
                    qry = "select b.acc_no, b.Title,b.author,b.roll_no,b.stud_name,b.token_no,convert(nvarchar,borrow_date,105) as 'Issued_Date',convert(nvarchar,due_date,105) as 'Due_Date', case when return_flag = 1 then convert(nvarchar,Return_date,105) else null end as 'Return_Date',b.book_issuedby,b.book_returnby from  borrow b,bookdetails bk where b.acc_no=bk.Acc_No   and bk.Lib_Code=b.lib_code";
                }
            }
            else
            {
                if (cbdate1.Checked == true)
                {
                    qry = "select b.acc_no, b.Title,b.author,b.roll_no,b.stud_name,b.token_no,convert(nvarchar,borrow_date,105) as 'Issued_Date',convert(nvarchar,due_date,105) as 'Due_Date', case when return_flag = 1 then convert(nvarchar,Return_date,105) else null end as 'Return_Date',b.book_issuedby,b.book_returnby from  borrow b,bookdetails bk where b.acc_no=bk.Acc_No and bk.Lib_Code=b.lib_code and b.borrow_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "' and bk.Dept_Code='" + ddldept.SelectedItem.ToString() + "'";
                }
                else
                {
                    qry = "select b.acc_no, b.Title,b.author,b.roll_no,b.stud_name,b.token_no,convert(nvarchar,borrow_date,105) as 'Issued_Date',convert(nvarchar,due_date,105) as 'Due_Date', case when return_flag = 1 then convert(nvarchar,Return_date,105) else null end as 'Return_Date',b.book_issuedby,b.book_returnby from  borrow b,bookdetails bk where b.acc_no=bk.Acc_No   and bk.Lib_Code=b.lib_code and bk.Dept_Code='" + ddldept.SelectedItem.ToString() + "'";
                }
            }
            if (rblStatus.SelectedIndex == 0)
            {
                qry = qry + " and b.acc_no='" + txtaccess.Text + "'";
            }
            else
            {

                if (ddlcriteria.SelectedIndex == 0)
                {
                    qry = qry + " and b.Title like '" + txttit.Text + "%' and b.author like '" + txtauth.Text + "%' ";
                }
                else if (ddlcriteria.SelectedIndex == 1)
                {
                    qry = qry + " and b.Title like '%" + txttit.Text + "%' and b.author like '%" + txtauth.Text + "%'";
                }
                else if (ddlcriteria.SelectedIndex == 2)
                {
                    if (ddlauthor.SelectedIndex == 0)
                    {
                        qry = qry + " and b.Title='" + Convert.ToString(ddltitle.Text).Trim() + "'";
                    }
                    else
                    {
                        qry = qry + " and b.Title='" + Convert.ToString(ddltitle.Text).Trim() + "' and b.author='" + ddlauthor.SelectedItem.ToString().Trim() + "'";
                    }

                }
                else
                {
                    qry = qry + " and b.Title like '%" + txttit.Text + "' and b.author like '%" + txtauth.Text + "'";
                }

            }
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");

          
          
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                bokcir.Columns.Add("SNo", typeof(string));
                bokcir.Columns.Add("Roll Number", typeof(string));
                bokcir.Columns.Add("Name", typeof(string));
                bokcir.Columns.Add("Token Number", typeof(string));
                bokcir.Columns.Add("Issue Date", typeof(string));
                bokcir.Columns.Add("Due Date", typeof(string));
                bokcir.Columns.Add("Return Date", typeof(string));
                bokcir.Columns.Add("Book Issued By", typeof(string));
                bokcir.Columns.Add("Book Returned By", typeof(string));


                
                int sno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drbokreport = bokcir.NewRow();
                    string rollno = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                    string name = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                    string tokennum = Convert.ToString(ds.Tables[0].Rows[i]["token_no"]);
                    string issuedate = Convert.ToString(ds.Tables[0].Rows[i]["Issued_Date"]);
                    string duedate = Convert.ToString(ds.Tables[0].Rows[i]["Due_Date"]);
                    string returndate = Convert.ToString(ds.Tables[0].Rows[i]["Return_Date"]);
                    string bookissueby = Convert.ToString(ds.Tables[0].Rows[i]["book_issuedby"]);
                    string bookreturnby = Convert.ToString(ds.Tables[0].Rows[i]["book_returnby"]);
                    string[] returnbok = bookreturnby.Split('/');
                    if (returnbok.Length == 2)
                        bokrtn = returnbok[0].ToString();

                    drbokreport["SNo"] = Convert.ToString(sno);
                    drbokreport["Roll Number"] = rollno;
                    drbokreport["Name"] = name;
                    drbokreport["Token Number"] = tokennum;
                    drbokreport["Issue Date"] = issuedate;
                    drbokreport["Due Date"] = duedate;
                    drbokreport["Return Date"] = returndate;
                    drbokreport["Book Issued By"] = bookissueby;
                    drbokreport["Book Returned By"] = bokrtn;
                    bokcir.Rows.Add(drbokreport);
                }

                grdManualExit.DataSource = bokcir;
                grdManualExit.DataBind();
                grdManualExit.Visible = true;
                divtable.Visible = true;
                btn_printmaster.Visible = true;
                btn_Excel.Visible = true;
                txt_excelname.Visible = true;
                div_report.Visible = true;
                lbl_reportname.Visible = true;

              


            }
            else
            {
                divtable.Visible = false;
                grdManualExit.Visible = false;
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                Printcontrol.Visible = false;
                btn_printmaster.Visible = false;
                btn_Excel.Visible = false;
                txt_excelname.Visible = false;
                div_report.Visible = false;
                lbl_reportname.Visible = false;
            }

      
        }
        catch
        {
        }
    }

   

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void GridView1_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdManualExit.PageIndex = e.NewPageIndex;
        btngoaccess_Click(sender, e);
    }

    protected void btngoaccess_Click(object sender, EventArgs e)
    {
        try
        {
              int sno = 0;
            string accno = string.Empty;
            ds.Clear();
            DataRow dr1;
            if (ddlsearch.SelectedIndex == 0)
            {

                accno = "select distinct bk.Acc_No,bk.Title,bk.Author from bookdetails bk,borrow b where b.acc_no=bk.Acc_No and b.title=bk.Title and b.author=bk.Author";

            }
            else if (ddlsearch.SelectedIndex == 1)
            {

                accno = "select distinct bk.Acc_No,bk.Title,bk.Author from bookdetails bk,borrow b where b.acc_no=bk.Acc_No and b.title=bk.Title and b.author=bk.Author and bk.Title like '%" + txtsearch.Text + "%'";
            }
            else if (ddlsearch.SelectedIndex == 2)
            {

                accno = "select distinct bk.Acc_No,bk.Title,bk.Author from bookdetails bk,borrow b where b.acc_no=bk.Acc_No and b.title=bk.Title and b.author=bk.Author and bk.Author like '%" + txtsearch.Text + "%'";
            }
            else
            {

                accno = "select distinct bk.Acc_No,bk.Title,bk.Author from bookdetails bk,borrow b where b.acc_no=bk.Acc_No and b.title=bk.Title and b.author=bk.Author and bk.Acc_No like '%" + txtsearch.Text + "%'";
            }
            ds = da.select_method_wo_parameter(accno, "text");

         
              GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.Visible = true;
                div1acc.Visible = true;
        }
        catch
        {
        }

    }

    protected void btnexaccess_Click(object sender, EventArgs e)
    {

    }

    protected void view_click(object sender, EventArgs e)
    {
        LinkButton lnkSelected = (LinkButton)sender;
        string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxS) - 1;
        string activerow = rowIndx.ToString();

       
            //string access1 = Convert.ToString(GridView1.Rows[Convert.ToInt32(activerow)].Cells[2].Text);
        string access1 = lnkSelected.Text;
            txtaccess.Text = access1;

            divaccess.Visible = false;
            divaccessno.Visible = false;
            //ds.Clear();
          
            txtsearch.Text = string.Empty;

            btnok.Visible = false;
            btnex.Visible = false;
            txtsearch.Visible = false;

      

    }

    protected void btn_okk1_Click(object sender, EventArgs e)
    {

       
        view_click(sender,e);

    }

    protected void btn_ex_Click(object sender, EventArgs e)
    {
        divaccess.Visible = false;
        divaccessno.Visible = false;


    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(grdManualExit, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch
        {

        }
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Book Circulation Report";
            string pagename = "book_circulation_report.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(grdManualExit, pagename, attendance, 0, ss, "1");
            Printcontrol.Visible = true;
        }
        catch { }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        lblAlertMsg.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #endregion

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
        catch { }
    }

    protected void ddlsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsearch.SelectedIndex == 0)
        {
            txtsearch.Visible = false;
        }
        else
        {
            txtsearch.Visible = true;
        }
    }

}