using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;

public partial class LibraryMod_Book_Allocation_Report : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    DataTable dtCommon = new DataTable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable columnhash = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    bool flag_true = false;
 
    DataTable bookall = new DataTable();
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
          
            txt_from.Attributes.Add("readonly", "readonly");
            txt_from.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Txtto.Attributes.Add("readonly", "readonly");
            Txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
            if (!IsPostBack)
            {
                Bindcollege();
                getLibPrivil();
                Booktype();
                //Fpspread.Visible = false;
                //rptprint.Visible = false;
                rdbLibrary.Visible = false;
                rdbtrans.Visible = false;
                rdbissue.Visible = false;
               
            }
        }
        catch
        {
        }
    }

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
        {
        }
    }

    public void BindLibrary(string LibCollection)
    {
        try
        {
            ddltransfrom.Items.Clear();
            ds.Clear();
            string College = Convert.ToString(ddlCollege.SelectedValue);
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " and college_code='" + College + "' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(lib, "text");
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    ddltransfrom.DataSource = ds;
                    ddltransfrom.DataTextField = "lib_name";
                    ddltransfrom.DataValueField = "lib_code";
                    ddltransfrom.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); 
        }
    }

    public void Booktype()
    {
        try
        {
            ddltype.Items.Add("Books");
            ddltype.Items.Add("Project Books");
            ddltype.Items.Add("Non Book Materials");
            ddltype.Items.Add("Back Volume");
            ddltype.Items.Add("Periodical");
        }
        catch
        {
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); 
        }
    }

    protected void rdbrack_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            if (rdbrack.Checked == true)
            {
                Transfrom.Text = "Library Name";
                //Lbltransto.Visible = false;
                //ddltransto.Visible = false;
                getLibPrivil();
            }
            if (rdbrackto.Checked == true)
            {
                Transfrom.Text = "Library Name";
                //Lbltransto.Visible = false;
                //ddltransto.Visible = false;
                getLibPrivil();
            }
            if (rdbLibrary.Checked == true)
            {
                Transfrom.Text = "Transfer From";
                //Lbltransto.Visible = true;
                //ddltransto.Visible = true;
                getLibPrivil();

            }
            if (rdbtrans.Checked == true)
            {
                Transfrom.Text = "Transfer From";
                //Lbltransto.Visible = true;
                //ddltransto.Visible = true;
                Sql = "Select distinct from_lib_code from book_transfer where transfer_type =1";
                ds = d2.select_method_wo_parameter(Sql, "text");

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    ddltransfrom.DataSource = ds;
                    ddltransfrom.DataTextField = "from_lib_code";
                    ddltransfrom.DataValueField = "lib_from_lib_codecode";
                    ddltransfrom.DataBind();
                    //ddltransto.DataSource = ds;
                    //ddltransto.DataTextField = "from_lib_code";
                   // ddltransto.DataValueField = "from_lib_code";
                   // ddltransto.DataBind();
                }

            }
            if (rdbissue.Checked == true)
            {
                Transfrom.Text = "Transfer From";
                //Lbltransto.Visible = true;
                //ddltransto.Visible = true;
                Sql = "Select distinct from_lib_code from book_transfer where transfer_type =2";

                ds = d2.select_method_wo_parameter(Sql, "text");

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    ddltransfrom.DataSource = ds;
                    ddltransfrom.DataTextField = "from_lib_code";
                    ddltransfrom.DataValueField = "from_lib_code";
                    ddltransfrom.DataBind();
                    //ddltransto.DataSource = ds;
                    //ddltransto.DataTextField = "from_lib_code";
                    //ddltransto.DataValueField = "from_lib_code";
                   // ddltransto.DataBind();
                }
            }
        }
        catch
        {
        }
    }

    protected void Cboldsearch_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (Cboldsearch.Checked == true)
            {
                Txtto.Enabled = true;
                txt_from.Enabled = true;
            }
            else
            {
                Txtto.Enabled = false;
                txt_from.Enabled = false;
            }
        }
        catch
        {

        }
    }

    protected void gridview1_onselectedindexchanged(object sender, EventArgs e)
    {
    }

    protected void gridview1_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        gridview1.PageIndex = e.NewPageIndex;
        Go_Click(sender, e);
    }

    protected void gridview1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //((CheckBox)e.Row.FindControl("allchk")).Attributes.Add("onclick",
            //    "javascript:SelectAll('" +
            //    ((CheckBox)e.Row.FindControl("allchk")).ClientID + "')");
            for (int grCol = 0; grCol < gridview1.Columns.Count; grCol++)
                e.Row.Cells[grCol].Visible = false;
            //e.Row.Cells[5].Visible = false;
           
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                //CheckBox cbsel = (CheckBox)e.Row.Cells[5].FindControl("selectchk");
                //cbsel.Visible = false;
                //cbsel.Text = "Select";

               
            }
          
        }

    }

    protected void Go_Click(object sender, EventArgs e)
    {
        try
        {
           
            string boktype = string.Empty;
            string category_var = string.Empty;
            string opt = string.Empty;
            string sqlIcount = string.Empty;
            string sqlBcount = string.Empty;
            DataSet bookallo = new DataSet();
            int sno = 0;
            DataRow dr;
            # region spread header
           
          
            if (rdbrack.Checked == true || rdbrackto.Checked == true)
            {
               // bookall.Columns.Add("Select", typeof(string));
                bookall.Columns.Add("SNo", typeof(string));
                bookall.Columns.Add("Acc No", typeof(string));
                bookall.Columns.Add("Title", typeof(string));
                bookall.Columns.Add("Author", typeof(string));
                bookall.Columns.Add("Rack Number", typeof(string));
                bookall.Columns.Add("Shelf Number", typeof(string));
                bookall.Columns.Add("Position", typeof(string));

                
            }
            else
            {
              //  bookall.Columns.Add("Select", typeof(string));
                bookall.Columns.Add("SNo", typeof(string));
                bookall.Columns.Add("Acc No", typeof(string));
                bookall.Columns.Add("Title", typeof(string));
                bookall.Columns.Add("Author", typeof(string));
                bookall.Columns.Add("Rack Number", typeof(string));
                bookall.Columns.Add("Shelf Number", typeof(string));
                bookall.Columns.Add("Transfer Date", typeof(string));
                bookall.Columns.Add("Returned", typeof(string));

               
            }

            #endregion
            string from_lib = string.Empty;
            string to_lib = string.Empty;
            string sql = string.Empty;
            string tit = string.Empty;
            string auth = string.Empty;
            string rno = string.Empty;
            string snos = string.Empty;

           int row = 0;
            int i = 0;

            #region rdbLibrary
            if (rdbLibrary.Checked == true)
            {
                from_lib = Convert.ToString(ddltransfrom.SelectedValue);
                //to_lib = Convert.ToString(ddltransto.SelectedValue);
                if (from_lib == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Transfer From Library";
                }
                if (to_lib == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Transfer From Library";
                }
                if (Cboldsearch.Checked == true)
                {
                    sql = "select distinct book_transfer.acc_no,book_transfer.transfer_date from book_transfer,bookdetails where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and book_transfer.acc_no=bookdetails.acc_no and transfer_date between '" + txt_from.Text + "' and '" + Txtto.Text + "'";
                }
                else
                    sql = "select distinct book_transfer.acc_no,book_transfer.transfer_date from book_transfer,bookdetails where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and book_transfer.acc_no=bookdetails.acc_no";

                bookallo = d2.select_method_wo_parameter(sql, "Text");
               
                    for ( row = i; row < bookallo.Tables[0].Rows.Count; row++)
                    {

                        dr = bookall.NewRow();
                        sno++;
                        int m = i;
                        m++;

                        dr["SNo"] = Convert.ToString(sno);
                        tit = d2.GetFunction("select title from bookdetails where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and lib_code='" + to_lib + "'");
                        auth = d2.GetFunction("select author from bookdetails where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and lib_code='" + to_lib + "'");
                        rno = d2.GetFunction("select rack_no from rack_allocation where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and lib_code='" + to_lib + "'");
                        snos = d2.GetFunction("select row_no from rack_allocation where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and lib_code='" + to_lib + "'");
                        
                         dr["Title"] = tit;
                         dr["Author"] = auth;
                         dr["Rack Number"] = rno;
                         dr["Shelf Number"] = snos;
                        bookall.Rows.Add(dr);
                    }
                   
                      gridview1.DataSource = bookall;
                    gridview1.DataBind();
                    gridview1.Visible = true;

                  
               
            }
            #endregion

            #region rdbtrans
            if (rdbtrans.Checked == true)
            {
                from_lib = Convert.ToString(ddltransfrom.SelectedValue);
                //to_lib = Convert.ToString(ddltransto.SelectedValue);
                if (from_lib == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Transfer From Library";
                }
                if (to_lib == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Transfer From Library";
                }
                if (Cboldsearch.Checked == true)
                {
                    sql = "select distinct book_transfer.acc_no,book_transfer.transfer_date,case when isnull(returned,0) =0 then 'Not Return' else 'Return' end returned from book_transfer,bookdetails where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and book_transfer.acc_no=bookdetails.acc_no and transfer_date between '" + txt_from.Text + "' and '" + Txtto.Text + "' and transfer_type = 1";
                }
                else
                    sql = "select distinct book_transfer.acc_no,book_transfer.transfer_date,case when isnull(returned,0) =0 then 'Not Return' else 'Return' end returned  from book_transfer,bookdetails where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and book_transfer.acc_no=bookdetails.acc_no and transfer_type = 1";

                bookallo = d2.select_method_wo_parameter(sql, "Text");
               
                for (row = i; row < bookallo.Tables[0].Rows.Count; row++)
                    {
                        dr = bookall.NewRow();
                        sno++;
                        int m = i;
                        m++;
                        dr["SNo"] = Convert.ToString(sno);

                        tit = d2.GetFunction("select title from bookdetails where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and dept_code='" + to_lib + "'");
                        auth = d2.GetFunction("select author from bookdetails where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and dept_code='" + to_lib + "'");
                        rno = d2.GetFunction("select rack_no from rack_allocation where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "'");
                        snos = d2.GetFunction("select row_no from rack_allocation where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "'");
                       
                         dr["Title"] = tit;
                         dr["Author"] = auth;
                         dr["Rack Number"] = rno;
                         dr["Shelf Number"] = snos;
                      
                        dr["Transfer Date"] = Convert.ToString(bookallo.Tables[0].Rows[row]["transfer_date"]);
                        dr["Returned"] = Convert.ToString(bookallo.Tables[0].Rows[row]["returned"]);
                        bookall.Rows.Add(dr);
                    }
                      gridview1.DataSource = bookall;
                    gridview1.DataBind();
                    gridview1.Visible = true;
                  

               
            }
            #endregion

            #region rdbissue
            if (rdbissue.Checked == true)
            {
                from_lib = Convert.ToString(ddltransfrom.SelectedValue);
                //to_lib = Convert.ToString(ddltransto.SelectedValue);
                if (from_lib == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Transfer From Library";
                }
                if (to_lib == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Transfer From Library";
                }
                if (Cboldsearch.Checked == true)
                {
                    sql = "select distinct book_transfer.acc_no,book_transfer.transfer_date,case when isnull(returned,0) =0 then 'Not Return' else 'Return' end returned  from book_transfer,bookdetails where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and book_transfer.acc_no=bookdetails.acc_no and transfer_date between '" + txt_from.Text + "' and '" + Txtto.Text + "' and transfer_type = 2";
                }
                else
                    sql = "select distinct book_transfer.acc_no,book_transfer.transfer_date,case when isnull(returned,0) =0 then 'Not Return' else 'Return' end returned  from book_transfer,bookdetails where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and book_transfer.acc_no=bookdetails.acc_no and transfer_type = 2";

                bookallo = d2.select_method_wo_parameter(sql, "Text");
                dr = bookall.NewRow();
                for (row = i; row < bookallo.Tables[0].Rows.Count; row++)
                    {
                        dr = bookall.NewRow();
                        sno++;
                        int m = i;
                        m++;
                        dr["SNo"] = Convert.ToString(sno);
                      
                        tit = d2.GetFunction("select title from bookdetails where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and lib_code='" + from_lib + "'");
                        auth = d2.GetFunction("select author from bookdetails where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and lib_code='" + from_lib + "'");
                        rno = d2.GetFunction("select rack_no from rack_allocation where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and lib_code='" + from_lib + "'");
                        snos = d2.GetFunction("select row_no from rack_allocation where acc_no='" + Convert.ToString(bookallo.Tables[0].Rows[row]["Acc_no"]) + "' and lib_code='" + from_lib + "'");
                        //  Fpspread.Sheets[0].Cells[m, 1].Text = Convert.ToString();
                        dr["Title"] = tit;
                         dr["Author"] = auth;
                         dr["Rack Number"] = rno;
                         dr["Shelf Number"] = snos;
                         dr["Transfer Date"] = Convert.ToString(bookallo.Tables[0].Rows[row]["transfer_date"]);
                         dr["Returned"] = Convert.ToString(bookallo.Tables[0].Rows[row]["returned"]);
                         bookall.Rows.Add(dr);
                       
                       
                       
                    }
              
                    gridview1.DataSource = bookall;
                    gridview1.DataBind();
                    gridview1.Visible = true;
                   
               
            }
            #endregion

            #region rdbrackto
            if (rdbrack.Checked == true || rdbrackto.Checked == true)
            {
                from_lib = Convert.ToString(ddltransfrom.SelectedValue);
                //to_lib = Convert.ToString(ddltransto.SelectedValue);
                if (from_lib == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Transfer From Library";
                }
                //if (to_lib == "")
                //{
                //    alertpopwindow.Visible = true;
                //    lblalerterr.Text = "Select The Transfer From Library";
                //}

                if (rdbrack.Checked == true)
                {
                    sql = "select bookdetails.acc_no,title,author,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') position from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK')  where    bookdetails.lib_code='" + from_lib + "' and rack_no <> ''  order by len(bookdetails.acc_no),bookdetails.acc_no";
                }
                if (rdbrackto.Checked == true)
                {
                    sql = "select bookdetails.acc_no,title,author,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') position from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK')  where    bookdetails.lib_code='" + from_lib + "' and (rack_no = '' or rack_no is null) order by len(bookdetails.acc_no),bookdetails.acc_no";
                }

                bookallo = d2.select_method_wo_parameter(sql, "Text");
              
                for (row = i; row < bookallo.Tables[0].Rows.Count; row++)
                    {
                        dr = bookall.NewRow();
                        sno++;
                        int m = i;
                        dr["SNo"] = Convert.ToString(sno);
                        dr["Acc No"] = Convert.ToString(bookallo.Tables[0].Rows[row]["acc_no"]);
                        dr["Title"] = Convert.ToString(bookallo.Tables[0].Rows[row]["title"]);
                        dr["Author"] = Convert.ToString(bookallo.Tables[0].Rows[row]["author"]);
                        dr["Rack Number"] = Convert.ToString(bookallo.Tables[0].Rows[row]["rack_no"]);
                        dr["Shelf Number"] = Convert.ToString(bookallo.Tables[0].Rows[row]["row_no"]);
                        dr["Position"] = Convert.ToString(bookallo.Tables[0].Rows[row]["position"]);


                        bookall.Rows.Add(dr);
                    }

              
                    gridview1.DataSource = bookall;
                    gridview1.DataBind();
                    gridview1.Visible = true;
                
            }
            #endregion
        }
        catch
        {
        }
    }

   

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Book Allocation Report";
            string pagename = "Book Allocation Report.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrol1.loadspreaddetails(gridview1, pagename, degreedetails,0,ss,"1");
            Printcontrol1.Visible = true;
        }
        catch
        {
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(gridview1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }
    #endregion

    public override void VerifyRenderingInServerForm(Control control)
    { }

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
            BindLibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

 
}