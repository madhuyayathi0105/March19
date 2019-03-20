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
using System.Globalization;
using System.Configuration;

public partial class LibraryMod_Subscription_Subscribe : System.Web.UI.Page
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
    string Sql = "";
    DataSet dsload = new DataSet();
    DateTime StrCDate = new DateTime();
    int StrDayNum = 0;
    string StrDay = "";
    string StrMonth = "";
    string strYear = "";
    int IntMonthNum = 0;
    int StrIssYear = 0;
    int StartCount = 0;
    int IntMonthIssueNo = 0;
    int IntPrevMonth = 0;
    int IntPayMode = 0;
    DateTime StrListDate1 = new DateTime();
    int n = 0;
    int i = 0;
    string Accdate = "";
    string Acctime = "";
    string StrCount = "";
    string StrMonthIssNo = "";
    int IntCount = 0;
    int insave = 0;
    Boolean Cellclick = false;
    DataSet dsgetjcode = new DataSet();
    string sname = "";
    DataTable subsentry = new DataTable();
    DataTable accessno = new DataTable();
    DataRow drsubs;
    DataRow drsubaccess;
    DataTable supp = new DataTable();
    DataRow drsuppl;
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
                Type();
                search();
                ReportType();
                DeliveryType();
                Active();
                loadlang();
                loadbud();
                Binddeopt();
                dtp_subsdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                dtp_subsdate.Attributes.Add("readonly", "readonly");
                fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                fromdate.Attributes.Add("readonly", "readonly");
                todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                todate.Attributes.Add("readonly", "readonly");
                DTP_StartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                DTP_StartDate.Attributes.Add("readonly", "readonly");
                dtp_renewaldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                dtp_renewaldate.Attributes.Add("readonly", "readonly");
                dtp_dddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                dtp_dddate.Attributes.Add("readonly", "readonly");
                DTP_UpdStartDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                DTP_UpdStartDate.Attributes.Add("readonly", "readonly");
                DTP_ChangeDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                DTP_ChangeDate.Attributes.Add("readonly", "readonly");
                grdSubscription.Visible = false;
                //rptprint.Visible = false;
                ddl_delivery.Items.Add("All");
                for (int i = 1900; i <= DateTime.Now.AddYears(4).Year; i++)
                {
                    ddl_year.Items.Add(i.ToString());
                    dd_Subyr.Items.Add(i.ToString());
                }
                string year = Convert.ToString(DateTime.Now.ToString("yyyy"));
                ddl_year.Items.FindByText(year).Selected = true;
                dd_Subyr.Items.FindByText(year).Selected = true;
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdSubscription.Visible = false;
        // rptprint.Visible = false;
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
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;
            Library(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void Library(string LibCodeCol)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                SelectQ = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCodeCol + " and college_code in('" + College + "') ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(SelectQ, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();

                    ddl_lib.DataSource = ds;
                    ddl_lib.DataTextField = "lib_name";
                    ddl_lib.DataValueField = "lib_code";
                    ddl_lib.DataBind();

                    ddl_sub_lib.DataSource = ds;
                    ddl_sub_lib.DataTextField = "lib_name";
                    ddl_sub_lib.DataValueField = "lib_code";
                    ddl_sub_lib.DataBind();

                    ddl_supp_lib.DataSource = ds;
                    ddl_supp_lib.DataTextField = "lib_name";
                    ddl_supp_lib.DataValueField = "lib_code";
                    ddl_supp_lib.DataBind();
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            subyear();
            grdSubscription.Visible = false;
            // rptprint.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }

    #endregion

    public void Binddeopt()
    {
        try
        {

            string Collegecode = ddlCollege.SelectedValue.ToString();
            string hed = " SELECT  Dept_Name  FROM Department  where College_Code='" + Collegecode + "'  ORDER BY Dept_Name ";
            DataSet ds2 = d2.select_method_wo_parameter(hed, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddldept.DataSource = ds2;
                ddldept.DataTextField = "Dept_Name";
                ddldept.DataValueField = "Dept_Name";
                ddldept.DataBind();

            }

        }
        catch
        {
        }
    }

    #region Type
    public void Type()
    {
        try
        {
            ddl_Type.Items.Add("Journals");
            ddl_Type.Items.Add("News Papers");
            ddl_Type.Items.Add("All");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdSubscription.Visible = false;
        // rptprint.Visible = false;
    }
    #endregion

    #region Sub.Year

    public void subyear()
    {
        try
        {
            //ddl_year.Items.Clear();
            //ds = da.select_method_wo_parameter("bind_batch", "sp");
            //int count = ds.Tables[0].Rows.Count;
            //if (count > 0)
            //{
            //    ddl_year.DataSource = ds;
            //    ddl_year.DataTextField = "batch_year";
            //    ddl_year.DataValueField = "batch_year";
            //    ddl_year.DataBind();

            //    dd_Subyr.DataSource = ds;
            //    dd_Subyr.DataTextField = "batch_year";
            //    dd_Subyr.DataValueField = "batch_year";
            //    dd_Subyr.DataBind();
            //}
            //int count1 = ds.Tables[1].Rows.Count;
            //if (count > 0)
            //{
            //    int max_bat = 0;
            //    max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            //    ddl_year.SelectedValue = max_bat.ToString();
            //}
            //ddl_year.Items.Clear();

            //libcode = Convert.ToString(ddllibrary.SelectedValue);

            //if (!string.IsNullOrEmpty(userCollegeCode) && !string.IsNullOrEmpty(libcode))
            //{
            //    string yer = "select distinct s.Subscription_Year from subscription s,library l where l.college_code=" + userCollegeCode + " and l.lib_code=s.lib_code and l.lib_code='" + libcode + "'";
            //    ds.Clear();
            //    ds = d2.select_method_wo_parameter(yer, "text");
            //}
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    ddl_year.DataSource = ds;
            //    ddl_year.DataTextField = "Subscription_Year";
            //    ddl_year.DataValueField = "Subscription_Year";
            //    ddl_year.DataBind();

            //    dd_Subyr.DataSource = ds;
            //    dd_Subyr.DataTextField = "Subscription_Year";
            //    dd_Subyr.DataValueField = "Subscription_Year";
            //    dd_Subyr.DataBind();


            //}
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }

    protected void Chk_year_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_year.Checked == true)
                ddl_year.Enabled = true;
            else
                ddl_year.Enabled = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
    {

        grdSubscription.Visible = false;
        // rptprint.Visible = false;
    }

    #endregion

    #region Search
    public void search()
    {
        try
        {

            ddlsearch.Items.Clear();
            ddlsearch.Items.Add("All");
            ddlsearch.Items.Add("Supplier Name");
            ddlsearch.Items.Add("Journal Code");
            ddlsearch.Items.Add("Journal Name");
            ddlsearch.Items.Add("DD Number");


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }

    protected void ddlSearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdSubscription.Visible = false;
            // rptprint.Visible = false;
            if (ddlsearch.Text == "All")
            {
                txt_bysearch.Visible = false;
                Label_lang.Visible = false;
                ddldept.Visible = false;
            }
            else if (ddlsearch.Text == "Journal Name")
            {
                txt_bysearch.Visible = true;
                txt_bysearch.Width = 100;
                Label_lang.Visible = true;
                ddldept.Visible = false;
                Cbo_TitleLanguage.Visible = true;
            }

            else
            {
                Label_lang.Visible = false;
                txt_bysearch.Visible = true;
                Cbo_TitleLanguage.Visible = false;
                txt_bysearch.Width = 175;
                ddldept.Visible = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    protected void ddldep_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    #endregion

    #region ReportType
    public void ReportType()
    {
        try
        {

            ddl_reportype.Items.Clear();
            ddl_reportype.Items.Add("Subscribed");
            ddl_reportype.Items.Add("Not Subscribed");


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }



    }

    protected void ddl_reportype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdSubscription.Visible = false;
            // rptprint.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    #endregion

    #region DeliveryType
    public void DeliveryType()
    {
        try
        {
            ddl_delivery.Items.Clear();
            collcode = ddlCollege.SelectedValue.ToString();
            string delivery = "SELECT DISTINCT ISNULL(DeliveryType,'') DeliveryType  FROM Journal_Master J,Library L WHERE J.Lib_Code = L.Lib_Code AND College_Code ='" + collcode + "' ORDER BY DeliveryType ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(delivery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_delivery.DataSource = ds;
                ddl_delivery.DataTextField = "DeliveryType";
                ddl_delivery.DataValueField = "DeliveryType";
                ddl_delivery.DataBind();
                ddl_delivery.Items.Add("All");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    protected void ddl_delivery_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdSubscription.Visible = false;
        // rptprint.Visible = false;
    }
    #endregion

    #region Active

    public void Active()
    {
        try
        {
            rblActive.Items.Add("Active");
            rblActive.Items.Add("InActive");
            rblActive.Items.Add("Both");
            rblActive.Items.FindByText("Active").Selected = true;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }
    protected void rblActive_Selected(object sender, EventArgs e)
    {
        grdSubscription.Visible = false;
        // rptprint.Visible = false;
    }
    #endregion

    #region Language
    public void loadlang()
    {
        try
        {
            Cbo_TitleLanguage.Items.Clear();
            Cbo_TitleLanguage.Items.Add("English");
            Cbo_TitleLanguage.Items.Add("Tamil");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }

    protected void ddl_lang_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdSubscription.Visible = false;
        // rptprint.Visible = false;
    }
    #endregion

    #region Go

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetrecords = new DataSet();
            dsgetrecords = getdetails();
            if (dsgetrecords.Tables.Count > 0 && dsgetrecords.Tables[0].Rows.Count > 0)
            {
                if (ddl_reportype.SelectedIndex == 0)
                    loadspreaddetails(dsgetrecords);
                else
                    loadspreaddetail(dsgetrecords);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
    }

    #endregion

    #region loadsubscribe

    private DataSet getdetails()
    {
        try
        {
            #region get Value

            string getrecord = "";
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_reportype.SelectedIndex == 0)
            {
                getrecord = "SELECT Subs_Code,CONVERT(varchar(20),Subs_Date,103)Subs_Date,CONVERT(varchar(20),FromDate,103)FromDate,CONVERT(varchar(20),ToDate,103)ToDate,Supplier,Journal_Name,Subscription_Year,ISNULL(TotIssues,0) TotIssues,ISNULL(Subscription_Price,0) Subscription_Price,subs_quotation.quo_code,subs_quotation.supsubs_code,SubsCost,subscription.DD_No,DDAmount,isnull(TamilJrnlName,'') TamilJrnlName,isnull(TitleLanguage,0) TitleLanguage FROM  subscription LEFT JOIN subs_quotation ON subscription.quo_code = subs_quotation.quo_code AND subscription.lib_code = subs_quotation.lib_code LEFT JOIN CO_VendorMaster ON subscription.supplier_code = CO_VendorMaster.VendorCode INNER JOIN journal_master ON subscription.journal_code = journal_master.journal_code where subscription.lib_code = '" + libcode + "'";

                if (ddlsearch.Text == "Journal Name" && txt_bysearch.Text != "")
                {
                    if (Cbo_TitleLanguage.Text == "Tamil")
                        getrecord += " and isnull(journal_master.titlelanguage,0) = 1  and journal_master.journal_name like '" + txt_bysearch.Text + "%'";
                    else
                        getrecord += " and isnull(journal_master.titlelanguage,0) = 0 and journal_master.journal_name like '" + txt_bysearch.Text + "%'";
                }
                if (Chk_year.Checked == true)
                    getrecord += " and Subscription_Year = '" + ddl_year.Text + "'";
                if (txt_bysubscode.Text != "")
                    getrecord += " and subs_code = '" + txt_bysubscode.Text + "'";
                if (ddlsearch.Text == "Supplier Name" && txt_bysearch.Text != "")
                    getrecord += " and CO_VendorMaster.VendorCompName like '" + txt_bysearch.Text + "%'";
                if (ddlsearch.Text == "Journal Code" && txt_bysearch.Text != "")
                    getrecord += " and subscription.journal_code ='" + txt_bysearch.Text + "'";
                if (ddlsearch.Text == "DD Number" && txt_bysearch.Text != "")
                    getrecord += " and subscription.dd_no='" + txt_bysearch.Text + "'";
                if (ddlsearch.Text == "DD Date" && txt_bysearch.Text != "")
                    getrecord += " and subscription.dd_date='" + txt_bysearch.Text + "'";

                if (rblActive.SelectedIndex == 0)
                    getrecord += " and subscription.active = 1";
                else if (rblActive.SelectedIndex == 1)
                    getrecord += " and subscription.active = 0";
                if (ddl_Type.Text == "Journals")
                    getrecord += " AND ISNULL(PeriodicalType,1) = 1 ";
                else if (ddl_Type.Text == "News Papers")
                    getrecord += " AND ISNULL(PeriodicalType,1) = 2 ";
                if (ddl_delivery.Text != "All")
                    getrecord += " and DeliveryType ='" + ddl_delivery.Text + "' ";
                getrecord += " order by journal_master.journal_name,Subscription_Year ";
            }
            else
            {
                getrecord = "SELECT Journal_Code,Journal_Name,Supplier,ISNULL(TitleLanguage,0) TitleLanguage  FROM Journal_Master where Journal_Code not in (SELECT Journal_Code FROM Subscription WHERE Subscription_year ='" + ddl_year.Text + "' and Journal_master.lib_Code = Subscription.lib_Code and lib_Code ='" + libcode + "' )and lib_Code ='" + libcode + "'  ";
                if (ddlsearch.Text == "Journal Name" && txt_bysearch.Text != "")
                {
                    if (Cbo_TitleLanguage.Text == "Tamil")
                        getrecord += " and isnull(journal_master.titlelanguage,0) = 1  and journal_master.journal_name like '" + txt_bysearch.Text + "%'";
                    else
                        getrecord += " and isnull(journal_master.titlelanguage,0) = 0 and journal_master.journal_name like '" + txt_bysearch.Text + "%'";
                }
                if (ddlsearch.Text == "Supplier Name" && txt_bysearch.Text != "")
                    getrecord += " and Supplier like '" + txt_bysearch.Text + "%'";
                if (ddlsearch.Text == "Journal Code" && txt_bysearch.Text != "")
                    getrecord += " and journal_code ='" + txt_bysearch.Text + "'";
                if (ddlsearch.Text == "Department" && ddldept.Text != "")
                    getrecord += " and Department.Dept_Name='" + ddldept.SelectedItem + "'";
                if (ddl_Type.Text == "Journals")
                    getrecord += " AND ISNULL(PeriodicalType,1) = 1 ";
                else if (ddl_Type.Text == "News Papers")
                    getrecord += " AND ISNULL(PeriodicalType,1) = 2 ";
                getrecord += " order by journal_master.journal_name ";

            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(getrecord, "Text");
            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
        return dsload;
    }

    public void loadspreaddetails(DataSet ds)
    {
        try
        {
            subsentry.Columns.Add("SNo", typeof(string));
            subsentry.Columns.Add("Journal Name", typeof(string));
            subsentry.Columns.Add("Subs Date", typeof(string));
            subsentry.Columns.Add("Subs Year", typeof(string));
            subsentry.Columns.Add("Subs Period From", typeof(string));
            subsentry.Columns.Add("Subs Period To", typeof(string));
            subsentry.Columns.Add("Price", typeof(string));
            subsentry.Columns.Add("Subs Amount", typeof(string));
            subsentry.Columns.Add("DD No", typeof(string));
            subsentry.Columns.Add("DD Amount", typeof(string));
            subsentry.Columns.Add("Supplier", typeof(string));
            subsentry.Columns.Add("Subs Code", typeof(string));
            subsentry.Columns.Add("Journal Code", typeof(string));


            int sno = 0;
            string sfrom = "";
            string sto = "";

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drsubs = subsentry.NewRow();
                    string lang = Convert.ToString(ds.Tables[0].Rows[row]["TitleLanguage"]).Trim();
                    string jtitle = Convert.ToString(ds.Tables[0].Rows[row]["Journal_Name"]).Trim();
                    string sdate = Convert.ToString(ds.Tables[0].Rows[row]["Subs_Date"]).Trim();
                    string syear = Convert.ToString(ds.Tables[0].Rows[row]["Subscription_Year"]).Trim();
                    string sfdate = Convert.ToString(ds.Tables[0].Rows[row]["FromDate"]).Trim();
                    string stdte = Convert.ToString(ds.Tables[0].Rows[row]["ToDate"]).Trim();
                    string sprice = Convert.ToString(ds.Tables[0].Rows[row]["Subscription_Price"]).Trim();
                    string scost = Convert.ToString(ds.Tables[0].Rows[row]["SubsCost"]).Trim();
                    string sdd = Convert.ToString(ds.Tables[0].Rows[row]["DD_No"]).Trim();
                    string ssddam = Convert.ToString(ds.Tables[0].Rows[row]["DDAmount"]).Trim();
                    sname = Convert.ToString(ds.Tables[0].Rows[row]["Supplier"]).Trim();
                    string subcode = Convert.ToString(ds.Tables[0].Rows[row]["Subs_Code"]).Trim();
                    if (sfdate != "")
                    {
                        string[] fromsplit = sfdate.Split(' ');
                        sfrom = fromsplit[0];
                    }
                    if (stdte != "")
                    {
                        string[] tosplit = stdte.Split(' ');
                        sto = tosplit[0];
                    }
                    drsubs["SNo"] = Convert.ToString(sno);
                    if (lang == "1")
                    {
                        drsubs["Journal Name"] = jtitle;
                    }
                    else
                    {
                        drsubs["Journal Name"] = jtitle;
                    }
                    drsubs["Subs Date"] = sdate;
                    drsubs["Subs Year"] = syear;
                    drsubs["Subs Period From"] = sfrom;
                    drsubs["Subs Period To"] = sto;
                    drsubs["Price"] = sprice;
                    drsubs["Subs Amount"] = scost;
                    drsubs["DD No"] = sdd;
                    drsubs["DD Amount"] = ssddam;
                    drsubs["Supplier"] = sname;
                    drsubs["Subs Code"] = subcode;
                    subsentry.Rows.Add(drsubs);
                }
                grdSubscription.DataSource = subsentry;
                grdSubscription.DataBind();

                grdSubscription.Visible = true;
                rptprint.Visible = true;
                for (int l = 0; l < grdSubscription.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdSubscription.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdSubscription.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdSubscription.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            grdSubscription.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                            grdSubscription.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                            grdSubscription.Rows[l].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                            grdSubscription.Rows[l].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                            grdSubscription.Rows[l].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                            grdSubscription.Rows[l].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                            grdSubscription.Rows[l].Cells[9].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
    }

    public void loadspreaddetail(DataSet ds)
    {
        try
        {
            subsentry.Columns.Add("SNo", typeof(string));
            subsentry.Columns.Add("Journal Name", typeof(string));
            subsentry.Columns.Add("Subs Date", typeof(string));
            subsentry.Columns.Add("Subs Year", typeof(string));
            subsentry.Columns.Add("Subs Period From", typeof(string));
            subsentry.Columns.Add("Subs Period To", typeof(string));
            subsentry.Columns.Add("Price", typeof(string));
            subsentry.Columns.Add("Subs Amount", typeof(string));
            subsentry.Columns.Add("DD No", typeof(string));
            subsentry.Columns.Add("DD Amount", typeof(string));
            subsentry.Columns.Add("Supplier", typeof(string));
            subsentry.Columns.Add("Subs Code", typeof(string));
            subsentry.Columns.Add("Journal Code", typeof(string));


            int sno = 0;
            string id = "";
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsload.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drsubs = subsentry.NewRow();
                    string lang = Convert.ToString(dsload.Tables[0].Rows[row]["TitleLanguage"]).Trim();
                    string jname = Convert.ToString(dsload.Tables[0].Rows[row]["Journal_Name"]).Trim();
                    string sname = Convert.ToString(dsload.Tables[0].Rows[row]["Supplier"]).Trim();
                    string jcode = Convert.ToString(dsload.Tables[0].Rows[row]["Journal_Code"]).Trim();

                    drsubs["SNo"] = Convert.ToString(sno);
                    if (lang == "1")
                    {
                        drsubs["Journal Name"] = jname;
                    }
                    else
                    {
                        drsubs["Journal Name"] = jname;

                    }

                    drsubs["Supplier"] = sname;
                    drsubs["Journal Code"] = jcode;
                    subsentry.Rows.Add(drsubs);
                }
                grdSubscription.DataSource = subsentry;
                grdSubscription.DataBind();

                grdSubscription.Visible = true;
                rptprint.Visible = true;

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }



    protected void grdSubscription_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ddl_reportype.SelectedIndex == 0)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
            }
        }
        else if (ddl_reportype.SelectedIndex == 1)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
            }
        }
    }

    protected void grdSubscription_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdSubscription_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {

            popview.Visible = true;
            Page.MaintainScrollPositionOnPostBack = false;
            btn_save.Visible = false;
            btn_update.Visible = true;
            btn_Delete.Visible = true;
            loadbank();
            loadBranch();
            loadPlace();
            Page.MaintainScrollPositionOnPostBack = false;

            string activerow = "";
            string type = "";
            string libname = "";
            string getupdatebookqry = "";
            DataSet dsgetupdatebook = new DataSet();
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            if (ddllibrary.Items.Count > 0)
            {
                libname = Convert.ToString(ddllibrary.SelectedItem.Text);
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            }
            if (Convert.ToString(rowIndex) != "")
            {
                if (ddl_reportype.SelectedIndex == 0)
                {
                    string subcode = Convert.ToString(grdSubscription.Rows[rowIndex].Cells[11].Text);
                    txt_subscode.Text = Convert.ToString(subcode);
                    string jcode = d2.GetFunction("SELECT Journal_Code from subscription s where s.lib_code = '" + libcode + "' and s.subs_code='" + subcode + "'");
                    string suppcode = d2.GetFunction("SELECT Supplier_Code from subscription s where s.lib_code = '" + libcode + "' and s.subs_code='" + subcode + "'");
                    string getsql = "select * from Journal_Master where journal_code='" + jcode + "' ";
                    getsql += "select * from supplier_details where supplier_code='" + suppcode + "'";
                    dsgetupdatebook.Clear();
                    dsgetupdatebook = d2.select_method_wo_parameter(getsql, "Text");
                    if (dsgetupdatebook.Tables.Count > 0 && dsgetupdatebook.Tables[0].Rows.Count > 0)
                    {
                        txt_joucode.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["Journal_Code"]);
                        txt_title.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["journal_name"]);
                        Textperiodicity.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["Periodicity"]);
                        Txt_IssueBy.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["IssueBy"]);
                        Txt_PerIssue.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["PerIssueNo"]);
                        Txt_TotIssue.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["TotalNoIssues"]);
                        Txt_IssueType.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["IssueType"]);
                        Txt_IssueTypeVal.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["IssueTypeVAl"]);
                        text_ttamil.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["TamilJrnlName"]);
                        txt_journalprice.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["journal_price"]);
                        Txt_SubsAmt.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["SubsAmount"]);
                        txt_remarks.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["remarks"]);
                        txt_journalissues.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["TotalNoIssues"]);
                        txt_supp_name.Text = sname;
                    }
                    if (dsgetupdatebook.Tables.Count > 0 && dsgetupdatebook.Tables[1].Rows.Count > 0)
                    {
                        Textadd.Text = Convert.ToString(dsgetupdatebook.Tables[1].Rows[0]["doorst_no"]);
                        Textemail.Text = Convert.ToString(dsgetupdatebook.Tables[1].Rows[0]["EmailID1"]);
                        TextWebsite.Text = Convert.ToString(dsgetupdatebook.Tables[1].Rows[0]["website"]);
                    }
                }
                else
                {
                    string jocode = Convert.ToString(grdSubscription.Rows[rowIndex].Cells[12].Text);
                    string subcodde = d2.GetFunction("SELECT subs_code from subscription s where s.lib_code = '" + libcode + "' and Journal_Code='" + jocode + "'");
                    if (subcodde == "0" || subcodde == "")
                    {
                        AutoGen();
                    }
                    else
                    {
                        txt_subscode.Text = Convert.ToString(subcodde);
                    }
                    string getsql = "SELECT * from Journal_Master j where j.lib_code = '" + libcode + "' and j.journal_code='" + jocode + "'";
                    dsgetupdatebook.Clear();
                    dsgetupdatebook = d2.select_method_wo_parameter(getsql, "Text");
                    if (dsgetupdatebook.Tables[0].Rows.Count > 0)
                    {
                        string jcode = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["Journal_Code"]);
                        txt_joucode.Text = Convert.ToString(jcode);
                        txt_title.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["journal_name"]);
                        Textperiodicity.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["Periodicity"]);
                        Txt_IssueBy.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["IssueBy"]);
                        Txt_PerIssue.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["PerIssueNo"]);
                        Txt_TotIssue.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["TotalNoIssues"]);
                        Txt_IssueType.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["IssueType"]);
                        Txt_IssueTypeVal.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["IssueTypeVAl"]);
                        text_ttamil.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["TamilJrnlName"]);
                        txt_journalprice.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["journal_price"]);
                        Txt_SubsAmt.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["SubsAmount"]);
                        txt_remarks.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["remarks"]);

                        txt_journalissues.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["TotalNoIssues"]);
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
            string degreedetails = "Subscription_Subscribe";
            string pagename = "Subscription_Subscribe.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdSubscription, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdSubscription, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
    #endregion

    #region Add

    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            popview.Visible = true;
            loadbank();
            loadBranch();
            loadPlace();
            AutoGen();
            Clear();
            btn_save.Visible = true;
            btn_update.Visible = false;
            btn_Delete.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
    }

    #endregion

    #region Addpopup

    protected void ddl_lib_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void loadbud()
    {
        try
        {

            Cbo_Head.Items.Clear();
            if (!string.IsNullOrEmpty(userCollegeCode))
            {
                string yer = "Select TextVal,textcode from textvaltable where college_code='" + userCollegeCode + "' and TextCriteria='LBHed' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(yer, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Cbo_Head.DataSource = ds;
                Cbo_Head.DataTextField = "TextVal";
                Cbo_Head.DataValueField = "textcode";
                Cbo_Head.DataBind();

            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    protected void ddl_Budget_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void dd_Subyr_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadMonths();

    }

    #region Journalcodepopup


    protected void btn_joucode_OnClick(object sener, EventArgs e)
    {
        popupselectjournal_Code.Visible = true;
    }

    protected void ddl_sub_libOnSelectedIndexChanged(object sender, EventArgs e)
    {
        grdPeriodical.Visible = false;

        btn_jour_exit1.Visible = false;

    }

    //protected void grdPeriodical_onpageindexchanged(object sender, GridViewPageEventArgs e)
    //{
    //    grdPeriodical.PageIndex = e.NewPageIndex;
    //    btn_perio_go_Click(sender, e);
    //}

    protected void grdPeriodical_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdPeriodical_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            string title1 = "";
            string accno1 = "";
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            popupselectjournal_Code.Visible = false;


            string var_suppliercode = grdPeriodical.Rows[rowIndex].Cells[1].Text;
            if (ddl_sub_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_sub_lib.SelectedValue);
            if (var_suppliercode != "")
            {
                string sqljournal = "select journal_code,journal_name,ISNULL(Periodicity,'') Periodicity,ISNULL(IssueBy,0) IssueBy,ISNULL(PerIssueNo,0) PerIssueNo,ISNULL(TotalNoIssues,0) TotalNoIssues,ISNULL(IssueType,0) IssueType,ISNULL(IssueTypeVAl,'') IssueTypeVAl,ISNULL(TamilJrnlName ,'') TamilJrnlName,isnull(TitleLanguage,0) TitleLanguage,journal_price,IssueBy  from journal_master where journal_code = '" + var_suppliercode + "' and lib_code='" + libcode + "'";
                dsgetjcode.Clear();
                dsgetjcode = d2.select_method_wo_parameter(sqljournal, "Text");
            }
            if (dsgetjcode.Tables[0].Rows.Count > 0)
            {
                string jlang = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["TitleLanguage"]);
                if (jlang == "1")
                    txt_title.Font.Name = "Amudham";
                else
                    txt_title.Font.Name = "Arial";
                txt_joucode.Text = var_suppliercode;
                txt_title.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["journal_name"]);
                Textperiodicity.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["Periodicity"]);
                Txt_IssueBy.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["IssueBy"]);
                Txt_PerIssue.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["PerIssueNo"]);
                Txt_TotIssue.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["TotalNoIssues"]);
                Txt_IssueType.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["IssueType"]);
                Txt_IssueTypeVal.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["IssueTypeVAl"]);
                text_ttamil.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["TamilJrnlName"]);
                txt_journalprice.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["journal_price"]);
                Txt_SubsAmt.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["journal_price"]);
                txt_journalissues.Text = Convert.ToString(dsgetjcode.Tables[0].Rows[0]["IssueBy"]);
            }


        }
        catch
        {
        }
    }

    protected void btn_perio_go_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetjourcode = new DataSet();
            string search1 = "";
            dsgetjourcode = getjournalnodetails();
            if (dsgetjourcode.Tables.Count > 0 && dsgetjourcode.Tables[0].Rows.Count > 0)
            {
                loadspreadjourcodedetails(dsgetjourcode);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }

    #region Load_Access_No

    private DataSet getjournalnodetails()
    {
        DataSet dsload2 = new DataSet();
        try
        {
            #region get Value

            string sqlgetaccno = "";
            string search = "";
            string libcode = "";
            string code = "";
            string title = "";

            if (ddl_sub_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_sub_lib.SelectedValue);
            if (txt_code.Text != "")
                code = "and journal_code='" + txt_code.Text + "'";
            if (txtTitle.Text != "")
                title = "and journal_name ='" + txtTitle.Text + "' ";
            if (!string.IsNullOrEmpty(libcode))
            {

                sqlgetaccno = "select journal_code,journal_name,isnull(TamilJrnlName,'') TamilJrnlName,isnull(TitleLanguage,0) TitleLanguage from journal_master where lib_code = '" + libcode + "' " + code + title + " order by len(journal_code),journal_code";
                dsload2.Clear();
                dsload2 = d2.select_method_wo_parameter(sqlgetaccno, "Text");
            }


            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


        return dsload2;


    }

    public void loadspreadjourcodedetails(DataSet ds)
    {
        try
        {

            accessno.Columns.Add("Code", typeof(string));
            accessno.Columns.Add("Periodical Title", typeof(string));

            int sno = 0;
            string id = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drsubaccess = accessno.NewRow();

                    string lang = Convert.ToString(ds.Tables[0].Rows[row]["TitleLanguage"]).Trim();
                    string jtitle = Convert.ToString(ds.Tables[0].Rows[row]["Journal_Name"]).Trim();
                    string jcode = Convert.ToString(ds.Tables[0].Rows[row]["journal_code"]).Trim();


                    drsubaccess["Code"] = jcode;
                    if (lang == "1")
                    {
                        drsubaccess["Periodical Title"] = jtitle;

                    }
                    else
                    {
                        drsubaccess["Periodical Title"] = jtitle;
                    }

                    accessno.Rows.Add(drsubaccess);

                }
                grdPeriodical.DataSource = accessno;
                grdPeriodical.DataBind();
                grdPeriodical.Visible = true;

                btn_jour_exit1.Visible = true;


            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    #region Journal_Ok_And_Exit


    protected void btn_jour_exit1_Click(object sender, EventArgs e)
    {
        popupselectjournal_Code.Visible = false;
    }
    #endregion


    #endregion

    #endregion

    #region SupplierNamePopup

    //protected void grdSupplier_onpageindexchanged(object sender, GridViewPageEventArgs e)
    //{
    //    grdSupplier.PageIndex = e.NewPageIndex;
    //    btn_supp_go_Click(sender, e);
    //}

    protected void grdSupplier_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdSupplier_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            popupselectjournal_Code.Visible = false;

            DataSet dssuppde = new DataSet();
            DivSuppliername.Visible = false;

            if (Convert.ToString(rowIndex) != "")
            {
                string dupcode = Convert.ToString(grdSupplier.Rows[rowIndex].Cells[1].Text);
                txt_supp_name.Text = Convert.ToString(grdSupplier.Rows[rowIndex].Cells[2].Text);
                string suppdetails = "select * from supplier_details where supplier_code='" + dupcode + "'";
                dssuppde.Clear();
                dssuppde = d2.select_method_wo_parameter(suppdetails, "Text");
                if (dssuppde.Tables[0].Rows.Count > 0)
                {
                    Textadd.Text = Convert.ToString(dssuppde.Tables[0].Rows[0]["doorst_no"]);
                    Textemail.Text = Convert.ToString(dssuppde.Tables[0].Rows[0]["EmailID1"]);
                    TextWebsite.Text = Convert.ToString(dssuppde.Tables[0].Rows[0]["website"]);

                }

            }

        }
        catch
        {
        }
    }

    protected void btn_supp_name_OnClick(object sender, EventArgs e)
    {
        DivSuppliername.Visible = true;
    }

    protected void ddl_supp_lib_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        grdPeriodical.Visible = false;

        btn_supp_exit1.Visible = false;
    }

    protected void btn_supp_go_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetsuppcode = new DataSet();
            dsgetsuppcode = getsuppnodetails();
            if (dsgetsuppcode.Tables.Count > 0 && dsgetsuppcode.Tables[0].Rows.Count > 0)
            {
                loadspreadsuppcodedetails(dsgetsuppcode);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
    }

    private DataSet getsuppnodetails()
    {
        DataSet dsload3 = new DataSet();
        try
        {
            #region get Value

            string sqlsuppcode = "";
            if (TextSuppliercoe.Text == "" && TextSuppliername.Text == "")
                sqlsuppcode = "select VendorCode,VendorCompName from CO_VendorMaster where LibraryFlag='1'";
            else if (TextSuppliercoe.Text != "" && TextSuppliername.Text != "")
                sqlsuppcode = "select VendorCode,VendorCompName from CO_VendorMaster where VendorCode='" + TextSuppliercoe.Text + "' and VendorCompName ='" + TextSuppliername.Text + "' and LibraryFlag='1'";
            else if (TextSuppliername.Text != "")
                sqlsuppcode = "select VendorCode,VendorCompName from CO_VendorMaster where  VendorCompName ='" + TextSuppliername.Text + "' and LibraryFlag='1' ";
            else
                sqlsuppcode = "select VendorCode,VendorCompName from CO_VendorMaster where  VendorCode='" + TextSuppliercoe.Text + "' and LibraryFlag='1' ";
            dsload3.Clear();
            dsload3 = d2.select_method_wo_parameter(sqlsuppcode, "Text");

            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
        return dsload3;
    }

    public void loadspreadsuppcodedetails(DataSet ds)
    {
        try
        {
            supp.Columns.Add("Supplier Code");
            supp.Columns.Add("Supplier Name");
            int sno = 0;
            string id = "";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drsuppl = supp.NewRow();
                    string scode = Convert.ToString(ds.Tables[0].Rows[row]["VendorCode"]).Trim();
                    string sname = Convert.ToString(ds.Tables[0].Rows[row]["VendorCompName"]).Trim();
                    drsuppl["Supplier Code"] = scode;
                    drsuppl["Supplier Name"] = sname;
                    supp.Rows.Add(drsuppl);
                }
                grdSupplier.DataSource = supp;
                grdSupplier.DataBind();
                grdSupplier.Visible = true;
                btn_supp_exit1.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
    }

    protected void btn_supp_exit1_Click(object sender, EventArgs e)
    {
        DivSuppliername.Visible = false;
    }

    #endregion

    protected void rbl_amttype_Selected(object sender, EventArgs e)
    {
        try
        {
            if (rbl_amttype.SelectedIndex == 0)
            {
                Label_dd.Text = "DD No:";
                Label_da.Text = "DD Date:";
                Label_dm.Text = "DD Amt:";
            }
            else if (rbl_amttype.SelectedIndex == 1)
            {
                Label_dd.Text = "Cheque No:";
                Label_da.Text = "Cheque Date:";
                Label_dm.Text = "Cheque Amt:";
            }
            else
            {
                Label_dd.Text = "Transfer No:";
                Label_da.Text = "Transfer Date:";
                Label_dm.Text = "Transfer Amt:";

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
    }


    #region BankName
    public void loadbank()
    {
        try
        {
            Cbo_BankName.Items.Clear();
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (!string.IsNullOrEmpty(collcode))
            {
                string yer = "SELECT DISTINCT ISNULL(BankName,'') BankName FROM Subscription S,Library L WHERE S.Lib_Code = L.Lib_Code AND College_Code ='" + collcode + "' AND ISNULL(BankName,'') <> ''";
                ds.Clear();
                ds = d2.select_method_wo_parameter(yer, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Cbo_BankName.DataSource = ds;
                Cbo_BankName.DataTextField = "BankName";
                Cbo_BankName.DataValueField = "BankName";
                Cbo_BankName.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }
    protected void ddl_Bank_Name_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region Branch
    public void loadBranch()
    {
        try
        {
            Cbo_Branch.Items.Clear();
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (!string.IsNullOrEmpty(collcode))
            {
                string yer = "SELECT DISTINCT ISNULL(Branch,'') Branch FROM Subscription S,Library L WHERE S.Lib_Code = L.Lib_Code AND College_Code ='" + collcode + "' AND ISNULL(Branch,'') <> ''";
                ds.Clear();
                ds = d2.select_method_wo_parameter(yer, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Cbo_Branch.DataSource = ds;
                Cbo_Branch.DataTextField = "Branch";
                Cbo_Branch.DataValueField = "Branch";
                Cbo_Branch.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }
    protected void ddl_Branch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region Place
    public void loadPlace()
    {
        try
        {
            Cbo_Place.Items.Clear();
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (!string.IsNullOrEmpty(collcode))
            {
                string yer = "SELECT DISTINCT ISNULL(Place,'') Place FROM Subscription S,Library L WHERE S.Lib_Code = L.Lib_Code AND College_Code ='" + collcode + "' AND ISNULL(Place,'') <> ''";
                ds.Clear();
                ds = d2.select_method_wo_parameter(yer, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Cbo_Place.DataSource = ds;
                Cbo_Place.DataTextField = "Place";
                Cbo_Place.DataValueField = "Place";
                Cbo_Place.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }
    protected void ddl_place_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    public void LoadMonths()
    {
        try
        {
            int k = 0;
            string strDate = "";
            int c = 0;
            Lst_IssueList.Items.Clear();
            string IssueType = "";
            if (Txt_IssueTypeVal.Text != "")
            {
                if (Txt_IssueType.Text == "3")
                {
                    IssueType = Convert.ToString(Txt_IssueTypeVal.Text);
                    string[] StrList = IssueType.Split('/');
                    if (StrList.Length >= 0)
                    {
                        for (i = 0; i < StrList.Length; i++)
                        {
                            string[] StrList1 = StrList[i].Split(';');
                            if (StrList1.Length > 0)
                            {
                                string[] StrList2 = StrList1[2].Split(',');
                                if (StrList2.Length >= 0)
                                {
                                    for (k = 0; k < StrList2.Length; k++)
                                    {

                                        strDate = StrList2[k] + "/" + StrList1[0] + "/" + dd_Subyr.Text;
                                        Lst_IssueList.Items.Add(strDate);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Txt_IssueType.Text == "2")
                {
                    c = 0;
                    IssueType = Convert.ToString(Txt_IssueTypeVal.Text);
                    string[] StrList = IssueType.Split('/');
                    if (StrList.Length >= 0)
                    {
                        for (i = 0; i < StrList.Length; i++)
                        {
                            string[] StrList1 = StrList[i].Split(';');
                            if (StrList1.Length > 0)
                            {
                                Lst_IssueList.Items.Add(StrList1[0]);
                                //Lst_IssueList.Items[c]=StrList1[1];
                                c = c + 1;
                            }
                        }
                    }

                }
                else if (Txt_IssueType.Text == "1")
                {
                    c = 0;
                    IssueType = Convert.ToString(Txt_IssueTypeVal.Text);
                    string[] StrList = IssueType.Split('/');

                    if (StrList.Length >= 0)
                    {
                        for (i = 0; i < StrList.Length; i++)
                        {
                            string[] StrList1 = StrList[i].Split(';');
                            if (StrList1.Length > 0)
                            {
                                Lst_IssueList.Items.Add(StrList1[0]);
                                //Lst_IssueList.ItemData[c] = StrList1[1];
                                c = c + 1;
                            }
                        }
                    }

                }
            }
            else if (Txt_IssueType.Text == "0")
            { }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }
    #endregion

    #region LnkChangeissuedate

    protected void LnkChangeissuedate_Click(object sender, EventArgs e)
    {
        popwindowChangeissuedate.Visible = true;
    }


    #region ChangeIssuePopup
    protected void btnSaveIssuedate_Click(object sender, EventArgs e)
    {
        try
        {

            if (txt_joucode.Text != "" && dd_Subyr.Text != "")
            {
                string issuedate = d2.GetFunction("SELECT count(*) FROM Journal J,Journal_Issues I WHERE J.Journal_Code = I.Journal_Code ANd J.Subs_Year = I.Subs_Year AND J.Issue_No = I.IssueNo AND J.Journal_Code ='" + txt_joucode.Text + "' AND J.Subs_Year ='" + dd_Subyr.Text + "' AND IssueDate >='" + DTP_UpdStartDate.Text + "' ");
                int issuedat = Convert.ToInt32(issuedate);
                if (issuedat > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Journals are received in this date";
                    return;
                }
                else
                {
                    Divissuerecord.Visible = true;
                    lbl_Divissuerecord.Text = "Are you sure to update";

                }

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }

    protected void btn_yes_issue_Click(object sender, EventArgs e)
    {
        Divissuerecord.Visible = false;
        //      ' *******************to Save Journal Issues Status*********************
        //' *********************************************************************
        int issueSave = 0;

        if (txt_joucode.Text != "" && dd_Subyr.Text != "")
        {
            Sql = "DELETE FROM Journal_Issues WHERE IssueDate >='" + DTP_UpdStartDate.Text + "' AND Journal_Code ='" + txt_joucode.Text + "' AND Subs_Year =" + dd_Subyr.Text + "";
            issueSave = d2.update_method_wo_parameter(Sql, "Text");
        }


        StrCDate = getdate(DTP_StartDate.Text.ToString());
        StartCount = 1;
        IntPrevMonth = 0;
        if (Txt_IssueBy.Text == "1")
        {
            IntCount = 1;
            IntMonthIssueNo = 1;
            IntPrevMonth = 1;
            while (StrCDate <= getdate(todate.Text.ToString()))
            {
                StrDay = Convert.ToString(StrCDate.DayOfWeek);
                string dateonly = Convert.ToString(StrCDate.Date);
                StrDayNum = Convert.ToInt32(dateonly);
                StrMonth = getmonth(StrCDate.Month.ToString());
                StrIssYear = StrCDate.Year;
                IntMonthNum = StrCDate.Month;
                strYear = dd_Subyr.Text;
                if (IntMonthIssueNo != IntPrevMonth)
                    IntMonthIssueNo = 1;

                for (i = 1; i < Convert.ToInt32(Txt_PerIssue.Text); i++)
                {
                    Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + StrDayNum + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + i + ",'" + StrCDate + "')";
                    insave = d2.update_method_wo_parameter(Sql, "Text");
                    IntCount = IntCount + 1;
                    IntMonthIssueNo = IntMonthIssueNo + 1;
                }
                StrCDate = StrCDate.AddDays(1);
                IntPrevMonth = IntMonthNum;
            }
        }
        else if (Txt_IssueBy.Text == "2")
        {
            DateTime update = Convert.ToDateTime(DTP_UpdStartDate.Text);
            StrMonth = Convert.ToString(update.Month);
            StrCount = d2.GetFunction("SELECT MAX(IssueNo) FROM Journal_Issues WHERE Journal_Code ='" + txt_joucode.Text + "' AND Subs_Year =" + dd_Subyr.Text + "");
            StrMonthIssNo = d2.GetFunction("SELECT MAX(MonthIssue_No) FROM Journal_Issues WHERE Journal_Code ='" + txt_joucode.Text + "' AND Subs_Year =" + dd_Subyr.Text + " AND IssueMonthNum =" + StrMonth + "");
            IntCount = Convert.ToInt32(StrCount) + 1;
            IntMonthIssueNo = Convert.ToInt32(StrMonthIssNo) + 1;
            IntPrevMonth = Convert.ToInt32(StrMonthIssNo) + 1;

            while (StrCDate <= getdate(todate.Text.ToString()))
            {
                StrDay = Convert.ToString(StrCDate.DayOfWeek);
                string dateonly = Convert.ToString(StrCDate.Date);
                StrDayNum = Convert.ToInt32(dateonly);
                StrMonth = getmonth(StrCDate.Month.ToString());
                StrIssYear = StrCDate.Year;
                IntMonthNum = StrCDate.Month;
                strYear = dd_Subyr.Text;
                if (IntMonthIssueNo != IntPrevMonth)
                    IntMonthIssueNo = 1;

                for (i = 0; i < Lst_IssueList.Items.Count - 1; i++)
                {
                    //if (Left(StrDay, 3) = Left(Lst_IssueList.List(i), 3))
                    //{
                    for (n = 1; n < Convert.ToInt32(Lst_IssueList.Items[i]); i++)
                    {
                        Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + StrDayNum + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + IntMonthIssueNo + ",'" + StrCDate + "')";
                        insave = d2.update_method_wo_parameter(Sql, "Text");
                        IntCount = IntCount + 1;
                        IntMonthIssueNo = IntMonthIssueNo + 1;
                    }
                    // }
                }
                StrCDate = StrCDate.AddDays(1);
                IntPrevMonth = IntMonthNum;
            }
        }
        else if (Txt_IssueBy.Text == "3")
        {
            IntCount = 1;
            IntMonthIssueNo = 1;
            IntPrevMonth = 1;
            while (StrCDate <= getdate(todate.Text.ToString()))
            {
                StrDay = Convert.ToString(StrCDate.DayOfWeek);
                string dateonly = Convert.ToString(StrCDate.Date);
                StrDayNum = Convert.ToInt32(dateonly);
                StrMonth = getmonth(StrCDate.Month.ToString());
                StrIssYear = StrCDate.Year;
                IntMonthNum = StrCDate.Month;
                strYear = dd_Subyr.Text;
                if (IntMonthIssueNo != IntPrevMonth)
                    IntMonthIssueNo = 1;
                for (i = 0; i < Lst_IssueList.Items.Count - 1; i++)
                {
                    if (StrDayNum == Convert.ToInt32(Lst_IssueList.Items[i]))
                    {
                        for (n = 1; n < Convert.ToInt32(Lst_IssueList.Items[i]); i++)
                        {
                            Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + StrDayNum + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + IntMonthIssueNo + ",'" + StrCDate + "')";
                            insave = d2.update_method_wo_parameter(Sql, "Text");
                            IntCount = IntCount + 1;
                            IntMonthIssueNo = IntMonthIssueNo + 1;
                        }
                    }
                }
                StrCDate = StrCDate.AddDays(1);
                IntPrevMonth = IntMonthNum;
            }
        }
        else if (Txt_IssueBy.Text == "4")
        {
            IntCount = 1;
            IntMonthIssueNo = 1;
            IntPrevMonth = 1;
            while (StrCDate <= getdate(todate.Text.ToString()))
            {
                StrDay = Convert.ToString(StrCDate.DayOfWeek);
                string dateonly = Convert.ToString(StrCDate.Date);
                StrDayNum = Convert.ToInt32(dateonly);
                StrMonth = getmonth(StrCDate.Month.ToString());
                StrIssYear = StrCDate.Year;
                IntMonthNum = StrCDate.Month;
                strYear = dd_Subyr.Text;
                if (IntMonthIssueNo != IntPrevMonth)
                    IntMonthIssueNo = 1;
                for (i = 0; i < Lst_IssueList.Items.Count - 1; i++)
                {
                    string IssueList = Convert.ToString(Lst_IssueList.Items[i]);
                    string[] StrListDate = IssueList.Split('/');
                    if (StrListDate.Length > 1)
                    {
                        StrListDate1 = Convert.ToDateTime(StrListDate[1] + "/" + StrListDate[0] + "/" + StrListDate[2]);
                        if (StrCDate == StrListDate1)
                        {
                            Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + StrDayNum + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + IntMonthIssueNo + ",'" + StrCDate + "')";
                            insave = d2.update_method_wo_parameter(Sql, "Text");
                            IntCount = IntCount + 1;
                            IntMonthIssueNo = IntMonthIssueNo + 1;
                        }
                    }
                }
                StrCDate = StrCDate.AddDays(1);
                IntPrevMonth = IntMonthNum;

            }
        }
        else if (Txt_IssueBy.Text == "5")
        {
            IntCount = 1;
            IntMonthIssueNo = 1;
            IntPrevMonth = 1;
            while (StrCDate <= getdate(todate.Text.ToString()))
            {
                StrDay = Convert.ToString(StrCDate.DayOfWeek);
                string dateonly = Convert.ToString(StrCDate.Date);
                StrDayNum = Convert.ToInt32(dateonly);
                StrMonth = getmonth(StrCDate.Month.ToString());
                StrIssYear = StrCDate.Year;
                IntMonthNum = StrCDate.Month;
                strYear = dd_Subyr.Text;
                if (IntMonthIssueNo != IntPrevMonth)
                    IntMonthIssueNo = 1;
                for (i = 1; i < Convert.ToInt32(Txt_PerIssue.Text); i++)
                {
                    Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + StrDayNum + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + i + ",'" + StrCDate + "')";
                    insave = d2.update_method_wo_parameter(Sql, "Text");
                    IntCount = IntCount + 1;
                    IntMonthIssueNo = IntMonthIssueNo + 1;
                }
                StrCDate = StrCDate.AddDays(1);
                IntPrevMonth = IntMonthNum;
            }
        }
        else
        {

        }

        DataSet rsBud = new DataSet();

        if (Cbo_Head.Text != "")
        {
            Sql = "select * from LibBudgetMaster WHERE Dept_Name ='" + Txt_Department.Text + "' AND Head_Code ='" + Cbo_Head.Text + "' and '" + Accdate + "' between Budget_From And Budget_To ";
            rsBud.Clear();
            rsBud = d2.select_method_wo_parameter(Sql, "Text");
            if (rsBud.Tables[0].Rows.Count > 0)
            {
                string JSpendAmt = Convert.ToString(rsBud.Tables[0].Rows[0]["JSpendAmt"]);
                string JBudAmt = Convert.ToString(rsBud.Tables[0].Rows[0]["JBudAmt"]);
                int JSAmt = Convert.ToInt32(JSpendAmt);
                int JBAmt = Convert.ToInt32(JBudAmt);
                if (JSAmt + Convert.ToInt32(Txt_SubsAmt.Text) > JBAmt)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Amount Exceeding Budget Amount";
                    return;
                }

            }
            Sql = "update LibBudgetMaster set JSpendAmt =JSpendAmt +" + Convert.ToInt32(Txt_SubsAmt.Text) + ",TotSpendAmt =TotSpendAmt+" + Convert.ToInt32(Txt_SubsAmt.Text) + ",JBalAmt = JBalAmt -" + Convert.ToInt32(Txt_SubsAmt.Text) + ",TotBalAmt = TotBalAmt - " + Convert.ToInt32(Txt_SubsAmt.Text) + " WHERE Dept_Name ='" + Txt_Department.Text + "' AND Head_Code =" + Cbo_Head.Text + "";
            insave = d2.update_method_wo_parameter(Sql, "Text");
        }
        if (insave > 0)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Record Updated Successfully";


        }

    }

    protected void bbtn_no_issue_Click(object sender, EventArgs e)
    {
        Divissuerecord.Visible = false;
        popwindowChangeissuedate.Visible = false;
    }

    protected void Buttonclose_Click(object sender, EventArgs e)
    {
        popwindowChangeissuedate.Visible = false;
    }
    #endregion


    #endregion

    #region Save
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            int IntPayMode = 0;
            IntPayMode = 1;
            CalDays();

            DataSet dssave = new DataSet();
            if (rbl_amttype.SelectedIndex == 0)
                IntPayMode = 1;
            else if (rbl_amttype.SelectedIndex == 1)
                IntPayMode = 2;
            else
                IntPayMode = 3;
            //AutoGen();

            Accdate = DateTime.Now.ToString("MM/dd/yyyy");
            Acctime = DateTime.Now.ToString("hh:mm tt");
            libcode = Convert.ToString(ddl_lib.SelectedValue);
            collcode = Convert.ToString(ddlCollege.SelectedValue);


            string subsdate = dtp_subsdate.Text;
            string[] subfromdate = subsdate.Split('/');
            if (subfromdate.Length == 3)
                subsdate = subfromdate[1].ToString() + "-" + subfromdate[0].ToString() + "-" + subfromdate[2].ToString();

            string dddate = dtp_dddate.Text;
            string[] fromdate1 = dddate.Split('/');
            if (fromdate1.Length == 3)
                dddate = fromdate1[1].ToString() + "-" + fromdate1[0].ToString() + "-" + fromdate1[2].ToString();

            string from_date = fromdate.Text;
            string[] fromdate2 = from_date.Split('/');
            if (fromdate2.Length == 3)
                from_date = fromdate2[1].ToString() + "-" + fromdate2[0].ToString() + "-" + fromdate2[2].ToString();

            string to_date = todate.Text;
            string[] todate3 = to_date.Split('/');
            if (todate3.Length == 3)
                to_date = todate3[1].ToString() + "-" + todate3[0].ToString() + "-" + todate3[2].ToString();


            string renewaldate = dtp_renewaldate.Text;
            string[] fromdate4 = renewaldate.Split('/');
            if (fromdate4.Length == 3)
                renewaldate = fromdate4[1].ToString() + "-" + fromdate4[0].ToString() + "-" + fromdate4[2].ToString();


            string StartDate = DTP_StartDate.Text;
            string[] fromdate5 = StartDate.Split('/');
            if (fromdate5.Length == 3)
                StartDate = fromdate5[1].ToString() + "-" + fromdate5[0].ToString() + "-" + fromdate5[2].ToString();

            Sql = "INSERT INTO subscription(sno,access_date,access_time,subs_code,subs_date,quo_code,dd_no,dd_date,fromdate,todate,renewal_date,infavourof,place,remarks,lib_code,ddamount,bankname,branch,journal_issue,journal_price,DD_Comm,inst_mem,sub_period,Active,Confirm_Received,Journal_Code,S_Term,Supplier_Code,Subscription_Year,SubsCost,Discount,Subscription_Price,TotIssues,StartDate,PayMode)values('" + Txtsno.Text + "','" + Convert.ToString(Accdate) + "','" + Convert.ToString(Acctime) + "','" + txt_subscode.Text + "','" + subsdate + "','" + txt_subsquocode.Text + "','" + txt_ddno.Text + "','" + dddate + "','" + from_date + "','" + to_date + "','" + renewaldate + "','" + txt_favourof.Text + "','" + Cbo_Place.Text + "','" + txt_remarks.Text + "','" + libcode + "','" + txt_ddamount.Text + "','" + Cbo_BankName.Text + "','" + Cbo_Branch.Text + "','" + txt_journalissues.Text + "','" + txt_journalprice.Text + "','0',1,1,1,1,'" + txt_joucode.Text + "','" + Txt_STerm.Text + "','" + txt_suppliercode.Text + "','" + dd_Subyr.Text + "','" + Txt_Cost.Text + "','" + Txt_Discount.Text + "','" + Txt_SubsAmt.Text + "','" + Txt_NoofIssues.Text + "','" + StartDate + "'," + IntPayMode + ")";
            insave = d2.update_method_wo_parameter(Sql, "Text");
            Sql = "update subscription set active = 0 where journal_code ='" + txt_joucode.Text + "' and s_term <> '" + Txt_STerm.Text + "'";
            insave = d2.update_method_wo_parameter(Sql, "Text");
            Sql = "update subs_quotation set access_date = '" + Accdate + "',access_time = '" + Acctime + "',subs_made = 'Yes' where quo_code = '" + txt_subsquocode.Text + "' and lib_code = '" + libcode + "'";
            insave = d2.update_method_wo_parameter(Sql, "Text");

            //' *******************to Save Journal Issues Status*********************
            //' *********************************************************************



            StrCDate = getdate(DTP_StartDate.Text.ToString());
            StartCount = 1;
            IntPrevMonth = 0;
            if (Txt_IssueBy.Text == "1")
            {
                IntCount = 1;
                IntMonthIssueNo = 1;
                IntPrevMonth = 1;
                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    StrDay = Convert.ToString(StrCDate.DayOfWeek);
                    string dateonly = Convert.ToString(StrCDate.Date);

                    StrMonth = getmonth(StrCDate.Month.ToString());
                    StrIssYear = StrCDate.Year;
                    IntMonthNum = StrCDate.Month;
                    strYear = dd_Subyr.Text;
                    if (IntMonthIssueNo != IntPrevMonth)
                        IntMonthIssueNo = 1;

                    for (i = 1; i < Convert.ToInt32(Txt_PerIssue.Text); i++)
                    {
                        Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate,) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + dateonly + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + i + ",'" + StrCDate + "')";
                        insave = d2.update_method_wo_parameter(Sql, "Text");
                        IntCount = IntCount + 1;
                        IntMonthIssueNo = IntMonthIssueNo + 1;
                    }
                    StrCDate = StrCDate.AddDays(1);
                    IntPrevMonth = IntMonthNum;
                }
            }
            else if (Txt_IssueBy.Text == "2")
            {
                IntCount = 1;
                IntMonthIssueNo = 1;
                IntPrevMonth = 1;

                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    StrDay = Convert.ToString(StrCDate.DayOfWeek);
                    string dateonly = Convert.ToString(StrCDate.Date);
                    int.TryParse(dateonly, out StrDayNum);

                    StrIssYear = StrCDate.Year;
                    IntMonthNum = StrCDate.Month;
                    strYear = dd_Subyr.Text;
                    if (IntMonthIssueNo != IntPrevMonth)
                        IntMonthIssueNo = 1;

                    for (i = 0; i < Lst_IssueList.Items.Count - 1; i++)
                    {
                        //if (Left(StrDay, 3) = Left(Lst_IssueList.List(i), 3) )
                        //{
                        for (n = 1; n < Convert.ToInt32(Lst_IssueList.Items[i]); i++)
                        {
                            Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + dateonly + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + IntMonthIssueNo + ",'" + StrCDate + "')";
                            insave = d2.update_method_wo_parameter(Sql, "Text");
                            IntCount = IntCount + 1;
                            IntMonthIssueNo = IntMonthIssueNo + 1;
                        }
                        //}
                    }
                    StrCDate = StrCDate.AddDays(1);
                    IntPrevMonth = IntMonthNum;
                }
            }
            else if (Txt_IssueBy.Text == "3")
            {
                IntCount = 1;
                IntMonthIssueNo = 1;
                IntPrevMonth = 1;
                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    StrDay = Convert.ToString(StrCDate.DayOfWeek);
                    string dateonly = Convert.ToString(StrCDate.Date);

                    StrMonth = getmonth(StrCDate.Month.ToString());
                    StrIssYear = StrCDate.Year;
                    IntMonthNum = StrCDate.Month;
                    strYear = dd_Subyr.Text;
                    if (IntMonthIssueNo != IntPrevMonth)
                        IntMonthIssueNo = 1;
                    for (i = 0; i < Lst_IssueList.Items.Count - 1; i++)
                    {
                        if (StrDayNum == Convert.ToInt32(Lst_IssueList.Items[i]))
                        {
                            for (n = 1; n < Convert.ToInt32(Lst_IssueList.Items[i]); i++)
                            {
                                Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + dateonly + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + IntMonthIssueNo + ",'" + StrCDate + "')";
                                insave = d2.update_method_wo_parameter(Sql, "Text");
                                IntCount = IntCount + 1;
                                IntMonthIssueNo = IntMonthIssueNo + 1;
                            }
                        }
                    }
                    StrCDate = StrCDate.AddDays(1);
                    IntPrevMonth = IntMonthNum;
                }
            }
            else if (Txt_IssueBy.Text == "4")
            {
                IntCount = 1;
                IntMonthIssueNo = 1;
                IntPrevMonth = 1;
                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    StrDay = Convert.ToString(StrCDate.DayOfWeek);
                    string dateonly = Convert.ToString(StrCDate.Date);

                    StrMonth = getmonth(StrCDate.Month.ToString());
                    StrIssYear = StrCDate.Year;
                    IntMonthNum = StrCDate.Month;
                    strYear = dd_Subyr.Text;
                    if (IntMonthIssueNo != IntPrevMonth)
                        IntMonthIssueNo = 1;
                    for (i = 0; i < Lst_IssueList.Items.Count - 1; i++)
                    {
                        string IssueList = Convert.ToString(Lst_IssueList.Items[i]);
                        string[] StrListDate = IssueList.Split('/');
                        if (StrListDate.Length > 1)
                        {
                            StrListDate1 = Convert.ToDateTime(StrListDate[1] + "/" + StrListDate[0] + "/" + StrListDate[2]);
                            if (StrCDate == StrListDate1)
                            {
                                Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + dateonly + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + IntMonthIssueNo + ",'" + StrCDate + "')";
                                insave = d2.update_method_wo_parameter(Sql, "Text");
                                IntCount = IntCount + 1;
                                IntMonthIssueNo = IntMonthIssueNo + 1;
                            }
                        }
                    }
                    StrCDate = StrCDate.AddDays(1);
                    IntPrevMonth = IntMonthNum;

                }
            }
            else if (Txt_IssueBy.Text == "5")
            {
                IntCount = 1;
                IntMonthIssueNo = 1;
                IntPrevMonth = 1;
                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    StrDay = Convert.ToString(StrCDate.DayOfWeek);
                    string dateonly = Convert.ToString(StrCDate.Day);

                    StrMonth = getmonth(StrCDate.Month.ToString());
                    StrIssYear = StrCDate.Year;
                    IntMonthNum = StrCDate.Month;
                    strYear = dd_Subyr.Text;
                    if (IntMonthIssueNo != IntPrevMonth)
                        IntMonthIssueNo = 1;
                    for (i = 1; i < Convert.ToInt32(Txt_PerIssue.Text); i++)
                    {
                        Sql = "INSERT INTO Journal_Issues(Journal_Code,Subs_Year,IssueDay,IssueDayNum,IssueYear,IssueMonth,IssueMonthNum,MonthIssue_No,IssueNo,Issue_Status,Lib_Code,College_Code,DayIssue_No,IssueDate) VALUES('" + txt_joucode.Text + "'," + strYear + ",'" + StrDay + "'," + dateonly + "," + StrIssYear + ",'" + StrMonth + "'," + IntMonthNum + "," + IntMonthIssueNo + "," + IntCount + "," + "0,'" + libcode + "'," + collcode + "," + i + ",'" + StrCDate + "')";
                        insave = d2.update_method_wo_parameter(Sql, "Text");
                        IntCount = IntCount + 1;
                        IntMonthIssueNo = IntMonthIssueNo + 1;
                    }
                    StrCDate = StrCDate.AddDays(1);
                    IntPrevMonth = IntMonthNum;
                }
            }
            else
            {

            }

            DataSet rsBud = new DataSet();
            string budjet = string.Empty;
            if (Cbo_Head.Items.Count > 0)
                budjet = Convert.ToString(Cbo_Head.SelectedValue);
            if (Convert.ToString(Cbo_Head.SelectedItem) != "")
            {
                Sql = "select * from LibBudgetMaster WHERE Dept_Name ='" + Txt_Department.Text + "' AND Head_Code ='" + budjet + "' and '" + Accdate + "' between Budget_From And Budget_To ";
                rsBud.Clear();
                rsBud = d2.select_method_wo_parameter(Sql, "Text");
                if (rsBud.Tables[0].Rows.Count > 0)
                {
                    string JSpendAmt = Convert.ToString(rsBud.Tables[0].Rows[0]["JSpendAmt"]);
                    string JBudAmt = Convert.ToString(rsBud.Tables[0].Rows[0]["JBudAmt"]);
                    int JSAmt = Convert.ToInt32(JSpendAmt);
                    int JBAmt = Convert.ToInt32(JBudAmt);
                    if (JSAmt + Convert.ToInt32(Txt_SubsAmt.Text) > JBAmt)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Amount Exceeding Budget Amount";
                        return;
                    }

                }
                Sql = "update LibBudgetMaster set JSpendAmt =JSpendAmt +" + Convert.ToInt32(Txt_SubsAmt.Text) + ",TotSpendAmt =TotSpendAmt+" + Convert.ToInt32(Txt_SubsAmt.Text) + ",JBalAmt = JBalAmt -" + Convert.ToInt32(Txt_SubsAmt.Text) + ",TotBalAmt = TotBalAmt - " + Convert.ToInt32(Txt_SubsAmt.Text) + " WHERE Dept_Name ='" + Txt_Department.Text + "' AND Head_Code =" + Cbo_Head.Text + "";
                insave = d2.update_method_wo_parameter(Sql, "Text");
            }

            alertpopwindow.Visible = true;
            lblalerterr.Text = "Record Saved Successfully";
            Clear();


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
    }

    public void CalDays()
    {
        try
        {
            int IntCount = 0;
            string StrDay = "";
            int StrDayNum = 0;
            string StrMonth = "";
            IntCount = 0;
            int i = 0;
            StrCDate = getdate(DTP_StartDate.Text.ToString());
            if (Txt_IssueBy.Text == "1")
            {
                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    for (i = 1; i < Convert.ToInt32(Txt_PerIssue.Text); i++)
                    {
                        IntCount = IntCount + 1;
                    }
                    StrCDate = StrCDate.AddDays(1);
                }
            }
            else if (Txt_IssueBy.Text == "2")
            {
                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    StrDay = Convert.ToString(StrCDate.Day);
                    for (i = 0; i < Lst_IssueList.Items.Count - 1; i++)
                    {
                        //if (Left(StrDay, 3) == Left(Lst_IssueList.List(i), 3))
                        //    IntCount = IntCount + 1;
                    }

                    StrCDate = StrCDate.AddDays(1);
                }
            }
            else if (Txt_IssueBy.Text == "3")
            {
                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    StrDayNum = Convert.ToInt32(StrCDate.Date);
                    for (i = 0; i < Lst_IssueList.Items.Count - 1; i++)
                    {
                        if (StrDayNum == Convert.ToInt32(Lst_IssueList.Items[i]))
                            IntCount = IntCount + 1;

                    }
                    StrCDate = StrCDate.AddDays(1);
                }
            }
            else if (Txt_IssueBy.Text == "4")
            {
                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    StrMonth = Convert.ToString(StrCDate.Month);
                    for (i = 0; i < Lst_IssueList.Items.Count - 1; i++)
                    {
                        if (StrCDate == Convert.ToDateTime(Lst_IssueList.Items[i]))
                            IntCount = IntCount + 1;

                    }
                    StrCDate = StrCDate.AddDays(1);
                }
            }
            else if (Txt_IssueBy.Text == "5")
            {
                while (StrCDate <= getdate(todate.Text.ToString()))
                {
                    for (i = 1; i < Convert.ToInt32(Txt_PerIssue.Text); i++)
                    {
                        IntCount = IntCount + 1;
                    }
                    StrCDate = StrCDate.AddDays(1);
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Update issue information in journal master";
                //return;
            }
            if (IntCount == 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Update issue information in journal master";
                // return;

            }
            Txt_NoofIssues.Text = Convert.ToString(IntCount);

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }

    public void AutoGen()
    {
        int codeno = 0;
        DataSet rs4 = new DataSet();
        libcode = Convert.ToString(ddllibrary.SelectedValue);
        Sql = "   select top 1 CAST(RIGHT(sm.subs_code, LEN(sm.subs_code) - PATINDEX('%[0-9]%', sm.subs_code)+1) AS INT) as subs_code FROM subscription sm where lib_code = '" + libcode + "' order by CAST(RIGHT(sm.subs_code, LEN(sm.subs_code) - PATINDEX('%[0-9]%', sm.subs_code)+1) AS INT) desc";
        rs4.Clear();
        rs4 = d2.select_method_wo_parameter(Sql, "Text");
        if (rs4.Tables[0].Rows.Count > 0)
        {
            string subs_code = Convert.ToString(rs4.Tables[0].Rows[0]["subs_code"]);
            //subs_code = subs_code.Remove(0, 3);
            codeno = Convert.ToInt32(subs_code) + 1;
            txt_subscode.Text = "SUB" + Convert.ToString(codeno);
        }
        else
            txt_subscode.Text = "SUB1";

    }
    private DateTime getdate(string getspl) //dd/MMM/yyyy hh:mm tt
    {
        DateTime date = new DateTime();
        try
        {
            date = DateTime.ParseExact(getspl, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return date;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }
        return new DateTime();
    }
    #endregion

    #region Update
    protected void btn_update_Click(object sender, EventArgs e)
    {
        update();
    }

    public void update()
    {

        try
        {

            int update = 0;

            if (rbl_amttype.SelectedIndex == 0)
                IntPayMode = 1;
            else if (rbl_amttype.SelectedIndex == 1)
                IntPayMode = 2;
            else
                IntPayMode = 3;

            libcode = Convert.ToString(ddllibrary.SelectedValue);
            string subsdate = dtp_subsdate.Text;
            string[] subfromdate = subsdate.Split('/');
            if (subfromdate.Length == 3)
                subsdate = subfromdate[1].ToString() + "-" + subfromdate[0].ToString() + "-" + subfromdate[2].ToString();

            string dddate = dtp_dddate.Text;
            string[] fromdate1 = dddate.Split('/');
            if (fromdate1.Length == 3)
                dddate = fromdate1[1].ToString() + "-" + fromdate1[0].ToString() + "-" + fromdate1[2].ToString();

            string from_date = fromdate.Text;
            string[] fromdate2 = from_date.Split('/');
            if (fromdate2.Length == 3)
                from_date = fromdate2[1].ToString() + "-" + fromdate2[0].ToString() + "-" + fromdate2[2].ToString();

            string to_date = todate.Text;
            string[] todate3 = to_date.Split('/');
            if (todate3.Length == 3)
                to_date = todate3[1].ToString() + "-" + todate3[0].ToString() + "-" + todate3[2].ToString();


            string renewaldate = dtp_renewaldate.Text;
            string[] fromdate4 = renewaldate.Split('/');
            if (fromdate4.Length == 3)
                renewaldate = fromdate4[1].ToString() + "-" + fromdate4[0].ToString() + "-" + fromdate4[2].ToString();


            string StartDate = DTP_StartDate.Text;
            string[] fromdate5 = StartDate.Split('/');
            if (fromdate5.Length == 3)
                StartDate = fromdate5[1].ToString() + "-" + fromdate5[0].ToString() + "-" + fromdate5[2].ToString();

            double subssmount = Convert.ToDouble(Txt_SubsAmt.Text);
            // double discount = Convert.ToDouble(Txt_Discount.Text);

            Accdate = DateTime.Now.ToString("yyyy-MM-dd");
            Acctime = DateTime.Now.ToString("hh:mm tt");
            //libcode = Convert.ToString(ddl_lib.SelectedValue);
            //collcode = Convert.ToString(ddlCollege.SelectedValue);

            Sql = "if exists(select subs_code from subscription where subs_code = '" + txt_subscode.Text + "'  and lib_code = '" + libcode + "') update subscription set  todate = '" + to_date + "', fromdate = '" + from_date + "',access_date = '" + Accdate + "',access_time = '" + Acctime + "',subs_date = '" + subsdate + "',quo_code = '" + txt_subsquocode.Text + "', dd_no = '" + txt_ddno.Text + "',dd_date = '" + dddate + "',renewal_date = '" + renewaldate + "',infavourof = '" + txt_favourof.Text + "',place = '" + Cbo_Place.Text + "',remarks = '" + txt_remarks.Text + "',ddamount='" + txt_ddamount.Text + "',bankname='" + Cbo_BankName.Text + "',branch='" + Cbo_Branch.Text + "',journal_issue='" + txt_journalissues.Text + "',journal_price='" + txt_journalprice.Text + "',Supplier_Code ='" + txt_suppliercode.Text + "',Subscription_Year='" + dd_Subyr.Text + "',SubsCost ='" + Txt_Cost.Text + "',Discount ='" + Txt_Discount.Text + "',Subscription_Price='" + subssmount + "',TotIssues='" + Txt_NoofIssues.Text + "',StartDate ='" + StartDate + "',PayMode ='" + IntPayMode + "' where subs_code = '" + txt_subscode.Text + "' and lib_code = '" + libcode + "' else INSERT INTO subscription(sno,access_date,access_time,subs_code,subs_date,quo_code,dd_no,dd_date,fromdate,todate,renewal_date,infavourof,place,remarks,lib_code,ddamount,bankname,branch,journal_issue,journal_price,DD_Comm,inst_mem,sub_period,Active,Confirm_Received,Journal_Code,S_Term,Supplier_Code,Subscription_Year,SubsCost,Discount,Subscription_Price,TotIssues,StartDate,PayMode)values('" + Txtsno.Text + "','" + Convert.ToString(Accdate) + "','" + Convert.ToString(Acctime) + "','" + txt_subscode.Text + "','" + subsdate + "','" + txt_subsquocode.Text + "','" + txt_ddno.Text + "','" + dddate + "','" + from_date + "','" + to_date + "','" + renewaldate + "','" + txt_favourof.Text + "','" + Cbo_Place.Text + "','" + txt_remarks.Text + "','" + libcode + "','" + txt_ddamount.Text + "','" + Cbo_BankName.Text + "','" + Cbo_Branch.Text + "','" + txt_journalissues.Text + "','" + txt_journalprice.Text + "','0',1,1,1,1,'" + txt_joucode.Text + "','" + Txt_STerm.Text + "','" + txt_suppliercode.Text + "','" + dd_Subyr.Text + "','" + Txt_Cost.Text + "','" + Txt_Discount.Text + "','" + Txt_SubsAmt.Text + "','" + Txt_NoofIssues.Text + "','" + StartDate + "'," + IntPayMode + ")";
            update = d2.update_method_wo_parameter(Sql, "Text");
            if (update > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Subscription Details Updated Successfully";
            }


            Clear();
            AutoGen();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }
    #endregion

    #region Delete
    protected void btn_Delete_Click(object sender, EventArgs e)
    {
        Delete();
    }

    public void Delete()
    {
        try
        {

            DataSet rs2 = new DataSet();
            libcode = Convert.ToString(ddl_lib.SelectedValue);
            collcode = Convert.ToString(ddlCollege.SelectedValue);
            Sql = "SELECT * FROM Journal WHERE Journal_Code ='" + txt_joucode.Text + "' AND Subs_Year =" + dd_Subyr.Text + " AND Lib_Code ='" + libcode + "' ";
            rs2.Clear();
            rs2 = d2.select_method_wo_parameter(Sql, "Text");
            if (rs2.Tables[0].Rows.Count > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Can't delete, some journals are received";
                return;
            }
            else
            {
                surediv.Visible = true;
                lbl_sure.Text = "Are You Sure to Delete this Subscription";

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }


    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {

            int delete = 0;
            libcode = Convert.ToString(ddl_lib.SelectedValue);
            collcode = Convert.ToString(ddlCollege.SelectedValue);
            Sql = "delete from subscription where subs_code = '" + txt_subscode.Text + "' and lib_code = '" + libcode + "'";
            delete = d2.update_method_wo_parameter(Sql, "Text");
            Sql = "DELETE FROM Journal_Issues WHERE Journal_Code ='" + txt_joucode.Text + "' AND Subs_Year =" + dd_Subyr.Text + " AND Lib_Code ='" + libcode + "' AND College_Code =" + collcode + "";
            delete = d2.update_method_wo_parameter(Sql, "Text");
            Sql = "update subs_quotation set subs_made = 'No' where quo_code = '" + txt_subsquocode.Text + "' and lib_code = '" + libcode + "'";
            delete = d2.update_method_wo_parameter(Sql, "Text");
            //if (delete > 0)
            //{
            surediv.Visible = false;
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Deleted SuccessFully";
            // }
            Clear();
            AutoGen();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }


    }


    #endregion

    #region Close
    protected void btn_popclose_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
    }

    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popupselectjournal_Code.Visible = false;
    }

    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        DivSuppliername.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    #endregion

    #region Common_Method

    public void Clear()
    {

        Txt_STerm.Text = "";
        //txt_subscode.Text = "";
        txt_joucode.Text = "";
        txt_title.Text = "";
        txt_suppliercode.Text = "";
        txt_supp_name.Text = "";
        Textadd.Text = "";
        Textemail.Text = "";
        TextWebsite.Text = "";
        Txt_NoofIssues.Text = "";
        txt_journalissues.Text = "";
        txt_journalprice.Text = "";
        // Txt_Cost.Text = "";
        //Txt_Discount.Text = "";
        Txt_SubsAmt.Text = "";
        txt_remarks.Text = "";
        txt_ddno.Text = "";
        txt_ddamount.Text = "";
        txt_favourof.Text = "";
        txt_subsquocode.Text = "";
        Txt_IssueBy.Text = "";
        Txt_PerIssue.Text = "";
        Txt_TotIssue.Text = "";
        Txt_IssueType.Text = "";
        Lst_IssueList.Items.Clear();
        Txtsno.Text = "";
    }

    public string getmonth(string mon)
    {
        string month = "";
        try
        {

            if (mon == "1")
                month = "January";
            else if (mon == "2")
                month = "February";
            else if (mon == "3")
                month = "March";
            else if (mon == "4")
                month = "April";
            else if (mon == "5")
                month = "May";
            else if (mon == "6")
                month = "June";
            else if (mon == "7")
                month = "July";
            else if (mon == "8")
                month = "August";
            else if (mon == "9")
                month = "September";
            else if (mon == "10")
                month = "October";
            else if (mon == "11")
                month = "November";
            else
                month = "December";

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Subscribe"); }

        return month;


    }

    #endregion

    public void todate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string dt = string.Empty;
            string date = Convert.ToString(todate.Text);
            string[] dtdate = date.Split('/');
            if (dtdate.Length == 3)
                date = dtdate[1].ToString() + "/" + dtdate[0].ToString() + "/" + dtdate[2].ToString();
            dt = Convert.ToDateTime(date).AddDays(1).ToString();
            DateTime datedval = Convert.ToDateTime(dt);
            dtp_renewaldate.Text = datedval.ToString("dd/MM/yyyy");
        }
        catch (Exception ex) { }
    }

    public void Txt_Cost_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            double cost = Convert.ToDouble(Txt_Cost.Text);
            Txt_SubsAmt.Text =Convert.ToString(cost);
        }
        catch (Exception ex) { }
    }

    protected void Txt_Discount_OnTextChanged(object sender, EventArgs e)
    {
        double CurVal = Convert.ToDouble(Txt_Cost.Text);
        double discount = Convert.ToDouble(Txt_Discount.Text);
        double price = CurVal - (CurVal * (discount / 100));
        Txt_SubsAmt.Text = Convert.ToString(price);
    }

}