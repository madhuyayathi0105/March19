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

public partial class LibraryMod_TransferReports : System.Web.UI.Page
{
    #region Field Declaration
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 dacces2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string boktype;
    DataTable dttransfer = new DataTable();
    DataRow dr;
    string libcodecollection = "";
    string LibCollection = "";
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
                Bindbooktype();
                ddlCollege_SelectedIndexChanged(sender, e);
                getLibPrivil();
                string frmlib = "select lib_name,lib_code from library  order by lib_code";
                ds = dacces2.select_method_wo_parameter(frmlib, "Text");
                ddltransferfrom.Items.Clear();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddltransferfrom.DataSource = ds;
                    ddltransferfrom.DataTextField = "lib_name";
                    ddltransferfrom.DataValueField = "lib_code";
                    ddltransferfrom.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    #region college

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
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    #endregion

    #region Booktype

    public void Bindbooktype()
    {
        try
        {
            ddlbooktype.Items.Add("Books");
            ddlbooktype.Items.Add("Project Books");
            ddlbooktype.Items.Add("Non Book Materials");
            ddlbooktype.Items.Add("Back Volume");
            ddlbooktype.Items.Add("Periodical");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    #endregion

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    protected void ddlbooktype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            switch (ddlbooktype.SelectedIndex)
            {
                case 0:
                    boktype = "BOK";
                    break;
                case 1:
                    boktype = "PRO";
                    break;
                case 2:
                    boktype = "NBM";
                    break;
                case 3:
                    boktype = "BVO";
                    break;
                case 4:
                    boktype = "PER";
                    break;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }


    }

    public void getLibPrivil()
    {
        try
        {
            Hashtable hsLibcode = new Hashtable();

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
            to_library(LibCollection);
            
           // frm_library(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void frm_library(string libcode)
    {
        try
        {
           
        }
        catch (Exception)
        {
        }
    }
    public void to_library(string libcode)
    {
        try
        {
            string College = ddlCollege.SelectedValue;
            string libcodefrom = Convert.ToString(ddltransferfrom.SelectedValue);
            if (rbltransfertype.SelectedValue == "Transfer Library")
            {
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcodecollection + " AND college_code=" + College + " and lib_code<>'" + libcodefrom + "' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(lib_name, "text");
                ddltransferto.Items.Clear();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddltransferto.DataSource = ds;
                    ddltransferto.DataTextField = "lib_name";
                    ddltransferto.DataValueField = "lib_code";
                    ddltransferto.DataBind();
                }
            }
            else if (rbltransfertype.SelectedValue == "Transfer Dept" || rbltransfertype.SelectedValue == "Dept Return")
            {
                string selectQuery = "SELECT DISTINCT To_Lib_Code FROM Book_Transfer WHERE Transfer_Type =2 AND From_Lib_Code ='" + ddltransferfrom.SelectedValue + "' ORDER BY To_Lib_Code";
                ds = dacces2.select_method_wo_parameter(selectQuery, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddltransferto.DataSource = ds;
                    ddltransferto.DataTextField = "To_Lib_Code";
                    ddltransferto.DataValueField = "To_Lib_Code";
                    ddltransferto.DataBind();
                    ddltransferto.Items.Insert(0, "All");
                }
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    protected void rbltransfertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();

            //Fpspread6.Visible = false;
            //rptprint1.Visible = false;

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    protected void ddltransferfrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    protected void ddltransferto_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    protected void cbfrom_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbfrom.Checked)
            {
                txt_fromdate1.Enabled = true;
                txt_todate1.Enabled = true;
            }
            else
            {
                txt_fromdate1.Enabled = false;
                txt_todate1.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    #endregion

    #region go

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdate1 = string.Empty;
            string todate1 = string.Empty;
            string Sql = string.Empty;
            string from_lib = "", to_lib = "";
            if (cbfrom.Checked)
            {
                string fromDate = txt_fromdate1.Text;
                string toDate = txt_todate1.Text;
                string[] fromdate = fromDate.Split('/');
                string[] todate = toDate.Split('/');
                if (fromdate.Length == 3)
                    fromdate1 = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

                if (todate.Length == 3)
                    todate1 = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();

            }
            if (rbltransfertype.SelectedValue == "Transfer Library")
            {
                ds = dacces2.select_method_wo_parameter("select lib_code from library where lib_name='" + ddltransferfrom.SelectedItem + "'", "text");
                if (ds.Tables[0].Rows.Count > 0)
                    from_lib = ds.Tables[0].Rows[0]["lib_code"].ToString();

                to_lib = ddltransferto.SelectedValue;
                if (from_lib == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Transfer From Library";
                    return;
                }
                if (to_lib == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Select The Transfer To Library";
                    return;
                }
                ddlbooktype_SelectedIndexChanged(sender, e);


                switch (boktype)
                {
                    case "BOK":
                        if (cbfrom.Checked == false)
                            Sql = "SELECT distinct(select lib_name from library where lib_code=book_transfer.from_lib_code), (select lib_name from library where lib_code=book_transfer.to_lib_code),book_transfer.acc_no as 'Access Number',title as 'Title',author as 'Author',cast(price as float) as 'Price',book_transfer.transfer_date as 'Transfer Date'from book_transfer,bookdetails where book_transfer.from_lib_code='" + from_lib + "' and book_transfer.to_lib_code='" + to_lib + "' and bookdetails.acc_no =book_transfer.acc_no and ltrim(rtrim(booktype))='bok' and book_status<>'Transfer' order by book_transfer.transfer_date asc ";
                        else if (cbfrom.Checked == true)
                            Sql = "SELECT distinct(select lib_name from library where lib_code=book_transfer.from_lib_code) , (select lib_name from library where lib_code=book_transfer.to_lib_code) ,book_transfer.acc_no as 'Access Number',title as 'Title',author as 'Author',cast(price as float) as 'Price',transfer_date as 'Transfer Date' from book_transfer,bookdetails where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "'  and bookdetails.acc_no=book_transfer.acc_no  and transfer_date between '" + fromdate1 + "' and '" + todate1 + "' and ltrim(rtrim(booktype))='bok' and book_status<>'Transfer' order by transfer_date asc";

                        ds1 = d2.select_method_wo_parameter(Sql, "text");

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            dttransfer.Columns.Add("SNo", typeof(string));
                            dttransfer.Columns.Add("Access Number", typeof(string));
                            dttransfer.Columns.Add("Title", typeof(string));
                            dttransfer.Columns.Add("Author", typeof(string));
                            dttransfer.Columns.Add("Price", typeof(string));
                            dttransfer.Columns.Add("Transfer Date", typeof(string));



                            int sno = 0;
                            for (int r = 0; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                sno++;
                                dr = dttransfer.NewRow();
                                dr["SNo"] = sno.ToString();
                                dr["Access Number"] = ds1.Tables[0].Rows[r]["Access Number"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                                dr["Price"] = ds1.Tables[0].Rows[r]["Price"].ToString();
                                dr["Transfer Date"] = Convert.ToDateTime(ds1.Tables[0].Rows[r]["Transfer Date"]).ToString("MM/dd/yyyy");
                                dttransfer.Rows.Add(dr);

                            }


                            grdManualExit.DataSource = dttransfer;
                            grdManualExit.DataBind();
                            grdManualExit.Visible = true;
                            rptprint1.Visible = true;


                        }

                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Found";
                            grdManualExit.Visible = false;
                            rptprint1.Visible = false;
                        }

                        break;
                    case "NBM":
                        if (cbfrom.Checked == false)
                            Sql = "SELECT distinct (select lib_name from library where lib_code=book_transfer.from_lib_code), (select lib_name from library where lib_code=book_transfer.to_lib_code),book_transfer.acc_no as 'Access Number',title as 'Title',author as 'Author',transfer_date as 'Transfer Date'from book_transfer,nonbookmat where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and ltrim(rtrim(booktype))='nbm' and nonbookmat.nonbookmat_no=book_transfer.acc_no  order by transfer_date asc ";
                        else if (cbfrom.Checked == true)
                            Sql = "SELECT distinct (select lib_name from library where lib_code=book_transfer.from_lib_code), (select lib_name from library where lib_code=book_transfer.to_lib_code),book_transfer.acc_no as 'Access Number',title as 'Title',author as 'Author',transfer_date as 'Transfer Date'  from book_transfer,nonbookmat where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "'  and nonbookmat.nonbookmat_no=book_transfer.acc_no and transfer_date between '" + fromdate1 + "' and '" + todate1 + "' and ltrim(rtrim(booktype))='nbm' order by transfer_date asc";


                        ds1 = d2.select_method_wo_parameter(Sql, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            dttransfer.Columns.Add("SNo", typeof(string));
                            dttransfer.Columns.Add("Access Number", typeof(string));
                            dttransfer.Columns.Add("Title", typeof(string));
                            dttransfer.Columns.Add("Author", typeof(string));
                            dttransfer.Columns.Add("Transfer Date", typeof(string));



                            int sno = 0;
                            for (int r = 0; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                sno++;
                                dr = dttransfer.NewRow();
                                dr["SNo"] = sno.ToString();
                                dr["Access Number"] = ds1.Tables[0].Rows[r]["Access Number"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                                dr["Transfer Date"] = Convert.ToDateTime(ds1.Tables[0].Rows[r]["Transfer Date"]).ToString("MM/dd/yyyy");
                                dttransfer.Rows.Add(dr);

                            }


                            grdManualExit.DataSource = dttransfer;
                            grdManualExit.DataBind();
                            grdManualExit.Visible = true;
                            rptprint1.Visible = true;


                        }

                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Found";
                            grdManualExit.Visible = false;
                            rptprint1.Visible = false;
                        }

                        break;
                    case "BVO":
                        if (cbfrom.Checked == false)
                            Sql = "SELECT distinct (select lib_name from library where lib_code=book_transfer.from_lib_code), (select lib_name from library where lib_code=book_transfer.to_lib_code),book_transfer.acc_no as 'Access Number',title as 'Title',publisher as 'Publisher',transfer_date as 'Transfer Date'from book_transfer,back_volume where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and ltrim(rtrim(booktype))='bvo' and back_volume.access_code =book_transfer.acc_no  order by transfer_date asc ";

                        else if (cbfrom.Checked == true)
                            Sql = "SELECT distinct (select lib_name from library where lib_code=book_transfer.from_lib_code), (select lib_name from library where lib_code=book_transfer.to_lib_code),book_transfer.acc_no as 'Access Number',title as 'Title',publisher as 'Publisher',transfer_date as 'Tansfer Date' from book_transfer,back_volume where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "'  and back_volume.access_code =book_transfer.acc_no  and transfer_date between '" + todate1 + "' and '" + todate1 + "'and ltrim(rtrim(booktype))='bvo' order by transfer_date asc";


                        ds1 = d2.select_method_wo_parameter(Sql, "text");

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            dttransfer.Columns.Add("SNo", typeof(string));
                            dttransfer.Columns.Add("Access Number", typeof(string));
                            dttransfer.Columns.Add("Title", typeof(string));
                            dttransfer.Columns.Add("Publisher", typeof(string));
                            dttransfer.Columns.Add("Transfer Date", typeof(string));



                            int sno = 0;
                            for (int r = 0; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                sno++;
                                dr = dttransfer.NewRow();
                                dr["SNo"] = sno.ToString();
                                dr["Access Number"] = ds1.Tables[0].Rows[r]["Access Number"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Publisher"] = ds1.Tables[0].Rows[r]["Publisher"].ToString();
                                dr["Transfer Date"] = Convert.ToDateTime(ds1.Tables[0].Rows[r]["Transfer Date"]).ToString("MM/dd/yyyy");
                                dttransfer.Rows.Add(dr);

                            }


                            grdManualExit.DataSource = dttransfer;
                            grdManualExit.DataBind();
                            grdManualExit.Visible = true;
                            rptprint1.Visible = true;


                        }

                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Found";
                            grdManualExit.Visible = false;
                            rptprint1.Visible = false;
                        }

                        break;
                    case "PRO":
                        if (cbfrom.Checked == false)
                            Sql = "SELECT distinct (select lib_name from library where lib_code=book_transfer.from_lib_code), (select lib_name from library where lib_code=book_transfer.to_lib_code),book_transfer.acc_no as 'Access number',title as 'Title',roll_no as 'Roll Number',name as 'Name',transfer_date as 'Transfer Date' from book_transfer,project_book where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and project_book.probook_accno =book_transfer.acc_no and ltrim(rtrim(booktype))='pro' order by transfer_date asc ";
                        else if (cbfrom.Checked == true)
                            Sql = "SELECT distinct (select lib_name from library where lib_code=book_transfer.from_lib_code), (select lib_name from library where lib_code=book_transfer.to_lib_code),book_transfer.acc_no as 'Access Number',title as 'Title',roll_no as 'Roll Number',name as 'Name',transfer_date as 'Transfer Date' from book_transfer,project_book  where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "'  and project_book.probook_accno =book_transfer.acc_no and transfer_date between '" + fromdate1 + "' and '" + todate1 + "' and ltrim(rtrim(booktype))='pro' order by transfer_date asc";


                        ds1 = d2.select_method_wo_parameter(Sql, "text");

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            dttransfer.Columns.Add("SNo", typeof(string));
                            dttransfer.Columns.Add("Access Number", typeof(string));
                            dttransfer.Columns.Add("Title", typeof(string));
                            dttransfer.Columns.Add("Roll Number", typeof(string));
                            dttransfer.Columns.Add("Name", typeof(string));
                            dttransfer.Columns.Add("Transfer Date", typeof(string));



                            int sno = 0;
                            for (int r = 0; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                sno++;
                                dr = dttransfer.NewRow();
                                dr["SNo"] = sno.ToString();
                                dr["Access Number"] = ds1.Tables[0].Rows[r]["Access Number"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Roll Number"] = ds1.Tables[0].Rows[r]["Roll Number"].ToString();
                                dr["Name"] = ds1.Tables[0].Rows[r]["Name"].ToString();
                                dr["Transfer Date"] = Convert.ToDateTime(ds1.Tables[0].Rows[r]["Transfer Date"]).ToString("MM/dd/yyyy");
                                dttransfer.Rows.Add(dr);

                            }


                            grdManualExit.DataSource = dttransfer;
                            grdManualExit.DataBind();
                            grdManualExit.Visible = true;
                            rptprint1.Visible = true;


                        }

                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Found";
                            grdManualExit.Visible = false;
                            rptprint1.Visible = false;
                        }
                        break;
                    case "PER":
                        if (cbfrom.Checked == false)
                            Sql = "SELECT distinct (select lib_name from library where lib_code=book_transfer.from_lib_code), (select lib_name from library where lib_code=book_transfer.to_lib_code),book_transfer.acc_no as 'Access Number',title as 'Title',access_date as 'Access Date',transfer_date as 'Transfer Date' from book_transfer,journal where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and journal.access_code =book_transfer.acc_no  and ltrim(rtrim(booktype))='PER' order by transfer_date asc ";

                        else if (cbfrom.Checked == true)
                            Sql = "SELECT distinct (select lib_name from library where lib_code=book_transfer.from_lib_code), (select lib_name from library where lib_code=book_transfer.to_lib_code),book_transfer.acc_no as 'Access Number',title as 'Title',access_date as 'Access Date',transfer_date as 'Transfer Date'from book_transfer,journal where from_lib_code='" + from_lib + "' and to_lib_code='" + to_lib + "' and journal.access_code =book_transfer.acc_no  and transfer_date between '" + fromdate1 + "' and '" + todate1 + "' and ltrim(rtrim(booktype))='PER' order by transfer_date asc";


                        ds1 = d2.select_method_wo_parameter(Sql, "text");


                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            dttransfer.Columns.Add("SNo", typeof(string));
                            dttransfer.Columns.Add("Access Number", typeof(string));
                            dttransfer.Columns.Add("Title", typeof(string));
                            dttransfer.Columns.Add("Roll Number", typeof(string));
                            dttransfer.Columns.Add("Name", typeof(string));
                            dttransfer.Columns.Add("Transfer Date", typeof(string));



                            int sno = 0;
                            for (int r = 0; r < ds1.Tables[0].Rows.Count; r++)
                            {
                                sno++;
                                dr = dttransfer.NewRow();
                                dr["SNo"] = sno.ToString();
                                dr["Access Number"] = ds1.Tables[0].Rows[r]["Access Number"].ToString();
                                dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                                dr["Access Date"] = Convert.ToDateTime(ds1.Tables[0].Rows[r]["Access Date"]).ToString("MM/dd/yyyy");
                                dr["Transfer Date"] = Convert.ToDateTime(ds1.Tables[0].Rows[r]["Transfer Date"]).ToString("MM/dd/yyyy");
                                dttransfer.Rows.Add(dr);

                            }


                            grdManualExit.DataSource = dttransfer;
                            grdManualExit.DataBind();
                            grdManualExit.Visible = true;
                            rptprint1.Visible = true;


                        }

                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "No Record Found";
                            grdManualExit.Visible = false;
                            rptprint1.Visible = false;
                        }
                        break;
                }
            }
            else if (rbltransfertype.SelectedValue == "Transfer Dept")
            {
                Sql = "SELECT To_Lib_Code,T.Acc_No,Title,Author,Transfer_Date ";
                Sql = Sql + "FROM Book_Transfer T,BookDetails B ";
                Sql = Sql + "WHERE T.Acc_No = B.Acc_No AND T.From_Lib_Code = B.Lib_Code ";
                Sql = Sql + "AND T.Transfer_Type = 2 AND Returned = 0 AND B.Transfered = 1 ";
                if (ddltransferto.Text != "All")
                    Sql = Sql + " AND T.To_Lib_Code ='" + ddltransferto.Text + "' ";

                if (cbfrom.Checked == true)
                    Sql = Sql + "AND Transfer_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "' ";

                ds1 = d2.select_method_wo_parameter(Sql, "text");

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dttransfer.Columns.Add("SNo", typeof(string));
                    dttransfer.Columns.Add("Transfered Date", typeof(string));
                    dttransfer.Columns.Add("Transfered Department", typeof(string));
                    dttransfer.Columns.Add("Acc No", typeof(string));
                    dttransfer.Columns.Add("Title", typeof(string));
                    dttransfer.Columns.Add("Author", typeof(string));



                    int sno = 0;
                    for (int r = 0; r < ds1.Tables[0].Rows.Count; r++)
                    {
                        sno++;
                        dr = dttransfer.NewRow();
                        dr["SNo"] = sno.ToString();
                        dr["Transfered Date"] = Convert.ToDateTime(ds1.Tables[0].Rows[r]["Transfer_Date"]).ToString("MM/dd/yyyy");
                        dr["Transfered Department"] = ds1.Tables[0].Rows[r]["To_Lib_Code"].ToString();
                        dr["Acc No"] = ds1.Tables[0].Rows[r]["Acc_No"].ToString();
                        dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                        dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                        dttransfer.Rows.Add(dr);

                    }

                    grdManualExit.DataSource = dttransfer;
                    grdManualExit.DataBind();
                    grdManualExit.Visible = true;
                    rptprint1.Visible = true;

                }

                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found";
                    grdManualExit.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else if (rbltransfertype.SelectedValue == "Dept Return")
            {
                Sql = "SELECT To_Lib_Code,T.Acc_No,Title,Author,Transfer_Date ";
                Sql = Sql + "FROM Book_Transfer T,BookDetails B ";
                Sql = Sql + "WHERE T.Acc_No = B.Acc_No AND T.From_Lib_Code = B.Lib_Code ";
                Sql = Sql + "AND T.Transfer_Type = 2 AND Returned = 1 ";
                if (ddltransferto.Text != "All")
                    Sql = Sql + " AND T.To_Lib_Code ='" + ddltransferto.Text + "'";

                if (cbfrom.Checked == true)
                    Sql = Sql + "AND Transfer_Date BETWEEN '" + fromdate1 + "' AND '" + todate1 + "' ";

                ds1 = d2.select_method_wo_parameter(Sql, "text");


                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dttransfer.Columns.Add("SNo", typeof(string));
                    dttransfer.Columns.Add("Transfered Department", typeof(string));
                    dttransfer.Columns.Add("Acc No", typeof(string));
                    dttransfer.Columns.Add("Title", typeof(string));
                    dttransfer.Columns.Add("Author", typeof(string));



                    int sno = 0;
                    for (int r = 0; r < ds1.Tables[0].Rows.Count; r++)
                    {
                        sno++;
                        dr = dttransfer.NewRow();
                        dr["SNo"] = sno.ToString();
                        dr["Transfered Department"] = ds1.Tables[0].Rows[r]["To_Lib_Code"].ToString();
                        dr["Acc No"] = ds1.Tables[0].Rows[r]["Acc_No"].ToString();
                        dr["Title"] = ds1.Tables[0].Rows[r]["Title"].ToString();
                        dr["Author"] = ds1.Tables[0].Rows[r]["Author"].ToString();
                        dttransfer.Rows.Add(dr);

                    }

                    grdManualExit.DataSource = dttransfer;
                    grdManualExit.DataBind();
                    grdManualExit.Visible = true;
                    rptprint1.Visible = true;


                }

                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found";
                    grdManualExit.Visible = false;
                    rptprint1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    #endregion



    protected void grdManualExit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void grdManualExit_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdManualExit.PageIndex = e.NewPageIndex;
        btn_go_Click(sender, e);
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        lbl_norec1.Visible = false;
        try
        {
            string reportname = "TransferReports";
            if (reportname.ToString().Trim() != "")
            {


                d2.printexcelreportgrid(grdManualExit, reportname);

                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {

            string duebooks = "TransferReports";
            string pagename = "TransferReports.aspx";


            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdManualExit, pagename, duebooks, 0, ss, "1");

            Printcontrolhed2.Visible = true;
            lbl_norec1.Visible = false;
        }

        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "TransferReports");
        }
    }


    #endregion
}