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

public partial class LibraryMod_BooKAllocation : System.Web.UI.Page
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
    Boolean pageno = false;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;
    int selectedpage = 0;
    static int first = 0;
    Boolean pageno1 = false;
    int ivalue1 = 0;
    int curpage1 = 0;
    double pageSize2 = 0.0;
    int pagecnt1 = 0;
    int pgsize1 = 0;
    DataTable bokaloca = new DataTable();
    DataTable bokaloca1 = new DataTable();
    DataRow drallo;
    DataRow drallo1;

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

            if (grdBooks.Rows.Count == 0)
            {
                BookSDiv.Visible = true;
                bokaloca.Columns.Add("Acc No", typeof(string));
                bokaloca.Columns.Add("Title", typeof(string));
                bokaloca.Columns.Add("Edition", typeof(string));
                bokaloca.Columns.Add("Status", typeof(string));
                bokaloca.Columns.Add("Rack", typeof(string));
                bokaloca.Columns.Add("Shelves", typeof(string));
                bokaloca.Columns.Add("Department", typeof(string));
                grdBooks.DataSource = bokaloca;
                grdBooks.DataBind();
                grdBooks.Visible = true;
            }
            if (grdTranBooks.Rows.Count == 0)
            {
                TransBookDiv.Visible = true;
                bokaloca1.Columns.Add("Title", typeof(string));
                bokaloca1.Columns.Add("Call No", typeof(string));
                bokaloca1.Columns.Add("Acc No", typeof(string));
                grdTranBooks.DataSource = bokaloca1;
                grdTranBooks.DataBind();
                grdTranBooks.Visible = true;
            }
            if (!IsPostBack)
            {
                Bindcollege();
                getLibPrivil();
                Bindbook();
                Booktype();
                Location();
                Loadcategory();
                txt_transdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                rdbLibrary_CheckedChange(sender, e);
            }
        }
        catch (Exception ex)
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

                ddlstat_college.DataSource = dtCommon;
                ddlstat_college.DataTextField = "collname";
                ddlstat_college.DataValueField = "college_code";
                ddlstat_college.DataBind();
                ddlstat_college.SelectedIndex = 0;
                ddlstat_college.Enabled = true;
            }
        }
        catch
        (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void BindLibrary(string LibCollection)
    {
        try
        {
            ddl_library.Items.Clear();
            ds.Clear();
            string College = Convert.ToString(ddlCollege.SelectedValue);
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " and college_code='" + College + "' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(lib, "text");
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    ddl_library.DataSource = ds;
                    ddl_library.DataTextField = "lib_name";
                    ddl_library.DataValueField = "lib_code";
                    ddl_library.DataBind();

                    ddlmoveto.DataSource = ds;
                    ddlmoveto.DataTextField = "lib_name";
                    ddlmoveto.DataValueField = "lib_code";
                    ddlmoveto.DataBind();
                    ddlmoveto.SelectedIndex = 0;
                    ddlmoveto.Enabled = true;

                    ddllibrary_sts.DataSource = ds;
                    ddllibrary_sts.DataTextField = "lib_name";
                    ddllibrary_sts.DataValueField = "lib_code";
                    ddllibrary_sts.DataBind();
                    ddllibrary_sts.SelectedIndex = 0;
                    ddllibrary_sts.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void bindshelf()
    {
        try
        {
            ddlself1.Items.Clear();
            ds.Clear();
            string rack = "SELECT distinct row_no FROM rackrow_master where  lib_code='" + ddl_library.SelectedValue + "' and  Rack_No='" + ddlrack2.SelectedValue + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(rack, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlself1.DataSource = ds;
                ddlself1.DataTextField = "row_no";
                ddlself1.DataValueField = "row_no";
                ddlself1.DataBind();
                pos();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void bindshelf1()
    {
        try
        {
            ddlshelf.Items.Clear();
            ds.Clear();
            string rack = "SELECT distinct row_no FROM rackrow_master where  lib_code='" + ddl_library.SelectedValue + "' and  Rack_No ='" + ddlmoveto.SelectedValue + "'";
            ds = da.select_method_wo_parameter(rack, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlshelf.DataSource = ds;
                ddlshelf.DataTextField = "row_no";
                ddlshelf.DataValueField = "row_no";
                ddlshelf.DataBind();
                pos1();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void bindrack()
    {
        try
        {
            ddlrack2.Items.Clear();
            ds.Clear();
            string rack = "select distinct rack_no  from rackrow_master where  lib_code='" + ddl_library.SelectedValue + "'";
            ds = da.select_method_wo_parameter(rack, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlrack2.DataSource = ds;
                ddlrack2.DataTextField = "rack_no";
                ddlrack2.DataValueField = "rack_no";
                ddlrack2.DataBind();
                ddlmoveto.DataSource = ds;
                ddlmoveto.DataTextField = "rack_no";
                ddlmoveto.DataValueField = "rack_no";
                ddlmoveto.DataBind();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void bindrack1()
    {
        try
        {
            ddlrack2.Items.Clear();
            ds.Clear();
            string rack = "select distinct rack_no  from rackrow_master where  lib_code='" + ddl_library.SelectedValue + "'";
            ds = da.select_method_wo_parameter(rack, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlmoveto.DataSource = ds;
                ddlmoveto.DataTextField = "rack_no";
                ddlmoveto.DataValueField = "rack_no";
                ddlmoveto.DataBind();
                bindshelf();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void Bindbook()
    {
        try
        {
            ddlbook.Items.Add("All");
            ddlbook.Items.Add("Access Number");
            ddlbook.Items.Add("Title");
            ddlbook.Items.Add("Call no");
            ddlbook.Items.Add("Department");
            ddlbook.Items.Add("Author");
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void Booktype()
    {
        try
        {
            ddlsearchbook.Items.Add("Books");
            ddlsearchbook.Items.Add("Project Books");
            ddlsearchbook.Items.Add("Non Book Materials");
            ddlsearchbook.Items.Add("Back Volume");
            ddlsearchbook.Items.Add("Periodical");
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void place()
    {
        try
        {
            string sql1 = string.Empty;
            sql1 = "SELECT ISNULL(Max_Capacity,0) Max_Capacity FROM RowPos_Master ";
            sql1 = sql1 + "WHERE Rack_No ='" + ddlrack2.SelectedValue + "' AND Row_No ='" + ddlself1.SelectedValue + "' AND Pos_No='" + ddlposi.SelectedValue + "' AND Lib_Code ='" + ddl_library.SelectedValue + "' ";
            ds = da.select_method_wo_parameter(sql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpla.DataSource = ds;
                ddlpla.DataTextField = "Max_Capacity";
                ddlpla.DataValueField = "Max_Capacity";
                ddlpla.DataBind();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void place1()
    {
        try
        {
            string sql1 = string.Empty;
            sql1 = "SELECT ISNULL(Max_Capacity,0) Max_Capacity FROM RowPos_Master ";
            sql1 = sql1 + "WHERE Rack_No ='" + ddlrack2.SelectedValue + "' AND Row_No ='" + ddlself1.SelectedValue + "' AND Pos_No='" + ddlposi.SelectedItem + "' AND Lib_Code ='" + ddl_library.SelectedValue + "' ";
            ds = da.select_method_wo_parameter(sql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlplace.DataSource = ds;
                ddlplace.DataTextField = "Max_Capacity";
                ddlplace.DataValueField = "Max_Capacity";
                ddlplace.DataBind();
                ddlplace.Items.Insert(0, "Select");


            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }

    }

    public void Bookdept()
    {
        try
        {
            ddlmoveto.Items.Clear();
            ddldeptm.Items.Clear();
            ddlmoveto.Text = "---Select---";
            string dept = "select distinct isnull(dept_code,'') dept_code from bookdetails order by dept_code";
            ds.Clear();
            ds = da.select_method_wo_parameter(dept, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlmoveto.DataSource = ds;
                ddlmoveto.DataTextField = "dept_code";
                ddlmoveto.DataValueField = "dept_code";
                ddldeptm.DataSource = ds;
                ddldeptm.DataTextField = "dept_code";
                ddldeptm.DataValueField = "dept_code";
                ddldeptm.DataBind();
                ddlmoveto.DataBind();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    public void pos()
    {
        try
        {
            ddlposi.Items.Clear();
            ddlposi.Text = "---Select---";
            string spos = " SELECT Pos_No FROM RowPos_Master WHERE Rack_No ='" + ddlrack2.SelectedValue + "' AND row_no='" + ddlself1.SelectedValue + "' AND Lib_Code ='" + ddl_library.SelectedValue + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(spos, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlposi.DataSource = ds;
                ddlposi.DataTextField = "Pos_No";
                ddlposi.DataValueField = "Pos_No";
                ddlposi.DataBind();
                place();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation");
        }
    }

    public void pos1()
    {
        try
        {
            ddlposition.Items.Clear();
            ddlposition.Text = "---Select---";
            string spos = " SELECT Pos_No FROM RowPos_Master WHERE Rack_No ='" + ddlmoveto.SelectedValue + "' AND row_no='" + ddlshelf.SelectedValue + "' AND Lib_Code ='" + ddl_library.SelectedValue + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(spos, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlposition.DataSource = ds;
                ddlposition.DataTextField = "Pos_No";
                ddlposition.DataValueField = "Pos_No";
                ddlposition.DataBind();
                place1();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation");
        }
    }

    public void Loadcategory()
    {
        string Sql = string.Empty;
        string College = Convert.ToString(ddlCollege.SelectedValue);
        ddlCategory.Items.Clear();
        Sql = "select distinct cat from libcat where lib_code='" + ddl_library.SelectedValue + "' and college_code='" + College + "'";
        ds.Clear();
        ds = da.select_method_wo_parameter(Sql, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                ddlCategory.DataSource = ds;
                ddlCategory.DataTextField = "cat";
                ddlCategory.DataBind();
            }
        }
    }

    public void Location()
    {
        try
        {
            ddlreason.Items.Clear();
            ds.Clear();
            string loc = "SELECT  TextVal,textcode FROM TextValTable WHERE TextCriteria = 'LbLoc' AND College_Code ='" + Convert.ToString(ddlCollege.SelectedValue) + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(loc, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlreason.DataSource = ds;
                ddlreason.DataTextField = "TextVal";
                ddlreason.DataValueField = "textcode";
                ddlreason.DataBind();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void rdbrack_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            col.Visible = true;
            col1.Visible = true;
            Lblmoveself.Visible = true;
            ddlshelf.Visible = true;
            col99.Visible = true;
            col6.Visible = true;
            Tr1.Visible = true;
            Lblmoveto1.Text = "MoveToRack";
            Button5.Visible = true;
            ddlrack2.Enabled = false;
            ddlself1.Enabled = false;
            ddlposi.Enabled = false;
            ddlpla.Enabled = false;
            bindrack1();
            ddlshelf_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void ddlmoveto_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //bindshelf();
        }
        catch
        {
        }
    }

    protected void ddlrack2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindshelf();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void ddlmovetorack_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindshelf1();
            ddlshelf_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void ddlshelf_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            pos1();
            // loadpos();
            string sql = string.Empty;
            sql = "select max_capacity from rackrow_master where row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
            DataSet dsm = da.select_method_wo_parameter(sql, "text");
            if (dsm.Tables[0].Rows.Count > 0)
            {
                ddlmaxcap.Text = Convert.ToString(dsm.Tables[0].Rows[0]["max_capacity"]);
            }
            else
                ddlmaxcap.Text = "0";
            string boktype = string.Empty;
            if (ddlsearchbook.SelectedItem.Text == "Books")
            {
                if (cbref.Checked == true)
                    boktype = "REF";
                else
                    boktype = "BOK";
            }
            sql = "select count(rack_allocation.rack_no) as count from rack_allocation,rackrow_master where rackrow_master.lib_code=rack_allocation.lib_code and rackrow_master.rack_no=rack_allocation.rack_no and rackrow_master.row_no=rack_allocation.row_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_allocation.book_type= '" + boktype + "' group by max_capacity";
            DataSet dsm1 = da.select_method_wo_parameter(sql, "text");
            if (dsm1.Tables[0].Rows.Count > 0)
            {
                ddlbooks.Text = Convert.ToString(dsm1.Tables[0].Rows[0]["count"]);
            }
            else
                ddlbooks.Text = "0";
            sql = "select max_capacity from rack_master where rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
            DataSet dsm12 = da.select_method_wo_parameter(sql, "text");
            if (dsm12.Tables[0].Rows.Count > 0)
            {
                ddlmaxcap1.Text = Convert.ToString(dsm12.Tables[0].Rows[0]["max_capacity"]);
            }
            else
                ddlmaxcap1.Text = "0";
            sql = "select count(rack_allocation.rack_no) as count from rack_allocation,rackrow_master where rackrow_master.lib_code=rack_allocation.lib_code and rackrow_master.rack_no=rack_allocation.rack_no and rackrow_master.row_no=rack_allocation.row_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (rack_allocation.book_type='REF' or rack_allocation.book_type='BOK') group by max_capacity";
            DataSet dsm13 = da.select_method_wo_parameter(sql, "text");
            if (dsm13.Tables[0].Rows.Count > 0)
            {
                ddlbooksav.Text = Convert.ToString(dsm13.Tables[0].Rows[0]["count"]);
            }
            else
                ddlbooksav.Text = "0";
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void ddlshelf1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            pos();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void ddlposi_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            place();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void ddlposition_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            place1();
            string sql = string.Empty;
            sql = "SELECT ISNULL(Max_Capacity,0) Max_Capacity FROM RowPos_Master ";
            sql = sql + "WHERE Rack_No ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' AND Row_No ='" + Convert.ToString(ddlshelf.SelectedValue) + "' AND Pos_No='" + Convert.ToString(ddlposition.SelectedItem) + "' AND Lib_Code ='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
            DataSet dsm = da.select_method_wo_parameter(sql, "text");
            if (dsm.Tables[0].Rows.Count > 0)
            {
                TextBox1.Text = Convert.ToString(dsm.Tables[0].Rows[0]["Max_Capacity"]);
            }
            else
                TextBox1.Text = "0";
            string j = string.Empty;
            for (int i = 0; i < dsm.Tables[0].Rows.Count; i++)
            {
                j = Convert.ToString(i);
                sql = d2.GetFunction("SELECT ISNULL(COUNT(*),0) FROM Rack_Allocation WHERE Rack_No ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' AND Row_No ='" + Convert.ToString(ddlshelf.SelectedValue) + "' AND Pos_No ='" + Convert.ToString(ddlposition.SelectedItem) + "' AND Pos_Place =" + i + " AND Lib_Code ='" + Convert.ToString(ddl_library.SelectedValue) + "' ");
                if (sql == "0")
                    ddlplace.Items.Add(j);
            }
            TextBox2.Text = d2.GetFunction("SELECT ISNULL(COUNT(*),0) FROM Rack_Allocation WHERE Rack_No ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' AND Row_No ='" + Convert.ToString(ddlshelf.SelectedValue) + "' AND Pos_No ='" + Convert.ToString(ddlposition.SelectedItem) + "' AND Lib_Code ='" + Convert.ToString(ddl_library.SelectedValue) + "' ");
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void rdbrackto_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            col.Visible = true;
            col1.Visible = true;
            Lblmoveself.Visible = true;
            ddlshelf.Visible = true;
            col99.Visible = true;
            col6.Visible = true;
            Tr1.Visible = true;
            Lblmoveto1.Text = "MoveToRack";
            Button5.Visible = true;
            bindrack();
            bindshelf();
            bindshelf1();
            pos();
            pos1();
            place();
            place1();
            ddlrack2.Enabled = true;
            ddlself1.Enabled = true;
            ddlposi.Enabled = true;
            ddlpla.Enabled = true;
            ddlshelf_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void rdbLibrary_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            col6.Visible = true;
            Lblmoveself.Visible = false;
            ddlshelf.Visible = false;
            col99.Visible = true;
            col.Visible = false;
            col1.Visible = false;
            Tr1.Visible = false;
            Lblmoveto1.Text = "MoveToLibrary";
            Button5.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void rdbtrans_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            col6.Visible = true;
            Lblmoveself.Visible = false;
            ddlshelf.Visible = false;
            col99.Visible = true;
            col.Visible = false;
            col1.Visible = false;
            Tr1.Visible = false;
            Lblmoveto1.Text = "MoveTo";
            Button5.Visible = false;
            Bookdept();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void rdbreturn_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            col6.Visible = true;
            Lblmoveself.Visible = false;
            ddlshelf.Visible = false;
            col99.Visible = true;
            col.Visible = false;
            col1.Visible = false;
            Tr1.Visible = false;
            Lblmoveto1.Text = "MoveToLibrary";
            Button5.Visible = false;
            Bookdept();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void rdbissue_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            col6.Visible = true;
            Lblmoveself.Visible = false;
            ddlshelf.Visible = false;
            col99.Visible = true;
            col.Visible = false;
            col1.Visible = false;
            Tr1.Visible = false;
            Lblmoveto1.Text = "IssueToDept";
            Button5.Visible = false;
            Bookdept();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Book Statistic");
        }
    }

    protected void grdBooks_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdBooks.PageIndex = e.NewPageIndex;
        Go_Click(sender, e);
    }

    protected void Go_Click(object sender, EventArgs e)
    {
        try
        {
            string boktype = string.Empty;
            string category_var = string.Empty;
            string opt = string.Empty;
            string Sql = string.Empty;
            string sqlIcount = string.Empty;
            string sqlBcount = string.Empty;
            DataSet bookallo = new DataSet();
            int sno = 0;

            if (ddlsearchbook.SelectedItem.Text == "Books")
            {
                if (cbref.Checked == true)
                    boktype = "REF";
                else
                    boktype = "BOK";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Project Books")
            {
                boktype = "PRO";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Non Book Materials")
            {
                boktype = "NBM";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Back Volume")
            {
                boktype = "BVO";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Periodical")
            {
                boktype = "PER";
            }

            if (Cboldsearch.Checked == true || ddlbook.SelectedItem.Text == "All")

                if (Convert.ToString(ddlCategory.SelectedItem) == "" || Convert.ToString(ddlCategory.SelectedItem) == "All")
                    category_var = "category like '%'";
                else
                    category_var = "category='" + ddlCategory + "'";
            if (rdbrackto.Checked == true)
            {
                if (boktype == "BOK")
                {
                    if (ddlbook.SelectedItem.Text == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (ddlbook.SelectedItem.Text == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (ddlbook.SelectedItem.Text == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author ,publisher,edition ,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";
                            if (ddlposi.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_No ='" + ddlposi.SelectedItem.Text + "'";
                            if (ddlpla.SelectedItem.Text != "")
                                Sql = Sql + "AND Pos_Place ='" + ddlpla.SelectedItem.Text + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                {
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                            }
                            sqlIcount = "select count(*) BookCount from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF') and book_status ='Issued'";
                            sqlBcount = "select count(*) BookCount from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF') and book_status ='Binding'";
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";
                            if (ddlposi.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_No ='" + ddlposi.SelectedItem.Text + "' ";
                            if (ddlpla.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_Place ='" + ddlpla.SelectedItem.Text + "' ";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            sqlIcount = "select count(*) BookCount from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF') and book_status = 'Issued'";
                            sqlBcount = "select count(*) BookCount from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF')  and book_status ='Binding'";
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and (book_type='BOK' or book_type='REF')";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            sqlIcount = "select count(*) BookCount from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and (book_type='BOK' or book_type='REF') and book_status = 'Issued'";
                            sqlBcount = "select count(*) BookCount from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and (book_type='BOK' or book_type='REF')  and book_status ='Binding'";
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code)  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and book_type='BOK'";
                            if (Convert.ToString(ddlposi.SelectedItem) != "")
                                Sql = Sql + " AND Pos_No ='" + Convert.ToString(ddlposi.SelectedItem) + "' ";
                            if (Convert.ToString(ddlpla.SelectedItem) != "")
                                Sql = Sql + " AND Pos_Place ='" + Convert.ToString(ddlpla.SelectedItem) + "' ";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            sqlIcount = "select count(*) BookCount from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code)  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and book_type='BOK' and book_status = 'Issued'";
                            sqlBcount = "select count(*) BookCount from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code)  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and book_type='BOK'  and book_status ='Binding'";
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and book_type='BOK'";

                            if (ddlposi.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_No ='" + ddlposi.SelectedItem.Text + "' ";
                            if (ddlpla.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_Place ='" + ddlpla.SelectedItem.Text + "' ";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            sqlIcount = "select count(*) BookCount from from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and book_type='BOK' and book_status = 'Issued'";
                            sqlBcount = "select count(*) BookCount from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and book_type='BOK'  and book_status ='Binding'";
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and book_type='BOK'";
                            if (ddlposi.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_No ='" + ddlposi.SelectedItem.Text + "' ";
                            if (ddlpla.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_Place ='" + ddlpla.SelectedItem.Text + "' ";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            sqlIcount = "select count(*) BookCount from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and book_type='BOK' and book_status = 'Issued'";
                            sqlBcount = "select count(*) BookCount from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'  and book_type='BOK'  and book_status ='Binding'";
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }

                if (boktype == "REF")
                {
                    if (ddlbook.SelectedItem.Text == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (ddlbook.SelectedItem.Text == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (ddlbook.SelectedItem.Text == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";
                            if (ddlposi.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_No ='" + ddlposi.SelectedItem.Text + "'";
                            if (ddlpla.SelectedItem.Text != "")
                                Sql = Sql + "AND Pos_Place ='" + ddlpla.SelectedItem.Text + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql = "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                {
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                            }
                            sqlIcount = "select count(*) BookCount from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF') book_status = 'Issued'";
                            sqlBcount = "select count(*) BookCount from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF')  and book_status ='Binding'";
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";
                            if (ddlposi.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_No ='" + ddlposi.SelectedItem.Text + "' ";
                            if (ddlpla.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_Place ='" + ddlpla.SelectedItem.Text + "' ";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            sqlIcount = "select count(*) BookCount from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + ddl_library.SelectedValue + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF') and book_status = 'Issued'";
                            sqlBcount = "select count(*) BookCount from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + ddl_library.SelectedValue + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF')  and book_status ='Binding'";
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";
                            if (ddlposi.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_No ='" + ddlposi.SelectedItem.Text + "' ";
                            if (ddlpla.SelectedItem.Text != "")
                                Sql = Sql + " AND Pos_Place ='" + ddlpla.SelectedItem.Text + "' ";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            sqlIcount = "select count(*) BookCount from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF') and book_status = 'Issued'";
                            sqlBcount = "select count(*) BookCount from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and (book_type='BOK' or book_type='REF')  and book_status ='Binding'";
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }
                if (boktype == "NBM")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Non Book Materials")
                        opt = "nonbookmat_no'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code) where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='NBM'";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code) where  " + opt + " and nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='NBM'";
                    else
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat  left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code) where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='NBM'";
                }
                if (boktype == "BVO")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "access_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code) where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='BVO' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code) where  " + opt + " and back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='BVO'";
                    else
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code) where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='BVO'";
                }
                if (boktype == "PRO")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Project Books")
                        opt = "probook_accno'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code) where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PRO'";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code) where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PRO'";
                    else
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code) where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PRO'";
                }
                if (boktype == "PER")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_name like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code) where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PER' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code)  where  " + opt + " and journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PER'";
                    else
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code)  where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PER'";
                }
                // if()
            }
            else if (rdbtrans.Checked == true)
            {
                if (boktype == "BOK")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author ,publisher,edition ,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and (book_type='BOK' or book_type='REF')) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";
                            if (Convert.ToString(ddlrack2.SelectedItem) != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";

                            if (Convert.ToString(ddlself1.SelectedItem) != "")
                                Sql = Sql + " and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and (book_type='BOK' or book_type='REF')) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";
                            if (Convert.ToString(ddlrack2.SelectedItem) != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";
                            if (Convert.ToString(ddlself1.SelectedItem) != "")
                                Sql = Sql + " and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and (book_type='BOK' or book_type='REF')) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";
                            if (ddlrack2.SelectedItem.Text != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and (book_type='BOK' or book_type='REF'))  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";
                            if (ddlrack2.SelectedItem.Text != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and (book_type='BOK' or book_type='REF')) where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";
                            if (ddlrack2.SelectedItem.Text != "")
                                Sql = Sql + " and rack_no='" + ddlrack2.SelectedValue + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and (book_type='BOK' or book_type='REF')) where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";

                            if (ddlrack2.SelectedItem.Text != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }
                if (boktype == "REF")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and (book_type='BOK' or book_type='REF')) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";
                            if (ddlrack2.SelectedItem.Text != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and (book_type='BOK' or book_type='REF')) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";
                            if (ddlrack2.SelectedItem.Text != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and (book_type='BOK' or book_type='REF')) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";
                            if (ddlrack2.SelectedItem.Text != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK')  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";

                            if (ddlrack2.SelectedItem.Text != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK') where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";

                            if (Convert.ToString(ddlrack2.SelectedItem) != "")
                                Sql = Sql + " and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK') where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and dept_code ='" + ddldeptm.SelectedValue + "'";
                            if (ddlrack2.SelectedItem.Text != "")
                                Sql = Sql + " and rack_no='" + ddlrack2.SelectedValue + "'";
                            if (ddlself1.SelectedItem.Text != "")
                                Sql = Sql + " and row_no='" + ddlself1.SelectedValue + "'";
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }
                if (boktype == "NBM")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Non Book Materials")
                        opt = "nonbookmat_no'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code) where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='NBM'";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code) where  " + opt + " and nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='NBM'";
                    else
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat  left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code) where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='NBM'";
                }
                if (boktype == "BVO")
                {
                    if (ddlbook.SelectedItem.Text == "Access Number")
                        opt = "access_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (ddlbook.SelectedItem.Text == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code) where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='BVO' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code) where  " + opt + " and back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='BVO'";
                    else
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code) where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='BVO'";
                }
                if (boktype == "PRO")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Project Books")
                        opt = "probook_accno'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code) where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PRO'";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code) where  " + opt + "  and project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PRO'";
                    else
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code) where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PRO'";
                }
                if (boktype == "PER")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_name like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code) where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PER' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code)  where  " + opt + " and journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PER'";
                    else
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code)  where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_no='" + Convert.ToString(ddlrack2.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlself1.SelectedValue) + "' and book_type='PER'";
                }
            }
            else if (rdbissue.Checked == true)
            {
                if (boktype == "BOK")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Department")
                        opt = "bookdetails.dept_code like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Author")
                        opt = "bookdetails.author like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and isnull(transfered,0) = 0";//' and rack_flag = 0";
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0";// ' and rack_flag = 0";
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0";//' and rack_flag = 0";
                        }
                        if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                        {
                            int parsedValue;
                            if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                            {
                                Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            else
                                Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            string catego = Convert.ToString(ddlCategory.SelectedItem);
                            if (catego == "All" || catego == "")
                            {
                                Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK')  where   ref = 'No' and   category like '%' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0"; // ' and rack_flag = 0 and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and "

                            }
                            else
                            {
                                Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK')  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and category='" + ddlCategory.SelectedValue + "'  and isnull(transfered,0) = 0";  // ' and rack_flag = 0  "

                            }
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK') where    " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0   " + category_var + "";
                            //' and rack_flag = 0" ref = 'No'
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK') where     bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0   " + category_var + ""; //  ' and rack_flag = 0" ref = 'No' and
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }

                if (boktype == "REF")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0";// ' and rack_flag = 0"
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0";//' and rack_flag = 0"
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0";//' and rack_flag = 0"
                        }

                        if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                        {
                            int parsedValue;
                            if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                            {
                                Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            else
                                Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code)  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0";//' and rack_flag = 0"
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'  and isnull(transfered,0) = 0";//' and rack_flag = 0"

                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and isnull(transfered,0) = 0 ";// ' and rack_flag = 0"
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }
                if (boktype == "NBM")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Non Book Materials")
                        opt = "nonbookmat_no'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code and book_type='NBM') where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code and book_type='NBM') where  " + opt + " and nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat  left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code and book_type='NBM') where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
                if (boktype == "BVO")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "access_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code and book_type='BVO') where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code and book_type='BVO') where  " + opt + " and back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code and book_type='BVO') where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
                if (boktype == "PRO")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Project Books")
                        opt = "probook_accno'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code and book_type='PRO') where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code and book_type='PRO') where  " + opt + "  and project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code and book_type='BOK') where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
                if (boktype == "PER")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_name like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code and book_type='PER') where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
                    else if (ddlbook.SelectedItem.Text != "All" && txt_booksearch.Text != "")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code and book_type='PER')  where  " + opt + " and journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code and book_type='PER')  where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
            }
            else if (rdbreturn.Checked == true)
            {
                if (boktype == "BOK")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Department")
                        opt = "bookdetails.dept_code like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Author")
                        opt = "bookdetails.author like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0 ";// ' and rack_flag = 0
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0";//' and rack_flag = 0"
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code   from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0"; //' and rack_flag = 0"
                        }
                        if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                        {
                            int parsedValue;
                            if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                            {
                                Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            else
                                Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            if (Convert.ToString(ddlCategory.SelectedItem) == "All" || Convert.ToString(ddlCategory.SelectedItem) == "")
                            {
                                Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code    from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK')  where   ref = 'No' and category like '%' and transfer_type = 2 and isnull(returned,0) = 0 and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";//   ' and rack_flag = 0  and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' "
                            }
                            else
                            {
                                Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code    from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK')  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and category='" + ddlCategory.SelectedValue + "' and transfer_type = 2 and isnull(returned,0) = 0";//  ' and rack_flag = 0  "
                            }
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            if (Convert.ToString(ddldeptm.SelectedItem) != "All" && Convert.ToString(ddldeptm.SelectedItem) != "")
                            {
                                Sql = Sql + " and book_transfer.to_lib_code ='" + Convert.ToString(ddldeptm.SelectedItem) + "' ";
                            }
                        }
                        else if (ddlbook.SelectedItem.Text != "All" && ddlbook.SelectedItem.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code    from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no   left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK') where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0   " + category_var + ""; // ' and rack_flag = 0"
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            if (ddldeptm.SelectedItem.Text != "All")
                            {
                                Sql = Sql + " and book_transfer.to_lib_code ='" + ddldeptm.SelectedItem.Text + "' ";
                            }
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code    from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK') where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0  " + category_var + "";//  ' and rack_flag = 0"
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            if (ddldeptm.SelectedItem.Text != "All")
                            {
                                Sql = Sql + " and book_transfer.to_lib_code ='" + ddldeptm.SelectedItem.Text + "' ";
                            }
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }

                if (boktype == "REF")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code    from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0";// ' and rack_flag = 0"
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code    from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0";// ' and rack_flag = 0"
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code    from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0";// ' and rack_flag = 0"
                        }
                        if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                        {
                            int parsedValue;
                            if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                            {
                                Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            else
                                Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code    from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code)  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0";// ' and rack_flag = 0"
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code    from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0";// ' and rack_flag = 0"
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code   from book_transfer inner join bookdetails on book_transfer.acc_no = bookdetails.acc_no  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and transfer_type = 2 and isnull(returned,0) = 0";// ' and rack_flag = 0"
                        }
                        if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                        {
                            int parsedValue;
                            if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                            {
                                Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            else
                                Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }

                if (boktype == "NBM")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Non Book Materials")
                        opt = "nonbookmat_no'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code and book_type='NBM') where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code and book_type='NBM') where  " + opt + " and nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat  left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code and book_type='NBM') where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
                if (boktype == "BVO")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "access_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code and book_type='BVO') where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code and book_type='BVO') where  " + opt + " and back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code and book_type='BVO') where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
                if (boktype == "PRO")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Project Books")
                        opt = "probook_accno'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code and book_type='PRO') where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code and book_type='PRO') where  " + opt + "  and project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code and book_type='BOK') where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
                if (boktype == "PER")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_name like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code and book_type='PER') where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code and book_type='PER')  where  " + opt + " and journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code and book_type='PER')  where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
            }
            else
            {
                if (boktype == "BOK")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Department")
                        opt = "bookdetails.dept_code like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Author")
                        opt = "bookdetails.author like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";// ' and rack_flag = 0"
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";// ' and rack_flag = 0"
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";// ' and rack_flag = 0"
                        }
                        if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                        {
                            int parsedValue;
                            if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                            {
                                Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            else
                                Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            if (Convert.ToString(ddlCategory.SelectedItem) == "All" || Convert.ToString(ddlCategory.SelectedItem) == "")
                            {
                                Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK')  where   ref = 'No'  and category like '%' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";//   ' and rack_flag = 0  and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'"
                            }
                            else
                            {
                                Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK')  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and category='" + ddlCategory.SelectedValue + "'";//   ' and rack_flag = 0  "
                            }
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK') where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'   " + category_var + "";//  ' and rack_flag = 0"
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code and book_type='BOK') where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'   " + category_var + "";//  ' and rack_flag = 0"
                            if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                            {
                                int parsedValue;
                                if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                                {
                                    Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                    Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                    Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                                }
                                else
                                    Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }
                if (boktype == "REF")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "bookdetails.title like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "bookdetails.acc_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Call No")
                        opt = "bookdetails.call_no like '" + (txt_booksearch.Text).Trim() + "%'";
                    if (cbref.Checked == true)
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";// ' and rack_flag = 0"

                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'Yes' and " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";//' and rack_flag = 0"

                        }
                        else
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where    ref = 'Yes' and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";// ' and rack_flag = 0"
                        }
                        if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                        {
                            int parsedValue;
                            if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                            {
                                Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            else
                                Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                        }
                    }
                    else
                    {
                        if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code)  where   ref = 'No' and  bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";//' and rack_flag = 0"
                        }
                        else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        {
                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  ref = 'No' and  " + opt + "  and bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";// ' and rack_flag = 0"
                        }
                        else
                        {

                            Sql = "select bookdetails.acc_no,title ,author,publisher,edition,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position,call_no,isnull(dept_code,'') dept_code  from bookdetails left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where   ref = 'No' and   bookdetails.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";// ' and rack_flag = 0"
                        }
                        if (Txtfromacc.Text != "" && Txttoacc.Text != "")
                        {
                            int parsedValue;
                            if (!int.TryParse(Txtfromacc.Text, out parsedValue))
                            {
                                Sql = Sql + "AND CASE WHEN IsNumeric(right(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "'";
                                Sql = Sql + "and case when isnumeric(left(bookdetails.acc_no,1)) = 1 then cast(substring(bookdetails.acc_no,(PATINDEX('%[0-9]%',";
                                Sql = Sql + "bookdetails.acc_no)),len(bookdetails.acc_no))as int) end between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                            }
                            else
                                Sql = Sql + " and bookdetails.acc_no between '" + Txtfromacc.Text + "' and '" + Txttoacc.Text + "' ";
                        }
                    }
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }
                if (boktype == "NBM")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Non Book Materials")
                        opt = "nonbookmat_no'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code and book_type='NBM') where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code and book_type='NBM') where  " + opt + " and nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select nonbookmat_no,title ,author,publisher,volume,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from nonbookmat  left join rack_allocation on (nonbookmat.nonbookmat_no=rack_allocation.acc_no and rack_allocation.lib_code=nonbookmat.lib_code and book_type='NBM') where  nonbookmat.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
                if (boktype == "BVO")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Access Number")
                        opt = "access_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code and book_type='BVO') where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code and book_type='BVO') where  " + opt + " and back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select access_code,title ,publisher,remarks,volumeno,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from back_volume left join rack_allocation on (back_volume.access_code=rack_allocation.acc_no and rack_allocation.lib_code=back_volume.lib_code and book_type='BVO') where  back_volume.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                }
                if (boktype == "PRO")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Project Books")
                        opt = "probook_accno'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Title")
                        opt = "title like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code and book_type='PRO') where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code and book_type='PRO') where  " + opt + "  and project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select probook_accno,title ,roll_no,name,guide_name,rack_no,row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from project_book left join rack_allocation on (project_book.probook_accno=rack_allocation.acc_no and rack_allocation.lib_code=project_book.lib_code and book_type='BOK') where  project_book.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
                if (boktype == "PER")
                {
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_code'" + (txt_booksearch.Text).Trim() + "%'";
                    if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
                        opt = "journal_name like '" + (txt_booksearch.Text).Trim() + "%'";

                    if (Convert.ToString(ddlbook.SelectedItem) == "All")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code and book_type='PER') where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' ";
                    else if (Convert.ToString(ddlbook.SelectedItem) != "All" && txt_booksearch.Text != "")
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code and book_type='PER')  where  " + opt + " and journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    else
                        Sql = "select journal_code,journal_name,journal_master.access_date,rack_no,rack_allocation.row_no,isnull(pos_no,'')+' - '+isnull(pos_place,'') Position from journal_master left join rack_allocation on (journal_master.journal_code=rack_allocation.acc_no and rack_allocation.lib_code=journal_master.lib_code and book_type='PER')  where  journal_master.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                }
            }
            bookallo = d2.select_method_wo_parameter(Sql, "Text");
            int ii = 0;

            if (grdBooks.Rows.Count != 0)
            {
                bokaloca.Columns.Add("Acc No", typeof(string));
                bokaloca.Columns.Add("Title", typeof(string));
                bokaloca.Columns.Add("Edition", typeof(string));
                bokaloca.Columns.Add("Status", typeof(string));
                bokaloca.Columns.Add("Rack", typeof(string));
                bokaloca.Columns.Add("Shelves", typeof(string));
                bokaloca.Columns.Add("Department", typeof(string));
            }
            if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
            {
                for (int row = ii; row < bookallo.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drallo = bokaloca.NewRow();
                    drallo["Acc No"] = Convert.ToString(bookallo.Tables[0].Rows[row]["acc_no"]);
                    drallo["Title"] = Convert.ToString(bookallo.Tables[0].Rows[row]["Title"]);
                    drallo["Edition"] = Convert.ToString(bookallo.Tables[0].Rows[row]["edition"]);
                    drallo["Status"] = "Not Allocated";
                    drallo["Rack"] = Convert.ToString(bookallo.Tables[0].Rows[row]["rack_no"]);
                    drallo["Shelves"] = Convert.ToString(bookallo.Tables[0].Rows[row]["row_no"]);
                    drallo["Department"] = Convert.ToString(bookallo.Tables[0].Rows[row]["dept_code"]);
                    bokaloca.Rows.Add(drallo);
                }
                grdBooks.DataSource = bokaloca;
                grdBooks.DataBind();
                grdBooks.Visible = true;
                if (grdBooks.Rows.Count > 0)
                {

                    CheckBox select = grdBooks.Rows[1].FindControl("chkenbl") as CheckBox;
                    select.Visible = false;
                }
            }
            else
            {
                grdBooks.DataSource = bokaloca;
                grdBooks.DataBind();
                grdBooks.Visible = true;
                if (grdBooks.Rows.Count > 0)
                {

                    CheckBox select = grdBooks.Rows[1].FindControl("chkenbl") as CheckBox;
                    select.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation");
        }
    }

    protected void RowHead(GridView grdBooks)
    {
        for (int head = 0; head < 1; head++)
        {
            grdBooks.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grdBooks.Rows[head].Font.Bold = true;
            grdBooks.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    protected void btntrans_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            string var_accessno = string.Empty;
            // string Sql=string.Empty;
            DataSet iss = new DataSet();
            DataSet issus = new DataSet();
            string boktype = string.Empty;
            int f = 0;
            string nowtime = DateTime.Now.ToString();
            string[] spl = nowtime.Split();
            string[] Spl1 = spl[1].Split(':');
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Books")
            {
                if (cbref.Checked == true)
                    boktype = "REF";
                else
                    boktype = "BOK";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Project Books")
            {
                boktype = "PRO";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Non Book Materials")
            {
                boktype = "NBM";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Back Volume")
            {
                boktype = "BVO";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Periodical")
            {
                boktype = "PER";
            }
            if (Convert.ToString(ddl_library.SelectedValue) == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select the Library";
            }

            //if (rdbrack.Checked == true || rdbrackto.Checked == true || rdbLibrary.Checked == true || rdbissue.Checked == true || rdbreturn.Checked == true)
            //{
            if (rdbLibrary.Checked == true || rdbrack.Checked == true || rdbrackto.Checked == true)
            {
                if (Convert.ToString(ddlmoveto.SelectedValue) == "")
                {
                    DivIssue.Visible = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select the Library Name into which the Selected books are to be Moved";
                }
                else
                {
                    LblIssuesName.Text = "Are You Sure To Move The Selected Book(s) ?Do You Want to Continue";
                    DivIssue.Visible = true;
                    return;
                }
            }
            if (rdbissue.Checked == true || rdbreturn.Checked == true)
            {
                if (Convert.ToString(ddlmoveto.SelectedValue) == "")
                {
                    DivIssue.Visible = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select the Department Name into which the Selected books are to be Moved";
                }
                else
                {
                    LblIssuesName.Text = "Are You Sure To Move The Selected Book(s) ?Do You Want to Continue";
                    DivIssue.Visible = true;
                    return;
                }
            }
            if (rdbreturn.Checked == true)
            {
                if (Convert.ToString(ddldeptm.SelectedValue) == "")
                {
                    DivIssue.Visible = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select the Department Name into which the Selected books are to be Moved";
                }
                else
                {
                    LblIssuesName.Text = "Are You Sure To Move The Selected Book(s) ?Do You Want to Continue";
                    DivIssue.Visible = true;
                    return;
                }
            }
            // }
        }
        catch
        {

        }
    }

    protected void btnIssueYes_Click(object sender, EventArgs e)
    {
        try
        {
            DivIssue.Visible = false;
            string Sql = string.Empty;
            string var_accessno = string.Empty;
            // string Sql=string.Empty;
            DataSet iss = new DataSet();
            DataSet issus = new DataSet();
            string boktype = string.Empty;
            int f = 0;
            string nowtime = DateTime.Now.ToString();
            string[] spl = nowtime.Split();
            string[] Spl1 = spl[1].Split(':');

            string accessdate = string.Empty;
            string Accdate1 = Convert.ToString(txt_transdate.Text);
            string[] adate1 = Accdate1.Split('/');
            if (adate1.Length == 3)
                accessdate = adate1[2].ToString() + "-" + adate1[1].ToString() + "-" + adate1[0].ToString();

            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Books")
            {
                if (cbref.Checked == true)
                    boktype = "REF";
                else
                    boktype = "BOK";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Project Books")
            {
                boktype = "PRO";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Non Book Materials")
            {
                boktype = "NBM";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Back Volume")
            {
                boktype = "BVO";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Periodical")
            {
                boktype = "PER";
            }
            if (Convert.ToString(ddl_library.SelectedValue) == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select the Library";
            }
            if (rdbLibrary.Checked == true)
            {
                if (boktype == "BOK")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from bookdetails where book_status in ('Issued','Binding')  and acc_no in ( '" + var_accessno + "')  and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");
                            }
                            Sql = "update bookdetails set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "',rack_flag =0 where acc_no ='" + var_accessno + "' ";
                            //   and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',0)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");
                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";
                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }


                if (boktype == "REF")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from bookdetails where book_status in ('Issued','Binding')  and acc_no = '" + var_accessno + "'  and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";

                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";

                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";

                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";

                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update bookdetails set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' , rack_flag = 0 where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                            int up2 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',0)";

                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "BVO")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from back_volume where issue_flag in ('Issued','Binding')  and access_code = '" + var_accessno + "'";

                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BVO')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BVO')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }
                            Sql = "update back_volume set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "'  where access_code = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',0)";

                            int ins = d2.update_method_wo_parameter(Sql, "Text");

                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "PRO")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from project_book where issue_flag in ('Issued','Binding')  and probook_accno = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Project Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PRO')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PRO')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }
                            Sql = "update project_book set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' where probook_accno = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',0)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");

                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "NBM")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from nonbookmat where issue_flag in ('Issued','Binding')  and nonbookmat_no = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Non Book Material with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='NBM')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='NBM')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }
                            Sql = "update nonbookmat set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' where nonbookmat_no = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',0)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");

                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "PER")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from journal where issue_flag in ('Issued','Binding')  and journal_code = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Periodicals may be under issue or binding ,So Transfer Is Not Allowed ";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PER')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PER')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");
                            }
                            Sql = "update journal_master set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "',rackno='Nil',row_no='Nil' where journal_code = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',0)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");

                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Periodical(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }

            }
            #region dept trans
            else if (rdbtrans.Checked == true)
            {
                //if (Convert.ToString(ddlmoveto.SelectedValue) == "")
                //{
                //    alertpopwindow.Visible = true;
                //    lblalerterr.Text = "Select the Department Name into which the Selected books are to be Issued";
                //}
                //else
                //{


                if (boktype == "BOK")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from bookdetails where book_status in ('Issued','Binding')  and acc_no = '" + var_accessno + "'  and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update bookdetails set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' , rack_flag = 0 where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',0)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }


                if (boktype == "REF")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from bookdetails where book_status in ('Issued','Binding')  and acc_no = '" + var_accessno + "'  and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";

                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";

                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";

                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";

                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update bookdetails set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' , rack_flag = 0 where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',0)";

                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "BVO")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from back_volume where issue_flag in ('Issued','Binding')  and access_code = '" + var_accessno + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Back Volume with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BVO')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BVO')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update back_volume set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "'  where access_code = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',1)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "PRO")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from project_book where issue_flag in ('Issued','Binding')  and probook_accno = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Project Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PRO')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PRO')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update project_book set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' where probook_accno = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',1)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "NBM")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from nonbookmat where issue_flag in ('Issued','Binding')  and nonbookmat_no = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Non Book Material with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='NBM')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='NBM')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update nonbookmat set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' where nonbookmat_no = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',1)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }

                if (boktype == "PER")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from journal where issue_flag in ('Issued','Binding')  and journal_code = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Periodicals may be under issue or binding ,So Transfer Is Not Allowed";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PER')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PER')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update journal_master set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "',rackno='Nil',row_no='Nil' where journal_code = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "update journal set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "'where journal_code = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',1)";
                            int ins1 = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Periodical(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }


            }
            #endregion

            #region issudept
            else if (rdbissue.Checked == true)
            {
                //if (Convert.ToString(ddlmoveto.SelectedValue) == "")
                //{
                //    alertpopwindow.Visible = true;
                //    lblalerterr.Text = "Select the Department Name into which the Selected books are to be Issued";
                //}
                //else
                //{


                if (boktype == "BOK")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from bookdetails where book_status in ('Issued','Binding')  and acc_no = '" + var_accessno + "'  and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";

                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";

                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";

                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "'  and (book_type='BOK' or book_type='REF')";
                                //   and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }


                            Sql = "update bookdetails set rack_flag = 0,Transfered = 1,Book_Status ='Transfered' where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type,returned) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2,0)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");



                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }


                if (boktype == "REF")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from bookdetails where book_status in ('Issued','Binding')  and acc_no = '" + var_accessno + "'  and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";


                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";

                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";

                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";


                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";


                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update bookdetails set rack_flag = 0,Transfered = 1,Book_Status ='Transfered' where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";


                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type,returned) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2,0)";


                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }

                if (boktype == "BVO")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from back_volume where issue_flag in ('Issued','Binding')  and access_code = '" + var_accessno + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Back Volume with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BVO')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BVO')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }
                            Sql = "update back_volume set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "'  where access_code = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "PRO")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from project_book where issue_flag in ('Issued','Binding')  and probook_accno = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Project Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PRO')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PRO')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }
                            Sql = "update project_book set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' where probook_accno = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "NBM")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from nonbookmat where issue_flag in ('Issued','Binding')  and nonbookmat_no = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Non Book Material with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='NBM')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='NBM')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }
                            Sql = "update nonbookmat set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' where nonbookmat_no = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "PER")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from journal where issue_flag in ('Issued','Binding')  and journal_code = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Periodicals may be under issue or binding ,So Transfer Is Not Allowed";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PER')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PER')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }
                            Sql = "update journal_master set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "',rackno='Nil',row_no='Nil' where journal_code = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "update journal set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "'where journal_code = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2)";
                            int ins1 = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Periodical(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }

                //  }
            }
            #endregion

            #region deptreturn
            else if (rdbreturn.Checked == true)
            {
                //if (Convert.ToString(ddlmoveto.SelectedValue) == "")
                //{
                //    alertpopwindow.Visible = true;
                //    lblalerterr.Text = "Select the Department Name into which the Selected books are to be Issued";
                //}
                //else
                //{


                if (boktype == "BOK")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from bookdetails where book_status in ('Issued','Binding')  and acc_no = '" + var_accessno + "'  and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";


                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";

                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";


                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";

                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";

                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update bookdetails set rack_flag = 0,Transfered = 0,Book_Status ='Available' where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";

                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "UPDATE book_transfer set returned = 1 where acc_no ='" + var_accessno + "' and transfer_type = 2 and isnull(returned,0) = 0 ";

                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "BVO")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from back_volume where issue_flag in ('Issued','Binding')  and access_code = '" + var_accessno + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Back Volume with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BVO')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BVO')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update back_volume set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "'  where access_code = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);

                }
                if (boktype == "PRO")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from project_book where issue_flag in ('Issued','Binding')  and probook_accno = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Project Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PRO')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PRO')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update project_book set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' where probook_accno = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "NBM")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from nonbookmat where issue_flag in ('Issued','Binding')  and nonbookmat_no = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Non Book Material with Access no. " + var_accessno + " cannot be moved as it is under issue";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='NBM')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='NBM')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update nonbookmat set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "' where nonbookmat_no = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2)";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }
                if (boktype == "PER")
                {
                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            Sql = "select * from journal where issue_flag in ('Issued','Binding')  and journal_code = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Periodicals may be under issue or binding ,So Transfer Is Not Allowed";
                            }
                            else
                            {
                                f = 1;
                            }
                            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PER')";
                            issus = d2.select_method_wo_parameter(Sql, "text");
                            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                            {
                                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retu = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";
                                int retus = d2.update_method_wo_parameter(Sql, "Text");
                                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='PER')";
                                int up = d2.update_method_wo_parameter(Sql, "Text");


                            }

                            Sql = "update journal_master set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "',rackno='Nil',row_no='Nil' where journal_code = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int up1 = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "update journal set lib_code ='" + Convert.ToString(ddlmoveto.SelectedValue) + "'where journal_code = '" + var_accessno + "'and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";
                            int ins = d2.update_method_wo_parameter(Sql, "Text");
                            Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type) VALUES ('" + Convert.ToString(ddldeptm.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" + accessdate + "','" + boktype + "',2)";
                            int ins1 = d2.update_method_wo_parameter(Sql, "Text");


                            if (f == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Periodical(s) are Successfully Transferred .......!";

                            }

                        }
                    }
                    Go_Click(sender, e);
                    Btngo1_Click(sender, e);
                }








                //if (boktype == "REF")
                //{
                //    for (int row = 0; row < Fpspread.Sheets[0].RowCount; row++)
                //    {
                //        int selected = 0;
                //        int.TryParse(Convert.ToString(Fpspread.Sheets[0].Cells[row, 1].Value), out selected);
                //        if (selected == 1)
                //        {
                //            var_accessno = Fpspread.Sheets[0].Cells[row, 2].Text =
                //           Sql = "select * from bookdetails where book_status in ('Issued','Binding')  and acc_no = '" + var_accessno + "'  and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";


                //            iss = d2.select_method_wo_parameter(Sql, "text");
                //            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                //            {
                //                alertpopwindow.Visible = true;
                //                lblalerterr.Text = "Book with Access no. " + var_accessno + " cannot be moved as it is under issue";
                //            }
                //            else
                //            {
                //                f = 1;
                //            }
                //            Sql = "select rack_no,row_no,lib_code from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";

                //            issus = d2.select_method_wo_parameter(Sql, "text");
                //            if (issus.Tables.Count > 0 && issus.Tables[0].Rows.Count > 0)
                //            {
                //                Sql = "update rackrow_master set no_of_copies = no_of_copies - 1 where rack_no ='" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(issus.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";

                //                int retu = d2.update_method_wo_parameter(Sql, "Text");
                //                Sql = "update rack_master set no_of_copies = no_of_copies -1 where rack_no= '" + Convert.ToString(issus.Tables[0].Rows[0]["rack_no"]) + "' and lib_code = '" + Convert.ToString(issus.Tables[0].Rows[0]["lib_code"]) + "'";


                //                int retus = d2.update_method_wo_parameter(Sql, "Text");
                //                Sql = "delete from rack_allocation where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (book_type='BOK' or book_type='REF')";


                //                int up = d2.update_method_wo_parameter(Sql, "Text");


                //            }
                //            else
                //            {
                //                Sql = "update bookdetails set rack_flag = 0,Transfered = 1,Book_Status ='Transfered' where acc_no = '" + var_accessno + "' and lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'";


                //                int up = d2.update_method_wo_parameter(Sql, "Text");
                //                Sql = "INSERT INTO book_transfer (from_lib_code,to_lib_code,acc_no,transfer_date,booktype,transfer_type,returned) VALUES ('" + Convert.ToString(ddl_library.SelectedValue) + "','" + Convert.ToString(ddlmoveto.SelectedValue) + "','" + var_accessno + "','" & trans_date.Value & "','" + boktype + "',2,0)";


                //                int ins = d2.update_method_wo_parameter(Sql, "Text");

                //            }
                //            if (f == 1)
                //            {
                //                alertpopwindow.Visible = true;
                //                lblalerterr.Text = "Other than (Binding & Issued) Book(s) are Successfully Transferred .......!";
                //            }

                //        }
                //    }
                //}


                // }
            }
            #endregion

            #region rack
            else if (rdbrack.Checked == true || rdbrackto.Checked == true)
            {

                string libcode = Convert.ToString(ddl_library.SelectedValue);
                string rackno = Convert.ToString(ddlmoveto.SelectedValue);
                string rowno = Convert.ToString(ddlshelf.SelectedValue);
                string posno = Convert.ToString(ddlposition.SelectedItem);
                string placeno = Convert.ToString(ddlplace.SelectedValue);
                int inss10 = 0;

                if (Convert.ToString(ddlmoveto.SelectedValue) == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select the Rack No. into which the Selected books are to be Moved";
                }
                if (Convert.ToString(ddlshelf.SelectedValue) == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select the Shelf No. into which the Selected books are to be Moved";
                }
                else
                {
                    Sql = "select cast(max_capacity as numeric) - cast(no_of_copies as numeric), rack_no,row_no from rackrow_master  where rack_no ='" + rackno + "' and row_no = '" + rowno + "' and  lib_code = '" + Convert.ToString(ddl_library.SelectedValue) + "'";

                    foreach (GridViewRow row in grdBooks.Rows)
                    {
                        CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                        int RowCnt = Convert.ToInt32(row.RowIndex);
                        if (cbsel.Checked == true)
                        {
                            var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                            if (cbref.Checked == true)
                                Sql = "select lib_code,rack_no,row_no from rack_allocation where acc_no = '" + var_accessno + "' and book_type='REF' and lib_code='" + libcode + "'";

                            else
                                Sql = "select lib_code,rack_no,row_no from rack_allocation where acc_no = '" + var_accessno + "' and book_type='" + boktype + "' and lib_code='" + libcode + "'";
                            iss = d2.select_method_wo_parameter(Sql, "text");
                            if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count == 0)
                            {
                                if (boktype == "PER")
                                {
                                    Sql = "update journal_master set rackno='" + rackno + "',row_no='" + rowno + "',pos_no ='" + posno + "',Pos_Place ='" + placeno + "'  where lib_code='" + libcode + "' and journal_code= '" + var_accessno + "'";
                                    int ins = d2.update_method_wo_parameter(Sql, "Text");

                                }
                                if (cbref.Checked == true)
                                {
                                    Sql = "insert into rack_allocation values('" + libcode + "','" + rackno + "','" + rowno + "','" + var_accessno + "','" + spl[0] + "','" + spl[1] + "','REF','" + posno + "','" + placeno + "')";
                                    int inss = d2.update_method_wo_parameter(Sql, "Text");

                                    Sql = "update bookdetails set lib_code ='" + libcode + "' , rack_flag = 1 where acc_no = '" + var_accessno + "'and lib_code ='" + libcode + "'";
                                    int upp = d2.update_method_wo_parameter(Sql, "Text");


                                }
                                else
                                {
                                    Sql = "insert into rack_allocation values('" + libcode + "','" + rackno + "','" + rowno + "','" + var_accessno + "','" + spl[0] + "','" + spl[1] + "','" + boktype + "','" + posno + "','" + placeno + "')";
                                    int inss = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "update bookdetails set lib_code ='" + libcode + "' , rack_flag = 1 where acc_no = '" + var_accessno + "' and lib_code ='" + libcode + "'";
                                    int upp = d2.update_method_wo_parameter(Sql, "Text");
                                }
                                Sql = "update bookdetails set RackLoc ='" + Convert.ToString(ddlreason.SelectedValue) + "' where acc_no ='" + var_accessno + "' and lib_code ='" + libcode + "' ";
                                int upp1 = d2.update_method_wo_parameter(Sql, "Text");

                                Sql = "update rack_master set no_of_copies = no_of_copies +1 where lib_code  = '" + libcode + "' and rack_no = '" + rackno + "'";
                                int upp2 = d2.update_method_wo_parameter(Sql, "Text");


                                Sql = "update rackrow_master set no_of_copies  = no_of_copies +1 where rack_no = '" + rackno + "' and  row_no = '" + rowno + "' and lib_code = '" + libcode + "'";

                                int upp3 = d2.update_method_wo_parameter(Sql, "Text");

                                Sql = "update RowPos_Master set no_of_copies  = no_of_copies +1 where rack_no = '" + rackno + "' and  row_no = '" + rowno + "' and Pos_No ='" + posno + "' and lib_code = '" + libcode + "'";
                                inss10 = d2.update_method_wo_parameter(Sql, "Text");

                            }
                            else
                            {
                                Sql = "select rack_no,row_no,pos_no,Pos_Place,lib_code from rack_allocation where acc_no ='" + var_accessno + "' AND book_type='" + boktype + "'";

                                iss = d2.select_method_wo_parameter(Sql, "text");
                                if (Convert.ToString(ddlposition.SelectedItem) == "")
                                {
                                    Sql = "delete from rack_allocation where lib_code ='" + libcode + "' and rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["row_no"]) + "' and acc_no = '" + var_accessno + "'";
                                    int inss = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "insert into rack_allocation(lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type) values('" + libcode + "','" + rackno + "','" + rowno + "','" + var_accessno + "','" + spl[0] + "','" + spl[1] + "','" + boktype + "')";
                                    int inss2 = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "update bookdetails set lib_code ='" + libcode + "' , rack_flag = 1 where acc_no = '" + var_accessno + "' and lib_code='" + libcode + "'";
                                    int inss3 = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "update rackrow_master set no_of_copies  = no_of_copies -1  where rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + libcode + "'";

                                    int inss4 = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "update rackrow_master set no_of_copies  = no_of_copies +1  where rack_no = '" + rackno + "' and row_no = '" + rowno + "' and lib_code = '" + libcode + "'";
                                    int inss5 = d2.update_method_wo_parameter(Sql, "Text");

                                    Sql = "update rack_master set no_of_copies = no_of_copies -1 where lib_code = '" + libcode + "' and rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "'";
                                    int inss6 = d2.update_method_wo_parameter(Sql, "Text");


                                    Sql = "update rack_master set no_of_copies = no_of_copies +1 where lib_code = '" + libcode + "' and rack_no = '" + rackno + "'";
                                    int inss7 = d2.update_method_wo_parameter(Sql, "Text");



                                }
                                else
                                {
                                    Sql = "delete from rack_allocation where lib_code ='" + libcode + "' and rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["row_no"]) + "' and pos_no ='" + Convert.ToString(iss.Tables[0].Rows[0]["pos_no"]) + "' and Pos_Place ='" + Convert.ToString(iss.Tables[0].Rows[0]["Pos_Place"]) + "' and acc_no = '" + var_accessno + "'";

                                    int inss = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "insert into rack_allocation(lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type,pos_no,Pos_Place) values('" + libcode + "','" + rackno + "','" + rowno + "','" + var_accessno + "','" + spl[0] + "','" + spl[1] + "','" + boktype + "','" + posno + "','" + placeno + "')";

                                    int inss2 = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "update bookdetails set lib_code ='" + libcode + "' , rack_flag = 1 where acc_no = '" + var_accessno + "' and lib_code='" + libcode + "'";
                                    int inss3 = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "update rackrow_master set no_of_copies  = no_of_copies -1  where rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + libcode + "'";


                                    int inss4 = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "update rackrow_master set no_of_copies  = no_of_copies +1  where rack_no = '" + rackno + "' and row_no = '" + rowno + "' and lib_code = '" + libcode + "'";

                                    int inss5 = d2.update_method_wo_parameter(Sql, "Text");

                                    Sql = "update rack_master set no_of_copies = no_of_copies -1 where lib_code = '" + libcode + "' and rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "'";

                                    int inss6 = d2.update_method_wo_parameter(Sql, "Text");


                                    Sql = "update rack_master set no_of_copies = no_of_copies +1 where lib_code = '" + libcode + "' and rack_no = '" + rackno + "'";

                                    int inss7 = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "update rackrow_master set no_of_copies  = no_of_copies -1  where rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + libcode + "' ";


                                    int inss8 = d2.update_method_wo_parameter(Sql, "Text");
                                    Sql = "update rackrow_master set no_of_copies  = no_of_copies +1  where rack_no = '" + rackno + "' and row_no = '" + rowno + "' and lib_code = '" + libcode + "' ";


                                    int inss9 = d2.update_method_wo_parameter(Sql, "Text");
                                }
                                Sql = "update bookdetails set RackLoc ='" + Convert.ToString(ddlreason.SelectedValue) + "' where acc_no ='" + var_accessno + "' and lib_code ='" + libcode + "' ";




                                inss10 = d2.update_method_wo_parameter(Sql, "Text");

                            }
                            if (inss10 == 1)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Selected Books Are Successfully Moved From The Shelf";
                                Go_Click(sender, e);
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Selected Books Are Not Moved ";

                            }

                        }
                    }
                }


            }
            #endregion

        }
        catch (Exception ex)

        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void btnIssueNo_Click(object sender, EventArgs e)
    {
        try
        {
            DivIssue.Visible = false;
        }
        catch (Exception ex)

        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void btntran_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql = string.Empty;
            DataSet iss = new DataSet();
            bool Bookflag = false;
            string var_accessno = string.Empty;
            string libcode = Convert.ToString(ddl_library.SelectedValue);
            string boktype = string.Empty;
            foreach (GridViewRow row in grdBooks.Rows)
            {
                CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                int RowCnt = Convert.ToInt32(row.RowIndex);
                if (cbsel.Checked == true)
                {
                    var_accessno = Convert.ToString(grdBooks.Rows[RowCnt].Cells[2].Text);
                    Sql = "select rack_no,row_no from rack_allocation where acc_no ='" + var_accessno + "'";

                    iss = d2.select_method_wo_parameter(Sql, "text");
                    if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count != 0)
                    {
                        Sql = "delete from rack_allocation where lib_code ='" + libcode + "' and rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["row_no"]) + "' and acc_no = '" + var_accessno + "'";
                        int inss10 = d2.update_method_wo_parameter(Sql, "Text");


                        Sql = "update rackrow_master set no_of_copies  = no_of_copies -1  where rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "' and row_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["row_no"]) + "' and lib_code = '" + libcode + "'";
                        int ins = d2.update_method_wo_parameter(Sql, "Text");
                        Sql = "update rack_master set no_of_copies = no_of_copies -1 where lib_code = '" + libcode + "' and rack_no = '" + Convert.ToString(iss.Tables[0].Rows[0]["rack_no"]) + "'";

                        int ins1 = d2.update_method_wo_parameter(Sql, "Text");


                        if (boktype == "BOK")
                        {
                            Sql = "update bookdetails set rack_flag = 0 where acc_no = '" + var_accessno + "'";
                        }
                        if (boktype == "PER")
                        {
                            Sql = "update journal_master set rackno='Nil',row_no='Nil' where journal_code ='" + var_accessno + "' and lib_code='" + libcode + "'";
                        }

                    }

                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Selected Books Are Successfully Removed From The Shelf";
                    Bookflag = true;

                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select One Or More Books To Remove From The Shelf";
                    Bookflag = true;
                }
            }

        }
        catch
        {

        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void Btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            Div1.Visible = true;
            Div2.Visible = true;
            txt_infra.Text = "";
            txt_infra.Focus();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void Btnsub_Click(object sender, EventArgs e)
    {
        try
        {
            Div1.Visible = true;
            string txt = Convert.ToString(ddlreason.SelectedItem);
            string sql = string.Empty;
            DataSet iss = new DataSet();
            if (txt != "")
            {
                sql = "Select * from textvaltable where college_code=" + Convert.ToString(ddlCollege.SelectedValue) + "and TextCriteria='LbLoc' and TextVal='" + txt + "'";
                iss = d2.select_method_wo_parameter(sql, "text");
                if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                {
                    sql = "delete from textvaltable where TextVal='" + txt + "' and college_code=" + Convert.ToString(ddlCollege.SelectedValue) + "and TextCriteria='LbLoc'";
                    int up = d2.update_method_wo_parameter(sql, "Text");
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter Reason";
            }
            Location();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {

            string txt = txt_infra.Text;
            string sql = string.Empty;
            DataSet iss = new DataSet();
            if (txt != "")
            {
                sql = "Select * from textvaltable where college_code=" + Convert.ToString(ddlCollege.SelectedValue) + "and TextCriteria='LbLoc' and TextVal='" + txt + "'";
                iss = d2.select_method_wo_parameter(sql, "text");
                if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count <= 0)
                {
                    sql = "update textvaltable set TextVal='" + txt + "' where college_code=" + Convert.ToString(ddlCollege.SelectedValue) + "and TextCriteria='LbLoc'";
                    int up = d2.update_method_wo_parameter(sql, "Text");
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter Reason";
            }
            Location();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            Div1.Visible = false;

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        try
        {
            if (Txtfromacc.Text == "" || Txttoacc.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Both From And To Range.')", true);
                return;
            }

            if (Convert.ToInt32(Txtfromacc.Text) > Convert.ToInt32(Txttoacc.Text))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('To Range Should Be Greater Than Or Equal To From Range.')", true);
                return;
            }

            foreach (GridViewRow row in grdBooks.Rows)
            {
                Label sno = (Label)row.FindControl("lbl_sno");
                string sl_no = sno.Text;
                if (!string.IsNullOrEmpty(sl_no))
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                    if (Convert.ToInt32(sl_no) >= Convert.ToInt32(Txtfromacc.Text) && Convert.ToInt32(sl_no) <= Convert.ToInt32(Txttoacc.Text))
                    {
                        cbsel.Checked = true;

                    }
                    else
                    {
                        cbsel.Checked = false;
                    }
                }
            }
            Txtfromacc.Text = "";
            Txttoacc.Text = "";

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void grdTranBooks_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdTranBooks.PageIndex = e.NewPageIndex;
        Btngo1_Click(sender, e);
    }

    protected void Btngo1_Click(object sender, EventArgs e)
    {
        try
        {
            string boktype = string.Empty;
            string Sql = string.Empty;
            if (ddlsearchbook.SelectedItem.Text == "Books")
            {
                if (cbref.Checked == true)
                    boktype = "REF";
                else
                    boktype = "BOK";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Project Books")
            {
                boktype = "PRO";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Non Book Materials")
            {
                boktype = "NBM";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Back Volume")
            {
                boktype = "BVO";
            }
            if (Convert.ToString(ddlsearchbook.SelectedItem) == "Periodical")
            {
                boktype = "PER";
            }
            if (Convert.ToString(ddlmoveto.SelectedItem) == "" || rdbLibrary.Checked == true)
            {
                if (boktype == "BOK")
                {
                    if (cbref.Checked == true)
                    {
                        Sql = "Select bookdetails.acc_no,bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails,rack_allocation where rack_allocation.lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.book_type='REF' and ref='yes'";
                    }
                    else
                        Sql = "Select bookdetails.acc_no,bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails where  bookdetails.lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and ref='no'";
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }

                if (boktype == "PRO")
                {
                    Sql = "select probook_accno,title,roll_no,name,guide_name  from project_book where lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "'";

                }
                if (boktype == "NBM")
                {
                    Sql = "select nonbookmat_no,title,author,publisher,volume  from nonbookmat  where lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "'";
                }
                if (boktype == "BVO")
                {
                    Sql = "select access_code ,title,publisher,remarks,volumeno from back_volume  where lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "'";

                }
                if (boktype == "PER")
                {
                    Sql = "select journal_code,journal_name,journal_master.access_date from journal_master where lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "'";
                }
                if (boktype == "REF")
                {
                    if (cbref.Checked == true)
                    {
                        Sql = "Select distinct (bookdetails.acc_no),bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails,rack_allocation where rack_allocation.lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.book_type='REF' and ref='yes'";
                    }
                    else
                        Sql = "Select distinct (bookdetails.acc_no),bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails where  bookdetails.lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and ref='no'";
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }
            }

            else if (Convert.ToString(ddlmoveto.SelectedItem) == "" || rdbtrans.Checked == true)
            {
                if (boktype == "BOK")
                {
                    if (cbref.Checked == true)
                    {
                        Sql = "Select distinct(bookdetails.acc_no),bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails,rack_allocation where rack_allocation.lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.book_type='REF' and ref='yes'";

                    }
                    else
                        Sql = "Select distinct(bookdetails.acc_no),bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails where  bookdetails.lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and ref='no'";
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";

                }
                if (boktype == "PRO")
                {
                    Sql = "select probook_accno,title,roll_no,name,guide_name  from project_book where lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "'";
                }
                if (boktype == "NBM")
                {
                    Sql = "select nonbookmat_no,title,author,publisher,volume  from nonbookmat  where lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "'";
                }
                if (boktype == "BVO")
                {
                    Sql = "select access_code ,title,publisher,remarks,volumeno from back_volume  where lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "'";
                }
                if (boktype == "PER")
                {
                    Sql = "select journal_code,journal_name,journal_master.access_date from journal_master where lib_code='" + Convert.ToString(ddlmoveto.SelectedValue) + "'";
                }
                // if (boktype == "REF")
                //{
                //       if (cbref.Checked == true)
                //    {
                //       Sql = "Select distinct(bookdetails.acc_no),bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails,rack_allocation where rack_allocation.lib_code='" & GetLibraryCode(cbo_racknoorlibtype.Text) & "' and rack_allocation.book_type='REF' and ref='yes'"
                //;

                //    }
                //    else
                //        Sql = "Select distinct(bookdetails.acc_no),bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails where  bookdetails.lib_code='" & GetLibraryCode(cbo_racknoorlibtype.Text) & "' and ref='no'"
                //          }
            }
            else if (rdbrack.Checked == true || rdbrackto.Checked == true)
            {
                if (boktype == "BOK")
                {
                    if (cbref.Checked == true)
                    {
                        Sql = "Select bookdetails.acc_no,bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails,rack_allocation where bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (rack_allocation.book_type='REF' or rack_allocation.book_type='BOK') and ref='yes'  AND bookdetails.lib_code = '" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    }
                    else
                        Sql = "Select bookdetails.acc_no,bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails,rack_allocation where bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_allocation.book_type='BOK' and ref='no'  AND bookdetails.lib_code = '" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }
                if (boktype == "REF")
                {
                    if (cbref.Checked == true)
                    {
                        Sql = "Select bookdetails.acc_no,bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails,rack_allocation where bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and (rack_allocation.book_type='REF' or rack_allocation.book_type='BOK') and ref='yes'  AND bookdetails.lib_code = '" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    }
                    else
                        Sql = "Select bookdetails.acc_no,bookdetails.title,bookdetails.author,bookdetails.publisher,bookdetails.edition,call_no from bookdetails,rack_allocation where bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_allocation.book_type='BOK' and ref='no'  AND bookdetails.lib_code = '" + Convert.ToString(ddl_library.SelectedValue) + "'";
                    Sql = Sql + "order by LEN(bookdetails.Acc_No),bookdetails.Acc_No";
                }
                if (boktype == "PRO")
                {
                    Sql = "select probook_accno,title,roll_no,name,guide_name from project_book,rack_allocation where probook_accno=rack_allocation.acc_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "' and rack_allocation.book_type='PRO'";
                }
                if (boktype == "NBM")
                {
                    Sql = "select nonbookmat_no,title,author,publisher,volume from nonbookmat,rack_allocation where nonbookmat_no=rack_allocation.acc_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'and rack_allocation.book_type='NBM'";
                }
                if (boktype == "BVO")
                {
                    Sql = "select access_code ,title,publisher,remarks,volumeno from back_volume,rack_allocation where access_code=rack_allocation.acc_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'and rack_allocation.book_type='BVO'";
                }
                if (boktype == "PER")
                {
                    Sql = "select journal_code,journal_name,journal_master.access_date from journal_master,rack_allocation where journal_code=rack_allocation.acc_no and rack_allocation.rack_no='" + Convert.ToString(ddlmoveto.SelectedValue) + "' and rack_allocation.row_no='" + Convert.ToString(ddlshelf.SelectedValue) + "' and rack_allocation.lib_code='" + Convert.ToString(ddl_library.SelectedValue) + "'and rack_allocation.book_type='PER'";
                }
            }
            DataSet bookallo = new DataSet();
            int sno = 0;
            bookallo = d2.select_method_wo_parameter(Sql, "Text");
            int row1 = 0;
            if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
            {

                if (bokaloca.Rows.Count == 0)
                {
                    for (row1 = 0; row1 < bookallo.Tables[0].Rows.Count; row1++)
                    {
                        sno++;
                        drallo1 = bokaloca1.NewRow();
                        drallo1["Title"] = Convert.ToString(bookallo.Tables[0].Rows[row1]["Title"]);
                        drallo1["Call No"] = Convert.ToString(bookallo.Tables[0].Rows[row1]["call_no"]);
                        drallo1["Acc No"] = Convert.ToString(bookallo.Tables[0].Rows[row1]["Acc_no"]);
                        bokaloca1.Rows.Add(drallo1);
                    }
                }

                grdTranBooks.DataSource = bokaloca1;
                grdTranBooks.DataBind();
                grdTranBooks.Visible = true;
                if (grdTranBooks.Rows.Count > 0)
                {
                    CheckBox selectall = grdTranBooks.Rows[0].FindControl("selectall") as CheckBox;
                    selectall.Visible = true;
                    CheckBox select = grdTranBooks.Rows[0].FindControl("select") as CheckBox;
                    select.Visible = false;
                }
                RowHead1(grdTranBooks);
            }
            else
            {
                grdTranBooks.DataSource = bokaloca1;
                grdTranBooks.DataBind();
                grdTranBooks.Visible = true;
                if (grdTranBooks.Rows.Count > 0)
                {
                    CheckBox selectall = grdTranBooks.Rows[0].FindControl("selectall") as CheckBox;
                    selectall.Visible = true;
                    CheckBox select = grdTranBooks.Rows[0].FindControl("select") as CheckBox;
                    select.Visible = false;
                }
                RowHead1(grdTranBooks);
            }
            Btngo1_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation");
        }
    }

    protected void RowHead1(GridView grdTranBooks)
    {
        for (int head = 0; head < 1; head++)
        {
            grdTranBooks.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grdTranBooks.Rows[head].Font.Bold = true;
            grdTranBooks.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    #region Status_Popup

    protected void ddlcollege_sts_SelectedIndexChanged(object sender, EventArgs e)
    {
        RackFpSpread.Visible = false;
        rptprint.Visible = false;
    }

    protected void ddllibrary_sts_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RackFpSpread.Visible = false;
            rptprint.Visible = false;
            LoadRack();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void ddlrack_sts_SelectedIndexChanged(object sender, EventArgs e)
    {
        RackFpSpread.Visible = false;
        rptprint.Visible = false;

    }

    protected void btn_sts_Rack_Go_Click(object sender, EventArgs e)
    {
        try
        {
            ds = getrackstatus();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                rackloadspread(ds);
            }
            else
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";

            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    #region Fspread
    private DataSet getrackstatus()
    {

        DataSet dsload1 = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string stalibcode = "";
            string statrack = "";


            if (ddlstat_college.Items.Count > 0)
                collegecode = Convert.ToString(ddlstat_college.SelectedValue);
            if (ddllibrary_sts.Items.Count > 0)
                stalibcode = Convert.ToString(ddllibrary_sts.SelectedValue);
            if (ddlsts_rackno.Items.Count > 0)
                statrack = Convert.ToString(ddlsts_rackno.SelectedValue);
            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(stalibcode))
            {
                if (statrack == "" || statrack == "All")
                    selQ = "select distinct rack_no,lib_code  from rack_master where rack_master.lib_code='" + stalibcode + "'";
                else
                    selQ = "select distinct rack_no,lib_code  from rack_master where rack_No='" + statrack + "' and rack_master.lib_code='" + stalibcode + "'";

            }
            dsload1.Clear();
            dsload1 = d2.select_method_wo_parameter(selQ, "Text");
            #endregion
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }

        return dsload1;
    }

    public void rackloadspread(DataSet dsrack)
    {
        try
        {
            DataSet dsrackrow = new DataSet();
            DataSet dscat = new DataSet();
            string categ = "";
            string categ1 = "";
            string acopies = "";
            string maxcap = "";
            RackFpSpread.SaveChanges();
            if (dsrack.Tables.Count > 0 && dsrack.Tables[0].Rows.Count > 0)
            {
                RackFpSpread.Sheets[0].RowCount = 0;
                RackFpSpread.CommandBar.Visible = false;
                RackFpSpread.Sheets[0].AutoPostBack = true;
                RackFpSpread.Sheets[0].ColumnHeader.RowCount = 1;
                RackFpSpread.Sheets[0].ColumnHeader.Columns.Count = 0;
                RackFpSpread.Sheets[0].RowHeader.Visible = false;
                //FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                //darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //darkstyle.ForeColor = Color.White;

                for (int col1 = 0; col1 < dsrack.Tables[0].Rows.Count; col1++)
                {
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, cncessionColCnt].Text = "Allot";
                    RackFpSpread.Sheets[0].ColumnHeader.Columns.Count++;
                    string racknum = Convert.ToString(dsrack.Tables[0].Rows[col1]["rack_no"]);
                    RackFpSpread.Sheets[0].ColumnHeader.Cells[0, col1].Text = racknum;
                    RackFpSpread.Sheets[0].ColumnHeader.Cells[0, col1].HorizontalAlign = HorizontalAlign.Center;
                    RackFpSpread.Sheets[0].ColumnHeader.Cells[0, col1].BackColor = ColorTranslator.FromHtml("brown");
                }
                int c = 0;
                for (int row = 0; row < dsrack.Tables[0].Rows.Count; row++)
                {
                    string racknum = Convert.ToString(dsrack.Tables[0].Rows[row]["rack_no"]);
                    string licode = Convert.ToString(dsrack.Tables[0].Rows[row]["lib_code"]);
                    int col = row;

                    if (racknum != "")
                    {
                        string getrackqry = "SELECT distinct row_no,lib_code,rack_no from rackrow_master where rack_no='" + racknum + "' and lib_code='" + licode + "' order by row_no ";
                        dsrackrow.Clear();
                        dsrackrow = d2.select_method_wo_parameter(getrackqry, "Text");
                        if (dsrackrow.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < dsrackrow.Tables[0].Rows.Count; i++)
                            {
                                RackFpSpread.Sheets[0].RowCount++;
                                string rano = Convert.ToString(dsrackrow.Tables[0].Rows[i]["rack_no"]);
                                string rono = Convert.ToString(dsrackrow.Tables[0].Rows[i]["row_no"]);
                                string lcode = Convert.ToString(dsrackrow.Tables[0].Rows[i]["lib_code"]);
                                acopies = d2.GetFunction("select no_of_copies from rackrow_master where lib_code='" + lcode + "' and rack_no='" + rano + "' and row_no ='" + rono + "'");
                                maxcap = d2.GetFunction("select max_capacity from rackrow_master where lib_code='" + lcode + "' and rack_no='" + rano + "' and row_no ='" + rono + "'");
                                int acopies1 = Convert.ToInt32(acopies);
                                int maxcap1 = Convert.ToInt32(maxcap);
                                if (acopies1 == maxcap1)
                                {
                                    RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].BackColor = ColorTranslator.FromHtml("Purple");
                                    // fpSpread3.BackColor = &HC0C0FF

                                }
                                else if (acopies1 > 0)
                                {
                                    RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].BackColor = ColorTranslator.FromHtml("Green");
                                }
                                else
                                {
                                    RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].BackColor = ColorTranslator.FromHtml("Yellow");
                                }
                                string nooftitle1 = d2.GetFunction("select count(acc_no) from rack_allocation where lib_code='" + lcode + "' and rack_no='" + rano + "' and row_no ='" + rono + "'");
                                string cat = "select cat from libcat where lib_code='" + lcode + "' and rno='" + rano + "' and sno ='" + rono + "'";
                                dscat.Clear();
                                dscat = d2.select_method_wo_parameter(cat, "Text");
                                if (dscat.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < dscat.Tables[0].Rows.Count; j++)
                                    {
                                        string cat1 = Convert.ToString(dscat.Tables[0].Rows[j]["cat"]);
                                        if (cat1 != "")
                                        {
                                            if (categ == "")
                                                categ = cat1;
                                            else
                                                categ = categ + "," + cat1;
                                        }
                                    }

                                }
                                string status = "SH-" + racknum + "AVAIL-" + acopies + "TOT-" + maxcap + "IM-" + categ + "";

                                RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].Text = status;
                                RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                //RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount, i].BackColor = ColorTranslator.FromHtml("brown"); 
                            }

                        }
                    }
                    c = c + 1;

                }
                RackFpSpread.Sheets[0].PageSize = RackFpSpread.Sheets[0].RowCount;
                RackFpSpread.SaveChanges();
                RackFpSpread.Visible = true;
                rptprint.Visible = true;
            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }

    }



    #endregion

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Book Allocation";
            string pagename = "Book Allocation.aspx";
            Printcontrol.loadspreaddetails(RackFpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(RackFpSpread, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }
    }
    #endregion

    #endregion

    #region LoadRack
    public void LoadRack()
    {
        try
        {
            ddlsts_rackno.Items.Clear();
            string libcode1 = "";
            if (ddllibrary_sts.Items.Count > 0)
                libcode1 = Convert.ToString(ddllibrary_sts.SelectedValue);
            string qrybook = "SELECT DISTINCT  ISNULL(rack_no,'') rack_no FROM rack_master WHERE  Lib_Code ='" + libcode1 + "' order by rack_no ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrybook, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsts_rackno.DataSource = ds;
                ddlsts_rackno.DataTextField = "rack_no";
                ddlsts_rackno.DataValueField = "rack_no";
                ddlsts_rackno.DataBind();
                ddlsts_rackno.Items.Insert(0, "All");
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "BooKAllocation"); }

    }
    #endregion

    #region Link_Status

    protected void link_status_Click(object sender, EventArgs e)
    {
        DivStatus.Visible = true;
        Bindcollege();
        getLibPrivil();
        LoadRack();
        RackFpSpread.Visible = false;
    }

    #endregion

    protected void btn_Question_Bank_popup_Click(object sender, EventArgs e)
    {
        DivStatus.Visible = false;

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