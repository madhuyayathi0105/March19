using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;

public partial class LibraryMod_Rack_Status_Monitor : System.Web.UI.Page
{

    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Dictionary<string, string> dicStaffList = new Dictionary<string, string>();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();
    Dictionary<int, string> dicGrindBind = new Dictionary<int, string>();
    public SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    public SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collcode = string.Empty;
    string libcode = string.Empty;
    Boolean Cellclick = false;
    Dictionary<int, string> dicRowColor = new Dictionary<int, string>();
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
                LoadRack();
                grdRackStatus.Visible = false;
                //rptprint.Visible = false;
                rack_st_field.Visible = false;
                rack_st_des.Visible = false;
                fpfieldset.Visible = false;
            }
        }
        catch
        {

        }
    }

    #region College

    public void Bindcollege()
    {
        try
        {
            ddlstat_college.Items.Clear();
            dtCommon.Clear();
            ddlstat_college.Enabled = false;
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
                ddlstat_college.DataSource = dtCommon;
                ddlstat_college.DataTextField = "collname";
                ddlstat_college.DataValueField = "college_code";
                ddlstat_college.DataBind();
                ddlstat_college.SelectedIndex = 0;
                ddlstat_college.Enabled = true;

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    protected void ddlcollege_sts_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdRackStatus.Visible = false;
        //  rptprint.Visible = false;
        rack_st_field.Visible = false;
        rack_st_des.Visible = false;
        fpfieldset.Visible = false;
        getLibPrivil();
    }

    #endregion

    #region Library

    public void Library(string libcode)
    {
        try
        {
            ddllibrary_sts.Items.Clear();
            ds.Clear();
            string College = ddlstat_college.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib_name, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary_sts.DataSource = ds;
                    ddllibrary_sts.DataTextField = "lib_name";
                    ddllibrary_sts.DataValueField = "lib_code";
                    ddllibrary_sts.DataBind();
                    // ddllibrary.Items.Insert(0, "All");
                }
            }
        }
        catch
        {

        }


    }

    protected void ddllibrary_sts_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdRackStatus.Visible = false;
            //  rptprint.Visible = false;
            rack_st_field.Visible = false;
            rack_st_des.Visible = false;
            fpfieldset.Visible = false;
            LoadRack();
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
            string coll_Code = Convert.ToString(ddlstat_college.SelectedValue);
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

    #region LoadRack

    public void LoadRack()
    {
        try
        {
            ddlsts_rackno.Items.Clear();
            string libcode1 = "";
            if (ddllibrary_sts.Items.Count > 0)
                libcode1 = Convert.ToString(ddllibrary_sts.SelectedValue);
            string qrybook = "select distinct(RM.lib_code),LB.lib_name,RM.rack_no,RM.max_capacity,RM.no_of_copies ,RM.noof_rows,CAST(RIGHT(rm.rack_no, LEN(rm.rack_no) - PATINDEX('%[0-9]%', rm.rack_no)+1) AS INT), LEFT(rm.rack_no, PATINDEX('%[0-9]%', rm.rack_no)-1) FROM rack_master RM,library LB where RM.lib_code = LB.lib_code  and RM.lib_code='" + libcode1 + "' ORDER BY LEFT(rm.rack_no, PATINDEX('%[0-9]%', rm.rack_no)-1)  ,  CAST(RIGHT(rm.rack_no, LEN(rm.rack_no) - PATINDEX('%[0-9]%', rm.rack_no)+1) AS INT)";
            // "SELECT DISTINCT  ISNULL(rack_no,'') rack_no FROM rack_master WHERE  Lib_Code ='" + libcode1 + "' order by rack_no ";
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
        catch
        {

        }

    }

    protected void ddlrack_sts_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdRackStatus.Visible = false;
        //  rptprint.Visible = false;
        rack_st_field.Visible = false;
        fpfieldset.Visible = false;
        rack_st_des.Visible = false;

    }

    #endregion

    #region Go

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
        catch
        {

        }
    }

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
        { }

        return dsload1;
    }

    public void rackloadspread(DataSet dsrack)
    {
        try
        {
            DataRow drstat;
            DataTable drstatrack = new DataTable();

            DataTable dtFinal = new DataTable();
            DataSet dsrackrow = new DataSet();
            DataSet dscat = new DataSet();
            string categ = "";
            string categ1 = "";
            string acopies = "";
            string maxcap = "";
            Hashtable htRack = new Hashtable();
            int rowCount = 0;
            int finalRowCount = 0;
            if (dsrack.Tables.Count > 0 && dsrack.Tables[0].Rows.Count > 0)
            {
                for (int col1 = 0; col1 < dsrack.Tables[0].Rows.Count; col1++)
                {
                    drstat = drstatrack.NewRow();
                    string racknum = Convert.ToString(dsrack.Tables[0].Rows[col1]["rack_no"]);
                    drstatrack.Columns.Add(racknum);
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
                            rowCount = Convert.ToInt32(dsrackrow.Tables[0].Rows.Count);
                            if (rowCount > finalRowCount)
                            {
                                finalRowCount = rowCount;
                            }
                            for (int i = 0; i < dsrackrow.Tables[0].Rows.Count; i++)
                            {
                                string rano = Convert.ToString(dsrackrow.Tables[0].Rows[i]["rack_no"]);
                                string rono = Convert.ToString(dsrackrow.Tables[0].Rows[i]["row_no"]);
                                string lcode = Convert.ToString(dsrackrow.Tables[0].Rows[i]["lib_code"]);
                                acopies = d2.GetFunction("select no_of_copies from rackrow_master where lib_code='" + lcode + "' and rack_no='" + racknum + "' and row_no ='" + rono + "'");
                                maxcap = d2.GetFunction("select max_capacity from rackrow_master where lib_code='" + lcode + "' and rack_no='" + racknum + "' and row_no ='" + rono + "'");
                                int maxcap1 = Convert.ToInt32(maxcap);
                                
                                string nooftitle1 = d2.GetFunction("select count(acc_no) from rack_allocation where lib_code='" + lcode + "' and rack_no='" + racknum + "' and row_no ='" + rono + "'");
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
                                int acopies1 = Convert.ToInt32(nooftitle1);
                                int Available = maxcap1 - acopies1;
                                string status = "SH - " + rono + "  - TOT - " + maxcap + " - Filled - " + nooftitle1 + " - AVAIL - " + Available + " - IM -" + categ + "";
                                c = c + 1;
                                drstat = drstatrack.NewRow();
                                drstat[racknum] = drstat[racknum] + status;
                                drstatrack.Rows.Add(drstat);
                                if (acopies1 == 0)//No Shelf Entry
                                {
                                    dicRowColor.Add(drstatrack.Rows.Count - 1, "No Shelf Entry");
                                }
                                if (acopies1 > 0)//Partially Filled
                                {
                                    dicRowColor.Add(drstatrack.Rows.Count - 1, "Partially Filled");
                                }
                                if (Available == 0)//Fully Filled
                                {
                                    dicRowColor.Add(drstatrack.Rows.Count - 1, "Fully Filled");
                                }
                            }
                        }
                    }
                }
                grdRackStatus.DataSource = drstatrack;
                grdRackStatus.DataBind();
                grdRackStatus.Visible = true;
                fpfieldset.Visible = true;
                rack_st_field.Visible = true;
                rack_st_des.Visible = true;
                rptprint.Visible = true;
                foreach (KeyValuePair<int, string> dr in dicRowColor)
                {
                    int g = dr.Key;
                    string DicValue = dr.Value;
                    if (DicValue == "No Shelf Entry")
                    {
                        grdRackStatus.Rows[g].BackColor = Color.Yellow;
                    }
                    if (DicValue == "Partially Filled")
                    {
                        grdRackStatus.Rows[g].BackColor = Color.PaleGreen;
                    }
                    if (DicValue == "Fully Filled")
                    {
                        grdRackStatus.Rows[g].BackColor = Color.Red;
                    }
                }
            }
        }
        catch
        {

        }
    }

    #endregion

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Rack Status Report";
            string pagename = "Inward_Entry.aspx";
            string ss = "";
            Printcontrolhed2.loadspreaddetails(grdRackStatus, pagename, degreedetails,0, ss,"1");
            Printcontrolhed2.Visible = true;
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
                d2.printexcelreportgrid(grdRackStatus, reportname);
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

    protected void grdRackStatus_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    public void grdRackStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable rack = new DataTable();
            DataRow drrow;
            string libname = "";
            DataSet dsgetupdatebook = new DataSet();

            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

            if (ddllibrary_sts.Items.Count > 0)
            {
                libname = Convert.ToString(ddllibrary_sts.SelectedItem.Text);
                libcode = Convert.ToString(ddllibrary_sts.SelectedValue);
            }
            if (Convert.ToString(rowIndex) != "" && Convert.ToString(selectedCellIndex) != "-1")
            {
                string getra = Convert.ToString(grdRackStatus.Rows[rowIndex].Cells[selectedCellIndex].Text);
                string getrackNo = Convert.ToString(grdRackStatus.HeaderRow.Cells[selectedCellIndex].Text);
                if (getra != "")
                {
                    string[] rash = getra.Split('-');
                    string RowValue = rash[1].Trim();
                    string getsql = " select bookdetails.acc_no,title ,author,call_no from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  bookdetails.lib_code='" + libcode + "'  and rack_no='" + getrackNo + "' and row_no='" + RowValue + "' and (rack_allocation.book_type='BOK' or  rack_allocation.book_type='REF') order by acc_no";

                    dsgetupdatebook.Clear();
                    dsgetupdatebook = d2.select_method_wo_parameter(getsql, "Text");

                    rack.Columns.Add("Access No");
                    rack.Columns.Add("Title");
                    rack.Columns.Add("Author");
                    rack.Columns.Add("Call No");

                    if (dsgetupdatebook.Tables.Count > 0 && dsgetupdatebook.Tables[0].Rows.Count > 0)
                    {
                        FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
                        int sno = 0;
                        for (int row = 0; row < dsgetupdatebook.Tables[0].Rows.Count; row++)
                        {
                            sno++;
                            drrow = rack.NewRow();
                            drrow["Access No"] = Convert.ToString(dsgetupdatebook.Tables[0].Rows[row]["acc_no"]).Trim();
                            drrow["Title"] = Convert.ToString(dsgetupdatebook.Tables[0].Rows[row]["title"]).Trim();
                            drrow["Author"] = Convert.ToString(dsgetupdatebook.Tables[0].Rows[row]["author"]).Trim();
                            drrow["Call No"] = Convert.ToString(dsgetupdatebook.Tables[0].Rows[row]["call_no"]).Trim();
                            rack.Rows.Add(drrow);
                        }
                    }
                    div1.Visible = true;
                    grdCellClick.DataSource = rack;
                    grdCellClick.DataBind();
                    grdCellClick.Visible = true;
                    Divfspreadstatus.Visible = true;
                    Buttonexit.Visible = true;
                }
            }
            else
            {
                Divfspreadstatus.Visible = false;

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

    protected void btn_popclose5_Click(object sender, EventArgs e)
    {
        Divfspreadstatus.Visible = false;
    }

    protected void Buttonexit_Click(object sender, EventArgs e)
    {
        Divfspreadstatus.Visible = false;
    }
}