using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Collections;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Configuration;

public partial class LibraryMod_NewBookRequest : System.Web.UI.Page
{
    string usercollegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string groupusercode = string.Empty;
    string college_code = string.Empty;
    string lib_code = string.Empty;
    string qry = string.Empty;
    string serial_no = string.Empty;
    string currtime = string.Empty;
    int tot1;
    bool cellflag = false;
    bool flag_true = false;
    Hashtable ht = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DataTable bokreq = new DataTable();
    DataTable bokre = new DataTable();
    static int searchby = 0;
    static string searchlibcode = string.Empty;
    DataRow drbok;
    DataRow drbo;
    DataTable bokstaff = new DataTable();
    DataRow drstaff;
    int selectedCellIndex = 0;
    int selectedcount = 0;
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
           
                usercollegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]) : "";
                usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]) : "";
                singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]) : "";
                groupusercode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : "";
          
            if (!IsPostBack)
            {
                bindclg();
                getLibPrivil();
                txt_fromdate1.Attributes.Add("readonly", "readonly");
                txt_fromdate1.Text = DateTime.Now.ToString("dd/mm/yyyy");
                txt_todate1.Attributes.Add("readonly", "readonly");
                txt_todate1.Text = DateTime.Now.ToString("dd/mm/yyyy");
                //  binddept();
            }
        }
        catch (Exception ex)
        { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearch(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();
        if (searchby == 1)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 title FROM request_book where title Like '" + prefixText + "%' AND Lib_Code='" + searchlibcode + "'  order by title";
            else
                query = "SELECT DISTINCT  TOP  100 title FROM request_book where title Like '" + prefixText + "%'  order by title";
        }
        else if (searchby == 2)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 author FROM request_book where author Like '" + prefixText + "%'  AND Lib_Code='" + searchlibcode + "'  order by author";
            else
                query = "SELECT DISTINCT  TOP  100 author FROM request_book where author Like '" + prefixText + "%'  order by author";
        }
        else if (searchby == 3)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 publisher FROM request_book where publisher Like '" + prefixText + "%'  AND Lib_Code='" + searchlibcode + "'  order by publisher";
            else
                query = "SELECT DISTINCT  TOP  100 publisher FROM request_book where publisher Like '" + prefixText + "%'   order by publisher";
        }
        else if (searchby == 6)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 edition FROM request_book where edition Like '" + prefixText + "%'  AND Lib_Code='" + searchlibcode + "'  order by edition";
            else
                query = "SELECT DISTINCT  TOP  100 edition FROM request_book where edition Like '" + prefixText + "%'  order by edition";
        }
        if (searchby == 1 || searchby == 2 || searchby == 3 || searchby == 6)
            values = ws.Getname(query);
        return values;
    }

    #region BindHeaders

    public void bindclg()
    {
        try
        {
            ddlclg.Items.Clear();
            string columnfied = string.Empty;
            string group_user = Session["collegecode"] != null ? Convert.ToString(Session["collegecode"]) : "";
            if (group_user.Contains(";"))
            {
                string[] groupsemi = group_user.Split(';');
                group_user = Convert.ToString(groupsemi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfied = " and group_code='" + group_user + "'";
            }
            else if (Session["collegecode"] != null)
            {
                columnfied = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfied));
            ds = da.select_method("bind_college", ht, "sp");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                ddlclg.SelectedIndex = 0;
            }
        }
        catch
        {
        }
    }

    public void bindlibrary(string LibCollection)
    {
        try
        {
            ddllibrary.Items.Clear();
            college_code = Convert.ToString(Session["collegecode"]);

            string lib = "select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND college_code='" + college_code + "' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds.Clear();
            ds = da.select_method_wo_parameter(lib, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddllibrary.DataSource = ds;
                ddllibrary.DataTextField = "lib_name";
                ddllibrary.DataValueField = "lib_code";
                ddllibrary.DataBind();
                ddllibrary.Items.Insert(0, "All");
                searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
            }

        }
        catch
        {
        }
    }

    public void libcode()
    {
        try
        {
            college_code = Convert.ToString(Session["collegecode"]);
            string lib = ddllibrary.SelectedItem.ToString();
            if (ddllibrary.SelectedIndex != 0)
            {
                string libcode = "select lib_name,lib_code from library where college_code='" + college_code + "' and lib_name='" + lib + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(libcode, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lib_code = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
                }
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
            ddlsearch.Items.Clear();
            ds.Clear();
            string dept = string.Empty;
            college_code = Convert.ToString(Session["collegecode"]);
            // libcode();
            if (!string.IsNullOrEmpty(college_code))
            {
                dept = "select distinct(dept_name) as department from journal_dept where college_code ='" + college_code + "'";
                if (ddllibrary.SelectedIndex != 0)
                {
                    dept = dept + " and Lib_Code='" + lib_code + "'";
                }
                ds.Clear();
                ds = da.select_method_wo_parameter(dept, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlsearch.DataSource = ds;
                    ddlsearch.DataTextField = "department";
                    ddlsearch.DataValueField = "department";
                    ddlsearch.DataBind();
                    ddlsearch.Items.Insert(0, "All");
                }
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
            string coll_Code = Convert.ToString(ddlclg.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            Hashtable hsLibcode = new Hashtable();
            if (singleuser.ToLower() == "true")
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

            bindlibrary(LibCollection);
            bindlib(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void ddlclg_selectedIndex_changed(object sender, EventArgs e)
    {
    }

    protected void ddllibrary_selectedindex_changed(object sender, EventArgs e)
    {
        searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
    }

    protected void ddlsearchby_selectedindex_changed(object sender, EventArgs e)
    {

        if (ddlSearchby.SelectedIndex == 0)
        {
            txtsearchby.Visible = false;
            ddlsearch.Visible = false;
            txt_todate1.Visible = false;
            txt_fromdate1.Visible = false;
            lbl_todate.Visible = false;
            lblfromdt.Visible = false;
        }
        else if (ddlSearchby.SelectedIndex == 5)
        {
            txt_fromdate1.Visible = true;
            txt_todate1.Visible = true;
            lblfromdt.Visible = true;
            lbl_todate.Visible = true;
            ddlsearch.Visible = false;
            txtsearchby.Visible = false;
        }
        else if (ddlSearchby.SelectedIndex == 8)
        {
            txtsearchby.Visible = false;
            ddlsearch.Visible = true;
            txt_todate1.Visible = false;
            txt_fromdate1.Visible = false;
            lbl_todate.Visible = false;
            lblfromdt.Visible = false;
            binddept();
        }
        else
        {
            txtsearchby.Visible = true;
            ddlsearch.Visible = false;
            txt_todate1.Visible = false;
            txt_fromdate1.Visible = false;
            lbl_todate.Visible = false;
            lblfromdt.Visible = false;
        }
        searchby = ddlSearchby.SelectedIndex;
    }

    protected void ddlsearch_selectedindex_changed(object sender, EventArgs e)
    {
    }

    protected void grdBookReq_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 2; i < e.Row.Cells.Count; i++)
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

    protected void grdBookReq_onselectedindexchanged(object sender, EventArgs e)
    {
        divaddnew.Visible = true;
        divaddnew1.Visible = true;
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        if (Convert.ToString(rowIndex) != "" && Convert.ToString(rowIndex) != "-1")
        {
            string lib_name = ddllibrary.SelectedItem.ToString();
            string title2 = Convert.ToString(grdBookReq.Rows[0].Cells[3].Text);
            string auth = Convert.ToString(grdBookReq.Rows[rowIndex].Cells[4].Text);
            string publisher1 = Convert.ToString(grdBookReq.Rows[rowIndex].Cells[5].Text);
            if (publisher1 == "&nbsp;")
            {
                publisher1 = "";
            }
            string suplier = Convert.ToString(grdBookReq.Rows[rowIndex].Cells[6].Text);
            if (suplier == "&nbsp;")
            {
                suplier = "";
            }
            string reqbystaff = Convert.ToString(grdBookReq.Rows[rowIndex].Cells[7].Text);
            if (reqbystaff == "&nbsp;")
            {
                reqbystaff = "";
            }
            // string purposes = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text.ToString();
            string reqno = Convert.ToString(grdBookReq.Rows[rowIndex].Cells[2].Text);
            if (reqno == "&nbsp;")
            {
                reqno = "";
            }
            string reqdate = Convert.ToString(grdBookReq.Rows[rowIndex].Cells[9].Text);
            if (reqdate == "&nbsp;")
            {
                reqdate = "";
            }
            string rate1 = Convert.ToString(grdBookReq.Rows[rowIndex].Cells[11].Text);
            if (rate1 == "&nbsp;")
            {
                rate1 = "";
            }
            string no_copies = Convert.ToString(grdBookReq.Rows[rowIndex].Cells[12].Text);
            if (no_copies == "&nbsp;")
            {
                no_copies = "";
            }
            string totprice = Convert.ToString(grdBookReq.Rows[rowIndex].Cells[13].Text);
            if (totprice == "&nbsp;")
            {
                totprice = "";
            }
            txttitle.Text = title2;
            txtauthor.Text = auth;
            txtpublish.Text = publisher1;
            txtsupplier.Text = suplier;
            txtreqBystaff.Text = reqbystaff;
            txtrequisitionno.Text = reqno;
            //txtreqon.Text = reqdate;
            txtPrice.Text = rate1;
            txtnoofcopies.Text = no_copies;
            txttotprice.Text = totprice;
        }
        txtrequisitionno.Visible = true;
        txtremcopy.Enabled = true;
        txtrecievedcopy.Enabled = true;
        btnsave.Visible = false;
        btnupdate.Visible = true;
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            libcode();
            btn_delete.Visible = true;
            if (ddllibrary.SelectedIndex == 0)
            {
                if (ddlSearchby.SelectedIndex == 0)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book";
                }
                else if (ddlSearchby.SelectedIndex == 1)
                {
                    qry = "select 0serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where  title like '%" + txtsearchby.Text + "%'";

                }
                else if (ddlSearchby.SelectedIndex == 2)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where  author like '%" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 3)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where  publisher like '%" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 4)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where  serial_no like '" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 5)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where  access_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "'";
                }
                else if (ddlSearchby.SelectedIndex == 6)
                {
                    qry = "select 0 as s,serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where  publisher like '%" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 7)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where  publisher like '%" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 8)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where  department = '" + Convert.ToString(ddlsearch.SelectedItem.Text) + "'";
                }
            }
            else
            {
                if (ddlSearchby.SelectedIndex == 0)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where lib_code='" + lib_code + "'";

                }
                else if (ddlSearchby.SelectedIndex == 1)
                {
                    qry = "select 0serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where lib_code='" + lib_code + "' and title like '%" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 2)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where lib_code='" + lib_code + "' and author like '%" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 3)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where lib_code='" + lib_code + "' and publisher like '%" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 4)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where lib_code='" + lib_code + "' and serial_no like '" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 5)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where lib_code='" + lib_code + "' and access_date between '" + txt_fromdate1.Text + "' and '" + txt_todate1.Text + "'";
                }
                else if (ddlSearchby.SelectedIndex == 6)
                {
                    qry = "select 0 as s,serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where lib_code='" + lib_code + "' and publisher like '%" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 7)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where lib_code='" + lib_code + "' and publisher like '%" + txtsearchby.Text + "%'";
                }
                else if (ddlSearchby.SelectedIndex == 8)
                {
                    qry = "select serial_no,title,author,publisher,edition,access_date,available_flag,No_of_Reqbooks,price,supplier,receivedcopies,remainingcopies,department from request_book where lib_code='" + lib_code + "' and department = '" + Convert.ToString(ddlsearch.SelectedItem.Text) + "'";
                }
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bokreq.Columns.Add("Requisition No", typeof(string));
                bokreq.Columns.Add("Title", typeof(string));
                bokreq.Columns.Add("Author", typeof(string));
                bokreq.Columns.Add("Publisher", typeof(string));
                bokreq.Columns.Add("Supplier", typeof(string));
                bokreq.Columns.Add("Request By/Justification", typeof(string));
                bokreq.Columns.Add("Department", typeof(string));
                bokreq.Columns.Add("Requested Date", typeof(string));
                bokreq.Columns.Add("Available Status", typeof(string));
                bokreq.Columns.Add("Rate", typeof(string));
                bokreq.Columns.Add("No Of Request Books", typeof(string));
                bokreq.Columns.Add("Total Amount", typeof(string));
                bokreq.Columns.Add("Copies Recieved From Supplier", typeof(string));
                bokreq.Columns.Add("Remaining Copies", typeof(string));

                int sno = 0;
                int tot1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drbok = bokreq.NewRow();
                    string requisitionno = Convert.ToString(ds.Tables[0].Rows[i]["serial_no"]);
                    string title = Convert.ToString(ds.Tables[0].Rows[i]["title"]);
                    string author = Convert.ToString(ds.Tables[0].Rows[i]["author"]);
                    string publish = Convert.ToString(ds.Tables[0].Rows[i]["publisher"]);
                    string supp = Convert.ToString(ds.Tables[0].Rows[i]["supplier"]);
                    string reqby = Convert.ToString(ds.Tables[0].Rows[i]["edition"]);
                    string department = Convert.ToString(ds.Tables[0].Rows[i]["department"]);
                    string reqdate = Convert.ToString(ds.Tables[0].Rows[i]["access_date"]);
                    DateTime dt = new DateTime();
                    dt = Convert.ToDateTime(reqdate);
                    reqdate = dt.ToString("dd/MMM/yyyy");
                    string availablestat = Convert.ToString(ds.Tables[0].Rows[i]["available_flag"]);
                    string rate = Convert.ToString(ds.Tables[0].Rows[i]["price"]);
                    string noofreqbooks = Convert.ToString(ds.Tables[0].Rows[i]["No_of_Reqbooks"]);

                    string totamt = string.Empty;
                    string coprecfromsup = Convert.ToString(ds.Tables[0].Rows[i]["receivedcopies"]);
                    string remcop = Convert.ToString(ds.Tables[0].Rows[i]["remainingcopies"]);

                    if (Convert.ToString(rate) != "" && Convert.ToString(noofreqbooks) != "")
                    {
                        tot1 = Convert.ToInt32(rate) * Convert.ToInt32(noofreqbooks);
                        totamt = Convert.ToString(tot1);
                    }
                    drbok["Requisition No"] = requisitionno;
                    drbok["Title"] = title;
                    drbok["Author"] = author;
                    drbok["Publisher"] = publish;
                    drbok["Supplier"] = supp;
                    drbok["Request By/Justification"] = reqby;
                    drbok["Department"] = department;
                    drbok["Requested Date"] = reqdate;
                    drbok["Available Status"] = availablestat;
                    drbok["Rate"] = rate;
                    drbok["No Of Request Books"] = noofreqbooks;
                    drbok["Total Amount"] = totamt;
                    drbok["Copies Recieved From Supplier"] = coprecfromsup;
                    drbok["Remaining Copies"] = remcop;
                    bokreq.Rows.Add(drbok);
                }
                grdBookReq.DataSource = bokreq;
                grdBookReq.DataBind();
                grdBookReq.Visible = true;
             
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                divtable.Visible = true;
                grdBookReq.Visible = true;
                select_range.Visible = true;
                btnPopAlertClose.Visible = false;
                divPopupAlert.Visible = false;
                divAlertContent.Visible = false;

            }
            else
            {
                divtable.Visible = false;
                grdBookReq.Visible = false;
                //div_report.Visible = false;
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                btnPopAlertClose.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
            }
        }
        catch
        {
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
        catch { }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(grdBookReq, report);
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
            string ss = Session["usercode"].ToString();
            string books = "New Book Request";
            string pagename = "NewBookRequest.aspx";
            Printcontrolhed2.loadspreaddetails(grdBookReq, pagename, books, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }


    protected void ddldep_selectedIndex_changed(object sender, EventArgs e)
    {

    }

    protected void ddllib_selectedIndex_changed(object sender, EventArgs e)
    {
    }

    #region ButtonAddClick

    protected void btnaddnew_click(object sender, EventArgs e)
    {
        divaddnew.Visible = true;
        divaddnew1.Visible = true;
        getLibPrivil();
        binddep();
        txtreqon.Attributes.Add("readonly", "readonly");
        txtreqon.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtreqon.Enabled = false;
    }

    protected void btnexit_click(object sender, EventArgs e)
    {
        divaddnew1.Visible = false;
        divaddnew.Visible = false;
    }

    public void bindlib(string LibCollection)
    {
        try
        {
            ddllib.Items.Clear();
            college_code = Convert.ToString(Session["collegecode"]);
            if (!string.IsNullOrEmpty(college_code))
            {
                string lib = "select lib_name,lib_code from library " + LibCollection + " and  college_code='" + college_code + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(lib, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddllib.DataSource = ds;
                    ddllib.DataTextField = "lib_name";
                    ddllib.DataValueField = "lib_name";
                    ddllib.DataBind();

                }
            }
        }
        catch
        {
        }
    }

    public void binddep()
    {
        try
        {
            ddldep.Items.Clear();
            ds.Clear();
            string dept = string.Empty;
            college_code = Convert.ToString(Session["collegecode"]);

            if (!string.IsNullOrEmpty(college_code))
            {
                dept = "select distinct(dept_name) as department from journal_dept where college_code ='" + college_code + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(dept, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddldep.DataSource = ds;
                    ddldep.DataTextField = "department";
                    ddldep.DataValueField = "department";
                    ddldep.DataBind();

                }
            }
        }

        catch
        {
        }
    }

    protected void btnsave_click(object sender, EventArgs e)
    {
        try
        {
            string insertqry = string.Empty;
            int insertqry1;
            college_code = Convert.ToString(Session["collegecode"]);
            string lib1 = Convert.ToString(ddllib.SelectedItem);
            string libcode = "select lib_name,lib_code from library where college_code='" + college_code + "' and lib_name='" + lib1 + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lib_code = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }
            if (txttitle.Text == "")
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Enter Book Name";
                return;
            }
            else if (txtauthor.Text == "")
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Enter Author Name";
                return;
            }
            else if (txtsupplier.Text == "")
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Enter The Supplier Name";
                return;
            }
            else if (txtreqBystaff.Text == "")
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Enter The Staff Code";
                return;
            }
            else if (txtnoofcopies.Text == "")
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Enter No Of Req Books";
                return;
            }
            string serialno1 = string.Empty;
            string serial = "select max(convert(int,isnull(serial_no,'0'),4)) as serialno from request_book where lib_code='" + lib_code + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(serial, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                serialno1 = Convert.ToString(ds.Tables[0].Rows[0]["serialno"]);

            }
            if (serialno1 == "" || serialno1 == null)
            {
                serialno1 = "0";
            }
            int serno = Convert.ToInt32(serialno1);
            serno = serno + 1;
            serial_no = Convert.ToString(serno);
            currtime = DateTime.Now.ToString("hh:mm:ss tt");
            string date = txtreqon.Text;
            string[] dt_DATE = date.Split('/');
            if (dt_DATE.Length == 3)
                date = dt_DATE[1].ToString() + "/" + dt_DATE[0].ToString() + "/" + dt_DATE[2].ToString();


            insertqry = "if not exists(select * from request_book where No_of_Reqbooks= '" + txtnoofcopies.Text + "'and title='" + txttitle.Text + "' and author='" + txtauthor.Text + "' and publisher='" + txtpublish.Text + "' and edition='" + txtreqBystaff.Text + "' and price='" + txtPrice.Text + "' and supplier='" + txtsupplier.Text + "' and purpose='" + txtpurpose.Text + "' and receivedcopies='" + txtrecievedcopy.Text + "' and remainingcopies='" + txtremcopy.Text + "' and  serial_no='" + serial_no + "' and lib_code='" + lib_code + "')  insert into request_book(serial_no,title,author,publisher,edition,access_date,access_time,Lib_Code,available_flag,receivedcopies,remainingcopies,department,No_Of_Reqbooks,price,supplier,purpose) Values('" + serial_no + "','" + txttitle.Text + "','" + txtauthor.Text + "','" + txtpublish.Text + "','" + txtreqBystaff.Text + "','" + date + "','" + currtime + "','" + lib_code + "','Available','" + txtrecievedcopy.Text + "','" + txtremcopy.Text + "','" + ddldep.Text + "','" + txtnoofcopies.Text + "','" + txtPrice.Text + "','" + txtsupplier.Text + "','" + txtpurpose.Text + "') else update request_book set No_of_Reqbooks= '" + txtnoofcopies.Text + "', title='" + txttitle.Text + "',author='" + txtauthor.Text + "',publisher='" + txtpublish.Text + "',edition='" + txtreqBystaff.Text + "',price='" + txtPrice.Text + "',supplier='" + txtsupplier.Text + "',purpose='" + txtpurpose.Text + "',receivedcopies='" + txtrecievedcopy.Text + "',remainingcopies='" + txtremcopy.Text + "' where serial_no='" + serial_no + "' and lib_code='" + lib_code + "'";

            insertqry1 = da.update_method_wo_parameter(insertqry, "text");

            if (insertqry1 == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Not Saved";
                btnPopAlertClose.Visible = true;
                divAlertContent.Visible = true;
                divaddnew.Visible = false;
                divaddnew1.Visible = false;
            }
            else
            {
                divaddnew.Visible = false;
                divaddnew1.Visible = false;
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                divAlertContent.Visible = true;
                btnPopAlertClose.Visible = true;
                lblAlertMsg.Text = "Records Saved Successfully";
            }
        }
        catch
        {
        }
    }

    protected void txttit_ontextchanged(object sender, EventArgs e)
    {
        try
        {
            college_code = Convert.ToString(Session["collegecode"]);
            string lib1 = Convert.ToString(ddllib.SelectedItem);
            string libcode = "select lib_name,lib_code from library where college_code='" + college_code + "' and lib_name='" + lib1 + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lib_code = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }
            string titqry = string.Empty;


            titqry = "select distinct acc_no,title,author,edition,publisher,supplier,price from bookdetails where title like  '" + txttitle.Text + "%' and lib_code='" + lib_code + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(titqry, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtauthor.Focus();
                lbladdalert.Visible = true;
                lbladdalert.Text = "Title Like '" + txttitle.Text + "' found in the library,do you want to search for it?";
                divaddpopup.Visible = true;
                divaddpopup1.Visible = true;
                btnaddpopexit.Visible = true;
                btnaddpopok.Visible = true;
            }
            else
            { }
        }

        catch
        {
        }
    }

    protected void btnPopAlertaddok_Click(object sender, EventArgs e)
    {
        try
        {
            int sno = 0;
            college_code = Convert.ToString(Session["collegecode"]);
            string lib1 = Convert.ToString(ddllib.SelectedItem);
            string libcode = "select lib_name,lib_code from library where college_code='" + college_code + "' and lib_name='" + lib1 + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lib_code = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }
            string titqry = string.Empty;


            titqry = "select distinct acc_no,title,author,edition,publisher,supplier,price from bookdetails where title like  '" + txttitle.Text + "%' and lib_code='" + lib_code + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(titqry, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                bokre.Columns.Add("Access No", typeof(string));
                bokre.Columns.Add("Title", typeof(string));
                bokre.Columns.Add("Author", typeof(string));
                bokre.Columns.Add("Edition", typeof(string));
                bokre.Columns.Add("Publisher", typeof(string));
                bokre.Columns.Add("Supplier", typeof(string));
                bokre.Columns.Add("Price", typeof(string));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drbo = bokre.NewRow();
                    string acc_no = Convert.ToString(ds.Tables[0].Rows[i]["acc_no"]);
                    string title = Convert.ToString(ds.Tables[0].Rows[i]["title"]);
                    string author = Convert.ToString(ds.Tables[0].Rows[i]["author"]);
                    string publish = Convert.ToString(ds.Tables[0].Rows[i]["publisher"]);
                    string supp = Convert.ToString(ds.Tables[0].Rows[i]["supplier"]);
                    string reqby = Convert.ToString(ds.Tables[0].Rows[i]["edition"]);
                    string price1 = Convert.ToString(ds.Tables[0].Rows[i]["price"]);

                    drbo["Access No"] = acc_no;
                    drbo["Title"] = title;
                    drbo["Author"] = author;
                    drbo["Edition"] = publish;
                    drbo["Publisher"] = supp;
                    drbo["Supplier"] = reqby;
                    drbo["Price"] = price1;

                    bokre.Rows.Add(drbo);


                }
                gridview3.DataSource = bokreq;
                gridview3.DataBind();
                gridview3.Visible = true;
                div4.Visible = true;
                div1.Visible = true;
                div3.Visible = true;
                gridview3.Visible = true;
                divaddpopup.Visible = false;
                divaddpopup1.Visible = false;
                btnaddpopexit.Visible = false;
                btnaddpopok.Visible = false;


            }
        }
        catch
        {
        }
    }

    protected void gridview3_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void gridview3_onselectedindexchanged(object sender, EventArgs e)
    {

        divstafflist.Visible = true;
        divstafflist1.Visible = true;
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

    }

    protected void btn_ok1_Click(object sender, EventArgs e)
    {


        string title = gridview3.Rows[selectedCellIndex].Cells[1].Text;

        string author = gridview3.Rows[selectedCellIndex].Cells[2].Text;
        string publis = gridview3.Rows[selectedCellIndex].Cells[3].Text;
        string suppli = gridview3.Rows[selectedCellIndex].Cells[4].Text;
        string editions = gridview3.Rows[selectedCellIndex].Cells[5].Text;
        string pric = gridview3.Rows[selectedCellIndex].Cells[6].Text;

        div1.Visible = false;
        div3.Visible = false;

        txttitle.Text = title;
        txtauthor.Text = author;
        txtpublish.Text = publis;
        txtPrice.Text = pric;




    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        div1.Visible = false;
        div3.Visible = false;
    }

    protected void btnPopAlertAddexit_Click(object sender, EventArgs e)
    {
        divaddpopup.Visible = false;
        divaddpopup1.Visible = false;
    }

    protected void txtnoofcopies_changed(object sender, EventArgs e)
    {
        txtnoofcopies.Focus();
        double priceamt = Convert.ToDouble(txtPrice.Text);
        double total = 0;
        double noofcop = Convert.ToDouble(txtnoofcopies.Text);
        if (priceamt != 0 && noofcop != 0)
        {
            total = priceamt * noofcop;
        }
        string totamt = Convert.ToString(total);
        txttotprice.Text = totamt;


    }

    protected void txtreqbystaff_changed(object sender, EventArgs e)
    {
        try
        {
            txtreqBystaff.Focus();

            college_code = Convert.ToString(Session["collegecode"]);
            string qry1 = "SELECT distinct staffmaster.staff_code,staffmaster.staff_name,hrdept_master.dept_name FROM staffmaster,stafftrans,hrdept_master where  staffmaster.staff_code  ='" + txtreqBystaff.Text + "' and hrdept_master.dept_code=stafftrans.dept_code and  staffmaster.staff_code=stafftrans.staff_code and staffmaster.settled =0 and stafftrans.latestrec<>0 and staffmaster.college_code=hrdept_master.college_code and staffmaster.college_code='" + college_code + "' order by hrdept_master.dept_name,staffmaster.staff_code ";
            ds.Clear();
            ds = da.select_method_wo_parameter(qry1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                divPopupAlert.Visible = false;
                divAlertContent.Visible = false;
                btnPopAlertClose.Visible = false;
                lblAlertMsg.Visible = false;



            }
            else
            {
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                btnPopAlertClose.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Staff Code does not Exists";


            }
        }
        catch
        {
        }
    }

    #endregion

    #region ButtonStaffCode

    private void staffsearchgo()
    {
        try
        {
            int sno = 0;
            college_code = Convert.ToString(Session["collegecode"]);
            string stafqry = string.Empty;
            if (ddlsearstaff.SelectedIndex == 0)
            {
                stafqry = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name ,hrdept_master.dept_name FROM staffmaster , stafftrans,hrdept_master where staffmaster.staff_code = stafftrans.staff_code    and stafftrans.dept_code = hrdept_master.dept_code and stafftrans.latestrec <> 0 AND staffmaster.resign = 0 and  staffmaster.settled = 0   and staffmaster.college_code =hrdept_master.college_code and staffmaster.college_code='" + college_code + "'";
            }
            else if (ddlsearstaff.SelectedIndex == 1)
            {
                stafqry = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name ,hrdept_master.dept_name FROM staffmaster , stafftrans,hrdept_master where staffmaster.staff_code = stafftrans.staff_code    and stafftrans.dept_code = hrdept_master.dept_code and stafftrans.latestrec <> 0 AND staffmaster.resign = 0 and  staffmaster.settled = 0   and staffmaster.college_code =hrdept_master.college_code and staffmaster.college_code='" + college_code + "' and staffmaster.staff_name='" + txtsearstaff.Text + "' ";
            }
            else
            {
                stafqry = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name ,hrdept_master.dept_name FROM staffmaster , stafftrans,hrdept_master where staffmaster.staff_code = stafftrans.staff_code    and stafftrans.dept_code = hrdept_master.dept_code and stafftrans.latestrec <> 0 AND staffmaster.resign = 0 and  staffmaster.settled = 0   and staffmaster.college_code =hrdept_master.college_code and staffmaster.college_code='" + college_code + "' and staffmaster.staff_code='" + txtsearstaff.Text + "' ";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(stafqry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bokstaff.Columns.Add("Staff No", typeof(string));
                bokstaff.Columns.Add("Staff Name", typeof(string));
                bokstaff.Columns.Add("Department", typeof(string));


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    sno++;
                    drstaff = bokstaff.NewRow();
                    string staffno = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                    string staffname = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                    string deptname = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);

                    drstaff["Staff No"] = staffno;
                    drstaff["Staff Name"] = staffname;
                    drstaff["Department"] = deptname;
                    bokstaff.Rows.Add(drstaff);

                }
                grdStaff.DataSource = bokstaff;
                grdStaff.DataBind();
                grdStaff.Visible = true;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                div2.Visible = true;
                grdStaff.Visible = true;
                divPopupAlert.Visible = false;
                btnPopAlertClose.Visible = false;

            }
            else
            {
                div2.Visible = false;
                grdStaff.Visible = false;
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                divAlertContent.Visible = true;
                btnPopAlertClose.Visible = true;


            }
        }
        catch
        {
        }
    }

    protected void btnreqstaff_click(object sender, EventArgs e)
    {
        try
        {
            divstafflist.Visible = true;
            divstafflist1.Visible = true;
            staffsearchgo();

        }
        catch
        {
        }
    }

    protected void ddlsearstaff_selectedindex_changed(object sender, EventArgs e)
    {
        if (ddlsearstaff.SelectedIndex == 0)
        {
            txtsearstaff.Visible = false;
        }
        else
        {
            txtsearstaff.Visible = true;
        }

    }

    protected void btnseargo_click(object sender, EventArgs e)
    {
        try
        {
            string staffqry = string.Empty;
            staffsearchgo();
        }
        catch
        {
        }
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
                   , SelectedGridCellgrdStaff.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdStaff_onselectedindexchanged(object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        selectedCellIndex = int.Parse(this.SelectedGridCellgrdStaff.Value);
        string staffcod = grdStaff.Rows[rowIndex].Cells[1].Text;
        txtreqBystaff.Text = staffcod;
        divstafflist.Visible = false;
        divstafflist1.Visible = false;
    }

    protected void btn_ex_Click(object sender, EventArgs e)
    {
        divstafflist.Visible = false;
        divstafflist1.Visible = false;
    }

    #endregion

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        lblAlertMsg.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    protected void btnupdate_click(object sender, EventArgs e)
    {
    }

    protected void Btn_range_Click(object sender, EventArgs e)
    {
        if (txt_frange.Text == "" || txt_trange.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Both From And To Range.')", true);
            return;
        }

        if (Convert.ToInt32(txt_frange.Text) > Convert.ToInt32(txt_trange.Text))
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('To Range Should Be Greater Than Or Equal To From Range.')", true);
            return;
        }

        foreach (GridViewRow row in grdBookReq.Rows)
        {
            Label sno = (Label)row.FindControl("lbl_sno");
            string sl_no = sno.Text;
            if (sl_no != "")
            {
                CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                if (Convert.ToInt32(sl_no) >= Convert.ToInt32(txt_frange.Text) && Convert.ToInt32(sl_no) <= Convert.ToInt32(txt_trange.Text))
                {
                    cbsel.Checked = true;
                }
                else
                {
                    cbsel.Checked = false;
                }
            }
        }

        txt_frange.Text = "";
        txt_trange.Text = "";
    }

    #region Delete

    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {

            if (grdBookReq.Rows.Count > 0)
            {


                foreach (GridViewRow row in grdBookReq.Rows)
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                    if (!cbsel.Checked)
                        continue;
                    else
                        selectedcount++;
                }
                if (selectedcount == 0)
                {
                    divPopupAlert.Visible = true;
                    lblAlertMsg.Text = "Select atleast one entry to delete";
                    return;
                }
                else
                {
                    Diveleterecord.Visible = true;
                    lbl_Diveleterecord.Text = "Are you sure to delete the selected record?";
                }

            }
        }
        catch (Exception ex) { }


    }

    protected void btn_detele_yes__record_Click(object sender, EventArgs e)
    {
        try
        {
            string deletebook = "";
            int deletere = 0;
            string serialno = "";
            string title1 = "";
            string libcode = "";
            Diveleterecord.Visible = false;

            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (grdBookReq.Rows.Count > 0)
            {

                if (lbl_Diveleterecord.Text == "Are you sure to delete the selected record?")
                {
                    foreach (GridViewRow row in grdBookReq.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        if (!cbsel.Checked)
                            continue;
                        serialno = Convert.ToString(row.Cells[2].Text);
                        if (serialno == "&nbsp;")
                        {
                            serialno = "";
                        }
                        title1 = Convert.ToString(row.Cells[3].Text);
                        if (title1.Contains("amp;"))
                            title1 = title1.Replace("&amp;", "&");

                        deletebook = "delete from request_book where serial_no='" + serialno + "' and title='" + title1 + "'";
                        deletere = da.update_method_wo_parameter(deletebook, "Text");
                    }
                }
                if (deletere > 0)
                {
                    Diveleterecord.Visible = false;
                    divPopupAlert.Visible = true;
                    lblAlertMsg.Text = "Record deleted successfully";
                    btngo_click(sender, e);
                }
            }
        }
        catch (Exception ex) { }
    }

    protected void btn_detele_no__recordClick(object sender, EventArgs e)
    {

        try
        {
            Diveleterecord.Visible = false;

        }
        catch (Exception ex) { }

    }

    #endregion

}